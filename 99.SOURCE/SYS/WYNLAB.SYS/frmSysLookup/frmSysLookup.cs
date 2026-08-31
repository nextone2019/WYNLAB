using System.Data;
using WYNLAB.Base;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.SYS;

/// <summary>
/// LookUp(콤보)관리 화면 - sysLookupM(LookUp 정의)/sysLookupP(파라미터)를 관리한다. 여기서 등록한
/// 내용을 화면들이 LookUpEditWyn.LookupKey로 그대로 참조해서 쓴다(ComboLookupProvider.Fetch가
/// api/combo-lookups/{key}/items를 호출 - WYNLAB.BaseForm.ControlDataSources 참고) - 이 화면은
/// 화면 클래스를 새로 만드는 게 아니라 "설정값"만 만든다. 팝업관리(frmSysPopup)와 발상은
/// 같지만, LookUp은 결과셋 전체가 아니라 값(코드)+표시값(명칭) 두 필드만 뽑아 쓰기 때문에
/// 컬럼 그리드가 없다 - 하위 그리드 하나(grd2)는 파라미터 목록 전용이다.
///
/// "파라미터생성" 버튼은 프로시저를 실행하지 않고 입력 파라미터 구조만 읽어서
/// (api/lookup-admin/describe-proc, PopupAdminController와 같은 introspection 로직 재사용)
/// grd2에 없는 파라미터만 기본값으로 채워 넣는다 - 이미 설정해둔 행(설명을 손으로 고친 것)은
/// 그대로 둔다. 프로시저마다 파라미터명/개수가 전부 다를 수 있다(사장님 결정).
///
/// SYS 모듈 전체의 접근제어(메뉴권한만으로는 부족하다는 사장님 우려)는 아직 확정 전이다
/// (project_wynlab_popup_lookup_framework 메모리 참고) - 지금은 다른 화면과 동일하게
/// TSMMENUAUTH + PROC_PREFIX로만 보호된 상태.
/// </summary>
public partial class frmSysLookup : BaseForm
{
    private DataTable _lookups = new();
    private DataTable _params = new();
    private string? _editingLookupKey; // null이면 신규모드

    public frmSysLookup()
    {
        InitializeComponent();

        Text = "LookUp관리";
        MenuCd = "SYS_LOOKUP";

        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        TrackDirty(panData);

        EnterNewMode();
    }

    public override async Task QueryClick()
    {
        var keyword = txtSearchQ.Text.Trim();

        _lookups = await QueryAsync("USP_SYS_LOOKUP_Q", new
        {
            p_work_type = "Q",
            p_lookup_key = keyword,
            p_lookup_nm = keyword
        });

        var editingLookupKey = _editingLookupKey;

        gvw1.FocusedRowObjectChanged -= Gvw1_FocusedRowObjectChanged;
        try
        {
            grd1.DataSource = _lookups;

            if (editingLookupKey != null)
            {
                var handle = FindRowHandle(editingLookupKey);
                if (handle != null) gvw1.FocusedRowHandle = handle.Value;
            }
        }
        finally
        {
            gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;
        }

        var row = editingLookupKey == null ? null : FindRow(editingLookupKey);
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
        if (_editingLookupKey == null)
        {
            AppMessageBox.Show("삭제할 LookUp을 먼저 선택해주세요.", "안내");
            return;
        }

        var confirm = AppMessageBox.Show(
            $"선택하신 LookUp을 삭제 하시겠습니까?\n\n[{_editingLookupKey}] {txtLookupNm.Text}\n이 LookUp을 쓰는 화면이 있으면 그 화면의 콤보가 더 이상 동작하지 않습니다.",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await SaveAsync("USP_SYS_LOOKUP_S", new
        {
            p_work_type = "D",
            p_lookup_key = _editingLookupKey
        });

        if (!result.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(result), "삭제 실패");
            return;
        }

        await QueryClick();
        Toast.Show("삭제되었습니다.");
    }

    public override Task NewRowClick()
    {
        if (_editingLookupKey == null)
        {
            AppMessageBox.Show("LookUp을 먼저 등록한 뒤에 파라미터를 추가할 수 있습니다.", "안내");
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

    public override async Task SaveClick()
    {
        if (string.IsNullOrWhiteSpace(txtLookupKey.Text) || string.IsNullOrWhiteSpace(txtLookupNm.Text) ||
            string.IsNullOrWhiteSpace(txtProcNm.Text) || string.IsNullOrWhiteSpace(txtValueField.Text) ||
            string.IsNullOrWhiteSpace(txtDisplayField.Text))
        {
            AppMessageBox.Show("LookUp키/LookUp명/프로시저명/값필드/표시필드는 필수입니다.", "확인");
            return;
        }

        var wasNew = _editingLookupKey == null;

        var result = await SaveAsync("USP_SYS_LOOKUP_S", new
        {
            p_work_type = wasNew ? "N" : "U",
            p_lookup_key = txtLookupKey.Text.ToUpper(),
            p_proc_nm = txtProcNm.Text,
            p_lookup_nm = txtLookupNm.Text,
            p_value_field = txtValueField.Text,
            p_display_field = txtDisplayField.Text,
            p_use_yn = chkUseYn.Checked ? "Y" : "N",
            p_remark = txtRemark.Text
        });

        if (!result.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(result), "저장 실패");
            return;
        }

        var savedLookupKey = wasNew ? (result.GeneratedCode ?? txtLookupKey.Text.ToUpper()) : _editingLookupKey!;

        // 그리드에서 편집 중이던 셀 값을 먼저 확정해야 아래 저장에 마지막 수정이 포함된다.
        gvw2.CloseEditor();
        gvw2.UpdateCurrentRow();

        var paramSaveError = await SaveParamRowsAsync(savedLookupKey);
        if (paramSaveError != null)
        {
            AppMessageBox.Show(paramSaveError, "저장 실패");
            return;
        }

        _params.AcceptChanges();
        _editingLookupKey = savedLookupKey;
        await QueryClick();
        Toast.Show(wasNew ? "등록되었습니다." : "수정되었습니다.");
    }

    /// <summary>grd2(파라미터 목록)에서 바뀐 행마다 USP_SYS_LOOKUP_S_1을 한 번씩 호출한다
    /// (frmSysPopup.SaveColumnRowsAsync와 같은 구조). param_nm이 키라 그 값 자체를 고친 행은
    /// "원래 키 삭제 + 새 키 등록"으로 표현한다.</summary>
    private async Task<string?> SaveParamRowsAsync(string lookupKey)
    {
        foreach (DataRow row in _params.Rows)
        {
            string? error = row.RowState switch
            {
                DataRowState.Added => await SaveNewParamAsync(lookupKey, row),
                DataRowState.Modified => await SaveModifiedParamAsync(lookupKey, row),
                DataRowState.Deleted => await SaveDeletedParamAsync(lookupKey, row),
                _ => null
            };
            if (error != null) return error;
        }
        return null;
    }

    private async Task<string?> SaveNewParamAsync(string lookupKey, DataRow row)
    {
        var paramNm = ProcData.Str(row, "param_nm", DataRowVersion.Current);
        if (paramNm.Length == 0) return null; // 파라미터명 없이 행만 추가된 빈 행은 건너뜀

        var result = await SaveAsync("USP_SYS_LOOKUP_S_1", ParamParams("N", lookupKey, row, DataRowVersion.Current));
        return result.Success ? null : $"[{paramNm}] {FormatSaveFailMessage(result)}";
    }

    private async Task<string?> SaveModifiedParamAsync(string lookupKey, DataRow row)
    {
        var origParamNm = ProcData.Str(row, "param_nm", DataRowVersion.Original);
        var paramNm = ProcData.Str(row, "param_nm", DataRowVersion.Current);

        if (paramNm != origParamNm)
        {
            var delResult = await SaveAsync("USP_SYS_LOOKUP_S_1",
                new { p_work_type = "D", p_lookup_key = lookupKey, p_param_nm = origParamNm });
            if (!delResult.Success) return $"[{origParamNm}] {FormatSaveFailMessage(delResult)}";

            var addResult = await SaveAsync("USP_SYS_LOOKUP_S_1", ParamParams("N", lookupKey, row, DataRowVersion.Current));
            return addResult.Success ? null : $"[{paramNm}] {FormatSaveFailMessage(addResult)}";
        }

        var result = await SaveAsync("USP_SYS_LOOKUP_S_1", ParamParams("U", lookupKey, row, DataRowVersion.Current));
        return result.Success ? null : $"[{paramNm}] {FormatSaveFailMessage(result)}";
    }

    private async Task<string?> SaveDeletedParamAsync(string lookupKey, DataRow row)
    {
        var paramNm = ProcData.Str(row, "param_nm", DataRowVersion.Original);
        var result = await SaveAsync("USP_SYS_LOOKUP_S_1",
            new { p_work_type = "D", p_lookup_key = lookupKey, p_param_nm = paramNm });
        return result.Success ? null : $"[{paramNm}] {FormatSaveFailMessage(result)}";
    }

    private static Dictionary<string, string?> ParamParams(string workType, string lookupKey, DataRow row, DataRowVersion version) => new()
    {
        ["p_work_type"] = workType,
        ["p_lookup_key"] = lookupKey,
        ["p_param_nm"] = ProcData.Str(row, "param_nm", version),
        ["p_caption"] = ProcData.Str(row, "caption", version),
        ["p_sort"] = ProcData.Str(row, "sort", version)
    };

    /// <summary>프로시저의 입력 파라미터 구조를 읽어와서(실행은 안 함), grd2에 아직 없는
    /// 파라미터만 기본값으로 채워 넣는다. 이미 있는 행(사람이 설명을 손으로 고쳐둔 것)은
    /// 그대로 둔다. 결과셋 컬럼 목록도 같이 내려오는데(describe-proc이 팝업관리와 공유하는
    /// introspection이라 컬럼도 같이 옴), 여기서는 안 쓰고 값필드/표시필드로 뭘 쓸 수 있는지
    /// 참고하라고 토스트 메시지에만 보여준다 - LookUp은 컬럼 그리드가 없어서 자동으로 채울
    /// 자리가 없다(값필드/표시필드는 직접 입력).</summary>
    private async void btnGenerateParams_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtProcNm.Text))
        {
            AppMessageBox.Show("프로시저명을 먼저 입력해주세요.", "안내");
            return;
        }

        DescribeProcResultDto? discovered;
        try
        {
            discovered = await ApiClient.PostAsync<DescribeProcRequest, DescribeProcResultDto>(
                "api/lookup-admin/describe-proc", new DescribeProcRequest { ProcName = txtProcNm.Text.Trim() });
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

        var existing = _params.Rows.Cast<DataRow>()
            .Select(r => Convert.ToString(r["param_nm"]))
            .Where(v => v != null)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var added = 0;
        var nextSort = _params.Rows.Count == 0 ? 1 : _params.Rows.Cast<DataRow>().Max(r => Convert.ToInt32(r["sort"])) + 1;

        foreach (var p in discovered.Params)
        {
            if (existing.Contains(p.ParamNm)) continue;

            var row = _params.NewRow();
            row["param_nm"] = p.ParamNm;
            row["caption"] = string.Empty;
            row["sort"] = nextSort++;
            _params.Rows.Add(row);
            added++;
        }

        grd2.RefreshDataSource();

        var columnHint = discovered.Columns.Count > 0
            ? $" 사용 가능한 컬럼: {string.Join(", ", discovered.Columns.Select(c => c.ColumnNm))}"
            : string.Empty;
        Toast.Show(added > 0 ? $"파라미터 {added}개를 추가했습니다.{columnHint}" : $"새로 추가할 파라미터가 없습니다.{columnHint}");
    }

    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
    {
        if (e.Row is DataRowView view) EnterEditMode(view.Row);
    }

    private DataRow? FindRow(string lookupKey) =>
        _lookups.Rows.Cast<DataRow>()
            .FirstOrDefault(r => string.Equals(Convert.ToString(r["lookup_key"]), lookupKey, StringComparison.OrdinalIgnoreCase));

    private int? FindRowHandle(string lookupKey)
    {
        var column = gvw1.Columns["lookup_key"];
        if (column == null) return null;

        for (var handle = 0; handle < gvw1.RowCount; handle++)
        {
            if (string.Equals(Convert.ToString(gvw1.GetRowCellValue(handle, column)), lookupKey, StringComparison.OrdinalIgnoreCase))
                return handle;
        }
        return null;
    }

    /// <summary>panData/그리드를 채우는 부분은 SuppressDirtyTracking으로 감싼다 - frmSysPopup과
    /// 같은 이유(코드가 값을 채우는 것뿐인데 TrackDirty가 "사용자가 고쳤다"로 오인하는 것 방지).</summary>
    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingLookupKey = null;
            txtLookupKey.Text = string.Empty;
            txtLookupKey.ReadOnly = false;
            txtLookupNm.Text = string.Empty;
            txtProcNm.Text = string.Empty;
            txtValueField.Text = string.Empty;
            txtDisplayField.Text = string.Empty;
            chkUseYn.Checked = true;
            txtRemark.Text = string.Empty;

            _params = _params.Clone();
            TrackDirty(_params);
            grd2.DataSource = _params;
        });
        txtLookupKey.Focus();
    }

    private void EnterEditMode(DataRow lookup)
    {
        var lookupKey = Str(lookup, "lookup_key");
        var isSameLookup = _editingLookupKey == lookupKey;

        SuppressDirtyTracking(() =>
        {
            _editingLookupKey = lookupKey;
            txtLookupKey.Text = lookupKey;
            txtLookupKey.ReadOnly = true; // LookUp키는 키라 수정 불가
            txtLookupNm.Text = Str(lookup, "lookup_nm");
            txtProcNm.Text = Str(lookup, "proc_nm");
            txtValueField.Text = Str(lookup, "value_field");
            txtDisplayField.Text = Str(lookup, "display_field");
            chkUseYn.Checked = Str(lookup, "use_yn") != "N";
            txtRemark.Text = Str(lookup, "remark");
        });

        if (!isSameLookup) _ = LoadParamsAsync(lookupKey);
    }

    private async Task LoadParamsAsync(string lookupKey)
    {
        _params = await QueryAsync("USP_SYS_LOOKUP_Q", new
        {
            p_work_type = "Q1",
            p_lookup_key = lookupKey
        });
        TrackDirty(_params);

        grd2.DataSource = _params;
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

    private class DescribeProcRequest
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
}
