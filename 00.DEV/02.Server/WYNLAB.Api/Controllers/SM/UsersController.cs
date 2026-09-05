using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Authorization;
using WYNLAB.Api.Repositories.SM;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.SM;

/// <summary>
/// 사용자관리 화면(TSMUSER)용 CRUD API.
/// 로그인(AuthController)과 달리 [Authorize] 필수 - JWT 토큰 없으면 401.
/// 등록/수정/삭제/조회 각 액션은 [RequireMenuPermission("SM", "frmUserAuth", ...)]로 TSMMENUAUTH 기준
/// 실제 권한도 서버에서 한 번 더 검증한다 - 클라이언트의 버튼 비활성화(CanInsert 등)는 UX용일
/// 뿐이고, API를 직접 호출하는 우회는 이 필터가 막는다.
/// </summary>
[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserManageRepository _repo;

    public UsersController(IUserManageRepository repo) => _repo = repo;

    [HttpGet]
    [RequireMenuPermission("SM", "frmUserAuth", MenuAction.View)]
    public async Task<ActionResult<List<UserListItemDto>>> GetAll([FromQuery] string? userId, [FromQuery] string? userNm)
    {
        var rows = await _repo.GetAllAsync(userId, userNm);
        return Ok(rows.Select(MapToDto).ToList());
    }

    [HttpPost]
    [RequireMenuPermission("SM", "frmUserAuth", MenuAction.Insert)]
    public async Task<ActionResult<ApiResult>> Create([FromBody] UserCreateRequest request)
    {
        if (await _repo.ExistsAsync(request.UserId))
            return Ok(new ApiResult { Success = false, Message = "이미 존재하는 아이디입니다." });

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var result = await _repo.CreateAsync(request.UserId, request.UserNm, passwordHash, request.EmpNo, request.Email);

        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage });

        return Ok(new ApiResult { Success = true, GeneratedCode = result.GeneratedCode });
    }

    [HttpPut("{userId}")]
    [RequireMenuPermission("SM", "frmUserAuth", MenuAction.Update)]
    public async Task<ActionResult<ApiResult>> Update(string userId, [FromBody] UserUpdateRequest request)
    {
        if (!await _repo.ExistsAsync(userId))
            return Ok(new ApiResult { Success = false, Message = "존재하지 않는 사용자입니다." });

        var result = await _repo.UpdateAsync(userId, request.UserNm, request.EmpNo, request.Email, request.UseYn);

        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage });

        return Ok(new ApiResult { Success = true });
    }

    /// <summary>물리삭제가 아닌 사용여부(USE_YN) 비활성화 - 표준 삭제 방식</summary>
    [HttpDelete("{userId}")]
    [RequireMenuPermission("SM", "frmUserAuth", MenuAction.Delete)]
    public async Task<ActionResult<ApiResult>> Delete(string userId)
    {
        var result = await _repo.SetUseYnAsync(userId, useYn: false);
        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage });

        return Ok(new ApiResult { Success = true });
    }

    /// <summary>이 사용자의 그룹소속 배정 화면용 - 전체 그룹 + 소속여부(IsMember)</summary>
    [HttpGet("{userId}/groups")]
    [RequireMenuPermission("SM", "frmUserAuth", MenuAction.View)]
    public async Task<ActionResult<List<UserGroupAssignDto>>> GetGroups(string userId)
    {
        var rows = await _repo.GetUserGroupsAsync(userId);
        return Ok(rows.Select(r => new UserGroupAssignDto
        {
            UserGrpCd = r.UserGrpCd,
            UserGrpNm = r.UserGrpNm,
            IsMember = r.IsMember
        }).ToList());
    }

    /// <summary>체크된 그룹 목록으로 이 사용자의 소속을 치환</summary>
    [HttpPut("{userId}/groups")]
    [RequireMenuPermission("SM", "frmUserAuth", MenuAction.Update)]
    public async Task<ActionResult<ApiResult>> UpdateGroups(string userId, [FromBody] UpdateUserGroupsRequest request)
    {
        var result = await _repo.ReplaceUserGroupsAsync(userId, request.UserGrpCds);
        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage });

        return Ok(new ApiResult { Success = true });
    }

    private static UserListItemDto MapToDto(UserManageRow row) => new()
    {
        UserId = row.UserId,
        UserNm = row.UserNm,
        EmpNo = row.EmpNo,
        EmpNm = row.EmpNm,
        DeptCd = row.DeptCd,
        DeptNm = row.DeptNm,
        Email = row.Email,
        UseYn = row.UseYn == "Y",
        DeveloperYn = row.DeveloperYn == "Y",
        LastLoginDt = row.LastLoginDt
    };
}
