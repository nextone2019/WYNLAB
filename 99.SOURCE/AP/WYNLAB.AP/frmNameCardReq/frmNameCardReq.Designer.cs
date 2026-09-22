// VS 디자이너가 자동 생성하는 필드 선언에는 = null!을 안 붙여서 nullable 경고(CS8618)가
// 계속 나기 때문에, 이 파일(디자이너 전용)만 nullable 검사를 끈다 - 흔한 관례.
#nullable disable
using DevExpress.XtraGrid;
using WYNLAB.Base.Controls;

namespace WYNLAB.AP;

/// <summary>
/// 명함신청서 화면 - frmAcc(TEMPLATE 표준 레이아웃: grd1 목록 + panData 상세)를 그대로 따르되
/// 필드가 많아서 panData를 2열로 배치한다. 필드를 더 늘릴 때는 panData에 라벨+입력컨트롤 쌍을
/// 그대로 이어 붙이면 된다(다음 Y좌표는 기존 필드보다 30만큼 아래).
/// </summary>
public partial class frmNameCardReq
{
    private System.ComponentModel.IContainer components = null;
    private PanelWyn panBase = null!;

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
        this.panBase = new WYNLAB.Base.Controls.PanelWyn();
        this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
        this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.colReqNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colNameKor = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDeptKor = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colApprStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
        this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
        this.panData = new WYNLAB.Base.Controls.PanelWyn();
        this.lblReqNo = new DevExpress.XtraEditors.LabelControl();
        this.txtReqNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblStatCd = new DevExpress.XtraEditors.LabelControl();
        this.txtStatCd = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblApprStatCd = new DevExpress.XtraEditors.LabelControl();
        this.txtApprStatCd = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblRegDt = new DevExpress.XtraEditors.LabelControl();
        this.txtRegDt = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDeptNm = new DevExpress.XtraEditors.LabelControl();
        this.txtDeptNm = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblReqEmpNm = new DevExpress.XtraEditors.LabelControl();
        this.txtReqEmpNm = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblAppNo = new DevExpress.XtraEditors.LabelControl();
        this.txtAppNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblNameKor = new DevExpress.XtraEditors.LabelControl();
        this.txtNameKor = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblNameEng = new DevExpress.XtraEditors.LabelControl();
        this.txtNameEng = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDeptKor = new DevExpress.XtraEditors.LabelControl();
        this.txtDeptKor = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDeptEng = new DevExpress.XtraEditors.LabelControl();
        this.txtDeptEng = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblJobGrade = new DevExpress.XtraEditors.LabelControl();
        this.txtJobGrade = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblMobile = new DevExpress.XtraEditors.LabelControl();
        this.txtMobile = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblEmail = new DevExpress.XtraEditors.LabelControl();
        this.txtEmail = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblApprover = new DevExpress.XtraEditors.LabelControl();
        this.txtApprover = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.txtApproverEmpNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblRemark = new DevExpress.XtraEditors.LabelControl();
        this.memoRemark = new WYNLAB.Base.Controls.MemoEditWyn();
        this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.txtSearchQ = new WYNLAB.Base.Controls.TextEditWyn();
        this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
        this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        this.panBase.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
        this.panelWyn3.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).BeginInit();
        this.panelWyn8.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
        this.panelWyn2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
        this.panelWyn5.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
        this.panData.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtReqNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtStatCd.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtApprStatCd.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtRegDt.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtReqEmpNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtAppNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtNameKor.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtNameEng.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptKor.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptEng.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtJobGrade.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtMobile.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmail.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtApprover.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtApproverEmpNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
        this.panelWyn6.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchQ.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.paTitle)).BeginInit();
        this.paTitle.SuspendLayout();
        this.SuspendLayout();
        //
        // panBase
        //
        this.panBase.Appearance.BackColor = System.Drawing.Color.White;
        this.panBase.Appearance.Options.UseBackColor = true;
        this.panBase.Controls.Add(this.panelWyn3);
        this.panBase.Controls.Add(this.panHeader);
        this.panBase.Controls.Add(this.paTitle);
        this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panBase.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panBase.Location = new System.Drawing.Point(0, 0);
        this.panBase.Name = "panBase";
        this.panBase.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
        this.panBase.Size = new System.Drawing.Size(1200, 640);
        this.panBase.TabIndex = 5;
        //
        // panelWyn3
        //
        this.panelWyn3.Controls.Add(this.panelWyn5);
        this.panelWyn3.Controls.Add(this.splitterWyn1);
        this.panelWyn3.Controls.Add(this.panelWyn8);
        this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panelWyn3.Location = new System.Drawing.Point(3, 82);
        this.panelWyn3.Name = "panelWyn3";
        this.panelWyn3.Size = new System.Drawing.Size(1194, 555);
        this.panelWyn3.TabIndex = 7;
        //
        // panelWyn8 (좌측 - 목록)
        //
        this.panelWyn8.Controls.Add(this.grd1);
        this.panelWyn8.Controls.Add(this.panelWyn2);
        this.panelWyn8.Dock = System.Windows.Forms.DockStyle.Left;
        this.panelWyn8.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panelWyn8.Location = new System.Drawing.Point(0, 0);
        this.panelWyn8.Name = "panelWyn8";
        this.panelWyn8.Size = new System.Drawing.Size(380, 555);
        this.panelWyn8.TabIndex = 12;
        //
        // grd1 (목록)
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd1.Location = new System.Drawing.Point(0, 27);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.Size = new System.Drawing.Size(380, 528);
        this.grd1.TabIndex = 10;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colReqNo,
        this.colNameKor,
        this.colDeptKor,
        this.colStatCd,
        this.colApprStatCd});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsSelection.InvertSelection = true;
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        //
        // colReqNo (신청번호)
        //
        this.colReqNo.Caption = "신청번호";
        this.colReqNo.FieldName = "req_no";
        this.colReqNo.Name = "colReqNo";
        this.colReqNo.Visible = true;
        this.colReqNo.VisibleIndex = 0;
        this.colReqNo.Width = 90;
        //
        // colNameKor (성명)
        //
        this.colNameKor.Caption = "성명";
        this.colNameKor.FieldName = "name_kor";
        this.colNameKor.Name = "colNameKor";
        this.colNameKor.Visible = true;
        this.colNameKor.VisibleIndex = 1;
        this.colNameKor.Width = 70;
        //
        // colDeptKor (부서명)
        //
        this.colDeptKor.Caption = "부서명";
        this.colDeptKor.FieldName = "dept_kor";
        this.colDeptKor.Name = "colDeptKor";
        this.colDeptKor.Visible = true;
        this.colDeptKor.VisibleIndex = 2;
        this.colDeptKor.Width = 90;
        //
        // colStatCd (진행상태)
        //
        this.colStatCd.Caption = "진행상태";
        this.colStatCd.FieldName = "stat_cd";
        this.colStatCd.Name = "colStatCd";
        this.colStatCd.Visible = true;
        this.colStatCd.VisibleIndex = 3;
        this.colStatCd.Width = 60;
        //
        // colApprStatCd (결재상태)
        //
        this.colApprStatCd.Caption = "결재상태";
        this.colApprStatCd.FieldName = "appr_stat_cd";
        this.colApprStatCd.Name = "colApprStatCd";
        this.colApprStatCd.Visible = true;
        this.colApprStatCd.VisibleIndex = 4;
        this.colApprStatCd.Width = 60;
        //
        // panelWyn2
        //
        this.panelWyn2.Controls.Add(this.sectionHeaderWyn4);
        this.panelWyn2.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn2.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panelWyn2.Location = new System.Drawing.Point(0, 0);
        this.panelWyn2.Name = "panelWyn2";
        this.panelWyn2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
        this.panelWyn2.Size = new System.Drawing.Size(380, 27);
        this.panelWyn2.TabIndex = 11;
        //
        // sectionHeaderWyn4
        //
        this.sectionHeaderWyn4.BackColor = System.Drawing.Color.White;
        this.sectionHeaderWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
        this.sectionHeaderWyn4.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.sectionHeaderWyn4.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
        this.sectionHeaderWyn4.Location = new System.Drawing.Point(5, 0);
        this.sectionHeaderWyn4.Name = "sectionHeaderWyn4";
        this.sectionHeaderWyn4.Size = new System.Drawing.Size(375, 25);
        this.sectionHeaderWyn4.TabIndex = 8;
        this.sectionHeaderWyn4.Text = "목록";
        //
        // splitterWyn1
        //
        this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        this.splitterWyn1.Location = new System.Drawing.Point(380, 0);
        this.splitterWyn1.Name = "splitterWyn1";
        this.splitterWyn1.Size = new System.Drawing.Size(10, 555);
        this.splitterWyn1.TabIndex = 9;
        this.splitterWyn1.TabStop = false;
        //
        // panelWyn5 (우측 - 상세 등록)
        //
        this.panelWyn5.Controls.Add(this.panData);
        this.panelWyn5.Controls.Add(this.panelWyn6);
        this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panelWyn5.Location = new System.Drawing.Point(390, 0);
        this.panelWyn5.Name = "panelWyn5";
        this.panelWyn5.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
        this.panelWyn5.Size = new System.Drawing.Size(804, 555);
        this.panelWyn5.TabIndex = 6;
        //
        // panData (상세 입력)
        //
        this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        this.panData.Controls.Add(this.lblReqNo);
        this.panData.Controls.Add(this.txtReqNo);
        this.panData.Controls.Add(this.lblStatCd);
        this.panData.Controls.Add(this.txtStatCd);
        this.panData.Controls.Add(this.lblApprStatCd);
        this.panData.Controls.Add(this.txtApprStatCd);
        this.panData.Controls.Add(this.lblRegDt);
        this.panData.Controls.Add(this.txtRegDt);
        this.panData.Controls.Add(this.lblDeptNm);
        this.panData.Controls.Add(this.txtDeptNm);
        this.panData.Controls.Add(this.lblReqEmpNm);
        this.panData.Controls.Add(this.txtReqEmpNm);
        this.panData.Controls.Add(this.lblAppNo);
        this.panData.Controls.Add(this.txtAppNo);
        this.panData.Controls.Add(this.lblNameKor);
        this.panData.Controls.Add(this.txtNameKor);
        this.panData.Controls.Add(this.lblNameEng);
        this.panData.Controls.Add(this.txtNameEng);
        this.panData.Controls.Add(this.lblDeptKor);
        this.panData.Controls.Add(this.txtDeptKor);
        this.panData.Controls.Add(this.lblDeptEng);
        this.panData.Controls.Add(this.txtDeptEng);
        this.panData.Controls.Add(this.lblJobGrade);
        this.panData.Controls.Add(this.txtJobGrade);
        this.panData.Controls.Add(this.lblMobile);
        this.panData.Controls.Add(this.txtMobile);
        this.panData.Controls.Add(this.lblEmail);
        this.panData.Controls.Add(this.txtEmail);
        this.panData.Controls.Add(this.lblApprover);
        this.panData.Controls.Add(this.txtApprover);
        this.panData.Controls.Add(this.txtApproverEmpNo);
        this.panData.Controls.Add(this.lblRemark);
        this.panData.Controls.Add(this.memoRemark);
        this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panData.Location = new System.Drawing.Point(3, 27);
        this.panData.Name = "panData";
        this.panData.Size = new System.Drawing.Size(801, 528);
        this.panData.TabIndex = 8;
        //
        // lblReqNo
        //
        this.lblReqNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblReqNo.Appearance.Options.UseFont = true;
        this.lblReqNo.Location = new System.Drawing.Point(11, 20);
        this.lblReqNo.Name = "lblReqNo";
        this.lblReqNo.Size = new System.Drawing.Size(75, 15);
        this.lblReqNo.TabIndex = 0;
        this.lblReqNo.Text = "신청번호";
        //
        // txtReqNo
        //
        this.txtReqNo.Location = new System.Drawing.Point(95, 18);
        this.txtReqNo.Name = "txtReqNo";
        this.txtReqNo.ReadOnly = true;
        this.txtReqNo.Size = new System.Drawing.Size(165, 20);
        this.txtReqNo.TabIndex = 1;
        //
        // lblStatCd
        //
        this.lblStatCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblStatCd.Appearance.Options.UseFont = true;
        this.lblStatCd.Location = new System.Drawing.Point(280, 20);
        this.lblStatCd.Name = "lblStatCd";
        this.lblStatCd.Size = new System.Drawing.Size(75, 15);
        this.lblStatCd.TabIndex = 2;
        this.lblStatCd.Text = "진행상태";
        //
        // txtStatCd
        //
        this.txtStatCd.Location = new System.Drawing.Point(364, 18);
        this.txtStatCd.Name = "txtStatCd";
        this.txtStatCd.ReadOnly = true;
        this.txtStatCd.Size = new System.Drawing.Size(200, 20);
        this.txtStatCd.TabIndex = 3;
        //
        // lblApprStatCd
        //
        this.lblApprStatCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblApprStatCd.Appearance.Options.UseFont = true;
        this.lblApprStatCd.Location = new System.Drawing.Point(11, 50);
        this.lblApprStatCd.Name = "lblApprStatCd";
        this.lblApprStatCd.Size = new System.Drawing.Size(75, 15);
        this.lblApprStatCd.TabIndex = 4;
        this.lblApprStatCd.Text = "결재상태";
        //
        // txtApprStatCd
        //
        this.txtApprStatCd.Location = new System.Drawing.Point(95, 48);
        this.txtApprStatCd.Name = "txtApprStatCd";
        this.txtApprStatCd.ReadOnly = true;
        this.txtApprStatCd.Size = new System.Drawing.Size(165, 20);
        this.txtApprStatCd.TabIndex = 5;
        //
        // lblRegDt
        //
        this.lblRegDt.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblRegDt.Appearance.Options.UseFont = true;
        this.lblRegDt.Location = new System.Drawing.Point(280, 50);
        this.lblRegDt.Name = "lblRegDt";
        this.lblRegDt.Size = new System.Drawing.Size(75, 15);
        this.lblRegDt.TabIndex = 6;
        this.lblRegDt.Text = "등록일시";
        //
        // txtRegDt
        //
        this.txtRegDt.Location = new System.Drawing.Point(364, 48);
        this.txtRegDt.Name = "txtRegDt";
        this.txtRegDt.ReadOnly = true;
        this.txtRegDt.Size = new System.Drawing.Size(200, 20);
        this.txtRegDt.TabIndex = 7;
        //
        // lblDeptNm
        //
        this.lblDeptNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDeptNm.Appearance.Options.UseFont = true;
        this.lblDeptNm.Location = new System.Drawing.Point(11, 80);
        this.lblDeptNm.Name = "lblDeptNm";
        this.lblDeptNm.Size = new System.Drawing.Size(75, 15);
        this.lblDeptNm.TabIndex = 8;
        this.lblDeptNm.Text = "신청부서";
        //
        // txtDeptNm
        //
        this.txtDeptNm.Location = new System.Drawing.Point(95, 78);
        this.txtDeptNm.Name = "txtDeptNm";
        this.txtDeptNm.ReadOnly = true;
        this.txtDeptNm.Size = new System.Drawing.Size(165, 20);
        this.txtDeptNm.TabIndex = 9;
        //
        // lblReqEmpNm
        //
        this.lblReqEmpNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblReqEmpNm.Appearance.Options.UseFont = true;
        this.lblReqEmpNm.Location = new System.Drawing.Point(280, 80);
        this.lblReqEmpNm.Name = "lblReqEmpNm";
        this.lblReqEmpNm.Size = new System.Drawing.Size(75, 15);
        this.lblReqEmpNm.TabIndex = 10;
        this.lblReqEmpNm.Text = "신청자";
        //
        // txtReqEmpNm
        //
        this.txtReqEmpNm.Location = new System.Drawing.Point(364, 78);
        this.txtReqEmpNm.Name = "txtReqEmpNm";
        this.txtReqEmpNm.ReadOnly = true;
        this.txtReqEmpNm.Size = new System.Drawing.Size(200, 20);
        this.txtReqEmpNm.TabIndex = 11;
        //
        // lblAppNo
        //
        this.lblAppNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblAppNo.Appearance.Options.UseFont = true;
        this.lblAppNo.Location = new System.Drawing.Point(11, 110);
        this.lblAppNo.Name = "lblAppNo";
        this.lblAppNo.Size = new System.Drawing.Size(75, 15);
        this.lblAppNo.TabIndex = 12;
        this.lblAppNo.Text = "결재번호";
        //
        // txtAppNo
        //
        this.txtAppNo.Location = new System.Drawing.Point(95, 108);
        this.txtAppNo.Name = "txtAppNo";
        this.txtAppNo.ReadOnly = true;
        this.txtAppNo.Size = new System.Drawing.Size(165, 20);
        this.txtAppNo.TabIndex = 13;
        //
        // lblNameKor
        //
        this.lblNameKor.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblNameKor.Appearance.Options.UseFont = true;
        this.lblNameKor.Location = new System.Drawing.Point(11, 150);
        this.lblNameKor.Name = "lblNameKor";
        this.lblNameKor.Size = new System.Drawing.Size(75, 15);
        this.lblNameKor.TabIndex = 14;
        this.lblNameKor.Text = "성명(한글)";
        //
        // txtNameKor
        //
        this.txtNameKor.Location = new System.Drawing.Point(95, 148);
        this.txtNameKor.Name = "txtNameKor";
        this.txtNameKor.Required = true;
        this.txtNameKor.Size = new System.Drawing.Size(165, 20);
        this.txtNameKor.TabIndex = 15;
        //
        // lblNameEng
        //
        this.lblNameEng.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblNameEng.Appearance.Options.UseFont = true;
        this.lblNameEng.Location = new System.Drawing.Point(280, 150);
        this.lblNameEng.Name = "lblNameEng";
        this.lblNameEng.Size = new System.Drawing.Size(75, 15);
        this.lblNameEng.TabIndex = 16;
        this.lblNameEng.Text = "성명(영문)";
        //
        // txtNameEng
        //
        this.txtNameEng.Location = new System.Drawing.Point(364, 148);
        this.txtNameEng.Name = "txtNameEng";
        this.txtNameEng.Size = new System.Drawing.Size(200, 20);
        this.txtNameEng.TabIndex = 17;
        //
        // lblDeptKor
        //
        this.lblDeptKor.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDeptKor.Appearance.Options.UseFont = true;
        this.lblDeptKor.Location = new System.Drawing.Point(11, 180);
        this.lblDeptKor.Name = "lblDeptKor";
        this.lblDeptKor.Size = new System.Drawing.Size(75, 15);
        this.lblDeptKor.TabIndex = 18;
        this.lblDeptKor.Text = "부서명(한글)";
        //
        // txtDeptKor
        //
        this.txtDeptKor.Location = new System.Drawing.Point(95, 178);
        this.txtDeptKor.Name = "txtDeptKor";
        this.txtDeptKor.Size = new System.Drawing.Size(165, 20);
        this.txtDeptKor.TabIndex = 19;
        //
        // lblDeptEng
        //
        this.lblDeptEng.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDeptEng.Appearance.Options.UseFont = true;
        this.lblDeptEng.Location = new System.Drawing.Point(280, 180);
        this.lblDeptEng.Name = "lblDeptEng";
        this.lblDeptEng.Size = new System.Drawing.Size(75, 15);
        this.lblDeptEng.TabIndex = 20;
        this.lblDeptEng.Text = "부서명(영문)";
        //
        // txtDeptEng
        //
        this.txtDeptEng.Location = new System.Drawing.Point(364, 178);
        this.txtDeptEng.Name = "txtDeptEng";
        this.txtDeptEng.Size = new System.Drawing.Size(200, 20);
        this.txtDeptEng.TabIndex = 21;
        //
        // lblJobGrade
        //
        this.lblJobGrade.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblJobGrade.Appearance.Options.UseFont = true;
        this.lblJobGrade.Location = new System.Drawing.Point(11, 210);
        this.lblJobGrade.Name = "lblJobGrade";
        this.lblJobGrade.Size = new System.Drawing.Size(75, 15);
        this.lblJobGrade.TabIndex = 22;
        this.lblJobGrade.Text = "직위";
        //
        // txtJobGrade
        //
        this.txtJobGrade.Location = new System.Drawing.Point(95, 208);
        this.txtJobGrade.Name = "txtJobGrade";
        this.txtJobGrade.Size = new System.Drawing.Size(165, 20);
        this.txtJobGrade.TabIndex = 23;
        //
        // lblMobile
        //
        this.lblMobile.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblMobile.Appearance.Options.UseFont = true;
        this.lblMobile.Location = new System.Drawing.Point(280, 210);
        this.lblMobile.Name = "lblMobile";
        this.lblMobile.Size = new System.Drawing.Size(75, 15);
        this.lblMobile.TabIndex = 24;
        this.lblMobile.Text = "Mobile";
        //
        // txtMobile
        //
        this.txtMobile.Location = new System.Drawing.Point(364, 208);
        this.txtMobile.Name = "txtMobile";
        this.txtMobile.Size = new System.Drawing.Size(200, 20);
        this.txtMobile.TabIndex = 25;
        //
        // lblEmail
        //
        this.lblEmail.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblEmail.Appearance.Options.UseFont = true;
        this.lblEmail.Location = new System.Drawing.Point(11, 240);
        this.lblEmail.Name = "lblEmail";
        this.lblEmail.Size = new System.Drawing.Size(75, 15);
        this.lblEmail.TabIndex = 26;
        this.lblEmail.Text = "E-mail";
        //
        // txtEmail
        //
        this.txtEmail.Location = new System.Drawing.Point(95, 238);
        this.txtEmail.Name = "txtEmail";
        this.txtEmail.Size = new System.Drawing.Size(165, 20);
        this.txtEmail.TabIndex = 27;
        //
        // lblApprover
        //
        this.lblApprover.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblApprover.Appearance.Options.UseFont = true;
        this.lblApprover.Location = new System.Drawing.Point(280, 240);
        this.lblApprover.Name = "lblApprover";
        this.lblApprover.Size = new System.Drawing.Size(75, 15);
        this.lblApprover.TabIndex = 28;
        this.lblApprover.Text = "결재자";
        //
        // txtApprover (결재상신 시 승인자 선택 - P_EMP 팝업)
        //
        this.txtApprover.Location = new System.Drawing.Point(364, 238);
        this.txtApprover.LookupKey = "P_EMP";
        this.txtApprover.MatchField = "emp_nm";
        this.txtApprover.Name = "txtApprover";
        this.txtApprover.Size = new System.Drawing.Size(200, 20);
        this.txtApprover.TabIndex = 29;
        //
        // txtApproverEmpNo (화면에 안 보이는 숨김 필드 - txtApprover가 P_EMP에서 고른 emp_no를 여기 채움)
        //
        this.txtApproverEmpNo.Location = new System.Drawing.Point(364, 264);
        this.txtApproverEmpNo.Name = "txtApproverEmpNo";
        this.txtApproverEmpNo.Size = new System.Drawing.Size(200, 20);
        this.txtApproverEmpNo.TabIndex = 30;
        this.txtApproverEmpNo.Visible = false;
        //
        // lblRemark
        //
        this.lblRemark.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblRemark.Appearance.Options.UseFont = true;
        this.lblRemark.Location = new System.Drawing.Point(11, 270);
        this.lblRemark.Name = "lblRemark";
        this.lblRemark.Size = new System.Drawing.Size(75, 15);
        this.lblRemark.TabIndex = 31;
        this.lblRemark.Text = "비고";
        //
        // memoRemark
        //
        this.memoRemark.Location = new System.Drawing.Point(95, 270);
        this.memoRemark.Name = "memoRemark";
        this.memoRemark.Size = new System.Drawing.Size(469, 100);
        this.memoRemark.TabIndex = 32;
        //
        // panelWyn6
        //
        this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
        this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panelWyn6.Location = new System.Drawing.Point(3, 0);
        this.panelWyn6.Name = "panelWyn6";
        this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
        this.panelWyn6.Size = new System.Drawing.Size(801, 27);
        this.panelWyn6.TabIndex = 7;
        //
        // sectionHeaderWyn3
        //
        this.sectionHeaderWyn3.BackColor = System.Drawing.Color.White;
        this.sectionHeaderWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
        this.sectionHeaderWyn3.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.sectionHeaderWyn3.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
        this.sectionHeaderWyn3.Location = new System.Drawing.Point(5, 0);
        this.sectionHeaderWyn3.Name = "sectionHeaderWyn3";
        this.sectionHeaderWyn3.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
        this.sectionHeaderWyn3.Size = new System.Drawing.Size(796, 25);
        this.sectionHeaderWyn3.TabIndex = 8;
        this.sectionHeaderWyn3.Text = "상세 등록";
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        this.panHeader.Controls.Add(this.txtSearchQ);
        this.panHeader.Controls.Add(this.labelControl1);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panHeader.Location = new System.Drawing.Point(3, 33);
        this.panHeader.Name = "panHeader";
        this.panHeader.Size = new System.Drawing.Size(1194, 49);
        this.panHeader.TabIndex = 8;
        //
        // txtSearchQ
        //
        this.txtSearchQ.Location = new System.Drawing.Point(106, 15);
        this.txtSearchQ.Name = "txtSearchQ";
        this.txtSearchQ.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
        this.txtSearchQ.Size = new System.Drawing.Size(265, 20);
        this.txtSearchQ.TabIndex = 0;
        //
        // labelControl1
        //
        this.labelControl1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.labelControl1.Appearance.Options.UseFont = true;
        this.labelControl1.Location = new System.Drawing.Point(25, 18);
        this.labelControl1.Name = "labelControl1";
        this.labelControl1.Size = new System.Drawing.Size(77, 15);
        this.labelControl1.TabIndex = 0;
        this.labelControl1.Text = "신청번호";
        //
        // paTitle
        //
        this.paTitle.Controls.Add(this.sectionHeaderWyn1);
        this.paTitle.Dock = System.Windows.Forms.DockStyle.Top;
        this.paTitle.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.paTitle.Location = new System.Drawing.Point(3, 0);
        this.paTitle.Name = "paTitle";
        this.paTitle.Size = new System.Drawing.Size(1194, 33);
        this.paTitle.TabIndex = 5;
        //
        // sectionHeaderWyn1
        //
        this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
        this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
        this.sectionHeaderWyn1.Location = new System.Drawing.Point(5, 6);
        this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
        this.sectionHeaderWyn1.Size = new System.Drawing.Size(209, 23);
        this.sectionHeaderWyn1.TabIndex = 8;
        this.sectionHeaderWyn1.Text = "명함신청서";
        //
        // frmNameCardReq
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1200, 640);
        this.Controls.Add(this.panBase);
        this.Name = "frmNameCardReq";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
        this.panelWyn3.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).EndInit();
        this.panelWyn8.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
        this.panelWyn2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
        this.panelWyn5.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
        this.panData.ResumeLayout(false);
        this.panData.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtReqNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtStatCd.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtApprStatCd.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtRegDt.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtReqEmpNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtAppNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtNameKor.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtNameEng.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptKor.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptEng.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtJobGrade.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtMobile.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmail.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtApprover.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtApproverEmpNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
        this.panelWyn6.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        this.panHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchQ.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
        this.paTitle.ResumeLayout(false);
        this.ResumeLayout(false);

    }

    private PanelWyn paTitle;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private PanelWyn panelWyn6;
    private SectionHeaderWyn sectionHeaderWyn1;
    private SplitterWyn splitterWyn1;
    private SectionHeaderWyn sectionHeaderWyn3;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colReqNo;
    private DevExpress.XtraGrid.Columns.GridColumn colNameKor;
    private DevExpress.XtraGrid.Columns.GridColumn colDeptKor;
    private DevExpress.XtraGrid.Columns.GridColumn colStatCd;
    private DevExpress.XtraGrid.Columns.GridColumn colApprStatCd;
    private PanelWyn panelWyn2;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn panHeader;
    private TextEditWyn txtSearchQ;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private PanelWyn panelWyn8;
    private DevExpress.XtraEditors.LabelControl lblReqNo;
    private TextEditWyn txtReqNo;
    private DevExpress.XtraEditors.LabelControl lblStatCd;
    private TextEditWyn txtStatCd;
    private DevExpress.XtraEditors.LabelControl lblApprStatCd;
    private TextEditWyn txtApprStatCd;
    private DevExpress.XtraEditors.LabelControl lblRegDt;
    private TextEditWyn txtRegDt;
    private DevExpress.XtraEditors.LabelControl lblDeptNm;
    private TextEditWyn txtDeptNm;
    private DevExpress.XtraEditors.LabelControl lblReqEmpNm;
    private TextEditWyn txtReqEmpNm;
    private DevExpress.XtraEditors.LabelControl lblAppNo;
    private TextEditWyn txtAppNo;
    private DevExpress.XtraEditors.LabelControl lblNameKor;
    private TextEditWyn txtNameKor;
    private DevExpress.XtraEditors.LabelControl lblNameEng;
    private TextEditWyn txtNameEng;
    private DevExpress.XtraEditors.LabelControl lblDeptKor;
    private TextEditWyn txtDeptKor;
    private DevExpress.XtraEditors.LabelControl lblDeptEng;
    private TextEditWyn txtDeptEng;
    private DevExpress.XtraEditors.LabelControl lblJobGrade;
    private TextEditWyn txtJobGrade;
    private DevExpress.XtraEditors.LabelControl lblMobile;
    private TextEditWyn txtMobile;
    private DevExpress.XtraEditors.LabelControl lblEmail;
    private TextEditWyn txtEmail;
    private DevExpress.XtraEditors.LabelControl lblApprover;
    private PopupLookupEditWyn txtApprover;
    private TextEditWyn txtApproverEmpNo;
    private DevExpress.XtraEditors.LabelControl lblRemark;
    private MemoEditWyn memoRemark;
}
