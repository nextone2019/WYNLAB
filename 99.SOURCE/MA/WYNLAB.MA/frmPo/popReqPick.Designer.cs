// 구매요청 불러오기 팝업(popReqPick) 디자이너 - VS 디자이너로 배치를 편집할 수 있다. 로직은 popReqPick.cs.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

public partial class popReqPick
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.lblDate = new DevExpress.XtraEditors.LabelControl();
        this.dteFrom = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblTilde = new DevExpress.XtraEditors.LabelControl();
        this.dteTo = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblReqNo = new DevExpress.XtraEditors.LabelControl();
        this.txtReqNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblKeyword = new DevExpress.XtraEditors.LabelControl();
        this.txtKeyword = new WYNLAB.Base.Controls.TextEditWyn();
        this.btnSearch = new WYNLAB.Base.Controls.ButtonWyn();
        this.lblCust = new DevExpress.XtraEditors.LabelControl();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.colSel = new DevExpress.XtraGrid.Columns.GridColumn();
        this.chkSel = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
        this.colReqNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colReqDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colReqTitle = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCustNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colNextQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRemainQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDelvDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.panFooter = new WYNLAB.Base.Controls.PanelWyn();
        this.chkAll = new DevExpress.XtraEditors.CheckEdit();
        this.lblHint = new DevExpress.XtraEditors.LabelControl();
        this.btnOk = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnClose = new WYNLAB.Base.Controls.ButtonWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dteFrom.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteFrom.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteTo.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteTo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtReqNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtKeyword.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkSel)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panFooter)).BeginInit();
        this.panFooter.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.chkAll.Properties)).BeginInit();
        this.SuspendLayout();
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblDate);
        this.panHeader.Controls.Add(this.dteFrom);
        this.panHeader.Controls.Add(this.lblTilde);
        this.panHeader.Controls.Add(this.dteTo);
        this.panHeader.Controls.Add(this.lblReqNo);
        this.panHeader.Controls.Add(this.txtReqNo);
        this.panHeader.Controls.Add(this.lblKeyword);
        this.panHeader.Controls.Add(this.txtKeyword);
        this.panHeader.Controls.Add(this.btnSearch);
        this.panHeader.Controls.Add(this.lblCust);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.Location = new System.Drawing.Point(0, 0);
        this.panHeader.Name = "panHeader";
        this.panHeader.Size = new System.Drawing.Size(1000, 72);
        this.panHeader.TabIndex = 0;
        //
        // lblDate
        //
        this.lblDate.Location = new System.Drawing.Point(28, 14);
        this.lblDate.Name = "lblDate";
        this.lblDate.Size = new System.Drawing.Size(48, 15);
        this.lblDate.TabIndex = 0;
        this.lblDate.Text = "요청일자";
        //
        // dteFrom
        //
        this.dteFrom.Location = new System.Drawing.Point(72, 11);
        this.dteFrom.Name = "dteFrom";
        this.dteFrom.Size = new System.Drawing.Size(110, 20);
        this.dteFrom.TabIndex = 1;
        //
        // lblTilde
        //
        this.lblTilde.Location = new System.Drawing.Point(188, 14);
        this.lblTilde.Name = "lblTilde";
        this.lblTilde.Size = new System.Drawing.Size(7, 15);
        this.lblTilde.TabIndex = 2;
        this.lblTilde.Text = "~";
        //
        // dteTo
        //
        this.dteTo.Location = new System.Drawing.Point(201, 11);
        this.dteTo.Name = "dteTo";
        this.dteTo.Size = new System.Drawing.Size(110, 20);
        this.dteTo.TabIndex = 3;
        //
        // lblReqNo
        //
        this.lblReqNo.Location = new System.Drawing.Point(334, 14);
        this.lblReqNo.Name = "lblReqNo";
        this.lblReqNo.Size = new System.Drawing.Size(66, 15);
        this.lblReqNo.TabIndex = 4;
        this.lblReqNo.Text = "구매요청번호";
        //
        // txtReqNo
        //
        this.txtReqNo.Location = new System.Drawing.Point(408, 11);
        this.txtReqNo.Name = "txtReqNo";
        this.txtReqNo.Size = new System.Drawing.Size(130, 20);
        this.txtReqNo.TabIndex = 5;
        //
        // lblKeyword
        //
        this.lblKeyword.Location = new System.Drawing.Point(562, 14);
        this.lblKeyword.Name = "lblKeyword";
        this.lblKeyword.Size = new System.Drawing.Size(72, 15);
        this.lblKeyword.TabIndex = 6;
        this.lblKeyword.Text = "품번/품명/규격";
        //
        // txtKeyword
        //
        this.txtKeyword.Location = new System.Drawing.Point(642, 11);
        this.txtKeyword.Name = "txtKeyword";
        this.txtKeyword.Size = new System.Drawing.Size(160, 20);
        this.txtKeyword.TabIndex = 7;
        //
        // btnSearch
        //
        this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnSearch.Location = new System.Drawing.Point(822, 9);
        this.btnSearch.Name = "btnSearch";
        this.btnSearch.Size = new System.Drawing.Size(80, 24);
        this.btnSearch.TabIndex = 8;
        this.btnSearch.Text = "조회";
        //
        // lblCust
        //
        this.lblCust.Location = new System.Drawing.Point(16, 46);
        this.lblCust.Name = "lblCust";
        this.lblCust.Size = new System.Drawing.Size(60, 15);
        this.lblCust.TabIndex = 9;
        this.lblCust.Text = "거래처 전체";
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd1.Location = new System.Drawing.Point(0, 72);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.chkSel});
        this.grd1.Size = new System.Drawing.Size(1000, 396);
        this.grd1.TabIndex = 1;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colSel,
        this.colReqNo,
        this.colReqDate,
        this.colReqTitle,
        this.colCustNm,
        this.colItemNo,
        this.colItemNm,
        this.colItemSpec,
        this.colUnitCd,
        this.colQty,
        this.colNextQty,
        this.colRemainQty,
        this.colDelvDate});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        //
        // colSel
        //
        this.colSel.Caption = "선택";
        this.colSel.ColumnEdit = this.chkSel;
        this.colSel.FieldName = "sel";
        this.colSel.Name = "colSel";
        this.colSel.Visible = true;
        this.colSel.VisibleIndex = 0;
        this.colSel.Width = 45;
        //
        // chkSel
        //
        this.chkSel.AutoHeight = false;
        this.chkSel.Name = "chkSel";
        //
        // colReqNo
        //
        this.colReqNo.Caption = "구매요청번호";
        this.colReqNo.FieldName = "req_no";
        this.colReqNo.Name = "colReqNo";
        this.colReqNo.OptionsColumn.AllowEdit = false;
        this.colReqNo.Visible = true;
        this.colReqNo.VisibleIndex = 1;
        this.colReqNo.Width = 110;
        //
        // colReqDate
        //
        this.colReqDate.Caption = "요청일자";
        this.colReqDate.FieldName = "req_date";
        this.colReqDate.Name = "colReqDate";
        this.colReqDate.OptionsColumn.AllowEdit = false;
        this.colReqDate.Visible = true;
        this.colReqDate.VisibleIndex = 2;
        this.colReqDate.Width = 80;
        //
        // colReqTitle
        //
        this.colReqTitle.Caption = "구매요청명";
        this.colReqTitle.FieldName = "req_title";
        this.colReqTitle.Name = "colReqTitle";
        this.colReqTitle.OptionsColumn.AllowEdit = false;
        this.colReqTitle.Visible = true;
        this.colReqTitle.VisibleIndex = 3;
        this.colReqTitle.Width = 150;
        //
        // colCustNm
        //
        this.colCustNm.Caption = "거래처";
        this.colCustNm.FieldName = "cust_nm";
        this.colCustNm.Name = "colCustNm";
        this.colCustNm.OptionsColumn.AllowEdit = false;
        this.colCustNm.Visible = true;
        this.colCustNm.VisibleIndex = 4;
        this.colCustNm.Width = 110;
        //
        // colItemNo
        //
        this.colItemNo.Caption = "품번";
        this.colItemNo.FieldName = "item_no";
        this.colItemNo.Name = "colItemNo";
        this.colItemNo.OptionsColumn.AllowEdit = false;
        this.colItemNo.Visible = true;
        this.colItemNo.VisibleIndex = 5;
        this.colItemNo.Width = 100;
        //
        // colItemNm
        //
        this.colItemNm.Caption = "품명";
        this.colItemNm.FieldName = "item_nm";
        this.colItemNm.Name = "colItemNm";
        this.colItemNm.OptionsColumn.AllowEdit = false;
        this.colItemNm.Visible = true;
        this.colItemNm.VisibleIndex = 6;
        this.colItemNm.Width = 140;
        //
        // colItemSpec
        //
        this.colItemSpec.Caption = "규격";
        this.colItemSpec.FieldName = "item_spec";
        this.colItemSpec.Name = "colItemSpec";
        this.colItemSpec.OptionsColumn.AllowEdit = false;
        this.colItemSpec.Visible = true;
        this.colItemSpec.VisibleIndex = 7;
        this.colItemSpec.Width = 120;
        //
        // colUnitCd
        //
        this.colUnitCd.Caption = "단위";
        this.colUnitCd.FieldName = "unit_cd";
        this.colUnitCd.Name = "colUnitCd";
        this.colUnitCd.OptionsColumn.AllowEdit = false;
        this.colUnitCd.Visible = true;
        this.colUnitCd.VisibleIndex = 8;
        this.colUnitCd.Width = 50;
        //
        // colQty
        //
        this.colQty.Caption = "요청수량";
        this.colQty.DisplayFormat.FormatString = "#,##0.####";
        this.colQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colQty.FieldName = "qty";
        this.colQty.Name = "colQty";
        this.colQty.OptionsColumn.AllowEdit = false;
        this.colQty.Visible = true;
        this.colQty.VisibleIndex = 9;
        this.colQty.Width = 80;
        //
        // colNextQty
        //
        this.colNextQty.Caption = "발주수량";
        this.colNextQty.DisplayFormat.FormatString = "#,##0.####";
        this.colNextQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colNextQty.FieldName = "next_qty";
        this.colNextQty.Name = "colNextQty";
        this.colNextQty.OptionsColumn.AllowEdit = false;
        this.colNextQty.Visible = true;
        this.colNextQty.VisibleIndex = 10;
        this.colNextQty.Width = 80;
        //
        // colRemainQty
        //
        this.colRemainQty.Caption = "잔량";
        this.colRemainQty.DisplayFormat.FormatString = "#,##0.####";
        this.colRemainQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colRemainQty.FieldName = "remain_qty";
        this.colRemainQty.Name = "colRemainQty";
        this.colRemainQty.OptionsColumn.AllowEdit = false;
        this.colRemainQty.Visible = true;
        this.colRemainQty.VisibleIndex = 11;
        this.colRemainQty.Width = 80;
        //
        // colDelvDate
        //
        this.colDelvDate.Caption = "납기일자";
        this.colDelvDate.FieldName = "delv_date";
        this.colDelvDate.Name = "colDelvDate";
        this.colDelvDate.OptionsColumn.AllowEdit = false;
        this.colDelvDate.Visible = true;
        this.colDelvDate.VisibleIndex = 12;
        this.colDelvDate.Width = 80;
        //
        // panFooter
        //
        this.panFooter.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panFooter.Appearance.Options.UseBackColor = true;
        this.panFooter.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panFooter.Controls.Add(this.chkAll);
        this.panFooter.Controls.Add(this.lblHint);
        this.panFooter.Controls.Add(this.btnOk);
        this.panFooter.Controls.Add(this.btnClose);
        this.panFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panFooter.Location = new System.Drawing.Point(0, 468);
        this.panFooter.Name = "panFooter";
        this.panFooter.Size = new System.Drawing.Size(1000, 44);
        this.panFooter.TabIndex = 2;
        //
        // chkAll
        //
        this.chkAll.Location = new System.Drawing.Point(16, 12);
        this.chkAll.Name = "chkAll";
        this.chkAll.Properties.Caption = "전체 선택";
        this.chkAll.Size = new System.Drawing.Size(80, 20);
        this.chkAll.TabIndex = 0;
        //
        // lblHint
        //
        this.lblHint.Location = new System.Drawing.Point(112, 15);
        this.lblHint.Name = "lblHint";
        this.lblHint.Size = new System.Drawing.Size(60, 15);
        this.lblHint.TabIndex = 1;
        this.lblHint.Text = "체크한 품목의 잔량이 발주수량으로 채워집니다.";
        //
        // btnOk
        //
        this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnOk.Location = new System.Drawing.Point(796, 10);
        this.btnOk.Name = "btnOk";
        this.btnOk.Size = new System.Drawing.Size(90, 24);
        this.btnOk.TabIndex = 2;
        this.btnOk.Text = "불러오기";
        //
        // btnClose
        //
        this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnClose.Location = new System.Drawing.Point(894, 10);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new System.Drawing.Size(90, 24);
        this.btnClose.TabIndex = 3;
        this.btnClose.Text = "닫기";
        //
        // popReqPick
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1000, 512);
        this.Controls.Add(this.grd1);
        this.Controls.Add(this.panFooter);
        this.Controls.Add(this.panHeader);
        this.MinimizeBox = false;
        this.Name = "popReqPick";
        this.ShowIcon = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "구매요청 불러오기";
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        this.panHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dteFrom.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteFrom.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteTo.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteTo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtReqNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtKeyword.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkSel)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panFooter)).EndInit();
        this.panFooter.ResumeLayout(false);
        this.panFooter.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.chkAll.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblDate;
    private DateEditWyn dteFrom;
    private DevExpress.XtraEditors.LabelControl lblTilde;
    private DateEditWyn dteTo;
    private DevExpress.XtraEditors.LabelControl lblReqNo;
    private TextEditWyn txtReqNo;
    private DevExpress.XtraEditors.LabelControl lblKeyword;
    private TextEditWyn txtKeyword;
    private ButtonWyn btnSearch;
    private DevExpress.XtraEditors.LabelControl lblCust;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colSel;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkSel;
    private DevExpress.XtraGrid.Columns.GridColumn colReqNo;
    private DevExpress.XtraGrid.Columns.GridColumn colReqDate;
    private DevExpress.XtraGrid.Columns.GridColumn colReqTitle;
    private DevExpress.XtraGrid.Columns.GridColumn colCustNm;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colQty;
    private DevExpress.XtraGrid.Columns.GridColumn colNextQty;
    private DevExpress.XtraGrid.Columns.GridColumn colRemainQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDelvDate;
    private PanelWyn panFooter;
    private DevExpress.XtraEditors.CheckEdit chkAll;
    private DevExpress.XtraEditors.LabelControl lblHint;
    private ButtonWyn btnOk;
    private ButtonWyn btnClose;
}
