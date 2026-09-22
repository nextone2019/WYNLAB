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

    /// <summary>TSMUSER.MUST_CHANGE_PWD_YN='Y' - 로그인 자체는 성공했지만(자격증명은 맞음),
    /// 새 비밀번호를 설정하기 전엔 앱을 쓸 수 없다. 이때 발급된 AccessToken은 change-password
    /// 호출만 허용하는 제한된 토큰이다(서버 MustChangePasswordFilter 참고) - LoginForm은 이
    /// 값이 true면 ShellForm을 열지 말고 강제 비밀번호 변경 다이얼로그부터 띄워야 한다.</summary>
    public bool RequirePasswordChange { get; set; }
}

public class UserInfoDto
{
    public string UserId { get; set; } = string.Empty;
    public string UserNm { get; set; } = string.Empty;
    public string? EmpNo { get; set; }
    public string? DeptNm { get; set; }

    /// <summary>TSMUSER.DEVELOPER_YN - "시스템관리자"(SYS 모듈/개발자 전용 도구 접근) 판단
    /// 조건(사장님 지시, 2026-08-31). USER_TYPE(일반 메뉴권한 우회용 관리자)과는 별개 축이다.
    /// 사용자등록 화면에 이 값을 고치는 UI가 없다 - DB에서 직접 UPDATE해야만 바뀐다.</summary>
    public bool DeveloperYn { get; set; }

    /// <summary>TSMUSER.USER_TYPE 원본 값('A'=관리자/'U'=일반) - SQL 로그 뷰어처럼 정확히
    /// 'A'인지 확인해야 하는 화면(ShellForm)을 위해 원본 값을 내려준다.</summary>
    public string? UserType { get; set; }

    /// <summary>TSMUSER.ACC_ID(TBAACC 참조) - 로그인한 사용자가 어느 사업장 소속인지(2026-09-08
    /// 추가). BA 모듈 저장프로시저들(USP_BA_DEPT_S 등)에 아직 남아있는 "로그인 세션에 사업장
    /// 생기면 채우도록 전환" TODO가 바로 이 값을 쓰라는 뜻이다.</summary>
    public long? AccId { get; set; }
    public string? AccNm { get; set; }
}
