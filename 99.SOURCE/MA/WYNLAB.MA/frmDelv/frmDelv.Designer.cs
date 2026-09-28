// ?⑺뭹?깅줉(frmDelv) - frmPo? 媛숈? Master-One Sheet 援ъ“(TMADELVM/TMADELVD ??? 2026-09-25). ?쇱씤? 諛쒖＜ 遺덈윭?ㅺ린濡쒕쭔 異붽??쒕떎.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

public partial class frmDelv
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
        this.chkcolQc = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
        this.colSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPoNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPoSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPoQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPoRemainQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDelvQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLotNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colQcYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colQcStatNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPoDelvDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colWhId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLocId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colNextQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
        this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
        this.btnDeletRow1 = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnLoadPo = new WYNLAB.Base.Controls.ButtonWyn();
        this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
        this.panData = new WYNLAB.Base.Controls.PanelWyn();
        this.lblAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblStatCd = new DevExpress.XtraEditors.LabelControl();
        this.cboStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblDelvNo = new DevExpress.XtraEditors.LabelControl();
        this.txtDelvNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDelvDate = new DevExpress.XtraEditors.LabelControl();
        this.dteDelvDate = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblCust = new DevExpress.XtraEditors.LabelControl();
        this.txtCustNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.txtCustId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblVendorDocNo = new DevExpress.XtraEditors.LabelControl();
        this.txtVendorDocNo = new WYNLAB.Base.Controls.TextEditWyn();
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
        this.lblSearchDelvNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchDelvNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
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
        ((System.ComponentModel.ISupportInitialize)(this.chkcolQc)).BeginInit();
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
        ((System.ComponentModel.ISupportInitialize)(this.txtDelvNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteDelvDate.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteDelvDate.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCustNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCustId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtVendorDocNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCfmDt.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCfmUserId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
        this.panelWyn6.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchDelvNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
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
        // chkcolQc
        //
        this.chkcolQc.AutoHeight = false;
        this.chkcolQc.Name = "chkcolQc";
        this.chkcolQc.ValueChecked = "Y";
        this.chkcolQc.ValueUnchecked = "N";
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd1.EmbeddedNavigator.Buttons.Append.Visible = false;
        this.grd1.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
        this.grd1.EmbeddedNavigator.Buttons.Edit.Visible = false;
        this.grd1.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
        this.grd1.EmbeddedNavigator.Buttons.Remove.Visible = false;
        this.grd1.Location = new System.Drawing.Point(3, 287);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.popcolWh,
                this.popcolLoc,
                this.chkcolQc});
        this.grd1.Size = new System.Drawing.Size(1664, 440);
        this.grd1.TabIndex = 0;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colSerl,
                this.colItemId,
                this.colItemNo,
                this.colItemNm,
                this.colItemSpec,
                this.colUnitCd,
                this.colPoNo,
                this.colPoSerl,
                this.colPoQty,
                this.colPoRemainQty,
                this.colDelvQty,
                this.colLotNo,
                this.colQcYn,
                this.colQcStatNm,
                this.colPoDelvDate,
                this.colWhId,
                this.colLocId,
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
        // colItemId
        //
        this.colItemId.Caption = "품목";
        this.colItemId.FieldName = "item_id";
        this.colItemId.Name = "colItemId";
        this.colItemId.OptionsColumn.AllowEdit = false;
        this.colItemId.Visible = false;
        this.colItemId.VisibleIndex = 1;
        this.colItemId.Width = 70;
        //
        // colItemNo
        //
        this.colItemNo.Caption = "품번";
        this.colItemNo.FieldName = "item_no";
        this.colItemNo.Name = "colItemNo";
        this.colItemNo.OptionsColumn.AllowEdit = false;
        this.colItemNo.Visible = true;
        this.colItemNo.VisibleIndex = 2;
        this.colItemNo.Width = 100;
        //
        // colItemNm
        //
        this.colItemNm.Caption = "품명";
        this.colItemNm.FieldName = "item_nm";
        this.colItemNm.Name = "colItemNm";
        this.colItemNm.OptionsColumn.AllowEdit = false;
        this.colItemNm.Visible = true;
        this.colItemNm.VisibleIndex = 3;
        this.colItemNm.Width = 140;
        //
        // colItemSpec
        //
        this.colItemSpec.Caption = "洹쒓꺽";
        this.colItemSpec.FieldName = "item_spec";
        this.colItemSpec.Name = "colItemSpec";
        this.colItemSpec.OptionsColumn.AllowEdit = false;
        this.colItemSpec.Visible = true;
        this.colItemSpec.VisibleIndex = 4;
        this.colItemSpec.Width = 110;
        //
        // colUnitCd
        //
        this.colUnitCd.Caption = "단위";
        this.colUnitCd.FieldName = "unit_cd";
        this.colUnitCd.Name = "colUnitCd";
        this.colUnitCd.OptionsColumn.AllowEdit = false;
        this.colUnitCd.Visible = true;
        this.colUnitCd.VisibleIndex = 5;
        this.colUnitCd.Width = 50;
        //
        // colPoNo
        //
        this.colPoNo.Caption = "諛쒖＜踰덊샇";
        this.colPoNo.FieldName = "src_no";
        this.colPoNo.Name = "colPoNo";
        this.colPoNo.OptionsColumn.AllowEdit = false;
        this.colPoNo.Visible = true;
        this.colPoNo.VisibleIndex = 6;
        this.colPoNo.Width = 110;
        //
        // colPoSerl
        //
        this.colPoSerl.Caption = "발주순번";
        this.colPoSerl.FieldName = "src_serl";
        this.colPoSerl.Name = "colPoSerl";
        this.colPoSerl.OptionsColumn.AllowEdit = false;
        this.colPoSerl.Visible = true;
        this.colPoSerl.VisibleIndex = 7;
        this.colPoSerl.Width = 60;
        //
        // colPoQty
        //
        this.colPoQty.Caption = "발주수량";
        this.colPoQty.DisplayFormat.FormatString = "#,##0.####";
        this.colPoQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colPoQty.FieldName = "po_qty";
        this.colPoQty.Name = "colPoQty";
        this.colPoQty.OptionsColumn.AllowEdit = false;
        this.colPoQty.Visible = true;
        this.colPoQty.VisibleIndex = 8;
        this.colPoQty.Width = 80;
        //
        // colPoRemainQty
        //
        this.colPoRemainQty.Caption = "발주잔량";
        this.colPoRemainQty.DisplayFormat.FormatString = "#,##0.####";
        this.colPoRemainQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colPoRemainQty.FieldName = "po_remain_qty";
        this.colPoRemainQty.Name = "colPoRemainQty";
        this.colPoRemainQty.OptionsColumn.AllowEdit = false;
        this.colPoRemainQty.Visible = true;
        this.colPoRemainQty.VisibleIndex = 9;
        this.colPoRemainQty.Width = 80;
        //
        // colDelvQty
        //
        this.colDelvQty.Caption = "납품수량";
        this.colDelvQty.DisplayFormat.FormatString = "#,##0.####";
        this.colDelvQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colDelvQty.FieldName = "delv_qty";
        this.colDelvQty.Name = "colDelvQty";
        this.colDelvQty.Visible = true;
        this.colDelvQty.VisibleIndex = 10;
        this.colDelvQty.Width = 90;
        //
        // colLotNo
        //
        this.colLotNo.Caption = "LOT";
        this.colLotNo.FieldName = "lot_no";
        this.colLotNo.Name = "colLotNo";
        this.colLotNo.Visible = true;
        this.colLotNo.VisibleIndex = 11;
        this.colLotNo.Width = 100;
        //
        // colQcYn
        //
        this.colQcYn.Caption = "검사여부";
        this.colQcYn.ColumnEdit = this.chkcolQc;
        this.colQcYn.FieldName = "qc_yn";
        this.colQcYn.Name = "colQcYn";
        this.colQcYn.OptionsColumn.AllowEdit = false;
        this.colQcYn.Visible = true;
        this.colQcYn.VisibleIndex = 12;
        this.colQcYn.Width = 45;
        //
        // colQcStatNm
        //
        this.colQcStatNm.Caption = "검사상태명";
        this.colQcStatNm.FieldName = "qc_stat_nm";
        this.colQcStatNm.Name = "colQcStatNm";
        this.colQcStatNm.OptionsColumn.AllowEdit = false;
        this.colQcStatNm.Visible = true;
        this.colQcStatNm.VisibleIndex = 13;
        this.colQcStatNm.Width = 80;
        //
        // colPoDelvDate
        //
        this.colPoDelvDate.Caption = "납기일";
        this.colPoDelvDate.FieldName = "po_delv_date";
        this.colPoDelvDate.Name = "colPoDelvDate";
        this.colPoDelvDate.OptionsColumn.AllowEdit = false;
        this.colPoDelvDate.Visible = true;
        this.colPoDelvDate.VisibleIndex = 14;
        this.colPoDelvDate.Width = 90;
        //
        // colWhId
        //
        this.colWhId.Caption = "李쎄퀬";
        this.colWhId.ColumnEdit = this.popcolWh;
        this.colWhId.FieldName = "wh_id";
        this.colWhId.Name = "colWhId";
        this.colWhId.Visible = true;
        this.colWhId.VisibleIndex = 15;
        this.colWhId.Width = 90;
        //
        // colLocId
        //
        this.colLocId.Caption = "위치";
        this.colLocId.ColumnEdit = this.popcolLoc;
        this.colLocId.FieldName = "loc_id";
        this.colLocId.Name = "colLocId";
        this.colLocId.Visible = true;
        this.colLocId.VisibleIndex = 16;
        this.colLocId.Width = 90;
        //
        // colNextQty
        //
        this.colNextQty.Caption = "검사입고처리";
        this.colNextQty.DisplayFormat.FormatString = "#,##0.####";
        this.colNextQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colNextQty.FieldName = "next_qty";
        this.colNextQty.Name = "colNextQty";
        this.colNextQty.OptionsColumn.AllowEdit = false;
        this.colNextQty.Visible = true;
        this.colNextQty.VisibleIndex = 17;
        this.colNextQty.Width = 90;
        //
        // colRemark
        //
        this.colRemark.Caption = "鍮꾧퀬";
        this.colRemark.FieldName = "remark";
        this.colRemark.Name = "colRemark";
        this.colRemark.Visible = true;
        this.colRemark.VisibleIndex = 18;
        this.colRemark.Width = 160;
        //
        // panelWyn1
        //
        this.panelWyn1.Controls.Add(this.sectionHeaderWyn2);
        this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn1.Location = new System.Drawing.Point(3, 260);
        this.panelWyn1.Name = "panelWyn1";
        this.panelWyn1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
        this.panelWyn1.Size = new System.Drawing.Size(1664, 27);
        this.panelWyn1.TabIndex = 8;
        //
        // panelWyn7
        //
        this.panelWyn7.Controls.Add(this.btnLoadPo);
        this.panelWyn7.Controls.Add(this.btnDeletRow1);
        this.panelWyn7.Appearance.BackColor = System.Drawing.Color.White;
        this.panelWyn7.Appearance.Options.UseBackColor = true;
        this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn7.Location = new System.Drawing.Point(1540, 0);
        this.panelWyn7.Name = "panelWyn7";
        this.panelWyn7.Padding = new System.Windows.Forms.Padding(5, 3, 0, 2);
        this.panelWyn7.Size = new System.Drawing.Size(124, 30);
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
        this.btnDeletRow1.Size = new System.Drawing.Size(58, 22);
        this.btnDeletRow1.Text = "행삭제";
        this.btnDeletRow1.TabIndex = 0;
        this.btnDeletRow1.ToolTip = "행삭제";
        //
        // btnLoadPo
        //
        this.btnLoadPo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
        this.btnLoadPo.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnLoadPo.Location = new System.Drawing.Point(68, 4);
        this.btnLoadPo.Name = "btnLoadPo";
        this.btnLoadPo.Size = new System.Drawing.Size(100, 22);
        this.btnLoadPo.TabIndex = 1;
        this.btnLoadPo.Text = "발주 불러오기";
        this.btnLoadPo.ToolTip = "승인 완료된 발주의 납품 가능 수량을 불러옵니다.";
        //
        // sectionHeaderWyn2
        //
        this.sectionHeaderWyn2.BackColor = System.Drawing.Color.White;
        this.sectionHeaderWyn2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.sectionHeaderWyn2.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.sectionHeaderWyn2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
        this.sectionHeaderWyn2.Location = new System.Drawing.Point(5, 0);
        this.sectionHeaderWyn2.Name = "sectionHeaderWyn2";
        this.sectionHeaderWyn2.Size = new System.Drawing.Size(1535, 25);
        this.sectionHeaderWyn2.TabIndex = 10;
        this.sectionHeaderWyn2.Text = "납품 품목 상세정보 등록";
        //
        // panelWyn5
        //
        this.panelWyn5.Controls.Add(this.panData);
        this.panelWyn5.Controls.Add(this.panelWyn6);
        this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn5.Location = new System.Drawing.Point(3, 0);
        this.panelWyn5.Name = "panelWyn5";
        this.panelWyn5.Size = new System.Drawing.Size(1664, 260);
        this.panelWyn5.TabIndex = 6;
        //
        // panData
        //
        this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        this.panData.Controls.Add(this.lblAccId);
        this.panData.Controls.Add(this.cboAccId);
        this.panData.Controls.Add(this.lblStatCd);
        this.panData.Controls.Add(this.cboStatCd);
        this.panData.Controls.Add(this.lblDelvNo);
        this.panData.Controls.Add(this.txtDelvNo);
        this.panData.Controls.Add(this.lblDelvDate);
        this.panData.Controls.Add(this.dteDelvDate);
        this.panData.Controls.Add(this.lblCust);
        this.panData.Controls.Add(this.txtCustNm);
        this.panData.Controls.Add(this.txtCustId);
        this.panData.Controls.Add(this.lblVendorDocNo);
        this.panData.Controls.Add(this.txtVendorDocNo);
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
        this.panData.Size = new System.Drawing.Size(1664, 233);
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
        this.lblStatCd.Location = new System.Drawing.Point(294, 15);
        this.lblStatCd.Name = "lblStatCd";
        this.lblStatCd.Size = new System.Drawing.Size(48, 15);
        this.lblStatCd.TabIndex = 4;
        this.lblStatCd.Text = "진행상태";
        //
        // cboStatCd
        //
        this.cboStatCd.Location = new System.Drawing.Point(350, 12);
        this.cboStatCd.LookupKey = "L_MA0005";
        this.cboStatCd.Name = "cboStatCd";
        this.cboStatCd.Properties.ReadOnly = true;
        this.cboStatCd.Size = new System.Drawing.Size(150, 20);
        this.cboStatCd.TabIndex = 5;
        //
        // lblDelvNo
        //
        this.lblDelvNo.Location = new System.Drawing.Point(16, 43);
        this.lblDelvNo.Name = "lblDelvNo";
        this.lblDelvNo.Size = new System.Drawing.Size(48, 15);
        this.lblDelvNo.TabIndex = 6;
        this.lblDelvNo.Text = "납품번호";
        //
        // txtDelvNo
        //
        this.txtDelvNo.Location = new System.Drawing.Point(100, 40);
        this.txtDelvNo.Name = "txtDelvNo";
        this.txtDelvNo.Properties.ReadOnly = true;
        this.txtDelvNo.Size = new System.Drawing.Size(150, 20);
        this.txtDelvNo.TabIndex = 7;
        //
        // lblDelvDate
        //
        this.lblDelvDate.Location = new System.Drawing.Point(294, 43);
        this.lblDelvDate.Name = "lblDelvDate";
        this.lblDelvDate.Size = new System.Drawing.Size(48, 15);
        this.lblDelvDate.TabIndex = 8;
        this.lblDelvDate.Text = "납품일자";
        //
        // dteDelvDate
        //
        this.dteDelvDate.Location = new System.Drawing.Point(350, 40);
        this.dteDelvDate.Name = "dteDelvDate";
        this.dteDelvDate.Size = new System.Drawing.Size(150, 20);
        this.dteDelvDate.TabIndex = 9;
        //
        // lblCust
        //
        this.lblCust.Location = new System.Drawing.Point(28, 71);
        this.lblCust.Name = "lblCust";
        this.lblCust.Size = new System.Drawing.Size(36, 15);
        this.lblCust.TabIndex = 10;
        this.lblCust.Text = "거래처";
        //
        // txtCustNm
        //
        this.txtCustNm.Location = new System.Drawing.Point(100, 68);
        this.txtCustNm.LookupKey = "P_CUST";
        this.txtCustNm.MatchField = "cust_nm";
        this.txtCustNm.Name = "txtCustNm";
        this.txtCustNm.Size = new System.Drawing.Size(150, 20);
        this.txtCustNm.TabIndex = 11;
        //
        // txtCustId
        //
        this.txtCustId.Location = new System.Drawing.Point(100, 68);
        this.txtCustId.Name = "txtCustId";
        this.txtCustId.Size = new System.Drawing.Size(150, 20);
        this.txtCustId.TabIndex = 12;
        this.txtCustId.Visible = false;
        //
        // lblVendorDocNo
        //
        this.lblVendorDocNo.Location = new System.Drawing.Point(270, 71);
        this.lblVendorDocNo.Name = "lblVendorDocNo";
        this.lblVendorDocNo.Size = new System.Drawing.Size(72, 15);
        this.lblVendorDocNo.TabIndex = 13;
        this.lblVendorDocNo.Text = "거래명세서번호";
        //
        // txtVendorDocNo
        //
        this.txtVendorDocNo.Location = new System.Drawing.Point(350, 68);
        this.txtVendorDocNo.Name = "txtVendorDocNo";
        this.txtVendorDocNo.Size = new System.Drawing.Size(150, 20);
        this.txtVendorDocNo.TabIndex = 14;
        //
        // lblDept
        //
        this.lblDept.Location = new System.Drawing.Point(40, 99);
        this.lblDept.Name = "lblDept";
        this.lblDept.Size = new System.Drawing.Size(24, 15);
        this.lblDept.TabIndex = 15;
        this.lblDept.Text = "부서";
        //
        // txtDeptNm
        //
        this.txtDeptNm.Location = new System.Drawing.Point(100, 96);
        this.txtDeptNm.LookupKey = "P_DEPT";
        this.txtDeptNm.MatchField = "dept_nm";
        this.txtDeptNm.Name = "txtDeptNm";
        this.txtDeptNm.Size = new System.Drawing.Size(150, 20);
        this.txtDeptNm.TabIndex = 16;
        //
        // txtDeptId
        //
        this.txtDeptId.Location = new System.Drawing.Point(100, 96);
        this.txtDeptId.Name = "txtDeptId";
        this.txtDeptId.Size = new System.Drawing.Size(150, 20);
        this.txtDeptId.TabIndex = 17;
        this.txtDeptId.Visible = false;
        //
        // lblEmp
        //
        this.lblEmp.Location = new System.Drawing.Point(306, 99);
        this.lblEmp.Name = "lblEmp";
        this.lblEmp.Size = new System.Drawing.Size(36, 15);
        this.lblEmp.TabIndex = 18;
        this.lblEmp.Text = "검사자";
        //
        // txtEmpNm
        //
        this.txtEmpNm.Location = new System.Drawing.Point(350, 96);
        this.txtEmpNm.LookupKey = "P_EMP";
        this.txtEmpNm.MatchField = "emp_nm";
        this.txtEmpNm.Name = "txtEmpNm";
        this.txtEmpNm.Size = new System.Drawing.Size(150, 20);
        this.txtEmpNm.TabIndex = 19;
        //
        // txtEmpId
        //
        this.txtEmpId.Location = new System.Drawing.Point(350, 96);
        this.txtEmpId.Name = "txtEmpId";
        this.txtEmpId.Size = new System.Drawing.Size(150, 20);
        this.txtEmpId.TabIndex = 20;
        this.txtEmpId.Visible = false;
        //
        // lblCfmDt
        //
        this.lblCfmDt.Location = new System.Drawing.Point(16, 127);
        this.lblCfmDt.Name = "lblCfmDt";
        this.lblCfmDt.Size = new System.Drawing.Size(48, 15);
        this.lblCfmDt.TabIndex = 21;
        this.lblCfmDt.Text = "확정일시";
        //
        // txtCfmDt
        //
        this.txtCfmDt.Location = new System.Drawing.Point(100, 124);
        this.txtCfmDt.Name = "txtCfmDt";
        this.txtCfmDt.Properties.ReadOnly = true;
        this.txtCfmDt.Size = new System.Drawing.Size(150, 20);
        this.txtCfmDt.TabIndex = 22;
        //
        // lblCfmUserId
        //
        this.lblCfmUserId.Location = new System.Drawing.Point(306, 127);
        this.lblCfmUserId.Name = "lblCfmUserId";
        this.lblCfmUserId.Size = new System.Drawing.Size(36, 15);
        this.lblCfmUserId.TabIndex = 23;
        this.lblCfmUserId.Text = "확정자";
        //
        // txtCfmUserId
        //
        this.txtCfmUserId.Location = new System.Drawing.Point(350, 124);
        this.txtCfmUserId.Name = "txtCfmUserId";
        this.txtCfmUserId.Properties.ReadOnly = true;
        this.txtCfmUserId.Size = new System.Drawing.Size(150, 20);
        this.txtCfmUserId.TabIndex = 24;
        //
        // lblRemark
        //
        this.lblRemark.Location = new System.Drawing.Point(40, 155);
        this.lblRemark.Name = "lblRemark";
        this.lblRemark.Size = new System.Drawing.Size(24, 15);
        this.lblRemark.TabIndex = 25;
        this.lblRemark.Text = "鍮꾧퀬";
        //
        // memoRemark
        //
        this.memoRemark.Location = new System.Drawing.Point(100, 152);
        this.memoRemark.Name = "memoRemark";
        this.memoRemark.Size = new System.Drawing.Size(600, 25);
        this.memoRemark.TabIndex = 26;
        //
        // btnConfirm
        //
        this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnConfirm.Location = new System.Drawing.Point(560, 12);
        this.btnConfirm.Name = "btnConfirm";
        this.btnConfirm.Size = new System.Drawing.Size(100, 24);
        this.btnConfirm.TabIndex = 27;
        this.btnConfirm.Text = "확정";
        this.btnConfirm.ToolTip = "납품을 확정합니다. 확정하면 발주 라인 납품수량에 반영됩니다.";
        //
        // btnConfirmCancel
        //
        this.btnConfirmCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnConfirmCancel.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnConfirmCancel.Location = new System.Drawing.Point(560, 40);
        this.btnConfirmCancel.Name = "btnConfirmCancel";
        this.btnConfirmCancel.Size = new System.Drawing.Size(100, 24);
        this.btnConfirmCancel.TabIndex = 28;
        this.btnConfirmCancel.Text = "확정취소";
        this.btnConfirmCancel.ToolTip = "확정을 취소합니다(검사 또는 입고가 진행된 납품은 불가).";
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
        this.sectionHeaderWyn3.Text = "납품 정보등록";
        //
        // lblSearchDelvNo
        //
        this.lblSearchDelvNo.Location = new System.Drawing.Point(25, 18);
        this.lblSearchDelvNo.Name = "lblSearchDelvNo";
        this.lblSearchDelvNo.Size = new System.Drawing.Size(48, 15);
        this.lblSearchDelvNo.TabIndex = 29;
        this.lblSearchDelvNo.Text = "납품번호";
        //
        // txtSearchDelvNo
        //
        this.txtSearchDelvNo.Location = new System.Drawing.Point(81, 15);
        this.txtSearchDelvNo.Name = "txtSearchDelvNo";
        this.txtSearchDelvNo.Size = new System.Drawing.Size(180, 20);
        this.txtSearchDelvNo.TabIndex = 30;
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblSearchDelvNo);
        this.panHeader.Controls.Add(this.txtSearchDelvNo);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.Location = new System.Drawing.Point(5, 0);
        this.panHeader.Name = "panHeader";
        this.panHeader.Size = new System.Drawing.Size(1670, 49);
        this.panHeader.TabIndex = 8;
        //
        // frmDelv
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 800);
        this.Controls.Add(this.panBase);
        this.Name = "frmDelv";
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
        ((System.ComponentModel.ISupportInitialize)(this.chkcolQc)).EndInit();
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
        ((System.ComponentModel.ISupportInitialize)(this.txtDelvNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteDelvDate.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteDelvDate.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCustNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCustId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtVendorDocNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCfmDt.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCfmUserId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
        this.panelWyn6.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchDelvNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn4;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private PopupLookupColumnEdit popcolWh;
    private PopupLookupColumnEdit popcolLoc;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkcolQc;
    private DevExpress.XtraGrid.Columns.GridColumn colSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colItemId;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colPoNo;
    private DevExpress.XtraGrid.Columns.GridColumn colPoSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colPoQty;
    private DevExpress.XtraGrid.Columns.GridColumn colPoRemainQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDelvQty;
    private DevExpress.XtraGrid.Columns.GridColumn colLotNo;
    private DevExpress.XtraGrid.Columns.GridColumn colQcYn;
    private DevExpress.XtraGrid.Columns.GridColumn colQcStatNm;
    private DevExpress.XtraGrid.Columns.GridColumn colPoDelvDate;
    private DevExpress.XtraGrid.Columns.GridColumn colWhId;
    private DevExpress.XtraGrid.Columns.GridColumn colLocId;
    private DevExpress.XtraGrid.Columns.GridColumn colNextQty;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private PanelWyn panelWyn1;
    private PanelWyn panelWyn7;
    private ButtonWyn btnDeletRow1;
    private ButtonWyn btnLoadPo;
    private SectionHeaderWyn sectionHeaderWyn2;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblAccId;
    private LookUpEditWyn cboAccId;
    private DevExpress.XtraEditors.LabelControl lblStatCd;
    private LookUpEditWyn cboStatCd;
    private DevExpress.XtraEditors.LabelControl lblDelvNo;
    private TextEditWyn txtDelvNo;
    private DevExpress.XtraEditors.LabelControl lblDelvDate;
    private DateEditWyn dteDelvDate;
    private DevExpress.XtraEditors.LabelControl lblCust;
    private PopupLookupEditWyn txtCustNm;
    private TextEditWyn txtCustId;
    private DevExpress.XtraEditors.LabelControl lblVendorDocNo;
    private TextEditWyn txtVendorDocNo;
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
    private DevExpress.XtraEditors.LabelControl lblSearchDelvNo;
    private TextEditWyn txtSearchDelvNo;
    private PanelWyn panHeader;
}
