using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Text;
using DevExpress.LookAndFeel;
using DevExpress.Utils.Controls;
using DevExpress.Utils.Svg;

namespace WYNLAB.Shell;

/// <summary>
/// DevExpress 내장 SVG 아이콘(2,600여 개)을 이름으로 꺼내서 원하는 크기·색의 이미지로 만들어준다.
///
/// 예전엔 아이콘을 코드로 직접 그렸는데(MenuIconPainters), 도형 몇 개로 흉내 내는 수준이라
/// 모듈이 늘어날수록 "적당한 그림"을 새로 그려야 했고 완성도도 들쭉날쭉했다. DevExpress가
/// 이미 잘 만들어둔 세트를 쓰면 이름만 바꿔 끼우면 되고 톤도 자동으로 통일된다.
///
/// [이름 규칙 - 실제로 확인한 것] 리소스 이름은 반드시 URL 인코딩된 형태여야 한다.
///   O  "svgimages/icon%20builder/actions_home.svg"
///   X  "svgimages/icon builder/actions_home.svg"   (공백 그대로면 못 찾고 null이 온다)
/// 폴더명에 공백이 있는 카테고리(icon builder, business objects 등)에서만 문제가 되는데,
/// 눈으로는 구분이 안 되고 조용히 null이 될 뿐이라 놓치기 쉽다.
///
/// [색] icon builder 계열은 검은 단색 도형이라 어두운 사이드바에 그대로 올리면 보이지 않는다.
/// 그래서 모양(알파)만 남기고 색을 원하는 값으로 덮어씌운다 - 구멍이나 여백은 투명이라
/// 그대로 유지되므로 실루엣이 뭉개지지 않는다.
/// </summary>
internal static class SvgIcons
{
    // 자주 쓰는 아이콘 이름을 한 곳에 모아둔다 - 호출부에 URL 인코딩된 긴 문자열이 흩어지면
    // 오타가 나도 조용히 아이콘만 안 보여서 원인을 찾기 어렵다.
    // Home은 2026-09-03에 사용자가 고른 Lucide 세트(embedded:home.svg)로 바뀌었다 - 아래
    // 상단 툴바 아이콘 블록 주석 참고.
    public const string Home = "embedded:home.svg";
    public const string Settings = "svgimages/icon%20builder/actions_settings.svg";
    public const string ShoppingCart = "svgimages/icon%20builder/shopping_shoppingcart.svg";
    public const string Database = "svgimages/icon%20builder/actions_database.svg";
    public const string Box = "svgimages/icon%20builder/shopping_box.svg";
    public const string User = "svgimages/icon%20builder/actions_user.svg";
    public const string Security = "svgimages/icon%20builder/security_security.svg";
    public const string Money = "svgimages/icon%20builder/business_money.svg";
    public const string Folder = "svgimages/business%20objects/bo_folder.svg";
    /// <summary>{ } 코드 브라켓 모양 - "Developer Tool" 최상위 메뉴용(icon builder 계열엔 코드/
    /// 개발도구를 나타낼 만한 아이콘이 없어서 xaf 계열에서 골랐다).</summary>
    public const string Code = "svgimages/xaf/action_showscript.svg";

    // 상단 툴바 아이콘 - 예전엔 DevExpress 번들 SVG 세트(icon%20builder/xaf 등)를 썼는데,
    // 2026-09-03에 사용자가 직접 고른 Lucide 아이콘(Assets/ToolbarIcons/*.svg, EmbeddedResource)
    // 세트로 전부 갈아끼웠다 - "embedded:" 접두사로 Load()가 DevExpress 리소스캐시 대신
    // LoadEmbedded로 분기하게 한다(아래 Load 참고). 탭목록/탭전체닫기/SQL로그 3개만 사용자가
    // 준 세트에 대응하는 파일이 없어서 DevExpress 번들 그대로 남겨뒀다.
    public const string ToolbarSearch = "embedded:query.svg";
    public const string ToolbarNew = "embedded:new.svg";
    public const string ToolbarDelete = "embedded:delete.svg";
    public const string ToolbarRowAdd = "embedded:addrow.svg";
    public const string ToolbarRowDelete = "embedded:deleterow.svg";
    public const string ToolbarSave = "embedded:save.svg";
    public const string ToolbarPrint = "embedded:print.svg";
    public const string ToolbarTabList = "svgimages/xaf/action_windowlist.svg";
    public const string ToolbarCloseAll = "svgimages/xaf/action_closeallwindows.svg";
    public const string ToolbarSqlLog = "svgimages/icon%20builder/actions_database.svg";
    public const string ToolbarLogout = "embedded:logout.svg";

    /// <summary>사이드바 메뉴 접기/펼치기 버튼(sidebarToggleButton)이 상태에 따라 바꿔 끼우는
    /// 두 아이콘 - "이 아이콘을 누르면 일어날 동작"을 보여주는 관례라(현재 상태를 그리는 게
    /// 아니라), 펼쳐진 상태에선 MenuHide(접어라)를, 접힌 상태에선 MenuView(펼쳐라)를 보여준다
    /// (ShellForm.RefreshSidebarToggleIcon 참고). 예전엔 상태와 무관하게 햄버거 아이콘
    /// 하나(NavigationToggle)만 썼었다.</summary>
    public const string MenuHide = "embedded:menu_hide.svg";
    public const string MenuView = "embedded:menu_view.svg";

    /// <summary>
    /// 아이콘을 size×size 이미지로 만들어 돌려준다. 이름을 못 찾으면(오타/버전 차이) null -
    /// 호출부는 아이콘 없이도 동작하게 두는 편이 낫다(아이콘 하나 때문에 메뉴가 안 뜨면 곤란).
    /// </summary>
    public static Image? Load(string resourceName, int size, Color color)
    {
        try
        {
            if (resourceName.StartsWith("embedded:", StringComparison.Ordinal))
                return LoadEmbedded(resourceName.Substring("embedded:".Length), size, color);

            // GetSvgImage는 SvgImage(벡터)가 아니라 이미 요청한 크기로 그려진 Image를 돌려준다 -
            // 별도의 래스터화 단계가 필요 없다.
            //
            // [중요] 이 Image를 Dispose하면 안 된다. ImageResourceCache가 이름+크기별로 캐싱해서
            // 같은 인스턴스를 계속 돌려주기 때문에, 한 번 폐기하면 그 다음부터 같은 아이콘을
            // 요청할 때 이미 죽은 객체가 와서 조용히 실패한다(실제로 겪음 - 같은 아이콘을 두
            // 번째로 쓰는 자리에서만 아이콘이 안 보였다). 소유권은 캐시에 있고 우리는 빌려 쓸 뿐이다.
            var rendered = DevExpress.Images.ImageResourceCache.Default.GetSvgImage(
                resourceName, null, new Size(size, size));
            if (rendered == null) return null;

            return Tint(rendered, color);
        }
        catch
        {
            // 아이콘을 못 만드는 상황(리소스 없음 등)에 앱이 멈출 이유는 없다.
            return null;
        }
    }

    /// <summary>Assets/ToolbarIcons에 EmbeddedResource로 구운 커스텀 SVG(사용자가 직접 고른
    /// Lucide 아이콘셋)를 불러온다. Lucide는 stroke="currentColor"로 그려서 색을 CSS가
    /// 채워주는 걸 전제하는데, DevExpress SvgImage는 그 키워드 자체를 색으로 해석 못 한다
    /// (실제로 검은색 등 예측 불가한 값으로 떨어질 수 있어 확인 안 하고 그대로 안 씀) - 그래서
    /// 파싱 전에 SVG 텍스트에서 그 자리를 원하는 색의 헥스값으로 직접 치환해둔다. 이렇게 하면
    /// DevExpress 번들 세트처럼 래스터화 후 ColorMatrix로 다시 물들이는(Tint) 단계가 필요 없다 -
    /// 이미 원하는 색으로 그려져 나온다.</summary>
    private static Image? LoadEmbedded(string fileName, int size, Color color)
    {
        var resourcePath = $"WYNLAB.Shell.Assets.ToolbarIcons.{fileName}";
        using var resourceStream = typeof(SvgIcons).Assembly.GetManifestResourceStream(resourcePath);
        if (resourceStream == null) return null;

        using var reader = new StreamReader(resourceStream);
        var svgText = reader.ReadToEnd().Replace("currentColor", ColorTranslator.ToHtml(color));

        using var svgStream = new MemoryStream(Encoding.UTF8.GetBytes(svgText));
        var svgImage = SvgImage.FromStream(svgStream);
        return ImageHelper.CreateImageFromSvgImage(svgImage, new Size(size, size), UserLookAndFeel.Default);
    }

    /// <summary>모양(알파)은 그대로 두고 색만 지정한 색으로 바꾼다.</summary>
    private static Image Tint(Image source, Color color)
    {
        var result = new Bitmap(source.Width, source.Height, PixelFormat.Format32bppArgb);

        using (var g = Graphics.FromImage(result))
        using (var attributes = new ImageAttributes())
        {
            // 원본 RGB를 0으로 죽이고 원하는 색을 더한다(알파는 원본 그대로 곱해서 유지).
            // ColorMatrix 한 번으로 처리하면 픽셀을 직접 훑는 것보다 훨씬 빠르고, 가장자리
            // 안티에일리어싱도 자연스럽게 남는다.
            var matrix = new ColorMatrix(new[]
            {
                new[] { 0f, 0f, 0f, 0f, 0f },
                new[] { 0f, 0f, 0f, 0f, 0f },
                new[] { 0f, 0f, 0f, 0f, 0f },
                new[] { 0f, 0f, 0f, 1f, 0f },
                new[] { color.R / 255f, color.G / 255f, color.B / 255f, 0f, 1f }
            });
            attributes.SetColorMatrix(matrix);

            g.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height),
                0, 0, source.Width, source.Height, GraphicsUnit.Pixel, attributes);
        }

        return result;
    }
}
