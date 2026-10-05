using System.Text;

namespace WYNLAB.Shared;

/// <summary>
/// 전각(全角) 문자를 반각으로 바꾼다(2026-10-05). Windows 11 + 한글 IME에서 입력 모드가 자동으로 "전각"으로 바뀌면 영문/숫자/공백/기호가
/// 전각 문자(ＡＢＣ１２３, 전각 공백 U+3000)로 들어와 글자 사이가 벌어져 보이고, 코드/품번/이름 검색이 안 맞는 데이터가 등록되곤 했다.
/// 서버(모든 요청 문자열)와 클라이언트(입력 컨트롤)가 같은 규칙을 쓰도록 여기 하나에 둔다.
///
/// 바꾸는 것: 전각 ASCII(U+FF01~U+FF5E → U+0021~U+007E), 전각 공백(U+3000)·줄바꿈 없는 공백(U+00A0) → 일반 공백.
/// 안 건드리는 것: 한글/한자/일본어 문자와 그 외 모든 문자(전각 원화 ￦ 등은 의도적으로 제외).
/// </summary>
public static class TextNormalizer
{
    /// <summary>전각 문자가 하나라도 있는지 - 대부분의 문자열은 여기서 false라 새 문자열을 만들지 않는다.</summary>
    public static bool NeedsFix(string? text)
    {
        if (string.IsNullOrEmpty(text)) return false;
        foreach (var c in text!)
            if (IsFullWidth(c)) return true;
        return false;
    }

    public static string? ToHalfWidth(string? text)
    {
        if (!NeedsFix(text)) return text;

        var sb = new StringBuilder(text!.Length);
        foreach (var c in text)
            sb.Append(ToHalfWidth(c));
        return sb.ToString();
    }

    public static char ToHalfWidth(char c) =>
        c == '\u3000' || c == '\u00A0' ? ' '
        : c >= '\uFF01' && c <= '\uFF5E' ? (char)(c - 0xFEE0)
        : c;

    public static bool IsFullWidth(char c) => c == '\u3000' || c == '\u00A0' || (c >= '\uFF01' && c <= '\uFF5E');
}