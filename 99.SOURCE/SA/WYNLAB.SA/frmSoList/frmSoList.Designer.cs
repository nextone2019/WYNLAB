// 수주현황 - frmPoList(구매발주현황)와 같은 Master-SubGrid 패턴(grd1 목록 위/grd2 품목상세 아래).
// 좌표는 기본값이고 VS 디자이너로 자유롭게 조정 가능합니다.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.SA;

public partial class frmSoList
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSoList));
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
            this.colDetGiQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetBillQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetRemainQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetPrice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetTotalAmt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetDelvDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelTop = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colSoNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.hyperlinkSoNo = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.colSoDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSoTitle = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookupcolStatCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colCustNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDeptNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAppNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colApprStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
            this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblSearchSoNo = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchSoNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSearchSoTitle = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchSoTitle = new WYNLAB.Base.Controls.TextEditWyn();
            this.paTitleH = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
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
            ((System.ComponentModel.ISupportInitialize)(this.hyperlinkSoNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolStatCd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchSoNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchSoTitle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).BeginInit();
            this.paTitleH.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
            this.panelWyn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
            this.SuspendLayout();
            //
            // panBase
            //
            this.panBase.Appearance.BackColor = System.Drawing.Color.White;
            this.panBase.Appearance.Options.UseBackColor = true;
            this.panBase.Controls.Add(this.panelSplit);
            this.panBase.Controls.Add(this.panHeader);
            this.panBase.Controls.Add(this.paTitleH);
            this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBase.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panBase.Location = new System.Drawing.Point(0, 0);
            this.panBase.Name = "panBase";
            this.panBase.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.panBase.Size = new System.Drawing.Size(1400, 800);
            this.panBase.TabIndex = 0;
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
            this.panelSplit.Location = new System.Drawing.Point(5, 74);
            this.panelSplit.Name = "panelSplit";
            this.panelSplit.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.panelSplit.Size = new System.Drawing.Size(1390, 721);
            this.panelSplit.TabIndex = 7;
            //
            // panelBottom
            //
            this.panelBottom.Appearance.BackColor = System.Drawing.Color.White;
            this.panelBottom.Appearance.Options.UseBackColor = true;
            this.panelBottom.Controls.Add(this.grd2);
            this.panelBottom.Controls.Add(this.panelWyn2);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelBottom.Location = new System.Drawing.Point(3, 416);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelBottom.Size = new System.Drawing.Size(1384, 305);
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
            this.grd2.Size = new System.Drawing.Size(1384, 270);
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
            this.colDetGiQty,
            this.colDetBillQty,
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
            this.colDetQty.Caption = "수주수량";
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
            this.colDetNextQty.Caption = "명세서누계";
            this.colDetNextQty.DisplayFormat.FormatString = "#,##0.####";
            this.colDetNextQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDetNextQty.FieldName = "next_qty";
            this.colDetNextQty.Name = "colDetNextQty";
            this.colDetNextQty.Visible = true;
            this.colDetNextQty.VisibleIndex = 5;
            this.colDetNextQty.Width = 80;
            //
            // colDetGiQty
            //
            this.colDetGiQty.Caption = "출고누계";
            this.colDetGiQty.DisplayFormat.FormatString = "#,##0.####";
            this.colDetGiQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDetGiQty.FieldName = "gi_qty";
            this.colDetGiQty.Name = "colDetGiQty";
            this.colDetGiQty.Visible = true;
            this.colDetGiQty.VisibleIndex = 6;
            this.colDetGiQty.Width = 80;
            //
            // colDetBillQty
            //
            this.colDetBillQty.Caption = "매출누계";
            this.colDetBillQty.DisplayFormat.FormatString = "#,##0.####";
            this.colDetBillQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDetBillQty.FieldName = "bill_qty";
            this.colDetBillQty.Name = "colDetBillQty";
            this.colDetBillQty.Visible = true;
            this.colDetBillQty.VisibleIndex = 7;
            this.colDetBillQty.Width = 80;
            //
            // colDetRemainQty
            //
            this.colDetRemainQty.Caption = "명세서 잔량";
            this.colDetRemainQty.DisplayFormat.FormatString = "#,##0.####";
            this.colDetRemainQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDetRemainQty.FieldName = "remain_qty";
            this.colDetRemainQty.Name = "colDetRemainQty";
            this.colDetRemainQty.Visible = true;
            this.colDetRemainQty.VisibleIndex = 9;
            this.colDetRemainQty.Width = 70;
            //
            // colDetUnitCd
            //
            this.colDetUnitCd.Caption = "단위";
            this.colDetUnitCd.FieldName = "unit_cd";
            this.colDetUnitCd.Name = "colDetUnitCd";
            this.colDetUnitCd.Visible = true;
            this.colDetUnitCd.VisibleIndex = 10;
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
            this.colDetPrice.VisibleIndex = 11;
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
            this.colDetTotalAmt.VisibleIndex = 12;
            this.colDetTotalAmt.Width = 90;
            //
            // colDetDelvDate
            //
            this.colDetDelvDate.Caption = "납기일";
            this.colDetDelvDate.FieldName = "delv_date";
            this.colDetDelvDate.Name = "colDetDelvDate";
            this.colDetDelvDate.Visible = true;
            this.colDetDelvDate.VisibleIndex = 13;
            this.colDetDelvDate.Width = 90;
            //
            // colDetWhNm
            //
            this.colDetWhNm.Caption = "창고";
            this.colDetWhNm.FieldName = "wh_nm";
            this.colDetWhNm.Name = "colDetWhNm";
            this.colDetWhNm.Visible = true;
            this.colDetWhNm.VisibleIndex = 14;
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
            this.panelTop.Size = new System.Drawing.Size(1384, 410);
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
            this.hyperlinkSoNo,
            this.lookupcolStatCd});
            this.grd1.Size = new System.Drawing.Size(1384, 375);
            this.grd1.TabIndex = 1;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            //
            // gvw1
            //
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colSoNo,
            this.colSoDate,
            this.colSoTitle,
            this.colStatCd,
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
            // colSoNo
            //
            this.colSoNo.Caption = "수주번호";
            this.colSoNo.ColumnEdit = this.hyperlinkSoNo;
            this.colSoNo.FieldName = "so_no";
            this.colSoNo.Name = "colSoNo";
            this.colSoNo.Visible = true;
            this.colSoNo.VisibleIndex = 0;
            this.colSoNo.Width = 110;
            //
            // hyperlinkSoNo
            //
            this.hyperlinkSoNo.Name = "hyperlinkSoNo";
            //
            // colSoDate
            //
            this.colSoDate.Caption = "수주일자";
            this.colSoDate.FieldName = "so_date";
            this.colSoDate.Name = "colSoDate";
            this.colSoDate.Visible = true;
            this.colSoDate.VisibleIndex = 1;
            this.colSoDate.Width = 90;
            //
            // colSoTitle
            //
            this.colSoTitle.Caption = "건명";
            this.colSoTitle.FieldName = "so_title";
            this.colSoTitle.Name = "colSoTitle";
            this.colSoTitle.Visible = true;
            this.colSoTitle.VisibleIndex = 2;
            this.colSoTitle.Width = 200;
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
            // colCustNm
            //
            this.colCustNm.Caption = "거래처";
            this.colCustNm.FieldName = "cust_nm";
            this.colCustNm.Name = "colCustNm";
            this.colCustNm.Visible = true;
            this.colCustNm.VisibleIndex = 4;
            this.colCustNm.Width = 120;
            //
            // colDeptNm
            //
            this.colDeptNm.Caption = "부서";
            this.colDeptNm.FieldName = "dept_nm";
            this.colDeptNm.Name = "colDeptNm";
            this.colDeptNm.Visible = true;
            this.colDeptNm.VisibleIndex = 5;
            this.colDeptNm.Width = 100;
            //
            // colEmpNm
            //
            this.colEmpNm.Caption = "담당자";
            this.colEmpNm.FieldName = "emp_nm";
            this.colEmpNm.Name = "colEmpNm";
            this.colEmpNm.Visible = true;
            this.colEmpNm.VisibleIndex = 6;
            this.colEmpNm.Width = 90;
            //
            // colAppNo
            //
            this.colAppNo.Caption = "결재번호";
            this.colAppNo.FieldName = "app_no";
            this.colAppNo.Name = "colAppNo";
            this.colAppNo.Visible = true;
            this.colAppNo.VisibleIndex = 7;
            this.colAppNo.Width = 90;
            //
            // colApprStatCd
            //
            this.colApprStatCd.Caption = "결재상태";
            this.colApprStatCd.FieldName = "appr_stat_cd";
            this.colApprStatCd.Name = "colApprStatCd";
            this.colApprStatCd.Visible = true;
            this.colApprStatCd.VisibleIndex = 8;
            this.colApprStatCd.Width = 80;
            //
            // panHeader
            //
            this.panHeader.Appearance.BackColor = System.Drawing.Color.White;
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.Controls.Add(this.lblSearchAccId);
            this.panHeader.Controls.Add(this.cboSearchAccId);
            this.panHeader.Controls.Add(this.lblSearchSoNo);
            this.panHeader.Controls.Add(this.txtSearchSoNo);
            this.panHeader.Controls.Add(this.lblSearchSoTitle);
            this.panHeader.Controls.Add(this.txtSearchSoTitle);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 25);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1390, 49);
            this.panHeader.TabIndex = 8;
            // 
            // lblSearchAccId (조회조건 첫 번째 - 사업장 표준, 2026-10-03)
            // 
            this.lblSearchAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSearchAccId.Appearance.Options.UseFont = true;
            this.lblSearchAccId.Location = new System.Drawing.Point(19, 20);
            this.lblSearchAccId.Name = "lblSearchAccId";
            this.lblSearchAccId.Size = new System.Drawing.Size(36, 15);
            this.lblSearchAccId.TabIndex = 0;
            this.lblSearchAccId.Text = "사업장";
            // 
            // cboSearchAccId (사업장 LookUp, Required - 기본값은 화면 생성자에서 로그인 사업장)
            // 
            this.cboSearchAccId.EditValue = "";
            this.cboSearchAccId.Location = new System.Drawing.Point(63, 17);
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
            // lblSearchSoNo
            //
            this.lblSearchSoNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblSearchSoNo.Appearance.Options.UseFont = true;
            this.lblSearchSoNo.Location = new System.Drawing.Point(218, 20);
            this.lblSearchSoNo.Name = "lblSearchSoNo";
            this.lblSearchSoNo.Size = new System.Drawing.Size(48, 15);
            this.lblSearchSoNo.TabIndex = 0;
            this.lblSearchSoNo.Text = "수주번호";
            //
            // txtSearchSoNo
            //
            this.txtSearchSoNo.Location = new System.Drawing.Point(274, 17);
            this.txtSearchSoNo.Name = "txtSearchSoNo";
            this.txtSearchSoNo.Size = new System.Drawing.Size(160, 20);
            this.txtSearchSoNo.TabIndex = 1;
            //
            // lblSearchSoTitle
            //
            this.lblSearchSoTitle.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblSearchSoTitle.Appearance.Options.UseFont = true;
            this.lblSearchSoTitle.Location = new System.Drawing.Point(454, 20);
            this.lblSearchSoTitle.Name = "lblSearchSoTitle";
            this.lblSearchSoTitle.Size = new System.Drawing.Size(30, 15);
            this.lblSearchSoTitle.TabIndex = 2;
            this.lblSearchSoTitle.Text = "건명";
            //
            // txtSearchSoTitle
            //
            this.txtSearchSoTitle.Location = new System.Drawing.Point(494, 17);
            this.txtSearchSoTitle.Name = "txtSearchSoTitle";
            this.txtSearchSoTitle.Size = new System.Drawing.Size(200, 20);
            this.txtSearchSoTitle.TabIndex = 3;
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
            this.paTitleH.Size = new System.Drawing.Size(1390, 25);
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
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1385, 25);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 14;
            this.sectionHeaderWyn1.Text = "수주현황 [frmSoList]";
            //
            // panelWyn1
            //
            this.panelWyn1.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn1.Appearance.Options.UseBackColor = true;
            this.panelWyn1.Controls.Add(this.sectionHeaderWyn2);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(0, 8);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn1.Size = new System.Drawing.Size(1384, 27);
            this.panelWyn1.TabIndex = 14;
            //
            // sectionHeaderWyn2
            //
            this.sectionHeaderWyn2.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn2.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn2.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn2.Name = "sectionHeaderWyn2";
            this.sectionHeaderWyn2.Size = new System.Drawing.Size(1379, 25);
            this.sectionHeaderWyn2.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn2.SvgIcon")));
            this.sectionHeaderWyn2.TabIndex = 8;
            this.sectionHeaderWyn2.Text = "수주 LIST";
            //
            // splitterWyn1
            //
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitterWyn1.Location = new System.Drawing.Point(3, 410);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(1384, 6);
            this.splitterWyn1.TabIndex = 3;
            this.splitterWyn1.TabStop = false;
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
            this.panelWyn2.Size = new System.Drawing.Size(1384, 27);
            this.panelWyn2.TabIndex = 13;
            //
            // sectionHeaderWyn4
            //
            this.sectionHeaderWyn4.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn4.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn4.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn4.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn4.Name = "sectionHeaderWyn4";
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(1379, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 8;
            this.sectionHeaderWyn4.Text = "수주 품목상세";
            //
            // frmSoList
            //
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Controls.Add(this.panBase);
            this.Name = "frmSoList";
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
            ((System.ComponentModel.ISupportInitialize)(this.hyperlinkSoNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolStatCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchSoNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchSoTitle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).EndInit();
            this.paTitleH.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
            this.panelWyn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
            this.ResumeLayout(false);

    }

    private PanelWyn panBase;
    private PanelWyn panelSplit;
    private PanelWyn panelTop;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colSoNo;
    private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit hyperlinkSoNo;
    private DevExpress.XtraGrid.Columns.GridColumn colSoDate;
    private DevExpress.XtraGrid.Columns.GridColumn colSoTitle;
    private DevExpress.XtraGrid.Columns.GridColumn colStatCd;
    private LookUpColumnEdit lookupcolStatCd;
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
    private DevExpress.XtraGrid.Columns.GridColumn colDetGiQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDetBillQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDetRemainQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDetUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colDetPrice;
    private DevExpress.XtraGrid.Columns.GridColumn colDetTotalAmt;
    private DevExpress.XtraGrid.Columns.GridColumn colDetDelvDate;
    private DevExpress.XtraGrid.Columns.GridColumn colDetWhNm;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchSoNo;
    private TextEditWyn txtSearchSoNo;
    private DevExpress.XtraEditors.LabelControl lblSearchSoTitle;
    private TextEditWyn txtSearchSoTitle;
    private PanelWyn paTitleH;
    private SectionHeaderWyn sectionHeaderWyn1;
    private PanelWyn panelWyn1;
    private SectionHeaderWyn sectionHeaderWyn2;
    private SplitterWyn splitterWyn1;
    private PanelWyn panelWyn2;
    private SectionHeaderWyn sectionHeaderWyn4;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private WYNLAB.Base.Controls.LookUpEditWyn cboSearchAccId;
}
