// VS 디자이너가 자동 생성하는 필드 선언에는 = null!을 안 붙여서 nullable 경고(CS8618)가
// 계속 나기 때문에, 이 파일(디자이너 전용)만 nullable 검사를 끈다 - 흔한 관례(frmAIBuilder/
// frmAutoKey와 동일).
#nullable disable
using WYNLAB.Base.Controls;

namespace WYNLAB.SYS;

/// <summary>
/// 프로시저 빌더 - 화면 배치(위치/크기)만 이 파일이 맡는다. 실제 조회/생성 로직과 grid
/// column의 Visible 토글(조회/저장 전환)은 frmProcBuilder.cs 참고. 2026-09-14에 코드-only
/// 방식(BuildLayout 직접 호출)에서 이 방식(디자이너 지원)으로 바꿨다 - Visual Studio 폼
/// 디자이너를 열어 컨트롤을 마우스로 직접 옮기고 싶다는 요청.
/// </summary>
public partial class frmProcBuilder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProcBuilder));
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.cboType = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblModule = new DevExpress.XtraEditors.LabelControl();
            this.cboModule = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblType = new DevExpress.XtraEditors.LabelControl();
            this.lblProcName = new DevExpress.XtraEditors.LabelControl();
            this.txtProcName = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtMasterTable = new WYNLAB.Base.Controls.TextEditWyn();
            this.btnDescribeMaster = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnSelectAllMaster = new WYNLAB.Base.Controls.ButtonWyn();
            this.grdMasterColumns = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvwMasterColumns = new WYNLAB.Base.Controls.GridViewWyn();
            this.colMasterColumnNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMasterSqlType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMasterIsPrimaryKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkMaster = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colMasterIsSelected = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMasterIsWhereFilter = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMasterIncludeInsert = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMasterIncludeUpdate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtDetailTable = new WYNLAB.Base.Controls.TextEditWyn();
            this.btnDescribeDetail = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnSelectAllDetail = new WYNLAB.Base.Controls.ButtonWyn();
            this.lblLinkColumn = new DevExpress.XtraEditors.LabelControl();
            this.cboLinkColumn = new DevExpress.XtraEditors.ComboBoxEdit();
            this.panBody = new System.Windows.Forms.Panel();
            this.panelWyn13 = new WYNLAB.Base.Controls.PanelWyn();
            this.memoPreview = new WYNLAB.Base.Controls.MemoEditWyn();
            this.panelWyn12 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnGenerate = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnCreateProc = new WYNLAB.Base.Controls.ButtonWyn();
            this.panelWyn11 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.grdDetailColumns = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvwDetailColumns = new WYNLAB.Base.Controls.GridViewWyn();
            this.colDetailColumnNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetailSqlType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetailIsPrimaryKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkDetail = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colDetailIsSelected = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn9 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn10 = new WYNLAB.Base.Controls.PanelWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.splitterWyn2 = new WYNLAB.Base.Controls.SplitterWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboModule.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProcName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMasterTable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMasterColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwMasterColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailTable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLinkColumn.Properties)).BeginInit();
            this.panBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn13)).BeginInit();
            this.panelWyn13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoPreview.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn12)).BeginInit();
            this.panelWyn12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn11)).BeginInit();
            this.panelWyn11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetailColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwDetailColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).BeginInit();
            this.paTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
            this.panelWyn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
            this.panelWyn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
            this.panelWyn4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
            this.panelWyn6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).BeginInit();
            this.panelWyn8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn9)).BeginInit();
            this.panelWyn9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn10)).BeginInit();
            this.panelWyn10.SuspendLayout();
            this.SuspendLayout();
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(240)))), ((int)(((byte)(242)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.Controls.Add(this.cboType);
            this.panHeader.Controls.Add(this.lblModule);
            this.panHeader.Controls.Add(this.cboModule);
            this.panHeader.Controls.Add(this.lblType);
            this.panHeader.Controls.Add(this.lblProcName);
            this.panHeader.Controls.Add(this.txtProcName);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(0, 33);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(960, 103);
            this.panHeader.TabIndex = 0;
            // 
            // cboType
            // 
            this.cboType.EditValue = "";
            this.cboType.Location = new System.Drawing.Point(254, 9);
            this.cboType.LookupKey = "L_SM0007";
            this.cboType.Name = "cboType";
            this.cboType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboType.Properties.NullText = "";
            this.cboType.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cboType.Size = new System.Drawing.Size(172, 20);
            this.cboType.TabIndex = 8;
            // 
            // lblModule
            // 
            this.lblModule.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModule.Appearance.Options.UseFont = true;
            this.lblModule.Location = new System.Drawing.Point(12, 12);
            this.lblModule.Name = "lblModule";
            this.lblModule.Size = new System.Drawing.Size(41, 15);
            this.lblModule.TabIndex = 0;
            this.lblModule.Text = "Module";
            // 
            // cboModule
            // 
            this.cboModule.EditValue = "";
            this.cboModule.Location = new System.Drawing.Point(59, 9);
            this.cboModule.Name = "cboModule";
            this.cboModule.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboModule.Properties.NullText = "";
            this.cboModule.Size = new System.Drawing.Size(100, 20);
            this.cboModule.TabIndex = 1;
            // 
            // lblType
            // 
            this.lblType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblType.Appearance.Options.UseFont = true;
            this.lblType.Location = new System.Drawing.Point(165, 12);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(83, 15);
            this.lblType.TabIndex = 4;
            this.lblType.Text = "Procedure Type";
            // 
            // lblProcName
            // 
            this.lblProcName.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcName.Appearance.Options.UseFont = true;
            this.lblProcName.Location = new System.Drawing.Point(432, 14);
            this.lblProcName.Name = "lblProcName";
            this.lblProcName.Size = new System.Drawing.Size(90, 15);
            this.lblProcName.TabIndex = 6;
            this.lblProcName.Text = "Procedure Name";
            // 
            // txtProcName
            // 
            this.txtProcName.Location = new System.Drawing.Point(528, 11);
            this.txtProcName.Name = "txtProcName";
            this.txtProcName.Size = new System.Drawing.Size(214, 20);
            this.txtProcName.TabIndex = 7;
            // 
            // txtMasterTable
            // 
            this.txtMasterTable.Location = new System.Drawing.Point(5, 5);
            this.txtMasterTable.Name = "txtMasterTable";
            this.txtMasterTable.Size = new System.Drawing.Size(179, 20);
            this.txtMasterTable.TabIndex = 9;
            // 
            // btnDescribeMaster
            // 
            this.btnDescribeMaster.BackColor = System.Drawing.Color.Transparent;
            this.btnDescribeMaster.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnDescribeMaster.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDescribeMaster.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnDescribeMaster.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDescribeMaster.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDescribeMaster.Image = null;
            this.btnDescribeMaster.Location = new System.Drawing.Point(190, 3);
            this.btnDescribeMaster.Name = "btnDescribeMaster";
            this.btnDescribeMaster.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDescribeMaster.Size = new System.Drawing.Size(110, 24);
            this.btnDescribeMaster.TabIndex = 10;
            this.btnDescribeMaster.Text = "컬럼 가져오기";
            this.btnDescribeMaster.ToolTip = null;
            // 
            // btnSelectAllMaster
            // 
            this.btnSelectAllMaster.BackColor = System.Drawing.Color.Transparent;
            this.btnSelectAllMaster.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnSelectAllMaster.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectAllMaster.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnSelectAllMaster.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnSelectAllMaster.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnSelectAllMaster.Image = null;
            this.btnSelectAllMaster.Location = new System.Drawing.Point(305, 3);
            this.btnSelectAllMaster.Name = "btnSelectAllMaster";
            this.btnSelectAllMaster.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnSelectAllMaster.Size = new System.Drawing.Size(95, 24);
            this.btnSelectAllMaster.TabIndex = 20;
            this.btnSelectAllMaster.Text = "전체선택";
            this.btnSelectAllMaster.ToolTip = null;
            // 
            // grdMasterColumns
            // 
            this.grdMasterColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMasterColumns.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdMasterColumns.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdMasterColumns.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdMasterColumns.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdMasterColumns.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdMasterColumns.Location = new System.Drawing.Point(0, 61);
            this.grdMasterColumns.MainView = this.gvwMasterColumns;
            this.grdMasterColumns.Name = "grdMasterColumns";
            this.grdMasterColumns.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.chkMaster});
            this.grdMasterColumns.Size = new System.Drawing.Size(410, 319);
            this.grdMasterColumns.TabIndex = 11;
            this.grdMasterColumns.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvwMasterColumns});
            // 
            // gvwMasterColumns
            // 
            this.gvwMasterColumns.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colMasterColumnNm,
            this.colMasterSqlType,
            this.colMasterIsPrimaryKey,
            this.colMasterIsSelected,
            this.colMasterIsWhereFilter,
            this.colMasterIncludeInsert,
            this.colMasterIncludeUpdate});
            this.gvwMasterColumns.EmptyText = "컬럼을 아직 안 가져왔습니다.";
            this.gvwMasterColumns.GridControl = this.grdMasterColumns;
            this.gvwMasterColumns.Name = "gvwMasterColumns";
            this.gvwMasterColumns.OptionsBehavior.Editable = false;
            this.gvwMasterColumns.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvwMasterColumns.OptionsView.ShowGroupPanel = false;
            // 
            // colMasterColumnNm
            // 
            this.colMasterColumnNm.Caption = "컬럼명";
            this.colMasterColumnNm.FieldName = "ColumnNm";
            this.colMasterColumnNm.Name = "colMasterColumnNm";
            this.colMasterColumnNm.OptionsColumn.AllowEdit = false;
            this.colMasterColumnNm.Visible = true;
            this.colMasterColumnNm.VisibleIndex = 0;
            // 
            // colMasterSqlType
            // 
            this.colMasterSqlType.Caption = "타입";
            this.colMasterSqlType.FieldName = "SqlType";
            this.colMasterSqlType.Name = "colMasterSqlType";
            this.colMasterSqlType.OptionsColumn.AllowEdit = false;
            this.colMasterSqlType.Visible = true;
            this.colMasterSqlType.VisibleIndex = 1;
            this.colMasterSqlType.Width = 90;
            // 
            // colMasterIsPrimaryKey
            // 
            this.colMasterIsPrimaryKey.Caption = "PK";
            this.colMasterIsPrimaryKey.ColumnEdit = this.chkMaster;
            this.colMasterIsPrimaryKey.FieldName = "IsPrimaryKey";
            this.colMasterIsPrimaryKey.Name = "colMasterIsPrimaryKey";
            this.colMasterIsPrimaryKey.OptionsColumn.AllowEdit = false;
            this.colMasterIsPrimaryKey.Visible = true;
            this.colMasterIsPrimaryKey.VisibleIndex = 2;
            this.colMasterIsPrimaryKey.Width = 40;
            // 
            // chkMaster
            // 
            this.chkMaster.Name = "chkMaster";
            // 
            // colMasterIsSelected
            // 
            this.colMasterIsSelected.Caption = "선택";
            this.colMasterIsSelected.ColumnEdit = this.chkMaster;
            this.colMasterIsSelected.FieldName = "IsSelected";
            this.colMasterIsSelected.Name = "colMasterIsSelected";
            this.colMasterIsSelected.Visible = true;
            this.colMasterIsSelected.VisibleIndex = 3;
            this.colMasterIsSelected.Width = 50;
            // 
            // colMasterIsWhereFilter
            // 
            this.colMasterIsWhereFilter.Caption = "WHERE 필터";
            this.colMasterIsWhereFilter.ColumnEdit = this.chkMaster;
            this.colMasterIsWhereFilter.FieldName = "IsWhereFilter";
            this.colMasterIsWhereFilter.Name = "colMasterIsWhereFilter";
            this.colMasterIsWhereFilter.Visible = true;
            this.colMasterIsWhereFilter.VisibleIndex = 4;
            this.colMasterIsWhereFilter.Width = 80;
            // 
            // colMasterIncludeInsert
            // 
            this.colMasterIncludeInsert.Caption = "Insert";
            this.colMasterIncludeInsert.ColumnEdit = this.chkMaster;
            this.colMasterIncludeInsert.FieldName = "IncludeInsert";
            this.colMasterIncludeInsert.Name = "colMasterIncludeInsert";
            this.colMasterIncludeInsert.Visible = true;
            this.colMasterIncludeInsert.VisibleIndex = 5;
            this.colMasterIncludeInsert.Width = 60;
            // 
            // colMasterIncludeUpdate
            // 
            this.colMasterIncludeUpdate.Caption = "Update";
            this.colMasterIncludeUpdate.ColumnEdit = this.chkMaster;
            this.colMasterIncludeUpdate.FieldName = "IncludeUpdate";
            this.colMasterIncludeUpdate.Name = "colMasterIncludeUpdate";
            this.colMasterIncludeUpdate.Visible = true;
            this.colMasterIncludeUpdate.VisibleIndex = 6;
            this.colMasterIncludeUpdate.Width = 60;
            // 
            // txtDetailTable
            // 
            this.txtDetailTable.Location = new System.Drawing.Point(5, 7);
            this.txtDetailTable.Name = "txtDetailTable";
            this.txtDetailTable.Size = new System.Drawing.Size(179, 20);
            this.txtDetailTable.TabIndex = 14;
            // 
            // btnDescribeDetail
            // 
            this.btnDescribeDetail.BackColor = System.Drawing.Color.Transparent;
            this.btnDescribeDetail.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnDescribeDetail.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDescribeDetail.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnDescribeDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDescribeDetail.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDescribeDetail.Image = null;
            this.btnDescribeDetail.Location = new System.Drawing.Point(190, 4);
            this.btnDescribeDetail.Name = "btnDescribeDetail";
            this.btnDescribeDetail.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDescribeDetail.Size = new System.Drawing.Size(107, 24);
            this.btnDescribeDetail.TabIndex = 15;
            this.btnDescribeDetail.Text = "컬럼 가져오기";
            this.btnDescribeDetail.ToolTip = null;
            // 
            // btnSelectAllDetail
            // 
            this.btnSelectAllDetail.BackColor = System.Drawing.Color.Transparent;
            this.btnSelectAllDetail.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnSelectAllDetail.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectAllDetail.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnSelectAllDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnSelectAllDetail.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnSelectAllDetail.Image = null;
            this.btnSelectAllDetail.Location = new System.Drawing.Point(300, 4);
            this.btnSelectAllDetail.Name = "btnSelectAllDetail";
            this.btnSelectAllDetail.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnSelectAllDetail.Size = new System.Drawing.Size(95, 24);
            this.btnSelectAllDetail.TabIndex = 21;
            this.btnSelectAllDetail.Text = "전체선택";
            this.btnSelectAllDetail.ToolTip = null;
            // 
            // lblLinkColumn
            // 
            this.lblLinkColumn.Location = new System.Drawing.Point(32, 38);
            this.lblLinkColumn.Name = "lblLinkColumn";
            this.lblLinkColumn.Size = new System.Drawing.Size(234, 14);
            this.lblLinkColumn.TabIndex = 16;
            this.lblLinkColumn.Text = "연결 컬럼(상세 테이블에서 마스터 키를 담는 컬럼)";
            this.lblLinkColumn.Visible = false;
            // 
            // cboLinkColumn
            // 
            this.cboLinkColumn.Location = new System.Drawing.Point(32, 56);
            this.cboLinkColumn.Name = "cboLinkColumn";
            this.cboLinkColumn.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboLinkColumn.Size = new System.Drawing.Size(200, 20);
            this.cboLinkColumn.TabIndex = 17;
            this.cboLinkColumn.Visible = false;
            // 
            // panBody
            // 
            this.panBody.Controls.Add(this.panelWyn13);
            this.panBody.Controls.Add(this.panelWyn12);
            this.panBody.Controls.Add(this.panelWyn11);
            this.panBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBody.Location = new System.Drawing.Point(418, 136);
            this.panBody.Name = "panBody";
            this.panBody.Size = new System.Drawing.Size(542, 664);
            this.panBody.TabIndex = 1;
            // 
            // panelWyn13
            // 
            this.panelWyn13.Controls.Add(this.memoPreview);
            this.panelWyn13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn13.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn13.Location = new System.Drawing.Point(0, 62);
            this.panelWyn13.Name = "panelWyn13";
            this.panelWyn13.Padding = new System.Windows.Forms.Padding(5);
            this.panelWyn13.Size = new System.Drawing.Size(542, 602);
            this.panelWyn13.TabIndex = 15;
            // 
            // memoPreview
            // 
            this.memoPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoPreview.Location = new System.Drawing.Point(5, 5);
            this.memoPreview.Name = "memoPreview";
            this.memoPreview.Size = new System.Drawing.Size(532, 592);
            this.memoPreview.TabIndex = 3;
            // 
            // panelWyn12
            // 
            this.panelWyn12.Controls.Add(this.btnGenerate);
            this.panelWyn12.Controls.Add(this.btnCreateProc);
            this.panelWyn12.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn12.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn12.Location = new System.Drawing.Point(0, 27);
            this.panelWyn12.Name = "panelWyn12";
            this.panelWyn12.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn12.Size = new System.Drawing.Size(542, 35);
            this.panelWyn12.TabIndex = 14;
            // 
            // btnGenerate
            // 
            this.btnGenerate.BackColor = System.Drawing.Color.Transparent;
            this.btnGenerate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnGenerate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerate.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnGenerate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnGenerate.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnGenerate.Image = null;
            this.btnGenerate.Location = new System.Drawing.Point(4, 6);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnGenerate.Size = new System.Drawing.Size(120, 24);
            this.btnGenerate.TabIndex = 1;
            this.btnGenerate.Text = "스크립트 생성";
            this.btnGenerate.ToolTip = null;
            // 
            // btnCreateProc
            // 
            this.btnCreateProc.BackColor = System.Drawing.Color.Transparent;
            this.btnCreateProc.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnCreateProc.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCreateProc.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnCreateProc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnCreateProc.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnCreateProc.Image = null;
            this.btnCreateProc.Location = new System.Drawing.Point(127, 5);
            this.btnCreateProc.Name = "btnCreateProc";
            this.btnCreateProc.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnCreateProc.Size = new System.Drawing.Size(100, 24);
            this.btnCreateProc.TabIndex = 2;
            this.btnCreateProc.Text = "DB 반영";
            this.btnCreateProc.ToolTip = null;
            // 
            // panelWyn11
            // 
            this.panelWyn11.Controls.Add(this.sectionHeaderWyn3);
            this.panelWyn11.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn11.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn11.Location = new System.Drawing.Point(0, 0);
            this.panelWyn11.Name = "panelWyn11";
            this.panelWyn11.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn11.Size = new System.Drawing.Size(542, 27);
            this.panelWyn11.TabIndex = 13;
            // 
            // sectionHeaderWyn3
            // 
            this.sectionHeaderWyn3.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn3.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn3.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn3.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn3.Name = "sectionHeaderWyn3";
            this.sectionHeaderWyn3.Size = new System.Drawing.Size(537, 25);
            this.sectionHeaderWyn3.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn3.SvgIcon")));
            this.sectionHeaderWyn3.TabIndex = 8;
            this.sectionHeaderWyn3.Text = "Procedure Script";
            // 
            // grdDetailColumns
            // 
            this.grdDetailColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDetailColumns.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdDetailColumns.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdDetailColumns.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdDetailColumns.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdDetailColumns.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdDetailColumns.Location = new System.Drawing.Point(0, 75);
            this.grdDetailColumns.MainView = this.gvwDetailColumns;
            this.grdDetailColumns.Name = "grdDetailColumns";
            this.grdDetailColumns.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.chkDetail});
            this.grdDetailColumns.Size = new System.Drawing.Size(410, 203);
            this.grdDetailColumns.TabIndex = 0;
            this.grdDetailColumns.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvwDetailColumns});
            // 
            // gvwDetailColumns
            // 
            this.gvwDetailColumns.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDetailColumnNm,
            this.colDetailSqlType,
            this.colDetailIsPrimaryKey,
            this.colDetailIsSelected});
            this.gvwDetailColumns.EmptyText = "컬럼을 아직 안 가져왔습니다.";
            this.gvwDetailColumns.GridControl = this.grdDetailColumns;
            this.gvwDetailColumns.Name = "gvwDetailColumns";
            this.gvwDetailColumns.OptionsBehavior.Editable = false;
            this.gvwDetailColumns.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvwDetailColumns.OptionsView.ShowGroupPanel = false;
            // 
            // colDetailColumnNm
            // 
            this.colDetailColumnNm.Caption = "컬럼명";
            this.colDetailColumnNm.FieldName = "ColumnNm";
            this.colDetailColumnNm.Name = "colDetailColumnNm";
            this.colDetailColumnNm.OptionsColumn.AllowEdit = false;
            this.colDetailColumnNm.Visible = true;
            this.colDetailColumnNm.VisibleIndex = 0;
            // 
            // colDetailSqlType
            // 
            this.colDetailSqlType.Caption = "타입";
            this.colDetailSqlType.FieldName = "SqlType";
            this.colDetailSqlType.Name = "colDetailSqlType";
            this.colDetailSqlType.OptionsColumn.AllowEdit = false;
            this.colDetailSqlType.Visible = true;
            this.colDetailSqlType.VisibleIndex = 1;
            this.colDetailSqlType.Width = 90;
            // 
            // colDetailIsPrimaryKey
            // 
            this.colDetailIsPrimaryKey.Caption = "PK";
            this.colDetailIsPrimaryKey.ColumnEdit = this.chkDetail;
            this.colDetailIsPrimaryKey.FieldName = "IsPrimaryKey";
            this.colDetailIsPrimaryKey.Name = "colDetailIsPrimaryKey";
            this.colDetailIsPrimaryKey.OptionsColumn.AllowEdit = false;
            this.colDetailIsPrimaryKey.Visible = true;
            this.colDetailIsPrimaryKey.VisibleIndex = 2;
            this.colDetailIsPrimaryKey.Width = 40;
            // 
            // chkDetail
            // 
            this.chkDetail.Name = "chkDetail";
            // 
            // colDetailIsSelected
            // 
            this.colDetailIsSelected.Caption = "선택";
            this.colDetailIsSelected.ColumnEdit = this.chkDetail;
            this.colDetailIsSelected.FieldName = "IsSelected";
            this.colDetailIsSelected.Name = "colDetailIsSelected";
            this.colDetailIsSelected.Visible = true;
            this.colDetailIsSelected.VisibleIndex = 3;
            this.colDetailIsSelected.Width = 50;
            // 
            // panelWyn1
            // 
            this.panelWyn1.Controls.Add(this.panHeader);
            this.panelWyn1.Controls.Add(this.paTitle);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(0, 0);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Size = new System.Drawing.Size(960, 136);
            this.panelWyn1.TabIndex = 7;
            // 
            // paTitle
            // 
            this.paTitle.Controls.Add(this.sectionHeaderWyn1);
            this.paTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle.Location = new System.Drawing.Point(0, 0);
            this.paTitle.Name = "paTitle";
            this.paTitle.Size = new System.Drawing.Size(960, 33);
            this.paTitle.TabIndex = 7;
            // 
            // sectionHeaderWyn1
            // 
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(0, 0);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(960, 33);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 8;
            this.sectionHeaderWyn1.Text = "LookUp관리 [SYS_LOOKUP]";
            // 
            // panelWyn2
            // 
            this.panelWyn2.Controls.Add(this.grdMasterColumns);
            this.panelWyn2.Controls.Add(this.panelWyn3);
            this.panelWyn2.Controls.Add(this.panelWyn4);
            this.panelWyn2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn2.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn2.Location = new System.Drawing.Point(0, 0);
            this.panelWyn2.Name = "panelWyn2";
            this.panelWyn2.Size = new System.Drawing.Size(410, 380);
            this.panelWyn2.TabIndex = 8;
            // 
            // panelWyn3
            // 
            this.panelWyn3.Controls.Add(this.panelWyn5);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(0, 27);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.panelWyn3.Size = new System.Drawing.Size(410, 34);
            this.panelWyn3.TabIndex = 0;
            // 
            // panelWyn5
            // 
            this.panelWyn5.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panelWyn5.Controls.Add(this.txtMasterTable);
            this.panelWyn5.Controls.Add(this.btnDescribeMaster);
            this.panelWyn5.Controls.Add(this.btnSelectAllMaster);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(0, 2);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Size = new System.Drawing.Size(410, 30);
            this.panelWyn5.TabIndex = 13;
            // 
            // panelWyn4
            // 
            this.panelWyn4.Controls.Add(this.sectionHeaderWyn4);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(0, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn4.Size = new System.Drawing.Size(410, 27);
            this.panelWyn4.TabIndex = 12;
            // 
            // sectionHeaderWyn4
            // 
            this.sectionHeaderWyn4.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn4.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn4.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn4.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn4.Name = "sectionHeaderWyn4";
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(405, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 8;
            this.sectionHeaderWyn4.Text = "Master Table";
            // 
            // panelWyn6
            // 
            this.panelWyn6.Controls.Add(this.grdDetailColumns);
            this.panelWyn6.Controls.Add(this.panelWyn7);
            this.panelWyn6.Controls.Add(this.panelWyn9);
            this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn6.Location = new System.Drawing.Point(0, 386);
            this.panelWyn6.Name = "panelWyn6";
            this.panelWyn6.Size = new System.Drawing.Size(410, 278);
            this.panelWyn6.TabIndex = 8;
            // 
            // panelWyn7
            // 
            this.panelWyn7.Controls.Add(this.panelWyn8);
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(0, 27);
            this.panelWyn7.Name = "panelWyn7";
            this.panelWyn7.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.panelWyn7.Size = new System.Drawing.Size(410, 48);
            this.panelWyn7.TabIndex = 0;
            // 
            // panelWyn8
            // 
            this.panelWyn8.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panelWyn8.Controls.Add(this.txtDetailTable);
            this.panelWyn8.Controls.Add(this.btnDescribeDetail);
            this.panelWyn8.Controls.Add(this.btnSelectAllDetail);
            this.panelWyn8.Controls.Add(this.lblLinkColumn);
            this.panelWyn8.Controls.Add(this.cboLinkColumn);
            this.panelWyn8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn8.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn8.Location = new System.Drawing.Point(0, 2);
            this.panelWyn8.Name = "panelWyn8";
            this.panelWyn8.Size = new System.Drawing.Size(410, 44);
            this.panelWyn8.TabIndex = 13;
            // 
            // panelWyn9
            // 
            this.panelWyn9.Controls.Add(this.sectionHeaderWyn2);
            this.panelWyn9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn9.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn9.Location = new System.Drawing.Point(0, 0);
            this.panelWyn9.Name = "panelWyn9";
            this.panelWyn9.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn9.Size = new System.Drawing.Size(410, 27);
            this.panelWyn9.TabIndex = 12;
            // 
            // sectionHeaderWyn2
            // 
            this.sectionHeaderWyn2.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn2.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn2.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn2.Name = "sectionHeaderWyn2";
            this.sectionHeaderWyn2.Size = new System.Drawing.Size(405, 25);
            this.sectionHeaderWyn2.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn2.SvgIcon")));
            this.sectionHeaderWyn2.TabIndex = 8;
            this.sectionHeaderWyn2.Text = "Detail Table";
            // 
            // panelWyn10
            // 
            this.panelWyn10.Controls.Add(this.panelWyn6);
            this.panelWyn10.Controls.Add(this.splitterWyn1);
            this.panelWyn10.Controls.Add(this.panelWyn2);
            this.panelWyn10.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelWyn10.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn10.Location = new System.Drawing.Point(0, 136);
            this.panelWyn10.Name = "panelWyn10";
            this.panelWyn10.Size = new System.Drawing.Size(410, 664);
            this.panelWyn10.TabIndex = 9;
            // 
            // splitterWyn1
            // 
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitterWyn1.Location = new System.Drawing.Point(0, 380);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(410, 6);
            this.splitterWyn1.TabIndex = 9;
            this.splitterWyn1.TabStop = false;
            // 
            // splitterWyn2
            // 
            this.splitterWyn2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn2.Location = new System.Drawing.Point(410, 136);
            this.splitterWyn2.Name = "splitterWyn2";
            this.splitterWyn2.Size = new System.Drawing.Size(8, 664);
            this.splitterWyn2.TabIndex = 10;
            this.splitterWyn2.TabStop = false;
            // 
            // frmProcBuilder
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(960, 800);
            this.Controls.Add(this.panBody);
            this.Controls.Add(this.splitterWyn2);
            this.Controls.Add(this.panelWyn10);
            this.Controls.Add(this.panelWyn1);
            this.Name = "frmProcBuilder";
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboModule.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProcName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMasterTable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMasterColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwMasterColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailTable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLinkColumn.Properties)).EndInit();
            this.panBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn13)).EndInit();
            this.panelWyn13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoPreview.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn12)).EndInit();
            this.panelWyn12.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn11)).EndInit();
            this.panelWyn11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDetailColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwDetailColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
            this.paTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
            this.panelWyn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
            this.panelWyn6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
            this.panelWyn7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).EndInit();
            this.panelWyn8.ResumeLayout(false);
            this.panelWyn8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn9)).EndInit();
            this.panelWyn9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn10)).EndInit();
            this.panelWyn10.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblModule;
    private LookUpEditWyn cboModule;
    private DevExpress.XtraEditors.LabelControl lblType;
    private DevExpress.XtraEditors.LabelControl lblProcName;
    private TextEditWyn txtProcName;
    private TextEditWyn txtMasterTable;
    private ButtonWyn btnDescribeMaster;
    private ButtonWyn btnSelectAllMaster;
    private GridControlWyn grdMasterColumns;
    private GridViewWyn gvwMasterColumns;
    private DevExpress.XtraGrid.Columns.GridColumn colMasterColumnNm;
    private DevExpress.XtraGrid.Columns.GridColumn colMasterSqlType;
    private DevExpress.XtraGrid.Columns.GridColumn colMasterIsPrimaryKey;
    private DevExpress.XtraGrid.Columns.GridColumn colMasterIsSelected;
    private DevExpress.XtraGrid.Columns.GridColumn colMasterIsWhereFilter;
    private DevExpress.XtraGrid.Columns.GridColumn colMasterIncludeInsert;
    private DevExpress.XtraGrid.Columns.GridColumn colMasterIncludeUpdate;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkMaster;
    private TextEditWyn txtDetailTable;
    private ButtonWyn btnDescribeDetail;
    private ButtonWyn btnSelectAllDetail;
    private DevExpress.XtraEditors.LabelControl lblLinkColumn;
    private DevExpress.XtraEditors.ComboBoxEdit cboLinkColumn;
    private System.Windows.Forms.Panel panBody;
    private GridControlWyn grdDetailColumns;
    private GridViewWyn gvwDetailColumns;
    private DevExpress.XtraGrid.Columns.GridColumn colDetailColumnNm;
    private DevExpress.XtraGrid.Columns.GridColumn colDetailSqlType;
    private DevExpress.XtraGrid.Columns.GridColumn colDetailIsPrimaryKey;
    private DevExpress.XtraGrid.Columns.GridColumn colDetailIsSelected;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkDetail;
    private ButtonWyn btnGenerate;
    private ButtonWyn btnCreateProc;
    private MemoEditWyn memoPreview;
    private PanelWyn panelWyn1;
    private PanelWyn paTitle;
    private SectionHeaderWyn sectionHeaderWyn1;
    private PanelWyn panelWyn2;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn5;
    private PanelWyn panelWyn4;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn panelWyn6;
    private PanelWyn panelWyn7;
    private PanelWyn panelWyn8;
    private PanelWyn panelWyn9;
    private SectionHeaderWyn sectionHeaderWyn2;
    private PanelWyn panelWyn10;
    private SplitterWyn splitterWyn1;
    private SplitterWyn splitterWyn2;
    private LookUpEditWyn cboType;
    private PanelWyn panelWyn13;
    private PanelWyn panelWyn12;
    private PanelWyn panelWyn11;
    private SectionHeaderWyn sectionHeaderWyn3;
}
