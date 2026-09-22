using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.SYS;

/// <summary>
/// AI Builder - 화면개발 자동화(화면생성기) 도구. 조회/저장 프로시저 이름을 넣고 "Describe"로
/// 컬럼/파라미터를 읽어온 뒤, 미리보기 탭에서 다듬고 "Generate Code"로 실제 frmXxx.cs/
/// .Designer.cs를 만든다. 실제 조립 로직은 ScreenTemplateGenerator.cs 참고.
///
/// 하단 미리보기 탭 구성(2026-09-04 통합 이후, 최종): "검색조건"(구 Save Params - panHeader에
/// 노출할 조회 파라미터 체크) / "Query Sources (다중)" / "Save Actions (다중)" 3개뿐이다.
/// 초기 버전에 있던 Master Columns/Sub Columns 탭(템플릿별 grd1/grd2 컬럼을 각각 따로 편집)은
/// Query Sources 탭 하나가 grd1~grd3을 전부 대체하면서 완전히 불필요해졌고, 이번 정리에서
/// Designer.cs에서도 물리적으로 삭제했다(한동안 PageVisible=false로 화면에서만 숨겨뒀었음).
/// </summary>
public partial class frmAIBuilder : BaseForm
{
    // 이 화면은 특정 레포(이 WYNLAB 저장소) 전용 개발 도구라 배포 대상마다 달라질 이유가 없다 -
    // Deploy-Local.ps1/DB 접속 문자열처럼 이 프로젝트의 다른 도구들도 이미 로컬 경로를
    // 그대로 쓰고 있다.
    private const string RepoRoot = @"D:\01. SOURCE\00. WYNLAB";

    private class DescribeProcRequest
    {
        public string ProcName { get; set; } = string.Empty;
        public string? WorkType { get; set; }
    }

    /// <summary>describe-proc-multi는 WorkType이 필수(서버 쪽 DescribeProcMultiRequest와 동일한
    /// 모양) - describe-proc의 WorkType은 저장프로시저 describe 때 생략 가능해서 nullable이지만
    /// 이건 항상 있어야 한다.</summary>
    private class DescribeProcMultiRequest
    {
        public string ProcName { get; set; } = string.Empty;
        public string WorkType { get; set; } = string.Empty;
    }

    private class ProcWorkTypesRequest
    {
        public string ProcName { get; set; } = string.Empty;
    }

    // 라벨은 아직 이름 정리 전(합의 대기) - 화면구조가 바뀌는 게 아니라 표시 문구만 바뀔
    // 사안이라 우선 그대로 둔다. 아이콘(TemplateIconFiles)은 이름과 무관하게 실제 화면구조를
    // 그린 것이라 먼저 넣는다.
    private static readonly (TemplateKind Kind, string Label)[] TemplateOptions =
    {
        (TemplateKind.SingleGrid, "Single Grid"),
        (TemplateKind.MasterSubGrid, "Master-Sub Grid"),
        (TemplateKind.MasterFormSubGrid, "Master-Form-Sub Grid"),
        (TemplateKind.MasterOneSheet, "Master-One Sheet"),
        (TemplateKind.TreeMasterSubGrid, "Tree-Master-Sub Grid"),
    };

    // TemplateOptions와 같은 순서 - Assets/TemplateIcons/*.svg(TemplateIcons.cs가 로드),
    // 실제 템플릿 화면의 구역 배치를 축소한 와이어프레임. MasterOneSheet는 2026-09-09 재설계로
    // grd1(마스터 목록)이 없어져서 전용 아이콘(form_grid.svg - 검색줄+폼+그리드, 왼쪽 목록 없음)을
    // 새로 그렸다. MasterFormSubGrid/TreeMasterSubGrid는 아직 전용 아이콘이 없어 임시로
    // master_form_grid.svg를 재사용한다(구조가 제일 비슷함 - 그리드+상세폼) - 나중에 각자
    // 전용 아이콘(탭 1~2개짜리 / 트리)으로 교체할 것.
    private static readonly string[] TemplateIconFiles = { "grid.svg", "master_grid.svg", "master_form_grid.svg", "form_grid.svg", "master_form_grid.svg" };

    public frmAIBuilder()
    {
        InitializeComponent();

        Text = "AI Builder";

        Controls.Add(BuildScreenHeader());

        //cboModule.Properties.Items.AddRange(new[] { "SM", "BA", "SA", "PR", "SYS", "MA" });
       // cboModule.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
       // cboModule.SelectedIndex = 0;

        // 모듈+화면클래스명을 고르면 Proc Prefix를 "USP_{모듈}_{화면클래스명}_" 형태로 자동
        // 제안한다(2026-09-09 변경 - 예전엔 모듈 공통값만 채워서(TSMMINOR.rel_cd1 의존) 사용자가
        // 안 고치고 그대로 저장하면 frmEMP처럼 "USP_BA_"만 저장돼 그 화면 권한으로 BA모듈
        // 전체 프로시저를 다 부를 수 있게 되는 문제가 있었다 - 실제로 겪음). 화면클래스명을 아직
        // 안 적었으면 모듈 공통값("USP_BA_" 등)만 채워서 예전과 같은 동작을 유지한다 - 어차피
        // 자유입력 필드라 사용자가 최종적으로 원하는 값으로 고쳐 쓸 수 있다.
        cboModule.EditValueChanged += (s, e) => UpdateSuggestedProcPrefix();
        txtScreenClassNm.EditValueChanged += (s, e) => UpdateSuggestedProcPrefix();

        // 템플릿 3종을 이름만으로 고르면 구조가 헷갈린다는 피드백에 따라, 실제 화면 레이아웃을
        // 축소한 아이콘(TemplateIcons.cs)과 함께 고르도록 ImageComboBoxEdit을 쓴다.
        // 24x18로는 팝업 목록에서 와이어프레임 구분이 잘 안 보인다는 피드백(2026-09-06)에 따라
        // 40x40으로 키운다 - 닫힌 콤보 박스 자체(Designer.cs, Height=24)는 그대로 두되, 드롭다운
        // 팝업 목록 행은 ImageList.ImageSize를 그대로 따라가므로 목록에서만 실질적으로 커 보인다.
        var templateImages = new ImageList { ImageSize = new Size(40, 40), ColorDepth = ColorDepth.Depth32Bit };
        foreach (var iconFile in TemplateIconFiles)
            templateImages.Images.Add(TemplateIcons.Load(iconFile, 40) ?? new Bitmap(40, 40));
        cboTemplateKind.Properties.SmallImages = templateImages;
        cboTemplateKind.Properties.Items.AddRange(TemplateOptions.Select((o, i) =>
            new ImageComboBoxItem(o.Label, o.Kind.ToString(), i)).ToArray());
        // 예전엔 첫 번째 옵션(SingleGrid)을 기본 선택해뒀는데, 사용자가 Template을 고르는 걸
        // 깜빡해도 항상 SingleGrid로 조용히 생성돼버려서(2026-09-07 지적) 처음엔 빈 값으로 두고
        // Create Form 시점에 GenerateCode()가 선택 여부를 직접 검증한다.
        cboTemplateKind.EditValue = null;

        // Save Params 탭은 원래 용도(저장 파라미터 매칭)를 잃었으니 검색조건(panHeader) 선택용으로
        // 재활용한다 - grd1 조회프로시저(work_type='Q')의 파라미터 중 work_type을 뺀 나머지를
        // 보여주고 체크박스로 "이건 검색창으로 노출"만 고르면 된다(2026-09-04 추가). 컬럼 3개
        // 그리드가 마침 Name/SqlType/체크형태로 딱 맞아서 새 탭 없이 재사용한다.
        tabSaveParams.Text = "검색조건";
        gvwSaveParams.Role = GridRoleWyn.Edit;
        // Designer.cs 기본값은 AllowEdit=false(예전엔 Name/SqlType이 항상 describe 결과 그대로였음) -
        // 이제 사람이 프로시저 파라미터가 아닌 행을 직접 추가할 수 있어야 하므로 편집 가능으로 푼다
        // (2026-09-08).
        colSpName.OptionsColumn.AllowEdit = true;
        colSpSqlType.OptionsColumn.AllowEdit = true;
        colSpSqlType.VisibleIndex = 2; // Caption 컬럼이 그 사이(1)에 새로 끼어드므로 뒤로 밀어준다.
        colSpMatchedColumn.Caption = "검색조건 포함";
        colSpMatchedColumn.FieldName = "Include";
        colSpMatchedColumn.VisibleIndex = 6;
        var includeEdit = new RepositoryItemCheckEdit();
        grdSaveParams.RepositoryItems.Add(includeEdit);
        colSpMatchedColumn.ColumnEdit = includeEdit;

        // panHeader에 실제로 화면에 보일지(Visible) - Include(조회 시 전송 여부)와 별개 축이다
        // (2026-09-09 요청 - "view컬럼을 추가해서 row추가 할때 디폴트로 Y 세팅하고 폼 생성 시 N인
        // 건들은 컨트롤 만들어지지만 visible속성 false로"). 같은 체크박스 리포지토리 아이템
        // (includeEdit)을 그대로 재사용 - 둘 다 단순 Y/N 체크박스라 설정을 따로 가질 이유가 없다.
        var colSpView = new DevExpress.XtraGrid.Columns.GridColumn
        {
            Caption = "View",
            FieldName = "View",
            Name = "colSpView",
            ColumnEdit = includeEdit,
            Visible = true,
            VisibleIndex = 7,
            Width = 50,
        };

        // 필수입력 강조(RequiredFieldExtensions.MarkRequired) 적용 여부(2026-09-09 요청) - 같은
        // 체크박스 리포지토리 아이템(includeEdit)을 재사용한다.
        var colSpRequired = new DevExpress.XtraGrid.Columns.GridColumn
        {
            Caption = "Required",
            FieldName = "Required",
            Name = "colSpRequired",
            ColumnEdit = includeEdit,
            Visible = true,
            VisibleIndex = 8,
            Width = 60,
        };

        // panHeader에 실제로 만들어질 때 입력칸 앞에 붙는 라벨 문구(2026-09-09 요청 - "검색조건에
        // Caption컬럼을 추가해서 폼생성시 panHeader에 컨트롤이 만들어질때 그 앞에 라벨의 캡션으로
        // 적용해줘"). 그리드/panData 컬럼 미리보기와 같은 이름(Caption)을 쓴다 -
        // ScreenTemplateGenerator.BuildSearchFieldBlocks가 그대로 읽어서 LabelControl.Text로 쓴다.
        var colSpCaption = new DevExpress.XtraGrid.Columns.GridColumn
        {
            Caption = "Caption(라벨)",
            FieldName = "Caption",
            Name = "colSpCaption",
            Visible = true,
            VisibleIndex = 1,
            Width = 110,
        };

        // 조회프로시저 파라미터가 아닌 컨트롤(예: dept_cd 옆의 dept_nm)도 행을 추가해서 넣을 수
        // 있게 - Name/SqlType은 그대로 두되(수동으로 추가한 행은 직접 타이핑), Control 종류는
        // 그리드/panData 컬럼 미리보기(gvwColumnPreview2)와 같은 어휘(L_SM0005)를 재사용한다
        // (2026-09-08. 2026-09-09에 하드코딩 SQL이던 L_SYS_CTRLKIND에서 기초코드 기반 L_SM0005로
        // 교체 - 관리자가 frmMinorCode에서 컨트롤 종류를 직접 관리할 수 있게). RepositoryItem은
        // GridControl마다 따로 필요하므로 lookUpControlKind를 그대로 못 쓰고 이 그리드 전용
        // 인스턴스를 새로 만든다.
        var colSpParamName = new DevExpress.XtraGrid.Columns.GridColumn
        {
            Caption = "SP 파라미터",
            FieldName = "ParamName",
            Name = "colSpParamName",
            Visible = true,
            VisibleIndex = 3,
            Width = 110,
        };
        var lookUpSearchControlKind = new LookUpColumnEdit { LookupKey = "L_SM0005" };
        grdSaveParams.RepositoryItems.Add(lookUpSearchControlKind);
        var colSpControlKind = new DevExpress.XtraGrid.Columns.GridColumn
        {
            Caption = "Control",
            FieldName = "ControlKind",
            Name = "colSpControlKind",
            ColumnEdit = lookUpSearchControlKind,
            Visible = true,
            VisibleIndex = 4,
            Width = 100,
        };
        var colSpLookupKey = new DevExpress.XtraGrid.Columns.GridColumn
        {
            Caption = "LookupKey (COMBO/POP)",
            FieldName = "LookupKey",
            Name = "colSpLookupKey",
            Visible = true,
            VisibleIndex = 5,
            Width = 150,
        };
        gvwSaveParams.Columns.AddRange(new[] { colSpCaption, colSpParamName, colSpControlKind, colSpLookupKey, colSpView, colSpRequired });

        gvwSaveParams.RowAdd += (s, e) => gvwSaveParams.AddNewRow();
        gvwSaveParams.RowDelete += (s, e) => DeleteFocusedRow(gvwSaveParams);

        grdSaveParams.DataSource = NewSearchParamsTable();

        // 상위메뉴는 직접 타이핑하지 않고 메뉴트리 팝업(P_MENU)에서 고른다 - frmMenu.cs의
        // 상위메뉴 선택과 같은 패턴. MapField가 고른 메뉴의 menu_id를 txtUpperMenuCd(화면에는
        // 안 보임)에 자동으로 채운다.
        txtUpperMenuNm.LookupKey = "P_MENU";
        txtUpperMenuNm.MatchField = "menu_nm";
        txtUpperMenuNm.MapField("menu_id", txtUpperMenuCd);

        // 쿼리소스/저장액션 - 템플릿 종류와 무관하게 이 2개 탭이 전부 담당한다. 행을 몇 개든
        // 추가/삭제할 수 있고, 선택한 행의 하위 내용(레코드셋 컬럼/파라미터)을 아래 미리보기
        // 그리드에 다시 채운다 - 그리드 자체는 이 2개로 고정, 프로시저/레코드셋/저장액션이
        // 몇 개든 여기 행만 늘어난다.
        gvwQueryResultSets.Role = GridRoleWyn.Edit;
        gvwQueryResultSets.RowAdd += (s, e) => gvwQueryResultSets.AddNewRow();
        gvwQueryResultSets.RowDelete += (s, e) =>
        {
            try { if (gvwQueryResultSets.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };
        gvwQueryResultSets.FocusedRowObjectChanged += (s, e) =>
            grdColumnPreview2.DataSource = gvwQueryResultSets.GetFocusedRow() is DataRowView view && view.Row["ColumnsTable"] is DataTable t ? t : new DataTable();
        grdQueryResultSets.DataSource = NewQueryResultSetsTable();
        // Role을 안 걸어주면 Designer 기본값(Editable=false)이 그대로 남아 Caption/In Grid/
        // Key/Control 전부 편집이 안 된다(2026-09-04 실제 발견).
        gvwColumnPreview2.Role = GridRoleWyn.Edit;

        // TargetSlot/SourceSlot은 이 템플릿(MasterFormSubGrid)이 실제로 갖고 있는 컨트롤 이름
        // 뿐이라 자유입력 대신 드롭다운으로 고정한다 - 오타로 생성이 실패하는 걸 원천 차단.
        var targetSlotEdit = new RepositoryItemComboBox { TextEditStyle = TextEditStyles.DisableTextEditor };
        // tree1 = TreeMasterSubGrid 템플릿의 마스터(트리) 슬롯 - grd1과 같은 역할(마스터)이지만
        // 실제 컨트롤 이름이 tree1이라 따로 넣는다(2026-09-09, "Target Control에 tree가 안
        // 보인다" 실제 지적 - grd1만 있으면 트리 템플릿을 쓸 때 뭘 골라야 할지 알 수 없었다).
        targetSlotEdit.Items.AddRange(new object[] { "grd1", "tree1", "grd2", "grd3", "grd4", "grd5" });
        grdQueryResultSets.RepositoryItems.Add(targetSlotEdit);
        colQrTargetSlot.ColumnEdit = targetSlotEdit;

        gvwSaveActions.Role = GridRoleWyn.Edit;
        gvwSaveActions.RowAdd += (s, e) => gvwSaveActions.AddNewRow();
        gvwSaveActions.RowDelete += (s, e) =>
        {
            try { if (gvwSaveActions.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };
        gvwSaveActions.FocusedRowObjectChanged += (s, e) =>
            grdSaveActionParams.DataSource = gvwSaveActions.GetFocusedRow() is DataRowView view && view.Row["ParamsTable"] is DataTable t ? t : new DataTable();
        grdSaveActions.DataSource = NewSaveActionsTable();
        // gvwColumnPreview2와 같은 이유 - Role을 안 걸어주면 MatchedColumn 수동 수정이 안 된다.
        gvwSaveActionParams.Role = GridRoleWyn.Edit;

        var sourceSlotEdit = new RepositoryItemComboBox { TextEditStyle = TextEditStyles.DisableTextEditor };
        sourceSlotEdit.Items.AddRange(new object[] { "grd1", "panData", "grd2", "grd3", "grd4", "grd5" });
        grdSaveActions.RepositoryItems.Add(sourceSlotEdit);
        colSaSourceSlot.ColumnEdit = sourceSlotEdit;

        // 조회할/저장할 프로시저 "계획" 그리드(2026-09-06 추가) - 위쪽에 프로시저 이름을 한 줄씩
        // 입력하는 방식은 여러 프로시저를 미리 죽 적어두기 번거로웠다("아래(Query Sources/Save
        // Actions)는 describe 결과라 레코드셋/저장액션 단위로 행이 생기니, 입력 단계에서 여러
        // 프로시저를 한꺼번에 적어두기엔 안 맞다"는 지적). 그래서 입력 전용 그리드를 별도로 두고,
        // "Describe All"이 이 그리드의 행을 하나씩 돌며 기존 AddQuerySourceRowsAsync/
        // AutoFillSaveActionAsync를 그대로 호출한다 - 실제 저장 형태(QuerySources/SaveActions)는
        // 그대로다.
        gvwQueryProcPlan.Role = GridRoleWyn.Edit;
        gvwQueryProcPlan.RowAdd += (s, e) => gvwQueryProcPlan.AddNewRow();
        gvwQueryProcPlan.RowDelete += (s, e) => DeleteFocusedRow(gvwQueryProcPlan);
        grdQueryProcPlan.DataSource = NewQueryProcPlanTable();
        // 전용 Add/Delete 버튼(btnRowAdd1/btnDeleteRow1, 2026-09-06 레이아웃 개편으로 헤더 툴바에
        // 추가)을 뒀으니 내장 네비게이터는 숨긴다 - RowAdd/RowDelete 구독 자체는 그대로 둔다(엑셀
        // 붙여넣기로 행이 모자랄 때 GridViewWynBehavior.PasteFromClipboard가 이 델리게이트로 행을
        // 늘린다 - 네비게이터 노출 여부와 무관하게 항상 동작해야 함).
        grdQueryProcPlan.UseEmbeddedNavigator = false;
        btnRowAdd1.Click += (s, e) => AddPlanRow(gvwQueryProcPlan);
        btnDeleteRow1.Click += (s, e) => DeleteFocusedRow(gvwQueryProcPlan);

        // WorkType/TargetSlot을 이 계획 그리드에 컬럼으로 노출했던 적이 있었는데(2026-09-09
        // 오전) - "프로시저 이름만 넣으면 Describe가 알아서 Q/Q1을 찾아준다"는 자동탐색 기능이
        // 바로 뒤이어 추가되면서 오히려 방해가 됐다(사용자 실제 지적, 2026-09-09 오후 - "WorkType/
        // TargetSlot 컬럼은 제거되어야 하는거 아니야?"). 이 그리드는 다시 "프로시저 이름만" 받고,
        // WorkType/TargetSlot은 항상 DescribeQueryAsync의 자동탐색(ListProcWorkTypesAsync)에
        // 맡긴다 - 특정 슬롯을 사람이 직접 지정하고 싶으면 Describe 이후 오른쪽 Query Sources
        // 그리드(grdQueryResultSets, Target Slot 드롭다운 이미 있음)에서 바꾸면 된다.

        gvwSaveProcPlan.Role = GridRoleWyn.Edit;
        gvwSaveProcPlan.RowAdd += (s, e) => gvwSaveProcPlan.AddNewRow();
        gvwSaveProcPlan.RowDelete += (s, e) => DeleteFocusedRow(gvwSaveProcPlan);
        grdSaveProcPlan.DataSource = NewSaveProcPlanTable();
        grdSaveProcPlan.UseEmbeddedNavigator = false;
        btnRowAdd2.Click += (s, e) => AddPlanRow(gvwSaveProcPlan);
        btnDeleteRow2.Click += (s, e) => DeleteFocusedRow(gvwSaveProcPlan);

        // 이 그리드는 Query 계획 그리드와 같은 이유로 프로시저 이름만 받는다(2026-09-09) - 예전엔
        // Scope/SourceSlot을 여기서 직접 고르게 했는데, 프로시저가 Header인지 Detail인지/어느
        // 슬롯인지는 사람이 매번 판단해서 입력하기보다 "몇 번째 줄인가"로 충분히 예측 가능하다
        // ("Save Actions탭에서도 왼쪽그리드에는 프로시져명만 입력하고 Describe하면 오른쪽에
        // 데이터가 자동으로 만들어져야해" 요청). DescribeSaveAsync가 첫 줄=Header(또는 행단위
        // 템플릿이면 grd1), 그 다음 줄부터 순서대로 grd2/grd3/...로 자동 배정한다 - 잘못
        // 배정됐으면 Describe 이후 오른쪽 Save Actions 그리드(Scope/Source Control 컬럼, 이미
        // 편집 가능)에서 바로 고치면 된다.

        btnDescribeQueryAll.Click += async (s, e) => await DescribeQueryAsync();
        btnDescribeSaveAll.Click += async (s, e) => await DescribeSaveAsync();
        btnDescribeQueryMulti.Click += async (s, e) => await DescribeQueryMultiAsync();
        btnDescribeSaveMulti.Click += async (s, e) => await DescribeSaveActionAsync();
        btnGenerate.Click += async (s, e) => await GenerateCode();
    }

    /// <summary>모듈(cboModule)+화면클래스명(txtScreenClassNm) 조합으로 "USP_{모듈}_{화면}_"
    /// 형태의 Proc Prefix를 제안한다 - 화면클래스명이 아직 비어있으면 모듈 공통값("USP_BA_" 등)만
    /// 채운다. 자유입력 필드라 사용자가 최종적으로 다시 고쳐 쓸 수 있다.</summary>
    private void UpdateSuggestedProcPrefix()
    {
        var moduleCd = cboModule.EditValue?.ToString()?.Trim().ToUpperInvariant() ?? string.Empty;
        if (moduleCd.Length == 0) { txtProcPrefix.Text = string.Empty; return; }

        // "frmEMP" -> "EMP", "frmMinorCode" -> "MINORCODE" - 실제 생성되는 프로시저 이름
        // (USP_BA_EMP_Q 등)과 맞추기 위해 영문/숫자만 남기고 대문자로 바꾼다.
        var screenNm = txtScreenClassNm.Text.Trim();
        if (screenNm.StartsWith("frm", StringComparison.OrdinalIgnoreCase))
            screenNm = screenNm.Substring(3);
        var screenSuffix = new string(screenNm.Where(char.IsLetterOrDigit).ToArray()).ToUpperInvariant();

        txtProcPrefix.Text = screenSuffix.Length > 0
            ? $"USP_{moduleCd}_{screenSuffix}_"
            : $"USP_{moduleCd}_";
    }

    private static void DeleteFocusedRow(GridViewWyn view)
    {
        try { if (view.GetFocusedRow() is DataRowView row) row.Row.Delete(); }
        catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
    }

    /// <summary>계획 그리드(Query/Save Proc Plan)에 새 행을 추가한다 - 직전 행이 아직 커밋 안 된
    /// 채(방금 타이핑만 하고 셀 편집기를 안 벗어난 상태) 곧바로 AddNewRow()를 부르면 그 값이
    /// 통째로 날아간다(실제로 겪음, 2026-09-07 - 첫 행에 입력 후 Add를 누르면 첫 행이 비워짐).
    /// GridViewWynBehavior.PasteFromClipboard와 같은 이유로 CloseEditor+UpdateCurrentRow로 먼저
    /// 확정한 뒤에 새 행을 만든다. AddNewRow는 포커스만 새 행(NewItemRowHandle)으로 옮길 뿐 셀
    /// 편집기까지 자동으로 열어주지는 않아서(실제로 겪음 - Add를 눌러도 바로 타이핑이 안 됨),
    /// ShowEditor()로 명시적으로 편집 상태를 연다. 그 다음 어디에 입력하면 되는지 눈에 띄게
    /// 잠깐 깜빡여서 강조한다.</summary>
    private static void AddPlanRow(GridViewWyn view)
    {
        view.CloseEditor();
        view.UpdateCurrentRow();
        view.AddNewRow();
        view.ShowEditor();
        FlashFocusedRow(view);
    }

    /// <summary>포커스된 행 배경을 연노랑(UiTheme.RequiredFieldBackColor)으로 몇 차례 깜빡인다
    /// (0.25초 간격 6회 = 1.5초 후 원래대로) - RowCellStyle에 임시 핸들러를 붙였다가 끝나면
    /// 스스로 떼어낸다(계속 쌓이면 다른 행까지 잘못 칠할 수 있어서 반드시 해제). 다시 그리게
    /// 만드는 건 반드시 GridControl.Invalidate()(순수 WinForms 재도색 요청)로만 한다 -
    /// GridView.RefreshRow()는 그 행을 데이터소스에서 다시 읽어오려 드는데, 이 시점의 새 행은
    /// 아직 실제 DataRow가 아니라 NewItemRowHandle(가짜 핸들)이라 그 재조회가 방금 열어둔
    /// ShowEditor() 편집 상태를 매번 취소시켜버렸다(실제로 겪음, 2026-09-07 - Add 직후 셀에
    /// 포커스가 안 붙고 화면이 4번 정도 깜빡거림).</summary>
    private static void FlashFocusedRow(GridViewWyn view)
    {
        var rowHandle = view.FocusedRowHandle;
        var blinkOn = true;
        var ticksLeft = 6;

        void OnRowCellStyle(object? s, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.RowHandle != rowHandle || !blinkOn) return;
            e.Appearance.BackColor = UiTheme.RequiredFieldBackColor;
            e.Appearance.Options.UseBackColor = true;
        }

        view.RowCellStyle += OnRowCellStyle;

        var timer = new System.Windows.Forms.Timer { Interval = 250 };
        timer.Tick += (s, e) =>
        {
            blinkOn = !blinkOn;
            view.GridControl.Invalidate();
            if (--ticksLeft > 0) return;

            timer.Stop();
            timer.Dispose();
            view.RowCellStyle -= OnRowCellStyle;
            view.GridControl.Invalidate();
        };
        timer.Start();
    }

    // 프로시저 이름만 받는다(2026-09-09) - WorkType/TargetSlot은 DescribeQueryAsync가 항상
    // ListProcWorkTypesAsync로 자동탐색한다(위 DescribeQueryAsync 주석 참고).
    private static DataTable NewQueryProcPlanTable()
    {
        var t = new DataTable();
        t.Columns.Add("ProcName", typeof(string));
        return t;
    }

    // 프로시저 이름만 받는다(2026-09-09) - Scope/SourceSlot은 DescribeSaveAsync가 계획 그리드의
    // 줄 순서로 자동 배정한다(위 생성자 주석 참고).
    private static DataTable NewSaveProcPlanTable()
    {
        var t = new DataTable();
        t.Columns.Add("ProcName", typeof(string));
        return t;
    }

    private static DataTable NewQueryResultSetsTable()
    {
        var t = new DataTable();
        t.Columns.Add("ProcName", typeof(string));
        t.Columns.Add("WorkType", typeof(string));
        t.Columns.Add("ResultSetIndex", typeof(int));
        t.Columns.Add("TargetSlot", typeof(string));
        t.Columns.Add("ColumnsTable", typeof(object)); // 화면엔 안 보임(gvwQueryResultSets에 이 컬럼용 GridColumn이 없음) - 선택된 행의 레코드셋 컬럼 미리보기를 들고 다니는 자리
        return t;
    }

    private static DataTable NewSearchParamsTable()
    {
        var t = new DataTable();
        t.Columns.Add("Name", typeof(string));
        t.Columns.Add("SqlType", typeof(string));
        // 조회프로시저의 실제 파라미터일 때만 채운다("p_" 접두어 포함, 예: "p_dept_cd") - 비어있으면
        // 화면표시 전용 컨트롤(예: dept_cd 옆의 dept_nm)이라 조회 시 안 보낸다(2026-09-08).
        t.Columns.Add("ParamName", typeof(string));
        // panHeader에 실제로 만들어질 때 입력칸 앞에 붙는 라벨 문구(2026-09-09) - 비어있으면
        // Name(원본 파라미터명)이 그대로 라벨이 된다(PopulateSearchParams 기본값).
        t.Columns.Add("Caption", typeof(string));
        t.Columns.Add("ControlKind", typeof(string));
        t.Columns.Add("LookupKey", typeof(string));
        t.Columns.Add("Include", typeof(bool));
        // panHeader에 컨트롤 자체는 항상 만들어지지만(코드에서 값을 읽고 쓸 수 있어야 하므로) 화면에
        // 보일지는 별개다(2026-09-09 요청) - false면 라벨+컨트롤 둘 다 Visible=false로 생성된다.
        // Include(조회 시 전송 여부)와 완전히 독립된 축이다 - 예: 화면엔 안 보이지만 값은 계속
        // 보내야 하는 숨은 파라미터도 가능해야 하므로.
        t.Columns.Add("View", typeof(bool));
        // 생성된 컨트롤에 필수입력 강조(RequiredFieldExtensions.MarkRequired)를 적용할지(2026-09-09
        // 요청) - 그리드/panData 컬럼 미리보기(ToColumnPreviewTable)의 Required와 같은 개념이다.
        t.Columns.Add("Required", typeof(bool));
        t.Columns["ControlKind"]!.DefaultValue = "TEXT";
        t.Columns["Include"]!.DefaultValue = true;
        t.Columns["View"]!.DefaultValue = true;
        t.Columns["Required"]!.DefaultValue = false;
        return t;
    }

    private static DataTable NewSaveActionsTable()
    {
        var t = new DataTable();
        t.Columns.Add("ProcName", typeof(string));
        t.Columns.Add("Scope", typeof(string));
        t.Columns.Add("SourceSlot", typeof(string));
        t.Columns.Add("KeyParam", typeof(string));
        t.Columns.Add("ParamsTable", typeof(object)); // 화면엔 안 보임 - 선택된 행의 파라미터 매칭 미리보기
        return t;
    }

    /// <summary>(Proc,WorkType)을 describe-proc-multi로 조회해서 grdQueryResultSets에 레코드셋
    /// 개수만큼 행을 채운다 - 같은 (Proc,WorkType)의 기존 행은 먼저 지운다. targetSlotOrder가
    /// 있으면 레코드셋 순서대로 자동 배정(DescribeQueryAsync의 자동화 경로), 없으면(수동 버튼
    /// 경로) 빈 값으로 남겨 사람이 드롭다운에서 고르게 한다. 이 프로시저의 파라미터 목록도 같이
    /// 돌려준다(검색조건 후보 - DescribeQueryAsync가 grd1 호출 결과에서만 쓴다).</summary>
    private async Task<(int Count, List<ProcParamInfoDto> Params)> AddQuerySourceRowsAsync(DataTable table, string procName, string workType, string[] targetSlotOrder)
    {
        var result = await ApiClient.PostAsync<DescribeProcMultiRequest, DescribeProcMultiResultDto>(
            "api/screen-builder/describe-proc-multi", new DescribeProcMultiRequest { ProcName = procName, WorkType = workType });
        if (result == null) return (0, new List<ProcParamInfoDto>());

        var toRemove = table.Rows.Cast<DataRow>()
            .Where(r => string.Equals(r["ProcName"]?.ToString(), procName, StringComparison.OrdinalIgnoreCase)
                     && string.Equals(r["WorkType"]?.ToString(), workType, StringComparison.OrdinalIgnoreCase))
            .ToList();
        // 같은 (Proc,WorkType)을 다시 조회(재describe)할 때, 레코드셋 인덱스가 같은 이전 행의
        // ColumnsTable을 먼저 기억해둔다 - 안 그러면 Caption/ControlKind(COMBO 등)/LookupKey/
        // Required를 사람이 직접 고쳐놨어도 재조회 한 번에 전부 기본값으로 되돌아간다(2026-09-11
        // 실제 발견 - "필수입력 체크했는데 반영 안 됨": grd2를 선택사항으로 만든 뒤 이 화면을 다시
        // 만들면서 레코드셋 조회를 다시 눌렀는데, 그 전에 체크해둔 Required가 일부만 남고 나머지는
        // 사라짐 - SaveParams 그리드(AddSaveActionRowsAsync 근처)는 이미 prev 값을 이어받는데
        // ColumnsTable 쪽만 그 로직이 빠져 있었다).
        var previousColumnsByIndex = toRemove
            .Where(r => r["ColumnsTable"] is DataTable)
            .ToDictionary(r => Convert.ToInt32(r["ResultSetIndex"]), r => (DataTable)r["ColumnsTable"]);
        foreach (var r in toRemove) table.Rows.Remove(r);

        foreach (var rs in result.ResultSets)
        {
            var row = table.NewRow();
            row["ProcName"] = procName;
            row["WorkType"] = workType;
            row["ResultSetIndex"] = rs.Index;
            row["TargetSlot"] = rs.Index < targetSlotOrder.Length ? targetSlotOrder[rs.Index] : string.Empty;
            var previousColumns = previousColumnsByIndex.TryGetValue(rs.Index, out var prevColumnsTable) ? prevColumnsTable : null;
            row["ColumnsTable"] = ToColumnPreviewTable(rs.Columns, previousColumns);
            table.Rows.Add(row);
        }
        table.AcceptChanges();
        return (result.ResultSets.Count, result.Params);
    }

    /// <summary>프로시저 소스에서 "@p_work_type = 'X'" 리터럴만 찾아 work_type 후보 목록을
    /// 받아온다(api/screen-builder/proc-work-types, 실행하지 않는 정적 텍스트 검색) -
    /// DescribeQueryAsync가 계획 그리드에 WorkType을 비워둔 행을 자동으로 펼치는 데 쓴다. 실패하면
    /// (프로시저 이름 오타, 네트워크 문제 등) 빈 목록 - 호출부가 "Q" 하나로 폴백한다.</summary>
    private static async Task<List<string>> DescribeProcWorkTypesAsync(string procName)
    {
        try
        {
            var result = await ApiClient.PostAsync<ProcWorkTypesRequest, ProcWorkTypesResultDto>(
                "api/screen-builder/proc-work-types", new ProcWorkTypesRequest { ProcName = procName });
            return result?.WorkTypes ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }

    /// <summary>GenerateCode()의 재검증(FindStaleProcReferenceAsync) 전용 - 그리드 행을 안
    /// 건드리고 describe 결과만 가볍게 다시 조회한다(AddQuerySourceRowsAsync는 grdQueryResultSets
    /// 행까지 갱신해서 이 용도엔 과함). 같은 (Proc,WorkType)을 검색조건/컬럼 검증에서 중복으로
    /// describe하지 않게 cache에 담아 재사용한다. 실패하면(프로시저 이름 오타, 네트워크 문제 등)
    /// null을 돌려준다 - 호출부가 그 경우 검증을 건너뛴다(fail-open, 어차피 진짜 오류면 이후 실제
    /// 생성/저장 단계에서 별도로 걸러진다).</summary>
    private static async Task<DescribeProcMultiResultDto?> DescribeProcMultiCachedAsync(
        Dictionary<(string ProcName, string WorkType), DescribeProcMultiResultDto?> cache, string procName, string workType)
    {
        var key = (procName, workType);
        if (cache.TryGetValue(key, out var cached)) return cached;

        DescribeProcMultiResultDto? result;
        try
        {
            result = await ApiClient.PostAsync<DescribeProcMultiRequest, DescribeProcMultiResultDto>(
                "api/screen-builder/describe-proc-multi", new DescribeProcMultiRequest { ProcName = procName, WorkType = workType });
        }
        catch
        {
            result = null;
        }
        cache[key] = result;
        return result;
    }

    /// <summary>저장프로시저 하나의 파라미터 이름만 가볍게 다시 조회한다(describe-proc, work_type
    /// 없음) - FindStaleProcReferenceAsync의 Save Actions 재검증 전용.</summary>
    private static async Task<List<string>?> DescribeSaveProcParamNamesAsync(string procName)
    {
        try
        {
            var result = await ApiClient.PostAsync<DescribeProcRequest, DescribeProcResultDto>(
                "api/screen-builder/describe-proc", new DescribeProcRequest { ProcName = procName, WorkType = null });
            return result?.Params.Select(p => p.ParamNm).ToList();
        }
        catch
        {
            return null;
        }
    }

    /// <summary>GenerateCode()가 코드를 만들기 직전에 부른다 - Query Sources/Save Actions/검색조건이
    /// 지금도 실제 프로시저와 맞는지 전부 재확인한다(1: 검색조건 파라미터, 2: 각 그리드/panData가
    /// 쓰는 컬럼, 3: Save Actions 파라미터). 문제를 찾으면 사람이 읽을 메시지를 돌려주고, 없으면
    /// (또는 그 프로시저의 describe 자체가 실패했으면 - fail-open) null을 돌려준다.</summary>
    private async Task<string?> FindStaleProcReferenceAsync(ScreenGenSpec spec)
    {
        var multiCache = new Dictionary<(string ProcName, string WorkType), DescribeProcMultiResultDto?>();

        // 1) 검색조건 - grd1(또는 TreeMasterSubGrid의 tree1) 조회프로시저의 실제 파라미터로
        // 지정된 것들만 검증한다(ParamName이 비어있는 화면표시 전용 컨트롤, 예: dept_nm은
        // 애초에 프로시저 파라미터가 아니므로 대상 아님, 2026-09-08).
        var grd1Query = spec.QuerySources.FirstOrDefault(q => q.ResultSetBindings.Any(b => b.TargetSlot == "grd1" || b.TargetSlot == "tree1"));
        var paramFields = spec.SearchFields.Where(f => !string.IsNullOrWhiteSpace(f.ParamName)).ToList();
        if (grd1Query != null && paramFields.Count > 0)
        {
            var liveResult = await DescribeProcMultiCachedAsync(multiCache, grd1Query.ProcName, grd1Query.WorkType);
            var liveParams = liveResult?.Params.Select(p => p.ParamNm).ToList();
            if (liveParams is { Count: > 0 })
            {
                var missing = paramFields
                    .Where(f => !liveParams.Any(lp => string.Equals($"p_{lp}", f.ParamName, StringComparison.OrdinalIgnoreCase)))
                    .Select(f => f.ParamName)
                    .ToList();
                if (missing.Count > 0)
                    return $"검색조건으로 고른 파라미터가 지금의 조회프로시저({grd1Query.ProcName})에 없습니다: {string.Join(", ", missing)}";
            }
        }

        // 2) Query Sources 컬럼 - grd1/grd2/grd3/panData가 실제로 쓰는(IncludeInGrid) 컬럼들.
        foreach (var query in spec.QuerySources)
        {
            var liveResult = await DescribeProcMultiCachedAsync(multiCache, query.ProcName, query.WorkType);
            if (liveResult == null || liveResult.ResultSets.Count == 0) continue;

            foreach (var binding in query.ResultSetBindings)
            {
                var liveColumns = liveResult.ResultSets.FirstOrDefault(rs => rs.Index == binding.ResultSetIndex)?.Columns;
                if (liveColumns == null || liveColumns.Count == 0) continue;

                var missing = binding.Columns.Where(c => c.IncludeInGrid)
                    .Where(c => !liveColumns.Any(lc => string.Equals(lc.ColumnNm, c.Name, StringComparison.OrdinalIgnoreCase)))
                    .Select(c => c.Name)
                    .ToList();
                if (missing.Count > 0)
                    return $"{binding.TargetSlot}에 쓰는 컬럼이 지금의 조회프로시저({query.ProcName})에 없습니다: {string.Join(", ", missing)}";
            }
        }

        // 3) Save Actions 파라미터.
        foreach (var action in spec.SaveActions)
        {
            var liveParams = await DescribeSaveProcParamNamesAsync(action.ProcName);
            if (liveParams is not { Count: > 0 }) continue;

            var missing = action.SaveParams
                .Where(p => !liveParams.Any(lp => string.Equals(lp, p, StringComparison.OrdinalIgnoreCase)))
                .ToList();
            if (missing.Count > 0)
                return $"{action.SourceSlot} 저장에 쓰는 파라미터가 지금의 저장프로시저({action.ProcName})에 없습니다: {string.Join(", ", missing)}";
        }

        return null;
    }

    /// <summary>grdQueryResultSets에 입력된 모든 (Proc,WorkType) 행을 한 번에 다시 describe한다
    /// (2026-09-07 변경 - 예전엔 행을 하나씩 선택해서 버튼을 반복 클릭해야 했다). 이미 사람이
    /// 골라둔 TargetSlot은 ResultSetIndex 기준으로 그대로 유지한다 - 안 그러면 다시 조회할 때마다
    /// 빈 값으로 초기화돼서 매번 다시 골라야 한다.</summary>
    private async Task DescribeQueryMultiAsync()
    {
        if (grdQueryResultSets.DataSource is not DataTable table) return;

        // DescribeQueryAsync와 같은 이유(2026-09-08) - 방금 타이핑한 행이 아직 안 커밋된 채로
        // 읽히는 것을 막는다.
        gvwQueryResultSets.CloseEditor();
        gvwQueryResultSets.UpdateCurrentRow();

        var pairs = table.Rows.Cast<DataRow>()
            .Select(r => (ProcName: r["ProcName"]?.ToString()?.Trim() ?? string.Empty, WorkType: r["WorkType"]?.ToString()?.Trim() ?? string.Empty))
            .Where(p => !string.IsNullOrWhiteSpace(p.ProcName) && !string.IsNullOrWhiteSpace(p.WorkType))
            .Distinct()
            .ToList();
        if (pairs.Count == 0)
        {
            AppMessageBox.Show("Proc/WorkType이 입력된 행이 없습니다.", "입력 필요");
            return;
        }

        var totalCount = 0;
        foreach (var (procName, workType) in pairs)
        {
            var existingSlots = table.Rows.Cast<DataRow>()
                .Where(r => string.Equals(r["ProcName"]?.ToString(), procName, StringComparison.OrdinalIgnoreCase)
                         && string.Equals(r["WorkType"]?.ToString(), workType, StringComparison.OrdinalIgnoreCase))
                .OrderBy(r => (int)r["ResultSetIndex"])
                .Select(r => r["TargetSlot"]?.ToString() ?? string.Empty)
                .ToArray();

            var (count, _) = await AddQuerySourceRowsAsync(table, procName, workType, existingSlots);
            totalCount += count;
        }
        grdQueryResultSets.RefreshDataSource();
        Toast.Show($"{pairs.Count}개 행, 레코드셋 {totalCount}개를 다시 불러왔습니다.");
    }

    /// <summary>지금까지 불러온 모든 컬럼(grdQueryResultSets의 모든 레코드셋)을 매칭 후보로
    /// 모은다 - Save 파라미터 자동매칭(DescribeSaveParamsTableAsync)이 쓴다.</summary>
    private HashSet<string> CollectKnownColumns()
    {
        var knownColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (grdQueryResultSets.DataSource is DataTable qrTable)
            foreach (DataRow r in qrTable.Rows)
                if (r["ColumnsTable"] is DataTable ct)
                    foreach (DataRow cr in ct.Rows) knownColumns.Add(cr["Name"]?.ToString() ?? string.Empty);
        return knownColumns;
    }

    /// <summary>저장프로시저 하나를 describe해서 파라미터별 SQL타입 + 자동매칭된 컬럼명(Param/
    /// SqlType/MatchedColumn 3컬럼)을 담은 미리보기 테이블을 만든다 - work_type/user_id/client_pc는
    /// 프레임워크가 자동 처리하므로 매칭 대상에서 제외한다.</summary>
    private async Task<DataTable> DescribeSaveParamsTableAsync(string procName, HashSet<string> knownColumns)
    {
        var result = await ApiClient.PostAsync<DescribeProcRequest, DescribeProcResultDto>(
            "api/screen-builder/describe-proc", new DescribeProcRequest { ProcName = procName, WorkType = null });

        var table = new DataTable();
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("SqlType", typeof(string));
        table.Columns.Add("MatchedColumn", typeof(string));

        foreach (var p in result?.Params ?? new())
        {
            if (p.ParamNm.Equals("work_type", StringComparison.OrdinalIgnoreCase)) continue;
            if (p.ParamNm.Equals("user_id", StringComparison.OrdinalIgnoreCase)) continue;
            if (p.ParamNm.Equals("client_pc", StringComparison.OrdinalIgnoreCase)) continue;

            var matched = knownColumns.FirstOrDefault(n =>
                n.Replace("_", "").Equals(p.ParamNm.Replace("_", ""), StringComparison.OrdinalIgnoreCase));
            table.Rows.Add(p.ParamNm, p.SqlType, matched ?? string.Empty);
        }
        table.AcceptChanges();
        return table;
    }

    /// <summary>grdSaveActions에 입력된 모든 저장액션 행을 한 번에 다시 describe/매칭한다
    /// (2026-09-07 변경 - 예전엔 행을 하나씩 선택해서 버튼을 반복 클릭해야 했다).</summary>
    private async Task DescribeSaveActionAsync()
    {
        if (grdSaveActions.DataSource is not DataTable table) return;

        // DescribeQueryAsync와 같은 이유(2026-09-08) - 방금 타이핑한 행이 아직 안 커밋된 채로
        // 읽히는 것을 막는다.
        gvwSaveActions.CloseEditor();
        gvwSaveActions.UpdateCurrentRow();

        var rows = table.Rows.Cast<DataRow>()
            .Where(r => !string.IsNullOrWhiteSpace(r["ProcName"]?.ToString()))
            .ToList();
        if (rows.Count == 0)
        {
            AppMessageBox.Show("Proc가 입력된 저장액션 행이 없습니다.", "입력 필요");
            return;
        }

        var knownColumns = CollectKnownColumns();
        var totalParams = 0;
        var totalMatched = 0;
        foreach (var row in rows)
        {
            var procName = row["ProcName"]!.ToString()!.Trim();
            var paramsTable = await DescribeSaveParamsTableAsync(procName, knownColumns);
            row["ParamsTable"] = paramsTable;
            totalParams += paramsTable.Rows.Count;
            totalMatched += paramsTable.Rows.Cast<DataRow>().Count(r => !string.IsNullOrWhiteSpace(r["MatchedColumn"].ToString()));
        }
        table.AcceptChanges();
        grdSaveActions.RefreshDataSource();

        // 지금 포커스된 행이 있으면 그 행의 파라미터 미리보기 그리드도 최신값으로 갱신한다.
        if (gvwSaveActions.GetFocusedRow() is DataRowView focused && focused.Row["ParamsTable"] is DataTable focusedParams)
            grdSaveActionParams.DataSource = focusedParams;

        Toast.Show($"{rows.Count}개 저장액션, 파라미터 {totalParams}개 중 {totalMatched}개 자동매칭됐습니다.");
    }

    /// <summary>주어진 프로시저를 저장액션 한 행으로 자동으로 채운다(describe + 파라미터
    /// 자동매칭) - 같은 procName의 기존 행은 먼저 지운다. KeyParam은 Detail scope에서만
    /// 의미가 있지만 채워둬도 Header에서는 그냥 무시되니 항상 채운다.</summary>
    private async Task AutoFillSaveActionAsync(DataTable saTable, string procName, SaveActionScope scope, string sourceSlot, HashSet<string> knownColumns)
    {
        var toRemove = saTable.Rows.Cast<DataRow>()
            .Where(r => string.Equals(r["ProcName"]?.ToString(), procName, StringComparison.OrdinalIgnoreCase))
            .ToList();
        foreach (var r in toRemove) saTable.Rows.Remove(r);

        var paramsTable = await DescribeSaveParamsTableAsync(procName, knownColumns);

        var row = saTable.NewRow();
        row["ProcName"] = procName;
        row["Scope"] = scope.ToString();
        row["SourceSlot"] = sourceSlot;
        row["KeyParam"] = txtDetailKeyParam.Text.Trim();
        row["ParamsTable"] = paramsTable;
        saTable.Rows.Add(row);
        saTable.AcceptChanges();
    }

    private TemplateKind SelectedTemplateKind =>
        Enum.TryParse<TemplateKind>(cboTemplateKind.EditValue?.ToString(), out var kind) ? kind : TemplateKind.SingleGrid;

    /// <summary>"조회할 프로시저" 계획 그리드(grdQueryProcPlan)의 모든 행을 순서대로 describe해서
    /// "Query Sources" 탭을 채운다 - 프로시저가 여러 개(예: grd1은 USP_XXX_Q, grd2는 완전히
    /// 별도인 USP_XXX_Q_1)여도 계획 그리드에 몇 줄이든 먼저 적어두고 이 버튼 한 번으로 전부
    /// 처리한다(2026-09-06 변경 - 예전엔 프로시저 하나씩 입력창에 타이핑→버튼 클릭을 반복해야
    /// 해서 번거로웠다). grd1으로 채운 조회의 파라미터만 "검색조건" 탭 후보로 올라간다(panHeader가
    /// grd1 조회 파라미터 기준이므로).
    ///
    /// Query Sources는 항상 지금의 계획(grdQueryProcPlan) 내용으로 처음부터 다시 만든다 - 예전엔
    /// 여기서 새로 describe한 (Proc,WorkType)만 그 이름으로 upsert하고 끝나서, 계획 그리드에서
    /// 잘못 등록한 행을 지운 뒤 Describe를 다시 눌러도 그 잘못된 프로시저가 만든 결과 행이 그대로
    /// 남아있었다(2026-09-08 실제 발견 - USP_BA_ITEM_Q를 Save Actions 계획에 잘못 등록했다가
    /// 지웠는데 Describe를 다시 눌러도 안 없어짐). 계획에 없으면 결과에도 없어야 하므로 매번
    /// 통째로 비우고 다시 채운다.</summary>
    private async Task DescribeQueryAsync()
    {
        if (grdQueryProcPlan.DataSource is not DataTable planTable) return;
        if (grdQueryResultSets.DataSource is not DataTable qrTable) return;

        // 방금 타이핑만 하고 셀 편집기를 안 벗어난 행(보통 마지막 줄)은 아직 planTable에 반영이
        // 안 된 상태다 - 그대로 읽으면 그 행만 ProcName이 비어보여서 "처음 눌렀을 때는 프로시저
        // 하나만 처리된다"는 증상으로 나타난다(2026-09-08 실제 발견, AddPlanRow가 이미 같은 이유로
        // CloseEditor+UpdateCurrentRow를 쓰던 것과 동일한 원인).
        gvwQueryProcPlan.CloseEditor();
        gvwQueryProcPlan.UpdateCurrentRow();

        var rows = planTable.Rows.Cast<DataRow>()
            .Where(r => !string.IsNullOrWhiteSpace(r["ProcName"]?.ToString()))
            .ToList();

        qrTable.Rows.Clear();
        grdQueryResultSets.RefreshDataSource();

        if (rows.Count == 0)
        {
            grdSaveParams.DataSource = NewSearchParamsTable();
            AppMessageBox.Show("조회할 프로시저를 한 줄 이상 입력하세요.", "입력 필요");
            return;
        }

        var totalCount = 0;
        var hasGrd1Target = false;
        // 계획 그리드는 프로시저 이름만 받는다 - 그 프로시저가 실제로 갖고 있는 work_type을
        // 자동으로 찾아 전부 describe한다(2026-09-09, "프로시저 이름만 넣으면 Q/Q1을 알아서
        // 찾아달라, 사람은 Target Control만 지정하면 되게" 요청). 찾은 순서대로 마스터 슬롯부터
        // grd2/grd3/...를 미리 배정해두고, 사람은 Query Sources 그리드에서 Target Control만
        // 확인/조정하면 된다. 마스터 슬롯 이름은 템플릿에 따라 다르다(TreeMasterSubGrid는 tree1,
        // 그 외는 grd1) - 지금 선택된 템플릿 기준으로 첫 슬롯을 정한다.
        var masterSlot = SelectedTemplateKind == TemplateKind.TreeMasterSubGrid ? "tree1" : "grd1";
        var slotSequence = new[] { masterSlot, "grd2", "grd3", "grd4", "grd5" };
        foreach (var row in rows)
        {
            var procName = row["ProcName"]!.ToString()!.Trim();

            // 이 계획 그리드는 프로시저 이름만 받는다(2026-09-09 - WorkType/TargetSlot 컬럼을
            // 넣었다가 자동탐색 기능과 겹쳐 오히려 헷갈린다는 지적으로 다시 뺐다) - 항상
            // ListProcWorkTypesAsync로 그 프로시저가 실제로 갖고 있는 work_type을 전부 찾아서
            // 발견 순서대로 grd1/grd2/.../tree1에 미리 배정한다. 특정 work_type을 다른 슬롯으로
            // 옮기고 싶으면 Describe 이후 오른쪽 Query Sources 그리드에서 Target Slot을 바꾸면 된다.
            var discovered = await DescribeProcWorkTypesAsync(procName);
            if (discovered.Count == 0) discovered = new List<string> { "Q" }; // 못 찾으면 기존 기본값으로 폴백(fail-open)
            var plan = discovered.Select((wt, i) => (WorkType: wt, TargetSlot: i < slotSequence.Length ? slotSequence[i] : string.Empty)).ToList();

            foreach (var (workType, targetSlot) in plan)
            {
                var (count, queryParams) = await AddQuerySourceRowsAsync(qrTable, procName, workType, new[] { targetSlot });
                // tree1 = TreeMasterSubGrid의 마스터 슬롯(grd1과 같은 역할) - 검색조건은 항상 그
                // "마스터" 조회의 파라미터에서 채워야 하므로 grd1과 동일하게 취급한다.
                if (targetSlot == "grd1" || targetSlot == "tree1") { PopulateSearchParams(queryParams); hasGrd1Target = true; }
                totalCount += count;
            }
        }
        // grd1을 노리는 행이 이번엔 아예 없으면(계획에서 지워짐) 검색조건도 같이 비운다 - 있으면
        // PopulateSearchParams가 이미 그 안에서 체크상태 보존까지 처리했으니 건드리지 않는다.
        if (!hasGrd1Target) grdSaveParams.DataSource = NewSearchParamsTable();

        grdQueryResultSets.RefreshDataSource();
        Toast.Show($"{rows.Count}개 프로시저, 레코드셋 {totalCount}개를 새로 불러왔습니다(이전 결과는 초기화됨).");
    }

    /// <summary>grd1 조회프로시저(work_type="Q")의 파라미터를 "검색조건" 탭에 채운다 - work_type은
    /// 화면 코드가 직접 처리하므로 후보에서 뺀다. 기존에 사람이 손대둔 값(Include/ControlKind/
    /// LookupKey/ParamName)은 같은 파라미터명이면 그대로 유지한다 - Describe Query를 다시 눌러도
    /// 안 날아가게. 프로시저 파라미터가 아닌, 사람이 직접 추가해둔 행(예: dept_nm)은 이번 describe
    /// 결과에 안 나타나므로 그대로 뒤에 옮겨 붙인다 - Describe가 그 행을 지워버리면 매번 다시
    /// 추가해야 해서(2026-09-08, "SP 파라미터 아닌 컨트롤도 추가" 요청) 그 행만 골라 보존한다.</summary>
    private void PopulateSearchParams(List<ProcParamInfoDto> queryParams)
    {
        var previousRows = (grdSaveParams.DataSource as DataTable)?.Rows.Cast<DataRow>().ToList()
            ?? new List<DataRow>();
        var previousByName = previousRows
            .GroupBy(r => r["Name"]?.ToString() ?? string.Empty, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

        var table = NewSearchParamsTable();
        var liveNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var p in queryParams)
        {
            if (p.ParamNm.Equals("work_type", StringComparison.OrdinalIgnoreCase)) continue;
            liveNames.Add(p.ParamNm);

            var prev = previousByName.TryGetValue(p.ParamNm, out var pr) ? pr : null;
            var row = table.NewRow();
            row["Name"] = p.ParamNm;
            row["SqlType"] = p.SqlType;
            row["ParamName"] = prev?["ParamName"]?.ToString() is { Length: > 0 } pn ? pn : $"p_{p.ParamNm}";
            // 그리드/panData 컬럼 미리보기(ToColumnPreviewTable)와 같은 기본값 관례 - 처음엔
            // 파라미터명 그대로를 라벨로 쓰고, 사람이 보기 좋은 문구로 고치면 그 값을 유지한다.
            row["Caption"] = prev?["Caption"]?.ToString() is { Length: > 0 } cap ? cap : p.ParamNm;
            row["ControlKind"] = prev?["ControlKind"]?.ToString() is { Length: > 0 } ck ? ck : "TEXT";
            row["LookupKey"] = prev?["LookupKey"]?.ToString() ?? string.Empty;
            row["Include"] = prev != null ? (bool)prev["Include"] : true;
            row["View"] = prev?["View"] is bool prevView ? prevView : true;
            row["Required"] = prev?["Required"] is bool prevRequired && prevRequired;
            table.Rows.Add(row);
        }

        // 사람이 수동으로 추가한, 지금의 프로시저 파라미터 목록에는 없는 행들을 그대로 옮겨 붙인다.
        foreach (var pr in previousRows)
        {
            var name = pr["Name"]?.ToString() ?? string.Empty;
            if (liveNames.Contains(name)) continue;
            table.Rows.Add(pr.ItemArray);
        }

        table.AcceptChanges();
        grdSaveParams.DataSource = table;
    }

    /// <summary>"저장할 프로시저" 계획 그리드(grdSaveProcPlan)의 모든 행을 순서대로 describe/매칭해서
    /// "Save Actions" 탭을 채운다 - 저장프로시저가 여러 개(예: 헤더는 USP_XXX_S, grd2 명세는
    /// 완전히 별도인 USP_XXX_S_1)여도 계획 그리드에 몇 줄이든 먼저 적어두고 이 버튼 한 번으로
    /// 전부 처리한다(2026-09-06 변경 - 예전엔 프로시저 하나씩 입력창에 타이핑→버튼 클릭을
    /// 반복해야 해서 번거로웠다).
    ///
    /// DescribeQueryAsync와 같은 이유(2026-09-08) - Save Actions도 항상 지금의 계획 내용으로
    /// 처음부터 다시 만든다. 계획 그리드에서 잘못 등록한 행을 지운 뒤 Describe를 다시 눌러도
    /// 그 잘못된 프로시저가 만든 Save Actions 행이 그대로 남아있으면 안 되므로, 매번 통째로
    /// 비우고 다시 채운다.</summary>
    private async Task DescribeSaveAsync()
    {
        if (grdSaveProcPlan.DataSource is not DataTable planTable) return;
        if (grdSaveActions.DataSource is not DataTable saTable) return;

        // DescribeQueryAsync와 같은 이유 - 마지막으로 타이핑한 행이 아직 커밋 안 된 채로 읽히는
        // 것을 막는다(2026-09-08).
        gvwSaveProcPlan.CloseEditor();
        gvwSaveProcPlan.UpdateCurrentRow();

        var rows = planTable.Rows.Cast<DataRow>()
            .Where(r => !string.IsNullOrWhiteSpace(r["ProcName"]?.ToString()))
            .ToList();

        saTable.Rows.Clear();
        grdSaveActions.RefreshDataSource();
        grdSaveActionParams.DataSource = new DataTable();

        if (rows.Count == 0)
        {
            AppMessageBox.Show("저장할 프로시저를 한 줄 이상 입력하세요.", "입력 필요");
            return;
        }

        // 이 계획 그리드는 프로시저 이름만 받는다(2026-09-09 - "왼쪽그리드에는 프로시져명만
        // 입력하고 Describe하면 오른쪽에 데이터가 자동으로 만들어져야해" 요청). Query의 work_type과
        // 달리 Scope(Header/Detail)/SourceSlot은 프로시저 텍스트만 보고 안전하게 자동판별할 방법이
        // 없으므로(정적 신호가 없음), GenerateCode()가 이미 쓰는 것과 같은 규칙 - "첫 줄=헤더
        // 저장(행단위 템플릿이면 grd1, 그 외엔 panData), 그 다음 줄부터 순서대로 grd2/grd3/..."로
        // 예측 가능하게 배정한다. 잘못 배정된 줄은 Describe 이후 오른쪽 Save Actions 그리드에서
        // Scope/Source Control을 직접 고치면 된다(그 그리드는 이미 편집 가능 - gvwSaveActions.Role
        // = GridRoleWyn.Edit).
        var isRowLevel = SelectedTemplateKind is TemplateKind.SingleGrid or TemplateKind.MasterSubGrid;
        var headerScope = isRowLevel ? SaveActionScope.Detail : SaveActionScope.Header;
        var headerSlot = isRowLevel ? "grd1" : "panData";
        var detailSlotSequence = new[] { "grd2", "grd3", "grd4", "grd5" };

        var knownColumns = CollectKnownColumns();
        for (var idx = 0; idx < rows.Count; idx++)
        {
            var procName = rows[idx]["ProcName"]!.ToString()!.Trim();
            var scope = idx == 0 ? headerScope : SaveActionScope.Detail;
            var sourceSlot = idx == 0
                ? headerSlot
                : (idx - 1 < detailSlotSequence.Length ? detailSlotSequence[idx - 1] : string.Empty);

            await AutoFillSaveActionAsync(saTable, procName, scope, sourceSlot, knownColumns);
        }

        grdSaveActions.RefreshDataSource();
        Toast.Show($"{rows.Count}개 저장프로시저를 새로 불러왔습니다(이전 결과는 초기화됨) - Save Actions 탭에서 매칭 결과를 확인하세요.");
    }

    /// <summary>previous가 있으면(같은 (Proc,WorkType,ResultSetIndex)을 다시 describe하는 경우)
    /// 컬럼 이름이 같은 이전 행에서 Caption/IncludeInGrid/ControlKind/LookupKey/Required를
    /// 그대로 이어받는다 - 재describe 한 번에 사람이 손으로 고친 설정이 전부 기본값으로 되돌아가는
    /// 것을 막는다(AddQuerySourceRowsAsync 주석 참고, 2026-09-11).</summary>
    private static DataTable ToColumnPreviewTable(List<ProcColumnInfoDto>? columns, DataTable? previous = null)
    {
        var table = new DataTable();
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("SqlType", typeof(string));
        table.Columns.Add("Caption", typeof(string));
        table.Columns.Add("IncludeInGrid", typeof(bool));
        table.Columns.Add("IsKey", typeof(bool));
        table.Columns.Add("ControlKind", typeof(string));
        table.Columns.Add("LookupKey", typeof(string));
        // 이 컬럼목록(masterColumns)은 그리드(grd1~5) 컬럼과 panData 필드 생성에 동시에 쓰인다
        // (ScreenTemplateGenerator 주석 참고) - 그래서 Required 체크 하나로 둘 다 커버된다
        // (2026-09-09 요청). 기본값 false - 필수인 컬럼만 사람이 직접 체크한다.
        table.Columns.Add("Required", typeof(bool));

        var previousByName = previous?.Rows.Cast<DataRow>()
            .ToDictionary(r => r["Name"]?.ToString() ?? string.Empty, r => r, StringComparer.OrdinalIgnoreCase)
            ?? new Dictionary<string, DataRow>(StringComparer.OrdinalIgnoreCase);

        foreach (var c in columns ?? new())
        {
            // ControlKind는 TEXT/CHECK/NUMBER/COMBO/DTE/POP/MEMO 7종 중 하나만 허용(편집기는
            // Designer.cs의 lookUpControlKind - LookUpColumnEdit(LookupKey="L_SM0005"),
            // 2026-09-07에 하드코딩 콤보에서 바꿈, 2026-09-09에 L_SYS_CTRLKIND에서 기초코드 기반
            // L_SM0005로 교체) - bit/날짜형만 자동으로 CHECK/DTE 추정, 나머지는
            // 전부 TEXT로 시작하고 COMBO(LookUp 연결)/POP(팝업 연결)/NUMBER/MEMO는 사람이 직접 고른다.
            var kind = c.SqlType.Equals("bit", StringComparison.OrdinalIgnoreCase) ? "CHECK"
                : c.SqlType is "int" or "decimal" or "numeric" or "money" or "float" or "bigint" or "smallint" ? "NUMBER"
                : c.SqlType is "date" or "datetime" or "datetime2" or "smalldatetime" ? "DTE"
                : "TEXT";

            var prev = previousByName.TryGetValue(c.ColumnNm, out var pr) ? pr : null;
            var caption = prev?["Caption"]?.ToString() is { Length: > 0 } cap ? cap : c.ColumnNm;
            var includeInGrid = prev?["IncludeInGrid"] is bool prevInclude ? prevInclude : true;
            var controlKind = prev?["ControlKind"]?.ToString() is { Length: > 0 } ck ? ck : kind;
            var lookupKey = prev?["LookupKey"]?.ToString() ?? string.Empty;
            var required = prev?["Required"] is bool prevRequired && prevRequired;

            table.Rows.Add(c.ColumnNm, c.SqlType, caption, includeInGrid, false, controlKind, lookupKey, required);
        }
        table.AcceptChanges();
        return table;
    }

    private async Task GenerateCode()
    {
        if (string.IsNullOrWhiteSpace(cboTemplateKind.EditValue?.ToString()))
        {
            AppMessageBox.Show("Template을 선택하세요.", "입력 필요");
            return;
        }

        // BuildQuerySourcesAndSaveActions가 각 그리드의 DataTable을 직접 읽는데, 방금 셀 편집기에서
        // 타이핑만 하고 다른 곳을 안 눌렀으면(예: Column Preview에서 to_unit_cd의 Control을
        // COMBO로 바꾼 직후 바로 "코드생성" 클릭) 그 값이 아직 DataRow에 안 박혀서 통째로 누락된
        // 채 생성된다(2026-09-07 실제 발견 - frmItem11의 to_unit_cd가 COMBO로 지정했는데도 그냥
        // TEXT 컬럼으로 생성됨). AddPlanRow의 CloseEditor+UpdateCurrentRow와 같은 이유로, 읽기
        // 직전에 관련 그리드 전부를 먼저 커밋한다 - 특히 gvwColumnPreview2(그리드 컬럼별 Control/
        // LookupKey)가 이 버그의 실제 원인이었다.
        foreach (var view in new[] { gvwSaveParams, gvwQueryResultSets, gvwColumnPreview2, gvwSaveActions, gvwSaveActionParams })
        {
            view.CloseEditor();
            view.UpdateCurrentRow();
        }

        try
        {
            var spec = new ScreenGenSpec
            {
                Module = cboModule.EditValue.ToString(),
                ScreenClassNm = txtScreenClassNm.Text.Trim(),
                MenuCaption = txtMenuCaption.Text.Trim(),
                UpperMenuId = long.TryParse(txtUpperMenuCd.Text.Trim(), out var upperMenuId) ? upperMenuId : (long?)null,
                ProcPrefix = txtProcPrefix.Text.Trim(),
                Kind = SelectedTemplateKind,
                MasterKeyColumn = string.IsNullOrWhiteSpace(txtMasterKeyColumn.Text) ? null : txtMasterKeyColumn.Text.Trim(),
                MasterParentColumn = string.IsNullOrWhiteSpace(txtMasterParentColumn.Text) ? null : txtMasterParentColumn.Text.Trim(),
                DetailKeyParam = string.IsNullOrWhiteSpace(txtDetailKeyParam.Text) ? null : txtDetailKeyParam.Text.Trim(),
            };

            spec.SearchFields = ReadSearchFields(grdSaveParams.DataSource as DataTable);
            BuildQuerySourcesAndSaveActions(spec);

            // QueryProc/SaveProc/DetailWorkType은 예전엔 위쪽 전용 입력칸(txtQueryProc 등)에서
            // 받았는데, 그 칸들을 없애고 Query Sources/Save Actions 그리드 하나로 합쳤으므로
            // (2026-09-06) 방금 채운 spec.QuerySources/SaveActions에서 그대로 뽑아 쓴다 - 2개
            // 템플릿(SingleGrid/MasterSubGrid)의 공용 ApplyTokens가 __QUERY_PROC__/__SAVE_PROC__/
            // __DETAIL_WORK_TYPE__ 치환에 이 값을 그대로 쓴다(MasterFormSubGrid/MasterOneSheet는
            // 프로시저가 여러 개일 수 있어 각자 자체 LocalApplyTokens를 쓰고 이 필드들을 안 본다,
            // 2026-09-09).
            var grd1Query = spec.QuerySources.FirstOrDefault(q => q.ResultSetBindings.Any(b => b.TargetSlot == "grd1"));
            spec.QueryProc = grd1Query?.ProcName ?? string.Empty;
            spec.DetailWorkType = spec.QuerySources.FirstOrDefault(q => q.ResultSetBindings.Any(b => b.TargetSlot == "grd2"))?.WorkType;

            // Query Sources/Save Actions/검색조건은 전부 "Describe를 눌렀던 시점"의 프로시저
            // 스냅샷을 그대로 코드에 박아 넣는다. 그 뒤 프로시저가 바뀌었는데(컬럼/파라미터 추가・
            // 삭제・이름변경) 다시 Describe를 안 돌리면 실행 전엔 안 드러나다가, 실제로 조회/저장을
            // 눌렀을 때 "프로시저의 매개 변수가 아닙니다"/컬럼을 찾을 수 없다는 SqlException으로
            // 그대로 죽는다(2026-09-07 실제 발견 - frmItem333의 검색조건 item_id가 USP_BA_ITEM_Q의
            // 실제 파라미터인 item_cd/item_nm와 어긋나 있었음). 코드를 만들기 전에 관련 프로시저를
            // 전부 한 번 더 describe해서 지금 스펙이 실제로 유효한지 확인한다 - 템플릿 미선택
            // 가드와 같은 원칙(생성 후가 아니라 생성 전에 막는다).
            var staleProblem = await FindStaleProcReferenceAsync(spec);
            if (staleProblem != null)
            {
                AppMessageBox.Show(
                    staleProblem + "\n\n프로시저가 그 사이 바뀌었을 수 있습니다 - Query Sources/Save Actions 탭에서 Describe를 다시 실행해 확인한 뒤 생성하세요.",
                    "프로시저 확인 필요");
                return;
            }

            var isRowLevel = spec.Kind is TemplateKind.SingleGrid or TemplateKind.MasterSubGrid;
            var headerScope = isRowLevel ? SaveActionScope.Detail : SaveActionScope.Header;
            var headerSlot = isRowLevel ? "grd1" : "panData";
            spec.SaveProc = spec.SaveActions.FirstOrDefault(a => a.Scope == headerScope && a.SourceSlot == headerSlot)?.ProcName;

            var result = ScreenTemplateGenerator.Generate(spec, RepoRoot);
            var menuMsg = await RegisterMenuAsync(spec);

            AppMessageBox.Show("다음 파일이 생성/수정됐습니다:\n\n" + string.Join("\n", result.CreatedFiles) + "\n\n" + menuMsg, "생성 완료");
        }
        catch (Exception ex)
        {
            AppMessageBox.Show(ex.Message, "생성 실패");
        }
    }

    /// <summary>grdQueryResultSets/grdSaveActions(DescribeQueryAsync/DescribeSaveAsync가 채워둔다)를
    /// 그대로 QuerySources/SaveActions로 옮긴다 - 템플릿 종류와 무관하게 이 하나의 변환만 쓴다.</summary>
    private void BuildQuerySourcesAndSaveActions(ScreenGenSpec spec)
    {
        spec.QuerySources = new List<QuerySource>();
        if (grdQueryResultSets.DataSource is DataTable qrTable)
        {
            foreach (var group in qrTable.Rows.Cast<DataRow>()
                .GroupBy(r => (Proc: r["ProcName"]?.ToString() ?? string.Empty, WorkType: r["WorkType"]?.ToString() ?? string.Empty)))
            {
                if (string.IsNullOrWhiteSpace(group.Key.Proc)) continue;
                spec.QuerySources.Add(new QuerySource
                {
                    ProcName = group.Key.Proc,
                    WorkType = group.Key.WorkType,
                    ResultSetBindings = group.Select(r => new ResultSetBinding
                    {
                        ResultSetIndex = r["ResultSetIndex"] is int i ? i : 0,
                        TargetSlot = r["TargetSlot"]?.ToString() ?? string.Empty,
                        Columns = r["ColumnsTable"] is DataTable ct ? ReadColumnSpecs(ct) : new List<ColumnSpec>(),
                    }).ToList()
                });
            }
        }

        spec.SaveActions = new List<SaveAction>();
        if (grdSaveActions.DataSource is DataTable saTable)
        {
            foreach (DataRow row in saTable.Rows)
            {
                var procName = row["ProcName"]?.ToString();
                if (string.IsNullOrWhiteSpace(procName)) continue;

                var action = new SaveAction
                {
                    // net48의 string.IsNullOrWhiteSpace는 [NotNullWhen(false)]가 없어(.NET Core+에서만
                    // 추가됨) 바로 위 가드로 이미 null이 아님을 확인했는데도 컴파일러가 못 알아채고
                    // 경고한다 - 실제로는 안전하다.
                    ProcName = procName!,
                    Scope = string.Equals(row["Scope"]?.ToString(), "Header", StringComparison.OrdinalIgnoreCase)
                        ? SaveActionScope.Header : SaveActionScope.Detail,
                    SourceSlot = row["SourceSlot"]?.ToString() ?? string.Empty,
                    KeyParam = string.IsNullOrWhiteSpace(row["KeyParam"]?.ToString()) ? null : row["KeyParam"]!.ToString(),
                };
                if (row["ParamsTable"] is DataTable pt)
                {
                    foreach (DataRow pr in pt.Rows)
                    {
                        var paramNm = pr["Name"].ToString()!;
                        action.SaveParams.Add(paramNm);
                        var matched = pr["MatchedColumn"].ToString();
                        if (!string.IsNullOrWhiteSpace(matched)) action.SaveParamColumnMap[paramNm] = matched;
                    }
                }
                spec.SaveActions.Add(action);
            }
        }
    }

    /// <summary>코드생성과 같은 값으로 TSMMENU에도 바로 등록한다(api/menus - MENU_ID는 서버가
    /// IDENTITY로 채번). 마이그레이션 .sql 파일(BuildMenuSql)도 그대로 같이 생성되므로, 다른
    /// 환경(운영 DB 등)에 반영할 때는 그 파일로 리뷰 후 적용한다 - 여기서의 등록은 지금 붙어있는
    /// 서버(보통 로컬 개발 DB) 한정이다.</summary>
    private async Task<string> RegisterMenuAsync(ScreenGenSpec spec)
    {
        try
        {
            var req = new MenuCreateRequest
            {
                MenuNm = spec.MenuCaption,
                UpperMenuId = spec.UpperMenuId,
                MenuLevel = 3,
                MenuType = "FORM",
                Module = spec.Module,
                ScreenClassNm = spec.ScreenClassNm,
                ProcPrefix = spec.ProcPrefix,
                SortOrder = 10,
            };

            var result = await ApiClient.PostAsync<MenuCreateRequest, ApiResult>("api/menus", req);
            if (result == null)
                return "메뉴 등록 결과를 확인하지 못했습니다 - 메뉴관리 화면에서 직접 확인/등록하세요.";

            return result.Success
                ? "TSMMENU에도 등록됐습니다 - 재로그인(또는 메뉴 새로고침) 후 메뉴트리에 보입니다."
                : $"메뉴 등록에 실패했습니다({result.Message}).";
        }
        catch (Exception ex)
        {
            return $"메뉴 자동등록에 실패했습니다({ex.Message}) - 메뉴관리 화면에서 직접 등록하세요.";
        }
    }

    /// <summary>"검색조건" 탭(grdSaveParams, Include 체크된 것만)에서 panHeader에 만들 컨트롤
    /// 목록을 뽑는다 - ParamName이 채워진 항목만 조회 시 실제 파라미터로 보내진다(비어있으면
    /// 화면표시 전용). Name이 비어있는 행(추가만 하고 아직 안 채운 빈 행)은 건너뛴다.</summary>
    private static List<ColumnSpec> ReadSearchFields(DataTable? table)
    {
        var list = new List<ColumnSpec>();
        if (table == null) return list;
        foreach (DataRow row in table.Rows)
        {
            if (row["Include"] is not bool include || !include) continue;
            var name = row["Name"]?.ToString() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(name)) continue;

            list.Add(new ColumnSpec
            {
                Name = name,
                SqlType = row["SqlType"]?.ToString() is { Length: > 0 } sqlType ? sqlType : "nvarchar",
                Caption = row["Caption"]?.ToString() is { Length: > 0 } cap ? cap : name,
                ControlKind = row["ControlKind"]?.ToString() is { Length: > 0 } ck ? ck : "TEXT",
                LookupKey = row["LookupKey"]?.ToString(),
                ParamName = row["ParamName"]?.ToString(),
                Visible = row["View"] is bool view ? view : true,
                Required = row["Required"] is bool required && required,
            });
        }
        return list;
    }

    private static List<ColumnSpec> ReadColumnSpecs(DataTable? table)
    {
        var list = new List<ColumnSpec>();
        if (table == null) return list;

        foreach (DataRow row in table.Rows)
        {
            list.Add(new ColumnSpec
            {
                Name = row["Name"].ToString() ?? string.Empty,
                SqlType = row["SqlType"].ToString() ?? "nvarchar",
                Caption = row["Caption"].ToString() is { Length: > 0 } cap ? cap : row["Name"].ToString() ?? string.Empty,
                IncludeInGrid = row.Table.Columns.Contains("IncludeInGrid") && (bool)row["IncludeInGrid"],
                IsKey = row.Table.Columns.Contains("IsKey") && (bool)row["IsKey"],
                ControlKind = row["ControlKind"].ToString() is { Length: > 0 } ck ? ck : "TEXT",
                LookupKey = row.Table.Columns.Contains("LookupKey") ? row["LookupKey"].ToString() : null,
                Required = row.Table.Columns.Contains("Required") && (bool)row["Required"],
            });
        }
        return list;
    }

}
