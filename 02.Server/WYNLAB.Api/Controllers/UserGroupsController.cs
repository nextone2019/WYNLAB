using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Authorization;
using WYNLAB.Api.Repositories;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers;

/// <summary>
/// 사용자그룹관리 화면(TSMUSERGRP + TSMUSERGRPMAP)용 CRUD/배정 API. 로그인 필수(JWT).
/// 각 액션은 [RequireMenuPermission("SM_USERGRP", ...)]로 TSMMENUAUTH 기준 권한을 서버에서도 검증한다.
/// </summary>
[ApiController]
[Route("api/user-groups")]
[Authorize]
public class UserGroupsController : ControllerBase
{
    private readonly IUserGroupManageRepository _repo;

    public UserGroupsController(IUserGroupManageRepository repo) => _repo = repo;

    [HttpGet]
    [RequireMenuPermission("SM_USERGRP", MenuAction.View)]
    public async Task<ActionResult<List<UserGroupListItemDto>>> GetAll([FromQuery] string? userGrpNm)
    {
        var rows = await _repo.GetAllAsync(userGrpNm);
        return Ok(rows.Select(MapToDto).ToList());
    }

    [HttpPost]
    [RequireMenuPermission("SM_USERGRP", MenuAction.Insert)]
    public async Task<ActionResult<ApiResult>> Create([FromBody] UserGroupCreateRequest request)
    {
        if (await _repo.ExistsAsync(request.UserGrpCd))
            return Ok(new ApiResult { Success = false, Message = "이미 존재하는 그룹코드입니다." });

        await _repo.CreateAsync(request.UserGrpCd, request.UserGrpNm, request.Description, request.SortOrder);
        return Ok(new ApiResult { Success = true });
    }

    [HttpPut("{userGrpCd}")]
    [RequireMenuPermission("SM_USERGRP", MenuAction.Update)]
    public async Task<ActionResult<ApiResult>> Update(string userGrpCd, [FromBody] UserGroupUpdateRequest request)
    {
        if (!await _repo.ExistsAsync(userGrpCd))
            return Ok(new ApiResult { Success = false, Message = "존재하지 않는 그룹입니다." });

        await _repo.UpdateAsync(userGrpCd, request.UserGrpNm, request.Description, request.SortOrder, request.UseYn);
        return Ok(new ApiResult { Success = true });
    }

    [HttpDelete("{userGrpCd}")]
    [RequireMenuPermission("SM_USERGRP", MenuAction.Delete)]
    public async Task<ActionResult<ApiResult>> Delete(string userGrpCd)
    {
        await _repo.SetUseYnAsync(userGrpCd, useYn: false);
        return Ok(new ApiResult { Success = true });
    }

    /// <summary>이 그룹의 소속 배정 화면용 - 전체 사용자 + 소속여부(IsMember)</summary>
    [HttpGet("{userGrpCd}/members")]
    [RequireMenuPermission("SM_USERGRP", MenuAction.View)]
    public async Task<ActionResult<List<UserGroupMemberDto>>> GetMembers(string userGrpCd)
    {
        var rows = await _repo.GetMembersAsync(userGrpCd);
        return Ok(rows.Select(r => new UserGroupMemberDto
        {
            UserId = r.UserId,
            UserNm = r.UserNm,
            DeptNm = r.DeptNm,
            IsMember = r.IsMember
        }).ToList());
    }

    /// <summary>체크된 사용자 목록으로 소속을 치환. 화면에서 저장 버튼 누를 때 한 번에 호출</summary>
    [HttpPut("{userGrpCd}/members")]
    [RequireMenuPermission("SM_USERGRP", MenuAction.Update)]
    public async Task<ActionResult<ApiResult>> UpdateMembers(string userGrpCd, [FromBody] UpdateGroupMembersRequest request)
    {
        if (!await _repo.ExistsAsync(userGrpCd))
            return Ok(new ApiResult { Success = false, Message = "존재하지 않는 그룹입니다." });

        await _repo.ReplaceMembersAsync(userGrpCd, request.UserIds);
        return Ok(new ApiResult { Success = true });
    }

    private static UserGroupListItemDto MapToDto(UserGroupManageRow row) => new()
    {
        UserGrpCd = row.UserGrpCd,
        UserGrpNm = row.UserGrpNm,
        Description = row.Description,
        SortOrder = row.SortOrder,
        UseYn = row.UseYn == "Y",
        MemberCount = row.MemberCount
    };
}
