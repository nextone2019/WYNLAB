using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Repositories;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers;

/// <summary>
/// 사용자관리 화면(TSMUSER)용 CRUD API.
/// 로그인(AuthController)과 달리 [Authorize] 필수 - JWT 토큰 없으면 401.
/// 화면 자체의 등록/수정/삭제 권한 체크(CanInsert 등)는 클라이언트에서 버튼 비활성화로 1차 처리하지만,
/// 서버에서도 최소한 "로그인 여부"는 반드시 검증한다.
/// </summary>
[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserManageRepository _repo;

    public UsersController(IUserManageRepository repo) => _repo = repo;

    [HttpGet]
    public async Task<ActionResult<List<UserListItemDto>>> GetAll([FromQuery] string? userId, [FromQuery] string? userNm)
    {
        var rows = await _repo.GetAllAsync(userId, userNm);
        return Ok(rows.Select(MapToDto).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<ApiResult>> Create([FromBody] UserCreateRequest request)
    {
        if (await _repo.ExistsAsync(request.UserId))
            return Ok(new ApiResult { Success = false, Message = "이미 존재하는 아이디입니다." });

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        await _repo.CreateAsync(request.UserId, request.UserNm, passwordHash, request.EmpNo,
            request.DeptCd, request.PositionNm, request.Email, request.MobileNo, request.IsAdminYn);

        return Ok(new ApiResult { Success = true });
    }

    [HttpPut("{userId}")]
    public async Task<ActionResult<ApiResult>> Update(string userId, [FromBody] UserUpdateRequest request)
    {
        if (!await _repo.ExistsAsync(userId))
            return Ok(new ApiResult { Success = false, Message = "존재하지 않는 사용자입니다." });

        await _repo.UpdateAsync(userId, request.UserNm, request.EmpNo, request.DeptCd,
            request.PositionNm, request.Email, request.MobileNo, request.UseYn, request.IsAdminYn);

        return Ok(new ApiResult { Success = true });
    }

    /// <summary>물리삭제가 아닌 사용여부(USE_YN) 비활성화 - 표준 삭제 방식</summary>
    [HttpDelete("{userId}")]
    public async Task<ActionResult<ApiResult>> Delete(string userId)
    {
        await _repo.SetUseYnAsync(userId, useYn: false);
        return Ok(new ApiResult { Success = true });
    }

    private static UserListItemDto MapToDto(UserManageRow row) => new()
    {
        UserId = row.UserId,
        UserNm = row.UserNm,
        EmpNo = row.EmpNo,
        DeptCd = row.DeptCd,
        DeptNm = row.DeptNm,
        PositionNm = row.PositionNm,
        Email = row.Email,
        MobileNo = row.MobileNo,
        UseYn = row.UseYn == "Y",
        IsAdminYn = row.IsAdminYn == "Y",
        LastLoginDt = row.LastLoginDt
    };
}
