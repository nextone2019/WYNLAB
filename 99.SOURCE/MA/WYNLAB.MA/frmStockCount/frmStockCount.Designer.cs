// 재고실사등록(frmStockCount) - Master + 대상창고 + 라인 그리드(TMACNTM/W/D). 대상선별 시 현재고를 스냅샷으로 라인에 펼치고, 선별된 라인은 삭제하지 않는 한 전부 실사수량을 입력해야 입력완료/확정된다. 확정하면 차이만큼 조정 수불(ADJ_IN/ADJ_OUT)이 생긴다.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

public partial class frmStockCount
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
        this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
        this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
        this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
        this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
        this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
        this.featBar = new WYNLAB.Popup.FeatureBarWyn();
        this.panData = new WYNLAB.Base.Controls.PanelWyn();
        this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.popcolItem = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
        this.chkcolYn = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
        this.lookupcolReason = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.colSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colWhId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLotNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colBookQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMoveQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colFinQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRecntYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDiffQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colAdjReason = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colAddYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCntUser = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.btnAddRow1 = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnDeletRow1 = new WYNLAB.Base.Controls.ButtonWyn();
        this.chkUnentered = new WYNLAB.Base.Controls.CheckBoxWyn();
        this.btnSnap = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnDone = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnUndone = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnRecnt = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnConfirm = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnConfirmCancel = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnCancelDoc = new WYNLAB.Base.Controls.ButtonWyn();
        this.lblSummary = new DevExpress.XtraEditors.LabelControl();
        this.lblAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblStatCd = new DevExpress.XtraEditors.LabelControl();
        this.cboStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblDocNo = new DevExpress.XtraEditors.LabelControl();
        this.txtDocNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDocDate = new DevExpress.XtraEditors.LabelControl();
        this.dteDocDate = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblCntType = new DevExpress.XtraEditors.LabelControl();
        this.cboCntType = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblWh = new DevExpress.XtraEditors.LabelControl();
        this.txtWhNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.txtWhId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblCntTitle = new DevExpress.XtraEditors.LabelControl();
        this.txtCntTitle = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDept = new DevExpress.XtraEditors.LabelControl();
        this.txtDeptNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.txtDeptId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblEmp = new DevExpress.XtraEditors.LabelControl();
        this.txtEmpNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.txtEmpId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblTolQty = new DevExpress.XtraEditors.LabelControl();
        this.txtTolQty = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblTolRate = new DevExpress.XtraEditors.LabelControl();
        this.txtTolRate = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblCfmDt = new DevExpress.XtraEditors.LabelControl();
        this.txtCfmDt = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblCfmUserId = new DevExpress.XtraEditors.LabelControl();
        this.txtCfmUserId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblRemark = new DevExpress.XtraEditors.LabelControl();
        this.memoRemark = new DevExpress.XtraEditors.MemoEdit();
        this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblSearchNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchNo = new WYNLAB.Base.Controls.TextEditWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.featBar)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolItem)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkcolYn)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolReason)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDocNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteDocDate.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteDocDate.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboCntType.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkUnentered.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWhNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWhId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCntTitle.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtTolQty.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtTolRate.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCfmDt.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCfmUserId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchNo.Properties)).BeginInit();
        this.SuspendLayout();
        //
        // panBase
        //
        this.panBase.Controls.Add(this.panelWyn3);
        this.panBase.Controls.Add(this.panHeader);
        this.panBase.Appearance.BackColor = System.Drawing.Color.White;
        this.panBase.Appearance.Options.UseBackColor = true;
        this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panBase.Location = new System.Drawing.Point(0, 0);
        this.panBase.Name = "panBase";
        this.panBase.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
        this.panBase.Size = new System.Drawing.Size(1680, 800);
        this.panBase.TabIndex = 6;
        //
        // panelWyn3
        //
        this.panelWyn3.Controls.Add(this.panelWyn4);
        this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelWyn3.Location = new System.Drawing.Point(5, 49);
        this.panelWyn3.Name = "panelWyn3";
        this.panelWyn3.Size = new System.Drawing.Size(1670, 746);
        this.panelWyn3.TabIndex = 7;
        //
        // panelWyn4
        //
        this.panelWyn4.Controls.Add(this.grd1);
        this.panelWyn4.Controls.Add(this.panelWyn7);
        this.panelWyn4.Controls.Add(this.panelWyn1);
        this.panelWyn4.Controls.Add(this.panelWyn5);
        this.panelWyn4.Controls.Add(this.featBar);
        this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelWyn4.Location = new System.Drawing.Point(0, 0);
        this.panelWyn4.Name = "panelWyn4";
        this.panelWyn4.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.panelWyn4.Size = new System.Drawing.Size(1670, 746);
        this.panelWyn4.TabIndex = 7;
        //
        // featBar (공통 기능 버튼 패널 - 메뉴등록 '화면 기능'에서 켠 전자결재/첨부파일 버튼만 보이고, 켠 기능이 없으면 숨는다)
        //
        this.featBar.Dock = System.Windows.Forms.DockStyle.Top;
        this.featBar.Location = new System.Drawing.Point(3, 0);
        this.featBar.Name = "featBar";
        this.featBar.Size = new System.Drawing.Size(1664, 33);
        this.featBar.TabIndex = 10;
        //
        // popcolItem
        //
        this.popcolItem.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "...")});
        this.popcolItem.LookupKey = "P_ITEM";
        this.popcolItem.PopupConditions = "p_stock_yn=Y";
        this.popcolItem.Name = "popcolItem";
        //
        // chkcolYn
        //
        this.chkcolYn.AutoHeight = false;
        this.chkcolYn.Name = "chkcolYn";
        this.chkcolYn.ValueChecked = "Y";
        this.chkcolYn.ValueUnchecked = "N";
        //
        // lookupcolReason
        //
        this.lookupcolReason.AutoHeight = false;
        this.lookupcolReason.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolReason.LookupKey = "L_MA0016";
        this.lookupcolReason.Name = "lookupcolReason";
        this.lookupcolReason.NullText = "";
        this.lookupcolReason.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd1.EmbeddedNavigator.Buttons.Append.Visible = false;
        this.grd1.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
        this.grd1.EmbeddedNavigator.Buttons.Edit.Visible = false;
        this.grd1.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
        this.grd1.EmbeddedNavigator.Buttons.Remove.Visible = false;
        this.grd1.Location = new System.Drawing.Point(3, 360);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.popcolItem,
        this.chkcolYn,
        this.lookupcolReason});
        this.grd1.Size = new System.Drawing.Size(1664, 380);
        this.grd1.TabIndex = 0;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colSerl,
        this.colItemId,
        this.colItemNo,
        this.colItemNm,
        this.colItemSpec,
        this.colUnitCd,
        this.colWhId,
        this.colWhNm,
        this.colLotNo,
        this.colBookQty,
        this.colMoveQty,
        this.colFinQty,
        this.colRecntYn,
        this.colDiffQty,
        this.colAdjReason,
        this.colAddYn,
        this.colCntUser,
        this.colRemark});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        //
        // colSerl
        //
        this.colSerl.Caption = "순번";
        this.colSerl.FieldName = "serl";
        this.colSerl.Name = "colSerl";
        this.colSerl.OptionsColumn.AllowEdit = false;
        this.colSerl.Visible = true;
        this.colSerl.VisibleIndex = 0;
        this.colSerl.Width = 50;
        //
        // colItemId
        //
        this.colItemId.Caption = "품목";
        this.colItemId.FieldName = "item_id";
        this.colItemId.Name = "colItemId";
        this.colItemId.Width = 60;
        //
        // colItemNo
        //
        this.colItemNo.Caption = "품번";
        this.colItemNo.ColumnEdit = this.popcolItem;
        this.colItemNo.FieldName = "item_no";
        this.colItemNo.Name = "colItemNo";
        this.colItemNo.Visible = true;
        this.colItemNo.VisibleIndex = 1;
        this.colItemNo.Width = 110;
        //
        // colItemNm
        //
        this.colItemNm.Caption = "품명";
        this.colItemNm.FieldName = "item_nm";
        this.colItemNm.Name = "colItemNm";
        this.colItemNm.OptionsColumn.AllowEdit = false;
        this.colItemNm.Visible = true;
        this.colItemNm.VisibleIndex = 2;
        this.colItemNm.Width = 150;
        //
        // colItemSpec
        //
        this.colItemSpec.Caption = "규격";
        this.colItemSpec.FieldName = "item_spec";
        this.colItemSpec.Name = "colItemSpec";
        this.colItemSpec.OptionsColumn.AllowEdit = false;
        this.colItemSpec.Visible = true;
        this.colItemSpec.VisibleIndex = 3;
        this.colItemSpec.Width = 110;
        //
        // colUnitCd
        //
        this.colUnitCd.Caption = "단위";
        this.colUnitCd.FieldName = "unit_cd";
        this.colUnitCd.Name = "colUnitCd";
        this.colUnitCd.OptionsColumn.AllowEdit = false;
        this.colUnitCd.Visible = true;
        this.colUnitCd.VisibleIndex = 4;
        this.colUnitCd.Width = 50;
        //
        // colLotNo
        //
        this.colLotNo.Caption = "LOT";
        this.colLotNo.FieldName = "lot_no";
        this.colLotNo.Name = "colLotNo";
        this.colLotNo.Visible = true;
        this.colLotNo.VisibleIndex = 6;
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
        this.colBookQty.VisibleIndex = 7;
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
        this.colMoveQty.VisibleIndex = 8;
        this.colMoveQty.Width = 70;
        //
        // colFinQty
        //
        this.colFinQty.Caption = "실사수량";
        this.colFinQty.DisplayFormat.FormatString = "#,##0.####";
        this.colFinQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colFinQty.FieldName = "fin_qty";
        this.colFinQty.Name = "colFinQty";
        this.colFinQty.Visible = true;
        this.colFinQty.VisibleIndex = 9;
        this.colFinQty.Width = 90;
        //
        // colRecntYn
        //
        this.colRecntYn.Caption = "재실사";
        this.colRecntYn.ColumnEdit = this.chkcolYn;
        this.colRecntYn.FieldName = "recnt_yn";
        this.colRecntYn.Name = "colRecntYn";
        this.colRecntYn.OptionsColumn.AllowEdit = false;
        this.colRecntYn.Visible = true;
        this.colRecntYn.VisibleIndex = 10;
        this.colRecntYn.Width = 55;
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
        this.colDiffQty.VisibleIndex = 11;
        this.colDiffQty.Width = 80;
        //
        // colAdjReason
        //
        this.colAdjReason.Caption = "조정사유";
        this.colAdjReason.ColumnEdit = this.lookupcolReason;
        this.colAdjReason.FieldName = "adj_reason";
        this.colAdjReason.Name = "colAdjReason";
        this.colAdjReason.Visible = true;
        this.colAdjReason.VisibleIndex = 12;
        this.colAdjReason.Width = 90;
        //
        // colAddYn
        //
        this.colAddYn.Caption = "추가";
        this.colAddYn.ColumnEdit = this.chkcolYn;
        this.colAddYn.FieldName = "add_yn";
        this.colAddYn.Name = "colAddYn";
        this.colAddYn.OptionsColumn.AllowEdit = false;
        this.colAddYn.Visible = true;
        this.colAddYn.VisibleIndex = 13;
        this.colAddYn.Width = 45;
        //
        // colCntUser
        //
        this.colCntUser.Caption = "입력자";
        this.colCntUser.FieldName = "cnt_user_id";
        this.colCntUser.Name = "colCntUser";
        this.colCntUser.OptionsColumn.AllowEdit = false;
        this.colCntUser.Visible = true;
        this.colCntUser.VisibleIndex = 14;
        this.colCntUser.Width = 80;
        //
        // colRemark
        //
        this.colRemark.Caption = "비고";
        this.colRemark.FieldName = "remark";
        this.colRemark.Name = "colRemark";
        this.colRemark.Visible = true;
        this.colRemark.VisibleIndex = 15;
        this.colRemark.Width = 180;
        //
        // panelWyn1
        //
        this.panelWyn1.Controls.Add(this.sectionHeaderWyn2);
        this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn1.Location = new System.Drawing.Point(3, 330);
        this.panelWyn1.Name = "panelWyn1";
        this.panelWyn1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
        this.panelWyn1.Size = new System.Drawing.Size(1664, 27);
        this.panelWyn1.TabIndex = 8;
        //
        // sectionHeaderWyn2
        //
        this.sectionHeaderWyn2.BackColor = System.Drawing.Color.White;
        this.sectionHeaderWyn2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.sectionHeaderWyn2.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.sectionHeaderWyn2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
        this.sectionHeaderWyn2.Location = new System.Drawing.Point(5, 0);
        this.sectionHeaderWyn2.Name = "sectionHeaderWyn2";
        this.sectionHeaderWyn2.Size = new System.Drawing.Size(1659, 25);
        this.sectionHeaderWyn2.TabIndex = 9;
        this.sectionHeaderWyn2.Text = "재고실사 라인 입력 (선별된 라인은 삭제하지 않는 한 모두 실사수량을 입력해야 합니다. 0도 입력입니다)";
        //
        // panelWyn7
        //
        this.panelWyn7.Controls.Add(this.btnAddRow1);
        this.panelWyn7.Controls.Add(this.btnDeletRow1);
        this.panelWyn7.Controls.Add(this.chkUnentered);
        this.panelWyn7.Controls.Add(this.lblSummary);
        this.panelWyn7.Appearance.BackColor = System.Drawing.Color.White;
        this.panelWyn7.Appearance.Options.UseBackColor = true;
        this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn7.Location = new System.Drawing.Point(3, 360);
        this.panelWyn7.Name = "panelWyn7";
        this.panelWyn7.Padding = new System.Windows.Forms.Padding(5, 3, 0, 2);
        this.panelWyn7.Size = new System.Drawing.Size(1664, 30);
        this.panelWyn7.TabIndex = 9;
        //
        // btnAddRow1
        //
        this.btnAddRow1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
        this.btnAddRow1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(229)))), ((int)(((byte)(205)))));
        this.btnAddRow1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(239)))));
        this.btnAddRow1.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnAddRow1.Location = new System.Drawing.Point(6, 4);
        this.btnAddRow1.Name = "btnAddRow1";
        this.btnAddRow1.Size = new System.Drawing.Size(70, 24);
        this.btnAddRow1.TabIndex = 0;
        this.btnAddRow1.Text = "라인추가";
        this.btnAddRow1.ToolTip = "실사 중 발견한 계획 외 재고 라인을 추가합니다(실사중 상태에서만).";
        //
        // btnDeletRow1
        //
        this.btnDeletRow1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
        this.btnDeletRow1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(198)))), ((int)(((byte)(193)))));
        this.btnDeletRow1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(236)))), ((int)(((byte)(234)))));
        this.btnDeletRow1.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnDeletRow1.Location = new System.Drawing.Point(78, 4);
        this.btnDeletRow1.Name = "btnDeletRow1";
        this.btnDeletRow1.Size = new System.Drawing.Size(60, 24);
        this.btnDeletRow1.TabIndex = 1;
        this.btnDeletRow1.Text = "행삭제";
        this.btnDeletRow1.ToolTip = "이번 실사에서 뺄 라인을 삭제합니다(실사중 상태에서만).";
        //
        // chkUnentered
        //
        this.chkUnentered.Location = new System.Drawing.Point(146, 5);
        this.chkUnentered.Name = "chkUnentered";
        this.chkUnentered.Properties.Caption = "실사수량 미등록건만 보기";
        this.chkUnentered.Size = new System.Drawing.Size(170, 22);
        this.chkUnentered.TabIndex = 2;
        this.chkUnentered.ToolTip = "실사수량을 아직 입력하지 않은 라인만 보여줍니다(해제하면 전체).";
        //
        // lblSummary
        //
        this.lblSummary.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSummary.Appearance.Options.UseFont = true;
        this.lblSummary.Location = new System.Drawing.Point(330, 8);
        this.lblSummary.Name = "lblSummary";
        this.lblSummary.Size = new System.Drawing.Size(600, 15);
        this.lblSummary.TabIndex = 3;
        this.lblSummary.Text = "";
        //
        // panelWyn5
        //
        this.panelWyn5.Controls.Add(this.panData);
        this.panelWyn5.Controls.Add(this.panelWyn6);
        this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn5.Location = new System.Drawing.Point(3, 0);
        this.panelWyn5.Name = "panelWyn5";
        this.panelWyn5.Size = new System.Drawing.Size(1664, 307);
        this.panelWyn5.TabIndex = 6;
        //
        // panelWyn6
        //
        this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
        this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn6.Location = new System.Drawing.Point(0, 0);
        this.panelWyn6.Name = "panelWyn6";
        this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
        this.panelWyn6.Size = new System.Drawing.Size(1664, 27);
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
        this.sectionHeaderWyn3.Size = new System.Drawing.Size(1659, 25);
        this.sectionHeaderWyn3.TabIndex = 9;
        this.sectionHeaderWyn3.Text = "재고실사 정보등록";
        //
        // panData
        //
        this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        this.panData.Controls.Add(this.lblAccId);
        this.panData.Controls.Add(this.cboAccId);
        this.panData.Controls.Add(this.lblStatCd);
        this.panData.Controls.Add(this.cboStatCd);
        this.panData.Controls.Add(this.lblDocNo);
        this.panData.Controls.Add(this.txtDocNo);
        this.panData.Controls.Add(this.lblDocDate);
        this.panData.Controls.Add(this.dteDocDate);
        this.panData.Controls.Add(this.lblCntType);
        this.panData.Controls.Add(this.cboCntType);
        this.panData.Controls.Add(this.lblWh);
        this.panData.Controls.Add(this.txtWhNm);
        this.panData.Controls.Add(this.txtWhId);
        this.panData.Controls.Add(this.lblCntTitle);
        this.panData.Controls.Add(this.txtCntTitle);
        this.panData.Controls.Add(this.lblDept);
        this.panData.Controls.Add(this.txtDeptNm);
        this.panData.Controls.Add(this.txtDeptId);
        this.panData.Controls.Add(this.lblEmp);
        this.panData.Controls.Add(this.txtEmpNm);
        this.panData.Controls.Add(this.txtEmpId);
        this.panData.Controls.Add(this.lblTolQty);
        this.panData.Controls.Add(this.txtTolQty);
        this.panData.Controls.Add(this.lblTolRate);
        this.panData.Controls.Add(this.txtTolRate);
        this.panData.Controls.Add(this.lblCfmDt);
        this.panData.Controls.Add(this.txtCfmDt);
        this.panData.Controls.Add(this.lblCfmUserId);
        this.panData.Controls.Add(this.txtCfmUserId);
        this.panData.Controls.Add(this.lblRemark);
        this.panData.Controls.Add(this.memoRemark);
        this.panData.Controls.Add(this.btnSnap);
        this.panData.Controls.Add(this.btnDone);
        this.panData.Controls.Add(this.btnUndone);
        this.panData.Controls.Add(this.btnRecnt);
        this.panData.Controls.Add(this.btnConfirm);
        this.panData.Controls.Add(this.btnConfirmCancel);
        this.panData.Controls.Add(this.btnCancelDoc);
        this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panData.Location = new System.Drawing.Point(0, 27);
        this.panData.Name = "panData";
        this.panData.Size = new System.Drawing.Size(1664, 280);
        this.panData.TabIndex = 8;
        //
        // lblAccId
        //
        this.lblAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblAccId.Appearance.Options.UseFont = true;
        this.lblAccId.Location = new System.Drawing.Point(28, 15);
        this.lblAccId.Name = "lblAccId";
        this.lblAccId.Size = new System.Drawing.Size(36, 15);
        this.lblAccId.TabIndex = 2;
        this.lblAccId.Text = "사업장";
        //
        // cboAccId
        //
        this.cboAccId.Location = new System.Drawing.Point(100, 12);
        this.cboAccId.LookupKey = "L_ACC";
        this.cboAccId.Required = true;
        this.cboAccId.Name = "cboAccId";
        this.cboAccId.Size = new System.Drawing.Size(150, 20);
        this.cboAccId.TabIndex = 3;
        //
        // lblStatCd
        //
        this.lblStatCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblStatCd.Appearance.Options.UseFont = true;
        this.lblStatCd.Location = new System.Drawing.Point(270, 15);
        this.lblStatCd.Name = "lblStatCd";
        this.lblStatCd.Size = new System.Drawing.Size(48, 15);
        this.lblStatCd.TabIndex = 4;
        this.lblStatCd.Text = "진행상태";
        //
        // cboStatCd
        //
        this.cboStatCd.Location = new System.Drawing.Point(350, 12);
        this.cboStatCd.LookupKey = "L_MA0014";
        this.cboStatCd.Properties.ReadOnly = true;
        this.cboStatCd.Name = "cboStatCd";
        this.cboStatCd.Size = new System.Drawing.Size(150, 20);
        this.cboStatCd.TabIndex = 5;
        //
        // lblDocNo
        //
        this.lblDocNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDocNo.Appearance.Options.UseFont = true;
        this.lblDocNo.Location = new System.Drawing.Point(16, 43);
        this.lblDocNo.Name = "lblDocNo";
        this.lblDocNo.Size = new System.Drawing.Size(48, 15);
        this.lblDocNo.TabIndex = 6;
        this.lblDocNo.Text = "실사번호";
        //
        // txtDocNo
        //
        this.txtDocNo.Location = new System.Drawing.Point(100, 40);
        this.txtDocNo.Properties.ReadOnly = true;
        this.txtDocNo.Name = "txtDocNo";
        this.txtDocNo.Size = new System.Drawing.Size(150, 20);
        this.txtDocNo.TabIndex = 7;
        //
        // lblDocDate
        //
        this.lblDocDate.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDocDate.Appearance.Options.UseFont = true;
        this.lblDocDate.Location = new System.Drawing.Point(270, 43);
        this.lblDocDate.Name = "lblDocDate";
        this.lblDocDate.Size = new System.Drawing.Size(48, 15);
        this.lblDocDate.TabIndex = 8;
        this.lblDocDate.Text = "기준일자";
        //
        // dteDocDate
        //
        this.dteDocDate.Location = new System.Drawing.Point(350, 40);
        this.dteDocDate.Required = true;
        this.dteDocDate.Name = "dteDocDate";
        this.dteDocDate.Size = new System.Drawing.Size(150, 20);
        this.dteDocDate.TabIndex = 9;
        //
        // lblCntType
        //
        this.lblCntType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblCntType.Appearance.Options.UseFont = true;
        this.lblCntType.Location = new System.Drawing.Point(16, 71);
        this.lblCntType.Name = "lblCntType";
        this.lblCntType.Size = new System.Drawing.Size(48, 15);
        this.lblCntType.TabIndex = 10;
        this.lblCntType.Text = "실사유형";
        //
        // cboCntType
        //
        this.cboCntType.Location = new System.Drawing.Point(100, 68);
        this.cboCntType.LookupKey = "L_MA0015";
        this.cboCntType.Required = true;
        this.cboCntType.Name = "cboCntType";
        this.cboCntType.Size = new System.Drawing.Size(150, 20);
        this.cboCntType.TabIndex = 11;
        //
        // lblWh
        //
        this.lblWh.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblWh.Appearance.Options.UseFont = true;
        this.lblWh.Location = new System.Drawing.Point(40, 99);
        this.lblWh.Name = "lblWh";
        this.lblWh.Size = new System.Drawing.Size(24, 15);
        this.lblWh.TabIndex = 14;
        this.lblWh.Text = "창고";
        //
        // txtWhNm
        //
        this.txtWhNm.Location = new System.Drawing.Point(100, 96);
        this.txtWhNm.LookupKey = "P_WH";
        this.txtWhNm.MatchField = "wh_nm";
        this.txtWhNm.Required = true;
        this.txtWhNm.Name = "txtWhNm";
        this.txtWhNm.Size = new System.Drawing.Size(150, 20);
        this.txtWhNm.TabIndex = 15;
        //
        // txtWhId
        //
        this.txtWhId.Location = new System.Drawing.Point(100, 96);
        this.txtWhId.Name = "txtWhId";
        this.txtWhId.Size = new System.Drawing.Size(150, 20);
        this.txtWhId.Visible = false;
        //
        // lblCntTitle
        //
        this.lblCntTitle.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblCntTitle.Appearance.Options.UseFont = true;
        this.lblCntTitle.Location = new System.Drawing.Point(282, 99);
        this.lblCntTitle.Name = "lblCntTitle";
        this.lblCntTitle.Size = new System.Drawing.Size(36, 15);
        this.lblCntTitle.TabIndex = 16;
        this.lblCntTitle.Text = "실사명";
        //
        // txtCntTitle
        //
        this.txtCntTitle.Location = new System.Drawing.Point(350, 96);
        this.txtCntTitle.Name = "txtCntTitle";
        this.txtCntTitle.Size = new System.Drawing.Size(300, 20);
        this.txtCntTitle.TabIndex = 17;
        //
        // lblDept
        //
        this.lblDept.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDept.Appearance.Options.UseFont = true;
        this.lblDept.Location = new System.Drawing.Point(40, 127);
        this.lblDept.Name = "lblDept";
        this.lblDept.Size = new System.Drawing.Size(24, 15);
        this.lblDept.TabIndex = 16;
        this.lblDept.Text = "부서";
        //
        // txtDeptNm
        //
        this.txtDeptNm.Location = new System.Drawing.Point(100, 124);
        this.txtDeptNm.LookupKey = "P_DEPT";
        this.txtDeptNm.MatchField = "dept_nm";
        this.txtDeptNm.Name = "txtDeptNm";
        this.txtDeptNm.Size = new System.Drawing.Size(150, 20);
        this.txtDeptNm.TabIndex = 17;
        //
        // txtDeptId
        //
        this.txtDeptId.Location = new System.Drawing.Point(100, 124);
        this.txtDeptId.Name = "txtDeptId";
        this.txtDeptId.Size = new System.Drawing.Size(150, 20);
        this.txtDeptId.Visible = false;
        //
        // lblEmp
        //
        this.lblEmp.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblEmp.Appearance.Options.UseFont = true;
        this.lblEmp.Location = new System.Drawing.Point(282, 127);
        this.lblEmp.Name = "lblEmp";
        this.lblEmp.Size = new System.Drawing.Size(36, 15);
        this.lblEmp.TabIndex = 18;
        this.lblEmp.Text = "담당자";
        //
        // txtEmpNm
        //
        this.txtEmpNm.Location = new System.Drawing.Point(350, 124);
        this.txtEmpNm.LookupKey = "P_EMP";
        this.txtEmpNm.MatchField = "emp_nm";
        this.txtEmpNm.Name = "txtEmpNm";
        this.txtEmpNm.Size = new System.Drawing.Size(150, 20);
        this.txtEmpNm.TabIndex = 19;
        //
        // txtEmpId
        //
        this.txtEmpId.Location = new System.Drawing.Point(350, 124);
        this.txtEmpId.Name = "txtEmpId";
        this.txtEmpId.Size = new System.Drawing.Size(150, 20);
        this.txtEmpId.Visible = false;
        //
        // lblTolQty
        //
        this.lblTolQty.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblTolQty.Appearance.Options.UseFont = true;
        this.lblTolQty.Location = new System.Drawing.Point(16, 155);
        this.lblTolQty.Name = "lblTolQty";
        this.lblTolQty.Size = new System.Drawing.Size(78, 15);
        this.lblTolQty.TabIndex = 20;
        this.lblTolQty.Text = "허용오차(수량)";
        //
        // txtTolQty
        //
        this.txtTolQty.Location = new System.Drawing.Point(100, 152);
        this.txtTolQty.Name = "txtTolQty";
        this.txtTolQty.Size = new System.Drawing.Size(150, 20);
        this.txtTolQty.TabIndex = 21;
        //
        // lblTolRate
        //
        this.lblTolRate.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblTolRate.Appearance.Options.UseFont = true;
        this.lblTolRate.Location = new System.Drawing.Point(262, 155);
        this.lblTolRate.Name = "lblTolRate";
        this.lblTolRate.Size = new System.Drawing.Size(66, 15);
        this.lblTolRate.TabIndex = 22;
        this.lblTolRate.Text = "허용오차(%)";
        //
        // txtTolRate
        //
        this.txtTolRate.Location = new System.Drawing.Point(350, 152);
        this.txtTolRate.Name = "txtTolRate";
        this.txtTolRate.Size = new System.Drawing.Size(150, 20);
        this.txtTolRate.TabIndex = 23;
        //
        // lblCfmDt
        //
        this.lblCfmDt.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblCfmDt.Appearance.Options.UseFont = true;
        this.lblCfmDt.Location = new System.Drawing.Point(16, 183);
        this.lblCfmDt.Name = "lblCfmDt";
        this.lblCfmDt.Size = new System.Drawing.Size(48, 15);
        this.lblCfmDt.TabIndex = 24;
        this.lblCfmDt.Text = "확정일시";
        //
        // txtCfmDt
        //
        this.txtCfmDt.Location = new System.Drawing.Point(100, 180);
        this.txtCfmDt.Properties.ReadOnly = true;
        this.txtCfmDt.Name = "txtCfmDt";
        this.txtCfmDt.Size = new System.Drawing.Size(150, 20);
        this.txtCfmDt.TabIndex = 25;
        //
        // lblCfmUserId
        //
        this.lblCfmUserId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblCfmUserId.Appearance.Options.UseFont = true;
        this.lblCfmUserId.Location = new System.Drawing.Point(282, 183);
        this.lblCfmUserId.Name = "lblCfmUserId";
        this.lblCfmUserId.Size = new System.Drawing.Size(36, 15);
        this.lblCfmUserId.TabIndex = 26;
        this.lblCfmUserId.Text = "확정자";
        //
        // txtCfmUserId
        //
        this.txtCfmUserId.Location = new System.Drawing.Point(350, 180);
        this.txtCfmUserId.Properties.ReadOnly = true;
        this.txtCfmUserId.Name = "txtCfmUserId";
        this.txtCfmUserId.Size = new System.Drawing.Size(150, 20);
        this.txtCfmUserId.TabIndex = 27;
        //
        // lblRemark
        //
        this.lblRemark.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblRemark.Appearance.Options.UseFont = true;
        this.lblRemark.Location = new System.Drawing.Point(40, 211);
        this.lblRemark.Name = "lblRemark";
        this.lblRemark.Size = new System.Drawing.Size(24, 15);
        this.lblRemark.TabIndex = 28;
        this.lblRemark.Text = "비고";
        //
        // memoRemark
        //
        this.memoRemark.Location = new System.Drawing.Point(100, 208);
        this.memoRemark.Name = "memoRemark";
        this.memoRemark.Size = new System.Drawing.Size(400, 25);
        this.memoRemark.TabIndex = 29;
        //
        // btnSnap
        //
        this.btnSnap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
        this.btnSnap.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnSnap.Location = new System.Drawing.Point(16, 246);
        this.btnSnap.Name = "btnSnap";
        this.btnSnap.Size = new System.Drawing.Size(100, 24);
        this.btnSnap.TabIndex = 34;
        this.btnSnap.Text = "대상선별";
        this.btnSnap.ToolTip = "지금 재고를 스냅샷으로 떠서 실사 대상 라인을 선별하고 바로 수량 입력(실사중)을 시작합니다.";
        //
        // btnDone
        //
        this.btnDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
        this.btnDone.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnDone.Location = new System.Drawing.Point(122, 246);
        this.btnDone.Name = "btnDone";
        this.btnDone.Size = new System.Drawing.Size(100, 24);
        this.btnDone.TabIndex = 36;
        this.btnDone.Text = "입력완료";
        this.btnDone.ToolTip = "모든 라인의 실사수량이 입력돼야 합니다. 차이를 계산·고정합니다.";
        //
        // btnUndone
        //
        this.btnUndone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
        this.btnUndone.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnUndone.Location = new System.Drawing.Point(228, 246);
        this.btnUndone.Name = "btnUndone";
        this.btnUndone.Size = new System.Drawing.Size(100, 24);
        this.btnUndone.TabIndex = 37;
        this.btnUndone.Text = "입력완료취소";
        this.btnUndone.ToolTip = "입력완료를 취소하고 실사중으로 되돌립니다.";
        //
        // btnRecnt
        //
        this.btnRecnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
        this.btnRecnt.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnRecnt.Location = new System.Drawing.Point(334, 246);
        this.btnRecnt.Name = "btnRecnt";
        this.btnRecnt.Size = new System.Drawing.Size(100, 24);
        this.btnRecnt.TabIndex = 38;
        this.btnRecnt.Text = "재실사지정";
        this.btnRecnt.ToolTip = "체크한 라인을 다시 실사하도록 지정합니다(실사수량을 다시 입력).";
        //
        // btnConfirm
        //
        this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
        this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnConfirm.Location = new System.Drawing.Point(440, 246);
        this.btnConfirm.Name = "btnConfirm";
        this.btnConfirm.Size = new System.Drawing.Size(100, 24);
        this.btnConfirm.TabIndex = 39;
        this.btnConfirm.Text = "확정";
        this.btnConfirm.ToolTip = "차이가 있는 라인의 조정 수불을 만들고 현재고를 맞춥니다.";
        //
        // btnConfirmCancel
        //
        this.btnConfirmCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
        this.btnConfirmCancel.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnConfirmCancel.Location = new System.Drawing.Point(546, 246);
        this.btnConfirmCancel.Name = "btnConfirmCancel";
        this.btnConfirmCancel.Size = new System.Drawing.Size(100, 24);
        this.btnConfirmCancel.TabIndex = 40;
        this.btnConfirmCancel.Text = "확정취소";
        this.btnConfirmCancel.ToolTip = "조정 수불을 역거래로 되돌립니다.";
        //
        // btnCancelDoc
        //
        this.btnCancelDoc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
        this.btnCancelDoc.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnCancelDoc.Location = new System.Drawing.Point(652, 246);
        this.btnCancelDoc.Name = "btnCancelDoc";
        this.btnCancelDoc.Size = new System.Drawing.Size(100, 24);
        this.btnCancelDoc.TabIndex = 41;
        this.btnCancelDoc.Text = "실사취소";
        this.btnCancelDoc.ToolTip = "확정 전 실사를 취소합니다(창고 진행 제한이 풀립니다).";
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblSearchAccId);
        this.panHeader.Controls.Add(this.cboSearchAccId);
        this.panHeader.Controls.Add(this.lblSearchNo);
        this.panHeader.Controls.Add(this.txtSearchNo);
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
        this.lblSearchAccId.Location = new System.Drawing.Point(25, 18);
        this.lblSearchAccId.Name = "lblSearchAccId";
        this.lblSearchAccId.Size = new System.Drawing.Size(36, 15);
        this.lblSearchAccId.TabIndex = 0;
        this.lblSearchAccId.Text = "사업장";
        //
        // cboSearchAccId
        //
        this.cboSearchAccId.Location = new System.Drawing.Point(69, 15);
        this.cboSearchAccId.LookupKey = "L_ACC";
        this.cboSearchAccId.Required = true;
        this.cboSearchAccId.Name = "cboSearchAccId";
        this.cboSearchAccId.Size = new System.Drawing.Size(131, 20);
        this.cboSearchAccId.TabIndex = 1;
        //
        // lblSearchNo
        //
        this.lblSearchNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchNo.Appearance.Options.UseFont = true;
        this.lblSearchNo.Location = new System.Drawing.Point(224, 18);
        this.lblSearchNo.Name = "lblSearchNo";
        this.lblSearchNo.Size = new System.Drawing.Size(48, 15);
        this.lblSearchNo.TabIndex = 31;
        this.lblSearchNo.Text = "실사번호";
        //
        // txtSearchNo
        //
        this.txtSearchNo.Location = new System.Drawing.Point(280, 15);
        this.txtSearchNo.Name = "txtSearchNo";
        this.txtSearchNo.Size = new System.Drawing.Size(180, 20);
        this.txtSearchNo.TabIndex = 32;
        //
        // frmStockCount
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 800);
        this.Controls.Add(this.panBase);
        this.Name = "frmStockCount";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.featBar)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolItem)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkcolYn)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolReason)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDocNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteDocDate.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteDocDate.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboCntType.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkUnentered.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWhNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWhId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCntTitle.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtTolQty.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtTolRate.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCfmDt.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCfmUserId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchNo.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn4;
    private PanelWyn panelWyn1;
    private PanelWyn panelWyn7;
    private PanelWyn panelWyn5;
    private WYNLAB.Popup.FeatureBarWyn featBar;
    private PanelWyn panData;
    private PanelWyn panelWyn6;
    private PanelWyn panHeader;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private PopupLookupColumnEdit popcolItem;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkcolYn;
    private LookUpColumnEdit lookupcolReason;
    private DevExpress.XtraGrid.Columns.GridColumn colSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colItemId;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colWhId;
    private DevExpress.XtraGrid.Columns.GridColumn colWhNm;
    private DevExpress.XtraGrid.Columns.GridColumn colLotNo;
    private DevExpress.XtraGrid.Columns.GridColumn colBookQty;
    private DevExpress.XtraGrid.Columns.GridColumn colMoveQty;
    private DevExpress.XtraGrid.Columns.GridColumn colFinQty;
    private DevExpress.XtraGrid.Columns.GridColumn colRecntYn;
    private DevExpress.XtraGrid.Columns.GridColumn colDiffQty;
    private DevExpress.XtraGrid.Columns.GridColumn colAdjReason;
    private DevExpress.XtraGrid.Columns.GridColumn colAddYn;
    private DevExpress.XtraGrid.Columns.GridColumn colCntUser;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private SectionHeaderWyn sectionHeaderWyn2;
    private SectionHeaderWyn sectionHeaderWyn3;
    private ButtonWyn btnAddRow1;
    private ButtonWyn btnDeletRow1;
    private CheckBoxWyn chkUnentered;
    private ButtonWyn btnSnap;
    private ButtonWyn btnDone;
    private ButtonWyn btnUndone;
    private ButtonWyn btnRecnt;
    private ButtonWyn btnConfirm;
    private ButtonWyn btnConfirmCancel;
    private ButtonWyn btnCancelDoc;
    private DevExpress.XtraEditors.LabelControl lblSummary;
    private DevExpress.XtraEditors.LabelControl lblAccId;
    private LookUpEditWyn cboAccId;
    private DevExpress.XtraEditors.LabelControl lblStatCd;
    private LookUpEditWyn cboStatCd;
    private DevExpress.XtraEditors.LabelControl lblDocNo;
    private TextEditWyn txtDocNo;
    private DevExpress.XtraEditors.LabelControl lblDocDate;
    private DateEditWyn dteDocDate;
    private DevExpress.XtraEditors.LabelControl lblCntType;
    private LookUpEditWyn cboCntType;
    private DevExpress.XtraEditors.LabelControl lblWh;
    private PopupLookupEditWyn txtWhNm;
    private TextEditWyn txtWhId;
    private DevExpress.XtraEditors.LabelControl lblCntTitle;
    private TextEditWyn txtCntTitle;
    private DevExpress.XtraEditors.LabelControl lblDept;
    private PopupLookupEditWyn txtDeptNm;
    private TextEditWyn txtDeptId;
    private DevExpress.XtraEditors.LabelControl lblEmp;
    private PopupLookupEditWyn txtEmpNm;
    private TextEditWyn txtEmpId;
    private DevExpress.XtraEditors.LabelControl lblTolQty;
    private TextEditWyn txtTolQty;
    private DevExpress.XtraEditors.LabelControl lblTolRate;
    private TextEditWyn txtTolRate;
    private DevExpress.XtraEditors.LabelControl lblCfmDt;
    private TextEditWyn txtCfmDt;
    private DevExpress.XtraEditors.LabelControl lblCfmUserId;
    private TextEditWyn txtCfmUserId;
    private DevExpress.XtraEditors.LabelControl lblRemark;
    private DevExpress.XtraEditors.MemoEdit memoRemark;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private LookUpEditWyn cboSearchAccId;
    private DevExpress.XtraEditors.LabelControl lblSearchNo;
    private TextEditWyn txtSearchNo;
}
