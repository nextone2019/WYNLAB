// @AI_BUILDER:BEGIN FILE_HEADER
// AI Builder 마스터-서브그리드 템플릿 원본 - grd1(마스터, 인라인편집) 선택에 따라 grd2(서브,
// 조회전용)가 재조회된다. VS 디자이너로 열어 제목영역/여백/색상을 다듬으면 AI Builder가
// 만드는 모든 마스터-서브그리드 화면에 그대로 반영된다. "// @AI_BUILDER:BEGIN~END" 구간의
// 예시 컨트롤(txtSample1, colM1 등)은 실제 화면에는 생성되지 않는다.
// @AI_BUILDER:END FILE_HEADER
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.TEMPLATE;

public partial class TplMasterSubGrid
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
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        // @AI_BUILDER:BEGIN SEARCH_FIELD_NEW
        this.txtSample1 = new WYNLAB.Base.Controls.TextEditWyn();
        // @AI_BUILDER:END SEARCH_FIELD_NEW
        this.btnQuery = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnSave = new WYNLAB.Base.Controls.ButtonWyn();
        this.panelSplit = new WYNLAB.Base.Controls.PanelWyn();
        this.panelLeft = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderMaster = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        // @AI_BUILDER:BEGIN MASTER_COLUMN_NEW
        this.colM1 = new DevExpress.XtraGrid.Columns.GridColumn();
        // @AI_BUILDER:END MASTER_COLUMN_NEW
        this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
        this.panelRight = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderSub = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
        // @AI_BUILDER:BEGIN DETAIL_COLUMN_NEW
        this.colD1 = new DevExpress.XtraGrid.Columns.GridColumn();
        // @AI_BUILDER:END DETAIL_COLUMN_NEW

        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        this.panBase.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panelSplit)).BeginInit();
        this.panelSplit.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panelLeft)).BeginInit();
        this.panelLeft.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelRight)).BeginInit();
        this.panelRight.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
        this.SuspendLayout();
        //
        // panHeader
        //
        // @AI_BUILDER:BEGIN SEARCH_FIELD_CONFIG
        this.txtSample1.Location = new System.Drawing.Point(16, 20);
        this.txtSample1.Name = "txtSample1";
        this.txtSample1.Size = new System.Drawing.Size(150, 20);
        this.panHeader.Controls.Add(this.txtSample1);
        // @AI_BUILDER:END SEARCH_FIELD_CONFIG
        this.btnQuery.Location = new System.Drawing.Point(1055, 18);
        this.btnQuery.Name = "btnQuery";
        this.btnQuery.Size = new System.Drawing.Size(80, 24);
        this.btnQuery.Text = "조회";
        this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnSave.Location = new System.Drawing.Point(1145, 18);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new System.Drawing.Size(80, 24);
        this.btnSave.Text = "저장";
        this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.panHeader.Controls.Add(this.btnQuery);
        this.panHeader.Controls.Add(this.btnSave);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.Height = 60;
        this.panHeader.Location = new System.Drawing.Point(0, 0);
        this.panHeader.Name = "panHeader";
        this.panHeader.Size = new System.Drawing.Size(1245, 60);
        this.panHeader.TabIndex = 0;
        //
        // sectionHeaderMaster
        //
        this.sectionHeaderMaster.Dock = System.Windows.Forms.DockStyle.Top;
        this.sectionHeaderMaster.Height = 28;
        this.sectionHeaderMaster.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.sectionHeaderMaster.Location = new System.Drawing.Point(0, 0);
        this.sectionHeaderMaster.Name = "sectionHeaderMaster";
        this.sectionHeaderMaster.Text = "__MENU_CAPTION__";
        //
        // gvw1
        //
        // @AI_BUILDER:BEGIN MASTER_COLUMN_CONFIG
        this.colM1.Caption = "예시컬럼";
        this.colM1.FieldName = "sample1";
        this.colM1.Name = "colM1";
        this.colM1.Visible = true;
        this.colM1.VisibleIndex = 0;
        this.colM1.Width = 100;
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colM1});
        // @AI_BUILDER:END MASTER_COLUMN_CONFIG
        this.gvw1.GridControl = this.grd1;
        this.gvw1.Name = "gvw1";
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd1.Location = new System.Drawing.Point(0, 28);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.Size = new System.Drawing.Size(500, 492);
        this.grd1.TabIndex = 1;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { this.gvw1});
        //
        // panelLeft
        //
        this.panelLeft.Controls.Add(this.grd1);
        this.panelLeft.Controls.Add(this.sectionHeaderMaster);
        this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
        this.panelLeft.Location = new System.Drawing.Point(0, 0);
        this.panelLeft.Name = "panelLeft";
        this.panelLeft.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
        this.panelLeft.Size = new System.Drawing.Size(500, 520);
        this.panelLeft.TabIndex = 0;
        this.panelLeft.Width = 500;
        //
        // splitterWyn1
        //
        this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Left;
        this.splitterWyn1.Location = new System.Drawing.Point(500, 0);
        this.splitterWyn1.Name = "splitterWyn1";
        this.splitterWyn1.Size = new System.Drawing.Size(1, 520);
        this.splitterWyn1.TabIndex = 1;
        this.splitterWyn1.TabStop = false;
        //
        // sectionHeaderSub
        //
        this.sectionHeaderSub.Dock = System.Windows.Forms.DockStyle.Top;
        this.sectionHeaderSub.Height = 28;
        this.sectionHeaderSub.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.sectionHeaderSub.Location = new System.Drawing.Point(0, 0);
        this.sectionHeaderSub.Name = "sectionHeaderSub";
        this.sectionHeaderSub.Text = "상세";
        //
        // gvw2
        //
        // @AI_BUILDER:BEGIN DETAIL_COLUMN_CONFIG
        this.colD1.Caption = "예시컬럼";
        this.colD1.FieldName = "sample1";
        this.colD1.Name = "colD1";
        this.colD1.Visible = true;
        this.colD1.VisibleIndex = 0;
        this.colD1.Width = 100;
        this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colD1});
        // @AI_BUILDER:END DETAIL_COLUMN_CONFIG
        this.gvw2.GridControl = this.grd2;
        this.gvw2.Name = "gvw2";
        //
        // grd2
        //
        this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd2.Location = new System.Drawing.Point(0, 28);
        this.grd2.MainView = this.gvw2;
        this.grd2.Name = "grd2";
        this.grd2.Size = new System.Drawing.Size(744, 492);
        this.grd2.TabIndex = 1;
        this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { this.gvw2});
        //
        // panelRight
        //
        this.panelRight.Controls.Add(this.grd2);
        this.panelRight.Controls.Add(this.sectionHeaderSub);
        this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelRight.Location = new System.Drawing.Point(501, 0);
        this.panelRight.Name = "panelRight";
        this.panelRight.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
        this.panelRight.Size = new System.Drawing.Size(744, 520);
        this.panelRight.TabIndex = 2;
        //
        // panelSplit
        //
        this.panelSplit.Controls.Add(this.panelRight);
        this.panelSplit.Controls.Add(this.splitterWyn1);
        this.panelSplit.Controls.Add(this.panelLeft);
        this.panelSplit.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelSplit.Location = new System.Drawing.Point(0, 60);
        this.panelSplit.Name = "panelSplit";
        this.panelSplit.Size = new System.Drawing.Size(1245, 520);
        this.panelSplit.TabIndex = 1;
        //
        // panBase
        //
        this.panBase.Controls.Add(this.panelSplit);
        this.panBase.Controls.Add(this.panHeader);
        this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panBase.Location = new System.Drawing.Point(0, 0);
        this.panBase.Name = "panBase";
        this.panBase.Size = new System.Drawing.Size(1245, 580);
        this.panBase.TabIndex = 0;
        //
        // TplMasterSubGrid
        //
        this.ClientSize = new System.Drawing.Size(1245, 580);
        this.Controls.Add(this.panBase);
        this.Name = "TplMasterSubGrid";

        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        this.panHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panelSplit)).EndInit();
        this.panelSplit.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panelLeft)).EndInit();
        this.panelLeft.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelRight)).EndInit();
        this.panelRight.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panHeader;
    // @AI_BUILDER:BEGIN SEARCH_FIELD_DECL
    private TextEditWyn txtSample1;
    // @AI_BUILDER:END SEARCH_FIELD_DECL
    private ButtonWyn btnQuery;
    private ButtonWyn btnSave;
    private PanelWyn panelSplit;
    private PanelWyn panelLeft;
    private SectionHeaderWyn sectionHeaderMaster;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    // @AI_BUILDER:BEGIN MASTER_COLUMN_DECL
    private DevExpress.XtraGrid.Columns.GridColumn colM1;
    // @AI_BUILDER:END MASTER_COLUMN_DECL
    private SplitterWyn splitterWyn1;
    private PanelWyn panelRight;
    private SectionHeaderWyn sectionHeaderSub;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    // @AI_BUILDER:BEGIN DETAIL_COLUMN_DECL
    private DevExpress.XtraGrid.Columns.GridColumn colD1;
    // @AI_BUILDER:END DETAIL_COLUMN_DECL
}
