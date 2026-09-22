// VS 디자이너가 자동 생성하는 필드 선언에는 = null!을 안 붙여서 nullable 경고(CS8618)가
// 계속 나기 때문에, 이 파일(디자이너 전용)만 nullable 검사를 끈다 - 흔한 관례.
#nullable disable
using DevExpress.XtraGrid;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

/// <summary>
/// 사업장등록 화면 - TEMPLATE(frmMinorCode 표준 레이아웃)에서 하위목록(grd2) 없이 grd1(목록) +
/// panData(상세 등록)만 남긴 버전. 자세한 컬럼정의는 나중에 추가 예정이라 지금은 사업장코드/
/// 사업장명 2개 필드만 배치한다 - 필드를 더 늘릴 때는 panData에 라벨+입력컨트롤 쌍을 그대로
/// 이어 붙이면 된다(다음 Y좌표는 기존 필드보다 30만큼 아래).
/// </summary>
public partial class frmAcc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAcc));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.tabDetailGrids = new WYNLAB.Base.Controls.TabControlWyn();
            this.tabDetail3 = new DevExpress.XtraTab.XtraTabPage();
            this.panFile = new WYNLAB.Base.Controls.PanelWyn();
            this.grdFile = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvwFile = new WYNLAB.Base.Controls.GridViewWyn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn13 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnFileAttach = new WYNLAB.Base.Controls.ButtonWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.cboDetailVatType = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblDetailBizKind = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailBizKind = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailBizType = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailBizType = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailVatType = new DevExpress.XtraEditors.LabelControl();
            this.lblDetailVatRate = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailVatRate = new WYNLAB.Base.Controls.TextEditWyn();
            this.ymdOpenDate = new WYNLAB.Base.Controls.DateEditWyn();
            this.txtAddr2Eng = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtAddr1Eng = new WYNLAB.Base.Controls.TextEditWyn();
            this.lookUpEditWyn1 = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.picStamp = new WYNLAB.Base.Controls.PictureEditWyn();
            this.picLogo = new WYNLAB.Base.Controls.PictureEditWyn();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.txtAccId = new WYNLAB.Base.Controls.TextEditWyn();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl17 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl16 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.lblAccNm = new DevExpress.XtraEditors.LabelControl();
            this.txtAddr2 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtAddr1 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtZipCode = new WYNLAB.Base.Controls.TextEditWyn();
            this.textEditWyn3 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtFax = new WYNLAB.Base.Controls.TextEditWyn();
            this.textEditWyn2 = new WYNLAB.Base.Controls.TextEditWyn();
            this.textEditWyn1 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtOwnerNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtTel = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtBizNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtAccNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.txtAccNm_Q = new WYNLAB.Base.Controls.TextEditWyn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
            this.panelWyn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabDetailGrids)).BeginInit();
            this.tabDetailGrids.SuspendLayout();
            this.tabDetail3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panFile)).BeginInit();
            this.panFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn13)).BeginInit();
            this.panelWyn13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
            this.panelWyn4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailVatType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailBizKind.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailBizType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailVatRate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ymdOpenDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ymdOpenDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddr2Eng.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddr1Eng.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditWyn1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStamp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddr2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddr1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtZipCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditWyn3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFax.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditWyn2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditWyn1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOwnerNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBizNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccNm.Properties)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtAccNm_Q.Properties)).BeginInit();
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
            this.panBase.Size = new System.Drawing.Size(1165, 750);
            this.panBase.TabIndex = 5;
            // 
            // panelWyn3
            // 
            this.panelWyn3.Controls.Add(this.panelWyn1);
            this.panelWyn3.Controls.Add(this.panelWyn5);
            this.panelWyn3.Controls.Add(this.splitterWyn1);
            this.panelWyn3.Controls.Add(this.panelWyn8);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(3, 82);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Size = new System.Drawing.Size(1159, 665);
            this.panelWyn3.TabIndex = 7;
            // 
            // panelWyn1
            // 
            this.panelWyn1.Controls.Add(this.tabDetailGrids);
            this.panelWyn1.Controls.Add(this.panelWyn4);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(412, 453);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Size = new System.Drawing.Size(747, 212);
            this.panelWyn1.TabIndex = 13;
            // 
            // tabDetailGrids
            // 
            this.tabDetailGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabDetailGrids.Location = new System.Drawing.Point(0, 27);
            this.tabDetailGrids.Name = "tabDetailGrids";
            this.tabDetailGrids.SelectedTabPage = this.tabDetail3;
            this.tabDetailGrids.Size = new System.Drawing.Size(747, 185);
            this.tabDetailGrids.TabIndex = 10;
            this.tabDetailGrids.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabDetail3});
            // 
            // tabDetail3
            // 
            this.tabDetail3.Controls.Add(this.panFile);
            this.tabDetail3.Name = "tabDetail3";
            this.tabDetail3.Size = new System.Drawing.Size(745, 159);
            this.tabDetail3.Text = "첨부파일    ";
            // 
            // panFile
            // 
            this.panFile.Controls.Add(this.grdFile);
            this.panFile.Controls.Add(this.panelWyn13);
            this.panFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panFile.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panFile.Location = new System.Drawing.Point(0, 0);
            this.panFile.Name = "panFile";
            this.panFile.Size = new System.Drawing.Size(745, 159);
            this.panFile.TabIndex = 2;
            // 
            // grdFile
            // 
            this.grdFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdFile.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdFile.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdFile.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdFile.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdFile.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdFile.Location = new System.Drawing.Point(0, 30);
            this.grdFile.MainView = this.gvwFile;
            this.grdFile.Name = "grdFile";
            this.grdFile.Size = new System.Drawing.Size(745, 129);
            this.grdFile.TabIndex = 1;
            this.grdFile.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvwFile});
            // 
            // gvwFile
            // 
            this.gvwFile.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6});
            this.gvwFile.GridControl = this.grdFile;
            this.gvwFile.HighlightFocusedRow = true;
            this.gvwFile.Name = "gvwFile";
            this.gvwFile.OptionsBehavior.Editable = false;
            this.gvwFile.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvwFile.OptionsView.ColumnAutoWidth = false;
            this.gvwFile.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "순번";
            this.gridColumn3.FieldName = "Serl";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            this.gridColumn3.Width = 44;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "FILE NAME";
            this.gridColumn4.FieldName = "FileNm";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 1;
            this.gridColumn4.Width = 285;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "FileSize";
            this.gridColumn5.FieldName = "FileSize";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 2;
            this.gridColumn5.Width = 84;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "비고";
            this.gridColumn6.FieldName = "Remark";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 3;
            this.gridColumn6.Width = 241;
            // 
            // panelWyn13
            // 
            this.panelWyn13.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn13.Appearance.Options.UseBackColor = true;
            this.panelWyn13.Controls.Add(this.btnFileAttach);
            this.panelWyn13.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn13.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn13.Location = new System.Drawing.Point(0, 0);
            this.panelWyn13.Name = "panelWyn13";
            this.panelWyn13.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn13.Size = new System.Drawing.Size(745, 30);
            this.panelWyn13.TabIndex = 11;
            // 
            // btnFileAttach
            // 
            this.btnFileAttach.BackColor = System.Drawing.Color.Transparent;
            this.btnFileAttach.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnFileAttach.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFileAttach.FillColor = System.Drawing.Color.White;
            this.btnFileAttach.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnFileAttach.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnFileAttach.Image = null;
            this.btnFileAttach.Location = new System.Drawing.Point(5, 2);
            this.btnFileAttach.Name = "btnFileAttach";
            this.btnFileAttach.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnFileAttach.Size = new System.Drawing.Size(95, 26);
            this.btnFileAttach.TabIndex = 0;
            this.btnFileAttach.Text = "FILE첨부";
            this.btnFileAttach.ToolTip = null;
            // 
            // panelWyn4
            // 
            this.panelWyn4.Controls.Add(this.sectionHeaderWyn2);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(0, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn4.Size = new System.Drawing.Size(747, 27);
            this.panelWyn4.TabIndex = 9;
            this.panelWyn4.Visible = false;
            // 
            // sectionHeaderWyn2
            // 
            this.sectionHeaderWyn2.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn2.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn2.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn2.Name = "sectionHeaderWyn2";
            this.sectionHeaderWyn2.Size = new System.Drawing.Size(742, 25);
            this.sectionHeaderWyn2.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn2.SvgIcon")));
            this.sectionHeaderWyn2.TabIndex = 10;
            this.sectionHeaderWyn2.Text = "부가정보 등록";
            // 
            // panelWyn5
            // 
            this.panelWyn5.Controls.Add(this.panData);
            this.panelWyn5.Controls.Add(this.panelWyn6);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(412, 0);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelWyn5.Size = new System.Drawing.Size(747, 453);
            this.panelWyn5.TabIndex = 6;
            // 
            // panData
            // 
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Controls.Add(this.cboDetailVatType);
            this.panData.Controls.Add(this.lblDetailBizKind);
            this.panData.Controls.Add(this.txtDetailBizKind);
            this.panData.Controls.Add(this.lblDetailBizType);
            this.panData.Controls.Add(this.txtDetailBizType);
            this.panData.Controls.Add(this.lblDetailVatType);
            this.panData.Controls.Add(this.lblDetailVatRate);
            this.panData.Controls.Add(this.txtDetailVatRate);
            this.panData.Controls.Add(this.ymdOpenDate);
            this.panData.Controls.Add(this.txtAddr2Eng);
            this.panData.Controls.Add(this.txtAddr1Eng);
            this.panData.Controls.Add(this.lookUpEditWyn1);
            this.panData.Controls.Add(this.picStamp);
            this.panData.Controls.Add(this.picLogo);
            this.panData.Controls.Add(this.labelControl6);
            this.panData.Controls.Add(this.txtAccId);
            this.panData.Controls.Add(this.labelControl5);
            this.panData.Controls.Add(this.labelControl13);
            this.panData.Controls.Add(this.labelControl4);
            this.panData.Controls.Add(this.labelControl12);
            this.panData.Controls.Add(this.labelControl3);
            this.panData.Controls.Add(this.labelControl2);
            this.panData.Controls.Add(this.labelControl17);
            this.panData.Controls.Add(this.labelControl15);
            this.panData.Controls.Add(this.labelControl14);
            this.panData.Controls.Add(this.labelControl10);
            this.panData.Controls.Add(this.labelControl11);
            this.panData.Controls.Add(this.labelControl9);
            this.panData.Controls.Add(this.labelControl16);
            this.panData.Controls.Add(this.labelControl8);
            this.panData.Controls.Add(this.labelControl7);
            this.panData.Controls.Add(this.lblAccNm);
            this.panData.Controls.Add(this.txtAddr2);
            this.panData.Controls.Add(this.txtAddr1);
            this.panData.Controls.Add(this.txtZipCode);
            this.panData.Controls.Add(this.textEditWyn3);
            this.panData.Controls.Add(this.txtFax);
            this.panData.Controls.Add(this.textEditWyn2);
            this.panData.Controls.Add(this.textEditWyn1);
            this.panData.Controls.Add(this.txtOwnerNm);
            this.panData.Controls.Add(this.txtTel);
            this.panData.Controls.Add(this.txtBizNo);
            this.panData.Controls.Add(this.txtAccNm);
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(3, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(744, 426);
            this.panData.TabIndex = 8;
            // 
            // cboDetailVatType
            // 
            this.cboDetailVatType.EditValue = "";
            this.cboDetailVatType.Location = new System.Drawing.Point(92, 169);
            this.cboDetailVatType.LookupKey = "L_CM0004";
            this.cboDetailVatType.Name = "cboDetailVatType";
            this.cboDetailVatType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailVatType.Properties.NullText = "";
            this.cboDetailVatType.Size = new System.Drawing.Size(202, 20);
            this.cboDetailVatType.TabIndex = 35;
            // 
            // lblDetailBizKind
            // 
            this.lblDetailBizKind.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailBizKind.Appearance.Options.UseFont = true;
            this.lblDetailBizKind.Location = new System.Drawing.Point(64, 145);
            this.lblDetailBizKind.Name = "lblDetailBizKind";
            this.lblDetailBizKind.Size = new System.Drawing.Size(24, 15);
            this.lblDetailBizKind.TabIndex = 37;
            this.lblDetailBizKind.Text = "업종";
            // 
            // txtDetailBizKind
            // 
            this.txtDetailBizKind.Location = new System.Drawing.Point(92, 142);
            this.txtDetailBizKind.Name = "txtDetailBizKind";
            this.txtDetailBizKind.Size = new System.Drawing.Size(202, 20);
            this.txtDetailBizKind.TabIndex = 33;
            // 
            // lblDetailBizType
            // 
            this.lblDetailBizType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailBizType.Appearance.Options.UseFont = true;
            this.lblDetailBizType.Location = new System.Drawing.Point(372, 145);
            this.lblDetailBizType.Name = "lblDetailBizType";
            this.lblDetailBizType.Size = new System.Drawing.Size(24, 15);
            this.lblDetailBizType.TabIndex = 38;
            this.lblDetailBizType.Text = "업태";
            // 
            // txtDetailBizType
            // 
            this.txtDetailBizType.Location = new System.Drawing.Point(402, 142);
            this.txtDetailBizType.Name = "txtDetailBizType";
            this.txtDetailBizType.Size = new System.Drawing.Size(154, 20);
            this.txtDetailBizType.TabIndex = 34;
            // 
            // lblDetailVatType
            // 
            this.lblDetailVatType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailVatType.Appearance.Options.UseFont = true;
            this.lblDetailVatType.Location = new System.Drawing.Point(28, 172);
            this.lblDetailVatType.Name = "lblDetailVatType";
            this.lblDetailVatType.Size = new System.Drawing.Size(60, 15);
            this.lblDetailVatType.TabIndex = 39;
            this.lblDetailVatType.Text = "부가세유형";
            // 
            // lblDetailVatRate
            // 
            this.lblDetailVatRate.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailVatRate.Appearance.Options.UseFont = true;
            this.lblDetailVatRate.Location = new System.Drawing.Point(330, 172);
            this.lblDetailVatRate.Name = "lblDetailVatRate";
            this.lblDetailVatRate.Size = new System.Drawing.Size(66, 15);
            this.lblDetailVatRate.TabIndex = 40;
            this.lblDetailVatRate.Text = "부가세율(%)";
            // 
            // txtDetailVatRate
            // 
            this.txtDetailVatRate.Location = new System.Drawing.Point(402, 169);
            this.txtDetailVatRate.Name = "txtDetailVatRate";
            this.txtDetailVatRate.Size = new System.Drawing.Size(154, 20);
            this.txtDetailVatRate.TabIndex = 36;
            // 
            // ymdOpenDate
            // 
            this.ymdOpenDate.EditValue = null;
            this.ymdOpenDate.Location = new System.Drawing.Point(402, 12);
            this.ymdOpenDate.Name = "ymdOpenDate";
            this.ymdOpenDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ymdOpenDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ymdOpenDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.ymdOpenDate.Size = new System.Drawing.Size(154, 20);
            this.ymdOpenDate.TabIndex = 1;
            this.ymdOpenDate.YyyyMmDd = null;
            // 
            // txtAddr2Eng
            // 
            this.txtAddr2Eng.Location = new System.Drawing.Point(91, 301);
            this.txtAddr2Eng.Name = "txtAddr2Eng";
            this.txtAddr2Eng.Size = new System.Drawing.Size(464, 20);
            this.txtAddr2Eng.TabIndex = 14;
            // 
            // txtAddr1Eng
            // 
            this.txtAddr1Eng.Location = new System.Drawing.Point(91, 278);
            this.txtAddr1Eng.Name = "txtAddr1Eng";
            this.txtAddr1Eng.Size = new System.Drawing.Size(464, 20);
            this.txtAddr1Eng.TabIndex = 13;
            // 
            // lookUpEditWyn1
            // 
            this.lookUpEditWyn1.EditValue = "";
            this.lookUpEditWyn1.Location = new System.Drawing.Point(402, 39);
            this.lookUpEditWyn1.LookupKey = "L_CM0003";
            this.lookUpEditWyn1.Name = "lookUpEditWyn1";
            this.lookUpEditWyn1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditWyn1.Properties.NullText = "";
            this.lookUpEditWyn1.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.lookUpEditWyn1.Size = new System.Drawing.Size(154, 20);
            this.lookUpEditWyn1.TabIndex = 3;
            // 
            // picStamp
            // 
            this.picStamp.AllowDrop = true;
            this.picStamp.Cursor = System.Windows.Forms.Cursors.Default;
            this.picStamp.Location = new System.Drawing.Point(392, 331);
            this.picStamp.Name = "picStamp";
            this.picStamp.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.picStamp.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            this.picStamp.Size = new System.Drawing.Size(161, 83);
            this.picStamp.TabIndex = 16;
            // 
            // picLogo
            // 
            this.picLogo.AllowDrop = true;
            this.picLogo.Cursor = System.Windows.Forms.Cursors.Default;
            this.picLogo.Location = new System.Drawing.Point(91, 331);
            this.picLogo.Name = "picLogo";
            this.picLogo.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.picLogo.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            this.picLogo.Size = new System.Drawing.Size(230, 83);
            this.picLogo.TabIndex = 15;
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl6.Appearance.Options.UseFont = true;
            this.labelControl6.Location = new System.Drawing.Point(347, 331);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(39, 15);
            this.labelControl6.TabIndex = 2;
            this.labelControl6.Text = "STAMP";
            // 
            // txtAccId
            // 
            this.txtAccId.Location = new System.Drawing.Point(254, 14);
            this.txtAccId.Name = "txtAccId";
            this.txtAccId.Properties.ReadOnly = true;
            this.txtAccId.Size = new System.Drawing.Size(40, 20);
            this.txtAccId.TabIndex = 1;
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(51, 331);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(32, 15);
            this.labelControl5.TabIndex = 2;
            this.labelControl5.Text = "LOGO";
            // 
            // labelControl13
            // 
            this.labelControl13.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl13.Appearance.Options.UseFont = true;
            this.labelControl13.Location = new System.Drawing.Point(28, 304);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(55, 15);
            this.labelControl13.TabIndex = 2;
            this.labelControl13.Text = "영문주소2";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(52, 253);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(31, 15);
            this.labelControl4.TabIndex = 2;
            this.labelControl4.Text = "주소2";
            // 
            // labelControl12
            // 
            this.labelControl12.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl12.Appearance.Options.UseFont = true;
            this.labelControl12.Location = new System.Drawing.Point(28, 280);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(55, 15);
            this.labelControl12.TabIndex = 2;
            this.labelControl12.Text = "영문주소1";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(52, 228);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(31, 15);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "주소1";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(35, 203);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 15);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "우편번호";
            // 
            // labelControl17
            // 
            this.labelControl17.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl17.Appearance.Options.UseFont = true;
            this.labelControl17.Location = new System.Drawing.Point(348, 15);
            this.labelControl17.Name = "labelControl17";
            this.labelControl17.Size = new System.Drawing.Size(48, 15);
            this.labelControl17.TabIndex = 2;
            this.labelControl17.Text = "개업일자";
            // 
            // labelControl15
            // 
            this.labelControl15.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl15.Appearance.Options.UseFont = true;
            this.labelControl15.Location = new System.Drawing.Point(338, 118);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(58, 15);
            this.labelControl15.TabIndex = 2;
            this.labelControl15.Text = "대표E-mail";
            // 
            // labelControl14
            // 
            this.labelControl14.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl14.Appearance.Options.UseFont = true;
            this.labelControl14.Location = new System.Drawing.Point(24, 118);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(59, 15);
            this.labelControl14.TabIndex = 2;
            this.labelControl14.Text = "Homepage";
            // 
            // labelControl10
            // 
            this.labelControl10.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl10.Appearance.Options.UseFont = true;
            this.labelControl10.Location = new System.Drawing.Point(316, 67);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(80, 15);
            this.labelControl10.TabIndex = 2;
            this.labelControl10.Text = "대표자명(영문)";
            // 
            // labelControl11
            // 
            this.labelControl11.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl11.Appearance.Options.UseFont = true;
            this.labelControl11.Location = new System.Drawing.Point(372, 42);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(24, 15);
            this.labelControl11.TabIndex = 2;
            this.labelControl11.Text = "통화";
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl9.Appearance.Options.UseFont = true;
            this.labelControl9.Location = new System.Drawing.Point(35, 67);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(48, 15);
            this.labelControl9.TabIndex = 2;
            this.labelControl9.Text = "대표자명";
            // 
            // labelControl16
            // 
            this.labelControl16.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl16.Appearance.Options.UseFont = true;
            this.labelControl16.Location = new System.Drawing.Point(375, 93);
            this.labelControl16.Name = "labelControl16";
            this.labelControl16.Size = new System.Drawing.Size(21, 15);
            this.labelControl16.TabIndex = 2;
            this.labelControl16.Text = "FAX";
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl8.Appearance.Options.UseFont = true;
            this.labelControl8.Location = new System.Drawing.Point(35, 93);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(48, 15);
            this.labelControl8.TabIndex = 2;
            this.labelControl8.Text = "전화번호";
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl7.Appearance.Options.UseFont = true;
            this.labelControl7.Location = new System.Drawing.Point(23, 42);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(60, 15);
            this.labelControl7.TabIndex = 2;
            this.labelControl7.Text = "사업자번호";
            // 
            // lblAccNm
            // 
            this.lblAccNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblAccNm.Appearance.Options.UseFont = true;
            this.lblAccNm.Location = new System.Drawing.Point(35, 17);
            this.lblAccNm.Name = "lblAccNm";
            this.lblAccNm.Size = new System.Drawing.Size(48, 15);
            this.lblAccNm.TabIndex = 2;
            this.lblAccNm.Text = "사업장명";
            // 
            // txtAddr2
            // 
            this.txtAddr2.Location = new System.Drawing.Point(91, 249);
            this.txtAddr2.Name = "txtAddr2";
            this.txtAddr2.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtAddr2.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtAddr2.Properties.Appearance.Options.UseBackColor = true;
            this.txtAddr2.Properties.Appearance.Options.UseForeColor = true;
            this.txtAddr2.Size = new System.Drawing.Size(465, 20);
            this.txtAddr2.TabIndex = 12;
            // 
            // txtAddr1
            // 
            this.txtAddr1.Location = new System.Drawing.Point(91, 225);
            this.txtAddr1.Name = "txtAddr1";
            this.txtAddr1.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtAddr1.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtAddr1.Properties.Appearance.Options.UseBackColor = true;
            this.txtAddr1.Properties.Appearance.Options.UseForeColor = true;
            this.txtAddr1.Size = new System.Drawing.Size(465, 20);
            this.txtAddr1.TabIndex = 11;
            // 
            // txtZipCode
            // 
            this.txtZipCode.Location = new System.Drawing.Point(91, 200);
            this.txtZipCode.Name = "txtZipCode";
            this.txtZipCode.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtZipCode.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtZipCode.Properties.Appearance.Options.UseBackColor = true;
            this.txtZipCode.Properties.Appearance.Options.UseForeColor = true;
            this.txtZipCode.Size = new System.Drawing.Size(85, 20);
            this.txtZipCode.TabIndex = 10;
            // 
            // textEditWyn3
            // 
            this.textEditWyn3.Location = new System.Drawing.Point(402, 115);
            this.textEditWyn3.Name = "textEditWyn3";
            this.textEditWyn3.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textEditWyn3.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textEditWyn3.Properties.Appearance.Options.UseBackColor = true;
            this.textEditWyn3.Properties.Appearance.Options.UseForeColor = true;
            this.textEditWyn3.Size = new System.Drawing.Size(154, 20);
            this.textEditWyn3.TabIndex = 9;
            // 
            // txtFax
            // 
            this.txtFax.Location = new System.Drawing.Point(402, 90);
            this.txtFax.Name = "txtFax";
            this.txtFax.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtFax.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFax.Properties.Appearance.Options.UseBackColor = true;
            this.txtFax.Properties.Appearance.Options.UseForeColor = true;
            this.txtFax.Size = new System.Drawing.Size(154, 20);
            this.txtFax.TabIndex = 7;
            // 
            // textEditWyn2
            // 
            this.textEditWyn2.Location = new System.Drawing.Point(91, 115);
            this.textEditWyn2.Name = "textEditWyn2";
            this.textEditWyn2.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textEditWyn2.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textEditWyn2.Properties.Appearance.Options.UseBackColor = true;
            this.textEditWyn2.Properties.Appearance.Options.UseForeColor = true;
            this.textEditWyn2.Size = new System.Drawing.Size(203, 20);
            this.textEditWyn2.TabIndex = 8;
            // 
            // textEditWyn1
            // 
            this.textEditWyn1.Location = new System.Drawing.Point(402, 64);
            this.textEditWyn1.Name = "textEditWyn1";
            this.textEditWyn1.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textEditWyn1.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textEditWyn1.Properties.Appearance.Options.UseBackColor = true;
            this.textEditWyn1.Properties.Appearance.Options.UseForeColor = true;
            this.textEditWyn1.Size = new System.Drawing.Size(154, 20);
            this.textEditWyn1.TabIndex = 5;
            // 
            // txtOwnerNm
            // 
            this.txtOwnerNm.Location = new System.Drawing.Point(91, 64);
            this.txtOwnerNm.Name = "txtOwnerNm";
            this.txtOwnerNm.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtOwnerNm.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtOwnerNm.Properties.Appearance.Options.UseBackColor = true;
            this.txtOwnerNm.Properties.Appearance.Options.UseForeColor = true;
            this.txtOwnerNm.Size = new System.Drawing.Size(203, 20);
            this.txtOwnerNm.TabIndex = 4;
            // 
            // txtTel
            // 
            this.txtTel.Location = new System.Drawing.Point(91, 90);
            this.txtTel.Name = "txtTel";
            this.txtTel.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtTel.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTel.Properties.Appearance.Options.UseBackColor = true;
            this.txtTel.Properties.Appearance.Options.UseForeColor = true;
            this.txtTel.Size = new System.Drawing.Size(203, 20);
            this.txtTel.TabIndex = 6;
            // 
            // txtBizNo
            // 
            this.txtBizNo.Location = new System.Drawing.Point(91, 39);
            this.txtBizNo.Name = "txtBizNo";
            this.txtBizNo.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtBizNo.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBizNo.Properties.Appearance.Options.UseBackColor = true;
            this.txtBizNo.Properties.Appearance.Options.UseForeColor = true;
            this.txtBizNo.Size = new System.Drawing.Size(203, 20);
            this.txtBizNo.TabIndex = 2;
            // 
            // txtAccNm
            // 
            this.txtAccNm.Location = new System.Drawing.Point(91, 14);
            this.txtAccNm.Name = "txtAccNm";
            this.txtAccNm.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtAccNm.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtAccNm.Properties.Appearance.Options.UseBackColor = true;
            this.txtAccNm.Properties.Appearance.Options.UseForeColor = true;
            this.txtAccNm.Required = true;
            this.txtAccNm.Size = new System.Drawing.Size(161, 20);
            this.txtAccNm.TabIndex = 0;
            // 
            // panelWyn6
            // 
            this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
            this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn6.Location = new System.Drawing.Point(3, 0);
            this.panelWyn6.Name = "panelWyn6";
            this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn6.Size = new System.Drawing.Size(744, 27);
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
            this.sectionHeaderWyn3.Size = new System.Drawing.Size(739, 25);
            this.sectionHeaderWyn3.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn3.SvgIcon")));
            this.sectionHeaderWyn3.TabIndex = 10;
            this.sectionHeaderWyn3.Text = "사업장 정보 등록";
            // 
            // splitterWyn1
            // 
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Location = new System.Drawing.Point(402, 0);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(10, 665);
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
            this.panelWyn8.Size = new System.Drawing.Size(402, 665);
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
            this.grd1.Size = new System.Drawing.Size(402, 638);
            this.grd1.TabIndex = 10;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightFocusedRow = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw1.OptionsSelection.InvertSelection = true;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "사업장ID";
            this.gridColumn1.FieldName = "acc_id";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 81;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "사업장명";
            this.gridColumn2.FieldName = "acc_nm";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 293;
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
            this.sectionHeaderWyn4.TabIndex = 10;
            this.sectionHeaderWyn4.Text = "사업장 LIST";
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.txtAccNm_Q);
            this.panHeader.Controls.Add(this.labelControl1);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(3, 33);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1159, 49);
            this.panHeader.TabIndex = 8;
            // 
            // txtAccNm_Q
            // 
            this.txtAccNm_Q.Location = new System.Drawing.Point(80, 16);
            this.txtAccNm_Q.Name = "txtAccNm_Q";
            this.txtAccNm_Q.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtAccNm_Q.Size = new System.Drawing.Size(265, 20);
            this.txtAccNm_Q.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(26, 19);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 15);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "사업장명";
            // 
            // paTitle
            // 
            this.paTitle.Controls.Add(this.sectionHeaderWyn1);
            this.paTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle.Location = new System.Drawing.Point(3, 0);
            this.paTitle.Name = "paTitle";
            this.paTitle.Size = new System.Drawing.Size(1159, 33);
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
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1159, 33);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 12;
            this.sectionHeaderWyn1.Text = "사업장 등록 [frmAcc]";
            // 
            // frmAcc
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1165, 750);
            this.Controls.Add(this.panBase);
            this.Name = "frmAcc";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabDetailGrids)).EndInit();
            this.tabDetailGrids.ResumeLayout(false);
            this.tabDetail3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panFile)).EndInit();
            this.panFile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn13)).EndInit();
            this.panelWyn13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
            this.panData.ResumeLayout(false);
            this.panData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailVatType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailBizKind.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailBizType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailVatRate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ymdOpenDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ymdOpenDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddr2Eng.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddr1Eng.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditWyn1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStamp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddr2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddr1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtZipCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditWyn3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFax.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditWyn2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditWyn1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOwnerNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBizNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccNm.Properties)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtAccNm_Q.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
            this.paTitle.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private PanelWyn paTitle;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private PanelWyn panelWyn6;
    private SplitterWyn splitterWyn1;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
    private PanelWyn panelWyn2;
    private PanelWyn panHeader;
    private TextEditWyn txtAccNm_Q;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private PanelWyn panelWyn8;
    private TextEditWyn txtAccId;
    private DevExpress.XtraEditors.LabelControl lblAccNm;
    private TextEditWyn txtAccNm;
    private DevExpress.XtraEditors.LabelControl labelControl4;
    private DevExpress.XtraEditors.LabelControl labelControl3;
    private DevExpress.XtraEditors.LabelControl labelControl2;
    private TextEditWyn txtAddr2;
    private TextEditWyn txtAddr1;
    private TextEditWyn txtZipCode;
    private PictureEditWyn picLogo;
    private DevExpress.XtraEditors.LabelControl labelControl5;
    private PictureEditWyn picStamp;
    private DevExpress.XtraEditors.LabelControl labelControl6;
    private DevExpress.XtraEditors.LabelControl labelControl9;
    private DevExpress.XtraEditors.LabelControl labelControl8;
    private DevExpress.XtraEditors.LabelControl labelControl7;
    private TextEditWyn txtOwnerNm;
    private TextEditWyn txtTel;
    private TextEditWyn txtBizNo;
    private TextEditWyn txtAddr2Eng;
    private TextEditWyn txtAddr1Eng;
    private LookUpEditWyn lookUpEditWyn1;
    private DevExpress.XtraEditors.LabelControl labelControl13;
    private DevExpress.XtraEditors.LabelControl labelControl12;
    private DevExpress.XtraEditors.LabelControl labelControl10;
    private DevExpress.XtraEditors.LabelControl labelControl11;
    private TextEditWyn textEditWyn1;
    private DevExpress.XtraEditors.LabelControl labelControl15;
    private DevExpress.XtraEditors.LabelControl labelControl14;
    private DevExpress.XtraEditors.LabelControl labelControl16;
    private TextEditWyn textEditWyn3;
    private TextEditWyn txtFax;
    private TextEditWyn textEditWyn2;
    private SectionHeaderWyn sectionHeaderWyn4;
    private SectionHeaderWyn sectionHeaderWyn3;
    private SectionHeaderWyn sectionHeaderWyn1;
    private PanelWyn panelWyn1;
    private PanelWyn panelWyn4;
    private SectionHeaderWyn sectionHeaderWyn2;
    private TabControlWyn tabDetailGrids;
    private DevExpress.XtraTab.XtraTabPage tabDetail3;
    private PanelWyn panFile;
    private GridControlWyn grdFile;
    private GridViewWyn gvwFile;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
    private PanelWyn panelWyn13;
    private ButtonWyn btnFileAttach;
    private DateEditWyn ymdOpenDate;
    private DevExpress.XtraEditors.LabelControl labelControl17;
    private LookUpEditWyn cboDetailVatType;
    private DevExpress.XtraEditors.LabelControl lblDetailBizKind;
    private TextEditWyn txtDetailBizKind;
    private DevExpress.XtraEditors.LabelControl lblDetailBizType;
    private TextEditWyn txtDetailBizType;
    private DevExpress.XtraEditors.LabelControl lblDetailVatType;
    private DevExpress.XtraEditors.LabelControl lblDetailVatRate;
    private TextEditWyn txtDetailVatRate;
}
