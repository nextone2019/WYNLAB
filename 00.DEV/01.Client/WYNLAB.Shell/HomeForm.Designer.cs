// VS 디자이너가 자동 생성하는 필드 선언에는 = null!을 안 붙여서 nullable 경고(CS8618)가
// 계속 나기 때문에, 이 파일(디자이너 전용)만 nullable 검사를 끈다 - 흔한 관례.
#nullable disable
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraTab;
using WYNLAB.Base.Controls;

namespace WYNLAB.Shell;

/// <summary>
/// 홈 화면 디자이너 - 페이지 골격(왼쪽: KPI/결재 리스트/공지사항, 오른쪽: 오늘의 일정/빠른 실행)과
/// 결재 리스트(제목줄/탭줄/검색조건/하위탭/그리드) 안의 고정 컨트롤이 전부 여기 있다.
/// 데이터에 따라 개수가 달라지는 부분(공지/일정 행, 기안서 작성 타일/칩)과 아이콘 이미지, 사용자 정보
/// 같은 런타임 값은 HomeForm.cs / HomeForm.ApprovalCenter.cs가 채운다.
/// </summary>
public partial class HomeForm
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.panScroll = new System.Windows.Forms.Panel();
            this.panContent = new System.Windows.Forms.Panel();
            this.panMain = new System.Windows.Forms.Panel();
            this.panLeft = new System.Windows.Forms.Panel();
            this.panNotice = new System.Windows.Forms.Panel();
            this.panNoticeFrame = new System.Windows.Forms.Panel();
            this.panNoticeList = new System.Windows.Forms.Panel();
            this.panNoticeHead = new System.Windows.Forms.Panel();
            this.lblNoticeColTitle = new DevExpress.XtraEditors.LabelControl();
            this.lblNoticeColAuthor = new DevExpress.XtraEditors.LabelControl();
            this.lblNoticeColDate = new DevExpress.XtraEditors.LabelControl();
            this.lblNoticeColImportant = new DevExpress.XtraEditors.LabelControl();
            this.panNoticeHeadMarginL = new System.Windows.Forms.Panel();
            this.panNoticeHeadMarginR = new System.Windows.Forms.Panel();
            this.panNoticeHeadLine = new System.Windows.Forms.Panel();
            this.panNoticeSpacer = new System.Windows.Forms.Panel();
            this.panNoticeHeader = new System.Windows.Forms.Panel();
            this.tabNotice = new DevExpress.XtraTab.XtraTabControl();
            this.tabPageNotice = new DevExpress.XtraTab.XtraTabPage();
            this.lblNoticeAdd = new DevExpress.XtraEditors.LabelControl();
            this.panGap2 = new System.Windows.Forms.Panel();
            this.panApproval = new System.Windows.Forms.Panel();
            this.panApprovalBody = new System.Windows.Forms.Panel();
            this.panListView = new System.Windows.Forms.Panel();
            this.grdApproval = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvwApproval = new WYNLAB.Base.Controls.GridViewWyn();
            this.colKind = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAppNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colReqDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDocTypeNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDocNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAppTitle = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colReqEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatText = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCurApprEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLastApprEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panSubRow = new System.Windows.Forms.Panel();
            this.tabSub = new DevExpress.XtraTab.XtraTabControl();
            this.tabPageSubPending = new DevExpress.XtraTab.XtraTabPage();
            this.tabPageSubRejected = new DevExpress.XtraTab.XtraTabPage();
            this.tabPageSubDone = new DevExpress.XtraTab.XtraTabPage();
            this.panListGap = new System.Windows.Forms.Panel();
            this.panSearch = new System.Windows.Forms.Panel();
            this.lblSearchAcc = new DevExpress.XtraEditors.LabelControl();
            this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblSearchDocType = new DevExpress.XtraEditors.LabelControl();
            this.cboSearchDocType = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblSearchDate = new DevExpress.XtraEditors.LabelControl();
            this.dteSearchFrom = new WYNLAB.Base.Controls.DateEditWyn();
            this.lblSearchTilde = new DevExpress.XtraEditors.LabelControl();
            this.dteSearchTo = new WYNLAB.Base.Controls.DateEditWyn();
            this.lblSearchTitle = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchTitle = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSearchReqEmp = new DevExpress.XtraEditors.LabelControl();
            this.popSearchReqEmp = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.txtSearchReqEmpNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.btnSearch = new WYNLAB.Base.Controls.ButtonWyn();
            this.panComposeView = new System.Windows.Forms.Panel();
            this.flpTiles = new System.Windows.Forms.FlowLayoutPanel();
            this.lblComposeHint = new DevExpress.XtraEditors.LabelControl();
            this.flpChips = new System.Windows.Forms.FlowLayoutPanel();
            this.panTabRow = new System.Windows.Forms.Panel();
            this.tabMain = new DevExpress.XtraTab.XtraTabControl();
            this.tabPageDrafted = new DevExpress.XtraTab.XtraTabPage();
            this.tabPageInbox = new DevExpress.XtraTab.XtraTabPage();
            this.tabPageCompose = new DevExpress.XtraTab.XtraTabPage();
            this.panColGap = new System.Windows.Forms.Panel();
            this.panRight = new System.Windows.Forms.Panel();
            this.panSchedule = new System.Windows.Forms.Panel();
            this.panScheduleList = new System.Windows.Forms.Panel();
            this.panScheduleHeader = new System.Windows.Forms.Panel();
            this.pnlScheduleTitle = new System.Windows.Forms.Panel();
            this.dotSchedule = new System.Windows.Forms.Panel();
            this.lblScheduleTitle = new DevExpress.XtraEditors.LabelControl();
            this.lblScheduleDate = new DevExpress.XtraEditors.LabelControl();
            this.panScheduleGap = new System.Windows.Forms.Panel();
            this.panQuickLaunch = new System.Windows.Forms.Panel();
            this.panQuickTiles = new System.Windows.Forms.Panel();
            this.tileQuickSchedule = new System.Windows.Forms.Panel();
            this.lblQuickSchedule = new DevExpress.XtraEditors.LabelControl();
            this.panQuickGap2 = new System.Windows.Forms.Panel();
            this.tileQuickBoard = new System.Windows.Forms.Panel();
            this.lblQuickBoard = new DevExpress.XtraEditors.LabelControl();
            this.panQuickGap1 = new System.Windows.Forms.Panel();
            this.tileQuickAppr = new System.Windows.Forms.Panel();
            this.lblQuickAppr = new DevExpress.XtraEditors.LabelControl();
            this.panQuickTitleGap = new System.Windows.Forms.Panel();
            this.pnlQuickTitle = new System.Windows.Forms.Panel();
            this.dotQuick = new System.Windows.Forms.Panel();
            this.lblQuickTitle = new DevExpress.XtraEditors.LabelControl();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.panScroll.SuspendLayout();
            this.panContent.SuspendLayout();
            this.panMain.SuspendLayout();
            this.panLeft.SuspendLayout();
            this.panNotice.SuspendLayout();
            this.panNoticeFrame.SuspendLayout();
            this.panNoticeHead.SuspendLayout();
            this.panNoticeHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabNotice)).BeginInit();
            this.tabNotice.SuspendLayout();
            this.panApproval.SuspendLayout();
            this.panApprovalBody.SuspendLayout();
            this.panListView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdApproval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwApproval)).BeginInit();
            this.panSubRow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabSub)).BeginInit();
            this.tabSub.SuspendLayout();
            this.panSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchDocType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchTitle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popSearchReqEmp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchReqEmpNo.Properties)).BeginInit();
            this.panComposeView.SuspendLayout();
            this.panTabRow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).BeginInit();
            this.tabMain.SuspendLayout();
            this.panRight.SuspendLayout();
            this.panSchedule.SuspendLayout();
            this.panScheduleHeader.SuspendLayout();
            this.pnlScheduleTitle.SuspendLayout();
            this.panQuickLaunch.SuspendLayout();
            this.panQuickTiles.SuspendLayout();
            this.tileQuickSchedule.SuspendLayout();
            this.tileQuickBoard.SuspendLayout();
            this.tileQuickAppr.SuspendLayout();
            this.pnlQuickTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panScroll
            // 
            this.panScroll.AutoScroll = true;
            this.panScroll.BackColor = System.Drawing.Color.White;
            this.panScroll.Controls.Add(this.panContent);
            this.panScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panScroll.Location = new System.Drawing.Point(0, 0);
            this.panScroll.Name = "panScroll";
            this.panScroll.Size = new System.Drawing.Size(1560, 900);
            this.panScroll.TabIndex = 0;
            // 
            // panContent
            // 
            this.panContent.AutoSize = true;
            this.panContent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panContent.Controls.Add(this.panMain);
            this.panContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.panContent.Location = new System.Drawing.Point(0, 0);
            this.panContent.Name = "panContent";
            this.panContent.Padding = new System.Windows.Forms.Padding(24, 20, 24, 24);
            this.panContent.Size = new System.Drawing.Size(1560, 898);
            this.panContent.TabIndex = 0;
            // 
            // panMain
            // 
            this.panMain.Controls.Add(this.panLeft);
            this.panMain.Controls.Add(this.panColGap);
            this.panMain.Controls.Add(this.panRight);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.panMain.Location = new System.Drawing.Point(24, 20);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(1512, 854);
            this.panMain.TabIndex = 0;
            // 
            // panLeft
            // 
            this.panLeft.Controls.Add(this.panNotice);
            this.panLeft.Controls.Add(this.panGap2);
            this.panLeft.Controls.Add(this.panApproval);
            this.panLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panLeft.Location = new System.Drawing.Point(0, 0);
            this.panLeft.Name = "panLeft";
            this.panLeft.Size = new System.Drawing.Size(1176, 854);
            this.panLeft.TabIndex = 0;
            // 
            // panNotice
            // 
            this.panNotice.Controls.Add(this.panNoticeFrame);
            this.panNotice.Controls.Add(this.panNoticeSpacer);
            this.panNotice.Controls.Add(this.panNoticeHeader);
            this.panNotice.Dock = System.Windows.Forms.DockStyle.Top;
            this.panNotice.Location = new System.Drawing.Point(0, 496);
            this.panNotice.Name = "panNotice";
            this.panNotice.Padding = new System.Windows.Forms.Padding(16);
            this.panNotice.Size = new System.Drawing.Size(1176, 270);
            this.panNotice.TabIndex = 0;
            // 
            // panNoticeFrame
            // 
            this.panNoticeFrame.Controls.Add(this.panNoticeList);
            this.panNoticeFrame.Controls.Add(this.panNoticeHead);
            this.panNoticeFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panNoticeFrame.Location = new System.Drawing.Point(16, 45);
            this.panNoticeFrame.Name = "panNoticeFrame";
            this.panNoticeFrame.Size = new System.Drawing.Size(1144, 209);
            this.panNoticeFrame.TabIndex = 0;
            this.panNoticeFrame.Paint += new System.Windows.Forms.PaintEventHandler(this.BorderFrame_Paint);
            // 
            // panNoticeList
            // 
            this.panNoticeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panNoticeList.Location = new System.Drawing.Point(0, 30);
            this.panNoticeList.Name = "panNoticeList";
            this.panNoticeList.Size = new System.Drawing.Size(1144, 179);
            this.panNoticeList.TabIndex = 0;
            // 
            // panNoticeHead
            // 
            this.panNoticeHead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.panNoticeHead.Controls.Add(this.lblNoticeColTitle);
            this.panNoticeHead.Controls.Add(this.lblNoticeColAuthor);
            this.panNoticeHead.Controls.Add(this.lblNoticeColDate);
            this.panNoticeHead.Controls.Add(this.lblNoticeColImportant);
            this.panNoticeHead.Controls.Add(this.panNoticeHeadMarginL);
            this.panNoticeHead.Controls.Add(this.panNoticeHeadMarginR);
            this.panNoticeHead.Controls.Add(this.panNoticeHeadLine);
            this.panNoticeHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.panNoticeHead.Location = new System.Drawing.Point(0, 0);
            this.panNoticeHead.Name = "panNoticeHead";
            this.panNoticeHead.Size = new System.Drawing.Size(1144, 30);
            this.panNoticeHead.TabIndex = 1;
            // 
            // lblNoticeColTitle
            // 
            this.lblNoticeColTitle.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblNoticeColTitle.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(132)))), ((int)(((byte)(138)))));
            this.lblNoticeColTitle.Appearance.Options.UseFont = true;
            this.lblNoticeColTitle.Appearance.Options.UseForeColor = true;
            this.lblNoticeColTitle.Appearance.Options.UseTextOptions = true;
            this.lblNoticeColTitle.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lblNoticeColTitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblNoticeColTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNoticeColTitle.Location = new System.Drawing.Point(102, 0);
            this.lblNoticeColTitle.Name = "lblNoticeColTitle";
            this.lblNoticeColTitle.Size = new System.Drawing.Size(790, 29);
            this.lblNoticeColTitle.TabIndex = 0;
            this.lblNoticeColTitle.Text = "제목";
            // 
            // lblNoticeColAuthor
            // 
            this.lblNoticeColAuthor.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblNoticeColAuthor.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(132)))), ((int)(((byte)(138)))));
            this.lblNoticeColAuthor.Appearance.Options.UseFont = true;
            this.lblNoticeColAuthor.Appearance.Options.UseForeColor = true;
            this.lblNoticeColAuthor.Appearance.Options.UseTextOptions = true;
            this.lblNoticeColAuthor.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lblNoticeColAuthor.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblNoticeColAuthor.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblNoticeColAuthor.Location = new System.Drawing.Point(892, 0);
            this.lblNoticeColAuthor.Name = "lblNoticeColAuthor";
            this.lblNoticeColAuthor.Size = new System.Drawing.Size(90, 29);
            this.lblNoticeColAuthor.TabIndex = 1;
            this.lblNoticeColAuthor.Text = "작성자";
            // 
            // lblNoticeColDate
            // 
            this.lblNoticeColDate.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblNoticeColDate.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(132)))), ((int)(((byte)(138)))));
            this.lblNoticeColDate.Appearance.Options.UseFont = true;
            this.lblNoticeColDate.Appearance.Options.UseForeColor = true;
            this.lblNoticeColDate.Appearance.Options.UseTextOptions = true;
            this.lblNoticeColDate.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lblNoticeColDate.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblNoticeColDate.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblNoticeColDate.Location = new System.Drawing.Point(982, 0);
            this.lblNoticeColDate.Name = "lblNoticeColDate";
            this.lblNoticeColDate.Size = new System.Drawing.Size(150, 29);
            this.lblNoticeColDate.TabIndex = 2;
            this.lblNoticeColDate.Text = "작성일시";
            // 
            // lblNoticeColImportant
            // 
            this.lblNoticeColImportant.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblNoticeColImportant.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(132)))), ((int)(((byte)(138)))));
            this.lblNoticeColImportant.Appearance.Options.UseFont = true;
            this.lblNoticeColImportant.Appearance.Options.UseForeColor = true;
            this.lblNoticeColImportant.Appearance.Options.UseTextOptions = true;
            this.lblNoticeColImportant.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lblNoticeColImportant.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblNoticeColImportant.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblNoticeColImportant.Location = new System.Drawing.Point(12, 0);
            this.lblNoticeColImportant.Name = "lblNoticeColImportant";
            this.lblNoticeColImportant.Size = new System.Drawing.Size(90, 29);
            this.lblNoticeColImportant.TabIndex = 3;
            this.lblNoticeColImportant.Text = "중요";
            // 
            // panNoticeHeadMarginL
            // 
            this.panNoticeHeadMarginL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.panNoticeHeadMarginL.Dock = System.Windows.Forms.DockStyle.Left;
            this.panNoticeHeadMarginL.Location = new System.Drawing.Point(0, 0);
            this.panNoticeHeadMarginL.Name = "panNoticeHeadMarginL";
            this.panNoticeHeadMarginL.Size = new System.Drawing.Size(12, 29);
            this.panNoticeHeadMarginL.TabIndex = 4;
            // 
            // panNoticeHeadMarginR
            // 
            this.panNoticeHeadMarginR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.panNoticeHeadMarginR.Dock = System.Windows.Forms.DockStyle.Right;
            this.panNoticeHeadMarginR.Location = new System.Drawing.Point(1132, 0);
            this.panNoticeHeadMarginR.Name = "panNoticeHeadMarginR";
            this.panNoticeHeadMarginR.Size = new System.Drawing.Size(12, 29);
            this.panNoticeHeadMarginR.TabIndex = 5;
            // 
            // panNoticeHeadLine
            // 
            this.panNoticeHeadLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(224)))), ((int)(((byte)(229)))));
            this.panNoticeHeadLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panNoticeHeadLine.Location = new System.Drawing.Point(0, 29);
            this.panNoticeHeadLine.Name = "panNoticeHeadLine";
            this.panNoticeHeadLine.Size = new System.Drawing.Size(1144, 1);
            this.panNoticeHeadLine.TabIndex = 6;
            // 
            // panNoticeSpacer
            // 
            this.panNoticeSpacer.Dock = System.Windows.Forms.DockStyle.Top;
            this.panNoticeSpacer.Location = new System.Drawing.Point(16, 41);
            this.panNoticeSpacer.Name = "panNoticeSpacer";
            this.panNoticeSpacer.Size = new System.Drawing.Size(1144, 4);
            this.panNoticeSpacer.TabIndex = 1;
            // 
            // panNoticeHeader
            // 
            this.panNoticeHeader.Controls.Add(this.tabNotice);
            this.panNoticeHeader.Controls.Add(this.lblNoticeAdd);
            this.panNoticeHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panNoticeHeader.Location = new System.Drawing.Point(16, 16);
            this.panNoticeHeader.Name = "panNoticeHeader";
            this.panNoticeHeader.Size = new System.Drawing.Size(1144, 25);
            this.panNoticeHeader.TabIndex = 2;
            // 
            // tabNotice
            // 
            this.tabNotice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabNotice.Location = new System.Drawing.Point(0, 0);
            this.tabNotice.Name = "tabNotice";
            this.tabNotice.SelectedTabPage = this.tabPageNotice;
            this.tabNotice.Size = new System.Drawing.Size(1120, 25);
            this.tabNotice.TabIndex = 0;
            this.tabNotice.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabPageNotice});
            // 
            // tabPageNotice
            // 
            this.tabPageNotice.Name = "tabPageNotice";
            this.tabPageNotice.Size = new System.Drawing.Size(1118, 0);
            this.tabPageNotice.Text = "사내 공지사항";
            // 
            // lblNoticeAdd
            // 
            this.lblNoticeAdd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblNoticeAdd.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(132)))), ((int)(((byte)(138)))));
            this.lblNoticeAdd.Appearance.Options.UseFont = true;
            this.lblNoticeAdd.Appearance.Options.UseForeColor = true;
            this.lblNoticeAdd.Appearance.Options.UseTextOptions = true;
            this.lblNoticeAdd.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblNoticeAdd.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblNoticeAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblNoticeAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblNoticeAdd.Location = new System.Drawing.Point(1120, 0);
            this.lblNoticeAdd.Name = "lblNoticeAdd";
            this.lblNoticeAdd.Size = new System.Drawing.Size(24, 25);
            this.lblNoticeAdd.TabIndex = 1;
            this.lblNoticeAdd.Text = "+";
            this.lblNoticeAdd.Click += new System.EventHandler(this.lblNoticeAdd_Click);
            // 
            // panGap2
            // 
            this.panGap2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panGap2.Location = new System.Drawing.Point(0, 480);
            this.panGap2.Name = "panGap2";
            this.panGap2.Size = new System.Drawing.Size(1176, 16);
            this.panGap2.TabIndex = 1;
            // 
            // panApproval
            // 
            this.panApproval.Controls.Add(this.panApprovalBody);
            this.panApproval.Controls.Add(this.panTabRow);
            this.panApproval.Dock = System.Windows.Forms.DockStyle.Top;
            this.panApproval.Location = new System.Drawing.Point(0, 0);
            this.panApproval.Name = "panApproval";
            this.panApproval.Padding = new System.Windows.Forms.Padding(16, 12, 16, 16);
            this.panApproval.Size = new System.Drawing.Size(1176, 480);
            this.panApproval.TabIndex = 2;
            // 
            // panApprovalBody
            // 
            this.panApprovalBody.Controls.Add(this.panListView);
            this.panApprovalBody.Controls.Add(this.panComposeView);
            this.panApprovalBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panApprovalBody.Location = new System.Drawing.Point(16, 37);
            this.panApprovalBody.Name = "panApprovalBody";
            this.panApprovalBody.Size = new System.Drawing.Size(1144, 427);
            this.panApprovalBody.TabIndex = 0;
            // 
            // panListView
            // 
            this.panListView.Controls.Add(this.panelWyn1);
            this.panListView.Controls.Add(this.panSubRow);
            this.panListView.Controls.Add(this.panListGap);
            this.panListView.Controls.Add(this.panSearch);
            this.panListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panListView.Location = new System.Drawing.Point(0, 0);
            this.panListView.Name = "panListView";
            this.panListView.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.panListView.Size = new System.Drawing.Size(1144, 427);
            this.panListView.TabIndex = 0;
            // 
            // grdApproval
            // 
            this.grdApproval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdApproval.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdApproval.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdApproval.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdApproval.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdApproval.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdApproval.Location = new System.Drawing.Point(0, 3);
            this.grdApproval.MainView = this.gvwApproval;
            this.grdApproval.Name = "grdApproval";
            this.grdApproval.Size = new System.Drawing.Size(1144, 316);
            this.grdApproval.TabIndex = 0;
            this.grdApproval.UseEmbeddedNavigator = false;
            this.grdApproval.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvwApproval});
            this.grdApproval.SizeChanged += new System.EventHandler(this.grdApproval_SizeChanged);
            // 
            // gvwApproval
            // 
            this.gvwApproval.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.gvwApproval.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvwApproval.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gvwApproval.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colKind,
            this.colAppNo,
            this.colReqDt,
            this.colDocTypeNm,
            this.colDocNo,
            this.colAppTitle,
            this.colReqEmpNm,
            this.colStatText,
            this.colCurApprEmpNm,
            this.colLastApprEmpNm});
            this.gvwApproval.GridControl = this.grdApproval;
            this.gvwApproval.HighlightFocusedRow = true;
            this.gvwApproval.Name = "gvwApproval";
            this.gvwApproval.OptionsBehavior.Editable = false;
            this.gvwApproval.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvwApproval.OptionsView.ColumnAutoWidth = false;
            this.gvwApproval.OptionsView.EnableAppearanceEvenRow = true;
            this.gvwApproval.OptionsView.ShowGroupPanel = false;
            this.gvwApproval.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            this.gvwApproval.RowHeight = 25;
            this.gvwApproval.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gvwApproval_RowCellClick);
            // 
            // colKind
            // 
            this.colKind.AppearanceCell.Options.UseTextOptions = true;
            this.colKind.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colKind.AppearanceHeader.Options.UseTextOptions = true;
            this.colKind.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colKind.Caption = "구분";
            this.colKind.FieldName = "kind";
            this.colKind.Name = "colKind";
            this.colKind.OptionsColumn.AllowEdit = false;
            this.colKind.Visible = true;
            this.colKind.VisibleIndex = 0;
            this.colKind.Width = 56;
            // 
            // colAppNo
            // 
            this.colAppNo.AppearanceCell.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.colAppNo.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(200)))));
            this.colAppNo.AppearanceCell.Options.UseFont = true;
            this.colAppNo.AppearanceCell.Options.UseForeColor = true;
            this.colAppNo.AppearanceCell.Options.UseTextOptions = true;
            this.colAppNo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAppNo.AppearanceHeader.Options.UseTextOptions = true;
            this.colAppNo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAppNo.Caption = "결재번호";
            this.colAppNo.FieldName = "app_no";
            this.colAppNo.Name = "colAppNo";
            this.colAppNo.OptionsColumn.AllowEdit = false;
            this.colAppNo.Visible = true;
            this.colAppNo.VisibleIndex = 1;
            this.colAppNo.Width = 100;
            // 
            // colReqDt
            // 
            this.colReqDt.AppearanceCell.Options.UseTextOptions = true;
            this.colReqDt.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colReqDt.AppearanceHeader.Options.UseTextOptions = true;
            this.colReqDt.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colReqDt.Caption = "상신일시";
            this.colReqDt.FieldName = "req_dt";
            this.colReqDt.Name = "colReqDt";
            this.colReqDt.OptionsColumn.AllowEdit = false;
            this.colReqDt.Visible = true;
            this.colReqDt.VisibleIndex = 2;
            this.colReqDt.Width = 130;
            // 
            // colDocTypeNm
            // 
            this.colDocTypeNm.AppearanceCell.Options.UseTextOptions = true;
            this.colDocTypeNm.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDocTypeNm.AppearanceHeader.Options.UseTextOptions = true;
            this.colDocTypeNm.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDocTypeNm.Caption = "문서구분";
            this.colDocTypeNm.FieldName = "doc_type_nm";
            this.colDocTypeNm.Name = "colDocTypeNm";
            this.colDocTypeNm.OptionsColumn.AllowEdit = false;
            this.colDocTypeNm.Visible = true;
            this.colDocTypeNm.VisibleIndex = 3;
            this.colDocTypeNm.Width = 120;
            // 
            // colDocNo
            // 
            this.colDocNo.AppearanceCell.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.colDocNo.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(200)))));
            this.colDocNo.AppearanceCell.Options.UseFont = true;
            this.colDocNo.AppearanceCell.Options.UseForeColor = true;
            this.colDocNo.AppearanceCell.Options.UseTextOptions = true;
            this.colDocNo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDocNo.AppearanceHeader.Options.UseTextOptions = true;
            this.colDocNo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDocNo.Caption = "문서번호";
            this.colDocNo.FieldName = "doc_no";
            this.colDocNo.Name = "colDocNo";
            this.colDocNo.OptionsColumn.AllowEdit = false;
            this.colDocNo.Visible = true;
            this.colDocNo.VisibleIndex = 4;
            this.colDocNo.Width = 110;
            // 
            // colAppTitle
            // 
            this.colAppTitle.Caption = "제목";
            this.colAppTitle.FieldName = "app_title";
            this.colAppTitle.Name = "colAppTitle";
            this.colAppTitle.OptionsColumn.AllowEdit = false;
            this.colAppTitle.Visible = true;
            this.colAppTitle.VisibleIndex = 5;
            this.colAppTitle.Width = 260;
            // 
            // colReqEmpNm
            // 
            this.colReqEmpNm.AppearanceCell.Options.UseTextOptions = true;
            this.colReqEmpNm.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colReqEmpNm.AppearanceHeader.Options.UseTextOptions = true;
            this.colReqEmpNm.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colReqEmpNm.Caption = "기안자";
            this.colReqEmpNm.FieldName = "req_emp_nm";
            this.colReqEmpNm.Name = "colReqEmpNm";
            this.colReqEmpNm.OptionsColumn.AllowEdit = false;
            this.colReqEmpNm.Visible = true;
            this.colReqEmpNm.VisibleIndex = 6;
            this.colReqEmpNm.Width = 80;
            // 
            // colStatText
            // 
            this.colStatText.AppearanceCell.Options.UseTextOptions = true;
            this.colStatText.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colStatText.AppearanceHeader.Options.UseTextOptions = true;
            this.colStatText.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colStatText.Caption = "결재진행상태";
            this.colStatText.FieldName = "stat_text";
            this.colStatText.Name = "colStatText";
            this.colStatText.OptionsColumn.AllowEdit = false;
            this.colStatText.Visible = true;
            this.colStatText.VisibleIndex = 7;
            this.colStatText.Width = 100;
            // 
            // colCurApprEmpNm
            // 
            this.colCurApprEmpNm.AppearanceCell.Options.UseTextOptions = true;
            this.colCurApprEmpNm.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCurApprEmpNm.AppearanceHeader.Options.UseTextOptions = true;
            this.colCurApprEmpNm.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCurApprEmpNm.Caption = "결재대기자";
            this.colCurApprEmpNm.FieldName = "cur_appr_emp_nm";
            this.colCurApprEmpNm.Name = "colCurApprEmpNm";
            this.colCurApprEmpNm.OptionsColumn.AllowEdit = false;
            this.colCurApprEmpNm.Visible = true;
            this.colCurApprEmpNm.VisibleIndex = 8;
            this.colCurApprEmpNm.Width = 90;
            // 
            // colLastApprEmpNm
            // 
            this.colLastApprEmpNm.AppearanceCell.Options.UseTextOptions = true;
            this.colLastApprEmpNm.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colLastApprEmpNm.AppearanceHeader.Options.UseTextOptions = true;
            this.colLastApprEmpNm.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colLastApprEmpNm.Caption = "최종승인자";
            this.colLastApprEmpNm.FieldName = "last_appr_emp_nm";
            this.colLastApprEmpNm.Name = "colLastApprEmpNm";
            this.colLastApprEmpNm.OptionsColumn.AllowEdit = false;
            this.colLastApprEmpNm.Visible = true;
            this.colLastApprEmpNm.VisibleIndex = 9;
            this.colLastApprEmpNm.Width = 90;
            // 
            // panSubRow
            // 
            this.panSubRow.Controls.Add(this.tabSub);
            this.panSubRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSubRow.Location = new System.Drawing.Point(0, 83);
            this.panSubRow.Name = "panSubRow";
            this.panSubRow.Size = new System.Drawing.Size(1144, 25);
            this.panSubRow.TabIndex = 1;
            // 
            // tabSub
            // 
            this.tabSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSub.Location = new System.Drawing.Point(0, 0);
            this.tabSub.Name = "tabSub";
            this.tabSub.SelectedTabPage = this.tabPageSubPending;
            this.tabSub.Size = new System.Drawing.Size(1144, 25);
            this.tabSub.TabIndex = 0;
            this.tabSub.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabPageSubPending,
            this.tabPageSubRejected,
            this.tabPageSubDone});
            this.tabSub.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.tabSub_SelectedPageChanged);
            // 
            // tabPageSubPending
            // 
            this.tabPageSubPending.Name = "tabPageSubPending";
            this.tabPageSubPending.Size = new System.Drawing.Size(1142, 0);
            this.tabPageSubPending.Text = " 미 결 재    ";
            // 
            // tabPageSubRejected
            // 
            this.tabPageSubRejected.Name = "tabPageSubRejected";
            this.tabPageSubRejected.Size = new System.Drawing.Size(1142, 0);
            this.tabPageSubRejected.Text = " 반    려    ";
            // 
            // tabPageSubDone
            // 
            this.tabPageSubDone.Name = "tabPageSubDone";
            this.tabPageSubDone.Size = new System.Drawing.Size(1142, 0);
            this.tabPageSubDone.Text = " 결 재  완 료     ";
            // 
            // panListGap
            // 
            this.panListGap.Dock = System.Windows.Forms.DockStyle.Top;
            this.panListGap.Location = new System.Drawing.Point(0, 75);
            this.panListGap.Name = "panListGap";
            this.panListGap.Size = new System.Drawing.Size(1144, 8);
            this.panListGap.TabIndex = 2;
            // 
            // panSearch
            // 
            this.panSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(245)))), ((int)(((byte)(248)))));
            this.panSearch.Controls.Add(this.lblSearchAcc);
            this.panSearch.Controls.Add(this.cboSearchAccId);
            this.panSearch.Controls.Add(this.lblSearchDocType);
            this.panSearch.Controls.Add(this.cboSearchDocType);
            this.panSearch.Controls.Add(this.lblSearchDate);
            this.panSearch.Controls.Add(this.dteSearchFrom);
            this.panSearch.Controls.Add(this.lblSearchTilde);
            this.panSearch.Controls.Add(this.dteSearchTo);
            this.panSearch.Controls.Add(this.lblSearchTitle);
            this.panSearch.Controls.Add(this.txtSearchTitle);
            this.panSearch.Controls.Add(this.lblSearchReqEmp);
            this.panSearch.Controls.Add(this.popSearchReqEmp);
            this.panSearch.Controls.Add(this.txtSearchReqEmpNo);
            this.panSearch.Controls.Add(this.btnSearch);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(0, 3);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(1144, 72);
            this.panSearch.TabIndex = 3;
            this.panSearch.Paint += new System.Windows.Forms.PaintEventHandler(this.panSearch_Paint);
            // 
            // lblSearchAcc
            // 
            this.lblSearchAcc.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSearchAcc.Appearance.Options.UseFont = true;
            this.lblSearchAcc.Location = new System.Drawing.Point(12, 13);
            this.lblSearchAcc.Name = "lblSearchAcc";
            this.lblSearchAcc.Size = new System.Drawing.Size(36, 15);
            this.lblSearchAcc.TabIndex = 0;
            this.lblSearchAcc.Text = "사업장";
            // 
            // cboSearchAccId
            // 
            this.cboSearchAccId.EditValue = "";
            this.cboSearchAccId.Location = new System.Drawing.Point(56, 10);
            this.cboSearchAccId.LookupKey = "L_ACC";
            this.cboSearchAccId.Name = "cboSearchAccId";
            this.cboSearchAccId.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.cboSearchAccId.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboSearchAccId.Properties.Appearance.Options.UseBackColor = true;
            this.cboSearchAccId.Properties.Appearance.Options.UseForeColor = true;
            this.cboSearchAccId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboSearchAccId.Properties.NullText = "";
            this.cboSearchAccId.Required = true;
            this.cboSearchAccId.Size = new System.Drawing.Size(131, 20);
            this.cboSearchAccId.TabIndex = 0;
            // 
            // lblSearchDocType
            // 
            this.lblSearchDocType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSearchDocType.Appearance.Options.UseFont = true;
            this.lblSearchDocType.Location = new System.Drawing.Point(203, 13);
            this.lblSearchDocType.Name = "lblSearchDocType";
            this.lblSearchDocType.Size = new System.Drawing.Size(48, 15);
            this.lblSearchDocType.TabIndex = 1;
            this.lblSearchDocType.Text = "문서구분";
            // 
            // cboSearchDocType
            // 
            this.cboSearchDocType.EditValue = "";
            this.cboSearchDocType.Location = new System.Drawing.Point(259, 10);
            this.cboSearchDocType.LookupKey = "L_AP0002";
            this.cboSearchDocType.Name = "cboSearchDocType";
            this.cboSearchDocType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboSearchDocType.Properties.NullText = "";
            this.cboSearchDocType.Size = new System.Drawing.Size(140, 20);
            this.cboSearchDocType.TabIndex = 1;
            // 
            // lblSearchDate
            // 
            this.lblSearchDate.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSearchDate.Appearance.Options.UseFont = true;
            this.lblSearchDate.Location = new System.Drawing.Point(415, 13);
            this.lblSearchDate.Name = "lblSearchDate";
            this.lblSearchDate.Size = new System.Drawing.Size(48, 15);
            this.lblSearchDate.TabIndex = 2;
            this.lblSearchDate.Text = "기안일자";
            // 
            // dteSearchFrom
            // 
            this.dteSearchFrom.EditValue = null;
            this.dteSearchFrom.Location = new System.Drawing.Point(471, 10);
            this.dteSearchFrom.Name = "dteSearchFrom";
            this.dteSearchFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteSearchFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteSearchFrom.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dteSearchFrom.Size = new System.Drawing.Size(100, 20);
            this.dteSearchFrom.TabIndex = 2;
            this.dteSearchFrom.YyyyMmDd = null;
            // 
            // lblSearchTilde
            // 
            this.lblSearchTilde.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSearchTilde.Appearance.Options.UseFont = true;
            this.lblSearchTilde.Location = new System.Drawing.Point(576, 13);
            this.lblSearchTilde.Name = "lblSearchTilde";
            this.lblSearchTilde.Size = new System.Drawing.Size(8, 15);
            this.lblSearchTilde.TabIndex = 3;
            this.lblSearchTilde.Text = "~";
            // 
            // dteSearchTo
            // 
            this.dteSearchTo.EditValue = null;
            this.dteSearchTo.Location = new System.Drawing.Point(588, 10);
            this.dteSearchTo.Name = "dteSearchTo";
            this.dteSearchTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteSearchTo.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteSearchTo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dteSearchTo.Size = new System.Drawing.Size(100, 20);
            this.dteSearchTo.TabIndex = 3;
            this.dteSearchTo.YyyyMmDd = null;
            // 
            // lblSearchTitle
            // 
            this.lblSearchTitle.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSearchTitle.Appearance.Options.UseFont = true;
            this.lblSearchTitle.Location = new System.Drawing.Point(12, 43);
            this.lblSearchTitle.Name = "lblSearchTitle";
            this.lblSearchTitle.Size = new System.Drawing.Size(24, 15);
            this.lblSearchTitle.TabIndex = 4;
            this.lblSearchTitle.Text = "제목";
            // 
            // txtSearchTitle
            // 
            this.txtSearchTitle.Location = new System.Drawing.Point(56, 40);
            this.txtSearchTitle.Name = "txtSearchTitle";
            this.txtSearchTitle.Size = new System.Drawing.Size(343, 20);
            this.txtSearchTitle.TabIndex = 4;
            this.txtSearchTitle.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchTitle_KeyDown);
            // 
            // lblSearchReqEmp
            // 
            this.lblSearchReqEmp.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSearchReqEmp.Appearance.Options.UseFont = true;
            this.lblSearchReqEmp.Location = new System.Drawing.Point(415, 43);
            this.lblSearchReqEmp.Name = "lblSearchReqEmp";
            this.lblSearchReqEmp.Size = new System.Drawing.Size(36, 15);
            this.lblSearchReqEmp.TabIndex = 5;
            this.lblSearchReqEmp.Text = "기안자";
            // 
            // popSearchReqEmp
            // 
            this.popSearchReqEmp.Location = new System.Drawing.Point(471, 40);
            this.popSearchReqEmp.LookupKey = "P_EMP";
            this.popSearchReqEmp.MatchField = "emp_nm";
            this.popSearchReqEmp.Name = "popSearchReqEmp";
            this.popSearchReqEmp.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", 24, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.popSearchReqEmp.Size = new System.Drawing.Size(217, 20);
            this.popSearchReqEmp.TabIndex = 5;
            this.popSearchReqEmp.ToolTip = null;
            // 
            // txtSearchReqEmpNo
            // 
            this.txtSearchReqEmpNo.Location = new System.Drawing.Point(700, 66);
            this.txtSearchReqEmpNo.Name = "txtSearchReqEmpNo";
            this.txtSearchReqEmpNo.Size = new System.Drawing.Size(50, 20);
            this.txtSearchReqEmpNo.TabIndex = 7;
            this.txtSearchReqEmpNo.Visible = false;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.FillColor = System.Drawing.Color.White;
            this.btnSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnSearch.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnSearch.Image = null;
            this.btnSearch.Location = new System.Drawing.Point(706, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnSearch.Size = new System.Drawing.Size(92, 50);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "검색";
            this.btnSearch.ToolTip = null;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panComposeView
            // 
            this.panComposeView.Controls.Add(this.flpTiles);
            this.panComposeView.Controls.Add(this.lblComposeHint);
            this.panComposeView.Controls.Add(this.flpChips);
            this.panComposeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panComposeView.Location = new System.Drawing.Point(0, 0);
            this.panComposeView.Name = "panComposeView";
            this.panComposeView.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.panComposeView.Size = new System.Drawing.Size(1144, 427);
            this.panComposeView.TabIndex = 1;
            // 
            // flpTiles
            // 
            this.flpTiles.AutoScroll = true;
            this.flpTiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTiles.Location = new System.Drawing.Point(0, 54);
            this.flpTiles.Name = "flpTiles";
            this.flpTiles.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.flpTiles.Size = new System.Drawing.Size(1144, 349);
            this.flpTiles.TabIndex = 0;
            // 
            // lblComposeHint
            // 
            this.lblComposeHint.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblComposeHint.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(172)))), ((int)(((byte)(178)))));
            this.lblComposeHint.Appearance.Options.UseFont = true;
            this.lblComposeHint.Appearance.Options.UseForeColor = true;
            this.lblComposeHint.Appearance.Options.UseTextOptions = true;
            this.lblComposeHint.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lblComposeHint.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblComposeHint.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblComposeHint.Location = new System.Drawing.Point(0, 403);
            this.lblComposeHint.Name = "lblComposeHint";
            this.lblComposeHint.Size = new System.Drawing.Size(1144, 24);
            this.lblComposeHint.TabIndex = 1;
            this.lblComposeHint.Text = "작성할 문서를 더블클릭하면 신규 작성 화면으로 이동합니다.";
            // 
            // flpChips
            // 
            this.flpChips.Dock = System.Windows.Forms.DockStyle.Top;
            this.flpChips.Location = new System.Drawing.Point(0, 12);
            this.flpChips.Name = "flpChips";
            this.flpChips.Size = new System.Drawing.Size(1144, 42);
            this.flpChips.TabIndex = 2;
            this.flpChips.WrapContents = false;
            // 
            // panTabRow
            // 
            this.panTabRow.Controls.Add(this.tabMain);
            this.panTabRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTabRow.Location = new System.Drawing.Point(16, 12);
            this.panTabRow.Name = "panTabRow";
            this.panTabRow.Size = new System.Drawing.Size(1144, 25);
            this.panTabRow.TabIndex = 1;
            // 
            // tabMain
            // 
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.tabMain.SelectedTabPage = this.tabPageDrafted;
            this.tabMain.Size = new System.Drawing.Size(1144, 25);
            this.tabMain.TabIndex = 0;
            this.tabMain.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabPageDrafted,
            this.tabPageInbox,
            this.tabPageCompose});
            this.tabMain.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.tabMain_SelectedPageChanged);
            // 
            // tabPageDrafted
            // 
            this.tabPageDrafted.Name = "tabPageDrafted";
            this.tabPageDrafted.Size = new System.Drawing.Size(1142, 0);
            this.tabPageDrafted.Text = " 기 안 함    ";
            // 
            // tabPageInbox
            // 
            this.tabPageInbox.Name = "tabPageInbox";
            this.tabPageInbox.Size = new System.Drawing.Size(1142, 0);
            this.tabPageInbox.Text = " 결 재 함    ";
            // 
            // tabPageCompose
            // 
            this.tabPageCompose.Name = "tabPageCompose";
            this.tabPageCompose.Size = new System.Drawing.Size(1142, 0);
            this.tabPageCompose.Text = " 기 안 서   작 성    ";
            // 
            // panColGap
            // 
            this.panColGap.Dock = System.Windows.Forms.DockStyle.Right;
            this.panColGap.Location = new System.Drawing.Point(1176, 0);
            this.panColGap.Name = "panColGap";
            this.panColGap.Size = new System.Drawing.Size(16, 854);
            this.panColGap.TabIndex = 1;
            // 
            // panRight
            // 
            this.panRight.Controls.Add(this.panSchedule);
            this.panRight.Controls.Add(this.panScheduleGap);
            this.panRight.Controls.Add(this.panQuickLaunch);
            this.panRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panRight.Location = new System.Drawing.Point(1192, 0);
            this.panRight.Name = "panRight";
            this.panRight.Size = new System.Drawing.Size(320, 854);
            this.panRight.TabIndex = 2;
            // 
            // panSchedule
            // 
            this.panSchedule.Controls.Add(this.panScheduleList);
            this.panSchedule.Controls.Add(this.panScheduleHeader);
            this.panSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panSchedule.Location = new System.Drawing.Point(0, 0);
            this.panSchedule.Name = "panSchedule";
            this.panSchedule.Padding = new System.Windows.Forms.Padding(16);
            this.panSchedule.Size = new System.Drawing.Size(320, 714);
            this.panSchedule.TabIndex = 0;
            // 
            // panScheduleList
            // 
            this.panScheduleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panScheduleList.Location = new System.Drawing.Point(16, 40);
            this.panScheduleList.Name = "panScheduleList";
            this.panScheduleList.Size = new System.Drawing.Size(288, 658);
            this.panScheduleList.TabIndex = 0;
            // 
            // panScheduleHeader
            // 
            this.panScheduleHeader.Controls.Add(this.pnlScheduleTitle);
            this.panScheduleHeader.Controls.Add(this.lblScheduleDate);
            this.panScheduleHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panScheduleHeader.Location = new System.Drawing.Point(16, 16);
            this.panScheduleHeader.Name = "panScheduleHeader";
            this.panScheduleHeader.Size = new System.Drawing.Size(288, 24);
            this.panScheduleHeader.TabIndex = 1;
            // 
            // pnlScheduleTitle
            // 
            this.pnlScheduleTitle.Controls.Add(this.dotSchedule);
            this.pnlScheduleTitle.Controls.Add(this.lblScheduleTitle);
            this.pnlScheduleTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlScheduleTitle.Name = "pnlScheduleTitle";
            this.pnlScheduleTitle.Size = new System.Drawing.Size(90, 24);
            this.pnlScheduleTitle.TabIndex = 0;
            // 
            // dotSchedule
            // 
            this.dotSchedule.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(125)))));
            this.dotSchedule.Location = new System.Drawing.Point(0, 8);
            this.dotSchedule.Name = "dotSchedule";
            this.dotSchedule.Size = new System.Drawing.Size(8, 8);
            this.dotSchedule.TabIndex = 0;
            this.dotSchedule.Paint += new System.Windows.Forms.PaintEventHandler(this.SectionDot_Paint);
            // 
            // lblScheduleTitle
            // 
            this.lblScheduleTitle.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblScheduleTitle.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.lblScheduleTitle.Appearance.Options.UseFont = true;
            this.lblScheduleTitle.Appearance.Options.UseForeColor = true;
            this.lblScheduleTitle.Location = new System.Drawing.Point(14, 3);
            this.lblScheduleTitle.Name = "lblScheduleTitle";
            this.lblScheduleTitle.Size = new System.Drawing.Size(64, 15);
            this.lblScheduleTitle.TabIndex = 1;
            this.lblScheduleTitle.Text = "오늘의 일정";
            // 
            // lblScheduleDate
            // 
            this.lblScheduleDate.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblScheduleDate.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(132)))), ((int)(((byte)(138)))));
            this.lblScheduleDate.Appearance.Options.UseFont = true;
            this.lblScheduleDate.Appearance.Options.UseForeColor = true;
            this.lblScheduleDate.Appearance.Options.UseTextOptions = true;
            this.lblScheduleDate.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblScheduleDate.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblScheduleDate.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblScheduleDate.Location = new System.Drawing.Point(208, 0);
            this.lblScheduleDate.Name = "lblScheduleDate";
            this.lblScheduleDate.Size = new System.Drawing.Size(80, 24);
            this.lblScheduleDate.TabIndex = 1;
            this.lblScheduleDate.Text = "MM/dd";
            // 
            // panScheduleGap
            // 
            this.panScheduleGap.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panScheduleGap.Location = new System.Drawing.Point(0, 714);
            this.panScheduleGap.Name = "panScheduleGap";
            this.panScheduleGap.Size = new System.Drawing.Size(320, 16);
            this.panScheduleGap.TabIndex = 1;
            // 
            // panQuickLaunch
            // 
            this.panQuickLaunch.Controls.Add(this.panQuickTiles);
            this.panQuickLaunch.Controls.Add(this.panQuickTitleGap);
            this.panQuickLaunch.Controls.Add(this.pnlQuickTitle);
            this.panQuickLaunch.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panQuickLaunch.Location = new System.Drawing.Point(0, 730);
            this.panQuickLaunch.Name = "panQuickLaunch";
            this.panQuickLaunch.Padding = new System.Windows.Forms.Padding(16);
            this.panQuickLaunch.Size = new System.Drawing.Size(320, 124);
            this.panQuickLaunch.TabIndex = 2;
            // 
            // panQuickTiles
            // 
            this.panQuickTiles.Controls.Add(this.tileQuickSchedule);
            this.panQuickTiles.Controls.Add(this.panQuickGap2);
            this.panQuickTiles.Controls.Add(this.tileQuickBoard);
            this.panQuickTiles.Controls.Add(this.panQuickGap1);
            this.panQuickTiles.Controls.Add(this.tileQuickAppr);
            this.panQuickTiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panQuickTiles.Location = new System.Drawing.Point(16, 48);
            this.panQuickTiles.Name = "panQuickTiles";
            this.panQuickTiles.Size = new System.Drawing.Size(288, 60);
            this.panQuickTiles.TabIndex = 0;
            // 
            // tileQuickSchedule
            // 
            this.tileQuickSchedule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.tileQuickSchedule.Controls.Add(this.lblQuickSchedule);
            this.tileQuickSchedule.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tileQuickSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tileQuickSchedule.Location = new System.Drawing.Point(208, 0);
            this.tileQuickSchedule.Name = "tileQuickSchedule";
            this.tileQuickSchedule.Size = new System.Drawing.Size(80, 60);
            this.tileQuickSchedule.TabIndex = 0;
            this.tileQuickSchedule.Tag = "SM|frmSchedule";
            this.tileQuickSchedule.Click += new System.EventHandler(this.QuickLaunch_Click);
            // 
            // lblQuickSchedule
            // 
            this.lblQuickSchedule.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblQuickSchedule.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.lblQuickSchedule.Appearance.Options.UseFont = true;
            this.lblQuickSchedule.Appearance.Options.UseForeColor = true;
            this.lblQuickSchedule.Appearance.Options.UseTextOptions = true;
            this.lblQuickSchedule.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblQuickSchedule.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lblQuickSchedule.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblQuickSchedule.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblQuickSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblQuickSchedule.Location = new System.Drawing.Point(0, 0);
            this.lblQuickSchedule.Name = "lblQuickSchedule";
            this.lblQuickSchedule.Size = new System.Drawing.Size(80, 60);
            this.lblQuickSchedule.TabIndex = 0;
            this.lblQuickSchedule.Tag = "SM|frmSchedule";
            this.lblQuickSchedule.Text = "일정관리";
            this.lblQuickSchedule.Click += new System.EventHandler(this.QuickLaunch_Click);
            // 
            // panQuickGap2
            // 
            this.panQuickGap2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panQuickGap2.Location = new System.Drawing.Point(200, 0);
            this.panQuickGap2.Name = "panQuickGap2";
            this.panQuickGap2.Size = new System.Drawing.Size(8, 60);
            this.panQuickGap2.TabIndex = 1;
            // 
            // tileQuickBoard
            // 
            this.tileQuickBoard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.tileQuickBoard.Controls.Add(this.lblQuickBoard);
            this.tileQuickBoard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tileQuickBoard.Dock = System.Windows.Forms.DockStyle.Left;
            this.tileQuickBoard.Location = new System.Drawing.Point(104, 0);
            this.tileQuickBoard.Name = "tileQuickBoard";
            this.tileQuickBoard.Size = new System.Drawing.Size(96, 60);
            this.tileQuickBoard.TabIndex = 2;
            this.tileQuickBoard.Tag = "SM|frmBoard";
            this.tileQuickBoard.Click += new System.EventHandler(this.QuickLaunch_Click);
            // 
            // lblQuickBoard
            // 
            this.lblQuickBoard.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblQuickBoard.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.lblQuickBoard.Appearance.Options.UseFont = true;
            this.lblQuickBoard.Appearance.Options.UseForeColor = true;
            this.lblQuickBoard.Appearance.Options.UseTextOptions = true;
            this.lblQuickBoard.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblQuickBoard.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lblQuickBoard.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblQuickBoard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblQuickBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblQuickBoard.Location = new System.Drawing.Point(0, 0);
            this.lblQuickBoard.Name = "lblQuickBoard";
            this.lblQuickBoard.Size = new System.Drawing.Size(96, 60);
            this.lblQuickBoard.TabIndex = 0;
            this.lblQuickBoard.Tag = "SM|frmBoard";
            this.lblQuickBoard.Text = "공지등록";
            this.lblQuickBoard.Click += new System.EventHandler(this.QuickLaunch_Click);
            // 
            // panQuickGap1
            // 
            this.panQuickGap1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panQuickGap1.Location = new System.Drawing.Point(96, 0);
            this.panQuickGap1.Name = "panQuickGap1";
            this.panQuickGap1.Size = new System.Drawing.Size(8, 60);
            this.panQuickGap1.TabIndex = 3;
            // 
            // tileQuickAppr
            // 
            this.tileQuickAppr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.tileQuickAppr.Controls.Add(this.lblQuickAppr);
            this.tileQuickAppr.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tileQuickAppr.Dock = System.Windows.Forms.DockStyle.Left;
            this.tileQuickAppr.Location = new System.Drawing.Point(0, 0);
            this.tileQuickAppr.Name = "tileQuickAppr";
            this.tileQuickAppr.Size = new System.Drawing.Size(96, 60);
            this.tileQuickAppr.TabIndex = 4;
            this.tileQuickAppr.Tag = "AP|frmApprInbox";
            this.tileQuickAppr.Click += new System.EventHandler(this.QuickLaunch_Click);
            // 
            // lblQuickAppr
            // 
            this.lblQuickAppr.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblQuickAppr.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.lblQuickAppr.Appearance.Options.UseFont = true;
            this.lblQuickAppr.Appearance.Options.UseForeColor = true;
            this.lblQuickAppr.Appearance.Options.UseTextOptions = true;
            this.lblQuickAppr.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblQuickAppr.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lblQuickAppr.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblQuickAppr.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblQuickAppr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblQuickAppr.Location = new System.Drawing.Point(0, 0);
            this.lblQuickAppr.Name = "lblQuickAppr";
            this.lblQuickAppr.Size = new System.Drawing.Size(96, 60);
            this.lblQuickAppr.TabIndex = 0;
            this.lblQuickAppr.Tag = "AP|frmApprInbox";
            this.lblQuickAppr.Text = "전자결재함";
            this.lblQuickAppr.Click += new System.EventHandler(this.QuickLaunch_Click);
            // 
            // panQuickTitleGap
            // 
            this.panQuickTitleGap.Dock = System.Windows.Forms.DockStyle.Top;
            this.panQuickTitleGap.Location = new System.Drawing.Point(16, 40);
            this.panQuickTitleGap.Name = "panQuickTitleGap";
            this.panQuickTitleGap.Size = new System.Drawing.Size(288, 8);
            this.panQuickTitleGap.TabIndex = 1;
            // 
            // pnlQuickTitle
            // 
            this.pnlQuickTitle.Controls.Add(this.dotQuick);
            this.pnlQuickTitle.Controls.Add(this.lblQuickTitle);
            this.pnlQuickTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlQuickTitle.Location = new System.Drawing.Point(16, 16);
            this.pnlQuickTitle.Name = "pnlQuickTitle";
            this.pnlQuickTitle.Size = new System.Drawing.Size(288, 24);
            this.pnlQuickTitle.TabIndex = 2;
            // 
            // dotQuick
            // 
            this.dotQuick.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(142)))), ((int)(((byte)(247)))));
            this.dotQuick.Location = new System.Drawing.Point(0, 8);
            this.dotQuick.Name = "dotQuick";
            this.dotQuick.Size = new System.Drawing.Size(8, 8);
            this.dotQuick.TabIndex = 0;
            this.dotQuick.Paint += new System.Windows.Forms.PaintEventHandler(this.SectionDot_Paint);
            // 
            // lblQuickTitle
            // 
            this.lblQuickTitle.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblQuickTitle.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.lblQuickTitle.Appearance.Options.UseFont = true;
            this.lblQuickTitle.Appearance.Options.UseForeColor = true;
            this.lblQuickTitle.Location = new System.Drawing.Point(14, 3);
            this.lblQuickTitle.Name = "lblQuickTitle";
            this.lblQuickTitle.Size = new System.Drawing.Size(52, 15);
            this.lblQuickTitle.TabIndex = 1;
            this.lblQuickTitle.Text = "빠른 실행";
            // 
            // panelWyn1
            // 
            this.panelWyn1.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn1.Appearance.Options.UseBackColor = true;
            this.panelWyn1.Controls.Add(this.grdApproval);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(0, 108);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.panelWyn1.Size = new System.Drawing.Size(1144, 319);
            this.panelWyn1.TabIndex = 4;
            // 
            // HomeForm
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1560, 900);
            this.Controls.Add(this.panScroll);
            this.Name = "HomeForm";
            this.Text = "Home";
            this.Load += new System.EventHandler(this.HomeForm_Load);
            this.panScroll.ResumeLayout(false);
            this.panScroll.PerformLayout();
            this.panContent.ResumeLayout(false);
            this.panMain.ResumeLayout(false);
            this.panLeft.ResumeLayout(false);
            this.panNotice.ResumeLayout(false);
            this.panNoticeFrame.ResumeLayout(false);
            this.panNoticeHead.ResumeLayout(false);
            this.panNoticeHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabNotice)).EndInit();
            this.tabNotice.ResumeLayout(false);
            this.panApproval.ResumeLayout(false);
            this.panApprovalBody.ResumeLayout(false);
            this.panListView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdApproval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwApproval)).EndInit();
            this.panSubRow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabSub)).EndInit();
            this.tabSub.ResumeLayout(false);
            this.panSearch.ResumeLayout(false);
            this.panSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchDocType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteSearchTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchTitle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popSearchReqEmp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchReqEmpNo.Properties)).EndInit();
            this.panComposeView.ResumeLayout(false);
            this.panTabRow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.panRight.ResumeLayout(false);
            this.panSchedule.ResumeLayout(false);
            this.panScheduleHeader.ResumeLayout(false);
            this.pnlScheduleTitle.ResumeLayout(false);
            this.pnlScheduleTitle.PerformLayout();
            this.panQuickLaunch.ResumeLayout(false);
            this.panQuickTiles.ResumeLayout(false);
            this.tileQuickSchedule.ResumeLayout(false);
            this.tileQuickBoard.ResumeLayout(false);
            this.tileQuickAppr.ResumeLayout(false);
            this.pnlQuickTitle.ResumeLayout(false);
            this.pnlQuickTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private System.Windows.Forms.Panel panScroll;
    private System.Windows.Forms.Panel panContent;
    private System.Windows.Forms.Panel panMain;
    private System.Windows.Forms.Panel panLeft;
    private System.Windows.Forms.Panel panColGap;
    private System.Windows.Forms.Panel panRight;
    private System.Windows.Forms.Panel panGap2;
    private System.Windows.Forms.Panel panApproval;
    private System.Windows.Forms.Panel panApprovalBody;
    private System.Windows.Forms.Panel panListView;
    private GridControlWyn grdApproval;
    private GridViewWyn gvwApproval;
    private GridColumn colKind;
    private GridColumn colAppNo;
    private GridColumn colReqDt;
    private GridColumn colDocTypeNm;
    private GridColumn colDocNo;
    private GridColumn colAppTitle;
    private GridColumn colReqEmpNm;
    private GridColumn colStatText;
    private GridColumn colCurApprEmpNm;
    private GridColumn colLastApprEmpNm;
    private System.Windows.Forms.Panel panSubRow;
    private XtraTabControl tabSub;
    private XtraTabPage tabPageSubPending;
    private XtraTabPage tabPageSubRejected;
    private XtraTabPage tabPageSubDone;
    private System.Windows.Forms.Panel panListGap;
    private System.Windows.Forms.Panel panSearch;
    private LabelControl lblSearchAcc;
    private LookUpEditWyn cboSearchAccId;
    private LabelControl lblSearchDocType;
    private LookUpEditWyn cboSearchDocType;
    private LabelControl lblSearchDate;
    private DateEditWyn dteSearchFrom;
    private LabelControl lblSearchTilde;
    private DateEditWyn dteSearchTo;
    private LabelControl lblSearchTitle;
    private TextEditWyn txtSearchTitle;
    private LabelControl lblSearchReqEmp;
    private PopupLookupEditWyn popSearchReqEmp;
    private TextEditWyn txtSearchReqEmpNo;
    private ButtonWyn btnSearch;
    private System.Windows.Forms.Panel panComposeView;
    private System.Windows.Forms.FlowLayoutPanel flpTiles;
    private LabelControl lblComposeHint;
    private System.Windows.Forms.FlowLayoutPanel flpChips;
    private System.Windows.Forms.Panel panTabRow;
    private XtraTabControl tabMain;
    private XtraTabPage tabPageDrafted;
    private XtraTabPage tabPageInbox;
    private XtraTabPage tabPageCompose;
    private System.Windows.Forms.Panel panNotice;
    private System.Windows.Forms.Panel panNoticeFrame;
    private System.Windows.Forms.Panel panNoticeList;
    private System.Windows.Forms.Panel panNoticeHead;
    private LabelControl lblNoticeColTitle;
    private LabelControl lblNoticeColAuthor;
    private LabelControl lblNoticeColDate;
    private LabelControl lblNoticeColImportant;
    private System.Windows.Forms.Panel panNoticeHeadMarginL;
    private System.Windows.Forms.Panel panNoticeHeadMarginR;
    private System.Windows.Forms.Panel panNoticeHeadLine;
    private System.Windows.Forms.Panel panNoticeSpacer;
    private System.Windows.Forms.Panel panNoticeHeader;
    private XtraTabControl tabNotice;
    private XtraTabPage tabPageNotice;
    private LabelControl lblNoticeAdd;
    private System.Windows.Forms.Panel panSchedule;
    private System.Windows.Forms.Panel panScheduleList;
    private System.Windows.Forms.Panel panScheduleHeader;
    private System.Windows.Forms.Panel pnlScheduleTitle;
    private System.Windows.Forms.Panel dotSchedule;
    private LabelControl lblScheduleTitle;
    private LabelControl lblScheduleDate;
    private System.Windows.Forms.Panel panScheduleGap;
    private System.Windows.Forms.Panel panQuickLaunch;
    private System.Windows.Forms.Panel panQuickTiles;
    private System.Windows.Forms.Panel tileQuickAppr;
    private LabelControl lblQuickAppr;
    private System.Windows.Forms.Panel panQuickGap1;
    private System.Windows.Forms.Panel tileQuickBoard;
    private LabelControl lblQuickBoard;
    private System.Windows.Forms.Panel panQuickGap2;
    private System.Windows.Forms.Panel tileQuickSchedule;
    private LabelControl lblQuickSchedule;
    private System.Windows.Forms.Panel panQuickTitleGap;
    private System.Windows.Forms.Panel pnlQuickTitle;
    private System.Windows.Forms.Panel dotQuick;
    private LabelControl lblQuickTitle;
    private PanelWyn panelWyn1;
}
