using System.Data;
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

/// <summary>
/// 공정실적 - 한 투입 LOT의 공정 결과(TPRRSLTM/TPRRSLTD). "실적 대기 LOT 불러오기"(popPick, USP_PR_RSLTREADYPICK_Q)로 작업지시 공정과
/// 투입 LOT를 고르고, 양품/불량을 입력한다. 투입/산출 단위가 다른 공정(EDS: 웨이퍼 -> Die)은 웨이퍼별 합/부 판정 그리드가 켜지고 그 합계가
/// 양품/불량이 된다(엑셀 불러오기 지원 - 외주처가 FTP로 보낸 파일). 같은 단위 공정(Bumping/Packaging/Final Test)은 양품/불량을 직접 입력.
///
/// 확정(USP_PR_RSLT_C_S)하면 백플러시: 투입 LOT가 투입수량만큼 소진(PR_OUT)되고 양품이 산출 LOT로 외주처 창고에 입고(PR_IN)된다.
/// 공정에 분할수량이 있으면 산출 LOT가 그 수량씩 나뉜다(Packaging 5,000개). 확정취소는 역거래로 되돌리며, 산출 LOT가 이미 다른
/// 공정/이전에 쓰였으면 서버가 막는다. 확정 후에는 산출 LOT 목록이 아래 그리드에 보인다.
/// </summary>
public partial class frmRslt : BaseForm
{
    private DataTable _wafers = new();
    private readonly DevExpress.XtraEditors.Repository.RepositoryItemComboBox _waferCombo = new();
    private DataTable _outLots = new();
    private string? _editingKey; // null이면 신규모드
    private string _statCd = "0";
    private string _inUnit = string.Empty;
    private string _rootLot = string.Empty; // 작업지시 시작 LOT 번호 - 웨이퍼 번호는 "<시작 LOT>-NN"으로 정의(공정을 거쳐 LOT 이름이 바뀌어도 같은 웨이퍼는 같은 이름)
    private string _outUnit = string.Empty;

    /// <summary>투입/산출 단위가 다르면(웨이퍼 -> Die) 웨이퍼별 판정 모드 - 양품/불량은 그리드 합계.</summary>
    private bool WaferMode => !string.IsNullOrEmpty(_inUnit) && !string.Equals(_inUnit, _outUnit, StringComparison.OrdinalIgnoreCase);

    public frmRslt()
    {
        InitializeComponent();

        Text = "공정실적";

        Controls.Add(BuildScreenHeader());

        gvw1.Role = GridRoleWyn.Edit;
        gvw1.HighlightFocusedRow = true;
        gvw1.RowAdd += (s, e) => AddWaferRow();
        gvw1.RowDelete += (s, e) => DeleteWaferRow();
        gvw1.CellValueChanged += Gvw1_CellValueChanged;
        gvw2.HighlightFocusedRow = true;

        // 웨이퍼 번호는 LOT의 투입수량(장수)만큼 01..N 중에서 고른다. 웨이퍼는 별도 마스터가 없어 투입수량이 곧 번호 범위다(엑셀의 다른 표기는 직접 입력도 허용).
        grd1.RepositoryItems.Add(_waferCombo);
        colWaferNo.ColumnEdit = _waferCombo;
        spnInQty.EditValueChanged += (s, e) => RefreshWaferChoices();

        btnAddRow1.Click += (s, e) => AddWaferRow();
        btnDeletRow1.Click += (s, e) => DeleteWaferRow();
        btnLoadReady.Click += async (s, e) => await SafeExecuteAsync(LoadFromReadyAsync, "실적 대기 LOT 불러오기");
        btnImportExcel.Click += (s, e) => SafeExecute(ImportExcel, "엑셀 불러오기");
        btnConfirm.Click += async (s, e) => await SafeExecuteAsync(() => ConfirmAsync(true), "공정실적 확정");
        btnConfirmCancel.Click += async (s, e) => await SafeExecuteAsync(() => ConfirmAsync(false), "공정실적 확정취소");

        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) => await SafeExecuteAsync(EnsureSchemaAsync, "공정실적 화면 초기화");
    }

    private static decimal Dec(object? value) => decimal.TryParse(value?.ToString(), out var d) ? d : 0;

    private static string Fmt(object? value) => value == null || value == DBNull.Value ? string.Empty : Dec(value).ToString("#,##0.####");

    /// <summary>새 폼은 아직 조회한 적이 없어 그리드 테이블에 컬럼이 없다 - 조건에 안 걸리는 조회로 스키마만 먼저 받아 둔다.</summary>
    private async Task EnsureSchemaAsync()
    {
        if (_wafers.Columns.Count > 0) return;

        var tables = await QueryMultiAsync("USP_PR_RSLT_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q", ["p_rslt_id"] = "-1" });
        if (tables.Count > 1) _wafers = tables[1];
        if (tables.Count > 2) _outLots = tables[2];
        EnterNewMode();
    }

    public override async Task QueryClick() => await QueryCore(forceKey: null);

    private async Task QueryCore(string? forceKey)
    {
        // 조회 공통 규칙: 번호를 안 넣은 조회는 서버가 "가장 최근 1건"을 돌려주므로, 조회할 대상이 없는 것으로 보고 신규 모드로 간다.
        if (forceKey == null && string.IsNullOrWhiteSpace(txtSearchRsltNo.Text))
        {
            EnterNewMode();
            Toast.Show("조회할 번호가 없어 신규 입력 상태로 전환했습니다.");
            return;
        }

        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_rslt_id"] = forceKey,
            ["p_rslt_no"] = forceKey == null ? txtSearchRsltNo.Text : null,
        };
        var tables = await QueryMultiAsync("USP_PR_RSLT_Q", p);
        var header = tables.Count > 0 ? tables[0] : new DataTable();
        _wafers = tables.Count > 1 ? tables[1] : new DataTable();
        _outLots = tables.Count > 2 ? tables[2] : new DataTable();

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
            _editingKey = row["rslt_id"]?.ToString();
            _statCd = row["stat_cd"]?.ToString() ?? "0";
            _inUnit = row["in_unit_cd"]?.ToString() ?? string.Empty;
            _rootLot = row["start_lot_no"]?.ToString() ?? string.Empty;
            _outUnit = row["out_unit_cd"]?.ToString() ?? string.Empty;
            cboAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            cboStatCd.EditValue = _statCd;
            txtRsltNo.Text = row["rslt_no"]?.ToString() ?? string.Empty;
            txtSearchRsltNo.Text = txtRsltNo.Text; // 조회 버튼이 현재 문서를 다시 읽도록
            dteRsltDate.YyyyMmDd = row["rslt_date"]?.ToString();
            txtWoId.Text = row["wo_id"]?.ToString() ?? string.Empty;
            txtWoSerl.Text = row["wo_serl"]?.ToString() ?? string.Empty;
            txtWoNo.Text = row["wo_no"]?.ToString() ?? string.Empty;
            txtProcNm.Text = row["proc_nm"]?.ToString() ?? string.Empty;
            txtCustNm.Text = row["cust_nm"]?.ToString() ?? string.Empty;
            txtWhNm.Text = row["wh_nm"]?.ToString() ?? string.Empty;
            txtSrcFileNm.Text = row["src_file_nm"]?.ToString() ?? string.Empty;
            txtInLotId.Text = row["in_lot_id"]?.ToString() ?? string.Empty;
            txtInLotNo.Text = row["in_lot_no"]?.ToString() ?? string.Empty;
            txtInItemNm.Text = row["in_item_nm"]?.ToString() ?? string.Empty;
            txtOutItemNm.Text = row["out_item_nm"]?.ToString() ?? string.Empty;
            spnInQty.EditValue = Dec(row["in_qty"]);
            spnGoodQty.EditValue = Dec(row["good_qty"]);
            spnBadQty.EditValue = Dec(row["bad_qty"]);
            txtYield.Text = row["yield_rate"] == DBNull.Value ? string.Empty : Dec(row["yield_rate"]).ToString("#,##0.##");
            txtSplitQty.Text = Fmt(row["split_qty"]);
            txtUnits.Text = UnitsText();
            memoRemark.Text = row["remark"]?.ToString() ?? string.Empty;

            TrackDirty(_wafers);
            grd1.DataSource = _wafers;
            grd2.DataSource = _outLots;
        });
        ApplyLock();
    }

    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = null;
            _statCd = "0";
            _inUnit = string.Empty;
            _rootLot = string.Empty;
            _outUnit = string.Empty;
            cboAccId.EditValue = Session.AccId?.ToString();
            cboStatCd.EditValue = "0";
            txtRsltNo.Text = string.Empty;
            dteRsltDate.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");
            foreach (var t in new[] { txtWoId, txtWoSerl, txtWoNo, txtProcNm, txtCustNm, txtWhNm, txtSrcFileNm, txtInLotId, txtInLotNo, txtInItemNm, txtOutItemNm,
                                      txtYield, txtSplitQty, txtUnits })
                t.Text = string.Empty;
            spnInQty.EditValue = 0m;
            spnGoodQty.EditValue = 0m;
            spnBadQty.EditValue = 0m;
            memoRemark.Text = string.Empty;

            _wafers = _wafers.Clone();
            _outLots = _outLots.Clone();
            TrackDirty(_wafers);
            grd1.DataSource = _wafers;
            grd2.DataSource = _outLots;
        });
        ApplyLock();
    }

    private string UnitsText() =>
        string.IsNullOrEmpty(_inUnit) ? string.Empty : $"투입 {_inUnit} → 산출 {_outUnit}";

    /// <summary>확정된 실적은 전체가 조회전용. 웨이퍼별 판정 모드에서는 양품/불량이 그리드 합계로만 정해진다.</summary>
    private void ApplyLock()
    {
        var confirmed = _statCd == "C";
        var wafer = WaferMode;

        dteRsltDate.Properties.ReadOnly = confirmed;
        memoRemark.Properties.ReadOnly = confirmed;
        spnInQty.Properties.ReadOnly = confirmed;
        spnGoodQty.Properties.ReadOnly = confirmed || wafer;
        spnBadQty.Properties.ReadOnly = confirmed || wafer;
        gvw1.OptionsBehavior.Editable = !confirmed && wafer;
        grd1.Enabled = wafer;
        btnAddRow1.Enabled = !confirmed && wafer;
        btnDeletRow1.Enabled = !confirmed && wafer;
        btnImportExcel.Enabled = !confirmed && wafer;
        btnLoadReady.Enabled = !confirmed && _editingKey == null;
        btnConfirm.Enabled = !confirmed && _editingKey != null;
        btnConfirmCancel.Enabled = confirmed;
    }

    public override Task NewClick()
    {
        EnterNewMode();
        FocusFirstEntryField(panData); // 사업장 다음 첫 탭오더 컨트롤에 커서(표준)
        return Task.CompletedTask;
    }

    public override Task NewRowClick()
    {
        AddWaferRow();
        return Task.CompletedTask;
    }

    public override Task DeleteRowClick()
    {
        DeleteWaferRow();
        return Task.CompletedTask;
    }

    // ===== 웨이퍼별 판정 그리드 =====

    private void AddWaferRow()
    {
        if (_statCd == "C" || !WaferMode) return;
        gvw1.AddNewRow();

        // 아직 안 쓴 가장 작은 웨이퍼 번호를 미리 넣어 준다(바꾸려면 콤보에서 다시 고르면 됨).
        var used = _wafers.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted).Select(r => r["wafer_no"]?.ToString()).ToHashSet();
        var next = _waferCombo.Items.Cast<string>().FirstOrDefault(n => !used.Contains(n));
        if (next != null && gvw1.FocusedRowHandle != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
            gvw1.SetRowCellValue(gvw1.FocusedRowHandle, colWaferNo, next);
    }

    private void RefreshWaferChoices()
    {
        var n = (int)Math.Min(Dec(spnInQty.EditValue), 999);
        _waferCombo.Items.Clear();
        for (var i = 1; i <= n; i++) _waferCombo.Items.Add(WaferName(i));
    }

    private string WaferName(int i) => string.IsNullOrEmpty(_rootLot) ? i.ToString("00") : $"{_rootLot}-{i:00}";

    /// <summary>엑셀 등에서 "1"/"01"처럼 숫자만 온 웨이퍼 번호는 "<시작 LOT>-NN"으로 맞춘다(이미 LOT가 붙은 값은 그대로).</summary>
    private string NormalizeWafer(string text) => int.TryParse(text, out var n) && n > 0 ? WaferName(n) : text;

    /// <summary>투입수량(장수)만큼 웨이퍼 행(01..N, Good/Bad 0)을 미리 만들어 사용자는 판정 수량만 채우게 한다.</summary>
    private void GenerateWaferRows()
    {
        if (_statCd == "C" || !WaferMode || _wafers.Rows.Count > 0) return;
        var n = (int)Math.Min(Dec(spnInQty.EditValue), 999);
        for (var i = 1; i <= n; i++)
        {
            var nr = _wafers.NewRow();
            nr["wafer_no"] = WaferName(i);
            nr["good_qty"] = 0m;
            nr["bad_qty"] = 0m;
            nr["gross_qty"] = 0m;
            _wafers.Rows.Add(nr);
        }
    }

    private void DeleteWaferRow()
    {
        if (_statCd == "C" || !WaferMode) return;
        try { if (gvw1.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
        catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        RecalcFromWafers();
    }

    private void Gvw1_CellValueChanged(object? sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
    {
        if (e.Value is string text && (e.Column == colGoodQty || e.Column == colBadQty))
        {
            gvw1.SetRowCellValue(e.RowHandle, e.Column,
                decimal.TryParse(text.Replace(",", ""), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var n)
                    ? n : 0m);
        }

        if (e.Column == colGoodQty || e.Column == colBadQty)
        {
            gvw1.SetRowCellValue(e.RowHandle, colGrossQty, Dec(gvw1.GetRowCellValue(e.RowHandle, colGoodQty)) + Dec(gvw1.GetRowCellValue(e.RowHandle, colBadQty)));
            RecalcFromWafers();
        }
    }

    /// <summary>웨이퍼 라인 합계를 양품/불량 수량 칸에 반영한다.</summary>
    private void RecalcFromWafers()
    {
        if (!WaferMode) return;
        decimal good = 0, bad = 0;
        foreach (DataRow r in _wafers.Rows)
        {
            if (r.RowState == DataRowState.Deleted) continue;
            good += Dec(r["good_qty"]);
            bad += Dec(r["bad_qty"]);
        }
        spnGoodQty.EditValue = good;
        spnBadQty.EditValue = bad;
    }

    /// <summary>외주처가 FTP로 보낸 .xlsx를 읽어 웨이퍼 그리드를 통째로 바꾼다. 헤더에서 웨이퍼 번호/Good/Bad 열을 찾고(웨이퍼|wafer, good|양품|합격,
    /// bad|불량|불합격), 못 찾으면 A/B/C열을 그 순서로 본다. 첫 행이 숫자가 아닌 글자면 헤더로 보고 건너뛴다.</summary>
    private void ImportExcel()
    {
        if (_statCd == "C" || !WaferMode) return;

        using var dlg = new OpenFileDialog { Filter = "Excel (*.xlsx)|*.xlsx" };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        using var spreadsheet = new SpreadsheetControl();
        try { spreadsheet.LoadDocument(dlg.FileName); }
        catch (Exception ex)
        {
            AppMessageBox.Show($"엑셀 파일을 여는 중 오류가 발생했습니다.\n{ex.Message}", "불러오기 실패");
            return;
        }

        var sheet = spreadsheet.Document.Worksheets.First();
        var used = sheet.GetUsedRange();

        int waferCol = -1, goodCol = -1, badCol = -1;
        for (var c = used.LeftColumnIndex; c <= used.RightColumnIndex; c++)
        {
            var h = (sheet[used.TopRowIndex, c].DisplayText ?? string.Empty).Trim().ToLowerInvariant();
            if (waferCol < 0 && (h.Contains("wafer") || h.Contains("웨이퍼"))) waferCol = c;
            else if (goodCol < 0 && (h.Contains("good") || h.Contains("양품") || h.Contains("합격")) && !h.Contains("불")) goodCol = c;
            else if (badCol < 0 && (h.Contains("bad") || h.Contains("불량") || h.Contains("불합격"))) badCol = c;
        }
        var headerFound = waferCol >= 0 && goodCol >= 0 && badCol >= 0;
        if (!headerFound)
        {
            waferCol = used.LeftColumnIndex;
            goodCol = used.LeftColumnIndex + 1;
            badCol = used.LeftColumnIndex + 2;
            if (badCol > used.RightColumnIndex)
            {
                AppMessageBox.Show("웨이퍼번호 / Good / Bad 세 개 열이 필요합니다.", "불러오기 실패");
                return;
            }
        }

        var startRow = used.TopRowIndex;
        // 헤더를 못 찾았어도 첫 행 Good 칸이 숫자가 아니면 헤더로 본다
        if (headerFound || !decimal.TryParse((sheet[startRow, goodCol].DisplayText ?? string.Empty).Replace(",", ""), out _)) startRow++;

        var rows = new List<(string wafer, decimal good, decimal bad)>();
        for (var r = startRow; r <= used.BottomRowIndex; r++)
        {
            var wafer = NormalizeWafer((sheet[r, waferCol].DisplayText ?? string.Empty).Trim());
            if (wafer.Length == 0) continue;
            var goodText = (sheet[r, goodCol].DisplayText ?? string.Empty).Replace(",", "").Trim();
            var badText = (sheet[r, badCol].DisplayText ?? string.Empty).Replace(",", "").Trim();
            if (!decimal.TryParse(goodText, out var good) || !decimal.TryParse(badText, out var bad))
            {
                AppMessageBox.Show($"{r + 1}행(웨이퍼 {wafer})의 Good/Bad 값이 숫자가 아닙니다.", "불러오기 실패");
                return;
            }
            rows.Add((wafer, good, bad));
        }
        if (rows.Count == 0)
        {
            AppMessageBox.Show("읽을 데이터 행이 없습니다.", "불러오기 실패");
            return;
        }
        var dup = rows.GroupBy(x => x.wafer, StringComparer.OrdinalIgnoreCase).FirstOrDefault(g => g.Count() > 1);
        if (dup != null)
        {
            AppMessageBox.Show($"웨이퍼 번호 '{dup.Key}'가 엑셀에 중복되어 있습니다.", "불러오기 실패");
            return;
        }

        gvw1.CloseEditor();
        foreach (DataRow old in _wafers.Rows.Cast<DataRow>().ToList()) old.Delete(); // 저장 때 D로 지워지고 아래 신규 행이 N으로 들어간다
        foreach (var (wafer, good, bad) in rows)
        {
            var nr = _wafers.NewRow();
            nr["wafer_no"] = wafer;
            nr["good_qty"] = good;
            nr["bad_qty"] = bad;
            nr["gross_qty"] = good + bad;
            _wafers.Rows.Add(nr);
        }
        txtSrcFileNm.Text = System.IO.Path.GetFileName(dlg.FileName);
        RecalcFromWafers();
        Toast.Show($"웨이퍼 {rows.Count}건을 불러왔습니다. 저장 후 확정하세요.");
    }

    // ===== 대상 선택 / 저장 / 확정 =====

    /// <summary>실적 대기 LOT 불러오기 - 진행 중 작업지시의 공정 중 그 공정 외주처 창고에 투입 LOT 재고가 있는 행 한 건을 고른다. 투입수량 기본값은 그 LOT의 현재고.</summary>
    private async Task LoadFromReadyAsync()
    {
        if (_statCd == "C" || _editingKey != null) return;

        var columns = new[]
        {
            new PickColumn("wo_no", "작업지시", 110), new PickColumn("wo_serl", "순번", 45), new PickColumn("proc_nm", "공정", 90),
            new PickColumn("cust_nm", "외주처", 100), new PickColumn("in_lot_no", "투입 LOT", 130), new PickColumn("in_item_nm", "투입품목", 120),
            new PickColumn("in_unit_cd", "단위", 45), new PickColumn("in_qty", "현재고", 80, true), new PickColumn("out_item_nm", "산출품목", 120),
            new PickColumn("out_unit_cd", "산출단위", 60), new PickColumn("split_qty", "분할수량", 75, true), new PickColumn("wh_nm", "창고", 100),
        };
        var picked = popPick.Pick(this, MenuId, "실적 대기 LOT 불러오기", "USP_PR_RSLTREADYPICK_Q", "작업지시번호", columns, cboAccId.EditValue?.ToString(),
            rows => rows.Count > 1 ? "실적은 LOT 한 건씩 등록합니다. 한 건만 선택하세요." : null,
            emptyHint: "표시할 LOT가 없습니다. ① 작업지시가 '확정' 상태인지 ② 투입 LOT가 그 공정 외주처 창고에 재고로 있는지(웨이퍼는 웨이퍼입고 화면에서 입고) 확인하세요.");
        if (picked == null || picked.Rows.Count == 0) return;

        var r = picked.Rows[0];
        _inUnit = r["in_unit_cd"]?.ToString() ?? string.Empty;
        _rootLot = r["start_lot_no"]?.ToString() ?? string.Empty;
        _outUnit = r["out_unit_cd"]?.ToString() ?? string.Empty;
        txtWoId.Text = r["wo_id"]?.ToString() ?? string.Empty;
        txtWoSerl.Text = r["wo_serl"]?.ToString() ?? string.Empty;
        txtWoNo.Text = r["wo_no"]?.ToString() ?? string.Empty;
        txtProcNm.Text = r["proc_nm"]?.ToString() ?? string.Empty;
        txtCustNm.Text = r["cust_nm"]?.ToString() ?? string.Empty;
        txtWhNm.Text = r["wh_nm"]?.ToString() ?? string.Empty;
        txtInLotId.Text = r["in_lot_id"]?.ToString() ?? string.Empty;
        txtInLotNo.Text = r["in_lot_no"]?.ToString() ?? string.Empty;
        txtInItemNm.Text = r["in_item_nm"]?.ToString() ?? string.Empty;
        txtOutItemNm.Text = r["out_item_nm"]?.ToString() ?? string.Empty;
        txtSplitQty.Text = Fmt(r["split_qty"]);
        txtUnits.Text = UnitsText();
        spnInQty.EditValue = Dec(r["in_qty"]);
        spnGoodQty.EditValue = 0m;
        spnBadQty.EditValue = 0m;
        txtSrcFileNm.Text = string.Empty;

        _wafers = _wafers.Clone();
        TrackDirty(_wafers);
        grd1.DataSource = _wafers;
        ApplyLock();
        GenerateWaferRows();
        await Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        if (_statCd == "C")
        {
            AppMessageBox.Show("확정된 실적은 수정할 수 없습니다. 먼저 확정취소하세요.", "안내");
            return;
        }
        if (string.IsNullOrWhiteSpace(txtWoId.Text) || string.IsNullOrWhiteSpace(txtInLotId.Text))
        {
            AppMessageBox.Show("먼저 '실적 대기 LOT 불러오기'로 작업지시 공정과 투입 LOT를 선택하세요.", "안내");
            return;
        }

        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        var headerParams = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_rslt_id"] = _editingKey,
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_rslt_date"] = dteRsltDate.YyyyMmDd,
            ["p_wo_id"] = txtWoId.Text,
            ["p_wo_serl"] = txtWoSerl.Text,
            ["p_in_lot_id"] = txtInLotId.Text,
            ["p_in_qty"] = spnInQty.EditValue?.ToString(),
            ["p_good_qty"] = spnGoodQty.EditValue?.ToString(),
            ["p_bad_qty"] = spnBadQty.EditValue?.ToString(),
            ["p_src_file_nm"] = txtSrcFileNm.Text,
            ["p_remark"] = memoRemark.Text,
        };
        var headerResult = await SaveAsync("USP_PR_RSLT_S", headerParams);
        if (headerResult == null || !headerResult.Success)
        {
            AppMessageBox.Show(headerResult?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        var headerKey = _editingKey ?? headerResult.GeneratedCode;

        if (WaferMode)
        {
            // 엑셀로 통째로 바꾼 경우 같은 웨이퍼 번호가 D와 N에 같이 있으므로 삭제 -> 수정 -> 추가 순서로 저장한다.
            var ordered = _wafers.Rows.Cast<DataRow>()
                .Where(r => r.RowState != DataRowState.Unchanged)
                .OrderBy(r => r.RowState == DataRowState.Deleted ? 0 : r.RowState == DataRowState.Modified ? 1 : 2)
                .ToList();
            foreach (var row in ordered)
            {
                var workType = row.RowState == DataRowState.Deleted ? "D" : row.RowState == DataRowState.Added ? "N" : "U";
                var version = row.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;

                var detailParams = new Dictionary<string, string?>
                {
                    ["p_work_type"] = workType,
                    ["p_rslt_id"] = headerKey,
                    ["p_serl"] = ProcData.Str(row, "serl", version),
                    ["p_wafer_no"] = ProcData.Str(row, "wafer_no", version),
                    ["p_good_qty"] = ProcData.Str(row, "good_qty", version),
                    ["p_bad_qty"] = ProcData.Str(row, "bad_qty", version),
                    ["p_remark"] = ProcData.Str(row, "remark", version),
                };
                var detailResult = await SaveAsync("USP_PR_RSLT_S_1", detailParams);
                if (detailResult == null || !detailResult.Success)
                {
                    AppMessageBox.Show(detailResult?.Message ?? "웨이퍼 판정 저장에 실패했습니다.", "저장 실패");
                    _editingKey ??= headerResult.GeneratedCode;
                    await QueryCore(forceKey: _editingKey);
                    return;
                }
            }
        }

        _editingKey ??= headerResult.GeneratedCode;
        Toast.Show("저장되었습니다.");
        await QueryCore(forceKey: _editingKey);
    }

    public override async Task DeleteClick()
    {
        if (_editingKey == null) return;

        var result = await SaveAsync("USP_PR_RSLT_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = "D",
            ["p_rslt_id"] = _editingKey,
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

    /// <summary>확정/확정취소 - 미저장 변경이 있으면 서버 상태와 어긋나므로 먼저 저장하게 한다.</summary>
    private async Task ConfirmAsync(bool confirm)
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

        var waferCnt = WaferMode ? _wafers.Rows.Cast<DataRow>().Count(r => r.RowState != DataRowState.Deleted) : 0;
        var waferNote = confirm && WaferMode && waferCnt != (int)Dec(spnInQty.EditValue)
            ? $"\n\n※ 투입 {Dec(spnInQty.EditValue):#,##0.####}장인데 웨이퍼 판정은 {waferCnt}장뿐입니다."
            : string.Empty;
        var msg = confirm
            ? "공정실적을 확정하시겠습니까?" + waferNote + "\n투입 LOT가 소진되고 양품이 산출 LOT로 외주처 창고에 입고됩니다."
            : "공정실적 확정을 취소하시겠습니까?\n역거래 수불이 남고 산출 LOT가 삭제됩니다(이미 다른 공정에서 쓰였으면 취소되지 않습니다).";
        if (AppMessageBox.Show(msg, confirm ? "공정실적 확정" : "공정실적 확정취소", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        var result = await SaveAsync("USP_PR_RSLT_C_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = confirm ? "C" : "CC",
            ["p_rslt_id"] = _editingKey,
        });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "처리에 실패했습니다.", confirm ? "확정 실패" : "확정취소 실패");
            return;
        }

        Toast.Show(confirm ? "확정되었습니다." : "확정이 취소되었습니다.");
        await QueryCore(forceKey: _editingKey);
    }

    /// <summary>다른 화면에서 실적번호로 열 때 호출된다(key=rslt_id 문자열).</summary>
    public override async Task FocusRecordAsync(string key) => await QueryCore(forceKey: key);
}
