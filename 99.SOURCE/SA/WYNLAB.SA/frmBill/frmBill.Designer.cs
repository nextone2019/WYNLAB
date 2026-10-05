// 매출등록(frmBill) - 헤더(panData, 고객 기준, 세금계산서 정보) + 매출 품목 그리드(grd1, 확정 명세서 라인 1건당 1행, 부분매출 가능).
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.SA;

public partial class frmBill
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
        this.lblSearchBillNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchBillNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.shHeader = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panData = new WYNLAB.Base.Controls.PanelWyn();
        this.lblAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblStatCd = new DevExpress.XtraEditors.LabelControl();
        this.cboStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblBillNo = new DevExpress.XtraEditors.LabelControl();
        this.txtBillNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblCurCd = new DevExpress.XtraEditors.LabelControl();
        this.txtCurCd = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblExcRate = new DevExpress.XtraEditors.LabelControl();
        this.txtExcRate = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblAmt = new DevExpress.XtraEditors.LabelControl();
        this.txtAmt = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblVat = new DevExpress.XtraEditors.LabelControl();
        this.txtVat = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblTotalAmt = new DevExpress.XtraEditors.LabelControl();
        this.txtTotalAmt = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblKorTotalAmt = new DevExpress.XtraEditors.LabelControl();
        this.txtKorTotalAmt = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblBillDate = new DevExpress.XtraEditors.LabelControl();
        this.dteBillDate = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblCustNm = new DevExpress.XtraEditors.LabelControl();
        this.txtCustNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.lblTaxInvNo = new DevExpress.XtraEditors.LabelControl();
        this.txtTaxInvNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDeptNm = new DevExpress.XtraEditors.LabelControl();
        this.txtDeptNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.lblEmpNm = new DevExpress.XtraEditors.LabelControl();
        this.txtEmpNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
        this.lblTaxInvDate = new DevExpress.XtraEditors.LabelControl();
        this.dteTaxInvDate = new WYNLAB.Base.Controls.DateEditWyn();
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
        this.colInvcSerl = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colSoNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colUnit = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colInvcQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRemain = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colPrice = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colAmt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colVat = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colTotalAmt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCurCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colGiStatus = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchBillNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtBillNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCurCd.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtExcRate.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtAmt.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtVat.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtTotalAmt.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtKorTotalAmt.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteBillDate.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteBillDate.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCustNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtTaxInvNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteTaxInvDate.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteTaxInvDate.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCustId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panTool)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
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
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblSearchAccId);
        this.panHeader.Controls.Add(this.cboSearchAccId);
        this.panHeader.Controls.Add(this.lblSearchBillNo);
        this.panHeader.Controls.Add(this.txtSearchBillNo);
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
        // lblSearchBillNo
        //
        this.lblSearchBillNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchBillNo.Appearance.Options.UseFont = true;
        this.lblSearchBillNo.Location = new System.Drawing.Point(224, 18);
        this.lblSearchBillNo.Name = "lblSearchBillNo";
        this.lblSearchBillNo.Size = new System.Drawing.Size(58, 15);
        this.lblSearchBillNo.TabIndex = 2;
        this.lblSearchBillNo.Text = "매출번호";
        //
        // txtSearchBillNo
        //
        this.txtSearchBillNo.Location = new System.Drawing.Point(296, 15);
        this.txtSearchBillNo.Name = "txtSearchBillNo";
        this.txtSearchBillNo.Size = new System.Drawing.Size(130, 20);
        this.txtSearchBillNo.TabIndex = 3;
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
        this.shHeader.TabIndex = 4;
        this.shHeader.Text = "매출 정보등록";
        //
        // panData
        //
        this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        this.panData.Controls.Add(this.lblAccId);
        this.panData.Controls.Add(this.cboAccId);
        this.panData.Controls.Add(this.lblStatCd);
        this.panData.Controls.Add(this.cboStatCd);
        this.panData.Controls.Add(this.lblBillNo);
        this.panData.Controls.Add(this.txtBillNo);
        this.panData.Controls.Add(this.lblCurCd);
        this.panData.Controls.Add(this.txtCurCd);
        this.panData.Controls.Add(this.lblExcRate);
        this.panData.Controls.Add(this.txtExcRate);
        this.panData.Controls.Add(this.lblAmt);
        this.panData.Controls.Add(this.txtAmt);
        this.panData.Controls.Add(this.lblVat);
        this.panData.Controls.Add(this.txtVat);
        this.panData.Controls.Add(this.lblTotalAmt);
        this.panData.Controls.Add(this.txtTotalAmt);
        this.panData.Controls.Add(this.lblKorTotalAmt);
        this.panData.Controls.Add(this.txtKorTotalAmt);
        this.panData.Controls.Add(this.lblBillDate);
        this.panData.Controls.Add(this.dteBillDate);
        this.panData.Controls.Add(this.lblCustNm);
        this.panData.Controls.Add(this.txtCustNm);
        this.panData.Controls.Add(this.lblTaxInvNo);
        this.panData.Controls.Add(this.txtTaxInvNo);
        this.panData.Controls.Add(this.lblDeptNm);
        this.panData.Controls.Add(this.txtDeptNm);
        this.panData.Controls.Add(this.lblEmpNm);
        this.panData.Controls.Add(this.txtEmpNm);
        this.panData.Controls.Add(this.lblTaxInvDate);
        this.panData.Controls.Add(this.dteTaxInvDate);
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
        this.panData.Size = new System.Drawing.Size(1664, 194);
        this.panData.TabIndex = 31;
        //
        // lblAccId
        //
        this.lblAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblAccId.Appearance.Options.UseFont = true;
        this.lblAccId.Location = new System.Drawing.Point(28, 15);
        this.lblAccId.Name = "lblAccId";
        this.lblAccId.Size = new System.Drawing.Size(44, 15);
        this.lblAccId.TabIndex = 5;
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
        this.cboAccId.TabIndex = 6;
        //
        // lblStatCd
        //
        this.lblStatCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblStatCd.Appearance.Options.UseFont = true;
        this.lblStatCd.Location = new System.Drawing.Point(282, 15);
        this.lblStatCd.Name = "lblStatCd";
        this.lblStatCd.Size = new System.Drawing.Size(58, 15);
        this.lblStatCd.TabIndex = 7;
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
        this.cboStatCd.TabIndex = 8;
        //
        // lblBillNo
        //
        this.lblBillNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblBillNo.Appearance.Options.UseFont = true;
        this.lblBillNo.Location = new System.Drawing.Point(536, 15);
        this.lblBillNo.Name = "lblBillNo";
        this.lblBillNo.Size = new System.Drawing.Size(58, 15);
        this.lblBillNo.TabIndex = 9;
        this.lblBillNo.Text = "매출번호";
        //
        // txtBillNo
        //
        this.txtBillNo.Location = new System.Drawing.Point(608, 12);
        this.txtBillNo.Properties.ReadOnly = true;
        this.txtBillNo.Name = "txtBillNo";
        this.txtBillNo.Size = new System.Drawing.Size(150, 20);
        this.txtBillNo.TabIndex = 10;
        //
        // lblBillDate
        //
        this.lblBillDate.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblBillDate.Appearance.Options.UseFont = true;
        this.lblBillDate.Location = new System.Drawing.Point(28, 43);
        this.lblBillDate.Name = "lblBillDate";
        this.lblBillDate.Size = new System.Drawing.Size(58, 15);
        this.lblBillDate.TabIndex = 11;
        this.lblBillDate.Text = "매출일자";
        //
        // dteBillDate
        //
        this.dteBillDate.Location = new System.Drawing.Point(100, 40);
        this.dteBillDate.Required = true;
        this.dteBillDate.Name = "dteBillDate";
        this.dteBillDate.Size = new System.Drawing.Size(150, 20);
        this.dteBillDate.TabIndex = 12;
        //
        // lblCustNm
        //
        this.lblCustNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblCustNm.Appearance.Options.UseFont = true;
        this.lblCustNm.Location = new System.Drawing.Point(282, 43);
        this.lblCustNm.Name = "lblCustNm";
        this.lblCustNm.Size = new System.Drawing.Size(30, 15);
        this.lblCustNm.TabIndex = 13;
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
        this.txtCustNm.TabIndex = 14;
        //
        // lblTaxInvNo
        //
        this.lblTaxInvNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblTaxInvNo.Appearance.Options.UseFont = true;
        this.lblTaxInvNo.Location = new System.Drawing.Point(536, 43);
        this.lblTaxInvNo.Name = "lblTaxInvNo";
        this.lblTaxInvNo.Size = new System.Drawing.Size(72, 15);
        this.lblTaxInvNo.TabIndex = 15;
        this.lblTaxInvNo.Text = "계산서번호";
        //
        // txtTaxInvNo
        //
        this.txtTaxInvNo.Location = new System.Drawing.Point(608, 40);
        this.txtTaxInvNo.Name = "txtTaxInvNo";
        this.txtTaxInvNo.Size = new System.Drawing.Size(150, 20);
        this.txtTaxInvNo.TabIndex = 16;
        //
        // lblDeptNm
        //
        this.lblDeptNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDeptNm.Appearance.Options.UseFont = true;
        this.lblDeptNm.Location = new System.Drawing.Point(28, 71);
        this.lblDeptNm.Name = "lblDeptNm";
        this.lblDeptNm.Size = new System.Drawing.Size(30, 15);
        this.lblDeptNm.TabIndex = 17;
        this.lblDeptNm.Text = "부서";
        //
        // txtDeptNm
        //
        this.txtDeptNm.Location = new System.Drawing.Point(100, 68);
        this.txtDeptNm.LookupKey = "P_DEPT";
        this.txtDeptNm.MatchField = "dept_nm";
        this.txtDeptNm.Name = "txtDeptNm";
        this.txtDeptNm.Size = new System.Drawing.Size(150, 20);
        this.txtDeptNm.TabIndex = 18;
        //
        // lblEmpNm
        //
        this.lblEmpNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblEmpNm.Appearance.Options.UseFont = true;
        this.lblEmpNm.Location = new System.Drawing.Point(282, 71);
        this.lblEmpNm.Name = "lblEmpNm";
        this.lblEmpNm.Size = new System.Drawing.Size(44, 15);
        this.lblEmpNm.TabIndex = 19;
        this.lblEmpNm.Text = "담당자";
        //
        // txtEmpNm
        //
        this.txtEmpNm.Location = new System.Drawing.Point(354, 68);
        this.txtEmpNm.LookupKey = "P_EMP";
        this.txtEmpNm.MatchField = "emp_nm";
        this.txtEmpNm.Name = "txtEmpNm";
        this.txtEmpNm.Size = new System.Drawing.Size(150, 20);
        this.txtEmpNm.TabIndex = 20;
        //
        // lblTaxInvDate
        //
        this.lblTaxInvDate.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblTaxInvDate.Appearance.Options.UseFont = true;
        this.lblTaxInvDate.Location = new System.Drawing.Point(536, 71);
        this.lblTaxInvDate.Name = "lblTaxInvDate";
        this.lblTaxInvDate.Size = new System.Drawing.Size(72, 15);
        this.lblTaxInvDate.TabIndex = 21;
        this.lblTaxInvDate.Text = "계산서일자";
        //
        // dteTaxInvDate
        //
        this.dteTaxInvDate.Location = new System.Drawing.Point(608, 68);
        this.dteTaxInvDate.Name = "dteTaxInvDate";
        this.dteTaxInvDate.Size = new System.Drawing.Size(150, 20);
        this.dteTaxInvDate.TabIndex = 22;
        //
        // txtCustId
        //
        this.txtCustId.Location = new System.Drawing.Point(1000, 12);
        this.txtCustId.Name = "txtCustId";
        this.txtCustId.Size = new System.Drawing.Size(60, 20);
        this.txtCustId.TabIndex = 23;
        this.txtCustId.Visible = false;
        //
        // txtDeptId
        //
        this.txtDeptId.Location = new System.Drawing.Point(1070, 12);
        this.txtDeptId.Name = "txtDeptId";
        this.txtDeptId.Size = new System.Drawing.Size(60, 20);
        this.txtDeptId.TabIndex = 24;
        this.txtDeptId.Visible = false;
        //
        // txtEmpId
        //
        this.txtEmpId.Location = new System.Drawing.Point(1140, 12);
        this.txtEmpId.Name = "txtEmpId";
        this.txtEmpId.Size = new System.Drawing.Size(60, 20);
        this.txtEmpId.TabIndex = 25;
        this.txtEmpId.Visible = false;
        //
        // lblRemark
        //
        this.lblRemark.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblRemark.Appearance.Options.UseFont = true;
        this.lblRemark.Location = new System.Drawing.Point(28, 99);
        this.lblRemark.Name = "lblRemark";
        this.lblRemark.Size = new System.Drawing.Size(24, 15);
        this.lblRemark.TabIndex = 26;
        this.lblRemark.Text = "비고";
        //
        // memoRemark
        //
        this.memoRemark.Location = new System.Drawing.Point(100, 96);
        this.memoRemark.Name = "memoRemark";
        this.memoRemark.Size = new System.Drawing.Size(674, 40);
        this.memoRemark.TabIndex = 27;
        //
        // lblCurCd
        //
        this.lblCurCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblCurCd.Appearance.Options.UseFont = true;
        this.lblCurCd.Location = new System.Drawing.Point(28, 147);
        this.lblCurCd.Name = "lblCurCd";
        this.lblCurCd.Size = new System.Drawing.Size(48, 15);
        this.lblCurCd.TabIndex = 40;
        this.lblCurCd.Text = "통화";
        //
        // txtCurCd
        //
        this.txtCurCd.Location = new System.Drawing.Point(100, 144);
        this.txtCurCd.Name = "txtCurCd";
        this.txtCurCd.Properties.ReadOnly = true;
        this.txtCurCd.Properties.Appearance.Options.UseTextOptions = true;
        this.txtCurCd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
        this.txtCurCd.Size = new System.Drawing.Size(60, 20);
        this.txtCurCd.TabIndex = 41;
        //
        // lblExcRate
        //
        this.lblExcRate.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblExcRate.Appearance.Options.UseFont = true;
        this.lblExcRate.Location = new System.Drawing.Point(282, 147);
        this.lblExcRate.Name = "lblExcRate";
        this.lblExcRate.Size = new System.Drawing.Size(48, 15);
        this.lblExcRate.TabIndex = 42;
        this.lblExcRate.Text = "환율";
        //
        // txtExcRate
        //
        this.txtExcRate.Location = new System.Drawing.Point(354, 144);
        this.txtExcRate.Name = "txtExcRate";
        this.txtExcRate.Properties.Appearance.Options.UseTextOptions = true;
        this.txtExcRate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
        this.txtExcRate.Size = new System.Drawing.Size(100, 20);
        this.txtExcRate.TabIndex = 43;
        //
        // lblAmt
        //
        this.lblAmt.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblAmt.Appearance.Options.UseFont = true;
        this.lblAmt.Location = new System.Drawing.Point(536, 147);
        this.lblAmt.Name = "lblAmt";
        this.lblAmt.Size = new System.Drawing.Size(48, 15);
        this.lblAmt.TabIndex = 44;
        this.lblAmt.Text = "공급가액";
        //
        // txtAmt
        //
        this.txtAmt.Location = new System.Drawing.Point(608, 144);
        this.txtAmt.Name = "txtAmt";
        this.txtAmt.Properties.ReadOnly = true;
        this.txtAmt.Properties.Appearance.Options.UseTextOptions = true;
        this.txtAmt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
        this.txtAmt.Size = new System.Drawing.Size(150, 20);
        this.txtAmt.TabIndex = 45;
        //
        // lblVat
        //
        this.lblVat.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblVat.Appearance.Options.UseFont = true;
        this.lblVat.Location = new System.Drawing.Point(790, 147);
        this.lblVat.Name = "lblVat";
        this.lblVat.Size = new System.Drawing.Size(48, 15);
        this.lblVat.TabIndex = 46;
        this.lblVat.Text = "부가세";
        //
        // txtVat
        //
        this.txtVat.Location = new System.Drawing.Point(844, 144);
        this.txtVat.Name = "txtVat";
        this.txtVat.Properties.ReadOnly = true;
        this.txtVat.Properties.Appearance.Options.UseTextOptions = true;
        this.txtVat.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
        this.txtVat.Size = new System.Drawing.Size(110, 20);
        this.txtVat.TabIndex = 47;
        //
        // lblTotalAmt
        //
        this.lblTotalAmt.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblTotalAmt.Appearance.Options.UseFont = true;
        this.lblTotalAmt.Location = new System.Drawing.Point(974, 147);
        this.lblTotalAmt.Name = "lblTotalAmt";
        this.lblTotalAmt.Size = new System.Drawing.Size(48, 15);
        this.lblTotalAmt.TabIndex = 48;
        this.lblTotalAmt.Text = "합계금액";
        //
        // txtTotalAmt
        //
        this.txtTotalAmt.Location = new System.Drawing.Point(1034, 144);
        this.txtTotalAmt.Name = "txtTotalAmt";
        this.txtTotalAmt.Properties.ReadOnly = true;
        this.txtTotalAmt.Properties.Appearance.Options.UseTextOptions = true;
        this.txtTotalAmt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
        this.txtTotalAmt.Size = new System.Drawing.Size(120, 20);
        this.txtTotalAmt.TabIndex = 49;
        //
        // lblKorTotalAmt
        //
        this.lblKorTotalAmt.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblKorTotalAmt.Appearance.Options.UseFont = true;
        this.lblKorTotalAmt.Location = new System.Drawing.Point(1174, 147);
        this.lblKorTotalAmt.Name = "lblKorTotalAmt";
        this.lblKorTotalAmt.Size = new System.Drawing.Size(48, 15);
        this.lblKorTotalAmt.TabIndex = 50;
        this.lblKorTotalAmt.Text = "원화합계";
        //
        // txtKorTotalAmt
        //
        this.txtKorTotalAmt.Location = new System.Drawing.Point(1234, 144);
        this.txtKorTotalAmt.Name = "txtKorTotalAmt";
        this.txtKorTotalAmt.Properties.ReadOnly = true;
        this.txtKorTotalAmt.Properties.Appearance.Options.UseTextOptions = true;
        this.txtKorTotalAmt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
        this.txtKorTotalAmt.Size = new System.Drawing.Size(130, 20);
        this.txtKorTotalAmt.TabIndex = 51;
        //
        // lblHint
        //
        this.lblHint.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblHint.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
        this.lblHint.Appearance.Options.UseFont = true;
        this.lblHint.Appearance.Options.UseForeColor = true;
        this.lblHint.Location = new System.Drawing.Point(28, 172);
        this.lblHint.Name = "lblHint";
        this.lblHint.Size = new System.Drawing.Size(60, 15);
        this.lblHint.TabIndex = 28;
        this.lblHint.Text = "※ 고객을 고르고 [명세서 품목 불러오기]로 확정된 거래명세서 품목(같은 고객의 여러 명세서 가능 - 월합계 계산서)을 가져와 매출수량을 조정(부분매출) → 저장 → [확정]하면 명세서 매출누계가 갱신됩니다. 출고 여부와 무관하게 확정된 명세서면 매출을 등록할 수 있습니다. 외화 매출의 환율은 매출일 기준 환율정보가 자동으로 적용되며 필요하면 직접 고칠 수 있습니다(거래명세서는 수주 환율 그대로).";
        //
        // btnConfirm
        //
        this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnConfirm.Location = new System.Drawing.Point(820, 12);
        this.btnConfirm.Name = "btnConfirm";
        this.btnConfirm.Size = new System.Drawing.Size(170, 24);
        this.btnConfirm.TabIndex = 29;
        this.btnConfirm.Text = "확정";
        this.btnConfirm.ToolTip = "매출을 확정합니다. 확정 후에는 수정할 수 없습니다.";
        //
        // btnConfirmCancel
        //
        this.btnConfirmCancel.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnConfirmCancel.Location = new System.Drawing.Point(820, 40);
        this.btnConfirmCancel.Name = "btnConfirmCancel";
        this.btnConfirmCancel.Size = new System.Drawing.Size(170, 24);
        this.btnConfirmCancel.TabIndex = 30;
        this.btnConfirmCancel.Text = "확정취소";
        this.btnConfirmCancel.ToolTip = "확정을 취소하고 명세서 매출누계를 되돌립니다.";
        //
        // shLines
        //
        this.shLines.BackColor = System.Drawing.Color.White;
        this.shLines.Dock = System.Windows.Forms.DockStyle.Top;
        this.shLines.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shLines.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.shLines.Location = new System.Drawing.Point(3, 193);
        this.shLines.Name = "shLines";
        this.shLines.Size = new System.Drawing.Size(1664, 27);
        this.shLines.TabIndex = 32;
        this.shLines.Text = "매출 품목 (확정 거래명세서 라인 1건 = 1행, 수량만 조정하면 부분매출 - 단가/세율은 명세서 값)";
        //
        // panTool
        //
        this.panTool.Controls.Add(this.btnPickLine);
        this.panTool.Controls.Add(this.btnDelLine);
        this.panTool.Dock = System.Windows.Forms.DockStyle.Top;
        this.panTool.Location = new System.Drawing.Point(3, 220);
        this.panTool.Name = "panTool";
        this.panTool.Size = new System.Drawing.Size(1664, 30);
        this.panTool.TabIndex = 35;
        //
        // btnPickLine
        //
        this.btnPickLine.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnPickLine.Location = new System.Drawing.Point(8, 3);
        this.btnPickLine.Name = "btnPickLine";
        this.btnPickLine.Size = new System.Drawing.Size(170, 24);
        this.btnPickLine.TabIndex = 33;
        this.btnPickLine.Text = "명세서 품목 불러오기";
        this.btnPickLine.ToolTip = "확정된 거래명세서 라인을 매출 품목으로 가져옵니다(같은 고객의 여러 명세서 가능).";
        //
        // btnDelLine
        //
        this.btnDelLine.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnDelLine.Location = new System.Drawing.Point(184, 3);
        this.btnDelLine.Name = "btnDelLine";
        this.btnDelLine.Size = new System.Drawing.Size(90, 24);
        this.btnDelLine.TabIndex = 34;
        this.btnDelLine.Text = "품목 삭제";
        this.btnDelLine.ToolTip = "선택한 품목 행을 지웁니다.";
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd1.Location = new System.Drawing.Point(3, 250);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.Size = new System.Drawing.Size(1664, 496);
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
        this.colInvcSerl,
        this.colSoNo,
        this.colItemNo,
        this.colItemNm,
        this.colUnit,
        this.colInvcQty,
        this.colRemain,
        this.colQty,
        this.colPrice,
        this.colAmt,
        this.colVat,
        this.colTotalAmt,
        this.colCurCd,
        this.colGiStatus,
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
        // colInvcSerl
        //
        this.colInvcSerl.Caption = "명세서순번";
        this.colInvcSerl.FieldName = "invc_serl";
        this.colInvcSerl.Name = "colInvcSerl";
        this.colInvcSerl.OptionsColumn.AllowEdit = false;
        this.colInvcSerl.Visible = true;
        this.colInvcSerl.VisibleIndex = 2;
        this.colInvcSerl.Width = 70;
        //
        // colSoNo
        //
        this.colSoNo.Caption = "수주번호";
        this.colSoNo.FieldName = "so_no";
        this.colSoNo.Name = "colSoNo";
        this.colSoNo.OptionsColumn.AllowEdit = false;
        this.colSoNo.Visible = true;
        this.colSoNo.VisibleIndex = 3;
        this.colSoNo.Width = 110;
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
        // colInvcQty
        //
        this.colInvcQty.Caption = "명세서수량";
        this.colInvcQty.DisplayFormat.FormatString = "#,##0.####";
        this.colInvcQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colInvcQty.FieldName = "invc_qty";
        this.colInvcQty.Name = "colInvcQty";
        this.colInvcQty.OptionsColumn.AllowEdit = false;
        this.colInvcQty.Visible = true;
        this.colInvcQty.VisibleIndex = 7;
        this.colInvcQty.Width = 90;
        //
        // colRemain
        //
        this.colRemain.Caption = "매출 가능";
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
        this.colQty.Caption = "매출수량";
        this.colQty.DisplayFormat.FormatString = "#,##0.####";
        this.colQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colQty.FieldName = "qty";
        this.colQty.Name = "colQty";
        this.colQty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "qty", "{0:#,##0.####}")});
        this.colQty.Visible = true;
        this.colQty.VisibleIndex = 9;
        this.colQty.Width = 95;
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
        this.colPrice.Width = 90;
        //
        // colAmt
        //
        this.colAmt.Caption = "공급가";
        this.colAmt.DisplayFormat.FormatString = "#,##0.####";
        this.colAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colAmt.FieldName = "amt";
        this.colAmt.Name = "colAmt";
        this.colAmt.OptionsColumn.AllowEdit = false;
        this.colAmt.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "amt", "{0:#,##0.####}")});
        this.colAmt.Visible = true;
        this.colAmt.VisibleIndex = 11;
        this.colAmt.Width = 110;
        //
        // colVat
        //
        this.colVat.Caption = "부가세";
        this.colVat.DisplayFormat.FormatString = "#,##0.####";
        this.colVat.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colVat.FieldName = "vat";
        this.colVat.Name = "colVat";
        this.colVat.OptionsColumn.AllowEdit = false;
        this.colVat.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "vat", "{0:#,##0.####}")});
        this.colVat.Visible = true;
        this.colVat.VisibleIndex = 12;
        this.colVat.Width = 100;
        //
        // colTotalAmt
        //
        this.colTotalAmt.Caption = "합계금액";
        this.colTotalAmt.DisplayFormat.FormatString = "#,##0.####";
        this.colTotalAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colTotalAmt.FieldName = "total_amt";
        this.colTotalAmt.Name = "colTotalAmt";
        this.colTotalAmt.OptionsColumn.AllowEdit = false;
        this.colTotalAmt.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
        new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "total_amt", "{0:#,##0.####}")});
        this.colTotalAmt.Visible = true;
        this.colTotalAmt.VisibleIndex = 13;
        this.colTotalAmt.Width = 110;
        //
        // colCurCd
        //
        this.colCurCd.Caption = "통화";
        this.colCurCd.FieldName = "cur_cd";
        this.colCurCd.Name = "colCurCd";
        this.colCurCd.OptionsColumn.AllowEdit = false;
        this.colCurCd.Visible = true;
        this.colCurCd.VisibleIndex = 14;
        this.colCurCd.Width = 50;
        //
        // colGiStatus
        //
        this.colGiStatus.Caption = "출고상태";
        this.colGiStatus.FieldName = "gi_status";
        this.colGiStatus.Name = "colGiStatus";
        this.colGiStatus.OptionsColumn.AllowEdit = false;
        this.colGiStatus.Visible = true;
        this.colGiStatus.VisibleIndex = 15;
        this.colGiStatus.Width = 80;
        //
        // colRemark
        //
        this.colRemark.Caption = "비고";
        this.colRemark.FieldName = "remark";
        this.colRemark.Name = "colRemark";
        this.colRemark.Visible = true;
        this.colRemark.VisibleIndex = 16;
        this.colRemark.Width = 200;
        //
        // frmBill
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 800);
        this.Controls.Add(this.panBase);
        this.Name = "frmBill";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchBillNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboStatCd.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtBillNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCurCd.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtExcRate.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtAmt.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtVat.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtTotalAmt.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtKorTotalAmt.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteBillDate.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteBillDate.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCustNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtTaxInvNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteTaxInvDate.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteTaxInvDate.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtCustId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtEmpId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.memoRemark.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panTool)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panWork;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private LookUpEditWyn cboSearchAccId;
    private DevExpress.XtraEditors.LabelControl lblSearchBillNo;
    private TextEditWyn txtSearchBillNo;
    private SectionHeaderWyn shHeader;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblAccId;
    private LookUpEditWyn cboAccId;
    private DevExpress.XtraEditors.LabelControl lblStatCd;
    private LookUpEditWyn cboStatCd;
    private DevExpress.XtraEditors.LabelControl lblBillNo;
    private TextEditWyn txtBillNo;
    private DevExpress.XtraEditors.LabelControl lblCurCd;
    private TextEditWyn txtCurCd;
    private DevExpress.XtraEditors.LabelControl lblExcRate;
    private TextEditWyn txtExcRate;
    private DevExpress.XtraEditors.LabelControl lblAmt;
    private TextEditWyn txtAmt;
    private DevExpress.XtraEditors.LabelControl lblVat;
    private TextEditWyn txtVat;
    private DevExpress.XtraEditors.LabelControl lblTotalAmt;
    private TextEditWyn txtTotalAmt;
    private DevExpress.XtraEditors.LabelControl lblKorTotalAmt;
    private TextEditWyn txtKorTotalAmt;
    private DevExpress.XtraEditors.LabelControl lblBillDate;
    private DateEditWyn dteBillDate;
    private DevExpress.XtraEditors.LabelControl lblCustNm;
    private PopupLookupEditWyn txtCustNm;
    private DevExpress.XtraEditors.LabelControl lblTaxInvNo;
    private TextEditWyn txtTaxInvNo;
    private DevExpress.XtraEditors.LabelControl lblDeptNm;
    private PopupLookupEditWyn txtDeptNm;
    private DevExpress.XtraEditors.LabelControl lblEmpNm;
    private PopupLookupEditWyn txtEmpNm;
    private DevExpress.XtraEditors.LabelControl lblTaxInvDate;
    private DateEditWyn dteTaxInvDate;
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
    private DevExpress.XtraGrid.Columns.GridColumn colInvcSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colSoNo;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colUnit;
    private DevExpress.XtraGrid.Columns.GridColumn colInvcQty;
    private DevExpress.XtraGrid.Columns.GridColumn colRemain;
    private DevExpress.XtraGrid.Columns.GridColumn colQty;
    private DevExpress.XtraGrid.Columns.GridColumn colPrice;
    private DevExpress.XtraGrid.Columns.GridColumn colAmt;
    private DevExpress.XtraGrid.Columns.GridColumn colVat;
    private DevExpress.XtraGrid.Columns.GridColumn colTotalAmt;
    private DevExpress.XtraGrid.Columns.GridColumn colCurCd;
    private DevExpress.XtraGrid.Columns.GridColumn colGiStatus;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
}
