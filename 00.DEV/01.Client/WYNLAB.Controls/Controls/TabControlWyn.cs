using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using DevExpress.XtraTab;

namespace WYNLAB.Base.Controls;

/// <summary>
/// 각진 기본 DevExpress 탭 대신, ShellForm의 MDI 문서탭과 같은 "위쪽 모서리만 둥근" 탭 모양을
/// 쓰는 XtraTabControl. 화면 안(그룹배정/권한부여처럼 화면 내부 탭)에서 같은 시각 언어를 쓰고
/// 싶다는 요청으로 ShellForm.TabbedMdiManager_CustomDrawTabHeader/TopRoundedRect의 그리기
/// 로직을 재사용 가능한 컨트롤로 뽑아냈다.
///
/// 활성/비활성/호버 상태는 ShellForm의 MDI탭과 같은 원칙으로 배경색 차이 하나로만 구분한다
/// (강조 바 없음 - ShellForm에서 강조 바가 과하다는 피드백을 받아 단순화했던 것과 같은 결론이라
/// 여기도 처음부터 배경색만 쓴다). 비활성/호버 색은 ShellForm의 MDI탭과 같은 고정 회색조 값이고,
/// 활성 배경은 UiTheme.TabActiveBackColor를 읽어써서 MDI탭과 똑같이 브랜드색(ToolbarColor)에서
/// 파생된 색을 쓴다 - WYNLAB.Controls는 AppConfig를 직접 모르지만(UiTheme.cs 클래스 설명 참고)
/// AppConfig.ApplyBrandDerivedTheme이 그 색을 미리 계산해서 UiTheme에 넣어주므로 결과적으로
/// MDI 탭과 정확히 같은 색이 된다.
/// </summary>
[ToolboxItem(true)]
public class TabControlWyn : XtraTabControl
{
    private static readonly Color InactiveBg = Color.FromArgb(232, 234, 238);
    private static readonly Color HotBg = Color.FromArgb(244, 245, 247);
    private static readonly Color InactiveFg = Color.FromArgb(120, 122, 128);
    private static readonly Color ActiveFg = Color.FromArgb(35, 35, 38);

    public TabControlWyn()
    {
        AppearancePage.Header.BackColor = InactiveBg;
        AppearancePage.Header.ForeColor = InactiveFg;
        AppearancePage.Header.Options.UseBackColor = true;
        AppearancePage.Header.Options.UseForeColor = true;

        AppearancePage.HeaderHotTracked.BackColor = HotBg;
        AppearancePage.HeaderHotTracked.ForeColor = ActiveFg;
        AppearancePage.HeaderHotTracked.Options.UseBackColor = true;
        AppearancePage.HeaderHotTracked.Options.UseForeColor = true;

        // 활성 탭 배경은 고정값이 아니라 UiTheme.TabActiveBackColor를 읽는다 - ShellForm의 MDI
        // 문서탭과 같은 색(브랜드색 파생)을 쓰기 위함(UiTheme.TabActiveBackColor 설명 참고).
        AppearancePage.HeaderActive.BackColor = UiTheme.TabActiveBackColor;
        AppearancePage.HeaderActive.ForeColor = ActiveFg;
        AppearancePage.HeaderActive.Options.UseBackColor = true;
        AppearancePage.HeaderActive.Options.UseForeColor = true;

        CustomDrawTabHeader += TabControlWyn_CustomDrawTabHeader;
    }

    /// <summary>ShellForm.TabbedMdiManager_CustomDrawTabHeader와 그리기 로직이 동일하다 -
    /// 배경만 위쪽이 둥근 사각형으로 직접 채우고, 텍스트/아이콘은 DevExpress 기본 로직에
    /// 그대로 맡긴다.</summary>
    private void TabControlWyn_CustomDrawTabHeader(object? sender, TabHeaderCustomDrawEventArgs e)
    {
        var info = e.TabHeaderInfo;
        var rect = e.Bounds;
        if (rect.Width <= 0 || rect.Height <= 0) return;

        rect.Inflate(-1, 0);
        rect.Y += 2;
        rect.Height -= 2;

        var isActive = info.IsActiveState;
        var isHot = info.IsHotState;
        var back = isActive ? UiTheme.TabActiveBackColor : (isHot ? HotBg : InactiveBg);

        var g = e.Graphics;
        var oldMode = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var path = TopRoundedRect(rect, 3))
        {
            using (var brush = new SolidBrush(back))
            {
                g.FillPath(brush, path);
            }
            using var pen = new Pen(ColorHelper.Adjust(back, -24));
            g.DrawPath(pen, path);
        }

        g.SmoothingMode = oldMode;

        e.DefaultDrawImage();
        e.DefaultDrawText();
        e.DefaultDrawButtons();
        e.Handled = true;
    }

    /// <summary>위쪽 두 모서리만 둥근 사각형 - ShellForm.TopRoundedRect와 동일.</summary>
    private static GraphicsPath TopRoundedRect(Rectangle bounds, int radius)
    {
        var path = new GraphicsPath();
        var d = radius * 2;
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddLine(bounds.Right, bounds.Y + radius, bounds.Right, bounds.Bottom);
        path.AddLine(bounds.Right, bounds.Bottom, bounds.X, bounds.Bottom);
        path.AddLine(bounds.X, bounds.Bottom, bounds.X, bounds.Y + radius);
        path.CloseFigure();
        return path;
    }
}
