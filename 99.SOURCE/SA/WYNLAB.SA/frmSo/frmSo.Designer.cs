// 수주등록 - frmPo(구매발주등록)와 같은 Master-One Sheet 패턴, 검수(qc_yn)는 뺐다.
// 좌표는 기본값이고 VS 디자이너로 자유롭게 조정 가능합니다.
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
            this.colWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLocId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.popcolLoc = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSrcType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSrcNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStockYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkcolYn = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colStopYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStopRemark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnLoadQt = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnPickItem = new WYNLAB.Base.Controls.ButtonWyn();
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
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
            this.txtVatRate = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblAppNo = new DevExpress.XtraEditors.LabelControl();
            this.txtAppNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblApprStatCd = new DevExpress.XtraEditors.LabelControl();
            this.lblSoTitle = new DevExpress.XtraEditors.LabelControl();
            this.txtSoTitle = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblRemark = new DevExpress.XtraEditors.LabelControl();
            this.memoRemark = new DevExpress.XtraEditors.MemoEdit();
            this.panBtn = new WYNLAB.Base.Controls.PanelWyn();
            this.btnOpenApproval = new WYNLAB.Base.Controls.ButtonWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
            this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
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
            ((System.ComponentModel.ISupportInitialize)(this.panBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchSoNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).BeginInit();
            this.paTitleH.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
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
            this.panelWyn3.Controls.Add(this.panBtn);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(5, 53);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Size = new System.Drawing.Size(1390, 742);
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
            this.panelWyn4.Location = new System.Drawing.Point(0, 30);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.panelWyn4.Size = new System.Drawing.Size(1390, 712);
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
            this.grd1.Location = new System.Drawing.Point(3, 284);
            this.grd1.MainView = this.gvw1;
            this.grd1.Name = "grd1";
            this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.popcolItem,
            this.lookupcolUnitCd,
            this.datecolDelv,
            this.popcolWh,
            this.popcolLoc,
            this.chkcolYn});
            this.grd1.Size = new System.Drawing.Size(1384, 428);
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
            this.colWhNm,
            this.colLocId,
            this.gridColumn2,
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
            new DevExpress.XtraEditors.Controls.EditorButton()});
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
            this.colItemSpec.Caption = "규격";
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
            this.colWhId.Caption = "창고ID";
            this.colWhId.ColumnEdit = this.popcolWh;
            this.colWhId.FieldName = "wh_id";
            this.colWhId.Name = "colWhId";
            this.colWhId.Width = 80;
            // 
            // popcolWh
            // 
            this.popcolWh.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.popcolWh.LookupKey = "P_WH";
            this.popcolWh.Name = "popcolWh";
            // 
            // colWhNm
            // 
            this.colWhNm.Caption = "창고";
            this.colWhNm.Name = "colWhNm";
            this.colWhNm.Visible = true;
            this.colWhNm.VisibleIndex = 19;
            // 
            // colLocId
            // 
            this.colLocId.Caption = "LocationID";
            this.colLocId.ColumnEdit = this.popcolLoc;
            this.colLocId.FieldName = "loc_id";
            this.colLocId.Name = "colLocId";
            this.colLocId.Width = 80;
            // 
            // popcolLoc
            // 
            this.popcolLoc.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.popcolLoc.LookupKey = "P_LOC";
            this.popcolLoc.Name = "popcolLoc";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Location";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 20;
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
            this.colStopYn.Caption = "중단";
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
            this.panelWyn7.Controls.Add(this.btnPickItem);
            this.panelWyn7.Controls.Add(this.btnLoadQt);
            this.panelWyn7.Controls.Add(this.btnLineStop);
            this.panelWyn7.Controls.Add(this.btnLineStopCancel);
            this.panelWyn7.Controls.Add(this.btnDeletRow1);
            this.panelWyn7.Controls.Add(this.btnAddRow1);
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(3, 254);
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
            this.btnLoadQt.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnLoadQt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnLoadQt.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnLoadQt.Image = null;
            this.btnLoadQt.Location = new System.Drawing.Point(130, 4);
            this.btnLoadQt.Name = "btnLoadQt";
            this.btnLoadQt.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnLoadQt.Size = new System.Drawing.Size(100, 24);
            this.btnLoadQt.TabIndex = 0;
            this.btnLoadQt.Text = "견적 불러오기";
            this.btnLoadQt.ToolTip = "확정된 견적의 수량을 수주 품목으로 불러옵니다.";
            //
            // btnPickItem (품목 여러 건 선택 - 공통 팝업 다중선택 모드, 고른 만큼 행 추가)
            //
            this.btnPickItem.BackColor = System.Drawing.Color.Transparent;
            this.btnPickItem.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnPickItem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPickItem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnPickItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnPickItem.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnPickItem.Image = null;
            this.btnPickItem.Location = new System.Drawing.Point(234, 4);
            this.btnPickItem.Name = "btnPickItem";
            this.btnPickItem.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnPickItem.Size = new System.Drawing.Size(80, 24);
            this.btnPickItem.TabIndex = 0;
            this.btnPickItem.Text = "품목선택";
            this.btnPickItem.ToolTip = "품목 팝업에서 여러 건을 체크해 한 번에 추가합니다. 이미 담은 품목도 다시 고를 수 있습니다.";
            // 
            // btnLineStop
            // 
            this.btnLineStop.BackColor = System.Drawing.Color.Transparent;
            this.btnLineStop.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnLineStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLineStop.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnLineStop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnLineStop.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnLineStop.Image = null;
            this.btnLineStop.Location = new System.Drawing.Point(318, 4);
            this.btnLineStop.Name = "btnLineStop";
            this.btnLineStop.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnLineStop.Size = new System.Drawing.Size(109, 24);
            this.btnLineStop.TabIndex = 0;
            this.btnLineStop.Text = "수주품목 중단처리";
            this.btnLineStop.ToolTip = "선택한 수주 품목의 수량을 남아서 종결합니다(승인 완료된 수주만).";
            // 
            // btnLineStopCancel
            // 
            this.btnLineStopCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnLineStopCancel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnLineStopCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLineStopCancel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnLineStopCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnLineStopCancel.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnLineStopCancel.Image = null;
            this.btnLineStopCancel.Location = new System.Drawing.Point(431, 4);
            this.btnLineStopCancel.Name = "btnLineStopCancel";
            this.btnLineStopCancel.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnLineStopCancel.Size = new System.Drawing.Size(109, 24);
            this.btnLineStopCancel.TabIndex = 0;
            this.btnLineStopCancel.Text = "수주품목 중단취소";
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
            this.btnDeletRow1.Size = new System.Drawing.Size(60, 24);
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
            this.btnAddRow1.Size = new System.Drawing.Size(60, 24);
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
            this.panelWyn1.Location = new System.Drawing.Point(3, 214);
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
            this.panelWyn5.Size = new System.Drawing.Size(1384, 214);
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
            this.panData.Controls.Add(this.labelControl1);
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
            this.panData.Size = new System.Drawing.Size(1384, 214);
            this.panData.TabIndex = 8;
            // 
            // lblAccId
            // 
            this.lblAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblAccId.Appearance.Options.UseFont = true;
            this.lblAccId.Location = new System.Drawing.Point(19, 14);
            this.lblAccId.Name = "lblAccId";
            this.lblAccId.Size = new System.Drawing.Size(36, 15);
            this.lblAccId.TabIndex = 0;
            this.lblAccId.Text = "사업장";
            // 
            // cboAccId
            // 
            this.cboAccId.EditValue = "";
            this.cboAccId.Location = new System.Drawing.Point(60, 11);
            this.cboAccId.LookupKey = "L_ACC";
            this.cboAccId.Name = "cboAccId";
            this.cboAccId.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.cboAccId.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboAccId.Properties.Appearance.Options.UseBackColor = true;
            this.cboAccId.Properties.Appearance.Options.UseForeColor = true;
            this.cboAccId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboAccId.Properties.NullText = "";
            this.cboAccId.Required = true;
            this.cboAccId.Size = new System.Drawing.Size(124, 20);
            this.cboAccId.TabIndex = 0;
            // 
            // lblSoNo
            // 
            this.lblSoNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblSoNo.Appearance.Options.UseFont = true;
            this.lblSoNo.Location = new System.Drawing.Point(201, 14);
            this.lblSoNo.Name = "lblSoNo";
            this.lblSoNo.Size = new System.Drawing.Size(48, 15);
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
            this.txtSoNo.Location = new System.Drawing.Point(253, 11);
            this.txtSoNo.Name = "txtSoNo";
            this.txtSoNo.Properties.ReadOnly = true;
            this.txtSoNo.Size = new System.Drawing.Size(129, 20);
            this.txtSoNo.TabIndex = 1;
            // 
            // lblStatCd
            // 
            this.lblStatCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblStatCd.Appearance.Options.UseFont = true;
            this.lblStatCd.Location = new System.Drawing.Point(648, 14);
            this.lblStatCd.Name = "lblStatCd";
            this.lblStatCd.Size = new System.Drawing.Size(48, 15);
            this.lblStatCd.TabIndex = 3;
            this.lblStatCd.Text = "진행상태";
            // 
            // cboApprStatCd
            // 
            this.cboApprStatCd.EditValue = "";
            this.cboApprStatCd.Location = new System.Drawing.Point(890, 41);
            this.cboApprStatCd.LookupKey = "L_AP0001";
            this.cboApprStatCd.Name = "cboApprStatCd";
            this.cboApprStatCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboApprStatCd.Properties.NullText = "";
            this.cboApprStatCd.Properties.ReadOnly = true;
            this.cboApprStatCd.Size = new System.Drawing.Size(120, 20);
            this.cboApprStatCd.TabIndex = 13;
            // 
            // cboStatCd
            // 
            this.cboStatCd.EditValue = "";
            this.cboStatCd.Location = new System.Drawing.Point(700, 11);
            this.cboStatCd.LookupKey = "L_MA0002";
            this.cboStatCd.Name = "cboStatCd";
            this.cboStatCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboStatCd.Properties.NullText = "";
            this.cboStatCd.Properties.ReadOnly = true;
            this.cboStatCd.Size = new System.Drawing.Size(108, 20);
            this.cboStatCd.TabIndex = 3;
            // 
            // lblSoDate
            // 
            this.lblSoDate.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblSoDate.Appearance.Options.UseFont = true;
            this.lblSoDate.Location = new System.Drawing.Point(7, 44);
            this.lblSoDate.Name = "lblSoDate";
            this.lblSoDate.Size = new System.Drawing.Size(48, 15);
            this.lblSoDate.TabIndex = 6;
            this.lblSoDate.Text = "수주일자";
            // 
            // dteSoDate
            // 
            this.dteSoDate.EditValue = new System.DateTime(2026, 10, 1, 0, 0, 0, 0);
            this.dteSoDate.Location = new System.Drawing.Point(60, 41);
            this.dteSoDate.Name = "dteSoDate";
            this.dteSoDate.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.dteSoDate.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.dteSoDate.Properties.Appearance.Options.UseBackColor = true;
            this.dteSoDate.Properties.Appearance.Options.UseForeColor = true;
            this.dteSoDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteSoDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dteSoDate.Required = true;
            this.dteSoDate.Size = new System.Drawing.Size(124, 20);
            this.dteSoDate.TabIndex = 4;
            this.dteSoDate.YyyyMmDd = "20261001";
            // 
            // lblDelvDate
            // 
            this.lblDelvDate.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblDelvDate.Appearance.Options.UseFont = true;
            this.lblDelvDate.Location = new System.Drawing.Point(201, 44);
            this.lblDelvDate.Name = "lblDelvDate";
            this.lblDelvDate.Size = new System.Drawing.Size(48, 15);
            this.lblDelvDate.TabIndex = 8;
            this.lblDelvDate.Text = "납기일자";
            // 
            // dteDelvDate
            // 
            this.dteDelvDate.EditValue = new System.DateTime(2026, 10, 1, 0, 0, 0, 0);
            this.dteDelvDate.Location = new System.Drawing.Point(253, 41);
            this.dteDelvDate.Name = "dteDelvDate";
            this.dteDelvDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteDelvDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dteDelvDate.Size = new System.Drawing.Size(129, 20);
            this.dteDelvDate.TabIndex = 5;
            this.dteDelvDate.YyyyMmDd = "20261001";
            // 
            // lblDeptNm
            // 
            this.lblDeptNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblDeptNm.Appearance.Options.UseFont = true;
            this.lblDeptNm.Location = new System.Drawing.Point(436, 44);
            this.lblDeptNm.Name = "lblDeptNm";
            this.lblDeptNm.Size = new System.Drawing.Size(24, 15);
            this.lblDeptNm.TabIndex = 10;
            this.lblDeptNm.Text = "부서";
            // 
            // txtDeptNm
            // 
            this.txtDeptNm.Location = new System.Drawing.Point(465, 41);
            this.txtDeptNm.LookupKey = "P_DEPT";
            this.txtDeptNm.MatchField = "dept_nm";
            this.txtDeptNm.Name = "txtDeptNm";
            this.txtDeptNm.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtDeptNm.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDeptNm.Properties.Appearance.Options.UseBackColor = true;
            this.txtDeptNm.Properties.Appearance.Options.UseForeColor = true;
            this.txtDeptNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDeptNm.Required = true;
            this.txtDeptNm.Size = new System.Drawing.Size(159, 20);
            this.txtDeptNm.TabIndex = 6;
            this.txtDeptNm.ToolTip = null;
            // 
            // txtDeptId
            // 
            this.txtDeptId.Location = new System.Drawing.Point(1200, 156);
            this.txtDeptId.Name = "txtDeptId";
            this.txtDeptId.Size = new System.Drawing.Size(135, 20);
            this.txtDeptId.TabIndex = 12;
            this.txtDeptId.Visible = false;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(648, 73);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 15);
            this.labelControl1.TabIndex = 13;
            this.labelControl1.Text = "부가세율";
            // 
            // lblEmpNm
            // 
            this.lblEmpNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblEmpNm.Appearance.Options.UseFont = true;
            this.lblEmpNm.Location = new System.Drawing.Point(660, 44);
            this.lblEmpNm.Name = "lblEmpNm";
            this.lblEmpNm.Size = new System.Drawing.Size(36, 15);
            this.lblEmpNm.TabIndex = 13;
            this.lblEmpNm.Text = "담당자";
            // 
            // txtEmpNm
            // 
            this.txtEmpNm.Location = new System.Drawing.Point(700, 41);
            this.txtEmpNm.LookupKey = "P_EMP";
            this.txtEmpNm.MatchField = "emp_nm";
            this.txtEmpNm.Name = "txtEmpNm";
            this.txtEmpNm.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtEmpNm.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtEmpNm.Properties.Appearance.Options.UseBackColor = true;
            this.txtEmpNm.Properties.Appearance.Options.UseForeColor = true;
            this.txtEmpNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtEmpNm.Required = true;
            this.txtEmpNm.Size = new System.Drawing.Size(108, 20);
            this.txtEmpNm.TabIndex = 7;
            this.txtEmpNm.ToolTip = null;
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
            this.lblCustNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblCustNm.Appearance.Options.UseFont = true;
            this.lblCustNm.Location = new System.Drawing.Point(424, 14);
            this.lblCustNm.Name = "lblCustNm";
            this.lblCustNm.Size = new System.Drawing.Size(36, 15);
            this.lblCustNm.TabIndex = 16;
            this.lblCustNm.Text = "거래처";
            // 
            // txtCustNm
            // 
            this.txtCustNm.Location = new System.Drawing.Point(465, 11);
            this.txtCustNm.LookupKey = "P_CUST";
            this.txtCustNm.MatchField = "cust_nm";
            this.txtCustNm.Name = "txtCustNm";
            this.txtCustNm.PopupConditions = "p_cust_class=SA";
            this.txtCustNm.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtCustNm.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCustNm.Properties.Appearance.Options.UseBackColor = true;
            this.txtCustNm.Properties.Appearance.Options.UseForeColor = true;
            this.txtCustNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtCustNm.Required = true;
            this.txtCustNm.Size = new System.Drawing.Size(159, 20);
            this.txtCustNm.TabIndex = 2;
            this.txtCustNm.ToolTip = null;
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
            this.lblCurCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblCurCd.Appearance.Options.UseFont = true;
            this.lblCurCd.Location = new System.Drawing.Point(31, 73);
            this.lblCurCd.Name = "lblCurCd";
            this.lblCurCd.Size = new System.Drawing.Size(24, 15);
            this.lblCurCd.TabIndex = 19;
            this.lblCurCd.Text = "통화";
            // 
            // cboCurCd
            // 
            this.cboCurCd.EditValue = "";
            this.cboCurCd.Location = new System.Drawing.Point(60, 70);
            this.cboCurCd.LookupKey = "L_CM0003";
            this.cboCurCd.Name = "cboCurCd";
            this.cboCurCd.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.cboCurCd.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboCurCd.Properties.Appearance.Options.UseBackColor = true;
            this.cboCurCd.Properties.Appearance.Options.UseForeColor = true;
            this.cboCurCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboCurCd.Properties.NullText = "";
            this.cboCurCd.Required = true;
            this.cboCurCd.Size = new System.Drawing.Size(124, 20);
            this.cboCurCd.TabIndex = 8;
            // 
            // lblExcRate
            // 
            this.lblExcRate.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblExcRate.Appearance.Options.UseFont = true;
            this.lblExcRate.Location = new System.Drawing.Point(225, 73);
            this.lblExcRate.Name = "lblExcRate";
            this.lblExcRate.Size = new System.Drawing.Size(24, 15);
            this.lblExcRate.TabIndex = 21;
            this.lblExcRate.Text = "환율";
            // 
            // txtExcRate
            // 
            this.txtExcRate.Location = new System.Drawing.Point(253, 70);
            this.txtExcRate.Name = "txtExcRate";
            this.txtExcRate.Size = new System.Drawing.Size(129, 20);
            this.txtExcRate.TabIndex = 9;
            // 
            // lblVatType
            // 
            this.lblVatType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblVatType.Appearance.Options.UseFont = true;
            this.lblVatType.Location = new System.Drawing.Point(400, 73);
            this.lblVatType.Name = "lblVatType";
            this.lblVatType.Size = new System.Drawing.Size(60, 15);
            this.lblVatType.TabIndex = 23;
            this.lblVatType.Text = "부가세유형";
            // 
            // cboVatType
            // 
            this.cboVatType.EditValue = "";
            this.cboVatType.Location = new System.Drawing.Point(465, 70);
            this.cboVatType.LookupKey = "L_CM0004";
            this.cboVatType.Name = "cboVatType";
            this.cboVatType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboVatType.Properties.NullText = "";
            this.cboVatType.Size = new System.Drawing.Size(159, 20);
            this.cboVatType.TabIndex = 10;
            // 
            // txtVatRate
            // 
            this.txtVatRate.Location = new System.Drawing.Point(700, 70);
            this.txtVatRate.Name = "txtVatRate";
            this.txtVatRate.Size = new System.Drawing.Size(108, 20);
            this.txtVatRate.TabIndex = 11;
            // 
            // lblAppNo
            // 
            this.lblAppNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblAppNo.Appearance.Options.UseFont = true;
            this.lblAppNo.Location = new System.Drawing.Point(834, 14);
            this.lblAppNo.Name = "lblAppNo";
            this.lblAppNo.Size = new System.Drawing.Size(48, 15);
            this.lblAppNo.TabIndex = 27;
            this.lblAppNo.Text = "결재번호";
            // 
            // txtAppNo
            // 
            this.txtAppNo.Location = new System.Drawing.Point(890, 11);
            this.txtAppNo.Name = "txtAppNo";
            this.txtAppNo.Properties.ReadOnly = true;
            this.txtAppNo.Size = new System.Drawing.Size(120, 20);
            this.txtAppNo.TabIndex = 12;
            // 
            // lblApprStatCd
            // 
            this.lblApprStatCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblApprStatCd.Appearance.Options.UseFont = true;
            this.lblApprStatCd.Location = new System.Drawing.Point(834, 44);
            this.lblApprStatCd.Name = "lblApprStatCd";
            this.lblApprStatCd.Size = new System.Drawing.Size(48, 15);
            this.lblApprStatCd.TabIndex = 29;
            this.lblApprStatCd.Text = "결재상태";
            // 
            // lblSoTitle
            // 
            this.lblSoTitle.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblSoTitle.Appearance.Options.UseFont = true;
            this.lblSoTitle.Location = new System.Drawing.Point(19, 103);
            this.lblSoTitle.Name = "lblSoTitle";
            this.lblSoTitle.Size = new System.Drawing.Size(36, 15);
            this.lblSoTitle.TabIndex = 30;
            this.lblSoTitle.Text = "수주명";
            // 
            // txtSoTitle
            // 
            this.txtSoTitle.Location = new System.Drawing.Point(60, 100);
            this.txtSoTitle.Name = "txtSoTitle";
            this.txtSoTitle.Size = new System.Drawing.Size(748, 20);
            this.txtSoTitle.TabIndex = 31;
            // 
            // lblRemark
            // 
            this.lblRemark.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblRemark.Appearance.Options.UseFont = true;
            this.lblRemark.Location = new System.Drawing.Point(31, 130);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(24, 15);
            this.lblRemark.TabIndex = 32;
            this.lblRemark.Text = "비고";
            // 
            // memoRemark
            // 
            this.memoRemark.Location = new System.Drawing.Point(60, 130);
            this.memoRemark.Name = "memoRemark";
            this.memoRemark.Size = new System.Drawing.Size(748, 72);
            this.memoRemark.TabIndex = 33;
            //
            // panBtn
            //
            this.panBtn.Appearance.BackColor = System.Drawing.Color.White;
            this.panBtn.Appearance.Options.UseBackColor = true;
            this.panBtn.Controls.Add(this.btnOpenApproval);
            this.panBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.panBtn.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panBtn.Location = new System.Drawing.Point(0, 0);
            this.panBtn.Name = "panBtn";
            this.panBtn.Padding = new System.Windows.Forms.Padding(0, 4, 8, 4);
            this.panBtn.Size = new System.Drawing.Size(1390, 30);
            this.panBtn.TabIndex = 9;
            //
            // btnOpenApproval
            //
            this.btnOpenApproval.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenApproval.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnOpenApproval.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenApproval.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnOpenApproval.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnOpenApproval.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnOpenApproval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenApproval.Image = null;
            this.btnOpenApproval.Location = new System.Drawing.Point(1282, 3);
            this.btnOpenApproval.Name = "btnOpenApproval";
            this.btnOpenApproval.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnOpenApproval.Size = new System.Drawing.Size(100, 24);
            this.btnOpenApproval.TabIndex = 0;
            this.btnOpenApproval.Text = "전자결재";
            this.btnOpenApproval.ToolTip = "저장된 수주의 전자결재를 상신/승인합니다.";
            //
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.White;
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.Controls.Add(this.lblSearchAccId);
            this.panHeader.Controls.Add(this.cboSearchAccId);
            this.panHeader.Controls.Add(this.lblSearchSoNo);
            this.panHeader.Controls.Add(this.txtSearchSoNo);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 25);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1390, 28);
            this.panHeader.TabIndex = 8;
            // 
            // lblSearchAccId (조회조건 첫 번째 - 사업장 표준, 2026-10-03)
            // 
            this.lblSearchAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSearchAccId.Appearance.Options.UseFont = true;
            this.lblSearchAccId.Location = new System.Drawing.Point(12, 8);
            this.lblSearchAccId.Name = "lblSearchAccId";
            this.lblSearchAccId.Size = new System.Drawing.Size(36, 15);
            this.lblSearchAccId.TabIndex = 0;
            this.lblSearchAccId.Text = "사업장";
            // 
            // cboSearchAccId (사업장 LookUp, Required - 기본값은 화면 생성자에서 로그인 사업장)
            // 
            this.cboSearchAccId.EditValue = "";
            this.cboSearchAccId.Location = new System.Drawing.Point(56, 5);
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
            this.panHeader.Visible = false;
            // 
            // lblSearchSoNo
            // 
            this.lblSearchSoNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblSearchSoNo.Appearance.Options.UseFont = true;
            this.lblSearchSoNo.Location = new System.Drawing.Point(211, 8);
            this.lblSearchSoNo.Name = "lblSearchSoNo";
            this.lblSearchSoNo.Size = new System.Drawing.Size(48, 15);
            this.lblSearchSoNo.TabIndex = 0;
            this.lblSearchSoNo.Text = "수주번호";
            // 
            // txtSearchSoNo
            // 
            this.txtSearchSoNo.Location = new System.Drawing.Point(267, 5);
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
            ((System.ComponentModel.ISupportInitialize)(this.panBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchSoNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).EndInit();
            this.paTitleH.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
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
    private ButtonWyn btnPickItem;
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
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private DevExpress.XtraGrid.Columns.GridColumn colWhNm;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
    private PanelWyn panBtn;
    private ButtonWyn btnOpenApproval;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private WYNLAB.Base.Controls.LookUpEditWyn cboSearchAccId;
}
