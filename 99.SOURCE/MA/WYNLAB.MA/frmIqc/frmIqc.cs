using System.Data;
using DevExpress.XtraEditors;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

/// <summary>
/// 수입검사등록 - 확정된 납품 중 검사대상(qc_yn='Y') 라인의 미검사 잔량을 "검사대기 불러오기"(popPick,
/// USP_MA_DELVIQCPICK_Q)로 가져와 합격/특채/불합격 수량을 판정한다(TMAIQCM/TMAIQCD). frmDelv와 같은 구조.
///
/// 판정 규칙(서버 USP_MA_IQC_S_1이 다시 검증): 합격+특채+불합격=검사수량, 불합격이 있으면 불량유형/처분(반품/폐기) 필수,
/// 특채가 있으면 비고에 특채 사유 필수. 화면은 검사수량/특채/불합격을 바꾸면 합격 수량을 나머지로 자동 계산한다.
/// 확정(USP_MA_IQC_C_S)하면 합격+특채 수량이 입고대기(VMA_GR_READY)로 넘어가고, 반품 처분 수량은 발주 잔량으로
/// 복원된다. 확정 후에는 조회전용이고 입고가 진행된 검사는 확정취소도 서버가 막는다.
/// </summary>
public partial class frmIqc : BaseForm
{
    private DataTable _detail = new();
    private string? _editingKey; // null이면 신규모드
    private string _statCd = "0";
    private bool _syncingQty; // 합격 자동계산 재귀 가드

    public frmIqc()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준(2026-10-03): 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "수입검사등록";


        txtDeptNm.MapField("dept_id", txtDeptId);
        txtEmpNm.MapField("emp_id", txtEmpId);
        txtEmpNm.LinkDept(txtDeptNm, txtDeptId); // 담당자 팝업은 선택한 부서 소속만, 담당자를 고르면 부서도 채움
        txtCustNm.MapField("cust_id", txtCustId);

        gvw1.Role = GridRoleWyn.Edit;
        gvw1.HighlightFocusedRow = true;
        gvw1.RowAdd += (s, e) => AppMessageBox.Show("수입검사 품목은 '검사대기 불러오기'로만 추가할 수 있습니다.", "안내");
        gvw1.RowDelete += (s, e) => DeleteFocusedRow();
        gvw1.CellValueChanged += Gvw1_CellValueChanged;
        btnDeletRow1.Click += (s, e) => DeleteFocusedRow();
        btnLoadDelv.Click += async (s, e) => await SafeExecuteAsync(LoadFromDelvAsync, "검사대기 불러오기");
        btnConfirm.Click += async (s, e) => await SafeExecuteAsync(() => ConfirmAsync(true), "수입검사 확정");
        btnConfirmCancel.Click += async (s, e) => await SafeExecuteAsync(() => ConfirmAsync(false), "수입검사 확정취소");

        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) => await SafeExecuteAsync(EnsureDetailSchemaAsync, "수입검사 화면 초기화");
    }

    private static decimal Dec(object? value) => decimal.TryParse(value?.ToString(), out var d) ? d : 0;

    private void DeleteFocusedRow()
    {
        if (_statCd == "C") return;
        try { if (gvw1.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
        catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
    }

    /// <summary>검사수량/특채/불합격을 고치면 합격 수량을 "검사수량 - 특채 - 불합격"으로 다시 채운다(음수가 되면 그대로 두고
    /// 저장 때 합계 불일치로 알려준다). 표본수는 검사수량을 따라가되 사용자가 따로 입력한 값은 유지한다.</summary>
    private void Gvw1_CellValueChanged(object? sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
    {
        if (_syncingQty || _statCd == "C") return;
        if (e.Column != colInspQty && e.Column != colConcQty && e.Column != colFailQty) return;

        var insp = Dec(gvw1.GetRowCellValue(e.RowHandle, colInspQty));
        var conc = Dec(gvw1.GetRowCellValue(e.RowHandle, colConcQty));
        var fail = Dec(gvw1.GetRowCellValue(e.RowHandle, colFailQty));
        var pass = insp - conc - fail;

        _syncingQty = true;
        try
        {
            if (pass >= 0) gvw1.SetRowCellValue(e.RowHandle, colPassQty, pass);
            if (e.Column == colInspQty && Dec(gvw1.GetRowCellValue(e.RowHandle, colSampleQty)) == 0)
                gvw1.SetRowCellValue(e.RowHandle, colSampleQty, insp);
        }
        finally
        {
            _syncingQty = false;
        }
    }

    /// <summary>새 폼은 아직 조회한 적이 없어 _detail에 컬럼이 없다 - frmDelv와 같은 이유로 스키마만 먼저 받아 둔다.</summary>
    private async Task EnsureDetailSchemaAsync()
    {
        if (_detail.Columns.Count > 0) return;

        var tables = await QueryMultiAsync("USP_MA_IQC_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q", ["p_iqc_id"] = "-1" });
        if (tables.Count > 1) _detail = tables[1];
        EnterNewMode();
    }

    public override async Task QueryClick() => await QueryCore(forceKey: null);

    private async Task QueryCore(string? forceKey)
    {
        // 조회 공통 규칙(2026-09-26): 결과가 없으면 신규 입력 모드로 전환한다. 검색 번호를 안 넣은 조회는 서버가 "조건 없음"을 "가장 최근 문서 1건"으로 처리하므로,
        // 조회할 대상이 없는 것으로 보고 서버를 부르지 않고 바로 신규 모드로 간다.
        if (forceKey == null && string.IsNullOrWhiteSpace(txtSearchIqcNo.Text))
        {
            EnterNewMode();
            Toast.Show("조회할 번호가 없어 신규 입력 상태로 전환했습니다.");
            return;
        }
        var p = new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "Q",
            ["p_iqc_id"] = forceKey,
            ["p_iqc_no"] = forceKey == null ? txtSearchIqcNo.Text : null,
        };
        var tables = await QueryMultiAsync("USP_MA_IQC_Q", p);
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
            _editingKey = row["iqc_id"]?.ToString();
            _statCd = row["stat_cd"]?.ToString() ?? "0";
            cboAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            txtIqcNo.Text = row["iqc_no"]?.ToString() ?? string.Empty;
            txtSearchIqcNo.Text = txtIqcNo.Text; // 링크로 열었거나 저장 후에도 조회 버튼이 현재 문서를 다시 읽도록
            dteIqcDate.YyyyMmDd = row["iqc_date"]?.ToString();
            cboStatCd.EditValue = _statCd;
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
            cboAccId.EditValue = Session.AccId?.ToString();
            txtIqcNo.Text = string.Empty;
            dteIqcDate.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");
            cboStatCd.EditValue = "0";
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

    private void ApplyLock()
    {
        var locked = _statCd == "C";
        foreach (var edit in new BaseEdit[] { dteIqcDate, txtCustNm, txtDeptNm, txtEmpNm, memoRemark })
            edit.Properties.ReadOnly = locked;
        gvw1.OptionsBehavior.Editable = !locked;
        btnLoadDelv.Enabled = !locked;
        btnDeletRow1.Enabled = !locked;
        btnConfirm.Enabled = !locked && _editingKey != null;
        btnConfirmCancel.Enabled = locked;
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    /// <summary>검사대기 불러오기 - 확정 납품 중 검사대상이고 미검사 잔량이 있는 라인을 가져온다. 기본 판정은 전량 합격이다.</summary>
    private async Task LoadFromDelvAsync()
    {
        if (_statCd == "C") return;
        gvw1.CloseEditor();

        var columns = new[]
        {
            new PickColumn("delv_no", "납품번호", 110), new PickColumn("delv_date", "납품일자", 80), new PickColumn("cust_nm", "거래처", 110),
            new PickColumn("item_no", "품번", 100), new PickColumn("item_nm", "품명", 140), new PickColumn("item_spec", "규격", 110),
            new PickColumn("unit_cd", "단위", 50), new PickColumn("lot_no", "LOT", 90), new PickColumn("delv_qty", "납품수량", 80, true),
            new PickColumn("next_qty", "검사완료", 80, true), new PickColumn("wait_qty", "검사대기", 80, true),
            new PickColumn("wait_days", "경과일", 60), new PickColumn("po_no", "발주번호", 110),
        };
        var picked = popPick.Pick(this, MenuId, "검사대기 불러오기", "USP_MA_DELVIQCPICK_Q", "납품번호", columns,
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
            var delvId = r["delv_id"].ToString();
            var delvSerl = r["delv_serl"].ToString();
            var dup = _detail.Rows.Cast<DataRow>().Any(d =>
                d.RowState != DataRowState.Deleted
                && d["src_type"]?.ToString() == "DELV"
                && d["src_id"]?.ToString() == delvId
                && d["src_serl"]?.ToString() == delvSerl);
            if (dup) { skipped++; continue; }

            var wait = Dec(r["wait_qty"]);
            var row = _detail.NewRow();
            row["item_id"] = r["item_id"];
            row["item_no"] = r["item_no"];
            row["item_nm"] = r["item_nm"];
            row["item_spec"] = r["item_spec"];
            row["unit_cd"] = r["unit_cd"];
            row["lot_no"] = r["lot_no"];
            row["po_no"] = r["po_no"];
            row["delv_qty"] = r["delv_qty"];
            row["insp_qty"] = wait;
            row["sample_qty"] = wait;
            row["pass_qty"] = wait;
            row["conc_qty"] = 0;
            row["fail_qty"] = 0;
            row["next_qty"] = 0;
            row["wh_id"] = r["wh_id"];
            row["wh_nm"] = r["wh_nm"];
            row["loc_id"] = r["loc_id"];
            row["loc_nm"] = r["loc_nm"];
            row["src_type"] = "DELV";
            row["src_id"] = r["delv_id"];
            row["src_no"] = r["delv_no"];
            row["src_serl"] = r["delv_serl"];
            _detail.Rows.Add(row);
        }

        if (skipped > 0) Toast.Show($"이미 불러온 납품 품목 {skipped}건은 건너뛰었습니다.");
    }

    /// <summary>서버가 저장 때마다 같은 검증을 하지만, 헤더만 저장되고 라인이 실패하는 반쪽 저장을 줄이려고 판정 규칙을 먼저 확인한다.</summary>
    private bool ValidateRows()
    {
        foreach (DataRow row in _detail.Rows)
        {
            if (row.RowState == DataRowState.Deleted || row.RowState == DataRowState.Unchanged) continue;

            var name = $"{row["item_nm"]} (순번 {row["serl"]})";
            var insp = Dec(row["insp_qty"]);
            var pass = Dec(row["pass_qty"]);
            var conc = Dec(row["conc_qty"]);
            var fail = Dec(row["fail_qty"]);

            if (insp <= 0) { AppMessageBox.Show($"{name}: 검사수량을 0보다 크게 입력하세요.", "안내"); return false; }
            if (pass + conc + fail != insp)
            {
                AppMessageBox.Show($"{name}: 합격+특채+불합격({pass + conc + fail:#,0.####})이 검사수량({insp:#,0.####})과 다릅니다.", "안내");
                return false;
            }
            if (fail > 0 && (string.IsNullOrEmpty(row["fail_reason_cd"]?.ToString()) || string.IsNullOrEmpty(row["fail_action_cd"]?.ToString())))
            {
                AppMessageBox.Show($"{name}: 불합격 수량이 있으면 불량유형과 불합격처분(반품/폐기)을 선택하세요.", "안내");
                return false;
            }
            if (conc > 0 && string.IsNullOrWhiteSpace(row["remark"]?.ToString()))
            {
                AppMessageBox.Show($"{name}: 특채 수량이 있으면 비고에 특채 사유를 입력하세요.", "안내");
                return false;
            }
        }
        return true;
    }

    public override async Task SaveClick()
    {
        if (_statCd == "C")
        {
            AppMessageBox.Show("확정된 수입검사는 수정할 수 없습니다. 먼저 확정취소하세요.", "안내");
            return;
        }
        if (string.IsNullOrWhiteSpace(txtCustId.Text))
        {
            AppMessageBox.Show("거래처를 입력하세요.", "안내");
            return;
        }

        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();
        if (!ValidateRows()) return;

        var headerParams = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_iqc_id"] = _editingKey,
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_iqc_date"] = dteIqcDate.YyyyMmDd,
            ["p_cust_id"] = txtCustId.Text,
            ["p_dept_id"] = txtDeptId.Text,
            ["p_emp_id"] = txtEmpId.Text,
            ["p_remark"] = memoRemark.Text,
        };
        var headerResult = await SaveAsync("USP_MA_IQC_S", headerParams);
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
                ["p_iqc_id"] = headerKey,
                ["p_serl"] = ProcData.Str(row, "serl", version),
                ["p_insp_qty"] = ProcData.Str(row, "insp_qty", version),
                ["p_sample_qty"] = ProcData.Str(row, "sample_qty", version),
                ["p_pass_qty"] = ProcData.Str(row, "pass_qty", version),
                ["p_conc_qty"] = ProcData.Str(row, "conc_qty", version),
                ["p_fail_qty"] = ProcData.Str(row, "fail_qty", version),
                ["p_fail_reason_cd"] = ProcData.Str(row, "fail_reason_cd", version),
                ["p_fail_action_cd"] = ProcData.Str(row, "fail_action_cd", version),
                ["p_wh_id"] = ProcData.Str(row, "wh_id", version),
                ["p_loc_id"] = ProcData.Str(row, "loc_id", version),
                ["p_src_type"] = ProcData.Str(row, "src_type", version),
                ["p_src_id"] = ProcData.Str(row, "src_id", version),
                ["p_src_serl"] = ProcData.Str(row, "src_serl", version),
                ["p_remark"] = ProcData.Str(row, "remark", version),
            };

            var detailResult = await SaveAsync("USP_MA_IQC_S_1", detailParams);
            if (detailResult == null || !detailResult.Success)
            {
                AppMessageBox.Show(detailResult?.Message ?? "품목 저장에 실패했습니다.", "저장 실패");
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

        var result = await SaveAsync("USP_MA_IQC_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = "D",
            ["p_iqc_id"] = _editingKey,
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
            ? "검사 판정을 확정하시겠습니까?\n확정하면 합격+특채 수량이 입고대기로 넘어가고, 반품 처분 수량은 발주 잔량으로 복원됩니다."
            : "수입검사 확정을 취소하시겠습니까?";
        if (AppMessageBox.Show(msg, confirm ? "수입검사 확정" : "수입검사 확정취소", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        var result = await SaveAsync("USP_MA_IQC_C_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = confirm ? "C" : "CC",
            ["p_iqc_id"] = _editingKey,
        });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "처리에 실패했습니다.", confirm ? "확정 실패" : "확정취소 실패");
            return;
        }

        Toast.Show(confirm ? "확정되었습니다." : "확정이 취소되었습니다.");
        await QueryCore(forceKey: _editingKey);
    }

    /// <summary>수입검사현황(frmIqcList)에서 검사번호를 더블클릭했을 때 호출된다(key=iqc_id 문자열).</summary>
    public override async Task FocusRecordAsync(string key) => await QueryCore(forceKey: key);
}
