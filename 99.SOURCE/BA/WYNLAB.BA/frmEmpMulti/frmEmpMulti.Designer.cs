// AI Builder가 싱글그리드 템플릿을 복제해서 자동 생성 - 2026-09-19.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmEmpMulti
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmpMulti));
            this.panBody = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.col1EmpId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1AccId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpcol1AccId = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.col1EmpNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1EmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1EmpNmEng = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1DeptId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spinEditcol1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.col1DeptNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.popcol1DeptNm = new WYNLAB.Base.Controls.PopupLookupColumnEdit();
            this.col1EntDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dateEditcol1 = new WYNLAB.Base.Controls.DateColumnEdit();
            this.col1GrpEntDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1JobGrade = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1JobType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.col1RetYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1RetDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1SexCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpcol1SexCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
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
            this.col1Photo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1ValidateResult = new DevExpress.XtraGrid.Columns.GridColumn();
            this.paTitle1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.paTitleH = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.lookUpColumnEdit1 = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.btnValidate = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnUploadExcel = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnDownloadTemplate = new WYNLAB.Base.Controls.ButtonWyn();
            this.cboAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panBody)).BeginInit();
            this.panBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcol1AccId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditcol1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcol1DeptNm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcol1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcol1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcol1SexCd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).BeginInit();
            this.paTitle1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).BeginInit();
            this.paTitleH.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpColumnEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).BeginInit();
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
            this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.chkEdit1,
            this.spinEditcol1,
            this.dateEditcol1,
            this.lookUpcol1AccId,
            this.lookUpcol1SexCd,
            this.popcol1DeptNm,
            this.lookUpColumnEdit1});
            this.grd1.Size = new System.Drawing.Size(1235, 450);
            this.grd1.TabIndex = 1;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col1EmpId,
            this.col1AccId,
            this.col1EmpNo,
            this.col1EmpNm,
            this.col1EmpNmEng,
            this.col1DeptId,
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
            this.col1PayYn,
            this.col1Photo,
            this.col1ValidateResult});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // col1EmpId
            // 
            this.col1EmpId.Caption = "사원ID";
            this.col1EmpId.FieldName = "EMP_ID";
            this.col1EmpId.Name = "col1EmpId";
            this.col1EmpId.Visible = false;
            this.col1EmpId.Width = 55;
            // 
            // col1AccId
            // 
            this.col1AccId.Caption = "사업장";
            this.col1AccId.ColumnEdit = this.lookUpcol1AccId;
            this.col1AccId.FieldName = "acc_id";
            this.col1AccId.Name = "col1AccId";
            this.col1AccId.Visible = false;
            this.col1AccId.Width = 100;
            // 
            // lookUpcol1AccId
            // 
            this.lookUpcol1AccId.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpcol1AccId.LookupKey = "L_ACC";
            this.lookUpcol1AccId.Name = "lookUpcol1AccId";
            this.lookUpcol1AccId.NullText = "";
            this.lookUpcol1AccId.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // col1EmpNo
            // 
            this.col1EmpNo.Caption = "사번";
            this.col1EmpNo.FieldName = "emp_no";
            this.col1EmpNo.Name = "col1EmpNo";
            this.col1EmpNo.Visible = true;
            this.col1EmpNo.VisibleIndex = 2;
            this.col1EmpNo.Width = 81;
            // 
            // col1EmpNm
            // 
            this.col1EmpNm.Caption = "사원명";
            this.col1EmpNm.FieldName = "emp_nm";
            this.col1EmpNm.Name = "col1EmpNm";
            this.col1EmpNm.Visible = true;
            this.col1EmpNm.VisibleIndex = 3;
            this.col1EmpNm.Width = 94;
            // 
            // col1EmpNmEng
            // 
            this.col1EmpNmEng.Caption = "사원명(영문)";
            this.col1EmpNmEng.FieldName = "emp_nm_eng";
            this.col1EmpNmEng.Name = "col1EmpNmEng";
            this.col1EmpNmEng.Visible = true;
            this.col1EmpNmEng.VisibleIndex = 4;
            this.col1EmpNmEng.Width = 100;
            // 
            // col1DeptId
            // 
            this.col1DeptId.Caption = "부서ID";
            this.col1DeptId.ColumnEdit = this.spinEditcol1;
            this.col1DeptId.FieldName = "dept_id";
            this.col1DeptId.Name = "col1DeptId";
            this.col1DeptId.Visible = false;
            this.col1DeptId.Width = 100;
            // 
            // spinEditcol1
            // 
            this.spinEditcol1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditcol1.Name = "spinEditcol1";
            // 
            // col1DeptNm
            // 
            this.col1DeptNm.Caption = "부서";
            this.col1DeptNm.ColumnEdit = this.popcol1DeptNm;
            this.col1DeptNm.FieldName = "dept_nm";
            this.col1DeptNm.Name = "col1DeptNm";
            this.col1DeptNm.Visible = true;
            this.col1DeptNm.VisibleIndex = 6;
            this.col1DeptNm.Width = 100;
            // 
            // popcol1DeptNm
            // 
            this.popcol1DeptNm.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.popcol1DeptNm.LookupKey = "P_DEPT";
            this.popcol1DeptNm.Name = "popcol1DeptNm";
            // 
            // col1EntDate
            // 
            this.col1EntDate.Caption = "입사일자";
            this.col1EntDate.ColumnEdit = this.dateEditcol1;
            this.col1EntDate.FieldName = "ent_date";
            this.col1EntDate.Name = "col1EntDate";
            this.col1EntDate.Visible = true;
            this.col1EntDate.VisibleIndex = 7;
            this.col1EntDate.Width = 100;
            // 
            // dateEditcol1
            // 
            this.dateEditcol1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditcol1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditcol1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dateEditcol1.Name = "dateEditcol1";
            // 
            // col1GrpEntDate
            // 
            this.col1GrpEntDate.Caption = "그룹입사일자";
            this.col1GrpEntDate.FieldName = "grp_ent_date";
            this.col1GrpEntDate.Name = "col1GrpEntDate";
            this.col1GrpEntDate.Visible = true;
            this.col1GrpEntDate.VisibleIndex = 8;
            this.col1GrpEntDate.Width = 100;
            // 
            // col1JobGrade
            // 
            this.col1JobGrade.Caption = "직급";
            this.col1JobGrade.FieldName = "job_grade";
            this.col1JobGrade.Name = "col1JobGrade";
            this.col1JobGrade.Visible = true;
            this.col1JobGrade.VisibleIndex = 9;
            this.col1JobGrade.Width = 100;
            // 
            // col1JobType
            // 
            this.col1JobType.Caption = "직위";
            // 직위(job_type)는 자유 입력(VARCHAR 10) - 데이터 없는 빈 LookUp 편집기를 걸어두면 값이 안 보여서 뺐다(2026-09-26).
            this.col1JobType.FieldName = "job_type";
            this.col1JobType.Name = "col1JobType";
            this.col1JobType.Visible = true;
            this.col1JobType.VisibleIndex = 10;
            this.col1JobType.Width = 100;
            // 
            // chkEdit1
            // 
            this.chkEdit1.Name = "chkEdit1";
            // 
            // col1RetYn
            // 
            this.col1RetYn.Caption = "퇴사";
            this.col1RetYn.FieldName = "ret_yn";
            this.col1RetYn.Name = "col1RetYn";
            this.col1RetYn.Visible = true;
            this.col1RetYn.VisibleIndex = 11;
            this.col1RetYn.Width = 100;
            // 
            // col1RetDate
            // 
            this.col1RetDate.Caption = "퇴사일자";
            this.col1RetDate.ColumnEdit = this.dateEditcol1;
            this.col1RetDate.FieldName = "ret_date";
            this.col1RetDate.Name = "col1RetDate";
            this.col1RetDate.Visible = true;
            this.col1RetDate.VisibleIndex = 12;
            this.col1RetDate.Width = 100;
            // 
            // col1SexCd
            // 
            this.col1SexCd.Caption = "성별";
            this.col1SexCd.ColumnEdit = this.lookUpcol1SexCd;
            this.col1SexCd.FieldName = "sex_cd";
            this.col1SexCd.Name = "col1SexCd";
            this.col1SexCd.Visible = true;
            this.col1SexCd.VisibleIndex = 13;
            this.col1SexCd.Width = 100;
            // 
            // lookUpcol1SexCd
            // 
            this.lookUpcol1SexCd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpcol1SexCd.LookupKey = "L_CM0005";
            this.lookUpcol1SexCd.Name = "lookUpcol1SexCd";
            this.lookUpcol1SexCd.NullText = "";
            this.lookUpcol1SexCd.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // col1Tel
            // 
            this.col1Tel.Caption = "전화번호";
            this.col1Tel.FieldName = "tel";
            this.col1Tel.Name = "col1Tel";
            this.col1Tel.Visible = true;
            this.col1Tel.VisibleIndex = 14;
            this.col1Tel.Width = 100;
            // 
            // col1HpTel
            // 
            this.col1HpTel.Caption = "Mobile";
            this.col1HpTel.FieldName = "hp_tel";
            this.col1HpTel.Name = "col1HpTel";
            this.col1HpTel.Visible = true;
            this.col1HpTel.VisibleIndex = 15;
            this.col1HpTel.Width = 100;
            // 
            // col1Email
            // 
            this.col1Email.Caption = "E-mail";
            this.col1Email.FieldName = "email";
            this.col1Email.Name = "col1Email";
            this.col1Email.Visible = true;
            this.col1Email.VisibleIndex = 16;
            this.col1Email.Width = 100;
            // 
            // col1NatCd
            // 
            this.col1NatCd.Caption = "국가";
            this.col1NatCd.FieldName = "nat_cd";
            this.col1NatCd.Name = "col1NatCd";
            this.col1NatCd.Visible = true;
            this.col1NatCd.VisibleIndex = 17;
            this.col1NatCd.Width = 100;
            // 
            // col1ZipCode
            // 
            this.col1ZipCode.Caption = "우편번호";
            this.col1ZipCode.FieldName = "zip_code";
            this.col1ZipCode.Name = "col1ZipCode";
            this.col1ZipCode.Visible = true;
            this.col1ZipCode.VisibleIndex = 18;
            this.col1ZipCode.Width = 100;
            // 
            // col1Addr1
            // 
            this.col1Addr1.Caption = "주소1";
            this.col1Addr1.FieldName = "addr1";
            this.col1Addr1.Name = "col1Addr1";
            this.col1Addr1.Visible = true;
            this.col1Addr1.VisibleIndex = 19;
            this.col1Addr1.Width = 100;
            // 
            // col1Addr2
            // 
            this.col1Addr2.Caption = "주소2";
            this.col1Addr2.FieldName = "addr2";
            this.col1Addr2.Name = "col1Addr2";
            this.col1Addr2.Visible = true;
            this.col1Addr2.VisibleIndex = 20;
            this.col1Addr2.Width = 100;
            // 
            // col1HoliYn
            // 
            this.col1HoliYn.Caption = "연차관리";
            this.col1HoliYn.FieldName = "holi_yn";
            this.col1HoliYn.Name = "col1HoliYn";
            this.col1HoliYn.Visible = true;
            this.col1HoliYn.VisibleIndex = 21;
            this.col1HoliYn.Width = 100;
            // 
            // col1DiligYn
            // 
            this.col1DiligYn.Caption = "근태관리";
            this.col1DiligYn.FieldName = "dilig_yn";
            this.col1DiligYn.Name = "col1DiligYn";
            this.col1DiligYn.Visible = true;
            this.col1DiligYn.VisibleIndex = 22;
            this.col1DiligYn.Width = 100;
            // 
            // col1PayYn
            // 
            this.col1PayYn.Caption = "급여관리";
            this.col1PayYn.FieldName = "pay_yn";
            this.col1PayYn.Name = "col1PayYn";
            this.col1PayYn.Visible = true;
            this.col1PayYn.VisibleIndex = 23;
            this.col1PayYn.Width = 100;
            // 
            // col1Photo
            // 
            this.col1Photo.Caption = "photo";
            this.col1Photo.FieldName = "photo";
            this.col1Photo.Name = "col1Photo";
            this.col1Photo.Visible = false;
            this.col1Photo.Width = 100;
            // 
            // col1ValidateResult
            // 
            this.col1ValidateResult.Caption = "검증결과";
            this.col1ValidateResult.FieldName = "validate_result";
            this.col1ValidateResult.Name = "col1ValidateResult";
            this.col1ValidateResult.OptionsColumn.AllowEdit = false;
            this.col1ValidateResult.Visible = true;
            this.col1ValidateResult.VisibleIndex = 24;
            this.col1ValidateResult.Width = 320;
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
            // panHeader
            // 
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.btnValidate);
            this.panHeader.Controls.Add(this.btnUploadExcel);
            this.panHeader.Controls.Add(this.btnDownloadTemplate);
            this.panHeader.Controls.Add(this.cboAccId);
            this.panHeader.Controls.Add(this.labelControl1);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 30);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1235, 60);
            this.panHeader.TabIndex = 0;
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
            this.sectionHeaderWyn1.SvgIcon = null;
            this.sectionHeaderWyn1.TabIndex = 8;
            this.sectionHeaderWyn1.Text = "사원정보일괄등록 [frmEmpMulti]";
            // 
            // lookUpColumnEdit1
            // 
            this.lookUpColumnEdit1.AutoHeight = false;
            this.lookUpColumnEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpColumnEdit1.Name = "lookUpColumnEdit1";
            this.lookUpColumnEdit1.NullText = "";
            this.lookUpColumnEdit1.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // btnValidate
            // 
            this.btnValidate.BackColor = System.Drawing.Color.Transparent;
            this.btnValidate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnValidate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnValidate.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnValidate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnValidate.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnValidate.Image = null;
            this.btnValidate.Location = new System.Drawing.Point(462, 17);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnValidate.Size = new System.Drawing.Size(116, 24);
            this.btnValidate.TabIndex = 9;
            this.btnValidate.Text = "검증";
            this.btnValidate.ToolTip = null;
            // 
            // btnUploadExcel
            // 
            this.btnUploadExcel.BackColor = System.Drawing.Color.Transparent;
            this.btnUploadExcel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnUploadExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUploadExcel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnUploadExcel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnUploadExcel.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnUploadExcel.Image = null;
            this.btnUploadExcel.Location = new System.Drawing.Point(344, 17);
            this.btnUploadExcel.Name = "btnUploadExcel";
            this.btnUploadExcel.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnUploadExcel.Size = new System.Drawing.Size(116, 24);
            this.btnUploadExcel.TabIndex = 8;
            this.btnUploadExcel.Text = "엑셀업로드";
            this.btnUploadExcel.ToolTip = null;
            // 
            // btnDownloadTemplate
            // 
            this.btnDownloadTemplate.BackColor = System.Drawing.Color.Transparent;
            this.btnDownloadTemplate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnDownloadTemplate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDownloadTemplate.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnDownloadTemplate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDownloadTemplate.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDownloadTemplate.Image = null;
            this.btnDownloadTemplate.Location = new System.Drawing.Point(226, 17);
            this.btnDownloadTemplate.Name = "btnDownloadTemplate";
            this.btnDownloadTemplate.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDownloadTemplate.Size = new System.Drawing.Size(116, 24);
            this.btnDownloadTemplate.TabIndex = 7;
            this.btnDownloadTemplate.Text = "엑셀양식다운로드";
            this.btnDownloadTemplate.ToolTip = null;
            this.btnDownloadTemplate.Click += new System.EventHandler(this.btnDownloadTemplate_Click);
            // 
            // cboAccId
            // 
            this.cboAccId.EditValue = "";
            this.cboAccId.Location = new System.Drawing.Point(61, 20);
            this.cboAccId.LookupKey = "L_ACC";
            this.cboAccId.Required = true;
            this.cboAccId.Name = "cboAccId";
            this.cboAccId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboAccId.Properties.NullText = "";
            this.cboAccId.Size = new System.Drawing.Size(159, 20);
            this.cboAccId.TabIndex = 6;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(17, 23);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 15);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "사업장";
            // 
            // frmEmpMulti
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1245, 580);
            this.Controls.Add(this.panBody);
            this.Controls.Add(this.panHeader);
            this.Controls.Add(this.paTitleH);
            this.Name = "frmEmpMulti";
            this.Padding = new System.Windows.Forms.Padding(5);
            ((System.ComponentModel.ISupportInitialize)(this.panBody)).EndInit();
            this.panBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcol1AccId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditcol1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popcol1DeptNm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcol1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcol1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcol1SexCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).EndInit();
            this.paTitle1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).EndInit();
            this.paTitleH.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lookUpColumnEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAccId.Properties)).EndInit();
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
    private DevExpress.XtraGrid.Columns.GridColumn col1EmpId;
    private DevExpress.XtraGrid.Columns.GridColumn col1AccId;
    private DevExpress.XtraGrid.Columns.GridColumn col1EmpNo;
    private DevExpress.XtraGrid.Columns.GridColumn col1EmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn col1EmpNmEng;
    private DevExpress.XtraGrid.Columns.GridColumn col1DeptId;
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
    private DevExpress.XtraGrid.Columns.GridColumn col1Photo;
    private DevExpress.XtraGrid.Columns.GridColumn col1ValidateResult;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkEdit1;
    private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spinEditcol1;
    private WYNLAB.Base.Controls.DateColumnEdit dateEditcol1;
    private LookUpColumnEdit lookUpcol1AccId;
    private LookUpColumnEdit lookUpcol1SexCd;
    private PopupLookupColumnEdit popcol1DeptNm;
    private LookUpColumnEdit lookUpColumnEdit1;
    private ButtonWyn btnValidate;
    private ButtonWyn btnUploadExcel;
    private ButtonWyn btnDownloadTemplate;
    private LookUpEditWyn cboAccId;
    private DevExpress.XtraEditors.LabelControl labelControl1;
}
