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
    public string? Email { get; set; }
    public bool UseYn { get; set; }

    /// <summary>grd1(사용자 목록) "사용" 컬럼 전용 - 그 컬럼의 ColumnEdit(LookUpColumnEdit,
    /// L_CM0100)는 셀 값을 그 LookUp의 값필드(문자열 "Y"/"N")와 비교해서 표시 텍스트를 찾는데,
    /// UseYn(bool)을 그대로 넣으면 절대 안 맞아서 항상 빈 칸으로 보인다(2026-09-03 실제 발견 -
    /// "grd1의 UseYn컬럼에 데이터가 왜 조회 되지 않아?"). 그래서 그 컬럼만 이 문자열을 대신
    /// 바라본다 - 읽기전용(리스트 선택 → panData에서 실제 수정), 값 자체는 UseYn을 그대로 따라간다.</summary>
    public string UseYnCd => UseYn ? "Y" : "N";

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

    /// <summary>비밀번호 찾기(잊어버렸을 때 인증코드를 보낼 주소) 용도 - 선택 입력.
    /// 비어있으면 그 계정은 이메일 셀프 초기화를 못 쓴다(관리자 경유만 가능).</summary>
    public string? Email { get; set; }
}

/// <summary>사용자 수정 요청 - 비밀번호/USER_ID는 여기서 변경하지 않음(비밀번호는 별도 초기화 기능으로 분리 예정)</summary>
public class UserUpdateRequest
{
    public string UserNm { get; set; } = string.Empty;
    public string? EmpNo { get; set; }
    public string? Email { get; set; }
    public bool UseYn { get; set; }
}

/// <summary>로그인된 상태에서 비밀번호 변경(강제변경 다이얼로그 포함) - 현재 비밀번호까지
/// 같이 받아서 서버가 재검증한다(이미 로그인했다는 사실만으로 새 비밀번호를 설정하게 두지
/// 않음 - 화면을 잠깐 비운 사이 다른 사람이 만지는 경우 등에 대비).</summary>
public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

/// <summary>비밀번호 찾기 - 1단계(인증코드 발송 요청). 계정 존재 여부를 알려주지 않기 위해
/// 응답은 항상 성공으로 보이는 동일한 메시지여야 한다(AuthController 참고).</summary>
public class PasswordResetRequestDto
{
    public string UserId { get; set; } = string.Empty;
}

/// <summary>비밀번호 찾기 - 2단계(인증코드 확인 + 새 비밀번호 설정). 성공하면 그대로 로그인까지
/// 처리되어 LoginResponse가 돌아온다(AuthController 참고).</summary>
public class PasswordResetConfirmDto
{
    public string UserId { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
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
