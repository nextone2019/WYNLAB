// AI Builder가 마스터-폼-서브그리드 템플릿을 복제해서 자동 생성 - 2026-09-17.
// 2026-09-17에 마스터 목록(grd1)을 DevExpress SchedulerControl(월간 캘린더)로 손으로 바꿨다 -
// "일정관리를 카렌더 형태로" 요청. panData/저장/삭제 로직은 템플릿 그대로 재사용하고, 캘린더는
// 순수 브라우징+선택 UI로만 쓴다(달력 자체 드래그/리사이즈/편집 다이얼로그는 전부 꺼둠 -
// AllowAppointmentXxx 이벤트 참고) - 실제 입력/저장은 항상 panData + 툴바 버튼을 거친다.
using System.Data;
using System.Drawing;
using System.Globalization;
using DevExpress.XtraScheduler;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.SM;

public partial class frmSchedule : BaseForm
{
    private DataTable _list = new();
    private string? _editingKey; // null이면 신규모드

    // QueryCore가 스스로 Appointments.DataSource를 다시 붙이거나(재조회/저장 후) 달력을 특정
    // 날짜로 넘기는 동안은 SelectionChanged가 사용자가 고른 것처럼 오작동하면 안 된다 -
    // SuppressMasterRowSwitchConfirm과 같은 이유의 가드.
    private bool _suppressSelectionChanged;

    // 색상 태그 팔레트 - DevExpress Scheduler의 AppointmentLabel 기능을 그대로 쓴다(Label을
    // color_cd에 매핑해두면 달력이 알아서 그 색으로 그려줌, 별도 CustomDraw 불필요). id는 등록
    // 순서가 아니라 고정 숫자로 명시한다 - DB에 저장된 COLOR_CD 값이 나중에 코드 순서가 바뀌어도
    // 안 깨지게(2026-09-17).
    private static readonly (string Id, string Name, Color Color)[] ColorPalette =
    {
        ("1", "파랑", Color.FromArgb(0x4F, 0x8E, 0xF7)),
        ("2", "빨강", Color.FromArgb(0xF7, 0x66, 0x66)),
        ("3", "초록", Color.FromArgb(0x4C, 0xAF, 0x7D)),
        ("4", "주황", Color.FromArgb(0xF5, 0xA7, 0x42)),
        ("5", "보라", Color.FromArgb(0x9B, 0x6F, 0xD6)),
        ("6", "회색", Color.FromArgb(0x8C, 0x92, 0x9B)),
        ("7", "청록", Color.FromArgb(0x3F, 0xC1, 0xC9)),
        ("8", "분홍", Color.FromArgb(0xF2, 0x7F, 0xB0)),
    };

    private readonly Dictionary<string, Panel> _swatchesById = new();
    private string _selectedColorCd = ColorPalette[0].Id;

    private const int DetailPanelWidth = 760; // panelWyn8(캘린더)의 Designer 기본 너비 - 상세패널 열렸을 때만 이 너비로 고정

    /// <summary>상세패널(스플리터+panelWyn4/panData) 전체를 열고닫는다 - panData만 숨기면 그
    /// 자리가 빈 칸으로 남아서 캘린더가 안 넓어지므로, 스플리터/우측패널까지 같이 접고 캘린더
    /// (panelWyn8)를 Fill로 늘린다(2026-09-17 요청).</summary>
    private void ShowDetailPanel(bool show)
    {
        panData.Visible = show;
        splitterWyn1.Visible = show;
        panelWyn4.Visible = show;
        panelWyn8.Dock = show ? DockStyle.Left : DockStyle.Fill;
        if (show) panelWyn8.Width = DetailPanelWidth;
    }

    public frmSchedule()
    {
        InitializeComponent();

        // VS 디자이너가 이 폼 자체를 편집 화면으로 열 때도 이 생성자가 그대로 실행된다 - 그때
        // ShowDetailPanel(false)/EnterNewMode() 등 런타임 로직까지 돌면 상세패널(panData)이
        // 숨겨진/찌그러진 상태로 캔버스에 나타나서 마우스로 편집을 할 수가 없다(2026-09-17,
        // "화면수정할 수 있도록" 요청 - 디자이너에서 열리게 해달라는 의미로 확인).
        // Control.DesignMode는 root 폼 자기 자신의 생성자에서는 아직 Site가 안 붙어서 항상
        // false로 나오는 유명한 함정이라 LicenseManager로 판별한다.
        if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            return;

        Text = "일정관리";


        foreach (var c in ColorPalette)
            schedulerStorage1.Appointments.Labels.Add(c.Id, c.Name, c.Name, c.Color);
        BuildColorSwatches();

        // 캘린더는 순수 브라우징+선택 전용이다 - 드래그로 옮기기/크기조절/더블클릭 편집 다이얼로그/
        // Delete키 삭제/빈칸에서 만들기까지 DevExpress 기본 상호작용을 전부 끄고, 모든 실제 변경은
        // panData + 툴바(신규/저장/삭제)로만 하게 한다(2026-09-17 - "카렌더 형태로" 요청은 보는
        // 방식을 캘린더로 바꿔달라는 것이지, 결재/보안 검증 없이 달력에서 직접 데이터가 바뀌는
        // 것까지 원한 건 아니라고 판단 - 이 화면의 나머지 CRUD 규약과도 일관성 유지).
        schedulerControl1.AllowAppointmentCreate += (s, e) => e.Allow = false;
        schedulerControl1.AllowAppointmentEdit += (s, e) => e.Allow = false;
        schedulerControl1.AllowAppointmentDelete += (s, e) => e.Allow = false;
        schedulerControl1.AllowAppointmentDrag += (s, e) => e.Allow = false;
        schedulerControl1.AllowAppointmentResize += (s, e) => e.Allow = false;
        // 위 AllowAppointmentXxx "이벤트"들은 실제 변경 적용 여부만 막을 뿐 UI 자체를 끄지 않는다.
        // 네이티브 팝업("Untitled - Event")을 완전히 죽이려면 3중으로 막아야 한다(2026-09-17,
        // 같은 문제 재발 - 한 가지 스위치로는 부족했음):
        //  1) OptionsCustomization의 "프로퍼티"(이벤트와 이름은 같지만 별개 - UsedAppointmentType)
        //     로 UI 상호작용 자체를 차단
        var oc = schedulerControl1.OptionsCustomization;
        oc.AllowAppointmentCreate = DevExpress.XtraScheduler.UsedAppointmentType.None;
        oc.AllowAppointmentEdit = DevExpress.XtraScheduler.UsedAppointmentType.None;
        oc.AllowAppointmentDelete = DevExpress.XtraScheduler.UsedAppointmentType.None;
        oc.AllowAppointmentDrag = DevExpress.XtraScheduler.UsedAppointmentType.None;
        oc.AllowAppointmentResize = DevExpress.XtraScheduler.UsedAppointmentType.None;
        oc.AllowInplaceEditor = DevExpress.XtraScheduler.UsedAppointmentType.None;
        //  2) 편집/생성 다이얼로그 표시 자체를 금지
        oc.AllowDisplayAppointmentForm = DevExpress.XtraScheduler.AllowDisplayAppointmentForm.Never;
        //  3) 그래도 폼이 뜨려는 순간을 가로채는 최종 방어선 - Handled=true면 DevExpress가 자기
        //     폼을 절대 띄우지 않는다(공식 "커스텀 에디터로 교체" 패턴).
        schedulerControl1.EditAppointmentFormShowing += (s, e) => e.Handled = true;
        schedulerControl1.SelectionChanged += SchedulerControl1_SelectionChanged;

        // 휴일(토/일) 날짜 숫자를 빨간색으로 - MonthView의 날짜 셀도 "DayHeader"로 그려진다
        // (요일 제목행은 별개의 CustomDrawDayOfWeekHeader) - 공휴일 테이블은 아직 없어서 주말만.
        schedulerControl1.CustomDrawDayHeader += (s, e) =>
        {
            if (e.ObjectInfo is not DevExpress.XtraScheduler.Drawing.SingleWeekCellBase cell) return;
            var isWeekend = cell.Date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
            cell.Header.CaptionAppearance.ForeColor = isWeekend ? Color.Red : cell.Header.CaptionAppearance.ForeColor;
            cell.Header.CaptionAppearance.Options.UseForeColor = isWeekend;
        };

        // 평소엔 캘린더만 보이고, 등록된 일정/빈 날짜를 더블클릭했을 때만 상세패널을 연다
        // (2026-09-17 요청) - MouseDoubleClick이 뜨기 전에 SelectionChanged가 먼저 실행되어
        // panData 내용은 이미 채워져 있으므로, 여기선 보여주기만 하면 된다.
        schedulerControl1.MouseDoubleClick += (s, e) => ShowDetailPanel(true);
        btnDetailClose.Click += async (s, e) =>
        {
            if (!await ConfirmDiscardUnsavedAsync()) return;
            ShowDetailPanel(false);
        };

        // 개발자용 마우스오버 툴팁(BindingField) - 실제 적용은 BaseForm.ApplyBindingFieldTooltips가
        // 공통으로 처리한다(Session.IsDeveloper일 때만).
        txtDetailScheduleId.Tag = new BindingFieldTag("schedule_id");
        cboDetailAccId.Tag = new BindingFieldTag("acc_id");
        txtDetailTitle.Tag = new BindingFieldTag("title");
        memDetailContent.Tag = new BindingFieldTag("content");
        chkDetailAllDay.Tag = new BindingFieldTag("all_day_yn");
        dteDetailStartDt.Tag = new BindingFieldTag("start_dt");
        txtDetailStartTm.Tag = new BindingFieldTag("start_tm");
        dteDetailEndDt.Tag = new BindingFieldTag("end_dt");
        txtDetailEndTm.Tag = new BindingFieldTag("end_tm");
        txtDetailRegDt.Tag = new BindingFieldTag("reg_dt");

        chkDetailAllDay.Properties.ValueChecked = "Y";
        chkDetailAllDay.Properties.ValueUnchecked = "N";
        chkDetailAllDay.CheckedChanged += (s, e) => ApplyAllDayEnabled();

        TrackDirty(panData);

        EnterNewMode();
        ShowDetailPanel(false); // 화면 오픈 시엔 캘린더만 - 더블클릭해야 열림
        Load += async (s, e) => await QueryClick();
    }

    /// <summary>종일이면 시간칸을 잠그고 비운다 - "종일"인데 시간값이 남아있는 어중간한 상태를
    /// 막는다.</summary>
    private void ApplyAllDayEnabled()
    {
        var enabled = !chkDetailAllDay.Checked;
        txtDetailStartTm.Properties.ReadOnly = !enabled;
        txtDetailEndTm.Properties.ReadOnly = !enabled;
        txtDetailStartTm.Enabled = enabled;
        txtDetailEndTm.Enabled = enabled;
        if (!enabled)
        {
            txtDetailStartTm.Text = string.Empty;
            txtDetailEndTm.Text = string.Empty;
        }
    }

    /// <summary>색상 스와치를 panColorSwatches 안에 코드로 그린다(HomeForm의 "반복되는 작은
    /// 컨트롤은 코드로" 관례) - 클릭하면 _selectedColorCd를 바꾸고 선택 테두리를 다시 그린다.</summary>
    private void BuildColorSwatches()
    {
        const int size = 22, gap = 4;
        var x = 0;
        foreach (var c in ColorPalette)
        {
            var sw = new Panel
            {
                Location = new Point(x, 1),
                Size = new Size(size, size),
                BackColor = c.Color,
                Cursor = Cursors.Hand,
                Tag = c.Id,
            };
            sw.Click += (s, e) =>
            {
                _selectedColorCd = c.Id;
                IsDirty = true;
                RedrawColorSwatchSelection();
            };
            panColorSwatches.Controls.Add(sw);
            _swatchesById[c.Id] = sw;
            x += size + gap;
        }
        RedrawColorSwatchSelection();
    }

    private void RedrawColorSwatchSelection()
    {
        foreach (var kv in _swatchesById)
            kv.Value.BorderStyle = kv.Key == _selectedColorCd ? BorderStyle.Fixed3D : BorderStyle.None;
    }

    /// <summary>"9:30"/"930"/"0930" 등 느슨한 입력을 저장용 4자리 "HHmm"으로 정규화한다. 빈 값은
    /// 그대로 빈 값(종일 일정이거나 아직 안 채운 경우). 잘못된 값이면 null(호출부에서 검증 실패
    /// 처리).</summary>
    private static string? NormalizeHhMm(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        var digits = new string(input.Where(char.IsDigit).ToArray());
        if (digits.Length is < 1 or > 4) return null;
        digits = digits.PadLeft(4, '0');
        if (!int.TryParse(digits.Substring(0, 2), out var hh) || !int.TryParse(digits.Substring(2, 2), out var mm)) return null;
        if (hh is < 0 or > 23 || mm is < 0 or > 59) return null;
        return $"{hh:D2}{mm:D2}";
    }

    private static string FormatHhMm(string? hhmm) =>
        hhmm != null && hhmm.Length == 4 ? $"{hhmm.Substring(0, 2)}:{hhmm.Substring(2, 2)}" : string.Empty;

    public override async Task QueryClick() => await QueryCore(preserveSelection: false);

    /// <summary>사용자가 직접 누른 조회(preserveSelection: false)는 새 검색이므로 캘린더를 오늘로
    /// 되돌리고 신규모드로, 저장/삭제 뒤의 내부 재조회(preserveSelection: true)는 방금 편집하던
    /// 건을 그대로 panData에 다시 채운다(2026-09-06 요청과 같은 패턴 - 저장 직후 화면이 빈
    /// 것처럼 보이면 안 됨).</summary>
    private async Task QueryCore(bool preserveSelection)
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_emp_no"] = Session.EmpNo, // 본인 일정만(USP_SM_SCHEDULE_Q가 이 값으로 필터)
        };
        _list = await QueryAsync("USP_SM_SCHEDULE_Q", p);
        AddSchedulerCalcColumns(_list);

        var editingKey = preserveSelection ? _editingKey : null;
        var row = editingKey == null ? null : FindRow(editingKey);

        _suppressSelectionChanged = true;
        try
        {
            schedulerStorage1.Appointments.DataSource = _list;
            // 방금 저장/조회한 건이 있으면 그 달로 달력을 넘겨서 바로 눈에 보이게 한다 - 정확히
            // 그 일정을 하이라이트하는 것까지는 안 하고(캘린더 자체 선택 API가 그리드의
            // FocusedRowHandle만큼 단순하지 않음), 날짜만 맞춰준다. panData는 아래에서 직접 채운다.
            if (row != null && DateTime.TryParseExact(Str(row, "start_dt"), "yyyyMMdd", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var jumpTo))
                schedulerControl1.Start = jumpTo;
        }
        finally { _suppressSelectionChanged = false; }

        if (row != null) OnMasterSelected(row);
        else EnterNewMode();
    }

    /// <summary>캘린더에서 일정(약속)을 클릭했으면 그 행을 panData에 채우고, 빈 날짜를
    /// 클릭/드래그했으면(선택된 약속 없음) 그 날짜를 시작일로 미리 채운 신규모드로 들어간다 -
    /// grd1이 있던 시절 FocusedRowObjectChanged가 하던 역할을 캘린더 버전으로 그대로 옮긴 것.
    /// ConfirmMasterRowSwitch의 그리드 전용 되돌리기 트릭은 캘린더에 그대로 못 옮기므로(선택
    /// API가 핸들 기반이 아님), 저장 안 된 변경이 있으면 미리 물어보기만 하고 진행한다 - 취소를
    /// 선택해도 캘린더 쪽 선택 자체는 이미 바뀐 뒤라 panData만 이전 내용 그대로 남는다(약간의
    /// 시각적 불일치는 감수 - 미저장 변경을 조용히 버리는 것보다는 낫다).</summary>
    private async void SchedulerControl1_SelectionChanged(object? sender, EventArgs e)
    {
        if (_suppressSelectionChanged) return;
        if (!await ConfirmDiscardUnsavedAsync()) return;

        if (schedulerControl1.SelectedAppointments.Count > 0)
        {
            var sourceRow = schedulerControl1.SelectedAppointments[0].GetRow(schedulerStorage1);
            var row = sourceRow as DataRow ?? (sourceRow as DataRowView)?.Row;
            if (row != null)
            {
                OnMasterSelected(row);
                return;
            }
        }

        var interval = schedulerControl1.SelectedInterval;
        if (interval.Start == default)
        {
            EnterNewMode();
            return;
        }
        // Scheduler의 SelectedInterval.End는 배타적(다음날 0시) - 사용자가 실제로 클릭/드래그한
        // 마지막 날짜(포함)로 되돌린다. AddCalcColumns가 END_DT에 +1일을 하는 것과 반대 방향.
        var endInclusive = interval.End > interval.Start ? interval.End.AddDays(-1) : interval.Start;
        EnterNewMode(interval.Start, endInclusive);
    }

    /// <summary>미저장 변경이 있으면 저장/버림/취소를 묻는다 - 캘린더 선택 변경과 상세패널
    /// 닫기 버튼이 공유하는 로직(둘 다 "지금 panData 내용을 버려도 되는가"를 묻는 같은 상황).
    /// true면 계속 진행해도 된다는 뜻(취소했거나 저장 실패면 false).</summary>
    private async Task<bool> ConfirmDiscardUnsavedAsync()
    {
        if (!HasUnsavedChanges) return true;

        var confirm = AppMessageBox.Show(
            "변경 내역이 저장되지 않았습니다.\n저장하시겠습니까?",
            "변경 내역 확인", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        if (confirm == DialogResult.Cancel) return false;

        if (confirm == DialogResult.Yes)
        {
            await SafeExecuteAsync(SaveClick, "저장");
            return !HasUnsavedChanges; // 저장 실패면 false - panData를 그대로 둔다
        }

        IsDirty = false; // "아니오" - 미저장 변경을 버리고 진행
        return true;
    }

    /// <summary>조회된 목록에서 키(schedule_id)로 행을 찾는다. 없으면 null.</summary>
    private DataRow? FindRow(string key) =>
        _list.Rows.Cast<DataRow>()
            .FirstOrDefault(r => string.Equals(Convert.ToString(r["schedule_id"]), key, StringComparison.OrdinalIgnoreCase));

    /// <summary>SchedulerStorage.Appointments가 Start/End로 읽을 수 있는 실제 DateTime 컬럼을
    /// 만든다 - USP_SM_SCHEDULE_Q가 돌려주는 start_dt/end_dt는 "yyyyMMdd" 문자열(프로젝트 날짜
    /// 컨벤션)이라 Scheduler가 그대로는 못 읽는다(DateTime.Parse가 이 포맷을 못 알아봄). calc_end는
    /// 원래 값(마지막 날 포함)에 +1일 해서 넣는다 - Scheduler의 End/all-day 구간은 iCal DTEND와
    /// 같은 배타적 규칙(다음날 0시)을 쓰기 때문에, 그대로 넣으면 마지막 날 하루가 캘린더에서 빠져
    /// 보인다. 이 컬럼들은 화면 표시 전용이고 저장에는 안 쓴다(원본 start_dt/end_dt만 저장).</summary>
    private static void AddSchedulerCalcColumns(DataTable table)
    {
        if (!table.Columns.Contains("calc_start")) table.Columns.Add("calc_start", typeof(DateTime));
        if (!table.Columns.Contains("calc_end")) table.Columns.Add("calc_end", typeof(DateTime));
        if (!table.Columns.Contains("calc_all_day")) table.Columns.Add("calc_all_day", typeof(bool));

        foreach (DataRow row in table.Rows)
        {
            var start = ParseYyyyMmDd(Str(row, "start_dt")) ?? DateTime.Today;
            var endInclusive = ParseYyyyMmDd(Str(row, "end_dt")) ?? start;
            var isAllDay = Str(row, "all_day_yn") != "N";

            if (isAllDay)
            {
                row["calc_start"] = start;
                row["calc_end"] = endInclusive.AddDays(1); // iCal DTEND식 배타적 종료 - 위 주석 참고
            }
            else
            {
                // 시간이 있는 일정은 마지막 날 +1일을 하면 안 된다(그 자체로 이미 정확한 종료 시각) -
                // start_tm/end_tm이 비어있으면(데이터 이상) 00:00으로 방어.
                row["calc_start"] = start.Add(ParseHhMmToTimeSpan(Str(row, "start_tm")));
                row["calc_end"] = endInclusive.Add(ParseHhMmToTimeSpan(Str(row, "end_tm")));
            }
            row["calc_all_day"] = isAllDay;
        }
    }

    private static TimeSpan ParseHhMmToTimeSpan(string hhmm) =>
        hhmm.Length == 4 && int.TryParse(hhmm.Substring(0, 2), out var hh) && int.TryParse(hhmm.Substring(2, 2), out var mm)
            ? new TimeSpan(hh, mm, 0)
            : TimeSpan.Zero;

    private static DateTime? ParseYyyyMmDd(string yyyyMmDd) =>
        yyyyMmDd.Length == 8 && DateTime.TryParseExact(yyyyMmDd, "yyyyMMdd", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var dt)
            ? dt
            : null;

    private static string Str(DataRow row, string columnName) =>
        row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value
            ? (Convert.ToString(row[columnName]) ?? string.Empty)
            : string.Empty;

    private void OnMasterSelected(DataRow row)
    {
        // 코드가 값을 채우는 것뿐인데 TrackDirty(panData)가 "사용자가 고쳤다"로 오인하지 않게 감싼다.
        SuppressDirtyTracking(() =>
        {
            _editingKey = row["schedule_id"]?.ToString();
        txtDetailScheduleId.Text = row["schedule_id"]?.ToString() ?? string.Empty;
        cboDetailAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
        txtDetailTitle.Text = row["title"]?.ToString() ?? string.Empty;
        memDetailContent.Text = row["content"]?.ToString() ?? string.Empty;
        chkDetailAllDay.Checked = Str(row, "all_day_yn") != "N";
        dteDetailStartDt.YyyyMmDd = row["start_dt"]?.ToString();
        dteDetailEndDt.YyyyMmDd = row["end_dt"]?.ToString();
        txtDetailStartTm.Text = FormatHhMm(Str(row, "start_tm"));
        txtDetailEndTm.Text = FormatHhMm(Str(row, "end_tm"));
        ApplyAllDayEnabled();
        _selectedColorCd = string.IsNullOrEmpty(Str(row, "color_cd")) ? ColorPalette[0].Id : Str(row, "color_cd");
        RedrawColorSwatchSelection();
        txtDetailRegDt.Text = row["reg_dt"]?.ToString() ?? string.Empty;
        });
    }

    private void EnterNewMode() => EnterNewMode(null, null);

    /// <summary>prefillStart/End는 캘린더에서 빈 날짜를 클릭/드래그했을 때만 채워진다(툴바
    /// "신규" 버튼은 그냥 EnterNewMode()만 부름 - 오늘 날짜를 억지로 넣지 않는다).</summary>
    private void EnterNewMode(DateTime? prefillStart, DateTime? prefillEnd)
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = null;
        txtDetailScheduleId.Text = string.Empty;
        cboDetailAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        txtDetailTitle.Text = string.Empty;
        memDetailContent.Text = string.Empty;
        chkDetailAllDay.Checked = true;
        dteDetailStartDt.YyyyMmDd = prefillStart?.ToString("yyyyMMdd");
        dteDetailEndDt.YyyyMmDd = prefillEnd?.ToString("yyyyMMdd");
        txtDetailStartTm.Text = string.Empty;
        txtDetailEndTm.Text = string.Empty;
        ApplyAllDayEnabled();
        _selectedColorCd = ColorPalette[0].Id;
        RedrawColorSwatchSelection();
        txtDetailRegDt.Text = string.Empty;
        });
    }

    public override Task NewClick()
    {
        EnterNewMode();
        ShowDetailPanel(true); // 툴바 "신규"도 더블클릭과 같은 진입점 - 패널을 열어줘야 입력 가능
        return Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        // 저장프로시저가 지정 안 된 조회전용 화면이면 여기서 막는다(TplSingleGrid.cs와 같은 가드).
        if (string.IsNullOrEmpty("USP_SM_SCHEDULE_S"))
        {
            AppMessageBox.Show("이 화면은 조회전용으로 생성됐습니다 - 저장프로시저가 지정되지 않았습니다.", "저장 불가");
            return;
        }

        // 종일이 아니면 시작/종료 시각이 둘 다 "HH:mm" 꼴로 유효해야 한다 - 느슨한 입력("9:30",
        // "930")은 여기서 4자리로 정규화하고, 틀린 값이면 저장 자체를 막는다.
        string? startTm = string.Empty, endTm = string.Empty;
        if (!chkDetailAllDay.Checked)
        {
            startTm = NormalizeHhMm(txtDetailStartTm.Text);
            endTm = NormalizeHhMm(txtDetailEndTm.Text);
            if (startTm == null || endTm == null || startTm == string.Empty || endTm == string.Empty)
            {
                AppMessageBox.Show("종일이 아닌 일정은 시작/종료 시각을 \"HH:mm\" 형식으로 입력해야 합니다.", "입력 확인");
                return;
            }
        }

        // ---- 1) 헤더(panData -> USP_SM_SCHEDULE_S) ----
        var headerParams = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_schedule_id"] = txtDetailScheduleId.Text,
            ["p_acc_id"] = cboDetailAccId.EditValue?.ToString() ?? string.Empty,
            ["p_title"] = txtDetailTitle.Text,
            ["p_content"] = memDetailContent.Text,
            ["p_start_dt"] = dteDetailStartDt.YyyyMmDd,
            ["p_end_dt"] = dteDetailEndDt.YyyyMmDd,
            ["p_all_day_yn"] = chkDetailAllDay.Checked ? "Y" : "N",
            ["p_start_tm"] = startTm,
            ["p_end_tm"] = endTm,
            ["p_color_cd"] = _selectedColorCd,
        };

        var headerResult = await SaveAsync("USP_SM_SCHEDULE_S", headerParams);
        if (headerResult == null || !headerResult.Success)
        {
            AppMessageBox.Show(headerResult?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        _editingKey ??= headerResult.GeneratedCode;
        Toast.Show("저장되었습니다.");
        await QueryCore(preserveSelection: true); // 방금 저장한 행 유지 - QueryClick(사용자 조회)과 다른 경로
    }

    public override async Task DeleteClick()
    {
        if (_editingKey == null) return;

        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "D",
            ["p_schedule_id"] = _editingKey,
        };
        var result = await SaveAsync("USP_SM_SCHEDULE_S", p);
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "삭제에 실패했습니다.", "삭제 실패");
            return;
        }

        _editingKey = null;
        Toast.Show("삭제되었습니다.");
        await QueryClick();
    }
}
