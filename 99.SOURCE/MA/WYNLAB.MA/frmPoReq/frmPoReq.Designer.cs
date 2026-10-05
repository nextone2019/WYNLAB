// AI Builder Master-One Sheet 템플릿(TplMasterOneSheet) 기반 - 이 화면은 하위그리드가
// 품목 하나뿐이라 grd2~grd5 탭 구조를 걷어내고 grd1 하나만 남겼다(2026-09-22).
// 디자인(제목영역/여백/색상)을 바꾸려면 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 참고하되,
// 필드 구성 자체는 이 파일에서 직접 다듬으면 됩니다 - VS 디자이너로 자유롭게 편집 가능합니다.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

public partial class frmPoReq
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions3 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject9 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject10 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject11 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject12 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPoReq));
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions4 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject13 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject14 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject15 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject16 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions5 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject17 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject18 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject19 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject20 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions6 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject21 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject22 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject23 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject24 = new DevExpress.Utils.SerializableAppearanceObject();
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
            this.colQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNextQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRemainQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookupcolUnitCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colPrice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAmt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVatRate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotalAmt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colKorPrice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colKorAmt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colKorVat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colKorTotalAmt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCustId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.popcolCust = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
            this.colWhId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.popcolWh = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
            this.colWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDelvDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.datecolDelv = new WYNLAB.Base.Controls.DateColumnEdit();
            this.colSrcType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookupcolSrcType = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colSrcNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDeletRow1 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAddRow1 = new WYNLAB.Base.Controls.ButtonWyn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.btnSaveAs = new WYNLAB.Base.Controls.ButtonWyn();
            this.lblAccId = new DevExpress.XtraEditors.LabelControl();
            this.cboAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblPoType = new DevExpress.XtraEditors.LabelControl();
            this.cboApprStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.txtExcRate = new WYNLAB.Base.Controls.TextEditWyn();
            this.cboPoType = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblReqNo = new DevExpress.XtraEditors.LabelControl();
            this.txtEmpNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtApprId = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtReqId = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtReqNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblStatCd = new DevExpress.XtraEditors.LabelControl();
            this.cboStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblReqDate = new DevExpress.XtraEditors.LabelControl();
            this.dteReqDate = new WYNLAB.Base.Controls.DateEditWyn();
            this.lblCurCd = new DevExpress.XtraEditors.LabelControl();
            this.cboCurCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblDeptNm = new DevExpress.XtraEditors.LabelControl();
            this.txtDeptNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.txtDeptId = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblEmpNm = new DevExpress.XtraEditors.LabelControl();
            this.txtEmpNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.txtEmpId = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblCustNm = new DevExpress.XtraEditors.LabelControl();
            this.txtCustNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.txtCustId = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblAppNo = new DevExpress.XtraEditors.LabelControl();
            this.txtAppNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblApprStatCd = new DevExpress.XtraEditors.LabelControl();
            this.lblSumAmt = new DevExpress.XtraEditors.LabelControl();
            this.txtSumAmt = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSumVat = new DevExpress.XtraEditors.LabelControl();
            this.txtSumVat = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSumTotalAmt = new DevExpress.XtraEditors.LabelControl();
            this.txtSumTotalAmt = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblReqTitle = new DevExpress.XtraEditors.LabelControl();
            this.txtReqTitle = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblRemark = new DevExpress.XtraEditors.LabelControl();
            this.memoRemark = new DevExpress.XtraEditors.MemoEdit();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnOpenApproval = new WYNLAB.Base.Controls.ButtonWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
            this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblSearchReqNo = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchReqNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
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
            ((System.ComponentModel.ISupportInitialize)(this.popcolCust)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolWh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolDelv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolDelv.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolSrcType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboApprStatCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcRate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPoType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtApprId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReqId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReqNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteReqDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteReqDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCurCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAppNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSumAmt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSumVat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSumTotalAmt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReqTitle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
            this.panelWyn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchReqNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).BeginInit();
            this.paTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panBase
            // 
            this.panBase.Appearance.BackColor = System.Drawing.Color.White;
            this.panBase.Appearance.Options.UseBackColor = true;
            this.panBase.Controls.Add(this.panelWyn3);
            this.panBase.Controls.Add(this.panHeader);
            this.panBase.Controls.Add(this.paTitle);
            this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBase.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panBase.Location = new System.Drawing.Point(0, 0);
            this.panBase.Name = "panBase";
            this.panBase.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.panBase.Size = new System.Drawing.Size(1680, 760);
            this.panBase.TabIndex = 6;
            // 
            // panelWyn3
            // 
            this.panelWyn3.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn3.Appearance.Options.UseBackColor = true;
            this.panelWyn3.Controls.Add(this.panelWyn4);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(5, 82);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Size = new System.Drawing.Size(1670, 673);
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
            this.panelWyn4.Size = new System.Drawing.Size(1670, 673);
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
            this.grd1.Location = new System.Drawing.Point(3, 376);
            this.grd1.MainView = this.gvw1;
            this.grd1.Name = "grd1";
            this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.popcolItem,
            this.lookupcolUnitCd,
            this.popcolCust,
            this.popcolWh,
            this.datecolDelv,
            this.lookupcolSrcType});
            this.grd1.Size = new System.Drawing.Size(1664, 297);
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
            this.colQty,
            this.colNextQty,
            this.colRemainQty,
            this.colUnitCd,
            this.colPrice,
            this.colAmt,
            this.colVatRate,
            this.colVat,
            this.colTotalAmt,
            this.colKorPrice,
            this.colKorAmt,
            this.colKorVat,
            this.colKorTotalAmt,
            this.colCustId,
            this.colWhId,
            this.colWhNm,
            this.colDelvDate,
            this.colSrcType,
            this.colSrcNo});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightFocusedRow = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowFooter = true;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            this.gvw1.RequiredFields = "item_no,qty";
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
            this.colItemNo.Width = 157;
            // 
            // popcolItem
            // 
            this.popcolItem.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "...", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.popcolItem.LookupKey = "P_ITEM_PO";
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
            this.colItemNm.Width = 150;
            // 
            // colItemSpec
            // 
            this.colItemSpec.Caption = "규격";
            this.colItemSpec.FieldName = "item_spec";
            this.colItemSpec.Name = "colItemSpec";
            this.colItemSpec.OptionsColumn.AllowEdit = false;
            this.colItemSpec.Visible = true;
            this.colItemSpec.VisibleIndex = 4;
            this.colItemSpec.Width = 120;
            // 
            // colQty
            // 
            this.colQty.Caption = "구매요청수량";
            this.colQty.DisplayFormat.FormatString = "#,##0.####";
            this.colQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colQty.FieldName = "qty";
            this.colQty.Name = "colQty";
            this.colQty.Visible = true;
            this.colQty.VisibleIndex = 5;
            this.colQty.Width = 90;
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
            this.colNextQty.VisibleIndex = 6;
            this.colNextQty.Width = 80;
            // 
            // colRemainQty
            // 
            this.colRemainQty.Caption = "발주잔량";
            this.colRemainQty.DisplayFormat.FormatString = "#,##0.####";
            this.colRemainQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colRemainQty.FieldName = "remain_qty";
            this.colRemainQty.Name = "colRemainQty";
            this.colRemainQty.OptionsColumn.AllowEdit = false;
            this.colRemainQty.Visible = true;
            this.colRemainQty.VisibleIndex = 7;
            this.colRemainQty.Width = 80;
            // 
            // colUnitCd
            // 
            this.colUnitCd.Caption = "구매단위";
            this.colUnitCd.ColumnEdit = this.lookupcolUnitCd;
            this.colUnitCd.FieldName = "unit_cd";
            this.colUnitCd.Name = "colUnitCd";
            this.colUnitCd.Visible = true;
            this.colUnitCd.VisibleIndex = 8;
            this.colUnitCd.Width = 70;
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
            this.colKorPrice.Width = 90;
            // 
            // colKorAmt
            // 
            this.colKorAmt.Caption = "원화공급가액";
            this.colKorAmt.DisplayFormat.FormatString = "#,##0.####";
            this.colKorAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colKorAmt.FieldName = "kor_amt";
            this.colKorAmt.Name = "colKorAmt";
            this.colKorAmt.Visible = true;
            this.colKorAmt.VisibleIndex = 15;
            this.colKorAmt.Width = 100;
            // 
            // colKorVat
            // 
            this.colKorVat.Caption = "원화부가세";
            this.colKorVat.DisplayFormat.FormatString = "#,##0.####";
            this.colKorVat.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colKorVat.FieldName = "kor_vat";
            this.colKorVat.Name = "colKorVat";
            this.colKorVat.Visible = true;
            this.colKorVat.VisibleIndex = 16;
            this.colKorVat.Width = 90;
            // 
            // colKorTotalAmt
            // 
            this.colKorTotalAmt.Caption = "원화합계";
            this.colKorTotalAmt.DisplayFormat.FormatString = "#,##0.####";
            this.colKorTotalAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colKorTotalAmt.FieldName = "kor_total_amt";
            this.colKorTotalAmt.Name = "colKorTotalAmt";
            this.colKorTotalAmt.Visible = true;
            this.colKorTotalAmt.VisibleIndex = 17;
            this.colKorTotalAmt.Width = 100;
            // 
            // colCustId
            // 
            this.colCustId.Caption = "거래처";
            this.colCustId.ColumnEdit = this.popcolCust;
            this.colCustId.FieldName = "cust_id";
            this.colCustId.Name = "colCustId";
            this.colCustId.Visible = true;
            this.colCustId.VisibleIndex = 18;
            this.colCustId.Width = 90;
            // 
            // popcolCust
            // 
            this.popcolCust.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, "...", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.popcolCust.LookupKey = "P_CUST";
            this.popcolCust.PopupConditions = "p_cust_class=PO";
            this.popcolCust.Name = "popcolCust";
            // 
            // colWhId
            // 
            this.colWhId.Caption = "창고";
            this.colWhId.ColumnEdit = this.popcolWh;
            this.colWhId.FieldName = "wh_id";
            this.colWhId.Name = "colWhId";
            this.colWhId.Width = 90;
            // 
            // popcolWh
            // 
            this.popcolWh.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions3, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject9, serializableAppearanceObject10, serializableAppearanceObject11, serializableAppearanceObject12, "...", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.popcolWh.LookupKey = "P_WH";
            this.popcolWh.Name = "popcolWh";
            // 
            // colWhNm
            // 
            this.colWhNm.Caption = "창고명";
            this.colWhNm.FieldName = "wh_nm";
            this.colWhNm.Name = "colWhNm";
            this.colWhNm.OptionsColumn.AllowEdit = false;
            this.colWhNm.Visible = true;
            this.colWhNm.VisibleIndex = 19;
            this.colWhNm.Width = 110;
            // 
            // colDelvDate
            // 
            this.colDelvDate.Caption = "요청납기일";
            this.colDelvDate.ColumnEdit = this.datecolDelv;
            this.colDelvDate.FieldName = "delv_date";
            this.colDelvDate.Name = "colDelvDate";
            this.colDelvDate.Visible = true;
            this.colDelvDate.VisibleIndex = 20;
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
            // colSrcType
            // 
            this.colSrcType.Caption = "원천구분";
            this.colSrcType.ColumnEdit = this.lookupcolSrcType;
            this.colSrcType.FieldName = "src_type";
            this.colSrcType.Name = "colSrcType";
            this.colSrcType.OptionsColumn.AllowEdit = false;
            this.colSrcType.Visible = true;
            this.colSrcType.VisibleIndex = 21;
            this.colSrcType.Width = 90;
            // 
            // lookupcolSrcType
            // 
            this.lookupcolSrcType.AutoHeight = false;
            this.lookupcolSrcType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookupcolSrcType.LookupKey = "L_MA0004";
            this.lookupcolSrcType.Name = "lookupcolSrcType";
            this.lookupcolSrcType.NullText = "";
            this.lookupcolSrcType.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
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
            // panelWyn7
            // 
            this.panelWyn7.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn7.Appearance.Options.UseBackColor = true;
            this.panelWyn7.Controls.Add(this.btnDeletRow1);
            this.panelWyn7.Controls.Add(this.btnAddRow1);
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(3, 346);
            this.panelWyn7.Name = "panelWyn7";
            this.panelWyn7.Padding = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.panelWyn7.Size = new System.Drawing.Size(1664, 30);
            this.panelWyn7.TabIndex = 9;
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
            this.btnDeletRow1.Location = new System.Drawing.Point(68, 4);
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
            this.btnAddRow1.Location = new System.Drawing.Point(6, 4);
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
            this.panelWyn1.Location = new System.Drawing.Point(3, 319);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn1.Size = new System.Drawing.Size(1664, 27);
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
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(1659, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 10;
            this.sectionHeaderWyn4.Text = "구매요청상세정보";
            // 
            // panelWyn5
            // 
            this.panelWyn5.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn5.Appearance.Options.UseBackColor = true;
            this.panelWyn5.Controls.Add(this.panData);
            this.panelWyn5.Controls.Add(this.panelWyn2);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(3, 0);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Size = new System.Drawing.Size(1664, 319);
            this.panelWyn5.TabIndex = 6;
            // 
            // panData
            // 
            this.panData.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panData.Appearance.Options.UseBackColor = true;
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Controls.Add(this.btnSaveAs);
            this.panData.Controls.Add(this.lblAccId);
            this.panData.Controls.Add(this.cboAccId);
            this.panData.Controls.Add(this.lblPoType);
            this.panData.Controls.Add(this.cboApprStatCd);
            this.panData.Controls.Add(this.txtExcRate);
            this.panData.Controls.Add(this.cboPoType);
            this.panData.Controls.Add(this.lblReqNo);
            this.panData.Controls.Add(this.txtEmpNo);
            this.panData.Controls.Add(this.txtApprId);
            this.panData.Controls.Add(this.txtReqId);
            this.panData.Controls.Add(this.txtReqNo);
            this.panData.Controls.Add(this.lblStatCd);
            this.panData.Controls.Add(this.cboStatCd);
            this.panData.Controls.Add(this.lblReqDate);
            this.panData.Controls.Add(this.dteReqDate);
            this.panData.Controls.Add(this.lblCurCd);
            this.panData.Controls.Add(this.cboCurCd);
            this.panData.Controls.Add(this.lblDeptNm);
            this.panData.Controls.Add(this.txtDeptNm);
            this.panData.Controls.Add(this.txtDeptId);
            this.panData.Controls.Add(this.lblEmpNm);
            this.panData.Controls.Add(this.txtEmpNm);
            this.panData.Controls.Add(this.txtEmpId);
            this.panData.Controls.Add(this.lblCustNm);
            this.panData.Controls.Add(this.txtCustNm);
            this.panData.Controls.Add(this.txtCustId);
            this.panData.Controls.Add(this.lblAppNo);
            this.panData.Controls.Add(this.txtAppNo);
            this.panData.Controls.Add(this.lblApprStatCd);
            this.panData.Controls.Add(this.lblSumAmt);
            this.panData.Controls.Add(this.txtSumAmt);
            this.panData.Controls.Add(this.lblSumVat);
            this.panData.Controls.Add(this.txtSumVat);
            this.panData.Controls.Add(this.lblSumTotalAmt);
            this.panData.Controls.Add(this.txtSumTotalAmt);
            this.panData.Controls.Add(this.lblReqTitle);
            this.panData.Controls.Add(this.txtReqTitle);
            this.panData.Controls.Add(this.lblRemark);
            this.panData.Controls.Add(this.memoRemark);
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 33);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(1664, 286);
            this.panData.TabIndex = 8;
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveAs.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnSaveAs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveAs.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnSaveAs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnSaveAs.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnSaveAs.Image = null;
            this.btnSaveAs.Location = new System.Drawing.Point(485, 9);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnSaveAs.Size = new System.Drawing.Size(87, 24);
            this.btnSaveAs.TabIndex = 0;
            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.ToolTip = null;
            // 
            // lblAccId
            // 
            this.lblAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblAccId.Appearance.Options.UseFont = true;
            this.lblAccId.Location = new System.Drawing.Point(36, 14);
            this.lblAccId.Name = "lblAccId";
            this.lblAccId.Size = new System.Drawing.Size(36, 15);
            this.lblAccId.TabIndex = 0;
            this.lblAccId.Text = "사업장";
            // 
            // cboAccId
            // 
            this.cboAccId.EditValue = "";
            this.cboAccId.Location = new System.Drawing.Point(79, 11);
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
            this.cboAccId.Size = new System.Drawing.Size(159, 20);
            this.cboAccId.TabIndex = 1;
            // 
            // lblPoType
            // 
            this.lblPoType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblPoType.Appearance.Options.UseFont = true;
            this.lblPoType.Location = new System.Drawing.Point(524, 43);
            this.lblPoType.Name = "lblPoType";
            this.lblPoType.Size = new System.Drawing.Size(48, 15);
            this.lblPoType.TabIndex = 2;
            this.lblPoType.Text = "발주구분";
            // 
            // cboApprStatCd
            // 
            this.cboApprStatCd.EditValue = "";
            this.cboApprStatCd.Enabled = false;
            this.cboApprStatCd.Location = new System.Drawing.Point(800, 69);
            this.cboApprStatCd.LookupKey = "L_AP0001";
            this.cboApprStatCd.Name = "cboApprStatCd";
            this.cboApprStatCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboApprStatCd.Properties.NullText = "";
            this.cboApprStatCd.Properties.ReadOnly = true;
            this.cboApprStatCd.Size = new System.Drawing.Size(100, 20);
            this.cboApprStatCd.TabIndex = 3;
            // 
            // txtExcRate
            // 
            this.txtExcRate.Location = new System.Drawing.Point(421, 69);
            this.txtExcRate.Name = "txtExcRate";
            this.txtExcRate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtExcRate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtExcRate.Size = new System.Drawing.Size(60, 20);
            this.txtExcRate.TabIndex = 47;
            // 
            // cboPoType
            // 
            this.cboPoType.EditValue = "";
            this.cboPoType.Location = new System.Drawing.Point(578, 40);
            this.cboPoType.LookupKey = "L_MA0003";
            this.cboPoType.Name = "cboPoType";
            this.cboPoType.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.cboPoType.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboPoType.Properties.Appearance.Options.UseBackColor = true;
            this.cboPoType.Properties.Appearance.Options.UseForeColor = true;
            this.cboPoType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboPoType.Properties.NullText = "";
            this.cboPoType.Required = true;
            this.cboPoType.Size = new System.Drawing.Size(141, 20);
            this.cboPoType.TabIndex = 3;
            // 
            // lblReqNo
            // 
            this.lblReqNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblReqNo.Appearance.Options.UseFont = true;
            this.lblReqNo.Location = new System.Drawing.Point(262, 14);
            this.lblReqNo.Name = "lblReqNo";
            this.lblReqNo.Size = new System.Drawing.Size(48, 15);
            this.lblReqNo.TabIndex = 4;
            this.lblReqNo.Text = "요청번호";
            // 
            // txtEmpNo
            // 
            this.txtEmpNo.Location = new System.Drawing.Point(421, 40);
            this.txtEmpNo.Name = "txtEmpNo";
            this.txtEmpNo.Properties.ReadOnly = true;
            this.txtEmpNo.Size = new System.Drawing.Size(60, 20);
            this.txtEmpNo.TabIndex = 5;
            // 
            // txtApprId
            // 
            this.txtApprId.Location = new System.Drawing.Point(666, 69);
            this.txtApprId.Name = "txtApprId";
            this.txtApprId.Properties.ReadOnly = true;
            this.txtApprId.Size = new System.Drawing.Size(53, 20);
            this.txtApprId.TabIndex = 5;
            // 
            // txtReqId
            // 
            this.txtReqId.Location = new System.Drawing.Point(421, 11);
            this.txtReqId.Name = "txtReqId";
            this.txtReqId.Properties.ReadOnly = true;
            this.txtReqId.Size = new System.Drawing.Size(60, 20);
            this.txtReqId.TabIndex = 5;
            // 
            // txtReqNo
            // 
            this.txtReqNo.Location = new System.Drawing.Point(315, 11);
            this.txtReqNo.Name = "txtReqNo";
            this.txtReqNo.Properties.ReadOnly = true;
            this.txtReqNo.Size = new System.Drawing.Size(104, 20);
            this.txtReqNo.TabIndex = 5;
            // 
            // lblStatCd
            // 
            this.lblStatCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblStatCd.Appearance.Options.UseFont = true;
            this.lblStatCd.Location = new System.Drawing.Point(743, 43);
            this.lblStatCd.Name = "lblStatCd";
            this.lblStatCd.Size = new System.Drawing.Size(48, 15);
            this.lblStatCd.TabIndex = 6;
            this.lblStatCd.Text = "진행상태";
            // 
            // cboStatCd
            // 
            this.cboStatCd.EditValue = "";
            this.cboStatCd.Location = new System.Drawing.Point(800, 40);
            this.cboStatCd.LookupKey = "L_CM0007";
            this.cboStatCd.Name = "cboStatCd";
            this.cboStatCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboStatCd.Properties.NullText = "";
            this.cboStatCd.Properties.ReadOnly = true;
            this.cboStatCd.Size = new System.Drawing.Size(100, 20);
            this.cboStatCd.TabIndex = 7;
            // 
            // lblReqDate
            // 
            this.lblReqDate.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblReqDate.Appearance.Options.UseFont = true;
            this.lblReqDate.Location = new System.Drawing.Point(743, 14);
            this.lblReqDate.Name = "lblReqDate";
            this.lblReqDate.Size = new System.Drawing.Size(48, 15);
            this.lblReqDate.TabIndex = 8;
            this.lblReqDate.Text = "요청일자";
            // 
            // dteReqDate
            // 
            this.dteReqDate.EditValue = new System.DateTime(2026, 9, 22, 0, 0, 0, 0);
            this.dteReqDate.Location = new System.Drawing.Point(800, 11);
            this.dteReqDate.Name = "dteReqDate";
            this.dteReqDate.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.dteReqDate.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.dteReqDate.Properties.Appearance.Options.UseBackColor = true;
            this.dteReqDate.Properties.Appearance.Options.UseForeColor = true;
            this.dteReqDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteReqDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dteReqDate.Required = true;
            this.dteReqDate.Size = new System.Drawing.Size(100, 20);
            this.dteReqDate.TabIndex = 9;
            this.dteReqDate.YyyyMmDd = "20260922";
            // 
            // lblCurCd
            // 
            this.lblCurCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblCurCd.Appearance.Options.UseFont = true;
            this.lblCurCd.Location = new System.Drawing.Point(286, 72);
            this.lblCurCd.Name = "lblCurCd";
            this.lblCurCd.Size = new System.Drawing.Size(24, 15);
            this.lblCurCd.TabIndex = 10;
            this.lblCurCd.Text = "통화";
            // 
            // cboCurCd
            // 
            this.cboCurCd.EditValue = "";
            this.cboCurCd.Location = new System.Drawing.Point(315, 69);
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
            this.cboCurCd.Size = new System.Drawing.Size(104, 20);
            this.cboCurCd.TabIndex = 11;
            // 
            // lblDeptNm
            // 
            this.lblDeptNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblDeptNm.Appearance.Options.UseFont = true;
            this.lblDeptNm.Location = new System.Drawing.Point(48, 43);
            this.lblDeptNm.Name = "lblDeptNm";
            this.lblDeptNm.Size = new System.Drawing.Size(24, 15);
            this.lblDeptNm.TabIndex = 12;
            this.lblDeptNm.Text = "부서";
            // 
            // txtDeptNm
            // 
            this.txtDeptNm.Location = new System.Drawing.Point(79, 40);
            this.txtDeptNm.LookupKey = "P_DEPT";
            this.txtDeptNm.MatchField = "dept_nm";
            this.txtDeptNm.Name = "txtDeptNm";
            this.txtDeptNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", 24, true, true, false, editorButtonImageOptions4, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject13, serializableAppearanceObject14, serializableAppearanceObject15, serializableAppearanceObject16, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.txtDeptNm.Size = new System.Drawing.Size(159, 20);
            this.txtDeptNm.TabIndex = 13;
            this.txtDeptNm.ToolTip = null;
            // 
            // txtDeptId
            // 
            this.txtDeptId.Location = new System.Drawing.Point(79, 40);
            this.txtDeptId.Name = "txtDeptId";
            this.txtDeptId.Size = new System.Drawing.Size(150, 20);
            this.txtDeptId.TabIndex = 14;
            this.txtDeptId.Visible = false;
            // 
            // lblEmpNm
            // 
            this.lblEmpNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblEmpNm.Appearance.Options.UseFont = true;
            this.lblEmpNm.Location = new System.Drawing.Point(274, 43);
            this.lblEmpNm.Name = "lblEmpNm";
            this.lblEmpNm.Size = new System.Drawing.Size(36, 15);
            this.lblEmpNm.TabIndex = 15;
            this.lblEmpNm.Text = "담당자";
            // 
            // txtEmpNm
            // 
            this.txtEmpNm.Location = new System.Drawing.Point(315, 40);
            this.txtEmpNm.LookupKey = "P_EMP";
            this.txtEmpNm.MatchField = "emp_nm";
            this.txtEmpNm.Name = "txtEmpNm";
            this.txtEmpNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", 24, true, true, false, editorButtonImageOptions5, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject17, serializableAppearanceObject18, serializableAppearanceObject19, serializableAppearanceObject20, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.txtEmpNm.Size = new System.Drawing.Size(104, 20);
            this.txtEmpNm.TabIndex = 16;
            this.txtEmpNm.ToolTip = null;
            // 
            // txtEmpId
            // 
            this.txtEmpId.Location = new System.Drawing.Point(1042, 410);
            this.txtEmpId.Name = "txtEmpId";
            this.txtEmpId.Size = new System.Drawing.Size(150, 20);
            this.txtEmpId.TabIndex = 17;
            this.txtEmpId.Visible = false;
            // 
            // lblCustNm
            // 
            this.lblCustNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblCustNm.Appearance.Options.UseFont = true;
            this.lblCustNm.Location = new System.Drawing.Point(36, 72);
            this.lblCustNm.Name = "lblCustNm";
            this.lblCustNm.Size = new System.Drawing.Size(36, 15);
            this.lblCustNm.TabIndex = 18;
            this.lblCustNm.Text = "거래처";
            // 
            // txtCustNm
            // 
            this.txtCustNm.Location = new System.Drawing.Point(79, 69);
            this.txtCustNm.LookupKey = "P_CUST";
            this.txtCustNm.PopupConditions = "p_cust_class=PO";
            this.txtCustNm.MatchField = "cust_nm";
            this.txtCustNm.Name = "txtCustNm";
            this.txtCustNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", 24, true, true, false, editorButtonImageOptions6, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject21, serializableAppearanceObject22, serializableAppearanceObject23, serializableAppearanceObject24, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.txtCustNm.Size = new System.Drawing.Size(159, 20);
            this.txtCustNm.TabIndex = 19;
            this.txtCustNm.ToolTip = null;
            // 
            // txtCustId
            // 
            this.txtCustId.Location = new System.Drawing.Point(79, 69);
            this.txtCustId.Name = "txtCustId";
            this.txtCustId.Size = new System.Drawing.Size(150, 20);
            this.txtCustId.TabIndex = 20;
            this.txtCustId.Visible = false;
            // 
            // lblAppNo
            // 
            this.lblAppNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblAppNo.Appearance.Options.UseFont = true;
            this.lblAppNo.Location = new System.Drawing.Point(524, 72);
            this.lblAppNo.Name = "lblAppNo";
            this.lblAppNo.Size = new System.Drawing.Size(48, 15);
            this.lblAppNo.TabIndex = 21;
            this.lblAppNo.Text = "결재번호";
            // 
            // txtAppNo
            // 
            this.txtAppNo.Location = new System.Drawing.Point(578, 69);
            this.txtAppNo.Name = "txtAppNo";
            this.txtAppNo.Properties.ReadOnly = true;
            this.txtAppNo.Size = new System.Drawing.Size(87, 20);
            this.txtAppNo.TabIndex = 22;
            // 
            // lblApprStatCd
            // 
            this.lblApprStatCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblApprStatCd.Appearance.Options.UseFont = true;
            this.lblApprStatCd.Location = new System.Drawing.Point(743, 72);
            this.lblApprStatCd.Name = "lblApprStatCd";
            this.lblApprStatCd.Size = new System.Drawing.Size(48, 15);
            this.lblApprStatCd.TabIndex = 23;
            this.lblApprStatCd.Text = "결재상태";
            // 
            // lblSumAmt
            // 
            this.lblSumAmt.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblSumAmt.Appearance.Options.UseFont = true;
            this.lblSumAmt.Location = new System.Drawing.Point(24, 101);
            this.lblSumAmt.Name = "lblSumAmt";
            this.lblSumAmt.Size = new System.Drawing.Size(48, 15);
            this.lblSumAmt.TabIndex = 40;
            this.lblSumAmt.Text = "공급가액";
            // 
            // txtSumAmt
            // 
            this.txtSumAmt.Location = new System.Drawing.Point(79, 98);
            this.txtSumAmt.Name = "txtSumAmt";
            this.txtSumAmt.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSumAmt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtSumAmt.Properties.DisplayFormat.FormatString = "#,##0.####";
            this.txtSumAmt.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtSumAmt.Properties.ReadOnly = true;
            this.txtSumAmt.Size = new System.Drawing.Size(159, 20);
            this.txtSumAmt.TabIndex = 41;
            // 
            // lblSumVat
            // 
            this.lblSumVat.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblSumVat.Appearance.Options.UseFont = true;
            this.lblSumVat.Location = new System.Drawing.Point(262, 101);
            this.lblSumVat.Name = "lblSumVat";
            this.lblSumVat.Size = new System.Drawing.Size(48, 15);
            this.lblSumVat.TabIndex = 42;
            this.lblSumVat.Text = "부가세액";
            // 
            // txtSumVat
            // 
            this.txtSumVat.Location = new System.Drawing.Point(315, 98);
            this.txtSumVat.Name = "txtSumVat";
            this.txtSumVat.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSumVat.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtSumVat.Properties.DisplayFormat.FormatString = "#,##0.####";
            this.txtSumVat.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtSumVat.Properties.ReadOnly = true;
            this.txtSumVat.Size = new System.Drawing.Size(166, 20);
            this.txtSumVat.TabIndex = 43;
            // 
            // lblSumTotalAmt
            // 
            this.lblSumTotalAmt.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblSumTotalAmt.Appearance.Options.UseFont = true;
            this.lblSumTotalAmt.Location = new System.Drawing.Point(524, 101);
            this.lblSumTotalAmt.Name = "lblSumTotalAmt";
            this.lblSumTotalAmt.Size = new System.Drawing.Size(48, 15);
            this.lblSumTotalAmt.TabIndex = 44;
            this.lblSumTotalAmt.Text = "합계금액";
            // 
            // txtSumTotalAmt
            // 
            this.txtSumTotalAmt.Location = new System.Drawing.Point(578, 98);
            this.txtSumTotalAmt.Name = "txtSumTotalAmt";
            this.txtSumTotalAmt.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSumTotalAmt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtSumTotalAmt.Properties.DisplayFormat.FormatString = "#,##0.####";
            this.txtSumTotalAmt.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtSumTotalAmt.Properties.ReadOnly = true;
            this.txtSumTotalAmt.Size = new System.Drawing.Size(141, 20);
            this.txtSumTotalAmt.TabIndex = 45;
            // 
            // lblReqTitle
            // 
            this.lblReqTitle.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblReqTitle.Appearance.Options.UseFont = true;
            this.lblReqTitle.Location = new System.Drawing.Point(12, 130);
            this.lblReqTitle.Name = "lblReqTitle";
            this.lblReqTitle.Size = new System.Drawing.Size(60, 15);
            this.lblReqTitle.TabIndex = 25;
            this.lblReqTitle.Text = "구매요청명";
            // 
            // txtReqTitle
            // 
            this.txtReqTitle.Location = new System.Drawing.Point(79, 127);
            this.txtReqTitle.Name = "txtReqTitle";
            this.txtReqTitle.Size = new System.Drawing.Size(821, 20);
            this.txtReqTitle.TabIndex = 26;
            // 
            // lblRemark
            // 
            this.lblRemark.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblRemark.Appearance.Options.UseFont = true;
            this.lblRemark.Location = new System.Drawing.Point(48, 156);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(24, 15);
            this.lblRemark.TabIndex = 27;
            this.lblRemark.Text = "비고";
            // 
            // memoRemark
            // 
            this.memoRemark.Location = new System.Drawing.Point(79, 155);
            this.memoRemark.Name = "memoRemark";
            this.memoRemark.Size = new System.Drawing.Size(821, 125);
            this.memoRemark.TabIndex = 28;
            // 
            // panelWyn2
            // 
            this.panelWyn2.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn2.Appearance.Options.UseBackColor = true;
            this.panelWyn2.Controls.Add(this.btnOpenApproval);
            this.panelWyn2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn2.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn2.Location = new System.Drawing.Point(0, 0);
            this.panelWyn2.Name = "panelWyn2";
            this.panelWyn2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn2.Size = new System.Drawing.Size(1664, 33);
            this.panelWyn2.TabIndex = 9;
            // 
            // btnOpenApproval
            // 
            this.btnOpenApproval.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenApproval.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnOpenApproval.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenApproval.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnOpenApproval.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnOpenApproval.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnOpenApproval.Image = null;
            this.btnOpenApproval.Location = new System.Drawing.Point(1, 4);
            this.btnOpenApproval.Name = "btnOpenApproval";
            this.btnOpenApproval.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnOpenApproval.Size = new System.Drawing.Size(100, 24);
            this.btnOpenApproval.TabIndex = 0;
            this.btnOpenApproval.Text = "전자결재";
            this.btnOpenApproval.ToolTip = null;
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.Controls.Add(this.lblSearchAccId);
            this.panHeader.Controls.Add(this.cboSearchAccId);
            this.panHeader.Controls.Add(this.lblSearchReqNo);
            this.panHeader.Controls.Add(this.txtSearchReqNo);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 33);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1670, 49);
            this.panHeader.TabIndex = 8;
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
            this.panHeader.Visible = false;
            // 
            // lblSearchReqNo
            // 
            this.lblSearchReqNo.Location = new System.Drawing.Point(224, 18);
            this.lblSearchReqNo.Name = "lblSearchReqNo";
            this.lblSearchReqNo.Size = new System.Drawing.Size(40, 14);
            this.lblSearchReqNo.TabIndex = 0;
            this.lblSearchReqNo.Text = "요청번호";
            // 
            // txtSearchReqNo
            // 
            this.txtSearchReqNo.Location = new System.Drawing.Point(289, 15);
            this.txtSearchReqNo.Name = "txtSearchReqNo";
            this.txtSearchReqNo.Size = new System.Drawing.Size(180, 20);
            this.txtSearchReqNo.TabIndex = 1;
            // 
            // paTitle
            // 
            this.paTitle.Appearance.BackColor = System.Drawing.Color.White;
            this.paTitle.Appearance.Options.UseBackColor = true;
            this.paTitle.Controls.Add(this.sectionHeaderWyn1);
            this.paTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle.Location = new System.Drawing.Point(5, 0);
            this.paTitle.Name = "paTitle";
            this.paTitle.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.paTitle.Size = new System.Drawing.Size(1670, 33);
            this.paTitle.TabIndex = 5;
            // 
            // sectionHeaderWyn1
            // 
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1665, 31);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 11;
            this.sectionHeaderWyn1.Text = "구매요청등록 [frmPoReq]";
            // 
            // frmPoReq
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1680, 760);
            this.Controls.Add(this.panBase);
            this.Name = "frmPoReq";
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
            ((System.ComponentModel.ISupportInitialize)(this.popcolCust)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolWh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolDelv.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolDelv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolSrcType)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.cboApprStatCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcRate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPoType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtApprId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReqId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReqNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteReqDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteReqDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCurCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAppNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSumAmt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSumVat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSumTotalAmt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReqTitle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
            this.panelWyn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchReqNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
            this.paTitle.ResumeLayout(false);
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
    private PopupLookupColumnEdit popcolItem;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colQty;
    private DevExpress.XtraGrid.Columns.GridColumn colNextQty;
    private DevExpress.XtraGrid.Columns.GridColumn colRemainQty;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitCd;
    private LookUpColumnEdit lookupcolUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colPrice;
    private DevExpress.XtraGrid.Columns.GridColumn colAmt;
    private DevExpress.XtraGrid.Columns.GridColumn colVatRate;
    private DevExpress.XtraGrid.Columns.GridColumn colVat;
    private DevExpress.XtraGrid.Columns.GridColumn colTotalAmt;
    private DevExpress.XtraGrid.Columns.GridColumn colKorPrice;
    private DevExpress.XtraGrid.Columns.GridColumn colKorAmt;
    private DevExpress.XtraGrid.Columns.GridColumn colKorVat;
    private DevExpress.XtraGrid.Columns.GridColumn colKorTotalAmt;
    private DevExpress.XtraGrid.Columns.GridColumn colCustId;
    private PopupLookupColumnEdit popcolCust;
    private DevExpress.XtraGrid.Columns.GridColumn colWhId;
    private PopupLookupColumnEdit popcolWh;
    private DevExpress.XtraGrid.Columns.GridColumn colWhNm;
    private DevExpress.XtraGrid.Columns.GridColumn colDelvDate;
    private DateColumnEdit datecolDelv;
    private DevExpress.XtraGrid.Columns.GridColumn colSrcType;
    private LookUpColumnEdit lookupcolSrcType;
    private DevExpress.XtraGrid.Columns.GridColumn colSrcNo;
    private PanelWyn panelWyn1;
    private PanelWyn panelWyn7;
    private ButtonWyn btnDeletRow1;
    private ButtonWyn btnAddRow1;
    private ButtonWyn btnOpenApproval;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblAccId;
    private LookUpEditWyn cboAccId;
    private DevExpress.XtraEditors.LabelControl lblPoType;
    private LookUpEditWyn cboPoType;
    private DevExpress.XtraEditors.LabelControl lblReqNo;
    private TextEditWyn txtReqNo;
    private DevExpress.XtraEditors.LabelControl lblStatCd;
    private LookUpEditWyn cboStatCd;
    private DevExpress.XtraEditors.LabelControl lblReqDate;
    private DateEditWyn dteReqDate;
    private DevExpress.XtraEditors.LabelControl lblCurCd;
    private LookUpEditWyn cboCurCd;
    private DevExpress.XtraEditors.LabelControl lblDeptNm;
    private PopupLookupEditWyn txtDeptNm;
    private TextEditWyn txtDeptId;
    private DevExpress.XtraEditors.LabelControl lblEmpNm;
    private PopupLookupEditWyn txtEmpNm;
    private TextEditWyn txtEmpId;
    private DevExpress.XtraEditors.LabelControl lblCustNm;
    private PopupLookupEditWyn txtCustNm;
    private TextEditWyn txtCustId;
    private DevExpress.XtraEditors.LabelControl lblAppNo;
    private TextEditWyn txtAppNo;
    private DevExpress.XtraEditors.LabelControl lblApprStatCd;
    private DevExpress.XtraEditors.LabelControl lblSumAmt;
    private TextEditWyn txtSumAmt;
    private DevExpress.XtraEditors.LabelControl lblSumVat;
    private TextEditWyn txtSumVat;
    private DevExpress.XtraEditors.LabelControl lblSumTotalAmt;
    private TextEditWyn txtSumTotalAmt;
    private DevExpress.XtraEditors.LabelControl lblReqTitle;
    private TextEditWyn txtReqTitle;
    private DevExpress.XtraEditors.LabelControl lblRemark;
    private DevExpress.XtraEditors.MemoEdit memoRemark;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchReqNo;
    private TextEditWyn txtSearchReqNo;
    private PanelWyn paTitle;
    private SectionHeaderWyn sectionHeaderWyn1;
    private SectionHeaderWyn sectionHeaderWyn4;
    private TextEditWyn txtReqId;
    private PanelWyn panelWyn2;
    private TextEditWyn txtApprId;
    private LookUpEditWyn cboApprStatCd;
    private TextEditWyn txtExcRate;
    private TextEditWyn txtEmpNo;
    private ButtonWyn btnSaveAs;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private WYNLAB.Base.Controls.LookUpEditWyn cboSearchAccId;
}
