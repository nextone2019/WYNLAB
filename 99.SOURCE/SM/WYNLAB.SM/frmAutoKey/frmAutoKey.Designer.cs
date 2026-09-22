// AI Builder가 마스터-폼-서브그리드 템플릿을 복제해서 자동 생성 - 2026-09-14.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
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
        this.colD1NewKey = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colD1RegUserId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colD1RegDt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colD1UptUserId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colD1UptDt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.spinEditcolD1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
        this.dateEditcolD1 = new WYNLAB.Base.Controls.DateColumnEdit();
            this.tabDetail2 = new DevExpress.XtraTab.XtraTabPage();
            this.grd3 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw3 = new WYNLAB.Base.Controls.GridViewWyn();
            this.tabDetail3 = new DevExpress.XtraTab.XtraTabPage();
            this.grd4 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw4 = new WYNLAB.Base.Controls.GridViewWyn();
            this.tabDetail4 = new DevExpress.XtraTab.XtraTabPage();
            this.grd5 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw5 = new WYNLAB.Base.Controls.GridViewWyn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
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
        this.colMSeqLen = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMRegUserId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMRegDt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMUptUserId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMUptDt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.spinEditcolM = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
        this.dateEditcolM = new WYNLAB.Base.Controls.DateColumnEdit();
        this.lookUpcolMDateType = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
        this.lblSearchTableName = new DevExpress.XtraEditors.LabelControl();
        this.txtTableName = new WYNLAB.Base.Controls.TextEditWyn();
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
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
            this.tabDetail2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw3)).BeginInit();
            this.tabDetail3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw4)).BeginInit();
            this.tabDetail4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
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
            this.tabDetailGrids.Location = new System.Drawing.Point(3, 245);
            this.tabDetailGrids.Name = "tabDetailGrids";
            this.tabDetailGrids.SelectedTabPage = this.tabDetail1;
            this.tabDetailGrids.Size = new System.Drawing.Size(820, 248);
            this.tabDetailGrids.TabIndex = 7;
            this.tabDetailGrids.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabDetail1,
            this.tabDetail2,
            this.tabDetail3,
            this.tabDetail4});
            // 
            // tabDetail1
            // 
            this.tabDetail1.Controls.Add(this.grd2);
            this.tabDetail1.Name = "tabDetail1";
            this.tabDetail1.Size = new System.Drawing.Size(818, 222);
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
            this.grd2.Size = new System.Drawing.Size(818, 222);
            this.grd2.TabIndex = 0;
            this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw2});
            //
            // gvw2
            //
            this.gvw2.GridControl = this.grd2;
            this.gvw2.HighlightFocusedRow = true;
            this.gvw2.Name = "gvw2";
            this.gvw2.OptionsBehavior.Editable = false;
            this.gvw2.OptionsView.ColumnAutoWidth = false;
            this.gvw2.OptionsView.ShowGroupPanel = false;
            //
            // colD1
            //
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
        this.colD1Seq.FieldName = "seq";
        this.colD1Seq.Name = "colD1Seq";
        this.colD1Seq.ColumnEdit = this.spinEditcolD1;
        this.colD1Seq.Visible = true;
        this.colD1Seq.VisibleIndex = 5;
        this.colD1Seq.Width = 100;
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
        this.colD1RegDt.FieldName = "reg_dt";
        this.colD1RegDt.Name = "colD1RegDt";
        this.colD1RegDt.ColumnEdit = this.dateEditcolD1;
        this.colD1RegDt.Visible = true;
        this.colD1RegDt.VisibleIndex = 8;
        this.colD1RegDt.Width = 100;
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
        this.colD1UptDt.FieldName = "upt_dt";
        this.colD1UptDt.Name = "colD1UptDt";
        this.colD1UptDt.ColumnEdit = this.dateEditcolD1;
        this.colD1UptDt.Visible = true;
        this.colD1UptDt.VisibleIndex = 10;
        this.colD1UptDt.Width = 100;
        //
        // spinEditcolD1
        //
        this.spinEditcolD1.Name = "spinEditcolD1";
        //
        // dateEditcolD1
        //
        this.dateEditcolD1.Name = "dateEditcolD1";
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
        this.grd2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { this.spinEditcolD1, this.dateEditcolD1});
            //
            // tabDetail2
            // 
            this.tabDetail2.Controls.Add(this.grd3);
            this.tabDetail2.Name = "tabDetail2";
            this.tabDetail2.Size = new System.Drawing.Size(822, 224);
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
            this.grd3.Size = new System.Drawing.Size(822, 224);
            this.grd3.TabIndex = 0;
            this.grd3.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw3});
            //
            // gvw3
            //
            this.gvw3.GridControl = this.grd3;
            this.gvw3.HighlightFocusedRow = true;
            this.gvw3.Name = "gvw3";
            this.gvw3.OptionsBehavior.Editable = false;
            this.gvw3.OptionsView.ColumnAutoWidth = false;
            this.gvw3.OptionsView.ShowGroupPanel = false;
            //
            // colD2
            //
            //
            // tabDetail3
            //
            this.tabDetail3.Controls.Add(this.grd4);
            this.tabDetail3.Name = "tabDetail3";
            this.tabDetail3.Size = new System.Drawing.Size(822, 224);
            this.tabDetail3.Text = "tabDetail3";
            //
            // grd4
            //
            this.grd4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd4.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grd4.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grd4.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grd4.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grd4.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grd4.Location = new System.Drawing.Point(0, 0);
            this.grd4.MainView = this.gvw4;
            this.grd4.Name = "grd4";
            this.grd4.Size = new System.Drawing.Size(822, 224);
            this.grd4.TabIndex = 0;
            this.grd4.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw4});
            //
            // gvw4
            //
            this.gvw4.GridControl = this.grd4;
            this.gvw4.HighlightFocusedRow = true;
            this.gvw4.Name = "gvw4";
            this.gvw4.OptionsBehavior.Editable = false;
            this.gvw4.OptionsView.ColumnAutoWidth = false;
            this.gvw4.OptionsView.ShowGroupPanel = false;
            //
            // colD3
            //
            //
            // tabDetail4
            //
            this.tabDetail4.Controls.Add(this.grd5);
            this.tabDetail4.Name = "tabDetail4";
            this.tabDetail4.Size = new System.Drawing.Size(822, 224);
            this.tabDetail4.Text = "tabDetail4";
            //
            // grd5
            //
            this.grd5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd5.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grd5.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grd5.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grd5.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grd5.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grd5.Location = new System.Drawing.Point(0, 0);
            this.grd5.MainView = this.gvw5;
            this.grd5.Name = "grd5";
            this.grd5.Size = new System.Drawing.Size(822, 224);
            this.grd5.TabIndex = 0;
            this.grd5.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw5});
            //
            // gvw5
            //
            this.gvw5.GridControl = this.grd5;
            this.gvw5.HighlightFocusedRow = true;
            this.gvw5.Name = "gvw5";
            this.gvw5.OptionsBehavior.Editable = false;
            this.gvw5.OptionsView.ColumnAutoWidth = false;
            this.gvw5.OptionsView.ShowGroupPanel = false;
            //
            // colD4
            //
            //
            // panelWyn1
            // 
            this.panelWyn1.Controls.Add(this.sectionHeaderWyn2);
            this.panelWyn1.Controls.Add(this.panelWyn7);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(3, 218);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn1.Size = new System.Drawing.Size(820, 27);
            this.panelWyn1.TabIndex = 8;
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
            this.panelWyn5.Size = new System.Drawing.Size(820, 218);
            this.panelWyn5.TabIndex = 6;
            // 
            // panData
            // 
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(820, 191);
            this.panData.TabIndex = 8;
            //
            // lblDetailSample1
            //
        this.lblDetailTableName.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailTableName.Appearance.Options.UseFont = true;
        this.lblDetailTableName.Location = new System.Drawing.Point(16, 19);
        this.lblDetailTableName.Name = "lblDetailTableName";
        this.lblDetailTableName.Text = "TableID";
        this.txtDetailTableName.Location = new System.Drawing.Point(120, 16);
        this.txtDetailTableName.Name = "txtDetailTableName";
        this.txtDetailTableName.Size = new System.Drawing.Size(220, 20);
        this.panData.Controls.Add(this.lblDetailTableName);
        this.panData.Controls.Add(this.txtDetailTableName);
        this.lblDetailTableDesc.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailTableDesc.Appearance.Options.UseFont = true;
        this.lblDetailTableDesc.Location = new System.Drawing.Point(16, 47);
        this.lblDetailTableDesc.Name = "lblDetailTableDesc";
        this.lblDetailTableDesc.Text = "Table명";
        this.txtDetailTableDesc.Location = new System.Drawing.Point(120, 44);
        this.txtDetailTableDesc.Name = "txtDetailTableDesc";
        this.txtDetailTableDesc.Size = new System.Drawing.Size(220, 20);
        this.panData.Controls.Add(this.lblDetailTableDesc);
        this.panData.Controls.Add(this.txtDetailTableDesc);
        this.lblDetailPreFix.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailPreFix.Appearance.Options.UseFont = true;
        this.lblDetailPreFix.Location = new System.Drawing.Point(16, 75);
        this.lblDetailPreFix.Name = "lblDetailPreFix";
        this.lblDetailPreFix.Text = "채번코드";
        this.txtDetailPreFix.Location = new System.Drawing.Point(120, 72);
        this.txtDetailPreFix.Name = "txtDetailPreFix";
        this.txtDetailPreFix.Size = new System.Drawing.Size(220, 20);
        this.panData.Controls.Add(this.lblDetailPreFix);
        this.panData.Controls.Add(this.txtDetailPreFix);
        this.lblDetailKeyCol.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailKeyCol.Appearance.Options.UseFont = true;
        this.lblDetailKeyCol.Location = new System.Drawing.Point(16, 103);
        this.lblDetailKeyCol.Name = "lblDetailKeyCol";
        this.lblDetailKeyCol.Text = "채번컬럼";
        this.txtDetailKeyCol.Location = new System.Drawing.Point(120, 100);
        this.txtDetailKeyCol.Name = "txtDetailKeyCol";
        this.txtDetailKeyCol.Size = new System.Drawing.Size(220, 20);
        this.panData.Controls.Add(this.lblDetailKeyCol);
        this.panData.Controls.Add(this.txtDetailKeyCol);
        this.lblDetailDateCol.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailDateCol.Appearance.Options.UseFont = true;
        this.lblDetailDateCol.Location = new System.Drawing.Point(16, 131);
        this.lblDetailDateCol.Name = "lblDetailDateCol";
        this.lblDetailDateCol.Text = "date_col";
        this.txtDetailDateCol.Location = new System.Drawing.Point(120, 128);
        this.txtDetailDateCol.Name = "txtDetailDateCol";
        this.txtDetailDateCol.Size = new System.Drawing.Size(220, 20);
        this.panData.Controls.Add(this.lblDetailDateCol);
        this.panData.Controls.Add(this.txtDetailDateCol);
        this.lblDetailDateType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailDateType.Appearance.Options.UseFont = true;
        this.lblDetailDateType.Location = new System.Drawing.Point(16, 159);
        this.lblDetailDateType.Name = "lblDetailDateType";
        this.lblDetailDateType.Text = "날짜유형";
        this.cboDetailDateType.Location = new System.Drawing.Point(120, 156);
        this.cboDetailDateType.Name = "cboDetailDateType";
        this.cboDetailDateType.Size = new System.Drawing.Size(220, 20);
        this.cboDetailDateType.LookupKey = "L_SM0006";
        this.panData.Controls.Add(this.lblDetailDateType);
        this.panData.Controls.Add(this.cboDetailDateType);
        this.lblDetailSeqLen.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailSeqLen.Appearance.Options.UseFont = true;
        this.lblDetailSeqLen.Location = new System.Drawing.Point(16, 187);
        this.lblDetailSeqLen.Name = "lblDetailSeqLen";
        this.lblDetailSeqLen.Text = "순번자리수";
        this.numDetailSeqLen.Location = new System.Drawing.Point(120, 184);
        this.numDetailSeqLen.Name = "numDetailSeqLen";
        this.numDetailSeqLen.Size = new System.Drawing.Size(220, 20);
        this.panData.Controls.Add(this.lblDetailSeqLen);
        this.panData.Controls.Add(this.numDetailSeqLen);
        this.lblDetailRegUserId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailRegUserId.Appearance.Options.UseFont = true;
        this.lblDetailRegUserId.Location = new System.Drawing.Point(16, 215);
        this.lblDetailRegUserId.Name = "lblDetailRegUserId";
        this.lblDetailRegUserId.Text = "reg_user_id";
        this.txtDetailRegUserId.Location = new System.Drawing.Point(120, 212);
        this.txtDetailRegUserId.Name = "txtDetailRegUserId";
        this.txtDetailRegUserId.Size = new System.Drawing.Size(220, 20);
        this.panData.Controls.Add(this.lblDetailRegUserId);
        this.panData.Controls.Add(this.txtDetailRegUserId);
        this.lblDetailRegDt.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailRegDt.Appearance.Options.UseFont = true;
        this.lblDetailRegDt.Location = new System.Drawing.Point(16, 243);
        this.lblDetailRegDt.Name = "lblDetailRegDt";
        this.lblDetailRegDt.Text = "reg_dt";
        this.dteDetailRegDt.Location = new System.Drawing.Point(120, 240);
        this.dteDetailRegDt.Name = "dteDetailRegDt";
        this.dteDetailRegDt.Size = new System.Drawing.Size(220, 20);
        this.panData.Controls.Add(this.lblDetailRegDt);
        this.panData.Controls.Add(this.dteDetailRegDt);
        this.lblDetailUptUserId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailUptUserId.Appearance.Options.UseFont = true;
        this.lblDetailUptUserId.Location = new System.Drawing.Point(16, 271);
        this.lblDetailUptUserId.Name = "lblDetailUptUserId";
        this.lblDetailUptUserId.Text = "upt_user_id";
        this.txtDetailUptUserId.Location = new System.Drawing.Point(120, 268);
        this.txtDetailUptUserId.Name = "txtDetailUptUserId";
        this.txtDetailUptUserId.Size = new System.Drawing.Size(220, 20);
        this.panData.Controls.Add(this.lblDetailUptUserId);
        this.panData.Controls.Add(this.txtDetailUptUserId);
        this.lblDetailUptDt.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailUptDt.Appearance.Options.UseFont = true;
        this.lblDetailUptDt.Location = new System.Drawing.Point(16, 299);
        this.lblDetailUptDt.Name = "lblDetailUptDt";
        this.lblDetailUptDt.Text = "upt_dt";
        this.dteDetailUptDt.Location = new System.Drawing.Point(120, 296);
        this.dteDetailUptDt.Name = "dteDetailUptDt";
        this.dteDetailUptDt.Size = new System.Drawing.Size(220, 20);
        this.panData.Controls.Add(this.lblDetailUptDt);
        this.panData.Controls.Add(this.dteDetailUptDt);
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
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightFocusedRow = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsSelection.InvertSelection = true;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            //
            // colM1
            //
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
        this.colMTableDesc.Caption = "Table명";
        this.colMTableDesc.FieldName = "table_desc";
        this.colMTableDesc.Name = "colMTableDesc";
        this.colMTableDesc.Visible = true;
        this.colMTableDesc.VisibleIndex = 1;
        this.colMTableDesc.Width = 100;
        //
        // colMPreFix
        //
        this.colMPreFix.Caption = "채번코드";
        this.colMPreFix.FieldName = "pre_fix";
        this.colMPreFix.Name = "colMPreFix";
        this.colMPreFix.Visible = true;
        this.colMPreFix.VisibleIndex = 2;
        this.colMPreFix.Width = 100;
        //
        // colMKeyCol
        //
        this.colMKeyCol.Caption = "채번컬럼";
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
        this.colMDateType.Caption = "날짜유형";
        this.colMDateType.FieldName = "date_type";
        this.colMDateType.Name = "colMDateType";
        this.colMDateType.ColumnEdit = this.lookUpcolMDateType;
        this.colMDateType.Visible = true;
        this.colMDateType.VisibleIndex = 5;
        this.colMDateType.Width = 100;
        //
        // colMSeqLen
        //
        this.colMSeqLen.Caption = "순번자리수";
        this.colMSeqLen.FieldName = "seq_len";
        this.colMSeqLen.Name = "colMSeqLen";
        this.colMSeqLen.ColumnEdit = this.spinEditcolM;
        this.colMSeqLen.Visible = true;
        this.colMSeqLen.VisibleIndex = 6;
        this.colMSeqLen.Width = 100;
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
        this.colMRegDt.FieldName = "reg_dt";
        this.colMRegDt.Name = "colMRegDt";
        this.colMRegDt.ColumnEdit = this.dateEditcolM;
        this.colMRegDt.Visible = true;
        this.colMRegDt.VisibleIndex = 8;
        this.colMRegDt.Width = 100;
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
        this.colMUptDt.FieldName = "upt_dt";
        this.colMUptDt.Name = "colMUptDt";
        this.colMUptDt.ColumnEdit = this.dateEditcolM;
        this.colMUptDt.Visible = true;
        this.colMUptDt.VisibleIndex = 10;
        this.colMUptDt.Width = 100;
        //
        // spinEditcolM
        //
        this.spinEditcolM.Name = "spinEditcolM";
        //
        // dateEditcolM
        //
        this.dateEditcolM.Name = "dateEditcolM";
        //
        // lookUpcolMDateType
        //
        this.lookUpcolMDateType.LookupKey = "L_SM0006";
        this.lookUpcolMDateType.Name = "lookUpcolMDateType";
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
        this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { this.spinEditcolM, this.dateEditcolM, this.lookUpcolMDateType});
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
            // txtSearchQ
            //
        this.lblSearchTableName.Location = new System.Drawing.Point(16, 24);
        this.lblSearchTableName.Name = "lblSearchTableName";
        this.lblSearchTableName.Text = "table_name";
        this.panHeader.Controls.Add(this.lblSearchTableName);
        this.txtTableName.Location = new System.Drawing.Point(96, 20);
        this.txtTableName.Name = "txtTableName";
        this.txtTableName.Size = new System.Drawing.Size(150, 20);
        this.panHeader.Controls.Add(this.txtTableName);
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
            this.sectionHeaderWyn1.Text = "자동채번등록 [frmAutoKey]";
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
            this.tabDetail2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw3)).EndInit();
            this.tabDetail3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw4)).EndInit();
            this.tabDetail4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
            this.panelWyn7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
            this.panData.ResumeLayout(false);
            this.panData.PerformLayout();
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
    private DevExpress.XtraTab.XtraTabPage tabDetail2;
    private GridControlWyn grd3;
    private GridViewWyn gvw3;
    private DevExpress.XtraTab.XtraTabPage tabDetail3;
    private GridControlWyn grd4;
    private GridViewWyn gvw4;
    private DevExpress.XtraTab.XtraTabPage tabDetail4;
    private GridControlWyn grd5;
    private GridViewWyn gvw5;
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
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private PanelWyn paTitle;
    private SectionHeaderWyn sectionHeaderWyn2;
    private SectionHeaderWyn sectionHeaderWyn3;
    private SectionHeaderWyn sectionHeaderWyn4;
    private SectionHeaderWyn sectionHeaderWyn1;
}
