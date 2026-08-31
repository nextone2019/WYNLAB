using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Authorization;
using WYNLAB.Api.Repositories.Framework;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.Framework;

/// <summary>
/// LookUp관리 화면(SYS_LOOKUP) 전용 - "파라미터생성" 버튼이 부르는 프로시저 구조 조회(입력
/// 파라미터 + 결과셋 컬럼). PopupAdminController.DescribeProc와 완전히 같은 기능(프로시저
/// 이름만 다를 뿐 introspection 로직은 SSP_POP_*/SSP_CBO_* 구분 없이 동일)이라 같은
/// IPopupLookupRepository.DescribeProcAsync를 그대로 재사용한다 - introspection 로직을
/// 두 곳에 복붙하지 않기 위함. 메뉴권한은 이 화면(SYS_LOOKUP) 것을 건다 - PopupAdminController와
/// 권한이 분리되어 있어야 한쪽 화면 권한만 가진 사람이 다른 관리화면 기능까지 쓰게 되는 걸 막는다.
/// sysLookupM/P 자체의 조회/저장은 새 컨트롤러 없이 기존 범용 데이터 통로(api/data/*)를 그대로
/// 쓴다 - USP_SYS_LOOKUP_Q/_S가 다른 화면들과 똑같은 방식으로 동작하기 때문.
/// </summary>
[ApiController]
[Route("api/lookup-admin")]
[Authorize]
public class LookupAdminController : ControllerBase
{
    private readonly IPopupLookupRepository _repo;

    public LookupAdminController(IPopupLookupRepository repo) => _repo = repo;

    public class DescribeProcRequest
    {
        public string ProcName { get; set; } = string.Empty;
    }

    [HttpPost("describe-proc")]
    [RequireMenuPermission("SYS_LOOKUP", MenuAction.View)]
    public async Task<ActionResult<DescribeProcResultDto>> DescribeProc([FromBody] DescribeProcRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ProcName) || !request.ProcName.StartsWith("SSP_", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new ApiResult { Success = false, Message = "SSP_로 시작하는 프로시저만 지정할 수 있습니다." });

        var result = await _repo.DescribeProcAsync(request.ProcName);
        return Ok(result);
    }
}
