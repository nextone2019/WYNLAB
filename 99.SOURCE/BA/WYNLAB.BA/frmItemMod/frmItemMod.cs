using System.Data;
using DevExpress.XtraGrid.Columns;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

/// <summary>
/// 품목일괄수정 화면 - 조회조건으로 전체 품목을 grd1에 편집 가능한 그리드로 불러와 여러 품목의 정보를 한꺼번에 고친 뒤 [저장]한다.
/// 신규 품목은 품목일괄등록(frmItemMulti), 삭제는 품목등록(frmItem)에서 하므로 이 화면은 행 추가/삭제를 막는다. 저장은 새 프로시저 없이 frmItem과 똑같은
/// USP_BA_ITEM_S(U)를 "변경한 행마다" 호출한다(조회도 USP_BA_ITEM_Q 그대로 - PROC_PREFIX USP_BA_ITEM_ 안에 들어온다).
///
/// 창고/위치/담당부서/담당자/구매처는 이름 칸에 팝업(P_WH/P_LOC/P_DEPT/P_EMP/P_CUST) 편집기를 붙이고 숨은 ID 칸을 같이 맞춘다(ResultSelected, frmEtcOut과 같은 방식).
/// 이름 칸을 지우면 ID도 비워 연결이 해제된다(281번 마이그레이션으로 USP_BA_ITEM_S(U)가 담당자/구매처의 빈 값도 저장한다).
/// 단위/발주단위/자산구분/상태/품목그룹1~4는 콤보 컬럼, 수입검사/공정검사/LOT관리/재고관리는 체크 컬럼이다. 품목그룹 1~4는 편집할 때 상위 그룹에 속한 하위 그룹만 고를 수 있다(상위를 바꾸면 하위는 비워진다, CustomRowCellEditForEditing). 표시는 그 단 전체 그룹 이름으로 한다.
///
/// 저장 전 검사(하나라도 걸리면 아무것도 저장하지 않는다): 품번/품명 필수, 그리드 안에서 품번 중복 금지, 품번을 바꾼 행은 이미 등록된 다른 품번과 겹치지 않는지 DB 확인
/// (USP_BA_ITEM_Q Q2), 이름만 직접 고치고 팝업으로 확정하지 않은 행(이름과 ID가 어긋남) 금지. 검사를 통과한 뒤에는 행마다 저장하며, 저장 도중 실패한 행은 수정 상태로 남겨 둔다.
/// </summary>
public partial class frmItemMod : BaseForm
{
    // 이름 칸 -> (ID 필드, 이름 필드). 이름만 바뀌고 ID가 그대로면 팝업으로 확정하지 않은 입력이다.
    private static readonly (string Name, string Id, string Caption)[] MasterPairs =
    {
        ("wh_nm", "wh_id", "창고"), ("loc_nm", "loc_id", "위치"), ("dept_nm", "dept_id", "담당부서"),
        ("emp_nm", "emp_id", "담당자"), ("cust_nm", "cust_id", "구매처"),
    };

    private DataTable _list = new();
    private bool _syncing;

    // 품목그룹 1~4단 - 단(level)별 전체 그룹(id, 이름, 상위 id)과 (단, 상위)별로 만들어 둔 편집용 콤보. 편집할 때만 상위 그룹에 속한 하위 그룹만 보여준다.
    private GridColumn[] _grpCols = Array.Empty<GridColumn>();
    private readonly Dictionary<int, List<(string Id, string Nm, string Par)>> _grp = new();
    private readonly Dictionary<(int Lvl, string Par), DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit> _grpEditors = new();

    public frmItemMod()
    {
        InitializeComponent();
        _grpCols = new[] { colGrp1, colGrp2, colGrp3, colGrp4 };

        // 조회조건 사업장 - 화면 표준: 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "품목일괄수정";

        Controls.Add(BuildScreenHeader());

        // 품목그룹 1~4단 콤보 - 파라미터를 채운 "뒤에" LookupKey를 지정해야 한다(frmItem 주석 참고). 상위 그룹 조건은 비워서 그 단의 전체 그룹을 보여준다.
        var grpEditors = new[] { lookupcolGrp1, lookupcolGrp2, lookupcolGrp3, lookupcolGrp4 };
        for (var i = 0; i < grpEditors.Length; i++)
        {
            grpEditors[i].SetParam("p_grp_lvl", (i + 1).ToString());
            grpEditors[i].SetParam("p_par_grp_id", string.Empty);
            grpEditors[i].LookupKey = "L_ITEM_GRP";
        }

        gvw1.Role = GridRoleWyn.Edit;
        gvw1.HighlightFocusedRow = true;
        gvw1.RowAdd += (s, e) => AppMessageBox.Show("신규 품목은 품목일괄등록 화면에서 등록합니다.", "안내");
        gvw1.RowDelete += (s, e) => AppMessageBox.Show("품목 삭제는 품목등록 화면에서 합니다.", "안내");
        gvw1.CellValueChanged += Gvw1_CellValueChanged;
        gvw1.CustomRowCellEditForEditing += Gvw1_CustomRowCellEditForEditing;

        popcolWh.ResultSelected += (h, r) => ApplyPicked(h, r, colWhId, colWhNm, "wh_nm");
        popcolLoc.ResultSelected += (h, r) => ApplyPicked(h, r, colLocId, colLocNm, "loc_nm");
        popcolDept.ResultSelected += (h, r) => ApplyPicked(h, r, colDeptId, colDeptNm, "dept_nm");
        popcolEmp.ResultSelected += (h, r) =>
        {
            ApplyPicked(h, r, colEmpId, colEmpNm, "emp_nm");
            // 담당자를 고르면 사원의 소속 부서도 같이 채운다(P_EMP가 dept_id/dept_nm을 돌려준다)
            if (r.Row != null && r.Row.TryGetValue("dept_id", out var deptId) && !string.IsNullOrEmpty(deptId))
            {
                r.Row.TryGetValue("dept_nm", out var deptNm);
                _syncing = true;
                try
                {
                    gvw1.SetRowCellValue(h, colDeptId, deptId);
                    gvw1.SetRowCellValue(h, colDeptNm, deptNm);
                }
                finally { _syncing = false; }
            }
        };
        // 담당자 팝업은 그 행에 선택된 담당부서 소속 인원만 보인다(P_EMP의 부서명 조건 p_dept_nm을 미리 채움). 부서가 비어 있으면 전체.
        popcolEmp.ConditionProvider = () =>
        {
            var dept = Convert.ToString(gvw1.GetRowCellValue(gvw1.FocusedRowHandle, colDeptNm))?.Trim();
            return string.IsNullOrEmpty(dept) ? new Dictionary<string, string?>() : new Dictionary<string, string?> { ["p_dept_nm"] = dept };
        };
        popcolCust.ResultSelected += (h, r) => ApplyPicked(h, r, colCustId, colCustNm, "cust_nm");

        Load += async (s, e) =>
        {
            await SafeExecuteAsync(LoadGroupsAsync, "품목그룹 조회");
            await QueryClick();
        };
    }

    // ===== 품목그룹 연쇄 =====

    /// <summary>1~4단 품목그룹 전체를 한 번 받아 둔다(L_ITEM_GRP, 상위 조건 없이 단별 전체).</summary>
    private async Task LoadGroupsAsync()
    {
        if (ComboLookupProvider.Fetch == null) return;
        for (var lvl = 1; lvl <= 4; lvl++)
        {
            var result = await ComboLookupProvider.Fetch("L_ITEM_GRP", new Dictionary<string, string?> { ["p_grp_lvl"] = lvl.ToString(), ["p_par_grp_id"] = string.Empty });
            _grp[lvl] = result.Items.Select(i => (
                i.Value, i.Display,
                i.Row != null && i.Row.TryGetValue("par_grp_id", out var par) ? par ?? string.Empty : string.Empty)).ToList();
        }
        _grpEditors.Clear();
    }

    /// <summary>해당 단의 편집용 콤보 - 1단은 전체, 2단 이상은 상위 그룹에 속한 것만(상위가 비면 빈 목록). 맨 위 빈 항목으로 선택을 해제할 수 있다.</summary>
    private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit GetGrpEditor(int lvl, string? parent)
    {
        var key = (lvl, lvl == 1 ? string.Empty : parent ?? string.Empty);
        if (_grpEditors.TryGetValue(key, out var editor)) return editor;

        var table = new DataTable();
        table.Columns.Add("id", typeof(string));
        table.Columns.Add("nm", typeof(string));
        table.Rows.Add(string.Empty, string.Empty);
        if (_grp.TryGetValue(lvl, out var all))
            foreach (var g in all.Where(g => lvl == 1 || (key.Item2.Length > 0 && g.Par == key.Item2)))
                table.Rows.Add(g.Id, g.Nm);

        editor = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        {
            DataSource = table, ValueMember = "id", DisplayMember = "nm", NullText = string.Empty, ShowHeader = false,
        };
        editor.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nm"));
        grd1.RepositoryItems.Add(editor);
        _grpEditors[key] = editor;
        return editor;
    }

    private void Gvw1_CustomRowCellEditForEditing(object? sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
    {
        var idx = Array.IndexOf(_grpCols, e.Column);
        if (idx < 0 || _grp.Count == 0) return;
        var parent = idx == 0 ? null : Convert.ToString(gvw1.GetRowCellValue(e.RowHandle, _grpCols[idx - 1]));
        e.RepositoryItem = GetGrpEditor(idx + 1, parent);
    }

    // ===== 조회 =====

    public override async Task QueryClick()
    {
        var raw = await QueryAsync("USP_BA_ITEM_Q", new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "Q",
            ["p_item_no"] = txtSearchItemNo.Text.Trim(),
            ["p_item_nm"] = txtSearchItemNm.Text.Trim(),
        });

        _list = ToEditTable(raw);
        TrackDirty(_list);
        grd1.DataSource = _list;
    }

    /// <summary>조회 결과를 편집용 테이블로 바꾼다 - 콤보/팝업 컬럼이 값을 문자열로 비교하므로 ID 계열(bigint)도 문자열로 담고 안전재고만 숫자로 둔다.
    /// 모든 행은 변경 없음(Unchanged) 상태로 시작해야 "고친 행만 저장"이 된다.</summary>
    private static DataTable ToEditTable(DataTable raw)
    {
        var table = new DataTable();
        foreach (DataColumn c in raw.Columns)
            table.Columns.Add(c.ColumnName, c.ColumnName == "safe_qty" ? typeof(decimal) : typeof(string));
        foreach (DataRow r in raw.Rows)
        {
            var n = table.NewRow();
            foreach (DataColumn c in raw.Columns)
            {
                var v = r[c];
                if (v == null || v == DBNull.Value) continue;
                n[c.ColumnName] = c.ColumnName == "safe_qty" ? Convert.ToDecimal(v) : Convert.ToString(v) ?? string.Empty;
            }
            table.Rows.Add(n);
        }
        table.AcceptChanges();
        return table;
    }

    // ===== 편집 보조 =====

    private void Gvw1_CellValueChanged(object? sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
    {
        if (_syncing) return;

        // 안전재고에 "1,500"처럼 문자열을 입력하면 decimal로 바꿔 넣는다(다른 화면과 같은 처리)
        if (e.Column == colSafeQty && e.Value is string text)
        {
            _syncing = true;
            try
            {
                gvw1.SetRowCellValue(e.RowHandle, e.Column,
                    decimal.TryParse(text.Replace(",", ""), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var n)
                        ? n : (object)DBNull.Value);
            }
            finally { _syncing = false; }
            return;
        }

        // 상위 품목그룹을 바꾸면 하위 그룹은 더 이상 유효하지 않을 수 있어 비운다(품목등록 화면과 같은 규칙).
        var gi = Array.IndexOf(_grpCols, e.Column);
        if (gi >= 0 && gi < 3)
        {
            _syncing = true;
            try { for (var j = gi + 1; j < 4; j++) gvw1.SetRowCellValue(e.RowHandle, _grpCols[j], null); }
            finally { _syncing = false; }
            return;
        }

        // 이름을 지우면 연결(ID)도 해제한다. 팝업 선택은 ApplyPicked가 ID/이름을 함께 채운다.
        foreach (var (name, id, _) in MasterPairs)
        {
            if (e.Column.FieldName != name || !string.IsNullOrWhiteSpace(Convert.ToString(e.Value))) continue;
            _syncing = true;
            try { gvw1.SetRowCellValue(e.RowHandle, id, null); }
            finally { _syncing = false; }
            return;
        }
    }

    private void ApplyPicked(int rowHandle, PopupLookupResult result, GridColumn idColumn, GridColumn nameColumn, string nameField)
    {
        string? name = null;
        if (result.Row != null)
            foreach (var kv in result.Row)
                if (string.Equals(kv.Key, nameField, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(kv.Value)) { name = kv.Value; break; }
        name ??= result.Display;

        _syncing = true;
        try
        {
            gvw1.SetRowCellValue(rowHandle, idColumn, result.Code);
            gvw1.SetRowCellValue(rowHandle, nameColumn, name);
        }
        finally { _syncing = false; }
    }

    // ===== 저장 =====

    private static string Cur(DataRow row, string col) => Convert.ToString(row[col, DataRowVersion.Current])?.Trim() ?? string.Empty;
    private static string Orig(DataRow row, string col) => Convert.ToString(row[col, DataRowVersion.Original])?.Trim() ?? string.Empty;

    public override async Task SaveClick()
    {
        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        var modified = _list.Rows.Cast<DataRow>().Where(r => r.RowState == DataRowState.Modified).ToList();
        if (modified.Count == 0)
        {
            Toast.Show("변경된 내용이 없습니다.");
            return;
        }

        var errors = new List<string>();

        // 1) 필수값 / 이름-ID 어긋남
        foreach (var row in modified)
        {
            var label = $"[{Cur(row, "item_no")}]";
            if (Cur(row, "item_no").Length == 0) errors.Add($"품번이 비어 있는 행이 있습니다. (품목ID {Cur(row, "item_id")})");
            if (Cur(row, "item_nm").Length == 0) errors.Add($"{label} 품명을 입력하세요.");
            foreach (var (name, id, caption) in MasterPairs)
                if (Cur(row, name) != Orig(row, name) && Cur(row, id) == Orig(row, id) && Cur(row, name).Length > 0)
                    errors.Add($"{label} {caption}은(는) 팝업에서 선택하세요. (직접 입력한 이름 '{Cur(row, name)}'과 연결이 맞지 않습니다)");
        }

        // 1-1) 품목그룹: 그룹을 건드린 행은 하위 그룹이 바로 위 상위 그룹에 속해야 한다(안 건드린 기존 데이터는 검사하지 않는다)
        if (_grp.Count == 4)
            foreach (var row in modified.Where(r => new[] { "grp1_id", "grp2_id", "grp3_id", "grp4_id" }.Any(c => Cur(r, c) != Orig(r, c))))
                for (var lvl = 2; lvl <= 4; lvl++)
                {
                    var child = Cur(row, $"grp{lvl}_id");
                    if (child.Length == 0) continue;
                    var parent = Cur(row, $"grp{lvl - 1}_id");
                    var found = _grp[lvl].FirstOrDefault(g => g.Id == child);
                    if (found.Id != null && (parent.Length == 0 || found.Par != parent))
                        errors.Add($"[{Cur(row, "item_no")}] 품목그룹{lvl}이(가) 상위 품목그룹{lvl - 1}에 속하지 않습니다.");
                }

        // 2) 그리드 안 품번 중복
        var dup = _list.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted)
            .GroupBy(r => Cur(r, "item_no"), StringComparer.OrdinalIgnoreCase)
            .Where(g => g.Key.Length > 0 && g.Count() > 1).Select(g => g.Key).ToList();
        foreach (var no in dup) errors.Add($"품번 중복(목록 안): {no}");

        // 3) 품번을 바꾼 행은 이미 등록된 다른 품번과 겹치는지 DB에서 확인(목록에 있는 품번은 위 중복 검사가 이미 본다)
        var changedNos = modified.Where(r => Cur(r, "item_no") != Orig(r, "item_no") && Cur(r, "item_no").Length > 0).Select(r => Cur(r, "item_no")).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        if (changedNos.Count > 0)
        {
            var found = await QueryAsync("USP_BA_ITEM_Q", new Dictionary<string, string?>
            {
                ["p_work_type"] = "Q2",
                ["p_item_no_list"] = string.Join(",", changedNos),
            });
            var loaded = _list.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted)
                .Select(r => Orig(r, "item_no")).ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (DataRow f in found.Rows)
            {
                var no = Convert.ToString(f["item_no"]) ?? string.Empty;
                if (!loaded.Contains(no)) errors.Add($"품번 중복(이미 등록됨): {no}");
            }
        }

        if (errors.Count > 0)
        {
            AppMessageBox.Show("저장하지 않았습니다. 아래 내용을 먼저 고쳐주세요.\n\n" + string.Join("\n", errors.Distinct().Take(15))
                + (errors.Count > 15 ? $"\n... 외 {errors.Count - 15}건" : string.Empty), "저장 확인");
            return;
        }

        // 4) 행마다 저장 - 성공한 행만 확정하고 실패한 행은 수정 상태로 남긴다.
        var saved = 0;
        var failed = new List<string>();
        foreach (var row in modified)
        {
            const DataRowVersion v = DataRowVersion.Current;
            var result = await SaveAsync("USP_BA_ITEM_S", new Dictionary<string, string?>
            {
                ["p_work_type"] = "U",
                ["p_acc_id"] = ProcData.Str(row, "acc_id", v),
                ["p_item_id"] = ProcData.Str(row, "item_id", v),
                ["p_item_no"] = ProcData.Str(row, "item_no", v),
                ["p_item_nm"] = ProcData.Str(row, "item_nm", v),
                ["p_item_spec"] = ProcData.Str(row, "item_spec", v),
                ["p_unit_cd"] = ProcData.Str(row, "unit_cd", v),
                ["p_po_unit_cd"] = ProcData.Str(row, "po_unit_cd", v),
                ["p_wh_id"] = ProcData.Str(row, "wh_id", v),
                ["p_loc_id"] = ProcData.Str(row, "loc_id", v),
                ["p_safe_qty"] = ProcData.Str(row, "safe_qty", v),
                ["p_dept_id"] = ProcData.Str(row, "dept_id", v),
                ["p_emp_id"] = ProcData.Str(row, "emp_id", v),
                ["p_cust_id"] = ProcData.Str(row, "cust_id", v),
                ["p_asset_type"] = ProcData.Str(row, "asset_type", v),
                ["p_out_type"] = ProcData.Str(row, "out_type", v),
                ["p_po_qc_yn"] = ProcData.Str(row, "po_qc_yn", v),
                ["p_prod_qc_yn"] = ProcData.Str(row, "prod_qc_yn", v),
                ["p_lot_yn"] = ProcData.Str(row, "lot_yn", v),
                ["p_stock_yn"] = ProcData.Str(row, "stock_yn", v),
                ["p_stat_cd"] = ProcData.Str(row, "stat_cd", v),
                ["p_grp1_id"] = ProcData.Str(row, "grp1_id", v),
                ["p_grp2_id"] = ProcData.Str(row, "grp2_id", v),
                ["p_grp3_id"] = ProcData.Str(row, "grp3_id", v),
                ["p_grp4_id"] = ProcData.Str(row, "grp4_id", v),
                ["p_remark"] = ProcData.Str(row, "remark", v),
            });
            if (result != null && result.Success)
            {
                row.AcceptChanges();
                saved++;
            }
            else failed.Add($"[{Cur(row, "item_no")}] {result?.Message ?? "저장에 실패했습니다."}");
        }

        if (failed.Count == 0)
        {
            Toast.Show($"{saved}건 저장되었습니다.");
            return;
        }

        AppMessageBox.Show($"{saved}건 저장, {failed.Count}건 실패했습니다. 실패한 행은 수정 상태로 남겨 두었습니다.\n\n"
            + string.Join("\n", failed.Take(10)) + (failed.Count > 10 ? $"\n... 외 {failed.Count - 10}건" : string.Empty), "저장 결과");
    }

    public override Task NewClick() => Task.CompletedTask;
    public override Task DeleteClick() => Task.CompletedTask;
    protected override bool ConfirmDeleteByDefault => false; // 조회전용 - 삭제 기능 없음
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;
}
