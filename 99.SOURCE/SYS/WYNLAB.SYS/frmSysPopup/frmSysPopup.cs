using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.SYS;

/// <summary>
/// 팝업관리 화면 - sysPopUpM(팝업 정의)/sysPopUpD(컬럼 설정)을 관리한다. 여기서 등록한 내용을
/// popPopUp이 런타임에 그대로 읽어서 팝업을 그린다(WYNLAB.BaseForm.popPopUp 참고) -
/// 이 화면은 화면 클래스를 새로 만드는 게 아니라 "설정값"만 만든다.
///
/// "컬럼생성" 버튼은 프로시저를 실행하지 않고 (1) 결과셋 컬럼 구조, (2) 입력 파라미터(조회조건)
/// 구조를 한 번에 읽어서(api/popup-admin/describe-proc, 서버는 sys.parameters +
/// sys.dm_exec_describe_first_result_set 사용) grd2/grd3에 없는 것만 기본값으로 채워 넣는다 -
/// 이미 설정해둔 행(캡션/타입 등 손으로 고친 것)은 그대로 둔다. 프로시저마다 파라미터명/개수가
/// 전부 다르다는 전제(사장님 결정 - "@p_keyword 하나로 통일" 가정을 버림)라서, sysPopUpM에는
/// 검색창 라벨을 따로 안 두고 sysPopUpS(조회조건 하나=행 하나)로 관리한다.
///
/// SYS 모듈 전체의 접근제어(메뉴권한만으로는 부족하다는 사장님 우려)는 아직 확정 전이다
/// (project_wynlab_popup_lookup_framework 메모리 참고) - 지금은 다른 화면과 동일하게
/// TSMMENUAUTH + PROC_PREFIX로만 보호된 상태.
/// </summary>
public partial class frmSysPopup : BaseForm
{
    private DataTable _popups = new();
    private DataTable _columns = new();
    private DataTable _searchFields = new();
    private string? _editingPopupKey; // null이면 신규모드

    public frmSysPopup()
    {
        InitializeComponent();

        Text = "팝업관리";

        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        // grd1(팝업 목록)은 조회전용, grd2(컬럼)/grd3(조회조건)은 셀 직접 입력이 필요해서 Edit -
        // frmSysLookup과 같은 이유로 명시(2026-09-02, [[project_wynlab_grid_role_mechanism]]).
        gvw1.Role = GridRoleWyn.Query;
        gvw2.Role = GridRoleWyn.Edit;
        gvw3.Role = GridRoleWyn.Edit;

        // 개발자용 마우스오버 툴팁(BindingField) - 실제 적용은 BaseForm.ApplyBindingFieldTooltips가
        // 공통으로 처리한다(Session.IsDeveloper일 때만). 그리드 컬럼은 FieldName이 이미 DB
        // 컬럼명이라 자동 적용되지만, panData 개별 컨트롤은 Tag에 미리 넣어둬야 잡힌다
        // (2026-09-12 감사 - 이 화면엔 원래 빠져있었음).
        txtPopupKey.Tag = new BindingFieldTag("popup_key");
        txtPopupNm.Tag = new BindingFieldTag("popup_nm");
        txtProcNm.Tag = new BindingFieldTag("proc_nm");
        chkHierarchical.Tag = new BindingFieldTag("hierarchical_yn");
        txtKeyField.Tag = new BindingFieldTag("key_field");
        txtParentField.Tag = new BindingFieldTag("parent_field");
        txtDisplayField.Tag = new BindingFieldTag("display_field");
        chkUseYn.Tag = new BindingFieldTag("use_yn");
        txtPopupWidth.Tag = new BindingFieldTag("popup_width");
        txtPopupHeight.Tag = new BindingFieldTag("popup_height");
        txtRemark.Tag = new BindingFieldTag("remark");

        TrackDirty(panData);

        EnterNewMode();
    }

    // frmSysLookup과 같은 이유로 분리 - 사용자 조회(preserveSelection: false, 항상 0번 행)와
    // 저장/삭제 뒤 내부 재조회(preserveSelection: true, 방금 행 유지)는 다른 동작이어야 한다
    // (2026-09-02, [[feedback_query_refocus_after_save]]).
    public override async Task QueryClick() => await QueryCore(preserveSelection: false);

    private async Task QueryCore(bool preserveSelection)
    {
        var keyword = txtSearchQ.Text.Trim();

        _popups = await QueryAsync("SSP_SYS_POPUP_Q", new
        {
            p_work_type = "Q",
            p_popup_key = keyword,
            p_popup_nm = keyword
        });

        var editingPopupKey = preserveSelection ? _editingPopupKey : null;

        gvw1.FocusedRowObjectChanged -= Gvw1_FocusedRowObjectChanged;
        SuppressMasterRowSwitchConfirm(gvw1, () =>
        {
            grd1.DataSource = _popups;

            if (editingPopupKey != null)
            {
                var handle = FindRowHandle(editingPopupKey);
                if (handle != null) gvw1.FocusedRowHandle = handle.Value;
            }
        });
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        var row = editingPopupKey == null ? null : FindRow(editingPopupKey);
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
        if (_editingPopupKey == null)
        {
            AppMessageBox.Show("삭제할 팝업을 먼저 선택해주세요.", "안내");
            return;
        }

        var confirm = AppMessageBox.Show(
            $"선택하신 팝업을 삭제 하시겠습니까?\n\n[{_editingPopupKey}] {txtPopupNm.Text}\n이 팝업을 쓰는 화면이 있으면 그 화면의 \"...\" 버튼이 더 이상 동작하지 않습니다.",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await SaveAsync("SSP_SYS_POPUP_S", new
        {
            p_work_type = "D",
            p_popup_key = _editingPopupKey
        });

        if (!result.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(result), "삭제 실패");
            return;
        }

        _editingPopupKey = null;
        await QueryClick();
        Toast.Show("삭제되었습니다.");
    }

    public override Task NewRowClick()
    {
        if (_editingPopupKey == null)
        {
            AppMessageBox.Show("팝업을 먼저 등록한 뒤에 컬럼을 추가할 수 있습니다.", "안내");
            return Task.CompletedTask;
        }

        gvw2.AddNewRow();
        return Task.CompletedTask;
    }

    public override Task DeleteRowClick()
    {
        var handle = gvw2.FocusedRowHandle;
        if (handle >= 0) gvw2.DeleteRow(handle);
        return Task.CompletedTask;
    }

    /// <summary>grd3(조회조건)용 행추가/삭제 - BaseForm.NewRowClick/DeleteRowClick은 이미 grd2가
    /// 쓰고 있어서(override는 화면당 한 번뿐) grd3은 별도의 평범한 메서드로 둔다.</summary>
    private Task NewSearchFieldRowClick()
    {
        if (_editingPopupKey == null)
        {
            AppMessageBox.Show("팝업을 먼저 등록한 뒤에 조회조건을 추가할 수 있습니다.", "안내");
            return Task.CompletedTask;
        }

        gvw3.AddNewRow();
        return Task.CompletedTask;
    }

    private Task DeleteSearchFieldRowClick()
    {
        var handle = gvw3.FocusedRowHandle;
        if (handle >= 0) gvw3.DeleteRow(handle);
        return Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        if (string.IsNullOrWhiteSpace(txtPopupKey.Text) || string.IsNullOrWhiteSpace(txtPopupNm.Text) || string.IsNullOrWhiteSpace(txtProcNm.Text))
        {
            AppMessageBox.Show("팝업키/팝업명/프로시저명은 필수입니다.", "확인");
            return;
        }

        var wasNew = _editingPopupKey == null;

        var result = await SaveAsync("SSP_SYS_POPUP_S", new
        {
            p_work_type = wasNew ? "N" : "U",
            p_popup_key = txtPopupKey.Text.ToUpper(),
            p_proc_nm = txtProcNm.Text,
            p_popup_nm = txtPopupNm.Text,
            p_hierarchical_yn = chkHierarchical.Checked ? "Y" : "N",
            p_key_field = txtKeyField.Text,
            p_parent_field = string.IsNullOrWhiteSpace(txtParentField.Text) ? null : txtParentField.Text,
            p_display_field = txtDisplayField.Text,
            p_popup_width = string.IsNullOrWhiteSpace(txtPopupWidth.Text) ? "700" : txtPopupWidth.Text,
            p_popup_height = string.IsNullOrWhiteSpace(txtPopupHeight.Text) ? "500" : txtPopupHeight.Text,
            p_use_yn = chkUseYn.Checked ? "Y" : "N",
            p_remark = txtRemark.Text
        });

        if (!result.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(result), "저장 실패");
            return;
        }

        var savedPopupKey = wasNew ? (result.GeneratedCode ?? txtPopupKey.Text.ToUpper()) : _editingPopupKey!;

        // 그리드에서 편집 중이던 셀 값을 먼저 확정해야 아래 저장에 마지막 수정이 포함된다.
        gvw2.CloseEditor();
        gvw2.UpdateCurrentRow();
        gvw3.CloseEditor();
        gvw3.UpdateCurrentRow();

        var columnSaveError = await SaveColumnRowsAsync(savedPopupKey);
        if (columnSaveError != null)
        {
            AppMessageBox.Show(columnSaveError, "저장 실패");
            return;
        }

        var searchFieldSaveError = await SaveSearchFieldRowsAsync(savedPopupKey);
        if (searchFieldSaveError != null)
        {
            AppMessageBox.Show(searchFieldSaveError, "저장 실패");
            return;
        }

        _columns.AcceptChanges();
        _searchFields.AcceptChanges();
        _editingPopupKey = savedPopupKey;
        await QueryCore(preserveSelection: true); // 방금 저장한 행 유지 - QueryClick(사용자 조회)과 다른 경로
        Toast.Show(wasNew ? "등록되었습니다." : "수정되었습니다.");
    }

    /// <summary>grd2(컬럼 설정)에서 바뀐 행마다 SSP_SYS_POPUP_S_1을 한 번씩 호출한다
    /// (USP_SM_MINORCODE_S_1/frmItem.SaveUnitRowsAsync와 같은 구조). column_nm이 키라 그 값
    /// 자체를 고친 행은 "원래 키 삭제 + 새 키 등록"으로 표현한다(이 프로시저의 'U' 분기도
    /// 키는 WHERE에서만 쓰고 안 바꾼다).</summary>
    private async Task<string?> SaveColumnRowsAsync(string popupKey)
    {
        foreach (DataRow row in _columns.Rows)
        {
            string? error = row.RowState switch
            {
                DataRowState.Added => await SaveNewColumnAsync(popupKey, row),
                DataRowState.Modified => await SaveModifiedColumnAsync(popupKey, row),
                DataRowState.Deleted => await SaveDeletedColumnAsync(popupKey, row),
                _ => null
            };
            if (error != null) return error;
        }
        return null;
    }

    private async Task<string?> SaveNewColumnAsync(string popupKey, DataRow row)
    {
        var columnNm = ProcData.Str(row, "column_nm", DataRowVersion.Current);
        if (string.IsNullOrEmpty(columnNm)) return null; // 컬럼명 없이 행만 추가된 빈 행은 건너뜀

        var result = await SaveAsync("SSP_SYS_POPUP_S_1", ColumnParams("N", popupKey, row, DataRowVersion.Current));
        return result.Success ? null : $"[{columnNm}] {FormatSaveFailMessage(result)}";
    }

    private async Task<string?> SaveModifiedColumnAsync(string popupKey, DataRow row)
    {
        var origColumnNm = ProcData.Str(row, "column_nm", DataRowVersion.Original);
        var columnNm = ProcData.Str(row, "column_nm", DataRowVersion.Current);

        if (columnNm != origColumnNm)
        {
            var delResult = await SaveAsync("SSP_SYS_POPUP_S_1",
                new { p_work_type = "D", p_popup_key = popupKey, p_column_nm = origColumnNm });
            if (!delResult.Success) return $"[{origColumnNm}] {FormatSaveFailMessage(delResult)}";

            var addResult = await SaveAsync("SSP_SYS_POPUP_S_1", ColumnParams("N", popupKey, row, DataRowVersion.Current));
            return addResult.Success ? null : $"[{columnNm}] {FormatSaveFailMessage(addResult)}";
        }

        var result = await SaveAsync("SSP_SYS_POPUP_S_1", ColumnParams("U", popupKey, row, DataRowVersion.Current));
        return result.Success ? null : $"[{columnNm}] {FormatSaveFailMessage(result)}";
    }

    private async Task<string?> SaveDeletedColumnAsync(string popupKey, DataRow row)
    {
        var columnNm = ProcData.Str(row, "column_nm", DataRowVersion.Original);
        var result = await SaveAsync("SSP_SYS_POPUP_S_1",
            new { p_work_type = "D", p_popup_key = popupKey, p_column_nm = columnNm });
        return result.Success ? null : $"[{columnNm}] {FormatSaveFailMessage(result)}";
    }

    private static Dictionary<string, string?> ColumnParams(string workType, string popupKey, DataRow row, DataRowVersion version) => new()
    {
        ["p_work_type"] = workType,
        ["p_popup_key"] = popupKey,
        ["p_column_nm"] = ProcData.Str(row, "column_nm", version),
        ["p_caption"] = ProcData.Str(row, "caption", version),
        ["p_control_type"] = ProcData.Str(row, "control_type", version) is { Length: > 0 } ct ? ct : "TEXT",
        ["p_lookup_proc_nm"] = ProcData.Str(row, "lookup_proc_nm", version),
        ["p_sort"] = ProcData.Str(row, "sort", version),
        ["p_width"] = ProcData.Str(row, "width", version),
        ["p_visible_yn"] = ProcData.Str(row, "visible_yn", version) is { Length: > 0 } v ? v : "Y"
    };

    /// <summary>grd3(조회조건)에서 바뀐 행마다 SSP_SYS_POPUP_S_2를 한 번씩 호출한다 -
    /// SaveColumnRowsAsync와 완전히 같은 구조(param_nm이 키라 키 자체를 고친 행은 삭제+재등록).</summary>
    private async Task<string?> SaveSearchFieldRowsAsync(string popupKey)
    {
        foreach (DataRow row in _searchFields.Rows)
        {
            string? error = row.RowState switch
            {
                DataRowState.Added => await SaveNewSearchFieldAsync(popupKey, row),
                DataRowState.Modified => await SaveModifiedSearchFieldAsync(popupKey, row),
                DataRowState.Deleted => await SaveDeletedSearchFieldAsync(popupKey, row),
                _ => null
            };
            if (error != null) return error;
        }
        return null;
    }

    private async Task<string?> SaveNewSearchFieldAsync(string popupKey, DataRow row)
    {
        var paramNm = ProcData.Str(row, "param_nm", DataRowVersion.Current);
        if (string.IsNullOrEmpty(paramNm)) return null; // 파라미터명 없이 행만 추가된 빈 행은 건너뜀

        var result = await SaveAsync("SSP_SYS_POPUP_S_2", SearchFieldParams("N", popupKey, row, DataRowVersion.Current));
        return result.Success ? null : $"[{paramNm}] {FormatSaveFailMessage(result)}";
    }

    private async Task<string?> SaveModifiedSearchFieldAsync(string popupKey, DataRow row)
    {
        var origParamNm = ProcData.Str(row, "param_nm", DataRowVersion.Original);
        var paramNm = ProcData.Str(row, "param_nm", DataRowVersion.Current);

        if (paramNm != origParamNm)
        {
            var delResult = await SaveAsync("SSP_SYS_POPUP_S_2",
                new { p_work_type = "D", p_popup_key = popupKey, p_param_nm = origParamNm });
            if (!delResult.Success) return $"[{origParamNm}] {FormatSaveFailMessage(delResult)}";

            var addResult = await SaveAsync("SSP_SYS_POPUP_S_2", SearchFieldParams("N", popupKey, row, DataRowVersion.Current));
            return addResult.Success ? null : $"[{paramNm}] {FormatSaveFailMessage(addResult)}";
        }

        var result = await SaveAsync("SSP_SYS_POPUP_S_2", SearchFieldParams("U", popupKey, row, DataRowVersion.Current));
        return result.Success ? null : $"[{paramNm}] {FormatSaveFailMessage(result)}";
    }

    private async Task<string?> SaveDeletedSearchFieldAsync(string popupKey, DataRow row)
    {
        var paramNm = ProcData.Str(row, "param_nm", DataRowVersion.Original);
        var result = await SaveAsync("SSP_SYS_POPUP_S_2",
            new { p_work_type = "D", p_popup_key = popupKey, p_param_nm = paramNm });
        return result.Success ? null : $"[{paramNm}] {FormatSaveFailMessage(result)}";
    }

    private static Dictionary<string, string?> SearchFieldParams(string workType, string popupKey, DataRow row, DataRowVersion version) => new()
    {
        ["p_work_type"] = workType,
        ["p_popup_key"] = popupKey,
        ["p_param_nm"] = ProcData.Str(row, "param_nm", version),
        ["p_caption"] = ProcData.Str(row, "caption", version),
        ["p_control_type"] = ProcData.Str(row, "control_type", version) is { Length: > 0 } ct ? ct : "TEXT",
        ["p_sort"] = ProcData.Str(row, "sort", version),
        ["p_width"] = ProcData.Str(row, "width", version)
    };

    /// <summary>프로시저 구조를 읽어와서(실행은 안 함), grd2/grd3에 아직 없는 컬럼/조회조건만
    /// 기본값으로 채워 넣는다. 이미 있는 행(사람이 캡션/타입 등을 손으로 고쳐둔 것)은 그대로
    /// 둔다 - 그래서 프로시저에 컬럼/파라미터를 추가하고 이 버튼을 다시 눌러도 기존 설정이
    /// 안 지워진다.</summary>
    private async void btnGenerateColumns_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtProcNm.Text))
        {
            AppMessageBox.Show("프로시저명을 먼저 입력해주세요.", "안내");
            return;
        }

        DescribeProcResultDto? discovered;
        try
        {
            discovered = await ApiClient.PostAsync<DescribeColumnsRequest, DescribeProcResultDto>(
                "api/popup-admin/describe-proc", new DescribeColumnsRequest { ProcName = txtProcNm.Text.Trim() });
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"프로시저 구조를 읽어오지 못했습니다.\n{ex.Message}", "오류");
            return;
        }

        if (discovered == null || (discovered.Columns.Count == 0 && discovered.Params.Count == 0))
        {
            AppMessageBox.Show("결과셋/파라미터를 찾지 못했습니다. 프로시저명을 확인해주세요.", "안내");
            return;
        }

        // grd2/grd3에 바인딩된 DataTable을 직접 건드리는 구간 - 예외가 SafeExecute 없이 새면
        // 그리드 바인딩 상태가 깨진 채로 남을 수 있다(frmSysLookup.btnGenerateParams_Click에서
        // 실제로 겪은 사고, 2026-09-02 - 같은 패턴이라 여기도 감싸둠).
        try
        {
            var addedColumns = AddMissingColumns(discovered.Columns);
            var addedSearchFields = AddMissingSearchFields(discovered.Params);

            grd2.RefreshDataSource();
            grd3.RefreshDataSource();
            Toast.Show(addedColumns > 0 || addedSearchFields > 0
                ? $"컬럼 {addedColumns}개, 조회조건 {addedSearchFields}개를 추가했습니다."
                : "새로 추가할 컬럼/조회조건이 없습니다.");
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"[컬럼생성] 처리 중 오류가 발생했습니다.\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private int AddMissingColumns(List<ProcColumnInfoDto> discovered)
    {
        var existing = _columns.Rows.Cast<DataRow>()
            .Select(r => Convert.ToString(r["column_nm"]))
            .Where(v => v != null)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var added = 0;
        var nextSort = _columns.Rows.Count == 0 ? 1 : _columns.Rows.Cast<DataRow>().Max(r => Convert.ToInt32(r["sort"])) + 1;

        foreach (var col in discovered)
        {
            if (existing.Contains(col.ColumnNm)) continue;

            var row = _columns.NewRow();
            row["column_nm"] = col.ColumnNm;
            row["caption"] = col.ColumnNm;
            row["control_type"] = col.SuggestedControlType;
            row["lookup_proc_nm"] = DBNull.Value;
            row["sort"] = nextSort++;
            row["width"] = 100;
            row["visible_yn"] = "Y";
            _columns.Rows.Add(row);
            added++;
        }

        return added;
    }

    private int AddMissingSearchFields(List<ProcParamInfoDto> discovered)
    {
        var existing = _searchFields.Rows.Cast<DataRow>()
            .Select(r => Convert.ToString(r["param_nm"]))
            .Where(v => v != null)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var added = 0;
        var nextSort = _searchFields.Rows.Count == 0 ? 1 : _searchFields.Rows.Cast<DataRow>().Max(r => Convert.ToInt32(r["sort"])) + 1;

        foreach (var p in discovered)
        {
            if (existing.Contains(p.ParamNm)) continue;

            var row = _searchFields.NewRow();
            row["param_nm"] = p.ParamNm;
            row["caption"] = p.ParamNm;
            row["control_type"] = p.SuggestedControlType;
            row["sort"] = nextSort++;
            row["width"] = 120;
            _searchFields.Rows.Add(row);
            added++;
        }

        return added;
    }

    /// <summary>다른 행을 고를 때 panData/grd2(컬럼)/grd3(검색조건)에 저장 안 된 변경이 있으면
    /// 먼저 확인한다(BaseForm.ConfirmMasterRowSwitch 참고, 2026-09-06 - 모든 화면 공통 적용).</summary>
    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e) =>
        ConfirmMasterRowSwitch(gvw1, e, row => EnterEditMode(row.Row));

    private DataRow? FindRow(string popupKey) =>
        _popups.Rows.Cast<DataRow>()
            .FirstOrDefault(r => string.Equals(Convert.ToString(r["popup_key"]), popupKey, StringComparison.OrdinalIgnoreCase));

    /// <summary>popup_key로 grd1의 행 핸들을 찾는다. 없으면 null. 컬럼 셀 값으로 찾는 방식은
    /// 그 키 컬럼이 화면에 안 보이는 숨김 컬럼일 때 안 먹힌다(2026-09-11 실제 발견, frmEMP에서
    /// 재현) - FindRow로 찾은 DataRow의 DataTable상 인덱스를 GridView.GetRowHandle로 표시 행
    /// 핸들로 변환하면 키 컬럼이 보이든 안 보이든 항상 동작한다.</summary>
    private int? FindRowHandle(string popupKey)
    {
        var row = FindRow(popupKey);
        if (row == null) return null;

        var rowIndex = _popups.Rows.IndexOf(row);
        var handle = gvw1.GetRowHandle(rowIndex);
        return handle >= 0 ? handle : null;
    }

    /// <summary>panData/그리드를 채우는 부분은 SuppressDirtyTracking으로 감싼다 - 안 그러면
    /// 코드가 값을 채우는 것뿐인데 TrackDirty가 "사용자가 고쳤다"로 오인해서, 조회/행 선택
    /// 직후부터 화면을 닫을 때 저장 확인이 뜨는 오작동이 생긴다.</summary>
    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingPopupKey = null;
            txtPopupKey.Text = string.Empty;
            txtPopupKey.ReadOnly = false;
            txtPopupNm.Text = string.Empty;
            txtProcNm.Text = string.Empty;
            chkHierarchical.Checked = false;
            txtKeyField.Text = string.Empty;
            txtParentField.Text = string.Empty;
            txtDisplayField.Text = string.Empty;
            chkUseYn.Checked = true;
            txtPopupWidth.Text = "700";
            txtPopupHeight.Text = "500";
            txtRemark.Text = string.Empty;

            _columns = _columns.Clone();
            TrackDirty(_columns);
            grd2.DataSource = _columns;

            _searchFields = _searchFields.Clone();
            TrackDirty(_searchFields);
            grd3.DataSource = _searchFields;
        });
        txtPopupKey.Focus();
    }

    private void EnterEditMode(DataRow popup)
    {
        var popupKey = Str(popup, "popup_key");
        var isSamePopup = _editingPopupKey == popupKey;

        SuppressDirtyTracking(() =>
        {
            _editingPopupKey = popupKey;
            txtPopupKey.Text = popupKey;
            txtPopupKey.ReadOnly = true; // 팝업키는 키라 수정 불가
            txtPopupNm.Text = Str(popup, "popup_nm");
            txtProcNm.Text = Str(popup, "proc_nm");
            chkHierarchical.Checked = Str(popup, "hierarchical_yn") == "Y";
            txtKeyField.Text = Str(popup, "key_field");
            txtParentField.Text = Str(popup, "parent_field");
            txtDisplayField.Text = Str(popup, "display_field");
            chkUseYn.Checked = Str(popup, "use_yn") != "N";
            txtPopupWidth.Text = Str(popup, "popup_width");
            txtPopupHeight.Text = Str(popup, "popup_height");
            txtRemark.Text = Str(popup, "remark");
        });

        if (!isSamePopup)
        {
            _ = LoadColumnsAsync(popupKey);
            _ = LoadSearchFieldsAsync(popupKey);
        }
    }

    private async Task LoadColumnsAsync(string popupKey)
    {
        _columns = await QueryAsync("SSP_SYS_POPUP_Q", new
        {
            p_work_type = "Q1",
            p_popup_key = popupKey
        });
        TrackDirty(_columns);

        grd2.DataSource = _columns;
    }

    private async Task LoadSearchFieldsAsync(string popupKey)
    {
        _searchFields = await QueryAsync("SSP_SYS_POPUP_Q", new
        {
            p_work_type = "Q2",
            p_popup_key = popupKey
        });
        TrackDirty(_searchFields);

        grd3.DataSource = _searchFields;
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

    private class DescribeColumnsRequest
    {
        public string ProcName { get; set; } = string.Empty;
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

    private async void btnAddRow3_Click(object sender, EventArgs e)
    {
        await NewSearchFieldRowClick();
    }

    private async void btnDeletRow3_Click(object sender, EventArgs e)
    {
        await DeleteSearchFieldRowClick();
    }
}
