using System.Data;
using Dapper;
using WYNLAB.Api.Data;

namespace WYNLAB.Api.Repositories;

public interface IUserManageRepository
{
    Task<List<UserManageRow>> GetAllAsync(string? userId = null, string? userNm = null);
    Task<bool> ExistsAsync(string userId);
    Task CreateAsync(string userId, string userNm, string passwordHash, string? empNo,
        string? deptCd, string? positionNm, string? email, string? mobileNo, bool isAdminYn);
    Task UpdateAsync(string userId, string userNm, string? empNo, string? deptCd,
        string? positionNm, string? email, string? mobileNo, bool useYn, bool isAdminYn);
    Task SetUseYnAsync(string userId, bool useYn);
    Task<List<UserGroupAssignRow>> GetUserGroupsAsync(string userId);
    Task ReplaceUserGroupsAsync(string userId, List<string> userGrpCds);
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
                UserId = string.IsNullOrWhiteSpace(userId) ? null : userId,
                UserNm = string.IsNullOrWhiteSpace(userNm) ? null : userNm
            },
            commandType: CommandType.StoredProcedure);
        return result.ToList();
    }

    public async Task<bool> ExistsAsync(string userId)
    {
        using var conn = _context.CreateConnection();
        var count = await conn.ExecuteScalarAsync<int>("USP_SM_USER_Q_1",
            new { UserId = userId }, commandType: CommandType.StoredProcedure);
        return count > 0;
    }

    public async Task CreateAsync(string userId, string userNm, string passwordHash, string? empNo,
        string? deptCd, string? positionNm, string? email, string? mobileNo, bool isAdminYn)
    {
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync("USP_SM_USER_S", new
        {
            Mode = "C",
            UserId = userId,
            UserNm = userNm,
            PasswordHash = passwordHash,
            EmpNo = empNo,
            DeptCd = deptCd,
            PositionNm = positionNm,
            Email = email,
            MobileNo = mobileNo,
            IsAdminYn = isAdminYn ? "Y" : "N"
        }, commandType: CommandType.StoredProcedure);
    }

    public async Task UpdateAsync(string userId, string userNm, string? empNo, string? deptCd,
        string? positionNm, string? email, string? mobileNo, bool useYn, bool isAdminYn)
    {
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync("USP_SM_USER_S", new
        {
            Mode = "U",
            UserId = userId,
            UserNm = userNm,
            EmpNo = empNo,
            DeptCd = deptCd,
            PositionNm = positionNm,
            Email = email,
            MobileNo = mobileNo,
            UseYn = useYn ? "Y" : "N",
            IsAdminYn = isAdminYn ? "Y" : "N"
        }, commandType: CommandType.StoredProcedure);
    }

    /// <summary>물리삭제 대신 USE_YN='N' 처리 - 이력/참조무결성 보존을 위한 표준 삭제 방식</summary>
    public async Task SetUseYnAsync(string userId, bool useYn)
    {
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync("USP_SM_USER_S_1",
            new { UserId = userId, UseYn = useYn ? "Y" : "N" }, commandType: CommandType.StoredProcedure);
    }

    public async Task<List<UserGroupAssignRow>> GetUserGroupsAsync(string userId)
    {
        using var conn = _context.CreateConnection();
        var result = await conn.QueryAsync<UserGroupAssignRow>("USP_SM_USER_Q_2",
            new { UserId = userId }, commandType: CommandType.StoredProcedure);
        return result.ToList();
    }

    /// <summary>체크된 그룹코드 목록으로 이 사용자의 소속을 치환(콤마구분 문자열로 프로시저 전달)</summary>
    public async Task ReplaceUserGroupsAsync(string userId, List<string> userGrpCds)
    {
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync("USP_SM_USER_S_2", new
        {
            UserId = userId,
            UserGrpCds = userGrpCds.Count > 0 ? string.Join(",", userGrpCds) : null
        }, commandType: CommandType.StoredProcedure);
    }
}
