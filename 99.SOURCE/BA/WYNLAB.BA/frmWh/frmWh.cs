using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.BA;

/// <summary>
/// 창고/위치등록 화면 - panData에 창고정보(TBAWH), grd2에 그 창고에 속한 위치 목록(TBALOC)을
/// 한 화면에서 등록한다(frmMinorCode의 대분류/소분류 구조와 같은 원칙 - grd1에서 창고를 고르면
/// panData/grd2가 그 창고 기준으로 채워지고, grd2는 각 행마다 USP_BA_WH_LOC_S로 저장된다).
/// wh_id/loc_id 둘 다 IDENTITY라 frmMinorCode(사용자가 코드를 직접 입력하는 방식)와 달리 신규
/// 등록 시 서버가 돌려주는 값을 그대로 쓴다(frmDept/frmAcc와 같은 방식).
/// </summary>
public partial class frmWh : BaseForm
{
    private DataTable _whs = new();
    private DataTable _locs = new();
    private string? _editingWhId; // null이면 신규모드

    public frmWh()
    {
        InitializeComponent();

        Text = "창고/위치등록";

        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        // grd1(창고 목록)은 조회전용 - 등록/수정은 우측 panData에서.
        gvw1.Role = GridRoleWyn.Query;

        // grd2(위치)는 편집 가능 - 네비게이터의 추가/삭제 버튼도 이미 있는 검증 로직
        // (NewRowClick/DeleteRowClick, 창고 미저장 시 막는 등)을 그대로 태운다. 헤더의 +/- 버튼과
        // 네비게이터 버튼 둘 다 같은 메서드를 부르므로 동작이 어긋나지 않는다.
        gvw2.Role = GridRoleWyn.Edit;
        gvw2.HighlightUnsavedCells = true;
        gvw2.RowAdd += async (s, e) => await NewRowClick();
        gvw2.RowDelete += async (s, e) => await DeleteRowClick();

        // 개발자용 마우스오버 툴팁(BindingField).
        cboDetailAccId.Tag = new BindingFieldTag("acc_id");
        txtDetailWhNm.Tag = new BindingFieldTag("wh_nm");
        txtDetailWhType.Tag = new BindingFieldTag("wh_type");
        txtDetailDeptId.Tag = new BindingFieldTag("dept_id");
        popDetailDeptNm.Tag = new BindingFieldTag("dept_nm");
        txtDetailEmpId.Tag = new BindingFieldTag("emp_id");
        popDetailEmpNm.Tag = new BindingFieldTag("emp_nm");

        // 담당부서/담당자 팝업(멀티필드 모드) - frmItem의 창고/위치 팝업과 같은 방식. 이 컨트롤
        // 자신이 명칭(MatchField)이고, 옆의 읽기전용 텍스트박스에 MapField로 ID를 채운다.
        // MapField의 첫 번째 인자는 팝업 결과 행의 실제 컬럼명이어야 한다 - P_DEPT/P_EMP는
        // SSP_POP_DEPT_Q/SSP_POP_EMP_Q가 각각 DEPT_ID/EMP_ID를 대문자로 SELECT하고 있어서
        // (P_WH/P_LOC은 내가 새로 만들며 소문자로 썼음) 소문자 "dept_id"/"emp_id"로는 못 찾는다
        // (2026-09-15 실제 발견 - popPopUp.RowToDict가 DataColumn.ColumnName을 그대로 키로 써서
        // 대소문자를 그대로 구분한다).
        popDetailDeptNm.MatchField = "dept_nm";
        popDetailDeptNm.MapField("DEPT_ID", txtDetailDeptId);
        popDetailEmpNm.MatchField = "emp_nm";
        popDetailEmpNm.MapField("EMP_ID", txtDetailEmpId);

        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick() => await QueryCore(preserveSelection: false);

    private async Task QueryCore(bool preserveSelection)
    {
        var keyword = txtWhNm_Q.Text.Trim();

        _whs = await QueryAsync("USP_BA_WH_Q", new
        {
            p_work_type = "Q",
            p_wh_nm = keyword
        });

        var editingWhId = preserveSelection ? _editingWhId : null;

        gvw1.FocusedRowObjectChanged -= Gvw1_FocusedRowObjectChanged;
        SuppressMasterRowSwitchConfirm(gvw1, () =>
        {
            grd1.DataSource = _whs;
            if (editingWhId != null)
            {
                var handle = FindRowHandle(editingWhId);
                if (handle != null) gvw1.FocusedRowHandle = handle.Value;
            }
        });
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        var row = editingWhId == null ? null : FindRow(editingWhId);
        if (row != null) await EnterEditModeAsync(row);
        else if (gvw1.GetFocusedRow() is DataRowView focusedView) await EnterEditModeAsync(focusedView.Row);
        else EnterNewMode();
    }

    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e) =>
        ConfirmMasterRowSwitch(gvw1, e, row => _ = EnterEditModeAsync(row.Row));

    private DataRow? FindRow(string whId) =>
        _whs.Rows.Cast<DataRow>()
            .FirstOrDefault(r => string.Equals(Convert.ToString(r["wh_id"]), whId, StringComparison.OrdinalIgnoreCase));

    private int? FindRowHandle(string whId)
    {
        var row = FindRow(whId);
        if (row == null) return null;

        var rowIndex = _whs.Rows.IndexOf(row);
        var handle = gvw1.GetRowHandle(rowIndex);
        return handle >= 0 ? handle : null;
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingWhId = null;
            cboDetailAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
            txtDetailWhNm.Text = string.Empty;
            txtDetailWhType.Text = string.Empty;
            txtDetailDeptId.Text = string.Empty;
            popDetailDeptNm.Text = string.Empty;
            txtDetailEmpId.Text = string.Empty;
            popDetailEmpNm.Text = string.Empty;

            _locs = _locs.Clone();
            TrackDirty(_locs);
            grd2.DataSource = _locs;
        });
        txtDetailWhNm.Focus();
    }

    private async Task EnterEditModeAsync(DataRow row)
    {
        SuppressDirtyTracking(() =>
        {
            _editingWhId = Str(row, "wh_id");
            cboDetailAccId.EditValue = Str(row, "acc_id");
            txtDetailWhNm.Text = Str(row, "wh_nm");
            txtDetailWhType.Text = Str(row, "wh_type");
            txtDetailDeptId.Text = Str(row, "dept_id");
            popDetailDeptNm.Text = Str(row, "dept_nm");
            txtDetailEmpId.Text = Str(row, "emp_id");
            popDetailEmpNm.Text = Str(row, "emp_nm");
        });

        await LoadLocsAsync(_editingWhId!);
    }

    private async Task LoadLocsAsync(string whId)
    {
        _locs = await QueryAsync("USP_BA_WH_Q", new
        {
            p_work_type = "Q1",
            p_wh_id = whId
        });
        TrackDirty(_locs);
        grd2.DataSource = _locs;
    }

    public override Task NewRowClick()
    {
        // 창고가 아직 저장 안 된 신규 입력 상태에서 위치부터 추가하면 wh_id 없는 위치가 되어
        // 정합성이 깨진다 - 창고를 먼저 저장해야 위치를 추가할 수 있게 막는다.
        if (_editingWhId == null)
        {
            AppMessageBox.Show("창고를 먼저 등록한 뒤에 위치를 추가할 수 있습니다.", "안내");
            return Task.CompletedTask;
        }

        gvw2.AddNewRow();
        return Task.CompletedTask;
    }

    public override Task DeleteRowClick()
    {
        gvw2.CloseEditor();
        gvw2.UpdateCurrentRow();

        var handle = gvw2.FocusedRowHandle;
        if (handle >= 0) gvw2.DeleteRow(handle);
        return Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        if (string.IsNullOrWhiteSpace(txtDetailWhNm.Text))
        {
            AppMessageBox.Show("창고명은 필수입니다.", "확인");
            return;
        }

        var wasNew = _editingWhId == null;

        var headerResult = await SaveAsync("USP_BA_WH_S", new
        {
            p_work_type = wasNew ? "N" : "U",
            p_wh_id = wasNew ? null : _editingWhId,
            p_wh_nm = txtDetailWhNm.Text,
            p_acc_id = cboDetailAccId.EditValue?.ToString(),
            p_wh_type = txtDetailWhType.Text,
            p_dept_id = string.IsNullOrEmpty(txtDetailDeptId.Text) ? null : txtDetailDeptId.Text,
            p_emp_id = string.IsNullOrEmpty(txtDetailEmpId.Text) ? null : txtDetailEmpId.Text
        });

        if (!headerResult.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(headerResult), "저장 실패");
            return;
        }

        var whId = wasNew ? headerResult.GeneratedCode! : _editingWhId!;

        gvw2.CloseEditor();
        gvw2.UpdateCurrentRow();

        var locSaveError = await SaveLocRowsAsync(whId);
        if (locSaveError != null)
        {
            AppMessageBox.Show(locSaveError, "저장 실패");
            return;
        }

        _locs.AcceptChanges();
        _editingWhId = whId;
        gvw2.ClearDirtyMarks();
        await QueryCore(preserveSelection: true); // 방금 저장한 창고 유지
        Toast.Show(wasNew ? "등록되었습니다." : "수정되었습니다.");
    }

    /// <summary>변경된 위치 행마다 USP_BA_WH_LOC_S를 순서대로 한 번씩 호출한다(frmMinorCode의
    /// SaveMinorRowsAsync와 같은 방식) - loc_id는 IDENTITY라 코드 변경(삭제+재등록) 분기가
    /// 필요 없다.</summary>
    private async Task<string?> SaveLocRowsAsync(string whId)
    {
        foreach (DataRow row in _locs.Rows)
        {
            string? error = row.RowState switch
            {
                DataRowState.Added => await SaveLocAsync("N", whId, row, DataRowVersion.Current),
                DataRowState.Modified => await SaveLocAsync("U", whId, row, DataRowVersion.Current),
                DataRowState.Deleted => await SaveLocAsync("D", whId, row, DataRowVersion.Original),
                _ => null
            };
            if (error != null) return error;
        }
        return null;
    }

    private async Task<string?> SaveLocAsync(string workType, string whId, DataRow row, DataRowVersion version)
    {
        var locNm = ProcData.Str(row, "loc_nm", version);
        if (workType == "N" && string.IsNullOrEmpty(locNm)) return null; // 이름 없이 행만 추가된 빈 행은 건너뜀

        var locId = ProcData.Str(row, "loc_id", version);
        var result = await SaveAsync("USP_BA_WH_LOC_S", new
        {
            p_work_type = workType,
            p_wh_id = whId,
            p_loc_id = workType == "N" ? null : locId,
            p_loc_nm = locNm,
            p_loc_type = ProcData.Str(row, "loc_type", version)
        });

        return result.Success ? null : $"[{locNm}] {FormatSaveFailMessage(result)}";
    }

    public override async Task DeleteClick()
    {
        if (_editingWhId == null)
        {
            AppMessageBox.Show("삭제할 창고를 먼저 선택해주세요.", "안내");
            return;
        }

        var confirm = AppMessageBox.Show(
            $"선택하신 창고를 삭제 하시겠습니까?\n\n{txtDetailWhNm.Text}",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await SaveAsync("USP_BA_WH_S", new
        {
            p_work_type = "D",
            p_wh_id = _editingWhId
        });

        if (!result.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(result), "삭제 실패");
            return;
        }

        _editingWhId = null;
        await QueryClick();
        Toast.Show("삭제되었습니다.");
    }

    private async void btnAddRow2_Click(object sender, EventArgs e) => await NewRowClick();
    private async void btnDeletRow2_Click(object sender, EventArgs e) => await DeleteRowClick();

    private static string Str(DataRow row, string columnName) =>
        row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value
            ? Convert.ToString(row[columnName]) ?? string.Empty
            : string.Empty;

    private static string FormatSaveFailMessage(ApiResult? result)
    {
        var message = result?.Message ?? "저장에 실패했습니다.";
        return result is { ErrorCode: not 0 } ? $"{message} (오류코드: {result.ErrorCode})" : message;
    }
}
