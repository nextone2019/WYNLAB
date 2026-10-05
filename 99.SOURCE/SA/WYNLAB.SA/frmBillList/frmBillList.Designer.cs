// 매출현황(frmBillList) - 매출 라인 단위 목록 조회전용(매출번호 더블클릭으로 매출등록 열기).
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.SA;

public partial class frmBillList
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
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblSearchDate = new DevExpress.XtraEditors.LabelControl();
        this.dteSearchFrom = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblTilde = new DevExpress.XtraEditors.LabelControl();
        this.dteSearchTo = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblSearchStat = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchStat = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblSearchBillNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchBillNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSearchInvcNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchInvcNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSearchCustNm = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchCustNm = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSearchKeyword = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchKeyword = new WYNLAB.Base.Controls.TextEditWyn();
        this.shList = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.hyperlink1 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
        this.lookupcol1_0 = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.col1BillNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1BillDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1CustNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1Stat = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1TaxInvNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1TaxInvDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1Serl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1InvcNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1SoNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1ItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1ItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1Unit = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1Qty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1Price = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1Amt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1Vat = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1TotalAmt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1CurCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1GiStatus = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1Remark = new DevExpress.XtraGrid.Columns.GridColumn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchStat.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchBillNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchInvcNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchCustNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.hyperlink1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcol1_0)).BeginInit();
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
        this.panHeader.Controls.Add(this.lblSearchBillNo);
        this.panHeader.Controls.Add(this.txtSearchBillNo);
        this.panHeader.Controls.Add(this.lblSearchInvcNo);
        this.panHeader.Controls.Add(this.txtSearchInvcNo);
        this.panHeader.Controls.Add(this.lblSearchCustNm);
        this.panHeader.Controls.Add(this.txtSearchCustNm);
        this.panHeader.Controls.Add(this.lblSearchKeyword);
        this.panHeader.Controls.Add(this.txtSearchKeyword);
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
        this.lblSearchDate.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchDate.Appearance.Options.UseFont = true;
        this.lblSearchDate.Location = new System.Drawing.Point(224, 18);
        this.lblSearchDate.Name = "lblSearchDate";
        this.lblSearchDate.Size = new System.Drawing.Size(58, 15);
        this.lblSearchDate.TabIndex = 2;
        this.lblSearchDate.Text = "매출일자";
        //
        // dteSearchFrom
        //
        this.dteSearchFrom.Location = new System.Drawing.Point(296, 15);
        this.dteSearchFrom.Name = "dteSearchFrom";
        this.dteSearchFrom.Size = new System.Drawing.Size(110, 20);
        this.dteSearchFrom.TabIndex = 3;
        //
        // lblTilde
        //
        this.lblTilde.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblTilde.Appearance.Options.UseFont = true;
        this.lblTilde.Location = new System.Drawing.Point(412, 18);
        this.lblTilde.Name = "lblTilde";
        this.lblTilde.Size = new System.Drawing.Size(7, 15);
        this.lblTilde.TabIndex = 4;
        this.lblTilde.Text = "~";
        //
        // dteSearchTo
        //
        this.dteSearchTo.Location = new System.Drawing.Point(425, 15);
        this.dteSearchTo.Name = "dteSearchTo";
        this.dteSearchTo.Size = new System.Drawing.Size(110, 20);
        this.dteSearchTo.TabIndex = 5;
        //
        // lblSearchStat
        //
        this.lblSearchStat.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchStat.Appearance.Options.UseFont = true;
        this.lblSearchStat.Location = new System.Drawing.Point(559, 18);
        this.lblSearchStat.Name = "lblSearchStat";
        this.lblSearchStat.Size = new System.Drawing.Size(30, 15);
        this.lblSearchStat.TabIndex = 6;
        this.lblSearchStat.Text = "상태";
        //
        // cboSearchStat
        //
        this.cboSearchStat.Location = new System.Drawing.Point(603, 15);
        this.cboSearchStat.LookupKey = "L_MA0002";
        this.cboSearchStat.Properties.NullText = "";
        this.cboSearchStat.Name = "cboSearchStat";
        this.cboSearchStat.Size = new System.Drawing.Size(110, 20);
        this.cboSearchStat.TabIndex = 7;
        //
        // lblSearchBillNo
        //
        this.lblSearchBillNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchBillNo.Appearance.Options.UseFont = true;
        this.lblSearchBillNo.Location = new System.Drawing.Point(737, 18);
        this.lblSearchBillNo.Name = "lblSearchBillNo";
        this.lblSearchBillNo.Size = new System.Drawing.Size(58, 15);
        this.lblSearchBillNo.TabIndex = 8;
        this.lblSearchBillNo.Text = "매출번호";
        //
        // txtSearchBillNo
        //
        this.txtSearchBillNo.Location = new System.Drawing.Point(809, 15);
        this.txtSearchBillNo.Name = "txtSearchBillNo";
        this.txtSearchBillNo.Size = new System.Drawing.Size(130, 20);
        this.txtSearchBillNo.TabIndex = 9;
        //
        // lblSearchInvcNo
        //
        this.lblSearchInvcNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchInvcNo.Appearance.Options.UseFont = true;
        this.lblSearchInvcNo.Location = new System.Drawing.Point(963, 18);
        this.lblSearchInvcNo.Name = "lblSearchInvcNo";
        this.lblSearchInvcNo.Size = new System.Drawing.Size(72, 15);
        this.lblSearchInvcNo.TabIndex = 10;
        this.lblSearchInvcNo.Text = "명세서번호";
        //
        // txtSearchInvcNo
        //
        this.txtSearchInvcNo.Location = new System.Drawing.Point(1049, 15);
        this.txtSearchInvcNo.Name = "txtSearchInvcNo";
        this.txtSearchInvcNo.Size = new System.Drawing.Size(130, 20);
        this.txtSearchInvcNo.TabIndex = 11;
        //
        // lblSearchCustNm
        //
        this.lblSearchCustNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchCustNm.Appearance.Options.UseFont = true;
        this.lblSearchCustNm.Location = new System.Drawing.Point(1203, 18);
        this.lblSearchCustNm.Name = "lblSearchCustNm";
        this.lblSearchCustNm.Size = new System.Drawing.Size(30, 15);
        this.lblSearchCustNm.TabIndex = 12;
        this.lblSearchCustNm.Text = "고객";
        //
        // txtSearchCustNm
        //
        this.txtSearchCustNm.Location = new System.Drawing.Point(1247, 15);
        this.txtSearchCustNm.Name = "txtSearchCustNm";
        this.txtSearchCustNm.Size = new System.Drawing.Size(130, 20);
        this.txtSearchCustNm.TabIndex = 13;
        //
        // lblSearchKeyword
        //
        this.lblSearchKeyword.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchKeyword.Appearance.Options.UseFont = true;
        this.lblSearchKeyword.Location = new System.Drawing.Point(1401, 18);
        this.lblSearchKeyword.Name = "lblSearchKeyword";
        this.lblSearchKeyword.Size = new System.Drawing.Size(65, 15);
        this.lblSearchKeyword.TabIndex = 14;
        this.lblSearchKeyword.Text = "품번/품명";
        //
        // txtSearchKeyword
        //
        this.txtSearchKeyword.Location = new System.Drawing.Point(1480, 15);
        this.txtSearchKeyword.Name = "txtSearchKeyword";
        this.txtSearchKeyword.Size = new System.Drawing.Size(130, 20);
        this.txtSearchKeyword.TabIndex = 15;
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
        this.shList.TabIndex = 16;
        this.shList.Text = "매출 목록 (매출번호를 더블클릭하면 매출등록이 열립니다)";
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
        this.grd1.TabIndex = 17;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.col1BillNo,
        this.col1BillDate,
        this.col1CustNm,
        this.col1Stat,
        this.col1TaxInvNo,
        this.col1TaxInvDate,
        this.col1Serl,
        this.col1InvcNo,
        this.col1SoNo,
        this.col1ItemNo,
        this.col1ItemNm,
        this.col1Unit,
        this.col1Qty,
        this.col1Price,
        this.col1Amt,
        this.col1Vat,
        this.col1TotalAmt,
        this.col1CurCd,
        this.col1GiStatus,
        this.col1Remark});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsBehavior.Editable = false;
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowFooter = true;
        this.gvw1.OptionsView.ShowGroupPanel = false;
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
        // col1BillNo
        //
        this.col1BillNo.Caption = "매출번호";
        this.col1BillNo.ColumnEdit = this.hyperlink1;
        this.col1BillNo.FieldName = "bill_no";
        this.col1BillNo.Name = "col1BillNo";
        this.col1BillNo.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "bill_no", "합계 ({0:#,##0}건)")});
        this.col1BillNo.Visible = true;
        this.col1BillNo.VisibleIndex = 0;
        this.col1BillNo.Width = 110;
        //
        // col1BillDate
        //
        this.col1BillDate.Caption = "매출일자";
        this.col1BillDate.FieldName = "bill_date";
        this.col1BillDate.Name = "col1BillDate";
        this.col1BillDate.Visible = true;
        this.col1BillDate.VisibleIndex = 1;
        this.col1BillDate.Width = 80;
        //
        // col1CustNm
        //
        this.col1CustNm.Caption = "고객";
        this.col1CustNm.FieldName = "cust_nm";
        this.col1CustNm.Name = "col1CustNm";
        this.col1CustNm.Visible = true;
        this.col1CustNm.VisibleIndex = 2;
        this.col1CustNm.Width = 130;
        //
        // col1Stat
        //
        this.col1Stat.Caption = "상태";
        this.col1Stat.ColumnEdit = this.lookupcol1_0;
        this.col1Stat.FieldName = "stat_cd";
        this.col1Stat.Name = "col1Stat";
        this.col1Stat.Visible = true;
        this.col1Stat.VisibleIndex = 3;
        this.col1Stat.Width = 70;
        //
        // col1TaxInvNo
        //
        this.col1TaxInvNo.Caption = "계산서번호";
        this.col1TaxInvNo.FieldName = "tax_inv_no";
        this.col1TaxInvNo.Name = "col1TaxInvNo";
        this.col1TaxInvNo.Visible = true;
        this.col1TaxInvNo.VisibleIndex = 4;
        this.col1TaxInvNo.Width = 110;
        //
        // col1TaxInvDate
        //
        this.col1TaxInvDate.Caption = "계산서일자";
        this.col1TaxInvDate.FieldName = "tax_inv_date";
        this.col1TaxInvDate.Name = "col1TaxInvDate";
        this.col1TaxInvDate.Visible = true;
        this.col1TaxInvDate.VisibleIndex = 5;
        this.col1TaxInvDate.Width = 80;
        //
        // col1Serl
        //
        this.col1Serl.Caption = "순번";
        this.col1Serl.FieldName = "serl";
        this.col1Serl.Name = "col1Serl";
        this.col1Serl.Visible = true;
        this.col1Serl.VisibleIndex = 6;
        this.col1Serl.Width = 45;
        //
        // col1InvcNo
        //
        this.col1InvcNo.Caption = "명세서번호";
        this.col1InvcNo.FieldName = "invc_no";
        this.col1InvcNo.Name = "col1InvcNo";
        this.col1InvcNo.Visible = true;
        this.col1InvcNo.VisibleIndex = 7;
        this.col1InvcNo.Width = 110;
        //
        // col1SoNo
        //
        this.col1SoNo.Caption = "수주번호";
        this.col1SoNo.FieldName = "so_no";
        this.col1SoNo.Name = "col1SoNo";
        this.col1SoNo.Visible = true;
        this.col1SoNo.VisibleIndex = 8;
        this.col1SoNo.Width = 110;
        //
        // col1ItemNo
        //
        this.col1ItemNo.Caption = "품번";
        this.col1ItemNo.FieldName = "item_no";
        this.col1ItemNo.Name = "col1ItemNo";
        this.col1ItemNo.Visible = true;
        this.col1ItemNo.VisibleIndex = 9;
        this.col1ItemNo.Width = 110;
        //
        // col1ItemNm
        //
        this.col1ItemNm.Caption = "품명";
        this.col1ItemNm.FieldName = "item_nm";
        this.col1ItemNm.Name = "col1ItemNm";
        this.col1ItemNm.Visible = true;
        this.col1ItemNm.VisibleIndex = 10;
        this.col1ItemNm.Width = 150;
        //
        // col1Unit
        //
        this.col1Unit.Caption = "단위";
        this.col1Unit.FieldName = "unit_cd";
        this.col1Unit.Name = "col1Unit";
        this.col1Unit.Visible = true;
        this.col1Unit.VisibleIndex = 11;
        this.col1Unit.Width = 50;
        //
        // col1Qty
        //
        this.col1Qty.Caption = "매출수량";
        this.col1Qty.DisplayFormat.FormatString = "#,##0.####";
        this.col1Qty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col1Qty.FieldName = "qty";
        this.col1Qty.Name = "col1Qty";
        this.col1Qty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "qty", "{0:#,##0.####}")});
        this.col1Qty.Visible = true;
        this.col1Qty.VisibleIndex = 12;
        this.col1Qty.Width = 90;
        //
        // col1Price
        //
        this.col1Price.Caption = "단가";
        this.col1Price.DisplayFormat.FormatString = "#,##0.####";
        this.col1Price.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col1Price.FieldName = "price";
        this.col1Price.Name = "col1Price";
        this.col1Price.Visible = true;
        this.col1Price.VisibleIndex = 13;
        this.col1Price.Width = 90;
        //
        // col1Amt
        //
        this.col1Amt.Caption = "공급가";
        this.col1Amt.DisplayFormat.FormatString = "#,##0.####";
        this.col1Amt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col1Amt.FieldName = "amt";
        this.col1Amt.Name = "col1Amt";
        this.col1Amt.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "amt", "{0:#,##0.####}")});
        this.col1Amt.Visible = true;
        this.col1Amt.VisibleIndex = 14;
        this.col1Amt.Width = 110;
        //
        // col1Vat
        //
        this.col1Vat.Caption = "부가세";
        this.col1Vat.DisplayFormat.FormatString = "#,##0.####";
        this.col1Vat.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col1Vat.FieldName = "vat";
        this.col1Vat.Name = "col1Vat";
        this.col1Vat.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "vat", "{0:#,##0.####}")});
        this.col1Vat.Visible = true;
        this.col1Vat.VisibleIndex = 15;
        this.col1Vat.Width = 100;
        //
        // col1TotalAmt
        //
        this.col1TotalAmt.Caption = "합계금액";
        this.col1TotalAmt.DisplayFormat.FormatString = "#,##0.####";
        this.col1TotalAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col1TotalAmt.FieldName = "total_amt";
        this.col1TotalAmt.Name = "col1TotalAmt";
        this.col1TotalAmt.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "total_amt", "{0:#,##0.####}")});
        this.col1TotalAmt.Visible = true;
        this.col1TotalAmt.VisibleIndex = 16;
        this.col1TotalAmt.Width = 110;
        //
        // col1CurCd
        //
        this.col1CurCd.Caption = "통화";
        this.col1CurCd.FieldName = "cur_cd";
        this.col1CurCd.Name = "col1CurCd";
        this.col1CurCd.Visible = true;
        this.col1CurCd.VisibleIndex = 17;
        this.col1CurCd.Width = 50;
        //
        // col1GiStatus
        //
        this.col1GiStatus.Caption = "출고상태";
        this.col1GiStatus.FieldName = "gi_status";
        this.col1GiStatus.Name = "col1GiStatus";
        this.col1GiStatus.Visible = true;
        this.col1GiStatus.VisibleIndex = 18;
        this.col1GiStatus.Width = 80;
        //
        // col1Remark
        //
        this.col1Remark.Caption = "비고";
        this.col1Remark.FieldName = "remark";
        this.col1Remark.Name = "col1Remark";
        this.col1Remark.Visible = true;
        this.col1Remark.VisibleIndex = 19;
        this.col1Remark.Width = 200;
        //
        // frmBillList
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 800);
        this.Controls.Add(this.panBase);
        this.Name = "frmBillList";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchStat.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchBillNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchInvcNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchCustNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.hyperlink1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcol1_0)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panWork;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private LookUpEditWyn cboSearchAccId;
    private DevExpress.XtraEditors.LabelControl lblSearchDate;
    private DateEditWyn dteSearchFrom;
    private DevExpress.XtraEditors.LabelControl lblTilde;
    private DateEditWyn dteSearchTo;
    private DevExpress.XtraEditors.LabelControl lblSearchStat;
    private LookUpEditWyn cboSearchStat;
    private DevExpress.XtraEditors.LabelControl lblSearchBillNo;
    private TextEditWyn txtSearchBillNo;
    private DevExpress.XtraEditors.LabelControl lblSearchInvcNo;
    private TextEditWyn txtSearchInvcNo;
    private DevExpress.XtraEditors.LabelControl lblSearchCustNm;
    private TextEditWyn txtSearchCustNm;
    private DevExpress.XtraEditors.LabelControl lblSearchKeyword;
    private TextEditWyn txtSearchKeyword;
    private SectionHeaderWyn shList;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit hyperlink1;
    private LookUpColumnEdit lookupcol1_0;
    private DevExpress.XtraGrid.Columns.GridColumn col1BillNo;
    private DevExpress.XtraGrid.Columns.GridColumn col1BillDate;
    private DevExpress.XtraGrid.Columns.GridColumn col1CustNm;
    private DevExpress.XtraGrid.Columns.GridColumn col1Stat;
    private DevExpress.XtraGrid.Columns.GridColumn col1TaxInvNo;
    private DevExpress.XtraGrid.Columns.GridColumn col1TaxInvDate;
    private DevExpress.XtraGrid.Columns.GridColumn col1Serl;
    private DevExpress.XtraGrid.Columns.GridColumn col1InvcNo;
    private DevExpress.XtraGrid.Columns.GridColumn col1SoNo;
    private DevExpress.XtraGrid.Columns.GridColumn col1ItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn col1ItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn col1Unit;
    private DevExpress.XtraGrid.Columns.GridColumn col1Qty;
    private DevExpress.XtraGrid.Columns.GridColumn col1Price;
    private DevExpress.XtraGrid.Columns.GridColumn col1Amt;
    private DevExpress.XtraGrid.Columns.GridColumn col1Vat;
    private DevExpress.XtraGrid.Columns.GridColumn col1TotalAmt;
    private DevExpress.XtraGrid.Columns.GridColumn col1CurCd;
    private DevExpress.XtraGrid.Columns.GridColumn col1GiStatus;
    private DevExpress.XtraGrid.Columns.GridColumn col1Remark;
}
