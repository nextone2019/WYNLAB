// 웨이퍼입고(frmRcv) - 좌측 입고 목록 그리드(grd1) + 스플리터 + 우측 입력(panData). TPRRCV. 라우팅관리와 같은 목록-상세 구조(하위 그리드는 없음).
// 작업지시와 무관하게 먼저 도착하는 웨이퍼를 품목/LOT번호/수량/입고창고로 직접 등록한다(작업지시가 나중에 이 LOT를 골라 배정).
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

public partial class frmRcv
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
        this.lookupcolListStat = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.colListNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colListDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colListLot = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colListItem = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colListQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colListWh = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colListStat = new DevExpress.XtraGrid.Columns.GridColumn();
        this.shList = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
        this.panRight = new WYNLAB.Base.Controls.PanelWyn();
        this.panData = new WYNLAB.Base.Controls.PanelWyn();
        this.lblAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblStatCd = new DevExpress.XtraEditors.LabelControl();
        this.cboStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblRcvNo = new DevExpress.XtraEditors.LabelControl();
        this.txtRcvNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblRcvDate = new DevExpress.XtraEditors.LabelControl();
        this.dteRcvDate = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblLotNo = new DevExpress.XtraEditors.LabelControl();
        this.txtLotNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblQty = new DevExpress.XtraEditors.LabelControl();
        this.spnQty = new WYNLAB.Base.Controls.SpinEditWyn();
        this.lblItemNm = new DevExpress.XtraEditors.LabelControl();
        this.txtItemNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.txtItemId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblWhNm = new DevExpress.XtraEditors.LabelControl();
        this.txtWhNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.txtWhId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSup = new DevExpress.XtraEditors.LabelControl();
        this.txtSupNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.txtSupId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblWoNo = new DevExpress.XtraEditors.LabelControl();
        this.txtWoNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblRemark = new DevExpress.XtraEditors.LabelControl();
        this.memoRemark = new DevExpress.XtraEditors.MemoEdit();
        this.lblHint = new DevExpress.XtraEditors.LabelControl();
        this.btnConfirm = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnConfirmCancel = new WYNLAB.Base.Controls.ButtonWyn();
        this.shHeader = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
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
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolListStat)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panRight)).BeginInit();
        this.panRight.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
        this.panData.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtRcvNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteRcvDate.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteRcvDate.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtLotNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.spnQty.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtItemNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtItemId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWhNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWhId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSupNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSupId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWoNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).BeginInit();
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
        this.panLeft.Size = new System.Drawing.Size(560, 746);
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
        this.shList.Size = new System.Drawing.Size(560, 27);
        this.shList.TabIndex = 0;
        this.shList.Text = "웨이퍼 입고 LIST";
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd1.Location = new System.Drawing.Point(0, 27);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.lookupcolListStat});
        this.grd1.Size = new System.Drawing.Size(560, 719);
        this.grd1.TabIndex = 1;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colListNo,
        this.colListDate,
        this.colListLot,
        this.colListItem,
        this.colListQty,
        this.colListWh,
        this.colListStat});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsBehavior.Editable = false;
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        //
        // lookupcolListStat
        //
        this.lookupcolListStat.AutoHeight = false;
        this.lookupcolListStat.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolListStat.LookupKey = "L_PR0006";
        this.lookupcolListStat.Name = "lookupcolListStat";
        this.lookupcolListStat.NullText = "";
        //
        // colListNo
        //
        this.colListNo.AppearanceCell.Options.UseTextOptions = true;
        this.colListNo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        this.colListNo.Caption = "입고번호";
        this.colListNo.FieldName = "rcv_no";
        this.colListNo.Name = "colListNo";
        this.colListNo.Visible = true;
        this.colListNo.VisibleIndex = 0;
        this.colListNo.Width = 92;
        //
        // colListDate
        //
        this.colListDate.Caption = "입고일자";
        this.colListDate.FieldName = "rcv_date";
        this.colListDate.Name = "colListDate";
        this.colListDate.Visible = true;
        this.colListDate.VisibleIndex = 1;
        this.colListDate.Width = 76;
        //
        // colListLot
        //
        this.colListLot.Caption = "LOT";
        this.colListLot.FieldName = "lot_no";
        this.colListLot.Name = "colListLot";
        this.colListLot.Visible = true;
        this.colListLot.VisibleIndex = 2;
        this.colListLot.Width = 100;
        //
        // colListItem
        //
        this.colListItem.Caption = "품목";
        this.colListItem.FieldName = "item_nm";
        this.colListItem.Name = "colListItem";
        this.colListItem.Visible = true;
        this.colListItem.VisibleIndex = 3;
        this.colListItem.Width = 80;
        //
        // colListQty
        //
        this.colListQty.Caption = "수량";
        this.colListQty.DisplayFormat.FormatString = "#,##0.####";
        this.colListQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colListQty.FieldName = "qty";
        this.colListQty.Name = "colListQty";
        this.colListQty.Visible = true;
        this.colListQty.VisibleIndex = 4;
        this.colListQty.Width = 55;
        //
        // colListWh
        //
        this.colListWh.Caption = "입고 창고";
        this.colListWh.FieldName = "wh_nm";
        this.colListWh.Name = "colListWh";
        this.colListWh.Visible = true;
        this.colListWh.VisibleIndex = 5;
        this.colListWh.Width = 90;
        //
        // colListStat
        //
        this.colListStat.Caption = "상태";
        this.colListStat.ColumnEdit = this.lookupcolListStat;
        this.colListStat.FieldName = "stat_cd";
        this.colListStat.Name = "colListStat";
        this.colListStat.Visible = true;
        this.colListStat.VisibleIndex = 6;
        this.colListStat.Width = 55;
        //
        // splitterWyn1
        //
        this.splitterWyn1.BackColor = System.Drawing.Color.White;
        this.splitterWyn1.Location = new System.Drawing.Point(560, 0);
        this.splitterWyn1.Name = "splitterWyn1";
        this.splitterWyn1.Size = new System.Drawing.Size(10, 746);
        this.splitterWyn1.TabIndex = 1;
        this.splitterWyn1.TabStop = false;
        //
        // panRight
        //
        this.panRight.Controls.Add(this.panData);
        this.panRight.Controls.Add(this.shHeader);
        this.panRight.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panRight.Location = new System.Drawing.Point(570, 0);
        this.panRight.Name = "panRight";
        this.panRight.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
        this.panRight.Size = new System.Drawing.Size(1100, 746);
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
        this.shHeader.Size = new System.Drawing.Size(1097, 27);
        this.shHeader.TabIndex = 0;
        this.shHeader.Text = "웨이퍼 입고 정보등록";
        //
        // panData
        //
        this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        this.panData.Controls.Add(this.lblAccId);
        this.panData.Controls.Add(this.cboAccId);
        this.panData.Controls.Add(this.lblStatCd);
        this.panData.Controls.Add(this.cboStatCd);
        this.panData.Controls.Add(this.lblRcvNo);
        this.panData.Controls.Add(this.txtRcvNo);
        this.panData.Controls.Add(this.lblRcvDate);
        this.panData.Controls.Add(this.dteRcvDate);
        this.panData.Controls.Add(this.lblLotNo);
        this.panData.Controls.Add(this.txtLotNo);
        this.panData.Controls.Add(this.lblQty);
        this.panData.Controls.Add(this.spnQty);
        this.panData.Controls.Add(this.lblItemNm);
        this.panData.Controls.Add(this.txtItemNm);
        this.panData.Controls.Add(this.txtItemId);
        this.panData.Controls.Add(this.lblWhNm);
        this.panData.Controls.Add(this.txtWhNm);
        this.panData.Controls.Add(this.txtWhId);
        this.panData.Controls.Add(this.lblSup);
        this.panData.Controls.Add(this.txtSupNm);
        this.panData.Controls.Add(this.txtSupId);
        this.panData.Controls.Add(this.lblWoNo);
        this.panData.Controls.Add(this.txtWoNo);
        this.panData.Controls.Add(this.lblRemark);
        this.panData.Controls.Add(this.memoRemark);
        this.panData.Controls.Add(this.lblHint);
        this.panData.Controls.Add(this.btnConfirm);
        this.panData.Controls.Add(this.btnConfirmCancel);
        this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panData.Location = new System.Drawing.Point(3, 27);
        this.panData.Name = "panData";
        this.panData.Size = new System.Drawing.Size(1097, 719);
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
        // lblStatCd
        //
        this.lblStatCd.Location = new System.Drawing.Point(282, 15);
        this.lblStatCd.Name = "lblStatCd";
        this.lblStatCd.Size = new System.Drawing.Size(48, 15);
        this.lblStatCd.TabIndex = 2;
        this.lblStatCd.Text = "진행상태";
        //
        // cboStatCd
        //
        this.cboStatCd.Location = new System.Drawing.Point(354, 12);
        this.cboStatCd.LookupKey = "L_PR0006";
        this.cboStatCd.Name = "cboStatCd";
        this.cboStatCd.Properties.NullText = "";
        this.cboStatCd.Properties.ReadOnly = true;
        this.cboStatCd.Size = new System.Drawing.Size(150, 20);
        this.cboStatCd.TabIndex = 3;
        //
        // lblRcvNo
        //
        this.lblRcvNo.Location = new System.Drawing.Point(536, 15);
        this.lblRcvNo.Name = "lblRcvNo";
        this.lblRcvNo.Size = new System.Drawing.Size(48, 15);
        this.lblRcvNo.TabIndex = 4;
        this.lblRcvNo.Text = "입고번호";
        //
        // txtRcvNo
        //
        this.txtRcvNo.Location = new System.Drawing.Point(624, 12);
        this.txtRcvNo.Name = "txtRcvNo";
        this.txtRcvNo.Properties.ReadOnly = true;
        this.txtRcvNo.Size = new System.Drawing.Size(150, 20);
        this.txtRcvNo.TabIndex = 5;
        //
        // lblRcvDate
        //
        this.lblRcvDate.Location = new System.Drawing.Point(28, 43);
        this.lblRcvDate.Name = "lblRcvDate";
        this.lblRcvDate.Size = new System.Drawing.Size(48, 15);
        this.lblRcvDate.TabIndex = 6;
        this.lblRcvDate.Text = "입고일자";
        //
        // dteRcvDate
        //
        this.dteRcvDate.Location = new System.Drawing.Point(100, 40);
        this.dteRcvDate.Name = "dteRcvDate";
        this.dteRcvDate.Required = true;
        this.dteRcvDate.Size = new System.Drawing.Size(150, 20);
        this.dteRcvDate.TabIndex = 7;
        //
        // lblLotNo
        //
        this.lblLotNo.Location = new System.Drawing.Point(282, 43);
        this.lblLotNo.Name = "lblLotNo";
        this.lblLotNo.Size = new System.Drawing.Size(48, 15);
        this.lblLotNo.TabIndex = 8;
        this.lblLotNo.Text = "LOT번호";
        //
        // txtLotNo
        //
        this.txtLotNo.Location = new System.Drawing.Point(354, 40);
        this.txtLotNo.Name = "txtLotNo";
        this.txtLotNo.Required = true;
        this.txtLotNo.Size = new System.Drawing.Size(150, 20);
        this.txtLotNo.TabIndex = 9;
        //
        // lblQty
        //
        this.lblQty.Location = new System.Drawing.Point(536, 43);
        this.lblQty.Name = "lblQty";
        this.lblQty.Size = new System.Drawing.Size(60, 15);
        this.lblQty.TabIndex = 10;
        this.lblQty.Text = "입고수량(장)";
        //
        // spnQty
        //
        this.spnQty.Location = new System.Drawing.Point(624, 40);
        this.spnQty.Name = "spnQty";
        this.spnQty.Size = new System.Drawing.Size(150, 20);
        this.spnQty.TabIndex = 11;
        //
        // lblItemNm
        //
        this.lblItemNm.Location = new System.Drawing.Point(28, 71);
        this.lblItemNm.Name = "lblItemNm";
        this.lblItemNm.Size = new System.Drawing.Size(24, 15);
        this.lblItemNm.TabIndex = 12;
        this.lblItemNm.Text = "품목";
        //
        // txtItemNm
        //
        this.txtItemNm.Location = new System.Drawing.Point(100, 68);
        this.txtItemNm.LookupKey = "P_ITEM";
        this.txtItemNm.MatchField = "item_nm";
        this.txtItemNm.Name = "txtItemNm";
        this.txtItemNm.Required = true;
        this.txtItemNm.Size = new System.Drawing.Size(150, 20);
        this.txtItemNm.TabIndex = 13;
        //
        // txtItemId
        //
        this.txtItemId.Location = new System.Drawing.Point(1000, 40);
        this.txtItemId.Name = "txtItemId";
        this.txtItemId.Size = new System.Drawing.Size(60, 20);
        this.txtItemId.TabIndex = 14;
        this.txtItemId.Visible = false;
        //
        // lblWhNm
        //
        this.lblWhNm.Location = new System.Drawing.Point(282, 71);
        this.lblWhNm.Name = "lblWhNm";
        this.lblWhNm.Size = new System.Drawing.Size(60, 15);
        this.lblWhNm.TabIndex = 15;
        this.lblWhNm.Text = "입고 창고";
        //
        // txtWhNm
        //
        this.txtWhNm.Location = new System.Drawing.Point(354, 68);
        this.txtWhNm.LookupKey = "P_WH";
        this.txtWhNm.MatchField = "wh_nm";
        this.txtWhNm.Name = "txtWhNm";
        this.txtWhNm.Required = true;
        this.txtWhNm.Size = new System.Drawing.Size(150, 20);
        this.txtWhNm.TabIndex = 16;
        //
        // txtWhId
        //
        this.txtWhId.Location = new System.Drawing.Point(1000, 68);
        this.txtWhId.Name = "txtWhId";
        this.txtWhId.Size = new System.Drawing.Size(60, 20);
        this.txtWhId.TabIndex = 17;
        this.txtWhId.Visible = false;
        //
        // lblSup
        //
        this.lblSup.Location = new System.Drawing.Point(536, 71);
        this.lblSup.Name = "lblSup";
        this.lblSup.Size = new System.Drawing.Size(36, 15);
        this.lblSup.TabIndex = 18;
        this.lblSup.Text = "공급처";
        //
        // txtSupNm
        //
        this.txtSupNm.Location = new System.Drawing.Point(624, 68);
        this.txtSupNm.LookupKey = "P_CUST";
        this.txtSupNm.PopupConditions = "p_cust_class=PO";
        this.txtSupNm.MatchField = "cust_nm";
        this.txtSupNm.Name = "txtSupNm";
        this.txtSupNm.Size = new System.Drawing.Size(150, 20);
        this.txtSupNm.TabIndex = 19;
        //
        // txtSupId
        //
        this.txtSupId.Location = new System.Drawing.Point(1000, 96);
        this.txtSupId.Name = "txtSupId";
        this.txtSupId.Size = new System.Drawing.Size(60, 20);
        this.txtSupId.TabIndex = 20;
        this.txtSupId.Visible = false;
        //
        // lblWoNo
        //
        this.lblWoNo.Location = new System.Drawing.Point(28, 99);
        this.lblWoNo.Name = "lblWoNo";
        this.lblWoNo.Size = new System.Drawing.Size(60, 15);
        this.lblWoNo.TabIndex = 21;
        this.lblWoNo.Text = "배정 작업지시";
        //
        // txtWoNo
        //
        this.txtWoNo.Location = new System.Drawing.Point(100, 96);
        this.txtWoNo.Name = "txtWoNo";
        this.txtWoNo.Properties.ReadOnly = true;
        this.txtWoNo.Size = new System.Drawing.Size(150, 20);
        this.txtWoNo.TabIndex = 22;
        //
        // lblRemark
        //
        this.lblRemark.Location = new System.Drawing.Point(40, 127);
        this.lblRemark.Name = "lblRemark";
        this.lblRemark.Size = new System.Drawing.Size(24, 15);
        this.lblRemark.TabIndex = 23;
        this.lblRemark.Text = "비고";
        //
        // memoRemark
        //
        this.memoRemark.Location = new System.Drawing.Point(100, 124);
        this.memoRemark.Name = "memoRemark";
        this.memoRemark.Size = new System.Drawing.Size(674, 25);
        this.memoRemark.TabIndex = 24;
        //
        // lblHint
        //
        this.lblHint.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
        this.lblHint.Appearance.Options.UseForeColor = true;
        this.lblHint.Location = new System.Drawing.Point(28, 168);
        this.lblHint.Name = "lblHint";
        this.lblHint.Size = new System.Drawing.Size(60, 15);
        this.lblHint.TabIndex = 25;
        this.lblHint.Text = "※ 공급처(TSMC 등)에서 첫 공정 외주처(Amkor)로 직송되어 도착한 웨이퍼를 등록합니다. 작업지시와 무관하게 먼저 입고하고, 확정하면 입고 창고에 LOT 재고가 생깁니다. 작업지시는 나중에 [입고 LOT 불러오기]로 이 LOT를 배정합니다.";
        //
        // btnConfirm
        //
        this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnConfirm.Location = new System.Drawing.Point(820, 12);
        this.btnConfirm.Name = "btnConfirm";
        this.btnConfirm.Size = new System.Drawing.Size(170, 24);
        this.btnConfirm.TabIndex = 26;
        this.btnConfirm.Text = "입고 확정";
        this.btnConfirm.ToolTip = "입고를 확정합니다. 입고 창고에 LOT 재고가 생기고 작업지시에서 고를 수 있게 됩니다.";
        //
        // btnConfirmCancel
        //
        this.btnConfirmCancel.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnConfirmCancel.Location = new System.Drawing.Point(820, 40);
        this.btnConfirmCancel.Name = "btnConfirmCancel";
        this.btnConfirmCancel.Size = new System.Drawing.Size(170, 24);
        this.btnConfirmCancel.TabIndex = 27;
        this.btnConfirmCancel.Text = "입고 확정취소";
        this.btnConfirmCancel.ToolTip = "확정을 취소하고 재고를 되돌립니다(작업지시에 배정되었거나 공정실적에서 쓴 LOT는 취소할 수 없습니다).";
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblSearchKeyword);
        this.panHeader.Controls.Add(this.txtSearchKeyword);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.Location = new System.Drawing.Point(5, 0);
        this.panHeader.Name = "panHeader";
        this.panHeader.Size = new System.Drawing.Size(1670, 49);
        this.panHeader.TabIndex = 0;
        //
        // lblSearchKeyword
        //
        this.lblSearchKeyword.Location = new System.Drawing.Point(25, 18);
        this.lblSearchKeyword.Name = "lblSearchKeyword";
        this.lblSearchKeyword.Size = new System.Drawing.Size(96, 15);
        this.lblSearchKeyword.TabIndex = 0;
        this.lblSearchKeyword.Text = "입고번호/LOT/품목";
        //
        // txtSearchKeyword
        //
        this.txtSearchKeyword.Location = new System.Drawing.Point(140, 15);
        this.txtSearchKeyword.Name = "txtSearchKeyword";
        this.txtSearchKeyword.Size = new System.Drawing.Size(180, 20);
        this.txtSearchKeyword.TabIndex = 1;
        //
        // frmRcv
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 800);
        this.Controls.Add(this.panBase);
        this.Name = "frmRcv";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).EndInit();
        this.panWork.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panLeft)).EndInit();
        this.panLeft.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolListStat)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panRight)).EndInit();
        this.panRight.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
        this.panData.ResumeLayout(false);
        this.panData.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtRcvNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteRcvDate.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteRcvDate.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtLotNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.spnQty.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtItemNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtItemId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWhNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWhId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSupNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSupId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWoNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        this.panHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panWork;
    private PanelWyn panLeft;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private LookUpColumnEdit lookupcolListStat;
    private DevExpress.XtraGrid.Columns.GridColumn colListNo;
    private DevExpress.XtraGrid.Columns.GridColumn colListDate;
    private DevExpress.XtraGrid.Columns.GridColumn colListLot;
    private DevExpress.XtraGrid.Columns.GridColumn colListItem;
    private DevExpress.XtraGrid.Columns.GridColumn colListQty;
    private DevExpress.XtraGrid.Columns.GridColumn colListWh;
    private DevExpress.XtraGrid.Columns.GridColumn colListStat;
    private SectionHeaderWyn shList;
    private SplitterWyn splitterWyn1;
    private PanelWyn panRight;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblAccId;
    private LookUpEditWyn cboAccId;
    private DevExpress.XtraEditors.LabelControl lblStatCd;
    private LookUpEditWyn cboStatCd;
    private DevExpress.XtraEditors.LabelControl lblRcvNo;
    private TextEditWyn txtRcvNo;
    private DevExpress.XtraEditors.LabelControl lblRcvDate;
    private DateEditWyn dteRcvDate;
    private DevExpress.XtraEditors.LabelControl lblLotNo;
    private TextEditWyn txtLotNo;
    private DevExpress.XtraEditors.LabelControl lblQty;
    private SpinEditWyn spnQty;
    private DevExpress.XtraEditors.LabelControl lblItemNm;
    private PopupLookupEditWyn txtItemNm;
    private TextEditWyn txtItemId;
    private DevExpress.XtraEditors.LabelControl lblWhNm;
    private PopupLookupEditWyn txtWhNm;
    private TextEditWyn txtWhId;
    private DevExpress.XtraEditors.LabelControl lblSup;
    private PopupLookupEditWyn txtSupNm;
    private TextEditWyn txtSupId;
    private DevExpress.XtraEditors.LabelControl lblWoNo;
    private TextEditWyn txtWoNo;
    private DevExpress.XtraEditors.LabelControl lblRemark;
    private DevExpress.XtraEditors.MemoEdit memoRemark;
    private DevExpress.XtraEditors.LabelControl lblHint;
    private ButtonWyn btnConfirm;
    private ButtonWyn btnConfirmCancel;
    private SectionHeaderWyn shHeader;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchKeyword;
    private TextEditWyn txtSearchKeyword;
}
