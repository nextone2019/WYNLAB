using System.Data;
using Dapper;
using WYNLAB.Api.Data;

namespace WYNLAB.Api.Repositories.SM;

public interface IUserManageRepository
{
    Task<List<UserManageRow>> GetAllAsync(string? userId = null, string? userNm = null);
    Task<bool> ExistsAsync(string userId);
    Task<ProcResult> CreateAsync(string userId, string userNm, string passwordHash, long? empId, long? accId, string? userType, string? email, bool mustChangePwd = false);
    Task<ProcResult> UpdateAsync(string userId, string userNm, long? empId, long? accId, string? userType, string? email, bool useYn);
    Task<ProcResult> SetUseYnAsync(string userId, bool useYn);
    Task<List<UserGroupAssignRow>> GetUserGroupsAsync(string userId);
    Task<ProcResult> ReplaceUserGroupsAsync(string userId, List<string> userGrpCds);
    /// <summary>비밀번호 교체(강제변경 다이얼로그 등) - 현재 비밀번호 확인은 AuthService가
    /// BCrypt로 이미 끝내고 새 해시만 넘어온다. MUST_CHANGE_PWD_YN도 여기서 같이 'N'으로 풀린다.</summary>
    Task<ProcResult> ChangePasswordAsync(string userId, string newPasswordHash);
}

/// <summary>DB 조회 전용 - 목록 화면 표시용 (비밀번호 해시는 절대 포함하지 않음)</summary>
public class UserManageRow
{
    public string UserId { get; set; } = string.Empty;
    public string UserNm { get; set; } = string.Empty;
    public long? EmpId { get; set; }
    public string? EmpNo { get; set; }
    public string? EmpNm { get; set; }
    public string? DeptNm { get; set; }
    public long? AccId { get; set; }
    public string? AccNm { get; set; }
    public string? Email { get; set; }
    public string UseYn { get; set; } = "Y";
    /// <summary>TSMUSER.DEVELOPER_YN - 조회 전용(표시용). CreateAsync/UpdateAsync엔 이 값을
    /// 받는 파라미터가 아예 없다 - 이 화면으로는 절대 못 바꾼다(사장님 지시, 2026-08-31).</summary>
    public string DeveloperYn { get; set; } = "N";
    public string UserType { get; set; } = "U";
    public DateTime? LastLoginDt { get; set; }
}

/// <summary>사용자 기준 그룹소속 배정 화면용 - 전체 그룹 + 이 사용자의 소속여부</summary>
public class UserGroupAssignRow
{
    public string UserGrpCd { get; set; } = string.Empty;
    public string UserGrpNm { get; set; } = string.Empty;
    public bool IsMember { get; set; }
}

/// <summary>
/// 데이터 처리는 전부 저장프로시저(USP_SM_USERAUTH_*)로 위임한다. 이름을 화면(frmUserAuth)과
/// 맞추기 위해 원래 USP_SM_USER_*였던 걸 2026-08-27에 sp_rename으로 바꿨다(017 마이그레이션).
/// 명명규칙: USP_SM_USERAUTH_Q(조회) / _Q_1,_Q_2(보조조회) / _S(저장) / _S_1,_S_2(추가 저장동작)
/// </summary>
public class UserManageRepository : IUserManageRepository
{
    private readonly IDapperContext _context;

    public UserManageRepository(IDapperContext context) => _context = context;

    public async Task<List<UserManageRow>> GetAllAsync(string? userId = null, string? userNm = null)
    {
        using var conn = _context.CreateConnection();
        var result = await conn.QueryAsync<UserManageRow>("USP_SM_USERAUTH_Q",
            new
            {
                p_work_type = "Q",
                p_user_id = string.IsNullOrWhiteSpace(userId) ? null : userId,
                p_user_nm = string.IsNullOrWhiteSpace(userNm) ? null : userNm
            },
            commandType: CommandType.StoredProcedure);
        return result.ToList();
    }

    public async Task<bool> ExistsAsync(string userId)
    {
        using var conn = _context.CreateConnection();
        var count = await conn.ExecuteScalarAsync<int>("USP_SM_USERAUTH_Q_1",
            new { p_work_type = "Q", p_user_id = userId }, commandType: CommandType.StoredProcedure);
        return count > 0;
    }

    public async Task<ProcResult> CreateAsync(string userId, string userNm, string passwordHash, long? empId, long? accId, string? userType, string? email, bool mustChangePwd = false)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "N");
        p.Add("p_user_id", userId);
        p.Add("p_user_nm", userNm);
        p.Add("p_password_hash", passwordHash);
        p.Add("p_emp_id", empId);
        p.Add("p_acc_id", accId);
        p.Add("p_user_type", string.IsNullOrWhiteSpace(userType) ? "U" : userType);
        p.Add("p_email", email);
        p.Add("p_must_change_pwd_yn", mustChangePwd ? "Y" : "N");
        p.AddStandardOutputs(withGeneratedCode: true, pascalCase: true);

        await conn.ExecuteAsync("USP_SM_USERAUTH_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(withGeneratedCode: true, pascalCase: true);
    }

    public async Task<ProcResult> UpdateAsync(string userId, string userNm, long? empId, long? accId, string? userType, string? email, bool useYn)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "U");
        p.Add("p_user_id", userId);
        p.Add("p_user_nm", userNm);
        p.Add("p_emp_id", empId);
        p.Add("p_acc_id", accId);
        p.Add("p_user_type", string.IsNullOrWhiteSpace(userType) ? "U" : userType);
        p.Add("p_email", email);
        p.Add("p_use_yn", useYn ? "Y" : "N");
        p.AddStandardOutputs(withGeneratedCode: true, pascalCase: true);

        await conn.ExecuteAsync("USP_SM_USERAUTH_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(withGeneratedCode: true, pascalCase: true);
    }

    public async Task<ProcResult> ChangePasswordAsync(string userId, string newPasswordHash)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "U");
        p.Add("p_user_id", userId);
        p.Add("p_password_hash", newPasswordHash);
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("USP_SM_USERAUTH_S_3", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }

    /// <summary>물리삭제 대신 USE_YN='N' 처리 - 이력/참조무결성 보존을 위한 표준 삭제 방식(work_type='D')</summary>
    public async Task<ProcResult> SetUseYnAsync(string userId, bool useYn)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "D");
        p.Add("p_user_id", userId);
        p.Add("p_use_yn", useYn ? "Y" : "N");
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("USP_SM_USERAUTH_S_1", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }

    public async Task<List<UserGroupAssignRow>> GetUserGroupsAsync(string userId)
    {
        using var conn = _context.CreateConnection();
        var result = await conn.QueryAsync<UserGroupAssignRow>("USP_SM_USERAUTH_Q_2",
            new { p_work_type = "Q", p_user_id = userId }, commandType: CommandType.StoredProcedure);
        return result.ToList();
    }

    /// <summary>체크된 그룹코드 목록으로 이 사용자의 소속을 치환(콤마구분 문자열로 프로시저 전달, work_type='U')</summary>
    public async Task<ProcResult> ReplaceUserGroupsAsync(string userId, List<string> userGrpCds)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "U");
        p.Add("p_user_id", userId);
        p.Add("p_user_grp_cds", userGrpCds.Count > 0 ? string.Join(",", userGrpCds) : null);
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("USP_SM_USERAUTH_S_2", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }
}
