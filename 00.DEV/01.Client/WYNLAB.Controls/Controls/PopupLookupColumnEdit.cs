using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace WYNLAB.Base.Controls;

/// <summary>임시 진단 로그(2026-09-25) - 그리드 품번 팝업이 "아무 반응 없음"인 원인을 사용자에게 묻지
/// 않고 직접 확인하려고 %LocalAppData%\WYNLAB\popup-debug.log에 남긴다. 원인 확정 후 제거한다.</summary>
internal static class PopupDebugLog
{
    public static void Write(string message)
    {
        try
        {
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WYNLAB");
            Directory.CreateDirectory(dir);
            File.AppendAllText(Path.Combine(dir, "popup-debug.log"), $"[{DateTime.Now:HH:mm:ss.fff}] {message}{Environment.NewLine}");
        }
        catch
        {
        }
    }
}

/// <summary>
/// PopupLookupEditWyn(패널용 독립 컨트롤)의 팝업(sysPopUpM.popup_key) 연결을 그리드 컬럼 편집기
/// (GridColumn.ColumnEdit)에서도 그대로 쓰기 위한 RepositoryItem 버전 - LookUpColumnEdit과 같은
/// 발상/등록 방식이다(그 클래스 설명 참고, 이 클래스는 컬럼당 하나씩 만든다는 점까지 동일).
/// AI Builder의 Control="POP"이 grd1/grd2/grd3 컬럼에서도 실제 팝업(...버튼)을 쓸 수 있게 하려고
/// 추가했다(2026-09-07, 처음엔 panData 상세폼만 지원하고 그리드는 미뤄뒀었음).
///
/// LookUpColumnEdit과 다른 점: LookUpEditWyn.LookupKey는 RepositoryItemLookUpEdit 자체가 가진
/// DataSource/Columns/ValueMember 같은 표준 프로퍼티를 그대로 채우는 것이라, 그리드가 셀 편집기를
/// 만들 때 Properties를 이 RepositoryItem 인스턴스로 그대로 공유해주는 것만으로 자동 연결된다.
/// 반면 PopupLookupEditWyn.LookupKey는 표준 RepositoryItemButtonEdit엔 없는 이 컨트롤만의 커스텀
/// 프로퍼티라, 셀 편집기(런타임에 새로 만들어지는 PopupLookupEditWyn 인스턴스)가 자동으로 이 값을
/// 물려받지 않는다 - 그래서 PopupLookupEditWyn.LookupKey 게터가 자기 필드가 비어있으면
/// (Properties as PopupLookupColumnEdit)?.LookupKey로 폴백하도록 따로 손봤다(PopupLookupEditWyn.cs
/// 참고) - 그리드 편집 중엔 실제로 Properties가 이 인스턴스를 가리키므로 그 경로로 값이 전달된다.
///
/// 클릭/더블클릭 이벤트는 반드시 이 RepositoryItem에 건다(생성자 아래 참고) - PopupLookupEditWyn
/// 생성자에서 `ButtonClick += ...`처럼 거는 이벤트는 ButtonEdit이 Properties(RepositoryItem)로
/// 전달(forward)하는 이벤트라서, 그리드가 셀 편집기를 만든 뒤 Properties를 이 공유 아이템으로
/// 교체하는 순간 생성자 때 걸어둔 구독은 옛 Properties와 함께 버려진다 - 그래서 그리드 안에서는
/// "..." 버튼이 아무 반응도 없었다(2026-09-23~25 실제 겪음).
/// </summary>
[ToolboxItem(true)]
public class PopupLookupColumnEdit : RepositoryItemButtonEdit
{
    public const string CustomEditName = "PopupLookupColumnEdit";

    static PopupLookupColumnEdit()
    {
        // DevExpress 자체 "ButtonEdit" 등록 엔트리를 그대로 리플렉션해서 확인한 EditorType 이름 -
        // 설치된 DevExpress.XtraEditors.v21.2.dll을 직접 리플렉션해서 맞췄다(추측 금지 컨벤션,
        // LookUpColumnEdit 클래스 설명의 ControlNavigator 사례와 같은 이유). EditorType만
        // PopupLookupEditWyn으로 바꾸고 ViewInfo/Painter는 ButtonEdit 것을 그대로 재사용한다.
        EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(
            CustomEditName,
            typeof(PopupLookupEditWyn),
            typeof(PopupLookupColumnEdit),
            typeof(ButtonEditViewInfo),
            new ButtonEditPainter(),
            true));
    }

    public PopupLookupColumnEdit()
    {
        NullText = string.Empty;

        // "..." 버튼 클릭과 셀 더블클릭 둘 다 같은 팝업을 연다.
        ButtonClick += (s, e) => OpenFromEditor(s, "ButtonClick");
        DoubleClick += (s, e) => OpenFromEditor(s, "DoubleClick");
    }

    private bool _opening;

    /// <summary>팝업에서 고른 값을 SetRowCellValue로 써넣는 동안 true - 그 쓰기가 일으키는
    /// CellValueChanged가 다시 팝업을 여는 것(GridViewWynBehavior.OnCellValueChanged)을 막는다.</summary>
    internal bool IsApplyingResult { get; private set; }

    private void OpenFromEditor(object? sender, string source)
    {
        PopupDebugLog.Write($"{source} 발생. sender={sender?.GetType().Name ?? "null"}, Parent={(sender as Control)?.Parent?.GetType().Name ?? "null"}");

        if (sender is not Control editor || editor.Parent is not GridControl grid || grid.FocusedView is not GridView view)
        {
            PopupDebugLog.Write($"{source}: 그리드를 못 찾아 중단");
            return;
        }

        var column = view.FocusedColumn;
        var rowHandle = view.FocusedRowHandle;
        if (column == null || rowHandle == GridControl.InvalidRowHandle) return;

        _ = OpenPopupForCellAsync(view, rowHandle, column, userOpened: true);
    }

    /// <summary>PopupLookupColumnEdit의 LookupKey로 공용 팝업을 열고, 고른 결과(키값)를 그 셀에
    /// SetRowCellValue로 써넣는다. 이 한 번의 SetRowCellValue가 CellValueChanged를 정상적으로
    /// 발생시키므로, 화면의 CellValueChanged 핸들러(품번 -> 품명/규격/단위 자동채움 등)는 그대로
    /// 동작한다. 호출부가 전부 fire-and-forget이라 예외를 여기서 직접 잡아 보여준다(안 잡으면 아무도
    /// 그 Task를 기다리지 않아 조용히 사라진다 - PopupLookupEditWyn.ButtonClick과 같은 함정).</summary>
    internal async Task OpenPopupForCellAsync(GridView view, int rowHandle, GridColumn column, bool userOpened = false)
    {
        if (_opening) return;
        _opening = true;
        try
        {
            PopupDebugLog.Write($"OpenPopupForCellAsync 진입. LookupKey={LookupKey ?? "null"}, OpenPopup등록={PopupLookupProvider.OpenPopup != null}, row={rowHandle}, col={column.FieldName}");
            if (string.IsNullOrEmpty(LookupKey) || PopupLookupProvider.OpenPopup == null) return;

            view.FocusedRowHandle = rowHandle;
            // "..." 버튼/더블클릭으로 직접 연 경우(userOpened)엔 셀에 이미 든 값(예: 방금 고른 품번)을 검색어로 쓰지 않는다 - 쓰면 그 품번 1건만
            // 검색돼서 "결과 1건이면 팝업 생략" 규칙(popPopUp.ShowAsync)에 걸려 팝업이 아예 안 뜬다. 값을 직접 타이핑해서 바뀐 경우에만 검색어로 쓴다.
            var keyword = userOpened ? null : Convert.ToString(view.ActiveEditor?.EditValue ?? view.GetRowCellValue(rowHandle, column));
            PopupLookupResult? result;
            PopupLookupProvider.ExtraConditions = ConditionProvider?.Invoke();
            try { result = await PopupLookupProvider.OpenPopup(LookupKey!, view.GridControl, string.IsNullOrEmpty(keyword) ? null : keyword); }
            finally { PopupLookupProvider.ExtraConditions = null; }
            PopupDebugLog.Write($"OpenPopup 반환. result={(result == null ? "null(취소)" : result.Code)}");
            if (result == null) return;

            // 팝업이 떠 있는 동안 활성 편집기가 옛 텍스트를 들고 있어서, 그대로 두면 포커스가 빠질 때
            // 옛 값이 다시 커밋되어 방금 고른 값을 덮어쓴다 - 편집을 취소하고 셀에 직접 쓴다.
            view.HideEditor();
            IsApplyingResult = true;
            try { view.SetRowCellValue(rowHandle, column, result.Code); }
            finally { IsApplyingResult = false; }
            RaiseResultSelected(rowHandle, result);
        }
        catch (Exception ex)
        {
            PopupDebugLog.Write($"예외: {ex}");
            MessageBox.Show($"팝업을 여는 중 오류가 발생했습니다.\n{ex.Message}", "오류");
        }
        finally
        {
            _opening = false;
        }
    }

    /// <summary>ButtonEdit 계열 공통 버그(LookUpColumnEdit.EndInit 참고) - Designer의 BeginInit/
    /// EndInit 구간을 지나면서 코드로 등록하지 않은 버튼이 비워질 수 있어서, 여기서 다시 채워
    /// 넣는다. 화면마다 Designer.cs에 Buttons.AddRange를 직접 안 넣어도 항상 "..." 버튼이 보인다.</summary>
    public override void EndInit()
    {
        base.EndInit();
        if (Buttons.Count == 0)
        {
            Buttons.Add(new EditorButton(ButtonPredefines.Ellipsis, "...") { Width = 24 });
        }
    }

    public override string EditorTypeName => CustomEditName;

    /// <summary>어느 팝업을 열지(sysPopUpM.popup_key), 예: "P_MENU". PopupLookupEditWyn.LookupKey와
    /// 완전히 같은 방식 - 이 값만 지정하면 셀 편집 중 "..." 버튼으로 팝업이 뜬다.</summary>
    [Category("WYNLAB")]
    [Description("sysPopUpM에 등록해둔 팝업 이름(popup_key). 이 값만 지정하면 \"...\" 버튼으로 팝업이 뜹니다.")]
    [DefaultValue(null)]
    public string? LookupKey { get; set; }

    /// <summary>팝업을 열 때마다 호출돼서 "화면 상태에서 온 추가 조회조건"(예: p_cust_id, p_base_date)을 돌려주는 콜백 -
    /// 화면 코드에서 지정한다(Designer 저장 안 함). 값은 PopupLookupProvider.ExtraConditions로 팝업 엔진에 전달된다.</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Func<Dictionary<string, string?>>? ConditionProvider { get; set; }

    /// <summary>팝업에서 행을 골라 셀에 값을 써넣은 "직후" 발생(rowHandle, 팝업 결과 전체 행) - 셀 하나만 채워지는
    /// 그리드에서, 같은 행의 다른 칸(예: 단가)을 팝업 결과 행의 컬럼으로 바로 채우고 싶을 때 쓴다.</summary>
    public event Action<int, PopupLookupResult>? ResultSelected;

    internal void RaiseResultSelected(int rowHandle, PopupLookupResult result) => ResultSelected?.Invoke(rowHandle, result);
}
