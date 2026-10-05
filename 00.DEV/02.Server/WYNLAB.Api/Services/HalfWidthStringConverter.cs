using System.Text.Json;
using System.Text.Json.Serialization;
using WYNLAB.Shared;

namespace WYNLAB.Api.Services;

/// <summary>
/// 요청 본문(JSON)의 모든 문자열을 읽는 순간 반각으로 바꾼다(2026-10-05). Windows 11 IME가 자동으로 전각 입력 모드가 되어 전각 영문/숫자/공백이
/// 저장 데이터와 검색 조건에 섞여 들어오던 문제를, 화면마다 고치지 않고 서버 입구 한 곳에서 막는다 - 범용 저장/조회(Dictionary 본문)와
/// 타입 DTO 본문 둘 다 이 변환기를 거친다. 쓰기(응답)는 그대로 둔다. 규칙은 <see cref="TextNormalizer"/> 참고.
/// </summary>
public sealed class HalfWidthStringConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        TextNormalizer.ToHalfWidth(reader.GetString());

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value);
}