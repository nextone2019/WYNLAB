using System.Data;
using WYNLAB.Base;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.SM.TEMPLATE;

/// <summary>
/// 새 화면 개발용 템플릿(레이아웃 전용) - 사용법은 TemplateForm.Designer.cs 상단 주석 참고.
///
/// grd1(목록)에서 행을 고르면 그 상세를 panData에 채우고(EnterEditMode), 저장 전이면
/// EnterNewMode로 비워둔다 - frmMinorCode/frmUserAuth가 전부 따르는 표준 패턴이다. 조회는
/// api/data/*(범용 데이터 통로, GENERIC_DATA_API.md 참고) 또는 화면 전용 API 중 편한 쪽을
/// 쓰면 된다 - 아래 QueryClick은 범용 통로(QueryAsync/SaveAsync) 예시로 채워뒀다.
///
/// QueryClick은 저장 후 재조회에서도 방금 편집하던 행을 다시 찾아 포커스를 복원한다(그냥
/// EnterNewMode로 비우면 저장 직후 패널이 비어 보인다 - frmDept에서 실제로 겪은 뒤 여기에
/// 기본값으로 반영함, 2026-08-28). 이 구조는 지우지 말고 "cd" 자리만 실제 키 컬럼명으로
/// 바꿔서 쓸 것 - grd1이 아니라 트리(tree1)를 쓰는 화면이면 frmDept.QueryClick을 참고해서
/// FindNodeByFieldValue 버전으로 바꾼다.
///
/// 화면종료 시 저장 확인(생성자의 TrackDirty(panData), EnterNewMode/EnterEditMode의
/// SuppressDirtyTracking)도 기본으로 들어가 있다 - 새 화면을 만들 때 이 구조 역시 지우지 말 것.
/// grd2가 조회 전용이 아니라 편집 가능한 화면이면 그 DataTable에도 TrackDirty(그 테이블)를
/// 추가로 걸어야 한다(frmItem.cs의 _units 참고). BaseForm.ConfirmCloseAsync가 개별 탭 X버튼과
/// ShellForm의 일괄닫기 양쪽에서 공통으로 이 dirty 상태를 확인해 "{화면명} 화면의 변경 내역이
/// 존재 합니다..." 확인창을 띄운다 - 화면 쪽에서 닫기 확인 로직을 따로 만들 필요는 없다.
/// </summary>
public partial class TemplateForm : BaseForm
{
    private DataTable _list = new();
    private string? _editingCd; // null이면 신규모드

    public TemplateForm()
    {
        InitializeComponent();

        Text = "화면명";
        MenuCd = "MENU_CD_HERE";

        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        // 화면종료 시 저장 확인(BaseForm.ConfirmCloseAsync)이 panData의 값 변경을 감지할 수
        // 있도록 - 하위 그리드(grd2)가 조회 전용이 아니라 편집 가능한 화면이면 그 DataTable도
        // TrackDirty(DataTable)로 똑같이 걸어줄 것(frmItem.cs의 _units 참고).
        TrackDirty(panData);

        EnterNewMode();
    }

    /// <summary>사용자가 툴바(또는 Ctrl+Q)에서 직접 누른 조회 - 새 검색이므로 이전에 어느
    /// 행을 보고 있었는지는 무시하고 결과 1행부터 보여준다(preserveSelection: false). 저장/
    /// 삭제 뒤의 재조회는 이 메서드가 아니라 QueryCore를 직접 preserveSelection: true로
    /// 불러야 한다 - 그래야 방금 편집하던 행이 그대로 유지된다(아래 QueryCore 설명 참고,
    /// feedback_query_refocus_after_save 메모리). 새 화면을 만들 때 이 구조 자체는 지우지
    /// 말 것 - "조회를 누르면 1행부터"와 "저장하면 방금 그 행 유지"는 서로 다른 요구라 하나의
    /// 메서드로 뭉쳐서 처리하면 둘 중 하나는 항상 깨진다(실제로 frmEmp에서 이 둘을 구분 안
    /// 해서 "조회를 눌러도 이전 선택이 그대로 남는다"는 문제를 겪었다, 2026-08-31).</summary>
    public override async Task QueryClick() => await QueryCore(preserveSelection: false);

    /// <summary>목록 조회 - 검색조건을 늘리려면 아래 익명 객체에 p_ 파라미터를 추가하고
    /// 프로시저에 같은 이름의 파라미터(기본값 NULL)와 WHERE 조건을 넣으면 된다.
    ///
    /// [반드시 이 형태를 유지할 것] 저장(SaveClick)/삭제(DeleteClick)가 끝나면 이 메서드를
    /// preserveSelection: true로 다시 부르는데, 여기서 무조건 EnterNewMode로 비우거나
    /// grd1.DataSource만 갈아끼우고 끝내면 "저장은 됐는데 방금 편집하던 행이 패널에 안
    /// 보인다"는 문제가 반드시 생긴다(빈 화면이거나, DevExpress가 임의로 고른 엉뚱한 행이
    /// 대신 뜬다) - frmDept에서 실제로 겪었고 grd1을 쓰는 화면도 원리가 같아서 똑같이 겪는다.
    /// 그래서 preserveSelection이 true일 때는: ① 방금까지 편집 중이던 키를 새 목록에서 다시
    /// 찾고 ② 그 행에 포커스를 옮기되 재바인딩 중 발생하는 FocusedRowObjectChanged로 엉뚱한
    /// 행이 먼저 걸리지 않도록 이벤트를 잠깐 끊었다가 ③ 마지막에 그 행으로 EnterEditMode
    /// (없으면 EnterNewMode)를 명시적으로 한 번만 부른다. preserveSelection이 false일 때는
    /// editingCd 자체를 null로 취급해서 아래 로직이 자연스럽게 "0번 행 기준"으로 흐르게 한다.
    ///
    /// editingCd로 찾지 못했을 때(preserveSelection: false 포함, 삭제 후 등)도 grd1.DataSource를
    /// 새로 갈아끼우면 DevExpress가 0번 행에 자동으로 포커스를 준다 - 근데 이건
    /// FocusedRowObjectChanged를 거치지 않는 "조용한" 포커스라서 그 행 데이터가 패널에 안
    /// 채워진 채로 남는다(그리드에 1건만 있으면 사용자가 클릭해도 이미 포커스된 행이라 아무
    /// 일도 안 일어나 "선택해도 패널에 안 뿌려진다"로 보인다 - frmSysPopup에서 실제로 겪음).
    /// 그래서 editingCd로 못 찾았어도 grd1이 이미 포커스해둔 행이 있으면 그걸로 EnterEditMode를
    /// 한 번 불러준다.
    /// 새 화면을 만들 때 이 구조 자체는 지우지 말고, "cd" 자리만 실제 키 컬럼명으로 바꿀 것.</summary>
    private async Task QueryCore(bool preserveSelection)
    {
        var keyword = txtSearchQ.Text.Trim();

        _list = await QueryAsync("USP_SM_TEMPLATE_Q", new
        {
            p_work_type = "Q",
            p_keyword = keyword
        });

        var editingCd = preserveSelection ? _editingCd : null;

        gvw1.FocusedRowObjectChanged -= Gvw1_FocusedRowObjectChanged;
        try
        {
            grd1.DataSource = _list;

            if (editingCd != null)
            {
                var handle = FindRowHandle(editingCd);
                if (handle != null) gvw1.FocusedRowHandle = handle.Value;
            }
        }
        finally
        {
            gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;
        }

        var row = editingCd == null ? null : FindRow(editingCd);
        if (row != null) EnterEditMode(row);
        else if (gvw1.GetFocusedRow() is DataRowView focusedView) EnterEditMode(focusedView.Row);
        else EnterNewMode();
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    public override async Task DeleteClick()
    {
        if (_editingCd == null)
        {
            AppMessageBox.Show("삭제할 항목을 먼저 선택해주세요.", "안내");
            return;
        }

        var confirm = AppMessageBox.Show(
            $"선택하신 항목을 삭제 하시겠습니까?\n\n[{_editingCd}]",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await SaveAsync("USP_SM_TEMPLATE_S", new
        {
            p_work_type = "D",
            p_cd = _editingCd
        });

        if (!result.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(result), "삭제 실패");
            return;
        }

        _editingCd = null;
        await QueryClick();
        Toast.Show("삭제되었습니다.");
    }

    public override Task NewRowClick()
    {
        // 하위 그리드(grd2)에 새 행을 추가하는 자리 - frmMinorCode.NewRowClick 참고
        // (상위가 아직 저장 전이면 막는 등 필요한 가드는 여기서 추가).
        return Task.CompletedTask;
    }

    public override Task DeleteRowClick()
    {
        // 하위 그리드(grd2)에서 포커스 행을 지우는 자리 - frmMinorCode.DeleteRowClick 참고.
        return Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        var wasNew = _editingCd == null;

        var result = await SaveAsync("USP_SM_TEMPLATE_S", new
        {
            p_work_type = wasNew ? "N" : "U",
            p_cd = _editingCd
        });

        if (!result.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(result), "저장 실패");
            return;
        }

        _editingCd = wasNew ? result.GeneratedCode : _editingCd;
        await QueryClick();
        Toast.Show(wasNew ? "등록되었습니다." : "수정되었습니다.");
    }

    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
    {
        if (e.Row is DataRowView view) EnterEditMode(view.Row);
    }

    /// <summary>조회된 목록에서 키로 행을 찾는다. 없으면 null.</summary>
    private DataRow? FindRow(string cd) =>
        _list.Rows.Cast<DataRow>()
            .FirstOrDefault(r => string.Equals(Convert.ToString(r["cd"]), cd, StringComparison.OrdinalIgnoreCase));

    /// <summary>cd로 grd1의 행 핸들을 찾는다. 없으면 null.
    ///
    /// GridView.LocateByValue를 안 쓰는 이유: DataTable 컬럼이 실제로는 JsonElement를 담고
    /// 있어서(ProcData.ToDataTable 참고) 순수 string과 절대 같다고 판정되지 않는다 -
    /// Convert.ToString으로 비교하는 이 방식이 frmMinorCode에서 검증된 방법이다.</summary>
    private int? FindRowHandle(string cd)
    {
        var column = gvw1.Columns["cd"];
        if (column == null) return null;

        for (var handle = 0; handle < gvw1.RowCount; handle++)
        {
            if (string.Equals(Convert.ToString(gvw1.GetRowCellValue(handle, column)), cd, StringComparison.OrdinalIgnoreCase))
                return handle;
        }
        return null;
    }

    /// <summary>panData를 채우는 부분은 반드시 SuppressDirtyTracking으로 감쌀 것 - 코드가
    /// 값을 채우는 것뿐인데 TrackDirty(panData)가 "사용자가 고쳤다"로 오인하면 재조회/신규모드
    /// 진입 직후부터 화면을 닫을 때 저장 확인이 뜨는 오작동이 생긴다.</summary>
    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingCd = null;
            // panData 컨트롤을 여기서 전부 비운다(디자이너로 컨트롤을 추가한 뒤 채울 것).
        });
        txtSearchQ.Focus();
    }

    private void EnterEditMode(DataRow row)
    {
        SuppressDirtyTracking(() =>
        {
            _editingCd = Str(row, "cd");
            // panData 컨트롤을 여기서 row 값으로 채운다(디자이너로 컨트롤을 추가한 뒤 채울 것).
        });
    }

    private static string Str(DataRow row, string columnName) =>
        row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value
            ? Convert.ToString(row[columnName]) ?? string.Empty
            : string.Empty;

    /// <summary>ApiResult.ErrorCode는 SQL 예외(ERROR_NUMBER())일 때만 채워진다(0이면 업무로직
    /// 판단만으로 실패 - 예: 필수값 누락) - 그럴 때만 메시지에 오류번호를 같이 보여준다.</summary>
    private static string FormatSaveFailMessage(ApiResult? result)
    {
        var message = result?.Message ?? "저장에 실패했습니다.";
        return result is { ErrorCode: not 0 } ? $"{message} (오류코드: {result.ErrorCode})" : message;
    }

    // 하위 그리드 헤더의 +/- 버튼(Designer.cs가 이 두 메서드에 직접 연결)이 NewRowClick/
    // DeleteRowClick을 그대로 호출한다 - 생성자에서 또 구독하면 클릭 한 번에 두 번씩 불리므로
    // 연결은 여기 이 두 메서드 하나로만 존재해야 한다(frmMinorCode에서 실제로 겪은 문제).
    private async void btnAddRow2_Click(object sender, EventArgs e)
    {
        await NewRowClick();
    }

    private async void btnDeletRow2_Click(object sender, EventArgs e)
    {
        await DeleteRowClick();
    }
}
