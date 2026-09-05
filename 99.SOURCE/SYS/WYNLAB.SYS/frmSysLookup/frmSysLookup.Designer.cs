// VS 디자이너가 자동 생성하는 필드 선언에는 = null!을 안 붙여서 nullable 경고(CS8618)가
// 계속 나기 때문에, 이 파일(디자이너 전용)만 nullable 검사를 끈다 - 흔한 관례.
#nullable disable
using DevExpress.XtraGrid;
using WYNLAB.Base.Controls;

namespace WYNLAB.SYS;

/// <summary>
/// frmSysPopup(팝업관리)을 복사해서 만든 LookUp관리 화면 - 구조는 완전히 같다(제목바 + 검색패널 +
/// 좌우 스플리터 + 목록그리드(grd1)/상세패널(panData)/하위그리드(grd2)). 팝업과 달리 컬럼 그리드가
/// 없다 - LookUp은 결과셋 전체를 보여주는 게 아니라 값(코드)/표시값(명칭) 두 필드만 뽑아 쓰기
/// 때문에, 하위그리드 하나는 파라미터 목록(sysLookupP) 전용이다.
/// </summary>
public partial class frmSysLookup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSysLookup));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colParamNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colParamCaption = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colParamSort = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colParamTestValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.splitterColumns = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelColumns = new WYNLAB.Base.Controls.PanelWyn();
            this.grd4 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw4 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colColumnNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colColumnCaption = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colColumnWidth = new DevExpress.XtraGrid.Columns.GridColumn();
            this.sectionHeaderColumns = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn2 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelPreview = new WYNLAB.Base.Controls.PanelWyn();
            this.grd3 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw3 = new WYNLAB.Base.Controls.GridViewWyn();
            this.panelWyn9 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnPreview = new WYNLAB.Base.Controls.ButtonWyn();
            this.sectionHeaderWyn5 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDeletRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAddRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colLookupKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLookupNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProcNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colValueField = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDisplayField = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn10 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnCopy = new WYNLAB.Base.Controls.ButtonWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.txtSearchQ = new WYNLAB.Base.Controls.TextEditWyn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblLookupKey = new DevExpress.XtraEditors.LabelControl();
            this.txtLookupKey = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblLookupNm = new DevExpress.XtraEditors.LabelControl();
            this.txtLookupNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblProcNm = new DevExpress.XtraEditors.LabelControl();
            this.txtProcNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.btnGenerateParams = new WYNLAB.Base.Controls.ButtonWyn();
            this.lblValueField = new DevExpress.XtraEditors.LabelControl();
            this.txtValueField = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDisplayField = new DevExpress.XtraEditors.LabelControl();
            this.txtDisplayField = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblUseYn = new DevExpress.XtraEditors.LabelControl();
            this.chkUseYn = new WYNLAB.Base.Controls.CheckBoxWyn();
            this.lblRemark = new DevExpress.XtraEditors.LabelControl();
            this.txtRemark = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSourceType = new DevExpress.XtraEditors.LabelControl();
            this.cboSourceType = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblQueryTxt = new DevExpress.XtraEditors.LabelControl();
            this.txtQueryTxt = new WYNLAB.Base.Controls.MemoEditWyn();
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
            ((System.ComponentModel.ISupportInitialize)(this.panelColumns)).BeginInit();
            this.panelColumns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelPreview)).BeginInit();
            this.panelPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn9)).BeginInit();
            this.panelWyn9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLookupKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLookupNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProcNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValueField.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDisplayField.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUseYn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSourceType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQueryTxt.Properties)).BeginInit();
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
            this.panelWyn4.Controls.Add(this.splitterWyn2);
            this.panelWyn4.Controls.Add(this.panelPreview);
            this.panelWyn4.Controls.Add(this.panelWyn1);
            this.panelWyn4.Controls.Add(this.panelWyn5);
            this.panelWyn4.Controls.Add(this.splitterColumns);
            this.panelWyn4.Controls.Add(this.panelColumns);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(412, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelWyn4.Size = new System.Drawing.Size(747, 515);
            this.panelWyn4.TabIndex = 7;
            //
            // grd2 (파라미터 목록 - sysLookupP 한 행 = 이 LookUp 프로시져가 받는 파라미터 하나)
            //
            this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd2.Location = new System.Drawing.Point(3, 187);
            this.grd2.MainView = this.gvw2;
            this.grd2.Name = "grd2";
            this.grd2.Size = new System.Drawing.Size(744, 328);
            this.grd2.TabIndex = 7;
            this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw2});
            //
            // gvw2
            //
            this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colParamNm,
            this.colParamCaption,
            this.colParamSort,
            this.colParamTestValue});
            this.gvw2.GridControl = this.grd2;
            this.gvw2.HighlightFocusedRow = true;
            this.gvw2.Name = "gvw2";
            this.gvw2.OptionsView.ColumnAutoWidth = false;
            this.gvw2.OptionsView.ShowGroupPanel = false;
            //
            // colParamNm
            //
            this.colParamNm.Caption = "파라미터명";
            this.colParamNm.FieldName = "param_nm";
            this.colParamNm.Name = "colParamNm";
            this.colParamNm.OptionsColumn.AllowEdit = false;
            this.colParamNm.Visible = true;
            this.colParamNm.VisibleIndex = 0;
            this.colParamNm.Width = 120;
            //
            // colParamCaption
            //
            this.colParamCaption.Caption = "설명(참고용)";
            this.colParamCaption.FieldName = "caption";
            this.colParamCaption.Name = "colParamCaption";
            this.colParamCaption.Visible = true;
            this.colParamCaption.VisibleIndex = 1;
            this.colParamCaption.Width = 200;
            //
            // colParamSort
            //
            this.colParamSort.Caption = "순서";
            this.colParamSort.FieldName = "sort";
            this.colParamSort.Name = "colParamSort";
            this.colParamSort.Visible = true;
            this.colParamSort.VisibleIndex = 2;
            this.colParamSort.Width = 60;
            //
            // colParamTestValue (sysLookupP엔 저장 안 됨 - "실행결과 미리보기" 전용 임시 입력칸.
            // btnPreview_Click이 이 값을 읽어서 서버에 보낸다)
            //
            this.colParamTestValue.Caption = "테스트값(미리보기 전용)";
            this.colParamTestValue.FieldName = "test_value";
            this.colParamTestValue.Name = "colParamTestValue";
            this.colParamTestValue.Visible = true;
            this.colParamTestValue.VisibleIndex = 3;
            this.colParamTestValue.Width = 140;
            //
            // splitterColumns (파라미터 목록 grd2 <-> 컬럼 목록 panelColumns 사이 - splitterWyn1과
            // 같은 컨트롤을 좌우 방향(Dock=Right)으로 재사용)
            //
            this.splitterColumns.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterColumns.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitterColumns.Location = new System.Drawing.Point(407, 0);
            this.splitterColumns.Name = "splitterColumns";
            this.splitterColumns.Size = new System.Drawing.Size(10, 515);
            this.splitterColumns.TabIndex = 21;
            this.splitterColumns.TabStop = false;
            //
            // panelColumns (LookUp 팝업에 값필드/표시필드 외 추가로 보여줄 컬럼 구성 - sysLookupC.
            // "파라미터생성" 버튼이 grd2와 같은 방식으로 결과셋 컬럼을 자동으로 채워 넣는다
            // (btnGenerateParams_Click 참고). 필드명은 소스(프로시져/쿼리) 결과셋의 실제 컬럼명이라
            // 읽기전용, 캡션/폭만 직접 입력한다.)
            //
            this.panelColumns.Controls.Add(this.grd4);
            this.panelColumns.Controls.Add(this.sectionHeaderColumns);
            this.panelColumns.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelColumns.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelColumns.Location = new System.Drawing.Point(417, 0);
            this.panelColumns.Name = "panelColumns";
            this.panelColumns.Padding = new System.Windows.Forms.Padding(5, 27, 0, 0);
            this.panelColumns.Size = new System.Drawing.Size(330, 515);
            this.panelColumns.TabIndex = 22;
            //
            // grd4 (컬럼 목록)
            //
            this.grd4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd4.Location = new System.Drawing.Point(5, 27);
            this.grd4.MainView = this.gvw4;
            this.grd4.Name = "grd4";
            this.grd4.Size = new System.Drawing.Size(325, 488);
            this.grd4.TabIndex = 0;
            this.grd4.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw4});
            //
            // gvw4
            //
            this.gvw4.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colColumnNm,
            this.colColumnCaption,
            this.colColumnWidth});
            this.gvw4.GridControl = this.grd4;
            this.gvw4.HighlightFocusedRow = true;
            this.gvw4.Name = "gvw4";
            this.gvw4.OptionsView.ColumnAutoWidth = false;
            this.gvw4.OptionsView.ShowGroupPanel = false;
            //
            // colColumnNm
            //
            this.colColumnNm.Caption = "필드명";
            this.colColumnNm.FieldName = "column_nm";
            this.colColumnNm.Name = "colColumnNm";
            this.colColumnNm.OptionsColumn.AllowEdit = false;
            this.colColumnNm.Visible = true;
            this.colColumnNm.VisibleIndex = 0;
            this.colColumnNm.Width = 110;
            //
            // colColumnCaption
            //
            this.colColumnCaption.Caption = "캡션";
            this.colColumnCaption.FieldName = "caption";
            this.colColumnCaption.Name = "colColumnCaption";
            this.colColumnCaption.Visible = true;
            this.colColumnCaption.VisibleIndex = 1;
            this.colColumnCaption.Width = 110;
            //
            // colColumnWidth
            //
            this.colColumnWidth.Caption = "폭";
            this.colColumnWidth.FieldName = "width";
            this.colColumnWidth.Name = "colColumnWidth";
            this.colColumnWidth.Visible = true;
            this.colColumnWidth.VisibleIndex = 2;
            this.colColumnWidth.Width = 60;
            //
            // sectionHeaderColumns
            //
            this.sectionHeaderColumns.BackColor = System.Drawing.Color.White;
            this.sectionHeaderColumns.Dock = System.Windows.Forms.DockStyle.Top;
            this.sectionHeaderColumns.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderColumns.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderColumns.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderColumns.Name = "sectionHeaderColumns";
            this.sectionHeaderColumns.Size = new System.Drawing.Size(325, 27);
            this.sectionHeaderColumns.TabIndex = 0;
            this.sectionHeaderColumns.Text = "컬럼 목록";
            //
            // splitterWyn2 (파라미터 목록 grd2 <-> 실행결과 미리보기 panelPreview 사이 - splitterWyn1과
            // 같은 컨트롤을 세로 방향(Dock=Bottom)으로 재사용)
            //
            this.splitterWyn2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterWyn2.Location = new System.Drawing.Point(3, 305);
            this.splitterWyn2.Name = "splitterWyn2";
            this.splitterWyn2.Size = new System.Drawing.Size(744, 10);
            this.splitterWyn2.TabIndex = 19;
            this.splitterWyn2.TabStop = false;
            //
            // panelPreview (프로시저/쿼리를 실제로 실행해서 결과를 훑어보기만 하는 용도 - 저장되는
            // 데이터 아님. btnPreview_Click이 api/lookup-admin/preview를 불러서 grd3에 그대로
            // 바인딩한다. gvw3엔 디자이너에서 고정 컬럼을 안 넣는다 - 프로시저/쿼리마다 결과
            // 컬럼이 전부 달라서 GridView의 기본 컬럼 자동생성(OptionsBehavior.AutoPopulateColumns,
            // 기본값 true)에 맡긴다.)
            //
            this.panelPreview.Controls.Add(this.grd3);
            this.panelPreview.Controls.Add(this.panelWyn9);
            this.panelPreview.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelPreview.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelPreview.Location = new System.Drawing.Point(3, 315);
            this.panelPreview.Name = "panelPreview";
            this.panelPreview.Size = new System.Drawing.Size(744, 200);
            this.panelPreview.TabIndex = 20;
            //
            // grd3 (실행결과 미리보기)
            //
            this.grd3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd3.Location = new System.Drawing.Point(0, 27);
            this.grd3.MainView = this.gvw3;
            this.grd3.Name = "grd3";
            this.grd3.Size = new System.Drawing.Size(744, 173);
            this.grd3.TabIndex = 0;
            this.grd3.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw3});
            //
            // gvw3
            //
            this.gvw3.GridControl = this.grd3;
            this.gvw3.Name = "gvw3";
            this.gvw3.OptionsBehavior.Editable = false;
            this.gvw3.OptionsView.ShowGroupPanel = false;
            //
            // panelWyn9
            //
            this.panelWyn9.Controls.Add(this.btnPreview);
            this.panelWyn9.Controls.Add(this.sectionHeaderWyn5);
            this.panelWyn9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn9.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn9.Location = new System.Drawing.Point(0, 0);
            this.panelWyn9.Name = "panelWyn9";
            this.panelWyn9.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn9.Size = new System.Drawing.Size(744, 27);
            this.panelWyn9.TabIndex = 1;
            //
            // btnPreview
            //
            this.btnPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreview.Location = new System.Drawing.Point(614, 1);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(125, 23);
            this.btnPreview.TabIndex = 0;
            this.btnPreview.Text = "실행결과 미리보기";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
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
            this.sectionHeaderWyn5.TabIndex = 8;
            this.sectionHeaderWyn5.Text = "실행결과 미리보기 (저장 안 됨)";
            //
            // panelWyn1
            //
            this.panelWyn1.Controls.Add(this.panelWyn7);
            this.panelWyn1.Controls.Add(this.sectionHeaderWyn2);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(3, 160);
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
            this.sectionHeaderWyn2.Text = "파라미터 목록";
            //
            // panelWyn5
            //
            this.panelWyn5.Controls.Add(this.panData);
            this.panelWyn5.Controls.Add(this.panelWyn6);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(3, 0);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Size = new System.Drawing.Size(744, 382);
            this.panelWyn5.TabIndex = 6;
            //
            // panData (LookUp 상세 입력)
            //
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Controls.Add(this.lblLookupKey);
            this.panData.Controls.Add(this.txtLookupKey);
            this.panData.Controls.Add(this.lblLookupNm);
            this.panData.Controls.Add(this.txtLookupNm);
            this.panData.Controls.Add(this.lblProcNm);
            this.panData.Controls.Add(this.txtProcNm);
            this.panData.Controls.Add(this.btnGenerateParams);
            this.panData.Controls.Add(this.lblValueField);
            this.panData.Controls.Add(this.txtValueField);
            this.panData.Controls.Add(this.lblDisplayField);
            this.panData.Controls.Add(this.txtDisplayField);
            this.panData.Controls.Add(this.lblUseYn);
            this.panData.Controls.Add(this.chkUseYn);
            this.panData.Controls.Add(this.lblRemark);
            this.panData.Controls.Add(this.txtRemark);
            this.panData.Controls.Add(this.lblSourceType);
            this.panData.Controls.Add(this.cboSourceType);
            this.panData.Controls.Add(this.lblQueryTxt);
            this.panData.Controls.Add(this.txtQueryTxt);
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(744, 355);
            this.panData.TabIndex = 8;
            //
            // lblLookupKey
            //
            this.lblLookupKey.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLookupKey.Location = new System.Drawing.Point(11, 12);
            this.lblLookupKey.Name = "lblLookupKey";
            this.lblLookupKey.Size = new System.Drawing.Size(90, 15);
            this.lblLookupKey.TabIndex = 0;
            this.lblLookupKey.Text = "LookUp키";
            //
            // txtLookupKey
            //
            this.txtLookupKey.Location = new System.Drawing.Point(110, 10);
            this.txtLookupKey.Name = "txtLookupKey";
            this.txtLookupKey.Size = new System.Drawing.Size(97, 20);
            this.txtLookupKey.TabIndex = 1;
            //
            // lblLookupNm
            //
            this.lblLookupNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLookupNm.Location = new System.Drawing.Point(220, 12);
            this.lblLookupNm.Name = "lblLookupNm";
            this.lblLookupNm.Size = new System.Drawing.Size(70, 15);
            this.lblLookupNm.TabIndex = 2;
            this.lblLookupNm.Text = "LookUp명";
            //
            // txtLookupNm
            //
            this.txtLookupNm.Location = new System.Drawing.Point(300, 10);
            this.txtLookupNm.Name = "txtLookupNm";
            this.txtLookupNm.Size = new System.Drawing.Size(180, 20);
            this.txtLookupNm.TabIndex = 3;
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
            // btnGenerateParams
            //
            this.btnGenerateParams.Location = new System.Drawing.Point(300, 37);
            this.btnGenerateParams.Name = "btnGenerateParams";
            this.btnGenerateParams.Size = new System.Drawing.Size(100, 23);
            this.btnGenerateParams.TabIndex = 6;
            this.btnGenerateParams.Text = "파라미터생성";
            this.btnGenerateParams.Click += new System.EventHandler(this.btnGenerateParams_Click);
            //
            // lblValueField
            //
            this.lblValueField.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblValueField.Location = new System.Drawing.Point(11, 68);
            this.lblValueField.Name = "lblValueField";
            this.lblValueField.Size = new System.Drawing.Size(90, 15);
            this.lblValueField.TabIndex = 7;
            this.lblValueField.Text = "값필드(코드)";
            //
            // txtValueField
            //
            this.txtValueField.Location = new System.Drawing.Point(110, 66);
            this.txtValueField.Name = "txtValueField";
            this.txtValueField.Size = new System.Drawing.Size(97, 20);
            this.txtValueField.TabIndex = 8;
            //
            // lblDisplayField
            //
            this.lblDisplayField.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDisplayField.Location = new System.Drawing.Point(220, 68);
            this.lblDisplayField.Name = "lblDisplayField";
            this.lblDisplayField.Size = new System.Drawing.Size(70, 15);
            this.lblDisplayField.TabIndex = 9;
            this.lblDisplayField.Text = "표시필드(명칭)";
            //
            // txtDisplayField
            //
            this.txtDisplayField.Location = new System.Drawing.Point(300, 66);
            this.txtDisplayField.Name = "txtDisplayField";
            this.txtDisplayField.Size = new System.Drawing.Size(180, 20);
            this.txtDisplayField.TabIndex = 10;
            //
            // lblUseYn
            //
            this.lblUseYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblUseYn.Location = new System.Drawing.Point(11, 96);
            this.lblUseYn.Name = "lblUseYn";
            this.lblUseYn.Size = new System.Drawing.Size(90, 15);
            this.lblUseYn.TabIndex = 11;
            this.lblUseYn.Text = "사용여부";
            //
            // chkUseYn
            //
            this.chkUseYn.Location = new System.Drawing.Point(110, 93);
            this.chkUseYn.Name = "chkUseYn";
            this.chkUseYn.Properties.Caption = "사용";
            this.chkUseYn.Size = new System.Drawing.Size(80, 20);
            this.chkUseYn.TabIndex = 12;
            //
            // lblRemark
            //
            this.lblRemark.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRemark.Location = new System.Drawing.Point(220, 96);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(70, 15);
            this.lblRemark.TabIndex = 13;
            this.lblRemark.Text = "비고";
            //
            // txtRemark
            //
            this.txtRemark.Location = new System.Drawing.Point(300, 93);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(400, 20);
            this.txtRemark.TabIndex = 14;
            //
            // lblSourceType (프로시저/쿼리 중 어느 방식으로 결과를 만들지 - LookUp키/LookUp명과
            // 같은 첫 줄 오른쪽 빈 공간에 놓는다)
            //
            this.lblSourceType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSourceType.Location = new System.Drawing.Point(500, 12);
            this.lblSourceType.Name = "lblSourceType";
            this.lblSourceType.Size = new System.Drawing.Size(70, 15);
            this.lblSourceType.TabIndex = 15;
            this.lblSourceType.Text = "소스유형";
            //
            // cboSourceType (BindCodeList로 코드값을 직접 채운다 - frmSysLookup.cs 생성자 참고.
            // sysLookupM처럼 서버에 등록된 목록이 아니라 화면 자체가 아는 고정 2개짜리 목록이라
            // LookupKey는 안 쓴다)
            //
            this.cboSourceType.Location = new System.Drawing.Point(575, 10);
            this.cboSourceType.Name = "cboSourceType";
            this.cboSourceType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboSourceType.Size = new System.Drawing.Size(100, 20);
            this.cboSourceType.TabIndex = 16;
            //
            // lblQueryTxt (프로시저명 대신 직접 SQL을 쓰는 경우 - 소스유형=쿼리일 때만 의미가
            // 있어서, 화면 로직에서 소스유형에 따라 이 줄과 프로시저명/파라미터생성 줄을
            // 서로 바꿔가며 보여준다)
            //
            this.lblQueryTxt.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblQueryTxt.Location = new System.Drawing.Point(11, 123);
            this.lblQueryTxt.Name = "lblQueryTxt";
            this.lblQueryTxt.Size = new System.Drawing.Size(90, 15);
            this.lblQueryTxt.TabIndex = 17;
            this.lblQueryTxt.Text = "쿼리문";
            //
            // txtQueryTxt
            //
            this.txtQueryTxt.Location = new System.Drawing.Point(110, 121);
            this.txtQueryTxt.Name = "txtQueryTxt";
            // 기존(55px)의 4배 - 쿼리문이 한 줄 이상 넘어가면 스크롤 없이 더 볼 수 있게 해달라는
            // 요청(2026-09-02). panData/panelWyn5 높이도 늘어난 만큼 같이 키워야 한다(아래 참고).
            this.txtQueryTxt.Size = new System.Drawing.Size(620, 220);
            this.txtQueryTxt.TabIndex = 18;
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
            this.sectionHeaderWyn3.Text = "LookUp 상세";
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
            // grd1 (목록)
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
            this.colLookupKey,
            this.colLookupNm,
            this.colProcNm,
            this.colValueField,
            this.colDisplayField});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightFocusedRow = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsSelection.InvertSelection = true;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            //
            // colLookupKey
            //
            this.colLookupKey.Caption = "LookUp키";
            this.colLookupKey.FieldName = "lookup_key";
            this.colLookupKey.Name = "colLookupKey";
            this.colLookupKey.Visible = true;
            this.colLookupKey.VisibleIndex = 0;
            this.colLookupKey.Width = 80;
            //
            // colLookupNm
            //
            this.colLookupNm.Caption = "LookUp명";
            this.colLookupNm.FieldName = "lookup_nm";
            this.colLookupNm.Name = "colLookupNm";
            this.colLookupNm.Visible = true;
            this.colLookupNm.VisibleIndex = 1;
            this.colLookupNm.Width = 100;
            //
            // colProcNm
            //
            this.colProcNm.Caption = "프로시저명";
            this.colProcNm.FieldName = "proc_nm";
            this.colProcNm.Name = "colProcNm";
            this.colProcNm.Visible = true;
            this.colProcNm.VisibleIndex = 2;
            this.colProcNm.Width = 130;
            //
            // colValueField
            //
            this.colValueField.Caption = "값필드";
            this.colValueField.FieldName = "value_field";
            this.colValueField.Name = "colValueField";
            this.colValueField.Visible = true;
            this.colValueField.VisibleIndex = 3;
            this.colValueField.Width = 70;
            //
            // colDisplayField
            //
            this.colDisplayField.Caption = "표시필드";
            this.colDisplayField.FieldName = "display_field";
            this.colDisplayField.Name = "colDisplayField";
            this.colDisplayField.Visible = true;
            this.colDisplayField.VisibleIndex = 4;
            this.colDisplayField.Width = 70;
            //
            // panelWyn2
            //
            this.panelWyn2.Controls.Add(this.panelWyn10);
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
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(327, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 8;
            this.sectionHeaderWyn4.Text = "LookUp 목록";
            //
            // panelWyn10 (선택한 LookUp을 복사해서 새 LookUp을 만드는 버튼 - 파라미터 목록의
            // 행추가/행삭제 버튼(panelWyn7)과 같은 배치 방식)
            //
            this.panelWyn10.Controls.Add(this.btnCopy);
            this.panelWyn10.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelWyn10.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn10.Location = new System.Drawing.Point(332, 0);
            this.panelWyn10.Name = "panelWyn10";
            this.panelWyn10.Size = new System.Drawing.Size(70, 25);
            this.panelWyn10.TabIndex = 9;
            //
            // btnCopy
            //
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopy.Location = new System.Drawing.Point(2, 1);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(65, 22);
            this.btnCopy.TabIndex = 0;
            this.btnCopy.Text = "복사";
            this.btnCopy.ToolTip = "선택한 LookUp을 복사해서 새 LookUp 만들기";
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
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
            this.sectionHeaderWyn1.Text = "LookUp관리 [SYS_LOOKUP]";
            //
            // frmSysLookup
            //
            this.ClientSize = new System.Drawing.Size(1165, 600);
            this.Controls.Add(this.panBase);
            this.Name = "frmSysLookup";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelColumns)).EndInit();
            this.panelColumns.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelPreview)).EndInit();
            this.panelPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn9)).EndInit();
            this.panelWyn9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
            this.panelWyn7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLookupKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLookupNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProcNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValueField.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDisplayField.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUseYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSourceType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQueryTxt.Properties)).EndInit();
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
    private DevExpress.XtraEditors.LabelControl lblLookupKey;
    private TextEditWyn txtLookupKey;
    private DevExpress.XtraEditors.LabelControl lblLookupNm;
    private TextEditWyn txtLookupNm;
    private DevExpress.XtraEditors.LabelControl lblProcNm;
    private TextEditWyn txtProcNm;
    private ButtonWyn btnGenerateParams;
    private DevExpress.XtraEditors.LabelControl lblValueField;
    private TextEditWyn txtValueField;
    private DevExpress.XtraEditors.LabelControl lblDisplayField;
    private TextEditWyn txtDisplayField;
    private DevExpress.XtraEditors.LabelControl lblUseYn;
    private CheckBoxWyn chkUseYn;
    private DevExpress.XtraEditors.LabelControl lblRemark;
    private TextEditWyn txtRemark;
    private DevExpress.XtraEditors.LabelControl lblSourceType;
    private LookUpEditWyn cboSourceType;
    private DevExpress.XtraEditors.LabelControl lblQueryTxt;
    private MemoEditWyn txtQueryTxt;
    private PanelWyn panelWyn6;
    private SectionHeaderWyn sectionHeaderWyn1;
    private SplitterWyn splitterWyn1;
    private SectionHeaderWyn sectionHeaderWyn3;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colLookupKey;
    private DevExpress.XtraGrid.Columns.GridColumn colLookupNm;
    private DevExpress.XtraGrid.Columns.GridColumn colProcNm;
    private DevExpress.XtraGrid.Columns.GridColumn colValueField;
    private DevExpress.XtraGrid.Columns.GridColumn colDisplayField;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private SplitterWyn splitterWyn2;
    private PanelWyn panelPreview;
    private GridControlWyn grd3;
    private GridViewWyn gvw3;
    private PanelWyn panelWyn9;
    private ButtonWyn btnPreview;
    private SectionHeaderWyn sectionHeaderWyn5;
    private DevExpress.XtraGrid.Columns.GridColumn colParamNm;
    private DevExpress.XtraGrid.Columns.GridColumn colParamCaption;
    private DevExpress.XtraGrid.Columns.GridColumn colParamSort;
    private DevExpress.XtraGrid.Columns.GridColumn colParamTestValue;
    private PanelWyn panelWyn2;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn panelWyn10;
    private ButtonWyn btnCopy;
    private PanelWyn panelWyn1;
    private SectionHeaderWyn sectionHeaderWyn2;
    private PanelWyn panHeader;
    private TextEditWyn txtSearchQ;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private ButtonWyn btnDeletRow2;
    private ButtonWyn btnAddRow2;
    private PanelWyn panelWyn7;
    private PanelWyn panelWyn8;
    private SplitterWyn splitterColumns;
    private PanelWyn panelColumns;
    private GridControlWyn grd4;
    private GridViewWyn gvw4;
    private DevExpress.XtraGrid.Columns.GridColumn colColumnNm;
    private DevExpress.XtraGrid.Columns.GridColumn colColumnCaption;
    private DevExpress.XtraGrid.Columns.GridColumn colColumnWidth;
    private SectionHeaderWyn sectionHeaderColumns;
}
