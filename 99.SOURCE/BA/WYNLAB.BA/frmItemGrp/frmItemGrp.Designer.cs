// @AI_BUILDER:BEGIN FILE_HEADER
// AI Builder 마스터-상세폼-서브그리드 템플릿 원본 - 기초코드등록(frmMinorCode)과 같은 형태다:
// grd1(마스터 목록, 조회전용) 선택 -> panData(상세 편집폼)에 값을 채우고 -> grd2(하위 목록,
// 조회전용)를 다시 조회한다. VS 디자이너로 열어 제목영역/여백/색상을 다듬으면 AI Builder가
// 만드는 모든 마스터-상세폼-서브그리드 화면에 그대로 반영된다.
// @AI_BUILDER:END FILE_HEADER
#nullable disable
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmItemGrp
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemGrp));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
        this.colDAccCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDItemLvl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDItemClassCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDItemClassNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDParItemClassCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDRemark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDeletRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAddRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
        this.lblDetailAccCd = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailAccCd = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDetailItemLvl = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailItemLvl = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDetailItemClassCd = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailItemClassCd = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDetailItemClassNm = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailItemClassNm = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDetailParItemClassCd = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailParItemClassCd = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDetailRemark = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailRemark = new WYNLAB.Base.Controls.TextEditWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.colMAccCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMItemLvl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMItemClassCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMItemClassNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMParItemClassCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMRemark = new DevExpress.XtraGrid.Columns.GridColumn();
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
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
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
            this.panBase.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
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
            this.panelWyn3.Location = new System.Drawing.Point(3, 82);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Size = new System.Drawing.Size(1239, 495);
            this.panelWyn3.TabIndex = 7;
            // 
            // panelWyn4
            // 
            this.panelWyn4.Controls.Add(this.grd2);
            this.panelWyn4.Controls.Add(this.panelWyn1);
            this.panelWyn4.Controls.Add(this.panelWyn5);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(412, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelWyn4.Size = new System.Drawing.Size(827, 495);
            this.panelWyn4.TabIndex = 7;
            // 
            // grd2
            // 
            this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd2.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grd2.Location = new System.Drawing.Point(3, 245);
            this.grd2.MainView = this.gvw2;
            this.grd2.Name = "grd2";
            this.grd2.Size = new System.Drawing.Size(824, 250);
            this.grd2.TabIndex = 7;
            this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw2});
            //
            // gvw2
            //
        this.colDAccCd.Caption = "acc_cd";
        this.colDAccCd.FieldName = "acc_cd";
        this.colDAccCd.Name = "colDAccCd";
        this.colDAccCd.Visible = true;
        this.colDAccCd.VisibleIndex = 0;
        this.colDAccCd.Width = 100;
        this.colDItemLvl.Caption = "item_lvl";
        this.colDItemLvl.FieldName = "item_lvl";
        this.colDItemLvl.Name = "colDItemLvl";
        this.colDItemLvl.Visible = true;
        this.colDItemLvl.VisibleIndex = 1;
        this.colDItemLvl.Width = 100;
        this.colDItemClassCd.Caption = "item_class_cd";
        this.colDItemClassCd.FieldName = "item_class_cd";
        this.colDItemClassCd.Name = "colDItemClassCd";
        this.colDItemClassCd.Visible = true;
        this.colDItemClassCd.VisibleIndex = 2;
        this.colDItemClassCd.Width = 100;
        this.colDItemClassNm.Caption = "item_class_nm";
        this.colDItemClassNm.FieldName = "item_class_nm";
        this.colDItemClassNm.Name = "colDItemClassNm";
        this.colDItemClassNm.Visible = true;
        this.colDItemClassNm.VisibleIndex = 3;
        this.colDItemClassNm.Width = 100;
        this.colDParItemClassCd.Caption = "par_item_class_cd";
        this.colDParItemClassCd.FieldName = "par_item_class_cd";
        this.colDParItemClassCd.Name = "colDParItemClassCd";
        this.colDParItemClassCd.Visible = true;
        this.colDParItemClassCd.VisibleIndex = 4;
        this.colDParItemClassCd.Width = 100;
        this.colDRemark.Caption = "remark";
        this.colDRemark.FieldName = "remark";
        this.colDRemark.Name = "colDRemark";
        this.colDRemark.Visible = true;
        this.colDRemark.VisibleIndex = 5;
        this.colDRemark.Width = 100;
        this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDAccCd,
            this.colDItemLvl,
            this.colDItemClassCd,
            this.colDItemClassNm,
            this.colDParItemClassCd,
            this.colDRemark});
            this.gvw2.GridControl = this.grd2;
            this.gvw2.HighlightFocusedRow = true;
            this.gvw2.Name = "gvw2";
            this.gvw2.OptionsBehavior.Editable = false;
            this.gvw2.OptionsView.ColumnAutoWidth = false;
            this.gvw2.OptionsView.ShowGroupPanel = false;
            // 
            // panelWyn1
            // 
            this.panelWyn1.Controls.Add(this.panelWyn7);
            this.panelWyn1.Controls.Add(this.sectionHeaderWyn2);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(3, 218);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn1.Size = new System.Drawing.Size(824, 27);
            this.panelWyn1.TabIndex = 8;
            // 
            // panelWyn7
            // 
            this.panelWyn7.Controls.Add(this.btnDeletRow2);
            this.panelWyn7.Controls.Add(this.btnAddRow2);
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(756, 0);
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
            this.btnDeletRow2.Image = ((System.Drawing.Image)(resources.GetObject("btnDeletRow2.Image")));
            this.btnDeletRow2.Location = new System.Drawing.Point(42, 2);
            this.btnDeletRow2.Name = "btnDeletRow2";
            this.btnDeletRow2.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDeletRow2.Size = new System.Drawing.Size(24, 22);
            this.btnDeletRow2.TabIndex = 0;
            this.btnDeletRow2.ToolTip = "행삭제";
            // 
            // btnAddRow2
            // 
            this.btnAddRow2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddRow2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(231)))), ((int)(((byte)(203)))));
            this.btnAddRow2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddRow2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(245)))), ((int)(((byte)(231)))));
            this.btnAddRow2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnAddRow2.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnAddRow2.Image = ((System.Drawing.Image)(resources.GetObject("btnAddRow2.Image")));
            this.btnAddRow2.Location = new System.Drawing.Point(10, 2);
            this.btnAddRow2.Name = "btnAddRow2";
            this.btnAddRow2.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnAddRow2.Size = new System.Drawing.Size(24, 22);
            this.btnAddRow2.TabIndex = 0;
            this.btnAddRow2.ToolTip = "행추가";
            // 
            // sectionHeaderWyn2
            // 
            this.sectionHeaderWyn2.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn2.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn2.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn2.Name = "sectionHeaderWyn2";
            this.sectionHeaderWyn2.Size = new System.Drawing.Size(819, 25);
            this.sectionHeaderWyn2.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn2.SvgIcon")));
            this.sectionHeaderWyn2.TabIndex = 8;
            this.sectionHeaderWyn2.Text = "하위 목록";
            // 
            // panelWyn5
            // 
            this.panelWyn5.Controls.Add(this.panData);
            this.panelWyn5.Controls.Add(this.panelWyn6);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(3, 0);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Size = new System.Drawing.Size(824, 218);
            this.panelWyn5.TabIndex = 6;
            // 
            // panData
            // 
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(824, 191);
            this.panData.TabIndex = 8;
        this.lblDetailAccCd.Location = new System.Drawing.Point(16, 19);
        this.lblDetailAccCd.Name = "lblDetailAccCd";
        this.lblDetailAccCd.Text = "사업장";
        this.txtDetailAccCd.Location = new System.Drawing.Point(120, 16);
        this.txtDetailAccCd.Name = "txtDetailAccCd";
        this.txtDetailAccCd.Size = new System.Drawing.Size(220, 20);
        this.panData.Controls.Add(this.lblDetailAccCd);
        this.panData.Controls.Add(this.txtDetailAccCd);
        this.lblDetailItemLvl.Location = new System.Drawing.Point(16, 47);
        this.lblDetailItemLvl.Name = "lblDetailItemLvl";
        this.lblDetailItemLvl.Text = "item_lvl";
        this.txtDetailItemLvl.Location = new System.Drawing.Point(120, 44);
        this.txtDetailItemLvl.Name = "txtDetailItemLvl";
        this.txtDetailItemLvl.Size = new System.Drawing.Size(220, 20);
        this.panData.Controls.Add(this.lblDetailItemLvl);
        this.panData.Controls.Add(this.txtDetailItemLvl);
        this.lblDetailItemClassCd.Location = new System.Drawing.Point(16, 75);
        this.lblDetailItemClassCd.Name = "lblDetailItemClassCd";
        this.lblDetailItemClassCd.Text = "품목그룹코드";
        this.txtDetailItemClassCd.Location = new System.Drawing.Point(120, 72);
        this.txtDetailItemClassCd.Name = "txtDetailItemClassCd";
        this.txtDetailItemClassCd.Size = new System.Drawing.Size(220, 20);
        this.panData.Controls.Add(this.lblDetailItemClassCd);
        this.panData.Controls.Add(this.txtDetailItemClassCd);
        this.lblDetailItemClassNm.Location = new System.Drawing.Point(16, 103);
        this.lblDetailItemClassNm.Name = "lblDetailItemClassNm";
        this.lblDetailItemClassNm.Text = "품목그룹";
        this.txtDetailItemClassNm.Location = new System.Drawing.Point(120, 100);
        this.txtDetailItemClassNm.Name = "txtDetailItemClassNm";
        this.txtDetailItemClassNm.Size = new System.Drawing.Size(220, 20);
        this.panData.Controls.Add(this.lblDetailItemClassNm);
        this.panData.Controls.Add(this.txtDetailItemClassNm);
        this.lblDetailParItemClassCd.Location = new System.Drawing.Point(16, 131);
        this.lblDetailParItemClassCd.Name = "lblDetailParItemClassCd";
        this.lblDetailParItemClassCd.Text = "par_item_class_cd";
        this.txtDetailParItemClassCd.Location = new System.Drawing.Point(120, 128);
        this.txtDetailParItemClassCd.Name = "txtDetailParItemClassCd";
        this.txtDetailParItemClassCd.Size = new System.Drawing.Size(220, 20);
        this.panData.Controls.Add(this.lblDetailParItemClassCd);
        this.panData.Controls.Add(this.txtDetailParItemClassCd);
        this.lblDetailRemark.Location = new System.Drawing.Point(16, 159);
        this.lblDetailRemark.Name = "lblDetailRemark";
        this.lblDetailRemark.Text = "remark";
        this.txtDetailRemark.Location = new System.Drawing.Point(120, 156);
        this.txtDetailRemark.Name = "txtDetailRemark";
        this.txtDetailRemark.Size = new System.Drawing.Size(220, 20);
        this.panData.Controls.Add(this.lblDetailRemark);
        this.panData.Controls.Add(this.txtDetailRemark);
            //
            // panelWyn6
            // 
            this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
            this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn6.Location = new System.Drawing.Point(0, 0);
            this.panelWyn6.Name = "panelWyn6";
            this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn6.Size = new System.Drawing.Size(824, 27);
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
            this.sectionHeaderWyn3.Size = new System.Drawing.Size(819, 25);
            this.sectionHeaderWyn3.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn3.SvgIcon")));
            this.sectionHeaderWyn3.TabIndex = 8;
            this.sectionHeaderWyn3.Text = "상세 등록";
            // 
            // splitterWyn1
            // 
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Location = new System.Drawing.Point(402, 0);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(10, 495);
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
            this.panelWyn8.Size = new System.Drawing.Size(402, 495);
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
            this.grd1.Size = new System.Drawing.Size(402, 468);
            this.grd1.TabIndex = 10;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            //
            // gvw1
            //
        this.colMAccCd.Caption = "사업장";
        this.colMAccCd.FieldName = "acc_cd";
        this.colMAccCd.Name = "colMAccCd";
        this.colMAccCd.Visible = true;
        this.colMAccCd.VisibleIndex = 0;
        this.colMAccCd.Width = 100;
        this.colMItemLvl.Caption = "item_lvl";
        this.colMItemLvl.FieldName = "item_lvl";
        this.colMItemLvl.Name = "colMItemLvl";
        this.colMItemLvl.Visible = true;
        this.colMItemLvl.VisibleIndex = 1;
        this.colMItemLvl.Width = 100;
        this.colMItemClassCd.Caption = "품목그룹코드";
        this.colMItemClassCd.FieldName = "item_class_cd";
        this.colMItemClassCd.Name = "colMItemClassCd";
        this.colMItemClassCd.Visible = true;
        this.colMItemClassCd.VisibleIndex = 2;
        this.colMItemClassCd.Width = 100;
        this.colMItemClassNm.Caption = "품목그룹";
        this.colMItemClassNm.FieldName = "item_class_nm";
        this.colMItemClassNm.Name = "colMItemClassNm";
        this.colMItemClassNm.Visible = true;
        this.colMItemClassNm.VisibleIndex = 3;
        this.colMItemClassNm.Width = 100;
        this.colMParItemClassCd.Caption = "par_item_class_cd";
        this.colMParItemClassCd.FieldName = "par_item_class_cd";
        this.colMParItemClassCd.Name = "colMParItemClassCd";
        this.colMParItemClassCd.Visible = true;
        this.colMParItemClassCd.VisibleIndex = 4;
        this.colMParItemClassCd.Width = 100;
        this.colMRemark.Caption = "remark";
        this.colMRemark.FieldName = "remark";
        this.colMRemark.Name = "colMRemark";
        this.colMRemark.Visible = true;
        this.colMRemark.VisibleIndex = 5;
        this.colMRemark.Width = 100;
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colMAccCd,
            this.colMItemLvl,
            this.colMItemClassCd,
            this.colMItemClassNm,
            this.colMParItemClassCd,
            this.colMRemark});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightFocusedRow = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsSelection.InvertSelection = true;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
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
            this.sectionHeaderWyn4.TabIndex = 8;
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
            this.panHeader.Location = new System.Drawing.Point(3, 33);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1239, 49);
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
            this.paTitle.Location = new System.Drawing.Point(3, 0);
            this.paTitle.Name = "paTitle";
            this.paTitle.Size = new System.Drawing.Size(1239, 33);
            this.paTitle.TabIndex = 5;
            // 
            // sectionHeaderWyn1
            // 
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(5, 6);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(596, 23);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 8;
            this.sectionHeaderWyn1.Text = "MasterFormSubGrid [frmMasterFormSubGrid]";
            // 
            // frmItemGrp
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1245, 580);
            this.Controls.Add(this.panBase);
            this.Name = "frmItemGrp";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
            this.panelWyn7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
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
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private DevExpress.XtraGrid.Columns.GridColumn colDAccCd;
    private DevExpress.XtraGrid.Columns.GridColumn colDItemLvl;
    private DevExpress.XtraGrid.Columns.GridColumn colDItemClassCd;
    private DevExpress.XtraGrid.Columns.GridColumn colDItemClassNm;
    private DevExpress.XtraGrid.Columns.GridColumn colDParItemClassCd;
    private DevExpress.XtraGrid.Columns.GridColumn colDRemark;
    private PanelWyn panelWyn1;
    private PanelWyn panelWyn7;
    private ButtonWyn btnDeletRow2;
    private ButtonWyn btnAddRow2;
    private SectionHeaderWyn sectionHeaderWyn2;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblDetailAccCd;
    private TextEditWyn txtDetailAccCd;
    private DevExpress.XtraEditors.LabelControl lblDetailItemLvl;
    private TextEditWyn txtDetailItemLvl;
    private DevExpress.XtraEditors.LabelControl lblDetailItemClassCd;
    private TextEditWyn txtDetailItemClassCd;
    private DevExpress.XtraEditors.LabelControl lblDetailItemClassNm;
    private TextEditWyn txtDetailItemClassNm;
    private DevExpress.XtraEditors.LabelControl lblDetailParItemClassCd;
    private TextEditWyn txtDetailParItemClassCd;
    private DevExpress.XtraEditors.LabelControl lblDetailRemark;
    private TextEditWyn txtDetailRemark;
    private PanelWyn panelWyn6;
    private SectionHeaderWyn sectionHeaderWyn3;
    private SplitterWyn splitterWyn1;
    private PanelWyn panelWyn8;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colMAccCd;
    private DevExpress.XtraGrid.Columns.GridColumn colMItemLvl;
    private DevExpress.XtraGrid.Columns.GridColumn colMItemClassCd;
    private DevExpress.XtraGrid.Columns.GridColumn colMItemClassNm;
    private DevExpress.XtraGrid.Columns.GridColumn colMParItemClassCd;
    private DevExpress.XtraGrid.Columns.GridColumn colMRemark;
    private PanelWyn panelWyn2;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private PanelWyn paTitle;
    private SectionHeaderWyn sectionHeaderWyn1;
}
