// VS 디자이너가 자동 생성하는 필드 선언에는 = null!을 안 붙여서 nullable 경고(CS8618)가
// 계속 나기 때문에, 이 파일(디자이너 전용)만 nullable 검사를 끈다 - 흔한 관례. 이 프로젝트를
// 복사해서 새 화면을 만들 때도 그 화면의 Designer.cs 맨 위에 이 줄을 그대로 유지할 것.
#nullable disable
using DevExpress.XtraGrid;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

/// <summary>
/// 부서등록 화면 - TEMPLATE(frmMinorCode 표준 레이아웃)에서 복사해서 만듦. 왼쪽 목록은 부서가
/// 계층구조라 grd1(그리드) 대신 tree1(트리)로 바꿨다 - dept_cd/par_dept_cd로 트리를 구성한다.
/// grd2(하위 목록)는 tree1에서 선택한 부서 소속 사원 목록(USP_BA_DEPT_Q의 Q1 분기).
/// </summary>
public partial class frmDept
{
    private System.ComponentModel.IContainer components = null;
    private PanelWyn panBase = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colEmpNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colJobGrade = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colJobType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colHpTel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEmail = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDeletRow2 = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddRow2 = new DevExpress.XtraEditors.SimpleButton();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.txtParDeptNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.lblDeptCd = new DevExpress.XtraEditors.LabelControl();
            this.txtDeptCd = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDeptNm = new DevExpress.XtraEditors.LabelControl();
            this.txtDeptNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblParDeptCd = new DevExpress.XtraEditors.LabelControl();
            this.txtParDeptCd = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.lblParDeptNm = new DevExpress.XtraEditors.LabelControl();
            this.lblDeptType = new DevExpress.XtraEditors.LabelControl();
            this.txtDeptType = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblRemark = new DevExpress.XtraEditors.LabelControl();
            this.txtRemark = new WYNLAB.Base.Controls.TextEditWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.tree1 = new WYNLAB.Base.Controls.TreeListWyn();
            this.colTreeDeptCd = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colTreeDeptNm = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colTreeParDeptCd = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.txtDeptCd_Q = new WYNLAB.Base.Controls.TextEditWyn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
            this.panelWyn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
            this.panelWyn4.SuspendLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtParDeptNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParDeptCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
            this.panelWyn6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).BeginInit();
            this.panelWyn8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tree1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
            this.panelWyn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptCd_Q.Properties)).BeginInit();
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
            this.panelWyn3.Controls.Add(this.panelWyn4);
            this.panelWyn3.Controls.Add(this.splitterWyn1);
            this.panelWyn3.Controls.Add(this.panelWyn8);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(3, 82);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Size = new System.Drawing.Size(1159, 515);
            this.panelWyn3.TabIndex = 7;
            // 
            // panelWyn4
            // 
            this.panelWyn4.Controls.Add(this.grd2);
            this.panelWyn4.Controls.Add(this.panelWyn1);
            this.panelWyn4.Controls.Add(this.panelWyn5);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(412, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelWyn4.Size = new System.Drawing.Size(747, 515);
            this.panelWyn4.TabIndex = 7;
            // 
            // grd2
            // 
            this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd2.Location = new System.Drawing.Point(3, 245);
            this.grd2.MainView = this.gvw2;
            this.grd2.Name = "grd2";
            this.grd2.Size = new System.Drawing.Size(744, 270);
            this.grd2.TabIndex = 7;
            this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw2});
            // 
            // gvw2
            // 
            this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colEmpNo,
            this.colEmpNm,
            this.colJobGrade,
            this.colJobType,
            this.colTel,
            this.colHpTel,
            this.colEmail});
            this.gvw2.GridControl = this.grd2;
            this.gvw2.HighlightFocusedRow = true;
            this.gvw2.Name = "gvw2";
            this.gvw2.OptionsView.ColumnAutoWidth = false;
            this.gvw2.OptionsView.ShowGroupPanel = false;
            // 
            // colEmpNo
            // 
            this.colEmpNo.Caption = "사번";
            this.colEmpNo.FieldName = "emp_no";
            this.colEmpNo.Name = "colEmpNo";
            this.colEmpNo.Visible = true;
            this.colEmpNo.VisibleIndex = 0;
            this.colEmpNo.Width = 80;
            // 
            // colEmpNm
            // 
            this.colEmpNm.Caption = "성명";
            this.colEmpNm.FieldName = "emp_nm";
            this.colEmpNm.Name = "colEmpNm";
            this.colEmpNm.Visible = true;
            this.colEmpNm.VisibleIndex = 1;
            this.colEmpNm.Width = 100;
            // 
            // colJobGrade
            // 
            this.colJobGrade.Caption = "직급";
            this.colJobGrade.FieldName = "job_grade";
            this.colJobGrade.Name = "colJobGrade";
            this.colJobGrade.Visible = true;
            this.colJobGrade.VisibleIndex = 2;
            this.colJobGrade.Width = 70;
            // 
            // colJobType
            // 
            this.colJobType.Caption = "직책";
            this.colJobType.FieldName = "job_type";
            this.colJobType.Name = "colJobType";
            this.colJobType.Visible = true;
            this.colJobType.VisibleIndex = 3;
            this.colJobType.Width = 70;
            // 
            // colTel
            // 
            this.colTel.Caption = "전화";
            this.colTel.FieldName = "tel";
            this.colTel.Name = "colTel";
            this.colTel.Visible = true;
            this.colTel.VisibleIndex = 4;
            this.colTel.Width = 110;
            // 
            // colHpTel
            // 
            this.colHpTel.Caption = "휴대폰";
            this.colHpTel.FieldName = "hp_tel";
            this.colHpTel.Name = "colHpTel";
            this.colHpTel.Visible = true;
            this.colHpTel.VisibleIndex = 5;
            this.colHpTel.Width = 110;
            // 
            // colEmail
            // 
            this.colEmail.Caption = "이메일";
            this.colEmail.FieldName = "email";
            this.colEmail.Name = "colEmail";
            this.colEmail.Visible = true;
            this.colEmail.VisibleIndex = 6;
            this.colEmail.Width = 180;
            // 
            // panelWyn1
            // 
            this.panelWyn1.Controls.Add(this.panelWyn7);
            this.panelWyn1.Controls.Add(this.sectionHeaderWyn2);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(3, 218);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn1.Size = new System.Drawing.Size(744, 27);
            this.panelWyn1.TabIndex = 8;
            // 
            // panelWyn7
            // 
            this.panelWyn7.Controls.Add(this.btnDeletRow2);
            this.panelWyn7.Controls.Add(this.btnAddRow2);
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(676, 0);
            this.panelWyn7.Name = "panelWyn7";
            this.panelWyn7.Size = new System.Drawing.Size(68, 25);
            this.panelWyn7.TabIndex = 9;
            // 
            // btnDeletRow2
            // 
            this.btnDeletRow2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeletRow2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(236)))), ((int)(((byte)(234)))));
            this.btnDeletRow2.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(198)))), ((int)(((byte)(193)))));
            this.btnDeletRow2.Appearance.Font = new System.Drawing.Font("Segoe MDL2 Assets", 11F);
            this.btnDeletRow2.Appearance.Options.UseBackColor = true;
            this.btnDeletRow2.Appearance.Options.UseBorderColor = true;
            this.btnDeletRow2.Appearance.Options.UseFont = true;
            this.btnDeletRow2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnDeletRow2.ImageOptions.Image")));
            this.btnDeletRow2.Location = new System.Drawing.Point(42, 2);
            this.btnDeletRow2.Name = "btnDeletRow2";
            this.btnDeletRow2.Size = new System.Drawing.Size(24, 22);
            this.btnDeletRow2.TabIndex = 0;
            this.btnDeletRow2.ToolTip = "행삭제";
            this.btnDeletRow2.Click += new System.EventHandler(this.btnDeletRow2_Click);
            // 
            // btnAddRow2
            // 
            this.btnAddRow2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddRow2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(245)))), ((int)(((byte)(231)))));
            this.btnAddRow2.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(231)))), ((int)(((byte)(203)))));
            this.btnAddRow2.Appearance.Font = new System.Drawing.Font("Segoe MDL2 Assets", 11F);
            this.btnAddRow2.Appearance.Options.UseBackColor = true;
            this.btnAddRow2.Appearance.Options.UseBorderColor = true;
            this.btnAddRow2.Appearance.Options.UseFont = true;
            this.btnAddRow2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAddRow2.ImageOptions.Image")));
            this.btnAddRow2.Location = new System.Drawing.Point(10, 2);
            this.btnAddRow2.Name = "btnAddRow2";
            this.btnAddRow2.Size = new System.Drawing.Size(24, 22);
            this.btnAddRow2.TabIndex = 0;
            this.btnAddRow2.ToolTip = "행추가";
            this.btnAddRow2.Click += new System.EventHandler(this.btnAddRow2_Click);
            // 
            // sectionHeaderWyn2
            // 
            this.sectionHeaderWyn2.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn2.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn2.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn2.Name = "sectionHeaderWyn2";
            this.sectionHeaderWyn2.Size = new System.Drawing.Size(739, 25);
            this.sectionHeaderWyn2.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn2.SvgIcon")));
            this.sectionHeaderWyn2.TabIndex = 8;
            this.sectionHeaderWyn2.Text = "소속 사원";
            // 
            // panelWyn5
            // 
            this.panelWyn5.Controls.Add(this.panData);
            this.panelWyn5.Controls.Add(this.panelWyn6);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(3, 0);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Size = new System.Drawing.Size(744, 218);
            this.panelWyn5.TabIndex = 6;
            // 
            // panData
            // 
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Controls.Add(this.txtParDeptNm);
            this.panData.Controls.Add(this.lblDeptCd);
            this.panData.Controls.Add(this.txtDeptCd);
            this.panData.Controls.Add(this.lblDeptNm);
            this.panData.Controls.Add(this.txtDeptNm);
            this.panData.Controls.Add(this.lblParDeptCd);
            this.panData.Controls.Add(this.txtParDeptCd);
            this.panData.Controls.Add(this.lblParDeptNm);
            this.panData.Controls.Add(this.lblDeptType);
            this.panData.Controls.Add(this.txtDeptType);
            this.panData.Controls.Add(this.lblRemark);
            this.panData.Controls.Add(this.txtRemark);
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(744, 191);
            this.panData.TabIndex = 8;
            // 
            // txtParDeptNm
            // 
            this.txtParDeptNm.Location = new System.Drawing.Point(286, 78);
            this.txtParDeptNm.LookupKey = "P_DEPT";
            this.txtParDeptNm.Name = "txtParDeptNm";
            this.txtParDeptNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", 24, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.txtParDeptNm.Size = new System.Drawing.Size(179, 20);
            this.txtParDeptNm.TabIndex = 23;
            this.txtParDeptNm.ToolTip = null;
            // 
            // lblDeptCd
            // 
            this.lblDeptCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDeptCd.Appearance.Options.UseFont = true;
            this.lblDeptCd.Location = new System.Drawing.Point(11, 20);
            this.lblDeptCd.Name = "lblDeptCd";
            this.lblDeptCd.Size = new System.Drawing.Size(48, 15);
            this.lblDeptCd.TabIndex = 0;
            this.lblDeptCd.Text = "부서코드";
            // 
            // txtDeptCd
            // 
            this.txtDeptCd.Location = new System.Drawing.Point(110, 18);
            this.txtDeptCd.Name = "txtDeptCd";
            this.txtDeptCd.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtDeptCd.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDeptCd.Properties.Appearance.Options.UseBackColor = true;
            this.txtDeptCd.Properties.Appearance.Options.UseForeColor = true;
            this.txtDeptCd.Required = true;
            this.txtDeptCd.Size = new System.Drawing.Size(97, 20);
            this.txtDeptCd.TabIndex = 1;
            // 
            // lblDeptNm
            // 
            this.lblDeptNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDeptNm.Appearance.Options.UseFont = true;
            this.lblDeptNm.Location = new System.Drawing.Point(11, 50);
            this.lblDeptNm.Name = "lblDeptNm";
            this.lblDeptNm.Size = new System.Drawing.Size(36, 15);
            this.lblDeptNm.TabIndex = 2;
            this.lblDeptNm.Text = "부서명";
            // 
            // txtDeptNm
            // 
            this.txtDeptNm.Location = new System.Drawing.Point(110, 48);
            this.txtDeptNm.Name = "txtDeptNm";
            this.txtDeptNm.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtDeptNm.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDeptNm.Properties.Appearance.Options.UseBackColor = true;
            this.txtDeptNm.Properties.Appearance.Options.UseForeColor = true;
            this.txtDeptNm.Required = true;
            this.txtDeptNm.Size = new System.Drawing.Size(300, 20);
            this.txtDeptNm.TabIndex = 3;
            // 
            // lblParDeptCd
            // 
            this.lblParDeptCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblParDeptCd.Appearance.Options.UseFont = true;
            this.lblParDeptCd.Location = new System.Drawing.Point(11, 80);
            this.lblParDeptCd.Name = "lblParDeptCd";
            this.lblParDeptCd.Size = new System.Drawing.Size(72, 15);
            this.lblParDeptCd.TabIndex = 4;
            this.lblParDeptCd.Text = "상위부서코드";
            // 
            // txtParDeptCd
            // 
            this.txtParDeptCd.Location = new System.Drawing.Point(110, 78);
            this.txtParDeptCd.LookupKey = "P_DEPT";
            this.txtParDeptCd.Name = "txtParDeptCd";
            this.txtParDeptCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", 24, true, true, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.txtParDeptCd.Size = new System.Drawing.Size(97, 20);
            this.txtParDeptCd.TabIndex = 5;
            this.txtParDeptCd.ToolTip = null;
            // 
            // lblParDeptNm
            // 
            this.lblParDeptNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblParDeptNm.Appearance.Options.UseFont = true;
            this.lblParDeptNm.Location = new System.Drawing.Point(220, 80);
            this.lblParDeptNm.Name = "lblParDeptNm";
            this.lblParDeptNm.Size = new System.Drawing.Size(60, 15);
            this.lblParDeptNm.TabIndex = 21;
            this.lblParDeptNm.Text = "상위부서명";
            // 
            // lblDeptType
            // 
            this.lblDeptType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDeptType.Appearance.Options.UseFont = true;
            this.lblDeptType.Location = new System.Drawing.Point(11, 110);
            this.lblDeptType.Name = "lblDeptType";
            this.lblDeptType.Size = new System.Drawing.Size(48, 15);
            this.lblDeptType.TabIndex = 6;
            this.lblDeptType.Text = "부서유형";
            // 
            // txtDeptType
            // 
            this.txtDeptType.Location = new System.Drawing.Point(110, 108);
            this.txtDeptType.Name = "txtDeptType";
            this.txtDeptType.Size = new System.Drawing.Size(97, 20);
            this.txtDeptType.TabIndex = 7;
            // 
            // lblRemark
            // 
            this.lblRemark.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRemark.Appearance.Options.UseFont = true;
            this.lblRemark.Location = new System.Drawing.Point(11, 140);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(24, 15);
            this.lblRemark.TabIndex = 8;
            this.lblRemark.Text = "비고";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(110, 138);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(500, 20);
            this.txtRemark.TabIndex = 9;
            // 
            // panelWyn6
            // 
            this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
            this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn6.Location = new System.Drawing.Point(0, 0);
            this.panelWyn6.Name = "panelWyn6";
            this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn6.Size = new System.Drawing.Size(744, 27);
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
            this.sectionHeaderWyn3.Size = new System.Drawing.Size(739, 25);
            this.sectionHeaderWyn3.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn3.SvgIcon")));
            this.sectionHeaderWyn3.TabIndex = 8;
            this.sectionHeaderWyn3.Text = "부서 상세";
            // 
            // splitterWyn1
            // 
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Location = new System.Drawing.Point(402, 0);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(10, 515);
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
            this.panelWyn8.Size = new System.Drawing.Size(402, 515);
            this.panelWyn8.TabIndex = 12;
            // 
            // tree1
            // 
            this.tree1.Appearance.Row.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.tree1.Appearance.Row.Options.UseFont = true;
            this.tree1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colTreeDeptCd,
            this.colTreeDeptNm,
            this.colTreeParDeptCd});
            this.tree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree1.Location = new System.Drawing.Point(0, 27);
            this.tree1.Name = "tree1";
            this.tree1.OptionsView.AutoWidth = false;
            this.tree1.OptionsView.ShowHorzLines = false;
            this.tree1.OptionsView.ShowIndicator = false;
            this.tree1.OptionsView.ShowVertLines = false;
            this.tree1.RowHeight = 26;
            this.tree1.Size = new System.Drawing.Size(402, 488);
            this.tree1.TabIndex = 10;
            // 
            // colTreeDeptCd
            // 
            this.colTreeDeptCd.Caption = "부서코드";
            this.colTreeDeptCd.FieldName = "dept_cd";
            this.colTreeDeptCd.Name = "colTreeDeptCd";
            this.colTreeDeptCd.Visible = true;
            this.colTreeDeptCd.VisibleIndex = 0;
            this.colTreeDeptCd.Width = 100;
            // 
            // colTreeDeptNm
            // 
            this.colTreeDeptNm.Caption = "부서명";
            this.colTreeDeptNm.FieldName = "dept_nm";
            this.colTreeDeptNm.Name = "colTreeDeptNm";
            this.colTreeDeptNm.Visible = true;
            this.colTreeDeptNm.VisibleIndex = 1;
            this.colTreeDeptNm.Width = 260;
            // 
            // colTreeParDeptCd
            // 
            this.colTreeParDeptCd.Caption = "상위부서코드";
            this.colTreeParDeptCd.FieldName = "par_dept_cd";
            this.colTreeParDeptCd.Name = "colTreeParDeptCd";
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
            this.sectionHeaderWyn4.TabIndex = 8;
            this.sectionHeaderWyn4.Text = "부서 트리";
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.txtDeptCd_Q);
            this.panHeader.Controls.Add(this.labelControl1);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(3, 33);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1159, 49);
            this.panHeader.TabIndex = 8;
            // 
            // txtDeptCd_Q
            // 
            this.txtDeptCd_Q.Location = new System.Drawing.Point(95, 15);
            this.txtDeptCd_Q.Name = "txtDeptCd_Q";
            this.txtDeptCd_Q.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtDeptCd_Q.Size = new System.Drawing.Size(179, 20);
            this.txtDeptCd_Q.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(25, 18);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(65, 15);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "부서코드/명";
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
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(5, 6);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(209, 23);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 8;
            this.sectionHeaderWyn1.Text = "부서등록 [frmDept]";
            // 
            // frmDept
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1165, 600);
            this.Controls.Add(this.panBase);
            this.Name = "frmDept";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
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
            ((System.ComponentModel.ISupportInitialize)(this.txtParDeptNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParDeptCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
            this.panelWyn6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).EndInit();
            this.panelWyn8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tree1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
            this.panelWyn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptCd_Q.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
            this.paTitle.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private PanelWyn paTitle;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn4;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblDeptCd;
    private TextEditWyn txtDeptCd;
    private DevExpress.XtraEditors.LabelControl lblDeptNm;
    private TextEditWyn txtDeptNm;
    private DevExpress.XtraEditors.LabelControl lblParDeptCd;
    private PopupLookupEditWyn txtParDeptCd;
    private DevExpress.XtraEditors.LabelControl lblParDeptNm;
    private DevExpress.XtraEditors.LabelControl lblDeptType;
    private TextEditWyn txtDeptType;
    private DevExpress.XtraEditors.LabelControl lblRemark;
    private TextEditWyn txtRemark;
    private PanelWyn panelWyn6;
    private SectionHeaderWyn sectionHeaderWyn1;
    private SplitterWyn splitterWyn1;
    private SectionHeaderWyn sectionHeaderWyn3;
    private TreeListWyn tree1;
    private DevExpress.XtraTreeList.Columns.TreeListColumn colTreeDeptCd;
    private DevExpress.XtraTreeList.Columns.TreeListColumn colTreeDeptNm;
    private DevExpress.XtraTreeList.Columns.TreeListColumn colTreeParDeptCd;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private DevExpress.XtraGrid.Columns.GridColumn colEmpNo;
    private DevExpress.XtraGrid.Columns.GridColumn colEmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn colJobGrade;
    private DevExpress.XtraGrid.Columns.GridColumn colJobType;
    private DevExpress.XtraGrid.Columns.GridColumn colTel;
    private DevExpress.XtraGrid.Columns.GridColumn colHpTel;
    private DevExpress.XtraGrid.Columns.GridColumn colEmail;
    private PanelWyn panelWyn2;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn panelWyn1;
    private SectionHeaderWyn sectionHeaderWyn2;
    private PanelWyn panHeader;
    private TextEditWyn txtDeptCd_Q;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private DevExpress.XtraEditors.SimpleButton btnDeletRow2;
    private DevExpress.XtraEditors.SimpleButton btnAddRow2;
    private PanelWyn panelWyn7;
    private PanelWyn panelWyn8;
    private PopupLookupEditWyn txtParDeptNm;
}
