namespace WYNLAB.Shared.Dtos;

/// <summary>사용자 목록/상세 조회용 - 로그인 세션(UserInfoDto)과는 별개, 관리화면 전용</summary>
public class UserListItemDto
{
    public string UserId { get; set; } = string.Empty;
    public string UserNm { get; set; } = string.Empty;
    public string? EmpNo { get; set; }
    public string? DeptCd { get; set; }
    public string? DeptNm { get; set; }
    public string? PositionNm { get; set; }
    public string? Email { get; set; }
    public string? MobileNo { get; set; }
    public bool UseYn { get; set; }
    public bool IsAdminYn { get; set; }
    public DateTime? LastLoginDt { get; set; }
}

/// <summary>사용자 신규등록 요청</summary>
public class UserCreateRequest
{
    public string UserId { get; set; } = string.Empty;
    public string UserNm { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // 초기 비밀번호, 서버에서 BCrypt 해시 처리
    public string? EmpNo { get; set; }
    public string? DeptCd { get; set; }
    public string? PositionNm { get; set; }
    public string? Email { get; set; }
    public string? MobileNo { get; set; }
    public bool IsAdminYn { get; set; }
}

/// <summary>사용자 수정 요청 - 비밀번호/USER_ID는 여기서 변경하지 않음(비밀번호는 별도 초기화 기능으로 분리 예정)</summary>
public class UserUpdateRequest
{
    public string UserNm { get; set; } = string.Empty;
    public string? EmpNo { get; set; }
    public string? DeptCd { get; set; }
    public string? PositionNm { get; set; }
    public string? Email { get; set; }
    public string? MobileNo { get; set; }
    public bool UseYn { get; set; }
    public bool IsAdminYn { get; set; }
}

public class ApiResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
}
