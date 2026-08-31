using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Repositories;
using WYNLAB.Api.Repositories.Framework;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.Framework;

/// <summary>
/// 팝업 검색(PopupLookupEditWyn) 런타임 조회. 로그인만 되어 있으면 어느 화면에서든 호출 가능해야
/// 하는 공용 조회라(SSP_CBO_CODE_Q/MinorCodeController.Lookup과 같은 이유) [RequireMenuPermission]을
/// 안 건다 - 화면별 메뉴권한/PROC_PREFIX로는 다른 엔티티의 팝업을 못 열게 되므로.
/// </summary>
[ApiController]
[Route("api/lookups")]
[Authorize]
public class LookupsController : ControllerBase
{
    private readonly IPopupLookupRepository _popupRepo;
    private readonly IGenericDataRepository _dataRepo;

    public LookupsController(IPopupLookupRepository popupRepo, IGenericDataRepository dataRepo)
    {
        _popupRepo = popupRepo;
        _dataRepo = dataRepo;
    }

    /// <summary>이 팝업을 어떻게 그릴지(트리/그리드, 컬럼 구성, 창 크기) - PopupLookupEditWyn이
    /// "..." 버튼을 처음 누를 때 한 번 받아온다.</summary>
    [HttpGet("{key}/definition")]
    public async Task<ActionResult<PopupDefinitionDto>> GetDefinition(string key)
    {
        var def = await _popupRepo.GetDefinitionAsync(key);
        if (def == null) return NotFound();
        return Ok(def);
    }

    /// <summary>실제 검색 실행 - sysPopUpM.proc_nm(SSP_POP_*_Q)을 찾아서 조회조건 값들로 부른다.
    /// 프로시저마다 파라미터명/개수가 전부 달라서(사장님 결정 - "@p_keyword 하나로 통일" 가정을
    /// 버림) 화면이 "조회조건 param_nm -> 입력값" 딕셔너리를 그대로 보내고, 그걸 GenericDataRepository가
    /// "p_"로 시작하는 키만 걸러서 그대로 프로시저 파라미터로 넘긴다(QueryAsync는 원래도 파라미터
    /// 개수에 대한 가정이 없다 - 여기서 새로 바뀐 건 화면이 한 개가 아니라 여러 개를 보낼 수
    /// 있다는 것뿐). 쿼리스트링으로는 임의 키 개수를 깔끔하게 못 받아서 POST+JSON 바디로 받는다.
    /// 프로시저 이름은 화면이 아니라 서버가 sysPopUpM에서 조회한 값이므로, 화면이 임의의
    /// 프로시저명을 실행시킬 수는 없다(popup_key 화이트리스트 = sysPopUpM에 등록된 것만).</summary>
    [HttpPost("{key}/search")]
    public async Task<ActionResult<DataQueryResponse>> Search(string key, [FromBody] Dictionary<string, string?> conditions)
    {
        var def = await _popupRepo.GetDefinitionAsync(key);
        if (def == null) return NotFound();

        var result = await _dataRepo.QueryAsync(def.ProcNm, conditions);
        return Ok(result);
    }
}
