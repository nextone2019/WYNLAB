using System.Drawing;

namespace WYNLAB.Base;

/// <summary>
/// 회사별 커스텀 툴바 색상(밝은색/어두운색 무관)에 대해 아이콘·텍스트 색을
/// 자동으로 흰색 또는 짙은 회색으로 선택해주는 헬퍼. 상대휘도 공식(YIQ 근사) 사용.
/// </summary>
public static class ColorHelper
{
    /// <summary>"#RRGGBB"/"RRGGBB"뿐 아니라 색 이름("LightCyan")도 받는다 - 사이트환경설정(frmSiteConfig)이 예전엔
    /// ColorTranslator.ToHtml로 저장해서 알려진 색은 이름으로 DB에 들어갔고, 그러면 16진수 파싱이 예외를 내서
    /// SiteThemeSync가 조용히 무시하는 바람에 저장한 색이 아무 화면에도 적용되지 않았다(2026-09-25).</summary>
    public static Color FromHex(string hex)
    {
        hex = hex.Trim();
        var digits = hex.TrimStart('#');
        if (digits.Length != 6 || !digits.All(Uri.IsHexDigit)) return ColorTranslator.FromHtml(hex);

        hex = digits;
        var r = Convert.ToInt32(hex.Substring(0, 2), 16);
        var g = Convert.ToInt32(hex.Substring(2, 2), 16);
        var b = Convert.ToInt32(hex.Substring(4, 2), 16);
        return Color.FromArgb(r, g, b);
    }

    /// <summary>배경색 밝기에 따라 대비되는 전경색(흰색/짙은회색)을 반환</summary>
    public static Color GetContrastColor(Color background)
    {
        var luminance = (0.299 * background.R + 0.587 * background.G + 0.114 * background.B) / 255;
        return luminance > 0.6 ? Color.FromArgb(45, 45, 45) : Color.White;
    }

    /// <summary>배경색보다 살짝 밝은/어두운 톤 - 세그먼트 그룹 배경 등에 사용</summary>
    public static Color Adjust(Color baseColor, int amount)
    {
        int Clamp(int v) => Math.Max(0, Math.Min(255, v));
        return Color.FromArgb(Clamp(baseColor.R + amount), Clamp(baseColor.G + amount), Clamp(baseColor.B + amount));
    }

    /// <summary>base를 target 쪽으로 ratio(0~1)만큼 섞은 색. 같은 색 계열을 유지하면서
    /// 더 밝게(target=White) 또는 더 어둡게(target=Black) 만들 때 사용 - Adjust(고정값 가감)와
    /// 달리 원래 색이 얼마나 밝은지와 무관하게 항상 일관된 비율로 섞인다.</summary>
    public static Color Mix(Color baseColor, Color target, float ratio)
    {
        ratio = Math.Max(0f, Math.Min(1f, ratio));
        int Lerp(int a, int b) => (int)Math.Round(a + (b - a) * ratio);
        return Color.FromArgb(Lerp(baseColor.R, target.R), Lerp(baseColor.G, target.G), Lerp(baseColor.B, target.B));
    }
}
