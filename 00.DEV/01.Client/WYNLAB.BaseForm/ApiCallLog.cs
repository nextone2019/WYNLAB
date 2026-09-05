using System.ComponentModel;

namespace WYNLAB.Base;

/// <summary>
/// api/data/query, api/data/save로 나간 요청 한 건 - SQL 프로파일러 화면(ShellForm)에 그대로
/// 그리드 행 하나로 표시된다. 원본 SQL 텍스트가 아니라 "프로시저명 + 파라미터"인 이유: 이
/// 클라이언트는 애초에 서버에 SQL 텍스트를 보내지 않는다(범용 데이터 통로 - GENERIC_DATA_API.md
/// 참고) - 실제로 서버에 보낸 것을 그대로 보여주는 게 이 클라이언트 입장에서 가장 정직한 로그다.
///
/// Params를 문자열이 아니라 Dictionary로 들고 있는 이유: ToSqlText()가 SSMS에 그대로 붙여넣어
/// 실행할 수 있는 EXEC 문을 만들어야 하는데, 미리 "key=value, key=value" 형태 문자열로 합쳐두면
/// 값 안에 쉼표가 들어있을 때 다시 정확히 못 쪼갠다 - 원본 키/값 쌍을 그대로 들고 있어야 안전하다.
/// </summary>
public class ApiCallLogEntry
{
    public DateTime Timestamp { get; set; }
    public string MenuId { get; set; } = string.Empty;
    public string ProcName { get; set; } = string.Empty;
    public Dictionary<string, string?> Params { get; set; } = new();
    public bool Success { get; set; }
    public string? Message { get; set; }
    public long DurationMs { get; set; }

    /// <summary>그리드에서 한 줄로 훑어볼 SQL 미리보기 - ToSqlText()를 한 줄로 압축한 것.
    /// 전체(줄바꿈 있는 그대로 붙여넣기 좋은 형태)는 하단 메모(ShellForm)에 별도로 보여준다.</summary>
    public string SqlPreview => ToSqlText().Replace("\n", " ").Replace("    ", "");

    /// <summary>SSMS 등 SQL 클라이언트에 그대로 붙여넣어 실행할 수 있는 EXEC 문. 값은 항상
    /// N'...'로 감싼다 - 실제 프로시저 파라미터 타입이 뭐든(VARCHAR/INT/DATETIME) SQL Server가
    /// 문자열 리터럴을 알아서 변환해 받아들이므로, 여기서 타입을 추측해서 따옴표를 뺄 필요가
    /// 없고 오히려 잘못 추측하면 실행이 깨진다.</summary>
    public string ToSqlText()
    {
        if (Params.Count == 0) return $"EXEC {ProcName}";

        var lines = Params.Select(kv => $"    @{kv.Key} = {FormatSqlValue(kv.Value)}");
        return $"EXEC {ProcName}\n" + string.Join(",\n", lines);
    }

    private static string FormatSqlValue(string? value) =>
        value == null ? "NULL" : $"N'{value.Replace("'", "''")}'";
}

/// <summary>
/// 이 클라이언트가 보낸 api/data/* 요청 내역을 인메모리로 쌓아두는 저장소. ProcData.cs가 요청
/// 하나 끝날 때마다 Record()를 부르고, ShellForm의 SQL 로그 뷰어 패널이 Entries를 그리드에
/// 바인딩해서 보여준다(관리자 - TSMUSER.USER_TYPE='A' - 전용).
///
/// 정적 클래스인 이유: 로그가 특정 화면 인스턴스에 속하지 않고 앱 전체(어느 BaseForm에서
/// ProcData를 호출하든)에 걸쳐 쌓여야 하고, 뷰어 패널을 닫았다 열어도 그대로 남아있어야 하기
/// 때문이다(요청사항: "화면을 끄더라도 계속 쌓이고 언제든 다시 볼 수 있게").
///
/// BindingList를 쓰는 이유: DevExpress GridControl이 BindingList의 ListChanged를 자동으로
/// 구독해서, Record()로 새 행이 추가되거나 Clear()로 비워질 때마다 뷰어가 열려있으면 그리드가
/// 별도 새로고침 코드 없이 저절로 갱신된다.
/// </summary>
public static class ApiCallLog
{
    /// <summary>요청을 실제로 기록할지 여부 - "시작" 버튼을 누르기 전까지는 기본적으로 꺼져있다.
    /// 뷰어 패널이 떠 있는지와는 무관하다(패널을 닫아도 이 값이 true면 계속 쌓인다).</summary>
    public static bool IsCapturing { get; set; }

    public static BindingList<ApiCallLogEntry> Entries { get; } = new();

    public static void Record(string menuId, string procName, Dictionary<string, string?> callParams, bool success, string? message, long durationMs)
    {
        if (!IsCapturing) return;

        Entries.Add(new ApiCallLogEntry
        {
            Timestamp = DateTime.Now,
            MenuId = menuId,
            ProcName = procName,
            Params = callParams,
            Success = success,
            Message = message,
            DurationMs = durationMs
        });
    }

    public static void Clear() => Entries.Clear();
}
