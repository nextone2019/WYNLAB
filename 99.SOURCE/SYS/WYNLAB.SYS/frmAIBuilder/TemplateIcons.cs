using System.Drawing;
using DevExpress.LookAndFeel;
using DevExpress.Utils.Controls;
using DevExpress.Utils.Svg;

namespace WYNLAB.SYS;

/// <summary>AI Builder의 템플릿 선택기(cboTemplateKind)에 쓰는 미리보기 아이콘 - 추상적인
/// 심볼이 아니라 실제 템플릿 화면의 구역 배치(목록/상세폼/하위그리드)를 축소한 와이어프레임
/// 다이어그램이다(Assets/TemplateIcons/*.svg). WYNLAB.Shell/SvgIcons.cs의 커스텀 SVG 로딩과
/// 같은 방식이지만, WYNLAB.SYS는 Shell 프로젝트를 참조하지 않아 이 화면 전용으로 따로 둔다 -
/// 다른 화면에서도 필요해지면 그때 WYNLAB.Controls 같은 공용 프로젝트로 옮긴다.</summary>
internal static class TemplateIcons
{
    public static Image? Load(string fileName, int size)
    {
        try
        {
            var resourcePath = $"WYNLAB.SYS.Assets.TemplateIcons.{fileName}";
            using var resourceStream = typeof(TemplateIcons).Assembly.GetManifestResourceStream(resourcePath);
            if (resourceStream == null) return null;

            var svgImage = SvgImage.FromStream(resourceStream);
            return ImageHelper.CreateImageFromSvgImage(svgImage, new Size(size, size), UserLookAndFeel.Default);
        }
        catch
        {
            // 아이콘 하나 때문에 템플릿 선택 자체가 막히면 안 된다 - 못 만들면 그냥 아이콘 없이.
            return null;
        }
    }
}
