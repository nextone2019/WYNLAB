using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Popup;

namespace WYNLAB.MA;

/// <summary>
/// 구매발주등록 - frmPoReq와 완전히 같은 Master-One Sheet 구조(TMAPOM/TMAPOD 대상). 발주는
/// 확정 가격이 필요해서 그리드에 단가/공급가액/부가세/합계/원화환산 컬럼이 추가되고, 이 계산은
/// 요청 화면과 마찬가지로 화면(Gvw1_CellValueChanged)에서만 하고 저장 프로시저는 그 값을 그대로
/// 받아 저장한다(USP_MA_PO_S_1 - 계산로직을 서버에 두지 않는 기존 원칙 그대로).
/// </summary>
public partial class frmPo : BaseForm
{
    private DataTable _detail = new();
    private DataTable _itemCache = new();
    private string? _editingKey; // null이면 신규모드

    public frmPo()
    {
        InitializeComponent();

        Text = "구매발주등록";

        Controls.Add(BuildScreenHeader());

        txtDeptNm.MapField("dept_id", txtDeptId);
        txtEmpNm.MapField("emp_id", txtEmpId);
        txtCustNm.MapField("cust_id", txtCustId);
        txtCustNm.MapField("vat_type", cboVatType);
        txtCustNm.MapField("vat_rate", txtVatRate);

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

        btnAddRow1.Click += (s, e) => gvw1.AddNewRow();
        btnDeletRow1.Click += (s, e) =>
        {
            try { if (gvw1.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };

        var btnOpenApproval = new ButtonWyn
        {
            Text = "전자결재",
            Location = new System.Drawing.Point(1055, 9),
            Size = new System.Drawing.Size(90, 24),
        };
        btnOpenApproval.Click += async (s, e) => await SafeExecuteAsync(OpenApprovalAsync, "전자결재");
        panData.Controls.Add(btnOpenApproval);

        btnLoadReq.Click += async (s, e) => await SafeExecuteAsync(LoadFromRequestAsync, "구매요청 불러오기");
        btnLineStop.Click += async (s, e) => await SafeExecuteAsync(() => StopLineAsync(true), "발주 라인 마감");
        btnLineStopCancel.Click += async (s, e) => await SafeExecuteAsync(() => StopLineAsync(false), "발주 라인 마감취소");

        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) =>
        {
            // frmPoReq와 같은 이유(그쪽 주석 참고, 2026-09-22) - 조건 없이 QueryClick을 부르면
            // USP_MA_PO_Q가 마지막 저장된 발주서를 그대로 띄워버린다. EnterNewMode()가 이미
            // 신규모드를 잡아뒀으니 그대로 둔다.
            _itemCache = await LoadPopupRowsAsync("P_ITEM");
            await EnsureDetailSchemaAsync();
        };
    }

    // colItemNo(품번) 재귀 가드 - frmPoReq.Gvw1_CellValueChanged와 같은 이유.
    private bool _syncingItemRow;

    /// <summary>품번(colItemNo) 선택/타이핑 시 품목ID(숨겨진 colItemId)/품명/규격/단위 자동채움
    /// (frmPoReq와 동일한 이유 - 그쪽 주석 참고) + 수량/단가/세율이 바뀔 때마다 공급가액/부가세/
    /// 합계/원화환산을 화면에서 다시 계산한다 - 서버는 이 값을 그대로 저장만 한다(USP_MA_PO_S_1).</summary>
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
                    if (!Equals(Convert.ToInt64(r["item_id"]), itemId)) continue;
                    row = r;
                    break;
                }
            }
            if (row == null && !string.IsNullOrWhiteSpace(typed))
            {
                foreach (DataRow r in _itemCache.Rows)
                {
                    if (!string.Equals(r["item_no"]?.ToString(), typed, StringComparison.OrdinalIgnoreCase)) continue;
                    row = r;
                    break;
                }
            }

            _syncingItemRow = true;
            try
            {
                gvw1.SetRowCellValue(e.RowHandle, colItemId, row?["item_id"]);
                gvw1.SetRowCellValue(e.RowHandle, colItemNo, row?["item_no"]);
                gvw1.SetRowCellValue(e.RowHandle, colItemNm, row?["item_nm"]);
                gvw1.SetRowCellValue(e.RowHandle, colItemSpec, row?["item_spec"]);
                gvw1.SetRowCellValue(e.RowHandle, colUnitCd, row?["unit_cd"]);
            }
            finally
            {
                _syncingItemRow = false;
            }

            // 구매단가(TMAPOPRICE)에서 거래처/기준일에 맞는 단가를 채운다. 사용자가 이미 단가를 넣은 행은 건드리지 않는다.
            if (row != null && gvw1.GetDataRow(e.RowHandle) is { } dataRow)
                _ = SafeExecuteAsync(() => ApplyPriceAsync(dataRow), "구매단가 조회");
        }
        else if (e.Column == colQty || e.Column == colNextQty)
        {
            var qty = ToDecimal(gvw1.GetRowCellValue(e.RowHandle, colQty));
            var nextQty = ToDecimal(gvw1.GetRowCellValue(e.RowHandle, colNextQty));
            gvw1.SetRowCellValue(e.RowHandle, colRemainQty, qty - nextQty);
        }

        if (e.Column == colQty || e.Column == colPrice || e.Column == colVatRate)
        {
            RecalcRow(e.RowHandle);
        }
    }

    /// <summary>공급가액=수량*단가, 부가세=공급가액*세율, 합계=공급가액+부가세, 원화환산은
    /// 헤더의 환율(txtExcRate)을 곱한다.</summary>
    private void RecalcRow(int rowHandle)
    {
        var qty = ToDecimal(gvw1.GetRowCellValue(rowHandle, colQty));
        var price = ToDecimal(gvw1.GetRowCellValue(rowHandle, colPrice));
        var vatRate = ToDecimal(gvw1.GetRowCellValue(rowHandle, colVatRate));
        var (amt, vat, totalAmt, excRate) = CalcAmounts(qty, price, vatRate);

        gvw1.SetRowCellValue(rowHandle, colAmt, amt);
        gvw1.SetRowCellValue(rowHandle, colVat, vat);
        gvw1.SetRowCellValue(rowHandle, colTotalAmt, totalAmt);
        gvw1.SetRowCellValue(rowHandle, colKorPrice, price * excRate);
        gvw1.SetRowCellValue(rowHandle, colKorAmt, amt * excRate);
        gvw1.SetRowCellValue(rowHandle, colKorVat, vat * excRate);
        gvw1.SetRowCellValue(rowHandle, colKorTotalAmt, totalAmt * excRate);
    }

    private bool IsNumberColumn(DevExpress.XtraGrid.Columns.GridColumn column) =>
        column == colQty || column == colPrice || column == colVatRate;

    private (decimal amt, decimal vat, decimal totalAmt, decimal excRate) CalcAmounts(decimal qty, decimal price, decimal vatRate)
    {
        var excRate = ToDecimal(txtExcRate.Text);
        if (excRate == 0) excRate = 1;

        var amt = qty * price;
        var vat = amt * vatRate / 100;
        return (amt, vat, amt + vat, excRate);
    }

    /// <summary>그리드 셀 편집을 거치지 않고 DataRow에 직접 값을 넣는 경로(요청 불러오기, 단가 자동조회 뒤)에서
    /// RecalcRow와 같은 계산을 한다 - 그 경로는 CellValueChanged가 안 타므로 금액이 안 채워진다.</summary>
    private void ApplyAmounts(DataRow row)
    {
        var qty = ToDecimal(row["qty"]);
        var price = ToDecimal(row["price"]);
        var vatRate = ToDecimal(row["vat_rate"]);
        var (amt, vat, totalAmt, excRate) = CalcAmounts(qty, price, vatRate);

        row["amt"] = amt;
        row["vat"] = vat;
        row["total_amt"] = totalAmt;
        row["kor_price"] = price * excRate;
        row["kor_amt"] = amt * excRate;
        row["kor_vat"] = vat * excRate;
        row["kor_total_amt"] = totalAmt * excRate;
    }

    private static decimal ToDecimal(object? value)
    {
        if (value == null || value == DBNull.Value) return 0;
        return decimal.TryParse(value.ToString(), out var d) ? d : 0;
    }

    public override async Task QueryClick() => await QueryCore(forceKey: null);

    private async Task QueryCore(string? forceKey)
    {
        // 조회 공통 규칙(2026-09-26): 결과가 없으면 신규 입력 모드로 전환한다. 검색 번호를 안 넣은 조회는 서버가 "조건 없음"을 "가장 최근 문서 1건"으로 처리하므로,
        // 조회할 대상이 없는 것으로 보고 서버를 부르지 않고 바로 신규 모드로 간다.
        if (forceKey == null && string.IsNullOrWhiteSpace(txtSearchPoNo.Text))
        {
            EnterNewMode();
            Toast.Show("조회할 번호가 없어 신규 입력 상태로 전환했습니다.");
            return;
        }
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_po_id"] = forceKey,
            ["p_po_no"] = forceKey == null ? txtSearchPoNo.Text : null,
        };
        var tables = await QueryMultiAsync("USP_MA_PO_Q", p);
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
        SuppressDirtyTracking(() =>
        {
            _editingKey = row["po_id"]?.ToString();
            cboAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            txtPoNo.Text = row["po_no"]?.ToString() ?? string.Empty;
            dtePoDate.YyyyMmDd = row["po_date"]?.ToString();
            dteDelvDate.YyyyMmDd = row["delv_date"]?.ToString();
            txtPoTitle.Text = row["po_title"]?.ToString() ?? string.Empty;
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
            cboVatType.EditValue = row["vat_type"]?.ToString() ?? string.Empty;
            txtVatRate.Text = row["vat_rate"]?.ToString() ?? string.Empty;
            txtAppNo.Text = row["app_no"]?.ToString() ?? string.Empty;
            cboApprStatCd.EditValue = row["appr_stat_cd"]?.ToString() ?? string.Empty;
            memoRemark.Text = row["remark"]?.ToString() ?? string.Empty;
        });

        // 검사/재고 정책은 발주 승인 전까지만 바꿀 수 있다(승인 뒤에는 결재 받은 내용과 처리 경로가 달라지지 않게)
        var approved = cboStatCd.EditValue?.ToString() == "C";
        colQcYn.OptionsColumn.AllowEdit = !approved;
        colStockYn.OptionsColumn.AllowEdit = !approved;

        return BindDetailGridAsync();
    }

    private Task BindDetailGridAsync()
    {
        TrackDirty(_detail);
        grd1.DataSource = _detail;
        return Task.CompletedTask;
    }

    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = null;
            cboAccId.EditValue = Session.AccId?.ToString();
            txtPoNo.Text = string.Empty;
            dtePoDate.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");
            dteDelvDate.YyyyMmDd = null;
            txtPoTitle.Text = string.Empty;
            cboStatCd.EditValue = "0";
            cboPoType.EditValue = cboPoType.FirstItemValue ?? string.Empty;
            txtCustId.Text = string.Empty;
            txtCustNm.ClearSelf(); // .Text="" 대신 - MapField(vat_type/vat_rate) 매핑필드는 이 아래에서 따로 기본값을 정한다
            txtDeptId.Text = Session.DeptId?.ToString() ?? string.Empty;
            txtDeptNm.Text = Session.DeptNm;
            txtEmpId.Text = Session.EmpId?.ToString() ?? string.Empty;
            txtEmpNm.Text = Session.EmpNm; // 담당자명 = 세션 사원명(EmpId의 이름) - 사용자 이름(UserNm)이 아니다
            cboCurCd.EditValue = "KRW";
            txtExcRate.Text = "1";
            cboVatType.EditValue = null;
            txtVatRate.Text = "10";
            txtAppNo.Text = string.Empty;
            cboApprStatCd.EditValue = string.Empty;
            memoRemark.Text = string.Empty;

            _detail = _detail.Clone();
            TrackDirty(_detail);
            grd1.DataSource = _detail;
        });
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
            ["p_po_id"] = _editingKey,
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_po_date"] = dtePoDate.YyyyMmDd,
            ["p_po_type"] = cboPoType.EditValue?.ToString(),
            ["p_po_title"] = txtPoTitle.Text,
            ["p_cust_id"] = txtCustId.Text,
            ["p_dept_id"] = txtDeptId.Text,
            ["p_emp_id"] = txtEmpId.Text,
            ["p_cur_cd"] = cboCurCd.EditValue?.ToString(),
            ["p_exc_rate"] = txtExcRate.Text,
            ["p_delv_date"] = dteDelvDate.YyyyMmDd,
            ["p_vat_type"] = cboVatType.EditValue?.ToString(),
            ["p_vat_rate"] = txtVatRate.Text,
            ["p_remark"] = memoRemark.Text,
        };

        var headerResult = await SaveAsync("USP_MA_PO_S", headerParams);
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
                ["p_po_id"] = headerKey,
                ["p_serl"] = ProcData.Str(row, "serl", version),
                ["p_item_id"] = ProcData.Str(row, "item_id", version),
                ["p_unit_cd"] = ProcData.Str(row, "unit_cd", version),
                ["p_qty"] = ProcData.Str(row, "qty", version),
                ["p_price"] = ProcData.Str(row, "price", version),
                ["p_amt"] = ProcData.Str(row, "amt", version),
                ["p_vat"] = ProcData.Str(row, "vat", version),
                ["p_total_amt"] = ProcData.Str(row, "total_amt", version),
                ["p_kor_price"] = ProcData.Str(row, "kor_price", version),
                ["p_kor_amt"] = ProcData.Str(row, "kor_amt", version),
                ["p_kor_vat"] = ProcData.Str(row, "kor_vat", version),
                ["p_kor_total_amt"] = ProcData.Str(row, "kor_total_amt", version),
                ["p_vat_rate"] = ProcData.Str(row, "vat_rate", version),
                ["p_delv_date"] = ProcData.Str(row, "delv_date", version),
                ["p_wh_id"] = ProcData.Str(row, "wh_id", version),
                ["p_loc_id"] = ProcData.Str(row, "loc_id", version),
                ["p_qc_yn"] = ProcData.Str(row, "qc_yn", version),
                ["p_stock_yn"] = ProcData.Str(row, "stock_yn", version),
                ["p_src_type"] = ProcData.Str(row, "src_type", version),
                ["p_src_id"] = ProcData.Str(row, "src_id", version),
                ["p_src_no"] = ProcData.Str(row, "src_no", version),
                ["p_src_serl"] = ProcData.Str(row, "src_serl", version),
            };

            var detailResult = await SaveAsync("USP_MA_PO_S_1", detailParams);
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

        var result = await SaveAsync("USP_MA_PO_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = "D",
            ["p_po_id"] = _editingKey,
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

    /// <summary>새 폼은 아직 조회한 적이 없어 _detail에 컬럼이 없다 - 그 상태로 행을 추가하면 값이 DataTable이
    /// 아니라 그리드에만 남아 저장 때 빠진다. 조건에 안 걸리는 조회(p_po_id=-1)로 스키마만 받아 신규모드를 다시 잡는다.</summary>
    private async Task EnsureDetailSchemaAsync()
    {
        if (_detail.Columns.Count > 0) return;

        var tables = await QueryMultiAsync("USP_MA_PO_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_po_id"] = "-1",
        });
        if (tables.Count > 1) _detail = tables[1];
        EnterNewMode();
    }

    /// <summary>구매단가(USP_MA_POPRICE_Q 'Q1')에서 이 품목/거래처/발주일에 적용되는 단가를 찾아 채운다.
    /// 이미 단가가 들어 있는 행(사용자가 직접 입력)은 덮어쓰지 않는다.</summary>
    private async Task ApplyPriceAsync(DataRow row)
    {
        if (row.RowState == DataRowState.Detached || row.RowState == DataRowState.Deleted) return;
        if (ToDecimal(row["price"]) != 0) return;

        var itemId = row["item_id"]?.ToString();
        if (string.IsNullOrEmpty(itemId)) return;

        var result = await QueryAsync("USP_MA_POPRICE_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q1",
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_item_id"] = itemId,
            ["p_cust_id"] = string.IsNullOrWhiteSpace(txtCustId.Text) ? null : txtCustId.Text,
            ["p_base_date"] = dtePoDate.YyyyMmDd,
        });
        if (result.Rows.Count == 0) return;
        if (row.RowState == DataRowState.Detached || row.RowState == DataRowState.Deleted) return;

        row["price"] = result.Rows[0]["price"];
        ApplyAmounts(row);
    }

    /// <summary>승인 완료된 구매요청의 잔량 품목을 팝업(popReqPick)에서 골라 발주 품목으로 가져온다. 가져온 행은
    /// src_type='POREQ'와 요청 키(src_id/src_serl)를 갖고, 이후 서버가 저장 때 요청 잔량 검증과 요청 라인
    /// next_qty 재계산을 한다. 이미 그리드에 같은 요청 라인이 있으면 건너뛴다.</summary>
    private async Task LoadFromRequestAsync()
    {
        gvw1.CloseEditor();

        var picked = popReqPick.Pick(this, MenuId, cboAccId.EditValue?.ToString(), txtCustId.Text, txtCustNm.Text);
        if (picked == null || picked.Rows.Count == 0) return;

        // 헤더가 비어 있는 항목은 첫 요청 품목의 값으로 채운다(사용자가 이미 넣은 값은 유지)
        var first = picked.Rows[0];
        if (string.IsNullOrWhiteSpace(txtCustId.Text))
        {
            txtCustId.Text = first["cust_id"]?.ToString() ?? string.Empty;
            txtCustNm.Text = first["cust_nm"]?.ToString() ?? string.Empty;
        }
        if (cboPoType.EditValue == null && first["po_type"] != DBNull.Value) cboPoType.EditValue = first["po_type"].ToString();

        var vatRate = string.IsNullOrWhiteSpace(txtVatRate.Text) ? "0" : txtVatRate.Text;
        var skipped = 0;

        foreach (DataRow r in picked.Rows)
        {
            var reqId = r["req_id"].ToString();
            var reqSerl = r["req_serl"].ToString();
            var dup = _detail.Rows.Cast<DataRow>().Any(d =>
                d.RowState != DataRowState.Deleted
                && d["src_type"]?.ToString() == "POREQ"
                && d["src_id"]?.ToString() == reqId
                && d["src_serl"]?.ToString() == reqSerl);
            if (dup) { skipped++; continue; }

            var row = _detail.NewRow();
            row["item_id"] = r["item_id"];
            row["item_no"] = r["item_no"];
            row["item_nm"] = r["item_nm"];
            row["item_spec"] = r["item_spec"];
            row["unit_cd"] = r["unit_cd"];
            row["qty"] = r["remain_qty"];
            row["next_qty"] = 0;
            row["remain_qty"] = r["remain_qty"];
            row["price"] = 0;
            row["vat_rate"] = vatRate;
            row["delv_date"] = r["delv_date"];
            row["wh_id"] = r["wh_id"];
            row["wh_nm"] = r["wh_nm"];
            row["loc_id"] = r["loc_id"];
            row["loc_nm"] = r["loc_nm"];
            row["src_type"] = "POREQ";
            row["src_id"] = r["req_id"];
            row["src_no"] = r["req_no"];
            row["src_serl"] = r["req_serl"];
            row["stop_yn"] = "N";
            _detail.Rows.Add(row);

            await ApplyPriceAsync(row);
            ApplyAmounts(row);
        }

        if (skipped > 0) Toast.Show($"이미 불러온 요청 품목 {skipped}건은 건너뛰었습니다.");
    }

    /// <summary>선택한 발주 품목을 마감(잔량이 남아도 종결)하거나 마감을 취소한다. 저장된 품목, 승인 완료된 발주만
    /// 대상이고, 미저장 변경이 있으면 재조회로 사라지므로 먼저 저장하게 한다.</summary>
    private async Task StopLineAsync(bool stop)
    {
        if (_editingKey == null)
        {
            AppMessageBox.Show("저장된 발주에서만 마감할 수 있습니다.", "안내");
            return;
        }
        gvw1.CloseEditor();
        if (IsDirty)
        {
            AppMessageBox.Show("저장하지 않은 변경이 있습니다. 먼저 저장한 뒤 마감하세요.", "안내");
            return;
        }
        if (gvw1.GetFocusedRow() is not DataRowView view || view.Row.RowState == DataRowState.Added)
        {
            AppMessageBox.Show("마감할 발주 품목을 선택하세요.", "안내");
            return;
        }
        if (cboStatCd.EditValue?.ToString() != "C")
        {
            AppMessageBox.Show("승인 완료된 발주의 품목만 마감할 수 있습니다.", "안내");
            return;
        }

        var itemNm = view.Row["item_nm"]?.ToString();
        string? remark = null;
        if (stop)
        {
            remark = DevExpress.XtraEditors.XtraInputBox.Show($"[{itemNm}] 마감 사유를 입력하세요.\n(잔량이 남아 있어도 이 품목의 납품/입고 대상에서 제외됩니다)", "발주 라인 마감", string.Empty);
            if (remark == null) return; // 취소
        }
        else if (AppMessageBox.Show($"[{itemNm}] 마감을 취소하시겠습니까?", "마감취소", MessageBoxButtons.YesNo) != DialogResult.Yes)
        {
            return;
        }

        var result = await SaveAsync("USP_MA_POSTOP_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = "U",
            ["p_po_id"] = _editingKey,
            ["p_serl"] = view.Row["serl"]?.ToString(),
            ["p_stop_yn"] = stop ? "Y" : "N",
            ["p_stop_remark"] = remark,
        });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "처리에 실패했습니다.", stop ? "마감 실패" : "마감취소 실패");
            return;
        }

        Toast.Show(stop ? "마감되었습니다." : "마감이 취소되었습니다.");
        await QueryCore(forceKey: _editingKey);
    }

    /// <summary>구매발주현황(frmPoList)에서 발주번호 하이퍼링크를 더블클릭했을 때 호출된다
    /// (key=po_id 문자열).</summary>
    public override async Task FocusRecordAsync(string key) => await QueryCore(forceKey: key);

    /// <summary>전자결재 - frmPoReq.OpenApprovalAsync와 완전히 같은 방식, doc_type만 "PO"로
    /// 다르다.</summary>
    private async Task OpenApprovalAsync()
    {
        if (_editingKey == null)
        {
            AppMessageBox.Show("먼저 저장한 뒤 전자결재를 열어주세요.", "안내");
            return;
        }

        var changed = await popApp.ShowAsync(
            "PO", long.Parse(_editingKey), txtPoNo.Text,
            $"구매발주서 - {txtPoTitle.Text}", memoRemark.Text, this);

        if (changed) await QueryCore(forceKey: _editingKey);
    }
}
