using System.Data;
using WYNLAB.Base;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.BA;

/// <summary>
/// 새 화면 개발용 템플릿(레이아웃 전용) - 사용법은 frmItemGrp.Designer.cs 상단 주석 참고.
///
/// grd1(목록)에서 행을 고르면 그 상세를 panData에 채우고(EnterEditMode), 저장 전이면
/// EnterNewMode로 비워둔다 - frmMinorCode/frmUserAuth가 전부 따르는 표준 패턴이다. 조회는
/// api/data/*(범용 데이터 통로, GENERIC_DATA_API.md 참고) 또는 화면 전용 API 중 편한 쪽을
/// 쓰면 된다 - 아래 QueryClick은 범용 통로(QueryAsync/SaveAsync) 예시로 채워뒀다.
/// </summary>
public partial class frmItemGrp : BaseForm
{
    private DataTable _list = new();
    private string? _editingCd; // null이면 신규모드

    public frmItemGrp()
    {
        InitializeComponent();

        Text = "품목그룹등록";
        MenuCd = "BA_ITEMGRP";

        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        // 화면종료 시 저장 확인(BaseForm.ConfirmCloseAsync)이 panData의 값 변경을 감지할 수
        // 있도록 - 하위 그리드(grd2)가 조회 전용이 아니라 편집 가능한 화면이면 그 DataTable도
        // TrackDirty(DataTable)로 똑같이 걸어줄 것(frmItem.cs의 _units 참고).
        TrackDirty(panData);

        EnterNewMode();
    }

    /// <summary>목록 조회 - 검색조건을 늘리려면 아래 익명 객체에 p_ 파라미터를 추가하고
    /// 프로시저에 같은 이름의 파라미터(기본값 NULL)와 WHERE 조건을 넣으면 된다.</summary>
    public override async Task QueryClick()
    {
        var keyword = txtSearchQ.Text.Trim();

        _list = await QueryAsync("USP_SM_TEMPLATE_Q", new
        {
            p_work_type = "Q",
            p_keyword = keyword
        });

        grd1.DataSource = _list;
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
