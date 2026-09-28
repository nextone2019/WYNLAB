// AI Builder가 트리마스터-서브그리드 템플릿을 복제해서 자동 생성 - 2026-09-09.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
#nullable disable
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmDept
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDept));
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.tabDetailGrids = new WYNLAB.Base.Controls.TabControlWyn();
            this.tabDetail1 = new DevExpress.XtraTab.XtraTabPage();
            this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colD1EmpNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1EmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1EmpNmEng = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1JobGrade = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1JobType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1Tel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1HpTel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1Email = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDeletRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAddRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.txtDetailRemark = new WYNLAB.Base.Controls.MemoEditWyn();
            this.lblDetailAccId = new DevExpress.XtraEditors.LabelControl();
            this.cboDetailAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.numDetailDeptId = new WYNLAB.Base.Controls.SpinEditWyn();
            this.lblDetailDeptNm = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailDeptNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.numDetailParDeptId = new WYNLAB.Base.Controls.SpinEditWyn();
            this.lblDetailParDeptNm = new DevExpress.XtraEditors.LabelControl();
            this.popDetailParDeptNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.lblDetailDeptType = new DevExpress.XtraEditors.LabelControl();
            this.cboDetailDeptType = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblDetailRemark = new DevExpress.XtraEditors.LabelControl();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.tree1 = new WYNLAB.Base.Controls.TreeListWyn();
            this.treeColAccId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.lookUpTreeAccId = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.treeColDeptId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.spinEditTree = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.treeColDeptNm = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeColParDeptId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeColParDeptNm = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeColDeptType = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.lookUpTreeDeptType = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.treeColRemark = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtDeptId = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSearchDeptNm = new DevExpress.XtraEditors.LabelControl();
            this.txtDeptNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.paTitleH = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
            this.panelWyn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
            this.panelWyn4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabDetailGrids)).BeginInit();
            this.tabDetailGrids.SuspendLayout();
            this.tabDetail1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailAccId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDetailDeptId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailDeptNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDetailParDeptId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popDetailParDeptNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailDeptType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
            this.panelWyn6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).BeginInit();
            this.panelWyn8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tree1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpTreeAccId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditTree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpTreeDeptType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
            this.panelWyn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).BeginInit();
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
            this.panBase.Size = new System.Drawing.Size(1245, 580);
            this.panBase.TabIndex = 6;
            // 
            // panelWyn3
            // 
            this.panelWyn3.Controls.Add(this.panelWyn4);
            this.panelWyn3.Controls.Add(this.splitterWyn1);
            this.panelWyn3.Controls.Add(this.panelWyn8);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(5, 74);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Size = new System.Drawing.Size(1235, 501);
            this.panelWyn3.TabIndex = 7;
            // 
            // panelWyn4
            // 
            this.panelWyn4.Controls.Add(this.tabDetailGrids);
            this.panelWyn4.Controls.Add(this.panelWyn7);
            this.panelWyn4.Controls.Add(this.panelWyn1);
            this.panelWyn4.Controls.Add(this.panelWyn5);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(412, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelWyn4.Size = new System.Drawing.Size(823, 501);
            this.panelWyn4.TabIndex = 7;
            // 
            // tabDetailGrids
            // 
            this.tabDetailGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabDetailGrids.Location = new System.Drawing.Point(3, 272);
            this.tabDetailGrids.Name = "tabDetailGrids";
            this.tabDetailGrids.SelectedTabPage = this.tabDetail1;
            this.tabDetailGrids.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            this.tabDetailGrids.Size = new System.Drawing.Size(820, 229);
            this.tabDetailGrids.TabIndex = 7;
            this.tabDetailGrids.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabDetail1});
            // 
            // tabDetail1
            // 
            this.tabDetail1.Controls.Add(this.grd2);
            this.tabDetail1.Name = "tabDetail1";
            this.tabDetail1.Size = new System.Drawing.Size(818, 227);
            this.tabDetail1.Text = "소속 사원정보     ";
            // 
            // grd2
            // 
            this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd2.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grd2.Location = new System.Drawing.Point(0, 0);
            this.grd2.MainView = this.gvw2;
            this.grd2.Name = "grd2";
            this.grd2.Size = new System.Drawing.Size(818, 227);
            this.grd2.TabIndex = 0;
            this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw2});
            // 
            // gvw2
            // 
            this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colD1EmpNo,
            this.colD1EmpNm,
            this.colD1EmpNmEng,
            this.colD1JobGrade,
            this.colD1JobType,
            this.colD1Tel,
            this.colD1HpTel,
            this.colD1Email});
            this.gvw2.GridControl = this.grd2;
            this.gvw2.HighlightFocusedRow = true;
            this.gvw2.Name = "gvw2";
            this.gvw2.OptionsBehavior.Editable = false;
            this.gvw2.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw2.OptionsView.ColumnAutoWidth = false;
            this.gvw2.OptionsView.ShowGroupPanel = false;
            // 
            // colD1EmpNo
            // 
            this.colD1EmpNo.Caption = "사번";
            this.colD1EmpNo.FieldName = "emp_no";
            this.colD1EmpNo.Name = "colD1EmpNo";
            this.colD1EmpNo.Visible = true;
            this.colD1EmpNo.VisibleIndex = 0;
            this.colD1EmpNo.Width = 100;
            // 
            // colD1EmpNm
            // 
            this.colD1EmpNm.Caption = "사원명";
            this.colD1EmpNm.FieldName = "emp_nm";
            this.colD1EmpNm.Name = "colD1EmpNm";
            this.colD1EmpNm.Visible = true;
            this.colD1EmpNm.VisibleIndex = 1;
            this.colD1EmpNm.Width = 100;
            // 
            // colD1EmpNmEng
            // 
            this.colD1EmpNmEng.Caption = "사원명(영문)";
            this.colD1EmpNmEng.FieldName = "emp_nm_eng";
            this.colD1EmpNmEng.Name = "colD1EmpNmEng";
            this.colD1EmpNmEng.Visible = true;
            this.colD1EmpNmEng.VisibleIndex = 2;
            this.colD1EmpNmEng.Width = 100;
            // 
            // colD1JobGrade
            // 
            this.colD1JobGrade.Caption = "직책";
            this.colD1JobGrade.FieldName = "job_grade";
            this.colD1JobGrade.Name = "colD1JobGrade";
            this.colD1JobGrade.Visible = true;
            this.colD1JobGrade.VisibleIndex = 3;
            this.colD1JobGrade.Width = 100;
            // 
            // colD1JobType
            // 
            this.colD1JobType.Caption = "직무";
            this.colD1JobType.FieldName = "job_type";
            this.colD1JobType.Name = "colD1JobType";
            this.colD1JobType.Visible = true;
            this.colD1JobType.VisibleIndex = 4;
            this.colD1JobType.Width = 100;
            // 
            // colD1Tel
            // 
            this.colD1Tel.Caption = "연락처";
            this.colD1Tel.FieldName = "tel";
            this.colD1Tel.Name = "colD1Tel";
            this.colD1Tel.Visible = true;
            this.colD1Tel.VisibleIndex = 5;
            this.colD1Tel.Width = 100;
            // 
            // colD1HpTel
            // 
            this.colD1HpTel.Caption = "Mobile";
            this.colD1HpTel.FieldName = "hp_tel";
            this.colD1HpTel.Name = "colD1HpTel";
            this.colD1HpTel.Visible = true;
            this.colD1HpTel.VisibleIndex = 6;
            this.colD1HpTel.Width = 100;
            // 
            // colD1Email
            // 
            this.colD1Email.Caption = "E-mail";
            this.colD1Email.FieldName = "email";
            this.colD1Email.Name = "colD1Email";
            this.colD1Email.Visible = true;
            this.colD1Email.VisibleIndex = 7;
            this.colD1Email.Width = 100;
            // 
            // panelWyn1
            // 
            this.panelWyn1.Controls.Add(this.sectionHeaderWyn2);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(3, 245);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn1.Size = new System.Drawing.Size(820, 27);
            this.panelWyn1.TabIndex = 8;
            // 
            // sectionHeaderWyn2
            // 
            this.sectionHeaderWyn2.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn2.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn2.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn2.Name = "sectionHeaderWyn2";
            this.sectionHeaderWyn2.Size = new System.Drawing.Size(747, 25);
            this.sectionHeaderWyn2.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn2.SvgIcon")));
            this.sectionHeaderWyn2.TabIndex = 10;
            this.sectionHeaderWyn2.Text = "소속 인원 정보";
            // 
            // panelWyn7
            // 
            this.panelWyn7.Controls.Add(this.btnDeletRow2);
            this.panelWyn7.Controls.Add(this.btnAddRow2);
            this.panelWyn7.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn7.Appearance.Options.UseBackColor = true;
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(752, 0);
            this.panelWyn7.Name = "panelWyn7";
            this.panelWyn7.Padding = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.panelWyn7.Size = new System.Drawing.Size(68, 30);
            this.panelWyn7.TabIndex = 9;
            // 
            // btnDeletRow2
            // 
            this.btnDeletRow2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeletRow2.BackColor = System.Drawing.Color.Transparent;
            this.btnDeletRow2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(198)))), ((int)(((byte)(193)))));
            this.btnDeletRow2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeletRow2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(236)))), ((int)(((byte)(234)))));
            this.btnDeletRow2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDeletRow2.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDeletRow2.Image = null;
            this.btnDeletRow2.Location = new System.Drawing.Point(68, 4);
            this.btnDeletRow2.Name = "btnDeletRow2";
            this.btnDeletRow2.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDeletRow2.Size = new System.Drawing.Size(58, 22);
            this.btnDeletRow2.Text = "행삭제";
            this.btnDeletRow2.TabIndex = 0;
            this.btnDeletRow2.ToolTip = "행삭제(현재 탭)";
            // 
            // btnAddRow2
            // 
            this.btnAddRow2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddRow2.BackColor = System.Drawing.Color.Transparent;
            this.btnAddRow2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(231)))), ((int)(((byte)(203)))));
            this.btnAddRow2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddRow2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(245)))), ((int)(((byte)(231)))));
            this.btnAddRow2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnAddRow2.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnAddRow2.Image = null;
            this.btnAddRow2.Location = new System.Drawing.Point(6, 4);
            this.btnAddRow2.Name = "btnAddRow2";
            this.btnAddRow2.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnAddRow2.Size = new System.Drawing.Size(58, 22);
            this.btnAddRow2.Text = "행추가";
            this.btnAddRow2.TabIndex = 0;
            this.btnAddRow2.ToolTip = "행추가(현재 탭)";
            // 
            // panelWyn5
            // 
            this.panelWyn5.Controls.Add(this.panData);
            this.panelWyn5.Controls.Add(this.panelWyn6);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(3, 0);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Size = new System.Drawing.Size(820, 245);
            this.panelWyn5.TabIndex = 6;
            // 
            // panData
            // 
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Controls.Add(this.txtDetailRemark);
            this.panData.Controls.Add(this.lblDetailAccId);
            this.panData.Controls.Add(this.cboDetailAccId);
            this.panData.Controls.Add(this.numDetailDeptId);
            this.panData.Controls.Add(this.lblDetailDeptNm);
            this.panData.Controls.Add(this.txtDetailDeptNm);
            this.panData.Controls.Add(this.numDetailParDeptId);
            this.panData.Controls.Add(this.lblDetailParDeptNm);
            this.panData.Controls.Add(this.popDetailParDeptNm);
            this.panData.Controls.Add(this.lblDetailDeptType);
            this.panData.Controls.Add(this.cboDetailDeptType);
            this.panData.Controls.Add(this.lblDetailRemark);
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(820, 218);
            this.panData.TabIndex = 8;
            // 
            // txtDetailRemark
            // 
            this.txtDetailRemark.Location = new System.Drawing.Point(84, 137);
            this.txtDetailRemark.Name = "txtDetailRemark";
            this.txtDetailRemark.Size = new System.Drawing.Size(451, 64);
            this.txtDetailRemark.TabIndex = 13;
            // 
            // lblDetailAccId
            // 
            this.lblDetailAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailAccId.Appearance.Options.UseFont = true;
            this.lblDetailAccId.Location = new System.Drawing.Point(43, 19);
            this.lblDetailAccId.Name = "lblDetailAccId";
            this.lblDetailAccId.Size = new System.Drawing.Size(36, 15);
            this.lblDetailAccId.TabIndex = 0;
            this.lblDetailAccId.Text = "사업장";
            // 
            // cboDetailAccId
            // 
            this.cboDetailAccId.EditValue = "";
            this.cboDetailAccId.Location = new System.Drawing.Point(84, 16);
            this.cboDetailAccId.LookupKey = "L_ACC";
            this.cboDetailAccId.Name = "cboDetailAccId";
            this.cboDetailAccId.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.cboDetailAccId.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboDetailAccId.Properties.Appearance.Options.UseBackColor = true;
            this.cboDetailAccId.Properties.Appearance.Options.UseForeColor = true;
            this.cboDetailAccId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailAccId.Properties.NullText = "";
            this.cboDetailAccId.Required = true;
            this.cboDetailAccId.Size = new System.Drawing.Size(220, 20);
            this.cboDetailAccId.TabIndex = 1;
            // 
            // numDetailDeptId
            // 
            this.numDetailDeptId.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numDetailDeptId.Location = new System.Drawing.Point(306, 45);
            this.numDetailDeptId.Name = "numDetailDeptId";
            this.numDetailDeptId.Properties.MaxValue = new decimal(new int[] {
            -727379969,
            232,
            0,
            0});
            this.numDetailDeptId.Properties.ReadOnly = true;
            this.numDetailDeptId.Size = new System.Drawing.Size(53, 20);
            this.numDetailDeptId.TabIndex = 3;
            // 
            // lblDetailDeptNm
            // 
            this.lblDetailDeptNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailDeptNm.Appearance.Options.UseFont = true;
            this.lblDetailDeptNm.Location = new System.Drawing.Point(43, 48);
            this.lblDetailDeptNm.Name = "lblDetailDeptNm";
            this.lblDetailDeptNm.Size = new System.Drawing.Size(36, 15);
            this.lblDetailDeptNm.TabIndex = 4;
            this.lblDetailDeptNm.Text = "부서명";
            // 
            // txtDetailDeptNm
            // 
            this.txtDetailDeptNm.Location = new System.Drawing.Point(84, 45);
            this.txtDetailDeptNm.Name = "txtDetailDeptNm";
            this.txtDetailDeptNm.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtDetailDeptNm.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDetailDeptNm.Properties.Appearance.Options.UseBackColor = true;
            this.txtDetailDeptNm.Properties.Appearance.Options.UseForeColor = true;
            this.txtDetailDeptNm.Required = true;
            this.txtDetailDeptNm.Size = new System.Drawing.Size(220, 20);
            this.txtDetailDeptNm.TabIndex = 5;
            // 
            // numDetailParDeptId
            // 
            this.numDetailParDeptId.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numDetailParDeptId.Location = new System.Drawing.Point(306, 74);
            this.numDetailParDeptId.Name = "numDetailParDeptId";
            this.numDetailParDeptId.Properties.MaxValue = new decimal(new int[] {
            -727379969,
            232,
            0,
            0});
            this.numDetailParDeptId.Properties.ReadOnly = true;
            this.numDetailParDeptId.Size = new System.Drawing.Size(53, 20);
            this.numDetailParDeptId.TabIndex = 7;
            // 
            // lblDetailParDeptNm
            // 
            this.lblDetailParDeptNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailParDeptNm.Appearance.Options.UseFont = true;
            this.lblDetailParDeptNm.Location = new System.Drawing.Point(31, 85);
            this.lblDetailParDeptNm.Name = "lblDetailParDeptNm";
            this.lblDetailParDeptNm.Size = new System.Drawing.Size(48, 15);
            this.lblDetailParDeptNm.TabIndex = 8;
            this.lblDetailParDeptNm.Text = "상위부서";
            // 
            // popDetailParDeptNm
            // 
            this.popDetailParDeptNm.Location = new System.Drawing.Point(84, 74);
            this.popDetailParDeptNm.LookupKey = "P_DEPT";
            this.popDetailParDeptNm.MatchField = "dept_nm";
            this.popDetailParDeptNm.Name = "popDetailParDeptNm";
            this.popDetailParDeptNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", 24, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.popDetailParDeptNm.Size = new System.Drawing.Size(220, 20);
            this.popDetailParDeptNm.TabIndex = 9;
            this.popDetailParDeptNm.ToolTip = null;
            // 
            // lblDetailDeptType
            // 
            this.lblDetailDeptType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailDeptType.Appearance.Options.UseFont = true;
            this.lblDetailDeptType.Location = new System.Drawing.Point(31, 108);
            this.lblDetailDeptType.Name = "lblDetailDeptType";
            this.lblDetailDeptType.Size = new System.Drawing.Size(48, 15);
            this.lblDetailDeptType.TabIndex = 10;
            this.lblDetailDeptType.Text = "부서구분";
            // 
            // cboDetailDeptType
            // 
            this.cboDetailDeptType.EditValue = "";
            this.cboDetailDeptType.Location = new System.Drawing.Point(84, 105);
            this.cboDetailDeptType.LookupKey = "L_BA0003";
            this.cboDetailDeptType.Name = "cboDetailDeptType";
            this.cboDetailDeptType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailDeptType.Properties.NullText = "";
            this.cboDetailDeptType.Size = new System.Drawing.Size(220, 20);
            this.cboDetailDeptType.TabIndex = 11;
            // 
            // lblDetailRemark
            // 
            this.lblDetailRemark.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailRemark.Appearance.Options.UseFont = true;
            this.lblDetailRemark.Location = new System.Drawing.Point(55, 139);
            this.lblDetailRemark.Name = "lblDetailRemark";
            this.lblDetailRemark.Size = new System.Drawing.Size(24, 15);
            this.lblDetailRemark.TabIndex = 12;
            this.lblDetailRemark.Text = "비고";
            // 
            // panelWyn6
            // 
            this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
            this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn6.Location = new System.Drawing.Point(0, 0);
            this.panelWyn6.Name = "panelWyn6";
            this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn6.Size = new System.Drawing.Size(820, 27);
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
            this.sectionHeaderWyn3.Size = new System.Drawing.Size(815, 25);
            this.sectionHeaderWyn3.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn3.SvgIcon")));
            this.sectionHeaderWyn3.TabIndex = 9;
            this.sectionHeaderWyn3.Text = "부서정보 등록";
            // 
            // splitterWyn1
            // 
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Location = new System.Drawing.Point(402, 0);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(10, 501);
            this.splitterWyn1.TabIndex = 9;
            this.splitterWyn1.TabStop = false;
            // 
            // panelWyn8
            // 
            this.panelWyn8.Controls.Add(this.tree1);
            this.panelWyn8.Controls.Add(this.panelWyn2);
            this.panelWyn8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelWyn8.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn8.Location = new System.Drawing.Point(0, 0);
            this.panelWyn8.Name = "panelWyn8";
            this.panelWyn8.Size = new System.Drawing.Size(402, 501);
            this.panelWyn8.TabIndex = 12;
            // 
            // tree1
            // 
            this.tree1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeColAccId,
            this.treeColDeptId,
            this.treeColDeptNm,
            this.treeColParDeptId,
            this.treeColParDeptNm,
            this.treeColDeptType,
            this.treeColRemark});
            this.tree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree1.Location = new System.Drawing.Point(0, 27);
            this.tree1.Name = "tree1";
            this.tree1.OptionsBehavior.Editable = false;
            this.tree1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.spinEditTree,
            this.lookUpTreeAccId,
            this.lookUpTreeDeptType});
            this.tree1.RowHeight = 26;
            this.tree1.Size = new System.Drawing.Size(402, 474);
            this.tree1.TabIndex = 10;
            // 
            // treeColAccId
            // 
            this.treeColAccId.Caption = "사업장";
            this.treeColAccId.ColumnEdit = this.lookUpTreeAccId;
            this.treeColAccId.FieldName = "acc_id";
            this.treeColAccId.Name = "treeColAccId";
            this.treeColAccId.Width = 150;
            // 
            // lookUpTreeAccId
            // 
            this.lookUpTreeAccId.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpTreeAccId.LookupKey = "L_ACC";
            this.lookUpTreeAccId.Name = "lookUpTreeAccId";
            this.lookUpTreeAccId.NullText = "";
            this.lookUpTreeAccId.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // treeColDeptId
            // 
            this.treeColDeptId.Caption = "부서ID";
            this.treeColDeptId.ColumnEdit = this.spinEditTree;
            this.treeColDeptId.FieldName = "dept_id";
            this.treeColDeptId.Name = "treeColDeptId";
            this.treeColDeptId.Visible = true;
            this.treeColDeptId.VisibleIndex = 1;
            this.treeColDeptId.Width = 104;
            // 
            // spinEditTree
            // 
            this.spinEditTree.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditTree.Name = "spinEditTree";
            // 
            // treeColDeptNm
            // 
            this.treeColDeptNm.Caption = "부서";
            this.treeColDeptNm.FieldName = "dept_nm";
            this.treeColDeptNm.Name = "treeColDeptNm";
            this.treeColDeptNm.Visible = true;
            this.treeColDeptNm.VisibleIndex = 0;
            this.treeColDeptNm.Width = 273;
            // 
            // treeColParDeptId
            // 
            this.treeColParDeptId.Caption = "상위부서ID";
            this.treeColParDeptId.ColumnEdit = this.spinEditTree;
            this.treeColParDeptId.FieldName = "par_dept_id";
            this.treeColParDeptId.Name = "treeColParDeptId";
            this.treeColParDeptId.Width = 42;
            // 
            // treeColParDeptNm
            // 
            this.treeColParDeptNm.Caption = "상위부서";
            this.treeColParDeptNm.FieldName = "par_dept_nm";
            this.treeColParDeptNm.Name = "treeColParDeptNm";
            this.treeColParDeptNm.Width = 42;
            // 
            // treeColDeptType
            // 
            this.treeColDeptType.Caption = "부서구분";
            this.treeColDeptType.ColumnEdit = this.lookUpTreeDeptType;
            this.treeColDeptType.FieldName = "dept_type";
            this.treeColDeptType.Name = "treeColDeptType";
            this.treeColDeptType.Width = 133;
            // 
            // lookUpTreeDeptType
            // 
            this.lookUpTreeDeptType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpTreeDeptType.LookupKey = "L_BA0003";
            this.lookUpTreeDeptType.Name = "lookUpTreeDeptType";
            this.lookUpTreeDeptType.NullText = "";
            this.lookUpTreeDeptType.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // treeColRemark
            // 
            this.treeColRemark.Caption = "비고";
            this.treeColRemark.FieldName = "remark";
            this.treeColRemark.Name = "treeColRemark";
            this.treeColRemark.Width = 20;
            // 
            // panelWyn2
            // 
            this.panelWyn2.Controls.Add(this.sectionHeaderWyn4);
            this.panelWyn2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn2.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn2.Location = new System.Drawing.Point(0, 0);
            this.panelWyn2.Name = "panelWyn2";
            this.panelWyn2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn2.Size = new System.Drawing.Size(402, 27);
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
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(397, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 9;
            this.sectionHeaderWyn4.Text = "목록";
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.labelControl1);
            this.panHeader.Controls.Add(this.txtDeptId);
            this.panHeader.Controls.Add(this.lblSearchDeptNm);
            this.panHeader.Controls.Add(this.txtDeptNm);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 25);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1235, 49);
            this.panHeader.TabIndex = 8;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(850, 18);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 15);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "검색조건";
            this.labelControl1.Visible = false;
            // 
            // txtDeptId
            // 
            this.txtDeptId.Location = new System.Drawing.Point(921, 15);
            this.txtDeptId.Name = "txtDeptId";
            this.txtDeptId.Size = new System.Drawing.Size(150, 20);
            this.txtDeptId.TabIndex = 2;
            this.txtDeptId.Visible = false;
            // 
            // lblSearchDeptNm
            // 
            this.lblSearchDeptNm.Location = new System.Drawing.Point(20, 18);
            this.lblSearchDeptNm.Name = "lblSearchDeptNm";
            this.lblSearchDeptNm.Size = new System.Drawing.Size(20, 14);
            this.lblSearchDeptNm.TabIndex = 3;
            this.lblSearchDeptNm.Text = "부서";
            // 
            // txtDeptNm
            // 
            this.txtDeptNm.Location = new System.Drawing.Point(46, 15);
            this.txtDeptNm.Name = "txtDeptNm";
            this.txtDeptNm.Size = new System.Drawing.Size(150, 20);
            this.txtDeptNm.TabIndex = 4;
            // 
            // paTitleH
            // 
            this.paTitleH.Controls.Add(this.sectionHeaderWyn1);
            this.paTitleH.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitleH.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitleH.Location = new System.Drawing.Point(5, 0);
            this.paTitleH.Name = "paTitleH";
            this.paTitleH.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.paTitleH.Size = new System.Drawing.Size(1235, 25);
            this.paTitleH.TabIndex = 9;
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
            this.sectionHeaderWyn1.TabIndex = 14;
            this.sectionHeaderWyn1.Text = "부서등록 [frmDept]";
            // 
            // frmDept
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1245, 580);
            this.Controls.Add(this.panBase);
            this.Name = "frmDept";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabDetailGrids)).EndInit();
            this.tabDetailGrids.ResumeLayout(false);
            this.tabDetail1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
            this.panelWyn7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
            this.panData.ResumeLayout(false);
            this.panData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailAccId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDetailDeptId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailDeptNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDetailParDeptId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popDetailParDeptNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailDeptType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
            this.panelWyn6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).EndInit();
            this.panelWyn8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tree1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpTreeAccId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditTree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpTreeDeptType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
            this.panelWyn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).EndInit();
            this.paTitleH.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private PanelWyn panBase;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn4;
    private TabControlWyn tabDetailGrids;
    private DevExpress.XtraTab.XtraTabPage tabDetail1;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private DevExpress.XtraGrid.Columns.GridColumn colD1EmpNo;
    private DevExpress.XtraGrid.Columns.GridColumn colD1EmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn colD1EmpNmEng;
    private DevExpress.XtraGrid.Columns.GridColumn colD1JobGrade;
    private DevExpress.XtraGrid.Columns.GridColumn colD1JobType;
    private DevExpress.XtraGrid.Columns.GridColumn colD1Tel;
    private DevExpress.XtraGrid.Columns.GridColumn colD1HpTel;
    private DevExpress.XtraGrid.Columns.GridColumn colD1Email;
    private PanelWyn panelWyn1;
    private PanelWyn panelWyn7;
    private ButtonWyn btnDeletRow2;
    private ButtonWyn btnAddRow2;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblDetailAccId;
    private LookUpEditWyn cboDetailAccId;
    private SpinEditWyn numDetailDeptId;
    private DevExpress.XtraEditors.LabelControl lblDetailDeptNm;
    private TextEditWyn txtDetailDeptNm;
    private SpinEditWyn numDetailParDeptId;
    private DevExpress.XtraEditors.LabelControl lblDetailParDeptNm;
    private PopupLookupEditWyn popDetailParDeptNm;
    private DevExpress.XtraEditors.LabelControl lblDetailDeptType;
    private LookUpEditWyn cboDetailDeptType;
    private DevExpress.XtraEditors.LabelControl lblDetailRemark;
    private PanelWyn panelWyn6;
    private SplitterWyn splitterWyn1;
    private PanelWyn panelWyn8;
    private TreeListWyn tree1;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeColAccId;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeColDeptId;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeColDeptNm;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeColParDeptId;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeColParDeptNm;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeColDeptType;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeColRemark;
    private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spinEditTree;
    private LookUpColumnEdit lookUpTreeAccId;
    private LookUpColumnEdit lookUpTreeDeptType;
    private PanelWyn panelWyn2;
    private PanelWyn panHeader;
    private TextEditWyn txtDeptId;
    private DevExpress.XtraEditors.LabelControl lblSearchDeptNm;
    private TextEditWyn txtDeptNm;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private SectionHeaderWyn sectionHeaderWyn2;
    private SectionHeaderWyn sectionHeaderWyn3;
    private SectionHeaderWyn sectionHeaderWyn4;
    private MemoEditWyn txtDetailRemark;
    private PanelWyn paTitleH;
    private SectionHeaderWyn sectionHeaderWyn1;
}
