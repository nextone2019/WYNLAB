using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Report;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.SA;

/// <summary>
/// 거래명세서등록 - 고객 기준 거래명세서 한 건 = 헤더(고객/일자) + 명세서 품목(grd1, 확정 수주 라인 1건당 1행, 같은 고객의 여러 수주 가능).
/// 수량/단가/부가세율은 수정할 수 있고(수주 단가로 시작) 금액은 저장할 때 서버가 계산한다. 실제 출고와 무관하게 발행할 수 있는 서류다.
/// 흐름: 고객 선택 → [수주 품목 불러오기] → 수량/단가 조정 → 저장 → [확정](수주 라인의 명세서누계 next_qty 갱신, 이후 수정 불가).
/// 확정된 명세서는 출고등록(수불)과 매출등록(세금계산서)의 기준이 된다. 출고/매출이 하나라도 걸려 있으면 확정취소할 수 없다.
/// </summary>
public partial class frmInvc : BaseForm
{
    private DataTable _lines = new();
    private string? _editingKey; // 저장된 명세서의 invc_id. null이면 신규모드
    private string _statCd = "0";

    public frmInvc()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준(2026-10-03): 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "거래명세서등록";

        Controls.Add(BuildScreenHeader());

        txtCustNm.MapField("cust_id", txtCustId);
        txtDeptNm.MapField("dept_id", txtDeptId);
        txtEmpNm.MapField("emp_id", txtEmpId);
        txtEmpNm.LinkDept(txtDeptNm, txtDeptId); // 담당자 팝업은 선택한 부서 소속만, 담당자를 고르면 부서도 채움

        gvw1.Role = GridRoleWyn.Edit;
        gvw1.HighlightFocusedRow = true;
        gvw1.RowAdd += (s, e) => AppMessageBox.Show("명세서 품목은 [수주 품목 불러오기]로 추가합니다.", "안내");
        gvw1.RowDelete += (s, e) => DeleteLine();
        gvw1.CellValueChanged += Gvw1_CellValueChanged;

        btnPickLine.Click += async (s, e) => await SafeExecuteAsync(PickSoAsync, "수주 품목 불러오기");
        btnDelLine.Click += (s, e) => DeleteLine();
        btnConfirm.Click += async (s, e) => await SafeExecuteAsync(() => ChangeStatusAsync(true), "거래명세서 확정");
        btnConfirmCancel.Click += async (s, e) => await SafeExecuteAsync(() => ChangeStatusAsync(false), "거래명세서 확정취소");

        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) => await SafeExecuteAsync(EnsureSchemaAsync, "거래명세서등록 화면 초기화");
    }

    private static decimal Dec(object? value) => decimal.TryParse(value?.ToString(), out var d) ? d : 0;

    private static object Db(object? value) => value == null || (value is string s && s.Length == 0) ? DBNull.Value : value;

    private IEnumerable<DataRow> Live(DataTable table) => table.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted);

    // ===== 조회 / 신규 =====

    /// <summary>새 폼은 아직 조회한 적이 없어 그리드 테이블에 컬럼이 없다 - 조건에 안 걸리는 조회로 스키마만 먼저 받아 둔다.</summary>
    private async Task EnsureSchemaAsync()
    {
        if (_lines.Columns.Count > 0) return;

        var tables = await QueryMultiAsync("USP_SA_INVC_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q", ["p_invc_id"] = "-1" });
        if (tables.Count > 1) _lines = tables[1];
        EnterNewMode();
    }

    public override async Task QueryClick() => await QueryCore(forceKey: null);

    private async Task QueryCore(string? forceKey)
    {
        // 조회 공통 규칙: 번호를 안 넣은 조회는 서버가 "가장 최근 1건"을 돌려주므로, 조회할 대상이 없는 것으로 보고 신규 모드로 간다.
        if (forceKey == null && string.IsNullOrWhiteSpace(txtSearchInvcNo.Text))
        {
            EnterNewMode();
            Toast.Show("조회할 번호가 없어 신규 입력 상태로 전환했습니다.");
            return;
        }

        var tables = await QueryMultiAsync("USP_SA_INVC_Q", new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "Q",
            ["p_invc_id"] = forceKey,
            ["p_invc_no"] = forceKey == null ? txtSearchInvcNo.Text : null,
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
            _editingKey = row["invc_id"]?.ToString();
            _statCd = row["stat_cd"]?.ToString() ?? "0";
            cboAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            cboStatCd.EditValue = _statCd;
            txtInvcNo.Text = row["invc_no"]?.ToString() ?? string.Empty;
            txtSearchInvcNo.Text = txtInvcNo.Text; // 링크로 열었거나 저장 후에도 조회 버튼이 현재 문서를 다시 읽도록
            dteInvcDate.YyyyMmDd = row["invc_date"]?.ToString();
            txtCustId.Text = row["cust_id"]?.ToString() ?? string.Empty;
            txtCustNm.Text = row["cust_nm"]?.ToString() ?? string.Empty;
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
            txtInvcNo.Text = string.Empty;
            dteInvcDate.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");
            txtCustId.Text = string.Empty;
            txtCustNm.Text = string.Empty;
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

    /// <summary>확정된 거래명세서는 전체가 조회전용.</summary>
    private void ApplyLock()
    {
        var created = _editingKey != null;
        var confirmed = _statCd == "C";

        foreach (var edit in new DevExpress.XtraEditors.BaseEdit[] { dteInvcDate, txtCustNm, txtDeptNm, txtEmpNm, memoRemark })
            edit.Properties.ReadOnly = confirmed;
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
        AppMessageBox.Show("명세서 품목은 [수주 품목 불러오기]로 추가합니다.", "안내");
        return Task.CompletedTask;
    }

    public override Task DeleteRowClick()
    {
        DeleteLine();
        return Task.CompletedTask;
    }

    // ===== 품목 편집 =====

    /// <summary>수량/단가/세율에 "1,500"처럼 문자열을 넣으면 숫자로 바꾸고, 바뀐 행의 금액을 다시 계산해 보여준다(저장 때 서버가 같은 식으로 다시 계산).</summary>
    private void Gvw1_CellValueChanged(object? sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
    {
        if (e.Column != colQty && e.Column != colPrice && e.Column != colVatRate) return;
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

    // ===== 수주 품목 선택 =====

    /// <summary>수주 품목 불러오기 - 확정(결재완료)된 수주 중 명세서 잔량이 남은 라인을 여러 건(여러 수주 포함) 체크해 명세서 품목 행으로 가져온다.
    /// 고객이 아직 비어 있으면 고른 수주의 고객이 헤더에 채워진다. 수량은 잔량으로, 단가는 수주 단가로 시작한다(둘 다 수정 가능).</summary>
    private async Task PickSoAsync()
    {
        if (_statCd == "C") return;

        var columns = new[]
        {
            new PickColumn("so_no", "수주번호", 110), new PickColumn("so_serl", "순번", 45), new PickColumn("so_date", "수주일", 80),
            new PickColumn("cust_nm", "거래처", 110), new PickColumn("item_no", "품번", 100), new PickColumn("item_nm", "품명", 140),
            new PickColumn("unit_cd", "단위", 50), new PickColumn("qty", "수주수량", 80, true), new PickColumn("next_qty", "명세서누계", 85, true),
            new PickColumn("reserved_qty", "타 명세서 배정", 95, true), new PickColumn("remain_qty", "발행 가능", 80, true), new PickColumn("price", "수주단가", 80, true),
            new PickColumn("delv_date", "납기", 80),
        };
        var custId = txtCustId.Text;
        var picked = popPick.Pick(this, MenuId, "수주 품목 불러오기" + (string.IsNullOrEmpty(custId) ? string.Empty : $" - {txtCustNm.Text}"),
            "USP_SA_INVCSOPICK_Q", "수주번호", columns, cboAccId.EditValue?.ToString(),
            rows => rows.Select(r => r["cust_id"]?.ToString()).Distinct().Count() > 1 ? "한 거래명세서에는 같은 고객의 수주 품목만 담을 수 있습니다. 한 고객씩 선택하세요." : null,
            emptyHint: "명세서로 발행할 수 있는 수주 품목이 없습니다(확정(결재완료)되고 마감되지 않은 수주에서, 확정된 명세서와 다른 미확정 명세서가 잡은 수량을 뺀 잔량이 남은 라인만 나옵니다).",
            extra: new Dictionary<string, string?> { ["p_cust_id"] = string.IsNullOrEmpty(custId) ? null : custId, ["p_invc_id"] = _editingKey });
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
            if (Live(_lines).Any(x => x["so_id"]?.ToString() == r["so_id"]?.ToString() && x["so_serl"]?.ToString() == r["so_serl"]?.ToString())) continue;

            var nr = _lines.NewRow();
            nr["serl"] = NextSerl(_lines, "serl");
            nr["so_id"] = Db(r["so_id"]);
            nr["so_no"] = Db(r["so_no"]);
            nr["so_serl"] = Db(r["so_serl"]);
            nr["item_id"] = Db(r["item_id"]);
            nr["item_no"] = Db(r["item_no"]);
            nr["item_nm"] = Db(r["item_nm"]);
            nr["unit_cd"] = Db(r["unit_cd"]);
            nr["so_qty"] = Dec(r["qty"]);
            nr["so_remain_qty"] = Dec(r["remain_qty"]);
            nr["qty"] = Dec(r["remain_qty"]);
            nr["price"] = Dec(r["price"]);
            nr["vat_rate"] = Dec(r["vat_rate"]);
            nr["cur_cd"] = Db(r["cur_cd"]);
            nr["gi_qty"] = 0m;
            nr["bill_qty"] = 0m;
            Recalc(nr);
            _lines.Rows.Add(nr);
            added++;
        }
        Toast.Show(added == 0 ? "이미 담긴 수주 품목입니다." : $"{added}건을 추가했습니다. 수량/단가를 조정한 뒤 저장하세요.");
        if (added > 0) gvw1.FocusedRowHandle = gvw1.GetRowHandle(_lines.Rows.Count - 1);
        await Task.CompletedTask;
    }

    // ===== 출력 =====

    /// <summary>Shell 툴바의 [출력] - 공통 출력 엔진(WYNLAB.Report)의 거래 문서 양식으로 거래명세서 미리보기/인쇄/PDF. 화면 값이 아니라 저장된 값(USP_SA_INVC_P)을 찍는다.
    /// 공급자(사업장) 정보/직인과 공급받는자(고객) 정보는 프로시저가 FN_RPT_ACC/FN_RPT_CUST로 채워 주고, 이 화면은 컬럼 구성과 제목만 정한다.</summary>
    public override async Task PrintClick() => await SafeExecuteAsync(PrintAsync, "거래명세서 출력");

    private async Task PrintAsync()
    {
        if (_editingKey == null)
        {
            AppMessageBox.Show("먼저 저장한 뒤 출력하세요.", "안내");
            return;
        }
        gvw1.CloseEditor();
        if (IsDirty)
        {
            AppMessageBox.Show("저장하지 않은 변경이 있습니다. 먼저 저장하세요. (출력은 저장된 내용 기준입니다)", "안내");
            return;
        }

        var tables = await QueryMultiAsync("USP_SA_INVC_P", new Dictionary<string, string?> { ["p_work_type"] = "P", ["p_invc_id"] = _editingKey });
        if (tables.Count < 2 || tables[0].Rows.Count == 0)
        {
            AppMessageBox.Show("출력할 거래명세서를 찾을 수 없습니다.", "안내");
            return;
        }
        var h = tables[0].Rows[0];
        var lines = tables[1];
        var date = h["invc_date"]?.ToString() ?? string.Empty;

        var model = new TradeDocModel
        {
            Title = "거 래 명 세 서",
            DocNoLabel = "명세서번호", DocNo = h["invc_no"]?.ToString() ?? string.Empty,
            DateLabel = "명세서일자", Date = date.Length == 8 ? $"{date.Substring(0, 4)}-{date.Substring(4, 2)}-{date.Substring(6, 2)}" : date,
            Left = TradeParty.FromRow(h, "sup", "공급자"),
            Right = TradeParty.FromRow(h, "rcv", "공급받는자"),
            Lines = lines,
            Remark = h["remark"]?.ToString(),
            FooterLeft = h["sup_nm"]?.ToString(),
            Watermark = h["stat_cd"]?.ToString() == "C" ? null : "작성중", // 확정 전 문서는 거래 서류로 쓰지 못하게 표시
        };
        model.CopyLabels.Add("공급받는자 보관용");
        model.CopyLabels.Add("공급자 보관용");
        model.Notes.Add("※ 본 명세서는 거래 사실을 확인하는 서류입니다.");

        model.Columns.Add(new TradeColumn("No", "serl", 28, TradeAlign.Center));
        model.Columns.Add(new TradeColumn("품번", "item_no", 78));
        model.Columns.Add(new TradeColumn("품명", "item_nm", 120));
        model.Columns.Add(new TradeColumn("규격", "item_spec", 80));
        model.Columns.Add(new TradeColumn("단위", "unit_cd", 34, TradeAlign.Center));
        model.Columns.Add(new TradeColumn("수량", "qty", 58, TradeAlign.Right, "#,##0.####"));
        model.Columns.Add(new TradeColumn("단가", "price", 62, TradeAlign.Right, "#,##0.####"));
        model.Columns.Add(new TradeColumn("공급가액", "amt", 82, TradeAlign.Right, "#,##0.##"));
        model.Columns.Add(new TradeColumn("부가세", "vat", 66, TradeAlign.Right, "#,##0.##"));

        model.AddSumTotals("#,##0.##", ("공급가액", "amt"), ("부가세", "vat"), ("합계금액", "total_amt"));
        var cur = lines.Rows.Count > 0 ? lines.Rows[0]["cur_cd"]?.ToString() : null;
        model.SummaryLabel = string.IsNullOrEmpty(cur) ? "합계금액" : $"합계금액 ({cur})";
        model.SummaryValue = model.Totals[model.Totals.Count - 1].Value;

        ReportKit.Preview(model, this);
    }

    // ===== 저장 / 삭제 / 확정 =====

    public override async Task SaveClick()
    {
        if (_statCd == "C")
        {
            AppMessageBox.Show("확정된 거래명세서는 수정할 수 없습니다. 먼저 확정취소하세요.", "안내");
            return;
        }
        if (string.IsNullOrWhiteSpace(txtCustId.Text))
        {
            AppMessageBox.Show("고객을 선택하세요. ([수주 품목 불러오기]로 수주를 고르면 고객이 채워집니다)", "안내");
            return;
        }

        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        foreach (var line in Live(_lines))
        {
            if (Dec(line["qty"]) <= 0)
            {
                AppMessageBox.Show($"품목 {line["so_no"]}-{line["so_serl"]}의 명세서수량을 입력하세요.", "안내");
                return;
            }
        }

        var headerResult = await SaveAsync("USP_SA_INVC_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_invc_id"] = _editingKey,
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_invc_date"] = dteInvcDate.YyyyMmDd,
            ["p_cust_id"] = txtCustId.Text,
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

        // 삭제 -> 추가/수정 순서(삭제로 풀린 수주 잔량을 다른 행이 쓸 수 있게)
        foreach (DataRow row in _lines.Rows.Cast<DataRow>().Where(r => r.RowState == DataRowState.Deleted).ToList())
            if (!await SaveLineAsync("D", key, row, DataRowVersion.Original)) return;
        foreach (DataRow row in _lines.Rows.Cast<DataRow>().Where(r => r.RowState == DataRowState.Added || r.RowState == DataRowState.Modified).ToList())
            if (!await SaveLineAsync(row.RowState == DataRowState.Added ? "N" : "U", key, row, DataRowVersion.Current)) return;

        Toast.Show("저장되었습니다.");
        await QueryCore(forceKey: key);
    }

    private async Task<bool> SaveLineAsync(string workType, string? key, DataRow row, DataRowVersion version)
    {
        var result = await SaveAsync("USP_SA_INVC_S_1", new Dictionary<string, string?>
        {
            ["p_work_type"] = workType,
            ["p_invc_id"] = key,
            ["p_serl"] = ProcData.Str(row, "serl", version),
            ["p_so_id"] = ProcData.Str(row, "so_id", version),
            ["p_so_serl"] = ProcData.Str(row, "so_serl", version),
            ["p_qty"] = ProcData.Str(row, "qty", version),
            ["p_price"] = ProcData.Str(row, "price", version),
            ["p_vat_rate"] = ProcData.Str(row, "vat_rate", version),
            ["p_remark"] = ProcData.Str(row, "remark", version),
        });
        if (result != null && result.Success) return true;

        // 한 행이라도 서버가 거부하면 메시지를 보여주고 지금까지 저장된 상태로 다시 조회한다(화면과 서버가 어긋나지 않게).
        AppMessageBox.Show(result?.Message ?? "명세서 품목 저장에 실패했습니다.", "저장 실패");
        await QueryCore(forceKey: key);
        return false;
    }

    protected override bool ConfirmDeleteByDefault => false; // 삭제 확인창을 DeleteClick에서 직접 띄움(문서번호 등 상세 문구)

    public override async Task DeleteClick()
    {
        if (_editingKey == null) return;

        var confirm = AppMessageBox.Show($"선택하신 거래명세서를 삭제 하시겠습니까?\n\n[{txtInvcNo.Text}] {txtCustNm.Text}",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await SaveAsync("USP_SA_INVC_S", new Dictionary<string, string?> { ["p_work_type"] = "D", ["p_invc_id"] = _editingKey });
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
            ? "거래명세서를 확정하시겠습니까?\n확정하면 수정할 수 없고 수주의 명세서누계가 갱신됩니다."
            : "거래명세서 확정을 취소하시겠습니까?\n수주의 명세서누계가 되돌려집니다. (출고/매출이 등록되어 있으면 취소할 수 없습니다)";
        if (AppMessageBox.Show(msg, confirm ? "거래명세서 확정" : "거래명세서 확정취소", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        var result = await SaveAsync("USP_SA_INVC_C_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = confirm ? "C" : "CC",
            ["p_invc_id"] = _editingKey,
        });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "처리에 실패했습니다.", confirm ? "확정 실패" : "확정취소 실패");
            return;
        }

        Toast.Show(confirm ? "확정되었습니다." : "확정이 취소되었습니다.");
        await QueryCore(forceKey: _editingKey);
    }

    /// <summary>다른 화면(거래명세서현황 등)에서 번호로 열 때 호출된다(key=invc_id 문자열).</summary>
    public override async Task FocusRecordAsync(string key) => await QueryCore(forceKey: key);
}
