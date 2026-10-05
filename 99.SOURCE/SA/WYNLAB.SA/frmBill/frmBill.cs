using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.SA;

/// <summary>
/// 매출등록 - 고객 기준 매출(세금계산서) 한 건 = 헤더(고객/일자/계산서번호) + 매출 품목(grd1, 확정 거래명세서 라인 1건당 1행, 같은 고객의 여러 명세서 가능 - 월합계 계산서).
/// 수량만 조정하면 부분매출이 되고(명세서 잔량 이하), 단가/부가세율은 명세서 값을 그대로 쓴다(수정 불가). 금액은 저장할 때 서버가 계산한다.
/// 확정된 명세서면 출고 여부와 무관하게 매출을 등록할 수 있다(출고상태는 참고용 컬럼). 확정하면 명세서 라인의 매출누계(bill_qty)가 갱신된다.
/// </summary>
public partial class frmBill : BaseForm
{
    private DataTable _lines = new();
    private string? _editingKey; // 저장된 매출의 bill_id. null이면 신규모드
    private string _statCd = "0";

    public frmBill()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준(2026-10-03): 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "매출등록";

        Controls.Add(BuildScreenHeader());

        txtCustNm.MapField("cust_id", txtCustId);
        txtDeptNm.MapField("dept_id", txtDeptId);
        txtEmpNm.MapField("emp_id", txtEmpId);
        txtEmpNm.LinkDept(txtDeptNm, txtDeptId); // 담당자 팝업은 선택한 부서 소속만, 담당자를 고르면 부서도 채움

        gvw1.Role = GridRoleWyn.Edit;
        gvw1.HighlightFocusedRow = true;
        gvw1.RowAdd += (s, e) => AppMessageBox.Show("매출 품목은 [명세서 품목 불러오기]로 추가합니다.", "안내");
        gvw1.RowDelete += (s, e) => DeleteLine();
        gvw1.CellValueChanged += Gvw1_CellValueChanged;

        btnPickLine.Click += async (s, e) => await SafeExecuteAsync(PickInvcAsync, "명세서 품목 불러오기");
        btnDelLine.Click += (s, e) => DeleteLine();
        btnConfirm.Click += async (s, e) => await SafeExecuteAsync(() => ChangeStatusAsync(true), "매출 확정");
        btnConfirmCancel.Click += async (s, e) => await SafeExecuteAsync(() => ChangeStatusAsync(false), "매출 확정취소");

        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) => await SafeExecuteAsync(EnsureSchemaAsync, "매출등록 화면 초기화");
    }

    private static decimal Dec(object? value) => decimal.TryParse(value?.ToString(), out var d) ? d : 0;

    private static object Db(object? value) => value == null || (value is string s && s.Length == 0) ? DBNull.Value : value;

    private IEnumerable<DataRow> Live(DataTable table) => table.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted);

    // ===== 조회 / 신규 =====

    /// <summary>새 폼은 아직 조회한 적이 없어 그리드 테이블에 컬럼이 없다 - 조건에 안 걸리는 조회로 스키마만 먼저 받아 둔다.</summary>
    private async Task EnsureSchemaAsync()
    {
        if (_lines.Columns.Count > 0) return;

        var tables = await QueryMultiAsync("USP_SA_BILL_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q", ["p_bill_id"] = "-1" });
        if (tables.Count > 1) _lines = tables[1];
        EnterNewMode();
    }

    public override async Task QueryClick() => await QueryCore(forceKey: null);

    private async Task QueryCore(string? forceKey)
    {
        // 조회 공통 규칙: 번호를 안 넣은 조회는 서버가 "가장 최근 1건"을 돌려주므로, 조회할 대상이 없는 것으로 보고 신규 모드로 간다.
        if (forceKey == null && string.IsNullOrWhiteSpace(txtSearchBillNo.Text))
        {
            EnterNewMode();
            Toast.Show("조회할 번호가 없어 신규 입력 상태로 전환했습니다.");
            return;
        }

        var tables = await QueryMultiAsync("USP_SA_BILL_Q", new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "Q",
            ["p_bill_id"] = forceKey,
            ["p_bill_no"] = forceKey == null ? txtSearchBillNo.Text : null,
        });
        var header = tables.Count > 0 ? tables[0] : new DataTable();
        _lines = tables.Count > 1 ? tables[1] : new DataTable();

        if (header.Rows.Count > 0) OnRowLoaded(header.Rows[0]);
        else
        {
            EnterNewMode();
            Toast.Show("조회 결과가 없어 신규 입력 상태로 전환했습니다.");
        }
    }

    private void BindGrids()
    {
        TrackDirty(_lines);
        grd1.DataSource = _lines;
    }

    private void OnRowLoaded(DataRow row)
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = row["bill_id"]?.ToString();
            _statCd = row["stat_cd"]?.ToString() ?? "0";
            cboAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            cboStatCd.EditValue = _statCd;
            txtBillNo.Text = row["bill_no"]?.ToString() ?? string.Empty;
            txtSearchBillNo.Text = txtBillNo.Text; // 링크로 열었거나 저장 후에도 조회 버튼이 현재 문서를 다시 읽도록
            dteBillDate.YyyyMmDd = row["bill_date"]?.ToString();
            txtCustId.Text = row["cust_id"]?.ToString() ?? string.Empty;
            txtCustNm.Text = row["cust_nm"]?.ToString() ?? string.Empty;
            txtTaxInvNo.Text = row["tax_inv_no"]?.ToString() ?? string.Empty;
            dteTaxInvDate.YyyyMmDd = row["tax_inv_date"]?.ToString();
            txtDeptId.Text = row["dept_id"]?.ToString() ?? string.Empty;
            txtDeptNm.Text = row["dept_nm"]?.ToString() ?? string.Empty;
            txtEmpId.Text = row["emp_id"]?.ToString() ?? string.Empty;
            txtEmpNm.Text = row["emp_nm"]?.ToString() ?? string.Empty;
            memoRemark.Text = row["remark"]?.ToString() ?? string.Empty;
            ShowCurrency(row);

            BindGrids();
        });
        ApplyLock();
    }

    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = null;
            _statCd = "0";
            cboAccId.EditValue = Session.AccId?.ToString();
            cboStatCd.EditValue = "0";
            txtBillNo.Text = string.Empty;
            dteBillDate.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");
            txtCustId.Text = string.Empty;
            txtCustNm.Text = string.Empty;
            txtTaxInvNo.Text = string.Empty;
            dteTaxInvDate.YyyyMmDd = null;
            txtDeptId.Text = Session.DeptId?.ToString() ?? string.Empty;
            txtDeptNm.Text = Session.DeptNm;
            txtEmpId.Text = Session.EmpId?.ToString() ?? string.Empty;
            txtEmpNm.Text = Session.EmpNm;
            memoRemark.Text = string.Empty;
            ShowCurrency(null);

            _lines = _lines.Clone();
            BindGrids();
        });
        ApplyLock();
    }

    /// <summary>확정된 매출은 전체가 조회전용.</summary>
    /// <summary>헤더의 통화/환율/합계 표시 - 서버가 라인 기준으로 채운 값을 보여 준다(조회 전용). 문서가 없으면 비운다.</summary>
    private void ShowCurrency(DataRow? r)
    {
        string S(string c) => r != null && r.Table.Columns.Contains(c) ? r[c]?.ToString() ?? string.Empty : string.Empty;
        string N(string c) => S(c).Length > 0 && decimal.TryParse(S(c), out var d) ? d.ToString("#,##0.####") : string.Empty;
        txtCurCd.Text = S("cur_cd");
        txtExcRate.Text = N("exc_rate");
        txtAmt.Text = N("amt");
        txtVat.Text = N("vat");
        txtTotalAmt.Text = N("total_amt");
        txtKorTotalAmt.Text = N("kor_total_amt");
    }

    private void ApplyLock()
    {
        var created = _editingKey != null;
        var confirmed = _statCd == "C";

        foreach (var edit in new DevExpress.XtraEditors.BaseEdit[] { dteBillDate, txtCustNm, txtTaxInvNo, dteTaxInvDate, txtDeptNm, txtEmpNm, memoRemark })
            edit.Properties.ReadOnly = confirmed;
        // 환율은 외화 매출일 때만 직접 고칠 수 있다(원화는 1 고정, 확정되면 조회전용). 기본값은 매출일 기준 환율정보.
        txtExcRate.Properties.ReadOnly = confirmed || string.IsNullOrEmpty(txtCurCd.Text) || txtCurCd.Text == "KRW";
        gvw1.OptionsBehavior.Editable = !confirmed;
        btnPickLine.Enabled = !confirmed;
        btnDelLine.Enabled = !confirmed;
        btnConfirm.Enabled = created && !confirmed;
        btnConfirmCancel.Enabled = created && confirmed;
    }

    public override Task NewClick()
    {
        EnterNewMode();
        FocusFirstEntryField(panData); // 사업장 다음 첫 탭오더 컨트롤에 커서(표준)
        return Task.CompletedTask;
    }

    public override Task NewRowClick()
    {
        AppMessageBox.Show("매출 품목은 [명세서 품목 불러오기]로 추가합니다.", "안내");
        return Task.CompletedTask;
    }

    public override Task DeleteRowClick()
    {
        DeleteLine();
        return Task.CompletedTask;
    }

    // ===== 품목 편집 =====

    /// <summary>매출수량에 "1,500"처럼 문자열을 넣으면 숫자로 바꾸고, 금액을 다시 계산해 보여준다(저장 때 서버가 같은 식으로 다시 계산).</summary>
    private void Gvw1_CellValueChanged(object? sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
    {
        if (e.Column != colQty) return;
        if (e.Value is string text)
        {
            gvw1.SetRowCellValue(e.RowHandle, e.Column,
                decimal.TryParse(text.Replace(",", ""), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var n) ? n : 0m);
            return;
        }
        if (gvw1.GetRow(e.RowHandle) is DataRowView view) Recalc(view.Row);
    }

    private static void Recalc(DataRow row)
    {
        var amt = Math.Round(Dec(row["qty"]) * Dec(row["price"]), 4);
        var vat = Math.Round(amt * Dec(row["vat_rate"]) / 100, 4);
        row["amt"] = amt;
        row["vat"] = vat;
        row["total_amt"] = amt + vat;
    }

    private static int NextSerl(DataTable table, string column)
    {
        var max = 0;
        foreach (DataRow r in table.Rows)
        {
            var version = r.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;
            if (int.TryParse(r[column, version]?.ToString(), out var v) && v > max) max = v;
        }
        return max + 1;
    }

    private void DeleteLine()
    {
        if (_statCd == "C") return;
        try
        {
            if (gvw1.GetFocusedRow() is not DataRowView view) return;
            view.Row.Delete();
        }
        catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
    }

    // ===== 명세서 품목 선택 =====

    /// <summary>명세서 품목 불러오기 - 확정된 거래명세서 중 매출 잔량이 남은 라인을 여러 건(여러 명세서 포함) 체크해 매출 품목 행으로 가져온다.
    /// 수량은 잔량 전부로 시작하고 줄이면 부분매출이 된다. 고객이 아직 비어 있으면 고른 명세서의 고객이 헤더에 채워진다.</summary>
    private async Task PickInvcAsync()
    {
        if (_statCd == "C") return;

        var columns = new[]
        {
            new PickColumn("invc_no", "명세서번호", 110), new PickColumn("invc_serl", "순번", 45), new PickColumn("invc_date", "명세서일", 80),
            new PickColumn("cust_nm", "거래처", 110), new PickColumn("so_no", "수주번호", 100), new PickColumn("item_no", "품번", 100), new PickColumn("item_nm", "품명", 140),
            new PickColumn("unit_cd", "단위", 50), new PickColumn("qty", "명세서수량", 85, true), new PickColumn("bill_qty", "매출누계", 80, true),
            new PickColumn("reserved_qty", "타 매출 배정", 90, true), new PickColumn("remain_qty", "매출 가능", 80, true), new PickColumn("price", "단가", 80, true),
            new PickColumn("gi_status", "출고상태", 80),
        };
        var custId = txtCustId.Text;
        var picked = popPick.Pick(this, MenuId, "명세서 품목 불러오기" + (string.IsNullOrEmpty(custId) ? string.Empty : $" - {txtCustNm.Text}"),
            "USP_SA_BILLINVCPICK_Q", "명세서번호", columns, cboAccId.EditValue?.ToString(),
            rows => rows.Select(r => r["cust_id"]?.ToString()).Distinct().Count() > 1 ? "한 매출에는 같은 고객의 명세서 품목만 담을 수 있습니다. 한 고객씩 선택하세요." : null,
            emptyHint: "매출로 등록할 수 있는 명세서 품목이 없습니다(확정된 거래명세서에서, 확정된 매출과 다른 미확정 매출이 잡은 수량을 뺀 잔량이 남은 라인만 나옵니다).",
            extra: new Dictionary<string, string?> { ["p_cust_id"] = string.IsNullOrEmpty(custId) ? null : custId, ["p_bill_id"] = _editingKey });
        if (picked == null || picked.Rows.Count == 0) return;

        if (string.IsNullOrEmpty(txtCustId.Text))
        {
            txtCustId.Text = picked.Rows[0]["cust_id"]?.ToString() ?? string.Empty;
            txtCustNm.Text = picked.Rows[0]["cust_nm"]?.ToString() ?? string.Empty;
        }

        gvw1.CloseEditor();
        var added = 0;
        foreach (DataRow r in picked.Rows)
        {
            if (Live(_lines).Any(x => x["invc_id"]?.ToString() == r["invc_id"]?.ToString() && x["invc_serl"]?.ToString() == r["invc_serl"]?.ToString())) continue;

            var nr = _lines.NewRow();
            nr["serl"] = NextSerl(_lines, "serl");
            nr["invc_id"] = Db(r["invc_id"]);
            nr["invc_no"] = Db(r["invc_no"]);
            nr["invc_serl"] = Db(r["invc_serl"]);
            nr["so_no"] = Db(r["so_no"]);
            nr["item_id"] = Db(r["item_id"]);
            nr["item_no"] = Db(r["item_no"]);
            nr["item_nm"] = Db(r["item_nm"]);
            nr["unit_cd"] = Db(r["unit_cd"]);
            nr["invc_qty"] = Dec(r["qty"]);
            nr["invc_remain_qty"] = Dec(r["remain_qty"]);
            nr["qty"] = Dec(r["remain_qty"]);
            nr["price"] = Dec(r["price"]);
            nr["vat_rate"] = Dec(r["vat_rate"]);
            nr["cur_cd"] = Db(r["cur_cd"]);
            nr["gi_status"] = Db(r["gi_status"]);
            Recalc(nr);
            _lines.Rows.Add(nr);
            added++;
        }
        Toast.Show(added == 0 ? "이미 담긴 명세서 품목입니다." : $"{added}건을 추가했습니다. 부분매출이면 매출수량을 줄인 뒤 저장하세요.");
        if (added > 0) gvw1.FocusedRowHandle = gvw1.GetRowHandle(_lines.Rows.Count - 1);
        await Task.CompletedTask;
    }

    // ===== 저장 / 삭제 / 확정 =====

    public override async Task SaveClick()
    {
        if (_statCd == "C")
        {
            AppMessageBox.Show("확정된 매출은 수정할 수 없습니다. 먼저 확정취소하세요.", "안내");
            return;
        }
        if (string.IsNullOrWhiteSpace(txtCustId.Text))
        {
            AppMessageBox.Show("고객을 선택하세요. ([명세서 품목 불러오기]로 명세서를 고르면 고객이 채워집니다)", "안내");
            return;
        }

        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        foreach (var line in Live(_lines))
        {
            if (Dec(line["qty"]) <= 0)
            {
                AppMessageBox.Show($"품목 {line["invc_no"]}-{line["invc_serl"]}의 매출수량을 입력하세요.", "안내");
                return;
            }
        }

        // 외화 매출에서 환율을 직접 고쳤으면 그 값을 보낸다(비우면 서버가 매출일 기준 환율을 쓴다/유지한다).
        string? excRate = !txtExcRate.Properties.ReadOnly && decimal.TryParse(txtExcRate.Text.Replace(",", string.Empty), out var ex) && ex > 0
            ? ex.ToString(System.Globalization.CultureInfo.InvariantCulture) : null;

        var headerResult = await SaveAsync("USP_SA_BILL_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_bill_id"] = _editingKey,
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_bill_date"] = dteBillDate.YyyyMmDd,
            ["p_cust_id"] = txtCustId.Text,
            ["p_tax_inv_no"] = txtTaxInvNo.Text,
            ["p_tax_inv_date"] = dteTaxInvDate.YyyyMmDd,
            ["p_exc_rate"] = excRate,
            ["p_dept_id"] = txtDeptId.Text,
            ["p_emp_id"] = txtEmpId.Text,
            ["p_remark"] = memoRemark.Text,
        });
        if (headerResult == null || !headerResult.Success)
        {
            AppMessageBox.Show(headerResult?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        var key = _editingKey ?? headerResult.GeneratedCode;
        _editingKey ??= key;

        // 삭제 -> 추가/수정 순서(삭제로 풀린 명세서 잔량을 다른 행이 쓸 수 있게)
        foreach (DataRow row in _lines.Rows.Cast<DataRow>().Where(r => r.RowState == DataRowState.Deleted).ToList())
            if (!await SaveLineAsync("D", key, row, DataRowVersion.Original)) return;
        foreach (DataRow row in _lines.Rows.Cast<DataRow>().Where(r => r.RowState == DataRowState.Added || r.RowState == DataRowState.Modified).ToList())
            if (!await SaveLineAsync(row.RowState == DataRowState.Added ? "N" : "U", key, row, DataRowVersion.Current)) return;

        Toast.Show("저장되었습니다.");
        await QueryCore(forceKey: key);
    }

    private async Task<bool> SaveLineAsync(string workType, string? key, DataRow row, DataRowVersion version)
    {
        var result = await SaveAsync("USP_SA_BILL_S_1", new Dictionary<string, string?>
        {
            ["p_work_type"] = workType,
            ["p_bill_id"] = key,
            ["p_serl"] = ProcData.Str(row, "serl", version),
            ["p_invc_id"] = ProcData.Str(row, "invc_id", version),
            ["p_invc_serl"] = ProcData.Str(row, "invc_serl", version),
            ["p_qty"] = ProcData.Str(row, "qty", version),
            ["p_remark"] = ProcData.Str(row, "remark", version),
        });
        if (result != null && result.Success) return true;

        // 한 행이라도 서버가 거부하면 메시지를 보여주고 지금까지 저장된 상태로 다시 조회한다(화면과 서버가 어긋나지 않게).
        AppMessageBox.Show(result?.Message ?? "매출 품목 저장에 실패했습니다.", "저장 실패");
        await QueryCore(forceKey: key);
        return false;
    }

    protected override bool ConfirmDeleteByDefault => false; // 삭제 확인창을 DeleteClick에서 직접 띄움(문서번호 등 상세 문구)

    public override async Task DeleteClick()
    {
        if (_editingKey == null) return;

        var confirm = AppMessageBox.Show($"선택하신 매출을 삭제 하시겠습니까?\n\n[{txtBillNo.Text}] {txtCustNm.Text}",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await SaveAsync("USP_SA_BILL_S", new Dictionary<string, string?> { ["p_work_type"] = "D", ["p_bill_id"] = _editingKey });
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
    private async Task ChangeStatusAsync(bool confirm)
    {
        if (_editingKey == null)
        {
            AppMessageBox.Show("먼저 저장한 뒤 " + (confirm ? "확정" : "확정취소") + "하세요.", "안내");
            return;
        }
        gvw1.CloseEditor();
        if (IsDirty)
        {
            AppMessageBox.Show("저장하지 않은 변경이 있습니다. 먼저 저장하세요.", "안내");
            return;
        }

        var msg = confirm
            ? "매출을 확정하시겠습니까?\n확정하면 수정할 수 없고 명세서의 매출누계가 갱신됩니다."
            : "매출 확정을 취소하시겠습니까?\n명세서의 매출누계가 되돌려집니다.";
        if (AppMessageBox.Show(msg, confirm ? "매출 확정" : "매출 확정취소", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        var result = await SaveAsync("USP_SA_BILL_C_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = confirm ? "C" : "CC",
            ["p_bill_id"] = _editingKey,
        });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "처리에 실패했습니다.", confirm ? "확정 실패" : "확정취소 실패");
            return;
        }

        Toast.Show(confirm ? "확정되었습니다." : "확정이 취소되었습니다.");
        await QueryCore(forceKey: _editingKey);
    }

    /// <summary>다른 화면(매출현황 등)에서 번호로 열 때 호출된다(key=bill_id 문자열).</summary>
    public override async Task FocusRecordAsync(string key) => await QueryCore(forceKey: key);
}
