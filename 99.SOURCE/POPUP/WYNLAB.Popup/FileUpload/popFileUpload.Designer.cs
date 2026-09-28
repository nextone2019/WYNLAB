// VS 디자이너가 자동 생성하는 필드 선언에는 = null!을 안 붙여서 nullable 경고(CS8618)가
// 계속 나기 때문에, 이 파일(디자이너 전용)만 nullable 검사를 끈다 - 흔한 관례.
#nullable disable
using DevExpress.XtraGrid;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.Popup;

/// <summary>
/// 초기 레이아웃 - 파일업로드 공통 팝업 목업(대상 표시줄 / 드래그앤드롭 영역 / 재시도·다운로드·삭제
/// 툴바 / 파일목록 그리드 / 하단 요약+닫기)을 그대로 컨트롤로 배치만 해둔 상태다. 위치·크기는
/// 사장님이 VS 디자이너에서 직접 다시 잡을 예정이라 대략적인 값만 채워뒀고, 버튼 Click 등
/// 업무로직은 전혀 연결하지 않았다(2026-09-06 요청 - "폼생성 해주면 컨트롤배치 다시 해서 디자인
/// 바꿔볼게. 그 후에 다시 스크립트 입혀보자").
/// </summary>
public partial class popFileUpload
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
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblTarget;
    private PanelWyn panToolbar;
    private ButtonWyn btnRetry;
    private ButtonWyn btnDownload;
    private ButtonWyn btnDelete;
    private ButtonWyn btnSelectFile;
    private PanelWyn panSummary;
    private DevExpress.XtraEditors.LabelControl lblSummary;
    private DevExpress.XtraEditors.ProgressBarControl progressUpload;
    private PanelWyn panFooter;

    private void InitializeComponent()
    {
            this.panSummary = new WYNLAB.Base.Controls.PanelWyn();
            this.progressUpload = new DevExpress.XtraEditors.ProgressBarControl();
            this.lblSummary = new DevExpress.XtraEditors.LabelControl();
            this.panFooter = new WYNLAB.Base.Controls.PanelWyn();
            this.btnClose = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnSelectFile = new WYNLAB.Base.Controls.ButtonWyn();
            this.panToolbar = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDelete = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnDownload = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnRetry = new WYNLAB.Base.Controls.ButtonWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.lblTarget = new DevExpress.XtraEditors.LabelControl();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn19 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn17 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn18 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panPreview = new WYNLAB.Base.Controls.PanelWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panSummary)).BeginInit();
            this.panSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressUpload.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panFooter)).BeginInit();
            this.panFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panToolbar)).BeginInit();
            this.panToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // panSummary
            // 
            this.panSummary.Appearance.BackColor = System.Drawing.Color.White;
            this.panSummary.Appearance.Options.UseBackColor = true;
            this.panSummary.Controls.Add(this.progressUpload);
            this.panSummary.Controls.Add(this.lblSummary);
            this.panSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panSummary.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panSummary.Location = new System.Drawing.Point(0, 521);
            this.panSummary.Name = "panSummary";
            this.panSummary.Size = new System.Drawing.Size(1116, 26);
            this.panSummary.TabIndex = 5;
            // 
            // progressUpload
            // 
            this.progressUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressUpload.Location = new System.Drawing.Point(816, 4);
            this.progressUpload.Name = "progressUpload";
            this.progressUpload.Size = new System.Drawing.Size(288, 18);
            this.progressUpload.TabIndex = 1;
            this.progressUpload.Visible = false;
            // 
            // lblSummary
            // 
            this.lblSummary.Location = new System.Drawing.Point(12, 6);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(31, 14);
            this.lblSummary.TabIndex = 0;
            this.lblSummary.Text = "총 0건";
            // 
            // panFooter
            // 
            this.panFooter.Appearance.BackColor = System.Drawing.Color.White;
            this.panFooter.Appearance.Options.UseBackColor = true;
            this.panFooter.Controls.Add(this.btnClose);
            this.panFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panFooter.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panFooter.Location = new System.Drawing.Point(0, 547);
            this.panFooter.Name = "panFooter";
            this.panFooter.Size = new System.Drawing.Size(1116, 48);
            this.panFooter.TabIndex = 6;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.FillColor = System.Drawing.Color.White;
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnClose.Image = null;
            this.btnClose.Location = new System.Drawing.Point(1027, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnClose.Size = new System.Drawing.Size(86, 40);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.ToolTip = null;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.BackColor = System.Drawing.Color.Transparent;
            this.btnSelectFile.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnSelectFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectFile.FillColor = System.Drawing.Color.White;
            this.btnSelectFile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnSelectFile.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnSelectFile.Image = null;
            this.btnSelectFile.Location = new System.Drawing.Point(5, 5);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnSelectFile.Size = new System.Drawing.Size(100, 28);
            this.btnSelectFile.TabIndex = 1;
            this.btnSelectFile.Text = "파일 선택";
            this.btnSelectFile.ToolTip = null;
            // 
            // panToolbar
            // 
            this.panToolbar.Appearance.BackColor = System.Drawing.Color.White;
            this.panToolbar.Appearance.Options.UseBackColor = true;
            this.panToolbar.Controls.Add(this.btnSelectFile);
            this.panToolbar.Controls.Add(this.btnDelete);
            this.panToolbar.Controls.Add(this.btnDownload);
            this.panToolbar.Controls.Add(this.btnRetry);
            this.panToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panToolbar.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panToolbar.Location = new System.Drawing.Point(0, 40);
            this.panToolbar.Name = "panToolbar";
            this.panToolbar.Size = new System.Drawing.Size(1116, 40);
            this.panToolbar.TabIndex = 2;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.FillColor = System.Drawing.Color.White;
            this.btnDelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDelete.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDelete.Image = null;
            this.btnDelete.Location = new System.Drawing.Point(287, 5);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDelete.Size = new System.Drawing.Size(80, 28);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "삭제";
            this.btnDelete.ToolTip = null;
            // 
            // btnDownload
            // 
            this.btnDownload.BackColor = System.Drawing.Color.Transparent;
            this.btnDownload.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnDownload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDownload.FillColor = System.Drawing.Color.White;
            this.btnDownload.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDownload.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDownload.Image = null;
            this.btnDownload.Location = new System.Drawing.Point(199, 5);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDownload.Size = new System.Drawing.Size(80, 28);
            this.btnDownload.TabIndex = 1;
            this.btnDownload.Text = "다운로드";
            this.btnDownload.ToolTip = null;
            // 
            // btnRetry
            // 
            this.btnRetry.BackColor = System.Drawing.Color.Transparent;
            this.btnRetry.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnRetry.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRetry.FillColor = System.Drawing.Color.White;
            this.btnRetry.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnRetry.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnRetry.Image = null;
            this.btnRetry.Location = new System.Drawing.Point(111, 5);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnRetry.Size = new System.Drawing.Size(80, 28);
            this.btnRetry.TabIndex = 0;
            this.btnRetry.Text = "재시도";
            this.btnRetry.ToolTip = null;
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.White;
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.Controls.Add(this.lblTarget);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(0, 0);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1116, 40);
            this.panHeader.TabIndex = 1;
            // 
            // lblTarget
            // 
            this.lblTarget.Location = new System.Drawing.Point(12, 12);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(32, 14);
            this.lblTarget.TabIndex = 0;
            this.lblTarget.Text = "대상 : ";
            // 
            // panelWyn1
            // 
            this.panelWyn1.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn1.Appearance.Options.UseBackColor = true;
            this.panelWyn1.Controls.Add(this.grd1);
            this.panelWyn1.Controls.Add(this.panPreview);
            this.panelWyn1.Controls.Add(this.panSummary);
            this.panelWyn1.Controls.Add(this.panFooter);
            this.panelWyn1.Controls.Add(this.panToolbar);
            this.panelWyn1.Controls.Add(this.panHeader);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(0, 0);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Size = new System.Drawing.Size(1116, 595);
            this.panelWyn1.TabIndex = 1;
            // 
            // grd1
            // 
            this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd1.Location = new System.Drawing.Point(0, 80);
            this.grd1.MainView = this.gvw1;
            this.grd1.Name = "grd1";
            this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.grd1.Size = new System.Drawing.Size(527, 441);
            this.grd1.TabIndex = 7;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn19,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn13,
            this.gridColumn14,
            this.gridColumn15,
            this.gridColumn16,
            this.gridColumn17,
            this.gridColumn18});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn19
            // 
            this.gridColumn19.Caption = " ";
            this.gridColumn19.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gridColumn19.FieldName = "IsChecked";
            this.gridColumn19.Name = "gridColumn19";
            this.gridColumn19.Visible = true;
            this.gridColumn19.VisibleIndex = 0;
            this.gridColumn19.Width = 40;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "FILE ID";
            this.gridColumn1.FieldName = "FileId";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "문서구분";
            this.gridColumn2.FieldName = "DocType";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 91;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "문서ID";
            this.gridColumn3.FieldName = "DocId";
            this.gridColumn3.Name = "gridColumn3";
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "문서번호";
            this.gridColumn4.FieldName = "DocNo";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "문서순번";
            this.gridColumn5.FieldName = "DocSerl";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 3;
            this.gridColumn5.Width = 53;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "FILE순번";
            this.gridColumn6.FieldName = "Serl";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 4;
            this.gridColumn6.Width = 57;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "첨부구분";
            this.gridColumn7.FieldName = "FileType";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 5;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "FILE NAME";
            this.gridColumn8.FieldName = "FileNm";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 6;
            this.gridColumn8.Width = 281;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "FILE SIZE";
            this.gridColumn9.FieldName = "FileSize";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 7;
            this.gridColumn9.Width = 104;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "MimeType";
            this.gridColumn10.FieldName = "MimeType";
            this.gridColumn10.Name = "gridColumn10";
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "FILE저장경로";
            this.gridColumn11.FieldName = "FilePath";
            this.gridColumn11.Name = "gridColumn11";
            // 
            // gridColumn12
            // 
            this.gridColumn12.Caption = "저장위치구분";
            this.gridColumn12.FieldName = "StorageType";
            this.gridColumn12.Name = "gridColumn12";
            // 
            // gridColumn13
            // 
            this.gridColumn13.Caption = "FORM";
            this.gridColumn13.FieldName = "FormId";
            this.gridColumn13.Name = "gridColumn13";
            // 
            // gridColumn14
            // 
            this.gridColumn14.Caption = "비고";
            this.gridColumn14.FieldName = "Remark";
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 9;
            this.gridColumn14.Width = 227;
            // 
            // gridColumn15
            // 
            this.gridColumn15.Caption = "RegUserId";
            this.gridColumn15.FieldName = "RegUserId";
            this.gridColumn15.Name = "gridColumn15";
            // 
            // gridColumn16
            // 
            this.gridColumn16.Caption = "등록자명";
            this.gridColumn16.FieldName = "RegUserNm";
            this.gridColumn16.Name = "gridColumn16";
            // 
            // gridColumn17
            // 
            this.gridColumn17.Caption = "실패여부";
            this.gridColumn17.FieldName = "FailYn";
            this.gridColumn17.Name = "gridColumn17";
            this.gridColumn17.Visible = true;
            this.gridColumn17.VisibleIndex = 8;
            // 
            // gridColumn18
            // 
            this.gridColumn18.Caption = "재시도횟수";
            this.gridColumn18.FieldName = "RetryCnt";
            this.gridColumn18.Name = "gridColumn18";
            // 
            // panPreview
            // 
            this.panPreview.Appearance.BackColor = System.Drawing.Color.White;
            this.panPreview.Appearance.Options.UseBackColor = true;
            this.panPreview.Dock = System.Windows.Forms.DockStyle.Right;
            this.panPreview.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panPreview.Location = new System.Drawing.Point(527, 80);
            this.panPreview.Name = "panPreview";
            this.panPreview.Size = new System.Drawing.Size(589, 441);
            this.panPreview.TabIndex = 8;
            // 
            // popFileUpload
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.ClientSize = new System.Drawing.Size(1116, 595);
            this.Controls.Add(this.panelWyn1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IconOptions.ShowIcon = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "popFileUpload";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "첨부파일관리 [popFileUpload]";
            ((System.ComponentModel.ISupportInitialize)(this.panSummary)).EndInit();
            this.panSummary.ResumeLayout(false);
            this.panSummary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressUpload.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panFooter)).EndInit();
            this.panFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panToolbar)).EndInit();
            this.panToolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panPreview)).EndInit();
            this.ResumeLayout(false);

    }

    private PanelWyn panelWyn1;
    private ButtonWyn btnClose;
    private GridControlWyn grd1;
    private DevExpress.XtraGrid.Views.Grid.GridView gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn15;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn16;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn17;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn18;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn19;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
    private PanelWyn panPreview;
}
