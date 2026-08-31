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
    public static string DeptCd => SessionManager.Current.UserInfo?.DeptCd ?? string.Empty;
    public static string DeptNm => SessionManager.Current.UserInfo?.DeptNm ?? string.Empty;
    public static bool IsAdmin => SessionManager.Current.UserInfo?.UserType == "A";

    /// <summary>TSMUSER.DEVELOPER_YN - "시스템관리자"(SYS 모듈/개발자 전용 도구 접근) 판단
    /// 조건(사장님 지시, 2026-08-31). IsAdmin(USER_TYPE 기준, 메뉴권한 우회용 일반 관리자)과는
    /// 별개 축 - 사용자등록 화면에 이 값을 고치는 UI가 없어서 DB에서 직접 UPDATE해야만 바뀐다.</summary>
    public static bool IsDeveloper => SessionManager.Current.UserInfo?.DeveloperYn ?? false;
    public static string? UserType => SessionManager.Current.UserInfo?.UserType;
    public static bool IsSignedIn => SessionManager.Current.IsSignedIn;
}
