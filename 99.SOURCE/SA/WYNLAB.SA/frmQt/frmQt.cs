using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.SA;

/// <summary>
/// 견적등록 - Master-One Sheet 구조(TMAQTM/TMAQTD 대상), 구매발주등록(frmPo)과 같은 패턴이지만:
///  - 결재 없음(수주 확정 때만 결재가 필요하다는 정책, [[project_wynlab_sales_module_design]] 참고).
///  - 검사/재고/창고(qc_yn/stock_yn/wh_id/loc_id) 없음 - 아직 창고에 닿는 단계가 아니다.
///  - 원천(src_type) 없음 - 견적이 영업 프로세스의 첫 문서.
///  - "요청 불러오기" 대신 "견적확정/확정취소" 버튼 - 확정된 견적만 수주에서 불러올 수 있다.
/// 금액 계산(공급가액/부가세/합계/원화환산)은 frmPo와 동일하게 화면에서만 하고 서버는 그대로 저장한다.
/// </summary>
public partial class frmQt : BaseForm
{
    private DataTable _detail = new();
    private DataTable _itemCache = new();
    private string? _editingKey; // null이면 신규모드

    public frmQt()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준(2026-10-03): 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "견적등록";

        txtDeptNm.MapField("dept_id", txtDeptId);
        txtEmpNm.MapField("emp_id", txtEmpId);
        txtEmpNm.LinkDept(txtDeptNm, txtDeptId); // 담당자 팝업은 선택한 부서 소속만, 담당자를 고르면 부서도 채움
        txtCustNm.MapField("cust_id", txtCustId);

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

        btnConfirm.Click += async (s, e) => await SafeExecuteAsync(() => ConfirmAsync(true), "견적확정");
        btnConfirmCancel.Click += async (s, e) => await SafeExecuteAsync(() => ConfirmAsync(false), "견적확정취소");

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

            // 판매단가(TMASAPRICE)에서 거래처/기준일에 맞는 단가를 채운다. 이미 단가가 있는 행은 건드리지 않는다.
            if (row != null && gvw1.GetDataRow(e.RowHandle) is { } dataRow)
                _ = SafeExecuteAsync(() => ApplyPriceAsync(dataRow), "판매단가 조회");
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
        if (forceKey == null && string.IsNullOrWhiteSpace(txtSearchQtNo.Text))
        {
            EnterNewMode();
            Toast.Show("조회할 번호가 없어 신규 입력 상태로 전환했습니다.");
            return;
        }
        var p = new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "Q",
            ["p_qt_id"] = forceKey,
            ["p_qt_no"] = forceKey == null ? txtSearchQtNo.Text : null,
        };
        var tables = await QueryMultiAsync("USP_SA_QT_Q", p);
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
            _editingKey = row["qt_id"]?.ToString();
            cboAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            txtQtNo.Text = row["qt_no"]?.ToString() ?? string.Empty;
            txtSearchQtNo.Text = txtQtNo.Text; // 링크로 열었거나 저장 후에도 조회 버튼이 현재 문서를 다시 읽도록
            dteQtDate.YyyyMmDd = row["qt_date"]?.ToString();
            dteValidDate.YyyyMmDd = row["valid_date"]?.ToString();
            txtQtTitle.Text = row["qt_title"]?.ToString() ?? string.Empty;
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
            chkCfmYn.Checked = row["cfm_yn"]?.ToString() == "Y";
            memoRemark.Text = row["remark"]?.ToString() ?? string.Empty;
        });

        var confirmed = chkCfmYn.Checked;
        gvw1.OptionsBehavior.Editable = !confirmed;
        btnAddRow1.Enabled = !confirmed;
        btnDeletRow1.Enabled = !confirmed;

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
            txtQtNo.Text = string.Empty;
            dteQtDate.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");
            dteValidDate.YyyyMmDd = DateTime.Today.AddDays(30).ToString("yyyyMMdd");
            txtQtTitle.Text = string.Empty;
            cboStatCd.EditValue = "0";
            txtCustId.Text = string.Empty;
            txtCustNm.Text = string.Empty;
            txtDeptId.Text = Session.DeptId?.ToString() ?? string.Empty;
            txtDeptNm.Text = Session.DeptNm;
            txtEmpId.Text = Session.EmpId?.ToString() ?? string.Empty;
            txtEmpNm.Text = Session.EmpNm;
            cboCurCd.EditValue = "KRW";
            txtExcRate.Text = "1";
            cboVatType.EditValue = null;
            txtVatRate.Text = "10";
            chkCfmYn.Checked = false;
            memoRemark.Text = string.Empty;

            _detail = _detail.Clone();
            TrackDirty(_detail);
            grd1.DataSource = _detail;

            gvw1.OptionsBehavior.Editable = true;
            btnAddRow1.Enabled = true;
            btnDeletRow1.Enabled = true;
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
            ["p_qt_id"] = _editingKey,
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_qt_date"] = dteQtDate.YyyyMmDd,
            ["p_valid_date"] = dteValidDate.YyyyMmDd,
            ["p_qt_title"] = txtQtTitle.Text,
            ["p_cust_id"] = txtCustId.Text,
            ["p_dept_id"] = txtDeptId.Text,
            ["p_emp_id"] = txtEmpId.Text,
            ["p_cur_cd"] = cboCurCd.EditValue?.ToString(),
            ["p_exc_rate"] = txtExcRate.Text,
            ["p_vat_type"] = cboVatType.EditValue?.ToString(),
            ["p_vat_rate"] = txtVatRate.Text,
            ["p_remark"] = memoRemark.Text,
        };

        var headerResult = await SaveAsync("USP_SA_QT_S", headerParams);
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
                ["p_qt_id"] = headerKey,
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
            };

            var detailResult = await SaveAsync("USP_SA_QT_S_1", detailParams);
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

        var result = await SaveAsync("USP_SA_QT_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = "D",
            ["p_qt_id"] = _editingKey,
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

        var tables = await QueryMultiAsync("USP_SA_QT_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_qt_id"] = "-1",
        });
        if (tables.Count > 1) _detail = tables[1];
        EnterNewMode();
    }

    /// <summary>판매단가(USP_SA_PRICE_Q 'Q1')에서 이 품목/거래처/견적일에 적용되는 단가를 찾아 채운다.
    /// 이미 단가가 들어 있는 행(사용자가 직접 입력)은 덮어쓰지 않는다.</summary>
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
            ["p_base_date"] = dteQtDate.YyyyMmDd,
        });
        if (result.Rows.Count == 0) return;
        if (row.RowState == DataRowState.Detached || row.RowState == DataRowState.Deleted) return;

        row["price"] = result.Rows[0]["price"];
        ApplyAmounts(row);
    }

    /// <summary>견적확정/확정취소 - 확정되면 이 견적은 수정/삭제가 막히고 수주 화면의 "견적 불러오기"
    /// 대상이 된다. 저장 안 된 변경이 있으면 먼저 저장하게 한다.</summary>
    private async Task ConfirmAsync(bool confirm)
    {
        if (_editingKey == null)
        {
            AppMessageBox.Show("먼저 저장한 뒤 확정할 수 있습니다.", "안내");
            return;
        }
        if (IsDirty)
        {
            AppMessageBox.Show("저장하지 않은 변경이 있습니다. 먼저 저장한 뒤 처리하세요.", "안내");
            return;
        }
        if (confirm && AppMessageBox.Show("이 견적을 확정하시겠습니까? 확정 후에는 수정/삭제할 수 없습니다.", "견적확정", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;
        if (!confirm && AppMessageBox.Show("견적확정을 취소하시겠습니까?", "확정취소", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;

        var result = await SaveAsync("USP_SA_QT_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = confirm ? "C" : "CC",
            ["p_qt_id"] = _editingKey,
        });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "처리에 실패했습니다.", confirm ? "확정 실패" : "확정취소 실패");
            return;
        }

        Toast.Show(confirm ? "확정되었습니다." : "확정이 취소되었습니다.");
        await QueryCore(forceKey: _editingKey);
    }

    public override async Task FocusRecordAsync(string key) => await QueryCore(forceKey: key);
}
