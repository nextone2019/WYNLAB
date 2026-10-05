using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

/// <summary>
/// 라우팅관리 - 기초코드등록과 같은 구조다. 좌측 grd1에 라우팅 목록(TPRROUTEM), 우측에 선택한 라우팅의 헤더 입력(panData)과 공정 체인 그리드
/// grd2(TPRROUTED)가 있고, 조회(목록)와 저장(우측 편집 내용)을 한 화면에서 같이 한다. 조회 조건이 비어 있으면 전체 목록이고, 목록에서 행을 고르면 우측이
/// 그 라우팅으로 바뀐다(저장 안 된 변경이 있으면 먼저 확인 - BaseForm.ConfirmMasterRowSwitch). 신규는 툴바 신규로 우측을 비우고 입력한다.
///
/// 공정 하나는 "산출품목"을 정하면 투입품목이 그 품목 BOM(BOM관리)의 주원료로 자동 결정된다(서버가 채우고 검증). 앞 공정의 산출품목이 다음 공정의 투입품목이어야 하므로
/// 저장할 때 끊긴 곳이 있으면 알려준다(막지는 않는다). 우측 상단 "적용 제품" 그리드(TPRITEMROUTE)로 이 라우팅을 쓸 제품을 연결하고 제품마다 기본 라우팅 1개를 지정한다 -
/// 작업지시는 제품을 고르면 그 제품에 연결된 라우팅만 고를 수 있다. 라우팅을 고쳐도 이미 낸 작업지시에는 영향이 없다(작업지시가 공정행을 복사해서 갖는다).
/// 공정은 공정마스터(L_PRPROC)에서 고른다. 작업지시에서 쓴 라우팅은 삭제 대신 사용여부를 끈다. 라우팅코드는 한 번 저장하면 바꿀 수 없다.
/// </summary>
public partial class frmRoute : BaseForm
{
    private DataTable _list = new();
    private DataTable _detail = new();
    private DataTable _items = new();       // 적용 제품(TPRITEMROUTE)
    private DataTable _itemCache = new();
    private string? _editingKey; // 선택한(저장된) 라우팅의 route_id. null이면 신규모드
    private int _loadSeq;        // 목록 행을 빠르게 넘길 때 늦게 온 응답이 최신 선택을 덮어쓰지 않게 하는 번호

    public frmRoute()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준(2026-10-03): 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "라우팅관리";

        Controls.Add(BuildScreenHeader());

        // grd1(라우팅 목록)은 조회전용
        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        gvw2.Role = GridRoleWyn.Edit;
        gvw2.HighlightFocusedRow = true;
        gvw2.RowAdd += (s, e) => AddRow();
        gvw2.RowDelete += (s, e) => DeleteRow();
        gvw2.InitNewRow += Gvw2_InitNewRow;
        gvw2.CellValueChanged += Gvw2_CellValueChanged;

        btnAddRow1.Click += (s, e) => AddRow();
        btnDeletRow1.Click += (s, e) => DeleteRow();

        gvw3.Role = GridRoleWyn.Edit;
        gvw3.HighlightFocusedRow = true;
        gvw3.RowAdd += (s, e) => AddItemRow();
        gvw3.RowDelete += (s, e) => DeleteItemRow();
        btnAddItem.Click += (s, e) => AddItemRow();
        btnDelItem.Click += (s, e) => DeleteItemRow();

        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) =>
        {
            _itemCache = await LoadPopupRowsAsync("P_ITEM");
            await SafeExecuteAsync(EnsureSchemaAsync, "라우팅 화면 초기화");
            await SafeExecuteAsync(QueryClick, "라우팅 목록 조회");
        };
    }

    private static decimal Dec(object? value) => decimal.TryParse(value?.ToString(), out var d) ? d : 0;

    private void AddRow() => gvw2.AddNewRow();

    private void AddItemRow() => gvw3.AddNewRow();

    private void DeleteItemRow()
    {
        gvw3.CloseEditor();
        gvw3.UpdateCurrentRow();
        try { if (gvw3.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
        catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
    }

    private void DeleteRow()
    {
        // 방금 AddNewRow로 만든 행에 포커스가 있으면 아직 "새 행 편집 중"이라 삭제가 조용히 무시된다 - 먼저 편집 중인 셀을 확정한다.
        gvw2.CloseEditor();
        gvw2.UpdateCurrentRow();
        try { if (gvw2.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
        catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
    }

    /// <summary>새 화면은 아직 조회한 적이 없어 공정 그리드 테이블에 컬럼이 없다 - 존재하지 않는 라우팅으로 스키마만 먼저 받아 둔다.</summary>
    private async Task EnsureSchemaAsync()
    {
        if (_detail.Columns.Count > 0) return;

        var tables = await QueryMultiAsync("USP_PR_ROUTE_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q", ["p_route_id"] = "-1" });
        if (tables.Count > 1) _detail = tables[1];
        if (tables.Count > 2) _items = tables[2];
        EnterNewMode();
    }

    /// <summary>새 공정 행의 순번은 지금 있는 마지막 순번 + 1.</summary>
    private void Gvw2_InitNewRow(object? sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
    {
        var max = 0;
        foreach (DataRow r in _detail.Rows)
            if (r.RowState != DataRowState.Deleted && int.TryParse(r["serl"]?.ToString(), out var s) && s > max) max = s;
        gvw2.SetRowCellValue(e.RowHandle, colSerl, max + 1);
    }

    private DataRow? FindItem(object? value)
    {
        if (!long.TryParse(value?.ToString(), out var id)) return null;
        foreach (DataRow r in _itemCache.Rows)
            if (long.TryParse(r["item_id"]?.ToString(), out var rid) && rid == id) return r;
        return null;
    }

    private bool _syncing;

    private void Gvw2_CellValueChanged(object? sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
    {
        if (_syncing) return;

        if (e.Value is string text && (e.Column == colSplitQty || e.Column == colPrice))
        {
            gvw2.SetRowCellValue(e.RowHandle, e.Column,
                decimal.TryParse(text.Replace(",", ""), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var n)
                    ? n : (object)DBNull.Value);
            return;
        }

        // 품목을 고르면 그 품목의 재고단위를 투입/산출 단위로 채운다(산출 단위는 정산단위 기본값이기도 하다)
        if (e.Column == colInItemId || e.Column == colOutItemId)
        {
            var unit = FindItem(e.Value)?["unit_cd"]?.ToString();
            if (string.IsNullOrEmpty(unit)) return;
            _syncing = true;
            try
            {
                if (e.Column == colInItemId) gvw2.SetRowCellValue(e.RowHandle, colInUnit, unit);
                else
                {
                    gvw2.SetRowCellValue(e.RowHandle, colOutUnit, unit);
                    if (string.IsNullOrEmpty(gvw2.GetRowCellValue(e.RowHandle, colPriceUnit)?.ToString()))
                        gvw2.SetRowCellValue(e.RowHandle, colPriceUnit, unit);
                }
            }
            finally { _syncing = false; }
        }
    }

    // ===== 조회(좌측 목록) =====

    // 사용자가 조회 버튼을 누른 경우(preserveSelection: false, 0번 행부터 새로 시작)와 저장/삭제 뒤 내부 재조회(true, 방금 편집하던 행 유지)는
    // 다른 동작이어야 한다(기초코드등록과 같은 규칙, [[feedback_query_refocus_after_save]]).
    public override async Task QueryClick() => await QueryCore(preserveSelection: false);

    private async Task QueryCore(bool preserveSelection)
    {
        // 라우팅코드/명 하나로 같이 검색한다. 비우면 전체.
        _list = await QueryAsync("USP_PR_ROUTE_Q", new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "L",
            ["p_keyword"] = txtSearchRouteCd.Text.Trim(),
            ["p_acc_id"] = Session.AccId?.ToString(),
        });

        var editingKey = preserveSelection ? _editingKey : null;

        if (editingKey == null)
        {
            // 편집 중이던 행이 없으면 DevExpress 기본 동작(재바인딩 시 자동으로 첫 행에 포커스 -> FocusedRowObjectChanged -> 우측 채움)에 맡긴다.
            grd1.DataSource = _list;
            if (_list.Rows.Count == 0) EnterNewMode();
            return;
        }

        var targetRow = FindListRow(editingKey);

        // 편집 중이던 행에 포커스를 정확히 복원한다. 재바인딩이 먼저 0번 행에 포커스를 주면서 우측이 엉뚱한 라우팅으로 잠깐 바뀌는 것을 막으려고
        // 재바인딩 + 포커스 복원이 끝날 때까지 이벤트와 행 전환 확인(ConfirmMasterRowSwitch)을 눌러 두고, 최종 한 행에 대해서만 아래에서 우측을 채운다.
        gvw1.FocusedRowObjectChanged -= Gvw1_FocusedRowObjectChanged;
        SuppressMasterRowSwitchConfirm(gvw1, () =>
        {
            grd1.DataSource = _list;
            if (targetRow != null)
            {
                var handle = gvw1.GetRowHandle(_list.Rows.IndexOf(targetRow));
                if (handle >= 0) gvw1.FocusedRowHandle = handle;
            }
        });
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        if (targetRow != null) await LoadRouteAsync(editingKey);
        else EnterNewMode();
    }

    private DataRow? FindListRow(string routeId) =>
        _list.Rows.Cast<DataRow>().FirstOrDefault(r => string.Equals(r["route_id"]?.ToString(), routeId, StringComparison.OrdinalIgnoreCase));

    /// <summary>목록에서 다른 행을 고를 때 우측 편집 내용에 저장 안 된 변경이 있으면 먼저 확인한다(BaseForm.ConfirmMasterRowSwitch).</summary>
    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e) =>
        ConfirmMasterRowSwitch(gvw1, e, row => _ = SafeExecuteAsync(() => LoadRouteAsync(row.Row["route_id"]?.ToString()), "라우팅 조회"));

    /// <summary>선택한 라우팅 한 건(헤더 + 공정행)을 우측에 채운다.</summary>
    private async Task LoadRouteAsync(string? routeId)
    {
        if (string.IsNullOrEmpty(routeId)) { EnterNewMode(); return; }

        var seq = ++_loadSeq;
        var tables = await QueryMultiAsync("USP_PR_ROUTE_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q", ["p_route_id"] = routeId });
        if (seq != _loadSeq) return; // 그 사이 다른 행을 골랐다 - 이 응답은 버린다

        var header = tables.Count > 0 ? tables[0] : new DataTable();
        _detail = tables.Count > 1 ? tables[1] : new DataTable();
        _items = tables.Count > 2 ? tables[2] : new DataTable();

        if (header.Rows.Count > 0) OnRowLoaded(header.Rows[0]);
        else EnterNewMode();
    }

    private void OnRowLoaded(DataRow row)
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = row["route_id"]?.ToString();
            cboAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            txtRouteCd.Text = row["route_cd"]?.ToString() ?? string.Empty;
            txtRouteCd.Properties.ReadOnly = true; // 라우팅코드는 저장 후 변경 불가
            txtRouteNm.Text = row["route_nm"]?.ToString() ?? string.Empty;
            chkUseYn.Checked = row["use_yn"]?.ToString() != "N";
            memoRemark.Text = row["remark"]?.ToString() ?? string.Empty;
            lblUsedNote.Text = row["used_yn"]?.ToString() == "Y" ? "※ 작업지시에서 사용 중인 라우팅입니다. 삭제할 수 없고, 수정은 이후 작업지시에만 반영됩니다." : string.Empty;

            TrackDirty(_detail);
            grd2.DataSource = _detail;
            TrackDirty(_items);
            grd3.DataSource = _items;
        });
    }

    private void EnterNewMode()
    {
        _loadSeq++; // 진행 중이던 목록 행 조회 응답은 무효
        SuppressDirtyTracking(() =>
        {
            _editingKey = null;
            cboAccId.EditValue = Session.AccId?.ToString();
            txtRouteCd.Text = string.Empty;
            txtRouteCd.Properties.ReadOnly = false;
            txtRouteNm.Text = string.Empty;
            chkUseYn.Checked = true;
            memoRemark.Text = string.Empty;
            lblUsedNote.Text = string.Empty;

            _detail = _detail.Clone();
            TrackDirty(_detail);
            grd2.DataSource = _detail;
            _items = _items.Clone();
            TrackDirty(_items);
            grd3.DataSource = _items;
        });
    }

    public override Task NewClick()
    {
        EnterNewMode();
        FocusFirstEntryField(panData); // 사업장 다음 첫 탭오더 컨트롤에 커서(표준)
        return Task.CompletedTask;
    }

    public override Task NewRowClick()
    {
        AddRow();
        return Task.CompletedTask;
    }

    public override Task DeleteRowClick()
    {
        DeleteRow();
        return Task.CompletedTask;
    }

    // ===== 저장 / 삭제 =====

    /// <summary>공정 순서대로 앞 공정 산출품목 = 다음 공정 투입품목인지 본다. 끊긴 곳이 있으면 안내 문구를 돌려준다(없으면 null).</summary>
    private string? CheckChain()
    {
        var rows = _detail.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted)
            .OrderBy(r => int.TryParse(r["serl"]?.ToString(), out var s) ? s : int.MaxValue).ToList();
        for (var i = 1; i < rows.Count; i++)
        {
            var prevOut = rows[i - 1]["out_item_id"]?.ToString();
            var curIn = rows[i]["in_item_id"]?.ToString();
            if (!string.IsNullOrEmpty(prevOut) && !string.IsNullOrEmpty(curIn) && prevOut != curIn)
                return $"{rows[i - 1]["serl"]}번 공정의 산출품목과 {rows[i]["serl"]}번 공정의 투입품목이 다릅니다.";
        }
        return null;
    }

    public override async Task SaveClick()
    {
        gvw2.CloseEditor();
        gvw2.UpdateCurrentRow();
        gvw3.CloseEditor();
        gvw3.UpdateCurrentRow();

        var broken = CheckChain();
        if (broken != null && AppMessageBox.Show(broken + "\n그래도 저장하시겠습니까?", "공정 연결 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        var headerParams = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_route_id"] = _editingKey,
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_route_cd"] = txtRouteCd.Text,
            ["p_route_nm"] = txtRouteNm.Text,
            ["p_use_yn"] = chkUseYn.Checked ? "Y" : "N",
            ["p_remark"] = memoRemark.Text,
        };
        var headerResult = await SaveAsync("USP_PR_ROUTE_S", headerParams);
        if (headerResult == null || !headerResult.Success)
        {
            AppMessageBox.Show(headerResult?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        var headerKey = _editingKey ?? headerResult.GeneratedCode;

        // 삭제 -> 수정 -> 추가 순서(순번을 지우고 다시 쓰는 경우 대비)
        var rows = _detail.Rows.Cast<DataRow>()
            .Where(r => r.RowState != DataRowState.Unchanged)
            .OrderBy(r => r.RowState == DataRowState.Deleted ? 0 : r.RowState == DataRowState.Modified ? 1 : 2)
            .ToList();
        foreach (var row in rows)
        {
            var workType = row.RowState == DataRowState.Deleted ? "D" : row.RowState == DataRowState.Added ? "N" : "U";
            var version = row.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;

            var detailParams = new Dictionary<string, string?>
            {
                ["p_work_type"] = workType,
                ["p_route_id"] = headerKey,
                ["p_serl"] = ProcData.Str(row, "serl", version),
                ["p_proc_cd"] = ProcData.Str(row, "proc_cd", version),
                ["p_cust_id"] = ProcData.Str(row, "cust_id", version),
                ["p_in_item_id"] = ProcData.Str(row, "in_item_id", version),
                ["p_out_item_id"] = ProcData.Str(row, "out_item_id", version),
                ["p_in_unit_cd"] = ProcData.Str(row, "in_unit_cd", version),
                ["p_out_unit_cd"] = ProcData.Str(row, "out_unit_cd", version),
                ["p_split_qty"] = ProcData.Str(row, "split_qty", version),
                ["p_price_unit_cd"] = ProcData.Str(row, "price_unit_cd", version),
                ["p_price"] = ProcData.Str(row, "price", version),
                ["p_remark"] = ProcData.Str(row, "remark", version),
            };
            var detailResult = await SaveAsync("USP_PR_ROUTE_S_1", detailParams);
            if (detailResult == null || !detailResult.Success)
            {
                AppMessageBox.Show(detailResult?.Message ?? "공정 저장에 실패했습니다.", "저장 실패");
                // 헤더는 이미 저장됐으므로 그 상태로 다시 조회해서 화면을 실제 저장 상태에 맞춘다.
                _editingKey ??= headerResult.GeneratedCode;
                await QueryCore(preserveSelection: true);
                return;
            }
        }

        // 적용 제품: 삭제 -> 추가/수정(기본 지정은 서버가 제품당 1개로 정리한다)
        var itemRows = _items.Rows.Cast<DataRow>()
            .Where(r => r.RowState != DataRowState.Unchanged)
            .OrderBy(r => r.RowState == DataRowState.Deleted ? 0 : 1)
            .ToList();
        foreach (var row in itemRows)
        {
            var del = row.RowState == DataRowState.Deleted;
            var version = del ? DataRowVersion.Original : DataRowVersion.Current;
            // 제품을 다른 제품으로 바꾼 행은 옛 제품 연결을 먼저 지운다
            if (row.RowState == DataRowState.Modified)
            {
                var oldItem = row["item_id", DataRowVersion.Original]?.ToString();
                var newItem = row["item_id", DataRowVersion.Current]?.ToString();
                if (oldItem != newItem && !string.IsNullOrEmpty(oldItem))
                {
                    var delOld = await SaveAsync("USP_PR_ROUTE_S_2", new Dictionary<string, string?>
                    {
                        ["p_work_type"] = "D", ["p_route_id"] = headerKey, ["p_item_id"] = oldItem,
                    });
                    if (delOld == null || !delOld.Success)
                    {
                        AppMessageBox.Show(delOld?.Message ?? "적용 제품 저장에 실패했습니다.", "저장 실패");
                        _editingKey ??= headerResult.GeneratedCode;
                        await QueryCore(preserveSelection: true);
                        return;
                    }
                }
            }
            var itemId = ProcData.Str(row, "item_id", version);
            if (string.IsNullOrEmpty(itemId)) continue;
            var linkResult = await SaveAsync("USP_PR_ROUTE_S_2", new Dictionary<string, string?>
            {
                ["p_work_type"] = del ? "D" : "N",
                ["p_route_id"] = headerKey,
                ["p_item_id"] = itemId,
                ["p_default_yn"] = ProcData.Str(row, "default_yn", version),
            });
            if (linkResult == null || !linkResult.Success)
            {
                AppMessageBox.Show(linkResult?.Message ?? "적용 제품 저장에 실패했습니다.", "저장 실패");
                _editingKey ??= headerResult.GeneratedCode;
                await QueryCore(preserveSelection: true);
                return;
            }
        }

        _editingKey ??= headerResult.GeneratedCode;
        Toast.Show("저장되었습니다.");
        await QueryCore(preserveSelection: true); // 방금 저장한 라우팅을 목록에서 그대로 선택해 둔다(신규였다면 목록에 새로 나타난다)
    }

    protected override bool ConfirmDeleteByDefault => false; // 삭제 확인창을 DeleteClick에서 직접 띄움(문서번호 등 상세 문구)

    public override async Task DeleteClick()
    {
        if (_editingKey == null)
        {
            AppMessageBox.Show("삭제할 라우팅을 목록에서 먼저 선택해주세요.", "안내");
            return;
        }

        var confirm = AppMessageBox.Show(
            $"선택하신 라우팅을 삭제 하시겠습니까?\n\n[{txtRouteCd.Text}] {txtRouteNm.Text}",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await SaveAsync("USP_PR_ROUTE_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = "D",
            ["p_route_id"] = _editingKey,
        });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "삭제에 실패했습니다.", "삭제 실패");
            return;
        }

        // 지워진 라우팅을 계속 편집 상태로 두면 안 되므로 편집 대상을 먼저 놓아준다. 재조회가 첫 행을 골라 우측을 채운다.
        _editingKey = null;
        await QueryClick();
        if (_list.Rows.Count == 0) EnterNewMode();
        Toast.Show("삭제되었습니다.");
    }

    /// <summary>다른 화면에서 라우팅을 열 때 호출된다(key=route_id 문자열). 목록을 다시 불러오고 그 행을 선택한다.</summary>
    public override async Task FocusRecordAsync(string key)
    {
        _editingKey = key;
        await QueryCore(preserveSelection: true);
    }
}
