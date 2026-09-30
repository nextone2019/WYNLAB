// 라우팅관리(frmRoute) - 기초코드등록과 같은 구조: 좌측 라우팅 목록 그리드(grd1) + 스플리터 + 우측(헤더 입력 panData + 공정 체인 그리드 grd2). TPRROUTEM/TPRROUTED.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

public partial class frmRoute
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
        this.colListCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colListNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colListItem = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colListUse = new DevExpress.XtraGrid.Columns.GridColumn();
        this.shList = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
        this.panRight = new WYNLAB.Base.Controls.PanelWyn();
        this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
        this.lookupcolProc = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.lookupcolUnit = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.popcolCust = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
        this.popcolInItem = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
        this.popcolOutItem = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
        this.colSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colProcCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCustId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colInItemId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colInUnit = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colOutItemId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colOutUnit = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colSplitQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPriceUnit = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPrice = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.panTool = new WYNLAB.Base.Controls.PanelWyn();
        this.btnAddRow1 = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnDeletRow1 = new WYNLAB.Base.Controls.ButtonWyn();
        this.shLine = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panData = new WYNLAB.Base.Controls.PanelWyn();
        this.lblAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblRouteCd = new DevExpress.XtraEditors.LabelControl();
        this.txtRouteCd = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblRouteNm = new DevExpress.XtraEditors.LabelControl();
        this.txtRouteNm = new WYNLAB.Base.Controls.TextEditWyn();
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
        this.lblSearchRouteCd = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchRouteCd = new WYNLAB.Base.Controls.TextEditWyn();
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
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolProc)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolUnit)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolCust)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolInItem)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolOutItem)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panTool)).BeginInit();
        this.panTool.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
        this.panData.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtRouteCd.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtRouteNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtItemNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtItemId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkUseYn.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchRouteCd.Properties)).BeginInit();
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
        this.shList.Text = "라우팅 LIST";
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
        this.colListCd,
        this.colListNm,
        this.colListItem,
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
        // colListCd
        //
        this.colListCd.AppearanceCell.Options.UseTextOptions = true;
        this.colListCd.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        this.colListCd.Caption = "라우팅코드";
        this.colListCd.FieldName = "route_cd";
        this.colListCd.Name = "colListCd";
        this.colListCd.Visible = true;
        this.colListCd.VisibleIndex = 0;
        this.colListCd.Width = 95;
        //
        // colListNm
        //
        this.colListNm.Caption = "라우팅명";
        this.colListNm.FieldName = "route_nm";
        this.colListNm.Name = "colListNm";
        this.colListNm.Visible = true;
        this.colListNm.VisibleIndex = 1;
        this.colListNm.Width = 150;
        //
        // colListItem
        //
        this.colListItem.Caption = "완제품";
        this.colListItem.FieldName = "item_nm";
        this.colListItem.Name = "colListItem";
        this.colListItem.Visible = true;
        this.colListItem.VisibleIndex = 2;
        this.colListItem.Width = 110;
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
        this.shHeader.Text = "라우팅 정보등록";
        //
        // panData
        //
        this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        this.panData.Controls.Add(this.lblAccId);
        this.panData.Controls.Add(this.cboAccId);
        this.panData.Controls.Add(this.lblRouteCd);
        this.panData.Controls.Add(this.txtRouteCd);
        this.panData.Controls.Add(this.lblRouteNm);
        this.panData.Controls.Add(this.txtRouteNm);
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
        this.panData.Size = new System.Drawing.Size(1664, 138);
        this.panData.TabIndex = 1;
        //
        // lblAccId
        //
        this.lblAccId.Location = new System.Drawing.Point(28, 15);
        this.lblAccId.Name = "lblAccId";
        this.lblAccId.Size = new System.Drawing.Size(36, 15);
        this.lblAccId.TabIndex = 0;
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
        // lblRouteCd
        //
        this.lblRouteCd.Location = new System.Drawing.Point(282, 15);
        this.lblRouteCd.Name = "lblRouteCd";
        this.lblRouteCd.Size = new System.Drawing.Size(60, 15);
        this.lblRouteCd.TabIndex = 2;
        this.lblRouteCd.Text = "라우팅코드";
        //
        // txtRouteCd
        //
        this.txtRouteCd.Location = new System.Drawing.Point(354, 12);
        this.txtRouteCd.Name = "txtRouteCd";
        this.txtRouteCd.Required = true;
        this.txtRouteCd.Size = new System.Drawing.Size(150, 20);
        this.txtRouteCd.TabIndex = 3;
        //
        // lblRouteNm
        //
        this.lblRouteNm.Location = new System.Drawing.Point(536, 15);
        this.lblRouteNm.Name = "lblRouteNm";
        this.lblRouteNm.Size = new System.Drawing.Size(48, 15);
        this.lblRouteNm.TabIndex = 4;
        this.lblRouteNm.Text = "라우팅명";
        //
        // txtRouteNm
        //
        this.txtRouteNm.Location = new System.Drawing.Point(624, 12);
        this.txtRouteNm.Name = "txtRouteNm";
        this.txtRouteNm.Required = true;
        this.txtRouteNm.Size = new System.Drawing.Size(250, 20);
        this.txtRouteNm.TabIndex = 5;
        //
        // lblItemNm
        //
        this.lblItemNm.Location = new System.Drawing.Point(28, 43);
        this.lblItemNm.Name = "lblItemNm";
        this.lblItemNm.Size = new System.Drawing.Size(36, 15);
        this.lblItemNm.TabIndex = 6;
        this.lblItemNm.Text = "완제품";
        //
        // txtItemNm
        //
        this.txtItemNm.Location = new System.Drawing.Point(100, 40);
        this.txtItemNm.LookupKey = "P_ITEM";
        this.txtItemNm.MatchField = "item_nm";
        this.txtItemNm.Name = "txtItemNm";
        this.txtItemNm.Size = new System.Drawing.Size(150, 20);
        this.txtItemNm.TabIndex = 7;
        //
        // txtItemId
        //
        this.txtItemId.Location = new System.Drawing.Point(1200, 40);
        this.txtItemId.Name = "txtItemId";
        this.txtItemId.Size = new System.Drawing.Size(60, 20);
        this.txtItemId.TabIndex = 8;
        this.txtItemId.Visible = false;
        //
        // lblUseYn
        //
        this.lblUseYn.Location = new System.Drawing.Point(282, 43);
        this.lblUseYn.Name = "lblUseYn";
        this.lblUseYn.Size = new System.Drawing.Size(48, 15);
        this.lblUseYn.TabIndex = 9;
        this.lblUseYn.Text = "사용여부";
        //
        // chkUseYn
        //
        this.chkUseYn.Location = new System.Drawing.Point(352, 40);
        this.chkUseYn.Name = "chkUseYn";
        this.chkUseYn.Properties.Caption = "사용";
        this.chkUseYn.Size = new System.Drawing.Size(80, 20);
        this.chkUseYn.TabIndex = 10;
        //
        // lblUsedNote
        //
        this.lblUsedNote.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(60)))), ((int)(((byte)(50)))));
        this.lblUsedNote.Appearance.Options.UseForeColor = true;
        this.lblUsedNote.Location = new System.Drawing.Point(536, 43);
        this.lblUsedNote.Name = "lblUsedNote";
        this.lblUsedNote.Size = new System.Drawing.Size(60, 15);
        this.lblUsedNote.TabIndex = 11;
        this.lblUsedNote.Text = "";
        //
        // lblRemark
        //
        this.lblRemark.Location = new System.Drawing.Point(40, 71);
        this.lblRemark.Name = "lblRemark";
        this.lblRemark.Size = new System.Drawing.Size(24, 15);
        this.lblRemark.TabIndex = 12;
        this.lblRemark.Text = "비고";
        //
        // memoRemark
        //
        this.memoRemark.Location = new System.Drawing.Point(100, 68);
        this.memoRemark.Name = "memoRemark";
        this.memoRemark.Size = new System.Drawing.Size(774, 25);
        this.memoRemark.TabIndex = 13;
        //
        // lblHint
        //
        this.lblHint.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
        this.lblHint.Appearance.Options.UseForeColor = true;
        this.lblHint.Location = new System.Drawing.Point(28, 106);
        this.lblHint.Name = "lblHint";
        this.lblHint.Size = new System.Drawing.Size(60, 15);
        this.lblHint.TabIndex = 14;
        this.lblHint.Text = "※ 공정을 순서대로 추가하세요. 앞 공정의 산출품목이 다음 공정의 투입품목이 됩니다. 수정 내용은 이후에 만드는 작업지시에만 반영됩니다.";
        //
        // shLine
        //
        this.shLine.BackColor = System.Drawing.Color.White;
        this.shLine.Dock = System.Windows.Forms.DockStyle.Top;
        this.shLine.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shLine.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.shLine.Location = new System.Drawing.Point(3, 165);
        this.shLine.Name = "shLine";
        this.shLine.Size = new System.Drawing.Size(1664, 27);
        this.shLine.TabIndex = 2;
        this.shLine.Text = "공정 체인 (투입품목 → 산출품목)";
        //
        // panTool
        //
        this.panTool.Controls.Add(this.btnAddRow1);
        this.panTool.Controls.Add(this.btnDeletRow1);
        this.panTool.Dock = System.Windows.Forms.DockStyle.Top;
        this.panTool.Location = new System.Drawing.Point(3, 192);
        this.panTool.Name = "panTool";
        this.panTool.Size = new System.Drawing.Size(1664, 30);
        this.panTool.TabIndex = 3;
        //
        // btnAddRow1
        //
        this.btnAddRow1.Cursor = System.Windows.Forms.Cursors.Hand;
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
        this.grd2.Location = new System.Drawing.Point(3, 222);
        this.grd2.MainView = this.gvw2;
        this.grd2.Name = "grd2";
        this.grd2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.lookupcolProc,
        this.lookupcolUnit,
        this.popcolCust,
        this.popcolInItem,
        this.popcolOutItem});
        this.grd2.Size = new System.Drawing.Size(1664, 524);
        this.grd2.TabIndex = 4;
        this.grd2.UseEmbeddedNavigator = false;
        this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw2});
        //
        // gvw2
        //
        this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colSerl,
        this.colProcCd,
        this.colCustId,
        this.colInItemId,
        this.colInUnit,
        this.colOutItemId,
        this.colOutUnit,
        this.colSplitQty,
        this.colPriceUnit,
        this.colPrice,
        this.colRemark});
        this.gvw2.GridControl = this.grd2;
        this.gvw2.HighlightFocusedRow = true;
        this.gvw2.Name = "gvw2";
        this.gvw2.OptionsView.ColumnAutoWidth = false;
        this.gvw2.OptionsView.ShowGroupPanel = false;
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
        // lookupcolUnit
        //
        this.lookupcolUnit.AutoHeight = false;
        this.lookupcolUnit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolUnit.LookupKey = "L_CM0001";
        this.lookupcolUnit.Name = "lookupcolUnit";
        this.lookupcolUnit.NullText = "";
        //
        // popcolCust
        //
        this.popcolCust.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "...")});
        this.popcolCust.LookupKey = "P_CUST";
        this.popcolCust.PopupConditions = "p_cust_class=OS";
        this.popcolCust.Name = "popcolCust";
        //
        // popcolInItem
        //
        this.popcolInItem.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "...")});
        this.popcolInItem.LookupKey = "P_ITEM";
        this.popcolInItem.Name = "popcolInItem";
        //
        // popcolOutItem
        //
        this.popcolOutItem.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "...")});
        this.popcolOutItem.LookupKey = "P_ITEM";
        this.popcolOutItem.Name = "popcolOutItem";
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
        // colProcCd
        //
        this.colProcCd.Caption = "공정";
        this.colProcCd.ColumnEdit = this.lookupcolProc;
        this.colProcCd.FieldName = "proc_cd";
        this.colProcCd.Name = "colProcCd";
        this.colProcCd.Visible = true;
        this.colProcCd.VisibleIndex = 1;
        this.colProcCd.Width = 110;
        //
        // colCustId
        //
        this.colCustId.Caption = "기본 외주처";
        this.colCustId.ColumnEdit = this.popcolCust;
        this.colCustId.FieldName = "cust_id";
        this.colCustId.Name = "colCustId";
        this.colCustId.Visible = true;
        this.colCustId.VisibleIndex = 2;
        this.colCustId.Width = 120;
        //
        // colInItemId
        //
        this.colInItemId.Caption = "투입품목";
        this.colInItemId.ColumnEdit = this.popcolInItem;
        this.colInItemId.FieldName = "in_item_id";
        this.colInItemId.Name = "colInItemId";
        this.colInItemId.Visible = true;
        this.colInItemId.VisibleIndex = 3;
        this.colInItemId.Width = 120;
        //
        // colInUnit
        //
        this.colInUnit.Caption = "투입단위";
        this.colInUnit.ColumnEdit = this.lookupcolUnit;
        this.colInUnit.FieldName = "in_unit_cd";
        this.colInUnit.Name = "colInUnit";
        this.colInUnit.Visible = true;
        this.colInUnit.VisibleIndex = 4;
        this.colInUnit.Width = 70;
        //
        // colOutItemId
        //
        this.colOutItemId.Caption = "산출품목";
        this.colOutItemId.ColumnEdit = this.popcolOutItem;
        this.colOutItemId.FieldName = "out_item_id";
        this.colOutItemId.Name = "colOutItemId";
        this.colOutItemId.Visible = true;
        this.colOutItemId.VisibleIndex = 5;
        this.colOutItemId.Width = 120;
        //
        // colOutUnit
        //
        this.colOutUnit.Caption = "산출단위";
        this.colOutUnit.ColumnEdit = this.lookupcolUnit;
        this.colOutUnit.FieldName = "out_unit_cd";
        this.colOutUnit.Name = "colOutUnit";
        this.colOutUnit.Visible = true;
        this.colOutUnit.VisibleIndex = 6;
        this.colOutUnit.Width = 70;
        //
        // colSplitQty
        //
        this.colSplitQty.Caption = "분할수량";
        this.colSplitQty.DisplayFormat.FormatString = "#,##0.####";
        this.colSplitQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colSplitQty.FieldName = "split_qty";
        this.colSplitQty.Name = "colSplitQty";
        this.colSplitQty.Visible = true;
        this.colSplitQty.VisibleIndex = 7;
        this.colSplitQty.Width = 85;
        //
        // colPriceUnit
        //
        this.colPriceUnit.Caption = "정산단위";
        this.colPriceUnit.ColumnEdit = this.lookupcolUnit;
        this.colPriceUnit.FieldName = "price_unit_cd";
        this.colPriceUnit.Name = "colPriceUnit";
        this.colPriceUnit.Visible = true;
        this.colPriceUnit.VisibleIndex = 8;
        this.colPriceUnit.Width = 70;
        //
        // colPrice
        //
        this.colPrice.Caption = "기본 가공단가";
        this.colPrice.DisplayFormat.FormatString = "#,##0.####";
        this.colPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colPrice.FieldName = "price";
        this.colPrice.Name = "colPrice";
        this.colPrice.Visible = true;
        this.colPrice.VisibleIndex = 9;
        this.colPrice.Width = 100;
        //
        // colRemark
        //
        this.colRemark.Caption = "비고";
        this.colRemark.FieldName = "remark";
        this.colRemark.Name = "colRemark";
        this.colRemark.Visible = true;
        this.colRemark.VisibleIndex = 10;
        this.colRemark.Width = 200;
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblSearchRouteCd);
        this.panHeader.Controls.Add(this.txtSearchRouteCd);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.Location = new System.Drawing.Point(5, 0);
        this.panHeader.Name = "panHeader";
        this.panHeader.Size = new System.Drawing.Size(1670, 49);
        this.panHeader.TabIndex = 0;
        //
        // lblSearchRouteCd
        //
        this.lblSearchRouteCd.Location = new System.Drawing.Point(25, 18);
        this.lblSearchRouteCd.Name = "lblSearchRouteCd";
        this.lblSearchRouteCd.Size = new System.Drawing.Size(60, 15);
        this.lblSearchRouteCd.TabIndex = 0;
        this.lblSearchRouteCd.Text = "라우팅코드/명";
        //
        // txtSearchRouteCd
        //
        this.txtSearchRouteCd.Location = new System.Drawing.Point(100, 15);
        this.txtSearchRouteCd.Name = "txtSearchRouteCd";
        this.txtSearchRouteCd.Size = new System.Drawing.Size(180, 20);
        this.txtSearchRouteCd.TabIndex = 1;
        //
        // frmRoute
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 800);
        this.Controls.Add(this.panBase);
        this.Name = "frmRoute";
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
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolProc)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolUnit)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolCust)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolInItem)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolOutItem)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panTool)).EndInit();
        this.panTool.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
        this.panData.ResumeLayout(false);
        this.panData.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtRouteCd.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtRouteNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtItemNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtItemId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkUseYn.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        this.panHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchRouteCd.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panWork;
    private PanelWyn panLeft;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkcolListUse;
    private DevExpress.XtraGrid.Columns.GridColumn colListCd;
    private DevExpress.XtraGrid.Columns.GridColumn colListNm;
    private DevExpress.XtraGrid.Columns.GridColumn colListItem;
    private DevExpress.XtraGrid.Columns.GridColumn colListUse;
    private SectionHeaderWyn shList;
    private SplitterWyn splitterWyn1;
    private PanelWyn panRight;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private LookUpColumnEdit lookupcolProc;
    private LookUpColumnEdit lookupcolUnit;
    private PopupLookupColumnEdit popcolCust;
    private PopupLookupColumnEdit popcolInItem;
    private PopupLookupColumnEdit popcolOutItem;
    private DevExpress.XtraGrid.Columns.GridColumn colSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colProcCd;
    private DevExpress.XtraGrid.Columns.GridColumn colCustId;
    private DevExpress.XtraGrid.Columns.GridColumn colInItemId;
    private DevExpress.XtraGrid.Columns.GridColumn colInUnit;
    private DevExpress.XtraGrid.Columns.GridColumn colOutItemId;
    private DevExpress.XtraGrid.Columns.GridColumn colOutUnit;
    private DevExpress.XtraGrid.Columns.GridColumn colSplitQty;
    private DevExpress.XtraGrid.Columns.GridColumn colPriceUnit;
    private DevExpress.XtraGrid.Columns.GridColumn colPrice;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private PanelWyn panTool;
    private ButtonWyn btnAddRow1;
    private ButtonWyn btnDeletRow1;
    private SectionHeaderWyn shLine;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblAccId;
    private LookUpEditWyn cboAccId;
    private DevExpress.XtraEditors.LabelControl lblRouteCd;
    private TextEditWyn txtRouteCd;
    private DevExpress.XtraEditors.LabelControl lblRouteNm;
    private TextEditWyn txtRouteNm;
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
    private DevExpress.XtraEditors.LabelControl lblSearchRouteCd;
    private TextEditWyn txtSearchRouteCd;
}
