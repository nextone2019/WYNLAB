// AI Builder가 싱글그리드 템플릿을 복제해서 자동 생성 - 2026-09-18.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
#nullable disable
using WYNLAB.Base;
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.panBody = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.col1EmpNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1EmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1EmpNmEng = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1EntDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dateEditcol1 = new WYNLAB.Base.Controls.DateColumnEdit();
            this.col1GrpEntDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1JobGrade = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpcol1JobGrade = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.col1JobType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1RetYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.col1RetDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1SexCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpcol1SexCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.col1Tel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1Email = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1EmpId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDeptId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDeptNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.paTitle1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.txtDeptNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblSearchDeptId = new DevExpress.XtraEditors.LabelControl();
            this.txtEmpNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtDeptId = new WYNLAB.Base.Controls.TextEditWyn();
            this.paTitleH = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBody)).BeginInit();
            this.panBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcol1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcol1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcol1JobGrade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcol1SexCd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).BeginInit();
            this.paTitle1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).BeginInit();
            this.paTitleH.SuspendLayout();
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
            this.dateEditcol1,
            this.lookUpcol1JobGrade,
            this.lookUpcol1SexCd});
            this.grd1.Size = new System.Drawing.Size(1235, 450);
            this.grd1.TabIndex = 1;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col1EmpNo,
            this.col1EmpNm,
            this.col1EmpNmEng,
            this.col1EntDate,
            this.col1GrpEntDate,
            this.col1JobGrade,
            this.col1JobType,
            this.col1RetYn,
            this.col1RetDate,
            this.col1SexCd,
            this.col1Tel,
            this.col1Email,
            this.col1EmpId,
            this.colDeptId,
            this.colDeptNm});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // col1EmpNo
            // 
            this.col1EmpNo.Caption = "사번";
            this.col1EmpNo.FieldName = "emp_no";
            this.col1EmpNo.Name = "col1EmpNo";
            this.col1EmpNo.OptionsColumn.ReadOnly = true;
            this.col1EmpNo.Visible = true;
            this.col1EmpNo.VisibleIndex = 1;
            this.col1EmpNo.Width = 81;
            // 
            // col1EmpNm
            // 
            this.col1EmpNm.Caption = "사원명";
            this.col1EmpNm.FieldName = "emp_nm";
            this.col1EmpNm.Name = "col1EmpNm";
            this.col1EmpNm.OptionsColumn.ReadOnly = true;
            this.col1EmpNm.Visible = true;
            this.col1EmpNm.VisibleIndex = 2;
            this.col1EmpNm.Width = 95;
            // 
            // col1EmpNmEng
            // 
            this.col1EmpNmEng.Caption = "사원명(영문)";
            this.col1EmpNmEng.FieldName = "emp_nm_eng";
            this.col1EmpNmEng.Name = "col1EmpNmEng";
            this.col1EmpNmEng.OptionsColumn.ReadOnly = true;
            this.col1EmpNmEng.Visible = true;
            this.col1EmpNmEng.VisibleIndex = 3;
            this.col1EmpNmEng.Width = 130;
            // 
            // col1EntDate
            // 
            this.col1EntDate.Caption = "입사일자";
            this.col1EntDate.ColumnEdit = this.dateEditcol1;
            this.col1EntDate.FieldName = "ent_date";
            this.col1EntDate.Name = "col1EntDate";
            this.col1EntDate.OptionsColumn.ReadOnly = true;
            this.col1EntDate.Visible = true;
            this.col1EntDate.VisibleIndex = 4;
            this.col1EntDate.Width = 98;
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
            this.col1GrpEntDate.ColumnEdit = this.dateEditcol1;
            this.col1GrpEntDate.FieldName = "grp_ent_date";
            this.col1GrpEntDate.Name = "col1GrpEntDate";
            this.col1GrpEntDate.OptionsColumn.ReadOnly = true;
            this.col1GrpEntDate.Visible = true;
            this.col1GrpEntDate.VisibleIndex = 5;
            this.col1GrpEntDate.Width = 97;
            // 
            // col1JobGrade
            // 
            this.col1JobGrade.Caption = "직책";
            this.col1JobGrade.ColumnEdit = this.lookUpcol1JobGrade;
            this.col1JobGrade.FieldName = "job_grade";
            this.col1JobGrade.Name = "col1JobGrade";
            this.col1JobGrade.OptionsColumn.ReadOnly = true;
            this.col1JobGrade.Visible = true;
            this.col1JobGrade.VisibleIndex = 6;
            this.col1JobGrade.Width = 102;
            // 
            // lookUpcol1JobGrade
            // 
            this.lookUpcol1JobGrade.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpcol1JobGrade.LookupKey = "";
            this.lookUpcol1JobGrade.Name = "lookUpcol1JobGrade";
            this.lookUpcol1JobGrade.NullText = "";
            this.lookUpcol1JobGrade.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // col1JobType
            // 
            this.col1JobType.Caption = "직무";
            this.col1JobType.FieldName = "job_type";
            this.col1JobType.Name = "col1JobType";
            this.col1JobType.OptionsColumn.ReadOnly = true;
            this.col1JobType.Visible = true;
            this.col1JobType.VisibleIndex = 7;
            this.col1JobType.Width = 89;
            // 
            // col1RetYn
            // 
            this.col1RetYn.Caption = "퇴사";
            this.col1RetYn.ColumnEdit = this.chkEdit1;
            this.col1RetYn.FieldName = "ret_yn";
            this.col1RetYn.Name = "col1RetYn";
            this.col1RetYn.OptionsColumn.ReadOnly = true;
            this.col1RetYn.Visible = true;
            this.col1RetYn.VisibleIndex = 8;
            this.col1RetYn.Width = 51;
            // 
            // chkEdit1
            // 
            this.chkEdit1.Name = "chkEdit1";
            // 
            // col1RetDate
            // 
            this.col1RetDate.Caption = "퇴사일자";
            this.col1RetDate.ColumnEdit = this.dateEditcol1;
            this.col1RetDate.FieldName = "ret_date";
            this.col1RetDate.Name = "col1RetDate";
            this.col1RetDate.OptionsColumn.ReadOnly = true;
            this.col1RetDate.Visible = true;
            this.col1RetDate.VisibleIndex = 9;
            this.col1RetDate.Width = 117;
            // 
            // col1SexCd
            // 
            this.col1SexCd.Caption = "성별";
            this.col1SexCd.ColumnEdit = this.lookUpcol1SexCd;
            this.col1SexCd.FieldName = "sex_cd";
            this.col1SexCd.Name = "col1SexCd";
            this.col1SexCd.OptionsColumn.ReadOnly = true;
            this.col1SexCd.Visible = true;
            this.col1SexCd.VisibleIndex = 10;
            this.col1SexCd.Width = 116;
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
            this.col1Tel.Caption = "연락처";
            this.col1Tel.FieldName = "tel";
            this.col1Tel.Name = "col1Tel";
            this.col1Tel.OptionsColumn.ReadOnly = true;
            this.col1Tel.Visible = true;
            this.col1Tel.VisibleIndex = 11;
            this.col1Tel.Width = 322;
            // 
            // col1Email
            // 
            this.col1Email.Caption = "E-mail";
            this.col1Email.FieldName = "email";
            this.col1Email.Name = "col1Email";
            this.col1Email.OptionsColumn.ReadOnly = true;
            this.col1Email.Visible = true;
            this.col1Email.VisibleIndex = 12;
            this.col1Email.Width = 197;
            // 
            // col1EmpId
            // 
            this.col1EmpId.Caption = "사원ID";
            this.col1EmpId.FieldName = "EMP_ID";
            this.col1EmpId.Name = "col1EmpId";
            this.col1EmpId.OptionsColumn.ReadOnly = true;
            this.col1EmpId.Width = 100;
            // 
            // colDeptId
            // 
            this.colDeptId.Caption = "부서ID";
            this.colDeptId.FieldName = "dept_id";
            this.colDeptId.Name = "colDeptId";
            this.colDeptId.OptionsColumn.ReadOnly = true;
            // 
            // colDeptNm
            // 
            this.colDeptNm.Caption = "부서";
            this.colDeptNm.FieldName = "dept_nm";
            this.colDeptNm.Name = "colDeptNm";
            this.colDeptNm.OptionsColumn.ReadOnly = true;
            this.colDeptNm.Visible = true;
            this.colDeptNm.VisibleIndex = 0;
            this.colDeptNm.Width = 129;
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
            this.sectionHeaderWyn4.Text = "사원LIST";
            // 
            // panHeader
            // 
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.txtDeptNm);
            this.panHeader.Controls.Add(this.labelControl1);
            this.panHeader.Controls.Add(this.lblSearchDeptId);
            this.panHeader.Controls.Add(this.txtEmpNm);
            this.panHeader.Controls.Add(this.txtDeptId);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 30);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1235, 60);
            this.panHeader.TabIndex = 0;
            // 
            // txtDeptNm
            // 
            this.txtDeptNm.Location = new System.Drawing.Point(44, 21);
            this.txtDeptNm.LookupKey = "P_DEPT";
            this.txtDeptNm.MatchField = "dept_nm";
            this.txtDeptNm.Name = "txtDeptNm";
            this.txtDeptNm.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtDeptNm.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDeptNm.Properties.Appearance.Options.UseBackColor = true;
            this.txtDeptNm.Properties.Appearance.Options.UseForeColor = true;
            this.txtDeptNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", 24, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.txtDeptNm.Size = new System.Drawing.Size(175, 20);
            this.txtDeptNm.TabIndex = 5;
            this.txtDeptNm.ToolTip = null;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(277, 24);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(30, 14);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "사원명";
            // 
            // lblSearchDeptId
            // 
            this.lblSearchDeptId.Location = new System.Drawing.Point(18, 24);
            this.lblSearchDeptId.Name = "lblSearchDeptId";
            this.lblSearchDeptId.Size = new System.Drawing.Size(20, 14);
            this.lblSearchDeptId.TabIndex = 6;
            this.lblSearchDeptId.Text = "부서";
            // 
            // txtEmpNm
            // 
            this.txtEmpNm.Location = new System.Drawing.Point(314, 21);
            this.txtEmpNm.Name = "txtEmpNm";
            this.txtEmpNm.Size = new System.Drawing.Size(122, 20);
            this.txtEmpNm.TabIndex = 7;
            // 
            // txtDeptId
            // 
            this.txtDeptId.Location = new System.Drawing.Point(591, 19);
            this.txtDeptId.Name = "txtDeptId";
            this.txtDeptId.Size = new System.Drawing.Size(150, 20);
            this.txtDeptId.TabIndex = 7;
            this.txtDeptId.Visible = false;
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
            this.sectionHeaderWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1230, 25);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 10;
            this.sectionHeaderWyn1.Text = "사원정보조회 [frmEmpList]";
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
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcol1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcol1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcol1JobGrade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcol1SexCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).EndInit();
            this.paTitle1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).EndInit();
            this.paTitleH.ResumeLayout(false);
            this.ResumeLayout(false);

    }
    private PanelWyn panHeader;
    private PanelWyn panBody;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private PanelWyn paTitle1;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn paTitleH;
    private DevExpress.XtraGrid.Columns.GridColumn col1EmpNo;
    private DevExpress.XtraGrid.Columns.GridColumn col1EmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn col1EmpNmEng;
    private DevExpress.XtraGrid.Columns.GridColumn col1EntDate;
    private DevExpress.XtraGrid.Columns.GridColumn col1GrpEntDate;
    private DevExpress.XtraGrid.Columns.GridColumn col1JobGrade;
    private DevExpress.XtraGrid.Columns.GridColumn col1JobType;
    private DevExpress.XtraGrid.Columns.GridColumn col1RetYn;
    private DevExpress.XtraGrid.Columns.GridColumn col1RetDate;
    private DevExpress.XtraGrid.Columns.GridColumn col1SexCd;
    private DevExpress.XtraGrid.Columns.GridColumn col1Tel;
    private DevExpress.XtraGrid.Columns.GridColumn col1Email;
    private DevExpress.XtraGrid.Columns.GridColumn col1EmpId;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkEdit1;
    private WYNLAB.Base.Controls.DateColumnEdit dateEditcol1;
    private LookUpColumnEdit lookUpcol1JobGrade;
    private LookUpColumnEdit lookUpcol1SexCd;
    private PopupLookupEditWyn txtDeptNm;
    private DevExpress.XtraEditors.LabelControl lblSearchDeptId;
    private TextEditWyn txtDeptId;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private TextEditWyn txtEmpNm;
    private SectionHeaderWyn sectionHeaderWyn1;
    private DevExpress.XtraGrid.Columns.GridColumn colDeptId;
    private DevExpress.XtraGrid.Columns.GridColumn colDeptNm;
}
