using DevExpress.XtraEditors;
using WYNLAB.Shared.Dtos;
using WYNLAB.UI.Common;
using System.Drawing;

namespace WYNLAB.Modules.System;

/// <summary>
/// 사용자그룹관리 화면. TSMUSERGRP를 관리하고, 더블클릭으로 들어가는 수정 팝업(UserGroupEditForm)
/// 안에서 그룹 소속 사용자 배정(TSMUSERGRPMAP)까지 같이 처리한다.
/// UserListForm과 동일하게 검색조건(그룹명) 패널을 상단에 둔다.
/// </summary>
public class UserGroupListForm : BaseGridForm
{
    private List<UserGroupListItemDto> _currentList = new();

    private readonly Panel searchPanel = new() { Dock = DockStyle.Top, Height = 46, BackColor = Color.FromArgb(250, 250, 251) };
    private readonly TextEdit txtSearchGrpNm = new();
    private readonly SimpleButton btnSearch = new() { Text = "검색" };

    public UserGroupListForm()
    {
        Text = "사용자그룹관리";
        MenuCd = "SM_USERGRP"; // TSMMENU 등록 코드와 일치해야 권한이 정상 반영됨

        BuildSearchPanel();

        MainGridView.OptionsBehavior.Editable = false;
        MainGridView.DoubleClick += async (s, e) => await OpenEditPopupAsync();

        Load += async (s, e) => await QueryAsync();
    }

    private void BuildSearchPanel()
    {
        var bottomBorder = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(228, 229, 232) };
        searchPanel.Controls.Add(bottomBorder);

        var lbl = new LabelControl { Text = "그룹명", Location = new Point(16, 15), AutoSizeMode = LabelAutoSizeMode.None, Size = new Size(44, 18) };
        lbl.Appearance.ForeColor = Color.FromArgb(100, 100, 100);
        lbl.Appearance.Font = AppFonts.Caption;
        searchPanel.Controls.Add(lbl);

        txtSearchGrpNm.Font = AppFonts.Body;
        txtSearchGrpNm.Location = new Point(62, 11);
        txtSearchGrpNm.Size = new Size(160, 24);
        txtSearchGrpNm.KeyDown += (s, e) =>
        {
            if (e.KeyCode != Keys.Enter) return;
            e.Handled = true;
            _ = QueryAsync();
        };
        searchPanel.Controls.Add(txtSearchGrpNm);

        btnSearch.Font = AppFonts.Body;
        btnSearch.Location = new Point(232, 10);
        btnSearch.Size = new Size(72, 26);
        btnSearch.Click += async (s, e) => await QueryAsync();
        searchPanel.Controls.Add(btnSearch);

        Controls.Add(searchPanel);
    }

    public override async Task QueryAsync()
    {
        var query = $"api/user-groups?userGrpNm={Uri.EscapeDataString(txtSearchGrpNm.Text.Trim())}";
        _currentList = await ApiClient.GetAsync<List<UserGroupListItemDto>>(query) ?? new();
        MainGrid.DataSource = _currentList;
    }

    public override async Task NewAsync()
    {
        using var form = new UserGroupEditForm();
        if (form.ShowDialog() == DialogResult.OK)
        {
            await QueryAsync();
        }
    }

    public override async Task DeleteAsync()
    {
        var selected = MainGridView.GetFocusedRow() as UserGroupListItemDto;
        if (selected == null)
        {
            XtraMessageBox.Show("삭제할 그룹을 선택해주세요.", "안내");
            return;
        }

        var confirm = XtraMessageBox.Show(
            $"'{selected.UserGrpNm}({selected.UserGrpCd})' 그룹을 사용중지 처리하시겠습니까?\n소속된 사용자 {selected.MemberCount}명의 그룹 권한 합산에서 이 그룹이 빠집니다.",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        await ApiClient.DeleteAsync($"api/user-groups/{selected.UserGrpCd}");
        await QueryAsync();
    }

    private async Task OpenEditPopupAsync()
    {
        var selected = MainGridView.GetFocusedRow() as UserGroupListItemDto;
        if (selected == null) return;

        using var form = new UserGroupEditForm(selected);
        if (form.ShowDialog() == DialogResult.OK)
        {
            await QueryAsync();
        }
    }
}
