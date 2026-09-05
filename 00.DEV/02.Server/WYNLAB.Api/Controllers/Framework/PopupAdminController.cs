using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Authorization;
using WYNLAB.Api.Repositories.Framework;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.Framework;

/// <summary>
/// 팝업관리 화면(SYS_POPUP) 전용 - "컬럼생성" 버튼이 부르는 프로시저 구조 조회(입력 파라미터 +
/// 결과셋 컬럼, 둘 다). 이건 조회권한이 있는 사람만 써야 하는 관리 기능이라(sysPopUpM/D/S 자체
/// CRUD와 달리 임의의 프로시저 이름을 넣어 구조를 캐볼 수 있음) 다른 lookups 엔드포인트와 달리
/// 메뉴권한을 건다. sysPopUpM/D/S 자체의 조회/저장은 새 컨트롤러 없이 기존 범용 데이터 통로
/// (api/data/*)를 그대로 쓴다 - USP_SYS_POPUP_Q/_S가 다른 화면들과 똑같은 방식으로 동작하기 때문.
/// </summary>
[ApiController]
[Route("api/popup-admin")]
[Authorize]
public class PopupAdminController : ControllerBase
{
    private readonly IPopupLookupRepository _repo;

    public PopupAdminController(IPopupLookupRepository repo) => _repo = repo;

    public class DescribeColumnsRequest
    {
        public string ProcName { get; set; } = string.Empty;
    }

    [HttpPost("describe-proc")]
    [RequireMenuPermission("SYS", "frmSysPopup", MenuAction.View)]
    public async Task<ActionResult<DescribeProcResultDto>> DescribeProc([FromBody] DescribeColumnsRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ProcName) || !request.ProcName.StartsWith("SSP_POP_", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new ApiResult { Success = false, Message = "SSP_POP_로 시작하는 프로시저만 지정할 수 있습니다." });

        var result = await _repo.DescribeProcAsync(request.ProcName);
        return Ok(result);
    }
}
