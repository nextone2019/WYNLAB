// VS 디자이너(리포트 디자이너)가 자동 생성하는 필드 선언 규칙을 그대로 따르는 디자이너 전용
// 파일 - nullable 경고를 피하려고 이 파일만 nullable 검사를 끈다(frmNameCardReq.Designer.cs와
// 같은 관례).
#nullable disable

namespace WYNLAB.AP.Report;

partial class rptNameCardReq
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrLabelTitle = new DevExpress.XtraReports.UI.XRLabel();
        this.xrCapReqNo = new DevExpress.XtraReports.UI.XRLabel();
        this.xrValReqNo = new DevExpress.XtraReports.UI.XRLabel();
        this.xrCapRegDt = new DevExpress.XtraReports.UI.XRLabel();
        this.xrValRegDt = new DevExpress.XtraReports.UI.XRLabel();
        this.xrCapDeptNm = new DevExpress.XtraReports.UI.XRLabel();
        this.xrValDeptNm = new DevExpress.XtraReports.UI.XRLabel();
        this.xrCapEmpNm = new DevExpress.XtraReports.UI.XRLabel();
        this.xrValEmpNm = new DevExpress.XtraReports.UI.XRLabel();
        this.xrCapAppNo = new DevExpress.XtraReports.UI.XRLabel();
        this.xrValAppNo = new DevExpress.XtraReports.UI.XRLabel();
        this.xrCapApprStatCd = new DevExpress.XtraReports.UI.XRLabel();
        this.xrValApprStatCd = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
        this.xrCapNameKor = new DevExpress.XtraReports.UI.XRLabel();
        this.xrValNameKor = new DevExpress.XtraReports.UI.XRLabel();
        this.xrCapNameEng = new DevExpress.XtraReports.UI.XRLabel();
        this.xrValNameEng = new DevExpress.XtraReports.UI.XRLabel();
        this.xrCapDeptKor = new DevExpress.XtraReports.UI.XRLabel();
        this.xrValDeptKor = new DevExpress.XtraReports.UI.XRLabel();
        this.xrCapDeptEng = new DevExpress.XtraReports.UI.XRLabel();
        this.xrValDeptEng = new DevExpress.XtraReports.UI.XRLabel();
        this.xrCapJobGrade = new DevExpress.XtraReports.UI.XRLabel();
        this.xrValJobGrade = new DevExpress.XtraReports.UI.XRLabel();
        this.xrCapMobile = new DevExpress.XtraReports.UI.XRLabel();
        this.xrValMobile = new DevExpress.XtraReports.UI.XRLabel();
        this.xrCapEmail = new DevExpress.XtraReports.UI.XRLabel();
        this.xrValEmail = new DevExpress.XtraReports.UI.XRLabel();
        this.xrCapRemark = new DevExpress.XtraReports.UI.XRLabel();
        this.xrValRemark = new DevExpress.XtraReports.UI.XRLabel();
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        this.SuspendLayout();
        //
        // TopMargin
        //
        this.TopMargin.HeightF = 30F;
        this.TopMargin.Name = "TopMargin";
        //
        // BottomMargin
        //
        this.BottomMargin.HeightF = 30F;
        this.BottomMargin.Name = "BottomMargin";
        //
        // ReportHeader
        //
        this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabelTitle});
        this.ReportHeader.HeightF = 60F;
        this.ReportHeader.Name = "ReportHeader";
        //
        // xrLabelTitle (제목 - 명함제작요청서)
        //
        this.xrLabelTitle.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold);
        this.xrLabelTitle.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10F);
        this.xrLabelTitle.Name = "xrLabelTitle";
        this.xrLabelTitle.SizeF = new System.Drawing.SizeF(650F, 40F);
        this.xrLabelTitle.StylePriority.UseFont = false;
        this.xrLabelTitle.StylePriority.UseTextAlignment = false;
        this.xrLabelTitle.Text = "명함제작요청서";
        this.xrLabelTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        //
        // Detail
        //
        this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrCapReqNo,
            this.xrValReqNo,
            this.xrCapRegDt,
            this.xrValRegDt,
            this.xrCapDeptNm,
            this.xrValDeptNm,
            this.xrCapEmpNm,
            this.xrValEmpNm,
            this.xrCapAppNo,
            this.xrValAppNo,
            this.xrCapApprStatCd,
            this.xrValApprStatCd,
            this.xrLine1,
            this.xrCapNameKor,
            this.xrValNameKor,
            this.xrCapNameEng,
            this.xrValNameEng,
            this.xrCapDeptKor,
            this.xrValDeptKor,
            this.xrCapDeptEng,
            this.xrValDeptEng,
            this.xrCapJobGrade,
            this.xrValJobGrade,
            this.xrCapMobile,
            this.xrValMobile,
            this.xrCapEmail,
            this.xrValEmail,
            this.xrCapRemark,
            this.xrValRemark});
        this.Detail.HeightF = 330F;
        this.Detail.Name = "Detail";
        //
        // 신청번호 / 신청일자
        //
        this.xrCapReqNo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10F);
        this.xrCapReqNo.Name = "xrCapReqNo";
        this.xrCapReqNo.SizeF = new System.Drawing.SizeF(90F, 20F);
        this.xrCapReqNo.Text = "신청번호";
        this.xrValReqNo.LocationFloat = new DevExpress.Utils.PointFloat(95F, 10F);
        this.xrValReqNo.Name = "xrValReqNo";
        this.xrValReqNo.SizeF = new System.Drawing.SizeF(220F, 20F);
        this.xrValReqNo.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[req_no]")});
        this.xrCapRegDt.LocationFloat = new DevExpress.Utils.PointFloat(330F, 10F);
        this.xrCapRegDt.Name = "xrCapRegDt";
        this.xrCapRegDt.SizeF = new System.Drawing.SizeF(90F, 20F);
        this.xrCapRegDt.Text = "신청일자";
        this.xrValRegDt.LocationFloat = new DevExpress.Utils.PointFloat(425F, 10F);
        this.xrValRegDt.Name = "xrValRegDt";
        this.xrValRegDt.SizeF = new System.Drawing.SizeF(225F, 20F);
        this.xrValRegDt.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[reg_dt]")});
        //
        // 신청부서 / 신청자
        //
        this.xrCapDeptNm.LocationFloat = new DevExpress.Utils.PointFloat(0F, 40F);
        this.xrCapDeptNm.Name = "xrCapDeptNm";
        this.xrCapDeptNm.SizeF = new System.Drawing.SizeF(90F, 20F);
        this.xrCapDeptNm.Text = "신청부서";
        this.xrValDeptNm.LocationFloat = new DevExpress.Utils.PointFloat(95F, 40F);
        this.xrValDeptNm.Name = "xrValDeptNm";
        this.xrValDeptNm.SizeF = new System.Drawing.SizeF(220F, 20F);
        this.xrValDeptNm.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[dept_nm]")});
        this.xrCapEmpNm.LocationFloat = new DevExpress.Utils.PointFloat(330F, 40F);
        this.xrCapEmpNm.Name = "xrCapEmpNm";
        this.xrCapEmpNm.SizeF = new System.Drawing.SizeF(90F, 20F);
        this.xrCapEmpNm.Text = "신청자";
        this.xrValEmpNm.LocationFloat = new DevExpress.Utils.PointFloat(425F, 40F);
        this.xrValEmpNm.Name = "xrValEmpNm";
        this.xrValEmpNm.SizeF = new System.Drawing.SizeF(225F, 20F);
        this.xrValEmpNm.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[emp_nm]")});
        //
        // 결재번호 / 결재상태
        //
        this.xrCapAppNo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 70F);
        this.xrCapAppNo.Name = "xrCapAppNo";
        this.xrCapAppNo.SizeF = new System.Drawing.SizeF(90F, 20F);
        this.xrCapAppNo.Text = "결재번호";
        this.xrValAppNo.LocationFloat = new DevExpress.Utils.PointFloat(95F, 70F);
        this.xrValAppNo.Name = "xrValAppNo";
        this.xrValAppNo.SizeF = new System.Drawing.SizeF(220F, 20F);
        this.xrValAppNo.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[app_no]")});
        this.xrCapApprStatCd.LocationFloat = new DevExpress.Utils.PointFloat(330F, 70F);
        this.xrCapApprStatCd.Name = "xrCapApprStatCd";
        this.xrCapApprStatCd.SizeF = new System.Drawing.SizeF(90F, 20F);
        this.xrCapApprStatCd.Text = "결재상태";
        this.xrValApprStatCd.LocationFloat = new DevExpress.Utils.PointFloat(425F, 70F);
        this.xrValApprStatCd.Name = "xrValApprStatCd";
        this.xrValApprStatCd.SizeF = new System.Drawing.SizeF(225F, 20F);
        this.xrValApprStatCd.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[appr_stat_cd]")});
        //
        // xrLine1 (구분선)
        //
        this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 105F);
        this.xrLine1.Name = "xrLine1";
        this.xrLine1.SizeF = new System.Drawing.SizeF(650F, 2F);
        //
        // 성명(한글) / 성명(영문)
        //
        this.xrCapNameKor.LocationFloat = new DevExpress.Utils.PointFloat(0F, 115F);
        this.xrCapNameKor.Name = "xrCapNameKor";
        this.xrCapNameKor.SizeF = new System.Drawing.SizeF(90F, 20F);
        this.xrCapNameKor.Text = "성명(한글)";
        this.xrValNameKor.LocationFloat = new DevExpress.Utils.PointFloat(95F, 115F);
        this.xrValNameKor.Name = "xrValNameKor";
        this.xrValNameKor.SizeF = new System.Drawing.SizeF(220F, 20F);
        this.xrValNameKor.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[name_kor]")});
        this.xrCapNameEng.LocationFloat = new DevExpress.Utils.PointFloat(330F, 115F);
        this.xrCapNameEng.Name = "xrCapNameEng";
        this.xrCapNameEng.SizeF = new System.Drawing.SizeF(90F, 20F);
        this.xrCapNameEng.Text = "성명(영문)";
        this.xrValNameEng.LocationFloat = new DevExpress.Utils.PointFloat(425F, 115F);
        this.xrValNameEng.Name = "xrValNameEng";
        this.xrValNameEng.SizeF = new System.Drawing.SizeF(225F, 20F);
        this.xrValNameEng.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[name_eng]")});
        //
        // 부서명(한글) / 부서명(영문)
        //
        this.xrCapDeptKor.LocationFloat = new DevExpress.Utils.PointFloat(0F, 145F);
        this.xrCapDeptKor.Name = "xrCapDeptKor";
        this.xrCapDeptKor.SizeF = new System.Drawing.SizeF(90F, 20F);
        this.xrCapDeptKor.Text = "부서명(한글)";
        this.xrValDeptKor.LocationFloat = new DevExpress.Utils.PointFloat(95F, 145F);
        this.xrValDeptKor.Name = "xrValDeptKor";
        this.xrValDeptKor.SizeF = new System.Drawing.SizeF(220F, 20F);
        this.xrValDeptKor.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[dept_kor]")});
        this.xrCapDeptEng.LocationFloat = new DevExpress.Utils.PointFloat(330F, 145F);
        this.xrCapDeptEng.Name = "xrCapDeptEng";
        this.xrCapDeptEng.SizeF = new System.Drawing.SizeF(90F, 20F);
        this.xrCapDeptEng.Text = "부서명(영문)";
        this.xrValDeptEng.LocationFloat = new DevExpress.Utils.PointFloat(425F, 145F);
        this.xrValDeptEng.Name = "xrValDeptEng";
        this.xrValDeptEng.SizeF = new System.Drawing.SizeF(225F, 20F);
        this.xrValDeptEng.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[dept_eng]")});
        //
        // 직위 / 휴대폰
        //
        this.xrCapJobGrade.LocationFloat = new DevExpress.Utils.PointFloat(0F, 175F);
        this.xrCapJobGrade.Name = "xrCapJobGrade";
        this.xrCapJobGrade.SizeF = new System.Drawing.SizeF(90F, 20F);
        this.xrCapJobGrade.Text = "직위";
        this.xrValJobGrade.LocationFloat = new DevExpress.Utils.PointFloat(95F, 175F);
        this.xrValJobGrade.Name = "xrValJobGrade";
        this.xrValJobGrade.SizeF = new System.Drawing.SizeF(220F, 20F);
        this.xrValJobGrade.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[job_grade]")});
        this.xrCapMobile.LocationFloat = new DevExpress.Utils.PointFloat(330F, 175F);
        this.xrCapMobile.Name = "xrCapMobile";
        this.xrCapMobile.SizeF = new System.Drawing.SizeF(90F, 20F);
        this.xrCapMobile.Text = "Mobile";
        this.xrValMobile.LocationFloat = new DevExpress.Utils.PointFloat(425F, 175F);
        this.xrValMobile.Name = "xrValMobile";
        this.xrValMobile.SizeF = new System.Drawing.SizeF(225F, 20F);
        this.xrValMobile.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[mobile]")});
        //
        // 이메일
        //
        this.xrCapEmail.LocationFloat = new DevExpress.Utils.PointFloat(0F, 205F);
        this.xrCapEmail.Name = "xrCapEmail";
        this.xrCapEmail.SizeF = new System.Drawing.SizeF(90F, 20F);
        this.xrCapEmail.Text = "E-mail";
        this.xrValEmail.LocationFloat = new DevExpress.Utils.PointFloat(95F, 205F);
        this.xrValEmail.Name = "xrValEmail";
        this.xrValEmail.SizeF = new System.Drawing.SizeF(220F, 20F);
        this.xrValEmail.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[email]")});
        //
        // 비고
        //
        this.xrCapRemark.LocationFloat = new DevExpress.Utils.PointFloat(0F, 235F);
        this.xrCapRemark.Name = "xrCapRemark";
        this.xrCapRemark.SizeF = new System.Drawing.SizeF(90F, 20F);
        this.xrCapRemark.Text = "비고";
        this.xrValRemark.CanGrow = true;
        this.xrValRemark.LocationFloat = new DevExpress.Utils.PointFloat(95F, 235F);
        this.xrValRemark.Name = "xrValRemark";
        this.xrValRemark.SizeF = new System.Drawing.SizeF(555F, 80F);
        this.xrValRemark.StylePriority.UseTextAlignment = false;
        this.xrValRemark.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrValRemark.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[remark]")});
        //
        // rptNameCardReq
        //
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
        this.Version = "21.2";
        ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
        this.ResumeLayout();
    }

    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
    private DevExpress.XtraReports.UI.XRLabel xrLabelTitle;
    private DevExpress.XtraReports.UI.XRLabel xrCapReqNo;
    private DevExpress.XtraReports.UI.XRLabel xrValReqNo;
    private DevExpress.XtraReports.UI.XRLabel xrCapRegDt;
    private DevExpress.XtraReports.UI.XRLabel xrValRegDt;
    private DevExpress.XtraReports.UI.XRLabel xrCapDeptNm;
    private DevExpress.XtraReports.UI.XRLabel xrValDeptNm;
    private DevExpress.XtraReports.UI.XRLabel xrCapEmpNm;
    private DevExpress.XtraReports.UI.XRLabel xrValEmpNm;
    private DevExpress.XtraReports.UI.XRLabel xrCapAppNo;
    private DevExpress.XtraReports.UI.XRLabel xrValAppNo;
    private DevExpress.XtraReports.UI.XRLabel xrCapApprStatCd;
    private DevExpress.XtraReports.UI.XRLabel xrValApprStatCd;
    private DevExpress.XtraReports.UI.XRLine xrLine1;
    private DevExpress.XtraReports.UI.XRLabel xrCapNameKor;
    private DevExpress.XtraReports.UI.XRLabel xrValNameKor;
    private DevExpress.XtraReports.UI.XRLabel xrCapNameEng;
    private DevExpress.XtraReports.UI.XRLabel xrValNameEng;
    private DevExpress.XtraReports.UI.XRLabel xrCapDeptKor;
    private DevExpress.XtraReports.UI.XRLabel xrValDeptKor;
    private DevExpress.XtraReports.UI.XRLabel xrCapDeptEng;
    private DevExpress.XtraReports.UI.XRLabel xrValDeptEng;
    private DevExpress.XtraReports.UI.XRLabel xrCapJobGrade;
    private DevExpress.XtraReports.UI.XRLabel xrValJobGrade;
    private DevExpress.XtraReports.UI.XRLabel xrCapMobile;
    private DevExpress.XtraReports.UI.XRLabel xrValMobile;
    private DevExpress.XtraReports.UI.XRLabel xrCapEmail;
    private DevExpress.XtraReports.UI.XRLabel xrValEmail;
    private DevExpress.XtraReports.UI.XRLabel xrCapRemark;
    private DevExpress.XtraReports.UI.XRLabel xrValRemark;
}
