// @AI_BUILDER:BEGIN FILE_HEADER
// AI Builder 마스터-상세폼-서브그리드 템플릿 원본 - 이 파일 자체는 실행되지 않는다(어떤
// 메뉴에도 등록돼 있지 않음). VS에서 열어 로직 골격을 확인/조정하는 용도.
// @AI_BUILDER:END FILE_HEADER
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmItemGrp : BaseForm
{
    private DataTable _list = new();
    private DataTable _detail = new();
    private string? _editingKey; // null이면 신규모드

    public frmItemGrp()
    {
        InitializeComponent();

        Text = "품목그룹등록";

        Controls.Add(BuildScreenHeader());

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.FocusedRowObjectChanged += async (s, e) => await OnMasterSelectedAsync();

        gvw2.Role = GridRoleWyn.Query; // 하위그리드는 조회전용으로 생성됨 - 편집/저장이 필요하면 직접 추가
        gvw2.HighlightFocusedRow = true;

      

        EnterNewMode();
        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
        };
        _list = await QueryAsync("USP_BA_ITEMGRP_Q", p);
        grd1.DataSource = _list;
        EnterNewMode();
    }

    private async Task OnMasterSelectedAsync()
    {
        if (gvw1.GetFocusedRow() is not DataRowView view) { EnterNewMode(); return; }

        var row = view.Row;
        _editingKey = row[""]?.ToString();
        txtDetailAccCd.Text = row["acc_cd"]?.ToString() ?? string.Empty;
        txtDetailItemLvl.Text = row["item_lvl"]?.ToString() ?? string.Empty;
        txtDetailItemClassCd.Text = row["item_class_cd"]?.ToString() ?? string.Empty;
        txtDetailItemClassNm.Text = row["item_class_nm"]?.ToString() ?? string.Empty;
        txtDetailParItemClassCd.Text = row["par_item_class_cd"]?.ToString() ?? string.Empty;
        txtDetailRemark.Text = row["remark"]?.ToString() ?? string.Empty;
        await LoadDetailAsync();
    }

    private void EnterNewMode()
    {
        _editingKey = null;
        txtDetailAccCd.Text = string.Empty;
        txtDetailItemLvl.Text = string.Empty;
        txtDetailItemClassCd.Text = string.Empty;
        txtDetailItemClassNm.Text = string.Empty;
        txtDetailParItemClassCd.Text = string.Empty;
        txtDetailRemark.Text = string.Empty;
        _detail = _detail.Clone();
        grd2.DataSource = _detail;
    }

    /// <summary>선택된(또는 방금 저장한) 마스터 행의 하위 목록을 조회한다.</summary>
    private async Task LoadDetailAsync()
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q1",
            ["p_"] = _editingKey,
        };
        _detail = await QueryAsync("USP_BA_ITEMGRP_Q", p);
        grd2.DataSource = _detail;
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        if (string.IsNullOrEmpty("USP_BA_ITEMGRP_S"))
        {
            AppMessageBox.Show("이 화면은 조회전용으로 생성됐습니다 - 저장프로시저가 지정되지 않았습니다.", "저장 불가");
            return;
        }

        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_p_work_type"] = null, // TODO: 값 채우기
            ["p_p_acc_cd"] = null, // TODO: 값 채우기
            ["p_p_item_lvl"] = null, // TODO: 값 채우기
            ["p_p_item_class_cd"] = null, // TODO: 값 채우기
            ["p_p_item_class_nm"] = null, // TODO: 값 채우기
            ["p_p_par_item_class_cd"] = null, // TODO: 값 채우기
            ["p_p_remark"] = null, // TODO: 값 채우기
            ["p_p_user_id"] = null, // TODO: 값 채우기
            ["p_p_client_pc"] = null, // TODO: 값 채우기
        };

        var result = await SaveAsync("USP_BA_ITEMGRP_S", p);
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        Toast.Show("저장되었습니다.");
        await QueryClick();
    }

    public override async Task DeleteClick()
    {
        if (_editingKey == null) return;

        if (string.IsNullOrEmpty("USP_BA_ITEMGRP_S"))
        {
            AppMessageBox.Show("저장프로시저가 지정되지 않았습니다.", "삭제 불가");
            return;
        }

        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "D",
            ["p_"] = _editingKey,
        };
        var result = await SaveAsync("USP_BA_ITEMGRP_S", p);
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "삭제에 실패했습니다.", "삭제 실패");
            return;
        }

        Toast.Show("삭제되었습니다.");
        await QueryClick();
    }
}
