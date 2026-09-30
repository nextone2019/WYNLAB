using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

/// <summary>
/// 외주이전 - 외주처 간(Amkor -> ITEK 등) LOT 이동 문서(TPRXFERM/TPRXFERD). 반도체 후공정처럼 공정마다 외주처가 바뀌어 자재가 자사를 거치지
/// 않고 직접 넘어가는 구간을 "출발 확인 -> 도착 확인 -> (차이 있으면) 차이 처리"로 통제한다. 재고는 항상 자사 소유이고 위치만 옮겨진다:
/// 출발 외주처 창고 -> 이동중 재고 -> 도착 외주처 창고. 도착 수량이 출발보다 적으면 차이가 이동중 재고에 남고, 귀책(출발처/도착처/운송/기타)과
/// 처리방법(손실/재입고)을 정해 정리한다(정리 전까지 이전은 "도착(차이대기)" 상태).
///
/// 흐름: [이전 대상 LOT 불러오기](popPick, USP_PR_XFERREADYPICK_Q)로 같은 작업지시/공정 구간의 LOT를 골라 저장(상태 지시) -> 출발 확인(이동중) ->
/// 도착 수량 입력 후 도착 확인 -> 차이 처리. 각 단계는 취소 가능(출발 취소는 도착 확인 전, 도착 취소는 차이 처리 전). 역이전(N/R의 R)은
/// 프로시저는 지원하지만 이 화면은 정방향만 만든다.
/// </summary>
public partial class frmXfer : BaseForm
{
    private DataTable _lines = new();
    private string? _editingKey; // null이면 신규모드
    private string _statCd = "0";

    public frmXfer()
    {
        InitializeComponent();

        Text = "외주이전";

        Controls.Add(BuildScreenHeader());

        gvw1.Role = GridRoleWyn.Edit;
        gvw1.HighlightFocusedRow = true;
        gvw1.RowAdd += (s, e) => AppMessageBox.Show("이전 LOT는 '이전 대상 LOT 불러오기'로만 추가할 수 있습니다.", "안내");
        gvw1.RowDelete += (s, e) => DeleteFocusedRow();
        gvw1.CellValueChanged += Gvw1_CellValueChanged;
        gvw1.ShowingEditor += Gvw1_ShowingEditor;

        btnDeletRow1.Click += (s, e) => DeleteFocusedRow();
        btnLoadLot.Click += async (s, e) => await SafeExecuteAsync(LoadFromReadyAsync, "이전 대상 LOT 불러오기");
        btnOut.Click += async (s, e) => await SafeExecuteAsync(() => StepAsync("OUT", "출발 확인", "출발 외주처 창고에서 이동중 재고로 옮깁니다."), "출발 확인");
        btnOutCancel.Click += async (s, e) => await SafeExecuteAsync(() => StepAsync("OUTCC", "출발 취소", "이동중 재고를 출발 창고로 되돌립니다."), "출발 취소");
        btnIn.Click += async (s, e) => await SafeExecuteAsync(ArriveAsync, "도착 확인");
        btnInCancel.Click += async (s, e) => await SafeExecuteAsync(() => StepAsync("INCC", "도착 취소", "도착 창고 재고를 이동중 재고로 되돌립니다."), "도착 취소");
        btnDiff.Click += async (s, e) => await SafeExecuteAsync(() => DiffAsync(cancel: false), "차이 처리");
        btnDiffCancel.Click += async (s, e) => await SafeExecuteAsync(() => DiffAsync(cancel: true), "차이 처리 취소");

        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) => await SafeExecuteAsync(EnsureSchemaAsync, "외주이전 화면 초기화");
    }

    private static decimal Dec(object? value) => decimal.TryParse(value?.ToString(), out var d) ? d : 0;

    private bool IsDraft => _statCd == "0";

    /// <summary>새 폼은 아직 조회한 적이 없어 그리드 테이블에 컬럼이 없다 - 조건에 안 걸리는 조회로 스키마만 먼저 받아 둔다.</summary>
    private async Task EnsureSchemaAsync()
    {
        if (_lines.Columns.Count > 0) return;

        var tables = await QueryMultiAsync("USP_PR_XFER_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q", ["p_xfer_id"] = "-1" });
        if (tables.Count > 1) _lines = tables[1];
        EnterNewMode();
    }

    /// <summary>상태에 따라 셀 편집을 막는다 - 지시 상태: 출발수량/비고, 이동중: 도착수량, 도착(차이대기): 차이가 남은 행의 귀책/처리.</summary>
    private void Gvw1_ShowingEditor(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        var col = gvw1.FocusedColumn;
        var row = gvw1.GetFocusedDataRow();
        if (col == null || row == null) return;

        bool allow = true;
        if (col == colOutQty || col == colRemark) allow = IsDraft;
        else if (col == colInQty) allow = _statCd == "1";
        else if (col == colDiffResp || col == colDiffAct)
            allow = _statCd == "2" && Dec(row["diff_qty"]) > 0 && string.IsNullOrEmpty(row["diff_act_cd"]?.ToString());
        if (!allow) e.Cancel = true;
    }

    private void Gvw1_CellValueChanged(object? sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
    {
        if (e.Value is string text && (e.Column == colOutQty || e.Column == colInQty))
        {
            gvw1.SetRowCellValue(e.RowHandle, e.Column,
                decimal.TryParse(text.Replace(",", ""), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var n)
                    ? n : (object)DBNull.Value);
        }
    }

    private void DeleteFocusedRow()
    {
        if (!IsDraft) return;
        try { if (gvw1.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
        catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
    }

    public override async Task QueryClick() => await QueryCore(forceKey: null);

    private async Task QueryCore(string? forceKey)
    {
        // 조회 공통 규칙: 번호를 안 넣은 조회는 서버가 "가장 최근 1건"을 돌려주므로, 조회할 대상이 없는 것으로 보고 신규 모드로 간다.
        if (forceKey == null && string.IsNullOrWhiteSpace(txtSearchXferNo.Text))
        {
            EnterNewMode();
            Toast.Show("조회할 번호가 없어 신규 입력 상태로 전환했습니다.");
            return;
        }

        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_xfer_id"] = forceKey,
            ["p_xfer_no"] = forceKey == null ? txtSearchXferNo.Text : null,
        };
        var tables = await QueryMultiAsync("USP_PR_XFER_Q", p);
        var header = tables.Count > 0 ? tables[0] : new DataTable();
        _lines = tables.Count > 1 ? tables[1] : new DataTable();

        if (header.Rows.Count > 0) OnRowLoaded(header.Rows[0]);
        else
        {
            EnterNewMode();
            Toast.Show("조회 결과가 없어 신규 입력 상태로 전환했습니다.");
        }
    }

    private static string StepText(object? serl, object? cust) =>
        string.IsNullOrEmpty(serl?.ToString()) ? string.Empty : $"{serl}번 공정 · {cust}";

    private void OnRowLoaded(DataRow row)
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = row["xfer_id"]?.ToString();
            _statCd = row["stat_cd"]?.ToString() ?? "0";
            cboAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            cboStatCd.EditValue = _statCd;
            txtXferNo.Text = row["xfer_no"]?.ToString() ?? string.Empty;
            txtSearchXferNo.Text = txtXferNo.Text; // 조회 버튼이 현재 문서를 다시 읽도록
            dteXferDate.YyyyMmDd = row["xfer_date"]?.ToString();
            cboKind.EditValue = row["xfer_kind"]?.ToString() ?? "N";
            txtWoId.Text = row["wo_id"]?.ToString() ?? string.Empty;
            txtWoNo.Text = row["wo_no"]?.ToString() ?? string.Empty;
            txtFromSerl.Text = row["from_serl"]?.ToString() ?? string.Empty;
            txtToSerl.Text = row["to_serl"]?.ToString() ?? string.Empty;
            txtFrom.Text = StepText(row["from_serl"], row["from_cust_nm"]);
            txtTo.Text = StepText(row["to_serl"], row["to_cust_nm"]);
            txtOutDt.Text = row["out_dt"] is DateTime od ? od.ToString("yyyy-MM-dd HH:mm") : string.Empty;
            txtInDt.Text = row["in_dt"] is DateTime idt ? idt.ToString("yyyy-MM-dd HH:mm") : string.Empty;
            memoRemark.Text = row["remark"]?.ToString() ?? string.Empty;

            TrackDirty(_lines);
            grd1.DataSource = _lines;
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
            txtXferNo.Text = string.Empty;
            dteXferDate.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");
            cboKind.EditValue = "N";
            foreach (var t in new[] { txtWoId, txtWoNo, txtFromSerl, txtToSerl, txtFrom, txtTo, txtOutDt, txtInDt })
                t.Text = string.Empty;
            memoRemark.Text = string.Empty;

            _lines = _lines.Clone();
            TrackDirty(_lines);
            grd1.DataSource = _lines;
        });
        ApplyLock();
    }

    /// <summary>상태별 잠금: 지시(0)만 헤더/LOT 편집, 이동중(1)은 도착수량 입력, 도착(2)/완료(E)는 차이 처리, 취소(X)는 조회전용.</summary>
    private void ApplyLock()
    {
        var created = _editingKey != null;

        dteXferDate.Properties.ReadOnly = !IsDraft;
        memoRemark.Properties.ReadOnly = !IsDraft;
        btnDeletRow1.Enabled = IsDraft;
        btnLoadLot.Enabled = IsDraft;

        btnOut.Enabled = created && IsDraft;
        btnOutCancel.Enabled = created && _statCd == "1";
        btnIn.Enabled = created && _statCd == "1";
        btnInCancel.Enabled = created && (_statCd == "2" || _statCd == "E");
        btnDiff.Enabled = created && _statCd == "2";
        btnDiffCancel.Enabled = created && (_statCd == "2" || _statCd == "E");
    }

    public override Task NewClick()
    {
        EnterNewMode();
        FocusFirstEntryField(panData); // 사업장 다음 첫 탭오더 컨트롤에 커서(표준)
        return Task.CompletedTask;
    }

    /// <summary>이전 대상 LOT 불러오기 - 같은 작업지시/같은 공정 구간(출발->도착)의 LOT만 한 문서로 묶는다. 이미 담긴 LOT는 건너뛰고, 기본 출발수량은 그 LOT 현재고 전체.</summary>
    private async Task LoadFromReadyAsync()
    {
        if (!IsDraft) return;
        gvw1.CloseEditor();

        var columns = new[]
        {
            new PickColumn("wo_no", "작업지시", 110), new PickColumn("from_serl", "출발", 45), new PickColumn("from_proc_nm", "출발 공정", 90),
            new PickColumn("from_cust_nm", "출발 외주처", 100), new PickColumn("to_serl", "도착", 45), new PickColumn("to_proc_nm", "도착 공정", 90),
            new PickColumn("to_cust_nm", "도착 외주처", 100), new PickColumn("lot_no", "LOT", 130), new PickColumn("item_nm", "품명", 120),
            new PickColumn("unit_cd", "단위", 45), new PickColumn("stock_qty", "현재고", 85, true),
        };
        var picked = popPick.Pick(this, MenuId, "이전 대상 LOT 불러오기", "USP_PR_XFERREADYPICK_Q", "작업지시번호", columns, cboAccId.EditValue?.ToString(),
            rows =>
            {
                var first = rows[0];
                return rows.Any(r => r["wo_id"]?.ToString() != first["wo_id"]?.ToString()
                                  || r["from_serl"]?.ToString() != first["from_serl"]?.ToString()
                                  || r["to_serl"]?.ToString() != first["to_serl"]?.ToString())
                    ? "같은 작업지시, 같은 공정 구간(출발->도착)의 LOT만 한 번에 불러올 수 있습니다." : null;
            },
            emptyHint: "표시할 LOT가 없습니다. ① 작업지시가 '확정' 상태인지 ② 앞 공정 실적을 확정해서 산출 LOT가 그 공정 외주처 창고에 재고로 있는지 확인하세요.");
        if (picked == null || picked.Rows.Count == 0) return;

        var first = picked.Rows[0];
        var newWo = first["wo_id"]?.ToString();
        var newFrom = first["from_serl"]?.ToString();
        var newTo = first["to_serl"]?.ToString();

        var hasHeader = !string.IsNullOrEmpty(txtWoId.Text);
        if (hasHeader && (txtWoId.Text != newWo || txtFromSerl.Text != newFrom || txtToSerl.Text != newTo))
        {
            if (_lines.Rows.Cast<DataRow>().Any(r => r.RowState != DataRowState.Deleted))
            {
                AppMessageBox.Show("이미 다른 작업지시/공정 구간의 LOT가 담겨 있습니다. 새 이전 문서에서 불러오세요.", "안내");
                return;
            }
        }
        if (_editingKey != null && hasHeader && (txtWoId.Text != newWo || txtFromSerl.Text != newFrom || txtToSerl.Text != newTo))
        {
            AppMessageBox.Show("저장된 이전 문서와 다른 공정 구간의 LOT는 담을 수 없습니다.", "안내");
            return;
        }

        txtWoId.Text = newWo ?? string.Empty;
        txtWoNo.Text = first["wo_no"]?.ToString() ?? string.Empty;
        txtFromSerl.Text = newFrom ?? string.Empty;
        txtToSerl.Text = newTo ?? string.Empty;
        txtFrom.Text = StepText(first["from_serl"], first["from_cust_nm"]);
        txtTo.Text = StepText(first["to_serl"], first["to_cust_nm"]);

        var skipped = 0;
        foreach (DataRow r in picked.Rows)
        {
            var lotId = r["lot_id"]?.ToString();
            var dup = _lines.Rows.Cast<DataRow>().Any(d => d.RowState != DataRowState.Deleted && d["lot_id"]?.ToString() == lotId);
            if (dup) { skipped++; continue; }

            var nr = _lines.NewRow();
            nr["item_id"] = r["item_id"];
            nr["item_no"] = r["item_no"];
            nr["item_nm"] = r["item_nm"];
            nr["lot_id"] = r["lot_id"];
            nr["lot_no"] = r["lot_no"];
            nr["unit_cd"] = r["unit_cd"];
            nr["out_qty"] = Dec(r["stock_qty"]);
            _lines.Rows.Add(nr);
        }
        if (skipped > 0) Toast.Show($"이미 담긴 LOT {skipped}건은 건너뛰었습니다.");
        await Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        if (_statCd == "1")
        {
            // 이동중 상태에서 저장 = 도착 수량 입력값만 서버에 남겨 둔다(도착 확인은 별도 버튼).
            gvw1.CloseEditor();
            gvw1.UpdateCurrentRow();
            if (!await SaveArrivalQtyAsync()) return;
            Toast.Show("도착 수량이 저장되었습니다.");
            await QueryCore(forceKey: _editingKey);
            return;
        }
        if (!IsDraft)
        {
            AppMessageBox.Show("출발 확인된 이전은 수정할 수 없습니다. (이동중: 도착 수량 입력, 도착 후: 차이 처리)", "안내");
            return;
        }
        if (string.IsNullOrWhiteSpace(txtWoId.Text))
        {
            AppMessageBox.Show("먼저 '이전 대상 LOT 불러오기'로 작업지시와 공정 구간, LOT를 선택하세요.", "안내");
            return;
        }

        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        foreach (DataRow row in _lines.Rows)
        {
            if (row.RowState == DataRowState.Deleted || row.RowState == DataRowState.Unchanged) continue;
            if (Dec(row["out_qty"]) <= 0)
            {
                AppMessageBox.Show($"'{row["lot_no"]}' 출발 수량을 0보다 크게 입력하세요.", "안내");
                return;
            }
        }

        var headerParams = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_xfer_id"] = _editingKey,
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_xfer_date"] = dteXferDate.YyyyMmDd,
            ["p_xfer_kind"] = "N",
            ["p_wo_id"] = txtWoId.Text,
            ["p_from_serl"] = txtFromSerl.Text,
            ["p_to_serl"] = txtToSerl.Text,
            ["p_remark"] = memoRemark.Text,
        };
        var headerResult = await SaveAsync("USP_PR_XFER_S", headerParams);
        if (headerResult == null || !headerResult.Success)
        {
            AppMessageBox.Show(headerResult?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        var headerKey = _editingKey ?? headerResult.GeneratedCode;

        foreach (DataRow row in _lines.Rows.Cast<DataRow>().ToList())
        {
            if (row.RowState == DataRowState.Unchanged) continue;

            var workType = row.RowState == DataRowState.Deleted ? "D" : row.RowState == DataRowState.Added ? "N" : "U";
            var version = row.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;

            var detailParams = new Dictionary<string, string?>
            {
                ["p_work_type"] = workType,
                ["p_xfer_id"] = headerKey,
                ["p_serl"] = ProcData.Str(row, "serl", version),
                ["p_lot_id"] = ProcData.Str(row, "lot_id", version),
                ["p_out_qty"] = ProcData.Str(row, "out_qty", version),
                ["p_remark"] = ProcData.Str(row, "remark", version),
            };
            var detailResult = await SaveAsync("USP_PR_XFER_S_1", detailParams);
            if (detailResult == null || !detailResult.Success)
            {
                AppMessageBox.Show(detailResult?.Message ?? "LOT 저장에 실패했습니다.", "저장 실패");
                _editingKey ??= headerResult.GeneratedCode;
                await QueryCore(forceKey: _editingKey);
                return;
            }
        }

        _editingKey ??= headerResult.GeneratedCode;
        Toast.Show("저장되었습니다.");
        await QueryCore(forceKey: _editingKey);
    }

    /// <summary>이동중 상태에서 사용자가 입력한 도착 수량(in_qty)을 서버에 남긴다. 비운 행은 그대로(도착 확인 때 전량 도착으로 처리).</summary>
    private async Task<bool> SaveArrivalQtyAsync()
    {
        foreach (DataRow row in _lines.Rows.Cast<DataRow>().ToList())
        {
            if (row.RowState != DataRowState.Modified) continue;
            var result = await SaveAsync("USP_PR_XFER_S_1", new Dictionary<string, string?>
            {
                ["p_work_type"] = "I",
                ["p_xfer_id"] = _editingKey,
                ["p_serl"] = ProcData.Str(row, "serl", DataRowVersion.Current),
                ["p_in_qty"] = ProcData.Str(row, "in_qty", DataRowVersion.Current),
            });
            if (result == null || !result.Success)
            {
                AppMessageBox.Show(result?.Message ?? "도착 수량 저장에 실패했습니다.", "저장 실패");
                return false;
            }
        }
        return true;
    }

    public override async Task DeleteClick()
    {
        if (_editingKey == null) return;

        var result = await SaveAsync("USP_PR_XFER_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = "D",
            ["p_xfer_id"] = _editingKey,
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

    /// <summary>출발 확인/출발 취소/도착 취소 - 미저장 변경이 있으면 서버 상태와 어긋나므로 먼저 저장하게 한다.</summary>
    private async Task StepAsync(string workType, string caption, string desc)
    {
        if (_editingKey == null)
        {
            AppMessageBox.Show("먼저 저장한 뒤 " + caption + "하세요.", "안내");
            return;
        }
        gvw1.CloseEditor();
        if (IsDirty)
        {
            AppMessageBox.Show("저장하지 않은 변경이 있습니다. 먼저 저장하세요.", "안내");
            return;
        }
        if (AppMessageBox.Show($"{caption} 처리하시겠습니까?\n{desc}", caption, MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        var result = await SaveAsync("USP_PR_XFER_C_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = workType,
            ["p_xfer_id"] = _editingKey,
        });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "처리에 실패했습니다.", caption + " 실패");
            return;
        }

        Toast.Show(caption + " 처리되었습니다.");
        await QueryCore(forceKey: _editingKey);
    }

    /// <summary>도착 확인 - 그리드에 입력한 도착 수량을 먼저 서버에 남기고 확인한다. 비운 LOT는 전량 도착. 차이가 있으면 도착(차이대기) 상태가 된다.</summary>
    private async Task ArriveAsync()
    {
        if (_editingKey == null || _statCd != "1") return;
        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        var msg = "도착 확인하시겠습니까?\n도착 수량을 입력하지 않은 LOT는 전량 도착으로 처리하고, 출발보다 적으면 차이가 이동중 재고에 남아 차이 처리를 해야 합니다.";
        if (AppMessageBox.Show(msg, "도착 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        if (!await SaveArrivalQtyAsync()) return;

        var result = await SaveAsync("USP_PR_XFER_C_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = "IN",
            ["p_xfer_id"] = _editingKey,
        });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "처리에 실패했습니다.", "도착 확인 실패");
            await QueryCore(forceKey: _editingKey);
            return;
        }

        Toast.Show("도착 확인 처리되었습니다.");
        await QueryCore(forceKey: _editingKey);
    }

    /// <summary>차이 처리/차이 처리 취소 - 선택한 LOT 행 하나. 귀책/처리방법은 그리드 셀에서 고른 값을 쓴다.</summary>
    private async Task DiffAsync(bool cancel)
    {
        if (_editingKey == null) return;
        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        var row = gvw1.GetFocusedDataRow();
        if (row == null)
        {
            AppMessageBox.Show("차이를 처리할 LOT 행을 선택하세요.", "안내");
            return;
        }

        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = cancel ? "CC" : "P",
            ["p_xfer_id"] = _editingKey,
            ["p_serl"] = row["serl"]?.ToString(),
        };
        if (!cancel)
        {
            var resp = row["diff_resp_cd"]?.ToString();
            var act = row["diff_act_cd"]?.ToString();
            if (Dec(row["diff_qty"]) <= 0)
            {
                AppMessageBox.Show("차이 수량이 없는 LOT입니다.", "안내");
                return;
            }
            if (string.IsNullOrEmpty(resp) || string.IsNullOrEmpty(act))
            {
                AppMessageBox.Show("차이 귀책과 차이 처리(손실/재입고)를 선택한 뒤 처리하세요.", "안내");
                return;
            }
            var actNm = act == "LOSS" ? "손실 처리(재고 차감)" : "재입고(도착 창고로 입고)";
            if (AppMessageBox.Show($"'{row["lot_no"]}' 차이 {Dec(row["diff_qty"]):#,##0.####}을(를) {actNm}하시겠습니까?", "차이 처리", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            p["p_resp_cd"] = resp;
            p["p_act_cd"] = act;
        }
        else if (AppMessageBox.Show("이 LOT의 차이 처리를 취소하시겠습니까?", "차이 처리 취소", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        var result = await SaveAsync("USP_PR_XFER_DIFF_S", p);
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "처리에 실패했습니다.", cancel ? "차이 처리 취소 실패" : "차이 처리 실패");
            return;
        }

        Toast.Show(cancel ? "차이 처리가 취소되었습니다." : "차이 처리되었습니다.");
        await QueryCore(forceKey: _editingKey);
    }

    /// <summary>다른 화면에서 이전번호로 열 때 호출된다(key=xfer_id 문자열).</summary>
    public override async Task FocusRecordAsync(string key) => await QueryCore(forceKey: key);
}
