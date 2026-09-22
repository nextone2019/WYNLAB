// AI Builder가 마스터-폼-서브그리드 템플릿을 복제해서 자동 생성 - 2026-09-09.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
//
// 2026-09-09 수정 - 생성 직후 사용자가 VS 디자이너에서 화면을 다듬으면서(사원 사진/계정/부서
// 컨트롤을 더 나은 것으로 교체) 하위그리드(grd2~grd5) 탭 전체와 numDetailAccId/cboDetailDeptNm을
// 지웠는데, 이 코드비하인드는 그 변경을 안 따라가서 존재하지 않는 컨트롤을 참조해 빌드가 깨져
// 있었다 - 지워진 컨트롤 참조를 전부 정리하고, 새로 추가된 실제 컨트롤(cboDetailAccCd=계정
// LookUp, txtDetailDeptNm=부서명 표시, picEmpPhoto=PictureEditWyn 사원사진)에 맞게 다시 연결했다.
// USP_BA_EMP_Q/USP_BA_EMP_S도 emp_id(진짜 PK)를 다루도록 같이 고쳤다(120_USP_BA_EMP_EmpId_Fix.sql
// 참고 - EMP_ID가 조회 결과에 아예 없었고, 저장도 emp_no로만 걸려 있었다).
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmEMP : BaseForm
{
    private DataTable _list = new();
    private string? _editingKey; // null이면 신규모드

    public frmEMP()
    {
        InitializeComponent();

        Text = "사원등록";

        Controls.Add(BuildScreenHeader());

        // txtDetailDeptNm(부서)에 "영업"처럼 이름을 직접 치고 포커스를 벗어나면 자동조회되도록
        // - MatchField는 Designer.cs(LookupKey 옆)에 지정돼 있지만, 실제 채워질 대상(숨겨진
        // 진짜 FK인 txtDetailDeptId)을 MapField로 등록해줘야 PopupLookupEditWyn.OnLeaveAsync가
        // 동작한다(2026-09-10 실제로 겪음 - MapField가 아예 없어서 타이핑해도 팝업이 안 떴다).
        txtDetailDeptNm.MapField("DEPT_ID", txtDetailDeptId);

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        // 다른 마스터 행을 고를 때 panData에 저장 안 된 변경이 있으면 먼저 확인한다
        // (BaseForm.ConfirmMasterRowSwitch 참고, 2026-09-06 - 모든 화면 공통 적용). 이름 있는
        // 메서드로 등록해야 QueryCore가 grd1을 다시 그리는 동안 잠깐 구독을 끊을 수 있다(바로 아래
        // Gvw1_FocusedRowObjectChanged 설명 참고) - 람다로 등록하면 나중에 뗄 방법이 없다.
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        // 개발자용 마우스오버 툴팁(BindingField) - 실제 적용은 BaseForm.ApplyBindingFieldTooltips가
        // 공통으로 처리한다(Session.IsDeveloper일 때만). 그리드 컬럼은 FieldName이 이미 DB
        // 컬럼명이라 자동 적용되지만, panData 개별 컨트롤은 Tag에 미리 넣어둬야 잡힌다(2026-09-12
        // 감사 - 이 화면엔 원래 빠져있었음, "사원등록에서 툴팁이 안 뜬다"로 실제 발견).
        cboDetailAccCd.Tag = new BindingFieldTag("acc_id");
        txtDetailEmpNo.Tag = new BindingFieldTag("emp_no");
        txtDetailEmpNm.Tag = new BindingFieldTag("emp_nm");
        txtDetailEmpNmEng.Tag = new BindingFieldTag("emp_nm_eng");
        txtDetailDeptId.Tag = new BindingFieldTag("dept_id");
        txtDetailDeptNm.Tag = new BindingFieldTag("dept_nm");
        dteDetailEntDate.Tag = new BindingFieldTag("ent_date");
        dteDetailGrpEntDate.Tag = new BindingFieldTag("grp_ent_date");
        txtDetailJobGrade.Tag = new BindingFieldTag("job_grade");
        txtDetailJobType.Tag = new BindingFieldTag("job_type");
        chkDetailRetYn.Tag = new BindingFieldTag("ret_yn");
        dteDetailRetDate.Tag = new BindingFieldTag("ret_date");
        cboDetailSexCd.Tag = new BindingFieldTag("sex_cd");
        txtDetailTel.Tag = new BindingFieldTag("tel");
        txtDetailHpTel.Tag = new BindingFieldTag("hp_tel");
        txtDetailEmail.Tag = new BindingFieldTag("email");
        txtDetailNatCd.Tag = new BindingFieldTag("nat_cd");
        txtDetailZipCode.Tag = new BindingFieldTag("zip_code");
        txtDetailAddr1.Tag = new BindingFieldTag("addr1");
        txtDetailAddr2.Tag = new BindingFieldTag("addr2");
        chkDetailHoliYn.Tag = new BindingFieldTag("holi_yn");
        chkDetailDiligYn.Tag = new BindingFieldTag("dilig_yn");
        chkDetailPayYn.Tag = new BindingFieldTag("pay_yn");
        picEmpPhoto.Tag = new BindingFieldTag("photo");

        // 화면종료 시 저장 확인(BaseForm.ConfirmCloseAsync)과 다른 마스터 행으로 옮길 때 확인
        // (ConfirmMasterRowSwitch) 둘 다 이 추적에 기댄다 - picEmpPhoto(PictureEditWyn)도 BaseEdit
        // 계열이라 이미지가 바뀌면 자동으로 여기 걸린다(frmSiteConfig의 PictureEdit과 같은 이유).
        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick() => await QueryCore(preserveSelection: false);

    /// <summary>사용자가 직접 누른 조회(preserveSelection: false)는 새 검색이므로 0번 행부터,
    /// 저장/삭제 뒤의 내부 재조회(preserveSelection: true)는 방금 편집하던 행에 포커스를 되돌려
    /// panData까지 그 값 그대로 다시 채운다 - 안 그러면 저장 직후 조회했을 때 panData가 안
    /// 채워진 것처럼 보인다(2026-09-06 요청 - 새 화면을 이 템플릿에서 복제하면 기본으로 따라온다.
    /// 지우지 말 것).</summary>
    private async Task QueryCore(bool preserveSelection)
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_emp_no"] = txtEmpNo.Text,
            // p_dept_id는 서버에서 BIGINT 파라미터라서 빈 문자열을 그대로 보내면 숫자 변환 오류가
            // 난다(2026-09-09 실제로 겪음) - 비어있으면 null로 보내야 NULL 파라미터로 바인딩된다.
            ["p_dept_id"] = string.IsNullOrWhiteSpace(txtDeptId.Text) ? null : txtDeptId.Text,
        };
        _list = await QueryAsync("USP_BA_EMP_Q", p);

        var editingKey = preserveSelection ? _editingKey : null;

        // 저장 직후(SaveClick -> QueryCore)엔 아직 IsDirty가 true로 남아있을 수 있다(DataTable.
        // AcceptChanges()는 RowChanged를 안 냄) - 이 재바인딩이 그 상태에서 자동으로 0번 행에
        // 포커스를 주면 ConfirmMasterRowSwitch가 "변경사항이 있다"고 또 확인창을 띄우는 오작동이
        // 생긴다. 아래에서 직접 EnterNewMode/OnMasterSelectedAsync를 호출해 최종 상태를 맞추므로
        // 이 재바인딩 구간만 조용히 지나가면 된다.
        //
        // 구독을 잠깐 끊는 이유(2026-09-08 실제 발견): SuppressMasterRowSwitchConfirm은 "확인창"만
        // 막지, 안에서 gvw1.FocusedRowHandle을 바꾸면 FocusedRowObjectChanged 자체는 그대로
        // 발생한다 - 그 이벤트가 다시 OnMasterSelectedAsync를 fire-and-forget으로 부르고, 바로
        // 아래에서 같은 행에 대해 또 한 번(이번엔 await로) OnMasterSelectedAsync를 부른다. 저장
        // 직후처럼 그 두 호출이 겹치면 나중에 끝나는 쪽이 먼저 끝난 쪽의 TrackDirty 구독/그리드
        // 바인딩을 덮어써서, panData를 닫아도 될 상태로 다시 정리했다고 여겼던 IsDirty가 조용히
        // 다시 true가 되는 경우가 있었다(저장 버튼을 누르고 바로 탭을 닫아도 "변경 내역이
        // 존재합니다" 확인창이 뜸). 아래 명시적 호출 하나만으로 충분하므로, 재바인딩 구간에서는
        // 이벤트를 끊어 중복 호출 자체를 없앤다(frmAcc.cs가 이미 쓰던 패턴).
        gvw1.FocusedRowObjectChanged -= Gvw1_FocusedRowObjectChanged;
        SuppressMasterRowSwitchConfirm(gvw1, () =>
        {
            grd1.DataSource = _list;
            if (editingKey != null)
            {
                var handle = FindRowHandle(editingKey);
                if (handle != null) gvw1.FocusedRowHandle = handle.Value;
            }
        });
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        var row = editingKey == null ? null : FindRow(editingKey);
        if (row != null) OnMasterSelected(row);
        // editingKey가 없으면(최초 조회, preserveSelection=false) grd1을 바인딩한 직후 DevExpress가
        // 스스로 0번 행에 포커스를 준다 - 그 자동 포커스 행을 그대로 panData에 채운다. 여기서
        // EnterNewMode()로 바로 넘어가면(예전 버전의 실제 버그) 목록엔 데이터가 있는데 처음 눌러보기
        // 전까지 panData는 계속 비어있는 것처럼 보인다(2026-09-08 실제 발견 - frmItem 최초 조회 시
        // panData 안 채워짐). frmAcc.cs가 이미 쓰던 패턴 그대로 가져온다.
        else if (gvw1.GetFocusedRow() is DataRowView focusedView) OnMasterSelected(focusedView.Row);
        else EnterNewMode();
    }

    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e) =>
        ConfirmMasterRowSwitch(gvw1, e, row => OnMasterSelected(row.Row));

    /// <summary>조회된 목록에서 키(emp_id)로 행을 찾는다. 없으면 null.</summary>
    private DataRow? FindRow(string key) =>
        _list.Rows.Cast<DataRow>()
            .FirstOrDefault(r => string.Equals(Convert.ToString(r["emp_id"]), key, StringComparison.OrdinalIgnoreCase));

    /// <summary>키(emp_id)로 grd1의 행 핸들을 찾는다. 없으면 null. colMEmpId가 화면에 안 보이는
    /// 숨김 컬럼(Visible=false)이라 컬럼 셀 값으로 찾는 방식은 못 쓴다 - FindRow로 찾은 DataRow의
    /// DataTable상 인덱스를 GridView.GetRowHandle로 표시 행 핸들로 변환한다(frmCust.FindRowHandle과
    /// 같은 이유/패턴 - "그리드에 보이는 컬럼이 없어도 동작함". 2026-09-11 실제 발견 - 저장 후
    /// 재조회하면 선택된 행이 안 돌아오고 항상 0번 행으로 가던 버그, 컬럼 기반 조회가 숨김 컬럼에서
    /// 안 먹혀서였다).</summary>
    private int? FindRowHandle(string key)
    {
        var row = FindRow(key);
        if (row == null) return null;

        var rowIndex = _list.Rows.IndexOf(row);
        var handle = gvw1.GetRowHandle(rowIndex);
        return handle >= 0 ? handle : null;
    }

    private void OnMasterSelected(DataRow row)
    {
        // 코드가 값을 채우는 것뿐인데 TrackDirty(panData)가 "사용자가 고쳤다"로 오인하지 않게 감싼다.
        SuppressDirtyTracking(() =>
        {
            _editingKey = row["emp_id"]?.ToString();
            cboDetailAccCd.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            txtDetailEmpNo.Text = row["emp_no"]?.ToString() ?? string.Empty;
            txtDetailEmpNm.Text = row["emp_nm"]?.ToString() ?? string.Empty;
            txtDetailEmpNmEng.Text = row["emp_nm_eng"]?.ToString() ?? string.Empty;
            txtDetailDeptId.EditValue = decimal.TryParse(row["DEPT_ID"]?.ToString(), out var numDeptId) ? numDeptId : (decimal?)null;
            txtDetailDeptNm.Text = row["dept_nm"]?.ToString() ?? string.Empty;
            dteDetailEntDate.YyyyMmDd = row["ent_date"]?.ToString();
            dteDetailGrpEntDate.YyyyMmDd = row["grp_ent_date"]?.ToString();
            txtDetailJobGrade.Text = row["job_grade"]?.ToString() ?? string.Empty;
            txtDetailJobType.Text = row["job_type"]?.ToString() ?? string.Empty;
            chkDetailRetYn.Checked = row["ret_yn"]?.ToString() == "Y";
            dteDetailRetDate.YyyyMmDd = row["ret_date"]?.ToString();
            cboDetailSexCd.EditValue = row["sex_cd"]?.ToString() ?? string.Empty;
            txtDetailTel.Text = row["tel"]?.ToString() ?? string.Empty;
            txtDetailHpTel.Text = row["hp_tel"]?.ToString() ?? string.Empty;
            txtDetailEmail.Text = row["email"]?.ToString() ?? string.Empty;
            txtDetailNatCd.Text = row["nat_cd"]?.ToString() ?? string.Empty;
            txtDetailZipCode.Text = row["zip_code"]?.ToString() ?? string.Empty;
            txtDetailAddr1.Text = row["addr1"]?.ToString() ?? string.Empty;
            txtDetailAddr2.Text = row["addr2"]?.ToString() ?? string.Empty;
            chkDetailHoliYn.Checked = row["holi_yn"]?.ToString() == "Y";
            chkDetailDiligYn.Checked = row["dilig_yn"]?.ToString() == "Y";
            chkDetailPayYn.Checked = row["pay_yn"]?.ToString() == "Y";
            SetPhotoFromBase64(row["photo"]?.ToString());
        });
    }

    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = null;
            // 신규입력 시 계정을 매번 고르게 하지 않고 로그인 세션의 계정을 기본값으로 채운다
            // (2026-09-09 요청) - 필요하면 사용자가 직접 다른 계정으로 바꿀 수 있다.
            cboDetailAccCd.EditValue = Session.AccId?.ToString() ?? string.Empty;
            txtDetailEmpNo.Text = string.Empty;
            txtDetailEmpNm.Text = string.Empty;
            txtDetailEmpNmEng.Text = string.Empty;
            txtDetailDeptId.EditValue = null;
            txtDetailDeptNm.Text = string.Empty;
            dteDetailEntDate.YyyyMmDd = null;
            dteDetailGrpEntDate.YyyyMmDd = null;
            txtDetailJobGrade.Text = string.Empty;
            txtDetailJobType.Text = string.Empty;
            chkDetailRetYn.Checked = false;
            dteDetailRetDate.YyyyMmDd = null;
            cboDetailSexCd.EditValue = string.Empty;
            txtDetailTel.Text = string.Empty;
            txtDetailHpTel.Text = string.Empty;
            txtDetailEmail.Text = string.Empty;
            txtDetailNatCd.Text = string.Empty;
            txtDetailZipCode.Text = string.Empty;
            txtDetailAddr1.Text = string.Empty;
            txtDetailAddr2.Text = string.Empty;
            chkDetailHoliYn.Checked = false;
            chkDetailDiligYn.Checked = false;
            chkDetailPayYn.Checked = false;
            picEmpPhoto.ImageBytes = null;
        });
    }

    /// <summary>서버가 돌려준 Base64 문자열(photo 컬럼, image/varbinary -> JSON 직렬화 시 자동으로
    /// Base64가 됨)을 picEmpPhoto(PictureEditWyn.ImageBytes)에 그대로 되돌린다. 값이 없으면(사진
    /// 미등록) 그냥 비운다 - 저장된 값이 어떤 이유로든 유효한 이미지가 아니어도(예전에 텍스트
    /// 필드였을 때의 잔여 데이터 등) 화면 전체가 죽으면 안 되므로 조용히 빈 상태로 둔다.</summary>
    private void SetPhotoFromBase64(string? base64)
    {
        if (string.IsNullOrEmpty(base64))
        {
            picEmpPhoto.ImageBytes = null;
            return;
        }

        try
        {
            picEmpPhoto.ImageBytes = Convert.FromBase64String(base64);
        }
        catch
        {
            picEmpPhoto.ImageBytes = null;
        }
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        // ---- 헤더(panData -> USP_BA_EMP_S) ----
        var headerParams = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_emp_id"] = _editingKey,
            ["p_acc_id"] = cboDetailAccCd.EditValue?.ToString(),
            ["p_emp_no"] = txtDetailEmpNo.Text,
            ["p_emp_nm"] = txtDetailEmpNm.Text,
            ["p_emp_nm_eng"] = txtDetailEmpNmEng.Text,
            ["p_dept_id"] = txtDetailDeptId.EditValue?.ToString(),
            ["p_ent_date"] = dteDetailEntDate.YyyyMmDd,
            ["p_grp_ent_date"] = dteDetailGrpEntDate.YyyyMmDd,
            ["p_job_grade"] = txtDetailJobGrade.Text,
            ["p_job_type"] = txtDetailJobType.Text,
            ["p_ret_yn"] = chkDetailRetYn.Checked ? "Y" : "N",
            ["p_ret_date"] = dteDetailRetDate.YyyyMmDd,
            ["p_sex_cd"] = cboDetailSexCd.EditValue?.ToString(),
            ["p_tel"] = txtDetailTel.Text,
            ["p_hp_tel"] = txtDetailHpTel.Text,
            ["p_email"] = txtDetailEmail.Text,
            ["p_nat_cd"] = txtDetailNatCd.Text,
            ["p_zip_code"] = txtDetailZipCode.Text,
            ["p_addr1"] = txtDetailAddr1.Text,
            ["p_addr2"] = txtDetailAddr2.Text,
            ["p_holi_yn"] = chkDetailHoliYn.Checked ? "Y" : "N",
            ["p_dilig_yn"] = chkDetailDiligYn.Checked ? "Y" : "N",
            ["p_pay_yn"] = chkDetailPayYn.Checked ? "Y" : "N",
            ["p_photo"] = picEmpPhoto.ImageBytes is { Length: > 0 } photoBytes ? Convert.ToBase64String(photoBytes) : null,
        };

        var headerResult = await SaveAsync("USP_BA_EMP_S", headerParams);
        if (headerResult == null || !headerResult.Success)
        {
            AppMessageBox.Show(headerResult?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        _editingKey ??= headerResult.GeneratedCode;
        Toast.Show("저장되었습니다.");
        await QueryCore(preserveSelection: true); // 방금 저장한 행 유지 - QueryClick(사용자 조회)과 다른 경로
    }

    public override async Task DeleteClick()
    {
        if (_editingKey == null) return;

        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "D",
            ["p_emp_id"] = _editingKey,
        };
        var result = await SaveAsync("USP_BA_EMP_S", p);
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "삭제에 실패했습니다.", "삭제 실패");
            return;
        }

        _editingKey = null;
        Toast.Show("삭제되었습니다.");
        await QueryClick();
    }
}
