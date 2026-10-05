#nullable disable
using DevExpress.XtraGrid;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmItemMulti
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private PanelWyn panBase;
    private PanelWyn paTitle;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private LookUpEditWyn cboAccId;
    private ButtonWyn btnDownloadTemplate;
    private ButtonWyn btnUploadExcel;
    private ButtonWyn btnValidate;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn2;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colPoUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colSafeQty;
    private DevExpress.XtraGrid.Columns.GridColumn colWhNm;
    private DevExpress.XtraGrid.Columns.GridColumn colLocNm;
    private DevExpress.XtraGrid.Columns.GridColumn colDeptNm;
    private DevExpress.XtraGrid.Columns.GridColumn colEmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn colCustNm;
    private DevExpress.XtraGrid.Columns.GridColumn colAssetType;
    private DevExpress.XtraGrid.Columns.GridColumn colOutType;
    private DevExpress.XtraGrid.Columns.GridColumn colStatCd;
    private DevExpress.XtraGrid.Columns.GridColumn colPoQcYn;
    private DevExpress.XtraGrid.Columns.GridColumn colProdQcYn;
    private DevExpress.XtraGrid.Columns.GridColumn colLotYn;
    private DevExpress.XtraGrid.Columns.GridColumn colStockYn;
    private DevExpress.XtraGrid.Columns.GridColumn colGrp1Nm;
    private DevExpress.XtraGrid.Columns.GridColumn colGrp2Nm;
    private DevExpress.XtraGrid.Columns.GridColumn colGrp3Nm;
    private DevExpress.XtraGrid.Columns.GridColumn colGrp4Nm;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private DevExpress.XtraGrid.Columns.GridColumn colValidateResult;
    private LookUpColumnEdit lookUpUnitCd;
    private LookUpColumnEdit lookUpAssetType;
    private LookUpColumnEdit lookUpStatCd;
    private PopupLookupColumnEdit popcolWh;
    private PopupLookupColumnEdit popcolLoc;
    private PopupLookupColumnEdit popcolDept;
    private PopupLookupColumnEdit popcolEmp;
    private PopupLookupColumnEdit popcolCust;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkYn;

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemMulti));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpUnitCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colPoUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSafeQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLocNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDeptNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCustNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAssetType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpAssetType = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colOutType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpStatCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.popcolWh = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
            this.popcolLoc = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
            this.popcolDept = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
            this.popcolEmp = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
            this.popcolCust = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
            this.colPoQcYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkYn = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colProdQcYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLotYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStockYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGrp1Nm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGrp2Nm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGrp3Nm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGrp4Nm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colValidateResult = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.btnValidate = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnUploadExcel = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnDownloadTemplate = new WYNLAB.Base.Controls.ButtonWyn();
            this.cboAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
            this.panelWyn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpUnitCd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpAssetType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpStatCd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolWh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolLoc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolDept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolEmp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolCust)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkYn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
            this.panelWyn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).BeginInit();
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
            this.panBase.Size = new System.Drawing.Size(1300, 620);
            this.panBase.TabIndex = 0;
            // 
            // panelWyn3
            // 
            this.panelWyn3.Controls.Add(this.grd1);
            this.panelWyn3.Controls.Add(this.panelWyn2);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(3, 82);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelWyn3.Size = new System.Drawing.Size(1294, 535);
            this.panelWyn3.TabIndex = 3;
            // 
            // grd1
            // 
            this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd1.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grd1.Location = new System.Drawing.Point(0, 35);
            this.grd1.MainView = this.gvw1;
            this.grd1.Name = "grd1";
            this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lookUpUnitCd,
            this.lookUpAssetType,
            this.lookUpStatCd,
            this.popcolWh,
            this.popcolLoc,
            this.popcolDept,
            this.popcolEmp,
            this.popcolCust,
            this.chkYn});
            this.grd1.Size = new System.Drawing.Size(1294, 500);
            this.grd1.TabIndex = 1;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colItemNo,
            this.colItemNm,
            this.colItemSpec,
            this.colUnitCd,
            this.colPoUnitCd,
            this.colSafeQty,
            this.colWhNm,
            this.colLocNm,
            this.colDeptNm,
            this.colEmpNm,
            this.colCustNm,
            this.colAssetType,
            this.colOutType,
            this.colStatCd,
            this.colPoQcYn,
            this.colProdQcYn,
            this.colLotYn,
            this.colStockYn,
            this.colGrp1Nm,
            this.colGrp2Nm,
            this.colGrp3Nm,
            this.colGrp4Nm,
            this.colRemark,
            this.colValidateResult});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightUnsavedCells = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.RequiredFields = "item_no,item_nm,unit_cd,asset_type";
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // colItemNo
            // 
            this.colItemNo.Caption = "품목코드";
            this.colItemNo.FieldName = "item_no";
            this.colItemNo.Name = "colItemNo";
            this.colItemNo.Visible = true;
            this.colItemNo.VisibleIndex = 0;
            this.colItemNo.Width = 100;
            // 
            // colItemNm
            // 
            this.colItemNm.Caption = "품목명";
            this.colItemNm.FieldName = "item_nm";
            this.colItemNm.Name = "colItemNm";
            this.colItemNm.Visible = true;
            this.colItemNm.VisibleIndex = 1;
            this.colItemNm.Width = 150;
            // 
            // colItemSpec
            // 
            this.colItemSpec.Caption = "규격";
            this.colItemSpec.FieldName = "item_spec";
            this.colItemSpec.Name = "colItemSpec";
            this.colItemSpec.Visible = true;
            this.colItemSpec.VisibleIndex = 2;
            this.colItemSpec.Width = 100;
            // 
            // colUnitCd
            // 
            this.colUnitCd.Caption = "재고단위";
            this.colUnitCd.ColumnEdit = this.lookUpUnitCd;
            this.colUnitCd.FieldName = "unit_cd";
            this.colUnitCd.Name = "colUnitCd";
            this.colUnitCd.Visible = true;
            this.colUnitCd.VisibleIndex = 3;
            this.colUnitCd.Width = 70;
            // 
            // lookUpUnitCd
            // 
            this.lookUpUnitCd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpUnitCd.LookupKey = "L_CM0001";
            this.lookUpUnitCd.Name = "lookUpUnitCd";
            this.lookUpUnitCd.NullText = "";
            this.lookUpUnitCd.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colPoUnitCd
            // 
            this.colPoUnitCd.Caption = "구매단위";
            this.colPoUnitCd.ColumnEdit = this.lookUpUnitCd;
            this.colPoUnitCd.FieldName = "po_unit_cd";
            this.colPoUnitCd.Name = "colPoUnitCd";
            this.colPoUnitCd.Visible = true;
            this.colPoUnitCd.VisibleIndex = 4;
            this.colPoUnitCd.Width = 70;
            // 
            // colSafeQty
            // 
            this.colSafeQty.Caption = "안전재고수량";
            this.colSafeQty.FieldName = "safe_qty";
            this.colSafeQty.Name = "colSafeQty";
            this.colSafeQty.Visible = true;
            this.colSafeQty.VisibleIndex = 5;
            this.colSafeQty.Width = 90;
            // 
            // colWhNm
            // 
            this.colWhNm.Caption = "창고";
            this.colWhNm.ColumnEdit = this.popcolWh;
            this.colWhNm.FieldName = "wh_nm";
            this.colWhNm.Name = "colWhNm";
            this.colWhNm.Visible = true;
            this.colWhNm.VisibleIndex = 6;
            this.colWhNm.Width = 90;
            // 
            // colLocNm
            // 
            this.colLocNm.Caption = "위치";
            this.colLocNm.ColumnEdit = this.popcolLoc;
            this.colLocNm.FieldName = "loc_nm";
            this.colLocNm.Name = "colLocNm";
            this.colLocNm.Visible = true;
            this.colLocNm.VisibleIndex = 7;
            this.colLocNm.Width = 90;
            // 
            // colDeptNm
            // 
            this.colDeptNm.Caption = "담당부서";
            this.colDeptNm.ColumnEdit = this.popcolDept;
            this.colDeptNm.FieldName = "dept_nm";
            this.colDeptNm.Name = "colDeptNm";
            this.colDeptNm.Visible = true;
            this.colDeptNm.VisibleIndex = 8;
            this.colDeptNm.Width = 90;
            // 
            // colEmpNm
            // 
            this.colEmpNm.Caption = "담당자";
            this.colEmpNm.ColumnEdit = this.popcolEmp;
            this.colEmpNm.FieldName = "emp_nm";
            this.colEmpNm.Name = "colEmpNm";
            this.colEmpNm.Visible = true;
            this.colEmpNm.VisibleIndex = 9;
            this.colEmpNm.Width = 80;
            // 
            // colCustNm
            // 
            this.colCustNm.Caption = "구매처";
            this.colCustNm.ColumnEdit = this.popcolCust;
            this.colCustNm.FieldName = "cust_nm";
            this.colCustNm.Name = "colCustNm";
            this.colCustNm.Visible = true;
            this.colCustNm.VisibleIndex = 10;
            this.colCustNm.Width = 90;
            // 
            // colAssetType
            // 
            this.colAssetType.Caption = "자산구분";
            this.colAssetType.ColumnEdit = this.lookUpAssetType;
            this.colAssetType.FieldName = "asset_type";
            this.colAssetType.Name = "colAssetType";
            this.colAssetType.Visible = true;
            this.colAssetType.VisibleIndex = 11;
            this.colAssetType.Width = 70;
            // 
            // lookUpAssetType
            // 
            this.lookUpAssetType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpAssetType.LookupKey = "L_CM0002";
            this.lookUpAssetType.Name = "lookUpAssetType";
            this.lookUpAssetType.NullText = "";
            this.lookUpAssetType.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colOutType
            // 
            this.colOutType.Caption = "출고유형";
            this.colOutType.FieldName = "out_type";
            this.colOutType.Name = "colOutType";
            this.colOutType.Visible = true;
            this.colOutType.VisibleIndex = 12;
            this.colOutType.Width = 70;
            // 
            // colStatCd
            // 
            this.colStatCd.Caption = "품목상태";
            this.colStatCd.ColumnEdit = this.lookUpStatCd;
            this.colStatCd.FieldName = "stat_cd";
            this.colStatCd.Name = "colStatCd";
            this.colStatCd.Visible = true;
            this.colStatCd.VisibleIndex = 13;
            this.colStatCd.Width = 70;
            // 
            // lookUpStatCd
            // 
            this.lookUpStatCd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpStatCd.LookupKey = "L_BA0002";
            this.lookUpStatCd.Name = "lookUpStatCd";
            this.lookUpStatCd.NullText = "";
            this.lookUpStatCd.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // popcolWh
            // 
            this.popcolWh.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "...")});
            this.popcolWh.LookupKey = "P_WH";
            this.popcolWh.Name = "popcolWh";
            // 
            // popcolLoc
            // 
            this.popcolLoc.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "...")});
            this.popcolLoc.LookupKey = "P_LOC";
            this.popcolLoc.Name = "popcolLoc";
            // 
            // popcolDept
            // 
            this.popcolDept.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "...")});
            this.popcolDept.LookupKey = "P_DEPT";
            this.popcolDept.Name = "popcolDept";
            // 
            // popcolEmp
            // 
            this.popcolEmp.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "...")});
            this.popcolEmp.LookupKey = "P_EMP";
            this.popcolEmp.Name = "popcolEmp";
            // 
            // popcolCust
            // 
            this.popcolCust.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "...")});
            this.popcolCust.LookupKey = "P_CUST";
            this.popcolCust.PopupConditions = "p_cust_class=PO";
            this.popcolCust.Name = "popcolCust";            // 
            // colPoQcYn
            // 
            this.colPoQcYn.Caption = "수입검사";
            this.colPoQcYn.ColumnEdit = this.chkYn;
            this.colPoQcYn.FieldName = "po_qc_yn";
            this.colPoQcYn.Name = "colPoQcYn";
            this.colPoQcYn.Visible = true;
            this.colPoQcYn.VisibleIndex = 14;
            this.colPoQcYn.Width = 60;
            // 
            // chkYn
            // 
            this.chkYn.Name = "chkYn";
            this.chkYn.ValueChecked = "Y";
            this.chkYn.ValueUnchecked = "N";
            // 
            // colProdQcYn
            // 
            this.colProdQcYn.Caption = "공정검사";
            this.colProdQcYn.ColumnEdit = this.chkYn;
            this.colProdQcYn.FieldName = "prod_qc_yn";
            this.colProdQcYn.Name = "colProdQcYn";
            this.colProdQcYn.Visible = true;
            this.colProdQcYn.VisibleIndex = 15;
            this.colProdQcYn.Width = 60;
            // 
            // colLotYn
            // 
            this.colLotYn.Caption = "LOT관리";
            this.colLotYn.ColumnEdit = this.chkYn;
            this.colLotYn.FieldName = "lot_yn";
            this.colLotYn.Name = "colLotYn";
            this.colLotYn.Visible = true;
            this.colLotYn.VisibleIndex = 16;
            this.colLotYn.Width = 60;
            // 
            // colStockYn
            // 
            this.colStockYn.Caption = "재고관리";
            this.colStockYn.ColumnEdit = this.chkYn;
            this.colStockYn.FieldName = "stock_yn";
            this.colStockYn.Name = "colStockYn";
            this.colStockYn.Visible = true;
            this.colStockYn.VisibleIndex = 17;
            this.colStockYn.Width = 60;
            // 
            // colGrp1Nm
            // 
            this.colGrp1Nm.Caption = "품목그룹1";
            this.colGrp1Nm.FieldName = "grp1_nm";
            this.colGrp1Nm.Name = "colGrp1Nm";
            this.colGrp1Nm.Visible = true;
            this.colGrp1Nm.VisibleIndex = 18;
            this.colGrp1Nm.Width = 90;
            // 
            // colGrp2Nm
            // 
            this.colGrp2Nm.Caption = "품목그룹2";
            this.colGrp2Nm.FieldName = "grp2_nm";
            this.colGrp2Nm.Name = "colGrp2Nm";
            this.colGrp2Nm.Visible = true;
            this.colGrp2Nm.VisibleIndex = 19;
            this.colGrp2Nm.Width = 90;
            // 
            // colGrp3Nm
            // 
            this.colGrp3Nm.Caption = "품목그룹3";
            this.colGrp3Nm.FieldName = "grp3_nm";
            this.colGrp3Nm.Name = "colGrp3Nm";
            this.colGrp3Nm.Visible = true;
            this.colGrp3Nm.VisibleIndex = 20;
            this.colGrp3Nm.Width = 90;
            // 
            // colGrp4Nm
            // 
            this.colGrp4Nm.Caption = "품목그룹4";
            this.colGrp4Nm.FieldName = "grp4_nm";
            this.colGrp4Nm.Name = "colGrp4Nm";
            this.colGrp4Nm.Visible = true;
            this.colGrp4Nm.VisibleIndex = 21;
            this.colGrp4Nm.Width = 90;
            // 
            // colRemark
            // 
            this.colRemark.Caption = "비고";
            this.colRemark.FieldName = "remark";
            this.colRemark.Name = "colRemark";
            this.colRemark.Visible = true;
            this.colRemark.VisibleIndex = 22;
            this.colRemark.Width = 150;
            // 
            // colValidateResult
            // 
            this.colValidateResult.Caption = "검증결과";
            this.colValidateResult.FieldName = "validate_result";
            this.colValidateResult.Name = "colValidateResult";
            this.colValidateResult.OptionsColumn.AllowEdit = false;
            this.colValidateResult.Visible = true;
            this.colValidateResult.VisibleIndex = 23;
            this.colValidateResult.Width = 260;
            // 
            // panelWyn2
            // 
            this.panelWyn2.Controls.Add(this.sectionHeaderWyn4);
            this.panelWyn2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn2.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn2.Location = new System.Drawing.Point(0, 8);
            this.panelWyn2.Name = "panelWyn2";
            this.panelWyn2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn2.Size = new System.Drawing.Size(1294, 27);
            this.panelWyn2.TabIndex = 0;
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.btnValidate);
            this.panHeader.Controls.Add(this.btnUploadExcel);
            this.panHeader.Controls.Add(this.btnDownloadTemplate);
            this.panHeader.Controls.Add(this.cboAccId);
            this.panHeader.Controls.Add(this.labelControl1);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(3, 33);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1294, 49);
            this.panHeader.TabIndex = 2;
            // 
            // btnValidate
            // 
            this.btnValidate.BackColor = System.Drawing.Color.Transparent;
            this.btnValidate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnValidate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnValidate.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnValidate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnValidate.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnValidate.Image = null;
            this.btnValidate.Location = new System.Drawing.Point(461, 12);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnValidate.Size = new System.Drawing.Size(116, 24);
            this.btnValidate.TabIndex = 4;
            this.btnValidate.Text = "검증";
            this.btnValidate.ToolTip = null;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // btnUploadExcel
            // 
            this.btnUploadExcel.BackColor = System.Drawing.Color.Transparent;
            this.btnUploadExcel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnUploadExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUploadExcel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnUploadExcel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnUploadExcel.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnUploadExcel.Image = null;
            this.btnUploadExcel.Location = new System.Drawing.Point(343, 12);
            this.btnUploadExcel.Name = "btnUploadExcel";
            this.btnUploadExcel.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnUploadExcel.Size = new System.Drawing.Size(116, 24);
            this.btnUploadExcel.TabIndex = 3;
            this.btnUploadExcel.Text = "엑셀업로드";
            this.btnUploadExcel.ToolTip = null;
            this.btnUploadExcel.Click += new System.EventHandler(this.btnUploadExcel_Click);
            // 
            // btnDownloadTemplate
            // 
            this.btnDownloadTemplate.BackColor = System.Drawing.Color.Transparent;
            this.btnDownloadTemplate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnDownloadTemplate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDownloadTemplate.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnDownloadTemplate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDownloadTemplate.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDownloadTemplate.Image = null;
            this.btnDownloadTemplate.Location = new System.Drawing.Point(225, 12);
            this.btnDownloadTemplate.Name = "btnDownloadTemplate";
            this.btnDownloadTemplate.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDownloadTemplate.Size = new System.Drawing.Size(116, 24);
            this.btnDownloadTemplate.TabIndex = 2;
            this.btnDownloadTemplate.Text = "엑셀양식다운로드";
            this.btnDownloadTemplate.ToolTip = null;
            this.btnDownloadTemplate.Click += new System.EventHandler(this.btnDownloadTemplate_Click);
            // 
            // cboAccId
            // 
            this.cboAccId.EditValue = "";
            this.cboAccId.Location = new System.Drawing.Point(60, 15);
            this.cboAccId.LookupKey = "L_ACC";
            this.cboAccId.Required = true;
            this.cboAccId.Name = "cboAccId";
            this.cboAccId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboAccId.Properties.NullText = "";
            this.cboAccId.Size = new System.Drawing.Size(159, 20);
            this.cboAccId.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(16, 18);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 15);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "사업장";
            // 
            // paTitle
            // 
            this.paTitle.Controls.Add(this.sectionHeaderWyn1);
            this.paTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle.Location = new System.Drawing.Point(3, 0);
            this.paTitle.Name = "paTitle";
            this.paTitle.Size = new System.Drawing.Size(1294, 33);
            this.paTitle.TabIndex = 1;
            // 
            // sectionHeaderWyn1
            // 
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(0, 0);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1294, 33);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 13;
            this.sectionHeaderWyn1.Text = "품목일괄등록 [frmItemMulti]";
            // 
            // sectionHeaderWyn4
            // 
            this.sectionHeaderWyn4.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn4.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn4.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn4.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn4.Name = "sectionHeaderWyn4";
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(1289, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 9;
            this.sectionHeaderWyn4.Text = "품목정보등록";
            // 
            // frmItemMulti
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1300, 620);
            this.Controls.Add(this.panBase);
            this.Name = "frmItemMulti";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpUnitCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpAssetType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpStatCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolWh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolLoc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolDept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolEmp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolCust)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkYn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
            this.panelWyn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
            this.paTitle.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private SectionHeaderWyn sectionHeaderWyn1;
    private SectionHeaderWyn sectionHeaderWyn4;
}
