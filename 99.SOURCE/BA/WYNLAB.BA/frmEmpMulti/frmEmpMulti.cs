using System.Data;
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.BA;

/// <summary>
/// 사원정보일괄등록 - grd1 한 줄이 사원 한 명. 엑셀에서 복사해 붙여넣거나(Ctrl+V) [엑셀업로드]로 파일을 통째로
/// 읽어 여러 명을 한 번에 입력한 뒤 [검증] -> [저장]으로 신규 등록한다(2026-09-26 frmItemMulti와 같은 방식으로
/// 완성 - 그 전에는 양식다운로드/업로드/검증 버튼이 비어 있었다).
///
/// 엑셀 양식/업로드는 그리드에 "보이는 컬럼 순서" 그대로 값이 매핑된다(헤더 글자로 찾지 않음) - 컬럼 순서를
/// 바꾸면 [엑셀양식다운로드]로 양식을 다시 받아야 한다.
///
/// 사업장은 조회조건줄(cboAccId)의 값을 모든 행에 적용한다. 부서는 "부서명 또는 부서ID"로 입력받아 [검증]에서
/// 실제 부서ID로 바꾼다(P_DEPT 팝업 프레임워크 데이터를 그대로 재사용). 성별(L_CM0005)은 코드/코드명 어느 쪽이든
/// 정확히 일치하는 코드로 바꾼다. 날짜는 yyyyMMdd/yyyy-MM-dd/yyyy.MM.dd 모두 받아 yyyyMMdd로 통일한다. 연차/근태/
/// 급여/퇴사는 Y/N(비우면 N).
///
/// 저장 정책: 검증 오류가 하나라도 있으면 저장 자체를 막는다(부분저장 없음) - [저장]을 누르면 항상 먼저 전체
/// 재검증부터 한다. 사번은 같은 사업장의 기존 사원과 겹치면 안 된다(USP_BA_EMP_S에는 중복 검사가 없어서 화면이 막는다).
/// </summary>
public partial class frmEmpMulti : BaseForm
{
    // 입력 가능한 컬럼(필드명)과 DB 길이 제한 - VARCHAR 길이를 넘는 값은 서버가 오류 없이 잘라버리므로(2026-09-26 확인:
    // 프로시저 파라미터 길이 그대로) 검증에서 미리 막는다. 0 = 길이 제한 검사 안 함(날짜/Y·N/성별은 별도 규칙).
    private static readonly (string Field, string Label, int MaxLen)[] EditColumns =
    {
        ("emp_no", "사번", 20), ("emp_nm", "사원명", 100), ("emp_nm_eng", "사원명(영문)", 100), ("dept_nm", "부서", 0),
        ("ent_date", "입사일자", 0), ("grp_ent_date", "그룹입사일자", 0), ("job_grade", "직급", 10), ("job_type", "직위", 10),
        ("ret_yn", "퇴사", 0), ("ret_date", "퇴사일자", 0), ("sex_cd", "성별", 0),
        ("tel", "전화번호", 30), ("hp_tel", "Mobile", 30), ("email", "E-mail", 50), ("nat_cd", "국가", 10),
        ("zip_code", "우편번호", 20), ("addr1", "주소1", 300), ("addr2", "주소2", 300),
        ("holi_yn", "연차관리", 0), ("dilig_yn", "근태관리", 0), ("pay_yn", "급여관리", 0),
    };

    private DataTable _list = new();

    public frmEmpMulti()
    {
        InitializeComponent();

        Text = "사원정보일괄등록";

        gvw1.Role = GridRoleWyn.Edit;
        gvw1.HighlightFocusedRow = true;
        gvw1.RowAdd += (s, e) => gvw1.AddNewRow();
        gvw1.RowDelete += (s, e) =>
        {
            try { if (gvw1.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };

        // 검증에서 오류가 있던 행은 배경을 옅은 빨강으로 표시한다 - validate_result가 비어있으면 기본색.
        gvw1.RowCellStyle += (s, e) =>
        {
            if (e.RowHandle < 0) return;
            if (string.IsNullOrEmpty(gvw1.GetRowCellValue(e.RowHandle, "validate_result") as string)) return;

            e.Appearance.BackColor = System.Drawing.Color.FromArgb(253, 236, 234);
            e.Appearance.Options.UseBackColor = true;
        };

        cboAccId.Tag = new BindingFieldTag("acc_id");
        cboAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;

        _list = BuildEmptyTable();
        TrackDirty(_list);
        grd1.DataSource = _list;
    }

    private static DataTable BuildEmptyTable()
    {
        var table = new DataTable();
        foreach (var (field, _, _) in EditColumns) table.Columns.Add(field, typeof(string));
        table.Columns.Add("validate_result", typeof(string));
        table.Columns.Add("dept_id", typeof(string));     // [검증]이 역매핑한 부서ID(저장에만 씀)
        table.Columns.Add("sex_cd_resolved", typeof(string));
        return table;
    }

    /// <summary>엑셀 양식/업로드가 따르는 컬럼 순서 = 그리드에 지금 보이는 입력 컬럼 순서.</summary>
    private List<string> ExcelFields() =>
        gvw1.VisibleColumns.Cast<DevExpress.XtraGrid.Columns.GridColumn>()
            .Select(c => c.FieldName)
            .Where(f => EditColumns.Any(e => e.Field == f))
            .ToList();

    // 이 화면엔 "조회"할 기존 목록이 없다(일괄 입력 전용) - 툴바 조회 버튼은 아무 일도 안 한다.
    public override Task QueryClick() => Task.CompletedTask;

    public override Task NewClick()
    {
        if (_list.Rows.Count == 0) return Task.CompletedTask;

        var confirm = AppMessageBox.Show(
            "입력 중인 내용을 모두 지우고 새로 시작하시겠습니까?",
            "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return Task.CompletedTask;

        _list.Rows.Clear();
        grd1.RefreshDataSource();
        return Task.CompletedTask;
    }

    public override Task DeleteClick() => DeleteRowClick();

    public override Task NewRowClick()
    {
        gvw1.AddNewRow();
        return Task.CompletedTask;
    }

    public override Task DeleteRowClick()
    {
        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        var handle = gvw1.FocusedRowHandle;
        if (handle >= 0) gvw1.DeleteRow(handle);
        return Task.CompletedTask;
    }

    // ==================== 엑셀 양식/업로드 ====================

    private void btnDownloadTemplate_Click(object sender, EventArgs e) => DownloadTemplate();
    private void btnUploadExcel_Click(object sender, EventArgs e) => UploadExcel();

    private async void btnValidate_Click(object sender, EventArgs e)
    {
        try
        {
            var (ok, errorCount) = await ValidateAllAsync();
            grd1.RefreshDataSource();
            Toast.Show(ok ? "검증을 통과했습니다." : $"{errorCount}건의 행에서 오류가 발견되었습니다 - 빨간색 행을 확인해주세요.");
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"검증 중 오류가 발생했습니다.\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>빈 양식(헤더만)을 엑셀로 내려받는다 - 지금 그리드를 건드리지 않고 컬럼 구조만 같은 빈 DataTable로
    /// 잠깐 바꿔치기했다가 내보낸 뒤 원래대로 되돌린다(검증결과 컬럼은 양식에서 뺀다).</summary>
    private void DownloadTemplate()
    {
        using var dlg = new SaveFileDialog
        {
            Filter = "Excel (*.xlsx)|*.xlsx",
            FileName = $"사원일괄등록_양식_{DateTime.Now:yyyyMMdd}.xlsx"
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        var originalSource = grd1.DataSource;
        var validateWasVisible = col1ValidateResult.Visible;
        try
        {
            gvw1.CloseEditor();
            col1ValidateResult.Visible = false;
            grd1.DataSource = _list.Clone();
            gvw1.ExportToXlsx(dlg.FileName);
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"엑셀 양식을 저장하는 중 오류가 발생했습니다.\n{ex.Message}", "다운로드 실패");
            return;
        }
        finally
        {
            grd1.DataSource = originalSource;
            col1ValidateResult.Visible = validateWasVisible;
        }

        Toast.Show("엑셀 양식을 저장했습니다.");
        try { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(dlg.FileName) { UseShellExecute = true }); }
        catch { /* 저장 자체는 끝났으니 여는 것만 실패해도 무시 */ }
    }

    /// <summary>엑셀 파일을 통째로 읽어 그리드 끝에 행으로 추가한다. 첫 행은 헤더로 보고 건너뛰며, 그 다음 행부터
    /// 그리드에 "보이는 컬럼 순서" 그대로 값을 채운다. DevExpress SpreadsheetControl(화면에 안 띄움)로 읽는다.</summary>
    private void UploadExcel()
    {
        using var dlg = new OpenFileDialog { Filter = "Excel (*.xlsx)|*.xlsx" };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        using var spreadsheet = new SpreadsheetControl();
        try
        {
            spreadsheet.LoadDocument(dlg.FileName);
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"엑셀 파일을 여는 중 오류가 발생했습니다.\n{ex.Message}", "업로드 실패");
            return;
        }

        var fields = ExcelFields();
        var sheet = spreadsheet.Document.Worksheets.First();
        var used = sheet.GetUsedRange();
        var added = 0;

        for (var r = used.TopRowIndex + 1; r <= used.BottomRowIndex; r++) // 1행(헤더)은 건너뜀
        {
            var row = _list.NewRow();
            var hasAny = false;

            for (var i = 0; i < fields.Count; i++)
            {
                var c = used.LeftColumnIndex + i;
                if (c > used.RightColumnIndex) break;

                var text = sheet[r, c].DisplayText?.Trim() ?? string.Empty;
                if (text.Length > 0) hasAny = true;
                row[fields[i]] = text;
            }

            if (!hasAny) continue; // 완전히 빈 줄은 건너뜀
            _list.Rows.Add(row);
            added++;
        }

        grd1.RefreshDataSource();
        Toast.Show($"{added}건 불러왔습니다. [검증]을 눌러 확인해주세요.");
    }

    // ==================== 검증 ====================

    /// <summary>그리드 전체 행을 검증한다 - 결과는 각 행의 validate_result(그리드 표시)와 dept_id/sex_cd_resolved(저장에
    /// 쓸 역매핑 값, 숨김)에 반영된다. 반환값은 (전체 통과 여부, 오류 있는 행 수).</summary>
    private async Task<(bool Ok, int ErrorCount)> ValidateAllAsync()
    {
        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        var accId = cboAccId.EditValue?.ToString();
        var rows = _list.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted).ToList();

        // 고정 목록은 반복문 전에 한 번만 받아 재사용한다.
        var depts = await LoadPopupRowsAsync("P_DEPT");
        var sexItems = await FetchComboItemsAsync("L_CM0005");
        var existingNos = await LoadExistingEmpNosAsync(accId);
        var seenNos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var row in rows)
        {
            if (EditColumns.All(c => Str(row, c.Field).Length == 0))
            {
                row["validate_result"] = string.Empty;
                continue; // 완전히 빈 줄(붙여넣기로 남은 자리 등)은 검증 대상에서 뺀다.
            }

            var errors = new List<string>();

            var empNo = Str(row, "emp_no");
            if (empNo.Length == 0) errors.Add("사번 필수");
            else if (!seenNos.Add(empNo)) errors.Add("사번 중복(입력한 행 안에서)");
            else if (existingNos.Contains(empNo)) errors.Add("사번 중복(이미 등록됨)");
            if (Str(row, "emp_nm").Length == 0) errors.Add("사원명 필수");

            foreach (var (field, label, maxLen) in EditColumns)
                if (maxLen > 0 && Str(row, field).Length > maxLen) errors.Add($"{label} {maxLen}자 초과");

            // 부서(필수) - 부서명 또는 부서ID
            row["dept_id"] = ResolveDept(depts, Str(row, "dept_nm"), errors, out var deptNm);
            if (deptNm != null) row["dept_nm"] = deptNm;

            // 날짜
            foreach (var (field, label) in new[] { ("ent_date", "입사일자"), ("grp_ent_date", "그룹입사일자"), ("ret_date", "퇴사일자") })
            {
                var text = Str(row, field);
                if (text.Length == 0) continue;

                var ymd = NormalizeYmd(text);
                if (ymd == null) errors.Add($"{label} 형식 오류('{text}')");
                else row[field] = ymd;
            }

            // Y/N
            foreach (var (field, label) in new[] { ("ret_yn", "퇴사"), ("holi_yn", "연차관리"), ("dilig_yn", "근태관리"), ("pay_yn", "급여관리") })
            {
                var text = Str(row, field);
                if (text.Length == 0) continue;
                if (!text.Equals("Y", StringComparison.OrdinalIgnoreCase) && !text.Equals("N", StringComparison.OrdinalIgnoreCase))
                    errors.Add($"{label}는 Y 또는 N");
            }

            // 성별 - 코드 또는 코드명
            var sexText = Str(row, "sex_cd");
            row["sex_cd_resolved"] = null;
            if (sexText.Length > 0)
            {
                var match = sexItems.FirstOrDefault(it =>
                    string.Equals(it.Value, sexText, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(it.Display?.Trim(), sexText, StringComparison.OrdinalIgnoreCase));
                if (match == null) errors.Add($"성별 '{sexText}' 없음");
                else row["sex_cd_resolved"] = match.Value;
            }

            if (Str(row, "ret_yn").Equals("Y", StringComparison.OrdinalIgnoreCase) && Str(row, "ret_date").Length == 0)
                errors.Add("퇴사자는 퇴사일자 필요");

            row["validate_result"] = errors.Count == 0 ? string.Empty : string.Join("; ", errors);
        }

        var errorRows = rows.Count(r => !string.IsNullOrEmpty(r["validate_result"]?.ToString()));
        return (errorRows == 0, errorRows);
    }

    /// <summary>부서 입력(부서명 또는 부서ID)을 부서ID로 바꾼다. 못 찾거나 이름이 여러 개면 오류 - 정규화된 부서명은
    /// resolvedName으로 돌려줘서 셀에 다시 쓴다(ID로 입력했어도 이름이 보이게).</summary>
    private static string? ResolveDept(DataTable depts, string text, List<string> errors, out string? resolvedName)
    {
        resolvedName = null;
        if (text.Length == 0)
        {
            errors.Add("부서 필수");
            return null;
        }

        var all = depts.Rows.Cast<DataRow>().ToList();
        DataRow? match = null;

        if (long.TryParse(text, out var id))
            match = all.FirstOrDefault(r => long.TryParse(Str(r, "dept_id"), out var v) && v == id);

        if (match == null)
        {
            var byName = all.Where(r => string.Equals(Str(r, "dept_nm"), text, StringComparison.OrdinalIgnoreCase)).ToList();
            if (byName.Count > 1)
            {
                errors.Add($"부서 '{text}' 여러 건 일치(모호함)");
                return null;
            }
            match = byName.FirstOrDefault();
        }

        if (match == null)
        {
            errors.Add($"부서 '{text}' 없음");
            return null;
        }

        resolvedName = Str(match, "dept_nm");
        return Str(match, "dept_id");
    }

    /// <summary>같은 사업장에 이미 등록된 사번 목록(중복 검사용). 조회에 실패하면 예외를 그대로 올린다 - 조용히 빈
    /// 목록으로 넘어가면 중복 사번이 그대로 저장된다.</summary>
    private async Task<HashSet<string>> LoadExistingEmpNosAsync(string? accId)
    {
        var table = await QueryAsync("USP_BA_EMP_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q" });
        return new HashSet<string>(
            table.Rows.Cast<DataRow>()
                .Where(r => string.IsNullOrEmpty(accId) || Str(r, "acc_id").Length == 0 || Str(r, "acc_id") == accId)
                .Select(r => Str(r, "emp_no"))
                .Where(s => s.Length > 0),
            StringComparer.OrdinalIgnoreCase);
    }

    private static async Task<List<CodeLookupItem>> FetchComboItemsAsync(string lookupKey)
    {
        if (ComboLookupProvider.Fetch == null) return new List<CodeLookupItem>();

        try
        {
            var result = await ComboLookupProvider.Fetch(lookupKey, new Dictionary<string, string?>());
            return result.Items;
        }
        catch
        {
            return new List<CodeLookupItem>();
        }
    }

    // ==================== 저장 ====================

    public override async Task SaveClick()
    {
        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        var accId = cboAccId.EditValue?.ToString();
        if (string.IsNullOrEmpty(accId))
        {
            AppMessageBox.Show("사업장을 먼저 선택해주세요.", "확인");
            return;
        }

        var rows = _list.Rows.Cast<DataRow>()
            .Where(r => r.RowState != DataRowState.Deleted && EditColumns.Any(c => Str(r, c.Field).Length > 0))
            .ToList();
        if (rows.Count == 0)
        {
            AppMessageBox.Show("등록할 사원이 없습니다.", "안내");
            return;
        }

        var (ok, errorCount) = await ValidateAllAsync();
        grd1.RefreshDataSource();
        if (!ok)
        {
            AppMessageBox.Show($"검증 오류가 있는 행이 {errorCount}건 있어 저장할 수 없습니다.\n빨간색 행의 검증결과를 확인해주세요.", "저장 불가");
            return;
        }

        var saved = 0;
        foreach (var row in rows)
        {
            var result = await SaveAsync("USP_BA_EMP_S", new Dictionary<string, string?>
            {
                ["p_work_type"] = "N",
                ["p_acc_id"] = accId,
                ["p_emp_no"] = Str(row, "emp_no"),
                ["p_emp_nm"] = Str(row, "emp_nm"),
                ["p_emp_nm_eng"] = NullIfEmpty(row, "emp_nm_eng"),
                ["p_dept_id"] = NullIfEmpty(row, "dept_id"),
                ["p_ent_date"] = NullIfEmpty(row, "ent_date"),
                ["p_grp_ent_date"] = NullIfEmpty(row, "grp_ent_date"),
                ["p_job_grade"] = NullIfEmpty(row, "job_grade"),
                ["p_job_type"] = NullIfEmpty(row, "job_type"),
                ["p_ret_yn"] = YOrN(row, "ret_yn"),
                ["p_ret_date"] = NullIfEmpty(row, "ret_date"),
                ["p_sex_cd"] = NullIfEmpty(row, "sex_cd_resolved"),
                ["p_tel"] = NullIfEmpty(row, "tel"),
                ["p_hp_tel"] = NullIfEmpty(row, "hp_tel"),
                ["p_email"] = NullIfEmpty(row, "email"),
                ["p_nat_cd"] = NullIfEmpty(row, "nat_cd"),
                ["p_zip_code"] = NullIfEmpty(row, "zip_code"),
                ["p_addr1"] = NullIfEmpty(row, "addr1"),
                ["p_addr2"] = NullIfEmpty(row, "addr2"),
                ["p_holi_yn"] = YOrN(row, "holi_yn"),
                ["p_dilig_yn"] = YOrN(row, "dilig_yn"),
                ["p_pay_yn"] = YOrN(row, "pay_yn"),
            });

            if (result == null || !result.Success)
            {
                AppMessageBox.Show(
                    $"[{Str(row, "emp_no")}] {FormatSaveFailMessage(result)}\n\n앞서 {saved}건은 이미 등록되었습니다.",
                    "저장 실패");
                return;
            }
            saved++;
        }

        _list.Rows.Clear();
        grd1.RefreshDataSource();
        Toast.Show($"{saved}건 등록되었습니다.");
    }

    // ==================== 공통 ====================

    private static string Str(DataRow row, string columnName) =>
        row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value
            ? (Convert.ToString(row[columnName]) ?? string.Empty).Trim()
            : string.Empty;

    private static string? NullIfEmpty(DataRow row, string columnName)
    {
        var s = Str(row, columnName);
        return s.Length == 0 ? null : s;
    }

    private static string YOrN(DataRow row, string columnName) =>
        string.Equals(Str(row, columnName), "Y", StringComparison.OrdinalIgnoreCase) ? "Y" : "N";

    /// <summary>"2026-01-01"/"2026.1.1"/"20260101" 어느 형식이든 실제 존재하는 날짜면 yyyyMMdd로, 아니면 null.</summary>
    private static string? NormalizeYmd(string text)
    {
        var digits = new string(text.Where(char.IsDigit).ToArray());
        if (digits.Length != 8) return null;

        return DateTime.TryParseExact(digits, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out _) ? digits : null;
    }

    private static string FormatSaveFailMessage(ApiResult? result)
    {
        var message = result?.Message ?? "저장에 실패했습니다.";
        return result is { ErrorCode: not 0 } ? $"{message} (오류코드: {result.ErrorCode})" : message;
    }
}
