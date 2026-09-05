using System.Data;
using DevExpress.XtraGrid.Views.Grid;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.BA;

/// <summary>
/// 품목등록 화면 - grd1(품목 목록)에서 고르면 panData(TBAITEM 상세)를 채우고, grd2에
/// 그 품목의 단위환산(TBAITEMUNIT)을 보여준다. grd2는 조회 전용이 아니라 입력/편집 가능
/// (frmDept의 소속사원 grd2와 다른 점 - 여기는 실제로 등록/수정 대상이다).
///
/// USP_BA_ITEM_S가 저장 시 unit_cd != po_unit_cd면 TBAITEMUNIT에 1:1 환산행을 자동으로
/// 만들어준다(unit_cd 기준=fr_unit_cd) - grd2는 그 자동생성된 행의 비율을 고치거나, 필요하면
/// 행을 더 추가하는 용도다.
/// </summary>
public partial class frmItem : BaseForm
{
    private DataTable _items = new();
    private DataTable _units = new();
    private string? _editingItemId; // TBAITEM.item_id(BIGINT)를 문자열로 들고 있음. null이면 신규모드.

    public frmItem()
    {
        InitializeComponent();

        Text = "품목등록";

        // 기본단위/구매단위 - LookUp관리(frmSysLookup)에 등록해둔 "단위"(L_CM0001)를 그대로
        // 참조한다. LookupKey만 지정하면 그 뒤로는 서버(sysLookupM/P)가 무슨 프로시저/쿼리로
        // 채울지 전부 알아서 처리해서, 이 화면 코드는 실제 데이터 출처(프로시저였다가 나중에
        // 쿼리로 바뀌어도)와 완전히 무관하다(2026-09-02 - 기존 TextEditWyn 자유입력에서
        // LookUpEditWyn 선택형으로 전환하라는 요청).
        cboUnitCd.LookupKey = "L_CM0001";  //단위
        cboPoUnitCd.LookupKey = "L_CM0001";
        cboAssetType.LookupKey = "L_CM00002"; //자산구분

        // grd1(품목 목록)은 조회전용, grd2(환산단위)는 실제 등록/수정 대상이라 Edit -
        // frmSysLookup과 같은 이유로 명시(2026-09-02, [[project_wynlab_grid_role_mechanism]]).
        gvw1.Role = GridRoleWyn.Query;
        gvw2.Role = GridRoleWyn.Edit;

        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;
        gvw2.InitNewRow += Gvw2_InitNewRow;

        // 환산단위(grd2 컬럼) - panData의 기본단위/구매단위와 같은 LookUp("L_CM0001")을 그리드
        // 셀 편집기로도 붙인다. LookUpEditWyn 자신은 Control이라 그리드 컬럼엔 못 들어가서,
        // RepositoryItemLookUpEdit 버전을 만들어주는 정적 헬퍼를 쓴다(LookUpEditWyn.
        // CreateGridRepositoryItemAsync 설명 참고) - 컬럼 편집기 목록은 한 번 만들면 끝(호출
        // 시점 스냅샷)이라 화면을 열 때 한 번만 비동기로 채운다.
        _ = SetupUnitToLookupAsync();

        // 화면종료 시 저장 확인(BaseForm.ConfirmCloseAsync) - grd2(TBAITEMUNIT)는 편집 가능한
        // 그리드라 panData뿐 아니라 그 DataTable도 걸어야 한다. _units는 EnterNewMode/
        // LoadUnitsAsync에서 매번 새 인스턴스로 교체되므로, 두 곳 모두에서 재구독한다(아래 참고).
        TrackDirty(panData);

        EnterNewMode();
    }

    /// <summary>품목 목록 조회. 저장 후 재조회에서도 편집하던 품목이 그대로 선택돼 있어야
    /// 한다 - QueryClick은 항상 이 형태를 유지할 것(TemplateForm.QueryClick 주석 참고).</summary>
    public override async Task QueryClick()
    {
        var keyword = txtSearchQ.Text.Trim();

        _items = await QueryAsync("USP_BA_ITEM_Q", new
        {
            p_work_type = "Q",
            p_item_cd = keyword,
            p_item_nm = keyword
        });

        var editingItemId = _editingItemId;

        gvw1.FocusedRowObjectChanged -= Gvw1_FocusedRowObjectChanged;
        try
        {
            grd1.DataSource = _items;

            if (editingItemId != null)
            {
                var handle = FindRowHandle(editingItemId);
                if (handle != null) gvw1.FocusedRowHandle = handle.Value;
            }
        }
        finally
        {
            gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;
        }

        var row = editingItemId == null ? null : FindItemRow(editingItemId);
        if (row != null) EnterEditMode(row);
        else EnterNewMode();
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    public override async Task DeleteClick()
    {
        if (_editingItemId == null)
        {
            AppMessageBox.Show("삭제할 품목을 먼저 선택해주세요.", "안내");
            return;
        }

        var confirm = AppMessageBox.Show(
            $"선택하신 품목을 삭제 하시겠습니까?\n\n[{txtItemCd.Text}] {txtItemNm.Text}",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await SaveAsync("USP_BA_ITEM_S", new
        {
            p_work_type = "D",
            p_item_id = _editingItemId
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
        // 품목이 아직 저장 전(신규 입력 중)이면 item_id가 없어서 TBAITEMUNIT에 행을 못 붙인다.
        if (_editingItemId == null)
        {
            AppMessageBox.Show("품목을 먼저 등록한 뒤에 단위환산을 추가할 수 있습니다.", "안내");
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
        if (string.IsNullOrWhiteSpace(txtItemCd.Text) || string.IsNullOrWhiteSpace(txtItemNm.Text))
        {
            AppMessageBox.Show("품목코드와 품목명은 필수입니다.", "확인");
            return;
        }

        var wasNew = _editingItemId == null;

        var result = await SaveAsync("USP_BA_ITEM_S", new
        {
            p_work_type = wasNew ? "N" : "U",
            p_item_id = _editingItemId,
            p_item_cd = txtItemCd.Text,
            p_item_no = txtItemNo.Text,
            p_item_nm = txtItemNm.Text,
            p_item_spec = txtItemSpec.Text,
            p_unit_cd = cboUnitCd.EditValue?.ToString() ?? string.Empty,
            p_po_unit_cd = cboPoUnitCd.EditValue?.ToString() ?? string.Empty,
            p_wh_cd = txtWhCd.Text,
            p_loc_cd = txtLocCd.Text,
            p_safe_qty = string.IsNullOrWhiteSpace(txtSafeQty.Text) ? null : txtSafeQty.Text,
            p_dept_cd = txtDeptCd.Text,
            p_emp_no = txtEmpNo.Text,
            p_prod_yn = txtProdYn.Text,
            p_cust_cd = txtCustCd.Text,
            p_asset_type = cboAssetType.EditValue?.ToString() ??  string.Empty,
            p_out_type = txtOutType.Text,
            p_po_qc_yn = txtPoQcYn.Text,
            p_prod_qc_yn = txtProdQcYn.Text,
            p_lot_yn = txtLotYn.Text,
            p_stock_yn = txtStockYn.Text,
            p_po_yn = txtPoYn.Text,
            p_po_price = string.IsNullOrWhiteSpace(txtPoPrice.Text) ? null : txtPoPrice.Text,
            p_sale_yn = txtSaleYn.Text,
            p_sale_price = string.IsNullOrWhiteSpace(txtSalePrice.Text) ? null : txtSalePrice.Text,
            p_stat_cd = txtStatCd.Text,
            p_item_class1 = txtItemClass1.Text,
            p_item_class2 = txtItemClass2.Text,
            p_item_class3 = txtItemClass3.Text,
            p_item_class4 = txtItemClass4.Text,
            p_po_acnt_cd = txtPoAcntCd.Text,
            p_sale_acnt_cd = txtSaleAcntCd.Text,
            p_remark = txtRemark.Text
        });

        if (!result.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(result), "저장 실패");
            return;
        }

        var savedItemId = wasNew ? (result.GeneratedCode ?? _editingItemId!) : _editingItemId!;

        // 그리드에서 편집 중이던 셀 값을 먼저 확정해야 아래 저장에 마지막 수정이 포함된다.
        gvw2.CloseEditor();
        gvw2.UpdateCurrentRow();

        var unitSaveError = await SaveUnitRowsAsync(savedItemId);
        if (unitSaveError != null)
        {
            AppMessageBox.Show(unitSaveError, "저장 실패");
            return;
        }

        _units.AcceptChanges();
        _editingItemId = savedItemId;
        await QueryClick();
        Toast.Show(wasNew ? "등록되었습니다." : "수정되었습니다.");
    }

    /// <summary>
    /// grd2(TBAITEMUNIT)에서 바뀐 행마다 USP_BA_ITEMUNIT_S를 한 번씩 호출한다 - 007/
    /// USP_SM_MINORCODE_S_1과 같은 단일 레코드 CRUD 구조라 그리드 전체를 한 번에 못 보낸다.
    ///
    /// 키가 (item_cd, fr_unit_cd, to_unit_cd) 3개라 fr_unit_cd/to_unit_cd 자체를 고친 행은
    /// "원래 키 삭제(D) + 새 키 등록(N)"으로 표현한다 - frmMinorCode.SaveModifiedMinorAsync와
    /// 같은 이유(이 프로시저의 'U' 분기도 키는 WHERE에서만 쓰고 안 바꾼다).
    /// </summary>
    private async Task<string?> SaveUnitRowsAsync(string itemId)
    {
        foreach (DataRow row in _units.Rows)
        {
            string? error = row.RowState switch
            {
                DataRowState.Added => await SaveNewUnitAsync(itemId, row),
                DataRowState.Modified => await SaveModifiedUnitAsync(itemId, row),
                DataRowState.Deleted => await SaveDeletedUnitAsync(itemId, row),
                _ => null
            };
            if (error != null) return error;
        }
        return null;
    }

    private async Task<string?> SaveNewUnitAsync(string itemId, DataRow row)
    {
        var frUnitCd = ProcData.Str(row, "fr_unit_cd", DataRowVersion.Current);
        var toUnitCd = ProcData.Str(row, "to_unit_cd", DataRowVersion.Current);
        if (frUnitCd.Length == 0 || toUnitCd.Length == 0) return null; // 단위코드 없이 행만 추가된 빈 행은 건너뜀

        var result = await SaveAsync("USP_BA_ITEMUNIT_S", UnitParams("N", itemId, row, DataRowVersion.Current));
        return result.Success ? null : $"[{frUnitCd}->{toUnitCd}] {FormatSaveFailMessage(result)}";
    }

    private async Task<string?> SaveModifiedUnitAsync(string itemId, DataRow row)
    {
        var origFr = ProcData.Str(row, "fr_unit_cd", DataRowVersion.Original);
        var origTo = ProcData.Str(row, "to_unit_cd", DataRowVersion.Original);
        var curFr = ProcData.Str(row, "fr_unit_cd", DataRowVersion.Current);
        var curTo = ProcData.Str(row, "to_unit_cd", DataRowVersion.Current);

        if (curFr != origFr || curTo != origTo)
        {
            var delResult = await SaveAsync("USP_BA_ITEMUNIT_S", new
            {
                p_work_type = "D",
                p_item_cd = itemId,
                p_fr_unit_cd = origFr,
                p_to_unit_cd = origTo
            });
            if (!delResult.Success) return $"[{origFr}->{origTo}] {FormatSaveFailMessage(delResult)}";

            var addResult = await SaveAsync("USP_BA_ITEMUNIT_S", UnitParams("N", itemId, row, DataRowVersion.Current));
            return addResult.Success ? null : $"[{curFr}->{curTo}] {FormatSaveFailMessage(addResult)}";
        }

        var result = await SaveAsync("USP_BA_ITEMUNIT_S", UnitParams("U", itemId, row, DataRowVersion.Current));
        return result.Success ? null : $"[{curFr}->{curTo}] {FormatSaveFailMessage(result)}";
    }

    private async Task<string?> SaveDeletedUnitAsync(string itemId, DataRow row)
    {
        var frUnitCd = ProcData.Str(row, "fr_unit_cd", DataRowVersion.Original);
        var toUnitCd = ProcData.Str(row, "to_unit_cd", DataRowVersion.Original);
        var result = await SaveAsync("USP_BA_ITEMUNIT_S", new
        {
            p_work_type = "D",
            p_item_cd = itemId,
            p_fr_unit_cd = frUnitCd,
            p_to_unit_cd = toUnitCd
        });
        return result.Success ? null : $"[{frUnitCd}->{toUnitCd}] {FormatSaveFailMessage(result)}";
    }

    private static Dictionary<string, string?> UnitParams(string workType, string itemId, DataRow row, DataRowVersion version) => new()
    {
        ["p_work_type"] = workType,
        ["p_item_cd"] = itemId,
        ["p_fr_unit_cd"] = ProcData.Str(row, "fr_unit_cd", version),
        ["p_fr_qty"] = ProcData.Str(row, "fr_qty", version),
        ["p_to_unit_cd"] = ProcData.Str(row, "to_unit_cd", version),
        ["p_to_qty"] = ProcData.Str(row, "to_qty", version),
        ["p_remark"] = ProcData.Str(row, "remark", version)
    };

    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
    {
        if (e.Row is DataRowView view) EnterEditMode(view.Row);
    }

    /// <summary>grd2에 새 행을 추가하면 기준단위를 현재 품목의 기본단위로 채운다 - 신규 환산행은
    /// 대부분 그 품목 기준단위에서 다른 단위로 바꾸는 경우라 기본값이 있는 편이 자연스럽다.</summary>
    private void Gvw2_InitNewRow(object? sender, InitNewRowEventArgs e)
    {
        gvw2.SetRowCellValue(e.RowHandle, "fr_unit_cd", cboUnitCd.EditValue);
    }

    private async Task SetupUnitToLookupAsync()
    {
        var lookup = await LookUpEditWyn.CreateGridRepositoryItemAsync("L_CM0001");
        grd2.RepositoryItems.Add(lookup);
        colUnitToUnitCd.ColumnEdit = lookup;
    }

    private DataRow? FindItemRow(string itemId) =>
        _items.Rows.Cast<DataRow>()
            .FirstOrDefault(r => string.Equals(Convert.ToString(r["item_id"]), itemId, StringComparison.OrdinalIgnoreCase));

    /// <summary>item_id로 grd1의 행 핸들을 찾는다. GridView.LocateByValue를 안 쓰는 이유는
    /// frmMinorCode.FindMajorRowHandle과 같다(DataTable 컬럼이 실제로는 JsonElement).</summary>
    private int? FindRowHandle(string itemId)
    {
        var column = gvw1.Columns["item_id"];
        if (column == null) return null;

        for (var handle = 0; handle < gvw1.RowCount; handle++)
        {
            if (string.Equals(Convert.ToString(gvw1.GetRowCellValue(handle, column)), itemId, StringComparison.OrdinalIgnoreCase))
                return handle;
        }
        return null;
    }

    /// <summary>panData/그리드를 채우는 부분은 SuppressDirtyTracking으로 감싼다 - 안 그러면
    /// 코드가 값을 채우는 것뿐인데 TrackDirty가 "사용자가 고쳤다"로 오인해서, 조회/행 선택
    /// 직후부터 화면을 닫을 때 저장 확인이 뜨는 오작동이 생긴다.</summary>
    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingItemId = null;
            txtItemCd.Text = string.Empty;
            txtItemCd.ReadOnly = false;
            txtItemNo.Text = string.Empty;
            txtItemNm.Text = string.Empty;
            txtItemSpec.Text = string.Empty;
            cboUnitCd.EditValue = null;
            cboPoUnitCd.EditValue = null;
            txtWhCd.Text = string.Empty;
            txtLocCd.Text = string.Empty;
            txtSafeQty.Text = string.Empty;
            txtDeptCd.Text = string.Empty;
            txtEmpNo.Text = string.Empty;
            txtProdYn.Text = string.Empty;
            txtCustCd.Text = string.Empty;
            cboAssetType.EditValue = string.Empty;
            txtOutType.Text = string.Empty;
            txtPoQcYn.Text = string.Empty;
            txtProdQcYn.Text = string.Empty;
            txtLotYn.Text = string.Empty;
            txtStockYn.Text = string.Empty;
            txtPoYn.Text = string.Empty;
            txtPoPrice.Text = string.Empty;
            txtSaleYn.Text = string.Empty;
            txtSalePrice.Text = string.Empty;
            txtStatCd.Text = string.Empty;
            txtItemClass1.Text = string.Empty;
            txtItemClass2.Text = string.Empty;
            txtItemClass3.Text = string.Empty;
            txtItemClass4.Text = string.Empty;
            txtPoAcntCd.Text = string.Empty;
            txtSaleAcntCd.Text = string.Empty;
            txtRemark.Text = string.Empty;

            _units = _units.Clone();
            TrackDirty(_units);
            grd2.DataSource = _units;
        });
        txtItemCd.Focus();
    }

    private void EnterEditMode(DataRow item)
    {
        var itemId = Str(item, "item_id");
        var isSameItem = _editingItemId == itemId;

        SuppressDirtyTracking(() =>
        {
            _editingItemId = itemId;
            txtItemCd.Text = Str(item, "item_cd");
            txtItemCd.ReadOnly = true; // 품목코드는 더 이상 키는 아니지만, 목록 재조회 매칭 편의상 즉시 수정은 막아둔다
            txtItemNo.Text = Str(item, "item_no");
            txtItemNm.Text = Str(item, "item_nm");
            txtItemSpec.Text = Str(item, "item_spec");
            cboUnitCd.EditValue = Str(item, "unit_cd");
            cboPoUnitCd.EditValue = Str(item, "po_unit_cd");
            txtWhCd.Text = Str(item, "wh_cd");
            txtLocCd.Text = Str(item, "loc_cd");
            txtSafeQty.Text = Str(item, "safe_qty");
            txtDeptCd.Text = Str(item, "dept_cd");
            txtEmpNo.Text = Str(item, "emp_no");
            txtProdYn.Text = Str(item, "prod_yn");
            txtCustCd.Text = Str(item, "cust_cd");
            cboAssetType.EditValue = Str(item, "asset_type");
            txtOutType.Text = Str(item, "out_type");
            txtPoQcYn.Text = Str(item, "po_qc_yn");
            txtProdQcYn.Text = Str(item, "prod_qc_yn");
            txtLotYn.Text = Str(item, "lot_yn");
            txtStockYn.Text = Str(item, "stock_yn");
            txtPoYn.Text = Str(item, "po_yn");
            txtPoPrice.Text = Str(item, "po_price");
            txtSaleYn.Text = Str(item, "sale_yn");
            txtSalePrice.Text = Str(item, "sale_price");
            txtStatCd.Text = Str(item, "stat_cd");
            txtItemClass1.Text = Str(item, "item_class1");
            txtItemClass2.Text = Str(item, "item_class2");
            txtItemClass3.Text = Str(item, "item_class3");
            txtItemClass4.Text = Str(item, "item_class4");
            txtPoAcntCd.Text = Str(item, "po_acnt_cd");
            txtSaleAcntCd.Text = Str(item, "sale_acnt_cd");
            txtRemark.Text = Str(item, "remark");
        });

        if (!isSameItem) _ = LoadUnitsAsync(itemId);
    }

    private async Task LoadUnitsAsync(string itemId)
    {
        _units = await QueryAsync("USP_BA_ITEMUNIT_Q", new
        {
            p_work_type = "Q",
            p_item_cd = itemId
        });
        TrackDirty(_units);

        grd2.DataSource = _units;
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
