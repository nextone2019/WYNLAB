// 작업지시(frmWo) - Master-One Sheet 구조(TPRWOM/TPRWOD). 헤더 입력 + 공정별 외주 진행 그리드(라우팅 복사본, 외주처/단가/납기만 수정) + LOT 현황 그리드.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

public partial class frmWo
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
        this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
        this.panMat = new WYNLAB.Base.Controls.PanelWyn();
        this.shMat = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.grd3 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw3 = new WYNLAB.Base.Controls.GridViewWyn();
        this.lookupcolMatType = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.colMatProc = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMatType = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMatItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMatItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMatUnit = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMatQtyPer = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMatLoss = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMatReq = new DevExpress.XtraGrid.Columns.GridColumn();
        this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
        this.colLotSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLotNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLotItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLotItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLotUnit = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLotInit = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLotWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLotStock = new DevExpress.XtraGrid.Columns.GridColumn();
        this.shLot = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.popcolCust = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
        this.popcolWh = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
        this.datecolDue = new WYNLAB.Base.Controls.DateColumnEdit();
        this.lookupcolStat = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.colSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colProcNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCustId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colWhId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colInItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colOutItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colSplitQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPriceUnit = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPrice = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDueDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colInQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colGoodQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colBadQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colYield = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.shProc = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panData = new WYNLAB.Base.Controls.PanelWyn();
        this.lblAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblStatCd = new DevExpress.XtraEditors.LabelControl();
        this.cboStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblWoNo = new DevExpress.XtraEditors.LabelControl();
        this.txtWoNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblWoDate = new DevExpress.XtraEditors.LabelControl();
        this.dteWoDate = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblRouteId = new DevExpress.XtraEditors.LabelControl();
        this.cboRouteId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblDelvDate = new DevExpress.XtraEditors.LabelControl();
        this.dteDelvDate = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblStartLotNo = new DevExpress.XtraEditors.LabelControl();
        this.txtStartLotNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.txtStartLotId = new WYNLAB.Base.Controls.TextEditWyn();
        this.btnPickLot = new WYNLAB.Base.Controls.ButtonWyn();
        this.lblStartQty = new DevExpress.XtraEditors.LabelControl();
        this.spnStartQty = new WYNLAB.Base.Controls.SpinEditWyn();
        this.lblItemNm = new DevExpress.XtraEditors.LabelControl();
        this.txtItemNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.txtItemId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSoNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSoNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.txtSoId = new WYNLAB.Base.Controls.TextEditWyn();
        this.txtSoSerl = new WYNLAB.Base.Controls.TextEditWyn();
        this.btnPickSo = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnClearSo = new WYNLAB.Base.Controls.ButtonWyn();
        this.lblDept = new DevExpress.XtraEditors.LabelControl();
        this.txtDeptNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.txtDeptId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblEmp = new DevExpress.XtraEditors.LabelControl();
        this.txtEmpNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.txtEmpId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblRemark = new DevExpress.XtraEditors.LabelControl();
        this.memoRemark = new DevExpress.XtraEditors.MemoEdit();
        this.lblHint = new DevExpress.XtraEditors.LabelControl();
        this.btnConfirm = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnConfirmCancel = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnStop = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnStopCancel = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnComplete = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnCompleteCancel = new WYNLAB.Base.Controls.ButtonWyn();
        this.shHeader = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblSearchWoNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchWoNo = new WYNLAB.Base.Controls.TextEditWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        this.panBase.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).BeginInit();
        this.panWork.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panMat)).BeginInit();
        this.panMat.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolMatType)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolCust)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolWh)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.datecolDue)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolStat)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
        this.panData.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWoNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteWoDate.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteWoDate.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboRouteId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteDelvDate.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteDelvDate.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtStartLotNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtStartLotId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.spnStartQty.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtItemNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtItemId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSoNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSoId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSoSerl.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchWoNo.Properties)).BeginInit();
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
        this.panWork.Controls.Add(this.grd2);
        this.panWork.Controls.Add(this.panMat);
        this.panWork.Controls.Add(this.shLot);
        this.panWork.Controls.Add(this.splitterWyn1);
        this.panWork.Controls.Add(this.grd1);
        this.panWork.Controls.Add(this.shProc);
        this.panWork.Controls.Add(this.panData);
        this.panWork.Controls.Add(this.shHeader);
        this.panWork.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panWork.Location = new System.Drawing.Point(5, 49);
        this.panWork.Name = "panWork";
        this.panWork.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.panWork.Size = new System.Drawing.Size(1670, 746);
        this.panWork.TabIndex = 1;
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
        this.shHeader.Text = "작업지시 정보등록";
        //
        // panData
        //
        this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        this.panData.Controls.Add(this.lblAccId);
        this.panData.Controls.Add(this.cboAccId);
        this.panData.Controls.Add(this.lblStatCd);
        this.panData.Controls.Add(this.cboStatCd);
        this.panData.Controls.Add(this.lblWoNo);
        this.panData.Controls.Add(this.txtWoNo);
        this.panData.Controls.Add(this.lblWoDate);
        this.panData.Controls.Add(this.dteWoDate);
        this.panData.Controls.Add(this.lblRouteId);
        this.panData.Controls.Add(this.cboRouteId);
        this.panData.Controls.Add(this.lblDelvDate);
        this.panData.Controls.Add(this.dteDelvDate);
        this.panData.Controls.Add(this.lblStartLotNo);
        this.panData.Controls.Add(this.txtStartLotNo);
        this.panData.Controls.Add(this.txtStartLotId);
        this.panData.Controls.Add(this.btnPickLot);
        this.panData.Controls.Add(this.lblStartQty);
        this.panData.Controls.Add(this.spnStartQty);
        this.panData.Controls.Add(this.lblItemNm);
        this.panData.Controls.Add(this.txtItemNm);
        this.panData.Controls.Add(this.txtItemId);
        this.panData.Controls.Add(this.lblSoNo);
        this.panData.Controls.Add(this.txtSoNo);
        this.panData.Controls.Add(this.txtSoId);
        this.panData.Controls.Add(this.txtSoSerl);
        this.panData.Controls.Add(this.btnPickSo);
        this.panData.Controls.Add(this.btnClearSo);
        this.panData.Controls.Add(this.lblDept);
        this.panData.Controls.Add(this.txtDeptNm);
        this.panData.Controls.Add(this.txtDeptId);
        this.panData.Controls.Add(this.lblEmp);
        this.panData.Controls.Add(this.txtEmpNm);
        this.panData.Controls.Add(this.txtEmpId);
        this.panData.Controls.Add(this.lblRemark);
        this.panData.Controls.Add(this.memoRemark);
        this.panData.Controls.Add(this.lblHint);
        this.panData.Controls.Add(this.btnConfirm);
        this.panData.Controls.Add(this.btnConfirmCancel);
        this.panData.Controls.Add(this.btnStop);
        this.panData.Controls.Add(this.btnStopCancel);
        this.panData.Controls.Add(this.btnComplete);
        this.panData.Controls.Add(this.btnCompleteCancel);
        this.panData.Dock = System.Windows.Forms.DockStyle.Top;
        this.panData.Location = new System.Drawing.Point(3, 27);
        this.panData.Name = "panData";
        this.panData.Size = new System.Drawing.Size(1664, 214);
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
        this.cboStatCd.LookupKey = "L_PR0001";
        this.cboStatCd.Name = "cboStatCd";
        this.cboStatCd.Properties.NullText = "";
        this.cboStatCd.Properties.ReadOnly = true;
        this.cboStatCd.Size = new System.Drawing.Size(150, 20);
        this.cboStatCd.TabIndex = 3;
        //
        // lblWoNo
        //
        this.lblWoNo.Location = new System.Drawing.Point(536, 15);
        this.lblWoNo.Name = "lblWoNo";
        this.lblWoNo.Size = new System.Drawing.Size(60, 15);
        this.lblWoNo.TabIndex = 4;
        this.lblWoNo.Text = "작업지시번호";
        //
        // txtWoNo
        //
        this.txtWoNo.Location = new System.Drawing.Point(624, 12);
        this.txtWoNo.Name = "txtWoNo";
        this.txtWoNo.Properties.ReadOnly = true;
        this.txtWoNo.Size = new System.Drawing.Size(150, 20);
        this.txtWoNo.TabIndex = 5;
        //
        // lblWoDate
        //
        this.lblWoDate.Location = new System.Drawing.Point(28, 43);
        this.lblWoDate.Name = "lblWoDate";
        this.lblWoDate.Size = new System.Drawing.Size(48, 15);
        this.lblWoDate.TabIndex = 6;
        this.lblWoDate.Text = "작업일자";
        //
        // dteWoDate
        //
        this.dteWoDate.Location = new System.Drawing.Point(100, 40);
        this.dteWoDate.Name = "dteWoDate";
        this.dteWoDate.Required = true;
        this.dteWoDate.Size = new System.Drawing.Size(150, 20);
        this.dteWoDate.TabIndex = 7;
        //
        // lblRouteId
        //
        this.lblRouteId.Location = new System.Drawing.Point(536, 43);
        this.lblRouteId.Name = "lblRouteId";
        this.lblRouteId.Size = new System.Drawing.Size(36, 15);
        this.lblRouteId.TabIndex = 8;
        this.lblRouteId.Text = "라우팅";
        //
        // cboRouteId
        //
        this.cboRouteId.Location = new System.Drawing.Point(624, 40);
        this.cboRouteId.LookupKey = "L_PRROUTE_ITEM";
        this.cboRouteId.Name = "cboRouteId";
        this.cboRouteId.Properties.NullText = "";
        this.cboRouteId.Required = true;
        this.cboRouteId.Size = new System.Drawing.Size(150, 20);
        this.cboRouteId.TabIndex = 9;
        //
        // lblDelvDate
        //
        this.lblDelvDate.Location = new System.Drawing.Point(536, 71);
        this.lblDelvDate.Name = "lblDelvDate";
        this.lblDelvDate.Size = new System.Drawing.Size(36, 15);
        this.lblDelvDate.TabIndex = 10;
        this.lblDelvDate.Text = "납기일";
        //
        // dteDelvDate
        //
        this.dteDelvDate.Location = new System.Drawing.Point(624, 68);
        this.dteDelvDate.Name = "dteDelvDate";
        this.dteDelvDate.Size = new System.Drawing.Size(150, 20);
        this.dteDelvDate.TabIndex = 11;
        //
        // lblStartLotNo
        //
        this.lblStartLotNo.Location = new System.Drawing.Point(28, 71);
        this.lblStartLotNo.Name = "lblStartLotNo";
        this.lblStartLotNo.Size = new System.Drawing.Size(48, 15);
        this.lblStartLotNo.TabIndex = 12;
        this.lblStartLotNo.Text = "시작 LOT";
        //
        // txtStartLotNo
        //
        this.txtStartLotNo.Location = new System.Drawing.Point(100, 68);
        this.txtStartLotNo.Name = "txtStartLotNo";
        this.txtStartLotNo.Properties.ReadOnly = true;
        this.txtStartLotNo.Required = true;
        this.txtStartLotNo.Size = new System.Drawing.Size(100, 20);
        this.txtStartLotNo.TabIndex = 13;
        //
        // txtStartLotId
        //
        this.txtStartLotId.Location = new System.Drawing.Point(1000, 68);
        this.txtStartLotId.Name = "txtStartLotId";
        this.txtStartLotId.Size = new System.Drawing.Size(60, 20);
        this.txtStartLotId.TabIndex = 60;
        this.txtStartLotId.Visible = false;
        //
        // btnPickLot
        //
        this.btnPickLot.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnPickLot.Location = new System.Drawing.Point(204, 66);
        this.btnPickLot.Name = "btnPickLot";
        this.btnPickLot.Size = new System.Drawing.Size(76, 24);
        this.btnPickLot.TabIndex = 61;
        this.btnPickLot.Text = "LOT 선택";
        this.btnPickLot.ToolTip = "웨이퍼입고에서 확정한, 아직 작업지시에 배정되지 않은 LOT를 골라 시작 LOT로 배정합니다.";
        //
        // lblStartQty
        //
        this.lblStartQty.Location = new System.Drawing.Point(282, 71);
        this.lblStartQty.Name = "lblStartQty";
        this.lblStartQty.Size = new System.Drawing.Size(60, 15);
        this.lblStartQty.TabIndex = 14;
        this.lblStartQty.Text = "시작수량(장)";
        //
        // spnStartQty
        //
        this.spnStartQty.Location = new System.Drawing.Point(354, 68);
        this.spnStartQty.Name = "spnStartQty";
        this.spnStartQty.Properties.ReadOnly = true;
        this.spnStartQty.Size = new System.Drawing.Size(150, 20);
        this.spnStartQty.TabIndex = 15;
        //
        // lblItemNm
        //
        this.lblItemNm.Location = new System.Drawing.Point(282, 43);
        this.lblItemNm.Name = "lblItemNm";
        this.lblItemNm.Size = new System.Drawing.Size(36, 15);
        this.lblItemNm.TabIndex = 16;
        this.lblItemNm.Text = "제품";
        //
        // txtItemNm
        //
        this.txtItemNm.Location = new System.Drawing.Point(354, 40);
        this.txtItemNm.LookupKey = "P_ITEM";
        this.txtItemNm.MatchField = "item_nm";
        this.txtItemNm.Name = "txtItemNm";
        this.txtItemNm.Required = true;
        this.txtItemNm.Size = new System.Drawing.Size(150, 20);
        this.txtItemNm.TabIndex = 17;
        //
        // txtItemId
        //
        this.txtItemId.Location = new System.Drawing.Point(1100, 40);
        this.txtItemId.Name = "txtItemId";
        this.txtItemId.Size = new System.Drawing.Size(60, 20);
        this.txtItemId.TabIndex = 62;
        this.txtItemId.Visible = false;
        //
        // lblSoNo
        //
        this.lblSoNo.Location = new System.Drawing.Point(28, 99);
        this.lblSoNo.Name = "lblSoNo";
        this.lblSoNo.Size = new System.Drawing.Size(48, 15);
        this.lblSoNo.TabIndex = 18;
        this.lblSoNo.Text = "수주번호";
        //
        // txtSoNo
        //
        this.txtSoNo.Location = new System.Drawing.Point(100, 96);
        this.txtSoNo.Name = "txtSoNo";
        this.txtSoNo.Properties.ReadOnly = true;
        this.txtSoNo.Size = new System.Drawing.Size(150, 20);
        this.txtSoNo.TabIndex = 19;
        //
        // txtSoId
        //
        this.txtSoId.Location = new System.Drawing.Point(1200, 96);
        this.txtSoId.Name = "txtSoId";
        this.txtSoId.Size = new System.Drawing.Size(60, 20);
        this.txtSoId.TabIndex = 20;
        this.txtSoId.Visible = false;
        //
        // txtSoSerl
        //
        this.txtSoSerl.Location = new System.Drawing.Point(1270, 96);
        this.txtSoSerl.Name = "txtSoSerl";
        this.txtSoSerl.Size = new System.Drawing.Size(60, 20);
        this.txtSoSerl.TabIndex = 21;
        this.txtSoSerl.Visible = false;
        //
        // btnPickSo
        //
        this.btnPickSo.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnPickSo.Location = new System.Drawing.Point(258, 94);
        this.btnPickSo.Name = "btnPickSo";
        this.btnPickSo.Size = new System.Drawing.Size(76, 24);
        this.btnPickSo.TabIndex = 22;
        this.btnPickSo.Text = "수주 선택";
        this.btnPickSo.ToolTip = "생산할 수주 품목을 골라 이 작업지시와 연결합니다(선택 사항).";
        //
        // btnClearSo
        //
        this.btnClearSo.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnClearSo.Location = new System.Drawing.Point(340, 94);
        this.btnClearSo.Name = "btnClearSo";
        this.btnClearSo.Size = new System.Drawing.Size(60, 24);
        this.btnClearSo.TabIndex = 23;
        this.btnClearSo.Text = "해제";
        this.btnClearSo.ToolTip = "수주 연결을 지웁니다.";
        //
        // lblDept
        //
        this.lblDept.Location = new System.Drawing.Point(28, 127);
        this.lblDept.Name = "lblDept";
        this.lblDept.Size = new System.Drawing.Size(24, 15);
        this.lblDept.TabIndex = 24;
        this.lblDept.Text = "부서";
        //
        // txtDeptNm
        //
        this.txtDeptNm.Location = new System.Drawing.Point(100, 124);
        this.txtDeptNm.LookupKey = "P_DEPT";
        this.txtDeptNm.MatchField = "dept_nm";
        this.txtDeptNm.Name = "txtDeptNm";
        this.txtDeptNm.Size = new System.Drawing.Size(150, 20);
        this.txtDeptNm.TabIndex = 25;
        //
        // txtDeptId
        //
        this.txtDeptId.Location = new System.Drawing.Point(1200, 124);
        this.txtDeptId.Name = "txtDeptId";
        this.txtDeptId.Size = new System.Drawing.Size(60, 20);
        this.txtDeptId.TabIndex = 26;
        this.txtDeptId.Visible = false;
        //
        // lblEmp
        //
        this.lblEmp.Location = new System.Drawing.Point(282, 127);
        this.lblEmp.Name = "lblEmp";
        this.lblEmp.Size = new System.Drawing.Size(36, 15);
        this.lblEmp.TabIndex = 27;
        this.lblEmp.Text = "담당자";
        //
        // txtEmpNm
        //
        this.txtEmpNm.Location = new System.Drawing.Point(354, 124);
        this.txtEmpNm.LookupKey = "P_EMP";
        this.txtEmpNm.MatchField = "emp_nm";
        this.txtEmpNm.Name = "txtEmpNm";
        this.txtEmpNm.Size = new System.Drawing.Size(150, 20);
        this.txtEmpNm.TabIndex = 28;
        //
        // txtEmpId
        //
        this.txtEmpId.Location = new System.Drawing.Point(1270, 124);
        this.txtEmpId.Name = "txtEmpId";
        this.txtEmpId.Size = new System.Drawing.Size(60, 20);
        this.txtEmpId.TabIndex = 29;
        this.txtEmpId.Visible = false;
        //
        // lblRemark
        //
        this.lblRemark.Location = new System.Drawing.Point(40, 155);
        this.lblRemark.Name = "lblRemark";
        this.lblRemark.Size = new System.Drawing.Size(24, 15);
        this.lblRemark.TabIndex = 30;
        this.lblRemark.Text = "비고";
        //
        // memoRemark
        //
        this.memoRemark.Location = new System.Drawing.Point(100, 152);
        this.memoRemark.Name = "memoRemark";
        this.memoRemark.Size = new System.Drawing.Size(674, 25);
        this.memoRemark.TabIndex = 31;
        //
        // lblHint
        //
        this.lblHint.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
        this.lblHint.Appearance.Options.UseForeColor = true;
        this.lblHint.Location = new System.Drawing.Point(28, 187);
        this.lblHint.Name = "lblHint";
        this.lblHint.Size = new System.Drawing.Size(60, 15);
        this.lblHint.TabIndex = 32;
        this.lblHint.Text = "※ 저장한 뒤 [확정]해야 공정실적/외주이전을 등록할 수 있습니다. 시작 LOT는 [웨이퍼입고] 화면에서 입고 확정한 LOT를 [LOT 선택]으로 고릅니다(재고가 있어야 실적 대기 LOT에 나옵니다).";
        //
        // btnConfirm
        //
        this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnConfirm.Location = new System.Drawing.Point(820, 12);
        this.btnConfirm.Name = "btnConfirm";
        this.btnConfirm.Size = new System.Drawing.Size(100, 24);
        this.btnConfirm.TabIndex = 37;
        this.btnConfirm.Text = "확정";
        this.btnConfirm.ToolTip = "작업지시를 확정합니다. 확정해야 공정실적/외주이전을 등록할 수 있고 실적 대기 LOT 불러오기에 나옵니다.";
        //
        // btnConfirmCancel
        //
        this.btnConfirmCancel.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnConfirmCancel.Location = new System.Drawing.Point(820, 40);
        this.btnConfirmCancel.Name = "btnConfirmCancel";
        this.btnConfirmCancel.Size = new System.Drawing.Size(100, 24);
        this.btnConfirmCancel.TabIndex = 38;
        this.btnConfirmCancel.Text = "확정취소";
        this.btnConfirmCancel.ToolTip = "확정을 취소하고 계획 상태로 되돌립니다(실적/이전이 시작되기 전에만).";
        //
        // btnStop
        //
        this.btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnStop.Location = new System.Drawing.Point(928, 12);
        this.btnStop.Name = "btnStop";
        this.btnStop.Size = new System.Drawing.Size(100, 24);
        this.btnStop.TabIndex = 33;
        this.btnStop.Text = "중단";
        this.btnStop.ToolTip = "작업지시를 중단합니다(실적/이전 등록 불가). 중단해제로 되돌릴 수 있습니다.";
        //
        // btnStopCancel
        //
        this.btnStopCancel.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnStopCancel.Location = new System.Drawing.Point(928, 40);
        this.btnStopCancel.Name = "btnStopCancel";
        this.btnStopCancel.Size = new System.Drawing.Size(100, 24);
        this.btnStopCancel.TabIndex = 34;
        this.btnStopCancel.Text = "중단해제";
        //
        // btnComplete
        //
        this.btnComplete.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnComplete.Location = new System.Drawing.Point(1036, 12);
        this.btnComplete.Name = "btnComplete";
        this.btnComplete.Size = new System.Drawing.Size(100, 24);
        this.btnComplete.TabIndex = 35;
        this.btnComplete.Text = "완료";
        this.btnComplete.ToolTip = "생산을 마칩니다. 끝나지 않은 외주 이전이 있으면 완료할 수 없습니다.";
        //
        // btnCompleteCancel
        //
        this.btnCompleteCancel.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnCompleteCancel.Location = new System.Drawing.Point(1036, 40);
        this.btnCompleteCancel.Name = "btnCompleteCancel";
        this.btnCompleteCancel.Size = new System.Drawing.Size(100, 24);
        this.btnCompleteCancel.TabIndex = 36;
        this.btnCompleteCancel.Text = "완료취소";
        //
        // shProc
        //
        this.shProc.BackColor = System.Drawing.Color.White;
        this.shProc.Dock = System.Windows.Forms.DockStyle.Top;
        this.shProc.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shProc.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.shProc.Location = new System.Drawing.Point(3, 241);
        this.shProc.Name = "shProc";
        this.shProc.Size = new System.Drawing.Size(1664, 27);
        this.shProc.TabIndex = 2;
        this.shProc.Text = "공정별 외주 진행 (외주처/창고/단가/납기 수정 가능)";
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Top;
        this.grd1.Location = new System.Drawing.Point(3, 268);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.popcolCust,
        this.popcolWh,
        this.datecolDue,
        this.lookupcolStat});
        this.grd1.Size = new System.Drawing.Size(1664, 190);
        this.grd1.TabIndex = 3;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colSerl,
        this.colProcNm,
        this.colCustId,
        this.colWhId,
        this.colInItemNm,
        this.colOutItemNm,
        this.colSplitQty,
        this.colPriceUnit,
        this.colPrice,
        this.colDueDate,
        this.colStatCd,
        this.colInQty,
        this.colGoodQty,
        this.colBadQty,
        this.colYield,
        this.colRemark});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        //
        // popcolCust
        //
        this.popcolCust.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "...")});
        this.popcolCust.LookupKey = "P_CUST";
        this.popcolCust.PopupConditions = "p_cust_class=OS";
        this.popcolCust.Name = "popcolCust";
        //
        // popcolWh
        //
        this.popcolWh.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "...")});
        this.popcolWh.LookupKey = "P_WH";
        this.popcolWh.Name = "popcolWh";
        //
        // datecolDue
        //
        this.datecolDue.AutoHeight = false;
        this.datecolDue.Name = "datecolDue";
        //
        // lookupcolStat
        //
        this.lookupcolStat.AutoHeight = false;
        this.lookupcolStat.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolStat.LookupKey = "L_PR0002";
        this.lookupcolStat.Name = "lookupcolStat";
        this.lookupcolStat.NullText = "";
        //
        // colSerl
        //
        this.colSerl.Caption = "순번";
        this.colSerl.FieldName = "serl";
        this.colSerl.Name = "colSerl";
        this.colSerl.OptionsColumn.AllowEdit = false;
        this.colSerl.Visible = true;
        this.colSerl.VisibleIndex = 0;
        this.colSerl.Width = 45;
        //
        // colProcNm
        //
        this.colProcNm.Caption = "공정";
        this.colProcNm.FieldName = "proc_nm";
        this.colProcNm.Name = "colProcNm";
        this.colProcNm.OptionsColumn.AllowEdit = false;
        this.colProcNm.Visible = true;
        this.colProcNm.VisibleIndex = 1;
        this.colProcNm.Width = 90;
        //
        // colCustId
        //
        this.colCustId.Caption = "외주처";
        this.colCustId.ColumnEdit = this.popcolCust;
        this.colCustId.FieldName = "cust_id";
        this.colCustId.Name = "colCustId";
        this.colCustId.Visible = true;
        this.colCustId.VisibleIndex = 2;
        this.colCustId.Width = 110;
        //
        // colWhId
        //
        this.colWhId.Caption = "외주처 창고";
        this.colWhId.ColumnEdit = this.popcolWh;
        this.colWhId.FieldName = "wh_id";
        this.colWhId.Name = "colWhId";
        this.colWhId.Visible = true;
        this.colWhId.VisibleIndex = 3;
        this.colWhId.Width = 110;
        //
        // colInItemNm
        //
        this.colInItemNm.Caption = "투입품목";
        this.colInItemNm.FieldName = "in_item_nm";
        this.colInItemNm.Name = "colInItemNm";
        this.colInItemNm.OptionsColumn.AllowEdit = false;
        this.colInItemNm.Visible = true;
        this.colInItemNm.VisibleIndex = 4;
        this.colInItemNm.Width = 110;
        //
        // colOutItemNm
        //
        this.colOutItemNm.Caption = "산출품목";
        this.colOutItemNm.FieldName = "out_item_nm";
        this.colOutItemNm.Name = "colOutItemNm";
        this.colOutItemNm.OptionsColumn.AllowEdit = false;
        this.colOutItemNm.Visible = true;
        this.colOutItemNm.VisibleIndex = 5;
        this.colOutItemNm.Width = 110;
        //
        // colSplitQty
        //
        this.colSplitQty.Caption = "분할수량";
        this.colSplitQty.DisplayFormat.FormatString = "#,##0.####";
        this.colSplitQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colSplitQty.FieldName = "split_qty";
        this.colSplitQty.Name = "colSplitQty";
        this.colSplitQty.Visible = true;
        this.colSplitQty.VisibleIndex = 6;
        this.colSplitQty.Width = 75;
        //
        // colPriceUnit
        //
        this.colPriceUnit.Caption = "정산단위";
        this.colPriceUnit.FieldName = "price_unit_cd";
        this.colPriceUnit.Name = "colPriceUnit";
        this.colPriceUnit.Visible = true;
        this.colPriceUnit.VisibleIndex = 7;
        this.colPriceUnit.Width = 65;
        //
        // colPrice
        //
        this.colPrice.Caption = "가공단가";
        this.colPrice.DisplayFormat.FormatString = "#,##0.####";
        this.colPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colPrice.FieldName = "price";
        this.colPrice.Name = "colPrice";
        this.colPrice.Visible = true;
        this.colPrice.VisibleIndex = 8;
        this.colPrice.Width = 85;
        //
        // colDueDate
        //
        this.colDueDate.Caption = "납기";
        this.colDueDate.ColumnEdit = this.datecolDue;
        this.colDueDate.FieldName = "due_date";
        this.colDueDate.Name = "colDueDate";
        this.colDueDate.Visible = true;
        this.colDueDate.VisibleIndex = 9;
        this.colDueDate.Width = 90;
        //
        // colStatCd
        //
        this.colStatCd.Caption = "상태";
        this.colStatCd.ColumnEdit = this.lookupcolStat;
        this.colStatCd.FieldName = "stat_cd";
        this.colStatCd.Name = "colStatCd";
        this.colStatCd.OptionsColumn.AllowEdit = false;
        this.colStatCd.Visible = true;
        this.colStatCd.VisibleIndex = 10;
        this.colStatCd.Width = 60;
        //
        // colInQty
        //
        this.colInQty.Caption = "투입 누계";
        this.colInQty.DisplayFormat.FormatString = "#,##0.####";
        this.colInQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colInQty.FieldName = "in_qty";
        this.colInQty.Name = "colInQty";
        this.colInQty.OptionsColumn.AllowEdit = false;
        this.colInQty.Visible = true;
        this.colInQty.VisibleIndex = 11;
        this.colInQty.Width = 85;
        //
        // colGoodQty
        //
        this.colGoodQty.Caption = "양품 누계";
        this.colGoodQty.DisplayFormat.FormatString = "#,##0.####";
        this.colGoodQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colGoodQty.FieldName = "good_qty";
        this.colGoodQty.Name = "colGoodQty";
        this.colGoodQty.OptionsColumn.AllowEdit = false;
        this.colGoodQty.Visible = true;
        this.colGoodQty.VisibleIndex = 12;
        this.colGoodQty.Width = 85;
        //
        // colBadQty
        //
        this.colBadQty.Caption = "불량 누계";
        this.colBadQty.DisplayFormat.FormatString = "#,##0.####";
        this.colBadQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colBadQty.FieldName = "bad_qty";
        this.colBadQty.Name = "colBadQty";
        this.colBadQty.OptionsColumn.AllowEdit = false;
        this.colBadQty.Visible = true;
        this.colBadQty.VisibleIndex = 13;
        this.colBadQty.Width = 85;
        //
        // colYield
        //
        this.colYield.Caption = "수율(%)";
        this.colYield.DisplayFormat.FormatString = "#,##0.##";
        this.colYield.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colYield.FieldName = "yield_rate";
        this.colYield.Name = "colYield";
        this.colYield.OptionsColumn.AllowEdit = false;
        this.colYield.Visible = true;
        this.colYield.VisibleIndex = 14;
        this.colYield.Width = 65;
        //
        // colRemark
        //
        this.colRemark.Caption = "비고";
        this.colRemark.FieldName = "remark";
        this.colRemark.Name = "colRemark";
        this.colRemark.Visible = true;
        this.colRemark.VisibleIndex = 15;
        this.colRemark.Width = 160;
        //
        // panMat
        //
        this.panMat.Controls.Add(this.grd3);
        this.panMat.Controls.Add(this.shMat);
        this.panMat.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panMat.Location = new System.Drawing.Point(3, 566);
        this.panMat.Name = "panMat";
        this.panMat.Size = new System.Drawing.Size(1664, 190);
        this.panMat.TabIndex = 8;
        //
        // shMat
        //
        this.shMat.BackColor = System.Drawing.Color.White;
        this.shMat.Dock = System.Windows.Forms.DockStyle.Top;
        this.shMat.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shMat.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.shMat.Location = new System.Drawing.Point(0, 0);
        this.shMat.Name = "shMat";
        this.shMat.Size = new System.Drawing.Size(1664, 27);
        this.shMat.TabIndex = 0;
        this.shMat.Text = "자재소요 (BOM 복사본 - 주원료/원자재/부자재/소모품)";
        //
        // grd3
        //
        this.grd3.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd3.Location = new System.Drawing.Point(0, 27);
        this.grd3.MainView = this.gvw3;
        this.grd3.Name = "grd3";
        this.grd3.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.lookupcolMatType});
        this.grd3.Size = new System.Drawing.Size(1664, 163);
        this.grd3.TabIndex = 1;
        this.grd3.UseEmbeddedNavigator = false;
        this.grd3.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw3});
        //
        // gvw3
        //
        this.gvw3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colMatProc,
        this.colMatType,
        this.colMatItemNo,
        this.colMatItemNm,
        this.colMatUnit,
        this.colMatQtyPer,
        this.colMatLoss,
        this.colMatReq});
        this.gvw3.GridControl = this.grd3;
        this.gvw3.HighlightFocusedRow = true;
        this.gvw3.Name = "gvw3";
        this.gvw3.OptionsBehavior.Editable = false;
        this.gvw3.OptionsView.ColumnAutoWidth = false;
        this.gvw3.OptionsView.ShowGroupPanel = false;
        //
        // lookupcolMatType
        //
        this.lookupcolMatType.AutoHeight = false;
        this.lookupcolMatType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolMatType.LookupKey = "L_PR0009";
        this.lookupcolMatType.Name = "lookupcolMatType";
        this.lookupcolMatType.NullText = "";
        //
        // colMatProc
        //
        this.colMatProc.Caption = "공정";
        this.colMatProc.FieldName = "proc_nm";
        this.colMatProc.Name = "colMatProc";
        this.colMatProc.OptionsColumn.AllowEdit = false;
        this.colMatProc.Visible = true;
        this.colMatProc.VisibleIndex = 0;
        this.colMatProc.Width = 110;
        //
        // colMatType
        //
        this.colMatType.Caption = "구분";
        this.colMatType.ColumnEdit = this.lookupcolMatType;
        this.colMatType.FieldName = "comp_type";
        this.colMatType.Name = "colMatType";
        this.colMatType.OptionsColumn.AllowEdit = false;
        this.colMatType.Visible = true;
        this.colMatType.VisibleIndex = 1;
        this.colMatType.Width = 80;
        //
        // colMatItemNo
        //
        this.colMatItemNo.Caption = "품번";
        this.colMatItemNo.FieldName = "item_no";
        this.colMatItemNo.Name = "colMatItemNo";
        this.colMatItemNo.OptionsColumn.AllowEdit = false;
        this.colMatItemNo.Visible = true;
        this.colMatItemNo.VisibleIndex = 2;
        this.colMatItemNo.Width = 110;
        //
        // colMatItemNm
        //
        this.colMatItemNm.Caption = "품명";
        this.colMatItemNm.FieldName = "item_nm";
        this.colMatItemNm.Name = "colMatItemNm";
        this.colMatItemNm.OptionsColumn.AllowEdit = false;
        this.colMatItemNm.Visible = true;
        this.colMatItemNm.VisibleIndex = 3;
        this.colMatItemNm.Width = 160;
        //
        // colMatUnit
        //
        this.colMatUnit.Caption = "단위";
        this.colMatUnit.FieldName = "unit_cd";
        this.colMatUnit.Name = "colMatUnit";
        this.colMatUnit.OptionsColumn.AllowEdit = false;
        this.colMatUnit.Visible = true;
        this.colMatUnit.VisibleIndex = 4;
        this.colMatUnit.Width = 60;
        //
        // colMatQtyPer
        //
        this.colMatQtyPer.Caption = "소요수량(단위당)";
        this.colMatQtyPer.DisplayFormat.FormatString = "#,##0.######";
        this.colMatQtyPer.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colMatQtyPer.FieldName = "qty_per";
        this.colMatQtyPer.Name = "colMatQtyPer";
        this.colMatQtyPer.OptionsColumn.AllowEdit = false;
        this.colMatQtyPer.Visible = true;
        this.colMatQtyPer.VisibleIndex = 5;
        this.colMatQtyPer.Width = 110;
        //
        // colMatLoss
        //
        this.colMatLoss.Caption = "손실율(%)";
        this.colMatLoss.DisplayFormat.FormatString = "#,##0.######";
        this.colMatLoss.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colMatLoss.FieldName = "loss_rate";
        this.colMatLoss.Name = "colMatLoss";
        this.colMatLoss.OptionsColumn.AllowEdit = false;
        this.colMatLoss.Visible = true;
        this.colMatLoss.VisibleIndex = 6;
        this.colMatLoss.Width = 80;
        //
        // colMatReq
        //
        this.colMatReq.Caption = "소요수량(예상)";
        this.colMatReq.DisplayFormat.FormatString = "#,##0.######";
        this.colMatReq.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colMatReq.FieldName = "req_qty";
        this.colMatReq.Name = "colMatReq";
        this.colMatReq.OptionsColumn.AllowEdit = false;
        this.colMatReq.Visible = true;
        this.colMatReq.VisibleIndex = 7;
        this.colMatReq.Width = 110;
        //
        // splitterWyn1
        //
        this.splitterWyn1.BackColor = System.Drawing.Color.White;
        this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Top;
        this.splitterWyn1.Location = new System.Drawing.Point(3, 458);
        this.splitterWyn1.Name = "splitterWyn1";
        this.splitterWyn1.Size = new System.Drawing.Size(1664, 10);
        this.splitterWyn1.TabIndex = 7;
        this.splitterWyn1.TabStop = false;
        //
        // shLot
        //
        this.shLot.BackColor = System.Drawing.Color.White;
        this.shLot.Dock = System.Windows.Forms.DockStyle.Top;
        this.shLot.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shLot.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.shLot.Location = new System.Drawing.Point(3, 468);
        this.shLot.Name = "shLot";
        this.shLot.Size = new System.Drawing.Size(1664, 27);
        this.shLot.TabIndex = 4;
        this.shLot.Text = "LOT 현황 (현재 재고 위치)";
        //
        // grd2
        //
        this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd2.Location = new System.Drawing.Point(3, 495);
        this.grd2.MainView = this.gvw2;
        this.grd2.Name = "grd2";
        this.grd2.Size = new System.Drawing.Size(1664, 261);
        this.grd2.TabIndex = 5;
        this.grd2.UseEmbeddedNavigator = false;
        this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw2});
        //
        // gvw2
        //
        this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colLotSerl,
        this.colLotNo,
        this.colLotItemNo,
        this.colLotItemNm,
        this.colLotUnit,
        this.colLotInit,
        this.colLotWhNm,
        this.colLotStock});
        this.gvw2.GridControl = this.grd2;
        this.gvw2.HighlightFocusedRow = true;
        this.gvw2.Name = "gvw2";
        this.gvw2.OptionsBehavior.Editable = false;
        this.gvw2.OptionsView.ColumnAutoWidth = false;
        this.gvw2.OptionsView.ShowGroupPanel = false;
        //
        // colLotSerl
        //
        this.colLotSerl.Caption = "생성 공정";
        this.colLotSerl.FieldName = "wo_serl";
        this.colLotSerl.Name = "colLotSerl";
        this.colLotSerl.Visible = true;
        this.colLotSerl.VisibleIndex = 0;
        this.colLotSerl.Width = 70;
        //
        // colLotNo
        //
        this.colLotNo.Caption = "LOT";
        this.colLotNo.FieldName = "lot_no";
        this.colLotNo.Name = "colLotNo";
        this.colLotNo.Visible = true;
        this.colLotNo.VisibleIndex = 1;
        this.colLotNo.Width = 130;
        //
        // colLotItemNo
        //
        this.colLotItemNo.Caption = "품번";
        this.colLotItemNo.FieldName = "item_no";
        this.colLotItemNo.Name = "colLotItemNo";
        this.colLotItemNo.Visible = true;
        this.colLotItemNo.VisibleIndex = 2;
        this.colLotItemNo.Width = 100;
        //
        // colLotItemNm
        //
        this.colLotItemNm.Caption = "품명";
        this.colLotItemNm.FieldName = "item_nm";
        this.colLotItemNm.Name = "colLotItemNm";
        this.colLotItemNm.Visible = true;
        this.colLotItemNm.VisibleIndex = 3;
        this.colLotItemNm.Width = 140;
        //
        // colLotUnit
        //
        this.colLotUnit.Caption = "단위";
        this.colLotUnit.FieldName = "unit_cd";
        this.colLotUnit.Name = "colLotUnit";
        this.colLotUnit.Visible = true;
        this.colLotUnit.VisibleIndex = 4;
        this.colLotUnit.Width = 50;
        //
        // colLotInit
        //
        this.colLotInit.Caption = "생성 수량";
        this.colLotInit.DisplayFormat.FormatString = "#,##0.####";
        this.colLotInit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colLotInit.FieldName = "init_qty";
        this.colLotInit.Name = "colLotInit";
        this.colLotInit.Visible = true;
        this.colLotInit.VisibleIndex = 5;
        this.colLotInit.Width = 90;
        //
        // colLotWhNm
        //
        this.colLotWhNm.Caption = "현재 위치(창고)";
        this.colLotWhNm.FieldName = "wh_nm";
        this.colLotWhNm.Name = "colLotWhNm";
        this.colLotWhNm.Visible = true;
        this.colLotWhNm.VisibleIndex = 6;
        this.colLotWhNm.Width = 130;
        //
        // colLotStock
        //
        this.colLotStock.Caption = "현재고";
        this.colLotStock.DisplayFormat.FormatString = "#,##0.####";
        this.colLotStock.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colLotStock.FieldName = "stock_qty";
        this.colLotStock.Name = "colLotStock";
        this.colLotStock.Visible = true;
        this.colLotStock.VisibleIndex = 7;
        this.colLotStock.Width = 90;
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblSearchAccId);
        this.panHeader.Controls.Add(this.cboSearchAccId);
        this.panHeader.Controls.Add(this.lblSearchWoNo);
        this.panHeader.Controls.Add(this.txtSearchWoNo);
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
        // lblSearchWoNo
        //
        this.lblSearchWoNo.Location = new System.Drawing.Point(224, 18);
        this.lblSearchWoNo.Name = "lblSearchWoNo";
        this.lblSearchWoNo.Size = new System.Drawing.Size(60, 15);
        this.lblSearchWoNo.TabIndex = 0;
        this.lblSearchWoNo.Text = "작업지시번호";
        //
        // txtSearchWoNo
        //
        this.txtSearchWoNo.Location = new System.Drawing.Point(309, 15);
        this.txtSearchWoNo.Name = "txtSearchWoNo";
        this.txtSearchWoNo.Size = new System.Drawing.Size(180, 20);
        this.txtSearchWoNo.TabIndex = 1;
        //
        // frmWo
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 800);
        this.Controls.Add(this.panBase);
        this.Name = "frmWo";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).EndInit();
        this.panWork.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panMat)).EndInit();
        this.panMat.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolMatType)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolCust)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.popcolWh)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.datecolDue)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolStat)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
        this.panData.ResumeLayout(false);
        this.panData.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWoNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteWoDate.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteWoDate.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboRouteId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteDelvDate.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteDelvDate.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtStartLotNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtStartLotId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.spnStartQty.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtItemNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtItemId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSoNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSoId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSoSerl.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        this.panHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchWoNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panWork;
    private GridControlWyn grd2;
    private PanelWyn panMat;
    private SectionHeaderWyn shMat;
    private GridControlWyn grd3;
    private GridViewWyn gvw3;
    private LookUpColumnEdit lookupcolMatType;
    private DevExpress.XtraGrid.Columns.GridColumn colMatProc;
    private DevExpress.XtraGrid.Columns.GridColumn colMatType;
    private DevExpress.XtraGrid.Columns.GridColumn colMatItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colMatItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colMatUnit;
    private DevExpress.XtraGrid.Columns.GridColumn colMatQtyPer;
    private DevExpress.XtraGrid.Columns.GridColumn colMatLoss;
    private DevExpress.XtraGrid.Columns.GridColumn colMatReq;
    private GridViewWyn gvw2;
    private DevExpress.XtraGrid.Columns.GridColumn colLotSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colLotNo;
    private DevExpress.XtraGrid.Columns.GridColumn colLotItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colLotItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colLotUnit;
    private DevExpress.XtraGrid.Columns.GridColumn colLotInit;
    private DevExpress.XtraGrid.Columns.GridColumn colLotWhNm;
    private DevExpress.XtraGrid.Columns.GridColumn colLotStock;
    private SectionHeaderWyn shLot;
    private GridControlWyn grd1;
    private SplitterWyn splitterWyn1;
    private GridViewWyn gvw1;
    private PopupLookupColumnEdit popcolCust;
    private PopupLookupColumnEdit popcolWh;
    private DateColumnEdit datecolDue;
    private LookUpColumnEdit lookupcolStat;
    private DevExpress.XtraGrid.Columns.GridColumn colSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colProcNm;
    private DevExpress.XtraGrid.Columns.GridColumn colCustId;
    private DevExpress.XtraGrid.Columns.GridColumn colWhId;
    private DevExpress.XtraGrid.Columns.GridColumn colInItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colOutItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colSplitQty;
    private DevExpress.XtraGrid.Columns.GridColumn colPriceUnit;
    private DevExpress.XtraGrid.Columns.GridColumn colPrice;
    private DevExpress.XtraGrid.Columns.GridColumn colDueDate;
    private DevExpress.XtraGrid.Columns.GridColumn colStatCd;
    private DevExpress.XtraGrid.Columns.GridColumn colInQty;
    private DevExpress.XtraGrid.Columns.GridColumn colGoodQty;
    private DevExpress.XtraGrid.Columns.GridColumn colBadQty;
    private DevExpress.XtraGrid.Columns.GridColumn colYield;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private SectionHeaderWyn shProc;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblAccId;
    private LookUpEditWyn cboAccId;
    private DevExpress.XtraEditors.LabelControl lblStatCd;
    private LookUpEditWyn cboStatCd;
    private DevExpress.XtraEditors.LabelControl lblWoNo;
    private TextEditWyn txtWoNo;
    private DevExpress.XtraEditors.LabelControl lblWoDate;
    private DateEditWyn dteWoDate;
    private DevExpress.XtraEditors.LabelControl lblRouteId;
    private LookUpEditWyn cboRouteId;
    private DevExpress.XtraEditors.LabelControl lblDelvDate;
    private DateEditWyn dteDelvDate;
    private DevExpress.XtraEditors.LabelControl lblStartLotNo;
    private TextEditWyn txtStartLotNo;
    private TextEditWyn txtStartLotId;
    private ButtonWyn btnPickLot;
    private DevExpress.XtraEditors.LabelControl lblStartQty;
    private SpinEditWyn spnStartQty;
    private DevExpress.XtraEditors.LabelControl lblItemNm;
    private PopupLookupEditWyn txtItemNm;
    private TextEditWyn txtItemId;
    private DevExpress.XtraEditors.LabelControl lblSoNo;
    private TextEditWyn txtSoNo;
    private TextEditWyn txtSoId;
    private TextEditWyn txtSoSerl;
    private ButtonWyn btnPickSo;
    private ButtonWyn btnClearSo;
    private DevExpress.XtraEditors.LabelControl lblDept;
    private PopupLookupEditWyn txtDeptNm;
    private TextEditWyn txtDeptId;
    private DevExpress.XtraEditors.LabelControl lblEmp;
    private PopupLookupEditWyn txtEmpNm;
    private TextEditWyn txtEmpId;
    private DevExpress.XtraEditors.LabelControl lblRemark;
    private DevExpress.XtraEditors.MemoEdit memoRemark;
    private DevExpress.XtraEditors.LabelControl lblHint;
    private ButtonWyn btnConfirm;
    private ButtonWyn btnConfirmCancel;
    private ButtonWyn btnStop;
    private ButtonWyn btnStopCancel;
    private ButtonWyn btnComplete;
    private ButtonWyn btnCompleteCancel;
    private SectionHeaderWyn shHeader;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchWoNo;
    private TextEditWyn txtSearchWoNo;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private WYNLAB.Base.Controls.LookUpEditWyn cboSearchAccId;
}
