using System.Data;
using Dapper;
using WYNLAB.Api.Data;

namespace WYNLAB.Api.Repositories.SM;

/// <summary>비밀번호 찾기(이메일 인증코드) - TSMPWDRESETTOKEN을 USP_SM_PWDRESET_S로 위임한다.</summary>
public interface IPwdResetRepository
{
    /// <summary>인증코드 발급 - 이 사용자의 기존 미사용 코드는 프로시저 안에서 전부 무효화된다.</summary>
    Task<ProcResult> IssueCodeAsync(string userId, string code, DateTime expireAt);

    /// <summary>인증코드 확인 + 비밀번호 교체를 한 트랜잭션으로(프로시저 안에서 처리).</summary>
    Task<ProcResult> ConsumeCodeAsync(string userId, string code, string newPasswordHash);
}

public class PwdResetRepository : IPwdResetRepository
{
    private readonly IDapperContext _context;

    public PwdResetRepository(IDapperContext context) => _context = context;

    public async Task<ProcResult> IssueCodeAsync(string userId, string code, DateTime expireAt)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "N");
        p.Add("p_user_id", userId);
        p.Add("p_code", code);
        p.Add("p_expire_dt", expireAt);
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("USP_SM_PWDRESET_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }

    public async Task<ProcResult> ConsumeCodeAsync(string userId, string code, string newPasswordHash)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "C");
        p.Add("p_user_id", userId);
        p.Add("p_code", code);
        p.Add("p_new_password_hash", newPasswordHash);
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("USP_SM_PWDRESET_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }
}
