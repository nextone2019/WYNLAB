namespace WYNLAB.Base;

/// <summary>
/// 로그인 세션값을 화면(BaseForm 상속) 밖에서도(서비스/헬퍼 클래스 등) 어디서든
/// Session.UserId 처럼 짧게 꺼내 쓰기 위한 정적 접근자.
/// 실제 값의 출처는 SessionManager.Current.UserInfo(로그인시 서버의
/// SSP_WYNLAB_GetSession 프로시저 조회 결과) 그대로이며, 로그인 전에는 빈 값/false를 돌려준다.
/// </summary>
public static class Session
{
    public static string UserId => SessionManager.Current.UserInfo?.UserId ?? string.Empty;
    public static string UserNm => SessionManager.Current.UserInfo?.UserNm ?? string.Empty;
    public static string EmpNo => SessionManager.Current.UserInfo?.EmpNo ?? string.Empty;
    /// <summary>로그인 사용자에 연결된 사원(EmpId)의 이름 - 신규 진입 때 담당자를 채울 땐 UserNm이 아니라 이걸 쓴다(UserNm은 사용자 이름).</summary>
    public static string EmpNm => SessionManager.Current.UserInfo?.EmpNm ?? string.Empty;
    public static string DeptNm => SessionManager.Current.UserInfo?.DeptNm ?? string.Empty;

    /// <summary>TSMUSER.EMP_ID/그 사원의 DEPT_ID(2026-09-22 추가) - EmpNo/DeptNm(표시용 문자열)과
    /// 달리 실제 FK 저장이 필요한 화면(구매요청등록 등)에서 신규 진입 시 그대로 쓴다.</summary>
    public static long? EmpId => SessionManager.Current.UserInfo?.EmpId;
    public static long? DeptId => SessionManager.Current.UserInfo?.DeptId;
    public static bool IsAdmin => SessionManager.Current.UserInfo?.UserType == "A";

    /// <summary>TSMUSER.DEVELOPER_YN - "시스템관리자"(SYS 모듈/개발자 전용 도구 접근) 판단
    /// 조건(사장님 지시, 2026-08-31). IsAdmin(USER_TYPE 기준, 메뉴권한 우회용 일반 관리자)과는
    /// 별개 축 - 사용자등록 화면에 이 값을 고치는 UI가 없어서 DB에서 직접 UPDATE해야만 바뀐다.</summary>
    public static bool IsDeveloper => SessionManager.Current.UserInfo?.DeveloperYn ?? false;
    public static string? UserType => SessionManager.Current.UserInfo?.UserType;

    /// <summary>TSMUSER.ACC_ID(TBAACC 참조, 2026-09-08 추가) - 로그인한 사용자의 사업장. BA
    /// 모듈 저장프로시저들의 "로그인 세션에 사업장 생기면 채우도록 전환" TODO에 그대로 넘기면 된다
    /// (예: p_acc_id = Session.AccId).</summary>
    public static long? AccId => SessionManager.Current.UserInfo?.AccId;
    public static string AccNm => SessionManager.Current.UserInfo?.AccNm ?? string.Empty;
    public static bool IsSignedIn => SessionManager.Current.IsSignedIn;
}
