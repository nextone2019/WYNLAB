using System.Drawing;

namespace WYNLAB.UI.Common;

/// <summary>
/// 회사별 커스텀 툴바 색상(밝은색/어두운색 무관)에 대해 아이콘·텍스트 색을
/// 자동으로 흰색 또는 짙은 회색으로 선택해주는 헬퍼. 상대휘도 공식(YIQ 근사) 사용.
/// </summary>
public static class ColorHelper
{
    public static Color FromHex(string hex)
    {
        hex = hex.TrimStart('#');
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
}
