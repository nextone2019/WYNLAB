using System.Data;
using System.Drawing;
using System.Globalization;
using DevExpress.XtraEditors;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

/// <summary>
/// 구매단가등록 - 품목 마스터 중심 구조(2026-09-26 변경).
///  - grd1(왼쪽): 품목마스터(TBAITEM) 목록 + 품목별 "최종단가"(USP_MA_POPRICE_Q 'Q'). 최종단가 = 오늘 이전에 시작한
///    단가 중 시작일이 가장 늦은 1건(거래처 무관) - 서버가 OUTER APPLY로 붙여서 준다.
///  - panData(오른쪽 위): grd1에서 고른 품목의 정보(읽기 전용).
///  - grd2(오른쪽 아래): 그 품목의 구매단가 목록(USP_MA_POPRICE_Q 'Q2') - 여기서 바로 추가/수정/삭제/개정하고 저장한다.
///
/// 적용 규칙: 거래처가 비어 있으면 "전체 거래처 공통 단가", 채워져 있으면 그 거래처에만 적용되는 단가.
/// 종료일이 비어 있으면 무기한(서버에는 '99991231'). 같은 품목 + 같은 거래처 범위 안에서 기간이 겹치는
/// 단가는 저장할 수 없다(서버 USP_MA_POPRICE_S가 최종 검증, 화면도 저장 전에 미리 검사).
///
/// 편의 기능:
///  - 거래처는 팝업으로 고르거나 직접 타이핑/엑셀 붙여넣기 모두 가능(등록된 이름만 인정 - 오타로 거래처가 비면
///    "전체 거래처"로 바뀌어버리는 사고를 막으려고 못 찾으면 원래 값으로 되돌린다).
///  - [단가개정] - 선택한 단가를 새 시작일로 개정: 이전 단가의 종료일을 전날로 자동 정리하고 같은 내용의
///    새 행을 만들어 단가 칸으로 바로 이동한다(가격 인상 때마다 종료일/새 행을 손으로 맞추는 실수를 없앰).
///  - 상태(적용중/예정/만료)를 오늘 기준으로 계산해 보여주고, 만료 행은 회색/예정 행은 파란색 글자로 구분.
///  - 저장 전에 필수값/기간 역전/같은 화면 안의 기간 겹침을 한꺼번에 검사한다.
///  - 저장은 삭제 -> 수정 -> 추가 순서(개정처럼 기존 단가 종료일을 줄이고 새 단가를 넣는 조합이 서버
///    겹침 검사에 걸리지 않게). 중간에 실패해도 이미 저장된 행은 다시 저장되지 않는다.
///  - 저장 후에는 품목 목록도 다시 조회해서 최종단가가 바로 갱신되고, 방금 편집하던 품목에 포커스가 남는다.
/// </summary>
public partial class frmPoPrice : BaseForm, WYNLAB.Popup.IFeatureHost
{
    private const string OpenEnd = "99991231";

    private DataTable _items = new();
    private DataTable _prices = BuildEmptyTable();
    private DataTable _custCache = new();
    private string? _editingItemId;
    private bool _syncing;

    public frmPoPrice()
    {
        InitializeComponent();

        Text = "구매단가등록";

        cboAccId.EditValue = Session.AccId?.ToString();
        cboPriceYn.SelectedIndex = 0;

        // grd1: 품목 목록(조회 전용). 다른 품목으로 옮길 때 저장 안 된 단가 변경이 있으면 먼저 확인한다
        // (BaseForm.ConfirmMasterRowSwitch - 이름 있는 메서드로 등록해야 재조회 중 잠깐 구독을 끊을 수 있다).
        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;
        gvw1.CustomColumnDisplayText += Gvw1_CustomColumnDisplayText;
        colMLastPrice.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

        // grd2: 선택한 품목의 단가(편집).
        gvw2.Role = GridRoleWyn.Edit;
        gvw2.HighlightFocusedRow = true;
        gvw2.HighlightUnsavedCells = true;
        colPrice.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

        // 행추가는 그리드 자체의 새 행(AddNewRow)으로 - 우클릭 메뉴의 행추가/행복사/멀티행추가도 이 델리게이트를
        // 그대로 탄다(GridViewWynBehavior.AddRowAfterFocused 참고).
        gvw2.RowAdd += (s, e) => AddRow();
        gvw2.RowDelete += (s, e) => DeleteSelectedRows();
        gvw2.InitNewRow += Gvw2_InitNewRow;
        gvw2.CellValueChanged += Gvw2_CellValueChanged;
        gvw2.CustomColumnDisplayText += Gvw2_CustomColumnDisplayText;
        gvw2.RowCellStyle += Gvw2_RowCellStyle;
        gvw2.ShowingEditor += Gvw2_ShowingEditor;
        gvw2.FocusedRowChanged += (s, e) => featBar.UpdateState();   // 결재 상태 표시는 선택한 단가 행 기준
        featBar.FeaturesLoaded += (s, e) => featBar.UpdateState();

        btnAddRow1.Click += (s, e) => AddRow();
        btnDeletRow1.Click += (s, e) => DeleteSelectedRows();
        btnRevise.Click += (s, e) => ReviseSelectedRow();

        // 개발자용 마우스오버 툴팁(BindingField) - grid 컬럼은 FieldName으로 자동, panData는 Tag에 미리 넣어둔다.
        cboDetailAccCd.Tag = new BindingFieldTag("acc_id");
        txtDetailItemNo.Tag = new BindingFieldTag("item_no");
        txtDetailItemNm.Tag = new BindingFieldTag("item_nm");
        txtDetailItemSpec.Tag = new BindingFieldTag("item_spec");
        txtDetailCustNm.Tag = new BindingFieldTag("cust_nm");
        cboDetailAssetType.Tag = new BindingFieldTag("asset_type");
        cboDetailStatCd.Tag = new BindingFieldTag("stat_cd");
        //cboDetailUnitCd.Tag = new BindingFieldTag("unit_cd");
        cboDetailPoUnitCd.Tag = new BindingFieldTag("po_unit_cd");
        //txtDetailRemark.Tag = new BindingFieldTag("remark");

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
            "start_date", "end_date", "cur_cd", "unit_cd", "price", "remark", "stat_nm", "app_id", "app_no", "appr_stat_cd"
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

    /// <summary>restoreItemId가 있으면(저장 직후 재조회) 조회 후 그 품목으로 포커스를 되돌린다 - 저장했더니 화면이
    /// 맨 위 품목으로 튀어버리면 방금 고친 품목을 다시 찾아야 해서 불편하다.</summary>
    private async Task QueryCore(string? restoreItemId)
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_acc_id"] = NullIfEmpty(cboAccId.EditValue?.ToString()),
            ["p_item_kw"] = NullIfEmpty(txtKeyword.Text.Trim()),
            ["p_price_yn"] = cboPriceYn.SelectedIndex switch { 1 => "Y", 2 => "N", _ => null },
        };

        var table = await QueryAsync("USP_MA_POPRICE_Q", p);

        // 재바인딩 구간엔 행 전환 이벤트를 끊고, 아래에서 최종 행에 대해 직접 한 번만 채운다(frmEmp와 같은 이유 -
        // 이벤트로도 한 번, 아래에서도 한 번 불리면 두 호출이 겹쳐 나중에 끝난 쪽이 먼저 끝난 쪽을 덮어쓴다).
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

    /// <summary>선택한 품목의 정보를 panData에 채우고, 그 품목의 단가 목록(grd2)을 조회한다(SUB 그리드는 마스터가
    /// 바뀔 때마다 새로 채운다).</summary>
    private async Task LoadItemAsync(DataRow item)
    {
        var itemId = Cell(item, "item_id");
        _editingItemId = itemId;

        cboDetailAccCd.EditValue = Cell(item, "acc_id");
        txtDetailItemNo.Text = Cell(item, "item_no");
        txtDetailItemNm.Text = Cell(item, "item_nm");
        txtDetailItemSpec.Text = Cell(item, "item_spec");
        txtDetailCustNm.Text = Cell(item, "cust_nm");
        cboDetailAssetType.EditValue = Cell(item, "asset_type");
        cboDetailStatCd.EditValue = Cell(item, "stat_cd");
        //cboDetailUnitCd.EditValue = Cell(item, "unit_cd");
        cboDetailPoUnitCd.EditValue = Cell(item, "po_unit_cd");
       // txtDetailRemark.Text = Cell(item, "remark");

        var prices = await QueryAsync("USP_MA_POPRICE_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q2",
            ["p_acc_id"] = NullIfEmpty(Cell(item, "acc_id")),
            ["p_item_id"] = itemId,
        });

        // 응답을 기다리는 사이 다른 품목으로 옮겼으면 이 결과는 버린다(늦게 온 응답이 새 품목의 단가를 덮어쓰지 않게).
        if (_editingItemId != itemId) return;

        SetPrices(prices);
    }

    private void ClearItem()
    {
        _editingItemId = null;
        foreach (var edit in new BaseEdit[] { cboDetailAccCd, txtDetailItemNo, txtDetailItemNm, txtDetailItemSpec, txtDetailCustNm,
                                              cboDetailAssetType, cboDetailStatCd, /*cboDetailUnitCd,*/ cboDetailPoUnitCd/*, txtDetailRemark*/ })
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

    /// <summary>새 행 기본값 - 선택한 품목/사업장, 시작일은 오늘, 통화는 원화, 단위는 그 품목의 구매단위(없으면 재고단위).</summary>
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

            var unit = Cell(item, "po_unit_cd");
            if (unit.Length == 0) unit = Cell(item, "unit_cd");
            if (unit.Length > 0) gvw2.SetRowCellValue(e.RowHandle, colUnitCd, unit);
        }
        finally
        {
            _syncing = false;
        }
    }

    /// <summary>선택된 셀들이 걸친 행(없으면 포커스 행)을 삭제 표시한다 - 저장 전까지는 화면에서만 지워지고,
    /// 저장해야 서버에 반영된다(다른 편집 그리드와 같은 방식).</summary>
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
            // 핸들이 큰 것(=아래쪽 행)부터 지워야 앞쪽 행 핸들이 밀리지 않는다.
            foreach (var handle in handles.OrderByDescending(h => h))
            {
                var row = gvw2.GetDataRow(handle);
                if (row != null && IsRowLocked(row))
                {
                    Toast.Show("결재 상신된 단가는 삭제할 수 없습니다.");
                    continue;
                }
                row?.Delete();
            }
        }
        catch (Exception ex)
        {
            AppMessageBox.Show(ex.Message, "삭제 실패");
        }
    }

    /// <summary>선택한 단가를 새 시작일로 개정한다: ① 이전 단가의 종료일을 새 시작일 전날로 줄이고(이미 그 전에
    /// 끝난 단가면 건드리지 않음) ② 같은 품목/거래처/단위/통화의 새 행(종료일 무기한, 단가는 이전 값 그대로 -
    /// 바로 고치도록 단가 칸으로 이동)을 만든다. 저장은 사용자가 [저장]을 눌러야 반영된다.</summary>
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
            // 이전 단가가 새 시작일 이후까지 이어질 때만 전날로 줄인다 - 이미 그 전에 끝난 단가는 그대로 둔다.
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

    /// <summary>거래처 셀 - 비우면 "전체 거래처 공통 단가"가 된다. 그래서 오타로 못 찾은 경우를 비운 것과 같게
    /// 처리하면 안 된다(공통 단가로 조용히 바뀌어 저장되는 사고) - 못 찾으면 원래 거래처로 되돌리고 알려준다.</summary>
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

        // 못 찾음 - 이 행의 기존 거래처(cust_id 기준)로 되돌린다. 캐시를 아직 못 받은 상태면(빈 캐시) 그대로 둔다.
        if (_custCache.Rows.Count == 0) return;

        var previousId = Convert.ToString(gvw2.GetRowCellValue(rowHandle, colCustId));
        var previous = string.IsNullOrEmpty(previousId) ? null : FindRow(_custCache, "cust_id", null, previousId);
        gvw2.SetRowCellValue(rowHandle, colCustNm, previous?["cust_nm"]);
        Toast.Show($"등록되지 않은 거래처입니다: {typed}");
    }

    /// <summary>엑셀 붙여넣기는 에디터를 거치지 않아 "2026-01-01" 같은 값이 그대로 들어온다 - yyyyMMdd로 맞추고,
    /// 날짜가 아니면 비운다(종료일은 빈 값이 무기한이라 조용히 넘어가지 않게 알려준다).</summary>
    private void NormalizeDateCell(int rowHandle, DevExpress.XtraGrid.Columns.GridColumn column, object? value)
    {
        var text = value is DateTime dt ? dt.ToString("yyyyMMdd") : Convert.ToString(value)?.Trim();
        if (string.IsNullOrEmpty(text)) return;

        var normalized = NormalizeYmd(text);
        // 날짜 선택기는 DateTime을 셀에 넣는다 - 글자가 같아 보여도(20260927) 셀 값이 문자열 yyyyMMdd가 되도록 다시 써야 한다.
        if (value is string && normalized == text) return;

        gvw2.SetRowCellValue(rowHandle, column, normalized);
        if (normalized == null) Toast.Show($"날짜 형식이 올바르지 않아 비웠습니다: {text}");
    }

    /// <summary>단가 칸에 문자열이 들어오면(직접 타이핑/붙여넣기) 숫자로 바꿔 넣는다 - 그래야 천단위 표시 서식이
    /// 적용되고 저장 때 숫자로 읽힌다. 숫자가 아니면 비운다.</summary>
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

    /// <summary>품목 목록의 최종단가 적용종료일 - 서버 값 99991231은 "무기한"으로 보여준다.</summary>
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

    /// <summary>만료된 단가는 회색, 아직 시작 전인 단가는 파란색 글자로 - 지금 실제로 쓰이는(적용중) 행이
    /// 한눈에 들어오게 한다. 서버 stat_nm이 아니라 화면에서 날짜로 계산해서(StatusOf) 편집 중에도 바로
    /// 바뀐다.</summary>
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

    /// <summary>오늘 기준 상태 - 서버 USP_MA_POPRICE_Q의 stat_nm과 같은 규칙.</summary>
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

        // 삭제 -> 수정 -> 추가 순서(클래스 설명 참고).
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

            var result = await SaveAsync("USP_MA_POPRICE_S", p);
            if (result == null || !result.Success)
            {
                var label = ProcData.Str(row, "item_no", version) ?? "?";
                AppMessageBox.Show($"[{label}] {result?.Message ?? "저장에 실패했습니다."}", "저장 실패");
                return;
            }

            // 저장에 성공한 행은 확정 처리한다 - 뒤 행에서 실패해 사용자가 고치고 다시 저장할 때 이 행이
            // 또 서버로 가면(특히 추가 행은 중복 등록/기간 겹침 오류) 안 된다.
            if (state == DataRowState.Added && result.GeneratedCode != null) row["price_id"] = result.GeneratedCode;
            row.AcceptChanges();
        }

        Toast.Show("저장되었습니다.");
        await QueryCore(focusedItemId); // 품목 목록의 최종단가도 같이 갱신
    }

    /// <summary>저장 전 검사 - 바뀐 행의 필수값/기간 역전, 그리고 화면에 있는 전체 행에서 같은 거래처
    /// 범위의 기간 겹침을 한 번에 찾아 한 메시지로 알려준다(서버 검증은 행마다 하나씩 실패해서 하나 고치면 또
    /// 다음이 나오는 식이라 불편하다). 서버가 최종 검증이므로 여기서 놓쳐도 저장은 막힌다.</summary>
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
                // 시작일 순으로 훑으면서 지금까지 가장 늦게 끝나는 행(holder)과 겹치는지만 보면 된다.
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

    // ==================== 전자결재/첨부(FeatureBarWyn) - 결재 단위 = 단가 한 줄(doc_id = price_id) ====================

    /// <summary>결재 상신된(진행/승인완료) 단가 행 - 결재를 쓰는 메뉴에서만 잠긴다. 반려(R)나 상신 전은 수정 가능.</summary>
    private bool IsRowLocked(DataRow row) => featBar.ApprovalEnabled && Cell(row, "appr_stat_cd") is "0" or "1" or "E";

    /// <summary>잠긴 행은 적용종료일/비고만 고칠 수 있다(단가개정이 이전 단가의 종료일을 줄이기 때문 - 서버도 같은 기준).</summary>
    private void Gvw2_ShowingEditor(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (gvw2.GetFocusedDataRow() is DataRow row && IsRowLocked(row) && gvw2.FocusedColumn != colEndDate && gvw2.FocusedColumn != colRemark)
            e.Cancel = true;
    }

    public WYNLAB.Popup.FeatureContext GetFeatureContext()
    {
        var row = gvw2.GetFocusedDataRow();
        var priceId = row == null ? string.Empty : Cell(row, "price_id");
        return new WYNLAB.Popup.FeatureContext
        {
            DocId = long.TryParse(priceId, out var id) ? id : null,
            DocNo = priceId,
            Title = row == null ? string.Empty : $"구매단가 {Cell(row, "item_no")} {Cell(row, "item_nm")} - {Cell(row, "price")} {Cell(row, "cur_cd")}/{Cell(row, "unit_cd")}",
            Text = row == null ? string.Empty : $"적용기간 {Cell(row, "start_date")} ~ {(Cell(row, "end_date") is { Length: > 0 } and not OpenEnd ? Cell(row, "end_date") : "무기한")}" + (Cell(row, "cust_nm").Length > 0 ? $" / 거래처 {Cell(row, "cust_nm")}" : " / 전체 거래처"),
            HasUnsavedChanges = HasUnsavedChanges,
            AppNo = row == null ? null : Cell(row, "app_no"),
            ApprStatCd = row == null ? null : Cell(row, "appr_stat_cd"),
        };
    }

    public void OnFeatureChanged(string featureCd)
    {
        // 결재 상신/반려로 연결 상태가 바뀌었으니 선택한 품목의 단가를 다시 읽는다(열기 전에 미저장 변경이 없음을 확인했다).
        if (featureCd == "APPROVAL" && gvw1.GetFocusedDataRow() is DataRow item) _ = SafeExecuteAsync(() => LoadItemAsync(item), "단가 재조회");
    }

    // ==================== 공통 ====================
    private static string? NullIfEmpty(string? value) => string.IsNullOrWhiteSpace(value) ? null : value;

    private static string Cell(DataRow row, string column) =>
        !row.Table.Columns.Contains(column) || row[column] == DBNull.Value ? string.Empty
        : row[column] is DateTime d ? d.ToString("yyyyMMdd")
        : Convert.ToString(row[column], CultureInfo.InvariantCulture) ?? string.Empty;

    /// <summary>"2026-01-01"/"2026.1.1"/"20260101" 어느 형식이든 실제 존재하는 날짜면 yyyyMMdd로, 아니면 null.</summary>
    private static string? NormalizeYmd(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;

        var digits = new string(text.Where(char.IsDigit).ToArray());
        if (digits.Length != 8) return null;

        return DateTime.TryParseExact(digits, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _) ? digits : null;
    }

    /// <summary>캐시에서 행 찾기 - 먼저 ID(숫자)로, 못 찾으면 텍스트 컬럼으로(대소문자 무시). 팝업 선택은 ID가,
    /// 직접 타이핑/붙여넣기는 이름이 들어오므로 둘 다 받는다(frmPoReq의 품번 처리와 같은 규칙).</summary>
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
