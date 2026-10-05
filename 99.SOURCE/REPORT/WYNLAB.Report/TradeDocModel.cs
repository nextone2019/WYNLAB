using System.Data;
using System.Drawing;
using System.Globalization;

namespace WYNLAB.Report;

/// <summary>
/// "거래 문서"(거래명세서/견적서/발주서/출고증/세금계산서 청구서 ...) 한 장의 내용 - 양식(레이아웃)은 TradeDocReport가 전부 책임지고,
/// 화면은 이 모델만 채워서 ReportKit.Preview에 넘긴다. 새 문서 출력물이 필요하면 리포트 디자이너 없이 이 모델만 채우면 된다.
/// </summary>
public class TradeDocModel
{
    /// <summary>문서 제목(예: "거래 명 세 서").</summary>
    public string Title { get; set; } = string.Empty;
    public string DocNoLabel { get; set; } = "문서번호";
    public string DocNo { get; set; } = string.Empty;
    public string DateLabel { get; set; } = "일자";
    public string Date { get; set; } = string.Empty;

    /// <summary>좌측 당사자(보통 공급자 = 우리 사업장)와 우측 당사자(공급받는자 = 고객). 둘 다 비우면 당사자 박스를 그리지 않는다.</summary>
    public TradeParty? Left { get; set; }
    public TradeParty? Right { get; set; }

    /// <summary>당사자 박스 아래의 한 줄 요약(예: 합계금액). 비우면 안 그린다.</summary>
    public string? SummaryLabel { get; set; }
    public string? SummaryValue { get; set; }

    public List<TradeColumn> Columns { get; } = new();
    public DataTable Lines { get; set; } = new();

    /// <summary>품목 표 아래 우측에 쌓이는 합계 줄(라벨, 값).</summary>
    public List<KeyValuePair<string, string>> Totals { get; } = new();

    public string? Remark { get; set; }
    /// <summary>비고 아래의 안내 문구(한 줄씩).</summary>
    public List<string> Notes { get; } = new();

    /// <summary>보관용 구분. 값이 N개면 같은 내용을 N장(각각 새 페이지) 출력한다. 비우면 1장.</summary>
    public List<string> CopyLabels { get; } = new();

    /// <summary>미리보기/인쇄 배경에 깔리는 워터마크(예: 미확정 문서는 "작성중"). 비우면 없음.</summary>
    public string? Watermark { get; set; }

    /// <summary>페이지 하단 좌측 문구(보통 사업장명).</summary>
    public string? FooterLeft { get; set; }

    /// <summary>합계 줄 추가 + 합계 계산 - Lines의 숫자 컬럼을 더해 Totals에 넣는다. (라벨, 컬럼명) 쌍을 순서대로.</summary>
    public TradeDocModel AddSumTotals(string format, params (string Label, string Field)[] sums)
    {
        foreach (var (label, field) in sums)
        {
            decimal total = 0;
            if (Lines.Columns.Contains(field))
                foreach (DataRow r in Lines.Rows)
                    if (r[field] != DBNull.Value && decimal.TryParse(Convert.ToString(r[field], CultureInfo.InvariantCulture), NumberStyles.Any, CultureInfo.InvariantCulture, out var v)) total += v;
            Totals.Add(new KeyValuePair<string, string>(label, total.ToString(format, CultureInfo.InvariantCulture)));
        }
        return this;
    }
}

public class TradeColumn
{
    public TradeColumn(string caption, string field, float width, TradeAlign align = TradeAlign.Left, string? format = null)
    {
        Caption = caption; Field = field; Width = width; Align = align; Format = format;
    }

    public string Caption { get; }
    public string Field { get; }
    /// <summary>상대 폭(가중치) - 합계 대비 비율로 표 폭에 맞춰진다.</summary>
    public float Width { get; }
    public TradeAlign Align { get; }
    /// <summary>예: "#,##0.####". 숫자 컬럼은 보통 Right + 서식.</summary>
    public string? Format { get; }
}

public enum TradeAlign { Left, Center, Right }

/// <summary>당사자(공급자/공급받는자) 박스 내용: 줄 단위 (라벨, 값) 한 쌍 또는 두 쌍.</summary>
public class TradeParty
{
    public string Caption { get; set; } = string.Empty;
    public List<TradePartyRow> Rows { get; } = new();
    /// <summary>직인 이미지(공급자 박스의 성명 칸 옆에 겹쳐 찍힌다). null이면 안 찍음.</summary>
    public byte[]? Stamp { get; set; }
    /// <summary>로고 이미지(문서 좌상단). null이면 안 그림.</summary>
    public byte[]? Logo { get; set; }
    /// <summary>Rows 중 직인을 겹칠 줄(0부터). 기본 1 = 상호/성명 줄.</summary>
    public int StampRow { get; set; } = 1;

    /// <summary>
    /// 표준 당사자 컬럼 규약(FN_RPT_ACC/FN_RPT_CUST + 프로시저의 접두사 별칭: {prefix}_nm, _biz_no, _owner_nm, _addr, _biz_kind, _biz_type, _tel, _fax, _stamp, _logo)
    /// 로 한 행에서 표준 5줄(등록번호/상호·성명/주소/업태·종목/전화·팩스)을 만든다.
    /// </summary>
    public static TradeParty FromRow(DataRow row, string prefix, string caption)
    {
        string S(string n) => row.Table.Columns.Contains($"{prefix}_{n}") && row[$"{prefix}_{n}"] != DBNull.Value ? Convert.ToString(row[$"{prefix}_{n}"])?.Trim() ?? string.Empty : string.Empty;
        byte[]? B(string n)
        {
            var col = $"{prefix}_{n}";
            if (!row.Table.Columns.Contains(col) || row[col] == DBNull.Value) return null;
            if (row[col] is byte[] bytes) return bytes.Length > 0 ? bytes : null;
            var text = Convert.ToString(row[col]);
            if (string.IsNullOrWhiteSpace(text)) return null;
            try { return Convert.FromBase64String(text); } catch (FormatException) { return null; }
        }

        var p = new TradeParty { Caption = caption, Stamp = B("stamp"), Logo = B("logo") };
        p.Rows.Add(new TradePartyRow("등록번호", S("biz_no")));
        p.Rows.Add(new TradePartyRow("상호", S("nm"), "성명", S("owner_nm")));
        p.Rows.Add(new TradePartyRow("주소", S("addr")));
        p.Rows.Add(new TradePartyRow("업태", S("biz_kind"), "종목", S("biz_type")));
        p.Rows.Add(new TradePartyRow("전화", S("tel"), "팩스", S("fax")));
        return p;
    }
}

public class TradePartyRow
{
    public TradePartyRow(string label1, string value1, string? label2 = null, string? value2 = null)
    {
        Label1 = label1; Value1 = value1; Label2 = label2; Value2 = value2;
    }

    public string Label1 { get; }
    public string Value1 { get; }
    /// <summary>null이면 Value1이 한 줄 전체를 쓴다.</summary>
    public string? Label2 { get; }
    public string? Value2 { get; }
}
