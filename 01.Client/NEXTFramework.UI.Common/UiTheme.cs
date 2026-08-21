using System.Drawing;

namespace NEXTFramework.UI.Common;

/// <summary>
/// AppConfig.Theme(appsettings.json의 hex 문자열)를 실제 Color 객체로 변환해서 제공.
/// RequiredFieldExtensions가 이 클래스의 색상을 사용해서 컨트롤을 꾸민다.
/// </summary>
public static class UiTheme
{
    public static Color RequiredFieldBackColor => ColorHelper.FromHex(AppConfig.Theme.RequiredFieldBackColor);
    public static Color RequiredFieldForeColor => ColorHelper.FromHex(AppConfig.Theme.RequiredFieldForeColor);
    public static Color RequiredHeaderForeColor => ColorHelper.FromHex(AppConfig.Theme.RequiredHeaderForeColor);
}
