using System.Data;
using Dapper;
using WYNLAB.Api.Data;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Repositories.Framework;

public interface IPopupLookupRepository
{
    Task<PopupDefinitionDto?> GetDefinitionAsync(string popupKey);

    Task<DescribeProcResultDto> DescribeProcAsync(string procName);
}

/// <summary>
/// sysPopUpM/sysPopUpD(팝업 정의/컬럼 설정) 조회 + "컬럼생성" 버튼이 쓰는 프로시저 결과셋 구조
/// 읽기. 이 데이터는 업무 데이터가 아니라 화면들이 팝업을 어떻게 그릴지 정의하는 프레임워크
/// 메타데이터라 sysPopUpM/D(대문자 아님)에 둔다 - 자세한 설계는 프로젝트 메모리 참고.
/// </summary>
public class PopupLookupRepository : IPopupLookupRepository
{
    private readonly IDapperContext _context;

    public PopupLookupRepository(IDapperContext context) => _context = context;

    public async Task<PopupDefinitionDto?> GetDefinitionAsync(string popupKey)
    {
        using var conn = _context.CreateConnection();

        var master = await conn.QueryFirstOrDefaultAsync<PopupDefinitionDto>(
            @"SELECT popup_key AS PopupKey, proc_nm AS ProcNm, popup_nm AS PopupNm,
                     CAST(CASE WHEN hierarchical_yn = 'Y' THEN 1 ELSE 0 END AS BIT) AS HierarchicalYn,
                     key_field AS KeyField, parent_field AS ParentField, display_field AS DisplayField,
                     popup_width AS PopupWidth, popup_height AS PopupHeight
              FROM sysPopUpM
              WHERE popup_key = @popupKey AND use_yn = 'Y'",
            new { popupKey });

        if (master == null) return null;

        var columns = await conn.QueryAsync<PopupColumnDto>(
            @"SELECT column_nm AS ColumnNm, ISNULL(caption, column_nm) AS Caption,
                     control_type AS ControlType, lookup_proc_nm AS LookupProcNm,
                     sort AS Sort, width AS Width,
                     CAST(CASE WHEN visible_yn = 'Y' THEN 1 ELSE 0 END AS BIT) AS VisibleYn
              FROM sysPopUpD
              WHERE popup_key = @popupKey
              ORDER BY sort",
            new { popupKey });

        master.Columns = columns.ToList();

        var searchFields = await conn.QueryAsync<PopupSearchFieldDto>(
            @"SELECT param_nm AS ParamNm, ISNULL(caption, param_nm) AS Caption,
                     control_type AS ControlType, sort AS Sort, width AS Width
              FROM sysPopUpS
              WHERE popup_key = @popupKey
              ORDER BY sort",
            new { popupKey });
        master.SearchFields = searchFields.ToList();

        return master;
    }

    /// <summary>"컬럼생성" 버튼 - 프로시저를 실제로 실행하지 않고 (1) 입력 파라미터(조회조건),
    /// (2) 결과셋 컬럼 구조를 한 번에 읽는다. 예전에는 모든 SSP_POP_*_Q가 "@p_keyword" 하나만
    /// 받는다고 가정하고 "EXEC dbo.proc @p_keyword = NULL"을 고정 문자열로 썼는데, 프로시저마다
    /// 파라미터명/개수가 전부 달라지면서(사장님 결정 - 통일된 규칙 없음) 그 가정이 깨졌다.
    /// 그래서 먼저 sys.parameters로 실제 파라미터 목록을 읽고, 그걸로 EXEC 문을 동적으로
    /// 만들어(전부 NULL로) 결과셋 구조를 읽는다 - 파라미터 이름/개수가 뭐든 상관없이 동작한다.</summary>
    public async Task<DescribeProcResultDto> DescribeProcAsync(string procName)
    {
        using var conn = _context.CreateConnection();

        var paramRows = await conn.QueryAsync<(string name, string type_name)>(
            @"SELECT p.name, TYPE_NAME(p.user_type_id) AS type_name
              FROM sys.parameters p
              JOIN sys.objects o ON o.object_id = p.object_id
              WHERE o.name = @procName AND p.is_output = 0
              ORDER BY p.parameter_id",
            new { procName });

        var paramList = paramRows.ToList();

        var paramDtos = paramList.Select(p => new ProcParamInfoDto
        {
            ParamNm = p.name.TrimStart('@'),
            SqlType = p.type_name,
            SuggestedControlType = p.type_name.StartsWith("date", StringComparison.OrdinalIgnoreCase) ? "DATE" : "TEXT"
        }).ToList();

        var execArgs = string.Join(", ", paramList.Select(p => $"{p.name} = NULL"));
        var stmt = $"EXEC dbo.{procName}" + (execArgs.Length > 0 ? $" {execArgs}" : string.Empty);

        var columnRows = await conn.QueryAsync<(string name, string system_type_name)>(
            "SELECT name, system_type_name FROM sys.dm_exec_describe_first_result_set(@stmt, NULL, 0)",
            new { stmt });

        var columnDtos = columnRows.Select(r => new ProcColumnInfoDto
        {
            ColumnNm = r.name,
            SqlType = r.system_type_name,
            SuggestedControlType = r.system_type_name.StartsWith("date", StringComparison.OrdinalIgnoreCase)
                ? "DATE" : "TEXT"
        }).ToList();

        return new DescribeProcResultDto { Columns = columnDtos, Params = paramDtos };
    }
}
