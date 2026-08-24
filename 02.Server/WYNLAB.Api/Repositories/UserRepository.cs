using System.Data;
using Dapper;
using WYNLAB.Api.Data;

namespace WYNLAB.Api.Repositories;

/// <summary>DB 조회 전용 내부 모델 - TSMUSER 원본 행 (해시 비밀번호 포함)</summary>
public class UserRow
{
    public string UserId { get; set; } = string.Empty;
    public string UserNm { get; set; } = string.Empty;
    public string? EmpNo { get; set; }
    public string? DeptCd { get; set; }
    public string? DeptNm { get; set; }
    public string? PositionNm { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string UseYn { get; set; } = "Y";
    public string IsAdminYn { get; set; } = "N";
    public int PwdFailCnt { get; set; }
}

/// <summary>USP_SM_GetUserSession 프로시저의 결과셋 2개를 담는 조합 모델</summary>
public class UserSessionResult
{
    public UserRow? User { get; set; }
    public List<string> GroupCodes { get; set; } = new();
}

public interface IUserRepository
{
    /// <summary>
    /// USP_SM_GetUserSession 프로시저 호출 - 사용자 기본정보 + 소속그룹 목록을 한 번에 조회.
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
            "USP_SM_GetUserSession",
            new { p_work_type = "Q", p_user_id = userId },
            commandType: System.Data.CommandType.StoredProcedure);

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
