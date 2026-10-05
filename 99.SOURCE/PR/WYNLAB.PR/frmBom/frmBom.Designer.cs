// BOM관리(frmBom) - 라우팅관리와 같은 구조: 좌측 BOM 목록(grd1) + 스플리터 + 우측(헤더 panData + 구성품 그리드 grd2). TPRBOMM/TPRBOMD.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

public partial class frmBom
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
        this.panWork = new WYNLAB.Base.Controls.PanelWyn();
        this.panLeft = new WYNLAB.Base.Controls.PanelWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.chkcolListUse = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
        this.colListNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colListNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colListCnt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colListUse = new DevExpress.XtraGrid.Columns.GridColumn();
        this.shList = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
        this.panRight = new WYNLAB.Base.Controls.PanelWyn();
        this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
        this.lookupcolType = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.lookupcolUnit = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.lookupcolProc = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.popcolItem = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
        this.colSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCompType = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCompItem = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colQtyPer = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colUnit = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLossRate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colProcCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.panTool = new WYNLAB.Base.Controls.PanelWyn();
        this.btnAddRow1 = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnDeletRow1 = new WYNLAB.Base.Controls.ButtonWyn();
        this.shLine = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panData = new WYNLAB.Base.Controls.PanelWyn();
        this.lblAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblItemNm = new DevExpress.XtraEditors.LabelControl();
        this.txtItemNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.txtItemId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblUseYn = new DevExpress.XtraEditors.LabelControl();
        this.chkUseYn = new DevExpress.XtraEditors.CheckEdit();
        this.lblUsedNote = new DevExpress.XtraEditors.LabelControl();
        this.lblRemark = new DevExpress.XtraEditors.LabelControl();
        this.memoRemark = new DevExpress.XtraEditors.MemoEdit();
        this.lblHint = new DevExpress.XtraEditors.LabelControl();
        this.shHeader = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblSearchKeyword = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchKeyword = new WYNLAB.Base.Controls.TextEditWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        this.panBase.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).BeginInit();
        this.panWork.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panLeft)).BeginInit();
        this.panLeft.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkcolListUse)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panRight)).BeginInit();
        this.panRight.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolType)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolUnit)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolProc)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolItem)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panTool)).BeginInit();
        this.panTool.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
        this.panData.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtItemNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtItemId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkUseYn.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
        this.SuspendLayout();
        //
        // panBase
        //
        this.panBase.Appearance.BackColor = System.Drawing.Color.White;
        this.panBase.Appearance.Options.UseBackColor = true;
        this.panBase.Controls.Add(this.panWork);
        this.panBase.Controls.Add(this.panHeader);
        this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panBase.Location = new System.Drawing.Point(0, 0);
        this.panBase.Name = "panBase";
        this.panBase.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
        this.panBase.Size = new System.Drawing.Size(1680, 800);
        this.panBase.TabIndex = 0;
        //
        // panWork
        //
        this.panWork.Controls.Add(this.panRight);
        this.panWork.Controls.Add(this.splitterWyn1);
        this.panWork.Controls.Add(this.panLeft);
        this.panWork.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panWork.Location = new System.Drawing.Point(5, 49);
        this.panWork.Name = "panWork";
        this.panWork.Size = new System.Drawing.Size(1670, 746);
        this.panWork.TabIndex = 1;
        //
        // panLeft
        //
        this.panLeft.Controls.Add(this.grd1);
        this.panLeft.Controls.Add(this.shList);
        this.panLeft.Dock = System.Windows.Forms.DockStyle.Left;
        this.panLeft.Location = new System.Drawing.Point(0, 0);
        this.panLeft.Name = "panLeft";
        this.panLeft.Size = new System.Drawing.Size(402, 746);
        this.panLeft.TabIndex = 0;
        //
        // shList
        //
        this.shList.BackColor = System.Drawing.Color.White;
        this.shList.Dock = System.Windows.Forms.DockStyle.Top;
        this.shList.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shList.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
        this.shList.Location = new System.Drawing.Point(0, 0);
        this.shList.Name = "shList";
        this.shList.Size = new System.Drawing.Size(402, 27);
        this.shList.TabIndex = 0;
        this.shList.Text = "BOM LIST";
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd1.Location = new System.Drawing.Point(0, 27);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.chkcolListUse});
        this.grd1.Size = new System.Drawing.Size(402, 719);
        this.grd1.TabIndex = 1;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colListNo,
        this.colListNm,
        this.colListCnt,
        this.colListUse});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsBehavior.Editable = false;
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        //
        // chkcolListUse
        //
        this.chkcolListUse.AutoHeight = false;
        this.chkcolListUse.Name = "chkcolListUse";
        this.chkcolListUse.ValueChecked = "Y";
        this.chkcolListUse.ValueUnchecked = "N";
        //
        // colListNo
        //
        this.colListNo.Caption = "품번";
        this.colListNo.FieldName = "item_no";
        this.colListNo.Name = "colListNo";
        this.colListNo.Visible = true;
        this.colListNo.VisibleIndex = 0;
        this.colListNo.Width = 100;
        //
        // colListNm
        //
        this.colListNm.Caption = "품명";
        this.colListNm.FieldName = "item_nm";
        this.colListNm.Name = "colListNm";
        this.colListNm.Visible = true;
        this.colListNm.VisibleIndex = 1;
        this.colListNm.Width = 150;
        //
        // colListCnt
        //
        this.colListCnt.Caption = "구성품수";
        this.colListCnt.AppearanceCell.Options.UseTextOptions = true;
        this.colListCnt.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        this.colListCnt.FieldName = "comp_cnt";
        this.colListCnt.Name = "colListCnt";
        this.colListCnt.Visible = true;
        this.colListCnt.VisibleIndex = 2;
        this.colListCnt.Width = 70;
        //
        // colListUse
        //
        this.colListUse.Caption = "사용";
        this.colListUse.ColumnEdit = this.chkcolListUse;
        this.colListUse.FieldName = "use_yn";
        this.colListUse.Name = "colListUse";
        this.colListUse.Visible = true;
        this.colListUse.VisibleIndex = 3;
        this.colListUse.Width = 45;
        //
        // splitterWyn1
        //
        this.splitterWyn1.BackColor = System.Drawing.Color.White;
        this.splitterWyn1.Location = new System.Drawing.Point(402, 0);
        this.splitterWyn1.Name = "splitterWyn1";
        this.splitterWyn1.Size = new System.Drawing.Size(10, 746);
        this.splitterWyn1.TabIndex = 1;
        this.splitterWyn1.TabStop = false;
        //
        // panRight
        //
        this.panRight.Controls.Add(this.grd2);
        this.panRight.Controls.Add(this.panTool);
        this.panRight.Controls.Add(this.shLine);
        this.panRight.Controls.Add(this.panData);
        this.panRight.Controls.Add(this.shHeader);
        this.panRight.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panRight.Location = new System.Drawing.Point(412, 0);
        this.panRight.Name = "panRight";
        this.panRight.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
        this.panRight.Size = new System.Drawing.Size(1258, 746);
        this.panRight.TabIndex = 2;
        //
        // shHeader
        //
        this.shHeader.BackColor = System.Drawing.Color.White;
        this.shHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.shHeader.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shHeader.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
        this.shHeader.Location = new System.Drawing.Point(3, 0);
        this.shHeader.Name = "shHeader";
        this.shHeader.Size = new System.Drawing.Size(1664, 27);
        this.shHeader.TabIndex = 0;
        this.shHeader.Text = "BOM 정보등록";
        //
        // panData
        //
        this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        this.panData.Controls.Add(this.lblAccId);
        this.panData.Controls.Add(this.cboAccId);
        this.panData.Controls.Add(this.lblItemNm);
        this.panData.Controls.Add(this.txtItemNm);
        this.panData.Controls.Add(this.txtItemId);
        this.panData.Controls.Add(this.lblUseYn);
        this.panData.Controls.Add(this.chkUseYn);
        this.panData.Controls.Add(this.lblUsedNote);
        this.panData.Controls.Add(this.lblRemark);
        this.panData.Controls.Add(this.memoRemark);
        this.panData.Controls.Add(this.lblHint);
        this.panData.Dock = System.Windows.Forms.DockStyle.Top;
        this.panData.Location = new System.Drawing.Point(3, 27);
        this.panData.Name = "panData";
        this.panData.Size = new System.Drawing.Size(1664, 110);
        this.panData.TabIndex = 1;
        //
        // lblAccId
        //
        this.lblAccId.Location = new System.Drawing.Point(28, 15);
        this.lblAccId.Name = "lblAccId";
        this.lblAccId.Size = new System.Drawing.Size(36, 15);
        this.lblAccId.Text = "사업장";
        //
        // cboAccId
        //
        this.cboAccId.Location = new System.Drawing.Point(100, 12);
        this.cboAccId.LookupKey = "L_ACC";
        this.cboAccId.Name = "cboAccId";
        this.cboAccId.Properties.NullText = "";
        this.cboAccId.Required = true;
        this.cboAccId.Size = new System.Drawing.Size(150, 20);
        this.cboAccId.TabIndex = 1;
        //
        // lblItemNm
        //
        this.lblItemNm.Location = new System.Drawing.Point(282, 15);
        this.lblItemNm.Name = "lblItemNm";
        this.lblItemNm.Size = new System.Drawing.Size(48, 15);
        this.lblItemNm.Text = "상위품목";
        //
        // txtItemNm
        //
        this.txtItemNm.Location = new System.Drawing.Point(354, 12);
        this.txtItemNm.LookupKey = "P_ITEM";
        this.txtItemNm.MatchField = "item_nm";
        this.txtItemNm.Name = "txtItemNm";
        this.txtItemNm.Required = true;
        this.txtItemNm.Size = new System.Drawing.Size(250, 20);
        this.txtItemNm.TabIndex = 3;
        //
        // txtItemId
        //
        this.txtItemId.Location = new System.Drawing.Point(1200, 12);
        this.txtItemId.Name = "txtItemId";
        this.txtItemId.Size = new System.Drawing.Size(60, 20);
        this.txtItemId.TabIndex = 4;
        this.txtItemId.Visible = false;
        //
        // lblUseYn
        //
        this.lblUseYn.Location = new System.Drawing.Point(640, 15);
        this.lblUseYn.Name = "lblUseYn";
        this.lblUseYn.Size = new System.Drawing.Size(48, 15);
        this.lblUseYn.Text = "사용여부";
        //
        // chkUseYn
        //
        this.chkUseYn.Location = new System.Drawing.Point(710, 12);
        this.chkUseYn.Name = "chkUseYn";
        this.chkUseYn.Properties.Caption = "사용";
        this.chkUseYn.Size = new System.Drawing.Size(80, 20);
        this.chkUseYn.TabIndex = 6;
        //
        // lblUsedNote
        //
        this.lblUsedNote.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(60)))), ((int)(((byte)(50)))));
        this.lblUsedNote.Appearance.Options.UseForeColor = true;
        this.lblUsedNote.Location = new System.Drawing.Point(820, 15);
        this.lblUsedNote.Name = "lblUsedNote";
        this.lblUsedNote.Size = new System.Drawing.Size(60, 15);
        this.lblUsedNote.TabIndex = 7;
        this.lblUsedNote.Text = "";
        //
        // lblRemark
        //
        this.lblRemark.Location = new System.Drawing.Point(40, 43);
        this.lblRemark.Name = "lblRemark";
        this.lblRemark.Size = new System.Drawing.Size(24, 15);
        this.lblRemark.Text = "비고";
        //
        // memoRemark
        //
        this.memoRemark.Location = new System.Drawing.Point(100, 40);
        this.memoRemark.Name = "memoRemark";
        this.memoRemark.Size = new System.Drawing.Size(774, 25);
        this.memoRemark.TabIndex = 9;
        //
        // lblHint
        //
        this.lblHint.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
        this.lblHint.Appearance.Options.UseForeColor = true;
        this.lblHint.Location = new System.Drawing.Point(28, 78);
        this.lblHint.Name = "lblHint";
        this.lblHint.Size = new System.Drawing.Size(60, 15);
        this.lblHint.TabIndex = 10;
        this.lblHint.Text = "※ 주원료(공정에 투입되는 LOT)는 BOM당 1개, 원자재/부자재/소모품은 몇 개든 등록할 수 있습니다. 소요수량은 상위 품목 1단위를 만드는 데 드는 수량입니다. 수정 내용은 이후에 만드는 작업지시에만 반영됩니다.";
        //
        // shLine
        //
        this.shLine.BackColor = System.Drawing.Color.White;
        this.shLine.Dock = System.Windows.Forms.DockStyle.Top;
        this.shLine.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shLine.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.shLine.Location = new System.Drawing.Point(3, 137);
        this.shLine.Name = "shLine";
        this.shLine.Size = new System.Drawing.Size(1664, 27);
        this.shLine.TabIndex = 2;
        this.shLine.Text = "구성품 (주원료 / 원자재 / 부자재 / 소모품)";
        //
        // panTool
        //
        this.panTool.Controls.Add(this.btnAddRow1);
        this.panTool.Controls.Add(this.btnDeletRow1);
        this.panTool.Dock = System.Windows.Forms.DockStyle.Top;
        this.panTool.Location = new System.Drawing.Point(3, 164);
        this.panTool.Name = "panTool";
        this.panTool.Size = new System.Drawing.Size(1664, 30);
        this.panTool.TabIndex = 3;
        //
        // btnAddRow1
        //
        this.btnAddRow1.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnAddRow1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(229)))), ((int)(((byte)(205)))));
        this.btnAddRow1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(239)))));
        this.btnAddRow1.Location = new System.Drawing.Point(8, 3);
        this.btnAddRow1.Name = "btnAddRow1";
        this.btnAddRow1.Size = new System.Drawing.Size(60, 24);
        this.btnAddRow1.TabIndex = 0;
        this.btnAddRow1.Text = "행추가";
        //
        // btnDeletRow1
        //
        this.btnDeletRow1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(198)))), ((int)(((byte)(193)))));
        this.btnDeletRow1.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnDeletRow1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(236)))), ((int)(((byte)(234)))));
        this.btnDeletRow1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
        this.btnDeletRow1.Location = new System.Drawing.Point(72, 3);
        this.btnDeletRow1.Name = "btnDeletRow1";
        this.btnDeletRow1.Size = new System.Drawing.Size(60, 24);
        this.btnDeletRow1.TabIndex = 1;
        this.btnDeletRow1.Text = "행삭제";
        //
        // grd2
        //
        this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd2.Location = new System.Drawing.Point(3, 194);
        this.grd2.MainView = this.gvw2;
        this.grd2.Name = "grd2";
        this.grd2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.lookupcolType,
        this.lookupcolUnit,
        this.lookupcolProc,
        this.popcolItem});
        this.grd2.Size = new System.Drawing.Size(1664, 552);
        this.grd2.TabIndex = 4;
        this.grd2.UseEmbeddedNavigator = false;
        this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw2});
        //
        // gvw2
        //
        this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colSerl,
        this.colCompType,
        this.colCompItem,
        this.colQtyPer,
        this.colUnit,
        this.colLossRate,
        this.colProcCd,
        this.colRemark});
        this.gvw2.GridControl = this.grd2;
        this.gvw2.HighlightFocusedRow = true;
        this.gvw2.Name = "gvw2";
        this.gvw2.OptionsView.ColumnAutoWidth = false;
        this.gvw2.OptionsView.ShowGroupPanel = false;
        //
        // lookupcolType
        //
        this.lookupcolType.AutoHeight = false;
        this.lookupcolType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolType.LookupKey = "L_PR0009";
        this.lookupcolType.Name = "lookupcolType";
        this.lookupcolType.NullText = "";
        //
        // lookupcolUnit
        //
        this.lookupcolUnit.AutoHeight = false;
        this.lookupcolUnit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolUnit.LookupKey = "L_CM0001";
        this.lookupcolUnit.Name = "lookupcolUnit";
        this.lookupcolUnit.NullText = "";
        //
        // lookupcolProc
        //
        this.lookupcolProc.AutoHeight = false;
        this.lookupcolProc.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolProc.LookupKey = "L_PRPROC";
        this.lookupcolProc.Name = "lookupcolProc";
        this.lookupcolProc.NullText = "";
        //
        // popcolItem
        //
        this.popcolItem.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "...")});
        this.popcolItem.LookupKey = "P_ITEM";
        this.popcolItem.Name = "popcolItem";
        //
        // colSerl
        //
        this.colSerl.Caption = "순번";
        this.colSerl.OptionsColumn.AllowEdit = false;
        this.colSerl.FieldName = "serl";
        this.colSerl.Name = "colSerl";
        this.colSerl.Visible = true;
        this.colSerl.VisibleIndex = 0;
        this.colSerl.Width = 50;
        //
        // colCompType
        //
        this.colCompType.Caption = "구분";
        this.colCompType.ColumnEdit = this.lookupcolType;
        this.colCompType.FieldName = "comp_type";
        this.colCompType.Name = "colCompType";
        this.colCompType.Visible = true;
        this.colCompType.VisibleIndex = 1;
        this.colCompType.Width = 90;
        //
        // colCompItem
        //
        this.colCompItem.Caption = "구성품";
        this.colCompItem.ColumnEdit = this.popcolItem;
        this.colCompItem.FieldName = "comp_item_id";
        this.colCompItem.Name = "colCompItem";
        this.colCompItem.Visible = true;
        this.colCompItem.VisibleIndex = 2;
        this.colCompItem.Width = 180;
        //
        // colQtyPer
        //
        this.colQtyPer.Caption = "소요수량";
        this.colQtyPer.DisplayFormat.FormatString = "#,##0.######";
        this.colQtyPer.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colQtyPer.FieldName = "qty_per";
        this.colQtyPer.Name = "colQtyPer";
        this.colQtyPer.Visible = true;
        this.colQtyPer.VisibleIndex = 3;
        this.colQtyPer.Width = 90;
        //
        // colUnit
        //
        this.colUnit.Caption = "단위";
        this.colUnit.ColumnEdit = this.lookupcolUnit;
        this.colUnit.FieldName = "unit_cd";
        this.colUnit.Name = "colUnit";
        this.colUnit.Visible = true;
        this.colUnit.VisibleIndex = 4;
        this.colUnit.Width = 70;
        //
        // colLossRate
        //
        this.colLossRate.Caption = "손실율(%)";
        this.colLossRate.DisplayFormat.FormatString = "#,##0.######";
        this.colLossRate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colLossRate.FieldName = "loss_rate";
        this.colLossRate.Name = "colLossRate";
        this.colLossRate.Visible = true;
        this.colLossRate.VisibleIndex = 5;
        this.colLossRate.Width = 80;
        //
        // colProcCd
        //
        this.colProcCd.Caption = "투입/소모 공정";
        this.colProcCd.ColumnEdit = this.lookupcolProc;
        this.colProcCd.FieldName = "proc_cd";
        this.colProcCd.Name = "colProcCd";
        this.colProcCd.Visible = true;
        this.colProcCd.VisibleIndex = 6;
        this.colProcCd.Width = 120;
        //
        // colRemark
        //
        this.colRemark.Caption = "비고";
        this.colRemark.FieldName = "remark";
        this.colRemark.Name = "colRemark";
        this.colRemark.Visible = true;
        this.colRemark.VisibleIndex = 7;
        this.colRemark.Width = 220;
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblSearchAccId);
        this.panHeader.Controls.Add(this.cboSearchAccId);
        this.panHeader.Controls.Add(this.lblSearchKeyword);
        this.panHeader.Controls.Add(this.txtSearchKeyword);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.Location = new System.Drawing.Point(5, 0);
        this.panHeader.Name = "panHeader";
        this.panHeader.Size = new System.Drawing.Size(1670, 49);
        this.panHeader.TabIndex = 0;
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
        // lblSearchKeyword
        //
        this.lblSearchKeyword.Location = new System.Drawing.Point(224, 18);
        this.lblSearchKeyword.Name = "lblSearchKeyword";
        this.lblSearchKeyword.Size = new System.Drawing.Size(60, 15);
        this.lblSearchKeyword.Text = "품번/품명";
        //
        // txtSearchKeyword
        //
        this.txtSearchKeyword.Location = new System.Drawing.Point(299, 15);
        this.txtSearchKeyword.Name = "txtSearchKeyword";
        this.txtSearchKeyword.Size = new System.Drawing.Size(180, 20);
        this.txtSearchKeyword.TabIndex = 1;
        //
        // frmBom
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 800);
        this.Controls.Add(this.panBase);
        this.Name = "frmBom";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).EndInit();
        this.panWork.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panLeft)).EndInit();
        this.panLeft.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkcolListUse)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panRight)).EndInit();
        this.panRight.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolType)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolUnit)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolProc)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolItem)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panTool)).EndInit();
        this.panTool.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
        this.panData.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtItemNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtItemId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkUseYn.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panWork;
    private PanelWyn panLeft;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkcolListUse;
    private DevExpress.XtraGrid.Columns.GridColumn colListNo;
    private DevExpress.XtraGrid.Columns.GridColumn colListNm;
    private DevExpress.XtraGrid.Columns.GridColumn colListCnt;
    private DevExpress.XtraGrid.Columns.GridColumn colListUse;
    private SectionHeaderWyn shList;
    private SplitterWyn splitterWyn1;
    private PanelWyn panRight;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private LookUpColumnEdit lookupcolType;
    private LookUpColumnEdit lookupcolUnit;
    private LookUpColumnEdit lookupcolProc;
    private PopupLookupColumnEdit popcolItem;
    private DevExpress.XtraGrid.Columns.GridColumn colSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colCompType;
    private DevExpress.XtraGrid.Columns.GridColumn colCompItem;
    private DevExpress.XtraGrid.Columns.GridColumn colQtyPer;
    private DevExpress.XtraGrid.Columns.GridColumn colUnit;
    private DevExpress.XtraGrid.Columns.GridColumn colLossRate;
    private DevExpress.XtraGrid.Columns.GridColumn colProcCd;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private PanelWyn panTool;
    private ButtonWyn btnAddRow1;
    private ButtonWyn btnDeletRow1;
    private SectionHeaderWyn shLine;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblAccId;
    private LookUpEditWyn cboAccId;
    private DevExpress.XtraEditors.LabelControl lblItemNm;
    private PopupLookupEditWyn txtItemNm;
    private TextEditWyn txtItemId;
    private DevExpress.XtraEditors.LabelControl lblUseYn;
    private DevExpress.XtraEditors.CheckEdit chkUseYn;
    private DevExpress.XtraEditors.LabelControl lblUsedNote;
    private DevExpress.XtraEditors.LabelControl lblRemark;
    private DevExpress.XtraEditors.MemoEdit memoRemark;
    private DevExpress.XtraEditors.LabelControl lblHint;
    private SectionHeaderWyn shHeader;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchKeyword;
    private TextEditWyn txtSearchKeyword;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private WYNLAB.Base.Controls.LookUpEditWyn cboSearchAccId;
}
