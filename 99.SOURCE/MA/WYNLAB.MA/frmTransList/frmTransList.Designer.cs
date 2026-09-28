// 수불현황(frmTransList) - 수불 원장(TMATRANS) 조회 전용 화면(2026-09-25). 입고 +, 출고 -(역거래 포함).
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

public partial class frmTransList
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTransList));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelSplit = new WYNLAB.Base.Controls.PanelWyn();
            this.panelTop = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colTransId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTransDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTransKind = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookupcolKind = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colTransType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookupcolType = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLocNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLotNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSignedQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStockYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkcolStock = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colReversalYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkcolRev = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colCustNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSrcNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRegUserId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRegDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.lblSearchFrom = new DevExpress.XtraEditors.LabelControl();
            this.dteSearchFrom = new WYNLAB.Base.Controls.DateEditWyn();
            this.dteSearchTo = new WYNLAB.Base.Controls.DateEditWyn();
            this.lblSearchKeyword = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchKeyword = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSearchWh = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchWh = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSearchKind = new DevExpress.XtraEditors.LabelControl();
            this.cboSearchKind = new DevExpress.XtraEditors.ComboBoxEdit();
            this.paTitleH = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.paTitle1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelSplit)).BeginInit();
            this.panelSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelTop)).BeginInit();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolKind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkcolStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkcolRev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchWh.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchKind.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).BeginInit();
            this.paTitleH.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).BeginInit();
            this.paTitle1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panBase
            // 
            this.panBase.Appearance.BackColor = System.Drawing.Color.White;
            this.panBase.Appearance.Options.UseBackColor = true;
            this.panBase.Controls.Add(this.panelSplit);
            this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBase.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panBase.Location = new System.Drawing.Point(0, 0);
            this.panBase.Name = "panBase";
            this.panBase.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.panBase.Size = new System.Drawing.Size(1680, 760);
            this.panBase.TabIndex = 6;
            // 
            // panelSplit
            // 
            this.panelSplit.Appearance.BackColor = System.Drawing.Color.White;
            this.panelSplit.Appearance.Options.UseBackColor = true;
            this.panelSplit.Controls.Add(this.panelTop);
            this.panelSplit.Controls.Add(this.panHeader);
            this.panelSplit.Controls.Add(this.paTitleH);
            this.panelSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSplit.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelSplit.Location = new System.Drawing.Point(5, 0);
            this.panelSplit.Name = "panelSplit";
            this.panelSplit.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.panelSplit.Size = new System.Drawing.Size(1670, 755);
            this.panelSplit.TabIndex = 7;
            // 
            // panelTop
            // 
            this.panelTop.Appearance.BackColor = System.Drawing.Color.White;
            this.panelTop.Appearance.Options.UseBackColor = true;
            this.panelTop.Controls.Add(this.grd1);
            this.panelTop.Controls.Add(this.paTitle1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTop.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelTop.Location = new System.Drawing.Point(3, 74);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelTop.Size = new System.Drawing.Size(1664, 681);
            this.panelTop.TabIndex = 0;
            // 
            // grd1
            // 
            this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd1.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grd1.Location = new System.Drawing.Point(0, 35);
            this.grd1.MainView = this.gvw1;
            this.grd1.Name = "grd1";
            this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lookupcolKind,
            this.lookupcolType,
            this.chkcolStock,
            this.chkcolRev});
            this.grd1.Size = new System.Drawing.Size(1664, 646);
            this.grd1.TabIndex = 1;
            this.grd1.UseEmbeddedNavigator = false;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTransId,
            this.colTransDate,
            this.colTransKind,
            this.colTransType,
            this.colItemNo,
            this.colItemNm,
            this.colItemSpec,
            this.colWhNm,
            this.colLocNm,
            this.colLotNo,
            this.colUnitCd,
            this.colQty,
            this.colSignedQty,
            this.colStockYn,
            this.colReversalYn,
            this.colCustNm,
            this.colSrcNo,
            this.colRemark,
            this.colRegUserId,
            this.colRegDt});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightFocusedRow = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // colTransId
            // 
            this.colTransId.Caption = "수불번호";
            this.colTransId.FieldName = "trans_id";
            this.colTransId.Name = "colTransId";
            this.colTransId.OptionsColumn.AllowEdit = false;
            this.colTransId.Visible = true;
            this.colTransId.VisibleIndex = 0;
            this.colTransId.Width = 80;
            // 
            // colTransDate
            // 
            this.colTransDate.Caption = "수불일자";
            this.colTransDate.FieldName = "trans_date";
            this.colTransDate.Name = "colTransDate";
            this.colTransDate.OptionsColumn.AllowEdit = false;
            this.colTransDate.Visible = true;
            this.colTransDate.VisibleIndex = 1;
            this.colTransDate.Width = 90;
            // 
            // colTransKind
            // 
            this.colTransKind.Caption = "구분";
            this.colTransKind.ColumnEdit = this.lookupcolKind;
            this.colTransKind.FieldName = "trans_kind";
            this.colTransKind.Name = "colTransKind";
            this.colTransKind.OptionsColumn.AllowEdit = false;
            this.colTransKind.Visible = true;
            this.colTransKind.VisibleIndex = 2;
            this.colTransKind.Width = 60;
            // 
            // lookupcolKind
            // 
            this.lookupcolKind.AutoHeight = false;
            this.lookupcolKind.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookupcolKind.LookupKey = "L_MA0010";
            this.lookupcolKind.Name = "lookupcolKind";
            this.lookupcolKind.NullText = "";
            this.lookupcolKind.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colTransType
            // 
            this.colTransType.Caption = "수불유형";
            this.colTransType.ColumnEdit = this.lookupcolType;
            this.colTransType.FieldName = "trans_type";
            this.colTransType.Name = "colTransType";
            this.colTransType.OptionsColumn.AllowEdit = false;
            this.colTransType.Visible = true;
            this.colTransType.VisibleIndex = 3;
            this.colTransType.Width = 100;
            // 
            // lookupcolType
            // 
            this.lookupcolType.AutoHeight = false;
            this.lookupcolType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookupcolType.LookupKey = "L_MA0011";
            this.lookupcolType.Name = "lookupcolType";
            this.lookupcolType.NullText = "";
            this.lookupcolType.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colItemNo
            // 
            this.colItemNo.Caption = "품번";
            this.colItemNo.FieldName = "item_no";
            this.colItemNo.Name = "colItemNo";
            this.colItemNo.OptionsColumn.AllowEdit = false;
            this.colItemNo.Visible = true;
            this.colItemNo.VisibleIndex = 4;
            this.colItemNo.Width = 100;
            // 
            // colItemNm
            // 
            this.colItemNm.Caption = "품명";
            this.colItemNm.FieldName = "item_nm";
            this.colItemNm.Name = "colItemNm";
            this.colItemNm.OptionsColumn.AllowEdit = false;
            this.colItemNm.Visible = true;
            this.colItemNm.VisibleIndex = 5;
            this.colItemNm.Width = 140;
            // 
            // colItemSpec
            // 
            this.colItemSpec.Caption = "규격";
            this.colItemSpec.FieldName = "item_spec";
            this.colItemSpec.Name = "colItemSpec";
            this.colItemSpec.OptionsColumn.AllowEdit = false;
            this.colItemSpec.Visible = true;
            this.colItemSpec.VisibleIndex = 6;
            this.colItemSpec.Width = 110;
            // 
            // colWhNm
            // 
            this.colWhNm.Caption = "창고";
            this.colWhNm.FieldName = "wh_nm";
            this.colWhNm.Name = "colWhNm";
            this.colWhNm.OptionsColumn.AllowEdit = false;
            this.colWhNm.Visible = true;
            this.colWhNm.VisibleIndex = 7;
            this.colWhNm.Width = 100;
            // 
            // colLocNm
            // 
            this.colLocNm.Caption = "위치";
            this.colLocNm.FieldName = "loc_nm";
            this.colLocNm.Name = "colLocNm";
            this.colLocNm.OptionsColumn.AllowEdit = false;
            this.colLocNm.Visible = true;
            this.colLocNm.VisibleIndex = 8;
            this.colLocNm.Width = 90;
            // 
            // colLotNo
            // 
            this.colLotNo.Caption = "LOT";
            this.colLotNo.FieldName = "lot_no";
            this.colLotNo.Name = "colLotNo";
            this.colLotNo.OptionsColumn.AllowEdit = false;
            this.colLotNo.Visible = true;
            this.colLotNo.VisibleIndex = 9;
            this.colLotNo.Width = 90;
            // 
            // colUnitCd
            // 
            this.colUnitCd.Caption = "단위";
            this.colUnitCd.FieldName = "unit_cd";
            this.colUnitCd.Name = "colUnitCd";
            this.colUnitCd.OptionsColumn.AllowEdit = false;
            this.colUnitCd.Visible = true;
            this.colUnitCd.VisibleIndex = 10;
            this.colUnitCd.Width = 50;
            // 
            // colQty
            // 
            this.colQty.Caption = "수량";
            this.colQty.DisplayFormat.FormatString = "#,##0.####";
            this.colQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colQty.FieldName = "qty";
            this.colQty.Name = "colQty";
            this.colQty.OptionsColumn.AllowEdit = false;
            this.colQty.Visible = true;
            this.colQty.VisibleIndex = 11;
            this.colQty.Width = 80;
            // 
            // colSignedQty
            // 
            this.colSignedQty.Caption = "증감";
            this.colSignedQty.DisplayFormat.FormatString = "#,##0.####";
            this.colSignedQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colSignedQty.FieldName = "signed_qty";
            this.colSignedQty.Name = "colSignedQty";
            this.colSignedQty.OptionsColumn.AllowEdit = false;
            this.colSignedQty.Visible = true;
            this.colSignedQty.VisibleIndex = 12;
            this.colSignedQty.Width = 80;
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
            this.colStockYn.Width = 65;
            // 
            // chkcolStock
            // 
            this.chkcolStock.AutoHeight = false;
            this.chkcolStock.Name = "chkcolStock";
            this.chkcolStock.ValueChecked = "Y";
            this.chkcolStock.ValueUnchecked = "N";
            // 
            // colReversalYn
            // 
            this.colReversalYn.Caption = "역거래";
            this.colReversalYn.ColumnEdit = this.chkcolRev;
            this.colReversalYn.FieldName = "reversal_yn";
            this.colReversalYn.Name = "colReversalYn";
            this.colReversalYn.OptionsColumn.AllowEdit = false;
            this.colReversalYn.Visible = true;
            this.colReversalYn.VisibleIndex = 14;
            this.colReversalYn.Width = 60;
            // 
            // chkcolRev
            // 
            this.chkcolRev.AutoHeight = false;
            this.chkcolRev.Name = "chkcolRev";
            this.chkcolRev.ValueChecked = "Y";
            this.chkcolRev.ValueUnchecked = "N";
            // 
            // colCustNm
            // 
            this.colCustNm.Caption = "거래처";
            this.colCustNm.FieldName = "cust_nm";
            this.colCustNm.Name = "colCustNm";
            this.colCustNm.OptionsColumn.AllowEdit = false;
            this.colCustNm.Visible = true;
            this.colCustNm.VisibleIndex = 15;
            this.colCustNm.Width = 110;
            // 
            // colSrcNo
            // 
            this.colSrcNo.Caption = "원천번호";
            this.colSrcNo.FieldName = "src_no";
            this.colSrcNo.Name = "colSrcNo";
            this.colSrcNo.OptionsColumn.AllowEdit = false;
            this.colSrcNo.Visible = true;
            this.colSrcNo.VisibleIndex = 16;
            this.colSrcNo.Width = 110;
            // 
            // colRemark
            // 
            this.colRemark.Caption = "비고";
            this.colRemark.FieldName = "remark";
            this.colRemark.Name = "colRemark";
            this.colRemark.OptionsColumn.AllowEdit = false;
            this.colRemark.Visible = true;
            this.colRemark.VisibleIndex = 17;
            this.colRemark.Width = 130;
            // 
            // colRegUserId
            // 
            this.colRegUserId.Caption = "등록자";
            this.colRegUserId.FieldName = "reg_user_id";
            this.colRegUserId.Name = "colRegUserId";
            this.colRegUserId.OptionsColumn.AllowEdit = false;
            this.colRegUserId.Visible = true;
            this.colRegUserId.VisibleIndex = 18;
            this.colRegUserId.Width = 80;
            // 
            // colRegDt
            // 
            this.colRegDt.Caption = "등록일시";
            this.colRegDt.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.colRegDt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colRegDt.FieldName = "reg_dt";
            this.colRegDt.Name = "colRegDt";
            this.colRegDt.OptionsColumn.AllowEdit = false;
            this.colRegDt.Visible = true;
            this.colRegDt.VisibleIndex = 19;
            this.colRegDt.Width = 120;
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.lblSearchFrom);
            this.panHeader.Controls.Add(this.dteSearchFrom);
            this.panHeader.Controls.Add(this.dteSearchTo);
            this.panHeader.Controls.Add(this.lblSearchKeyword);
            this.panHeader.Controls.Add(this.txtSearchKeyword);
            this.panHeader.Controls.Add(this.lblSearchWh);
            this.panHeader.Controls.Add(this.txtSearchWh);
            this.panHeader.Controls.Add(this.lblSearchKind);
            this.panHeader.Controls.Add(this.cboSearchKind);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(3, 25);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1664, 49);
            this.panHeader.TabIndex = 8;
            // 
            // lblSearchFrom
            // 
            this.lblSearchFrom.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblSearchFrom.Appearance.Options.UseFont = true;
            this.lblSearchFrom.Location = new System.Drawing.Point(27, 20);
            this.lblSearchFrom.Name = "lblSearchFrom";
            this.lblSearchFrom.Size = new System.Drawing.Size(48, 15);
            this.lblSearchFrom.TabIndex = 0;
            this.lblSearchFrom.Text = "수불일자";
            // 
            // dteSearchFrom
            // 
            this.dteSearchFrom.EditValue = new System.DateTime(2026, 9, 27, 0, 0, 0, 0);
            this.dteSearchFrom.Location = new System.Drawing.Point(81, 17);
            this.dteSearchFrom.Name = "dteSearchFrom";
            this.dteSearchFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteSearchFrom.Size = new System.Drawing.Size(100, 20);
            this.dteSearchFrom.TabIndex = 1;
            this.dteSearchFrom.YyyyMmDd = "20260927";
            // 
            // dteSearchTo
            // 
            this.dteSearchTo.EditValue = new System.DateTime(2026, 9, 27, 0, 0, 0, 0);
            this.dteSearchTo.Location = new System.Drawing.Point(182, 17);
            this.dteSearchTo.Name = "dteSearchTo";
            this.dteSearchTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteSearchTo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dteSearchTo.Size = new System.Drawing.Size(100, 20);
            this.dteSearchTo.TabIndex = 3;
            this.dteSearchTo.YyyyMmDd = "20260927";
            // 
            // lblSearchKeyword
            // 
            this.lblSearchKeyword.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblSearchKeyword.Appearance.Options.UseFont = true;
            this.lblSearchKeyword.Location = new System.Drawing.Point(319, 20);
            this.lblSearchKeyword.Name = "lblSearchKeyword";
            this.lblSearchKeyword.Size = new System.Drawing.Size(78, 15);
            this.lblSearchKeyword.TabIndex = 4;
            this.lblSearchKeyword.Text = "품번/품명/규격";
            // 
            // txtSearchKeyword
            // 
            this.txtSearchKeyword.Location = new System.Drawing.Point(406, 17);
            this.txtSearchKeyword.Name = "txtSearchKeyword";
            this.txtSearchKeyword.Size = new System.Drawing.Size(189, 20);
            this.txtSearchKeyword.TabIndex = 5;
            // 
            // lblSearchWh
            // 
            this.lblSearchWh.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblSearchWh.Appearance.Options.UseFont = true;
            this.lblSearchWh.Location = new System.Drawing.Point(632, 20);
            this.lblSearchWh.Name = "lblSearchWh";
            this.lblSearchWh.Size = new System.Drawing.Size(24, 15);
            this.lblSearchWh.TabIndex = 6;
            this.lblSearchWh.Text = "창고";
            // 
            // txtSearchWh
            // 
            this.txtSearchWh.Location = new System.Drawing.Point(662, 17);
            this.txtSearchWh.Name = "txtSearchWh";
            this.txtSearchWh.Size = new System.Drawing.Size(100, 20);
            this.txtSearchWh.TabIndex = 7;
            // 
            // lblSearchKind
            // 
            this.lblSearchKind.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblSearchKind.Appearance.Options.UseFont = true;
            this.lblSearchKind.Location = new System.Drawing.Point(799, 20);
            this.lblSearchKind.Name = "lblSearchKind";
            this.lblSearchKind.Size = new System.Drawing.Size(48, 15);
            this.lblSearchKind.TabIndex = 8;
            this.lblSearchKind.Text = "수불구분";
            // 
            // cboSearchKind
            // 
            this.cboSearchKind.Location = new System.Drawing.Point(852, 17);
            this.cboSearchKind.Name = "cboSearchKind";
            this.cboSearchKind.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboSearchKind.Properties.Items.AddRange(new object[] {
            "전체",
            "입고",
            "출고"});
            this.cboSearchKind.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboSearchKind.Size = new System.Drawing.Size(115, 20);
            this.cboSearchKind.TabIndex = 9;
            // 
            // paTitleH
            // 
            this.paTitleH.Appearance.BackColor = System.Drawing.Color.White;
            this.paTitleH.Appearance.Options.UseBackColor = true;
            this.paTitleH.Controls.Add(this.sectionHeaderWyn1);
            this.paTitleH.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitleH.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitleH.Location = new System.Drawing.Point(3, 0);
            this.paTitleH.Name = "paTitleH";
            this.paTitleH.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.paTitleH.Size = new System.Drawing.Size(1664, 25);
            this.paTitleH.TabIndex = 9;
            // 
            // sectionHeaderWyn1
            // 
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1659, 25);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 11;
            this.sectionHeaderWyn1.Text = "수불현황 [frmTransList]";
            // 
            // paTitle1
            // 
            this.paTitle1.Appearance.BackColor = System.Drawing.Color.White;
            this.paTitle1.Appearance.Options.UseBackColor = true;
            this.paTitle1.Controls.Add(this.sectionHeaderWyn4);
            this.paTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle1.Location = new System.Drawing.Point(0, 8);
            this.paTitle1.Name = "paTitle1";
            this.paTitle1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.paTitle1.Size = new System.Drawing.Size(1664, 27);
            this.paTitle1.TabIndex = 13;
            // 
            // sectionHeaderWyn4
            // 
            this.sectionHeaderWyn4.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn4.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn4.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn4.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn4.Name = "sectionHeaderWyn4";
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(1659, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 8;
            this.sectionHeaderWyn4.Text = "수불현황";
            // 
            // frmTransList
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1680, 760);
            this.Controls.Add(this.panBase);
            this.Name = "frmTransList";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelSplit)).EndInit();
            this.panelSplit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelTop)).EndInit();
            this.panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolKind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkcolStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkcolRev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchWh.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchKind.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).EndInit();
            this.paTitleH.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).EndInit();
            this.paTitle1.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private PanelWyn panBase;
    private PanelWyn panelSplit;
    private PanelWyn panelTop;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private LookUpColumnEdit lookupcolKind;
    private LookUpColumnEdit lookupcolType;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkcolStock;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkcolRev;
    private DevExpress.XtraGrid.Columns.GridColumn colTransId;
    private DevExpress.XtraGrid.Columns.GridColumn colTransDate;
    private DevExpress.XtraGrid.Columns.GridColumn colTransKind;
    private DevExpress.XtraGrid.Columns.GridColumn colTransType;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colWhNm;
    private DevExpress.XtraGrid.Columns.GridColumn colLocNm;
    private DevExpress.XtraGrid.Columns.GridColumn colLotNo;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colQty;
    private DevExpress.XtraGrid.Columns.GridColumn colSignedQty;
    private DevExpress.XtraGrid.Columns.GridColumn colStockYn;
    private DevExpress.XtraGrid.Columns.GridColumn colReversalYn;
    private DevExpress.XtraGrid.Columns.GridColumn colCustNm;
    private DevExpress.XtraGrid.Columns.GridColumn colSrcNo;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private DevExpress.XtraGrid.Columns.GridColumn colRegUserId;
    private DevExpress.XtraGrid.Columns.GridColumn colRegDt;
    private DevExpress.XtraEditors.LabelControl lblSearchFrom;
    private DateEditWyn dteSearchFrom;
    private DateEditWyn dteSearchTo;
    private DevExpress.XtraEditors.LabelControl lblSearchKeyword;
    private TextEditWyn txtSearchKeyword;
    private DevExpress.XtraEditors.LabelControl lblSearchWh;
    private TextEditWyn txtSearchWh;
    private DevExpress.XtraEditors.LabelControl lblSearchKind;
    private DevExpress.XtraEditors.ComboBoxEdit cboSearchKind;
    private PanelWyn panHeader;
    private PanelWyn paTitleH;
    private SectionHeaderWyn sectionHeaderWyn1;
    private PanelWyn paTitle1;
    private SectionHeaderWyn sectionHeaderWyn4;
}
