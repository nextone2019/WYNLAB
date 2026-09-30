// 작업지시현황(frmWoStatus) - Master-SubGrid 조회 화면: 작업지시 목록(grd1) + 선택한 작업지시의 공정 진행(grd2) + LOT 현재 위치(grd3).
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

public partial class frmWoStatus
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
        this.grd3 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw3 = new WYNLAB.Base.Controls.GridViewWyn();
        this.colLotSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLotNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLotItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLotUnit = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLotInit = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLotWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLotStock = new DevExpress.XtraGrid.Columns.GridColumn();
        this.shLot = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
        this.lookupcolStat2 = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.colPSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPProcNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPCustNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPInItem = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPOutItem = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPDue = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPStat = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPIn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPGood = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPBad = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPYield = new DevExpress.XtraGrid.Columns.GridColumn();
        this.shProc = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
        this.splitterWyn2 = new WYNLAB.Base.Controls.SplitterWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.hyperlinkWoNo = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
        this.lookupcolStat = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.colWoNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colWoDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRouteNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colStartLot = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colStartQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colSoNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDelvDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCurProc = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCurGood = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCurUnit = new DevExpress.XtraGrid.Columns.GridColumn();
        this.shList = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.lblSearchDate = new DevExpress.XtraEditors.LabelControl();
        this.dteSearchFrom = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblTilde = new DevExpress.XtraEditors.LabelControl();
        this.dteSearchTo = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblSearchStat = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchStat = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblSearchWoNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchWoNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSearchLotNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchLotNo = new WYNLAB.Base.Controls.TextEditWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        this.panBase.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).BeginInit();
        this.panWork.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolStat2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.hyperlinkWoNo)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolStat)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchStat.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchWoNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchLotNo.Properties)).BeginInit();
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
        this.panWork.Controls.Add(this.grd3);
        this.panWork.Controls.Add(this.shLot);
        this.panWork.Controls.Add(this.splitterWyn2);
        this.panWork.Controls.Add(this.grd2);
        this.panWork.Controls.Add(this.shProc);
        this.panWork.Controls.Add(this.splitterWyn1);
        this.panWork.Controls.Add(this.grd1);
        this.panWork.Controls.Add(this.shList);
        this.panWork.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panWork.Location = new System.Drawing.Point(5, 49);
        this.panWork.Name = "panWork";
        this.panWork.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.panWork.Size = new System.Drawing.Size(1670, 746);
        this.panWork.TabIndex = 1;
        //
        // shList
        //
        this.shList.BackColor = System.Drawing.Color.White;
        this.shList.Dock = System.Windows.Forms.DockStyle.Top;
        this.shList.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shList.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.shList.Location = new System.Drawing.Point(3, 0);
        this.shList.Name = "shList";
        this.shList.Size = new System.Drawing.Size(1664, 27);
        this.shList.TabIndex = 0;
        this.shList.Text = "작업지시 목록 (작업지시번호를 더블클릭하면 작업지시 화면이 열립니다)";
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Top;
        this.grd1.Location = new System.Drawing.Point(3, 27);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.hyperlinkWoNo,
        this.lookupcolStat});
        this.grd1.Size = new System.Drawing.Size(1664, 240);
        this.grd1.TabIndex = 1;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colWoNo,
        this.colWoDate,
        this.colRouteNm,
        this.colItemNm,
        this.colStartLot,
        this.colStartQty,
        this.colSoNo,
        this.colDelvDate,
        this.colStatCd,
        this.colCurProc,
        this.colCurGood,
        this.colCurUnit});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsBehavior.Editable = false;
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        //
        // lookupcolStat
        //
        this.lookupcolStat.AutoHeight = false;
        this.lookupcolStat.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolStat.LookupKey = "L_PR0001";
        this.lookupcolStat.Name = "lookupcolStat";
        this.lookupcolStat.NullText = "";
        //
        // colWoNo
        //
        this.colWoNo.Caption = "작업지시번호";
        this.colWoNo.ColumnEdit = this.hyperlinkWoNo;
        this.colWoNo.FieldName = "wo_no";
        this.colWoNo.Name = "colWoNo";
        this.colWoNo.Visible = true;
        this.colWoNo.VisibleIndex = 0;
        this.colWoNo.Width = 120;
        //
        // hyperlinkWoNo
        //
        this.hyperlinkWoNo.Name = "hyperlinkWoNo";
        //
        // colWoDate
        //
        this.colWoDate.Caption = "작업일자";
        this.colWoDate.FieldName = "wo_date";
        this.colWoDate.Name = "colWoDate";
        this.colWoDate.Visible = true;
        this.colWoDate.VisibleIndex = 1;
        this.colWoDate.Width = 80;
        //
        // colRouteNm
        //
        this.colRouteNm.Caption = "라우팅";
        this.colRouteNm.FieldName = "route_nm";
        this.colRouteNm.Name = "colRouteNm";
        this.colRouteNm.Visible = true;
        this.colRouteNm.VisibleIndex = 2;
        this.colRouteNm.Width = 150;
        //
        // colItemNm
        //
        this.colItemNm.Caption = "완제품";
        this.colItemNm.FieldName = "item_nm";
        this.colItemNm.Name = "colItemNm";
        this.colItemNm.Visible = true;
        this.colItemNm.VisibleIndex = 3;
        this.colItemNm.Width = 130;
        //
        // colStartLot
        //
        this.colStartLot.Caption = "시작 LOT";
        this.colStartLot.FieldName = "start_lot_no";
        this.colStartLot.Name = "colStartLot";
        this.colStartLot.Visible = true;
        this.colStartLot.VisibleIndex = 4;
        this.colStartLot.Width = 120;
        //
        // colStartQty
        //
        this.colStartQty.Caption = "시작수량";
        this.colStartQty.DisplayFormat.FormatString = "#,##0.####";
        this.colStartQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colStartQty.FieldName = "start_qty";
        this.colStartQty.Name = "colStartQty";
        this.colStartQty.Visible = true;
        this.colStartQty.VisibleIndex = 5;
        this.colStartQty.Width = 75;
        //
        // colSoNo
        //
        this.colSoNo.Caption = "수주번호";
        this.colSoNo.FieldName = "so_no";
        this.colSoNo.Name = "colSoNo";
        this.colSoNo.Visible = true;
        this.colSoNo.VisibleIndex = 6;
        this.colSoNo.Width = 110;
        //
        // colDelvDate
        //
        this.colDelvDate.Caption = "납기";
        this.colDelvDate.FieldName = "delv_date";
        this.colDelvDate.Name = "colDelvDate";
        this.colDelvDate.Visible = true;
        this.colDelvDate.VisibleIndex = 7;
        this.colDelvDate.Width = 80;
        //
        // colStatCd
        //
        this.colStatCd.Caption = "상태";
        this.colStatCd.ColumnEdit = this.lookupcolStat;
        this.colStatCd.FieldName = "stat_cd";
        this.colStatCd.Name = "colStatCd";
        this.colStatCd.Visible = true;
        this.colStatCd.VisibleIndex = 8;
        this.colStatCd.Width = 60;
        //
        // colCurProc
        //
        this.colCurProc.Caption = "진행 공정";
        this.colCurProc.FieldName = "cur_proc_nm";
        this.colCurProc.Name = "colCurProc";
        this.colCurProc.Visible = true;
        this.colCurProc.VisibleIndex = 9;
        this.colCurProc.Width = 100;
        //
        // colCurGood
        //
        this.colCurGood.Caption = "그 공정 양품";
        this.colCurGood.DisplayFormat.FormatString = "#,##0.####";
        this.colCurGood.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colCurGood.FieldName = "cur_good_qty";
        this.colCurGood.Name = "colCurGood";
        this.colCurGood.Visible = true;
        this.colCurGood.VisibleIndex = 10;
        this.colCurGood.Width = 90;
        //
        // colCurUnit
        //
        this.colCurUnit.Caption = "단위";
        this.colCurUnit.FieldName = "cur_unit_cd";
        this.colCurUnit.Name = "colCurUnit";
        this.colCurUnit.Visible = true;
        this.colCurUnit.VisibleIndex = 11;
        this.colCurUnit.Width = 50;
        //
        // splitterWyn1
        //
        this.splitterWyn1.BackColor = System.Drawing.Color.White;
        this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Top;
        this.splitterWyn1.Location = new System.Drawing.Point(3, 267);
        this.splitterWyn1.Name = "splitterWyn1";
        this.splitterWyn1.Size = new System.Drawing.Size(1664, 10);
        this.splitterWyn1.TabIndex = 6;
        this.splitterWyn1.TabStop = false;
        //
        // splitterWyn2
        //
        this.splitterWyn2.BackColor = System.Drawing.Color.White;
        this.splitterWyn2.Dock = System.Windows.Forms.DockStyle.Top;
        this.splitterWyn2.Location = new System.Drawing.Point(3, 474);
        this.splitterWyn2.Name = "splitterWyn2";
        this.splitterWyn2.Size = new System.Drawing.Size(1664, 10);
        this.splitterWyn2.TabIndex = 7;
        this.splitterWyn2.TabStop = false;
        //
        // shProc
        //
        this.shProc.BackColor = System.Drawing.Color.White;
        this.shProc.Dock = System.Windows.Forms.DockStyle.Top;
        this.shProc.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shProc.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.shProc.Location = new System.Drawing.Point(3, 267);
        this.shProc.Name = "shProc";
        this.shProc.Size = new System.Drawing.Size(1664, 27);
        this.shProc.TabIndex = 2;
        this.shProc.Text = "공정별 외주 진행";
        //
        // grd2
        //
        this.grd2.Dock = System.Windows.Forms.DockStyle.Top;
        this.grd2.Location = new System.Drawing.Point(3, 294);
        this.grd2.MainView = this.gvw2;
        this.grd2.Name = "grd2";
        this.grd2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.lookupcolStat2});
        this.grd2.Size = new System.Drawing.Size(1664, 170);
        this.grd2.TabIndex = 3;
        this.grd2.UseEmbeddedNavigator = false;
        this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw2});
        //
        // gvw2
        //
        this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colPSerl,
        this.colPProcNm,
        this.colPCustNm,
        this.colPWhNm,
        this.colPInItem,
        this.colPOutItem,
        this.colPDue,
        this.colPStat,
        this.colPIn,
        this.colPGood,
        this.colPBad,
        this.colPYield});
        this.gvw2.GridControl = this.grd2;
        this.gvw2.HighlightFocusedRow = true;
        this.gvw2.Name = "gvw2";
        this.gvw2.OptionsBehavior.Editable = false;
        this.gvw2.OptionsView.ColumnAutoWidth = false;
        this.gvw2.OptionsView.ShowGroupPanel = false;
        //
        // lookupcolStat2
        //
        this.lookupcolStat2.AutoHeight = false;
        this.lookupcolStat2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolStat2.LookupKey = "L_PR0002";
        this.lookupcolStat2.Name = "lookupcolStat2";
        this.lookupcolStat2.NullText = "";
        //
        // colPSerl
        //
        this.colPSerl.Caption = "순번";
        this.colPSerl.FieldName = "serl";
        this.colPSerl.Name = "colPSerl";
        this.colPSerl.Visible = true;
        this.colPSerl.VisibleIndex = 0;
        this.colPSerl.Width = 45;
        //
        // colPProcNm
        //
        this.colPProcNm.Caption = "공정";
        this.colPProcNm.FieldName = "proc_nm";
        this.colPProcNm.Name = "colPProcNm";
        this.colPProcNm.Visible = true;
        this.colPProcNm.VisibleIndex = 1;
        this.colPProcNm.Width = 90;
        //
        // colPCustNm
        //
        this.colPCustNm.Caption = "외주처";
        this.colPCustNm.FieldName = "cust_nm";
        this.colPCustNm.Name = "colPCustNm";
        this.colPCustNm.Visible = true;
        this.colPCustNm.VisibleIndex = 2;
        this.colPCustNm.Width = 100;
        //
        // colPWhNm
        //
        this.colPWhNm.Caption = "외주처 창고";
        this.colPWhNm.FieldName = "wh_nm";
        this.colPWhNm.Name = "colPWhNm";
        this.colPWhNm.Visible = true;
        this.colPWhNm.VisibleIndex = 3;
        this.colPWhNm.Width = 110;
        //
        // colPInItem
        //
        this.colPInItem.Caption = "투입품목";
        this.colPInItem.FieldName = "in_item_nm";
        this.colPInItem.Name = "colPInItem";
        this.colPInItem.Visible = true;
        this.colPInItem.VisibleIndex = 4;
        this.colPInItem.Width = 110;
        //
        // colPOutItem
        //
        this.colPOutItem.Caption = "산출품목";
        this.colPOutItem.FieldName = "out_item_nm";
        this.colPOutItem.Name = "colPOutItem";
        this.colPOutItem.Visible = true;
        this.colPOutItem.VisibleIndex = 5;
        this.colPOutItem.Width = 110;
        //
        // colPDue
        //
        this.colPDue.Caption = "납기";
        this.colPDue.FieldName = "due_date";
        this.colPDue.Name = "colPDue";
        this.colPDue.Visible = true;
        this.colPDue.VisibleIndex = 6;
        this.colPDue.Width = 80;
        //
        // colPStat
        //
        this.colPStat.Caption = "상태";
        this.colPStat.ColumnEdit = this.lookupcolStat2;
        this.colPStat.FieldName = "stat_cd";
        this.colPStat.Name = "colPStat";
        this.colPStat.Visible = true;
        this.colPStat.VisibleIndex = 7;
        this.colPStat.Width = 60;
        //
        // colPIn
        //
        this.colPIn.Caption = "투입 누계";
        this.colPIn.DisplayFormat.FormatString = "#,##0.####";
        this.colPIn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colPIn.FieldName = "in_qty";
        this.colPIn.Name = "colPIn";
        this.colPIn.Visible = true;
        this.colPIn.VisibleIndex = 8;
        this.colPIn.Width = 85;
        //
        // colPGood
        //
        this.colPGood.Caption = "양품 누계";
        this.colPGood.DisplayFormat.FormatString = "#,##0.####";
        this.colPGood.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colPGood.FieldName = "good_qty";
        this.colPGood.Name = "colPGood";
        this.colPGood.Visible = true;
        this.colPGood.VisibleIndex = 9;
        this.colPGood.Width = 85;
        //
        // colPBad
        //
        this.colPBad.Caption = "불량 누계";
        this.colPBad.DisplayFormat.FormatString = "#,##0.####";
        this.colPBad.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colPBad.FieldName = "bad_qty";
        this.colPBad.Name = "colPBad";
        this.colPBad.Visible = true;
        this.colPBad.VisibleIndex = 10;
        this.colPBad.Width = 85;
        //
        // colPYield
        //
        this.colPYield.Caption = "수율(%)";
        this.colPYield.DisplayFormat.FormatString = "#,##0.##";
        this.colPYield.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colPYield.FieldName = "yield_rate";
        this.colPYield.Name = "colPYield";
        this.colPYield.Visible = true;
        this.colPYield.VisibleIndex = 11;
        this.colPYield.Width = 65;
        //
        // shLot
        //
        this.shLot.BackColor = System.Drawing.Color.White;
        this.shLot.Dock = System.Windows.Forms.DockStyle.Top;
        this.shLot.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shLot.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.shLot.Location = new System.Drawing.Point(3, 464);
        this.shLot.Name = "shLot";
        this.shLot.Size = new System.Drawing.Size(1664, 27);
        this.shLot.TabIndex = 4;
        this.shLot.Text = "LOT 현황 (현재 재고 위치 - 외주처 창고/이동중 재고 포함)";
        //
        // grd3
        //
        this.grd3.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd3.Location = new System.Drawing.Point(3, 491);
        this.grd3.MainView = this.gvw3;
        this.grd3.Name = "grd3";
        this.grd3.Size = new System.Drawing.Size(1664, 255);
        this.grd3.TabIndex = 5;
        this.grd3.UseEmbeddedNavigator = false;
        this.grd3.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw3});
        //
        // gvw3
        //
        this.gvw3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colLotSerl,
        this.colLotNo,
        this.colLotItemNm,
        this.colLotUnit,
        this.colLotInit,
        this.colLotWhNm,
        this.colLotStock});
        this.gvw3.GridControl = this.grd3;
        this.gvw3.HighlightFocusedRow = true;
        this.gvw3.Name = "gvw3";
        this.gvw3.OptionsBehavior.Editable = false;
        this.gvw3.OptionsView.ColumnAutoWidth = false;
        this.gvw3.OptionsView.ShowGroupPanel = false;
        //
        // colLotSerl
        //
        this.colLotSerl.Caption = "생성 공정";
        this.colLotSerl.FieldName = "wo_serl";
        this.colLotSerl.Name = "colLotSerl";
        this.colLotSerl.Visible = true;
        this.colLotSerl.VisibleIndex = 0;
        this.colLotSerl.Width = 70;
        //
        // colLotNo
        //
        this.colLotNo.Caption = "LOT";
        this.colLotNo.FieldName = "lot_no";
        this.colLotNo.Name = "colLotNo";
        this.colLotNo.Visible = true;
        this.colLotNo.VisibleIndex = 1;
        this.colLotNo.Width = 140;
        //
        // colLotItemNm
        //
        this.colLotItemNm.Caption = "품명";
        this.colLotItemNm.FieldName = "item_nm";
        this.colLotItemNm.Name = "colLotItemNm";
        this.colLotItemNm.Visible = true;
        this.colLotItemNm.VisibleIndex = 2;
        this.colLotItemNm.Width = 140;
        //
        // colLotUnit
        //
        this.colLotUnit.Caption = "단위";
        this.colLotUnit.FieldName = "unit_cd";
        this.colLotUnit.Name = "colLotUnit";
        this.colLotUnit.Visible = true;
        this.colLotUnit.VisibleIndex = 3;
        this.colLotUnit.Width = 50;
        //
        // colLotInit
        //
        this.colLotInit.Caption = "생성 수량";
        this.colLotInit.DisplayFormat.FormatString = "#,##0.####";
        this.colLotInit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colLotInit.FieldName = "init_qty";
        this.colLotInit.Name = "colLotInit";
        this.colLotInit.Visible = true;
        this.colLotInit.VisibleIndex = 4;
        this.colLotInit.Width = 90;
        //
        // colLotWhNm
        //
        this.colLotWhNm.Caption = "현재 위치(창고)";
        this.colLotWhNm.FieldName = "wh_nm";
        this.colLotWhNm.Name = "colLotWhNm";
        this.colLotWhNm.Visible = true;
        this.colLotWhNm.VisibleIndex = 5;
        this.colLotWhNm.Width = 130;
        //
        // colLotStock
        //
        this.colLotStock.Caption = "현재고";
        this.colLotStock.DisplayFormat.FormatString = "#,##0.####";
        this.colLotStock.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colLotStock.FieldName = "stock_qty";
        this.colLotStock.Name = "colLotStock";
        this.colLotStock.Visible = true;
        this.colLotStock.VisibleIndex = 6;
        this.colLotStock.Width = 90;
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblSearchDate);
        this.panHeader.Controls.Add(this.dteSearchFrom);
        this.panHeader.Controls.Add(this.lblTilde);
        this.panHeader.Controls.Add(this.dteSearchTo);
        this.panHeader.Controls.Add(this.lblSearchStat);
        this.panHeader.Controls.Add(this.cboSearchStat);
        this.panHeader.Controls.Add(this.lblSearchWoNo);
        this.panHeader.Controls.Add(this.txtSearchWoNo);
        this.panHeader.Controls.Add(this.lblSearchLotNo);
        this.panHeader.Controls.Add(this.txtSearchLotNo);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.Location = new System.Drawing.Point(5, 0);
        this.panHeader.Name = "panHeader";
        this.panHeader.Size = new System.Drawing.Size(1670, 49);
        this.panHeader.TabIndex = 0;
        //
        // lblSearchDate
        //
        this.lblSearchDate.Location = new System.Drawing.Point(25, 18);
        this.lblSearchDate.Name = "lblSearchDate";
        this.lblSearchDate.Size = new System.Drawing.Size(48, 15);
        this.lblSearchDate.TabIndex = 0;
        this.lblSearchDate.Text = "작업일자";
        //
        // dteSearchFrom
        //
        this.dteSearchFrom.Location = new System.Drawing.Point(90, 15);
        this.dteSearchFrom.Name = "dteSearchFrom";
        this.dteSearchFrom.Size = new System.Drawing.Size(110, 20);
        this.dteSearchFrom.TabIndex = 1;
        //
        // lblTilde
        //
        this.lblTilde.Location = new System.Drawing.Point(206, 18);
        this.lblTilde.Name = "lblTilde";
        this.lblTilde.Size = new System.Drawing.Size(7, 15);
        this.lblTilde.TabIndex = 2;
        this.lblTilde.Text = "~";
        //
        // dteSearchTo
        //
        this.dteSearchTo.Location = new System.Drawing.Point(219, 15);
        this.dteSearchTo.Name = "dteSearchTo";
        this.dteSearchTo.Size = new System.Drawing.Size(110, 20);
        this.dteSearchTo.TabIndex = 3;
        //
        // lblSearchStat
        //
        this.lblSearchStat.Location = new System.Drawing.Point(352, 18);
        this.lblSearchStat.Name = "lblSearchStat";
        this.lblSearchStat.Size = new System.Drawing.Size(24, 15);
        this.lblSearchStat.TabIndex = 4;
        this.lblSearchStat.Text = "상태";
        //
        // cboSearchStat
        //
        this.cboSearchStat.Location = new System.Drawing.Point(390, 15);
        this.cboSearchStat.LookupKey = "L_PR0001";
        this.cboSearchStat.Name = "cboSearchStat";
        this.cboSearchStat.Properties.NullText = "";
        this.cboSearchStat.Size = new System.Drawing.Size(110, 20);
        this.cboSearchStat.TabIndex = 5;
        //
        // lblSearchWoNo
        //
        this.lblSearchWoNo.Location = new System.Drawing.Point(524, 18);
        this.lblSearchWoNo.Name = "lblSearchWoNo";
        this.lblSearchWoNo.Size = new System.Drawing.Size(60, 15);
        this.lblSearchWoNo.TabIndex = 6;
        this.lblSearchWoNo.Text = "작업지시번호";
        //
        // txtSearchWoNo
        //
        this.txtSearchWoNo.Location = new System.Drawing.Point(610, 15);
        this.txtSearchWoNo.Name = "txtSearchWoNo";
        this.txtSearchWoNo.Size = new System.Drawing.Size(140, 20);
        this.txtSearchWoNo.TabIndex = 7;
        //
        // lblSearchLotNo
        //
        this.lblSearchLotNo.Location = new System.Drawing.Point(774, 18);
        this.lblSearchLotNo.Name = "lblSearchLotNo";
        this.lblSearchLotNo.Size = new System.Drawing.Size(48, 15);
        this.lblSearchLotNo.TabIndex = 8;
        this.lblSearchLotNo.Text = "시작 LOT";
        //
        // txtSearchLotNo
        //
        this.txtSearchLotNo.Location = new System.Drawing.Point(838, 15);
        this.txtSearchLotNo.Name = "txtSearchLotNo";
        this.txtSearchLotNo.Size = new System.Drawing.Size(140, 20);
        this.txtSearchLotNo.TabIndex = 9;
        //
        // frmWoStatus
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 800);
        this.Controls.Add(this.panBase);
        this.Name = "frmWoStatus";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).EndInit();
        this.panWork.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolStat2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.hyperlinkWoNo)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolStat)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        this.panHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchStat.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchWoNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchLotNo.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panWork;
    private GridControlWyn grd3;
    private GridViewWyn gvw3;
    private DevExpress.XtraGrid.Columns.GridColumn colLotSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colLotNo;
    private DevExpress.XtraGrid.Columns.GridColumn colLotItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colLotUnit;
    private DevExpress.XtraGrid.Columns.GridColumn colLotInit;
    private DevExpress.XtraGrid.Columns.GridColumn colLotWhNm;
    private DevExpress.XtraGrid.Columns.GridColumn colLotStock;
    private SectionHeaderWyn shLot;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private LookUpColumnEdit lookupcolStat2;
    private DevExpress.XtraGrid.Columns.GridColumn colPSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colPProcNm;
    private DevExpress.XtraGrid.Columns.GridColumn colPCustNm;
    private DevExpress.XtraGrid.Columns.GridColumn colPWhNm;
    private DevExpress.XtraGrid.Columns.GridColumn colPInItem;
    private DevExpress.XtraGrid.Columns.GridColumn colPOutItem;
    private DevExpress.XtraGrid.Columns.GridColumn colPDue;
    private DevExpress.XtraGrid.Columns.GridColumn colPStat;
    private DevExpress.XtraGrid.Columns.GridColumn colPIn;
    private DevExpress.XtraGrid.Columns.GridColumn colPGood;
    private DevExpress.XtraGrid.Columns.GridColumn colPBad;
    private DevExpress.XtraGrid.Columns.GridColumn colPYield;
    private SectionHeaderWyn shProc;
    private GridControlWyn grd1;
    private SplitterWyn splitterWyn1;
    private SplitterWyn splitterWyn2;
    private GridViewWyn gvw1;
    private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit hyperlinkWoNo;
    private LookUpColumnEdit lookupcolStat;
    private DevExpress.XtraGrid.Columns.GridColumn colWoNo;
    private DevExpress.XtraGrid.Columns.GridColumn colWoDate;
    private DevExpress.XtraGrid.Columns.GridColumn colRouteNm;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colStartLot;
    private DevExpress.XtraGrid.Columns.GridColumn colStartQty;
    private DevExpress.XtraGrid.Columns.GridColumn colSoNo;
    private DevExpress.XtraGrid.Columns.GridColumn colDelvDate;
    private DevExpress.XtraGrid.Columns.GridColumn colStatCd;
    private DevExpress.XtraGrid.Columns.GridColumn colCurProc;
    private DevExpress.XtraGrid.Columns.GridColumn colCurGood;
    private DevExpress.XtraGrid.Columns.GridColumn colCurUnit;
    private SectionHeaderWyn shList;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchDate;
    private DateEditWyn dteSearchFrom;
    private DevExpress.XtraEditors.LabelControl lblTilde;
    private DateEditWyn dteSearchTo;
    private DevExpress.XtraEditors.LabelControl lblSearchStat;
    private LookUpEditWyn cboSearchStat;
    private DevExpress.XtraEditors.LabelControl lblSearchWoNo;
    private TextEditWyn txtSearchWoNo;
    private DevExpress.XtraEditors.LabelControl lblSearchLotNo;
    private TextEditWyn txtSearchLotNo;
}
