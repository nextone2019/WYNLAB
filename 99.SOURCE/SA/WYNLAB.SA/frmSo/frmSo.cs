using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Popup;

namespace WYNLAB.SA;

/// <summary>
/// 수주등록 - frmPo(구매발주등록)와 완전히 같은 Master-One Sheet 구조(TMASOM/TMASOD 대상). 구매의
/// 검사(qc_yn)는 없고 재고(stock_yn/창고/위치)는 그대로 있다(출하가 재고를 차감하니 필요).
/// "구매요청 불러오기" 대신 "견적 불러오기"(popQtPick, 확정된 견적의 잔량만 대상). 결재는 수주
/// 확정 때 필요(정책, [[project_wynlab_sales_module_design]]) - frmPo와 동일한 방식으로
/// doc_type만 "SO"로 다르다. 금액 계산은 frmPo/frmQt와 동일하게 화면에서만 하고 서버는 그대로 저장.
/// </summary>
public partial class frmSo : BaseForm
{
    private DataTable _detail = new();
    private DataTable _itemCache = new();
    private string? _editingKey; // null이면 신규모드

    public frmSo()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준(2026-10-03): 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "수주등록";

        txtDeptNm.MapField("dept_id", txtDeptId);
        txtEmpNm.MapField("emp_id", txtEmpId);
        txtEmpNm.LinkDept(txtDeptNm, txtDeptId); // 담당자 팝업은 선택한 부서 소속만, 담당자를 고르면 부서도 채움
        txtCustNm.MapField("cust_id", txtCustId);
        txtCustNm.MapField("vat_type", cboVatType); // 거래처를 고르면 그 거래처의 부가세유형/세율이 채워진다(구매발주와 동일)
        txtCustNm.MapField("vat_rate", txtVatRate);

        // 부가세유형을 고르면 그 유형의 세율(L_CM0004의 rel_cd1)을 부가세율에 채운다(2026-10-05 요청). 조회로 문서를 읽을 때는 이 콤보를 채운 바로 다음 줄에서
        // 저장돼 있던 실제 세율로 다시 덮어쓰므로(OnRowLoaded) 이 핸들러 결과가 남지 않는다 - 사용자가 직접 유형을 바꿀 때만 실질적으로 적용된다.
        cboVatType.EditValueChanged += (s, e) =>
        {
            if (string.IsNullOrEmpty(cboVatType.EditValue?.ToString())) return;
            var rate = cboVatType.GetColumnValue("rel_cd1");
            if (rate != null && rate != DBNull.Value) txtVatRate.Text = rate.ToString();
        };

        gvw1.Role = GridRoleWyn.Edit;
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

        btnOpenApproval.Click += async (s, e) => await SafeExecuteAsync(OpenApprovalAsync, "전자결재");

        btnLoadQt.Click += async (s, e) => await SafeExecuteAsync(LoadFromQuoteAsync, "견적 불러오기");
        btnPickItem.Click += async (s, e) => await SafeExecuteAsync(PickItemsAsync, "품목선택");
        btnLineStop.Click += async (s, e) => await SafeExecuteAsync(() => StopLineAsync(true), "수주 라인 마감");
        btnLineStopCancel.Click += async (s, e) => await SafeExecuteAsync(() => StopLineAsync(false), "수주 라인 마감취소");

        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) =>
        {
            _itemCache = await LoadPopupRowsAsync("P_ITEM");
            await EnsureDetailSchemaAsync();
        };
    }

    private bool _syncingItemRow;

    private void Gvw1_CellValueChanged(object? sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
    {
        if (_syncingItemRow) return;

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

            if (row != null && gvw1.GetDataRow(e.RowHandle) is { } dataRow)
                _ = SafeExecuteAsync(() => ApplyPriceAsync(dataRow), "판매단가 조회");
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
        if (forceKey == null && string.IsNullOrWhiteSpace(txtSearchSoNo.Text))
        {
            EnterNewMode();
            Toast.Show("조회할 번호가 없어 신규 입력 상태로 전환했습니다.");
            return;
        }
        var p = new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "Q",
            ["p_so_id"] = forceKey,
            ["p_so_no"] = forceKey == null ? txtSearchSoNo.Text : null,
        };
        var tables = await QueryMultiAsync("USP_SA_SO_Q", p);
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
            _editingKey = row["so_id"]?.ToString();
            cboAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            txtSoNo.Text = row["so_no"]?.ToString() ?? string.Empty;
            txtSearchSoNo.Text = txtSoNo.Text; // 링크로 열었거나 저장 후에도 조회 버튼이 현재 문서를 다시 읽도록
            dteSoDate.YyyyMmDd = row["so_date"]?.ToString();
            dteDelvDate.YyyyMmDd = row["delv_date"]?.ToString();
            txtSoTitle.Text = row["so_title"]?.ToString() ?? string.Empty;
            cboStatCd.EditValue = row["stat_cd"]?.ToString() ?? string.Empty;
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

        ApplyLockState();
        return BindDetailGridAsync();
    }

    private Task BindDetailGridAsync()
    {
        TrackDirty(_detail);
        grd1.DataSource = _detail;
        return Task.CompletedTask;
    }

    /// <summary>결재 상신된(상신/진행중/승인완료) 문서는 헤더/품목을 수정하지 못하게 잠근다 - 반려되거나 상신 취소되면 다시 수정할 수 있다(서버도 같은 기준으로 저장을 거부).</summary>
    private void ApplyLockState() =>
        ApplyApprovalLock(IsApprovalLockedStatus(cboApprStatCd.EditValue?.ToString()), panData, gvw1, btnAddRow1, btnDeletRow1, btnLoadQt, btnPickItem);

    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = null;
            cboAccId.EditValue = Session.AccId?.ToString();
            txtSoNo.Text = string.Empty;
            dteSoDate.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");
            dteDelvDate.YyyyMmDd = null;
            txtSoTitle.Text = string.Empty;
            cboStatCd.EditValue = "0";
            txtCustId.Text = string.Empty;
            txtCustNm.ClearSelf(); // .Text="" 대신 - 거래처에 매핑된 부가세유형/세율은 아래에서 기본값을 정한다
            txtDeptId.Text = Session.DeptId?.ToString() ?? string.Empty;
            txtDeptNm.Text = Session.DeptNm;
            txtEmpId.Text = Session.EmpId?.ToString() ?? string.Empty;
            txtEmpNm.Text = Session.EmpNm;
            cboCurCd.EditValue = "KRW";
            txtExcRate.Text = "1";
            cboVatType.EditValue = cboVatType.FirstItemValue; // 신규 기본 = 목록 첫 유형(부가세일반) - 세율은 위 핸들러가 채운다
            var defaultRate = string.IsNullOrEmpty(cboVatType.EditValue?.ToString()) ? null : cboVatType.GetColumnValue("rel_cd1"); // 같은 유형이 이미 선택돼 있어도 세율은 확실히 초기화
            txtVatRate.Text = defaultRate == null || defaultRate == DBNull.Value ? "10" : defaultRate.ToString(); // 콤보 목록이 아직 안 읽힌 시점(생성자)은 10
            txtAppNo.Text = string.Empty;
            cboApprStatCd.EditValue = string.Empty;
            memoRemark.Text = string.Empty;

            _detail = _detail.Clone();
            TrackDirty(_detail);
            grd1.DataSource = _detail;
        });
        ApplyLockState();
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        if (IsApprovalLockedStatus(cboApprStatCd.EditValue?.ToString()))
        {
            AppMessageBox.Show("결재 상신된 문서는 수정할 수 없습니다.", "안내");
            return;
        }
        var headerParams = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_so_id"] = _editingKey,
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_so_date"] = dteSoDate.YyyyMmDd,
            ["p_so_title"] = txtSoTitle.Text,
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

        var headerResult = await SaveAsync("USP_SA_SO_S", headerParams);
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
                ["p_so_id"] = headerKey,
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
                ["p_stock_yn"] = ProcData.Str(row, "stock_yn", version),
                ["p_src_type"] = ProcData.Str(row, "src_type", version),
                ["p_src_id"] = ProcData.Str(row, "src_id", version),
                ["p_src_no"] = ProcData.Str(row, "src_no", version),
                ["p_src_serl"] = ProcData.Str(row, "src_serl", version),
            };

            var detailResult = await SaveAsync("USP_SA_SO_S_1", detailParams);
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
        if (IsApprovalLockedStatus(cboApprStatCd.EditValue?.ToString()))
        {
            AppMessageBox.Show("결재 상신된 문서는 삭제할 수 없습니다.", "안내");
            return;
        }
        if (_editingKey == null) return;

        var result = await SaveAsync("USP_SA_SO_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = "D",
            ["p_so_id"] = _editingKey,
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

    private async Task EnsureDetailSchemaAsync()
    {
        if (_detail.Columns.Count > 0) return;

        var tables = await QueryMultiAsync("USP_SA_SO_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_so_id"] = "-1",
        });
        if (tables.Count > 1) _detail = tables[1];
        EnterNewMode();
    }

    /// <summary>판매단가(USP_SA_PRICE_Q 'Q1')에서 이 품목/거래처/수주일에 적용되는 단가를 찾아 채운다.
    /// 이미 단가가 들어 있는 행(사용자가 직접 입력, 혹은 견적에서 불러온 행)은 덮어쓰지 않는다.</summary>
    private async Task ApplyPriceAsync(DataRow row)
    {
        if (row.RowState == DataRowState.Detached || row.RowState == DataRowState.Deleted) return;
        if (ToDecimal(row["price"]) != 0) return;

        var itemId = row["item_id"]?.ToString();
        if (string.IsNullOrEmpty(itemId)) return;

        var result = await QueryAsync("USP_SA_PRICE_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q1",
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_item_id"] = itemId,
            ["p_cust_id"] = string.IsNullOrWhiteSpace(txtCustId.Text) ? null : txtCustId.Text,
            ["p_base_date"] = dteSoDate.YyyyMmDd,
        });
        if (result.Rows.Count == 0) return;
        if (row.RowState == DataRowState.Detached || row.RowState == DataRowState.Deleted) return;

        row["price"] = result.Rows[0]["price"];
        ApplyAmounts(row);
    }

    /// <summary>품목선택 - 공통 팝업(P_ITEM)을 다중 선택으로 열어 체크한 품목마다 행을 추가한다(견적 없이 직접 수주). 같은 품목을 또 골라도 막지 않는다.
    /// 단가는 판매단가에서 자동 조회한다.</summary>
    private async Task PickItemsAsync()
    {
        if (IsApprovalLockedStatus(cboApprStatCd.EditValue?.ToString())) return;
        if (_detail.Columns.Count == 0) { Toast.Show("화면을 준비하는 중입니다. 잠시 후 다시 시도해주세요."); return; }
        gvw1.CloseEditor();

        var picked = await WYNLAB.Popup.popPopUp.ShowMultiAsync("P_ITEM", this);
        if (picked == null || picked.Count == 0) return;

        var vatRate = string.IsNullOrWhiteSpace(txtVatRate.Text) ? "0" : txtVatRate.Text;
        var missing = 0;
        foreach (var p in picked)
        {
            DataRow? src = null;
            foreach (DataRow r in _itemCache.Rows)
                if (string.Equals(r["item_no"]?.ToString(), p.Code, StringComparison.OrdinalIgnoreCase)
                    || r["item_id"]?.ToString() == p.Code) { src = r; break; }
            if (src == null) { missing++; continue; }

            var row = _detail.NewRow();
            row["item_id"] = src["item_id"];
            row["item_no"] = src["item_no"];
            row["item_nm"] = src["item_nm"];
            row["item_spec"] = src["item_spec"];
            row["unit_cd"] = src["unit_cd"];
            row["qty"] = 0;
            row["next_qty"] = 0;
            row["remain_qty"] = 0;
            row["price"] = 0;
            row["vat_rate"] = vatRate;
            row["stop_yn"] = "N";
            _detail.Rows.Add(row);

            await ApplyPriceAsync(row);
            ApplyAmounts(row);
        }
        if (missing > 0) Toast.Show($"품목 {missing}건은 품목 목록을 아직 못 받아 추가하지 못했습니다. 잠시 후 다시 시도해주세요.");
    }
    /// <summary>확정된 견적의 잔량 품목을 팝업(popQtPick)에서 골라 수주 품목으로 가져온다. 가져온 행은
    /// src_type='QT'와 견적 키(src_id/src_serl)를 갖고, 저장 때 USP_SA_SO_S_1이 견적 라인의
    /// next_qty를 재계산한다. 이미 그리드에 같은 견적 라인이 있으면 건너뛴다.</summary>
    private async Task LoadFromQuoteAsync()
    {
        gvw1.CloseEditor();

        var picked = popQtPick.Pick(this, MenuId, cboAccId.EditValue?.ToString(), txtCustId.Text, txtCustNm.Text);
        if (picked == null || picked.Rows.Count == 0) return;

        var first = picked.Rows[0];
        if (string.IsNullOrWhiteSpace(txtCustId.Text))
        {
            txtCustId.Text = first["cust_id"]?.ToString() ?? string.Empty;
            txtCustNm.Text = first["cust_nm"]?.ToString() ?? string.Empty;
        }

        var vatRate = string.IsNullOrWhiteSpace(txtVatRate.Text) ? "0" : txtVatRate.Text;
        var skipped = 0;

        foreach (DataRow r in picked.Rows)
        {
            var qtId = r["qt_id"].ToString();
            var qtSerl = r["qt_serl"].ToString();
            var dup = _detail.Rows.Cast<DataRow>().Any(d =>
                d.RowState != DataRowState.Deleted
                && d["src_type"]?.ToString() == "QT"
                && d["src_id"]?.ToString() == qtId
                && d["src_serl"]?.ToString() == qtSerl);
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
            row["src_type"] = "QT";
            row["src_id"] = r["qt_id"];
            row["src_no"] = r["qt_no"];
            row["src_serl"] = r["qt_serl"];
            row["stop_yn"] = "N";
            _detail.Rows.Add(row);

            await ApplyPriceAsync(row);
            ApplyAmounts(row);
        }

        if (skipped > 0) Toast.Show($"이미 불러온 견적 품목 {skipped}건은 건너뛰었습니다.");
    }

    /// <summary>선택한 수주 품목을 마감(잔량이 남아도 종결)하거나 마감을 취소한다.</summary>
    private async Task StopLineAsync(bool stop)
    {
        if (_editingKey == null)
        {
            AppMessageBox.Show("저장된 수주에서만 마감할 수 있습니다.", "안내");
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
            AppMessageBox.Show("마감할 수주 품목을 선택하세요.", "안내");
            return;
        }
        if (cboStatCd.EditValue?.ToString() != "C")
        {
            AppMessageBox.Show("승인 완료된 수주의 품목만 마감할 수 있습니다.", "안내");
            return;
        }

        var itemNm = view.Row["item_nm"]?.ToString();
        string? remark = null;
        if (stop)
        {
            remark = DevExpress.XtraEditors.XtraInputBox.Show($"[{itemNm}] 마감 사유를 입력하세요.\n(잔량이 남아 있어도 이 품목의 출하 대상에서 제외됩니다)", "수주 라인 마감", string.Empty);
            if (remark == null) return;
        }
        else if (AppMessageBox.Show($"[{itemNm}] 마감을 취소하시겠습니까?", "마감취소", MessageBoxButtons.YesNo) != DialogResult.Yes)
        {
            return;
        }

        var result = await SaveAsync("USP_SA_SOSTOP_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = "U",
            ["p_so_id"] = _editingKey,
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

    public override async Task FocusRecordAsync(string key) => await QueryCore(forceKey: key);

    /// <summary>전자결재 - frmPo.OpenApprovalAsync와 완전히 같은 방식, doc_type만 "SO"로 다르다.</summary>
    private async Task OpenApprovalAsync()
    {
        if (_editingKey == null)
        {
            AppMessageBox.Show("먼저 저장한 뒤 전자결재를 열어주세요.", "안내");
            return;
        }

        var changed = await popApp.ShowAsync(
            "SO", long.Parse(_editingKey), txtSoNo.Text,
            $"수주서 - {txtSoTitle.Text}", memoRemark.Text, this);

        if (changed) await QueryCore(forceKey: _editingKey);
    }
}
