namespace WYNLAB.Shared.Dtos;

public class LoginRequest
{
    public string UserId { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ClientVersion { get; set; } = string.Empty; // ClickOnce 배포 버전
}

public class LoginResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }          // 실패 사유 (아이디없음/비번틀림/잠김 등)
    public string? AccessToken { get; set; }       // JWT
    public string? RefreshToken { get; set; }
    public UserInfoDto? UserInfo { get; set; }
    public List<MenuDto> Menus { get; set; } = new();
}

public class UserInfoDto
{
    public string UserId { get; set; } = string.Empty;
    public string UserNm { get; set; } = string.Empty;
    public string? EmpNo { get; set; }
    public string? DeptCd { get; set; }
    public string? DeptNm { get; set; }
    public string? PositionNm { get; set; }
    public bool IsAdminYn { get; set; }
}
