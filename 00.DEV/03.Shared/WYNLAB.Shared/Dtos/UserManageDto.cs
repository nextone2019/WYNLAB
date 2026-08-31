namespace WYNLAB.Shared.Dtos;

/// <summary>사용자 목록/상세 조회용 - 로그인 세션(UserInfoDto)과는 별개, 관리화면 전용</summary>
public class UserListItemDto
{
    public string UserId { get; set; } = string.Empty;
    public string UserNm { get; set; } = string.Empty;
    public string? EmpNo { get; set; }
    public string? EmpNm { get; set; }
    public string? DeptCd { get; set; }
    public string? DeptNm { get; set; }
    public bool UseYn { get; set; }

    /// <summary>TSMUSER.DEVELOPER_YN - 조회 전용(표시용). 이 화면(UserCreateRequest/
    /// UserUpdateRequest)에는 이 값을 고치는 필드가 없다 - DB에서 직접 UPDATE해야만 바뀐다
    /// (사장님 지시, 2026-08-31 - "시스템관리자" 판단 조건이라 UI 편집 경로를 아예 없앰).</summary>
    public bool DeveloperYn { get; set; }
    public DateTime? LastLoginDt { get; set; }
}

/// <summary>사용자 신규등록 요청</summary>
public class UserCreateRequest
{
    public string UserId { get; set; } = string.Empty;
    public string UserNm { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // 초기 비밀번호, 서버에서 BCrypt 해시 처리

    /// <summary>TSMUSER.EMP_NO - 부서(DeptCd/DeptNm)는 여기 없다. TSMUSER는 더 이상 DEPT_CD를
    /// 직접 저장하지 않고, EMP_NO로 TBAEMP를 조인해서 그때그때 얻어온다(사장님 지시,
    /// 2026-08-31) - EmpNo만 저장하면 부서는 자동으로 따라온다.</summary>
    public string? EmpNo { get; set; }
}

/// <summary>사용자 수정 요청 - 비밀번호/USER_ID는 여기서 변경하지 않음(비밀번호는 별도 초기화 기능으로 분리 예정)</summary>
public class UserUpdateRequest
{
    public string UserNm { get; set; } = string.Empty;
    public string? EmpNo { get; set; }
    public bool UseYn { get; set; }
}

public class ApiResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }

    /// <summary>신규등록(Create) 응답에서만 사용 - 채번/확정된 PK값(지금은 입력값 그대로 에코)</summary>
    public string? GeneratedCode { get; set; }

    /// <summary>실패 시에만 채워짐 - 프로시저 TRY/CATCH가 잡은 SQL 오류번호(ERROR_NUMBER(), 예:
    /// PK 중복이면 2627). 화면에서 특정 오류번호별로 다른 안내를 보여주고 싶을 때 참고용 - 0이면
    /// SQL 예외가 아니라 업무로직 판단(ReturnCode)만으로 실패한 것.</summary>
    public int ErrorCode { get; set; }
}
