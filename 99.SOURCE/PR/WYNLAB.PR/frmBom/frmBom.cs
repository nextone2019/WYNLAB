using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

/// <summary>
/// BOM관리 - 라우팅관리와 같은 구조다. 좌측 grd1에 BOM 목록(TPRBOMM, 품목당 1개), 우측에 선택한 BOM의 헤더(상위 품목/사용여부/비고)와 구성품 그리드 grd2(TPRBOMD).
/// 구성품 구분은 주원료(MAIN, 공정에 투입되는 LOT - BOM당 1개) / 원자재(RAW) / 부자재(SUB) / 소모품(CON)이고, 소요수량은 상위 품목 1단위를 만드는 데 드는 수량,
/// 손실율(%)과 투입/소모 공정(비우면 상위 품목을 만드는 공정 전체)을 둘 수 있다. 라우팅 공정은 산출품목의 BOM 주원료를 투입품목으로 쓰고, 작업지시를 저장할 때
/// 각 공정 산출품목의 BOM이 자재소요(TPRWOMAT)로 복사된다 - BOM을 고쳐도 이미 낸 작업지시에는 영향이 없다. 상위 품목은 저장 후 바꿀 수 없다(라우팅이 쓰는 BOM이면 서버가 막는다).
/// </summary>
public partial class frmBom : BaseForm
{
    private DataTable _list = new();
    private DataTable _detail = new();
    private DataTable _itemCache = new();
    private string? _editingKey; // 선택한(저장된) BOM의 bom_id. null이면 신규모드
    private int _loadSeq;        // 목록 행을 빠르게 넘길 때 늦게 온 응답이 최신 선택을 덮어쓰지 않게 하는 번호

    public frmBom()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준(2026-10-03): 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "BOM관리";

        Controls.Add(BuildScreenHeader());

        txtItemNm.MapField("item_id", txtItemId);

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

        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) =>
        {
            _itemCache = await LoadPopupRowsAsync("P_ITEM");
            await SafeExecuteAsync(EnsureSchemaAsync, "BOM 화면 초기화");
            await SafeExecuteAsync(QueryClick, "BOM 목록 조회");
        };
    }

    private void AddRow() => gvw2.AddNewRow();

    private void DeleteRow()
    {
        gvw2.CloseEditor();
        gvw2.UpdateCurrentRow();
        try { if (gvw2.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
        catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
    }

    private async Task EnsureSchemaAsync()
    {
        if (_detail.Columns.Count > 0) return;

        var tables = await QueryMultiAsync("USP_PR_BOM_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q", ["p_bom_id"] = "-1" });
        if (tables.Count > 1) _detail = tables[1];
        EnterNewMode();
    }

    /// <summary>새 구성품 행: 순번은 마지막 + 1, 구분은 주원료가 아직 없으면 주원료, 있으면 원자재, 손실율 0.</summary>
    private void Gvw2_InitNewRow(object? sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
    {
        var max = 0;
        var hasMain = false;
        foreach (DataRow r in _detail.Rows)
        {
            if (r.RowState == DataRowState.Deleted) continue;
            if (int.TryParse(r["serl"]?.ToString(), out var s) && s > max) max = s;
            if (r["comp_type"]?.ToString() == "MAIN") hasMain = true;
        }
        gvw2.SetRowCellValue(e.RowHandle, colSerl, max + 1);
        gvw2.SetRowCellValue(e.RowHandle, colCompType, hasMain ? "RAW" : "MAIN");
        gvw2.SetRowCellValue(e.RowHandle, colQtyPer, 1m);
        gvw2.SetRowCellValue(e.RowHandle, colLossRate, 0m);
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

        // 숫자 컬럼에 "1,500"처럼 문자열을 입력하면 decimal로 바꿔 넣는다(다른 화면과 같은 처리)
        if (e.Value is string text && (e.Column == colQtyPer || e.Column == colLossRate))
        {
            gvw2.SetRowCellValue(e.RowHandle, e.Column,
                decimal.TryParse(text.Replace(",", ""), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var n)
                    ? n : (object)DBNull.Value);
            return;
        }

        // 구성품을 고르면 그 품목의 재고단위를 단위 기본값으로 채운다
        if (e.Column == colCompItem)
        {
            var unit = FindItem(e.Value)?["unit_cd"]?.ToString();
            if (string.IsNullOrEmpty(unit)) return;
            _syncing = true;
            try { gvw2.SetRowCellValue(e.RowHandle, colUnit, unit); }
            finally { _syncing = false; }
        }
    }

    // ===== 조회(좌측 목록) =====

    public override async Task QueryClick() => await QueryCore(preserveSelection: false);

    private async Task QueryCore(bool preserveSelection)
    {
        _list = await QueryAsync("USP_PR_BOM_Q", new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "L",
            ["p_keyword"] = txtSearchKeyword.Text.Trim(),
            ["p_acc_id"] = Session.AccId?.ToString(),
        });

        var editingKey = preserveSelection ? _editingKey : null;

        if (editingKey == null)
        {
            grd1.DataSource = _list;
            if (_list.Rows.Count == 0) EnterNewMode();
            return;
        }

        var targetRow = FindListRow(editingKey);

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

        if (targetRow != null) await LoadBomAsync(editingKey);
        else EnterNewMode();
    }

    private DataRow? FindListRow(string bomId) =>
        _list.Rows.Cast<DataRow>().FirstOrDefault(r => string.Equals(r["bom_id"]?.ToString(), bomId, StringComparison.OrdinalIgnoreCase));

    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e) =>
        ConfirmMasterRowSwitch(gvw1, e, row => _ = SafeExecuteAsync(() => LoadBomAsync(row.Row["bom_id"]?.ToString()), "BOM 조회"));

    private async Task LoadBomAsync(string? bomId)
    {
        if (string.IsNullOrEmpty(bomId)) { EnterNewMode(); return; }

        var seq = ++_loadSeq;
        var tables = await QueryMultiAsync("USP_PR_BOM_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q", ["p_bom_id"] = bomId });
        if (seq != _loadSeq) return;

        var header = tables.Count > 0 ? tables[0] : new DataTable();
        _detail = tables.Count > 1 ? tables[1] : new DataTable();

        if (header.Rows.Count > 0) OnRowLoaded(header.Rows[0]);
        else EnterNewMode();
    }

    private void OnRowLoaded(DataRow row)
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = row["bom_id"]?.ToString();
            cboAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
            txtItemId.Text = row["item_id"]?.ToString() ?? string.Empty;
            txtItemNm.Text = row["item_nm"]?.ToString() ?? string.Empty;
            chkUseYn.Checked = row["use_yn"]?.ToString() != "N";
            memoRemark.Text = row["remark"]?.ToString() ?? string.Empty;
            lblUsedNote.Text = row["used_yn"]?.ToString() == "Y" ? "※ 라우팅에서 산출품목으로 쓰는 BOM입니다. 삭제/주원료 변경은 불가합니다." : string.Empty;

            TrackDirty(_detail);
            grd2.DataSource = _detail;
        });
    }

    private void EnterNewMode()
    {
        _loadSeq++;
        SuppressDirtyTracking(() =>
        {
            _editingKey = null;
            cboAccId.EditValue = Session.AccId?.ToString();
            txtItemId.Text = string.Empty;
            txtItemNm.Text = string.Empty;
            chkUseYn.Checked = true;
            memoRemark.Text = string.Empty;
            lblUsedNote.Text = string.Empty;

            _detail = _detail.Clone();
            TrackDirty(_detail);
            grd2.DataSource = _detail;
        });
    }

    public override Task NewClick()
    {
        EnterNewMode();
        FocusFirstEntryField(panData);
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

    public override async Task SaveClick()
    {
        gvw2.CloseEditor();
        gvw2.UpdateCurrentRow();

        // 주원료는 정확히 1개여야 라우팅 공정의 투입품목이 결정된다 - 서버는 2개 이상만 막으므로 없는 경우는 여기서 알려준다.
        var live = _detail.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted).ToList();
        if (!live.Any(r => r["comp_type"]?.ToString() == "MAIN") &&
            AppMessageBox.Show("주원료(공정에 투입되는 LOT) 구성품이 없습니다.\n주원료가 없으면 라우팅 공정의 투입품목을 정할 수 없습니다. 그래도 저장하시겠습니까?",
                "주원료 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        var headerParams = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_bom_id"] = _editingKey,
            ["p_acc_id"] = cboAccId.EditValue?.ToString(),
            ["p_item_id"] = txtItemId.Text,
            ["p_use_yn"] = chkUseYn.Checked ? "Y" : "N",
            ["p_remark"] = memoRemark.Text,
        };
        var headerResult = await SaveAsync("USP_PR_BOM_S", headerParams);
        if (headerResult == null || !headerResult.Success)
        {
            AppMessageBox.Show(headerResult?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        var headerKey = _editingKey ?? headerResult.GeneratedCode;

        // 삭제 -> 수정 -> 추가 순서(주원료를 바꿔 끼우는 경우 "주원료 1개" 검사에 걸리지 않게)
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
                ["p_bom_id"] = headerKey,
                ["p_serl"] = ProcData.Str(row, "serl", version),
                ["p_comp_type"] = ProcData.Str(row, "comp_type", version),
                ["p_comp_item_id"] = ProcData.Str(row, "comp_item_id", version),
                ["p_qty_per"] = ProcData.Str(row, "qty_per", version),
                ["p_unit_cd"] = ProcData.Str(row, "unit_cd", version),
                ["p_loss_rate"] = ProcData.Str(row, "loss_rate", version),
                ["p_proc_cd"] = ProcData.Str(row, "proc_cd", version),
                ["p_remark"] = ProcData.Str(row, "remark", version),
            };
            var detailResult = await SaveAsync("USP_PR_BOM_S_1", detailParams);
            if (detailResult == null || !detailResult.Success)
            {
                AppMessageBox.Show(detailResult?.Message ?? "구성품 저장에 실패했습니다.", "저장 실패");
                _editingKey ??= headerResult.GeneratedCode;
                await QueryCore(preserveSelection: true);
                return;
            }
        }

        _editingKey ??= headerResult.GeneratedCode;
        Toast.Show("저장되었습니다.");
        await QueryCore(preserveSelection: true);
    }

    protected override bool ConfirmDeleteByDefault => false; // 삭제 확인창을 DeleteClick에서 직접 띄움(문서번호 등 상세 문구)

    public override async Task DeleteClick()
    {
        if (_editingKey == null)
        {
            AppMessageBox.Show("삭제할 BOM을 목록에서 먼저 선택해주세요.", "안내");
            return;
        }

        var confirm = AppMessageBox.Show(
            $"선택하신 BOM을 삭제 하시겠습니까?\n\n{txtItemNm.Text}",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await SaveAsync("USP_PR_BOM_S", new Dictionary<string, string?>
        {
            ["p_work_type"] = "D",
            ["p_bom_id"] = _editingKey,
        });
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "삭제에 실패했습니다.", "삭제 실패");
            return;
        }

        _editingKey = null;
        await QueryClick();
        if (_list.Rows.Count == 0) EnterNewMode();
        Toast.Show("삭제되었습니다.");
    }

    public override async Task FocusRecordAsync(string key)
    {
        _editingKey = key;
        await QueryCore(preserveSelection: true);
    }
}
