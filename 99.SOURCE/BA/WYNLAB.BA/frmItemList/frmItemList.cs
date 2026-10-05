// AI Builder가 싱글그리드 템플릿을 복제해서 자동 생성 - 2026-09-16.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmItemList : BaseForm
{
    private DataTable _list = new();

    public frmItemList()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준(2026-10-03): 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "품목정보조회";


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
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "Q",
            ["p_item_no"] = txtItemNo_Q.Text,
            //["p_item_nm"] = txtItemNm_Q.Text,
            //["p_item_spec"] = txtItemNm_Q.Text,
            ["p_stat_cd"] = cboStatCd_Q.EditValue.ToString(),
            ["p_asset_type"] = cboAssetType_Q.EditValue.ToString(),
        };
        _list = await QueryAsync("USP_BA_ITEMLIST_Q", p);
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
