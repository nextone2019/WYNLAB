using System.Data;
using Dapper;
using WYNLAB.Api.Data;

namespace WYNLAB.Api.Repositories;

public interface IUserManageRepository
{
    Task<List<UserManageRow>> GetAllAsync(string? userId = null, string? userNm = null);
    Task<bool> ExistsAsync(string userId);
    Task<ProcResult> CreateAsync(string userId, string userNm, string passwordHash, string? empNo,
        string? deptCd, string? positionNm, string? email, string? mobileNo, bool isAdminYn);
    Task<ProcResult> UpdateAsync(string userId, string userNm, string? empNo, string? deptCd,
        string? positionNm, string? email, string? mobileNo, bool useYn, bool isAdminYn);
    Task<ProcResult> SetUseYnAsync(string userId, bool useYn);
    Task<List<UserGroupAssignRow>> GetUserGroupsAsync(string userId);
    Task<ProcResult> ReplaceUserGroupsAsync(string userId, List<string> userGrpCds);
}

/// <summary>DB 조회 전용 - 목록 화면 표시용 (비밀번호 해시는 절대 포함하지 않음)</summary>
public class UserManageRow
{
    public string UserId { get; set; } = string.Empty;
    public string UserNm { get; set; } = string.Empty;
    public string? EmpNo { get; set; }
    public string? DeptCd { get; set; }
    public string? DeptNm { get; set; }
    public string? PositionNm { get; set; }
    public string? Email { get; set; }
    public string? MobileNo { get; set; }
    public string UseYn { get; set; } = "Y";
    public string IsAdminYn { get; set; } = "N";
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
/// 데이터 처리는 전부 저장프로시저(USP_SM_USER_*)로 위임한다.
/// 명명규칙: USP_SM_USER_Q(조회) / _Q_1,_Q_2(보조조회) / _S(저장) / _S_1,_S_2(추가 저장동작)
/// </summary>
public class UserManageRepository : IUserManageRepository
{
    private readonly IDapperContext _context;

    public UserManageRepository(IDapperContext context) => _context = context;

    public async Task<List<UserManageRow>> GetAllAsync(string? userId = null, string? userNm = null)
    {
        using var conn = _context.CreateConnection();
        var result = await conn.QueryAsync<UserManageRow>("USP_SM_USER_Q",
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
        var count = await conn.ExecuteScalarAsync<int>("USP_SM_USER_Q_1",
            new { p_work_type = "Q", p_user_id = userId }, commandType: CommandType.StoredProcedure);
        return count > 0;
    }

    public async Task<ProcResult> CreateAsync(string userId, string userNm, string passwordHash, string? empNo,
        string? deptCd, string? positionNm, string? email, string? mobileNo, bool isAdminYn)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "N");
        p.Add("p_user_id", userId);
        p.Add("p_user_nm", userNm);
        p.Add("p_password_hash", passwordHash);
        p.Add("p_emp_no", empNo);
        p.Add("p_dept_cd", deptCd);
        p.Add("p_position_nm", positionNm);
        p.Add("p_email", email);
        p.Add("p_mobile_no", mobileNo);
        p.Add("p_is_admin_yn", isAdminYn ? "Y" : "N");
        p.AddStandardOutputs(withGeneratedCode: true, pascalCase: true);

        await conn.ExecuteAsync("USP_SM_USER_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(withGeneratedCode: true, pascalCase: true);
    }

    public async Task<ProcResult> UpdateAsync(string userId, string userNm, string? empNo, string? deptCd,
        string? positionNm, string? email, string? mobileNo, bool useYn, bool isAdminYn)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "U");
        p.Add("p_user_id", userId);
        p.Add("p_user_nm", userNm);
        p.Add("p_emp_no", empNo);
        p.Add("p_dept_cd", deptCd);
        p.Add("p_position_nm", positionNm);
        p.Add("p_email", email);
        p.Add("p_mobile_no", mobileNo);
        p.Add("p_use_yn", useYn ? "Y" : "N");
        p.Add("p_is_admin_yn", isAdminYn ? "Y" : "N");
        p.AddStandardOutputs(withGeneratedCode: true, pascalCase: true);

        await conn.ExecuteAsync("USP_SM_USER_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(withGeneratedCode: true, pascalCase: true);
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

        await conn.ExecuteAsync("USP_SM_USER_S_1", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }

    public async Task<List<UserGroupAssignRow>> GetUserGroupsAsync(string userId)
    {
        using var conn = _context.CreateConnection();
        var result = await conn.QueryAsync<UserGroupAssignRow>("USP_SM_USER_Q_2",
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

        await conn.ExecuteAsync("USP_SM_USER_S_2", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }
}
