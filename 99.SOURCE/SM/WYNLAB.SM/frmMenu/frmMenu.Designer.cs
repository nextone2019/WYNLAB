#nullable disable
using WYNLAB.Base.Controls;

namespace WYNLAB.SM.MENU;

public partial class frmMenu
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }
    private TreeListWyn menuTree;
    private DevExpress.XtraTreeList.Columns.TreeListColumn colMenuId;
    private DevExpress.XtraTreeList.Columns.TreeListColumn colMenuNm;
    private ButtonWyn btnNewTop;
    private ButtonWyn btnNewChild;

    private PanelWyn panData;

    private GroupBoxWyn grpBasic;
    private TextEditWyn txtMenuId;
    private DevExpress.XtraEditors.LabelControl lblMenuNm;
    private TextEditWyn txtMenuNm;
    private TextEditWyn txtUpperMenuId;
    private DevExpress.XtraEditors.LabelControl lblUpperMenuNm;
    private PopupLookupEditWyn txtUpperMenuNm;
    private DevExpress.XtraEditors.LabelControl lblMenuLevel;
    private DevExpress.XtraEditors.LabelControl lblMenuType;

    private GroupBoxWyn grpConn;
    private DevExpress.XtraEditors.LabelControl lblModule;
    private DevExpress.XtraEditors.LabelControl lblScreenClassNm;
    private TextEditWyn txtScreenClassNm;
    private DevExpress.XtraEditors.LabelControl lblIconNm;
    private DevExpress.XtraEditors.ImageComboBoxEdit cboIconNm;
    private DevExpress.XtraEditors.LabelControl lblProcPrefix;
    private TextEditWyn txtProcPrefix;
    private DevExpress.XtraEditors.LabelControl lblSortOrder;
    private SpinEditWyn spnSortOrder;
    private CheckBoxWyn chkUseYn;

    private GroupBoxWyn grpAuth;
    private GroupBoxWyn grpFeature;
    private CheckBoxWyn chkFeatApproval;
    private DevExpress.XtraEditors.LabelControl lblApprDocType;
    private LookUpEditWyn cboApprDocType;
    private CheckBoxWyn chkFeatFile;
    private DevExpress.XtraEditors.LabelControl lblFileDocType;
    private TextEditWyn txtFileDocType;
    private DevExpress.XtraEditors.LabelControl lblAuthNm1;
    private TextEditWyn txtAuthNm1;
    private DevExpress.XtraEditors.LabelControl lblAuthNm2;
    private TextEditWyn txtAuthNm2;
    private DevExpress.XtraEditors.LabelControl lblAuthNm3;
    private TextEditWyn txtAuthNm3;
    private DevExpress.XtraEditors.LabelControl lblAuthNm4;
    private TextEditWyn txtAuthNm4;
    private DevExpress.XtraEditors.LabelControl lblAuthNm5;
    private TextEditWyn txtAuthNm5;
    private DevExpress.XtraEditors.LabelControl lblAuthNm6;
    private TextEditWyn txtAuthNm6;
    private DevExpress.XtraEditors.LabelControl lblAuthNm7;
    private TextEditWyn txtAuthNm7;
    private DevExpress.XtraEditors.LabelControl lblAuthNm8;
    private TextEditWyn txtAuthNm8;
    private DevExpress.XtraEditors.LabelControl lblAuthNm9;
    private TextEditWyn txtAuthNm9;
    private DevExpress.XtraEditors.LabelControl lblAuthNm10;
    private TextEditWyn txtAuthNm10;

    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMenu));
            this.menuTree = new WYNLAB.Base.Controls.TreeListWyn();
            this.colMenuId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colMenuNm = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.lblFormHint = new WYNLAB.Base.Controls.LabelWyn();
            this.lblFormTitle = new WYNLAB.Base.Controls.LabelWyn();
            this.grpBasic = new WYNLAB.Base.Controls.GroupBoxWyn();
            this.chkUseYn = new WYNLAB.Base.Controls.CheckBoxWyn();
            this.cboMenuType = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.txtMenuId = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblMenuNm = new DevExpress.XtraEditors.LabelControl();
            this.txtMenuNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtUpperMenuId = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblUpperMenuNm = new DevExpress.XtraEditors.LabelControl();
            this.txtUpperMenuNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.lblMenuLevel = new DevExpress.XtraEditors.LabelControl();
            this.spnMenuLevel = new WYNLAB.Base.Controls.SpinEditWyn();
            this.lblMenuType = new DevExpress.XtraEditors.LabelControl();
            this.grpConn = new WYNLAB.Base.Controls.GroupBoxWyn();
            this.cboModule = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblModule = new DevExpress.XtraEditors.LabelControl();
            this.lblScreenClassNm = new DevExpress.XtraEditors.LabelControl();
            this.txtScreenClassNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblIconNm = new DevExpress.XtraEditors.LabelControl();
            this.cboIconNm = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.lblProcPrefix = new DevExpress.XtraEditors.LabelControl();
            this.txtProcPrefix = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSortOrder = new DevExpress.XtraEditors.LabelControl();
            this.spnSortOrder = new WYNLAB.Base.Controls.SpinEditWyn();
            this.grpAuth = new WYNLAB.Base.Controls.GroupBoxWyn();
            this.grpFeature = new WYNLAB.Base.Controls.GroupBoxWyn();
            this.chkFeatApproval = new WYNLAB.Base.Controls.CheckBoxWyn();
            this.lblApprDocType = new DevExpress.XtraEditors.LabelControl();
            this.cboApprDocType = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.chkFeatFile = new WYNLAB.Base.Controls.CheckBoxWyn();
            this.lblFileDocType = new DevExpress.XtraEditors.LabelControl();
            this.txtFileDocType = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblAuthNm1 = new DevExpress.XtraEditors.LabelControl();
            this.txtAuthNm1 = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblAuthNm2 = new DevExpress.XtraEditors.LabelControl();
            this.txtAuthNm2 = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblAuthNm3 = new DevExpress.XtraEditors.LabelControl();
            this.txtAuthNm3 = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblAuthNm4 = new DevExpress.XtraEditors.LabelControl();
            this.txtAuthNm4 = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblAuthNm5 = new DevExpress.XtraEditors.LabelControl();
            this.txtAuthNm5 = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblAuthNm6 = new DevExpress.XtraEditors.LabelControl();
            this.txtAuthNm6 = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblAuthNm7 = new DevExpress.XtraEditors.LabelControl();
            this.txtAuthNm7 = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblAuthNm8 = new DevExpress.XtraEditors.LabelControl();
            this.txtAuthNm8 = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblAuthNm9 = new DevExpress.XtraEditors.LabelControl();
            this.txtAuthNm9 = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblAuthNm10 = new DevExpress.XtraEditors.LabelControl();
            this.txtAuthNm10 = new WYNLAB.Base.Controls.TextEditWyn();
            this.btnNewTop = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnNewChild = new WYNLAB.Base.Controls.ButtonWyn();
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnSaveInline = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnCancelEdit = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnCopy = new WYNLAB.Base.Controls.ButtonWyn();
            this.svgImageCollection1 = new DevExpress.Utils.SvgImageCollection(this.components);
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            ((System.ComponentModel.ISupportInitialize)(this.menuTree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
            this.grpBasic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkUseYn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboMenuType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMenuId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMenuNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpperMenuId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpperMenuNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnMenuLevel.Properties)).BeginInit();
            this.grpConn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboModule.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtScreenClassNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboIconNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProcPrefix.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnSortOrder.Properties)).BeginInit();
            this.grpAuth.SuspendLayout();
            this.grpFeature.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkFeatApproval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboApprDocType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFeatFile.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFileDocType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthNm1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthNm2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthNm3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthNm4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthNm5.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthNm6.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthNm7.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthNm8.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthNm9.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthNm10.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).BeginInit();
            this.paTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
            this.panelWyn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
            this.panelWyn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
            this.panelWyn4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuTree
            // 
            this.menuTree.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colMenuId,
            this.colMenuNm});
            this.menuTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuTree.Location = new System.Drawing.Point(0, 0);
            this.menuTree.Name = "menuTree";
            this.menuTree.OptionsView.ShowColumns = false;
            this.menuTree.RowHeight = 26;
            this.menuTree.Size = new System.Drawing.Size(531, 565);
            this.menuTree.TabIndex = 0;
            // 
            // colMenuId
            // 
            this.colMenuId.Caption = "硫붾돱ID";
            this.colMenuId.FieldName = "MenuId";
            this.colMenuId.Name = "colMenuId";
            // 
            // colMenuNm
            // 
            this.colMenuNm.Caption = "메뉴명";
            this.colMenuNm.FieldName = "MenuNm";
            this.colMenuNm.Name = "colMenuNm";
            this.colMenuNm.Visible = true;
            this.colMenuNm.VisibleIndex = 0;
            // 
            // panData
            // 
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Controls.Add(this.lblFormHint);
            this.panData.Controls.Add(this.lblFormTitle);
            this.panData.Controls.Add(this.grpBasic);
            this.panData.Controls.Add(this.grpConn);
            this.panData.Controls.Add(this.grpAuth);
            this.panData.Controls.Add(this.grpFeature);
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 35);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(616, 530);
            this.panData.TabIndex = 1;
            // 
            // lblFormHint
            // 
            this.lblFormHint.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblFormHint.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.lblFormHint.Appearance.Options.UseFont = true;
            this.lblFormHint.Appearance.Options.UseForeColor = true;
            this.lblFormHint.Location = new System.Drawing.Point(19, 28);
            this.lblFormHint.Name = "lblFormHint";
            this.lblFormHint.Size = new System.Drawing.Size(72, 15);
            this.lblFormHint.TabIndex = 4;
            this.lblFormHint.Text = "                  ";
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblFormTitle.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.lblFormTitle.Appearance.Options.UseFont = true;
            this.lblFormTitle.Appearance.Options.UseForeColor = true;
            this.lblFormTitle.Location = new System.Drawing.Point(7, 7);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(84, 15);
            this.lblFormTitle.TabIndex = 4;
            this.lblFormTitle.Text = "                     ";
            // 
            // grpBasic
            // 
            this.grpBasic.Controls.Add(this.chkUseYn);
            this.grpBasic.Controls.Add(this.cboMenuType);
            this.grpBasic.Controls.Add(this.txtMenuId);
            this.grpBasic.Controls.Add(this.lblMenuNm);
            this.grpBasic.Controls.Add(this.txtMenuNm);
            this.grpBasic.Controls.Add(this.txtUpperMenuId);
            this.grpBasic.Controls.Add(this.lblUpperMenuNm);
            this.grpBasic.Controls.Add(this.txtUpperMenuNm);
            this.grpBasic.Controls.Add(this.lblMenuLevel);
            this.grpBasic.Controls.Add(this.spnMenuLevel);
            this.grpBasic.Controls.Add(this.lblMenuType);
            this.grpBasic.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.grpBasic.Location = new System.Drawing.Point(15, 53);
            this.grpBasic.Name = "grpBasic";
            this.grpBasic.Size = new System.Drawing.Size(540, 117);
            this.grpBasic.TabIndex = 0;
            this.grpBasic.TabStop = false;
            this.grpBasic.Text = "기본정보";
            // 
            // chkUseYn
            // 
            this.chkUseYn.Location = new System.Drawing.Point(101, 80);
            this.chkUseYn.Name = "chkUseYn";
            this.chkUseYn.Properties.Caption = "사용";
            this.chkUseYn.Size = new System.Drawing.Size(47, 20);
            this.chkUseYn.TabIndex = 0;
            // 
            // cboMenuType
            // 
            this.cboMenuType.EditValue = "";
            this.cboMenuType.Location = new System.Drawing.Point(241, 80);
            this.cboMenuType.LookupKey = "L_SM0008";
            this.cboMenuType.Name = "cboMenuType";
            this.cboMenuType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboMenuType.Properties.NullText = "";
            this.cboMenuType.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cboMenuType.Size = new System.Drawing.Size(159, 20);
            this.cboMenuType.TabIndex = 11;
            // 
            // txtMenuId
            // 
            this.txtMenuId.Location = new System.Drawing.Point(322, 24);
            this.txtMenuId.Name = "txtMenuId";
            this.txtMenuId.Properties.ReadOnly = true;
            this.txtMenuId.Size = new System.Drawing.Size(78, 20);
            this.txtMenuId.TabIndex = 1;
            // 
            // lblMenuNm
            // 
            this.lblMenuNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenuNm.Appearance.Options.UseFont = true;
            this.lblMenuNm.Location = new System.Drawing.Point(60, 27);
            this.lblMenuNm.Name = "lblMenuNm";
            this.lblMenuNm.Size = new System.Drawing.Size(36, 15);
            this.lblMenuNm.TabIndex = 2;
            this.lblMenuNm.Text = "메뉴명";
            // 
            // txtMenuNm
            // 
            this.txtMenuNm.Location = new System.Drawing.Point(100, 24);
            this.txtMenuNm.Name = "txtMenuNm";
            this.txtMenuNm.Size = new System.Drawing.Size(220, 20);
            this.txtMenuNm.TabIndex = 3;
            // 
            // txtUpperMenuId
            // 
            this.txtUpperMenuId.Location = new System.Drawing.Point(322, 52);
            this.txtUpperMenuId.Name = "txtUpperMenuId";
            this.txtUpperMenuId.Properties.ReadOnly = true;
            this.txtUpperMenuId.Size = new System.Drawing.Size(78, 20);
            this.txtUpperMenuId.TabIndex = 5;
            // 
            // lblUpperMenuNm
            // 
            this.lblUpperMenuNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpperMenuNm.Appearance.Options.UseFont = true;
            this.lblUpperMenuNm.Location = new System.Drawing.Point(36, 55);
            this.lblUpperMenuNm.Name = "lblUpperMenuNm";
            this.lblUpperMenuNm.Size = new System.Drawing.Size(60, 15);
            this.lblUpperMenuNm.TabIndex = 6;
            this.lblUpperMenuNm.Text = "상위메뉴명";
            // 
            // txtUpperMenuNm
            // 
            this.txtUpperMenuNm.Location = new System.Drawing.Point(100, 52);
            this.txtUpperMenuNm.Name = "txtUpperMenuNm";
            this.txtUpperMenuNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtUpperMenuNm.Size = new System.Drawing.Size(220, 20);
            this.txtUpperMenuNm.TabIndex = 7;
            this.txtUpperMenuNm.ToolTip = null;
            // 
            // lblMenuLevel
            // 
            this.lblMenuLevel.Location = new System.Drawing.Point(355, 107);
            this.lblMenuLevel.Name = "lblMenuLevel";
            this.lblMenuLevel.Size = new System.Drawing.Size(40, 14);
            this.lblMenuLevel.TabIndex = 8;
            this.lblMenuLevel.Text = "메뉴레벨";
            this.lblMenuLevel.Visible = false;
            // 
            // spnMenuLevel
            // 
            this.spnMenuLevel.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spnMenuLevel.Location = new System.Drawing.Point(399, 104);
            this.spnMenuLevel.Name = "spnMenuLevel";
            this.spnMenuLevel.Properties.MaxValue = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.spnMenuLevel.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spnMenuLevel.Properties.ReadOnly = true;
            this.spnMenuLevel.Size = new System.Drawing.Size(70, 20);
            this.spnMenuLevel.TabIndex = 9;
            this.spnMenuLevel.Visible = false;
            // 
            // lblMenuType
            // 
            this.lblMenuType.Location = new System.Drawing.Point(190, 83);
            this.lblMenuType.Name = "lblMenuType";
            this.lblMenuType.Size = new System.Drawing.Size(40, 14);
            this.lblMenuType.TabIndex = 10;
            this.lblMenuType.Text = "메뉴유형";
            // 
            // grpConn
            // 
            this.grpConn.Controls.Add(this.cboModule);
            this.grpConn.Controls.Add(this.lblModule);
            this.grpConn.Controls.Add(this.lblScreenClassNm);
            this.grpConn.Controls.Add(this.txtScreenClassNm);
            this.grpConn.Controls.Add(this.lblIconNm);
            this.grpConn.Controls.Add(this.cboIconNm);
            this.grpConn.Controls.Add(this.lblProcPrefix);
            this.grpConn.Controls.Add(this.txtProcPrefix);
            this.grpConn.Controls.Add(this.lblSortOrder);
            this.grpConn.Controls.Add(this.spnSortOrder);
            this.grpConn.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.grpConn.Location = new System.Drawing.Point(15, 182);
            this.grpConn.Name = "grpConn";
            this.grpConn.Size = new System.Drawing.Size(540, 109);
            this.grpConn.TabIndex = 1;
            this.grpConn.TabStop = false;
            this.grpConn.Text = "연결정보";
            // 
            // cboModule
            // 
            this.cboModule.EditValue = "";
            this.cboModule.Location = new System.Drawing.Point(100, 19);
            this.cboModule.LookupKey = "L_SM0003";
            this.cboModule.Name = "cboModule";
            this.cboModule.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboModule.Properties.NullText = "";
            this.cboModule.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cboModule.Size = new System.Drawing.Size(220, 20);
            this.cboModule.TabIndex = 10;
            // 
            // lblModule
            // 
            this.lblModule.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModule.Appearance.Options.UseFont = true;
            this.lblModule.Location = new System.Drawing.Point(55, 25);
            this.lblModule.Name = "lblModule";
            this.lblModule.Size = new System.Drawing.Size(41, 15);
            this.lblModule.TabIndex = 0;
            this.lblModule.Text = "Module";
            // 
            // lblScreenClassNm
            // 
            this.lblScreenClassNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreenClassNm.Appearance.Options.UseFont = true;
            this.lblScreenClassNm.Location = new System.Drawing.Point(38, 51);
            this.lblScreenClassNm.Name = "lblScreenClassNm";
            this.lblScreenClassNm.Size = new System.Drawing.Size(58, 15);
            this.lblScreenClassNm.TabIndex = 2;
            this.lblScreenClassNm.Text = "formName";
            // 
            // txtScreenClassNm
            // 
            this.txtScreenClassNm.Location = new System.Drawing.Point(100, 48);
            this.txtScreenClassNm.Name = "txtScreenClassNm";
            this.txtScreenClassNm.Size = new System.Drawing.Size(220, 20);
            this.txtScreenClassNm.TabIndex = 3;
            // 
            // lblIconNm
            // 
            this.lblIconNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIconNm.Appearance.Options.UseFont = true;
            this.lblIconNm.Location = new System.Drawing.Point(335, 51);
            this.lblIconNm.Name = "lblIconNm";
            this.lblIconNm.Size = new System.Drawing.Size(48, 15);
            this.lblIconNm.TabIndex = 4;
            this.lblIconNm.Text = "아이콘명";
            // 
            // cboIconNm
            // 
            this.cboIconNm.Location = new System.Drawing.Point(387, 48);
            this.cboIconNm.Name = "cboIconNm";
            this.cboIconNm.Size = new System.Drawing.Size(91, 20);
            this.cboIconNm.TabIndex = 5;
            // 
            // lblProcPrefix
            // 
            this.lblProcPrefix.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcPrefix.Appearance.Options.UseFont = true;
            this.lblProcPrefix.Location = new System.Drawing.Point(6, 82);
            this.lblProcPrefix.Name = "lblProcPrefix";
            this.lblProcPrefix.Size = new System.Drawing.Size(90, 15);
            this.lblProcPrefix.TabIndex = 6;
            this.lblProcPrefix.Text = "Procedure접두사";
            // 
            // txtProcPrefix
            // 
            this.txtProcPrefix.Location = new System.Drawing.Point(100, 79);
            this.txtProcPrefix.Name = "txtProcPrefix";
            this.txtProcPrefix.Size = new System.Drawing.Size(220, 20);
            this.txtProcPrefix.TabIndex = 7;
            // 
            // lblSortOrder
            // 
            this.lblSortOrder.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSortOrder.Appearance.Options.UseFont = true;
            this.lblSortOrder.Location = new System.Drawing.Point(335, 82);
            this.lblSortOrder.Name = "lblSortOrder";
            this.lblSortOrder.Size = new System.Drawing.Size(48, 15);
            this.lblSortOrder.TabIndex = 8;
            this.lblSortOrder.Text = "정렬순서";
            // 
            // spnSortOrder
            // 
            this.spnSortOrder.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spnSortOrder.Location = new System.Drawing.Point(387, 79);
            this.spnSortOrder.Name = "spnSortOrder";
            this.spnSortOrder.Properties.MaxValue = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.spnSortOrder.Size = new System.Drawing.Size(91, 20);
            this.spnSortOrder.TabIndex = 9;
            // 
            // grpAuth
            // 
            this.grpAuth.Controls.Add(this.lblAuthNm1);
            this.grpAuth.Controls.Add(this.txtAuthNm1);
            this.grpAuth.Controls.Add(this.lblAuthNm2);
            this.grpAuth.Controls.Add(this.txtAuthNm2);
            this.grpAuth.Controls.Add(this.lblAuthNm3);
            this.grpAuth.Controls.Add(this.txtAuthNm3);
            this.grpAuth.Controls.Add(this.lblAuthNm4);
            this.grpAuth.Controls.Add(this.txtAuthNm4);
            this.grpAuth.Controls.Add(this.lblAuthNm5);
            this.grpAuth.Controls.Add(this.txtAuthNm5);
            this.grpAuth.Controls.Add(this.lblAuthNm6);
            this.grpAuth.Controls.Add(this.txtAuthNm6);
            this.grpAuth.Controls.Add(this.lblAuthNm7);
            this.grpAuth.Controls.Add(this.txtAuthNm7);
            this.grpAuth.Controls.Add(this.lblAuthNm8);
            this.grpAuth.Controls.Add(this.txtAuthNm8);
            this.grpAuth.Controls.Add(this.lblAuthNm9);
            this.grpAuth.Controls.Add(this.txtAuthNm9);
            this.grpAuth.Controls.Add(this.lblAuthNm10);
            this.grpAuth.Controls.Add(this.txtAuthNm10);
            this.grpAuth.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.grpAuth.Location = new System.Drawing.Point(15, 297);
            this.grpAuth.Name = "grpAuth";
            this.grpAuth.Size = new System.Drawing.Size(540, 165);
            this.grpAuth.TabIndex = 3;
            this.grpAuth.TabStop = false;
            this.grpAuth.Text = "추가권한 캡션(AUTH01~10)";
            // 
            // grpFeature (화면 기능 - 2026-10-03: 이 화면에 켤 공통 기능. 켜면 화면의 FeatureBarWyn에 해당 버튼이 나타난다)
            // 
            this.grpFeature.Controls.Add(this.chkFeatApproval);
            this.grpFeature.Controls.Add(this.lblApprDocType);
            this.grpFeature.Controls.Add(this.cboApprDocType);
            this.grpFeature.Controls.Add(this.chkFeatFile);
            this.grpFeature.Controls.Add(this.lblFileDocType);
            this.grpFeature.Controls.Add(this.txtFileDocType);
            this.grpFeature.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.grpFeature.Location = new System.Drawing.Point(15, 468);
            this.grpFeature.Name = "grpFeature";
            this.grpFeature.Size = new System.Drawing.Size(540, 52);
            this.grpFeature.TabIndex = 4;
            this.grpFeature.TabStop = false;
            this.grpFeature.Text = "화면 기능";
            // 
            // chkFeatApproval
            // 
            this.chkFeatApproval.Location = new System.Drawing.Point(12, 22);
            this.chkFeatApproval.Name = "chkFeatApproval";
            this.chkFeatApproval.Properties.Caption = "전자결재 사용";
            this.chkFeatApproval.Size = new System.Drawing.Size(106, 20);
            this.chkFeatApproval.TabIndex = 0;
            // 
            // lblApprDocType
            // 
            this.lblApprDocType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblApprDocType.Appearance.Options.UseFont = true;
            this.lblApprDocType.Location = new System.Drawing.Point(124, 25);
            this.lblApprDocType.Name = "lblApprDocType";
            this.lblApprDocType.Size = new System.Drawing.Size(48, 15);
            this.lblApprDocType.TabIndex = 1;
            this.lblApprDocType.Text = "문서유형";
            // 
            // cboApprDocType
            // 
            this.cboApprDocType.EditValue = "";
            this.cboApprDocType.Location = new System.Drawing.Point(178, 22);
            this.cboApprDocType.LookupKey = "L_AP0002";
            this.cboApprDocType.Name = "cboApprDocType";
            this.cboApprDocType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboApprDocType.Properties.NullText = "";
            this.cboApprDocType.Size = new System.Drawing.Size(100, 20);
            this.cboApprDocType.TabIndex = 2;
            // 
            // chkFeatFile
            // 
            this.chkFeatFile.Location = new System.Drawing.Point(298, 22);
            this.chkFeatFile.Name = "chkFeatFile";
            this.chkFeatFile.Properties.Caption = "첨부파일 사용";
            this.chkFeatFile.Size = new System.Drawing.Size(106, 20);
            this.chkFeatFile.TabIndex = 3;
            // 
            // lblFileDocType
            // 
            this.lblFileDocType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblFileDocType.Appearance.Options.UseFont = true;
            this.lblFileDocType.Location = new System.Drawing.Point(410, 25);
            this.lblFileDocType.Name = "lblFileDocType";
            this.lblFileDocType.Size = new System.Drawing.Size(48, 15);
            this.lblFileDocType.TabIndex = 4;
            this.lblFileDocType.Text = "첨부구분";
            // 
            // txtFileDocType
            // 
            this.txtFileDocType.Location = new System.Drawing.Point(464, 22);
            this.txtFileDocType.Name = "txtFileDocType";
            this.txtFileDocType.Size = new System.Drawing.Size(66, 20);
            this.txtFileDocType.TabIndex = 5;            // 
            // lblAuthNm1
            // 
            this.lblAuthNm1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthNm1.Appearance.Options.UseFont = true;
            this.lblAuthNm1.Location = new System.Drawing.Point(44, 25);
            this.lblAuthNm1.Name = "lblAuthNm1";
            this.lblAuthNm1.Size = new System.Drawing.Size(50, 15);
            this.lblAuthNm1.TabIndex = 0;
            this.lblAuthNm1.Text = "권한명01";
            // 
            // txtAuthNm1
            // 
            this.txtAuthNm1.Location = new System.Drawing.Point(100, 22);
            this.txtAuthNm1.Name = "txtAuthNm1";
            this.txtAuthNm1.Size = new System.Drawing.Size(145, 20);
            this.txtAuthNm1.TabIndex = 1;
            // 
            // lblAuthNm2
            // 
            this.lblAuthNm2.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthNm2.Appearance.Options.UseFont = true;
            this.lblAuthNm2.Location = new System.Drawing.Point(44, 51);
            this.lblAuthNm2.Name = "lblAuthNm2";
            this.lblAuthNm2.Size = new System.Drawing.Size(50, 15);
            this.lblAuthNm2.TabIndex = 2;
            this.lblAuthNm2.Text = "권한명02";
            // 
            // txtAuthNm2
            // 
            this.txtAuthNm2.Location = new System.Drawing.Point(100, 48);
            this.txtAuthNm2.Name = "txtAuthNm2";
            this.txtAuthNm2.Size = new System.Drawing.Size(145, 20);
            this.txtAuthNm2.TabIndex = 3;
            // 
            // lblAuthNm3
            // 
            this.lblAuthNm3.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthNm3.Appearance.Options.UseFont = true;
            this.lblAuthNm3.Location = new System.Drawing.Point(44, 77);
            this.lblAuthNm3.Name = "lblAuthNm3";
            this.lblAuthNm3.Size = new System.Drawing.Size(50, 15);
            this.lblAuthNm3.TabIndex = 4;
            this.lblAuthNm3.Text = "권한명03";
            // 
            // txtAuthNm3
            // 
            this.txtAuthNm3.Location = new System.Drawing.Point(100, 74);
            this.txtAuthNm3.Name = "txtAuthNm3";
            this.txtAuthNm3.Size = new System.Drawing.Size(145, 20);
            this.txtAuthNm3.TabIndex = 5;
            // 
            // lblAuthNm4
            // 
            this.lblAuthNm4.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthNm4.Appearance.Options.UseFont = true;
            this.lblAuthNm4.Location = new System.Drawing.Point(44, 103);
            this.lblAuthNm4.Name = "lblAuthNm4";
            this.lblAuthNm4.Size = new System.Drawing.Size(50, 15);
            this.lblAuthNm4.TabIndex = 6;
            this.lblAuthNm4.Text = "권한명04";
            // 
            // txtAuthNm4
            // 
            this.txtAuthNm4.Location = new System.Drawing.Point(100, 100);
            this.txtAuthNm4.Name = "txtAuthNm4";
            this.txtAuthNm4.Size = new System.Drawing.Size(145, 20);
            this.txtAuthNm4.TabIndex = 7;
            // 
            // lblAuthNm5
            // 
            this.lblAuthNm5.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthNm5.Appearance.Options.UseFont = true;
            this.lblAuthNm5.Location = new System.Drawing.Point(44, 129);
            this.lblAuthNm5.Name = "lblAuthNm5";
            this.lblAuthNm5.Size = new System.Drawing.Size(50, 15);
            this.lblAuthNm5.TabIndex = 8;
            this.lblAuthNm5.Text = "권한명05";
            // 
            // txtAuthNm5
            // 
            this.txtAuthNm5.Location = new System.Drawing.Point(100, 126);
            this.txtAuthNm5.Name = "txtAuthNm5";
            this.txtAuthNm5.Size = new System.Drawing.Size(145, 20);
            this.txtAuthNm5.TabIndex = 9;
            // 
            // lblAuthNm6
            // 
            this.lblAuthNm6.Location = new System.Drawing.Point(279, 25);
            this.lblAuthNm6.Name = "lblAuthNm6";
            this.lblAuthNm6.Size = new System.Drawing.Size(44, 14);
            this.lblAuthNm6.TabIndex = 10;
            this.lblAuthNm6.Text = "권한명06";
            // 
            // txtAuthNm6
            // 
            this.txtAuthNm6.Location = new System.Drawing.Point(333, 22);
            this.txtAuthNm6.Name = "txtAuthNm6";
            this.txtAuthNm6.Size = new System.Drawing.Size(145, 20);
            this.txtAuthNm6.TabIndex = 11;
            // 
            // lblAuthNm7
            // 
            this.lblAuthNm7.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthNm7.Appearance.Options.UseFont = true;
            this.lblAuthNm7.Location = new System.Drawing.Point(273, 51);
            this.lblAuthNm7.Name = "lblAuthNm7";
            this.lblAuthNm7.Size = new System.Drawing.Size(50, 15);
            this.lblAuthNm7.TabIndex = 12;
            this.lblAuthNm7.Text = "권한명07";
            // 
            // txtAuthNm7
            // 
            this.txtAuthNm7.Location = new System.Drawing.Point(333, 48);
            this.txtAuthNm7.Name = "txtAuthNm7";
            this.txtAuthNm7.Size = new System.Drawing.Size(145, 20);
            this.txtAuthNm7.TabIndex = 13;
            // 
            // lblAuthNm8
            // 
            this.lblAuthNm8.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthNm8.Appearance.Options.UseFont = true;
            this.lblAuthNm8.Location = new System.Drawing.Point(273, 77);
            this.lblAuthNm8.Name = "lblAuthNm8";
            this.lblAuthNm8.Size = new System.Drawing.Size(50, 15);
            this.lblAuthNm8.TabIndex = 14;
            this.lblAuthNm8.Text = "권한명08";
            // 
            // txtAuthNm8
            // 
            this.txtAuthNm8.Location = new System.Drawing.Point(333, 74);
            this.txtAuthNm8.Name = "txtAuthNm8";
            this.txtAuthNm8.Size = new System.Drawing.Size(145, 20);
            this.txtAuthNm8.TabIndex = 15;
            // 
            // lblAuthNm9
            // 
            this.lblAuthNm9.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthNm9.Appearance.Options.UseFont = true;
            this.lblAuthNm9.Location = new System.Drawing.Point(273, 103);
            this.lblAuthNm9.Name = "lblAuthNm9";
            this.lblAuthNm9.Size = new System.Drawing.Size(50, 15);
            this.lblAuthNm9.TabIndex = 16;
            this.lblAuthNm9.Text = "권한명09";
            // 
            // txtAuthNm9
            // 
            this.txtAuthNm9.Location = new System.Drawing.Point(333, 100);
            this.txtAuthNm9.Name = "txtAuthNm9";
            this.txtAuthNm9.Size = new System.Drawing.Size(145, 20);
            this.txtAuthNm9.TabIndex = 17;
            // 
            // lblAuthNm10
            // 
            this.lblAuthNm10.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthNm10.Appearance.Options.UseFont = true;
            this.lblAuthNm10.Location = new System.Drawing.Point(273, 129);
            this.lblAuthNm10.Name = "lblAuthNm10";
            this.lblAuthNm10.Size = new System.Drawing.Size(50, 15);
            this.lblAuthNm10.TabIndex = 18;
            this.lblAuthNm10.Text = "권한명10";
            // 
            // txtAuthNm10
            // 
            this.txtAuthNm10.Location = new System.Drawing.Point(333, 126);
            this.txtAuthNm10.Name = "txtAuthNm10";
            this.txtAuthNm10.Size = new System.Drawing.Size(145, 20);
            this.txtAuthNm10.TabIndex = 19;
            // 
            // btnNewTop
            // 
            this.btnNewTop.BackColor = System.Drawing.Color.Transparent;
            this.btnNewTop.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnNewTop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNewTop.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnNewTop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnNewTop.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnNewTop.Image = null;
            this.btnNewTop.Location = new System.Drawing.Point(113, 4);
            this.btnNewTop.Name = "btnNewTop";
            this.btnNewTop.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnNewTop.Size = new System.Drawing.Size(107, 24);
            this.btnNewTop.TabIndex = 2;
            this.btnNewTop.Text = "최상위메뉴추가";
            this.btnNewTop.ToolTip = null;
            // 
            // btnNewChild
            // 
            this.btnNewChild.BackColor = System.Drawing.Color.Transparent;
            this.btnNewChild.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnNewChild.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNewChild.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnNewChild.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnNewChild.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnNewChild.Image = null;
            this.btnNewChild.Location = new System.Drawing.Point(3, 4);
            this.btnNewChild.Name = "btnNewChild";
            this.btnNewChild.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnNewChild.Size = new System.Drawing.Size(107, 24);
            this.btnNewChild.TabIndex = 3;
            this.btnNewChild.Text = "하위메뉴추가";
            this.btnNewChild.ToolTip = null;
            // 
            // paTitle
            // 
            this.paTitle.Controls.Add(this.sectionHeaderWyn1);
            this.paTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle.Location = new System.Drawing.Point(5, 5);
            this.paTitle.Name = "paTitle";
            this.paTitle.Size = new System.Drawing.Size(1155, 25);
            this.paTitle.TabIndex = 6;
            // 
            // sectionHeaderWyn1
            // 
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(0, 0);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1155, 25);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 8;
            this.sectionHeaderWyn1.Text = "메뉴 등록 [frmMenu]";
            // 
            // panelWyn1
            // 
            this.panelWyn1.Controls.Add(this.panelWyn2);
            this.panelWyn1.Controls.Add(this.paTitle);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(0, 0);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Padding = new System.Windows.Forms.Padding(5);
            this.panelWyn1.Size = new System.Drawing.Size(1165, 600);
            this.panelWyn1.TabIndex = 7;
            // 
            // panelWyn2
            // 
            this.panelWyn2.Controls.Add(this.menuTree);
            this.panelWyn2.Controls.Add(this.splitterWyn1);
            this.panelWyn2.Controls.Add(this.panelWyn3);
            this.panelWyn2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn2.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn2.Location = new System.Drawing.Point(5, 30);
            this.panelWyn2.Name = "panelWyn2";
            this.panelWyn2.Size = new System.Drawing.Size(1155, 565);
            this.panelWyn2.TabIndex = 7;
            // 
            // panelWyn3
            // 
            this.panelWyn3.Controls.Add(this.panData);
            this.panelWyn3.Controls.Add(this.panelWyn4);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(539, 0);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Size = new System.Drawing.Size(616, 565);
            this.panelWyn3.TabIndex = 1;
            // 
            // panelWyn4
            // 
            this.panelWyn4.Controls.Add(this.btnSaveInline);
            this.panelWyn4.Controls.Add(this.btnNewChild);
            this.panelWyn4.Controls.Add(this.btnCancelEdit);
            this.panelWyn4.Controls.Add(this.btnNewTop);
            this.panelWyn4.Controls.Add(this.btnCopy);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(0, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn4.Size = new System.Drawing.Size(616, 35);
            this.panelWyn4.TabIndex = 9;
            // 
            // btnSaveInline
            // 
            this.btnSaveInline.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveInline.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(122)))), ((int)(((byte)(201)))));
            this.btnSaveInline.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveInline.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(122)))), ((int)(((byte)(201)))));
            this.btnSaveInline.ForeColor = System.Drawing.Color.White;
            this.btnSaveInline.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnSaveInline.Image = null;
            this.btnSaveInline.Location = new System.Drawing.Point(443, 4);
            this.btnSaveInline.Name = "btnSaveInline";
            this.btnSaveInline.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnSaveInline.Size = new System.Drawing.Size(107, 24);
            this.btnSaveInline.TabIndex = 11;
            this.btnSaveInline.Text = "저장";
            this.btnSaveInline.ToolTip = null;
            this.btnSaveInline.Visible = false;
            // 
            // btnCancelEdit
            // 
            this.btnCancelEdit.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelEdit.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnCancelEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelEdit.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnCancelEdit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnCancelEdit.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnCancelEdit.Image = null;
            this.btnCancelEdit.Location = new System.Drawing.Point(333, 4);
            this.btnCancelEdit.Name = "btnCancelEdit";
            this.btnCancelEdit.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnCancelEdit.Size = new System.Drawing.Size(107, 24);
            this.btnCancelEdit.TabIndex = 10;
            this.btnCancelEdit.Text = "취소";
            this.btnCancelEdit.ToolTip = null;
            // 
            // btnCopy
            // 
            this.btnCopy.BackColor = System.Drawing.Color.Transparent;
            this.btnCopy.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnCopy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCopy.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnCopy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnCopy.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnCopy.Image = null;
            this.btnCopy.Location = new System.Drawing.Point(223, 4);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnCopy.Size = new System.Drawing.Size(107, 24);
            this.btnCopy.TabIndex = 12;
            this.btnCopy.Text = "복사";
            this.btnCopy.ToolTip = null;
            // 
            // svgImageCollection1
            // 
            this.svgImageCollection1.Add("datapanel", "image://svgimages/outlook inspired/datapanel.svg");
            this.svgImageCollection1.Add("open", "image://svgimages/actions/open.svg");
            // 
            // splitterWyn1
            // 
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitterWyn1.Location = new System.Drawing.Point(531, 0);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(8, 565);
            this.splitterWyn1.TabIndex = 2;
            this.splitterWyn1.TabStop = false;
            // 
            // frmMenu
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1165, 600);
            this.Controls.Add(this.panelWyn1);
            this.Name = "frmMenu";
            ((System.ComponentModel.ISupportInitialize)(this.menuTree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
            this.panData.ResumeLayout(false);
            this.panData.PerformLayout();
            this.grpBasic.ResumeLayout(false);
            this.grpBasic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkUseYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboMenuType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMenuId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMenuNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpperMenuId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpperMenuNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnMenuLevel.Properties)).EndInit();
            this.grpConn.ResumeLayout(false);
            this.grpConn.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboModule.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtScreenClassNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboIconNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProcPrefix.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnSortOrder.Properties)).EndInit();
            this.grpFeature.ResumeLayout(false);
            this.grpFeature.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkFeatApproval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboApprDocType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFeatFile.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFileDocType.Properties)).EndInit();
            this.grpAuth.ResumeLayout(false);
            this.grpAuth.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthNm1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthNm2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthNm3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthNm4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthNm5.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthNm6.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthNm7.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthNm8.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthNm9.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthNm10.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
            this.paTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
            this.panelWyn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection1)).EndInit();
            this.ResumeLayout(false);

    }

    private PanelWyn paTitle;
    private SectionHeaderWyn sectionHeaderWyn1;
    private PanelWyn panelWyn1;
    private PanelWyn panelWyn2;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn4;
    private LookUpEditWyn cboModule;
    private ButtonWyn btnSaveInline;
    private ButtonWyn btnCancelEdit;
    private ButtonWyn btnCopy;
    private LookUpEditWyn cboMenuType;
    private SpinEditWyn spnMenuLevel;
    private LabelWyn lblFormTitle;
    private LabelWyn lblFormHint;
    private DevExpress.Utils.SvgImageCollection svgImageCollection1;
    private SplitterWyn splitterWyn1;
}
