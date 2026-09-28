// ?섏＜?깅줉 - frmPo(援щℓ諛쒖＜?깅줉)? 媛숈? Master-One Sheet ?⑦꽩, 寃??qc_yn)??類먮떎.
// 醫뚰몴??湲곕낯媛믪씠怨?VS ?붿옄?대꼫濡??먯쑀濡?쾶 議곗젙 媛?ν빀?덈떎.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.SA;

public partial class frmSo
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSo));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colSerl = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.popcolItem = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
            this.colItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookupcolUnitCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNextQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRemainQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPrice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAmt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVatRate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotalAmt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colKorPrice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colKorAmt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colKorVat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colKorTotalAmt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDelvDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.datecolDelv = new WYNLAB.Base.Controls.DateColumnEdit();
            this.colWhId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.popcolWh = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
            this.colLocId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.popcolLoc = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
            this.colSrcType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSrcNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStockYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkcolYn = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colStopYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStopRemark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnLoadQt = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnLineStop = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnLineStopCancel = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnDeletRow1 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAddRow1 = new WYNLAB.Base.Controls.ButtonWyn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.lblAccId = new DevExpress.XtraEditors.LabelControl();
            this.cboAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblSoNo = new DevExpress.XtraEditors.LabelControl();
            this.txtSoId = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtSoNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblStatCd = new DevExpress.XtraEditors.LabelControl();
            this.cboApprStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.cboStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblSoDate = new DevExpress.XtraEditors.LabelControl();
            this.dteSoDate = new WYNLAB.Base.Controls.DateEditWyn();
            this.lblDelvDate = new DevExpress.XtraEditors.LabelControl();
            this.dteDelvDate = new WYNLAB.Base.Controls.DateEditWyn();
            this.lblDeptNm = new DevExpress.XtraEditors.LabelControl();
            this.txtDeptNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.txtDeptId = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblEmpNm = new DevExpress.XtraEditors.LabelControl();
            this.txtEmpNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.txtEmpId = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblCustNm = new DevExpress.XtraEditors.LabelControl();
            this.txtCustNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.txtCustId = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblCurCd = new DevExpress.XtraEditors.LabelControl();
            this.cboCurCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblExcRate = new DevExpress.XtraEditors.LabelControl();
            this.txtExcRate = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblVatType = new DevExpress.XtraEditors.LabelControl();
            this.cboVatType = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblVatRate = new DevExpress.XtraEditors.LabelControl();
            this.txtVatRate = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblAppNo = new DevExpress.XtraEditors.LabelControl();
            this.txtAppNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblApprStatCd = new DevExpress.XtraEditors.LabelControl();
            this.lblSoTitle = new DevExpress.XtraEditors.LabelControl();
            this.txtSoTitle = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblRemark = new DevExpress.XtraEditors.LabelControl();
            this.memoRemark = new DevExpress.XtraEditors.MemoEdit();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.lblSearchSoNo = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchSoNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.paTitleH = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
            this.panelWyn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
            this.panelWyn4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolUnitCd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolDelv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolDelv.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolWh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolLoc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkcolYn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSoId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSoNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboApprStatCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSoDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSoDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDelvDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDelvDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCurCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcRate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboVatType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVatRate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAppNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSoTitle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchSoNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).BeginInit();
            this.paTitleH.SuspendLayout();
            this.SuspendLayout();
            //
            // panBase
            //
            this.panBase.Appearance.BackColor = System.Drawing.Color.White;
            this.panBase.Appearance.Options.UseBackColor = true;
            this.panBase.Controls.Add(this.panelWyn3);
            this.panBase.Controls.Add(this.panHeader);
            this.panBase.Controls.Add(this.paTitleH);
            this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBase.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panBase.Location = new System.Drawing.Point(0, 0);
            this.panBase.Name = "panBase";
            this.panBase.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.panBase.Size = new System.Drawing.Size(1400, 800);
            this.panBase.TabIndex = 0;
            //
            // panelWyn3
            //
            this.panelWyn3.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn3.Appearance.Options.UseBackColor = true;
            this.panelWyn3.Controls.Add(this.panelWyn4);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(5, 74);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Size = new System.Drawing.Size(1390, 721);
            this.panelWyn3.TabIndex = 7;
            //
            // panelWyn4
            //
            this.panelWyn4.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn4.Appearance.Options.UseBackColor = true;
            this.panelWyn4.Controls.Add(this.grd1);
            this.panelWyn4.Controls.Add(this.panelWyn7);
            this.panelWyn4.Controls.Add(this.panelWyn1);
            this.panelWyn4.Controls.Add(this.panelWyn5);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(0, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.panelWyn4.Size = new System.Drawing.Size(1390, 721);
            this.panelWyn4.TabIndex = 7;
            //
            // grd1
            //
            this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd1.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grd1.Location = new System.Drawing.Point(3, 234);
            this.grd1.MainView = this.gvw1;
            this.grd1.Name = "grd1";
            this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.popcolItem,
            this.lookupcolUnitCd,
            this.datecolDelv,
            this.popcolWh,
            this.popcolLoc,
            this.chkcolYn});
            this.grd1.Size = new System.Drawing.Size(1384, 487);
            this.grd1.TabIndex = 0;
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
            this.colQty,
            this.colNextQty,
            this.colRemainQty,
            this.colPrice,
            this.colAmt,
            this.colVatRate,
            this.colVat,
            this.colTotalAmt,
            this.colKorPrice,
            this.colKorAmt,
            this.colKorVat,
            this.colKorTotalAmt,
            this.colDelvDate,
            this.colWhId,
            this.colLocId,
            this.colSrcType,
            this.colSrcNo,
            this.colStockYn,
            this.colStopYn,
            this.colStopRemark});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightFocusedRow = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
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
            this.colItemId.Visible = true;
            this.colItemId.VisibleIndex = 1;
            this.colItemId.Width = 70;
            //
            // colItemNo
            //
            this.colItemNo.Caption = "품번";
            this.colItemNo.ColumnEdit = this.popcolItem;
            this.colItemNo.FieldName = "item_no";
            this.colItemNo.Name = "colItemNo";
            this.colItemNo.Visible = true;
            this.colItemNo.VisibleIndex = 2;
            this.colItemNo.Width = 90;
            //
            // popcolItem
            //
            this.popcolItem.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis)});
            this.popcolItem.LookupKey = "P_ITEM";
            this.popcolItem.Name = "popcolItem";
            //
            // colItemNm
            //
            this.colItemNm.Caption = "품명";
            this.colItemNm.FieldName = "item_nm";
            this.colItemNm.Name = "colItemNm";
            this.colItemNm.OptionsColumn.AllowEdit = false;
            this.colItemNm.Visible = true;
            this.colItemNm.VisibleIndex = 3;
            this.colItemNm.Width = 130;
            //
            // colItemSpec
            //
            this.colItemSpec.Caption = "洹쒓꺽";
            this.colItemSpec.FieldName = "item_spec";
            this.colItemSpec.Name = "colItemSpec";
            this.colItemSpec.OptionsColumn.AllowEdit = false;
            this.colItemSpec.Visible = true;
            this.colItemSpec.VisibleIndex = 4;
            this.colItemSpec.Width = 100;
            //
            // colUnitCd
            //
            this.colUnitCd.Caption = "단위";
            this.colUnitCd.ColumnEdit = this.lookupcolUnitCd;
            this.colUnitCd.FieldName = "unit_cd";
            this.colUnitCd.Name = "colUnitCd";
            this.colUnitCd.Visible = true;
            this.colUnitCd.VisibleIndex = 5;
            this.colUnitCd.Width = 60;
            //
            // lookupcolUnitCd
            //
            this.lookupcolUnitCd.AutoHeight = false;
            this.lookupcolUnitCd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookupcolUnitCd.LookupKey = "L_CM0001";
            this.lookupcolUnitCd.Name = "lookupcolUnitCd";
            this.lookupcolUnitCd.NullText = "";
            this.lookupcolUnitCd.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            //
            // colQty
            //
            this.colQty.Caption = "수주수량";
            this.colQty.DisplayFormat.FormatString = "#,##0.####";
            this.colQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colQty.FieldName = "qty";
            this.colQty.Name = "colQty";
            this.colQty.Visible = true;
            this.colQty.VisibleIndex = 6;
            this.colQty.Width = 70;
            //
            // colNextQty
            //
            this.colNextQty.Caption = "진행수량";
            this.colNextQty.DisplayFormat.FormatString = "#,##0.####";
            this.colNextQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colNextQty.FieldName = "next_qty";
            this.colNextQty.Name = "colNextQty";
            this.colNextQty.OptionsColumn.AllowEdit = false;
            this.colNextQty.Visible = true;
            this.colNextQty.VisibleIndex = 7;
            this.colNextQty.Width = 70;
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
            this.colRemainQty.VisibleIndex = 8;
            this.colRemainQty.Width = 70;
            //
            // colPrice
            //
            this.colPrice.Caption = "단가";
            this.colPrice.DisplayFormat.FormatString = "#,##0.####";
            this.colPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colPrice.FieldName = "price";
            this.colPrice.Name = "colPrice";
            this.colPrice.Visible = true;
            this.colPrice.VisibleIndex = 9;
            this.colPrice.Width = 80;
            //
            // colAmt
            //
            this.colAmt.Caption = "공급가액";
            this.colAmt.DisplayFormat.FormatString = "#,##0.####";
            this.colAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colAmt.FieldName = "amt";
            this.colAmt.Name = "colAmt";
            this.colAmt.OptionsColumn.AllowEdit = false;
            this.colAmt.Visible = true;
            this.colAmt.VisibleIndex = 10;
            this.colAmt.Width = 90;
            //
            // colVatRate
            //
            this.colVatRate.Caption = "부가세율";
            this.colVatRate.DisplayFormat.FormatString = "#,##0.####";
            this.colVatRate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colVatRate.FieldName = "vat_rate";
            this.colVatRate.Name = "colVatRate";
            this.colVatRate.Visible = true;
            this.colVatRate.VisibleIndex = 11;
            this.colVatRate.Width = 70;
            //
            // colVat
            //
            this.colVat.Caption = "부가세액";
            this.colVat.DisplayFormat.FormatString = "#,##0.####";
            this.colVat.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colVat.FieldName = "vat";
            this.colVat.Name = "colVat";
            this.colVat.OptionsColumn.AllowEdit = false;
            this.colVat.Visible = true;
            this.colVat.VisibleIndex = 12;
            this.colVat.Width = 80;
            //
            // colTotalAmt
            //
            this.colTotalAmt.Caption = "합계금액";
            this.colTotalAmt.DisplayFormat.FormatString = "#,##0.####";
            this.colTotalAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTotalAmt.FieldName = "total_amt";
            this.colTotalAmt.Name = "colTotalAmt";
            this.colTotalAmt.OptionsColumn.AllowEdit = false;
            this.colTotalAmt.Visible = true;
            this.colTotalAmt.VisibleIndex = 13;
            this.colTotalAmt.Width = 90;
            //
            // colKorPrice
            //
            this.colKorPrice.Caption = "원화단가";
            this.colKorPrice.DisplayFormat.FormatString = "#,##0.####";
            this.colKorPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colKorPrice.FieldName = "kor_price";
            this.colKorPrice.Name = "colKorPrice";
            this.colKorPrice.OptionsColumn.AllowEdit = false;
            this.colKorPrice.Visible = true;
            this.colKorPrice.VisibleIndex = 14;
            this.colKorPrice.Width = 80;
            //
            // colKorAmt
            //
            this.colKorAmt.Caption = "원화공급가액";
            this.colKorAmt.DisplayFormat.FormatString = "#,##0.####";
            this.colKorAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colKorAmt.FieldName = "kor_amt";
            this.colKorAmt.Name = "colKorAmt";
            this.colKorAmt.OptionsColumn.AllowEdit = false;
            this.colKorAmt.Visible = true;
            this.colKorAmt.VisibleIndex = 15;
            this.colKorAmt.Width = 90;
            //
            // colKorVat
            //
            this.colKorVat.Caption = "원화부가세";
            this.colKorVat.DisplayFormat.FormatString = "#,##0.####";
            this.colKorVat.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colKorVat.FieldName = "kor_vat";
            this.colKorVat.Name = "colKorVat";
            this.colKorVat.OptionsColumn.AllowEdit = false;
            this.colKorVat.Visible = true;
            this.colKorVat.VisibleIndex = 16;
            this.colKorVat.Width = 80;
            //
            // colKorTotalAmt
            //
            this.colKorTotalAmt.Caption = "원화합계";
            this.colKorTotalAmt.DisplayFormat.FormatString = "#,##0.####";
            this.colKorTotalAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colKorTotalAmt.FieldName = "kor_total_amt";
            this.colKorTotalAmt.Name = "colKorTotalAmt";
            this.colKorTotalAmt.OptionsColumn.AllowEdit = false;
            this.colKorTotalAmt.Visible = true;
            this.colKorTotalAmt.VisibleIndex = 17;
            this.colKorTotalAmt.Width = 90;
            //
            // colDelvDate
            //
            this.colDelvDate.Caption = "납기일";
            this.colDelvDate.ColumnEdit = this.datecolDelv;
            this.colDelvDate.FieldName = "delv_date";
            this.colDelvDate.Name = "colDelvDate";
            this.colDelvDate.Visible = true;
            this.colDelvDate.VisibleIndex = 18;
            this.colDelvDate.Width = 90;
            //
            // datecolDelv
            //
            this.datecolDelv.AutoHeight = false;
            this.datecolDelv.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.datecolDelv.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.datecolDelv.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.datecolDelv.Name = "datecolDelv";
            //
            // colWhId
            //
            this.colWhId.Caption = "李쎄퀬";
            this.colWhId.ColumnEdit = this.popcolWh;
            this.colWhId.FieldName = "wh_id";
            this.colWhId.Name = "colWhId";
            this.colWhId.Visible = true;
            this.colWhId.VisibleIndex = 19;
            this.colWhId.Width = 80;
            //
            // popcolWh
            //
            this.popcolWh.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis)});
            this.popcolWh.LookupKey = "P_WH";
            this.popcolWh.Name = "popcolWh";
            //
            // colLocId
            //
            this.colLocId.Caption = "위치";
            this.colLocId.ColumnEdit = this.popcolLoc;
            this.colLocId.FieldName = "loc_id";
            this.colLocId.Name = "colLocId";
            this.colLocId.Visible = true;
            this.colLocId.VisibleIndex = 20;
            this.colLocId.Width = 80;
            //
            // popcolLoc
            //
            this.popcolLoc.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis)});
            this.popcolLoc.LookupKey = "P_LOC";
            this.popcolLoc.Name = "popcolLoc";
            //
            // colSrcType
            //
            this.colSrcType.Caption = "원천구분";
            this.colSrcType.FieldName = "src_type";
            this.colSrcType.Name = "colSrcType";
            this.colSrcType.OptionsColumn.AllowEdit = false;
            this.colSrcType.Visible = true;
            this.colSrcType.VisibleIndex = 21;
            this.colSrcType.Width = 90;
            //
            // colSrcNo
            //
            this.colSrcNo.Caption = "원천번호";
            this.colSrcNo.FieldName = "src_no";
            this.colSrcNo.Name = "colSrcNo";
            this.colSrcNo.OptionsColumn.AllowEdit = false;
            this.colSrcNo.Visible = true;
            this.colSrcNo.VisibleIndex = 22;
            this.colSrcNo.Width = 90;
            //
            // colStockYn
            //
            this.colStockYn.Caption = "재고반영";
            this.colStockYn.ColumnEdit = this.chkcolYn;
            this.colStockYn.FieldName = "stock_yn";
            this.colStockYn.Name = "colStockYn";
            this.colStockYn.Visible = true;
            this.colStockYn.VisibleIndex = 23;
            this.colStockYn.Width = 50;
            //
            // chkcolYn
            //
            this.chkcolYn.AutoHeight = false;
            this.chkcolYn.Name = "chkcolYn";
            this.chkcolYn.ValueChecked = "Y";
            this.chkcolYn.ValueUnchecked = "N";
            //
            // colStopYn
            //
            this.colStopYn.Caption = "留덇컧";
            this.colStopYn.ColumnEdit = this.chkcolYn;
            this.colStopYn.FieldName = "stop_yn";
            this.colStopYn.Name = "colStopYn";
            this.colStopYn.OptionsColumn.AllowEdit = false;
            this.colStopYn.Visible = true;
            this.colStopYn.VisibleIndex = 24;
            this.colStopYn.Width = 50;
            //
            // colStopRemark
            //
            this.colStopRemark.Caption = "마감사유";
            this.colStopRemark.FieldName = "stop_remark";
            this.colStopRemark.Name = "colStopRemark";
            this.colStopRemark.OptionsColumn.AllowEdit = false;
            this.colStopRemark.Visible = true;
            this.colStopRemark.VisibleIndex = 25;
            this.colStopRemark.Width = 160;
            //
            // panelWyn7
            //
            this.panelWyn7.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn7.Appearance.Options.UseBackColor = true;
            this.panelWyn7.Controls.Add(this.btnLoadQt);
            this.panelWyn7.Controls.Add(this.btnLineStop);
            this.panelWyn7.Controls.Add(this.btnLineStopCancel);
            this.panelWyn7.Controls.Add(this.btnDeletRow1);
            this.panelWyn7.Controls.Add(this.btnAddRow1);
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(3, 204);
            this.panelWyn7.Name = "panelWyn7";
            this.panelWyn7.Padding = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.panelWyn7.Size = new System.Drawing.Size(1384, 30);
            this.panelWyn7.TabIndex = 9;
            //
            // btnLoadQt
            //
            this.btnLoadQt.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadQt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnLoadQt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLoadQt.FillColor = System.Drawing.Color.White;
            this.btnLoadQt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnLoadQt.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnLoadQt.Image = null;
            this.btnLoadQt.Location = new System.Drawing.Point(130, 4);
            this.btnLoadQt.Name = "btnLoadQt";
            this.btnLoadQt.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnLoadQt.Size = new System.Drawing.Size(100, 22);
            this.btnLoadQt.TabIndex = 0;
            this.btnLoadQt.Text = "견적 불러오기";
            this.btnLoadQt.ToolTip = "확정된 견적의 수량을 수주 품목으로 불러옵니다.";
            //
            // btnLineStop
            //
            this.btnLineStop.BackColor = System.Drawing.Color.Transparent;
            this.btnLineStop.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnLineStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLineStop.FillColor = System.Drawing.Color.White;
            this.btnLineStop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnLineStop.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnLineStop.Image = null;
            this.btnLineStop.Location = new System.Drawing.Point(236, 4);
            this.btnLineStop.Name = "btnLineStop";
            this.btnLineStop.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnLineStop.Size = new System.Drawing.Size(84, 22);
            this.btnLineStop.TabIndex = 0;
            this.btnLineStop.Text = "라인마감";
            this.btnLineStop.ToolTip = "선택한 수주 품목의 수량을 남아서 종결합니다(승인 완료된 수주만).";
            //
            // btnLineStopCancel
            //
            this.btnLineStopCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnLineStopCancel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnLineStopCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLineStopCancel.FillColor = System.Drawing.Color.White;
            this.btnLineStopCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnLineStopCancel.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnLineStopCancel.Image = null;
            this.btnLineStopCancel.Location = new System.Drawing.Point(324, 4);
            this.btnLineStopCancel.Name = "btnLineStopCancel";
            this.btnLineStopCancel.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnLineStopCancel.Size = new System.Drawing.Size(84, 22);
            this.btnLineStopCancel.TabIndex = 0;
            this.btnLineStopCancel.Text = "留덇컧痍⑥냼";
            this.btnLineStopCancel.ToolTip = "선택한 수주 품목의 마감을 취소합니다.";
            //
            // btnDeletRow1
            //
            this.btnDeletRow1.BackColor = System.Drawing.Color.Transparent;
            this.btnDeletRow1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(198)))), ((int)(((byte)(193)))));
            this.btnDeletRow1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeletRow1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(236)))), ((int)(((byte)(234)))));
            this.btnDeletRow1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDeletRow1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDeletRow1.Image = null;
            this.btnDeletRow1.Location = new System.Drawing.Point(65, 4);
            this.btnDeletRow1.Name = "btnDeletRow1";
            this.btnDeletRow1.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDeletRow1.Size = new System.Drawing.Size(58, 22);
            this.btnDeletRow1.TabIndex = 0;
            this.btnDeletRow1.Text = "행삭제";
            this.btnDeletRow1.ToolTip = "행삭제";
            //
            // btnAddRow1
            //
            this.btnAddRow1.BackColor = System.Drawing.Color.Transparent;
            this.btnAddRow1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(231)))), ((int)(((byte)(203)))));
            this.btnAddRow1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddRow1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(245)))), ((int)(((byte)(231)))));
            this.btnAddRow1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnAddRow1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnAddRow1.Image = null;
            this.btnAddRow1.Location = new System.Drawing.Point(3, 4);
            this.btnAddRow1.Name = "btnAddRow1";
            this.btnAddRow1.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnAddRow1.Size = new System.Drawing.Size(58, 22);
            this.btnAddRow1.TabIndex = 0;
            this.btnAddRow1.Text = "행추가";
            this.btnAddRow1.ToolTip = "행추가";
            //
            // panelWyn1
            //
            this.panelWyn1.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn1.Appearance.Options.UseBackColor = true;
            this.panelWyn1.Controls.Add(this.sectionHeaderWyn4);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(3, 164);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn1.Size = new System.Drawing.Size(1384, 40);
            this.panelWyn1.TabIndex = 8;
            //
            // sectionHeaderWyn4
            //
            this.sectionHeaderWyn4.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn4.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn4.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn4.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn4.Name = "sectionHeaderWyn4";
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(1379, 38);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 10;
            this.sectionHeaderWyn4.Text = "수주 품목정보";
            //
            // panelWyn5
            //
            this.panelWyn5.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn5.Appearance.Options.UseBackColor = true;
            this.panelWyn5.Controls.Add(this.panData);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(3, 0);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Size = new System.Drawing.Size(1384, 164);
            this.panelWyn5.TabIndex = 6;
            //
            // panData
            //
            this.panData.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panData.Appearance.Options.UseBackColor = true;
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Controls.Add(this.lblAccId);
            this.panData.Controls.Add(this.cboAccId);
            this.panData.Controls.Add(this.lblSoNo);
            this.panData.Controls.Add(this.txtSoId);
            this.panData.Controls.Add(this.txtSoNo);
            this.panData.Controls.Add(this.lblStatCd);
            this.panData.Controls.Add(this.cboApprStatCd);
            this.panData.Controls.Add(this.cboStatCd);
            this.panData.Controls.Add(this.lblSoDate);
            this.panData.Controls.Add(this.dteSoDate);
            this.panData.Controls.Add(this.lblDelvDate);
            this.panData.Controls.Add(this.dteDelvDate);
            this.panData.Controls.Add(this.lblDeptNm);
            this.panData.Controls.Add(this.txtDeptNm);
            this.panData.Controls.Add(this.txtDeptId);
            this.panData.Controls.Add(this.lblEmpNm);
            this.panData.Controls.Add(this.txtEmpNm);
            this.panData.Controls.Add(this.txtEmpId);
            this.panData.Controls.Add(this.lblCustNm);
            this.panData.Controls.Add(this.txtCustNm);
            this.panData.Controls.Add(this.txtCustId);
            this.panData.Controls.Add(this.lblCurCd);
            this.panData.Controls.Add(this.cboCurCd);
            this.panData.Controls.Add(this.lblExcRate);
            this.panData.Controls.Add(this.txtExcRate);
            this.panData.Controls.Add(this.lblVatType);
            this.panData.Controls.Add(this.cboVatType);
            this.panData.Controls.Add(this.lblVatRate);
            this.panData.Controls.Add(this.txtVatRate);
            this.panData.Controls.Add(this.lblAppNo);
            this.panData.Controls.Add(this.txtAppNo);
            this.panData.Controls.Add(this.lblApprStatCd);
            this.panData.Controls.Add(this.lblSoTitle);
            this.panData.Controls.Add(this.txtSoTitle);
            this.panData.Controls.Add(this.lblRemark);
            this.panData.Controls.Add(this.memoRemark);
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 0);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(1384, 164);
            this.panData.TabIndex = 8;
            //
            // lblAccId
            //
            this.lblAccId.Location = new System.Drawing.Point(27, 14);
            this.lblAccId.Name = "lblAccId";
            this.lblAccId.Size = new System.Drawing.Size(36, 14);
            this.lblAccId.TabIndex = 0;
            this.lblAccId.Text = "사업장";
            //
            // cboAccId
            //
            this.cboAccId.EditValue = "";
            this.cboAccId.Location = new System.Drawing.Point(60, 11);
            this.cboAccId.LookupKey = "L_ACC";
            this.cboAccId.Name = "cboAccId";
            this.cboAccId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboAccId.Properties.NullText = "";
            this.cboAccId.Required = true;
            this.cboAccId.Size = new System.Drawing.Size(124, 20);
            this.cboAccId.TabIndex = 0;
            //
            // lblSoNo
            //
            this.lblSoNo.Location = new System.Drawing.Point(247, 14);
            this.lblSoNo.Name = "lblSoNo";
            this.lblSoNo.Size = new System.Drawing.Size(48, 14);
            this.lblSoNo.TabIndex = 1;
            this.lblSoNo.Text = "수주번호";
            //
            // txtSoId
            //
            this.txtSoId.Location = new System.Drawing.Point(1200, 130);
            this.txtSoId.Name = "txtSoId";
            this.txtSoId.Size = new System.Drawing.Size(47, 20);
            this.txtSoId.TabIndex = 2;
            this.txtSoId.Visible = false;
            //
            // txtSoNo
            //
            this.txtSoNo.Location = new System.Drawing.Point(302, 11);
            this.txtSoNo.Name = "txtSoNo";
            this.txtSoNo.Properties.ReadOnly = true;
            this.txtSoNo.Size = new System.Drawing.Size(150, 20);
            this.txtSoNo.TabIndex = 2;
            //
            // lblStatCd
            //
            this.lblStatCd.Location = new System.Drawing.Point(500, 14);
            this.lblStatCd.Name = "lblStatCd";
            this.lblStatCd.Size = new System.Drawing.Size(48, 14);
            this.lblStatCd.TabIndex = 3;
            this.lblStatCd.Text = "진행상태";
            //
            // cboApprStatCd
            //
            this.cboApprStatCd.EditValue = "";
            this.cboApprStatCd.Location = new System.Drawing.Point(555, 38);
            this.cboApprStatCd.LookupKey = "L_AP0001";
            this.cboApprStatCd.Name = "cboApprStatCd";
            this.cboApprStatCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboApprStatCd.Properties.NullText = "";
            this.cboApprStatCd.Properties.ReadOnly = true;
            this.cboApprStatCd.Size = new System.Drawing.Size(108, 20);
            this.cboApprStatCd.TabIndex = 4;
            //
            // cboStatCd
            //
            this.cboStatCd.EditValue = "";
            this.cboStatCd.Location = new System.Drawing.Point(555, 11);
            this.cboStatCd.LookupKey = "L_MA0002";
            this.cboStatCd.Name = "cboStatCd";
            this.cboStatCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboStatCd.Properties.NullText = "";
            this.cboStatCd.Properties.ReadOnly = true;
            this.cboStatCd.Size = new System.Drawing.Size(108, 20);
            this.cboStatCd.TabIndex = 5;
            //
            // lblSoDate
            //
            this.lblSoDate.Location = new System.Drawing.Point(27, 67);
            this.lblSoDate.Name = "lblSoDate";
            this.lblSoDate.Size = new System.Drawing.Size(36, 14);
            this.lblSoDate.TabIndex = 6;
            this.lblSoDate.Text = "수주일자";
            //
            // dteSoDate
            //
            this.dteSoDate.Location = new System.Drawing.Point(60, 64);
            this.dteSoDate.Name = "dteSoDate";
            this.dteSoDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteSoDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dteSoDate.Required = true;
            this.dteSoDate.Size = new System.Drawing.Size(124, 20);
            this.dteSoDate.TabIndex = 7;
            //
            // lblDelvDate
            //
            this.lblDelvDate.Location = new System.Drawing.Point(193, 67);
            this.lblDelvDate.Name = "lblDelvDate";
            this.lblDelvDate.Size = new System.Drawing.Size(36, 14);
            this.lblDelvDate.TabIndex = 8;
            this.lblDelvDate.Text = "납기일자";
            //
            // dteDelvDate
            //
            this.dteDelvDate.Location = new System.Drawing.Point(234, 64);
            this.dteDelvDate.Name = "dteDelvDate";
            this.dteDelvDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteDelvDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dteDelvDate.Size = new System.Drawing.Size(124, 20);
            this.dteDelvDate.TabIndex = 9;
            //
            // lblDeptNm
            //
            this.lblDeptNm.Location = new System.Drawing.Point(39, 41);
            this.lblDeptNm.Name = "lblDeptNm";
            this.lblDeptNm.Size = new System.Drawing.Size(24, 14);
            this.lblDeptNm.TabIndex = 10;
            this.lblDeptNm.Text = "부서";
            //
            // txtDeptNm
            //
            this.txtDeptNm.Location = new System.Drawing.Point(60, 38);
            this.txtDeptNm.LookupKey = "P_DEPT";
            this.txtDeptNm.MatchField = "dept_nm";
            this.txtDeptNm.Name = "txtDeptNm";
            this.txtDeptNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis)});
            this.txtDeptNm.Required = true;
            this.txtDeptNm.Size = new System.Drawing.Size(124, 20);
            this.txtDeptNm.TabIndex = 11;
            //
            // txtDeptId
            //
            this.txtDeptId.Location = new System.Drawing.Point(1200, 156);
            this.txtDeptId.Name = "txtDeptId";
            this.txtDeptId.Size = new System.Drawing.Size(135, 20);
            this.txtDeptId.TabIndex = 12;
            this.txtDeptId.Visible = false;
            //
            // lblEmpNm
            //
            this.lblEmpNm.Location = new System.Drawing.Point(193, 41);
            this.lblEmpNm.Name = "lblEmpNm";
            this.lblEmpNm.Size = new System.Drawing.Size(36, 14);
            this.lblEmpNm.TabIndex = 13;
            this.lblEmpNm.Text = "담당자";
            //
            // txtEmpNm
            //
            this.txtEmpNm.Location = new System.Drawing.Point(234, 38);
            this.txtEmpNm.LookupKey = "P_EMP";
            this.txtEmpNm.MatchField = "emp_nm";
            this.txtEmpNm.Name = "txtEmpNm";
            this.txtEmpNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis)});
            this.txtEmpNm.Required = true;
            this.txtEmpNm.Size = new System.Drawing.Size(90, 20);
            this.txtEmpNm.TabIndex = 14;
            //
            // txtEmpId
            //
            this.txtEmpId.Location = new System.Drawing.Point(1200, 182);
            this.txtEmpId.Name = "txtEmpId";
            this.txtEmpId.Size = new System.Drawing.Size(115, 20);
            this.txtEmpId.TabIndex = 15;
            this.txtEmpId.Visible = false;
            //
            // lblCustNm
            //
            this.lblCustNm.Location = new System.Drawing.Point(335, 41);
            this.lblCustNm.Name = "lblCustNm";
            this.lblCustNm.Size = new System.Drawing.Size(36, 14);
            this.lblCustNm.TabIndex = 16;
            this.lblCustNm.Text = "거래처";
            //
            // txtCustNm
            //
            this.txtCustNm.Location = new System.Drawing.Point(377, 38);
            this.txtCustNm.LookupKey = "P_CUST";
            this.txtCustNm.MatchField = "cust_nm";
            this.txtCustNm.Name = "txtCustNm";
            this.txtCustNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis)});
            this.txtCustNm.Required = true;
            this.txtCustNm.Size = new System.Drawing.Size(160, 20);
            this.txtCustNm.TabIndex = 17;
            //
            // txtCustId
            //
            this.txtCustId.Location = new System.Drawing.Point(1200, 208);
            this.txtCustId.Name = "txtCustId";
            this.txtCustId.Size = new System.Drawing.Size(120, 20);
            this.txtCustId.TabIndex = 18;
            this.txtCustId.Visible = false;
            //
            // lblCurCd
            //
            this.lblCurCd.Location = new System.Drawing.Point(710, 14);
            this.lblCurCd.Name = "lblCurCd";
            this.lblCurCd.Size = new System.Drawing.Size(24, 14);
            this.lblCurCd.TabIndex = 19;
            this.lblCurCd.Text = "통화";
            //
            // cboCurCd
            //
            this.cboCurCd.EditValue = "";
            this.cboCurCd.Location = new System.Drawing.Point(716, 11);
            this.cboCurCd.LookupKey = "L_CM0003";
            this.cboCurCd.Name = "cboCurCd";
            this.cboCurCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboCurCd.Properties.NullText = "";
            this.cboCurCd.Required = true;
            this.cboCurCd.Size = new System.Drawing.Size(80, 20);
            this.cboCurCd.TabIndex = 20;
            //
            // lblExcRate
            //
            this.lblExcRate.Location = new System.Drawing.Point(710, 41);
            this.lblExcRate.Name = "lblExcRate";
            this.lblExcRate.Size = new System.Drawing.Size(24, 14);
            this.lblExcRate.TabIndex = 21;
            this.lblExcRate.Text = "환율";
            //
            // txtExcRate
            //
            this.txtExcRate.Location = new System.Drawing.Point(716, 38);
            this.txtExcRate.Name = "txtExcRate";
            this.txtExcRate.Size = new System.Drawing.Size(80, 20);
            this.txtExcRate.TabIndex = 22;
            //
            // lblVatType
            //
            this.lblVatType.Location = new System.Drawing.Point(686, 67);
            this.lblVatType.Name = "lblVatType";
            this.lblVatType.Size = new System.Drawing.Size(48, 14);
            this.lblVatType.TabIndex = 23;
            this.lblVatType.Text = "怨쇱꽭援щ텇";
            //
            // cboVatType
            //
            this.cboVatType.EditValue = "";
            this.cboVatType.Location = new System.Drawing.Point(740, 64);
            this.cboVatType.LookupKey = "L_CM0004";
            this.cboVatType.Name = "cboVatType";
            this.cboVatType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboVatType.Properties.NullText = "";
            this.cboVatType.Size = new System.Drawing.Size(80, 20);
            this.cboVatType.TabIndex = 24;
            //
            // lblVatRate
            //
            this.lblVatRate.Location = new System.Drawing.Point(830, 67);
            this.lblVatRate.Name = "lblVatRate";
            this.lblVatRate.Size = new System.Drawing.Size(48, 14);
            this.lblVatRate.TabIndex = 25;
            this.lblVatRate.Text = "부가세율";
            //
            // txtVatRate
            //
            this.txtVatRate.Location = new System.Drawing.Point(885, 64);
            this.txtVatRate.Name = "txtVatRate";
            this.txtVatRate.Size = new System.Drawing.Size(60, 20);
            this.txtVatRate.TabIndex = 26;
            //
            // lblAppNo
            //
            this.lblAppNo.Location = new System.Drawing.Point(973, 14);
            this.lblAppNo.Name = "lblAppNo";
            this.lblAppNo.Size = new System.Drawing.Size(48, 14);
            this.lblAppNo.TabIndex = 27;
            this.lblAppNo.Text = "寃곗옱踰덊샇";
            //
            // txtAppNo
            //
            this.txtAppNo.Location = new System.Drawing.Point(1027, 11);
            this.txtAppNo.Name = "txtAppNo";
            this.txtAppNo.Properties.ReadOnly = true;
            this.txtAppNo.Size = new System.Drawing.Size(120, 20);
            this.txtAppNo.TabIndex = 28;
            //
            // lblApprStatCd
            //
            this.lblApprStatCd.Location = new System.Drawing.Point(500, 41);
            this.lblApprStatCd.Name = "lblApprStatCd";
            this.lblApprStatCd.Size = new System.Drawing.Size(48, 14);
            this.lblApprStatCd.TabIndex = 29;
            this.lblApprStatCd.Text = "결재상태";
            //
            // lblSoTitle
            //
            this.lblSoTitle.Location = new System.Drawing.Point(27, 93);
            this.lblSoTitle.Name = "lblSoTitle";
            this.lblSoTitle.Size = new System.Drawing.Size(36, 14);
            this.lblSoTitle.TabIndex = 30;
            this.lblSoTitle.Text = "嫄대챸";
            //
            // txtSoTitle
            //
            this.txtSoTitle.Location = new System.Drawing.Point(60, 90);
            this.txtSoTitle.Name = "txtSoTitle";
            this.txtSoTitle.Size = new System.Drawing.Size(600, 20);
            this.txtSoTitle.TabIndex = 31;
            //
            // lblRemark
            //
            this.lblRemark.Location = new System.Drawing.Point(39, 119);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(24, 14);
            this.lblRemark.TabIndex = 32;
            this.lblRemark.Text = "鍮꾧퀬";
            //
            // memoRemark
            //
            this.memoRemark.Location = new System.Drawing.Point(60, 116);
            this.memoRemark.Name = "memoRemark";
            this.memoRemark.Size = new System.Drawing.Size(600, 40);
            this.memoRemark.TabIndex = 33;
            //
            // panHeader
            //
            this.panHeader.Appearance.BackColor = System.Drawing.Color.White;
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.Controls.Add(this.lblSearchSoNo);
            this.panHeader.Controls.Add(this.txtSearchSoNo);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 25);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1390, 49);
            this.panHeader.TabIndex = 8;
            //
            // lblSearchSoNo
            //
            this.lblSearchSoNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblSearchSoNo.Appearance.Options.UseFont = true;
            this.lblSearchSoNo.Location = new System.Drawing.Point(19, 20);
            this.lblSearchSoNo.Name = "lblSearchSoNo";
            this.lblSearchSoNo.Size = new System.Drawing.Size(48, 15);
            this.lblSearchSoNo.TabIndex = 0;
            this.lblSearchSoNo.Text = "수주번호";
            //
            // txtSearchSoNo
            //
            this.txtSearchSoNo.Location = new System.Drawing.Point(75, 17);
            this.txtSearchSoNo.Name = "txtSearchSoNo";
            this.txtSearchSoNo.Size = new System.Drawing.Size(187, 20);
            this.txtSearchSoNo.TabIndex = 1;
            //
            // paTitleH
            //
            this.paTitleH.Appearance.BackColor = System.Drawing.Color.White;
            this.paTitleH.Appearance.Options.UseBackColor = true;
            this.paTitleH.Controls.Add(this.sectionHeaderWyn1);
            this.paTitleH.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitleH.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitleH.Location = new System.Drawing.Point(5, 0);
            this.paTitleH.Name = "paTitleH";
            this.paTitleH.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.paTitleH.Size = new System.Drawing.Size(1390, 25);
            this.paTitleH.TabIndex = 10;
            //
            // sectionHeaderWyn1
            //
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1385, 25);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 14;
            this.sectionHeaderWyn1.Text = "수주등록 [frmSo]";
            //
            // frmSo
            //
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Controls.Add(this.panBase);
            this.Name = "frmSo";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolUnitCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolDelv.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolDelv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolWh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolLoc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkcolYn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
            this.panelWyn7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
            this.panData.ResumeLayout(false);
            this.panData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSoId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSoNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboApprStatCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSoDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSoDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDelvDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDelvDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCurCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcRate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboVatType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVatRate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAppNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSoTitle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchSoNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).EndInit();
            this.paTitleH.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private PanelWyn panBase;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn4;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colItemId;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNo;
    private PopupLookupColumnEdit popcolItem;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitCd;
    private LookUpColumnEdit lookupcolUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colQty;
    private DevExpress.XtraGrid.Columns.GridColumn colNextQty;
    private DevExpress.XtraGrid.Columns.GridColumn colRemainQty;
    private DevExpress.XtraGrid.Columns.GridColumn colPrice;
    private DevExpress.XtraGrid.Columns.GridColumn colAmt;
    private DevExpress.XtraGrid.Columns.GridColumn colVatRate;
    private DevExpress.XtraGrid.Columns.GridColumn colVat;
    private DevExpress.XtraGrid.Columns.GridColumn colTotalAmt;
    private DevExpress.XtraGrid.Columns.GridColumn colKorPrice;
    private DevExpress.XtraGrid.Columns.GridColumn colKorAmt;
    private DevExpress.XtraGrid.Columns.GridColumn colKorVat;
    private DevExpress.XtraGrid.Columns.GridColumn colKorTotalAmt;
    private DevExpress.XtraGrid.Columns.GridColumn colDelvDate;
    private DateColumnEdit datecolDelv;
    private DevExpress.XtraGrid.Columns.GridColumn colWhId;
    private PopupLookupColumnEdit popcolWh;
    private DevExpress.XtraGrid.Columns.GridColumn colLocId;
    private PopupLookupColumnEdit popcolLoc;
    private DevExpress.XtraGrid.Columns.GridColumn colSrcType;
    private DevExpress.XtraGrid.Columns.GridColumn colSrcNo;
    private DevExpress.XtraGrid.Columns.GridColumn colStockYn;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkcolYn;
    private DevExpress.XtraGrid.Columns.GridColumn colStopYn;
    private DevExpress.XtraGrid.Columns.GridColumn colStopRemark;
    private PanelWyn panelWyn7;
    private ButtonWyn btnLoadQt;
    private ButtonWyn btnLineStop;
    private ButtonWyn btnLineStopCancel;
    private ButtonWyn btnDeletRow1;
    private ButtonWyn btnAddRow1;
    private PanelWyn panelWyn1;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblAccId;
    private LookUpEditWyn cboAccId;
    private DevExpress.XtraEditors.LabelControl lblSoNo;
    private TextEditWyn txtSoId;
    private TextEditWyn txtSoNo;
    private DevExpress.XtraEditors.LabelControl lblStatCd;
    private LookUpEditWyn cboApprStatCd;
    private LookUpEditWyn cboStatCd;
    private DevExpress.XtraEditors.LabelControl lblSoDate;
    private DateEditWyn dteSoDate;
    private DevExpress.XtraEditors.LabelControl lblDelvDate;
    private DateEditWyn dteDelvDate;
    private DevExpress.XtraEditors.LabelControl lblDeptNm;
    private PopupLookupEditWyn txtDeptNm;
    private TextEditWyn txtDeptId;
    private DevExpress.XtraEditors.LabelControl lblEmpNm;
    private PopupLookupEditWyn txtEmpNm;
    private TextEditWyn txtEmpId;
    private DevExpress.XtraEditors.LabelControl lblCustNm;
    private PopupLookupEditWyn txtCustNm;
    private TextEditWyn txtCustId;
    private DevExpress.XtraEditors.LabelControl lblCurCd;
    private LookUpEditWyn cboCurCd;
    private DevExpress.XtraEditors.LabelControl lblExcRate;
    private TextEditWyn txtExcRate;
    private DevExpress.XtraEditors.LabelControl lblVatType;
    private LookUpEditWyn cboVatType;
    private DevExpress.XtraEditors.LabelControl lblVatRate;
    private TextEditWyn txtVatRate;
    private DevExpress.XtraEditors.LabelControl lblAppNo;
    private TextEditWyn txtAppNo;
    private DevExpress.XtraEditors.LabelControl lblApprStatCd;
    private DevExpress.XtraEditors.LabelControl lblSoTitle;
    private TextEditWyn txtSoTitle;
    private DevExpress.XtraEditors.LabelControl lblRemark;
    private DevExpress.XtraEditors.MemoEdit memoRemark;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchSoNo;
    private TextEditWyn txtSearchSoNo;
    private PanelWyn paTitleH;
    private SectionHeaderWyn sectionHeaderWyn1;
}
