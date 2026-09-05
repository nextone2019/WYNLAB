using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Authorization;
using WYNLAB.Api.Repositories;
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
    private readonly IGenericDataRepository _dataRepo;

    public LookupAdminController(IPopupLookupRepository repo, IGenericDataRepository dataRepo)
    {
        _repo = repo;
        _dataRepo = dataRepo;
    }

    public class DescribeProcRequest
    {
        public string ProcName { get; set; } = string.Empty;
    }

    [HttpPost("describe-proc")]
    [RequireMenuPermission("SYS", "frmSysLookup", MenuAction.View)]
    public async Task<ActionResult<DescribeProcResultDto>> DescribeProc([FromBody] DescribeProcRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ProcName) || !request.ProcName.StartsWith("SSP_", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new ApiResult { Success = false, Message = "SSP_로 시작하는 프로시저만 지정할 수 있습니다." });

        var result = await _repo.DescribeProcAsync(request.ProcName);
        return Ok(result);
    }

    public class DescribeQueryRequest
    {
        public string QueryText { get; set; } = string.Empty;
    }

    /// <summary>LookUp소스유형='쿼리'용 "파라미터생성" - DescribeProc과 같은 목적(입력 파라미터 +
    /// 결과셋 컬럼)이지만 대상이 프로시저가 아니라 쿼리문 텍스트 자체다. SELECT로 시작하는지만
    /// 최소한으로 확인한다 - 관리자(SYS_LOOKUP 화면 권한 보유자)가 직접 입력한 텍스트이고, 실제로
    /// 실행하는 게 아니라 메타데이터만 읽으므로(DescribeQueryAsync 참고) 그 이상 제한할 이유는
    /// 없다.</summary>
    [HttpPost("describe-query")]
    [RequireMenuPermission("SYS", "frmSysLookup", MenuAction.View)]
    public async Task<ActionResult<DescribeProcResultDto>> DescribeQuery([FromBody] DescribeQueryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.QueryText) || !request.QueryText.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new ApiResult { Success = false, Message = "SELECT로 시작하는 쿼리문만 지정할 수 있습니다." });

        var result = await _repo.DescribeQueryAsync(request.QueryText);
        return Ok(result);
    }

    public class PreviewRequest
    {
        public string SourceType { get; set; } = "P";
        public string? ProcName { get; set; }
        public string? QueryText { get; set; }

        /// <summary>파라미터 목록(grd2)에 등록된 이름 -> 테스트값. 값을 안 채운 파라미터는 그대로
        /// NULL로 실행된다 - 필터 조건에 쓰이는 파라미터가 NULL이면 SQL에서 `= NULL`은 오류가
        /// 아니라 그냥 0건이라 안전하지만, 그러면 미리보기가 매번 빈 결과만 보여줘 쓸모가 없다는
        /// 지적(2026-09-02)으로 grd2에 "테스트값" 컬럼을 추가해서 admin이 직접 값을 넣어볼 수
        /// 있게 했다. 키는 이미 "p_" 접두사가 붙은 채로 저장돼 있다(sysLookupP.param_nm 관례).</summary>
        public Dictionary<string, string?> Params { get; set; } = new();
    }

    /// <summary>"실행결과 미리보기" 버튼 - 저장 여부와 무관하게 지금 화면에 입력된 프로시저/쿼리를
    /// 실제로 실행해서 결과를 그대로 보여준다. 실행 자체는 GenericDataRepository를 그대로
    /// 재사용한다 - 이 LookUp을 실제로 쓰는 화면이 열릴 때(LookupRepository.GetItemsAsync)와
    /// 완전히 같은 통로라, "미리보기"라고 해서 더 위험하거나 더 안전하지 않다(같은 신뢰 수준).</summary>
    [HttpPost("preview")]
    [RequireMenuPermission("SYS", "frmSysLookup", MenuAction.View)]
    public async Task<ActionResult<DataQueryResponse>> Preview([FromBody] PreviewRequest request)
    {
        var isQuery = request.SourceType == "Q";
        if (isQuery && (string.IsNullOrWhiteSpace(request.QueryText) || !request.QueryText.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase)))
            return BadRequest(new ApiResult { Success = false, Message = "SELECT로 시작하는 쿼리문만 지정할 수 있습니다." });
        if (!isQuery && (string.IsNullOrWhiteSpace(request.ProcName) || !request.ProcName.StartsWith("SSP_", StringComparison.OrdinalIgnoreCase)))
            return BadRequest(new ApiResult { Success = false, Message = "SSP_로 시작하는 프로시저만 지정할 수 있습니다." });

        var paramValues = request.Params;

        var result = isQuery
            ? await _dataRepo.QueryRawAsync(request.QueryText!, paramValues)
            : await _dataRepo.QueryAsync(request.ProcName!, paramValues);

        return Ok(result);
    }
}
