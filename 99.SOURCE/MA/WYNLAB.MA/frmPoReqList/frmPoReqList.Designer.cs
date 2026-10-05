// AI Builder Master-SubGrid 템플릿(TplMasterSubGrid) 기반 - 좌측 grd1(구매요청 목록, 조회전용)
// 선택에 따라 우측 grd2(품목 상세, 조회전용)가 재조회된다. 좌측 목록의 구매요청번호는
// 하이퍼링크로 표시되고, 더블클릭하면 frmPoReq를 그 건으로 열어 조회한다(2026-09-22).
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

public partial class frmPoReqList
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPoReqList));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
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
            this.colDetDelvDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.paTitle1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelTop = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colReqNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.hyperlinkReqNo = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.colReqDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colReqTitle = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookupcolStatCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colPoType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookupcolPoType = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colCustNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDeptNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAppNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colApprStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
            this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblSearchFrom = new DevExpress.XtraEditors.LabelControl();
            this.dteSearchFrom = new WYNLAB.Base.Controls.DateEditWyn();
            this.dteSearchTo = new WYNLAB.Base.Controls.DateEditWyn();
            this.lblSearchReqNo = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchReqNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSearchReqTitle = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchReqTitle = new WYNLAB.Base.Controls.TextEditWyn();
            this.paTitleH = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelBottom)).BeginInit();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).BeginInit();
            this.paTitle1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelTop)).BeginInit();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hyperlinkReqNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolStatCd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolPoType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchReqNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchReqTitle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).BeginInit();
            this.paTitleH.SuspendLayout();
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
            this.panBase.Controls.Add(this.paTitleH);
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
            this.panelBottom.Controls.Add(this.paTitle1);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelBottom.Location = new System.Drawing.Point(5, 462);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelBottom.Size = new System.Drawing.Size(1670, 293);
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
            this.grd2.Size = new System.Drawing.Size(1670, 258);
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
            this.colDetQty.Caption = "구매요청수량";
            this.colDetQty.DisplayFormat.FormatString = "#,##0.####";
            this.colDetQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDetQty.FieldName = "qty";
            this.colDetQty.Name = "colDetQty";
            this.colDetQty.Visible = true;
            this.colDetQty.VisibleIndex = 4;
            this.colDetQty.Width = 90;
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
            this.colDetRemainQty.Caption = "발주잔량";
            this.colDetRemainQty.DisplayFormat.FormatString = "#,##0.####";
            this.colDetRemainQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDetRemainQty.FieldName = "remain_qty";
            this.colDetRemainQty.Name = "colDetRemainQty";
            this.colDetRemainQty.Visible = true;
            this.colDetRemainQty.VisibleIndex = 6;
            this.colDetRemainQty.Width = 80;
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
            // colDetDelvDate
            // 
            this.colDetDelvDate.Caption = "요청납기일";
            this.colDetDelvDate.FieldName = "delv_date";
            this.colDetDelvDate.Name = "colDetDelvDate";
            this.colDetDelvDate.Visible = true;
            this.colDetDelvDate.VisibleIndex = 8;
            this.colDetDelvDate.Width = 90;
            // 
            // colDetWhNm
            // 
            this.colDetWhNm.Caption = "창고";
            this.colDetWhNm.FieldName = "wh_nm";
            this.colDetWhNm.Name = "colDetWhNm";
            this.colDetWhNm.Visible = true;
            this.colDetWhNm.VisibleIndex = 9;
            this.colDetWhNm.Width = 90;
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
            this.sectionHeaderWyn4.Text = "구매요청 품목상세";
            // 
            // splitterWyn1
            // 
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitterWyn1.Location = new System.Drawing.Point(5, 454);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(1670, 8);
            this.splitterWyn1.TabIndex = 9;
            this.splitterWyn1.TabStop = false;
            // 
            // panelTop
            // 
            this.panelTop.Appearance.BackColor = System.Drawing.Color.White;
            this.panelTop.Appearance.Options.UseBackColor = true;
            this.panelTop.Controls.Add(this.grd1);
            this.panelTop.Controls.Add(this.panelWyn1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelTop.Location = new System.Drawing.Point(5, 74);
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
            this.hyperlinkReqNo,
            this.lookupcolStatCd,
            this.lookupcolPoType});
            this.grd1.Size = new System.Drawing.Size(1670, 345);
            this.grd1.TabIndex = 1;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colReqNo,
            this.colReqDate,
            this.colReqTitle,
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
            // colReqNo
            // 
            this.colReqNo.Caption = "구매요청번호";
            this.colReqNo.ColumnEdit = this.hyperlinkReqNo;
            this.colReqNo.FieldName = "req_no";
            this.colReqNo.Name = "colReqNo";
            this.colReqNo.Visible = true;
            this.colReqNo.VisibleIndex = 0;
            this.colReqNo.Width = 110;
            // 
            // hyperlinkReqNo
            // 
            this.hyperlinkReqNo.Name = "hyperlinkReqNo";
            // 
            // colReqDate
            // 
            this.colReqDate.Caption = "요청일자";
            this.colReqDate.FieldName = "req_date";
            this.colReqDate.Name = "colReqDate";
            this.colReqDate.Visible = true;
            this.colReqDate.VisibleIndex = 1;
            this.colReqDate.Width = 90;
            // 
            // colReqTitle
            // 
            this.colReqTitle.Caption = "구매요청명";
            this.colReqTitle.FieldName = "req_title";
            this.colReqTitle.Name = "colReqTitle";
            this.colReqTitle.Visible = true;
            this.colReqTitle.VisibleIndex = 2;
            this.colReqTitle.Width = 200;
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
            this.lookupcolStatCd.LookupKey = "L_CM0007";
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
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1665, 25);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 8;
            this.sectionHeaderWyn1.Text = "구매요청 LIST";
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
            this.panHeader.Controls.Add(this.lblSearchReqNo);
            this.panHeader.Controls.Add(this.txtSearchReqNo);
            this.panHeader.Controls.Add(this.lblSearchReqTitle);
            this.panHeader.Controls.Add(this.txtSearchReqTitle);
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
            this.lblSearchAccId.Location = new System.Drawing.Point(17, 19);
            this.lblSearchAccId.Name = "lblSearchAccId";
            this.lblSearchAccId.Size = new System.Drawing.Size(36, 15);
            this.lblSearchAccId.TabIndex = 0;
            this.lblSearchAccId.Text = "사업장";
            // 
            // cboSearchAccId (사업장 LookUp, Required - 기본값은 화면 생성자에서 로그인 사업장)
            // 
            this.cboSearchAccId.EditValue = "";
            this.cboSearchAccId.Location = new System.Drawing.Point(61, 16);
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
            this.lblSearchFrom.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblSearchFrom.Appearance.Options.UseFont = true;
            this.lblSearchFrom.Location = new System.Drawing.Point(216, 19);
            this.lblSearchFrom.Name = "lblSearchFrom";
            this.lblSearchFrom.Size = new System.Drawing.Size(48, 15);
            this.lblSearchFrom.TabIndex = 4;
            this.lblSearchFrom.Text = "요청일자";
            // 
            // dteSearchFrom
            // 
            this.dteSearchFrom.EditValue = new System.DateTime(2026, 9, 27, 0, 0, 0, 0);
            this.dteSearchFrom.Location = new System.Drawing.Point(270, 16);
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
            this.dteSearchTo.Location = new System.Drawing.Point(371, 16);
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
            // lblSearchReqNo
            // 
            this.lblSearchReqNo.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblSearchReqNo.Appearance.Options.UseFont = true;
            this.lblSearchReqNo.Location = new System.Drawing.Point(494, 19);
            this.lblSearchReqNo.Name = "lblSearchReqNo";
            this.lblSearchReqNo.Size = new System.Drawing.Size(72, 15);
            this.lblSearchReqNo.TabIndex = 0;
            this.lblSearchReqNo.Text = "구매요청번호";
            // 
            // txtSearchReqNo
            // 
            this.txtSearchReqNo.Location = new System.Drawing.Point(571, 16);
            this.txtSearchReqNo.Name = "txtSearchReqNo";
            this.txtSearchReqNo.Size = new System.Drawing.Size(150, 20);
            this.txtSearchReqNo.TabIndex = 2;
            // 
            // lblSearchReqTitle
            // 
            this.lblSearchReqTitle.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblSearchReqTitle.Appearance.Options.UseFont = true;
            this.lblSearchReqTitle.Location = new System.Drawing.Point(745, 19);
            this.lblSearchReqTitle.Name = "lblSearchReqTitle";
            this.lblSearchReqTitle.Size = new System.Drawing.Size(60, 15);
            this.lblSearchReqTitle.TabIndex = 2;
            this.lblSearchReqTitle.Text = "구매요청명";
            // 
            // txtSearchReqTitle
            // 
            this.txtSearchReqTitle.Location = new System.Drawing.Point(811, 16);
            this.txtSearchReqTitle.Name = "txtSearchReqTitle";
            this.txtSearchReqTitle.Size = new System.Drawing.Size(200, 20);
            this.txtSearchReqTitle.TabIndex = 3;
            // 
            // paTitleH
            // 
            this.paTitleH.Appearance.BackColor = System.Drawing.Color.White;
            this.paTitleH.Appearance.Options.UseBackColor = true;
            this.paTitleH.Controls.Add(this.sectionHeaderWyn2);
            this.paTitleH.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitleH.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitleH.Location = new System.Drawing.Point(5, 0);
            this.paTitleH.Name = "paTitleH";
            this.paTitleH.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.paTitleH.Size = new System.Drawing.Size(1670, 25);
            this.paTitleH.TabIndex = 8;
            // 
            // sectionHeaderWyn2
            // 
            this.sectionHeaderWyn2.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn2.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn2.Name = "sectionHeaderWyn2";
            this.sectionHeaderWyn2.Size = new System.Drawing.Size(1665, 25);
            this.sectionHeaderWyn2.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn2.SvgIcon")));
            this.sectionHeaderWyn2.TabIndex = 11;
            this.sectionHeaderWyn2.Text = "품목현황 [frmItemList]";
            // 
            // frmPoReqList
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1680, 760);
            this.Controls.Add(this.panBase);
            this.Name = "frmPoReqList";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelBottom)).EndInit();
            this.panelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).EndInit();
            this.paTitle1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelTop)).EndInit();
            this.panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hyperlinkReqNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolStatCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupcolPoType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchReqNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchReqTitle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).EndInit();
            this.paTitleH.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
            this.ResumeLayout(false);

    }

    private PanelWyn panBase;
    private PanelWyn panelTop;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colReqNo;
    private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit hyperlinkReqNo;
    private DevExpress.XtraGrid.Columns.GridColumn colReqDate;
    private DevExpress.XtraGrid.Columns.GridColumn colReqTitle;
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
    private DevExpress.XtraGrid.Columns.GridColumn colDetDelvDate;
    private DevExpress.XtraGrid.Columns.GridColumn colDetWhNm;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchReqNo;
    private TextEditWyn txtSearchReqNo;
    private DevExpress.XtraEditors.LabelControl lblSearchReqTitle;
    private TextEditWyn txtSearchReqTitle;
    private PanelWyn paTitle1;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn panelWyn1;
    private SectionHeaderWyn sectionHeaderWyn1;
    private PanelWyn paTitleH;
    private SectionHeaderWyn sectionHeaderWyn2;
    private SplitterWyn splitterWyn1;
    private DevExpress.XtraEditors.LabelControl lblSearchFrom;
    private DateEditWyn dteSearchFrom;
    private DateEditWyn dteSearchTo;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private WYNLAB.Base.Controls.LookUpEditWyn cboSearchAccId;
}
