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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
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
            this.panQueryToolbar = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDescribeQueryMulti = new WYNLAB.Base.Controls.ButtonWyn();
            this.grdQueryResultSets = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvwQueryResultSets = new WYNLAB.Base.Controls.GridViewWyn();
            this.colQrProc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQrWorkType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQrIndex = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQrTargetSlot = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdColumnPreview2 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvwColumnPreview2 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colCp2Name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCp2SqlType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCp2Caption = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCp2Include = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkEditPreview = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colCp2IsKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCp2ControlKind = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboEditControlKind = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.colCp2LookupKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tabSaveMulti = new DevExpress.XtraTab.XtraTabPage();
            this.panSaveToolbar = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDescribeSaveMulti = new WYNLAB.Base.Controls.ButtonWyn();
            this.grdSaveActions = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvwSaveActions = new WYNLAB.Base.Controls.GridViewWyn();
            this.colSaProc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSaScope = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboEditScope = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.colSaSourceSlot = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSaKeyParam = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdSaveActionParams = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvwSaveActionParams = new WYNLAB.Base.Controls.GridViewWyn();
            this.colSapName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSapSqlType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSapMatchedColumn = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.lblTemplateKind = new DevExpress.XtraEditors.LabelControl();
            this.cboTemplateKind = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.lblQueryProc = new DevExpress.XtraEditors.LabelControl();
            this.txtQueryProc = new WYNLAB.Base.Controls.TextEditWyn();
            this.btnDescribeQuery = new WYNLAB.Base.Controls.ButtonWyn();
            this.lblDetailWorkType = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailWorkType = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailKeyParam = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailKeyParam = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblMasterKeyColumn = new DevExpress.XtraEditors.LabelControl();
            this.txtMasterKeyColumn = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSaveProc = new DevExpress.XtraEditors.LabelControl();
            this.txtSaveProc = new WYNLAB.Base.Controls.TextEditWyn();
            this.btnDescribeSave = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnGenerate = new WYNLAB.Base.Controls.ButtonWyn();
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
            ((System.ComponentModel.ISupportInitialize)(this.grdQueryResultSets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwQueryResultSets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdColumnPreview2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwColumnPreview2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEditPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboEditControlKind)).BeginInit();
            this.tabSaveMulti.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSaveActions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSaveActions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboEditScope)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSaveActionParams)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSaveActionParams)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelForm)).BeginInit();
            this.panelForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboModule.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtScreenClassNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMenuCaption.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpperMenuCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpperMenuNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProcPrefix.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTemplateKind.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQueryProc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailWorkType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailKeyParam.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMasterKeyColumn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaveProc.Properties)).BeginInit();
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
            this.panBase.Size = new System.Drawing.Size(1161, 547);
            this.panBase.TabIndex = 0;
            // 
            // panelPreview
            // 
            this.panelPreview.Controls.Add(this.panelWyn1);
            this.panelPreview.Controls.Add(this.panelForm);
            this.panelPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPreview.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelPreview.Location = new System.Drawing.Point(0, 0);
            this.panelPreview.Name = "panelPreview";
            this.panelPreview.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.panelPreview.Size = new System.Drawing.Size(1161, 547);
            this.panelPreview.TabIndex = 1;
            // 
            // panelWyn1
            // 
            this.panelWyn1.Controls.Add(this.tabPreview);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(5, 161);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.panelWyn1.Size = new System.Drawing.Size(1151, 386);
            this.panelWyn1.TabIndex = 1;
            // 
            // tabPreview
            // 
            this.tabPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPreview.Location = new System.Drawing.Point(0, 5);
            this.tabPreview.Name = "tabPreview";
            this.tabPreview.Size = new System.Drawing.Size(1151, 381);
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
            this.tabSaveParams.Size = new System.Drawing.Size(1149, 355);
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
            this.grdSaveParams.Size = new System.Drawing.Size(1149, 355);
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
            // 
            // colSpName
            // 
            this.colSpName.Caption = "Param";
            this.colSpName.FieldName = "Name";
            this.colSpName.Name = "colSpName";
            this.colSpName.OptionsColumn.AllowEdit = false;
            this.colSpName.Visible = true;
            this.colSpName.VisibleIndex = 0;
            this.colSpName.Width = 150;
            // 
            // colSpSqlType
            // 
            this.colSpSqlType.Caption = "SQL Type";
            this.colSpSqlType.FieldName = "SqlType";
            this.colSpSqlType.Name = "colSpSqlType";
            this.colSpSqlType.OptionsColumn.AllowEdit = false;
            this.colSpSqlType.Visible = true;
            this.colSpSqlType.VisibleIndex = 1;
            this.colSpSqlType.Width = 90;
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
            // chkEditPreview / cboEditControlKind - grdColumnPreview2(레코드셋 컬럼 미리보기)의
            // In Grid/Key 체크박스, Control 콤보 편집기. 원래는 삭제된 Master/Sub Columns 탭의
            // 그리드와도 공유했으나 그 탭들이 없어지면서 이제 grdColumnPreview2 전용이다.
            //
            this.chkEditPreview.Name = "chkEditPreview";
            //
            this.cboEditControlKind.Items.AddRange(new object[] {
            "TEXT",
            "CHECK",
            "NUMBER",
            "COMBO"});
            this.cboEditControlKind.Name = "cboEditControlKind";
            this.cboEditControlKind.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            //
            // tabQueryMulti
            //
            this.tabQueryMulti.Controls.Add(this.grdColumnPreview2);
            this.tabQueryMulti.Controls.Add(this.grdQueryResultSets);
            this.tabQueryMulti.Controls.Add(this.panQueryToolbar);
            this.tabQueryMulti.Name = "tabQueryMulti";
            this.tabQueryMulti.Size = new System.Drawing.Size(1149, 355);
            this.tabQueryMulti.Text = "Query Sources (다중)";
            //
            // panQueryToolbar
            //
            this.panQueryToolbar.Controls.Add(this.btnDescribeQueryMulti);
            this.panQueryToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panQueryToolbar.Location = new System.Drawing.Point(0, 0);
            this.panQueryToolbar.Name = "panQueryToolbar";
            this.panQueryToolbar.Size = new System.Drawing.Size(1149, 37);
            this.panQueryToolbar.TabIndex = 2;
            //
            // btnDescribeQueryMulti
            //
            this.btnDescribeQueryMulti.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnDescribeQueryMulti.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDescribeQueryMulti.FillColor = System.Drawing.Color.White;
            this.btnDescribeQueryMulti.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDescribeQueryMulti.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDescribeQueryMulti.Image = null;
            this.btnDescribeQueryMulti.Location = new System.Drawing.Point(8, 5);
            this.btnDescribeQueryMulti.Name = "btnDescribeQueryMulti";
            this.btnDescribeQueryMulti.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDescribeQueryMulti.Size = new System.Drawing.Size(160, 27);
            this.btnDescribeQueryMulti.TabIndex = 1;
            this.btnDescribeQueryMulti.Text = "레코드셋 조회";
            this.btnDescribeQueryMulti.ToolTip = "선택 행(Proc/WorkType) 레코드셋 조회 - 결과셋 수만큼 행이 자동으로 채워집니다";
            //
            // grdQueryResultSets
            //
            this.grdQueryResultSets.Dock = System.Windows.Forms.DockStyle.Top;
            this.grdQueryResultSets.Height = 160;
            this.grdQueryResultSets.Location = new System.Drawing.Point(0, 37);
            this.grdQueryResultSets.MainView = this.gvwQueryResultSets;
            this.grdQueryResultSets.Name = "grdQueryResultSets";
            this.grdQueryResultSets.Size = new System.Drawing.Size(1149, 160);
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
            //
            // colQrProc
            //
            this.colQrProc.Caption = "Proc";
            this.colQrProc.FieldName = "ProcName";
            this.colQrProc.Name = "colQrProc";
            this.colQrProc.Visible = true;
            this.colQrProc.VisibleIndex = 0;
            this.colQrProc.Width = 220;
            //
            // colQrWorkType
            //
            this.colQrWorkType.Caption = "WorkType";
            this.colQrWorkType.FieldName = "WorkType";
            this.colQrWorkType.Name = "colQrWorkType";
            this.colQrWorkType.Visible = true;
            this.colQrWorkType.VisibleIndex = 1;
            this.colQrWorkType.Width = 90;
            //
            // colQrIndex
            //
            this.colQrIndex.Caption = "Index";
            this.colQrIndex.FieldName = "ResultSetIndex";
            this.colQrIndex.Name = "colQrIndex";
            this.colQrIndex.OptionsColumn.AllowEdit = false;
            this.colQrIndex.Visible = true;
            this.colQrIndex.VisibleIndex = 2;
            this.colQrIndex.Width = 50;
            //
            // colQrTargetSlot
            //
            this.colQrTargetSlot.Caption = "Target Slot";
            this.colQrTargetSlot.FieldName = "TargetSlot";
            this.colQrTargetSlot.Name = "colQrTargetSlot";
            this.colQrTargetSlot.Visible = true;
            this.colQrTargetSlot.VisibleIndex = 3;
            this.colQrTargetSlot.Width = 220;
            //
            // grdColumnPreview2
            //
            this.grdColumnPreview2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdColumnPreview2.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdColumnPreview2.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdColumnPreview2.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdColumnPreview2.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdColumnPreview2.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdColumnPreview2.Location = new System.Drawing.Point(0, 189);
            this.grdColumnPreview2.MainView = this.gvwColumnPreview2;
            this.grdColumnPreview2.Name = "grdColumnPreview2";
            this.grdColumnPreview2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.chkEditPreview,
            this.cboEditControlKind});
            this.grdColumnPreview2.Size = new System.Drawing.Size(1149, 166);
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
            this.colCp2LookupKey});
            this.gvwColumnPreview2.GridControl = this.grdColumnPreview2;
            this.gvwColumnPreview2.Name = "gvwColumnPreview2";
            this.gvwColumnPreview2.OptionsBehavior.Editable = false;
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
            this.colCp2SqlType.Caption = "SQL Type";
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
            this.colCp2ControlKind.ColumnEdit = this.cboEditControlKind;
            this.colCp2ControlKind.FieldName = "ControlKind";
            this.colCp2ControlKind.Name = "colCp2ControlKind";
            this.colCp2ControlKind.Visible = true;
            this.colCp2ControlKind.VisibleIndex = 5;
            this.colCp2ControlKind.Width = 100;
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
            // tabSaveMulti
            //
            this.tabSaveMulti.Controls.Add(this.grdSaveActionParams);
            this.tabSaveMulti.Controls.Add(this.grdSaveActions);
            this.tabSaveMulti.Controls.Add(this.panSaveToolbar);
            this.tabSaveMulti.Name = "tabSaveMulti";
            this.tabSaveMulti.Size = new System.Drawing.Size(1149, 355);
            this.tabSaveMulti.Text = "Save Actions (다중)";
            //
            // panSaveToolbar
            //
            this.panSaveToolbar.Controls.Add(this.btnDescribeSaveMulti);
            this.panSaveToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSaveToolbar.Location = new System.Drawing.Point(0, 0);
            this.panSaveToolbar.Name = "panSaveToolbar";
            this.panSaveToolbar.Size = new System.Drawing.Size(1149, 37);
            this.panSaveToolbar.TabIndex = 2;
            //
            // btnDescribeSaveMulti
            //
            this.btnDescribeSaveMulti.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnDescribeSaveMulti.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDescribeSaveMulti.FillColor = System.Drawing.Color.White;
            this.btnDescribeSaveMulti.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDescribeSaveMulti.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDescribeSaveMulti.Image = null;
            this.btnDescribeSaveMulti.Location = new System.Drawing.Point(8, 5);
            this.btnDescribeSaveMulti.Name = "btnDescribeSaveMulti";
            this.btnDescribeSaveMulti.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDescribeSaveMulti.Size = new System.Drawing.Size(160, 27);
            this.btnDescribeSaveMulti.TabIndex = 1;
            this.btnDescribeSaveMulti.Text = "파라미터 조회/매칭";
            this.btnDescribeSaveMulti.ToolTip = "선택 행(Proc) 파라미터 조회/매칭";
            //
            // grdSaveActions
            //
            this.grdSaveActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grdSaveActions.Height = 160;
            this.grdSaveActions.Location = new System.Drawing.Point(0, 37);
            this.grdSaveActions.MainView = this.gvwSaveActions;
            this.grdSaveActions.Name = "grdSaveActions";
            this.grdSaveActions.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cboEditScope});
            this.grdSaveActions.Size = new System.Drawing.Size(1149, 160);
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
            //
            // colSaProc
            //
            this.colSaProc.Caption = "Proc";
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
            this.colSaSourceSlot.Caption = "Source Slot";
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
            // grdSaveActionParams
            //
            this.grdSaveActionParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSaveActionParams.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdSaveActionParams.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdSaveActionParams.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdSaveActionParams.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdSaveActionParams.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdSaveActionParams.Location = new System.Drawing.Point(0, 189);
            this.grdSaveActionParams.MainView = this.gvwSaveActionParams;
            this.grdSaveActionParams.Name = "grdSaveActionParams";
            this.grdSaveActionParams.Size = new System.Drawing.Size(1149, 166);
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
            this.colSapSqlType.Caption = "SQL Type";
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
            this.panelForm.Controls.Add(this.lblTemplateKind);
            this.panelForm.Controls.Add(this.cboTemplateKind);
            this.panelForm.Controls.Add(this.lblQueryProc);
            this.panelForm.Controls.Add(this.txtQueryProc);
            this.panelForm.Controls.Add(this.btnDescribeQuery);
            this.panelForm.Controls.Add(this.lblDetailWorkType);
            this.panelForm.Controls.Add(this.txtDetailWorkType);
            this.panelForm.Controls.Add(this.lblDetailKeyParam);
            this.panelForm.Controls.Add(this.txtDetailKeyParam);
            this.panelForm.Controls.Add(this.lblMasterKeyColumn);
            this.panelForm.Controls.Add(this.txtMasterKeyColumn);
            this.panelForm.Controls.Add(this.lblSaveProc);
            this.panelForm.Controls.Add(this.txtSaveProc);
            this.panelForm.Controls.Add(this.btnDescribeSave);
            this.panelForm.Controls.Add(this.btnGenerate);
            this.panelForm.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelForm.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelForm.Location = new System.Drawing.Point(5, 0);
            this.panelForm.Name = "panelForm";
            this.panelForm.Size = new System.Drawing.Size(1151, 195);
            this.panelForm.TabIndex = 0;
            // 
            // cboModule
            // 
            this.cboModule.EditValue = "";
            this.cboModule.Location = new System.Drawing.Point(82, 13);
            this.cboModule.LookupKey = "L_SM0003";
            this.cboModule.Name = "cboModule";
            this.cboModule.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboModule.Properties.NullText = "";
            this.cboModule.Size = new System.Drawing.Size(150, 20);
            this.cboModule.TabIndex = 26;
            // 
            // lblModule
            // 
            this.lblModule.Location = new System.Drawing.Point(37, 16);
            this.lblModule.Name = "lblModule";
            this.lblModule.Size = new System.Drawing.Size(39, 14);
            this.lblModule.TabIndex = 0;
            this.lblModule.Text = "Module";
            // 
            // lblScreenClassNm
            // 
            this.lblScreenClassNm.Location = new System.Drawing.Point(245, 16);
            this.lblScreenClassNm.Name = "lblScreenClassNm";
            this.lblScreenClassNm.Size = new System.Drawing.Size(62, 14);
            this.lblScreenClassNm.TabIndex = 2;
            this.lblScreenClassNm.Text = "Form Name";
            // 
            // txtScreenClassNm
            // 
            this.txtScreenClassNm.Location = new System.Drawing.Point(313, 13);
            this.txtScreenClassNm.Name = "txtScreenClassNm";
            this.txtScreenClassNm.Size = new System.Drawing.Size(150, 20);
            this.txtScreenClassNm.TabIndex = 3;
            // 
            // lblMenuCaption
            // 
            this.lblMenuCaption.Location = new System.Drawing.Point(470, 16);
            this.lblMenuCaption.Name = "lblMenuCaption";
            this.lblMenuCaption.Size = new System.Drawing.Size(75, 14);
            this.lblMenuCaption.TabIndex = 4;
            this.lblMenuCaption.Text = "Menu Caption";
            // 
            // txtMenuCaption
            // 
            this.txtMenuCaption.Location = new System.Drawing.Point(553, 13);
            this.txtMenuCaption.Name = "txtMenuCaption";
            this.txtMenuCaption.Size = new System.Drawing.Size(220, 20);
            this.txtMenuCaption.TabIndex = 5;
            // 
            // lblUpperMenuCd
            // 
            this.lblUpperMenuCd.Location = new System.Drawing.Point(9, 53);
            this.lblUpperMenuCd.Name = "lblUpperMenuCd";
            this.lblUpperMenuCd.Size = new System.Drawing.Size(67, 14);
            this.lblUpperMenuCd.TabIndex = 6;
            this.lblUpperMenuCd.Text = "Upper Menu";
            // 
            // txtUpperMenuCd
            // 
            this.txtUpperMenuCd.Location = new System.Drawing.Point(481, 253);
            this.txtUpperMenuCd.Name = "txtUpperMenuCd";
            this.txtUpperMenuCd.Properties.ReadOnly = true;
            this.txtUpperMenuCd.Size = new System.Drawing.Size(100, 20);
            this.txtUpperMenuCd.TabIndex = 7;
            this.txtUpperMenuCd.Visible = false;
            // 
            // txtUpperMenuNm
            // 
            this.txtUpperMenuNm.Location = new System.Drawing.Point(82, 50);
            this.txtUpperMenuNm.Name = "txtUpperMenuNm";
            this.txtUpperMenuNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", 24, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.txtUpperMenuNm.Size = new System.Drawing.Size(150, 20);
            this.txtUpperMenuNm.TabIndex = 8;
            this.txtUpperMenuNm.ToolTip = null;
            // 
            // lblProcPrefix
            // 
            this.lblProcPrefix.Location = new System.Drawing.Point(249, 53);
            this.lblProcPrefix.Name = "lblProcPrefix";
            this.lblProcPrefix.Size = new System.Drawing.Size(58, 14);
            this.lblProcPrefix.TabIndex = 9;
            this.lblProcPrefix.Text = "Proc Prefix";
            // 
            // txtProcPrefix
            // 
            this.txtProcPrefix.Location = new System.Drawing.Point(313, 50);
            this.txtProcPrefix.Name = "txtProcPrefix";
            this.txtProcPrefix.Size = new System.Drawing.Size(150, 20);
            this.txtProcPrefix.TabIndex = 10;
            // 
            // lblTemplateKind
            // 
            this.lblTemplateKind.Location = new System.Drawing.Point(493, 53);
            this.lblTemplateKind.Name = "lblTemplateKind";
            this.lblTemplateKind.Size = new System.Drawing.Size(52, 14);
            this.lblTemplateKind.TabIndex = 11;
            this.lblTemplateKind.Text = "Template";
            //
            // cboTemplateKind
            //
            this.cboTemplateKind.Location = new System.Drawing.Point(553, 50);
            this.cboTemplateKind.Name = "cboTemplateKind";
            this.cboTemplateKind.Properties.NullText = "";
            this.cboTemplateKind.Size = new System.Drawing.Size(220, 24);
            this.cboTemplateKind.TabIndex = 12;
            // 
            // lblQueryProc
            // 
            this.lblQueryProc.Location = new System.Drawing.Point(15, 95);
            this.lblQueryProc.Name = "lblQueryProc";
            this.lblQueryProc.Size = new System.Drawing.Size(61, 14);
            this.lblQueryProc.TabIndex = 13;
            this.lblQueryProc.Text = "Query Proc";
            // 
            // txtQueryProc
            // 
            this.txtQueryProc.Location = new System.Drawing.Point(82, 92);
            this.txtQueryProc.Name = "txtQueryProc";
            this.txtQueryProc.Size = new System.Drawing.Size(263, 20);
            this.txtQueryProc.TabIndex = 14;
            // 
            // btnDescribeQuery
            // 
            this.btnDescribeQuery.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnDescribeQuery.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDescribeQuery.FillColor = System.Drawing.Color.White;
            this.btnDescribeQuery.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDescribeQuery.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDescribeQuery.Image = null;
            this.btnDescribeQuery.Location = new System.Drawing.Point(351, 86);
            this.btnDescribeQuery.Name = "btnDescribeQuery";
            this.btnDescribeQuery.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDescribeQuery.Size = new System.Drawing.Size(112, 29);
            this.btnDescribeQuery.TabIndex = 15;
            this.btnDescribeQuery.Text = "Describe Query";
            this.btnDescribeQuery.ToolTip = null;
            //
            // lblDetailWorkType
            //
            this.lblDetailWorkType.Location = new System.Drawing.Point(15, 160);
            this.lblDetailWorkType.Name = "lblDetailWorkType";
            this.lblDetailWorkType.Size = new System.Drawing.Size(91, 14);
            this.lblDetailWorkType.TabIndex = 16;
            this.lblDetailWorkType.Text = "Detail WorkType";
            //
            // txtDetailWorkType
            //
            this.txtDetailWorkType.EditValue = "Q1";
            this.txtDetailWorkType.Location = new System.Drawing.Point(115, 157);
            this.txtDetailWorkType.Name = "txtDetailWorkType";
            this.txtDetailWorkType.Size = new System.Drawing.Size(80, 20);
            this.txtDetailWorkType.TabIndex = 17;
            //
            // lblDetailKeyParam
            //
            this.lblDetailKeyParam.Location = new System.Drawing.Point(215, 160);
            this.lblDetailKeyParam.Name = "lblDetailKeyParam";
            this.lblDetailKeyParam.Size = new System.Drawing.Size(91, 14);
            this.lblDetailKeyParam.TabIndex = 18;
            this.lblDetailKeyParam.Text = "Detail Key Param";
            //
            // txtDetailKeyParam
            //
            this.txtDetailKeyParam.Location = new System.Drawing.Point(315, 157);
            this.txtDetailKeyParam.Name = "txtDetailKeyParam";
            this.txtDetailKeyParam.Size = new System.Drawing.Size(150, 20);
            this.txtDetailKeyParam.TabIndex = 19;
            //
            // lblMasterKeyColumn
            //
            this.lblMasterKeyColumn.Location = new System.Drawing.Point(478, 160);
            this.lblMasterKeyColumn.Name = "lblMasterKeyColumn";
            this.lblMasterKeyColumn.Size = new System.Drawing.Size(104, 14);
            this.lblMasterKeyColumn.TabIndex = 20;
            this.lblMasterKeyColumn.Text = "Master Key Column";
            //
            // txtMasterKeyColumn
            //
            this.txtMasterKeyColumn.Location = new System.Drawing.Point(588, 157);
            this.txtMasterKeyColumn.Name = "txtMasterKeyColumn";
            this.txtMasterKeyColumn.Size = new System.Drawing.Size(150, 20);
            this.txtMasterKeyColumn.TabIndex = 21;
            // 
            // lblSaveProc
            // 
            this.lblSaveProc.Location = new System.Drawing.Point(22, 129);
            this.lblSaveProc.Name = "lblSaveProc";
            this.lblSaveProc.Size = new System.Drawing.Size(54, 14);
            this.lblSaveProc.TabIndex = 22;
            this.lblSaveProc.Text = "Save Proc";
            // 
            // txtSaveProc
            // 
            this.txtSaveProc.Location = new System.Drawing.Point(82, 126);
            this.txtSaveProc.Name = "txtSaveProc";
            this.txtSaveProc.Size = new System.Drawing.Size(263, 20);
            this.txtSaveProc.TabIndex = 23;
            // 
            // btnDescribeSave
            // 
            this.btnDescribeSave.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnDescribeSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDescribeSave.FillColor = System.Drawing.Color.White;
            this.btnDescribeSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDescribeSave.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDescribeSave.Image = null;
            this.btnDescribeSave.Location = new System.Drawing.Point(351, 120);
            this.btnDescribeSave.Name = "btnDescribeSave";
            this.btnDescribeSave.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDescribeSave.Size = new System.Drawing.Size(112, 29);
            this.btnDescribeSave.TabIndex = 24;
            this.btnDescribeSave.Text = "Describe Save";
            this.btnDescribeSave.ToolTip = null;
            // 
            // btnGenerate
            // 
            this.btnGenerate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnGenerate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerate.FillColor = System.Drawing.Color.White;
            this.btnGenerate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnGenerate.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnGenerate.Image = null;
            this.btnGenerate.Location = new System.Drawing.Point(553, 86);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnGenerate.Size = new System.Drawing.Size(220, 63);
            this.btnGenerate.TabIndex = 25;
            this.btnGenerate.Text = "Create Form";
            this.btnGenerate.ToolTip = null;
            // 
            // paTitle
            // 
            this.paTitle.Controls.Add(this.sectionHeaderWyn1);
            this.paTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle.Location = new System.Drawing.Point(0, 0);
            this.paTitle.Name = "paTitle";
            this.paTitle.Size = new System.Drawing.Size(1161, 33);
            this.paTitle.TabIndex = 6;
            // 
            // sectionHeaderWyn1
            // 
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(5, 6);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(209, 23);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 8;
            this.sectionHeaderWyn1.Text = "AI Builder [frmAIBuilder]";
            // 
            // frmAIBuilder
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1161, 580);
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
            ((System.ComponentModel.ISupportInitialize)(this.grdQueryResultSets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwQueryResultSets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdColumnPreview2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwColumnPreview2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEditPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboEditControlKind)).EndInit();
            this.tabSaveMulti.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSaveActions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSaveActions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboEditScope)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSaveActionParams)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSaveActionParams)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelForm)).EndInit();
            this.panelForm.ResumeLayout(false);
            this.panelForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboModule.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtScreenClassNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMenuCaption.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpperMenuCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpperMenuNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProcPrefix.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTemplateKind.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQueryProc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailWorkType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailKeyParam.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMasterKeyColumn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaveProc.Properties)).EndInit();
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
    private DevExpress.XtraEditors.LabelControl lblQueryProc;
    private TextEditWyn txtQueryProc;
    private ButtonWyn btnDescribeQuery;
    private DevExpress.XtraEditors.LabelControl lblDetailWorkType;
    private TextEditWyn txtDetailWorkType;
    private DevExpress.XtraEditors.LabelControl lblDetailKeyParam;
    private TextEditWyn txtDetailKeyParam;
    private DevExpress.XtraEditors.LabelControl lblMasterKeyColumn;
    private TextEditWyn txtMasterKeyColumn;
    private DevExpress.XtraEditors.LabelControl lblSaveProc;
    private TextEditWyn txtSaveProc;
    private ButtonWyn btnDescribeSave;
    private ButtonWyn btnGenerate;

    private PanelWyn panelPreview;
    private TabControlWyn tabPreview;

    private DevExpress.XtraTab.XtraTabPage tabSaveParams;
    private GridControlWyn grdSaveParams;
    private GridViewWyn gvwSaveParams;
    private DevExpress.XtraGrid.Columns.GridColumn colSpName;
    private DevExpress.XtraGrid.Columns.GridColumn colSpSqlType;
    private DevExpress.XtraGrid.Columns.GridColumn colSpMatchedColumn;

    // 다중 쿼리소스/저장액션(MasterFormTabGrid류) - 몇 개든 행으로 추가하면 되고, 그리드 자체는
    // 항상 이 2개 탭 고정이다(템플릿에 그리드/저장프로시저가 늘어나도 여기는 안 바뀜).
    private DevExpress.XtraTab.XtraTabPage tabQueryMulti;
    private PanelWyn panQueryToolbar;
    private ButtonWyn btnDescribeQueryMulti;
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
    private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cboEditControlKind;
    private DevExpress.XtraGrid.Columns.GridColumn colCp2LookupKey;

    private DevExpress.XtraTab.XtraTabPage tabSaveMulti;
    private PanelWyn panSaveToolbar;
    private ButtonWyn btnDescribeSaveMulti;
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
    private SectionHeaderWyn sectionHeaderWyn1;
    private PanelWyn panelWyn1;
    private LookUpEditWyn cboModule;
}
