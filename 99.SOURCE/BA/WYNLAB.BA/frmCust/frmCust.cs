// AI Builder가 마스터-폼-탭그리드 템플릿을 복제해서 자동 생성 - 2026-09-04.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmCust : BaseForm
{
    private DataTable _list = new();
    private DataTable _detail1 = new();
    private DataTable _detail2 = new();
    private string? _editingKey; // null이면 신규모드

    public frmCust()
    {
        InitializeComponent();

        Text = "거래처등록";

        Controls.Add(BuildScreenHeader());

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.FocusedRowObjectChanged += async (s, e) => await OnMasterSelectedAsync();

        // grd2/grd3는 MasterFormSubGrid의 grd2(조회전용)와 달리 편집 가능하다 - 각자 자기
        // 저장프로시저(USP_BA_CUST_S_1/USP_BA_CUST_S_2)로 저장되기 때문. Role=Edit이면 그리드
        // 자신의 EmbeddedNavigator에도 추가/삭제 버튼이 뜬다(탭당 하나씩, 독립 동작).
        gvw2.Role = GridRoleWyn.Edit;
        gvw2.HighlightFocusedRow = true;
        gvw2.RowAdd += (s, e) => gvw2.AddNewRow();
        gvw2.RowDelete += (s, e) =>
        {
            try { if (gvw2.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };

        gvw3.Role = GridRoleWyn.Edit;
        gvw3.HighlightFocusedRow = true;
        gvw3.RowAdd += (s, e) => gvw3.AddNewRow();
        gvw3.RowDelete += (s, e) =>
        {
            try { if (gvw3.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };

        // panelWyn1의 공용 추가/삭제 버튼 - 현재 활성 탭의 그리드에 적용(각 그리드 자체
        // EmbeddedNavigator와 별개로, 탭을 안 넘나들어도 되는 지름길).
        btnAddRow2.Click += (s, e) => ActiveDetailView().AddNewRow();
        btnDeletRow2.Click += (s, e) =>
        {
            try { if (ActiveDetailView().GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };

        EnterNewMode();
        Load += async (s, e) => await QueryClick();
    }

    private GridViewWyn ActiveDetailView() => ReferenceEquals(tabDetailGrids.SelectedTabPage, tabDetail2) ? gvw3 : gvw2;

    public override async Task QueryClick()
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
        };
        _list = await QueryAsync("USP_BA_CUST_Q", p);
        grd1.DataSource = _list;
        EnterNewMode();
    }

    private async Task OnMasterSelectedAsync()
    {
        if (gvw1.GetFocusedRow() is not DataRowView view) { EnterNewMode(); return; }

        var row = view.Row;
        _editingKey = row["cust_cd"]?.ToString();
        txtDetailCustCd.Text = row["cust_cd"]?.ToString() ?? string.Empty;
        txtDetailCustNm.Text = row["cust_nm"]?.ToString() ?? string.Empty;
        txtDetailBizNo.Text = row["biz_no"]?.ToString() ?? string.Empty;
        txtDetailTel.Text = row["tel"]?.ToString() ?? string.Empty;
        cboDetailCurCd.EditValue = row["cur_cd"]?.ToString() ?? string.Empty;
        txtDetailOwnerNm.Text = row["owner_nm"]?.ToString() ?? string.Empty;
        txtDetailZipCode.Text = row["zip_code"]?.ToString() ?? string.Empty;
        txtDetailAddr1.Text = row["addr1"]?.ToString() ?? string.Empty;
        txtDetailAddr2.Text = row["addr2"]?.ToString() ?? string.Empty;
        txtDetailHomepage.Text = row["homepage"]?.ToString() ?? string.Empty;
        txtDetailEmail.Text = row["email"]?.ToString() ?? string.Empty;
        txtDetailFax.Text = row["fax"]?.ToString() ?? string.Empty;
        txtDetailBizKind.Text = row["biz_kind"]?.ToString() ?? string.Empty;
        txtDetailBizType.Text = row["biz_type"]?.ToString() ?? string.Empty;
        txtDetailTransOpenDate.Text = row["trans_open_date"]?.ToString() ?? string.Empty;
        txtDetailVatType.Text = row["vat_type"]?.ToString() ?? string.Empty;
        txtDetailVatRate.Text = row["vat_rate"]?.ToString() ?? string.Empty;
        txtDetailRemark.Text = row["remark"]?.ToString() ?? string.Empty;
        cboDetailStatCd.EditValue = row["stat_cd"]?.ToString() ?? string.Empty;
        txtDetailEmpNo.Text = row["emp_no"]?.ToString() ?? string.Empty;
        await LoadDetailAsync();
    }

    private void EnterNewMode()
    {
        _editingKey = null;
        txtDetailCustCd.Text = string.Empty;
        txtDetailCustNm.Text = string.Empty;
        txtDetailBizNo.Text = string.Empty;
        txtDetailTel.Text = string.Empty;
        cboDetailCurCd.EditValue = string.Empty;
        txtDetailOwnerNm.Text = string.Empty;
        txtDetailZipCode.Text = string.Empty;
        txtDetailAddr1.Text = string.Empty;
        txtDetailAddr2.Text = string.Empty;
        txtDetailHomepage.Text = string.Empty;
        txtDetailEmail.Text = string.Empty;
        txtDetailFax.Text = string.Empty;
        txtDetailBizKind.Text = string.Empty;
        txtDetailBizType.Text = string.Empty;
        txtDetailTransOpenDate.Text = string.Empty;
        txtDetailVatType.Text = string.Empty;
        txtDetailVatRate.Text = string.Empty;
        txtDetailRemark.Text = string.Empty;
        cboDetailStatCd.EditValue = string.Empty;
        txtDetailEmpNo.Text = string.Empty;
        _detail1 = _detail1.Clone();
        _detail2 = _detail2.Clone();
        grd2.DataSource = _detail1;
        grd3.DataSource = _detail2;
    }

    /// <summary>선택된 마스터 행의 하위 목록 2개(grd2/grd3)를 한 번의 호출로 같이 조회한다 -
    /// USP_BA_CUST_Q가 work_type='Q1'일 때 레코드셋을 2개(순서대로
    /// grd2용, grd3용) 반환하기 때문에 QueryMultiAsync를 쓴다(QueryAsync는 첫 레코드셋만 받음).</summary>
    private async Task LoadDetailAsync()
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q1",
            ["p_cust_cd"] = _editingKey,
        };
        var tables = await QueryMultiAsync("USP_BA_CUST_Q", p);
        _detail1 = tables.Count > 0 ? tables[0] : new DataTable();
        _detail2 = tables.Count > 1 ? tables[1] : new DataTable();
        grd2.DataSource = _detail1;
        grd3.DataSource = _detail2;
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        // ---- 1) 헤더(panData -> USP_BA_CUST_S) ----
        var headerParams = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_cust_cd"] = txtDetailCustCd.Text,
            ["p_cust_nm"] = txtDetailCustNm.Text,
            ["p_biz_no"] = txtDetailBizNo.Text,
            ["p_tel"] = txtDetailTel.Text,
            ["p_cur_cd"] = cboDetailCurCd.EditValue?.ToString() ?? string.Empty,
            ["p_owner_nm"] = txtDetailOwnerNm.Text,
            ["p_zip_code"] = txtDetailZipCode.Text,
            ["p_addr1"] = txtDetailAddr1.Text,
            ["p_addr2"] = txtDetailAddr2.Text,
            ["p_homepage"] = txtDetailHomepage.Text,
            ["p_email"] = txtDetailEmail.Text,
            ["p_fax"] = txtDetailFax.Text,
            ["p_biz_kind"] = txtDetailBizKind.Text,
            ["p_biz_type"] = txtDetailBizType.Text,
            ["p_trans_open_date"] = txtDetailTransOpenDate.Text,
            ["p_vat_type"] = txtDetailVatType.Text,
            ["p_vat_rate"] = txtDetailVatRate.Text,
            ["p_remark"] = txtDetailRemark.Text,
            ["p_stat_cd"] = cboDetailStatCd.EditValue?.ToString() ?? string.Empty,
            ["p_emp_no"] = txtDetailEmpNo.Text,
        };

        var headerResult = await SaveAsync("USP_BA_CUST_S", headerParams);
        if (headerResult == null || !headerResult.Success)
        {
            AppMessageBox.Show(headerResult?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        var headerKey = _editingKey ?? headerResult.GeneratedCode;

        // ---- 2) 명세1(grd2 -> USP_BA_CUST_S_1) ----
        var detail1Ok = await SaveDetailRowsAsync(gvw2, _detail1, "USP_BA_CUST_S_1", headerKey, (row, version) => new Dictionary<string, string?>
        {
            ["p_cust_cd"] = ProcData.Str(row, "cust_cd", version),
            ["p_serl"] = ProcData.Str(row, "serl", version),
            ["p_prsn_nm"] = ProcData.Str(row, "prsn_nm", version),
            ["p_grade"] = ProcData.Str(row, "grade", version),
            ["p_tel1"] = ProcData.Str(row, "tel1", version),
            ["p_tel2"] = ProcData.Str(row, "tel2", version),
            ["p_fax"] = ProcData.Str(row, "fax", version),
            ["p_email"] = ProcData.Str(row, "email", version),
        });
        if (!detail1Ok) return;

        // ---- 3) 명세2(grd3 -> USP_BA_CUST_S_2) ----
        var detail2Ok = await SaveDetailRowsAsync(gvw3, _detail2, "USP_BA_CUST_S_2", headerKey, (row, version) => new Dictionary<string, string?>
        {
            ["p_cust_cd"] = ProcData.Str(row, "cust_cd", version),
            ["p_serl"] = ProcData.Str(row, "serl", version),
            ["p_bank_cd"] = ProcData.Str(row, "bank_cd", version),
            ["p_acnt_no"] = ProcData.Str(row, "acnt_no", version),
            ["p_remark"] = ProcData.Str(row, "remark", version),
        });
        if (!detail2Ok) return;

        Toast.Show("저장되었습니다.");
        await QueryClick();
    }

    /// <summary>grd2/grd3 공통 저장 루프 - 변경된 행마다 N/U/D로 나눠 저장한다(SingleGrid.SaveClick과
    /// 같은 RowState 판정 방식). extraParams는 화면마다 다른 컬럼->파라미터 매핑을 행 하나 기준으로
    /// 만들어주는 콜백(생성기가 컬럼 목록으로 채워넣음).</summary>
    private async Task<bool> SaveDetailRowsAsync(GridViewWyn gvw, DataTable table, string saveProc, string? masterKey,
        Func<DataRow, DataRowVersion, Dictionary<string, string?>> extraParams)
    {
        gvw.CloseEditor();
        gvw.UpdateCurrentRow();

        foreach (DataRow row in table.Rows.Cast<DataRow>().ToList())
        {
            if (row.RowState == DataRowState.Unchanged) continue;

            var workType = row.RowState == DataRowState.Deleted ? "D" : row.RowState == DataRowState.Added ? "N" : "U";
            // Deleted 행에서 DataRowVersion.Current를 읽으면 DeletedRowInaccessibleException이 난다
            // (ProcData.Str 주석 참고) - 그래서 컬럼 매핑에도 이 버전을 그대로 넘겨준다.
            var version = row.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;

            var p = new Dictionary<string, string?>
            {
                ["p_work_type"] = workType,
                ["p_cust_cd"] = masterKey,
            };
            foreach (var kv in extraParams(row, version)) p[kv.Key] = kv.Value;

            var result = await SaveAsync(saveProc, p);
            if (result == null || !result.Success)
            {
                AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
                return false;
            }
        }
        return true;
    }

    public override async Task DeleteClick()
    {
        if (_editingKey == null) return;

        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "D",
            ["p_cust_cd"] = _editingKey,
        };
        var result = await SaveAsync("USP_BA_CUST_S", p);
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "삭제에 실패했습니다.", "삭제 실패");
            return;
        }

        Toast.Show("삭제되었습니다.");
        await QueryClick();
    }
}
