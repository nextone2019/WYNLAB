using DevExpress.XtraEditors;
using WYNLAB.Base;
using System.Drawing;

namespace WYNLAB.Shell;

/// <summary>
/// 로그인 직후 항상 MDI에 열려있는 홈 화면. 사용자가 탭을 닫을 수 없다
/// (OnFormClosing에서 앱 종료가 아닌 경우 취소).
///
/// 회사별로 원하는 콘텐츠(결재리스트, 공지사항, 업무 대시보드 등)를 자유롭게 구성할 수 있는
/// 자리다. 지금은 자리표시용 문구만 있고, 실제 콘텐츠는 나중에 채워 넣으면 된다.
/// BaseForm을 상속하므로 QueryClick 등을 override해서 홈화면에도 조회/새로고침 동작을
/// 붙일 수 있다.
/// </summary>
public class HomeForm : BaseForm
{
    public HomeForm()
    {
        Text = "Home";
        Name = "__HOME__"; // ShellForm이 이 이름으로 기존 홈 탭을 찾아서 재사용/보호함

        var label = new LabelControl
        {
            Text = "HOME 화면\n\n여기에 회사별 대시보드, 결재리스트, 공지사항 등을\n자유롭게 구성할 수 있습니다.",
            Dock = DockStyle.Fill
        };
        label.Appearance.Font = AppFonts.Heading;
        label.Appearance.ForeColor = Color.FromArgb(120, 120, 120);
        label.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        label.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

        Controls.Add(label);
    }

    /// <summary>
    /// 사용자가 탭의 X 버튼을 눌러도 닫히지 않도록 막는다.
    /// 전체 프로그램이 종료되거나(MDI 부모가 닫힐 때), 서버 전환으로 강제 정리될 때만 허용.
    /// </summary>
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (e.CloseReason != CloseReason.MdiFormClosing)
        {
            e.Cancel = true;
            return;
        }
        base.OnFormClosing(e);
    }
}
