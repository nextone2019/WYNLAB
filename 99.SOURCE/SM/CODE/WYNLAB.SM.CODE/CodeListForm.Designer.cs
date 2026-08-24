// VS 디자이너가 자동 생성하는 필드 선언에는 = null!을 안 붙여서 nullable 경고(CS8618)가
// 계속 나기 때문에, 이 파일(디자이너 전용)만 nullable 검사를 끈다 - 흔한 관례.
#nullable disable
using DevExpress.XtraGrid;
using WYNLAB.Base.Controls;

namespace WYNLAB.SM.CODE;

/// <summary>
/// 빈 캔버스 - VS 디자이너에서 드래그앤드롭으로 직접 배치할 예정이라 여기엔 아무 컨트롤도
/// 미리 넣어두지 않았다. 이전 손으로 짠 레이아웃/업무로직은 CodeListForm.cs.reference /
/// CodeListForm.Designer.cs.reference에 그대로 남겨뒀다 - 디자인이 정해지면 그쪽 로직을
/// 새 컨트롤 이름에 맞춰 다시 연결하면 된다.
/// </summary>
public partial class CodeListForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }
    private PanelWyn panBase = null!;
    private TextEditWyn txtmajor_cd = null!;

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeListForm));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDeletRow2 = new DevExpress.XtraEditors.SimpleButton();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.btnAddRow2 = new DevExpress.XtraEditors.SimpleButton();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.cborel_cd_type10 = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.cborel_cd_type6 = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.cborel_cd_type9 = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.cborel_cd_type5 = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.cborel_cd_type8 = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.cborel_cd_type4 = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.cborel_cd_type7 = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.cborel_cd_type3 = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.cborel_cd_type2 = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.cborel_cd_type1 = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtrel_title10 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtrel_title6 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtrel_cd10 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtrel_cd6 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtrel_title9 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtrel_title5 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtrel_cd9 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtrel_cd5 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtrel_title8 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtrel_title4 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtrel_cd8 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtrel_cd4 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtrel_title7 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtrel_cd7 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtrel_title3 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtrel_cd3 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtrel_title2 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtrel_cd2 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtrel_title1 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtrel_cd1 = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtmajor_nm = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtmajor_cd = new WYNLAB.Base.Controls.TextEditWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.txtminor_cd_q = new WYNLAB.Base.Controls.TextEditWyn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
            this.panelWyn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
            this.panelWyn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
            this.panelWyn4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cborel_cd_type10.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cborel_cd_type6.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cborel_cd_type9.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cborel_cd_type5.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cborel_cd_type8.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cborel_cd_type4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cborel_cd_type7.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cborel_cd_type3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cborel_cd_type2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cborel_cd_type1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_title10.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_title6.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_cd10.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_cd6.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_title9.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_title5.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_cd9.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_cd5.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_title8.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_title4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_cd8.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_cd4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_title7.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_cd7.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_title3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_cd3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_title2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_cd2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_title1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_cd1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtmajor_nm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtmajor_cd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
            this.panelWyn6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtminor_cd_q.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).BeginInit();
            this.paTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // panBase
            // 
            this.panBase.Appearance.BackColor = System.Drawing.Color.White;
            this.panBase.Appearance.Options.UseBackColor = true;
            this.panBase.Controls.Add(this.panelWyn3);
            this.panBase.Controls.Add(this.panHeader);
            this.panBase.Controls.Add(this.paTitle);
            this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBase.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panBase.Location = new System.Drawing.Point(0, 0);
            this.panBase.Name = "panBase";
            this.panBase.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.panBase.Size = new System.Drawing.Size(1165, 600);
            this.panBase.TabIndex = 5;
            // 
            // panelWyn3
            // 
            this.panelWyn3.Controls.Add(this.grd1);
            this.panelWyn3.Controls.Add(this.panelWyn2);
            this.panelWyn3.Controls.Add(this.splitterWyn1);
            this.panelWyn3.Controls.Add(this.panelWyn4);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(3, 82);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Size = new System.Drawing.Size(1159, 515);
            this.panelWyn3.TabIndex = 7;
            // 
            // grd1
            // 
            this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd1.Location = new System.Drawing.Point(0, 27);
            this.grd1.MainView = this.gvw1;
            this.grd1.Name = "grd1";
            this.grd1.Size = new System.Drawing.Size(382, 488);
            this.grd1.TabIndex = 10;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "대분류코드";
            this.gridColumn1.FieldName = "major_cd";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "대분류명";
            this.gridColumn2.FieldName = "major_nm";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // panelWyn2
            // 
            this.panelWyn2.Controls.Add(this.sectionHeaderWyn4);
            this.panelWyn2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn2.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn2.Location = new System.Drawing.Point(0, 0);
            this.panelWyn2.Name = "panelWyn2";
            this.panelWyn2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.panelWyn2.Size = new System.Drawing.Size(382, 27);
            this.panelWyn2.TabIndex = 11;
            // 
            // sectionHeaderWyn4
            // 
            this.sectionHeaderWyn4.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn4.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn4.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn4.Name = "sectionHeaderWyn4";
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(377, 27);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 8;
            this.sectionHeaderWyn4.Text = "대분류 LIST";
            // 
            // splitterWyn1
            // 
            this.splitterWyn1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.splitterWyn1.Appearance.Options.UseBackColor = true;
            this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitterWyn1.Location = new System.Drawing.Point(382, 0);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(10, 515);
            this.splitterWyn1.TabIndex = 9;
            this.splitterWyn1.TabStop = false;
            // 
            // panelWyn4
            // 
            this.panelWyn4.Controls.Add(this.grd2);
            this.panelWyn4.Controls.Add(this.panelWyn1);
            this.panelWyn4.Controls.Add(this.panelWyn5);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(392, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Size = new System.Drawing.Size(767, 515);
            this.panelWyn4.TabIndex = 7;
            // 
            // grd2
            // 
            this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd2.Location = new System.Drawing.Point(0, 245);
            this.grd2.MainView = this.gvw2;
            this.grd2.Name = "grd2";
            this.grd2.Size = new System.Drawing.Size(767, 270);
            this.grd2.TabIndex = 7;
            this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw2});
            // 
            // gvw2
            // 
            this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7});
            this.gvw2.GridControl = this.grd2;
            this.gvw2.Name = "gvw2";
            this.gvw2.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "소분류코드";
            this.gridColumn3.FieldName = "minor_cd";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "소분류명";
            this.gridColumn4.FieldName = "minor_nm";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 1;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "순서";
            this.gridColumn5.FieldName = "sort";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 2;
            this.gridColumn5.Width = 50;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "사용여부";
            this.gridColumn6.FieldName = "use_yn";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 3;
            this.gridColumn6.Width = 60;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "비고";
            this.gridColumn7.FieldName = "remark";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 4;
            // 
            // panelWyn1
            // 
            this.panelWyn1.Controls.Add(this.btnDeletRow2);
            this.panelWyn1.Controls.Add(this.sectionHeaderWyn2);
            this.panelWyn1.Controls.Add(this.btnAddRow2);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(0, 218);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.panelWyn1.Size = new System.Drawing.Size(767, 27);
            this.panelWyn1.TabIndex = 8;
            // 
            // btnDeletRow2
            // 
            this.btnDeletRow2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeletRow2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(236)))), ((int)(((byte)(234)))));
            this.btnDeletRow2.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(198)))), ((int)(((byte)(193)))));
            this.btnDeletRow2.Appearance.Font = new System.Drawing.Font("Segoe MDL2 Assets", 11F);
            this.btnDeletRow2.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnDeletRow2.Appearance.Options.UseBackColor = true;
            this.btnDeletRow2.Appearance.Options.UseBorderColor = true;
            this.btnDeletRow2.Appearance.Options.UseFont = true;
            this.btnDeletRow2.Appearance.Options.UseForeColor = true;
            this.btnDeletRow2.Location = new System.Drawing.Point(738, 3);
            this.btnDeletRow2.Name = "btnDeletRow2";
            this.btnDeletRow2.Size = new System.Drawing.Size(24, 22);
            this.btnDeletRow2.TabIndex = 0;
            this.btnDeletRow2.Text = "";
            this.btnDeletRow2.ToolTip = "행삭제";
            this.btnDeletRow2.Click += new System.EventHandler(this.btnDeletRow2_Click);
            // 
            // sectionHeaderWyn2
            // 
            this.sectionHeaderWyn2.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn2.Dock = System.Windows.Forms.DockStyle.Left;
            this.sectionHeaderWyn2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn2.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn2.Name = "sectionHeaderWyn2";
            this.sectionHeaderWyn2.Size = new System.Drawing.Size(425, 27);
            this.sectionHeaderWyn2.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn2.SvgIcon")));
            this.sectionHeaderWyn2.TabIndex = 8;
            this.sectionHeaderWyn2.Text = "소분류 등록";
            // 
            // btnAddRow2
            // 
            this.btnAddRow2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddRow2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(245)))), ((int)(((byte)(231)))));
            this.btnAddRow2.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(231)))), ((int)(((byte)(203)))));
            this.btnAddRow2.Appearance.Font = new System.Drawing.Font("Segoe MDL2 Assets", 11F);
            this.btnAddRow2.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnAddRow2.Appearance.Options.UseBackColor = true;
            this.btnAddRow2.Appearance.Options.UseBorderColor = true;
            this.btnAddRow2.Appearance.Options.UseFont = true;
            this.btnAddRow2.Appearance.Options.UseForeColor = true;
            this.btnAddRow2.Location = new System.Drawing.Point(710, 3);
            this.btnAddRow2.Name = "btnAddRow2";
            this.btnAddRow2.Size = new System.Drawing.Size(24, 22);
            this.btnAddRow2.TabIndex = 0;
            this.btnAddRow2.Text = "";
            this.btnAddRow2.ToolTip = "행추가";
            this.btnAddRow2.Click += new System.EventHandler(this.btnAddRow2_Click);
            // 
            // panelWyn5
            // 
            this.panelWyn5.Controls.Add(this.panData);
            this.panelWyn5.Controls.Add(this.panelWyn6);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(0, 0);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Size = new System.Drawing.Size(767, 218);
            this.panelWyn5.TabIndex = 6;
            // 
            // panData
            // 
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Controls.Add(this.cborel_cd_type10);
            this.panData.Controls.Add(this.cborel_cd_type6);
            this.panData.Controls.Add(this.cborel_cd_type9);
            this.panData.Controls.Add(this.cborel_cd_type5);
            this.panData.Controls.Add(this.cborel_cd_type8);
            this.panData.Controls.Add(this.cborel_cd_type4);
            this.panData.Controls.Add(this.cborel_cd_type7);
            this.panData.Controls.Add(this.cborel_cd_type3);
            this.panData.Controls.Add(this.cborel_cd_type2);
            this.panData.Controls.Add(this.cborel_cd_type1);
            this.panData.Controls.Add(this.labelControl15);
            this.panData.Controls.Add(this.labelControl11);
            this.panData.Controls.Add(this.labelControl14);
            this.panData.Controls.Add(this.labelControl10);
            this.panData.Controls.Add(this.labelControl13);
            this.panData.Controls.Add(this.labelControl9);
            this.panData.Controls.Add(this.labelControl12);
            this.panData.Controls.Add(this.labelControl8);
            this.panData.Controls.Add(this.labelControl6);
            this.panData.Controls.Add(this.labelControl7);
            this.panData.Controls.Add(this.labelControl5);
            this.panData.Controls.Add(this.labelControl4);
            this.panData.Controls.Add(this.txtrel_title10);
            this.panData.Controls.Add(this.txtrel_title6);
            this.panData.Controls.Add(this.txtrel_cd10);
            this.panData.Controls.Add(this.txtrel_cd6);
            this.panData.Controls.Add(this.txtrel_title9);
            this.panData.Controls.Add(this.txtrel_title5);
            this.panData.Controls.Add(this.txtrel_cd9);
            this.panData.Controls.Add(this.txtrel_cd5);
            this.panData.Controls.Add(this.txtrel_title8);
            this.panData.Controls.Add(this.txtrel_title4);
            this.panData.Controls.Add(this.txtrel_cd8);
            this.panData.Controls.Add(this.txtrel_cd4);
            this.panData.Controls.Add(this.txtrel_title7);
            this.panData.Controls.Add(this.txtrel_cd7);
            this.panData.Controls.Add(this.txtrel_title3);
            this.panData.Controls.Add(this.txtrel_cd3);
            this.panData.Controls.Add(this.txtrel_title2);
            this.panData.Controls.Add(this.txtrel_cd2);
            this.panData.Controls.Add(this.txtrel_title1);
            this.panData.Controls.Add(this.txtrel_cd1);
            this.panData.Controls.Add(this.txtmajor_nm);
            this.panData.Controls.Add(this.txtmajor_cd);
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(767, 191);
            this.panData.TabIndex = 8;
            // 
            // cborel_cd_type10
            // 
            this.cborel_cd_type10.Location = new System.Drawing.Point(510, 148);
            this.cborel_cd_type10.Name = "cborel_cd_type10";
            this.cborel_cd_type10.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cborel_cd_type10.Properties.NullText = "";
            this.cborel_cd_type10.Size = new System.Drawing.Size(116, 20);
            this.cborel_cd_type10.TabIndex = 2;
            // 
            // cborel_cd_type6
            // 
            this.cborel_cd_type6.Location = new System.Drawing.Point(510, 44);
            this.cborel_cd_type6.Name = "cborel_cd_type6";
            this.cborel_cd_type6.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cborel_cd_type6.Properties.NullText = "";
            this.cborel_cd_type6.Size = new System.Drawing.Size(116, 20);
            this.cborel_cd_type6.TabIndex = 2;
            // 
            // cborel_cd_type9
            // 
            this.cborel_cd_type9.Location = new System.Drawing.Point(510, 122);
            this.cborel_cd_type9.Name = "cborel_cd_type9";
            this.cborel_cd_type9.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cborel_cd_type9.Properties.NullText = "";
            this.cborel_cd_type9.Size = new System.Drawing.Size(116, 20);
            this.cborel_cd_type9.TabIndex = 2;
            // 
            // cborel_cd_type5
            // 
            this.cborel_cd_type5.Location = new System.Drawing.Point(162, 148);
            this.cborel_cd_type5.Name = "cborel_cd_type5";
            this.cborel_cd_type5.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cborel_cd_type5.Properties.NullText = "";
            this.cborel_cd_type5.Size = new System.Drawing.Size(116, 20);
            this.cborel_cd_type5.TabIndex = 2;
            // 
            // cborel_cd_type8
            // 
            this.cborel_cd_type8.Location = new System.Drawing.Point(510, 96);
            this.cborel_cd_type8.Name = "cborel_cd_type8";
            this.cborel_cd_type8.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cborel_cd_type8.Properties.NullText = "";
            this.cborel_cd_type8.Size = new System.Drawing.Size(116, 20);
            this.cborel_cd_type8.TabIndex = 2;
            // 
            // cborel_cd_type4
            // 
            this.cborel_cd_type4.Location = new System.Drawing.Point(162, 122);
            this.cborel_cd_type4.Name = "cborel_cd_type4";
            this.cborel_cd_type4.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cborel_cd_type4.Properties.NullText = "";
            this.cborel_cd_type4.Size = new System.Drawing.Size(116, 20);
            this.cborel_cd_type4.TabIndex = 2;
            // 
            // cborel_cd_type7
            // 
            this.cborel_cd_type7.Location = new System.Drawing.Point(510, 70);
            this.cborel_cd_type7.Name = "cborel_cd_type7";
            this.cborel_cd_type7.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cborel_cd_type7.Properties.NullText = "";
            this.cborel_cd_type7.Size = new System.Drawing.Size(116, 20);
            this.cborel_cd_type7.TabIndex = 2;
            // 
            // cborel_cd_type3
            // 
            this.cborel_cd_type3.Location = new System.Drawing.Point(162, 96);
            this.cborel_cd_type3.Name = "cborel_cd_type3";
            this.cborel_cd_type3.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cborel_cd_type3.Properties.NullText = "";
            this.cborel_cd_type3.Size = new System.Drawing.Size(116, 20);
            this.cborel_cd_type3.TabIndex = 2;
            // 
            // cborel_cd_type2
            // 
            this.cborel_cd_type2.Location = new System.Drawing.Point(162, 70);
            this.cborel_cd_type2.Name = "cborel_cd_type2";
            this.cborel_cd_type2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cborel_cd_type2.Properties.NullText = "";
            this.cborel_cd_type2.Size = new System.Drawing.Size(116, 20);
            this.cborel_cd_type2.TabIndex = 2;
            // 
            // cborel_cd_type1
            // 
            this.cborel_cd_type1.Location = new System.Drawing.Point(162, 44);
            this.cborel_cd_type1.Name = "cborel_cd_type1";
            this.cborel_cd_type1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cborel_cd_type1.Properties.NullText = "";
            this.cborel_cd_type1.Size = new System.Drawing.Size(116, 20);
            this.cborel_cd_type1.TabIndex = 2;
            // 
            // labelControl15
            // 
            this.labelControl15.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl15.Appearance.Options.UseFont = true;
            this.labelControl15.Location = new System.Drawing.Point(381, 150);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(38, 15);
            this.labelControl15.TabIndex = 0;
            this.labelControl15.Text = "참조10";
            // 
            // labelControl11
            // 
            this.labelControl11.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl11.Appearance.Options.UseFont = true;
            this.labelControl11.Location = new System.Drawing.Point(388, 49);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(31, 15);
            this.labelControl11.TabIndex = 0;
            this.labelControl11.Text = "참조6";
            // 
            // labelControl14
            // 
            this.labelControl14.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl14.Appearance.Options.UseFont = true;
            this.labelControl14.Location = new System.Drawing.Point(388, 127);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(31, 15);
            this.labelControl14.TabIndex = 0;
            this.labelControl14.Text = "참조9";
            // 
            // labelControl10
            // 
            this.labelControl10.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl10.Appearance.Options.UseFont = true;
            this.labelControl10.Location = new System.Drawing.Point(40, 150);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(31, 15);
            this.labelControl10.TabIndex = 0;
            this.labelControl10.Text = "참조5";
            // 
            // labelControl13
            // 
            this.labelControl13.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl13.Appearance.Options.UseFont = true;
            this.labelControl13.Location = new System.Drawing.Point(388, 101);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(31, 15);
            this.labelControl13.TabIndex = 0;
            this.labelControl13.Text = "참조8";
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl9.Appearance.Options.UseFont = true;
            this.labelControl9.Location = new System.Drawing.Point(40, 127);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(31, 15);
            this.labelControl9.TabIndex = 0;
            this.labelControl9.Text = "참조4";
            // 
            // labelControl12
            // 
            this.labelControl12.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl12.Appearance.Options.UseFont = true;
            this.labelControl12.Location = new System.Drawing.Point(388, 75);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(31, 15);
            this.labelControl12.TabIndex = 0;
            this.labelControl12.Text = "참조7";
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl8.Appearance.Options.UseFont = true;
            this.labelControl8.Location = new System.Drawing.Point(40, 101);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(31, 15);
            this.labelControl8.TabIndex = 0;
            this.labelControl8.Text = "참조3";
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl6.Appearance.Options.UseFont = true;
            this.labelControl6.Location = new System.Drawing.Point(40, 75);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(31, 15);
            this.labelControl6.TabIndex = 0;
            this.labelControl6.Text = "참조2";
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl7.Appearance.Options.UseFont = true;
            this.labelControl7.Location = new System.Drawing.Point(40, 49);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(31, 15);
            this.labelControl7.TabIndex = 0;
            this.labelControl7.Text = "참조1";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(227, 20);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(48, 15);
            this.labelControl5.TabIndex = 0;
            this.labelControl5.Text = "대분류명";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(11, 20);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 15);
            this.labelControl4.TabIndex = 0;
            this.labelControl4.Text = "대분류코드";
            // 
            // txtrel_title10
            // 
            this.txtrel_title10.Location = new System.Drawing.Point(424, 148);
            this.txtrel_title10.Name = "txtrel_title10";
            this.txtrel_title10.Size = new System.Drawing.Size(84, 20);
            this.txtrel_title10.TabIndex = 1;
            // 
            // txtrel_title6
            // 
            this.txtrel_title6.Location = new System.Drawing.Point(424, 44);
            this.txtrel_title6.Name = "txtrel_title6";
            this.txtrel_title6.Size = new System.Drawing.Size(84, 20);
            this.txtrel_title6.TabIndex = 1;
            // 
            // txtrel_cd10
            // 
            this.txtrel_cd10.Location = new System.Drawing.Point(628, 148);
            this.txtrel_cd10.Name = "txtrel_cd10";
            this.txtrel_cd10.Size = new System.Drawing.Size(87, 20);
            this.txtrel_cd10.TabIndex = 1;
            // 
            // txtrel_cd6
            // 
            this.txtrel_cd6.Location = new System.Drawing.Point(628, 44);
            this.txtrel_cd6.Name = "txtrel_cd6";
            this.txtrel_cd6.Size = new System.Drawing.Size(87, 20);
            this.txtrel_cd6.TabIndex = 1;
            // 
            // txtrel_title9
            // 
            this.txtrel_title9.Location = new System.Drawing.Point(424, 122);
            this.txtrel_title9.Name = "txtrel_title9";
            this.txtrel_title9.Size = new System.Drawing.Size(84, 20);
            this.txtrel_title9.TabIndex = 1;
            // 
            // txtrel_title5
            // 
            this.txtrel_title5.Location = new System.Drawing.Point(76, 148);
            this.txtrel_title5.Name = "txtrel_title5";
            this.txtrel_title5.Size = new System.Drawing.Size(84, 20);
            this.txtrel_title5.TabIndex = 1;
            // 
            // txtrel_cd9
            // 
            this.txtrel_cd9.Location = new System.Drawing.Point(628, 122);
            this.txtrel_cd9.Name = "txtrel_cd9";
            this.txtrel_cd9.Size = new System.Drawing.Size(87, 20);
            this.txtrel_cd9.TabIndex = 1;
            // 
            // txtrel_cd5
            // 
            this.txtrel_cd5.Location = new System.Drawing.Point(280, 148);
            this.txtrel_cd5.Name = "txtrel_cd5";
            this.txtrel_cd5.Size = new System.Drawing.Size(87, 20);
            this.txtrel_cd5.TabIndex = 1;
            // 
            // txtrel_title8
            // 
            this.txtrel_title8.Location = new System.Drawing.Point(424, 96);
            this.txtrel_title8.Name = "txtrel_title8";
            this.txtrel_title8.Size = new System.Drawing.Size(84, 20);
            this.txtrel_title8.TabIndex = 1;
            // 
            // txtrel_title4
            // 
            this.txtrel_title4.Location = new System.Drawing.Point(76, 122);
            this.txtrel_title4.Name = "txtrel_title4";
            this.txtrel_title4.Size = new System.Drawing.Size(84, 20);
            this.txtrel_title4.TabIndex = 1;
            // 
            // txtrel_cd8
            // 
            this.txtrel_cd8.Location = new System.Drawing.Point(628, 96);
            this.txtrel_cd8.Name = "txtrel_cd8";
            this.txtrel_cd8.Size = new System.Drawing.Size(87, 20);
            this.txtrel_cd8.TabIndex = 1;
            // 
            // txtrel_cd4
            // 
            this.txtrel_cd4.Location = new System.Drawing.Point(280, 122);
            this.txtrel_cd4.Name = "txtrel_cd4";
            this.txtrel_cd4.Size = new System.Drawing.Size(87, 20);
            this.txtrel_cd4.TabIndex = 1;
            // 
            // txtrel_title7
            // 
            this.txtrel_title7.Location = new System.Drawing.Point(424, 70);
            this.txtrel_title7.Name = "txtrel_title7";
            this.txtrel_title7.Size = new System.Drawing.Size(84, 20);
            this.txtrel_title7.TabIndex = 1;
            // 
            // txtrel_cd7
            // 
            this.txtrel_cd7.Location = new System.Drawing.Point(628, 70);
            this.txtrel_cd7.Name = "txtrel_cd7";
            this.txtrel_cd7.Size = new System.Drawing.Size(87, 20);
            this.txtrel_cd7.TabIndex = 1;
            // 
            // txtrel_title3
            // 
            this.txtrel_title3.Location = new System.Drawing.Point(76, 96);
            this.txtrel_title3.Name = "txtrel_title3";
            this.txtrel_title3.Size = new System.Drawing.Size(84, 20);
            this.txtrel_title3.TabIndex = 1;
            // 
            // txtrel_cd3
            // 
            this.txtrel_cd3.Location = new System.Drawing.Point(280, 96);
            this.txtrel_cd3.Name = "txtrel_cd3";
            this.txtrel_cd3.Size = new System.Drawing.Size(87, 20);
            this.txtrel_cd3.TabIndex = 1;
            // 
            // txtrel_title2
            // 
            this.txtrel_title2.Location = new System.Drawing.Point(76, 70);
            this.txtrel_title2.Name = "txtrel_title2";
            this.txtrel_title2.Size = new System.Drawing.Size(84, 20);
            this.txtrel_title2.TabIndex = 1;
            // 
            // txtrel_cd2
            // 
            this.txtrel_cd2.Location = new System.Drawing.Point(280, 70);
            this.txtrel_cd2.Name = "txtrel_cd2";
            this.txtrel_cd2.Size = new System.Drawing.Size(87, 20);
            this.txtrel_cd2.TabIndex = 1;
            // 
            // txtrel_title1
            // 
            this.txtrel_title1.Location = new System.Drawing.Point(76, 44);
            this.txtrel_title1.Name = "txtrel_title1";
            this.txtrel_title1.Size = new System.Drawing.Size(84, 20);
            this.txtrel_title1.TabIndex = 1;
            // 
            // txtrel_cd1
            // 
            this.txtrel_cd1.Location = new System.Drawing.Point(280, 44);
            this.txtrel_cd1.Name = "txtrel_cd1";
            this.txtrel_cd1.Size = new System.Drawing.Size(87, 20);
            this.txtrel_cd1.TabIndex = 1;
            // 
            // txtmajor_nm
            // 
            this.txtmajor_nm.Location = new System.Drawing.Point(280, 18);
            this.txtmajor_nm.Name = "txtmajor_nm";
            this.txtmajor_nm.Size = new System.Drawing.Size(435, 20);
            this.txtmajor_nm.TabIndex = 1;
            // 
            // txtmajor_cd
            // 
            this.txtmajor_cd.Location = new System.Drawing.Point(76, 18);
            this.txtmajor_cd.Name = "txtmajor_cd";
            this.txtmajor_cd.Size = new System.Drawing.Size(84, 20);
            this.txtmajor_cd.TabIndex = 1;
            // 
            // panelWyn6
            // 
            this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
            this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn6.Location = new System.Drawing.Point(0, 0);
            this.panelWyn6.Name = "panelWyn6";
            this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.panelWyn6.Size = new System.Drawing.Size(767, 27);
            this.panelWyn6.TabIndex = 7;
            // 
            // sectionHeaderWyn3
            // 
            this.sectionHeaderWyn3.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn3.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn3.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn3.Name = "sectionHeaderWyn3";
            this.sectionHeaderWyn3.Size = new System.Drawing.Size(762, 27);
            this.sectionHeaderWyn3.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn3.SvgIcon")));
            this.sectionHeaderWyn3.TabIndex = 8;
            this.sectionHeaderWyn3.Text = "대분류 등록";
            // 
            // panHeader
            // 
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.txtminor_cd_q);
            this.panHeader.Controls.Add(this.labelControl1);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(3, 33);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1159, 49);
            this.panHeader.TabIndex = 8;
            // 
            // txtminor_cd_q
            // 
            this.txtminor_cd_q.Location = new System.Drawing.Point(106, 15);
            this.txtminor_cd_q.Name = "txtminor_cd_q";
            this.txtminor_cd_q.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtminor_cd_q.Size = new System.Drawing.Size(767, 20);
            this.txtminor_cd_q.TabIndex = 0;
            this.txtminor_cd_q.EditValueChanged += new System.EventHandler(this.textEditWyn1_EditValueChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(25, 20);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(77, 15);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "대분류코드/명";
            // 
            // paTitle
            // 
            this.paTitle.Controls.Add(this.sectionHeaderWyn1);
            this.paTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle.Location = new System.Drawing.Point(3, 0);
            this.paTitle.Name = "paTitle";
            this.paTitle.Size = new System.Drawing.Size(1159, 33);
            this.paTitle.TabIndex = 5;
            // 
            // sectionHeaderWyn1
            // 
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(5, 6);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(209, 23);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 8;
            this.sectionHeaderWyn1.Text = "기초코드 등록";
            // 
            // CodeListForm
            // 
            this.ClientSize = new System.Drawing.Size(1165, 600);
            this.Controls.Add(this.panBase);
            this.Name = "CodeListForm";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
            this.panelWyn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
            this.panData.ResumeLayout(false);
            this.panData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cborel_cd_type10.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cborel_cd_type6.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cborel_cd_type9.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cborel_cd_type5.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cborel_cd_type8.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cborel_cd_type4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cborel_cd_type7.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cborel_cd_type3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cborel_cd_type2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cborel_cd_type1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_title10.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_title6.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_cd10.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_cd6.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_title9.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_title5.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_cd9.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_cd5.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_title8.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_title4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_cd8.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_cd4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_title7.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_cd7.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_title3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_cd3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_title2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_cd2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_title1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtrel_cd1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtmajor_nm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtmajor_cd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
            this.panelWyn6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtminor_cd_q.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
            this.paTitle.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private PanelWyn paTitle;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn4;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private PanelWyn panelWyn6;
    private SectionHeaderWyn sectionHeaderWyn1;
    private SplitterWyn splitterWyn1;
    private DevExpress.XtraEditors.LabelControl labelControl5;
    private DevExpress.XtraEditors.LabelControl labelControl4;
    private TextEditWyn txtmajor_nm;
    private LookUpEditWyn cborel_cd_type10;
    private LookUpEditWyn cborel_cd_type6;
    private LookUpEditWyn cborel_cd_type9;
    private LookUpEditWyn cborel_cd_type5;
    private LookUpEditWyn cborel_cd_type8;
    private LookUpEditWyn cborel_cd_type4;
    private LookUpEditWyn cborel_cd_type7;
    private LookUpEditWyn cborel_cd_type3;
    private LookUpEditWyn cborel_cd_type2;
    private LookUpEditWyn cborel_cd_type1;
    private DevExpress.XtraEditors.LabelControl labelControl15;
    private DevExpress.XtraEditors.LabelControl labelControl11;
    private DevExpress.XtraEditors.LabelControl labelControl14;
    private DevExpress.XtraEditors.LabelControl labelControl10;
    private DevExpress.XtraEditors.LabelControl labelControl13;
    private DevExpress.XtraEditors.LabelControl labelControl9;
    private DevExpress.XtraEditors.LabelControl labelControl12;
    private DevExpress.XtraEditors.LabelControl labelControl8;
    private DevExpress.XtraEditors.LabelControl labelControl6;
    private DevExpress.XtraEditors.LabelControl labelControl7;
    private TextEditWyn txtrel_title10;
    private TextEditWyn txtrel_title6;
    private TextEditWyn txtrel_cd10;
    private TextEditWyn txtrel_cd6;
    private TextEditWyn txtrel_title9;
    private TextEditWyn txtrel_title5;
    private TextEditWyn txtrel_cd9;
    private TextEditWyn txtrel_cd5;
    private TextEditWyn txtrel_title8;
    private TextEditWyn txtrel_title4;
    private TextEditWyn txtrel_cd8;
    private TextEditWyn txtrel_cd4;
    private TextEditWyn txtrel_title7;
    private TextEditWyn txtrel_cd7;
    private TextEditWyn txtrel_title3;
    private TextEditWyn txtrel_cd3;
    private TextEditWyn txtrel_title2;
    private TextEditWyn txtrel_cd2;
    private TextEditWyn txtrel_title1;
    private TextEditWyn txtrel_cd1;
    private SectionHeaderWyn sectionHeaderWyn3;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private PanelWyn panelWyn2;
    private SectionHeaderWyn sectionHeaderWyn4;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
    private PanelWyn panelWyn1;
    private SectionHeaderWyn sectionHeaderWyn2;
    private PanelWyn panHeader;
    private TextEditWyn txtminor_cd_q;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private DevExpress.XtraEditors.SimpleButton btnDeletRow2;
    private DevExpress.XtraEditors.SimpleButton btnAddRow2;
}
