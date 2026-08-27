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
    public List<ShortcutDto> Shortcuts { get; set; } = new();
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

    /// <summary>TSMUSER.USER_TYPE 원본 값('A'=관리자/'U'=일반). IsAdminYn은 이미 이 값을 bool로
    /// 뭉갠 것이라 "관리자냐 아니냐"만 필요하면 그걸 쓰면 되지만, SQL 로그 뷰어처럼 정확히
    /// 'A'인지 확인해야 하는 화면(ShellForm)을 위해 원본 값도 같이 내려준다.</summary>
    public string? UserType { get; set; }
}
