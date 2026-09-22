namespace WYNLAB.Api.Controllers;

/// <summary>
/// reg_pc/upt_pc 등 감사컬럼에 남길 접속 정보 - "PC명 | IP" 한 값으로 합쳐서 남긴다(사장님 지시,
/// 2026-09-11 - "REG_PC, UPT_PC 컬럼에 데이터가 제대로 들어가지 않고 있다"). 그 전에는 컨트롤러마다
/// 제각각이었다: 대부분(DataController 등)은 접속 IP만 남겼고, 일부(FilesController/
/// SiteConfigController)는 PC명이 있으면 IP 대신 "대체"했다 - 어느 쪽이든 한쪽 정보만 반쪽으로
/// 찍혔다. PC명은 클라이언트(WYNLAB.BaseForm.ApiClient.CreateClient)가 매 요청에 실어 보내는
/// X-Client-Pc 헤더(Environment.MachineName) - 헤더가 없으면(다른 도구로 직접 API를 호출한 경우
/// 등) "-"로 채운다.
///
/// IP는 원래 서버가 보는 접속 IP(RemoteIpAddress)를 썼는데, 클라이언트/서버가 같은 PC(로컬
/// 개발환경)에서 localhost로 붙으면 이게 무조건 루프백(::1)으로 찍혀 "IP 정보가 반영 안 되는 것
/// 같다"는 문의가 들어왔다(2026-09-11, "NEXTONE | 192.168.120.1 이렇게 등록되길 바라는건데"). 그래서
/// 클라이언트가 자신의 실제 사설망 IPv4도 X-Client-Ip 헤더로 같이 보내고, 이 헤더가 있으면
/// RemoteIpAddress보다 우선한다 - 헤더가 없으면(구버전 클라이언트, 또는 다른 도구로 직접 호출한
/// 경우) RemoteIpAddress로 되돌아간다. PC명과 마찬가지로 클라이언트가 자체 보고하는 값이라
/// 신뢰하지 않는 보안 판단에는 쓰지 않는다 - 감사이력 참고용일 뿐이다.
/// </summary>
public static class ClientPcInfo
{
    public static string Build(HttpContext context)
    {
        string? pcName = context.Request.Headers["X-Client-Pc"];
        string? reportedIp = context.Request.Headers["X-Client-Ip"];
        var ip = string.IsNullOrWhiteSpace(reportedIp) ? context.Connection.RemoteIpAddress?.ToString() : reportedIp;
        return $"{(string.IsNullOrWhiteSpace(pcName) ? "-" : pcName)} | {(string.IsNullOrWhiteSpace(ip) ? "-" : ip)}";
    }
}
