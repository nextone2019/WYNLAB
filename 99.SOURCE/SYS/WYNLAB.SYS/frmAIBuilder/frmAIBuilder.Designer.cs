// VS 디자이너가 자동 생성하는 필드 선언에는 = null!을 안 붙여서 nullable 경고(CS8618)가
// 계속 나기 때문에, 이 파일(디자이너 전용)만 nullable 검사를 끈다 - 흔한 관례(frmMenuAuth와 동일).
#nullable disable
using WYNLAB.Base.Controls;

namespace WYNLAB.SYS;

/// <summary>
/// 화면개발 자동화(화면생성기) 도구 - 상단 입력폼(모듈/화면명/템플릿/프로시저)과 하단 미리보기
/// 탭(마스터컬럼/서브컬럼/저장파라미터)으로 구성. 실제 조회/생성 로직은 frmAIBuilder.cs,
/// 컬럼→코드 조립은 ScreenTemplateGenerator.cs 참고.
/// </summary>
public partial class frmAIBuilder
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAIBuilder));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelPreview = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.tabPreview = new WYNLAB.Base.Controls.TabControlWyn();
            this.tabSaveParams = new DevExpress.XtraTab.XtraTabPage();
            this.grdSaveParams = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvwSaveParams = new WYNLAB.Base.Controls.GridViewWyn();
            this.colSpName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSpSqlType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSpMatchedColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tabQueryMulti = new DevExpress.XtraTab.XtraTabPage();
            this.grdColumnPreview2 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvwColumnPreview2 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colCp2Name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCp2SqlType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCp2Caption = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCp2Include = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkEditPreview = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colCp2IsKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCp2ControlKind = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpControlKind = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colCp2LookupKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCp2Required = new DevExpress.XtraGrid.Columns.GridColumn();
            this.splitterWyn4 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn9 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.grdQueryResultSets = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvwQueryResultSets = new WYNLAB.Base.Controls.GridViewWyn();
            this.colQrProc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQrWorkType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQrIndex = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQrTargetSlot = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panQueryToolbar = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDescribeQueryMulti = new WYNLAB.Base.Controls.ButtonWyn();
            this.splitterWyn3 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.panQueryPlan = new WYNLAB.Base.Controls.PanelWyn();
            this.grdQueryProcPlan = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvwQueryProcPlan = new WYNLAB.Base.Controls.GridViewWyn();
            this.colQppProc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDescribeQueryAll = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnDeleteRow1 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnRowAdd1 = new WYNLAB.Base.Controls.ButtonWyn();
            this.tabSaveMulti = new DevExpress.XtraTab.XtraTabPage();
            this.grdSaveActionParams = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvwSaveActionParams = new WYNLAB.Base.Controls.GridViewWyn();
            this.colSapName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSapSqlType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSapMatchedColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn10 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn11 = new WYNLAB.Base.Controls.PanelWyn();
            this.grdSaveActions = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvwSaveActions = new WYNLAB.Base.Controls.GridViewWyn();
            this.colSaProc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSaScope = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboEditScope = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.colSaSourceSlot = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSaKeyParam = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panSaveToolbar = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDescribeSaveMulti = new WYNLAB.Base.Controls.ButtonWyn();
            this.splitterWyn2 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panSavePlan = new WYNLAB.Base.Controls.PanelWyn();
            this.grdSaveProcPlan = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvwSaveProcPlan = new WYNLAB.Base.Controls.GridViewWyn();
            this.colSppProc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDeleteRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnRowAdd2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnDescribeSaveAll = new WYNLAB.Base.Controls.ButtonWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelForm = new WYNLAB.Base.Controls.PanelWyn();
            this.cboModule = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblModule = new DevExpress.XtraEditors.LabelControl();
            this.lblScreenClassNm = new DevExpress.XtraEditors.LabelControl();
            this.txtScreenClassNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblMenuCaption = new DevExpress.XtraEditors.LabelControl();
            this.txtMenuCaption = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblUpperMenuCd = new DevExpress.XtraEditors.LabelControl();
            this.txtUpperMenuCd = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtUpperMenuNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.lblProcPrefix = new DevExpress.XtraEditors.LabelControl();
            this.txtProcPrefix = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailKeyParam = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailKeyParam = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblMasterKeyColumn = new DevExpress.XtraEditors.LabelControl();
            this.txtMasterKeyColumn = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblMasterParentColumn = new DevExpress.XtraEditors.LabelControl();
            this.txtMasterParentColumn = new WYNLAB.Base.Controls.TextEditWyn();
            this.panelWyn12 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnGenerate = new WYNLAB.Base.Controls.ButtonWyn();
            this.lblTemplateKind = new DevExpress.XtraEditors.LabelControl();
            this.cboTemplateKind = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelPreview)).BeginInit();
            this.panelPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabPreview)).BeginInit();
            this.tabPreview.SuspendLayout();
            this.tabSaveParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSaveParams)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSaveParams)).BeginInit();
            this.tabQueryMulti.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdColumnPreview2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwColumnPreview2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEditPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpControlKind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn9)).BeginInit();
            this.panelWyn9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdQueryResultSets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwQueryResultSets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panQueryToolbar)).BeginInit();
            this.panQueryToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).BeginInit();
            this.panelWyn8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panQueryPlan)).BeginInit();
            this.panQueryPlan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdQueryProcPlan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwQueryProcPlan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            this.tabSaveMulti.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSaveActionParams)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSaveActionParams)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn10)).BeginInit();
            this.panelWyn10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn11)).BeginInit();
            this.panelWyn11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSaveActions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSaveActions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboEditScope)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panSaveToolbar)).BeginInit();
            this.panSaveToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panSavePlan)).BeginInit();
            this.panSavePlan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSaveProcPlan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSaveProcPlan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
            this.panelWyn4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
            this.panelWyn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelForm)).BeginInit();
            this.panelForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboModule.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtScreenClassNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMenuCaption.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpperMenuCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpperMenuNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProcPrefix.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailKeyParam.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMasterKeyColumn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMasterParentColumn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn12)).BeginInit();
            this.panelWyn12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboTemplateKind.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).BeginInit();
            this.paTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // panBase
            // 
            this.panBase.Controls.Add(this.panelPreview);
            this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBase.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panBase.Location = new System.Drawing.Point(0, 33);
            this.panBase.Name = "panBase";
            this.panBase.Size = new System.Drawing.Size(1570, 705);
            this.panBase.TabIndex = 0;
            // 
            // panelPreview
            // 
            this.panelPreview.Controls.Add(this.panelWyn1);
            this.panelPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPreview.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelPreview.Location = new System.Drawing.Point(0, 0);
            this.panelPreview.Name = "panelPreview";
            this.panelPreview.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.panelPreview.Size = new System.Drawing.Size(1570, 705);
            this.panelPreview.TabIndex = 1;
            // 
            // panelWyn1
            // 
            this.panelWyn1.Controls.Add(this.tabPreview);
            this.panelWyn1.Controls.Add(this.panelWyn3);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(5, 0);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.panelWyn1.Size = new System.Drawing.Size(1560, 705);
            this.panelWyn1.TabIndex = 1;
            // 
            // tabPreview
            // 
            this.tabPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPreview.Location = new System.Drawing.Point(0, 137);
            this.tabPreview.Name = "tabPreview";
            this.tabPreview.SelectedTabPage = this.tabSaveParams;
            this.tabPreview.Size = new System.Drawing.Size(1560, 568);
            this.tabPreview.TabIndex = 0;
            this.tabPreview.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabSaveParams,
            this.tabQueryMulti,
            this.tabSaveMulti});
            // 
            // tabSaveParams
            // 
            this.tabSaveParams.Controls.Add(this.grdSaveParams);
            this.tabSaveParams.Name = "tabSaveParams";
            this.tabSaveParams.Size = new System.Drawing.Size(1558, 542);
            this.tabSaveParams.Text = "Save Params";
            // 
            // grdSaveParams
            // 
            this.grdSaveParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSaveParams.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdSaveParams.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdSaveParams.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdSaveParams.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdSaveParams.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdSaveParams.Location = new System.Drawing.Point(0, 0);
            this.grdSaveParams.MainView = this.gvwSaveParams;
            this.grdSaveParams.Name = "grdSaveParams";
            this.grdSaveParams.Size = new System.Drawing.Size(1558, 542);
            this.grdSaveParams.TabIndex = 0;
            this.grdSaveParams.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvwSaveParams});
            // 
            // gvwSaveParams
            // 
            this.gvwSaveParams.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colSpName,
            this.colSpSqlType,
            this.colSpMatchedColumn});
            this.gvwSaveParams.GridControl = this.grdSaveParams;
            this.gvwSaveParams.Name = "gvwSaveParams";
            this.gvwSaveParams.OptionsBehavior.Editable = false;
            this.gvwSaveParams.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvwSaveParams.OptionsView.ColumnAutoWidth = false;
            this.gvwSaveParams.OptionsView.ShowGroupPanel = false;
            // 
            // colSpName
            // 
            this.colSpName.Caption = "Param";
            this.colSpName.FieldName = "Name";
            this.colSpName.Name = "colSpName";
            this.colSpName.OptionsColumn.AllowEdit = false;
            this.colSpName.Visible = true;
            this.colSpName.VisibleIndex = 0;
            this.colSpName.Width = 125;
            // 
            // colSpSqlType
            // 
            this.colSpSqlType.Caption = "Data Type";
            this.colSpSqlType.FieldName = "SqlType";
            this.colSpSqlType.Name = "colSpSqlType";
            this.colSpSqlType.OptionsColumn.AllowEdit = false;
            this.colSpSqlType.Visible = true;
            this.colSpSqlType.VisibleIndex = 1;
            this.colSpSqlType.Width = 142;
            // 
            // colSpMatchedColumn
            // 
            this.colSpMatchedColumn.Caption = "Matched Column (blank = TODO)";
            this.colSpMatchedColumn.FieldName = "MatchedColumn";
            this.colSpMatchedColumn.Name = "colSpMatchedColumn";
            this.colSpMatchedColumn.Visible = true;
            this.colSpMatchedColumn.VisibleIndex = 2;
            this.colSpMatchedColumn.Width = 220;
            // 
            // tabQueryMulti
            // 
            this.tabQueryMulti.Controls.Add(this.grdColumnPreview2);
            this.tabQueryMulti.Controls.Add(this.splitterWyn4);
            this.tabQueryMulti.Controls.Add(this.panelWyn9);
            this.tabQueryMulti.Name = "tabQueryMulti";
            this.tabQueryMulti.Size = new System.Drawing.Size(1558, 542);
            this.tabQueryMulti.Text = "Query Sources (다중)";
            // 
            // grdColumnPreview2
            // 
            this.grdColumnPreview2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdColumnPreview2.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdColumnPreview2.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdColumnPreview2.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdColumnPreview2.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdColumnPreview2.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdColumnPreview2.Location = new System.Drawing.Point(0, 206);
            this.grdColumnPreview2.MainView = this.gvwColumnPreview2;
            this.grdColumnPreview2.Name = "grdColumnPreview2";
            this.grdColumnPreview2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.chkEditPreview,
            this.lookUpControlKind});
            this.grdColumnPreview2.Size = new System.Drawing.Size(1558, 336);
            this.grdColumnPreview2.TabIndex = 2;
            this.grdColumnPreview2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvwColumnPreview2});
            // 
            // gvwColumnPreview2
            // 
            this.gvwColumnPreview2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCp2Name,
            this.colCp2SqlType,
            this.colCp2Caption,
            this.colCp2Include,
            this.colCp2IsKey,
            this.colCp2ControlKind,
            this.colCp2LookupKey,
            this.colCp2Required});
            this.gvwColumnPreview2.GridControl = this.grdColumnPreview2;
            this.gvwColumnPreview2.Name = "gvwColumnPreview2";
            this.gvwColumnPreview2.OptionsBehavior.Editable = false;
            this.gvwColumnPreview2.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            // 
            // colCp2Name
            // 
            this.colCp2Name.Caption = "Column";
            this.colCp2Name.FieldName = "Name";
            this.colCp2Name.Name = "colCp2Name";
            this.colCp2Name.OptionsColumn.AllowEdit = false;
            this.colCp2Name.Visible = true;
            this.colCp2Name.VisibleIndex = 0;
            this.colCp2Name.Width = 150;
            // 
            // colCp2SqlType
            // 
            this.colCp2SqlType.Caption = "Data Type";
            this.colCp2SqlType.FieldName = "SqlType";
            this.colCp2SqlType.Name = "colCp2SqlType";
            this.colCp2SqlType.OptionsColumn.AllowEdit = false;
            this.colCp2SqlType.Visible = true;
            this.colCp2SqlType.VisibleIndex = 1;
            this.colCp2SqlType.Width = 90;
            // 
            // colCp2Caption
            // 
            this.colCp2Caption.Caption = "Caption";
            this.colCp2Caption.FieldName = "Caption";
            this.colCp2Caption.Name = "colCp2Caption";
            this.colCp2Caption.Visible = true;
            this.colCp2Caption.VisibleIndex = 2;
            this.colCp2Caption.Width = 150;
            // 
            // colCp2Include
            // 
            this.colCp2Include.Caption = "In Grid";
            this.colCp2Include.ColumnEdit = this.chkEditPreview;
            this.colCp2Include.FieldName = "IncludeInGrid";
            this.colCp2Include.Name = "colCp2Include";
            this.colCp2Include.Visible = true;
            this.colCp2Include.VisibleIndex = 3;
            this.colCp2Include.Width = 60;
            // 
            // chkEditPreview
            // 
            this.chkEditPreview.Name = "chkEditPreview";
            // 
            // colCp2IsKey
            // 
            this.colCp2IsKey.Caption = "Key";
            this.colCp2IsKey.ColumnEdit = this.chkEditPreview;
            this.colCp2IsKey.FieldName = "IsKey";
            this.colCp2IsKey.Name = "colCp2IsKey";
            this.colCp2IsKey.Visible = true;
            this.colCp2IsKey.VisibleIndex = 4;
            this.colCp2IsKey.Width = 50;
            // 
            // colCp2ControlKind
            // 
            this.colCp2ControlKind.Caption = "Control";
            this.colCp2ControlKind.ColumnEdit = this.lookUpControlKind;
            this.colCp2ControlKind.FieldName = "ControlKind";
            this.colCp2ControlKind.Name = "colCp2ControlKind";
            this.colCp2ControlKind.Visible = true;
            this.colCp2ControlKind.VisibleIndex = 5;
            this.colCp2ControlKind.Width = 100;
            // 
            // lookUpControlKind
            // 
            this.lookUpControlKind.AutoHeight = false;
            this.lookUpControlKind.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpControlKind.LookupKey = "L_SM0005";
            this.lookUpControlKind.Name = "lookUpControlKind";
            this.lookUpControlKind.NullText = "";
            // 
            // colCp2LookupKey
            // 
            this.colCp2LookupKey.Caption = "LookupKey (COMBO only)";
            this.colCp2LookupKey.FieldName = "LookupKey";
            this.colCp2LookupKey.Name = "colCp2LookupKey";
            this.colCp2LookupKey.Visible = true;
            this.colCp2LookupKey.VisibleIndex = 6;
            this.colCp2LookupKey.Width = 150;
            // 
            // colCp2Required
            // 
            this.colCp2Required.Caption = "Required";
            this.colCp2Required.ColumnEdit = this.chkEditPreview;
            this.colCp2Required.FieldName = "Required";
            this.colCp2Required.Name = "colCp2Required";
            this.colCp2Required.Visible = true;
            this.colCp2Required.VisibleIndex = 7;
            this.colCp2Required.Width = 60;
            // 
            // splitterWyn4
            // 
            this.splitterWyn4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn4.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitterWyn4.Location = new System.Drawing.Point(0, 200);
            this.splitterWyn4.Name = "splitterWyn4";
            this.splitterWyn4.Size = new System.Drawing.Size(1558, 6);
            this.splitterWyn4.TabIndex = 6;
            this.splitterWyn4.TabStop = false;
            // 
            // panelWyn9
            // 
            this.panelWyn9.Controls.Add(this.panelWyn7);
            this.panelWyn9.Controls.Add(this.splitterWyn3);
            this.panelWyn9.Controls.Add(this.panelWyn8);
            this.panelWyn9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn9.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn9.Location = new System.Drawing.Point(0, 0);
            this.panelWyn9.Name = "panelWyn9";
            this.panelWyn9.Size = new System.Drawing.Size(1558, 200);
            this.panelWyn9.TabIndex = 5;
            // 
            // panelWyn7
            // 
            this.panelWyn7.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn7.Appearance.Options.UseBackColor = true;
            this.panelWyn7.Controls.Add(this.grdQueryResultSets);
            this.panelWyn7.Controls.Add(this.panQueryToolbar);
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(308, 0);
            this.panelWyn7.Name = "panelWyn7";
            this.panelWyn7.Size = new System.Drawing.Size(1250, 200);
            this.panelWyn7.TabIndex = 3;
            // 
            // grdQueryResultSets
            // 
            this.grdQueryResultSets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdQueryResultSets.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdQueryResultSets.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdQueryResultSets.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdQueryResultSets.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdQueryResultSets.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdQueryResultSets.Location = new System.Drawing.Point(0, 34);
            this.grdQueryResultSets.MainView = this.gvwQueryResultSets;
            this.grdQueryResultSets.Name = "grdQueryResultSets";
            this.grdQueryResultSets.Size = new System.Drawing.Size(1250, 166);
            this.grdQueryResultSets.TabIndex = 0;
            this.grdQueryResultSets.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvwQueryResultSets});
            // 
            // gvwQueryResultSets
            // 
            this.gvwQueryResultSets.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colQrProc,
            this.colQrWorkType,
            this.colQrIndex,
            this.colQrTargetSlot});
            this.gvwQueryResultSets.GridControl = this.grdQueryResultSets;
            this.gvwQueryResultSets.Name = "gvwQueryResultSets";
            this.gvwQueryResultSets.OptionsBehavior.Editable = false;
            this.gvwQueryResultSets.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvwQueryResultSets.OptionsView.ColumnAutoWidth = false;
            this.gvwQueryResultSets.OptionsView.ShowGroupPanel = false;
            // 
            // colQrProc
            // 
            this.colQrProc.Caption = "Procedure Name";
            this.colQrProc.FieldName = "ProcName";
            this.colQrProc.Name = "colQrProc";
            this.colQrProc.Visible = true;
            this.colQrProc.VisibleIndex = 0;
            this.colQrProc.Width = 236;
            // 
            // colQrWorkType
            // 
            this.colQrWorkType.Caption = "WorkType";
            this.colQrWorkType.FieldName = "WorkType";
            this.colQrWorkType.Name = "colQrWorkType";
            this.colQrWorkType.Visible = true;
            this.colQrWorkType.VisibleIndex = 1;
            this.colQrWorkType.Width = 112;
            // 
            // colQrIndex
            // 
            this.colQrIndex.Caption = "DataSet Seq";
            this.colQrIndex.FieldName = "ResultSetIndex";
            this.colQrIndex.Name = "colQrIndex";
            this.colQrIndex.OptionsColumn.AllowEdit = false;
            this.colQrIndex.Visible = true;
            this.colQrIndex.VisibleIndex = 2;
            this.colQrIndex.Width = 111;
            // 
            // colQrTargetSlot
            // 
            this.colQrTargetSlot.Caption = "Target Control";
            this.colQrTargetSlot.FieldName = "TargetSlot";
            this.colQrTargetSlot.Name = "colQrTargetSlot";
            this.colQrTargetSlot.Visible = true;
            this.colQrTargetSlot.VisibleIndex = 3;
            this.colQrTargetSlot.Width = 220;
            // 
            // panQueryToolbar
            // 
            this.panQueryToolbar.Controls.Add(this.btnDescribeQueryMulti);
            this.panQueryToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panQueryToolbar.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panQueryToolbar.Location = new System.Drawing.Point(0, 0);
            this.panQueryToolbar.Name = "panQueryToolbar";
            this.panQueryToolbar.Size = new System.Drawing.Size(1250, 34);
            this.panQueryToolbar.TabIndex = 2;
            // 
            // btnDescribeQueryMulti
            // 
            this.btnDescribeQueryMulti.BackColor = System.Drawing.Color.Transparent;
            this.btnDescribeQueryMulti.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnDescribeQueryMulti.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDescribeQueryMulti.FillColor = System.Drawing.Color.White;
            this.btnDescribeQueryMulti.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDescribeQueryMulti.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDescribeQueryMulti.Image = null;
            this.btnDescribeQueryMulti.Location = new System.Drawing.Point(8, 2);
            this.btnDescribeQueryMulti.Name = "btnDescribeQueryMulti";
            this.btnDescribeQueryMulti.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDescribeQueryMulti.Size = new System.Drawing.Size(160, 29);
            this.btnDescribeQueryMulti.TabIndex = 1;
            this.btnDescribeQueryMulti.Text = "레코드셋 조회";
            this.btnDescribeQueryMulti.ToolTip = "전체 행(Proc/WorkType) 레코드셋 다시 조회 - 결과셋 수만큼 행이 자동으로 채워집니다";
            // 
            // splitterWyn3
            // 
            this.splitterWyn3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn3.Location = new System.Drawing.Point(300, 0);
            this.splitterWyn3.Name = "splitterWyn3";
            this.splitterWyn3.Size = new System.Drawing.Size(8, 200);
            this.splitterWyn3.TabIndex = 19;
            this.splitterWyn3.TabStop = false;
            // 
            // panelWyn8
            // 
            this.panelWyn8.Controls.Add(this.panQueryPlan);
            this.panelWyn8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelWyn8.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn8.Location = new System.Drawing.Point(0, 0);
            this.panelWyn8.Name = "panelWyn8";
            this.panelWyn8.Size = new System.Drawing.Size(300, 200);
            this.panelWyn8.TabIndex = 4;
            // 
            // panQueryPlan
            // 
            this.panQueryPlan.Controls.Add(this.grdQueryProcPlan);
            this.panQueryPlan.Controls.Add(this.panelWyn5);
            this.panQueryPlan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panQueryPlan.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panQueryPlan.Location = new System.Drawing.Point(0, 0);
            this.panQueryPlan.Name = "panQueryPlan";
            this.panQueryPlan.Size = new System.Drawing.Size(300, 200);
            this.panQueryPlan.TabIndex = 13;
            // 
            // grdQueryProcPlan
            // 
            this.grdQueryProcPlan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdQueryProcPlan.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdQueryProcPlan.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdQueryProcPlan.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdQueryProcPlan.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdQueryProcPlan.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdQueryProcPlan.Location = new System.Drawing.Point(0, 33);
            this.grdQueryProcPlan.MainView = this.gvwQueryProcPlan;
            this.grdQueryProcPlan.Name = "grdQueryProcPlan";
            this.grdQueryProcPlan.Size = new System.Drawing.Size(300, 167);
            this.grdQueryProcPlan.TabIndex = 1;
            this.grdQueryProcPlan.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvwQueryProcPlan});
            // 
            // gvwQueryProcPlan
            // 
            this.gvwQueryProcPlan.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colQppProc});
            this.gvwQueryProcPlan.GridControl = this.grdQueryProcPlan;
            this.gvwQueryProcPlan.Name = "gvwQueryProcPlan";
            this.gvwQueryProcPlan.OptionsBehavior.Editable = false;
            this.gvwQueryProcPlan.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvwQueryProcPlan.OptionsView.ShowGroupPanel = false;
            this.gvwQueryProcPlan.OptionsView.ShowIndicator = false;
            // 
            // colQppProc
            // 
            this.colQppProc.Caption = "Query Procedure Name";
            this.colQppProc.FieldName = "ProcName";
            this.colQppProc.Name = "colQppProc";
            this.colQppProc.Visible = true;
            this.colQppProc.VisibleIndex = 0;
            this.colQppProc.Width = 150;
            // 
            // panelWyn5
            // 
            this.panelWyn5.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn5.Appearance.Options.UseBackColor = true;
            this.panelWyn5.Controls.Add(this.btnDescribeQueryAll);
            this.panelWyn5.Controls.Add(this.btnDeleteRow1);
            this.panelWyn5.Controls.Add(this.btnRowAdd1);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(0, 0);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Size = new System.Drawing.Size(300, 33);
            this.panelWyn5.TabIndex = 9;
            // 
            // btnDescribeQueryAll
            // 
            this.btnDescribeQueryAll.BackColor = System.Drawing.Color.Transparent;
            this.btnDescribeQueryAll.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnDescribeQueryAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDescribeQueryAll.FillColor = System.Drawing.Color.White;
            this.btnDescribeQueryAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDescribeQueryAll.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDescribeQueryAll.Image = null;
            this.btnDescribeQueryAll.Location = new System.Drawing.Point(186, 2);
            this.btnDescribeQueryAll.Name = "btnDescribeQueryAll";
            this.btnDescribeQueryAll.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDescribeQueryAll.Size = new System.Drawing.Size(85, 27);
            this.btnDescribeQueryAll.TabIndex = 1;
            this.btnDescribeQueryAll.Text = "Describe";
            this.btnDescribeQueryAll.ToolTip = "이 목록의 모든 프로시저를 work_type=\"Q\"/grd1 기준으로 조회합니다 - 다른 슬롯/워크타입이 필요하면 Query Sources 탭에서" +
    " 고치세요";
            // 
            // btnDeleteRow1
            // 
            this.btnDeleteRow1.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteRow1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnDeleteRow1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteRow1.FillColor = System.Drawing.Color.White;
            this.btnDeleteRow1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDeleteRow1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDeleteRow1.Image = null;
            this.btnDeleteRow1.Location = new System.Drawing.Point(94, 2);
            this.btnDeleteRow1.Name = "btnDeleteRow1";
            this.btnDeleteRow1.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDeleteRow1.Size = new System.Drawing.Size(85, 27);
            this.btnDeleteRow1.TabIndex = 1;
            this.btnDeleteRow1.Text = "Delete";
            this.btnDeleteRow1.ToolTip = "이 목록의 모든 프로시저를 Header/panData 기준으로 매칭합니다 - 다른 Scope/Slot이 필요하면 Save Actions 탭에서 고" +
    "치세요";
            // 
            // btnRowAdd1
            // 
            this.btnRowAdd1.BackColor = System.Drawing.Color.Transparent;
            this.btnRowAdd1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnRowAdd1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRowAdd1.FillColor = System.Drawing.Color.White;
            this.btnRowAdd1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnRowAdd1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnRowAdd1.Image = null;
            this.btnRowAdd1.Location = new System.Drawing.Point(3, 2);
            this.btnRowAdd1.Name = "btnRowAdd1";
            this.btnRowAdd1.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnRowAdd1.Size = new System.Drawing.Size(85, 27);
            this.btnRowAdd1.TabIndex = 1;
            this.btnRowAdd1.Text = "Add";
            this.btnRowAdd1.ToolTip = "이 목록의 모든 프로시저를 Header/panData 기준으로 매칭합니다 - 다른 Scope/Slot이 필요하면 Save Actions 탭에서 고" +
    "치세요";
            // 
            // tabSaveMulti
            // 
            this.tabSaveMulti.Controls.Add(this.grdSaveActionParams);
            this.tabSaveMulti.Controls.Add(this.splitterWyn1);
            this.tabSaveMulti.Controls.Add(this.panelWyn10);
            this.tabSaveMulti.Name = "tabSaveMulti";
            this.tabSaveMulti.Size = new System.Drawing.Size(1558, 542);
            this.tabSaveMulti.Text = "Save Actions (다중)";
            // 
            // grdSaveActionParams
            // 
            this.grdSaveActionParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSaveActionParams.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdSaveActionParams.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdSaveActionParams.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdSaveActionParams.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdSaveActionParams.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdSaveActionParams.Location = new System.Drawing.Point(0, 208);
            this.grdSaveActionParams.MainView = this.gvwSaveActionParams;
            this.grdSaveActionParams.Name = "grdSaveActionParams";
            this.grdSaveActionParams.Size = new System.Drawing.Size(1558, 334);
            this.grdSaveActionParams.TabIndex = 2;
            this.grdSaveActionParams.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvwSaveActionParams});
            // 
            // gvwSaveActionParams
            // 
            this.gvwSaveActionParams.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colSapName,
            this.colSapSqlType,
            this.colSapMatchedColumn});
            this.gvwSaveActionParams.GridControl = this.grdSaveActionParams;
            this.gvwSaveActionParams.Name = "gvwSaveActionParams";
            this.gvwSaveActionParams.OptionsBehavior.Editable = false;
            this.gvwSaveActionParams.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            // 
            // colSapName
            // 
            this.colSapName.Caption = "Param";
            this.colSapName.FieldName = "Name";
            this.colSapName.Name = "colSapName";
            this.colSapName.OptionsColumn.AllowEdit = false;
            this.colSapName.Visible = true;
            this.colSapName.VisibleIndex = 0;
            this.colSapName.Width = 150;
            // 
            // colSapSqlType
            // 
            this.colSapSqlType.Caption = "Data Type";
            this.colSapSqlType.FieldName = "SqlType";
            this.colSapSqlType.Name = "colSapSqlType";
            this.colSapSqlType.OptionsColumn.AllowEdit = false;
            this.colSapSqlType.Visible = true;
            this.colSapSqlType.VisibleIndex = 1;
            this.colSapSqlType.Width = 90;
            // 
            // colSapMatchedColumn
            // 
            this.colSapMatchedColumn.Caption = "Matched Column (blank = TODO)";
            this.colSapMatchedColumn.FieldName = "MatchedColumn";
            this.colSapMatchedColumn.Name = "colSapMatchedColumn";
            this.colSapMatchedColumn.Visible = true;
            this.colSapMatchedColumn.VisibleIndex = 2;
            this.colSapMatchedColumn.Width = 220;
            // 
            // splitterWyn1
            // 
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitterWyn1.Location = new System.Drawing.Point(0, 200);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(1558, 8);
            this.splitterWyn1.TabIndex = 16;
            this.splitterWyn1.TabStop = false;
            // 
            // panelWyn10
            // 
            this.panelWyn10.Controls.Add(this.panelWyn11);
            this.panelWyn10.Controls.Add(this.splitterWyn2);
            this.panelWyn10.Controls.Add(this.panSavePlan);
            this.panelWyn10.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn10.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn10.Location = new System.Drawing.Point(0, 0);
            this.panelWyn10.Name = "panelWyn10";
            this.panelWyn10.Size = new System.Drawing.Size(1558, 200);
            this.panelWyn10.TabIndex = 3;
            // 
            // panelWyn11
            // 
            this.panelWyn11.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn11.Appearance.Options.UseBackColor = true;
            this.panelWyn11.Controls.Add(this.grdSaveActions);
            this.panelWyn11.Controls.Add(this.panSaveToolbar);
            this.panelWyn11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn11.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn11.Location = new System.Drawing.Point(308, 0);
            this.panelWyn11.Name = "panelWyn11";
            this.panelWyn11.Size = new System.Drawing.Size(1250, 200);
            this.panelWyn11.TabIndex = 4;
            // 
            // grdSaveActions
            // 
            this.grdSaveActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSaveActions.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdSaveActions.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdSaveActions.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdSaveActions.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdSaveActions.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdSaveActions.Location = new System.Drawing.Point(0, 34);
            this.grdSaveActions.MainView = this.gvwSaveActions;
            this.grdSaveActions.Name = "grdSaveActions";
            this.grdSaveActions.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cboEditScope});
            this.grdSaveActions.Size = new System.Drawing.Size(1250, 166);
            this.grdSaveActions.TabIndex = 0;
            this.grdSaveActions.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvwSaveActions});
            // 
            // gvwSaveActions
            // 
            this.gvwSaveActions.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colSaProc,
            this.colSaScope,
            this.colSaSourceSlot,
            this.colSaKeyParam});
            this.gvwSaveActions.GridControl = this.grdSaveActions;
            this.gvwSaveActions.Name = "gvwSaveActions";
            this.gvwSaveActions.OptionsBehavior.Editable = false;
            this.gvwSaveActions.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvwSaveActions.OptionsView.ShowGroupPanel = false;
            // 
            // colSaProc
            // 
            this.colSaProc.Caption = "Procedure Name";
            this.colSaProc.FieldName = "ProcName";
            this.colSaProc.Name = "colSaProc";
            this.colSaProc.Visible = true;
            this.colSaProc.VisibleIndex = 0;
            this.colSaProc.Width = 220;
            // 
            // colSaScope
            // 
            this.colSaScope.Caption = "Scope";
            this.colSaScope.ColumnEdit = this.cboEditScope;
            this.colSaScope.FieldName = "Scope";
            this.colSaScope.Name = "colSaScope";
            this.colSaScope.Visible = true;
            this.colSaScope.VisibleIndex = 1;
            this.colSaScope.Width = 90;
            // 
            // cboEditScope
            // 
            this.cboEditScope.Items.AddRange(new object[] {
            "Header",
            "Detail"});
            this.cboEditScope.Name = "cboEditScope";
            this.cboEditScope.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // colSaSourceSlot
            // 
            this.colSaSourceSlot.Caption = "Source Control";
            this.colSaSourceSlot.FieldName = "SourceSlot";
            this.colSaSourceSlot.Name = "colSaSourceSlot";
            this.colSaSourceSlot.Visible = true;
            this.colSaSourceSlot.VisibleIndex = 2;
            this.colSaSourceSlot.Width = 220;
            // 
            // colSaKeyParam
            // 
            this.colSaKeyParam.Caption = "Key Param (Detail만, 상위키를 받을 파라미터)";
            this.colSaKeyParam.FieldName = "KeyParam";
            this.colSaKeyParam.Name = "colSaKeyParam";
            this.colSaKeyParam.Visible = true;
            this.colSaKeyParam.VisibleIndex = 3;
            this.colSaKeyParam.Width = 260;
            // 
            // panSaveToolbar
            // 
            this.panSaveToolbar.Controls.Add(this.btnDescribeSaveMulti);
            this.panSaveToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSaveToolbar.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panSaveToolbar.Location = new System.Drawing.Point(0, 0);
            this.panSaveToolbar.Name = "panSaveToolbar";
            this.panSaveToolbar.Size = new System.Drawing.Size(1250, 34);
            this.panSaveToolbar.TabIndex = 2;
            // 
            // btnDescribeSaveMulti
            // 
            this.btnDescribeSaveMulti.BackColor = System.Drawing.Color.Transparent;
            this.btnDescribeSaveMulti.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnDescribeSaveMulti.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDescribeSaveMulti.FillColor = System.Drawing.Color.White;
            this.btnDescribeSaveMulti.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDescribeSaveMulti.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDescribeSaveMulti.Image = null;
            this.btnDescribeSaveMulti.Location = new System.Drawing.Point(8, 2);
            this.btnDescribeSaveMulti.Name = "btnDescribeSaveMulti";
            this.btnDescribeSaveMulti.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDescribeSaveMulti.Size = new System.Drawing.Size(160, 29);
            this.btnDescribeSaveMulti.TabIndex = 1;
            this.btnDescribeSaveMulti.Text = "파라미터 조회/매칭";
            this.btnDescribeSaveMulti.ToolTip = "전체 행(Proc) 파라미터 다시 조회/매칭";
            // 
            // splitterWyn2
            // 
            this.splitterWyn2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn2.Location = new System.Drawing.Point(300, 0);
            this.splitterWyn2.Name = "splitterWyn2";
            this.splitterWyn2.Size = new System.Drawing.Size(8, 200);
            this.splitterWyn2.TabIndex = 18;
            this.splitterWyn2.TabStop = false;
            // 
            // panSavePlan
            // 
            this.panSavePlan.Controls.Add(this.grdSaveProcPlan);
            this.panSavePlan.Controls.Add(this.panelWyn4);
            this.panSavePlan.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSavePlan.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panSavePlan.Location = new System.Drawing.Point(0, 0);
            this.panSavePlan.Name = "panSavePlan";
            this.panSavePlan.Size = new System.Drawing.Size(300, 200);
            this.panSavePlan.TabIndex = 14;
            // 
            // grdSaveProcPlan
            // 
            this.grdSaveProcPlan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSaveProcPlan.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdSaveProcPlan.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdSaveProcPlan.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdSaveProcPlan.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdSaveProcPlan.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdSaveProcPlan.Location = new System.Drawing.Point(0, 33);
            this.grdSaveProcPlan.MainView = this.gvwSaveProcPlan;
            this.grdSaveProcPlan.Name = "grdSaveProcPlan";
            this.grdSaveProcPlan.Size = new System.Drawing.Size(300, 167);
            this.grdSaveProcPlan.TabIndex = 1;
            this.grdSaveProcPlan.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvwSaveProcPlan});
            // 
            // gvwSaveProcPlan
            // 
            this.gvwSaveProcPlan.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colSppProc});
            this.gvwSaveProcPlan.GridControl = this.grdSaveProcPlan;
            this.gvwSaveProcPlan.Name = "gvwSaveProcPlan";
            this.gvwSaveProcPlan.OptionsBehavior.Editable = false;
            this.gvwSaveProcPlan.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvwSaveProcPlan.OptionsView.ShowGroupPanel = false;
            this.gvwSaveProcPlan.OptionsView.ShowIndicator = false;
            // 
            // colSppProc
            // 
            this.colSppProc.Caption = "Save Procedure Name";
            this.colSppProc.FieldName = "ProcName";
            this.colSppProc.Name = "colSppProc";
            this.colSppProc.Visible = true;
            this.colSppProc.VisibleIndex = 0;
            this.colSppProc.Width = 150;
            // 
            // panelWyn4
            // 
            this.panelWyn4.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn4.Appearance.Options.UseBackColor = true;
            this.panelWyn4.Controls.Add(this.btnDeleteRow2);
            this.panelWyn4.Controls.Add(this.btnRowAdd2);
            this.panelWyn4.Controls.Add(this.btnDescribeSaveAll);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(0, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Size = new System.Drawing.Size(300, 33);
            this.panelWyn4.TabIndex = 8;
            // 
            // btnDeleteRow2
            // 
            this.btnDeleteRow2.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteRow2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnDeleteRow2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteRow2.FillColor = System.Drawing.Color.White;
            this.btnDeleteRow2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDeleteRow2.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDeleteRow2.Image = null;
            this.btnDeleteRow2.Location = new System.Drawing.Point(94, 2);
            this.btnDeleteRow2.Name = "btnDeleteRow2";
            this.btnDeleteRow2.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDeleteRow2.Size = new System.Drawing.Size(85, 27);
            this.btnDeleteRow2.TabIndex = 1;
            this.btnDeleteRow2.Text = "Delete";
            this.btnDeleteRow2.ToolTip = "이 목록의 모든 프로시저를 Header/panData 기준으로 매칭합니다 - 다른 Scope/Slot이 필요하면 Save Actions 탭에서 고" +
    "치세요";
            // 
            // btnRowAdd2
            // 
            this.btnRowAdd2.BackColor = System.Drawing.Color.Transparent;
            this.btnRowAdd2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnRowAdd2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRowAdd2.FillColor = System.Drawing.Color.White;
            this.btnRowAdd2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnRowAdd2.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnRowAdd2.Image = null;
            this.btnRowAdd2.Location = new System.Drawing.Point(3, 2);
            this.btnRowAdd2.Name = "btnRowAdd2";
            this.btnRowAdd2.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnRowAdd2.Size = new System.Drawing.Size(85, 27);
            this.btnRowAdd2.TabIndex = 1;
            this.btnRowAdd2.Text = "Add";
            this.btnRowAdd2.ToolTip = "이 목록의 모든 프로시저를 Header/panData 기준으로 매칭합니다 - 다른 Scope/Slot이 필요하면 Save Actions 탭에서 고" +
    "치세요";
            // 
            // btnDescribeSaveAll
            // 
            this.btnDescribeSaveAll.BackColor = System.Drawing.Color.Transparent;
            this.btnDescribeSaveAll.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnDescribeSaveAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDescribeSaveAll.FillColor = System.Drawing.Color.White;
            this.btnDescribeSaveAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDescribeSaveAll.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDescribeSaveAll.Image = null;
            this.btnDescribeSaveAll.Location = new System.Drawing.Point(186, 2);
            this.btnDescribeSaveAll.Name = "btnDescribeSaveAll";
            this.btnDescribeSaveAll.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDescribeSaveAll.Size = new System.Drawing.Size(85, 27);
            this.btnDescribeSaveAll.TabIndex = 1;
            this.btnDescribeSaveAll.Text = "Describe";
            this.btnDescribeSaveAll.ToolTip = "이 목록의 모든 프로시저를 Header/panData 기준으로 매칭합니다 - 다른 Scope/Slot이 필요하면 Save Actions 탭에서 고" +
    "치세요";
            // 
            // panelWyn3
            // 
            this.panelWyn3.Controls.Add(this.panelForm);
            this.panelWyn3.Controls.Add(this.panelWyn12);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(0, 5);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Size = new System.Drawing.Size(1560, 132);
            this.panelWyn3.TabIndex = 1;
            // 
            // panelForm
            // 
            this.panelForm.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panelForm.Controls.Add(this.cboModule);
            this.panelForm.Controls.Add(this.lblModule);
            this.panelForm.Controls.Add(this.lblScreenClassNm);
            this.panelForm.Controls.Add(this.txtScreenClassNm);
            this.panelForm.Controls.Add(this.lblMenuCaption);
            this.panelForm.Controls.Add(this.txtMenuCaption);
            this.panelForm.Controls.Add(this.lblUpperMenuCd);
            this.panelForm.Controls.Add(this.txtUpperMenuCd);
            this.panelForm.Controls.Add(this.txtUpperMenuNm);
            this.panelForm.Controls.Add(this.lblProcPrefix);
            this.panelForm.Controls.Add(this.txtProcPrefix);
            this.panelForm.Controls.Add(this.lblDetailKeyParam);
            this.panelForm.Controls.Add(this.txtDetailKeyParam);
            this.panelForm.Controls.Add(this.lblMasterKeyColumn);
            this.panelForm.Controls.Add(this.txtMasterKeyColumn);
            this.panelForm.Controls.Add(this.lblMasterParentColumn);
            this.panelForm.Controls.Add(this.txtMasterParentColumn);
            this.panelForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelForm.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelForm.Location = new System.Drawing.Point(0, 48);
            this.panelForm.Name = "panelForm";
            this.panelForm.Size = new System.Drawing.Size(1560, 84);
            this.panelForm.TabIndex = 0;
            // 
            // cboModule
            // 
            this.cboModule.EditValue = "";
            this.cboModule.Location = new System.Drawing.Point(99, 6);
            this.cboModule.LookupKey = "L_SM0003";
            this.cboModule.Name = "cboModule";
            this.cboModule.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboModule.Properties.NullText = "";
            this.cboModule.Size = new System.Drawing.Size(168, 20);
            this.cboModule.TabIndex = 0;
            // 
            // lblModule
            // 
            this.lblModule.Location = new System.Drawing.Point(55, 9);
            this.lblModule.Name = "lblModule";
            this.lblModule.Size = new System.Drawing.Size(39, 14);
            this.lblModule.TabIndex = 0;
            this.lblModule.Text = "Module";
            // 
            // lblScreenClassNm
            // 
            this.lblScreenClassNm.Location = new System.Drawing.Point(318, 35);
            this.lblScreenClassNm.Name = "lblScreenClassNm";
            this.lblScreenClassNm.Size = new System.Drawing.Size(62, 14);
            this.lblScreenClassNm.TabIndex = 2;
            this.lblScreenClassNm.Text = "Form Name";
            // 
            // txtScreenClassNm
            // 
            this.txtScreenClassNm.Location = new System.Drawing.Point(385, 32);
            this.txtScreenClassNm.Name = "txtScreenClassNm";
            this.txtScreenClassNm.Size = new System.Drawing.Size(168, 20);
            this.txtScreenClassNm.TabIndex = 3;
            // 
            // lblMenuCaption
            // 
            this.lblMenuCaption.Location = new System.Drawing.Point(587, 35);
            this.lblMenuCaption.Name = "lblMenuCaption";
            this.lblMenuCaption.Size = new System.Drawing.Size(75, 14);
            this.lblMenuCaption.TabIndex = 4;
            this.lblMenuCaption.Text = "Menu Caption";
            // 
            // txtMenuCaption
            // 
            this.txtMenuCaption.Location = new System.Drawing.Point(668, 32);
            this.txtMenuCaption.Name = "txtMenuCaption";
            this.txtMenuCaption.Size = new System.Drawing.Size(168, 20);
            this.txtMenuCaption.TabIndex = 4;
            // 
            // lblUpperMenuCd
            // 
            this.lblUpperMenuCd.Location = new System.Drawing.Point(27, 35);
            this.lblUpperMenuCd.Name = "lblUpperMenuCd";
            this.lblUpperMenuCd.Size = new System.Drawing.Size(67, 14);
            this.lblUpperMenuCd.TabIndex = 6;
            this.lblUpperMenuCd.Text = "Upper Menu";
            // 
            // txtUpperMenuCd
            // 
            this.txtUpperMenuCd.Location = new System.Drawing.Point(850, 341);
            this.txtUpperMenuCd.Name = "txtUpperMenuCd";
            this.txtUpperMenuCd.Properties.ReadOnly = true;
            this.txtUpperMenuCd.Size = new System.Drawing.Size(100, 20);
            this.txtUpperMenuCd.TabIndex = 7;
            this.txtUpperMenuCd.Visible = false;
            // 
            // txtUpperMenuNm
            // 
            this.txtUpperMenuNm.Location = new System.Drawing.Point(99, 32);
            this.txtUpperMenuNm.Name = "txtUpperMenuNm";
            this.txtUpperMenuNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", 24, true, true, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.txtUpperMenuNm.Size = new System.Drawing.Size(168, 20);
            this.txtUpperMenuNm.TabIndex = 2;
            this.txtUpperMenuNm.ToolTip = null;
            // 
            // lblProcPrefix
            // 
            this.lblProcPrefix.Location = new System.Drawing.Point(322, 9);
            this.lblProcPrefix.Name = "lblProcPrefix";
            this.lblProcPrefix.Size = new System.Drawing.Size(58, 14);
            this.lblProcPrefix.TabIndex = 9;
            this.lblProcPrefix.Text = "Proc Prefix";
            // 
            // txtProcPrefix
            // 
            this.txtProcPrefix.Location = new System.Drawing.Point(385, 6);
            this.txtProcPrefix.Name = "txtProcPrefix";
            this.txtProcPrefix.Size = new System.Drawing.Size(168, 20);
            this.txtProcPrefix.TabIndex = 1;
            // 
            // lblDetailKeyParam
            // 
            this.lblDetailKeyParam.Location = new System.Drawing.Point(289, 61);
            this.lblDetailKeyParam.Name = "lblDetailKeyParam";
            this.lblDetailKeyParam.Size = new System.Drawing.Size(91, 14);
            this.lblDetailKeyParam.TabIndex = 18;
            this.lblDetailKeyParam.Text = "Detail Key Param";
            // 
            // txtDetailKeyParam
            // 
            this.txtDetailKeyParam.Location = new System.Drawing.Point(385, 58);
            this.txtDetailKeyParam.Name = "txtDetailKeyParam";
            this.txtDetailKeyParam.Size = new System.Drawing.Size(168, 20);
            this.txtDetailKeyParam.TabIndex = 6;
            // 
            // lblMasterKeyColumn
            // 
            this.lblMasterKeyColumn.Location = new System.Drawing.Point(10, 61);
            this.lblMasterKeyColumn.Name = "lblMasterKeyColumn";
            this.lblMasterKeyColumn.Size = new System.Drawing.Size(84, 14);
            this.lblMasterKeyColumn.TabIndex = 20;
            this.lblMasterKeyColumn.Text = "Master Key Col.";
            // 
            // txtMasterKeyColumn
            // 
            this.txtMasterKeyColumn.Location = new System.Drawing.Point(99, 58);
            this.txtMasterKeyColumn.Name = "txtMasterKeyColumn";
            this.txtMasterKeyColumn.Size = new System.Drawing.Size(168, 20);
            this.txtMasterKeyColumn.TabIndex = 5;
            // 
            // lblMasterParentColumn
            // 
            this.lblMasterParentColumn.Location = new System.Drawing.Point(562, 61);
            this.lblMasterParentColumn.Name = "lblMasterParentColumn";
            this.lblMasterParentColumn.Size = new System.Drawing.Size(100, 14);
            this.lblMasterParentColumn.TabIndex = 21;
            this.lblMasterParentColumn.Text = "Master Parent Col.";
            // 
            // txtMasterParentColumn
            // 
            this.txtMasterParentColumn.Location = new System.Drawing.Point(668, 58);
            this.txtMasterParentColumn.Name = "txtMasterParentColumn";
            this.txtMasterParentColumn.Size = new System.Drawing.Size(168, 20);
            this.txtMasterParentColumn.TabIndex = 7;
            // 
            // panelWyn12
            // 
            this.panelWyn12.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn12.Appearance.Options.UseBackColor = true;
            this.panelWyn12.Controls.Add(this.btnGenerate);
            this.panelWyn12.Controls.Add(this.lblTemplateKind);
            this.panelWyn12.Controls.Add(this.cboTemplateKind);
            this.panelWyn12.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn12.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn12.Location = new System.Drawing.Point(0, 0);
            this.panelWyn12.Name = "panelWyn12";
            this.panelWyn12.Size = new System.Drawing.Size(1560, 48);
            this.panelWyn12.TabIndex = 9;
            // 
            // btnGenerate
            // 
            this.btnGenerate.BackColor = System.Drawing.Color.Transparent;
            this.btnGenerate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnGenerate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerate.FillColor = System.Drawing.Color.White;
            this.btnGenerate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnGenerate.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnGenerate.Image = null;
            this.btnGenerate.Location = new System.Drawing.Point(387, 2);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnGenerate.Size = new System.Drawing.Size(167, 42);
            this.btnGenerate.TabIndex = 8;
            this.btnGenerate.Text = "Create Form";
            this.btnGenerate.ToolTip = null;
            // 
            // lblTemplateKind
            // 
            this.lblTemplateKind.Location = new System.Drawing.Point(42, 5);
            this.lblTemplateKind.Name = "lblTemplateKind";
            this.lblTemplateKind.Size = new System.Drawing.Size(52, 14);
            this.lblTemplateKind.TabIndex = 11;
            this.lblTemplateKind.Text = "Template";
            // 
            // cboTemplateKind
            // 
            this.cboTemplateKind.Location = new System.Drawing.Point(100, 3);
            this.cboTemplateKind.Name = "cboTemplateKind";
            this.cboTemplateKind.Size = new System.Drawing.Size(280, 20);
            this.cboTemplateKind.TabIndex = 0;
            // 
            // paTitle
            // 
            this.paTitle.Controls.Add(this.sectionHeaderWyn1);
            this.paTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle.Location = new System.Drawing.Point(0, 0);
            this.paTitle.Name = "paTitle";
            this.paTitle.Size = new System.Drawing.Size(1570, 33);
            this.paTitle.TabIndex = 6;
            // 
            // sectionHeaderWyn1
            // 
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Dock = System.Windows.Forms.DockStyle.Left;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(0, 0);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(322, 33);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 9;
            this.sectionHeaderWyn1.Text = "AI Builder [frmAIBuilder]";
            // 
            // frmAIBuilder
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1570, 738);
            this.Controls.Add(this.panBase);
            this.Controls.Add(this.paTitle);
            this.Name = "frmAIBuilder";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelPreview)).EndInit();
            this.panelPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabPreview)).EndInit();
            this.tabPreview.ResumeLayout(false);
            this.tabSaveParams.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSaveParams)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSaveParams)).EndInit();
            this.tabQueryMulti.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdColumnPreview2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwColumnPreview2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEditPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpControlKind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn9)).EndInit();
            this.panelWyn9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
            this.panelWyn7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdQueryResultSets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwQueryResultSets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panQueryToolbar)).EndInit();
            this.panQueryToolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).EndInit();
            this.panelWyn8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panQueryPlan)).EndInit();
            this.panQueryPlan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdQueryProcPlan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwQueryProcPlan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            this.tabSaveMulti.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSaveActionParams)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSaveActionParams)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn10)).EndInit();
            this.panelWyn10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn11)).EndInit();
            this.panelWyn11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSaveActions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSaveActions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboEditScope)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panSaveToolbar)).EndInit();
            this.panSaveToolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panSavePlan)).EndInit();
            this.panSavePlan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSaveProcPlan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSaveProcPlan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelForm)).EndInit();
            this.panelForm.ResumeLayout(false);
            this.panelForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboModule.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtScreenClassNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMenuCaption.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpperMenuCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpperMenuNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProcPrefix.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailKeyParam.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMasterKeyColumn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMasterParentColumn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn12)).EndInit();
            this.panelWyn12.ResumeLayout(false);
            this.panelWyn12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboTemplateKind.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
            this.paTitle.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private PanelWyn panBase;
    private PanelWyn panelForm;
    private DevExpress.XtraEditors.LabelControl lblModule;
    private DevExpress.XtraEditors.LabelControl lblScreenClassNm;
    private TextEditWyn txtScreenClassNm;
    private DevExpress.XtraEditors.LabelControl lblMenuCaption;
    private TextEditWyn txtMenuCaption;
    private DevExpress.XtraEditors.LabelControl lblUpperMenuCd;
    private TextEditWyn txtUpperMenuCd;
    private PopupLookupEditWyn txtUpperMenuNm;
    private DevExpress.XtraEditors.LabelControl lblProcPrefix;
    private TextEditWyn txtProcPrefix;
    private DevExpress.XtraEditors.LabelControl lblTemplateKind;
    private DevExpress.XtraEditors.ImageComboBoxEdit cboTemplateKind;
    private DevExpress.XtraEditors.LabelControl lblDetailKeyParam;
    private TextEditWyn txtDetailKeyParam;
    private DevExpress.XtraEditors.LabelControl lblMasterKeyColumn;
    private TextEditWyn txtMasterKeyColumn;
    private DevExpress.XtraEditors.LabelControl lblMasterParentColumn;
    private TextEditWyn txtMasterParentColumn;
    private ButtonWyn btnGenerate;

    private PanelWyn panelPreview;
    private TabControlWyn tabPreview;

    private DevExpress.XtraTab.XtraTabPage tabSaveParams;
    private GridControlWyn grdSaveParams;
    private GridViewWyn gvwSaveParams;
    private DevExpress.XtraGrid.Columns.GridColumn colSpName;
    private DevExpress.XtraGrid.Columns.GridColumn colSpSqlType;
    private DevExpress.XtraGrid.Columns.GridColumn colSpMatchedColumn;

    // 다중 쿼리소스/저장액션(MasterFormSubGrid류) - 몇 개든 행으로 추가하면 되고, 그리드 자체는
    // 항상 이 2개 탭 고정이다(템플릿에 그리드/저장프로시저가 늘어나도 여기는 안 바뀜).
    private DevExpress.XtraTab.XtraTabPage tabQueryMulti;
    private PanelWyn panQueryToolbar;
    private ButtonWyn btnDescribeQueryMulti;
    private ButtonWyn btnDescribeQueryAll;
    private PanelWyn panQueryPlan;
    private GridControlWyn grdQueryProcPlan;
    private GridViewWyn gvwQueryProcPlan;
    private DevExpress.XtraGrid.Columns.GridColumn colQppProc;
    private GridControlWyn grdQueryResultSets;
    private GridViewWyn gvwQueryResultSets;
    private DevExpress.XtraGrid.Columns.GridColumn colQrProc;
    private DevExpress.XtraGrid.Columns.GridColumn colQrWorkType;
    private DevExpress.XtraGrid.Columns.GridColumn colQrIndex;
    private DevExpress.XtraGrid.Columns.GridColumn colQrTargetSlot;
    private GridControlWyn grdColumnPreview2;
    private GridViewWyn gvwColumnPreview2;
    private DevExpress.XtraGrid.Columns.GridColumn colCp2Name;
    private DevExpress.XtraGrid.Columns.GridColumn colCp2SqlType;
    private DevExpress.XtraGrid.Columns.GridColumn colCp2Caption;
    private DevExpress.XtraGrid.Columns.GridColumn colCp2Include;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkEditPreview;
    private DevExpress.XtraGrid.Columns.GridColumn colCp2IsKey;
    private DevExpress.XtraGrid.Columns.GridColumn colCp2ControlKind;
    private DevExpress.XtraGrid.Columns.GridColumn colCp2LookupKey;
    private DevExpress.XtraGrid.Columns.GridColumn colCp2Required;

    private DevExpress.XtraTab.XtraTabPage tabSaveMulti;
    private PanelWyn panSaveToolbar;
    private ButtonWyn btnDescribeSaveMulti;
    private ButtonWyn btnDescribeSaveAll;
    private PanelWyn panSavePlan;
    private GridControlWyn grdSaveProcPlan;
    private GridViewWyn gvwSaveProcPlan;
    private DevExpress.XtraGrid.Columns.GridColumn colSppProc;
    private GridControlWyn grdSaveActions;
    private GridViewWyn gvwSaveActions;
    private DevExpress.XtraGrid.Columns.GridColumn colSaProc;
    private DevExpress.XtraGrid.Columns.GridColumn colSaScope;
    private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cboEditScope;
    private DevExpress.XtraGrid.Columns.GridColumn colSaSourceSlot;
    private DevExpress.XtraGrid.Columns.GridColumn colSaKeyParam;
    private GridControlWyn grdSaveActionParams;
    private GridViewWyn gvwSaveActionParams;
    private DevExpress.XtraGrid.Columns.GridColumn colSapName;
    private DevExpress.XtraGrid.Columns.GridColumn colSapSqlType;
    private DevExpress.XtraGrid.Columns.GridColumn colSapMatchedColumn;

    private PanelWyn paTitle;
    private PanelWyn panelWyn1;
    private LookUpEditWyn cboModule;
    private PanelWyn panelWyn4;
    private ButtonWyn btnDeleteRow2;
    private ButtonWyn btnRowAdd2;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn5;
    private ButtonWyn btnDeleteRow1;
    private ButtonWyn btnRowAdd1;
    private PanelWyn panelWyn9;
    private PanelWyn panelWyn8;
    private PanelWyn panelWyn7;
    private SplitterWyn splitterWyn1;
    private PanelWyn panelWyn10;
    private PanelWyn panelWyn11;
    private SplitterWyn splitterWyn4;
    private PanelWyn panelWyn12;
    private SectionHeaderWyn sectionHeaderWyn1;
    private SplitterWyn splitterWyn2;
    private SplitterWyn splitterWyn3;
    private LookUpColumnEdit lookUpControlKind;
}
