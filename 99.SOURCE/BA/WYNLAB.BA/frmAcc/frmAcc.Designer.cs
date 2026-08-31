// VS 디자이너가 자동 생성하는 필드 선언에는 = null!을 안 붙여서 nullable 경고(CS8618)가
// 계속 나기 때문에, 이 파일(디자이너 전용)만 nullable 검사를 끈다 - 흔한 관례.
#nullable disable
using DevExpress.XtraGrid;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

/// <summary>
/// 사업장등록 화면 - TEMPLATE(frmMinorCode 표준 레이아웃)에서 하위목록(grd2) 없이 grd1(목록) +
/// panData(상세 등록)만 남긴 버전. 자세한 컬럼정의는 나중에 추가 예정이라 지금은 사업장코드/
/// 사업장명 2개 필드만 배치한다 - 필드를 더 늘릴 때는 panData에 라벨+입력컨트롤 쌍을 그대로
/// 이어 붙이면 된다(다음 Y좌표는 기존 필드보다 30만큼 아래).
/// </summary>
public partial class frmAcc
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
        this.panBase = new WYNLAB.Base.Controls.PanelWyn();
        this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
        this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
        this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
        this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
        this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
        this.panData = new WYNLAB.Base.Controls.PanelWyn();
        this.lblAccCd = new DevExpress.XtraEditors.LabelControl();
        this.txtAccCd = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblAccNm = new DevExpress.XtraEditors.LabelControl();
        this.txtAccNm = new WYNLAB.Base.Controls.TextEditWyn();
        this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.txtSearchQ = new WYNLAB.Base.Controls.TextEditWyn();
        this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
        this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        this.panBase.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
        this.panelWyn3.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).BeginInit();
        this.panelWyn8.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
        this.panelWyn2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
        this.panelWyn5.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
        this.panData.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtAccCd.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtAccNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
        this.panelWyn6.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchQ.Properties)).BeginInit();
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
        this.panelWyn3.Controls.Add(this.panelWyn5);
        this.panelWyn3.Controls.Add(this.splitterWyn1);
        this.panelWyn3.Controls.Add(this.panelWyn8);
        this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panelWyn3.Location = new System.Drawing.Point(3, 82);
        this.panelWyn3.Name = "panelWyn3";
        this.panelWyn3.Size = new System.Drawing.Size(1159, 515);
        this.panelWyn3.TabIndex = 7;
        //
        // panelWyn8 (좌측 - 목록)
        //
        this.panelWyn8.Controls.Add(this.grd1);
        this.panelWyn8.Controls.Add(this.panelWyn2);
        this.panelWyn8.Dock = System.Windows.Forms.DockStyle.Left;
        this.panelWyn8.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panelWyn8.Location = new System.Drawing.Point(0, 0);
        this.panelWyn8.Name = "panelWyn8";
        this.panelWyn8.Size = new System.Drawing.Size(402, 515);
        this.panelWyn8.TabIndex = 12;
        //
        // grd1 (목록)
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd1.Location = new System.Drawing.Point(0, 27);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.Size = new System.Drawing.Size(402, 488);
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
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsSelection.InvertSelection = true;
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        //
        // gridColumn1 (사업장코드)
        //
        this.gridColumn1.Caption = "사업장코드";
        this.gridColumn1.FieldName = "acc_cd";
        this.gridColumn1.Name = "gridColumn1";
        this.gridColumn1.Visible = true;
        this.gridColumn1.VisibleIndex = 0;
        this.gridColumn1.Width = 100;
        //
        // gridColumn2 (사업장명)
        //
        this.gridColumn2.Caption = "사업장명";
        this.gridColumn2.FieldName = "acc_nm";
        this.gridColumn2.Name = "gridColumn2";
        this.gridColumn2.Visible = true;
        this.gridColumn2.VisibleIndex = 1;
        this.gridColumn2.Width = 250;
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
        this.sectionHeaderWyn4.TabIndex = 8;
        this.sectionHeaderWyn4.Text = "목록";
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
        // panelWyn5 (우측 - 상세 등록)
        //
        this.panelWyn5.Controls.Add(this.panData);
        this.panelWyn5.Controls.Add(this.panelWyn6);
        this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panelWyn5.Location = new System.Drawing.Point(412, 0);
        this.panelWyn5.Name = "panelWyn5";
        this.panelWyn5.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
        this.panelWyn5.Size = new System.Drawing.Size(747, 515);
        this.panelWyn5.TabIndex = 6;
        //
        // panData (상세 입력)
        //
        this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        this.panData.Controls.Add(this.lblAccCd);
        this.panData.Controls.Add(this.txtAccCd);
        this.panData.Controls.Add(this.lblAccNm);
        this.panData.Controls.Add(this.txtAccNm);
        this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panData.Location = new System.Drawing.Point(3, 27);
        this.panData.Name = "panData";
        this.panData.Size = new System.Drawing.Size(744, 488);
        this.panData.TabIndex = 8;
        //
        // lblAccCd
        //
        this.lblAccCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblAccCd.Appearance.Options.UseFont = true;
        this.lblAccCd.Location = new System.Drawing.Point(11, 20);
        this.lblAccCd.Name = "lblAccCd";
        this.lblAccCd.Size = new System.Drawing.Size(60, 15);
        this.lblAccCd.TabIndex = 0;
        this.lblAccCd.Text = "사업장코드";
        //
        // txtAccCd
        //
        this.txtAccCd.Location = new System.Drawing.Point(110, 18);
        this.txtAccCd.Name = "txtAccCd";
        this.txtAccCd.Required = true;
        this.txtAccCd.Size = new System.Drawing.Size(150, 20);
        this.txtAccCd.TabIndex = 1;
        //
        // lblAccNm
        //
        this.lblAccNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblAccNm.Appearance.Options.UseFont = true;
        this.lblAccNm.Location = new System.Drawing.Point(11, 50);
        this.lblAccNm.Name = "lblAccNm";
        this.lblAccNm.Size = new System.Drawing.Size(60, 15);
        this.lblAccNm.TabIndex = 2;
        this.lblAccNm.Text = "사업장명";
        //
        // txtAccNm
        //
        this.txtAccNm.Location = new System.Drawing.Point(110, 48);
        this.txtAccNm.Name = "txtAccNm";
        this.txtAccNm.Required = true;
        this.txtAccNm.Size = new System.Drawing.Size(300, 20);
        this.txtAccNm.TabIndex = 3;
        //
        // panelWyn6
        //
        this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
        this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panelWyn6.Location = new System.Drawing.Point(3, 0);
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
        this.sectionHeaderWyn3.TabIndex = 8;
        this.sectionHeaderWyn3.Text = "상세 등록";
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        this.panHeader.Controls.Add(this.txtSearchQ);
        this.panHeader.Controls.Add(this.labelControl1);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panHeader.Location = new System.Drawing.Point(3, 33);
        this.panHeader.Name = "panHeader";
        this.panHeader.Size = new System.Drawing.Size(1159, 49);
        this.panHeader.TabIndex = 8;
        //
        // txtSearchQ
        //
        this.txtSearchQ.Location = new System.Drawing.Point(106, 15);
        this.txtSearchQ.Name = "txtSearchQ";
        this.txtSearchQ.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
        this.txtSearchQ.Size = new System.Drawing.Size(265, 20);
        this.txtSearchQ.TabIndex = 0;
        //
        // labelControl1
        //
        this.labelControl1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.labelControl1.Appearance.Options.UseFont = true;
        this.labelControl1.Location = new System.Drawing.Point(25, 18);
        this.labelControl1.Name = "labelControl1";
        this.labelControl1.Size = new System.Drawing.Size(77, 15);
        this.labelControl1.TabIndex = 0;
        this.labelControl1.Text = "검색조건";
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
        this.sectionHeaderWyn1.TabIndex = 8;
        this.sectionHeaderWyn1.Text = "사업장등록";
        //
        // frmAcc
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1165, 600);
        this.Controls.Add(this.panBase);
        this.Name = "frmAcc";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
        this.panelWyn3.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).EndInit();
        this.panelWyn8.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
        this.panelWyn2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
        this.panelWyn5.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
        this.panData.ResumeLayout(false);
        this.panData.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtAccCd.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtAccNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
        this.panelWyn6.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        this.panHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchQ.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
        this.paTitle.ResumeLayout(false);
        this.ResumeLayout(false);

    }

    private PanelWyn paTitle;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private PanelWyn panelWyn6;
    private SectionHeaderWyn sectionHeaderWyn1;
    private SplitterWyn splitterWyn1;
    private SectionHeaderWyn sectionHeaderWyn3;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
    private PanelWyn panelWyn2;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn panHeader;
    private TextEditWyn txtSearchQ;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private PanelWyn panelWyn8;
    private DevExpress.XtraEditors.LabelControl lblAccCd;
    private TextEditWyn txtAccCd;
    private DevExpress.XtraEditors.LabelControl lblAccNm;
    private TextEditWyn txtAccNm;
}
