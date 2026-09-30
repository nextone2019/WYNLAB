// 외주이전(frmXfer) - Master-One Sheet 구조(TPRXFERM/TPRXFERD). 헤더(작업지시/출발-도착 공정) + LOT 라인 그리드(출발/도착 수량, 차이 처리).
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

public partial class frmXfer
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
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.lookupcolResp = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.lookupcolAct = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.colSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLotNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colUnit = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colOutQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colInQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDiffQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDiffResp = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDiffAct = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDiffDt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.panTool = new WYNLAB.Base.Controls.PanelWyn();
        this.btnDeletRow1 = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnDiff = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnDiffCancel = new WYNLAB.Base.Controls.ButtonWyn();
        this.shLine = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panData = new WYNLAB.Base.Controls.PanelWyn();
        this.lblAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblStatCd = new DevExpress.XtraEditors.LabelControl();
        this.cboStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblXferNo = new DevExpress.XtraEditors.LabelControl();
        this.txtXferNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblXferDate = new DevExpress.XtraEditors.LabelControl();
        this.dteXferDate = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblKind = new DevExpress.XtraEditors.LabelControl();
        this.cboKind = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblWoNo = new DevExpress.XtraEditors.LabelControl();
        this.txtWoNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblFrom = new DevExpress.XtraEditors.LabelControl();
        this.txtFrom = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblTo = new DevExpress.XtraEditors.LabelControl();
        this.txtTo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblOutDt = new DevExpress.XtraEditors.LabelControl();
        this.txtOutDt = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblInDt = new DevExpress.XtraEditors.LabelControl();
        this.txtInDt = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblRemark = new DevExpress.XtraEditors.LabelControl();
        this.memoRemark = new DevExpress.XtraEditors.MemoEdit();
        this.txtWoId = new WYNLAB.Base.Controls.TextEditWyn();
        this.txtFromSerl = new WYNLAB.Base.Controls.TextEditWyn();
        this.txtToSerl = new WYNLAB.Base.Controls.TextEditWyn();
        this.btnLoadLot = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnOut = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnOutCancel = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnIn = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnInCancel = new WYNLAB.Base.Controls.ButtonWyn();
        this.shHeader = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.lblSearchXferNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchXferNo = new WYNLAB.Base.Controls.TextEditWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        this.panBase.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).BeginInit();
        this.panWork.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolResp)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolAct)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panTool)).BeginInit();
        this.panTool.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
        this.panData.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtXferNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteXferDate.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteXferDate.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboKind.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWoNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtFrom.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtTo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtOutDt.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtInDt.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWoId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtFromSerl.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtToSerl.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchXferNo.Properties)).BeginInit();
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
        this.panWork.Controls.Add(this.grd1);
        this.panWork.Controls.Add(this.panTool);
        this.panWork.Controls.Add(this.shLine);
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
        this.shHeader.Text = "외주 이전 정보등록";
        //
        // panData
        //
        this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        this.panData.Controls.Add(this.lblAccId);
        this.panData.Controls.Add(this.cboAccId);
        this.panData.Controls.Add(this.lblStatCd);
        this.panData.Controls.Add(this.cboStatCd);
        this.panData.Controls.Add(this.lblXferNo);
        this.panData.Controls.Add(this.txtXferNo);
        this.panData.Controls.Add(this.lblXferDate);
        this.panData.Controls.Add(this.dteXferDate);
        this.panData.Controls.Add(this.lblKind);
        this.panData.Controls.Add(this.cboKind);
        this.panData.Controls.Add(this.lblWoNo);
        this.panData.Controls.Add(this.txtWoNo);
        this.panData.Controls.Add(this.lblFrom);
        this.panData.Controls.Add(this.txtFrom);
        this.panData.Controls.Add(this.lblTo);
        this.panData.Controls.Add(this.txtTo);
        this.panData.Controls.Add(this.lblOutDt);
        this.panData.Controls.Add(this.txtOutDt);
        this.panData.Controls.Add(this.lblInDt);
        this.panData.Controls.Add(this.txtInDt);
        this.panData.Controls.Add(this.lblRemark);
        this.panData.Controls.Add(this.memoRemark);
        this.panData.Controls.Add(this.txtWoId);
        this.panData.Controls.Add(this.txtFromSerl);
        this.panData.Controls.Add(this.txtToSerl);
        this.panData.Controls.Add(this.btnLoadLot);
        this.panData.Controls.Add(this.btnOut);
        this.panData.Controls.Add(this.btnOutCancel);
        this.panData.Controls.Add(this.btnIn);
        this.panData.Controls.Add(this.btnInCancel);
        this.panData.Dock = System.Windows.Forms.DockStyle.Top;
        this.panData.Location = new System.Drawing.Point(3, 27);
        this.panData.Name = "panData";
        this.panData.Size = new System.Drawing.Size(1664, 192);
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
        this.cboStatCd.LookupKey = "L_PR0003";
        this.cboStatCd.Name = "cboStatCd";
        this.cboStatCd.Properties.NullText = "";
        this.cboStatCd.Properties.ReadOnly = true;
        this.cboStatCd.Size = new System.Drawing.Size(150, 20);
        this.cboStatCd.TabIndex = 3;
        //
        // lblXferNo
        //
        this.lblXferNo.Location = new System.Drawing.Point(536, 15);
        this.lblXferNo.Name = "lblXferNo";
        this.lblXferNo.Size = new System.Drawing.Size(48, 15);
        this.lblXferNo.TabIndex = 4;
        this.lblXferNo.Text = "이전번호";
        //
        // txtXferNo
        //
        this.txtXferNo.Location = new System.Drawing.Point(624, 12);
        this.txtXferNo.Name = "txtXferNo";
        this.txtXferNo.Properties.ReadOnly = true;
        this.txtXferNo.Size = new System.Drawing.Size(150, 20);
        this.txtXferNo.TabIndex = 5;
        //
        // lblXferDate
        //
        this.lblXferDate.Location = new System.Drawing.Point(28, 43);
        this.lblXferDate.Name = "lblXferDate";
        this.lblXferDate.Size = new System.Drawing.Size(48, 15);
        this.lblXferDate.TabIndex = 6;
        this.lblXferDate.Text = "이전일자";
        //
        // dteXferDate
        //
        this.dteXferDate.Location = new System.Drawing.Point(100, 40);
        this.dteXferDate.Name = "dteXferDate";
        this.dteXferDate.Required = true;
        this.dteXferDate.Size = new System.Drawing.Size(150, 20);
        this.dteXferDate.TabIndex = 7;
        //
        // lblKind
        //
        this.lblKind.Location = new System.Drawing.Point(282, 43);
        this.lblKind.Name = "lblKind";
        this.lblKind.Size = new System.Drawing.Size(48, 15);
        this.lblKind.TabIndex = 8;
        this.lblKind.Text = "이전구분";
        //
        // cboKind
        //
        this.cboKind.Location = new System.Drawing.Point(354, 40);
        this.cboKind.LookupKey = "L_PR0008";
        this.cboKind.Name = "cboKind";
        this.cboKind.Properties.NullText = "";
        this.cboKind.Properties.ReadOnly = true;
        this.cboKind.Size = new System.Drawing.Size(150, 20);
        this.cboKind.TabIndex = 9;
        //
        // lblWoNo
        //
        this.lblWoNo.Location = new System.Drawing.Point(536, 43);
        this.lblWoNo.Name = "lblWoNo";
        this.lblWoNo.Size = new System.Drawing.Size(60, 15);
        this.lblWoNo.TabIndex = 10;
        this.lblWoNo.Text = "작업지시번호";
        //
        // txtWoNo
        //
        this.txtWoNo.Location = new System.Drawing.Point(624, 40);
        this.txtWoNo.Name = "txtWoNo";
        this.txtWoNo.Properties.ReadOnly = true;
        this.txtWoNo.Size = new System.Drawing.Size(150, 20);
        this.txtWoNo.TabIndex = 11;
        //
        // lblFrom
        //
        this.lblFrom.Location = new System.Drawing.Point(28, 71);
        this.lblFrom.Name = "lblFrom";
        this.lblFrom.Size = new System.Drawing.Size(48, 15);
        this.lblFrom.TabIndex = 12;
        this.lblFrom.Text = "출발 공정";
        //
        // txtFrom
        //
        this.txtFrom.Location = new System.Drawing.Point(100, 68);
        this.txtFrom.Name = "txtFrom";
        this.txtFrom.Properties.ReadOnly = true;
        this.txtFrom.Size = new System.Drawing.Size(250, 20);
        this.txtFrom.TabIndex = 13;
        //
        // lblTo
        //
        this.lblTo.Location = new System.Drawing.Point(376, 71);
        this.lblTo.Name = "lblTo";
        this.lblTo.Size = new System.Drawing.Size(48, 15);
        this.lblTo.TabIndex = 14;
        this.lblTo.Text = "도착 공정";
        //
        // txtTo
        //
        this.txtTo.Location = new System.Drawing.Point(448, 68);
        this.txtTo.Name = "txtTo";
        this.txtTo.Properties.ReadOnly = true;
        this.txtTo.Size = new System.Drawing.Size(250, 20);
        this.txtTo.TabIndex = 15;
        //
        // lblOutDt
        //
        this.lblOutDt.Location = new System.Drawing.Point(28, 99);
        this.lblOutDt.Name = "lblOutDt";
        this.lblOutDt.Size = new System.Drawing.Size(48, 15);
        this.lblOutDt.TabIndex = 16;
        this.lblOutDt.Text = "출발확인";
        //
        // txtOutDt
        //
        this.txtOutDt.Location = new System.Drawing.Point(100, 96);
        this.txtOutDt.Name = "txtOutDt";
        this.txtOutDt.Properties.ReadOnly = true;
        this.txtOutDt.Size = new System.Drawing.Size(150, 20);
        this.txtOutDt.TabIndex = 17;
        //
        // lblInDt
        //
        this.lblInDt.Location = new System.Drawing.Point(282, 99);
        this.lblInDt.Name = "lblInDt";
        this.lblInDt.Size = new System.Drawing.Size(48, 15);
        this.lblInDt.TabIndex = 18;
        this.lblInDt.Text = "도착확인";
        //
        // txtInDt
        //
        this.txtInDt.Location = new System.Drawing.Point(354, 96);
        this.txtInDt.Name = "txtInDt";
        this.txtInDt.Properties.ReadOnly = true;
        this.txtInDt.Size = new System.Drawing.Size(150, 20);
        this.txtInDt.TabIndex = 19;
        //
        // lblRemark
        //
        this.lblRemark.Location = new System.Drawing.Point(40, 127);
        this.lblRemark.Name = "lblRemark";
        this.lblRemark.Size = new System.Drawing.Size(24, 15);
        this.lblRemark.TabIndex = 20;
        this.lblRemark.Text = "비고";
        //
        // memoRemark
        //
        this.memoRemark.Location = new System.Drawing.Point(100, 124);
        this.memoRemark.Name = "memoRemark";
        this.memoRemark.Size = new System.Drawing.Size(674, 25);
        this.memoRemark.TabIndex = 21;
        //
        // txtWoId
        //
        this.txtWoId.Location = new System.Drawing.Point(1200, 12);
        this.txtWoId.Name = "txtWoId";
        this.txtWoId.Size = new System.Drawing.Size(60, 20);
        this.txtWoId.TabIndex = 22;
        this.txtWoId.Visible = false;
        //
        // txtFromSerl
        //
        this.txtFromSerl.Location = new System.Drawing.Point(1270, 12);
        this.txtFromSerl.Name = "txtFromSerl";
        this.txtFromSerl.Size = new System.Drawing.Size(60, 20);
        this.txtFromSerl.TabIndex = 23;
        this.txtFromSerl.Visible = false;
        //
        // txtToSerl
        //
        this.txtToSerl.Location = new System.Drawing.Point(1340, 12);
        this.txtToSerl.Name = "txtToSerl";
        this.txtToSerl.Size = new System.Drawing.Size(60, 20);
        this.txtToSerl.TabIndex = 24;
        this.txtToSerl.Visible = false;
        //
        // btnLoadLot
        //
        this.btnLoadLot.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnLoadLot.Location = new System.Drawing.Point(820, 12);
        this.btnLoadLot.Name = "btnLoadLot";
        this.btnLoadLot.Size = new System.Drawing.Size(170, 24);
        this.btnLoadLot.TabIndex = 25;
        this.btnLoadLot.Text = "이전 대상 LOT 불러오기";
        this.btnLoadLot.ToolTip = "진행 중인 작업지시에서, 다음 공정으로 옮길 수 있는 LOT(출발 공정 외주처 창고 재고)를 불러옵니다. 같은 작업지시/공정 구간끼리만 한 문서로 묶입니다.";
        //
        // btnOut
        //
        this.btnOut.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnOut.Location = new System.Drawing.Point(820, 40);
        this.btnOut.Name = "btnOut";
        this.btnOut.Size = new System.Drawing.Size(170, 24);
        this.btnOut.TabIndex = 26;
        this.btnOut.Text = "출발 확인";
        this.btnOut.ToolTip = "출발 외주처 창고에서 이동중 재고로 옮깁니다.";
        //
        // btnOutCancel
        //
        this.btnOutCancel.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnOutCancel.Location = new System.Drawing.Point(820, 68);
        this.btnOutCancel.Name = "btnOutCancel";
        this.btnOutCancel.Size = new System.Drawing.Size(170, 24);
        this.btnOutCancel.TabIndex = 27;
        this.btnOutCancel.Text = "출발 취소";
        this.btnOutCancel.ToolTip = "출발 확인을 취소하고 출발 창고로 되돌립니다(도착 확인 전에만).";
        //
        // btnIn
        //
        this.btnIn.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnIn.Location = new System.Drawing.Point(820, 96);
        this.btnIn.Name = "btnIn";
        this.btnIn.Size = new System.Drawing.Size(170, 24);
        this.btnIn.TabIndex = 28;
        this.btnIn.Text = "도착 확인";
        this.btnIn.ToolTip = "도착 외주처 창고로 입고합니다. 도착 수량을 안 넣은 LOT는 전량 도착으로 봅니다. 차이가 있으면 아래에서 차이 처리를 하세요.";
        //
        // btnInCancel
        //
        this.btnInCancel.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnInCancel.Location = new System.Drawing.Point(820, 124);
        this.btnInCancel.Name = "btnInCancel";
        this.btnInCancel.Size = new System.Drawing.Size(170, 24);
        this.btnInCancel.TabIndex = 29;
        this.btnInCancel.Text = "도착 취소";
        this.btnInCancel.ToolTip = "도착 확인을 취소하고 이동중 재고로 되돌립니다(차이 처리 전에만).";
        //
        // shLine
        //
        this.shLine.BackColor = System.Drawing.Color.White;
        this.shLine.Dock = System.Windows.Forms.DockStyle.Top;
        this.shLine.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shLine.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.shLine.Location = new System.Drawing.Point(3, 219);
        this.shLine.Name = "shLine";
        this.shLine.Size = new System.Drawing.Size(1664, 27);
        this.shLine.TabIndex = 2;
        this.shLine.Text = "이전 LOT (출발수량 / 도착수량 / 차이 처리)";
        //
        // panTool
        //
        this.panTool.Controls.Add(this.btnDeletRow1);
        this.panTool.Controls.Add(this.btnDiff);
        this.panTool.Controls.Add(this.btnDiffCancel);
        this.panTool.Dock = System.Windows.Forms.DockStyle.Top;
        this.panTool.Location = new System.Drawing.Point(3, 246);
        this.panTool.Name = "panTool";
        this.panTool.Size = new System.Drawing.Size(1664, 30);
        this.panTool.TabIndex = 3;
        //
        // btnDeletRow1
        //
        this.btnDeletRow1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(198)))), ((int)(((byte)(193)))));
        this.btnDeletRow1.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnDeletRow1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(236)))), ((int)(((byte)(234)))));
        this.btnDeletRow1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
        this.btnDeletRow1.Location = new System.Drawing.Point(8, 3);
        this.btnDeletRow1.Name = "btnDeletRow1";
        this.btnDeletRow1.Size = new System.Drawing.Size(60, 24);
        this.btnDeletRow1.TabIndex = 0;
        this.btnDeletRow1.Text = "행삭제";
        //
        // btnDiff
        //
        this.btnDiff.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnDiff.Location = new System.Drawing.Point(88, 3);
        this.btnDiff.Name = "btnDiff";
        this.btnDiff.Size = new System.Drawing.Size(120, 24);
        this.btnDiff.TabIndex = 1;
        this.btnDiff.Text = "차이 처리";
        this.btnDiff.ToolTip = "선택한 LOT 행의 귀책/처리방법(손실·재입고)을 지정해 차이 수량을 정리합니다.";
        //
        // btnDiffCancel
        //
        this.btnDiffCancel.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnDiffCancel.Location = new System.Drawing.Point(214, 3);
        this.btnDiffCancel.Name = "btnDiffCancel";
        this.btnDiffCancel.Size = new System.Drawing.Size(120, 24);
        this.btnDiffCancel.TabIndex = 2;
        this.btnDiffCancel.Text = "차이 처리 취소";
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd1.Location = new System.Drawing.Point(3, 276);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.lookupcolResp,
        this.lookupcolAct});
        this.grd1.Size = new System.Drawing.Size(1664, 470);
        this.grd1.TabIndex = 4;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colSerl,
        this.colLotNo,
        this.colItemNo,
        this.colItemNm,
        this.colUnit,
        this.colOutQty,
        this.colInQty,
        this.colDiffQty,
        this.colDiffResp,
        this.colDiffAct,
        this.colDiffDt,
        this.colRemark});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        //
        // lookupcolResp
        //
        this.lookupcolResp.AutoHeight = false;
        this.lookupcolResp.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolResp.LookupKey = "L_PR0005";
        this.lookupcolResp.Name = "lookupcolResp";
        this.lookupcolResp.NullText = "";
        //
        // lookupcolAct
        //
        this.lookupcolAct.AutoHeight = false;
        this.lookupcolAct.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolAct.LookupKey = "L_PR0007";
        this.lookupcolAct.Name = "lookupcolAct";
        this.lookupcolAct.NullText = "";
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
        // colLotNo
        //
        this.colLotNo.Caption = "LOT";
        this.colLotNo.FieldName = "lot_no";
        this.colLotNo.Name = "colLotNo";
        this.colLotNo.OptionsColumn.AllowEdit = false;
        this.colLotNo.Visible = true;
        this.colLotNo.VisibleIndex = 1;
        this.colLotNo.Width = 140;
        //
        // colItemNo
        //
        this.colItemNo.Caption = "품번";
        this.colItemNo.FieldName = "item_no";
        this.colItemNo.Name = "colItemNo";
        this.colItemNo.OptionsColumn.AllowEdit = false;
        this.colItemNo.Visible = true;
        this.colItemNo.VisibleIndex = 2;
        this.colItemNo.Width = 100;
        //
        // colItemNm
        //
        this.colItemNm.Caption = "품명";
        this.colItemNm.FieldName = "item_nm";
        this.colItemNm.Name = "colItemNm";
        this.colItemNm.OptionsColumn.AllowEdit = false;
        this.colItemNm.Visible = true;
        this.colItemNm.VisibleIndex = 3;
        this.colItemNm.Width = 140;
        //
        // colUnit
        //
        this.colUnit.Caption = "단위";
        this.colUnit.FieldName = "unit_cd";
        this.colUnit.Name = "colUnit";
        this.colUnit.OptionsColumn.AllowEdit = false;
        this.colUnit.Visible = true;
        this.colUnit.VisibleIndex = 4;
        this.colUnit.Width = 50;
        //
        // colOutQty
        //
        this.colOutQty.Caption = "출발 수량";
        this.colOutQty.DisplayFormat.FormatString = "#,##0.####";
        this.colOutQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colOutQty.FieldName = "out_qty";
        this.colOutQty.Name = "colOutQty";
        this.colOutQty.Visible = true;
        this.colOutQty.VisibleIndex = 5;
        this.colOutQty.Width = 95;
        //
        // colInQty
        //
        this.colInQty.Caption = "도착 수량";
        this.colInQty.DisplayFormat.FormatString = "#,##0.####";
        this.colInQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colInQty.FieldName = "in_qty";
        this.colInQty.Name = "colInQty";
        this.colInQty.Visible = true;
        this.colInQty.VisibleIndex = 6;
        this.colInQty.Width = 95;
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
        this.colDiffQty.VisibleIndex = 7;
        this.colDiffQty.Width = 75;
        //
        // colDiffResp
        //
        this.colDiffResp.Caption = "차이 귀책";
        this.colDiffResp.ColumnEdit = this.lookupcolResp;
        this.colDiffResp.FieldName = "diff_resp_cd";
        this.colDiffResp.Name = "colDiffResp";
        this.colDiffResp.Visible = true;
        this.colDiffResp.VisibleIndex = 8;
        this.colDiffResp.Width = 90;
        //
        // colDiffAct
        //
        this.colDiffAct.Caption = "차이 처리";
        this.colDiffAct.ColumnEdit = this.lookupcolAct;
        this.colDiffAct.FieldName = "diff_act_cd";
        this.colDiffAct.Name = "colDiffAct";
        this.colDiffAct.Visible = true;
        this.colDiffAct.VisibleIndex = 9;
        this.colDiffAct.Width = 90;
        //
        // colDiffDt
        //
        this.colDiffDt.Caption = "처리일시";
        this.colDiffDt.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
        this.colDiffDt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
        this.colDiffDt.FieldName = "diff_dt";
        this.colDiffDt.Name = "colDiffDt";
        this.colDiffDt.OptionsColumn.AllowEdit = false;
        this.colDiffDt.Visible = true;
        this.colDiffDt.VisibleIndex = 10;
        this.colDiffDt.Width = 120;
        //
        // colRemark
        //
        this.colRemark.Caption = "비고";
        this.colRemark.FieldName = "remark";
        this.colRemark.Name = "colRemark";
        this.colRemark.Visible = true;
        this.colRemark.VisibleIndex = 11;
        this.colRemark.Width = 200;
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblSearchXferNo);
        this.panHeader.Controls.Add(this.txtSearchXferNo);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.Location = new System.Drawing.Point(5, 0);
        this.panHeader.Name = "panHeader";
        this.panHeader.Size = new System.Drawing.Size(1670, 49);
        this.panHeader.TabIndex = 0;
        //
        // lblSearchXferNo
        //
        this.lblSearchXferNo.Location = new System.Drawing.Point(25, 18);
        this.lblSearchXferNo.Name = "lblSearchXferNo";
        this.lblSearchXferNo.Size = new System.Drawing.Size(48, 15);
        this.lblSearchXferNo.TabIndex = 0;
        this.lblSearchXferNo.Text = "이전번호";
        //
        // txtSearchXferNo
        //
        this.txtSearchXferNo.Location = new System.Drawing.Point(90, 15);
        this.txtSearchXferNo.Name = "txtSearchXferNo";
        this.txtSearchXferNo.Size = new System.Drawing.Size(180, 20);
        this.txtSearchXferNo.TabIndex = 1;
        //
        // frmXfer
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 800);
        this.Controls.Add(this.panBase);
        this.Name = "frmXfer";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).EndInit();
        this.panWork.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolResp)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolAct)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panTool)).EndInit();
        this.panTool.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
        this.panData.ResumeLayout(false);
        this.panData.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtXferNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteXferDate.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteXferDate.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboKind.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWoNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtFrom.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtTo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtOutDt.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtInDt.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWoId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtFromSerl.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtToSerl.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        this.panHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchXferNo.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panWork;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private LookUpColumnEdit lookupcolResp;
    private LookUpColumnEdit lookupcolAct;
    private DevExpress.XtraGrid.Columns.GridColumn colSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colLotNo;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colUnit;
    private DevExpress.XtraGrid.Columns.GridColumn colOutQty;
    private DevExpress.XtraGrid.Columns.GridColumn colInQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDiffQty;
    private DevExpress.XtraGrid.Columns.GridColumn colDiffResp;
    private DevExpress.XtraGrid.Columns.GridColumn colDiffAct;
    private DevExpress.XtraGrid.Columns.GridColumn colDiffDt;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private PanelWyn panTool;
    private ButtonWyn btnDeletRow1;
    private ButtonWyn btnDiff;
    private ButtonWyn btnDiffCancel;
    private SectionHeaderWyn shLine;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblAccId;
    private LookUpEditWyn cboAccId;
    private DevExpress.XtraEditors.LabelControl lblStatCd;
    private LookUpEditWyn cboStatCd;
    private DevExpress.XtraEditors.LabelControl lblXferNo;
    private TextEditWyn txtXferNo;
    private DevExpress.XtraEditors.LabelControl lblXferDate;
    private DateEditWyn dteXferDate;
    private DevExpress.XtraEditors.LabelControl lblKind;
    private LookUpEditWyn cboKind;
    private DevExpress.XtraEditors.LabelControl lblWoNo;
    private TextEditWyn txtWoNo;
    private DevExpress.XtraEditors.LabelControl lblFrom;
    private TextEditWyn txtFrom;
    private DevExpress.XtraEditors.LabelControl lblTo;
    private TextEditWyn txtTo;
    private DevExpress.XtraEditors.LabelControl lblOutDt;
    private TextEditWyn txtOutDt;
    private DevExpress.XtraEditors.LabelControl lblInDt;
    private TextEditWyn txtInDt;
    private DevExpress.XtraEditors.LabelControl lblRemark;
    private DevExpress.XtraEditors.MemoEdit memoRemark;
    private TextEditWyn txtWoId;
    private TextEditWyn txtFromSerl;
    private TextEditWyn txtToSerl;
    private ButtonWyn btnLoadLot;
    private ButtonWyn btnOut;
    private ButtonWyn btnOutCancel;
    private ButtonWyn btnIn;
    private ButtonWyn btnInCancel;
    private SectionHeaderWyn shHeader;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchXferNo;
    private TextEditWyn txtSearchXferNo;
}
