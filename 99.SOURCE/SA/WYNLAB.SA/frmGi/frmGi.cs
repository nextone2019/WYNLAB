using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.SA;

/// <summary>
/// 출고(출고)등록 - 고객 기준 출고(Goods Issue) 한 건 = 헤더(고객/선적정보) + 출고 품목(grd1, 확정 거래명세서 라인 1건당 1행, 같은 고객의 여러 명세서 가능)
/// + 선택한 품목의 LOT 상세(grd2, LOT/창고/수량). 품목 행의 출고수량은 LOT 상세 합계(읽기전용).
/// 흐름: 고객 선택 → [명세서 품목 불러오기] → 품목 행 선택 → [출고 LOT 선택](팝업에서 자사창고/외주처를 골라 조회한 재고 LOT를 여러 건) + 수량 → 저장 → [확정].
/// 확정하면 LOT 행마다 그 창고 재고가 차감되고(부족하면 거부) 거래명세서 라인의 출고누계(gi_qty)가 품목 행 수량으로 갱신된다. 확정취소는 그대로 복원.
/// 출고구분(자사창고/외주처 직송)은 LOT 행마다 있다(고른 창고의 종류로 자동 결정) - 한 출고에 자사 LOT와 외주처 직송 LOT가 섞일 수 있다.
/// </summary>
public partial class frmGi : BaseForm
{
    private DataTable _lines = new();
    private DataTable _lots = new();
    private string? _editingKey; // 저장된 출고의 gi_id. null이면 신규모드
    private string _statCd = "0";

    public frmGi()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준(2026-10-03): 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "출고등록";

        Controls.Add(BuildScreenHeader());

        txtCustNm.MapField("cust_id", txtCustId);
        txtDeptNm.MapField("dept_id", txtDeptId);
        txtEmpNm.MapField("emp_id", txtEmpId);
        txtEmpNm.LinkDept(txtDeptNm, txtDeptId); // 담당자 팝업은 선택한 부서 소속만, 담당자를 고르면 부서도 채움

        gvw1.Role = GridRoleWyn.Edit;
        gvw1.HighlightFocusedRow = true;
        gvw1.RowAdd += (s, e) => AppMessageBox.Show("출고 품목은 [명세서 품목 불러오기]로 추가합니다.", "안내");
        gvw1.RowDelete += (s, e) => DeleteLine();
        gvw1.FocusedRowChanged += (s, e) => ApplyLotFilter();

        gvw2.Role = GridRoleWyn.Edit;
        gvw2.HighlightFocusedRow = true;
        gvw2.RowAdd += (s, e) => AppMessageBox.Show("출고 LOT는 [출고 LOT 선택]으로 추가합니다.", "안내");
        gvw2.RowDelete += (s, e) => DeleteLot();
        gvw2.CellValueChanged += Gvw2_CellValueChanged;

        btnPickLine.Click += async (s, e) => await SafeExecuteAsync(PickInvcAsync, "명세서 품목 불러오기");
        btnDelLine.Click += (s, e) => DeleteLine();
        btnPickLot.Click += async (s, e) => await SafeExecuteAsync(PickLotAsync, "출고 LOT 선택");
        btnDelLot.Click += (s, e) => DeleteLot();
        btnConfirm.Click += async (s, e) => await SafeExecuteAsync(() => ChangeStatusAsync(true), "출고 확정");
        btnConfirmCancel.Click += async (s, e) => await SafeExecuteAsync(() => ChangeStatusAsync(false), "출고 확정취소");

        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) => await SafeExecuteAsync(EnsureSchemaAsync, "출고등록 화면 초기화");
    }

    private static decimal Dec(object? value) => decimal.TryParse(value?.ToString(), out var d) ? d : 0;

    private static object Db(object? value) => value == null || (value is string s && s.Length == 0) ? DBNull.Value : value;

    private IEnumerable<DataRow> Live(DataTable table) => table.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted);

    // ===== 조회 / 신규 =====

    /// <summary>새 폼은 아직 조회한 적이 없어 그리드 테이블에 컬럼이 없다 - 조건에 안 걸리는 조회로 스키마만 먼저 받아 둔다.</summary>
    private async Task EnsureSchemaAsync()
    {
        if (_lines.Columns.Count > 0) return;

        var tables = await QueryMultiAsync("USP_SA_GI_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q", ["p_gi_id"] = "-1" });
        if (tables.Count > 1) _lines = tables[1];
        if (tables.Count > 2) _lots = tables[2];
        EnterNewMode();
    }

    public override async Task QueryClick() => await QueryCore(forceKey: null);

    private async Task QueryCore(string? forceKey)
    {
        // 조회 공통 규칙: 번호를 안 넣은 조회는 서버가 "가장 최근 1건"을 돌려주므로, 조회할 대상이 없는 것으로 보고 신규 모드로 간다.
        if (forceKey == null && string.IsNullOrWhiteSpace(txtSearchGiNo.Text))
        {
            EnterNewMode();
            Toast.Show("조회할 번호가 없어 신규 입력 상태로 전환했습니다.");
            return;
        }

        var tables = await QueryMultiAsync("USP_SA_GI_Q", new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "Q",
            ["p_gi_id"] = forceKey,
            ["p_gi_no"] = forceKey == null ? txtSearchGiNo.Text : null,
        });
        var header = tables.Count > 0 ? tables[0] : new DataTable();
        _lines = tables.Count > 1 ? tables[1] : new DataTable();
        _lots = tables.Count > 2 ? tables[2] : new DataTable();

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
        TrackDirty(_lots);
        grd1.DataSource = _lines;
        grd2.DataSource = _lots.DefaultView;
        ApplyLotFilter();
    }

    /// <summary>MAIN/SUB 그리드 관례 - 품목 행이 바뀌면 그 품목의 LOT 행만 보이게 한다(품목 행이 없으면 LOT 그리드도 비운다).</summary>
    private void ApplyLotFilter()
    {
        var serl = gvw1.GetFocusedRow() is DataRowView v ? v.Row["serl"]?.ToString() : null;
        _lots.DefaultView.RowFilter = string.IsNullOrEmpty(serl) ? "1 = 0" : $"serl = {serl}";
    }

    private void OnRowLoaded(DataRow row)
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = row["gi_id"]?.ToString();
            _statCd = row["stat_cd"]?.ToString() ?? "0";
            cboAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            cboStatCd.EditValue = _statCd;
            txtGiNo.Text = row["gi_no"]?.ToString() ?? string.Empty;
            txtSearchGiNo.Text = txtGiNo.Text; // 링크로 열었거나 저장 후에도 조회 버튼이 현재 문서를 다시 읽도록
            dteGiDate.YyyyMmDd = row["gi_date"]?.ToString();
            txtCustId.Text = row["cust_id"]?.ToString() ?? string.Empty;
            txtCustNm.Text = row["cust_nm"]?.ToString() ?? string.Empty;
            dteShipDate.YyyyMmDd = row["ship_date"]?.ToString();
            txtCarrier.Text = row["carrier"]?.ToString() ?? string.Empty;
            txtBlNo.Text = row["bl_no"]?.ToString() ?? string.Empty;
            txtDest.Text = row["dest"]?.ToString() ?? string.Empty;
            txtDeptId.Text = row["dept_id"]?.ToString() ?? string.Empty;
            txtDeptNm.Text = row["dept_nm"]?.ToString() ?? string.Empty;
            txtEmpId.Text = row["emp_id"]?.ToString() ?? string.Empty;
            txtEmpNm.Text = row["emp_nm"]?.ToString() ?? string.Empty;
            memoRemark.Text = row["remark"]?.ToString() ?? string.Empty;

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
            txtGiNo.Text = string.Empty;
            dteGiDate.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");
            txtCustId.Text = string.Empty;
            txtCustNm.Text = string.Empty;
            dteShipDate.YyyyMmDd = null;
            txtCarrier.Text = string.Empty;
            txtBlNo.Text = string.Empty;
            txtDest.Text = string.Empty;
            txtDeptId.Text = Session.DeptId?.ToString() ?? string.Empty;
            txtDeptNm.Text = Session.DeptNm;
            txtEmpId.Text = Session.EmpId?.ToString() ?? string.Empty;
            txtEmpNm.Text = Session.EmpNm;
            memoRemark.Text = string.Empty;

            _lines = _lines.Clone();
            _lots = _lots.Clone();
            BindGrids();
        });
        ApplyLock();
    }

    /// <summary>확정된 출고는 전체가 조회전용.</summary>
    private void ApplyLock()
    {
        var created = _editingKey != null;
        var confirmed = _statCd == "C";

        foreach (var edit in new DevExpress.XtraEditors.BaseEdit[] { dteGiDate, txtCustNm, dteShipDate, txtCarrier, txtBlNo, txtDest, txtDeptNm, txtEmpNm, memoRemark })
            edit.Properties.ReadOnly = confirmed;
        gvw1.OptionsBehavior.Editable = !confirmed;
        gvw2.OptionsBehavior.Editable = !confirmed;
        btnPickLine.Enabled = !confirmed;
        btnDelLine.Enabled = !confirmed;
        btnPickLot.Enabled = !confirmed;
        btnDelLot.Enabled = !confirmed;
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
        AppMessageBox.Show("출고 품목은 [명세서 품목 불러오기], LOT는 [출고 LOT 선택]으로 추가합니다.", "안내");
        return Task.CompletedTask;
    }

    public override Task DeleteRowClick()
    {
        // 툴바의 행삭제는 마지막으로 포커스가 있던 그리드의 행을 지운다(LOT 그리드에 포커스가 있으면 LOT 행, 아니면 품목 행).
        if (gvw2.GridControl.ContainsFocus) DeleteLot(); else DeleteLine();
        return Task.CompletedTask;
    }

    // ===== 출고 품목(수주 라인) / LOT 상세 편집 =====

    /// <summary>LOT 수량에 "1,500"처럼 문자열을 넣으면 숫자로 바꾸고, 품목 행의 출고수량을 LOT 합계로 다시 맞춘다.</summary>
    private void Gvw2_CellValueChanged(object? sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
    {
        if (e.Column != colLtQty) return;
        if (e.Value is string text)
        {
            gvw2.SetRowCellValue(e.RowHandle, colLtQty,
                decimal.TryParse(text.Replace(",", ""), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var n) ? n : 0m);
            return;
        }
        if (gvw2.GetRow(e.RowHandle) is DataRowView lot && FindLine(lot.Row["serl"]) is { } line) SyncLine(line);
    }

    private DataRow? FindLine(object? serl) => Live(_lines).FirstOrDefault(r => r["serl"]?.ToString() == serl?.ToString());

    /// <summary>품목 행의 출고수량/금액 = 그 품목 LOT 행 수량의 합계.</summary>
    private void SyncLine(DataRow line)
    {
        var serl = line["serl"]?.ToString();
        var sum = Live(_lots).Where(r => r["serl"]?.ToString() == serl).Sum(r => Dec(r["qty"]));
        line["qty"] = sum;
        line["amt"] = Math.Round(sum * Dec(line["price"]), 4);
    }

    private static int NextSerl(DataTable table, string column, Func<DataRow, bool>? filter = null)
    {
        var max = 0;
        foreach (DataRow r in table.Rows)
        {
            var version = r.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;
            if (filter != null && r.RowState != DataRowState.Deleted && !filter(r)) continue;
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
            var serl = view.Row["serl"]?.ToString();
            foreach (var lot in Live(_lots).Where(r => r["serl"]?.ToString() == serl).ToList()) lot.Delete(); // 저장 시 서버가 품목 삭제와 함께 LOT 상세를 지운다
            view.Row.Delete();
            ApplyLotFilter();
        }
        catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
    }

    private void DeleteLot()
    {
        if (_statCd == "C") return;
        try
        {
            if (gvw2.GetFocusedRow() is not DataRowView view) return;
            var line = FindLine(view.Row["serl"]);
            view.Row.Delete();
            if (line != null) SyncLine(line);
        }
        catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
    }

    // ===== 수주 품목 / LOT 선택 =====

    /// <summary>명세서 품목 불러오기 - 같은 고객의 확정된 거래명세서 중 출고 잔량이 남은 라인을 여러 건(여러 명세서 포함) 체크해 출고 품목 행으로 가져온다.
    /// 고객이 아직 비어 있으면 고른 명세서의 고객이 헤더에 채워진다. 이미 담긴 명세서 라인은 건너뛴다.</summary>
    private async Task PickInvcAsync()
    {
        if (_statCd == "C") return;

        var columns = new[]
        {
            new PickColumn("invc_no", "명세서번호", 110), new PickColumn("invc_serl", "순번", 45), new PickColumn("invc_date", "명세서일", 80),
            new PickColumn("so_no", "수주번호", 100), new PickColumn("cust_nm", "거래처", 110), new PickColumn("item_no", "품번", 100), new PickColumn("item_nm", "품명", 140),
            new PickColumn("unit_cd", "단위", 50), new PickColumn("qty", "명세서수량", 85, true), new PickColumn("gi_qty", "출고누계", 80, true),
            new PickColumn("reserved_qty", "타 출고 배정", 85, true), new PickColumn("remain_qty", "출고 가능", 80, true), new PickColumn("delv_date", "납기", 80),
        };
        var custId = txtCustId.Text;
        var picked = popPick.Pick(this, MenuId, "명세서 품목 불러오기" + (string.IsNullOrEmpty(custId) ? string.Empty : $" - {txtCustNm.Text}"),
            "USP_SA_GIINVCPICK_Q", "명세서번호", columns, cboAccId.EditValue?.ToString(),
            rows => rows.Select(r => r["cust_id"]?.ToString()).Distinct().Count() > 1 ? "한 출고에는 같은 고객의 명세서 품목만 담을 수 있습니다. 한 고객씩 선택하세요." : null,
            emptyHint: "출고할 수 있는 명세서 품목이 없습니다(확정된 거래명세서에서 확정된 출고와 다른 미확정 출고가 잡은 수량을 뺀 잔량이 남은 라인만 나옵니다).",
            extra: new Dictionary<string, string?> { ["p_cust_id"] = string.IsNullOrEmpty(custId) ? null : custId, ["p_gi_id"] = _editingKey });
        if (picked == null || picked.Rows.Count == 0) return;

        if (string.IsNullOrEmpty(txtCustId.Text))
        {
            txtCustId.Text = picked.Rows[0]["cust_id"]?.ToString() ?? string.Empty;
            txtCustNm.Text = picked.Rows[0]["cust_nm"]?.ToString() ?? string.Empty;
        }

        gvw1.CloseEditor();
        gvw2.CloseEditor();
        var added = 0;
        foreach (DataRow r in picked.Rows)
        {
            if (Live(_lines).Any(x => x["invc_id"]?.ToString() == r["invc_id"]?.ToString() && x["invc_serl"]?.ToString() == r["invc_serl"]?.ToString())) continue;

            var nr = _lines.NewRow();
            nr["serl"] = NextSerl(_lines, "serl");
            nr["invc_id"] = Db(r["invc_id"]);
            nr["invc_no"] = Db(r["invc_no"]);
            nr["invc_serl"] = Db(r["invc_serl"]);
            nr["so_id"] = Db(r["so_id"]);
            nr["so_no"] = Db(r["so_no"]);
            nr["so_serl"] = Db(r["so_serl"]);
            nr["item_id"] = Db(r["item_id"]);
            nr["item_no"] = Db(r["item_no"]);
            nr["item_nm"] = Db(r["item_nm"]);
            nr["unit_cd"] = Db(r["unit_cd"]);
            nr["invc_qty"] = Dec(r["qty"]);
            nr["invc_remain_qty"] = Dec(r["remain_qty"]);
            nr["price"] = Db(r["price"]);
            nr["qty"] = 0m;
            nr["amt"] = 0m;
            _lines.Rows.Add(nr);
            added++;
        }
        Toast.Show(added == 0 ? "이미 담긴 명세서 품목입니다." : $"{added}건을 추가했습니다. 품목 행을 선택하고 [출고 LOT 선택]으로 LOT를 지정하세요.");
        if (added > 0) gvw1.FocusedRowHandle = gvw1.GetRowHandle(_lines.Rows.Count - 1);
        await Task.CompletedTask;
    }

    /// <summary>출고 LOT 선택 - 선택한 품목의 재고가 있는 LOT를 여러 건 체크해 LOT 상세 행으로 추가한다(출고구분에 맞는 창고만: 자사창고 출고 = 외주처 창고 제외,
    /// 외주처 직송 = 외주처 창고만). 수량은 LOT 재고만큼, 단 수주 잔량(같은 수주 라인의 다른 품목 행/이미 담은 LOT 제외)을 넘지 않게 채운다.
    /// 예) 수주 20,000 / 재고 LOT 5,000+5,000+5,000+4,500 → 4개를 한 번에 체크하면 LOT 4행, 품목 출고수량 19,500(수주 잔량 500은 다음 출고에).</summary>
    private async Task PickLotAsync()
    {
        if (_statCd == "C") return;
        if (gvw1.GetFocusedRow() is not DataRowView view)
        {
            AppMessageBox.Show("LOT를 지정할 출고 품목 행을 먼저 선택하세요. ([명세서 품목 불러오기]로 품목을 추가합니다)", "안내");
            return;
        }

        var line = view.Row;
        var columns = new[]
        {
            new PickColumn("ship_kind_nm", "구분", 70), new PickColumn("lot_no", "LOT", 150), new PickColumn("wh_nm", "창고", 110), new PickColumn("stock_qty", "현재고", 80, true),
            new PickColumn("reserved_qty", "타 출고 배정", 90, true), new PickColumn("avail_qty", "가용", 80, true), new PickColumn("reserved_docs", "배정 출고번호", 150),
            new PickColumn("wo_no", "작업지시", 100),
        };
        // 위쪽 '출고구분' 콤보(전체/자사창고/외주처)로 조회 범위를 고른다 - 고른 LOT의 출고구분은 그 창고 종류로 행마다 자동 결정된다.
        var picked = popPick.Pick(this, MenuId, "출고 LOT 선택", "USP_SA_GILOTPICK_Q", "LOT/창고", columns, cboAccId.EditValue?.ToString(), null,
            emptyHint: "출고할 수 있는 재고 LOT가 없습니다. ① 출고구분(자사창고/외주처)을 바꿔 보세요. ② 재고가 있어도 다른 미확정 출고가 이미 모두 잡은 LOT는 나오지 않습니다(출고현황에서 미확정 출고를 확인하세요). ③ 완제품 재고는 공정실적의 Final Test 확정으로 생긴 LOT입니다.",
            extra: new Dictionary<string, string?> { ["p_item_id"] = line["item_id"]?.ToString(), ["p_gi_id"] = _editingKey },
            choices: new[] { new KeyValuePair<string, string>("", "전체"), new KeyValuePair<string, string>("W", "자사창고"), new KeyValuePair<string, string>("D", "외주처") },
            choiceParam: "p_ship_kind", choiceLabel: "출고구분", choiceDefault: "");
        if (picked == null || picked.Rows.Count == 0) return;

        gvw1.CloseEditor();
        gvw2.CloseEditor();

        var serl = line["serl"]?.ToString();
        var otherLines = Live(_lines).Where(r => r != line && r["invc_id"]?.ToString() == line["invc_id"]?.ToString() && r["invc_serl"]?.ToString() == line["invc_serl"]?.ToString())
            .Sum(r => Dec(r["qty"]));
        var mine = Live(_lots).Where(r => r["serl"]?.ToString() == serl).ToList();
        var left = Dec(line["invc_remain_qty"]) - otherLines - mine.Sum(r => Dec(r["qty"]));

        var added = 0;
        var skippedNoAvail = 0;
        foreach (DataRow r in picked.Rows)
        {
            if (mine.Any(m => m["lot_id"]?.ToString() == r["lot_id"]?.ToString() && m["wh_id"]?.ToString() == r["wh_id"]?.ToString())) continue; // 이미 담은 LOT
            // 가용 = 현재고 - 다른 출고가 잡은 수량(서버 계산) - 이 출고의 다른 행(다른 품목 포함)이 이미 잡은 같은 LOT/창고 수량
            var stock = Dec(r["stock_qty"]);
            var usedHere = Live(_lots).Where(m => m["lot_id"]?.ToString() == r["lot_id"]?.ToString() && m["wh_id"]?.ToString() == r["wh_id"]?.ToString()).Sum(m => Dec(m["qty"]));
            var avail = Dec(r["avail_qty"]) - usedHere;
            if (avail <= 0) { skippedNoAvail++; continue; }
            var qty = Math.Min(avail, left);
            if (qty <= 0)
            {
                Toast.Show("명세서 잔량을 모두 채워서 나머지 LOT는 추가하지 않았습니다.");
                break;
            }

            var nr = _lots.NewRow();
            nr["serl"] = line["serl"];
            nr["lot_serl"] = NextSerl(_lots, "lot_serl", x => x["serl"]?.ToString() == serl);
            nr["item_id"] = line["item_id"];
            nr["ship_kind"] = Db(r["ship_kind"]);
            nr["lot_id"] = Db(r["lot_id"]);
            nr["lot_no"] = Db(r["lot_no"]);
            nr["wh_id"] = Db(r["wh_id"]);
            nr["wh_nm"] = Db(r["wh_nm"]);
            nr["stock_qty"] = stock;
            nr["qty"] = qty;
            _lots.Rows.Add(nr);
            left -= qty;
            added++;
        }
        SyncLine(line);
        ApplyLotFilter();
        if (skippedNoAvail > 0) AppMessageBox.Show($"선택한 LOT 중 {skippedNoAvail}건은 다른 출고가 이미 모두 잡아 가용재고가 없어 추가하지 않았습니다. (팝업의 '타 출고 배정/배정 출고번호' 참고)", "안내");
        if (added > 0) Toast.Show($"LOT {added}건을 추가했습니다. (명세서 잔량 {Math.Max(left, 0):#,##0.####} 남음)");
        await Task.CompletedTask;
    }

    // ===== 저장 / 삭제 / 확정 =====

    public override async Task SaveClick()
    {
        if (_statCd == "C")
        {
            AppMessageBox.Show("확정된 출고는 수정할 수 없습니다. 먼저 확정취소하세요.", "안내");
            return;
        }
        if (string.IsNullOrWhiteSpace(txtCustId.Text))
        {
            AppMessageBox.Show("고객을 선택하세요. ([명세서 품목 불러오기]로 명세서를 고르면 고객이 채워집니다)", "안내");
            return;
        }

        gvw1.CloseEditor();
        gvw2.CloseEditor();
        gvw1.UpdateCurrentRow();
        gvw2.UpdateCurrentRow();

        foreach (var lot in Live(_lots))
        {
            var line = FindLine(lot["serl"]);
            var label = line == null ? "?" : $"{line["invc_no"]}-{line["invc_serl"]}";
            if (string.IsNullOrEmpty(lot["wh_id"]?.ToString()))
            {
                AppMessageBox.Show($"품목 {label}의 LOT 행에 출고 창고가 없습니다. [출고 LOT 선택]으로 지정하세요.", "안내");
                return;
            }
            if (Dec(lot["qty"]) <= 0)
            {
                AppMessageBox.Show($"품목 {label}의 LOT '{lot["lot_no"]}' 출고수량을 입력하세요.", "안내");
                return;
            }
        }

        var headerResult = await SaveAsync("USP_SA_GI_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_gi_id"] = _editingKey,
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_gi_date"] = dteGiDate.YyyyMmDd,
            ["p_cust_id"] = txtCustId.Text,
            ["p_ship_date"] = dteShipDate.YyyyMmDd,
            ["p_carrier"] = txtCarrier.Text,
            ["p_bl_no"] = txtBlNo.Text,
            ["p_dest"] = txtDest.Text,
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

        // 품목: 삭제 -> 추가/수정, LOT: 삭제 -> 수정/추가 순서. 삭제한 품목의 LOT 행은 서버가 품목 삭제 때 같이 지우므로 건너뛴다.
        var deletedLines = new HashSet<string>();
        foreach (DataRow row in _lines.Rows.Cast<DataRow>().Where(r => r.RowState == DataRowState.Deleted).ToList())
        {
            var serl = ProcData.Str(row, "serl", DataRowVersion.Original);
            deletedLines.Add(serl ?? string.Empty);
            if (!await SaveLineAsync("D", key, row, DataRowVersion.Original)) return;
        }
        foreach (DataRow row in _lines.Rows.Cast<DataRow>().Where(r => r.RowState == DataRowState.Added || r.RowState == DataRowState.Modified).ToList())
            if (!await SaveLineAsync(row.RowState == DataRowState.Added ? "N" : "U", key, row, DataRowVersion.Current)) return;

        foreach (DataRow row in _lots.Rows.Cast<DataRow>().Where(r => r.RowState == DataRowState.Deleted).ToList())
        {
            if (deletedLines.Contains(ProcData.Str(row, "serl", DataRowVersion.Original) ?? string.Empty)) continue;
            if (!await SaveLotAsync("D", key, row, DataRowVersion.Original)) return;
        }
        foreach (DataRow row in _lots.Rows.Cast<DataRow>().Where(r => r.RowState == DataRowState.Added || r.RowState == DataRowState.Modified).ToList())
            if (!await SaveLotAsync(row.RowState == DataRowState.Added ? "N" : "U", key, row, DataRowVersion.Current)) return;

        Toast.Show("저장되었습니다.");
        await QueryCore(forceKey: key);
    }

    private async Task<bool> SaveLineAsync(string workType, string? key, DataRow row, DataRowVersion version)
    {
        var result = await SaveAsync("USP_SA_GI_S_1", new Dictionary<string, string?>
        {
            ["p_work_type"] = workType,
            ["p_gi_id"] = key,
            ["p_serl"] = ProcData.Str(row, "serl", version),
            ["p_invc_id"] = ProcData.Str(row, "invc_id", version),
            ["p_invc_serl"] = ProcData.Str(row, "invc_serl", version),
            ["p_remark"] = ProcData.Str(row, "remark", version),
        });
        return await CheckSavedAsync(result, key, "출고 품목");
    }

    private async Task<bool> SaveLotAsync(string workType, string? key, DataRow row, DataRowVersion version)
    {
        var result = await SaveAsync("USP_SA_GI_S_2", new Dictionary<string, string?>
        {
            ["p_work_type"] = workType,
            ["p_gi_id"] = key,
            ["p_serl"] = ProcData.Str(row, "serl", version),
            ["p_lot_serl"] = ProcData.Str(row, "lot_serl", version),
            ["p_ship_kind"] = ProcData.Str(row, "ship_kind", version),
            ["p_lot_id"] = ProcData.Str(row, "lot_id", version),
            ["p_lot_no"] = ProcData.Str(row, "lot_no", version),
            ["p_wh_id"] = ProcData.Str(row, "wh_id", version),
            ["p_qty"] = ProcData.Str(row, "qty", version),
            ["p_remark"] = ProcData.Str(row, "remark", version),
        });
        return await CheckSavedAsync(result, key, "출고 LOT");
    }

    /// <summary>한 행이라도 서버가 거부하면 메시지를 보여주고 지금까지 저장된 상태로 다시 조회한다(화면과 서버가 어긋나지 않게).</summary>
    private async Task<bool> CheckSavedAsync(ApiResult? result, string? key, string what)
    {
        if (result != null && result.Success) return true;
        AppMessageBox.Show(result?.Message ?? $"{what} 저장에 실패했습니다.", "저장 실패");
        await QueryCore(forceKey: key);
        return false;
    }

    protected override bool ConfirmDeleteByDefault => false; // 삭제 확인창을 DeleteClick에서 직접 띄움(문서번호 등 상세 문구)

    public override async Task DeleteClick()
    {
        if (_editingKey == null) return;

        var confirm = AppMessageBox.Show($"선택하신 출고를 삭제 하시겠습니까?\n\n[{txtGiNo.Text}] {txtCustNm.Text}",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await SaveAsync("USP_SA_GI_S", new Dictionary<string, string?> { ["p_work_type"] = "D", ["p_gi_id"] = _editingKey });
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
        gvw2.CloseEditor();
        if (IsDirty)
        {
            AppMessageBox.Show("저장하지 않은 변경이 있습니다. 먼저 저장하세요.", "안내");
            return;
        }

        var msg = confirm
            ? "출고를 확정하시겠습니까?\n지정한 LOT의 재고가 차감되고 명세서 출고누계가 갱신됩니다."
            : "출고 확정을 취소하시겠습니까?\n차감된 재고와 명세서 출고누계가 되돌려집니다.";
        if (AppMessageBox.Show(msg, confirm ? "출고 확정" : "출고 확정취소", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        var result = await SaveAsync("USP_SA_GI_C_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = confirm ? "C" : "CC",
            ["p_gi_id"] = _editingKey,
        });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "처리에 실패했습니다.", confirm ? "확정 실패" : "확정취소 실패");
            return;
        }

        Toast.Show(confirm ? "확정되었습니다." : "확정이 취소되었습니다.");
        await QueryCore(forceKey: _editingKey);
    }

    /// <summary>다른 화면(출고현황 등)에서 번호로 열 때 호출된다(key=gi_id 문자열).</summary>
    public override async Task FocusRecordAsync(string key) => await QueryCore(forceKey: key);
}
