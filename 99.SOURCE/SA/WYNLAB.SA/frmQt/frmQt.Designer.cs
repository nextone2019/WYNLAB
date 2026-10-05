// 견적등록 - frmPo(구매발주등록)와 같은 Master-One Sheet 패턴, 결재/검수/재고 컬럼은 뺐다.
// 좌표는 기본값이고 VS 디자이너로 자유롭게 조정 가능합니다.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.SA;

public partial class frmQt
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQt));
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
            this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnConfirm = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnConfirmCancel = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnDeletRow1 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAddRow1 = new WYNLAB.Base.Controls.ButtonWyn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.lblAccId = new DevExpress.XtraEditors.LabelControl();
            this.cboAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblQtNo = new DevExpress.XtraEditors.LabelControl();
            this.txtQtNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblStatCd = new DevExpress.XtraEditors.LabelControl();
            this.cboStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.chkCfmYn = new WYNLAB.Base.Controls.CheckBoxWyn();
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
            this.lblQtDate = new DevExpress.XtraEditors.LabelControl();
            this.dteQtDate = new WYNLAB.Base.Controls.DateEditWyn();
            this.lblValidDate = new DevExpress.XtraEditors.LabelControl();
            this.dteValidDate = new WYNLAB.Base.Controls.DateEditWyn();
            this.lblExcRate = new DevExpress.XtraEditors.LabelControl();
            this.txtExcRate = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblVatType = new DevExpress.XtraEditors.LabelControl();
            this.cboVatType = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblVatRate = new DevExpress.XtraEditors.LabelControl();
            this.txtVatRate = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblQtTitle = new DevExpress.XtraEditors.LabelControl();
            this.txtQtTitle = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblRemark = new DevExpress.XtraEditors.LabelControl();
            this.memoRemark = new DevExpress.XtraEditors.MemoEdit();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
            this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblSearchQtNo = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchQtNo = new WYNLAB.Base.Controls.TextEditWyn();
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
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQtNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCfmYn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCurCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteQtDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteQtDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteValidDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteValidDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcRate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboVatType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVatRate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQtTitle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchQtNo.Properties)).BeginInit();
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
            this.panBase.Size = new System.Drawing.Size(1280, 800);
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
            this.panelWyn3.Size = new System.Drawing.Size(1270, 721);
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
            this.panelWyn4.Size = new System.Drawing.Size(1270, 721);
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
            this.datecolDelv});
            this.grd1.Size = new System.Drawing.Size(1264, 487);
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
            this.colRemark});
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
            this.colQty.Caption = "견적수량";
            this.colQty.DisplayFormat.FormatString = "#,##0.####";
            this.colQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colQty.FieldName = "qty";
            this.colQty.Name = "colQty";
            this.colQty.Visible = true;
            this.colQty.VisibleIndex = 6;
            this.colQty.Width = 70;
            //
            // colPrice
            //
            this.colPrice.Caption = "단가";
            this.colPrice.DisplayFormat.FormatString = "#,##0.####";
            this.colPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colPrice.FieldName = "price";
            this.colPrice.Name = "colPrice";
            this.colPrice.Visible = true;
            this.colPrice.VisibleIndex = 7;
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
            this.colAmt.VisibleIndex = 8;
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
            this.colVatRate.VisibleIndex = 9;
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
            this.colVat.VisibleIndex = 10;
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
            this.colTotalAmt.VisibleIndex = 11;
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
            this.colKorPrice.VisibleIndex = 12;
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
            this.colKorAmt.VisibleIndex = 13;
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
            this.colKorVat.VisibleIndex = 14;
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
            this.colKorTotalAmt.VisibleIndex = 15;
            this.colKorTotalAmt.Width = 90;
            //
            // colDelvDate
            //
            this.colDelvDate.Caption = "희망납기";
            this.colDelvDate.ColumnEdit = this.datecolDelv;
            this.colDelvDate.FieldName = "delv_date";
            this.colDelvDate.Name = "colDelvDate";
            this.colDelvDate.Visible = true;
            this.colDelvDate.VisibleIndex = 16;
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
            // colRemark
            //
            this.colRemark.Caption = "비고";
            this.colRemark.FieldName = "remark";
            this.colRemark.Name = "colRemark";
            this.colRemark.Visible = true;
            this.colRemark.VisibleIndex = 17;
            this.colRemark.Width = 160;
            //
            // panelWyn7
            //
            this.panelWyn7.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn7.Appearance.Options.UseBackColor = true;
            this.panelWyn7.Controls.Add(this.btnConfirm);
            this.panelWyn7.Controls.Add(this.btnConfirmCancel);
            this.panelWyn7.Controls.Add(this.btnDeletRow1);
            this.panelWyn7.Controls.Add(this.btnAddRow1);
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(3, 204);
            this.panelWyn7.Name = "panelWyn7";
            this.panelWyn7.Padding = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.panelWyn7.Size = new System.Drawing.Size(1264, 30);
            this.panelWyn7.TabIndex = 9;
            //
            // btnConfirm
            //
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfirm.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnConfirm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnConfirm.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnConfirm.Image = null;
            this.btnConfirm.Location = new System.Drawing.Point(130, 4);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnConfirm.Size = new System.Drawing.Size(84, 24);
            this.btnConfirm.TabIndex = 0;
            this.btnConfirm.Text = "견적확정";
            this.btnConfirm.ToolTip = "이 견적을 확정합니다. 확정 전에는 수정/삭제할 수 있고 수주에서 불러올 수 있습니다.";
            //
            // btnConfirmCancel
            //
            this.btnConfirmCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirmCancel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnConfirmCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfirmCancel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnConfirmCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnConfirmCancel.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnConfirmCancel.Image = null;
            this.btnConfirmCancel.Location = new System.Drawing.Point(218, 4);
            this.btnConfirmCancel.Name = "btnConfirmCancel";
            this.btnConfirmCancel.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnConfirmCancel.Size = new System.Drawing.Size(84, 24);
            this.btnConfirmCancel.TabIndex = 0;
            this.btnConfirmCancel.Text = "확정취소";
            this.btnConfirmCancel.ToolTip = "견적확정을 취소합니다.";
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
            this.panelWyn1.Location = new System.Drawing.Point(3, 164);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn1.Size = new System.Drawing.Size(1264, 40);
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
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(1259, 38);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 10;
            this.sectionHeaderWyn4.Text = "견적 품목정보";
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
            this.panelWyn5.Size = new System.Drawing.Size(1264, 164);
            this.panelWyn5.TabIndex = 6;
            //
            // panData
            //
            this.panData.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panData.Appearance.Options.UseBackColor = true;
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Controls.Add(this.lblAccId);
            this.panData.Controls.Add(this.cboAccId);
            this.panData.Controls.Add(this.lblQtNo);
            this.panData.Controls.Add(this.txtQtNo);
            this.panData.Controls.Add(this.lblStatCd);
            this.panData.Controls.Add(this.cboStatCd);
            this.panData.Controls.Add(this.chkCfmYn);
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
            this.panData.Controls.Add(this.lblQtDate);
            this.panData.Controls.Add(this.dteQtDate);
            this.panData.Controls.Add(this.lblValidDate);
            this.panData.Controls.Add(this.dteValidDate);
            this.panData.Controls.Add(this.lblExcRate);
            this.panData.Controls.Add(this.txtExcRate);
            this.panData.Controls.Add(this.lblVatType);
            this.panData.Controls.Add(this.cboVatType);
            this.panData.Controls.Add(this.lblVatRate);
            this.panData.Controls.Add(this.txtVatRate);
            this.panData.Controls.Add(this.lblQtTitle);
            this.panData.Controls.Add(this.txtQtTitle);
            this.panData.Controls.Add(this.lblRemark);
            this.panData.Controls.Add(this.memoRemark);
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 0);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(1264, 164);
            this.panData.TabIndex = 8;
            //
            // lblAccId
            //
            this.lblAccId.Location = new System.Drawing.Point(18, 14);
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
            // lblQtNo
            //
            this.lblQtNo.Location = new System.Drawing.Point(247, 14);
            this.lblQtNo.Name = "lblQtNo";
            this.lblQtNo.Size = new System.Drawing.Size(48, 14);
            this.lblQtNo.TabIndex = 1;
            this.lblQtNo.Text = "견적번호";
            //
            // txtQtNo
            //
            this.txtQtNo.Location = new System.Drawing.Point(302, 11);
            this.txtQtNo.Name = "txtQtNo";
            this.txtQtNo.Properties.ReadOnly = true;
            this.txtQtNo.Size = new System.Drawing.Size(150, 20);
            this.txtQtNo.TabIndex = 1;
            //
            // lblStatCd
            //
            this.lblStatCd.Location = new System.Drawing.Point(500, 14);
            this.lblStatCd.Name = "lblStatCd";
            this.lblStatCd.Size = new System.Drawing.Size(48, 14);
            this.lblStatCd.TabIndex = 2;
            this.lblStatCd.Text = "진행상태";
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
            this.cboStatCd.TabIndex = 3;
            //
            // chkCfmYn
            //
            this.chkCfmYn.Location = new System.Drawing.Point(700, 12);
            this.chkCfmYn.Name = "chkCfmYn";
            this.chkCfmYn.Properties.Caption = "견적확정여부";
            this.chkCfmYn.Properties.ReadOnly = true;
            this.chkCfmYn.Size = new System.Drawing.Size(90, 19);
            this.chkCfmYn.TabIndex = 4;
            //
            // lblDeptNm
            //
            this.lblDeptNm.Location = new System.Drawing.Point(30, 41);
            this.lblDeptNm.Name = "lblDeptNm";
            this.lblDeptNm.Size = new System.Drawing.Size(24, 14);
            this.lblDeptNm.TabIndex = 5;
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
            this.txtDeptNm.TabIndex = 6;
            //
            // txtDeptId
            //
            this.txtDeptId.Location = new System.Drawing.Point(1100, 130);
            this.txtDeptId.Name = "txtDeptId";
            this.txtDeptId.Size = new System.Drawing.Size(120, 20);
            this.txtDeptId.TabIndex = 7;
            this.txtDeptId.Visible = false;
            //
            // lblEmpNm
            //
            this.lblEmpNm.Location = new System.Drawing.Point(193, 41);
            this.lblEmpNm.Name = "lblEmpNm";
            this.lblEmpNm.Size = new System.Drawing.Size(36, 14);
            this.lblEmpNm.TabIndex = 8;
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
            this.txtEmpNm.TabIndex = 9;
            //
            // txtEmpId
            //
            this.txtEmpId.Location = new System.Drawing.Point(1100, 156);
            this.txtEmpId.Name = "txtEmpId";
            this.txtEmpId.Size = new System.Drawing.Size(120, 20);
            this.txtEmpId.TabIndex = 10;
            this.txtEmpId.Visible = false;
            //
            // lblCustNm
            //
            this.lblCustNm.Location = new System.Drawing.Point(335, 41);
            this.lblCustNm.Name = "lblCustNm";
            this.lblCustNm.Size = new System.Drawing.Size(36, 14);
            this.lblCustNm.TabIndex = 11;
            this.lblCustNm.Text = "거래처";
            //
            // txtCustNm
            //
            this.txtCustNm.Location = new System.Drawing.Point(377, 38);
            this.txtCustNm.LookupKey = "P_CUST";
            this.txtCustNm.PopupConditions = "p_cust_class=SA";
            this.txtCustNm.MatchField = "cust_nm";
            this.txtCustNm.Name = "txtCustNm";
            this.txtCustNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis)});
            this.txtCustNm.Required = true;
            this.txtCustNm.Size = new System.Drawing.Size(226, 20);
            this.txtCustNm.TabIndex = 12;
            //
            // txtCustId
            //
            this.txtCustId.Location = new System.Drawing.Point(1100, 182);
            this.txtCustId.Name = "txtCustId";
            this.txtCustId.Size = new System.Drawing.Size(120, 20);
            this.txtCustId.TabIndex = 13;
            this.txtCustId.Visible = false;
            //
            // lblCurCd
            //
            this.lblCurCd.Location = new System.Drawing.Point(618, 41);
            this.lblCurCd.Name = "lblCurCd";
            this.lblCurCd.Size = new System.Drawing.Size(24, 14);
            this.lblCurCd.TabIndex = 14;
            this.lblCurCd.Text = "통화";
            //
            // cboCurCd
            //
            this.cboCurCd.EditValue = "";
            this.cboCurCd.Location = new System.Drawing.Point(648, 38);
            this.cboCurCd.LookupKey = "L_CM0003";
            this.cboCurCd.Name = "cboCurCd";
            this.cboCurCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboCurCd.Properties.NullText = "";
            this.cboCurCd.Required = true;
            this.cboCurCd.Size = new System.Drawing.Size(80, 20);
            this.cboCurCd.TabIndex = 15;
            //
            // lblQtDate
            //
            this.lblQtDate.Location = new System.Drawing.Point(18, 67);
            this.lblQtDate.Name = "lblQtDate";
            this.lblQtDate.Size = new System.Drawing.Size(36, 14);
            this.lblQtDate.TabIndex = 16;
            this.lblQtDate.Text = "견적일자";
            //
            // dteQtDate
            //
            this.dteQtDate.Location = new System.Drawing.Point(60, 64);
            this.dteQtDate.Name = "dteQtDate";
            this.dteQtDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteQtDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dteQtDate.Required = true;
            this.dteQtDate.Size = new System.Drawing.Size(124, 20);
            this.dteQtDate.TabIndex = 17;
            //
            // lblValidDate
            //
            this.lblValidDate.Location = new System.Drawing.Point(193, 67);
            this.lblValidDate.Name = "lblValidDate";
            this.lblValidDate.Size = new System.Drawing.Size(36, 14);
            this.lblValidDate.TabIndex = 18;
            this.lblValidDate.Text = "유효기한";
            //
            // dteValidDate
            //
            this.dteValidDate.Location = new System.Drawing.Point(234, 64);
            this.dteValidDate.Name = "dteValidDate";
            this.dteValidDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteValidDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dteValidDate.Size = new System.Drawing.Size(124, 20);
            this.dteValidDate.TabIndex = 19;
            //
            // lblExcRate
            //
            this.lblExcRate.Location = new System.Drawing.Point(377, 67);
            this.lblExcRate.Name = "lblExcRate";
            this.lblExcRate.Size = new System.Drawing.Size(24, 14);
            this.lblExcRate.TabIndex = 20;
            this.lblExcRate.Text = "환율";
            //
            // txtExcRate
            //
            this.txtExcRate.Location = new System.Drawing.Point(407, 64);
            this.txtExcRate.Name = "txtExcRate";
            this.txtExcRate.Size = new System.Drawing.Size(80, 20);
            this.txtExcRate.TabIndex = 21;
            //
            // lblVatType
            //
            this.lblVatType.Location = new System.Drawing.Point(500, 67);
            this.lblVatType.Name = "lblVatType";
            this.lblVatType.Size = new System.Drawing.Size(48, 14);
            this.lblVatType.TabIndex = 22;
            this.lblVatType.Text = "과세구분";
            //
            // cboVatType
            //
            this.cboVatType.EditValue = "";
            this.cboVatType.Location = new System.Drawing.Point(555, 64);
            this.cboVatType.LookupKey = "L_CM0004";
            this.cboVatType.Name = "cboVatType";
            this.cboVatType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboVatType.Properties.NullText = "";
            this.cboVatType.Size = new System.Drawing.Size(108, 20);
            this.cboVatType.TabIndex = 23;
            //
            // lblVatRate
            //
            this.lblVatRate.Location = new System.Drawing.Point(680, 67);
            this.lblVatRate.Name = "lblVatRate";
            this.lblVatRate.Size = new System.Drawing.Size(48, 14);
            this.lblVatRate.TabIndex = 24;
            this.lblVatRate.Text = "부가세율";
            //
            // txtVatRate
            //
            this.txtVatRate.Location = new System.Drawing.Point(735, 64);
            this.txtVatRate.Name = "txtVatRate";
            this.txtVatRate.Size = new System.Drawing.Size(60, 20);
            this.txtVatRate.TabIndex = 25;
            //
            // lblQtTitle
            //
            this.lblQtTitle.Location = new System.Drawing.Point(18, 93);
            this.lblQtTitle.Name = "lblQtTitle";
            this.lblQtTitle.Size = new System.Drawing.Size(36, 14);
            this.lblQtTitle.TabIndex = 26;
            this.lblQtTitle.Text = "건명";
            //
            // txtQtTitle
            //
            this.txtQtTitle.Location = new System.Drawing.Point(60, 90);
            this.txtQtTitle.Name = "txtQtTitle";
            this.txtQtTitle.Size = new System.Drawing.Size(668, 20);
            this.txtQtTitle.TabIndex = 27;
            //
            // lblRemark
            //
            this.lblRemark.Location = new System.Drawing.Point(30, 119);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(24, 14);
            this.lblRemark.TabIndex = 28;
            this.lblRemark.Text = "비고";
            //
            // memoRemark
            //
            this.memoRemark.Location = new System.Drawing.Point(60, 116);
            this.memoRemark.Name = "memoRemark";
            this.memoRemark.Size = new System.Drawing.Size(668, 40);
            this.memoRemark.TabIndex = 29;
            //
            // panHeader
            //
            this.panHeader.Appearance.BackColor = System.Drawing.Color.White;
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.Controls.Add(this.lblSearchAccId);
            this.panHeader.Controls.Add(this.cboSearchAccId);
            this.panHeader.Controls.Add(this.lblSearchQtNo);
            this.panHeader.Controls.Add(this.txtSearchQtNo);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 25);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1270, 49);
            this.panHeader.TabIndex = 8;
            // 
            // lblSearchAccId (조회조건 첫 번째 - 사업장 표준, 2026-10-03)
            // 
            this.lblSearchAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSearchAccId.Appearance.Options.UseFont = true;
            this.lblSearchAccId.Location = new System.Drawing.Point(19, 20);
            this.lblSearchAccId.Name = "lblSearchAccId";
            this.lblSearchAccId.Size = new System.Drawing.Size(36, 15);
            this.lblSearchAccId.TabIndex = 0;
            this.lblSearchAccId.Text = "사업장";
            // 
            // cboSearchAccId (사업장 LookUp, Required - 기본값은 화면 생성자에서 로그인 사업장)
            // 
            this.cboSearchAccId.EditValue = "";
            this.cboSearchAccId.Location = new System.Drawing.Point(63, 17);
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
            // lblSearchQtNo
            //
            this.lblSearchQtNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblSearchQtNo.Appearance.Options.UseFont = true;
            this.lblSearchQtNo.Location = new System.Drawing.Point(218, 20);
            this.lblSearchQtNo.Name = "lblSearchQtNo";
            this.lblSearchQtNo.Size = new System.Drawing.Size(48, 15);
            this.lblSearchQtNo.TabIndex = 0;
            this.lblSearchQtNo.Text = "견적번호";
            //
            // txtSearchQtNo
            //
            this.txtSearchQtNo.Location = new System.Drawing.Point(274, 17);
            this.txtSearchQtNo.Name = "txtSearchQtNo";
            this.txtSearchQtNo.Size = new System.Drawing.Size(187, 20);
            this.txtSearchQtNo.TabIndex = 1;
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
            this.paTitleH.Size = new System.Drawing.Size(1270, 25);
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
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1265, 25);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 14;
            this.sectionHeaderWyn1.Text = "견적등록 [frmQt]";
            //
            // frmQt
            //
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1280, 800);
            this.Controls.Add(this.panBase);
            this.Name = "frmQt";
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
            ((System.ComponentModel.ISupportInitialize)(this.txtQtNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCfmYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCurCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteQtDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteQtDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteValidDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteValidDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcRate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboVatType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVatRate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQtTitle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchQtNo.Properties)).EndInit();
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
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private PanelWyn panelWyn7;
    private ButtonWyn btnConfirm;
    private ButtonWyn btnConfirmCancel;
    private ButtonWyn btnDeletRow1;
    private ButtonWyn btnAddRow1;
    private PanelWyn panelWyn1;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblAccId;
    private LookUpEditWyn cboAccId;
    private DevExpress.XtraEditors.LabelControl lblQtNo;
    private TextEditWyn txtQtNo;
    private DevExpress.XtraEditors.LabelControl lblStatCd;
    private LookUpEditWyn cboStatCd;
    private CheckBoxWyn chkCfmYn;
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
    private DevExpress.XtraEditors.LabelControl lblQtDate;
    private DateEditWyn dteQtDate;
    private DevExpress.XtraEditors.LabelControl lblValidDate;
    private DateEditWyn dteValidDate;
    private DevExpress.XtraEditors.LabelControl lblExcRate;
    private TextEditWyn txtExcRate;
    private DevExpress.XtraEditors.LabelControl lblVatType;
    private LookUpEditWyn cboVatType;
    private DevExpress.XtraEditors.LabelControl lblVatRate;
    private TextEditWyn txtVatRate;
    private DevExpress.XtraEditors.LabelControl lblQtTitle;
    private TextEditWyn txtQtTitle;
    private DevExpress.XtraEditors.LabelControl lblRemark;
    private DevExpress.XtraEditors.MemoEdit memoRemark;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchQtNo;
    private TextEditWyn txtSearchQtNo;
    private PanelWyn paTitleH;
    private SectionHeaderWyn sectionHeaderWyn1;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private WYNLAB.Base.Controls.LookUpEditWyn cboSearchAccId;
}
