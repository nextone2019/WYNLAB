// 공정실적현황(frmRsltStatus) - 공정실적 목록(grd1) + 선택한 실적의 산출 LOT(grd2) + 웨이퍼별 판정(grd3).
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

public partial class frmRsltStatus
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
        this.col3WSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col3WNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col3WGood = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col3WBad = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col3WGross = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col3WRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.shSub3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.splitterWyn2 = new WYNLAB.Base.Controls.SplitterWyn();
        this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
        this.col2OLot = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col2OItem = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col2OUnit = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col2OQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.shSub2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.lookupcol1_0 = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.hyperlink1 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
        this.col1RsltNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1RsltDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1WoNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1ProcNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1CustNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1InLot = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1InItem = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1InQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1GoodQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1BadQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1Yield = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1OutUnit = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1OutLots = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1Stat = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1CfmDt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1SrcFile = new DevExpress.XtraGrid.Columns.GridColumn();
        this.shList = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblSearchDate = new DevExpress.XtraEditors.LabelControl();
        this.dteSearchFrom = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblTilde = new DevExpress.XtraEditors.LabelControl();
        this.dteSearchTo = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblSearchStat = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchStat = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblSearchProc = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchProc = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblSearchRsltNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchRsltNo = new WYNLAB.Base.Controls.TextEditWyn();
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
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcol1_0)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.hyperlink1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchStat.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchProc.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchRsltNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchWoNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchLotNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
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
        this.panWork.Controls.Add(this.shSub3);
        this.panWork.Controls.Add(this.splitterWyn2);
        this.panWork.Controls.Add(this.grd2);
        this.panWork.Controls.Add(this.shSub2);
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
        this.shList.Text = "공정실적 목록 (실적번호/작업지시번호를 더블클릭하면 해당 화면이 열립니다)";
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Top;
        this.grd1.Location = new System.Drawing.Point(3, 27);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.hyperlink1,
        this.lookupcol1_0});
        this.grd1.Size = new System.Drawing.Size(1664, 260);
        this.grd1.TabIndex = 1;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // splitterWyn1
        //
        this.splitterWyn1.BackColor = System.Drawing.Color.White;
        this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Top;
        this.splitterWyn1.Location = new System.Drawing.Point(3, 287);
        this.splitterWyn1.Name = "splitterWyn1";
        this.splitterWyn1.Size = new System.Drawing.Size(1664, 10);
        this.splitterWyn1.TabIndex = 2;
        this.splitterWyn1.TabStop = false;
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.col1RsltNo,
        this.col1RsltDate,
        this.col1WoNo,
        this.col1ProcNm,
        this.col1CustNm,
        this.col1InLot,
        this.col1InItem,
        this.col1InQty,
        this.col1GoodQty,
        this.col1BadQty,
        this.col1Yield,
        this.col1OutUnit,
        this.col1OutLots,
        this.col1Stat,
        this.col1CfmDt,
        this.col1SrcFile});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsBehavior.Editable = false;
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        this.gvw1.OptionsView.ShowFooter = true;
        //
        // lookupcol1_0
        //
        this.lookupcol1_0.AutoHeight = false;
        this.lookupcol1_0.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcol1_0.LookupKey = "L_PR0006";
        this.lookupcol1_0.Name = "lookupcol1_0";
        this.lookupcol1_0.NullText = "";
        //
        // hyperlink1
        //
        this.hyperlink1.Name = "hyperlink1";
        //
        // col1RsltNo
        //
        this.col1RsltNo.Caption = "실적번호";
        this.col1RsltNo.ColumnEdit = this.hyperlink1;
        this.col1RsltNo.FieldName = "rslt_no";
        this.col1RsltNo.Name = "col1RsltNo";
        this.col1RsltNo.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "rslt_no", "합계 ({0:#,##0}건)")});
        this.col1RsltNo.Visible = true;
        this.col1RsltNo.VisibleIndex = 0;
        this.col1RsltNo.Width = 110;
        //
        // col1RsltDate
        //
        this.col1RsltDate.Caption = "실적일자";
        this.col1RsltDate.FieldName = "rslt_date";
        this.col1RsltDate.Name = "col1RsltDate";
        this.col1RsltDate.Visible = true;
        this.col1RsltDate.VisibleIndex = 1;
        this.col1RsltDate.Width = 80;
        //
        // col1WoNo
        //
        this.col1WoNo.Caption = "작업지시번호";
        this.col1WoNo.ColumnEdit = this.hyperlink1;
        this.col1WoNo.FieldName = "wo_no";
        this.col1WoNo.Name = "col1WoNo";
        this.col1WoNo.Visible = true;
        this.col1WoNo.VisibleIndex = 2;
        this.col1WoNo.Width = 110;
        //
        // col1ProcNm
        //
        this.col1ProcNm.Caption = "공정";
        this.col1ProcNm.FieldName = "proc_nm";
        this.col1ProcNm.Name = "col1ProcNm";
        this.col1ProcNm.Visible = true;
        this.col1ProcNm.VisibleIndex = 3;
        this.col1ProcNm.Width = 90;
        //
        // col1CustNm
        //
        this.col1CustNm.Caption = "외주처";
        this.col1CustNm.FieldName = "cust_nm";
        this.col1CustNm.Name = "col1CustNm";
        this.col1CustNm.Visible = true;
        this.col1CustNm.VisibleIndex = 4;
        this.col1CustNm.Width = 90;
        //
        // col1InLot
        //
        this.col1InLot.Caption = "투입 LOT";
        this.col1InLot.FieldName = "in_lot_no";
        this.col1InLot.Name = "col1InLot";
        this.col1InLot.Visible = true;
        this.col1InLot.VisibleIndex = 5;
        this.col1InLot.Width = 130;
        //
        // col1InItem
        //
        this.col1InItem.Caption = "투입품목";
        this.col1InItem.FieldName = "in_item_nm";
        this.col1InItem.Name = "col1InItem";
        this.col1InItem.Visible = true;
        this.col1InItem.VisibleIndex = 6;
        this.col1InItem.Width = 100;
        //
        // col1InQty
        //
        this.col1InQty.Caption = "투입수량";
        this.col1InQty.DisplayFormat.FormatString = "#,##0.####";
        this.col1InQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col1InQty.FieldName = "in_qty";
        this.col1InQty.Name = "col1InQty";
        this.col1InQty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "in_qty", "{0:#,##0.####}")});
        this.col1InQty.Visible = true;
        this.col1InQty.VisibleIndex = 7;
        this.col1InQty.Width = 80;
        //
        // col1GoodQty
        //
        this.col1GoodQty.Caption = "양품";
        this.col1GoodQty.DisplayFormat.FormatString = "#,##0.####";
        this.col1GoodQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col1GoodQty.FieldName = "good_qty";
        this.col1GoodQty.Name = "col1GoodQty";
        this.col1GoodQty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "good_qty", "{0:#,##0.####}")});
        this.col1GoodQty.Visible = true;
        this.col1GoodQty.VisibleIndex = 8;
        this.col1GoodQty.Width = 80;
        //
        // col1BadQty
        //
        this.col1BadQty.Caption = "불량";
        this.col1BadQty.DisplayFormat.FormatString = "#,##0.####";
        this.col1BadQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col1BadQty.FieldName = "bad_qty";
        this.col1BadQty.Name = "col1BadQty";
        this.col1BadQty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "bad_qty", "{0:#,##0.####}")});
        this.col1BadQty.Visible = true;
        this.col1BadQty.VisibleIndex = 9;
        this.col1BadQty.Width = 70;
        //
        // col1Yield
        //
        this.col1Yield.Caption = "수율(%)";
        this.col1Yield.DisplayFormat.FormatString = "#,##0.####";
        this.col1Yield.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col1Yield.FieldName = "yield_rate";
        this.col1Yield.Name = "col1Yield";
        this.col1Yield.Visible = true;
        this.col1Yield.VisibleIndex = 10;
        this.col1Yield.Width = 65;
        //
        // col1OutUnit
        //
        this.col1OutUnit.Caption = "산출단위";
        this.col1OutUnit.FieldName = "out_unit_cd";
        this.col1OutUnit.Name = "col1OutUnit";
        this.col1OutUnit.Visible = true;
        this.col1OutUnit.VisibleIndex = 11;
        this.col1OutUnit.Width = 60;
        //
        // col1OutLots
        //
        this.col1OutLots.Caption = "산출 LOT";
        this.col1OutLots.FieldName = "out_lots";
        this.col1OutLots.Name = "col1OutLots";
        this.col1OutLots.Visible = true;
        this.col1OutLots.VisibleIndex = 12;
        this.col1OutLots.Width = 220;
        //
        // col1Stat
        //
        this.col1Stat.Caption = "상태";
        this.col1Stat.ColumnEdit = this.lookupcol1_0;
        this.col1Stat.FieldName = "stat_cd";
        this.col1Stat.Name = "col1Stat";
        this.col1Stat.Visible = true;
        this.col1Stat.VisibleIndex = 13;
        this.col1Stat.Width = 60;
        //
        // col1CfmDt
        //
        this.col1CfmDt.Caption = "확정일시";
        this.col1CfmDt.FieldName = "cfm_dt";
        this.col1CfmDt.Name = "col1CfmDt";
        this.col1CfmDt.Visible = true;
        this.col1CfmDt.VisibleIndex = 14;
        this.col1CfmDt.Width = 130;
        //
        // col1SrcFile
        //
        this.col1SrcFile.Caption = "원본파일";
        this.col1SrcFile.FieldName = "src_file_nm";
        this.col1SrcFile.Name = "col1SrcFile";
        this.col1SrcFile.Visible = true;
        this.col1SrcFile.VisibleIndex = 15;
        this.col1SrcFile.Width = 160;
        //
        // shSub2
        //
        this.shSub2.BackColor = System.Drawing.Color.White;
        this.shSub2.Dock = System.Windows.Forms.DockStyle.Top;
        this.shSub2.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shSub2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.shSub2.Location = new System.Drawing.Point(3, 297);
        this.shSub2.Name = "shSub2";
        this.shSub2.Size = new System.Drawing.Size(1664, 27);
        this.shSub2.TabIndex = 3;
        this.shSub2.Text = "산출 LOT (이 실적으로 만들어진 LOT)";
        //
        // grd2
        //
        this.grd2.Dock = System.Windows.Forms.DockStyle.Top;
        this.grd2.Location = new System.Drawing.Point(3, 324);
        this.grd2.MainView = this.gvw2;
        this.grd2.Name = "grd2";
        this.grd2.Size = new System.Drawing.Size(1664, 170);
        this.grd2.TabIndex = 4;
        this.grd2.UseEmbeddedNavigator = false;
        this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw2});
        //
        // splitterWyn2
        //
        this.splitterWyn2.BackColor = System.Drawing.Color.White;
        this.splitterWyn2.Dock = System.Windows.Forms.DockStyle.Top;
        this.splitterWyn2.Location = new System.Drawing.Point(3, 494);
        this.splitterWyn2.Name = "splitterWyn2";
        this.splitterWyn2.Size = new System.Drawing.Size(1664, 10);
        this.splitterWyn2.TabIndex = 5;
        this.splitterWyn2.TabStop = false;
        //
        // gvw2
        //
        this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.col2OLot,
        this.col2OItem,
        this.col2OUnit,
        this.col2OQty});
        this.gvw2.GridControl = this.grd2;
        this.gvw2.HighlightFocusedRow = true;
        this.gvw2.Name = "gvw2";
        this.gvw2.OptionsBehavior.Editable = false;
        this.gvw2.OptionsView.ColumnAutoWidth = false;
        this.gvw2.OptionsView.ShowGroupPanel = false;
        this.gvw2.OptionsView.ShowFooter = true;
        //
        // col2OLot
        //
        this.col2OLot.Caption = "산출 LOT";
        this.col2OLot.FieldName = "lot_no";
        this.col2OLot.Name = "col2OLot";
        this.col2OLot.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "lot_no", "합계 ({0:#,##0}건)")});
        this.col2OLot.Visible = true;
        this.col2OLot.VisibleIndex = 0;
        this.col2OLot.Width = 160;
        //
        // col2OItem
        //
        this.col2OItem.Caption = "품명";
        this.col2OItem.FieldName = "item_nm";
        this.col2OItem.Name = "col2OItem";
        this.col2OItem.Visible = true;
        this.col2OItem.VisibleIndex = 1;
        this.col2OItem.Width = 140;
        //
        // col2OUnit
        //
        this.col2OUnit.Caption = "단위";
        this.col2OUnit.FieldName = "unit_cd";
        this.col2OUnit.Name = "col2OUnit";
        this.col2OUnit.Visible = true;
        this.col2OUnit.VisibleIndex = 2;
        this.col2OUnit.Width = 50;
        //
        // col2OQty
        //
        this.col2OQty.Caption = "수량";
        this.col2OQty.DisplayFormat.FormatString = "#,##0.####";
        this.col2OQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col2OQty.FieldName = "qty";
        this.col2OQty.Name = "col2OQty";
        this.col2OQty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "qty", "{0:#,##0.####}")});
        this.col2OQty.Visible = true;
        this.col2OQty.VisibleIndex = 3;
        this.col2OQty.Width = 90;
        //
        // shSub3
        //
        this.shSub3.BackColor = System.Drawing.Color.White;
        this.shSub3.Dock = System.Windows.Forms.DockStyle.Top;
        this.shSub3.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shSub3.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.shSub3.Location = new System.Drawing.Point(3, 504);
        this.shSub3.Name = "shSub3";
        this.shSub3.Size = new System.Drawing.Size(1664, 27);
        this.shSub3.TabIndex = 6;
        this.shSub3.Text = "웨이퍼별 합/부 판정 (엑셀 불러오기/직접 입력한 경우)";
        //
        // grd3
        //
        this.grd3.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd3.Location = new System.Drawing.Point(3, 531);
        this.grd3.MainView = this.gvw3;
        this.grd3.Name = "grd3";
        this.grd3.Size = new System.Drawing.Size(1664, 215);
        this.grd3.TabIndex = 7;
        this.grd3.UseEmbeddedNavigator = false;
        this.grd3.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw3});
        //
        // gvw3
        //
        this.gvw3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.col3WSerl,
        this.col3WNo,
        this.col3WGood,
        this.col3WBad,
        this.col3WGross,
        this.col3WRemark});
        this.gvw3.GridControl = this.grd3;
        this.gvw3.HighlightFocusedRow = true;
        this.gvw3.Name = "gvw3";
        this.gvw3.OptionsBehavior.Editable = false;
        this.gvw3.OptionsView.ColumnAutoWidth = false;
        this.gvw3.OptionsView.ShowGroupPanel = false;
        this.gvw3.OptionsView.ShowFooter = true;
        //
        // col3WSerl
        //
        this.col3WSerl.Caption = "순번";
        this.col3WSerl.FieldName = "serl";
        this.col3WSerl.Name = "col3WSerl";
        this.col3WSerl.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "serl", "합계 ({0:#,##0}건)")});
        this.col3WSerl.Visible = true;
        this.col3WSerl.VisibleIndex = 0;
        this.col3WSerl.Width = 50;
        //
        // col3WNo
        //
        this.col3WNo.Caption = "웨이퍼 번호";
        this.col3WNo.FieldName = "wafer_no";
        this.col3WNo.Name = "col3WNo";
        this.col3WNo.Visible = true;
        this.col3WNo.VisibleIndex = 1;
        this.col3WNo.Width = 100;
        //
        // col3WGood
        //
        this.col3WGood.Caption = "Good Die";
        this.col3WGood.DisplayFormat.FormatString = "#,##0.####";
        this.col3WGood.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col3WGood.FieldName = "good_qty";
        this.col3WGood.Name = "col3WGood";
        this.col3WGood.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "good_qty", "{0:#,##0.####}")});
        this.col3WGood.Visible = true;
        this.col3WGood.VisibleIndex = 2;
        this.col3WGood.Width = 90;
        //
        // col3WBad
        //
        this.col3WBad.Caption = "Bad Die";
        this.col3WBad.DisplayFormat.FormatString = "#,##0.####";
        this.col3WBad.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col3WBad.FieldName = "bad_qty";
        this.col3WBad.Name = "col3WBad";
        this.col3WBad.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "bad_qty", "{0:#,##0.####}")});
        this.col3WBad.Visible = true;
        this.col3WBad.VisibleIndex = 3;
        this.col3WBad.Width = 90;
        //
        // col3WGross
        //
        this.col3WGross.Caption = "Gross Die";
        this.col3WGross.DisplayFormat.FormatString = "#,##0.####";
        this.col3WGross.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col3WGross.FieldName = "gross_qty";
        this.col3WGross.Name = "col3WGross";
        this.col3WGross.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "gross_qty", "{0:#,##0.####}")});
        this.col3WGross.Visible = true;
        this.col3WGross.VisibleIndex = 4;
        this.col3WGross.Width = 90;
        //
        // col3WRemark
        //
        this.col3WRemark.Caption = "비고";
        this.col3WRemark.FieldName = "remark";
        this.col3WRemark.Name = "col3WRemark";
        this.col3WRemark.Visible = true;
        this.col3WRemark.VisibleIndex = 5;
        this.col3WRemark.Width = 200;
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblSearchAccId);
        this.panHeader.Controls.Add(this.cboSearchAccId);
        this.panHeader.Controls.Add(this.lblSearchDate);
        this.panHeader.Controls.Add(this.dteSearchFrom);
        this.panHeader.Controls.Add(this.lblTilde);
        this.panHeader.Controls.Add(this.dteSearchTo);
        this.panHeader.Controls.Add(this.lblSearchStat);
        this.panHeader.Controls.Add(this.cboSearchStat);
        this.panHeader.Controls.Add(this.lblSearchProc);
        this.panHeader.Controls.Add(this.cboSearchProc);
        this.panHeader.Controls.Add(this.lblSearchRsltNo);
        this.panHeader.Controls.Add(this.txtSearchRsltNo);
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
        // lblSearchDate
        //
        this.lblSearchDate.Location = new System.Drawing.Point(224, 18);
        this.lblSearchDate.Name = "lblSearchDate";
        this.lblSearchDate.Size = new System.Drawing.Size(48, 15);
        this.lblSearchDate.TabIndex = 0;
        this.lblSearchDate.Text = "실적일자";
        //
        // dteSearchFrom
        //
        this.dteSearchFrom.Location = new System.Drawing.Point(289, 15);
        this.dteSearchFrom.Name = "dteSearchFrom";
        this.dteSearchFrom.Size = new System.Drawing.Size(110, 20);
        this.dteSearchFrom.TabIndex = 1;
        //
        // lblTilde
        //
        this.lblTilde.Location = new System.Drawing.Point(405, 18);
        this.lblTilde.Name = "lblTilde";
        this.lblTilde.Size = new System.Drawing.Size(7, 15);
        this.lblTilde.TabIndex = 2;
        this.lblTilde.Text = "~";
        //
        // dteSearchTo
        //
        this.dteSearchTo.Location = new System.Drawing.Point(418, 15);
        this.dteSearchTo.Name = "dteSearchTo";
        this.dteSearchTo.Size = new System.Drawing.Size(110, 20);
        this.dteSearchTo.TabIndex = 3;
        //
        // lblSearchStat
        //
        this.lblSearchStat.Location = new System.Drawing.Point(551, 18);
        this.lblSearchStat.Name = "lblSearchStat";
        this.lblSearchStat.Size = new System.Drawing.Size(26, 15);
        this.lblSearchStat.TabIndex = 4;
        this.lblSearchStat.Text = "상태";
        //
        // cboSearchStat
        //
        this.cboSearchStat.Location = new System.Drawing.Point(591, 15);
        this.cboSearchStat.LookupKey = "L_PR0006";
        this.cboSearchStat.Name = "cboSearchStat";
        this.cboSearchStat.Properties.NullText = "";
        this.cboSearchStat.Size = new System.Drawing.Size(110, 20);
        this.cboSearchStat.TabIndex = 5;
        //
        // lblSearchProc
        //
        this.lblSearchProc.Location = new System.Drawing.Point(725, 18);
        this.lblSearchProc.Name = "lblSearchProc";
        this.lblSearchProc.Size = new System.Drawing.Size(26, 15);
        this.lblSearchProc.TabIndex = 6;
        this.lblSearchProc.Text = "공정";
        //
        // cboSearchProc
        //
        this.cboSearchProc.Location = new System.Drawing.Point(765, 15);
        this.cboSearchProc.LookupKey = "L_PRPROC";
        this.cboSearchProc.Name = "cboSearchProc";
        this.cboSearchProc.Properties.NullText = "";
        this.cboSearchProc.Size = new System.Drawing.Size(120, 20);
        this.cboSearchProc.TabIndex = 7;
        //
        // lblSearchRsltNo
        //
        this.lblSearchRsltNo.Location = new System.Drawing.Point(909, 18);
        this.lblSearchRsltNo.Name = "lblSearchRsltNo";
        this.lblSearchRsltNo.Size = new System.Drawing.Size(52, 15);
        this.lblSearchRsltNo.TabIndex = 8;
        this.lblSearchRsltNo.Text = "실적번호";
        //
        // txtSearchRsltNo
        //
        this.txtSearchRsltNo.Location = new System.Drawing.Point(975, 15);
        this.txtSearchRsltNo.Name = "txtSearchRsltNo";
        this.txtSearchRsltNo.Size = new System.Drawing.Size(130, 20);
        this.txtSearchRsltNo.TabIndex = 9;
        //
        // lblSearchWoNo
        //
        this.lblSearchWoNo.Location = new System.Drawing.Point(1129, 18);
        this.lblSearchWoNo.Name = "lblSearchWoNo";
        this.lblSearchWoNo.Size = new System.Drawing.Size(78, 15);
        this.lblSearchWoNo.TabIndex = 10;
        this.lblSearchWoNo.Text = "작업지시번호";
        //
        // txtSearchWoNo
        //
        this.txtSearchWoNo.Location = new System.Drawing.Point(1221, 15);
        this.txtSearchWoNo.Name = "txtSearchWoNo";
        this.txtSearchWoNo.Size = new System.Drawing.Size(130, 20);
        this.txtSearchWoNo.TabIndex = 11;
        //
        // lblSearchLotNo
        //
        this.lblSearchLotNo.Location = new System.Drawing.Point(1375, 18);
        this.lblSearchLotNo.Name = "lblSearchLotNo";
        this.lblSearchLotNo.Size = new System.Drawing.Size(24, 15);
        this.lblSearchLotNo.TabIndex = 12;
        this.lblSearchLotNo.Text = "LOT";
        //
        // txtSearchLotNo
        //
        this.txtSearchLotNo.Location = new System.Drawing.Point(1413, 15);
        this.txtSearchLotNo.Name = "txtSearchLotNo";
        this.txtSearchLotNo.Size = new System.Drawing.Size(130, 20);
        this.txtSearchLotNo.TabIndex = 13;
        //
        // frmRsltStatus
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 800);
        this.Controls.Add(this.panBase);
        this.Name = "frmRsltStatus";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).EndInit();
        this.panWork.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcol1_0)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.hyperlink1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        this.panHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchStat.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchProc.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchRsltNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchWoNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchLotNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panWork;
    private GridControlWyn grd3;
    private GridViewWyn gvw3;
    private DevExpress.XtraGrid.Columns.GridColumn col3WSerl;
    private DevExpress.XtraGrid.Columns.GridColumn col3WNo;
    private DevExpress.XtraGrid.Columns.GridColumn col3WGood;
    private DevExpress.XtraGrid.Columns.GridColumn col3WBad;
    private DevExpress.XtraGrid.Columns.GridColumn col3WGross;
    private DevExpress.XtraGrid.Columns.GridColumn col3WRemark;
    private SectionHeaderWyn shSub3;
    private SplitterWyn splitterWyn2;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private DevExpress.XtraGrid.Columns.GridColumn col2OLot;
    private DevExpress.XtraGrid.Columns.GridColumn col2OItem;
    private DevExpress.XtraGrid.Columns.GridColumn col2OUnit;
    private DevExpress.XtraGrid.Columns.GridColumn col2OQty;
    private SectionHeaderWyn shSub2;
    private SplitterWyn splitterWyn1;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private LookUpColumnEdit lookupcol1_0;
    private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit hyperlink1;
    private DevExpress.XtraGrid.Columns.GridColumn col1RsltNo;
    private DevExpress.XtraGrid.Columns.GridColumn col1RsltDate;
    private DevExpress.XtraGrid.Columns.GridColumn col1WoNo;
    private DevExpress.XtraGrid.Columns.GridColumn col1ProcNm;
    private DevExpress.XtraGrid.Columns.GridColumn col1CustNm;
    private DevExpress.XtraGrid.Columns.GridColumn col1InLot;
    private DevExpress.XtraGrid.Columns.GridColumn col1InItem;
    private DevExpress.XtraGrid.Columns.GridColumn col1InQty;
    private DevExpress.XtraGrid.Columns.GridColumn col1GoodQty;
    private DevExpress.XtraGrid.Columns.GridColumn col1BadQty;
    private DevExpress.XtraGrid.Columns.GridColumn col1Yield;
    private DevExpress.XtraGrid.Columns.GridColumn col1OutUnit;
    private DevExpress.XtraGrid.Columns.GridColumn col1OutLots;
    private DevExpress.XtraGrid.Columns.GridColumn col1Stat;
    private DevExpress.XtraGrid.Columns.GridColumn col1CfmDt;
    private DevExpress.XtraGrid.Columns.GridColumn col1SrcFile;
    private SectionHeaderWyn shList;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchDate;
    private DateEditWyn dteSearchFrom;
    private DevExpress.XtraEditors.LabelControl lblTilde;
    private DateEditWyn dteSearchTo;
    private DevExpress.XtraEditors.LabelControl lblSearchStat;
    private LookUpEditWyn cboSearchStat;
    private DevExpress.XtraEditors.LabelControl lblSearchProc;
    private LookUpEditWyn cboSearchProc;
    private DevExpress.XtraEditors.LabelControl lblSearchRsltNo;
    private TextEditWyn txtSearchRsltNo;
    private DevExpress.XtraEditors.LabelControl lblSearchWoNo;
    private TextEditWyn txtSearchWoNo;
    private DevExpress.XtraEditors.LabelControl lblSearchLotNo;
    private TextEditWyn txtSearchLotNo;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private WYNLAB.Base.Controls.LookUpEditWyn cboSearchAccId;
}
