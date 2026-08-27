using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Authorization;
using WYNLAB.Api.Repositories.SM;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.SM;

/// <summary>
/// 사용자그룹관리 화면(TSMUSERGRP + TSMUSERGRPMAP)용 CRUD/배정 API. 로그인 필수(JWT).
/// 각 액션은 [RequireMenuPermission("SM_USER", ...)]로 TSMMENUAUTH 기준 권한을 서버에서도 검증한다.
/// 메뉴코드가 SM_USER인 이유: frmUserManage의 사용자그룹관리 탭이 쓰는데, 전용 메뉴였던
/// SM_USERGRP는 SM 모듈 정리 때(013_SM_Retire_Redundant_Menus.sql) TSMMENU에서 삭제됐다 -
/// 그때 이 컨트롤러의 권한체크 참조를 SM_USER로 같이 옮겼어야 했는데 빠뜨려서, 이후 admin을
/// 포함한 모든 사용자가 이 API에서 항상 403을 받는 상태로 남아 있었다(MenuAuthController의
/// SM_AUTH와 같은 문제 - 2026-08-27 발견/수정).
/// </summary>
[ApiController]
[Route("api/user-groups")]
[Authorize]
public class UserGroupsController : ControllerBase
{
    private readonly IUserGroupManageRepository _repo;

    public UserGroupsController(IUserGroupManageRepository repo) => _repo = repo;

    [HttpGet]
    [RequireMenuPermission("SM_USER", MenuAction.View)]
    public async Task<ActionResult<List<UserGroupListItemDto>>> GetAll([FromQuery] string? userGrpNm)
    {
        var rows = await _repo.GetAllAsync(userGrpNm);
        return Ok(rows.Select(MapToDto).ToList());
    }

    [HttpPost]
    [RequireMenuPermission("SM_USER", MenuAction.Insert)]
    public async Task<ActionResult<ApiResult>> Create([FromBody] UserGroupCreateRequest request)
    {
        if (await _repo.ExistsAsync(request.UserGrpCd))
            return Ok(new ApiResult { Success = false, Message = "이미 존재하는 그룹코드입니다." });

        var result = await _repo.CreateAsync(request.UserGrpCd, request.UserGrpNm, request.Description, request.SortOrder);
        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage });

        return Ok(new ApiResult { Success = true, GeneratedCode = result.GeneratedCode });
    }

    [HttpPut("{userGrpCd}")]
    [RequireMenuPermission("SM_USER", MenuAction.Update)]
    public async Task<ActionResult<ApiResult>> Update(string userGrpCd, [FromBody] UserGroupUpdateRequest request)
    {
        if (!await _repo.ExistsAsync(userGrpCd))
            return Ok(new ApiResult { Success = false, Message = "존재하지 않는 그룹입니다." });

        var result = await _repo.UpdateAsync(userGrpCd, request.UserGrpNm, request.Description, request.SortOrder, request.UseYn);
        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage });

        return Ok(new ApiResult { Success = true });
    }

    [HttpDelete("{userGrpCd}")]
    [RequireMenuPermission("SM_USER", MenuAction.Delete)]
    public async Task<ActionResult<ApiResult>> Delete(string userGrpCd)
    {
        var result = await _repo.SetUseYnAsync(userGrpCd, useYn: false);
        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage });

        return Ok(new ApiResult { Success = true });
    }

    /// <summary>이 그룹의 소속 배정 화면용 - 전체 사용자 + 소속여부(IsMember)</summary>
    [HttpGet("{userGrpCd}/members")]
    [RequireMenuPermission("SM_USER", MenuAction.View)]
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
    [RequireMenuPermission("SM_USER", MenuAction.Update)]
    public async Task<ActionResult<ApiResult>> UpdateMembers(string userGrpCd, [FromBody] UpdateGroupMembersRequest request)
    {
        if (!await _repo.ExistsAsync(userGrpCd))
            return Ok(new ApiResult { Success = false, Message = "존재하지 않는 그룹입니다." });

        var result = await _repo.ReplaceMembersAsync(userGrpCd, request.UserIds);
        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage });

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
