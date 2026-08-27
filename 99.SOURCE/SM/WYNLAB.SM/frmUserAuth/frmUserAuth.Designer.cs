// VS 디자이너가 자동 생성하는 필드 선언에는 = null!을 안 붙여서 nullable 경고(CS8618)가
// 계속 나기 때문에, 이 파일(디자이너 전용)만 nullable 검사를 끈다 - 흔한 관례.
#nullable disable
using DevExpress.XtraGrid;
using WYNLAB.Base.Controls;

namespace WYNLAB.SM;

/// <summary>
/// 빈 캔버스 - VS 디자이너에서 드래그앤드롭으로 직접 배치할 예정이라 여기엔 아무 컨트롤도
/// 미리 넣어두지 않았다. 이전 손으로 짠 레이아웃/업무로직은 frmMinorCode.cs.reference /
/// frmMinorCode.Designer.cs.reference에 그대로 남겨뒀다 - 디자인이 정해지면 그쪽 로직을
/// 새 컨트롤 이름에 맞춰 다시 연결하면 된다.
/// </summary>
public partial class frmUserAuth
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

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserAuth));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.tabControlWyn1 = new WYNLAB.Base.Controls.TabControlWyn();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.panelWyn10 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn11 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn12 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn25 = new WYNLAB.Base.Controls.PanelWyn();
            this.tree1 = new WYNLAB.Base.Controls.TreeListWyn();
            this.colMenuCd = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn3 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colMenuType = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colViewYn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemCheckEdit10 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colInsertYn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemCheckEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colUpdateYn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemCheckEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colDeleteYn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemCheckEdit5 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.panelWyn27 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn28 = new WYNLAB.Base.Controls.PanelWyn();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
            this.sectionHeaderWyn9 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn3 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn26 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd3 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn13 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn14 = new WYNLAB.Base.Controls.PanelWyn();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.sectionHeaderWyn5 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn15 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn16 = new WYNLAB.Base.Controls.PanelWyn();
            this.checkBoxWyn1 = new WYNLAB.Base.Controls.CheckBoxWyn();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtuser_nm = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtuser_id = new WYNLAB.Base.Controls.TextEditWyn();
            this.panelWyn17 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn6 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn2 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn18 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn19 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn7 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn20 = new WYNLAB.Base.Controls.PanelWyn();
            this.txtUserGrpIdQ = new WYNLAB.Base.Controls.TextEditWyn();
            this.labelControl26 = new DevExpress.XtraEditors.LabelControl();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.panelWyn9 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn21 = new WYNLAB.Base.Controls.PanelWyn();
            this.tree2 = new WYNLAB.Base.Controls.TreeListWyn();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn4 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn5 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn6 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn7 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemCheckEdit9 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.treeListColumn8 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemCheckEdit6 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.treeListColumn9 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemCheckEdit7 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.treeListColumn10 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemCheckEdit8 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDeletRow2 = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddRow2 = new DevExpress.XtraEditors.SimpleButton();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn4 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn22 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd4 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn23 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn24 = new WYNLAB.Base.Controls.PanelWyn();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.sectionHeaderWyn8 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.memodescription = new WYNLAB.Base.Controls.MemoEditWyn();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtuser_grp_nm = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtuser_grp_cd = new WYNLAB.Base.Controls.TextEditWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.txtUserGrpCdQ = new WYNLAB.Base.Controls.TextEditWyn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlWyn1)).BeginInit();
            this.tabControlWyn1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn10)).BeginInit();
            this.panelWyn10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn11)).BeginInit();
            this.panelWyn11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn12)).BeginInit();
            this.panelWyn12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn25)).BeginInit();
            this.panelWyn25.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tree1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn27)).BeginInit();
            this.panelWyn27.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn28)).BeginInit();
            this.panelWyn28.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn26)).BeginInit();
            this.panelWyn26.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn13)).BeginInit();
            this.panelWyn13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn14)).BeginInit();
            this.panelWyn14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn15)).BeginInit();
            this.panelWyn15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn16)).BeginInit();
            this.panelWyn16.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxWyn1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtuser_nm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtuser_id.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn17)).BeginInit();
            this.panelWyn17.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn18)).BeginInit();
            this.panelWyn18.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn19)).BeginInit();
            this.panelWyn19.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn20)).BeginInit();
            this.panelWyn20.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserGrpIdQ.Properties)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn9)).BeginInit();
            this.panelWyn9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
            this.panelWyn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
            this.panelWyn4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn21)).BeginInit();
            this.panelWyn21.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tree2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn22)).BeginInit();
            this.panelWyn22.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn23)).BeginInit();
            this.panelWyn23.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn24)).BeginInit();
            this.panelWyn24.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memodescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtuser_grp_nm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtuser_grp_cd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
            this.panelWyn6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).BeginInit();
            this.panelWyn8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
            this.panelWyn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserGrpCdQ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).BeginInit();
            this.paTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // panBase
            // 
            this.panBase.Appearance.BackColor = System.Drawing.Color.White;
            this.panBase.Appearance.Options.UseBackColor = true;
            this.panBase.Controls.Add(this.tabControlWyn1);
            this.panBase.Controls.Add(this.paTitle);
            this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBase.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panBase.Location = new System.Drawing.Point(0, 0);
            this.panBase.Name = "panBase";
            this.panBase.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.panBase.Size = new System.Drawing.Size(1165, 600);
            this.panBase.TabIndex = 5;
            // 
            // tabControlWyn1
            // 
            this.tabControlWyn1.AppearancePage.Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(234)))), ((int)(((byte)(238)))));
            this.tabControlWyn1.AppearancePage.Header.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(122)))), ((int)(((byte)(128)))));
            this.tabControlWyn1.AppearancePage.Header.Options.UseBackColor = true;
            this.tabControlWyn1.AppearancePage.Header.Options.UseForeColor = true;
            this.tabControlWyn1.AppearancePage.HeaderActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tabControlWyn1.AppearancePage.HeaderActive.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.tabControlWyn1.AppearancePage.HeaderActive.Options.UseBackColor = true;
            this.tabControlWyn1.AppearancePage.HeaderActive.Options.UseForeColor = true;
            this.tabControlWyn1.AppearancePage.HeaderHotTracked.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            this.tabControlWyn1.AppearancePage.HeaderHotTracked.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.tabControlWyn1.AppearancePage.HeaderHotTracked.Options.UseBackColor = true;
            this.tabControlWyn1.AppearancePage.HeaderHotTracked.Options.UseForeColor = true;
            this.tabControlWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlWyn1.Location = new System.Drawing.Point(3, 30);
            this.tabControlWyn1.Name = "tabControlWyn1";
            this.tabControlWyn1.SelectedTabPage = this.xtraTabPage1;
            this.tabControlWyn1.Size = new System.Drawing.Size(1159, 567);
            this.tabControlWyn1.TabIndex = 10;
            this.tabControlWyn1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.panelWyn10);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(1157, 541);
            this.xtraTabPage1.Text = "  사용자별 권한관리    ";
            // 
            // panelWyn10
            // 
            this.panelWyn10.Controls.Add(this.panelWyn11);
            this.panelWyn10.Controls.Add(this.panelWyn20);
            this.panelWyn10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn10.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn10.Location = new System.Drawing.Point(0, 0);
            this.panelWyn10.Name = "panelWyn10";
            this.panelWyn10.Size = new System.Drawing.Size(1157, 541);
            this.panelWyn10.TabIndex = 9;
            // 
            // panelWyn11
            // 
            this.panelWyn11.Controls.Add(this.panelWyn12);
            this.panelWyn11.Controls.Add(this.splitterWyn2);
            this.panelWyn11.Controls.Add(this.panelWyn18);
            this.panelWyn11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn11.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn11.Location = new System.Drawing.Point(0, 49);
            this.panelWyn11.Name = "panelWyn11";
            this.panelWyn11.Size = new System.Drawing.Size(1157, 492);
            this.panelWyn11.TabIndex = 7;
            // 
            // panelWyn12
            // 
            this.panelWyn12.Controls.Add(this.panelWyn25);
            this.panelWyn12.Controls.Add(this.splitterWyn3);
            this.panelWyn12.Controls.Add(this.panelWyn26);
            this.panelWyn12.Controls.Add(this.panelWyn15);
            this.panelWyn12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn12.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn12.Location = new System.Drawing.Point(412, 0);
            this.panelWyn12.Name = "panelWyn12";
            this.panelWyn12.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelWyn12.Size = new System.Drawing.Size(745, 492);
            this.panelWyn12.TabIndex = 7;
            // 
            // panelWyn25
            // 
            this.panelWyn25.Controls.Add(this.tree1);
            this.panelWyn25.Controls.Add(this.panelWyn27);
            this.panelWyn25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn25.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn25.Location = new System.Drawing.Point(329, 187);
            this.panelWyn25.Name = "panelWyn25";
            this.panelWyn25.Size = new System.Drawing.Size(416, 305);
            this.panelWyn25.TabIndex = 10;
            // 
            // tree1
            // 
            this.tree1.Appearance.Row.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.tree1.Appearance.Row.Options.UseFont = true;
            this.tree1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colMenuCd,
            this.treeListColumn2,
            this.treeListColumn3,
            this.colMenuType,
            this.colViewYn,
            this.colInsertYn,
            this.colUpdateYn,
            this.colDeleteYn});
            this.tree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree1.Location = new System.Drawing.Point(0, 27);
            this.tree1.Name = "tree1";
            this.tree1.OptionsView.AutoWidth = false;
            this.tree1.OptionsView.ShowHorzLines = false;
            this.tree1.OptionsView.ShowIndicator = false;
            this.tree1.OptionsView.ShowVertLines = false;
            this.tree1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit3,
            this.repositoryItemCheckEdit4,
            this.repositoryItemCheckEdit5,
            this.repositoryItemCheckEdit10});
            this.tree1.RowHeight = 26;
            this.tree1.Size = new System.Drawing.Size(416, 278);
            this.tree1.TabIndex = 9;
            // 
            // colMenuCd
            // 
            this.colMenuCd.Caption = "메뉴코드";
            this.colMenuCd.FieldName = "MenuCd";
            this.colMenuCd.Name = "colMenuCd";
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.Caption = "상위메뉴코드";
            this.treeListColumn2.FieldName = "UpperMenuCd";
            this.treeListColumn2.Name = "treeListColumn2";
            // 
            // treeListColumn3
            // 
            this.treeListColumn3.Caption = "메뉴명";
            this.treeListColumn3.FieldName = "MenuNm";
            this.treeListColumn3.Name = "treeListColumn3";
            this.treeListColumn3.Visible = true;
            this.treeListColumn3.VisibleIndex = 0;
            this.treeListColumn3.Width = 195;
            // 
            // colMenuType
            // 
            this.colMenuType.Caption = "treeListColumn4";
            this.colMenuType.FieldName = "MenuType";
            this.colMenuType.Name = "colMenuType";
            // 
            // colViewYn
            // 
            this.colViewYn.Caption = "조회";
            this.colViewYn.ColumnEdit = this.repositoryItemCheckEdit10;
            this.colViewYn.FieldName = "ViewYn";
            this.colViewYn.Name = "colViewYn";
            this.colViewYn.Visible = true;
            this.colViewYn.VisibleIndex = 1;
            this.colViewYn.Width = 40;
            // 
            // repositoryItemCheckEdit10
            // 
            this.repositoryItemCheckEdit10.AutoHeight = false;
            this.repositoryItemCheckEdit10.Name = "repositoryItemCheckEdit10";
            // 
            // colInsertYn
            // 
            this.colInsertYn.Caption = "입력";
            this.colInsertYn.ColumnEdit = this.repositoryItemCheckEdit3;
            this.colInsertYn.FieldName = "InsertYn";
            this.colInsertYn.Name = "colInsertYn";
            this.colInsertYn.Visible = true;
            this.colInsertYn.VisibleIndex = 2;
            this.colInsertYn.Width = 36;
            // 
            // repositoryItemCheckEdit3
            // 
            this.repositoryItemCheckEdit3.AutoHeight = false;
            this.repositoryItemCheckEdit3.Name = "repositoryItemCheckEdit3";
            // 
            // colUpdateYn
            // 
            this.colUpdateYn.Caption = "저장";
            this.colUpdateYn.ColumnEdit = this.repositoryItemCheckEdit4;
            this.colUpdateYn.FieldName = "UpdateYn";
            this.colUpdateYn.Name = "colUpdateYn";
            this.colUpdateYn.Visible = true;
            this.colUpdateYn.VisibleIndex = 3;
            this.colUpdateYn.Width = 46;
            // 
            // repositoryItemCheckEdit4
            // 
            this.repositoryItemCheckEdit4.AutoHeight = false;
            this.repositoryItemCheckEdit4.Name = "repositoryItemCheckEdit4";
            // 
            // colDeleteYn
            // 
            this.colDeleteYn.Caption = "삭제";
            this.colDeleteYn.ColumnEdit = this.repositoryItemCheckEdit5;
            this.colDeleteYn.FieldName = "DeleteYn";
            this.colDeleteYn.Name = "colDeleteYn";
            this.colDeleteYn.Visible = true;
            this.colDeleteYn.VisibleIndex = 4;
            this.colDeleteYn.Width = 43;
            // 
            // repositoryItemCheckEdit5
            // 
            this.repositoryItemCheckEdit5.AutoHeight = false;
            this.repositoryItemCheckEdit5.Name = "repositoryItemCheckEdit5";
            // 
            // panelWyn27
            // 
            this.panelWyn27.Controls.Add(this.panelWyn28);
            this.panelWyn27.Controls.Add(this.sectionHeaderWyn9);
            this.panelWyn27.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn27.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn27.Location = new System.Drawing.Point(0, 0);
            this.panelWyn27.Name = "panelWyn27";
            this.panelWyn27.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn27.Size = new System.Drawing.Size(416, 27);
            this.panelWyn27.TabIndex = 8;
            // 
            // panelWyn28
            // 
            this.panelWyn28.Controls.Add(this.simpleButton5);
            this.panelWyn28.Controls.Add(this.simpleButton6);
            this.panelWyn28.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelWyn28.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn28.Location = new System.Drawing.Point(348, 0);
            this.panelWyn28.Name = "panelWyn28";
            this.panelWyn28.Size = new System.Drawing.Size(68, 25);
            this.panelWyn28.TabIndex = 9;
            this.panelWyn28.Visible = false;
            // 
            // simpleButton5
            // 
            this.simpleButton5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton5.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(236)))), ((int)(((byte)(234)))));
            this.simpleButton5.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(198)))), ((int)(((byte)(193)))));
            this.simpleButton5.Appearance.Font = new System.Drawing.Font("Segoe MDL2 Assets", 11F);
            this.simpleButton5.Appearance.Options.UseBackColor = true;
            this.simpleButton5.Appearance.Options.UseBorderColor = true;
            this.simpleButton5.Appearance.Options.UseFont = true;
            this.simpleButton5.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton5.ImageOptions.Image")));
            this.simpleButton5.Location = new System.Drawing.Point(42, 2);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(24, 22);
            this.simpleButton5.TabIndex = 0;
            this.simpleButton5.Text = "";
            this.simpleButton5.ToolTip = "행삭제";
            this.simpleButton5.Click += new System.EventHandler(this.btnDeletRow2_Click);
            // 
            // simpleButton6
            // 
            this.simpleButton6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton6.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(245)))), ((int)(((byte)(231)))));
            this.simpleButton6.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(231)))), ((int)(((byte)(203)))));
            this.simpleButton6.Appearance.Font = new System.Drawing.Font("Segoe MDL2 Assets", 11F);
            this.simpleButton6.Appearance.Options.UseBackColor = true;
            this.simpleButton6.Appearance.Options.UseBorderColor = true;
            this.simpleButton6.Appearance.Options.UseFont = true;
            this.simpleButton6.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton6.ImageOptions.Image")));
            this.simpleButton6.Location = new System.Drawing.Point(10, 2);
            this.simpleButton6.Name = "simpleButton6";
            this.simpleButton6.Size = new System.Drawing.Size(24, 22);
            this.simpleButton6.TabIndex = 0;
            this.simpleButton6.Text = "";
            this.simpleButton6.ToolTip = "행추가";
            this.simpleButton6.Click += new System.EventHandler(this.btnAddRow2_Click);
            // 
            // sectionHeaderWyn9
            // 
            this.sectionHeaderWyn9.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn9.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn9.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn9.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn9.Name = "sectionHeaderWyn9";
            this.sectionHeaderWyn9.Size = new System.Drawing.Size(411, 25);
            this.sectionHeaderWyn9.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn9.SvgIcon")));
            this.sectionHeaderWyn9.TabIndex = 8;
            this.sectionHeaderWyn9.Text = "사용자별권한 등록";
            // 
            // splitterWyn3
            // 
            this.splitterWyn3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn3.Location = new System.Drawing.Point(319, 187);
            this.splitterWyn3.Name = "splitterWyn3";
            this.splitterWyn3.Size = new System.Drawing.Size(10, 305);
            this.splitterWyn3.TabIndex = 11;
            this.splitterWyn3.TabStop = false;
            // 
            // panelWyn26
            // 
            this.panelWyn26.Controls.Add(this.grd3);
            this.panelWyn26.Controls.Add(this.panelWyn13);
            this.panelWyn26.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelWyn26.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn26.Location = new System.Drawing.Point(3, 187);
            this.panelWyn26.Name = "panelWyn26";
            this.panelWyn26.Size = new System.Drawing.Size(316, 305);
            this.panelWyn26.TabIndex = 10;
            // 
            // grd3
            // 
            this.grd3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd3.Location = new System.Drawing.Point(0, 27);
            this.grd3.MainView = this.gvw3;
            this.grd3.Name = "grd3";
            this.grd3.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.grd3.Size = new System.Drawing.Size(316, 278);
            this.grd3.TabIndex = 9;
            this.grd3.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw3});
            // 
            // gvw3
            // 
            this.gvw3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5});
            this.gvw3.GridControl = this.grd3;
            this.gvw3.Name = "gvw3";
            this.gvw3.OptionsView.ColumnAutoWidth = false;
            this.gvw3.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = " ";
            this.gridColumn3.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gridColumn3.FieldName = "IsMember";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            this.gridColumn3.Width = 29;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "사용자그룹ID";
            this.gridColumn4.FieldName = "UserGrpCd";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 1;
            this.gridColumn4.Width = 86;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "사용자그룹명";
            this.gridColumn5.FieldName = "UserGrpNm";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 2;
            this.gridColumn5.Width = 167;
            // 
            // panelWyn13
            // 
            this.panelWyn13.Controls.Add(this.panelWyn14);
            this.panelWyn13.Controls.Add(this.sectionHeaderWyn5);
            this.panelWyn13.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn13.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn13.Location = new System.Drawing.Point(0, 0);
            this.panelWyn13.Name = "panelWyn13";
            this.panelWyn13.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn13.Size = new System.Drawing.Size(316, 27);
            this.panelWyn13.TabIndex = 8;
            // 
            // panelWyn14
            // 
            this.panelWyn14.Controls.Add(this.simpleButton1);
            this.panelWyn14.Controls.Add(this.simpleButton2);
            this.panelWyn14.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelWyn14.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn14.Location = new System.Drawing.Point(248, 0);
            this.panelWyn14.Name = "panelWyn14";
            this.panelWyn14.Size = new System.Drawing.Size(68, 25);
            this.panelWyn14.TabIndex = 9;
            this.panelWyn14.Visible = false;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(236)))), ((int)(((byte)(234)))));
            this.simpleButton1.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(198)))), ((int)(((byte)(193)))));
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Segoe MDL2 Assets", 11F);
            this.simpleButton1.Appearance.Options.UseBackColor = true;
            this.simpleButton1.Appearance.Options.UseBorderColor = true;
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.ImageOptions.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(42, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(24, 22);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "";
            this.simpleButton1.ToolTip = "행삭제";
            this.simpleButton1.Click += new System.EventHandler(this.btnDeletRow2_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(245)))), ((int)(((byte)(231)))));
            this.simpleButton2.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(231)))), ((int)(((byte)(203)))));
            this.simpleButton2.Appearance.Font = new System.Drawing.Font("Segoe MDL2 Assets", 11F);
            this.simpleButton2.Appearance.Options.UseBackColor = true;
            this.simpleButton2.Appearance.Options.UseBorderColor = true;
            this.simpleButton2.Appearance.Options.UseFont = true;
            this.simpleButton2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.ImageOptions.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(10, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(24, 22);
            this.simpleButton2.TabIndex = 0;
            this.simpleButton2.Text = "";
            this.simpleButton2.ToolTip = "행추가";
            this.simpleButton2.Click += new System.EventHandler(this.btnAddRow2_Click);
            // 
            // sectionHeaderWyn5
            // 
            this.sectionHeaderWyn5.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn5.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn5.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn5.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn5.Name = "sectionHeaderWyn5";
            this.sectionHeaderWyn5.Size = new System.Drawing.Size(311, 25);
            this.sectionHeaderWyn5.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn5.SvgIcon")));
            this.sectionHeaderWyn5.TabIndex = 8;
            this.sectionHeaderWyn5.Text = "사용자별권한 등록";
            // 
            // panelWyn15
            // 
            this.panelWyn15.Controls.Add(this.panelWyn16);
            this.panelWyn15.Controls.Add(this.panelWyn17);
            this.panelWyn15.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn15.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn15.Location = new System.Drawing.Point(3, 0);
            this.panelWyn15.Name = "panelWyn15";
            this.panelWyn15.Size = new System.Drawing.Size(742, 187);
            this.panelWyn15.TabIndex = 6;
            // 
            // panelWyn16
            // 
            this.panelWyn16.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panelWyn16.Controls.Add(this.checkBoxWyn1);
            this.panelWyn16.Controls.Add(this.labelControl2);
            this.panelWyn16.Controls.Add(this.labelControl4);
            this.panelWyn16.Controls.Add(this.txtuser_nm);
            this.panelWyn16.Controls.Add(this.txtuser_id);
            this.panelWyn16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn16.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn16.Location = new System.Drawing.Point(0, 27);
            this.panelWyn16.Name = "panelWyn16";
            this.panelWyn16.Size = new System.Drawing.Size(742, 160);
            this.panelWyn16.TabIndex = 8;
            // 
            // checkBoxWyn1
            // 
            this.checkBoxWyn1.Location = new System.Drawing.Point(225, 16);
            this.checkBoxWyn1.Name = "checkBoxWyn1";
            this.checkBoxWyn1.Properties.Caption = "사용";
            this.checkBoxWyn1.Size = new System.Drawing.Size(80, 20);
            this.checkBoxWyn1.TabIndex = 7;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(35, 46);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 15);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "사용자명";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(35, 18);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(48, 15);
            this.labelControl4.TabIndex = 4;
            this.labelControl4.Text = "사용자ID";
            // 
            // txtuser_nm
            // 
            this.txtuser_nm.Location = new System.Drawing.Point(89, 43);
            this.txtuser_nm.Name = "txtuser_nm";
            this.txtuser_nm.Size = new System.Drawing.Size(97, 20);
            this.txtuser_nm.TabIndex = 6;
            // 
            // txtuser_id
            // 
            this.txtuser_id.Location = new System.Drawing.Point(89, 16);
            this.txtuser_id.Name = "txtuser_id";
            this.txtuser_id.Size = new System.Drawing.Size(97, 20);
            this.txtuser_id.TabIndex = 5;
            // 
            // panelWyn17
            // 
            this.panelWyn17.Controls.Add(this.sectionHeaderWyn6);
            this.panelWyn17.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn17.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn17.Location = new System.Drawing.Point(0, 0);
            this.panelWyn17.Name = "panelWyn17";
            this.panelWyn17.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn17.Size = new System.Drawing.Size(742, 27);
            this.panelWyn17.TabIndex = 7;
            // 
            // sectionHeaderWyn6
            // 
            this.sectionHeaderWyn6.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn6.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn6.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn6.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn6.Name = "sectionHeaderWyn6";
            this.sectionHeaderWyn6.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.sectionHeaderWyn6.Size = new System.Drawing.Size(737, 25);
            this.sectionHeaderWyn6.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn6.SvgIcon")));
            this.sectionHeaderWyn6.TabIndex = 8;
            this.sectionHeaderWyn6.Text = "사용자정보 등록";
            // 
            // splitterWyn2
            // 
            this.splitterWyn2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn2.Location = new System.Drawing.Point(402, 0);
            this.splitterWyn2.Name = "splitterWyn2";
            this.splitterWyn2.Size = new System.Drawing.Size(10, 492);
            this.splitterWyn2.TabIndex = 9;
            this.splitterWyn2.TabStop = false;
            // 
            // panelWyn18
            // 
            this.panelWyn18.Controls.Add(this.grd1);
            this.panelWyn18.Controls.Add(this.panelWyn19);
            this.panelWyn18.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelWyn18.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn18.Location = new System.Drawing.Point(0, 0);
            this.panelWyn18.Name = "panelWyn18";
            this.panelWyn18.Size = new System.Drawing.Size(402, 492);
            this.panelWyn18.TabIndex = 12;
            // 
            // grd1
            // 
            this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd1.Location = new System.Drawing.Point(0, 27);
            this.grd1.MainView = this.gvw1;
            this.grd1.Name = "grd1";
            this.grd1.Size = new System.Drawing.Size(402, 465);
            this.grd1.TabIndex = 12;
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
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "사용자ID";
            this.gridColumn1.FieldName = "UserId";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 98;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "사용자명";
            this.gridColumn2.FieldName = "UserNm";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 200;
            // 
            // panelWyn19
            // 
            this.panelWyn19.Controls.Add(this.sectionHeaderWyn7);
            this.panelWyn19.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn19.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn19.Location = new System.Drawing.Point(0, 0);
            this.panelWyn19.Name = "panelWyn19";
            this.panelWyn19.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn19.Size = new System.Drawing.Size(402, 27);
            this.panelWyn19.TabIndex = 11;
            // 
            // sectionHeaderWyn7
            // 
            this.sectionHeaderWyn7.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn7.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn7.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn7.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn7.Name = "sectionHeaderWyn7";
            this.sectionHeaderWyn7.Size = new System.Drawing.Size(397, 25);
            this.sectionHeaderWyn7.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn7.SvgIcon")));
            this.sectionHeaderWyn7.TabIndex = 8;
            this.sectionHeaderWyn7.Text = "사용자 LIST";
            // 
            // panelWyn20
            // 
            this.panelWyn20.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panelWyn20.Appearance.Options.UseBackColor = true;
            this.panelWyn20.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panelWyn20.Controls.Add(this.txtUserGrpIdQ);
            this.panelWyn20.Controls.Add(this.labelControl26);
            this.panelWyn20.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn20.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn20.Location = new System.Drawing.Point(0, 0);
            this.panelWyn20.Name = "panelWyn20";
            this.panelWyn20.Size = new System.Drawing.Size(1157, 49);
            this.panelWyn20.TabIndex = 8;
            // 
            // txtUserGrpIdQ
            // 
            this.txtUserGrpIdQ.Location = new System.Drawing.Point(100, 15);
            this.txtUserGrpIdQ.Name = "txtUserGrpIdQ";
            this.txtUserGrpIdQ.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtUserGrpIdQ.Size = new System.Drawing.Size(265, 20);
            this.txtUserGrpIdQ.TabIndex = 0;
            // 
            // labelControl26
            // 
            this.labelControl26.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl26.Appearance.Options.UseFont = true;
            this.labelControl26.Location = new System.Drawing.Point(31, 18);
            this.labelControl26.Name = "labelControl26";
            this.labelControl26.Size = new System.Drawing.Size(65, 15);
            this.labelControl26.TabIndex = 0;
            this.labelControl26.Text = "사용자ID/명";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.panelWyn9);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(1157, 541);
            this.xtraTabPage2.Text = "  사용자그룹별 권한관리    ";
            // 
            // panelWyn9
            // 
            this.panelWyn9.Controls.Add(this.panelWyn3);
            this.panelWyn9.Controls.Add(this.panHeader);
            this.panelWyn9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn9.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn9.Location = new System.Drawing.Point(0, 0);
            this.panelWyn9.Name = "panelWyn9";
            this.panelWyn9.Size = new System.Drawing.Size(1157, 541);
            this.panelWyn9.TabIndex = 9;
            // 
            // panelWyn3
            // 
            this.panelWyn3.Controls.Add(this.panelWyn4);
            this.panelWyn3.Controls.Add(this.splitterWyn1);
            this.panelWyn3.Controls.Add(this.panelWyn8);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(0, 49);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Size = new System.Drawing.Size(1157, 492);
            this.panelWyn3.TabIndex = 7;
            // 
            // panelWyn4
            // 
            this.panelWyn4.Controls.Add(this.panelWyn21);
            this.panelWyn4.Controls.Add(this.splitterWyn4);
            this.panelWyn4.Controls.Add(this.panelWyn22);
            this.panelWyn4.Controls.Add(this.panelWyn5);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(412, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelWyn4.Size = new System.Drawing.Size(745, 492);
            this.panelWyn4.TabIndex = 7;
            // 
            // panelWyn21
            // 
            this.panelWyn21.Controls.Add(this.tree2);
            this.panelWyn21.Controls.Add(this.panelWyn1);
            this.panelWyn21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn21.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn21.Location = new System.Drawing.Point(317, 194);
            this.panelWyn21.Name = "panelWyn21";
            this.panelWyn21.Size = new System.Drawing.Size(428, 298);
            this.panelWyn21.TabIndex = 11;
            // 
            // tree2
            // 
            this.tree2.Appearance.Row.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.tree2.Appearance.Row.Options.UseFont = true;
            this.tree2.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn4,
            this.treeListColumn5,
            this.treeListColumn6,
            this.treeListColumn7,
            this.treeListColumn8,
            this.treeListColumn9,
            this.treeListColumn10});
            this.tree2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree2.Location = new System.Drawing.Point(0, 27);
            this.tree2.Name = "tree2";
            this.tree2.OptionsView.AutoWidth = false;
            this.tree2.OptionsView.ShowHorzLines = false;
            this.tree2.OptionsView.ShowIndicator = false;
            this.tree2.OptionsView.ShowVertLines = false;
            this.tree2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit6,
            this.repositoryItemCheckEdit7,
            this.repositoryItemCheckEdit8,
            this.repositoryItemCheckEdit9});
            this.tree2.RowHeight = 26;
            this.tree2.Size = new System.Drawing.Size(428, 271);
            this.tree2.TabIndex = 10;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "메뉴코드";
            this.treeListColumn1.FieldName = "MenuCd";
            this.treeListColumn1.Name = "treeListColumn1";
            // 
            // treeListColumn4
            // 
            this.treeListColumn4.Caption = "상위메뉴코드";
            this.treeListColumn4.FieldName = "UpperMenuCd";
            this.treeListColumn4.Name = "treeListColumn4";
            // 
            // treeListColumn5
            // 
            this.treeListColumn5.Caption = "메뉴명";
            this.treeListColumn5.FieldName = "MenuNm";
            this.treeListColumn5.Name = "treeListColumn5";
            this.treeListColumn5.Visible = true;
            this.treeListColumn5.VisibleIndex = 0;
            this.treeListColumn5.Width = 195;
            // 
            // treeListColumn6
            // 
            this.treeListColumn6.Caption = "treeListColumn4";
            this.treeListColumn6.FieldName = "MenuType";
            this.treeListColumn6.Name = "treeListColumn6";
            // 
            // treeListColumn7
            // 
            this.treeListColumn7.Caption = "조회";
            this.treeListColumn7.ColumnEdit = this.repositoryItemCheckEdit9;
            this.treeListColumn7.FieldName = "ViewYn";
            this.treeListColumn7.Name = "treeListColumn7";
            this.treeListColumn7.Visible = true;
            this.treeListColumn7.VisibleIndex = 1;
            this.treeListColumn7.Width = 34;
            // 
            // repositoryItemCheckEdit9
            // 
            this.repositoryItemCheckEdit9.AutoHeight = false;
            this.repositoryItemCheckEdit9.Name = "repositoryItemCheckEdit9";
            // 
            // treeListColumn8
            // 
            this.treeListColumn8.Caption = "입력";
            this.treeListColumn8.ColumnEdit = this.repositoryItemCheckEdit6;
            this.treeListColumn8.FieldName = "InsertYn";
            this.treeListColumn8.Name = "treeListColumn8";
            this.treeListColumn8.Visible = true;
            this.treeListColumn8.VisibleIndex = 2;
            this.treeListColumn8.Width = 36;
            // 
            // repositoryItemCheckEdit6
            // 
            this.repositoryItemCheckEdit6.AutoHeight = false;
            this.repositoryItemCheckEdit6.Name = "repositoryItemCheckEdit6";
            // 
            // treeListColumn9
            // 
            this.treeListColumn9.Caption = "저장";
            this.treeListColumn9.ColumnEdit = this.repositoryItemCheckEdit7;
            this.treeListColumn9.FieldName = "UpdateYn";
            this.treeListColumn9.Name = "treeListColumn9";
            this.treeListColumn9.Visible = true;
            this.treeListColumn9.VisibleIndex = 3;
            this.treeListColumn9.Width = 46;
            // 
            // repositoryItemCheckEdit7
            // 
            this.repositoryItemCheckEdit7.AutoHeight = false;
            this.repositoryItemCheckEdit7.Name = "repositoryItemCheckEdit7";
            // 
            // treeListColumn10
            // 
            this.treeListColumn10.Caption = "삭제";
            this.treeListColumn10.ColumnEdit = this.repositoryItemCheckEdit8;
            this.treeListColumn10.FieldName = "DeleteYn";
            this.treeListColumn10.Name = "treeListColumn10";
            this.treeListColumn10.Visible = true;
            this.treeListColumn10.VisibleIndex = 4;
            this.treeListColumn10.Width = 43;
            // 
            // repositoryItemCheckEdit8
            // 
            this.repositoryItemCheckEdit8.AutoHeight = false;
            this.repositoryItemCheckEdit8.Name = "repositoryItemCheckEdit8";
            // 
            // panelWyn1
            // 
            this.panelWyn1.Controls.Add(this.panelWyn7);
            this.panelWyn1.Controls.Add(this.sectionHeaderWyn2);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(0, 0);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn1.Size = new System.Drawing.Size(428, 27);
            this.panelWyn1.TabIndex = 8;
            // 
            // panelWyn7
            // 
            this.panelWyn7.Controls.Add(this.btnDeletRow2);
            this.panelWyn7.Controls.Add(this.btnAddRow2);
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(360, 0);
            this.panelWyn7.Name = "panelWyn7";
            this.panelWyn7.Size = new System.Drawing.Size(68, 25);
            this.panelWyn7.TabIndex = 9;
            this.panelWyn7.Visible = false;
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
            this.btnDeletRow2.Text = "";
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
            this.btnAddRow2.Text = "";
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
            this.sectionHeaderWyn2.Size = new System.Drawing.Size(423, 25);
            this.sectionHeaderWyn2.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn2.SvgIcon")));
            this.sectionHeaderWyn2.TabIndex = 8;
            this.sectionHeaderWyn2.Text = "사용자그룹별 권한등록";
            // 
            // splitterWyn4
            // 
            this.splitterWyn4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn4.Location = new System.Drawing.Point(307, 194);
            this.splitterWyn4.Name = "splitterWyn4";
            this.splitterWyn4.Size = new System.Drawing.Size(10, 298);
            this.splitterWyn4.TabIndex = 12;
            this.splitterWyn4.TabStop = false;
            // 
            // panelWyn22
            // 
            this.panelWyn22.Controls.Add(this.grd4);
            this.panelWyn22.Controls.Add(this.panelWyn23);
            this.panelWyn22.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelWyn22.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn22.Location = new System.Drawing.Point(3, 194);
            this.panelWyn22.Name = "panelWyn22";
            this.panelWyn22.Size = new System.Drawing.Size(304, 298);
            this.panelWyn22.TabIndex = 11;
            // 
            // grd4
            // 
            this.grd4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd4.Location = new System.Drawing.Point(0, 27);
            this.grd4.MainView = this.gvw4;
            this.grd4.Name = "grd4";
            this.grd4.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit2});
            this.grd4.Size = new System.Drawing.Size(304, 271);
            this.grd4.TabIndex = 10;
            this.grd4.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw4});
            // 
            // gvw4
            // 
            this.gvw4.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10});
            this.gvw4.GridControl = this.grd4;
            this.gvw4.Name = "gvw4";
            this.gvw4.OptionsView.ColumnAutoWidth = false;
            this.gvw4.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn8
            // 
            this.gridColumn8.ColumnEdit = this.repositoryItemCheckEdit2;
            this.gridColumn8.FieldName = "IsMember";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 0;
            this.gridColumn8.Width = 40;
            // 
            // repositoryItemCheckEdit2
            // 
            this.repositoryItemCheckEdit2.AutoHeight = false;
            this.repositoryItemCheckEdit2.Name = "repositoryItemCheckEdit2";
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "사용자ID";
            this.gridColumn9.FieldName = "UserId";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 1;
            this.gridColumn9.Width = 100;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "사용자명";
            this.gridColumn10.FieldName = "UserNm";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 2;
            this.gridColumn10.Width = 134;
            // 
            // panelWyn23
            // 
            this.panelWyn23.Controls.Add(this.panelWyn24);
            this.panelWyn23.Controls.Add(this.sectionHeaderWyn8);
            this.panelWyn23.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn23.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn23.Location = new System.Drawing.Point(0, 0);
            this.panelWyn23.Name = "panelWyn23";
            this.panelWyn23.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn23.Size = new System.Drawing.Size(304, 27);
            this.panelWyn23.TabIndex = 8;
            // 
            // panelWyn24
            // 
            this.panelWyn24.Controls.Add(this.simpleButton3);
            this.panelWyn24.Controls.Add(this.simpleButton4);
            this.panelWyn24.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelWyn24.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn24.Location = new System.Drawing.Point(236, 0);
            this.panelWyn24.Name = "panelWyn24";
            this.panelWyn24.Size = new System.Drawing.Size(68, 25);
            this.panelWyn24.TabIndex = 9;
            // 
            // simpleButton3
            // 
            this.simpleButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton3.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(236)))), ((int)(((byte)(234)))));
            this.simpleButton3.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(198)))), ((int)(((byte)(193)))));
            this.simpleButton3.Appearance.Font = new System.Drawing.Font("Segoe MDL2 Assets", 11F);
            this.simpleButton3.Appearance.Options.UseBackColor = true;
            this.simpleButton3.Appearance.Options.UseBorderColor = true;
            this.simpleButton3.Appearance.Options.UseFont = true;
            this.simpleButton3.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton3.ImageOptions.Image")));
            this.simpleButton3.Location = new System.Drawing.Point(42, 2);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(24, 22);
            this.simpleButton3.TabIndex = 0;
            this.simpleButton3.Text = "";
            this.simpleButton3.ToolTip = "행삭제";
            this.simpleButton3.Click += new System.EventHandler(this.btnDeletRow2_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton4.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(245)))), ((int)(((byte)(231)))));
            this.simpleButton4.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(231)))), ((int)(((byte)(203)))));
            this.simpleButton4.Appearance.Font = new System.Drawing.Font("Segoe MDL2 Assets", 11F);
            this.simpleButton4.Appearance.Options.UseBackColor = true;
            this.simpleButton4.Appearance.Options.UseBorderColor = true;
            this.simpleButton4.Appearance.Options.UseFont = true;
            this.simpleButton4.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton4.ImageOptions.Image")));
            this.simpleButton4.Location = new System.Drawing.Point(10, 2);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(24, 22);
            this.simpleButton4.TabIndex = 0;
            this.simpleButton4.Text = "";
            this.simpleButton4.ToolTip = "행추가";
            this.simpleButton4.Click += new System.EventHandler(this.btnAddRow2_Click);
            // 
            // sectionHeaderWyn8
            // 
            this.sectionHeaderWyn8.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn8.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn8.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn8.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn8.Name = "sectionHeaderWyn8";
            this.sectionHeaderWyn8.Size = new System.Drawing.Size(299, 25);
            this.sectionHeaderWyn8.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn8.SvgIcon")));
            this.sectionHeaderWyn8.TabIndex = 8;
            this.sectionHeaderWyn8.Text = "사용자그룹별 권한등록";
            // 
            // panelWyn5
            // 
            this.panelWyn5.Controls.Add(this.panData);
            this.panelWyn5.Controls.Add(this.panelWyn6);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(3, 0);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Size = new System.Drawing.Size(742, 194);
            this.panelWyn5.TabIndex = 6;
            // 
            // panData
            // 
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Controls.Add(this.memodescription);
            this.panData.Controls.Add(this.labelControl6);
            this.panData.Controls.Add(this.labelControl3);
            this.panData.Controls.Add(this.labelControl5);
            this.panData.Controls.Add(this.txtuser_grp_nm);
            this.panData.Controls.Add(this.txtuser_grp_cd);
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(742, 167);
            this.panData.TabIndex = 8;
            // 
            // memodescription
            // 
            this.memodescription.Location = new System.Drawing.Point(108, 46);
            this.memodescription.Name = "memodescription";
            this.memodescription.Size = new System.Drawing.Size(354, 108);
            this.memodescription.TabIndex = 10;
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl6.Appearance.Options.UseFont = true;
            this.labelControl6.Location = new System.Drawing.Point(58, 52);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(46, 15);
            this.labelControl6.TabIndex = 6;
            this.labelControl6.Text = "REMARK";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(211, 22);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(72, 15);
            this.labelControl3.TabIndex = 6;
            this.labelControl3.Text = "사용자그룹명";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(32, 22);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(72, 15);
            this.labelControl5.TabIndex = 7;
            this.labelControl5.Text = "사용자그룹ID";
            // 
            // txtuser_grp_nm
            // 
            this.txtuser_grp_nm.Location = new System.Drawing.Point(289, 19);
            this.txtuser_grp_nm.Name = "txtuser_grp_nm";
            this.txtuser_grp_nm.Size = new System.Drawing.Size(173, 20);
            this.txtuser_grp_nm.TabIndex = 8;
            // 
            // txtuser_grp_cd
            // 
            this.txtuser_grp_cd.Location = new System.Drawing.Point(108, 20);
            this.txtuser_grp_cd.Name = "txtuser_grp_cd";
            this.txtuser_grp_cd.Size = new System.Drawing.Size(97, 20);
            this.txtuser_grp_cd.TabIndex = 9;
            // 
            // panelWyn6
            // 
            this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
            this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn6.Location = new System.Drawing.Point(0, 0);
            this.panelWyn6.Name = "panelWyn6";
            this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn6.Size = new System.Drawing.Size(742, 27);
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
            this.sectionHeaderWyn3.Size = new System.Drawing.Size(737, 25);
            this.sectionHeaderWyn3.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn3.SvgIcon")));
            this.sectionHeaderWyn3.TabIndex = 8;
            this.sectionHeaderWyn3.Text = "사용자그룹 등록";
            // 
            // splitterWyn1
            // 
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Location = new System.Drawing.Point(402, 0);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(10, 492);
            this.splitterWyn1.TabIndex = 9;
            this.splitterWyn1.TabStop = false;
            // 
            // panelWyn8
            // 
            this.panelWyn8.Controls.Add(this.grd2);
            this.panelWyn8.Controls.Add(this.panelWyn2);
            this.panelWyn8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelWyn8.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn8.Location = new System.Drawing.Point(0, 0);
            this.panelWyn8.Name = "panelWyn8";
            this.panelWyn8.Size = new System.Drawing.Size(402, 492);
            this.panelWyn8.TabIndex = 12;
            // 
            // grd2
            // 
            this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd2.Location = new System.Drawing.Point(0, 27);
            this.grd2.MainView = this.gvw2;
            this.grd2.Name = "grd2";
            this.grd2.Size = new System.Drawing.Size(402, 465);
            this.grd2.TabIndex = 12;
            this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw2});
            // 
            // gvw2
            // 
            this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn6,
            this.gridColumn7});
            this.gvw2.GridControl = this.grd2;
            this.gvw2.Name = "gvw2";
            this.gvw2.OptionsView.ColumnAutoWidth = false;
            this.gvw2.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "그룹코드";
            this.gridColumn6.FieldName = "UserGrpCd";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 0;
            this.gridColumn6.Width = 100;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "그룹명";
            this.gridColumn7.FieldName = "UserGrpNm";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 1;
            this.gridColumn7.Width = 194;
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
            this.sectionHeaderWyn4.Text = "사용자그룹 LIST";
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.txtUserGrpCdQ);
            this.panHeader.Controls.Add(this.labelControl1);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(0, 0);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1157, 49);
            this.panHeader.TabIndex = 8;
            // 
            // txtUserGrpCdQ
            // 
            this.txtUserGrpCdQ.Location = new System.Drawing.Point(100, 15);
            this.txtUserGrpCdQ.Name = "txtUserGrpCdQ";
            this.txtUserGrpCdQ.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtUserGrpCdQ.Size = new System.Drawing.Size(265, 20);
            this.txtUserGrpCdQ.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(9, 18);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(89, 15);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "사용자그룹ID/명";
            // 
            // paTitle
            // 
            this.paTitle.Controls.Add(this.sectionHeaderWyn1);
            this.paTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle.Location = new System.Drawing.Point(3, 0);
            this.paTitle.Name = "paTitle";
            this.paTitle.Size = new System.Drawing.Size(1159, 30);
            this.paTitle.TabIndex = 5;
            // 
            // sectionHeaderWyn1
            // 
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(0, 0);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1159, 30);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 8;
            this.sectionHeaderWyn1.Text = "사용자권한관리 [frmUserAuth]";
            // 
            // frmUserAuth
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1165, 600);
            this.Controls.Add(this.panBase);
            this.Name = "frmUserAuth";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControlWyn1)).EndInit();
            this.tabControlWyn1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn10)).EndInit();
            this.panelWyn10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn11)).EndInit();
            this.panelWyn11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn12)).EndInit();
            this.panelWyn12.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn25)).EndInit();
            this.panelWyn25.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tree1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn27)).EndInit();
            this.panelWyn27.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn28)).EndInit();
            this.panelWyn28.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn26)).EndInit();
            this.panelWyn26.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn13)).EndInit();
            this.panelWyn13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn14)).EndInit();
            this.panelWyn14.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn15)).EndInit();
            this.panelWyn15.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn16)).EndInit();
            this.panelWyn16.ResumeLayout(false);
            this.panelWyn16.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxWyn1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtuser_nm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtuser_id.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn17)).EndInit();
            this.panelWyn17.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn18)).EndInit();
            this.panelWyn18.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn19)).EndInit();
            this.panelWyn19.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn20)).EndInit();
            this.panelWyn20.ResumeLayout(false);
            this.panelWyn20.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserGrpIdQ.Properties)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn9)).EndInit();
            this.panelWyn9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn21)).EndInit();
            this.panelWyn21.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tree2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
            this.panelWyn7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn22)).EndInit();
            this.panelWyn22.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn23)).EndInit();
            this.panelWyn23.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn24)).EndInit();
            this.panelWyn24.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
            this.panData.ResumeLayout(false);
            this.panData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memodescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtuser_grp_nm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtuser_grp_cd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
            this.panelWyn6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).EndInit();
            this.panelWyn8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
            this.panelWyn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserGrpCdQ.Properties)).EndInit();
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
    private SectionHeaderWyn sectionHeaderWyn3;
    private PanelWyn panelWyn2;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn panelWyn1;
    private SectionHeaderWyn sectionHeaderWyn2;
    private PanelWyn panHeader;
    private TextEditWyn txtUserGrpCdQ;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private DevExpress.XtraEditors.SimpleButton btnDeletRow2;
    private DevExpress.XtraEditors.SimpleButton btnAddRow2;
    private PanelWyn panelWyn7;
    private PanelWyn panelWyn8;
    private PanelWyn panelWyn9;
    private TabControlWyn tabControlWyn1;
    private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
    private PanelWyn panelWyn10;
    private PanelWyn panelWyn11;
    private PanelWyn panelWyn12;
    private PanelWyn panelWyn13;
    private PanelWyn panelWyn14;
    private DevExpress.XtraEditors.SimpleButton simpleButton1;
    private DevExpress.XtraEditors.SimpleButton simpleButton2;
    private SectionHeaderWyn sectionHeaderWyn5;
    private PanelWyn panelWyn15;
    private PanelWyn panelWyn16;
    private PanelWyn panelWyn17;
    private SectionHeaderWyn sectionHeaderWyn6;
    private SplitterWyn splitterWyn2;
    private PanelWyn panelWyn18;
    private PanelWyn panelWyn19;
    private SectionHeaderWyn sectionHeaderWyn7;
    private PanelWyn panelWyn20;
    private TextEditWyn txtUserGrpIdQ;
    private DevExpress.XtraEditors.LabelControl labelControl26;
    private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
    private GridControlWyn grd1;
    private DevExpress.XtraGrid.Views.Grid.GridView gvw1;
    private GridControlWyn grd2;
    private DevExpress.XtraGrid.Views.Grid.GridView gvw2;
    private DevExpress.XtraEditors.LabelControl labelControl2;
    private DevExpress.XtraEditors.LabelControl labelControl4;
    private TextEditWyn txtuser_nm;
    private TextEditWyn txtuser_id;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
    private DevExpress.XtraEditors.LabelControl labelControl6;
    private DevExpress.XtraEditors.LabelControl labelControl3;
    private DevExpress.XtraEditors.LabelControl labelControl5;
    private TextEditWyn txtuser_grp_nm;
    private TextEditWyn txtuser_grp_cd;
    private GridControlWyn grd3;
    private DevExpress.XtraGrid.Views.Grid.GridView gvw3;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
    private CheckBoxWyn checkBoxWyn1;
    private TreeListWyn tree1;
    private GridControlWyn grd4;
    private DevExpress.XtraGrid.Views.Grid.GridView gvw4;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit2;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
    private PanelWyn panelWyn21;
    private SplitterWyn splitterWyn4;
    private PanelWyn panelWyn22;
    private PanelWyn panelWyn23;
    private PanelWyn panelWyn24;
    private DevExpress.XtraEditors.SimpleButton simpleButton3;
    private DevExpress.XtraEditors.SimpleButton simpleButton4;
    private SectionHeaderWyn sectionHeaderWyn8;
    private MemoEditWyn memodescription;
    private PanelWyn panelWyn25;
    private PanelWyn panelWyn27;
    private PanelWyn panelWyn28;
    private DevExpress.XtraEditors.SimpleButton simpleButton5;
    private DevExpress.XtraEditors.SimpleButton simpleButton6;
    private SectionHeaderWyn sectionHeaderWyn9;
    private SplitterWyn splitterWyn3;
    private PanelWyn panelWyn26;
    private DevExpress.XtraTreeList.Columns.TreeListColumn colMenuCd;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn3;
    private DevExpress.XtraTreeList.Columns.TreeListColumn colMenuType;
    private DevExpress.XtraTreeList.Columns.TreeListColumn colViewYn;
    private DevExpress.XtraTreeList.Columns.TreeListColumn colInsertYn;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit3;
    private DevExpress.XtraTreeList.Columns.TreeListColumn colUpdateYn;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit4;
    private DevExpress.XtraTreeList.Columns.TreeListColumn colDeleteYn;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit5;
    private TreeListWyn tree2;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn4;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn5;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn6;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn7;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn8;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit6;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn9;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit7;
    private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn10;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit8;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit9;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit10;
}
