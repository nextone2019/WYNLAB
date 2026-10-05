// 품목일괄수정(frmItemMod) - 조회조건 + 전체 품목을 편집 가능한 그리드(grd1)로 조회해서 한꺼번에 고친다. 신규는 품목일괄등록(frmItemMulti), 삭제는 품목등록(frmItem).
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmItemMod
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.panBase = new WYNLAB.Base.Controls.PanelWyn();
        this.panWork = new WYNLAB.Base.Controls.PanelWyn();
        this.shList = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.chkcolYn = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
        this.lookupcolUnit = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.lookupcolAsset = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.lookupcolStat = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.lookupcolGrp1 = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.lookupcolGrp2 = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.lookupcolGrp3 = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.lookupcolGrp4 = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.popcolWh = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
        this.popcolLoc = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
        this.popcolDept = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
        this.popcolEmp = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
        this.popcolCust = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
        this.colItemId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colAccId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPoUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colSafeQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colWhId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLocId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLocNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDeptId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDeptNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colEmpId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCustId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCustNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colAssetType = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colOutType = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPoQcYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colProdQcYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLotYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colStockYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colGrp1 = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colGrp2 = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colGrp3 = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colGrp4 = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblSearchItemNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchItemNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSearchItemNm = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchItemNm = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblHint = new DevExpress.XtraEditors.LabelControl();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        this.panBase.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).BeginInit();
        this.panWork.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkcolYn)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolUnit)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolAsset)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolStat)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolGrp1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolGrp2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolGrp3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolGrp4)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolWh)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolLoc)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolDept)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolEmp)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolCust)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchItemNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchItemNm.Properties)).BeginInit();
        this.SuspendLayout();
        //
        // panBase
        //
        this.panBase.Appearance.BackColor = System.Drawing.Color.White;
        this.panBase.Appearance.Options.UseBackColor = true;
        this.panBase.Controls.Add(this.panWork);
        this.panBase.Controls.Add(this.panHeader);
        this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panBase.Location = new System.Drawing.Point(0, 0);
        this.panBase.Name = "panBase";
        this.panBase.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
        this.panBase.Size = new System.Drawing.Size(1680, 800);
        this.panBase.TabIndex = 0;
        //
        // panWork
        //
        this.panWork.Controls.Add(this.grd1);
        this.panWork.Controls.Add(this.shList);
        this.panWork.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panWork.Location = new System.Drawing.Point(5, 49);
        this.panWork.Name = "panWork";
        this.panWork.Size = new System.Drawing.Size(1670, 746);
        this.panWork.TabIndex = 1;
        //
        // shList
        //
        this.shList.BackColor = System.Drawing.Color.White;
        this.shList.Dock = System.Windows.Forms.DockStyle.Top;
        this.shList.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shList.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.shList.Location = new System.Drawing.Point(0, 0);
        this.shList.Name = "shList";
        this.shList.Size = new System.Drawing.Size(1670, 27);
        this.shList.TabIndex = 0;
        this.shList.Text = "품목 LIST (셀을 직접 고친 뒤 저장 - 변경한 행만 저장됩니다)";
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd1.Location = new System.Drawing.Point(0, 27);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.chkcolYn,
        this.lookupcolUnit,
        this.lookupcolAsset,
        this.lookupcolStat,
        this.lookupcolGrp1,
        this.lookupcolGrp2,
        this.lookupcolGrp3,
        this.lookupcolGrp4,
        this.popcolWh,
        this.popcolLoc,
        this.popcolDept,
        this.popcolEmp,
        this.popcolCust});
        this.grd1.Size = new System.Drawing.Size(1670, 719);
        this.grd1.TabIndex = 1;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colItemId,
        this.colAccId,
        this.colItemNo,
        this.colItemNm,
        this.colItemSpec,
        this.colUnitCd,
        this.colPoUnitCd,
        this.colSafeQty,
        this.colWhId,
        this.colWhNm,
        this.colLocId,
        this.colLocNm,
        this.colDeptId,
        this.colDeptNm,
        this.colEmpId,
        this.colEmpNm,
        this.colCustId,
        this.colCustNm,
        this.colAssetType,
        this.colOutType,
        this.colPoQcYn,
        this.colProdQcYn,
        this.colLotYn,
        this.colStockYn,
        this.colStatCd,
        this.colGrp1,
        this.colGrp2,
        this.colGrp3,
        this.colGrp4,
        this.colRemark});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        //
        // chkcolYn
        //
        this.chkcolYn.AutoHeight = false;
        this.chkcolYn.Name = "chkcolYn";
        this.chkcolYn.ValueChecked = "Y";
        this.chkcolYn.ValueUnchecked = "N";
        //
        // lookupcolUnit
        //
        this.lookupcolUnit.AutoHeight = false;
        this.lookupcolUnit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolUnit.LookupKey = "L_CM0001";
        this.lookupcolUnit.Name = "lookupcolUnit";
        this.lookupcolUnit.NullText = "";
        //
        // lookupcolAsset
        //
        this.lookupcolAsset.AutoHeight = false;
        this.lookupcolAsset.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolAsset.LookupKey = "L_CM0002";
        this.lookupcolAsset.Name = "lookupcolAsset";
        this.lookupcolAsset.NullText = "";
        //
        // lookupcolStat
        //
        this.lookupcolStat.AutoHeight = false;
        this.lookupcolStat.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolStat.LookupKey = "L_BA0002";
        this.lookupcolStat.Name = "lookupcolStat";
        this.lookupcolStat.NullText = "";
        //
        // lookupcolGrp1
        //
        this.lookupcolGrp1.AutoHeight = false;
        this.lookupcolGrp1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolGrp1.Name = "lookupcolGrp1";
        this.lookupcolGrp1.NullText = "";
        //
        // lookupcolGrp2
        //
        this.lookupcolGrp2.AutoHeight = false;
        this.lookupcolGrp2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolGrp2.Name = "lookupcolGrp2";
        this.lookupcolGrp2.NullText = "";
        //
        // lookupcolGrp3
        //
        this.lookupcolGrp3.AutoHeight = false;
        this.lookupcolGrp3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolGrp3.Name = "lookupcolGrp3";
        this.lookupcolGrp3.NullText = "";
        //
        // lookupcolGrp4
        //
        this.lookupcolGrp4.AutoHeight = false;
        this.lookupcolGrp4.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolGrp4.Name = "lookupcolGrp4";
        this.lookupcolGrp4.NullText = "";
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
        this.popcolCust.Name = "popcolCust";
        //
        // colItemId
        //
        this.colItemId.Caption = "품목ID";
        this.colItemId.FieldName = "item_id";
        this.colItemId.Name = "colItemId";
        this.colItemId.OptionsColumn.AllowEdit = false;
        this.colItemId.Width = 60;
        //
        // colAccId
        //
        this.colAccId.Caption = "사업장";
        this.colAccId.FieldName = "acc_id";
        this.colAccId.Name = "colAccId";
        this.colAccId.OptionsColumn.AllowEdit = false;
        this.colAccId.Width = 60;
        //
        // colItemNo
        //
        this.colItemNo.Caption = "품번";
        this.colItemNo.FieldName = "item_no";
        this.colItemNo.Name = "colItemNo";
        this.colItemNo.Visible = true;
        this.colItemNo.VisibleIndex = 0;
        this.colItemNo.Width = 110;
        //
        // colItemNm
        //
        this.colItemNm.Caption = "품명";
        this.colItemNm.FieldName = "item_nm";
        this.colItemNm.Name = "colItemNm";
        this.colItemNm.Visible = true;
        this.colItemNm.VisibleIndex = 1;
        this.colItemNm.Width = 160;
        //
        // colItemSpec
        //
        this.colItemSpec.Caption = "규격";
        this.colItemSpec.FieldName = "item_spec";
        this.colItemSpec.Name = "colItemSpec";
        this.colItemSpec.Visible = true;
        this.colItemSpec.VisibleIndex = 2;
        this.colItemSpec.Width = 130;
        //
        // colUnitCd
        //
        this.colUnitCd.Caption = "단위";
        this.colUnitCd.ColumnEdit = this.lookupcolUnit;
        this.colUnitCd.FieldName = "unit_cd";
        this.colUnitCd.Name = "colUnitCd";
        this.colUnitCd.Visible = true;
        this.colUnitCd.VisibleIndex = 3;
        this.colUnitCd.Width = 70;
        //
        // colPoUnitCd
        //
        this.colPoUnitCd.Caption = "발주단위";
        this.colPoUnitCd.ColumnEdit = this.lookupcolUnit;
        this.colPoUnitCd.FieldName = "po_unit_cd";
        this.colPoUnitCd.Name = "colPoUnitCd";
        this.colPoUnitCd.Visible = true;
        this.colPoUnitCd.VisibleIndex = 4;
        this.colPoUnitCd.Width = 80;
        //
        // colSafeQty
        //
        this.colSafeQty.Caption = "안전재고";
        this.colSafeQty.DisplayFormat.FormatString = "#,##0.####";
        this.colSafeQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colSafeQty.FieldName = "safe_qty";
        this.colSafeQty.Name = "colSafeQty";
        this.colSafeQty.Visible = true;
        this.colSafeQty.VisibleIndex = 5;
        this.colSafeQty.Width = 80;
        //
        // colWhId
        //
        this.colWhId.Caption = "창고ID";
        this.colWhId.FieldName = "wh_id";
        this.colWhId.Name = "colWhId";
        this.colWhId.OptionsColumn.AllowEdit = false;
        this.colWhId.Width = 60;
        //
        // colWhNm
        //
        this.colWhNm.Caption = "창고";
        this.colWhNm.ColumnEdit = this.popcolWh;
        this.colWhNm.FieldName = "wh_nm";
        this.colWhNm.Name = "colWhNm";
        this.colWhNm.Visible = true;
        this.colWhNm.VisibleIndex = 6;
        this.colWhNm.Width = 110;
        //
        // colLocId
        //
        this.colLocId.Caption = "위치ID";
        this.colLocId.FieldName = "loc_id";
        this.colLocId.Name = "colLocId";
        this.colLocId.OptionsColumn.AllowEdit = false;
        this.colLocId.Width = 60;
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
        // colDeptId
        //
        this.colDeptId.Caption = "부서ID";
        this.colDeptId.FieldName = "dept_id";
        this.colDeptId.Name = "colDeptId";
        this.colDeptId.OptionsColumn.AllowEdit = false;
        this.colDeptId.Width = 60;
        //
        // colDeptNm
        //
        this.colDeptNm.Caption = "담당부서";
        this.colDeptNm.ColumnEdit = this.popcolDept;
        this.colDeptNm.FieldName = "dept_nm";
        this.colDeptNm.Name = "colDeptNm";
        this.colDeptNm.Visible = true;
        this.colDeptNm.VisibleIndex = 8;
        this.colDeptNm.Width = 100;
        //
        // colEmpId
        //
        this.colEmpId.Caption = "담당자ID";
        this.colEmpId.FieldName = "emp_id";
        this.colEmpId.Name = "colEmpId";
        this.colEmpId.OptionsColumn.AllowEdit = false;
        this.colEmpId.Width = 60;
        //
        // colEmpNm
        //
        this.colEmpNm.Caption = "담당자";
        this.colEmpNm.ColumnEdit = this.popcolEmp;
        this.colEmpNm.FieldName = "emp_nm";
        this.colEmpNm.Name = "colEmpNm";
        this.colEmpNm.Visible = true;
        this.colEmpNm.VisibleIndex = 9;
        this.colEmpNm.Width = 90;
        //
        // colCustId
        //
        this.colCustId.Caption = "구매처ID";
        this.colCustId.FieldName = "cust_id";
        this.colCustId.Name = "colCustId";
        this.colCustId.OptionsColumn.AllowEdit = false;
        this.colCustId.Width = 60;
        //
        // colCustNm
        //
        this.colCustNm.Caption = "구매처";
        this.colCustNm.ColumnEdit = this.popcolCust;
        this.colCustNm.FieldName = "cust_nm";
        this.colCustNm.Name = "colCustNm";
        this.colCustNm.Visible = true;
        this.colCustNm.VisibleIndex = 10;
        this.colCustNm.Width = 110;
        //
        // colAssetType
        //
        this.colAssetType.Caption = "자산구분";
        this.colAssetType.ColumnEdit = this.lookupcolAsset;
        this.colAssetType.FieldName = "asset_type";
        this.colAssetType.Name = "colAssetType";
        this.colAssetType.Visible = true;
        this.colAssetType.VisibleIndex = 11;
        this.colAssetType.Width = 90;
        //
        // colOutType
        //
        this.colOutType.Caption = "출고구분";
        this.colOutType.FieldName = "out_type";
        this.colOutType.Name = "colOutType";
        this.colOutType.Visible = true;
        this.colOutType.VisibleIndex = 12;
        this.colOutType.Width = 80;
        //
        // colPoQcYn
        //
        this.colPoQcYn.Caption = "수입검사";
        this.colPoQcYn.ColumnEdit = this.chkcolYn;
        this.colPoQcYn.AppearanceCell.Options.UseTextOptions = true;
        this.colPoQcYn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        this.colPoQcYn.FieldName = "po_qc_yn";
        this.colPoQcYn.Name = "colPoQcYn";
        this.colPoQcYn.Visible = true;
        this.colPoQcYn.VisibleIndex = 13;
        this.colPoQcYn.Width = 65;
        //
        // colProdQcYn
        //
        this.colProdQcYn.Caption = "공정검사";
        this.colProdQcYn.ColumnEdit = this.chkcolYn;
        this.colProdQcYn.AppearanceCell.Options.UseTextOptions = true;
        this.colProdQcYn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        this.colProdQcYn.FieldName = "prod_qc_yn";
        this.colProdQcYn.Name = "colProdQcYn";
        this.colProdQcYn.Visible = true;
        this.colProdQcYn.VisibleIndex = 14;
        this.colProdQcYn.Width = 65;
        //
        // colLotYn
        //
        this.colLotYn.Caption = "LOT관리";
        this.colLotYn.ColumnEdit = this.chkcolYn;
        this.colLotYn.AppearanceCell.Options.UseTextOptions = true;
        this.colLotYn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        this.colLotYn.FieldName = "lot_yn";
        this.colLotYn.Name = "colLotYn";
        this.colLotYn.Visible = true;
        this.colLotYn.VisibleIndex = 15;
        this.colLotYn.Width = 65;
        //
        // colStockYn
        //
        this.colStockYn.Caption = "재고관리";
        this.colStockYn.ColumnEdit = this.chkcolYn;
        this.colStockYn.AppearanceCell.Options.UseTextOptions = true;
        this.colStockYn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        this.colStockYn.FieldName = "stock_yn";
        this.colStockYn.Name = "colStockYn";
        this.colStockYn.Visible = true;
        this.colStockYn.VisibleIndex = 16;
        this.colStockYn.Width = 65;
        //
        // colStatCd
        //
        this.colStatCd.Caption = "상태";
        this.colStatCd.ColumnEdit = this.lookupcolStat;
        this.colStatCd.FieldName = "stat_cd";
        this.colStatCd.Name = "colStatCd";
        this.colStatCd.Visible = true;
        this.colStatCd.VisibleIndex = 17;
        this.colStatCd.Width = 80;
        //
        // colGrp1
        //
        this.colGrp1.Caption = "품목그룹1";
        this.colGrp1.ColumnEdit = this.lookupcolGrp1;
        this.colGrp1.FieldName = "grp1_id";
        this.colGrp1.Name = "colGrp1";
        this.colGrp1.Visible = true;
        this.colGrp1.VisibleIndex = 18;
        this.colGrp1.Width = 110;
        //
        // colGrp2
        //
        this.colGrp2.Caption = "품목그룹2";
        this.colGrp2.ColumnEdit = this.lookupcolGrp2;
        this.colGrp2.FieldName = "grp2_id";
        this.colGrp2.Name = "colGrp2";
        this.colGrp2.Visible = true;
        this.colGrp2.VisibleIndex = 19;
        this.colGrp2.Width = 110;
        //
        // colGrp3
        //
        this.colGrp3.Caption = "품목그룹3";
        this.colGrp3.ColumnEdit = this.lookupcolGrp3;
        this.colGrp3.FieldName = "grp3_id";
        this.colGrp3.Name = "colGrp3";
        this.colGrp3.Visible = true;
        this.colGrp3.VisibleIndex = 20;
        this.colGrp3.Width = 110;
        //
        // colGrp4
        //
        this.colGrp4.Caption = "품목그룹4";
        this.colGrp4.ColumnEdit = this.lookupcolGrp4;
        this.colGrp4.FieldName = "grp4_id";
        this.colGrp4.Name = "colGrp4";
        this.colGrp4.Visible = true;
        this.colGrp4.VisibleIndex = 21;
        this.colGrp4.Width = 110;
        //
        // colRemark
        //
        this.colRemark.Caption = "비고";
        this.colRemark.FieldName = "remark";
        this.colRemark.Name = "colRemark";
        this.colRemark.Visible = true;
        this.colRemark.VisibleIndex = 22;
        this.colRemark.Width = 220;
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblSearchAccId);
        this.panHeader.Controls.Add(this.cboSearchAccId);
        this.panHeader.Controls.Add(this.lblSearchItemNo);
        this.panHeader.Controls.Add(this.txtSearchItemNo);
        this.panHeader.Controls.Add(this.lblSearchItemNm);
        this.panHeader.Controls.Add(this.txtSearchItemNm);
        this.panHeader.Controls.Add(this.lblHint);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.Location = new System.Drawing.Point(5, 0);
        this.panHeader.Name = "panHeader";
        this.panHeader.Size = new System.Drawing.Size(1670, 49);
        this.panHeader.TabIndex = 0;
        //
        // lblSearchAccId
        //
        this.lblSearchAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchAccId.Appearance.Options.UseFont = true;
        this.lblSearchAccId.Location = new System.Drawing.Point(25, 18);
        this.lblSearchAccId.Name = "lblSearchAccId";
        this.lblSearchAccId.Size = new System.Drawing.Size(36, 15);
        this.lblSearchAccId.TabIndex = 0;
        this.lblSearchAccId.Text = "사업장";
        //
        // cboSearchAccId
        //
        this.cboSearchAccId.EditValue = "";
        this.cboSearchAccId.Location = new System.Drawing.Point(69, 15);
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
        // lblSearchItemNo
        //
        this.lblSearchItemNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchItemNo.Appearance.Options.UseFont = true;
        this.lblSearchItemNo.Location = new System.Drawing.Point(224, 18);
        this.lblSearchItemNo.Name = "lblSearchItemNo";
        this.lblSearchItemNo.Size = new System.Drawing.Size(24, 15);
        this.lblSearchItemNo.TabIndex = 2;
        this.lblSearchItemNo.Text = "품번";
        //
        // txtSearchItemNo
        //
        this.txtSearchItemNo.Location = new System.Drawing.Point(262, 15);
        this.txtSearchItemNo.Name = "txtSearchItemNo";
        this.txtSearchItemNo.Size = new System.Drawing.Size(150, 20);
        this.txtSearchItemNo.TabIndex = 3;
        //
        // lblSearchItemNm
        //
        this.lblSearchItemNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchItemNm.Appearance.Options.UseFont = true;
        this.lblSearchItemNm.Location = new System.Drawing.Point(436, 18);
        this.lblSearchItemNm.Name = "lblSearchItemNm";
        this.lblSearchItemNm.Size = new System.Drawing.Size(24, 15);
        this.lblSearchItemNm.TabIndex = 4;
        this.lblSearchItemNm.Text = "품명";
        //
        // txtSearchItemNm
        //
        this.txtSearchItemNm.Location = new System.Drawing.Point(474, 15);
        this.txtSearchItemNm.Name = "txtSearchItemNm";
        this.txtSearchItemNm.Size = new System.Drawing.Size(180, 20);
        this.txtSearchItemNm.TabIndex = 5;
        //
        // lblHint
        //
        this.lblHint.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblHint.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
        this.lblHint.Appearance.Options.UseFont = true;
        this.lblHint.Appearance.Options.UseForeColor = true;
        this.lblHint.Location = new System.Drawing.Point(690, 18);
        this.lblHint.Name = "lblHint";
        this.lblHint.Size = new System.Drawing.Size(60, 15);
        this.lblHint.TabIndex = 6;
        this.lblHint.Text = "※ 신규 품목은 품목일괄등록, 삭제는 품목등록에서 합니다. 이름 칸을 지우면 연결(창고/부서 등)이 해제됩니다.";
        //
        // frmItemMod
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 800);
        this.Controls.Add(this.panBase);
        this.Name = "frmItemMod";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).EndInit();
        this.panWork.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkcolYn)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolUnit)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolAsset)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolStat)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolGrp1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolGrp2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolGrp3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolGrp4)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolWh)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolLoc)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolDept)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolEmp)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolCust)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchItemNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchItemNm.Properties)).EndInit();
        this.panHeader.PerformLayout();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panWork;
    private SectionHeaderWyn shList;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkcolYn;
    private LookUpColumnEdit lookupcolUnit;
    private LookUpColumnEdit lookupcolAsset;
    private LookUpColumnEdit lookupcolStat;
    private LookUpColumnEdit lookupcolGrp1;
    private LookUpColumnEdit lookupcolGrp2;
    private LookUpColumnEdit lookupcolGrp3;
    private LookUpColumnEdit lookupcolGrp4;
    private PopupLookupColumnEdit popcolWh;
    private PopupLookupColumnEdit popcolLoc;
    private PopupLookupColumnEdit popcolDept;
    private PopupLookupColumnEdit popcolEmp;
    private PopupLookupColumnEdit popcolCust;
    private DevExpress.XtraGrid.Columns.GridColumn colItemId;
    private DevExpress.XtraGrid.Columns.GridColumn colAccId;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colPoUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colSafeQty;
    private DevExpress.XtraGrid.Columns.GridColumn colWhId;
    private DevExpress.XtraGrid.Columns.GridColumn colWhNm;
    private DevExpress.XtraGrid.Columns.GridColumn colLocId;
    private DevExpress.XtraGrid.Columns.GridColumn colLocNm;
    private DevExpress.XtraGrid.Columns.GridColumn colDeptId;
    private DevExpress.XtraGrid.Columns.GridColumn colDeptNm;
    private DevExpress.XtraGrid.Columns.GridColumn colEmpId;
    private DevExpress.XtraGrid.Columns.GridColumn colEmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn colCustId;
    private DevExpress.XtraGrid.Columns.GridColumn colCustNm;
    private DevExpress.XtraGrid.Columns.GridColumn colAssetType;
    private DevExpress.XtraGrid.Columns.GridColumn colOutType;
    private DevExpress.XtraGrid.Columns.GridColumn colPoQcYn;
    private DevExpress.XtraGrid.Columns.GridColumn colProdQcYn;
    private DevExpress.XtraGrid.Columns.GridColumn colLotYn;
    private DevExpress.XtraGrid.Columns.GridColumn colStockYn;
    private DevExpress.XtraGrid.Columns.GridColumn colStatCd;
    private DevExpress.XtraGrid.Columns.GridColumn colGrp1;
    private DevExpress.XtraGrid.Columns.GridColumn colGrp2;
    private DevExpress.XtraGrid.Columns.GridColumn colGrp3;
    private DevExpress.XtraGrid.Columns.GridColumn colGrp4;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private LookUpEditWyn cboSearchAccId;
    private DevExpress.XtraEditors.LabelControl lblSearchItemNo;
    private TextEditWyn txtSearchItemNo;
    private DevExpress.XtraEditors.LabelControl lblSearchItemNm;
    private TextEditWyn txtSearchItemNm;
    private DevExpress.XtraEditors.LabelControl lblHint;
}
