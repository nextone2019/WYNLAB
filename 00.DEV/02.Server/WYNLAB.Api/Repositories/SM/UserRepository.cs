using System.Data;
using Dapper;
using WYNLAB.Api.Data;

namespace WYNLAB.Api.Repositories.SM;

/// <summary>DB 조회 전용 내부 모델 - TSMUSER 원본 행 (해시 비밀번호 포함)</summary>
public class UserRow
{
    public string UserId { get; set; } = string.Empty;
    public string UserNm { get; set; } = string.Empty;
    public string? EmpNo { get; set; }
    /// <summary>TBAEMP.emp_nm - 세션의 사원 이름(UserNm은 로그인 사용자 이름이라 다를 수 있다).</summary>
    public string? EmpNm { get; set; }
    public string? DeptNm { get; set; }
    /// <summary>TSMUSER.EMP_ID/그 사원의 DEPT_ID(2026-09-22 추가) - EmpNo/DeptNm(표시용 문자열)과
    /// 달리 실제 FK가 필요한 화면(구매요청등록 신규진입 자동입력 등)을 위해 원본 ID도 같이
    /// 내려준다.</summary>
    public long? EmpId { get; set; }
    public long? DeptId { get; set; }
    /// <summary>TSMUSER.ACC_ID(TBAACC 참조, 2026-09-08 추가) - 로그인 세션에 사업장을 실어 보내기
    /// 위함(BA 모듈 저장프로시저들의 "로그인 세션에 사업장 생기면 채우도록 전환" TODO가 이 값을
    /// 쓰라는 뜻).</summary>
    public long? AccId { get; set; }
    public string? AccNm { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string UseYn { get; set; } = "Y";
    /// <summary>TSMUSER.DEVELOPER_YN - "시스템관리자" 판단 조건(사장님 지시, 2026-08-31).
    /// USER_TYPE(아래, 메뉴권한 우회용 일반 관리자)과는 별개 축이다.</summary>
    public string DeveloperYn { get; set; } = "N";
    /// <summary>A=관리자, U=일반사용자 - 관리자 우회(전체 메뉴/전체 권한 허용) 판단은
    /// 이 값 기준이다(MenuPermissionService/AuthService 참고).</summary>
    public string UserType { get; set; } = "U";
    public int PwdFailCnt { get; set; }
    /// <summary>TSMUSER.MUST_CHANGE_PWD_YN - 'Y'면 로그인은 성공해도 새 비밀번호를 강제로
    /// 설정해야 한다(AuthService.LoginAsync 참고). 만료로 인한 강제변경/관리자 초기화 둘 다
    /// 이 플래그 하나로 처리한다.</summary>
    public string MustChangePwdYn { get; set; } = "N";
}

/// <summary>SSP_WYNLAB_GetSession 프로시저의 결과셋 2개를 담는 조합 모델</summary>
public class UserSessionResult
{
    public UserRow? User { get; set; }
    public List<string> GroupCodes { get; set; } = new();
}

public interface IUserRepository
{
    /// <summary>
    /// SSP_WYNLAB_GetSession 프로시저 호출 - 사용자 기본정보 + 소속그룹 목록을 한 번에 조회.
    /// 로그인 처리(AuthService)에서 이 메서드 하나로 세션 구성에 필요한 데이터를 전부 가져온다.
    /// </summary>
    Task<UserSessionResult> GetSessionAsync(string userId);
    Task<ProcResult> UpdateLoginSuccessAsync(string userId);
    Task<ProcResult> IncreasePwdFailCountAsync(string userId);
    Task<ProcResult> InsertLoginHistAsync(string userId, string clientIp, string clientVersion, string resultCd);
}

public class UserRepository : IUserRepository
{
    private readonly IDapperContext _context;

    public UserRepository(IDapperContext context) => _context = context;

    public async Task<UserSessionResult> GetSessionAsync(string userId)
    {
        using var conn = _context.CreateConnection();
        using var multi = await conn.QueryMultipleAsync(
            "SSP_WYNLAB_GetSession",
            new { p_work_type = "Q", p_user_id = userId },
            commandType: CommandType.StoredProcedure);

        var user = await multi.ReadSingleOrDefaultAsync<UserRow>();
        var groupCodes = (await multi.ReadAsync<string>()).ToList();

        return new UserSessionResult { User = user, GroupCodes = groupCodes };
    }

    public async Task<ProcResult> UpdateLoginSuccessAsync(string userId)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "U");
        p.Add("p_user_id", userId);
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("SSP_SYS_LOGIN_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }

    public async Task<ProcResult> IncreasePwdFailCountAsync(string userId)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "U");
        p.Add("p_user_id", userId);
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("SSP_SYS_LOGIN_S_1", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }

    public async Task<ProcResult> InsertLoginHistAsync(string userId, string clientIp, string clientVersion, string resultCd)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "N");
        p.Add("p_user_id", userId);
        p.Add("p_client_ip", clientIp);
        p.Add("p_client_version", clientVersion);
        p.Add("p_result_cd", resultCd);
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("SSP_SYS_LOGIN_S_2", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }
}
