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
/// MasterFormSubGrid의 grd2(하위그리드)는 조회전용으로만 생성한다(기초코드등록의 소분류
/// 그리드처럼 별도 저장프로시저/파라미터매핑이 필요한데, AI Builder 1단계는 저장프로시저를
/// 화면당 하나만 받기 때문 - 하위그리드 편집/저장이 필요하면 생성 후 직접 추가한다).</summary>
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
            TemplateKind.MasterFormTabGrid => GenerateMasterFormTabGridFromTemplate(spec, repoRoot),
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
        var saveAction = SaveActionForSlot(spec, SaveActionScope.Detail, "grd1");

        // ---- 검색조건(panHeader) ----
        var searchNew = new StringBuilder();
        var searchDecl = new StringBuilder();
        foreach (var p in spec.SearchParams())
        {
            searchNew.AppendLine($"        this.txt{PascalCase(p)} = new WYNLAB.Base.Controls.TextEditWyn();");
            searchDecl.AppendLine($"    private TextEditWyn txt{PascalCase(p)};");
        }
        var searchConfig = new StringBuilder();
        var x = 16;
        foreach (var p in spec.SearchParams())
        {
            var field = $"txt{PascalCase(p)}";
            searchConfig.AppendLine($"        this.{field}.Location = new System.Drawing.Point({x}, 20);");
            searchConfig.AppendLine($"        this.{field}.Name = \"{field}\";");
            searchConfig.AppendLine($"        this.{field}.Size = new System.Drawing.Size(150, 20);");
            searchConfig.AppendLine($"        this.panHeader.Controls.Add(this.{field});");
            x += 166;
        }

        // ---- 그리드 컬럼(grd1) - 기존 헬퍼(CHECK/COMBO 처리 포함)를 그대로 재사용 ----
        var gridNew = new StringBuilder();
        foreach (var c in masterColumns.Where(c => c.IncludeInGrid))
            gridNew.AppendLine($"        this.col1{PascalCase(c.Name)} = new DevExpress.XtraGrid.Columns.GridColumn();");
        AppendCheckEditFieldIfNeeded(gridNew, masterColumns, "chkEdit1");
        AppendComboEditFieldsDecl(gridNew, masterColumns, "col1");

        var gridConfig = new StringBuilder();
        AppendGridColumns(gridConfig, masterColumns, "col1", "chkEdit1");
        gridConfig.AppendLine("        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {");
        gridConfig.AppendLine("            " + string.Join(",\r\n            ",
            masterColumns.Where(c => c.IncludeInGrid).Select(c => $"this.col1{PascalCase(c.Name)}")) + "});");
        AppendRepositoryItemsAddRange(gridConfig, "grd1", masterColumns, "chkEdit1", "col1");

        var gridDecl = new StringBuilder();
        foreach (var c in masterColumns.Where(c => c.IncludeInGrid))
            gridDecl.AppendLine($"    private DevExpress.XtraGrid.Columns.GridColumn col1{PascalCase(c.Name)};");
        if (HasCheckColumn(masterColumns))
            gridDecl.AppendLine("    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkEdit1;");
        foreach (var c in ComboColumns(masterColumns))
            gridDecl.AppendLine($"    private LookUpColumnEdit {ComboEditField("col1", c)};");

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
        var queryParams = new StringBuilder();
        foreach (var sp in spec.SearchParams())
            queryParams.AppendLine($"            [\"p_{sp}\"] = txt{PascalCase(sp)}.Text,");

        var saveParams = new StringBuilder();
        foreach (var kvp in saveAction.SaveParamColumnMap)
            saveParams.AppendLine($"                [\"p_{kvp.Key}\"] = row[\"{kvp.Value}\", version]?.ToString(),");
        foreach (var unmapped in saveAction.UnmappedSaveParams())
            saveParams.AppendLine($"                [\"p_{unmapped}\"] = null, // TODO: 값 채우기");

        formCs = SpliceBlock(formCs, "QUERY_PARAMS", queryParams.ToString());
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
        AppendSimpleColumns(masterColumns, "colM", "gvw1", masterNew, masterConfig, masterDecl);

        var detailNew = new StringBuilder();
        var detailConfig = new StringBuilder();
        var detailDecl = new StringBuilder();
        AppendSimpleColumns(detailColumns, "colD", "gvw2", detailNew, detailConfig, detailDecl);

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
    // 마스터-폼-서브그리드 = 템플릿 복제+치환 (WYNLAB.TEMPLATE/TplMasterFormSubGrid,
    // 기초코드등록과 같은 형태). grd1/grd2 컬럼은 마스터-서브그리드와 동일하게 TEXT 전용이고,
    // panData(상세폼)만 CHECK/COMBO까지 지원한다(싱글그리드가 이미 쓰던 DetailField* 헬퍼 재사용).
    // ================================================================================
    private static (string DesignerCs, string FormCs) GenerateMasterFormSubGridFromTemplate(ScreenGenSpec spec, string repoRoot)
    {
        var templateDir = Path.Combine(repoRoot, "99.SOURCE", "TEMPLATE", "WYNLAB.TEMPLATE");
        var designerCs = ReadTemplateFile(templateDir, "TplMasterFormSubGrid.Designer.cs");
        var formCs = ReadTemplateFile(templateDir, "TplMasterFormSubGrid.cs");

        var (searchNew, searchConfig, searchDecl) = BuildSearchFieldBlocks(spec);

        var masterColumns = ColumnsForSlot(spec, "grd1");
        var detailColumns = ColumnsForSlot(spec, "grd2");
        var saveAction = SaveActionForSlot(spec, SaveActionScope.Header, "panData");

        var masterNew = new StringBuilder();
        var masterConfig = new StringBuilder();
        var masterDecl = new StringBuilder();
        AppendSimpleColumns(masterColumns, "colM", "gvw1", masterNew, masterConfig, masterDecl);

        var detailNew = new StringBuilder();
        var detailConfig = new StringBuilder();
        var detailDecl = new StringBuilder();
        AppendSimpleColumns(detailColumns, "colD", "gvw2", detailNew, detailConfig, detailDecl);

        // ---- panData(상세폼) - 라벨+입력컨트롤을 세로로 쌓는다. CHECK/COMBO까지 지원. ----
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
        var y = 16;
        foreach (var c in masterColumns)
        {
            var lbl = $"lblDetail{PascalCase(c.Name)}";
            var edit = DetailFieldName(c);
            formConfig.AppendLine($"        this.{lbl}.Location = new System.Drawing.Point(16, {y + 3});");
            formConfig.AppendLine($"        this.{lbl}.Name = \"{lbl}\";");
            formConfig.AppendLine($"        this.{lbl}.Text = \"{EscapeCs(c.Caption)}\";");
            formConfig.AppendLine($"        this.{edit}.Location = new System.Drawing.Point(120, {y});");
            formConfig.AppendLine($"        this.{edit}.Name = \"{edit}\";");
            formConfig.AppendLine($"        this.{edit}.Size = new System.Drawing.Size(220, 20);");
            if (c.ControlKind == "COMBO")
                formConfig.AppendLine($"        this.{edit}.LookupKey = \"{EscapeCs(c.LookupKey ?? string.Empty)}\";");
            formConfig.AppendLine($"        this.panData.Controls.Add(this.{lbl});");
            formConfig.AppendLine($"        this.panData.Controls.Add(this.{edit});");
            y += 28;
        }

        var formAssign = new StringBuilder();
        var formClear = new StringBuilder();
        var formSaveParams = new StringBuilder();
        foreach (var c in masterColumns)
        {
            formAssign.AppendLine($"        {DetailFieldAssign(c, $"row[\"{c.Name}\"]")}");
            formClear.AppendLine($"        {DetailFieldClear(c)}");
        }
        foreach (var kvp in saveAction.SaveParamColumnMap)
        {
            var col = masterColumns.FirstOrDefault(c => c.Name == kvp.Value);
            var accessor = col != null ? DetailFieldReadExpr(col) : $"{DetailFieldName(new ColumnSpec { Name = kvp.Value })}.Text";
            formSaveParams.AppendLine($"            [\"p_{kvp.Key}\"] = {accessor},");
        }
        foreach (var unmapped in saveAction.UnmappedSaveParams())
            formSaveParams.AppendLine($"            [\"p_{unmapped}\"] = null, // TODO: 값 채우기");

        designerCs = SpliceBlock(designerCs, "SEARCH_FIELD_NEW", searchNew);
        designerCs = SpliceBlock(designerCs, "SEARCH_FIELD_CONFIG", searchConfig);
        designerCs = SpliceBlock(designerCs, "SEARCH_FIELD_DECL", searchDecl);
        designerCs = SpliceBlock(designerCs, "MASTER_COLUMN_NEW", masterNew.ToString());
        designerCs = SpliceBlock(designerCs, "MASTER_COLUMN_CONFIG", masterConfig.ToString());
        designerCs = SpliceBlock(designerCs, "MASTER_COLUMN_DECL", masterDecl.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_COLUMN_NEW", detailNew.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_COLUMN_CONFIG", detailConfig.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_COLUMN_DECL", detailDecl.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_FORM_NEW", formNew.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_FORM_CONFIG", formConfig.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_FORM_DECL", formDecl.ToString());

        formCs = SpliceBlock(formCs, "QUERY_PARAMS", BuildQueryParamsBlock(spec));
        formCs = SpliceBlock(formCs, "DETAIL_FORM_ASSIGN", formAssign.ToString());
        formCs = SpliceBlock(formCs, "DETAIL_FORM_CLEAR", formClear.ToString());
        formCs = SpliceBlock(formCs, "DETAIL_FORM_SAVE_PARAMS", formSaveParams.ToString());

        designerCs = ApplyTokens(designerCs, spec, "TplMasterFormSubGrid");
        formCs = ApplyTokens(formCs, spec, "TplMasterFormSubGrid");
        return (designerCs, formCs);
    }

    // ================================================================================
    // 마스터-폼-탭그리드 = 템플릿 복제+치환 (WYNLAB.TEMPLATE/TplMasterFormTabGrid). 위 세 템플릿과
    // 달리 QueryProc/SaveProc(단일 문자열) 대신 spec.QuerySources/SaveActions로 생성한다 - 조회
    // 프로시저 1콜이 레코드셋을 2개(grd2/grd3용) 반환하고, 저장프로시저가 3개(헤더/명세1/명세2)라
    // "쿼리 1개+저장 1개" 가정으로는 표현이 안 되기 때문(거래처+담당자+계좌 같은 1:N:N 구조).
    // ================================================================================
    private static (string DesignerCs, string FormCs) GenerateMasterFormTabGridFromTemplate(ScreenGenSpec spec, string repoRoot)
    {
        var templateDir = Path.Combine(repoRoot, "99.SOURCE", "TEMPLATE", "WYNLAB.TEMPLATE");
        var designerCs = ReadTemplateFile(templateDir, "TplMasterFormTabGrid.Designer.cs");
        var formCs = ReadTemplateFile(templateDir, "TplMasterFormTabGrid.cs");

        var masterQuery = spec.QuerySources.FirstOrDefault(q => q.ResultSetBindings.Any(b => b.TargetSlot == "grd1"))
            ?? throw new InvalidOperationException("QuerySources에 grd1을 채우는 조회가 없습니다(ResultSetBindings에 TargetSlot=\"grd1\" 필요).");
        // grd1/panData 둘 다 이 컬럼목록을 쓴다. 예전(QuerySources/SaveActions로 통합되기 전)에는
        // 화면 상단 "Master Columns" 탭에 별도로 describe해서 채우는 독립된 필드를 썼는데, 그건
        // Query Sources 탭에서 grd1을 다시 describe해도 반영이 안 되는 버그가 있었다(2026-09-04
        // 실제 발견 - grd1/panData가 항상 빈 채로 생성됨). 지금은 QuerySources 하나에서 grd1
        // 바인딩을 직접 찾으므로 그 문제 자체가 구조적으로 사라졌다.
        var masterColumns = masterQuery.ResultSetBindings.First(b => b.TargetSlot == "grd1").Columns;
        var detailQuery = spec.QuerySources.FirstOrDefault(q => !ReferenceEquals(q, masterQuery))
            ?? throw new InvalidOperationException("QuerySources에 grd2/grd3을 채우는 조회(레코드셋 2개짜리)가 없습니다.");
        var grd2Binding = detailQuery.ResultSetBindings.FirstOrDefault(b => b.TargetSlot == "grd2")
            ?? throw new InvalidOperationException("QuerySources 중 grd2로 바인딩된 레코드셋이 없습니다.");
        var grd3Binding = detailQuery.ResultSetBindings.FirstOrDefault(b => b.TargetSlot == "grd3")
            ?? throw new InvalidOperationException("QuerySources 중 grd3으로 바인딩된 레코드셋이 없습니다.");

        var headerSave = spec.SaveActions.FirstOrDefault(a => a.Scope == SaveActionScope.Header && a.SourceSlot == "panData")
            ?? throw new InvalidOperationException("SaveActions에 panData(헤더) 저장 항목이 없습니다.");
        var detail1Save = spec.SaveActions.FirstOrDefault(a => a.Scope == SaveActionScope.Detail && a.SourceSlot == "grd2")
            ?? throw new InvalidOperationException("SaveActions에 grd2 저장 항목이 없습니다.");
        var detail2Save = spec.SaveActions.FirstOrDefault(a => a.Scope == SaveActionScope.Detail && a.SourceSlot == "grd3")
            ?? throw new InvalidOperationException("SaveActions에 grd3 저장 항목이 없습니다.");

        var (searchNew, searchConfig, searchDecl) = BuildSearchFieldBlocks(spec);

        var masterNew = new StringBuilder();
        var masterConfig = new StringBuilder();
        var masterDecl = new StringBuilder();
        AppendSimpleColumns(masterColumns, "colM", "gvw1", masterNew, masterConfig, masterDecl);

        var detail1New = new StringBuilder();
        var detail1Config = new StringBuilder();
        var detail1Decl = new StringBuilder();
        AppendSimpleColumns(grd2Binding.Columns, "colD1", "gvw2", detail1New, detail1Config, detail1Decl);

        var detail2New = new StringBuilder();
        var detail2Config = new StringBuilder();
        var detail2Decl = new StringBuilder();
        AppendSimpleColumns(grd3Binding.Columns, "colD2", "gvw3", detail2New, detail2Config, detail2Decl);

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
        var y = 16;
        foreach (var c in masterColumns)
        {
            var lbl = $"lblDetail{PascalCase(c.Name)}";
            var edit = DetailFieldName(c);
            formConfig.AppendLine($"        this.{lbl}.Location = new System.Drawing.Point(16, {y + 3});");
            formConfig.AppendLine($"        this.{lbl}.Name = \"{lbl}\";");
            formConfig.AppendLine($"        this.{lbl}.Text = \"{EscapeCs(c.Caption)}\";");
            formConfig.AppendLine($"        this.{edit}.Location = new System.Drawing.Point(120, {y});");
            formConfig.AppendLine($"        this.{edit}.Name = \"{edit}\";");
            formConfig.AppendLine($"        this.{edit}.Size = new System.Drawing.Size(220, 20);");
            if (c.ControlKind == "COMBO")
                formConfig.AppendLine($"        this.{edit}.LookupKey = \"{EscapeCs(c.LookupKey ?? string.Empty)}\";");
            formConfig.AppendLine($"        this.panData.Controls.Add(this.{lbl});");
            formConfig.AppendLine($"        this.panData.Controls.Add(this.{edit});");
            y += 28;
        }

        var formAssign = new StringBuilder();
        var formClear = new StringBuilder();
        foreach (var c in masterColumns)
        {
            formAssign.AppendLine($"        {DetailFieldAssign(c, $"row[\"{c.Name}\"]")}");
            formClear.AppendLine($"        {DetailFieldClear(c)}");
        }

        var formSaveParams = new StringBuilder();
        foreach (var kvp in headerSave.SaveParamColumnMap)
        {
            var col = masterColumns.FirstOrDefault(c => c.Name == kvp.Value);
            var accessor = col != null ? DetailFieldReadExpr(col) : $"{DetailFieldName(new ColumnSpec { Name = kvp.Value })}.Text";
            formSaveParams.AppendLine($"            [\"p_{kvp.Key}\"] = {accessor},");
        }
        foreach (var unmapped in headerSave.UnmappedSaveParams())
            formSaveParams.AppendLine($"            [\"p_{unmapped}\"] = null, // TODO: 값 채우기");

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
        designerCs = SpliceBlock(designerCs, "DETAIL_FORM_NEW", formNew.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_FORM_CONFIG", formConfig.ToString());
        designerCs = SpliceBlock(designerCs, "DETAIL_FORM_DECL", formDecl.ToString());

        formCs = SpliceBlock(formCs, "QUERY_PARAMS", BuildQueryParamsBlock(spec));
        formCs = SpliceBlock(formCs, "DETAIL_FORM_ASSIGN", formAssign.ToString());
        formCs = SpliceBlock(formCs, "DETAIL_FORM_CLEAR", formClear.ToString());
        formCs = SpliceBlock(formCs, "DETAIL_FORM_SAVE_PARAMS", formSaveParams.ToString());
        formCs = SpliceBlock(formCs, "DETAIL1_SAVE_PARAMS", BuildDetailSaveParamsBlock(detail1Save));
        formCs = SpliceBlock(formCs, "DETAIL2_SAVE_PARAMS", BuildDetailSaveParamsBlock(detail2Save));

        var generatedHeader = $"// AI Builder가 마스터-폼-탭그리드 템플릿을 복제해서 자동 생성 - {DateTime.Now:yyyy-MM-dd}.\r\n" +
            "// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.\r\n";
        designerCs = SpliceBlock(designerCs, "FILE_HEADER", generatedHeader);
        formCs = SpliceBlock(formCs, "FILE_HEADER", generatedHeader);

        // 이 템플릿은 프로시저가 여러 개라 공용 ApplyTokens(spec.QueryProc/SaveProc 하나씩만 봄)를
        // 안 쓰고 QuerySources/SaveActions에서 직접 채운다.
        string LocalApplyTokens(string text) => text
            .Replace("__MENU_CAPTION__", EscapeCs(spec.MenuCaption))
            .Replace("__QUERY_PROC__", masterQuery.ProcName)
            .Replace("__DETAIL_QUERY_PROC__", detailQuery.ProcName)
            .Replace("__DETAIL_WORK_TYPE__", detailQuery.WorkType)
            .Replace("__SAVE_PROC__", headerSave.ProcName)
            .Replace("__SAVE_PROC_1__", detail1Save.ProcName)
            .Replace("__SAVE_PROC_2__", detail2Save.ProcName)
            .Replace("__DETAIL_KEY_PARAM__", spec.DetailKeyParam ?? string.Empty)
            .Replace("__MASTER_KEY_COLUMN__", spec.MasterKeyColumn ?? string.Empty)
            .Replace("namespace WYNLAB.TEMPLATE;", $"namespace WYNLAB.{spec.Module};")
            .Replace("TplMasterFormTabGrid", spec.ScreenClassNm);

        designerCs = LocalApplyTokens(designerCs);
        formCs = LocalApplyTokens(formCs);

        return (designerCs, formCs);
    }

    /// <summary>SaveAction(Detail scope) 하나의 ParamColumnMap -> "row"/"version" 지역변수를 읽는
    /// ["p_x"] = ProcData.Str(row, "col", version), 줄들. TplMasterFormTabGrid.SaveClick의
    /// (row, version) => new Dictionary&lt;...&gt; { ... } 콜백 안에 그대로 끼워진다.</summary>
    private static string BuildDetailSaveParamsBlock(SaveAction action)
    {
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

    private static (string New, string Config, string Decl) BuildSearchFieldBlocks(ScreenGenSpec spec)
    {
        var searchNew = new StringBuilder();
        var searchDecl = new StringBuilder();
        foreach (var p in spec.SearchParams())
        {
            searchNew.AppendLine($"        this.txt{PascalCase(p)} = new WYNLAB.Base.Controls.TextEditWyn();");
            searchDecl.AppendLine($"    private TextEditWyn txt{PascalCase(p)};");
        }
        var searchConfig = new StringBuilder();
        var x = 16;
        foreach (var p in spec.SearchParams())
        {
            var field = $"txt{PascalCase(p)}";
            searchConfig.AppendLine($"        this.{field}.Location = new System.Drawing.Point({x}, 20);");
            searchConfig.AppendLine($"        this.{field}.Name = \"{field}\";");
            searchConfig.AppendLine($"        this.{field}.Size = new System.Drawing.Size(150, 20);");
            searchConfig.AppendLine($"        this.panHeader.Controls.Add(this.{field});");
            x += 166;
        }
        return (searchNew.ToString(), searchConfig.ToString(), searchDecl.ToString());
    }

    private static string BuildQueryParamsBlock(ScreenGenSpec spec)
    {
        var sb = new StringBuilder();
        foreach (var sp in spec.SearchParams())
            sb.AppendLine($"            [\"p_{sp}\"] = txt{PascalCase(sp)}.Text,");
        return sb.ToString();
    }

    private static string BuildSaveParamsBlock(SaveAction saveAction)
    {
        var sb = new StringBuilder();
        foreach (var kvp in saveAction.SaveParamColumnMap)
            sb.AppendLine($"                [\"p_{kvp.Key}\"] = row[\"{kvp.Value}\", version]?.ToString(),");
        foreach (var unmapped in saveAction.UnmappedSaveParams())
            sb.AppendLine($"                [\"p_{unmapped}\"] = null, // TODO: 값 채우기");
        return sb.ToString();
    }

    /// <summary>그리드 컬럼 단순판(TEXT 전용) - Caption/FieldName/Name/Visible/VisibleIndex/Width만
    /// 설정한다. 체크박스/LookUp이 필요하면 생성 후 직접 ColumnEdit을 붙인다.</summary>
    private static void AppendSimpleColumns(List<ColumnSpec> columns, string prefix, string gvwField, StringBuilder newSb, StringBuilder configSb, StringBuilder declSb)
    {
        var included = columns.Where(c => c.IncludeInGrid).ToList();
        var idx = 0;
        foreach (var c in included)
        {
            var field = $"{prefix}{PascalCase(c.Name)}";
            newSb.AppendLine($"        this.{field} = new DevExpress.XtraGrid.Columns.GridColumn();");
            declSb.AppendLine($"    private DevExpress.XtraGrid.Columns.GridColumn {field};");

            configSb.AppendLine($"        this.{field}.Caption = \"{EscapeCs(c.Caption)}\";");
            configSb.AppendLine($"        this.{field}.FieldName = \"{c.Name}\";");
            configSb.AppendLine($"        this.{field}.Name = \"{field}\";");
            if (c.IsKey)
                configSb.AppendLine($"        this.{field}.OptionsColumn.AllowEdit = false;");
            configSb.AppendLine($"        this.{field}.Visible = true;");
            configSb.AppendLine($"        this.{field}.VisibleIndex = {idx};");
            configSb.AppendLine($"        this.{field}.Width = 100;");
            idx++;
        }
        configSb.AppendLine($"        this.{gvwField}.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {{");
        configSb.AppendLine("            " + string.Join(",\r\n            ", included.Select(c => $"this.{prefix}{PascalCase(c.Name)}")) + "});");
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
    private static IEnumerable<ColumnSpec> ComboColumns(List<ColumnSpec> cols) => cols.Where(c => c.IncludeInGrid && c.ControlKind == "COMBO");

    private static void AppendCheckEditFieldIfNeeded(StringBuilder sb, List<ColumnSpec> cols, string fieldName)
    {
        if (HasCheckColumn(cols))
            sb.AppendLine($"        this.{fieldName} = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();");
    }

    // COMBO 컬럼은 컬럼마다 LookupKey가 다를 수 있어서(품목/부서/거래처 등) chkEdit처럼 하나를
    // 공유하지 않고, 컬럼당 LookUpColumnEdit 인스턴스를 하나씩 만든다 - frmMenuAuth의
    // lookUpColumnEdit1/2와 같은 패턴.
    private static string ComboEditField(string colPrefix, ColumnSpec c) => $"lookUp{colPrefix}{PascalCase(c.Name)}";

    private static void AppendComboEditFieldsDecl(StringBuilder sb, List<ColumnSpec> cols, string colPrefix)
    {
        foreach (var c in ComboColumns(cols))
            sb.AppendLine($"        this.{ComboEditField(colPrefix, c)} = new WYNLAB.Base.Controls.LookUpColumnEdit();");
    }

    private static void AppendRepositoryItemsAddRange(StringBuilder sb, string gridField, List<ColumnSpec> cols, string chkField, string colPrefix)
    {
        var items = new List<string>();
        if (HasCheckColumn(cols)) items.Add($"this.{chkField}");
        items.AddRange(ComboColumns(cols).Select(c => $"this.{ComboEditField(colPrefix, c)}"));
        if (items.Count == 0) return;

        sb.AppendLine($"        this.{gridField}.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {{ {string.Join(", ", items)}}});");
    }

    private static void AppendGridColumns(StringBuilder sb, List<ColumnSpec> columns, string colPrefix, string chkField)
    {
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
        foreach (var c in ComboColumns(columns))
        {
            var field = ComboEditField(colPrefix, c);
            sb.AppendLine("        //");
            sb.AppendLine($"        // {field}");
            sb.AppendLine("        //");
            sb.AppendLine($"        this.{field}.LookupKey = \"{EscapeCs(c.LookupKey ?? string.Empty)}\";");
            sb.AppendLine($"        this.{field}.Name = \"{field}\";");
        }
    }

    // panData 상세컨트롤 - CHECK는 CheckBoxWyn, COMBO는 LookUpEditWyn(LookupKey로 연결), 그 외는
    // 전부 TextEditWyn(날짜/숫자도 일단 텍스트로 - 서식/전용 에디터 교체는 개발자가 화면에서 직접).
    private static string DetailFieldName(ColumnSpec c) =>
        (c.ControlKind == "CHECK" ? "chkDetail" : c.ControlKind == "COMBO" ? "cboDetail" : "txtDetail") + PascalCase(c.Name);

    private static string DetailFieldType(ColumnSpec c) =>
        c.ControlKind == "CHECK" ? "CheckBoxWyn" : c.ControlKind == "COMBO" ? "LookUpEditWyn" : "TextEditWyn";

    private static string DetailFieldDecl(ColumnSpec c) =>
        $"{DetailFieldName(c)} = new WYNLAB.Base.Controls.{DetailFieldType(c)}();";

    private static string DetailFieldAssign(ColumnSpec c, string valueExpr) => c.ControlKind switch
    {
        "CHECK" => $"{DetailFieldName(c)}.Checked = {valueExpr}?.ToString() == \"Y\";",
        "COMBO" => $"{DetailFieldName(c)}.EditValue = {valueExpr}?.ToString() ?? string.Empty;",
        _ => $"{DetailFieldName(c)}.Text = {valueExpr}?.ToString() ?? string.Empty;"
    };

    private static string DetailFieldClear(ColumnSpec c) => c.ControlKind switch
    {
        "CHECK" => $"{DetailFieldName(c)}.Checked = false;",
        "COMBO" => $"{DetailFieldName(c)}.EditValue = string.Empty;",
        _ => $"{DetailFieldName(c)}.Text = string.Empty;"
    };

    private static string DetailFieldReadExpr(ColumnSpec c) => c.ControlKind switch
    {
        "CHECK" => $"{DetailFieldName(c)}.Checked ? \"Y\" : \"N\"",
        "COMBO" => $"{DetailFieldName(c)}.EditValue?.ToString() ?? string.Empty",
        _ => $"{DetailFieldName(c)}.Text"
    };

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
        TemplateKind.MasterFormTabGrid => "TplMasterFormTabGrid",
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
