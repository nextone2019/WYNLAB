// AI Builder가 마스터-폼-서브그리드 템플릿을 복제해서 자동 생성 - 2026-09-17.
// 2026-09-17에 마스터 목록(grd1)을 DevExpress SchedulerControl(월간 캘린더)로 손으로 바꿨다 -
// "일정관리를 카렌더 형태로" 요청. 그 외 구조(검색조건/상세폼/버튼)는 템플릿 그대로.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.SM;

public partial class frmSchedule
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSchedule));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
        this.lblDetailScheduleId = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailScheduleId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDetailAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboDetailAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblDetailTitle = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailTitle = new WYNLAB.Base.Controls.TextEditWyn();
        this.chkDetailAllDay = new WYNLAB.Base.Controls.CheckBoxWyn();
        this.lblDetailAllDay = new DevExpress.XtraEditors.LabelControl();
        this.lblDetailStartDt = new DevExpress.XtraEditors.LabelControl();
        this.dteDetailStartDt = new WYNLAB.Base.Controls.DateEditWyn();
        this.txtDetailStartTm = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDetailEndDt = new DevExpress.XtraEditors.LabelControl();
        this.dteDetailEndDt = new WYNLAB.Base.Controls.DateEditWyn();
        this.txtDetailEndTm = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDetailColor = new DevExpress.XtraEditors.LabelControl();
        this.panColorSwatches = new System.Windows.Forms.Panel();
        this.btnDetailClose = new WYNLAB.Base.Controls.ButtonWyn();
        this.lblDetailContent = new DevExpress.XtraEditors.LabelControl();
        this.memDetailContent = new WYNLAB.Base.Controls.MemoEditWyn();
        this.lblDetailRegDt = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailRegDt = new WYNLAB.Base.Controls.TextEditWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.schedulerControl1 = new DevExpress.XtraScheduler.SchedulerControl();
            this.schedulerStorage1 = new DevExpress.XtraScheduler.SchedulerStorage();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
            this.panelWyn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
            this.panelWyn4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
            this.panelWyn6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).BeginInit();
            this.panelWyn8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerStorage1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
            this.panelWyn2.SuspendLayout();
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
            this.panBase.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.panBase.Size = new System.Drawing.Size(1245, 580);
            this.panBase.TabIndex = 6;
            //
            // panelWyn3
            //
            this.panelWyn3.Controls.Add(this.panelWyn4);
            this.panelWyn3.Controls.Add(this.splitterWyn1);
            this.panelWyn3.Controls.Add(this.panelWyn8);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(5, 82);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Size = new System.Drawing.Size(1235, 493);
            this.panelWyn3.TabIndex = 7;
            //
            // panelWyn4
            //
            this.panelWyn4.Controls.Add(this.panelWyn5);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(770, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelWyn4.Size = new System.Drawing.Size(465, 493);
            this.panelWyn4.TabIndex = 7;
            //
            // panelWyn5
            //
            this.panelWyn5.Controls.Add(this.panData);
            this.panelWyn5.Controls.Add(this.panelWyn6);
            this.panelWyn5.Controls.Add(this.btnDetailClose);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(3, 0);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Size = new System.Drawing.Size(462, 493);
            this.panelWyn5.TabIndex = 6;
            //
            // panData
            //
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(462, 466);
            this.panData.TabIndex = 8;
            //
            // lblDetailSample1
            //
        this.lblDetailScheduleId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailScheduleId.Appearance.Options.UseFont = true;
        this.lblDetailScheduleId.Location = new System.Drawing.Point(16, 19);
        this.lblDetailScheduleId.Name = "lblDetailScheduleId";
        this.lblDetailScheduleId.Text = "일정ID";
        this.txtDetailScheduleId.Location = new System.Drawing.Point(110, 16);
        this.txtDetailScheduleId.Name = "txtDetailScheduleId";
        this.txtDetailScheduleId.Size = new System.Drawing.Size(120, 20);
        this.txtDetailScheduleId.Properties.ReadOnly = true;
        this.panData.Controls.Add(this.lblDetailScheduleId);
        this.panData.Controls.Add(this.txtDetailScheduleId);
        this.lblDetailAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailAccId.Appearance.Options.UseFont = true;
        this.lblDetailAccId.Location = new System.Drawing.Point(250, 19);
        this.lblDetailAccId.Name = "lblDetailAccId";
        this.lblDetailAccId.Text = "사업장";
        this.cboDetailAccId.Location = new System.Drawing.Point(330, 16);
        this.cboDetailAccId.Name = "cboDetailAccId";
        this.cboDetailAccId.Size = new System.Drawing.Size(116, 20);
        this.cboDetailAccId.LookupKey = "L_ACC";
        this.cboDetailAccId.Required = true;
        this.panData.Controls.Add(this.lblDetailAccId);
        this.panData.Controls.Add(this.cboDetailAccId);
        this.lblDetailTitle.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailTitle.Appearance.Options.UseFont = true;
        this.lblDetailTitle.Location = new System.Drawing.Point(16, 47);
        this.lblDetailTitle.Name = "lblDetailTitle";
        this.lblDetailTitle.Text = "제목";
        this.txtDetailTitle.Location = new System.Drawing.Point(110, 44);
        this.txtDetailTitle.Name = "txtDetailTitle";
        this.txtDetailTitle.Size = new System.Drawing.Size(336, 20);
        this.txtDetailTitle.Required = true;
        this.panData.Controls.Add(this.lblDetailTitle);
        this.panData.Controls.Add(this.txtDetailTitle);
        this.chkDetailAllDay.Location = new System.Drawing.Point(110, 72);
        this.chkDetailAllDay.Name = "chkDetailAllDay";
        this.chkDetailAllDay.Checked = true;
        this.lblDetailAllDay.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailAllDay.Appearance.Options.UseFont = true;
        this.lblDetailAllDay.Location = new System.Drawing.Point(16, 75);
        this.lblDetailAllDay.Name = "lblDetailAllDay";
        this.lblDetailAllDay.Text = "종일";
        this.panData.Controls.Add(this.lblDetailAllDay);
        this.panData.Controls.Add(this.chkDetailAllDay);
        this.lblDetailStartDt.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailStartDt.Appearance.Options.UseFont = true;
        this.lblDetailStartDt.Location = new System.Drawing.Point(16, 103);
        this.lblDetailStartDt.Name = "lblDetailStartDt";
        this.lblDetailStartDt.Text = "시작일";
        this.dteDetailStartDt.Location = new System.Drawing.Point(110, 100);
        this.dteDetailStartDt.Name = "dteDetailStartDt";
        this.dteDetailStartDt.Size = new System.Drawing.Size(110, 20);
        this.txtDetailStartTm.Location = new System.Drawing.Point(226, 100);
        this.txtDetailStartTm.Name = "txtDetailStartTm";
        this.txtDetailStartTm.Size = new System.Drawing.Size(60, 20);
        this.txtDetailStartTm.Properties.NullText = "HH:mm";
        this.panData.Controls.Add(this.lblDetailStartDt);
        this.panData.Controls.Add(this.dteDetailStartDt);
        this.panData.Controls.Add(this.txtDetailStartTm);
        this.lblDetailEndDt.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailEndDt.Appearance.Options.UseFont = true;
        this.lblDetailEndDt.Location = new System.Drawing.Point(16, 131);
        this.lblDetailEndDt.Name = "lblDetailEndDt";
        this.lblDetailEndDt.Text = "종료일";
        this.dteDetailEndDt.Location = new System.Drawing.Point(110, 128);
        this.dteDetailEndDt.Name = "dteDetailEndDt";
        this.dteDetailEndDt.Size = new System.Drawing.Size(110, 20);
        this.txtDetailEndTm.Location = new System.Drawing.Point(226, 128);
        this.txtDetailEndTm.Name = "txtDetailEndTm";
        this.txtDetailEndTm.Size = new System.Drawing.Size(60, 20);
        this.txtDetailEndTm.Properties.NullText = "HH:mm";
        this.panData.Controls.Add(this.lblDetailEndDt);
        this.panData.Controls.Add(this.dteDetailEndDt);
        this.panData.Controls.Add(this.txtDetailEndTm);
        this.lblDetailColor.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailColor.Appearance.Options.UseFont = true;
        this.lblDetailColor.Location = new System.Drawing.Point(16, 159);
        this.lblDetailColor.Name = "lblDetailColor";
        this.lblDetailColor.Text = "색상";
        this.panColorSwatches.Location = new System.Drawing.Point(110, 156);
        this.panColorSwatches.Name = "panColorSwatches";
        this.panColorSwatches.Size = new System.Drawing.Size(340, 24);
        this.panData.Controls.Add(this.lblDetailColor);
        this.panData.Controls.Add(this.panColorSwatches);
        this.btnDetailClose.Location = new System.Drawing.Point(400, 2);
        this.btnDetailClose.Name = "btnDetailClose";
        this.btnDetailClose.Size = new System.Drawing.Size(56, 22);
        this.btnDetailClose.Text = "닫기";
        this.btnDetailClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.lblDetailContent.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailContent.Appearance.Options.UseFont = true;
        this.lblDetailContent.Location = new System.Drawing.Point(16, 187);
        this.lblDetailContent.Name = "lblDetailContent";
        this.lblDetailContent.Text = "내용";
        this.memDetailContent.Location = new System.Drawing.Point(110, 184);
        this.memDetailContent.Name = "memDetailContent";
        this.memDetailContent.Size = new System.Drawing.Size(336, 110);
        this.memDetailContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.panData.Controls.Add(this.lblDetailContent);
        this.panData.Controls.Add(this.memDetailContent);
        this.lblDetailRegDt.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailRegDt.Appearance.Options.UseFont = true;
        this.lblDetailRegDt.Location = new System.Drawing.Point(16, 307);
        this.lblDetailRegDt.Name = "lblDetailRegDt";
        this.lblDetailRegDt.Text = "등록일시";
        this.txtDetailRegDt.Location = new System.Drawing.Point(110, 304);
        this.txtDetailRegDt.Name = "txtDetailRegDt";
        this.txtDetailRegDt.Size = new System.Drawing.Size(150, 20);
        this.txtDetailRegDt.Properties.ReadOnly = true;
        this.txtDetailRegDt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.panData.Controls.Add(this.lblDetailRegDt);
        this.panData.Controls.Add(this.txtDetailRegDt);
            //
            // panelWyn6
            //
            this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
            this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn6.Location = new System.Drawing.Point(0, 0);
            this.panelWyn6.Name = "panelWyn6";
            this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn6.Size = new System.Drawing.Size(462, 27);
            this.panelWyn6.TabIndex = 7;
            //
            // splitterWyn1
            //
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Location = new System.Drawing.Point(760, 0);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(10, 493);
            this.splitterWyn1.TabIndex = 9;
            this.splitterWyn1.TabStop = false;
            //
            // panelWyn8
            //
            this.panelWyn8.Controls.Add(this.schedulerControl1);
            this.panelWyn8.Controls.Add(this.panelWyn2);
            this.panelWyn8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelWyn8.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn8.Location = new System.Drawing.Point(0, 0);
            this.panelWyn8.Name = "panelWyn8";
            this.panelWyn8.Size = new System.Drawing.Size(760, 493);
            this.panelWyn8.TabIndex = 12;
            //
            // schedulerControl1
            //
            this.schedulerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schedulerControl1.GroupType = DevExpress.XtraScheduler.SchedulerGroupType.Date;
            this.schedulerControl1.Location = new System.Drawing.Point(0, 27);
            this.schedulerControl1.Name = "schedulerControl1";
            this.schedulerControl1.Size = new System.Drawing.Size(760, 466);
            this.schedulerControl1.Storage = this.schedulerStorage1;
            this.schedulerControl1.TabIndex = 10;
            this.schedulerControl1.ActiveViewType = DevExpress.XtraScheduler.SchedulerViewType.Month;
            this.schedulerControl1.Views.DayView.Enabled = false;
            this.schedulerControl1.Views.WorkWeekView.Enabled = false;
            this.schedulerControl1.Views.WeekView.Enabled = false;
            this.schedulerControl1.Views.FullWeekView.Enabled = false;
            this.schedulerControl1.Views.MonthView.Enabled = true;
            this.schedulerControl1.Views.TimelineView.Enabled = false;
            this.schedulerControl1.Views.GanttView.Enabled = false;
            this.schedulerControl1.Views.YearView.Enabled = false;
            this.schedulerControl1.Views.AgendaView.Enabled = false;
            // 요일/오늘 셀 헤더 색을 앱 톤에 맞춘다 - 리플렉션으로 확인한 실제 속성만 사용
            // (MonthViewAppearance.HeaderCaption/TodayCellHeaderCaption - 처음 추측했던
            // AppearanceHeader.DayOfWeekHeader/NavigationHeader, WeekendsViewMode,
            // ShowNavigationButtons, AppearancePrintExport, AppointmentDisplayOptions.
            // ShowStartEndTime, Appointments.LabelStorage는 전부 존재하지 않아 빌드 에러로
            // 확인 후 제거했다).
            this.schedulerControl1.Views.MonthView.Appearance.HeaderCaption.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.schedulerControl1.Views.MonthView.Appearance.HeaderCaption.ForeColor = System.Drawing.Color.FromArgb(90, 94, 102);
            this.schedulerControl1.Views.MonthView.Appearance.HeaderCaption.Options.UseFont = true;
            this.schedulerControl1.Views.MonthView.Appearance.HeaderCaption.Options.UseForeColor = true;
            this.schedulerControl1.Views.MonthView.Appearance.HeaderCaption.BackColor = System.Drawing.Color.FromArgb(247, 248, 250);
            this.schedulerControl1.Views.MonthView.Appearance.HeaderCaption.Options.UseBackColor = true;
            this.schedulerControl1.Views.MonthView.Appearance.TodayCellHeaderCaption.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.schedulerControl1.Views.MonthView.Appearance.TodayCellHeaderCaption.ForeColor = System.Drawing.Color.FromArgb(41, 98, 218);
            this.schedulerControl1.Views.MonthView.Appearance.TodayCellHeaderCaption.Options.UseFont = true;
            this.schedulerControl1.Views.MonthView.Appearance.TodayCellHeaderCaption.Options.UseForeColor = true;
            this.schedulerControl1.Views.MonthView.Appearance.TodayCellHeaderCaption.BackColor = System.Drawing.Color.FromArgb(232, 240, 254);
            this.schedulerControl1.Views.MonthView.Appearance.TodayCellHeaderCaption.Options.UseBackColor = true;
            //
            // schedulerStorage1
            //
            this.schedulerStorage1.Appointments.Mappings.AppointmentId = "schedule_id";
            this.schedulerStorage1.Appointments.Mappings.Start = "calc_start";
            this.schedulerStorage1.Appointments.Mappings.End = "calc_end";
            this.schedulerStorage1.Appointments.Mappings.AllDay = "calc_all_day";
            this.schedulerStorage1.Appointments.Mappings.Subject = "title";
            this.schedulerStorage1.Appointments.Mappings.Description = "content";
            this.schedulerStorage1.Appointments.Mappings.Label = "color_cd";
            //
            // panelWyn2
            //
            this.panelWyn2.Controls.Add(this.sectionHeaderWyn4);
            this.panelWyn2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn2.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn2.Location = new System.Drawing.Point(0, 0);
            this.panelWyn2.Name = "panelWyn2";
            this.panelWyn2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn2.Size = new System.Drawing.Size(760, 27);
            this.panelWyn2.TabIndex = 11;
            //
            // paTitle
            //
            this.paTitle.Controls.Add(this.sectionHeaderWyn1);
            this.paTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle.Location = new System.Drawing.Point(5, 0);
            this.paTitle.Name = "paTitle";
            this.paTitle.Size = new System.Drawing.Size(1235, 33);
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
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1235, 33);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 9;
            this.sectionHeaderWyn1.Text = "일정관리 [frmSchedule]";
            //
            // sectionHeaderWyn4
            //
            this.sectionHeaderWyn4.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn4.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn4.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn4.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn4.Name = "sectionHeaderWyn4";
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(755, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 9;
            this.sectionHeaderWyn4.Text = "캘린더";
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
            this.sectionHeaderWyn3.Size = new System.Drawing.Size(457, 25);
            this.sectionHeaderWyn3.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn3.SvgIcon")));
            this.sectionHeaderWyn3.TabIndex = 9;
            this.sectionHeaderWyn3.Text = "상세 등록";
            //
            // frmSchedule
            //
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1245, 580);
            this.Controls.Add(this.panBase);
            this.Name = "frmSchedule";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
            this.panData.ResumeLayout(false);
            this.panData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
            this.panelWyn6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).EndInit();
            this.panelWyn8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.schedulerControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerStorage1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
            this.panelWyn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
            this.paTitle.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private PanelWyn panBase;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn4;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblDetailScheduleId;
    private TextEditWyn txtDetailScheduleId;
    private DevExpress.XtraEditors.LabelControl lblDetailAccId;
    private LookUpEditWyn cboDetailAccId;
    private DevExpress.XtraEditors.LabelControl lblDetailTitle;
    private TextEditWyn txtDetailTitle;
    private CheckBoxWyn chkDetailAllDay;
    private DevExpress.XtraEditors.LabelControl lblDetailAllDay;
    private DevExpress.XtraEditors.LabelControl lblDetailStartDt;
    private DateEditWyn dteDetailStartDt;
    private TextEditWyn txtDetailStartTm;
    private DevExpress.XtraEditors.LabelControl lblDetailEndDt;
    private DateEditWyn dteDetailEndDt;
    private TextEditWyn txtDetailEndTm;
    private DevExpress.XtraEditors.LabelControl lblDetailColor;
    private System.Windows.Forms.Panel panColorSwatches;
    private ButtonWyn btnDetailClose;
    private DevExpress.XtraEditors.LabelControl lblDetailContent;
    private MemoEditWyn memDetailContent;
    private DevExpress.XtraEditors.LabelControl lblDetailRegDt;
    private TextEditWyn txtDetailRegDt;
    private PanelWyn panelWyn6;
    private SplitterWyn splitterWyn1;
    private PanelWyn panelWyn8;
    private DevExpress.XtraScheduler.SchedulerControl schedulerControl1;
    private DevExpress.XtraScheduler.SchedulerStorage schedulerStorage1;
    private PanelWyn panelWyn2;
    private PanelWyn paTitle;
    private SectionHeaderWyn sectionHeaderWyn3;
    private SectionHeaderWyn sectionHeaderWyn4;
    private SectionHeaderWyn sectionHeaderWyn1;
}
