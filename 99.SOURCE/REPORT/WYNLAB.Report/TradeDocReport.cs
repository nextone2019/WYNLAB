using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;

namespace WYNLAB.Report;

/// <summary>
/// TradeDocModel → A4 세로 XtraReport. 리포트 디자이너/Designer.cs 없이 코드로만 조립한다(문서가 늘어도 양식 코드는 이 한 곳).
/// 구조(복사본마다 같은 구조가 한 그룹으로 반복되고, 그룹 사이에 페이지가 나뉜다):
///   GroupHeader1(복사본마다 1번) : 보관용 표시 / 제목 / 문서번호·일자 / 당사자 박스 2개 / 요약 줄
///   GroupHeader2(페이지마다)     : 품목 표 머리글(여러 쪽이면 매 쪽 반복)
///   Detail                       : 품목 한 줄
///   GroupFooter1                 : 합계 / 비고 / 안내문구
///   PageFooter                   : 사업장명 / 복사본별 쪽번호
/// 복사본(보관용 N장)은 Lines를 N번 복제해 _copy 컬럼으로 묶는 방식이라 별도 문서 병합 없이 한 리포트로 끝난다.
/// </summary>
public static class TradeDocReport
{
    private const float Margin = 40f;       // 1/100 인치 (A4 827 x 1169)
    private const float RowH = 20f;
    private static readonly Font Base = new("맑은 고딕", 9f);
    private static readonly Font Bold = new("맑은 고딕", 9f, FontStyle.Bold);
    private static readonly Color Shade = Color.FromArgb(235, 238, 243);

    public static XtraReport Build(TradeDocModel m)
    {
        var copies = m.CopyLabels.Count > 0 ? m.CopyLabels.ToArray() : new[] { string.Empty };

        var report = new XtraReport
        {
            PaperKind = PaperKind.A4,
            Landscape = false,
            Margins = new Margins((int)Margin, (int)Margin, (int)Margin, (int)Margin),
            Font = Base,
            DataSource = BuildSource(m, copies),
        };
        if (!string.IsNullOrWhiteSpace(m.Watermark))
        {
            report.Watermark.Text = m.Watermark;
            report.Watermark.Font = new Font("맑은 고딕", 80f, FontStyle.Bold);
            report.Watermark.ForeColor = Color.Gainsboro;
            report.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
            report.Watermark.TextTransparency = 120;
            report.Watermark.ShowBehind = true;
        }

        var w = 827f - Margin * 2; // 사용 폭

        var top = new TopMarginBand { HeightF = Margin };
        var bottom = new BottomMarginBand { HeightF = Margin };

        // ---- 복사본 그룹 헤더(제목/당사자) ----
        var gh1 = new GroupHeaderBand { Name = "ghCopy", Level = 0, RepeatEveryPage = false, KeepTogether = false };
        gh1.GroupFields.Add(new GroupField("_copy", XRColumnSortOrder.Ascending));
        BuildHeader(gh1, m, w);

        // ---- 표 머리글(매 쪽) ----
        var gh2 = new GroupHeaderBand { Name = "ghCols", Level = 1, RepeatEveryPage = true, HeightF = RowH + 2f };
        gh2.GroupFields.Add(new GroupField("_copy", XRColumnSortOrder.Ascending));
        gh2.Controls.Add(ColumnTable(m, w, header: true, y: 2f));

        // ---- 품목 ----
        var detail = new DetailBand { HeightF = RowH };
        detail.Controls.Add(ColumnTable(m, w, header: false, y: 0f));

        // ---- 합계/비고 ----
        var gf = new GroupFooterBand { Name = "gfCopy", Level = 0, HeightF = 10f, PageBreak = PageBreak.AfterBandExceptLastEntry };
        BuildFooter(gf, m, w);

        // ---- 페이지 푸터 ----
        var pf = new PageFooterBand { HeightF = 22f };
        pf.Controls.Add(new XRLabel { Text = m.FooterLeft ?? string.Empty, LocationF = new PointF(0, 4), SizeF = new SizeF(w * 0.6f, 16), Font = Base, ForeColor = Color.Gray });
        pf.Controls.Add(new XRPageInfo
        {
            PageInfo = PageInfo.Number,
            Format = "- {0} -",
            RunningBand = gh1, // 보관용(복사본)마다 쪽번호를 1부터 다시 센다
            LocationF = new PointF(w * 0.6f, 4), SizeF = new SizeF(w * 0.4f, 16),
            TextAlignment = TextAlignment.MiddleRight, Font = Base, ForeColor = Color.Gray,
        });

        report.Bands.AddRange(new Band[] { top, gh1, gh2, detail, gf, pf, bottom });
        gh1.Level = 1; gh2.Level = 0; gf.Level = 1; // 추가 후에 지정해야 그룹 헤더 순서(제목/당사자 → 표 머리글)가 보장된다
        return report;
    }

    /// <summary>Lines를 보관용 수만큼 복제하고 _copy(번호)/_label(보관용 표시) 컬럼을 붙인 데이터 소스.</summary>
    private static DataTable BuildSource(TradeDocModel m, string[] copies)
    {
        var src = m.Lines.Clone();
        src.Columns.Add("_copy", typeof(int));
        src.Columns.Add("_label", typeof(string));
        for (var i = 0; i < copies.Length; i++)
        {
            foreach (DataRow r in m.Lines.Rows)
            {
                var nr = src.NewRow();
                foreach (DataColumn c in m.Lines.Columns) nr[c.ColumnName] = r[c];
                nr["_copy"] = i;
                nr["_label"] = copies[i];
                src.Rows.Add(nr);
            }
            if (m.Lines.Rows.Count == 0) // 품목이 없어도 제목/당사자는 나오게 빈 줄 하나
            {
                var nr = src.NewRow();
                nr["_copy"] = i;
                nr["_label"] = copies[i];
                src.Rows.Add(nr);
            }
        }
        return src;
    }

    // ===================== 헤더 =====================

    private static void BuildHeader(GroupHeaderBand band, TradeDocModel m, float w)
    {
        var y = 0f;

        // 보관용 표시(우상단) - 복사본 컬럼에 바인딩
        var copyLabel = new XRLabel { LocationF = new PointF(0, y), SizeF = new SizeF(w, 16), Font = Base, ForeColor = Color.DimGray, TextAlignment = TextAlignment.MiddleRight };
        copyLabel.DataBindings.Add("Text", null, "_label");
        band.Controls.Add(copyLabel);

        if (m.Left?.Logo is { Length: > 0 } logo && TryImage(logo) is { } logoImg)
            band.Controls.Add(new XRPictureBox { Image = logoImg, Sizing = ImageSizeMode.Squeeze, LocationF = new PointF(0, y), SizeF = new SizeF(90, 34) });

        y += 18f;
        band.Controls.Add(new XRLabel
        {
            Text = m.Title, LocationF = new PointF(0, y), SizeF = new SizeF(w, 34), Font = new Font("맑은 고딕", 20f, FontStyle.Bold),
            TextAlignment = TextAlignment.MiddleCenter, Borders = BorderSide.Bottom, BorderWidth = 2f,
        });
        y += 42f;

        // 문서번호 / 일자
        var info = NewTable(w, y);
        info.BeginInit();
        var infoRow = new XRTableRow { HeightF = RowH };
        infoRow.Cells.Add(Cell(m.DocNoLabel, 14, Bold, shade: true, align: TextAlignment.MiddleCenter));
        infoRow.Cells.Add(Cell(m.DocNo, 36));
        infoRow.Cells.Add(Cell(m.DateLabel, 14, Bold, shade: true, align: TextAlignment.MiddleCenter));
        infoRow.Cells.Add(Cell(m.Date, 36));
        info.Rows.Add(infoRow);
        info.EndInit();
        info.HeightF = RowH;
        band.Controls.Add(info);
        y += RowH + 8f;

        // 당사자 박스
        if (m.Left != null || m.Right != null)
        {
            var gap = 8f;
            var pw = (w - gap) / 2f;
            var h = 0f;
            if (m.Left != null) h = Math.Max(h, AddParty(band, m.Left, 0, y, pw));
            if (m.Right != null) h = Math.Max(h, AddParty(band, m.Right, pw + gap, y, pw));
            y += h + 8f;
        }

        // 요약(합계금액 등)
        if (!string.IsNullOrWhiteSpace(m.SummaryLabel))
        {
            var sum = NewTable(w, y);
            sum.BeginInit();
            var row = new XRTableRow { HeightF = 26f };
            row.Cells.Add(Cell(m.SummaryLabel!, 18, Bold, shade: true, align: TextAlignment.MiddleCenter));
            var val = Cell(m.SummaryValue ?? string.Empty, 82, new Font("맑은 고딕", 11f, FontStyle.Bold), align: TextAlignment.MiddleRight);
            row.Cells.Add(val);
            sum.Rows.Add(row);
            sum.EndInit();
            sum.HeightF = 26f;
            band.Controls.Add(sum);
            y += 26f + 8f;
        }

        band.HeightF = y;
    }

    /// <summary>당사자 박스(세로 캡션 + 라벨/값 표)를 그리고 높이를 돌려준다. 직인은 성명 칸 위에 겹쳐 찍는다.</summary>
    private static float AddParty(GroupHeaderBand band, TradeParty p, float x, float y, float pw)
    {
        const float capW = 22f;
        var rows = p.Rows.Count;
        var h = rows * RowH;

        band.Controls.Add(new XRLabel
        {
            Text = p.Caption, LocationF = new PointF(x, y), SizeF = new SizeF(capW, h), Font = Bold, Angle = 90f,
            TextAlignment = TextAlignment.MiddleCenter, Borders = BorderSide.All, BackColor = Shade, Padding = new PaddingInfo(0, 0, 0, 0),
        });

        var t = NewTable(pw - capW, y);
        t.LocationF = new PointF(x + capW, y);
        t.BeginInit();
        foreach (var r in p.Rows)
        {
            var row = new XRTableRow { HeightF = RowH };
            row.Cells.Add(Cell(r.Label1, 17, Bold, shade: true, align: TextAlignment.MiddleCenter));
            if (r.Label2 == null)
                row.Cells.Add(Cell(r.Value1, 83));
            else
            {
                row.Cells.Add(Cell(r.Value1, 33));
                row.Cells.Add(Cell(r.Label2, 17, Bold, shade: true, align: TextAlignment.MiddleCenter));
                row.Cells.Add(Cell(r.Value2 ?? string.Empty, 33));
            }
            t.Rows.Add(row);
        }
        t.EndInit();
        t.HeightF = h;
        band.Controls.Add(t);

        if (p.Stamp is { Length: > 0 } stamp && TryImage(stamp) is { } img && p.StampRow >= 0 && p.StampRow < rows)
        {
            var size = 32f;
            band.Controls.Add(new XRPictureBox
            {
                Image = img, Sizing = ImageSizeMode.Squeeze,
                LocationF = new PointF(x + pw - size - 4f, y + p.StampRow * RowH - 6f),
                SizeF = new SizeF(size, size),
            });
        }
        return h;
    }

    // ===================== 표 / 푸터 =====================

    private static XRTable ColumnTable(TradeDocModel m, float w, bool header, float y)
    {
        var t = NewTable(w, y);
        t.BeginInit();
        var row = new XRTableRow { HeightF = RowH };
        foreach (var c in m.Columns)
        {
            var align = c.Align switch { TradeAlign.Right => TextAlignment.MiddleRight, TradeAlign.Center => TextAlignment.MiddleCenter, _ => TextAlignment.MiddleLeft };
            if (header)
                row.Cells.Add(Cell(c.Caption, c.Width, Bold, shade: true, align: TextAlignment.MiddleCenter));
            else
            {
                var cell = Cell(string.Empty, c.Width, align: align);
                cell.DataBindings.Add("Text", null, c.Field, c.Format == null ? string.Empty : "{0:" + c.Format + "}");
                row.Cells.Add(cell);
            }
        }
        t.Rows.Add(row);
        t.EndInit();
        t.HeightF = RowH;
        return t;
    }

    private static void BuildFooter(GroupFooterBand band, TradeDocModel m, float w)
    {
        var y = 6f;

        if (m.Totals.Count > 0)
        {
            var tw = Math.Min(w, 300f);
            var t = NewTable(tw, y);
            t.LocationF = new PointF(w - tw, y);
            t.BeginInit();
            foreach (var kv in m.Totals)
            {
                var row = new XRTableRow { HeightF = RowH };
                row.Cells.Add(Cell(kv.Key, 38, Bold, shade: true, align: TextAlignment.MiddleCenter));
                row.Cells.Add(Cell(kv.Value, 62, align: TextAlignment.MiddleRight));
                t.Rows.Add(row);
            }
            t.EndInit();
            t.HeightF = m.Totals.Count * RowH;
            band.Controls.Add(t);
            y += m.Totals.Count * RowH + 8f;
        }

        if (!string.IsNullOrWhiteSpace(m.Remark))
        {
            var t = NewTable(w, y);
            t.BeginInit();
            var row = new XRTableRow { HeightF = 44f };
            row.Cells.Add(Cell("비 고", 10, Bold, shade: true, align: TextAlignment.MiddleCenter));
            row.Cells.Add(Cell(m.Remark!, 90, align: TextAlignment.TopLeft));
            t.Rows.Add(row);
            t.EndInit();
            t.HeightF = 44f;
            band.Controls.Add(t);
            y += 44f + 8f;
        }

        foreach (var note in m.Notes)
        {
            band.Controls.Add(new XRLabel { Text = note, LocationF = new PointF(0, y), SizeF = new SizeF(w, 16), Font = Base, ForeColor = Color.DimGray });
            y += 16f;
        }

        band.HeightF = y + 4f;
    }

    // ===================== 공통 조각 =====================

    private static XRTable NewTable(float width, float y) => new()
    {
        LocationF = new PointF(0, y),
        WidthF = width,
        Borders = BorderSide.All,
        BorderWidth = 1f,
        Font = Base,
    };

    private static XRTableCell Cell(string text, float weight, Font? font = null, bool shade = false, TextAlignment align = TextAlignment.MiddleLeft)
    {
        var cell = new XRTableCell
        {
            Text = text, Weight = weight, TextAlignment = align, Padding = new PaddingInfo(4, 4, 0, 0),
            Borders = BorderSide.All, BorderWidth = 1f, Font = font ?? Base, CanGrow = false, WordWrap = true,
        };
        if (shade) cell.BackColor = Shade;
        return cell;
    }

    private static Image? TryImage(byte[] bytes)
    {
        try { return Image.FromStream(new MemoryStream(bytes)); }
        catch (ArgumentException) { return null; }
    }
}
