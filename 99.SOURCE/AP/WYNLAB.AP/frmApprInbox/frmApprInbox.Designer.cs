// VS 디자이너가 자동 생성하는 필드 선언에는 = null!을 안 붙여서 nullable 경고(CS8618)가
// 계속 나기 때문에, 이 파일(디자이너 전용)만 nullable 검사를 끈다 - 흔한 관례.
#nullable disable
using DevExpress.XtraGrid;
using WYNLAB.Base.Controls;

namespace WYNLAB.AP;

/// <summary>
/// 결재함 화면 - 내가 처리해야 할 결재 대기건 목록(grd1) + 하단 의견입력/승인/반려(panBottom).
/// frmAcc(grd1+panData 표준 레이아웃)에서 우측 상세패널 대신 하단 액션패널로 바꾼 변형.
/// </summary>
public partial class frmApprInbox
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
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.colAppNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colAppTitle = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDocType = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colReqEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colAppDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.panBottom = new WYNLAB.Base.Controls.PanelWyn();
        this.lblOpinion = new DevExpress.XtraEditors.LabelControl();
        this.memoOpinion = new WYNLAB.Base.Controls.MemoEditWyn();
        this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        this.panBase.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
        this.panelWyn3.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panBottom)).BeginInit();
        this.panBottom.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.memoOpinion.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
        this.panelWyn6.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.paTitle)).BeginInit();
        this.paTitle.SuspendLayout();
        this.SuspendLayout();
        //
        // panBase
        //
        this.panBase.Appearance.BackColor = System.Drawing.Color.White;
        this.panBase.Appearance.Options.UseBackColor = true;
        this.panBase.Controls.Add(this.panelWyn3);
        this.panBase.Controls.Add(this.paTitle);
        this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panBase.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panBase.Location = new System.Drawing.Point(0, 0);
        this.panBase.Name = "panBase";
        this.panBase.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
        this.panBase.Size = new System.Drawing.Size(1100, 600);
        this.panBase.TabIndex = 5;
        //
        // panelWyn3 (본문 - 목록 + 하단 액션패널)
        //
        this.panelWyn3.Controls.Add(this.grd1);
        this.panelWyn3.Controls.Add(this.panBottom);
        this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panelWyn3.Location = new System.Drawing.Point(3, 33);
        this.panelWyn3.Name = "panelWyn3";
        this.panelWyn3.Size = new System.Drawing.Size(1094, 564);
        this.panelWyn3.TabIndex = 7;
        //
        // grd1 (결재 대기 목록)
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd1.Location = new System.Drawing.Point(0, 0);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.Size = new System.Drawing.Size(1094, 444);
        this.grd1.TabIndex = 10;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colAppNo,
        this.colAppTitle,
        this.colDocType,
        this.colReqEmpNm,
        this.colAppDate});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsBehavior.Editable = false;
        this.gvw1.OptionsSelection.InvertSelection = true;
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        //
        // colAppNo (결재번호)
        //
        this.colAppNo.Caption = "결재번호";
        this.colAppNo.FieldName = "app_no";
        this.colAppNo.Name = "colAppNo";
        this.colAppNo.Visible = true;
        this.colAppNo.VisibleIndex = 0;
        this.colAppNo.Width = 100;
        //
        // colAppTitle (제목)
        //
        this.colAppTitle.Caption = "제목";
        this.colAppTitle.FieldName = "app_title";
        this.colAppTitle.Name = "colAppTitle";
        this.colAppTitle.Visible = true;
        this.colAppTitle.VisibleIndex = 1;
        this.colAppTitle.Width = 260;
        //
        // colDocType (문서유형)
        //
        this.colDocType.Caption = "문서유형";
        this.colDocType.FieldName = "doc_type";
        this.colDocType.Name = "colDocType";
        this.colDocType.Visible = true;
        this.colDocType.VisibleIndex = 2;
        this.colDocType.Width = 90;
        //
        // colReqEmpNm (기안자)
        //
        this.colReqEmpNm.Caption = "기안자";
        this.colReqEmpNm.FieldName = "req_emp_nm";
        this.colReqEmpNm.Name = "colReqEmpNm";
        this.colReqEmpNm.Visible = true;
        this.colReqEmpNm.VisibleIndex = 3;
        this.colReqEmpNm.Width = 80;
        //
        // colAppDate (기안일자)
        //
        this.colAppDate.Caption = "기안일자";
        this.colAppDate.FieldName = "app_date";
        this.colAppDate.Name = "colAppDate";
        this.colAppDate.Visible = true;
        this.colAppDate.VisibleIndex = 4;
        this.colAppDate.Width = 80;
        //
        // panBottom (의견입력 + 승인/반려 버튼 - 버튼은 코드에서 추가)
        //
        this.panBottom.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        this.panBottom.Controls.Add(this.lblOpinion);
        this.panBottom.Controls.Add(this.memoOpinion);
        this.panBottom.Controls.Add(this.panelWyn6);
        this.panBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panBottom.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panBottom.Location = new System.Drawing.Point(0, 444);
        this.panBottom.Name = "panBottom";
        this.panBottom.Size = new System.Drawing.Size(1094, 120);
        this.panBottom.TabIndex = 11;
        //
        // panelWyn6
        //
        this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
        this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.panelWyn6.Location = new System.Drawing.Point(0, 0);
        this.panelWyn6.Name = "panelWyn6";
        this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
        this.panelWyn6.Size = new System.Drawing.Size(1094, 27);
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
        this.sectionHeaderWyn3.Size = new System.Drawing.Size(1089, 25);
        this.sectionHeaderWyn3.TabIndex = 8;
        this.sectionHeaderWyn3.Text = "결재처리";
        //
        // lblOpinion
        //
        this.lblOpinion.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblOpinion.Appearance.Options.UseFont = true;
        this.lblOpinion.Location = new System.Drawing.Point(11, 37);
        this.lblOpinion.Name = "lblOpinion";
        this.lblOpinion.Size = new System.Drawing.Size(75, 15);
        this.lblOpinion.TabIndex = 0;
        this.lblOpinion.Text = "결재의견";
        //
        // memoOpinion
        //
        this.memoOpinion.Location = new System.Drawing.Point(95, 35);
        this.memoOpinion.Name = "memoOpinion";
        this.memoOpinion.Size = new System.Drawing.Size(500, 60);
        this.memoOpinion.TabIndex = 1;
        //
        // paTitle
        //
        this.paTitle.Controls.Add(this.sectionHeaderWyn1);
        this.paTitle.Dock = System.Windows.Forms.DockStyle.Top;
        this.paTitle.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
        this.paTitle.Location = new System.Drawing.Point(3, 0);
        this.paTitle.Name = "paTitle";
        this.paTitle.Size = new System.Drawing.Size(1094, 33);
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
        this.sectionHeaderWyn1.Text = "결재함";
        //
        // frmApprInbox
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1100, 600);
        this.Controls.Add(this.panBase);
        this.Name = "frmApprInbox";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
        this.panelWyn3.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panBottom)).EndInit();
        this.panBottom.ResumeLayout(false);
        this.panBottom.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.memoOpinion.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
        this.panelWyn6.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
        this.paTitle.ResumeLayout(false);
        this.ResumeLayout(false);

    }

    private PanelWyn paTitle;
    private PanelWyn panelWyn3;
    private SectionHeaderWyn sectionHeaderWyn1;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colAppNo;
    private DevExpress.XtraGrid.Columns.GridColumn colAppTitle;
    private DevExpress.XtraGrid.Columns.GridColumn colDocType;
    private DevExpress.XtraGrid.Columns.GridColumn colReqEmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn colAppDate;
    private PanelWyn panBottom;
    private DevExpress.XtraEditors.LabelControl lblOpinion;
    private MemoEditWyn memoOpinion;
    private PanelWyn panelWyn6;
    private SectionHeaderWyn sectionHeaderWyn3;
}
