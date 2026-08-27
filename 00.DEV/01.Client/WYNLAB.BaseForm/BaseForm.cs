using DevExpress.XtraEditors;
using System.Drawing;

namespace WYNLAB.Base;

/// <summary>
/// 모든 업무화면(MDI 자식폼)의 최상위 베이스.
/// XtraForm(DevExpress) 상속 - 스킨/테마가 자동 적용됨.
/// 여기에 공통 예외처리, 권한 반영, 화면종료 확인 등을 구현한다.
/// </summary>
public class BaseForm : XtraForm
{
    /// <summary>MENU.MENU_CD - 로그인 시 내려받은 권한을 이 코드로 조회해서 버튼 활성화/비활성화 처리</summary>
    public string MenuCd { get; set; } = string.Empty;

    public bool CanInsert { get; protected set; }
    public bool CanUpdate { get; protected set; }
    public bool CanDelete { get; protected set; }
    public bool CanExcel { get; protected set; } = true;

    /// <summary>
    /// 로그인 세션값 - 모든 업무화면(모듈 DLL 포함)에서 그대로 사용.
    /// 값의 실제 출처는 서버의 SSP_WYNLAB_GetSession 프로시저 결과(Session 클래스 참고).
    /// </summary>
    protected string CurrentUserId => Session.UserId;
    protected string CurrentUserNm => Session.UserNm;
    protected string CurrentEmpNo => Session.EmpNo;
    protected string CurrentDeptCd => Session.DeptCd;
    protected string CurrentDeptNm => Session.DeptNm;
    protected string CurrentPositionNm => Session.PositionNm;
    protected bool CurrentIsAdmin => Session.IsAdmin;

    // ===== MDI 상단 공통 툴바(조회/입력/삭제/행추가/행삭제/저장/출력)가 호출하는 표준 액션 =====
    // Shell의 툴바 버튼은 현재 활성화된 MDI 자식폼(this)의 아래 메서드를 그대로 호출한다.
    // 화면마다 필요한 것만 override 하면 되고, 안 쓰는 기능은 기본값(아무 동작 안 함)으로 둔다.
    // 행추가/행삭제(NewRowClick/DeleteRowClick)는 그리드 안에서 바로 편집하는(인라인 편집) 화면용 -
    // 서버 저장은 별도로 SaveClick에서 한번에 하고, 이 둘은 그리드 위 행 자체만 늘리고/줄인다.
    public virtual Task QueryClick() => Task.CompletedTask;
    public virtual Task NewClick() => Task.CompletedTask;
    public virtual Task DeleteClick() => Task.CompletedTask;
    public virtual Task NewRowClick() => Task.CompletedTask;
    public virtual Task DeleteRowClick() => Task.CompletedTask;
    public virtual Task SaveClick() => Task.CompletedTask;
    public virtual Task PrintClick() => Task.CompletedTask;

    /// <summary>
    /// 화면 타이틀 바(BuildScreenHeader) 왼쪽에 그려지는 아이콘. 기본은 폴더지만, 화면 성격에
    /// 더 맞는 아이콘이 있으면(예: 메뉴관리의 트리 아이콘) 화면 클래스에서 override한다.
    /// </summary>
    protected virtual Action<Graphics, Rectangle, Color> ScreenIconPainter => MenuIconPainters.Folder;

    // ===== 저장프로시저 직접 호출(범용 데이터 통로) =====
    // 화면마다 서버에 Controller/Repository를 만들지 않고 프로시저를 바로 부른다. 덕분에
    // 검색조건을 하나 추가할 때 화면과 프로시저만 고치면 되고 서버는 배포하지 않아도 된다.
    // 설계 배경과 보안 모델은 저장소 루트의 GENERIC_DATA_API.md 참고.
    //
    // MenuCd는 화면이 이미 갖고 있으므로 여기서 자동으로 채운다 - 서버가 이 값으로 권한과
    // 실행 가능한 프로시저를 판단하기 때문에, 화면마다 손으로 넘기게 두면 빠뜨리기 쉽다.

    /// <summary>조회 - 첫 번째 결과셋을 돌려준다. 그리드에 그대로 바인딩하면 된다.</summary>
    protected Task<System.Data.DataTable> QueryAsync(string procName, object? parameters = null) =>
        ProcData.QueryAsync(MenuCd, procName, parameters);

    /// <summary>조회 - 결과셋을 여러 개 돌려주는 프로시저용.</summary>
    protected Task<List<System.Data.DataTable>> QueryMultiAsync(string procName, object? parameters = null) =>
        ProcData.QueryMultiAsync(MenuCd, procName, parameters);

    /// <summary>저장/삭제 - 필요한 권한은 서버가 p_work_type(N/U/D)을 보고 판단한다.</summary>
    protected Task<WYNLAB.Shared.Dtos.ApiResult> SaveAsync(string procName, object? parameters = null) =>
        ProcData.SaveAsync(MenuCd, procName, parameters);

    public BaseForm()
    {
        // 폼의 글꼴을 지정해두면 자기 글꼴을 따로 정하지 않은 자식 컨트롤이 전부 이걸 물려받는다.
        // DevExpress 컨트롤은 Program.cs의 WindowsFormsSettings.DefaultFont가 맡지만, 순정
        // WinForms 컨트롤(Panel/Label 등)은 그 설정을 따르지 않아서 이쪽도 같이 잡아야
        // 한 화면 안에서 글꼴이 갈리지 않는다.
        this.Font = AppFonts.Body;

        this.MdiParent = null; // Shell에서 폼 생성 후 주입
        this.Load += BaseForm_Load;
    }

    private void BaseForm_Load(object? sender, EventArgs e)
    {
        ApplyMenuAuth();
    }

    /// <summary>
    /// 로그인 세션에 캐싱된 메뉴권한(MenuDto)을 조회하여
    /// 등록/수정/삭제/엑셀 버튼의 Enabled를 일괄 반영한다.
    /// 실제 구현은 SessionManager(사용자 세션 캐시) 완성 후 채운다.
    /// </summary>
    protected virtual void ApplyMenuAuth()
    {
        if (string.IsNullOrEmpty(MenuCd)) return;

        var auth = SessionManager.Current.GetMenuAuth(MenuCd);
        CanInsert = auth?.InsertYn ?? false;
        CanUpdate = auth?.UpdateYn ?? false;
        CanDelete = auth?.DeleteYn ?? false;
        CanExcel = auth?.ExcelYn ?? false;
    }

    /// <summary>
    /// 모든 업무화면 공통 타이틀 바 - 폴더 아이콘 + 화면명(Text) + 화면코드(MenuCd, 대괄호).
    /// 화면마다 제목 영역을 제각각 만들지 않고 이 메서드 하나로 통일해서, 어떤 화면을 열어도
    /// 같은 위치/스타일로 "지금 보고 있는 화면이 뭔지" 바로 알 수 있게 한다.
    /// 반드시 다른 Dock=Top 패널(조회조건 등)보다 나중에 Controls.Add 해야 맨 위를 차지한다.
    /// </summary>
    protected Panel BuildScreenHeader()
    {
        // 흰 배경(회색 배경이면 아래 본문과 색이 끊겨 보인다는 피드백)으로 본문과 자연스럽게
        // 이어지게 하고, 아래쪽 얇은 선 하나로만 구분한다.
        var header = new Panel { Dock = DockStyle.Top, Height = 34, BackColor = Color.White };
        var bottomBorder = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(228, 229, 232) };
        header.Controls.Add(bottomBorder);

        var icon = new PictureBox
        {
            Image = MenuIconPainters.Render(ScreenIconPainter, 16, Color.FromArgb(120, 124, 132)),
            SizeMode = PictureBoxSizeMode.CenterImage,
            Location = new Point(14, 4),
            Size = new Size(20, 20),
            BackColor = Color.Transparent
        };

        // AutoSizeMode.Default로 실제 텍스트 길이만큼만 폭을 차지하게 해서, 화면명이 짧을 때
        // (예: "메뉴관리") 코드 라벨([SM_MENU])이 옆에 붙지 않고 멀리 떨어져 보이던 문제를 없앤다.
        var lblTitle = new LabelControl
        {
            Text = Text,
            Location = new Point(38, 6),
            AutoSizeMode = LabelAutoSizeMode.Default
        };
        lblTitle.Appearance.Font = AppFonts.BodyBold;
        lblTitle.Appearance.ForeColor = Color.FromArgb(55, 55, 55);

        var lblCode = new LabelControl
        {
            Text = string.IsNullOrEmpty(MenuCd) ? string.Empty : $"[{MenuCd}]",
            AutoSizeMode = LabelAutoSizeMode.Default
        };
        lblCode.Appearance.Font = AppFonts.Caption;
        lblCode.Appearance.ForeColor = Color.FromArgb(150, 150, 150);

        header.Controls.Add(icon);
        header.Controls.Add(lblTitle);
        header.Controls.Add(lblCode);
        void PositionCode() => lblCode.Location = new Point(lblTitle.Right + 8, 8);
        header.Layout += (s, e) => PositionCode();
        PositionCode();

        return header;
    }

    /// <summary>
    /// 공통 예외처리 - 업무화면에서 try/catch 없이 이 메서드로 감싸서 호출.
    /// 로그 적재 + 사용자에게 표준화된 에러 메시지 표시.
    /// </summary>
    protected void SafeExecute(Action action, string actionNm = "")
    {
        try
        {
            ShowBusy();
            action();
        }
        catch (Exception ex)
        {
            // TODO: 공통 로거(Serilog 등) 연동
            AppMessageBox.Show($"[{actionNm}] 처리 중 오류가 발생했습니다.\n{ex.Message}",
                "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            HideBusy();
        }
    }

    /// <summary>
    /// API 호출 등 비동기 작업용 SafeExecute. 조회/저장/삭제 버튼 클릭 핸들러에서 주로 사용.
    /// </summary>
    protected async Task SafeExecuteAsync(Func<Task> action, string actionNm = "")
    {
        try
        {
            ShowBusy();
            await action();
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"[{actionNm}] 처리 중 오류가 발생했습니다.\n{ex.Message}",
                "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            HideBusy();
        }
    }

    private Panel? _busyOverlay;
    private SpinnerControl? _busySpinner;
    private System.Windows.Forms.Timer? _busyDelayTimer;
    private int _busyDepth;

    /// <summary>
    /// 오버레이를 띄우기 전에 기다리는 시간. 사내망에서 조회는 보통 100ms도 안 걸리는데,
    /// 그때마다 불투명 오버레이가 화면을 덮었다 걷히면 "로딩 표시"가 아니라 폼 전체가
    /// 번쩍이는 것으로 보인다(실제로 겪음 - 기초코드등록 조회). 이 시간 안에 끝나는 작업은
    /// 오버레이를 아예 띄우지 않아서 깜빡임이 없고, 그보다 오래 걸리는 작업에만 떠서
    /// 원래 의도(작업 중임을 확실히 알리기)를 그대로 살린다.
    /// </summary>
    private const int BusyDelayMs = 300;

    /// <summary>
    /// 화면 내용(그리드/입력영역) 위에 덮이는 오버레이 + 회전 스피너를 띄운다. 단, 곧바로
    /// 띄우지 않고 BusyDelayMs만큼 기다렸다가 띄운다(그 설명 참고).
    /// 예전엔 이 자리에서 전체 창 커서를 Cursors.WaitCursor로 바꿔서 "로딩 중"을 표시했는데,
    /// 커서 모양 변화만으로는 눈에 잘 안 띈다는 피드백에 따라 실제로 화면에 보이는 오버레이로
    /// 바꿨다. Shell 상단 툴바(ShellForm.AddIconBadgeButton)와 SafeExecute/SafeExecuteAsync
    /// 양쪽에서 공통으로 호출한다.
    ///
    /// 중첩 호출(예: 툴바가 ShowBusy를 부르고 그 안의 로직이 SafeExecuteAsync로 또 부르는 경우)에
    /// 대비해 깊이를 센다 - 안쪽 작업이 끝났다고 바깥 작업이 아직인데 오버레이가 걷히면 안 된다.
    /// </summary>
    public void ShowBusy()
    {
        _busyDepth++;
        if (_busyDepth > 1) return; // 이미 대기 중이거나 표시 중

        _busyForm = this;
        StartBusyTimer();
    }

    public void HideBusy()
    {
        if (_busyDepth > 0) _busyDepth--;
        if (_busyDepth > 0) return; // 바깥 작업이 아직 진행 중

        _busyDelayTimer?.Stop(); // 아직 안 떴으면 영영 안 뜨게 - 이게 깜빡임을 없애는 핵심
        if (_busyOverlay != null) _busyOverlay.Visible = false;
        if (ReferenceEquals(_busyForm, this)) _busyForm = null;
    }

    private void StartBusyTimer()
    {
        if (_busyDelayTimer == null)
        {
            _busyDelayTimer = new System.Windows.Forms.Timer { Interval = BusyDelayMs };
            _busyDelayTimer.Tick += (s, e) =>
            {
                _busyDelayTimer!.Stop();
                ShowBusyOverlayNow();
            };
        }

        _busyDelayTimer.Start();
    }

    /// <summary>지금 오버레이를 띄웠거나 띄우려고 대기 중인 폼. 모달 대화상자가 뜨는 동안
    /// 그 폼의 오버레이를 잠시 걷기 위해 정적으로 들고 있는다(SuspendBusyForModal 참고).</summary>
    private static BaseForm? _busyForm;

    /// <summary>
    /// 모달 대화상자(AppMessageBox 등)가 뜨는 동안 busy 오버레이를 잠시 걷는다.
    ///
    /// 툴바(ShellForm.AddIconBadgeButton)는 액션 전체를 ShowBusy/HideBusy로 감싸는데, 그 액션이
    /// 중간에 "삭제하시겠습니까?" 같은 확인창을 띄우면 사용자가 답할 때까지 계속 "작업 중"
    /// 상태다. 그러면 지연 시간이 지나 오버레이가 올라와서, 확인창 뒤 화면이 통째로 회색으로
    /// 덮여버린다(실제로 겪음 - 대분류 삭제 확인창). 사용자를 기다리는 시간은 작업 중이 아니므로
    /// 그동안은 걷어두고, 대화상자가 닫힌 뒤 실제 작업이 이어질 때 다시 지연 타이머를 건다.
    /// </summary>
    internal static void SuspendBusyForModal()
    {
        var form = _busyForm;
        if (form == null) return;

        form._busyDelayTimer?.Stop();
        if (form._busyOverlay != null) form._busyOverlay.Visible = false;
    }

    /// <summary>모달이 닫힌 뒤 호출 - 아직 작업이 끝나지 않았다면 지연 타이머를 처음부터 다시 건다.</summary>
    internal static void ResumeBusyAfterModal()
    {
        var form = _busyForm;
        if (form == null || form._busyDepth <= 0 || form.IsDisposed) return;

        form.StartBusyTimer();
    }

    private void ShowBusyOverlayNow()
    {
        if (_busyOverlay == null)
        {
            _busyOverlay = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(246, 247, 248) };
            _busySpinner = new SpinnerControl { SpinnerColor = Color.FromArgb(41, 121, 255) };
            _busyOverlay.Controls.Add(_busySpinner);
            _busyOverlay.Resize += (s, e) => CenterBusySpinner();
            Controls.Add(_busyOverlay);
        }

        CenterBusySpinner();
        _busyOverlay.Visible = true;
        _busyOverlay.BringToFront();
    }

    private void CenterBusySpinner()
    {
        if (_busyOverlay == null || _busySpinner == null) return;
        _busySpinner.Location = new Point((_busyOverlay.Width - _busySpinner.Width) / 2, (_busyOverlay.Height - _busySpinner.Height) / 2);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // 폼에 Controls로 붙지 않은 컴포넌트라 자동으로 정리되지 않는다 - 직접 끊어준다.
            _busyDelayTimer?.Stop();
            _busyDelayTimer?.Dispose();
        }
        base.Dispose(disposing);
    }
}
