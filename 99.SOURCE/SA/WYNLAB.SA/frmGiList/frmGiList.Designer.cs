// 출고현황(frmGiList) - 출고 목록 조회전용(번호 더블클릭으로 출고등록/수주 열기).
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.SA;

public partial class frmGiList
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
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.lookupcol1_0 = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.hyperlink1 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
        this.col1GiNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1GiDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1InvcNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1SoNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1CustNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1ShipKind = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1ShipDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1Carrier = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1BlNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1LineCnt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1TotalQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1LotList = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1Stat = new DevExpress.XtraGrid.Columns.GridColumn();
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
        this.lblSearchGiNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchGiNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSearchSoNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchSoNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSearchCustNm = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchCustNm = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSearchLotNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchLotNo = new WYNLAB.Base.Controls.TextEditWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        this.panBase.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).BeginInit();
        this.panWork.SuspendLayout();
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
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchGiNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchSoNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchCustNm.Properties)).BeginInit();
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
        this.shList.Text = "출고 목록 (출고번호를 더블클릭하면 출고등록이 열립니다)";
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd1.Location = new System.Drawing.Point(3, 27);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.hyperlink1,
        this.lookupcol1_0});
        this.grd1.Size = new System.Drawing.Size(1664, 719);
        this.grd1.TabIndex = 1;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.col1GiNo,
        this.col1GiDate,
        this.col1InvcNo,
        this.col1SoNo,
        this.col1CustNm,
        this.col1ShipKind,
        this.col1ShipDate,
        this.col1Carrier,
        this.col1BlNo,
        this.col1LineCnt,
        this.col1TotalQty,
        this.col1LotList,
        this.col1Stat});
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
        this.lookupcol1_0.LookupKey = "L_MA0002";
        this.lookupcol1_0.Name = "lookupcol1_0";
        this.lookupcol1_0.NullText = "";
        //
        // hyperlink1
        //
        this.hyperlink1.Name = "hyperlink1";
        //
        // col1GiNo
        //
        this.col1GiNo.Caption = "출고번호";
        this.col1GiNo.ColumnEdit = this.hyperlink1;
        this.col1GiNo.FieldName = "gi_no";
        this.col1GiNo.Name = "col1GiNo";
        this.col1GiNo.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "gi_no", "합계 ({0:#,##0}건)")});
        this.col1GiNo.Visible = true;
        this.col1GiNo.VisibleIndex = 0;
        this.col1GiNo.Width = 110;
        //
        // col1GiDate
        //
        this.col1GiDate.Caption = "출고일자";
        this.col1GiDate.FieldName = "gi_date";
        this.col1GiDate.Name = "col1GiDate";
        this.col1GiDate.Visible = true;
        this.col1GiDate.VisibleIndex = 1;
        this.col1GiDate.Width = 80;
        //
        // col1InvcNo
        //
        this.col1InvcNo.Caption = "명세서번호";
        this.col1InvcNo.FieldName = "invc_no";
        this.col1InvcNo.Name = "col1InvcNo";
        this.col1InvcNo.Visible = true;
        this.col1InvcNo.VisibleIndex = 2;
        this.col1InvcNo.Width = 160;
        //
        // col1SoNo
        //
        this.col1SoNo.Caption = "수주번호";
        this.col1SoNo.FieldName = "so_no";
        this.col1SoNo.Name = "col1SoNo";
        this.col1SoNo.Visible = true;
        this.col1SoNo.VisibleIndex = 3;
        this.col1SoNo.Width = 160;
        //
        // col1CustNm
        //
        this.col1CustNm.Caption = "고객";
        this.col1CustNm.FieldName = "cust_nm";
        this.col1CustNm.Name = "col1CustNm";
        this.col1CustNm.Visible = true;
        this.col1CustNm.VisibleIndex = 4;
        this.col1CustNm.Width = 130;
        //
        // col1ShipKind
        //
        this.col1ShipKind.Caption = "출고구분";
        this.col1ShipKind.FieldName = "ship_kind_nm";
        this.col1ShipKind.Name = "col1ShipKind";
        this.col1ShipKind.Visible = true;
        this.col1ShipKind.VisibleIndex = 5;
        this.col1ShipKind.Width = 130;
        //
        // col1ShipDate
        //
        this.col1ShipDate.Caption = "선적일";
        this.col1ShipDate.FieldName = "ship_date";
        this.col1ShipDate.Name = "col1ShipDate";
        this.col1ShipDate.Visible = true;
        this.col1ShipDate.VisibleIndex = 6;
        this.col1ShipDate.Width = 80;
        //
        // col1Carrier
        //
        this.col1Carrier.Caption = "운송사";
        this.col1Carrier.FieldName = "carrier";
        this.col1Carrier.Name = "col1Carrier";
        this.col1Carrier.Visible = true;
        this.col1Carrier.VisibleIndex = 7;
        this.col1Carrier.Width = 100;
        //
        // col1BlNo
        //
        this.col1BlNo.Caption = "B/L번호";
        this.col1BlNo.FieldName = "bl_no";
        this.col1BlNo.Name = "col1BlNo";
        this.col1BlNo.Visible = true;
        this.col1BlNo.VisibleIndex = 8;
        this.col1BlNo.Width = 120;
        //
        // col1LineCnt
        //
        this.col1LineCnt.Caption = "라인수";
        this.col1LineCnt.DisplayFormat.FormatString = "#,##0.####";
        this.col1LineCnt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col1LineCnt.FieldName = "line_cnt";
        this.col1LineCnt.Name = "col1LineCnt";
        this.col1LineCnt.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "line_cnt", "{0:#,##0.####}")});
        this.col1LineCnt.Visible = true;
        this.col1LineCnt.VisibleIndex = 9;
        this.col1LineCnt.Width = 60;
        //
        // col1TotalQty
        //
        this.col1TotalQty.Caption = "출고수량";
        this.col1TotalQty.DisplayFormat.FormatString = "#,##0.####";
        this.col1TotalQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col1TotalQty.FieldName = "total_qty";
        this.col1TotalQty.Name = "col1TotalQty";
        this.col1TotalQty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "total_qty", "{0:#,##0.####}")});
        this.col1TotalQty.Visible = true;
        this.col1TotalQty.VisibleIndex = 10;
        this.col1TotalQty.Width = 90;
        //
        // col1LotList
        //
        this.col1LotList.Caption = "LOT";
        this.col1LotList.FieldName = "lot_list";
        this.col1LotList.Name = "col1LotList";
        this.col1LotList.Visible = true;
        this.col1LotList.VisibleIndex = 11;
        this.col1LotList.Width = 260;
        //
        // col1Stat
        //
        this.col1Stat.Caption = "상태";
        this.col1Stat.ColumnEdit = this.lookupcol1_0;
        this.col1Stat.FieldName = "stat_cd";
        this.col1Stat.Name = "col1Stat";
        this.col1Stat.Visible = true;
        this.col1Stat.VisibleIndex = 12;
        this.col1Stat.Width = 70;
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
        this.panHeader.Controls.Add(this.lblSearchGiNo);
        this.panHeader.Controls.Add(this.txtSearchGiNo);
        this.panHeader.Controls.Add(this.lblSearchSoNo);
        this.panHeader.Controls.Add(this.txtSearchSoNo);
        this.panHeader.Controls.Add(this.lblSearchCustNm);
        this.panHeader.Controls.Add(this.txtSearchCustNm);
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
        this.lblSearchDate.Text = "출고일자";
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
        this.cboSearchStat.LookupKey = "L_MA0002";
        this.cboSearchStat.Name = "cboSearchStat";
        this.cboSearchStat.Properties.NullText = "";
        this.cboSearchStat.Size = new System.Drawing.Size(110, 20);
        this.cboSearchStat.TabIndex = 5;
        //
        // lblSearchGiNo
        //
        this.lblSearchGiNo.Location = new System.Drawing.Point(725, 18);
        this.lblSearchGiNo.Name = "lblSearchGiNo";
        this.lblSearchGiNo.Size = new System.Drawing.Size(52, 15);
        this.lblSearchGiNo.TabIndex = 6;
        this.lblSearchGiNo.Text = "출고번호";
        //
        // txtSearchGiNo
        //
        this.txtSearchGiNo.Location = new System.Drawing.Point(791, 15);
        this.txtSearchGiNo.Name = "txtSearchGiNo";
        this.txtSearchGiNo.Size = new System.Drawing.Size(130, 20);
        this.txtSearchGiNo.TabIndex = 7;
        //
        // lblSearchSoNo
        //
        this.lblSearchSoNo.Location = new System.Drawing.Point(945, 18);
        this.lblSearchSoNo.Name = "lblSearchSoNo";
        this.lblSearchSoNo.Size = new System.Drawing.Size(52, 15);
        this.lblSearchSoNo.TabIndex = 8;
        this.lblSearchSoNo.Text = "수주번호";
        //
        // txtSearchSoNo
        //
        this.txtSearchSoNo.Location = new System.Drawing.Point(1011, 15);
        this.txtSearchSoNo.Name = "txtSearchSoNo";
        this.txtSearchSoNo.Size = new System.Drawing.Size(130, 20);
        this.txtSearchSoNo.TabIndex = 9;
        //
        // lblSearchCustNm
        //
        this.lblSearchCustNm.Location = new System.Drawing.Point(1165, 18);
        this.lblSearchCustNm.Name = "lblSearchCustNm";
        this.lblSearchCustNm.Size = new System.Drawing.Size(26, 15);
        this.lblSearchCustNm.TabIndex = 10;
        this.lblSearchCustNm.Text = "고객";
        //
        // txtSearchCustNm
        //
        this.txtSearchCustNm.Location = new System.Drawing.Point(1205, 15);
        this.txtSearchCustNm.Name = "txtSearchCustNm";
        this.txtSearchCustNm.Size = new System.Drawing.Size(130, 20);
        this.txtSearchCustNm.TabIndex = 11;
        //
        // lblSearchLotNo
        //
        this.lblSearchLotNo.Location = new System.Drawing.Point(1359, 18);
        this.lblSearchLotNo.Name = "lblSearchLotNo";
        this.lblSearchLotNo.Size = new System.Drawing.Size(24, 15);
        this.lblSearchLotNo.TabIndex = 12;
        this.lblSearchLotNo.Text = "LOT";
        //
        // txtSearchLotNo
        //
        this.txtSearchLotNo.Location = new System.Drawing.Point(1397, 15);
        this.txtSearchLotNo.Name = "txtSearchLotNo";
        this.txtSearchLotNo.Size = new System.Drawing.Size(130, 20);
        this.txtSearchLotNo.TabIndex = 13;
        //
        // frmGiList
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 800);
        this.Controls.Add(this.panBase);
        this.Name = "frmGiList";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).EndInit();
        this.panWork.ResumeLayout(false);
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
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchGiNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchSoNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchCustNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchLotNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panWork;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private LookUpColumnEdit lookupcol1_0;
    private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit hyperlink1;
    private DevExpress.XtraGrid.Columns.GridColumn col1GiNo;
    private DevExpress.XtraGrid.Columns.GridColumn col1GiDate;
    private DevExpress.XtraGrid.Columns.GridColumn col1InvcNo;
    private DevExpress.XtraGrid.Columns.GridColumn col1SoNo;
    private DevExpress.XtraGrid.Columns.GridColumn col1CustNm;
    private DevExpress.XtraGrid.Columns.GridColumn col1ShipKind;
    private DevExpress.XtraGrid.Columns.GridColumn col1ShipDate;
    private DevExpress.XtraGrid.Columns.GridColumn col1Carrier;
    private DevExpress.XtraGrid.Columns.GridColumn col1BlNo;
    private DevExpress.XtraGrid.Columns.GridColumn col1LineCnt;
    private DevExpress.XtraGrid.Columns.GridColumn col1TotalQty;
    private DevExpress.XtraGrid.Columns.GridColumn col1LotList;
    private DevExpress.XtraGrid.Columns.GridColumn col1Stat;
    private SectionHeaderWyn shList;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchDate;
    private DateEditWyn dteSearchFrom;
    private DevExpress.XtraEditors.LabelControl lblTilde;
    private DateEditWyn dteSearchTo;
    private DevExpress.XtraEditors.LabelControl lblSearchStat;
    private LookUpEditWyn cboSearchStat;
    private DevExpress.XtraEditors.LabelControl lblSearchGiNo;
    private TextEditWyn txtSearchGiNo;
    private DevExpress.XtraEditors.LabelControl lblSearchSoNo;
    private TextEditWyn txtSearchSoNo;
    private DevExpress.XtraEditors.LabelControl lblSearchCustNm;
    private TextEditWyn txtSearchCustNm;
    private DevExpress.XtraEditors.LabelControl lblSearchLotNo;
    private TextEditWyn txtSearchLotNo;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private WYNLAB.Base.Controls.LookUpEditWyn cboSearchAccId;
}
