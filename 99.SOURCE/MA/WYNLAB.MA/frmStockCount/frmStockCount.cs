using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

/// <summary>
/// 재고실사등록 - 장부재고(TMASTOCK)와 실물 차이를 조정 수불(ADJ_IN/ADJ_OUT)로 맞춘다(TMACNTM/W/D). 설계: Document\재고실사_설계서.md.
/// 흐름: 작성(0: 헤더+대상창고) -> 대상선별(스냅샷으로 라인 선별, 곧바로 실사중(2): 수량 입력/라인 추가·삭제 - 예전 대상확정(1)+실사시작이 합쳐짐) -> 입력완료(3: 차이 고정) -> 확정(C) / 실사취소(X).
/// 규칙 - 선별된 라인은 삭제하지 않는 한 전부 실사수량을 입력해야 입력완료할 수 있고(0 유효, 빈칸 = 미입력) 전부 확정에 반영된다. 장부와 같으면 차이 0(수불 없음).
/// 상태 전이는 USP_MA_CNT_C_S 하나가 처리하고 화면은 버튼 활성/잠금만 상태(_statCd)로 맞춘다.
/// 저장 주의: 공통 저장 API가 빈 숫자 파라미터를 0으로 바꾸므로, 실사수량이 비어 있는(미입력) 행은 라인 저장을 호출하지 않는다 - 안 그러면 미입력이 0 입력으로 저장된다.
/// </summary>
public partial class frmStockCount : BaseForm, WYNLAB.Popup.IFeatureHost
{
    private DataTable _detail = new();
    private DataTable _itemCache = new();
    private string? _editingKey; // null이면 신규모드
    private string _statCd = "0";
    private string? _freezeMode;   // 대상선별 때 고정된 수불 통제 방식: A 완전 동결 / B 변동 보정
    private string? _appNo;        // 연결된 전자결재 번호/상태(메뉴에서 결재를 켠 경우만 의미가 있다)
    private string? _apprStatCd;
    private bool _syncingRow;

    public frmStockCount()
    {
        InitializeComponent();

        Text = "재고실사등록";
        Controls.Add(BuildScreenHeader());

        // 조회조건 사업장 - 화면 표준: 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        txtWhNm.MapField("wh_id", txtWhId);
        txtDeptNm.MapField("dept_id", txtDeptId);
        txtEmpNm.MapField("emp_id", txtEmpId);
        txtEmpNm.LinkDept(txtDeptNm, txtDeptId); // 담당자 팝업은 선택한 부서 소속만, 담당자를 고르면 부서도 채움

        // ---- 라인 그리드
        gvw1.Role = GridRoleWyn.Edit;
        gvw1.HighlightFocusedRow = true;
        gvw1.OptionsSelection.MultiSelect = true;   // 재실사지정에서 여러 라인을 체크해서 고른다
        gvw1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
        gvw1.RowAdd += (s, e) => AddRow();
        gvw1.RowDelete += (s, e) => DeleteFocusedRow();
        gvw1.InitNewRow += Gvw1_InitNewRow;
        gvw1.CellValueChanged += Gvw1_CellValueChanged;
        gvw1.ShowingEditor += Gvw1_ShowingEditor;
        gvw1.RowCellStyle += Gvw1_RowCellStyle;
        gvw1.KeyDown += Gvw1_KeyDown;

        btnAddRow1.Click += (s, e) => AddRow();
        btnDeletRow1.Click += (s, e) => DeleteFocusedRow();
        chkUnentered.CheckedChanged += (s, e) => ApplyUnenteredFilter();

        btnSnap.Click += async (s, e) => await SafeExecuteAsync(() => StateActionAsync("SNAP", "대상선별", null,
            "지금 재고를 스냅샷으로 떠서 실사 대상 라인을 선별하고 바로 수량 입력(실사중)을 시작합니다.\n대상선별 후에는 헤더와 대상 창고를 바꿀 수 없습니다. 계속하시겠습니까?"), "대상선별");
        btnDone.Click += async (s, e) => await SafeExecuteAsync(() => StateActionAsync("DONE", "입력완료", null,
            "입력을 완료하고 차이를 계산·고정합니다.\n모든 라인의 실사수량이 입력돼 있어야 합니다(0 가능, 빼려는 라인은 삭제). 계속하시겠습니까?"), "입력완료");
        btnUndone.Click += async (s, e) => await SafeExecuteAsync(() => StateActionAsync("UNDONE", "입력완료취소", null,
            "입력완료를 취소하고 실사중 상태로 되돌립니다. 계속하시겠습니까?"), "입력완료취소");
        btnRecnt.Click += async (s, e) => await SafeExecuteAsync(RecountAsync, "재실사지정");
        btnConfirm.Click += async (s, e) => await SafeExecuteAsync(() => StateActionAsync("C", "확정", null,
            "재고실사를 확정하시겠습니까?\n차이가 있는 라인은 조정 수불(조정증가/조정감소)이 생성되고 현재고가 맞춰집니다."), "재고실사 확정");
        btnConfirmCancel.Click += async (s, e) => await SafeExecuteAsync(() => StateActionAsync("CC", "확정취소", null,
            "확정을 취소하시겠습니까?\n역거래 수불이 남고 현재고가 원래대로 복원됩니다(조정으로 늘어난 재고를 이미 썼으면 취소할 수 없습니다)."), "재고실사 확정취소");
        btnCancelDoc.Click += async (s, e) => await SafeExecuteAsync(() => StateActionAsync("X", "실사취소", null,
            "이 재고실사를 취소하시겠습니까?\n재고에는 아무 영향이 없고, 같은 창고의 진행 제한이 풀립니다. 문서는 이력으로 남습니다."), "실사취소");

        TrackDirty(panData);
        featBar.FeaturesLoaded += (s, e) => ApplyState();   // 메뉴 기능을 읽은 직후(결재 사용 여부 확정) 잠금/버튼 상태를 다시 맞춘다

        EnterNewMode();
        Load += async (s, e) =>
        {
            await SafeExecuteAsync(EnsureDetailSchemaAsync, "재고실사 화면 초기화");
            _itemCache = await LoadPopupRowsAsync("P_ITEM");
        };
    }

    private static decimal Dec(object? value) => value == null || value == DBNull.Value ? 0 : decimal.TryParse(value.ToString(), out var d) ? d : 0;

    private static object? Col(DataRow? row, string column)
    {
        if (row == null || !row.Table.Columns.Contains(column)) return null;
        var value = row[column];
        return value == DBNull.Value ? null : value;
    }

    private bool Locked => featBar.ApprovalLocked;   // 결재 상신된 문서는 수정 불가(반려되면 다시 수정 가능)

    // ================= 라인 그리드 =================

    private void AddRow()
    {
        if (_statCd != "2" || Locked) return;
        if (_detail.Columns.Count == 0) { Toast.Show("화면을 준비하는 중입니다. 잠시 후 다시 시도해주세요."); return; }
        gvw1.AddNewRow();
    }

    /// <summary>실사중에서만, 이번 실사에서 뺄 라인을 삭제한다(삭제는 저장 때 서버에 반영).</summary>
    private void DeleteFocusedRow()
    {
        if (_statCd != "2" || Locked) return;
        try { if (gvw1.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
        catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        UpdateSummary();
    }

    /// <summary>계획 외로 추가하는 라인의 창고는 헤더 창고로 고정된다(서버도 헤더 창고로 저장).</summary>
    private void Gvw1_InitNewRow(object? sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
    {
        gvw1.SetRowCellValue(e.RowHandle, colAddYn, "Y");
        gvw1.SetRowCellValue(e.RowHandle, colRecntYn, "N");
        gvw1.SetRowCellValue(e.RowHandle, colWhId, txtWhId.Text);
        gvw1.SetRowCellValue(e.RowHandle, colWhNm, txtWhNm.Text);
    }

    /// <summary>상태별 편집 허용 칸: 실사중(2)은 수량·조정사유·비고(+계획 외로 추가한 새 행의 품번/창고/LOT), 입력완료(3)는 조정사유·비고만, 그 외는 읽기전용.</summary>
    private void Gvw1_ShowingEditor(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        var col = gvw1.FocusedColumn;
        var row = gvw1.GetFocusedDataRow();
        var isNew = row == null || row.RowState is DataRowState.Added or DataRowState.Detached;

        if (Locked || _statCd is not ("2" or "3")) { e.Cancel = true; return; }
        if (col == colItemNo || col == colLotNo) e.Cancel = !(isNew && _statCd == "2");
        else if (col == colFinQty) e.Cancel = _statCd != "2";
        else if (col == colAdjReason || col == colRemark) e.Cancel = false;
        else e.Cancel = true;
    }

    private void Gvw1_CellValueChanged(object? sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
    {
        if (_syncingRow) return;

        // 직접 타이핑한 숫자는 문자열로 들어온다 - 서식이 안 먹으므로 숫자로 바꿔 다시 쓴다(음수는 거부).
        if (e.Value is string text && e.Column == colFinQty)
        {
            object? number = decimal.TryParse(text.Replace(",", ""), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var n) ? n : null;
            if (number is decimal d && d < 0) { Toast.Show("실사수량은 0 이상이어야 합니다."); number = null; }
            gvw1.SetRowCellValue(e.RowHandle, e.Column, number);
            return;
        }

        if (e.Column == colFinQty)
        {
            // 장부/변동이 보이는 상태면 차이를 바로 다시 계산해 보여준다(서버는 입력완료/확정 때 다시 계산해 고정한다).
            var book = gvw1.GetRowCellValue(e.RowHandle, colBookQty);
            var fin = gvw1.GetRowCellValue(e.RowHandle, colFinQty);
            _syncingRow = true;
            try
            {
                if (fin == null || fin == DBNull.Value || book == null || book == DBNull.Value) gvw1.SetRowCellValue(e.RowHandle, colDiffQty, null);
                else gvw1.SetRowCellValue(e.RowHandle, colDiffQty, Dec(fin) - (Dec(book) + Dec(gvw1.GetRowCellValue(e.RowHandle, colMoveQty))));
            }
            finally { _syncingRow = false; }
            UpdateSummary();
        }
        else if (e.Column == colItemNo)
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
            }
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

    /// <summary>차이는 부호별 색(+ 파랑 / - 빨강), 변동이 있는 라인은 변동 칸을 주황으로 - 스냅샷 이후 수불이 있었다는 표시.</summary>
    private void Gvw1_RowCellStyle(object? sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
    {
        if (e.Column == colDiffQty)
        {
            var d = gvw1.GetRowCellValue(e.RowHandle, colDiffQty);
            if (d == null || d == DBNull.Value) return;
            if (Dec(d) > 0) e.Appearance.ForeColor = System.Drawing.Color.FromArgb(0, 90, 200);
            else if (Dec(d) < 0) e.Appearance.ForeColor = System.Drawing.Color.FromArgb(200, 40, 40);
        }
        else if (e.Column == colMoveQty)
        {
            var m = gvw1.GetRowCellValue(e.RowHandle, colMoveQty);
            if (m != null && m != DBNull.Value && Dec(m) != 0) e.Appearance.ForeColor = System.Drawing.Color.FromArgb(220, 110, 0);
        }
    }

    /// <summary>실사수량 칸에서 Enter = 확정하고 아래 행으로 이동(연속 입력).</summary>
    private void Gvw1_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter || gvw1.FocusedColumn != colFinQty || _statCd != "2") return;
        if (gvw1.IsNewItemRow(gvw1.FocusedRowHandle)) return;
        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();
        gvw1.MoveNext();
        e.Handled = true;
    }

    private void ApplyUnenteredFilter()
    {
        gvw1.ActiveFilterString = chkUnentered.Checked ? "[fin_qty] Is Null" : string.Empty;
    }

    /// <summary>총 라인 / 입력 / 미입력 / 차이 건수 - 입력완료 전에 남은 일이 얼마인지 한눈에 보게 한다.</summary>
    private void UpdateSummary()
    {
        var rows = _detail.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted).ToList();
        if (_detail.Columns.Count == 0 || rows.Count == 0) { lblSummary.Text = string.Empty; return; }

        var entered = rows.Count(r => r["fin_qty"] != DBNull.Value);
        var diff = rows.Count(r => r["diff_qty"] != DBNull.Value && Dec(r["diff_qty"]) != 0);
        var diffText = $"  /  차이 {diff}건";
        var freezeText = _freezeMode == "A" && _statCd is "1" or "2" or "3" ? "  /  [창고 동결 중 - 이 창고의 입출고/이동 차단]" : string.Empty;
        lblSummary.Text = $"라인 {rows.Count}건  /  입력 {entered}건  /  미입력 {rows.Count - entered}건{diffText}{freezeText}";
    }

    // ================= 조회 / 신규 =================

    private async Task EnsureDetailSchemaAsync()
    {
        if (_detail.Columns.Count > 0) return;
        var tables = await QueryMultiAsync("USP_MA_CNT_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q", ["p_cnt_id"] = "-1" });
        if (tables.Count > 1 && _detail.Columns.Count == 0) _detail = tables[1];
        if (_editingKey == null) EnterNewMode();
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
        var tables = await QueryMultiAsync("USP_MA_CNT_Q", new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "Q",
            ["p_cnt_id"] = forceKey,
            ["p_cnt_no"] = forceKey == null ? txtSearchNo.Text : null,
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
            _editingKey = row["cnt_id"]?.ToString();
            _statCd = row["stat_cd"]?.ToString() ?? "0";

            _freezeMode = row["freeze_mode"]?.ToString();
            _appNo = row["app_no"]?.ToString();
            _apprStatCd = row["appr_stat_cd"]?.ToString();
            cboAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            txtDocNo.Text = row["cnt_no"]?.ToString() ?? string.Empty;
            txtSearchNo.Text = txtDocNo.Text;
            dteDocDate.YyyyMmDd = row["cnt_date"]?.ToString();
            cboStatCd.EditValue = _statCd;
            cboCntType.EditValue = row["cnt_type"]?.ToString() ?? string.Empty;
            chkUnentered.Checked = false;
            txtWhId.Text = row["wh_id"]?.ToString() ?? string.Empty;
            txtWhNm.Text = row["wh_nm"]?.ToString() ?? string.Empty;
            txtCntTitle.Text = row["cnt_title"]?.ToString() ?? string.Empty;
            txtDeptId.Text = row["dept_id"]?.ToString() ?? string.Empty;
            txtDeptNm.Text = row["dept_nm"]?.ToString() ?? string.Empty;
            txtEmpId.Text = row["emp_id"]?.ToString() ?? string.Empty;
            txtEmpNm.Text = row["emp_nm"]?.ToString() ?? string.Empty;
            txtTolQty.Text = row["tol_qty"] is DBNull ? string.Empty : $"{Dec(row["tol_qty"]):#,0.####}";
            txtTolRate.Text = row["tol_rate"] is DBNull ? string.Empty : $"{Dec(row["tol_rate"]):#,0.####}";
            txtCfmDt.Text = row["cfm_dt"] is DateTime dt ? dt.ToString("yyyy-MM-dd HH:mm") : string.Empty;
            txtCfmUserId.Text = row["cfm_user_id"]?.ToString() ?? string.Empty;
            memoRemark.Text = row["remark"]?.ToString() ?? string.Empty;

            TrackDirty(_detail);
            grd1.DataSource = _detail;
            gvw1.ActiveFilterString = string.Empty;
        });
        ApplyState();
    }

    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = null;
            _statCd = "0";
            _freezeMode = null;
            _appNo = null;
            _apprStatCd = null;
            cboAccId.EditValue = Session.AccId?.ToString();
            txtDocNo.Text = string.Empty;
            dteDocDate.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");
            cboStatCd.EditValue = "0";
            cboCntType.EditValue = "ALL";
            chkUnentered.Checked = false;
            txtWhId.Text = string.Empty;
            txtWhNm.ClearSelf();
            txtCntTitle.Text = string.Empty;
            txtDeptId.Text = Session.DeptId?.ToString() ?? string.Empty;
            txtDeptNm.Text = Session.DeptNm;
            txtEmpId.Text = Session.EmpId?.ToString() ?? string.Empty;
            txtEmpNm.Text = Session.EmpNm;
            txtTolQty.Text = string.Empty;
            txtTolRate.Text = string.Empty;
            txtCfmDt.Text = string.Empty;
            txtCfmUserId.Text = string.Empty;
            memoRemark.Text = string.Empty;

            _detail = _detail.Clone();
            TrackDirty(_detail);
            grd1.DataSource = _detail;
            gvw1.ActiveFilterString = string.Empty;
        });
        ApplyState();
    }

    public override Task NewClick()
    {
        EnterNewMode();
        FocusFirstEntryField(panData);
        return Task.CompletedTask;
    }

    /// <summary>상태(_statCd)와 결재 잠금으로 입력칸/그리드/버튼 활성을 한 곳에서 맞춘다.</summary>
    private void ApplyState()
    {
        var headEditable = _statCd == "0" && !Locked;
        foreach (var edit in new BaseEdit[] { txtWhNm, cboCntType, dteDocDate, txtCntTitle, txtDeptNm, txtEmpNm, txtTolQty, txtTolRate, memoRemark })
            edit.Properties.ReadOnly = !headEditable;

        gvw1.OptionsBehavior.Editable = !Locked && _statCd is "2" or "3";
        btnAddRow1.Enabled = !Locked && _statCd == "2";
        btnDeletRow1.Enabled = !Locked && _statCd == "2";
        chkUnentered.Enabled = _statCd is "2" or "3";

        var saved = _editingKey != null;
        btnSnap.Enabled = saved && _statCd == "0" && !Locked;
        btnDone.Enabled = saved && _statCd == "2" && !Locked;
        btnUndone.Enabled = saved && _statCd == "3" && !Locked;
        btnRecnt.Enabled = saved && _statCd == "3" && !Locked;
        btnConfirm.Enabled = saved && _statCd == "3";            // 결재 상신된 실사는 승인완료여야 서버가 확정시킨다
        btnConfirmCancel.Enabled = saved && _statCd == "C";
        btnCancelDoc.Enabled = saved && _statCd is "2" or "3" && !Locked;

        // 변동 컬럼은 변동 보정(B) 방식으로 시작한 실사에서만 보인다 - 동결(A, 기본)은 선별 후 수불이 없어 항상 0이라 숨긴다.
        colMoveQty.Visible = _freezeMode == "B";

        UpdateSummary();
        featBar.UpdateState();
    }

    // ================= 저장 / 삭제 =================

    public override async Task SaveClick()
    {
        if (Locked) { AppMessageBox.Show("결재 상신된 문서는 수정할 수 없습니다.", "안내"); return; }
        if (_statCd is "C" or "X") { AppMessageBox.Show("확정/취소된 재고실사는 수정할 수 없습니다.", "안내"); return; }

        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        var key = _editingKey;

        if (_statCd == "0")
        {
            var headerResult = await SaveAsync("USP_MA_CNT_S", new Dictionary<string, string?>
            {
                ["p_work_type"] = _editingKey == null ? "N" : "U",
                ["p_cnt_id"] = _editingKey,
                ["p_acc_id"] = cboAccId.EditValue?.ToString(),
                ["p_wh_id"] = txtWhId.Text,
                ["p_cnt_title"] = txtCntTitle.Text,
                ["p_cnt_type"] = cboCntType.EditValue?.ToString(),
                ["p_cnt_date"] = dteDocDate.YyyyMmDd,
                ["p_tol_qty"] = txtTolQty.Text.Replace(",", ""),
                ["p_tol_rate"] = txtTolRate.Text.Replace(",", ""),
                ["p_dept_id"] = txtDeptId.Text,
                ["p_emp_id"] = txtEmpId.Text,
                ["p_remark"] = memoRemark.Text,
            });
            if (headerResult == null || !headerResult.Success)
            {
                AppMessageBox.Show(headerResult?.Message ?? "저장에 실패했습니다.", "저장 실패");
                return;
            }
            key = _editingKey ?? headerResult.GeneratedCode;
        }
        else
        {
            if (!ValidateLines()) return;

            foreach (DataRow row in _detail.Rows.Cast<DataRow>().ToList())
            {
                if (row.RowState == DataRowState.Unchanged) continue;

                var deleted = row.RowState == DataRowState.Deleted;
                var added = row.RowState == DataRowState.Added;
                var version = deleted ? DataRowVersion.Original : DataRowVersion.Current;
                var qty = ProcData.Str(row, "fin_qty", version);

                // 빈 수량 행은 서버에 보내지 않는다(공통 API가 빈 숫자를 0으로 바꿔 미입력이 0 입력으로 저장되는 것을 막는다).
                if (!deleted && string.IsNullOrEmpty(qty)) continue;

                var result = await SaveAsync("USP_MA_CNT_S_1", new Dictionary<string, string?>
                {
                    ["p_work_type"] = deleted ? "D" : added ? "N" : "U",
                    ["p_cnt_id"] = key,
                    ["p_serl"] = ProcData.Str(row, "serl", version),
                    ["p_item_id"] = ProcData.Str(row, "item_id", version),
                    ["p_lot_no"] = ProcData.Str(row, "lot_no", version),
                    ["p_cnt_qty"] = qty,
                    ["p_adj_reason"] = ProcData.Str(row, "adj_reason", version),
                    ["p_remark"] = ProcData.Str(row, "remark", version),
                });
                if (result == null || !result.Success)
                {
                    AppMessageBox.Show(result?.Message ?? "라인 저장에 실패했습니다.", "저장 실패");
                    await QueryCore(forceKey: key);
                    return;
                }
            }
        }

        _editingKey ??= key;
        Toast.Show("저장되었습니다.");
        await QueryCore(forceKey: _editingKey);
    }

    /// <summary>실사중에 계획 외로 추가한 새 라인은 품목/창고/수량이 있어야 한다(서버가 한 번 더 검증).</summary>
    private bool ValidateLines()
    {
        foreach (DataRow row in _detail.Rows)
        {
            if (row.RowState != DataRowState.Added) continue;
            if (string.IsNullOrWhiteSpace(Convert.ToString(row["item_id"])))
            {
                AppMessageBox.Show("추가한 라인의 품번을 입력하세요. (팝업에서 선택)", "안내");
                return false;
            }
            if (row["fin_qty"] == DBNull.Value)
            {
                AppMessageBox.Show($"추가한 라인 '{row["item_nm"]}'의 실사수량을 입력하세요.", "안내");
                return false;
            }
        }
        return true;
    }

    public override async Task DeleteClick()
    {
        if (Locked) { AppMessageBox.Show("결재 상신된 문서는 삭제할 수 없습니다.", "안내"); return; }
        if (_editingKey == null) return;
        if (_statCd != "0")
        {
            AppMessageBox.Show("작성 상태의 재고실사만 삭제할 수 있습니다. 대상선별 이후에는 '실사취소'로 정리하세요.", "안내");
            return;
        }

        var result = await SaveAsync("USP_MA_CNT_S", new Dictionary<string, string?> { ["p_work_type"] = "D", ["p_cnt_id"] = _editingKey });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "삭제에 실패했습니다.", "삭제 실패");
            return;
        }

        _editingKey = null;
        Toast.Show("삭제되었습니다.");
        EnterNewMode();
    }

    // ================= 상태 전이 =================

    private async Task StateActionAsync(string type, string label, string? serls = null, string? confirmMsg = null)
    {
        if (_editingKey == null) { AppMessageBox.Show($"먼저 저장한 뒤 {label}하세요.", "안내"); return; }
        gvw1.CloseEditor();
        if (IsDirty) { AppMessageBox.Show("저장하지 않은 변경이 있습니다. 먼저 저장하세요.", "안내"); return; }
        if (confirmMsg != null && AppMessageBox.Show(confirmMsg, $"재고실사 {label}", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        var result = await SaveAsync("USP_MA_CNT_C_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = type,
            ["p_cnt_id"] = _editingKey,
            ["p_serls"] = serls,
        });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "처리에 실패했습니다.", $"{label} 실패");
            return;
        }

        Toast.Show($"{label}되었습니다.");
        await QueryCore(forceKey: _editingKey);

        if (type == "SNAP")
        {
            // 다른 진행 중 실사에 이미 포함된 재고는 이 실사에서 제외된다 - 서버가 건수/실사번호를 메시지로 돌려준다.
            if (result.Message?.Contains("제외") == true) AppMessageBox.Show(result.Message, "대상선별 안내");

            // 대상 재고가 하나도 없는 창고도 대상선별은 통과한다 - '라인추가'로 발견한 재고를 직접 넣는다.
            if (_detail.Rows.Count == 0)
                AppMessageBox.Show("대상 재고(현재고 행)가 없어 실사 라인이 0건입니다.\n\n[라인추가]로 실사할 품목/LOT/수량을 직접 입력하세요.", "안내");
        }
    }

    /// <summary>체크한(없으면 포커스 행) 라인을 재실사로 지정한다 - 그 라인은 실사수량이 비워지고 다시 입력해야 한다.</summary>
    private async Task RecountAsync()
    {
        var handles = gvw1.GetSelectedRows().Where(h => h >= 0).ToList();
        if (handles.Count == 0 && gvw1.FocusedRowHandle >= 0) handles.Add(gvw1.FocusedRowHandle);
        if (handles.Count == 0) { AppMessageBox.Show("재실사할 라인을 체크하세요.", "안내"); return; }

        var serls = string.Join(",", handles.Select(h => Convert.ToString(gvw1.GetRowCellValue(h, colSerl))).Where(s => !string.IsNullOrEmpty(s)));
        await StateActionAsync("RECNT", "재실사지정", serls,
            $"선택한 {handles.Count}건을 재실사로 지정하고 실사중 상태로 되돌립니다.\n해당 라인의 실사수량은 비워지고 다시 입력해야 합니다. 계속하시겠습니까?");
    }

    /// <summary>결재 본문 - 결재자가 열어보지 않고도 판단할 수 있게 창고/기준일/라인·차이 건수를 요약하고, 차이 라인(최대 10건)과 비고를 붙인다.</summary>
    private string BuildApprovalText()
    {
        var rows = _detail.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted).ToList();
        var diffRows = rows.Where(r => r.Table.Columns.Contains("diff_qty") && r["diff_qty"] != DBNull.Value && Dec(r["diff_qty"]) != 0).ToList();

        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"창고: {txtWhNm.Text}   기준일: {dteDocDate.YyyyMmDd}   라인 {rows.Count}건 중 차이 {diffRows.Count}건");
        foreach (var r in diffRows.Take(10))
            sb.AppendLine($" - {r["item_no"]} {r["item_nm"]} / LOT {r["lot_no"]}: 장부 {Dec(r["book_qty"]):#,0.####} → 실사 {Dec(r["fin_qty"]):#,0.####} (차이 {Dec(r["diff_qty"]):+#,0.####;-#,0.####})");
        if (diffRows.Count > 10) sb.AppendLine($" ... 외 {diffRows.Count - 10}건");
        if (!string.IsNullOrWhiteSpace(memoRemark.Text)) sb.AppendLine().Append(memoRemark.Text);
        return sb.ToString();
    }

    // ---- 공통 기능 패널(FeatureBarWyn: 전자결재/첨부파일) 연동
    public WYNLAB.Popup.FeatureContext GetFeatureContext() => new()
    {
        DocId = long.TryParse(_editingKey, out var id) ? id : null,
        DocNo = txtDocNo.Text,
        Title = $"재고실사 {txtDocNo.Text}{(string.IsNullOrWhiteSpace(txtCntTitle.Text) ? string.Empty : " - " + txtCntTitle.Text)}",
        Text = BuildApprovalText(),
        HasUnsavedChanges = IsDirty,
        AppNo = _appNo,
        ApprStatCd = _apprStatCd,
    };

    public void OnFeatureChanged(string featureCd)
    {
        // 결재 상신/승인/반려로 문서 상태(잠금)가 바뀌었을 수 있으니 서버 기준으로 다시 읽는다.
        if (featureCd == "APPROVAL" && _editingKey != null) _ = SafeExecuteAsync(() => QueryCore(forceKey: _editingKey), "재고실사 재조회");
    }

    public override async Task FocusRecordAsync(string key) => await QueryCore(forceKey: key);
}
