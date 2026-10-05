using System.Data;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraSpreadsheet;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.BA;

/// <summary>
/// 품목일괄등록 화면 - grd1 한 줄이 품목 한 건. 엑셀에서 복사해 붙여넣거나(Ctrl+V, 그리드가
/// 자체 지원 - GridViewWynBehavior.PasteFromClipboard) 엑셀 파일을 통째로 업로드해서 여러 품목을
/// 한 번에 입력한 뒤 [검증] -> [저장]으로 등록한다. 붙여넣기/업로드 둘 다 그리드에 "보이는 컬럼
/// 순서" 그대로 값이 매핑되므로, [엑셀양식다운로드]로 받은 파일의 컬럼 순서를 그대로 유지해야
/// 한다(헤더 텍스트로 매핑하지 않음 - 붙여넣기가 원래 순서 기반이라 업로드도 같은 규칙으로
/// 맞춘 것).
///
/// 창고/위치/담당부서/담당자/구매처/품목그룹1~4는 그리드에 "이름"으로 입력받고 [검증]에서
/// 실제 ID로 역매핑한다 - frmItem의 팝업(P_WH/P_LOC/P_DEPT/P_EMP/P_CUST, api/lookups/*)과
/// 품목그룹 콤보(L_ITEM_GRP, api/combo-lookups/*)를 그대로 재사용한다. 둘 다 메뉴의
/// PROC_PREFIX 화이트리스트에 안 걸리는 별도 통로라(sysPopUpM/sysLookupM 메타데이터 기반) 이
/// 화면이 USP_BA_WH_Q 등을 직접 호출할 필요가 없다 - 저장도 새 프로시저 없이 frmItem과 똑같은
/// USP_BA_ITEM_S(N)를 행마다 그대로 호출한다(TplSingleGrid 패턴).
///
/// 단위/발주단위/자산구분/상태(unit_cd/po_unit_cd/asset_type/stat_cd)는 frmItem과 같은
/// MinorCode 콤보(L_CM0001/L_CM0002/L_BA0002)를 쓰는 실제 LookUpColumnEdit 컬럼이다 - 다만
/// "코드 또는 코드명 둘 다 입력 가능"해야 해서(2026-09-16 요청), 드롭다운으로 고르는 것
/// 외에 직접 타이핑하거나 엑셀에서 붙여넣은 값도 OnLookupCellValueChanged가 가로채 코드/
/// 코드명 중 정확히 일치하는 쪽을 찾아 코드로 바꿔 넣는다 - 못 찾으면 빈 칸으로 만든다
/// (오타를 냈다는 걸 사용자가 바로 알아챌 수 있게, "원자재"처럼 틀리게 쓰면 조용히 저장되는
/// 대신 빈칸으로 보인다). 이 즉시 변환은 그리드를 거치는 입력(직접 타이핑, Ctrl+V 붙여넣기)
/// 에만 적용된다 - [엑셀업로드](파일 통째로 읽기)는 DataTable을 직접 조작해서 그리드를 안
/// 거치므로, [검증]에서 같은 방식으로 한 번 더(코드/코드명 매칭 -> 숨은 컬럼 *_resolved에
/// 채움) 안전망을 둔다. out_type은 frmItem에서도 평범한 텍스트 입력(콤보 아님)이라 그대로
/// 둔다.
///
/// 창고/위치/담당부서/담당자/구매처 칸은 팝업 편집기(P_WH/P_LOC/P_DEPT/P_EMP/P_CUST, "..." 버튼/더블클릭/직접 타이핑)를 붙였다(frmItemMod와 같은 방식). 팝업으로 고르면 이름 칸과 ID 칸(wh_id 등)을 같이
/// 채우고, 그 ID는 [검증]이 이름이 그대로일 때(같은 이름이 여러 건이어도) 그대로 쓴다 - 직접 입력/붙여넣기/엑셀업로드한 이름은 지금처럼 [검증]이 이름으로 찾는다. 엑셀 붙여넣기 중에는 팝업이 뜨지 않는다
/// (GridViewWynBehavior). 담당자 팝업은 그 행의 담당부서 소속만 보이고 담당자를 고르면 담당부서도 채운다. 구매처 팝업은 구매(PO) 분류 거래처만(frmItem과 같음).
/// 품목그룹1~4 칸은 콤보(편집할 때만 상위 그룹 아래 하위 그룹만 목록에 보임)이고, 값은 계속 "그룹 이름 텍스트"라 붙여넣기/엑셀/기존 연쇄 확인(ResolveGrpChainForRowAsync)은 그대로다.
///
/// 저장 정책(2026-09-16 확정): 검증 오류가 하나라도 있으면 저장 자체를 막는다(부분저장 없음) -
/// [저장]을 누르면 항상 먼저 전체 재검증부터 한다.
/// </summary>
public partial class frmItemMulti : BaseForm
{
    private static readonly string[] VisibleColumns =
    {
        "item_no", "item_nm", "item_spec", "unit_cd", "po_unit_cd", "safe_qty",
        "wh_nm", "loc_nm", "dept_nm", "emp_nm", "cust_nm",
        "asset_type", "out_type", "stat_cd",
        "po_qc_yn", "prod_qc_yn", "lot_yn", "stock_yn",
        "grp1_nm", "grp2_nm", "grp3_nm", "grp4_nm", "remark"
    };

    private DataTable _list = new();

    // 이름 칸 -> (ID 칸, 팝업으로 고른 시점의 이름 칸). 이름이 그대로면 팝업이 정해 준 ID를 신뢰한다.
    private static readonly string[] PopupPairs = { "wh", "loc", "dept", "emp", "cust" };

    // 품목그룹 1~4단 - 단별 전체 그룹(id, 이름, 상위 id)과 (단, 상위)별로 만든 편집용 콤보.
    private GridColumn[] _grpCols = Array.Empty<GridColumn>();
    private readonly Dictionary<int, List<(string Id, string Nm, string Par)>> _grp = new();
    private readonly Dictionary<(int Lvl, string Par), RepositoryItemComboBox> _grpEditors = new();

    // 단위/자산구분/상태 콤보 목록 캐시 - OnLookupCellValueChanged가 셀이 바뀔 때마다 매번 서버를
    // 다시 부르면 타이핑/붙여넣기 중 화면이 버벅일 수 있어서, 화면 열릴 때 한 번만 받아둔다.
    // ValidateAllAsync는 [검증] 시점 기준 최신값이 필요해서 이 캐시를 안 쓰고 그때 다시 받는다.
    private List<CodeLookupItem> _unitCache = new();
    private List<CodeLookupItem> _assetTypeCache = new();
    private List<CodeLookupItem> _statCdCache = new();

    public frmItemMulti()
    {
        InitializeComponent();

        Text = "품목일괄등록";


        gvw1.Role = GridRoleWyn.Edit;
        gvw1.HighlightFocusedRow = true;
        gvw1.RowAdd += (s, e) => gvw1.AddNewRow();
        gvw1.RowDelete += (s, e) =>
        {
            try { if (gvw1.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };

        // 검증에서 오류가 있던 행은 배경을 옅은 빨강으로 표시해서 눈에 띄게 한다 - validate_result가
        // 비어있으면(아직 검증 전이거나 통과) 기본색 그대로 둔다.
        gvw1.RowCellStyle += (s, e) =>
        {
            if (e.RowHandle < 0) return;
            var value = gvw1.GetRowCellValue(e.RowHandle, "validate_result") as string;
            if (string.IsNullOrEmpty(value)) return;

            e.Appearance.BackColor = System.Drawing.Color.FromArgb(253, 236, 234);
            e.Appearance.Options.UseBackColor = true;
        };

        gvw1.CellValueChanged += OnLookupCellValueChanged;
        _grpCols = new[] { colGrp1Nm, colGrp2Nm, colGrp3Nm, colGrp4Nm };
        gvw1.CustomRowCellEditForEditing += Gvw1_CustomRowCellEditForEditing;
        WirePopupColumns();
        _ = LoadLookupCachesAsync();

        cboAccId.Tag = new BindingFieldTag("acc_id");
        cboAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;

        _list = BuildEmptyTable();
        TrackDirty(_list);
        grd1.DataSource = _list;
    }

    private static DataTable BuildEmptyTable()
    {
        var table = new DataTable();
        foreach (var col in VisibleColumns) table.Columns.Add(col, typeof(string));
        table.Columns.Add("validate_result", typeof(string));
        // 검증에서 역매핑한 ID(저장 시에만 씀, 그리드에는 안 보임).
        foreach (var idCol in new[] { "wh_id", "loc_id", "dept_id", "emp_id", "cust_id", "grp1_id", "grp2_id", "grp3_id", "grp4_id" })
            table.Columns.Add(idCol, typeof(string));
        // 팝업으로 ID를 정한 시점의 이름(wh_pick 등) - 이름이 그대로면 [검증]이 이 ID를 그대로 쓴다.
        foreach (var p in PopupPairs) table.Columns.Add(p + "_pick", typeof(string));
        // [엑셀업로드]는 그리드를 안 거치고 이 DataTable을 직접 채우므로(OnLookupCellValueChanged가
        // 못 잡음), [검증]에서 코드/코드명 매칭 결과를 따로 담아둘 안전망 컬럼(저장에만 씀).
        foreach (var resolvedCol in new[] { "unit_cd_resolved", "po_unit_cd_resolved", "asset_type_resolved", "stat_cd_resolved" })
            table.Columns.Add(resolvedCol, typeof(string));
        return table;
    }

    private async Task LoadLookupCachesAsync()
    {
        _unitCache = await FetchComboItemsAsync("L_CM0001");
        _assetTypeCache = await FetchComboItemsAsync("L_CM0002");
        _statCdCache = await FetchComboItemsAsync("L_BA0002");
        await LoadGroupsAsync();
    }

    // ===== 팝업 컬럼 연결 =====

    private void WirePopupColumns()
    {
        popcolWh.ResultSelected += (h, r) => ApplyPicked(h, r, "wh");
        popcolLoc.ResultSelected += (h, r) => ApplyPicked(h, r, "loc");
        popcolDept.ResultSelected += (h, r) => ApplyPicked(h, r, "dept");
        popcolEmp.ResultSelected += (h, r) =>
        {
            ApplyPicked(h, r, "emp");
            // 담당자를 고르면 사원의 소속 부서도 같이 채운다(P_EMP가 dept_id/dept_nm을 돌려준다)
            if (r.Row != null && r.Row.TryGetValue("dept_id", out var deptId) && !string.IsNullOrEmpty(deptId))
            {
                r.Row.TryGetValue("dept_nm", out var deptNm);
                SetPicked(h, "dept", deptId, deptNm ?? string.Empty);
            }
        };
        // 담당자 팝업은 그 행에 입력된 담당부서 소속 인원만 보인다(P_EMP의 부서명 조건 p_dept_nm). 부서가 비어 있으면 전체.
        popcolEmp.ConditionProvider = () =>
        {
            var dept = Convert.ToString(gvw1.GetRowCellValue(gvw1.FocusedRowHandle, colDeptNm))?.Trim();
            return string.IsNullOrEmpty(dept) ? new Dictionary<string, string?>() : new Dictionary<string, string?> { ["p_dept_nm"] = dept };
        };
        popcolCust.ResultSelected += (h, r) => ApplyPicked(h, r, "cust");
    }

    /// <summary>팝업에서 고른 결과를 이름 칸/ID 칸/pick 칸에 함께 넣는다(이름은 그 팝업이 돌려준 "{x}_nm" 컬럼, 없으면 표시값).</summary>
    private void ApplyPicked(int rowHandle, PopupLookupResult result, string key)
    {
        string? name = null;
        if (result.Row != null)
            foreach (var kv in result.Row)
                if (string.Equals(kv.Key, key + "_nm", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(kv.Value)) { name = kv.Value; break; }
        SetPicked(rowHandle, key, result.Code, name ?? result.Display ?? string.Empty);
    }

    private void SetPicked(int rowHandle, string key, string id, string name)
    {
        gvw1.SetRowCellValue(rowHandle, key + "_nm", name);
        if (gvw1.GetDataRow(rowHandle) is DataRow row) { row[key + "_id"] = id; row[key + "_pick"] = name; }
    }

    // ===== 품목그룹 콤보 =====

    /// <summary>1~4단 품목그룹 전체를 한 번 받아 둔다(L_ITEM_GRP, 상위 조건 없이 단별 전체 - 각 항목의 par_grp_id가 상위 그룹).</summary>
    private async Task LoadGroupsAsync()
    {
        if (ComboLookupProvider.Fetch == null) return;
        try
        {
            for (var lvl = 1; lvl <= 4; lvl++)
            {
                var result = await ComboLookupProvider.Fetch("L_ITEM_GRP", new Dictionary<string, string?> { ["p_grp_lvl"] = lvl.ToString(), ["p_par_grp_id"] = string.Empty });
                _grp[lvl] = result.Items.Select(i => (
                    i.Value, i.Display?.Trim() ?? string.Empty,
                    i.Row != null && i.Row.TryGetValue("par_grp_id", out var par) ? par ?? string.Empty : string.Empty)).ToList();
            }
            _grpEditors.Clear();
        }
        catch { /* 목록을 못 받으면 콤보 없이 텍스트 입력으로 둔다 - [검증]이 최종 확인 */ }
    }

    /// <summary>이 행에서 lvl단의 상위 그룹 id - 1단부터 이름으로 내려오며 찾는다(받아 둔 목록 기준). 못 찾으면 null.</summary>
    private string? ParentGroupId(DataRow row, int lvl)
    {
        string? par = null;
        for (var i = 1; i < lvl; i++)
        {
            var nm = Str(row, $"grp{i}_nm");
            if (nm.Length == 0 || !_grp.TryGetValue(i, out var list)) return null;
            var hit = list.FirstOrDefault(g => (i == 1 || g.Par == par) && string.Equals(g.Nm, nm, StringComparison.OrdinalIgnoreCase));
            if (hit.Id == null) return null;
            par = hit.Id;
        }
        return par;
    }

    /// <summary>편집용 콤보 - 1단은 전체, 2단 이상은 상위 그룹에 속한 것만(상위가 비면 빈 목록). 값은 그룹 이름 텍스트 그대로.</summary>
    private RepositoryItemComboBox GetGrpEditor(int lvl, string? parentId)
    {
        var key = (lvl, lvl == 1 ? string.Empty : parentId ?? string.Empty);
        if (_grpEditors.TryGetValue(key, out var editor)) return editor;

        editor = new RepositoryItemComboBox { TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard, NullText = string.Empty };
        editor.Items.Add(string.Empty);
        if (_grp.TryGetValue(lvl, out var all))
            foreach (var g in all.Where(g => lvl == 1 || (key.Item2.Length > 0 && g.Par == key.Item2)))
                editor.Items.Add(g.Nm);
        grd1.RepositoryItems.Add(editor);
        _grpEditors[key] = editor;
        return editor;
    }

    private void Gvw1_CustomRowCellEditForEditing(object? sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
    {
        var idx = Array.IndexOf(_grpCols, e.Column);
        if (idx < 0 || _grp.Count == 0) return;
        if (gvw1.GetDataRow(e.RowHandle) is not DataRow row) return;
        e.RepositoryItem = GetGrpEditor(idx + 1, idx == 0 ? null : ParentGroupId(row, idx + 1));
    }

    /// <summary>단위/발주단위/자산구분/상태 컬럼(LookUpColumnEdit) 값이 바뀔 때마다 코드/코드명
    /// 중 정확히 일치하는 쪽을 찾아 코드로 바꿔 넣는다 - 드롭다운으로 골랐을 때(이미 코드라 그대로
    /// 매치)뿐 아니라, 직접 타이핑하거나 엑셀에서 붙여넣었을 때(Ctrl+V, GridViewWynBehavior.
    /// PasteFromClipboard가 SetRowCellValue로 값을 그대로 꽂아 넣어서 LookUp 에디터의 이름-코드
    /// 변환을 거치지 않음)도 이 이벤트가 그대로 잡아서 처리한다. 못 찾으면 빈 칸으로 만든다 -
    /// 오타를 냈다는 걸 사용자가 바로 알아챌 수 있게 하기 위함(2026-09-16 요청). 캐시가 아직
    /// 안 실렸으면(화면을 막 열자마자 타이핑한 경우) 조용히 넘어간다 - [검증]이 최종 안전망이다.</summary>
    private void OnLookupCellValueChanged(object? sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
    {
        if (e.Column.FieldName is "grp1_nm" or "grp2_nm" or "grp3_nm" or "grp4_nm")
        {
            _ = ResolveGrpChainForRowAsync(e.RowHandle);
            return;
        }

        var items = e.Column.FieldName switch
        {
            "unit_cd" or "po_unit_cd" => _unitCache,
            "asset_type" => _assetTypeCache,
            "stat_cd" => _statCdCache,
            _ => null
        };
        if (items == null || items.Count == 0) return;

        var text = Convert.ToString(e.Value)?.Trim() ?? string.Empty;
        if (text.Length == 0) return;

        var match = items.FirstOrDefault(it =>
            string.Equals(it.Value, text, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(it.Display?.Trim(), text, StringComparison.OrdinalIgnoreCase));

        var resolved = match?.Value ?? string.Empty;
        if (string.Equals(resolved, text, StringComparison.OrdinalIgnoreCase)) return; // 이미 올바른 코드 - 재귀 방지 겸 불필요한 재대입 생략

        gvw1.SetRowCellValue(e.RowHandle, e.Column, resolved);
    }

    /// <summary>품목그룹1~4(grp1_nm~grp4_nm)도 unit_cd 등과 같은 "그리드 자체가 룩업이라 즉시
    /// 확인된다" 피드백을 적용한다(2026-09-16 요청) - 다만 상위->하위 연쇄 구조(L_ITEM_GRP,
    /// 상위그룹에 따라 하위 후보가 달라짐)라 고정 목록인 unit_cd처럼 LookUpColumnEdit 드롭다운은
    /// 못 쓴다. 대신 이 행의 grp1~4_nm 중 하나라도 바뀔 때마다 1단계부터 다시 상위->하위 순서로
    /// 재확인해서, 못 찾거나(오타) 상위 단계가 비어 연쇄가 끊긴 단계부터는 화면 텍스트 자체를
    /// 빈칸으로 되돌린다 - ResolveGrpChainAsync(검증 시점 안전망)와 같은 매칭 규칙(정확히 일치만
    /// 인정)이지만, 여기는 오타를 바로 알아챌 수 있게 즉시 반영한다는 점이 다르다.
    ///
    /// SetRowCellValue로 빈칸을 써넣으면 이 메서드가 다시 재귀 호출되지만(CellValueChanged가 또
    /// 발생), 이미 빈칸인 필드는 다시 안 건드리므로(길이 0이면 SetRowCellValue를 안 부름)
    /// 자연히 멈춘다. 붙여넣기처럼 grp1~4가 한꺼번에 바뀌는 경우 레벨마다 독립적으로 이 메서드가
    /// 호출되어 일시적으로 겹쳐 돌 수 있지만, 매번 그 시점의 실제 셀 값을 기준으로 처음부터
    /// 다시 확인하는 방식이라(캐시된 이전 상태에 의존하지 않음) 결국 올바른 값으로 수렴한다 -
    /// 저장 전 [검증]이 항상 마지막 안전망이다.</summary>
    private async Task ResolveGrpChainForRowAsync(int rowHandle)
    {
        if (gvw1.GetDataRow(rowHandle) is not DataRow row) return;
        if (ComboLookupProvider.Fetch == null) return;

        var nmFields = new[] { "grp1_nm", "grp2_nm", "grp3_nm", "grp4_nm" };
        var idFields = new[] { "grp1_id", "grp2_id", "grp3_id", "grp4_id" };
        var parGrpId = "0";
        var chainBroken = false;

        for (var i = 0; i < nmFields.Length; i++)
        {
            if (row.RowState is DataRowState.Detached or DataRowState.Deleted) return; // 그새 행이 삭제됐으면 중단

            var nm = Str(row, nmFields[i]);
            if (nm.Length == 0)
            {
                row[idFields[i]] = string.Empty;
                chainBroken = true;
                continue;
            }

            if (chainBroken)
            {
                gvw1.SetRowCellValue(rowHandle, gvw1.Columns[nmFields[i]], string.Empty);
                row[idFields[i]] = string.Empty;
                continue;
            }

            ComboLookupResult result;
            try
            {
                result = await ComboLookupProvider.Fetch("L_ITEM_GRP", new Dictionary<string, string?>
                {
                    ["p_grp_lvl"] = (i + 1).ToString(),
                    ["p_par_grp_id"] = parGrpId
                });
            }
            catch
            {
                return; // 조회 자체가 실패하면 화면 텍스트는 건드리지 않는다 - [검증]이 최종 안전망이다.
            }

            var matches = result.Items.Where(it => string.Equals(it.Display?.Trim(), nm, StringComparison.OrdinalIgnoreCase)).ToList();
            if (matches.Count == 1)
            {
                row[idFields[i]] = matches[0].Value;
                parGrpId = matches[0].Value;
            }
            else
            {
                gvw1.SetRowCellValue(rowHandle, gvw1.Columns[nmFields[i]], string.Empty);
                row[idFields[i]] = string.Empty;
                chainBroken = true;
            }
        }
    }

    // 이 화면은 "조회"할 기존 목록이 없다(일괄 입력 전용) - 툴바 조회 버튼은 아무 일도 안 한다.
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

    // 이 화면엔 "선택된 마스터 한 건 삭제" 개념이 없다 - 포커스된 그리드 행 삭제로 그대로 연결한다.
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

    private void btnDownloadTemplate_Click(object sender, EventArgs e) => DownloadTemplate();
    private void btnUploadExcel_Click(object sender, EventArgs e) => UploadExcel();
    private async void btnValidate_Click(object sender, EventArgs e)
    {
        var (ok, errorCount) = await ValidateAllAsync();
        grd1.RefreshDataSource();
        Toast.Show(ok ? "검증을 통과했습니다." : $"{errorCount}건의 행에서 오류가 발견되었습니다 - 빨간색 행을 확인해주세요.");
    }

    /// <summary>빈 양식(헤더만, 데이터 0행)을 엑셀로 내려받는다 - DevExpress GridView.ExportToXlsx가
    /// 행이 0개여도 컬럼 헤더는 그대로 내보내므로, 지금 그리드를 건드리지 않고 컬럼 구조만 같은
    /// 임시 DataTable로 잠깐 바꿔치기했다가 내보낸 뒤 원래대로 되돌린다.</summary>
    private void DownloadTemplate()
    {
        using var dlg = new SaveFileDialog
        {
            Filter = "Excel (*.xlsx)|*.xlsx",
            FileName = $"품목일괄등록_양식_{DateTime.Now:yyyyMMdd}.xlsx"
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        var originalSource = grd1.DataSource;
        try
        {
            grd1.DataSource = _list.Clone();
            gvw1.ExportToXlsx(dlg.FileName);
        }
        finally
        {
            grd1.DataSource = originalSource;
        }

        try { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(dlg.FileName) { UseShellExecute = true }); }
        catch { /* 저장 자체는 끝났으니 여는 것만 실패해도 무시 */ }
    }

    /// <summary>엑셀 파일을 통째로 읽어 그리드 끝에 행으로 추가한다. 첫 행은 헤더로 보고
    /// 건너뛰며, 그 다음 행부터 그리드에 "보이는 컬럼 순서" 그대로 값을 채운다(엑셀양식다운로드로
    /// 받은 파일과 컬럼 순서가 같다는 전제 - 헤더 텍스트로 찾지 않는다). 새 NuGet 패키지 없이
    /// 이미 로컬에 설치된 DevExpress.Spreadsheet(Office File API)로 직접 읽는다.</summary>
    private void UploadExcel()
    {
        using var dlg = new OpenFileDialog { Filter = "Excel (*.xlsx)|*.xlsx" };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        // 화면에 안 띄우는 SpreadsheetControl로 .xlsx를 읽는다 - 순수 비UI Workbook 클래스는
        // 이 PC에 설치된 DevExpress.Spreadsheet.v21.2.Core.dll에 없어서(리플렉션으로 확인,
        // IWorkbook 인터페이스만 있고 구현체가 internal) 대신 이 방법을 쓴다.
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

        var sheet = spreadsheet.Document.Worksheets.First(); // WorksheetCollection엔 정수 인덱서가 없다(이름 문자열만) - IEnumerable로 첫 시트를 얻는다.
        var used = sheet.GetUsedRange();
        var added = 0;

        for (var r = used.TopRowIndex + 1; r <= used.BottomRowIndex; r++) // 1행(헤더)은 건너뜀
        {
            var row = _list.NewRow();
            var hasAny = false;

            for (var i = 0; i < VisibleColumns.Length; i++)
            {
                var c = used.LeftColumnIndex + i;
                if (c > used.RightColumnIndex) break;

                var text = sheet[r, c].DisplayText?.Trim() ?? string.Empty;
                if (text.Length > 0) hasAny = true;
                row[VisibleColumns[i]] = text;
            }

            if (!hasAny) continue; // 완전히 빈 줄은 건너뜀
            _list.Rows.Add(row);
            added++;
        }

        grd1.RefreshDataSource();
        Toast.Show($"{added}건 불러왔습니다. [검증]을 눌러 확인해주세요.");
    }

    /// <summary>그리드 전체 행을 검증한다 - 결과는 각 행의 validate_result(그리드 표시)와
    /// wh_id/loc_id/... (저장에 쓸 역매핑된 ID, 숨김)에 반영된다. 반환값은 (전체 통과 여부,
    /// 오류 있는 행 수).</summary>
    private async Task<(bool Ok, int ErrorCount)> ValidateAllAsync()
    {
        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        var rows = _list.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted).ToList();
        var itemNos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // 단위/자산구분/상태 콤보는 행마다 다시 조회할 이유가 없는 고정 목록이라(품목그룹처럼
        // 상위그룹에 따라 달라지지 않음) 반복문 시작 전에 한 번만 받아와 재사용한다.
        var unitItems = await FetchComboItemsAsync("L_CM0001");
        var assetTypeItems = await FetchComboItemsAsync("L_CM0002");
        var statCdItems = await FetchComboItemsAsync("L_BA0002");

        foreach (var row in rows)
        {
            var errors = new List<string>();

            var itemNo = Str(row, "item_no");
            var itemNm = Str(row, "item_nm");
            var isBlankRow = rows.Count > 0 && VisibleColumns.All(c => Str(row, c).Length == 0);
            if (isBlankRow)
            {
                row["validate_result"] = string.Empty;
                continue; // 완전히 빈 줄(붙여넣기로 남은 자리 등)은 검증 대상에서 뺀다.
            }

            if (itemNo.Length == 0) errors.Add("품목코드 필수");
            if (itemNm.Length == 0) errors.Add("품목명 필수");
            if (Str(row, "unit_cd").Length == 0) errors.Add("재고단위 필수");
            if (Str(row, "asset_type").Length == 0) errors.Add("자산구분 필수");
            if (itemNo.Length > 0 && !itemNos.Add(itemNo)) errors.Add("품목코드 중복(입력한 행 안에서)");

            row["wh_id"] = await ResolvePopupIdAsync("P_WH", row, "wh_nm", "창고", errors);
            row["loc_id"] = await ResolvePopupIdAsync("P_LOC", row, "loc_nm", "위치", errors);
            row["dept_id"] = await ResolvePopupIdAsync("P_DEPT", row, "dept_nm", "담당부서", errors);
            row["emp_id"] = await ResolvePopupIdAsync("P_EMP", row, "emp_nm", "담당자", errors);
            row["cust_id"] = await ResolvePopupIdAsync("P_CUST", row, "cust_nm", "구매처", errors);
            await ResolveGrpChainAsync(row, errors);

            row["unit_cd_resolved"] = ResolveCodeOrName(unitItems, row, "unit_cd", "단위", errors);
            row["po_unit_cd_resolved"] = ResolveCodeOrName(unitItems, row, "po_unit_cd", "발주단위", errors);
            row["asset_type_resolved"] = ResolveCodeOrName(assetTypeItems, row, "asset_type", "자산구분", errors);
            row["stat_cd_resolved"] = ResolveCodeOrName(statCdItems, row, "stat_cd", "상태", errors);

            row["validate_result"] = errors.Count == 0 ? string.Empty : string.Join("; ", errors);
        }

        // 품목코드 DB 기존 중복 확인 - 그리드 안에서는 유일했더라도 이미 등록된 코드일 수 있다.
        var candidateNos = rows.Select(r => Str(r, "item_no")).Where(s => s.Length > 0).Distinct().ToList();
        if (candidateNos.Count > 0)
        {
            var existing = await QueryAsync("USP_BA_ITEM_Q", new
            {
                p_work_type = "Q2",
                p_item_no_list = string.Join(",", candidateNos)
            });
            var existingNos = new HashSet<string>(
                existing.Rows.Cast<DataRow>().Select(r => r["item_no"]?.ToString() ?? string.Empty),
                StringComparer.OrdinalIgnoreCase);

            foreach (var row in rows)
            {
                var itemNo = Str(row, "item_no");
                if (itemNo.Length == 0 || !existingNos.Contains(itemNo)) continue;

                var current = row["validate_result"]?.ToString() ?? string.Empty;
                row["validate_result"] = string.IsNullOrEmpty(current)
                    ? "품목코드 중복(이미 등록됨)"
                    : $"{current}; 품목코드 중복(이미 등록됨)";
            }
        }

        var errorRows = rows.Count(r => !string.IsNullOrEmpty(r["validate_result"]?.ToString()));
        return (errorRows == 0, errorRows);
    }

    /// <summary>팝업(P_WH/P_LOC/P_DEPT/P_EMP/P_CUST) 프레임워크로 이름 -> ID를 역매핑한다.
    /// PopupLookupEditWyn.OnLeaveAsync가 이미 쓰는 것과 같은 방식(LIKE 검색 후 정확히 일치하는
    ///것만 채택) - 값이 비어있으면 그냥 null(선택 안 함), 0개/2개 이상 일치하면 오류로 남긴다.</summary>
    private static async Task<string?> ResolvePopupIdAsync(string popupKey, DataRow row, string nmField, string label, List<string> errors)
    {
        var nm = Str(row, nmField);
        if (nm.Length == 0) return null;

        // 팝업으로 고른 뒤 이름이 그대로면 그 ID를 그대로 쓴다(같은 이름이 여러 건이어도 모호하지 않게).
        var key = nmField.Substring(0, nmField.Length - 3);
        var pickedId = Str(row, key + "_id");
        if (pickedId.Length > 0 && Str(row, key + "_pick") == nm) return pickedId;

        if (PopupLookupProvider.SearchExact == null)
        {
            errors.Add($"{label} 조회 기능을 사용할 수 없습니다.");
            return null;
        }

        List<PopupLookupResult> candidates;
        try
        {
            candidates = await PopupLookupProvider.SearchExact(popupKey, nm);
        }
        catch
        {
            errors.Add($"{label} 조회 중 오류가 발생했습니다.");
            return null;
        }

        var matches = candidates.Where(c => string.Equals(c.Display?.Trim(), nm, StringComparison.OrdinalIgnoreCase)).ToList();
        if (matches.Count == 0)
        {
            errors.Add($"{label} '{nm}' 없음");
            return null;
        }
        if (matches.Count > 1)
        {
            errors.Add($"{label} '{nm}' 여러 건 일치(모호함)");
            return null;
        }

        row[key + "_pick"] = nm; // 이 이름으로 정한 ID - 다음 검증에서 다시 찾지 않는다(이름이 바뀌면 pick이 달라져 다시 찾는다)
        return matches[0].Code;
    }

    /// <summary>품목그룹1~4는 상위->하위 순서로만 지정할 수 있다(상위 없이 하위만 지정 불가) -
    /// frmItem의 cboDetailGrp1~4Id 연쇄 LookUp(L_ITEM_GRP)과 똑같은 조회를 그대로 재사용한다.</summary>
    private static async Task ResolveGrpChainAsync(DataRow row, List<string> errors)
    {
        var nmFields = new[] { "grp1_nm", "grp2_nm", "grp3_nm", "grp4_nm" };
        var idFields = new[] { "grp1_id", "grp2_id", "grp3_id", "grp4_id" };
        var parGrpId = "0";
        var chainBroken = false;

        for (var i = 0; i < nmFields.Length; i++)
        {
            var nm = Str(row, nmFields[i]);
            if (nm.Length == 0)
            {
                row[idFields[i]] = string.Empty;
                chainBroken = true;
                continue;
            }

            if (chainBroken)
            {
                errors.Add($"품목그룹{i + 1}은 상위 품목그룹 없이 지정할 수 없음");
                row[idFields[i]] = string.Empty;
                continue;
            }

            if (ComboLookupProvider.Fetch == null)
            {
                errors.Add("품목그룹 조회 기능을 사용할 수 없습니다.");
                return;
            }

            var result = await ComboLookupProvider.Fetch("L_ITEM_GRP", new Dictionary<string, string?>
            {
                ["p_grp_lvl"] = (i + 1).ToString(),
                ["p_par_grp_id"] = parGrpId
            });

            var matches = result.Items.Where(it => string.Equals(it.Display?.Trim(), nm, StringComparison.OrdinalIgnoreCase)).ToList();
            if (matches.Count == 0)
            {
                errors.Add($"품목그룹{i + 1} '{nm}' 없음");
                row[idFields[i]] = string.Empty;
                chainBroken = true;
            }
            else if (matches.Count > 1)
            {
                errors.Add($"품목그룹{i + 1} '{nm}' 여러 건 일치(모호함)");
                row[idFields[i]] = string.Empty;
                chainBroken = true;
            }
            else
            {
                row[idFields[i]] = matches[0].Value;
                parGrpId = matches[0].Value;
            }
        }
    }

    /// <summary>단위/자산구분/상태처럼 상위그룹 없이 고정된 콤보 목록을 가져온다(L_ITEM_GRP과
    /// 달리 p_grp_lvl/p_par_grp_id 같은 파라미터가 필요 없는 단순 목록) - 행마다 다시 조회할
    /// 이유가 없어서 ValidateAllAsync가 반복문 시작 전에 딱 한 번만 부른다. 조회 실패해도
    /// 화면이 죽으면 안 되므로 빈 목록으로 대체한다 - 그러면 ResolveCodeOrName이 자연히
    /// "없음" 오류를 내게 된다(별도 예외 처리 불필요).</summary>
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

    /// <summary>코드("M") 또는 코드명("원재료") 어느 쪽으로 입력해도 정확히 일치하는 코드값을
    /// 찾아 돌려준다(대소문자 무시) - 값이 비어있으면 null(선택 안 함), 못 찾으면 오류로
    /// 남긴다. ResolvePopupIdAsync/ResolveGrpChainAsync와 같은 규칙(정확히 일치하는 것만
    /// 채택 - 부분일치/모호함은 지원하지 않는다).</summary>
    private static string? ResolveCodeOrName(List<CodeLookupItem> items, DataRow row, string columnName, string label, List<string> errors)
    {
        var text = Str(row, columnName);
        if (text.Length == 0) return null;

        var match = items.FirstOrDefault(it =>
            string.Equals(it.Value, text, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(it.Display?.Trim(), text, StringComparison.OrdinalIgnoreCase));

        if (match == null)
        {
            errors.Add($"{label} '{text}' 없음");
            return null;
        }

        return match.Value;
    }

    public override async Task SaveClick()
    {
        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        var rows = _list.Rows.Cast<DataRow>()
            .Where(r => r.RowState != DataRowState.Deleted && VisibleColumns.Any(c => Str(r, c).Length > 0))
            .ToList();
        if (rows.Count == 0)
        {
            AppMessageBox.Show("등록할 품목이 없습니다.", "안내");
            return;
        }

        var (ok, errorCount) = await ValidateAllAsync();
        grd1.RefreshDataSource();
        if (!ok)
        {
            AppMessageBox.Show($"검증 오류가 있는 행이 {errorCount}건 있어 저장할 수 없습니다.\n빨간색 행의 검증결과를 확인해주세요.", "저장 불가");
            return;
        }

        var accId = cboAccId.EditValue?.ToString();
        if (string.IsNullOrEmpty(accId))
        {
            AppMessageBox.Show("사업장을 먼저 선택해주세요.", "확인");
            return;
        }

        var saved = 0;
        foreach (var row in rows)
        {
            var result = await SaveAsync("USP_BA_ITEM_S", new Dictionary<string, string?>
            {
                ["p_work_type"] = "N",
                ["p_acc_id"] = accId,
                ["p_item_no"] = Str(row, "item_no"),
                ["p_item_nm"] = Str(row, "item_nm"),
                ["p_item_spec"] = Str(row, "item_spec"),
                ["p_unit_cd"] = Str(row, "unit_cd"),
                ["p_po_unit_cd"] = Str(row, "po_unit_cd"),
                ["p_wh_id"] = NullIfEmpty(row, "wh_id"),
                ["p_loc_id"] = NullIfEmpty(row, "loc_id"),
                ["p_safe_qty"] = NullIfEmpty(row, "safe_qty"),
                ["p_dept_id"] = NullIfEmpty(row, "dept_id"),
                ["p_emp_id"] = NullIfEmpty(row, "emp_id"),
                ["p_cust_id"] = NullIfEmpty(row, "cust_id"),
                ["p_asset_type"] = Str(row, "asset_type"),
                ["p_out_type"] = Str(row, "out_type"),
                ["p_po_qc_yn"] = YOrN(row, "po_qc_yn"),
                ["p_prod_qc_yn"] = YOrN(row, "prod_qc_yn"),
                ["p_lot_yn"] = YOrN(row, "lot_yn"),
                ["p_stock_yn"] = YOrN(row, "stock_yn"),
                ["p_stat_cd"] = Str(row, "stat_cd"),
                ["p_grp1_id"] = NullIfEmpty(row, "grp1_id"),
                ["p_grp2_id"] = NullIfEmpty(row, "grp2_id"),
                ["p_grp3_id"] = NullIfEmpty(row, "grp3_id"),
                ["p_grp4_id"] = NullIfEmpty(row, "grp4_id"),
                ["p_remark"] = Str(row, "remark"),
            });

            if (result == null || !result.Success)
            {
                AppMessageBox.Show(
                    $"[{Str(row, "item_no")}] {FormatSaveFailMessage(result)}\n\n앞서 {saved}건은 이미 등록되었습니다.",
                    "저장 실패");
                return;
            }
            saved++;
        }

        _list.Rows.Clear();
        grd1.RefreshDataSource();
        Toast.Show($"{saved}건 등록되었습니다.");
    }

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

    private static string FormatSaveFailMessage(ApiResult? result)
    {
        var message = result?.Message ?? "저장에 실패했습니다.";
        return result is { ErrorCode: not 0 } ? $"{message} (오류코드: {result.ErrorCode})" : message;
    }
}
