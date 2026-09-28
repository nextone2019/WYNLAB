using System.Data;
using System.Drawing;
using System.Globalization;
using DevExpress.XtraEditors;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.SA;

/// <summary>
/// 판매단가등록 - 구매단가등록(frmPoPrice)과 동일한 품목 마스터 중심 구조.
///  - grd1(왼쪽): 품목마스터(TBAITEM) 목록 + 품목별 "최종단가"(USP_SA_PRICE_Q 'Q').
///  - panData(오른쪽 위): grd1에서 고른 품목의 정보(읽기 전용).
///  - grd2(오른쪽 아래): 그 품목의 판매단가 목록(USP_SA_PRICE_Q 'Q2') - 여기서 바로 추가/수정/삭제/개정하고 저장한다.
///
/// 적용 규칙: 거래처가 비어 있으면 "전체 거래처 공통 단가", 채워져 있으면 그 거래처에만 적용되는 단가.
/// 종료일이 비어 있으면 무기한(서버에는 '99991231'). 같은 품목 + 같은 거래처 범위 안에서 기간이 겹치는
/// 단가는 저장할 수 없다(서버 USP_SA_PRICE_S가 최종 검증, 화면도 저장 전에 미리 검사).
/// 나머지 동작(거래처 팝업/타이핑, 단가개정, 상태 색상, 저장 순서)은 frmPoPrice와 동일하다.
/// </summary>
public partial class frmSaPrice : BaseForm
{
    private const string OpenEnd = "99991231";

    private DataTable _items = new();
    private DataTable _prices = BuildEmptyTable();
    private DataTable _custCache = new();
    private string? _editingItemId;
    private bool _syncing;

    public frmSaPrice()
    {
        InitializeComponent();

        Text = "판매단가등록";

        cboAccId.EditValue = Session.AccId?.ToString();
        cboPriceYn.SelectedIndex = 0;

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;
        gvw1.CustomColumnDisplayText += Gvw1_CustomColumnDisplayText;
        colMLastPrice.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

        gvw2.Role = GridRoleWyn.Edit;
        gvw2.HighlightFocusedRow = true;
        gvw2.HighlightUnsavedCells = true;
        colPrice.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

        gvw2.RowAdd += (s, e) => AddRow();
        gvw2.RowDelete += (s, e) => DeleteSelectedRows();
        gvw2.InitNewRow += Gvw2_InitNewRow;
        gvw2.CellValueChanged += Gvw2_CellValueChanged;
        gvw2.CustomColumnDisplayText += Gvw2_CustomColumnDisplayText;
        gvw2.RowCellStyle += Gvw2_RowCellStyle;

        btnAddRow1.Click += (s, e) => AddRow();
        btnDeletRow1.Click += (s, e) => DeleteSelectedRows();
        btnRevise.Click += (s, e) => ReviseSelectedRow();

        cboDetailAccCd.Tag = new BindingFieldTag("acc_id");
        txtDetailItemNo.Tag = new BindingFieldTag("item_no");
        txtDetailItemNm.Tag = new BindingFieldTag("item_nm");
        txtDetailItemSpec.Tag = new BindingFieldTag("item_spec");
        txtDetailCustNm.Tag = new BindingFieldTag("cust_nm");
        cboDetailStatCd.Tag = new BindingFieldTag("stat_cd");
        cboDetailUnitCd.Tag = new BindingFieldTag("unit_cd");

        TrackDirty(_prices);
        grd2.DataSource = _prices;

        Load += async (s, e) =>
        {
            try
            {
                _custCache = await LoadPopupRowsAsync("P_CUST");
            }
            catch
            {
                // 캐시는 타이핑/붙여넣기 자동채움용일 뿐 - 못 받아도 팝업 선택은 정상 동작한다.
            }

            await QueryCore(restoreItemId: null);
        };
    }

    private static DataTable BuildEmptyTable()
    {
        var table = new DataTable();
        foreach (var col in new[]
        {
            "price_id", "acc_id", "item_id", "item_no", "item_nm", "item_spec", "cust_id", "cust_nm",
            "start_date", "end_date", "cur_cd", "unit_cd", "price", "remark", "stat_nm"
        })
        {
            table.Columns.Add(col, typeof(object));
        }
        return table;
    }

    // ==================== 조회 ====================

    public override async Task QueryClick()
    {
        if (HasUnsavedChanges)
        {
            var confirm = AppMessageBox.Show(
                "저장하지 않은 변경 내용이 있습니다.\n조회하면 변경 내용이 사라집니다. 계속하시겠습니까?",
                "조회 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;
        }

        await QueryCore(restoreItemId: null);
    }

    private async Task QueryCore(string? restoreItemId)
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_acc_id"] = NullIfEmpty(cboAccId.EditValue?.ToString()),
            ["p_item_kw"] = NullIfEmpty(txtKeyword.Text.Trim()),
            ["p_price_yn"] = cboPriceYn.SelectedIndex switch { 1 => "Y", 2 => "N", _ => null },
        };

        var table = await QueryAsync("USP_SA_PRICE_Q", p);

        gvw1.FocusedRowObjectChanged -= Gvw1_FocusedRowObjectChanged;
        SuppressMasterRowSwitchConfirm(gvw1, () =>
        {
            _items = table;
            grd1.DataSource = _items;

            if (restoreItemId != null)
            {
                var index = _items.Rows.Cast<DataRow>().ToList().FindIndex(r => Cell(r, "item_id") == restoreItemId);
                if (index >= 0) gvw1.FocusedRowHandle = gvw1.GetRowHandle(index);
            }
        });
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        if (gvw1.GetFocusedDataRow() is DataRow focused) await LoadItemAsync(focused);
        else ClearItem();
    }

    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e) =>
        ConfirmMasterRowSwitch(gvw1, e, row => _ = SafeExecuteAsync(() => LoadItemAsync(row.Row), "단가 조회"));

    private async Task LoadItemAsync(DataRow item)
    {
        var itemId = Cell(item, "item_id");
        _editingItemId = itemId;

        cboDetailAccCd.EditValue = Cell(item, "acc_id");
        txtDetailItemNo.Text = Cell(item, "item_no");
        txtDetailItemNm.Text = Cell(item, "item_nm");
        txtDetailItemSpec.Text = Cell(item, "item_spec");
        txtDetailCustNm.Text = Cell(item, "cust_nm");
        cboDetailStatCd.EditValue = Cell(item, "stat_cd");
        cboDetailUnitCd.EditValue = Cell(item, "unit_cd");

        var prices = await QueryAsync("USP_SA_PRICE_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q2",
            ["p_acc_id"] = NullIfEmpty(Cell(item, "acc_id")),
            ["p_item_id"] = itemId,
        });

        if (_editingItemId != itemId) return;

        SetPrices(prices);
    }

    private void ClearItem()
    {
        _editingItemId = null;
        foreach (var edit in new BaseEdit[] { cboDetailAccCd, txtDetailItemNo, txtDetailItemNm, txtDetailItemSpec, txtDetailCustNm,
                                              cboDetailStatCd, cboDetailUnitCd })
            edit.EditValue = null;
        SetPrices(BuildEmptyTable());
    }

    private void SetPrices(DataTable table)
    {
        SuppressDirtyTracking(() =>
        {
            _prices = table;
            TrackDirty(_prices);
            grd2.DataSource = _prices;
        });
    }

    // ==================== 행 추가/삭제/개정 ====================

    public override Task NewClick()
    {
        AddRow();
        return Task.CompletedTask;
    }

    public override Task NewRowClick()
    {
        AddRow();
        return Task.CompletedTask;
    }

    public override Task DeleteClick()
    {
        DeleteSelectedRows();
        return Task.CompletedTask;
    }

    public override Task DeleteRowClick()
    {
        DeleteSelectedRows();
        return Task.CompletedTask;
    }

    private void AddRow()
    {
        if (gvw1.GetFocusedDataRow() == null)
        {
            Toast.Show("단가를 등록할 품목을 먼저 선택해주세요.");
            return;
        }

        gvw2.AddNewRow();
        gvw2.FocusedColumn = colCustNm;
    }

    private void Gvw2_InitNewRow(object? sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
    {
        if (gvw1.GetFocusedDataRow() is not DataRow item) return;

        _syncing = true;
        try
        {
            if (gvw2.GetRow(e.RowHandle) is DataRowView view)
            {
                view["acc_id"] = item["acc_id"];
                view["item_id"] = item["item_id"];
                view["item_no"] = item["item_no"];
                view["item_nm"] = item["item_nm"];
                view["item_spec"] = item["item_spec"];
            }

            gvw2.SetRowCellValue(e.RowHandle, colStartDate, DateTime.Today.ToString("yyyyMMdd"));
            gvw2.SetRowCellValue(e.RowHandle, colCurCd, "KRW");

            var unit = Cell(item, "unit_cd");
            if (unit.Length > 0) gvw2.SetRowCellValue(e.RowHandle, colUnitCd, unit);
        }
        finally
        {
            _syncing = false;
        }
    }

    private void DeleteSelectedRows()
    {
        gvw2.CloseEditor();
        gvw2.UpdateCurrentRow();

        var handles = gvw2.GetSelectedCells().Select(c => c.RowHandle).Where(h => h >= 0).Distinct().ToList();
        if (handles.Count == 0 && gvw2.FocusedRowHandle >= 0) handles.Add(gvw2.FocusedRowHandle);
        if (handles.Count == 0)
        {
            Toast.Show("삭제할 행을 먼저 선택해주세요.");
            return;
        }

        try
        {
            foreach (var handle in handles.OrderByDescending(h => h))
                gvw2.GetDataRow(handle)?.Delete();
        }
        catch (Exception ex)
        {
            AppMessageBox.Show(ex.Message, "삭제 실패");
        }
    }

    private void ReviseSelectedRow()
    {
        gvw2.CloseEditor();
        gvw2.UpdateCurrentRow();

        if (gvw2.GetFocusedDataRow() is not DataRow row)
        {
            AppMessageBox.Show("개정할 단가 행을 먼저 선택해주세요.", "단가개정");
            return;
        }

        var itemId = Cell(row, "item_id");
        var oldStart = Cell(row, "start_date");
        if (itemId.Length == 0 || oldStart.Length != 8)
        {
            AppMessageBox.Show("적용시작일이 입력된 단가만 개정할 수 있습니다.", "단가개정");
            return;
        }
        var oldEnd = Cell(row, "end_date");
        if (oldEnd.Length == 0) oldEnd = OpenEnd;

        var input = XtraInputBox.Show(
            $"새 단가의 적용시작일을 입력하세요. (예: {DateTime.Today:yyyyMMdd})\n이전 단가({oldStart} ~ {(oldEnd == OpenEnd ? "무기한" : oldEnd)})는 시작일 전날로 종료 처리됩니다.",
            "단가개정", DateTime.Today.ToString("yyyyMMdd"));
        if (string.IsNullOrWhiteSpace(input)) return;

        var newStart = NormalizeYmd(input);
        if (newStart == null)
        {
            AppMessageBox.Show("날짜 형식이 올바르지 않습니다. yyyyMMdd로 입력해주세요.", "단가개정");
            return;
        }
        if (string.CompareOrdinal(newStart, oldStart) <= 0)
        {
            AppMessageBox.Show($"새 시작일은 이전 단가의 시작일({oldStart})보다 늦어야 합니다.", "단가개정");
            return;
        }

        try
        {
            if (string.CompareOrdinal(oldEnd, newStart) >= 0)
                row["end_date"] = DateTime.ParseExact(newStart, "yyyyMMdd", CultureInfo.InvariantCulture).AddDays(-1).ToString("yyyyMMdd");

            var next = _prices.NewRow();
            foreach (var col in new[] { "acc_id", "item_id", "item_no", "item_nm", "item_spec", "cust_id", "cust_nm", "cur_cd", "unit_cd", "price" })
                next[col] = row[col];
            next["start_date"] = newStart;
            next["end_date"] = DBNull.Value;
            _prices.Rows.Add(next);

            gvw2.FocusedRowHandle = gvw2.GetRowHandle(_prices.Rows.IndexOf(next));
            gvw2.FocusedColumn = colPrice;
            gvw2.ShowEditor();
        }
        catch (Exception ex)
        {
            AppMessageBox.Show(ex.Message, "단가개정 실패");
        }
    }

    // ==================== 셀 편집 ====================

    private void Gvw2_CellValueChanged(object? sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
    {
        if (_syncing) return;

        _syncing = true;
        try
        {
            if (e.Column == colCustNm) ResolveCust(e.RowHandle, e.Value?.ToString());
            else if (e.Column == colStartDate || e.Column == colEndDate) NormalizeDateCell(e.RowHandle, e.Column, e.Value);
            else if (e.Column == colPrice) NormalizePriceCell(e.RowHandle, e.Value);
        }
        finally
        {
            _syncing = false;
        }
    }

    private void ResolveCust(int rowHandle, string? typed)
    {
        if (string.IsNullOrWhiteSpace(typed))
        {
            gvw2.SetRowCellValue(rowHandle, colCustId, null);
            gvw2.SetRowCellValue(rowHandle, colCustNm, null);
            return;
        }

        var cust = FindRow(_custCache, "cust_id", "cust_nm", typed);
        if (cust != null)
        {
            gvw2.SetRowCellValue(rowHandle, colCustId, cust["cust_id"]);
            gvw2.SetRowCellValue(rowHandle, colCustNm, cust["cust_nm"]);
            return;
        }

        if (_custCache.Rows.Count == 0) return;

        var previousId = Convert.ToString(gvw2.GetRowCellValue(rowHandle, colCustId));
        var previous = string.IsNullOrEmpty(previousId) ? null : FindRow(_custCache, "cust_id", null, previousId);
        gvw2.SetRowCellValue(rowHandle, colCustNm, previous?["cust_nm"]);
        Toast.Show($"등록되지 않은 거래처입니다: {typed}");
    }

    private void NormalizeDateCell(int rowHandle, DevExpress.XtraGrid.Columns.GridColumn column, object? value)
    {
        var text = value is DateTime dt ? dt.ToString("yyyyMMdd") : Convert.ToString(value)?.Trim();
        if (string.IsNullOrEmpty(text)) return;

        var normalized = NormalizeYmd(text);
        if (value is string && normalized == text) return;

        gvw2.SetRowCellValue(rowHandle, column, normalized);
        if (normalized == null) Toast.Show($"날짜 형식이 올바르지 않아 비웠습니다: {text}");
    }

    private void NormalizePriceCell(int rowHandle, object? value)
    {
        if (value is not string text) return;

        if (text.Trim().Length == 0)
        {
            gvw2.SetRowCellValue(rowHandle, colPrice, null);
            return;
        }

        if (decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out var number)
            || decimal.TryParse(text, NumberStyles.Number, CultureInfo.CurrentCulture, out number))
        {
            gvw2.SetRowCellValue(rowHandle, colPrice, number);
            return;
        }

        gvw2.SetRowCellValue(rowHandle, colPrice, null);
        Toast.Show($"단가는 숫자로 입력해주세요: {text}");
    }

    // ==================== 표시 ====================

    private void Gvw1_CustomColumnDisplayText(object? sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
    {
        if (e.Column == colMLastEndDate && Convert.ToString(e.Value) == OpenEnd) e.DisplayText = "무기한";
    }

    private void Gvw2_CustomColumnDisplayText(object? sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
    {
        if (e.Column == colEndDate)
        {
            var end = Convert.ToString(e.Value);
            if (string.IsNullOrEmpty(end) || end == OpenEnd) e.DisplayText = "무기한";
        }
        else if (e.Column == colCustNm)
        {
            if (string.IsNullOrEmpty(Convert.ToString(e.Value))) e.DisplayText = "(전체 거래처)";
        }
        else if (e.Column == colStatNm && e.ListSourceRowIndex >= 0)
        {
            var handle = gvw2.GetRowHandle(e.ListSourceRowIndex);
            e.DisplayText = StatusOf(
                Convert.ToString(gvw2.GetRowCellValue(handle, colStartDate)),
                Convert.ToString(gvw2.GetRowCellValue(handle, colEndDate)));
        }
    }

    private void Gvw2_RowCellStyle(object? sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
    {
        if (e.RowHandle < 0) return;

        var status = StatusOf(
            Convert.ToString(gvw2.GetRowCellValue(e.RowHandle, colStartDate)),
            Convert.ToString(gvw2.GetRowCellValue(e.RowHandle, colEndDate)));

        if (status == "만료")
        {
            e.Appearance.ForeColor = Color.FromArgb(150, 150, 150);
            e.Appearance.Options.UseForeColor = true;
        }
        else if (status == "예정")
        {
            e.Appearance.ForeColor = Color.FromArgb(37, 99, 235);
            e.Appearance.Options.UseForeColor = true;
        }
    }

    private static string StatusOf(string? start, string? end)
    {
        if (string.IsNullOrEmpty(start) || start.Length != 8) return string.Empty;
        var today = DateTime.Today.ToString("yyyyMMdd");
        if (!string.IsNullOrEmpty(end) && end.Length == 8 && string.CompareOrdinal(end, today) < 0) return "만료";
        if (string.CompareOrdinal(start, today) > 0) return "예정";
        return "적용중";
    }

    // ==================== 저장 ====================

    public override async Task SaveClick()
    {
        gvw2.CloseEditor();
        if (!gvw2.UpdateCurrentRow()) return;

        var error = ValidateRows();
        if (error != null)
        {
            AppMessageBox.Show(error, "저장 확인");
            return;
        }

        var changed = _prices.Rows.Cast<DataRow>()
            .Where(r => r.RowState is DataRowState.Added or DataRowState.Modified or DataRowState.Deleted)
            .ToList();
        if (changed.Count == 0)
        {
            Toast.Show("저장할 변경 내용이 없습니다.");
            return;
        }

        var focusedItemId = _editingItemId;

        foreach (var row in changed.OrderBy(r => r.RowState == DataRowState.Deleted ? 0 : r.RowState == DataRowState.Modified ? 1 : 2))
        {
            var state = row.RowState;
            var version = state == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;

            var p = new Dictionary<string, string?>
            {
                ["p_work_type"] = state == DataRowState.Deleted ? "D" : state == DataRowState.Added ? "N" : "U",
                ["p_price_id"] = ProcData.Str(row, "price_id", version),
                ["p_acc_id"] = ProcData.Str(row, "acc_id", version) ?? NullIfEmpty(cboAccId.EditValue?.ToString()),
                ["p_item_id"] = ProcData.Str(row, "item_id", version),
                ["p_cust_id"] = NullIfEmpty(ProcData.Str(row, "cust_id", version)),
                ["p_start_date"] = ProcData.Str(row, "start_date", version),
                ["p_end_date"] = NullIfEmpty(ProcData.Str(row, "end_date", version)),
                ["p_cur_cd"] = ProcData.Str(row, "cur_cd", version),
                ["p_unit_cd"] = ProcData.Str(row, "unit_cd", version),
                ["p_price"] = ProcData.Str(row, "price", version),
                ["p_remark"] = ProcData.Str(row, "remark", version),
            };

            var result = await SaveAsync("USP_SA_PRICE_S", p);
            if (result == null || !result.Success)
            {
                var label = ProcData.Str(row, "item_no", version) ?? "?";
                AppMessageBox.Show($"[{label}] {result?.Message ?? "저장에 실패했습니다."}", "저장 실패");
                return;
            }

            if (state == DataRowState.Added && result.GeneratedCode != null) row["price_id"] = result.GeneratedCode;
            row.AcceptChanges();
        }

        Toast.Show("저장되었습니다.");
        await QueryCore(focusedItemId);
    }

    private string? ValidateRows()
    {
        var problems = new List<string>();

        foreach (DataRow row in _prices.Rows)
        {
            if (row.RowState is not (DataRowState.Added or DataRowState.Modified)) continue;

            var line = LineNo(row);
            var start = Cell(row, "start_date");
            var end = Cell(row, "end_date");

            if (Cell(row, "item_id").Length == 0) problems.Add($"{line}행: 품목이 지정되지 않았습니다. 품목을 선택한 뒤 행을 추가해주세요.");
            if (NormalizeYmd(start) == null) problems.Add($"{line}행: 적용시작일이 올바르지 않습니다.");
            else if (end.Length > 0 && NormalizeYmd(end) == null) problems.Add($"{line}행: 적용종료일이 올바르지 않습니다.");
            else if (end.Length > 0 && string.CompareOrdinal(start, end) > 0) problems.Add($"{line}행: 적용시작일이 종료일보다 늦습니다.");

            var priceText = Cell(row, "price");
            if (priceText.Length == 0 || !decimal.TryParse(priceText, NumberStyles.Number, CultureInfo.InvariantCulture, out var price) || price < 0)
                problems.Add($"{line}행: 단가를 0 이상의 숫자로 입력해주세요.");
        }

        if (problems.Count == 0)
        {
            var groups = _prices.Rows.Cast<DataRow>()
                .Where(r => r.RowState != DataRowState.Deleted && Cell(r, "item_id").Length > 0 && Cell(r, "start_date").Length == 8)
                .GroupBy(r => (Cell(r, "item_id"), Cell(r, "cust_id")));

            foreach (var group in groups)
            {
                DataRow? holder = null;
                var maxEnd = string.Empty;
                foreach (var row in group.OrderBy(r => Cell(r, "start_date"), StringComparer.Ordinal))
                {
                    var start = Cell(row, "start_date");
                    var end = Cell(row, "end_date");
                    if (end.Length == 0) end = OpenEnd;

                    if (holder != null && string.CompareOrdinal(start, maxEnd) <= 0)
                        problems.Add($"{LineNo(holder)}행과 {LineNo(row)}행: 같은 거래처의 적용기간이 겹칩니다.");

                    if (holder == null || string.CompareOrdinal(end, maxEnd) > 0)
                    {
                        maxEnd = end;
                        holder = row;
                    }
                }
            }
        }

        if (problems.Count == 0) return null;

        const int max = 10;
        return string.Join("\n", problems.Take(max)) + (problems.Count > max ? $"\n... 외 {problems.Count - max}건" : string.Empty);
    }

    private int LineNo(DataRow row)
    {
        var index = _prices.Rows.IndexOf(row);
        var handle = index >= 0 ? gvw2.GetRowHandle(index) : -1;
        var visible = handle >= 0 ? gvw2.GetVisibleIndex(handle) : -1;
        return visible >= 0 ? visible + 1 : index + 1;
    }

    // ==================== 공통 ====================

    private static string? NullIfEmpty(string? value) => string.IsNullOrWhiteSpace(value) ? null : value;

    private static string Cell(DataRow row, string column) =>
        !row.Table.Columns.Contains(column) || row[column] == DBNull.Value ? string.Empty
        : row[column] is DateTime d ? d.ToString("yyyyMMdd")
        : Convert.ToString(row[column], CultureInfo.InvariantCulture) ?? string.Empty;

    private static string? NormalizeYmd(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;

        var digits = new string(text.Where(char.IsDigit).ToArray());
        if (digits.Length != 8) return null;

        return DateTime.TryParseExact(digits, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _) ? digits : null;
    }

    private static DataRow? FindRow(DataTable cache, string idColumn, string? textColumn, string? typed)
    {
        if (string.IsNullOrWhiteSpace(typed)) return null;
        typed = typed.Trim();

        if (long.TryParse(typed, out var id))
        {
            foreach (DataRow r in cache.Rows)
                if (r[idColumn] != DBNull.Value && Convert.ToInt64(r[idColumn]) == id) return r;
        }

        if (textColumn == null) return null;
        foreach (DataRow r in cache.Rows)
            if (string.Equals(Convert.ToString(r[textColumn]), typed, StringComparison.OrdinalIgnoreCase)) return r;

        return null;
    }
}
