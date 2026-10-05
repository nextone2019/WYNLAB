// 기타출고현황(frmEtcOutList) - Master-SubGrid 조회 화면(위 grd1 기타출고 목록, 아래 grd2 선택한 출고의 품목). frmGrList와 같은 구조(2026-10-03).
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

public partial class frmEtcOutList
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
        this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
        this.hyperlinkOutNo = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
        this.lookupcolStatCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.lookupcolType = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.chkcolStock = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
        this.colOutNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colOutDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colTransType = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDeptNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLineCnt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colTotalQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCfmDt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetOutQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetLotNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetLocNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetStockYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetSrcNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDetRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblSearchFrom = new DevExpress.XtraEditors.LabelControl();
        this.dteSearchFrom = new WYNLAB.Base.Controls.DateEditWyn();
        this.dteSearchTo = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblSearchOutNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchOutNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSearchTransType = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchTransType = new WYNLAB.Base.Controls.LookUpEditWyn();
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
        ((System.ComponentModel.ISupportInitialize)(this.paTitle)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.hyperlinkOutNo)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolStatCd)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolType)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkcolStock)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchOutNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchTransType.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchStatCd.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).BeginInit();
        this.SuspendLayout();
        //
        // panBase
        //
        this.panBase.Appearance.BackColor = System.Drawing.Color.White;
        this.panBase.Appearance.Options.UseBackColor = true;
        this.panBase.Controls.Add(this.panelBottom);
        this.panBase.Controls.Add(this.splitterWyn1);
        this.panBase.Controls.Add(this.panelTop);
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
        // panelBottom
        //
        this.panelBottom.Appearance.BackColor = System.Drawing.Color.White;
        this.panelBottom.Appearance.Options.UseBackColor = true;
        this.panelBottom.Controls.Add(this.grd2);
        this.panelBottom.Controls.Add(this.panelWyn2);
        this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelBottom.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panelBottom.Location = new System.Drawing.Point(5, 472);
        this.panelBottom.Name = "panelBottom";
        this.panelBottom.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
        this.panelBottom.Size = new System.Drawing.Size(1670, 283);
        this.panelBottom.TabIndex = 2;
        //
        // panelWyn2
        //
        this.panelWyn2.Appearance.BackColor = System.Drawing.Color.White;
        this.panelWyn2.Appearance.Options.UseBackColor = true;
        this.panelWyn2.Controls.Add(this.sectionHeaderWyn4);
        this.panelWyn2.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn2.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
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
        this.sectionHeaderWyn4.Text = "기타출고 품목 상세현황";
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
        this.chkcolStock});
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
        this.colDetOutQty,
        this.colDetLotNo,
        this.colDetWhNm,
        this.colDetLocNm,
        this.colDetStockYn,
        this.colDetSrcNo,
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
        // colDetOutQty
        //
        this.colDetOutQty.Caption = "출고수량";
        this.colDetOutQty.DisplayFormat.FormatString = "#,##0.####";
        this.colDetOutQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colDetOutQty.FieldName = "out_qty";
        this.colDetOutQty.Name = "colDetOutQty";
        this.colDetOutQty.OptionsColumn.AllowEdit = false;
        this.colDetOutQty.Visible = true;
        this.colDetOutQty.VisibleIndex = 5;
        this.colDetOutQty.Width = 90;
        //
        // colDetLotNo
        //
        this.colDetLotNo.Caption = "LOT";
        this.colDetLotNo.FieldName = "lot_no";
        this.colDetLotNo.Name = "colDetLotNo";
        this.colDetLotNo.OptionsColumn.AllowEdit = false;
        this.colDetLotNo.Visible = true;
        this.colDetLotNo.VisibleIndex = 6;
        this.colDetLotNo.Width = 90;
        //
        // colDetWhNm
        //
        this.colDetWhNm.Caption = "창고";
        this.colDetWhNm.FieldName = "wh_nm";
        this.colDetWhNm.Name = "colDetWhNm";
        this.colDetWhNm.OptionsColumn.AllowEdit = false;
        this.colDetWhNm.Visible = true;
        this.colDetWhNm.VisibleIndex = 7;
        this.colDetWhNm.Width = 90;
        //
        // colDetLocNm
        //
        this.colDetLocNm.Caption = "위치";
        this.colDetLocNm.FieldName = "loc_nm";
        this.colDetLocNm.Name = "colDetLocNm";
        this.colDetLocNm.OptionsColumn.AllowEdit = false;
        this.colDetLocNm.Visible = true;
        this.colDetLocNm.VisibleIndex = 8;
        this.colDetLocNm.Width = 90;
        //
        // colDetStockYn
        //
        this.colDetStockYn.Caption = "재고";
        this.colDetStockYn.ColumnEdit = this.chkcolStock;
        this.colDetStockYn.FieldName = "stock_yn";
        this.colDetStockYn.Name = "colDetStockYn";
        this.colDetStockYn.OptionsColumn.AllowEdit = false;
        this.colDetStockYn.Visible = true;
        this.colDetStockYn.VisibleIndex = 9;
        this.colDetStockYn.Width = 45;
        //
        // colDetSrcNo
        //
        this.colDetSrcNo.Caption = "요청번호";
        this.colDetSrcNo.FieldName = "src_no";
        this.colDetSrcNo.Name = "colDetSrcNo";
        this.colDetSrcNo.OptionsColumn.AllowEdit = false;
        this.colDetSrcNo.Visible = true;
        this.colDetSrcNo.VisibleIndex = 10;
        this.colDetSrcNo.Width = 110;
        //
        // colDetRemark
        //
        this.colDetRemark.Caption = "비고";
        this.colDetRemark.FieldName = "remark";
        this.colDetRemark.Name = "colDetRemark";
        this.colDetRemark.OptionsColumn.AllowEdit = false;
        this.colDetRemark.Visible = true;
        this.colDetRemark.VisibleIndex = 11;
        this.colDetRemark.Width = 180;
        //
        // chkcolStock
        //
        this.chkcolStock.AutoHeight = false;
        this.chkcolStock.Name = "chkcolStock";
        this.chkcolStock.ValueChecked = "Y";
        this.chkcolStock.ValueUnchecked = "N";
        //
        // panelTop
        //
        this.panelTop.Appearance.BackColor = System.Drawing.Color.White;
        this.panelTop.Appearance.Options.UseBackColor = true;
        this.panelTop.Controls.Add(this.grd1);
        this.panelTop.Controls.Add(this.panelWyn1);
        this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelTop.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
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
        this.hyperlinkOutNo,
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
        this.colOutNo,
        this.colOutDate,
        this.colStatCd,
        this.colTransType,
        this.colDeptNm,
        this.colEmpNm,
        this.colLineCnt,
        this.colTotalQty,
        this.colCfmDt,
        this.colRemark});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsBehavior.Editable = false;
        this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        //
        // colOutNo
        //
        this.colOutNo.Caption = "출고번호";
        this.colOutNo.ColumnEdit = this.hyperlinkOutNo;
        this.colOutNo.FieldName = "out_no";
        this.colOutNo.Name = "colOutNo";
        this.colOutNo.OptionsColumn.AllowEdit = false;
        this.colOutNo.Visible = true;
        this.colOutNo.VisibleIndex = 0;
        this.colOutNo.Width = 110;
        //
        // colOutDate
        //
        this.colOutDate.Caption = "출고일자";
        this.colOutDate.FieldName = "out_date";
        this.colOutDate.Name = "colOutDate";
        this.colOutDate.OptionsColumn.AllowEdit = false;
        this.colOutDate.Visible = true;
        this.colOutDate.VisibleIndex = 1;
        this.colOutDate.Width = 90;
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
        // colTransType
        //
        this.colTransType.Caption = "출고유형";
        this.colTransType.ColumnEdit = this.lookupcolType;
        this.colTransType.FieldName = "trans_type";
        this.colTransType.Name = "colTransType";
        this.colTransType.OptionsColumn.AllowEdit = false;
        this.colTransType.Visible = true;
        this.colTransType.VisibleIndex = 3;
        this.colTransType.Width = 100;
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
        this.colEmpNm.Caption = "담당자";
        this.colEmpNm.FieldName = "emp_nm";
        this.colEmpNm.Name = "colEmpNm";
        this.colEmpNm.OptionsColumn.AllowEdit = false;
        this.colEmpNm.Visible = true;
        this.colEmpNm.VisibleIndex = 5;
        this.colEmpNm.Width = 80;
        //
        // colLineCnt
        //
        this.colLineCnt.Caption = "품목수";
        this.colLineCnt.DisplayFormat.FormatString = "#,##0.####";
        this.colLineCnt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colLineCnt.FieldName = "line_cnt";
        this.colLineCnt.Name = "colLineCnt";
        this.colLineCnt.OptionsColumn.AllowEdit = false;
        this.colLineCnt.Visible = true;
        this.colLineCnt.VisibleIndex = 6;
        this.colLineCnt.Width = 60;
        //
        // colTotalQty
        //
        this.colTotalQty.Caption = "총출고수량";
        this.colTotalQty.DisplayFormat.FormatString = "#,##0.####";
        this.colTotalQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colTotalQty.FieldName = "total_qty";
        this.colTotalQty.Name = "colTotalQty";
        this.colTotalQty.OptionsColumn.AllowEdit = false;
        this.colTotalQty.Visible = true;
        this.colTotalQty.VisibleIndex = 7;
        this.colTotalQty.Width = 90;
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
        this.colCfmDt.VisibleIndex = 8;
        this.colCfmDt.Width = 120;
        //
        // colRemark
        //
        this.colRemark.Caption = "비고";
        this.colRemark.FieldName = "remark";
        this.colRemark.Name = "colRemark";
        this.colRemark.OptionsColumn.AllowEdit = false;
        this.colRemark.Visible = true;
        this.colRemark.VisibleIndex = 9;
        this.colRemark.Width = 200;
        //
        // hyperlinkOutNo
        //
        this.hyperlinkOutNo.Name = "hyperlinkOutNo";
        //
        // lookupcolStatCd
        //
        this.lookupcolStatCd.AutoHeight = false;
        this.lookupcolStatCd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolStatCd.LookupKey = "L_MA0012";
        this.lookupcolStatCd.Name = "lookupcolStatCd";
        this.lookupcolStatCd.NullText = "";
        this.lookupcolStatCd.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
        //
        // lookupcolType
        //
        this.lookupcolType.AutoHeight = false;
        this.lookupcolType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolType.LookupKey = "L_MA0011";
        this.lookupcolType.Name = "lookupcolType";
        this.lookupcolType.NullText = "";
        this.lookupcolType.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
        //
        // panelWyn1
        //
        this.panelWyn1.Appearance.BackColor = System.Drawing.Color.White;
        this.panelWyn1.Appearance.Options.UseBackColor = true;
        this.panelWyn1.Controls.Add(this.sectionHeaderWyn1);
        this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
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
        this.sectionHeaderWyn1.Text = "기타출고 LIST";
        //
        // paTitle
        //
        this.paTitle.Appearance.BackColor = System.Drawing.Color.White;
        this.paTitle.Appearance.Options.UseBackColor = true;
        this.paTitle.Controls.Add(this.sectionHeaderWyn2);
        this.paTitle.Dock = System.Windows.Forms.DockStyle.Top;
        this.paTitle.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.paTitle.Location = new System.Drawing.Point(5, 0);
        this.paTitle.Name = "paTitle";
        this.paTitle.Size = new System.Drawing.Size(1670, 33);
        this.paTitle.TabIndex = 8;
        //
        // sectionHeaderWyn2
        //
        this.sectionHeaderWyn2.BackColor = System.Drawing.Color.White;
        this.sectionHeaderWyn2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.sectionHeaderWyn2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.sectionHeaderWyn2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
        this.sectionHeaderWyn2.Location = new System.Drawing.Point(5, 0);
        this.sectionHeaderWyn2.Name = "sectionHeaderWyn2";
        this.sectionHeaderWyn2.Size = new System.Drawing.Size(1670, 33);
        this.sectionHeaderWyn2.TabIndex = 9;
        this.sectionHeaderWyn2.Text = "기타출고현황 [frmEtcOutList]";
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
        this.panHeader.Controls.Add(this.lblSearchOutNo);
        this.panHeader.Controls.Add(this.txtSearchOutNo);
        this.panHeader.Controls.Add(this.lblSearchTransType);
        this.panHeader.Controls.Add(this.cboSearchTransType);
        this.panHeader.Controls.Add(this.lblSearchStatCd);
        this.panHeader.Controls.Add(this.cboSearchStatCd);
        this.panHeader.Controls.Add(this.lblSearchKeyword);
        this.panHeader.Controls.Add(this.txtSearchKeyword);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panHeader.Location = new System.Drawing.Point(5, 33);
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
        // cboSearchAccId (사업장 LookUp, Required - 기본값은 화면 생성자에서 로그인 사업장)
        //
        this.cboSearchAccId.EditValue = "";
        this.cboSearchAccId.Location = new System.Drawing.Point(60, 16);
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
        // lblSearchFrom
        //
        this.lblSearchFrom.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchFrom.Appearance.Options.UseFont = true;
        this.lblSearchFrom.Location = new System.Drawing.Point(215, 19);
        this.lblSearchFrom.Name = "lblSearchFrom";
        this.lblSearchFrom.Size = new System.Drawing.Size(48, 15);
        this.lblSearchFrom.TabIndex = 2;
        this.lblSearchFrom.Text = "출고일자";
        //
        // dteSearchFrom
        //
        this.dteSearchFrom.EditValue = null;
        this.dteSearchFrom.Location = new System.Drawing.Point(271, 16);
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
        this.dteSearchFrom.TabIndex = 3;
        this.dteSearchFrom.YyyyMmDd = null;
        //
        // dteSearchTo
        //
        this.dteSearchTo.EditValue = null;
        this.dteSearchTo.Location = new System.Drawing.Point(372, 16);
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
        this.dteSearchTo.TabIndex = 4;
        this.dteSearchTo.YyyyMmDd = null;
        //
        // lblSearchOutNo
        //
        this.lblSearchOutNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchOutNo.Appearance.Options.UseFont = true;
        this.lblSearchOutNo.Location = new System.Drawing.Point(489, 19);
        this.lblSearchOutNo.Name = "lblSearchOutNo";
        this.lblSearchOutNo.Size = new System.Drawing.Size(48, 15);
        this.lblSearchOutNo.TabIndex = 5;
        this.lblSearchOutNo.Text = "출고번호";
        //
        // txtSearchOutNo
        //
        this.txtSearchOutNo.Location = new System.Drawing.Point(543, 16);
        this.txtSearchOutNo.Name = "txtSearchOutNo";
        this.txtSearchOutNo.Size = new System.Drawing.Size(110, 20);
        this.txtSearchOutNo.TabIndex = 6;
        //
        // lblSearchTransType
        //
        this.lblSearchTransType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchTransType.Appearance.Options.UseFont = true;
        this.lblSearchTransType.Location = new System.Drawing.Point(673, 19);
        this.lblSearchTransType.Name = "lblSearchTransType";
        this.lblSearchTransType.Size = new System.Drawing.Size(48, 15);
        this.lblSearchTransType.TabIndex = 7;
        this.lblSearchTransType.Text = "출고유형";
        //
        // cboSearchTransType
        //
        this.cboSearchTransType.EditValue = "";
        this.cboSearchTransType.Location = new System.Drawing.Point(727, 16);
        this.cboSearchTransType.LookupKey = "L_MA0011_O";
        this.cboSearchTransType.Name = "cboSearchTransType";
        this.cboSearchTransType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.cboSearchTransType.Properties.NullText = "";
        this.cboSearchTransType.Size = new System.Drawing.Size(120, 20);
        this.cboSearchTransType.TabIndex = 8;
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
        this.cboSearchStatCd.EditValue = "";
        this.cboSearchStatCd.Location = new System.Drawing.Point(921, 16);
        this.cboSearchStatCd.LookupKey = "L_MA0012";
        this.cboSearchStatCd.Name = "cboSearchStatCd";
        this.cboSearchStatCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.cboSearchStatCd.Properties.NullText = "";
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
        // frmEtcOutList
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 760);
        this.Controls.Add(this.panBase);
        this.Name = "frmEtcOutList";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelBottom)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelTop)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.hyperlinkOutNo)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolStatCd)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolType)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkcolStock)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchOutNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchTransType.Properties)).EndInit();
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
    private PanelWyn paTitle;
    private SectionHeaderWyn sectionHeaderWyn4;
    private SectionHeaderWyn sectionHeaderWyn1;
    private SectionHeaderWyn sectionHeaderWyn2;
    private SplitterWyn splitterWyn1;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit hyperlinkOutNo;
    private LookUpColumnEdit lookupcolStatCd;
    private LookUpColumnEdit lookupcolType;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkcolStock;
    private DevExpress.XtraGrid.Columns.GridColumn colOutNo;
    private DevExpress.XtraGrid.Columns.GridColumn colOutDate;
    private DevExpress.XtraGrid.Columns.GridColumn colStatCd;
    private DevExpress.XtraGrid.Columns.GridColumn colTransType;
    private DevExpress.XtraGrid.Columns.GridColumn colDeptNm;
    private DevExpress.XtraGrid.Columns.GridColumn colEmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn colLineCnt;
    private DevExpress.XtraGrid.Columns.GridColumn colTotalQty;
    private DevExpress.XtraGrid.Columns.GridColumn colCfmDt;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private DevExpress.XtraGrid.Columns.GridColumn colDetSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colDetItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colDetItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colDetItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colDetUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colDetOutQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDetLotNo;
    private DevExpress.XtraGrid.Columns.GridColumn colDetWhNm;
    private DevExpress.XtraGrid.Columns.GridColumn colDetLocNm;
    private DevExpress.XtraGrid.Columns.GridColumn colDetStockYn;
    private DevExpress.XtraGrid.Columns.GridColumn colDetSrcNo;
    private DevExpress.XtraGrid.Columns.GridColumn colDetRemark;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private LookUpEditWyn cboSearchAccId;
    private DevExpress.XtraEditors.LabelControl lblSearchFrom;
    private DateEditWyn dteSearchFrom;
    private DateEditWyn dteSearchTo;
    private DevExpress.XtraEditors.LabelControl lblSearchOutNo;
    private TextEditWyn txtSearchOutNo;
    private DevExpress.XtraEditors.LabelControl lblSearchTransType;
    private LookUpEditWyn cboSearchTransType;
    private DevExpress.XtraEditors.LabelControl lblSearchStatCd;
    private LookUpEditWyn cboSearchStatCd;
    private DevExpress.XtraEditors.LabelControl lblSearchKeyword;
    private TextEditWyn txtSearchKeyword;
}
