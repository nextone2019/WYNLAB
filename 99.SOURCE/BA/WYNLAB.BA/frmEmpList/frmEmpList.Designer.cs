// AI Builder가 싱글그리드 템플릿을 복제해서 자동 생성 - 2026-09-03.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
#nullable disable
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmEmpList
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmpList));
            this.panBody = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.col1AccCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1EmpNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1EmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1EmpNmEng = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1DeptCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1DeptNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1EntDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1GrpEntDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1JobGrade = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1JobType = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1RetYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1RetDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1SexCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1Tel = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1HpTel = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1Email = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1NatCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1ZipCode = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1Addr1 = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1Addr2 = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1HoliYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1DiligYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1PayYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.chkEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.txtEmpNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.txtDeptCd = new WYNLAB.Base.Controls.TextEditWyn();
            this.paTitleH = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.paTitle1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBody)).BeginInit();
            this.panBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).BeginInit();
            this.paTitleH.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).BeginInit();
            this.paTitle1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panBody
            // 
            this.panBody.Controls.Add(this.grd1);
            this.panBody.Controls.Add(this.paTitle1);
            this.panBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBody.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panBody.Location = new System.Drawing.Point(5, 90);
            this.panBody.Name = "panBody";
            this.panBody.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panBody.Size = new System.Drawing.Size(1235, 485);
            this.panBody.TabIndex = 1;
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
            this.grd1.Size = new System.Drawing.Size(1235, 450);
            this.grd1.TabIndex = 1;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            //
            // gvw1
            //
        //
        // col1AccCd
        //
        this.col1AccCd.Caption = "계정코드";
        this.col1AccCd.FieldName = "acc_cd";
        this.col1AccCd.Name = "col1AccCd";
        this.col1AccCd.Visible = true;
        this.col1AccCd.VisibleIndex = 0;
        this.col1AccCd.Width = 100;
        //
        // col1EmpNo
        //
        this.col1EmpNo.Caption = "사원번호";
        this.col1EmpNo.FieldName = "emp_no";
        this.col1EmpNo.Name = "col1EmpNo";
        this.col1EmpNo.OptionsColumn.AllowEdit = false;
        this.col1EmpNo.Visible = true;
        this.col1EmpNo.VisibleIndex = 1;
        this.col1EmpNo.Width = 100;
        //
        // col1EmpNm
        //
        this.col1EmpNm.Caption = "사원명";
        this.col1EmpNm.FieldName = "emp_nm";
        this.col1EmpNm.Name = "col1EmpNm";
        this.col1EmpNm.Visible = true;
        this.col1EmpNm.VisibleIndex = 2;
        this.col1EmpNm.Width = 100;
        //
        // col1EmpNmEng
        //
        this.col1EmpNmEng.Caption = "영문성명";
        this.col1EmpNmEng.FieldName = "emp_nm_eng";
        this.col1EmpNmEng.Name = "col1EmpNmEng";
        this.col1EmpNmEng.Visible = true;
        this.col1EmpNmEng.VisibleIndex = 3;
        this.col1EmpNmEng.Width = 100;
        //
        // col1DeptCd
        //
        this.col1DeptCd.Caption = "부서코드";
        this.col1DeptCd.FieldName = "dept_cd";
        this.col1DeptCd.Name = "col1DeptCd";
        this.col1DeptCd.Visible = true;
        this.col1DeptCd.VisibleIndex = 4;
        this.col1DeptCd.Width = 100;
        //
        // col1DeptNm
        //
        this.col1DeptNm.Caption = "부서명";
        this.col1DeptNm.FieldName = "dept_nm";
        this.col1DeptNm.Name = "col1DeptNm";
        this.col1DeptNm.Visible = true;
        this.col1DeptNm.VisibleIndex = 5;
        this.col1DeptNm.Width = 100;
        //
        // col1EntDate
        //
        this.col1EntDate.Caption = "입사일";
        this.col1EntDate.FieldName = "ent_date";
        this.col1EntDate.Name = "col1EntDate";
        this.col1EntDate.Visible = true;
        this.col1EntDate.VisibleIndex = 6;
        this.col1EntDate.Width = 100;
        //
        // col1GrpEntDate
        //
        this.col1GrpEntDate.Caption = "그룹입사일";
        this.col1GrpEntDate.FieldName = "grp_ent_date";
        this.col1GrpEntDate.Name = "col1GrpEntDate";
        this.col1GrpEntDate.Visible = true;
        this.col1GrpEntDate.VisibleIndex = 7;
        this.col1GrpEntDate.Width = 100;
        //
        // col1JobGrade
        //
        this.col1JobGrade.Caption = "직급";
        this.col1JobGrade.FieldName = "job_grade";
        this.col1JobGrade.Name = "col1JobGrade";
        this.col1JobGrade.Visible = true;
        this.col1JobGrade.VisibleIndex = 8;
        this.col1JobGrade.Width = 100;
        //
        // col1JobType
        //
        this.col1JobType.Caption = "직종";
        this.col1JobType.FieldName = "job_type";
        this.col1JobType.Name = "col1JobType";
        this.col1JobType.Visible = true;
        this.col1JobType.VisibleIndex = 9;
        this.col1JobType.Width = 100;
        //
        // col1RetYn
        //
        this.col1RetYn.Caption = "퇴사여부";
        this.col1RetYn.FieldName = "ret_yn";
        this.col1RetYn.Name = "col1RetYn";
        this.col1RetYn.ColumnEdit = this.chkEdit1;
        this.col1RetYn.Visible = true;
        this.col1RetYn.VisibleIndex = 10;
        this.col1RetYn.Width = 100;
        //
        // col1RetDate
        //
        this.col1RetDate.Caption = "퇴사일";
        this.col1RetDate.FieldName = "ret_date";
        this.col1RetDate.Name = "col1RetDate";
        this.col1RetDate.Visible = true;
        this.col1RetDate.VisibleIndex = 11;
        this.col1RetDate.Width = 100;
        //
        // col1SexCd
        //
        this.col1SexCd.Caption = "성별코드";
        this.col1SexCd.FieldName = "sex_cd";
        this.col1SexCd.Name = "col1SexCd";
        this.col1SexCd.Visible = true;
        this.col1SexCd.VisibleIndex = 12;
        this.col1SexCd.Width = 100;
        //
        // col1Tel
        //
        this.col1Tel.Caption = "전화번호";
        this.col1Tel.FieldName = "tel";
        this.col1Tel.Name = "col1Tel";
        this.col1Tel.Visible = true;
        this.col1Tel.VisibleIndex = 13;
        this.col1Tel.Width = 100;
        //
        // col1HpTel
        //
        this.col1HpTel.Caption = "휴대폰";
        this.col1HpTel.FieldName = "hp_tel";
        this.col1HpTel.Name = "col1HpTel";
        this.col1HpTel.Visible = true;
        this.col1HpTel.VisibleIndex = 14;
        this.col1HpTel.Width = 100;
        //
        // col1Email
        //
        this.col1Email.Caption = "이메일";
        this.col1Email.FieldName = "email";
        this.col1Email.Name = "col1Email";
        this.col1Email.Visible = true;
        this.col1Email.VisibleIndex = 15;
        this.col1Email.Width = 100;
        //
        // col1NatCd
        //
        this.col1NatCd.Caption = "국적코드";
        this.col1NatCd.FieldName = "nat_cd";
        this.col1NatCd.Name = "col1NatCd";
        this.col1NatCd.Visible = true;
        this.col1NatCd.VisibleIndex = 16;
        this.col1NatCd.Width = 100;
        //
        // col1ZipCode
        //
        this.col1ZipCode.Caption = "우편번호";
        this.col1ZipCode.FieldName = "zip_code";
        this.col1ZipCode.Name = "col1ZipCode";
        this.col1ZipCode.Visible = true;
        this.col1ZipCode.VisibleIndex = 17;
        this.col1ZipCode.Width = 100;
        //
        // col1Addr1
        //
        this.col1Addr1.Caption = "주소1";
        this.col1Addr1.FieldName = "addr1";
        this.col1Addr1.Name = "col1Addr1";
        this.col1Addr1.Visible = true;
        this.col1Addr1.VisibleIndex = 18;
        this.col1Addr1.Width = 100;
        //
        // col1Addr2
        //
        this.col1Addr2.Caption = "주소2";
        this.col1Addr2.FieldName = "addr2";
        this.col1Addr2.Name = "col1Addr2";
        this.col1Addr2.Visible = true;
        this.col1Addr2.VisibleIndex = 19;
        this.col1Addr2.Width = 100;
        //
        // col1HoliYn
        //
        this.col1HoliYn.Caption = "휴일여부";
        this.col1HoliYn.FieldName = "holi_yn";
        this.col1HoliYn.Name = "col1HoliYn";
        this.col1HoliYn.ColumnEdit = this.chkEdit1;
        this.col1HoliYn.Visible = true;
        this.col1HoliYn.VisibleIndex = 20;
        this.col1HoliYn.Width = 100;
        //
        // col1DiligYn
        //
        this.col1DiligYn.Caption = "개근여부";
        this.col1DiligYn.FieldName = "dilig_yn";
        this.col1DiligYn.Name = "col1DiligYn";
        this.col1DiligYn.ColumnEdit = this.chkEdit1;
        this.col1DiligYn.Visible = true;
        this.col1DiligYn.VisibleIndex = 21;
        this.col1DiligYn.Width = 100;
        //
        // col1PayYn
        //
        this.col1PayYn.Caption = "급여여부";
        this.col1PayYn.FieldName = "pay_yn";
        this.col1PayYn.Name = "col1PayYn";
        this.col1PayYn.ColumnEdit = this.chkEdit1;
        this.col1PayYn.Visible = true;
        this.col1PayYn.VisibleIndex = 22;
        this.col1PayYn.Width = 100;
        //
        // chkEdit1
        //
        this.chkEdit1.Name = "chkEdit1";
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col1AccCd,
            this.col1EmpNo,
            this.col1EmpNm,
            this.col1EmpNmEng,
            this.col1DeptCd,
            this.col1DeptNm,
            this.col1EntDate,
            this.col1GrpEntDate,
            this.col1JobGrade,
            this.col1JobType,
            this.col1RetYn,
            this.col1RetDate,
            this.col1SexCd,
            this.col1Tel,
            this.col1HpTel,
            this.col1Email,
            this.col1NatCd,
            this.col1ZipCode,
            this.col1Addr1,
            this.col1Addr2,
            this.col1HoliYn,
            this.col1DiligYn,
            this.col1PayYn});
        this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { this.chkEdit1});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            //
            // panHeader
            //
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 30);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1235, 60);
            this.panHeader.TabIndex = 0;
        this.txtEmpNo.Location = new System.Drawing.Point(16, 20);
        this.txtEmpNo.Name = "txtEmpNo";
        this.txtEmpNo.Size = new System.Drawing.Size(150, 20);
        this.panHeader.Controls.Add(this.txtEmpNo);
        this.txtDeptCd.Location = new System.Drawing.Point(182, 20);
        this.txtDeptCd.Name = "txtDeptCd";
        this.txtDeptCd.Size = new System.Drawing.Size(150, 20);
        this.panHeader.Controls.Add(this.txtDeptCd);
            //
            // paTitleH
            // 
            this.paTitleH.Controls.Add(this.sectionHeaderWyn1);
            this.paTitleH.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitleH.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitleH.Location = new System.Drawing.Point(5, 5);
            this.paTitleH.Name = "paTitleH";
            this.paTitleH.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.paTitleH.Size = new System.Drawing.Size(1235, 25);
            this.paTitleH.TabIndex = 6;
            // 
            // sectionHeaderWyn1
            // 
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Dock = System.Windows.Forms.DockStyle.Left;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(259, 25);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 8;
            this.sectionHeaderWyn1.Text = "FormName [frm]";
            // 
            // paTitle1
            // 
            this.paTitle1.Controls.Add(this.sectionHeaderWyn4);
            this.paTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle1.Location = new System.Drawing.Point(0, 8);
            this.paTitle1.Name = "paTitle1";
            this.paTitle1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.paTitle1.Size = new System.Drawing.Size(1235, 27);
            this.paTitle1.TabIndex = 12;
            // 
            // sectionHeaderWyn4
            // 
            this.sectionHeaderWyn4.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn4.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn4.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn4.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn4.Name = "sectionHeaderWyn4";
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(1230, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 8;
            this.sectionHeaderWyn4.Text = "LIST";
            // 
            // frmEmpList
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1245, 580);
            this.Controls.Add(this.panBody);
            this.Controls.Add(this.panHeader);
            this.Controls.Add(this.paTitleH);
            this.Name = "frmEmpList";
            this.Padding = new System.Windows.Forms.Padding(5);
            ((System.ComponentModel.ISupportInitialize)(this.panBody)).EndInit();
            this.panBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).EndInit();
            this.paTitleH.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).EndInit();
            this.paTitle1.ResumeLayout(false);
            this.ResumeLayout(false);

    }
    private PanelWyn panHeader;
    private PanelWyn panBody;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private PanelWyn paTitle1;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn paTitleH;
    private SectionHeaderWyn sectionHeaderWyn1;
    private TextEditWyn txtEmpNo;
    private TextEditWyn txtDeptCd;
    private DevExpress.XtraGrid.Columns.GridColumn col1AccCd;
    private DevExpress.XtraGrid.Columns.GridColumn col1EmpNo;
    private DevExpress.XtraGrid.Columns.GridColumn col1EmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn col1EmpNmEng;
    private DevExpress.XtraGrid.Columns.GridColumn col1DeptCd;
    private DevExpress.XtraGrid.Columns.GridColumn col1DeptNm;
    private DevExpress.XtraGrid.Columns.GridColumn col1EntDate;
    private DevExpress.XtraGrid.Columns.GridColumn col1GrpEntDate;
    private DevExpress.XtraGrid.Columns.GridColumn col1JobGrade;
    private DevExpress.XtraGrid.Columns.GridColumn col1JobType;
    private DevExpress.XtraGrid.Columns.GridColumn col1RetYn;
    private DevExpress.XtraGrid.Columns.GridColumn col1RetDate;
    private DevExpress.XtraGrid.Columns.GridColumn col1SexCd;
    private DevExpress.XtraGrid.Columns.GridColumn col1Tel;
    private DevExpress.XtraGrid.Columns.GridColumn col1HpTel;
    private DevExpress.XtraGrid.Columns.GridColumn col1Email;
    private DevExpress.XtraGrid.Columns.GridColumn col1NatCd;
    private DevExpress.XtraGrid.Columns.GridColumn col1ZipCode;
    private DevExpress.XtraGrid.Columns.GridColumn col1Addr1;
    private DevExpress.XtraGrid.Columns.GridColumn col1Addr2;
    private DevExpress.XtraGrid.Columns.GridColumn col1HoliYn;
    private DevExpress.XtraGrid.Columns.GridColumn col1DiligYn;
    private DevExpress.XtraGrid.Columns.GridColumn col1PayYn;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkEdit1;
}
