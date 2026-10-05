using System.Data;
using DevExpress.XtraGrid.Views.Grid;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
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
    private DataTable _columns = new(); // sysLookupC - LookUp 팝업에 값필드/표시필드 외 추가로 보여줄 컬럼 구성(2026-09-03)
    private string? _editingLookupKey; // null이면 신규모드

    public frmSysLookup()
    {
        InitializeComponent();

        Text = "LookUp관리";

        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        // grd1(LookUp 목록)은 조회전용, grd2(파라미터 목록)는 셀 직접 입력이 필요해서 Edit -
        // 지금까지 둘 다 한 번도 명시적으로 안 건드려서 EmbeddedNavigator의 추가/삭제/편집
        // 버튼이 DevExpress 순정 기본값(전부 보임+활성)으로 남아있었다(2026-09-02 발견,
        // [[project_wynlab_grid_role_mechanism]] 참고 - 이제 프레임워크가 EndInit에서 Query를
        // 기본 적용하므로 Edit이 필요한 grd2는 반드시 명시해야 한다).
        gvw1.Role = GridRoleWyn.Query;
        gvw2.Role = GridRoleWyn.Edit;

        // grd4(컬럼 목록) - grd2와 같은 이유로 Edit. NewRowClick/DeleteRowClick(BaseForm 표준
        // 오버라이드, 상단 툴바 Ctrl+I/Ctrl+Shift+D)은 이미 grd2가 쓰고 있어서, grd4는 그 대신
        // RowAdd/RowDelete 이벤트로 EmbeddedNavigator의 추가/삭제 버튼에 직접 연결한다
        // (frmMinorCode의 grd2 방식과 동일 - frmSysPopup의 grd3처럼 별도 버튼을 새로 안 만들어도
        // Role=Edit이면 이제 EmbeddedNavigator 버튼이 자동으로 보인다, 2026-09-03).
        gvw4.Role = GridRoleWyn.Edit;
        gvw4.RowAdd += async (s, e) => await NewColumnRowClick();
        gvw4.RowDelete += async (s, e) => await DeleteColumnRowClick();
        gvw4.InitNewRow += Gvw4_InitNewRow;

        // sysLookupM에 등록된 목록이 아니라 이 화면 자체가 아는 고정 2개짜리 목록이라 LookupKey
        // 대신 BindCodeList로 직접 채운다 - LookUpEditWyn을 쓰는 이유는 순전히 드롭다운 화살표를
        // 항상 보여주는 OnHandleCreated 처리(LookUpEditWyn 클래스 설명 참고) 때문이지, 서버조회가
        // 필요해서가 아니다. ComboBoxEdit이었을 때는 EndInit 이후 화살표가 안 보이는 문제가 있었다
        // (2026-09-02, 품목등록 단위 콤보와 같은 원인).
        cboSourceType.BindCodeList(
            new[]
            {
                new CodeLookupItem { Value = "P", Display = "프로시저" },
                new CodeLookupItem { Value = "Q", Display = "쿼리" }
            },
            nameof(CodeLookupItem.Value), nameof(CodeLookupItem.Display));
        cboSourceType.EditValueChanged += (s, e) => ToggleSourceTypeFields();

        // 개발자용 마우스오버 툴팁(BindingField) - 실제 적용은 BaseForm.ApplyBindingFieldTooltips가
        // 공통으로 처리한다(Session.IsDeveloper일 때만). 그리드 컬럼은 FieldName이 이미 DB
        // 컬럼명이라 자동 적용되지만, panData 개별 컨트롤은 Tag에 미리 넣어둬야 잡힌다
        // (2026-09-12 감사 - 이 화면엔 원래 빠져있었음).
        txtLookupKey.Tag = new BindingFieldTag("lookup_key");
        txtLookupNm.Tag = new BindingFieldTag("lookup_nm");
        cboSourceType.Tag = new BindingFieldTag("source_type");
        txtProcNm.Tag = new BindingFieldTag("proc_nm");
        txtQueryTxt.Tag = new BindingFieldTag("query_txt");
        txtValueField.Tag = new BindingFieldTag("value_field");
        txtDisplayField.Tag = new BindingFieldTag("display_field");
        chkUseYn.Tag = new BindingFieldTag("use_yn");
        txtRemark.Tag = new BindingFieldTag("remark");

        TrackDirty(panData);

        EnterNewMode();
    }

    private bool IsQuerySource => cboSourceType.EditValue?.ToString() == "Q";

    /// <summary>소스유형(프로시저/쿼리)에 따라 실제로 쓰이는 쪽만 입력 가능하게 한다 - 둘 다 항상
    /// 화면에 보이지만(숨기지 말아달라는 요청, 2026-09-02), 지금 소스유형과 무관한 쪽은
    /// Enabled=false로 흐리게 비활성화해서 "지금 이건 안 쓰인다"는 걸 보여준다. 값 자체는 그대로
    /// 남겨둔다 - 소스유형을 왔다 갔다 바꿔도 이전에 입력한 프로시저명/쿼리문이 안 지워져서,
    /// 다시 그쪽으로 돌아가면 그대로 남아있다. "파라미터생성" 버튼은 두 모드 모두에서 쓴다 -
    /// 쿼리 모드에서도 쿼리문을 분석해서 파라미터/컬럼을 읽어온다(btnGenerateParams_Click 참고).</summary>
    private void ToggleSourceTypeFields()
    {
        var isQuery = IsQuerySource;
        txtProcNm.Enabled = !isQuery;
        txtQueryTxt.Enabled = isQuery;
    }

    // 사용자가 조회 버튼을 누른 경우(preserveSelection: false)와 저장/삭제 뒤 내부적으로
    // 다시 조회하는 경우(preserveSelection: true)는 포커스 동작이 달라야 한다 - 전자는 항상
    // 0번 행 기준으로 새로 시작해야 하고, 후자는 방금 편집하던 행을 그대로 유지해야 한다.
    // 이 둘을 구분 안 하고 _editingLookupKey를 무조건 복원하면, grd2에서 다른 LookUp을
    // 골라둔 채로 조회를 눌러도 그 행이 계속 선택된 채로 남는다(2026-09-02 실제 발견 -
    // [[feedback_query_refocus_after_save]]에 frmEmp/frmAcc/TemplateForm 등에 이미 적용된
    // 것과 같은 패턴인데 frmSysLookup은 이 화면을 만들 때 빠뜨렸음).
    public override async Task QueryClick() => await QueryCore(preserveSelection: false);

    private async Task QueryCore(bool preserveSelection)
    {
        var keyword = txtSearchQ.Text.Trim();

        _lookups = await QueryAsync("SSP_SYS_LOOKUP_Q", new
        {
            p_work_type = "Q",
            p_lookup_key = keyword,
            p_lookup_nm = keyword
        });

        var editingLookupKey = preserveSelection ? _editingLookupKey : null;

        gvw1.FocusedRowObjectChanged -= Gvw1_FocusedRowObjectChanged;
        SuppressMasterRowSwitchConfirm(gvw1, () =>
        {
            grd1.DataSource = _lookups;

            if (editingLookupKey != null)
            {
                var handle = FindRowHandle(editingLookupKey);
                if (handle != null) gvw1.FocusedRowHandle = handle.Value;
            }
        });
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

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

    protected override bool ConfirmDeleteByDefault => false; // 삭제 확인창을 DeleteClick에서 직접 띄움(문서번호 등 상세 문구)

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

        var result = await SaveAsync("SSP_SYS_LOOKUP_S", new
        {
            p_work_type = "D",
            p_lookup_key = _editingLookupKey
        });

        if (!result.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(result), "삭제 실패");
            return;
        }

        _editingLookupKey = null;
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

    /// <summary>grd4(컬럼 목록)용 행추가 - BaseForm.NewRowClick은 이미 grd2가 쓰고 있어서
    /// (override는 화면당 한 번뿐) grd4는 RowAdd 이벤트로 직접 연결한다(생성자 참고).</summary>
    private Task NewColumnRowClick()
    {
        if (_editingLookupKey == null)
        {
            AppMessageBox.Show("LookUp을 먼저 등록한 뒤에 컬럼을 추가할 수 있습니다.", "안내");
            return Task.CompletedTask;
        }

        gvw4.AddNewRow();
        return Task.CompletedTask;
    }

    private Task DeleteColumnRowClick()
    {
        var handle = gvw4.FocusedRowHandle;
        if (handle >= 0) gvw4.DeleteRow(handle);
        return Task.CompletedTask;
    }

    /// <summary>수동으로 행을 추가할 때(+ 버튼) 폭 기본값을 100으로 채운다 - 안 채우면 DBNull이라
    /// 저장 시 프로시저 기본값(100)에 기대는 대신 여기서 명시적으로 채워서 그리드에도 바로
    /// 보이게 한다. sort는 굳이 안 채워도 신규 행은 항상 맨 뒤에 붙으므로 프로시저 기본값(0)이면
    /// 충분하다(파라미터생성이 채워주는 sort와 달리, 수동 추가는 순서를 신경 안 써도 되는 경우가
    /// 대부분).</summary>
    private void Gvw4_InitNewRow(object? sender, InitNewRowEventArgs e)
    {
        gvw4.SetRowCellValue(e.RowHandle, "width", 100);
    }

    public override async Task SaveClick()
    {
        if (string.IsNullOrWhiteSpace(txtLookupKey.Text) || string.IsNullOrWhiteSpace(txtLookupNm.Text) ||
            string.IsNullOrWhiteSpace(txtValueField.Text) || string.IsNullOrWhiteSpace(txtDisplayField.Text))
        {
            AppMessageBox.Show("LookUp키/LookUp명/값필드/표시필드는 필수입니다.", "확인");
            return;
        }

        var isQuery = IsQuerySource;
        if (isQuery && string.IsNullOrWhiteSpace(txtQueryTxt.Text))
        {
            AppMessageBox.Show("소스유형이 쿼리면 쿼리문은 필수입니다.", "확인");
            return;
        }
        if (!isQuery && string.IsNullOrWhiteSpace(txtProcNm.Text))
        {
            AppMessageBox.Show("소스유형이 프로시저면 프로시저명은 필수입니다.", "확인");
            return;
        }

        var wasNew = _editingLookupKey == null;

        var result = await SaveAsync("SSP_SYS_LOOKUP_S", new
        {
            p_work_type = wasNew ? "N" : "U",
            p_lookup_key = txtLookupKey.Text.ToUpper(),
            p_source_type = isQuery ? "Q" : "P",
            p_proc_nm = isQuery ? null : txtProcNm.Text,
            p_query_txt = isQuery ? txtQueryTxt.Text : null,
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

        gvw4.CloseEditor();
        gvw4.UpdateCurrentRow();

        var columnSaveError = await SaveColumnRowsAsync(savedLookupKey);
        if (columnSaveError != null)
        {
            AppMessageBox.Show(columnSaveError, "저장 실패");
            return;
        }

        _params.AcceptChanges();
        _columns.AcceptChanges();
        _editingLookupKey = savedLookupKey;
        await QueryCore(preserveSelection: true); // 방금 저장한 행 유지 - QueryClick(사용자 조회)과 다른 경로
        Toast.Show(wasNew ? "등록되었습니다." : "수정되었습니다.");
    }

    /// <summary>grd2(파라미터 목록)에서 바뀐 행마다 SSP_SYS_LOOKUP_S_1을 한 번씩 호출한다
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
        if (string.IsNullOrEmpty(paramNm)) return null; // 파라미터명 없이 행만 추가된 빈 행은 건너뜀

        var result = await SaveAsync("SSP_SYS_LOOKUP_S_1", ParamParams("N", lookupKey, row, DataRowVersion.Current));
        return result.Success ? null : $"[{paramNm}] {FormatSaveFailMessage(result)}";
    }

    private async Task<string?> SaveModifiedParamAsync(string lookupKey, DataRow row)
    {
        var origParamNm = ProcData.Str(row, "param_nm", DataRowVersion.Original);
        var paramNm = ProcData.Str(row, "param_nm", DataRowVersion.Current);

        if (paramNm != origParamNm)
        {
            var delResult = await SaveAsync("SSP_SYS_LOOKUP_S_1",
                new { p_work_type = "D", p_lookup_key = lookupKey, p_param_nm = origParamNm });
            if (!delResult.Success) return $"[{origParamNm}] {FormatSaveFailMessage(delResult)}";

            var addResult = await SaveAsync("SSP_SYS_LOOKUP_S_1", ParamParams("N", lookupKey, row, DataRowVersion.Current));
            return addResult.Success ? null : $"[{paramNm}] {FormatSaveFailMessage(addResult)}";
        }

        var result = await SaveAsync("SSP_SYS_LOOKUP_S_1", ParamParams("U", lookupKey, row, DataRowVersion.Current));
        return result.Success ? null : $"[{paramNm}] {FormatSaveFailMessage(result)}";
    }

    private async Task<string?> SaveDeletedParamAsync(string lookupKey, DataRow row)
    {
        var paramNm = ProcData.Str(row, "param_nm", DataRowVersion.Original);
        var result = await SaveAsync("SSP_SYS_LOOKUP_S_1",
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

    /// <summary>grd4(컬럼 목록)에서 바뀐 행마다 SSP_SYS_LOOKUP_S_2를 한 번씩 호출한다 -
    /// SaveParamRowsAsync/SaveNewParamAsync 등과 완전히 같은 구조(column_nm이 키).</summary>
    private async Task<string?> SaveColumnRowsAsync(string lookupKey)
    {
        foreach (DataRow row in _columns.Rows)
        {
            string? error = row.RowState switch
            {
                DataRowState.Added => await SaveNewColumnAsync(lookupKey, row),
                DataRowState.Modified => await SaveModifiedColumnAsync(lookupKey, row),
                DataRowState.Deleted => await SaveDeletedColumnAsync(lookupKey, row),
                _ => null
            };
            if (error != null) return error;
        }
        return null;
    }

    private async Task<string?> SaveNewColumnAsync(string lookupKey, DataRow row)
    {
        var columnNm = ProcData.Str(row, "column_nm", DataRowVersion.Current);
        if (string.IsNullOrEmpty(columnNm)) return null; // 컬럼명 없이 행만 추가된 빈 행은 건너뜀

        var result = await SaveAsync("SSP_SYS_LOOKUP_S_2", ColumnParams("N", lookupKey, row, DataRowVersion.Current));
        return result.Success ? null : $"[{columnNm}] {FormatSaveFailMessage(result)}";
    }

    private async Task<string?> SaveModifiedColumnAsync(string lookupKey, DataRow row)
    {
        var origColumnNm = ProcData.Str(row, "column_nm", DataRowVersion.Original);
        var columnNm = ProcData.Str(row, "column_nm", DataRowVersion.Current);

        if (columnNm != origColumnNm)
        {
            var delResult = await SaveAsync("SSP_SYS_LOOKUP_S_2",
                new { p_work_type = "D", p_lookup_key = lookupKey, p_column_nm = origColumnNm });
            if (!delResult.Success) return $"[{origColumnNm}] {FormatSaveFailMessage(delResult)}";

            var addResult = await SaveAsync("SSP_SYS_LOOKUP_S_2", ColumnParams("N", lookupKey, row, DataRowVersion.Current));
            return addResult.Success ? null : $"[{columnNm}] {FormatSaveFailMessage(addResult)}";
        }

        var result = await SaveAsync("SSP_SYS_LOOKUP_S_2", ColumnParams("U", lookupKey, row, DataRowVersion.Current));
        return result.Success ? null : $"[{columnNm}] {FormatSaveFailMessage(result)}";
    }

    private async Task<string?> SaveDeletedColumnAsync(string lookupKey, DataRow row)
    {
        var columnNm = ProcData.Str(row, "column_nm", DataRowVersion.Original);
        var result = await SaveAsync("SSP_SYS_LOOKUP_S_2",
            new { p_work_type = "D", p_lookup_key = lookupKey, p_column_nm = columnNm });
        return result.Success ? null : $"[{columnNm}] {FormatSaveFailMessage(result)}";
    }

    private static Dictionary<string, string?> ColumnParams(string workType, string lookupKey, DataRow row, DataRowVersion version) => new()
    {
        ["p_work_type"] = workType,
        ["p_lookup_key"] = lookupKey,
        ["p_column_nm"] = ProcData.Str(row, "column_nm", version),
        ["p_caption"] = ProcData.Str(row, "caption", version),
        ["p_sort"] = ProcData.Str(row, "sort", version),
        ["p_width"] = ProcData.Str(row, "width", version)
    };

    /// <summary>프로시저(또는 쿼리문)의 입력 파라미터/결과셋 컬럼 구조를 읽어와서(실행은 안 함),
    /// grd2(파라미터 목록)와 grd4(컬럼 목록, sysLookupC)에 아직 없는 것만 기본값으로 채워 넣는다.
    /// 이미 있는 행(사람이 캡션/폭을 손으로 고쳐둔 것)은 그대로 둔다. grd4는 팝업 프레임워크의
    /// 컬럼 그리드(sysPopUpD)와 같은 발상 - LookUp 팝업이 값필드/표시필드 2개 고정이 아니라
    /// 조회된 컬럼 중 원하는 만큼 골라서 폭까지 지정할 수 있게 확장한 것이다(2026-09-03,
    /// 065_Lookup_Columns.sql). 값필드/표시필드 입력창은 여전히 별도로 직접 입력한다(둘은
    /// "값으로 쓸 컬럼이 뭔지"를 가리키는 것이라 grd4의 "팝업에 보여줄 컬럼" 목록과는 다른
    /// 개념 - 값필드가 grd4에 없어도 상관없다, 표시는 안 해도 값으로는 계속 쓸 수 있어야 하므로).
    /// 소스유형이 쿼리면 프로시저 대신 쿼리문 텍스트 자체를 분석한다(2026-09-02 추가 - 쿼리로
    /// 만든 LookUp도 조회된 컬럼을 코드/코드명 후보로 바로 볼 수 있어야 한다는 요청).</summary>
    private async void btnGenerateParams_Click(object sender, EventArgs e)
    {
        var isQuery = IsQuerySource;
        if (isQuery && string.IsNullOrWhiteSpace(txtQueryTxt.Text))
        {
            AppMessageBox.Show("쿼리문을 먼저 입력해주세요.", "안내");
            return;
        }
        if (!isQuery && string.IsNullOrWhiteSpace(txtProcNm.Text))
        {
            AppMessageBox.Show("프로시저명을 먼저 입력해주세요.", "안내");
            return;
        }

        DescribeProcResultDto? discovered;
        try
        {
            discovered = isQuery
                ? await ApiClient.PostAsync<DescribeQueryRequest, DescribeProcResultDto>(
                    "api/lookup-admin/describe-query", new DescribeQueryRequest { QueryText = txtQueryTxt.Text.Trim() })
                : await ApiClient.PostAsync<DescribeProcRequest, DescribeProcResultDto>(
                    "api/lookup-admin/describe-proc", new DescribeProcRequest { ProcName = txtProcNm.Text.Trim() });
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"{(isQuery ? "쿼리" : "프로시저")} 구조를 읽어오지 못했습니다.\n{ex.Message}", "오류");
            return;
        }

        if (discovered == null || (discovered.Columns.Count == 0 && discovered.Params.Count == 0))
        {
            AppMessageBox.Show($"결과셋/파라미터를 찾지 못했습니다. {(isQuery ? "쿼리문을" : "프로시저명을")} 확인해주세요.", "안내");
            return;
        }

        // grd2에 바인딩된 _params를 직접 Delete()/Add()로 건드리는 구간이라 예외가 새면 그리드
        // 바인딩 상태가 깨진 채로 남는다(2026-09-02, 이 예외가 SafeExecute 없이 새서 같은 폼의
        // 조회 리프레쉬까지 오작동한 사고가 실제로 있었음) - try/catch로 감싸서 실패해도 표준
        // 오류 메시지만 뜨고 핸들러가 깨끗하게 끝나도록 한다.
        try
        {
            // 프로시저/쿼리에서 더 이상 나오지 않는 파라미터는 목록에서 제거한다 - 예전엔 새로
            // 발견된 것만 추가하고 없어진 건 그대로 남겨둬서, 프로시저 파라미터를 줄여도(예: 1개
            // -> 0개) grd2에 예전 파라미터가 계속 남아있는 문제가 있었다(2026-09-02, L_ACC에서
            // 실제 발생). row.Delete()로 지워야 RowState가 Deleted가 되어 저장 시 실제 DB 삭제
            // (SSP_SYS_LOOKUP_S_1 work_type=D)까지 이어진다 - Rows.Remove는 그냥 메모리에서만
            // 사라져서 DB에는 안 지워진다.
            var discoveredNames = discovered.Params.Select(p => p.ParamNm).ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (var row in _params.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted).ToList())
            {
                var nm = Convert.ToString(row["param_nm"]);
                if (nm != null && !discoveredNames.Contains(nm))
                    row.Delete();
            }

            var remainingRows = _params.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted).ToList();
            var existing = remainingRows
                .Select(r => Convert.ToString(r["param_nm"]))
                .Where(v => v != null)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var added = 0;
            // Max()가 삭제 대상 행(Deleted)까지 훑으면 기본 인덱서가 DeletedRowInaccessibleException을
            // 던진다 - remainingRows(Deleted 제외)로 계산해야 함(2026-09-02, L_ACC 재현 버그 수정).
            var nextSort = remainingRows.Count == 0 ? 1 : remainingRows.Max(r => Convert.ToInt32(r["sort"])) + 1;

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

            // grd4(컬럼 목록, sysLookupC) - 파라미터와 같은 방식으로 없어진 컬럼은 지우고 새로
            // 발견된 컬럼만 기본값(캡션 없음, 폭 100)으로 채워 넣는다. 이미 있는 행(캡션/폭을
            // 손으로 고친 것)은 그대로 둔다.
            var discoveredColumnNames = discovered.Columns.Select(c => c.ColumnNm).ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (var row in _columns.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted).ToList())
            {
                var nm = Convert.ToString(row["column_nm"]);
                if (nm != null && !discoveredColumnNames.Contains(nm))
                    row.Delete();
            }

            var remainingColumnRows = _columns.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted).ToList();
            var existingColumns = remainingColumnRows
                .Select(r => Convert.ToString(r["column_nm"]))
                .Where(v => v != null)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var columnsAdded = 0;
            var nextColumnSort = remainingColumnRows.Count == 0 ? 1 : remainingColumnRows.Max(r => Convert.ToInt32(r["sort"])) + 1;

            foreach (var c in discovered.Columns)
            {
                if (existingColumns.Contains(c.ColumnNm)) continue;

                var row = _columns.NewRow();
                row["column_nm"] = c.ColumnNm;
                row["caption"] = string.Empty;
                row["sort"] = nextColumnSort++;
                row["width"] = 100;
                _columns.Rows.Add(row);
                columnsAdded++;
            }

            grd4.RefreshDataSource();

            // 값필드(코드)/표시필드(명칭)가 비어있으면 조회된 컬럼 중 첫 번째/두 번째를 그대로
            // 채워 넣는다 - 이미 값이 있으면(직접 입력했거나 이전에 채워둔 것) 건드리지 않는다.
            // "쿼리에서 조회된 컬럼을 코드/코드명 필드로 바로 쓸 수 있어야 한다"는 요청으로 추가
            // (2026-09-02) - 처음엔 토스트 힌트로만 컬럼명을 보여줬는데, 그것만으로는 "아무 일도
            // 안 일어난 것"처럼 보인다는 피드백을 받아 실제로 채워 넣는 쪽으로 바꿨다.
            var fieldsFilled = false;
            if (string.IsNullOrWhiteSpace(txtValueField.Text) && discovered.Columns.Count > 0)
            {
                txtValueField.Text = discovered.Columns[0].ColumnNm;
                fieldsFilled = true;
            }
            if (string.IsNullOrWhiteSpace(txtDisplayField.Text) && discovered.Columns.Count > 1)
            {
                txtDisplayField.Text = discovered.Columns[1].ColumnNm;
                fieldsFilled = true;
            }

            var columnHint = discovered.Columns.Count > 0
                ? $" 사용 가능한 컬럼: {string.Join(", ", discovered.Columns.Select(c => c.ColumnNm))}"
                : string.Empty;
            var fieldMsg = fieldsFilled ? " 값필드/표시필드를 채웠습니다." : string.Empty;
            var paramMsg = added > 0 ? $"파라미터 {added}개" : "새 파라미터 없음";
            var columnMsg = columnsAdded > 0 ? $"컬럼 {columnsAdded}개" : "새 컬럼 없음";
            Toast.Show($"{paramMsg}, {columnMsg}를 추가했습니다.{fieldMsg}{columnHint}");
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"[파라미터생성] 처리 중 오류가 발생했습니다.\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>지금 화면에 입력된 프로시저/쿼리를 실제로 실행해서 grd3에 그대로 보여준다 -
    /// 저장되는 데이터가 아니라 "이게 실제로 잘 도는지, 결과가 어떤 모양인지"만 확인하는 용도
    /// (2026-09-02 요청). 파라미터는 grd2에 등록된 이름만 그대로 서버에 보내고 값은 항상 NULL로
    /// 실행한다(LookupAdminController.Preview 참고) - 특정 값으로 걸러서 보는 기능은 아직 없다.</summary>
    private async void btnPreview_Click(object sender, EventArgs e)
    {
        var isQuery = IsQuerySource;
        if (isQuery && string.IsNullOrWhiteSpace(txtQueryTxt.Text))
        {
            AppMessageBox.Show("쿼리문을 먼저 입력해주세요.", "안내");
            return;
        }
        if (!isQuery && string.IsNullOrWhiteSpace(txtProcNm.Text))
        {
            AppMessageBox.Show("프로시저명을 먼저 입력해주세요.", "안내");
            return;
        }

        // param_nm -> 테스트값(비어있으면 NULL로 실행). 값을 하나도 안 채웠으면 필터 조건에 걸려
        // 매번 0건만 나올 수 있다는 걸 알아둘 것 - 실제로 결과를 보고 싶으면 테스트값을 채워야 한다.
        var paramValues = _params.Rows.Cast<DataRow>()
            .Select(r => Convert.ToString(r["param_nm"]))
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .Select(v => v!)
            .ToDictionary(v => v, v =>
            {
                var row = _params.Rows.Cast<DataRow>().First(r => Convert.ToString(r["param_nm"]) == v);
                var testValue = row.Table.Columns.Contains("test_value") ? Convert.ToString(row["test_value"]) : null;
                return string.IsNullOrWhiteSpace(testValue) ? null : testValue;
            });

        DataQueryResponse? response;
        try
        {
            response = await ApiClient.PostAsync<PreviewRequest, DataQueryResponse>("api/lookup-admin/preview", new PreviewRequest
            {
                SourceType = isQuery ? "Q" : "P",
                ProcName = isQuery ? null : txtProcNm.Text.Trim(),
                QueryText = isQuery ? txtQueryTxt.Text.Trim() : null,
                Params = paramValues
            });
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"실행 중 오류가 발생했습니다.\n{ex.Message}", "오류");
            return;
        }

        var table = response?.Tables.Count > 0 ? ToDataTable(response.Tables[0]) : new DataTable();
        grd3.DataSource = table;
        gvw3.PopulateColumns();
        ApplyColumnListToPreviewGrid();
        Toast.Show($"{table.Rows.Count}건 조회됨(저장되지 않음).");
    }

    /// <summary>grd4(컬럼 목록, sysLookupC)에 등록해둔 필드명(caption)/폭(width)을 미리보기
    /// 그리드(grd3)에도 그대로 적용한다 - 그동안 grd3는 결과 DataTable을 PopulateColumns()로만
    /// 채워서 DB 컬럼명 그대로, 기본 폭으로만 보였다(2026-09-06 요청).</summary>
    private void ApplyColumnListToPreviewGrid()
    {
        foreach (DataRow row in _columns.Rows)
        {
            if (row.RowState == DataRowState.Deleted) continue;

            var columnNm = Convert.ToString(row["column_nm"]);
            if (string.IsNullOrWhiteSpace(columnNm)) continue;

            var column = gvw3.Columns[columnNm];
            if (column == null) continue; // 실행 결과에 그 컬럼이 없으면 건너뜀

            var caption = row.Table.Columns.Contains("caption") ? Convert.ToString(row["caption"]) : null;
            if (!string.IsNullOrWhiteSpace(caption)) column.Caption = caption;

            if (row.Table.Columns.Contains("width") && row["width"] != DBNull.Value
                && int.TryParse(Convert.ToString(row["width"]), out var width) && width > 0)
                column.Width = width;
        }
    }

    private class PreviewRequest
    {
        public string SourceType { get; set; } = "P";
        public string? ProcName { get; set; }
        public string? QueryText { get; set; }
        public Dictionary<string, string?> Params { get; set; } = new();
    }

    /// <summary>다른 행을 고를 때 panData/grd2(파라미터)/grd4(컬럼)에 저장 안 된 변경이 있으면
    /// 먼저 확인한다(BaseForm.ConfirmMasterRowSwitch 참고, 2026-09-06 - 모든 화면 공통 적용).</summary>
    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e) =>
        ConfirmMasterRowSwitch(gvw1, e, row => EnterEditMode(row.Row));

    private DataRow? FindRow(string lookupKey) =>
        _lookups.Rows.Cast<DataRow>()
            .FirstOrDefault(r => string.Equals(Convert.ToString(r["lookup_key"]), lookupKey, StringComparison.OrdinalIgnoreCase));

    /// <summary>lookup_key로 grd1의 행 핸들을 찾는다. 없으면 null. 컬럼 셀 값으로 찾는 방식은
    /// 그 키 컬럼이 화면에 안 보이는 숨김 컬럼일 때 안 먹힌다(2026-09-11 실제 발견, frmEMP에서
    /// 재현) - FindRow로 찾은 DataRow의 DataTable상 인덱스를 GridView.GetRowHandle로 표시 행
    /// 핸들로 변환하면 키 컬럼이 보이든 안 보이든 항상 동작한다.</summary>
    private int? FindRowHandle(string lookupKey)
    {
        var row = FindRow(lookupKey);
        if (row == null) return null;

        var rowIndex = _lookups.Rows.IndexOf(row);
        var handle = gvw1.GetRowHandle(rowIndex);
        return handle >= 0 ? handle : null;
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
            cboSourceType.EditValue = "P"; // 기본값 프로시저 - 기존 동작과 그대로 호환
            txtQueryTxt.Text = string.Empty;
            txtValueField.Text = string.Empty;
            txtDisplayField.Text = string.Empty;
            chkUseYn.Checked = true;
            txtRemark.Text = string.Empty;

            _params = _params.Clone();
            EnsureTestValueColumn(_params);
            TrackDirty(_params);
            grd2.DataSource = _params;
            grd3.DataSource = null;

            _columns = _columns.Clone();
            TrackDirty(_columns);
            grd4.DataSource = _columns;
        });
        ToggleSourceTypeFields();
        txtLookupKey.Focus();
    }

    /// <summary>_params(sysLookupP)에 "테스트값" 칼럼을 추가한다 - DB에는 없는 화면 전용 칼럼이라
    /// 서버에서 조회해 온 DataTable/Clone() 양쪽 다 처음엔 이 칼럼이 없다. 저장 시(ParamParams)는
    /// 이 칼럼을 안 읽으므로 DB에 저장되지 않는다 - 순전히 "실행결과 미리보기" 입력용
    /// (btnPreview_Click 참고).</summary>
    private static void EnsureTestValueColumn(DataTable table)
    {
        if (!table.Columns.Contains("test_value"))
            table.Columns.Add("test_value", typeof(string));
    }

    /// <summary>선택한 LookUp을 그대로 복제해서 "신규모드"로 전환한다 - LookUp키만 비우고
    /// LookUp명 뒤엔 " (복사)"를 붙여 표시(원본이랑 헷갈리지 않게), 나머지 필드(프로시저명/
    /// 쿼리문/값필드/표시필드/사용여부/비고)는 지금 화면에 로드되어 있는 값을 그대로 둔다.
    /// 파라미터 목록도 같이 복제하되, 원본 행을 그대로 재사용하면 DataRowState가 Unchanged라
    /// 저장할 때 하나도 안 들어간다 - 그래서 새 DataTable에 NewRow로 다시 넣어(=Added 상태)
    /// 저장 시 새 LookUp키로 전부 새로 INSERT되게 한다. "L_CM0001을 복사해서 L_CM0002를
    /// 쉽게 만들고 싶다"는 요청으로 추가(2026-09-02).</summary>
    private void btnCopy_Click(object sender, EventArgs e)
    {
        if (_editingLookupKey == null)
        {
            AppMessageBox.Show("복사할 LookUp을 먼저 선택해주세요.", "안내");
            return;
        }

        SuppressDirtyTracking(() =>
        {
            _editingLookupKey = null;
            txtLookupKey.Text = string.Empty;
            txtLookupKey.ReadOnly = false;
            if (!string.IsNullOrWhiteSpace(txtLookupNm.Text)) txtLookupNm.Text += " (복사)";
            // txtProcNm/cboSourceType/txtQueryTxt/txtValueField/txtDisplayField/chkUseYn/txtRemark는
            // 지금 화면에 로드된 값을 그대로 둔다(원본과 동일하게 시작).

            var copiedParams = _params.Clone();
            foreach (DataRow row in _params.Rows)
            {
                var newRow = copiedParams.NewRow();
                newRow["param_nm"] = row["param_nm"];
                newRow["caption"] = row["caption"];
                newRow["sort"] = row["sort"];
                copiedParams.Rows.Add(newRow);
            }
            _params = copiedParams;
            TrackDirty(_params);
            grd2.DataSource = _params;
            grd3.DataSource = null;

            var copiedColumns = _columns.Clone();
            foreach (DataRow row in _columns.Rows)
            {
                var newRow = copiedColumns.NewRow();
                newRow["column_nm"] = row["column_nm"];
                newRow["caption"] = row["caption"];
                newRow["sort"] = row["sort"];
                newRow["width"] = row["width"];
                copiedColumns.Rows.Add(newRow);
            }
            _columns = copiedColumns;
            TrackDirty(_columns);
            grd4.DataSource = _columns;
        });
        ToggleSourceTypeFields();
        txtLookupKey.Focus();
        Toast.Show("복사되었습니다. 새 LookUp키를 입력하고 저장하세요.");
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
            // source_type 컬럼이 아직 없는 기존 데이터(마이그레이션 전 행)도 있을 수 있어서
            // 빈 값이면 프로시저(기존 방식)로 취급한다.
            cboSourceType.EditValue = Str(lookup, "source_type") == "Q" ? "Q" : "P";
            txtQueryTxt.Text = Str(lookup, "query_txt");
            txtValueField.Text = Str(lookup, "value_field");
            txtDisplayField.Text = Str(lookup, "display_field");
            chkUseYn.Checked = Str(lookup, "use_yn") != "N";
            txtRemark.Text = Str(lookup, "remark");
        });
        ToggleSourceTypeFields();
        grd3.DataSource = null;

        if (!isSameLookup)
        {
            _ = LoadParamsAsync(lookupKey);
            _ = LoadColumnsAsync(lookupKey);
        }
    }

    private async Task LoadParamsAsync(string lookupKey)
    {
        _params = await QueryAsync("SSP_SYS_LOOKUP_Q", new
        {
            p_work_type = "Q1",
            p_lookup_key = lookupKey
        });
        EnsureTestValueColumn(_params);
        TrackDirty(_params);

        grd2.DataSource = _params;
    }

    private async Task LoadColumnsAsync(string lookupKey)
    {
        _columns = await QueryAsync("SSP_SYS_LOOKUP_Q", new
        {
            p_work_type = "Q2",
            p_lookup_key = lookupKey
        });
        TrackDirty(_columns);

        grd4.DataSource = _columns;
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

    private class DescribeQueryRequest
    {
        public string QueryText { get; set; } = string.Empty;
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
