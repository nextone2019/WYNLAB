using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;

namespace WYNLAB.Api.Services;

/// <summary>
/// 외부 API 연동 인증키(TSMAPIDEF.auth_key_enc)를 Windows DPAPI(LocalMachine 범위)로 암호화한다.
/// 별도의 암호화 키 파일이나 web.config 항목이 필요 없다 - Windows가 이 서버 머신에 종속된 키를
/// 알아서 관리한다. 대신 DB를 다른 서버로 복원해도 이 값은 그 서버에서 복호화되지 않는다 -
/// web.config 환경변수(DB연결문자열/JWT시크릿)가 서버마다 새로 설정돼야 하는 것과 같은 성격이라
/// 수용 가능한 트레이드오프로 판단(2026-09-15, "암호화 저장" 결정).
/// </summary>
[SupportedOSPlatform("windows")]
public static class ApiKeyProtector
{
    private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("WYNLAB.ApiIntegration");

    public static byte[] Protect(string plainKey) =>
        ProtectedData.Protect(Encoding.UTF8.GetBytes(plainKey), Entropy, DataProtectionScope.LocalMachine);

    public static string Unprotect(byte[] encrypted) =>
        Encoding.UTF8.GetString(ProtectedData.Unprotect(encrypted, Entropy, DataProtectionScope.LocalMachine));
}
