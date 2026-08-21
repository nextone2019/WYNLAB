using System.Data;
using Dapper;
using WYNLAB.Api.Data;

namespace WYNLAB.Api.Repositories;

public interface IUserGroupManageRepository
{
    Task<List<UserGroupManageRow>> GetAllAsync(string? userGrpNm = null);
    Task<bool> ExistsAsync(string userGrpCd);
    Task CreateAsync(string userGrpCd, string userGrpNm, string? description, int sortOrder);
    Task UpdateAsync(string userGrpCd, string userGrpNm, string? description, int sortOrder, bool useYn);
    Task SetUseYnAsync(string userGrpCd, bool useYn);
    Task<List<GroupMemberRow>> GetMembersAsync(string userGrpCd);
    Task ReplaceMembersAsync(string userGrpCd, List<string> userIds);
}

/// <summary>DB 조회 전용 - 관리화면 목록 표시용. MemberCount는 프로시저 내부에서 서브쿼리로 같이 집계</summary>
public class UserGroupManageRow
{
    public string UserGrpCd { get; set; } = string.Empty;
    public string UserGrpNm { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public string UseYn { get; set; } = "Y";
    public int MemberCount { get; set; }
}

public class GroupMemberRow
{
    public string UserId { get; set; } = string.Empty;
    public string UserNm { get; set; } = string.Empty;
    public string? DeptNm { get; set; }
    public bool IsMember { get; set; }
}

/// <summary>
/// 데이터 처리는 전부 저장프로시저(USP_SM_USERGRP_*)로 위임한다.
/// 명명규칙: USP_SM_USERGRP_Q(조회) / _Q_1,_Q_2(보조조회) / _S(저장) / _S_1,_S_2(추가 저장동작)
/// </summary>
public class UserGroupManageRepository : IUserGroupManageRepository
{
    private readonly IDapperContext _context;

    public UserGroupManageRepository(IDapperContext context) => _context = context;

    public async Task<List<UserGroupManageRow>> GetAllAsync(string? userGrpNm = null)
    {
        using var conn = _context.CreateConnection();
        var result = await conn.QueryAsync<UserGroupManageRow>("USP_SM_USERGRP_Q",
            new { UserGrpNm = string.IsNullOrWhiteSpace(userGrpNm) ? null : userGrpNm },
            commandType: CommandType.StoredProcedure);
        return result.ToList();
    }

    public async Task<bool> ExistsAsync(string userGrpCd)
    {
        using var conn = _context.CreateConnection();
        var count = await conn.ExecuteScalarAsync<int>("USP_SM_USERGRP_Q_1",
            new { UserGrpCd = userGrpCd }, commandType: CommandType.StoredProcedure);
        return count > 0;
    }

    public async Task CreateAsync(string userGrpCd, string userGrpNm, string? description, int sortOrder)
    {
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync("USP_SM_USERGRP_S", new
        {
            Mode = "C",
            UserGrpCd = userGrpCd,
            UserGrpNm = userGrpNm,
            Description = description,
            SortOrder = sortOrder
        }, commandType: CommandType.StoredProcedure);
    }

    public async Task UpdateAsync(string userGrpCd, string userGrpNm, string? description, int sortOrder, bool useYn)
    {
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync("USP_SM_USERGRP_S", new
        {
            Mode = "U",
            UserGrpCd = userGrpCd,
            UserGrpNm = userGrpNm,
            Description = description,
            SortOrder = sortOrder,
            UseYn = useYn ? "Y" : "N"
        }, commandType: CommandType.StoredProcedure);
    }

    /// <summary>물리삭제 대신 USE_YN='N' 처리 - 과거 소속이력(TSMUSERGRPMAP) 참조무결성 보존을 위한 표준 삭제 방식</summary>
    public async Task SetUseYnAsync(string userGrpCd, bool useYn)
    {
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync("USP_SM_USERGRP_S_1",
            new { UserGrpCd = userGrpCd, UseYn = useYn ? "Y" : "N" }, commandType: CommandType.StoredProcedure);
    }

    public async Task<List<GroupMemberRow>> GetMembersAsync(string userGrpCd)
    {
        using var conn = _context.CreateConnection();
        var result = await conn.QueryAsync<GroupMemberRow>("USP_SM_USERGRP_Q_2",
            new { UserGrpCd = userGrpCd }, commandType: CommandType.StoredProcedure);
        return result.ToList();
    }

    /// <summary>체크된 사용자 목록으로 소속을 치환(콤마구분 문자열로 프로시저 전달)</summary>
    public async Task ReplaceMembersAsync(string userGrpCd, List<string> userIds)
    {
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync("USP_SM_USERGRP_S_2", new
        {
            UserGrpCd = userGrpCd,
            UserIds = userIds.Count > 0 ? string.Join(",", userIds) : null
        }, commandType: CommandType.StoredProcedure);
    }
}
