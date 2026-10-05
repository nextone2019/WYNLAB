using DevExpress.XtraReports.UI;

namespace WYNLAB.Report;

/// <summary>
/// 출력물 공통 진입점 - 어떤 XtraReport든(TradeDocReport가 만든 것이든 리포트 디자이너로 만든 것이든) 미리보기/인쇄/PDF 내보내기를 같은 방식으로 띄운다.
/// ReportPrintTool.ShowPreviewDialog가 인쇄·PDF·엑셀 내보내기 버튼을 갖춘 모달 미리보기라 별도 공용 팝업을 만들 필요가 없다.
/// </summary>
public static class ReportKit
{
    /// <summary>거래 문서 모델로 바로 미리보기.</summary>
    public static void Preview(TradeDocModel model, IWin32Window? owner = null)
    {
        using var report = TradeDocReport.Build(model);
        PreviewReport(report, owner);
    }

    /// <summary>이미 만든 리포트의 미리보기(이름을 Preview와 달리한 이유: 오버로드면 호출하는 모듈도 XtraReports 어셈블리를 참조해야 컴파일된다).</summary>
    public static void PreviewReport(XtraReport report, IWin32Window? owner = null)
    {
        using var tool = new ReportPrintTool(report);
        if (owner == null) tool.ShowPreviewDialog();
        else tool.ShowPreviewDialog(owner, DevExpress.LookAndFeel.UserLookAndFeel.Default);
    }

    /// <summary>화면 없이 PDF 파일로 저장(메일 첨부/자동 발송용).</summary>
    public static void ExportPdf(TradeDocModel model, string path)
    {
        using var report = TradeDocReport.Build(model);
        report.ExportToPdf(path);
    }
}
