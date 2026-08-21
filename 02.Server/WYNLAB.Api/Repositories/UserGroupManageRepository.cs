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

/// <summary>DB 조회 전용 - 관리화면 목록 표시용. MemberCount는 서브쿼리로 같이 집계</summary>
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

public class UserGroupManageRepository : IUserGroupManageRepository
{
    private readonly IDapperContext _context;

    public UserGroupManageRepository(IDapperContext context) => _context = context;

    public async Task<List<UserGroupManageRow>> GetAllAsync(string? userGrpNm = null)
    {
        const string sql = @"
            SELECT G.USER_GRP_CD AS UserGrpCd, G.USER_GRP_NM AS UserGrpNm, G.DESCRIPTION AS Description,
                   G.SORT_ORDER AS SortOrder, G.USE_YN AS UseYn,
                   (SELECT COUNT(1) FROM TSMUSERGRPMAP M WHERE M.USER_GRP_CD = G.USER_GRP_CD) AS MemberCount
            FROM TSMUSERGRP G
            WHERE (@UserGrpNm IS NULL OR G.USER_GRP_NM LIKE '%' + @UserGrpNm + '%')
            ORDER BY G.SORT_ORDER";

        using var conn = _context.CreateConnection();
        var result = await conn.QueryAsync<UserGroupManageRow>(sql,
            new { UserGrpNm = string.IsNullOrWhiteSpace(userGrpNm) ? null : userGrpNm });
        return result.ToList();
    }

    public async Task<bool> ExistsAsync(string userGrpCd)
    {
        const string sql = "SELECT COUNT(1) FROM TSMUSERGRP WHERE USER_GRP_CD = @UserGrpCd";
        using var conn = _context.CreateConnection();
        var count = await conn.ExecuteScalarAsync<int>(sql, new { UserGrpCd = userGrpCd });
        return count > 0;
    }

    public async Task CreateAsync(string userGrpCd, string userGrpNm, string? description, int sortOrder)
    {
        const string sql = @"
            INSERT INTO TSMUSERGRP (USER_GRP_CD, USER_GRP_NM, DESCRIPTION, SORT_ORDER, USE_YN)
            VALUES (@UserGrpCd, @UserGrpNm, @Description, @SortOrder, 'Y')";

        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync(sql, new { UserGrpCd = userGrpCd, UserGrpNm = userGrpNm, Description = description, SortOrder = sortOrder });
    }

    public async Task UpdateAsync(string userGrpCd, string userGrpNm, string? description, int sortOrder, bool useYn)
    {
        const string sql = @"
            UPDATE TSMUSERGRP SET
                USER_GRP_NM = @UserGrpNm, DESCRIPTION = @Description, SORT_ORDER = @SortOrder, USE_YN = @UseYn
            WHERE USER_GRP_CD = @UserGrpCd";

        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync(sql, new
        {
            UserGrpCd = userGrpCd,
            UserGrpNm = userGrpNm,
            Description = description,
            SortOrder = sortOrder,
            UseYn = useYn ? "Y" : "N"
        });
    }

    /// <summary>물리삭제 대신 USE_YN='N' 처리 - 과거 소속이력(TSMUSERGRPMAP) 참조무결성 보존을 위한 표준 삭제 방식</summary>
    public async Task SetUseYnAsync(string userGrpCd, bool useYn)
    {
        const string sql = "UPDATE TSMUSERGRP SET USE_YN = @UseYn WHERE USER_GRP_CD = @UserGrpCd";
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync(sql, new { UserGrpCd = userGrpCd, UseYn = useYn ? "Y" : "N" });
    }

    /// <summary>사용중인(USE_YN='Y') 전체 사용자에 대해, 이 그룹 소속 여부를 같이 내려준다 - 배정 화면의 체크그리드용</summary>
    public async Task<List<GroupMemberRow>> GetMembersAsync(string userGrpCd)
    {
        const string sql = @"
            SELECT U.USER_ID AS UserId, U.USER_NM AS UserNm, D.DEPT_NM AS DeptNm,
                   CASE WHEN M.USER_ID IS NULL THEN 0 ELSE 1 END AS IsMember
            FROM TSMUSER U
            LEFT JOIN TBADEPT D ON D.DEPT_CD = U.DEPT_CD
            LEFT JOIN TSMUSERGRPMAP M ON M.USER_ID = U.USER_ID AND M.USER_GRP_CD = @UserGrpCd
            WHERE U.USE_YN = 'Y'
            ORDER BY U.USER_NM";

        using var conn = _context.CreateConnection();
        var result = await conn.QueryAsync<GroupMemberRow>(sql, new { UserGrpCd = userGrpCd });
        return result.ToList();
    }

    /// <summary>기존 소속 매핑을 전부 지우고, 화면에서 체크된 userIds로 다시 채운다 (치환 방식, 트랜잭션 처리)</summary>
    public async Task ReplaceMembersAsync(string userGrpCd, List<string> userIds)
    {
        using var conn = _context.CreateConnection();
        conn.Open();
        using var tx = conn.BeginTransaction();

        await conn.ExecuteAsync(
            "DELETE FROM TSMUSERGRPMAP WHERE USER_GRP_CD = @UserGrpCd",
            new { UserGrpCd = userGrpCd }, tx);

        if (userIds.Count > 0)
        {
            await conn.ExecuteAsync(
                "INSERT INTO TSMUSERGRPMAP (USER_ID, USER_GRP_CD) VALUES (@UserId, @UserGrpCd)",
                userIds.Select(userId => new { UserId = userId, UserGrpCd = userGrpCd }), tx);
        }

        tx.Commit();
    }
}
