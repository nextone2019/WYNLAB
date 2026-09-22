// AI Builder가 마스터-폼-서브그리드 템플릿을 복제해서 자동 생성 - 2026-09-09.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmEMP
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEMP));
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colMEmpId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMAccId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpColumnEdit1 = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colMEmpNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMEmpNmEng = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMDeptId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spinEditcolM = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colMDeptNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMEntDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dateColumnEdit1 = new WYNLAB.Base.Controls.DateColumnEdit();
            this.colMGrpEntDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dateColumnEdit2 = new WYNLAB.Base.Controls.DateColumnEdit();
            this.colMJobGrade = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMJobType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMRetYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkEditcolM = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colMRetDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dateColumnEdit3 = new WYNLAB.Base.Controls.DateColumnEdit();
            this.colMSexCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpColumnEdit2 = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colMTel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMHpTel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMEmail = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMNatCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMZipCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMAddr1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMAddr2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMHoliYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMDiligYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMPayYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMPhoto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dateEditcolM = new WYNLAB.Base.Controls.DateColumnEdit();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblDetailHoliYn = new DevExpress.XtraEditors.LabelControl();
            this.chkDetailPayYn = new WYNLAB.Base.Controls.CheckBoxWyn();
            this.lblDetailPayYn = new DevExpress.XtraEditors.LabelControl();
            this.chkDetailDiligYn = new WYNLAB.Base.Controls.CheckBoxWyn();
            this.lblDetailDiligYn = new DevExpress.XtraEditors.LabelControl();
            this.chkDetailHoliYn = new WYNLAB.Base.Controls.CheckBoxWyn();
            this.txtDetailDeptNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.cboDetailSexCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.cboDetailAccCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.picEmpPhoto = new WYNLAB.Base.Controls.PictureEditWyn();
            this.lblDetailAccId = new DevExpress.XtraEditors.LabelControl();
            this.lblDetailEmpNo = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailEmpNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailEmpNm = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailEmpNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailEmpNmEng = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailDeptId = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtDetailEmpNmEng = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailDeptId = new DevExpress.XtraEditors.LabelControl();
            this.lblDetailDeptNm = new DevExpress.XtraEditors.LabelControl();
            this.lblDetailEntDate = new DevExpress.XtraEditors.LabelControl();
            this.dteDetailEntDate = new WYNLAB.Base.Controls.DateEditWyn();
            this.lblDetailGrpEntDate = new DevExpress.XtraEditors.LabelControl();
            this.dteDetailGrpEntDate = new WYNLAB.Base.Controls.DateEditWyn();
            this.lblDetailJobGrade = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailJobGrade = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailJobType = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailJobType = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailRetYn = new DevExpress.XtraEditors.LabelControl();
            this.chkDetailRetYn = new WYNLAB.Base.Controls.CheckBoxWyn();
            this.lblDetailRetDate = new DevExpress.XtraEditors.LabelControl();
            this.dteDetailRetDate = new WYNLAB.Base.Controls.DateEditWyn();
            this.lblDetailSexCd = new DevExpress.XtraEditors.LabelControl();
            this.lblDetailTel = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailTel = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailHpTel = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailHpTel = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailEmail = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailEmail = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailNatCd = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailNatCd = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailZipCode = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailZipCode = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailAddr1 = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailAddr1 = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailAddr2 = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailAddr2 = new WYNLAB.Base.Controls.TextEditWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.lblSearchEmpNo = new DevExpress.XtraEditors.LabelControl();
            this.txtDeptNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.txtEmpNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSearchDeptId = new DevExpress.XtraEditors.LabelControl();
            this.txtDeptId = new WYNLAB.Base.Controls.TextEditWyn();
            this.paTitleH = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
            this.panelWyn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
            this.panelWyn4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).BeginInit();
            this.panelWyn8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpColumnEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditcolM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateColumnEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateColumnEdit1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateColumnEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateColumnEdit2.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEditcolM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateColumnEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateColumnEdit3.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpColumnEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcolM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcolM.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
            this.panelWyn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkDetailPayYn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDetailDiligYn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDetailHoliYn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailDeptNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailSexCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailAccCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEmpPhoto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailEmpNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailEmpNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailDeptId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailEmpNmEng.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDetailEntDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDetailEntDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDetailGrpEntDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDetailGrpEntDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailJobGrade.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailJobType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDetailRetYn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDetailRetDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDetailRetDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailTel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailHpTel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailEmail.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailNatCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailZipCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailAddr1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailAddr2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
            this.panelWyn6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).BeginInit();
            this.paTitleH.SuspendLayout();
            this.SuspendLayout();
            // 
            // panBase
            // 
            this.panBase.Appearance.BackColor = System.Drawing.Color.White;
            this.panBase.Appearance.Options.UseBackColor = true;
            this.panBase.Controls.Add(this.panelWyn3);
            this.panBase.Controls.Add(this.panHeader);
            this.panBase.Controls.Add(this.paTitleH);
            this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBase.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panBase.Location = new System.Drawing.Point(0, 0);
            this.panBase.Name = "panBase";
            this.panBase.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.panBase.Size = new System.Drawing.Size(1688, 818);
            this.panBase.TabIndex = 6;
            // 
            // panelWyn3
            // 
            this.panelWyn3.Controls.Add(this.panelWyn4);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(5, 74);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Size = new System.Drawing.Size(1678, 739);
            this.panelWyn3.TabIndex = 7;
            // 
            // panelWyn4
            // 
            this.panelWyn4.Controls.Add(this.panelWyn8);
            this.panelWyn4.Controls.Add(this.splitterWyn1);
            this.panelWyn4.Controls.Add(this.panelWyn5);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(0, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelWyn4.Size = new System.Drawing.Size(1678, 739);
            this.panelWyn4.TabIndex = 7;
            // 
            // panelWyn8
            // 
            this.panelWyn8.Controls.Add(this.grd1);
            this.panelWyn8.Controls.Add(this.panelWyn2);
            this.panelWyn8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn8.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn8.Location = new System.Drawing.Point(3, 0);
            this.panelWyn8.Name = "panelWyn8";
            this.panelWyn8.Size = new System.Drawing.Size(984, 739);
            this.panelWyn8.TabIndex = 12;
            // 
            // grd1
            // 
            this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd1.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grd1.Location = new System.Drawing.Point(0, 27);
            this.grd1.MainView = this.gvw1;
            this.grd1.Name = "grd1";
            this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.chkEditcolM,
            this.spinEditcolM,
            this.dateEditcolM,
            this.lookUpColumnEdit1,
            this.dateColumnEdit1,
            this.dateColumnEdit2,
            this.dateColumnEdit3,
            this.lookUpColumnEdit2});
            this.grd1.Size = new System.Drawing.Size(984, 712);
            this.grd1.TabIndex = 10;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colMEmpId,
            this.colMAccId,
            this.colMEmpNo,
            this.colMEmpNm,
            this.colMEmpNmEng,
            this.colMDeptId,
            this.colMDeptNm,
            this.colMEntDate,
            this.colMGrpEntDate,
            this.colMJobGrade,
            this.colMJobType,
            this.colMRetYn,
            this.colMRetDate,
            this.colMSexCd,
            this.colMTel,
            this.colMHpTel,
            this.colMEmail,
            this.colMNatCd,
            this.colMZipCode,
            this.colMAddr1,
            this.colMAddr2,
            this.colMHoliYn,
            this.colMDiligYn,
            this.colMPayYn,
            this.colMPhoto});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightFocusedRow = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw1.OptionsSelection.InvertSelection = true;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // colMEmpId
            // 
            this.colMEmpId.FieldName = "emp_id";
            this.colMEmpId.Name = "colMEmpId";
            // 
            // colMAccId
            // 
            this.colMAccId.Caption = "사업장";
            this.colMAccId.ColumnEdit = this.lookUpColumnEdit1;
            this.colMAccId.FieldName = "acc_id";
            this.colMAccId.Name = "colMAccId";
            this.colMAccId.Width = 100;
            // 
            // lookUpColumnEdit1
            // 
            this.lookUpColumnEdit1.AutoHeight = false;
            this.lookUpColumnEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpColumnEdit1.LookupKey = "L_ACC";
            this.lookUpColumnEdit1.Name = "lookUpColumnEdit1";
            this.lookUpColumnEdit1.NullText = "";
            this.lookUpColumnEdit1.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colMEmpNo
            // 
            this.colMEmpNo.Caption = "사번";
            this.colMEmpNo.FieldName = "emp_no";
            this.colMEmpNo.Name = "colMEmpNo";
            this.colMEmpNo.OptionsColumn.AllowEdit = false;
            this.colMEmpNo.Visible = true;
            this.colMEmpNo.VisibleIndex = 1;
            this.colMEmpNo.Width = 73;
            // 
            // colMEmpNm
            // 
            this.colMEmpNm.Caption = "사원명";
            this.colMEmpNm.FieldName = "emp_nm";
            this.colMEmpNm.Name = "colMEmpNm";
            this.colMEmpNm.Visible = true;
            this.colMEmpNm.VisibleIndex = 2;
            this.colMEmpNm.Width = 89;
            // 
            // colMEmpNmEng
            // 
            this.colMEmpNmEng.Caption = "사원명(영문)";
            this.colMEmpNmEng.FieldName = "emp_nm_eng";
            this.colMEmpNmEng.Name = "colMEmpNmEng";
            this.colMEmpNmEng.Width = 100;
            // 
            // colMDeptId
            // 
            this.colMDeptId.Caption = "부서ID";
            this.colMDeptId.ColumnEdit = this.spinEditcolM;
            this.colMDeptId.FieldName = "DEPT_ID";
            this.colMDeptId.Name = "colMDeptId";
            this.colMDeptId.Width = 100;
            // 
            // spinEditcolM
            // 
            this.spinEditcolM.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditcolM.Name = "spinEditcolM";
            // 
            // colMDeptNm
            // 
            this.colMDeptNm.Caption = "부서";
            this.colMDeptNm.FieldName = "dept_nm";
            this.colMDeptNm.Name = "colMDeptNm";
            this.colMDeptNm.Visible = true;
            this.colMDeptNm.VisibleIndex = 0;
            this.colMDeptNm.Width = 100;
            // 
            // colMEntDate
            // 
            this.colMEntDate.Caption = "입사일자";
            this.colMEntDate.ColumnEdit = this.dateColumnEdit1;
            this.colMEntDate.FieldName = "ent_date";
            this.colMEntDate.Name = "colMEntDate";
            this.colMEntDate.Visible = true;
            this.colMEntDate.VisibleIndex = 3;
            this.colMEntDate.Width = 85;
            // 
            // dateColumnEdit1
            // 
            this.dateColumnEdit1.AutoHeight = false;
            this.dateColumnEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateColumnEdit1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateColumnEdit1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dateColumnEdit1.Name = "dateColumnEdit1";
            // 
            // colMGrpEntDate
            // 
            this.colMGrpEntDate.Caption = "그룹입사일자";
            this.colMGrpEntDate.ColumnEdit = this.dateColumnEdit2;
            this.colMGrpEntDate.FieldName = "grp_ent_date";
            this.colMGrpEntDate.Name = "colMGrpEntDate";
            this.colMGrpEntDate.Visible = true;
            this.colMGrpEntDate.VisibleIndex = 4;
            this.colMGrpEntDate.Width = 85;
            // 
            // dateColumnEdit2
            // 
            this.dateColumnEdit2.AutoHeight = false;
            this.dateColumnEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateColumnEdit2.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateColumnEdit2.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dateColumnEdit2.Name = "dateColumnEdit2";
            // 
            // colMJobGrade
            // 
            this.colMJobGrade.Caption = "직잭";
            this.colMJobGrade.FieldName = "job_grade";
            this.colMJobGrade.Name = "colMJobGrade";
            this.colMJobGrade.Visible = true;
            this.colMJobGrade.VisibleIndex = 5;
            this.colMJobGrade.Width = 100;
            // 
            // colMJobType
            // 
            this.colMJobType.Caption = "직무";
            this.colMJobType.FieldName = "job_type";
            this.colMJobType.Name = "colMJobType";
            this.colMJobType.Visible = true;
            this.colMJobType.VisibleIndex = 6;
            this.colMJobType.Width = 100;
            // 
            // colMRetYn
            // 
            this.colMRetYn.Caption = "퇴사";
            this.colMRetYn.ColumnEdit = this.chkEditcolM;
            this.colMRetYn.FieldName = "ret_yn";
            this.colMRetYn.Name = "colMRetYn";
            this.colMRetYn.Visible = true;
            this.colMRetYn.VisibleIndex = 7;
            this.colMRetYn.Width = 39;
            // 
            // chkEditcolM
            // 
            this.chkEditcolM.Name = "chkEditcolM";
            // 
            // colMRetDate
            // 
            this.colMRetDate.Caption = "퇴사일자";
            this.colMRetDate.ColumnEdit = this.dateColumnEdit3;
            this.colMRetDate.FieldName = "ret_date";
            this.colMRetDate.Name = "colMRetDate";
            this.colMRetDate.Visible = true;
            this.colMRetDate.VisibleIndex = 8;
            this.colMRetDate.Width = 85;
            // 
            // dateColumnEdit3
            // 
            this.dateColumnEdit3.AutoHeight = false;
            this.dateColumnEdit3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateColumnEdit3.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateColumnEdit3.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dateColumnEdit3.Name = "dateColumnEdit3";
            // 
            // colMSexCd
            // 
            this.colMSexCd.Caption = "성별";
            this.colMSexCd.ColumnEdit = this.lookUpColumnEdit2;
            this.colMSexCd.FieldName = "sex_cd";
            this.colMSexCd.Name = "colMSexCd";
            this.colMSexCd.Visible = true;
            this.colMSexCd.VisibleIndex = 9;
            this.colMSexCd.Width = 66;
            // 
            // lookUpColumnEdit2
            // 
            this.lookUpColumnEdit2.AutoHeight = false;
            this.lookUpColumnEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpColumnEdit2.LookupKey = "L_CM0005";
            this.lookUpColumnEdit2.Name = "lookUpColumnEdit2";
            this.lookUpColumnEdit2.NullText = "";
            this.lookUpColumnEdit2.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colMTel
            // 
            this.colMTel.Caption = "연락처";
            this.colMTel.FieldName = "tel";
            this.colMTel.Name = "colMTel";
            this.colMTel.Visible = true;
            this.colMTel.VisibleIndex = 10;
            this.colMTel.Width = 100;
            // 
            // colMHpTel
            // 
            this.colMHpTel.Caption = "MOBILE";
            this.colMHpTel.FieldName = "hp_tel";
            this.colMHpTel.Name = "colMHpTel";
            this.colMHpTel.Visible = true;
            this.colMHpTel.VisibleIndex = 11;
            this.colMHpTel.Width = 100;
            // 
            // colMEmail
            // 
            this.colMEmail.Caption = "E-MAIL";
            this.colMEmail.FieldName = "email";
            this.colMEmail.Name = "colMEmail";
            this.colMEmail.Visible = true;
            this.colMEmail.VisibleIndex = 12;
            this.colMEmail.Width = 100;
            // 
            // colMNatCd
            // 
            this.colMNatCd.Caption = "국가";
            this.colMNatCd.FieldName = "nat_cd";
            this.colMNatCd.Name = "colMNatCd";
            this.colMNatCd.Visible = true;
            this.colMNatCd.VisibleIndex = 13;
            this.colMNatCd.Width = 100;
            // 
            // colMZipCode
            // 
            this.colMZipCode.Caption = "우편번호";
            this.colMZipCode.FieldName = "zip_code";
            this.colMZipCode.Name = "colMZipCode";
            this.colMZipCode.Visible = true;
            this.colMZipCode.VisibleIndex = 14;
            this.colMZipCode.Width = 100;
            // 
            // colMAddr1
            // 
            this.colMAddr1.Caption = "주소1";
            this.colMAddr1.FieldName = "addr1";
            this.colMAddr1.Name = "colMAddr1";
            this.colMAddr1.Visible = true;
            this.colMAddr1.VisibleIndex = 15;
            this.colMAddr1.Width = 100;
            // 
            // colMAddr2
            // 
            this.colMAddr2.Caption = "주소2";
            this.colMAddr2.FieldName = "addr2";
            this.colMAddr2.Name = "colMAddr2";
            this.colMAddr2.Visible = true;
            this.colMAddr2.VisibleIndex = 16;
            this.colMAddr2.Width = 100;
            // 
            // colMHoliYn
            // 
            this.colMHoliYn.Caption = "연차대상";
            this.colMHoliYn.ColumnEdit = this.chkEditcolM;
            this.colMHoliYn.FieldName = "holi_yn";
            this.colMHoliYn.Name = "colMHoliYn";
            this.colMHoliYn.Visible = true;
            this.colMHoliYn.VisibleIndex = 17;
            this.colMHoliYn.Width = 100;
            // 
            // colMDiligYn
            // 
            this.colMDiligYn.Caption = "근태대상";
            this.colMDiligYn.ColumnEdit = this.chkEditcolM;
            this.colMDiligYn.FieldName = "dilig_yn";
            this.colMDiligYn.Name = "colMDiligYn";
            this.colMDiligYn.Visible = true;
            this.colMDiligYn.VisibleIndex = 18;
            this.colMDiligYn.Width = 100;
            // 
            // colMPayYn
            // 
            this.colMPayYn.Caption = "급여대상";
            this.colMPayYn.ColumnEdit = this.chkEditcolM;
            this.colMPayYn.FieldName = "pay_yn";
            this.colMPayYn.Name = "colMPayYn";
            this.colMPayYn.Visible = true;
            this.colMPayYn.VisibleIndex = 19;
            this.colMPayYn.Width = 100;
            // 
            // colMPhoto
            // 
            this.colMPhoto.Caption = "사진";
            this.colMPhoto.FieldName = "photo";
            this.colMPhoto.Name = "colMPhoto";
            this.colMPhoto.Visible = true;
            this.colMPhoto.VisibleIndex = 20;
            this.colMPhoto.Width = 100;
            // 
            // dateEditcolM
            // 
            this.dateEditcolM.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditcolM.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditcolM.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dateEditcolM.Name = "dateEditcolM";
            // 
            // panelWyn2
            // 
            this.panelWyn2.Controls.Add(this.sectionHeaderWyn4);
            this.panelWyn2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn2.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn2.Location = new System.Drawing.Point(0, 0);
            this.panelWyn2.Name = "panelWyn2";
            this.panelWyn2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn2.Size = new System.Drawing.Size(984, 27);
            this.panelWyn2.TabIndex = 11;
            // 
            // sectionHeaderWyn4
            // 
            this.sectionHeaderWyn4.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn4.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn4.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn4.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn4.Name = "sectionHeaderWyn4";
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(979, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 9;
            this.sectionHeaderWyn4.Text = "사원 LIST";
            // 
            // splitterWyn1
            // 
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitterWyn1.Location = new System.Drawing.Point(987, 0);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(10, 739);
            this.splitterWyn1.TabIndex = 10;
            this.splitterWyn1.TabStop = false;
            // 
            // panelWyn5
            // 
            this.panelWyn5.Controls.Add(this.panData);
            this.panelWyn5.Controls.Add(this.panelWyn6);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(997, 0);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Size = new System.Drawing.Size(681, 739);
            this.panelWyn5.TabIndex = 6;
            // 
            // panData
            // 
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Controls.Add(this.groupBox1);
            this.panData.Controls.Add(this.txtDetailDeptNm);
            this.panData.Controls.Add(this.cboDetailSexCd);
            this.panData.Controls.Add(this.cboDetailAccCd);
            this.panData.Controls.Add(this.picEmpPhoto);
            this.panData.Controls.Add(this.lblDetailAccId);
            this.panData.Controls.Add(this.lblDetailEmpNo);
            this.panData.Controls.Add(this.txtDetailEmpNo);
            this.panData.Controls.Add(this.lblDetailEmpNm);
            this.panData.Controls.Add(this.txtDetailEmpNm);
            this.panData.Controls.Add(this.lblDetailEmpNmEng);
            this.panData.Controls.Add(this.txtDetailDeptId);
            this.panData.Controls.Add(this.txtDetailEmpNmEng);
            this.panData.Controls.Add(this.lblDetailDeptId);
            this.panData.Controls.Add(this.lblDetailDeptNm);
            this.panData.Controls.Add(this.lblDetailEntDate);
            this.panData.Controls.Add(this.dteDetailEntDate);
            this.panData.Controls.Add(this.lblDetailGrpEntDate);
            this.panData.Controls.Add(this.dteDetailGrpEntDate);
            this.panData.Controls.Add(this.lblDetailJobGrade);
            this.panData.Controls.Add(this.txtDetailJobGrade);
            this.panData.Controls.Add(this.lblDetailJobType);
            this.panData.Controls.Add(this.txtDetailJobType);
            this.panData.Controls.Add(this.lblDetailRetYn);
            this.panData.Controls.Add(this.chkDetailRetYn);
            this.panData.Controls.Add(this.lblDetailRetDate);
            this.panData.Controls.Add(this.dteDetailRetDate);
            this.panData.Controls.Add(this.lblDetailSexCd);
            this.panData.Controls.Add(this.lblDetailTel);
            this.panData.Controls.Add(this.txtDetailTel);
            this.panData.Controls.Add(this.lblDetailHpTel);
            this.panData.Controls.Add(this.txtDetailHpTel);
            this.panData.Controls.Add(this.lblDetailEmail);
            this.panData.Controls.Add(this.txtDetailEmail);
            this.panData.Controls.Add(this.lblDetailNatCd);
            this.panData.Controls.Add(this.txtDetailNatCd);
            this.panData.Controls.Add(this.lblDetailZipCode);
            this.panData.Controls.Add(this.txtDetailZipCode);
            this.panData.Controls.Add(this.lblDetailAddr1);
            this.panData.Controls.Add(this.txtDetailAddr1);
            this.panData.Controls.Add(this.lblDetailAddr2);
            this.panData.Controls.Add(this.txtDetailAddr2);
            this.panData.Dock = System.Windows.Forms.DockStyle.Top;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(681, 321);
            this.panData.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblDetailHoliYn);
            this.groupBox1.Controls.Add(this.chkDetailPayYn);
            this.groupBox1.Controls.Add(this.lblDetailPayYn);
            this.groupBox1.Controls.Add(this.chkDetailDiligYn);
            this.groupBox1.Controls.Add(this.lblDetailDiligYn);
            this.groupBox1.Controls.Add(this.chkDetailHoliYn);
            this.groupBox1.Location = new System.Drawing.Point(13, 213);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(149, 88);
            this.groupBox1.TabIndex = 51;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Option";
            // 
            // lblDetailHoliYn
            // 
            this.lblDetailHoliYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailHoliYn.Appearance.Options.UseFont = true;
            this.lblDetailHoliYn.Location = new System.Drawing.Point(20, 18);
            this.lblDetailHoliYn.Name = "lblDetailHoliYn";
            this.lblDetailHoliYn.Size = new System.Drawing.Size(48, 15);
            this.lblDetailHoliYn.TabIndex = 40;
            this.lblDetailHoliYn.Text = "연차대상";
            // 
            // chkDetailPayYn
            // 
            this.chkDetailPayYn.Location = new System.Drawing.Point(102, 63);
            this.chkDetailPayYn.Name = "chkDetailPayYn";
            this.chkDetailPayYn.Properties.Caption = "";
            this.chkDetailPayYn.Size = new System.Drawing.Size(16, 20);
            this.chkDetailPayYn.TabIndex = 2;
            // 
            // lblDetailPayYn
            // 
            this.lblDetailPayYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailPayYn.Appearance.Options.UseFont = true;
            this.lblDetailPayYn.Location = new System.Drawing.Point(20, 66);
            this.lblDetailPayYn.Name = "lblDetailPayYn";
            this.lblDetailPayYn.Size = new System.Drawing.Size(48, 15);
            this.lblDetailPayYn.TabIndex = 44;
            this.lblDetailPayYn.Text = "급여대상";
            // 
            // chkDetailDiligYn
            // 
            this.chkDetailDiligYn.Location = new System.Drawing.Point(102, 39);
            this.chkDetailDiligYn.Name = "chkDetailDiligYn";
            this.chkDetailDiligYn.Properties.Caption = "";
            this.chkDetailDiligYn.Size = new System.Drawing.Size(16, 20);
            this.chkDetailDiligYn.TabIndex = 1;
            // 
            // lblDetailDiligYn
            // 
            this.lblDetailDiligYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailDiligYn.Appearance.Options.UseFont = true;
            this.lblDetailDiligYn.Location = new System.Drawing.Point(20, 42);
            this.lblDetailDiligYn.Name = "lblDetailDiligYn";
            this.lblDetailDiligYn.Size = new System.Drawing.Size(48, 15);
            this.lblDetailDiligYn.TabIndex = 42;
            this.lblDetailDiligYn.Text = "근태대상";
            // 
            // chkDetailHoliYn
            // 
            this.chkDetailHoliYn.Location = new System.Drawing.Point(102, 15);
            this.chkDetailHoliYn.Name = "chkDetailHoliYn";
            this.chkDetailHoliYn.Properties.Caption = "";
            this.chkDetailHoliYn.Size = new System.Drawing.Size(16, 20);
            this.chkDetailHoliYn.TabIndex = 0;
            // 
            // txtDetailDeptNm
            // 
            this.txtDetailDeptNm.Location = new System.Drawing.Point(255, 131);
            this.txtDetailDeptNm.LookupKey = "P_DEPT";
            this.txtDetailDeptNm.MatchField = "dept_nm";
            this.txtDetailDeptNm.Name = "txtDetailDeptNm";
            this.txtDetailDeptNm.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtDetailDeptNm.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDetailDeptNm.Properties.Appearance.Options.UseBackColor = true;
            this.txtDetailDeptNm.Properties.Appearance.Options.UseForeColor = true;
            this.txtDetailDeptNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", 24, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.txtDetailDeptNm.Required = true;
            this.txtDetailDeptNm.Size = new System.Drawing.Size(175, 20);
            this.txtDetailDeptNm.TabIndex = 4;
            this.txtDetailDeptNm.ToolTip = null;
            // 
            // cboDetailSexCd
            // 
            this.cboDetailSexCd.EditValue = "";
            this.cboDetailSexCd.Location = new System.Drawing.Point(493, 15);
            this.cboDetailSexCd.LookupKey = "L_CM0005";
            this.cboDetailSexCd.Name = "cboDetailSexCd";
            this.cboDetailSexCd.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.cboDetailSexCd.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboDetailSexCd.Properties.Appearance.Options.UseBackColor = true;
            this.cboDetailSexCd.Properties.Appearance.Options.UseForeColor = true;
            this.cboDetailSexCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailSexCd.Properties.NullText = "";
            this.cboDetailSexCd.Size = new System.Drawing.Size(175, 20);
            this.cboDetailSexCd.TabIndex = 7;
            // 
            // cboDetailAccCd
            // 
            this.cboDetailAccCd.EditValue = "";
            this.cboDetailAccCd.Location = new System.Drawing.Point(255, 15);
            this.cboDetailAccCd.LookupKey = "L_ACC";
            this.cboDetailAccCd.Name = "cboDetailAccCd";
            this.cboDetailAccCd.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.cboDetailAccCd.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboDetailAccCd.Properties.Appearance.Options.UseBackColor = true;
            this.cboDetailAccCd.Properties.Appearance.Options.UseForeColor = true;
            this.cboDetailAccCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailAccCd.Properties.NullText = "";
            this.cboDetailAccCd.Required = true;
            this.cboDetailAccCd.Size = new System.Drawing.Size(175, 20);
            this.cboDetailAccCd.TabIndex = 0;
            // 
            // picEmpPhoto
            // 
            this.picEmpPhoto.AllowDrop = true;
            this.picEmpPhoto.Cursor = System.Windows.Forms.Cursors.Default;
            this.picEmpPhoto.Location = new System.Drawing.Point(13, 16);
            this.picEmpPhoto.Name = "picEmpPhoto";
            this.picEmpPhoto.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.picEmpPhoto.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            this.picEmpPhoto.Size = new System.Drawing.Size(151, 188);
            this.picEmpPhoto.TabIndex = 48;
            // 
            // lblDetailAccId
            // 
            this.lblDetailAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailAccId.Appearance.Options.UseFont = true;
            this.lblDetailAccId.Location = new System.Drawing.Point(213, 22);
            this.lblDetailAccId.Name = "lblDetailAccId";
            this.lblDetailAccId.Size = new System.Drawing.Size(36, 15);
            this.lblDetailAccId.TabIndex = 0;
            this.lblDetailAccId.Text = "사업장";
            // 
            // lblDetailEmpNo
            // 
            this.lblDetailEmpNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailEmpNo.Appearance.Options.UseFont = true;
            this.lblDetailEmpNo.Location = new System.Drawing.Point(225, 50);
            this.lblDetailEmpNo.Name = "lblDetailEmpNo";
            this.lblDetailEmpNo.Size = new System.Drawing.Size(24, 15);
            this.lblDetailEmpNo.TabIndex = 2;
            this.lblDetailEmpNo.Text = "사번";
            // 
            // txtDetailEmpNo
            // 
            this.txtDetailEmpNo.Location = new System.Drawing.Point(255, 47);
            this.txtDetailEmpNo.Name = "txtDetailEmpNo";
            this.txtDetailEmpNo.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtDetailEmpNo.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDetailEmpNo.Properties.Appearance.Options.UseBackColor = true;
            this.txtDetailEmpNo.Properties.Appearance.Options.UseForeColor = true;
            this.txtDetailEmpNo.Required = true;
            this.txtDetailEmpNo.Size = new System.Drawing.Size(175, 20);
            this.txtDetailEmpNo.TabIndex = 1;
            // 
            // lblDetailEmpNm
            // 
            this.lblDetailEmpNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailEmpNm.Appearance.Options.UseFont = true;
            this.lblDetailEmpNm.Location = new System.Drawing.Point(213, 78);
            this.lblDetailEmpNm.Name = "lblDetailEmpNm";
            this.lblDetailEmpNm.Size = new System.Drawing.Size(36, 15);
            this.lblDetailEmpNm.TabIndex = 4;
            this.lblDetailEmpNm.Text = "사원명";
            // 
            // txtDetailEmpNm
            // 
            this.txtDetailEmpNm.Location = new System.Drawing.Point(255, 75);
            this.txtDetailEmpNm.Name = "txtDetailEmpNm";
            this.txtDetailEmpNm.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtDetailEmpNm.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDetailEmpNm.Properties.Appearance.Options.UseBackColor = true;
            this.txtDetailEmpNm.Properties.Appearance.Options.UseForeColor = true;
            this.txtDetailEmpNm.Required = true;
            this.txtDetailEmpNm.Size = new System.Drawing.Size(175, 20);
            this.txtDetailEmpNm.TabIndex = 2;
            // 
            // lblDetailEmpNmEng
            // 
            this.lblDetailEmpNmEng.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailEmpNmEng.Appearance.Options.UseFont = true;
            this.lblDetailEmpNmEng.Location = new System.Drawing.Point(181, 106);
            this.lblDetailEmpNmEng.Name = "lblDetailEmpNmEng";
            this.lblDetailEmpNmEng.Size = new System.Drawing.Size(68, 15);
            this.lblDetailEmpNmEng.TabIndex = 6;
            this.lblDetailEmpNmEng.Text = "사원명(영문)";
            // 
            // txtDetailDeptId
            // 
            this.txtDetailDeptId.Location = new System.Drawing.Point(540, 384);
            this.txtDetailDeptId.Name = "txtDetailDeptId";
            this.txtDetailDeptId.Size = new System.Drawing.Size(175, 20);
            this.txtDetailDeptId.TabIndex = 7;
            this.txtDetailDeptId.Visible = false;
            // 
            // txtDetailEmpNmEng
            // 
            this.txtDetailEmpNmEng.Location = new System.Drawing.Point(255, 103);
            this.txtDetailEmpNmEng.Name = "txtDetailEmpNmEng";
            this.txtDetailEmpNmEng.Size = new System.Drawing.Size(175, 20);
            this.txtDetailEmpNmEng.TabIndex = 3;
            // 
            // lblDetailDeptId
            // 
            this.lblDetailDeptId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailDeptId.Appearance.Options.UseFont = true;
            this.lblDetailDeptId.Location = new System.Drawing.Point(436, 383);
            this.lblDetailDeptId.Name = "lblDetailDeptId";
            this.lblDetailDeptId.Size = new System.Drawing.Size(36, 15);
            this.lblDetailDeptId.TabIndex = 8;
            this.lblDetailDeptId.Text = "부서ID";
            this.lblDetailDeptId.Visible = false;
            // 
            // lblDetailDeptNm
            // 
            this.lblDetailDeptNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailDeptNm.Appearance.Options.UseFont = true;
            this.lblDetailDeptNm.Location = new System.Drawing.Point(225, 132);
            this.lblDetailDeptNm.Name = "lblDetailDeptNm";
            this.lblDetailDeptNm.Size = new System.Drawing.Size(24, 15);
            this.lblDetailDeptNm.TabIndex = 10;
            this.lblDetailDeptNm.Text = "부서";
            // 
            // lblDetailEntDate
            // 
            this.lblDetailEntDate.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailEntDate.Appearance.Options.UseFont = true;
            this.lblDetailEntDate.Location = new System.Drawing.Point(201, 160);
            this.lblDetailEntDate.Name = "lblDetailEntDate";
            this.lblDetailEntDate.Size = new System.Drawing.Size(48, 15);
            this.lblDetailEntDate.TabIndex = 12;
            this.lblDetailEntDate.Text = "입사일자";
            // 
            // dteDetailEntDate
            // 
            this.dteDetailEntDate.EditValue = new System.DateTime(2026, 9, 9, 0, 0, 0, 0);
            this.dteDetailEntDate.Location = new System.Drawing.Point(255, 157);
            this.dteDetailEntDate.Name = "dteDetailEntDate";
            this.dteDetailEntDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteDetailEntDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteDetailEntDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dteDetailEntDate.Size = new System.Drawing.Size(100, 20);
            this.dteDetailEntDate.TabIndex = 5;
            this.dteDetailEntDate.YyyyMmDd = "20260909";
            // 
            // lblDetailGrpEntDate
            // 
            this.lblDetailGrpEntDate.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailGrpEntDate.Appearance.Options.UseFont = true;
            this.lblDetailGrpEntDate.Location = new System.Drawing.Point(177, 188);
            this.lblDetailGrpEntDate.Name = "lblDetailGrpEntDate";
            this.lblDetailGrpEntDate.Size = new System.Drawing.Size(72, 15);
            this.lblDetailGrpEntDate.TabIndex = 14;
            this.lblDetailGrpEntDate.Text = "그룹입사일자";
            // 
            // dteDetailGrpEntDate
            // 
            this.dteDetailGrpEntDate.EditValue = new System.DateTime(2026, 9, 9, 0, 0, 0, 0);
            this.dteDetailGrpEntDate.Location = new System.Drawing.Point(255, 185);
            this.dteDetailGrpEntDate.Name = "dteDetailGrpEntDate";
            this.dteDetailGrpEntDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteDetailGrpEntDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteDetailGrpEntDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dteDetailGrpEntDate.Size = new System.Drawing.Size(100, 20);
            this.dteDetailGrpEntDate.TabIndex = 6;
            this.dteDetailGrpEntDate.YyyyMmDd = "20260909";
            // 
            // lblDetailJobGrade
            // 
            this.lblDetailJobGrade.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailJobGrade.Appearance.Options.UseFont = true;
            this.lblDetailJobGrade.Location = new System.Drawing.Point(463, 158);
            this.lblDetailJobGrade.Name = "lblDetailJobGrade";
            this.lblDetailJobGrade.Size = new System.Drawing.Size(24, 15);
            this.lblDetailJobGrade.TabIndex = 16;
            this.lblDetailJobGrade.Text = "직잭";
            // 
            // txtDetailJobGrade
            // 
            this.txtDetailJobGrade.Location = new System.Drawing.Point(493, 155);
            this.txtDetailJobGrade.Name = "txtDetailJobGrade";
            this.txtDetailJobGrade.Size = new System.Drawing.Size(175, 20);
            this.txtDetailJobGrade.TabIndex = 13;
            // 
            // lblDetailJobType
            // 
            this.lblDetailJobType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailJobType.Appearance.Options.UseFont = true;
            this.lblDetailJobType.Location = new System.Drawing.Point(463, 188);
            this.lblDetailJobType.Name = "lblDetailJobType";
            this.lblDetailJobType.Size = new System.Drawing.Size(24, 15);
            this.lblDetailJobType.TabIndex = 18;
            this.lblDetailJobType.Text = "직무";
            // 
            // txtDetailJobType
            // 
            this.txtDetailJobType.Location = new System.Drawing.Point(493, 185);
            this.txtDetailJobType.Name = "txtDetailJobType";
            this.txtDetailJobType.Size = new System.Drawing.Size(175, 20);
            this.txtDetailJobType.TabIndex = 14;
            // 
            // lblDetailRetYn
            // 
            this.lblDetailRetYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailRetYn.Appearance.Options.UseFont = true;
            this.lblDetailRetYn.Location = new System.Drawing.Point(457, 130);
            this.lblDetailRetYn.Name = "lblDetailRetYn";
            this.lblDetailRetYn.Size = new System.Drawing.Size(24, 15);
            this.lblDetailRetYn.TabIndex = 20;
            this.lblDetailRetYn.Text = "퇴사";
            // 
            // chkDetailRetYn
            // 
            this.chkDetailRetYn.Location = new System.Drawing.Point(487, 127);
            this.chkDetailRetYn.Name = "chkDetailRetYn";
            this.chkDetailRetYn.Properties.Caption = "";
            this.chkDetailRetYn.Size = new System.Drawing.Size(26, 20);
            this.chkDetailRetYn.TabIndex = 11;
            // 
            // lblDetailRetDate
            // 
            this.lblDetailRetDate.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailRetDate.Appearance.Options.UseFont = true;
            this.lblDetailRetDate.Location = new System.Drawing.Point(514, 130);
            this.lblDetailRetDate.Name = "lblDetailRetDate";
            this.lblDetailRetDate.Size = new System.Drawing.Size(48, 15);
            this.lblDetailRetDate.TabIndex = 22;
            this.lblDetailRetDate.Text = "퇴사일자";
            // 
            // dteDetailRetDate
            // 
            this.dteDetailRetDate.EditValue = new System.DateTime(2026, 9, 9, 0, 0, 0, 0);
            this.dteDetailRetDate.Location = new System.Drawing.Point(568, 125);
            this.dteDetailRetDate.Name = "dteDetailRetDate";
            this.dteDetailRetDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteDetailRetDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteDetailRetDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dteDetailRetDate.Size = new System.Drawing.Size(100, 20);
            this.dteDetailRetDate.TabIndex = 12;
            this.dteDetailRetDate.YyyyMmDd = "20260909";
            // 
            // lblDetailSexCd
            // 
            this.lblDetailSexCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailSexCd.Appearance.Options.UseFont = true;
            this.lblDetailSexCd.Location = new System.Drawing.Point(463, 18);
            this.lblDetailSexCd.Name = "lblDetailSexCd";
            this.lblDetailSexCd.Size = new System.Drawing.Size(24, 15);
            this.lblDetailSexCd.TabIndex = 24;
            this.lblDetailSexCd.Text = "성별";
            // 
            // lblDetailTel
            // 
            this.lblDetailTel.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailTel.Appearance.Options.UseFont = true;
            this.lblDetailTel.Location = new System.Drawing.Point(451, 46);
            this.lblDetailTel.Name = "lblDetailTel";
            this.lblDetailTel.Size = new System.Drawing.Size(36, 15);
            this.lblDetailTel.TabIndex = 26;
            this.lblDetailTel.Text = "연락처";
            // 
            // txtDetailTel
            // 
            this.txtDetailTel.Location = new System.Drawing.Point(493, 43);
            this.txtDetailTel.Name = "txtDetailTel";
            this.txtDetailTel.Size = new System.Drawing.Size(175, 20);
            this.txtDetailTel.TabIndex = 8;
            // 
            // lblDetailHpTel
            // 
            this.lblDetailHpTel.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailHpTel.Appearance.Options.UseFont = true;
            this.lblDetailHpTel.Location = new System.Drawing.Point(445, 74);
            this.lblDetailHpTel.Name = "lblDetailHpTel";
            this.lblDetailHpTel.Size = new System.Drawing.Size(42, 15);
            this.lblDetailHpTel.TabIndex = 28;
            this.lblDetailHpTel.Text = "MOBILE";
            // 
            // txtDetailHpTel
            // 
            this.txtDetailHpTel.Location = new System.Drawing.Point(493, 71);
            this.txtDetailHpTel.Name = "txtDetailHpTel";
            this.txtDetailHpTel.Size = new System.Drawing.Size(175, 20);
            this.txtDetailHpTel.TabIndex = 9;
            // 
            // lblDetailEmail
            // 
            this.lblDetailEmail.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailEmail.Appearance.Options.UseFont = true;
            this.lblDetailEmail.Location = new System.Drawing.Point(448, 102);
            this.lblDetailEmail.Name = "lblDetailEmail";
            this.lblDetailEmail.Size = new System.Drawing.Size(39, 15);
            this.lblDetailEmail.TabIndex = 30;
            this.lblDetailEmail.Text = "E-MAIL";
            // 
            // txtDetailEmail
            // 
            this.txtDetailEmail.Location = new System.Drawing.Point(493, 99);
            this.txtDetailEmail.Name = "txtDetailEmail";
            this.txtDetailEmail.Size = new System.Drawing.Size(175, 20);
            this.txtDetailEmail.TabIndex = 10;
            // 
            // lblDetailNatCd
            // 
            this.lblDetailNatCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailNatCd.Appearance.Options.UseFont = true;
            this.lblDetailNatCd.Location = new System.Drawing.Point(508, 361);
            this.lblDetailNatCd.Name = "lblDetailNatCd";
            this.lblDetailNatCd.Size = new System.Drawing.Size(24, 15);
            this.lblDetailNatCd.TabIndex = 32;
            this.lblDetailNatCd.Text = "국가";
            this.lblDetailNatCd.Visible = false;
            // 
            // txtDetailNatCd
            // 
            this.txtDetailNatCd.Location = new System.Drawing.Point(538, 358);
            this.txtDetailNatCd.Name = "txtDetailNatCd";
            this.txtDetailNatCd.Size = new System.Drawing.Size(220, 20);
            this.txtDetailNatCd.TabIndex = 33;
            this.txtDetailNatCd.Visible = false;
            // 
            // lblDetailZipCode
            // 
            this.lblDetailZipCode.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailZipCode.Appearance.Options.UseFont = true;
            this.lblDetailZipCode.Location = new System.Drawing.Point(201, 215);
            this.lblDetailZipCode.Name = "lblDetailZipCode";
            this.lblDetailZipCode.Size = new System.Drawing.Size(48, 15);
            this.lblDetailZipCode.TabIndex = 34;
            this.lblDetailZipCode.Text = "우편번호";
            // 
            // txtDetailZipCode
            // 
            this.txtDetailZipCode.Location = new System.Drawing.Point(255, 212);
            this.txtDetailZipCode.Name = "txtDetailZipCode";
            this.txtDetailZipCode.Size = new System.Drawing.Size(100, 20);
            this.txtDetailZipCode.TabIndex = 15;
            // 
            // lblDetailAddr1
            // 
            this.lblDetailAddr1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailAddr1.Appearance.Options.UseFont = true;
            this.lblDetailAddr1.Location = new System.Drawing.Point(218, 243);
            this.lblDetailAddr1.Name = "lblDetailAddr1";
            this.lblDetailAddr1.Size = new System.Drawing.Size(31, 15);
            this.lblDetailAddr1.TabIndex = 36;
            this.lblDetailAddr1.Text = "주소1";
            // 
            // txtDetailAddr1
            // 
            this.txtDetailAddr1.Location = new System.Drawing.Point(255, 240);
            this.txtDetailAddr1.Name = "txtDetailAddr1";
            this.txtDetailAddr1.Size = new System.Drawing.Size(413, 20);
            this.txtDetailAddr1.TabIndex = 16;
            // 
            // lblDetailAddr2
            // 
            this.lblDetailAddr2.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailAddr2.Appearance.Options.UseFont = true;
            this.lblDetailAddr2.Location = new System.Drawing.Point(218, 271);
            this.lblDetailAddr2.Name = "lblDetailAddr2";
            this.lblDetailAddr2.Size = new System.Drawing.Size(31, 15);
            this.lblDetailAddr2.TabIndex = 38;
            this.lblDetailAddr2.Text = "주소2";
            // 
            // txtDetailAddr2
            // 
            this.txtDetailAddr2.Location = new System.Drawing.Point(255, 268);
            this.txtDetailAddr2.Name = "txtDetailAddr2";
            this.txtDetailAddr2.Size = new System.Drawing.Size(413, 20);
            this.txtDetailAddr2.TabIndex = 17;
            // 
            // panelWyn6
            // 
            this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
            this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn6.Location = new System.Drawing.Point(0, 0);
            this.panelWyn6.Name = "panelWyn6";
            this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn6.Size = new System.Drawing.Size(681, 27);
            this.panelWyn6.TabIndex = 7;
            // 
            // sectionHeaderWyn3
            // 
            this.sectionHeaderWyn3.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn3.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn3.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn3.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn3.Name = "sectionHeaderWyn3";
            this.sectionHeaderWyn3.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.sectionHeaderWyn3.Size = new System.Drawing.Size(676, 25);
            this.sectionHeaderWyn3.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn3.SvgIcon")));
            this.sectionHeaderWyn3.TabIndex = 9;
            this.sectionHeaderWyn3.Text = "사원정보 등록";
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.lblSearchEmpNo);
            this.panHeader.Controls.Add(this.txtDeptNm);
            this.panHeader.Controls.Add(this.txtEmpNo);
            this.panHeader.Controls.Add(this.lblSearchDeptId);
            this.panHeader.Controls.Add(this.txtDeptId);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 25);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1678, 49);
            this.panHeader.TabIndex = 8;
            // 
            // lblSearchEmpNo
            // 
            this.lblSearchEmpNo.Location = new System.Drawing.Point(17, 20);
            this.lblSearchEmpNo.Name = "lblSearchEmpNo";
            this.lblSearchEmpNo.Size = new System.Drawing.Size(55, 14);
            this.lblSearchEmpNo.TabIndex = 1;
            this.lblSearchEmpNo.Text = "사번/사원명";
            // 
            // txtDeptNm
            // 
            this.txtDeptNm.Location = new System.Drawing.Point(288, 17);
            this.txtDeptNm.LookupKey = "P_DEPT";
            this.txtDeptNm.Name = "txtDeptNm";
            this.txtDeptNm.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtDeptNm.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDeptNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", 24, true, true, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.txtDeptNm.Size = new System.Drawing.Size(175, 20);
            this.txtDeptNm.TabIndex = 1;
            this.txtDeptNm.ToolTip = null;
            // 
            // txtEmpNo
            // 
            this.txtEmpNo.Location = new System.Drawing.Point(79, 17);
            this.txtEmpNo.Name = "txtEmpNo";
            this.txtEmpNo.Size = new System.Drawing.Size(150, 20);
            this.txtEmpNo.TabIndex = 0;
            // 
            // lblSearchDeptId
            // 
            this.lblSearchDeptId.Location = new System.Drawing.Point(262, 20);
            this.lblSearchDeptId.Name = "lblSearchDeptId";
            this.lblSearchDeptId.Size = new System.Drawing.Size(20, 14);
            this.lblSearchDeptId.TabIndex = 3;
            this.lblSearchDeptId.Text = "부서";
            // 
            // txtDeptId
            // 
            this.txtDeptId.Location = new System.Drawing.Point(844, 21);
            this.txtDeptId.Name = "txtDeptId";
            this.txtDeptId.Size = new System.Drawing.Size(150, 20);
            this.txtDeptId.TabIndex = 4;
            this.txtDeptId.Visible = false;
            // 
            // paTitleH
            // 
            this.paTitleH.Controls.Add(this.sectionHeaderWyn1);
            this.paTitleH.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitleH.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitleH.Location = new System.Drawing.Point(5, 0);
            this.paTitleH.Name = "paTitleH";
            this.paTitleH.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.paTitleH.Size = new System.Drawing.Size(1678, 25);
            this.paTitleH.TabIndex = 10;
            // 
            // sectionHeaderWyn1
            // 
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1673, 25);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 14;
            this.sectionHeaderWyn1.Text = "사원등록 [frmEmp]";
            // 
            // frmEMP
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1688, 818);
            this.Controls.Add(this.panBase);
            this.Name = "frmEMP";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).EndInit();
            this.panelWyn8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpColumnEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditcolM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateColumnEdit1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateColumnEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateColumnEdit2.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateColumnEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEditcolM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateColumnEdit3.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateColumnEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpColumnEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcolM.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcolM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
            this.panelWyn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
            this.panData.ResumeLayout(false);
            this.panData.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkDetailPayYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDetailDiligYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDetailHoliYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailDeptNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailSexCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailAccCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEmpPhoto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailEmpNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailEmpNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailDeptId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailEmpNmEng.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDetailEntDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDetailEntDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDetailGrpEntDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDetailGrpEntDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailJobGrade.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailJobType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDetailRetYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDetailRetDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDetailRetDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailTel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailHpTel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailEmail.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailNatCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailZipCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailAddr1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailAddr2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
            this.panelWyn6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).EndInit();
            this.paTitleH.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private PanelWyn panBase;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn4;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblDetailAccId;
    private DevExpress.XtraEditors.LabelControl lblDetailEmpNo;
    private TextEditWyn txtDetailEmpNo;
    private DevExpress.XtraEditors.LabelControl lblDetailEmpNm;
    private TextEditWyn txtDetailEmpNm;
    private DevExpress.XtraEditors.LabelControl lblDetailEmpNmEng;
    private TextEditWyn txtDetailEmpNmEng;
    private DevExpress.XtraEditors.LabelControl lblDetailDeptId;
    private DevExpress.XtraEditors.LabelControl lblDetailDeptNm;
    private DevExpress.XtraEditors.LabelControl lblDetailEntDate;
    private DateEditWyn dteDetailEntDate;
    private DevExpress.XtraEditors.LabelControl lblDetailGrpEntDate;
    private DateEditWyn dteDetailGrpEntDate;
    private DevExpress.XtraEditors.LabelControl lblDetailJobGrade;
    private TextEditWyn txtDetailJobGrade;
    private DevExpress.XtraEditors.LabelControl lblDetailJobType;
    private TextEditWyn txtDetailJobType;
    private DevExpress.XtraEditors.LabelControl lblDetailRetYn;
    private CheckBoxWyn chkDetailRetYn;
    private DevExpress.XtraEditors.LabelControl lblDetailRetDate;
    private DateEditWyn dteDetailRetDate;
    private DevExpress.XtraEditors.LabelControl lblDetailSexCd;
    private DevExpress.XtraEditors.LabelControl lblDetailTel;
    private TextEditWyn txtDetailTel;
    private DevExpress.XtraEditors.LabelControl lblDetailHpTel;
    private TextEditWyn txtDetailHpTel;
    private DevExpress.XtraEditors.LabelControl lblDetailEmail;
    private TextEditWyn txtDetailEmail;
    private DevExpress.XtraEditors.LabelControl lblDetailNatCd;
    private TextEditWyn txtDetailNatCd;
    private DevExpress.XtraEditors.LabelControl lblDetailZipCode;
    private TextEditWyn txtDetailZipCode;
    private DevExpress.XtraEditors.LabelControl lblDetailAddr1;
    private TextEditWyn txtDetailAddr1;
    private DevExpress.XtraEditors.LabelControl lblDetailAddr2;
    private TextEditWyn txtDetailAddr2;
    private DevExpress.XtraEditors.LabelControl lblDetailHoliYn;
    private CheckBoxWyn chkDetailHoliYn;
    private DevExpress.XtraEditors.LabelControl lblDetailDiligYn;
    private CheckBoxWyn chkDetailDiligYn;
    private DevExpress.XtraEditors.LabelControl lblDetailPayYn;
    private CheckBoxWyn chkDetailPayYn;
    private PanelWyn panelWyn6;
    private PanelWyn panelWyn8;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colMEmpId;
    private DevExpress.XtraGrid.Columns.GridColumn colMAccId;
    private DevExpress.XtraGrid.Columns.GridColumn colMEmpNo;
    private DevExpress.XtraGrid.Columns.GridColumn colMEmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn colMEmpNmEng;
    private DevExpress.XtraGrid.Columns.GridColumn colMDeptId;
    private DevExpress.XtraGrid.Columns.GridColumn colMDeptNm;
    private DevExpress.XtraGrid.Columns.GridColumn colMEntDate;
    private DevExpress.XtraGrid.Columns.GridColumn colMGrpEntDate;
    private DevExpress.XtraGrid.Columns.GridColumn colMJobGrade;
    private DevExpress.XtraGrid.Columns.GridColumn colMJobType;
    private DevExpress.XtraGrid.Columns.GridColumn colMRetYn;
    private DevExpress.XtraGrid.Columns.GridColumn colMRetDate;
    private DevExpress.XtraGrid.Columns.GridColumn colMSexCd;
    private DevExpress.XtraGrid.Columns.GridColumn colMTel;
    private DevExpress.XtraGrid.Columns.GridColumn colMHpTel;
    private DevExpress.XtraGrid.Columns.GridColumn colMEmail;
    private DevExpress.XtraGrid.Columns.GridColumn colMNatCd;
    private DevExpress.XtraGrid.Columns.GridColumn colMZipCode;
    private DevExpress.XtraGrid.Columns.GridColumn colMAddr1;
    private DevExpress.XtraGrid.Columns.GridColumn colMAddr2;
    private DevExpress.XtraGrid.Columns.GridColumn colMHoliYn;
    private DevExpress.XtraGrid.Columns.GridColumn colMDiligYn;
    private DevExpress.XtraGrid.Columns.GridColumn colMPayYn;
    private DevExpress.XtraGrid.Columns.GridColumn colMPhoto;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkEditcolM;
    private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spinEditcolM;
    private WYNLAB.Base.Controls.DateColumnEdit dateEditcolM;
    private PanelWyn panelWyn2;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchEmpNo;
    private TextEditWyn txtEmpNo;
    private DevExpress.XtraEditors.LabelControl lblSearchDeptId;
    private TextEditWyn txtDeptId;
    private SectionHeaderWyn sectionHeaderWyn3;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PictureEditWyn picEmpPhoto;
    private LookUpEditWyn cboDetailAccCd;
    private PopupLookupEditWyn txtDetailDeptNm;
    private GroupBox groupBox1;
    private TextEditWyn txtDetailDeptId;
    private LookUpEditWyn cboDetailSexCd;
    private LookUpColumnEdit lookUpColumnEdit1;
    private SplitterWyn splitterWyn1;
    private PopupLookupEditWyn txtDeptNm;
    private DateColumnEdit dateColumnEdit1;
    private DateColumnEdit dateColumnEdit2;
    private DateColumnEdit dateColumnEdit3;
    private LookUpColumnEdit lookUpColumnEdit2;
    private PanelWyn paTitleH;
    private SectionHeaderWyn sectionHeaderWyn1;
}
