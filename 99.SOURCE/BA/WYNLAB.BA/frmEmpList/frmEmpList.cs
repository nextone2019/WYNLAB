// AI Builder가 싱글그리드 템플릿을 복제해서 자동 생성 - 2026-09-03.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmEmpList : BaseForm
{
    private DataTable _list = new();

    public frmEmpList()
    {
        InitializeComponent();

        Text = "사원정보조회";

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
            ["p_emp_no"] = txtEmpNo.Text,
            ["p_dept_cd"] = txtDeptCd.Text,
        };
        _list = await QueryAsync("USP_BA_EMPLIST_Q", p);
        grd1.DataSource = _list;
    }

    public override async Task SaveClick()
    {
        // 저장프로시저가 지정 안 된 조회전용 화면이면 여기서 막는다 - 코드생성 시점에 이 메서드
        // 자체를 없애는 대신 실행시점 가드로 처리해서, 나중에 저장프로시저를 붙여도 이 메서드
        // 골격을 그대로 재사용할 수 있게 한다.
        if (string.IsNullOrEmpty(""))
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
            };

            var result = await SaveAsync("", p);
            if (result == null || !result.Success)
            {
                AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
                return;
            }
        }

        Toast.Show("저장되었습니다.");
        await QueryClick();
    }
}
