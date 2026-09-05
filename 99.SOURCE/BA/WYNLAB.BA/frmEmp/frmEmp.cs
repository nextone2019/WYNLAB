using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.BA;

/// <summary>
/// 사원등록 화면 - TEMPLATE을 복사해서 만듦(GENERIC_DATA_API.md의 범용 데이터 통로 사용, 서버
/// Controller/Repository 없음 - 메뉴등록(TSMMENU)의 PROC_PREFIX=USP_BA_EMP_ 만으로 동작).
///
/// 검색창(panHeader)의 사원번호/명(txtEmpNo_Q) + 부서(txtDeptNm_Q, 팝업)로 조회하고, 우측
/// panData에서 사번/사원명/부서를 등록·수정한다. 부서는 두 군데(검색창의 txtDeptNm_Q, 상세의
/// txtParDeptNm) 다 팝업(P_DEPT)에서 이름으로 찾으면 부서코드가 숨김 필드(txtDeptCd_Q/
/// txtDeptCd)에 같이 채워지는 멀티필드 팝업 모드를 쓴다(00.DEV/POPUP_FRAMEWORK_GUIDE.md 참고,
/// frmUserAuth.txtEmpNm과 같은 패턴).
///
/// grd1(목록)에서 행을 고르면 그 상세를 panData에 채우고(EnterEditMode), 저장 전이면
/// EnterNewMode로 비워둔다 - frmMinorCode/frmUserAuth가 전부 따르는 표준 패턴.
/// </summary>
public partial class frmEmp : BaseForm
{
    private DataTable _list = new();
    private string? _editingCd; // null이면 신규모드

    public frmEmp()
    {
        InitializeComponent();

        Text = "사원등록";

        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        // grd1은 조회전용 - 이 화면엔 편집 가능한 그리드가 따로 없다(등록/수정은 우측 panData).
        gvw1.Role = GridRoleWyn.Query;

        // 부서 팝업 매핑(멀티필드 모드) - MatchField가 이 컨트롤 자신이 대표하는 팝업 결과
        // 컬럼(dept_nm)을 가리키고, MapField가 나머지 컬럼(dept_cd)을 숨김 필드에 채운다.
        // 검색창의 부서(txtDeptNm_Q)와 상세패널의 부서(txtParDeptNm) 둘 다 같은 방식.
        txtDeptNm_Q.MatchField = "dept_nm";
        txtDeptNm_Q.MapField("dept_cd", txtDeptCd_Q);
        txtParDeptNm.MatchField = "dept_nm";
        txtParDeptNm.MapField("dept_cd", txtDeptCd);

        // 개발자용 마우스오버 툴팁(BindingField)이 읽어갈 정보 - 실제 적용은
        // BaseForm.ApplyBindingFieldTooltips가 공통으로 처리한다. txtParDeptNm은
        // PopupLookupEditWyn이라 "Popup : P_DEPT"도 자동으로 같이 붙는다.
        txtEmpNo.Tag = new BindingFieldTag("emp_no");
        txtEmpNm.Tag = new BindingFieldTag("emp_nm");
        txtDeptCd.Tag = new BindingFieldTag("dept_cd");
        txtParDeptNm.Tag = new BindingFieldTag("dept_nm");
        picEmpPhoto.Tag = new BindingFieldTag("photo");

        // 화면종료 시 저장 확인(BaseForm.ConfirmCloseAsync)이 panData의 값 변경을 감지할 수 있도록.
        TrackDirty(panData);

        EnterNewMode();
    }

    /// <summary>사용자가 툴바에서 직접 누른 조회 - 새 검색이므로 이전에 어느 행을 보고
    /// 있었는지는 무시하고 결과 1행부터 보여준다(preserveSelection: false). 저장/삭제 뒤의
    /// 재조회(QueryCore를 직접 호출)와는 다른 경로 - 그쪽은 방금 편집하던 행을 그대로
    /// 유지해야 한다(feedback_query_refocus_after_save 메모리 참고).</summary>
    public override async Task QueryClick() => await QueryCore(preserveSelection: false);

    /// <summary>목록 조회 - 사원번호/명(txtEmpNo_Q)은 LIKE로 둘 다 찾고, 부서(txtDeptCd_Q)는
    /// 선택했을 때만 그 부서로 좁힌다.</summary>
    private async Task QueryCore(bool preserveSelection)
    {
        var keyword = txtEmpNo_Q.Text.Trim();
        var deptCd = txtDeptCd_Q.Text.Trim();

        _list = await QueryAsync("USP_BA_EMP_Q", new
        {
            p_work_type = "Q",
            p_emp_no = keyword,
            p_dept_cd = deptCd
        });

        // preserveSelection이 false면(사용자가 직접 조회) 방금까지 편집하던 키를 일부러
        // 무시한다 - 그래야 아래에서 DevExpress가 자동으로 잡아준 0번 행이 그대로 유지된다.
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
            AppMessageBox.Show("삭제할 사원을 먼저 선택해주세요.", "안내");
            return;
        }

        var confirm = AppMessageBox.Show(
            $"선택하신 사원을 삭제 하시겠습니까?\n\n[{_editingCd}]",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await SaveAsync("USP_BA_EMP_S", new
        {
            p_work_type = "D",
            p_emp_no = _editingCd
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
        if (string.IsNullOrWhiteSpace(txtEmpNo.Text) || string.IsNullOrWhiteSpace(txtEmpNm.Text))
        {
            AppMessageBox.Show("사번과 사원명은 필수입니다.", "확인");
            return;
        }

        var wasNew = _editingCd == null;

        var result = await SaveAsync("USP_BA_EMP_S", new
        {
            p_work_type = wasNew ? "N" : "U",
            p_emp_no = wasNew ? txtEmpNo.Text : _editingCd,
            p_emp_nm = txtEmpNm.Text,
            p_dept_cd = string.IsNullOrWhiteSpace(txtDeptCd.Text) ? null : txtDeptCd.Text,
            // 사진을 안 건드렸어도(다른 필드만 고쳤어도) picEmpPhoto엔 EnterEditMode가 채워둔
            // 기존 사진이 그대로 들어있으므로 매번 다시 보내는 걸로 충분하다 - 별도 "바뀐 경우만
            // 보내기" 분기가 필요 없다. null이면 서버가 사진 없음(NULL)으로 저장한다.
            p_photo = picEmpPhoto.ImageBytes is { } bytes ? Convert.ToBase64String(bytes) : null
        });

        if (!result.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(result), "저장 실패");
            return;
        }

        _editingCd = wasNew ? result.GeneratedCode : _editingCd;
        await QueryCore(preserveSelection: true); // 방금 저장한 행 유지 - QueryClick(사용자 조회)과 다른 경로
        Toast.Show(wasNew ? "등록되었습니다." : "수정되었습니다.");
    }

    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
    {
        if (e.Row is DataRowView view) EnterEditMode(view.Row);
    }

    /// <summary>조회된 목록에서 키(emp_no)로 행을 찾는다. 없으면 null.</summary>
    private DataRow? FindRow(string cd) =>
        _list.Rows.Cast<DataRow>()
            .FirstOrDefault(r => string.Equals(Convert.ToString(r["emp_no"]), cd, StringComparison.OrdinalIgnoreCase));

    /// <summary>emp_no로 grd1의 행 핸들을 찾는다. 없으면 null.
    /// GridView.LocateByValue를 안 쓰는 이유는 ProcData.ToDataTable 주석 참고(JsonElement 비교 문제).</summary>
    private int? FindRowHandle(string cd)
    {
        var column = gvw1.Columns["emp_no"];
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
            txtEmpNo.Text = string.Empty;
            txtEmpNo.ReadOnly = false;
            txtEmpNm.Text = string.Empty;
            txtParDeptNm.Text = string.Empty;
            txtDeptCd.Text = string.Empty;
            picEmpPhoto.ImageBytes = null;
        });
        txtEmpNo.Focus();
    }

    private void EnterEditMode(DataRow row)
    {
        SuppressDirtyTracking(() =>
        {
            _editingCd = Str(row, "emp_no");
            txtEmpNo.Text = _editingCd;
            txtEmpNo.ReadOnly = true; // 사번은 PK라 수정 불가
            txtEmpNm.Text = Str(row, "emp_nm");
            txtDeptCd.Text = Str(row, "dept_cd");
            txtParDeptNm.Text = Str(row, "dept_nm");

            // photo 컬럼은 서버가 VARBINARY(전 IMAGE)를 응답 JSON에 Base64 문자열로 담아 보낸다
            // (System.Text.Json의 byte[] 기본 직렬화) - 그대로 디코딩만 하면 된다.
            var photoBase64 = Str(row, "photo");
            picEmpPhoto.ImageBytes = string.IsNullOrEmpty(photoBase64) ? null : Convert.FromBase64String(photoBase64);
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
