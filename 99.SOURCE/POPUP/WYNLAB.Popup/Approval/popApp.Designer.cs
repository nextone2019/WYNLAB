// VS 디자이너가 자동 생성하는 필드 선언에는 = null!을 안 붙여서 nullable 경고(CS8618)가
// 계속 나기 때문에, 이 파일(디자이너 전용)만 nullable 검사를 끈다 - 흔한 관례.
#nullable disable
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.Popup;

/// <summary>
/// 전자결재 공용 팝업 - 컨트롤 배치는 여기서 담당(사용자가 VS 디자이너로 시각적으로 다듬을 수
/// 있게 frmAcc.Designer.cs와 같은 구조로 분리했다). 로직/이벤트 연결은 popApp.cs.
/// 세로 배치: panHeader(맨 위, 결재기본정보+툴바) - panCompose(작성모드 전용, 부서트리/사원/
/// 결재경로) - panGrids(결재라인/수신라인 그리드, 나머지 공간) - panBottom(맨 아래, 닫기).
/// </summary>
public partial class popApp
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

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(popApp));
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.lblAppNo = new DevExpress.XtraEditors.LabelControl();
            this.txtAppNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblAppDate = new DevExpress.XtraEditors.LabelControl();
            this.lblReqEmpNm = new DevExpress.XtraEditors.LabelControl();
            this.txtReqEmpNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblApprStatCd = new DevExpress.XtraEditors.LabelControl();
            this.lblTitle = new DevExpress.XtraEditors.LabelControl();
            this.txtTitle = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblOpinion = new DevExpress.XtraEditors.LabelControl();
            this.memoOpinion = new WYNLAB.Base.Controls.MemoEditWyn();
            this.btnRefresh = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnSubmit = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnApprove = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnReject = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnCancelApprove = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAck = new WYNLAB.Base.Controls.ButtonWyn();
            this.lblRoute = new DevExpress.XtraEditors.LabelControl();
            this.cboRoute = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnApplyRoute = new WYNLAB.Base.Controls.ButtonWyn();
            this.txtNewRouteNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.btnSaveRoute = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAddLine = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAddRecv = new WYNLAB.Base.Controls.ButtonWyn();
            this.treeEmp = new WYNLAB.Base.Controls.TreeListWyn();
            this.colTreeNm = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colTreeEmpNo = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colTreeJobGrade = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.grdRecv = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvwRecv = new WYNLAB.Base.Controls.GridViewWyn();
            this.colRecvSort = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRecvEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRecvStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRecvAppDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRecvRemark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdLine = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvwLine = new WYNLAB.Base.Controls.GridViewWyn();
            this.colLineSort = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLineEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLineStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLineAppDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLineRemark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnClose = new WYNLAB.Base.Controls.ButtonWyn();
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.ymdAppDate = new WYNLAB.Base.Controls.DateEditWyn();
            this.cboAppStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.panelWyn13 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.splitterWyn2 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn9 = new WYNLAB.Base.Controls.PanelWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn10 = new WYNLAB.Base.Controls.PanelWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAppNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReqEmpNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTitle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoOpinion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboRoute.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewRouteNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeEmp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdRecv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwRecv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).BeginInit();
            this.paTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ymdAppDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ymdAppDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAppStatCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn13)).BeginInit();
            this.panelWyn13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
            this.panelWyn4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
            this.panelWyn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
            this.panelWyn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
            this.panelWyn6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).BeginInit();
            this.panelWyn8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn9)).BeginInit();
            this.panelWyn9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn10)).BeginInit();
            this.panelWyn10.SuspendLayout();
            this.SuspendLayout();
            // 
            // panHeader
            // 
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.cboAppStatCd);
            this.panHeader.Controls.Add(this.ymdAppDate);
            this.panHeader.Controls.Add(this.lblAppNo);
            this.panHeader.Controls.Add(this.txtAppNo);
            this.panHeader.Controls.Add(this.lblAppDate);
            this.panHeader.Controls.Add(this.lblReqEmpNm);
            this.panHeader.Controls.Add(this.txtReqEmpNm);
            this.panHeader.Controls.Add(this.lblApprStatCd);
            this.panHeader.Controls.Add(this.lblTitle);
            this.panHeader.Controls.Add(this.txtTitle);
            this.panHeader.Controls.Add(this.lblOpinion);
            this.panHeader.Controls.Add(this.memoOpinion);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 5);
            this.panHeader.Name = "panHeader";
            this.panHeader.Padding = new System.Windows.Forms.Padding(8);
            this.panHeader.Size = new System.Drawing.Size(974, 141);
            this.panHeader.TabIndex = 0;
            // 
            // lblAppNo
            // 
            this.lblAppNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppNo.Appearance.Options.UseFont = true;
            this.lblAppNo.Location = new System.Drawing.Point(16, 9);
            this.lblAppNo.Name = "lblAppNo";
            this.lblAppNo.Size = new System.Drawing.Size(48, 15);
            this.lblAppNo.TabIndex = 0;
            this.lblAppNo.Text = "결재번호";
            // 
            // txtAppNo
            // 
            this.txtAppNo.Location = new System.Drawing.Point(71, 6);
            this.txtAppNo.Name = "txtAppNo";
            this.txtAppNo.Properties.ReadOnly = true;
            this.txtAppNo.Size = new System.Drawing.Size(121, 20);
            this.txtAppNo.TabIndex = 1;
            // 
            // lblAppDate
            // 
            this.lblAppDate.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppDate.Appearance.Options.UseFont = true;
            this.lblAppDate.Location = new System.Drawing.Point(215, 9);
            this.lblAppDate.Name = "lblAppDate";
            this.lblAppDate.Size = new System.Drawing.Size(48, 15);
            this.lblAppDate.TabIndex = 2;
            this.lblAppDate.Text = "상신일자";
            // 
            // lblReqEmpNm
            // 
            this.lblReqEmpNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReqEmpNm.Appearance.Options.UseFont = true;
            this.lblReqEmpNm.Location = new System.Drawing.Point(398, 9);
            this.lblReqEmpNm.Name = "lblReqEmpNm";
            this.lblReqEmpNm.Size = new System.Drawing.Size(36, 15);
            this.lblReqEmpNm.TabIndex = 4;
            this.lblReqEmpNm.Text = "상신자";
            // 
            // txtReqEmpNm
            // 
            this.txtReqEmpNm.Location = new System.Drawing.Point(440, 6);
            this.txtReqEmpNm.Name = "txtReqEmpNm";
            this.txtReqEmpNm.Properties.ReadOnly = true;
            this.txtReqEmpNm.Size = new System.Drawing.Size(130, 20);
            this.txtReqEmpNm.TabIndex = 5;
            // 
            // lblApprStatCd
            // 
            this.lblApprStatCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApprStatCd.Appearance.Options.UseFont = true;
            this.lblApprStatCd.Location = new System.Drawing.Point(628, 9);
            this.lblApprStatCd.Name = "lblApprStatCd";
            this.lblApprStatCd.Size = new System.Drawing.Size(48, 15);
            this.lblApprStatCd.TabIndex = 6;
            this.lblApprStatCd.Text = "결재상태";
            // 
            // lblTitle
            // 
            this.lblTitle.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Appearance.Options.UseFont = true;
            this.lblTitle.Location = new System.Drawing.Point(16, 35);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(48, 15);
            this.lblTitle.TabIndex = 8;
            this.lblTitle.Text = "문서제목";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(71, 31);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(745, 20);
            this.txtTitle.TabIndex = 9;
            // 
            // lblOpinion
            // 
            this.lblOpinion.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOpinion.Appearance.Options.UseFont = true;
            this.lblOpinion.Location = new System.Drawing.Point(4, 58);
            this.lblOpinion.Name = "lblOpinion";
            this.lblOpinion.Size = new System.Drawing.Size(60, 15);
            this.lblOpinion.TabIndex = 10;
            this.lblOpinion.Text = "기안자의견";
            // 
            // memoOpinion
            // 
            this.memoOpinion.Location = new System.Drawing.Point(71, 56);
            this.memoOpinion.Name = "memoOpinion";
            this.memoOpinion.Size = new System.Drawing.Size(745, 79);
            this.memoOpinion.TabIndex = 11;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FillColor = System.Drawing.Color.White;
            this.btnRefresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnRefresh.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnRefresh.Image = null;
            this.btnRefresh.Location = new System.Drawing.Point(6, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnRefresh.Size = new System.Drawing.Size(90, 26);
            this.btnRefresh.TabIndex = 12;
            this.btnRefresh.Text = "새로고침";
            this.btnRefresh.ToolTip = null;
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.Transparent;
            this.btnSubmit.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnSubmit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSubmit.FillColor = System.Drawing.Color.White;
            this.btnSubmit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnSubmit.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnSubmit.Image = null;
            this.btnSubmit.Location = new System.Drawing.Point(288, 2);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnSubmit.Size = new System.Drawing.Size(90, 26);
            this.btnSubmit.TabIndex = 13;
            this.btnSubmit.Text = "결재상신";
            this.btnSubmit.ToolTip = null;
            // 
            // btnApprove
            // 
            this.btnApprove.BackColor = System.Drawing.Color.Transparent;
            this.btnApprove.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnApprove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApprove.FillColor = System.Drawing.Color.White;
            this.btnApprove.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnApprove.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnApprove.Image = null;
            this.btnApprove.Location = new System.Drawing.Point(99, 2);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnApprove.Size = new System.Drawing.Size(90, 26);
            this.btnApprove.TabIndex = 14;
            this.btnApprove.Text = "승인";
            this.btnApprove.ToolTip = null;
            // 
            // btnReject
            // 
            this.btnReject.BackColor = System.Drawing.Color.Transparent;
            this.btnReject.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnReject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReject.FillColor = System.Drawing.Color.White;
            this.btnReject.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnReject.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnReject.Image = null;
            this.btnReject.Location = new System.Drawing.Point(384, 2);
            this.btnReject.Name = "btnReject";
            this.btnReject.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnReject.Size = new System.Drawing.Size(90, 26);
            this.btnReject.TabIndex = 15;
            this.btnReject.Text = "반려";
            this.btnReject.ToolTip = null;
            // 
            // btnCancelApprove
            // 
            this.btnCancelApprove.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelApprove.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnCancelApprove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelApprove.FillColor = System.Drawing.Color.White;
            this.btnCancelApprove.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnCancelApprove.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnCancelApprove.Image = null;
            this.btnCancelApprove.Location = new System.Drawing.Point(192, 2);
            this.btnCancelApprove.Name = "btnCancelApprove";
            this.btnCancelApprove.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnCancelApprove.Size = new System.Drawing.Size(90, 26);
            this.btnCancelApprove.TabIndex = 16;
            this.btnCancelApprove.Text = "승인취소";
            this.btnCancelApprove.ToolTip = null;
            // 
            // btnAck
            // 
            this.btnAck.BackColor = System.Drawing.Color.Transparent;
            this.btnAck.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnAck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAck.FillColor = System.Drawing.Color.White;
            this.btnAck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnAck.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnAck.Image = null;
            this.btnAck.Location = new System.Drawing.Point(480, 2);
            this.btnAck.Name = "btnAck";
            this.btnAck.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnAck.Size = new System.Drawing.Size(90, 26);
            this.btnAck.TabIndex = 17;
            this.btnAck.Text = "수신확인";
            this.btnAck.ToolTip = null;
            // 
            // lblRoute
            // 
            this.lblRoute.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRoute.Appearance.Options.UseFont = true;
            this.lblRoute.Location = new System.Drawing.Point(582, 12);
            this.lblRoute.Name = "lblRoute";
            this.lblRoute.Size = new System.Drawing.Size(88, 15);
            this.lblRoute.TabIndex = 0;
            this.lblRoute.Text = "저장된 결재경로";
            // 
            // cboRoute
            // 
            this.cboRoute.Location = new System.Drawing.Point(679, 10);
            this.cboRoute.Name = "cboRoute";
            this.cboRoute.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboRoute.Size = new System.Drawing.Size(200, 20);
            this.cboRoute.TabIndex = 1;
            // 
            // btnApplyRoute
            // 
            this.btnApplyRoute.BackColor = System.Drawing.Color.Transparent;
            this.btnApplyRoute.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnApplyRoute.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApplyRoute.FillColor = System.Drawing.Color.White;
            this.btnApplyRoute.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnApplyRoute.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnApplyRoute.Image = null;
            this.btnApplyRoute.Location = new System.Drawing.Point(883, 7);
            this.btnApplyRoute.Name = "btnApplyRoute";
            this.btnApplyRoute.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnApplyRoute.Size = new System.Drawing.Size(88, 26);
            this.btnApplyRoute.TabIndex = 2;
            this.btnApplyRoute.Text = "적용";
            this.btnApplyRoute.ToolTip = null;
            // 
            // txtNewRouteNm
            // 
            this.txtNewRouteNm.Location = new System.Drawing.Point(127, 1);
            this.txtNewRouteNm.Name = "txtNewRouteNm";
            this.txtNewRouteNm.Size = new System.Drawing.Size(200, 20);
            this.txtNewRouteNm.TabIndex = 4;
            // 
            // btnSaveRoute
            // 
            this.btnSaveRoute.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveRoute.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnSaveRoute.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveRoute.FillColor = System.Drawing.Color.White;
            this.btnSaveRoute.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnSaveRoute.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnSaveRoute.Image = null;
            this.btnSaveRoute.Location = new System.Drawing.Point(333, 0);
            this.btnSaveRoute.Name = "btnSaveRoute";
            this.btnSaveRoute.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnSaveRoute.Size = new System.Drawing.Size(60, 24);
            this.btnSaveRoute.TabIndex = 5;
            this.btnSaveRoute.Text = "저장";
            this.btnSaveRoute.ToolTip = null;
            // 
            // btnAddLine
            // 
            this.btnAddLine.BackColor = System.Drawing.Color.Transparent;
            this.btnAddLine.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnAddLine.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddLine.FillColor = System.Drawing.Color.White;
            this.btnAddLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnAddLine.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnAddLine.Image = null;
            this.btnAddLine.Location = new System.Drawing.Point(3, 3);
            this.btnAddLine.Name = "btnAddLine";
            this.btnAddLine.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnAddLine.Size = new System.Drawing.Size(120, 26);
            this.btnAddLine.TabIndex = 0;
            this.btnAddLine.Text = "승인자 추가";
            this.btnAddLine.ToolTip = null;
            // 
            // btnAddRecv
            // 
            this.btnAddRecv.BackColor = System.Drawing.Color.Transparent;
            this.btnAddRecv.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnAddRecv.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddRecv.FillColor = System.Drawing.Color.White;
            this.btnAddRecv.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnAddRecv.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnAddRecv.Image = null;
            this.btnAddRecv.Location = new System.Drawing.Point(129, 3);
            this.btnAddRecv.Name = "btnAddRecv";
            this.btnAddRecv.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnAddRecv.Size = new System.Drawing.Size(120, 26);
            this.btnAddRecv.TabIndex = 1;
            this.btnAddRecv.Text = "수신자 추가";
            this.btnAddRecv.ToolTip = null;
            // 
            // treeEmp
            // 
            this.treeEmp.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colTreeNm,
            this.colTreeEmpNo,
            this.colTreeJobGrade});
            this.treeEmp.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeEmp.ImageIndexFieldName = "ImgIdx";
            this.treeEmp.KeyFieldName = "NodeKey";
            this.treeEmp.Location = new System.Drawing.Point(0, 33);
            this.treeEmp.Name = "treeEmp";
            this.treeEmp.OptionsBehavior.Editable = false;
            this.treeEmp.ParentFieldName = "ParentKey";
            this.treeEmp.RowHeight = 26;
            this.treeEmp.Size = new System.Drawing.Size(364, 366);
            this.treeEmp.TabIndex = 0;
            // 
            // colTreeNm
            // 
            this.colTreeNm.Caption = "부서/사원";
            this.colTreeNm.FieldName = "Nm";
            this.colTreeNm.Name = "colTreeNm";
            this.colTreeNm.Visible = true;
            this.colTreeNm.VisibleIndex = 0;
            this.colTreeNm.Width = 180;
            // 
            // colTreeEmpNo
            // 
            this.colTreeEmpNo.Caption = "사번";
            this.colTreeEmpNo.FieldName = "EmpNo";
            this.colTreeEmpNo.Name = "colTreeEmpNo";
            this.colTreeEmpNo.Visible = true;
            this.colTreeEmpNo.VisibleIndex = 1;
            this.colTreeEmpNo.Width = 80;
            // 
            // colTreeJobGrade
            // 
            this.colTreeJobGrade.Caption = "직위";
            this.colTreeJobGrade.FieldName = "JobGrade";
            this.colTreeJobGrade.Name = "colTreeJobGrade";
            this.colTreeJobGrade.Visible = true;
            this.colTreeJobGrade.VisibleIndex = 2;
            this.colTreeJobGrade.Width = 70;
            // 
            // grdRecv
            // 
            this.grdRecv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdRecv.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdRecv.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdRecv.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdRecv.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdRecv.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdRecv.Location = new System.Drawing.Point(0, 27);
            this.grdRecv.MainView = this.gvwRecv;
            this.grdRecv.Name = "grdRecv";
            this.grdRecv.Size = new System.Drawing.Size(604, 194);
            this.grdRecv.TabIndex = 1;
            this.grdRecv.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvwRecv});
            // 
            // gvwRecv
            // 
            this.gvwRecv.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colRecvSort,
            this.colRecvEmpNm,
            this.colRecvStatCd,
            this.colRecvAppDt,
            this.colRecvRemark});
            this.gvwRecv.GridControl = this.grdRecv;
            this.gvwRecv.Name = "gvwRecv";
            this.gvwRecv.OptionsBehavior.Editable = false;
            this.gvwRecv.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvwRecv.OptionsView.ShowGroupPanel = false;
            // 
            // colRecvSort
            // 
            this.colRecvSort.Caption = "순번";
            this.colRecvSort.FieldName = "Sort";
            this.colRecvSort.Name = "colRecvSort";
            this.colRecvSort.Visible = true;
            this.colRecvSort.VisibleIndex = 0;
            this.colRecvSort.Width = 40;
            // 
            // colRecvEmpNm
            // 
            this.colRecvEmpNm.Caption = "수신자";
            this.colRecvEmpNm.FieldName = "EmpNm";
            this.colRecvEmpNm.Name = "colRecvEmpNm";
            this.colRecvEmpNm.Visible = true;
            this.colRecvEmpNm.VisibleIndex = 1;
            this.colRecvEmpNm.Width = 80;
            // 
            // colRecvStatCd
            // 
            this.colRecvStatCd.Caption = "상태";
            this.colRecvStatCd.FieldName = "StatCd";
            this.colRecvStatCd.Name = "colRecvStatCd";
            this.colRecvStatCd.Visible = true;
            this.colRecvStatCd.VisibleIndex = 2;
            this.colRecvStatCd.Width = 60;
            // 
            // colRecvAppDt
            // 
            this.colRecvAppDt.Caption = "확인일시";
            this.colRecvAppDt.FieldName = "AppDt";
            this.colRecvAppDt.Name = "colRecvAppDt";
            this.colRecvAppDt.Visible = true;
            this.colRecvAppDt.VisibleIndex = 3;
            this.colRecvAppDt.Width = 110;
            // 
            // colRecvRemark
            // 
            this.colRecvRemark.Caption = "의견";
            this.colRecvRemark.FieldName = "Remark";
            this.colRecvRemark.Name = "colRecvRemark";
            this.colRecvRemark.Visible = true;
            this.colRecvRemark.VisibleIndex = 4;
            this.colRecvRemark.Width = 150;
            // 
            // grdLine
            // 
            this.grdLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdLine.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdLine.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdLine.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdLine.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdLine.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdLine.Location = new System.Drawing.Point(0, 27);
            this.grdLine.MainView = this.gvwLine;
            this.grdLine.Name = "grdLine";
            this.grdLine.Size = new System.Drawing.Size(604, 112);
            this.grdLine.TabIndex = 1;
            this.grdLine.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvwLine});
            // 
            // gvwLine
            // 
            this.gvwLine.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colLineSort,
            this.colLineEmpNm,
            this.colLineStatCd,
            this.colLineAppDt,
            this.colLineRemark});
            this.gvwLine.GridControl = this.grdLine;
            this.gvwLine.Name = "gvwLine";
            this.gvwLine.OptionsBehavior.Editable = false;
            this.gvwLine.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvwLine.OptionsView.ShowGroupPanel = false;
            // 
            // colLineSort
            // 
            this.colLineSort.Caption = "순번";
            this.colLineSort.FieldName = "Sort";
            this.colLineSort.Name = "colLineSort";
            this.colLineSort.Visible = true;
            this.colLineSort.VisibleIndex = 0;
            this.colLineSort.Width = 40;
            // 
            // colLineEmpNm
            // 
            this.colLineEmpNm.Caption = "결재자";
            this.colLineEmpNm.FieldName = "EmpNm";
            this.colLineEmpNm.Name = "colLineEmpNm";
            this.colLineEmpNm.Visible = true;
            this.colLineEmpNm.VisibleIndex = 1;
            this.colLineEmpNm.Width = 80;
            // 
            // colLineStatCd
            // 
            this.colLineStatCd.Caption = "상태";
            this.colLineStatCd.FieldName = "StatCd";
            this.colLineStatCd.Name = "colLineStatCd";
            this.colLineStatCd.Visible = true;
            this.colLineStatCd.VisibleIndex = 2;
            this.colLineStatCd.Width = 60;
            // 
            // colLineAppDt
            // 
            this.colLineAppDt.Caption = "처리일시";
            this.colLineAppDt.FieldName = "AppDt";
            this.colLineAppDt.Name = "colLineAppDt";
            this.colLineAppDt.Visible = true;
            this.colLineAppDt.VisibleIndex = 3;
            this.colLineAppDt.Width = 110;
            // 
            // colLineRemark
            // 
            this.colLineRemark.Caption = "의견";
            this.colLineRemark.FieldName = "Remark";
            this.colLineRemark.Name = "colLineRemark";
            this.colLineRemark.Visible = true;
            this.colLineRemark.VisibleIndex = 4;
            this.colLineRemark.Width = 150;
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
            this.btnClose.Location = new System.Drawing.Point(893, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnClose.Size = new System.Drawing.Size(88, 26);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.ToolTip = null;
            // 
            // paTitle
            // 
            this.paTitle.Controls.Add(this.sectionHeaderWyn1);
            this.paTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle.Location = new System.Drawing.Point(0, 0);
            this.paTitle.Name = "paTitle";
            this.paTitle.Size = new System.Drawing.Size(984, 33);
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
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(984, 33);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 12;
            this.sectionHeaderWyn1.Text = "전자결재승인 [frmApp]";
            // 
            // ymdAppDate
            // 
            this.ymdAppDate.EditValue = null;
            this.ymdAppDate.Enabled = false;
            this.ymdAppDate.Location = new System.Drawing.Point(269, 6);
            this.ymdAppDate.Name = "ymdAppDate";
            this.ymdAppDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ymdAppDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ymdAppDate.Properties.ReadOnly = true;
            this.ymdAppDate.Size = new System.Drawing.Size(110, 20);
            this.ymdAppDate.TabIndex = 18;
            this.ymdAppDate.YyyyMmDd = null;
            // 
            // cboAppStatCd
            // 
            this.cboAppStatCd.EditValue = "";
            this.cboAppStatCd.Location = new System.Drawing.Point(682, 6);
            this.cboAppStatCd.Name = "cboAppStatCd";
            this.cboAppStatCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboAppStatCd.Properties.NullText = "";
            this.cboAppStatCd.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cboAppStatCd.Properties.ReadOnly = true;
            this.cboAppStatCd.Size = new System.Drawing.Size(134, 20);
            this.cboAppStatCd.TabIndex = 19;
            // 
            // panelWyn13
            // 
            this.panelWyn13.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn13.Appearance.Options.UseBackColor = true;
            this.panelWyn13.Controls.Add(this.btnClose);
            this.panelWyn13.Controls.Add(this.btnRefresh);
            this.panelWyn13.Controls.Add(this.btnApprove);
            this.panelWyn13.Controls.Add(this.btnCancelApprove);
            this.panelWyn13.Controls.Add(this.btnSubmit);
            this.panelWyn13.Controls.Add(this.btnReject);
            this.panelWyn13.Controls.Add(this.btnAck);
            this.panelWyn13.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn13.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn13.Location = new System.Drawing.Point(0, 33);
            this.panelWyn13.Name = "panelWyn13";
            this.panelWyn13.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn13.Size = new System.Drawing.Size(984, 30);
            this.panelWyn13.TabIndex = 12;
            // 
            // panelWyn1
            // 
            this.panelWyn1.Controls.Add(this.panelWyn8);
            this.panelWyn1.Controls.Add(this.panelWyn4);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(5, 146);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Size = new System.Drawing.Size(974, 426);
            this.panelWyn1.TabIndex = 13;
            // 
            // panelWyn4
            // 
            this.panelWyn4.Controls.Add(this.sectionHeaderWyn2);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(0, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn4.Size = new System.Drawing.Size(974, 27);
            this.panelWyn4.TabIndex = 10;
            this.panelWyn4.Visible = false;
            // 
            // sectionHeaderWyn2
            // 
            this.sectionHeaderWyn2.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn2.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn2.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn2.Name = "sectionHeaderWyn2";
            this.sectionHeaderWyn2.Size = new System.Drawing.Size(969, 25);
            this.sectionHeaderWyn2.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn2.SvgIcon")));
            this.sectionHeaderWyn2.TabIndex = 10;
            this.sectionHeaderWyn2.Text = "결재라인구성";
            // 
            // panelWyn2
            // 
            this.panelWyn2.Controls.Add(this.grdLine);
            this.panelWyn2.Controls.Add(this.panelWyn5);
            this.panelWyn2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn2.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn2.Location = new System.Drawing.Point(0, 0);
            this.panelWyn2.Name = "panelWyn2";
            this.panelWyn2.Size = new System.Drawing.Size(604, 139);
            this.panelWyn2.TabIndex = 12;
            // 
            // panelWyn3
            // 
            this.panelWyn3.Controls.Add(this.grdRecv);
            this.panelWyn3.Controls.Add(this.panelWyn6);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(0, 145);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Size = new System.Drawing.Size(604, 221);
            this.panelWyn3.TabIndex = 12;
            // 
            // panelWyn5
            // 
            this.panelWyn5.Controls.Add(this.txtNewRouteNm);
            this.panelWyn5.Controls.Add(this.btnSaveRoute);
            this.panelWyn5.Controls.Add(this.sectionHeaderWyn3);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(0, 0);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn5.Size = new System.Drawing.Size(604, 27);
            this.panelWyn5.TabIndex = 11;
            this.panelWyn5.Visible = false;
            // 
            // sectionHeaderWyn3
            // 
            this.sectionHeaderWyn3.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn3.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn3.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn3.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn3.Name = "sectionHeaderWyn3";
            this.sectionHeaderWyn3.Size = new System.Drawing.Size(599, 25);
            this.sectionHeaderWyn3.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn3.SvgIcon")));
            this.sectionHeaderWyn3.TabIndex = 10;
            this.sectionHeaderWyn3.Text = "승인부";
            // 
            // panelWyn6
            // 
            this.panelWyn6.Controls.Add(this.sectionHeaderWyn4);
            this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn6.Location = new System.Drawing.Point(0, 0);
            this.panelWyn6.Name = "panelWyn6";
            this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn6.Size = new System.Drawing.Size(604, 27);
            this.panelWyn6.TabIndex = 11;
            this.panelWyn6.Visible = false;
            // 
            // sectionHeaderWyn4
            // 
            this.sectionHeaderWyn4.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn4.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn4.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn4.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn4.Name = "sectionHeaderWyn4";
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(599, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 10;
            this.sectionHeaderWyn4.Text = "수신부";
            // 
            // panelWyn7
            // 
            this.panelWyn7.Controls.Add(this.panelWyn3);
            this.panelWyn7.Controls.Add(this.splitterWyn2);
            this.panelWyn7.Controls.Add(this.panelWyn2);
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(370, 33);
            this.panelWyn7.Name = "panelWyn7";
            this.panelWyn7.Size = new System.Drawing.Size(604, 366);
            this.panelWyn7.TabIndex = 13;
            // 
            // splitterWyn2
            // 
            this.splitterWyn2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitterWyn2.Location = new System.Drawing.Point(0, 139);
            this.splitterWyn2.Name = "splitterWyn2";
            this.splitterWyn2.Size = new System.Drawing.Size(604, 6);
            this.splitterWyn2.TabIndex = 13;
            this.splitterWyn2.TabStop = false;
            // 
            // panelWyn8
            // 
            this.panelWyn8.Controls.Add(this.panelWyn7);
            this.panelWyn8.Controls.Add(this.splitterWyn1);
            this.panelWyn8.Controls.Add(this.treeEmp);
            this.panelWyn8.Controls.Add(this.panelWyn9);
            this.panelWyn8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn8.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn8.Location = new System.Drawing.Point(0, 27);
            this.panelWyn8.Name = "panelWyn8";
            this.panelWyn8.Size = new System.Drawing.Size(974, 399);
            this.panelWyn8.TabIndex = 14;
            // 
            // panelWyn9
            // 
            this.panelWyn9.Controls.Add(this.btnApplyRoute);
            this.panelWyn9.Controls.Add(this.cboRoute);
            this.panelWyn9.Controls.Add(this.lblRoute);
            this.panelWyn9.Controls.Add(this.btnAddRecv);
            this.panelWyn9.Controls.Add(this.btnAddLine);
            this.panelWyn9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn9.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn9.Location = new System.Drawing.Point(0, 0);
            this.panelWyn9.Name = "panelWyn9";
            this.panelWyn9.Size = new System.Drawing.Size(974, 33);
            this.panelWyn9.TabIndex = 1;
            // 
            // splitterWyn1
            // 
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Location = new System.Drawing.Point(364, 33);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(6, 366);
            this.splitterWyn1.TabIndex = 16;
            this.splitterWyn1.TabStop = false;
            // 
            // panelWyn10
            // 
            this.panelWyn10.Controls.Add(this.panelWyn1);
            this.panelWyn10.Controls.Add(this.panHeader);
            this.panelWyn10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn10.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn10.Location = new System.Drawing.Point(0, 63);
            this.panelWyn10.Name = "panelWyn10";
            this.panelWyn10.Padding = new System.Windows.Forms.Padding(5);
            this.panelWyn10.Size = new System.Drawing.Size(984, 577);
            this.panelWyn10.TabIndex = 14;
            // 
            // popApp
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.ClientSize = new System.Drawing.Size(984, 640);
            this.Controls.Add(this.panelWyn10);
            this.Controls.Add(this.panelWyn13);
            this.Controls.Add(this.paTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "popApp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "전자결재";
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAppNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReqEmpNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTitle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoOpinion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboRoute.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewRouteNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeEmp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdRecv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwRecv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
            this.paTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ymdAppDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ymdAppDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAppStatCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn13)).EndInit();
            this.panelWyn13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
            this.panelWyn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
            this.panelWyn6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
            this.panelWyn7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).EndInit();
            this.panelWyn8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn9)).EndInit();
            this.panelWyn9.ResumeLayout(false);
            this.panelWyn9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn10)).EndInit();
            this.panelWyn10.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblAppNo;
    private TextEditWyn txtAppNo;
    private DevExpress.XtraEditors.LabelControl lblAppDate;
    private DevExpress.XtraEditors.LabelControl lblReqEmpNm;
    private TextEditWyn txtReqEmpNm;
    private DevExpress.XtraEditors.LabelControl lblApprStatCd;
    private DevExpress.XtraEditors.LabelControl lblTitle;
    private TextEditWyn txtTitle;
    private DevExpress.XtraEditors.LabelControl lblOpinion;
    private MemoEditWyn memoOpinion;
    private ButtonWyn btnRefresh;
    private ButtonWyn btnSubmit;
    private ButtonWyn btnApprove;
    private ButtonWyn btnReject;
    private ButtonWyn btnCancelApprove;
    private ButtonWyn btnAck;
    private TreeListWyn treeEmp;
    private DevExpress.XtraTreeList.Columns.TreeListColumn colTreeNm;
    private DevExpress.XtraTreeList.Columns.TreeListColumn colTreeEmpNo;
    private DevExpress.XtraTreeList.Columns.TreeListColumn colTreeJobGrade;
    private ButtonWyn btnAddLine;
    private ButtonWyn btnAddRecv;
    private DevExpress.XtraEditors.LabelControl lblRoute;
    private DevExpress.XtraEditors.ComboBoxEdit cboRoute;
    private ButtonWyn btnApplyRoute;
    private TextEditWyn txtNewRouteNm;
    private ButtonWyn btnSaveRoute;
    private GridControlWyn grdLine;
    private GridViewWyn gvwLine;
    private GridColumn colLineSort;
    private GridColumn colLineEmpNm;
    private GridColumn colLineStatCd;
    private GridColumn colLineAppDt;
    private GridColumn colLineRemark;
    private GridControlWyn grdRecv;
    private GridViewWyn gvwRecv;
    private GridColumn colRecvSort;
    private GridColumn colRecvEmpNm;
    private GridColumn colRecvStatCd;
    private GridColumn colRecvAppDt;
    private GridColumn colRecvRemark;
    private ButtonWyn btnClose;
    private PanelWyn paTitle;
    private SectionHeaderWyn sectionHeaderWyn1;
    private LookUpEditWyn cboAppStatCd;
    private DateEditWyn ymdAppDate;
    private PanelWyn panelWyn13;
    private PanelWyn panelWyn1;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn6;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn panelWyn7;
    private PanelWyn panelWyn2;
    private PanelWyn panelWyn5;
    private SectionHeaderWyn sectionHeaderWyn3;
    private PanelWyn panelWyn4;
    private SectionHeaderWyn sectionHeaderWyn2;
    private PanelWyn panelWyn8;
    private SplitterWyn splitterWyn2;
    private SplitterWyn splitterWyn1;
    private PanelWyn panelWyn9;
    private PanelWyn panelWyn10;
}
