using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Repositories.Framework;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.Framework;

/// <summary>
/// LookUp(콤보) 런타임 조회(LookUpEditWyn.LookupKey). 로그인만 되어 있으면 어느 화면에서든
/// 호출 가능해야 하는 공용 조회라(LookupsController.Search와 같은 이유) [RequireMenuPermission]을
/// 안 건다 - 화면별 메뉴권한/PROC_PREFIX로는 다른 화면이 등록한 LookUp을 못 쓰게 되므로.
/// </summary>
[ApiController]
[Route("api/combo-lookups")]
[Authorize]
public class ComboLookupsController : ControllerBase
{
    private readonly ILookupRepository _repo;

    public ComboLookupsController(ILookupRepository repo) => _repo = repo;

    /// <summary>실제 조회 실행 - sysLookupM.proc_nm을 찾아서 파라미터 값들로 부르고 value/display
    /// (항상)와 컬럼 구성(sysLookupC, 설정 없으면 빈 리스트)을 같이 돌려준다. 파라미터 개수는
    /// LookUp마다 다를 수 있어(sysLookupP 참고) POST+JSON 바디로 받는다(팝업 검색과 같은 이유 -
    /// PopupLookupForm/LookupsController.Search 참고).</summary>
    [HttpPost("{key}/items")]
    public async Task<ActionResult<LookupItemsResultDto>> GetItems(string key, [FromBody] Dictionary<string, string?> paramValues)
    {
        var result = await _repo.GetItemsAsync(key, paramValues);
        if (result == null) return NotFound();
        return Ok(result);
    }
}
