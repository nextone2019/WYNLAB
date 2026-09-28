using System.Data;
using DevExpress.XtraEditors;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

/// <summary>
/// 구매요청 불러오기 팝업 - frmPo에서 승인 완료된 구매요청의 잔량 품목을 골라 발주 품목으로 가져온다.
/// 코드+명 한 건을 고르는 공용 팝업 프레임워크(P_ 팝업)로는 여러 행을 체크해서 통째로 가져올 수 없어서
/// 별도 팝업으로 만들었다. popApp/popFileUpload와 같은 이유로 BaseForm이 아니라 XtraForm을 상속하고,
/// 데이터는 부른 화면(frmPo)의 MenuId로 범용 통로(ProcData)를 탄다 - USP_MA_POREQPICK_Q는 frmPo 메뉴의
/// 프로시저 접두사(USP_MA_) 안에 있다. 배치는 popReqPick.Designer.cs(VS 디자이너로 편집 가능).
/// </summary>
public partial class popReqPick : XtraForm
{
    private readonly long _menuId;
    private readonly string? _accId;
    private readonly string? _custId;
    private DataTable _table = new();

    /// <summary>체크한 행들(조회 결과와 같은 컬럼 구성 + sel). 확인(불러오기)으로 닫았을 때만 채워진다.</summary>
    public DataTable Selected { get; private set; } = new();

    /// <summary>VS 디자이너 전용 생성자 - 실제 코드에서는 호출하지 않는다.</summary>
    private popReqPick() : this(0, null, null, null) { }

    private popReqPick(long menuId, string? accId, string? custId, string? custNm)
    {
        InitializeComponent();

        _menuId = menuId;
        _accId = accId;
        _custId = string.IsNullOrWhiteSpace(custId) ? null : custId;

        lblCust.Text = _custId == null
            ? "거래처: 전체 (고른 품목의 거래처가 발주 거래처가 됩니다. 서로 다른 거래처는 한 번에 못 가져옵니다)"
            : $"거래처: {custNm}";

        dteFrom.YyyyMmDd = DateTime.Today.AddMonths(-3).ToString("yyyyMMdd");
        dteTo.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");

        gvw1.Role = GridRoleWyn.Edit; // sel 체크박스만 편집 가능(나머지 컬럼은 디자이너에서 AllowEdit=false)
        gvw1.HighlightFocusedRow = true;

        btnSearch.Click += async (s, e) => await SearchAsync();
        btnOk.Click += (s, e) => Confirm();
        btnClose.Click += (s, e) => DialogResult = DialogResult.Cancel;
        chkAll.CheckedChanged += (s, e) => SetAll(chkAll.Checked);
        txtReqNo.KeyDown += async (s, e) => { if (e.KeyCode == Keys.Enter) await SearchAsync(); };
        txtKeyword.KeyDown += async (s, e) => { if (e.KeyCode == Keys.Enter) await SearchAsync(); };

        Shown += async (s, e) => await SearchAsync();
    }

    /// <summary>팝업을 띄우고 체크한 행을 돌려준다(취소하거나 아무것도 안 고르면 null).</summary>
    public static DataTable? Pick(IWin32Window owner, long menuId, string? accId, string? custId, string? custNm)
    {
        using var dlg = new popReqPick(menuId, accId, custId, custNm);
        return dlg.ShowDialog(owner) == DialogResult.OK ? dlg.Selected : null;
    }

    private async Task SearchAsync()
    {
        try
        {
            var p = new Dictionary<string, string?>
            {
                ["p_work_type"] = "Q",
                ["p_acc_id"] = _accId,
                ["p_date_from"] = dteFrom.YyyyMmDd,
                ["p_date_to"] = dteTo.YyyyMmDd,
                ["p_req_no"] = txtReqNo.Text,
                ["p_cust_id"] = _custId,
                ["p_keyword"] = txtKeyword.Text,
            };
            _table = await ProcData.QueryAsync(_menuId, "USP_MA_POREQPICK_Q", p);
            if (!_table.Columns.Contains("sel")) _table.Columns.Add("sel", typeof(bool));
            foreach (DataRow r in _table.Rows) r["sel"] = false;

            chkAll.Checked = false;
            grd1.DataSource = _table;
        }
        catch (Exception ex)
        {
            AppMessageBox.Show(ex.Message, "구매요청 조회 실패");
        }
    }

    private void SetAll(bool value)
    {
        gvw1.CloseEditor();
        foreach (DataRow r in _table.Rows) r["sel"] = value;
    }

    private void Confirm()
    {
        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        var picked = _table.Rows.Cast<DataRow>().Where(r => r["sel"] is bool b && b).ToList();
        if (picked.Count == 0)
        {
            AppMessageBox.Show("불러올 품목을 체크하세요.", "안내");
            return;
        }

        // 발주 헤더 거래처는 하나라서, 서로 다른 거래처의 품목을 한 번에 가져올 수 없다.
        var custIds = picked.Select(r => r["cust_id"]?.ToString() ?? string.Empty).Distinct().ToList();
        if (custIds.Count > 1)
        {
            AppMessageBox.Show("서로 다른 거래처의 품목은 한 번에 불러올 수 없습니다. 거래처별로 나누어 불러오세요.", "안내");
            return;
        }

        Selected = _table.Clone();
        foreach (var r in picked) Selected.ImportRow(r);
        DialogResult = DialogResult.OK;
    }
}
