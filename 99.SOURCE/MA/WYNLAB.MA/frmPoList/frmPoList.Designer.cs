// AI Builder Master-SubGrid 템플릿(TplMasterSubGrid) 기반 - frmPoReqList와 완전히 같은 구조
// (grd1 위/grd2 아래, 2026-09-22 위/아래 배치 지침 반영), 대상만 TMAPOM/TMAPOD. 발주는 확정
// 가격이 있어서 grd2(품목 상세)에 단가/합계금액 컬럼이 추가된다(구매요청 현황엔 없음).
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

public partial class frmPoList
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPoList));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelSplit = new WYNLAB.Base.Controls.PanelWyn();
            this.panelBottom = new WYNLAB.Base.Controls.PanelWyn();
            this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colDetSerl = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetNextQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetRemainQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetPrice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetTotalAmt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetDelvDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelTop = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colPoNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.hyperlinkPoNo = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.colPoDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPoTitle = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookupcolStatCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colPoType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookupcolPoType = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colCustNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDeptNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAppNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colApprStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.lblSearchPoNo = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchPoNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSearchPoTitle = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchPoTitle = new WYNLAB.Base.Controls.TextEditWyn();
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
            this.paTitle1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.lblSearchFrom = new DevExpress.XtraEditors.LabelControl();
            this.dteSearchFrom = new WYNLAB.Base.Controls.DateEditWyn();
            this.dteSearchTo = new WYNLAB.Base.Controls.DateEditWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelSplit)).BeginInit();
            this.panelSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelBottom)).BeginInit();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelTop)).BeginInit();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hyperlinkPoNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolStatCd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolPoType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchPoNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchPoTitle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).BeginInit();
            this.paTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).BeginInit();
            this.paTitle1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panBase
            // 
            this.panBase.Appearance.BackColor = System.Drawing.Color.White;
            this.panBase.Appearance.Options.UseBackColor = true;
            this.panBase.Controls.Add(this.panelSplit);
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
            // panelSplit
            // 
            this.panelSplit.Appearance.BackColor = System.Drawing.Color.White;
            this.panelSplit.Appearance.Options.UseBackColor = true;
            this.panelSplit.Controls.Add(this.panelBottom);
            this.panelSplit.Controls.Add(this.splitterWyn1);
            this.panelSplit.Controls.Add(this.panelTop);
            this.panelSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSplit.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelSplit.Location = new System.Drawing.Point(5, 82);
            this.panelSplit.Name = "panelSplit";
            this.panelSplit.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.panelSplit.Size = new System.Drawing.Size(1670, 673);
            this.panelSplit.TabIndex = 7;
            // 
            // panelBottom
            // 
            this.panelBottom.Appearance.BackColor = System.Drawing.Color.White;
            this.panelBottom.Appearance.Options.UseBackColor = true;
            this.panelBottom.Controls.Add(this.grd2);
            this.panelBottom.Controls.Add(this.paTitle1);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelBottom.Location = new System.Drawing.Point(3, 386);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelBottom.Size = new System.Drawing.Size(1664, 287);
            this.panelBottom.TabIndex = 2;
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
            this.grd2.Size = new System.Drawing.Size(1664, 252);
            this.grd2.TabIndex = 1;
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
            this.colDetQty,
            this.colDetNextQty,
            this.colDetRemainQty,
            this.colDetUnitCd,
            this.colDetPrice,
            this.colDetTotalAmt,
            this.colDetDelvDate,
            this.colDetWhNm});
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
            this.colDetSerl.Visible = true;
            this.colDetSerl.VisibleIndex = 0;
            this.colDetSerl.Width = 50;
            // 
            // colDetItemNo
            // 
            this.colDetItemNo.Caption = "품번";
            this.colDetItemNo.FieldName = "item_no";
            this.colDetItemNo.Name = "colDetItemNo";
            this.colDetItemNo.Visible = true;
            this.colDetItemNo.VisibleIndex = 1;
            this.colDetItemNo.Width = 90;
            // 
            // colDetItemNm
            // 
            this.colDetItemNm.Caption = "품명";
            this.colDetItemNm.FieldName = "item_nm";
            this.colDetItemNm.Name = "colDetItemNm";
            this.colDetItemNm.Visible = true;
            this.colDetItemNm.VisibleIndex = 2;
            this.colDetItemNm.Width = 130;
            // 
            // colDetItemSpec
            // 
            this.colDetItemSpec.Caption = "규격";
            this.colDetItemSpec.FieldName = "item_spec";
            this.colDetItemSpec.Name = "colDetItemSpec";
            this.colDetItemSpec.Visible = true;
            this.colDetItemSpec.VisibleIndex = 3;
            this.colDetItemSpec.Width = 100;
            // 
            // colDetQty
            // 
            this.colDetQty.Caption = "발주수량";
            this.colDetQty.DisplayFormat.FormatString = "#,##0.####";
            this.colDetQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDetQty.FieldName = "qty";
            this.colDetQty.Name = "colDetQty";
            this.colDetQty.Visible = true;
            this.colDetQty.VisibleIndex = 4;
            this.colDetQty.Width = 80;
            // 
            // colDetNextQty
            // 
            this.colDetNextQty.Caption = "진행수량";
            this.colDetNextQty.DisplayFormat.FormatString = "#,##0.####";
            this.colDetNextQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDetNextQty.FieldName = "next_qty";
            this.colDetNextQty.Name = "colDetNextQty";
            this.colDetNextQty.Visible = true;
            this.colDetNextQty.VisibleIndex = 5;
            this.colDetNextQty.Width = 80;
            // 
            // colDetRemainQty
            // 
            this.colDetRemainQty.Caption = "잔량";
            this.colDetRemainQty.DisplayFormat.FormatString = "#,##0.####";
            this.colDetRemainQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDetRemainQty.FieldName = "remain_qty";
            this.colDetRemainQty.Name = "colDetRemainQty";
            this.colDetRemainQty.Visible = true;
            this.colDetRemainQty.VisibleIndex = 6;
            this.colDetRemainQty.Width = 70;
            // 
            // colDetUnitCd
            // 
            this.colDetUnitCd.Caption = "단위";
            this.colDetUnitCd.FieldName = "unit_cd";
            this.colDetUnitCd.Name = "colDetUnitCd";
            this.colDetUnitCd.Visible = true;
            this.colDetUnitCd.VisibleIndex = 7;
            this.colDetUnitCd.Width = 60;
            // 
            // colDetPrice
            // 
            this.colDetPrice.Caption = "단가";
            this.colDetPrice.DisplayFormat.FormatString = "#,##0.####";
            this.colDetPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDetPrice.FieldName = "price";
            this.colDetPrice.Name = "colDetPrice";
            this.colDetPrice.Visible = true;
            this.colDetPrice.VisibleIndex = 8;
            this.colDetPrice.Width = 80;
            // 
            // colDetTotalAmt
            // 
            this.colDetTotalAmt.Caption = "합계금액";
            this.colDetTotalAmt.DisplayFormat.FormatString = "#,##0.####";
            this.colDetTotalAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDetTotalAmt.FieldName = "total_amt";
            this.colDetTotalAmt.Name = "colDetTotalAmt";
            this.colDetTotalAmt.Visible = true;
            this.colDetTotalAmt.VisibleIndex = 9;
            this.colDetTotalAmt.Width = 90;
            // 
            // colDetDelvDate
            // 
            this.colDetDelvDate.Caption = "납기일";
            this.colDetDelvDate.FieldName = "delv_date";
            this.colDetDelvDate.Name = "colDetDelvDate";
            this.colDetDelvDate.Visible = true;
            this.colDetDelvDate.VisibleIndex = 10;
            this.colDetDelvDate.Width = 90;
            // 
            // colDetWhNm
            // 
            this.colDetWhNm.Caption = "창고";
            this.colDetWhNm.FieldName = "wh_nm";
            this.colDetWhNm.Name = "colDetWhNm";
            this.colDetWhNm.Visible = true;
            this.colDetWhNm.VisibleIndex = 11;
            this.colDetWhNm.Width = 90;
            // 
            // panelTop
            // 
            this.panelTop.Appearance.BackColor = System.Drawing.Color.White;
            this.panelTop.Appearance.Options.UseBackColor = true;
            this.panelTop.Controls.Add(this.grd1);
            this.panelTop.Controls.Add(this.panelWyn1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelTop.Location = new System.Drawing.Point(3, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelTop.Size = new System.Drawing.Size(1664, 380);
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
            this.hyperlinkPoNo,
            this.lookupcolStatCd,
            this.lookupcolPoType});
            this.grd1.Size = new System.Drawing.Size(1664, 345);
            this.grd1.TabIndex = 1;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colPoNo,
            this.colPoDate,
            this.colPoTitle,
            this.colStatCd,
            this.colPoType,
            this.colCustNm,
            this.colDeptNm,
            this.colEmpNm,
            this.colAppNo,
            this.colApprStatCd});
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
            this.colPoNo.ColumnEdit = this.hyperlinkPoNo;
            this.colPoNo.FieldName = "po_no";
            this.colPoNo.Name = "colPoNo";
            this.colPoNo.Visible = true;
            this.colPoNo.VisibleIndex = 0;
            this.colPoNo.Width = 110;
            // 
            // hyperlinkPoNo
            // 
            this.hyperlinkPoNo.Name = "hyperlinkPoNo";
            // 
            // colPoDate
            // 
            this.colPoDate.Caption = "발주일자";
            this.colPoDate.FieldName = "po_date";
            this.colPoDate.Name = "colPoDate";
            this.colPoDate.Visible = true;
            this.colPoDate.VisibleIndex = 1;
            this.colPoDate.Width = 90;
            // 
            // colPoTitle
            // 
            this.colPoTitle.Caption = "발주명";
            this.colPoTitle.FieldName = "po_title";
            this.colPoTitle.Name = "colPoTitle";
            this.colPoTitle.Visible = true;
            this.colPoTitle.VisibleIndex = 2;
            this.colPoTitle.Width = 200;
            // 
            // colStatCd
            // 
            this.colStatCd.Caption = "진행상태";
            this.colStatCd.ColumnEdit = this.lookupcolStatCd;
            this.colStatCd.FieldName = "stat_cd";
            this.colStatCd.Name = "colStatCd";
            this.colStatCd.Visible = true;
            this.colStatCd.VisibleIndex = 3;
            this.colStatCd.Width = 80;
            // 
            // lookupcolStatCd
            // 
            this.lookupcolStatCd.AutoHeight = false;
            this.lookupcolStatCd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookupcolStatCd.LookupKey = "L_MA0002";
            this.lookupcolStatCd.Name = "lookupcolStatCd";
            this.lookupcolStatCd.NullText = "";
            this.lookupcolStatCd.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colPoType
            // 
            this.colPoType.Caption = "발주구분";
            this.colPoType.ColumnEdit = this.lookupcolPoType;
            this.colPoType.FieldName = "po_type";
            this.colPoType.Name = "colPoType";
            this.colPoType.Visible = true;
            this.colPoType.VisibleIndex = 4;
            this.colPoType.Width = 90;
            // 
            // lookupcolPoType
            // 
            this.lookupcolPoType.AutoHeight = false;
            this.lookupcolPoType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookupcolPoType.LookupKey = "L_MA0003";
            this.lookupcolPoType.Name = "lookupcolPoType";
            this.lookupcolPoType.NullText = "";
            this.lookupcolPoType.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colCustNm
            // 
            this.colCustNm.Caption = "거래처";
            this.colCustNm.FieldName = "cust_nm";
            this.colCustNm.Name = "colCustNm";
            this.colCustNm.Visible = true;
            this.colCustNm.VisibleIndex = 5;
            this.colCustNm.Width = 120;
            // 
            // colDeptNm
            // 
            this.colDeptNm.Caption = "부서";
            this.colDeptNm.FieldName = "dept_nm";
            this.colDeptNm.Name = "colDeptNm";
            this.colDeptNm.Visible = true;
            this.colDeptNm.VisibleIndex = 6;
            this.colDeptNm.Width = 100;
            // 
            // colEmpNm
            // 
            this.colEmpNm.Caption = "담당자";
            this.colEmpNm.FieldName = "emp_nm";
            this.colEmpNm.Name = "colEmpNm";
            this.colEmpNm.Visible = true;
            this.colEmpNm.VisibleIndex = 7;
            this.colEmpNm.Width = 90;
            // 
            // colAppNo
            // 
            this.colAppNo.Caption = "결재번호";
            this.colAppNo.FieldName = "app_no";
            this.colAppNo.Name = "colAppNo";
            this.colAppNo.Visible = true;
            this.colAppNo.VisibleIndex = 8;
            this.colAppNo.Width = 90;
            // 
            // colApprStatCd
            // 
            this.colApprStatCd.Caption = "결재상태";
            this.colApprStatCd.FieldName = "appr_stat_cd";
            this.colApprStatCd.Name = "colApprStatCd";
            this.colApprStatCd.Visible = true;
            this.colApprStatCd.VisibleIndex = 9;
            this.colApprStatCd.Width = 80;
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.lblSearchFrom);
            this.panHeader.Controls.Add(this.dteSearchFrom);
            this.panHeader.Controls.Add(this.dteSearchTo);
            this.panHeader.Controls.Add(this.lblSearchPoNo);
            this.panHeader.Controls.Add(this.txtSearchPoNo);
            this.panHeader.Controls.Add(this.lblSearchPoTitle);
            this.panHeader.Controls.Add(this.txtSearchPoTitle);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 33);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1670, 49);
            this.panHeader.TabIndex = 8;
            // 
            // lblSearchPoNo
            // 
            this.lblSearchPoNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchPoNo.Appearance.Options.UseFont = true;
            this.lblSearchPoNo.Location = new System.Drawing.Point(311, 18);
            this.lblSearchPoNo.Name = "lblSearchPoNo";
            this.lblSearchPoNo.Size = new System.Drawing.Size(48, 15);
            this.lblSearchPoNo.TabIndex = 0;
            this.lblSearchPoNo.Text = "발주번호";
            // 
            // txtSearchPoNo
            // 
            this.txtSearchPoNo.Location = new System.Drawing.Point(367, 15);
            this.txtSearchPoNo.Name = "txtSearchPoNo";
            this.txtSearchPoNo.Size = new System.Drawing.Size(150, 20);
            this.txtSearchPoNo.TabIndex = 2;
            // 
            // lblSearchPoTitle
            // 
            this.lblSearchPoTitle.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchPoTitle.Appearance.Options.UseFont = true;
            this.lblSearchPoTitle.Location = new System.Drawing.Point(556, 18);
            this.lblSearchPoTitle.Name = "lblSearchPoTitle";
            this.lblSearchPoTitle.Size = new System.Drawing.Size(36, 15);
            this.lblSearchPoTitle.TabIndex = 2;
            this.lblSearchPoTitle.Text = "발주명";
            // 
            // txtSearchPoTitle
            // 
            this.txtSearchPoTitle.Location = new System.Drawing.Point(598, 15);
            this.txtSearchPoTitle.Name = "txtSearchPoTitle";
            this.txtSearchPoTitle.Size = new System.Drawing.Size(200, 20);
            this.txtSearchPoTitle.TabIndex = 3;
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
            this.paTitle.TabIndex = 5;
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
            this.sectionHeaderWyn4.Text = "구매발주 품목상세";
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
            this.panelWyn1.Size = new System.Drawing.Size(1664, 27);
            this.panelWyn1.TabIndex = 14;
            // 
            // sectionHeaderWyn1
            // 
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1659, 25);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 8;
            this.sectionHeaderWyn1.Text = "구매발주 LIST";
            // 
            // splitterWyn1
            // 
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitterWyn1.Location = new System.Drawing.Point(3, 380);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(1664, 6);
            this.splitterWyn1.TabIndex = 3;
            this.splitterWyn1.TabStop = false;
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
            this.sectionHeaderWyn2.TabIndex = 12;
            this.sectionHeaderWyn2.Text = "구매발주현황 [frmPoList]";
            // 
            // lblSearchFrom
            // 
            this.lblSearchFrom.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblSearchFrom.Appearance.Options.UseFont = true;
            this.lblSearchFrom.Location = new System.Drawing.Point(29, 18);
            this.lblSearchFrom.Name = "lblSearchFrom";
            this.lblSearchFrom.Size = new System.Drawing.Size(48, 15);
            this.lblSearchFrom.TabIndex = 7;
            this.lblSearchFrom.Text = "발주일자";
            // 
            // dteSearchFrom
            // 
            this.dteSearchFrom.EditValue = new System.DateTime(2026, 9, 27, 0, 0, 0, 0);
            this.dteSearchFrom.Location = new System.Drawing.Point(83, 15);
            this.dteSearchFrom.Name = "dteSearchFrom";
            this.dteSearchFrom.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.dteSearchFrom.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.dteSearchFrom.Properties.Appearance.Options.UseBackColor = true;
            this.dteSearchFrom.Properties.Appearance.Options.UseForeColor = true;
            this.dteSearchFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteSearchFrom.Required = true;
            this.dteSearchFrom.Size = new System.Drawing.Size(100, 20);
            this.dteSearchFrom.TabIndex = 0;
            this.dteSearchFrom.YyyyMmDd = "20260927";
            // 
            // dteSearchTo
            // 
            this.dteSearchTo.EditValue = new System.DateTime(2026, 9, 27, 0, 0, 0, 0);
            this.dteSearchTo.Location = new System.Drawing.Point(184, 15);
            this.dteSearchTo.Name = "dteSearchTo";
            this.dteSearchTo.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.dteSearchTo.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.dteSearchTo.Properties.Appearance.Options.UseBackColor = true;
            this.dteSearchTo.Properties.Appearance.Options.UseForeColor = true;
            this.dteSearchTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteSearchTo.Required = true;
            this.dteSearchTo.Size = new System.Drawing.Size(100, 20);
            this.dteSearchTo.TabIndex = 1;
            this.dteSearchTo.YyyyMmDd = "20260927";
            // 
            // frmPoList
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1680, 760);
            this.Controls.Add(this.panBase);
            this.Name = "frmPoList";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelSplit)).EndInit();
            this.panelSplit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelBottom)).EndInit();
            this.panelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelTop)).EndInit();
            this.panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hyperlinkPoNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolStatCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolPoType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchPoNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchPoTitle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
            this.paTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).EndInit();
            this.paTitle1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).EndInit();
            this.ResumeLayout(false);

    }

    private PanelWyn panBase;
    private PanelWyn panelSplit;
    private PanelWyn panelTop;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colPoNo;
    private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit hyperlinkPoNo;
    private DevExpress.XtraGrid.Columns.GridColumn colPoDate;
    private DevExpress.XtraGrid.Columns.GridColumn colPoTitle;
    private DevExpress.XtraGrid.Columns.GridColumn colStatCd;
    private LookUpColumnEdit lookupcolStatCd;
    private DevExpress.XtraGrid.Columns.GridColumn colPoType;
    private LookUpColumnEdit lookupcolPoType;
    private DevExpress.XtraGrid.Columns.GridColumn colCustNm;
    private DevExpress.XtraGrid.Columns.GridColumn colDeptNm;
    private DevExpress.XtraGrid.Columns.GridColumn colEmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn colAppNo;
    private DevExpress.XtraGrid.Columns.GridColumn colApprStatCd;
    private PanelWyn panelBottom;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private DevExpress.XtraGrid.Columns.GridColumn colDetSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colDetItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colDetItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colDetItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colDetQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDetNextQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDetRemainQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDetUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colDetPrice;
    private DevExpress.XtraGrid.Columns.GridColumn colDetTotalAmt;
    private DevExpress.XtraGrid.Columns.GridColumn colDetDelvDate;
    private DevExpress.XtraGrid.Columns.GridColumn colDetWhNm;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchPoNo;
    private TextEditWyn txtSearchPoNo;
    private DevExpress.XtraEditors.LabelControl lblSearchPoTitle;
    private TextEditWyn txtSearchPoTitle;
    private PanelWyn paTitle;
    private PanelWyn paTitle1;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn panelWyn1;
    private SectionHeaderWyn sectionHeaderWyn1;
    private SplitterWyn splitterWyn1;
    private SectionHeaderWyn sectionHeaderWyn2;
    private DevExpress.XtraEditors.LabelControl lblSearchFrom;
    private DateEditWyn dteSearchFrom;
    private DateEditWyn dteSearchTo;
}
