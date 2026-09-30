using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

/// <summary>
/// 작업지시 - 웨이퍼 LOT 1개의 공정 체인(TPRWOM/TPRWOD). 라우팅을 고르고 시작 LOT/수량을 넣어 저장하면 서버가 라우팅을
/// 공정행으로 복사하고(창고는 외주처의 외주창고로 자동 지정) 시작 LOT를 만든다. 공정행은 외주발주를 겸해서 외주처/창고/단가/
/// 납기/분할수량/비고만 고칠 수 있고 공정 추가/삭제는 없다(라우팅에서 온다). 수주(SO) 연결은 선택 사항.
/// 웨이퍼가 도착하면 시작 LOT는 웨이퍼입고 화면(frmRcv)에서 먼저 입고 확정한 미배정 LOT를 [LOT 선택]으로 배정한다.
/// 상태 흐름: 계획(0) -> 확정(C) -> 진행(1, 첫 실적 확정 시 자동) -> 완료(E) / 중단(X). 확정해야 공정실적/외주이전을 등록할 수 있고 불러오기 목록에 나온다
/// (USP_PR_WO_C_S: 확정/확정취소/중단/중단해제/완료/완료취소). 생성 후에는 라우팅/시작 LOT/수량을 바꿀 수 없다.
/// </summary>
public partial class frmWo : BaseForm
{
    private DataTable _detail = new();
    private DataTable _lots = new();
    private string? _editingKey; // null이면 신규모드
    private string _statCd = "0";

    public frmWo()
    {
        InitializeComponent();

        Text = "작업지시";

        Controls.Add(BuildScreenHeader());

        txtDeptNm.MapField("dept_id", txtDeptId);
        txtEmpNm.MapField("emp_id", txtEmpId);

        gvw1.Role = GridRoleWyn.Edit;
        gvw1.HighlightFocusedRow = true;
        gvw1.RowAdd += (s, e) => AppMessageBox.Show("공정은 라우팅에서 자동으로 만들어집니다. 추가/삭제할 수 없습니다.", "안내");
        gvw1.RowDelete += (s, e) => AppMessageBox.Show("공정은 라우팅에서 자동으로 만들어집니다. 추가/삭제할 수 없습니다.", "안내");
        gvw1.CellValueChanged += Gvw1_CellValueChanged;

        gvw2.HighlightFocusedRow = true;

        btnPickLot.Click += async (s, e) => await SafeExecuteAsync(PickLotAsync, "입고 LOT 선택");
        btnPickSo.Click += async (s, e) => await SafeExecuteAsync(PickSoAsync, "수주 선택");
        btnClearSo.Click += (s, e) => SetSo(null, null, null);
        btnConfirm.Click += async (s, e) => await SafeExecuteAsync(ConfirmWoAsync, "작업지시 확정");
        btnConfirmCancel.Click += async (s, e) => await SafeExecuteAsync(() => ChangeStatusAsync("CC", "확정취소"), "작업지시 확정취소");
        btnStop.Click += async (s, e) => await SafeExecuteAsync(() => ChangeStatusAsync("X", "중단"), "작업지시 중단");
        btnStopCancel.Click += async (s, e) => await SafeExecuteAsync(() => ChangeStatusAsync("XC", "중단해제"), "작업지시 중단해제");
        btnComplete.Click += async (s, e) => await SafeExecuteAsync(() => ChangeStatusAsync("E", "완료"), "작업지시 완료");
        btnCompleteCancel.Click += async (s, e) => await SafeExecuteAsync(() => ChangeStatusAsync("EC", "완료취소"), "작업지시 완료취소");

        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) => await SafeExecuteAsync(EnsureSchemaAsync, "작업지시 화면 초기화");
    }

    private static decimal Dec(object? value) => decimal.TryParse(value?.ToString(), out var d) ? d : 0;

    /// <summary>숫자 컬럼에 사용자가 "1,500"처럼 문자열을 입력하면 decimal로 바꿔 넣는다(다른 화면과 같은 처리).</summary>
    private void Gvw1_CellValueChanged(object? sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
    {
        if (e.Value is string text && (e.Column == colSplitQty || e.Column == colPrice))
        {
            gvw1.SetRowCellValue(e.RowHandle, e.Column,
                decimal.TryParse(text.Replace(",", ""), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var n)
                    ? n : (object)DBNull.Value);
        }
    }

    /// <summary>새 폼은 아직 조회한 적이 없어 그리드 테이블에 컬럼이 없다 - 조건에 안 걸리는 조회로 스키마만 먼저 받아 둔다.</summary>
    private async Task EnsureSchemaAsync()
    {
        if (_detail.Columns.Count > 0) return;

        var tables = await QueryMultiAsync("USP_PR_WO_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q", ["p_wo_id"] = "-1" });
        if (tables.Count > 1) _detail = tables[1];
        if (tables.Count > 2) _lots = tables[2];
        EnterNewMode();
    }

    public override async Task QueryClick() => await QueryCore(forceKey: null);

    private async Task QueryCore(string? forceKey)
    {
        // 조회 공통 규칙: 번호를 안 넣은 조회는 서버가 "가장 최근 1건"을 돌려주므로, 조회할 대상이 없는 것으로 보고 신규 모드로 간다.
        if (forceKey == null && string.IsNullOrWhiteSpace(txtSearchWoNo.Text))
        {
            EnterNewMode();
            Toast.Show("조회할 번호가 없어 신규 입력 상태로 전환했습니다.");
            return;
        }

        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_wo_id"] = forceKey,
            ["p_wo_no"] = forceKey == null ? txtSearchWoNo.Text : null,
        };
        var tables = await QueryMultiAsync("USP_PR_WO_Q", p);
        var header = tables.Count > 0 ? tables[0] : new DataTable();
        _detail = tables.Count > 1 ? tables[1] : new DataTable();
        _lots = tables.Count > 2 ? tables[2] : new DataTable();

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
            _editingKey = row["wo_id"]?.ToString();
            _statCd = row["stat_cd"]?.ToString() ?? "0";
            cboAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            cboStatCd.EditValue = _statCd;
            txtWoNo.Text = row["wo_no"]?.ToString() ?? string.Empty;
            txtSearchWoNo.Text = txtWoNo.Text; // 링크로 열었거나 저장 후 재조회해도 조회 버튼이 현재 문서를 다시 읽도록 조회 조건에 반영
            dteWoDate.YyyyMmDd = row["wo_date"]?.ToString();
            cboRouteId.EditValue = row["route_id"]?.ToString() ?? string.Empty;
            dteDelvDate.YyyyMmDd = row["delv_date"]?.ToString();
            txtStartLotId.Text = string.Empty;
            txtStartLotNo.Text = row["start_lot_no"]?.ToString() ?? string.Empty;
            spnStartQty.EditValue = Dec(row["start_qty"]);
            txtItemNm.Text = row["item_nm"]?.ToString() ?? string.Empty;
            SetSo(row["so_id"]?.ToString(), row["so_serl"]?.ToString(), row["so_no"]?.ToString());
            txtDeptId.Text = row["dept_id"]?.ToString() ?? string.Empty;
            txtDeptNm.Text = row["dept_nm"]?.ToString() ?? string.Empty;
            txtEmpId.Text = row["emp_id"]?.ToString() ?? string.Empty;
            txtEmpNm.Text = row["emp_nm"]?.ToString() ?? string.Empty;
            memoRemark.Text = row["remark"]?.ToString() ?? string.Empty;

            TrackDirty(_detail);
            grd1.DataSource = _detail;
            grd2.DataSource = _lots;
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
            txtWoNo.Text = string.Empty;
            dteWoDate.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");
            cboRouteId.EditValue = null;
            dteDelvDate.YyyyMmDd = null;
            txtStartLotId.Text = string.Empty;
            txtStartLotNo.Text = string.Empty;
            spnStartQty.EditValue = 0m;
            txtItemNm.Text = string.Empty;
            SetSo(null, null, null);
            txtDeptId.Text = Session.DeptId?.ToString() ?? string.Empty;
            txtDeptNm.Text = Session.DeptNm;
            txtEmpId.Text = Session.EmpId?.ToString() ?? string.Empty;
            txtEmpNm.Text = Session.EmpNm;
            memoRemark.Text = string.Empty;

            _detail = _detail.Clone();
            _lots = _lots.Clone();
            TrackDirty(_detail);
            grd1.DataSource = _detail;
            grd2.DataSource = _lots;
        });
        ApplyLock();
    }

    private void SetSo(string? soId, string? soSerl, string? soNo)
    {
        txtSoId.Text = soId ?? string.Empty;
        txtSoSerl.Text = soSerl ?? string.Empty;
        txtSoNo.Text = string.IsNullOrEmpty(soNo) ? string.Empty : (string.IsNullOrEmpty(soSerl) ? soNo : $"{soNo}-{soSerl}");
    }

    /// <summary>생성된 작업지시는 라우팅/시작 LOT/수량을 잠근다. 완료/중단 상태면 화면 전체가 조회 전용.</summary>
    private void ApplyLock()
    {
        var created = _editingKey != null;
        var closed = _statCd == "E" || _statCd == "X";

        cboRouteId.Properties.ReadOnly = created;
        btnPickLot.Enabled = !created;
        foreach (var edit in new DevExpress.XtraEditors.BaseEdit[] { dteWoDate, dteDelvDate, txtDeptNm, txtEmpNm, memoRemark })
            edit.Properties.ReadOnly = closed;
        gvw1.OptionsBehavior.Editable = created && !closed;
        btnPickSo.Enabled = !closed;
        btnClearSo.Enabled = !closed;

        btnConfirm.Enabled = created && _statCd == "0";
        btnConfirmCancel.Enabled = created && _statCd == "C";
        btnStop.Enabled = created && (_statCd == "C" || _statCd == "1");
        btnStopCancel.Enabled = created && _statCd == "X";
        btnComplete.Enabled = created && _statCd == "1";
        btnCompleteCancel.Enabled = created && _statCd == "E";
    }

    public override Task NewClick()
    {
        EnterNewMode();
        FocusFirstEntryField(panData); // 사업장 다음 첫 탭오더 컨트롤에 커서(표준)
        return Task.CompletedTask;
    }

    /// <summary>입고 LOT 선택 - 웨이퍼입고에서 확정한 미배정 LOT 한 건을 시작 LOT로 배정한다. 시작수량은 그 LOT의 재고 수량.</summary>
    private async Task PickLotAsync()
    {
        if (_editingKey != null) return;

        var columns = new[]
        {
            new PickColumn("lot_no", "LOT", 120), new PickColumn("item_nm", "품목", 110), new PickColumn("unit_cd", "단위", 45),
            new PickColumn("stock_qty", "재고수량", 80, true), new PickColumn("wh_nm", "창고", 110), new PickColumn("lot_date", "입고일", 80),
            new PickColumn("sup_cust_nm", "공급처", 110),
        };
        var picked = popPick.Pick(this, MenuId, "입고 LOT 선택", "USP_PR_WOLOTPICK_Q", "LOT번호", columns, cboAccId.EditValue?.ToString(),
            rows => rows.Count > 1 ? "시작 LOT는 한 건만 선택할 수 있습니다." : null,
            emptyHint: "선택할 웨이퍼 LOT가 없습니다. 먼저 웨이퍼입고 화면에서 입고를 확정하세요(이미 다른 작업지시에 배정된 LOT는 나오지 않습니다).");
        if (picked == null || picked.Rows.Count == 0) return;

        var r = picked.Rows[0];
        txtStartLotId.Text = r["lot_id"]?.ToString() ?? string.Empty;
        txtStartLotNo.Text = r["lot_no"]?.ToString() ?? string.Empty;
        txtItemNm.Text = r["item_nm"]?.ToString() ?? string.Empty;
        spnStartQty.EditValue = Dec(r["stock_qty"]);
        await Task.CompletedTask;
    }

    /// <summary>수주 선택 - 확정 여부와 관계없이 마감되지 않은 수주 품목 한 건을 골라 연결한다. 납기일이 비어 있으면 수주 납기로 채운다.</summary>
    private async Task PickSoAsync()
    {
        var columns = new[]
        {
            new PickColumn("so_no", "수주번호", 110), new PickColumn("so_serl", "순번", 45), new PickColumn("so_date", "수주일", 80),
            new PickColumn("cust_nm", "거래처", 110), new PickColumn("item_no", "품번", 100), new PickColumn("item_nm", "품명", 140),
            new PickColumn("unit_cd", "단위", 50), new PickColumn("qty", "수주수량", 80, true), new PickColumn("delv_date", "납기", 80),
        };
        var picked = popPick.Pick(this, MenuId, "수주 선택", "USP_PR_SOPICK_Q", "수주번호", columns, cboAccId.EditValue?.ToString(),
            rows => rows.Count > 1 ? "수주 품목은 한 건만 선택할 수 있습니다." : null);
        if (picked == null || picked.Rows.Count == 0) return;

        var r = picked.Rows[0];
        SetSo(r["so_id"]?.ToString(), r["so_serl"]?.ToString(), r["so_no"]?.ToString());
        if (string.IsNullOrWhiteSpace(dteDelvDate.YyyyMmDd) && !string.IsNullOrWhiteSpace(r["delv_date"]?.ToString()))
            dteDelvDate.YyyyMmDd = r["delv_date"]?.ToString();
        await Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        if (_statCd == "E" || _statCd == "X")
        {
            AppMessageBox.Show("완료되었거나 중단된 작업지시는 수정할 수 없습니다.", "안내");
            return;
        }

        if (_editingKey == null && string.IsNullOrWhiteSpace(txtStartLotId.Text))
        {
            AppMessageBox.Show("'LOT 선택'으로 입고된 웨이퍼 LOT를 선택하세요. (먼저 웨이퍼입고 화면에서 입고를 확정해야 합니다)", "안내");
            return;
        }

        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        var headerParams = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_wo_id"] = _editingKey,
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_wo_date"] = dteWoDate.YyyyMmDd,
            ["p_route_id"] = cboRouteId.EditValue?.ToString(),
            ["p_start_lot_id"] = _editingKey == null ? txtStartLotId.Text : null,
            ["p_so_id"] = txtSoId.Text,
            ["p_so_serl"] = txtSoSerl.Text,
            ["p_delv_date"] = dteDelvDate.YyyyMmDd,
            ["p_dept_id"] = txtDeptId.Text,
            ["p_emp_id"] = txtEmpId.Text,
            ["p_remark"] = memoRemark.Text,
        };
        var headerResult = await SaveAsync("USP_PR_WO_S", headerParams);
        if (headerResult == null || !headerResult.Success)
        {
            AppMessageBox.Show(headerResult?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        var headerKey = _editingKey ?? headerResult.GeneratedCode;

        // 공정행은 신규 저장 직후에는 아직 화면에 없다(서버가 방금 복사) - 수정한 행만 저장한다.
        foreach (DataRow row in _detail.Rows.Cast<DataRow>().ToList())
        {
            if (row.RowState != DataRowState.Modified) continue;

            // 외주처만 바꾸고 창고를 안 건드렸으면 창고를 비워 보내서 서버가 그 외주처의 외주창고로 채우게 한다.
            var custChanged = !Equals(row["cust_id", DataRowVersion.Original]?.ToString(), row["cust_id", DataRowVersion.Current]?.ToString());
            var whChanged = !Equals(row["wh_id", DataRowVersion.Original]?.ToString(), row["wh_id", DataRowVersion.Current]?.ToString());

            var detailParams = new Dictionary<string, string?>
            {
                ["p_work_type"] = "U",
                ["p_wo_id"] = headerKey,
                ["p_serl"] = ProcData.Str(row, "serl", DataRowVersion.Current),
                ["p_cust_id"] = ProcData.Str(row, "cust_id", DataRowVersion.Current),
                ["p_wh_id"] = custChanged && !whChanged ? null : ProcData.Str(row, "wh_id", DataRowVersion.Current),
                ["p_split_qty"] = ProcData.Str(row, "split_qty", DataRowVersion.Current),
                ["p_price_unit_cd"] = ProcData.Str(row, "price_unit_cd", DataRowVersion.Current),
                ["p_price"] = ProcData.Str(row, "price", DataRowVersion.Current),
                ["p_due_date"] = ProcData.Str(row, "due_date", DataRowVersion.Current),
                ["p_remark"] = ProcData.Str(row, "remark", DataRowVersion.Current),
            };
            var detailResult = await SaveAsync("USP_PR_WO_S_1", detailParams);
            if (detailResult == null || !detailResult.Success)
            {
                AppMessageBox.Show(detailResult?.Message ?? "공정 저장에 실패했습니다.", "저장 실패");
                _editingKey ??= headerResult.GeneratedCode;
                await QueryCore(forceKey: _editingKey);
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

        var result = await SaveAsync("USP_PR_WO_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = "D",
            ["p_wo_id"] = _editingKey,
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

    /// <summary>작업지시 확정 - 확정해야 공정실적/외주이전을 등록할 수 있고 불러오기 목록에도 나온다. 시작 LOT 재고가 첫 공정 외주처 창고에 아직 없으면
    /// (웨이퍼가 입고 전이면) 그 사실을 알리고 계속할지 묻는다 - 재고 없이 확정은 가능하지만 웨이퍼를 입고하기 전에는 실적 대기 LOT에 나오지 않는다.</summary>
    private async Task ConfirmWoAsync()
    {
        if (_editingKey == null)
        {
            AppMessageBox.Show("먼저 저장한 뒤 확정하세요.", "안내");
            return;
        }

        var startLotHasStock = _lots.Rows.Cast<DataRow>().Any(r => r["wo_serl"]?.ToString() == "0" && Dec(r["stock_qty"]) > 0);
        if (!startLotHasStock)
        {
            var msg = $"시작 LOT '{txtStartLotNo.Text}'의 재고가 첫 공정 외주처 창고에 아직 없습니다.\n웨이퍼입고 화면에서 그 LOT를 입고 확정했는지 확인하세요. 재고가 없으면 공정실적의 '실적 대기 LOT 불러오기'에 나오지 않습니다.\n\n그래도 확정하시겠습니까?";
            if (AppMessageBox.Show(msg, "시작 LOT 재고 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
        }
        await ChangeStatusAsync("C", "확정");
    }

    /// <summary>상태 처리 - 미저장 변경이 있으면 서버 상태와 어긋나므로 먼저 저장하게 한다.</summary>
    private async Task ChangeStatusAsync(string workType, string caption)
    {
        if (_editingKey == null) return;
        gvw1.CloseEditor();
        if (IsDirty)
        {
            AppMessageBox.Show("저장하지 않은 변경이 있습니다. 먼저 저장하세요.", "안내");
            return;
        }
        if (AppMessageBox.Show($"작업지시를 {caption} 처리하시겠습니까?", caption, MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        var result = await SaveAsync("USP_PR_WO_C_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = workType,
            ["p_wo_id"] = _editingKey,
        });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "처리에 실패했습니다.", caption + " 실패");
            return;
        }

        Toast.Show(caption + " 처리되었습니다.");
        await QueryCore(forceKey: _editingKey);
    }

    /// <summary>작업지시현황(frmWoStatus)에서 작업지시를 더블클릭했을 때 호출된다(key=wo_id 문자열).</summary>
    public override async Task FocusRecordAsync(string key) => await QueryCore(forceKey: key);
}
