using Dapper;
using WYNLAB.Api.Data;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Repositories.Framework;

public interface ILookupRepository
{
    Task<LookupItemsResultDto?> GetItemsAsync(string lookupKey, Dictionary<string, string?> paramValues);
}

/// <summary>
/// LookUp(콤보) 프레임워크 런타임 조회 - sysLookupM에서 lookup_key로 정의를 찾은 뒤, 그 소스를
/// 실행하고(GenericDataRepository 재사용 - 파라미터 개수에 대한 가정이 없어서 그대로 쓸 수 있다)
/// 결과 행에서 value_field/display_field 두 컬럼(Value/Display, 항상 채움)과 전체 컬럼(Row)을
/// 같이 돌려준다. source_type='P'(기존 방식, 기본값)면 proc_nm을 프로시저로 실행하고, 'Q'면
/// query_txt를 SQL 텍스트 그대로 실행한다 - "프로시저를 새로 안 만들고 간단한 조회는 쿼리 한
/// 줄로 바로 LookUp을 만들고 싶다"는 요청으로 추가(2026-09-02, 064_Lookup_Query_Source.sql).
///
/// sysLookupC(컬럼 구성)도 같이 조회해서 응답에 얹는다 - 비어있으면(설정 안 한 LookUp, 기존
/// 전부 해당) 클라이언트가 예전 그대로 값필드/표시필드 2컬럼 고정으로 그린다(2026-09-03,
/// 065_Lookup_Columns.sql - 팝업 프레임워크(sysPopUpD)처럼 조회된 컬럼 중 원하는 만큼 골라서
/// 폭까지 지정할 수 있게 확장).
///
/// 관리 화면(frmSysLookup)의 sysLookupM/P/C 자체 CRUD는 새 Repository 없이 기존 범용 데이터
/// 통로(api/data/*)를 그대로 쓴다 - USP_SYS_LOOKUP_Q/_S가 다른 화면들과 똑같은 방식으로 동작하기 때문.
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

    public async Task<LookupItemsResultDto?> GetItemsAsync(string lookupKey, Dictionary<string, string?> paramValues)
    {
        using var conn = _context.CreateConnection();

        // value_field/display_field는 DB에서 NOT NULL이라, "행 자체가 없음"을 구분하는 용도로
        // 안전하게 쓸 수 있다(proc_nm은 source_type='Q'일 때 정상적으로 NULL일 수 있어서 더 이상
        // 이 용도로 못 쓴다).
        var def = await conn.QueryFirstOrDefaultAsync<(string source_type, string? proc_nm, string? query_txt, string value_field, string display_field)>(
            "SELECT source_type, proc_nm, query_txt, value_field, display_field FROM sysLookupM WHERE lookup_key = @lookupKey AND use_yn = 'Y'",
            new { lookupKey });

        if (def.value_field == null) return null;

        var columns = (await conn.QueryAsync<(string column_nm, string? caption, int sort, int width)>(
            "SELECT column_nm, caption, sort, width FROM sysLookupC WHERE lookup_key = @lookupKey AND visible_yn = 'Y' ORDER BY sort",
            new { lookupKey }))
            .Select(c => new LookupColumnDefDto { ColumnNm = c.column_nm, Caption = c.caption, Sort = c.sort, Width = c.width })
            .ToList();

        DataQueryResponse result;
        if (def.source_type == "Q")
        {
            if (string.IsNullOrWhiteSpace(def.query_txt)) return new LookupItemsResultDto { Columns = columns };
            result = await _dataRepo.QueryRawAsync(def.query_txt, paramValues);
        }
        else
        {
            if (string.IsNullOrWhiteSpace(def.proc_nm)) return new LookupItemsResultDto { Columns = columns };
            result = await _dataRepo.QueryAsync(def.proc_nm, paramValues);
        }

        if (result.Tables.Count == 0) return new LookupItemsResultDto { Columns = columns };

        var items = result.Tables[0].Rows.Select(row => new LookupItemDto
        {
            Value = Convert.ToString(row.GetValueOrDefault(def.value_field)) ?? string.Empty,
            Display = Convert.ToString(row.GetValueOrDefault(def.display_field)) ?? string.Empty,
            Row = row.ToDictionary(kv => kv.Key, kv => (string?)Convert.ToString(kv.Value))
        }).ToList();

        return new LookupItemsResultDto { Columns = columns, Items = items };
    }
}
