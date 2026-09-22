namespace WYNLAB.Shared.Dtos;

/// <summary>
/// TSMSITECONFIG 한 줄(회사당 1건) - 이미지 3종(로그인배경/로고/파비콘)은 여기 안 담고
/// 별도 바이너리 엔드포인트(api/site-config/login-background 등)로 주고받는다 - JSON 응답에
/// 큰 base64 덩어리를 안 섞기 위함.
/// </summary>
public class SiteConfigDto
{
    // 브랜딩
    public string? CompanyNm { get; set; }

    // 메일 발신(비밀번호 제외 - 서버 환경변수로만 관리)
    public string? SmtpHost { get; set; }
    public int? SmtpPort { get; set; }
    public string? SmtpUsername { get; set; }
    public string? SmtpFromAddress { get; set; }
    public string? SmtpFromDisplayNm { get; set; }

    // 첨부파일 정책
    public string? FileBlockExtensions { get; set; }
    public int? FileMaxSizeMb { get; set; }

    // 비밀번호 정책
    public int? PwdExpireDays { get; set; }
    public int? PwdLockThreshold { get; set; }
    public int? PwdResetCodeValidMin { get; set; }
    public int? PwdMinLength { get; set; }
    public bool PwdRequireUpperLower { get; set; }
    public bool PwdRequireDigit { get; set; }
    public bool PwdRequireSpecial { get; set; }
    public string? InitPwdPolicy { get; set; } // "USER_ID" | "RANDOM"
    public bool ForceChangeOnFirstLogin { get; set; }

    // 자리비움 잠금화면 - NULL/0이면 비활성(클라이언트가 판단)
    public int? IdleTimeoutMinutes { get; set; }

    // 색상값(배포 시 개발자가 1회 지정)
    public string? RequiredFieldBackColor { get; set; }
    public string? GridHeaderBackColor { get; set; }
    public string? GridFocusedRowBackColor { get; set; }
    public string? BrandColor { get; set; }
    public string? TreeGroupBackColor { get; set; }
    public string? DividerColor { get; set; }
}

/// <summary>
/// 로그인 "전"에도 필요한 최소한의 브랜딩 정보(GET api/site-config/public, 인증 불필요) -
/// 회사명/브랜드컬러만 담는다. SiteConfigDto 전체는 frmSiteConfig 메뉴권한이 있는 개발자만
/// 볼 수 있어야 하므로, 로그인화면이 그 API를 그대로 못 부른다(권한 검증할 세션 자체가 없음).
/// </summary>
public class PublicBrandingDto
{
    public string? CompanyNm { get; set; }
    public string? BrandColor { get; set; }
}

/// <summary>
/// 로그인 "후" 일반 사용자 화면이 반영해야 하는 사이트설정 값(GET api/site-config/runtime,
/// 로그인만 되어 있으면 누구나 - frmSiteConfig 메뉴권한 불필요). UiTheme 색상 오버라이드와
/// 첨부파일 정책 클라이언트측 사전검증, 비밀번호 정책 안내에 쓰인다. SMTP/비밀번호(해시) 등
/// 민감값은 담지 않는다.
/// </summary>
public class RuntimeSiteConfigDto
{
    public string? FileBlockExtensions { get; set; }
    public int? FileMaxSizeMb { get; set; }

    public int? PwdMinLength { get; set; }
    public bool PwdRequireUpperLower { get; set; }
    public bool PwdRequireDigit { get; set; }
    public bool PwdRequireSpecial { get; set; }

    public int? IdleTimeoutMinutes { get; set; }

    public string? RequiredFieldBackColor { get; set; }
    public string? GridHeaderBackColor { get; set; }
    public string? GridFocusedRowBackColor { get; set; }
    public string? BrandColor { get; set; }
    public string? TreeGroupBackColor { get; set; }
    public string? DividerColor { get; set; }
}
