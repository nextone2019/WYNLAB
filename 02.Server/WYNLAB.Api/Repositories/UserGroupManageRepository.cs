using System.Data;
using Dapper;
using WYNLAB.Api.Data;

namespace WYNLAB.Api.Repositories;

public interface IUserGroupManageRepository
{
    Task<List<UserGroupManageRow>> GetAllAsync(string? userGrpNm = null);
    Task<bool> ExistsAsync(string userGrpCd);
    Task<ProcResult> CreateAsync(string userGrpCd, string userGrpNm, string? description, int sortOrder);
    Task<ProcResult> UpdateAsync(string userGrpCd, string userGrpNm, string? description, int sortOrder, bool useYn);
    Task<ProcResult> SetUseYnAsync(string userGrpCd, bool useYn);
    Task<List<GroupMemberRow>> GetMembersAsync(string userGrpCd);
    Task<ProcResult> ReplaceMembersAsync(string userGrpCd, List<string> userIds);
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
            new { p_work_type = "Q", p_user_grp_nm = string.IsNullOrWhiteSpace(userGrpNm) ? null : userGrpNm },
            commandType: CommandType.StoredProcedure);
        return result.ToList();
    }

    public async Task<bool> ExistsAsync(string userGrpCd)
    {
        using var conn = _context.CreateConnection();
        var count = await conn.ExecuteScalarAsync<int>("USP_SM_USERGRP_Q_1",
            new { p_work_type = "Q", p_user_grp_cd = userGrpCd }, commandType: CommandType.StoredProcedure);
        return count > 0;
    }

    public async Task<ProcResult> CreateAsync(string userGrpCd, string userGrpNm, string? description, int sortOrder)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "N");
        p.Add("p_user_grp_cd", userGrpCd);
        p.Add("p_user_grp_nm", userGrpNm);
        p.Add("p_description", description);
        p.Add("p_sort_order", sortOrder);
        p.AddStandardOutputs(withGeneratedCode: true, pascalCase: true);

        await conn.ExecuteAsync("USP_SM_USERGRP_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(withGeneratedCode: true, pascalCase: true);
    }

    public async Task<ProcResult> UpdateAsync(string userGrpCd, string userGrpNm, string? description, int sortOrder, bool useYn)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "U");
        p.Add("p_user_grp_cd", userGrpCd);
        p.Add("p_user_grp_nm", userGrpNm);
        p.Add("p_description", description);
        p.Add("p_sort_order", sortOrder);
        p.Add("p_use_yn", useYn ? "Y" : "N");
        p.AddStandardOutputs(withGeneratedCode: true, pascalCase: true);

        await conn.ExecuteAsync("USP_SM_USERGRP_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(withGeneratedCode: true, pascalCase: true);
    }

    /// <summary>물리삭제 대신 USE_YN='N' 처리 - 과거 소속이력(TSMUSERGRPMAP) 참조무결성 보존을 위한 표준 삭제 방식(work_type='D')</summary>
    public async Task<ProcResult> SetUseYnAsync(string userGrpCd, bool useYn)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "D");
        p.Add("p_user_grp_cd", userGrpCd);
        p.Add("p_use_yn", useYn ? "Y" : "N");
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("USP_SM_USERGRP_S_1", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }

    public async Task<List<GroupMemberRow>> GetMembersAsync(string userGrpCd)
    {
        using var conn = _context.CreateConnection();
        var result = await conn.QueryAsync<GroupMemberRow>("USP_SM_USERGRP_Q_2",
            new { p_work_type = "Q", p_user_grp_cd = userGrpCd }, commandType: CommandType.StoredProcedure);
        return result.ToList();
    }

    /// <summary>체크된 사용자 목록으로 소속을 치환(콤마구분 문자열로 프로시저 전달, work_type='U')</summary>
    public async Task<ProcResult> ReplaceMembersAsync(string userGrpCd, List<string> userIds)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "U");
        p.Add("p_user_grp_cd", userGrpCd);
        p.Add("p_user_ids", userIds.Count > 0 ? string.Join(",", userIds) : null);
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("USP_SM_USERGRP_S_2", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }
}
