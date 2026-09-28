using DevExpress.XtraGrid;
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Popup;

namespace WYNLAB.MA;

/// <summary>
/// 구매요청등록 - AI Builder Master-One Sheet 유형(grd1 없이 panHeader의 요청번호로 문서 1건을
/// 바로 조회/편집). 전자결재는 명함신청서(frmNameCardReq)와 완전히 같은 방식(popApp 공용 팝업,
/// doc_type="POREQ") - 결재상태는 이 화면이 직접 갱신하지 않고 USP_MA_POREQ_Q가 매 조회마다
/// TAPDOC을 JOIN해서 보여준다(Pull).
/// </summary>
public partial class frmPoReq : BaseForm
{
    private DataTable _detail = new();
    private DataTable _itemCache = new();
    private DataTable _whCache = new();
    private string? _editingKey; // null이면 신규모드

    public frmPoReq()
    {
        InitializeComponent();

        Text = "구매요청등록";

        Controls.Add(BuildScreenHeader());

        // 부서/담당자/거래처 - PopupLookupEditWyn 멀티필드 모드(frmEMP의 txtDetailDeptNm과
        // 같은 방식). 화면엔 이름만 보이고 실제 FK는 숨긴 필드(txtDeptId 등)에 채워진다.
        txtDeptNm.MapField("dept_id", txtDeptId);
        txtEmpNm.MapField("emp_id", txtEmpId);
        txtCustNm.MapField("cust_id", txtCustId);

        gvw1.Role = GridRoleWyn.Edit;
        // 금액 컬럼 합계를 그리드 하단(Footer)에 상시 표시(2026-09-26 요청) - 단가/수량/세율처럼 합계가 의미 없는 컬럼은 뺀다.
        foreach (var amountColumn in new[] { colAmt, colVat, colTotalAmt, colKorAmt, colKorVat, colKorTotalAmt })
            gvw1.AddColumnSummary(amountColumn, DevExpress.Data.SummaryItemType.Sum, "{0:#,##0.##}");
        gvw1.HighlightFocusedRow = true;
        gvw1.RowAdd += (s, e) => gvw1.AddNewRow();
        gvw1.RowDelete += (s, e) =>
        {
            try { if (gvw1.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };
        gvw1.CellValueChanged += Gvw1_CellValueChanged;

        // 품목 팝업(P_ITEM_PO)에 화면 헤더의 거래처/요청일자를 넘긴다 - 팝업은 그 거래처에 등록된 단가가 있으면 그것을, 없으면 거래처 없이
        // 등록된 공통 단가를 보여준다(SSP_POP_ITEM_PO_Q). 팝업에서 품목을 고르면 그 단가가 이 행에 바로 채워진다(PopcolItem_ResultSelected).
        popcolItem.ConditionProvider = () => new Dictionary<string, string?>
        {
            ["p_cust_id"] = string.IsNullOrWhiteSpace(txtCustId.Text) ? null : txtCustId.Text,
            ["p_base_date"] = dteReqDate.YyyyMmDd,
        };
        popcolItem.ResultSelected += PopcolItem_ResultSelected;

        // 환율/통화: 통화가 원화(KRW)면 환율은 1로 - 화면에 값을 채우는 중(_loadingHeader)에는 건드리지 않는다. 환율이 바뀌면 원화 컬럼 재계산.
        cboCurCd.EditValueChanged += (s, e) =>
        {
            if (!_loadingHeader && Equals(cboCurCd.EditValue?.ToString(), "KRW")) txtExcRate.Text = "1";
        };
        txtExcRate.EditValueChanged += (s, e) => RecalcAllKor();

        btnAddRow1.Click += (s, e) => gvw1.AddNewRow();
        btnDeletRow1.Click += (s, e) =>
        {
            try { if (gvw1.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };

        // 전자결재 - 버튼은 디자이너의 "구매요청정보" 타이틀 아래 패널(panelWyn2)에 있다(2026-09-25 위치 이동). frmNameCardReq.OpenApprovalAsync와 같은 방식.
        btnOpenApproval.Click += async (s, e) => await SafeExecuteAsync(OpenApprovalAsync, "전자결재");

        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) =>
        {
            // 화면을 열자마자 조회하지 않는다 - 검색조건 없이 QueryClick을 부르면 USP_MA_POREQ_Q가
            // "조건 없음"과 "가장 최근 건 요청"을 구분 못 해 마지막 저장된 요청서를 그대로 띄워버린다
            // (2026-09-22 실제 발견 - "화면을 처음 오픈하면 PANDATA의 구매요청번호에 값이 들어가
            // 있어"). EnterNewMode()가 이미 생성자에서 신규모드를 잡아뒀으니 사용자가 직접
            // 조회(Ctrl+Q)하거나 현황화면에서 드릴다운해오기 전까지는 그 상태 그대로 둔다.
            //
            // 신규모드로 열린 화면은 품목 그리드의 DataTable에 컬럼이 하나도 없다(_detail이 new DataTable()) - 그러면 팝업에서
            // 품목을 골라도 값을 받을 컬럼이 없어 셀 값이 조용히 버려진다(2026-09-25 실제 원인 - 팝업 선택이 그리드에
            // 반영되지 않음). 빈 요청(-1)으로 한 번 불러 컬럼 구조만 받아온다(frmPo.EnsureDetailSchemaAsync와 같은 방식).
            await EnsureDetailSchemaAsync();
            _itemCache = await LoadPopupRowsAsync("P_ITEM_PO");
            _whCache = await LoadPopupRowsAsync("P_WH"); // 창고 팝업에서 직접 고른 창고ID -> 창고명 표시용
        };
    }

    private async Task EnsureDetailSchemaAsync()
    {
        if (_detail.Columns.Count > 0) return;

        var tables = await QueryMultiAsync("USP_MA_POREQ_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_req_id"] = "-1",
        });
        if (tables.Count > 1 && _detail.Columns.Count == 0) _detail = tables[1];
        if (_editingKey == null) EnterNewMode();
    }

    // colItemNo(품번) 셀이 바뀌면(팝업 선택/직접 타이핑 둘 다) 같은 핸들러가 colItemId/colItemNo를
    // 다시 SetRowCellValue로 채워서 스스로를 재호출한다 - DataTable은 같은 값을 다시 넣어도
    // ColumnChanged를 또 발생시키므로 재귀 가드가 없으면 무한루프로 죽는다.
    private bool _syncingItemRow;

    /// <summary>품번(colItemNo) 셀이 바뀌면(팝업에서 고르든 직접 타이핑하든) 캐시에서 매칭되는
    /// 품목을 찾아 품목ID(숨겨진 colItemId)/품명/규격/단위를 같은 행에 채운다 - PopupLookupColumnEdit은
    /// 그 컬럼 하나만 채워주고 같은 행의 다른 셀까지는 안 건드리기 때문(PopupLookupColumnEdit.cs
    /// 설명 참고, panData의 MatchField/MapField와 달리 그리드는 컨트롤끼리 매핑할 대상이 없다).
    /// 팝업으로 고르면 그 컨트롤이 Text에 결과의 Code(=ITEM_ID, 숫자문자열)를 넣어주므로 먼저
    /// ID로 찾고, 숫자가 아니면(사용자가 품번을 직접 타이핑) 텍스트로 한 번 더 찾는다 - 둘 다
    /// 실패하면 없는 품번이므로 나머지 칸을 비운다. 수량이 바뀌면 발주잔량(remain_qty)도 화면에서
    /// 바로 다시 계산해서 보여준다(저장/재조회 때는 서버가 다시 계산해서 내려주므로 여기선 화면
    /// 표시용일 뿐).</summary>
    private void Gvw1_CellValueChanged(object? sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
    {
        if (_syncingItemRow) return;

        // 직접 타이핑/붙여넣기한 숫자는 문자열로 들어온다 - 그러면 천단위 표시 서식이 안 먹으므로 숫자(decimal)로 바꿔 다시 쓴다.
        if (e.Value is string text && IsNumberColumn(e.Column))
        {
            gvw1.SetRowCellValue(e.RowHandle, e.Column, decimal.TryParse(text.Replace(",", ""), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var number) ? number : (object?)null);
            return;
        }

        if (e.Column == colItemNo)
        {
            var typed = e.Value?.ToString();
            DataRow? row = null;
            if (long.TryParse(typed, out var itemId))
            {
                foreach (DataRow r in _itemCache.Rows)
                {
                    if (!Equals(Convert.ToInt64(Col(r, "item_id") ?? -1L), itemId)) continue;
                    row = r;
                    break;
                }
            }
            if (row == null && !string.IsNullOrWhiteSpace(typed))
            {
                foreach (DataRow r in _itemCache.Rows)
                {
                    if (!string.Equals(Convert.ToString(Col(r, "item_no")), typed, StringComparison.OrdinalIgnoreCase)) continue;
                    row = r;
                    break;
                }
            }

            if (row == null && !string.IsNullOrWhiteSpace(typed))
                Toast.Show($"등록되지 않은 품번입니다: {typed}");

            _syncingItemRow = true;
            try
            {
                gvw1.SetRowCellValue(e.RowHandle, colItemId, Col(row, "item_id"));
                gvw1.SetRowCellValue(e.RowHandle, colItemNo, Col(row, "item_no"));
                gvw1.SetRowCellValue(e.RowHandle, colItemNm, Col(row, "item_nm"));
                gvw1.SetRowCellValue(e.RowHandle, colItemSpec, Col(row, "item_spec"));
                // 단위는 구매단가에 등록된 단위(price_unit_cd)를 우선한다 - 그 단가는 그 단위 기준이다.
                var priceUnit = Col(row, "price_unit_cd");
                gvw1.SetRowCellValue(e.RowHandle, colUnitCd, priceUnit ?? Col(row, "unit_cd"));

                // 대표단가가 특정 거래처 단가이고 이 행의 거래처가 비어 있으면 그 거래처를 채운다(공통 단가면 건드리지 않음).
                // 거래처/창고 셀은 팝업 컬럼이라 SetRowCellValue로 값을 넣으면 "값이 바뀌었으니 팝업으로 확인"하며 팝업이 저절로 뜬다
                // (GridViewWynBehavior.OnCellValueChanged) - DataRow에 직접 써서 그 경로를 피한다.
                var lineRow = gvw1.GetDataRow(e.RowHandle);

                // 대표단가가 특정 거래처 단가이고 이 행의 거래처가 비어 있으면 그 거래처를 채운다(공통 단가면 건드리지 않음).
                var custId = Col(row, "cust_id");
                if (lineRow != null && custId != null && string.IsNullOrEmpty(Convert.ToString(lineRow["cust_id"])))
                    lineRow["cust_id"] = custId;

                // 품목의 기본 창고(팝업 결과의 wh_id/wh_nm) - 품목을 새로 고르면 그 품목의 창고로 바꾼다.
                if (lineRow != null)
                {
                    lineRow["wh_id"] = Col(row, "wh_id") ?? DBNull.Value;
                    lineRow["wh_nm"] = Col(row, "wh_nm") ?? DBNull.Value;
                }

                // 부가세율은 비어 있을 때만 기본값(발주등록 헤더의 기본 부가세율과 같은 10%).
                if (row != null && gvw1.GetRowCellValue(e.RowHandle, colVatRate) is null or DBNull)
                    gvw1.SetRowCellValue(e.RowHandle, colVatRate, DefaultVatRate);
            }
            finally
            {
                _syncingItemRow = false;
            }

            // 구매단가(TMAPOPRICE)에서 이 품목/거래처/요청일에 적용되는 단가를 채운다(이미 입력된 단가는 건드리지 않음).
            if (row != null && gvw1.GetDataRow(e.RowHandle) is { } dataRow)
            {
                var handle = e.RowHandle;
                _ = SafeExecuteAsync(() => ApplyPriceAsync(dataRow, handle), "구매단가 조회");
            }
        }
        else if (e.Column == colWhId)
        {
            // 창고를 직접 바꾸면(창고 팝업) 창고명도 같이 - 팝업 결과는 창고ID만 셀에 들어온다.
            _syncingItemRow = true;
            try { gvw1.SetRowCellValue(e.RowHandle, colWhNm, WhName(e.Value)); }
            finally { _syncingItemRow = false; }
        }
        else if (e.Column == colQty || e.Column == colNextQty)
        {
            var qty = ToDecimal(gvw1.GetRowCellValue(e.RowHandle, colQty));
            var nextQty = ToDecimal(gvw1.GetRowCellValue(e.RowHandle, colNextQty));
            gvw1.SetRowCellValue(e.RowHandle, colRemainQty, qty - nextQty);
        }

        if (e.Column == colQty || e.Column == colPrice || e.Column == colVatRate)
            RecalcRow(e.RowHandle);
    }

    private bool IsNumberColumn(DevExpress.XtraGrid.Columns.GridColumn column) =>
        column == colQty || column == colPrice || column == colVatRate ||
        column == colKorAmt || column == colKorVat || column == colKorTotalAmt;

    private const string DefaultVatRate = "10";

    /// <summary>공급가액=수량*단가, 부가세=공급가액*세율/100, 합계=공급가액+부가세(발주등록 RecalcRow와 같은 계산, 원화환산은 없음).</summary>
    private void RecalcRow(int rowHandle)
    {
        var price = ToDecimal(gvw1.GetRowCellValue(rowHandle, colPrice));
        var (amt, vat, total) = CalcAmounts(
            ToDecimal(gvw1.GetRowCellValue(rowHandle, colQty)),
            price,
            ToDecimal(gvw1.GetRowCellValue(rowHandle, colVatRate)));
        var exc = ExcRate();

        var wasSyncing = _syncingItemRow;
        _syncingItemRow = true; // 아래 SetRowCellValue가 다시 이 핸들러를 타지 않게
        try
        {
            gvw1.SetRowCellValue(rowHandle, colAmt, amt);
            gvw1.SetRowCellValue(rowHandle, colVat, vat);
            gvw1.SetRowCellValue(rowHandle, colTotalAmt, total);
            gvw1.SetRowCellValue(rowHandle, colKorPrice, price * exc);
            gvw1.SetRowCellValue(rowHandle, colKorAmt, amt * exc);
            gvw1.SetRowCellValue(rowHandle, colKorVat, vat * exc);
            gvw1.SetRowCellValue(rowHandle, colKorTotalAmt, total * exc);
        }
        finally { _syncingItemRow = wasSyncing; }
    }

    /// <summary>헤더 환율(비었거나 0이면 1) - 원화 = 외화 x 환율.</summary>
    private decimal ExcRate()
    {
        var rate = ToDecimal(txtExcRate.Text);
        return rate == 0 ? 1 : rate;
    }

    /// <summary>환율이 바뀌면 모든 품목의 원화 컬럼(kor_*)을 다시 계산한다. 화면에 값을 채우는 중(_loadingHeader)에는 안 한다 -
    /// 그때 다시 쓰면 방금 조회한 행이 "수정됨"으로 바뀌어 저장 대상이 된다.</summary>
    private void RecalcAllKor()
    {
        if (_loadingHeader || !_detail.Columns.Contains("kor_price")) return;

        var exc = ExcRate();
        foreach (DataRow row in _detail.Rows)
        {
            if (row.RowState == DataRowState.Deleted || row.RowState == DataRowState.Detached) continue;
            row["kor_price"] = ToDecimal(row["price"]) * exc;
            row["kor_amt"] = ToDecimal(row["amt"]) * exc;
            row["kor_vat"] = ToDecimal(row["vat"]) * exc;
            row["kor_total_amt"] = ToDecimal(row["total_amt"]) * exc;
        }
    }

    private DataTable? _totalsTable;

    /// <summary>헤더의 공급가액/부가세액/합계금액 표시칸이 품목 그리드 합계를 그대로 따라가게 한다 - 품목이 바뀔 때마다
    /// (셀 수정, 단가 자동조회, 행 삭제, 다시 조회) 다시 합산한다. 저장하면 서버가 같은 값을 헤더(TMAPOREQM)에 넣는다.</summary>
    private void AttachTotals()
    {
        if (_totalsTable != null)
        {
            _totalsTable.RowChanged -= DetailRowChanged;
            _totalsTable.RowDeleted -= DetailRowChanged;
            _totalsTable.TableCleared -= DetailTableCleared;
        }

        _totalsTable = _detail;
        _detail.RowChanged += DetailRowChanged;
        _detail.RowDeleted += DetailRowChanged;
        _detail.TableCleared += DetailTableCleared;
        RefreshTotals();
    }

    private void DetailRowChanged(object? sender, DataRowChangeEventArgs e) => RefreshTotals();
    private void DetailTableCleared(object? sender, DataTableClearEventArgs e) => RefreshTotals();

    private void RefreshTotals()
    {
        decimal amt = 0, vat = 0, total = 0;
        foreach (DataRow row in _detail.Rows)
        {
            if (row.RowState == DataRowState.Deleted || row.RowState == DataRowState.Detached) continue;
            if (_detail.Columns.Contains("amt")) amt += ToDecimal(row["amt"]);
            if (_detail.Columns.Contains("vat")) vat += ToDecimal(row["vat"]);
            if (_detail.Columns.Contains("total_amt")) total += ToDecimal(row["total_amt"]);
        }

        // 코드가 채우는 값이라 "사용자가 수정했다"(닫을 때 저장 확인)로 세면 안 된다. SuppressDirtyTracking은 끝에 IsDirty를 false로
        // 되돌리므로, 사용자가 이미 고친 상태(품목 셀 수정으로 이 함수가 불린 경우)는 그대로 보존한다.
        var wasDirty = IsDirty;
        SuppressDirtyTracking(() =>
        {
            txtSumAmt.EditValue = amt;
            txtSumVat.EditValue = vat;
            txtSumTotalAmt.EditValue = total;
        });
        IsDirty = wasDirty;
    }

    private static (decimal amt, decimal vat, decimal total) CalcAmounts(decimal qty, decimal price, decimal vatRate)
    {
        var amt = qty * price;
        var vat = amt * vatRate / 100;
        return (amt, vat, amt + vat);
    }

    /// <summary>단가 자동조회(USP_MA_POPRICE_Q 'Q1' - 거래처 전용 단가가 있으면 그것, 없으면 공통 단가)를 행에 채운다.
    /// 이미 단가가 있는 행(사용자가 직접 입력하거나 팝업 결과로 채워진 행)은 덮어쓰지 않는다. 값은 DataRow가 아니라 그리드
    /// (SetRowCellValue)로 쓴다 - 방금 추가해서 아직 확정 전인 새 행은 DataRow가 테이블에 없는 상태(Detached)라 DataRow에 직접 쓰면
    /// 화면에 안 보이다가 포커스가 행을 벗어나 확정된 뒤에야 나타난다(2026-09-26 "품번에서 포커스를 떠나야 단가가 들어간다"). 그리드로 쓰면
    /// CellValueChanged가 금액(RecalcRow)까지 바로 다시 계산한다.</summary>
    private async Task ApplyPriceAsync(DataRow row, int rowHandle)
    {
        if (row.RowState == DataRowState.Deleted) return;
        if (ToDecimal(row["price"]) != 0) return;

        var itemId = row["item_id"]?.ToString();
        if (string.IsNullOrEmpty(itemId)) return;

        var custId = row["cust_id"]?.ToString();
        if (string.IsNullOrWhiteSpace(custId)) custId = txtCustId.Text;

        var result = await QueryAsync("USP_MA_POPRICE_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q1",
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_item_id"] = itemId,
            ["p_cust_id"] = string.IsNullOrWhiteSpace(custId) ? null : custId,
            ["p_base_date"] = dteReqDate.YyyyMmDd,
        });
        if (result.Rows.Count == 0) return;

        // 조회를 기다리는 사이 행이 확정돼서 핸들이 바뀌었거나 지워졌을 수 있다 - 같은 DataRow를 가리키는 핸들을 다시 찾는다.
        var handle = FindRowHandle(row, rowHandle);
        if (handle == GridControl.InvalidRowHandle) return;
        if (ToDecimal(gvw1.GetRowCellValue(handle, colPrice)) != 0) return; // 그 사이 팝업 결과(PopcolItem_ResultSelected)나 사용자가 채웠으면 덮어쓰지 않는다.

        gvw1.SetRowCellValue(handle, colPrice, ToDecimal(result.Rows[0]["price"]));
    }

    /// <summary>이 DataRow를 가리키는 그리드 행 핸들 - 처음 알던 핸들(hint)이 아직 그 행이면 그대로, 아니면 테이블에서 위치를 찾는다.
    /// 못 찾으면(행이 지워짐) InvalidRowHandle.</summary>
    private int FindRowHandle(DataRow row, int hint)
    {
        if (hint != GridControl.InvalidRowHandle && ReferenceEquals(gvw1.GetDataRow(hint), row)) return hint;

        var index = _detail.Rows.IndexOf(row);
        return index >= 0 ? gvw1.GetRowHandle(index) : GridControl.InvalidRowHandle;
    }

    /// <summary>품목 팝업에서 행을 골랐을 때 - 팝업 결과 행에 든 단가(헤더 거래처/요청일자 기준으로 서버가 고른 것)를 이 행의 단가로
    /// 바로 채운다(선택하자마자 - 포커스 이동을 기다리지 않는다). 셀 값 변경(Gvw1_CellValueChanged)이 이미 품목 정보를 채웠고 이 시점엔
    /// 그 뒤다. 단가가 없는 품목이면(팝업 결과의 price가 비어 있으면) 건드리지 않는다 - 나머지는 ApplyPriceAsync(단가 자동조회)가
    /// 예전처럼 처리한다. 그리드(SetRowCellValue)로 쓰므로 확정 전인 새 행에도 바로 보이고 금액이 같이 다시 계산된다.</summary>
    private void PopcolItem_ResultSelected(int rowHandle, WYNLAB.Base.Controls.PopupLookupResult result)
    {
        if (rowHandle == GridControl.InvalidRowHandle) return;
        if (!result.Row.TryGetValue("price", out var priceText) || string.IsNullOrWhiteSpace(priceText)) return;
        if (!decimal.TryParse(priceText, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var price)) return;

        // 단가에 등록된 단위가 있으면 같이(그 단가는 그 단위 기준) - 통화는 이 화면에선 헤더 값이라 건드리지 않는다.
        if (result.Row.TryGetValue("price_unit_cd", out var unit) && !string.IsNullOrWhiteSpace(unit))
            gvw1.SetRowCellValue(rowHandle, colUnitCd, unit);

        gvw1.SetRowCellValue(rowHandle, colPrice, price); // CellValueChanged -> RecalcRow가 금액/원화금액까지 채운다
    }
    private string? WhName(object? whId)
    {
        var id = Convert.ToString(whId);
        if (string.IsNullOrEmpty(id)) return null;
        foreach (DataRow r in _whCache.Rows)
            if (Convert.ToString(Col(r, "wh_id")) == id) return Convert.ToString(Col(r, "wh_nm"));
        return null;
    }

    /// <summary>품목 캐시 행에서 컬럼 값을 꺼낸다 - 캐시는 서버가 돌려준 행에서 컬럼을 모으므로, 모든 행이 NULL인 컬럼
    /// (예: 오늘 적용되는 단가가 하나도 없을 때 price_unit_cd/cust_id)은 테이블에 아예 없다. 그때 r["x"]는 예외를
    /// 던져 품번 선택이 그리드에 반영되지 않는다 - 없거나 NULL이면 null.</summary>
    private static object? Col(DataRow? row, string column)
    {
        if (row == null || !row.Table.Columns.Contains(column)) return null;
        var value = row[column];
        return value == DBNull.Value ? null : value;
    }

    private static decimal ToDecimal(object? value) =>
        value == null || value == DBNull.Value ? 0 : Convert.ToDecimal(value);

    public override async Task QueryClick() => await QueryCore(forceKey: null);

    /// <summary>grd1(마스터 목록) 선택 단계가 없다 - panHeader의 요청번호로 USP_MA_POREQ_Q를
    /// 한 번 부르면 헤더(0번 레코드셋)+품목(1번 레코드셋)이 같이 온다. forceKey는 저장 직후
    /// 재조회, 그리고 구매요청현황(frmPoReqList)에서 더블클릭 드릴다운으로 넘어올 때 쓴다.</summary>
    private async Task QueryCore(string? forceKey)
    {
        // 조회 공통 규칙(2026-09-26): 결과가 없으면 신규 입력 모드로 전환한다. 검색 번호를 안 넣은 조회는 서버가 "조건 없음"을 "가장 최근 문서 1건"으로 처리하므로,
        // 조회할 대상이 없는 것으로 보고 서버를 부르지 않고 바로 신규 모드로 간다.
        if (forceKey == null && string.IsNullOrWhiteSpace(txtSearchReqNo.Text))
        {
            EnterNewMode();
            Toast.Show("조회할 번호가 없어 신규 입력 상태로 전환했습니다.");
            return;
        }
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_req_id"] = forceKey,
            ["p_req_no"] = forceKey == null ? txtSearchReqNo.Text : null,
        };
        var tables = await QueryMultiAsync("USP_MA_POREQ_Q", p);
        var header = tables.Count > 0 ? tables[0] : new DataTable();
        _detail = tables.Count > 1 ? tables[1] : new DataTable();

        if (header.Rows.Count > 0) await OnRowLoadedAsync(header.Rows[0]);
        else
        {
            EnterNewMode();
            Toast.Show("조회 결과가 없어 신규 입력 상태로 전환했습니다.");
        }
    }

    private Task OnRowLoadedAsync(DataRow row)
    {
        _loadingHeader = true; // 헤더 값을 채우는 동안 환율/통화 변경 핸들러가 품목을 다시 계산하지 않게
        SuppressDirtyTracking(() =>
        {
            _editingKey = row["req_id"]?.ToString();
            cboAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            txtReqNo.Text = row["req_no"]?.ToString() ?? string.Empty;
            dteReqDate.YyyyMmDd = row["req_date"]?.ToString();
            txtReqTitle.Text = row["req_title"]?.ToString() ?? string.Empty;
            cboStatCd.EditValue = row["stat_cd"]?.ToString() ?? string.Empty;
            cboPoType.EditValue = row["po_type"]?.ToString() ?? string.Empty;
            txtCustId.Text = row["cust_id"]?.ToString() ?? string.Empty;
            txtCustNm.Text = row["cust_nm"]?.ToString() ?? string.Empty;
            txtDeptId.Text = row["dept_id"]?.ToString() ?? string.Empty;
            txtDeptNm.Text = row["dept_nm"]?.ToString() ?? string.Empty;
            txtEmpId.Text = row["emp_id"]?.ToString() ?? string.Empty;
            txtEmpNm.Text = row["emp_nm"]?.ToString() ?? string.Empty;
            cboCurCd.EditValue = row["cur_cd"]?.ToString() ?? string.Empty;
            txtExcRate.Text = row["exc_rate"]?.ToString() ?? "1";
            txtAppNo.Text = row["app_no"]?.ToString() ?? string.Empty;
            cboApprStatCd.EditValue = row["appr_stat_cd"]?.ToString() ?? string.Empty;
            memoRemark.Text = row["remark"]?.ToString() ?? string.Empty;
        });
        _loadingHeader = false;

        return BindDetailGridAsync();
    }

    private Task BindDetailGridAsync()
    {
        TrackDirty(_detail);
        grd1.DataSource = _detail;
        AttachTotals();
        return Task.CompletedTask;
    }

    private bool _loadingHeader;

    private void EnterNewMode()
    {
        _loadingHeader = true;
        SuppressDirtyTracking(() =>
        {
            _editingKey = null;
            cboAccId.EditValue = Session.AccId?.ToString();
            txtReqNo.Text = string.Empty;
            dteReqDate.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");
            txtReqTitle.Text = string.Empty;
            cboStatCd.EditValue = "0";
            cboPoType.EditValue = null;
            txtCustId.Text = string.Empty;
            txtCustNm.Text = string.Empty;
            // 부서/담당자 - 로그인 세션값으로 자동입력(2026-09-22 요청, 사업장 자동입력과 같은 원칙).
            txtDeptId.Text = Session.DeptId?.ToString() ?? string.Empty;
            txtDeptNm.Text = Session.DeptNm;
            txtEmpId.Text = Session.EmpId?.ToString() ?? string.Empty;
            txtEmpNm.Text = Session.EmpNm; // 담당자명 = 세션 사원명(EmpId의 이름) - 사용자 이름(UserNm)이 아니다
            cboCurCd.EditValue = "KRW";
            txtExcRate.Text = "1";
            txtAppNo.Text = string.Empty;
            cboApprStatCd.EditValue = string.Empty;
            memoRemark.Text = string.Empty;

            _detail = _detail.Clone();
            TrackDirty(_detail);
            grd1.DataSource = _detail;
            AttachTotals();
        });
        _loadingHeader = false;
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        var headerParams = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_req_id"] = _editingKey,
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_req_date"] = dteReqDate.YyyyMmDd,
            ["p_req_title"] = txtReqTitle.Text,
            ["p_po_type"] = cboPoType.EditValue?.ToString(),
            ["p_cust_id"] = txtCustId.Text,
            ["p_dept_id"] = txtDeptId.Text,
            ["p_emp_id"] = txtEmpId.Text,
            ["p_cur_cd"] = cboCurCd.EditValue?.ToString(),
            ["p_exc_rate"] = ExcRate().ToString(System.Globalization.CultureInfo.InvariantCulture),
            ["p_remark"] = memoRemark.Text,
        };

        var headerResult = await SaveAsync("USP_MA_POREQ_S", headerParams);
        if (headerResult == null || !headerResult.Success)
        {
            AppMessageBox.Show(headerResult?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        var headerKey = _editingKey ?? headerResult.GeneratedCode;

        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        foreach (DataRow row in _detail.Rows.Cast<DataRow>().ToList())
        {
            if (row.RowState == DataRowState.Unchanged) continue;

            var workType = row.RowState == DataRowState.Deleted ? "D" : row.RowState == DataRowState.Added ? "N" : "U";
            var version = row.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;

            var detailParams = new Dictionary<string, string?>
            {
                ["p_work_type"] = workType,
                ["p_req_id"] = headerKey,
                ["p_serl"] = ProcData.Str(row, "serl", version),
                ["p_item_id"] = ProcData.Str(row, "item_id", version),
                ["p_qty"] = ProcData.Str(row, "qty", version),
                ["p_next_qty"] = ProcData.Str(row, "next_qty", version),
                ["p_unit_cd"] = ProcData.Str(row, "unit_cd", version),
                ["p_price"] = ProcData.Str(row, "price", version),
                ["p_amt"] = ProcData.Str(row, "amt", version),
                ["p_vat_rate"] = ProcData.Str(row, "vat_rate", version),
                ["p_vat"] = ProcData.Str(row, "vat", version),
                ["p_total_amt"] = ProcData.Str(row, "total_amt", version),
                ["p_kor_price"] = ProcData.Str(row, "kor_price", version),
                ["p_kor_amt"] = ProcData.Str(row, "kor_amt", version),
                ["p_kor_vat"] = ProcData.Str(row, "kor_vat", version),
                ["p_kor_total_amt"] = ProcData.Str(row, "kor_total_amt", version),
                ["p_cust_id"] = ProcData.Str(row, "cust_id", version),
                ["p_delv_date"] = ProcData.Str(row, "delv_date", version),
                ["p_wh_id"] = ProcData.Str(row, "wh_id", version),
                // 아래 셋은 화면에 열은 없지만 수정(U)이 이 값을 NULL로 덮어쓰지 않도록 원래 값을 그대로 다시 보낸다.
                ["p_loc_id"] = ProcData.Str(row, "loc_id", version),
                ["p_src_type"] = ProcData.Str(row, "src_type", version),
                ["p_src_id"] = ProcData.Str(row, "src_id", version),
                ["p_src_serl"] = ProcData.Str(row, "src_serl", version),
                ["p_src_no"] = ProcData.Str(row, "src_no", version),
                ["p_remark"] = ProcData.Str(row, "remark", version),
            };

            var detailResult = await SaveAsync("USP_MA_POREQ_S_1", detailParams);
            if (detailResult == null || !detailResult.Success)
            {
                AppMessageBox.Show(detailResult?.Message ?? "품목 저장에 실패했습니다.", "저장 실패");
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

        var result = await SaveAsync("USP_MA_POREQ_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = "D",
            ["p_req_id"] = _editingKey,
        });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "삭제에 실패했습니다.", "삭제 실패");
            return;
        }

        _editingKey = null;
        Toast.Show("삭제되었습니다.");
        EnterNewMode();
    }

    /// <summary>구매요청현황(frmPoReqList)에서 구매요청번호 하이퍼링크를 더블클릭했을 때
    /// 호출된다(key=req_id 문자열) - 검색조건 없이 그 건 하나만 조회한다.</summary>
    public override async Task FocusRecordAsync(string key) => await QueryCore(forceKey: key);

    /// <summary>전자결재 공용 팝업(popApp)을 연다 - frmNameCardReq.OpenApprovalAsync와 완전히
    /// 같은 방식, doc_type만 "POREQ"로 다르다.</summary>
    private async Task OpenApprovalAsync()
    {
        if (_editingKey == null)
        {
            AppMessageBox.Show("먼저 저장한 뒤 전자결재를 열어주세요.", "안내");
            return;
        }

        var changed = await popApp.ShowAsync(
            "POREQ", long.Parse(_editingKey), txtReqNo.Text,
            $"구매요청서 - {txtReqTitle.Text}", memoRemark.Text, this);

        if (changed) await QueryCore(forceKey: _editingKey);
    }
}
