using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

/// <summary>
/// 웨이퍼입고 - 공급처(TSMC 등)에서 첫 공정 외주처(Amkor)로 직송되어 도착한 웨이퍼를 작업지시와 무관하게 먼저 등록한다(TPRRCV).
/// 품목/LOT번호/수량/입고 창고를 직접 입력해 저장 후 [입고 확정]하면 창고에 미배정 LOT 재고가 생기고, 작업지시가 나중에 이 LOT를 골라 배정한다.
/// 같은 LOT를 나눠 입고해도 되며, 작업지시에 배정된 LOT는 추가 입고/확정취소가 서버에서 막힌다.
/// </summary>
public partial class frmRcv : BaseForm
{
    private DataTable _list = new();
    private string? _editingKey; // 선택한(저장된) 입고의 rcv_id. null이면 신규모드
    private string _statCd = "0";
    private int _loadSeq;        // 목록 행을 빠르게 넘길 때 늦게 온 응답이 최신 선택을 덮어쓰지 않게 하는 번호

    public frmRcv()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준(2026-10-03): 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "웨이퍼입고";

        Controls.Add(BuildScreenHeader());

        txtItemNm.MapField("item_id", txtItemId);
        txtWhNm.MapField("wh_id", txtWhId);
        txtSupNm.MapField("cust_id", txtSupId);

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        btnConfirm.Click += async (s, e) => await SafeExecuteAsync(() => ConfirmAsync(true), "웨이퍼 입고 확정");
        btnConfirmCancel.Click += async (s, e) => await SafeExecuteAsync(() => ConfirmAsync(false), "웨이퍼 입고 확정취소");

        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) => await SafeExecuteAsync(QueryClick, "웨이퍼 입고 목록 조회");
    }

    private static decimal Dec(object? value) => decimal.TryParse(value?.ToString(), out var d) ? d : 0;

    // ===== 조회(좌측 목록) =====

    // 사용자가 조회를 누른 경우(preserveSelection: false)와 저장/삭제 뒤 내부 재조회(true, 방금 편집하던 행 유지)는 다른 동작이어야 한다(라우팅관리와 같은 규칙).
    public override async Task QueryClick() => await QueryCore(preserveSelection: false);

    private async Task QueryCore(bool preserveSelection)
    {
        _list = await QueryAsync("USP_PR_RCV_Q", new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "L",
            ["p_keyword"] = txtSearchKeyword.Text.Trim(),
            ["p_acc_id"] = Session.AccId?.ToString(),
        });

        var editingKey = preserveSelection ? _editingKey : null;

        if (editingKey == null)
        {
            grd1.DataSource = _list; // 재바인딩하면 첫 행 포커스 -> FocusedRowObjectChanged -> 우측 채움
            if (_list.Rows.Count == 0) EnterNewMode();
            return;
        }

        var targetRow = _list.Rows.Cast<DataRow>().FirstOrDefault(r => string.Equals(r["rcv_id"]?.ToString(), editingKey, StringComparison.OrdinalIgnoreCase));

        gvw1.FocusedRowObjectChanged -= Gvw1_FocusedRowObjectChanged;
        SuppressMasterRowSwitchConfirm(gvw1, () =>
        {
            grd1.DataSource = _list;
            if (targetRow != null)
            {
                var handle = gvw1.GetRowHandle(_list.Rows.IndexOf(targetRow));
                if (handle >= 0) gvw1.FocusedRowHandle = handle;
            }
        });
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        if (targetRow != null) await LoadRcvAsync(editingKey);
        else EnterNewMode();
    }

    /// <summary>목록에서 다른 행을 고를 때 우측 편집 내용에 저장 안 된 변경이 있으면 먼저 확인한다(BaseForm.ConfirmMasterRowSwitch).</summary>
    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e) =>
        ConfirmMasterRowSwitch(gvw1, e, row => _ = SafeExecuteAsync(() => LoadRcvAsync(row.Row["rcv_id"]?.ToString()), "웨이퍼 입고 조회"));

    private async Task LoadRcvAsync(string? rcvId)
    {
        if (string.IsNullOrEmpty(rcvId)) { EnterNewMode(); return; }

        var seq = ++_loadSeq;
        var tables = await QueryMultiAsync("USP_PR_RCV_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q", ["p_rcv_id"] = rcvId });
        if (seq != _loadSeq) return; // 그 사이 다른 행을 골랐다 - 이 응답은 버린다

        if (tables.Count > 0 && tables[0].Rows.Count > 0) OnRowLoaded(tables[0].Rows[0]);
        else EnterNewMode();
    }

    private void OnRowLoaded(DataRow row)
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = row["rcv_id"]?.ToString();
            _statCd = row["stat_cd"]?.ToString() ?? "0";
            cboAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            cboStatCd.EditValue = _statCd;
            txtRcvNo.Text = row["rcv_no"]?.ToString() ?? string.Empty;
            dteRcvDate.YyyyMmDd = row["rcv_date"]?.ToString();
            txtWoNo.Text = row["lot_wo_no"]?.ToString() ?? string.Empty;
            txtLotNo.Text = row["lot_no"]?.ToString() ?? string.Empty;
            txtItemId.Text = row["item_id"]?.ToString() ?? string.Empty;
            txtItemNm.Text = row["item_nm"]?.ToString() ?? string.Empty;
            txtWhId.Text = row["wh_id"]?.ToString() ?? string.Empty;
            txtWhNm.Text = row["wh_nm"]?.ToString() ?? string.Empty;
            txtSupId.Text = row["sup_cust_id"]?.ToString() ?? string.Empty;
            txtSupNm.Text = row["sup_cust_nm"]?.ToString() ?? string.Empty;
            spnQty.EditValue = Dec(row["qty"]);
            memoRemark.Text = row["remark"]?.ToString() ?? string.Empty;
        });
        ApplyLock();
    }

    private void EnterNewMode()
    {
        _loadSeq++; // 진행 중이던 목록 행 조회 응답은 무효
        SuppressDirtyTracking(() =>
        {
            _editingKey = null;
            _statCd = "0";
            cboAccId.EditValue = Session.AccId?.ToString();
            cboStatCd.EditValue = "0";
            txtRcvNo.Text = string.Empty;
            dteRcvDate.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");
            foreach (var t in new DevExpress.XtraEditors.BaseEdit[] { txtWoNo, txtLotNo, txtItemId, txtItemNm, txtWhId, txtWhNm, txtSupId, txtSupNm })
                t.Text = string.Empty;
            spnQty.EditValue = 0m;
            memoRemark.Text = string.Empty;
        });
        ApplyLock();
    }

    /// <summary>확정된 입고는 전체가 조회전용. 작업지시는 저장 전에만 바꿀 수 있다(불러오기).</summary>
    private void ApplyLock()
    {
        var confirmed = _statCd == "C";
        var created = _editingKey != null;

        dteRcvDate.Properties.ReadOnly = confirmed;
        spnQty.Properties.ReadOnly = confirmed;
        txtSupNm.Properties.ReadOnly = confirmed;
        txtItemNm.Properties.ReadOnly = confirmed;
        txtWhNm.Properties.ReadOnly = confirmed;
        txtLotNo.Properties.ReadOnly = confirmed;
        memoRemark.Properties.ReadOnly = confirmed;
        btnConfirm.Enabled = created && !confirmed;
        btnConfirmCancel.Enabled = created && confirmed;
    }

    public override Task NewClick()
    {
        EnterNewMode();
        FocusFirstEntryField(panData); // 사업장 다음 첫 탭오더 컨트롤에 커서(표준)
        return Task.CompletedTask;
    }

    // ===== 저장 / 삭제 / 확정 =====

    public override async Task SaveClick()
    {
        if (_statCd == "C")
        {
            AppMessageBox.Show("확정된 입고는 수정할 수 없습니다. 먼저 확정취소하세요.", "안내");
            return;
        }
        var result = await SaveAsync("USP_PR_RCV_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_rcv_id"] = _editingKey,
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_rcv_date"] = dteRcvDate.YyyyMmDd,
            ["p_item_id"] = txtItemId.Text,
            ["p_lot_no"] = txtLotNo.Text.Trim(),
            ["p_wh_id"] = txtWhId.Text,
            ["p_qty"] = spnQty.EditValue?.ToString(),
            ["p_sup_cust_id"] = txtSupId.Text,
            ["p_remark"] = memoRemark.Text,
        });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        _editingKey ??= result.GeneratedCode;
        Toast.Show("저장되었습니다.");
        await QueryCore(preserveSelection: true); // 방금 저장한 입고를 목록에서 그대로 선택해 둔다
    }

    protected override bool ConfirmDeleteByDefault => false; // 삭제 확인창을 DeleteClick에서 직접 띄움(문서번호 등 상세 문구)

    public override async Task DeleteClick()
    {
        if (_editingKey == null)
        {
            AppMessageBox.Show("삭제할 웨이퍼 입고를 목록에서 먼저 선택해주세요.", "안내");
            return;
        }

        var confirm = AppMessageBox.Show(
            $"선택하신 웨이퍼 입고를 삭제 하시겠습니까?\n\n[{txtRcvNo.Text}] {txtItemNm.Text} / {txtLotNo.Text}",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await SaveAsync("USP_PR_RCV_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = "D",
            ["p_rcv_id"] = _editingKey,
        });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "삭제에 실패했습니다.", "삭제 실패");
            return;
        }

        _editingKey = null;
        await QueryClick();
        if (_list.Rows.Count == 0) EnterNewMode();
        Toast.Show("삭제되었습니다.");
    }

    /// <summary>확정/확정취소 - 미저장 변경이 있으면 서버 상태와 어긋나므로 먼저 저장하게 한다.</summary>
    private async Task ConfirmAsync(bool confirm)
    {
        if (_editingKey == null)
        {
            AppMessageBox.Show("먼저 저장한 뒤 " + (confirm ? "확정" : "확정취소") + "하세요.", "안내");
            return;
        }
        if (IsDirty)
        {
            AppMessageBox.Show("저장하지 않은 변경이 있습니다. 먼저 저장하세요.", "안내");
            return;
        }

        var msg = confirm
            ? $"웨이퍼 입고를 확정하시겠습니까?\n{txtWhNm.Text}에 LOT {txtLotNo.Text} {spnQty.EditValue:#,##0.####}장이 입고됩니다."
            : "웨이퍼 입고 확정을 취소하시겠습니까?\n재고가 되돌려집니다(그 LOT를 작업지시에 배정했거나 공정실적에서 썼으면 취소되지 않습니다).";
        if (AppMessageBox.Show(msg, confirm ? "웨이퍼 입고 확정" : "웨이퍼 입고 확정취소", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        var result = await SaveAsync("USP_PR_RCV_C_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = confirm ? "C" : "CC",
            ["p_rcv_id"] = _editingKey,
        });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "처리에 실패했습니다.", confirm ? "확정 실패" : "확정취소 실패");
            return;
        }

        Toast.Show(confirm ? "확정되었습니다." : "확정이 취소되었습니다.");
        await QueryCore(preserveSelection: true);
    }

    /// <summary>다른 화면에서 입고번호로 열 때 호출된다(key=rcv_id 문자열). 목록을 다시 불러오고 그 행을 선택한다.</summary>
    public override async Task FocusRecordAsync(string key)
    {
        _editingKey = key;
        await QueryCore(preserveSelection: true);
    }
}
