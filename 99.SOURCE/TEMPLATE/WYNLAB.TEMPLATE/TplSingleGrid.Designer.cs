// @AI_BUILDER:BEGIN FILE_HEADER
// AI Builder 싱글그리드 템플릿 원본 - 이 파일을 VS 디자이너로 열어 제목영역/여백/색상 등을
// 직접 다듬으면, 그 디자인이 AI Builder가 만드는 모든 싱글그리드 화면에 그대로 반영된다.
//
// "// @AI_BUILDER:BEGIN 이름" ~ "// @AI_BUILDER:END 이름" 사이는 생성기가 실제 검색조건/
// 그리드컬럼으로 통째로 갈아끼우는 구간이다 - 지금 들어있는 내용(txtSample1, colSample1 등)은
// "이 자리에 이런 게 들어간다"를 보여주는 예시일 뿐, 실제로 화면에 생성되지는 않는다. 이 구간
// 밖의 코드(패널 배치, 헤더 스타일, 버튼 위치 등)는 그대로 복제되어 실제 화면에 남는다.
//
// 주의: VS 디자이너로 이 파일을 열어 저장하면, 디자이너가 마커 주석("// @AI_BUILDER:...")과
// panBase 등 이 파일 특유의 구조를 못 알아보고 재직렬화하면서 통째로 망가뜨릴 수 있다(실제로
// 겪음, 2026-09-04 - panBase/panHeader가 통째로 사라지고 정체불명의 gridViewWyn1/2가 생김).
// 디자이너로 컨트롤을 새로 놓고 조정하는 건 괜찮지만, 저장하기 전에 마커 6쌍(SEARCH_FIELD_*,
// GRID_COLUMN_*)과 panBase/panHeader/sectionHeaderGrid/grd1이 그대로 남아있는지 diff로 꼭
// 확인할 것 - 사라졌으면 저장을 취소하고 이 파일을 원본에서 다시 시작하는 게 안전하다.
// @AI_BUILDER:END FILE_HEADER
#nullable disable
using WYNLAB.Base.Controls;

namespace WYNLAB.TEMPLATE;

public partial class TplSingleGrid
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TplSingleGrid));
            this.panBody = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            // @AI_BUILDER:BEGIN GRID_COLUMN_NEW
            this.colSample1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSample2 = new DevExpress.XtraGrid.Columns.GridColumn();
            // @AI_BUILDER:END GRID_COLUMN_NEW
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            // @AI_BUILDER:BEGIN SEARCH_FIELD_NEW
            this.txtSample1 = new WYNLAB.Base.Controls.TextEditWyn();
            // @AI_BUILDER:END SEARCH_FIELD_NEW
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
            // @AI_BUILDER:BEGIN GRID_COLUMN_CONFIG
            this.colSample1.Caption = "샘플1";
            this.colSample1.FieldName = "sample1";
            this.colSample1.Name = "colSample1";
            this.colSample1.Visible = true;
            this.colSample1.VisibleIndex = 0;
            this.colSample1.Width = 100;
            this.colSample2.Caption = "샘플2";
            this.colSample2.FieldName = "sample2";
            this.colSample2.Name = "colSample2";
            this.colSample2.Visible = true;
            this.colSample2.VisibleIndex = 1;
            this.colSample2.Width = 100;
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colSample1,
            this.colSample2});
            // @AI_BUILDER:END GRID_COLUMN_CONFIG
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
            // @AI_BUILDER:BEGIN SEARCH_FIELD_CONFIG
            this.txtSample1.Location = new System.Drawing.Point(16, 20);
            this.txtSample1.Name = "txtSample1";
            this.txtSample1.Size = new System.Drawing.Size(150, 20);
            this.panHeader.Controls.Add(this.txtSample1);
            // @AI_BUILDER:END SEARCH_FIELD_CONFIG
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
            // TplSingleGrid
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1245, 580);
            this.Controls.Add(this.panBody);
            this.Controls.Add(this.panHeader);
            this.Controls.Add(this.paTitleH);
            this.Name = "TplSingleGrid";
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
    // @AI_BUILDER:BEGIN SEARCH_FIELD_DECL
    private TextEditWyn txtSample1;
    // @AI_BUILDER:END SEARCH_FIELD_DECL
    // @AI_BUILDER:BEGIN GRID_COLUMN_DECL
    private DevExpress.XtraGrid.Columns.GridColumn colSample1;
    private DevExpress.XtraGrid.Columns.GridColumn colSample2;
    // @AI_BUILDER:END GRID_COLUMN_DECL
}
