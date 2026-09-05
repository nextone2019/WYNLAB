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

    // 라벨은 아직 이름 정리 전(합의 대기) - 화면구조가 바뀌는 게 아니라 표시 문구만 바뀔
    // 사안이라 우선 그대로 둔다. 아이콘(TemplateIconFiles)은 이름과 무관하게 실제 화면구조를
    // 그린 것이라 먼저 넣는다.
    private static readonly (TemplateKind Kind, string Label)[] TemplateOptions =
    {
        (TemplateKind.SingleGrid, "Single Grid"),
        (TemplateKind.MasterSubGrid, "Master-Sub Grid"),
        (TemplateKind.MasterFormSubGrid, "Master-Form-Sub Grid"),
        (TemplateKind.MasterFormTabGrid, "Master-Form-Tab Grid"),
    };

    // TemplateOptions와 같은 순서 - Assets/TemplateIcons/*.svg(TemplateIcons.cs가 로드),
    // 실제 템플릿 화면의 구역 배치를 축소한 와이어프레임. MasterFormTabGrid는 전용 아이콘을
    // 아직 안 그려서 임시로 master_form_grid.svg를 재사용한다(구조가 제일 비슷함) - 나중에
    // 탭 2개짜리 전용 아이콘으로 교체할 것.
    private static readonly string[] TemplateIconFiles = { "grid.svg", "master_grid.svg", "master_form_grid.svg", "master_form_grid.svg" };

    public frmAIBuilder()
    {
        InitializeComponent();

        Text = "AI Builder";

        Controls.Add(BuildScreenHeader());

        //cboModule.Properties.Items.AddRange(new[] { "SM", "BA", "SA", "PR", "SYS", "MA" });
       // cboModule.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
       // cboModule.SelectedIndex = 0;

        // 템플릿 3종을 이름만으로 고르면 구조가 헷갈린다는 피드백에 따라, 실제 화면 레이아웃을
        // 축소한 아이콘(TemplateIcons.cs)과 함께 고르도록 ImageComboBoxEdit을 쓴다.
        var templateImages = new ImageList { ImageSize = new Size(24, 18), ColorDepth = ColorDepth.Depth32Bit };
        foreach (var iconFile in TemplateIconFiles)
            templateImages.Images.Add(TemplateIcons.Load(iconFile, 24) ?? new Bitmap(24, 18));
        cboTemplateKind.Properties.SmallImages = templateImages;
        cboTemplateKind.Properties.Items.AddRange(TemplateOptions.Select((o, i) =>
            new ImageComboBoxItem(o.Label, o.Kind.ToString(), i)).ToArray());
        cboTemplateKind.EditValue = TemplateOptions[0].Kind.ToString();

        // Save Params 탭은 원래 용도(저장 파라미터 매칭)를 잃었으니 검색조건(panHeader) 선택용으로
        // 재활용한다 - grd1 조회프로시저(work_type='Q')의 파라미터 중 work_type을 뺀 나머지를
        // 보여주고 체크박스로 "이건 검색창으로 노출"만 고르면 된다(2026-09-04 추가). 컬럼 3개
        // 그리드가 마침 Name/SqlType/체크형태로 딱 맞아서 새 탭 없이 재사용한다.
        tabSaveParams.Text = "검색조건";
        gvwSaveParams.Role = GridRoleWyn.Edit;
        colSpName.OptionsColumn.AllowEdit = false;
        colSpSqlType.OptionsColumn.AllowEdit = false;
        colSpMatchedColumn.Caption = "검색조건 포함";
        colSpMatchedColumn.FieldName = "Include";
        var includeEdit = new RepositoryItemCheckEdit();
        grdSaveParams.RepositoryItems.Add(includeEdit);
        colSpMatchedColumn.ColumnEdit = includeEdit;
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

        // TargetSlot/SourceSlot은 이 템플릿(MasterFormTabGrid)이 실제로 갖고 있는 컨트롤 이름
        // 뿐이라 자유입력 대신 드롭다운으로 고정한다 - 오타로 생성이 실패하는 걸 원천 차단.
        var targetSlotEdit = new RepositoryItemComboBox { TextEditStyle = TextEditStyles.DisableTextEditor };
        targetSlotEdit.Items.AddRange(new object[] { "grd1", "grd2", "grd3" });
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
        sourceSlotEdit.Items.AddRange(new object[] { "grd1", "panData", "grd2", "grd3" });
        grdSaveActions.RepositoryItems.Add(sourceSlotEdit);
        colSaSourceSlot.ColumnEdit = sourceSlotEdit;

        btnDescribeQuery.Click += async (s, e) => await DescribeQueryAsync();
        btnDescribeSave.Click += async (s, e) => await DescribeSaveAsync();
        btnDescribeQueryMulti.Click += async (s, e) => await DescribeQueryMultiAsync();
        btnDescribeSaveMulti.Click += async (s, e) => await DescribeSaveActionAsync();
        btnGenerate.Click += async (s, e) => await GenerateCode();
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
        t.Columns.Add("Include", typeof(bool));
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
        foreach (var r in toRemove) table.Rows.Remove(r);

        foreach (var rs in result.ResultSets)
        {
            var row = table.NewRow();
            row["ProcName"] = procName;
            row["WorkType"] = workType;
            row["ResultSetIndex"] = rs.Index;
            row["TargetSlot"] = rs.Index < targetSlotOrder.Length ? targetSlotOrder[rs.Index] : string.Empty;
            row["ColumnsTable"] = ToColumnPreviewTable(rs.Columns);
            table.Rows.Add(row);
        }
        table.AcceptChanges();
        return (result.ResultSets.Count, result.Params);
    }

    /// <summary>선택된 grdQueryResultSets 행의 Proc/WorkType으로 레코드셋을 읽어온다(수동 경로 -
    /// MasterFormTabGrid에서 Query Proc/Detail WorkType 관례를 벗어나는 조회를 추가로 붙이고
    /// 싶을 때만 쓴다. 보통은 DescribeQueryAsync가 자동으로 채워준다).</summary>
    private async Task DescribeQueryMultiAsync()
    {
        if (grdQueryResultSets.DataSource is not DataTable table) return;
        if (gvwQueryResultSets.GetFocusedRow() is not DataRowView focused)
        {
            AppMessageBox.Show("먼저 조회할 행을 선택하세요(Proc/WorkType 입력 후).", "선택 필요");
            return;
        }

        var procName = focused.Row["ProcName"]?.ToString();
        var workType = focused.Row["WorkType"]?.ToString();
        if (string.IsNullOrWhiteSpace(procName) || string.IsNullOrWhiteSpace(workType))
        {
            AppMessageBox.Show("선택한 행의 Proc/WorkType을 입력하세요.", "입력 필요");
            return;
        }

        var (count, _) = await AddQuerySourceRowsAsync(table, procName, workType, Array.Empty<string>());
        grdQueryResultSets.RefreshDataSource();
        Toast.Show($"레코드셋 {count}개를 불러왔습니다 - Target Slot을 선택해주세요.");
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

    /// <summary>선택된 grdSaveActions 행의 Proc로 파라미터를 describe/매칭한다(수동 경로 - Save
    /// Proc 관례(_1/_2)를 벗어나는 저장액션을 추가로 붙이고 싶을 때만 쓴다. 보통은 DescribeSaveAsync가
    /// 자동으로 채워준다).</summary>
    private async Task DescribeSaveActionAsync()
    {
        if (grdSaveActions.DataSource is not DataTable table) return;
        if (gvwSaveActions.GetFocusedRow() is not DataRowView focused)
        {
            AppMessageBox.Show("먼저 저장액션 행을 선택하세요(Proc 입력 후).", "선택 필요");
            return;
        }

        var procName = focused.Row["ProcName"]?.ToString();
        if (string.IsNullOrWhiteSpace(procName))
        {
            AppMessageBox.Show("선택한 행의 Proc를 입력하세요.", "입력 필요");
            return;
        }

        var paramsTable = await DescribeSaveParamsTableAsync(procName, CollectKnownColumns());
        focused.Row["ParamsTable"] = paramsTable;
        table.AcceptChanges();
        grdSaveActionParams.DataSource = paramsTable;

        var matchedCount = paramsTable.Rows.Cast<DataRow>().Count(r => !string.IsNullOrWhiteSpace(r["MatchedColumn"].ToString()));
        Toast.Show($"파라미터 {paramsTable.Rows.Count}개 중 {matchedCount}개 자동매칭됐습니다.");
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

    /// <summary>Query Proc(work_type="Q")로 grd1을, 필요하면 Detail WorkType으로 나머지 그리드를
    /// 채운다 - 전부 "Query Sources" 탭 한 곳에만 쓴다(템플릿마다 슬롯 개수만 다르다).
    /// SingleGrid는 detail이 없고, MasterSubGrid/MasterFormSubGrid는 grd2 하나, MasterFormTabGrid는
    /// 레코드셋 2개짜리 조회 하나로 grd2+grd3를 같이 채운다.</summary>
    private async Task DescribeQueryAsync()
    {
        if (string.IsNullOrWhiteSpace(txtQueryProc.Text))
        {
            AppMessageBox.Show("Query Proc를 입력하세요.", "입력 필요");
            return;
        }
        if (grdQueryResultSets.DataSource is not DataTable qrTable) return;

        var (_, masterParams) = await AddQuerySourceRowsAsync(qrTable, txtQueryProc.Text.Trim(), "Q", new[] { "grd1" });
        PopulateSearchParams(masterParams);

        if (SelectedTemplateKind != TemplateKind.SingleGrid && !string.IsNullOrWhiteSpace(txtDetailWorkType.Text))
        {
            var detailSlots = SelectedTemplateKind == TemplateKind.MasterFormTabGrid
                ? new[] { "grd2", "grd3" }
                : new[] { "grd2" };
            var (count, _) = await AddQuerySourceRowsAsync(qrTable, txtQueryProc.Text.Trim(), txtDetailWorkType.Text.Trim(), detailSlots);
            if (SelectedTemplateKind == TemplateKind.MasterFormTabGrid && count > 2)
                AppMessageBox.Show($"{txtDetailWorkType.Text} 레코드셋이 {count}개라 grd2/grd3에 자동배정된 처음 2개 외 나머지는 Query Sources 탭에서 TargetSlot을 직접 지정해야 합니다(이 템플릿은 grd2/grd3까지만 지원).", "확인 필요");
        }

        grdQueryResultSets.RefreshDataSource();
        Toast.Show("컬럼을 불러왔습니다.");
    }

    /// <summary>grd1 조회프로시저(work_type="Q")의 파라미터를 "검색조건" 탭에 채운다 - work_type은
    /// 화면 코드가 직접 처리하므로 후보에서 뺀다. 기존에 사람이 체크해둔 값(Include)은 같은
    /// 파라미터명이면 그대로 유지한다 - Describe Query를 다시 눌러도 체크 상태가 안 날아가게.</summary>
    private void PopulateSearchParams(List<ProcParamInfoDto> queryParams)
    {
        var previous = (grdSaveParams.DataSource as DataTable)?.Rows.Cast<DataRow>()
            .ToDictionary(r => r["Name"]?.ToString() ?? string.Empty, r => (bool)r["Include"], StringComparer.OrdinalIgnoreCase)
            ?? new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

        var table = NewSearchParamsTable();
        foreach (var p in queryParams)
        {
            if (p.ParamNm.Equals("work_type", StringComparison.OrdinalIgnoreCase)) continue;
            table.Rows.Add(p.ParamNm, p.SqlType, previous.TryGetValue(p.ParamNm, out var wasIncluded) ? wasIncluded : true);
        }
        table.AcceptChanges();
        grdSaveParams.DataSource = table;
    }

    /// <summary>Save Proc 하나로 저장액션을 자동으로 채운다 - "Save Actions" 탭 한 곳에만 쓴다.
    /// SingleGrid/MasterSubGrid는 grd1 자체가 저장 대상(Detail scope, 행별 반복저장)이고,
    /// MasterFormSubGrid/MasterFormTabGrid는 panData가 헤더(Header scope, 1회 저장)다.
    /// MasterFormTabGrid는 여기에 더해 Save Proc에 "_1"/"_2"를 붙인 이름을 명세 저장프로시저로
    /// 추정해서 grd2/grd3 저장액션까지 자동으로 채운다(USP_XXX_S -> USP_XXX_S_1/_2 - 071
    /// 마이그레이션 등 기존 화면들이 실제로 따르는 명명 규칙). 추정이 틀리면(프로시저명이 이
    /// 규칙을 안 따르면) "Save Actions" 탭에서 직접 고치면 된다.</summary>
    private async Task DescribeSaveAsync()
    {
        if (string.IsNullOrWhiteSpace(txtSaveProc.Text))
        {
            AppMessageBox.Show("Save Proc를 입력하세요.", "입력 필요");
            return;
        }
        if (grdSaveActions.DataSource is not DataTable saTable) return;

        var headerProc = txtSaveProc.Text.Trim();
        var isRowLevel = SelectedTemplateKind is TemplateKind.SingleGrid or TemplateKind.MasterSubGrid;
        var headerScope = isRowLevel ? SaveActionScope.Detail : SaveActionScope.Header;
        var headerSlot = isRowLevel ? "grd1" : "panData";

        var knownColumns = CollectKnownColumns();
        await AutoFillSaveActionAsync(saTable, headerProc, headerScope, headerSlot, knownColumns);

        if (SelectedTemplateKind == TemplateKind.MasterFormTabGrid)
        {
            await AutoFillSaveActionAsync(saTable, headerProc + "_1", SaveActionScope.Detail, "grd2", knownColumns);
            await AutoFillSaveActionAsync(saTable, headerProc + "_2", SaveActionScope.Detail, "grd3", knownColumns);
        }

        grdSaveActions.RefreshDataSource();
        Toast.Show("저장 파라미터를 불러왔습니다 - Save Actions 탭에서 매칭 결과를 확인하세요.");
    }

    private static DataTable ToColumnPreviewTable(List<ProcColumnInfoDto>? columns)
    {
        var table = new DataTable();
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("SqlType", typeof(string));
        table.Columns.Add("Caption", typeof(string));
        table.Columns.Add("IncludeInGrid", typeof(bool));
        table.Columns.Add("IsKey", typeof(bool));
        table.Columns.Add("ControlKind", typeof(string));
        table.Columns.Add("LookupKey", typeof(string));

        foreach (var c in columns ?? new())
        {
            // ControlKind는 TEXT/CHECK/NUMBER/COMBO 4종 중 하나만 허용(콤보 편집기는 Designer.cs의
            // cboEditControlKind) - bit만 자동으로 CHECK 추정, 나머지는 전부 TEXT로 시작하고
            // COMBO(LookUp 연결)/NUMBER는 사람이 직접 고른다.
            var kind = c.SqlType.Equals("bit", StringComparison.OrdinalIgnoreCase) ? "CHECK"
                : c.SqlType is "int" or "decimal" or "numeric" or "money" or "float" or "bigint" or "smallint" ? "NUMBER"
                : "TEXT";
            table.Rows.Add(c.ColumnNm, c.SqlType, c.ColumnNm, true, false, kind, string.Empty);
        }
        table.AcceptChanges();
        return table;
    }

    private async Task GenerateCode()
    {
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
                QueryProc = txtQueryProc.Text.Trim(),
                DetailWorkType = string.IsNullOrWhiteSpace(txtDetailWorkType.Text) ? null : txtDetailWorkType.Text.Trim(),
                SaveProc = string.IsNullOrWhiteSpace(txtSaveProc.Text) ? null : txtSaveProc.Text.Trim(),
                MasterKeyColumn = string.IsNullOrWhiteSpace(txtMasterKeyColumn.Text) ? null : txtMasterKeyColumn.Text.Trim(),
                DetailKeyParam = string.IsNullOrWhiteSpace(txtDetailKeyParam.Text) ? null : txtDetailKeyParam.Text.Trim(),
            };

            spec.QueryParams = ReadSearchParams(grdSaveParams.DataSource as DataTable);
            BuildQuerySourcesAndSaveActions(spec);

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
                    ProcName = procName,
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

    /// <summary>"검색조건" 탭(grdSaveParams, Include 체크된 것만)에서 panHeader 검색창으로 만들
    /// 파라미터명 목록을 뽑는다.</summary>
    private static List<string> ReadSearchParams(DataTable? table)
    {
        var list = new List<string>();
        if (table == null) return list;
        foreach (DataRow row in table.Rows)
            if (row["Include"] is bool include && include)
                list.Add(row["Name"]?.ToString() ?? string.Empty);
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
            });
        }
        return list;
    }

}
