using DevExpress.XtraEditors;
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Popup;

namespace WYNLAB.MA;

/// <summary>
/// 기타출고요청등록 - 출고할 품목/수량을 요청서로 작성하고(TMAETCREQM/TMAETCREQD) 전자결재(popApp 공용 팝업, doc_type="ETCREQ")를 올린다.
/// 최종 승인되면 서버(USP_AP_APPR_S_ETCREQ)가 요청을 확정(stat_cd C)으로 바꾸고, 확정된 요청은 기타출고등록(frmEtcOut)의
/// "요청 불러오기"에서 출고할 수 있다. 결재상태는 이 화면이 직접 갱신하지 않고 USP_MA_ETCREQ_Q가 매 조회마다 TAPDOC을 JOIN해서
/// 보여준다(Pull). 확정되었거나 결재가 진행 중인 요청은 수정할 수 없다. frmPoReq와 같은 Master-One Sheet 구조.
/// </summary>
public partial class frmEtcOutReq : BaseForm
{
    private DataTable _detail = new();
    private DataTable _itemCache = new();
    private DataTable _whCache = new();
    private DataTable _locCache = new();
    private string? _editingKey; // null이면 신규모드
    private string _statCd = "0";
    private string _apprStatCd = string.Empty;
    private bool _syncingRow;

    public frmEtcOutReq()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준: 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "기타출고요청등록";

        txtDeptNm.MapField("dept_id", txtDeptId);
        txtEmpNm.MapField("emp_id", txtEmpId);
        txtEmpNm.LinkDept(txtDeptNm, txtDeptId); // 담당자 팝업은 선택한 부서 소속만, 담당자를 고르면 부서도 채움

        gvw1.Role = GridRoleWyn.Edit;
        gvw1.HighlightFocusedRow = true;
        gvw1.AddColumnSummary(colQty, DevExpress.Data.SummaryItemType.Sum, "{0:#,##0.####}");
        gvw1.RowAdd += (s, e) => AddRow();
        gvw1.RowDelete += (s, e) => DeleteFocusedRow();
        gvw1.CellValueChanged += Gvw1_CellValueChanged;
        btnAddRow1.Click += (s, e) => AddRow();
        btnDeletRow1.Click += (s, e) => DeleteFocusedRow();
        btnPickItem.Click += async (s, e) => await SafeExecuteAsync(PickItemsAsync, "품목선택");
        btnOpenApproval.Click += async (s, e) => await SafeExecuteAsync(OpenApprovalAsync, "전자결재");

        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) =>
        {
            await SafeExecuteAsync(EnsureDetailSchemaAsync, "기타출고요청 화면 초기화");
            _itemCache = await LoadPopupRowsAsync("P_ITEM");
            _whCache = await LoadPopupRowsAsync("P_WH");
            _locCache = await LoadPopupRowsAsync("P_LOC");
        };
    }

    private static decimal Dec(object? value) => value == null || value == DBNull.Value ? 0 : decimal.TryParse(value.ToString(), out var d) ? d : 0;

    private static object? Col(DataRow? row, string column)
    {
        if (row == null || !row.Table.Columns.Contains(column)) return null;
        var value = row[column];
        return value == DBNull.Value ? null : value;
    }

    /// <summary>확정(승인완료)되었거나 결재가 진행 중(상신/진행중)이면 수정할 수 없다. 반려는 고쳐서 다시 상신할 수 있다.</summary>
    private bool IsLocked => _statCd == "C" || _apprStatCd is "0" or "1";

    private void AddRow()
    {
        if (IsLocked) return;
        gvw1.AddNewRow();
    }

    /// <summary>품목선택 - 공통 팝업(P_ITEM)을 다중 선택으로 열어 체크한 품목마다 행을 추가한다. 이미 담은 품목을 또 골라도 막지 않고 고른 만큼 행이 생긴다.</summary>
    private async Task PickItemsAsync()
    {
        if (IsLocked) return;
        if (_detail.Columns.Count == 0) { Toast.Show("화면을 준비하는 중입니다. 잠시 후 다시 시도해주세요."); return; }
        gvw1.CloseEditor();

        var picked = await WYNLAB.Popup.popPopUp.ShowMultiAsync("P_ITEM", this);
        if (picked == null || picked.Count == 0) return;

        var missing = 0;
        foreach (var p in picked)
        {
            var src = FindCacheRow(_itemCache, "item_id", "item_no", p.Code);
            if (src == null) { missing++; continue; }

            var row = _detail.NewRow();
            foreach (var col in new[] { "item_id", "item_no", "item_nm", "item_spec", "unit_cd", "wh_id", "wh_nm" })
                if (_detail.Columns.Contains(col) && src.Table.Columns.Contains(col)) row[col] = src[col];
            _detail.Rows.Add(row);
        }
        if (missing > 0) Toast.Show($"품목 {missing}건은 품목 목록을 아직 못 받아 추가하지 못했습니다. 잠시 후 다시 시도해주세요.");
    }
    private void DeleteFocusedRow()
    {
        if (IsLocked) return;
        try { if (gvw1.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
        catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
    }

    private async Task EnsureDetailSchemaAsync()
    {
        if (_detail.Columns.Count > 0) return;
        var tables = await QueryMultiAsync("USP_MA_ETCREQ_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q", ["p_req_id"] = "-1" });
        if (tables.Count > 1 && _detail.Columns.Count == 0) _detail = tables[1];
        if (_editingKey == null) EnterNewMode();
    }

    private void Gvw1_CellValueChanged(object? sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
    {
        if (_syncingRow) return;

        if (e.Value is string text && e.Column == colQty)
        {
            gvw1.SetRowCellValue(e.RowHandle, e.Column, decimal.TryParse(text.Replace(",", ""), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var n) ? n : (object?)null);
            return;
        }

        if (e.Column == colItemNo)
        {
            var row = FindCacheRow(_itemCache, "item_id", "item_no", e.Value?.ToString());
            if (row == null && !string.IsNullOrWhiteSpace(e.Value?.ToString())) Toast.Show($"등록되지 않은 품번입니다: {e.Value}");

            _syncingRow = true;
            try
            {
                gvw1.SetRowCellValue(e.RowHandle, colItemId, Col(row, "item_id"));
                gvw1.SetRowCellValue(e.RowHandle, colItemNo, Col(row, "item_no"));
                gvw1.SetRowCellValue(e.RowHandle, colItemNm, Col(row, "item_nm"));
                gvw1.SetRowCellValue(e.RowHandle, colItemSpec, Col(row, "item_spec"));
                gvw1.SetRowCellValue(e.RowHandle, colUnitCd, Col(row, "unit_cd"));
                if (gvw1.GetDataRow(e.RowHandle) is { } lineRow && Col(row, "wh_id") != null && string.IsNullOrEmpty(Convert.ToString(lineRow["wh_id"])))
                {
                    lineRow["wh_id"] = Col(row, "wh_id");
                    lineRow["wh_nm"] = Col(row, "wh_nm") ?? DBNull.Value;
                }
            }
            finally { _syncingRow = false; }
        }
        else if (e.Column == colWhId)
        {
            _syncingRow = true;
            try { gvw1.SetRowCellValue(e.RowHandle, colWhNm, LookupName(_whCache, "wh_id", "wh_nm", e.Value)); }
            finally { _syncingRow = false; }
        }
        else if (e.Column == colLocId)
        {
            _syncingRow = true;
            try { gvw1.SetRowCellValue(e.RowHandle, colLocNm, LookupName(_locCache, "loc_id", "loc_nm", e.Value)); }
            finally { _syncingRow = false; }
        }
        else if (e.Column == colQty)
        {
            _syncingRow = true;
            try { gvw1.SetRowCellValue(e.RowHandle, colRemainQty, Dec(gvw1.GetRowCellValue(e.RowHandle, colQty)) - Dec(gvw1.GetRowCellValue(e.RowHandle, colNextQty))); }
            finally { _syncingRow = false; }
        }
    }

    private static DataRow? FindCacheRow(DataTable cache, string idColumn, string noColumn, string? typed)
    {
        if (string.IsNullOrWhiteSpace(typed)) return null;
        if (long.TryParse(typed, out var id) && cache.Columns.Contains(idColumn))
            foreach (DataRow r in cache.Rows) if (Convert.ToInt64(Col(r, idColumn) ?? -1L) == id) return r;
        foreach (DataRow r in cache.Rows)
            if (string.Equals(Convert.ToString(Col(r, noColumn)), typed, StringComparison.OrdinalIgnoreCase)) return r;
        return null;
    }

    private static string? LookupName(DataTable cache, string idColumn, string nameColumn, object? id)
    {
        var key = Convert.ToString(id);
        if (string.IsNullOrEmpty(key)) return null;
        foreach (DataRow r in cache.Rows) if (Convert.ToString(Col(r, idColumn)) == key) return Convert.ToString(Col(r, nameColumn));
        return null;
    }

    public override async Task QueryClick() => await QueryCore(forceKey: null);

    private async Task QueryCore(string? forceKey)
    {
        if (forceKey == null && string.IsNullOrWhiteSpace(txtSearchNo.Text))
        {
            EnterNewMode();
            Toast.Show("조회할 번호가 없어 신규 입력 상태로 전환했습니다.");
            return;
        }
        var tables = await QueryMultiAsync("USP_MA_ETCREQ_Q", new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "Q",
            ["p_req_id"] = forceKey,
            ["p_req_no"] = forceKey == null ? txtSearchNo.Text : null,
        });
        var header = tables.Count > 0 ? tables[0] : new DataTable();
        _detail = tables.Count > 1 ? tables[1] : new DataTable();

        if (header.Rows.Count > 0) OnRowLoaded(header.Rows[0]);
        else
        {
            EnterNewMode();
            Toast.Show("조회 결과가 없어 신규 입력 상태로 전환했습니다.");
        }
    }

    private void OnRowLoaded(DataRow row)
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = row["req_id"]?.ToString();
            _statCd = row["stat_cd"]?.ToString() ?? "0";
            _apprStatCd = row["appr_stat_cd"]?.ToString() ?? string.Empty;
            cboAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            txtDocNo.Text = row["req_no"]?.ToString() ?? string.Empty;
            txtSearchNo.Text = txtDocNo.Text;
            dteDocDate.YyyyMmDd = row["req_date"]?.ToString();
            txtReqTitle.Text = row["req_title"]?.ToString() ?? string.Empty;
            cboStatCd.EditValue = _statCd;
            cboTransType.EditValue = row["trans_type"]?.ToString() ?? string.Empty;
            txtDeptId.Text = row["dept_id"]?.ToString() ?? string.Empty;
            txtDeptNm.Text = row["dept_nm"]?.ToString() ?? string.Empty;
            txtEmpId.Text = row["emp_id"]?.ToString() ?? string.Empty;
            txtEmpNm.Text = row["emp_nm"]?.ToString() ?? string.Empty;
            txtAppNo.Text = row["app_no"]?.ToString() ?? string.Empty;
            cboApprStatCd.EditValue = _apprStatCd;
            txtCfmDt.Text = row["cfm_dt"] is DateTime dt ? dt.ToString("yyyy-MM-dd HH:mm") : string.Empty;
            memoRemark.Text = row["remark"]?.ToString() ?? string.Empty;

            TrackDirty(_detail);
            grd1.DataSource = _detail;
        });
        ApplyLock();
    }

    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = null;
            _statCd = "0";
            _apprStatCd = string.Empty;
            cboAccId.EditValue = Session.AccId?.ToString();
            txtDocNo.Text = string.Empty;
            dteDocDate.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");
            txtReqTitle.Text = string.Empty;
            cboStatCd.EditValue = "0";
            cboTransType.EditValue = "ETC_OUT"; // 출고유형 기본값 = 기타출고
            txtDeptId.Text = Session.DeptId?.ToString() ?? string.Empty;
            txtDeptNm.Text = Session.DeptNm;
            txtEmpId.Text = Session.EmpId?.ToString() ?? string.Empty;
            txtEmpNm.Text = Session.EmpNm; // 요청자명 = 세션 사원명(EmpId의 이름) - 사용자 이름(UserNm)이 아니다
            txtAppNo.Text = string.Empty;
            cboApprStatCd.EditValue = string.Empty;
            txtCfmDt.Text = string.Empty;
            memoRemark.Text = string.Empty;

            _detail = _detail.Clone();
            TrackDirty(_detail);
            grd1.DataSource = _detail;
        });
        ApplyLock();
    }

    private void ApplyLock()
    {
        var locked = IsLocked;
        foreach (var edit in new BaseEdit[] { dteDocDate, txtReqTitle, cboTransType, txtDeptNm, txtEmpNm, memoRemark })
            edit.Properties.ReadOnly = locked;
        gvw1.OptionsBehavior.Editable = !locked;
        btnAddRow1.Enabled = !locked;
        btnDeletRow1.Enabled = !locked;
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        if (IsLocked)
        {
            AppMessageBox.Show(_statCd == "C" ? "승인완료된 기타출고요청은 수정할 수 없습니다." : "결재가 진행 중인 기타출고요청은 수정할 수 없습니다.", "안내");
            return;
        }

        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        foreach (DataRow row in _detail.Rows)
        {
            if (row.RowState == DataRowState.Deleted || row.RowState == DataRowState.Unchanged) continue;
            if (string.IsNullOrWhiteSpace(Convert.ToString(row["item_id"])))
            {
                AppMessageBox.Show("품번을 입력하지 않은 행이 있습니다.", "안내");
                return;
            }
            if (Dec(row["qty"]) <= 0)
            {
                AppMessageBox.Show($"'{row["item_nm"]}' 요청수량을 0보다 크게 입력하세요.", "안내");
                return;
            }
        }

        var headerResult = await SaveAsync("USP_MA_ETCREQ_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_req_id"] = _editingKey,
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_req_date"] = dteDocDate.YyyyMmDd,
            ["p_req_title"] = txtReqTitle.Text,
            ["p_trans_type"] = cboTransType.EditValue?.ToString(),
            ["p_dept_id"] = txtDeptId.Text,
            ["p_emp_id"] = txtEmpId.Text,
            ["p_remark"] = memoRemark.Text,
        });
        if (headerResult == null || !headerResult.Success)
        {
            AppMessageBox.Show(headerResult?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        var headerKey = _editingKey ?? headerResult.GeneratedCode;

        foreach (DataRow row in _detail.Rows.Cast<DataRow>().ToList())
        {
            if (row.RowState == DataRowState.Unchanged) continue;

            var workType = row.RowState == DataRowState.Deleted ? "D" : row.RowState == DataRowState.Added ? "N" : "U";
            var version = row.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;

            var detailResult = await SaveAsync("USP_MA_ETCREQ_S_1", new Dictionary<string, string?>
            {
                ["p_work_type"] = workType,
                ["p_req_id"] = headerKey,
                ["p_serl"] = ProcData.Str(row, "serl", version),
                ["p_item_id"] = ProcData.Str(row, "item_id", version),
                ["p_qty"] = ProcData.Str(row, "qty", version),
                ["p_unit_cd"] = ProcData.Str(row, "unit_cd", version),
                ["p_wh_id"] = ProcData.Str(row, "wh_id", version),
                ["p_loc_id"] = ProcData.Str(row, "loc_id", version),
                ["p_lot_no"] = ProcData.Str(row, "lot_no", version),
                ["p_remark"] = ProcData.Str(row, "remark", version),
            });
            if (detailResult == null || !detailResult.Success)
            {
                AppMessageBox.Show(detailResult?.Message ?? "품목 저장에 실패했습니다.", "저장 실패");
                _editingKey ??= headerResult.GeneratedCode;
                await QueryCore(forceKey: _editingKey);
                return;
            }
        }

        _editingKey ??= headerResult.GeneratedCode;
        Toast.Show("저장되었습니다.");
        await QueryCore(forceKey: _editingKey);
    }

    public override async Task DeleteClick()
    {
        if (_editingKey == null) return;

        var result = await SaveAsync("USP_MA_ETCREQ_S", new Dictionary<string, string?> { ["p_work_type"] = "D", ["p_req_id"] = _editingKey });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "삭제에 실패했습니다.", "삭제 실패");
            return;
        }

        _editingKey = null;
        Toast.Show("삭제되었습니다.");
        EnterNewMode();
    }

    public override async Task FocusRecordAsync(string key) => await QueryCore(forceKey: key);

    /// <summary>전자결재 공용 팝업(popApp)을 연다 - frmPoReq.OpenApprovalAsync와 같은 방식, doc_type만 "ETCREQ".</summary>
    private async Task OpenApprovalAsync()
    {
        if (_editingKey == null)
        {
            AppMessageBox.Show("먼저 저장한 뒤 전자결재를 열어주세요.", "안내");
            return;
        }
        if (IsDirty)
        {
            AppMessageBox.Show("저장하지 않은 변경이 있습니다. 먼저 저장한 뒤 전자결재를 열어주세요.", "안내");
            return;
        }

        var changed = await popApp.ShowAsync("ETCREQ", long.Parse(_editingKey), txtDocNo.Text,
            $"기타출고요청서 - {txtReqTitle.Text}", memoRemark.Text, this);
        if (changed) await QueryCore(forceKey: _editingKey);
    }
}
