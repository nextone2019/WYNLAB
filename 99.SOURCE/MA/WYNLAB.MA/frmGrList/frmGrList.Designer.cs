// 구매입고현황(frmGrList) - Master-SubGrid 조회 화면. 입고번호를 더블클릭하면 frmGr가 그 건으로 열린다(2026-09-25).
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

public partial class frmGrList
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGrList));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelBottom = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colDetSerl = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetGrQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetNextQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetLotNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetLocNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetStockYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkcolStock = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colDetSrcType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookupcolSrc = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colDetSrcNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetPoNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetRemark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelTop = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colGrNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.hyperlinkGrNo = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.colGrDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookupcolStatCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colTransType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookupcolType = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colCustNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDeptNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLineCnt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotalQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAutoYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkcolAuto = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colCfmDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
            this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblSearchGrNo = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchGrNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSearchKeyword = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchKeyword = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSearchFrom = new DevExpress.XtraEditors.LabelControl();
            this.dteSearchFrom = new WYNLAB.Base.Controls.DateEditWyn();
            this.dteSearchTo = new WYNLAB.Base.Controls.DateEditWyn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelBottom)).BeginInit();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
            this.panelWyn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkcolStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolSrc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelTop)).BeginInit();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hyperlinkGrNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolStatCd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkcolAuto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchGrNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).BeginInit();
            this.paTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
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
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 9;
            this.sectionHeaderWyn4.Text = "구매입고품목 상세현황";
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
            this.chkcolStock,
            this.lookupcolSrc});
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
            this.colDetGrQty,
            this.colDetNextQty,
            this.colDetLotNo,
            this.colDetWhNm,
            this.colDetLocNm,
            this.colDetStockYn,
            this.colDetSrcType,
            this.colDetSrcNo,
            this.colDetPoNo,
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
            // colDetGrQty
            // 
            this.colDetGrQty.Caption = "입고수량";
            this.colDetGrQty.DisplayFormat.FormatString = "#,##0.####";
            this.colDetGrQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDetGrQty.FieldName = "gr_qty";
            this.colDetGrQty.Name = "colDetGrQty";
            this.colDetGrQty.OptionsColumn.AllowEdit = false;
            this.colDetGrQty.Visible = true;
            this.colDetGrQty.VisibleIndex = 5;
            this.colDetGrQty.Width = 90;
            // 
            // colDetNextQty
            // 
            this.colDetNextQty.Caption = "후속처리";
            this.colDetNextQty.DisplayFormat.FormatString = "#,##0.####";
            this.colDetNextQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDetNextQty.FieldName = "next_qty";
            this.colDetNextQty.Name = "colDetNextQty";
            this.colDetNextQty.OptionsColumn.AllowEdit = false;
            this.colDetNextQty.Visible = true;
            this.colDetNextQty.VisibleIndex = 6;
            this.colDetNextQty.Width = 80;
            // 
            // colDetLotNo
            // 
            this.colDetLotNo.Caption = "LOT";
            this.colDetLotNo.FieldName = "lot_no";
            this.colDetLotNo.Name = "colDetLotNo";
            this.colDetLotNo.OptionsColumn.AllowEdit = false;
            this.colDetLotNo.Visible = true;
            this.colDetLotNo.VisibleIndex = 7;
            this.colDetLotNo.Width = 90;
            // 
            // colDetWhNm
            // 
            this.colDetWhNm.Caption = "창고";
            this.colDetWhNm.FieldName = "wh_nm";
            this.colDetWhNm.Name = "colDetWhNm";
            this.colDetWhNm.OptionsColumn.AllowEdit = false;
            this.colDetWhNm.Visible = true;
            this.colDetWhNm.VisibleIndex = 8;
            this.colDetWhNm.Width = 90;
            // 
            // colDetLocNm
            // 
            this.colDetLocNm.Caption = "위치";
            this.colDetLocNm.FieldName = "loc_nm";
            this.colDetLocNm.Name = "colDetLocNm";
            this.colDetLocNm.OptionsColumn.AllowEdit = false;
            this.colDetLocNm.Visible = true;
            this.colDetLocNm.VisibleIndex = 9;
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
            this.colDetStockYn.VisibleIndex = 10;
            this.colDetStockYn.Width = 45;
            // 
            // chkcolStock
            // 
            this.chkcolStock.AutoHeight = false;
            this.chkcolStock.Name = "chkcolStock";
            this.chkcolStock.ValueChecked = "Y";
            this.chkcolStock.ValueUnchecked = "N";
            // 
            // colDetSrcType
            // 
            this.colDetSrcType.Caption = "입고원천";
            this.colDetSrcType.ColumnEdit = this.lookupcolSrc;
            this.colDetSrcType.FieldName = "src_type";
            this.colDetSrcType.Name = "colDetSrcType";
            this.colDetSrcType.OptionsColumn.AllowEdit = false;
            this.colDetSrcType.Visible = true;
            this.colDetSrcType.VisibleIndex = 11;
            this.colDetSrcType.Width = 80;
            // 
            // lookupcolSrc
            // 
            this.lookupcolSrc.AutoHeight = false;
            this.lookupcolSrc.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookupcolSrc.LookupKey = "L_MA0004";
            this.lookupcolSrc.Name = "lookupcolSrc";
            this.lookupcolSrc.NullText = "";
            this.lookupcolSrc.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colDetSrcNo
            // 
            this.colDetSrcNo.Caption = "원천번호";
            this.colDetSrcNo.FieldName = "src_no";
            this.colDetSrcNo.Name = "colDetSrcNo";
            this.colDetSrcNo.OptionsColumn.AllowEdit = false;
            this.colDetSrcNo.Visible = true;
            this.colDetSrcNo.VisibleIndex = 12;
            this.colDetSrcNo.Width = 110;
            // 
            // colDetPoNo
            // 
            this.colDetPoNo.Caption = "발주번호";
            this.colDetPoNo.FieldName = "po_no";
            this.colDetPoNo.Name = "colDetPoNo";
            this.colDetPoNo.OptionsColumn.AllowEdit = false;
            this.colDetPoNo.Visible = true;
            this.colDetPoNo.VisibleIndex = 13;
            this.colDetPoNo.Width = 110;
            // 
            // colDetRemark
            // 
            this.colDetRemark.Caption = "비고";
            this.colDetRemark.FieldName = "remark";
            this.colDetRemark.Name = "colDetRemark";
            this.colDetRemark.OptionsColumn.AllowEdit = false;
            this.colDetRemark.Visible = true;
            this.colDetRemark.VisibleIndex = 14;
            this.colDetRemark.Width = 160;
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
            this.hyperlinkGrNo,
            this.lookupcolStatCd,
            this.lookupcolType,
            this.chkcolAuto});
            this.grd1.Size = new System.Drawing.Size(1670, 345);
            this.grd1.TabIndex = 1;
            this.grd1.UseEmbeddedNavigator = false;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colGrNo,
            this.colGrDate,
            this.colStatCd,
            this.colTransType,
            this.colCustNm,
            this.colDeptNm,
            this.colEmpNm,
            this.colLineCnt,
            this.colTotalQty,
            this.colAutoYn,
            this.colCfmDt});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightFocusedRow = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // colGrNo
            // 
            this.colGrNo.Caption = "입고번호";
            this.colGrNo.ColumnEdit = this.hyperlinkGrNo;
            this.colGrNo.FieldName = "gr_no";
            this.colGrNo.Name = "colGrNo";
            this.colGrNo.OptionsColumn.AllowEdit = false;
            this.colGrNo.Visible = true;
            this.colGrNo.VisibleIndex = 0;
            this.colGrNo.Width = 110;
            // 
            // hyperlinkGrNo
            // 
            this.hyperlinkGrNo.Name = "hyperlinkGrNo";
            // 
            // colGrDate
            // 
            this.colGrDate.Caption = "입고일자";
            this.colGrDate.FieldName = "gr_date";
            this.colGrDate.Name = "colGrDate";
            this.colGrDate.OptionsColumn.AllowEdit = false;
            this.colGrDate.Visible = true;
            this.colGrDate.VisibleIndex = 1;
            this.colGrDate.Width = 90;
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
            // colTransType
            // 
            this.colTransType.Caption = "수불유형";
            this.colTransType.ColumnEdit = this.lookupcolType;
            this.colTransType.FieldName = "trans_type";
            this.colTransType.Name = "colTransType";
            this.colTransType.OptionsColumn.AllowEdit = false;
            this.colTransType.Visible = true;
            this.colTransType.VisibleIndex = 3;
            this.colTransType.Width = 90;
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
            // colCustNm
            // 
            this.colCustNm.Caption = "거래처";
            this.colCustNm.FieldName = "cust_nm";
            this.colCustNm.Name = "colCustNm";
            this.colCustNm.OptionsColumn.AllowEdit = false;
            this.colCustNm.Visible = true;
            this.colCustNm.VisibleIndex = 4;
            this.colCustNm.Width = 140;
            // 
            // colDeptNm
            // 
            this.colDeptNm.Caption = "부서";
            this.colDeptNm.FieldName = "dept_nm";
            this.colDeptNm.Name = "colDeptNm";
            this.colDeptNm.OptionsColumn.AllowEdit = false;
            this.colDeptNm.Visible = true;
            this.colDeptNm.VisibleIndex = 5;
            this.colDeptNm.Width = 100;
            // 
            // colEmpNm
            // 
            this.colEmpNm.Caption = "담당자";
            this.colEmpNm.FieldName = "emp_nm";
            this.colEmpNm.Name = "colEmpNm";
            this.colEmpNm.OptionsColumn.AllowEdit = false;
            this.colEmpNm.Visible = true;
            this.colEmpNm.VisibleIndex = 6;
            this.colEmpNm.Width = 90;
            // 
            // colLineCnt
            // 
            this.colLineCnt.Caption = "품목수";
            this.colLineCnt.FieldName = "line_cnt";
            this.colLineCnt.Name = "colLineCnt";
            this.colLineCnt.OptionsColumn.AllowEdit = false;
            this.colLineCnt.Visible = true;
            this.colLineCnt.VisibleIndex = 7;
            this.colLineCnt.Width = 60;
            // 
            // colTotalQty
            // 
            this.colTotalQty.Caption = "입고수량";
            this.colTotalQty.DisplayFormat.FormatString = "#,##0.####";
            this.colTotalQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTotalQty.FieldName = "total_qty";
            this.colTotalQty.Name = "colTotalQty";
            this.colTotalQty.OptionsColumn.AllowEdit = false;
            this.colTotalQty.Visible = true;
            this.colTotalQty.VisibleIndex = 8;
            this.colTotalQty.Width = 80;
            // 
            // colAutoYn
            // 
            this.colAutoYn.Caption = "자동";
            this.colAutoYn.ColumnEdit = this.chkcolAuto;
            this.colAutoYn.FieldName = "auto_yn";
            this.colAutoYn.Name = "colAutoYn";
            this.colAutoYn.OptionsColumn.AllowEdit = false;
            this.colAutoYn.Visible = true;
            this.colAutoYn.VisibleIndex = 9;
            this.colAutoYn.Width = 45;
            // 
            // chkcolAuto
            // 
            this.chkcolAuto.AutoHeight = false;
            this.chkcolAuto.Name = "chkcolAuto";
            this.chkcolAuto.ValueChecked = "Y";
            this.chkcolAuto.ValueUnchecked = "N";
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
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.lblSearchAccId);
            this.panHeader.Controls.Add(this.cboSearchAccId);
            this.panHeader.Controls.Add(this.lblSearchGrNo);
            this.panHeader.Controls.Add(this.txtSearchGrNo);
            this.panHeader.Controls.Add(this.lblSearchKeyword);
            this.panHeader.Controls.Add(this.txtSearchKeyword);
            this.panHeader.Controls.Add(this.lblSearchFrom);
            this.panHeader.Controls.Add(this.dteSearchFrom);
            this.panHeader.Controls.Add(this.dteSearchTo);
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
            // lblSearchGrNo
            // 
            this.lblSearchGrNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblSearchGrNo.Appearance.Options.UseFont = true;
            this.lblSearchGrNo.Location = new System.Drawing.Point(489, 19);
            this.lblSearchGrNo.Name = "lblSearchGrNo";
            this.lblSearchGrNo.Size = new System.Drawing.Size(48, 15);
            this.lblSearchGrNo.TabIndex = 0;
            this.lblSearchGrNo.Text = "입고번호";
            // 
            // txtSearchGrNo
            // 
            this.txtSearchGrNo.Location = new System.Drawing.Point(543, 16);
            this.txtSearchGrNo.Name = "txtSearchGrNo";
            this.txtSearchGrNo.Size = new System.Drawing.Size(130, 20);
            this.txtSearchGrNo.TabIndex = 2;
            // 
            // lblSearchKeyword
            // 
            this.lblSearchKeyword.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblSearchKeyword.Appearance.Options.UseFont = true;
            this.lblSearchKeyword.Location = new System.Drawing.Point(699, 19);
            this.lblSearchKeyword.Name = "lblSearchKeyword";
            this.lblSearchKeyword.Size = new System.Drawing.Size(65, 15);
            this.lblSearchKeyword.TabIndex = 2;
            this.lblSearchKeyword.Text = "거래처/품목";
            // 
            // txtSearchKeyword
            // 
            this.txtSearchKeyword.Location = new System.Drawing.Point(770, 16);
            this.txtSearchKeyword.Name = "txtSearchKeyword";
            this.txtSearchKeyword.Size = new System.Drawing.Size(160, 20);
            this.txtSearchKeyword.TabIndex = 3;
            // 
            // lblSearchFrom
            // 
            this.lblSearchFrom.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblSearchFrom.Appearance.Options.UseFont = true;
            this.lblSearchFrom.Location = new System.Drawing.Point(215, 19);
            this.lblSearchFrom.Name = "lblSearchFrom";
            this.lblSearchFrom.Size = new System.Drawing.Size(48, 15);
            this.lblSearchFrom.TabIndex = 4;
            this.lblSearchFrom.Text = "입고일자";
            // 
            // dteSearchFrom
            // 
            this.dteSearchFrom.EditValue = new System.DateTime(2026, 9, 27, 0, 0, 0, 0);
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
            this.dteSearchFrom.TabIndex = 0;
            this.dteSearchFrom.YyyyMmDd = "20260927";
            // 
            // dteSearchTo
            // 
            this.dteSearchTo.EditValue = new System.DateTime(2026, 9, 27, 0, 0, 0, 0);
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
            this.dteSearchTo.TabIndex = 1;
            this.dteSearchTo.YyyyMmDd = "20260927";
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
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 9;
            this.sectionHeaderWyn1.Text = "구매입고LIST";
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
            this.sectionHeaderWyn2.Location = new System.Drawing.Point(0, 0);
            this.sectionHeaderWyn2.Name = "sectionHeaderWyn2";
            this.sectionHeaderWyn2.Size = new System.Drawing.Size(1670, 33);
            this.sectionHeaderWyn2.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn2.SvgIcon")));
            this.sectionHeaderWyn2.TabIndex = 9;
            this.sectionHeaderWyn2.Text = "구매입고현황 [frmGrList]";
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
            // frmGrList
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1680, 760);
            this.Controls.Add(this.panBase);
            this.Name = "frmGrList";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelBottom)).EndInit();
            this.panelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
            this.panelWyn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkcolStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolSrc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelTop)).EndInit();
            this.panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hyperlinkGrNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolStatCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkcolAuto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchGrNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
            this.paTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
            this.ResumeLayout(false);

    }

    private PanelWyn panBase;
    private PanelWyn panelTop;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit hyperlinkGrNo;
    private LookUpColumnEdit lookupcolStatCd;
    private LookUpColumnEdit lookupcolType;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkcolAuto;
    private DevExpress.XtraGrid.Columns.GridColumn colGrNo;
    private DevExpress.XtraGrid.Columns.GridColumn colGrDate;
    private DevExpress.XtraGrid.Columns.GridColumn colStatCd;
    private DevExpress.XtraGrid.Columns.GridColumn colTransType;
    private DevExpress.XtraGrid.Columns.GridColumn colCustNm;
    private DevExpress.XtraGrid.Columns.GridColumn colDeptNm;
    private DevExpress.XtraGrid.Columns.GridColumn colEmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn colLineCnt;
    private DevExpress.XtraGrid.Columns.GridColumn colTotalQty;
    private DevExpress.XtraGrid.Columns.GridColumn colAutoYn;
    private DevExpress.XtraGrid.Columns.GridColumn colCfmDt;
    private PanelWyn panelBottom;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkcolStock;
    private LookUpColumnEdit lookupcolSrc;
    private DevExpress.XtraGrid.Columns.GridColumn colDetSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colDetItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colDetItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colDetItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colDetUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colDetGrQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDetNextQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDetLotNo;
    private DevExpress.XtraGrid.Columns.GridColumn colDetWhNm;
    private DevExpress.XtraGrid.Columns.GridColumn colDetLocNm;
    private DevExpress.XtraGrid.Columns.GridColumn colDetStockYn;
    private DevExpress.XtraGrid.Columns.GridColumn colDetSrcType;
    private DevExpress.XtraGrid.Columns.GridColumn colDetSrcNo;
    private DevExpress.XtraGrid.Columns.GridColumn colDetPoNo;
    private DevExpress.XtraGrid.Columns.GridColumn colDetRemark;
    private DevExpress.XtraEditors.LabelControl lblSearchGrNo;
    private TextEditWyn txtSearchGrNo;
    private DevExpress.XtraEditors.LabelControl lblSearchKeyword;
    private TextEditWyn txtSearchKeyword;
    private DevExpress.XtraEditors.LabelControl lblSearchFrom;
    private DateEditWyn dteSearchFrom;
    private DateEditWyn dteSearchTo;
    private PanelWyn panHeader;
    private PanelWyn panelWyn2;
    private SectionHeaderWyn sectionHeaderWyn4;
    private SplitterWyn splitterWyn1;
    private PanelWyn panelWyn1;
    private SectionHeaderWyn sectionHeaderWyn1;
    private PanelWyn paTitle;
    private SectionHeaderWyn sectionHeaderWyn2;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private WYNLAB.Base.Controls.LookUpEditWyn cboSearchAccId;
}
