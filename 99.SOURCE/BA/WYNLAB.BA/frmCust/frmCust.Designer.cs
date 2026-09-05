// AI Builder가 마스터-폼-탭그리드 템플릿을 복제해서 자동 생성 - 2026-09-04.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
#nullable disable
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmCust
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCust));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.tabDetailGrids = new WYNLAB.Base.Controls.TabControlWyn();
            this.tabDetail2 = new DevExpress.XtraTab.XtraTabPage();
            this.grd3 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw3 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colD2Serl = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD2BankCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD2AcntNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD2Remark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tabDetail1 = new DevExpress.XtraTab.XtraTabPage();
            this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colD1Serl = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1PrsnNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1Grade = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1Tel1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1Tel2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1Fax = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1Email = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDeletRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAddRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.lblDetailCustCd = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailCustCd = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailCustNm = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailCustNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailBizNo = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailBizNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailTel = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailTel = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailCurCd = new DevExpress.XtraEditors.LabelControl();
            this.cboDetailCurCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblDetailOwnerNm = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailOwnerNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailZipCode = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailZipCode = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailAddr1 = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailAddr1 = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailAddr2 = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailAddr2 = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailHomepage = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailHomepage = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailEmail = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailEmail = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailFax = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailFax = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailBizKind = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailBizKind = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailBizType = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailBizType = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailTransOpenDate = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailTransOpenDate = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailVatType = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailVatType = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailVatRate = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailVatRate = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailRemark = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailRemark = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailStatCd = new DevExpress.XtraEditors.LabelControl();
            this.cboDetailStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblDetailEmpNo = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailEmpNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colMCustNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMBizNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMTel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMCurCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMOwnerNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMZipCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMAddr1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMAddr2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMHomepage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMEmail = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMFax = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMBizKind = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMBizType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMTransOpenDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMVatType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMVatRate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMRemark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMEmpNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
            this.panelWyn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
            this.panelWyn4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabDetailGrids)).BeginInit();
            this.tabDetailGrids.SuspendLayout();
            this.tabDetail2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw3)).BeginInit();
            this.tabDetail1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailCustCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailCustNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailBizNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailTel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailCurCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailOwnerNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailZipCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailAddr1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailAddr2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailHomepage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailEmail.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailFax.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailBizKind.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailBizType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailTransOpenDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailVatType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailVatRate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailStatCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailEmpNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
            this.panelWyn6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).BeginInit();
            this.panelWyn8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
            this.panelWyn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
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
            this.panBase.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.panBase.Size = new System.Drawing.Size(1245, 580);
            this.panBase.TabIndex = 6;
            // 
            // panelWyn3
            // 
            this.panelWyn3.Controls.Add(this.panelWyn4);
            this.panelWyn3.Controls.Add(this.splitterWyn1);
            this.panelWyn3.Controls.Add(this.panelWyn8);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(5, 82);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Size = new System.Drawing.Size(1235, 493);
            this.panelWyn3.TabIndex = 7;
            // 
            // panelWyn4
            // 
            this.panelWyn4.Controls.Add(this.tabDetailGrids);
            this.panelWyn4.Controls.Add(this.panelWyn1);
            this.panelWyn4.Controls.Add(this.panelWyn5);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(412, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelWyn4.Size = new System.Drawing.Size(823, 493);
            this.panelWyn4.TabIndex = 7;
            // 
            // tabDetailGrids
            // 
            this.tabDetailGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabDetailGrids.Location = new System.Drawing.Point(3, 500);
            this.tabDetailGrids.Name = "tabDetailGrids";
            this.tabDetailGrids.SelectedTabPage = this.tabDetail2;
            this.tabDetailGrids.Size = new System.Drawing.Size(820, 0);
            this.tabDetailGrids.TabIndex = 7;
            this.tabDetailGrids.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabDetail1,
            this.tabDetail2});
            // 
            // tabDetail2
            // 
            this.tabDetail2.Controls.Add(this.grd3);
            this.tabDetail2.Name = "tabDetail2";
            this.tabDetail2.Size = new System.Drawing.Size(818, 222);
            this.tabDetail2.Text = "tabDetail2";
            // 
            // grd3
            // 
            this.grd3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd3.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grd3.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grd3.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grd3.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grd3.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grd3.Location = new System.Drawing.Point(0, 0);
            this.grd3.MainView = this.gvw3;
            this.grd3.Name = "grd3";
            this.grd3.Size = new System.Drawing.Size(818, 222);
            this.grd3.TabIndex = 0;
            this.grd3.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw3});
            // 
            // gvw3
            // 
            this.gvw3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colD2Serl,
            this.colD2BankCd,
            this.colD2AcntNo,
            this.colD2Remark});
            this.gvw3.GridControl = this.grd3;
            this.gvw3.HighlightFocusedRow = true;
            this.gvw3.Name = "gvw3";
            this.gvw3.OptionsBehavior.Editable = false;
            this.gvw3.OptionsView.ColumnAutoWidth = false;
            this.gvw3.OptionsView.ShowGroupPanel = false;
            // 
            // colD2Serl
            // 
            this.colD2Serl.Caption = "순번";
            this.colD2Serl.FieldName = "serl";
            this.colD2Serl.Name = "colD2Serl";
            this.colD2Serl.Visible = true;
            this.colD2Serl.VisibleIndex = 0;
            this.colD2Serl.Width = 100;
            // 
            // colD2BankCd
            // 
            this.colD2BankCd.Caption = "은행";
            this.colD2BankCd.FieldName = "bank_cd";
            this.colD2BankCd.Name = "colD2BankCd";
            this.colD2BankCd.Visible = true;
            this.colD2BankCd.VisibleIndex = 1;
            this.colD2BankCd.Width = 100;
            // 
            // colD2AcntNo
            // 
            this.colD2AcntNo.Caption = "계좌번호";
            this.colD2AcntNo.FieldName = "acnt_no";
            this.colD2AcntNo.Name = "colD2AcntNo";
            this.colD2AcntNo.Visible = true;
            this.colD2AcntNo.VisibleIndex = 2;
            this.colD2AcntNo.Width = 100;
            // 
            // colD2Remark
            // 
            this.colD2Remark.Caption = "비고";
            this.colD2Remark.FieldName = "remark";
            this.colD2Remark.Name = "colD2Remark";
            this.colD2Remark.Visible = true;
            this.colD2Remark.VisibleIndex = 3;
            this.colD2Remark.Width = 100;
            // 
            // tabDetail1
            // 
            this.tabDetail1.Controls.Add(this.grd2);
            this.tabDetail1.Name = "tabDetail1";
            this.tabDetail1.Size = new System.Drawing.Size(818, 0);
            this.tabDetail1.Text = "tabDetail1";
            // 
            // grd2
            // 
            this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd2.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grd2.Location = new System.Drawing.Point(0, 0);
            this.grd2.MainView = this.gvw2;
            this.grd2.Name = "grd2";
            this.grd2.Size = new System.Drawing.Size(818, 0);
            this.grd2.TabIndex = 0;
            this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw2});
            // 
            // gvw2
            // 
            this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colD1Serl,
            this.colD1PrsnNm,
            this.colD1Grade,
            this.colD1Tel1,
            this.colD1Tel2,
            this.colD1Fax,
            this.colD1Email});
            this.gvw2.GridControl = this.grd2;
            this.gvw2.HighlightFocusedRow = true;
            this.gvw2.Name = "gvw2";
            this.gvw2.OptionsBehavior.Editable = false;
            this.gvw2.OptionsView.ColumnAutoWidth = false;
            this.gvw2.OptionsView.ShowGroupPanel = false;
            // 
            // colD1Serl
            // 
            this.colD1Serl.Caption = "순번";
            this.colD1Serl.FieldName = "serl";
            this.colD1Serl.Name = "colD1Serl";
            this.colD1Serl.Visible = true;
            this.colD1Serl.VisibleIndex = 0;
            this.colD1Serl.Width = 100;
            // 
            // colD1PrsnNm
            // 
            this.colD1PrsnNm.Caption = "담당자명";
            this.colD1PrsnNm.FieldName = "prsn_nm";
            this.colD1PrsnNm.Name = "colD1PrsnNm";
            this.colD1PrsnNm.Visible = true;
            this.colD1PrsnNm.VisibleIndex = 1;
            this.colD1PrsnNm.Width = 100;
            // 
            // colD1Grade
            // 
            this.colD1Grade.Caption = "직급";
            this.colD1Grade.FieldName = "grade";
            this.colD1Grade.Name = "colD1Grade";
            this.colD1Grade.Visible = true;
            this.colD1Grade.VisibleIndex = 2;
            this.colD1Grade.Width = 100;
            // 
            // colD1Tel1
            // 
            this.colD1Tel1.Caption = "연락처1";
            this.colD1Tel1.FieldName = "tel1";
            this.colD1Tel1.Name = "colD1Tel1";
            this.colD1Tel1.Visible = true;
            this.colD1Tel1.VisibleIndex = 3;
            this.colD1Tel1.Width = 100;
            // 
            // colD1Tel2
            // 
            this.colD1Tel2.Caption = "연락처2";
            this.colD1Tel2.FieldName = "tel2";
            this.colD1Tel2.Name = "colD1Tel2";
            this.colD1Tel2.Visible = true;
            this.colD1Tel2.VisibleIndex = 4;
            this.colD1Tel2.Width = 100;
            // 
            // colD1Fax
            // 
            this.colD1Fax.Caption = "Fax";
            this.colD1Fax.FieldName = "fax";
            this.colD1Fax.Name = "colD1Fax";
            this.colD1Fax.Visible = true;
            this.colD1Fax.VisibleIndex = 5;
            this.colD1Fax.Width = 100;
            // 
            // colD1Email
            // 
            this.colD1Email.Caption = "E-mail";
            this.colD1Email.FieldName = "email";
            this.colD1Email.Name = "colD1Email";
            this.colD1Email.Visible = true;
            this.colD1Email.VisibleIndex = 6;
            this.colD1Email.Width = 100;
            // 
            // panelWyn1
            // 
            this.panelWyn1.Controls.Add(this.sectionHeaderWyn2);
            this.panelWyn1.Controls.Add(this.panelWyn7);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(3, 473);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn1.Size = new System.Drawing.Size(820, 27);
            this.panelWyn1.TabIndex = 8;
            // 
            // sectionHeaderWyn2
            // 
            this.sectionHeaderWyn2.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn2.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn2.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn2.Name = "sectionHeaderWyn2";
            this.sectionHeaderWyn2.Size = new System.Drawing.Size(747, 25);
            this.sectionHeaderWyn2.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn2.SvgIcon")));
            this.sectionHeaderWyn2.TabIndex = 10;
            this.sectionHeaderWyn2.Text = "하위 목록";
            // 
            // panelWyn7
            // 
            this.panelWyn7.Controls.Add(this.btnDeletRow2);
            this.panelWyn7.Controls.Add(this.btnAddRow2);
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(752, 0);
            this.panelWyn7.Name = "panelWyn7";
            this.panelWyn7.Size = new System.Drawing.Size(68, 25);
            this.panelWyn7.TabIndex = 9;
            // 
            // btnDeletRow2
            // 
            this.btnDeletRow2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeletRow2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(198)))), ((int)(((byte)(193)))));
            this.btnDeletRow2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeletRow2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(236)))), ((int)(((byte)(234)))));
            this.btnDeletRow2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDeletRow2.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDeletRow2.Image = null;
            this.btnDeletRow2.Location = new System.Drawing.Point(42, 2);
            this.btnDeletRow2.Name = "btnDeletRow2";
            this.btnDeletRow2.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDeletRow2.Size = new System.Drawing.Size(24, 22);
            this.btnDeletRow2.TabIndex = 0;
            this.btnDeletRow2.ToolTip = "행삭제(현재 탭)";
            // 
            // btnAddRow2
            // 
            this.btnAddRow2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddRow2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(231)))), ((int)(((byte)(203)))));
            this.btnAddRow2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddRow2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(245)))), ((int)(((byte)(231)))));
            this.btnAddRow2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnAddRow2.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnAddRow2.Image = null;
            this.btnAddRow2.Location = new System.Drawing.Point(10, 2);
            this.btnAddRow2.Name = "btnAddRow2";
            this.btnAddRow2.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnAddRow2.Size = new System.Drawing.Size(24, 22);
            this.btnAddRow2.TabIndex = 0;
            this.btnAddRow2.ToolTip = "행추가(현재 탭)";
            // 
            // panelWyn5
            // 
            this.panelWyn5.Controls.Add(this.panData);
            this.panelWyn5.Controls.Add(this.panelWyn6);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(3, 0);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Size = new System.Drawing.Size(820, 473);
            this.panelWyn5.TabIndex = 6;
            // 
            // panData
            // 
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Controls.Add(this.lblDetailCustCd);
            this.panData.Controls.Add(this.txtDetailCustCd);
            this.panData.Controls.Add(this.lblDetailCustNm);
            this.panData.Controls.Add(this.txtDetailCustNm);
            this.panData.Controls.Add(this.lblDetailBizNo);
            this.panData.Controls.Add(this.txtDetailBizNo);
            this.panData.Controls.Add(this.lblDetailTel);
            this.panData.Controls.Add(this.txtDetailTel);
            this.panData.Controls.Add(this.lblDetailCurCd);
            this.panData.Controls.Add(this.cboDetailCurCd);
            this.panData.Controls.Add(this.lblDetailOwnerNm);
            this.panData.Controls.Add(this.txtDetailOwnerNm);
            this.panData.Controls.Add(this.lblDetailZipCode);
            this.panData.Controls.Add(this.txtDetailZipCode);
            this.panData.Controls.Add(this.lblDetailAddr1);
            this.panData.Controls.Add(this.txtDetailAddr1);
            this.panData.Controls.Add(this.lblDetailAddr2);
            this.panData.Controls.Add(this.txtDetailAddr2);
            this.panData.Controls.Add(this.lblDetailHomepage);
            this.panData.Controls.Add(this.txtDetailHomepage);
            this.panData.Controls.Add(this.lblDetailEmail);
            this.panData.Controls.Add(this.txtDetailEmail);
            this.panData.Controls.Add(this.lblDetailFax);
            this.panData.Controls.Add(this.txtDetailFax);
            this.panData.Controls.Add(this.lblDetailBizKind);
            this.panData.Controls.Add(this.txtDetailBizKind);
            this.panData.Controls.Add(this.lblDetailBizType);
            this.panData.Controls.Add(this.txtDetailBizType);
            this.panData.Controls.Add(this.lblDetailTransOpenDate);
            this.panData.Controls.Add(this.txtDetailTransOpenDate);
            this.panData.Controls.Add(this.lblDetailVatType);
            this.panData.Controls.Add(this.txtDetailVatType);
            this.panData.Controls.Add(this.lblDetailVatRate);
            this.panData.Controls.Add(this.txtDetailVatRate);
            this.panData.Controls.Add(this.lblDetailRemark);
            this.panData.Controls.Add(this.txtDetailRemark);
            this.panData.Controls.Add(this.lblDetailStatCd);
            this.panData.Controls.Add(this.cboDetailStatCd);
            this.panData.Controls.Add(this.lblDetailEmpNo);
            this.panData.Controls.Add(this.txtDetailEmpNo);
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(820, 446);
            this.panData.TabIndex = 8;
            // 
            // lblDetailCustCd
            // 
            this.lblDetailCustCd.Location = new System.Drawing.Point(16, 19);
            this.lblDetailCustCd.Name = "lblDetailCustCd";
            this.lblDetailCustCd.Size = new System.Drawing.Size(50, 14);
            this.lblDetailCustCd.TabIndex = 0;
            this.lblDetailCustCd.Text = "거래처코드";
            // 
            // txtDetailCustCd
            // 
            this.txtDetailCustCd.Location = new System.Drawing.Point(120, 16);
            this.txtDetailCustCd.Name = "txtDetailCustCd";
            this.txtDetailCustCd.Size = new System.Drawing.Size(220, 20);
            this.txtDetailCustCd.TabIndex = 1;
            // 
            // lblDetailCustNm
            // 
            this.lblDetailCustNm.Location = new System.Drawing.Point(16, 47);
            this.lblDetailCustNm.Name = "lblDetailCustNm";
            this.lblDetailCustNm.Size = new System.Drawing.Size(30, 14);
            this.lblDetailCustNm.TabIndex = 2;
            this.lblDetailCustNm.Text = "거래처";
            // 
            // txtDetailCustNm
            // 
            this.txtDetailCustNm.Location = new System.Drawing.Point(120, 44);
            this.txtDetailCustNm.Name = "txtDetailCustNm";
            this.txtDetailCustNm.Size = new System.Drawing.Size(220, 20);
            this.txtDetailCustNm.TabIndex = 3;
            // 
            // lblDetailBizNo
            // 
            this.lblDetailBizNo.Location = new System.Drawing.Point(16, 75);
            this.lblDetailBizNo.Name = "lblDetailBizNo";
            this.lblDetailBizNo.Size = new System.Drawing.Size(50, 14);
            this.lblDetailBizNo.TabIndex = 4;
            this.lblDetailBizNo.Text = "사업자번호";
            // 
            // txtDetailBizNo
            // 
            this.txtDetailBizNo.Location = new System.Drawing.Point(120, 72);
            this.txtDetailBizNo.Name = "txtDetailBizNo";
            this.txtDetailBizNo.Size = new System.Drawing.Size(220, 20);
            this.txtDetailBizNo.TabIndex = 5;
            // 
            // lblDetailTel
            // 
            this.lblDetailTel.Location = new System.Drawing.Point(16, 103);
            this.lblDetailTel.Name = "lblDetailTel";
            this.lblDetailTel.Size = new System.Drawing.Size(30, 14);
            this.lblDetailTel.TabIndex = 6;
            this.lblDetailTel.Text = "연락처";
            // 
            // txtDetailTel
            // 
            this.txtDetailTel.Location = new System.Drawing.Point(120, 100);
            this.txtDetailTel.Name = "txtDetailTel";
            this.txtDetailTel.Size = new System.Drawing.Size(220, 20);
            this.txtDetailTel.TabIndex = 7;
            // 
            // lblDetailCurCd
            // 
            this.lblDetailCurCd.Location = new System.Drawing.Point(16, 131);
            this.lblDetailCurCd.Name = "lblDetailCurCd";
            this.lblDetailCurCd.Size = new System.Drawing.Size(20, 14);
            this.lblDetailCurCd.TabIndex = 8;
            this.lblDetailCurCd.Text = "통화";
            // 
            // cboDetailCurCd
            // 
            this.cboDetailCurCd.EditValue = "";
            this.cboDetailCurCd.Location = new System.Drawing.Point(120, 128);
            this.cboDetailCurCd.LookupKey = "L_CM0003";
            this.cboDetailCurCd.Name = "cboDetailCurCd";
            this.cboDetailCurCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailCurCd.Properties.NullText = "";
            this.cboDetailCurCd.Size = new System.Drawing.Size(220, 20);
            this.cboDetailCurCd.TabIndex = 9;
            // 
            // lblDetailOwnerNm
            // 
            this.lblDetailOwnerNm.Location = new System.Drawing.Point(16, 159);
            this.lblDetailOwnerNm.Name = "lblDetailOwnerNm";
            this.lblDetailOwnerNm.Size = new System.Drawing.Size(30, 14);
            this.lblDetailOwnerNm.TabIndex = 10;
            this.lblDetailOwnerNm.Text = "대표자";
            // 
            // txtDetailOwnerNm
            // 
            this.txtDetailOwnerNm.Location = new System.Drawing.Point(120, 156);
            this.txtDetailOwnerNm.Name = "txtDetailOwnerNm";
            this.txtDetailOwnerNm.Size = new System.Drawing.Size(220, 20);
            this.txtDetailOwnerNm.TabIndex = 11;
            // 
            // lblDetailZipCode
            // 
            this.lblDetailZipCode.Location = new System.Drawing.Point(16, 187);
            this.lblDetailZipCode.Name = "lblDetailZipCode";
            this.lblDetailZipCode.Size = new System.Drawing.Size(40, 14);
            this.lblDetailZipCode.TabIndex = 12;
            this.lblDetailZipCode.Text = "우편번호";
            // 
            // txtDetailZipCode
            // 
            this.txtDetailZipCode.Location = new System.Drawing.Point(120, 184);
            this.txtDetailZipCode.Name = "txtDetailZipCode";
            this.txtDetailZipCode.Size = new System.Drawing.Size(220, 20);
            this.txtDetailZipCode.TabIndex = 13;
            // 
            // lblDetailAddr1
            // 
            this.lblDetailAddr1.Location = new System.Drawing.Point(16, 215);
            this.lblDetailAddr1.Name = "lblDetailAddr1";
            this.lblDetailAddr1.Size = new System.Drawing.Size(27, 14);
            this.lblDetailAddr1.TabIndex = 14;
            this.lblDetailAddr1.Text = "주소1";
            // 
            // txtDetailAddr1
            // 
            this.txtDetailAddr1.Location = new System.Drawing.Point(120, 212);
            this.txtDetailAddr1.Name = "txtDetailAddr1";
            this.txtDetailAddr1.Size = new System.Drawing.Size(220, 20);
            this.txtDetailAddr1.TabIndex = 15;
            // 
            // lblDetailAddr2
            // 
            this.lblDetailAddr2.Location = new System.Drawing.Point(16, 243);
            this.lblDetailAddr2.Name = "lblDetailAddr2";
            this.lblDetailAddr2.Size = new System.Drawing.Size(27, 14);
            this.lblDetailAddr2.TabIndex = 16;
            this.lblDetailAddr2.Text = "주소2";
            // 
            // txtDetailAddr2
            // 
            this.txtDetailAddr2.Location = new System.Drawing.Point(120, 240);
            this.txtDetailAddr2.Name = "txtDetailAddr2";
            this.txtDetailAddr2.Size = new System.Drawing.Size(220, 20);
            this.txtDetailAddr2.TabIndex = 17;
            // 
            // lblDetailHomepage
            // 
            this.lblDetailHomepage.Location = new System.Drawing.Point(16, 271);
            this.lblDetailHomepage.Name = "lblDetailHomepage";
            this.lblDetailHomepage.Size = new System.Drawing.Size(59, 14);
            this.lblDetailHomepage.TabIndex = 18;
            this.lblDetailHomepage.Text = "Homepage";
            // 
            // txtDetailHomepage
            // 
            this.txtDetailHomepage.Location = new System.Drawing.Point(120, 268);
            this.txtDetailHomepage.Name = "txtDetailHomepage";
            this.txtDetailHomepage.Size = new System.Drawing.Size(220, 20);
            this.txtDetailHomepage.TabIndex = 19;
            // 
            // lblDetailEmail
            // 
            this.lblDetailEmail.Location = new System.Drawing.Point(16, 299);
            this.lblDetailEmail.Name = "lblDetailEmail";
            this.lblDetailEmail.Size = new System.Drawing.Size(31, 14);
            this.lblDetailEmail.TabIndex = 20;
            this.lblDetailEmail.Text = "E-mail";
            // 
            // txtDetailEmail
            // 
            this.txtDetailEmail.Location = new System.Drawing.Point(120, 296);
            this.txtDetailEmail.Name = "txtDetailEmail";
            this.txtDetailEmail.Size = new System.Drawing.Size(220, 20);
            this.txtDetailEmail.TabIndex = 21;
            // 
            // lblDetailFax
            // 
            this.lblDetailFax.Location = new System.Drawing.Point(16, 327);
            this.lblDetailFax.Name = "lblDetailFax";
            this.lblDetailFax.Size = new System.Drawing.Size(18, 14);
            this.lblDetailFax.TabIndex = 22;
            this.lblDetailFax.Text = "Fax";
            // 
            // txtDetailFax
            // 
            this.txtDetailFax.Location = new System.Drawing.Point(120, 324);
            this.txtDetailFax.Name = "txtDetailFax";
            this.txtDetailFax.Size = new System.Drawing.Size(220, 20);
            this.txtDetailFax.TabIndex = 23;
            // 
            // lblDetailBizKind
            // 
            this.lblDetailBizKind.Location = new System.Drawing.Point(16, 355);
            this.lblDetailBizKind.Name = "lblDetailBizKind";
            this.lblDetailBizKind.Size = new System.Drawing.Size(20, 14);
            this.lblDetailBizKind.TabIndex = 24;
            this.lblDetailBizKind.Text = "업종";
            // 
            // txtDetailBizKind
            // 
            this.txtDetailBizKind.Location = new System.Drawing.Point(120, 352);
            this.txtDetailBizKind.Name = "txtDetailBizKind";
            this.txtDetailBizKind.Size = new System.Drawing.Size(220, 20);
            this.txtDetailBizKind.TabIndex = 25;
            // 
            // lblDetailBizType
            // 
            this.lblDetailBizType.Location = new System.Drawing.Point(16, 383);
            this.lblDetailBizType.Name = "lblDetailBizType";
            this.lblDetailBizType.Size = new System.Drawing.Size(20, 14);
            this.lblDetailBizType.TabIndex = 26;
            this.lblDetailBizType.Text = "업태";
            // 
            // txtDetailBizType
            // 
            this.txtDetailBizType.Location = new System.Drawing.Point(120, 380);
            this.txtDetailBizType.Name = "txtDetailBizType";
            this.txtDetailBizType.Size = new System.Drawing.Size(220, 20);
            this.txtDetailBizType.TabIndex = 27;
            // 
            // lblDetailTransOpenDate
            // 
            this.lblDetailTransOpenDate.Location = new System.Drawing.Point(16, 411);
            this.lblDetailTransOpenDate.Name = "lblDetailTransOpenDate";
            this.lblDetailTransOpenDate.Size = new System.Drawing.Size(50, 14);
            this.lblDetailTransOpenDate.TabIndex = 28;
            this.lblDetailTransOpenDate.Text = "거래시작일";
            // 
            // txtDetailTransOpenDate
            // 
            this.txtDetailTransOpenDate.Location = new System.Drawing.Point(120, 408);
            this.txtDetailTransOpenDate.Name = "txtDetailTransOpenDate";
            this.txtDetailTransOpenDate.Size = new System.Drawing.Size(220, 20);
            this.txtDetailTransOpenDate.TabIndex = 29;
            // 
            // lblDetailVatType
            // 
            this.lblDetailVatType.Location = new System.Drawing.Point(16, 439);
            this.lblDetailVatType.Name = "lblDetailVatType";
            this.lblDetailVatType.Size = new System.Drawing.Size(50, 14);
            this.lblDetailVatType.TabIndex = 30;
            this.lblDetailVatType.Text = "부가세유형";
            // 
            // txtDetailVatType
            // 
            this.txtDetailVatType.Location = new System.Drawing.Point(120, 436);
            this.txtDetailVatType.Name = "txtDetailVatType";
            this.txtDetailVatType.Size = new System.Drawing.Size(220, 20);
            this.txtDetailVatType.TabIndex = 31;
            // 
            // lblDetailVatRate
            // 
            this.lblDetailVatRate.Location = new System.Drawing.Point(16, 467);
            this.lblDetailVatRate.Name = "lblDetailVatRate";
            this.lblDetailVatRate.Size = new System.Drawing.Size(40, 14);
            this.lblDetailVatRate.TabIndex = 32;
            this.lblDetailVatRate.Text = "부가세율";
            // 
            // txtDetailVatRate
            // 
            this.txtDetailVatRate.Location = new System.Drawing.Point(120, 464);
            this.txtDetailVatRate.Name = "txtDetailVatRate";
            this.txtDetailVatRate.Size = new System.Drawing.Size(220, 20);
            this.txtDetailVatRate.TabIndex = 33;
            // 
            // lblDetailRemark
            // 
            this.lblDetailRemark.Location = new System.Drawing.Point(16, 495);
            this.lblDetailRemark.Name = "lblDetailRemark";
            this.lblDetailRemark.Size = new System.Drawing.Size(20, 14);
            this.lblDetailRemark.TabIndex = 34;
            this.lblDetailRemark.Text = "비고";
            // 
            // txtDetailRemark
            // 
            this.txtDetailRemark.Location = new System.Drawing.Point(120, 492);
            this.txtDetailRemark.Name = "txtDetailRemark";
            this.txtDetailRemark.Size = new System.Drawing.Size(220, 20);
            this.txtDetailRemark.TabIndex = 35;
            // 
            // lblDetailStatCd
            // 
            this.lblDetailStatCd.Location = new System.Drawing.Point(16, 523);
            this.lblDetailStatCd.Name = "lblDetailStatCd";
            this.lblDetailStatCd.Size = new System.Drawing.Size(50, 14);
            this.lblDetailStatCd.TabIndex = 36;
            this.lblDetailStatCd.Text = "거래처상태";
            // 
            // cboDetailStatCd
            // 
            this.cboDetailStatCd.EditValue = "";
            this.cboDetailStatCd.Location = new System.Drawing.Point(120, 520);
            this.cboDetailStatCd.LookupKey = "L_BA0001";
            this.cboDetailStatCd.Name = "cboDetailStatCd";
            this.cboDetailStatCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailStatCd.Properties.NullText = "";
            this.cboDetailStatCd.Size = new System.Drawing.Size(220, 20);
            this.cboDetailStatCd.TabIndex = 37;
            // 
            // lblDetailEmpNo
            // 
            this.lblDetailEmpNo.Location = new System.Drawing.Point(16, 551);
            this.lblDetailEmpNo.Name = "lblDetailEmpNo";
            this.lblDetailEmpNo.Size = new System.Drawing.Size(60, 14);
            this.lblDetailEmpNo.TabIndex = 38;
            this.lblDetailEmpNo.Text = "거래처담당자";
            // 
            // txtDetailEmpNo
            // 
            this.txtDetailEmpNo.Location = new System.Drawing.Point(120, 548);
            this.txtDetailEmpNo.Name = "txtDetailEmpNo";
            this.txtDetailEmpNo.Size = new System.Drawing.Size(220, 20);
            this.txtDetailEmpNo.TabIndex = 39;
            // 
            // panelWyn6
            // 
            this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
            this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn6.Location = new System.Drawing.Point(0, 0);
            this.panelWyn6.Name = "panelWyn6";
            this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn6.Size = new System.Drawing.Size(820, 27);
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
            this.sectionHeaderWyn3.Size = new System.Drawing.Size(815, 25);
            this.sectionHeaderWyn3.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn3.SvgIcon")));
            this.sectionHeaderWyn3.TabIndex = 9;
            this.sectionHeaderWyn3.Text = "상세 등록";
            // 
            // splitterWyn1
            // 
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Location = new System.Drawing.Point(402, 0);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(10, 493);
            this.splitterWyn1.TabIndex = 9;
            this.splitterWyn1.TabStop = false;
            // 
            // panelWyn8
            // 
            this.panelWyn8.Controls.Add(this.grd1);
            this.panelWyn8.Controls.Add(this.panelWyn2);
            this.panelWyn8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelWyn8.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn8.Location = new System.Drawing.Point(0, 0);
            this.panelWyn8.Name = "panelWyn8";
            this.panelWyn8.Size = new System.Drawing.Size(402, 493);
            this.panelWyn8.TabIndex = 12;
            // 
            // grd1
            // 
            this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd1.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grd1.Location = new System.Drawing.Point(0, 27);
            this.grd1.MainView = this.gvw1;
            this.grd1.Name = "grd1";
            this.grd1.Size = new System.Drawing.Size(402, 466);
            this.grd1.TabIndex = 10;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colMCustNm,
            this.colMBizNo,
            this.colMTel,
            this.colMCurCd,
            this.colMOwnerNm,
            this.colMZipCode,
            this.colMAddr1,
            this.colMAddr2,
            this.colMHomepage,
            this.colMEmail,
            this.colMFax,
            this.colMBizKind,
            this.colMBizType,
            this.colMTransOpenDate,
            this.colMVatType,
            this.colMVatRate,
            this.colMRemark,
            this.colMStatCd,
            this.colMEmpNo});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightFocusedRow = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsSelection.InvertSelection = true;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // colMCustNm
            // 
            this.colMCustNm.Caption = "거래처";
            this.colMCustNm.FieldName = "cust_nm";
            this.colMCustNm.Name = "colMCustNm";
            this.colMCustNm.Visible = true;
            this.colMCustNm.VisibleIndex = 0;
            this.colMCustNm.Width = 100;
            // 
            // colMBizNo
            // 
            this.colMBizNo.Caption = "사업자번호";
            this.colMBizNo.FieldName = "biz_no";
            this.colMBizNo.Name = "colMBizNo";
            this.colMBizNo.Visible = true;
            this.colMBizNo.VisibleIndex = 1;
            this.colMBizNo.Width = 100;
            // 
            // colMTel
            // 
            this.colMTel.Caption = "연락처";
            this.colMTel.FieldName = "tel";
            this.colMTel.Name = "colMTel";
            this.colMTel.Visible = true;
            this.colMTel.VisibleIndex = 2;
            this.colMTel.Width = 100;
            // 
            // colMCurCd
            // 
            this.colMCurCd.Caption = "통화";
            this.colMCurCd.FieldName = "cur_cd";
            this.colMCurCd.Name = "colMCurCd";
            this.colMCurCd.Visible = true;
            this.colMCurCd.VisibleIndex = 3;
            this.colMCurCd.Width = 100;
            // 
            // colMOwnerNm
            // 
            this.colMOwnerNm.Caption = "대표자";
            this.colMOwnerNm.FieldName = "owner_nm";
            this.colMOwnerNm.Name = "colMOwnerNm";
            this.colMOwnerNm.Visible = true;
            this.colMOwnerNm.VisibleIndex = 4;
            this.colMOwnerNm.Width = 100;
            // 
            // colMZipCode
            // 
            this.colMZipCode.Caption = "우편번호";
            this.colMZipCode.FieldName = "zip_code";
            this.colMZipCode.Name = "colMZipCode";
            this.colMZipCode.Visible = true;
            this.colMZipCode.VisibleIndex = 5;
            this.colMZipCode.Width = 100;
            // 
            // colMAddr1
            // 
            this.colMAddr1.Caption = "주소1";
            this.colMAddr1.FieldName = "addr1";
            this.colMAddr1.Name = "colMAddr1";
            this.colMAddr1.Visible = true;
            this.colMAddr1.VisibleIndex = 6;
            this.colMAddr1.Width = 100;
            // 
            // colMAddr2
            // 
            this.colMAddr2.Caption = "주소2";
            this.colMAddr2.FieldName = "addr2";
            this.colMAddr2.Name = "colMAddr2";
            this.colMAddr2.Visible = true;
            this.colMAddr2.VisibleIndex = 7;
            this.colMAddr2.Width = 100;
            // 
            // colMHomepage
            // 
            this.colMHomepage.Caption = "Homepage";
            this.colMHomepage.FieldName = "homepage";
            this.colMHomepage.Name = "colMHomepage";
            this.colMHomepage.Visible = true;
            this.colMHomepage.VisibleIndex = 8;
            this.colMHomepage.Width = 100;
            // 
            // colMEmail
            // 
            this.colMEmail.Caption = "E-mail";
            this.colMEmail.FieldName = "email";
            this.colMEmail.Name = "colMEmail";
            this.colMEmail.Visible = true;
            this.colMEmail.VisibleIndex = 9;
            this.colMEmail.Width = 100;
            // 
            // colMFax
            // 
            this.colMFax.Caption = "Fax";
            this.colMFax.FieldName = "fax";
            this.colMFax.Name = "colMFax";
            this.colMFax.Visible = true;
            this.colMFax.VisibleIndex = 10;
            this.colMFax.Width = 100;
            // 
            // colMBizKind
            // 
            this.colMBizKind.Caption = "업종";
            this.colMBizKind.FieldName = "biz_kind";
            this.colMBizKind.Name = "colMBizKind";
            this.colMBizKind.Visible = true;
            this.colMBizKind.VisibleIndex = 11;
            this.colMBizKind.Width = 100;
            // 
            // colMBizType
            // 
            this.colMBizType.Caption = "업태";
            this.colMBizType.FieldName = "biz_type";
            this.colMBizType.Name = "colMBizType";
            this.colMBizType.Visible = true;
            this.colMBizType.VisibleIndex = 12;
            this.colMBizType.Width = 100;
            // 
            // colMTransOpenDate
            // 
            this.colMTransOpenDate.Caption = "거래시작일";
            this.colMTransOpenDate.FieldName = "trans_open_date";
            this.colMTransOpenDate.Name = "colMTransOpenDate";
            this.colMTransOpenDate.Visible = true;
            this.colMTransOpenDate.VisibleIndex = 13;
            this.colMTransOpenDate.Width = 100;
            // 
            // colMVatType
            // 
            this.colMVatType.Caption = "부가세유형";
            this.colMVatType.FieldName = "vat_type";
            this.colMVatType.Name = "colMVatType";
            this.colMVatType.Visible = true;
            this.colMVatType.VisibleIndex = 14;
            this.colMVatType.Width = 100;
            // 
            // colMVatRate
            // 
            this.colMVatRate.Caption = "부가세율";
            this.colMVatRate.FieldName = "vat_rate";
            this.colMVatRate.Name = "colMVatRate";
            this.colMVatRate.Visible = true;
            this.colMVatRate.VisibleIndex = 15;
            this.colMVatRate.Width = 100;
            // 
            // colMRemark
            // 
            this.colMRemark.Caption = "비고";
            this.colMRemark.FieldName = "remark";
            this.colMRemark.Name = "colMRemark";
            this.colMRemark.Visible = true;
            this.colMRemark.VisibleIndex = 16;
            this.colMRemark.Width = 100;
            // 
            // colMStatCd
            // 
            this.colMStatCd.Caption = "거래처상태";
            this.colMStatCd.FieldName = "stat_cd";
            this.colMStatCd.Name = "colMStatCd";
            this.colMStatCd.Visible = true;
            this.colMStatCd.VisibleIndex = 17;
            this.colMStatCd.Width = 100;
            // 
            // colMEmpNo
            // 
            this.colMEmpNo.Caption = "거래처담당자";
            this.colMEmpNo.FieldName = "emp_no";
            this.colMEmpNo.Name = "colMEmpNo";
            this.colMEmpNo.Visible = true;
            this.colMEmpNo.VisibleIndex = 18;
            this.colMEmpNo.Width = 100;
            // 
            // panelWyn2
            // 
            this.panelWyn2.Controls.Add(this.sectionHeaderWyn4);
            this.panelWyn2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn2.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn2.Location = new System.Drawing.Point(0, 0);
            this.panelWyn2.Name = "panelWyn2";
            this.panelWyn2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn2.Size = new System.Drawing.Size(402, 27);
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
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(397, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 9;
            this.sectionHeaderWyn4.Text = "목록";
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.labelControl1);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 33);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1235, 49);
            this.panHeader.TabIndex = 8;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(25, 18);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 15);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "검색조건";
            // 
            // paTitle
            // 
            this.paTitle.Controls.Add(this.sectionHeaderWyn1);
            this.paTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle.Location = new System.Drawing.Point(5, 0);
            this.paTitle.Name = "paTitle";
            this.paTitle.Size = new System.Drawing.Size(1235, 33);
            this.paTitle.TabIndex = 5;
            // 
            // sectionHeaderWyn1
            // 
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(0, 0);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1235, 33);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 9;
            this.sectionHeaderWyn1.Text = "거래처등록 [frmCust]";
            // 
            // frmCust
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1245, 580);
            this.Controls.Add(this.panBase);
            this.Name = "frmCust";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabDetailGrids)).EndInit();
            this.tabDetailGrids.ResumeLayout(false);
            this.tabDetail2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw3)).EndInit();
            this.tabDetail1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
            this.panelWyn7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
            this.panData.ResumeLayout(false);
            this.panData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailCustCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailCustNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailBizNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailTel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailCurCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailOwnerNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailZipCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailAddr1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailAddr2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailHomepage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailEmail.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailFax.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailBizKind.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailBizType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailTransOpenDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailVatType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailVatRate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailStatCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailEmpNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
            this.panelWyn6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).EndInit();
            this.panelWyn8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
            this.panelWyn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
            this.paTitle.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private PanelWyn panBase;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn4;
    private TabControlWyn tabDetailGrids;
    private DevExpress.XtraTab.XtraTabPage tabDetail1;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private DevExpress.XtraGrid.Columns.GridColumn colD1Serl;
    private DevExpress.XtraGrid.Columns.GridColumn colD1PrsnNm;
    private DevExpress.XtraGrid.Columns.GridColumn colD1Grade;
    private DevExpress.XtraGrid.Columns.GridColumn colD1Tel1;
    private DevExpress.XtraGrid.Columns.GridColumn colD1Tel2;
    private DevExpress.XtraGrid.Columns.GridColumn colD1Fax;
    private DevExpress.XtraGrid.Columns.GridColumn colD1Email;
    private DevExpress.XtraTab.XtraTabPage tabDetail2;
    private GridControlWyn grd3;
    private GridViewWyn gvw3;
    private DevExpress.XtraGrid.Columns.GridColumn colD2Serl;
    private DevExpress.XtraGrid.Columns.GridColumn colD2BankCd;
    private DevExpress.XtraGrid.Columns.GridColumn colD2AcntNo;
    private DevExpress.XtraGrid.Columns.GridColumn colD2Remark;
    private PanelWyn panelWyn1;
    private PanelWyn panelWyn7;
    private ButtonWyn btnDeletRow2;
    private ButtonWyn btnAddRow2;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblDetailCustCd;
    private TextEditWyn txtDetailCustCd;
    private DevExpress.XtraEditors.LabelControl lblDetailCustNm;
    private TextEditWyn txtDetailCustNm;
    private DevExpress.XtraEditors.LabelControl lblDetailBizNo;
    private TextEditWyn txtDetailBizNo;
    private DevExpress.XtraEditors.LabelControl lblDetailTel;
    private TextEditWyn txtDetailTel;
    private DevExpress.XtraEditors.LabelControl lblDetailCurCd;
    private LookUpEditWyn cboDetailCurCd;
    private DevExpress.XtraEditors.LabelControl lblDetailOwnerNm;
    private TextEditWyn txtDetailOwnerNm;
    private DevExpress.XtraEditors.LabelControl lblDetailZipCode;
    private TextEditWyn txtDetailZipCode;
    private DevExpress.XtraEditors.LabelControl lblDetailAddr1;
    private TextEditWyn txtDetailAddr1;
    private DevExpress.XtraEditors.LabelControl lblDetailAddr2;
    private TextEditWyn txtDetailAddr2;
    private DevExpress.XtraEditors.LabelControl lblDetailHomepage;
    private TextEditWyn txtDetailHomepage;
    private DevExpress.XtraEditors.LabelControl lblDetailEmail;
    private TextEditWyn txtDetailEmail;
    private DevExpress.XtraEditors.LabelControl lblDetailFax;
    private TextEditWyn txtDetailFax;
    private DevExpress.XtraEditors.LabelControl lblDetailBizKind;
    private TextEditWyn txtDetailBizKind;
    private DevExpress.XtraEditors.LabelControl lblDetailBizType;
    private TextEditWyn txtDetailBizType;
    private DevExpress.XtraEditors.LabelControl lblDetailTransOpenDate;
    private TextEditWyn txtDetailTransOpenDate;
    private DevExpress.XtraEditors.LabelControl lblDetailVatType;
    private TextEditWyn txtDetailVatType;
    private DevExpress.XtraEditors.LabelControl lblDetailVatRate;
    private TextEditWyn txtDetailVatRate;
    private DevExpress.XtraEditors.LabelControl lblDetailRemark;
    private TextEditWyn txtDetailRemark;
    private DevExpress.XtraEditors.LabelControl lblDetailStatCd;
    private LookUpEditWyn cboDetailStatCd;
    private DevExpress.XtraEditors.LabelControl lblDetailEmpNo;
    private TextEditWyn txtDetailEmpNo;
    private PanelWyn panelWyn6;
    private SplitterWyn splitterWyn1;
    private PanelWyn panelWyn8;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colMCustNm;
    private DevExpress.XtraGrid.Columns.GridColumn colMBizNo;
    private DevExpress.XtraGrid.Columns.GridColumn colMTel;
    private DevExpress.XtraGrid.Columns.GridColumn colMCurCd;
    private DevExpress.XtraGrid.Columns.GridColumn colMOwnerNm;
    private DevExpress.XtraGrid.Columns.GridColumn colMZipCode;
    private DevExpress.XtraGrid.Columns.GridColumn colMAddr1;
    private DevExpress.XtraGrid.Columns.GridColumn colMAddr2;
    private DevExpress.XtraGrid.Columns.GridColumn colMHomepage;
    private DevExpress.XtraGrid.Columns.GridColumn colMEmail;
    private DevExpress.XtraGrid.Columns.GridColumn colMFax;
    private DevExpress.XtraGrid.Columns.GridColumn colMBizKind;
    private DevExpress.XtraGrid.Columns.GridColumn colMBizType;
    private DevExpress.XtraGrid.Columns.GridColumn colMTransOpenDate;
    private DevExpress.XtraGrid.Columns.GridColumn colMVatType;
    private DevExpress.XtraGrid.Columns.GridColumn colMVatRate;
    private DevExpress.XtraGrid.Columns.GridColumn colMRemark;
    private DevExpress.XtraGrid.Columns.GridColumn colMStatCd;
    private DevExpress.XtraGrid.Columns.GridColumn colMEmpNo;
    private PanelWyn panelWyn2;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private PanelWyn paTitle;
    private SectionHeaderWyn sectionHeaderWyn2;
    private SectionHeaderWyn sectionHeaderWyn3;
    private SectionHeaderWyn sectionHeaderWyn4;
    private SectionHeaderWyn sectionHeaderWyn1;
}
