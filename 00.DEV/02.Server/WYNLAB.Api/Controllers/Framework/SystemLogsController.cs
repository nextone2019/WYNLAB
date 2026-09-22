using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.Framework;

/// <summary>
/// 서버(D:\WYNLAB_SVC\Api\logs\ 등)에 원격 데스크톱으로 들어가지 않아도, 오늘/최근 로그 파일의
/// 마지막 N줄을 그대로 화면에서 확인할 수 있게 한다(2026-09-15 - 로깅/모니터링 요청의 일부).
/// 로그에는 예외 스택트레이스 등 민감한 내부 정보가 그대로 담기므로 admin만 볼 수 있게 막는다
/// (JWT에 "개발자" 클레임은 없고 "isAdmin"만 있어서 그걸로 게이트한다 - RequireMenuPermission은
/// 메뉴 하나에 매달린 권한이라 이런 범용 시스템 엔드포인트엔 안 맞는다, LookupsController 등과
/// 같은 이유로 메뉴 게이트 없이 인증만 요구하는 대신 여기서는 admin 여부를 직접 확인).
/// </summary>
[ApiController]
[Route("api/system/logs")]
[Authorize]
public class SystemLogsController : ControllerBase
{
    private static readonly string LogsDir = Path.Combine(AppContext.BaseDirectory, "logs");

    [HttpGet("tail")]
    public ActionResult<ApiResult> Tail([FromQuery] int lines = 200)
    {
        if (User.FindFirstValue("isAdmin") != "Y")
            return Forbid();

        lines = Math.Clamp(lines, 1, 2000);

        // Serilog의 rollingInterval:Day는 파일명이 wynlab-yyyyMMdd.log 형태다 - 날짜순으로
        // 가장 최근 파일을 그냥 찾는다(오늘 파일이 아직 하나도 안 만들어졌을 수도 있으니
        // "오늘 날짜"를 가정하지 않는다).
        if (!Directory.Exists(LogsDir))
            return Ok(new ApiResult { Success = true, Message = "(로그 파일이 아직 없습니다)" });

        var latestFile = Directory.GetFiles(LogsDir, "wynlab-*.log")
            .OrderByDescending(f => f)
            .FirstOrDefault();
        if (latestFile == null)
            return Ok(new ApiResult { Success = true, Message = "(로그 파일이 아직 없습니다)" });

        // 로그 파일은 Serilog가 계속 열어서 쓰고 있으므로, 다른 프로세스도 읽을 수 있게
        // FileShare.ReadWrite로 연다(기본 File.ReadAllLines는 배타적으로 열어서 잠금 충돌이 남).
        using var stream = new FileStream(latestFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new StreamReader(stream);
        var allLines = new List<string>();
        while (reader.ReadLine() is { } line) allLines.Add(line);

        var tail = allLines.Skip(Math.Max(0, allLines.Count - lines));
        return Ok(new ApiResult { Success = true, Message = string.Join('\n', tail) });
    }
}
