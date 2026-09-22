using System.Text;
using System.Text.RegularExpressions;

namespace WYNLAB.SYS;

/// <summary>결과 - 실제로 만들어진 파일 경로 목록(리뷰/안내용).</summary>
public class ScreenGenResult
{
    public List<string> CreatedFiles { get; } = new();
}

/// <summary>AI Builder의 코드 생성 엔진 - ScreenGenSpec을 받아 frmXxx.cs/.Designer.cs,
/// csproj 항목, 메뉴등록 마이그레이션 파일을 실제로 디스크에 쓴다. 순수 문자열 조립이라
/// UI(frmAIBuilder)와 완전히 분리해뒀다 - 나중에 템플릿 종류가 늘어나도 여기만 손대면 된다.
///
/// 생성 패턴은 오늘 손으로 만든 frmMenuAuth.cs/.Designer.cs와 완전히 동일하다(PanelWyn/
/// SplitterWyn/GridControlWyn/GridViewWyn, BeginInit/EndInit, Fill 먼저 추가하고 Top/Left는
/// 나중에 추가해야 바깥쪽을 차지하는 Dock 순서 규칙 등). 그리드는 TEMPLATE 화면처럼 DataTable
/// 바인딩이라 DTO를 만들지 않는다 - GridColumn.FieldName이 곧 실제 DB 컬럼명이다.
///
/// MasterFormSubGrid의 grd1(마스터목록)은 항상 조회전용으로 생성한다 - grd2/grd3(하위그리드)는
/// 편집 가능하고 각자 저장프로시저를 지정할 수 있다(2026-09-08, 예전 MasterFormTabGrid 통합).</summary>
public static class ScreenTemplateGenerator
{
    public static ScreenGenResult Generate(ScreenGenSpec spec, string repoRoot)
    {
        var screenDir = Path.Combine(repoRoot, "99.SOURCE", spec.Module, $"WYNLAB.{spec.Module}", spec.ScreenClassNm);
        var csPath = Path.Combine(screenDir, $"{spec.ScreenClassNm}.cs");
        var designerPath = Path.Combine(screenDir, $"{spec.ScreenClassNm}.Designer.cs");
        var csprojPath = Path.Combine(repoRoot, "99.SOURCE", spec.Module, $"WYNLAB.{spec.Module}", $"WYNLAB.{spec.Module}.csproj");

        if (Directory.Exists(screenDir) && Directory.GetFiles(screenDir).Length > 0)
            throw new InvalidOperationException($"이미 존재하는 화면 폴더입니다 - 덮어쓰지 않습니다: {screenDir}");
        if (!File.Exists(csprojPath))
            throw new InvalidOperationException($"모듈 프로젝트 파일을 찾을 수 없습니다: {csprojPath}");

        Directory.CreateDirectory(screenDir);

        var (designerCs, formCs) = spec.Kind switch
        {
            TemplateKind.SingleGrid => GenerateSingleGridFromTemplate(spec, repoRoot),
            TemplateKind.MasterSubGrid => GenerateMasterSubGridFromTemplate(spec, repoRoot),
            TemplateKind.MasterFormSubGrid => GenerateMasterFormSubGridFromTemplate(spec, repoRoot),
            TemplateKind.MasterOneSheet => GenerateMasterOneSheetFromTemplate(spec, repoRoot),
            TemplateKind.TreeMasterSubGrid => GenerateTreeMasterSubGridFromTemplate(spec, repoRoot),
            _ => throw new InvalidOperationException($"알 수 없는 템플릿: {spec.Kind}")
        };
        File.WriteAllText(designerPath, designerCs);
        File.WriteAllText(csPath, formCs);
        var hasResx = CopyTemplateResxIfExists(repoRoot, spec, screenDir);
        AppendCsprojEntry(csprojPath, spec, hasResx);

        var migrationPath = NextMigrationPath(repoRoot, spec.ScreenClassNm);
        File.WriteAllText(migrationPath, BuildMenuSql(spec));

        var result = new ScreenGenResult();
        result.CreatedFiles.Add(designerPath);
        result.CreatedFiles.Add(csPath);
        if (hasResx) result.CreatedFiles.Add(Path.Combine(screenDir, $"{spec.ScreenClassNm}.resx"));
        result.CreatedFiles.Add(migrationPath);
        result.CreatedFiles.Add(csprojPath + " (수정됨)");
        return result;
    }

    // ================================================================================
    // 4개 템플릿이 전부 spec.QuerySources/SaveActions 하나의 모델만 읽는다(2026-09-04 통합) -
    // 템플릿마다 "그리드/폼 몇 개, 저장 몇 번"만 다를 뿐 컬럼/파라미터를 얻어오는 방식은 같다.
    // TargetSlot/SourceSlot으로 필요한 걸 찾아오는 두 헬퍼만 있으면 된다.
    // ================================================================================

    /// <summary>QuerySources 중 TargetSlot이 일치하는 레코드셋의 컬럼목록. 여러 QuerySource에
    /// 걸쳐 찾는다(같은 슬롯을 어느 프로시저/워크타입이 채우는지는 호출부가 알 필요 없음).</summary>
    private static List<ColumnSpec> ColumnsForSlot(ScreenGenSpec spec, string targetSlot)
    {
        foreach (var qs in spec.QuerySources)
        {
            var binding = qs.ResultSetBindings.FirstOrDefault(b => b.TargetSlot == targetSlot);
            if (binding != null) return binding.Columns;
        }
        throw new InvalidOperationException($"QuerySources에 {targetSlot}을 채우는 조회가 없습니다(Query Sources 탭에서 Describe Query를 먼저 실행하세요).");
    }

    private static SaveAction SaveActionForSlot(ScreenGenSpec spec, SaveActionScope scope, string sourceSlot) =>
        SaveActionForSlotOrNull(spec, scope, sourceSlot)
            ?? throw new InvalidOperationException($"SaveActions에 {sourceSlot}({scope}) 저장 항목이 없습니다(Save Actions 탭에서 Describe Save를 먼저 실행하세요).");

    /// <summary>MasterSubGrid처럼 저장 자체가 선택사항(Save Proc을 안 채우면 "조회전용"으로
    /// 생성됨)인 템플릿에서 쓴다 - 없으면 null.</summary>
    private static SaveAction? SaveActionForSlotOrNull(ScreenGenSpec spec, SaveActionScope scope, string sourceSlot) =>
        spec.SaveActions.FirstOrDefault(a => a.Scope == scope && a.SourceSlot == sourceSlot);

    // ================================================================================
    // 싱글그리드 = 템플릿 복제+치환 (WYNLAB.TEMPLATE/TplSingleGrid.cs/.Designer.cs가 원본).
    // ================================================================================
    private static (string DesignerCs, string FormCs) GenerateSingleGridFromTemplate(ScreenGenSpec spec, string repoRoot)
    {
        var templateDir = Path.Combine(repoRoot, "99.SOURCE", "TEMPLATE", "WYNLAB.TEMPLATE");
        var designerPath = Path.Combine(templateDir, "TplSingleGrid.Designer.cs");
        var formPath = Path.Combine(templateDir, "TplSingleGrid.cs");
        if (!File.Exists(designerPath) || !File.Exists(formPath))
            throw new InvalidOperationException($"싱글그리드 템플릿을 찾을 수 없습니다: {templateDir}");

        var designerCs = File.ReadAllText(designerPath);
        var formCs = File.ReadAllText(formPath);

        var masterColumns = ColumnsForSlot(spec, "grd1");
        // grd1 저장은 선택사항이다(MasterSubGrid와 같은 원칙, 2026-09-07) - Save Actions 탭에
        // grd1 행을 안 만들면 조회전용 화면으로 생성된다(SaveClick의 __SAVE_PROC__ 빈 문자열
        // 체크, TplSingleGrid.cs 참고 - 템플릿 쪽 가드는 이미 있었고 여기 하드 요구만 막혀있었음).
        var saveAction = SaveActionForSlotOrNull(spec, SaveActionScope.Detail, "grd1");

        // ---- 검색조건(panHeader) ----
        var (searchNew, searchConfig, searchDecl) = BuildSearchFieldBlocks(spec);

        // ---- 그리드 컬럼(grd1) - 기존 헬퍼(CHECK/COMBO/NUMBER/DTE 처리 포함)를 그대로 재사용 ----
        var gridNew = new StringBuilder();
        foreach (var c in masterColumns.Where(c => c.IncludeInGrid))
            gridNew.AppendLine($"        this.col1{PascalCase(c.Name)} = new DevExpress.XtraGrid.Columns.GridColumn();");
        AppendCheckEditFieldIfNeeded(gridNew, masterColumns, "chkEdit1");
        AppendSpinEditFieldIfNeeded(gridNew, masterColumns, "spinEditcol1");
        AppendDateEditFieldIfNeeded(gridNew, masterColumns, "dateEditcol1");
        // SingleGrid의 grd1은 항상 Role=Edit(TplSingleGrid.cs)라 POP(팝업버튼)도 실제로 동작한다.
        const bool allowPopupInGrid = true;
        AppendComboEditFieldsDecl(gridNew, masterColumns, "col1");
        AppendPopupEditFieldsDecl(gridNew, masterColumns, "col1", allowPopupInGrid);

        var gridConfig = new StringBuilder();
        AppendGridColumns(gridConfig, masterColumns, "col1", "chkEdit1", allowPopupInGrid);
        gridConfig.AppendLine("        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {");
        gridConfig.AppendLine("            " + string.Join(",\r\n            ",
            masterColumns.Where(c => c.IncludeInGrid).Select(c => $"this.col1{PascalCase(c.Name)}")) + "});");
        AppendRepositoryItemsAddRange(gridConfig, "grd1", masterColumns, "chkEdit1", "col1", allowPopupInGrid);

        var gridDecl = new StringBuilder();
        foreach (var c in masterColumns.Where(c => c.IncludeInGrid))
            gridDecl.AppendLine($"    private DevExpress.XtraGrid.Columns.GridColumn col1{PascalCase(c.Name)};");
        if (HasCheckColumn(masterColumns))
            gridDecl.AppendLine("    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkEdit1;");
        if (HasNumberColumn(masterColumns))
            gridDecl.AppendLine("    private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spinEditcol1;");
        if (HasDateColumn(masterColumns))
            gridDecl.AppendLine("    private WYNLAB.Base.Controls.DateColumnEdit dateEditcol1;");
        foreach (var c in ComboColumns(masterColumns))
            gridDecl.AppendLine($"    private LookUpColumnEdit {ComboEditField("col1", c)};");
        foreach (var c in PopColumns(masterColumns, allowPopupInGrid))
            gridDecl.AppendLine($"    private PopupLookupColumnEdit {PopEditField("col1", c)};");

        var generatedHeader = $"// AI Builder가 싱글그리드 템플릿을 복제해서 자동 생성 - {DateTime.Now:yyyy-MM-dd}.\r\n" +
            "// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.\r\n";
        designerCs = SpliceBlock(designerCs, "FILE_HEADER", generatedHeader);
        formCs = SpliceBlock(formCs, "FILE_HEADER", generatedHeader);

        designerCs = SpliceBlock(designerCs, "SEARCH_FIELD_NEW", searchNew.ToString());
        designerCs = SpliceBlock(designerCs, "SEARCH_FIELD_CONFIG", searchConfig.ToString());
        designerCs = SpliceBlock(designerCs, "SEARCH_FIELD_DECL", searchDecl.ToString());
        designerCs = SpliceBlock(designerCs, "GRID_COLUMN_NEW", gridNew.ToString());
        designerCs = SpliceBlock(designerCs, "GRID_COLUMN_CONFIG", gridConfig.ToString());
        designerCs = SpliceBlock(designerCs, "GRID_COLUMN_DECL", gridDecl.ToString());

        // ---- .cs: QueryClick/SaveClick 파라미터 ----
        var saveParams = new StringBuilder();
        if (saveAction != null)
        {
            foreach (var kvp in saveAction.SaveParamColumnMap)
                saveParams.AppendLine($"                [\"p_{kvp.Key}\"] = ProcData.Str(row, \"{kvp.Value}\", version),");
            foreach (var unmapped in saveAction.UnmappedSaveParams())
                saveParams.AppendLine($"                [\"p_{unmapped}\"] = null, // TODO: 값 채우기");
        }

        formCs = SpliceBlock(formCs, "QUERY_PARAMS", BuildQueryParamsBlock(spec));
        formCs = SpliceBlock(formCs, "SAVE_PARAMS", saveParams.ToString());

        // ---- 토큰 치환 + 클래스명/네임스페이스 ----
        designerCs = ApplyTokens(designerCs, spec, "TplSingleGrid");
        formCs = ApplyTokens(formCs, spec, "TplSingleGrid");

        return (designerCs, formCs);
    }

    // ================================================================================
    // 마스터-서브그리드 = 템플릿 복제+치환 (WYNLAB.TEMPLATE/TplMasterSubGrid).
    // grd1/grd2 컬럼은 이번 1단계에선 TEXT 전용(체크박스/LookUp 지원 안 함) - 필요하면 생성 후
    // 직접 추가한다(싱글그리드처럼 컬럼마다 리포지토리 아이템을 붙이는 건 추후 확장 예정).
    // ================================================================================
    private static (string DesignerCs, string FormCs) GenerateMasterSubGridFromTemplate(ScreenGenSpec spec, string repoRoot)
    {
        var templateDir = Path.Combine(repoRoot, "99.SOURCE", "TEMPLATE", "WYNLAB.TEMPLATE");
        var designerCs = ReadTemplateFile(templateDir, "TplMasterSubGrid.Designer.cs");
        var formCs = ReadTemplateFile(templateDir, "TplMasterSubGrid.cs");

        var (searchNew, searchConfig, searchDecl) = BuildSearchFieldBlocks(spec);

        var masterColumns = ColumnsForSlot(spec, "grd1");
        var detailColumns = ColumnsForSlot(spec, "grd2");
        // grd1 저장은 선택사항이다 - Save Actions 탭에 grd1 행을 안 만들면 템플릿이 "조회전용"으로
        // 생성된다(SaveClick의 __SAVE_PROC__ 빈 문자열 체크, TplMasterSubGrid.cs 참고).
        var saveAction = SaveActionForSlotOrNull(spec, SaveActionScope.Detail, "grd1");

        var masterNew = new StringBuilder();
        var masterConfig = new StringBuilder();
        var masterDecl = new StringBuilder();
        // TplMasterSubGrid.cs: gvw1(grd1)=Role.Edit(POP 버튼 동작), gvw2(grd2)=Role.Query(조회전용 -
        // POP 버튼이 안 보이므로 안 붙인다. ScreenTemplateGenerator.PopColumns 주석 참고).
        AppendSimpleColumns(masterColumns, "colM", "gvw1", masterNew, masterConfig, masterDecl, allowPopupInGrid: true);

        var detailNew = new StringBuilder();
        var detailConfig = new StringBuilder();
        var detailDecl = new StringBuilder();
        AppendSimpleColumns(detailColumns, "colD", "gvw2", detailNew, detailConfig, detailDecl, allowPopupInGrid: false);

        designerCs = SpliceBlock(designerCs, "SEARCH_FIELD_NEW", searchNew);
        designerCs = SpliceBlock(designerCs, "SEARCH_FIELD_CONFIG", searchConfig);
        designerCs = SpliceBlock(designerCs, "SEARCH_FIELD_DECL", searchDecl);
        designerCs = SpliceBlock(designerCs, "MASTER_COLUMN_NEW", masterNew.ToString());
        designerCs = SpliceBlock(designerCs, "MASTER_COLUMN_CONFIG", masterConfig.ToString());
        designerCs = SpliceBlock(designerCs, "MASTER_COLUMN_DECL", masterDecl.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_COLUMN_NEW", detailNew.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_COLUMN_CONFIG", detailConfig.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_COLUMN_DECL", detailDecl.ToString());

        formCs = SpliceBlock(formCs, "QUERY_PARAMS", BuildQueryParamsBlock(spec));
        formCs = SpliceBlock(formCs, "SAVE_PARAMS", saveAction != null ? BuildSaveParamsBlock(saveAction) : string.Empty);

        designerCs = ApplyTokens(designerCs, spec, "TplMasterSubGrid");
        formCs = ApplyTokens(formCs, spec, "TplMasterSubGrid");
        return (designerCs, formCs);
    }

    // ================================================================================
    // 마스터-폼-서브그리드 = 템플릿 복제+치환 (WYNLAB.TEMPLATE/TplMasterFormSubGrid). 위 두
    // 템플릿과 달리 QueryProc/SaveProc(단일 문자열) 대신 spec.QuerySources/SaveActions로
    // 생성한다 - 조회 프로시저 1콜이 레코드셋을 2개(grd2/grd3용) 반환하고, 저장프로시저가
    // 3개(헤더/명세1/명세2)라 "쿼리 1개+저장 1개" 가정으로는 표현이 안 되기 때문(거래처+담당자+
    // 계좌 같은 1:N:N 구조). grd3/detail2Save는 선택사항이라 하위그리드가 1개뿐인 화면도 그냥
    // grd3 바인딩만 안 채우면 된다(2026-09-08 - 예전엔 grd2 하나만 조회전용으로 만드는 별도
    // 템플릿(GenerateMasterFormSubGridFromTemplate의 옛 버전, ColumnsForSlot/SaveActionForSlotOrNull
    // 기반)이 있었는데, 이 메서드가 그 쓰임새를 완전히 포함해서 하나로 합쳤다).
    // ================================================================================
    private static (string DesignerCs, string FormCs) GenerateMasterFormSubGridFromTemplate(ScreenGenSpec spec, string repoRoot)
    {
        var templateDir = Path.Combine(repoRoot, "99.SOURCE", "TEMPLATE", "WYNLAB.TEMPLATE");
        var designerCs = ReadTemplateFile(templateDir, "TplMasterFormSubGrid.Designer.cs");
        var formCs = ReadTemplateFile(templateDir, "TplMasterFormSubGrid.cs");

        var masterQuery = spec.QuerySources.FirstOrDefault(q => q.ResultSetBindings.Any(b => b.TargetSlot == "grd1"))
            ?? throw new InvalidOperationException("QuerySources에 grd1을 채우는 조회가 없습니다(ResultSetBindings에 TargetSlot=\"grd1\" 필요).");
        // grd1/panData 둘 다 이 컬럼목록을 쓴다. 예전(QuerySources/SaveActions로 통합되기 전)에는
        // 화면 상단 "Master Columns" 탭에 별도로 describe해서 채우는 독립된 필드를 썼는데, 그건
        // Query Sources 탭에서 grd1을 다시 describe해도 반영이 안 되는 버그가 있었다(2026-09-04
        // 실제 발견 - grd1/panData가 항상 빈 채로 생성됨). 지금은 QuerySources 하나에서 grd1
        // 바인딩을 직접 찾으므로 그 문제 자체가 구조적으로 사라졌다.
        var masterColumns = masterQuery.ResultSetBindings.First(b => b.TargetSlot == "grd1").Columns;
        // grd2~grd5는 전부 선택사항이다(2026-09-06 - "grd1,grd2만 바인딩해도 진행되게 해줘",
        // 2026-09-08 - grd4/grd5 두 개를 추가해서 최대 4개까지 늘렸다, 2026-09-09 - grd2까지
        // 완전히 선택사항으로 풀어서 grd1+panData만 있는 화면도 이 템플릿으로 만들 수 있게 했다,
        // "GRD1 + PANDATA 만 존재하는 폼도 MASTER-FORM-SUB GRID로 사용할 수있도록" 요청) - 안
        // 채우면 컬럼 없는 빈 그리드로 생성되고 저장도 안 탄다(TplMasterFormSubGrid.SaveClick의
        // __SAVE_PROC_1~4__ 빈 문자열 체크, LoadDetailAsync의 __DETAIL_QUERY_PROC__ 빈 문자열
        // 체크 참고, MasterSubGrid의 grd1 조회전용 폴백과 같은 원칙). 나중에 실제로 쓰고 싶으면
        // QuerySources/Save Actions에 해당 슬롯 바인딩을 추가하고 다시 생성하면 된다. grd1과 달리
        // grd2~grd5는 masterQuery가 아닌 "다른" QuerySource(보통 별도 work_type, 예: Q1)의
        // 레코드셋이어야 한다 - LoadDetailAsync가 grd1 조회와 별개로, 마스터 행을 고를 때마다
        // 그 하나의 프로시저를 다시 부르기 때문(QueryMultiAsync 한 번으로 grd2~grd5 레코드셋을
        // 전부 받는다).
        var otherQueries = spec.QuerySources.Where(q => !ReferenceEquals(q, masterQuery)).ToList();
        var grd2Binding = otherQueries.SelectMany(q => q.ResultSetBindings).FirstOrDefault(b => b.TargetSlot == "grd2");
        var grd3Binding = otherQueries.SelectMany(q => q.ResultSetBindings).FirstOrDefault(b => b.TargetSlot == "grd3");
        var grd4Binding = otherQueries.SelectMany(q => q.ResultSetBindings).FirstOrDefault(b => b.TargetSlot == "grd4");
        var grd5Binding = otherQueries.SelectMany(q => q.ResultSetBindings).FirstOrDefault(b => b.TargetSlot == "grd5");
        var detailBinding = grd2Binding ?? grd3Binding ?? grd4Binding ?? grd5Binding;
        var detailQuery = detailBinding != null ? otherQueries.First(q => q.ResultSetBindings.Contains(detailBinding)) : null;

        // 헤더/grd2 저장 모두 선택사항이다(2026-09-07 - "SAVE 기능을 안 만들 수도 있어야 한다") -
        // grd3/grd4/grd5와 같은 원칙으로 확장했다. grd2~grd5 저장은 실제로는 헤더 저장이 돌려주는
        // 키가 있어야 동작하므로(SaveClick의 headerKey), 헤더가 없으면 TplMasterFormSubGrid.SaveClick
        // 맨 앞의 __SAVE_PROC__ 빈 문자열 체크가 저장 전체를 건너뛴다(헤더 없이 하위그리드만
        // 저장하는 조합은 이 템플릿 구조상 의미가 없어서 별도로 안 막아도 자연히 무력화된다).
        var headerSave = spec.SaveActions.FirstOrDefault(a => a.Scope == SaveActionScope.Header && a.SourceSlot == "panData");
        var detail1Save = spec.SaveActions.FirstOrDefault(a => a.Scope == SaveActionScope.Detail && a.SourceSlot == "grd2");
        var detail2Save = spec.SaveActions.FirstOrDefault(a => a.Scope == SaveActionScope.Detail && a.SourceSlot == "grd3");
        var detail3Save = spec.SaveActions.FirstOrDefault(a => a.Scope == SaveActionScope.Detail && a.SourceSlot == "grd4");
        var detail4Save = spec.SaveActions.FirstOrDefault(a => a.Scope == SaveActionScope.Detail && a.SourceSlot == "grd5");

        var (searchNew, searchConfig, searchDecl) = BuildSearchFieldBlocks(spec);

        var masterNew = new StringBuilder();
        var masterConfig = new StringBuilder();
        var masterDecl = new StringBuilder();
        // TplMasterFormSubGrid.cs: gvw1(grd1)=Role.Query(조회전용), gvw2~gvw5(grd2~grd5)=Role.Edit -
        // grd1은 POP 버튼이 안 보이므로 안 붙이고(panData만 반영), grd2~grd5는 실제 편집 가능하므로
        // 그대로 붙인다(ScreenTemplateGenerator.PopColumns 주석 참고).
        AppendSimpleColumns(masterColumns, "colM", "gvw1", masterNew, masterConfig, masterDecl, allowPopupInGrid: false);

        var detail1New = new StringBuilder();
        var detail1Config = new StringBuilder();
        var detail1Decl = new StringBuilder();
        if (grd2Binding != null)
            AppendSimpleColumns(grd2Binding.Columns, "colD1", "gvw2", detail1New, detail1Config, detail1Decl, allowPopupInGrid: true);

        var detail2New = new StringBuilder();
        var detail2Config = new StringBuilder();
        var detail2Decl = new StringBuilder();
        if (grd3Binding != null)
            AppendSimpleColumns(grd3Binding.Columns, "colD2", "gvw3", detail2New, detail2Config, detail2Decl, allowPopupInGrid: true);

        var detail3New = new StringBuilder();
        var detail3Config = new StringBuilder();
        var detail3Decl = new StringBuilder();
        if (grd4Binding != null)
            AppendSimpleColumns(grd4Binding.Columns, "colD3", "gvw4", detail3New, detail3Config, detail3Decl, allowPopupInGrid: true);

        var detail4New = new StringBuilder();
        var detail4Config = new StringBuilder();
        var detail4Decl = new StringBuilder();
        if (grd5Binding != null)
            AppendSimpleColumns(grd5Binding.Columns, "colD4", "gvw5", detail4New, detail4Config, detail4Decl, allowPopupInGrid: true);

        // ---- panData(상세폼) - MasterFormSubGrid와 동일하게 masterColumns(grd1과 같은 컬럼목록)를
        // 라벨+입력컨트롤로 ----
        var formNew = new StringBuilder();
        var formDecl = new StringBuilder();
        foreach (var c in masterColumns)
        {
            formNew.AppendLine($"        this.lblDetail{PascalCase(c.Name)} = new DevExpress.XtraEditors.LabelControl();");
            formNew.AppendLine($"        this.{DetailFieldDecl(c)}");
            formDecl.AppendLine($"    private DevExpress.XtraEditors.LabelControl lblDetail{PascalCase(c.Name)};");
            formDecl.AppendLine($"    private {DetailFieldType(c)} {DetailFieldName(c)};");
        }
        var formConfig = new StringBuilder();
        AppendPanDataFieldLayout(formConfig, masterColumns);

        var formAssign = new StringBuilder();
        var formClear = new StringBuilder();
        var formTag = new StringBuilder();
        foreach (var c in masterColumns)
        {
            formAssign.AppendLine($"        {DetailFieldAssign(c, $"row[\"{c.Name}\"]")}");
            formClear.AppendLine($"        {DetailFieldClear(c)}");
            formTag.AppendLine($"        {DetailFieldTag(c)}");
        }

        var formSaveParams = new StringBuilder();
        if (headerSave != null)
        {
            foreach (var kvp in headerSave.SaveParamColumnMap)
            {
                var col = masterColumns.FirstOrDefault(c => c.Name == kvp.Value);
                var accessor = col != null ? DetailFieldReadExpr(col) : $"{DetailFieldName(new ColumnSpec { Name = kvp.Value })}.Text";
                formSaveParams.AppendLine($"            [\"p_{kvp.Key}\"] = {accessor},");
            }
            foreach (var unmapped in headerSave.UnmappedSaveParams())
                formSaveParams.AppendLine($"            [\"p_{unmapped}\"] = null, // TODO: 값 채우기");
        }

        designerCs = SpliceBlock(designerCs, "SEARCH_FIELD_NEW", searchNew);
        designerCs = SpliceBlock(designerCs, "SEARCH_FIELD_CONFIG", searchConfig);
        designerCs = SpliceBlock(designerCs, "SEARCH_FIELD_DECL", searchDecl);
        designerCs = SpliceBlock(designerCs, "MASTER_COLUMN_NEW", masterNew.ToString());
        designerCs = SpliceBlock(designerCs, "MASTER_COLUMN_CONFIG", masterConfig.ToString());
        designerCs = SpliceBlock(designerCs, "MASTER_COLUMN_DECL", masterDecl.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_COLUMN_NEW", detail1New.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_COLUMN_CONFIG", detail1Config.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_COLUMN_DECL", detail1Decl.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL2_COLUMN_NEW", detail2New.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL2_COLUMN_CONFIG", detail2Config.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL2_COLUMN_DECL", detail2Decl.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL3_COLUMN_NEW", detail3New.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL3_COLUMN_CONFIG", detail3Config.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL3_COLUMN_DECL", detail3Decl.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL4_COLUMN_NEW", detail4New.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL4_COLUMN_CONFIG", detail4Config.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL4_COLUMN_DECL", detail4Decl.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_FORM_NEW", formNew.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_FORM_CONFIG", formConfig.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_FORM_DECL", formDecl.ToString());

        formCs = SpliceBlock(formCs, "QUERY_PARAMS", BuildQueryParamsBlock(spec));
        formCs = SpliceBlock(formCs, "DETAIL_FORM_ASSIGN", formAssign.ToString());
        formCs = SpliceBlock(formCs, "DETAIL_FORM_CLEAR", formClear.ToString());
        formCs = SpliceBlock(formCs, "DETAIL_FORM_TAG", formTag.ToString());
        formCs = SpliceBlock(formCs, "DETAIL_FORM_SAVE_PARAMS", formSaveParams.ToString());
        formCs = SpliceBlock(formCs, "DETAIL1_SAVE_PARAMS", BuildDetailSaveParamsBlock(detail1Save));
        formCs = SpliceBlock(formCs, "DETAIL2_SAVE_PARAMS", BuildDetailSaveParamsBlock(detail2Save));
        formCs = SpliceBlock(formCs, "DETAIL3_SAVE_PARAMS", BuildDetailSaveParamsBlock(detail3Save));
        formCs = SpliceBlock(formCs, "DETAIL4_SAVE_PARAMS", BuildDetailSaveParamsBlock(detail4Save));

        var generatedHeader = $"// AI Builder가 마스터-폼-서브그리드 템플릿을 복제해서 자동 생성 - {DateTime.Now:yyyy-MM-dd}.\r\n" +
            "// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.\r\n";
        designerCs = SpliceBlock(designerCs, "FILE_HEADER", generatedHeader);
        formCs = SpliceBlock(formCs, "FILE_HEADER", generatedHeader);

        // 이 템플릿은 프로시저가 여러 개라 공용 ApplyTokens(spec.QueryProc/SaveProc 하나씩만 봄)를
        // 안 쓰고 QuerySources/SaveActions에서 직접 채운다.
        string LocalApplyTokens(string text) => text
            .Replace("__MENU_CAPTION__", EscapeCs(spec.MenuCaption))
            .Replace("__QUERY_PROC__", masterQuery.ProcName)
            .Replace("__DETAIL_QUERY_PROC__", detailQuery?.ProcName ?? string.Empty)
            .Replace("__DETAIL_WORK_TYPE__", detailQuery?.WorkType ?? string.Empty)
            .Replace("__SAVE_PROC__", headerSave?.ProcName ?? string.Empty)
            .Replace("__SAVE_PROC_1__", detail1Save?.ProcName ?? string.Empty)
            .Replace("__SAVE_PROC_2__", detail2Save?.ProcName ?? string.Empty)
            .Replace("__SAVE_PROC_3__", detail3Save?.ProcName ?? string.Empty)
            .Replace("__SAVE_PROC_4__", detail4Save?.ProcName ?? string.Empty)
            .Replace("__DETAIL_KEY_PARAM__", spec.DetailKeyParam ?? string.Empty)
            .Replace("__MASTER_KEY_COLUMN__", spec.MasterKeyColumn ?? string.Empty)
            .Replace("namespace WYNLAB.TEMPLATE;", $"namespace WYNLAB.{spec.Module};")
            .Replace("TplMasterFormSubGrid", spec.ScreenClassNm);

        designerCs = LocalApplyTokens(designerCs);
        formCs = LocalApplyTokens(formCs);

        return (designerCs, formCs);
    }

    // ================================================================================
    // 트리마스터-서브그리드 = 템플릿 복제+치환 (WYNLAB.TEMPLATE/TplTreeMasterSubGrid).
    // GenerateMasterFormSubGridFromTemplate와 완전히 같은 구조/규칙(grd3·저장 전부 선택사항 등)
    // 인데 grd1(평범한 목록) 대신 tree1(자기참조 계층)이 마스터라는 점만 다르다 - 마스터 컬럼
    // 생성에 AppendSimpleColumns(GridColumn 대상) 대신 AppendTreeColumns(TreeListColumn 대상)를
    // 쓰고, tree1.KeyFieldName/ParentFieldName에 넣을 __MASTER_KEY_COLUMN__/
    // __MASTER_PARENT_COLUMN__ 토큰이 하나 더 있다(2026-09-08 추가 - 부서/메뉴처럼 상위-하위
    // 구조를 가진 목록이 마스터인 화면용).
    // ================================================================================
    private static (string DesignerCs, string FormCs) GenerateTreeMasterSubGridFromTemplate(ScreenGenSpec spec, string repoRoot)
    {
        var templateDir = Path.Combine(repoRoot, "99.SOURCE", "TEMPLATE", "WYNLAB.TEMPLATE");
        var designerCs = ReadTemplateFile(templateDir, "TplTreeMasterSubGrid.Designer.cs");
        var formCs = ReadTemplateFile(templateDir, "TplTreeMasterSubGrid.cs");

        // 마스터 슬롯은 tree1이 실제 컨트롤 이름이지만, 검색조건 자동채움(frmAIBuilder.
        // DescribeQueryAsync)/재검증(FindStaleProcReferenceAsync) 등 여러 공용 로직이 "grd1"을
        // 마스터 슬롯의 고정 식별자로 이미 쓰고 있어서(그리드 기반 템플릿과 공유) 완전히
        // "tree1"로 바꾸면 그 공용 로직을 전부 고쳐야 한다 - 대신 여기서 tree1/grd1 둘 다
        // 받아들인다(2026-09-09, "Target Control에 tree가 안 보인다" 실제 지적 - Target Control
        // 드롭다운에 tree1도 추가했다).
        var masterQuery = spec.QuerySources.FirstOrDefault(q => q.ResultSetBindings.Any(b => b.TargetSlot == "tree1" || b.TargetSlot == "grd1"))
            ?? throw new InvalidOperationException("QuerySources에 tree1(마스터)을 채우는 조회가 없습니다(ResultSetBindings에 TargetSlot=\"tree1\" 필요).");
        var masterColumns = masterQuery.ResultSetBindings.First(b => b.TargetSlot == "tree1" || b.TargetSlot == "grd1").Columns;
        // grd2~grd5는 전부 선택사항이다(2026-09-11 요청 - "바인딩 정보가 없어도 그냥 생성되어야
        // 해", GenerateMasterOneSheetFromTemplate과 같은 원칙). detailQuery(트리 마스터와 별도인
        // 하위그리드용 프로시저) 자체가 없으면 LocalApplyTokens에서 __DETAIL_QUERY_PROC__ 등이
        // 빈 문자열로 치환되고, TplTreeMasterSubGrid.LoadDetailAsync가 그 경우를 가드해서 빈
        // 문자열로 서버를 부르지 않는다.
        var detailQuery = spec.QuerySources.FirstOrDefault(q => !ReferenceEquals(q, masterQuery));
        var grd2Binding = detailQuery?.ResultSetBindings.FirstOrDefault(b => b.TargetSlot == "grd2");
        var grd3Binding = detailQuery?.ResultSetBindings.FirstOrDefault(b => b.TargetSlot == "grd3");
        var grd4Binding = detailQuery?.ResultSetBindings.FirstOrDefault(b => b.TargetSlot == "grd4");
        var grd5Binding = detailQuery?.ResultSetBindings.FirstOrDefault(b => b.TargetSlot == "grd5");

        var headerSave = spec.SaveActions.FirstOrDefault(a => a.Scope == SaveActionScope.Header && a.SourceSlot == "panData");
        var detail1Save = spec.SaveActions.FirstOrDefault(a => a.Scope == SaveActionScope.Detail && a.SourceSlot == "grd2");
        var detail2Save = spec.SaveActions.FirstOrDefault(a => a.Scope == SaveActionScope.Detail && a.SourceSlot == "grd3");
        var detail3Save = spec.SaveActions.FirstOrDefault(a => a.Scope == SaveActionScope.Detail && a.SourceSlot == "grd4");
        var detail4Save = spec.SaveActions.FirstOrDefault(a => a.Scope == SaveActionScope.Detail && a.SourceSlot == "grd5");

        var (searchNew, searchConfig, searchDecl) = BuildSearchFieldBlocks(spec);

        var masterNew = new StringBuilder();
        var masterConfig = new StringBuilder();
        var masterDecl = new StringBuilder();
        AppendTreeColumns(masterColumns, masterNew, masterConfig, masterDecl);

        var detail1New = new StringBuilder();
        var detail1Config = new StringBuilder();
        var detail1Decl = new StringBuilder();
        if (grd2Binding != null)
            AppendSimpleColumns(grd2Binding.Columns, "colD1", "gvw2", detail1New, detail1Config, detail1Decl, allowPopupInGrid: true);

        var detail2New = new StringBuilder();
        var detail2Config = new StringBuilder();
        var detail2Decl = new StringBuilder();
        if (grd3Binding != null)
            AppendSimpleColumns(grd3Binding.Columns, "colD2", "gvw3", detail2New, detail2Config, detail2Decl, allowPopupInGrid: true);

        var detail3New = new StringBuilder();
        var detail3Config = new StringBuilder();
        var detail3Decl = new StringBuilder();
        if (grd4Binding != null)
            AppendSimpleColumns(grd4Binding.Columns, "colD3", "gvw4", detail3New, detail3Config, detail3Decl, allowPopupInGrid: true);

        var detail4New = new StringBuilder();
        var detail4Config = new StringBuilder();
        var detail4Decl = new StringBuilder();
        if (grd5Binding != null)
            AppendSimpleColumns(grd5Binding.Columns, "colD4", "gvw5", detail4New, detail4Config, detail4Decl, allowPopupInGrid: true);

        // ---- panData(상세폼) - GenerateMasterFormSubGridFromTemplate와 동일하게 masterColumns를
        // 라벨+입력컨트롤로 ----
        var formNew = new StringBuilder();
        var formDecl = new StringBuilder();
        foreach (var c in masterColumns)
        {
            formNew.AppendLine($"        this.lblDetail{PascalCase(c.Name)} = new DevExpress.XtraEditors.LabelControl();");
            formNew.AppendLine($"        this.{DetailFieldDecl(c)}");
            formDecl.AppendLine($"    private DevExpress.XtraEditors.LabelControl lblDetail{PascalCase(c.Name)};");
            formDecl.AppendLine($"    private {DetailFieldType(c)} {DetailFieldName(c)};");
        }
        var formConfig = new StringBuilder();
        AppendPanDataFieldLayout(formConfig, masterColumns);

        var formAssign = new StringBuilder();
        var formClear = new StringBuilder();
        var formTag = new StringBuilder();
        foreach (var c in masterColumns)
        {
            formAssign.AppendLine($"        {DetailFieldAssign(c, $"row[\"{c.Name}\"]")}");
            formClear.AppendLine($"        {DetailFieldClear(c)}");
            formTag.AppendLine($"        {DetailFieldTag(c)}");
        }

        var formSaveParams = new StringBuilder();
        if (headerSave != null)
        {
            foreach (var kvp in headerSave.SaveParamColumnMap)
            {
                var col = masterColumns.FirstOrDefault(c => c.Name == kvp.Value);
                var accessor = col != null ? DetailFieldReadExpr(col) : $"{DetailFieldName(new ColumnSpec { Name = kvp.Value })}.Text";
                formSaveParams.AppendLine($"            [\"p_{kvp.Key}\"] = {accessor},");
            }
            foreach (var unmapped in headerSave.UnmappedSaveParams())
                formSaveParams.AppendLine($"            [\"p_{unmapped}\"] = null, // TODO: 값 채우기");
        }

        designerCs = SpliceBlock(designerCs, "SEARCH_FIELD_NEW", searchNew);
        designerCs = SpliceBlock(designerCs, "SEARCH_FIELD_CONFIG", searchConfig);
        designerCs = SpliceBlock(designerCs, "SEARCH_FIELD_DECL", searchDecl);
        designerCs = SpliceBlock(designerCs, "MASTER_COLUMN_NEW", masterNew.ToString());
        designerCs = SpliceBlock(designerCs, "MASTER_COLUMN_CONFIG", masterConfig.ToString());
        designerCs = SpliceBlock(designerCs, "MASTER_COLUMN_DECL", masterDecl.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_COLUMN_NEW", detail1New.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_COLUMN_CONFIG", detail1Config.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_COLUMN_DECL", detail1Decl.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL2_COLUMN_NEW", detail2New.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL2_COLUMN_CONFIG", detail2Config.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL2_COLUMN_DECL", detail2Decl.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL3_COLUMN_NEW", detail3New.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL3_COLUMN_CONFIG", detail3Config.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL3_COLUMN_DECL", detail3Decl.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL4_COLUMN_NEW", detail4New.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL4_COLUMN_CONFIG", detail4Config.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL4_COLUMN_DECL", detail4Decl.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_FORM_NEW", formNew.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_FORM_CONFIG", formConfig.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_FORM_DECL", formDecl.ToString());

        formCs = SpliceBlock(formCs, "QUERY_PARAMS", BuildQueryParamsBlock(spec));
        formCs = SpliceBlock(formCs, "DETAIL_FORM_ASSIGN", formAssign.ToString());
        formCs = SpliceBlock(formCs, "DETAIL_FORM_CLEAR", formClear.ToString());
        formCs = SpliceBlock(formCs, "DETAIL_FORM_TAG", formTag.ToString());
        formCs = SpliceBlock(formCs, "DETAIL_FORM_SAVE_PARAMS", formSaveParams.ToString());
        formCs = SpliceBlock(formCs, "DETAIL1_SAVE_PARAMS", BuildDetailSaveParamsBlock(detail1Save));
        formCs = SpliceBlock(formCs, "DETAIL2_SAVE_PARAMS", BuildDetailSaveParamsBlock(detail2Save));
        formCs = SpliceBlock(formCs, "DETAIL3_SAVE_PARAMS", BuildDetailSaveParamsBlock(detail3Save));
        formCs = SpliceBlock(formCs, "DETAIL4_SAVE_PARAMS", BuildDetailSaveParamsBlock(detail4Save));

        var generatedHeader = $"// AI Builder가 트리마스터-서브그리드 템플릿을 복제해서 자동 생성 - {DateTime.Now:yyyy-MM-dd}.\r\n" +
            "// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.\r\n";
        designerCs = SpliceBlock(designerCs, "FILE_HEADER", generatedHeader);
        formCs = SpliceBlock(formCs, "FILE_HEADER", generatedHeader);

        string LocalApplyTokens(string text) => text
            .Replace("__MENU_CAPTION__", EscapeCs(spec.MenuCaption))
            .Replace("__QUERY_PROC__", masterQuery.ProcName)
            .Replace("__DETAIL_QUERY_PROC__", detailQuery?.ProcName ?? string.Empty)
            .Replace("__DETAIL_WORK_TYPE__", detailQuery?.WorkType ?? string.Empty)
            .Replace("__SAVE_PROC__", headerSave?.ProcName ?? string.Empty)
            .Replace("__SAVE_PROC_1__", detail1Save?.ProcName ?? string.Empty)
            .Replace("__SAVE_PROC_2__", detail2Save?.ProcName ?? string.Empty)
            .Replace("__SAVE_PROC_3__", detail3Save?.ProcName ?? string.Empty)
            .Replace("__SAVE_PROC_4__", detail4Save?.ProcName ?? string.Empty)
            .Replace("__DETAIL_KEY_PARAM__", spec.DetailKeyParam ?? string.Empty)
            .Replace("__MASTER_KEY_COLUMN__", spec.MasterKeyColumn ?? string.Empty)
            .Replace("__MASTER_PARENT_COLUMN__", spec.MasterParentColumn ?? string.Empty)
            .Replace("namespace WYNLAB.TEMPLATE;", $"namespace WYNLAB.{spec.Module};")
            .Replace("TplTreeMasterSubGrid", spec.ScreenClassNm);

        designerCs = LocalApplyTokens(designerCs);
        formCs = LocalApplyTokens(formCs);

        return (designerCs, formCs);
    }

    // ================================================================================
    // 마스터-상세폼(단일 시트) = 템플릿 복제+치환 (WYNLAB.TEMPLATE/TplMasterOneSheet).
    // 2026-09-09 재설계 - grd1(마스터 목록 그리드) 자체가 없다("grd1이 없는 모습" 요청). 화면은
    // panHeader(검색조건) + panData(문서 자체) + 하위 그리드(grd2~grd5, 탭, 전부 선택사항)만
    // 있고, 툴바 조회 한 번으로 __QUERY_PROC__가 돌려주는 레코드셋들(0번=panData, 1~4번=
    // grd2~grd5)을 그대로 채운다 - grd1에서 행을 고르는 중간 단계가 없다. QuerySources 쪽에서는
    // 여전히 "grd1"을 헤더 레코드셋의 슬롯 이름으로 쓴다(frmAIBuilder의 마스터슬롯/검색조건
    // 자동채움 로직이 전부 "grd1"을 기준으로 하므로 - GenerateTreeMasterSubGridFromTemplate이
    // tree1을 grd1의 별칭으로 받아들이는 것과 같은 이유, 실제 그리드 컨트롤은 없어도 슬롯
    // 이름만 그대로 재사용한다). grd2도 이제 선택사항이다(예전엔 있었지만, "MasterFormSubGrid와
    // 달리 하위그리드가 아예 없는 단일 시트" 원래 취지를 살려 grd2~grd5 넷 다 없어도 생성되게
    // 했다) - GenerateMasterFormSubGridFromTemplate과 달리 detailQuery/grd2Binding을 필수로
    // 요구하지 않는다.
    // ================================================================================
    private static (string DesignerCs, string FormCs) GenerateMasterOneSheetFromTemplate(ScreenGenSpec spec, string repoRoot)
    {
        var templateDir = Path.Combine(repoRoot, "99.SOURCE", "TEMPLATE", "WYNLAB.TEMPLATE");
        var designerCs = ReadTemplateFile(templateDir, "TplMasterOneSheet.Designer.cs");
        var formCs = ReadTemplateFile(templateDir, "TplMasterOneSheet.cs");

        var headerQuery = spec.QuerySources.FirstOrDefault(q => q.ResultSetBindings.Any(b => b.TargetSlot == "grd1"))
            ?? throw new InvalidOperationException("QuerySources에 grd1(문서 헤더)을 채우는 조회가 없습니다(ResultSetBindings에 TargetSlot=\"grd1\" 필요).");
        var headerColumns = headerQuery.ResultSetBindings.First(b => b.TargetSlot == "grd1").Columns;

        // grd2~grd5는 전부 선택사항이다 - 헤더와 같은 QuerySource(다른 레코드셋)일 수도, 완전히
        // 별도 QuerySource(다른 프로시저)일 수도 있어 headerQuery 포함 전체에서 찾는다.
        var grd2Binding = spec.QuerySources.SelectMany(q => q.ResultSetBindings).FirstOrDefault(b => b.TargetSlot == "grd2");
        var grd3Binding = spec.QuerySources.SelectMany(q => q.ResultSetBindings).FirstOrDefault(b => b.TargetSlot == "grd3");
        var grd4Binding = spec.QuerySources.SelectMany(q => q.ResultSetBindings).FirstOrDefault(b => b.TargetSlot == "grd4");
        var grd5Binding = spec.QuerySources.SelectMany(q => q.ResultSetBindings).FirstOrDefault(b => b.TargetSlot == "grd5");
        // grd2~grd5가 헤더와 다른 프로시저에서 온다면 그 프로시저 하나(QueryMultiAsync 한 번)만
        // 쓸 수 있다 - 화면 코드(__QUERY_PROC__ 한 번)가 레코드셋 인덱스로 슬롯을 구분하므로,
        // 전부 headerQuery와 같은 QuerySource여야 한다(TplMasterFormSubGrid의 detailQuery와 달리
        // 여기는 프로시저가 하나뿐이다 - "grd1이 없는 모습"은 조회 자체가 한 번이라는 뜻이기도 함).
        var otherSlotQuery = new[] { grd2Binding, grd3Binding, grd4Binding, grd5Binding }
            .Where(b => b != null)
            .Select(b => spec.QuerySources.First(q => q.ResultSetBindings.Contains(b!)))
            .FirstOrDefault(q => !ReferenceEquals(q, headerQuery));
        if (otherSlotQuery != null)
            throw new InvalidOperationException($"grd2~grd5는 grd1과 같은 조회프로시저({headerQuery.ProcName})의 레코드셋이어야 합니다 - {otherSlotQuery.ProcName}은(는) 별도 QuerySource입니다.");

        var headerSave = SaveActionForSlotOrNull(spec, SaveActionScope.Header, "panData");
        var detail1Save = spec.SaveActions.FirstOrDefault(a => a.Scope == SaveActionScope.Detail && a.SourceSlot == "grd2");
        var detail2Save = spec.SaveActions.FirstOrDefault(a => a.Scope == SaveActionScope.Detail && a.SourceSlot == "grd3");
        var detail3Save = spec.SaveActions.FirstOrDefault(a => a.Scope == SaveActionScope.Detail && a.SourceSlot == "grd4");
        var detail4Save = spec.SaveActions.FirstOrDefault(a => a.Scope == SaveActionScope.Detail && a.SourceSlot == "grd5");

        var (searchNew, searchConfig, searchDecl) = BuildSearchFieldBlocks(spec);

        var detail1New = new StringBuilder();
        var detail1Config = new StringBuilder();
        var detail1Decl = new StringBuilder();
        if (grd2Binding != null)
            AppendSimpleColumns(grd2Binding.Columns, "colD1", "gvw2", detail1New, detail1Config, detail1Decl, allowPopupInGrid: true);

        var detail2New = new StringBuilder();
        var detail2Config = new StringBuilder();
        var detail2Decl = new StringBuilder();
        if (grd3Binding != null)
            AppendSimpleColumns(grd3Binding.Columns, "colD2", "gvw3", detail2New, detail2Config, detail2Decl, allowPopupInGrid: true);

        var detail3New = new StringBuilder();
        var detail3Config = new StringBuilder();
        var detail3Decl = new StringBuilder();
        if (grd4Binding != null)
            AppendSimpleColumns(grd4Binding.Columns, "colD3", "gvw4", detail3New, detail3Config, detail3Decl, allowPopupInGrid: true);

        var detail4New = new StringBuilder();
        var detail4Config = new StringBuilder();
        var detail4Decl = new StringBuilder();
        if (grd5Binding != null)
            AppendSimpleColumns(grd5Binding.Columns, "colD4", "gvw5", detail4New, detail4Config, detail4Decl, allowPopupInGrid: true);

        // ---- panData(문서 자체) - headerColumns를 라벨+입력컨트롤로 ----
        var formNew = new StringBuilder();
        var formDecl = new StringBuilder();
        foreach (var c in headerColumns)
        {
            formNew.AppendLine($"        this.lblDetail{PascalCase(c.Name)} = new DevExpress.XtraEditors.LabelControl();");
            formNew.AppendLine($"        this.{DetailFieldDecl(c)}");
            formDecl.AppendLine($"    private DevExpress.XtraEditors.LabelControl lblDetail{PascalCase(c.Name)};");
            formDecl.AppendLine($"    private {DetailFieldType(c)} {DetailFieldName(c)};");
        }
        var formConfig = new StringBuilder();
        AppendPanDataFieldLayout(formConfig, headerColumns);

        var formAssign = new StringBuilder();
        var formClear = new StringBuilder();
        var formTag = new StringBuilder();
        foreach (var c in headerColumns)
        {
            formAssign.AppendLine($"        {DetailFieldAssign(c, $"row[\"{c.Name}\"]")}");
            formClear.AppendLine($"        {DetailFieldClear(c)}");
            formTag.AppendLine($"        {DetailFieldTag(c)}");
        }

        var formSaveParams = new StringBuilder();
        if (headerSave != null)
        {
            foreach (var kvp in headerSave.SaveParamColumnMap)
            {
                var col = headerColumns.FirstOrDefault(c => c.Name == kvp.Value);
                var accessor = col != null ? DetailFieldReadExpr(col) : $"{DetailFieldName(new ColumnSpec { Name = kvp.Value })}.Text";
                formSaveParams.AppendLine($"            [\"p_{kvp.Key}\"] = {accessor},");
            }
            foreach (var unmapped in headerSave.UnmappedSaveParams())
                formSaveParams.AppendLine($"            [\"p_{unmapped}\"] = null, // TODO: 값 채우기");
        }

        designerCs = SpliceBlock(designerCs, "SEARCH_FIELD_NEW", searchNew);
        designerCs = SpliceBlock(designerCs, "SEARCH_FIELD_CONFIG", searchConfig);
        designerCs = SpliceBlock(designerCs, "SEARCH_FIELD_DECL", searchDecl);
        designerCs = SpliceBlock(designerCs, "DETAIL_COLUMN_NEW", detail1New.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_COLUMN_CONFIG", detail1Config.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_COLUMN_DECL", detail1Decl.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL2_COLUMN_NEW", detail2New.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL2_COLUMN_CONFIG", detail2Config.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL2_COLUMN_DECL", detail2Decl.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL3_COLUMN_NEW", detail3New.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL3_COLUMN_CONFIG", detail3Config.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL3_COLUMN_DECL", detail3Decl.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL4_COLUMN_NEW", detail4New.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL4_COLUMN_CONFIG", detail4Config.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL4_COLUMN_DECL", detail4Decl.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_FORM_NEW", formNew.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_FORM_CONFIG", formConfig.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_FORM_DECL", formDecl.ToString());

        formCs = SpliceBlock(formCs, "QUERY_PARAMS", BuildQueryParamsBlock(spec));
        formCs = SpliceBlock(formCs, "DETAIL_FORM_ASSIGN", formAssign.ToString());
        formCs = SpliceBlock(formCs, "DETAIL_FORM_CLEAR", formClear.ToString());
        formCs = SpliceBlock(formCs, "DETAIL_FORM_TAG", formTag.ToString());
        formCs = SpliceBlock(formCs, "DETAIL_FORM_SAVE_PARAMS", formSaveParams.ToString());
        formCs = SpliceBlock(formCs, "DETAIL1_SAVE_PARAMS", BuildDetailSaveParamsBlock(detail1Save));
        formCs = SpliceBlock(formCs, "DETAIL2_SAVE_PARAMS", BuildDetailSaveParamsBlock(detail2Save));
        formCs = SpliceBlock(formCs, "DETAIL3_SAVE_PARAMS", BuildDetailSaveParamsBlock(detail3Save));
        formCs = SpliceBlock(formCs, "DETAIL4_SAVE_PARAMS", BuildDetailSaveParamsBlock(detail4Save));

        var generatedHeader = $"// AI Builder가 마스터-상세폼(단일 시트) 템플릿을 복제해서 자동 생성 - {DateTime.Now:yyyy-MM-dd}.\r\n" +
            "// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.\r\n";
        designerCs = SpliceBlock(designerCs, "FILE_HEADER", generatedHeader);
        formCs = SpliceBlock(formCs, "FILE_HEADER", generatedHeader);

        // grd2~grd5가 전부 선택사항이라 프로시저가 여러 개일 수 있는 MasterFormSubGrid와 달리
        // 여기는 항상 headerQuery 하나뿐이지만(위 검증 참고), __SAVE_PROC_1~4__ 등 공용
        // ApplyTokens가 모르는 토큰이 있어 MasterFormSubGrid와 같은 방식으로 직접 채운다.
        string LocalApplyTokens(string text) => text
            .Replace("__MENU_CAPTION__", EscapeCs(spec.MenuCaption))
            .Replace("__QUERY_PROC__", headerQuery.ProcName)
            .Replace("__SAVE_PROC__", headerSave?.ProcName ?? string.Empty)
            .Replace("__SAVE_PROC_1__", detail1Save?.ProcName ?? string.Empty)
            .Replace("__SAVE_PROC_2__", detail2Save?.ProcName ?? string.Empty)
            .Replace("__SAVE_PROC_3__", detail3Save?.ProcName ?? string.Empty)
            .Replace("__SAVE_PROC_4__", detail4Save?.ProcName ?? string.Empty)
            .Replace("__MASTER_KEY_COLUMN__", spec.MasterKeyColumn ?? string.Empty)
            .Replace("namespace WYNLAB.TEMPLATE;", $"namespace WYNLAB.{spec.Module};")
            .Replace("TplMasterOneSheet", spec.ScreenClassNm);

        designerCs = LocalApplyTokens(designerCs);
        formCs = LocalApplyTokens(formCs);
        return (designerCs, formCs);
    }

    /// <summary>SaveAction(Detail scope) 하나의 ParamColumnMap -> "row"/"version" 지역변수를 읽는
    /// ["p_x"] = ProcData.Str(row, "col", version), 줄들. TplMasterFormSubGrid.SaveClick의
    /// (row, version) => new Dictionary&lt;...&gt; { ... } 콜백 안에 그대로 끼워진다. action이
    /// null이면(grd3처럼 선택사항인 슬롯을 안 채운 경우) 빈 문자열 - 그 콜백은 아무 파라미터도
    /// 안 채우지만, 어차피 __SAVE_PROC_2__가 빈 문자열이라 호출 자체가 안 일어난다.</summary>
    private static string BuildDetailSaveParamsBlock(SaveAction? action)
    {
        if (action == null) return string.Empty;

        var sb = new StringBuilder();
        foreach (var kvp in action.SaveParamColumnMap)
            sb.AppendLine($"            [\"p_{kvp.Key}\"] = ProcData.Str(row, \"{kvp.Value}\", version),");
        foreach (var unmapped in action.UnmappedSaveParams())
            sb.AppendLine($"            [\"p_{unmapped}\"] = null, // TODO: 값 채우기");
        return sb.ToString();
    }

    // ---- 템플릿 공통 헬퍼 ----

    private static string ReadTemplateFile(string templateDir, string fileName)
    {
        var path = Path.Combine(templateDir, fileName);
        if (!File.Exists(path)) throw new InvalidOperationException($"템플릿을 찾을 수 없습니다: {path}");
        return File.ReadAllText(path);
    }

    // 검색조건(panHeader) 컨트롤 - 조회프로시저의 실제 파라미터(ParamName 채워짐)와 화면표시
    // 전용 컨트롤(ParamName 비어있음, 예: dept_cd 옆의 dept_nm)을 같은 목록(SearchFields)에서
    // 함께 다룬다(2026-09-08). TEXT/CHECK/COMBO/NUMBER/DTE/POP/MEMO 전부 지원 - DetailFieldType은
    // panData와 완전히 같은 컨트롤 매핑이라 그대로 재사용한다. 각 입력칸 앞에는 Caption 텍스트의
    // LabelControl을 같이 놓는다(2026-09-09 요청 - "검색조건에 Caption컬럼을 추가해서 폼생성시
    // panHeader에 컨트롤이 만들어질때 그 앞에 라벨의 캡션으로 적용해줘") - panData의
    // AppendPanDataFieldLayout과 같은 원칙(라벨 폭 고정, AutoSize에 의존 안 함)이지만 panHeader는
    // 세로 한 줄(49px)뿐이라 라벨을 위가 아니라 왼쪽에 붙인다(popPopUp.BuildSearchPanel의
    // 조회조건 패널과 같은 가로 배치). f.Visible=false(검색조건 그리드의 View 체크박스, 2026-09-09
    // 요청)면 라벨/컨트롤 둘 다 그대로 만들어지되(코드에서 계속 값을 읽고 쓸 수 있어야 하므로)
    // Visible=false로 생성된다 - ParamName이 비어서 아예 조회에 안 보내는 것과는 독립된 축이다.
    private static (string New, string Config, string Decl) BuildSearchFieldBlocks(ScreenGenSpec spec)
    {
        var searchNew = new StringBuilder();
        var searchDecl = new StringBuilder();
        foreach (var f in spec.SearchFields)
        {
            searchNew.AppendLine($"        this.lblSearch{PascalCase(f.Name)} = new DevExpress.XtraEditors.LabelControl();");
            searchNew.AppendLine($"        this.{SearchFieldDecl(f)}");
            searchDecl.AppendLine($"    private DevExpress.XtraEditors.LabelControl lblSearch{PascalCase(f.Name)};");
            searchDecl.AppendLine($"    private {DetailFieldType(f)} {SearchFieldName(f)};");
        }
        var searchConfig = new StringBuilder();
        const int labelWidth = 80;
        const int editWidth = 150;
        const int fieldSpacing = 16;
        var x = 16;
        foreach (var f in spec.SearchFields)
        {
            var lbl = $"lblSearch{PascalCase(f.Name)}";
            var field = SearchFieldName(f);
            searchConfig.AppendLine($"        this.{lbl}.Location = new System.Drawing.Point({x}, 24);");
            searchConfig.AppendLine($"        this.{lbl}.Name = \"{lbl}\";");
            searchConfig.AppendLine($"        this.{lbl}.Text = \"{EscapeCs(f.Caption)}\";");
            if (!f.Visible) searchConfig.AppendLine($"        this.{lbl}.Visible = false;");
            searchConfig.AppendLine($"        this.panHeader.Controls.Add(this.{lbl});");

            searchConfig.AppendLine($"        this.{field}.Location = new System.Drawing.Point({x + labelWidth}, 20);");
            searchConfig.AppendLine($"        this.{field}.Name = \"{field}\";");
            searchConfig.AppendLine($"        this.{field}.Size = new System.Drawing.Size({editWidth}, 20);");
            if (f.ControlKind is "COMBO" or "POP")
                searchConfig.AppendLine($"        this.{field}.LookupKey = \"{EscapeCs(f.LookupKey ?? string.Empty)}\";");
            if (f.Required) searchConfig.AppendLine($"        this.{field}.Required = true;");
            if (!f.Visible) searchConfig.AppendLine($"        this.{field}.Visible = false;");
            searchConfig.AppendLine($"        this.panHeader.Controls.Add(this.{field});");
            x += labelWidth + editWidth + fieldSpacing;
        }
        return (searchNew.ToString(), searchConfig.ToString(), searchDecl.ToString());
    }

    // ParamName이 비어있는 항목(화면표시 전용)은 조회 시 아예 안 보낸다 - QUERY_PARAMS 딕셔너리에
    // 그 키 자체가 없어야 한다(빈 문자열/null을 보내는 것과 다름).
    private static string BuildQueryParamsBlock(ScreenGenSpec spec)
    {
        var sb = new StringBuilder();
        foreach (var f in spec.SearchFields)
        {
            if (string.IsNullOrWhiteSpace(f.ParamName)) continue;
            sb.AppendLine($"            [\"{f.ParamName}\"] = {SearchFieldReadExpr(f)},");
        }
        return sb.ToString();
    }

    private static string BuildSaveParamsBlock(SaveAction saveAction)
    {
        var sb = new StringBuilder();
        foreach (var kvp in saveAction.SaveParamColumnMap)
            sb.AppendLine($"                [\"p_{kvp.Key}\"] = ProcData.Str(row, \"{kvp.Value}\", version),");
        foreach (var unmapped in saveAction.UnmappedSaveParams())
            sb.AppendLine($"                [\"p_{unmapped}\"] = null, // TODO: 값 채우기");
        return sb.ToString();
    }

    /// <summary>그리드 컬럼 단순판(TEXT 전용) - Caption/FieldName/Name/Visible/VisibleIndex/Width만
    /// 설정한다. 체크박스/LookUp이 필요하면 생성 후 직접 ColumnEdit을 붙인다.</summary>
    /// <summary>MasterSubGrid/MasterFormSubGrid/MasterOneSheet 공용 grd1/grd2/grd3
    /// 컬럼 채우기. CHECK/COMBO 리포지토리 아이템 배선까지 SingleGrid의 AppendGridColumns와
    /// 동일하게 처리한다 - 예전엔 이 헬퍼가 이름 그대로 "단순" 텍스트 컬럼만 만들어서, COMBO로
    /// 지정한 컬럼(LookUp 연결)이 실제 생성된 화면에선 평범한 텍스트 컬럼으로 나오는 버그가
    /// 있었다(2026-09-07 실제 발견 - frmItem grd2의 fr_unit_cd/to_unit_cd를 COMBO+LookupKey로
    /// 지정했는데 반영 안 됨). gridField("grd1"/"grd2"/"grd3", RepositoryItems를 붙일 GridControl)는
    /// gvwField에서 그대로 유도된다 - 이 코드베이스의 모든 템플릿이 gvwN/grdN 명명 규칙을 따른다.</summary>
    private static void AppendSimpleColumns(List<ColumnSpec> columns, string prefix, string gvwField, StringBuilder newSb, StringBuilder configSb, StringBuilder declSb, bool allowPopupInGrid)
    {
        var gridField = "grd" + gvwField.Substring(3);
        var chkField = $"chkEdit{prefix}";
        var included = columns.Where(c => c.IncludeInGrid).ToList();

        foreach (var c in included)
            newSb.AppendLine($"        this.{prefix}{PascalCase(c.Name)} = new DevExpress.XtraGrid.Columns.GridColumn();");
        AppendCheckEditFieldIfNeeded(newSb, columns, chkField);
        AppendSpinEditFieldIfNeeded(newSb, columns, $"spinEdit{prefix}");
        AppendDateEditFieldIfNeeded(newSb, columns, $"dateEdit{prefix}");
        AppendComboEditFieldsDecl(newSb, columns, prefix);
        AppendPopupEditFieldsDecl(newSb, columns, prefix, allowPopupInGrid);

        AppendGridColumns(configSb, columns, prefix, chkField, allowPopupInGrid);
        configSb.AppendLine($"        this.{gvwField}.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {{");
        configSb.AppendLine("            " + string.Join(",\r\n            ", included.Select(c => $"this.{prefix}{PascalCase(c.Name)}")) + "});");
        // GridColumn.MarkRequired()(빈 셀만 배경색 강조)는 컬럼이 View.Columns에 실제로 추가된
        // "다음"에만 호출할 수 있다(RequiredFieldExtensions 주석 참고) - 그래서 AddRange 바로
        // 다음 줄에 놓는다(2026-09-09 요청). allowPopupInGrid가 false인 슬롯(Role=Query로 생성되는
        // 조회전용 그리드, 예: grd1/grd2)에서는 POP과 같은 이유로 건너뛴다 - 편집이 안 되는
        // 그리드에 "필수입력" 강조를 표시해봐야 사용자가 채울 방법이 없어 의미가 없다(2026-09-09,
        // 사장님 지적 - "roll이 query인 컨트롤에는 필수입력 여부를 적용할 필요가 없어").
        if (allowPopupInGrid)
        {
            foreach (var c in included.Where(c => c.Required))
                configSb.AppendLine($"        this.{prefix}{PascalCase(c.Name)}.MarkRequired();");
        }
        AppendRepositoryItemsAddRange(configSb, gridField, columns, chkField, prefix, allowPopupInGrid);

        foreach (var c in included)
            declSb.AppendLine($"    private DevExpress.XtraGrid.Columns.GridColumn {prefix}{PascalCase(c.Name)};");
        if (HasCheckColumn(columns))
            declSb.AppendLine($"    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit {chkField};");
        if (HasNumberColumn(columns))
            declSb.AppendLine($"    private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spinEdit{prefix};");
        if (HasDateColumn(columns))
            declSb.AppendLine($"    private WYNLAB.Base.Controls.DateColumnEdit dateEdit{prefix};");
        foreach (var c in ComboColumns(columns))
            declSb.AppendLine($"    private LookUpColumnEdit {ComboEditField(prefix, c)};");
        foreach (var c in PopColumns(columns, allowPopupInGrid))
            declSb.AppendLine($"    private PopupLookupColumnEdit {PopEditField(prefix, c)};");
    }

    /// <summary>"// @AI_BUILDER:BEGIN 이름" ~ "// @AI_BUILDER:END 이름" 구간(마커 줄 포함)을
    /// 통째로 걷어내고 그 자리에 실제 생성된 내용을 끼워 넣는다 - 마커 자체는 결과물에 안 남는다
    /// (이제 템플릿이 아니라 진짜 화면 코드이므로).</summary>
    private static string SpliceBlock(string text, string markerName, string replacement)
    {
        var pattern = $@"[ \t]*// @AI_BUILDER:BEGIN {markerName}\r?\n.*?[ \t]*// @AI_BUILDER:END {markerName}\r?\n";
        var match = Regex.Match(text, pattern, RegexOptions.Singleline);
        if (!match.Success)
            throw new InvalidOperationException($"템플릿에서 마커를 찾을 수 없습니다: {markerName}");
        return text.Substring(0, match.Index) + replacement + text.Substring(match.Index + match.Length);
    }

    private static string ApplyTokens(string text, ScreenGenSpec spec, string templateClassName)
    {
        text = text.Replace("__MENU_CAPTION__", EscapeCs(spec.MenuCaption));
        text = text.Replace("__QUERY_PROC__", spec.QueryProc);
        text = text.Replace("__SAVE_PROC__", spec.SaveProc ?? string.Empty);
        text = text.Replace("__DETAIL_WORK_TYPE__", spec.DetailWorkType ?? "Q1");
        text = text.Replace("__DETAIL_KEY_PARAM__", spec.DetailKeyParam ?? string.Empty);
        text = text.Replace("__MASTER_KEY_COLUMN__", spec.MasterKeyColumn ?? string.Empty);
        text = text.Replace("namespace WYNLAB.TEMPLATE;", $"namespace WYNLAB.{spec.Module};");
        text = text.Replace(templateClassName, spec.ScreenClassNm);
        return text;
    }

    private static string NextMigrationPath(string repoRoot, string screenClassNm)
    {
        var dbDir = Path.Combine(repoRoot, "00.DEV", "04.Database");
        var next = Directory.GetFiles(dbDir, "*.sql")
            .Select(f => Path.GetFileName(f))
            .Select(f => f.Length >= 3 && int.TryParse(f.Substring(0, 3), out var n) ? n : 0)
            .DefaultIfEmpty(0)
            .Max() + 1;
        return Path.Combine(dbDir, $"{next:000}_{screenClassNm}_Menu.sql");
    }

    private static bool HasCheckColumn(List<ColumnSpec> cols) => cols.Any(c => c.IncludeInGrid && c.ControlKind == "CHECK");
    private static bool HasNumberColumn(List<ColumnSpec> cols) => cols.Any(c => c.IncludeInGrid && c.ControlKind == "NUMBER");
    private static bool HasDateColumn(List<ColumnSpec> cols) => cols.Any(c => c.IncludeInGrid && c.ControlKind == "DTE");
    private static IEnumerable<ColumnSpec> ComboColumns(List<ColumnSpec> cols) => cols.Where(c => c.IncludeInGrid && c.ControlKind == "COMBO");

    // COMBO(LookUpColumnEdit)는 조회전용 그리드에서도 코드값을 명칭으로 바꿔 보여주는 실질적 효과가
    // 있어서 Role과 무관하게 항상 붙인다. 반면 POP(PopupLookupColumnEdit)는 ButtonEdit 기반이라
    // "..." 버튼을 눌러야만 의미가 있는데, 그 버튼은 그리드가 편집 가능(Role=Edit)할 때만 나타난다
    // - 조회전용 그리드(grd1 등)에 붙여봐야 아무 동작도 안 하면서 리포지토리 아이템만 늘어난다
    // (2026-09-07, 사장님 지적 - "grd1은 조회용이라 팝업 걸 필요없다. panData에만 필요"). 그래서
    // allowPopupInGrid가 false인 슬롯(각 템플릿에서 Role=Query로 생성되는 grd1/grd2)에서는 POP
    // 컬럼도 그냥 TEXT처럼 취급한다 - panData 쪽 DetailFieldType 등은 이 값과 무관하게 항상 POP를
    // 그대로 반영한다(같은 masterColumns를 grd1/panData 양쪽이 같이 쓰지만 panData는 폼이라 항상
    // 상호작용 가능하므로 이 제한이 필요 없다).
    private static IEnumerable<ColumnSpec> PopColumns(List<ColumnSpec> cols, bool allowPopupInGrid) =>
        allowPopupInGrid ? cols.Where(c => c.IncludeInGrid && c.ControlKind == "POP") : Enumerable.Empty<ColumnSpec>();

    private static void AppendCheckEditFieldIfNeeded(StringBuilder sb, List<ColumnSpec> cols, string fieldName)
    {
        if (HasCheckColumn(cols))
            sb.AppendLine($"        this.{fieldName} = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();");
    }

    // NUMBER/DTE는 컬럼마다 설정이 다르지 않아서(LookUp처럼 컬럼별 키가 필요 없음) CHECK처럼
    // 그리드 하나당 공유 리포지토리 아이템 하나씩만 만든다.
    private static void AppendSpinEditFieldIfNeeded(StringBuilder sb, List<ColumnSpec> cols, string fieldName)
    {
        if (HasNumberColumn(cols))
            sb.AppendLine($"        this.{fieldName} = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();");
    }

    private static void AppendDateEditFieldIfNeeded(StringBuilder sb, List<ColumnSpec> cols, string fieldName)
    {
        if (HasDateColumn(cols))
            sb.AppendLine($"        this.{fieldName} = new WYNLAB.Base.Controls.DateColumnEdit();");
    }

    // COMBO/POP 컬럼은 컬럼마다 LookupKey가 다를 수 있어서(품목/부서/거래처 등) chkEdit처럼 하나를
    // 공유하지 않고, 컬럼당 인스턴스를 하나씩 만든다 - frmMenuAuth의 lookUpColumnEdit1/2와 같은 패턴.
    private static string ComboEditField(string colPrefix, ColumnSpec c) => $"lookUp{colPrefix}{PascalCase(c.Name)}";
    private static string PopEditField(string colPrefix, ColumnSpec c) => $"pop{colPrefix}{PascalCase(c.Name)}";

    private static void AppendComboEditFieldsDecl(StringBuilder sb, List<ColumnSpec> cols, string colPrefix)
    {
        foreach (var c in ComboColumns(cols))
            sb.AppendLine($"        this.{ComboEditField(colPrefix, c)} = new WYNLAB.Base.Controls.LookUpColumnEdit();");
    }

    private static void AppendPopupEditFieldsDecl(StringBuilder sb, List<ColumnSpec> cols, string colPrefix, bool allowPopupInGrid)
    {
        foreach (var c in PopColumns(cols, allowPopupInGrid))
            sb.AppendLine($"        this.{PopEditField(colPrefix, c)} = new WYNLAB.Base.Controls.PopupLookupColumnEdit();");
    }

    private static void AppendRepositoryItemsAddRange(StringBuilder sb, string gridField, List<ColumnSpec> cols, string chkField, string colPrefix, bool allowPopupInGrid)
    {
        var items = new List<string>();
        if (HasCheckColumn(cols)) items.Add($"this.{chkField}");
        if (HasNumberColumn(cols)) items.Add($"this.spinEdit{colPrefix}");
        if (HasDateColumn(cols)) items.Add($"this.dateEdit{colPrefix}");
        items.AddRange(ComboColumns(cols).Select(c => $"this.{ComboEditField(colPrefix, c)}"));
        items.AddRange(PopColumns(cols, allowPopupInGrid).Select(c => $"this.{PopEditField(colPrefix, c)}"));
        if (items.Count == 0) return;

        sb.AppendLine($"        this.{gridField}.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {{ {string.Join(", ", items)}}});");
    }

    private static void AppendGridColumns(StringBuilder sb, List<ColumnSpec> columns, string colPrefix, string chkField, bool allowPopupInGrid)
    {
        var spinField = $"spinEdit{colPrefix}";
        var dateField = $"dateEdit{colPrefix}";
        var idx = 0;
        foreach (var c in columns.Where(c => c.IncludeInGrid))
        {
            var field = $"{colPrefix}{PascalCase(c.Name)}";
            sb.AppendLine("        //");
            sb.AppendLine($"        // {field}");
            sb.AppendLine("        //");
            sb.AppendLine($"        this.{field}.Caption = \"{EscapeCs(c.Caption)}\";");
            sb.AppendLine($"        this.{field}.FieldName = \"{c.Name}\";");
            sb.AppendLine($"        this.{field}.Name = \"{field}\";");
            if (c.IsKey)
                sb.AppendLine($"        this.{field}.OptionsColumn.AllowEdit = false;");
            if (c.ControlKind == "CHECK")
                sb.AppendLine($"        this.{field}.ColumnEdit = this.{chkField};");
            else if (c.ControlKind == "COMBO")
                sb.AppendLine($"        this.{field}.ColumnEdit = this.{ComboEditField(colPrefix, c)};");
            else if (c.ControlKind == "NUMBER")
                sb.AppendLine($"        this.{field}.ColumnEdit = this.{spinField};");
            else if (c.ControlKind == "DTE")
                sb.AppendLine($"        this.{field}.ColumnEdit = this.{dateField};");
            else if (c.ControlKind == "POP" && allowPopupInGrid)
                sb.AppendLine($"        this.{field}.ColumnEdit = this.{PopEditField(colPrefix, c)};");
            sb.AppendLine($"        this.{field}.Visible = true;");
            sb.AppendLine($"        this.{field}.VisibleIndex = {idx};");
            sb.AppendLine($"        this.{field}.Width = 100;");
            idx++;
        }
        if (HasCheckColumn(columns))
        {
            sb.AppendLine("        //");
            sb.AppendLine($"        // {chkField}");
            sb.AppendLine("        //");
            sb.AppendLine($"        this.{chkField}.Name = \"{chkField}\";");
        }
        if (HasNumberColumn(columns))
        {
            sb.AppendLine("        //");
            sb.AppendLine($"        // {spinField}");
            sb.AppendLine("        //");
            sb.AppendLine($"        this.{spinField}.Name = \"{spinField}\";");
        }
        if (HasDateColumn(columns))
        {
            sb.AppendLine("        //");
            sb.AppendLine($"        // {dateField}");
            sb.AppendLine("        //");
            sb.AppendLine($"        this.{dateField}.Name = \"{dateField}\";");
        }
        foreach (var c in ComboColumns(columns))
        {
            var field = ComboEditField(colPrefix, c);
            sb.AppendLine("        //");
            sb.AppendLine($"        // {field}");
            sb.AppendLine("        //");
            sb.AppendLine($"        this.{field}.LookupKey = \"{EscapeCs(c.LookupKey ?? string.Empty)}\";");
            sb.AppendLine($"        this.{field}.Name = \"{field}\";");
        }
        foreach (var c in PopColumns(columns, allowPopupInGrid))
        {
            var field = PopEditField(colPrefix, c);
            sb.AppendLine("        //");
            sb.AppendLine($"        // {field}");
            sb.AppendLine("        //");
            sb.AppendLine($"        this.{field}.LookupKey = \"{EscapeCs(c.LookupKey ?? string.Empty)}\";");
            sb.AppendLine($"        this.{field}.Name = \"{field}\";");
        }
    }

    // 트리(tree1) 컬럼 - grd1처럼 항상 조회전용이라 POP은 TEXT로 축소한다(PopColumns 주석과 같은
    // 원칙, allowPopupInGrid 파라미터 자체가 없다). TreeListColumn/TreeList.RepositoryItems API가
    // GridColumn/GridControl과 구조적으로 거의 같아서(Caption/FieldName/ColumnEdit/Visible/
    // VisibleIndex/Width, RepositoryItems 컬렉션) AppendGridColumns/AppendRepositoryItemsAddRange와
    // 같은 원리로 짠다 - 다만 트리는 컬럼 그리드가 하나(colPrefix "Tree" 고정)뿐이라 더 단순하다.
    private static void AppendTreeColumns(List<ColumnSpec> columns, StringBuilder newSb, StringBuilder configSb, StringBuilder declSb)
    {
        var included = columns.Where(c => c.IncludeInGrid).ToList();
        const string chkField = "chkEditTree";
        const string spinField = "spinEditTree";
        const string dateField = "dateEditTree";

        foreach (var c in included)
            newSb.AppendLine($"        this.treeCol{PascalCase(c.Name)} = new DevExpress.XtraTreeList.Columns.TreeListColumn();");
        if (HasCheckColumn(included)) newSb.AppendLine($"        this.{chkField} = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();");
        if (HasNumberColumn(included)) newSb.AppendLine($"        this.{spinField} = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();");
        if (HasDateColumn(included)) newSb.AppendLine($"        this.{dateField} = new WYNLAB.Base.Controls.DateColumnEdit();");
        foreach (var c in ComboColumns(included))
            newSb.AppendLine($"        this.{ComboEditField("Tree", c)} = new WYNLAB.Base.Controls.LookUpColumnEdit();");

        var idx = 0;
        foreach (var c in included)
        {
            var field = $"treeCol{PascalCase(c.Name)}";
            configSb.AppendLine("        //");
            configSb.AppendLine($"        // {field}");
            configSb.AppendLine("        //");
            configSb.AppendLine($"        this.{field}.Caption = \"{EscapeCs(c.Caption)}\";");
            configSb.AppendLine($"        this.{field}.FieldName = \"{c.Name}\";");
            configSb.AppendLine($"        this.{field}.Name = \"{field}\";");
            if (c.ControlKind == "CHECK") configSb.AppendLine($"        this.{field}.ColumnEdit = this.{chkField};");
            else if (c.ControlKind == "COMBO") configSb.AppendLine($"        this.{field}.ColumnEdit = this.{ComboEditField("Tree", c)};");
            else if (c.ControlKind == "NUMBER") configSb.AppendLine($"        this.{field}.ColumnEdit = this.{spinField};");
            else if (c.ControlKind == "DTE") configSb.AppendLine($"        this.{field}.ColumnEdit = this.{dateField};");
            configSb.AppendLine($"        this.{field}.Visible = true;");
            configSb.AppendLine($"        this.{field}.VisibleIndex = {idx};");
            configSb.AppendLine($"        this.{field}.Width = 150;");
            idx++;
        }
        configSb.AppendLine("        this.tree1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {");
        configSb.AppendLine("            " + string.Join(",\r\n            ", included.Select(c => $"this.treeCol{PascalCase(c.Name)}")) + "});");

        var repoItems = new List<string>();
        if (HasCheckColumn(included)) repoItems.Add($"this.{chkField}");
        if (HasNumberColumn(included)) repoItems.Add($"this.{spinField}");
        if (HasDateColumn(included)) repoItems.Add($"this.{dateField}");
        repoItems.AddRange(ComboColumns(included).Select(c => $"this.{ComboEditField("Tree", c)}"));
        if (repoItems.Count > 0)
            configSb.AppendLine($"        this.tree1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {{ {string.Join(", ", repoItems)}}});");

        if (HasCheckColumn(included)) configSb.AppendLine($"        this.{chkField}.Name = \"{chkField}\";");
        if (HasNumberColumn(included)) configSb.AppendLine($"        this.{spinField}.Name = \"{spinField}\";");
        if (HasDateColumn(included)) configSb.AppendLine($"        this.{dateField}.Name = \"{dateField}\";");
        foreach (var c in ComboColumns(included))
        {
            var field = ComboEditField("Tree", c);
            configSb.AppendLine($"        this.{field}.LookupKey = \"{EscapeCs(c.LookupKey ?? string.Empty)}\";");
            configSb.AppendLine($"        this.{field}.Name = \"{field}\";");
        }

        foreach (var c in included)
            declSb.AppendLine($"    private DevExpress.XtraTreeList.Columns.TreeListColumn treeCol{PascalCase(c.Name)};");
        if (HasCheckColumn(included)) declSb.AppendLine($"    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit {chkField};");
        if (HasNumberColumn(included)) declSb.AppendLine($"    private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit {spinField};");
        if (HasDateColumn(included)) declSb.AppendLine($"    private WYNLAB.Base.Controls.DateColumnEdit {dateField};");
        foreach (var c in ComboColumns(included))
            declSb.AppendLine($"    private LookUpColumnEdit {ComboEditField("Tree", c)};");
    }

    // panData 상세컨트롤 - CHECK는 CheckBoxWyn, COMBO는 LookUpEditWyn(LookupKey로 연결),
    // NUMBER는 SpinEditWyn, DTE는 DateEditWyn, POP는 PopupLookupEditWyn(LookupKey=sysPopUpM
    // popup_key - COMBO와 프로퍼티명이 우연히 같아서 같은 LookupKey 칸을 그대로 재사용한다,
    // 2026-09-07). MEMO는 MemoEditWyn(비고/설명처럼 여러 줄 입력, 2026-09-09) - .Text로
    // 읽고/쓰고/비우는 건 TEXT와 완전히 같아서 Assign/Clear/ReadExpr은 별도 분기 없이 기본(_)
    // 케이스를 그대로 탄다. 그 외(TEXT)는 TextEditWyn.
    private static string DetailFieldName(ColumnSpec c) => c.ControlKind switch
    {
        "CHECK" => "chkDetail" + PascalCase(c.Name),
        "COMBO" => "cboDetail" + PascalCase(c.Name),
        "NUMBER" => "numDetail" + PascalCase(c.Name),
        "DTE" => "dteDetail" + PascalCase(c.Name),
        "POP" => "popDetail" + PascalCase(c.Name),
        "MEMO" => "memDetail" + PascalCase(c.Name),
        _ => "txtDetail" + PascalCase(c.Name)
    };

    private static string DetailFieldType(ColumnSpec c) => c.ControlKind switch
    {
        "CHECK" => "CheckBoxWyn",
        "COMBO" => "LookUpEditWyn",
        "NUMBER" => "SpinEditWyn",
        "DTE" => "DateEditWyn",
        "POP" => "PopupLookupEditWyn",
        "MEMO" => "MemoEditWyn",
        _ => "TextEditWyn"
    };

    private static string DetailFieldDecl(ColumnSpec c) =>
        $"{DetailFieldName(c)} = new WYNLAB.Base.Controls.{DetailFieldType(c)}();";

    private static string DetailFieldAssign(ColumnSpec c, string valueExpr) => c.ControlKind switch
    {
        "CHECK" => $"{DetailFieldName(c)}.Checked = {valueExpr}?.ToString() == \"Y\";",
        "COMBO" => $"{DetailFieldName(c)}.EditValue = {valueExpr}?.ToString() ?? string.Empty;",
        "NUMBER" => $"{DetailFieldName(c)}.EditValue = decimal.TryParse({valueExpr}?.ToString(), out var num{PascalCase(c.Name)}) ? num{PascalCase(c.Name)} : (decimal?)null;",
        "DTE" => $"{DetailFieldName(c)}.YyyyMmDd = {valueExpr}?.ToString();",
        "POP" => $"{DetailFieldName(c)}.Text = {valueExpr}?.ToString() ?? string.Empty;",
        _ => $"{DetailFieldName(c)}.Text = {valueExpr}?.ToString() ?? string.Empty;"
    };

    // 신규입력 진입 시 사업장(acc_id/acc_cd) 콤보는 매번 사람이 고르게 하지 않고 로그인 세션의
    // 사업장을 기본값으로 채운다 - 모든 입력화면의 표준 동작(2026-09-09 frmEmp에서 처음 요청,
    // 2026-09-11 "모든 입력 프로그램의 기본기능이니 기억해둬"로 전 화면 확정). 필요하면 사용자가
    // 직접 다른 사업장으로 바꿀 수 있으니 여기서는 초기값만 채운다. 컬럼명이 acc_id/acc_cd인
    // COMBO에만 적용 - 그 외 COMBO는 그대로 빈 값으로 시작한다.
    private static bool IsAccColumn(ColumnSpec c) =>
        c.Name.Equals("acc_id", StringComparison.OrdinalIgnoreCase) ||
        c.Name.Equals("acc_cd", StringComparison.OrdinalIgnoreCase);

    private static string DetailFieldClear(ColumnSpec c) => c.ControlKind switch
    {
        "CHECK" => $"{DetailFieldName(c)}.Checked = false;",
        "COMBO" when IsAccColumn(c) => $"{DetailFieldName(c)}.EditValue = Session.AccId?.ToString() ?? string.Empty;",
        "COMBO" => $"{DetailFieldName(c)}.EditValue = string.Empty;",
        "NUMBER" => $"{DetailFieldName(c)}.EditValue = null;",
        "DTE" => $"{DetailFieldName(c)}.YyyyMmDd = null;",
        "POP" => $"{DetailFieldName(c)}.Text = string.Empty;",
        _ => $"{DetailFieldName(c)}.Text = string.Empty;"
    };

    // 개발자용 마우스오버 툴팁(BindingField)이 panData 개별 컨트롤에서도 뜨려면 생성자에서
    // Tag를 미리 채워둬야 한다(BaseForm.ApplyBindingFieldTooltips 참고) - 손으로 만든 화면
    // 여러 개(frmDept/frmEMP 등)에 이게 빠져있던 걸 2026-09-12에 발견해서 템플릿에도 추가했다.
    private static string DetailFieldTag(ColumnSpec c) => $"{DetailFieldName(c)}.Tag = new BindingFieldTag(\"{c.Name}\");";

    // NUMBER/DTE는 빈값일 때 반드시 C# null을 보내야 한다 - string.Empty("")를 보내면 서버가
    // SqlParameter를 NVARCHAR로 바인딩해서 프로시저의 BIGINT/DATETIME 파라미터에 암시적 변환을
    // 시도하다 "nvarchar을(를) numeric(으)로 변환하는 중 오류" 예외로 그대로 죽는다(2026-09-07
    // 실제 발견 - frmItem333의 dept_id를 비워두고 저장하면 재현됨). 이 예외는 프로시저 BEGIN TRY
    // 진입 전(RPC 파라미터 바인딩 시점)에 터져서 프로시저 자체의 오류처리도 못 거치고 원본
    // SqlException이 그대로 화면까지 올라온다. null이면 Dapper가 DBNull로 보내서 BIGINT/DATETIME
    // 파라미터의 기본값(NULL)이 그대로 적용된다. COMBO/TEXT는 대상 컬럼이 NVARCHAR라 빈 문자열도
    // 안전하므로 그대로 둔다.
    private static string DetailFieldReadExpr(ColumnSpec c) => c.ControlKind switch
    {
        "CHECK" => $"{DetailFieldName(c)}.Checked ? \"Y\" : \"N\"",
        "COMBO" => $"{DetailFieldName(c)}.EditValue?.ToString() ?? string.Empty",
        "NUMBER" => $"{DetailFieldName(c)}.EditValue?.ToString()",
        "DTE" => $"{DetailFieldName(c)}.YyyyMmDd",
        "POP" => $"{DetailFieldName(c)}.Text",
        _ => $"{DetailFieldName(c)}.Text"
    };

    // 검색조건(panHeader) 컨트롤 - DetailField*와 같은 원칙(ControlKind별 실제 컨트롤 타입/필드명/
    // 읽기표현식)이지만 "Detail" 접두어가 없다(예전부터 검색조건 텍스트박스는 txt{Pascal}이라
    // 그 관례를 그대로 따른다 - panData 필드는 txtDetail{Pascal}이라 이름이 겹치지 않는다).
    // 원본 데이터에서 값을 채우는 개념이 없으므로(사용자가 직접 입력/선택하는 검색조건)
    // Assign/Clear는 없다 - DetailFieldType은 컨트롤 타입 매핑이 완전히 같아 그대로 재사용한다.
    private static string SearchFieldName(ColumnSpec c) => c.ControlKind switch
    {
        "CHECK" => "chk" + PascalCase(c.Name),
        "COMBO" => "cbo" + PascalCase(c.Name),
        "NUMBER" => "num" + PascalCase(c.Name),
        "DTE" => "dte" + PascalCase(c.Name),
        "POP" => "pop" + PascalCase(c.Name),
        "MEMO" => "mem" + PascalCase(c.Name),
        _ => "txt" + PascalCase(c.Name)
    };

    private static string SearchFieldDecl(ColumnSpec c) =>
        $"{SearchFieldName(c)} = new WYNLAB.Base.Controls.{DetailFieldType(c)}();";

    private static string SearchFieldReadExpr(ColumnSpec c) => c.ControlKind switch
    {
        "CHECK" => $"{SearchFieldName(c)}.Checked ? \"Y\" : \"N\"",
        "COMBO" => $"{SearchFieldName(c)}.EditValue?.ToString() ?? string.Empty",
        "NUMBER" => $"{SearchFieldName(c)}.EditValue?.ToString()",
        "DTE" => $"{SearchFieldName(c)}.YyyyMmDd",
        "POP" => $"{SearchFieldName(c)}.Text",
        _ => $"{SearchFieldName(c)}.Text"
    };

    // panData 필드가 전부 세로 한 줄로만 쌓이면 컬럼이 많은 화면(품목 마스터 등 30개 이상)에서
    // 스크롤을 한참 내려야 다음 필드가 보인다(2026-09-07 실제 지적). 필드 수에 따라 자동으로
    // 2~3열로 나눠 배치한다 - 열 안에서는 위→아래로 채우고, 그 열이 다 차면 다음 열로 넘어간다
    // (신문 단 배치와 같은 순서). 적은 필드(12개 이하)는 지금까지처럼 1열 그대로 둔다 - AI
    // Builder가 만드는 기본 배치일 뿐이니 이후 사람이 VS Designer에서 위치/폭을 얼마든지 조정할
    // 수 있다(다른 panData 컨트롤들과 같은 원칙 - "화면개발 협업 모델" 참고).
    private static void AppendPanDataFieldLayout(StringBuilder formConfig, List<ColumnSpec> masterColumns)
    {
        const int rowHeight = 28;
        const int columnSpacing = 360; // 라벨(x=16)+입력칸(x=120,width=220) 한 벌이 차지하는 가로폭 + 다음 열과의 여백
        var columns = masterColumns.Count <= 12 ? 1 : masterColumns.Count <= 24 ? 2 : 3;
        var rowsPerColumn = (int)Math.Ceiling(masterColumns.Count / (double)columns);

        for (var i = 0; i < masterColumns.Count; i++)
        {
            var c = masterColumns[i];
            var col = i / rowsPerColumn;
            var rowInColumn = i % rowsPerColumn;
            var x = col * columnSpacing;
            var y = 16 + rowInColumn * rowHeight;

            var lbl = $"lblDetail{PascalCase(c.Name)}";
            var edit = DetailFieldName(c);
            formConfig.AppendLine($"        this.{lbl}.Appearance.Font = new System.Drawing.Font(\"맑은 고딕\", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));");
            formConfig.AppendLine($"        this.{lbl}.Appearance.Options.UseFont = true;");
            formConfig.AppendLine($"        this.{lbl}.Location = new System.Drawing.Point({16 + x}, {y + 3});");
            formConfig.AppendLine($"        this.{lbl}.Name = \"{lbl}\";");
            formConfig.AppendLine($"        this.{lbl}.Text = \"{EscapeCs(c.Caption)}\";");
            formConfig.AppendLine($"        this.{edit}.Location = new System.Drawing.Point({120 + x}, {y});");
            formConfig.AppendLine($"        this.{edit}.Name = \"{edit}\";");
            formConfig.AppendLine($"        this.{edit}.Size = new System.Drawing.Size(220, 20);");
            if (c.ControlKind is "COMBO" or "POP")
                formConfig.AppendLine($"        this.{edit}.LookupKey = \"{EscapeCs(c.LookupKey ?? string.Empty)}\";");
            if (c.Required) formConfig.AppendLine($"        this.{edit}.Required = true;");
            formConfig.AppendLine($"        this.panData.Controls.Add(this.{lbl});");
            formConfig.AppendLine($"        this.panData.Controls.Add(this.{edit});");
        }
    }

    // ================================================================================
    // csproj / migration
    // ================================================================================
    private static void AppendCsprojEntry(string csprojPath, ScreenGenSpec spec, bool hasResx)
    {
        var xml = File.ReadAllText(csprojPath);
        var marker = "</Project>";
        var idx = xml.LastIndexOf(marker, StringComparison.Ordinal);
        if (idx < 0) throw new InvalidOperationException($"csproj 형식을 인식할 수 없습니다: {csprojPath}");

        // resx가 있으면 frmDept/frmEmp 등 기존 화면과 동일하게 DependentUpon으로 연결한다 - 이게
        // 있어야 ComponentResourceManager(typeof(생성된 클래스))가 리소스를 "네임스페이스.클래스명"
        // 기준으로 찾고, 없으면(폴더 경로 기준 이름으로 묻혀서) SvgIcon이 런타임에 비어버린다.
        var resxEntry = hasResx
            ? $"    <EmbeddedResource Update=\"{spec.ScreenClassNm}\\{spec.ScreenClassNm}.resx\">\r\n" +
              $"      <DependentUpon>{spec.ScreenClassNm}.cs</DependentUpon>\r\n" +
              "    </EmbeddedResource>\r\n"
            : "";

        var entry =
            "  <ItemGroup>\r\n" +
            $"    <Compile Update=\"{spec.ScreenClassNm}\\{spec.ScreenClassNm}.cs\">\r\n" +
            "      <SubType>Form</SubType>\r\n" +
            "    </Compile>\r\n" +
            $"    <Compile Update=\"{spec.ScreenClassNm}\\{spec.ScreenClassNm}.Designer.cs\">\r\n" +
            $"      <DependentUpon>{spec.ScreenClassNm}.cs</DependentUpon>\r\n" +
            "    </Compile>\r\n" +
            resxEntry +
            "  </ItemGroup>\r\n\r\n";

        File.WriteAllText(csprojPath, xml.Substring(0, idx) + entry + xml.Substring(idx));
    }

    private static string TemplateBaseName(TemplateKind kind) => kind switch
    {
        TemplateKind.SingleGrid => "TplSingleGrid",
        TemplateKind.MasterSubGrid => "TplMasterSubGrid",
        TemplateKind.MasterFormSubGrid => "TplMasterFormSubGrid",
        TemplateKind.MasterOneSheet => "TplMasterOneSheet",
        TemplateKind.TreeMasterSubGrid => "TplTreeMasterSubGrid",
        _ => throw new InvalidOperationException($"알 수 없는 템플릿: {kind}")
    };

    /// <summary>템플릿 원본에 SvgIcon 같은 바이너리 리소스가 든 .resx가 있으면(TplSingleGrid.resx처럼)
    /// 생성 화면 폴더에도 화면 클래스명으로 그대로 복사한다. 리소스 키(sectionHeaderWyn1.SvgIcon 등)는
    /// 컨트롤 필드명 기준이고 클래스명과 무관하므로 내용은 손댈 필요가 없다 - 파일명만 바꿔서 복사하면
    /// 디자인(아이콘 포함)이 생성 화면에도 그대로 재현된다.</summary>
    private static bool CopyTemplateResxIfExists(string repoRoot, ScreenGenSpec spec, string screenDir)
    {
        var templateDir = Path.Combine(repoRoot, "99.SOURCE", "TEMPLATE", "WYNLAB.TEMPLATE");
        var srcResx = Path.Combine(templateDir, $"{TemplateBaseName(spec.Kind)}.resx");
        if (!File.Exists(srcResx)) return false;

        File.Copy(srcResx, Path.Combine(screenDir, $"{spec.ScreenClassNm}.resx"));
        return true;
    }

    /// <summary>MENU_ID는 IDENTITY라 여기서 채번하지 않는다 - 중복 판단은 이제 코드값이 아니라
    /// MODULE+SCREEN_CLASS_NM 조합(리플렉션 로딩 키, ShellForm.OpenMenuForm과 동일)으로 한다.
    /// 표준 감사컬럼(reg_user_id 등)이 NOT NULL이라 스크립트 실행 계정을 그대로 채워 넣는다.</summary>
    private static string BuildMenuSql(ScreenGenSpec spec)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"-- AI Builder가 자동 생성 - {spec.ScreenClassNm} 화면 메뉴 등록.");
        sb.AppendLine("-- 적용 전 검토 필요(다른 마이그레이션과 동일한 규칙) - 자동 실행되지 않는다.");
        sb.AppendLine();
        sb.AppendLine($"IF NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MODULE = '{spec.Module}' AND SCREEN_CLASS_NM = '{spec.ScreenClassNm}')");
        sb.AppendLine("BEGIN");
        sb.AppendLine("    INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)");
        sb.AppendLine($"    VALUES (N'{EscapeSql(spec.MenuCaption)}', {(spec.UpperMenuId?.ToString() ?? "NULL")}, 3, 'FORM', '{spec.Module}', '{spec.ScreenClassNm}', '{spec.ProcPrefix}', 10, 'Y', SUSER_SNAME(), GETDATE());");
        sb.AppendLine("END");
        sb.AppendLine("GO");
        return sb.ToString();
    }

    private static string EscapeCs(string s) => s.Replace("\"", "\\\"");
    private static string EscapeSql(string s) => s.Replace("'", "''");

    /// <summary>snake_case(또는 소문자) -> PascalCase. "user_nm" -> "UserNm".</summary>
    public static string PascalCase(string name)
    {
        var parts = name.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
        return string.Concat(parts.Select(p => char.ToUpperInvariant(p[0]) + p.Substring(1).ToLowerInvariant()));
    }
}
