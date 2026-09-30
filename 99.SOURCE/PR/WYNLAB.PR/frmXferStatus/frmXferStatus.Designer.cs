// 외주이전현황(frmXferStatus) - 외주이전 목록(grd1) + 선택한 이전의 LOT별 출발/도착/차이(grd2).
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

public partial class frmXferStatus
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
        this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
        this.lookupcol2_0 = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.lookupcol2_1 = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.col2Lot = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col2Item = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col2Unit = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col2OutQ = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col2InQ = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col2DiffQ = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col2Resp = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col2Act = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col2DiffDt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col2DiffRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.shSub2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.lookupcol1_0 = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.hyperlink1 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
        this.col1XferNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1XferDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1WoNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1FromProc = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1FromCust = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1ToProc = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1ToCust = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1LotCnt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1OutQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1InQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1DiffQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1Stat = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1OutDt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1InDt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.shList = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.lblSearchDate = new DevExpress.XtraEditors.LabelControl();
        this.dteSearchFrom = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblTilde = new DevExpress.XtraEditors.LabelControl();
        this.dteSearchTo = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblSearchStat = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchStat = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblSearchXferNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchXferNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSearchWoNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchWoNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSearchLotNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchLotNo = new WYNLAB.Base.Controls.TextEditWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        this.panBase.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).BeginInit();
        this.panWork.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcol2_0)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcol2_1)).BeginInit();
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
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchXferNo.Properties)).BeginInit();
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
        this.shList.Text = "외주이전 목록 (이전번호/작업지시번호를 더블클릭하면 해당 화면이 열립니다)";
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
        this.col1XferNo,
        this.col1XferDate,
        this.col1WoNo,
        this.col1FromProc,
        this.col1FromCust,
        this.col1ToProc,
        this.col1ToCust,
        this.col1LotCnt,
        this.col1OutQty,
        this.col1InQty,
        this.col1DiffQty,
        this.col1Stat,
        this.col1OutDt,
        this.col1InDt});
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
        this.lookupcol1_0.LookupKey = "L_PR0003";
        this.lookupcol1_0.Name = "lookupcol1_0";
        this.lookupcol1_0.NullText = "";
        //
        // hyperlink1
        //
        this.hyperlink1.Name = "hyperlink1";
        //
        // col1XferNo
        //
        this.col1XferNo.Caption = "이전번호";
        this.col1XferNo.ColumnEdit = this.hyperlink1;
        this.col1XferNo.FieldName = "xfer_no";
        this.col1XferNo.Name = "col1XferNo";
        this.col1XferNo.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "xfer_no", "합계 ({0:#,##0}건)")});
        this.col1XferNo.Visible = true;
        this.col1XferNo.VisibleIndex = 0;
        this.col1XferNo.Width = 110;
        //
        // col1XferDate
        //
        this.col1XferDate.Caption = "이전일자";
        this.col1XferDate.FieldName = "xfer_date";
        this.col1XferDate.Name = "col1XferDate";
        this.col1XferDate.Visible = true;
        this.col1XferDate.VisibleIndex = 1;
        this.col1XferDate.Width = 80;
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
        // col1FromProc
        //
        this.col1FromProc.Caption = "출발 공정";
        this.col1FromProc.FieldName = "from_proc_nm";
        this.col1FromProc.Name = "col1FromProc";
        this.col1FromProc.Visible = true;
        this.col1FromProc.VisibleIndex = 3;
        this.col1FromProc.Width = 90;
        //
        // col1FromCust
        //
        this.col1FromCust.Caption = "출발 외주처";
        this.col1FromCust.FieldName = "from_cust_nm";
        this.col1FromCust.Name = "col1FromCust";
        this.col1FromCust.Visible = true;
        this.col1FromCust.VisibleIndex = 4;
        this.col1FromCust.Width = 90;
        //
        // col1ToProc
        //
        this.col1ToProc.Caption = "도착 공정";
        this.col1ToProc.FieldName = "to_proc_nm";
        this.col1ToProc.Name = "col1ToProc";
        this.col1ToProc.Visible = true;
        this.col1ToProc.VisibleIndex = 5;
        this.col1ToProc.Width = 90;
        //
        // col1ToCust
        //
        this.col1ToCust.Caption = "도착 외주처";
        this.col1ToCust.FieldName = "to_cust_nm";
        this.col1ToCust.Name = "col1ToCust";
        this.col1ToCust.Visible = true;
        this.col1ToCust.VisibleIndex = 6;
        this.col1ToCust.Width = 90;
        //
        // col1LotCnt
        //
        this.col1LotCnt.Caption = "LOT 수";
        this.col1LotCnt.DisplayFormat.FormatString = "#,##0.####";
        this.col1LotCnt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col1LotCnt.FieldName = "lot_cnt";
        this.col1LotCnt.Name = "col1LotCnt";
        this.col1LotCnt.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "lot_cnt", "{0:#,##0.####}")});
        this.col1LotCnt.Visible = true;
        this.col1LotCnt.VisibleIndex = 7;
        this.col1LotCnt.Width = 55;
        //
        // col1OutQty
        //
        this.col1OutQty.Caption = "출발수량";
        this.col1OutQty.DisplayFormat.FormatString = "#,##0.####";
        this.col1OutQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col1OutQty.FieldName = "out_qty";
        this.col1OutQty.Name = "col1OutQty";
        this.col1OutQty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "out_qty", "{0:#,##0.####}")});
        this.col1OutQty.Visible = true;
        this.col1OutQty.VisibleIndex = 8;
        this.col1OutQty.Width = 85;
        //
        // col1InQty
        //
        this.col1InQty.Caption = "도착수량";
        this.col1InQty.DisplayFormat.FormatString = "#,##0.####";
        this.col1InQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col1InQty.FieldName = "in_qty";
        this.col1InQty.Name = "col1InQty";
        this.col1InQty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "in_qty", "{0:#,##0.####}")});
        this.col1InQty.Visible = true;
        this.col1InQty.VisibleIndex = 9;
        this.col1InQty.Width = 85;
        //
        // col1DiffQty
        //
        this.col1DiffQty.Caption = "차이";
        this.col1DiffQty.DisplayFormat.FormatString = "#,##0.####";
        this.col1DiffQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col1DiffQty.FieldName = "diff_qty";
        this.col1DiffQty.Name = "col1DiffQty";
        this.col1DiffQty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "diff_qty", "{0:#,##0.####}")});
        this.col1DiffQty.Visible = true;
        this.col1DiffQty.VisibleIndex = 10;
        this.col1DiffQty.Width = 70;
        //
        // col1Stat
        //
        this.col1Stat.Caption = "상태";
        this.col1Stat.ColumnEdit = this.lookupcol1_0;
        this.col1Stat.FieldName = "stat_cd";
        this.col1Stat.Name = "col1Stat";
        this.col1Stat.Visible = true;
        this.col1Stat.VisibleIndex = 11;
        this.col1Stat.Width = 75;
        //
        // col1OutDt
        //
        this.col1OutDt.Caption = "출발일시";
        this.col1OutDt.FieldName = "out_dt";
        this.col1OutDt.Name = "col1OutDt";
        this.col1OutDt.Visible = true;
        this.col1OutDt.VisibleIndex = 12;
        this.col1OutDt.Width = 130;
        //
        // col1InDt
        //
        this.col1InDt.Caption = "도착일시";
        this.col1InDt.FieldName = "in_dt";
        this.col1InDt.Name = "col1InDt";
        this.col1InDt.Visible = true;
        this.col1InDt.VisibleIndex = 13;
        this.col1InDt.Width = 130;
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
        this.shSub2.Text = "이전 LOT 내역 (출발/도착/차이 처리)";
        //
        // grd2
        //
        this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd2.Location = new System.Drawing.Point(3, 324);
        this.grd2.MainView = this.gvw2;
        this.grd2.Name = "grd2";
        this.grd2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.lookupcol2_0,
        this.lookupcol2_1});
        this.grd2.Size = new System.Drawing.Size(1664, 422);
        this.grd2.TabIndex = 4;
        this.grd2.UseEmbeddedNavigator = false;
        this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw2});
        //
        // gvw2
        //
        this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.col2Lot,
        this.col2Item,
        this.col2Unit,
        this.col2OutQ,
        this.col2InQ,
        this.col2DiffQ,
        this.col2Resp,
        this.col2Act,
        this.col2DiffDt,
        this.col2DiffRemark});
        this.gvw2.GridControl = this.grd2;
        this.gvw2.HighlightFocusedRow = true;
        this.gvw2.Name = "gvw2";
        this.gvw2.OptionsBehavior.Editable = false;
        this.gvw2.OptionsView.ColumnAutoWidth = false;
        this.gvw2.OptionsView.ShowGroupPanel = false;
        this.gvw2.OptionsView.ShowFooter = true;
        //
        // lookupcol2_0
        //
        this.lookupcol2_0.AutoHeight = false;
        this.lookupcol2_0.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcol2_0.LookupKey = "L_PR0005";
        this.lookupcol2_0.Name = "lookupcol2_0";
        this.lookupcol2_0.NullText = "";
        //
        // lookupcol2_1
        //
        this.lookupcol2_1.AutoHeight = false;
        this.lookupcol2_1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcol2_1.LookupKey = "L_PR0007";
        this.lookupcol2_1.Name = "lookupcol2_1";
        this.lookupcol2_1.NullText = "";
        //
        // col2Lot
        //
        this.col2Lot.Caption = "LOT";
        this.col2Lot.FieldName = "lot_no";
        this.col2Lot.Name = "col2Lot";
        this.col2Lot.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "lot_no", "합계 ({0:#,##0}건)")});
        this.col2Lot.Visible = true;
        this.col2Lot.VisibleIndex = 0;
        this.col2Lot.Width = 160;
        //
        // col2Item
        //
        this.col2Item.Caption = "품명";
        this.col2Item.FieldName = "item_nm";
        this.col2Item.Name = "col2Item";
        this.col2Item.Visible = true;
        this.col2Item.VisibleIndex = 1;
        this.col2Item.Width = 130;
        //
        // col2Unit
        //
        this.col2Unit.Caption = "단위";
        this.col2Unit.FieldName = "unit_cd";
        this.col2Unit.Name = "col2Unit";
        this.col2Unit.Visible = true;
        this.col2Unit.VisibleIndex = 2;
        this.col2Unit.Width = 50;
        //
        // col2OutQ
        //
        this.col2OutQ.Caption = "출발수량";
        this.col2OutQ.DisplayFormat.FormatString = "#,##0.####";
        this.col2OutQ.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col2OutQ.FieldName = "out_qty";
        this.col2OutQ.Name = "col2OutQ";
        this.col2OutQ.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "out_qty", "{0:#,##0.####}")});
        this.col2OutQ.Visible = true;
        this.col2OutQ.VisibleIndex = 3;
        this.col2OutQ.Width = 85;
        //
        // col2InQ
        //
        this.col2InQ.Caption = "도착수량";
        this.col2InQ.DisplayFormat.FormatString = "#,##0.####";
        this.col2InQ.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col2InQ.FieldName = "in_qty";
        this.col2InQ.Name = "col2InQ";
        this.col2InQ.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "in_qty", "{0:#,##0.####}")});
        this.col2InQ.Visible = true;
        this.col2InQ.VisibleIndex = 4;
        this.col2InQ.Width = 85;
        //
        // col2DiffQ
        //
        this.col2DiffQ.Caption = "차이";
        this.col2DiffQ.DisplayFormat.FormatString = "#,##0.####";
        this.col2DiffQ.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col2DiffQ.FieldName = "diff_qty";
        this.col2DiffQ.Name = "col2DiffQ";
        this.col2DiffQ.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "diff_qty", "{0:#,##0.####}")});
        this.col2DiffQ.Visible = true;
        this.col2DiffQ.VisibleIndex = 5;
        this.col2DiffQ.Width = 70;
        //
        // col2Resp
        //
        this.col2Resp.Caption = "차이 귀책";
        this.col2Resp.ColumnEdit = this.lookupcol2_0;
        this.col2Resp.FieldName = "diff_resp_cd";
        this.col2Resp.Name = "col2Resp";
        this.col2Resp.Visible = true;
        this.col2Resp.VisibleIndex = 6;
        this.col2Resp.Width = 90;
        //
        // col2Act
        //
        this.col2Act.Caption = "차이 처리";
        this.col2Act.ColumnEdit = this.lookupcol2_1;
        this.col2Act.FieldName = "diff_act_cd";
        this.col2Act.Name = "col2Act";
        this.col2Act.Visible = true;
        this.col2Act.VisibleIndex = 7;
        this.col2Act.Width = 90;
        //
        // col2DiffDt
        //
        this.col2DiffDt.Caption = "처리일시";
        this.col2DiffDt.FieldName = "diff_dt";
        this.col2DiffDt.Name = "col2DiffDt";
        this.col2DiffDt.Visible = true;
        this.col2DiffDt.VisibleIndex = 8;
        this.col2DiffDt.Width = 130;
        //
        // col2DiffRemark
        //
        this.col2DiffRemark.Caption = "처리 메모";
        this.col2DiffRemark.FieldName = "diff_remark";
        this.col2DiffRemark.Name = "col2DiffRemark";
        this.col2DiffRemark.Visible = true;
        this.col2DiffRemark.VisibleIndex = 9;
        this.col2DiffRemark.Width = 200;
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
        this.panHeader.Controls.Add(this.lblSearchXferNo);
        this.panHeader.Controls.Add(this.txtSearchXferNo);
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
        this.lblSearchDate.Text = "이전일자";
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
        this.lblSearchStat.Size = new System.Drawing.Size(26, 15);
        this.lblSearchStat.TabIndex = 4;
        this.lblSearchStat.Text = "상태";
        //
        // cboSearchStat
        //
        this.cboSearchStat.Location = new System.Drawing.Point(392, 15);
        this.cboSearchStat.LookupKey = "L_PR0003";
        this.cboSearchStat.Name = "cboSearchStat";
        this.cboSearchStat.Properties.NullText = "";
        this.cboSearchStat.Size = new System.Drawing.Size(110, 20);
        this.cboSearchStat.TabIndex = 5;
        //
        // lblSearchXferNo
        //
        this.lblSearchXferNo.Location = new System.Drawing.Point(526, 18);
        this.lblSearchXferNo.Name = "lblSearchXferNo";
        this.lblSearchXferNo.Size = new System.Drawing.Size(52, 15);
        this.lblSearchXferNo.TabIndex = 6;
        this.lblSearchXferNo.Text = "이전번호";
        //
        // txtSearchXferNo
        //
        this.txtSearchXferNo.Location = new System.Drawing.Point(592, 15);
        this.txtSearchXferNo.Name = "txtSearchXferNo";
        this.txtSearchXferNo.Size = new System.Drawing.Size(130, 20);
        this.txtSearchXferNo.TabIndex = 7;
        //
        // lblSearchWoNo
        //
        this.lblSearchWoNo.Location = new System.Drawing.Point(746, 18);
        this.lblSearchWoNo.Name = "lblSearchWoNo";
        this.lblSearchWoNo.Size = new System.Drawing.Size(78, 15);
        this.lblSearchWoNo.TabIndex = 8;
        this.lblSearchWoNo.Text = "작업지시번호";
        //
        // txtSearchWoNo
        //
        this.txtSearchWoNo.Location = new System.Drawing.Point(838, 15);
        this.txtSearchWoNo.Name = "txtSearchWoNo";
        this.txtSearchWoNo.Size = new System.Drawing.Size(130, 20);
        this.txtSearchWoNo.TabIndex = 9;
        //
        // lblSearchLotNo
        //
        this.lblSearchLotNo.Location = new System.Drawing.Point(992, 18);
        this.lblSearchLotNo.Name = "lblSearchLotNo";
        this.lblSearchLotNo.Size = new System.Drawing.Size(24, 15);
        this.lblSearchLotNo.TabIndex = 10;
        this.lblSearchLotNo.Text = "LOT";
        //
        // txtSearchLotNo
        //
        this.txtSearchLotNo.Location = new System.Drawing.Point(1030, 15);
        this.txtSearchLotNo.Name = "txtSearchLotNo";
        this.txtSearchLotNo.Size = new System.Drawing.Size(130, 20);
        this.txtSearchLotNo.TabIndex = 11;
        //
        // frmXferStatus
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 800);
        this.Controls.Add(this.panBase);
        this.Name = "frmXferStatus";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).EndInit();
        this.panWork.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcol2_0)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcol2_1)).EndInit();
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
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchXferNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchWoNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchLotNo.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panWork;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private LookUpColumnEdit lookupcol2_0;
    private LookUpColumnEdit lookupcol2_1;
    private DevExpress.XtraGrid.Columns.GridColumn col2Lot;
    private DevExpress.XtraGrid.Columns.GridColumn col2Item;
    private DevExpress.XtraGrid.Columns.GridColumn col2Unit;
    private DevExpress.XtraGrid.Columns.GridColumn col2OutQ;
    private DevExpress.XtraGrid.Columns.GridColumn col2InQ;
    private DevExpress.XtraGrid.Columns.GridColumn col2DiffQ;
    private DevExpress.XtraGrid.Columns.GridColumn col2Resp;
    private DevExpress.XtraGrid.Columns.GridColumn col2Act;
    private DevExpress.XtraGrid.Columns.GridColumn col2DiffDt;
    private DevExpress.XtraGrid.Columns.GridColumn col2DiffRemark;
    private SectionHeaderWyn shSub2;
    private SplitterWyn splitterWyn1;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private LookUpColumnEdit lookupcol1_0;
    private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit hyperlink1;
    private DevExpress.XtraGrid.Columns.GridColumn col1XferNo;
    private DevExpress.XtraGrid.Columns.GridColumn col1XferDate;
    private DevExpress.XtraGrid.Columns.GridColumn col1WoNo;
    private DevExpress.XtraGrid.Columns.GridColumn col1FromProc;
    private DevExpress.XtraGrid.Columns.GridColumn col1FromCust;
    private DevExpress.XtraGrid.Columns.GridColumn col1ToProc;
    private DevExpress.XtraGrid.Columns.GridColumn col1ToCust;
    private DevExpress.XtraGrid.Columns.GridColumn col1LotCnt;
    private DevExpress.XtraGrid.Columns.GridColumn col1OutQty;
    private DevExpress.XtraGrid.Columns.GridColumn col1InQty;
    private DevExpress.XtraGrid.Columns.GridColumn col1DiffQty;
    private DevExpress.XtraGrid.Columns.GridColumn col1Stat;
    private DevExpress.XtraGrid.Columns.GridColumn col1OutDt;
    private DevExpress.XtraGrid.Columns.GridColumn col1InDt;
    private SectionHeaderWyn shList;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchDate;
    private DateEditWyn dteSearchFrom;
    private DevExpress.XtraEditors.LabelControl lblTilde;
    private DateEditWyn dteSearchTo;
    private DevExpress.XtraEditors.LabelControl lblSearchStat;
    private LookUpEditWyn cboSearchStat;
    private DevExpress.XtraEditors.LabelControl lblSearchXferNo;
    private TextEditWyn txtSearchXferNo;
    private DevExpress.XtraEditors.LabelControl lblSearchWoNo;
    private TextEditWyn txtSearchWoNo;
    private DevExpress.XtraEditors.LabelControl lblSearchLotNo;
    private TextEditWyn txtSearchLotNo;
}
