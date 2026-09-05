// VS 디자이너가 자동 생성하는 필드 선언에는 = null!을 안 붙여서 nullable 경고(CS8618)가
// 계속 나기 때문에, 이 파일(디자이너 전용)만 nullable 검사를 끈다 - 흔한 관례. 이 프로젝트를
// 복사해서 새 화면을 만들 때도 그 화면의 Designer.cs 맨 위에 이 줄을 그대로 유지할 것.
#nullable disable
using DevExpress.XtraGrid;
using WYNLAB.Base.Controls;

namespace WYNLAB.SYS;

/// <summary>
/// 새 화면 개발용 템플릿 - frmMinorCode(기초코드등록)에서 표준 레이아웃(제목바 + 검색패널 +
/// 좌우 스플리터 + 목록그리드/상세패널/하위그리드)만 남기고 그 화면 전용 컬럼/입력컨트롤은
/// 전부 지운 것. 사용법:
///   1. 이 프로젝트 폴더(WYNLAB.SYS)를 새 화면 프로젝트로 복사
///   2. 복사한 폴더 안에서 frmSysPopup.cs / .Designer.cs / .resx 파일명을 새 화면명으로
///      바꾸고, 그 안의 클래스명(frmSysPopup)·네임스페이스(WYNLAB.SYS)도 새
///      화면명/WYNLAB.SM으로 바꾼다 - 반드시 파일을 새 이름으로 바꾼 "다음에" 클래스명을
///      바꿀 것(먼저 rename 리팩터부터 쓰면 원본과 이름이 겹쳐 컴파일 에러가 난다 - 실제로
///      겪음, frmUserAuth 만들 때 이 순서를 안 지켜서 1142개 에러가 났었다).
///   3. grd1(목록)/panData(상세 입력)/grd2(하위 목록) 자리에 디자이너로 컬럼/컨트롤을 배치
///   4. 완성되면 이 WYNLAB.SYS 프로젝트를 WYNLAB.SM.sln에서 빼고 실제 화면
///      프로젝트를 솔루션에 추가(또는 WYNLAB.SM 모듈 프로젝트 밑 폴더로 옮겨 넣기)
/// </summary>
public partial class frmSysPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSysPopup));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colColumnNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCaption = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colControlType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLookupProcNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSort = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWidth = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVisibleYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBoxControlType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemCheckEditVisible = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDeletRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAddRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.grd3 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw3 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colParamNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSearchCaption = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSearchControlType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSearchSort = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSearchWidth = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBoxSearchControlType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.panelWyn9 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn10 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDeletRow3 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAddRow3 = new WYNLAB.Base.Controls.ButtonWyn();
            this.sectionHeaderWyn5 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colPopupKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPopupNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProcNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colHierarchical = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.txtSearchQ = new WYNLAB.Base.Controls.TextEditWyn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblPopupKey = new DevExpress.XtraEditors.LabelControl();
            this.txtPopupKey = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblPopupNm = new DevExpress.XtraEditors.LabelControl();
            this.txtPopupNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblProcNm = new DevExpress.XtraEditors.LabelControl();
            this.txtProcNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.btnGenerateColumns = new WYNLAB.Base.Controls.ButtonWyn();
            this.lblHierarchical = new DevExpress.XtraEditors.LabelControl();
            this.chkHierarchical = new WYNLAB.Base.Controls.CheckBoxWyn();
            this.lblKeyField = new DevExpress.XtraEditors.LabelControl();
            this.txtKeyField = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblParentField = new DevExpress.XtraEditors.LabelControl();
            this.txtParentField = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDisplayField = new DevExpress.XtraEditors.LabelControl();
            this.txtDisplayField = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblUseYn = new DevExpress.XtraEditors.LabelControl();
            this.chkUseYn = new WYNLAB.Base.Controls.CheckBoxWyn();
            this.lblPopupWidth = new DevExpress.XtraEditors.LabelControl();
            this.txtPopupWidth = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblPopupHeight = new DevExpress.XtraEditors.LabelControl();
            this.txtPopupHeight = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblRemark = new DevExpress.XtraEditors.LabelControl();
            this.txtRemark = new WYNLAB.Base.Controls.TextEditWyn();
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
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxControlType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEditVisible)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxSearchControlType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn9)).BeginInit();
            this.panelWyn9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn10)).BeginInit();
            this.panelWyn10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPopupKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPopupNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProcNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkHierarchical.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKeyField.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParentField.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDisplayField.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUseYn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPopupWidth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPopupHeight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchQ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).BeginInit();
            this.paTitle.SuspendLayout();
            this.SuspendLayout();
            //
            // panBase
            //
            this.panBase.Appearance.BackColor = System.Drawing.Color.White;
            this.panBase.Controls.Add(this.panelWyn3);
            this.panBase.Controls.Add(this.panHeader);
            this.panBase.Controls.Add(this.paTitle);
            this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBase.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panBase.Location = new System.Drawing.Point(0, 0);
            this.panBase.Name = "panBase";
            this.panBase.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.panBase.Size = new System.Drawing.Size(1165, 600);
            this.panBase.TabIndex = 5;
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
            this.panelWyn3.Size = new System.Drawing.Size(1159, 515);
            this.panelWyn3.TabIndex = 7;
            //
            // panelWyn4
            //
            this.panelWyn4.Controls.Add(this.grd2);
            this.panelWyn4.Controls.Add(this.panelWyn1);
            this.panelWyn4.Controls.Add(this.grd3);
            this.panelWyn4.Controls.Add(this.panelWyn9);
            this.panelWyn4.Controls.Add(this.panelWyn5);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(412, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelWyn4.Size = new System.Drawing.Size(747, 515);
            this.panelWyn4.TabIndex = 7;
            //
            // grd2 (컬럼 설정 - sysPopUpD 한 행 = 팝업 그리드/트리의 컬럼 하나)
            //
            this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd2.Location = new System.Drawing.Point(3, 245);
            this.grd2.MainView = this.gvw2;
            this.grd2.Name = "grd2";
            this.grd2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBoxControlType,
            this.repositoryItemCheckEditVisible});
            this.grd2.Size = new System.Drawing.Size(744, 270);
            this.grd2.TabIndex = 7;
            this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw2});
            //
            // gvw2
            //
            this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colColumnNm,
            this.colCaption,
            this.colControlType,
            this.colLookupProcNm,
            this.colSort,
            this.colWidth,
            this.colVisibleYn});
            this.gvw2.GridControl = this.grd2;
            this.gvw2.HighlightFocusedRow = true;
            this.gvw2.Name = "gvw2";
            this.gvw2.OptionsView.ColumnAutoWidth = false;
            this.gvw2.OptionsView.ShowGroupPanel = false;
            //
            // colColumnNm
            //
            this.colColumnNm.Caption = "컬럼명";
            this.colColumnNm.FieldName = "column_nm";
            this.colColumnNm.Name = "colColumnNm";
            this.colColumnNm.OptionsColumn.AllowEdit = false;
            this.colColumnNm.Visible = true;
            this.colColumnNm.VisibleIndex = 0;
            this.colColumnNm.Width = 120;
            //
            // colCaption
            //
            this.colCaption.Caption = "캡션";
            this.colCaption.FieldName = "caption";
            this.colCaption.Name = "colCaption";
            this.colCaption.Visible = true;
            this.colCaption.VisibleIndex = 1;
            this.colCaption.Width = 100;
            //
            // colControlType
            //
            this.colControlType.Caption = "컨트롤타입";
            this.colControlType.ColumnEdit = this.repositoryItemComboBoxControlType;
            this.colControlType.FieldName = "control_type";
            this.colControlType.Name = "colControlType";
            this.colControlType.Visible = true;
            this.colControlType.VisibleIndex = 2;
            this.colControlType.Width = 90;
            //
            // colLookupProcNm
            //
            this.colLookupProcNm.Caption = "Lookup 프로시저";
            this.colLookupProcNm.FieldName = "lookup_proc_nm";
            this.colLookupProcNm.Name = "colLookupProcNm";
            this.colLookupProcNm.Visible = true;
            this.colLookupProcNm.VisibleIndex = 3;
            this.colLookupProcNm.Width = 140;
            //
            // colSort
            //
            this.colSort.Caption = "순서";
            this.colSort.FieldName = "sort";
            this.colSort.Name = "colSort";
            this.colSort.Visible = true;
            this.colSort.VisibleIndex = 4;
            this.colSort.Width = 50;
            //
            // colWidth
            //
            this.colWidth.Caption = "폭(px)";
            this.colWidth.FieldName = "width";
            this.colWidth.Name = "colWidth";
            this.colWidth.Visible = true;
            this.colWidth.VisibleIndex = 5;
            this.colWidth.Width = 60;
            //
            // colVisibleYn
            //
            this.colVisibleYn.Caption = "표시";
            this.colVisibleYn.ColumnEdit = this.repositoryItemCheckEditVisible;
            this.colVisibleYn.FieldName = "visible_yn";
            this.colVisibleYn.Name = "colVisibleYn";
            this.colVisibleYn.Visible = true;
            this.colVisibleYn.VisibleIndex = 6;
            this.colVisibleYn.Width = 50;
            //
            // repositoryItemComboBoxControlType
            //
            this.repositoryItemComboBoxControlType.AutoHeight = false;
            this.repositoryItemComboBoxControlType.Items.AddRange(new object[] {
            "TEXT",
            "DATE",
            "LOOKUP",
            "CHECK"});
            this.repositoryItemComboBoxControlType.Name = "repositoryItemComboBoxControlType";
            this.repositoryItemComboBoxControlType.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            //
            // repositoryItemCheckEditVisible
            //
            this.repositoryItemCheckEditVisible.AutoHeight = false;
            this.repositoryItemCheckEditVisible.Name = "repositoryItemCheckEditVisible";
            this.repositoryItemCheckEditVisible.ValueChecked = "Y";
            this.repositoryItemCheckEditVisible.ValueUnchecked = "N";
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
            this.panelWyn1.Size = new System.Drawing.Size(744, 27);
            this.panelWyn1.TabIndex = 8;
            //
            // panelWyn7
            //
            this.panelWyn7.Controls.Add(this.btnDeletRow2);
            this.panelWyn7.Controls.Add(this.btnAddRow2);
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(676, 0);
            this.panelWyn7.Name = "panelWyn7";
            this.panelWyn7.Size = new System.Drawing.Size(68, 25);
            this.panelWyn7.TabIndex = 9;
            //
            // btnDeletRow2
            //
            this.btnDeletRow2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeletRow2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(236)))), ((int)(((byte)(234)))));
            this.btnDeletRow2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(198)))), ((int)(((byte)(193)))));
            this.btnDeletRow2.Image = ((System.Drawing.Image)(resources.GetObject("btnDeletRow2.ImageOptions.Image")));
            this.btnDeletRow2.Location = new System.Drawing.Point(42, 2);
            this.btnDeletRow2.Name = "btnDeletRow2";
            this.btnDeletRow2.Size = new System.Drawing.Size(24, 22);
            this.btnDeletRow2.TabIndex = 0;
            this.btnDeletRow2.Text = "";
            this.btnDeletRow2.ToolTip = "행삭제";
            this.btnDeletRow2.Click += new System.EventHandler(this.btnDeletRow2_Click);
            //
            // btnAddRow2
            //
            this.btnAddRow2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddRow2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(245)))), ((int)(((byte)(231)))));
            this.btnAddRow2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(231)))), ((int)(((byte)(203)))));
            this.btnAddRow2.Image = ((System.Drawing.Image)(resources.GetObject("btnAddRow2.ImageOptions.Image")));
            this.btnAddRow2.Location = new System.Drawing.Point(10, 2);
            this.btnAddRow2.Name = "btnAddRow2";
            this.btnAddRow2.Size = new System.Drawing.Size(24, 22);
            this.btnAddRow2.TabIndex = 0;
            this.btnAddRow2.Text = "";
            this.btnAddRow2.ToolTip = "행추가";
            this.btnAddRow2.Click += new System.EventHandler(this.btnAddRow2_Click);
            //
            // sectionHeaderWyn2
            //
            this.sectionHeaderWyn2.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn2.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn2.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn2.Name = "sectionHeaderWyn2";
            this.sectionHeaderWyn2.Size = new System.Drawing.Size(739, 25);
            this.sectionHeaderWyn2.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn2.SvgIcon")));
            this.sectionHeaderWyn2.TabIndex = 8;
            this.sectionHeaderWyn2.Text = "컬럼 설정";
            //
            // grd3 (조회조건 - sysPopUpS 한 행 = 팝업 검색줄의 입력조건 하나. 프로시저마다
            // 파라미터명/개수가 전부 달라서 컬럼설정(grd2)과 별개로 관리한다.)
            //
            this.grd3.Dock = System.Windows.Forms.DockStyle.Top;
            this.grd3.Location = new System.Drawing.Point(3, 68);
            this.grd3.MainView = this.gvw3;
            this.grd3.Name = "grd3";
            this.grd3.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBoxSearchControlType});
            this.grd3.Size = new System.Drawing.Size(744, 150);
            this.grd3.TabIndex = 11;
            this.grd3.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw3});
            //
            // gvw3
            //
            this.gvw3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colParamNm,
            this.colSearchCaption,
            this.colSearchControlType,
            this.colSearchSort,
            this.colSearchWidth});
            this.gvw3.GridControl = this.grd3;
            this.gvw3.HighlightFocusedRow = true;
            this.gvw3.Name = "gvw3";
            this.gvw3.OptionsView.ColumnAutoWidth = false;
            this.gvw3.OptionsView.ShowGroupPanel = false;
            //
            // colParamNm
            //
            this.colParamNm.Caption = "파라미터명";
            this.colParamNm.FieldName = "param_nm";
            this.colParamNm.Name = "colParamNm";
            this.colParamNm.OptionsColumn.AllowEdit = false;
            this.colParamNm.Visible = true;
            this.colParamNm.VisibleIndex = 0;
            this.colParamNm.Width = 110;
            //
            // colSearchCaption
            //
            this.colSearchCaption.Caption = "라벨";
            this.colSearchCaption.FieldName = "caption";
            this.colSearchCaption.Name = "colSearchCaption";
            this.colSearchCaption.Visible = true;
            this.colSearchCaption.VisibleIndex = 1;
            this.colSearchCaption.Width = 140;
            //
            // colSearchControlType
            //
            this.colSearchControlType.Caption = "컨트롤타입";
            this.colSearchControlType.ColumnEdit = this.repositoryItemComboBoxSearchControlType;
            this.colSearchControlType.FieldName = "control_type";
            this.colSearchControlType.Name = "colSearchControlType";
            this.colSearchControlType.Visible = true;
            this.colSearchControlType.VisibleIndex = 2;
            this.colSearchControlType.Width = 90;
            //
            // colSearchSort
            //
            this.colSearchSort.Caption = "순서";
            this.colSearchSort.FieldName = "sort";
            this.colSearchSort.Name = "colSearchSort";
            this.colSearchSort.Visible = true;
            this.colSearchSort.VisibleIndex = 3;
            this.colSearchSort.Width = 50;
            //
            // colSearchWidth
            //
            this.colSearchWidth.Caption = "입력창 폭(px)";
            this.colSearchWidth.FieldName = "width";
            this.colSearchWidth.Name = "colSearchWidth";
            this.colSearchWidth.Visible = true;
            this.colSearchWidth.VisibleIndex = 4;
            this.colSearchWidth.Width = 80;
            //
            // repositoryItemComboBoxSearchControlType
            //
            this.repositoryItemComboBoxSearchControlType.AutoHeight = false;
            this.repositoryItemComboBoxSearchControlType.Items.AddRange(new object[] {
            "TEXT",
            "DATE"});
            this.repositoryItemComboBoxSearchControlType.Name = "repositoryItemComboBoxSearchControlType";
            this.repositoryItemComboBoxSearchControlType.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            //
            // panelWyn9
            //
            this.panelWyn9.Controls.Add(this.panelWyn10);
            this.panelWyn9.Controls.Add(this.sectionHeaderWyn5);
            this.panelWyn9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn9.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn9.Location = new System.Drawing.Point(3, 41);
            this.panelWyn9.Name = "panelWyn9";
            this.panelWyn9.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn9.Size = new System.Drawing.Size(744, 27);
            this.panelWyn9.TabIndex = 12;
            //
            // panelWyn10
            //
            this.panelWyn10.Controls.Add(this.btnDeletRow3);
            this.panelWyn10.Controls.Add(this.btnAddRow3);
            this.panelWyn10.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelWyn10.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn10.Location = new System.Drawing.Point(676, 0);
            this.panelWyn10.Name = "panelWyn10";
            this.panelWyn10.Size = new System.Drawing.Size(68, 25);
            this.panelWyn10.TabIndex = 13;
            //
            // btnDeletRow3
            //
            this.btnDeletRow3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeletRow3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(236)))), ((int)(((byte)(234)))));
            this.btnDeletRow3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(198)))), ((int)(((byte)(193)))));
            this.btnDeletRow3.Image = ((System.Drawing.Image)(resources.GetObject("btnDeletRow2.ImageOptions.Image")));
            this.btnDeletRow3.Location = new System.Drawing.Point(42, 2);
            this.btnDeletRow3.Name = "btnDeletRow3";
            this.btnDeletRow3.Size = new System.Drawing.Size(24, 22);
            this.btnDeletRow3.TabIndex = 0;
            this.btnDeletRow3.Text = "";
            this.btnDeletRow3.ToolTip = "행삭제";
            this.btnDeletRow3.Click += new System.EventHandler(this.btnDeletRow3_Click);
            //
            // btnAddRow3
            //
            this.btnAddRow3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddRow3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(245)))), ((int)(((byte)(231)))));
            this.btnAddRow3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(231)))), ((int)(((byte)(203)))));
            this.btnAddRow3.Image = ((System.Drawing.Image)(resources.GetObject("btnAddRow2.ImageOptions.Image")));
            this.btnAddRow3.Location = new System.Drawing.Point(10, 2);
            this.btnAddRow3.Name = "btnAddRow3";
            this.btnAddRow3.Size = new System.Drawing.Size(24, 22);
            this.btnAddRow3.TabIndex = 0;
            this.btnAddRow3.Text = "";
            this.btnAddRow3.ToolTip = "행추가";
            this.btnAddRow3.Click += new System.EventHandler(this.btnAddRow3_Click);
            //
            // sectionHeaderWyn5
            //
            this.sectionHeaderWyn5.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn5.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn5.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn5.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn5.Name = "sectionHeaderWyn5";
            this.sectionHeaderWyn5.Size = new System.Drawing.Size(739, 25);
            this.sectionHeaderWyn5.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn2.SvgIcon")));
            this.sectionHeaderWyn5.TabIndex = 8;
            this.sectionHeaderWyn5.Text = "조회조건";
            //
            // panelWyn5
            //
            this.panelWyn5.Controls.Add(this.panData);
            this.panelWyn5.Controls.Add(this.panelWyn6);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(3, 0);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Size = new System.Drawing.Size(744, 218);
            this.panelWyn5.TabIndex = 6;
            //
            // panData (팝업 상세 입력)
            //
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Controls.Add(this.lblPopupKey);
            this.panData.Controls.Add(this.txtPopupKey);
            this.panData.Controls.Add(this.lblPopupNm);
            this.panData.Controls.Add(this.txtPopupNm);
            this.panData.Controls.Add(this.lblProcNm);
            this.panData.Controls.Add(this.txtProcNm);
            this.panData.Controls.Add(this.btnGenerateColumns);
            this.panData.Controls.Add(this.lblHierarchical);
            this.panData.Controls.Add(this.chkHierarchical);
            this.panData.Controls.Add(this.lblKeyField);
            this.panData.Controls.Add(this.txtKeyField);
            this.panData.Controls.Add(this.lblParentField);
            this.panData.Controls.Add(this.txtParentField);
            this.panData.Controls.Add(this.lblDisplayField);
            this.panData.Controls.Add(this.txtDisplayField);
            this.panData.Controls.Add(this.lblUseYn);
            this.panData.Controls.Add(this.chkUseYn);
            this.panData.Controls.Add(this.lblPopupWidth);
            this.panData.Controls.Add(this.txtPopupWidth);
            this.panData.Controls.Add(this.lblPopupHeight);
            this.panData.Controls.Add(this.txtPopupHeight);
            this.panData.Controls.Add(this.lblRemark);
            this.panData.Controls.Add(this.txtRemark);
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(744, 191);
            this.panData.TabIndex = 8;
            //
            // lblPopupKey
            //
            this.lblPopupKey.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPopupKey.Location = new System.Drawing.Point(11, 12);
            this.lblPopupKey.Name = "lblPopupKey";
            this.lblPopupKey.Size = new System.Drawing.Size(90, 15);
            this.lblPopupKey.TabIndex = 0;
            this.lblPopupKey.Text = "팝업키";
            //
            // txtPopupKey
            //
            this.txtPopupKey.Location = new System.Drawing.Point(110, 10);
            this.txtPopupKey.Name = "txtPopupKey";
            this.txtPopupKey.Size = new System.Drawing.Size(97, 20);
            this.txtPopupKey.TabIndex = 1;
            //
            // lblPopupNm
            //
            this.lblPopupNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPopupNm.Location = new System.Drawing.Point(220, 12);
            this.lblPopupNm.Name = "lblPopupNm";
            this.lblPopupNm.Size = new System.Drawing.Size(70, 15);
            this.lblPopupNm.TabIndex = 2;
            this.lblPopupNm.Text = "팝업명";
            //
            // txtPopupNm
            //
            this.txtPopupNm.Location = new System.Drawing.Point(300, 10);
            this.txtPopupNm.Name = "txtPopupNm";
            this.txtPopupNm.Size = new System.Drawing.Size(180, 20);
            this.txtPopupNm.TabIndex = 3;
            //
            // lblProcNm
            //
            this.lblProcNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblProcNm.Location = new System.Drawing.Point(11, 40);
            this.lblProcNm.Name = "lblProcNm";
            this.lblProcNm.Size = new System.Drawing.Size(90, 15);
            this.lblProcNm.TabIndex = 4;
            this.lblProcNm.Text = "프로시저명";
            //
            // txtProcNm
            //
            this.txtProcNm.Location = new System.Drawing.Point(110, 38);
            this.txtProcNm.Name = "txtProcNm";
            this.txtProcNm.Size = new System.Drawing.Size(180, 20);
            this.txtProcNm.TabIndex = 5;
            //
            // btnGenerateColumns
            //
            this.btnGenerateColumns.Location = new System.Drawing.Point(300, 37);
            this.btnGenerateColumns.Name = "btnGenerateColumns";
            this.btnGenerateColumns.Size = new System.Drawing.Size(90, 23);
            this.btnGenerateColumns.TabIndex = 6;
            this.btnGenerateColumns.Text = "컬럼생성";
            this.btnGenerateColumns.Click += new System.EventHandler(this.btnGenerateColumns_Click);
            //
            // lblHierarchical
            //
            this.lblHierarchical.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblHierarchical.Location = new System.Drawing.Point(410, 40);
            this.lblHierarchical.Name = "lblHierarchical";
            this.lblHierarchical.Size = new System.Drawing.Size(70, 15);
            this.lblHierarchical.TabIndex = 7;
            this.lblHierarchical.Text = "계층형여부";
            //
            // chkHierarchical
            //
            this.chkHierarchical.Location = new System.Drawing.Point(490, 37);
            this.chkHierarchical.Name = "chkHierarchical";
            this.chkHierarchical.Properties.Caption = "트리로 표시";
            this.chkHierarchical.Size = new System.Drawing.Size(100, 20);
            this.chkHierarchical.TabIndex = 8;
            //
            // lblKeyField
            //
            this.lblKeyField.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblKeyField.Location = new System.Drawing.Point(11, 68);
            this.lblKeyField.Name = "lblKeyField";
            this.lblKeyField.Size = new System.Drawing.Size(90, 15);
            this.lblKeyField.TabIndex = 9;
            this.lblKeyField.Text = "키필드";
            //
            // txtKeyField
            //
            this.txtKeyField.Location = new System.Drawing.Point(110, 66);
            this.txtKeyField.Name = "txtKeyField";
            this.txtKeyField.Size = new System.Drawing.Size(97, 20);
            this.txtKeyField.TabIndex = 10;
            //
            // lblParentField
            //
            this.lblParentField.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblParentField.Location = new System.Drawing.Point(220, 68);
            this.lblParentField.Name = "lblParentField";
            this.lblParentField.Size = new System.Drawing.Size(70, 15);
            this.lblParentField.TabIndex = 11;
            this.lblParentField.Text = "부모필드";
            //
            // txtParentField
            //
            this.txtParentField.Location = new System.Drawing.Point(300, 66);
            this.txtParentField.Name = "txtParentField";
            this.txtParentField.Size = new System.Drawing.Size(180, 20);
            this.txtParentField.TabIndex = 12;
            //
            // lblDisplayField
            //
            this.lblDisplayField.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDisplayField.Location = new System.Drawing.Point(11, 96);
            this.lblDisplayField.Name = "lblDisplayField";
            this.lblDisplayField.Size = new System.Drawing.Size(90, 15);
            this.lblDisplayField.TabIndex = 13;
            this.lblDisplayField.Text = "표시필드";
            //
            // txtDisplayField
            //
            this.txtDisplayField.Location = new System.Drawing.Point(110, 94);
            this.txtDisplayField.Name = "txtDisplayField";
            this.txtDisplayField.Size = new System.Drawing.Size(97, 20);
            this.txtDisplayField.TabIndex = 14;
            //
            // lblUseYn
            //
            this.lblUseYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblUseYn.Location = new System.Drawing.Point(220, 96);
            this.lblUseYn.Name = "lblUseYn";
            this.lblUseYn.Size = new System.Drawing.Size(70, 15);
            this.lblUseYn.TabIndex = 15;
            this.lblUseYn.Text = "사용여부";
            //
            // chkUseYn
            //
            this.chkUseYn.Location = new System.Drawing.Point(300, 93);
            this.chkUseYn.Name = "chkUseYn";
            this.chkUseYn.Properties.Caption = "사용";
            this.chkUseYn.Size = new System.Drawing.Size(80, 20);
            this.chkUseYn.TabIndex = 16;
            //
            // lblPopupWidth
            //
            this.lblPopupWidth.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPopupWidth.Location = new System.Drawing.Point(11, 124);
            this.lblPopupWidth.Name = "lblPopupWidth";
            this.lblPopupWidth.Size = new System.Drawing.Size(90, 15);
            this.lblPopupWidth.TabIndex = 17;
            this.lblPopupWidth.Text = "팝업너비(px)";
            //
            // txtPopupWidth
            //
            this.txtPopupWidth.Location = new System.Drawing.Point(110, 122);
            this.txtPopupWidth.Name = "txtPopupWidth";
            this.txtPopupWidth.Size = new System.Drawing.Size(97, 20);
            this.txtPopupWidth.TabIndex = 18;
            //
            // lblPopupHeight
            //
            this.lblPopupHeight.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPopupHeight.Location = new System.Drawing.Point(220, 124);
            this.lblPopupHeight.Name = "lblPopupHeight";
            this.lblPopupHeight.Size = new System.Drawing.Size(70, 15);
            this.lblPopupHeight.TabIndex = 19;
            this.lblPopupHeight.Text = "팝업높이(px)";
            //
            // txtPopupHeight
            //
            this.txtPopupHeight.Location = new System.Drawing.Point(300, 122);
            this.txtPopupHeight.Name = "txtPopupHeight";
            this.txtPopupHeight.Size = new System.Drawing.Size(97, 20);
            this.txtPopupHeight.TabIndex = 20;
            //
            // lblRemark
            //
            this.lblRemark.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRemark.Location = new System.Drawing.Point(11, 152);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(90, 15);
            this.lblRemark.TabIndex = 21;
            this.lblRemark.Text = "비고";
            //
            // txtRemark
            //
            this.txtRemark.Location = new System.Drawing.Point(110, 150);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(500, 20);
            this.txtRemark.TabIndex = 22;
            //
            // panelWyn6
            //
            this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
            this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn6.Location = new System.Drawing.Point(0, 0);
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
            this.sectionHeaderWyn3.TabIndex = 8;
            this.sectionHeaderWyn3.Text = "팝업 상세";
            //
            // splitterWyn1
            //
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Location = new System.Drawing.Point(402, 0);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(10, 515);
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
            this.panelWyn8.Size = new System.Drawing.Size(402, 515);
            this.panelWyn8.TabIndex = 12;
            //
            // grd1 (목록 - 컬럼은 디자이너에서 추가)
            //
            this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd1.Location = new System.Drawing.Point(0, 27);
            this.grd1.MainView = this.gvw1;
            this.grd1.Name = "grd1";
            this.grd1.Size = new System.Drawing.Size(402, 488);
            this.grd1.TabIndex = 10;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            //
            // gvw1
            //
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colPopupKey,
            this.colPopupNm,
            this.colProcNm,
            this.colHierarchical});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightFocusedRow = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsSelection.InvertSelection = true;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            //
            // colPopupKey
            //
            this.colPopupKey.Caption = "팝업키";
            this.colPopupKey.FieldName = "popup_key";
            this.colPopupKey.Name = "colPopupKey";
            this.colPopupKey.Visible = true;
            this.colPopupKey.VisibleIndex = 0;
            this.colPopupKey.Width = 80;
            //
            // colPopupNm
            //
            this.colPopupNm.Caption = "팝업명";
            this.colPopupNm.FieldName = "popup_nm";
            this.colPopupNm.Name = "colPopupNm";
            this.colPopupNm.Visible = true;
            this.colPopupNm.VisibleIndex = 1;
            this.colPopupNm.Width = 120;
            //
            // colProcNm
            //
            this.colProcNm.Caption = "프로시저명";
            this.colProcNm.FieldName = "proc_nm";
            this.colProcNm.Name = "colProcNm";
            this.colProcNm.Visible = true;
            this.colProcNm.VisibleIndex = 2;
            this.colProcNm.Width = 140;
            //
            // colHierarchical
            //
            this.colHierarchical.Caption = "계층형";
            this.colHierarchical.FieldName = "hierarchical_yn";
            this.colHierarchical.Name = "colHierarchical";
            this.colHierarchical.Visible = true;
            this.colHierarchical.VisibleIndex = 3;
            this.colHierarchical.Width = 60;
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
            this.sectionHeaderWyn4.Text = "팝업 목록";
            //
            // panHeader
            //
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.txtSearchQ);
            this.panHeader.Controls.Add(this.labelControl1);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(3, 33);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1159, 49);
            this.panHeader.TabIndex = 8;
            //
            // txtSearchQ
            //
            this.txtSearchQ.Location = new System.Drawing.Point(106, 15);
            this.txtSearchQ.Name = "txtSearchQ";
            this.txtSearchQ.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtSearchQ.Size = new System.Drawing.Size(265, 20);
            this.txtSearchQ.TabIndex = 0;
            //
            // labelControl1
            //
            this.labelControl1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl1.Location = new System.Drawing.Point(25, 18);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(77, 15);
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
            this.paTitle.Size = new System.Drawing.Size(1159, 33);
            this.paTitle.TabIndex = 5;
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
            this.sectionHeaderWyn1.Text = "팝업관리 [SYS_POPUP]";
            //
            // frmSysPopup
            //
            this.ClientSize = new System.Drawing.Size(1165, 600);
            this.Controls.Add(this.panBase);
            this.Name = "frmSysPopup";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxControlType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEditVisible)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
            this.panelWyn7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxSearchControlType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn9)).EndInit();
            this.panelWyn9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn10)).EndInit();
            this.panelWyn10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPopupKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPopupNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProcNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkHierarchical.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKeyField.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParentField.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDisplayField.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUseYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPopupWidth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPopupHeight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchQ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
            this.paTitle.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private PanelWyn paTitle;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn4;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblPopupKey;
    private TextEditWyn txtPopupKey;
    private DevExpress.XtraEditors.LabelControl lblPopupNm;
    private TextEditWyn txtPopupNm;
    private DevExpress.XtraEditors.LabelControl lblProcNm;
    private TextEditWyn txtProcNm;
    private ButtonWyn btnGenerateColumns;
    private DevExpress.XtraEditors.LabelControl lblHierarchical;
    private CheckBoxWyn chkHierarchical;
    private DevExpress.XtraEditors.LabelControl lblKeyField;
    private TextEditWyn txtKeyField;
    private DevExpress.XtraEditors.LabelControl lblParentField;
    private TextEditWyn txtParentField;
    private DevExpress.XtraEditors.LabelControl lblDisplayField;
    private TextEditWyn txtDisplayField;
    private DevExpress.XtraEditors.LabelControl lblUseYn;
    private CheckBoxWyn chkUseYn;
    private DevExpress.XtraEditors.LabelControl lblPopupWidth;
    private TextEditWyn txtPopupWidth;
    private DevExpress.XtraEditors.LabelControl lblPopupHeight;
    private TextEditWyn txtPopupHeight;
    private DevExpress.XtraEditors.LabelControl lblRemark;
    private TextEditWyn txtRemark;
    private PanelWyn panelWyn6;
    private SectionHeaderWyn sectionHeaderWyn1;
    private SplitterWyn splitterWyn1;
    private SectionHeaderWyn sectionHeaderWyn3;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colPopupKey;
    private DevExpress.XtraGrid.Columns.GridColumn colPopupNm;
    private DevExpress.XtraGrid.Columns.GridColumn colProcNm;
    private DevExpress.XtraGrid.Columns.GridColumn colHierarchical;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private DevExpress.XtraGrid.Columns.GridColumn colColumnNm;
    private DevExpress.XtraGrid.Columns.GridColumn colCaption;
    private DevExpress.XtraGrid.Columns.GridColumn colControlType;
    private DevExpress.XtraGrid.Columns.GridColumn colLookupProcNm;
    private DevExpress.XtraGrid.Columns.GridColumn colSort;
    private DevExpress.XtraGrid.Columns.GridColumn colWidth;
    private DevExpress.XtraGrid.Columns.GridColumn colVisibleYn;
    private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxControlType;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEditVisible;
    private PanelWyn panelWyn2;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn panelWyn1;
    private SectionHeaderWyn sectionHeaderWyn2;
    private PanelWyn panHeader;
    private TextEditWyn txtSearchQ;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private ButtonWyn btnDeletRow2;
    private ButtonWyn btnAddRow2;
    private PanelWyn panelWyn7;
    private PanelWyn panelWyn8;
    private GridControlWyn grd3;
    private GridViewWyn gvw3;
    private DevExpress.XtraGrid.Columns.GridColumn colParamNm;
    private DevExpress.XtraGrid.Columns.GridColumn colSearchCaption;
    private DevExpress.XtraGrid.Columns.GridColumn colSearchControlType;
    private DevExpress.XtraGrid.Columns.GridColumn colSearchSort;
    private DevExpress.XtraGrid.Columns.GridColumn colSearchWidth;
    private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxSearchControlType;
    private PanelWyn panelWyn9;
    private PanelWyn panelWyn10;
    private ButtonWyn btnDeletRow3;
    private ButtonWyn btnAddRow3;
    private SectionHeaderWyn sectionHeaderWyn5;
}
