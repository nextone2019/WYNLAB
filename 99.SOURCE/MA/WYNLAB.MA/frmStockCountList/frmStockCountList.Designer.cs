// 재고실사현황(frmStockCountList) - Master-SubGrid 조회 화면(위 grd1 실사 목록, 아래 grd2 선택한 실사의 라인). frmEtcOutList와 같은 구조.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

public partial class frmStockCountList
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
        this.panelBottom = new WYNLAB.Base.Controls.PanelWyn();
        this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
        this.panelTop = new WYNLAB.Base.Controls.PanelWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
        this.hyperlinkCntNo = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
        this.lookupcolStatCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.lookupcolType = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.chkcolYn = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
        this.colCntNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCntDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCntType = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCntTitle = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colWhNms = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLineCnt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colEnteredCnt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDiffCnt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDeptNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCfmDt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colAppNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetLotNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetBookQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetFinQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetMoveQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetDiffQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetReason = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetAddYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblSearchFrom = new DevExpress.XtraEditors.LabelControl();
        this.dteSearchFrom = new WYNLAB.Base.Controls.DateEditWyn();
        this.dteSearchTo = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblSearchCntNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchCntNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSearchCntType = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchCntType = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblSearchStatCd = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblSearchKeyword = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchKeyword = new WYNLAB.Base.Controls.TextEditWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelBottom)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelTop)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.hyperlinkCntNo)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolStatCd)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolType)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkcolYn)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchCntNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchCntType.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchStatCd.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).BeginInit();
        this.SuspendLayout();
        //
        // panBase
        //
        this.panBase.Controls.Add(this.panelBottom);
        this.panBase.Controls.Add(this.splitterWyn1);
        this.panBase.Controls.Add(this.panelTop);
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
        // panelBottom
        //
        this.panelBottom.Controls.Add(this.grd2);
        this.panelBottom.Controls.Add(this.panelWyn2);
        this.panelBottom.Appearance.BackColor = System.Drawing.Color.White;
        this.panelBottom.Appearance.Options.UseBackColor = true;
        this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelBottom.Location = new System.Drawing.Point(5, 472);
        this.panelBottom.Name = "panelBottom";
        this.panelBottom.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
        this.panelBottom.Size = new System.Drawing.Size(1670, 283);
        this.panelBottom.TabIndex = 2;
        //
        // panelWyn2
        //
        this.panelWyn2.Controls.Add(this.sectionHeaderWyn4);
        this.panelWyn2.Appearance.BackColor = System.Drawing.Color.White;
        this.panelWyn2.Appearance.Options.UseBackColor = true;
        this.panelWyn2.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn2.Location = new System.Drawing.Point(0, 8);
        this.panelWyn2.Name = "panelWyn2";
        this.panelWyn2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
        this.panelWyn2.Size = new System.Drawing.Size(1670, 27);
        this.panelWyn2.TabIndex = 12;
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
        this.sectionHeaderWyn4.TabIndex = 9;
        this.sectionHeaderWyn4.Text = "재고실사 라인 상세현황";
        //
        // grd2
        //
        this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd2.EmbeddedNavigator.Buttons.Append.Visible = false;
        this.grd2.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
        this.grd2.EmbeddedNavigator.Buttons.Edit.Visible = false;
        this.grd2.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
        this.grd2.EmbeddedNavigator.Buttons.Remove.Visible = false;
        this.grd2.Location = new System.Drawing.Point(0, 35);
        this.grd2.MainView = this.gvw2;
        this.grd2.Name = "grd2";
        this.grd2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.chkcolYn});
        this.grd2.Size = new System.Drawing.Size(1670, 248);
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
        this.colDetWhNm,
        this.colDetLotNo,
        this.colDetBookQty,
        this.colDetFinQty,
        this.colDetMoveQty,
        this.colDetDiffQty,
        this.colDetReason,
        this.colDetAddYn,
        this.colDetRemark});
        this.gvw2.GridControl = this.grd2;
        this.gvw2.HighlightFocusedRow = true;
        this.gvw2.Name = "gvw2";
        this.gvw2.OptionsBehavior.Editable = false;
        this.gvw2.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
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
        // colDetWhNm
        //
        this.colDetWhNm.Caption = "창고";
        this.colDetWhNm.FieldName = "wh_nm";
        this.colDetWhNm.Name = "colDetWhNm";
        this.colDetWhNm.OptionsColumn.AllowEdit = false;
        this.colDetWhNm.Visible = true;
        this.colDetWhNm.VisibleIndex = 5;
        this.colDetWhNm.Width = 100;
        //
        // colDetLotNo
        //
        this.colDetLotNo.Caption = "LOT";
        this.colDetLotNo.FieldName = "lot_no";
        this.colDetLotNo.Name = "colDetLotNo";
        this.colDetLotNo.OptionsColumn.AllowEdit = false;
        this.colDetLotNo.Visible = true;
        this.colDetLotNo.VisibleIndex = 6;
        this.colDetLotNo.Width = 120;
        //
        // colDetBookQty
        //
        this.colDetBookQty.Caption = "장부수량";
        this.colDetBookQty.DisplayFormat.FormatString = "#,##0.####";
        this.colDetBookQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colDetBookQty.FieldName = "book_qty";
        this.colDetBookQty.Name = "colDetBookQty";
        this.colDetBookQty.OptionsColumn.AllowEdit = false;
        this.colDetBookQty.Visible = true;
        this.colDetBookQty.VisibleIndex = 7;
        this.colDetBookQty.Width = 90;
        //
        // colDetFinQty
        //
        this.colDetFinQty.Caption = "실사수량";
        this.colDetFinQty.DisplayFormat.FormatString = "#,##0.####";
        this.colDetFinQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colDetFinQty.FieldName = "fin_qty";
        this.colDetFinQty.Name = "colDetFinQty";
        this.colDetFinQty.OptionsColumn.AllowEdit = false;
        this.colDetFinQty.Visible = true;
        this.colDetFinQty.VisibleIndex = 8;
        this.colDetFinQty.Width = 90;
        //
        // colDetMoveQty
        //
        this.colDetMoveQty.Caption = "변동";
        this.colDetMoveQty.DisplayFormat.FormatString = "#,##0.####";
        this.colDetMoveQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colDetMoveQty.FieldName = "move_qty";
        this.colDetMoveQty.Name = "colDetMoveQty";
        this.colDetMoveQty.OptionsColumn.AllowEdit = false;
        this.colDetMoveQty.Visible = true;
        this.colDetMoveQty.VisibleIndex = 9;
        this.colDetMoveQty.Width = 70;
        //
        // colDetDiffQty
        //
        this.colDetDiffQty.Caption = "차이";
        this.colDetDiffQty.DisplayFormat.FormatString = "#,##0.####";
        this.colDetDiffQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colDetDiffQty.FieldName = "diff_qty";
        this.colDetDiffQty.Name = "colDetDiffQty";
        this.colDetDiffQty.OptionsColumn.AllowEdit = false;
        this.colDetDiffQty.Visible = true;
        this.colDetDiffQty.VisibleIndex = 10;
        this.colDetDiffQty.Width = 80;
        //
        // colDetReason
        //
        this.colDetReason.Caption = "조정사유";
        this.colDetReason.FieldName = "adj_reason_nm";
        this.colDetReason.Name = "colDetReason";
        this.colDetReason.OptionsColumn.AllowEdit = false;
        this.colDetReason.Visible = true;
        this.colDetReason.VisibleIndex = 11;
        this.colDetReason.Width = 90;
        //
        // colDetAddYn
        //
        this.colDetAddYn.Caption = "추가";
        this.colDetAddYn.ColumnEdit = this.chkcolYn;
        this.colDetAddYn.FieldName = "add_yn";
        this.colDetAddYn.Name = "colDetAddYn";
        this.colDetAddYn.OptionsColumn.AllowEdit = false;
        this.colDetAddYn.Visible = true;
        this.colDetAddYn.VisibleIndex = 12;
        this.colDetAddYn.Width = 45;
        //
        // colDetRemark
        //
        this.colDetRemark.Caption = "비고";
        this.colDetRemark.FieldName = "remark";
        this.colDetRemark.Name = "colDetRemark";
        this.colDetRemark.OptionsColumn.AllowEdit = false;
        this.colDetRemark.Visible = true;
        this.colDetRemark.VisibleIndex = 13;
        this.colDetRemark.Width = 180;
        //
        // chkcolYn
        //
        this.chkcolYn.AutoHeight = false;
        this.chkcolYn.Name = "chkcolYn";
        this.chkcolYn.ValueChecked = "Y";
        this.chkcolYn.ValueUnchecked = "N";
        //
        // panelTop
        //
        this.panelTop.Controls.Add(this.grd1);
        this.panelTop.Controls.Add(this.panelWyn1);
        this.panelTop.Appearance.BackColor = System.Drawing.Color.White;
        this.panelTop.Appearance.Options.UseBackColor = true;
        this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelTop.Location = new System.Drawing.Point(5, 82);
        this.panelTop.Name = "panelTop";
        this.panelTop.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
        this.panelTop.Size = new System.Drawing.Size(1670, 380);
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
        this.hyperlinkCntNo,
        this.lookupcolStatCd,
        this.lookupcolType});
        this.grd1.Size = new System.Drawing.Size(1670, 345);
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
        this.colCntType,
        this.colCntTitle,
        this.colWhNms,
        this.colLineCnt,
        this.colEnteredCnt,
        this.colDiffCnt,
        this.colDeptNm,
        this.colEmpNm,
        this.colCfmDt,
        this.colAppNo,
        this.colRemark});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsBehavior.Editable = false;
        this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
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
        // colCntType
        //
        this.colCntType.Caption = "실사유형";
        this.colCntType.ColumnEdit = this.lookupcolType;
        this.colCntType.FieldName = "cnt_type";
        this.colCntType.Name = "colCntType";
        this.colCntType.OptionsColumn.AllowEdit = false;
        this.colCntType.Visible = true;
        this.colCntType.VisibleIndex = 3;
        this.colCntType.Width = 90;
        //
        // colCntTitle
        //
        this.colCntTitle.Caption = "실사명";
        this.colCntTitle.FieldName = "cnt_title";
        this.colCntTitle.Name = "colCntTitle";
        this.colCntTitle.OptionsColumn.AllowEdit = false;
        this.colCntTitle.Visible = true;
        this.colCntTitle.VisibleIndex = 4;
        this.colCntTitle.Width = 180;
        //
        // colWhNms
        //
        this.colWhNms.Caption = "대상창고";
        this.colWhNms.FieldName = "wh_nms";
        this.colWhNms.Name = "colWhNms";
        this.colWhNms.OptionsColumn.AllowEdit = false;
        this.colWhNms.Visible = true;
        this.colWhNms.VisibleIndex = 5;
        this.colWhNms.Width = 160;
        //
        // colLineCnt
        //
        this.colLineCnt.Caption = "라인수";
        this.colLineCnt.DisplayFormat.FormatString = "#,##0.####";
        this.colLineCnt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colLineCnt.FieldName = "line_cnt";
        this.colLineCnt.Name = "colLineCnt";
        this.colLineCnt.OptionsColumn.AllowEdit = false;
        this.colLineCnt.Visible = true;
        this.colLineCnt.VisibleIndex = 7;
        this.colLineCnt.Width = 60;
        //
        // colEnteredCnt
        //
        this.colEnteredCnt.Caption = "입력";
        this.colEnteredCnt.DisplayFormat.FormatString = "#,##0.####";
        this.colEnteredCnt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colEnteredCnt.FieldName = "entered_cnt";
        this.colEnteredCnt.Name = "colEnteredCnt";
        this.colEnteredCnt.OptionsColumn.AllowEdit = false;
        this.colEnteredCnt.Visible = true;
        this.colEnteredCnt.VisibleIndex = 8;
        this.colEnteredCnt.Width = 60;
        //
        // colDiffCnt
        //
        this.colDiffCnt.Caption = "차이건수";
        this.colDiffCnt.DisplayFormat.FormatString = "#,##0.####";
        this.colDiffCnt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colDiffCnt.FieldName = "diff_cnt";
        this.colDiffCnt.Name = "colDiffCnt";
        this.colDiffCnt.OptionsColumn.AllowEdit = false;
        this.colDiffCnt.Visible = true;
        this.colDiffCnt.VisibleIndex = 9;
        this.colDiffCnt.Width = 70;
        //
        // colDeptNm
        //
        this.colDeptNm.Caption = "부서";
        this.colDeptNm.FieldName = "dept_nm";
        this.colDeptNm.Name = "colDeptNm";
        this.colDeptNm.OptionsColumn.AllowEdit = false;
        this.colDeptNm.Visible = true;
        this.colDeptNm.VisibleIndex = 10;
        this.colDeptNm.Width = 100;
        //
        // colEmpNm
        //
        this.colEmpNm.Caption = "담당자";
        this.colEmpNm.FieldName = "emp_nm";
        this.colEmpNm.Name = "colEmpNm";
        this.colEmpNm.OptionsColumn.AllowEdit = false;
        this.colEmpNm.Visible = true;
        this.colEmpNm.VisibleIndex = 11;
        this.colEmpNm.Width = 80;
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
        this.colCfmDt.VisibleIndex = 12;
        this.colCfmDt.Width = 120;
        //
        // colAppNo
        //
        this.colAppNo.Caption = "결재번호";
        this.colAppNo.FieldName = "app_no";
        this.colAppNo.Name = "colAppNo";
        this.colAppNo.OptionsColumn.AllowEdit = false;
        this.colAppNo.Visible = true;
        this.colAppNo.VisibleIndex = 13;
        this.colAppNo.Width = 100;
        //
        // colRemark
        //
        this.colRemark.Caption = "비고";
        this.colRemark.FieldName = "remark";
        this.colRemark.Name = "colRemark";
        this.colRemark.OptionsColumn.AllowEdit = false;
        this.colRemark.Visible = true;
        this.colRemark.VisibleIndex = 14;
        this.colRemark.Width = 200;
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
        // lookupcolType
        //
        this.lookupcolType.AutoHeight = false;
        this.lookupcolType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolType.LookupKey = "L_MA0015";
        this.lookupcolType.Name = "lookupcolType";
        this.lookupcolType.NullText = "";
        this.lookupcolType.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
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
        this.sectionHeaderWyn1.Text = "재고실사 LIST";
        //
        // splitterWyn1
        //
        this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Top;
        this.splitterWyn1.Location = new System.Drawing.Point(5, 462);
        this.splitterWyn1.Name = "splitterWyn1";
        this.splitterWyn1.Size = new System.Drawing.Size(1670, 10);
        this.splitterWyn1.TabIndex = 9;
        this.splitterWyn1.TabStop = false;
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
        this.panHeader.Controls.Add(this.lblSearchCntType);
        this.panHeader.Controls.Add(this.cboSearchCntType);
        this.panHeader.Controls.Add(this.lblSearchStatCd);
        this.panHeader.Controls.Add(this.cboSearchStatCd);
        this.panHeader.Controls.Add(this.lblSearchKeyword);
        this.panHeader.Controls.Add(this.txtSearchKeyword);
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
        this.txtSearchCntNo.Size = new System.Drawing.Size(110, 20);
        this.txtSearchCntNo.TabIndex = 6;
        //
        // lblSearchCntType
        //
        this.lblSearchCntType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchCntType.Appearance.Options.UseFont = true;
        this.lblSearchCntType.Location = new System.Drawing.Point(673, 19);
        this.lblSearchCntType.Name = "lblSearchCntType";
        this.lblSearchCntType.Size = new System.Drawing.Size(48, 15);
        this.lblSearchCntType.TabIndex = 7;
        this.lblSearchCntType.Text = "실사유형";
        //
        // cboSearchCntType
        //
        this.cboSearchCntType.Location = new System.Drawing.Point(727, 16);
        this.cboSearchCntType.LookupKey = "L_MA0015";
        this.cboSearchCntType.Name = "cboSearchCntType";
        this.cboSearchCntType.Size = new System.Drawing.Size(120, 20);
        this.cboSearchCntType.TabIndex = 8;
        //
        // lblSearchStatCd
        //
        this.lblSearchStatCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchStatCd.Appearance.Options.UseFont = true;
        this.lblSearchStatCd.Location = new System.Drawing.Point(867, 19);
        this.lblSearchStatCd.Name = "lblSearchStatCd";
        this.lblSearchStatCd.Size = new System.Drawing.Size(48, 15);
        this.lblSearchStatCd.TabIndex = 9;
        this.lblSearchStatCd.Text = "진행상태";
        //
        // cboSearchStatCd
        //
        this.cboSearchStatCd.Location = new System.Drawing.Point(921, 16);
        this.cboSearchStatCd.LookupKey = "L_MA0014";
        this.cboSearchStatCd.Name = "cboSearchStatCd";
        this.cboSearchStatCd.Size = new System.Drawing.Size(90, 20);
        this.cboSearchStatCd.TabIndex = 10;
        //
        // lblSearchKeyword
        //
        this.lblSearchKeyword.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchKeyword.Appearance.Options.UseFont = true;
        this.lblSearchKeyword.Location = new System.Drawing.Point(1031, 19);
        this.lblSearchKeyword.Name = "lblSearchKeyword";
        this.lblSearchKeyword.Size = new System.Drawing.Size(56, 15);
        this.lblSearchKeyword.TabIndex = 11;
        this.lblSearchKeyword.Text = "품번/품명";
        //
        // txtSearchKeyword
        //
        this.txtSearchKeyword.Location = new System.Drawing.Point(1095, 16);
        this.txtSearchKeyword.Name = "txtSearchKeyword";
        this.txtSearchKeyword.Size = new System.Drawing.Size(150, 20);
        this.txtSearchKeyword.TabIndex = 12;
        //
        // frmStockCountList
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 760);
        this.Controls.Add(this.panBase);
        this.Name = "frmStockCountList";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelBottom)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelTop)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.hyperlinkCntNo)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolStatCd)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolType)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkcolYn)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchCntNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchCntType.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchStatCd.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panelBottom;
    private PanelWyn panelWyn2;
    private PanelWyn panelTop;
    private PanelWyn panHeader;
    private PanelWyn panelWyn1;
    private SectionHeaderWyn sectionHeaderWyn4;
    private SectionHeaderWyn sectionHeaderWyn1;
    private SplitterWyn splitterWyn1;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit hyperlinkCntNo;
    private LookUpColumnEdit lookupcolStatCd;
    private LookUpColumnEdit lookupcolType;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkcolYn;
    private DevExpress.XtraGrid.Columns.GridColumn colCntNo;
    private DevExpress.XtraGrid.Columns.GridColumn colCntDate;
    private DevExpress.XtraGrid.Columns.GridColumn colStatCd;
    private DevExpress.XtraGrid.Columns.GridColumn colCntType;
    private DevExpress.XtraGrid.Columns.GridColumn colCntTitle;
    private DevExpress.XtraGrid.Columns.GridColumn colWhNms;
    private DevExpress.XtraGrid.Columns.GridColumn colLineCnt;
    private DevExpress.XtraGrid.Columns.GridColumn colEnteredCnt;
    private DevExpress.XtraGrid.Columns.GridColumn colDiffCnt;
    private DevExpress.XtraGrid.Columns.GridColumn colDeptNm;
    private DevExpress.XtraGrid.Columns.GridColumn colEmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn colCfmDt;
    private DevExpress.XtraGrid.Columns.GridColumn colAppNo;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private DevExpress.XtraGrid.Columns.GridColumn colDetSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colDetItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colDetItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colDetItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colDetUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colDetWhNm;
    private DevExpress.XtraGrid.Columns.GridColumn colDetLotNo;
    private DevExpress.XtraGrid.Columns.GridColumn colDetBookQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDetFinQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDetMoveQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDetDiffQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDetReason;
    private DevExpress.XtraGrid.Columns.GridColumn colDetAddYn;
    private DevExpress.XtraGrid.Columns.GridColumn colDetRemark;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private LookUpEditWyn cboSearchAccId;
    private DevExpress.XtraEditors.LabelControl lblSearchFrom;
    private DateEditWyn dteSearchFrom;
    private DateEditWyn dteSearchTo;
    private DevExpress.XtraEditors.LabelControl lblSearchCntNo;
    private TextEditWyn txtSearchCntNo;
    private DevExpress.XtraEditors.LabelControl lblSearchCntType;
    private LookUpEditWyn cboSearchCntType;
    private DevExpress.XtraEditors.LabelControl lblSearchStatCd;
    private LookUpEditWyn cboSearchStatCd;
    private DevExpress.XtraEditors.LabelControl lblSearchKeyword;
    private TextEditWyn txtSearchKeyword;
}
