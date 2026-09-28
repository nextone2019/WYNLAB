// 구매진행현황(frmPoStatus) - 승인 완료된 발주 라인 단위로 납품/검사/입고대기 진행을 한 줄에 보여주는 조회 전용 화면(2026-09-25).
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

public partial class frmPoStatus
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPoStatus));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelSplit = new WYNLAB.Base.Controls.PanelWyn();
            this.panelTop = new WYNLAB.Base.Controls.PanelWyn();
            this.paTitle1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colPoNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPoSerl = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPoDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCustNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPoQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDelvQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNetDelvQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRemainQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIqcWaitQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFailRetQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFailScrapQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGrReadyQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDelvDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLateDays = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStopYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkcolStop = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.lblSearchType = new DevExpress.XtraEditors.LabelControl();
            this.cboSearchType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblSearchPoNo = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchPoNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSearchKeyword = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchKeyword = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSearchFrom = new DevExpress.XtraEditors.LabelControl();
            this.dteSearchFrom = new WYNLAB.Base.Controls.DateEditWyn();
            this.dteSearchTo = new WYNLAB.Base.Controls.DateEditWyn();
            this.paTitleH = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelSplit)).BeginInit();
            this.panelSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelTop)).BeginInit();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).BeginInit();
            this.paTitle1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkcolStop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchPoNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).BeginInit();
            this.paTitleH.SuspendLayout();
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
            this.sectionHeaderWyn4.Text = "발주진행현황";
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
            this.chkcolStop});
            this.grd1.Size = new System.Drawing.Size(1664, 646);
            this.grd1.TabIndex = 1;
            this.grd1.UseEmbeddedNavigator = false;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colPoNo,
            this.colPoSerl,
            this.colPoDate,
            this.colCustNm,
            this.colItemNo,
            this.colItemNm,
            this.colItemSpec,
            this.colUnitCd,
            this.colPoQty,
            this.colDelvQty,
            this.colNetDelvQty,
            this.colRemainQty,
            this.colIqcWaitQty,
            this.colFailRetQty,
            this.colFailScrapQty,
            this.colGrReadyQty,
            this.colDelvDate,
            this.colLateDays,
            this.colStopYn});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightFocusedRow = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // colPoNo
            // 
            this.colPoNo.Caption = "발주번호";
            this.colPoNo.FieldName = "po_no";
            this.colPoNo.Name = "colPoNo";
            this.colPoNo.OptionsColumn.AllowEdit = false;
            this.colPoNo.Visible = true;
            this.colPoNo.VisibleIndex = 0;
            this.colPoNo.Width = 110;
            // 
            // colPoSerl
            // 
            this.colPoSerl.Caption = "순번";
            this.colPoSerl.FieldName = "po_serl";
            this.colPoSerl.Name = "colPoSerl";
            this.colPoSerl.OptionsColumn.AllowEdit = false;
            this.colPoSerl.Visible = true;
            this.colPoSerl.VisibleIndex = 1;
            this.colPoSerl.Width = 45;
            // 
            // colPoDate
            // 
            this.colPoDate.Caption = "발주일자";
            this.colPoDate.FieldName = "po_date";
            this.colPoDate.Name = "colPoDate";
            this.colPoDate.OptionsColumn.AllowEdit = false;
            this.colPoDate.Visible = true;
            this.colPoDate.VisibleIndex = 2;
            this.colPoDate.Width = 90;
            // 
            // colCustNm
            // 
            this.colCustNm.Caption = "거래처";
            this.colCustNm.FieldName = "cust_nm";
            this.colCustNm.Name = "colCustNm";
            this.colCustNm.OptionsColumn.AllowEdit = false;
            this.colCustNm.Visible = true;
            this.colCustNm.VisibleIndex = 3;
            this.colCustNm.Width = 130;
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
            // colUnitCd
            // 
            this.colUnitCd.Caption = "단위";
            this.colUnitCd.FieldName = "unit_cd";
            this.colUnitCd.Name = "colUnitCd";
            this.colUnitCd.OptionsColumn.AllowEdit = false;
            this.colUnitCd.Visible = true;
            this.colUnitCd.VisibleIndex = 7;
            this.colUnitCd.Width = 45;
            // 
            // colPoQty
            // 
            this.colPoQty.Caption = "발주수량";
            this.colPoQty.DisplayFormat.FormatString = "#,##0.####";
            this.colPoQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colPoQty.FieldName = "po_qty";
            this.colPoQty.Name = "colPoQty";
            this.colPoQty.OptionsColumn.AllowEdit = false;
            this.colPoQty.Visible = true;
            this.colPoQty.VisibleIndex = 8;
            this.colPoQty.Width = 80;
            // 
            // colDelvQty
            // 
            this.colDelvQty.Caption = "납품(확정)";
            this.colDelvQty.DisplayFormat.FormatString = "#,##0.####";
            this.colDelvQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDelvQty.FieldName = "delv_qty";
            this.colDelvQty.Name = "colDelvQty";
            this.colDelvQty.OptionsColumn.AllowEdit = false;
            this.colDelvQty.Visible = true;
            this.colDelvQty.VisibleIndex = 9;
            this.colDelvQty.Width = 80;
            // 
            // colNetDelvQty
            // 
            this.colNetDelvQty.Caption = "납품인정";
            this.colNetDelvQty.DisplayFormat.FormatString = "#,##0.####";
            this.colNetDelvQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colNetDelvQty.FieldName = "net_delv_qty";
            this.colNetDelvQty.Name = "colNetDelvQty";
            this.colNetDelvQty.OptionsColumn.AllowEdit = false;
            this.colNetDelvQty.Visible = true;
            this.colNetDelvQty.VisibleIndex = 10;
            this.colNetDelvQty.Width = 80;
            // 
            // colRemainQty
            // 
            this.colRemainQty.Caption = "미납잔량";
            this.colRemainQty.DisplayFormat.FormatString = "#,##0.####";
            this.colRemainQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colRemainQty.FieldName = "remain_qty";
            this.colRemainQty.Name = "colRemainQty";
            this.colRemainQty.OptionsColumn.AllowEdit = false;
            this.colRemainQty.Visible = true;
            this.colRemainQty.VisibleIndex = 11;
            this.colRemainQty.Width = 80;
            // 
            // colIqcWaitQty
            // 
            this.colIqcWaitQty.Caption = "검사대기";
            this.colIqcWaitQty.DisplayFormat.FormatString = "#,##0.####";
            this.colIqcWaitQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colIqcWaitQty.FieldName = "iqc_wait_qty";
            this.colIqcWaitQty.Name = "colIqcWaitQty";
            this.colIqcWaitQty.OptionsColumn.AllowEdit = false;
            this.colIqcWaitQty.Visible = true;
            this.colIqcWaitQty.VisibleIndex = 12;
            this.colIqcWaitQty.Width = 80;
            // 
            // colFailRetQty
            // 
            this.colFailRetQty.Caption = "불합격(반품)";
            this.colFailRetQty.DisplayFormat.FormatString = "#,##0.####";
            this.colFailRetQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colFailRetQty.FieldName = "fail_ret_qty";
            this.colFailRetQty.Name = "colFailRetQty";
            this.colFailRetQty.OptionsColumn.AllowEdit = false;
            this.colFailRetQty.Visible = true;
            this.colFailRetQty.VisibleIndex = 13;
            this.colFailRetQty.Width = 85;
            // 
            // colFailScrapQty
            // 
            this.colFailScrapQty.Caption = "불합격(폐기)";
            this.colFailScrapQty.DisplayFormat.FormatString = "#,##0.####";
            this.colFailScrapQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colFailScrapQty.FieldName = "fail_scrap_qty";
            this.colFailScrapQty.Name = "colFailScrapQty";
            this.colFailScrapQty.OptionsColumn.AllowEdit = false;
            this.colFailScrapQty.Visible = true;
            this.colFailScrapQty.VisibleIndex = 14;
            this.colFailScrapQty.Width = 85;
            // 
            // colGrReadyQty
            // 
            this.colGrReadyQty.Caption = "입고대기";
            this.colGrReadyQty.DisplayFormat.FormatString = "#,##0.####";
            this.colGrReadyQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colGrReadyQty.FieldName = "gr_ready_qty";
            this.colGrReadyQty.Name = "colGrReadyQty";
            this.colGrReadyQty.OptionsColumn.AllowEdit = false;
            this.colGrReadyQty.Visible = true;
            this.colGrReadyQty.VisibleIndex = 15;
            this.colGrReadyQty.Width = 80;
            // 
            // colDelvDate
            // 
            this.colDelvDate.Caption = "납기일";
            this.colDelvDate.FieldName = "delv_date";
            this.colDelvDate.Name = "colDelvDate";
            this.colDelvDate.OptionsColumn.AllowEdit = false;
            this.colDelvDate.Visible = true;
            this.colDelvDate.VisibleIndex = 16;
            this.colDelvDate.Width = 90;
            // 
            // colLateDays
            // 
            this.colLateDays.Caption = "지연일";
            this.colLateDays.FieldName = "late_days";
            this.colLateDays.Name = "colLateDays";
            this.colLateDays.OptionsColumn.AllowEdit = false;
            this.colLateDays.Visible = true;
            this.colLateDays.VisibleIndex = 17;
            this.colLateDays.Width = 60;
            // 
            // colStopYn
            // 
            this.colStopYn.Caption = "마감";
            this.colStopYn.ColumnEdit = this.chkcolStop;
            this.colStopYn.FieldName = "stop_yn";
            this.colStopYn.Name = "colStopYn";
            this.colStopYn.OptionsColumn.AllowEdit = false;
            this.colStopYn.Visible = true;
            this.colStopYn.VisibleIndex = 18;
            this.colStopYn.Width = 45;
            // 
            // chkcolStop
            // 
            this.chkcolStop.AutoHeight = false;
            this.chkcolStop.Name = "chkcolStop";
            this.chkcolStop.ValueChecked = "Y";
            this.chkcolStop.ValueUnchecked = "N";
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.lblSearchType);
            this.panHeader.Controls.Add(this.cboSearchType);
            this.panHeader.Controls.Add(this.lblSearchPoNo);
            this.panHeader.Controls.Add(this.txtSearchPoNo);
            this.panHeader.Controls.Add(this.lblSearchKeyword);
            this.panHeader.Controls.Add(this.txtSearchKeyword);
            this.panHeader.Controls.Add(this.lblSearchFrom);
            this.panHeader.Controls.Add(this.dteSearchFrom);
            this.panHeader.Controls.Add(this.dteSearchTo);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(3, 25);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1664, 49);
            this.panHeader.TabIndex = 8;
            // 
            // lblSearchType
            // 
            this.lblSearchType.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblSearchType.Appearance.Options.UseFont = true;
            this.lblSearchType.Location = new System.Drawing.Point(303, 19);
            this.lblSearchType.Name = "lblSearchType";
            this.lblSearchType.Size = new System.Drawing.Size(24, 15);
            this.lblSearchType.TabIndex = 0;
            this.lblSearchType.Text = "구분";
            // 
            // cboSearchType
            // 
            this.cboSearchType.Location = new System.Drawing.Point(335, 16);
            this.cboSearchType.Name = "cboSearchType";
            this.cboSearchType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboSearchType.Properties.Items.AddRange(new object[] {
            "전체",
            "미납품",
            "납기지연",
            "검사대기",
            "불합격발생",
            "입고대기"});
            this.cboSearchType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboSearchType.Size = new System.Drawing.Size(130, 20);
            this.cboSearchType.TabIndex = 2;
            // 
            // lblSearchPoNo
            // 
            this.lblSearchPoNo.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblSearchPoNo.Appearance.Options.UseFont = true;
            this.lblSearchPoNo.Location = new System.Drawing.Point(487, 19);
            this.lblSearchPoNo.Name = "lblSearchPoNo";
            this.lblSearchPoNo.Size = new System.Drawing.Size(48, 15);
            this.lblSearchPoNo.TabIndex = 2;
            this.lblSearchPoNo.Text = "발주번호";
            // 
            // txtSearchPoNo
            // 
            this.txtSearchPoNo.Location = new System.Drawing.Point(541, 16);
            this.txtSearchPoNo.Name = "txtSearchPoNo";
            this.txtSearchPoNo.Size = new System.Drawing.Size(120, 20);
            this.txtSearchPoNo.TabIndex = 3;
            // 
            // lblSearchKeyword
            // 
            this.lblSearchKeyword.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblSearchKeyword.Appearance.Options.UseFont = true;
            this.lblSearchKeyword.Location = new System.Drawing.Point(685, 19);
            this.lblSearchKeyword.Name = "lblSearchKeyword";
            this.lblSearchKeyword.Size = new System.Drawing.Size(63, 15);
            this.lblSearchKeyword.TabIndex = 4;
            this.lblSearchKeyword.Text = "거래처/품목";
            // 
            // txtSearchKeyword
            // 
            this.txtSearchKeyword.Location = new System.Drawing.Point(763, 16);
            this.txtSearchKeyword.Name = "txtSearchKeyword";
            this.txtSearchKeyword.Size = new System.Drawing.Size(150, 20);
            this.txtSearchKeyword.TabIndex = 4;
            // 
            // lblSearchFrom
            // 
            this.lblSearchFrom.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblSearchFrom.Appearance.Options.UseFont = true;
            this.lblSearchFrom.Location = new System.Drawing.Point(25, 19);
            this.lblSearchFrom.Name = "lblSearchFrom";
            this.lblSearchFrom.Size = new System.Drawing.Size(48, 15);
            this.lblSearchFrom.TabIndex = 6;
            this.lblSearchFrom.Text = "발주일자";
            // 
            // dteSearchFrom
            // 
            this.dteSearchFrom.EditValue = new System.DateTime(2026, 9, 27, 0, 0, 0, 0);
            this.dteSearchFrom.Location = new System.Drawing.Point(79, 16);
            this.dteSearchFrom.Name = "dteSearchFrom";
            this.dteSearchFrom.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.dteSearchFrom.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.dteSearchFrom.Properties.Appearance.Options.UseBackColor = true;
            this.dteSearchFrom.Properties.Appearance.Options.UseForeColor = true;
            this.dteSearchFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteSearchFrom.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dteSearchFrom.Required = true;
            this.dteSearchFrom.Size = new System.Drawing.Size(100, 20);
            this.dteSearchFrom.TabIndex = 0;
            this.dteSearchFrom.YyyyMmDd = "20260927";
            // 
            // dteSearchTo
            // 
            this.dteSearchTo.EditValue = new System.DateTime(2026, 9, 27, 0, 0, 0, 0);
            this.dteSearchTo.Location = new System.Drawing.Point(180, 16);
            this.dteSearchTo.Name = "dteSearchTo";
            this.dteSearchTo.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.dteSearchTo.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.dteSearchTo.Properties.Appearance.Options.UseBackColor = true;
            this.dteSearchTo.Properties.Appearance.Options.UseForeColor = true;
            this.dteSearchTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteSearchTo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dteSearchTo.Required = true;
            this.dteSearchTo.Size = new System.Drawing.Size(100, 20);
            this.dteSearchTo.TabIndex = 1;
            this.dteSearchTo.YyyyMmDd = "20260927";
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
            this.sectionHeaderWyn1.Text = "발주진행현황 [frmPoStatus]";
            // 
            // frmPoStatus
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1680, 760);
            this.Controls.Add(this.panBase);
            this.Name = "frmPoStatus";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelSplit)).EndInit();
            this.panelSplit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelTop)).EndInit();
            this.panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).EndInit();
            this.paTitle1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkcolStop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchPoNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).EndInit();
            this.paTitleH.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private PanelWyn panBase;
    private PanelWyn panelSplit;
    private PanelWyn panelTop;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkcolStop;
    private DevExpress.XtraGrid.Columns.GridColumn colPoNo;
    private DevExpress.XtraGrid.Columns.GridColumn colPoSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colPoDate;
    private DevExpress.XtraGrid.Columns.GridColumn colCustNm;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colPoQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDelvQty;
    private DevExpress.XtraGrid.Columns.GridColumn colNetDelvQty;
    private DevExpress.XtraGrid.Columns.GridColumn colRemainQty;
    private DevExpress.XtraGrid.Columns.GridColumn colIqcWaitQty;
    private DevExpress.XtraGrid.Columns.GridColumn colFailRetQty;
    private DevExpress.XtraGrid.Columns.GridColumn colFailScrapQty;
    private DevExpress.XtraGrid.Columns.GridColumn colGrReadyQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDelvDate;
    private DevExpress.XtraGrid.Columns.GridColumn colLateDays;
    private DevExpress.XtraGrid.Columns.GridColumn colStopYn;
    private DevExpress.XtraEditors.LabelControl lblSearchType;
    private DevExpress.XtraEditors.ComboBoxEdit cboSearchType;
    private DevExpress.XtraEditors.LabelControl lblSearchPoNo;
    private TextEditWyn txtSearchPoNo;
    private DevExpress.XtraEditors.LabelControl lblSearchKeyword;
    private TextEditWyn txtSearchKeyword;
    private DevExpress.XtraEditors.LabelControl lblSearchFrom;
    private DateEditWyn dteSearchFrom;
    private DateEditWyn dteSearchTo;
    private PanelWyn panHeader;
    private PanelWyn paTitle1;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn paTitleH;
    private SectionHeaderWyn sectionHeaderWyn1;
}
