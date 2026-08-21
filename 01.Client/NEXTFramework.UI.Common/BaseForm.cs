using DevExpress.XtraEditors;

namespace NEXTFramework.UI.Common;

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
    /// 값의 실제 출처는 서버의 USP_SM_GetUserSession 프로시저 결과.
    /// </summary>
    protected string CurrentUserId => SessionManager.Current.UserInfo?.UserId ?? string.Empty;
    protected string CurrentUserNm => SessionManager.Current.UserInfo?.UserNm ?? string.Empty;
    protected string CurrentEmpNo => SessionManager.Current.UserInfo?.EmpNo ?? string.Empty;
    protected string CurrentDeptCd => SessionManager.Current.UserInfo?.DeptCd ?? string.Empty;
    protected string CurrentDeptNm => SessionManager.Current.UserInfo?.DeptNm ?? string.Empty;
    protected string CurrentPositionNm => SessionManager.Current.UserInfo?.PositionNm ?? string.Empty;
    protected bool CurrentIsAdmin => SessionManager.Current.UserInfo?.IsAdminYn ?? false;

    // ===== MDI 상단 공통 툴바(조회/입력/저장/삭제/출력)가 호출하는 표준 액션 =====
    // Shell의 툴바 버튼은 현재 활성화된 MDI 자식폼(this)의 아래 메서드를 그대로 호출한다.
    // 화면마다 필요한 것만 override 하면 되고, 안 쓰는 기능은 기본값(아무 동작 안 함)으로 둔다.
    public virtual Task QueryAsync() => Task.CompletedTask;
    public virtual Task NewAsync() => Task.CompletedTask;
    public virtual Task SaveAsync() => Task.CompletedTask;
    public virtual Task DeleteAsync() => Task.CompletedTask;
    public virtual Task PrintAsync() => Task.CompletedTask;

    public BaseForm()
    {
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
    /// 공통 예외처리 - 업무화면에서 try/catch 없이 이 메서드로 감싸서 호출.
    /// 로그 적재 + 사용자에게 표준화된 에러 메시지 표시.
    /// </summary>
    protected void SafeExecute(Action action, string actionNm = "")
    {
        try
        {
            this.Cursor = Cursors.WaitCursor;
            action();
        }
        catch (Exception ex)
        {
            // TODO: 공통 로거(Serilog 등) 연동
            XtraMessageBox.Show($"[{actionNm}] 처리 중 오류가 발생했습니다.\n{ex.Message}",
                "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// API 호출 등 비동기 작업용 SafeExecute. 조회/저장/삭제 버튼 클릭 핸들러에서 주로 사용.
    /// </summary>
    protected async Task SafeExecuteAsync(Func<Task> action, string actionNm = "")
    {
        try
        {
            this.Cursor = Cursors.WaitCursor;
            await action();
        }
        catch (Exception ex)
        {
            XtraMessageBox.Show($"[{actionNm}] 처리 중 오류가 발생했습니다.\n{ex.Message}",
                "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }
}
