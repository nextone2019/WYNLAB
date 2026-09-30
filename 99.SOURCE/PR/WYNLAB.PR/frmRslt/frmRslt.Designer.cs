// 공정실적(frmRslt) - Master-One Sheet 구조(TPRRSLTM/TPRRSLTD). 헤더(투입 LOT/수량/양품/불량) + EDS 웨이퍼별 판정 그리드 + 산출 LOT 그리드.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

public partial class frmRslt
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
        this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
        this.colOutLotNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colOutItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colOutItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colOutUnit = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colOutQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.shOut = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.colSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colWaferNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colGoodQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colBadQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colGrossQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.panTool = new WYNLAB.Base.Controls.PanelWyn();
        this.btnAddRow1 = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnDeletRow1 = new WYNLAB.Base.Controls.ButtonWyn();
        this.shWafer = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panData = new WYNLAB.Base.Controls.PanelWyn();
        this.lblAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblStatCd = new DevExpress.XtraEditors.LabelControl();
        this.cboStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblRsltNo = new DevExpress.XtraEditors.LabelControl();
        this.txtRsltNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblRsltDate = new DevExpress.XtraEditors.LabelControl();
        this.dteRsltDate = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblWoNo = new DevExpress.XtraEditors.LabelControl();
        this.txtWoNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblProcNm = new DevExpress.XtraEditors.LabelControl();
        this.txtProcNm = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblCustNm = new DevExpress.XtraEditors.LabelControl();
        this.txtCustNm = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblWhNm = new DevExpress.XtraEditors.LabelControl();
        this.txtWhNm = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSrcFile = new DevExpress.XtraEditors.LabelControl();
        this.txtSrcFileNm = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblInLotNo = new DevExpress.XtraEditors.LabelControl();
        this.txtInLotNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblInItemNm = new DevExpress.XtraEditors.LabelControl();
        this.txtInItemNm = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblOutItemNm = new DevExpress.XtraEditors.LabelControl();
        this.txtOutItemNm = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblInQty = new DevExpress.XtraEditors.LabelControl();
        this.spnInQty = new WYNLAB.Base.Controls.SpinEditWyn();
        this.lblGoodQty = new DevExpress.XtraEditors.LabelControl();
        this.spnGoodQty = new WYNLAB.Base.Controls.SpinEditWyn();
        this.lblBadQty = new DevExpress.XtraEditors.LabelControl();
        this.spnBadQty = new WYNLAB.Base.Controls.SpinEditWyn();
        this.lblYield = new DevExpress.XtraEditors.LabelControl();
        this.txtYield = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSplit = new DevExpress.XtraEditors.LabelControl();
        this.txtSplitQty = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblUnits = new DevExpress.XtraEditors.LabelControl();
        this.txtUnits = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblRemark = new DevExpress.XtraEditors.LabelControl();
        this.memoRemark = new DevExpress.XtraEditors.MemoEdit();
        this.txtWoId = new WYNLAB.Base.Controls.TextEditWyn();
        this.txtWoSerl = new WYNLAB.Base.Controls.TextEditWyn();
        this.txtInLotId = new WYNLAB.Base.Controls.TextEditWyn();
        this.btnLoadReady = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnImportExcel = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnConfirm = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnConfirmCancel = new WYNLAB.Base.Controls.ButtonWyn();
        this.shHeader = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.lblSearchRsltNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchRsltNo = new WYNLAB.Base.Controls.TextEditWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        this.panBase.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).BeginInit();
        this.panWork.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panTool)).BeginInit();
        this.panTool.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
        this.panData.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtRsltNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteRsltDate.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteRsltDate.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWoNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtProcNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCustNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWhNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSrcFileNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtInLotNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtInItemNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtOutItemNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.spnInQty.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.spnGoodQty.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.spnBadQty.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtYield.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSplitQty.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtUnits.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWoId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWoSerl.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtInLotId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchRsltNo.Properties)).BeginInit();
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
        this.panWork.Controls.Add(this.shOut);
        this.panWork.Controls.Add(this.splitterWyn1);
        this.panWork.Controls.Add(this.grd1);
        this.panWork.Controls.Add(this.panTool);
        this.panWork.Controls.Add(this.shWafer);
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
        this.shHeader.Text = "공정실적 정보등록";
        //
        // panData
        //
        this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        this.panData.Controls.Add(this.lblAccId);
        this.panData.Controls.Add(this.cboAccId);
        this.panData.Controls.Add(this.lblStatCd);
        this.panData.Controls.Add(this.cboStatCd);
        this.panData.Controls.Add(this.lblRsltNo);
        this.panData.Controls.Add(this.txtRsltNo);
        this.panData.Controls.Add(this.lblRsltDate);
        this.panData.Controls.Add(this.dteRsltDate);
        this.panData.Controls.Add(this.lblWoNo);
        this.panData.Controls.Add(this.txtWoNo);
        this.panData.Controls.Add(this.lblProcNm);
        this.panData.Controls.Add(this.txtProcNm);
        this.panData.Controls.Add(this.lblCustNm);
        this.panData.Controls.Add(this.txtCustNm);
        this.panData.Controls.Add(this.lblWhNm);
        this.panData.Controls.Add(this.txtWhNm);
        this.panData.Controls.Add(this.lblSrcFile);
        this.panData.Controls.Add(this.txtSrcFileNm);
        this.panData.Controls.Add(this.lblInLotNo);
        this.panData.Controls.Add(this.txtInLotNo);
        this.panData.Controls.Add(this.lblInItemNm);
        this.panData.Controls.Add(this.txtInItemNm);
        this.panData.Controls.Add(this.lblOutItemNm);
        this.panData.Controls.Add(this.txtOutItemNm);
        this.panData.Controls.Add(this.lblInQty);
        this.panData.Controls.Add(this.spnInQty);
        this.panData.Controls.Add(this.lblGoodQty);
        this.panData.Controls.Add(this.spnGoodQty);
        this.panData.Controls.Add(this.lblBadQty);
        this.panData.Controls.Add(this.spnBadQty);
        this.panData.Controls.Add(this.lblYield);
        this.panData.Controls.Add(this.txtYield);
        this.panData.Controls.Add(this.lblSplit);
        this.panData.Controls.Add(this.txtSplitQty);
        this.panData.Controls.Add(this.lblUnits);
        this.panData.Controls.Add(this.txtUnits);
        this.panData.Controls.Add(this.lblRemark);
        this.panData.Controls.Add(this.memoRemark);
        this.panData.Controls.Add(this.txtWoId);
        this.panData.Controls.Add(this.txtWoSerl);
        this.panData.Controls.Add(this.txtInLotId);
        this.panData.Controls.Add(this.btnLoadReady);
        this.panData.Controls.Add(this.btnImportExcel);
        this.panData.Controls.Add(this.btnConfirm);
        this.panData.Controls.Add(this.btnConfirmCancel);
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
        this.cboStatCd.LookupKey = "L_PR0006";
        this.cboStatCd.Name = "cboStatCd";
        this.cboStatCd.Properties.NullText = "";
        this.cboStatCd.Properties.ReadOnly = true;
        this.cboStatCd.Size = new System.Drawing.Size(150, 20);
        this.cboStatCd.TabIndex = 3;
        //
        // lblRsltNo
        //
        this.lblRsltNo.Location = new System.Drawing.Point(536, 15);
        this.lblRsltNo.Name = "lblRsltNo";
        this.lblRsltNo.Size = new System.Drawing.Size(48, 15);
        this.lblRsltNo.TabIndex = 4;
        this.lblRsltNo.Text = "실적번호";
        //
        // txtRsltNo
        //
        this.txtRsltNo.Location = new System.Drawing.Point(624, 12);
        this.txtRsltNo.Name = "txtRsltNo";
        this.txtRsltNo.Properties.ReadOnly = true;
        this.txtRsltNo.Size = new System.Drawing.Size(150, 20);
        this.txtRsltNo.TabIndex = 5;
        //
        // lblRsltDate
        //
        this.lblRsltDate.Location = new System.Drawing.Point(28, 43);
        this.lblRsltDate.Name = "lblRsltDate";
        this.lblRsltDate.Size = new System.Drawing.Size(48, 15);
        this.lblRsltDate.TabIndex = 6;
        this.lblRsltDate.Text = "실적일자";
        //
        // dteRsltDate
        //
        this.dteRsltDate.Location = new System.Drawing.Point(100, 40);
        this.dteRsltDate.Name = "dteRsltDate";
        this.dteRsltDate.Required = true;
        this.dteRsltDate.Size = new System.Drawing.Size(150, 20);
        this.dteRsltDate.TabIndex = 7;
        //
        // lblWoNo
        //
        this.lblWoNo.Location = new System.Drawing.Point(282, 43);
        this.lblWoNo.Name = "lblWoNo";
        this.lblWoNo.Size = new System.Drawing.Size(60, 15);
        this.lblWoNo.TabIndex = 8;
        this.lblWoNo.Text = "작업지시번호";
        //
        // txtWoNo
        //
        this.txtWoNo.Location = new System.Drawing.Point(354, 40);
        this.txtWoNo.Name = "txtWoNo";
        this.txtWoNo.Properties.ReadOnly = true;
        this.txtWoNo.Size = new System.Drawing.Size(150, 20);
        this.txtWoNo.TabIndex = 9;
        //
        // lblProcNm
        //
        this.lblProcNm.Location = new System.Drawing.Point(536, 43);
        this.lblProcNm.Name = "lblProcNm";
        this.lblProcNm.Size = new System.Drawing.Size(24, 15);
        this.lblProcNm.TabIndex = 10;
        this.lblProcNm.Text = "공정";
        //
        // txtProcNm
        //
        this.txtProcNm.Location = new System.Drawing.Point(624, 40);
        this.txtProcNm.Name = "txtProcNm";
        this.txtProcNm.Properties.ReadOnly = true;
        this.txtProcNm.Size = new System.Drawing.Size(150, 20);
        this.txtProcNm.TabIndex = 11;
        //
        // lblCustNm
        //
        this.lblCustNm.Location = new System.Drawing.Point(28, 71);
        this.lblCustNm.Name = "lblCustNm";
        this.lblCustNm.Size = new System.Drawing.Size(36, 15);
        this.lblCustNm.TabIndex = 12;
        this.lblCustNm.Text = "외주처";
        //
        // txtCustNm
        //
        this.txtCustNm.Location = new System.Drawing.Point(100, 68);
        this.txtCustNm.Name = "txtCustNm";
        this.txtCustNm.Properties.ReadOnly = true;
        this.txtCustNm.Size = new System.Drawing.Size(150, 20);
        this.txtCustNm.TabIndex = 13;
        //
        // lblWhNm
        //
        this.lblWhNm.Location = new System.Drawing.Point(282, 71);
        this.lblWhNm.Name = "lblWhNm";
        this.lblWhNm.Size = new System.Drawing.Size(60, 15);
        this.lblWhNm.TabIndex = 14;
        this.lblWhNm.Text = "외주처 창고";
        //
        // txtWhNm
        //
        this.txtWhNm.Location = new System.Drawing.Point(354, 68);
        this.txtWhNm.Name = "txtWhNm";
        this.txtWhNm.Properties.ReadOnly = true;
        this.txtWhNm.Size = new System.Drawing.Size(150, 20);
        this.txtWhNm.TabIndex = 15;
        //
        // lblSrcFile
        //
        this.lblSrcFile.Location = new System.Drawing.Point(536, 71);
        this.lblSrcFile.Name = "lblSrcFile";
        this.lblSrcFile.Size = new System.Drawing.Size(48, 15);
        this.lblSrcFile.TabIndex = 16;
        this.lblSrcFile.Text = "원본파일";
        //
        // txtSrcFileNm
        //
        this.txtSrcFileNm.Location = new System.Drawing.Point(624, 68);
        this.txtSrcFileNm.Name = "txtSrcFileNm";
        this.txtSrcFileNm.Properties.ReadOnly = true;
        this.txtSrcFileNm.Size = new System.Drawing.Size(150, 20);
        this.txtSrcFileNm.TabIndex = 17;
        //
        // lblInLotNo
        //
        this.lblInLotNo.Location = new System.Drawing.Point(28, 99);
        this.lblInLotNo.Name = "lblInLotNo";
        this.lblInLotNo.Size = new System.Drawing.Size(48, 15);
        this.lblInLotNo.TabIndex = 18;
        this.lblInLotNo.Text = "투입 LOT";
        //
        // txtInLotNo
        //
        this.txtInLotNo.Location = new System.Drawing.Point(100, 96);
        this.txtInLotNo.Name = "txtInLotNo";
        this.txtInLotNo.Properties.ReadOnly = true;
        this.txtInLotNo.Size = new System.Drawing.Size(150, 20);
        this.txtInLotNo.TabIndex = 19;
        //
        // lblInItemNm
        //
        this.lblInItemNm.Location = new System.Drawing.Point(282, 99);
        this.lblInItemNm.Name = "lblInItemNm";
        this.lblInItemNm.Size = new System.Drawing.Size(48, 15);
        this.lblInItemNm.TabIndex = 20;
        this.lblInItemNm.Text = "투입품목";
        //
        // txtInItemNm
        //
        this.txtInItemNm.Location = new System.Drawing.Point(354, 96);
        this.txtInItemNm.Name = "txtInItemNm";
        this.txtInItemNm.Properties.ReadOnly = true;
        this.txtInItemNm.Size = new System.Drawing.Size(150, 20);
        this.txtInItemNm.TabIndex = 21;
        //
        // lblOutItemNm
        //
        this.lblOutItemNm.Location = new System.Drawing.Point(536, 99);
        this.lblOutItemNm.Name = "lblOutItemNm";
        this.lblOutItemNm.Size = new System.Drawing.Size(48, 15);
        this.lblOutItemNm.TabIndex = 22;
        this.lblOutItemNm.Text = "산출품목";
        //
        // txtOutItemNm
        //
        this.txtOutItemNm.Location = new System.Drawing.Point(624, 96);
        this.txtOutItemNm.Name = "txtOutItemNm";
        this.txtOutItemNm.Properties.ReadOnly = true;
        this.txtOutItemNm.Size = new System.Drawing.Size(150, 20);
        this.txtOutItemNm.TabIndex = 23;
        //
        // lblInQty
        //
        this.lblInQty.Location = new System.Drawing.Point(28, 127);
        this.lblInQty.Name = "lblInQty";
        this.lblInQty.Size = new System.Drawing.Size(48, 15);
        this.lblInQty.TabIndex = 24;
        this.lblInQty.Text = "투입수량";
        //
        // spnInQty
        //
        this.spnInQty.Location = new System.Drawing.Point(100, 124);
        this.spnInQty.Name = "spnInQty";
        this.spnInQty.Size = new System.Drawing.Size(150, 20);
        this.spnInQty.TabIndex = 25;
        //
        // lblGoodQty
        //
        this.lblGoodQty.Location = new System.Drawing.Point(282, 127);
        this.lblGoodQty.Name = "lblGoodQty";
        this.lblGoodQty.Size = new System.Drawing.Size(48, 15);
        this.lblGoodQty.TabIndex = 26;
        this.lblGoodQty.Text = "양품수량";
        //
        // spnGoodQty
        //
        this.spnGoodQty.Location = new System.Drawing.Point(354, 124);
        this.spnGoodQty.Name = "spnGoodQty";
        this.spnGoodQty.Size = new System.Drawing.Size(150, 20);
        this.spnGoodQty.TabIndex = 27;
        //
        // lblBadQty
        //
        this.lblBadQty.Location = new System.Drawing.Point(536, 127);
        this.lblBadQty.Name = "lblBadQty";
        this.lblBadQty.Size = new System.Drawing.Size(48, 15);
        this.lblBadQty.TabIndex = 28;
        this.lblBadQty.Text = "불량수량";
        //
        // spnBadQty
        //
        this.spnBadQty.Location = new System.Drawing.Point(624, 124);
        this.spnBadQty.Name = "spnBadQty";
        this.spnBadQty.Size = new System.Drawing.Size(150, 20);
        this.spnBadQty.TabIndex = 29;
        //
        // lblYield
        //
        this.lblYield.Location = new System.Drawing.Point(28, 155);
        this.lblYield.Name = "lblYield";
        this.lblYield.Size = new System.Drawing.Size(48, 15);
        this.lblYield.TabIndex = 30;
        this.lblYield.Text = "수율(%)";
        //
        // txtYield
        //
        this.txtYield.Location = new System.Drawing.Point(100, 152);
        this.txtYield.Name = "txtYield";
        this.txtYield.Properties.ReadOnly = true;
        this.txtYield.Size = new System.Drawing.Size(150, 20);
        this.txtYield.TabIndex = 31;
        //
        // lblSplit
        //
        this.lblSplit.Location = new System.Drawing.Point(282, 155);
        this.lblSplit.Name = "lblSplit";
        this.lblSplit.Size = new System.Drawing.Size(48, 15);
        this.lblSplit.TabIndex = 32;
        this.lblSplit.Text = "분할수량";
        //
        // txtSplitQty
        //
        this.txtSplitQty.Location = new System.Drawing.Point(354, 152);
        this.txtSplitQty.Name = "txtSplitQty";
        this.txtSplitQty.Properties.ReadOnly = true;
        this.txtSplitQty.Size = new System.Drawing.Size(150, 20);
        this.txtSplitQty.TabIndex = 33;
        //
        // lblUnits
        //
        this.lblUnits.Location = new System.Drawing.Point(536, 155);
        this.lblUnits.Name = "lblUnits";
        this.lblUnits.Size = new System.Drawing.Size(48, 15);
        this.lblUnits.TabIndex = 34;
        this.lblUnits.Text = "단위";
        //
        // txtUnits
        //
        this.txtUnits.Location = new System.Drawing.Point(624, 152);
        this.txtUnits.Name = "txtUnits";
        this.txtUnits.Properties.ReadOnly = true;
        this.txtUnits.Size = new System.Drawing.Size(150, 20);
        this.txtUnits.TabIndex = 35;
        //
        // lblRemark
        //
        this.lblRemark.Location = new System.Drawing.Point(40, 183);
        this.lblRemark.Name = "lblRemark";
        this.lblRemark.Size = new System.Drawing.Size(24, 15);
        this.lblRemark.TabIndex = 36;
        this.lblRemark.Text = "비고";
        //
        // memoRemark
        //
        this.memoRemark.Location = new System.Drawing.Point(100, 180);
        this.memoRemark.Name = "memoRemark";
        this.memoRemark.Size = new System.Drawing.Size(674, 25);
        this.memoRemark.TabIndex = 37;
        //
        // txtWoId
        //
        this.txtWoId.Location = new System.Drawing.Point(1200, 12);
        this.txtWoId.Name = "txtWoId";
        this.txtWoId.Size = new System.Drawing.Size(60, 20);
        this.txtWoId.TabIndex = 38;
        this.txtWoId.Visible = false;
        //
        // txtWoSerl
        //
        this.txtWoSerl.Location = new System.Drawing.Point(1270, 12);
        this.txtWoSerl.Name = "txtWoSerl";
        this.txtWoSerl.Size = new System.Drawing.Size(60, 20);
        this.txtWoSerl.TabIndex = 39;
        this.txtWoSerl.Visible = false;
        //
        // txtInLotId
        //
        this.txtInLotId.Location = new System.Drawing.Point(1340, 12);
        this.txtInLotId.Name = "txtInLotId";
        this.txtInLotId.Size = new System.Drawing.Size(60, 20);
        this.txtInLotId.TabIndex = 40;
        this.txtInLotId.Visible = false;
        //
        // btnLoadReady
        //
        this.btnLoadReady.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnLoadReady.Location = new System.Drawing.Point(820, 12);
        this.btnLoadReady.Name = "btnLoadReady";
        this.btnLoadReady.Size = new System.Drawing.Size(170, 24);
        this.btnLoadReady.TabIndex = 41;
        this.btnLoadReady.Text = "실적 대기 LOT 불러오기";
        this.btnLoadReady.ToolTip = "진행 중인 작업지시에서, 그 공정 외주처 창고에 투입할 LOT 재고가 있는 건을 불러옵니다.";
        //
        // btnImportExcel
        //
        this.btnImportExcel.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnImportExcel.Location = new System.Drawing.Point(820, 40);
        this.btnImportExcel.Name = "btnImportExcel";
        this.btnImportExcel.Size = new System.Drawing.Size(170, 24);
        this.btnImportExcel.TabIndex = 42;
        this.btnImportExcel.Text = "엑셀 불러오기(웨이퍼 판정)";
        this.btnImportExcel.ToolTip = "외주처가 보낸 엑셀(웨이퍼번호 / Good / Bad 열)을 읽어 웨이퍼별 판정 그리드를 채웁니다. 기존 행은 바뀝니다.";
        //
        // btnConfirm
        //
        this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnConfirm.Location = new System.Drawing.Point(820, 68);
        this.btnConfirm.Name = "btnConfirm";
        this.btnConfirm.Size = new System.Drawing.Size(170, 24);
        this.btnConfirm.TabIndex = 43;
        this.btnConfirm.Text = "확정";
        this.btnConfirm.ToolTip = "실적을 확정합니다. 투입 LOT가 소진되고 양품이 산출 LOT로 외주처 창고에 입고됩니다(분할수량이 있으면 나뉩니다).";
        //
        // btnConfirmCancel
        //
        this.btnConfirmCancel.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnConfirmCancel.Location = new System.Drawing.Point(820, 96);
        this.btnConfirmCancel.Name = "btnConfirmCancel";
        this.btnConfirmCancel.Size = new System.Drawing.Size(170, 24);
        this.btnConfirmCancel.TabIndex = 44;
        this.btnConfirmCancel.Text = "확정취소";
        this.btnConfirmCancel.ToolTip = "확정을 취소합니다(수불 역거래). 산출 LOT가 이미 다른 공정에서 쓰였으면 취소할 수 없습니다.";
        //
        // shWafer
        //
        this.shWafer.BackColor = System.Drawing.Color.White;
        this.shWafer.Dock = System.Windows.Forms.DockStyle.Top;
        this.shWafer.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shWafer.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.shWafer.Location = new System.Drawing.Point(3, 241);
        this.shWafer.Name = "shWafer";
        this.shWafer.Size = new System.Drawing.Size(1664, 27);
        this.shWafer.TabIndex = 2;
        this.shWafer.Text = "웨이퍼별 합/부 판정 (투입/산출 단위가 다른 공정에서 사용 - 합계가 양품/불량 수량이 됩니다)";
        //
        // panTool
        //
        this.panTool.Controls.Add(this.btnAddRow1);
        this.panTool.Controls.Add(this.btnDeletRow1);
        this.panTool.Dock = System.Windows.Forms.DockStyle.Top;
        this.panTool.Location = new System.Drawing.Point(3, 268);
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
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Top;
        this.grd1.Location = new System.Drawing.Point(3, 298);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.Size = new System.Drawing.Size(1664, 190);
        this.grd1.TabIndex = 4;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colSerl,
        this.colWaferNo,
        this.colGoodQty,
        this.colBadQty,
        this.colGrossQty,
        this.colRemark});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowFooter = true;
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
        // colWaferNo
        //
        this.colWaferNo.Caption = "웨이퍼 번호";
        this.colWaferNo.FieldName = "wafer_no";
        this.colWaferNo.Name = "colWaferNo";
        this.colWaferNo.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "wafer_no", "합계 ({0:#,##0}장)")});
        this.colWaferNo.Visible = true;
        this.colWaferNo.VisibleIndex = 1;
        this.colWaferNo.Width = 100;
        //
        // colGoodQty
        //
        this.colGoodQty.Caption = "Good Die";
        this.colGoodQty.DisplayFormat.FormatString = "#,##0.####";
        this.colGoodQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colGoodQty.FieldName = "good_qty";
        this.colGoodQty.Name = "colGoodQty";
        this.colGoodQty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "good_qty", "{0:#,##0.####}")});
        this.colGoodQty.Visible = true;
        this.colGoodQty.VisibleIndex = 2;
        this.colGoodQty.Width = 100;
        //
        // colBadQty
        //
        this.colBadQty.Caption = "Bad Die";
        this.colBadQty.DisplayFormat.FormatString = "#,##0.####";
        this.colBadQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colBadQty.FieldName = "bad_qty";
        this.colBadQty.Name = "colBadQty";
        this.colBadQty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "bad_qty", "{0:#,##0.####}")});
        this.colBadQty.Visible = true;
        this.colBadQty.VisibleIndex = 3;
        this.colBadQty.Width = 100;
        //
        // colGrossQty
        //
        this.colGrossQty.Caption = "Gross Die";
        this.colGrossQty.DisplayFormat.FormatString = "#,##0.####";
        this.colGrossQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colGrossQty.FieldName = "gross_qty";
        this.colGrossQty.Name = "colGrossQty";
        this.colGrossQty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "gross_qty", "{0:#,##0.####}")});
        this.colGrossQty.OptionsColumn.AllowEdit = false;
        this.colGrossQty.Visible = true;
        this.colGrossQty.VisibleIndex = 4;
        this.colGrossQty.Width = 100;
        //
        // colRemark
        //
        this.colRemark.Caption = "비고";
        this.colRemark.FieldName = "remark";
        this.colRemark.Name = "colRemark";
        this.colRemark.Visible = true;
        this.colRemark.VisibleIndex = 5;
        this.colRemark.Width = 220;
        //
        // splitterWyn1
        //
        this.splitterWyn1.BackColor = System.Drawing.Color.White;
        this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Top;
        this.splitterWyn1.Location = new System.Drawing.Point(3, 488);
        this.splitterWyn1.Name = "splitterWyn1";
        this.splitterWyn1.Size = new System.Drawing.Size(1664, 10);
        this.splitterWyn1.TabIndex = 7;
        this.splitterWyn1.TabStop = false;
        //
        // shOut
        //
        this.shOut.BackColor = System.Drawing.Color.White;
        this.shOut.Dock = System.Windows.Forms.DockStyle.Top;
        this.shOut.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shOut.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.shOut.Location = new System.Drawing.Point(3, 498);
        this.shOut.Name = "shOut";
        this.shOut.Size = new System.Drawing.Size(1664, 27);
        this.shOut.TabIndex = 5;
        this.shOut.Text = "산출 LOT (확정하면 생성됩니다)";
        //
        // grd2
        //
        this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd2.Location = new System.Drawing.Point(3, 525);
        this.grd2.MainView = this.gvw2;
        this.grd2.Name = "grd2";
        this.grd2.Size = new System.Drawing.Size(1664, 231);
        this.grd2.TabIndex = 6;
        this.grd2.UseEmbeddedNavigator = false;
        this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw2});
        //
        // gvw2
        //
        this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colOutLotNo,
        this.colOutItemNo,
        this.colOutItemNm,
        this.colOutUnit,
        this.colOutQty});
        this.gvw2.GridControl = this.grd2;
        this.gvw2.HighlightFocusedRow = true;
        this.gvw2.Name = "gvw2";
        this.gvw2.OptionsBehavior.Editable = false;
        this.gvw2.OptionsView.ColumnAutoWidth = false;
        this.gvw2.OptionsView.ShowGroupPanel = false;
        //
        // colOutLotNo
        //
        this.colOutLotNo.Caption = "산출 LOT";
        this.colOutLotNo.FieldName = "lot_no";
        this.colOutLotNo.Name = "colOutLotNo";
        this.colOutLotNo.Visible = true;
        this.colOutLotNo.VisibleIndex = 0;
        this.colOutLotNo.Width = 140;
        //
        // colOutItemNo
        //
        this.colOutItemNo.Caption = "품번";
        this.colOutItemNo.FieldName = "item_no";
        this.colOutItemNo.Name = "colOutItemNo";
        this.colOutItemNo.Visible = true;
        this.colOutItemNo.VisibleIndex = 1;
        this.colOutItemNo.Width = 100;
        //
        // colOutItemNm
        //
        this.colOutItemNm.Caption = "품명";
        this.colOutItemNm.FieldName = "item_nm";
        this.colOutItemNm.Name = "colOutItemNm";
        this.colOutItemNm.Visible = true;
        this.colOutItemNm.VisibleIndex = 2;
        this.colOutItemNm.Width = 140;
        //
        // colOutUnit
        //
        this.colOutUnit.Caption = "단위";
        this.colOutUnit.FieldName = "unit_cd";
        this.colOutUnit.Name = "colOutUnit";
        this.colOutUnit.Visible = true;
        this.colOutUnit.VisibleIndex = 3;
        this.colOutUnit.Width = 50;
        //
        // colOutQty
        //
        this.colOutQty.Caption = "수량";
        this.colOutQty.DisplayFormat.FormatString = "#,##0.####";
        this.colOutQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colOutQty.FieldName = "qty";
        this.colOutQty.Name = "colOutQty";
        this.colOutQty.Visible = true;
        this.colOutQty.VisibleIndex = 4;
        this.colOutQty.Width = 90;
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblSearchRsltNo);
        this.panHeader.Controls.Add(this.txtSearchRsltNo);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.Location = new System.Drawing.Point(5, 0);
        this.panHeader.Name = "panHeader";
        this.panHeader.Size = new System.Drawing.Size(1670, 49);
        this.panHeader.TabIndex = 0;
        //
        // lblSearchRsltNo
        //
        this.lblSearchRsltNo.Location = new System.Drawing.Point(25, 18);
        this.lblSearchRsltNo.Name = "lblSearchRsltNo";
        this.lblSearchRsltNo.Size = new System.Drawing.Size(48, 15);
        this.lblSearchRsltNo.TabIndex = 0;
        this.lblSearchRsltNo.Text = "실적번호";
        //
        // txtSearchRsltNo
        //
        this.txtSearchRsltNo.Location = new System.Drawing.Point(90, 15);
        this.txtSearchRsltNo.Name = "txtSearchRsltNo";
        this.txtSearchRsltNo.Size = new System.Drawing.Size(180, 20);
        this.txtSearchRsltNo.TabIndex = 1;
        //
        // frmRslt
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 800);
        this.Controls.Add(this.panBase);
        this.Name = "frmRslt";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).EndInit();
        this.panWork.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panTool)).EndInit();
        this.panTool.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
        this.panData.ResumeLayout(false);
        this.panData.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtRsltNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteRsltDate.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteRsltDate.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWoNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtProcNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCustNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWhNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSrcFileNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtInLotNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtInItemNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtOutItemNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.spnInQty.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.spnGoodQty.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.spnBadQty.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtYield.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSplitQty.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtUnits.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWoId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtWoSerl.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtInLotId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        this.panHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchRsltNo.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panWork;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private DevExpress.XtraGrid.Columns.GridColumn colOutLotNo;
    private DevExpress.XtraGrid.Columns.GridColumn colOutItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colOutItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colOutUnit;
    private DevExpress.XtraGrid.Columns.GridColumn colOutQty;
    private SectionHeaderWyn shOut;
    private GridControlWyn grd1;
    private SplitterWyn splitterWyn1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colWaferNo;
    private DevExpress.XtraGrid.Columns.GridColumn colGoodQty;
    private DevExpress.XtraGrid.Columns.GridColumn colBadQty;
    private DevExpress.XtraGrid.Columns.GridColumn colGrossQty;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private PanelWyn panTool;
    private ButtonWyn btnAddRow1;
    private ButtonWyn btnDeletRow1;
    private SectionHeaderWyn shWafer;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblAccId;
    private LookUpEditWyn cboAccId;
    private DevExpress.XtraEditors.LabelControl lblStatCd;
    private LookUpEditWyn cboStatCd;
    private DevExpress.XtraEditors.LabelControl lblRsltNo;
    private TextEditWyn txtRsltNo;
    private DevExpress.XtraEditors.LabelControl lblRsltDate;
    private DateEditWyn dteRsltDate;
    private DevExpress.XtraEditors.LabelControl lblWoNo;
    private TextEditWyn txtWoNo;
    private DevExpress.XtraEditors.LabelControl lblProcNm;
    private TextEditWyn txtProcNm;
    private DevExpress.XtraEditors.LabelControl lblCustNm;
    private TextEditWyn txtCustNm;
    private DevExpress.XtraEditors.LabelControl lblWhNm;
    private TextEditWyn txtWhNm;
    private DevExpress.XtraEditors.LabelControl lblSrcFile;
    private TextEditWyn txtSrcFileNm;
    private DevExpress.XtraEditors.LabelControl lblInLotNo;
    private TextEditWyn txtInLotNo;
    private DevExpress.XtraEditors.LabelControl lblInItemNm;
    private TextEditWyn txtInItemNm;
    private DevExpress.XtraEditors.LabelControl lblOutItemNm;
    private TextEditWyn txtOutItemNm;
    private DevExpress.XtraEditors.LabelControl lblInQty;
    private SpinEditWyn spnInQty;
    private DevExpress.XtraEditors.LabelControl lblGoodQty;
    private SpinEditWyn spnGoodQty;
    private DevExpress.XtraEditors.LabelControl lblBadQty;
    private SpinEditWyn spnBadQty;
    private DevExpress.XtraEditors.LabelControl lblYield;
    private TextEditWyn txtYield;
    private DevExpress.XtraEditors.LabelControl lblSplit;
    private TextEditWyn txtSplitQty;
    private DevExpress.XtraEditors.LabelControl lblUnits;
    private TextEditWyn txtUnits;
    private DevExpress.XtraEditors.LabelControl lblRemark;
    private DevExpress.XtraEditors.MemoEdit memoRemark;
    private TextEditWyn txtWoId;
    private TextEditWyn txtWoSerl;
    private TextEditWyn txtInLotId;
    private ButtonWyn btnLoadReady;
    private ButtonWyn btnImportExcel;
    private ButtonWyn btnConfirm;
    private ButtonWyn btnConfirmCancel;
    private SectionHeaderWyn shHeader;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchRsltNo;
    private TextEditWyn txtSearchRsltNo;
}
