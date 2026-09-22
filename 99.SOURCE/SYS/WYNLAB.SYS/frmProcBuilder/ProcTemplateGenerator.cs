using System.Text;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.SYS;

/// <summary>화면(frmProcBuilder)이 받은 입력을 이 세션에서 손으로 몇 번(USP_SM_MINORCODE_Q/S 스타일
/// 확인 → USP_SM_AUTOKEY_Q/S 작성 → 검증) 반복한 것과 똑같은 틀로 조립하는 순수 문자열 생성기.
/// ScreenTemplateGenerator(AI Builder)와 입출력 모양이 완전히 달라(화면 코드 vs SQL 텍스트)
/// 별도 클래스로 뒀다 - 다만 "다음 마이그레이션 번호 찾기"는 같은 계산을 그대로 다시 구현한다
/// (00.DEV/04.Database/*.sql 스캔, 001~999 3자리 접두어 중 최댓값+1).
///
/// 2026-09-14 재설계 - 예전엔 "마스터전용/마스터+상세" 두 패턴을 골라 Q+S를 항상 같이 만들고
/// 가져온 컬럼을 전부 썼는데, 실제로 써보니 두 가지가 아쉬웠다: (1) Q만 필요한데 S도 강제로
/// 같이 생기고 (2) 컬럼을 골라서 쓸 수가 없었다. 그래서 Q/S를 따로 하나씩 만들고, 컬럼도
/// 체크해서 고르는 방식으로 바꿨다 - 상세 테이블 조회(Q_1)는 여전히 지원한다(사용자 확인).
/// 여러 테이블에 걸쳐 상태를 바꾸는 승인(USP_AP_APPR)류 복잡한 케이스는 여전히 자동생성
/// 대상이 아니다(2026-09-13 확인 - "지금보다 더 복잡한 건 필요 없다").</summary>
public enum ProcType
{
    Query,
    Save
}

/// <summary>테이블 컬럼 하나 + 이 화면에서 체크한 용도 플래그. 컬럼 자체는 DescribeTableAsync가
/// 돌려준 걸 그대로 들고 있고(타입/PK/IDENTITY 판별에 계속 필요), 나머지 3개는 그리드의
/// 체크박스가 채운다 - IsWhereFilter는 Query에서만, IncludeInsert/IncludeUpdate는 Save에서만
/// 의미가 있다(다른 모드일 땐 그냥 무시됨 - ProcType별로 그리드 컬럼을 숨기지 않고 항상 다
/// 보여주는 대신, 생성 로직에서만 해당 모드의 플래그를 읽는다).</summary>
public class SelectedColumn
{
    public TableColumnInfoDto Column { get; set; } = null!;
    public bool IsSelected { get; set; }
    public bool IsWhereFilter { get; set; }
    public bool IncludeInsert { get; set; } = true;
    public bool IncludeUpdate { get; set; } = true;

    // 그리드 바인딩용 - DevExpress GridControl의 자동 FieldName 매핑은 최상위 프로퍼티만
    // 안전하게 지원해서(Column.ColumnNm처럼 점 표기로 중첩 프로퍼티를 묻는 건 리스트
    // DataSource에서 항상 되는 게 아니다), Column 안의 값을 여기서 그대로 통과시켜 보여준다.
    public string ColumnNm => Column.ColumnNm;
    public string SqlType => Column.SqlType;
    public bool IsPrimaryKey => Column.IsPrimaryKey;
}

public class ProcGenSpec
{
    /// <summary>최종 프로시저 이름 전체(예: "USP_SM_AUTOKEY_Q") - 화면이 모듈+기능명으로
    /// 자동 제안하지만, 이 값 자체는 사용자가 그대로 고쳐 쓸 수 있는 자유 텍스트다.</summary>
    public string ProcName { get; set; } = string.Empty;

    public ProcType Type { get; set; } = ProcType.Query;

    public string MasterTable { get; set; } = string.Empty;

    /// <summary>DescribeTableAsync가 돌려준 마스터 테이블 컬럼 전체(체크 여부와 무관) - PK 판별에
    /// 쓴다. 실제로 SQL에 들어가는 컬럼은 이 중 IsSelected(Query)/그 외 플래그(Save)가 켜진
    /// 것만이다.</summary>
    public List<SelectedColumn> MasterColumns { get; set; } = new();

    /// <summary>상세 테이블 조회(Query에서만, Q_1 분기) - 지정 안 하면 Q_1 자체가 안 생긴다.</summary>
    public string? DetailTable { get; set; }
    public List<SelectedColumn> DetailColumns { get; set; } = new();

    /// <summary>상세 테이블에서 마스터 PK 값을 들고 있는 컬럼(FK) - Q_1이 이 컬럼으로 필터한다.</summary>
    public string? DetailLinkColumn { get; set; }
}

public static class ProcTemplateGenerator
{
    private static readonly string[] AuditColumnNames =
    {
        "reg_user_id", "reg_dt", "reg_pc", "upt_user_id", "upt_dt", "upt_pc"
    };

    /// <summary>스펙을 검증하고 프로시저(Query 또는 Save 하나) SQL 텍스트를 만든다. 검증 실패 시
    /// ArgumentException을 던진다(메시지를 화면이 그대로 AppMessageBox에 보여줌).</summary>
    public static string Generate(ProcGenSpec spec)
    {
        if (string.IsNullOrWhiteSpace(spec.ProcName)) throw new ArgumentException("프로시저 이름을 입력해주세요.");
        if (string.IsNullOrWhiteSpace(spec.MasterTable)) throw new ArgumentException("마스터 테이블명을 입력해주세요.");
        if (spec.MasterColumns.Count == 0) throw new ArgumentException("마스터 테이블 컬럼을 먼저 가져와주세요.");

        var pk = spec.MasterColumns.Select(c => c.Column).FirstOrDefault(c => c.IsPrimaryKey);
        if (pk == null) throw new ArgumentException($"'{spec.MasterTable}' 테이블에 기본 키(PK)가 없습니다 - 이 생성기는 PK가 있는 테이블만 지원합니다.");

        if (!string.IsNullOrWhiteSpace(spec.DetailTable))
        {
            if (spec.DetailColumns.Count == 0) throw new ArgumentException("상세 테이블 컬럼을 먼저 가져와주세요.");
            if (string.IsNullOrWhiteSpace(spec.DetailLinkColumn)) throw new ArgumentException("상세 테이블의 연결 컬럼(마스터 키를 담는 컬럼)을 지정해주세요.");
            if (!spec.DetailColumns.Any(c => string.Equals(c.Column.ColumnNm, spec.DetailLinkColumn, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException($"연결 컬럼 '{spec.DetailLinkColumn}'이(가) '{spec.DetailTable}' 테이블에 없습니다.");
        }

        return spec.Type == ProcType.Query ? GenerateQueryProc(spec, pk) : GenerateSaveProc(spec, pk);
    }

    private static string GenerateQueryProc(ProcGenSpec spec, TableColumnInfoDto pk)
    {
        var pkParam = ParamName(pk.ColumnNm);
        var hasDetail = !string.IsNullOrWhiteSpace(spec.DetailTable);

        var selectCols = spec.MasterColumns.Where(c => c.IsSelected).ToList();
        if (selectCols.Count == 0) throw new ArgumentException("조회할 컬럼을 하나 이상 선택해주세요.");

        var whereCols = selectCols.Where(c => c.IsWhereFilter).ToList();

        var sb = new StringBuilder();
        sb.AppendLine($"CREATE OR ALTER PROCEDURE {spec.ProcName}");
        sb.AppendLine("    @p_work_type VARCHAR(50),");
        sb.AppendLine("    ---------------------------------------------------------------------------------------------------");
        foreach (var c in whereCols)
            sb.AppendLine($"    @{ParamName(c.Column.ColumnNm)} {FormatSqlType(c.Column)} = NULL,");
        if (hasDetail)
            sb.AppendLine($"    @{pkParam} {FormatSqlType(pk)} = NULL,\t\t/* WORK_TYPE = 'Q_1'일 때 상세 조회용 부모 키 */");
        sb.AppendLine("    ---------------------------------------------------------------------------------------------------");
        AppendStandardOutputParams(sb);
        sb.AppendLine("AS");
        sb.AppendLine("BEGIN");
        sb.AppendLine("    SET NOCOUNT ON;");
        sb.AppendLine("    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;");
        sb.AppendLine();
        sb.AppendLine("    BEGIN TRY");
        sb.AppendLine("        IF @p_work_type = 'Q'");
        sb.AppendLine("        BEGIN");
        sb.AppendLine("            SELECT");
        sb.AppendLine("                " + string.Join(", ", selectCols.Select(c => c.Column.ColumnNm)));
        sb.AppendLine($"            FROM {spec.MasterTable}");
        if (whereCols.Count > 0)
        {
            sb.AppendLine("            WHERE " + string.Join("\n                AND ", whereCols.Select(c =>
                $"(@{ParamName(c.Column.ColumnNm)} IS NULL OR {c.Column.ColumnNm} = @{ParamName(c.Column.ColumnNm)})")));
        }
        sb.AppendLine($"            ORDER BY {pk.ColumnNm};");
        sb.AppendLine("        END");

        if (hasDetail)
        {
            var detailSelectCols = spec.DetailColumns.Where(c => c.IsSelected).ToList();
            if (detailSelectCols.Count == 0) throw new ArgumentException("상세 테이블에서 조회할 컬럼을 하나 이상 선택해주세요.");

            sb.AppendLine("        ELSE IF @p_work_type = 'Q_1'");
            sb.AppendLine("        BEGIN");
            sb.AppendLine("            SELECT");
            sb.AppendLine("                " + string.Join(", ", detailSelectCols.Select(c => c.Column.ColumnNm)));
            sb.AppendLine($"            FROM {spec.DetailTable}");

            var detailPk = spec.DetailColumns.Select(c => c.Column).Where(c => c.IsPrimaryKey).Select(c => c.ColumnNm).ToList();
            if (detailPk.Count > 0)
            {
                sb.AppendLine($"            WHERE {spec.DetailLinkColumn} = @{pkParam}");
                sb.AppendLine($"            ORDER BY {string.Join(", ", detailPk)};");
            }
            else
            {
                sb.AppendLine($"            WHERE {spec.DetailLinkColumn} = @{pkParam};");
            }

            sb.AppendLine("        END");
        }

        sb.AppendLine("    END TRY");
        AppendStandardCatch(sb);
        sb.AppendLine("END");
        return sb.ToString();
    }

    private static string GenerateSaveProc(ProcGenSpec spec, TableColumnInfoDto pk)
    {
        var editable = spec.MasterColumns
            .Where(c => !c.Column.IsPrimaryKey && !IsAuditColumn(c.Column.ColumnNm) && (c.IncludeInsert || c.IncludeUpdate))
            .ToList();
        if (editable.Count == 0) throw new ArgumentException("Insert 또는 Update에 포함할 컬럼을 하나 이상 선택해주세요.");

        var pkParam = ParamName(pk.ColumnNm);
        var hasAuditColumn = (string name) => spec.MasterColumns.Any(c => string.Equals(c.Column.ColumnNm, name, StringComparison.OrdinalIgnoreCase));

        var sb = new StringBuilder();
        sb.AppendLine($"CREATE OR ALTER PROCEDURE {spec.ProcName}");
        sb.AppendLine("    @p_work_type VARCHAR(50),");
        sb.AppendLine("    ---------------------------------------------------------------------------------------------------");

        // PK 파라미터 - IDENTITY면 N일 때 자동 채번되므로 기본값 NULL(선택), 아니면 항상 필요.
        var pkDefault = pk.IsIdentity ? " = NULL" : "";
        var pkComment = pk.IsIdentity ? "\t\t-- U일 때 필수(변경 불가) - N일 때는 IDENTITY가 자동 채번" : "";
        sb.AppendLine($"    @{pkParam} {FormatSqlType(pk)}{pkDefault},{pkComment}");

        foreach (var c in editable)
        {
            // NOT NULL이고 그럴듯한 기본값도 없는 컬럼만 필수 파라미터로 남긴다(USP_SM_MENU_S의
            // @p_menu_nm처럼) - 그 외엔 전부 선택 입력(기존 관례상 대부분의 컬럼이 이쪽).
            var defaultText = (!c.Column.IsNullable && !HasSensibleDefault(c.Column)) ? "" : " = NULL";
            sb.AppendLine($"    @{ParamName(c.Column.ColumnNm)} {FormatSqlType(c.Column)}{defaultText},");
        }

        sb.AppendLine("    @p_user_id VARCHAR(50),");
        sb.AppendLine("    @p_client_pc NVARCHAR(200) = NULL,");
        sb.AppendLine("    ---------------------------------------------------------------------------------------------------");
        AppendStandardOutputParams(sb);
        sb.AppendLine("AS");
        sb.AppendLine("BEGIN");
        sb.AppendLine("    SET NOCOUNT ON;");
        sb.AppendLine("    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;");
        sb.AppendLine();
        sb.AppendLine("    BEGIN TRY");

        // ---- N (insert) ----
        var insertable = editable.Where(c => c.IncludeInsert).ToList();
        sb.AppendLine("        IF @p_work_type = 'N'");
        sb.AppendLine("        BEGIN");
        var insertCols = new List<string>();
        if (!pk.IsIdentity) insertCols.Add(pk.ColumnNm);
        insertCols.AddRange(insertable.Select(c => c.Column.ColumnNm));
        insertCols.AddRange(new[] { "reg_user_id", "reg_dt", "reg_pc" }.Where(hasAuditColumn));

        var insertVals = new List<string>();
        if (!pk.IsIdentity) insertVals.Add($"@{pkParam}");
        insertVals.AddRange(insertable.Select(c => $"@{ParamName(c.Column.ColumnNm)}"));
        if (hasAuditColumn("reg_user_id")) insertVals.Add("@p_user_id");
        if (hasAuditColumn("reg_dt")) insertVals.Add("GETDATE()");
        if (hasAuditColumn("reg_pc")) insertVals.Add("@p_client_pc");

        sb.AppendLine($"            INSERT INTO {spec.MasterTable} (");
        sb.AppendLine("                " + string.Join(", ", insertCols));
        sb.AppendLine("            )");
        sb.AppendLine("            VALUES (");
        sb.AppendLine("                " + string.Join(", ", insertVals));
        sb.AppendLine("            );");
        sb.AppendLine();
        sb.AppendLine(pk.IsIdentity
            ? "            SET @GeneratedCode = CAST(SCOPE_IDENTITY() AS VARCHAR(50));"
            : $"            SET @GeneratedCode = @{pkParam};");
        sb.AppendLine("        END");

        // ---- U (update) ----
        var updatable = editable.Where(c => c.IncludeUpdate).ToList();
        sb.AppendLine("        ELSE IF @p_work_type = 'U'");
        sb.AppendLine("        BEGIN");
        var setClauses = updatable.Select(c => $"{c.Column.ColumnNm} = @{ParamName(c.Column.ColumnNm)}").ToList();
        if (hasAuditColumn("upt_user_id")) setClauses.Add("upt_user_id = @p_user_id");
        if (hasAuditColumn("upt_dt")) setClauses.Add("upt_dt = GETDATE()");
        if (hasAuditColumn("upt_pc")) setClauses.Add("upt_pc = @p_client_pc");

        if (setClauses.Count == 0) throw new ArgumentException("Update에 포함할 컬럼을 하나 이상 선택해주세요.");

        sb.AppendLine($"            UPDATE {spec.MasterTable} SET");
        sb.AppendLine("                " + string.Join(", ", setClauses));
        sb.AppendLine($"            WHERE {pk.ColumnNm} = @{pkParam};");
        sb.AppendLine();
        sb.AppendLine(pk.IsIdentity
            ? $"            SET @GeneratedCode = CAST(@{pkParam} AS VARCHAR(50));"
            : $"            SET @GeneratedCode = @{pkParam};");
        sb.AppendLine("        END");

        // ---- D (delete) - 마스터 테이블만, 상세는 손대지 않는다(2026-09-13 확인된 관례) ----
        sb.AppendLine("        ELSE IF @p_work_type = 'D'");
        sb.AppendLine("        BEGIN");
        sb.AppendLine($"            DELETE FROM {spec.MasterTable} WHERE {pk.ColumnNm} = @{pkParam};");
        sb.AppendLine("        END");

        sb.AppendLine("    END TRY");
        AppendStandardCatch(sb);
        sb.AppendLine("END");
        return sb.ToString();
    }

    private static bool IsAuditColumn(string columnNm) => AuditColumnNames.Contains(columnNm, StringComparer.OrdinalIgnoreCase);

    /// <summary>@p_ + 컬럼명(소문자) - 대소문자가 섞인 컬럼명(TSMMENU의 MENU_ID 등)도 이 코드베이스
    /// 관례(feedback_sql_proc_style)상 파라미터명은 항상 소문자라 여기서 통일한다.</summary>
    private static string ParamName(string columnNm) => "p_" + columnNm.ToLowerInvariant();

    private static string FormatSqlType(TableColumnInfoDto col)
    {
        var t = col.SqlType.ToUpperInvariant();
        if (t is "VARCHAR" or "NVARCHAR" or "CHAR" or "NCHAR")
            return col.MaxLength is > 0 and < 4000 ? $"{t}({col.MaxLength})" : $"{t}(MAX)";
        return t;
    }

    /// <summary>NOT NULL이지만 DB 자체에 DEFAULT 제약이 있어 값을 안 넘겨도 되는 컬럼인지는
    /// 여기서 정확히 알 수 없다(DescribeTableAsync가 DEFAULT 정의까지는 안 읽음) - 흔한 이름
    /// 패턴(use_yn/del_yn 등 Y/N 플래그)만 "값을 안 넘겨도 대체로 안전하다"고 보수적으로
    /// 취급한다. 애매하면 그냥 필수 파라미터로 남겨두는 쪽(과생성 아닌 과요구)이 안전하다.</summary>
    private static bool HasSensibleDefault(TableColumnInfoDto col) =>
        col.ColumnNm.EndsWith("_yn", StringComparison.OrdinalIgnoreCase);

    private static void AppendStandardOutputParams(StringBuilder sb)
    {
        sb.AppendLine("    @GeneratedCode VARCHAR(50) = NULL OUTPUT,");
        sb.AppendLine("    @ReturnCode INT = 0 OUTPUT,");
        sb.AppendLine("    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,");
        sb.AppendLine("    @ErrorCode INT = 0 OUTPUT,");
        sb.AppendLine("    @ErrorMsg NVARCHAR(500) = NULL OUTPUT");
    }

    private static void AppendStandardCatch(StringBuilder sb)
    {
        sb.AppendLine("    BEGIN CATCH");
        sb.AppendLine("        SET @ReturnCode = -1;");
        sb.AppendLine("        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';");
        sb.AppendLine("        SET @ErrorCode = ERROR_NUMBER();");
        sb.AppendLine("        SET @ErrorMsg = ERROR_MESSAGE();");
        sb.AppendLine("    END CATCH");
    }

    /// <summary>ScreenTemplateGenerator.NextMigrationPath와 완전히 같은 계산(00.DEV/04.Database/*.sql
    /// 파일명 앞 3자리 숫자의 최댓값+1) - 그 메서드가 private이라 재사용 대신 그대로 다시
    /// 구현했다(10줄 남짓이라 중복이 문제될 정도는 아님, ScreenTemplateGenerator.cs 주석 참고).</summary>
    public static string NextMigrationPath(string repoRoot, string name)
    {
        var dbDir = Path.Combine(repoRoot, "00.DEV", "04.Database");
        var next = Directory.GetFiles(dbDir, "*.sql")
            .Select(Path.GetFileName)
            .Select(f => f!.Length >= 3 && int.TryParse(f.Substring(0, 3), out var n) ? n : 0)
            .DefaultIfEmpty(0)
            .Max() + 1;
        return Path.Combine(dbDir, $"{next:000}_{name}_Procs.sql");
    }
}
