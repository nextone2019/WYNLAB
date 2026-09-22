using System.Security.Cryptography;
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
    private readonly ISiteConfigRepository _siteConfig;

    public UsersController(IUserManageRepository repo, ISiteConfigRepository siteConfig)
    {
        _repo = repo;
        _siteConfig = siteConfig;
    }

    [HttpGet]
    [RequireMenuPermission("SM", "frmUserAuth", MenuAction.View)]
    public async Task<ActionResult<List<UserListItemDto>>> GetAll([FromQuery] string? userId, [FromQuery] string? userNm)
    {
        var rows = await _repo.GetAllAsync(userId, userNm);
        return Ok(rows.Select(MapToDto).ToList());
    }

    /// <summary>초기 비밀번호는 TSMSITECONFIG.init_pwd_policy(frmSiteConfig 비밀번호정책)를
    /// 따른다 - "RANDOM"이면 서버가 무작위로 생성해서 ApiResult.InitialPassword로 알려주고,
    /// 그 외(기본값)엔 예전부터의 관례대로 아이디와 같은 값을 쓴다. force_change_on_first_login이
    /// 켜져 있으면 MUST_CHANGE_PWD_YN='Y'로 만들어 최초 로그인 시 비밀번호 변경을 강제한다
    /// (2026-09-06 연동 - 그 전까진 frmUserAuth 화면에 비밀번호 입력 UI 자체가 없어서 항상
    /// 아이디와 동일한 값으로 "임시 처리"만 해뒀었다).</summary>
    [HttpPost]
    [RequireMenuPermission("SM", "frmUserAuth", MenuAction.Insert)]
    public async Task<ActionResult<ApiResult>> Create([FromBody] UserCreateRequest request)
    {
        if (await _repo.ExistsAsync(request.UserId))
            return Ok(new ApiResult { Success = false, Message = "이미 존재하는 아이디입니다." });

        var config = await _siteConfig.GetAsync();
        var initialPassword = config?.InitPwdPolicy == "RANDOM" ? GenerateRandomPassword() : request.UserId;
        var mustChangePwd = config?.ForceChangeOnFirstLogin ?? false;

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(initialPassword);
        var result = await _repo.CreateAsync(request.UserId, request.UserNm, passwordHash, request.EmpId, request.AccId, request.UserType, request.Email, mustChangePwd);

        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage });

        return Ok(new ApiResult { Success = true, GeneratedCode = result.GeneratedCode, InitialPassword = initialPassword });
    }

    /// <summary>영문 대/소문자+숫자 10자리 - 예측 불가능해야 하므로 RandomNumberGenerator를 쓴다
    /// (AuthService.GenerateCode와 같은 이유, System.Random 아님).</summary>
    private static string GenerateRandomPassword()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789";
        var bytes = RandomNumberGenerator.GetBytes(10);
        return new string(bytes.Select(b => chars[b % chars.Length]).ToArray());
    }

    [HttpPut("{userId}")]
    [RequireMenuPermission("SM", "frmUserAuth", MenuAction.Update)]
    public async Task<ActionResult<ApiResult>> Update(string userId, [FromBody] UserUpdateRequest request)
    {
        if (!await _repo.ExistsAsync(userId))
            return Ok(new ApiResult { Success = false, Message = "존재하지 않는 사용자입니다." });

        var result = await _repo.UpdateAsync(userId, request.UserNm, request.EmpId, request.AccId, request.UserType, request.Email, request.UseYn);

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
        EmpId = row.EmpId,
        EmpNo = row.EmpNo,
        EmpNm = row.EmpNm,
        DeptNm = row.DeptNm,
        AccId = row.AccId,
        AccNm = row.AccNm,
        Email = row.Email,
        UseYn = row.UseYn == "Y",
        UserType = row.UserType,
        DeveloperYn = row.DeveloperYn == "Y",
        LastLoginDt = row.LastLoginDt
    };
}
