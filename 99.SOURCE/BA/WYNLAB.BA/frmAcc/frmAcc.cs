using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.BA;

/// <summary>
/// 사업장등록 화면 - TEMPLATE을 복사해서 만듦(GENERIC_DATA_API.md의 범용 데이터 통로 사용,
/// 서버 Controller/Repository 없음 - 메뉴등록(TSMMENU)의 PROC_PREFIX=USP_BA_ACC_ 만으로 동작).
///
/// 지금은 사업장코드/사업장명 2개 필드만 있다 - 자세한 컬럼정의(주소/사업자번호 등)는 나중에
/// 추가 예정. 필드를 늘릴 때는 DB(TBAACC 컬럼 + USP_BA_ACC_Q/S)와 화면(panData 컨트롤 +
/// EnterNewMode/EnterEditMode/SaveClick의 값 채우기/읽기) 양쪽을 같이 늘리면 된다.
///
/// grd1(목록)에서 행을 고르면 그 상세를 panData에 채우고(EnterEditMode), 저장 전이면
/// EnterNewMode로 비워둔다 - frmMinorCode/frmUserAuth가 전부 따르는 표준 패턴.
/// </summary>
public partial class frmAcc : BaseForm
{
    private DataTable _list = new();
    private string? _editingCd; // null이면 신규모드

    public frmAcc()
    {
        InitializeComponent();

        Text = "사업장등록";

        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        // grd1은 조회전용 - 이 화면엔 편집 가능한 그리드가 따로 없다(등록/수정은 우측 panData).
        gvw1.Role = GridRoleWyn.Query;

        // 개발자용 마우스오버 툴팁(BindingField)이 읽어갈 정보 - 실제 적용은
        // BaseForm.ApplyBindingFieldTooltips가 공통으로 처리한다. gridColumn1/2는 FieldName이
        // 이미 실제 DB 컬럼명이라 손댈 것 없이 자동 적용된다.
        txtAccCd.Tag = new BindingFieldTag("acc_cd");
        txtAccNm.Tag = new BindingFieldTag("acc_nm");

        // 화면종료 시 저장 확인(BaseForm.ConfirmCloseAsync)이 panData의 값 변경을 감지할 수 있도록.
        TrackDirty(panData);

        EnterNewMode();
    }

    /// <summary>사용자가 툴바에서 직접 누른 조회 - 새 검색이므로 이전 선택은 무시하고 1행부터
    /// 보여준다(preserveSelection: false). 저장/삭제 뒤 재조회는 QueryCore를 직접 호출해서
    /// 방금 편집하던 행을 유지한다(feedback_query_refocus_after_save 메모리 참고).</summary>
    public override async Task QueryClick() => await QueryCore(preserveSelection: false);

    /// <summary>목록 조회 - 검색어 하나로 사업장코드/사업장명 둘 다(LIKE) 찾는다.</summary>
    private async Task QueryCore(bool preserveSelection)
    {
        var keyword = txtSearchQ.Text.Trim();

        _list = await QueryAsync("USP_BA_ACC_Q", new
        {
            p_work_type = "Q",
            p_acc_cd = keyword,
            p_acc_nm = keyword
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
            $"선택하신 사업장을 삭제 하시겠습니까?\n\n[{_editingCd}]",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await SaveAsync("USP_BA_ACC_S", new
        {
            p_work_type = "D",
            p_acc_cd = _editingCd
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

    // 이 화면엔 하위 그리드가 없다 - BaseForm 추상 멤버라 구현만 비워둔다.
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;

    public override async Task SaveClick()
    {
        if (string.IsNullOrWhiteSpace(txtAccCd.Text) || string.IsNullOrWhiteSpace(txtAccNm.Text))
        {
            AppMessageBox.Show("사업장코드와 사업장명은 필수입니다.", "확인");
            return;
        }

        var wasNew = _editingCd == null;

        var result = await SaveAsync("USP_BA_ACC_S", new
        {
            p_work_type = wasNew ? "N" : "U",
            p_acc_cd = wasNew ? txtAccCd.Text : _editingCd,
            p_acc_nm = txtAccNm.Text
        });

        if (!result.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(result), "저장 실패");
            return;
        }

        _editingCd = wasNew ? result.GeneratedCode : _editingCd;
        await QueryCore(preserveSelection: true); // 방금 저장한 행 유지
        Toast.Show(wasNew ? "등록되었습니다." : "수정되었습니다.");
    }

    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
    {
        if (e.Row is DataRowView view) EnterEditMode(view.Row);
    }

    /// <summary>조회된 목록에서 키(acc_cd)로 행을 찾는다. 없으면 null.</summary>
    private DataRow? FindRow(string cd) =>
        _list.Rows.Cast<DataRow>()
            .FirstOrDefault(r => string.Equals(Convert.ToString(r["acc_cd"]), cd, StringComparison.OrdinalIgnoreCase));

    /// <summary>acc_cd로 grd1의 행 핸들을 찾는다. 없으면 null.
    /// GridView.LocateByValue를 안 쓰는 이유는 ProcData.ToDataTable 주석 참고(JsonElement 비교 문제).</summary>
    private int? FindRowHandle(string cd)
    {
        var column = gvw1.Columns["acc_cd"];
        if (column == null) return null;

        for (var handle = 0; handle < gvw1.RowCount; handle++)
        {
            if (string.Equals(Convert.ToString(gvw1.GetRowCellValue(handle, column)), cd, StringComparison.OrdinalIgnoreCase))
                return handle;
        }
        return null;
    }

    /// <summary>panData를 채우는 부분은 반드시 SuppressDirtyTracking으로 감쌀 것 - 코드가 값을
    /// 채우는 것뿐인데 TrackDirty(panData)가 "사용자가 고쳤다"로 오인하면 재조회/신규모드 진입
    /// 직후부터 화면을 닫을 때 저장 확인이 뜨는 오작동이 생긴다.</summary>
    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingCd = null;
            txtAccCd.Text = string.Empty;
            txtAccCd.ReadOnly = false;
            txtAccNm.Text = string.Empty;
        });
        txtSearchQ.Focus();
    }

    private void EnterEditMode(DataRow row)
    {
        SuppressDirtyTracking(() =>
        {
            _editingCd = Str(row, "acc_cd");
            txtAccCd.Text = _editingCd;
            txtAccCd.ReadOnly = true; // 사업장코드는 PK라 수정 불가
            txtAccNm.Text = Str(row, "acc_nm");
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
}
