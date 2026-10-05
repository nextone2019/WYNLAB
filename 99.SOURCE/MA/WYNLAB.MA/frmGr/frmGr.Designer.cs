// 구매입고등록(frmGr) - frmPo와 같은 Master-One Sheet 구조(TMAGRM/TMAGRD 대상, 2026-09-25). 라인은 입고대기 불러오기로만 추가한다.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

public partial class frmGr
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
        this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
        this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.popcolWh = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
        this.popcolLoc = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
        this.chkcolStock = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
        this.lookupcolSrc = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.colSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLotNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colSrcType = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colSrcNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPoNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colReadyQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colGrQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colWhId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLocId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colStockYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colNextQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
        this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
        this.btnDeletRow1 = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnLoadReady = new WYNLAB.Base.Controls.ButtonWyn();
        this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
        this.panData = new WYNLAB.Base.Controls.PanelWyn();
        this.lblAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblStatCd = new DevExpress.XtraEditors.LabelControl();
        this.cboStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblGrNo = new DevExpress.XtraEditors.LabelControl();
        this.txtGrNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblGrDate = new DevExpress.XtraEditors.LabelControl();
        this.dteGrDate = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblTransType = new DevExpress.XtraEditors.LabelControl();
        this.cboTransType = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblAutoYn = new DevExpress.XtraEditors.LabelControl();
        this.txtAutoYn = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblCust = new DevExpress.XtraEditors.LabelControl();
        this.txtCustNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.txtCustId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDept = new DevExpress.XtraEditors.LabelControl();
        this.txtDeptNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.txtDeptId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblEmp = new DevExpress.XtraEditors.LabelControl();
        this.txtEmpNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.txtEmpId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblCfmDt = new DevExpress.XtraEditors.LabelControl();
        this.txtCfmDt = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblCfmUserId = new DevExpress.XtraEditors.LabelControl();
        this.txtCfmUserId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblRemark = new DevExpress.XtraEditors.LabelControl();
        this.memoRemark = new DevExpress.XtraEditors.MemoEdit();
        this.btnConfirm = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnConfirmCancel = new WYNLAB.Base.Controls.ButtonWyn();
        this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.lblSearchGrNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchGrNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        this.panBase.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
        this.panelWyn3.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
        this.panelWyn4.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolWh)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolLoc)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkcolStock)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolSrc)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
        this.panelWyn1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
        this.panelWyn7.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
        this.panelWyn5.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
        this.panData.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtGrNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteGrDate.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteGrDate.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboTransType.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtAutoYn.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCustNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCustId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCfmDt.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCfmUserId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
        this.panelWyn6.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchGrNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
        this.SuspendLayout();
        //
        // panBase
        //
        this.panBase.Appearance.BackColor = System.Drawing.Color.White;
        this.panBase.Appearance.Options.UseBackColor = true;
        this.panBase.Controls.Add(this.panelWyn3);
        this.panBase.Controls.Add(this.panHeader);
        this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panBase.Location = new System.Drawing.Point(0, 0);
        this.panBase.Name = "panBase";
        this.panBase.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
        this.panBase.Size = new System.Drawing.Size(1680, 800);
        this.panBase.TabIndex = 6;
        //
        // panelWyn3
        //
        this.panelWyn3.Controls.Add(this.panelWyn4);
        this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelWyn3.Location = new System.Drawing.Point(5, 49);
        this.panelWyn3.Name = "panelWyn3";
        this.panelWyn3.Size = new System.Drawing.Size(1670, 746);
        this.panelWyn3.TabIndex = 7;
        //
        // panelWyn4
        //
        this.panelWyn4.Controls.Add(this.grd1);
        this.panelWyn4.Controls.Add(this.panelWyn7);
        this.panelWyn4.Controls.Add(this.panelWyn1);
        this.panelWyn4.Controls.Add(this.panelWyn5);
        this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelWyn4.Location = new System.Drawing.Point(0, 0);
        this.panelWyn4.Name = "panelWyn4";
        this.panelWyn4.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.panelWyn4.Size = new System.Drawing.Size(1670, 746);
        this.panelWyn4.TabIndex = 7;
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
        // chkcolStock
        //
        this.chkcolStock.AutoHeight = false;
        this.chkcolStock.Name = "chkcolStock";
        this.chkcolStock.ValueChecked = "Y";
        this.chkcolStock.ValueUnchecked = "N";
        //
        // lookupcolSrc
        //
        this.lookupcolSrc.AutoHeight = false;
        this.lookupcolSrc.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolSrc.LookupKey = "L_MA0004";
        this.lookupcolSrc.Name = "lookupcolSrc";
        this.lookupcolSrc.NullText = "";
        this.lookupcolSrc.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd1.EmbeddedNavigator.Buttons.Append.Visible = false;
        this.grd1.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
        this.grd1.EmbeddedNavigator.Buttons.Edit.Visible = false;
        this.grd1.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
        this.grd1.EmbeddedNavigator.Buttons.Remove.Visible = false;
        this.grd1.Location = new System.Drawing.Point(3, 315);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.popcolWh,
                this.popcolLoc,
                this.chkcolStock,
                this.lookupcolSrc});
        this.grd1.Size = new System.Drawing.Size(1664, 412);
        this.grd1.TabIndex = 0;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colSerl,
                this.colItemNo,
                this.colItemNm,
                this.colItemSpec,
                this.colUnitCd,
                this.colLotNo,
                this.colSrcType,
                this.colSrcNo,
                this.colPoNo,
                this.colReadyQty,
                this.colGrQty,
                this.colWhId,
                this.colLocId,
                this.colStockYn,
                this.colNextQty,
                this.colRemark});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        //
        // colSerl
        //
        this.colSerl.Caption = "순번";
        this.colSerl.FieldName = "serl";
        this.colSerl.Name = "colSerl";
        this.colSerl.OptionsColumn.AllowEdit = false;
        this.colSerl.Visible = true;
        this.colSerl.VisibleIndex = 0;
        this.colSerl.Width = 50;
        //
        // colItemNo
        //
        this.colItemNo.Caption = "품번";
        this.colItemNo.FieldName = "item_no";
        this.colItemNo.Name = "colItemNo";
        this.colItemNo.OptionsColumn.AllowEdit = false;
        this.colItemNo.Visible = true;
        this.colItemNo.VisibleIndex = 1;
        this.colItemNo.Width = 100;
        //
        // colItemNm
        //
        this.colItemNm.Caption = "품명";
        this.colItemNm.FieldName = "item_nm";
        this.colItemNm.Name = "colItemNm";
        this.colItemNm.OptionsColumn.AllowEdit = false;
        this.colItemNm.Visible = true;
        this.colItemNm.VisibleIndex = 2;
        this.colItemNm.Width = 140;
        //
        // colItemSpec
        //
        this.colItemSpec.Caption = "규격";
        this.colItemSpec.FieldName = "item_spec";
        this.colItemSpec.Name = "colItemSpec";
        this.colItemSpec.OptionsColumn.AllowEdit = false;
        this.colItemSpec.Visible = true;
        this.colItemSpec.VisibleIndex = 3;
        this.colItemSpec.Width = 110;
        //
        // colUnitCd
        //
        this.colUnitCd.Caption = "단위";
        this.colUnitCd.FieldName = "unit_cd";
        this.colUnitCd.Name = "colUnitCd";
        this.colUnitCd.OptionsColumn.AllowEdit = false;
        this.colUnitCd.Visible = true;
        this.colUnitCd.VisibleIndex = 4;
        this.colUnitCd.Width = 50;
        //
        // colLotNo
        //
        this.colLotNo.Caption = "LOT";
        this.colLotNo.FieldName = "lot_no";
        this.colLotNo.Name = "colLotNo";
        this.colLotNo.OptionsColumn.AllowEdit = false;
        this.colLotNo.Visible = true;
        this.colLotNo.VisibleIndex = 5;
        this.colLotNo.Width = 90;
        //
        // colSrcType
        //
        this.colSrcType.Caption = "입고원천";
        this.colSrcType.ColumnEdit = this.lookupcolSrc;
        this.colSrcType.FieldName = "src_type";
        this.colSrcType.Name = "colSrcType";
        this.colSrcType.OptionsColumn.AllowEdit = false;
        this.colSrcType.Visible = true;
        this.colSrcType.VisibleIndex = 6;
        this.colSrcType.Width = 80;
        //
        // colSrcNo
        //
        this.colSrcNo.Caption = "원천번호";
        this.colSrcNo.FieldName = "src_no";
        this.colSrcNo.Name = "colSrcNo";
        this.colSrcNo.OptionsColumn.AllowEdit = false;
        this.colSrcNo.Visible = true;
        this.colSrcNo.VisibleIndex = 7;
        this.colSrcNo.Width = 110;
        //
        // colPoNo
        //
        this.colPoNo.Caption = "발주번호";
        this.colPoNo.FieldName = "po_no";
        this.colPoNo.Name = "colPoNo";
        this.colPoNo.OptionsColumn.AllowEdit = false;
        this.colPoNo.Visible = true;
        this.colPoNo.VisibleIndex = 8;
        this.colPoNo.Width = 110;
        //
        // colReadyQty
        //
        this.colReadyQty.Caption = "입고대기수량";
        this.colReadyQty.DisplayFormat.FormatString = "#,##0.####";
        this.colReadyQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colReadyQty.FieldName = "ready_qty";
        this.colReadyQty.Name = "colReadyQty";
        this.colReadyQty.OptionsColumn.AllowEdit = false;
        this.colReadyQty.Visible = true;
        this.colReadyQty.VisibleIndex = 9;
        this.colReadyQty.Width = 90;
        //
        // colGrQty
        //
        this.colGrQty.Caption = "입고수량";
        this.colGrQty.DisplayFormat.FormatString = "#,##0.####";
        this.colGrQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colGrQty.FieldName = "gr_qty";
        this.colGrQty.Name = "colGrQty";
        this.colGrQty.Visible = true;
        this.colGrQty.VisibleIndex = 10;
        this.colGrQty.Width = 90;
        //
        // colWhId
        //
        this.colWhId.Caption = "李쎄퀬";
        this.colWhId.ColumnEdit = this.popcolWh;
        this.colWhId.FieldName = "wh_id";
        this.colWhId.Name = "colWhId";
        this.colWhId.Visible = true;
        this.colWhId.VisibleIndex = 11;
        this.colWhId.Width = 90;
        //
        // colLocId
        //
        this.colLocId.Caption = "위치";
        this.colLocId.ColumnEdit = this.popcolLoc;
        this.colLocId.FieldName = "loc_id";
        this.colLocId.Name = "colLocId";
        this.colLocId.Visible = true;
        this.colLocId.VisibleIndex = 12;
        this.colLocId.Width = 90;
        //
        // colStockYn
        //
        this.colStockYn.Caption = "재고반영";
        this.colStockYn.ColumnEdit = this.chkcolStock;
        this.colStockYn.FieldName = "stock_yn";
        this.colStockYn.Name = "colStockYn";
        this.colStockYn.OptionsColumn.AllowEdit = false;
        this.colStockYn.Visible = true;
        this.colStockYn.VisibleIndex = 13;
        this.colStockYn.Width = 45;
        //
        // colNextQty
        //
        this.colNextQty.Caption = "후속처리";
        this.colNextQty.DisplayFormat.FormatString = "#,##0.####";
        this.colNextQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colNextQty.FieldName = "next_qty";
        this.colNextQty.Name = "colNextQty";
        this.colNextQty.OptionsColumn.AllowEdit = false;
        this.colNextQty.Visible = true;
        this.colNextQty.VisibleIndex = 14;
        this.colNextQty.Width = 80;
        //
        // colRemark
        //
        this.colRemark.Caption = "비고";
        this.colRemark.FieldName = "remark";
        this.colRemark.Name = "colRemark";
        this.colRemark.Visible = true;
        this.colRemark.VisibleIndex = 15;
        this.colRemark.Width = 160;
        //
        // panelWyn1
        //
        this.panelWyn1.Controls.Add(this.sectionHeaderWyn2);
        this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn1.Location = new System.Drawing.Point(3, 288);
        this.panelWyn1.Name = "panelWyn1";
        this.panelWyn1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
        this.panelWyn1.Size = new System.Drawing.Size(1664, 27);
        this.panelWyn1.TabIndex = 8;
        //
        // panelWyn7
        //
        this.panelWyn7.Controls.Add(this.btnLoadReady);
        this.panelWyn7.Controls.Add(this.btnDeletRow1);
        this.panelWyn7.Appearance.BackColor = System.Drawing.Color.White;
        this.panelWyn7.Appearance.Options.UseBackColor = true;
        this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn7.Location = new System.Drawing.Point(1520, 0);
        this.panelWyn7.Name = "panelWyn7";
        this.panelWyn7.Padding = new System.Windows.Forms.Padding(5, 3, 0, 2);
        this.panelWyn7.Size = new System.Drawing.Size(144, 30);
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
        this.btnDeletRow1.Location = new System.Drawing.Point(6, 4);
        this.btnDeletRow1.Name = "btnDeletRow1";
        this.btnDeletRow1.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
        this.btnDeletRow1.Size = new System.Drawing.Size(60, 24);
        this.btnDeletRow1.Text = "행삭제";
        this.btnDeletRow1.TabIndex = 0;
        this.btnDeletRow1.ToolTip = "행삭제";
        //
        // btnLoadReady
        //
        this.btnLoadReady.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
        this.btnLoadReady.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
        this.btnLoadReady.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnLoadReady.Location = new System.Drawing.Point(68, 4);
        this.btnLoadReady.Name = "btnLoadReady";
        this.btnLoadReady.Size = new System.Drawing.Size(120, 24);
        this.btnLoadReady.TabIndex = 1;
        this.btnLoadReady.Text = "입고대기 불러오기";
        this.btnLoadReady.ToolTip = "확정된 검사(합격+특채)건 중에서 입고 가능한 수량을 불러옵니다.";
        //
        // sectionHeaderWyn2
        //
        this.sectionHeaderWyn2.BackColor = System.Drawing.Color.White;
        this.sectionHeaderWyn2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.sectionHeaderWyn2.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.sectionHeaderWyn2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
        this.sectionHeaderWyn2.Location = new System.Drawing.Point(5, 0);
        this.sectionHeaderWyn2.Name = "sectionHeaderWyn2";
        this.sectionHeaderWyn2.Size = new System.Drawing.Size(1515, 25);
        this.sectionHeaderWyn2.TabIndex = 10;
        this.sectionHeaderWyn2.Text = "구매입고 품목 상세정보 등록";
        //
        // panelWyn5
        //
        this.panelWyn5.Controls.Add(this.panData);
        this.panelWyn5.Controls.Add(this.panelWyn6);
        this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn5.Location = new System.Drawing.Point(3, 0);
        this.panelWyn5.Name = "panelWyn5";
        this.panelWyn5.Size = new System.Drawing.Size(1664, 288);
        this.panelWyn5.TabIndex = 6;
        //
        // panData
        //
        this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        this.panData.Controls.Add(this.lblAccId);
        this.panData.Controls.Add(this.cboAccId);
        this.panData.Controls.Add(this.lblStatCd);
        this.panData.Controls.Add(this.cboStatCd);
        this.panData.Controls.Add(this.lblGrNo);
        this.panData.Controls.Add(this.txtGrNo);
        this.panData.Controls.Add(this.lblGrDate);
        this.panData.Controls.Add(this.dteGrDate);
        this.panData.Controls.Add(this.lblTransType);
        this.panData.Controls.Add(this.cboTransType);
        this.panData.Controls.Add(this.lblAutoYn);
        this.panData.Controls.Add(this.txtAutoYn);
        this.panData.Controls.Add(this.lblCust);
        this.panData.Controls.Add(this.txtCustNm);
        this.panData.Controls.Add(this.txtCustId);
        this.panData.Controls.Add(this.lblDept);
        this.panData.Controls.Add(this.txtDeptNm);
        this.panData.Controls.Add(this.txtDeptId);
        this.panData.Controls.Add(this.lblEmp);
        this.panData.Controls.Add(this.txtEmpNm);
        this.panData.Controls.Add(this.txtEmpId);
        this.panData.Controls.Add(this.lblCfmDt);
        this.panData.Controls.Add(this.txtCfmDt);
        this.panData.Controls.Add(this.lblCfmUserId);
        this.panData.Controls.Add(this.txtCfmUserId);
        this.panData.Controls.Add(this.lblRemark);
        this.panData.Controls.Add(this.memoRemark);
        this.panData.Controls.Add(this.btnConfirm);
        this.panData.Controls.Add(this.btnConfirmCancel);
        this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panData.Location = new System.Drawing.Point(0, 27);
        this.panData.Name = "panData";
        this.panData.Size = new System.Drawing.Size(1664, 261);
        this.panData.TabIndex = 8;
        //
        // lblAccId
        //
        this.lblAccId.Location = new System.Drawing.Point(28, 15);
        this.lblAccId.Name = "lblAccId";
        this.lblAccId.Size = new System.Drawing.Size(36, 15);
        this.lblAccId.TabIndex = 2;
        this.lblAccId.Text = "사업장";
        //
        // cboAccId
        //
        this.cboAccId.Location = new System.Drawing.Point(100, 12);
        this.cboAccId.LookupKey = "L_ACC";
        this.cboAccId.Name = "cboAccId";
        this.cboAccId.Size = new System.Drawing.Size(150, 20);
        this.cboAccId.TabIndex = 3;
        //
        // lblStatCd
        //
        this.lblStatCd.Location = new System.Drawing.Point(270, 15);
        this.lblStatCd.Name = "lblStatCd";
        this.lblStatCd.Size = new System.Drawing.Size(48, 15);
        this.lblStatCd.TabIndex = 4;
        this.lblStatCd.Text = "진행상태";
        //
        // cboStatCd
        //
        this.cboStatCd.Location = new System.Drawing.Point(350, 12);
        this.cboStatCd.LookupKey = "L_MA0012";
        this.cboStatCd.Name = "cboStatCd";
        this.cboStatCd.Properties.ReadOnly = true;
        this.cboStatCd.Size = new System.Drawing.Size(150, 20);
        this.cboStatCd.TabIndex = 5;
        //
        // lblGrNo
        //
        this.lblGrNo.Location = new System.Drawing.Point(16, 43);
        this.lblGrNo.Name = "lblGrNo";
        this.lblGrNo.Size = new System.Drawing.Size(48, 15);
        this.lblGrNo.TabIndex = 6;
        this.lblGrNo.Text = "입고번호";
        //
        // txtGrNo
        //
        this.txtGrNo.Location = new System.Drawing.Point(100, 40);
        this.txtGrNo.Name = "txtGrNo";
        this.txtGrNo.Properties.ReadOnly = true;
        this.txtGrNo.Size = new System.Drawing.Size(150, 20);
        this.txtGrNo.TabIndex = 7;
        //
        // lblGrDate
        //
        this.lblGrDate.Location = new System.Drawing.Point(270, 43);
        this.lblGrDate.Name = "lblGrDate";
        this.lblGrDate.Size = new System.Drawing.Size(48, 15);
        this.lblGrDate.TabIndex = 8;
        this.lblGrDate.Text = "입고일자";
        //
        // dteGrDate
        //
        this.dteGrDate.Location = new System.Drawing.Point(350, 40);
        this.dteGrDate.Name = "dteGrDate";
        this.dteGrDate.Size = new System.Drawing.Size(150, 20);
        this.dteGrDate.TabIndex = 9;
        //
        // lblTransType
        //
        this.lblTransType.Location = new System.Drawing.Point(16, 71);
        this.lblTransType.Name = "lblTransType";
        this.lblTransType.Size = new System.Drawing.Size(48, 15);
        this.lblTransType.TabIndex = 10;
        this.lblTransType.Text = "수불유형";
        //
        // cboTransType
        //
        this.cboTransType.Location = new System.Drawing.Point(100, 68);
        this.cboTransType.LookupKey = "L_MA0011";
        this.cboTransType.Name = "cboTransType";
        this.cboTransType.Properties.ReadOnly = true;
        this.cboTransType.Size = new System.Drawing.Size(150, 20);
        this.cboTransType.TabIndex = 11;
        //
        // lblAutoYn
        //
        this.lblAutoYn.Location = new System.Drawing.Point(270, 71);
        this.lblAutoYn.Name = "lblAutoYn";
        this.lblAutoYn.Size = new System.Drawing.Size(48, 15);
        this.lblAutoYn.TabIndex = 12;
        this.lblAutoYn.Text = "자동입고";
        //
        // txtAutoYn
        //
        this.txtAutoYn.Location = new System.Drawing.Point(350, 68);
        this.txtAutoYn.Name = "txtAutoYn";
        this.txtAutoYn.Properties.ReadOnly = true;
        this.txtAutoYn.Size = new System.Drawing.Size(150, 20);
        this.txtAutoYn.TabIndex = 13;
        //
        // lblCust
        //
        this.lblCust.Location = new System.Drawing.Point(28, 99);
        this.lblCust.Name = "lblCust";
        this.lblCust.Size = new System.Drawing.Size(36, 15);
        this.lblCust.TabIndex = 14;
        this.lblCust.Text = "거래처";
        //
        // txtCustNm
        //
        this.txtCustNm.Location = new System.Drawing.Point(100, 96);
        this.txtCustNm.LookupKey = "P_CUST";
        this.txtCustNm.PopupConditions = "p_cust_class=PO";
        this.txtCustNm.MatchField = "cust_nm";
        this.txtCustNm.Name = "txtCustNm";
        this.txtCustNm.Size = new System.Drawing.Size(150, 20);
        this.txtCustNm.TabIndex = 15;
        //
        // txtCustId
        //
        this.txtCustId.Location = new System.Drawing.Point(100, 96);
        this.txtCustId.Name = "txtCustId";
        this.txtCustId.Size = new System.Drawing.Size(150, 20);
        this.txtCustId.TabIndex = 16;
        this.txtCustId.Visible = false;
        //
        // lblDept
        //
        this.lblDept.Location = new System.Drawing.Point(40, 127);
        this.lblDept.Name = "lblDept";
        this.lblDept.Size = new System.Drawing.Size(24, 15);
        this.lblDept.TabIndex = 17;
        this.lblDept.Text = "부서";
        //
        // txtDeptNm
        //
        this.txtDeptNm.Location = new System.Drawing.Point(100, 124);
        this.txtDeptNm.LookupKey = "P_DEPT";
        this.txtDeptNm.MatchField = "dept_nm";
        this.txtDeptNm.Name = "txtDeptNm";
        this.txtDeptNm.Size = new System.Drawing.Size(150, 20);
        this.txtDeptNm.TabIndex = 18;
        //
        // txtDeptId
        //
        this.txtDeptId.Location = new System.Drawing.Point(100, 124);
        this.txtDeptId.Name = "txtDeptId";
        this.txtDeptId.Size = new System.Drawing.Size(150, 20);
        this.txtDeptId.TabIndex = 19;
        this.txtDeptId.Visible = false;
        //
        // lblEmp
        //
        this.lblEmp.Location = new System.Drawing.Point(282, 127);
        this.lblEmp.Name = "lblEmp";
        this.lblEmp.Size = new System.Drawing.Size(36, 15);
        this.lblEmp.TabIndex = 20;
        this.lblEmp.Text = "검사자";
        //
        // txtEmpNm
        //
        this.txtEmpNm.Location = new System.Drawing.Point(350, 124);
        this.txtEmpNm.LookupKey = "P_EMP";
        this.txtEmpNm.MatchField = "emp_nm";
        this.txtEmpNm.Name = "txtEmpNm";
        this.txtEmpNm.Size = new System.Drawing.Size(150, 20);
        this.txtEmpNm.TabIndex = 21;
        //
        // txtEmpId
        //
        this.txtEmpId.Location = new System.Drawing.Point(350, 124);
        this.txtEmpId.Name = "txtEmpId";
        this.txtEmpId.Size = new System.Drawing.Size(150, 20);
        this.txtEmpId.TabIndex = 22;
        this.txtEmpId.Visible = false;
        //
        // lblCfmDt
        //
        this.lblCfmDt.Location = new System.Drawing.Point(16, 155);
        this.lblCfmDt.Name = "lblCfmDt";
        this.lblCfmDt.Size = new System.Drawing.Size(48, 15);
        this.lblCfmDt.TabIndex = 23;
        this.lblCfmDt.Text = "확정일시";
        //
        // txtCfmDt
        //
        this.txtCfmDt.Location = new System.Drawing.Point(100, 152);
        this.txtCfmDt.Name = "txtCfmDt";
        this.txtCfmDt.Properties.ReadOnly = true;
        this.txtCfmDt.Size = new System.Drawing.Size(150, 20);
        this.txtCfmDt.TabIndex = 24;
        //
        // lblCfmUserId
        //
        this.lblCfmUserId.Location = new System.Drawing.Point(282, 155);
        this.lblCfmUserId.Name = "lblCfmUserId";
        this.lblCfmUserId.Size = new System.Drawing.Size(36, 15);
        this.lblCfmUserId.TabIndex = 25;
        this.lblCfmUserId.Text = "확정자";
        //
        // txtCfmUserId
        //
        this.txtCfmUserId.Location = new System.Drawing.Point(350, 152);
        this.txtCfmUserId.Name = "txtCfmUserId";
        this.txtCfmUserId.Properties.ReadOnly = true;
        this.txtCfmUserId.Size = new System.Drawing.Size(150, 20);
        this.txtCfmUserId.TabIndex = 26;
        //
        // lblRemark
        //
        this.lblRemark.Location = new System.Drawing.Point(40, 183);
        this.lblRemark.Name = "lblRemark";
        this.lblRemark.Size = new System.Drawing.Size(24, 15);
        this.lblRemark.TabIndex = 27;
        this.lblRemark.Text = "비고";
        //
        // memoRemark
        //
        this.memoRemark.Location = new System.Drawing.Point(100, 180);
        this.memoRemark.Name = "memoRemark";
        this.memoRemark.Size = new System.Drawing.Size(600, 25);
        this.memoRemark.TabIndex = 28;
        //
        // btnConfirm
        //
        this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnConfirm.Location = new System.Drawing.Point(560, 12);
        this.btnConfirm.Name = "btnConfirm";
        this.btnConfirm.Size = new System.Drawing.Size(100, 24);
        this.btnConfirm.TabIndex = 29;
        this.btnConfirm.Text = "확정";
        this.btnConfirm.ToolTip = "입고를 확정합니다. 확정하면 수불이 생성되고 재고관리 품목은 현재고에 반영됩니다.";
        //
        // btnConfirmCancel
        //
        this.btnConfirmCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnConfirmCancel.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnConfirmCancel.Location = new System.Drawing.Point(560, 40);
        this.btnConfirmCancel.Name = "btnConfirmCancel";
        this.btnConfirmCancel.Size = new System.Drawing.Size(100, 24);
        this.btnConfirmCancel.TabIndex = 30;
        this.btnConfirmCancel.Text = "확정취소";
        this.btnConfirmCancel.ToolTip = "확정을 취소합니다(수불 삭제, 재고 차감 - 재고가 모자라면 불가).";
        //
        // panelWyn6
        //
        this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
        this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn6.Location = new System.Drawing.Point(0, 0);
        this.panelWyn6.Name = "panelWyn6";
        this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
        this.panelWyn6.Size = new System.Drawing.Size(1664, 27);
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
        this.sectionHeaderWyn3.Size = new System.Drawing.Size(1659, 25);
        this.sectionHeaderWyn3.TabIndex = 9;
        this.sectionHeaderWyn3.Text = "구매입고 정보등록";
        //
        // lblSearchGrNo
        //
        this.lblSearchGrNo.Location = new System.Drawing.Point(224, 18);
        this.lblSearchGrNo.Name = "lblSearchGrNo";
        this.lblSearchGrNo.Size = new System.Drawing.Size(48, 15);
        this.lblSearchGrNo.TabIndex = 31;
        this.lblSearchGrNo.Text = "입고번호";
        //
        // txtSearchGrNo
        //
        this.txtSearchGrNo.Location = new System.Drawing.Point(280, 15);
        this.txtSearchGrNo.Name = "txtSearchGrNo";
        this.txtSearchGrNo.Size = new System.Drawing.Size(180, 20);
        this.txtSearchGrNo.TabIndex = 32;
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblSearchAccId);
        this.panHeader.Controls.Add(this.cboSearchAccId);
        this.panHeader.Controls.Add(this.lblSearchGrNo);
        this.panHeader.Controls.Add(this.txtSearchGrNo);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.Location = new System.Drawing.Point(5, 0);
        this.panHeader.Name = "panHeader";
        this.panHeader.Size = new System.Drawing.Size(1670, 49);
        this.panHeader.TabIndex = 8;
        // 
        // lblSearchAccId (조회조건 첫 번째 - 사업장 표준, 2026-10-03)
        // 
        this.lblSearchAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchAccId.Appearance.Options.UseFont = true;
        this.lblSearchAccId.Location = new System.Drawing.Point(25, 18);
        this.lblSearchAccId.Name = "lblSearchAccId";
        this.lblSearchAccId.Size = new System.Drawing.Size(36, 15);
        this.lblSearchAccId.TabIndex = 0;
        this.lblSearchAccId.Text = "사업장";
        // 
        // cboSearchAccId (사업장 LookUp, Required - 기본값은 화면 생성자에서 로그인 사업장)
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
        // frmGr
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 800);
        this.Controls.Add(this.panBase);
        this.Name = "frmGr";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
        this.panelWyn3.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
        this.panelWyn4.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolWh)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolLoc)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkcolStock)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolSrc)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
        this.panelWyn1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
        this.panelWyn7.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
        this.panelWyn5.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
        this.panData.ResumeLayout(false);
        this.panData.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtGrNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteGrDate.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteGrDate.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboTransType.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtAutoYn.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCustNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCustId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCfmDt.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCfmUserId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
        this.panelWyn6.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchGrNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn4;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private PopupLookupColumnEdit popcolWh;
    private PopupLookupColumnEdit popcolLoc;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkcolStock;
    private LookUpColumnEdit lookupcolSrc;
    private DevExpress.XtraGrid.Columns.GridColumn colSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colLotNo;
    private DevExpress.XtraGrid.Columns.GridColumn colSrcType;
    private DevExpress.XtraGrid.Columns.GridColumn colSrcNo;
    private DevExpress.XtraGrid.Columns.GridColumn colPoNo;
    private DevExpress.XtraGrid.Columns.GridColumn colReadyQty;
    private DevExpress.XtraGrid.Columns.GridColumn colGrQty;
    private DevExpress.XtraGrid.Columns.GridColumn colWhId;
    private DevExpress.XtraGrid.Columns.GridColumn colLocId;
    private DevExpress.XtraGrid.Columns.GridColumn colStockYn;
    private DevExpress.XtraGrid.Columns.GridColumn colNextQty;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private PanelWyn panelWyn1;
    private PanelWyn panelWyn7;
    private ButtonWyn btnDeletRow1;
    private ButtonWyn btnLoadReady;
    private SectionHeaderWyn sectionHeaderWyn2;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblAccId;
    private LookUpEditWyn cboAccId;
    private DevExpress.XtraEditors.LabelControl lblStatCd;
    private LookUpEditWyn cboStatCd;
    private DevExpress.XtraEditors.LabelControl lblGrNo;
    private TextEditWyn txtGrNo;
    private DevExpress.XtraEditors.LabelControl lblGrDate;
    private DateEditWyn dteGrDate;
    private DevExpress.XtraEditors.LabelControl lblTransType;
    private LookUpEditWyn cboTransType;
    private DevExpress.XtraEditors.LabelControl lblAutoYn;
    private TextEditWyn txtAutoYn;
    private DevExpress.XtraEditors.LabelControl lblCust;
    private PopupLookupEditWyn txtCustNm;
    private TextEditWyn txtCustId;
    private DevExpress.XtraEditors.LabelControl lblDept;
    private PopupLookupEditWyn txtDeptNm;
    private TextEditWyn txtDeptId;
    private DevExpress.XtraEditors.LabelControl lblEmp;
    private PopupLookupEditWyn txtEmpNm;
    private TextEditWyn txtEmpId;
    private DevExpress.XtraEditors.LabelControl lblCfmDt;
    private TextEditWyn txtCfmDt;
    private DevExpress.XtraEditors.LabelControl lblCfmUserId;
    private TextEditWyn txtCfmUserId;
    private DevExpress.XtraEditors.LabelControl lblRemark;
    private DevExpress.XtraEditors.MemoEdit memoRemark;
    private ButtonWyn btnConfirm;
    private ButtonWyn btnConfirmCancel;
    private PanelWyn panelWyn6;
    private SectionHeaderWyn sectionHeaderWyn3;
    private DevExpress.XtraEditors.LabelControl lblSearchGrNo;
    private TextEditWyn txtSearchGrNo;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private WYNLAB.Base.Controls.LookUpEditWyn cboSearchAccId;
}
