// AI Builder가 트리마스터-서브그리드 템플릿을 복제해서 자동 생성 - 2026-09-11.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
#nullable disable
using WYNLAB.Base;
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
            this.tabDetailGrids = new WYNLAB.Base.Controls.TabControlWyn();
            this.tabDetail1 = new DevExpress.XtraTab.XtraTabPage();
            this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnitNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAssetNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStockYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLotYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tabDetail2 = new DevExpress.XtraTab.XtraTabPage();
            this.grd3 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw3 = new WYNLAB.Base.Controls.GridViewWyn();
            this.tabDetail3 = new DevExpress.XtraTab.XtraTabPage();
            this.grd4 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw4 = new WYNLAB.Base.Controls.GridViewWyn();
            this.tabDetail4 = new DevExpress.XtraTab.XtraTabPage();
            this.grd5 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw5 = new WYNLAB.Base.Controls.GridViewWyn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDeletRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAddRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.lblDetailAccId = new DevExpress.XtraEditors.LabelControl();
            this.cboDetailAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.txtDetailGrpId = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailGrpNm = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailGrpNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailGrpLvl = new DevExpress.XtraEditors.LabelControl();
            this.cboDetailGrpLvl = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.cboDetailParGrpId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblDetailParGrpNm = new DevExpress.XtraEditors.LabelControl();
            this.lblDetailRemark = new DevExpress.XtraEditors.LabelControl();
            this.memDetailRemark = new WYNLAB.Base.Controls.MemoEditWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.tree1 = new WYNLAB.Base.Controls.TreeListWyn();
            this.treeColAccId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.lookUpTreeAccId = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.treeColGrpId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeColGrpNm = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeColGrpLvl = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.lookUpTreeGrpLvl = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.treeColParGrpId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeColParGrpNm = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeColRemark = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
            this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblSearchGrpNm = new DevExpress.XtraEditors.LabelControl();
            this.txtGrpNm_Q = new WYNLAB.Base.Controls.TextEditWyn();
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
            this.tabDetail2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw3)).BeginInit();
            this.tabDetail3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw4)).BeginInit();
            this.tabDetail4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailAccId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailGrpId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailGrpNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailGrpLvl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailParGrpId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memDetailRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
            this.panelWyn6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).BeginInit();
            this.panelWyn8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tree1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpTreeAccId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpTreeGrpLvl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
            this.panelWyn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtGrpNm_Q.Properties)).BeginInit();
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
            this.tabDetailGrids.Location = new System.Drawing.Point(3, 295);
            this.tabDetailGrids.Name = "tabDetailGrids";
            this.tabDetailGrids.SelectedTabPage = this.tabDetail1;
            this.tabDetailGrids.Size = new System.Drawing.Size(820, 198);
            this.tabDetailGrids.TabIndex = 7;
            this.tabDetailGrids.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            this.tabDetailGrids.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabDetail1});
            // 
            // tabDetail1
            // 
            this.tabDetail1.Controls.Add(this.grd2);
            this.tabDetail1.Name = "tabDetail1";
            this.tabDetail1.Size = new System.Drawing.Size(818, 172);
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
            this.grd2.Size = new System.Drawing.Size(818, 172);
            this.grd2.TabIndex = 0;
            this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw2});
            // 
            // gvw2
            // 
            this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colItemNo,
            this.colItemNm,
            this.colItemSpec,
            this.colUnitNm,
            this.colAssetNm,
            this.colWhNm,
            this.colStockYn,
            this.colLotYn,
            this.colStatNm});
            this.gvw2.GridControl = this.grd2;
            this.gvw2.HighlightFocusedRow = true;
            this.gvw2.Name = "gvw2";
            this.gvw2.OptionsBehavior.Editable = false;
            this.gvw2.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw2.OptionsView.ColumnAutoWidth = false;
            this.gvw2.OptionsView.ShowGroupPanel = false;
            // 
            // colItemNo
            // 
            this.colItemNo.Caption = "품목코드";
            this.colItemNo.FieldName = "item_no";
            this.colItemNo.Name = "colItemNo";
            this.colItemNo.Visible = true;
            this.colItemNo.VisibleIndex = 0;
            this.colItemNo.Width = 110;
            // 
            // colItemNm
            // 
            this.colItemNm.Caption = "품목명";
            this.colItemNm.FieldName = "item_nm";
            this.colItemNm.Name = "colItemNm";
            this.colItemNm.Visible = true;
            this.colItemNm.VisibleIndex = 1;
            this.colItemNm.Width = 170;
            // 
            // colItemSpec
            // 
            this.colItemSpec.Caption = "규격";
            this.colItemSpec.FieldName = "item_spec";
            this.colItemSpec.Name = "colItemSpec";
            this.colItemSpec.Visible = true;
            this.colItemSpec.VisibleIndex = 2;
            this.colItemSpec.Width = 150;
            // 
            // colUnitNm
            // 
            this.colUnitNm.Caption = "단위";
            this.colUnitNm.AppearanceCell.Options.UseTextOptions = true;
            this.colUnitNm.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colUnitNm.FieldName = "unit_nm";
            this.colUnitNm.Name = "colUnitNm";
            this.colUnitNm.Visible = true;
            this.colUnitNm.VisibleIndex = 3;
            this.colUnitNm.Width = 60;
            // 
            // colAssetNm
            // 
            this.colAssetNm.Caption = "자산구분";
            this.colAssetNm.FieldName = "asset_nm";
            this.colAssetNm.Name = "colAssetNm";
            this.colAssetNm.Visible = true;
            this.colAssetNm.VisibleIndex = 4;
            this.colAssetNm.Width = 90;
            // 
            // colWhNm
            // 
            this.colWhNm.Caption = "창고";
            this.colWhNm.FieldName = "wh_nm";
            this.colWhNm.Name = "colWhNm";
            this.colWhNm.Visible = true;
            this.colWhNm.VisibleIndex = 5;
            this.colWhNm.Width = 110;
            // 
            // colStockYn
            // 
            this.colStockYn.Caption = "재고관리";
            this.colStockYn.AppearanceCell.Options.UseTextOptions = true;
            this.colStockYn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colStockYn.FieldName = "stock_yn";
            this.colStockYn.Name = "colStockYn";
            this.colStockYn.Visible = true;
            this.colStockYn.VisibleIndex = 6;
            this.colStockYn.Width = 70;
            // 
            // colLotYn
            // 
            this.colLotYn.Caption = "LOT관리";
            this.colLotYn.AppearanceCell.Options.UseTextOptions = true;
            this.colLotYn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colLotYn.FieldName = "lot_yn";
            this.colLotYn.Name = "colLotYn";
            this.colLotYn.Visible = true;
            this.colLotYn.VisibleIndex = 7;
            this.colLotYn.Width = 70;
            // 
            // colStatNm
            // 
            this.colStatNm.Caption = "상태";
            this.colStatNm.AppearanceCell.Options.UseTextOptions = true;
            this.colStatNm.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colStatNm.FieldName = "stat_nm";
            this.colStatNm.Name = "colStatNm";
            this.colStatNm.Visible = true;
            this.colStatNm.VisibleIndex = 8;
            this.colStatNm.Width = 70;
            // 
            // tabDetail2
            // 
            this.tabDetail2.Controls.Add(this.grd3);
            this.tabDetail2.Name = "tabDetail2";
            this.tabDetail2.Size = new System.Drawing.Size(818, 172);
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
            this.grd3.Size = new System.Drawing.Size(818, 172);
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
            this.gvw3.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw3.OptionsView.ColumnAutoWidth = false;
            this.gvw3.OptionsView.ShowGroupPanel = false;
            // 
            // tabDetail3
            // 
            this.tabDetail3.Controls.Add(this.grd4);
            this.tabDetail3.Name = "tabDetail3";
            this.tabDetail3.Size = new System.Drawing.Size(818, 172);
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
            this.grd4.Size = new System.Drawing.Size(818, 172);
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
            this.gvw4.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw4.OptionsView.ColumnAutoWidth = false;
            this.gvw4.OptionsView.ShowGroupPanel = false;
            // 
            // tabDetail4
            // 
            this.tabDetail4.Controls.Add(this.grd5);
            this.tabDetail4.Name = "tabDetail4";
            this.tabDetail4.Size = new System.Drawing.Size(818, 172);
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
            this.grd5.Size = new System.Drawing.Size(818, 172);
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
            this.gvw5.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw5.OptionsView.ColumnAutoWidth = false;
            this.gvw5.OptionsView.ShowGroupPanel = false;
            // 
            // panelWyn7
            // 
            this.panelWyn7.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn7.Appearance.Options.UseBackColor = true;
            this.panelWyn7.Controls.Add(this.btnDeletRow2);
            this.panelWyn7.Controls.Add(this.btnAddRow2);
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(3, 265);
            this.panelWyn7.Name = "panelWyn7";
            this.panelWyn7.Padding = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.panelWyn7.Size = new System.Drawing.Size(820, 30);
            this.panelWyn7.TabIndex = 9;
            this.panelWyn7.Visible = false;
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
            this.btnDeletRow2.ToolTip = "행삭제(현재 탭)";
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
            this.btnAddRow2.ToolTip = "행추가(현재 탭)";
            // 
            // panelWyn1
            // 
            this.panelWyn1.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn1.Appearance.Options.UseBackColor = true;
            this.panelWyn1.Controls.Add(this.sectionHeaderWyn2);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(3, 238);
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
            this.sectionHeaderWyn2.Size = new System.Drawing.Size(815, 25);
            this.sectionHeaderWyn2.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn2.SvgIcon")));
            this.sectionHeaderWyn2.TabIndex = 10;
            this.sectionHeaderWyn2.Text = "소속 품목 LIST";
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
            this.panelWyn5.Size = new System.Drawing.Size(820, 238);
            this.panelWyn5.TabIndex = 6;
            // 
            // panData
            // 
            this.panData.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panData.Appearance.Options.UseBackColor = true;
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Controls.Add(this.lblDetailAccId);
            this.panData.Controls.Add(this.cboDetailAccId);
            this.panData.Controls.Add(this.txtDetailGrpId);
            this.panData.Controls.Add(this.lblDetailGrpNm);
            this.panData.Controls.Add(this.txtDetailGrpNm);
            this.panData.Controls.Add(this.lblDetailGrpLvl);
            this.panData.Controls.Add(this.cboDetailGrpLvl);
            this.panData.Controls.Add(this.cboDetailParGrpId);
            this.panData.Controls.Add(this.lblDetailParGrpNm);
            this.panData.Controls.Add(this.lblDetailRemark);
            this.panData.Controls.Add(this.memDetailRemark);
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(820, 211);
            this.panData.TabIndex = 8;
            // 
            // lblDetailAccId
            // 
            this.lblDetailAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailAccId.Appearance.Options.UseFont = true;
            this.lblDetailAccId.Location = new System.Drawing.Point(44, 19);
            this.lblDetailAccId.Name = "lblDetailAccId";
            this.lblDetailAccId.Size = new System.Drawing.Size(36, 15);
            this.lblDetailAccId.TabIndex = 0;
            this.lblDetailAccId.Text = "사업장";
            // 
            // cboDetailAccId
            // 
            this.cboDetailAccId.EditValue = "";
            this.cboDetailAccId.Location = new System.Drawing.Point(87, 16);
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
            this.cboDetailAccId.Size = new System.Drawing.Size(178, 20);
            this.cboDetailAccId.TabIndex = 1;
            // 
            // txtDetailGrpId
            // 
            this.txtDetailGrpId.Location = new System.Drawing.Point(266, 43);
            this.txtDetailGrpId.Name = "txtDetailGrpId";
            this.txtDetailGrpId.Properties.ReadOnly = true;
            this.txtDetailGrpId.Size = new System.Drawing.Size(52, 20);
            this.txtDetailGrpId.TabIndex = 3;
            // 
            // lblDetailGrpNm
            // 
            this.lblDetailGrpNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailGrpNm.Appearance.Options.UseFont = true;
            this.lblDetailGrpNm.Location = new System.Drawing.Point(44, 45);
            this.lblDetailGrpNm.Name = "lblDetailGrpNm";
            this.lblDetailGrpNm.Size = new System.Drawing.Size(36, 15);
            this.lblDetailGrpNm.TabIndex = 4;
            this.lblDetailGrpNm.Text = "그룹명";
            // 
            // txtDetailGrpNm
            // 
            this.txtDetailGrpNm.Location = new System.Drawing.Point(87, 43);
            this.txtDetailGrpNm.Name = "txtDetailGrpNm";
            this.txtDetailGrpNm.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtDetailGrpNm.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDetailGrpNm.Properties.Appearance.Options.UseBackColor = true;
            this.txtDetailGrpNm.Properties.Appearance.Options.UseForeColor = true;
            this.txtDetailGrpNm.Required = true;
            this.txtDetailGrpNm.Size = new System.Drawing.Size(178, 20);
            this.txtDetailGrpNm.TabIndex = 5;
            // 
            // lblDetailGrpLvl
            // 
            this.lblDetailGrpLvl.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailGrpLvl.Appearance.Options.UseFont = true;
            this.lblDetailGrpLvl.Location = new System.Drawing.Point(36, 73);
            this.lblDetailGrpLvl.Name = "lblDetailGrpLvl";
            this.lblDetailGrpLvl.Size = new System.Drawing.Size(44, 15);
            this.lblDetailGrpLvl.TabIndex = 6;
            this.lblDetailGrpLvl.Text = "그룹LVL";
            // 
            // cboDetailGrpLvl
            // 
            this.cboDetailGrpLvl.EditValue = "";
            this.cboDetailGrpLvl.Location = new System.Drawing.Point(87, 70);
            this.cboDetailGrpLvl.LookupKey = "L_CM0006";
            this.cboDetailGrpLvl.Name = "cboDetailGrpLvl";
            this.cboDetailGrpLvl.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.cboDetailGrpLvl.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboDetailGrpLvl.Properties.Appearance.Options.UseBackColor = true;
            this.cboDetailGrpLvl.Properties.Appearance.Options.UseForeColor = true;
            this.cboDetailGrpLvl.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailGrpLvl.Properties.NullText = "";
            this.cboDetailGrpLvl.Required = true;
            this.cboDetailGrpLvl.Size = new System.Drawing.Size(178, 20);
            this.cboDetailGrpLvl.TabIndex = 7;
            // 
            // cboDetailParGrpId
            // 
            this.cboDetailParGrpId.EditValue = "";
            this.cboDetailParGrpId.Location = new System.Drawing.Point(87, 97);
            this.cboDetailParGrpId.Name = "cboDetailParGrpId";
            this.cboDetailParGrpId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailParGrpId.Properties.NullText = "";
            this.cboDetailParGrpId.Size = new System.Drawing.Size(178, 20);
            this.cboDetailParGrpId.TabIndex = 9;
            // 
            // lblDetailParGrpNm
            // 
            this.lblDetailParGrpNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailParGrpNm.Appearance.Options.UseFont = true;
            this.lblDetailParGrpNm.Location = new System.Drawing.Point(20, 100);
            this.lblDetailParGrpNm.Name = "lblDetailParGrpNm";
            this.lblDetailParGrpNm.Size = new System.Drawing.Size(60, 15);
            this.lblDetailParGrpNm.TabIndex = 10;
            this.lblDetailParGrpNm.Text = "상위그룹명";
            // 
            // lblDetailRemark
            // 
            this.lblDetailRemark.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailRemark.Appearance.Options.UseFont = true;
            this.lblDetailRemark.Location = new System.Drawing.Point(56, 124);
            this.lblDetailRemark.Name = "lblDetailRemark";
            this.lblDetailRemark.Size = new System.Drawing.Size(24, 15);
            this.lblDetailRemark.TabIndex = 12;
            this.lblDetailRemark.Text = "비고";
            // 
            // memDetailRemark
            // 
            this.memDetailRemark.Location = new System.Drawing.Point(87, 124);
            this.memDetailRemark.Name = "memDetailRemark";
            this.memDetailRemark.Size = new System.Drawing.Size(477, 75);
            this.memDetailRemark.TabIndex = 13;
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
            this.sectionHeaderWyn3.Text = "품목그룹정보";
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
            this.panelWyn8.Controls.Add(this.tree1);
            this.panelWyn8.Controls.Add(this.panelWyn2);
            this.panelWyn8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelWyn8.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn8.Location = new System.Drawing.Point(0, 0);
            this.panelWyn8.Name = "panelWyn8";
            this.panelWyn8.Size = new System.Drawing.Size(402, 493);
            this.panelWyn8.TabIndex = 12;
            // 
            // tree1
            // 
            this.tree1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeColAccId,
            this.treeColGrpId,
            this.treeColGrpNm,
            this.treeColGrpLvl,
            this.treeColParGrpId,
            this.treeColParGrpNm,
            this.treeColRemark});
            this.tree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree1.Location = new System.Drawing.Point(0, 27);
            this.tree1.Name = "tree1";
            this.tree1.OptionsBehavior.Editable = false;
            this.tree1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lookUpTreeAccId,
            this.lookUpTreeGrpLvl});
            this.tree1.RowHeight = 26;
            this.tree1.Size = new System.Drawing.Size(402, 466);
            this.tree1.TabIndex = 10;
            // 
            // treeColAccId
            // 
            this.treeColAccId.Caption = "사업장";
            this.treeColAccId.ColumnEdit = this.lookUpTreeAccId;
            this.treeColAccId.FieldName = "acc_id";
            this.treeColAccId.Name = "treeColAccId";
            this.treeColAccId.Width = 150;
            // 
            // lookUpTreeAccId
            // 
            this.lookUpTreeAccId.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpTreeAccId.LookupKey = "L_ACC";
            this.lookUpTreeAccId.Name = "lookUpTreeAccId";
            this.lookUpTreeAccId.NullText = "";
            this.lookUpTreeAccId.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // treeColGrpId
            // 
            this.treeColGrpId.Caption = "그룹ID";
            this.treeColGrpId.FieldName = "grp_id";
            this.treeColGrpId.Name = "treeColGrpId";
            this.treeColGrpId.Visible = true;
            this.treeColGrpId.VisibleIndex = 1;
            this.treeColGrpId.Width = 81;
            // 
            // treeColGrpNm
            // 
            this.treeColGrpNm.Caption = "그룹명";
            this.treeColGrpNm.FieldName = "grp_nm";
            this.treeColGrpNm.Name = "treeColGrpNm";
            this.treeColGrpNm.Visible = true;
            this.treeColGrpNm.VisibleIndex = 0;
            this.treeColGrpNm.Width = 296;
            // 
            // treeColGrpLvl
            // 
            this.treeColGrpLvl.Caption = "그룹LVL";
            this.treeColGrpLvl.ColumnEdit = this.lookUpTreeGrpLvl;
            this.treeColGrpLvl.FieldName = "grp_lvl";
            this.treeColGrpLvl.Name = "treeColGrpLvl";
            this.treeColGrpLvl.Width = 150;
            // 
            // lookUpTreeGrpLvl
            // 
            this.lookUpTreeGrpLvl.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpTreeGrpLvl.LookupKey = "L_CM0006";
            this.lookUpTreeGrpLvl.Name = "lookUpTreeGrpLvl";
            this.lookUpTreeGrpLvl.NullText = "";
            this.lookUpTreeGrpLvl.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // treeColParGrpId
            // 
            this.treeColParGrpId.Caption = "상위그룹ID";
            this.treeColParGrpId.FieldName = "par_grp_id";
            this.treeColParGrpId.Name = "treeColParGrpId";
            this.treeColParGrpId.Width = 150;
            // 
            // treeColParGrpNm
            // 
            this.treeColParGrpNm.Caption = "상위그룹명";
            this.treeColParGrpNm.FieldName = "par_grp_nm";
            this.treeColParGrpNm.Name = "treeColParGrpNm";
            this.treeColParGrpNm.Width = 150;
            // 
            // treeColRemark
            // 
            this.treeColRemark.Caption = "비고";
            this.treeColRemark.FieldName = "remark";
            this.treeColRemark.Name = "treeColRemark";
            this.treeColRemark.Width = 150;
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
            this.sectionHeaderWyn4.Text = "품목그룹List";
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.lblSearchAccId);
            this.panHeader.Controls.Add(this.cboSearchAccId);
            this.panHeader.Controls.Add(this.lblSearchGrpNm);
            this.panHeader.Controls.Add(this.txtGrpNm_Q);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 33);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1235, 49);
            this.panHeader.TabIndex = 8;
            // 
            // lblSearchAccId (조회조건 첫 번째 - 사업장 표준, 2026-10-03)
            // 
            this.lblSearchAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSearchAccId.Appearance.Options.UseFont = true;
            this.lblSearchAccId.Location = new System.Drawing.Point(17, 19);
            this.lblSearchAccId.Name = "lblSearchAccId";
            this.lblSearchAccId.Size = new System.Drawing.Size(36, 15);
            this.lblSearchAccId.TabIndex = 0;
            this.lblSearchAccId.Text = "사업장";
            // 
            // cboSearchAccId (사업장 LookUp, Required - 기본값은 화면 생성자에서 로그인 사업장)
            // 
            this.cboSearchAccId.EditValue = "";
            this.cboSearchAccId.Location = new System.Drawing.Point(61, 16);
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
            // lblSearchGrpNm
            // 
            this.lblSearchGrpNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchGrpNm.Appearance.Options.UseFont = true;
            this.lblSearchGrpNm.Location = new System.Drawing.Point(216, 19);
            this.lblSearchGrpNm.Name = "lblSearchGrpNm";
            this.lblSearchGrpNm.Size = new System.Drawing.Size(60, 15);
            this.lblSearchGrpNm.TabIndex = 3;
            this.lblSearchGrpNm.Text = "품목그룹명";
            // 
            // txtGrpNm_Q
            // 
            this.txtGrpNm_Q.Location = new System.Drawing.Point(285, 16);
            this.txtGrpNm_Q.Name = "txtGrpNm_Q";
            this.txtGrpNm_Q.Size = new System.Drawing.Size(150, 20);
            this.txtGrpNm_Q.TabIndex = 4;
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
            this.paTitle.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.paTitle.Size = new System.Drawing.Size(1235, 33);
            this.paTitle.TabIndex = 5;
            // 
            // sectionHeaderWyn1
            // 
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1230, 33);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 11;
            this.sectionHeaderWyn1.Text = "품목그룹등록 [frmItemGrp]";
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
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailGrpId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailGrpNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailGrpLvl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailParGrpId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memDetailRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
            this.panelWyn6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).EndInit();
            this.panelWyn8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tree1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpTreeAccId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpTreeGrpLvl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
            this.panelWyn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtGrpNm_Q.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
            this.paTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
            this.ResumeLayout(false);

    }

    private PanelWyn panBase;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn4;
    private TabControlWyn tabDetailGrids;
    private DevExpress.XtraTab.XtraTabPage tabDetail1;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitNm;
    private DevExpress.XtraGrid.Columns.GridColumn colAssetNm;
    private DevExpress.XtraGrid.Columns.GridColumn colWhNm;
    private DevExpress.XtraGrid.Columns.GridColumn colStockYn;
    private DevExpress.XtraGrid.Columns.GridColumn colLotYn;
    private DevExpress.XtraGrid.Columns.GridColumn colStatNm;
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
    private DevExpress.XtraEditors.LabelControl lblDetailAccId;
    private LookUpEditWyn cboDetailAccId;
    private TextEditWyn txtDetailGrpId;
    private DevExpress.XtraEditors.LabelControl lblDetailGrpNm;
    private TextEditWyn txtDetailGrpNm;
    private DevExpress.XtraEditors.LabelControl lblDetailGrpLvl;
    private LookUpEditWyn cboDetailGrpLvl;
    private LookUpEditWyn cboDetailParGrpId;
    private DevExpress.XtraEditors.LabelControl lblDetailParGrpNm;
    private DevExpress.XtraEditors.LabelControl lblDetailRemark;
    private MemoEditWyn memDetailRemark;
    private PanelWyn panelWyn6;
    private SplitterWyn splitterWyn1;
    private PanelWyn panelWyn8;
    private TreeListWyn tree1;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeColAccId;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeColGrpId;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeColGrpNm;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeColGrpLvl;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeColParGrpId;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeColParGrpNm;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeColRemark;
    private LookUpColumnEdit lookUpTreeAccId;
    private LookUpColumnEdit lookUpTreeGrpLvl;
    private PanelWyn panelWyn2;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchGrpNm;
    private TextEditWyn txtGrpNm_Q;
    private PanelWyn paTitle;
    private SectionHeaderWyn sectionHeaderWyn2;
    private SectionHeaderWyn sectionHeaderWyn3;
    private SectionHeaderWyn sectionHeaderWyn4;
    private SectionHeaderWyn sectionHeaderWyn1;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private WYNLAB.Base.Controls.LookUpEditWyn cboSearchAccId;
}
