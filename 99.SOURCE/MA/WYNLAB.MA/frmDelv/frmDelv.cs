using System.Data;
using DevExpress.XtraEditors;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

/// <summary>
/// 납품등록 - 협력사가 납품한 물품을 구매/자재 담당자가 접수해서 기록한다(TMADELVM/TMADELVD). frmPo와 같은
/// Master-One Sheet 구조지만 품목은 직접 입력하지 않고 "발주 불러오기"(popPick, USP_MA_PODELVPICK_Q)로 승인 완료된
/// 발주 라인에서만 가져온다 - 품목/단위/검사여부/창고는 발주 라인 값이 복사되고, 여기서는 납품수량/LOT/창고만 바꾼다.
///
/// 작성(stat_cd='0') 상태에서만 수정/삭제할 수 있고, 확정(USP_MA_DELV_C_S)하면 발주 라인 next_qty(납품 인정 수량)가
/// 서버에서 다시 계산된다. 확정 후에는 화면 전체가 조회전용이고, 검사/입고가 진행된 납품은 확정취소도 서버가 막는다.
/// 입고방식이 "자동"일 때의 자동 입고는 입고 프로시저가 완성된 뒤 서버(USP_MA_DELV_C_S)에 연결된다 - 이 화면은 변경 없음.
/// </summary>
public partial class frmDelv : BaseForm
{
    private DataTable _detail = new();
    private string? _editingKey; // null이면 신규모드
    private string _statCd = "0";

    public frmDelv()
    {
        InitializeComponent();

        Text = "납품등록";

        Controls.Add(BuildScreenHeader());

        txtDeptNm.MapField("dept_id", txtDeptId);
        txtEmpNm.MapField("emp_id", txtEmpId);
        txtCustNm.MapField("cust_id", txtCustId);

        gvw1.Role = GridRoleWyn.Edit;
        gvw1.HighlightFocusedRow = true;
        gvw1.RowAdd += (s, e) => AppMessageBox.Show("납품 품목은 '발주 불러오기'로만 추가할 수 있습니다.", "안내");
        gvw1.RowDelete += (s, e) => DeleteFocusedRow();
        btnDeletRow1.Click += (s, e) => DeleteFocusedRow();
        btnLoadPo.Click += async (s, e) => await SafeExecuteAsync(LoadFromPoAsync, "발주 불러오기");
        btnConfirm.Click += async (s, e) => await SafeExecuteAsync(() => ConfirmAsync(true), "납품 확정");
        btnConfirmCancel.Click += async (s, e) => await SafeExecuteAsync(() => ConfirmAsync(false), "납품 확정취소");

        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) => await SafeExecuteAsync(EnsureDetailSchemaAsync, "납품 화면 초기화");
    }

    private static decimal Dec(object? value) => decimal.TryParse(value?.ToString(), out var d) ? d : 0;

    private void DeleteFocusedRow()
    {
        if (_statCd == "C") return;
        try { if (gvw1.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
        catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
    }

    /// <summary>새 폼은 아직 조회한 적이 없어 _detail에 컬럼이 없다 - 조건에 안 걸리는 조회(p_delv_id=-1)로 라인 스키마만
    /// 받아 신규모드를 다시 잡는다(frmPo.EnsureDetailSchemaAsync와 같은 이유).</summary>
    private async Task EnsureDetailSchemaAsync()
    {
        if (_detail.Columns.Count > 0) return;

        var tables = await QueryMultiAsync("USP_MA_DELV_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q", ["p_delv_id"] = "-1" });
        if (tables.Count > 1) _detail = tables[1];
        EnterNewMode();
    }

    public override async Task QueryClick() => await QueryCore(forceKey: null);

    private async Task QueryCore(string? forceKey)
    {
        // 조회 공통 규칙(2026-09-26): 결과가 없으면 신규 입력 모드로 전환한다. 검색 번호를 안 넣은 조회는 서버가 "조건 없음"을 "가장 최근 문서 1건"으로 처리하므로,
        // 조회할 대상이 없는 것으로 보고 서버를 부르지 않고 바로 신규 모드로 간다.
        if (forceKey == null && string.IsNullOrWhiteSpace(txtSearchDelvNo.Text))
        {
            EnterNewMode();
            Toast.Show("조회할 번호가 없어 신규 입력 상태로 전환했습니다.");
            return;
        }
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_delv_id"] = forceKey,
            ["p_delv_no"] = forceKey == null ? txtSearchDelvNo.Text : null,
        };
        var tables = await QueryMultiAsync("USP_MA_DELV_Q", p);
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
            _editingKey = row["delv_id"]?.ToString();
            _statCd = row["stat_cd"]?.ToString() ?? "0";
            cboAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            txtDelvNo.Text = row["delv_no"]?.ToString() ?? string.Empty;
            dteDelvDate.YyyyMmDd = row["delv_date"]?.ToString();
            cboStatCd.EditValue = _statCd;
            txtCustId.Text = row["cust_id"]?.ToString() ?? string.Empty;
            txtCustNm.Text = row["cust_nm"]?.ToString() ?? string.Empty;
            txtVendorDocNo.Text = row["vendor_doc_no"]?.ToString() ?? string.Empty;
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
            cboAccId.EditValue = Session.AccId?.ToString();
            txtDelvNo.Text = string.Empty;
            dteDelvDate.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");
            cboStatCd.EditValue = "0";
            txtCustId.Text = string.Empty;
            txtCustNm.Text = string.Empty;
            txtVendorDocNo.Text = string.Empty;
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

    /// <summary>확정된 납품은 화면 전체가 조회전용 - 편집기/그리드/불러오기/행삭제를 잠그고, 확정/확정취소 버튼은 상태에 맞게 켠다.</summary>
    private void ApplyLock()
    {
        var locked = _statCd == "C";
        foreach (var edit in new BaseEdit[] { dteDelvDate, txtCustNm, txtVendorDocNo, txtDeptNm, txtEmpNm, memoRemark })
            edit.Properties.ReadOnly = locked;
        gvw1.OptionsBehavior.Editable = !locked;
        btnLoadPo.Enabled = !locked;
        btnDeletRow1.Enabled = !locked;
        btnConfirm.Enabled = !locked && _editingKey != null;
        btnConfirmCancel.Enabled = locked;
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    /// <summary>발주 불러오기 - 승인 완료된 발주의 납품 가능 라인을 골라 가져온다. 이미 그리드에 있는 발주 라인은 건너뛴다.</summary>
    private async Task LoadFromPoAsync()
    {
        if (_statCd == "C") return;
        gvw1.CloseEditor();

        var columns = new[]
        {
            new PickColumn("po_no", "발주번호", 110), new PickColumn("po_date", "발주일자", 80), new PickColumn("cust_nm", "거래처", 110),
            new PickColumn("item_no", "품번", 100), new PickColumn("item_nm", "품명", 140), new PickColumn("item_spec", "규격", 110),
            new PickColumn("unit_cd", "단위", 50), new PickColumn("qty", "발주수량", 80, true), new PickColumn("next_qty", "납품인정", 80, true),
            new PickColumn("remain_qty", "잔량", 80, true), new PickColumn("allowed_qty", "납품가능", 80, true),
            new PickColumn("delv_date", "납기일", 80), new PickColumn("qc_yn", "검사", 45),
        };
        var picked = popPick.Pick(this, MenuId, "발주 불러오기", "USP_MA_PODELVPICK_Q", "발주번호", columns,
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
            var poId = r["po_id"].ToString();
            var poSerl = r["po_serl"].ToString();
            var dup = _detail.Rows.Cast<DataRow>().Any(d =>
                d.RowState != DataRowState.Deleted
                && d["src_type"]?.ToString() == "PO"
                && d["src_id"]?.ToString() == poId
                && d["src_serl"]?.ToString() == poSerl);
            if (dup) { skipped++; continue; }

            var qc = r["qc_yn"]?.ToString() == "Y" ? "Y" : "N";
            // 결과 컬럼 타입은 object(JSON 값) - 숫자 캐스트 대신 문자열 파싱으로 읽는다. 잔량이 없어도 허용율만큼 받을 수 있는 경우는 납품가능 수량.
            var remain = Dec(r["remain_qty"]) > 0 ? Dec(r["remain_qty"]) : Dec(r["allowed_qty"]);

            var row = _detail.NewRow();
            row["item_id"] = r["item_id"];
            row["item_no"] = r["item_no"];
            row["item_nm"] = r["item_nm"];
            row["item_spec"] = r["item_spec"];
            row["unit_cd"] = r["unit_cd"];
            row["delv_qty"] = remain;
            row["next_qty"] = 0;
            row["qc_yn"] = qc;
            row["qc_stat_nm"] = qc == "Y" ? "검사대기" : "면제";
            row["po_qty"] = r["qty"];
            row["po_remain_qty"] = r["remain_qty"];
            row["po_delv_date"] = r["delv_date"];
            row["wh_id"] = r["wh_id"];
            row["wh_nm"] = r["wh_nm"];
            row["loc_id"] = r["loc_id"];
            row["loc_nm"] = r["loc_nm"];
            row["src_type"] = "PO";
            row["src_id"] = r["po_id"];
            row["src_no"] = r["po_no"];
            row["src_serl"] = r["po_serl"];
            row["stop_yn"] = "N";
            _detail.Rows.Add(row);
        }

        if (skipped > 0) Toast.Show($"이미 불러온 발주 품목 {skipped}건은 건너뛰었습니다.");
    }

    public override async Task SaveClick()
    {
        if (_statCd == "C")
        {
            AppMessageBox.Show("확정된 납품은 수정할 수 없습니다. 먼저 확정취소하세요.", "안내");
            return;
        }
        if (string.IsNullOrWhiteSpace(txtCustId.Text))
        {
            AppMessageBox.Show("거래처를 입력하세요.", "안내");
            return;
        }

        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        // 서버가 다시 검증하지만, 헤더만 저장되고 라인이 실패하는 반쪽 저장을 줄이려고 기본 값은 먼저 확인한다.
        foreach (DataRow row in _detail.Rows)
        {
            if (row.RowState == DataRowState.Deleted || row.RowState == DataRowState.Unchanged) continue;
            if (!decimal.TryParse(row["delv_qty"]?.ToString(), out var q) || q <= 0)
            {
                AppMessageBox.Show($"'{row["item_nm"]}' 납품수량을 0보다 크게 입력하세요.", "안내");
                return;
            }
        }

        var headerParams = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_delv_id"] = _editingKey,
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_delv_date"] = dteDelvDate.YyyyMmDd,
            ["p_cust_id"] = txtCustId.Text,
            ["p_vendor_doc_no"] = txtVendorDocNo.Text,
            ["p_dept_id"] = txtDeptId.Text,
            ["p_emp_id"] = txtEmpId.Text,
            ["p_remark"] = memoRemark.Text,
        };
        var headerResult = await SaveAsync("USP_MA_DELV_S", headerParams);
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
                ["p_delv_id"] = headerKey,
                ["p_serl"] = ProcData.Str(row, "serl", version),
                ["p_delv_qty"] = ProcData.Str(row, "delv_qty", version),
                ["p_lot_no"] = ProcData.Str(row, "lot_no", version),
                ["p_wh_id"] = ProcData.Str(row, "wh_id", version),
                ["p_loc_id"] = ProcData.Str(row, "loc_id", version),
                ["p_src_type"] = ProcData.Str(row, "src_type", version),
                ["p_src_id"] = ProcData.Str(row, "src_id", version),
                ["p_src_serl"] = ProcData.Str(row, "src_serl", version),
                ["p_remark"] = ProcData.Str(row, "remark", version),
            };

            var detailResult = await SaveAsync("USP_MA_DELV_S_1", detailParams);
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

        var result = await SaveAsync("USP_MA_DELV_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = "D",
            ["p_delv_id"] = _editingKey,
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
            ? "납품을 확정하시겠습니까?\n확정하면 발주 라인의 납품수량이 반영되고, 검사대상 품목은 검사대기가 됩니다."
            : "납품 확정을 취소하시겠습니까?";
        if (AppMessageBox.Show(msg, confirm ? "납품 확정" : "납품 확정취소", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        var result = await SaveAsync("USP_MA_DELV_C_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = confirm ? "C" : "CC",
            ["p_delv_id"] = _editingKey,
        });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "처리에 실패했습니다.", confirm ? "확정 실패" : "확정취소 실패");
            return;
        }

        Toast.Show(confirm ? "확정되었습니다." : "확정이 취소되었습니다.");
        await QueryCore(forceKey: _editingKey);
    }

    /// <summary>납품현황(frmDelvList)에서 납품번호를 더블클릭했을 때 호출된다(key=delv_id 문자열).</summary>
    public override async Task FocusRecordAsync(string key) => await QueryCore(forceKey: key);
}
