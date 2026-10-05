// 수입검사등록(frmIqc) - frmPo와 같은 Master-One Sheet 구조(TMAIQCM/TMAIQCD 대상, 2026-09-25). 라인은 검사대기 불러오기로만 추가한다.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

public partial class frmIqc
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions6 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject21 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject22 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject23 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject24 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions7 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject25 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject26 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject27 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject28 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions8 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject29 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject30 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject31 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject32 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions3 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject9 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject10 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject11 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject12 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions4 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject13 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject14 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject15 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject16 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIqc));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colSerl = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLotNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPoNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDelvNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDelvQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInspQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSampleQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPassQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colConcQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFailQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFailReasonCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookupcolReason = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colFailActionCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookupcolAction = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colWhId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.popcolWh = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
            this.colLocId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.popcolLoc = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
            this.colNextQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnLoadDelv = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnDeletRow1 = new WYNLAB.Base.Controls.ButtonWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.lblAccId = new DevExpress.XtraEditors.LabelControl();
            this.cboAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblStatCd = new DevExpress.XtraEditors.LabelControl();
            this.cboStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblIqcNo = new DevExpress.XtraEditors.LabelControl();
            this.txtIqcNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblIqcDate = new DevExpress.XtraEditors.LabelControl();
            this.dteIqcDate = new WYNLAB.Base.Controls.DateEditWyn();
            this.lblCust = new DevExpress.XtraEditors.LabelControl();
            this.txtCustNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.txtCustId = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDept = new DevExpress.XtraEditors.LabelControl();
            this.txtDeptNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.txtDeptId = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblEmp = new DevExpress.XtraEditors.LabelControl();
            this.txtEmpNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.txtEmpId = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblCfmDt = new DevExpress.XtraEditors.LabelControl();
            this.txtCfmDt = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblCfmUserId = new DevExpress.XtraEditors.LabelControl();
            this.txtCfmUserId = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblRemark = new DevExpress.XtraEditors.LabelControl();
            this.memoRemark = new DevExpress.XtraEditors.MemoEdit();
            this.btnConfirm = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnConfirmCancel = new WYNLAB.Base.Controls.ButtonWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
            this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblSearchIqcNo = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchIqcNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.paTitle1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.paTitleH = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
            this.panelWyn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolReason)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolAction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolWh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolLoc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIqcNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteIqcDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteIqcDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCfmDt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCfmUserId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchIqcNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).BeginInit();
            this.paTitle1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).BeginInit();
            this.paTitleH.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
            this.panelWyn6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panBase
            // 
            this.panBase.Appearance.BackColor = System.Drawing.Color.White;
            this.panBase.Appearance.Options.UseBackColor = true;
            this.panBase.Controls.Add(this.panelWyn3);
            this.panBase.Controls.Add(this.panelWyn5);
            this.panBase.Controls.Add(this.panHeader);
            this.panBase.Controls.Add(this.paTitleH);
            this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBase.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panBase.Location = new System.Drawing.Point(0, 0);
            this.panBase.Name = "panBase";
            this.panBase.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.panBase.Size = new System.Drawing.Size(1680, 800);
            this.panBase.TabIndex = 6;
            // 
            // panelWyn3
            // 
            this.panelWyn3.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn3.Appearance.Options.UseBackColor = true;
            this.panelWyn3.Controls.Add(this.grd1);
            this.panelWyn3.Controls.Add(this.panelWyn7);
            this.panelWyn3.Controls.Add(this.paTitle1);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(5, 296);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.panelWyn3.Size = new System.Drawing.Size(1670, 499);
            this.panelWyn3.TabIndex = 7;
            // 
            // grd1
            // 
            this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd1.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grd1.Location = new System.Drawing.Point(0, 62);
            this.grd1.MainView = this.gvw1;
            this.grd1.Name = "grd1";
            this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.popcolWh,
            this.popcolLoc,
            this.lookupcolReason,
            this.lookupcolAction});
            this.grd1.Size = new System.Drawing.Size(1670, 437);
            this.grd1.TabIndex = 0;
            this.grd1.UseEmbeddedNavigator = false;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colSerl,
            this.colItemNo,
            this.colItemNm,
            this.colItemSpec,
            this.colUnitCd,
            this.colLotNo,
            this.colPoNo,
            this.colDelvNo,
            this.colDelvQty,
            this.colInspQty,
            this.colSampleQty,
            this.colPassQty,
            this.colConcQty,
            this.colFailQty,
            this.colFailReasonCd,
            this.colFailActionCd,
            this.colWhId,
            this.colLocId,
            this.colNextQty,
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
            // colItemNo
            // 
            this.colItemNo.Caption = "품번";
            this.colItemNo.FieldName = "item_no";
            this.colItemNo.Name = "colItemNo";
            this.colItemNo.OptionsColumn.AllowEdit = false;
            this.colItemNo.Visible = true;
            this.colItemNo.VisibleIndex = 1;
            this.colItemNo.Width = 100;
            // 
            // colItemNm
            // 
            this.colItemNm.Caption = "품명";
            this.colItemNm.FieldName = "item_nm";
            this.colItemNm.Name = "colItemNm";
            this.colItemNm.OptionsColumn.AllowEdit = false;
            this.colItemNm.Visible = true;
            this.colItemNm.VisibleIndex = 2;
            this.colItemNm.Width = 140;
            // 
            // colItemSpec
            // 
            this.colItemSpec.Caption = "규격";
            this.colItemSpec.FieldName = "item_spec";
            this.colItemSpec.Name = "colItemSpec";
            this.colItemSpec.OptionsColumn.AllowEdit = false;
            this.colItemSpec.Visible = true;
            this.colItemSpec.VisibleIndex = 3;
            this.colItemSpec.Width = 110;
            // 
            // colUnitCd
            // 
            this.colUnitCd.Caption = "단위";
            this.colUnitCd.FieldName = "unit_cd";
            this.colUnitCd.Name = "colUnitCd";
            this.colUnitCd.OptionsColumn.AllowEdit = false;
            this.colUnitCd.Visible = true;
            this.colUnitCd.VisibleIndex = 4;
            this.colUnitCd.Width = 50;
            // 
            // colLotNo
            // 
            this.colLotNo.Caption = "LOT";
            this.colLotNo.FieldName = "lot_no";
            this.colLotNo.Name = "colLotNo";
            this.colLotNo.OptionsColumn.AllowEdit = false;
            this.colLotNo.Visible = true;
            this.colLotNo.VisibleIndex = 5;
            this.colLotNo.Width = 90;
            // 
            // colPoNo
            // 
            this.colPoNo.Caption = "발주번호";
            this.colPoNo.FieldName = "po_no";
            this.colPoNo.Name = "colPoNo";
            this.colPoNo.OptionsColumn.AllowEdit = false;
            this.colPoNo.Visible = true;
            this.colPoNo.VisibleIndex = 6;
            this.colPoNo.Width = 110;
            // 
            // colDelvNo
            // 
            this.colDelvNo.Caption = "납품번호";
            this.colDelvNo.FieldName = "src_no";
            this.colDelvNo.Name = "colDelvNo";
            this.colDelvNo.OptionsColumn.AllowEdit = false;
            this.colDelvNo.Visible = true;
            this.colDelvNo.VisibleIndex = 7;
            this.colDelvNo.Width = 110;
            // 
            // colDelvQty
            // 
            this.colDelvQty.Caption = "납품수량";
            this.colDelvQty.DisplayFormat.FormatString = "#,##0.####";
            this.colDelvQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDelvQty.FieldName = "delv_qty";
            this.colDelvQty.Name = "colDelvQty";
            this.colDelvQty.OptionsColumn.AllowEdit = false;
            this.colDelvQty.Visible = true;
            this.colDelvQty.VisibleIndex = 8;
            this.colDelvQty.Width = 80;
            // 
            // colInspQty
            // 
            this.colInspQty.Caption = "검사수량";
            this.colInspQty.DisplayFormat.FormatString = "#,##0.####";
            this.colInspQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colInspQty.FieldName = "insp_qty";
            this.colInspQty.Name = "colInspQty";
            this.colInspQty.Visible = true;
            this.colInspQty.VisibleIndex = 9;
            this.colInspQty.Width = 80;
            // 
            // colSampleQty
            // 
            this.colSampleQty.Caption = "표본수";
            this.colSampleQty.DisplayFormat.FormatString = "#,##0.####";
            this.colSampleQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colSampleQty.FieldName = "sample_qty";
            this.colSampleQty.Name = "colSampleQty";
            this.colSampleQty.Visible = true;
            this.colSampleQty.VisibleIndex = 10;
            this.colSampleQty.Width = 70;
            // 
            // colPassQty
            // 
            this.colPassQty.Caption = "합격";
            this.colPassQty.DisplayFormat.FormatString = "#,##0.####";
            this.colPassQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colPassQty.FieldName = "pass_qty";
            this.colPassQty.Name = "colPassQty";
            this.colPassQty.Visible = true;
            this.colPassQty.VisibleIndex = 11;
            this.colPassQty.Width = 70;
            // 
            // colConcQty
            // 
            this.colConcQty.Caption = "특채";
            this.colConcQty.DisplayFormat.FormatString = "#,##0.####";
            this.colConcQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colConcQty.FieldName = "conc_qty";
            this.colConcQty.Name = "colConcQty";
            this.colConcQty.Visible = true;
            this.colConcQty.VisibleIndex = 12;
            this.colConcQty.Width = 70;
            // 
            // colFailQty
            // 
            this.colFailQty.Caption = "불합격";
            this.colFailQty.DisplayFormat.FormatString = "#,##0.####";
            this.colFailQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colFailQty.FieldName = "fail_qty";
            this.colFailQty.Name = "colFailQty";
            this.colFailQty.Visible = true;
            this.colFailQty.VisibleIndex = 13;
            this.colFailQty.Width = 70;
            // 
            // colFailReasonCd
            // 
            this.colFailReasonCd.Caption = "불량유형";
            this.colFailReasonCd.ColumnEdit = this.lookupcolReason;
            this.colFailReasonCd.FieldName = "fail_reason_cd";
            this.colFailReasonCd.Name = "colFailReasonCd";
            this.colFailReasonCd.Visible = true;
            this.colFailReasonCd.VisibleIndex = 14;
            this.colFailReasonCd.Width = 90;
            // 
            // lookupcolReason
            // 
            this.lookupcolReason.AutoHeight = false;
            this.lookupcolReason.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookupcolReason.LookupKey = "L_MA0007";
            this.lookupcolReason.Name = "lookupcolReason";
            this.lookupcolReason.NullText = "";
            this.lookupcolReason.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colFailActionCd
            // 
            this.colFailActionCd.Caption = "불합격처분";
            this.colFailActionCd.ColumnEdit = this.lookupcolAction;
            this.colFailActionCd.FieldName = "fail_action_cd";
            this.colFailActionCd.Name = "colFailActionCd";
            this.colFailActionCd.Visible = true;
            this.colFailActionCd.VisibleIndex = 15;
            this.colFailActionCd.Width = 110;
            // 
            // lookupcolAction
            // 
            this.lookupcolAction.AutoHeight = false;
            this.lookupcolAction.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookupcolAction.LookupKey = "L_MA0008";
            this.lookupcolAction.Name = "lookupcolAction";
            this.lookupcolAction.NullText = "";
            this.lookupcolAction.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colWhId
            // 
            this.colWhId.Caption = "창고";
            this.colWhId.ColumnEdit = this.popcolWh;
            this.colWhId.FieldName = "wh_id";
            this.colWhId.Name = "colWhId";
            this.colWhId.Visible = true;
            this.colWhId.VisibleIndex = 16;
            this.colWhId.Width = 90;
            // 
            // popcolWh
            // 
            this.popcolWh.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions6, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject21, serializableAppearanceObject22, serializableAppearanceObject23, serializableAppearanceObject24, "...", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
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
            this.colLocId.VisibleIndex = 17;
            this.colLocId.Width = 90;
            // 
            // popcolLoc
            // 
            this.popcolLoc.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions7, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject25, serializableAppearanceObject26, serializableAppearanceObject27, serializableAppearanceObject28, "...", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.popcolLoc.LookupKey = "P_LOC";
            this.popcolLoc.Name = "popcolLoc";
            // 
            // colNextQty
            // 
            this.colNextQty.Caption = "입고처리";
            this.colNextQty.DisplayFormat.FormatString = "#,##0.####";
            this.colNextQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colNextQty.FieldName = "next_qty";
            this.colNextQty.Name = "colNextQty";
            this.colNextQty.OptionsColumn.AllowEdit = false;
            this.colNextQty.Visible = true;
            this.colNextQty.VisibleIndex = 18;
            this.colNextQty.Width = 80;
            // 
            // colRemark
            // 
            this.colRemark.Caption = "비고(특채사유)";
            this.colRemark.FieldName = "remark";
            this.colRemark.Name = "colRemark";
            this.colRemark.Visible = true;
            this.colRemark.VisibleIndex = 19;
            this.colRemark.Width = 180;
            // 
            // panelWyn7
            // 
            this.panelWyn7.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn7.Appearance.Options.UseBackColor = true;
            this.panelWyn7.Controls.Add(this.btnLoadDelv);
            this.panelWyn7.Controls.Add(this.btnDeletRow1);
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(0, 32);
            this.panelWyn7.Name = "panelWyn7";
            this.panelWyn7.Padding = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.panelWyn7.Size = new System.Drawing.Size(1670, 30);
            this.panelWyn7.TabIndex = 9;
            // 
            // btnLoadDelv
            // 
            this.btnLoadDelv.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadDelv.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnLoadDelv.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLoadDelv.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnLoadDelv.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnLoadDelv.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnLoadDelv.Image = null;
            this.btnLoadDelv.Location = new System.Drawing.Point(68, 3);
            this.btnLoadDelv.Name = "btnLoadDelv";
            this.btnLoadDelv.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnLoadDelv.Size = new System.Drawing.Size(120, 24);
            this.btnLoadDelv.TabIndex = 1;
            this.btnLoadDelv.Text = "검사대기 불러오기";
            this.btnLoadDelv.ToolTip = "확정된 납품 중 검사대상 미검사 잔량을 불러옵니다";
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
            this.btnDeletRow1.Location = new System.Drawing.Point(6, 3);
            this.btnDeletRow1.Name = "btnDeletRow1";
            this.btnDeletRow1.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDeletRow1.Size = new System.Drawing.Size(60, 24);
            this.btnDeletRow1.TabIndex = 0;
            this.btnDeletRow1.Text = "행삭제";
            this.btnDeletRow1.ToolTip = "행삭제";
            // 
            // panelWyn5
            // 
            this.panelWyn5.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn5.Appearance.Options.UseBackColor = true;
            this.panelWyn5.Controls.Add(this.panData);
            this.panelWyn5.Controls.Add(this.panelWyn6);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(5, 74);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Size = new System.Drawing.Size(1670, 222);
            this.panelWyn5.TabIndex = 6;
            // 
            // panData
            // 
            this.panData.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panData.Appearance.Options.UseBackColor = true;
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Controls.Add(this.lblAccId);
            this.panData.Controls.Add(this.cboAccId);
            this.panData.Controls.Add(this.lblStatCd);
            this.panData.Controls.Add(this.cboStatCd);
            this.panData.Controls.Add(this.lblIqcNo);
            this.panData.Controls.Add(this.txtIqcNo);
            this.panData.Controls.Add(this.lblIqcDate);
            this.panData.Controls.Add(this.dteIqcDate);
            this.panData.Controls.Add(this.lblCust);
            this.panData.Controls.Add(this.txtCustNm);
            this.panData.Controls.Add(this.txtCustId);
            this.panData.Controls.Add(this.lblDept);
            this.panData.Controls.Add(this.txtDeptNm);
            this.panData.Controls.Add(this.txtDeptId);
            this.panData.Controls.Add(this.lblEmp);
            this.panData.Controls.Add(this.txtEmpNm);
            this.panData.Controls.Add(this.txtEmpId);
            this.panData.Controls.Add(this.lblCfmDt);
            this.panData.Controls.Add(this.txtCfmDt);
            this.panData.Controls.Add(this.lblCfmUserId);
            this.panData.Controls.Add(this.txtCfmUserId);
            this.panData.Controls.Add(this.lblRemark);
            this.panData.Controls.Add(this.memoRemark);
            this.panData.Controls.Add(this.btnConfirm);
            this.panData.Controls.Add(this.btnConfirmCancel);
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(1670, 195);
            this.panData.TabIndex = 8;
            // 
            // lblAccId
            // 
            this.lblAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblAccId.Appearance.Options.UseFont = true;
            this.lblAccId.Location = new System.Drawing.Point(32, 15);
            this.lblAccId.Name = "lblAccId";
            this.lblAccId.Size = new System.Drawing.Size(36, 15);
            this.lblAccId.TabIndex = 2;
            this.lblAccId.Text = "사업장";
            // 
            // cboAccId
            // 
            this.cboAccId.EditValue = "";
            this.cboAccId.Location = new System.Drawing.Point(74, 12);
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
            this.cboAccId.Size = new System.Drawing.Size(150, 20);
            this.cboAccId.TabIndex = 3;
            // 
            // lblStatCd
            // 
            this.lblStatCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblStatCd.Appearance.Options.UseFont = true;
            this.lblStatCd.Location = new System.Drawing.Point(270, 15);
            this.lblStatCd.Name = "lblStatCd";
            this.lblStatCd.Size = new System.Drawing.Size(48, 15);
            this.lblStatCd.TabIndex = 4;
            this.lblStatCd.Text = "진행상태";
            // 
            // cboStatCd
            // 
            this.cboStatCd.EditValue = "";
            this.cboStatCd.Location = new System.Drawing.Point(350, 12);
            this.cboStatCd.LookupKey = "L_MA0006";
            this.cboStatCd.Name = "cboStatCd";
            this.cboStatCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboStatCd.Properties.NullText = "";
            this.cboStatCd.Properties.ReadOnly = true;
            this.cboStatCd.Size = new System.Drawing.Size(150, 20);
            this.cboStatCd.TabIndex = 5;
            // 
            // lblIqcNo
            // 
            this.lblIqcNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblIqcNo.Appearance.Options.UseFont = true;
            this.lblIqcNo.Location = new System.Drawing.Point(20, 43);
            this.lblIqcNo.Name = "lblIqcNo";
            this.lblIqcNo.Size = new System.Drawing.Size(48, 15);
            this.lblIqcNo.TabIndex = 6;
            this.lblIqcNo.Text = "검사번호";
            // 
            // txtIqcNo
            // 
            this.txtIqcNo.Location = new System.Drawing.Point(74, 40);
            this.txtIqcNo.Name = "txtIqcNo";
            this.txtIqcNo.Properties.ReadOnly = true;
            this.txtIqcNo.Size = new System.Drawing.Size(150, 20);
            this.txtIqcNo.TabIndex = 7;
            // 
            // lblIqcDate
            // 
            this.lblIqcDate.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblIqcDate.Appearance.Options.UseFont = true;
            this.lblIqcDate.Location = new System.Drawing.Point(270, 43);
            this.lblIqcDate.Name = "lblIqcDate";
            this.lblIqcDate.Size = new System.Drawing.Size(48, 15);
            this.lblIqcDate.TabIndex = 8;
            this.lblIqcDate.Text = "검사일자";
            // 
            // dteIqcDate
            // 
            this.dteIqcDate.EditValue = new System.DateTime(2026, 9, 27, 0, 0, 0, 0);
            this.dteIqcDate.Location = new System.Drawing.Point(350, 40);
            this.dteIqcDate.Name = "dteIqcDate";
            this.dteIqcDate.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.dteIqcDate.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.dteIqcDate.Properties.Appearance.Options.UseBackColor = true;
            this.dteIqcDate.Properties.Appearance.Options.UseForeColor = true;
            this.dteIqcDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteIqcDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dteIqcDate.Required = true;
            this.dteIqcDate.Size = new System.Drawing.Size(150, 20);
            this.dteIqcDate.TabIndex = 9;
            this.dteIqcDate.YyyyMmDd = "20260927";
            // 
            // lblCust
            // 
            this.lblCust.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblCust.Appearance.Options.UseFont = true;
            this.lblCust.Location = new System.Drawing.Point(32, 71);
            this.lblCust.Name = "lblCust";
            this.lblCust.Size = new System.Drawing.Size(36, 15);
            this.lblCust.TabIndex = 10;
            this.lblCust.Text = "거래처";
            // 
            // txtCustNm
            // 
            this.txtCustNm.Location = new System.Drawing.Point(74, 68);
            this.txtCustNm.LookupKey = "P_CUST";
            this.txtCustNm.PopupConditions = "p_cust_class=PO";
            this.txtCustNm.MatchField = "cust_nm";
            this.txtCustNm.Name = "txtCustNm";
            this.txtCustNm.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtCustNm.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCustNm.Properties.Appearance.Options.UseBackColor = true;
            this.txtCustNm.Properties.Appearance.Options.UseForeColor = true;
            this.txtCustNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", 24, true, true, false, editorButtonImageOptions8, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject29, serializableAppearanceObject30, serializableAppearanceObject31, serializableAppearanceObject32, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.txtCustNm.Required = true;
            this.txtCustNm.Size = new System.Drawing.Size(150, 20);
            this.txtCustNm.TabIndex = 11;
            this.txtCustNm.ToolTip = null;
            // 
            // txtCustId
            // 
            this.txtCustId.Location = new System.Drawing.Point(74, 68);
            this.txtCustId.Name = "txtCustId";
            this.txtCustId.Size = new System.Drawing.Size(150, 20);
            this.txtCustId.TabIndex = 12;
            this.txtCustId.Visible = false;
            // 
            // lblDept
            // 
            this.lblDept.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblDept.Appearance.Options.UseFont = true;
            this.lblDept.Location = new System.Drawing.Point(44, 99);
            this.lblDept.Name = "lblDept";
            this.lblDept.Size = new System.Drawing.Size(24, 15);
            this.lblDept.TabIndex = 13;
            this.lblDept.Text = "부서";
            // 
            // txtDeptNm
            // 
            this.txtDeptNm.Location = new System.Drawing.Point(74, 96);
            this.txtDeptNm.LookupKey = "P_DEPT";
            this.txtDeptNm.MatchField = "dept_nm";
            this.txtDeptNm.Name = "txtDeptNm";
            this.txtDeptNm.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtDeptNm.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDeptNm.Properties.Appearance.Options.UseBackColor = true;
            this.txtDeptNm.Properties.Appearance.Options.UseForeColor = true;
            this.txtDeptNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", 24, true, true, false, editorButtonImageOptions3, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject9, serializableAppearanceObject10, serializableAppearanceObject11, serializableAppearanceObject12, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.txtDeptNm.Required = true;
            this.txtDeptNm.Size = new System.Drawing.Size(150, 20);
            this.txtDeptNm.TabIndex = 14;
            this.txtDeptNm.ToolTip = null;
            // 
            // txtDeptId
            // 
            this.txtDeptId.Location = new System.Drawing.Point(74, 96);
            this.txtDeptId.Name = "txtDeptId";
            this.txtDeptId.Size = new System.Drawing.Size(150, 20);
            this.txtDeptId.TabIndex = 15;
            this.txtDeptId.Visible = false;
            // 
            // lblEmp
            // 
            this.lblEmp.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblEmp.Appearance.Options.UseFont = true;
            this.lblEmp.Location = new System.Drawing.Point(282, 99);
            this.lblEmp.Name = "lblEmp";
            this.lblEmp.Size = new System.Drawing.Size(36, 15);
            this.lblEmp.TabIndex = 16;
            this.lblEmp.Text = "검사자";
            // 
            // txtEmpNm
            // 
            this.txtEmpNm.Location = new System.Drawing.Point(350, 96);
            this.txtEmpNm.LookupKey = "P_EMP";
            this.txtEmpNm.MatchField = "emp_nm";
            this.txtEmpNm.Name = "txtEmpNm";
            this.txtEmpNm.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtEmpNm.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtEmpNm.Properties.Appearance.Options.UseBackColor = true;
            this.txtEmpNm.Properties.Appearance.Options.UseForeColor = true;
            this.txtEmpNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", 24, true, true, false, editorButtonImageOptions4, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject13, serializableAppearanceObject14, serializableAppearanceObject15, serializableAppearanceObject16, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.txtEmpNm.Required = true;
            this.txtEmpNm.Size = new System.Drawing.Size(150, 20);
            this.txtEmpNm.TabIndex = 17;
            this.txtEmpNm.ToolTip = null;
            // 
            // txtEmpId
            // 
            this.txtEmpId.Location = new System.Drawing.Point(350, 96);
            this.txtEmpId.Name = "txtEmpId";
            this.txtEmpId.Size = new System.Drawing.Size(150, 20);
            this.txtEmpId.TabIndex = 18;
            this.txtEmpId.Visible = false;
            // 
            // lblCfmDt
            // 
            this.lblCfmDt.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblCfmDt.Appearance.Options.UseFont = true;
            this.lblCfmDt.Location = new System.Drawing.Point(20, 127);
            this.lblCfmDt.Name = "lblCfmDt";
            this.lblCfmDt.Size = new System.Drawing.Size(48, 15);
            this.lblCfmDt.TabIndex = 19;
            this.lblCfmDt.Text = "확정일시";
            // 
            // txtCfmDt
            // 
            this.txtCfmDt.Location = new System.Drawing.Point(74, 124);
            this.txtCfmDt.Name = "txtCfmDt";
            this.txtCfmDt.Properties.ReadOnly = true;
            this.txtCfmDt.Size = new System.Drawing.Size(150, 20);
            this.txtCfmDt.TabIndex = 20;
            // 
            // lblCfmUserId
            // 
            this.lblCfmUserId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblCfmUserId.Appearance.Options.UseFont = true;
            this.lblCfmUserId.Location = new System.Drawing.Point(282, 127);
            this.lblCfmUserId.Name = "lblCfmUserId";
            this.lblCfmUserId.Size = new System.Drawing.Size(36, 15);
            this.lblCfmUserId.TabIndex = 21;
            this.lblCfmUserId.Text = "확정자";
            // 
            // txtCfmUserId
            // 
            this.txtCfmUserId.Location = new System.Drawing.Point(350, 124);
            this.txtCfmUserId.Name = "txtCfmUserId";
            this.txtCfmUserId.Properties.ReadOnly = true;
            this.txtCfmUserId.Size = new System.Drawing.Size(150, 20);
            this.txtCfmUserId.TabIndex = 22;
            // 
            // lblRemark
            // 
            this.lblRemark.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblRemark.Appearance.Options.UseFont = true;
            this.lblRemark.Location = new System.Drawing.Point(44, 155);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(24, 15);
            this.lblRemark.TabIndex = 23;
            this.lblRemark.Text = "비고";
            // 
            // memoRemark
            // 
            this.memoRemark.Location = new System.Drawing.Point(74, 152);
            this.memoRemark.Name = "memoRemark";
            this.memoRemark.Size = new System.Drawing.Size(600, 25);
            this.memoRemark.TabIndex = 24;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfirm.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnConfirm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnConfirm.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnConfirm.Image = null;
            this.btnConfirm.Location = new System.Drawing.Point(566, 12);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnConfirm.Size = new System.Drawing.Size(100, 24);
            this.btnConfirm.TabIndex = 25;
            this.btnConfirm.Text = "확정";
            this.btnConfirm.ToolTip = "검사 판정을 확정합니다. 확정하면 입고대기 수량에 반영됩니다.";
            // 
            // btnConfirmCancel
            // 
            this.btnConfirmCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirmCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirmCancel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnConfirmCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfirmCancel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnConfirmCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnConfirmCancel.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnConfirmCancel.Image = null;
            this.btnConfirmCancel.Location = new System.Drawing.Point(566, 40);
            this.btnConfirmCancel.Name = "btnConfirmCancel";
            this.btnConfirmCancel.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnConfirmCancel.Size = new System.Drawing.Size(100, 24);
            this.btnConfirmCancel.TabIndex = 26;
            this.btnConfirmCancel.Text = "확정취소";
            this.btnConfirmCancel.ToolTip = "확정을 취소합니다(입고가 진행된 검사는 불가).";
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.lblSearchAccId);
            this.panHeader.Controls.Add(this.cboSearchAccId);
            this.panHeader.Controls.Add(this.lblSearchIqcNo);
            this.panHeader.Controls.Add(this.txtSearchIqcNo);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 25);
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
            // 
            // lblSearchIqcNo
            // 
            this.lblSearchIqcNo.Location = new System.Drawing.Point(224, 18);
            this.lblSearchIqcNo.Name = "lblSearchIqcNo";
            this.lblSearchIqcNo.Size = new System.Drawing.Size(40, 14);
            this.lblSearchIqcNo.TabIndex = 27;
            this.lblSearchIqcNo.Text = "검사번호";
            // 
            // txtSearchIqcNo
            // 
            this.txtSearchIqcNo.Location = new System.Drawing.Point(280, 15);
            this.txtSearchIqcNo.Name = "txtSearchIqcNo";
            this.txtSearchIqcNo.Size = new System.Drawing.Size(180, 20);
            this.txtSearchIqcNo.TabIndex = 28;
            // 
            // paTitle1
            // 
            this.paTitle1.Appearance.BackColor = System.Drawing.Color.White;
            this.paTitle1.Appearance.Options.UseBackColor = true;
            this.paTitle1.Controls.Add(this.sectionHeaderWyn4);
            this.paTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle1.Location = new System.Drawing.Point(0, 5);
            this.paTitle1.Name = "paTitle1";
            this.paTitle1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.paTitle1.Size = new System.Drawing.Size(1670, 27);
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
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(1665, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 8;
            this.sectionHeaderWyn4.Text = "수입검사 품목 상세정보";
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
            this.paTitleH.Size = new System.Drawing.Size(1670, 25);
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
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1665, 25);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 11;
            this.sectionHeaderWyn1.Text = "수입검사등록 [frmIqc]";
            // 
            // panelWyn6
            // 
            this.panelWyn6.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn6.Appearance.Options.UseBackColor = true;
            this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
            this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn6.Location = new System.Drawing.Point(0, 0);
            this.panelWyn6.Name = "panelWyn6";
            this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn6.Size = new System.Drawing.Size(1670, 27);
            this.panelWyn6.TabIndex = 8;
            // 
            // sectionHeaderWyn3
            // 
            this.sectionHeaderWyn3.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn3.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn3.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn3.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn3.Name = "sectionHeaderWyn3";
            this.sectionHeaderWyn3.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.sectionHeaderWyn3.Size = new System.Drawing.Size(1665, 25);
            this.sectionHeaderWyn3.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn3.SvgIcon")));
            this.sectionHeaderWyn3.TabIndex = 9;
            this.sectionHeaderWyn3.Text = "수입검사 등록";
            // 
            // frmIqc
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1680, 800);
            this.Controls.Add(this.panBase);
            this.Name = "frmIqc";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolReason)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolAction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolWh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolLoc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
            this.panelWyn7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
            this.panData.ResumeLayout(false);
            this.panData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIqcNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteIqcDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteIqcDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCfmDt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCfmUserId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchIqcNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).EndInit();
            this.paTitle1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).EndInit();
            this.paTitleH.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
            this.panelWyn6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
            this.ResumeLayout(false);

    }

    private PanelWyn panBase;
    private PanelWyn panelWyn3;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private PopupLookupColumnEdit popcolWh;
    private PopupLookupColumnEdit popcolLoc;
    private LookUpColumnEdit lookupcolReason;
    private LookUpColumnEdit lookupcolAction;
    private DevExpress.XtraGrid.Columns.GridColumn colSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colLotNo;
    private DevExpress.XtraGrid.Columns.GridColumn colPoNo;
    private DevExpress.XtraGrid.Columns.GridColumn colDelvNo;
    private DevExpress.XtraGrid.Columns.GridColumn colDelvQty;
    private DevExpress.XtraGrid.Columns.GridColumn colInspQty;
    private DevExpress.XtraGrid.Columns.GridColumn colSampleQty;
    private DevExpress.XtraGrid.Columns.GridColumn colPassQty;
    private DevExpress.XtraGrid.Columns.GridColumn colConcQty;
    private DevExpress.XtraGrid.Columns.GridColumn colFailQty;
    private DevExpress.XtraGrid.Columns.GridColumn colFailReasonCd;
    private DevExpress.XtraGrid.Columns.GridColumn colFailActionCd;
    private DevExpress.XtraGrid.Columns.GridColumn colWhId;
    private DevExpress.XtraGrid.Columns.GridColumn colLocId;
    private DevExpress.XtraGrid.Columns.GridColumn colNextQty;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private PanelWyn panelWyn7;
    private ButtonWyn btnDeletRow1;
    private ButtonWyn btnLoadDelv;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblAccId;
    private LookUpEditWyn cboAccId;
    private DevExpress.XtraEditors.LabelControl lblStatCd;
    private LookUpEditWyn cboStatCd;
    private DevExpress.XtraEditors.LabelControl lblIqcNo;
    private TextEditWyn txtIqcNo;
    private DevExpress.XtraEditors.LabelControl lblIqcDate;
    private DateEditWyn dteIqcDate;
    private DevExpress.XtraEditors.LabelControl lblCust;
    private PopupLookupEditWyn txtCustNm;
    private TextEditWyn txtCustId;
    private DevExpress.XtraEditors.LabelControl lblDept;
    private PopupLookupEditWyn txtDeptNm;
    private TextEditWyn txtDeptId;
    private DevExpress.XtraEditors.LabelControl lblEmp;
    private PopupLookupEditWyn txtEmpNm;
    private TextEditWyn txtEmpId;
    private DevExpress.XtraEditors.LabelControl lblCfmDt;
    private TextEditWyn txtCfmDt;
    private DevExpress.XtraEditors.LabelControl lblCfmUserId;
    private TextEditWyn txtCfmUserId;
    private DevExpress.XtraEditors.LabelControl lblRemark;
    private DevExpress.XtraEditors.MemoEdit memoRemark;
    private ButtonWyn btnConfirm;
    private ButtonWyn btnConfirmCancel;
    private DevExpress.XtraEditors.LabelControl lblSearchIqcNo;
    private TextEditWyn txtSearchIqcNo;
    private PanelWyn panHeader;
    private PanelWyn paTitle1;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn panelWyn6;
    private SectionHeaderWyn sectionHeaderWyn3;
    private PanelWyn paTitleH;
    private SectionHeaderWyn sectionHeaderWyn1;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private WYNLAB.Base.Controls.LookUpEditWyn cboSearchAccId;
}
