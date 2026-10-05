using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

/// <summary>
/// 기타출고등록 - 승인완료된 기타출고요청을 "요청 불러오기"(popPick, USP_MA_ETCREQPICK_Q)로 가져오거나 요청 없이 직접 품목을 입력해
/// 출고한다(TMAETCOUTM/TMAETCOUTD). 확정(USP_MA_ETCOUT_C_S)하면 수불(ETC_OUT, 출고계열)이 생기고 재고관리 품목(라인 재고반영=Y)만
/// 현재고(TMASTOCK)에서 차감된다 - 현재고가 모자라면 서버가 막는다. 요청을 불러온 라인은 요청 잔량 이내여야 하고 확정/확정취소가
/// 요청 라인의 출고수량(next_qty)을 다시 계산한다. frmGr와 같은 Master-One Sheet 구조.
/// </summary>
public partial class frmEtcOut : BaseForm, WYNLAB.Popup.IFeatureHost
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

    public frmEtcOut()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준: 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "기타출고등록";

        txtDeptNm.MapField("dept_id", txtDeptId);
        txtEmpNm.MapField("emp_id", txtEmpId);
        txtEmpNm.LinkDept(txtDeptNm, txtDeptId); // 담당자 팝업은 선택한 부서 소속만, 담당자를 고르면 부서도 채움

        gvw1.Role = GridRoleWyn.Edit;
        gvw1.HighlightFocusedRow = true;
        gvw1.AddColumnSummary(colOutQty, DevExpress.Data.SummaryItemType.Sum, "{0:#,##0.####}");
        gvw1.RowAdd += (s, e) => AddRow();
        gvw1.RowDelete += (s, e) => DeleteFocusedRow();
        gvw1.CellValueChanged += Gvw1_CellValueChanged;
        // 창고/위치는 이름 칸(wh_nm/loc_nm)에 팝업을 걸었다 - 팝업이 셀에 키값을 쓴 직후 ID와 이름을 같이 채운다.
        popcolWh.ResultSelected += (handle, result) => ApplyPickedMaster(handle, result, colWhId, colWhNm, "wh_nm");
        popcolLoc.ResultSelected += (handle, result) => ApplyPickedMaster(handle, result, colLocId, colLocNm, "loc_nm");
        // 요청에서 불러온 라인은 품목을 바꿀 수 없다(요청 품목 그대로 출고) - 직접 입력 라인만 품번을 고를 수 있다.
        gvw1.ShowingEditor += (s, e) =>
        {
            if (gvw1.FocusedColumn == colItemNo && gvw1.GetFocusedDataRow() is { } r && !string.IsNullOrEmpty(Convert.ToString(r["src_type"])))
                e.Cancel = true;
        };
        btnLoadReq.Click += async (s, e) => await SafeExecuteAsync(LoadFromRequestAsync, "요청 불러오기");
        btnAddRow1.Click += (s, e) => AddRow();
        btnDeletRow1.Click += (s, e) => DeleteFocusedRow();
        btnPickItem.Click += async (s, e) => await SafeExecuteAsync(PickItemsAsync, "품목선택");
        btnConfirm.Click += async (s, e) => await SafeExecuteAsync(() => ConfirmAsync(true), "기타출고 확정");
        btnConfirmCancel.Click += async (s, e) => await SafeExecuteAsync(() => ConfirmAsync(false), "기타출고 확정취소");

        TrackDirty(panData);
        featBar.FeaturesLoaded += (s, e) => ApplyLock();   // 메뉴 기능을 읽은 직후(결재 사용 여부 확정) 잠금/버튼 상태를 다시 맞춘다

        EnterNewMode();
        Load += async (s, e) =>
        {
            await SafeExecuteAsync(EnsureDetailSchemaAsync, "기타출고 화면 초기화");
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

        // 담은 라인의 현재고를 바로 보여준다.
        foreach (var added in _detail.Rows.Cast<DataRow>().Where(r => r.RowState == DataRowState.Added && string.IsNullOrEmpty(Convert.ToString(r["stock_yn"]))).ToList())
            await RefreshStockAsync(added, GridControl.InvalidRowHandle);
    }
    private void DeleteFocusedRow()
    {
        if (_statCd == "C") return;
        try { if (gvw1.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
        catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
    }

    private async Task EnsureDetailSchemaAsync()
    {
        if (_detail.Columns.Count > 0) return;
        var tables = await QueryMultiAsync("USP_MA_ETCOUT_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q", ["p_out_id"] = "-1" });
        if (tables.Count > 1 && _detail.Columns.Count == 0) _detail = tables[1];
        if (_editingKey == null) EnterNewMode();
    }

    private void Gvw1_CellValueChanged(object? sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
    {
        if (_syncingRow) return;

        if (e.Value is string text && e.Column == colOutQty)
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
        else if (e.Column == colWhNm || e.Column == colLocNm)
        {
            // 이름을 지우면 ID도 같이 비운다(팝업 선택 결과는 ApplyPickedMaster가 ID/이름을 함께 채운다).
            if (string.IsNullOrWhiteSpace(Convert.ToString(e.Value)))
            {
                _syncingRow = true;
                try { gvw1.SetRowCellValue(e.RowHandle, e.Column == colWhNm ? colWhId : colLocId, null); }
                finally { _syncingRow = false; }
                RefreshStockFor(e.RowHandle);
            }
        }

        // 현재고에 영향을 주는 값(품목/창고/위치/LOT)이 바뀌면 그 행의 현재고를 다시 읽어 보여준다.
        if (e.Column == colItemNo || e.Column == colLotNo)
        {
            var row = gvw1.GetDataRow(e.RowHandle);
            var handle = e.RowHandle;
            if (row != null) _ = SafeExecuteAsync(() => RefreshStockAsync(row, handle), "현재고 조회");
        }
    }

    /// <summary>이 행의 재고(품목/창고/위치/LOT)의 현재고와 품목의 재고관리 여부를 서버(USP_MA_STOCKQTY_Q)에서 읽어 현재고/재고반영 칸에 쓴다.
    /// 재고관리 품목이 아니거나 창고가 아직 없으면 현재고는 비운다. 재조회한 행(저장 상태 그대로)이 "수정됨"으로 바뀌어 저장 확인창이 뜨지 않게
    /// 변경 추적은 건드리지 않는다(Unchanged 행은 쓰고 나서 AcceptChanges).</summary>
    private async Task RefreshStockAsync(DataRow row, int handleHint)
    {
        if (row.RowState is DataRowState.Deleted) return;

        var itemId = Convert.ToString(row["item_id"]);
        var whId = Convert.ToString(row["wh_id"]);
        decimal? qty = null;
        var stockYn = Convert.ToString(row["stock_yn"]) is { Length: > 0 } cur ? cur : "N";

        if (!string.IsNullOrEmpty(itemId))
        {
            var result = await QueryAsync("USP_MA_STOCKQTY_Q", new Dictionary<string, string?>
            {
                ["p_acc_id"] = cboAccId.EditValue?.ToString(),
                ["p_work_type"] = "Q",
                ["p_item_id"] = itemId,
                ["p_wh_id"] = string.IsNullOrEmpty(whId) ? null : whId,
                ["p_loc_id"] = Convert.ToString(row["loc_id"]) is { Length: > 0 } loc ? loc : null,
                ["p_lot_no"] = Convert.ToString(row["lot_no"]),
            });
            if (result.Rows.Count > 0)
            {
                stockYn = Convert.ToString(result.Rows[0]["stock_yn"]) ?? "N";
                if (stockYn == "Y" && !string.IsNullOrEmpty(whId)) qty = Dec(result.Rows[0]["stock_qty"]);
            }
        }

        if (row.RowState is DataRowState.Deleted) return; // 조회를 기다리는 사이 행이 지워졌을 수 있다
        object qtyValue = qty.HasValue ? qty.Value : DBNull.Value;

        if (row.RowState == DataRowState.Detached)
        {
            // 방금 추가해서 아직 확정 전인 새 행은 그리드(SetRowCellValue)로 써야 바로 보인다.
            var handle = FindRowHandle(row, handleHint);
            if (handle == GridControl.InvalidRowHandle) return;
            _syncingRow = true;
            try
            {
                gvw1.SetRowCellValue(handle, colStockQty, qtyValue);
                gvw1.SetRowCellValue(handle, colStockYn, stockYn);
            }
            finally { _syncingRow = false; }
            return;
        }

        var wasDirty = IsDirty;
        var wasUnchanged = row.RowState == DataRowState.Unchanged;
        SuppressDirtyTracking(() =>
        {
            row["stock_qty"] = qtyValue;
            row["stock_yn"] = stockYn;
            if (wasUnchanged) row.AcceptChanges();
        });
        IsDirty = wasDirty;
    }

    /// <summary>이 DataRow를 가리키는 그리드 행 핸들 - 처음 알던 핸들(hint)이 아직 그 행이면 그대로, 아니면 테이블에서 위치를 찾는다.</summary>
    private int FindRowHandle(DataRow row, int hint)
    {
        if (hint != GridControl.InvalidRowHandle && ReferenceEquals(gvw1.GetDataRow(hint), row)) return hint;
        var index = _detail.Rows.IndexOf(row);
        return index >= 0 ? gvw1.GetRowHandle(index) : GridControl.InvalidRowHandle;
    }

    /// <summary>저장 직전 재고 통제 - 모든 라인의 현재고를 다시 읽어 보여주고, 재고관리 품목이 창고 없이 입력됐거나 같은 재고(품목/창고/위치/LOT)의
    /// 출고수량 합이 현재고를 넘으면 저장을 막는다(부족한 품목 목록을 안내). 서버(USP_MA_ETCOUT_S_1)도 같은 검증을 하고, 확정 때 한 번 더 한다.</summary>
    private async Task<bool> ValidateStockAsync()
    {
        var rows = _detail.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted && r.RowState != DataRowState.Detached).ToList();
        foreach (var r in rows) await RefreshStockAsync(r, GridControl.InvalidRowHandle);

        static bool IsStock(DataRow r) => Convert.ToString(r["stock_yn"]) == "Y";

        var noWarehouse = rows.Where(r => IsStock(r) && string.IsNullOrWhiteSpace(Convert.ToString(r["wh_id"]))).ToList();
        if (noWarehouse.Count > 0)
        {
            AppMessageBox.Show("재고관리 품목은 출고 창고를 입력해야 현재고를 확인할 수 있습니다.\n\n" +
                string.Join("\n", noWarehouse.Select(r => $"  - {r["item_no"]} {r["item_nm"]}")), "창고 입력 필요");
            return false;
        }

        var shortages = rows.Where(IsStock)
            .GroupBy(r => (Convert.ToString(r["item_id"]), Convert.ToString(r["wh_id"]), Convert.ToString(r["loc_id"]) ?? string.Empty, Convert.ToString(r["lot_no"]) ?? string.Empty))
            .Select(g => (First: g.First(), Total: g.Sum(r => Dec(r["out_qty"])), Stock: Dec(g.First()["stock_qty"])))
            .Where(x => x.Total > x.Stock)
            .ToList();
        if (shortages.Count == 0) return true;

        AppMessageBox.Show("현재고가 부족한 품목이 있어 저장할 수 없습니다. 출고수량을 줄이거나 재고를 확인하세요.\n\n" +
            string.Join("\n", shortages.Select(x =>
            {
                var lot = Convert.ToString(x.First["lot_no"]);
                return $"  - {x.First["item_no"]} {x.First["item_nm"]} / {x.First["wh_nm"]}{(string.IsNullOrEmpty(lot) ? string.Empty : $" / LOT {lot}")}\n" +
                       $"      현재고 {x.Stock:#,0.####}  <  출고 {x.Total:#,0.####}  (부족 {x.Total - x.Stock:#,0.####})";
            })), "재고 부족");
        return false;
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
        RefreshStockFor(rowHandle);
    }
    private void RefreshStockFor(int rowHandle)
    {
        if (gvw1.GetDataRow(rowHandle) is { } row) _ = SafeExecuteAsync(() => RefreshStockAsync(row, rowHandle), "현재고 조회");
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
        if (forceKey == null && string.IsNullOrWhiteSpace(txtSearchNo.Text))
        {
            EnterNewMode();
            Toast.Show("조회할 번호가 없어 신규 입력 상태로 전환했습니다.");
            return;
        }
        var tables = await QueryMultiAsync("USP_MA_ETCOUT_Q", new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "Q",
            ["p_out_id"] = forceKey,
            ["p_out_no"] = forceKey == null ? txtSearchNo.Text : null,
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
            _editingKey = row["out_id"]?.ToString();
            _statCd = row["stat_cd"]?.ToString() ?? "0";
            _appNo = row["app_no"]?.ToString();
            _apprStatCd = row["appr_stat_cd"]?.ToString();
            cboAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            txtDocNo.Text = row["out_no"]?.ToString() ?? string.Empty;
            txtSearchNo.Text = txtDocNo.Text;
            dteDocDate.YyyyMmDd = row["out_date"]?.ToString();
            cboStatCd.EditValue = _statCd;
            cboTransType.EditValue = row["trans_type"]?.ToString() ?? string.Empty;
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
            cboTransType.EditValue = "ETC_OUT"; // 출고유형 기본값 = 기타출고(요청을 불러오면 요청의 출고유형으로 바뀐다)
            txtDeptId.Text = Session.DeptId?.ToString() ?? string.Empty;
            txtDeptNm.Text = Session.DeptNm;
            txtEmpId.Text = Session.EmpId?.ToString() ?? string.Empty;
            txtEmpNm.Text = Session.EmpNm;
            txtCfmDt.Text = string.Empty;
            txtCfmUserId.Text = string.Empty;
            memoRemark.Text = string.Empty;

            _detail = _detail.Clone();
            TrackDirty(_detail);
            grd1.DataSource = _detail;
        });
        ApplyLock();
    }

    private void ApplyLock()
    {
        var locked = _statCd == "C" || featBar.ApprovalLocked;   // 결재 상신된 문서는 확정 전이라도 수정 불가(반려되면 다시 수정 가능)
        foreach (var edit in new BaseEdit[] { dteDocDate, cboTransType, txtDeptNm, txtEmpNm, memoRemark })
            edit.Properties.ReadOnly = locked;
        gvw1.OptionsBehavior.Editable = !locked;
        btnLoadReq.Enabled = !locked;
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

    /// <summary>요청 불러오기 - 승인완료(확정)된 기타출고요청 중 출고 잔량이 남은 라인을 골라 가져온다. 기본 출고수량은 잔량 전체,
    /// 이미 그리드에 있는 요청 라인은 건너뛴다. 창고/위치/LOT는 요청의 희망 값이 채워지고 확정 전에 바꿀 수 있다.</summary>
    private async Task LoadFromRequestAsync()
    {
        if (_statCd == "C") return;
        gvw1.CloseEditor();

        var columns = new[]
        {
            new PickColumn("src_no", "요청번호", 100), new PickColumn("req_date", "요청일", 80), new PickColumn("req_title", "제목", 130),
            new PickColumn("trans_type_nm", "출고유형", 90), new PickColumn("item_no", "품번", 100), new PickColumn("item_nm", "품명", 140),
            new PickColumn("item_spec", "규격", 110), new PickColumn("unit_cd", "단위", 50), new PickColumn("req_qty", "요청수량", 80, true),
            new PickColumn("remain_qty", "잔량", 80, true), new PickColumn("wh_nm", "창고", 90), new PickColumn("stock_yn", "재고", 45),
        };
        var picked = popPick.Pick(this, MenuId, "기타출고요청 불러오기", "USP_MA_ETCREQPICK_Q", "요청번호", columns,
            cboAccId.EditValue?.ToString(), string.Empty, string.Empty);
        if (picked == null || picked.Rows.Count == 0) return;

        // 불러온 요청의 출고유형을 이 출고에도 적용한다(요청이 여러 유형이면 첫 요청의 유형 - 한 출고는 한 수불유형으로 확정된다).
        if (picked.Columns.Contains("trans_type") && !string.IsNullOrEmpty(picked.Rows[0]["trans_type"]?.ToString()))
            cboTransType.EditValue = picked.Rows[0]["trans_type"].ToString();

        var skipped = 0;
        foreach (DataRow r in picked.Rows)
        {
            var srcId = r["src_id"]?.ToString();
            var srcSerl = r["src_serl"]?.ToString();
            var dup = _detail.Rows.Cast<DataRow>().Any(d =>
                d.RowState != DataRowState.Deleted && d["src_type"]?.ToString() == "ETCREQ"
                && d["src_id"]?.ToString() == srcId && d["src_serl"]?.ToString() == srcSerl);
            if (dup) { skipped++; continue; }

            var row = _detail.NewRow();
            row["item_id"] = r["item_id"];
            row["item_no"] = r["item_no"];
            row["item_nm"] = r["item_nm"];
            row["item_spec"] = r["item_spec"];
            row["unit_cd"] = r["unit_cd"];
            row["lot_no"] = r["lot_no"];
            row["out_qty"] = Dec(r["remain_qty"]);
            row["req_remain_qty"] = r["remain_qty"];
            row["wh_id"] = r["wh_id"];
            row["wh_nm"] = r["wh_nm"];
            row["loc_id"] = r["loc_id"];
            row["loc_nm"] = r["loc_nm"];
            row["stock_yn"] = r["stock_yn"];
            row["src_type"] = "ETCREQ";
            row["src_id"] = srcId;
            row["src_no"] = r["src_no"];
            row["src_serl"] = srcSerl;
            _detail.Rows.Add(row);
        }

        if (skipped > 0) Toast.Show($"이미 불러온 요청 품목 {skipped}건은 건너뛰었습니다.");

        // 불러온 라인의 현재고를 바로 보여준다.
        foreach (var added in _detail.Rows.Cast<DataRow>().Where(r => r.RowState == DataRowState.Added).ToList())
            await RefreshStockAsync(added, GridControl.InvalidRowHandle);
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
            AppMessageBox.Show("확정된 기타출고는 수정할 수 없습니다. 먼저 확정취소하세요.", "안내");
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
            }            if (Dec(row["out_qty"]) <= 0)
            {
                AppMessageBox.Show($"'{row["item_nm"]}' 출고수량을 0보다 크게 입력하세요.", "안내");
                return;
            }
            if (!string.IsNullOrEmpty(Convert.ToString(row["src_type"])) && row["req_remain_qty"] != DBNull.Value && Dec(row["out_qty"]) > Dec(row["req_remain_qty"]))
            {
                AppMessageBox.Show($"'{row["item_nm"]}' 출고수량이 요청 잔량({Dec(row["req_remain_qty"]):#,0.####})을 넘었습니다.", "안내");
                return;
            }
        }

        // 재고 통제 - 현재고를 다시 읽어 보여주고, 부족하면 저장하지 않는다.
        if (!await ValidateStockAsync()) return;

        var headerResult = await SaveAsync("USP_MA_ETCOUT_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_out_id"] = _editingKey,
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_out_date"] = dteDocDate.YyyyMmDd,
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

            var detailResult = await SaveAsync("USP_MA_ETCOUT_S_1", new Dictionary<string, string?>
            {
                ["p_work_type"] = workType,
                ["p_out_id"] = headerKey,
                ["p_serl"] = ProcData.Str(row, "serl", version),
                ["p_item_id"] = ProcData.Str(row, "item_id", version),
                ["p_out_qty"] = ProcData.Str(row, "out_qty", version),
                ["p_lot_no"] = ProcData.Str(row, "lot_no", version),
                ["p_wh_id"] = ProcData.Str(row, "wh_id", version),
                ["p_loc_id"] = ProcData.Str(row, "loc_id", version),
                ["p_src_type"] = ProcData.Str(row, "src_type", version),
                ["p_src_id"] = ProcData.Str(row, "src_id", version),
                ["p_src_serl"] = ProcData.Str(row, "src_serl", version),
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
        if (featBar.ApprovalLocked)
        {
            AppMessageBox.Show("결재 상신된 문서는 삭제할 수 없습니다.", "안내");
            return;
        }
        if (_editingKey == null) return;

        var result = await SaveAsync("USP_MA_ETCOUT_S", new Dictionary<string, string?> { ["p_work_type"] = "D", ["p_out_id"] = _editingKey });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "삭제에 실패했습니다.", "삭제 실패");
            return;
        }

        _editingKey = null;
        Toast.Show("삭제되었습니다.");
        EnterNewMode();
    }

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
            ? "기타출고를 확정하시겠습니까?\n확정하면 수불이 생성되고, 재고관리 품목은 현재고에서 차감됩니다."
            : "기타출고 확정을 취소하시겠습니까?\n역거래 수불이 남고 현재고가 복원됩니다.";
        if (AppMessageBox.Show(msg, $"기타출고 {label}", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        var result = await SaveAsync("USP_MA_ETCOUT_C_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = confirm ? "C" : "CC",
            ["p_out_id"] = _editingKey,
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
        Title = $"기타출고 {txtDocNo.Text}",
        Text = memoRemark.Text,
        HasUnsavedChanges = IsDirty,
        AppNo = _appNo,
        ApprStatCd = _apprStatCd,
    };

    public void OnFeatureChanged(string featureCd)
    {
        // 결재 상신/승인/반려로 문서 상태(잠금/확정)가 바뀌었을 수 있으니 서버 기준으로 다시 읽는다.
        if (featureCd == "APPROVAL" && _editingKey != null) _ = SafeExecuteAsync(() => QueryCore(forceKey: _editingKey), "기타출고 재조회");
    }

    public override async Task FocusRecordAsync(string key) => await QueryCore(forceKey: key);
}
