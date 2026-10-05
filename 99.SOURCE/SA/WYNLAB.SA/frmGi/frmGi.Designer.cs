// 출고등록(frmGi) - 헤더(panData) + 출고 품목 그리드(grd1, 거래명세서 라인 1건당 1행) + 선택한 품목의 LOT 상세 그리드(grd2).
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.SA;

public partial class frmGi
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
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblSearchGiNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchGiNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.shHeader = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panData = new WYNLAB.Base.Controls.PanelWyn();
        this.lblAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblStatCd = new DevExpress.XtraEditors.LabelControl();
        this.cboStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblGiNo = new DevExpress.XtraEditors.LabelControl();
        this.txtGiNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblGiDate = new DevExpress.XtraEditors.LabelControl();
        this.dteGiDate = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblCustNm = new DevExpress.XtraEditors.LabelControl();
        this.txtCustNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.lblShipDate = new DevExpress.XtraEditors.LabelControl();
        this.dteShipDate = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblCarrier = new DevExpress.XtraEditors.LabelControl();
        this.txtCarrier = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblBlNo = new DevExpress.XtraEditors.LabelControl();
        this.txtBlNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDest = new DevExpress.XtraEditors.LabelControl();
        this.txtDest = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDeptNm = new DevExpress.XtraEditors.LabelControl();
        this.txtDeptNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.lblEmpNm = new DevExpress.XtraEditors.LabelControl();
        this.txtEmpNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.txtCustId = new WYNLAB.Base.Controls.TextEditWyn();
        this.txtDeptId = new WYNLAB.Base.Controls.TextEditWyn();
        this.txtEmpId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblRemark = new DevExpress.XtraEditors.LabelControl();
        this.memoRemark = new DevExpress.XtraEditors.MemoEdit();
        this.lblHint = new DevExpress.XtraEditors.LabelControl();
        this.btnConfirm = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnConfirmCancel = new WYNLAB.Base.Controls.ButtonWyn();
        this.shLines = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panTool = new WYNLAB.Base.Controls.PanelWyn();
        this.btnPickLine = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnDelLine = new WYNLAB.Base.Controls.ButtonWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.colSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colInvcNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colSoNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colSoSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colUnit = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colSoQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRemain = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPrice = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colAmt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
        this.shLots = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panTool2 = new WYNLAB.Base.Controls.PanelWyn();
        this.btnPickLot = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnDelLot = new WYNLAB.Base.Controls.ButtonWyn();
        this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
        this.lookupcolKind = new WYNLAB.Base.Controls.LookUpColumnEdit();
        this.colLtSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLtKind = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLtLotNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLtWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLtStock = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLtQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLtRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        this.panBase.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).BeginInit();
        this.panWork.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
        this.panData.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panTool)).BeginInit();
        this.panTool.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panTool2)).BeginInit();
        this.panTool2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolKind)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtGiNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteGiDate.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteGiDate.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCustNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteShipDate.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteShipDate.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCarrier.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtBlNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDest.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCustId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchGiNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).BeginInit();
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
        this.panWork.Controls.Add(this.panTool2);
        this.panWork.Controls.Add(this.shLots);
        this.panWork.Controls.Add(this.splitterWyn1);
        this.panWork.Controls.Add(this.grd1);
        this.panWork.Controls.Add(this.panTool);
        this.panWork.Controls.Add(this.shLines);
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
        this.shHeader.Text = "출고 정보등록";
        //
        // panData
        //
        this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        this.panData.Controls.Add(this.lblAccId);
        this.panData.Controls.Add(this.cboAccId);
        this.panData.Controls.Add(this.lblStatCd);
        this.panData.Controls.Add(this.cboStatCd);
        this.panData.Controls.Add(this.lblGiNo);
        this.panData.Controls.Add(this.txtGiNo);
        this.panData.Controls.Add(this.lblGiDate);
        this.panData.Controls.Add(this.dteGiDate);
        this.panData.Controls.Add(this.lblCustNm);
        this.panData.Controls.Add(this.txtCustNm);
        this.panData.Controls.Add(this.lblShipDate);
        this.panData.Controls.Add(this.dteShipDate);
        this.panData.Controls.Add(this.lblCarrier);
        this.panData.Controls.Add(this.txtCarrier);
        this.panData.Controls.Add(this.lblBlNo);
        this.panData.Controls.Add(this.txtBlNo);
        this.panData.Controls.Add(this.lblDest);
        this.panData.Controls.Add(this.txtDest);
        this.panData.Controls.Add(this.lblDeptNm);
        this.panData.Controls.Add(this.txtDeptNm);
        this.panData.Controls.Add(this.lblEmpNm);
        this.panData.Controls.Add(this.txtEmpNm);
        this.panData.Controls.Add(this.txtCustId);
        this.panData.Controls.Add(this.txtDeptId);
        this.panData.Controls.Add(this.txtEmpId);
        this.panData.Controls.Add(this.lblRemark);
        this.panData.Controls.Add(this.memoRemark);
        this.panData.Controls.Add(this.lblHint);
        this.panData.Controls.Add(this.btnConfirm);
        this.panData.Controls.Add(this.btnConfirmCancel);
        this.panData.Dock = System.Windows.Forms.DockStyle.Top;
        this.panData.Location = new System.Drawing.Point(3, 27);
        this.panData.Name = "panData";
        this.panData.Size = new System.Drawing.Size(1664, 190);
        this.panData.TabIndex = 1;
        //
        // lblAccId
        //
        this.lblAccId.Location = new System.Drawing.Point(28, 15);
        this.lblAccId.Name = "lblAccId";
        this.lblAccId.Size = new System.Drawing.Size(48, 15);
        this.lblAccId.TabIndex = 2;
        this.lblAccId.Text = "사업장";
        //
        // cboAccId
        //
        this.cboAccId.Location = new System.Drawing.Point(100, 12);
        this.cboAccId.LookupKey = "L_ACC";
        this.cboAccId.Required = true;
        this.cboAccId.Properties.NullText = "";
        this.cboAccId.Name = "cboAccId";
        this.cboAccId.Size = new System.Drawing.Size(150, 20);
        this.cboAccId.TabIndex = 3;
        //
        // lblStatCd
        //
        this.lblStatCd.Location = new System.Drawing.Point(282, 15);
        this.lblStatCd.Name = "lblStatCd";
        this.lblStatCd.Size = new System.Drawing.Size(48, 15);
        this.lblStatCd.TabIndex = 4;
        this.lblStatCd.Text = "진행상태";
        //
        // cboStatCd
        //
        this.cboStatCd.Location = new System.Drawing.Point(354, 12);
        this.cboStatCd.LookupKey = "L_MA0002";
        this.cboStatCd.Properties.ReadOnly = true;
        this.cboStatCd.Properties.NullText = "";
        this.cboStatCd.Name = "cboStatCd";
        this.cboStatCd.Size = new System.Drawing.Size(150, 20);
        this.cboStatCd.TabIndex = 5;
        //
        // lblGiNo
        //
        this.lblGiNo.Location = new System.Drawing.Point(536, 15);
        this.lblGiNo.Name = "lblGiNo";
        this.lblGiNo.Size = new System.Drawing.Size(48, 15);
        this.lblGiNo.TabIndex = 6;
        this.lblGiNo.Text = "출고번호";
        //
        // txtGiNo
        //
        this.txtGiNo.Location = new System.Drawing.Point(608, 12);
        this.txtGiNo.Properties.ReadOnly = true;
        this.txtGiNo.Name = "txtGiNo";
        this.txtGiNo.Size = new System.Drawing.Size(150, 20);
        this.txtGiNo.TabIndex = 7;
        //
        // lblGiDate
        //
        this.lblGiDate.Location = new System.Drawing.Point(28, 43);
        this.lblGiDate.Name = "lblGiDate";
        this.lblGiDate.Size = new System.Drawing.Size(48, 15);
        this.lblGiDate.TabIndex = 8;
        this.lblGiDate.Text = "출고일자";
        //
        // dteGiDate
        //
        this.dteGiDate.Location = new System.Drawing.Point(100, 40);
        this.dteGiDate.Required = true;
        this.dteGiDate.Name = "dteGiDate";
        this.dteGiDate.Size = new System.Drawing.Size(150, 20);
        this.dteGiDate.TabIndex = 9;
        //
        // lblCustNm
        //
        this.lblCustNm.Location = new System.Drawing.Point(282, 43);
        this.lblCustNm.Name = "lblCustNm";
        this.lblCustNm.Size = new System.Drawing.Size(48, 15);
        this.lblCustNm.TabIndex = 10;
        this.lblCustNm.Text = "고객";
        //
        // txtCustNm
        //
        this.txtCustNm.Location = new System.Drawing.Point(354, 40);
        this.txtCustNm.LookupKey = "P_CUST";
        this.txtCustNm.MatchField = "cust_nm";
        this.txtCustNm.PopupConditions = "p_cust_class=SA";
        this.txtCustNm.Required = true;
        this.txtCustNm.Name = "txtCustNm";
        this.txtCustNm.Size = new System.Drawing.Size(150, 20);
        this.txtCustNm.TabIndex = 11;
        //
        // lblShipDate
        //
        this.lblShipDate.Location = new System.Drawing.Point(536, 43);
        this.lblShipDate.Name = "lblShipDate";
        this.lblShipDate.Size = new System.Drawing.Size(48, 15);
        this.lblShipDate.TabIndex = 12;
        this.lblShipDate.Text = "선적일자";
        //
        // dteShipDate
        //
        this.dteShipDate.Location = new System.Drawing.Point(608, 40);
        this.dteShipDate.Name = "dteShipDate";
        this.dteShipDate.Size = new System.Drawing.Size(150, 20);
        this.dteShipDate.TabIndex = 13;
        //
        // lblCarrier
        //
        this.lblCarrier.Location = new System.Drawing.Point(28, 71);
        this.lblCarrier.Name = "lblCarrier";
        this.lblCarrier.Size = new System.Drawing.Size(48, 15);
        this.lblCarrier.TabIndex = 14;
        this.lblCarrier.Text = "운송사";
        //
        // txtCarrier
        //
        this.txtCarrier.Location = new System.Drawing.Point(100, 68);
        this.txtCarrier.Name = "txtCarrier";
        this.txtCarrier.Size = new System.Drawing.Size(150, 20);
        this.txtCarrier.TabIndex = 15;
        //
        // lblBlNo
        //
        this.lblBlNo.Location = new System.Drawing.Point(282, 71);
        this.lblBlNo.Name = "lblBlNo";
        this.lblBlNo.Size = new System.Drawing.Size(48, 15);
        this.lblBlNo.TabIndex = 16;
        this.lblBlNo.Text = "B/L번호";
        //
        // txtBlNo
        //
        this.txtBlNo.Location = new System.Drawing.Point(354, 68);
        this.txtBlNo.Name = "txtBlNo";
        this.txtBlNo.Size = new System.Drawing.Size(150, 20);
        this.txtBlNo.TabIndex = 17;
        //
        // lblDest
        //
        this.lblDest.Location = new System.Drawing.Point(536, 71);
        this.lblDest.Name = "lblDest";
        this.lblDest.Size = new System.Drawing.Size(48, 15);
        this.lblDest.TabIndex = 18;
        this.lblDest.Text = "도착지";
        //
        // txtDest
        //
        this.txtDest.Location = new System.Drawing.Point(608, 68);
        this.txtDest.Name = "txtDest";
        this.txtDest.Size = new System.Drawing.Size(150, 20);
        this.txtDest.TabIndex = 19;
        //
        // lblDeptNm
        //
        this.lblDeptNm.Location = new System.Drawing.Point(28, 99);
        this.lblDeptNm.Name = "lblDeptNm";
        this.lblDeptNm.Size = new System.Drawing.Size(48, 15);
        this.lblDeptNm.TabIndex = 20;
        this.lblDeptNm.Text = "부서";
        //
        // txtDeptNm
        //
        this.txtDeptNm.Location = new System.Drawing.Point(100, 96);
        this.txtDeptNm.LookupKey = "P_DEPT";
        this.txtDeptNm.MatchField = "dept_nm";
        this.txtDeptNm.Name = "txtDeptNm";
        this.txtDeptNm.Size = new System.Drawing.Size(150, 20);
        this.txtDeptNm.TabIndex = 21;
        //
        // lblEmpNm
        //
        this.lblEmpNm.Location = new System.Drawing.Point(282, 99);
        this.lblEmpNm.Name = "lblEmpNm";
        this.lblEmpNm.Size = new System.Drawing.Size(48, 15);
        this.lblEmpNm.TabIndex = 22;
        this.lblEmpNm.Text = "담당자";
        //
        // txtEmpNm
        //
        this.txtEmpNm.Location = new System.Drawing.Point(354, 96);
        this.txtEmpNm.LookupKey = "P_EMP";
        this.txtEmpNm.MatchField = "emp_nm";
        this.txtEmpNm.Name = "txtEmpNm";
        this.txtEmpNm.Size = new System.Drawing.Size(150, 20);
        this.txtEmpNm.TabIndex = 23;
        //
        // txtCustId
        //
        this.txtCustId.Location = new System.Drawing.Point(1000, 12);
        this.txtCustId.Name = "txtCustId";
        this.txtCustId.Size = new System.Drawing.Size(60, 20);
        this.txtCustId.TabIndex = 24;
        this.txtCustId.Visible = false;
        //
        // txtDeptId
        //
        this.txtDeptId.Location = new System.Drawing.Point(1070, 12);
        this.txtDeptId.Name = "txtDeptId";
        this.txtDeptId.Size = new System.Drawing.Size(60, 20);
        this.txtDeptId.TabIndex = 25;
        this.txtDeptId.Visible = false;
        //
        // txtEmpId
        //
        this.txtEmpId.Location = new System.Drawing.Point(1140, 12);
        this.txtEmpId.Name = "txtEmpId";
        this.txtEmpId.Size = new System.Drawing.Size(60, 20);
        this.txtEmpId.TabIndex = 26;
        this.txtEmpId.Visible = false;
        //
        // lblRemark
        //
        this.lblRemark.Location = new System.Drawing.Point(28, 127);
        this.lblRemark.Name = "lblRemark";
        this.lblRemark.Size = new System.Drawing.Size(24, 15);
        this.lblRemark.TabIndex = 27;
        this.lblRemark.Text = "비고";
        //
        // memoRemark
        //
        this.memoRemark.Location = new System.Drawing.Point(100, 124);
        this.memoRemark.Name = "memoRemark";
        this.memoRemark.Size = new System.Drawing.Size(674, 40);
        this.memoRemark.TabIndex = 28;
        //
        // lblHint
        //
        this.lblHint.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
        this.lblHint.Appearance.Options.UseForeColor = true;
        this.lblHint.Location = new System.Drawing.Point(28, 170);
        this.lblHint.Name = "lblHint";
        this.lblHint.Size = new System.Drawing.Size(60, 15);
        this.lblHint.TabIndex = 29;
        this.lblHint.Text = "※ 고객을 고르고 [명세서 품목 불러오기]로 출고할 확정 거래명세서 품목(같은 고객의 여러 명세서 가능)을 가져온 뒤, 품목 행을 선택하고 [출고 LOT 선택]에서 자사창고/외주처를 골라 조회한 LOT를 여러 건 추가해 수량을 입력 → 저장 → [확정]하면 재고가 차감됩니다.";
        //
        // btnConfirm
        //
        this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnConfirm.Location = new System.Drawing.Point(820, 12);
        this.btnConfirm.Name = "btnConfirm";
        this.btnConfirm.Size = new System.Drawing.Size(170, 24);
        this.btnConfirm.TabIndex = 30;
        this.btnConfirm.Text = "확정";
        this.btnConfirm.ToolTip = "출고를 확정합니다. LOT 재고가 차감되고 거래명세서 출고누계가 갱신됩니다.";
        //
        // btnConfirmCancel
        //
        this.btnConfirmCancel.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnConfirmCancel.Location = new System.Drawing.Point(820, 40);
        this.btnConfirmCancel.Name = "btnConfirmCancel";
        this.btnConfirmCancel.Size = new System.Drawing.Size(170, 24);
        this.btnConfirmCancel.TabIndex = 31;
        this.btnConfirmCancel.Text = "확정취소";
        this.btnConfirmCancel.ToolTip = "확정을 취소하고 재고와 거래명세서 출고누계를 되돌립니다.";
        //
        // shLines
        //
        this.shLines.BackColor = System.Drawing.Color.White;
        this.shLines.Dock = System.Windows.Forms.DockStyle.Top;
        this.shLines.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shLines.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.shLines.Location = new System.Drawing.Point(3, 217);
        this.shLines.Name = "shLines";
        this.shLines.Size = new System.Drawing.Size(1664, 27);
        this.shLines.TabIndex = 32;
        this.shLines.Text = "출고 품목 (거래명세서 라인 1건 = 1행, 수량은 아래 LOT 상세의 합계)";
        //
        // panTool
        //
        this.panTool.Controls.Add(this.btnPickLine);
        this.panTool.Controls.Add(this.btnDelLine);
        this.panTool.Dock = System.Windows.Forms.DockStyle.Top;
        this.panTool.Location = new System.Drawing.Point(3, 244);
        this.panTool.Name = "panTool";
        this.panTool.Size = new System.Drawing.Size(1664, 30);
        this.panTool.TabIndex = 33;
        //
        // btnPickLine
        //
        this.btnPickLine.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnPickLine.Location = new System.Drawing.Point(8, 3);
        this.btnPickLine.Name = "btnPickLine";
        this.btnPickLine.Size = new System.Drawing.Size(160, 24);
        this.btnPickLine.TabIndex = 34;
        this.btnPickLine.Text = "명세서 품목 불러오기";
        this.btnPickLine.ToolTip = "같은 고객의 확정된 거래명세서 라인을 출고 품목으로 가져옵니다(여러 명세서 가능).";
        //
        // btnDelLine
        //
        this.btnDelLine.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnDelLine.Location = new System.Drawing.Point(174, 3);
        this.btnDelLine.Name = "btnDelLine";
        this.btnDelLine.Size = new System.Drawing.Size(90, 24);
        this.btnDelLine.TabIndex = 35;
        this.btnDelLine.Text = "품목 삭제";
        this.btnDelLine.ToolTip = "선택한 출고 품목(과 그 LOT 상세)을 지웁니다.";
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Top;
        this.grd1.Location = new System.Drawing.Point(3, 274);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.Size = new System.Drawing.Size(1664, 170);
        this.grd1.TabIndex = 36;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colSerl,
        this.colInvcNo,
        this.colSoNo,
        this.colSoSerl,
        this.colItemNo,
        this.colItemNm,
        this.colUnit,
        this.colSoQty,
        this.colRemain,
        this.colQty,
        this.colPrice,
        this.colAmt,
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
        this.colSerl.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "serl", "합계 ({0:#,##0}건)")});
        this.colSerl.Visible = true;
        this.colSerl.VisibleIndex = 0;
        this.colSerl.Width = 50;
        //
        // colInvcNo
        //
        this.colInvcNo.Caption = "명세서번호";
        this.colInvcNo.FieldName = "invc_no";
        this.colInvcNo.Name = "colInvcNo";
        this.colInvcNo.OptionsColumn.AllowEdit = false;
        this.colInvcNo.Visible = true;
        this.colInvcNo.VisibleIndex = 1;
        this.colInvcNo.Width = 110;
        //
        // colSoNo
        //
        this.colSoNo.Caption = "수주번호";
        this.colSoNo.FieldName = "so_no";
        this.colSoNo.Name = "colSoNo";
        this.colSoNo.OptionsColumn.AllowEdit = false;
        this.colSoNo.Visible = true;
        this.colSoNo.VisibleIndex = 2;
        this.colSoNo.Width = 110;
        //
        // colSoSerl
        //
        this.colSoSerl.Caption = "수주순번";
        this.colSoSerl.FieldName = "so_serl";
        this.colSoSerl.Name = "colSoSerl";
        this.colSoSerl.OptionsColumn.AllowEdit = false;
        this.colSoSerl.Visible = true;
        this.colSoSerl.VisibleIndex = 3;
        this.colSoSerl.Width = 60;
        //
        // colItemNo
        //
        this.colItemNo.Caption = "품번";
        this.colItemNo.FieldName = "item_no";
        this.colItemNo.Name = "colItemNo";
        this.colItemNo.OptionsColumn.AllowEdit = false;
        this.colItemNo.Visible = true;
        this.colItemNo.VisibleIndex = 4;
        this.colItemNo.Width = 110;
        //
        // colItemNm
        //
        this.colItemNm.Caption = "품명";
        this.colItemNm.FieldName = "item_nm";
        this.colItemNm.Name = "colItemNm";
        this.colItemNm.OptionsColumn.AllowEdit = false;
        this.colItemNm.Visible = true;
        this.colItemNm.VisibleIndex = 5;
        this.colItemNm.Width = 150;
        //
        // colUnit
        //
        this.colUnit.Caption = "단위";
        this.colUnit.FieldName = "unit_cd";
        this.colUnit.Name = "colUnit";
        this.colUnit.OptionsColumn.AllowEdit = false;
        this.colUnit.Visible = true;
        this.colUnit.VisibleIndex = 6;
        this.colUnit.Width = 50;
        //
        // colSoQty
        //
        this.colSoQty.Caption = "명세서수량";
        this.colSoQty.DisplayFormat.FormatString = "#,##0.####";
        this.colSoQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colSoQty.FieldName = "invc_qty";
        this.colSoQty.Name = "colSoQty";
        this.colSoQty.OptionsColumn.AllowEdit = false;
        this.colSoQty.Visible = true;
        this.colSoQty.VisibleIndex = 7;
        this.colSoQty.Width = 85;
        //
        // colRemain
        //
        this.colRemain.Caption = "명세서 잔량";
        this.colRemain.DisplayFormat.FormatString = "#,##0.####";
        this.colRemain.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colRemain.FieldName = "invc_remain_qty";
        this.colRemain.Name = "colRemain";
        this.colRemain.OptionsColumn.AllowEdit = false;
        this.colRemain.Visible = true;
        this.colRemain.VisibleIndex = 8;
        this.colRemain.Width = 85;
        //
        // colQty
        //
        this.colQty.Caption = "출고수량(LOT 합계)";
        this.colQty.DisplayFormat.FormatString = "#,##0.####";
        this.colQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colQty.FieldName = "qty";
        this.colQty.Name = "colQty";
        this.colQty.OptionsColumn.AllowEdit = false;
        this.colQty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "qty", "{0:#,##0.####}")});
        this.colQty.Visible = true;
        this.colQty.VisibleIndex = 9;
        this.colQty.Width = 120;
        //
        // colPrice
        //
        this.colPrice.Caption = "단가";
        this.colPrice.DisplayFormat.FormatString = "#,##0.####";
        this.colPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colPrice.FieldName = "price";
        this.colPrice.Name = "colPrice";
        this.colPrice.OptionsColumn.AllowEdit = false;
        this.colPrice.Visible = true;
        this.colPrice.VisibleIndex = 10;
        this.colPrice.Width = 80;
        //
        // colAmt
        //
        this.colAmt.Caption = "금액";
        this.colAmt.DisplayFormat.FormatString = "#,##0.####";
        this.colAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colAmt.FieldName = "amt";
        this.colAmt.Name = "colAmt";
        this.colAmt.OptionsColumn.AllowEdit = false;
        this.colAmt.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "amt", "{0:#,##0.####}")});
        this.colAmt.Visible = true;
        this.colAmt.VisibleIndex = 11;
        this.colAmt.Width = 100;
        //
        // colRemark
        //
        this.colRemark.Caption = "비고";
        this.colRemark.FieldName = "remark";
        this.colRemark.Name = "colRemark";
        this.colRemark.Visible = true;
        this.colRemark.VisibleIndex = 12;
        this.colRemark.Width = 200;
        //
        // splitterWyn1
        //
        this.splitterWyn1.BackColor = System.Drawing.Color.White;
        this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Top;
        this.splitterWyn1.Location = new System.Drawing.Point(3, 444);
        this.splitterWyn1.Name = "splitterWyn1";
        this.splitterWyn1.Size = new System.Drawing.Size(1664, 10);
        this.splitterWyn1.TabIndex = 37;
        this.splitterWyn1.TabStop = false;
        //
        // shLots
        //
        this.shLots.BackColor = System.Drawing.Color.White;
        this.shLots.Dock = System.Windows.Forms.DockStyle.Top;
        this.shLots.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shLots.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.shLots.Location = new System.Drawing.Point(3, 454);
        this.shLots.Name = "shLots";
        this.shLots.Size = new System.Drawing.Size(1664, 27);
        this.shLots.TabIndex = 38;
        this.shLots.Text = "출고 LOT 상세 (선택한 출고 품목의 LOT / 창고 / 수량)";
        //
        // panTool2
        //
        this.panTool2.Controls.Add(this.btnPickLot);
        this.panTool2.Controls.Add(this.btnDelLot);
        this.panTool2.Dock = System.Windows.Forms.DockStyle.Top;
        this.panTool2.Location = new System.Drawing.Point(3, 481);
        this.panTool2.Name = "panTool2";
        this.panTool2.Size = new System.Drawing.Size(1664, 30);
        this.panTool2.TabIndex = 39;
        //
        // btnPickLot
        //
        this.btnPickLot.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnPickLot.Location = new System.Drawing.Point(8, 3);
        this.btnPickLot.Name = "btnPickLot";
        this.btnPickLot.Size = new System.Drawing.Size(120, 24);
        this.btnPickLot.TabIndex = 40;
        this.btnPickLot.Text = "출고 LOT 선택";
        this.btnPickLot.ToolTip = "선택한 품목의 출고 LOT(재고 있는 창고)를 여러 건 한 번에 추가합니다.";
        //
        // btnDelLot
        //
        this.btnDelLot.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnDelLot.Location = new System.Drawing.Point(134, 3);
        this.btnDelLot.Name = "btnDelLot";
        this.btnDelLot.Size = new System.Drawing.Size(80, 24);
        this.btnDelLot.TabIndex = 41;
        this.btnDelLot.Text = "LOT 삭제";
        this.btnDelLot.ToolTip = "선택한 LOT 행을 지웁니다.";
        //
        // grd2
        //
        this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd2.Location = new System.Drawing.Point(3, 511);
        this.grd2.MainView = this.gvw2;
        this.grd2.Name = "grd2";
        this.grd2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.lookupcolKind});
        this.grd2.Size = new System.Drawing.Size(1664, 235);
        this.grd2.TabIndex = 42;
        this.grd2.UseEmbeddedNavigator = false;
        this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw2});
        //
        // gvw2
        //
        this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colLtSerl,
        this.colLtKind,
        this.colLtLotNo,
        this.colLtWhNm,
        this.colLtStock,
        this.colLtQty,
        this.colLtRemark});
        this.gvw2.GridControl = this.grd2;
        this.gvw2.HighlightFocusedRow = true;
        this.gvw2.Name = "gvw2";
        this.gvw2.OptionsView.ColumnAutoWidth = false;
        this.gvw2.OptionsView.ShowFooter = true;
        this.gvw2.OptionsView.ShowGroupPanel = false;
        //
        // lookupcolKind
        //
        this.lookupcolKind.AutoHeight = false;
        this.lookupcolKind.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.lookupcolKind.LookupKey = "L_SA0001";
        this.lookupcolKind.Name = "lookupcolKind";
        this.lookupcolKind.NullText = "";
        //
        // colLtSerl
        //
        this.colLtSerl.Caption = "순번";
        this.colLtSerl.FieldName = "lot_serl";
        this.colLtSerl.Name = "colLtSerl";
        this.colLtSerl.OptionsColumn.AllowEdit = false;
        this.colLtSerl.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "lot_serl", "합계 ({0:#,##0}건)")});
        this.colLtSerl.Visible = true;
        this.colLtSerl.VisibleIndex = 0;
        this.colLtSerl.Width = 50;
        //
        // colLtKind
        //
        this.colLtKind.Caption = "출고구분";
        this.colLtKind.ColumnEdit = this.lookupcolKind;
        this.colLtKind.FieldName = "ship_kind";
        this.colLtKind.Name = "colLtKind";
        this.colLtKind.OptionsColumn.AllowEdit = false;
        this.colLtKind.Visible = true;
        this.colLtKind.VisibleIndex = 1;
        this.colLtKind.Width = 95;
        //
        // colLtLotNo
        //
        this.colLtLotNo.Caption = "LOT";
        this.colLtLotNo.FieldName = "lot_no";
        this.colLtLotNo.Name = "colLtLotNo";
        this.colLtLotNo.OptionsColumn.AllowEdit = false;
        this.colLtLotNo.Visible = true;
        this.colLtLotNo.VisibleIndex = 2;
        this.colLtLotNo.Width = 160;
        //
        // colLtWhNm
        //
        this.colLtWhNm.Caption = "출고 창고";
        this.colLtWhNm.FieldName = "wh_nm";
        this.colLtWhNm.Name = "colLtWhNm";
        this.colLtWhNm.OptionsColumn.AllowEdit = false;
        this.colLtWhNm.Visible = true;
        this.colLtWhNm.VisibleIndex = 3;
        this.colLtWhNm.Width = 130;
        //
        // colLtStock
        //
        this.colLtStock.Caption = "현재고";
        this.colLtStock.DisplayFormat.FormatString = "#,##0.####";
        this.colLtStock.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colLtStock.FieldName = "stock_qty";
        this.colLtStock.Name = "colLtStock";
        this.colLtStock.OptionsColumn.AllowEdit = false;
        this.colLtStock.Visible = true;
        this.colLtStock.VisibleIndex = 4;
        this.colLtStock.Width = 90;
        //
        // colLtQty
        //
        this.colLtQty.Caption = "출고수량";
        this.colLtQty.DisplayFormat.FormatString = "#,##0.####";
        this.colLtQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colLtQty.FieldName = "qty";
        this.colLtQty.Name = "colLtQty";
        this.colLtQty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "qty", "{0:#,##0.####}")});
        this.colLtQty.Visible = true;
        this.colLtQty.VisibleIndex = 5;
        this.colLtQty.Width = 100;
        //
        // colLtRemark
        //
        this.colLtRemark.Caption = "비고";
        this.colLtRemark.FieldName = "remark";
        this.colLtRemark.Name = "colLtRemark";
        this.colLtRemark.Visible = true;
        this.colLtRemark.VisibleIndex = 6;
        this.colLtRemark.Width = 220;
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblSearchAccId);
        this.panHeader.Controls.Add(this.cboSearchAccId);
        this.panHeader.Controls.Add(this.lblSearchGiNo);
        this.panHeader.Controls.Add(this.txtSearchGiNo);
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
        // lblSearchGiNo
        //
        this.lblSearchGiNo.Location = new System.Drawing.Point(224, 18);
        this.lblSearchGiNo.Name = "lblSearchGiNo";
        this.lblSearchGiNo.Size = new System.Drawing.Size(48, 15);
        this.lblSearchGiNo.TabIndex = 0;
        this.lblSearchGiNo.Text = "출고번호";
        //
        // txtSearchGiNo
        //
        this.txtSearchGiNo.Location = new System.Drawing.Point(289, 15);
        this.txtSearchGiNo.Name = "txtSearchGiNo";
        this.txtSearchGiNo.Size = new System.Drawing.Size(180, 20);
        this.txtSearchGiNo.TabIndex = 1;
        //
        // frmGi
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 800);
        this.Controls.Add(this.panBase);
        this.Name = "frmGi";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).EndInit();
        this.panWork.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
        this.panData.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panTool)).EndInit();
        this.panTool.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panTool2)).EndInit();
        this.panTool2.ResumeLayout(false);
        this.panHeader.PerformLayout();
        this.panData.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.lookupcolKind)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtGiNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteGiDate.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteGiDate.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCustNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteShipDate.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteShipDate.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCarrier.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtBlNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDest.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCustId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchGiNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panWork;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchGiNo;
    private TextEditWyn txtSearchGiNo;
    private SectionHeaderWyn shHeader;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblAccId;
    private LookUpEditWyn cboAccId;
    private DevExpress.XtraEditors.LabelControl lblStatCd;
    private LookUpEditWyn cboStatCd;
    private DevExpress.XtraEditors.LabelControl lblGiNo;
    private TextEditWyn txtGiNo;
    private DevExpress.XtraEditors.LabelControl lblGiDate;
    private DateEditWyn dteGiDate;
    private DevExpress.XtraEditors.LabelControl lblCustNm;
    private PopupLookupEditWyn txtCustNm;
    private DevExpress.XtraEditors.LabelControl lblShipDate;
    private DateEditWyn dteShipDate;
    private DevExpress.XtraEditors.LabelControl lblCarrier;
    private TextEditWyn txtCarrier;
    private DevExpress.XtraEditors.LabelControl lblBlNo;
    private TextEditWyn txtBlNo;
    private DevExpress.XtraEditors.LabelControl lblDest;
    private TextEditWyn txtDest;
    private DevExpress.XtraEditors.LabelControl lblDeptNm;
    private PopupLookupEditWyn txtDeptNm;
    private DevExpress.XtraEditors.LabelControl lblEmpNm;
    private PopupLookupEditWyn txtEmpNm;
    private TextEditWyn txtCustId;
    private TextEditWyn txtDeptId;
    private TextEditWyn txtEmpId;
    private DevExpress.XtraEditors.LabelControl lblRemark;
    private DevExpress.XtraEditors.MemoEdit memoRemark;
    private DevExpress.XtraEditors.LabelControl lblHint;
    private ButtonWyn btnConfirm;
    private ButtonWyn btnConfirmCancel;
    private SectionHeaderWyn shLines;
    private PanelWyn panTool;
    private ButtonWyn btnPickLine;
    private ButtonWyn btnDelLine;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colInvcNo;
    private DevExpress.XtraGrid.Columns.GridColumn colSoNo;
    private DevExpress.XtraGrid.Columns.GridColumn colSoSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colUnit;
    private DevExpress.XtraGrid.Columns.GridColumn colSoQty;
    private DevExpress.XtraGrid.Columns.GridColumn colRemain;
    private DevExpress.XtraGrid.Columns.GridColumn colQty;
    private DevExpress.XtraGrid.Columns.GridColumn colPrice;
    private DevExpress.XtraGrid.Columns.GridColumn colAmt;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private SplitterWyn splitterWyn1;
    private SectionHeaderWyn shLots;
    private PanelWyn panTool2;
    private ButtonWyn btnPickLot;
    private ButtonWyn btnDelLot;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private LookUpColumnEdit lookupcolKind;
    private DevExpress.XtraGrid.Columns.GridColumn colLtSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colLtKind;
    private DevExpress.XtraGrid.Columns.GridColumn colLtLotNo;
    private DevExpress.XtraGrid.Columns.GridColumn colLtWhNm;
    private DevExpress.XtraGrid.Columns.GridColumn colLtStock;
    private DevExpress.XtraGrid.Columns.GridColumn colLtQty;
    private DevExpress.XtraGrid.Columns.GridColumn colLtRemark;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private WYNLAB.Base.Controls.LookUpEditWyn cboSearchAccId;
}
