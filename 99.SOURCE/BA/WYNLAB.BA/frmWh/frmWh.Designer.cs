#nullable disable
using DevExpress.XtraGrid;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmWh
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

    private PanelWyn panBase;
    private PanelWyn paTitle;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private TextEditWyn txtWhNm_Q;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn8;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colWhId;
    private DevExpress.XtraGrid.Columns.GridColumn colWhNm;
    private DevExpress.XtraGrid.Columns.GridColumn colWhType;
    private DevExpress.XtraGrid.Columns.GridColumn colWhDeptNm;
    private DevExpress.XtraGrid.Columns.GridColumn colWhEmpNm;
    private PanelWyn panelWyn2;
    private SplitterWyn splitterWyn1;
    private PanelWyn panelWyn4;
    private PanelWyn panelWyn5;
    private PanelWyn panelWyn6;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl labelControl4;
    private LookUpEditWyn cboDetailAccId;
    private DevExpress.XtraEditors.LabelControl labelControl5;
    private TextEditWyn txtDetailWhNm;
    private DevExpress.XtraEditors.LabelControl labelControl6;
    private TextEditWyn txtDetailWhType;
    private DevExpress.XtraEditors.LabelControl labelControl7;
    private TextEditWyn txtDetailDeptId;
    private PopupLookupEditWyn popDetailDeptNm;
    private DevExpress.XtraEditors.LabelControl labelControl8;
    private TextEditWyn txtDetailEmpId;
    private PopupLookupEditWyn popDetailEmpNm;
    private PanelWyn panelWyn1;
    private PanelWyn panelWyn7;
    private ButtonWyn btnAddRow2;
    private ButtonWyn btnDeletRow2;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private DevExpress.XtraGrid.Columns.GridColumn colLocId;
    private DevExpress.XtraGrid.Columns.GridColumn colLocNm;
    private DevExpress.XtraGrid.Columns.GridColumn colLocType;

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWh));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colLocId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLocNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLocType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDeletRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAddRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.cboDetailAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailWhNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailWhType = new WYNLAB.Base.Controls.TextEditWyn();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailDeptId = new WYNLAB.Base.Controls.TextEditWyn();
            this.popDetailDeptNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailEmpId = new WYNLAB.Base.Controls.TextEditWyn();
            this.popDetailEmpNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colWhId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWhType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWhDeptNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWhEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
            this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.txtWhNm_Q = new WYNLAB.Base.Controls.TextEditWyn();
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
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailAccId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailWhNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailWhType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailDeptId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popDetailDeptNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailEmpId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popDetailEmpNm.Properties)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtWhNm_Q.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).BeginInit();
            this.paTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
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
            this.panBase.Size = new System.Drawing.Size(1165, 600);
            this.panBase.TabIndex = 0;
            // 
            // panelWyn3
            // 
            this.panelWyn3.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn3.Appearance.Options.UseBackColor = true;
            this.panelWyn3.Controls.Add(this.panelWyn8);
            this.panelWyn3.Controls.Add(this.splitterWyn1);
            this.panelWyn3.Controls.Add(this.panelWyn4);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(3, 82);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Size = new System.Drawing.Size(1159, 515);
            this.panelWyn3.TabIndex = 3;
            // 
            // panelWyn4
            // 
            this.panelWyn4.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn4.Appearance.Options.UseBackColor = true;
            this.panelWyn4.Controls.Add(this.grd2);
            this.panelWyn4.Controls.Add(this.panelWyn7);
            this.panelWyn4.Controls.Add(this.panelWyn1);
            this.panelWyn4.Controls.Add(this.panelWyn5);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(409, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelWyn4.Size = new System.Drawing.Size(750, 515);
            this.panelWyn4.TabIndex = 2;
            // 
            // grd2
            // 
            this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd2.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grd2.Location = new System.Drawing.Point(3, 205);
            this.grd2.MainView = this.gvw2;
            this.grd2.Name = "grd2";
            this.grd2.Size = new System.Drawing.Size(747, 310);
            this.grd2.TabIndex = 2;
            this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw2});
            // 
            // gvw2
            // 
            this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colLocId,
            this.colLocNm,
            this.colLocType});
            this.gvw2.GridControl = this.grd2;
            this.gvw2.HighlightFocusedRow = true;
            this.gvw2.Name = "gvw2";
            this.gvw2.OptionsBehavior.Editable = false;
            this.gvw2.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw2.OptionsView.ColumnAutoWidth = false;
            this.gvw2.OptionsView.ShowGroupPanel = false;
            // 
            // colLocId
            // 
            this.colLocId.AppearanceCell.Options.UseTextOptions = true;
            this.colLocId.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colLocId.Caption = "위치ID";
            this.colLocId.FieldName = "loc_id";
            this.colLocId.Name = "colLocId";
            this.colLocId.OptionsColumn.AllowEdit = false;
            this.colLocId.Visible = true;
            this.colLocId.VisibleIndex = 0;
            this.colLocId.Width = 70;
            // 
            // colLocNm
            // 
            this.colLocNm.Caption = "위치명";
            this.colLocNm.FieldName = "loc_nm";
            this.colLocNm.Name = "colLocNm";
            this.colLocNm.Visible = true;
            this.colLocNm.VisibleIndex = 1;
            this.colLocNm.Width = 260;
            // 
            // colLocType
            // 
            this.colLocType.Caption = "위치유형";
            this.colLocType.FieldName = "loc_type";
            this.colLocType.Name = "colLocType";
            this.colLocType.Visible = true;
            this.colLocType.VisibleIndex = 2;
            this.colLocType.Width = 150;
            // 
            // panelWyn7
            // 
            this.panelWyn7.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn7.Appearance.Options.UseBackColor = true;
            this.panelWyn7.Controls.Add(this.btnDeletRow2);
            this.panelWyn7.Controls.Add(this.btnAddRow2);
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(3, 175);
            this.panelWyn7.Name = "panelWyn7";
            this.panelWyn7.Padding = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.panelWyn7.Size = new System.Drawing.Size(747, 30);
            this.panelWyn7.TabIndex = 1;
            // 
            // btnDeletRow2
            // 
            this.btnDeletRow2.BackColor = System.Drawing.Color.Transparent;
            this.btnDeletRow2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(198)))), ((int)(((byte)(193)))));
            this.btnDeletRow2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeletRow2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(236)))), ((int)(((byte)(234)))));
            this.btnDeletRow2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDeletRow2.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDeletRow2.Image = null;
            this.btnDeletRow2.Location = new System.Drawing.Point(68, 4);
            this.btnDeletRow2.Name = "btnDeletRow2";
            this.btnDeletRow2.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDeletRow2.Size = new System.Drawing.Size(60, 24);
            this.btnDeletRow2.TabIndex = 0;
            this.btnDeletRow2.Text = "행삭제";
            this.btnDeletRow2.ToolTip = "행삭제";
            this.btnDeletRow2.Click += new System.EventHandler(this.btnDeletRow2_Click);
            // 
            // btnAddRow2
            // 
            this.btnAddRow2.BackColor = System.Drawing.Color.Transparent;
            this.btnAddRow2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(231)))), ((int)(((byte)(203)))));
            this.btnAddRow2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddRow2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(245)))), ((int)(((byte)(231)))));
            this.btnAddRow2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnAddRow2.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnAddRow2.Image = null;
            this.btnAddRow2.Location = new System.Drawing.Point(6, 4);
            this.btnAddRow2.Name = "btnAddRow2";
            this.btnAddRow2.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnAddRow2.Size = new System.Drawing.Size(60, 24);
            this.btnAddRow2.TabIndex = 0;
            this.btnAddRow2.Text = "행추가";
            this.btnAddRow2.ToolTip = "행추가";
            this.btnAddRow2.Click += new System.EventHandler(this.btnAddRow2_Click);
            // 
            // panelWyn1
            // 
            this.panelWyn1.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn1.Appearance.Options.UseBackColor = true;
            this.panelWyn1.Controls.Add(this.sectionHeaderWyn2);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(3, 148);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn1.Size = new System.Drawing.Size(747, 27);
            this.panelWyn1.TabIndex = 1;
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
            this.sectionHeaderWyn2.Text = "창고별 LOCATION정보";
            // 
            // panelWyn5
            // 
            this.panelWyn5.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn5.Appearance.Options.UseBackColor = true;
            this.panelWyn5.Controls.Add(this.panData);
            this.panelWyn5.Controls.Add(this.panelWyn6);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(3, 0);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Size = new System.Drawing.Size(747, 148);
            this.panelWyn5.TabIndex = 0;
            // 
            // panData
            // 
            this.panData.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panData.Appearance.Options.UseBackColor = true;
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Controls.Add(this.labelControl4);
            this.panData.Controls.Add(this.cboDetailAccId);
            this.panData.Controls.Add(this.labelControl5);
            this.panData.Controls.Add(this.txtDetailWhNm);
            this.panData.Controls.Add(this.labelControl6);
            this.panData.Controls.Add(this.txtDetailWhType);
            this.panData.Controls.Add(this.labelControl7);
            this.panData.Controls.Add(this.txtDetailDeptId);
            this.panData.Controls.Add(this.popDetailDeptNm);
            this.panData.Controls.Add(this.labelControl8);
            this.panData.Controls.Add(this.txtDetailEmpId);
            this.panData.Controls.Add(this.popDetailEmpNm);
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(747, 121);
            this.panData.TabIndex = 1;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(27, 26);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(36, 15);
            this.labelControl4.TabIndex = 0;
            this.labelControl4.Text = "사업장";
            // 
            // cboDetailAccId
            // 
            this.cboDetailAccId.EditValue = "";
            this.cboDetailAccId.Location = new System.Drawing.Point(80, 24);
            this.cboDetailAccId.LookupKey = "L_ACC";
            this.cboDetailAccId.Name = "cboDetailAccId";
            this.cboDetailAccId.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.cboDetailAccId.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboDetailAccId.Properties.Appearance.Options.UseBackColor = true;
            this.cboDetailAccId.Properties.Appearance.Options.UseForeColor = true;
            this.cboDetailAccId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailAccId.Properties.NullText = "";
            this.cboDetailAccId.Required = true;
            this.cboDetailAccId.Size = new System.Drawing.Size(180, 20);
            this.cboDetailAccId.TabIndex = 1;
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(27, 52);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(36, 15);
            this.labelControl5.TabIndex = 2;
            this.labelControl5.Text = "창고명";
            // 
            // txtDetailWhNm
            // 
            this.txtDetailWhNm.Location = new System.Drawing.Point(80, 50);
            this.txtDetailWhNm.Name = "txtDetailWhNm";
            this.txtDetailWhNm.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtDetailWhNm.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDetailWhNm.Properties.Appearance.Options.UseBackColor = true;
            this.txtDetailWhNm.Properties.Appearance.Options.UseForeColor = true;
            this.txtDetailWhNm.Required = true;
            this.txtDetailWhNm.Size = new System.Drawing.Size(180, 20);
            this.txtDetailWhNm.TabIndex = 3;
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.labelControl6.Appearance.Options.UseFont = true;
            this.labelControl6.Location = new System.Drawing.Point(284, 52);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(48, 15);
            this.labelControl6.TabIndex = 4;
            this.labelControl6.Text = "창고유형";
            // 
            // txtDetailWhType
            // 
            this.txtDetailWhType.Location = new System.Drawing.Point(344, 50);
            this.txtDetailWhType.Name = "txtDetailWhType";
            this.txtDetailWhType.Size = new System.Drawing.Size(177, 20);
            this.txtDetailWhType.TabIndex = 5;
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.labelControl7.Appearance.Options.UseFont = true;
            this.labelControl7.Location = new System.Drawing.Point(15, 78);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(48, 15);
            this.labelControl7.TabIndex = 6;
            this.labelControl7.Text = "담당부서";
            // 
            // txtDetailDeptId
            // 
            this.txtDetailDeptId.Location = new System.Drawing.Point(202, 76);
            this.txtDetailDeptId.Name = "txtDetailDeptId";
            this.txtDetailDeptId.Properties.ReadOnly = true;
            this.txtDetailDeptId.Size = new System.Drawing.Size(58, 20);
            this.txtDetailDeptId.TabIndex = 7;
            // 
            // popDetailDeptNm
            // 
            this.popDetailDeptNm.Location = new System.Drawing.Point(80, 76);
            this.popDetailDeptNm.LookupKey = "P_DEPT";
            this.popDetailDeptNm.Name = "popDetailDeptNm";
            this.popDetailDeptNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.popDetailDeptNm.Size = new System.Drawing.Size(120, 20);
            this.popDetailDeptNm.TabIndex = 8;
            this.popDetailDeptNm.ToolTip = null;
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.labelControl8.Appearance.Options.UseFont = true;
            this.labelControl8.Location = new System.Drawing.Point(296, 78);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(36, 15);
            this.labelControl8.TabIndex = 9;
            this.labelControl8.Text = "담당자";
            // 
            // txtDetailEmpId
            // 
            this.txtDetailEmpId.Location = new System.Drawing.Point(466, 76);
            this.txtDetailEmpId.Name = "txtDetailEmpId";
            this.txtDetailEmpId.Properties.ReadOnly = true;
            this.txtDetailEmpId.Size = new System.Drawing.Size(55, 20);
            this.txtDetailEmpId.TabIndex = 10;
            // 
            // popDetailEmpNm
            // 
            this.popDetailEmpNm.Location = new System.Drawing.Point(344, 76);
            this.popDetailEmpNm.LookupKey = "P_EMP";
            this.popDetailEmpNm.Name = "popDetailEmpNm";
            this.popDetailEmpNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.popDetailEmpNm.Size = new System.Drawing.Size(120, 20);
            this.popDetailEmpNm.TabIndex = 11;
            this.popDetailEmpNm.ToolTip = null;
            // 
            // panelWyn6
            // 
            this.panelWyn6.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn6.Appearance.Options.UseBackColor = true;
            this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
            this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn6.Location = new System.Drawing.Point(0, 0);
            this.panelWyn6.Name = "panelWyn6";
            this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn6.Size = new System.Drawing.Size(747, 27);
            this.panelWyn6.TabIndex = 0;
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
            this.sectionHeaderWyn3.Size = new System.Drawing.Size(742, 25);
            this.sectionHeaderWyn3.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn3.SvgIcon")));
            this.sectionHeaderWyn3.TabIndex = 10;
            this.sectionHeaderWyn3.Text = "창고정보";
            // 
            // splitterWyn1
            // 
            this.splitterWyn1.BackColor = System.Drawing.Color.White;
            this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitterWyn1.Location = new System.Drawing.Point(399, 0);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(10, 515);
            this.splitterWyn1.TabIndex = 1;
            this.splitterWyn1.TabStop = false;
            // 
            // panelWyn8
            // 
            this.panelWyn8.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn8.Appearance.Options.UseBackColor = true;
            this.panelWyn8.Controls.Add(this.grd1);
            this.panelWyn8.Controls.Add(this.panelWyn2);
            this.panelWyn8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn8.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn8.Location = new System.Drawing.Point(0, 0);
            this.panelWyn8.Name = "panelWyn8";
            this.panelWyn8.Size = new System.Drawing.Size(399, 515);
            this.panelWyn8.TabIndex = 0;
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
            this.grd1.Size = new System.Drawing.Size(399, 488);
            this.grd1.TabIndex = 1;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colWhId,
            this.colWhNm,
            this.colWhType,
            this.colWhDeptNm,
            this.colWhEmpNm});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightFocusedRow = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw1.OptionsSelection.InvertSelection = true;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // colWhId
            // 
            this.colWhId.AppearanceCell.Options.UseTextOptions = true;
            this.colWhId.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWhId.Caption = "李쎄퀬ID";
            this.colWhId.FieldName = "wh_id";
            this.colWhId.Name = "colWhId";
            this.colWhId.Visible = true;
            this.colWhId.VisibleIndex = 0;
            this.colWhId.Width = 60;
            // 
            // colWhNm
            // 
            this.colWhNm.Caption = "창고명";
            this.colWhNm.FieldName = "wh_nm";
            this.colWhNm.Name = "colWhNm";
            this.colWhNm.Visible = true;
            this.colWhNm.VisibleIndex = 1;
            this.colWhNm.Width = 130;
            // 
            // colWhType
            // 
            this.colWhType.Caption = "창고유형";
            this.colWhType.FieldName = "wh_type";
            this.colWhType.Name = "colWhType";
            this.colWhType.Visible = true;
            this.colWhType.VisibleIndex = 2;
            this.colWhType.Width = 80;
            // 
            // colWhDeptNm
            // 
            this.colWhDeptNm.Caption = "담당부서";
            this.colWhDeptNm.FieldName = "dept_nm";
            this.colWhDeptNm.Name = "colWhDeptNm";
            this.colWhDeptNm.Visible = true;
            this.colWhDeptNm.VisibleIndex = 3;
            this.colWhDeptNm.Width = 90;
            // 
            // colWhEmpNm
            // 
            this.colWhEmpNm.Caption = "담당자";
            this.colWhEmpNm.FieldName = "emp_nm";
            this.colWhEmpNm.Name = "colWhEmpNm";
            this.colWhEmpNm.Visible = true;
            this.colWhEmpNm.VisibleIndex = 4;
            this.colWhEmpNm.Width = 70;
            // 
            // panelWyn2
            // 
            this.panelWyn2.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn2.Appearance.Options.UseBackColor = true;
            this.panelWyn2.Controls.Add(this.sectionHeaderWyn4);
            this.panelWyn2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn2.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn2.Location = new System.Drawing.Point(0, 0);
            this.panelWyn2.Name = "panelWyn2";
            this.panelWyn2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn2.Size = new System.Drawing.Size(399, 27);
            this.panelWyn2.TabIndex = 0;
            // 
            // sectionHeaderWyn4
            // 
            this.sectionHeaderWyn4.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn4.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn4.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn4.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn4.Name = "sectionHeaderWyn4";
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(394, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 10;
            this.sectionHeaderWyn4.Text = "李쎄퀬LIST";
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.lblSearchAccId);
            this.panHeader.Controls.Add(this.cboSearchAccId);
            this.panHeader.Controls.Add(this.txtWhNm_Q);
            this.panHeader.Controls.Add(this.labelControl1);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(3, 33);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1159, 49);
            this.panHeader.TabIndex = 2;
            // 
            // lblSearchAccId (조회조건 첫 번째 - 사업장 표준, 2026-10-03)
            // 
            this.lblSearchAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSearchAccId.Appearance.Options.UseFont = true;
            this.lblSearchAccId.Location = new System.Drawing.Point(25, 18);
            this.lblSearchAccId.Name = "lblSearchAccId";
            this.lblSearchAccId.Size = new System.Drawing.Size(36, 15);
            this.lblSearchAccId.TabIndex = 0;
            this.lblSearchAccId.Text = "사업장";
            // 
            // cboSearchAccId (사업장 LookUp, Required - 기본값은 화면 생성자에서 로그인 사업장)
            // 
            this.cboSearchAccId.EditValue = "";
            this.cboSearchAccId.Location = new System.Drawing.Point(69, 15);
            this.cboSearchAccId.LookupKey = "L_ACC";
            this.cboSearchAccId.Name = "cboSearchAccId";
            this.cboSearchAccId.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.cboSearchAccId.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboSearchAccId.Properties.Appearance.Options.UseBackColor = true;
            this.cboSearchAccId.Properties.Appearance.Options.UseForeColor = true;
            this.cboSearchAccId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboSearchAccId.Properties.NullText = "";
            this.cboSearchAccId.Required = true;
            this.cboSearchAccId.Size = new System.Drawing.Size(131, 20);
            this.cboSearchAccId.TabIndex = 1;
            // 
            // txtWhNm_Q
            // 
            this.txtWhNm_Q.Location = new System.Drawing.Point(266, 15);
            this.txtWhNm_Q.Name = "txtWhNm_Q";
            this.txtWhNm_Q.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtWhNm_Q.Size = new System.Drawing.Size(230, 20);
            this.txtWhNm_Q.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(224, 18);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 15);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "창고명";
            // 
            // paTitle
            // 
            this.paTitle.Appearance.BackColor = System.Drawing.Color.White;
            this.paTitle.Appearance.Options.UseBackColor = true;
            this.paTitle.Controls.Add(this.sectionHeaderWyn1);
            this.paTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle.Location = new System.Drawing.Point(3, 0);
            this.paTitle.Name = "paTitle";
            this.paTitle.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.paTitle.Size = new System.Drawing.Size(1159, 33);
            this.paTitle.TabIndex = 1;
            // 
            // sectionHeaderWyn1
            // 
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1154, 33);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 12;
            this.sectionHeaderWyn1.Text = "창고정보등록 [frmWh]";
            // 
            // frmWh
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1165, 600);
            this.Controls.Add(this.panBase);
            this.Name = "frmWh";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
            this.panelWyn7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
            this.panData.ResumeLayout(false);
            this.panData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailAccId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailWhNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailWhType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailDeptId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popDetailDeptNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailEmpId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popDetailEmpNm.Properties)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtWhNm_Q.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
            this.paTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
            this.ResumeLayout(false);

    }

    private SectionHeaderWyn sectionHeaderWyn1;
    private SectionHeaderWyn sectionHeaderWyn4;
    private SectionHeaderWyn sectionHeaderWyn3;
    private SectionHeaderWyn sectionHeaderWyn2;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private WYNLAB.Base.Controls.LookUpEditWyn cboSearchAccId;
}
