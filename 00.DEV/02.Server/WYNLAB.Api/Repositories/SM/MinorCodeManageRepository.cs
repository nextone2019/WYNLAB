using System.Data;
using Dapper;
using WYNLAB.Api.Data;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Repositories.SM;

public interface IMinorCodeManageRepository
{
    Task<MinorCodeQueryResponse> GetAsync(string? majorCd, string? majorNm, string? selectedMajorCd);
    Task<List<MinorItemDto>> GetMinorsAsync(string majorCd);
    Task<ProcResult> SaveMajorAsync(string workType, MajorSaveRequest request, string userId, string? clientPc);
    Task<ProcResult> SaveMinorsAsync(string majorCd, List<MinorItemDto> items, string userId, string? clientPc);
    Task<List<CodeLookupItemDto>> GetLookupAsync(string where);
}

/// <summary>
/// 데이터 처리는 전부 저장프로시저(USP_SM_MINORCODE_*)로 위임한다.
/// 명명규칙: USP_SM_MINORCODE_Q(대분류+소분류 조회) / _S(대분류 저장) / _S_1(소분류 그리드 전체 치환 저장)
/// TSMMAJOR/TSMMINOR 컬럼명이 이미 소문자라, Dapper는 대소문자 구분 없이 DTO 프로퍼티에
/// 그대로 자동매핑되므로 별도 Row 타입 없이 공유 DTO(MajorListItemDto/MinorItemDto)를 바로 사용한다.
/// </summary>
public class MinorCodeManageRepository : IMinorCodeManageRepository
{
    private readonly IDapperContext _context;

    public MinorCodeManageRepository(IDapperContext context) => _context = context;

    public async Task<MinorCodeQueryResponse> GetAsync(string? majorCd, string? majorNm, string? selectedMajorCd)
    {
        using var conn = _context.CreateConnection();

        var majors = (await conn.QueryAsync<MajorListItemDto>("USP_SM_MINORCODE_Q",
            new
            {
                p_work_type = "Q",
                p_major_cd = string.IsNullOrWhiteSpace(majorCd) ? null : majorCd,
                p_major_nm = string.IsNullOrWhiteSpace(majorNm) ? null : majorNm
            },
            commandType: CommandType.StoredProcedure)).ToList();

        var minors = string.IsNullOrWhiteSpace(selectedMajorCd)
            ? new List<MinorItemDto>()
            : await GetMinorsAsync(selectedMajorCd, conn);

        return new MinorCodeQueryResponse { Majors = majors, Minors = minors };
    }

    /// <summary>대분류 그리드(grd1)에서 포커스 행만 바뀌었을 때 쓰는 가벼운 조회 - 대분류
    /// 목록은 그대로 두고 소분류(grd2)만 새로 받아온다. GetAsync처럼 대분류까지 같이
    /// 돌려주면 화면에서 grd1.DataSource를 매번 재할당하게 되어, 행을 클릭할 때마다
    /// grd1 자체도 리프레시되는 것처럼 보이는 문제가 있었다(실제로 겪음).</summary>
    public async Task<List<MinorItemDto>> GetMinorsAsync(string majorCd)
    {
        using var conn = _context.CreateConnection();
        return await GetMinorsAsync(majorCd, conn);
    }

    private static async Task<List<MinorItemDto>> GetMinorsAsync(string majorCd, IDbConnection conn)
    {
        var minors = await conn.QueryAsync<MinorItemDto>("USP_SM_MINORCODE_Q",
            new { p_work_type = "Q1", p_major_cd = majorCd },
            commandType: CommandType.StoredProcedure);
        return minors.ToList();
    }

    public async Task<ProcResult> SaveMajorAsync(string workType, MajorSaveRequest request, string userId, string? clientPc)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", workType);
        p.Add("p_major_cd", request.major_cd);
        p.Add("p_major_nm", request.major_nm);
        p.Add("p_sys_yn", request.sys_yn ? "Y" : "N");
        for (var i = 1; i <= 10; i++)
        {
            p.Add($"p_rel_cd{i}", GetProp(request, $"rel_cd{i}"));
            p.Add($"p_rel_title{i}", GetProp(request, $"rel_title{i}"));
            p.Add($"p_rel_cd_type{i}", GetProp(request, $"rel_cd_type{i}"));
        }
        p.Add("p_remark", request.remark);
        p.Add("p_user_id", userId);
        p.Add("p_client_pc", clientPc);
        p.AddStandardOutputs(withGeneratedCode: true, pascalCase: true);

        await conn.ExecuteAsync("USP_SM_MINORCODE_S", p, commandType: CommandType.StoredProcedure);

        return p.ReadStandardOutputs(withGeneratedCode: true, pascalCase: true);
    }

    public async Task<ProcResult> SaveMinorsAsync(string majorCd, List<MinorItemDto> items, string userId, string? clientPc)
    {
        /* 입력된 MinorCd ToUpper()*/
        foreach (var item in items)
            item.minor_cd = item.minor_cd?.ToUpper() ?? string.Empty;

        var itemsJson = System.Text.Json.JsonSerializer.Serialize(items);

        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("major_cd", majorCd);
        p.Add("items_json", itemsJson);
        p.Add("user_id", userId);
        p.Add("client_pc", clientPc);
        p.AddStandardOutputs();

        await conn.ExecuteAsync("USP_SM_MINORCODE_S_1", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs();
    }

    /// <summary>콤보/LookUp 전용 프로시저(SSP_CBO_*) 호출 - @p_major_code는 SQL 텍스트가 아니라
    /// 진짜 파라미터 값 하나(대분류코드)다. 프로시져 자체의 WHERE 절은 고정 SQL이라
    /// 클라이언트가 이 값에 뭘 넣어도 쿼리 구조는 못 바꾼다(인젝션 불가).
    ///
    /// 파라미터 이름은 04.Database\010_SspCboCodeQ_StandardizeParam.sql과 반드시 같이 맞춰야
    /// 한다 - 로컬DB는 @p_code, 운영DB는 @p_where로 서로 다르게 올라가 있어서 실제로 500
    /// 오류가 났었다(SqlException: 매개 변수 '@p_where'이(가) 필요하지만 제공되지 않았습니다).</summary>
    public async Task<List<CodeLookupItemDto>> GetLookupAsync(string where)
    {
        using var conn = _context.CreateConnection();
        var items = await conn.QueryAsync<CodeLookupItemDto>("SSP_CBO_CODE_Q",
            new { p_major_cd = where }, commandType: CommandType.StoredProcedure);
        return items.ToList();
    }

    /// <summary>RelCd1~10/RelTitle1~10/RelCdType1~10처럼 번호가 붙는 반복 필드를 이름으로 꺼낸다 -
    /// 30개 필드를 각각 손으로 다 나열하는 대신 이 헬퍼로 루프를 돌린다.</summary>
    private static string? GetProp(MajorSaveRequest request, string propName) =>
        (string?)typeof(MajorSaveRequest).GetProperty(propName)!.GetValue(request);
}
