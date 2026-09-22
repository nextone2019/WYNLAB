using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Controllers;
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
/// 쓴다 - SSP_SYS_LOOKUP_Q/_S가 다른 화면들과 똑같은 방식으로 동작하기 때문.
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

    public class GenerateMinorLookupRequest
    {
        public string MajorCd { get; set; } = string.Empty;
    }

    /// <summary>frmMinorCode(대분류/소분류 등록)의 "LookUp생성" 버튼 - 지금 화면에 표시된
    /// 대분류 하나의 소분류 목록을 그대로 골라 쓸 수 있는 LookUp을 sysLookupM/C에 자동으로
    /// 만들어준다(2026-09-09 요청). lookup_key는 항상 "L_"+major_cd - 이미 같은 이름이 있으면
    /// 지우고 다시 만든다(대분류명이나 소분류 구성이 바뀐 뒤 다시 눌러도 항상 최신 상태가 되게).
    /// 기존 L_BA0001과 완전히 같은 구조(source_type='Q', TSMMINOR 조회, value_field=minor_cd/
    /// display_field=minor_nm, 코드/코드명 2열 컬럼 구성)로 만들되 WHERE절의 major_cd 리터럴만
    /// 이 대분류 값으로 바꾼다.
    ///
    /// frmMinorCode(SM)이 아니라 이 화면(SYS_LOOKUP)의 메뉴권한을 건다 - 클래스 설명대로
    /// LookUp관리는 이 컨트롤러 전체가 SYS_LOOKUP 화면 권한자만 쓸 수 있어야 하는 기능이라(Developer
    /// Tool 하위 기능), 트리거 버튼이 다른 화면(frmMinorCode)에 있다고 해서 그 화면 권한만으로
    /// sysLookupM/C를 쓸 수 있게 하면 이 컨트롤러의 권한 경계가 무너진다. 그래서 클라이언트도
    /// 이 화면 권한이 실린 범용 데이터 통로(BaseForm.SaveAsync, 메뉴별 PROC_PREFIX 화이트리스트)를
    /// 안 쓰고 이 전용 엔드포인트를 직접 부른다 - SSP_SYS_LOOKUP_S/_S_2도 여기서 서버 코드가
    /// 직접 고른 이름으로만 호출하므로(사용자가 프로시저명을 못 정함) 화이트리스트가 굳이 없어도
    /// 안전하다.</summary>
    [HttpPost("generate-minor-lookup")]
    [RequireMenuPermission("SYS", "frmSysLookup", MenuAction.Insert)]
    public async Task<ActionResult<ApiResult>> GenerateMinorLookup([FromBody] GenerateMinorLookupRequest request)
    {
        var majorCd = request.MajorCd?.Trim().ToUpperInvariant() ?? string.Empty;
        if (majorCd.Length == 0)
            return BadRequest(new ApiResult { Success = false, Message = "대분류코드가 없습니다." });

        var lookupKey = "L_" + majorCd;

        var majorRows = await _dataRepo.QueryRawAsync(
            "SELECT major_nm FROM TSMMAJOR WHERE major_cd = @p_major_cd",
            new Dictionary<string, string?> { ["p_major_cd"] = majorCd });
        var majorNm = majorRows.Tables.Count > 0 && majorRows.Tables[0].Rows.Count > 0
            ? Convert.ToString(majorRows.Tables[0].Rows[0].GetValueOrDefault("major_nm")) ?? lookupKey
            : lookupKey;

        // 같은 이름의 LookUp이 이미 있으면 지운다 - SSP_SYS_LOOKUP_S의 D 분기는 sysLookupC/P/M을
        // 순서대로 지우고, 대상이 없어도(0건) 에러 없이 그냥 넘어간다.
        await _dataRepo.SaveAsync("SSP_SYS_LOOKUP_S",
            new Dictionary<string, string?> { ["p_work_type"] = "D", ["p_lookup_key"] = lookupKey },
            CurrentUserId, ClientPc);

        var queryTxt =
            "SELECT   minor_cd, \r\n" +
            "            minor_nm \r\n" +
            "FROM TSMMINOR\r\n" +
            $"WHERE major_cd= '{majorCd}'\r\n" +
            "AND     use_yn = 'Y'\r\n" +
            "Order by sort, minor_nm";

        var createResult = await _dataRepo.SaveAsync("SSP_SYS_LOOKUP_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = "N",
            ["p_lookup_key"] = lookupKey,
            ["p_source_type"] = "Q",
            ["p_query_txt"] = queryTxt,
            ["p_lookup_nm"] = majorNm,
            ["p_value_field"] = "minor_cd",
            ["p_display_field"] = "minor_nm",
            ["p_use_yn"] = "Y"
        }, CurrentUserId, ClientPc);

        if (!createResult.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = createResult.FailMessage, ErrorCode = createResult.ErrorCode });

        // L_BA0001과 같은 컬럼 구성(코드/코드명 2열, 코드는 폭 0=숨김) - 실패해도 LookUp 자체는
        // 이미 만들어졌으니 조용히 넘어간다(컬럼 구성이 없으면 값/표시필드 2컬럼 고정으로 그려질
        // 뿐이라 화면이 못 쓰게 되지는 않는다, LookUpEditWyn.LoadFromLookupKeyAsync 참고).
        await _dataRepo.SaveAsync("SSP_SYS_LOOKUP_S_2", new Dictionary<string, string?>
        {
            ["p_work_type"] = "N",
            ["p_lookup_key"] = lookupKey,
            ["p_column_nm"] = "minor_cd",
            ["p_caption"] = "코드",
            ["p_sort"] = "1",
            ["p_width"] = "0"
        }, CurrentUserId, ClientPc);
        await _dataRepo.SaveAsync("SSP_SYS_LOOKUP_S_2", new Dictionary<string, string?>
        {
            ["p_work_type"] = "N",
            ["p_lookup_key"] = lookupKey,
            ["p_column_nm"] = "minor_nm",
            ["p_caption"] = "코드명",
            ["p_sort"] = "2",
            ["p_width"] = "100"
        }, CurrentUserId, ClientPc);

        return Ok(new ApiResult { Success = true, Message = $"LookUp [{lookupKey}]이(가) 생성되었습니다.", GeneratedCode = lookupKey });
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    private string? ClientPc => ClientPcInfo.Build(HttpContext);
}
