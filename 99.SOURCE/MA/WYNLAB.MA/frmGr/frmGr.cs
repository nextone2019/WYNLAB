using System.Data;
using DevExpress.XtraEditors;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

/// <summary>
/// 구매입고등록 - 확정된 수입검사(합격+특채)와 검사면제 납품 중 아직 입고 안 된 수량(입고대기, VMA_GR_READY)을 "입고대기
/// 불러오기"(popPick, USP_MA_GRREADYPICK_Q)로 가져와 입고한다(TMAGRM/TMAGRD). frmDelv/frmIqc와 같은 Master-One Sheet 구조.
///
/// 확정(USP_MA_GR_C_S)하면 수불(TMATRANS)이 생성되고 재고관리 품목(라인 재고=Y)만 현재고(TMASTOCK)에 반영된다. 확정취소는
/// 반대 방향 역거래를 남기고 현재고를 되돌리며, 이미 출고/이동돼 재고가 모자라면 서버가 막는다. 입고방식이 자동일 때 납품/검사
/// 확정이 만든 입고(자동입고=Y)는 이 화면에서 조회만 되고 수정/삭제/확정취소는 원천(납품/검사)의 확정취소로만 된다.
/// 이 화면은 구매입고(수불유형 PU_IN)만 다룬다 - 판매/생산 입고는 같은 테이블을 쓰는 별도 화면에서 다룬다.
/// </summary>
public partial class frmGr : BaseForm
{
    private const string TransType = "PU_IN";

    private DataTable _detail = new();
    private string? _editingKey; // null이면 신규모드
    private string _statCd = "0";
    private bool _isAuto;

    public frmGr()
    {
        InitializeComponent();

        Text = "구매입고등록";

        Controls.Add(BuildScreenHeader());

        txtDeptNm.MapField("dept_id", txtDeptId);
        txtEmpNm.MapField("emp_id", txtEmpId);
        txtCustNm.MapField("cust_id", txtCustId);

        gvw1.Role = GridRoleWyn.Edit;
        gvw1.HighlightFocusedRow = true;
        gvw1.RowAdd += (s, e) => AppMessageBox.Show("입고 품목은 '입고대기 불러오기'로만 추가할 수 있습니다.", "안내");
        gvw1.RowDelete += (s, e) => DeleteFocusedRow();
        btnDeletRow1.Click += (s, e) => DeleteFocusedRow();
        btnLoadReady.Click += async (s, e) => await SafeExecuteAsync(LoadFromReadyAsync, "입고대기 불러오기");
        btnConfirm.Click += async (s, e) => await SafeExecuteAsync(() => ConfirmAsync(true), "입고 확정");
        btnConfirmCancel.Click += async (s, e) => await SafeExecuteAsync(() => ConfirmAsync(false), "입고 확정취소");

        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) => await SafeExecuteAsync(EnsureDetailSchemaAsync, "구매입고 화면 초기화");
    }

    private static decimal Dec(object? value) => decimal.TryParse(value?.ToString(), out var d) ? d : 0;

    private void DeleteFocusedRow()
    {
        if (_statCd == "C" || _isAuto) return;
        try { if (gvw1.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
        catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
    }

    /// <summary>새 폼은 아직 조회한 적이 없어 _detail에 컬럼이 없다 - 조건에 안 걸리는 조회로 스키마만 먼저 받아 둔다.</summary>
    private async Task EnsureDetailSchemaAsync()
    {
        if (_detail.Columns.Count > 0) return;

        var tables = await QueryMultiAsync("USP_MA_GR_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q", ["p_gr_id"] = "-1" });
        if (tables.Count > 1) _detail = tables[1];
        EnterNewMode();
    }

    public override async Task QueryClick() => await QueryCore(forceKey: null);

    private async Task QueryCore(string? forceKey)
    {
        // 조회 공통 규칙(2026-09-26): 결과가 없으면 신규 입력 모드로 전환한다. 검색 번호를 안 넣은 조회는 서버가 "조건 없음"을 "가장 최근 문서 1건"으로 처리하므로,
        // 조회할 대상이 없는 것으로 보고 서버를 부르지 않고 바로 신규 모드로 간다.
        if (forceKey == null && string.IsNullOrWhiteSpace(txtSearchGrNo.Text))
        {
            EnterNewMode();
            Toast.Show("조회할 번호가 없어 신규 입력 상태로 전환했습니다.");
            return;
        }
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_gr_id"] = forceKey,
            ["p_gr_no"] = forceKey == null ? txtSearchGrNo.Text : null,
            ["p_trans_type"] = TransType, // 구매입고 화면은 구매입고만 본다
        };
        var tables = await QueryMultiAsync("USP_MA_GR_Q", p);
        var header = tables.Count > 0 ? tables[0] : new DataTable();
        _detail = tables.Count > 1 ? tables[1] : new DataTable();

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
            _editingKey = row["gr_id"]?.ToString();
            _statCd = row["stat_cd"]?.ToString() ?? "0";
            _isAuto = row["auto_yn"]?.ToString() == "Y";
            cboAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            txtGrNo.Text = row["gr_no"]?.ToString() ?? string.Empty;
            dteGrDate.YyyyMmDd = row["gr_date"]?.ToString();
            cboStatCd.EditValue = _statCd;
            cboTransType.EditValue = row["trans_type"]?.ToString() ?? TransType;
            txtAutoYn.Text = _isAuto ? "자동 생성" : "수동";
            txtCustId.Text = row["cust_id"]?.ToString() ?? string.Empty;
            txtCustNm.Text = row["cust_nm"]?.ToString() ?? string.Empty;
            txtDeptId.Text = row["dept_id"]?.ToString() ?? string.Empty;
            txtDeptNm.Text = row["dept_nm"]?.ToString() ?? string.Empty;
            txtEmpId.Text = row["emp_id"]?.ToString() ?? string.Empty;
            txtEmpNm.Text = row["emp_nm"]?.ToString() ?? string.Empty;
            txtCfmDt.Text = row["cfm_dt"] is DateTime dt ? dt.ToString("yyyy-MM-dd HH:mm") : string.Empty;
            txtCfmUserId.Text = row["cfm_user_id"]?.ToString() ?? string.Empty;
            memoRemark.Text = row["remark"]?.ToString() ?? string.Empty;

            TrackDirty(_detail);
            grd1.DataSource = _detail;
        });
        ApplyLock();
    }

    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = null;
            _statCd = "0";
            _isAuto = false;
            cboAccId.EditValue = Session.AccId?.ToString();
            txtGrNo.Text = string.Empty;
            dteGrDate.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");
            cboStatCd.EditValue = "0";
            cboTransType.EditValue = TransType;
            txtAutoYn.Text = "수동";
            txtCustId.Text = string.Empty;
            txtCustNm.Text = string.Empty;
            txtDeptId.Text = Session.DeptId?.ToString() ?? string.Empty;
            txtDeptNm.Text = Session.DeptNm;
            txtEmpId.Text = Session.EmpId?.ToString() ?? string.Empty;
            txtEmpNm.Text = Session.EmpNm; // 담당자명 = 세션 사원명(EmpId의 이름) - 사용자 이름(UserNm)이 아니다
            txtCfmDt.Text = string.Empty;
            txtCfmUserId.Text = string.Empty;
            memoRemark.Text = string.Empty;

            _detail = _detail.Clone();
            TrackDirty(_detail);
            grd1.DataSource = _detail;
        });
        ApplyLock();
    }

    /// <summary>확정된 입고와 자동 생성된 입고는 화면 전체가 조회전용. 확정취소 버튼은 수동으로 만든 확정 입고에서만 켠다.</summary>
    private void ApplyLock()
    {
        var locked = _statCd == "C" || _isAuto;
        foreach (var edit in new BaseEdit[] { dteGrDate, txtCustNm, txtDeptNm, txtEmpNm, memoRemark })
            edit.Properties.ReadOnly = locked;
        gvw1.OptionsBehavior.Editable = !locked;
        btnLoadReady.Enabled = !locked;
        btnDeletRow1.Enabled = !locked;
        btnConfirm.Enabled = !locked && _editingKey != null;
        btnConfirmCancel.Enabled = _statCd == "C" && !_isAuto;
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    /// <summary>입고대기 불러오기 - 확정 검사(합격+특채)와 검사면제 납품의 입고대기 잔량을 골라 가져온다. 기본 입고수량은 잔량 전체.
    /// 이미 그리드에 있는 원천 라인은 건너뛴다. 창고/위치는 원천 라인의 예정 값이 채워지고 확정 전에 바꿀 수 있다.</summary>
    private async Task LoadFromReadyAsync()
    {
        if (_statCd == "C" || _isAuto) return;
        gvw1.CloseEditor();

        var columns = new[]
        {
            new PickColumn("src_nm", "입고원천", 100), new PickColumn("src_no", "원천번호", 110), new PickColumn("ready_date", "대기일", 80),
            new PickColumn("cust_nm", "거래처", 110), new PickColumn("item_no", "품번", 100), new PickColumn("item_nm", "품명", 140),
            new PickColumn("item_spec", "규격", 110), new PickColumn("unit_cd", "단위", 50), new PickColumn("lot_no", "LOT", 90),
            new PickColumn("ready_qty", "입고대기", 80, true), new PickColumn("remain_qty", "잔량", 80, true),
            new PickColumn("wh_nm", "창고", 90), new PickColumn("stock_yn", "재고", 45), new PickColumn("po_no", "발주번호", 110),
        };
        var picked = popPick.Pick(this, MenuId, "입고대기 불러오기", "USP_MA_GRREADYPICK_Q", "납품/검사번호", columns,
            cboAccId.EditValue?.ToString(), txtCustId.Text, txtCustNm.Text);
        if (picked == null || picked.Rows.Count == 0) return;

        var first = picked.Rows[0];
        if (string.IsNullOrWhiteSpace(txtCustId.Text))
        {
            txtCustId.Text = first["cust_id"]?.ToString() ?? string.Empty;
            txtCustNm.Text = first["cust_nm"]?.ToString() ?? string.Empty;
        }

        var skipped = 0;
        foreach (DataRow r in picked.Rows)
        {
            var srcType = r["src_type"]?.ToString();
            var srcId = r["src_id"]?.ToString();
            var srcSerl = r["src_serl"]?.ToString();
            var dup = _detail.Rows.Cast<DataRow>().Any(d =>
                d.RowState != DataRowState.Deleted
                && d["src_type"]?.ToString() == srcType
                && d["src_id"]?.ToString() == srcId
                && d["src_serl"]?.ToString() == srcSerl);
            if (dup) { skipped++; continue; }

            var row = _detail.NewRow();
            row["item_id"] = r["item_id"];
            row["item_no"] = r["item_no"];
            row["item_nm"] = r["item_nm"];
            row["item_spec"] = r["item_spec"];
            row["unit_cd"] = r["unit_cd"];
            row["lot_no"] = r["lot_no"];
            row["gr_qty"] = Dec(r["remain_qty"]);
            row["next_qty"] = 0;
            row["ready_qty"] = r["ready_qty"];
            row["ready_remain_qty"] = r["remain_qty"];
            row["wh_id"] = r["wh_id"];
            row["wh_nm"] = r["wh_nm"];
            row["loc_id"] = r["loc_id"];
            row["loc_nm"] = r["loc_nm"];
            row["stock_yn"] = r["stock_yn"];
            row["src_type"] = srcType;
            row["src_id"] = srcId;
            row["src_no"] = r["src_no"];
            row["src_serl"] = srcSerl;
            row["po_id"] = r["po_id"];
            row["po_no"] = r["po_no"];
            row["po_serl"] = r["po_serl"];
            _detail.Rows.Add(row);
        }

        if (skipped > 0) Toast.Show($"이미 불러온 입고대기 품목 {skipped}건은 건너뛰었습니다.");
    }

    public override async Task SaveClick()
    {
        if (_statCd == "C" || _isAuto)
        {
            AppMessageBox.Show(_isAuto
                ? "자동 생성된 입고는 수정할 수 없습니다."
                : "확정된 입고는 수정할 수 없습니다. 먼저 확정취소하세요.", "안내");
            return;
        }

        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        foreach (DataRow row in _detail.Rows)
        {
            if (row.RowState == DataRowState.Deleted || row.RowState == DataRowState.Unchanged) continue;

            var name = $"{row["item_nm"]}";
            if (Dec(row["gr_qty"]) <= 0)
            {
                AppMessageBox.Show($"'{name}' 입고수량을 0보다 크게 입력하세요.", "안내");
                return;
            }
            if (Dec(row["gr_qty"]) > Dec(row["ready_remain_qty"]) && row["ready_remain_qty"] != DBNull.Value)
            {
                AppMessageBox.Show($"'{name}' 입고수량이 입고대기 잔량({Dec(row["ready_remain_qty"]):#,0.####})을 넘었습니다.", "안내");
                return;
            }
        }

        var headerParams = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_gr_id"] = _editingKey,
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_gr_date"] = dteGrDate.YyyyMmDd,
            ["p_trans_type"] = cboTransType.EditValue?.ToString() ?? TransType,
            ["p_cust_id"] = txtCustId.Text,
            ["p_dept_id"] = txtDeptId.Text,
            ["p_emp_id"] = txtEmpId.Text,
            ["p_remark"] = memoRemark.Text,
        };
        var headerResult = await SaveAsync("USP_MA_GR_S", headerParams);
        if (headerResult == null || !headerResult.Success)
        {
            AppMessageBox.Show(headerResult?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        var headerKey = _editingKey ?? headerResult.GeneratedCode;

        foreach (DataRow row in _detail.Rows.Cast<DataRow>().ToList())
        {
            if (row.RowState == DataRowState.Unchanged) continue;

            var workType = row.RowState == DataRowState.Deleted ? "D" : row.RowState == DataRowState.Added ? "N" : "U";
            var version = row.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;

            var detailParams = new Dictionary<string, string?>
            {
                ["p_work_type"] = workType,
                ["p_gr_id"] = headerKey,
                ["p_serl"] = ProcData.Str(row, "serl", version),
                ["p_gr_qty"] = ProcData.Str(row, "gr_qty", version),
                ["p_wh_id"] = ProcData.Str(row, "wh_id", version),
                ["p_loc_id"] = ProcData.Str(row, "loc_id", version),
                ["p_src_type"] = ProcData.Str(row, "src_type", version),
                ["p_src_id"] = ProcData.Str(row, "src_id", version),
                ["p_src_serl"] = ProcData.Str(row, "src_serl", version),
                ["p_remark"] = ProcData.Str(row, "remark", version),
            };

            var detailResult = await SaveAsync("USP_MA_GR_S_1", detailParams);
            if (detailResult == null || !detailResult.Success)
            {
                AppMessageBox.Show(detailResult?.Message ?? "품목 저장에 실패했습니다.", "저장 실패");
                // 헤더는 이미 저장됐으므로 그 상태로 다시 조회해서 화면을 실제 저장 상태에 맞춘다.
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

        var result = await SaveAsync("USP_MA_GR_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = "D",
            ["p_gr_id"] = _editingKey,
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

        var msg = confirm
            ? "입고를 확정하시겠습니까?\n확정하면 수불이 생성되고, 재고관리 품목은 현재고에 반영됩니다."
            : "입고 확정을 취소하시겠습니까?\n역거래 수불이 남고 현재고가 되돌려집니다(이미 출고/이동돼 재고가 모자라면 취소되지 않습니다).";
        if (AppMessageBox.Show(msg, confirm ? "입고 확정" : "입고 확정취소", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        var result = await SaveAsync("USP_MA_GR_C_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = confirm ? "C" : "CC",
            ["p_gr_id"] = _editingKey,
        });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "처리에 실패했습니다.", confirm ? "확정 실패" : "확정취소 실패");
            return;
        }

        Toast.Show(confirm ? "확정되었습니다." : "확정이 취소되었습니다.");
        await QueryCore(forceKey: _editingKey);
    }

    /// <summary>구매입고현황(frmGrList)에서 입고번호를 더블클릭했을 때 호출된다(key=gr_id 문자열).</summary>
    public override async Task FocusRecordAsync(string key) => await QueryCore(forceKey: key);
}
