using DevExpress.XtraEditors;
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

/// <summary>
/// 기초재고등록 - 품목/창고/LOT별 기초수량을 입력하고(TMAOPENM/TMAOPEND) 확정하면 수불(OPEN_IN, 입고계열)이 생기고
/// 재고관리 품목(라인 재고반영=Y)만 현재고(TMASTOCK)에 반영된다(USP_MA_OPEN_C_S). 확정취소는 역거래 수불을 남기고 현재고를
/// 되돌리며, 이미 출고/이동돼 재고가 모자라면 서버가 막는다. 결재는 타지 않는다. frmGr와 같은 Master-One Sheet 구조.
/// </summary>
public partial class frmOpenStock : BaseForm, WYNLAB.Popup.IFeatureHost
{
    private DataTable _detail = new();
    private DataTable _itemCache = new();
    private DataTable _whCache = new();
    private DataTable _locCache = new();
    private string? _editingKey; // null이면 신규모드
    private string _statCd = "0";
    private string? _appNo;        // 연결된 전자결재 번호/상태(메뉴에서 결재를 켠 경우만 의미가 있다)
    private string? _apprStatCd;
    private bool _syncingRow;

    public frmOpenStock()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준: 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "기초재고등록";

        txtDeptNm.MapField("dept_id", txtDeptId);
        txtEmpNm.MapField("emp_id", txtEmpId);
        txtEmpNm.LinkDept(txtDeptNm, txtDeptId); // 담당자 팝업은 선택한 부서 소속만, 담당자를 고르면 부서도 채움

        gvw1.Role = GridRoleWyn.Edit;
        gvw1.HighlightFocusedRow = true;
        gvw1.AddColumnSummary(colQty, DevExpress.Data.SummaryItemType.Sum, "{0:#,##0.####}");
        gvw1.RowAdd += (s, e) => AddRow();
        gvw1.RowDelete += (s, e) => DeleteFocusedRow();
        gvw1.CellValueChanged += Gvw1_CellValueChanged;
        // 창고/위치는 이름 칸(wh_nm/loc_nm)에 팝업을 걸었다 - 팝업이 셀에 키값을 쓴 직후 ID와 이름을 같이 채운다.
        popcolWh.ResultSelected += (handle, result) => ApplyPickedMaster(handle, result, colWhId, colWhNm, "wh_nm");
        popcolLoc.ResultSelected += (handle, result) => ApplyPickedMaster(handle, result, colLocId, colLocNm, "loc_nm");
        btnAddRow1.Click += (s, e) => AddRow();
        btnDeletRow1.Click += (s, e) => DeleteFocusedRow();
        btnPickItem.Click += async (s, e) => await SafeExecuteAsync(PickItemsAsync, "품목선택");
        btnConfirm.Click += async (s, e) => await SafeExecuteAsync(() => ConfirmAsync(true), "기초재고 확정");
        btnConfirmCancel.Click += async (s, e) => await SafeExecuteAsync(() => ConfirmAsync(false), "기초재고 확정취소");

        TrackDirty(panData);
        featBar.FeaturesLoaded += (s, e) => ApplyLock();   // 메뉴 기능을 읽은 직후(결재 사용 여부 확정) 잠금/버튼 상태를 다시 맞춘다

        EnterNewMode();
        Load += async (s, e) =>
        {
            await SafeExecuteAsync(EnsureDetailSchemaAsync, "기초재고 화면 초기화");
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

    private void AddRow()
    {
        if (_statCd == "C") return;
        gvw1.AddNewRow();
    }

    /// <summary>품목선택 - 공통 팝업(P_ITEM)을 다중 선택으로 열어 체크한 품목마다 행을 추가한다. 이미 담은 품목을 또 골라도 막지 않고 고른 만큼 행이 생긴다.</summary>
    private async Task PickItemsAsync()
    {
        if (_statCd == "C" || featBar.ApprovalLocked) return;
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
        if (_statCd == "C") return;
        try { if (gvw1.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
        catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
    }

    /// <summary>신규모드로 열린 화면은 그리드 DataTable에 컬럼이 없어 팝업 선택값을 받을 곳이 없다 - 빈 조회로 컬럼 구조만 먼저 받아 둔다.</summary>
    private async Task EnsureDetailSchemaAsync()
    {
        if (_detail.Columns.Count > 0) return;
        var tables = await QueryMultiAsync("USP_MA_OPEN_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q", ["p_open_id"] = "-1" });
        if (tables.Count > 1 && _detail.Columns.Count == 0) _detail = tables[1];
        if (_editingKey == null) EnterNewMode();
    }

    /// <summary>품번 셀이 바뀌면(팝업 선택/직접 입력) 캐시에서 품목을 찾아 같은 행의 품목ID/품명/규격/단위/기본창고를 채운다(frmPoReq와 같은 방식).
    /// 창고/위치 팝업 선택은 이름 칸을 채운다. 수량은 숫자로 바꿔 쓴다.</summary>
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
                // 품목의 기본 창고 - 팝업 컬럼이라 SetRowCellValue로 쓰면 팝업이 저절로 뜨므로 DataRow에 직접 쓴다(frmPoReq와 같은 이유).
                if (gvw1.GetDataRow(e.RowHandle) is { } lineRow && Col(row, "wh_id") != null && string.IsNullOrEmpty(Convert.ToString(lineRow["wh_id"])))
                {
                    lineRow["wh_id"] = Col(row, "wh_id");
                    lineRow["wh_nm"] = Col(row, "wh_nm") ?? DBNull.Value;
                }
            }
            finally { _syncingRow = false; }
        }
        else if (e.Column == colWhNm || e.Column == colLocNm)
        {
            // 이름을 지우면 ID도 같이 비운다(팝업 선택 결과는 ApplyPickedMaster가 ID/이름을 함께 채운다).
            if (string.IsNullOrWhiteSpace(Convert.ToString(e.Value)))
            {
                _syncingRow = true;
                try { gvw1.SetRowCellValue(e.RowHandle, e.Column == colWhNm ? colWhId : colLocId, null); }
                finally { _syncingRow = false; }
            }
        }
    }

    /// <summary>팝업(P_WH/P_LOC)에서 고른 행으로 같은 행의 ID 칸과 이름 칸을 함께 채운다. 팝업이 이름 칸에 쓴 키값은 여기서 이름으로 바로잡는다.</summary>
    private void ApplyPickedMaster(int rowHandle, PopupLookupResult result,
        DevExpress.XtraGrid.Columns.GridColumn idColumn, DevExpress.XtraGrid.Columns.GridColumn nameColumn, string nameField)
    {
        var name = result.Row != null && result.Row.TryGetValue(nameField, out var v) && !string.IsNullOrEmpty(v) ? v : result.Display;
        _syncingRow = true;
        try
        {
            gvw1.SetRowCellValue(rowHandle, idColumn, result.Code);
            gvw1.SetRowCellValue(rowHandle, nameColumn, name);
        }
        finally { _syncingRow = false; }
    }
    /// <summary>이름 칸에 글자가 있으면 ID가 있고 그 ID의 이름과 같아야 한다 - 이름을 직접 타이핑하고 팝업을 취소해 ID와 이름이 어긋난 행을 저장 전에 거른다.</summary>
    private static bool MasterMatches(DataRow row, string idColumn, string nameColumn, DataTable cache)
    {
        var name = Convert.ToString(row[nameColumn]);
        if (string.IsNullOrWhiteSpace(name)) return true;
        var id = Convert.ToString(row[idColumn]);
        if (string.IsNullOrEmpty(id)) return false;
        var known = LookupName(cache, idColumn, nameColumn, id);
        return known == null || known == name;   // 캐시를 못 받은 경우(known=null)는 서버 검증에 맡긴다
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
        // 조회 공통 규칙: 번호를 안 넣은 조회는 서버가 "가장 최근 문서 1건"으로 처리하므로, 대상이 없는 것으로 보고 신규 입력 모드로 간다.
        if (forceKey == null && string.IsNullOrWhiteSpace(txtSearchNo.Text))
        {
            EnterNewMode();
            Toast.Show("조회할 번호가 없어 신규 입력 상태로 전환했습니다.");
            return;
        }
        var tables = await QueryMultiAsync("USP_MA_OPEN_Q", new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "Q",
            ["p_open_id"] = forceKey,
            ["p_open_no"] = forceKey == null ? txtSearchNo.Text : null,
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
            _editingKey = row["open_id"]?.ToString();
            _statCd = row["stat_cd"]?.ToString() ?? "0";
            _appNo = row["app_no"]?.ToString();
            _apprStatCd = row["appr_stat_cd"]?.ToString();
            cboAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            txtDocNo.Text = row["open_no"]?.ToString() ?? string.Empty;
            txtSearchNo.Text = txtDocNo.Text;
            dteDocDate.YyyyMmDd = row["open_date"]?.ToString();
            cboStatCd.EditValue = _statCd;
            txtDeptId.Text = row["dept_id"]?.ToString() ?? string.Empty;
            txtDeptNm.Text = row["dept_nm"]?.ToString() ?? string.Empty;
            txtEmpId.Text = row["emp_id"]?.ToString() ?? string.Empty;
            txtEmpNm.Text = row["emp_nm"]?.ToString() ?? string.Empty;
            txtCfmDt.Text = row["cfm_dt"] is DateTime dt ? dt.ToString("yyyy-MM-dd HH:mm") : string.Empty;
            txtCfmUserId.Text = row["cfm_user_id"]?.ToString() ?? string.Empty;
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
            _appNo = null;
            _apprStatCd = null;
            cboAccId.EditValue = Session.AccId?.ToString();
            txtDocNo.Text = string.Empty;
            dteDocDate.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");
            cboStatCd.EditValue = "0";
            txtDeptId.Text = Session.DeptId?.ToString() ?? string.Empty;
            txtDeptNm.Text = Session.DeptNm;
            txtEmpId.Text = Session.EmpId?.ToString() ?? string.Empty;
            txtEmpNm.Text = Session.EmpNm; // 담당자명 = 세션 사원명(EmpId의 이름) - 사용자 이름(UserNm)이 아니다
            txtCfmDt.Text = string.Empty;
            txtCfmUserId.Text = string.Empty;
            memoRemark.Text = string.Empty;

            _detail = _detail.Clone();
            TrackDirty(_detail);
            grd1.DataSource = _detail;
        });
        ApplyLock();
    }

    /// <summary>확정된 문서는 화면 전체가 조회전용, 확정취소 버튼은 확정 문서에서만 켠다.</summary>
    private void ApplyLock()
    {
        var locked = _statCd == "C" || featBar.ApprovalLocked;   // 결재 상신된 문서는 확정 전이라도 수정 불가(반려되면 다시 수정 가능)
        foreach (var edit in new BaseEdit[] { dteDocDate, txtDeptNm, txtEmpNm, memoRemark })
            edit.Properties.ReadOnly = locked;
        gvw1.OptionsBehavior.Editable = !locked;
        btnAddRow1.Enabled = !locked;
        btnDeletRow1.Enabled = !locked;
        btnConfirm.Enabled = !locked && _editingKey != null;
        btnConfirmCancel.Enabled = _statCd == "C";
        // 결재를 쓰는 화면은 확정/확정취소를 결재 프로시저(승인완료/되돌림)가 처리하므로 별도 확정 버튼을 숨긴다.
        btnConfirm.Visible = btnConfirmCancel.Visible = !featBar.ApprovalEnabled;
        featBar.UpdateState();
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        if (featBar.ApprovalLocked)
        {
            AppMessageBox.Show("결재 상신된 문서는 수정할 수 없습니다.", "안내");
            return;
        }
        if (_statCd == "C")
        {
            AppMessageBox.Show("확정된 기초재고는 수정할 수 없습니다. 먼저 확정취소하세요.", "안내");
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
            if (!MasterMatches(row, "wh_id", "wh_nm", _whCache) || !MasterMatches(row, "loc_id", "loc_nm", _locCache))
            {
                AppMessageBox.Show("창고/위치는 팝업에서 선택해주세요. (등록되지 않은 값이 입력된 행이 있습니다.)", "안내");
                return;
            }            if (Dec(row["qty"]) <= 0)
            {
                AppMessageBox.Show($"'{row["item_nm"]}' 기초수량을 0보다 크게 입력하세요.", "안내");
                return;
            }
        }

        var headerResult = await SaveAsync("USP_MA_OPEN_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_open_id"] = _editingKey,
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_open_date"] = dteDocDate.YyyyMmDd,
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

            var detailResult = await SaveAsync("USP_MA_OPEN_S_1", new Dictionary<string, string?>
            {
                ["p_work_type"] = workType,
                ["p_open_id"] = headerKey,
                ["p_serl"] = ProcData.Str(row, "serl", version),
                ["p_item_id"] = ProcData.Str(row, "item_id", version),
                ["p_qty"] = ProcData.Str(row, "qty", version),
                ["p_lot_no"] = ProcData.Str(row, "lot_no", version),
                ["p_wh_id"] = ProcData.Str(row, "wh_id", version),
                ["p_loc_id"] = ProcData.Str(row, "loc_id", version),
                ["p_remark"] = ProcData.Str(row, "remark", version),
            });
            if (detailResult == null || !detailResult.Success)
            {
                AppMessageBox.Show(detailResult?.Message ?? "품목 저장에 실패했습니다.", "저장 실패");
                // 헤더는 이미 저장됐으므로 그 상태로 다시 조회해서 화면을 실제 저장 상태에 맞춘다.
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
        if (featBar.ApprovalLocked)
        {
            AppMessageBox.Show("결재 상신된 문서는 삭제할 수 없습니다.", "안내");
            return;
        }
        if (_editingKey == null) return;

        var result = await SaveAsync("USP_MA_OPEN_S", new Dictionary<string, string?> { ["p_work_type"] = "D", ["p_open_id"] = _editingKey });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "삭제에 실패했습니다.", "삭제 실패");
            return;
        }

        _editingKey = null;
        Toast.Show("삭제되었습니다.");
        EnterNewMode();
    }

    /// <summary>확정/확정취소 - 미저장 변경이 있으면 서버 상태와 어긋나므로 먼저 저장하게 한다.</summary>
    private async Task ConfirmAsync(bool confirm)
    {
        var label = confirm ? "확정" : "확정취소";
        if (_editingKey == null)
        {
            AppMessageBox.Show($"먼저 저장한 뒤 {label}하세요.", "안내");
            return;
        }
        gvw1.CloseEditor();
        if (IsDirty)
        {
            AppMessageBox.Show("저장하지 않은 변경이 있습니다. 먼저 저장하세요.", "안내");
            return;
        }

        var msg = confirm
            ? "기초재고를 확정하시겠습니까?\n확정하면 수불이 생성되고, 재고관리 품목은 현재고에 반영됩니다."
            : "기초재고 확정을 취소하시겠습니까?\n역거래 수불이 남고 현재고가 차감됩니다(이미 출고/이동돼 재고가 모자라면 취소되지 않습니다).";
        if (AppMessageBox.Show(msg, $"기초재고 {label}", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        var result = await SaveAsync("USP_MA_OPEN_C_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = confirm ? "C" : "CC",
            ["p_open_id"] = _editingKey,
        });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "처리에 실패했습니다.", $"{label} 실패");
            return;
        }

        Toast.Show(confirm ? "확정되었습니다." : "확정이 취소되었습니다.");
        await QueryCore(forceKey: _editingKey);
    }

    // ---- 공통 기능 패널(FeatureBarWyn: 전자결재/첨부파일) 연동
    public WYNLAB.Popup.FeatureContext GetFeatureContext() => new()
    {
        DocId = long.TryParse(_editingKey, out var id) ? id : null,
        DocNo = txtDocNo.Text,
        Title = $"기초재고 {txtDocNo.Text}",
        Text = memoRemark.Text,
        HasUnsavedChanges = IsDirty,
        AppNo = _appNo,
        ApprStatCd = _apprStatCd,
    };

    public void OnFeatureChanged(string featureCd)
    {
        // 결재 상신/승인/반려로 문서 상태(잠금/확정)가 바뀌었을 수 있으니 서버 기준으로 다시 읽는다.
        if (featureCd == "APPROVAL" && _editingKey != null) _ = SafeExecuteAsync(() => QueryCore(forceKey: _editingKey), "기초재고 재조회");
    }

    public override async Task FocusRecordAsync(string key) => await QueryCore(forceKey: key);
}
