// @AI_BUILDER:BEGIN FILE_HEADER
// AI Builder 마스터-상세폼(단일 시트) 템플릿 원본 - grd1(마스터 목록 그리드)이 없다. 화면 자체가
// 문서 하나를 곧바로 편집하는 폼(panData)이고, 하위 그리드(grd2/grd3/grd4/grd5, 탭으로 묶임)는
// 전부 선택사항인 그 문서의 명세다(2026-09-09 - "grd1이 없는 모습", panHeader에 키를 입력하고
// 툴바 조회 버튼을 누르면 그 문서 1건이 panData+하위그리드에 곧바로 채워짐). VS 디자이너로 열어
// 제목영역/여백/색상을 다듬으면 AI Builder가 만드는 모든 마스터-상세폼(단일시트) 화면에 그대로
// 반영된다.
// @AI_BUILDER:END FILE_HEADER
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.TEMPLATE;

public partial class TplMasterOneSheet
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TplMasterOneSheet));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.tabDetailGrids = new WYNLAB.Base.Controls.TabControlWyn();
            this.tabDetail1 = new DevExpress.XtraTab.XtraTabPage();
            this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
            // @AI_BUILDER:BEGIN DETAIL_COLUMN_NEW
            this.colD1 = new DevExpress.XtraGrid.Columns.GridColumn();
            // @AI_BUILDER:END DETAIL_COLUMN_NEW
            this.tabDetail2 = new DevExpress.XtraTab.XtraTabPage();
            this.grd3 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw3 = new WYNLAB.Base.Controls.GridViewWyn();
            // @AI_BUILDER:BEGIN DETAIL2_COLUMN_NEW
            this.colD2 = new DevExpress.XtraGrid.Columns.GridColumn();
            // @AI_BUILDER:END DETAIL2_COLUMN_NEW
            this.tabDetail3 = new DevExpress.XtraTab.XtraTabPage();
            this.grd4 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw4 = new WYNLAB.Base.Controls.GridViewWyn();
            // @AI_BUILDER:BEGIN DETAIL3_COLUMN_NEW
            this.colD3 = new DevExpress.XtraGrid.Columns.GridColumn();
            // @AI_BUILDER:END DETAIL3_COLUMN_NEW
            this.tabDetail4 = new DevExpress.XtraTab.XtraTabPage();
            this.grd5 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw5 = new WYNLAB.Base.Controls.GridViewWyn();
            // @AI_BUILDER:BEGIN DETAIL4_COLUMN_NEW
            this.colD4 = new DevExpress.XtraGrid.Columns.GridColumn();
            // @AI_BUILDER:END DETAIL4_COLUMN_NEW
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDeletRow1 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAddRow1 = new WYNLAB.Base.Controls.ButtonWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            // @AI_BUILDER:BEGIN DETAIL_FORM_NEW
            this.lblDetailSample1 = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailSample1 = new WYNLAB.Base.Controls.TextEditWyn();
            // @AI_BUILDER:END DETAIL_FORM_NEW
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            // @AI_BUILDER:BEGIN SEARCH_FIELD_NEW
            this.txtSearchQ = new WYNLAB.Base.Controls.TextEditWyn();
            // @AI_BUILDER:END SEARCH_FIELD_NEW
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWynH = new WYNLAB.Base.Controls.SectionHeaderWyn();
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
            this.panelWyn4.Controls.Add(this.panelWyn7);
            this.panelWyn4.Controls.Add(this.panelWyn1);
            this.panelWyn4.Controls.Add(this.panelWyn5);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(0, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.panelWyn4.Size = new System.Drawing.Size(1235, 493);
            this.panelWyn4.TabIndex = 7;
            //
            // tabDetailGrids
            //
            this.tabDetailGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabDetailGrids.Location = new System.Drawing.Point(3, 245);
            this.tabDetailGrids.Name = "tabDetailGrids";
            this.tabDetailGrids.SelectedTabPage = this.tabDetail1;
            this.tabDetailGrids.Size = new System.Drawing.Size(1229, 248);
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
            this.tabDetail1.Size = new System.Drawing.Size(1227, 222);
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
            this.grd2.Size = new System.Drawing.Size(1227, 222);
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
            // @AI_BUILDER:BEGIN DETAIL_COLUMN_CONFIG
            this.colD1.Caption = "예시컬럼";
            this.colD1.FieldName = "sample1";
            this.colD1.Name = "colD1";
            this.colD1.Visible = true;
            this.colD1.VisibleIndex = 0;
            this.colD1.Width = 100;
            this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colD1});
            // @AI_BUILDER:END DETAIL_COLUMN_CONFIG
            //
            // tabDetail2
            //
            this.tabDetail2.Controls.Add(this.grd3);
            this.tabDetail2.Name = "tabDetail2";
            this.tabDetail2.Size = new System.Drawing.Size(1227, 224);
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
            this.grd3.Size = new System.Drawing.Size(1227, 224);
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
            // @AI_BUILDER:BEGIN DETAIL2_COLUMN_CONFIG
            this.colD2.Caption = "예시컬럼";
            this.colD2.FieldName = "sample1";
            this.colD2.Name = "colD2";
            this.colD2.Visible = true;
            this.colD2.VisibleIndex = 0;
            this.colD2.Width = 100;
            this.gvw3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colD2});
            // @AI_BUILDER:END DETAIL2_COLUMN_CONFIG
            //
            // tabDetail3
            //
            this.tabDetail3.Controls.Add(this.grd4);
            this.tabDetail3.Name = "tabDetail3";
            this.tabDetail3.Size = new System.Drawing.Size(1227, 224);
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
            this.grd4.Size = new System.Drawing.Size(1227, 224);
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
            // @AI_BUILDER:BEGIN DETAIL3_COLUMN_CONFIG
            this.colD3.Caption = "예시컬럼";
            this.colD3.FieldName = "sample1";
            this.colD3.Name = "colD3";
            this.colD3.Visible = true;
            this.colD3.VisibleIndex = 0;
            this.colD3.Width = 100;
            this.gvw4.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colD3});
            // @AI_BUILDER:END DETAIL3_COLUMN_CONFIG
            //
            // tabDetail4
            //
            this.tabDetail4.Controls.Add(this.grd5);
            this.tabDetail4.Name = "tabDetail4";
            this.tabDetail4.Size = new System.Drawing.Size(1227, 224);
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
            this.grd5.Size = new System.Drawing.Size(1227, 224);
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
            // @AI_BUILDER:BEGIN DETAIL4_COLUMN_CONFIG
            this.colD4.Caption = "예시컬럼";
            this.colD4.FieldName = "sample1";
            this.colD4.Name = "colD4";
            this.colD4.Visible = true;
            this.colD4.VisibleIndex = 0;
            this.colD4.Width = 100;
            this.gvw5.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colD4});
            // @AI_BUILDER:END DETAIL4_COLUMN_CONFIG
            //
            // panelWyn1
            //
            this.panelWyn1.Controls.Add(this.sectionHeaderWyn2);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(3, 218);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn1.Size = new System.Drawing.Size(1229, 27);
            this.panelWyn1.TabIndex = 8;
            //
            // panelWyn7
            //
            this.panelWyn7.Controls.Add(this.btnDeletRow1);
            this.panelWyn7.Controls.Add(this.btnAddRow1);
            this.panelWyn7.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn7.Appearance.Options.UseBackColor = true;
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(1161, 0);
            this.panelWyn7.Name = "panelWyn7";
            this.panelWyn7.Padding = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.panelWyn7.Size = new System.Drawing.Size(68, 30);
            this.panelWyn7.TabIndex = 9;
            //
            // btnDeletRow1
            //
            this.btnDeletRow1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeletRow1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(198)))), ((int)(((byte)(193)))));
            this.btnDeletRow1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeletRow1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(236)))), ((int)(((byte)(234)))));
            this.btnDeletRow1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDeletRow1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDeletRow1.Image = null;
            this.btnDeletRow1.Location = new System.Drawing.Point(68, 4);
            this.btnDeletRow1.Name = "btnDeletRow1";
            this.btnDeletRow1.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDeletRow1.Size = new System.Drawing.Size(58, 22);
            this.btnDeletRow1.Text = "행삭제";
            this.btnDeletRow1.TabIndex = 0;
            this.btnDeletRow1.ToolTip = "행삭제(현재 탭)";
            //
            // btnAddRow1
            //
            this.btnAddRow1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddRow1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(231)))), ((int)(((byte)(203)))));
            this.btnAddRow1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddRow1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(245)))), ((int)(((byte)(231)))));
            this.btnAddRow1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnAddRow1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnAddRow1.Image = null;
            this.btnAddRow1.Location = new System.Drawing.Point(6, 4);
            this.btnAddRow1.Name = "btnAddRow1";
            this.btnAddRow1.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnAddRow1.Size = new System.Drawing.Size(58, 22);
            this.btnAddRow1.Text = "행추가";
            this.btnAddRow1.TabIndex = 0;
            this.btnAddRow1.ToolTip = "행추가(현재 탭)";
            //
            // sectionHeaderWyn2
            //
            this.sectionHeaderWyn2.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn2.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn2.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn2.Name = "sectionHeaderWyn2";
            this.sectionHeaderWyn2.Size = new System.Drawing.Size(1156, 25);
            this.sectionHeaderWyn2.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn2.SvgIcon")));
            this.sectionHeaderWyn2.TabIndex = 10;
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
            this.panelWyn5.Size = new System.Drawing.Size(1229, 218);
            this.panelWyn5.TabIndex = 6;
            //
            // panData
            //
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(1229, 191);
            this.panData.TabIndex = 8;
            //
            // lblDetailSample1
            //
            // @AI_BUILDER:BEGIN DETAIL_FORM_CONFIG
            this.lblDetailSample1.Location = new System.Drawing.Point(16, 19);
            this.lblDetailSample1.Name = "lblDetailSample1";
            this.lblDetailSample1.Size = new System.Drawing.Size(40, 14);
            this.lblDetailSample1.TabIndex = 0;
            this.lblDetailSample1.Text = "예시필드";
            //
            // txtDetailSample1
            //
            this.txtDetailSample1.Location = new System.Drawing.Point(120, 16);
            this.txtDetailSample1.Name = "txtDetailSample1";
            this.txtDetailSample1.Size = new System.Drawing.Size(220, 20);
            this.txtDetailSample1.TabIndex = 1;
            this.panData.Controls.Add(this.lblDetailSample1);
            this.panData.Controls.Add(this.txtDetailSample1);
            // @AI_BUILDER:END DETAIL_FORM_CONFIG
            //
            // panelWyn6
            //
            this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
            this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn6.Location = new System.Drawing.Point(0, 0);
            this.panelWyn6.Name = "panelWyn6";
            this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn6.Size = new System.Drawing.Size(1229, 27);
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
            this.sectionHeaderWyn3.Size = new System.Drawing.Size(1224, 25);
            this.sectionHeaderWyn3.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn3.SvgIcon")));
            this.sectionHeaderWyn3.TabIndex = 9;
            this.sectionHeaderWyn3.Text = "상세 등록";
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
            // @AI_BUILDER:BEGIN SEARCH_FIELD_CONFIG
            this.txtSearchQ.Location = new System.Drawing.Point(79, 15);
            this.txtSearchQ.Name = "txtSearchQ";
            this.txtSearchQ.Size = new System.Drawing.Size(265, 20);
            this.txtSearchQ.TabIndex = 1;
            this.panHeader.Controls.Add(this.txtSearchQ);
            // @AI_BUILDER:END SEARCH_FIELD_CONFIG
            //
            // paTitle
            //
            this.paTitle.Controls.Add(this.sectionHeaderWynH);
            this.paTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle.Location = new System.Drawing.Point(5, 0);
            this.paTitle.Name = "paTitle";
            this.paTitle.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.paTitle.Size = new System.Drawing.Size(1235, 25);
            this.paTitle.TabIndex = 5;
            //
            // sectionHeaderWynH
            //
            this.sectionHeaderWynH.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWynH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWynH.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWynH.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWynH.Location = new System.Drawing.Point(0, 0);
            this.sectionHeaderWynH.Name = "sectionHeaderWynH";
            this.sectionHeaderWynH.Size = new System.Drawing.Size(1235, 25);
            this.sectionHeaderWynH.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWynH.SvgIcon")));
            this.sectionHeaderWynH.TabIndex = 9;
            this.sectionHeaderWynH.Text = "__MENU_CAPTION__ [TplMasterOneSheet]";
            //
            // TplMasterOneSheet
            //
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1245, 580);
            this.Controls.Add(this.panBase);
            this.Name = "TplMasterOneSheet";
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
    // @AI_BUILDER:BEGIN DETAIL_COLUMN_DECL
    private DevExpress.XtraGrid.Columns.GridColumn colD1;
    // @AI_BUILDER:END DETAIL_COLUMN_DECL
    private DevExpress.XtraTab.XtraTabPage tabDetail2;
    private GridControlWyn grd3;
    private GridViewWyn gvw3;
    // @AI_BUILDER:BEGIN DETAIL2_COLUMN_DECL
    private DevExpress.XtraGrid.Columns.GridColumn colD2;
    // @AI_BUILDER:END DETAIL2_COLUMN_DECL
    private DevExpress.XtraTab.XtraTabPage tabDetail3;
    private GridControlWyn grd4;
    private GridViewWyn gvw4;
    // @AI_BUILDER:BEGIN DETAIL3_COLUMN_DECL
    private DevExpress.XtraGrid.Columns.GridColumn colD3;
    // @AI_BUILDER:END DETAIL3_COLUMN_DECL
    private DevExpress.XtraTab.XtraTabPage tabDetail4;
    private GridControlWyn grd5;
    private GridViewWyn gvw5;
    // @AI_BUILDER:BEGIN DETAIL4_COLUMN_DECL
    private DevExpress.XtraGrid.Columns.GridColumn colD4;
    // @AI_BUILDER:END DETAIL4_COLUMN_DECL
    private PanelWyn panelWyn1;
    private PanelWyn panelWyn7;
    private ButtonWyn btnDeletRow1;
    private ButtonWyn btnAddRow1;
    private SectionHeaderWyn sectionHeaderWyn2;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    // @AI_BUILDER:BEGIN DETAIL_FORM_DECL
    private DevExpress.XtraEditors.LabelControl lblDetailSample1;
    private TextEditWyn txtDetailSample1;
    // @AI_BUILDER:END DETAIL_FORM_DECL
    private PanelWyn panelWyn6;
    private SectionHeaderWyn sectionHeaderWyn3;
    private PanelWyn panHeader;
    // @AI_BUILDER:BEGIN SEARCH_FIELD_DECL
    private TextEditWyn txtSearchQ;
    // @AI_BUILDER:END SEARCH_FIELD_DECL
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private PanelWyn paTitle;
    private SectionHeaderWyn sectionHeaderWynH;
}
