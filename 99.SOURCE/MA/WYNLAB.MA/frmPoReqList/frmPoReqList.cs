using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

/// <summary>
/// 구매요청현황 - AI Builder Master-SubGrid 유형(좌측 grd1 목록, 우측 grd2는 선택행의 품목
/// 상세). 둘 다 조회전용이다 - 실제 등록/수정은 항상 frmPoReq에서만 한다(진행상태/결재정보를
/// 이 화면에서 직접 고치면 저장 프로시저가 없어 무의미하므로 편집 자체를 막았다). 구매요청번호
/// 컬럼을 더블클릭하면 frmPoReq를 그 건으로 열어 조회한다.
/// </summary>
public partial class frmPoReqList : BaseForm
{
    private DataTable _list = new();
    private DataTable _detail = new();

    public frmPoReqList()
    {
        InitializeComponent();

        Text = "구매요청현황";

        Controls.Add(BuildScreenHeader());

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.FocusedRowObjectChanged += async (s, e) => await LoadDetailAsync();
        gvw1.DoubleClick += async (s, e) => await SafeExecuteAsync(OpenReqAsync, "구매요청등록 열기");

        gvw2.Role = GridRoleWyn.Query;
        gvw2.HighlightFocusedRow = true;

        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_req_no"] = txtSearchReqNo.Text,
            ["p_req_title"] = txtSearchReqTitle.Text,
        };
        _list = await QueryAsync("USP_MA_POREQLIST_Q", p);
        grd1.DataSource = _list;
        grd2.DataSource = null;
    }

    /// <summary>grd1 선택행이 바뀔 때만 grd2(품목 상세)를 다시 조회한다(frmDept.cs 패턴) -
    /// MAIN/SUB 그리드 관례대로, 포커스 행이 없으면 SUB도 반드시 비운다.</summary>
    private async Task LoadDetailAsync()
    {
        if (gvw1.GetFocusedRow() is not DataRowView view) { grd2.DataSource = null; return; }

        _detail = await QueryAsync("USP_MA_POREQLIST_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q1",
            ["p_req_id"] = view.Row["req_id"]?.ToString(),
        });
        grd2.DataSource = _detail;
    }

    /// <summary>구매요청번호 더블클릭 - 같은 모듈(MA) 안이라 리플렉션 없이 바로 새 인스턴스를
    /// 열고 FocusRecordAsync(req_id)로 그 건에 포커스시킨다(frmApprInbox.OpenOriginalDocumentAsync
    /// 와 같은 원리, 모듈이 같으면 ModuleLoader가 필요 없다는 점만 다르다).</summary>
    private async Task OpenReqAsync()
    {
        if (gvw1.FocusedColumn != colReqNo) return;
        if (gvw1.GetFocusedRow() is not DataRowView view) return;

        var reqId = view.Row["req_id"]?.ToString();
        if (string.IsNullOrEmpty(reqId)) return;

        var menu = SessionManager.Current.Menus.FirstOrDefault(m => m.Module == "MA" && m.ScreenClassNm == "frmPoReq");
        var form = new frmPoReq { MenuId = menu?.MenuId ?? 0, MdiParent = MdiParent };
        form.Show();
        await form.FocusRecordAsync(reqId);
    }

    public override Task NewClick() => Task.CompletedTask;
    public override Task DeleteClick() => Task.CompletedTask;
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;
    public override Task SaveClick() => Task.CompletedTask;
}
