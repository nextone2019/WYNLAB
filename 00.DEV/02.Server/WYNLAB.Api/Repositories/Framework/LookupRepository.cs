using Dapper;
using WYNLAB.Api.Data;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Repositories.Framework;

public interface ILookupRepository
{
    Task<List<LookupItemDto>?> GetItemsAsync(string lookupKey, Dictionary<string, string?> paramValues);
}

/// <summary>
/// LookUp(콤보) 프레임워크 런타임 조회 - sysLookupM에서 lookup_key로 proc_nm/value_field/display_field를
/// 찾은 뒤, 그 프로시져를 실행하고(GenericDataRepository 재사용 - 파라미터 개수에 대한 가정이
/// 없어서 그대로 쓸 수 있다) 결과 행에서 value_field/display_field 두 컬럼만 뽑아 돌려준다.
/// 관리 화면(frmSysLookup)의 sysLookupM/P 자체 CRUD는 새 Repository 없이 기존 범용 데이터 통로
/// (api/data/*)를 그대로 쓴다 - USP_SYS_LOOKUP_Q/_S가 다른 화면들과 똑같은 방식으로 동작하기 때문.
/// </summary>
public class LookupRepository : ILookupRepository
{
    private readonly IDapperContext _context;
    private readonly IGenericDataRepository _dataRepo;

    public LookupRepository(IDapperContext context, IGenericDataRepository dataRepo)
    {
        _context = context;
        _dataRepo = dataRepo;
    }

    public async Task<List<LookupItemDto>?> GetItemsAsync(string lookupKey, Dictionary<string, string?> paramValues)
    {
        using var conn = _context.CreateConnection();

        var def = await conn.QueryFirstOrDefaultAsync<(string proc_nm, string value_field, string display_field)>(
            "SELECT proc_nm, value_field, display_field FROM sysLookupM WHERE lookup_key = @lookupKey AND use_yn = 'Y'",
            new { lookupKey });

        if (def.proc_nm == null) return null;

        var result = await _dataRepo.QueryAsync(def.proc_nm, paramValues);
        if (result.Tables.Count == 0) return new List<LookupItemDto>();

        return result.Tables[0].Rows.Select(row => new LookupItemDto
        {
            Value = Convert.ToString(row.GetValueOrDefault(def.value_field)) ?? string.Empty,
            Display = Convert.ToString(row.GetValueOrDefault(def.display_field)) ?? string.Empty
        }).ToList();
    }
}
