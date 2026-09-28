// AI Builder媛 留덉뒪?????쒕툕洹몃━???쒗뵆由우쓣 蹂듭젣?댁꽌 ?먮룞 ?앹꽦 - 2026-09-14.
// ?붿옄???쒕ぉ?곸뿭/?щ갚/?됱긽)??諛붽씀?ㅻ㈃ ???뚯씪???꾨땲???먮낯 ?쒗뵆由?99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)??怨좎튂?몄슂.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.SM;

public partial class frmAutoKey
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAutoKey));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.tabDetailGrids = new WYNLAB.Base.Controls.TabControlWyn();
            this.tabDetail1 = new DevExpress.XtraTab.XtraTabPage();
            this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colD1TableName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1BaseDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1Yyyy = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1Mm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1Dd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1Seq = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spinEditcolD1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colD1NewKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1RegUserId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1RegDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dateEditcolD1 = new WYNLAB.Base.Controls.DateColumnEdit();
            this.colD1UptUserId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1UptDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDeletRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAddRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.lblDetailTableName = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailTableName = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailTableDesc = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailTableDesc = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailPreFix = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailPreFix = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailKeyCol = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailKeyCol = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailDateCol = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailDateCol = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailDateType = new DevExpress.XtraEditors.LabelControl();
            this.cboDetailDateType = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblDetailSeqLen = new DevExpress.XtraEditors.LabelControl();
            this.numDetailSeqLen = new WYNLAB.Base.Controls.SpinEditWyn();
            this.lblDetailRegUserId = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailRegUserId = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailRegDt = new DevExpress.XtraEditors.LabelControl();
            this.dteDetailRegDt = new WYNLAB.Base.Controls.DateEditWyn();
            this.lblDetailUptUserId = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailUptUserId = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailUptDt = new DevExpress.XtraEditors.LabelControl();
            this.dteDetailUptDt = new WYNLAB.Base.Controls.DateEditWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colMTableName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMTableDesc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMPreFix = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMKeyCol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMDateCol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMDateType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpcolMDateType = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colMSeqLen = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spinEditcolM = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colMRegUserId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMRegDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dateEditcolM = new WYNLAB.Base.Controls.DateColumnEdit();
            this.colMUptUserId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMUptDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.lblSearchTableName = new DevExpress.XtraEditors.LabelControl();
            this.txtTableName = new WYNLAB.Base.Controls.TextEditWyn();
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
            this.tabDetail1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditcolD1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcolD1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcolD1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailTableName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailTableDesc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailPreFix.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailKeyCol.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailDateCol.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailDateType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDetailSeqLen.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailRegUserId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDetailRegDt.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDetailRegDt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailUptUserId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDetailUptDt.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDetailUptDt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
            this.panelWyn6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).BeginInit();
            this.panelWyn8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcolMDateType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditcolM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcolM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcolM.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
            this.panelWyn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTableName.Properties)).BeginInit();
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
            this.panelWyn3.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn3.Appearance.Options.UseBackColor = true;
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
            this.panelWyn4.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn4.Appearance.Options.UseBackColor = true;
            this.panelWyn4.Controls.Add(this.tabDetailGrids);
            this.panelWyn4.Controls.Add(this.panelWyn7);
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
            this.tabDetailGrids.Location = new System.Drawing.Point(3, 145);
            this.tabDetailGrids.Name = "tabDetailGrids";
            this.tabDetailGrids.SelectedTabPage = this.tabDetail1;
            this.tabDetailGrids.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            this.tabDetailGrids.Size = new System.Drawing.Size(820, 348);
            this.tabDetailGrids.TabIndex = 7;
            this.tabDetailGrids.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabDetail1});
            // 
            // tabDetail1
            // 
            this.tabDetail1.Controls.Add(this.grd2);
            this.tabDetail1.Name = "tabDetail1";
            this.tabDetail1.Size = new System.Drawing.Size(818, 346);
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
            this.grd2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.spinEditcolD1,
            this.dateEditcolD1});
            this.grd2.Size = new System.Drawing.Size(818, 346);
            this.grd2.TabIndex = 0;
            this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw2});
            // 
            // gvw2
            // 
            this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colD1TableName,
            this.colD1BaseDate,
            this.colD1Yyyy,
            this.colD1Mm,
            this.colD1Dd,
            this.colD1Seq,
            this.colD1NewKey,
            this.colD1RegUserId,
            this.colD1RegDt,
            this.colD1UptUserId,
            this.colD1UptDt});
            this.gvw2.GridControl = this.grd2;
            this.gvw2.HighlightFocusedRow = true;
            this.gvw2.Name = "gvw2";
            this.gvw2.OptionsBehavior.Editable = false;
            this.gvw2.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw2.OptionsView.ColumnAutoWidth = false;
            this.gvw2.OptionsView.ShowGroupPanel = false;
            // 
            // colD1TableName
            // 
            this.colD1TableName.Caption = "table_name";
            this.colD1TableName.FieldName = "table_name";
            this.colD1TableName.Name = "colD1TableName";
            this.colD1TableName.Visible = true;
            this.colD1TableName.VisibleIndex = 0;
            this.colD1TableName.Width = 100;
            // 
            // colD1BaseDate
            // 
            this.colD1BaseDate.Caption = "base_date";
            this.colD1BaseDate.FieldName = "base_date";
            this.colD1BaseDate.Name = "colD1BaseDate";
            this.colD1BaseDate.Visible = true;
            this.colD1BaseDate.VisibleIndex = 1;
            this.colD1BaseDate.Width = 100;
            // 
            // colD1Yyyy
            // 
            this.colD1Yyyy.Caption = "yyyy";
            this.colD1Yyyy.FieldName = "yyyy";
            this.colD1Yyyy.Name = "colD1Yyyy";
            this.colD1Yyyy.Visible = true;
            this.colD1Yyyy.VisibleIndex = 2;
            this.colD1Yyyy.Width = 100;
            // 
            // colD1Mm
            // 
            this.colD1Mm.Caption = "mm";
            this.colD1Mm.FieldName = "mm";
            this.colD1Mm.Name = "colD1Mm";
            this.colD1Mm.Visible = true;
            this.colD1Mm.VisibleIndex = 3;
            this.colD1Mm.Width = 100;
            // 
            // colD1Dd
            // 
            this.colD1Dd.Caption = "dd";
            this.colD1Dd.FieldName = "dd";
            this.colD1Dd.Name = "colD1Dd";
            this.colD1Dd.Visible = true;
            this.colD1Dd.VisibleIndex = 4;
            this.colD1Dd.Width = 100;
            // 
            // colD1Seq
            // 
            this.colD1Seq.Caption = "seq";
            this.colD1Seq.ColumnEdit = this.spinEditcolD1;
            this.colD1Seq.FieldName = "seq";
            this.colD1Seq.Name = "colD1Seq";
            this.colD1Seq.Visible = true;
            this.colD1Seq.VisibleIndex = 5;
            this.colD1Seq.Width = 100;
            // 
            // spinEditcolD1
            // 
            this.spinEditcolD1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditcolD1.Name = "spinEditcolD1";
            // 
            // colD1NewKey
            // 
            this.colD1NewKey.Caption = "new_key";
            this.colD1NewKey.FieldName = "new_key";
            this.colD1NewKey.Name = "colD1NewKey";
            this.colD1NewKey.Visible = true;
            this.colD1NewKey.VisibleIndex = 6;
            this.colD1NewKey.Width = 100;
            // 
            // colD1RegUserId
            // 
            this.colD1RegUserId.Caption = "reg_user_id";
            this.colD1RegUserId.FieldName = "reg_user_id";
            this.colD1RegUserId.Name = "colD1RegUserId";
            this.colD1RegUserId.Visible = true;
            this.colD1RegUserId.VisibleIndex = 7;
            this.colD1RegUserId.Width = 100;
            // 
            // colD1RegDt
            // 
            this.colD1RegDt.Caption = "reg_dt";
            this.colD1RegDt.ColumnEdit = this.dateEditcolD1;
            this.colD1RegDt.FieldName = "reg_dt";
            this.colD1RegDt.Name = "colD1RegDt";
            this.colD1RegDt.Visible = true;
            this.colD1RegDt.VisibleIndex = 8;
            this.colD1RegDt.Width = 100;
            // 
            // dateEditcolD1
            // 
            this.dateEditcolD1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditcolD1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditcolD1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dateEditcolD1.Name = "dateEditcolD1";
            // 
            // colD1UptUserId
            // 
            this.colD1UptUserId.Caption = "upt_user_id";
            this.colD1UptUserId.FieldName = "upt_user_id";
            this.colD1UptUserId.Name = "colD1UptUserId";
            this.colD1UptUserId.Visible = true;
            this.colD1UptUserId.VisibleIndex = 9;
            this.colD1UptUserId.Width = 100;
            // 
            // colD1UptDt
            // 
            this.colD1UptDt.Caption = "upt_dt";
            this.colD1UptDt.ColumnEdit = this.dateEditcolD1;
            this.colD1UptDt.FieldName = "upt_dt";
            this.colD1UptDt.Name = "colD1UptDt";
            this.colD1UptDt.Visible = true;
            this.colD1UptDt.VisibleIndex = 10;
            this.colD1UptDt.Width = 100;
            // 
            // panelWyn1
            // 
            this.panelWyn1.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn1.Appearance.Options.UseBackColor = true;
            this.panelWyn1.Controls.Add(this.sectionHeaderWyn2);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(3, 118);
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
            this.panelWyn7.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn7.Appearance.Options.UseBackColor = true;
            this.panelWyn7.Controls.Add(this.btnDeletRow2);
            this.panelWyn7.Controls.Add(this.btnAddRow2);
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(752, 0);
            this.panelWyn7.Name = "panelWyn7";
            this.panelWyn7.Padding = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.panelWyn7.Size = new System.Drawing.Size(68, 30);
            this.panelWyn7.TabIndex = 9;
            // 
            // btnDeletRow2
            // 
            this.btnDeletRow2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
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
            this.btnDeletRow2.Size = new System.Drawing.Size(58, 22);
            this.btnDeletRow2.Text = "?됱궘??";
            this.btnDeletRow2.TabIndex = 0;
            this.btnDeletRow2.ToolTip = "행삭제(현재 탭)";
            // 
            // btnAddRow2
            // 
            this.btnAddRow2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
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
            this.btnAddRow2.Size = new System.Drawing.Size(58, 22);
            this.btnAddRow2.Text = "?됱텛媛";
            this.btnAddRow2.TabIndex = 0;
            this.btnAddRow2.ToolTip = "행추가(현재 탭)";
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
            this.panelWyn5.Size = new System.Drawing.Size(820, 118);
            this.panelWyn5.TabIndex = 6;
            // 
            // panData
            // 
            this.panData.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panData.Appearance.Options.UseBackColor = true;
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Controls.Add(this.lblDetailTableName);
            this.panData.Controls.Add(this.txtDetailTableName);
            this.panData.Controls.Add(this.lblDetailTableDesc);
            this.panData.Controls.Add(this.txtDetailTableDesc);
            this.panData.Controls.Add(this.lblDetailPreFix);
            this.panData.Controls.Add(this.txtDetailPreFix);
            this.panData.Controls.Add(this.lblDetailKeyCol);
            this.panData.Controls.Add(this.txtDetailKeyCol);
            this.panData.Controls.Add(this.lblDetailDateCol);
            this.panData.Controls.Add(this.txtDetailDateCol);
            this.panData.Controls.Add(this.lblDetailDateType);
            this.panData.Controls.Add(this.cboDetailDateType);
            this.panData.Controls.Add(this.lblDetailSeqLen);
            this.panData.Controls.Add(this.numDetailSeqLen);
            this.panData.Controls.Add(this.lblDetailRegUserId);
            this.panData.Controls.Add(this.txtDetailRegUserId);
            this.panData.Controls.Add(this.lblDetailRegDt);
            this.panData.Controls.Add(this.dteDetailRegDt);
            this.panData.Controls.Add(this.lblDetailUptUserId);
            this.panData.Controls.Add(this.txtDetailUptUserId);
            this.panData.Controls.Add(this.lblDetailUptDt);
            this.panData.Controls.Add(this.dteDetailUptDt);
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(820, 91);
            this.panData.TabIndex = 8;
            // 
            // lblDetailTableName
            // 
            this.lblDetailTableName.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailTableName.Appearance.Options.UseFont = true;
            this.lblDetailTableName.Location = new System.Drawing.Point(24, 19);
            this.lblDetailTableName.Name = "lblDetailTableName";
            this.lblDetailTableName.Size = new System.Drawing.Size(40, 15);
            this.lblDetailTableName.TabIndex = 0;
            this.lblDetailTableName.Text = "TableID";
            // 
            // txtDetailTableName
            // 
            this.txtDetailTableName.Location = new System.Drawing.Point(70, 16);
            this.txtDetailTableName.Name = "txtDetailTableName";
            this.txtDetailTableName.Size = new System.Drawing.Size(128, 20);
            this.txtDetailTableName.TabIndex = 1;
            // 
            // lblDetailTableDesc
            // 
            this.lblDetailTableDesc.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailTableDesc.Appearance.Options.UseFont = true;
            this.lblDetailTableDesc.Location = new System.Drawing.Point(222, 19);
            this.lblDetailTableDesc.Name = "lblDetailTableDesc";
            this.lblDetailTableDesc.Size = new System.Drawing.Size(40, 15);
            this.lblDetailTableDesc.TabIndex = 2;
            this.lblDetailTableDesc.Text = "Table紐?";
            // 
            // txtDetailTableDesc
            // 
            this.txtDetailTableDesc.Location = new System.Drawing.Point(268, 16);
            this.txtDetailTableDesc.Name = "txtDetailTableDesc";
            this.txtDetailTableDesc.Size = new System.Drawing.Size(232, 20);
            this.txtDetailTableDesc.TabIndex = 3;
            // 
            // lblDetailPreFix
            // 
            this.lblDetailPreFix.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailPreFix.Appearance.Options.UseFont = true;
            this.lblDetailPreFix.Location = new System.Drawing.Point(528, 18);
            this.lblDetailPreFix.Name = "lblDetailPreFix";
            this.lblDetailPreFix.Size = new System.Drawing.Size(48, 15);
            this.lblDetailPreFix.TabIndex = 4;
            this.lblDetailPreFix.Text = "梨꾨쾲肄붾뱶";
            // 
            // txtDetailPreFix
            // 
            this.txtDetailPreFix.Location = new System.Drawing.Point(580, 15);
            this.txtDetailPreFix.Name = "txtDetailPreFix";
            this.txtDetailPreFix.Size = new System.Drawing.Size(81, 20);
            this.txtDetailPreFix.TabIndex = 5;
            // 
            // lblDetailKeyCol
            // 
            this.lblDetailKeyCol.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailKeyCol.Appearance.Options.UseFont = true;
            this.lblDetailKeyCol.Location = new System.Drawing.Point(16, 53);
            this.lblDetailKeyCol.Name = "lblDetailKeyCol";
            this.lblDetailKeyCol.Size = new System.Drawing.Size(48, 15);
            this.lblDetailKeyCol.TabIndex = 6;
            this.lblDetailKeyCol.Text = "梨꾨쾲而щ읆";
            // 
            // txtDetailKeyCol
            // 
            this.txtDetailKeyCol.Location = new System.Drawing.Point(70, 50);
            this.txtDetailKeyCol.Name = "txtDetailKeyCol";
            this.txtDetailKeyCol.Size = new System.Drawing.Size(128, 20);
            this.txtDetailKeyCol.TabIndex = 7;
            // 
            // lblDetailDateCol
            // 
            this.lblDetailDateCol.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailDateCol.Appearance.Options.UseFont = true;
            this.lblDetailDateCol.Location = new System.Drawing.Point(210, 50);
            this.lblDetailDateCol.Name = "lblDetailDateCol";
            this.lblDetailDateCol.Size = new System.Drawing.Size(52, 15);
            this.lblDetailDateCol.TabIndex = 8;
            this.lblDetailDateCol.Text = "?쇱옄 而щ읆";
            // 
            // txtDetailDateCol
            // 
            this.txtDetailDateCol.Location = new System.Drawing.Point(268, 47);
            this.txtDetailDateCol.Name = "txtDetailDateCol";
            this.txtDetailDateCol.Size = new System.Drawing.Size(93, 20);
            this.txtDetailDateCol.TabIndex = 9;
            // 
            // lblDetailDateType
            // 
            this.lblDetailDateType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailDateType.Appearance.Options.UseFont = true;
            this.lblDetailDateType.Location = new System.Drawing.Point(363, 50);
            this.lblDetailDateType.Name = "lblDetailDateType";
            this.lblDetailDateType.Size = new System.Drawing.Size(48, 15);
            this.lblDetailDateType.TabIndex = 10;
            this.lblDetailDateType.Text = "?좎쭨?좏삎";
            // 
            // cboDetailDateType
            // 
            this.cboDetailDateType.EditValue = "";
            this.cboDetailDateType.Location = new System.Drawing.Point(417, 47);
            this.cboDetailDateType.LookupKey = "L_SM0006";
            this.cboDetailDateType.Name = "cboDetailDateType";
            this.cboDetailDateType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailDateType.Properties.NullText = "";
            this.cboDetailDateType.Size = new System.Drawing.Size(83, 20);
            this.cboDetailDateType.TabIndex = 11;
            // 
            // lblDetailSeqLen
            // 
            this.lblDetailSeqLen.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailSeqLen.Appearance.Options.UseFont = true;
            this.lblDetailSeqLen.Location = new System.Drawing.Point(516, 50);
            this.lblDetailSeqLen.Name = "lblDetailSeqLen";
            this.lblDetailSeqLen.Size = new System.Drawing.Size(60, 15);
            this.lblDetailSeqLen.TabIndex = 12;
            this.lblDetailSeqLen.Text = "?쒕쾲?먮━??";
            // 
            // numDetailSeqLen
            // 
            this.numDetailSeqLen.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numDetailSeqLen.Location = new System.Drawing.Point(580, 47);
            this.numDetailSeqLen.Name = "numDetailSeqLen";
            this.numDetailSeqLen.Properties.MaxValue = new decimal(new int[] {
            -727379969,
            232,
            0,
            0});
            this.numDetailSeqLen.Size = new System.Drawing.Size(81, 20);
            this.numDetailSeqLen.TabIndex = 13;
            // 
            // lblDetailRegUserId
            // 
            this.lblDetailRegUserId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailRegUserId.Appearance.Options.UseFont = true;
            this.lblDetailRegUserId.Location = new System.Drawing.Point(399, 246);
            this.lblDetailRegUserId.Name = "lblDetailRegUserId";
            this.lblDetailRegUserId.Size = new System.Drawing.Size(59, 15);
            this.lblDetailRegUserId.TabIndex = 14;
            this.lblDetailRegUserId.Text = "reg_user_id";
            this.lblDetailRegUserId.Visible = false;
            // 
            // txtDetailRegUserId
            // 
            this.txtDetailRegUserId.Location = new System.Drawing.Point(502, 243);
            this.txtDetailRegUserId.Name = "txtDetailRegUserId";
            this.txtDetailRegUserId.Size = new System.Drawing.Size(220, 20);
            this.txtDetailRegUserId.TabIndex = 15;
            this.txtDetailRegUserId.Visible = false;
            // 
            // lblDetailRegDt
            // 
            this.lblDetailRegDt.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailRegDt.Appearance.Options.UseFont = true;
            this.lblDetailRegDt.Location = new System.Drawing.Point(425, 274);
            this.lblDetailRegDt.Name = "lblDetailRegDt";
            this.lblDetailRegDt.Size = new System.Drawing.Size(33, 15);
            this.lblDetailRegDt.TabIndex = 16;
            this.lblDetailRegDt.Text = "reg_dt";
            this.lblDetailRegDt.Visible = false;
            // 
            // dteDetailRegDt
            // 
            this.dteDetailRegDt.EditValue = new System.DateTime(2026, 9, 22, 0, 0, 0, 0);
            this.dteDetailRegDt.Location = new System.Drawing.Point(502, 271);
            this.dteDetailRegDt.Name = "dteDetailRegDt";
            this.dteDetailRegDt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteDetailRegDt.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteDetailRegDt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dteDetailRegDt.Size = new System.Drawing.Size(220, 20);
            this.dteDetailRegDt.TabIndex = 17;
            this.dteDetailRegDt.Visible = false;
            this.dteDetailRegDt.YyyyMmDd = "20260922";
            // 
            // lblDetailUptUserId
            // 
            this.lblDetailUptUserId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailUptUserId.Appearance.Options.UseFont = true;
            this.lblDetailUptUserId.Location = new System.Drawing.Point(398, 302);
            this.lblDetailUptUserId.Name = "lblDetailUptUserId";
            this.lblDetailUptUserId.Size = new System.Drawing.Size(60, 15);
            this.lblDetailUptUserId.TabIndex = 18;
            this.lblDetailUptUserId.Text = "upt_user_id";
            this.lblDetailUptUserId.Visible = false;
            // 
            // txtDetailUptUserId
            // 
            this.txtDetailUptUserId.Location = new System.Drawing.Point(502, 299);
            this.txtDetailUptUserId.Name = "txtDetailUptUserId";
            this.txtDetailUptUserId.Size = new System.Drawing.Size(220, 20);
            this.txtDetailUptUserId.TabIndex = 19;
            this.txtDetailUptUserId.Visible = false;
            // 
            // lblDetailUptDt
            // 
            this.lblDetailUptDt.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailUptDt.Appearance.Options.UseFont = true;
            this.lblDetailUptDt.Location = new System.Drawing.Point(424, 330);
            this.lblDetailUptDt.Name = "lblDetailUptDt";
            this.lblDetailUptDt.Size = new System.Drawing.Size(34, 15);
            this.lblDetailUptDt.TabIndex = 20;
            this.lblDetailUptDt.Text = "upt_dt";
            this.lblDetailUptDt.Visible = false;
            // 
            // dteDetailUptDt
            // 
            this.dteDetailUptDt.EditValue = new System.DateTime(2026, 9, 22, 0, 0, 0, 0);
            this.dteDetailUptDt.Location = new System.Drawing.Point(502, 327);
            this.dteDetailUptDt.Name = "dteDetailUptDt";
            this.dteDetailUptDt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteDetailUptDt.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteDetailUptDt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dteDetailUptDt.Size = new System.Drawing.Size(220, 20);
            this.dteDetailUptDt.TabIndex = 21;
            this.dteDetailUptDt.Visible = false;
            this.dteDetailUptDt.YyyyMmDd = "20260922";
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
            this.panelWyn8.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn8.Appearance.Options.UseBackColor = true;
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
            this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.spinEditcolM,
            this.dateEditcolM,
            this.lookUpcolMDateType});
            this.grd1.Size = new System.Drawing.Size(402, 466);
            this.grd1.TabIndex = 10;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colMTableName,
            this.colMTableDesc,
            this.colMPreFix,
            this.colMKeyCol,
            this.colMDateCol,
            this.colMDateType,
            this.colMSeqLen,
            this.colMRegUserId,
            this.colMRegDt,
            this.colMUptUserId,
            this.colMUptDt});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightFocusedRow = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw1.OptionsSelection.InvertSelection = true;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // colMTableName
            // 
            this.colMTableName.Caption = "TableID";
            this.colMTableName.FieldName = "table_name";
            this.colMTableName.Name = "colMTableName";
            this.colMTableName.Visible = true;
            this.colMTableName.VisibleIndex = 0;
            this.colMTableName.Width = 100;
            // 
            // colMTableDesc
            // 
            this.colMTableDesc.Caption = "Table紐?";
            this.colMTableDesc.FieldName = "table_desc";
            this.colMTableDesc.Name = "colMTableDesc";
            this.colMTableDesc.Visible = true;
            this.colMTableDesc.VisibleIndex = 1;
            this.colMTableDesc.Width = 100;
            // 
            // colMPreFix
            // 
            this.colMPreFix.Caption = "梨꾨쾲肄붾뱶";
            this.colMPreFix.FieldName = "pre_fix";
            this.colMPreFix.Name = "colMPreFix";
            this.colMPreFix.Visible = true;
            this.colMPreFix.VisibleIndex = 2;
            this.colMPreFix.Width = 100;
            // 
            // colMKeyCol
            // 
            this.colMKeyCol.Caption = "梨꾨쾲而щ읆";
            this.colMKeyCol.FieldName = "key_col";
            this.colMKeyCol.Name = "colMKeyCol";
            this.colMKeyCol.Visible = true;
            this.colMKeyCol.VisibleIndex = 3;
            this.colMKeyCol.Width = 100;
            // 
            // colMDateCol
            // 
            this.colMDateCol.Caption = "date_col";
            this.colMDateCol.FieldName = "date_col";
            this.colMDateCol.Name = "colMDateCol";
            this.colMDateCol.Visible = true;
            this.colMDateCol.VisibleIndex = 4;
            this.colMDateCol.Width = 100;
            // 
            // colMDateType
            // 
            this.colMDateType.Caption = "?좎쭨?좏삎";
            this.colMDateType.ColumnEdit = this.lookUpcolMDateType;
            this.colMDateType.FieldName = "date_type";
            this.colMDateType.Name = "colMDateType";
            this.colMDateType.Visible = true;
            this.colMDateType.VisibleIndex = 5;
            this.colMDateType.Width = 100;
            // 
            // lookUpcolMDateType
            // 
            this.lookUpcolMDateType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpcolMDateType.LookupKey = "L_SM0006";
            this.lookUpcolMDateType.Name = "lookUpcolMDateType";
            this.lookUpcolMDateType.NullText = "";
            this.lookUpcolMDateType.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colMSeqLen
            // 
            this.colMSeqLen.Caption = "?쒕쾲?먮━??";
            this.colMSeqLen.ColumnEdit = this.spinEditcolM;
            this.colMSeqLen.FieldName = "seq_len";
            this.colMSeqLen.Name = "colMSeqLen";
            this.colMSeqLen.Visible = true;
            this.colMSeqLen.VisibleIndex = 6;
            this.colMSeqLen.Width = 100;
            // 
            // spinEditcolM
            // 
            this.spinEditcolM.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditcolM.Name = "spinEditcolM";
            // 
            // colMRegUserId
            // 
            this.colMRegUserId.Caption = "reg_user_id";
            this.colMRegUserId.FieldName = "reg_user_id";
            this.colMRegUserId.Name = "colMRegUserId";
            this.colMRegUserId.Visible = true;
            this.colMRegUserId.VisibleIndex = 7;
            this.colMRegUserId.Width = 100;
            // 
            // colMRegDt
            // 
            this.colMRegDt.Caption = "reg_dt";
            this.colMRegDt.ColumnEdit = this.dateEditcolM;
            this.colMRegDt.FieldName = "reg_dt";
            this.colMRegDt.Name = "colMRegDt";
            this.colMRegDt.Visible = true;
            this.colMRegDt.VisibleIndex = 8;
            this.colMRegDt.Width = 100;
            // 
            // dateEditcolM
            // 
            this.dateEditcolM.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditcolM.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditcolM.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dateEditcolM.Name = "dateEditcolM";
            // 
            // colMUptUserId
            // 
            this.colMUptUserId.Caption = "upt_user_id";
            this.colMUptUserId.FieldName = "upt_user_id";
            this.colMUptUserId.Name = "colMUptUserId";
            this.colMUptUserId.Visible = true;
            this.colMUptUserId.VisibleIndex = 9;
            this.colMUptUserId.Width = 100;
            // 
            // colMUptDt
            // 
            this.colMUptDt.Caption = "upt_dt";
            this.colMUptDt.ColumnEdit = this.dateEditcolM;
            this.colMUptDt.FieldName = "upt_dt";
            this.colMUptDt.Name = "colMUptDt";
            this.colMUptDt.Visible = true;
            this.colMUptDt.VisibleIndex = 10;
            this.colMUptDt.Width = 100;
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
            this.panHeader.Controls.Add(this.lblSearchTableName);
            this.panHeader.Controls.Add(this.txtTableName);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 33);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1235, 49);
            this.panHeader.TabIndex = 8;
            // 
            // lblSearchTableName
            // 
            this.lblSearchTableName.Location = new System.Drawing.Point(18, 23);
            this.lblSearchTableName.Name = "lblSearchTableName";
            this.lblSearchTableName.Size = new System.Drawing.Size(36, 14);
            this.lblSearchTableName.TabIndex = 1;
            this.lblSearchTableName.Text = "TABLE";
            // 
            // txtTableName
            // 
            this.txtTableName.Location = new System.Drawing.Point(60, 20);
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.Size = new System.Drawing.Size(150, 20);
            this.txtTableName.TabIndex = 2;
            // 
            // paTitle
            // 
            this.paTitle.Appearance.BackColor = System.Drawing.Color.White;
            this.paTitle.Appearance.Options.UseBackColor = true;
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
            this.sectionHeaderWyn1.Text = "자동채번등록 [frmAutoKey]";
            // 
            // frmAutoKey
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1245, 580);
            this.Controls.Add(this.panBase);
            this.Name = "frmAutoKey";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabDetailGrids)).EndInit();
            this.tabDetailGrids.ResumeLayout(false);
            this.tabDetail1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditcolD1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcolD1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcolD1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
            this.panelWyn7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
            this.panData.ResumeLayout(false);
            this.panData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailTableName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailTableDesc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailPreFix.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailKeyCol.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailDateCol.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailDateType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDetailSeqLen.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailRegUserId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDetailRegDt.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDetailRegDt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailUptUserId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDetailUptDt.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDetailUptDt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
            this.panelWyn6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).EndInit();
            this.panelWyn8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcolMDateType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditcolM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcolM.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcolM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
            this.panelWyn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTableName.Properties)).EndInit();
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
    private DevExpress.XtraGrid.Columns.GridColumn colD1TableName;
    private DevExpress.XtraGrid.Columns.GridColumn colD1BaseDate;
    private DevExpress.XtraGrid.Columns.GridColumn colD1Yyyy;
    private DevExpress.XtraGrid.Columns.GridColumn colD1Mm;
    private DevExpress.XtraGrid.Columns.GridColumn colD1Dd;
    private DevExpress.XtraGrid.Columns.GridColumn colD1Seq;
    private DevExpress.XtraGrid.Columns.GridColumn colD1NewKey;
    private DevExpress.XtraGrid.Columns.GridColumn colD1RegUserId;
    private DevExpress.XtraGrid.Columns.GridColumn colD1RegDt;
    private DevExpress.XtraGrid.Columns.GridColumn colD1UptUserId;
    private DevExpress.XtraGrid.Columns.GridColumn colD1UptDt;
    private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spinEditcolD1;
    private WYNLAB.Base.Controls.DateColumnEdit dateEditcolD1;
    private PanelWyn panelWyn1;
    private PanelWyn panelWyn7;
    private ButtonWyn btnDeletRow2;
    private ButtonWyn btnAddRow2;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblDetailTableName;
    private TextEditWyn txtDetailTableName;
    private DevExpress.XtraEditors.LabelControl lblDetailTableDesc;
    private TextEditWyn txtDetailTableDesc;
    private DevExpress.XtraEditors.LabelControl lblDetailPreFix;
    private TextEditWyn txtDetailPreFix;
    private DevExpress.XtraEditors.LabelControl lblDetailKeyCol;
    private TextEditWyn txtDetailKeyCol;
    private DevExpress.XtraEditors.LabelControl lblDetailDateCol;
    private TextEditWyn txtDetailDateCol;
    private DevExpress.XtraEditors.LabelControl lblDetailDateType;
    private LookUpEditWyn cboDetailDateType;
    private DevExpress.XtraEditors.LabelControl lblDetailSeqLen;
    private SpinEditWyn numDetailSeqLen;
    private DevExpress.XtraEditors.LabelControl lblDetailRegUserId;
    private TextEditWyn txtDetailRegUserId;
    private DevExpress.XtraEditors.LabelControl lblDetailRegDt;
    private DateEditWyn dteDetailRegDt;
    private DevExpress.XtraEditors.LabelControl lblDetailUptUserId;
    private TextEditWyn txtDetailUptUserId;
    private DevExpress.XtraEditors.LabelControl lblDetailUptDt;
    private DateEditWyn dteDetailUptDt;
    private PanelWyn panelWyn6;
    private SplitterWyn splitterWyn1;
    private PanelWyn panelWyn8;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colMTableName;
    private DevExpress.XtraGrid.Columns.GridColumn colMTableDesc;
    private DevExpress.XtraGrid.Columns.GridColumn colMPreFix;
    private DevExpress.XtraGrid.Columns.GridColumn colMKeyCol;
    private DevExpress.XtraGrid.Columns.GridColumn colMDateCol;
    private DevExpress.XtraGrid.Columns.GridColumn colMDateType;
    private DevExpress.XtraGrid.Columns.GridColumn colMSeqLen;
    private DevExpress.XtraGrid.Columns.GridColumn colMRegUserId;
    private DevExpress.XtraGrid.Columns.GridColumn colMRegDt;
    private DevExpress.XtraGrid.Columns.GridColumn colMUptUserId;
    private DevExpress.XtraGrid.Columns.GridColumn colMUptDt;
    private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spinEditcolM;
    private WYNLAB.Base.Controls.DateColumnEdit dateEditcolM;
    private LookUpColumnEdit lookUpcolMDateType;
    private PanelWyn panelWyn2;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchTableName;
    private TextEditWyn txtTableName;
    private PanelWyn paTitle;
    private SectionHeaderWyn sectionHeaderWyn2;
    private SectionHeaderWyn sectionHeaderWyn3;
    private SectionHeaderWyn sectionHeaderWyn4;
    private SectionHeaderWyn sectionHeaderWyn1;
}
