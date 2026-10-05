using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Popup;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.AP;

/// <summary>
/// 명함신청서 화면 - 결재승인 프로세스 1차 테스트용. frmAcc(grd1 목록 + panData 상세)를 그대로
/// 따르고, 결재상신 하나만 결재엔진(ApprovalClient -> api/approvals/*, USP_AP_APPR_*)을 통해
/// 추가로 붙인다. 결재상태는 이 화면이 직접 갱신하지 않는다(Pull) - USP_HR_NAMECARD_Q가 매 조회마다
/// TAPDOC을 JOIN해서 최신 상태를 그대로 보여준다. THRNAMECARDREQ.stat_cd(진행상태)만 최종승인 시
/// USP_AP_APPR_S가 직접 'C'로 갱신한다(사장님 지시).
///
/// 승인/반려 처리는 이 화면이 아니라 결재함(frmApprInbox)의 몫이다 - 기안자 화면과 결재자 화면을
/// 분리해서 각자 책임을 명확히 했다.
/// </summary>
public partial class frmNameCardReq : BaseForm
{
    private DataTable _list = new();
    private long? _editingId; // null이면 신규모드

    public frmNameCardReq()
    {
        InitializeComponent();

        Text = "명함신청서";

        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;
        gvw1.Role = GridRoleWyn.Query;

        // 결재자 선택(P_EMP)은 공용 전자결재 화면(popApp)이 부서트리로 대체했으므로
        // 이 화면에서는 더 안 쓴다 - Designer.cs를 건드리는 대신 숨겨서 자리만 비워둔다.
        lblApprover.Visible = false;
        txtApprover.Visible = false;
        txtApproverEmpNo.Visible = false;

        // 표준 CRUD(조회/신규/저장/삭제) 버튼은 BaseForm 공통 툴바가 처리하지만, 전자결재 화면
        // 열기는 이 화면만의 고유 동작이라 panData에 직접 붙인다(ButtonWyn은 일반 Control이라
        // Designer의 BeginInit/EndInit 없이 코드에서 추가해도 안전 - frmDept의 grd2 행추가/삭제
        // 버튼과 같은 방식).
        var btnOpenApproval = new ButtonWyn
        {
            Text = "전자결재",
            Location = new System.Drawing.Point(95, 380),
            Size = new System.Drawing.Size(100, 26),
        };
        btnOpenApproval.Click += async (s, e) => await SafeExecuteAsync(OpenApprovalAsync, "전자결재");
        panData.Controls.Add(btnOpenApproval);

        // 출력물(명함신청서 미리보기/인쇄/PDF) - btnOpenApproval과 같은 이유로 코드에서 직접 추가.
        var btnPreviewPrint = new ButtonWyn
        {
            Text = "출력미리보기",
            Location = new System.Drawing.Point(205, 380),
            Size = new System.Drawing.Size(100, 26),
        };
        btnPreviewPrint.Click += (s, e) => SafeExecute(PreviewPrint, "출력미리보기");
        panData.Controls.Add(btnPreviewPrint);

        // 개발자용 마우스오버 툴팁(BindingField) - 실제 적용은 BaseForm.ApplyBindingFieldTooltips가
        // 공통으로 처리한다(Session.IsDeveloper일 때만). panData 개별 컨트롤은 Tag에 미리
        // 넣어둬야 잡힌다(2026-09-12 감사 - 이 화면엔 원래 빠져있었음).
        txtReqNo.Tag = new BindingFieldTag("req_no");
        txtStatCd.Tag = new BindingFieldTag("stat_cd");
        txtApprStatCd.Tag = new BindingFieldTag("appr_stat_cd");
        txtRegDt.Tag = new BindingFieldTag("reg_dt");
        txtDeptNm.Tag = new BindingFieldTag("dept_nm");
        txtReqEmpNm.Tag = new BindingFieldTag("emp_nm");
        txtAppNo.Tag = new BindingFieldTag("app_no");
        txtNameKor.Tag = new BindingFieldTag("name_kor");
        txtNameEng.Tag = new BindingFieldTag("name_eng");
        txtDeptKor.Tag = new BindingFieldTag("dept_kor");
        txtDeptEng.Tag = new BindingFieldTag("dept_eng");
        txtJobGrade.Tag = new BindingFieldTag("job_grade");
        txtMobile.Tag = new BindingFieldTag("mobile");
        txtEmail.Tag = new BindingFieldTag("email");
        memoRemark.Tag = new BindingFieldTag("remark");

        TrackDirty(panData);

        EnterNewMode();
    }

    public override async Task QueryClick() => await QueryCore(preserveSelection: false);

    /// <summary>결재함에서 이 화면의 대기건을 더블클릭했을 때 호출된다(BaseForm.FocusRecordAsync,
    /// key=req_id 문자열) - 검색조건 없이 그 건 하나만 조회해서 바로 상세에 채운다.</summary>
    public override async Task FocusRecordAsync(string key)
    {
        if (!long.TryParse(key, out var reqId)) return;

        _list = await QueryAsync("USP_HR_NAMECARD_Q", new { p_work_type = "Q", p_req_id = reqId });
        grd1.DataSource = _list;

        var row = FindRow(reqId);
        if (row != null) EnterEditMode(row);
    }

    private async Task QueryCore(bool preserveSelection)
    {
        var keyword = txtSearchQ.Text.Trim();

        _list = await QueryAsync("USP_HR_NAMECARD_Q", new
        {
            p_work_type = "Q",
            p_req_no = keyword
        });

        var editingId = preserveSelection ? _editingId : null;

        gvw1.FocusedRowObjectChanged -= Gvw1_FocusedRowObjectChanged;
        SuppressMasterRowSwitchConfirm(gvw1, () =>
        {
            grd1.DataSource = _list;

            if (editingId != null)
            {
                var handle = FindRowHandle(editingId.Value);
                if (handle != null) gvw1.FocusedRowHandle = handle.Value;
            }
        });
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        var row = editingId == null ? null : FindRow(editingId.Value);
        if (row != null) EnterEditMode(row);
        else if (gvw1.GetFocusedRow() is DataRowView focusedView) EnterEditMode(focusedView.Row);
        else EnterNewMode();
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    protected override bool ConfirmDeleteByDefault => false; // 삭제 확인창을 DeleteClick에서 직접 띄움(문서번호 등 상세 문구)

    public override async Task DeleteClick()
    {
        if (_editingId == null)
        {
            AppMessageBox.Show("삭제할 항목을 먼저 선택해주세요.", "안내");
            return;
        }

        if (!string.IsNullOrEmpty(txtAppNo.Text))
        {
            AppMessageBox.Show("이미 결재상신된 문서는 삭제할 수 없습니다.", "삭제 불가");
            return;
        }

        var confirm = AppMessageBox.Show(
            $"선택하신 명함신청을 삭제하시겠습니까?\n\n[{txtReqNo.Text}]",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await SaveAsync("USP_HR_NAMECARD_S", new
        {
            p_work_type = "D",
            p_req_id = _editingId
        });

        if (!result.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(result), "삭제 실패");
            return;
        }

        _editingId = null;
        await QueryClick();
        Toast.Show("삭제되었습니다.");
    }

    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;

    public override async Task SaveClick()
    {
        if (IsApprovalLockedStatus(txtApprStatCd.Text))
        {
            AppMessageBox.Show("결재 상신된 문서는 수정할 수 없습니다.", "안내");
            return;
        }
        if (string.IsNullOrWhiteSpace(txtNameKor.Text))
        {
            AppMessageBox.Show("성명(한글)은 필수입니다.", "확인");
            return;
        }

        var wasNew = _editingId == null;

        var result = await SaveAsync("USP_HR_NAMECARD_S", new
        {
            p_work_type = wasNew ? "N" : "U",
            p_req_id = wasNew ? null : (object?)_editingId,
            p_job_grade = txtJobGrade.Text,
            p_name_kor = txtNameKor.Text,
            p_name_eng = txtNameEng.Text,
            p_dept_kor = txtDeptKor.Text,
            p_dept_eng = txtDeptEng.Text,
            p_mobile = txtMobile.Text,
            p_email = txtEmail.Text,
            p_remark = memoRemark.Text
        });

        if (!result.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(result), "저장 실패");
            return;
        }

        _editingId = wasNew && result.GeneratedCode != null ? long.Parse(result.GeneratedCode) : _editingId;
        await QueryCore(preserveSelection: true);
        Toast.Show(wasNew ? "등록되었습니다." : "수정되었습니다.");
    }

    /// <summary>전자결재 공용 팝업(popApp)을 연다 - 저장 안 된 신규 건이면 먼저 저장부터
    /// 하고, 상신/승인/반려/취소/확인 중 뭐든 실제로 처리됐으면(반환값 true) 재조회해서 결재번호/
    /// 결재상태를 그대로 화면에 반영한다(Pull).</summary>
    private async Task OpenApprovalAsync()
    {
        if (_editingId == null)
        {
            AppMessageBox.Show("먼저 저장한 뒤 전자결재를 열어주세요.", "안내");
            return;
        }

        var changed = await popApp.ShowAsync(
            "NAMECARD", _editingId.Value, txtReqNo.Text,
            $"명함신청서 - {txtNameKor.Text}", memoRemark.Text, this);

        if (changed) await QueryCore(preserveSelection: true);
    }

    /// <summary>출력물 미리보기 - 현재 선택된 건 1개만 담은 단일행 DataTable을 리포트에 물린다
    /// (grd1의 _list 전체를 그대로 물리면 Detail 밴드가 검색결과 행 수만큼 반복 출력된다).
    /// ReportPrintTool.ShowPreviewDialog() 자체가 인쇄/PDF 등 내보내기 버튼을 갖춘 모달 미리보기
    /// 창이라 별도의 공용 팝업을 새로 만들 필요가 없다.</summary>
    private void PreviewPrint()
    {
        if (_editingId == null)
        {
            AppMessageBox.Show("먼저 저장한 뒤 출력해주세요.", "안내");
            return;
        }

        var row = FindRow(_editingId.Value);
        if (row == null) return;

        var singleRow = row.Table.Clone();
        singleRow.ImportRow(row);

        using var report = new Report.rptNameCardReq { DataSource = singleRow };
        using var tool = new DevExpress.XtraReports.UI.ReportPrintTool(report);
        tool.ShowPreviewDialog();
    }

    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e) =>
        ConfirmMasterRowSwitch(gvw1, e, row => EnterEditMode(row.Row));

    private DataRow? FindRow(long id) =>
        _list.Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToInt64(r["req_id"]) == id);

    /// <summary>키(req_id)로 grd1의 행 핸들을 찾는다. 없으면 null. 컬럼 셀 값으로 찾는 방식은
    /// 그 키 컬럼이 화면에 안 보이는 숨김 컬럼일 때 안 먹힌다(2026-09-11 실제 발견, frmEMP에서
    /// 재현) - FindRow로 찾은 DataRow의 DataTable상 인덱스를 GridView.GetRowHandle로 표시 행
    /// 핸들로 변환하면 키 컬럼이 보이든 안 보이든 항상 동작한다.</summary>
    private int? FindRowHandle(long id)
    {
        var row = FindRow(id);
        if (row == null) return null;

        var rowIndex = _list.Rows.IndexOf(row);
        var handle = gvw1.GetRowHandle(rowIndex);
        return handle >= 0 ? handle : null;
    }

    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingId = null;
            txtReqNo.Text = string.Empty;
            txtStatCd.Text = string.Empty;
            txtApprStatCd.Text = string.Empty;
            txtRegDt.Text = string.Empty;
            txtDeptNm.Text = Session.DeptNm;
            txtReqEmpNm.Text = Session.UserNm;
            txtAppNo.Text = string.Empty;
            txtNameKor.Text = Session.UserNm;
            txtNameEng.Text = string.Empty;
            txtDeptKor.Text = Session.DeptNm;
            txtDeptEng.Text = string.Empty;
            txtJobGrade.Text = string.Empty;
            txtMobile.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtApprover.Text = string.Empty;
            txtApproverEmpNo.Text = string.Empty;
            memoRemark.Text = string.Empty;
        });
        txtSearchQ.Focus();
        ApplyLockState();
    }

    private void EnterEditMode(DataRow row)
    {
        SuppressDirtyTracking(() =>
        {
            _editingId = Convert.ToInt64(row["req_id"]);
            txtReqNo.Text = Str(row, "req_no");
            txtStatCd.Text = Str(row, "stat_cd");
            txtApprStatCd.Text = Str(row, "appr_stat_cd");
            txtRegDt.Text = Str(row, "reg_dt");
            txtDeptNm.Text = Str(row, "dept_nm");
            txtReqEmpNm.Text = Str(row, "emp_nm");
            txtAppNo.Text = Str(row, "app_no");
            txtNameKor.Text = Str(row, "name_kor");
            txtNameEng.Text = Str(row, "name_eng");
            txtDeptKor.Text = Str(row, "dept_kor");
            txtDeptEng.Text = Str(row, "dept_eng");
            txtJobGrade.Text = Str(row, "job_grade");
            txtMobile.Text = Str(row, "mobile");
            txtEmail.Text = Str(row, "email");
            txtApprover.Text = string.Empty;
            txtApproverEmpNo.Text = string.Empty;
            memoRemark.Text = Str(row, "remark");
        });
        ApplyLockState();
    }

    /// <summary>결재 상신된(상신/진행중/승인완료) 신청서는 입력 항목을 수정하지 못하게 잠근다 - 반려/상신 취소되면 다시 수정 가능(서버도 같은 기준).</summary>
    private void ApplyLockState() => ApplyApprovalLock(IsApprovalLockedStatus(txtApprStatCd.Text), panData, null);

    private static string Str(DataRow row, string columnName) =>
        row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value
            ? Convert.ToString(row[columnName]) ?? string.Empty
            : string.Empty;

    private static string FormatSaveFailMessage(ApiResult? result)
    {
        var message = result?.Message ?? "저장에 실패했습니다.";
        return result is { ErrorCode: not 0 } ? $"{message} (오류코드: {result.ErrorCode})" : message;
    }
}
