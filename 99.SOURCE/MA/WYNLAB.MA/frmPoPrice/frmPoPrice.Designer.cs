// 구매단가등록 - 품목 마스터 중심 구조(2026-09-26 변경): 왼쪽 grd1 = 품목 목록(+최종단가), 오른쪽 = panData(선택한 품목 정보) + 그 아래 grd2(품목의 단가 등록).
// 사원등록(frmEmp) 표준 배치(제목 바/카드/헤더 아이콘)를 따름. VS 디자이너로 자유롭게 편집 가능합니다.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

public partial class frmPoPrice
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPoPrice));
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colMItemId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMPoUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookupcolMPoUnit = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colMLastPrice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMLastCurCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookupcolMCur = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colMLastCustNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMLastStartDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.datecolMStart = new WYNLAB.Base.Controls.DateColumnEdit();
            this.colMLastEndDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.datecolMEnd = new WYNLAB.Base.Controls.DateColumnEdit();
            this.colMLastStatNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colPriceId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCustId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCustNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.popcolCust = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
            this.colStartDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.datecolStart = new WYNLAB.Base.Controls.DateColumnEdit();
            this.colEndDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.datecolEnd = new WYNLAB.Base.Controls.DateColumnEdit();
            this.colCurCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookupcolCurCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookupcolUnitCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colPrice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panButtons = new WYNLAB.Base.Controls.PanelWyn();
            this.featBar = new WYNLAB.Popup.FeatureBarWyn();
            this.btnRevise = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnDeletRow1 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAddRow1 = new WYNLAB.Base.Controls.ButtonWyn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.lblDetailAccId = new DevExpress.XtraEditors.LabelControl();
            this.cboDetailAccCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblDetailItemNo = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailItemNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailItemNm = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailItemNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailItemSpec = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailItemSpec = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailCustNm = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailCustNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailAssetType = new DevExpress.XtraEditors.LabelControl();
            this.cboDetailAssetType = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblDetailStatCd = new DevExpress.XtraEditors.LabelControl();
            this.cboDetailStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblDetailPoUnitCd = new DevExpress.XtraEditors.LabelControl();
            this.cboDetailPoUnitCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.lblAccId = new DevExpress.XtraEditors.LabelControl();
            this.cboAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblKeyword = new DevExpress.XtraEditors.LabelControl();
            this.txtKeyword = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblPriceYn = new DevExpress.XtraEditors.LabelControl();
            this.cboPriceYn = new DevExpress.XtraEditors.ComboBoxEdit();
            this.paTitleH = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
            this.panelWyn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
            this.panelWyn4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).BeginInit();
            this.panelWyn8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolMPoUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolMCur)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolMStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolMStart.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolMEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolMEnd.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolCust)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolStart.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolEnd.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolCurCd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolUnitCd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.featBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panButtons)).BeginInit();
            this.panButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
            this.panelWyn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailAccCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailItemNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailItemNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailItemSpec.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailCustNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailAssetType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailStatCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailPoUnitCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
            this.panelWyn6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKeyword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPriceYn.Properties)).BeginInit();
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
            this.panBase.Size = new System.Drawing.Size(1280, 720);
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
            this.panelWyn3.Size = new System.Drawing.Size(1270, 641);
            this.panelWyn3.TabIndex = 7;
            // 
            // panelWyn4
            // 
            this.panelWyn4.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn4.Appearance.Options.UseBackColor = true;
            this.panelWyn4.Controls.Add(this.panelWyn8);
            this.panelWyn4.Controls.Add(this.splitterWyn1);
            this.panelWyn4.Controls.Add(this.panelWyn5);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(0, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.panelWyn4.Size = new System.Drawing.Size(1270, 641);
            this.panelWyn4.TabIndex = 7;
            // 
            // panelWyn8
            // 
            this.panelWyn8.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.panelWyn8.Appearance.Options.UseBackColor = true;
            this.panelWyn8.CardCornerRadius = 0;
            this.panelWyn8.Controls.Add(this.grd1);
            this.panelWyn8.Controls.Add(this.panelWyn7);
            this.panelWyn8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn8.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn8.Location = new System.Drawing.Point(0, 3);
            this.panelWyn8.Name = "panelWyn8";
            this.panelWyn8.Padding = new System.Windows.Forms.Padding(8, 4, 8, 8);
            this.panelWyn8.Size = new System.Drawing.Size(563, 635);
            this.panelWyn8.Style = WYNLAB.Base.Controls.PanelWynStyle.Card;
            this.panelWyn8.TabIndex = 12;
            // 
            // grd1
            // 
            this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd1.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grd1.Location = new System.Drawing.Point(8, 31);
            this.grd1.MainView = this.gvw1;
            this.grd1.Name = "grd1";
            this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lookupcolMPoUnit,
            this.lookupcolMCur,
            this.datecolMStart,
            this.datecolMEnd});
            this.grd1.Size = new System.Drawing.Size(547, 596);
            this.grd1.TabIndex = 10;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colMItemId,
            this.colMItemNo,
            this.colMItemNm,
            this.colMItemSpec,
            this.colMPoUnitCd,
            this.colMLastPrice,
            this.colMLastCurCd,
            this.colMLastCustNm,
            this.colMLastStartDate,
            this.colMLastEndDate,
            this.colMLastStatNm});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightFocusedRow = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // colMItemId
            // 
            this.colMItemId.Caption = "품목";
            this.colMItemId.FieldName = "item_id";
            this.colMItemId.Name = "colMItemId";
            // 
            // colMItemNo
            // 
            this.colMItemNo.Caption = "품번";
            this.colMItemNo.FieldName = "item_no";
            this.colMItemNo.Name = "colMItemNo";
            this.colMItemNo.Visible = true;
            this.colMItemNo.VisibleIndex = 0;
            this.colMItemNo.Width = 100;
            // 
            // colMItemNm
            // 
            this.colMItemNm.Caption = "품명";
            this.colMItemNm.FieldName = "item_nm";
            this.colMItemNm.Name = "colMItemNm";
            this.colMItemNm.Visible = true;
            this.colMItemNm.VisibleIndex = 1;
            this.colMItemNm.Width = 140;
            // 
            // colMItemSpec
            // 
            this.colMItemSpec.Caption = "규격";
            this.colMItemSpec.FieldName = "item_spec";
            this.colMItemSpec.Name = "colMItemSpec";
            this.colMItemSpec.Visible = true;
            this.colMItemSpec.VisibleIndex = 2;
            this.colMItemSpec.Width = 100;
            // 
            // colMPoUnitCd
            // 
            this.colMPoUnitCd.Caption = "구매단위";
            this.colMPoUnitCd.ColumnEdit = this.lookupcolMPoUnit;
            this.colMPoUnitCd.FieldName = "po_unit_cd";
            this.colMPoUnitCd.Name = "colMPoUnitCd";
            this.colMPoUnitCd.Visible = true;
            this.colMPoUnitCd.VisibleIndex = 3;
            this.colMPoUnitCd.Width = 70;
            // 
            // lookupcolMPoUnit
            // 
            this.lookupcolMPoUnit.AutoHeight = false;
            this.lookupcolMPoUnit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookupcolMPoUnit.LookupKey = "L_CM0001";
            this.lookupcolMPoUnit.Name = "lookupcolMPoUnit";
            this.lookupcolMPoUnit.NullText = "";
            this.lookupcolMPoUnit.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colMLastPrice
            // 
            this.colMLastPrice.Caption = "최종단가";
            this.colMLastPrice.DisplayFormat.FormatString = "#,##0.####";
            this.colMLastPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colMLastPrice.FieldName = "last_price";
            this.colMLastPrice.Name = "colMLastPrice";
            this.colMLastPrice.Visible = true;
            this.colMLastPrice.VisibleIndex = 4;
            this.colMLastPrice.Width = 100;
            // 
            // colMLastCurCd
            // 
            this.colMLastCurCd.Caption = "통화";
            this.colMLastCurCd.ColumnEdit = this.lookupcolMCur;
            this.colMLastCurCd.FieldName = "last_cur_cd";
            this.colMLastCurCd.Name = "colMLastCurCd";
            this.colMLastCurCd.Visible = true;
            this.colMLastCurCd.VisibleIndex = 5;
            this.colMLastCurCd.Width = 60;
            // 
            // lookupcolMCur
            // 
            this.lookupcolMCur.AutoHeight = false;
            this.lookupcolMCur.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookupcolMCur.LookupKey = "L_CM0003";
            this.lookupcolMCur.Name = "lookupcolMCur";
            this.lookupcolMCur.NullText = "";
            this.lookupcolMCur.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colMLastCustNm
            // 
            this.colMLastCustNm.Caption = "거래처";
            this.colMLastCustNm.FieldName = "last_cust_nm";
            this.colMLastCustNm.Name = "colMLastCustNm";
            this.colMLastCustNm.Visible = true;
            this.colMLastCustNm.VisibleIndex = 6;
            this.colMLastCustNm.Width = 120;
            // 
            // colMLastStartDate
            // 
            this.colMLastStartDate.Caption = "적용시작일";
            this.colMLastStartDate.ColumnEdit = this.datecolMStart;
            this.colMLastStartDate.FieldName = "last_start_date";
            this.colMLastStartDate.Name = "colMLastStartDate";
            this.colMLastStartDate.Visible = true;
            this.colMLastStartDate.VisibleIndex = 7;
            this.colMLastStartDate.Width = 90;
            // 
            // datecolMStart
            // 
            this.datecolMStart.AutoHeight = false;
            this.datecolMStart.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.datecolMStart.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.datecolMStart.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.datecolMStart.Name = "datecolMStart";
            // 
            // colMLastEndDate
            // 
            this.colMLastEndDate.Caption = "적용종료일";
            this.colMLastEndDate.ColumnEdit = this.datecolMEnd;
            this.colMLastEndDate.FieldName = "last_end_date";
            this.colMLastEndDate.Name = "colMLastEndDate";
            this.colMLastEndDate.Visible = true;
            this.colMLastEndDate.VisibleIndex = 8;
            this.colMLastEndDate.Width = 90;
            // 
            // datecolMEnd
            // 
            this.datecolMEnd.AutoHeight = false;
            this.datecolMEnd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.datecolMEnd.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.datecolMEnd.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.datecolMEnd.Name = "datecolMEnd";
            // 
            // colMLastStatNm
            // 
            this.colMLastStatNm.Caption = "상태";
            this.colMLastStatNm.FieldName = "last_stat_nm";
            this.colMLastStatNm.Name = "colMLastStatNm";
            this.colMLastStatNm.Visible = true;
            this.colMLastStatNm.VisibleIndex = 9;
            this.colMLastStatNm.Width = 60;
            // 
            // panelWyn7
            // 
            this.panelWyn7.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn7.Appearance.Options.UseBackColor = true;
            this.panelWyn7.Controls.Add(this.sectionHeaderWyn4);
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(8, 4);
            this.panelWyn7.Name = "panelWyn7";
            this.panelWyn7.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn7.Size = new System.Drawing.Size(547, 27);
            this.panelWyn7.TabIndex = 11;
            // 
            // sectionHeaderWyn4
            // 
            this.sectionHeaderWyn4.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn4.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn4.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn4.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn4.Name = "sectionHeaderWyn4";
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(542, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 9;
            this.sectionHeaderWyn4.Text = "품목 LIST";
            // 
            // splitterWyn1
            // 
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitterWyn1.Location = new System.Drawing.Point(563, 3);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(7, 635);
            this.splitterWyn1.TabIndex = 10;
            this.splitterWyn1.TabStop = false;
            // 
            // panelWyn5
            // 
            this.panelWyn5.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.panelWyn5.Appearance.Options.UseBackColor = true;
            this.panelWyn5.CardCornerRadius = 0;
            this.panelWyn5.Controls.Add(this.grd2);
            this.panelWyn5.Controls.Add(this.panButtons);
            this.panelWyn5.Controls.Add(this.featBar);
            this.panelWyn5.Controls.Add(this.panelWyn2);
            this.panelWyn5.Controls.Add(this.panData);
            this.panelWyn5.Controls.Add(this.panelWyn6);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(570, 3);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Padding = new System.Windows.Forms.Padding(8, 4, 8, 8);
            this.panelWyn5.Size = new System.Drawing.Size(700, 635);
            this.panelWyn5.Style = WYNLAB.Base.Controls.PanelWynStyle.Card;
            this.panelWyn5.TabIndex = 6;
            // 
            // featBar (공통 기능 버튼 패널 - 선택한 단가 한 줄에 대한 전자결재/첨부파일, 메뉴등록 '화면 기능'에서 켠 것만 보인다)
            // 
            this.featBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.featBar.Location = new System.Drawing.Point(8, 136);
            this.featBar.Name = "featBar";
            this.featBar.Size = new System.Drawing.Size(684, 33);
            this.featBar.TabIndex = 10;
            // 
            // grd2
            // 
            this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd2.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grd2.Location = new System.Drawing.Point(8, 199);
            this.grd2.MainView = this.gvw2;
            this.grd2.Name = "grd2";
            this.grd2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.popcolCust,
            this.datecolStart,
            this.datecolEnd,
            this.lookupcolCurCd,
            this.lookupcolUnitCd});
            this.grd2.Size = new System.Drawing.Size(684, 428);
            this.grd2.TabIndex = 2;
            this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw2});
            // 
            // gvw2
            // 
            this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colPriceId,
            this.colItemId,
            this.colCustId,
            this.colCustNm,
            this.colStartDate,
            this.colEndDate,
            this.colCurCd,
            this.colUnitCd,
            this.colPrice,
            this.colStatNm,
            this.colRemark});
            this.gvw2.GridControl = this.grd2;
            this.gvw2.HighlightFocusedRow = true;
            this.gvw2.Name = "gvw2";
            this.gvw2.OptionsBehavior.Editable = false;
            this.gvw2.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw2.OptionsView.ColumnAutoWidth = false;
            this.gvw2.OptionsView.ShowGroupPanel = false;
            // 
            // colPriceId
            // 
            this.colPriceId.Caption = "단가ID";
            this.colPriceId.FieldName = "price_id";
            this.colPriceId.Name = "colPriceId";
            // 
            // colItemId
            // 
            this.colItemId.Caption = "품목";
            this.colItemId.FieldName = "item_id";
            this.colItemId.Name = "colItemId";
            // 
            // colCustId
            // 
            this.colCustId.Caption = "거래처ID";
            this.colCustId.FieldName = "cust_id";
            this.colCustId.Name = "colCustId";
            // 
            // colCustNm
            // 
            this.colCustNm.Caption = "거래처";
            this.colCustNm.ColumnEdit = this.popcolCust;
            this.colCustNm.FieldName = "cust_nm";
            this.colCustNm.Name = "colCustNm";
            this.colCustNm.Visible = true;
            this.colCustNm.VisibleIndex = 0;
            this.colCustNm.Width = 150;
            // 
            // popcolCust
            // 
            this.popcolCust.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", 24, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "...", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.popcolCust.LookupKey = "P_CUST";
            this.popcolCust.PopupConditions = "p_cust_class=PO";
            this.popcolCust.Name = "popcolCust";
            // 
            // colStartDate
            // 
            this.colStartDate.Caption = "적용시작일";
            this.colStartDate.ColumnEdit = this.datecolStart;
            this.colStartDate.FieldName = "start_date";
            this.colStartDate.Name = "colStartDate";
            this.colStartDate.Visible = true;
            this.colStartDate.VisibleIndex = 1;
            this.colStartDate.Width = 95;
            // 
            // datecolStart
            // 
            this.datecolStart.AutoHeight = false;
            this.datecolStart.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.datecolStart.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.datecolStart.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.datecolStart.Name = "datecolStart";
            // 
            // colEndDate
            // 
            this.colEndDate.Caption = "적용종료일";
            this.colEndDate.ColumnEdit = this.datecolEnd;
            this.colEndDate.FieldName = "end_date";
            this.colEndDate.Name = "colEndDate";
            this.colEndDate.Visible = true;
            this.colEndDate.VisibleIndex = 2;
            this.colEndDate.Width = 95;
            // 
            // datecolEnd
            // 
            this.datecolEnd.AutoHeight = false;
            this.datecolEnd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.datecolEnd.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.datecolEnd.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.datecolEnd.Name = "datecolEnd";
            // 
            // colCurCd
            // 
            this.colCurCd.Caption = "통화";
            this.colCurCd.ColumnEdit = this.lookupcolCurCd;
            this.colCurCd.FieldName = "cur_cd";
            this.colCurCd.Name = "colCurCd";
            this.colCurCd.Visible = true;
            this.colCurCd.VisibleIndex = 3;
            this.colCurCd.Width = 70;
            // 
            // lookupcolCurCd
            // 
            this.lookupcolCurCd.AutoHeight = false;
            this.lookupcolCurCd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookupcolCurCd.LookupKey = "L_CM0003";
            this.lookupcolCurCd.Name = "lookupcolCurCd";
            this.lookupcolCurCd.NullText = "";
            this.lookupcolCurCd.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colUnitCd
            // 
            this.colUnitCd.Caption = "단위";
            this.colUnitCd.ColumnEdit = this.lookupcolUnitCd;
            this.colUnitCd.FieldName = "unit_cd";
            this.colUnitCd.Name = "colUnitCd";
            this.colUnitCd.Visible = true;
            this.colUnitCd.VisibleIndex = 4;
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
            this.colPrice.Caption = "구매단가";
            this.colPrice.DisplayFormat.FormatString = "#,##0.####";
            this.colPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colPrice.FieldName = "price";
            this.colPrice.Name = "colPrice";
            this.colPrice.Visible = true;
            this.colPrice.VisibleIndex = 5;
            this.colPrice.Width = 87;
            // 
            // colStatNm
            // 
            this.colStatNm.Caption = "상태";
            this.colStatNm.FieldName = "stat_nm";
            this.colStatNm.Name = "colStatNm";
            this.colStatNm.OptionsColumn.AllowEdit = false;
            this.colStatNm.Visible = true;
            this.colStatNm.VisibleIndex = 6;
            this.colStatNm.Width = 70;
            // 
            // colRemark
            // 
            this.colRemark.Caption = "비고";
            this.colRemark.FieldName = "remark";
            this.colRemark.Name = "colRemark";
            this.colRemark.Visible = true;
            this.colRemark.VisibleIndex = 7;
            this.colRemark.Width = 200;
            // 
            // panButtons
            // 
            this.panButtons.Appearance.BackColor = System.Drawing.Color.White;
            this.panButtons.Appearance.Options.UseBackColor = true;
            this.panButtons.Controls.Add(this.btnRevise);
            this.panButtons.Controls.Add(this.btnDeletRow1);
            this.panButtons.Controls.Add(this.btnAddRow1);
            this.panButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panButtons.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panButtons.Location = new System.Drawing.Point(8, 169);
            this.panButtons.Name = "panButtons";
            this.panButtons.Padding = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.panButtons.Size = new System.Drawing.Size(684, 30);
            this.panButtons.TabIndex = 1;
            // 
            // btnRevise
            // 
            this.btnRevise.BackColor = System.Drawing.Color.Transparent;
            this.btnRevise.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(214)))), ((int)(((byte)(244)))));
            this.btnRevise.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRevise.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(240)))), ((int)(((byte)(254)))));
            this.btnRevise.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnRevise.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnRevise.Image = null;
            this.btnRevise.Location = new System.Drawing.Point(134, 3);
            this.btnRevise.Name = "btnRevise";
            this.btnRevise.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnRevise.Size = new System.Drawing.Size(60, 24);
            this.btnRevise.TabIndex = 2;
            this.btnRevise.Text = "단가개정";
            this.btnRevise.ToolTip = "선택한 단가를 오늘 시작일로 개정합니다 - 이전 단가는 종료일이 자동으로 어제로 정리되고 같은 내용의 새 행이 만들어집니다.";
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
            this.btnDeletRow1.Location = new System.Drawing.Point(70, 3);
            this.btnDeletRow1.Name = "btnDeletRow1";
            this.btnDeletRow1.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDeletRow1.Size = new System.Drawing.Size(60, 24);
            this.btnDeletRow1.TabIndex = 1;
            this.btnDeletRow1.Text = "행삭제";
            this.btnDeletRow1.ToolTip = "선택한 행을 삭제합니다(저장 시 반영).";
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
            this.btnAddRow1.Location = new System.Drawing.Point(6, 3);
            this.btnAddRow1.Name = "btnAddRow1";
            this.btnAddRow1.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnAddRow1.Size = new System.Drawing.Size(60, 24);
            this.btnAddRow1.TabIndex = 0;
            this.btnAddRow1.Text = "행추가";
            this.btnAddRow1.ToolTip = "새 단가 행을 추가합니다 - 시작일자는 오늘, 종료일자는 비우면 무기한.";
            // 
            // panelWyn2
            // 
            this.panelWyn2.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn2.Appearance.Options.UseBackColor = true;
            this.panelWyn2.Controls.Add(this.sectionHeaderWyn2);
            this.panelWyn2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn2.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn2.Location = new System.Drawing.Point(8, 134);
            this.panelWyn2.Name = "panelWyn2";
            this.panelWyn2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn2.Size = new System.Drawing.Size(684, 35);
            this.panelWyn2.TabIndex = 3;
            // 
            // sectionHeaderWyn2
            // 
            this.sectionHeaderWyn2.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn2.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn2.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn2.Name = "sectionHeaderWyn2";
            this.sectionHeaderWyn2.Size = new System.Drawing.Size(679, 33);
            this.sectionHeaderWyn2.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn2.SvgIcon")));
            this.sectionHeaderWyn2.TabIndex = 0;
            this.sectionHeaderWyn2.Text = "구매단가 등록";
            // 
            // panData
            // 
            this.panData.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panData.Appearance.Options.UseBackColor = true;
            this.panData.Controls.Add(this.lblDetailAccId);
            this.panData.Controls.Add(this.cboDetailAccCd);
            this.panData.Controls.Add(this.lblDetailItemNo);
            this.panData.Controls.Add(this.txtDetailItemNo);
            this.panData.Controls.Add(this.lblDetailItemNm);
            this.panData.Controls.Add(this.txtDetailItemNm);
            this.panData.Controls.Add(this.lblDetailItemSpec);
            this.panData.Controls.Add(this.txtDetailItemSpec);
            this.panData.Controls.Add(this.lblDetailCustNm);
            this.panData.Controls.Add(this.txtDetailCustNm);
            this.panData.Controls.Add(this.lblDetailAssetType);
            this.panData.Controls.Add(this.cboDetailAssetType);
            this.panData.Controls.Add(this.lblDetailStatCd);
            this.panData.Controls.Add(this.cboDetailStatCd);
            this.panData.Controls.Add(this.lblDetailPoUnitCd);
            this.panData.Controls.Add(this.cboDetailPoUnitCd);
            this.panData.Dock = System.Windows.Forms.DockStyle.Top;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(8, 31);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(684, 103);
            this.panData.TabIndex = 8;
            // 
            // lblDetailAccId
            // 
            this.lblDetailAccId.Location = new System.Drawing.Point(28, 20);
            this.lblDetailAccId.Name = "lblDetailAccId";
            this.lblDetailAccId.Size = new System.Drawing.Size(30, 14);
            this.lblDetailAccId.TabIndex = 0;
            this.lblDetailAccId.Text = "사업장";
            // 
            // cboDetailAccCd
            // 
            this.cboDetailAccCd.EditValue = "";
            this.cboDetailAccCd.Location = new System.Drawing.Point(65, 17);
            this.cboDetailAccCd.LookupKey = "L_ACC";
            this.cboDetailAccCd.Name = "cboDetailAccCd";
            this.cboDetailAccCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailAccCd.Properties.NullText = "";
            this.cboDetailAccCd.Properties.ReadOnly = true;
            this.cboDetailAccCd.Size = new System.Drawing.Size(157, 20);
            this.cboDetailAccCd.TabIndex = 0;
            this.cboDetailAccCd.TabStop = false;
            // 
            // lblDetailItemNo
            // 
            this.lblDetailItemNo.Location = new System.Drawing.Point(38, 48);
            this.lblDetailItemNo.Name = "lblDetailItemNo";
            this.lblDetailItemNo.Size = new System.Drawing.Size(20, 14);
            this.lblDetailItemNo.TabIndex = 1;
            this.lblDetailItemNo.Text = "품번";
            // 
            // txtDetailItemNo
            // 
            this.txtDetailItemNo.Location = new System.Drawing.Point(65, 45);
            this.txtDetailItemNo.Name = "txtDetailItemNo";
            this.txtDetailItemNo.Properties.ReadOnly = true;
            this.txtDetailItemNo.Size = new System.Drawing.Size(157, 20);
            this.txtDetailItemNo.TabIndex = 1;
            this.txtDetailItemNo.TabStop = false;
            // 
            // lblDetailItemNm
            // 
            this.lblDetailItemNm.Location = new System.Drawing.Point(256, 48);
            this.lblDetailItemNm.Name = "lblDetailItemNm";
            this.lblDetailItemNm.Size = new System.Drawing.Size(20, 14);
            this.lblDetailItemNm.TabIndex = 2;
            this.lblDetailItemNm.Text = "품명";
            // 
            // txtDetailItemNm
            // 
            this.txtDetailItemNm.Location = new System.Drawing.Point(282, 45);
            this.txtDetailItemNm.Name = "txtDetailItemNm";
            this.txtDetailItemNm.Properties.ReadOnly = true;
            this.txtDetailItemNm.Size = new System.Drawing.Size(334, 20);
            this.txtDetailItemNm.TabIndex = 2;
            this.txtDetailItemNm.TabStop = false;
            // 
            // lblDetailItemSpec
            // 
            this.lblDetailItemSpec.Location = new System.Drawing.Point(38, 74);
            this.lblDetailItemSpec.Name = "lblDetailItemSpec";
            this.lblDetailItemSpec.Size = new System.Drawing.Size(32, 14);
            this.lblDetailItemSpec.TabIndex = 3;
            this.lblDetailItemSpec.Text = "규격";
            // 
            // txtDetailItemSpec
            // 
            this.txtDetailItemSpec.Location = new System.Drawing.Point(65, 71);
            this.txtDetailItemSpec.Name = "txtDetailItemSpec";
            this.txtDetailItemSpec.Properties.ReadOnly = true;
            this.txtDetailItemSpec.Size = new System.Drawing.Size(157, 20);
            this.txtDetailItemSpec.TabIndex = 3;
            this.txtDetailItemSpec.TabStop = false;
            // 
            // lblDetailCustNm
            // 
            this.lblDetailCustNm.Location = new System.Drawing.Point(423, 74);
            this.lblDetailCustNm.Name = "lblDetailCustNm";
            this.lblDetailCustNm.Size = new System.Drawing.Size(30, 14);
            this.lblDetailCustNm.TabIndex = 4;
            this.lblDetailCustNm.Text = "거래처";
            // 
            // txtDetailCustNm
            // 
            this.txtDetailCustNm.Location = new System.Drawing.Point(459, 71);
            this.txtDetailCustNm.Name = "txtDetailCustNm";
            this.txtDetailCustNm.Properties.ReadOnly = true;
            this.txtDetailCustNm.Size = new System.Drawing.Size(157, 20);
            this.txtDetailCustNm.TabIndex = 4;
            this.txtDetailCustNm.TabStop = false;
            // 
            // lblDetailAssetType
            // 
            this.lblDetailAssetType.Location = new System.Drawing.Point(236, 20);
            this.lblDetailAssetType.Name = "lblDetailAssetType";
            this.lblDetailAssetType.Size = new System.Drawing.Size(40, 14);
            this.lblDetailAssetType.TabIndex = 5;
            this.lblDetailAssetType.Text = "자산구분";
            // 
            // cboDetailAssetType
            // 
            this.cboDetailAssetType.EditValue = "";
            this.cboDetailAssetType.Location = new System.Drawing.Point(282, 17);
            this.cboDetailAssetType.LookupKey = "L_CM0002";
            this.cboDetailAssetType.Name = "cboDetailAssetType";
            this.cboDetailAssetType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailAssetType.Properties.NullText = "";
            this.cboDetailAssetType.Properties.ReadOnly = true;
            this.cboDetailAssetType.Size = new System.Drawing.Size(135, 20);
            this.cboDetailAssetType.TabIndex = 5;
            this.cboDetailAssetType.TabStop = false;
            // 
            // lblDetailStatCd
            // 
            this.lblDetailStatCd.Location = new System.Drawing.Point(433, 20);
            this.lblDetailStatCd.Name = "lblDetailStatCd";
            this.lblDetailStatCd.Size = new System.Drawing.Size(20, 14);
            this.lblDetailStatCd.TabIndex = 6;
            this.lblDetailStatCd.Text = "상태";
            // 
            // cboDetailStatCd
            // 
            this.cboDetailStatCd.EditValue = "";
            this.cboDetailStatCd.Location = new System.Drawing.Point(459, 17);
            this.cboDetailStatCd.LookupKey = "L_BA0002";
            this.cboDetailStatCd.Name = "cboDetailStatCd";
            this.cboDetailStatCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailStatCd.Properties.NullText = "";
            this.cboDetailStatCd.Properties.ReadOnly = true;
            this.cboDetailStatCd.Size = new System.Drawing.Size(157, 20);
            this.cboDetailStatCd.TabIndex = 6;
            this.cboDetailStatCd.TabStop = false;
            // 
            // lblDetailPoUnitCd
            // 
            this.lblDetailPoUnitCd.Location = new System.Drawing.Point(236, 74);
            this.lblDetailPoUnitCd.Name = "lblDetailPoUnitCd";
            this.lblDetailPoUnitCd.Size = new System.Drawing.Size(40, 14);
            this.lblDetailPoUnitCd.TabIndex = 8;
            this.lblDetailPoUnitCd.Text = "구매단위";
            // 
            // cboDetailPoUnitCd
            // 
            this.cboDetailPoUnitCd.EditValue = "";
            this.cboDetailPoUnitCd.Location = new System.Drawing.Point(282, 71);
            this.cboDetailPoUnitCd.LookupKey = "L_CM0001";
            this.cboDetailPoUnitCd.Name = "cboDetailPoUnitCd";
            this.cboDetailPoUnitCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailPoUnitCd.Properties.NullText = "";
            this.cboDetailPoUnitCd.Properties.ReadOnly = true;
            this.cboDetailPoUnitCd.Size = new System.Drawing.Size(135, 20);
            this.cboDetailPoUnitCd.TabIndex = 8;
            this.cboDetailPoUnitCd.TabStop = false;
            // 
            // panelWyn6
            // 
            this.panelWyn6.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn6.Appearance.Options.UseBackColor = true;
            this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
            this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn6.Location = new System.Drawing.Point(8, 4);
            this.panelWyn6.Name = "panelWyn6";
            this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn6.Size = new System.Drawing.Size(684, 27);
            this.panelWyn6.TabIndex = 7;
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
            this.sectionHeaderWyn3.Size = new System.Drawing.Size(679, 25);
            this.sectionHeaderWyn3.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn3.SvgIcon")));
            this.sectionHeaderWyn3.TabIndex = 9;
            this.sectionHeaderWyn3.Text = "품목 정보";
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.White;
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.Controls.Add(this.lblAccId);
            this.panHeader.Controls.Add(this.cboAccId);
            this.panHeader.Controls.Add(this.lblKeyword);
            this.panHeader.Controls.Add(this.txtKeyword);
            this.panHeader.Controls.Add(this.lblPriceYn);
            this.panHeader.Controls.Add(this.cboPriceYn);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 25);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1270, 49);
            this.panHeader.TabIndex = 8;
            // 
            // lblAccId
            // 
            this.lblAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblAccId.Appearance.Options.UseFont = true;
            this.lblAccId.Location = new System.Drawing.Point(19, 20);
            this.lblAccId.Name = "lblAccId";
            this.lblAccId.Size = new System.Drawing.Size(36, 15);
            this.lblAccId.TabIndex = 0;
            this.lblAccId.Text = "사업장";
            // 
            // cboAccId
            // 
            this.cboAccId.EditValue = "";
            this.cboAccId.Location = new System.Drawing.Point(61, 17);
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
            this.cboAccId.TabIndex = 0;
            // 
            // lblKeyword
            // 
            this.lblKeyword.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblKeyword.Appearance.Options.UseFont = true;
            this.lblKeyword.Location = new System.Drawing.Point(242, 20);
            this.lblKeyword.Name = "lblKeyword";
            this.lblKeyword.Size = new System.Drawing.Size(53, 15);
            this.lblKeyword.TabIndex = 2;
            this.lblKeyword.Text = "품번/품명";
            // 
            // txtKeyword
            // 
            this.txtKeyword.Location = new System.Drawing.Point(299, 17);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(187, 20);
            this.txtKeyword.TabIndex = 1;
            // 
            // lblPriceYn
            // 
            this.lblPriceYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblPriceYn.Appearance.Options.UseFont = true;
            this.lblPriceYn.Location = new System.Drawing.Point(526, 20);
            this.lblPriceYn.Name = "lblPriceYn";
            this.lblPriceYn.Size = new System.Drawing.Size(48, 15);
            this.lblPriceYn.TabIndex = 4;
            this.lblPriceYn.Text = "단가등록";
            // 
            // cboPriceYn
            // 
            this.cboPriceYn.Location = new System.Drawing.Point(579, 17);
            this.cboPriceYn.Name = "cboPriceYn";
            this.cboPriceYn.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboPriceYn.Properties.Items.AddRange(new object[] {
            "전체",
            "단가 등록된 품목",
            "단가 없는 품목"});
            this.cboPriceYn.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboPriceYn.Size = new System.Drawing.Size(150, 20);
            this.cboPriceYn.TabIndex = 2;
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
            this.sectionHeaderWyn1.Text = "구매단가등록 [frmPoPrice]";
            // 
            // frmPoPrice
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.panBase);
            this.Name = "frmPoPrice";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).EndInit();
            this.panelWyn8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolMPoUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolMCur)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolMStart.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolMStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolMEnd.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolMEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
            this.panelWyn7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcolCust)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolStart.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolEnd.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datecolEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolCurCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolUnitCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.featBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panButtons)).EndInit();
            this.panButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
            this.panelWyn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
            this.panData.ResumeLayout(false);
            this.panData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailAccCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailItemNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailItemNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailItemSpec.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailCustNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailAssetType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailStatCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailPoUnitCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
            this.panelWyn6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKeyword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPriceYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).EndInit();
            this.paTitleH.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private PanelWyn panBase;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn4;
    private PanelWyn panelWyn8;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colMItemId;
    private DevExpress.XtraGrid.Columns.GridColumn colMItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colMItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colMItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colMPoUnitCd;
    private LookUpColumnEdit lookupcolMPoUnit;
    private DevExpress.XtraGrid.Columns.GridColumn colMLastPrice;
    private DevExpress.XtraGrid.Columns.GridColumn colMLastCurCd;
    private LookUpColumnEdit lookupcolMCur;
    private DevExpress.XtraGrid.Columns.GridColumn colMLastCustNm;
    private DevExpress.XtraGrid.Columns.GridColumn colMLastStartDate;
    private DateColumnEdit datecolMStart;
    private DevExpress.XtraGrid.Columns.GridColumn colMLastEndDate;
    private DateColumnEdit datecolMEnd;
    private DevExpress.XtraGrid.Columns.GridColumn colMLastStatNm;
    private PanelWyn panelWyn7;
    private SectionHeaderWyn sectionHeaderWyn4;
    private SplitterWyn splitterWyn1;
    private PanelWyn panelWyn5;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private DevExpress.XtraGrid.Columns.GridColumn colPriceId;
    private DevExpress.XtraGrid.Columns.GridColumn colItemId;
    private DevExpress.XtraGrid.Columns.GridColumn colCustId;
    private DevExpress.XtraGrid.Columns.GridColumn colCustNm;
    private PopupLookupColumnEdit popcolCust;
    private DevExpress.XtraGrid.Columns.GridColumn colStartDate;
    private DateColumnEdit datecolStart;
    private DevExpress.XtraGrid.Columns.GridColumn colEndDate;
    private DateColumnEdit datecolEnd;
    private DevExpress.XtraGrid.Columns.GridColumn colCurCd;
    private LookUpColumnEdit lookupcolCurCd;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitCd;
    private LookUpColumnEdit lookupcolUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colPrice;
    private DevExpress.XtraGrid.Columns.GridColumn colStatNm;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private PanelWyn panelWyn2;
    private SectionHeaderWyn sectionHeaderWyn2;
    private PanelWyn panButtons;
    private WYNLAB.Popup.FeatureBarWyn featBar;
    private ButtonWyn btnRevise;
    private ButtonWyn btnDeletRow1;
    private ButtonWyn btnAddRow1;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblDetailAccId;
    private LookUpEditWyn cboDetailAccCd;
    private DevExpress.XtraEditors.LabelControl lblDetailItemNo;
    private TextEditWyn txtDetailItemNo;
    private DevExpress.XtraEditors.LabelControl lblDetailItemNm;
    private TextEditWyn txtDetailItemNm;
    private DevExpress.XtraEditors.LabelControl lblDetailItemSpec;
    private TextEditWyn txtDetailItemSpec;
    private DevExpress.XtraEditors.LabelControl lblDetailCustNm;
    private TextEditWyn txtDetailCustNm;
    private DevExpress.XtraEditors.LabelControl lblDetailAssetType;
    private LookUpEditWyn cboDetailAssetType;
    private DevExpress.XtraEditors.LabelControl lblDetailStatCd;
    private LookUpEditWyn cboDetailStatCd;
    private DevExpress.XtraEditors.LabelControl lblDetailPoUnitCd;
    private LookUpEditWyn cboDetailPoUnitCd;
    private PanelWyn panelWyn6;
    private SectionHeaderWyn sectionHeaderWyn3;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblAccId;
    private LookUpEditWyn cboAccId;
    private DevExpress.XtraEditors.LabelControl lblKeyword;
    private TextEditWyn txtKeyword;
    private DevExpress.XtraEditors.LabelControl lblPriceYn;
    private DevExpress.XtraEditors.ComboBoxEdit cboPriceYn;
    private PanelWyn paTitleH;
    private SectionHeaderWyn sectionHeaderWyn1;
}
