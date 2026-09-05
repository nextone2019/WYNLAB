// VS 디자이너가 자동 생성하는 필드 선언에는 = null!을 안 붙여서 nullable 경고(CS8618)가
// 계속 나기 때문에, 이 파일(디자이너 전용)만 nullable 검사를 끈다 - 흔한 관례. 이 프로젝트를
// 복사해서 새 화면을 만들 때도 그 화면의 Designer.cs 맨 위에 이 줄을 그대로 유지할 것.
#nullable disable
using DevExpress.XtraGrid;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

/// <summary>
/// 품목등록 화면 - grd1(품목 목록)/panData(품목 상세, TBAITEM 컬럼 그대로)/grd2(TBAITEMUNIT
/// 단위환산 입력그리드, 조회·편집 가능). panData는 31개 필드를 4단으로 빽빽하게 배치했다 -
/// 캡션은 컬럼명 기준 추정치라 실제 업무 용어와 다르면 고칠 것, 타입도 전부 기본 TextEditWyn이라
/// 코드성 필드(unit_cd/dept_cd/emp_no/cust_cd 등)는 LookUpEditWyn으로, Yn 필드는 CheckBoxWyn으로
/// 바꾸는 걸 권장한다.
/// </summary>
public partial class frmItem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItem));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colUnitFrUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnitFrQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnitToUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnitToQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnitRemark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDeletRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAddRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.lblItemCd = new DevExpress.XtraEditors.LabelControl();
            this.txtItemCd = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblItemNo = new DevExpress.XtraEditors.LabelControl();
            this.txtItemNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblItemNm = new DevExpress.XtraEditors.LabelControl();
            this.txtItemNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblItemSpec = new DevExpress.XtraEditors.LabelControl();
            this.txtItemSpec = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblUnitCd = new DevExpress.XtraEditors.LabelControl();
            this.cboUnitCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblPoUnitCd = new DevExpress.XtraEditors.LabelControl();
            this.cboAssetType = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.cboPoUnitCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblWhCd = new DevExpress.XtraEditors.LabelControl();
            this.txtWhCd = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblLocCd = new DevExpress.XtraEditors.LabelControl();
            this.txtLocCd = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSafeQty = new DevExpress.XtraEditors.LabelControl();
            this.txtSafeQty = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDeptCd = new DevExpress.XtraEditors.LabelControl();
            this.txtDeptCd = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblEmpNo = new DevExpress.XtraEditors.LabelControl();
            this.txtEmpNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblProdYn = new DevExpress.XtraEditors.LabelControl();
            this.txtProdYn = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblCustCd = new DevExpress.XtraEditors.LabelControl();
            this.txtCustCd = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblAssetType = new DevExpress.XtraEditors.LabelControl();
            this.lblOutType = new DevExpress.XtraEditors.LabelControl();
            this.txtOutType = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblPoQcYn = new DevExpress.XtraEditors.LabelControl();
            this.txtPoQcYn = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblProdQcYn = new DevExpress.XtraEditors.LabelControl();
            this.txtProdQcYn = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblLotYn = new DevExpress.XtraEditors.LabelControl();
            this.txtLotYn = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblStockYn = new DevExpress.XtraEditors.LabelControl();
            this.txtStockYn = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblPoYn = new DevExpress.XtraEditors.LabelControl();
            this.txtPoYn = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblPoPrice = new DevExpress.XtraEditors.LabelControl();
            this.txtPoPrice = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSaleYn = new DevExpress.XtraEditors.LabelControl();
            this.txtSaleYn = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSalePrice = new DevExpress.XtraEditors.LabelControl();
            this.txtSalePrice = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblStatCd = new DevExpress.XtraEditors.LabelControl();
            this.txtStatCd = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblItemClass1 = new DevExpress.XtraEditors.LabelControl();
            this.txtItemClass1 = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblItemClass2 = new DevExpress.XtraEditors.LabelControl();
            this.txtItemClass2 = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblItemClass3 = new DevExpress.XtraEditors.LabelControl();
            this.txtItemClass3 = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblItemClass4 = new DevExpress.XtraEditors.LabelControl();
            this.txtItemClass4 = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblPoAcntCd = new DevExpress.XtraEditors.LabelControl();
            this.txtPoAcntCd = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSaleAcntCd = new DevExpress.XtraEditors.LabelControl();
            this.txtSaleAcntCd = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblRemark = new DevExpress.XtraEditors.LabelControl();
            this.txtRemark = new WYNLAB.Base.Controls.TextEditWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colListItemId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colListItemCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colListItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colListUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colListStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.txtSearchQ = new WYNLAB.Base.Controls.TextEditWyn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
            this.panelWyn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
            this.panelWyn4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemSpec.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnitCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAssetType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPoUnitCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWhCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSafeQty.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProdYn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPoQcYn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProdQcYn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLotYn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStockYn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPoYn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPoPrice.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaleYn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSalePrice.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStatCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemClass1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemClass2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemClass3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemClass4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPoAcntCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaleAcntCd.Properties)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
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
            this.panelWyn4.Controls.Add(this.panelWyn5);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(412, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelWyn4.Size = new System.Drawing.Size(747, 515);
            this.panelWyn4.TabIndex = 7;
            // 
            // grd2
            // 
            this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd2.Location = new System.Drawing.Point(3, 272);
            this.grd2.MainView = this.gvw2;
            this.grd2.Name = "grd2";
            this.grd2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1});
            this.grd2.Size = new System.Drawing.Size(744, 243);
            this.grd2.TabIndex = 7;
            this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw2});
            // 
            // gvw2
            // 
            this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colUnitFrUnitCd,
            this.colUnitFrQty,
            this.colUnitToUnitCd,
            this.colUnitToQty,
            this.colUnitRemark});
            this.gvw2.GridControl = this.grd2;
            this.gvw2.HighlightFocusedRow = true;
            this.gvw2.Name = "gvw2";
            this.gvw2.OptionsView.ColumnAutoWidth = false;
            this.gvw2.OptionsView.ShowGroupPanel = false;
            // 
            // colUnitFrUnitCd
            // 
            this.colUnitFrUnitCd.Caption = "기준단위";
            this.colUnitFrUnitCd.FieldName = "fr_unit_cd";
            this.colUnitFrUnitCd.Name = "colUnitFrUnitCd";
            this.colUnitFrUnitCd.Visible = true;
            this.colUnitFrUnitCd.VisibleIndex = 0;
            this.colUnitFrUnitCd.Width = 90;
            // 
            // colUnitFrQty
            // 
            this.colUnitFrQty.Caption = "기준수량";
            this.colUnitFrQty.FieldName = "fr_qty";
            this.colUnitFrQty.Name = "colUnitFrQty";
            this.colUnitFrQty.Visible = true;
            this.colUnitFrQty.VisibleIndex = 1;
            this.colUnitFrQty.Width = 90;
            // 
            // colUnitToUnitCd
            // 
            this.colUnitToUnitCd.Caption = "환산단위";
            this.colUnitToUnitCd.ColumnEdit = this.repositoryItemLookUpEdit1;
            this.colUnitToUnitCd.FieldName = "to_unit_cd";
            this.colUnitToUnitCd.Name = "colUnitToUnitCd";
            this.colUnitToUnitCd.Visible = true;
            this.colUnitToUnitCd.VisibleIndex = 2;
            this.colUnitToUnitCd.Width = 90;
            // 
            // colUnitToQty
            // 
            this.colUnitToQty.Caption = "환산수량";
            this.colUnitToQty.FieldName = "to_qty";
            this.colUnitToQty.Name = "colUnitToQty";
            this.colUnitToQty.Visible = true;
            this.colUnitToQty.VisibleIndex = 3;
            this.colUnitToQty.Width = 90;
            // 
            // colUnitRemark
            // 
            this.colUnitRemark.Caption = "비고";
            this.colUnitRemark.FieldName = "remark";
            this.colUnitRemark.Name = "colUnitRemark";
            this.colUnitRemark.Visible = true;
            this.colUnitRemark.VisibleIndex = 4;
            this.colUnitRemark.Width = 200;
            // 
            // panelWyn1
            // 
            this.panelWyn1.Controls.Add(this.panelWyn7);
            this.panelWyn1.Controls.Add(this.sectionHeaderWyn2);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(3, 245);
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
            this.sectionHeaderWyn2.Text = "품목단위환산";
            // 
            // panelWyn5
            // 
            this.panelWyn5.Controls.Add(this.panData);
            this.panelWyn5.Controls.Add(this.panelWyn6);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(3, 0);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Size = new System.Drawing.Size(744, 245);
            this.panelWyn5.TabIndex = 6;
            // 
            // panData
            // 
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Controls.Add(this.lblItemCd);
            this.panData.Controls.Add(this.txtItemCd);
            this.panData.Controls.Add(this.lblItemNo);
            this.panData.Controls.Add(this.txtItemNo);
            this.panData.Controls.Add(this.lblItemNm);
            this.panData.Controls.Add(this.txtItemNm);
            this.panData.Controls.Add(this.lblItemSpec);
            this.panData.Controls.Add(this.txtItemSpec);
            this.panData.Controls.Add(this.lblUnitCd);
            this.panData.Controls.Add(this.cboUnitCd);
            this.panData.Controls.Add(this.lblPoUnitCd);
            this.panData.Controls.Add(this.cboAssetType);
            this.panData.Controls.Add(this.cboPoUnitCd);
            this.panData.Controls.Add(this.lblWhCd);
            this.panData.Controls.Add(this.txtWhCd);
            this.panData.Controls.Add(this.lblLocCd);
            this.panData.Controls.Add(this.txtLocCd);
            this.panData.Controls.Add(this.lblSafeQty);
            this.panData.Controls.Add(this.txtSafeQty);
            this.panData.Controls.Add(this.lblDeptCd);
            this.panData.Controls.Add(this.txtDeptCd);
            this.panData.Controls.Add(this.lblEmpNo);
            this.panData.Controls.Add(this.txtEmpNo);
            this.panData.Controls.Add(this.lblProdYn);
            this.panData.Controls.Add(this.txtProdYn);
            this.panData.Controls.Add(this.lblCustCd);
            this.panData.Controls.Add(this.txtCustCd);
            this.panData.Controls.Add(this.lblAssetType);
            this.panData.Controls.Add(this.lblOutType);
            this.panData.Controls.Add(this.txtOutType);
            this.panData.Controls.Add(this.lblPoQcYn);
            this.panData.Controls.Add(this.txtPoQcYn);
            this.panData.Controls.Add(this.lblProdQcYn);
            this.panData.Controls.Add(this.txtProdQcYn);
            this.panData.Controls.Add(this.lblLotYn);
            this.panData.Controls.Add(this.txtLotYn);
            this.panData.Controls.Add(this.lblStockYn);
            this.panData.Controls.Add(this.txtStockYn);
            this.panData.Controls.Add(this.lblPoYn);
            this.panData.Controls.Add(this.txtPoYn);
            this.panData.Controls.Add(this.lblPoPrice);
            this.panData.Controls.Add(this.txtPoPrice);
            this.panData.Controls.Add(this.lblSaleYn);
            this.panData.Controls.Add(this.txtSaleYn);
            this.panData.Controls.Add(this.lblSalePrice);
            this.panData.Controls.Add(this.txtSalePrice);
            this.panData.Controls.Add(this.lblStatCd);
            this.panData.Controls.Add(this.txtStatCd);
            this.panData.Controls.Add(this.lblItemClass1);
            this.panData.Controls.Add(this.txtItemClass1);
            this.panData.Controls.Add(this.lblItemClass2);
            this.panData.Controls.Add(this.txtItemClass2);
            this.panData.Controls.Add(this.lblItemClass3);
            this.panData.Controls.Add(this.txtItemClass3);
            this.panData.Controls.Add(this.lblItemClass4);
            this.panData.Controls.Add(this.txtItemClass4);
            this.panData.Controls.Add(this.lblPoAcntCd);
            this.panData.Controls.Add(this.txtPoAcntCd);
            this.panData.Controls.Add(this.lblSaleAcntCd);
            this.panData.Controls.Add(this.txtSaleAcntCd);
            this.panData.Controls.Add(this.lblRemark);
            this.panData.Controls.Add(this.txtRemark);
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(744, 218);
            this.panData.TabIndex = 8;
            // 
            // lblItemCd
            // 
            this.lblItemCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblItemCd.Location = new System.Drawing.Point(8, 18);
            this.lblItemCd.Name = "lblItemCd";
            this.lblItemCd.Size = new System.Drawing.Size(48, 15);
            this.lblItemCd.TabIndex = 0;
            this.lblItemCd.Text = "품목코드";
            // 
            // txtItemCd
            // 
            this.txtItemCd.Location = new System.Drawing.Point(82, 16);
            this.txtItemCd.Name = "txtItemCd";
            this.txtItemCd.Size = new System.Drawing.Size(108, 20);
            this.txtItemCd.TabIndex = 1;
            // 
            // lblItemNo
            // 
            this.lblItemNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblItemNo.Location = new System.Drawing.Point(200, 18);
            this.lblItemNo.Name = "lblItemNo";
            this.lblItemNo.Size = new System.Drawing.Size(24, 15);
            this.lblItemNo.TabIndex = 2;
            this.lblItemNo.Text = "품번";
            // 
            // txtItemNo
            // 
            this.txtItemNo.Location = new System.Drawing.Point(274, 16);
            this.txtItemNo.Name = "txtItemNo";
            this.txtItemNo.Size = new System.Drawing.Size(108, 20);
            this.txtItemNo.TabIndex = 3;
            // 
            // lblItemNm
            // 
            this.lblItemNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblItemNm.Location = new System.Drawing.Point(392, 18);
            this.lblItemNm.Name = "lblItemNm";
            this.lblItemNm.Size = new System.Drawing.Size(36, 15);
            this.lblItemNm.TabIndex = 4;
            this.lblItemNm.Text = "품목명";
            // 
            // txtItemNm
            // 
            this.txtItemNm.Location = new System.Drawing.Point(466, 16);
            this.txtItemNm.Name = "txtItemNm";
            this.txtItemNm.Size = new System.Drawing.Size(108, 20);
            this.txtItemNm.TabIndex = 5;
            // 
            // lblItemSpec
            // 
            this.lblItemSpec.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblItemSpec.Location = new System.Drawing.Point(584, 18);
            this.lblItemSpec.Name = "lblItemSpec";
            this.lblItemSpec.Size = new System.Drawing.Size(24, 15);
            this.lblItemSpec.TabIndex = 6;
            this.lblItemSpec.Text = "규격";
            // 
            // txtItemSpec
            // 
            this.txtItemSpec.Location = new System.Drawing.Point(658, 16);
            this.txtItemSpec.Name = "txtItemSpec";
            this.txtItemSpec.Size = new System.Drawing.Size(80, 20);
            this.txtItemSpec.TabIndex = 7;
            // 
            // lblUnitCd
            // 
            this.lblUnitCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblUnitCd.Location = new System.Drawing.Point(8, 45);
            this.lblUnitCd.Name = "lblUnitCd";
            this.lblUnitCd.Size = new System.Drawing.Size(48, 15);
            this.lblUnitCd.TabIndex = 8;
            this.lblUnitCd.Text = "기본단위";
            // 
            // cboUnitCd
            // 
            this.cboUnitCd.EditValue = "";
            this.cboUnitCd.Location = new System.Drawing.Point(82, 43);
            this.cboUnitCd.Name = "cboUnitCd";
            this.cboUnitCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboUnitCd.Properties.NullText = "";
            this.cboUnitCd.Size = new System.Drawing.Size(108, 20);
            this.cboUnitCd.TabIndex = 9;
            // 
            // lblPoUnitCd
            // 
            this.lblPoUnitCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPoUnitCd.Location = new System.Drawing.Point(200, 45);
            this.lblPoUnitCd.Name = "lblPoUnitCd";
            this.lblPoUnitCd.Size = new System.Drawing.Size(48, 15);
            this.lblPoUnitCd.TabIndex = 10;
            this.lblPoUnitCd.Text = "구매단위";
            // 
            // cboAssetType
            // 
            this.cboAssetType.EditValue = "";
            this.cboAssetType.Location = new System.Drawing.Point(274, 98);
            this.cboAssetType.Name = "cboAssetType";
            this.cboAssetType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboAssetType.Properties.NullText = "";
            this.cboAssetType.Size = new System.Drawing.Size(108, 20);
            this.cboAssetType.TabIndex = 11;
            // 
            // cboPoUnitCd
            // 
            this.cboPoUnitCd.EditValue = "";
            this.cboPoUnitCd.Location = new System.Drawing.Point(274, 43);
            this.cboPoUnitCd.Name = "cboPoUnitCd";
            this.cboPoUnitCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboPoUnitCd.Properties.NullText = "";
            this.cboPoUnitCd.Size = new System.Drawing.Size(108, 20);
            this.cboPoUnitCd.TabIndex = 11;
            // 
            // lblWhCd
            // 
            this.lblWhCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblWhCd.Location = new System.Drawing.Point(392, 45);
            this.lblWhCd.Name = "lblWhCd";
            this.lblWhCd.Size = new System.Drawing.Size(48, 15);
            this.lblWhCd.TabIndex = 12;
            this.lblWhCd.Text = "창고코드";
            // 
            // txtWhCd
            // 
            this.txtWhCd.Location = new System.Drawing.Point(466, 43);
            this.txtWhCd.Name = "txtWhCd";
            this.txtWhCd.Size = new System.Drawing.Size(108, 20);
            this.txtWhCd.TabIndex = 13;
            // 
            // lblLocCd
            // 
            this.lblLocCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLocCd.Location = new System.Drawing.Point(584, 45);
            this.lblLocCd.Name = "lblLocCd";
            this.lblLocCd.Size = new System.Drawing.Size(48, 15);
            this.lblLocCd.TabIndex = 14;
            this.lblLocCd.Text = "위치코드";
            // 
            // txtLocCd
            // 
            this.txtLocCd.Location = new System.Drawing.Point(658, 43);
            this.txtLocCd.Name = "txtLocCd";
            this.txtLocCd.Size = new System.Drawing.Size(80, 20);
            this.txtLocCd.TabIndex = 15;
            // 
            // lblSafeQty
            // 
            this.lblSafeQty.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSafeQty.Location = new System.Drawing.Point(8, 72);
            this.lblSafeQty.Name = "lblSafeQty";
            this.lblSafeQty.Size = new System.Drawing.Size(48, 15);
            this.lblSafeQty.TabIndex = 16;
            this.lblSafeQty.Text = "안전재고";
            // 
            // txtSafeQty
            // 
            this.txtSafeQty.Location = new System.Drawing.Point(82, 70);
            this.txtSafeQty.Name = "txtSafeQty";
            this.txtSafeQty.Size = new System.Drawing.Size(108, 20);
            this.txtSafeQty.TabIndex = 17;
            // 
            // lblDeptCd
            // 
            this.lblDeptCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDeptCd.Location = new System.Drawing.Point(200, 72);
            this.lblDeptCd.Name = "lblDeptCd";
            this.lblDeptCd.Size = new System.Drawing.Size(48, 15);
            this.lblDeptCd.TabIndex = 18;
            this.lblDeptCd.Text = "담당부서";
            // 
            // txtDeptCd
            // 
            this.txtDeptCd.Location = new System.Drawing.Point(274, 70);
            this.txtDeptCd.Name = "txtDeptCd";
            this.txtDeptCd.Size = new System.Drawing.Size(108, 20);
            this.txtDeptCd.TabIndex = 19;
            // 
            // lblEmpNo
            // 
            this.lblEmpNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblEmpNo.Location = new System.Drawing.Point(392, 72);
            this.lblEmpNo.Name = "lblEmpNo";
            this.lblEmpNo.Size = new System.Drawing.Size(36, 15);
            this.lblEmpNo.TabIndex = 20;
            this.lblEmpNo.Text = "담당자";
            // 
            // txtEmpNo
            // 
            this.txtEmpNo.Location = new System.Drawing.Point(466, 70);
            this.txtEmpNo.Name = "txtEmpNo";
            this.txtEmpNo.Size = new System.Drawing.Size(108, 20);
            this.txtEmpNo.TabIndex = 21;
            // 
            // lblProdYn
            // 
            this.lblProdYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblProdYn.Location = new System.Drawing.Point(584, 72);
            this.lblProdYn.Name = "lblProdYn";
            this.lblProdYn.Size = new System.Drawing.Size(48, 15);
            this.lblProdYn.TabIndex = 22;
            this.lblProdYn.Text = "생산여부";
            // 
            // txtProdYn
            // 
            this.txtProdYn.Location = new System.Drawing.Point(658, 70);
            this.txtProdYn.Name = "txtProdYn";
            this.txtProdYn.Size = new System.Drawing.Size(80, 20);
            this.txtProdYn.TabIndex = 23;
            // 
            // lblCustCd
            // 
            this.lblCustCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCustCd.Location = new System.Drawing.Point(8, 99);
            this.lblCustCd.Name = "lblCustCd";
            this.lblCustCd.Size = new System.Drawing.Size(60, 15);
            this.lblCustCd.TabIndex = 24;
            this.lblCustCd.Text = "거래처코드";
            // 
            // txtCustCd
            // 
            this.txtCustCd.Location = new System.Drawing.Point(82, 97);
            this.txtCustCd.Name = "txtCustCd";
            this.txtCustCd.Size = new System.Drawing.Size(108, 20);
            this.txtCustCd.TabIndex = 25;
            // 
            // lblAssetType
            // 
            this.lblAssetType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblAssetType.Location = new System.Drawing.Point(200, 99);
            this.lblAssetType.Name = "lblAssetType";
            this.lblAssetType.Size = new System.Drawing.Size(48, 15);
            this.lblAssetType.TabIndex = 26;
            this.lblAssetType.Text = "자산구분";
            // 
            // lblOutType
            // 
            this.lblOutType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblOutType.Location = new System.Drawing.Point(392, 99);
            this.lblOutType.Name = "lblOutType";
            this.lblOutType.Size = new System.Drawing.Size(48, 15);
            this.lblOutType.TabIndex = 28;
            this.lblOutType.Text = "출고구분";
            // 
            // txtOutType
            // 
            this.txtOutType.Location = new System.Drawing.Point(466, 97);
            this.txtOutType.Name = "txtOutType";
            this.txtOutType.Size = new System.Drawing.Size(108, 20);
            this.txtOutType.TabIndex = 29;
            // 
            // lblPoQcYn
            // 
            this.lblPoQcYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPoQcYn.Location = new System.Drawing.Point(584, 99);
            this.lblPoQcYn.Name = "lblPoQcYn";
            this.lblPoQcYn.Size = new System.Drawing.Size(48, 15);
            this.lblPoQcYn.TabIndex = 30;
            this.lblPoQcYn.Text = "구매검사";
            // 
            // txtPoQcYn
            // 
            this.txtPoQcYn.Location = new System.Drawing.Point(658, 97);
            this.txtPoQcYn.Name = "txtPoQcYn";
            this.txtPoQcYn.Size = new System.Drawing.Size(80, 20);
            this.txtPoQcYn.TabIndex = 31;
            // 
            // lblProdQcYn
            // 
            this.lblProdQcYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblProdQcYn.Location = new System.Drawing.Point(8, 126);
            this.lblProdQcYn.Name = "lblProdQcYn";
            this.lblProdQcYn.Size = new System.Drawing.Size(48, 15);
            this.lblProdQcYn.TabIndex = 32;
            this.lblProdQcYn.Text = "생산검사";
            // 
            // txtProdQcYn
            // 
            this.txtProdQcYn.Location = new System.Drawing.Point(82, 124);
            this.txtProdQcYn.Name = "txtProdQcYn";
            this.txtProdQcYn.Size = new System.Drawing.Size(108, 20);
            this.txtProdQcYn.TabIndex = 33;
            // 
            // lblLotYn
            // 
            this.lblLotYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLotYn.Location = new System.Drawing.Point(200, 126);
            this.lblLotYn.Name = "lblLotYn";
            this.lblLotYn.Size = new System.Drawing.Size(45, 15);
            this.lblLotYn.TabIndex = 34;
            this.lblLotYn.Text = "LOT여부";
            // 
            // txtLotYn
            // 
            this.txtLotYn.Location = new System.Drawing.Point(274, 124);
            this.txtLotYn.Name = "txtLotYn";
            this.txtLotYn.Size = new System.Drawing.Size(108, 20);
            this.txtLotYn.TabIndex = 35;
            // 
            // lblStockYn
            // 
            this.lblStockYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblStockYn.Location = new System.Drawing.Point(392, 126);
            this.lblStockYn.Name = "lblStockYn";
            this.lblStockYn.Size = new System.Drawing.Size(48, 15);
            this.lblStockYn.TabIndex = 36;
            this.lblStockYn.Text = "재고관리";
            // 
            // txtStockYn
            // 
            this.txtStockYn.Location = new System.Drawing.Point(466, 124);
            this.txtStockYn.Name = "txtStockYn";
            this.txtStockYn.Size = new System.Drawing.Size(108, 20);
            this.txtStockYn.TabIndex = 37;
            // 
            // lblPoYn
            // 
            this.lblPoYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPoYn.Location = new System.Drawing.Point(584, 126);
            this.lblPoYn.Name = "lblPoYn";
            this.lblPoYn.Size = new System.Drawing.Size(48, 15);
            this.lblPoYn.TabIndex = 38;
            this.lblPoYn.Text = "구매여부";
            // 
            // txtPoYn
            // 
            this.txtPoYn.Location = new System.Drawing.Point(658, 124);
            this.txtPoYn.Name = "txtPoYn";
            this.txtPoYn.Size = new System.Drawing.Size(80, 20);
            this.txtPoYn.TabIndex = 39;
            // 
            // lblPoPrice
            // 
            this.lblPoPrice.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPoPrice.Location = new System.Drawing.Point(8, 153);
            this.lblPoPrice.Name = "lblPoPrice";
            this.lblPoPrice.Size = new System.Drawing.Size(48, 15);
            this.lblPoPrice.TabIndex = 40;
            this.lblPoPrice.Text = "구매단가";
            // 
            // txtPoPrice
            // 
            this.txtPoPrice.Location = new System.Drawing.Point(82, 151);
            this.txtPoPrice.Name = "txtPoPrice";
            this.txtPoPrice.Size = new System.Drawing.Size(108, 20);
            this.txtPoPrice.TabIndex = 41;
            // 
            // lblSaleYn
            // 
            this.lblSaleYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSaleYn.Location = new System.Drawing.Point(200, 153);
            this.lblSaleYn.Name = "lblSaleYn";
            this.lblSaleYn.Size = new System.Drawing.Size(48, 15);
            this.lblSaleYn.TabIndex = 42;
            this.lblSaleYn.Text = "판매여부";
            // 
            // txtSaleYn
            // 
            this.txtSaleYn.Location = new System.Drawing.Point(274, 151);
            this.txtSaleYn.Name = "txtSaleYn";
            this.txtSaleYn.Size = new System.Drawing.Size(108, 20);
            this.txtSaleYn.TabIndex = 43;
            // 
            // lblSalePrice
            // 
            this.lblSalePrice.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSalePrice.Location = new System.Drawing.Point(392, 153);
            this.lblSalePrice.Name = "lblSalePrice";
            this.lblSalePrice.Size = new System.Drawing.Size(48, 15);
            this.lblSalePrice.TabIndex = 44;
            this.lblSalePrice.Text = "판매단가";
            // 
            // txtSalePrice
            // 
            this.txtSalePrice.Location = new System.Drawing.Point(466, 151);
            this.txtSalePrice.Name = "txtSalePrice";
            this.txtSalePrice.Size = new System.Drawing.Size(108, 20);
            this.txtSalePrice.TabIndex = 45;
            // 
            // lblStatCd
            // 
            this.lblStatCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblStatCd.Location = new System.Drawing.Point(584, 153);
            this.lblStatCd.Name = "lblStatCd";
            this.lblStatCd.Size = new System.Drawing.Size(48, 15);
            this.lblStatCd.TabIndex = 46;
            this.lblStatCd.Text = "상태코드";
            // 
            // txtStatCd
            // 
            this.txtStatCd.Location = new System.Drawing.Point(658, 151);
            this.txtStatCd.Name = "txtStatCd";
            this.txtStatCd.Size = new System.Drawing.Size(80, 20);
            this.txtStatCd.TabIndex = 47;
            // 
            // lblItemClass1
            // 
            this.lblItemClass1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblItemClass1.Location = new System.Drawing.Point(8, 180);
            this.lblItemClass1.Name = "lblItemClass1";
            this.lblItemClass1.Size = new System.Drawing.Size(55, 15);
            this.lblItemClass1.TabIndex = 48;
            this.lblItemClass1.Text = "품목분류1";
            // 
            // txtItemClass1
            // 
            this.txtItemClass1.Location = new System.Drawing.Point(82, 178);
            this.txtItemClass1.Name = "txtItemClass1";
            this.txtItemClass1.Size = new System.Drawing.Size(108, 20);
            this.txtItemClass1.TabIndex = 49;
            // 
            // lblItemClass2
            // 
            this.lblItemClass2.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblItemClass2.Location = new System.Drawing.Point(200, 180);
            this.lblItemClass2.Name = "lblItemClass2";
            this.lblItemClass2.Size = new System.Drawing.Size(55, 15);
            this.lblItemClass2.TabIndex = 50;
            this.lblItemClass2.Text = "품목분류2";
            // 
            // txtItemClass2
            // 
            this.txtItemClass2.Location = new System.Drawing.Point(274, 178);
            this.txtItemClass2.Name = "txtItemClass2";
            this.txtItemClass2.Size = new System.Drawing.Size(108, 20);
            this.txtItemClass2.TabIndex = 51;
            // 
            // lblItemClass3
            // 
            this.lblItemClass3.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblItemClass3.Location = new System.Drawing.Point(392, 180);
            this.lblItemClass3.Name = "lblItemClass3";
            this.lblItemClass3.Size = new System.Drawing.Size(55, 15);
            this.lblItemClass3.TabIndex = 52;
            this.lblItemClass3.Text = "품목분류3";
            // 
            // txtItemClass3
            // 
            this.txtItemClass3.Location = new System.Drawing.Point(466, 178);
            this.txtItemClass3.Name = "txtItemClass3";
            this.txtItemClass3.Size = new System.Drawing.Size(108, 20);
            this.txtItemClass3.TabIndex = 53;
            // 
            // lblItemClass4
            // 
            this.lblItemClass4.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblItemClass4.Location = new System.Drawing.Point(584, 180);
            this.lblItemClass4.Name = "lblItemClass4";
            this.lblItemClass4.Size = new System.Drawing.Size(55, 15);
            this.lblItemClass4.TabIndex = 54;
            this.lblItemClass4.Text = "품목분류4";
            // 
            // txtItemClass4
            // 
            this.txtItemClass4.Location = new System.Drawing.Point(658, 178);
            this.txtItemClass4.Name = "txtItemClass4";
            this.txtItemClass4.Size = new System.Drawing.Size(80, 20);
            this.txtItemClass4.TabIndex = 55;
            // 
            // lblPoAcntCd
            // 
            this.lblPoAcntCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPoAcntCd.Location = new System.Drawing.Point(8, 207);
            this.lblPoAcntCd.Name = "lblPoAcntCd";
            this.lblPoAcntCd.Size = new System.Drawing.Size(48, 15);
            this.lblPoAcntCd.TabIndex = 56;
            this.lblPoAcntCd.Text = "매입계정";
            // 
            // txtPoAcntCd
            // 
            this.txtPoAcntCd.Location = new System.Drawing.Point(82, 205);
            this.txtPoAcntCd.Name = "txtPoAcntCd";
            this.txtPoAcntCd.Size = new System.Drawing.Size(108, 20);
            this.txtPoAcntCd.TabIndex = 57;
            // 
            // lblSaleAcntCd
            // 
            this.lblSaleAcntCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSaleAcntCd.Location = new System.Drawing.Point(200, 207);
            this.lblSaleAcntCd.Name = "lblSaleAcntCd";
            this.lblSaleAcntCd.Size = new System.Drawing.Size(48, 15);
            this.lblSaleAcntCd.TabIndex = 58;
            this.lblSaleAcntCd.Text = "매출계정";
            // 
            // txtSaleAcntCd
            // 
            this.txtSaleAcntCd.Location = new System.Drawing.Point(274, 205);
            this.txtSaleAcntCd.Name = "txtSaleAcntCd";
            this.txtSaleAcntCd.Size = new System.Drawing.Size(108, 20);
            this.txtSaleAcntCd.TabIndex = 59;
            // 
            // lblRemark
            // 
            this.lblRemark.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRemark.Location = new System.Drawing.Point(392, 207);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(24, 15);
            this.lblRemark.TabIndex = 60;
            this.lblRemark.Text = "비고";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(466, 205);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(272, 20);
            this.txtRemark.TabIndex = 61;
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
            this.sectionHeaderWyn3.Text = "품목 상세";
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
            // grd1
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
            this.colListItemId,
            this.colListItemCd,
            this.colListItemNm,
            this.colListUnitCd,
            this.colListStatCd});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightFocusedRow = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsSelection.InvertSelection = true;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // colListItemId
            // 
            this.colListItemId.Caption = "item_id";
            this.colListItemId.FieldName = "item_id";
            this.colListItemId.Name = "colListItemId";
            // 
            // colListItemCd
            // 
            this.colListItemCd.Caption = "품목코드";
            this.colListItemCd.FieldName = "item_cd";
            this.colListItemCd.Name = "colListItemCd";
            this.colListItemCd.Visible = true;
            this.colListItemCd.VisibleIndex = 0;
            this.colListItemCd.Width = 100;
            // 
            // colListItemNm
            // 
            this.colListItemNm.Caption = "품목명";
            this.colListItemNm.FieldName = "item_nm";
            this.colListItemNm.Name = "colListItemNm";
            this.colListItemNm.Visible = true;
            this.colListItemNm.VisibleIndex = 1;
            this.colListItemNm.Width = 152;
            // 
            // colListUnitCd
            // 
            this.colListUnitCd.Caption = "단위";
            this.colListUnitCd.FieldName = "unit_cd";
            this.colListUnitCd.Name = "colListUnitCd";
            this.colListUnitCd.Visible = true;
            this.colListUnitCd.VisibleIndex = 2;
            this.colListUnitCd.Width = 70;
            // 
            // colListStatCd
            // 
            this.colListStatCd.Caption = "상태";
            this.colListStatCd.FieldName = "stat_cd";
            this.colListStatCd.Name = "colListStatCd";
            this.colListStatCd.Visible = true;
            this.colListStatCd.VisibleIndex = 3;
            this.colListStatCd.Width = 60;
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
            this.sectionHeaderWyn4.Text = "목록";
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
            this.labelControl1.Size = new System.Drawing.Size(48, 15);
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
            this.sectionHeaderWyn1.Text = "품목등록 [frmItem]";
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            // 
            // frmItem
            // 
            this.ClientSize = new System.Drawing.Size(1165, 600);
            this.Controls.Add(this.panBase);
            this.Name = "frmItem";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
            this.panelWyn7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
            this.panData.ResumeLayout(false);
            this.panData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemSpec.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnitCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAssetType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPoUnitCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWhCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSafeQty.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProdYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPoQcYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProdQcYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLotYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStockYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPoYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPoPrice.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaleYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSalePrice.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStatCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemClass1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemClass2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemClass3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemClass4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPoAcntCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaleAcntCd.Properties)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            this.ResumeLayout(false);

    }

    private PanelWyn paTitle;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn4;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblItemCd;
    private TextEditWyn txtItemCd;
    private DevExpress.XtraEditors.LabelControl lblItemNo;
    private TextEditWyn txtItemNo;
    private DevExpress.XtraEditors.LabelControl lblItemNm;
    private TextEditWyn txtItemNm;
    private DevExpress.XtraEditors.LabelControl lblItemSpec;
    private TextEditWyn txtItemSpec;
    private DevExpress.XtraEditors.LabelControl lblUnitCd;
    private LookUpEditWyn cboUnitCd;
    private DevExpress.XtraEditors.LabelControl lblPoUnitCd;
    private LookUpEditWyn cboPoUnitCd;
    private DevExpress.XtraEditors.LabelControl lblWhCd;
    private TextEditWyn txtWhCd;
    private DevExpress.XtraEditors.LabelControl lblLocCd;
    private TextEditWyn txtLocCd;
    private DevExpress.XtraEditors.LabelControl lblSafeQty;
    private TextEditWyn txtSafeQty;
    private DevExpress.XtraEditors.LabelControl lblDeptCd;
    private TextEditWyn txtDeptCd;
    private DevExpress.XtraEditors.LabelControl lblEmpNo;
    private TextEditWyn txtEmpNo;
    private DevExpress.XtraEditors.LabelControl lblProdYn;
    private TextEditWyn txtProdYn;
    private DevExpress.XtraEditors.LabelControl lblCustCd;
    private TextEditWyn txtCustCd;
    private DevExpress.XtraEditors.LabelControl lblAssetType;
    private DevExpress.XtraEditors.LabelControl lblOutType;
    private TextEditWyn txtOutType;
    private DevExpress.XtraEditors.LabelControl lblPoQcYn;
    private TextEditWyn txtPoQcYn;
    private DevExpress.XtraEditors.LabelControl lblProdQcYn;
    private TextEditWyn txtProdQcYn;
    private DevExpress.XtraEditors.LabelControl lblLotYn;
    private TextEditWyn txtLotYn;
    private DevExpress.XtraEditors.LabelControl lblStockYn;
    private TextEditWyn txtStockYn;
    private DevExpress.XtraEditors.LabelControl lblPoYn;
    private TextEditWyn txtPoYn;
    private DevExpress.XtraEditors.LabelControl lblPoPrice;
    private TextEditWyn txtPoPrice;
    private DevExpress.XtraEditors.LabelControl lblSaleYn;
    private TextEditWyn txtSaleYn;
    private DevExpress.XtraEditors.LabelControl lblSalePrice;
    private TextEditWyn txtSalePrice;
    private DevExpress.XtraEditors.LabelControl lblStatCd;
    private TextEditWyn txtStatCd;
    private DevExpress.XtraEditors.LabelControl lblItemClass1;
    private TextEditWyn txtItemClass1;
    private DevExpress.XtraEditors.LabelControl lblItemClass2;
    private TextEditWyn txtItemClass2;
    private DevExpress.XtraEditors.LabelControl lblItemClass3;
    private TextEditWyn txtItemClass3;
    private DevExpress.XtraEditors.LabelControl lblItemClass4;
    private TextEditWyn txtItemClass4;
    private DevExpress.XtraEditors.LabelControl lblPoAcntCd;
    private TextEditWyn txtPoAcntCd;
    private DevExpress.XtraEditors.LabelControl lblSaleAcntCd;
    private TextEditWyn txtSaleAcntCd;
    private DevExpress.XtraEditors.LabelControl lblRemark;
    private TextEditWyn txtRemark;
    private PanelWyn panelWyn6;
    private SectionHeaderWyn sectionHeaderWyn1;
    private SplitterWyn splitterWyn1;
    private SectionHeaderWyn sectionHeaderWyn3;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colListItemId;
    private DevExpress.XtraGrid.Columns.GridColumn colListItemCd;
    private DevExpress.XtraGrid.Columns.GridColumn colListItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colListUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colListStatCd;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitFrUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitFrQty;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitToUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitToQty;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitRemark;
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
    private LookUpEditWyn cboAssetType;
    private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
}
