using DevExpress.XtraEditors;
using WYNLAB.Shared.Dtos;
using WYNLAB.Base;
using System.Drawing;

namespace WYNLAB.SM.USER;

/// <summary>
/// 사용자관리 화면. TSMMENU.FORM_CLASS_NM 에 이 클래스의 어셈블리 정규화 이름이 등록되어
/// ShellForm에서 리플렉션으로 동적 오픈된다.
///
/// 화면 자체에는 버튼이 없다 - Shell 상단 공통 툴바(조회/입력/삭제/출력)가 이 화면이 활성화된 상태에서
/// QueryAsync/NewAsync/DeleteAsync/PrintAsync(BaseGridForm/BaseForm 상속)를 호출하는 구조.
/// 검색조건(아이디/이름)만 화면 상단에 직접 두고, 조회 실행 자체는 Shell 툴바 "조회" 버튼과
/// 이 패널의 "검색" 버튼/Enter 둘 다에서 QueryAsync()로 진입하도록 통일했다.
/// </summary>
public class UserListForm : BaseGridForm
{
    private List<UserListItemDto> _currentList = new();

    private readonly Panel searchPanel = new() { Dock = DockStyle.Top, Height = 46, BackColor = Color.FromArgb(250, 250, 251) };
    private readonly TextEdit txtSearchUserId = new();
    private readonly TextEdit txtSearchUserNm = new();
    private readonly SimpleButton btnSearch = new() { Text = "검색" };

    public UserListForm()
    {
        Text = "사용자관리";
        MenuCd = "SM_USER"; // TSMMENU 등록 코드와 일치해야 권한이 정상 반영됨

        BuildSearchPanel();

        MainGridView.OptionsBehavior.Editable = false; // 그리드 직접편집 금지, 팝업으로만 수정
        MainGridView.DoubleClick += async (s, e) => await OpenEditPopupAsync();

        // 화면이 열리자마자 목록을 바로 보여주는 게 사용성이 좋음 (Shell 툴바 "조회" 안 눌러도 되도록)
        Load += async (s, e) => await QueryAsync();

        // 공통 타이틀 바 - 검색패널보다 나중에 추가해야 맨 위를 차지한다
        Controls.Add(BuildScreenHeader());
    }

    /// <summary>검색조건(아이디/이름) 입력 영역. BaseGridForm 생성자에서 MainGrid(Dock=Fill)가
    /// 먼저 추가되므로, 여기서 Dock=Top 패널을 나중에 추가하면 자연스럽게 그리드 위쪽에 자리잡는다.</summary>
    private void BuildSearchPanel()
    {
        var bottomBorder = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(228, 229, 232) };
        searchPanel.Controls.Add(bottomBorder);

        AddSearchLabel("아이디", 16);
        txtSearchUserId.Font = AppFonts.Body;
        txtSearchUserId.Location = new Point(58, 11);
        txtSearchUserId.Size = new Size(140, 24);
        searchPanel.Controls.Add(txtSearchUserId);

        AddSearchLabel("이름", 216);
        txtSearchUserNm.Font = AppFonts.Body;
        txtSearchUserNm.Location = new Point(250, 11);
        txtSearchUserNm.Size = new Size(140, 24);
        searchPanel.Controls.Add(txtSearchUserNm);

        btnSearch.Font = AppFonts.Body;
        btnSearch.Location = new Point(408, 10);
        btnSearch.Size = new Size(72, 26);
        btnSearch.Click += async (s, e) => await QueryAsync();
        searchPanel.Controls.Add(btnSearch);

        void SearchOnEnter(object? s, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.Handled = true;
            _ = QueryAsync();
        }
        txtSearchUserId.KeyDown += SearchOnEnter;
        txtSearchUserNm.KeyDown += SearchOnEnter;

        Controls.Add(searchPanel);
    }

    private void AddSearchLabel(string text, int x)
    {
        var lbl = new LabelControl { Text = text, Location = new Point(x, 15), AutoSizeMode = LabelAutoSizeMode.None, Size = new Size(40, 18) };
        lbl.Appearance.ForeColor = Color.FromArgb(100, 100, 100);
        lbl.Appearance.Font = AppFonts.Caption;
        searchPanel.Controls.Add(lbl);
    }

    public override async Task QueryAsync()
    {
        var query = $"api/users?userId={Uri.EscapeDataString(txtSearchUserId.Text.Trim())}&userNm={Uri.EscapeDataString(txtSearchUserNm.Text.Trim())}";
        _currentList = await ApiClient.GetAsync<List<UserListItemDto>>(query) ?? new();
        MainGrid.DataSource = _currentList;
    }

    public override async Task NewAsync()
    {
        using var form = new UserEditForm(); // 신규모드
        if (form.ShowDialog() == DialogResult.OK)
        {
            await QueryAsync();
        }
    }

    public override async Task DeleteAsync()
    {
        var selected = MainGridView.GetFocusedRow() as UserListItemDto;
        if (selected == null)
        {
            AppMessageBox.Show("삭제할 사용자를 선택해주세요.", "안내");
            return;
        }

        var confirm = AppMessageBox.Show($"'{selected.UserNm}({selected.UserId})' 사용자를 사용중지 처리하시겠습니까?",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        await ApiClient.DeleteAsync($"api/users/{selected.UserId}");
        await QueryAsync();
    }

    /// <summary>더블클릭으로 수정 팝업 오픈 - Shell 툴바가 아닌 그리드 자체 동작</summary>
    private async Task OpenEditPopupAsync()
    {
        var selected = MainGridView.GetFocusedRow() as UserListItemDto;
        if (selected == null) return;

        using var form = new UserEditForm(selected);
        if (form.ShowDialog() == DialogResult.OK)
        {
            await QueryAsync();
        }
    }
}
