// 기타입고등록(frmEtcIn) - 기초재고와 같은 Master-One Sheet(TMAETCINM/TMAETCIND) + 입고유형. 확정하면 수불(입고유형)이 생기고 재고관리 품목은 현재고에 반영된다.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

public partial class frmEtcIn
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
        this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
        this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
        this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
        this.featBar = new WYNLAB.Popup.FeatureBarWyn();
        this.panData = new WYNLAB.Base.Controls.PanelWyn();
        this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.popcolItem = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
        this.popcolWh = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
        this.popcolLoc = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
        this.chkcolStock = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
        this.colSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLotNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colInQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colWhId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLocId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLocNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colStockYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.btnAddRow1 = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnDeletRow1 = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnPickItem = new WYNLAB.Base.Controls.ButtonWyn();
        this.lblAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblStatCd = new DevExpress.XtraEditors.LabelControl();
        this.cboStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblDocNo = new DevExpress.XtraEditors.LabelControl();
        this.txtDocNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDocDate = new DevExpress.XtraEditors.LabelControl();
        this.dteDocDate = new WYNLAB.Base.Controls.DateEditWyn();
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
        this.lblTransType = new DevExpress.XtraEditors.LabelControl();
        this.cboTransType = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblRemark = new DevExpress.XtraEditors.LabelControl();
        this.memoRemark = new DevExpress.XtraEditors.MemoEdit();
        this.btnConfirm = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnConfirmCancel = new WYNLAB.Base.Controls.ButtonWyn();
        this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblSearchNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchNo = new WYNLAB.Base.Controls.TextEditWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.featBar)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolItem)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolWh)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolLoc)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkcolStock)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDocNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteDocDate.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteDocDate.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCfmDt.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCfmUserId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboTransType.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchNo.Properties)).BeginInit();
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
        this.panelWyn4.Controls.Add(this.featBar);
        this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelWyn4.Location = new System.Drawing.Point(0, 0);
        this.panelWyn4.Name = "panelWyn4";
        this.panelWyn4.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.panelWyn4.Size = new System.Drawing.Size(1670, 746);
        this.panelWyn4.TabIndex = 7;
        //
        // featBar (공통 기능 버튼 패널 - 메뉴등록 '화면 기능'에서 켠 전자결재/첨부파일 버튼만 보이고, 켠 기능이 없으면 숨는다)
        //
        this.featBar.Dock = System.Windows.Forms.DockStyle.Top;
        this.featBar.Location = new System.Drawing.Point(3, 0);
        this.featBar.Name = "featBar";
        this.featBar.Size = new System.Drawing.Size(1664, 33);
        this.featBar.TabIndex = 10;
        //
        // popcolItem
        //
        this.popcolItem.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "...")});
        this.popcolItem.LookupKey = "P_ITEM";
        this.popcolItem.Name = "popcolItem";
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
        this.popcolItem,
                this.popcolWh,
                this.popcolLoc,
                this.chkcolStock});
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
                this.colItemId,
                this.colItemNo,
                this.colItemNm,
                this.colItemSpec,
                this.colUnitCd,
                this.colLotNo,
                this.colInQty,
                this.colWhId,
                this.colWhNm,
                this.colLocId,
                this.colLocNm,
                this.colStockYn,
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
        this.colItemId.Width = 60;
        //
        // colItemNo
        //
        this.colItemNo.Caption = "품번";
        this.colItemNo.ColumnEdit = this.popcolItem;
        this.colItemNo.FieldName = "item_no";
        this.colItemNo.Name = "colItemNo";
        this.colItemNo.Visible = true;
        this.colItemNo.VisibleIndex = 1;
        this.colItemNo.Width = 110;
        //
        // colItemNm
        //
        this.colItemNm.Caption = "품명";
        this.colItemNm.FieldName = "item_nm";
        this.colItemNm.Name = "colItemNm";
        this.colItemNm.OptionsColumn.AllowEdit = false;
        this.colItemNm.Visible = true;
        this.colItemNm.VisibleIndex = 2;
        this.colItemNm.Width = 150;
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
        this.colLotNo.Visible = true;
        this.colLotNo.VisibleIndex = 5;
        this.colLotNo.Width = 100;
        //
        // colInQty
        //
        this.colInQty.Caption = "입고수량";
        this.colInQty.DisplayFormat.FormatString = "#,##0.####";
        this.colInQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colInQty.FieldName = "in_qty";
        this.colInQty.Name = "colInQty";
        this.colInQty.Visible = true;
        this.colInQty.VisibleIndex = 6;
        this.colInQty.Width = 90;
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
        this.colWhNm.VisibleIndex = 7;
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
        this.colLocNm.VisibleIndex = 8;
        this.colLocNm.Width = 100;
        //
        // colStockYn
        //
        this.colStockYn.Caption = "재고반영";
        this.colStockYn.ColumnEdit = this.chkcolStock;
        this.colStockYn.FieldName = "stock_yn";
        this.colStockYn.Name = "colStockYn";
        this.colStockYn.OptionsColumn.AllowEdit = false;
        this.colStockYn.Visible = true;
        this.colStockYn.VisibleIndex = 11;
        this.colStockYn.Width = 55;
        //
        // colRemark
        //
        this.colRemark.Caption = "비고";
        this.colRemark.FieldName = "remark";
        this.colRemark.Name = "colRemark";
        this.colRemark.Visible = true;
        this.colRemark.VisibleIndex = 12;
        this.colRemark.Width = 180;
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
        this.panelWyn7.Controls.Add(this.btnAddRow1);
        this.panelWyn7.Controls.Add(this.btnDeletRow1);
        this.panelWyn7.Controls.Add(this.btnPickItem);
        this.panelWyn7.Appearance.BackColor = System.Drawing.Color.White;
        this.panelWyn7.Appearance.Options.UseBackColor = true;
        this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn7.Location = new System.Drawing.Point(1520, 0);
        this.panelWyn7.Name = "panelWyn7";
        this.panelWyn7.Padding = new System.Windows.Forms.Padding(5, 3, 0, 2);
        this.panelWyn7.Size = new System.Drawing.Size(144, 30);
        this.panelWyn7.TabIndex = 9;
        //
        // btnAddRow1
        //
        this.btnAddRow1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
        this.btnAddRow1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(229)))), ((int)(((byte)(205)))));
        this.btnAddRow1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(239)))));
        this.btnAddRow1.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnAddRow1.Location = new System.Drawing.Point(6, 4);
        this.btnAddRow1.Name = "btnAddRow1";
        this.btnAddRow1.Size = new System.Drawing.Size(60, 24);
        this.btnAddRow1.TabIndex = 0;
        this.btnAddRow1.Text = "행추가";
        //
        //
        // btnPickItem (품목 여러 건 선택 - 공통 팝업 다중선택 모드, 고른 만큼 행 추가)
        //
        this.btnPickItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
        this.btnPickItem.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnPickItem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
        this.btnPickItem.Location = new System.Drawing.Point(130, 4);
        this.btnPickItem.Name = "btnPickItem";
        this.btnPickItem.Size = new System.Drawing.Size(80, 24);
        this.btnPickItem.TabIndex = 2;
        this.btnPickItem.Text = "품목선택";
        this.btnPickItem.ToolTip = "품목 팝업에서 여러 건을 체크해 한 번에 추가합니다. 이미 담은 품목도 다시 고를 수 있습니다.";
        //
        // btnDeletRow1
        //
        this.btnDeletRow1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
        this.btnDeletRow1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(198)))), ((int)(((byte)(193)))));
        this.btnDeletRow1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(236)))), ((int)(((byte)(234)))));
        this.btnDeletRow1.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnDeletRow1.Location = new System.Drawing.Point(68, 4);
        this.btnDeletRow1.Name = "btnDeletRow1";
        this.btnDeletRow1.Size = new System.Drawing.Size(60, 24);
        this.btnDeletRow1.TabIndex = 1;
        this.btnDeletRow1.Text = "행삭제";
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
        this.sectionHeaderWyn2.Text = "기타입고 품목 상세정보 등록";
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
        this.sectionHeaderWyn3.Text = "기타입고 정보등록";
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
        this.panData.Controls.Add(this.lblDocNo);
        this.panData.Controls.Add(this.txtDocNo);
        this.panData.Controls.Add(this.lblDocDate);
        this.panData.Controls.Add(this.dteDocDate);
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
        this.panData.Controls.Add(this.lblTransType);
        this.panData.Controls.Add(this.cboTransType);
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
        this.lblAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblAccId.Appearance.Options.UseFont = true;
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
        this.lblStatCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblStatCd.Appearance.Options.UseFont = true;
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
        // lblDocNo
        //
        this.lblDocNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDocNo.Appearance.Options.UseFont = true;
        this.lblDocNo.Location = new System.Drawing.Point(16, 43);
        this.lblDocNo.Name = "lblDocNo";
        this.lblDocNo.Size = new System.Drawing.Size(60, 15);
        this.lblDocNo.TabIndex = 6;
        this.lblDocNo.Text = "입고번호";
        //
        // txtDocNo
        //
        this.txtDocNo.Location = new System.Drawing.Point(100, 40);
        this.txtDocNo.Name = "txtDocNo";
        this.txtDocNo.Properties.ReadOnly = true;
        this.txtDocNo.Size = new System.Drawing.Size(150, 20);
        this.txtDocNo.TabIndex = 7;
        //
        // lblDocDate
        //
        this.lblDocDate.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDocDate.Appearance.Options.UseFont = true;
        this.lblDocDate.Location = new System.Drawing.Point(270, 43);
        this.lblDocDate.Name = "lblDocDate";
        this.lblDocDate.Size = new System.Drawing.Size(48, 15);
        this.lblDocDate.TabIndex = 8;
        this.lblDocDate.Text = "입고일자";
        //
        // dteDocDate
        //
        this.dteDocDate.Location = new System.Drawing.Point(350, 40);
        this.dteDocDate.Name = "dteDocDate";
        this.dteDocDate.Size = new System.Drawing.Size(150, 20);
        this.dteDocDate.TabIndex = 9;
        //
        // lblDept
        //
        this.lblDept.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDept.Appearance.Options.UseFont = true;
        this.lblDept.Location = new System.Drawing.Point(40, 71);
        this.lblDept.Name = "lblDept";
        this.lblDept.Size = new System.Drawing.Size(24, 15);
        this.lblDept.TabIndex = 10;
        this.lblDept.Text = "부서";
        //
        // txtDeptNm
        //
        this.txtDeptNm.Location = new System.Drawing.Point(100, 68);
        this.txtDeptNm.LookupKey = "P_DEPT";
        this.txtDeptNm.MatchField = "dept_nm";
        this.txtDeptNm.Name = "txtDeptNm";
        this.txtDeptNm.Size = new System.Drawing.Size(150, 20);
        //
        // txtDeptId
        //
        this.txtDeptId.Location = new System.Drawing.Point(100, 68);
        this.txtDeptId.Name = "txtDeptId";
        this.txtDeptId.Size = new System.Drawing.Size(150, 20);
        this.txtDeptId.Visible = false;
        this.txtDeptNm.TabIndex = 11;
        //
        // lblEmp
        //
        this.lblEmp.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblEmp.Appearance.Options.UseFont = true;
        this.lblEmp.Location = new System.Drawing.Point(282, 71);
        this.lblEmp.Name = "lblEmp";
        this.lblEmp.Size = new System.Drawing.Size(36, 15);
        this.lblEmp.TabIndex = 12;
        this.lblEmp.Text = "담당자";
        //
        // txtEmpNm
        //
        this.txtEmpNm.Location = new System.Drawing.Point(350, 68);
        this.txtEmpNm.LookupKey = "P_EMP";
        this.txtEmpNm.MatchField = "emp_nm";
        this.txtEmpNm.Name = "txtEmpNm";
        this.txtEmpNm.Size = new System.Drawing.Size(150, 20);
        //
        // txtEmpId
        //
        this.txtEmpId.Location = new System.Drawing.Point(350, 68);
        this.txtEmpId.Name = "txtEmpId";
        this.txtEmpId.Size = new System.Drawing.Size(150, 20);
        this.txtEmpId.Visible = false;
        this.txtEmpNm.TabIndex = 13;
        //
        // lblCfmDt
        //
        this.lblCfmDt.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblCfmDt.Appearance.Options.UseFont = true;
        this.lblCfmDt.Location = new System.Drawing.Point(16, 99);
        this.lblCfmDt.Name = "lblCfmDt";
        this.lblCfmDt.Size = new System.Drawing.Size(48, 15);
        this.lblCfmDt.TabIndex = 14;
        this.lblCfmDt.Text = "확정일시";
        //
        // txtCfmDt
        //
        this.txtCfmDt.Location = new System.Drawing.Point(100, 96);
        this.txtCfmDt.Name = "txtCfmDt";
        this.txtCfmDt.Properties.ReadOnly = true;
        this.txtCfmDt.Size = new System.Drawing.Size(150, 20);
        this.txtCfmDt.TabIndex = 15;
        //
        // lblCfmUserId
        //
        this.lblCfmUserId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblCfmUserId.Appearance.Options.UseFont = true;
        this.lblCfmUserId.Location = new System.Drawing.Point(282, 99);
        this.lblCfmUserId.Name = "lblCfmUserId";
        this.lblCfmUserId.Size = new System.Drawing.Size(36, 15);
        this.lblCfmUserId.TabIndex = 16;
        this.lblCfmUserId.Text = "확정자";
        //
        // txtCfmUserId
        //
        this.txtCfmUserId.Location = new System.Drawing.Point(350, 96);
        this.txtCfmUserId.Name = "txtCfmUserId";
        this.txtCfmUserId.Properties.ReadOnly = true;
        this.txtCfmUserId.Size = new System.Drawing.Size(150, 20);
        this.txtCfmUserId.TabIndex = 17;
        //
        // lblTransType
        //
        this.lblTransType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblTransType.Appearance.Options.UseFont = true;
        this.lblTransType.Location = new System.Drawing.Point(16, 127);
        this.lblTransType.Name = "lblTransType";
        this.lblTransType.Size = new System.Drawing.Size(48, 15);
        this.lblTransType.TabIndex = 18;
        this.lblTransType.Text = "입고유형";
        //
        // cboTransType
        //
        this.cboTransType.Location = new System.Drawing.Point(100, 124);
        this.cboTransType.LookupKey = "L_MA0011_I";
        this.cboTransType.Name = "cboTransType";
        this.cboTransType.Size = new System.Drawing.Size(150, 20);
        this.cboTransType.TabIndex = 19;
        //
        // lblRemark
        //
        this.lblRemark.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblRemark.Appearance.Options.UseFont = true;
        this.lblRemark.Location = new System.Drawing.Point(40, 155);
        this.lblRemark.Name = "lblRemark";
        this.lblRemark.Size = new System.Drawing.Size(24, 15);
        this.lblRemark.TabIndex = 20;
        this.lblRemark.Text = "비고";
        //
        // memoRemark
        //
        this.memoRemark.Location = new System.Drawing.Point(100, 152);
        this.memoRemark.Name = "memoRemark";
        this.memoRemark.Size = new System.Drawing.Size(600, 25);
        this.memoRemark.TabIndex = 21;
        //
        // btnConfirm
        //
        this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
        this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnConfirm.Location = new System.Drawing.Point(560, 12);
        this.btnConfirm.Name = "btnConfirm";
        this.btnConfirm.Size = new System.Drawing.Size(100, 24);
        this.btnConfirm.TabIndex = 22;
        this.btnConfirm.Text = "확정";
        this.btnConfirm.ToolTip = "기타입고를 확정합니다. 확정하면 수불이 생성되고 재고관리 품목은 현재고에 반영됩니다.";
        //
        // btnConfirmCancel
        //
        this.btnConfirmCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
        this.btnConfirmCancel.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnConfirmCancel.Location = new System.Drawing.Point(560, 40);
        this.btnConfirmCancel.Name = "btnConfirmCancel";
        this.btnConfirmCancel.Size = new System.Drawing.Size(100, 24);
        this.btnConfirmCancel.TabIndex = 23;
        this.btnConfirmCancel.Text = "확정취소";
        this.btnConfirmCancel.ToolTip = "확정을 취소합니다(역거래 수불 추가, 재고 차감 - 재고가 모자라면 불가).";
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
        // lblSearchNo
        //
        this.lblSearchNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchNo.Appearance.Options.UseFont = true;
        this.lblSearchNo.Location = new System.Drawing.Point(224, 18);
        this.lblSearchNo.Name = "lblSearchNo";
        this.lblSearchNo.Size = new System.Drawing.Size(48, 15);
        this.lblSearchNo.TabIndex = 31;
        this.lblSearchNo.Text = "입고번호";
        //
        // txtSearchNo
        //
        this.txtSearchNo.Location = new System.Drawing.Point(280, 15);
        this.txtSearchNo.Name = "txtSearchNo";
        this.txtSearchNo.Size = new System.Drawing.Size(180, 20);
        this.txtSearchNo.TabIndex = 32;
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblSearchAccId);
        this.panHeader.Controls.Add(this.cboSearchAccId);
        this.panHeader.Controls.Add(this.lblSearchNo);
        this.panHeader.Controls.Add(this.txtSearchNo);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.Location = new System.Drawing.Point(5, 0);
        this.panHeader.Name = "panHeader";
        this.panHeader.Size = new System.Drawing.Size(1670, 49);
        this.panHeader.TabIndex = 8;
        //
        // lblSearchAccId (조회조건 첫 번째 - 사업장 표준)
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
        // frmEtcIn
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 800);
        this.Controls.Add(this.panBase);
        this.Name = "frmEtcIn";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.featBar)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolItem)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolWh)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolLoc)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkcolStock)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDocNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteDocDate.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteDocDate.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCfmDt.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCfmUserId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboTransType.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchNo.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn4;
    private PanelWyn panelWyn1;
    private PanelWyn panelWyn7;
    private PanelWyn panelWyn5;
    private WYNLAB.Popup.FeatureBarWyn featBar;
    private PanelWyn panData;
    private PanelWyn panelWyn6;
    private PanelWyn panHeader;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private PopupLookupColumnEdit popcolItem;
    private PopupLookupColumnEdit popcolWh;
    private PopupLookupColumnEdit popcolLoc;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkcolStock;
    private DevExpress.XtraGrid.Columns.GridColumn colSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colItemId;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colLotNo;
    private DevExpress.XtraGrid.Columns.GridColumn colInQty;
    private DevExpress.XtraGrid.Columns.GridColumn colWhId;
    private DevExpress.XtraGrid.Columns.GridColumn colWhNm;
    private DevExpress.XtraGrid.Columns.GridColumn colLocId;
    private DevExpress.XtraGrid.Columns.GridColumn colLocNm;
    private DevExpress.XtraGrid.Columns.GridColumn colStockYn;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private SectionHeaderWyn sectionHeaderWyn2;
    private SectionHeaderWyn sectionHeaderWyn3;
    private ButtonWyn btnAddRow1;
    private ButtonWyn btnDeletRow1;
    private ButtonWyn btnPickItem;
    private DevExpress.XtraEditors.LabelControl lblAccId;
    private LookUpEditWyn cboAccId;
    private DevExpress.XtraEditors.LabelControl lblStatCd;
    private LookUpEditWyn cboStatCd;
    private DevExpress.XtraEditors.LabelControl lblDocNo;
    private TextEditWyn txtDocNo;
    private DevExpress.XtraEditors.LabelControl lblDocDate;
    private DateEditWyn dteDocDate;
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
    private DevExpress.XtraEditors.LabelControl lblTransType;
    private LookUpEditWyn cboTransType;
    private DevExpress.XtraEditors.LabelControl lblRemark;
    private DevExpress.XtraEditors.MemoEdit memoRemark;
    private ButtonWyn btnConfirm;
    private ButtonWyn btnConfirmCancel;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private LookUpEditWyn cboSearchAccId;
    private DevExpress.XtraEditors.LabelControl lblSearchNo;
    private TextEditWyn txtSearchNo;
}
