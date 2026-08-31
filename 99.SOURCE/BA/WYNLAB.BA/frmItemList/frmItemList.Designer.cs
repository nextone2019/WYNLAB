// VS 디자이너가 자동 생성하는 필드 선언에는 = null!을 안 붙여서 nullable 경고(CS8618)가
// 계속 나기 때문에, 이 파일(디자이너 전용)만 nullable 검사를 끈다 - 흔한 관례.
#nullable disable
using DevExpress.XtraGrid;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

/// <summary>
/// 품목현황 화면 - 조회 전용 목록 하나만 있는 화면이라(등록/수정은 frmItem에서 한다),
/// 다른 화면들의 좌측 목록/우측 상세 2단 구성과 달리 grd1이 본문 전체를 채운다.
/// 컬럼을 더 늘리거나 줄이려면 gvw1.Columns에 GridColumn을 추가/제거하면 된다.
/// </summary>
public partial class frmItemList
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
        this.panBase = new WYNLAB.Base.Controls.PanelWyn();
        this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.colItemCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colWhCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLocCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colSafeQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDeptCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colEmpNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colStockYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colSaleYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colSalePrice = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPoYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPoPrice = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.txtSearchQ = new WYNLAB.Base.Controls.TextEditWyn();
        this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
        this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        this.panBase.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
        this.panelWyn3.SuspendLayout();
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
        this.panBase.Appearance.Options.UseBackColor = true;
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
        // panelWyn3 (본문 - 목록 전체 폭)
        //
        this.panelWyn3.Controls.Add(this.grd1);
        this.panelWyn3.Controls.Add(this.panelWyn2);
        this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panelWyn3.Location = new System.Drawing.Point(3, 82);
        this.panelWyn3.Name = "panelWyn3";
        this.panelWyn3.Size = new System.Drawing.Size(1159, 515);
        this.panelWyn3.TabIndex = 7;
        //
        // grd1 (목록 - 조회전용)
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd1.Location = new System.Drawing.Point(0, 27);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.Size = new System.Drawing.Size(1159, 488);
        this.grd1.TabIndex = 10;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colItemCd,
        this.colItemNm,
        this.colItemSpec,
        this.colUnitCd,
        this.colWhCd,
        this.colLocCd,
        this.colSafeQty,
        this.colDeptCd,
        this.colEmpNo,
        this.colStockYn,
        this.colSaleYn,
        this.colSalePrice,
        this.colPoYn,
        this.colPoPrice,
        this.colStatCd,
        this.colRemark});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsSelection.InvertSelection = true;
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        //
        // colItemCd (품목코드)
        //
        this.colItemCd.Caption = "품목코드";
        this.colItemCd.FieldName = "item_cd";
        this.colItemCd.Name = "colItemCd";
        this.colItemCd.Visible = true;
        this.colItemCd.VisibleIndex = 0;
        this.colItemCd.Width = 100;
        //
        // colItemNm (품목명)
        //
        this.colItemNm.Caption = "품목명";
        this.colItemNm.FieldName = "item_nm";
        this.colItemNm.Name = "colItemNm";
        this.colItemNm.Visible = true;
        this.colItemNm.VisibleIndex = 1;
        this.colItemNm.Width = 150;
        //
        // colItemSpec (규격)
        //
        this.colItemSpec.Caption = "규격";
        this.colItemSpec.FieldName = "item_spec";
        this.colItemSpec.Name = "colItemSpec";
        this.colItemSpec.Visible = true;
        this.colItemSpec.VisibleIndex = 2;
        this.colItemSpec.Width = 120;
        //
        // colUnitCd (단위)
        //
        this.colUnitCd.Caption = "단위";
        this.colUnitCd.FieldName = "unit_cd";
        this.colUnitCd.Name = "colUnitCd";
        this.colUnitCd.Visible = true;
        this.colUnitCd.VisibleIndex = 3;
        this.colUnitCd.Width = 60;
        //
        // colWhCd (창고)
        //
        this.colWhCd.Caption = "창고";
        this.colWhCd.FieldName = "wh_cd";
        this.colWhCd.Name = "colWhCd";
        this.colWhCd.Visible = true;
        this.colWhCd.VisibleIndex = 4;
        this.colWhCd.Width = 80;
        //
        // colLocCd (로케이션)
        //
        this.colLocCd.Caption = "로케이션";
        this.colLocCd.FieldName = "loc_cd";
        this.colLocCd.Name = "colLocCd";
        this.colLocCd.Visible = true;
        this.colLocCd.VisibleIndex = 5;
        this.colLocCd.Width = 80;
        //
        // colSafeQty (안전재고)
        //
        this.colSafeQty.Caption = "안전재고";
        this.colSafeQty.FieldName = "safe_qty";
        this.colSafeQty.Name = "colSafeQty";
        this.colSafeQty.Visible = true;
        this.colSafeQty.VisibleIndex = 6;
        this.colSafeQty.Width = 80;
        //
        // colDeptCd (담당부서)
        //
        this.colDeptCd.Caption = "담당부서";
        this.colDeptCd.FieldName = "dept_cd";
        this.colDeptCd.Name = "colDeptCd";
        this.colDeptCd.Visible = true;
        this.colDeptCd.VisibleIndex = 7;
        this.colDeptCd.Width = 80;
        //
        // colEmpNo (담당자)
        //
        this.colEmpNo.Caption = "담당자";
        this.colEmpNo.FieldName = "emp_no";
        this.colEmpNo.Name = "colEmpNo";
        this.colEmpNo.Visible = true;
        this.colEmpNo.VisibleIndex = 8;
        this.colEmpNo.Width = 80;
        //
        // colStockYn (재고관리)
        //
        this.colStockYn.Caption = "재고관리";
        this.colStockYn.FieldName = "stock_yn";
        this.colStockYn.Name = "colStockYn";
        this.colStockYn.Visible = true;
        this.colStockYn.VisibleIndex = 9;
        this.colStockYn.Width = 70;
        //
        // colSaleYn (판매여부)
        //
        this.colSaleYn.Caption = "판매여부";
        this.colSaleYn.FieldName = "sale_yn";
        this.colSaleYn.Name = "colSaleYn";
        this.colSaleYn.Visible = true;
        this.colSaleYn.VisibleIndex = 10;
        this.colSaleYn.Width = 70;
        //
        // colSalePrice (판매단가)
        //
        this.colSalePrice.Caption = "판매단가";
        this.colSalePrice.FieldName = "sale_price";
        this.colSalePrice.Name = "colSalePrice";
        this.colSalePrice.Visible = true;
        this.colSalePrice.VisibleIndex = 11;
        this.colSalePrice.Width = 90;
        //
        // colPoYn (구매여부)
        //
        this.colPoYn.Caption = "구매여부";
        this.colPoYn.FieldName = "po_yn";
        this.colPoYn.Name = "colPoYn";
        this.colPoYn.Visible = true;
        this.colPoYn.VisibleIndex = 12;
        this.colPoYn.Width = 70;
        //
        // colPoPrice (구매단가)
        //
        this.colPoPrice.Caption = "구매단가";
        this.colPoPrice.FieldName = "po_price";
        this.colPoPrice.Name = "colPoPrice";
        this.colPoPrice.Visible = true;
        this.colPoPrice.VisibleIndex = 13;
        this.colPoPrice.Width = 90;
        //
        // colStatCd (상태)
        //
        this.colStatCd.Caption = "상태";
        this.colStatCd.FieldName = "stat_cd";
        this.colStatCd.Name = "colStatCd";
        this.colStatCd.Visible = true;
        this.colStatCd.VisibleIndex = 14;
        this.colStatCd.Width = 70;
        //
        // colRemark (비고)
        //
        this.colRemark.Caption = "비고";
        this.colRemark.FieldName = "remark";
        this.colRemark.Name = "colRemark";
        this.colRemark.Visible = true;
        this.colRemark.VisibleIndex = 15;
        this.colRemark.Width = 150;
        //
        // panelWyn2
        //
        this.panelWyn2.Controls.Add(this.sectionHeaderWyn4);
        this.panelWyn2.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn2.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panelWyn2.Location = new System.Drawing.Point(0, 0);
        this.panelWyn2.Name = "panelWyn2";
        this.panelWyn2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
        this.panelWyn2.Size = new System.Drawing.Size(1159, 27);
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
        this.sectionHeaderWyn4.Size = new System.Drawing.Size(1154, 25);
        this.sectionHeaderWyn4.TabIndex = 8;
        this.sectionHeaderWyn4.Text = "목록";
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
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
        this.labelControl1.Appearance.Options.UseFont = true;
        this.labelControl1.Location = new System.Drawing.Point(25, 18);
        this.labelControl1.Name = "labelControl1";
        this.labelControl1.Size = new System.Drawing.Size(77, 15);
        this.labelControl1.TabIndex = 0;
        this.labelControl1.Text = "품목코드/명";
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
        this.sectionHeaderWyn1.TabIndex = 8;
        this.sectionHeaderWyn1.Text = "품목현황";
        //
        // frmItemList
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1165, 600);
        this.Controls.Add(this.panBase);
        this.Name = "frmItemList";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
        this.panelWyn3.ResumeLayout(false);
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
    private SectionHeaderWyn sectionHeaderWyn1;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colItemCd;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colWhCd;
    private DevExpress.XtraGrid.Columns.GridColumn colLocCd;
    private DevExpress.XtraGrid.Columns.GridColumn colSafeQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDeptCd;
    private DevExpress.XtraGrid.Columns.GridColumn colEmpNo;
    private DevExpress.XtraGrid.Columns.GridColumn colStockYn;
    private DevExpress.XtraGrid.Columns.GridColumn colSaleYn;
    private DevExpress.XtraGrid.Columns.GridColumn colSalePrice;
    private DevExpress.XtraGrid.Columns.GridColumn colPoYn;
    private DevExpress.XtraGrid.Columns.GridColumn colPoPrice;
    private DevExpress.XtraGrid.Columns.GridColumn colStatCd;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private PanelWyn panelWyn2;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn panHeader;
    private TextEditWyn txtSearchQ;
    private DevExpress.XtraEditors.LabelControl labelControl1;
}
