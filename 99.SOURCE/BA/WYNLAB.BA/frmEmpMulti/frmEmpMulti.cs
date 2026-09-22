// AI Builder가 싱글그리드 템플릿을 복제해서 자동 생성 - 2026-09-19.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmEmpMulti : BaseForm
{
    private DataTable _list = new();

    public frmEmpMulti()
    {
        InitializeComponent();

        Text = "사원정보일괄등록";

        Controls.Add(BuildScreenHeader());

        gvw1.Role = GridRoleWyn.Edit;
        gvw1.HighlightFocusedRow = true;
        gvw1.RowAdd += (s, e) => gvw1.AddNewRow();
        gvw1.RowDelete += (s, e) =>
        {
            try { if (gvw1.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };

        
        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            // TODO: 검색창(txtEmpNo/txtDeptId)이 아직 Designer에 없어 컴파일이 안 돼서 임시로
            // 막아뒀다(2026-09-21) - Designer에서 컨트롤 추가하면 원복.
            // ["p_emp_no"] = txtEmpNo.Text,
            // ["p_dept_id"] = txtDeptId.Text,
        };
        _list = await QueryAsync("USP_BA_EMP_Q", p);
        grd1.DataSource = _list;
    }

    public override async Task SaveClick()
    {
        // 저장프로시저가 지정 안 된 조회전용 화면이면 여기서 막는다 - 코드생성 시점에 이 메서드
        // 자체를 없애는 대신 실행시점 가드로 처리해서, 나중에 저장프로시저를 붙여도 이 메서드
        // 골격을 그대로 재사용할 수 있게 한다.
        if (string.IsNullOrEmpty("USP_BA_EMP_S"))
        {
            AppMessageBox.Show("이 화면은 조회전용으로 생성됐습니다 - 저장프로시저가 지정되지 않았습니다.", "저장 불가");
            return;
        }

        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        foreach (DataRow row in _list.Rows.Cast<DataRow>().ToList())
        {
            if (row.RowState == DataRowState.Unchanged) continue;

            var workType = row.RowState == DataRowState.Deleted ? "D" : row.RowState == DataRowState.Added ? "N" : "U";
            var version = row.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;

            var p = new Dictionary<string, string?>
            {
                ["p_work_type"] = workType,
                ["p_emp_id"] = ProcData.Str(row, "EMP_ID", version),
                ["p_acc_id"] = ProcData.Str(row, "acc_id", version),
                ["p_emp_no"] = ProcData.Str(row, "emp_no", version),
                ["p_emp_nm"] = ProcData.Str(row, "emp_nm", version),
                ["p_emp_nm_eng"] = ProcData.Str(row, "emp_nm_eng", version),
                ["p_dept_id"] = ProcData.Str(row, "dept_id", version),
                ["p_ent_date"] = ProcData.Str(row, "ent_date", version),
                ["p_grp_ent_date"] = ProcData.Str(row, "grp_ent_date", version),
                ["p_job_grade"] = ProcData.Str(row, "job_grade", version),
                ["p_job_type"] = ProcData.Str(row, "job_type", version),
                ["p_ret_yn"] = ProcData.Str(row, "ret_yn", version),
                ["p_ret_date"] = ProcData.Str(row, "ret_date", version),
                ["p_sex_cd"] = ProcData.Str(row, "sex_cd", version),
                ["p_tel"] = ProcData.Str(row, "tel", version),
                ["p_hp_tel"] = ProcData.Str(row, "hp_tel", version),
                ["p_email"] = ProcData.Str(row, "email", version),
                ["p_nat_cd"] = ProcData.Str(row, "nat_cd", version),
                ["p_zip_code"] = ProcData.Str(row, "zip_code", version),
                ["p_addr1"] = ProcData.Str(row, "addr1", version),
                ["p_addr2"] = ProcData.Str(row, "addr2", version),
                ["p_holi_yn"] = ProcData.Str(row, "holi_yn", version),
                ["p_dilig_yn"] = ProcData.Str(row, "dilig_yn", version),
                ["p_pay_yn"] = ProcData.Str(row, "pay_yn", version),
                ["p_photo"] = ProcData.Str(row, "photo", version),
            };

            var result = await SaveAsync("USP_BA_EMP_S", p);
            if (result == null || !result.Success)
            {
                AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
                return;
            }
        }

        Toast.Show("저장되었습니다.");
        await QueryClick();
    }

    private void btnDownloadTemplate_Click(object sender, EventArgs e)
    {

    }
}
