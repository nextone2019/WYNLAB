using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Authorization;
using WYNLAB.Api.Controllers;
using WYNLAB.Api.Repositories.SM;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.SM;

/// <summary>
/// 사이트(설치) 환경설정 - frmSiteConfig(개발자 전용, Developer Tool 메뉴 안) 전용 API.
/// [RequireMenuPermission("SM","frmSiteConfig",...)]로 메뉴권한(TSMMENUAUTH)을 검증한다 -
/// 이 메뉴는 일반 사용자/관리자에게 권한을 안 주는 게 전제(개발자 계정에만 부여).
/// </summary>
[ApiController]
[Route("api/site-config")]
[Authorize]
public class SiteConfigController : ControllerBase
{
    private const long MaxImageSizeBytes = 5 * 1024 * 1024; // 5MB - 로그인배경/로고/파비콘 각각의 상한

    private readonly ISiteConfigRepository _repo;

    public SiteConfigController(ISiteConfigRepository repo) => _repo = repo;

    [HttpGet]
    [RequireMenuPermission("SM", "frmSiteConfig", MenuAction.View)]
    public async Task<ActionResult<SiteConfigDto>> Get()
    {
        var config = await _repo.GetAsync();
        if (config == null) return NotFound();
        return Ok(config);
    }

    [HttpPut]
    [RequireMenuPermission("SM", "frmSiteConfig", MenuAction.Update)]
    public async Task<ActionResult<ApiResult>> Save([FromBody] SiteConfigDto dto)
    {
        var result = await _repo.SaveAsync(dto, CurrentUserId, ClientPc);
        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage });

        return Ok(new ApiResult { Success = true });
    }

    /// <summary>
    /// 로그인 화면이 로그인 "전"에 브랜드컬러/회사명을 받아야 하므로 인증 불필요 - 민감정보
    /// 아닌 두 값만 돌려준다(SiteConfigDto 전체가 아니라 PublicBrandingDto).
    /// </summary>
    [HttpGet("public")]
    [AllowAnonymous]
    public async Task<ActionResult<PublicBrandingDto>> GetPublic()
    {
        var config = await _repo.GetAsync();
        return Ok(new PublicBrandingDto { CompanyNm = config?.CompanyNm, BrandColor = config?.BrandColor });
    }

    /// <summary>
    /// 로그인 "후" 화면 전반이 반영해야 하는 값(UiTheme 색상, 첨부파일 정책, 비밀번호 정책
    /// 안내용) - frmSiteConfig 메뉴권한(개발자 전용)과 무관하게 로그인된 사용자라면 누구나
    /// 읽을 수 있다. SMTP/비밀번호 해시 등 민감값은 RuntimeSiteConfigDto에 아예 없음.
    /// </summary>
    [HttpGet("runtime")]
    public async Task<ActionResult<RuntimeSiteConfigDto>> GetRuntime()
    {
        var config = await _repo.GetAsync();
        if (config == null) return Ok(new RuntimeSiteConfigDto());

        return Ok(new RuntimeSiteConfigDto
        {
            FileBlockExtensions = config.FileBlockExtensions,
            FileMaxSizeMb = config.FileMaxSizeMb,
            PwdMinLength = config.PwdMinLength,
            PwdRequireUpperLower = config.PwdRequireUpperLower,
            PwdRequireDigit = config.PwdRequireDigit,
            PwdRequireSpecial = config.PwdRequireSpecial,
            IdleTimeoutMinutes = config.IdleTimeoutMinutes,
            RequiredFieldBackColor = config.RequiredFieldBackColor,
            GridHeaderBackColor = config.GridHeaderBackColor,
            GridFocusedRowBackColor = config.GridFocusedRowBackColor,
            BrandColor = config.BrandColor,
            TreeGroupBackColor = config.TreeGroupBackColor,
            DividerColor = config.DividerColor,
        });
    }

    [HttpGet("login-background")]
    [AllowAnonymous]
    public async Task<IActionResult> GetLoginBackground() => await GetImage("login_bg_image", "login_bg_mime");

    [HttpGet("logo")]
    [AllowAnonymous]
    public async Task<IActionResult> GetLogo() => await GetImage("logo_image", "logo_mime");

    [HttpGet("favicon")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFavicon() => await GetImage("favicon_image", "favicon_mime");

    /// <summary>
    /// 로그인 화면이 로그인 "전"에 배경/로고/파비콘 이미지를 받아야 하므로 [AllowAnonymous]다
    /// (2026-09-06 수정 - 원래 [Authorize]만 걸려있었는데, 그러면 토큰이 없는 로그인 화면
    /// 자체가 절대 호출할 수 없어 브랜딩 기능이 성립하지 않는 버그였다). 브랜딩 이미지라
    /// 민감정보가 아니라 익명 공개에 문제 없음 - 편집(PUT)만 개발자 메뉴권한으로 막는다.
    /// </summary>
    private async Task<IActionResult> GetImage(string column, string mimeColumn)
    {
        var image = await _repo.GetImageAsync(column, mimeColumn);
        if (image == null) return NotFound();
        return File(image.Value.Bytes, string.IsNullOrEmpty(image.Value.Mime) ? "application/octet-stream" : image.Value.Mime);
    }

    [HttpPut("login-background")]
    [RequireMenuPermission("SM", "frmSiteConfig", MenuAction.Update)]
    public Task<ActionResult<ApiResult>> SaveLoginBackground(IFormFile file) => SaveImage("login_bg_image", "login_bg_mime", file);

    [HttpPut("logo")]
    [RequireMenuPermission("SM", "frmSiteConfig", MenuAction.Update)]
    public Task<ActionResult<ApiResult>> SaveLogo(IFormFile file) => SaveImage("logo_image", "logo_mime", file);

    [HttpPut("favicon")]
    [RequireMenuPermission("SM", "frmSiteConfig", MenuAction.Update)]
    public Task<ActionResult<ApiResult>> SaveFavicon(IFormFile file) => SaveImage("favicon_image", "favicon_mime", file);

    private async Task<ActionResult<ApiResult>> SaveImage(string column, string mimeColumn, IFormFile file)
    {
        if (file.Length == 0) return Ok(new ApiResult { Success = false, Message = "파일이 비어있습니다." });
        if (file.Length > MaxImageSizeBytes)
            return Ok(new ApiResult { Success = false, Message = $"이미지 크기가 너무 큽니다(최대 {MaxImageSizeBytes / 1024 / 1024}MB)." });

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        await _repo.SaveImageAsync(column, mimeColumn, stream.ToArray(), file.ContentType);

        return Ok(new ApiResult { Success = true });
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    private string? ClientPc => ClientPcInfo.Build(HttpContext);
}
