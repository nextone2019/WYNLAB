// 수입검사현황(frmIqcList) - Master-SubGrid 조회 화면. 검사번호를 더블클릭하면 frmIqc가 그 건으로 열린다(2026-09-25).
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

public partial class frmIqcList
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
        this.panelSplit = new WYNLAB.Base.Controls.PanelWyn();
        this.panelTop = new WYNLAB.Base.Controls.PanelWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.hyperlinkIqcNo = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
        this.lookupcolStatCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.colIqcNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colIqcDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCustNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDeptNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLineCnt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colInspQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colOkQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colFailQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCfmDt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.sectionHeaderMaster = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
        this.panelBottom = new WYNLAB.Base.Controls.PanelWyn();
        this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
        this.lookupcolReason = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.lookupcolAction = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.colDetSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetLotNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetInspQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetSampleQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetPassQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetConcQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetFailQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetPassRate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetReason = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetAction = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetNextQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetDelvNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetPoNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.sectionHeaderSub = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.lblSearchIqcNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchIqcNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSearchKeyword = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchKeyword = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSearchFrom = new DevExpress.XtraEditors.LabelControl();
        this.dteSearchFrom = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblSearchTo = new DevExpress.XtraEditors.LabelControl();
        this.dteSearchTo = new WYNLAB.Base.Controls.DateEditWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        this.panBase.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panelSplit)).BeginInit();
        this.panelSplit.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panelTop)).BeginInit();
        this.panelTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.hyperlinkIqcNo)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolStatCd)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelBottom)).BeginInit();
        this.panelBottom.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolReason)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolAction)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchIqcNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
        this.SuspendLayout();
        //
        // panBase
        //
        this.panBase.Appearance.BackColor = System.Drawing.Color.White;
        this.panBase.Appearance.Options.UseBackColor = true;
        this.panBase.Controls.Add(this.panelSplit);
        this.panBase.Controls.Add(this.panHeader);
        this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panBase.Location = new System.Drawing.Point(0, 0);
        this.panBase.Name = "panBase";
        this.panBase.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
        this.panBase.Size = new System.Drawing.Size(1680, 760);
        this.panBase.TabIndex = 6;
        //
        // panelSplit
        //
        this.panelSplit.Controls.Add(this.panelBottom);
        this.panelSplit.Controls.Add(this.splitterWyn1);
        this.panelSplit.Controls.Add(this.panelTop);
        this.panelSplit.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelSplit.Location = new System.Drawing.Point(5, 49);
        this.panelSplit.Name = "panelSplit";
        this.panelSplit.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.panelSplit.Size = new System.Drawing.Size(1670, 706);
        this.panelSplit.TabIndex = 7;
        //
        // panelTop
        //
        this.panelTop.Controls.Add(this.grd1);
        this.panelTop.Controls.Add(this.sectionHeaderMaster);
        this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelTop.Location = new System.Drawing.Point(3, 0);
        this.panelTop.Name = "panelTop";
        this.panelTop.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
        this.panelTop.Size = new System.Drawing.Size(1664, 380);
        this.panelTop.TabIndex = 0;
        this.panelTop.Height = 380;
        //
        // hyperlinkIqcNo
        //
        this.hyperlinkIqcNo.Name = "hyperlinkIqcNo";
        this.hyperlinkIqcNo.SingleClick = false;
        //
        // lookupcolStatCd
        //
        this.lookupcolStatCd.AutoHeight = false;
        this.lookupcolStatCd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolStatCd.LookupKey = "L_MA0006";
        this.lookupcolStatCd.Name = "lookupcolStatCd";
        this.lookupcolStatCd.NullText = "";
        this.lookupcolStatCd.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd1.EmbeddedNavigator.Buttons.Append.Visible = false;
        this.grd1.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
        this.grd1.EmbeddedNavigator.Buttons.Edit.Visible = false;
        this.grd1.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
        this.grd1.EmbeddedNavigator.Buttons.Remove.Visible = false;
        this.grd1.Location = new System.Drawing.Point(0, 36);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.hyperlinkIqcNo,
                this.lookupcolStatCd});
        this.grd1.Size = new System.Drawing.Size(1664, 344);
        this.grd1.TabIndex = 1;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colIqcNo,
                this.colIqcDate,
                this.colStatCd,
                this.colCustNm,
                this.colDeptNm,
                this.colEmpNm,
                this.colLineCnt,
                this.colInspQty,
                this.colOkQty,
                this.colFailQty,
                this.colCfmDt});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsBehavior.Editable = false;
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        //
        // colIqcNo
        //
        this.colIqcNo.Caption = "검사번호";
        this.colIqcNo.ColumnEdit = this.hyperlinkIqcNo;
        this.colIqcNo.FieldName = "iqc_no";
        this.colIqcNo.Name = "colIqcNo";
        this.colIqcNo.OptionsColumn.AllowEdit = false;
        this.colIqcNo.Visible = true;
        this.colIqcNo.VisibleIndex = 0;
        this.colIqcNo.Width = 110;
        //
        // colIqcDate
        //
        this.colIqcDate.Caption = "검사일자";
        this.colIqcDate.FieldName = "iqc_date";
        this.colIqcDate.Name = "colIqcDate";
        this.colIqcDate.OptionsColumn.AllowEdit = false;
        this.colIqcDate.Visible = true;
        this.colIqcDate.VisibleIndex = 1;
        this.colIqcDate.Width = 90;
        //
        // colStatCd
        //
        this.colStatCd.Caption = "진행상태";
        this.colStatCd.ColumnEdit = this.lookupcolStatCd;
        this.colStatCd.FieldName = "stat_cd";
        this.colStatCd.Name = "colStatCd";
        this.colStatCd.OptionsColumn.AllowEdit = false;
        this.colStatCd.Visible = true;
        this.colStatCd.VisibleIndex = 2;
        this.colStatCd.Width = 80;
        //
        // colCustNm
        //
        this.colCustNm.Caption = "거래처";
        this.colCustNm.FieldName = "cust_nm";
        this.colCustNm.Name = "colCustNm";
        this.colCustNm.OptionsColumn.AllowEdit = false;
        this.colCustNm.Visible = true;
        this.colCustNm.VisibleIndex = 3;
        this.colCustNm.Width = 140;
        //
        // colDeptNm
        //
        this.colDeptNm.Caption = "부서";
        this.colDeptNm.FieldName = "dept_nm";
        this.colDeptNm.Name = "colDeptNm";
        this.colDeptNm.OptionsColumn.AllowEdit = false;
        this.colDeptNm.Visible = true;
        this.colDeptNm.VisibleIndex = 4;
        this.colDeptNm.Width = 100;
        //
        // colEmpNm
        //
        this.colEmpNm.Caption = "검사자";
        this.colEmpNm.FieldName = "emp_nm";
        this.colEmpNm.Name = "colEmpNm";
        this.colEmpNm.OptionsColumn.AllowEdit = false;
        this.colEmpNm.Visible = true;
        this.colEmpNm.VisibleIndex = 5;
        this.colEmpNm.Width = 90;
        //
        // colLineCnt
        //
        this.colLineCnt.Caption = "품목수";
        this.colLineCnt.FieldName = "line_cnt";
        this.colLineCnt.Name = "colLineCnt";
        this.colLineCnt.OptionsColumn.AllowEdit = false;
        this.colLineCnt.Visible = true;
        this.colLineCnt.VisibleIndex = 6;
        this.colLineCnt.Width = 60;
        //
        // colInspQty
        //
        this.colInspQty.Caption = "검사수량";
        this.colInspQty.DisplayFormat.FormatString = "#,##0.####";
        this.colInspQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colInspQty.FieldName = "insp_qty";
        this.colInspQty.Name = "colInspQty";
        this.colInspQty.OptionsColumn.AllowEdit = false;
        this.colInspQty.Visible = true;
        this.colInspQty.VisibleIndex = 7;
        this.colInspQty.Width = 80;
        //
        // colOkQty
        //
        this.colOkQty.Caption = "합격+특채";
        this.colOkQty.DisplayFormat.FormatString = "#,##0.####";
        this.colOkQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colOkQty.FieldName = "ok_qty";
        this.colOkQty.Name = "colOkQty";
        this.colOkQty.OptionsColumn.AllowEdit = false;
        this.colOkQty.Visible = true;
        this.colOkQty.VisibleIndex = 8;
        this.colOkQty.Width = 80;
        //
        // colFailQty
        //
        this.colFailQty.Caption = "불합격";
        this.colFailQty.DisplayFormat.FormatString = "#,##0.####";
        this.colFailQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colFailQty.FieldName = "fail_qty";
        this.colFailQty.Name = "colFailQty";
        this.colFailQty.OptionsColumn.AllowEdit = false;
        this.colFailQty.Visible = true;
        this.colFailQty.VisibleIndex = 9;
        this.colFailQty.Width = 70;
        //
        // colCfmDt
        //
        this.colCfmDt.Caption = "확정일시";
        this.colCfmDt.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
        this.colCfmDt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
        this.colCfmDt.FieldName = "cfm_dt";
        this.colCfmDt.Name = "colCfmDt";
        this.colCfmDt.OptionsColumn.AllowEdit = false;
        this.colCfmDt.Visible = true;
        this.colCfmDt.VisibleIndex = 10;
        this.colCfmDt.Width = 120;
        //
        // sectionHeaderMaster
        //
        this.sectionHeaderMaster.BackColor = System.Drawing.Color.White;
        this.sectionHeaderMaster.Dock = System.Windows.Forms.DockStyle.Top;
        this.sectionHeaderMaster.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.sectionHeaderMaster.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
        this.sectionHeaderMaster.Location = new System.Drawing.Point(0, 8);
        this.sectionHeaderMaster.Name = "sectionHeaderMaster";
        this.sectionHeaderMaster.Size = new System.Drawing.Size(1664, 28);
        this.sectionHeaderMaster.TabIndex = 0;
        this.sectionHeaderMaster.Text = "수입검사 목록";
        //
        // splitterWyn1
        //
        this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Top;
        this.splitterWyn1.Location = new System.Drawing.Point(3, 380);
        this.splitterWyn1.Name = "splitterWyn1";
        this.splitterWyn1.Size = new System.Drawing.Size(1664, 10);
        this.splitterWyn1.TabIndex = 1;
        this.splitterWyn1.TabStop = false;
        //
        // panelBottom
        //
        this.panelBottom.Controls.Add(this.grd2);
        this.panelBottom.Controls.Add(this.sectionHeaderSub);
        this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelBottom.Location = new System.Drawing.Point(3, 390);
        this.panelBottom.Name = "panelBottom";
        this.panelBottom.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
        this.panelBottom.Size = new System.Drawing.Size(1664, 316);
        this.panelBottom.TabIndex = 2;
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
        // grd2
        //
        this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd2.EmbeddedNavigator.Buttons.Append.Visible = false;
        this.grd2.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
        this.grd2.EmbeddedNavigator.Buttons.Edit.Visible = false;
        this.grd2.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
        this.grd2.EmbeddedNavigator.Buttons.Remove.Visible = false;
        this.grd2.Location = new System.Drawing.Point(0, 36);
        this.grd2.MainView = this.gvw2;
        this.grd2.Name = "grd2";
        this.grd2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.lookupcolReason,
                this.lookupcolAction});
        this.grd2.Size = new System.Drawing.Size(1664, 280);
        this.grd2.TabIndex = 1;
        this.grd2.UseEmbeddedNavigator = false;
        this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw2});
        //
        // gvw2
        //
        this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colDetSerl,
                this.colDetItemNo,
                this.colDetItemNm,
                this.colDetItemSpec,
                this.colDetUnitCd,
                this.colDetLotNo,
                this.colDetInspQty,
                this.colDetSampleQty,
                this.colDetPassQty,
                this.colDetConcQty,
                this.colDetFailQty,
                this.colDetPassRate,
                this.colDetReason,
                this.colDetAction,
                this.colDetNextQty,
                this.colDetDelvNo,
                this.colDetPoNo,
                this.colDetWhNm,
                this.colDetRemark});
        this.gvw2.GridControl = this.grd2;
        this.gvw2.HighlightFocusedRow = true;
        this.gvw2.Name = "gvw2";
        this.gvw2.OptionsBehavior.Editable = false;
        this.gvw2.OptionsView.ColumnAutoWidth = false;
        this.gvw2.OptionsView.ShowGroupPanel = false;
        //
        // colDetSerl
        //
        this.colDetSerl.Caption = "순번";
        this.colDetSerl.FieldName = "serl";
        this.colDetSerl.Name = "colDetSerl";
        this.colDetSerl.OptionsColumn.AllowEdit = false;
        this.colDetSerl.Visible = true;
        this.colDetSerl.VisibleIndex = 0;
        this.colDetSerl.Width = 50;
        //
        // colDetItemNo
        //
        this.colDetItemNo.Caption = "품번";
        this.colDetItemNo.FieldName = "item_no";
        this.colDetItemNo.Name = "colDetItemNo";
        this.colDetItemNo.OptionsColumn.AllowEdit = false;
        this.colDetItemNo.Visible = true;
        this.colDetItemNo.VisibleIndex = 1;
        this.colDetItemNo.Width = 100;
        //
        // colDetItemNm
        //
        this.colDetItemNm.Caption = "품명";
        this.colDetItemNm.FieldName = "item_nm";
        this.colDetItemNm.Name = "colDetItemNm";
        this.colDetItemNm.OptionsColumn.AllowEdit = false;
        this.colDetItemNm.Visible = true;
        this.colDetItemNm.VisibleIndex = 2;
        this.colDetItemNm.Width = 140;
        //
        // colDetItemSpec
        //
        this.colDetItemSpec.Caption = "규격";
        this.colDetItemSpec.FieldName = "item_spec";
        this.colDetItemSpec.Name = "colDetItemSpec";
        this.colDetItemSpec.OptionsColumn.AllowEdit = false;
        this.colDetItemSpec.Visible = true;
        this.colDetItemSpec.VisibleIndex = 3;
        this.colDetItemSpec.Width = 110;
        //
        // colDetUnitCd
        //
        this.colDetUnitCd.Caption = "단위";
        this.colDetUnitCd.FieldName = "unit_cd";
        this.colDetUnitCd.Name = "colDetUnitCd";
        this.colDetUnitCd.OptionsColumn.AllowEdit = false;
        this.colDetUnitCd.Visible = true;
        this.colDetUnitCd.VisibleIndex = 4;
        this.colDetUnitCd.Width = 50;
        //
        // colDetLotNo
        //
        this.colDetLotNo.Caption = "LOT";
        this.colDetLotNo.FieldName = "lot_no";
        this.colDetLotNo.Name = "colDetLotNo";
        this.colDetLotNo.OptionsColumn.AllowEdit = false;
        this.colDetLotNo.Visible = true;
        this.colDetLotNo.VisibleIndex = 5;
        this.colDetLotNo.Width = 90;
        //
        // colDetInspQty
        //
        this.colDetInspQty.Caption = "검사수량";
        this.colDetInspQty.DisplayFormat.FormatString = "#,##0.####";
        this.colDetInspQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colDetInspQty.FieldName = "insp_qty";
        this.colDetInspQty.Name = "colDetInspQty";
        this.colDetInspQty.OptionsColumn.AllowEdit = false;
        this.colDetInspQty.Visible = true;
        this.colDetInspQty.VisibleIndex = 6;
        this.colDetInspQty.Width = 80;
        //
        // colDetSampleQty
        //
        this.colDetSampleQty.Caption = "표본수";
        this.colDetSampleQty.DisplayFormat.FormatString = "#,##0.####";
        this.colDetSampleQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colDetSampleQty.FieldName = "sample_qty";
        this.colDetSampleQty.Name = "colDetSampleQty";
        this.colDetSampleQty.OptionsColumn.AllowEdit = false;
        this.colDetSampleQty.Visible = true;
        this.colDetSampleQty.VisibleIndex = 7;
        this.colDetSampleQty.Width = 70;
        //
        // colDetPassQty
        //
        this.colDetPassQty.Caption = "합격";
        this.colDetPassQty.DisplayFormat.FormatString = "#,##0.####";
        this.colDetPassQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colDetPassQty.FieldName = "pass_qty";
        this.colDetPassQty.Name = "colDetPassQty";
        this.colDetPassQty.OptionsColumn.AllowEdit = false;
        this.colDetPassQty.Visible = true;
        this.colDetPassQty.VisibleIndex = 8;
        this.colDetPassQty.Width = 70;
        //
        // colDetConcQty
        //
        this.colDetConcQty.Caption = "특채";
        this.colDetConcQty.DisplayFormat.FormatString = "#,##0.####";
        this.colDetConcQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colDetConcQty.FieldName = "conc_qty";
        this.colDetConcQty.Name = "colDetConcQty";
        this.colDetConcQty.OptionsColumn.AllowEdit = false;
        this.colDetConcQty.Visible = true;
        this.colDetConcQty.VisibleIndex = 9;
        this.colDetConcQty.Width = 70;
        //
        // colDetFailQty
        //
        this.colDetFailQty.Caption = "불합격";
        this.colDetFailQty.DisplayFormat.FormatString = "#,##0.####";
        this.colDetFailQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colDetFailQty.FieldName = "fail_qty";
        this.colDetFailQty.Name = "colDetFailQty";
        this.colDetFailQty.OptionsColumn.AllowEdit = false;
        this.colDetFailQty.Visible = true;
        this.colDetFailQty.VisibleIndex = 10;
        this.colDetFailQty.Width = 70;
        //
        // colDetPassRate
        //
        this.colDetPassRate.Caption = "합격률(%)";
        this.colDetPassRate.DisplayFormat.FormatString = "#,##0.####";
        this.colDetPassRate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colDetPassRate.FieldName = "pass_rate";
        this.colDetPassRate.Name = "colDetPassRate";
        this.colDetPassRate.OptionsColumn.AllowEdit = false;
        this.colDetPassRate.Visible = true;
        this.colDetPassRate.VisibleIndex = 11;
        this.colDetPassRate.Width = 80;
        //
        // colDetReason
        //
        this.colDetReason.Caption = "불량유형";
        this.colDetReason.ColumnEdit = this.lookupcolReason;
        this.colDetReason.FieldName = "fail_reason_cd";
        this.colDetReason.Name = "colDetReason";
        this.colDetReason.OptionsColumn.AllowEdit = false;
        this.colDetReason.Visible = true;
        this.colDetReason.VisibleIndex = 12;
        this.colDetReason.Width = 90;
        //
        // colDetAction
        //
        this.colDetAction.Caption = "불합격처분";
        this.colDetAction.ColumnEdit = this.lookupcolAction;
        this.colDetAction.FieldName = "fail_action_cd";
        this.colDetAction.Name = "colDetAction";
        this.colDetAction.OptionsColumn.AllowEdit = false;
        this.colDetAction.Visible = true;
        this.colDetAction.VisibleIndex = 13;
        this.colDetAction.Width = 110;
        //
        // colDetNextQty
        //
        this.colDetNextQty.Caption = "입고처리";
        this.colDetNextQty.DisplayFormat.FormatString = "#,##0.####";
        this.colDetNextQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colDetNextQty.FieldName = "next_qty";
        this.colDetNextQty.Name = "colDetNextQty";
        this.colDetNextQty.OptionsColumn.AllowEdit = false;
        this.colDetNextQty.Visible = true;
        this.colDetNextQty.VisibleIndex = 14;
        this.colDetNextQty.Width = 80;
        //
        // colDetDelvNo
        //
        this.colDetDelvNo.Caption = "납품번호";
        this.colDetDelvNo.FieldName = "delv_no";
        this.colDetDelvNo.Name = "colDetDelvNo";
        this.colDetDelvNo.OptionsColumn.AllowEdit = false;
        this.colDetDelvNo.Visible = true;
        this.colDetDelvNo.VisibleIndex = 15;
        this.colDetDelvNo.Width = 110;
        //
        // colDetPoNo
        //
        this.colDetPoNo.Caption = "발주번호";
        this.colDetPoNo.FieldName = "po_no";
        this.colDetPoNo.Name = "colDetPoNo";
        this.colDetPoNo.OptionsColumn.AllowEdit = false;
        this.colDetPoNo.Visible = true;
        this.colDetPoNo.VisibleIndex = 16;
        this.colDetPoNo.Width = 110;
        //
        // colDetWhNm
        //
        this.colDetWhNm.Caption = "창고";
        this.colDetWhNm.FieldName = "wh_nm";
        this.colDetWhNm.Name = "colDetWhNm";
        this.colDetWhNm.OptionsColumn.AllowEdit = false;
        this.colDetWhNm.Visible = true;
        this.colDetWhNm.VisibleIndex = 17;
        this.colDetWhNm.Width = 90;
        //
        // colDetRemark
        //
        this.colDetRemark.Caption = "비고";
        this.colDetRemark.FieldName = "remark";
        this.colDetRemark.Name = "colDetRemark";
        this.colDetRemark.OptionsColumn.AllowEdit = false;
        this.colDetRemark.Visible = true;
        this.colDetRemark.VisibleIndex = 18;
        this.colDetRemark.Width = 160;
        //
        // sectionHeaderSub
        //
        this.sectionHeaderSub.BackColor = System.Drawing.Color.White;
        this.sectionHeaderSub.Dock = System.Windows.Forms.DockStyle.Top;
        this.sectionHeaderSub.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.sectionHeaderSub.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
        this.sectionHeaderSub.Location = new System.Drawing.Point(0, 8);
        this.sectionHeaderSub.Name = "sectionHeaderSub";
        this.sectionHeaderSub.Size = new System.Drawing.Size(1664, 28);
        this.sectionHeaderSub.TabIndex = 0;
        this.sectionHeaderSub.Text = "수입검사 품목 상세";
        //
        // lblSearchIqcNo
        //
        this.lblSearchIqcNo.Location = new System.Drawing.Point(224, 18);
        this.lblSearchIqcNo.Name = "lblSearchIqcNo";
        this.lblSearchIqcNo.Size = new System.Drawing.Size(48, 15);
        this.lblSearchIqcNo.TabIndex = 0;
        this.lblSearchIqcNo.Text = "검사번호";
        //
        // txtSearchIqcNo
        //
        this.txtSearchIqcNo.Location = new System.Drawing.Point(280, 15);
        this.txtSearchIqcNo.Name = "txtSearchIqcNo";
        this.txtSearchIqcNo.Size = new System.Drawing.Size(130, 20);
        this.txtSearchIqcNo.TabIndex = 1;
        //
        // lblSearchKeyword
        //
        this.lblSearchKeyword.Location = new System.Drawing.Point(432, 18);
        this.lblSearchKeyword.Name = "lblSearchKeyword";
        this.lblSearchKeyword.Size = new System.Drawing.Size(72, 15);
        this.lblSearchKeyword.TabIndex = 2;
        this.lblSearchKeyword.Text = "거래처/품목";
        //
        // txtSearchKeyword
        //
        this.txtSearchKeyword.Location = new System.Drawing.Point(512, 15);
        this.txtSearchKeyword.Name = "txtSearchKeyword";
        this.txtSearchKeyword.Size = new System.Drawing.Size(160, 20);
        this.txtSearchKeyword.TabIndex = 3;
        //
        // lblSearchFrom
        //
        this.lblSearchFrom.Location = new System.Drawing.Point(694, 18);
        this.lblSearchFrom.Name = "lblSearchFrom";
        this.lblSearchFrom.Size = new System.Drawing.Size(48, 15);
        this.lblSearchFrom.TabIndex = 4;
        this.lblSearchFrom.Text = "검사일자";
        //
        // dteSearchFrom
        //
        this.dteSearchFrom.Location = new System.Drawing.Point(750, 15);
        this.dteSearchFrom.Name = "dteSearchFrom";
        this.dteSearchFrom.Size = new System.Drawing.Size(110, 20);
        this.dteSearchFrom.TabIndex = 5;
        //
        // lblSearchTo
        //
        this.lblSearchTo.Location = new System.Drawing.Point(882, 18);
        this.lblSearchTo.Name = "lblSearchTo";
        this.lblSearchTo.Size = new System.Drawing.Size(24, 15);
        this.lblSearchTo.TabIndex = 6;
        this.lblSearchTo.Text = "~";
        //
        // dteSearchTo
        //
        this.dteSearchTo.Location = new System.Drawing.Point(914, 15);
        this.dteSearchTo.Name = "dteSearchTo";
        this.dteSearchTo.Size = new System.Drawing.Size(110, 20);
        this.dteSearchTo.TabIndex = 7;
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblSearchAccId);
        this.panHeader.Controls.Add(this.cboSearchAccId);
        this.panHeader.Controls.Add(this.lblSearchIqcNo);
        this.panHeader.Controls.Add(this.txtSearchIqcNo);
        this.panHeader.Controls.Add(this.lblSearchKeyword);
        this.panHeader.Controls.Add(this.txtSearchKeyword);
        this.panHeader.Controls.Add(this.lblSearchFrom);
        this.panHeader.Controls.Add(this.dteSearchFrom);
        this.panHeader.Controls.Add(this.lblSearchTo);
        this.panHeader.Controls.Add(this.dteSearchTo);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.Location = new System.Drawing.Point(5, 0);
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
        // frmIqcList
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 760);
        this.Controls.Add(this.panBase);
        this.Name = "frmIqcList";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panelSplit)).EndInit();
        this.panelSplit.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panelTop)).EndInit();
        this.panelTop.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.hyperlinkIqcNo)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolStatCd)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelBottom)).EndInit();
        this.panelBottom.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolReason)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolAction)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchIqcNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panelSplit;
    private PanelWyn panelTop;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit hyperlinkIqcNo;
    private LookUpColumnEdit lookupcolStatCd;
    private DevExpress.XtraGrid.Columns.GridColumn colIqcNo;
    private DevExpress.XtraGrid.Columns.GridColumn colIqcDate;
    private DevExpress.XtraGrid.Columns.GridColumn colStatCd;
    private DevExpress.XtraGrid.Columns.GridColumn colCustNm;
    private DevExpress.XtraGrid.Columns.GridColumn colDeptNm;
    private DevExpress.XtraGrid.Columns.GridColumn colEmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn colLineCnt;
    private DevExpress.XtraGrid.Columns.GridColumn colInspQty;
    private DevExpress.XtraGrid.Columns.GridColumn colOkQty;
    private DevExpress.XtraGrid.Columns.GridColumn colFailQty;
    private DevExpress.XtraGrid.Columns.GridColumn colCfmDt;
    private SectionHeaderWyn sectionHeaderMaster;
    private SplitterWyn splitterWyn1;
    private PanelWyn panelBottom;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private LookUpColumnEdit lookupcolReason;
    private LookUpColumnEdit lookupcolAction;
    private DevExpress.XtraGrid.Columns.GridColumn colDetSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colDetItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colDetItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colDetItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colDetUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colDetLotNo;
    private DevExpress.XtraGrid.Columns.GridColumn colDetInspQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDetSampleQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDetPassQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDetConcQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDetFailQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDetPassRate;
    private DevExpress.XtraGrid.Columns.GridColumn colDetReason;
    private DevExpress.XtraGrid.Columns.GridColumn colDetAction;
    private DevExpress.XtraGrid.Columns.GridColumn colDetNextQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDetDelvNo;
    private DevExpress.XtraGrid.Columns.GridColumn colDetPoNo;
    private DevExpress.XtraGrid.Columns.GridColumn colDetWhNm;
    private DevExpress.XtraGrid.Columns.GridColumn colDetRemark;
    private SectionHeaderWyn sectionHeaderSub;
    private DevExpress.XtraEditors.LabelControl lblSearchIqcNo;
    private TextEditWyn txtSearchIqcNo;
    private DevExpress.XtraEditors.LabelControl lblSearchKeyword;
    private TextEditWyn txtSearchKeyword;
    private DevExpress.XtraEditors.LabelControl lblSearchFrom;
    private DateEditWyn dteSearchFrom;
    private DevExpress.XtraEditors.LabelControl lblSearchTo;
    private DateEditWyn dteSearchTo;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private WYNLAB.Base.Controls.LookUpEditWyn cboSearchAccId;
}
