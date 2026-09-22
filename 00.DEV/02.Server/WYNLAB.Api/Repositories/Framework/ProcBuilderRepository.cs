using Dapper;
using WYNLAB.Api.Data;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Repositories.Framework;

public interface IProcBuilderRepository
{
    /// <summary>테이블 하나의 컬럼 구조(이름/타입/길이/NULL여부/IDENTITY여부/PK여부)를 읽어온다 -
    /// ScreenBuilderRepository(프로시저를 실행해서 결과셋을 본다)와 달리 여기는 테이블 자체를
    /// INFORMATION_SCHEMA/sys.columns로 직접 읽는다(아무것도 실행하지 않음).</summary>
    Task<DescribeTableResultDto> DescribeTableAsync(string tableName);

    /// <summary>frmProcBuilder가 만든 CREATE OR ALTER PROCEDURE 텍스트를 그대로 실행한다 -
    /// "GO"로 나뉜 배치를 하나씩 순서대로 실행(2026-09-14 추가). 호출 전 검증(CREATE [OR ALTER]
    /// PROCEDURE로 시작하는지)은 컨트롤러 책임 - 여기는 이미 검증된 텍스트만 받는다고 가정한다.</summary>
    Task ExecuteSqlAsync(string sql);
}

public class ProcBuilderRepository : IProcBuilderRepository
{
    private readonly IDapperContext _context;

    public ProcBuilderRepository(IDapperContext context) => _context = context;

    public async Task<DescribeTableResultDto> DescribeTableAsync(string tableName)
    {
        using var conn = _context.CreateConnection();

        // PK 컬럼 이름 목록을 먼저 따로 읽는다 - INFORMATION_SCHEMA만으로는 컬럼 하나의 조회
        // 결과에 "이게 PK인지"를 바로 못 붙이므로(별도 조인이 필요), 여기서 미리 집합으로
        // 만들어두고 아래 컬럼 목록을 돌면서 있는지만 확인한다(137_AutoKey_Procs.sql 만들 때
        // 실제로 이 방식으로 손으로 확인했던 것과 같은 조회).
        var pkColumns = (await conn.QueryAsync<string>(
            @"SELECT kcu.COLUMN_NAME
              FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
              JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu ON tc.CONSTRAINT_NAME = kcu.CONSTRAINT_NAME
              WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY' AND tc.TABLE_NAME = @tableName",
            new { tableName })).ToHashSet(StringComparer.OrdinalIgnoreCase);

        var columns = (await conn.QueryAsync<TableColumnInfoDto>(
            @"SELECT
                  c.name AS ColumnNm,
                  t.name AS SqlType,
                  CASE WHEN t.name IN ('nvarchar', 'nchar') AND c.max_length > 0 THEN c.max_length / 2 ELSE c.max_length END AS MaxLength,
                  c.is_nullable AS IsNullable,
                  c.is_identity AS IsIdentity
              FROM sys.columns c
              JOIN sys.types t ON t.user_type_id = c.user_type_id
              WHERE c.object_id = OBJECT_ID(@tableName)
              ORDER BY c.column_id",
            new { tableName })).ToList();

        foreach (var col in columns)
            col.IsPrimaryKey = pkColumns.Contains(col.ColumnNm);

        return new DescribeTableResultDto { Columns = columns };
    }

    public async Task ExecuteSqlAsync(string sql)
    {
        using var conn = _context.CreateConnection();

        // 마이그레이션 파일을 sqlcmd로 적용할 때와 같은 방식 - "GO"만 있는 줄로 배치를 나눠서
        // 순서대로 실행한다(한 커넥션에서 여러 CREATE OR ALTER PROCEDURE를 한 배치로 보내면
        // "CREATE PROCEDURE는 배치의 첫 문장이어야 한다" 오류가 난다).
        var batches = sql.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
            .Aggregate(new List<string> { "" }, (batches, line) =>
            {
                if (line.Trim().Equals("GO", StringComparison.OrdinalIgnoreCase))
                    batches.Add("");
                else
                    batches[^1] += line + "\n";
                return batches;
            })
            .Select(b => b.Trim())
            .Where(b => b.Length > 0);

        foreach (var batch in batches)
            await conn.ExecuteAsync(batch);
    }
}
