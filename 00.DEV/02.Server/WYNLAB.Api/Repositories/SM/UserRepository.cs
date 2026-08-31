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
    public string? DeptCd { get; set; }
    public string? DeptNm { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string UseYn { get; set; } = "Y";
    /// <summary>TSMUSER.DEVELOPER_YN - "시스템관리자" 판단 조건(사장님 지시, 2026-08-31).
    /// USER_TYPE(아래, 메뉴권한 우회용 일반 관리자)과는 별개 축이다.</summary>
    public string DeveloperYn { get; set; } = "N";
    /// <summary>A=관리자, U=일반사용자 - 관리자 우회(전체 메뉴/전체 권한 허용) 판단은
    /// 이 값 기준이다(MenuPermissionService/AuthService 참고).</summary>
    public string UserType { get; set; } = "U";
    public int PwdFailCnt { get; set; }
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

        await conn.ExecuteAsync("USP_SM_LOGIN_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }

    public async Task<ProcResult> IncreasePwdFailCountAsync(string userId)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "U");
        p.Add("p_user_id", userId);
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("USP_SM_LOGIN_S_1", p, commandType: CommandType.StoredProcedure);
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

        await conn.ExecuteAsync("USP_SM_LOGIN_S_2", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }
}
