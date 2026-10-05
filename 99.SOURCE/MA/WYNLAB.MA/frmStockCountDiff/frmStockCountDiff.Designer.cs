// 실사차이분석(frmStockCountDiff) - 입력완료/확정된 실사의 라인별 차이 현황(조회전용 단일 그리드, 그룹 패널로 실사/창고/품목/사유별 집계). 실사번호 더블클릭 = 재고실사등록 열기.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

public partial class frmStockCountDiff
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
        this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.hyperlinkCntNo = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
        this.lookupcolStatCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.chkcolYn = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
        this.colCntNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCntDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLotNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colBookQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMoveQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colFinQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDiffQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colAdjReasonNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colAddYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colTransId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCfmDt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCntTitle = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblSearchFrom = new DevExpress.XtraEditors.LabelControl();
        this.dteSearchFrom = new WYNLAB.Base.Controls.DateEditWyn();
        this.dteSearchTo = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblSearchCntNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchCntNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSearchWh = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchWhNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.txtSearchWhId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSearchReason = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchReason = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblSearchKeyword = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchKeyword = new WYNLAB.Base.Controls.TextEditWyn();
        this.chkCfmOnly = new WYNLAB.Base.Controls.CheckBoxWyn();
        this.chkDiffOnly = new WYNLAB.Base.Controls.CheckBoxWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.hyperlinkCntNo)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolStatCd)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkcolYn)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchCntNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchWhNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchWhId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchReason.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkCfmOnly.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkDiffOnly.Properties)).BeginInit();
        this.SuspendLayout();
        //
        // panBase
        //
        this.panBase.Controls.Add(this.panelWyn2);
        this.panBase.Controls.Add(this.panHeader);
        this.panBase.Appearance.BackColor = System.Drawing.Color.White;
        this.panBase.Appearance.Options.UseBackColor = true;
        this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panBase.Location = new System.Drawing.Point(0, 0);
        this.panBase.Name = "panBase";
        this.panBase.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
        this.panBase.Size = new System.Drawing.Size(1680, 760);
        this.panBase.TabIndex = 6;
        //
        // panelWyn2
        //
        this.panelWyn2.Controls.Add(this.grd1);
        this.panelWyn2.Controls.Add(this.panelWyn1);
        this.panelWyn2.Appearance.BackColor = System.Drawing.Color.White;
        this.panelWyn2.Appearance.Options.UseBackColor = true;
        this.panelWyn2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelWyn2.Location = new System.Drawing.Point(5, 49);
        this.panelWyn2.Name = "panelWyn2";
        this.panelWyn2.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
        this.panelWyn2.Size = new System.Drawing.Size(1670, 706);
        this.panelWyn2.TabIndex = 2;
        //
        // panelWyn1
        //
        this.panelWyn1.Controls.Add(this.sectionHeaderWyn1);
        this.panelWyn1.Appearance.BackColor = System.Drawing.Color.White;
        this.panelWyn1.Appearance.Options.UseBackColor = true;
        this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn1.Location = new System.Drawing.Point(0, 8);
        this.panelWyn1.Name = "panelWyn1";
        this.panelWyn1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
        this.panelWyn1.Size = new System.Drawing.Size(1670, 27);
        this.panelWyn1.TabIndex = 12;
        //
        // sectionHeaderWyn1
        //
        this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
        this.sectionHeaderWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
        this.sectionHeaderWyn1.Location = new System.Drawing.Point(5, 0);
        this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
        this.sectionHeaderWyn1.Size = new System.Drawing.Size(1665, 25);
        this.sectionHeaderWyn1.TabIndex = 9;
        this.sectionHeaderWyn1.Text = "실사 라인별 차이 현황 (입력완료/확정된 실사 - 열 머리를 위 회색 영역으로 끌어다 놓으면 실사/창고/품목/조정사유별로 묶어 볼 수 있습니다)";
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
        this.hyperlinkCntNo,
        this.lookupcolStatCd,
        this.chkcolYn});
        this.grd1.Size = new System.Drawing.Size(1670, 671);
        this.grd1.TabIndex = 1;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colCntNo,
        this.colCntDate,
        this.colStatCd,
        this.colWhNm,
        this.colItemNo,
        this.colItemNm,
        this.colItemSpec,
        this.colUnitCd,
        this.colLotNo,
        this.colBookQty,
        this.colMoveQty,
        this.colFinQty,
        this.colDiffQty,
        this.colAdjReasonNm,
        this.colAddYn,
        this.colTransId,
        this.colCfmDt,
        this.colCntTitle,
        this.colRemark});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsBehavior.Editable = false;
        this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = true;
        //
        // colCntNo
        //
        this.colCntNo.Caption = "실사번호";
        this.colCntNo.ColumnEdit = this.hyperlinkCntNo;
        this.colCntNo.FieldName = "cnt_no";
        this.colCntNo.Name = "colCntNo";
        this.colCntNo.OptionsColumn.AllowEdit = false;
        this.colCntNo.Visible = true;
        this.colCntNo.VisibleIndex = 0;
        this.colCntNo.Width = 110;
        //
        // colCntDate
        //
        this.colCntDate.Caption = "기준일자";
        this.colCntDate.FieldName = "cnt_date";
        this.colCntDate.Name = "colCntDate";
        this.colCntDate.OptionsColumn.AllowEdit = false;
        this.colCntDate.Visible = true;
        this.colCntDate.VisibleIndex = 1;
        this.colCntDate.Width = 90;
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
        // colWhNm
        //
        this.colWhNm.Caption = "창고";
        this.colWhNm.FieldName = "wh_nm";
        this.colWhNm.Name = "colWhNm";
        this.colWhNm.OptionsColumn.AllowEdit = false;
        this.colWhNm.Visible = true;
        this.colWhNm.VisibleIndex = 3;
        this.colWhNm.Width = 100;
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
        this.colUnitCd.Width = 50;
        //
        // colLotNo
        //
        this.colLotNo.Caption = "LOT";
        this.colLotNo.FieldName = "lot_no";
        this.colLotNo.Name = "colLotNo";
        this.colLotNo.OptionsColumn.AllowEdit = false;
        this.colLotNo.Visible = true;
        this.colLotNo.VisibleIndex = 8;
        this.colLotNo.Width = 120;
        //
        // colBookQty
        //
        this.colBookQty.Caption = "장부수량";
        this.colBookQty.DisplayFormat.FormatString = "#,##0.####";
        this.colBookQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colBookQty.FieldName = "book_qty";
        this.colBookQty.Name = "colBookQty";
        this.colBookQty.OptionsColumn.AllowEdit = false;
        this.colBookQty.Visible = true;
        this.colBookQty.VisibleIndex = 9;
        this.colBookQty.Width = 90;
        //
        // colMoveQty
        //
        this.colMoveQty.Caption = "변동";
        this.colMoveQty.DisplayFormat.FormatString = "#,##0.####";
        this.colMoveQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colMoveQty.FieldName = "move_qty";
        this.colMoveQty.Name = "colMoveQty";
        this.colMoveQty.OptionsColumn.AllowEdit = false;
        this.colMoveQty.Visible = true;
        this.colMoveQty.VisibleIndex = 10;
        this.colMoveQty.Width = 70;
        //
        // colFinQty
        //
        this.colFinQty.Caption = "실사수량";
        this.colFinQty.DisplayFormat.FormatString = "#,##0.####";
        this.colFinQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colFinQty.FieldName = "fin_qty";
        this.colFinQty.Name = "colFinQty";
        this.colFinQty.OptionsColumn.AllowEdit = false;
        this.colFinQty.Visible = true;
        this.colFinQty.VisibleIndex = 11;
        this.colFinQty.Width = 90;
        //
        // colDiffQty
        //
        this.colDiffQty.Caption = "차이";
        this.colDiffQty.DisplayFormat.FormatString = "#,##0.####";
        this.colDiffQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colDiffQty.FieldName = "diff_qty";
        this.colDiffQty.Name = "colDiffQty";
        this.colDiffQty.OptionsColumn.AllowEdit = false;
        this.colDiffQty.Visible = true;
        this.colDiffQty.VisibleIndex = 12;
        this.colDiffQty.Width = 80;
        //
        // colAdjReasonNm
        //
        this.colAdjReasonNm.Caption = "조정사유";
        this.colAdjReasonNm.FieldName = "adj_reason_nm";
        this.colAdjReasonNm.Name = "colAdjReasonNm";
        this.colAdjReasonNm.OptionsColumn.AllowEdit = false;
        this.colAdjReasonNm.Visible = true;
        this.colAdjReasonNm.VisibleIndex = 13;
        this.colAdjReasonNm.Width = 90;
        //
        // colAddYn
        //
        this.colAddYn.Caption = "추가";
        this.colAddYn.ColumnEdit = this.chkcolYn;
        this.colAddYn.FieldName = "add_yn";
        this.colAddYn.Name = "colAddYn";
        this.colAddYn.OptionsColumn.AllowEdit = false;
        this.colAddYn.Visible = true;
        this.colAddYn.VisibleIndex = 14;
        this.colAddYn.Width = 45;
        //
        // colTransId
        //
        this.colTransId.Caption = "조정수불ID";
        this.colTransId.FieldName = "trans_id";
        this.colTransId.Name = "colTransId";
        this.colTransId.OptionsColumn.AllowEdit = false;
        this.colTransId.Visible = true;
        this.colTransId.VisibleIndex = 15;
        this.colTransId.Width = 80;
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
        this.colCfmDt.VisibleIndex = 16;
        this.colCfmDt.Width = 120;
        //
        // colCntTitle
        //
        this.colCntTitle.Caption = "실사명";
        this.colCntTitle.FieldName = "cnt_title";
        this.colCntTitle.Name = "colCntTitle";
        this.colCntTitle.OptionsColumn.AllowEdit = false;
        this.colCntTitle.Visible = true;
        this.colCntTitle.VisibleIndex = 17;
        this.colCntTitle.Width = 160;
        //
        // colRemark
        //
        this.colRemark.Caption = "비고";
        this.colRemark.FieldName = "remark";
        this.colRemark.Name = "colRemark";
        this.colRemark.OptionsColumn.AllowEdit = false;
        this.colRemark.Visible = true;
        this.colRemark.VisibleIndex = 18;
        this.colRemark.Width = 180;
        //
        // hyperlinkCntNo
        //
        this.hyperlinkCntNo.Name = "hyperlinkCntNo";
        //
        // lookupcolStatCd
        //
        this.lookupcolStatCd.AutoHeight = false;
        this.lookupcolStatCd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolStatCd.LookupKey = "L_MA0014";
        this.lookupcolStatCd.Name = "lookupcolStatCd";
        this.lookupcolStatCd.NullText = "";
        this.lookupcolStatCd.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
        //
        // chkcolYn
        //
        this.chkcolYn.AutoHeight = false;
        this.chkcolYn.Name = "chkcolYn";
        this.chkcolYn.ValueChecked = "Y";
        this.chkcolYn.ValueUnchecked = "N";
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        this.panHeader.Controls.Add(this.lblSearchAccId);
        this.panHeader.Controls.Add(this.cboSearchAccId);
        this.panHeader.Controls.Add(this.lblSearchFrom);
        this.panHeader.Controls.Add(this.dteSearchFrom);
        this.panHeader.Controls.Add(this.dteSearchTo);
        this.panHeader.Controls.Add(this.lblSearchCntNo);
        this.panHeader.Controls.Add(this.txtSearchCntNo);
        this.panHeader.Controls.Add(this.lblSearchWh);
        this.panHeader.Controls.Add(this.txtSearchWhNm);
        this.panHeader.Controls.Add(this.txtSearchWhId);
        this.panHeader.Controls.Add(this.lblSearchReason);
        this.panHeader.Controls.Add(this.cboSearchReason);
        this.panHeader.Controls.Add(this.lblSearchKeyword);
        this.panHeader.Controls.Add(this.txtSearchKeyword);
        this.panHeader.Controls.Add(this.chkCfmOnly);
        this.panHeader.Controls.Add(this.chkDiffOnly);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.Location = new System.Drawing.Point(5, 0);
        this.panHeader.Name = "panHeader";
        this.panHeader.Size = new System.Drawing.Size(1670, 49);
        this.panHeader.TabIndex = 8;
        //
        // lblSearchAccId
        //
        this.lblSearchAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchAccId.Appearance.Options.UseFont = true;
        this.lblSearchAccId.Location = new System.Drawing.Point(16, 19);
        this.lblSearchAccId.Name = "lblSearchAccId";
        this.lblSearchAccId.Size = new System.Drawing.Size(36, 15);
        this.lblSearchAccId.TabIndex = 0;
        this.lblSearchAccId.Text = "사업장";
        //
        // cboSearchAccId
        //
        this.cboSearchAccId.Location = new System.Drawing.Point(60, 16);
        this.cboSearchAccId.LookupKey = "L_ACC";
        this.cboSearchAccId.Required = true;
        this.cboSearchAccId.Name = "cboSearchAccId";
        this.cboSearchAccId.Size = new System.Drawing.Size(131, 20);
        this.cboSearchAccId.TabIndex = 1;
        //
        // lblSearchFrom
        //
        this.lblSearchFrom.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchFrom.Appearance.Options.UseFont = true;
        this.lblSearchFrom.Location = new System.Drawing.Point(215, 19);
        this.lblSearchFrom.Name = "lblSearchFrom";
        this.lblSearchFrom.Size = new System.Drawing.Size(48, 15);
        this.lblSearchFrom.TabIndex = 2;
        this.lblSearchFrom.Text = "기준일자";
        //
        // dteSearchFrom
        //
        this.dteSearchFrom.Location = new System.Drawing.Point(271, 16);
        this.dteSearchFrom.Required = true;
        this.dteSearchFrom.Name = "dteSearchFrom";
        this.dteSearchFrom.Size = new System.Drawing.Size(100, 20);
        this.dteSearchFrom.TabIndex = 3;
        //
        // dteSearchTo
        //
        this.dteSearchTo.Location = new System.Drawing.Point(372, 16);
        this.dteSearchTo.Required = true;
        this.dteSearchTo.Name = "dteSearchTo";
        this.dteSearchTo.Size = new System.Drawing.Size(100, 20);
        this.dteSearchTo.TabIndex = 4;
        //
        // lblSearchCntNo
        //
        this.lblSearchCntNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchCntNo.Appearance.Options.UseFont = true;
        this.lblSearchCntNo.Location = new System.Drawing.Point(489, 19);
        this.lblSearchCntNo.Name = "lblSearchCntNo";
        this.lblSearchCntNo.Size = new System.Drawing.Size(48, 15);
        this.lblSearchCntNo.TabIndex = 5;
        this.lblSearchCntNo.Text = "실사번호";
        //
        // txtSearchCntNo
        //
        this.txtSearchCntNo.Location = new System.Drawing.Point(543, 16);
        this.txtSearchCntNo.Name = "txtSearchCntNo";
        this.txtSearchCntNo.Size = new System.Drawing.Size(100, 20);
        this.txtSearchCntNo.TabIndex = 6;
        //
        // lblSearchWh
        //
        this.lblSearchWh.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchWh.Appearance.Options.UseFont = true;
        this.lblSearchWh.Location = new System.Drawing.Point(663, 19);
        this.lblSearchWh.Name = "lblSearchWh";
        this.lblSearchWh.Size = new System.Drawing.Size(24, 15);
        this.lblSearchWh.TabIndex = 7;
        this.lblSearchWh.Text = "창고";
        //
        // txtSearchWhNm
        //
        this.txtSearchWhNm.Location = new System.Drawing.Point(693, 16);
        this.txtSearchWhNm.LookupKey = "P_WH";
        this.txtSearchWhNm.MatchField = "wh_nm";
        this.txtSearchWhNm.Name = "txtSearchWhNm";
        this.txtSearchWhNm.Size = new System.Drawing.Size(120, 20);
        this.txtSearchWhNm.TabIndex = 8;
        //
        // txtSearchWhId
        //
        this.txtSearchWhId.Location = new System.Drawing.Point(693, 16);
        this.txtSearchWhId.Name = "txtSearchWhId";
        this.txtSearchWhId.Size = new System.Drawing.Size(120, 20);
        this.txtSearchWhId.Visible = false;
        //
        // lblSearchReason
        //
        this.lblSearchReason.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchReason.Appearance.Options.UseFont = true;
        this.lblSearchReason.Location = new System.Drawing.Point(833, 19);
        this.lblSearchReason.Name = "lblSearchReason";
        this.lblSearchReason.Size = new System.Drawing.Size(48, 15);
        this.lblSearchReason.TabIndex = 9;
        this.lblSearchReason.Text = "조정사유";
        //
        // cboSearchReason
        //
        this.cboSearchReason.Location = new System.Drawing.Point(887, 16);
        this.cboSearchReason.LookupKey = "L_MA0016";
        this.cboSearchReason.Name = "cboSearchReason";
        this.cboSearchReason.Size = new System.Drawing.Size(100, 20);
        this.cboSearchReason.TabIndex = 10;
        //
        // lblSearchKeyword
        //
        this.lblSearchKeyword.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchKeyword.Appearance.Options.UseFont = true;
        this.lblSearchKeyword.Location = new System.Drawing.Point(1007, 19);
        this.lblSearchKeyword.Name = "lblSearchKeyword";
        this.lblSearchKeyword.Size = new System.Drawing.Size(56, 15);
        this.lblSearchKeyword.TabIndex = 11;
        this.lblSearchKeyword.Text = "품번/품명";
        //
        // txtSearchKeyword
        //
        this.txtSearchKeyword.Location = new System.Drawing.Point(1071, 16);
        this.txtSearchKeyword.Name = "txtSearchKeyword";
        this.txtSearchKeyword.Size = new System.Drawing.Size(120, 20);
        this.txtSearchKeyword.TabIndex = 12;
        //
        // chkCfmOnly
        //
        this.chkCfmOnly.Location = new System.Drawing.Point(1211, 14);
        this.chkCfmOnly.Name = "chkCfmOnly";
        this.chkCfmOnly.Properties.Caption = "확정된 실사만";
        this.chkCfmOnly.Size = new System.Drawing.Size(110, 22);
        this.chkCfmOnly.TabIndex = 13;
        //
        // chkDiffOnly
        //
        this.chkDiffOnly.Location = new System.Drawing.Point(1331, 14);
        this.chkDiffOnly.Name = "chkDiffOnly";
        this.chkDiffOnly.Properties.Caption = "차이 있는 라인만";
        this.chkDiffOnly.Size = new System.Drawing.Size(130, 22);
        this.chkDiffOnly.TabIndex = 14;
        //
        // frmStockCountDiff
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 760);
        this.Controls.Add(this.panBase);
        this.Name = "frmStockCountDiff";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.hyperlinkCntNo)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolStatCd)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkcolYn)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchCntNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchWhNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchWhId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchReason.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkCfmOnly.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkDiffOnly.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panelWyn2;
    private PanelWyn panHeader;
    private PanelWyn panelWyn1;
    private SectionHeaderWyn sectionHeaderWyn1;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit hyperlinkCntNo;
    private LookUpColumnEdit lookupcolStatCd;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkcolYn;
    private DevExpress.XtraGrid.Columns.GridColumn colCntNo;
    private DevExpress.XtraGrid.Columns.GridColumn colCntDate;
    private DevExpress.XtraGrid.Columns.GridColumn colStatCd;
    private DevExpress.XtraGrid.Columns.GridColumn colWhNm;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colLotNo;
    private DevExpress.XtraGrid.Columns.GridColumn colBookQty;
    private DevExpress.XtraGrid.Columns.GridColumn colMoveQty;
    private DevExpress.XtraGrid.Columns.GridColumn colFinQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDiffQty;
    private DevExpress.XtraGrid.Columns.GridColumn colAdjReasonNm;
    private DevExpress.XtraGrid.Columns.GridColumn colAddYn;
    private DevExpress.XtraGrid.Columns.GridColumn colTransId;
    private DevExpress.XtraGrid.Columns.GridColumn colCfmDt;
    private DevExpress.XtraGrid.Columns.GridColumn colCntTitle;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private LookUpEditWyn cboSearchAccId;
    private DevExpress.XtraEditors.LabelControl lblSearchFrom;
    private DateEditWyn dteSearchFrom;
    private DateEditWyn dteSearchTo;
    private DevExpress.XtraEditors.LabelControl lblSearchCntNo;
    private TextEditWyn txtSearchCntNo;
    private DevExpress.XtraEditors.LabelControl lblSearchWh;
    private PopupLookupEditWyn txtSearchWhNm;
    private TextEditWyn txtSearchWhId;
    private DevExpress.XtraEditors.LabelControl lblSearchReason;
    private LookUpEditWyn cboSearchReason;
    private DevExpress.XtraEditors.LabelControl lblSearchKeyword;
    private TextEditWyn txtSearchKeyword;
    private CheckBoxWyn chkCfmOnly;
    private CheckBoxWyn chkDiffOnly;
}
