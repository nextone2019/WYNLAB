// @AI_BUILDER:BEGIN FILE_HEADER
// AI Builder 마스터-상세폼(단일 시트) 템플릿 원본 - 이 파일 자체는 실행되지 않는다(어떤 메뉴에도
// 등록돼 있지 않음). VS에서 열어 로직 골격을 확인/조정하는 용도.
// grd1(마스터 목록 그리드)이 없다 - panHeader에 검색조건을 입력하고 툴바의 조회 버튼을 누르면
// __QUERY_PROC__가 레코드셋을 여러 개(0번=panData용, 1~4번=grd2~grd5용, 전부 선택사항) 한 번에
// 돌려주고 그 0번 레코드셋의 첫 행을 그대로 panData에 채운다 - grd1에서 행을 선택하는 중간 단계
// 자체가 없다(2026-09-09 요청 - "grd1이 없는 모습").
// @AI_BUILDER:END FILE_HEADER
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.TEMPLATE;

public partial class TplMasterOneSheet : BaseForm
{
    private DataTable _detail1 = new();
    private DataTable _detail2 = new();
    private DataTable _detail3 = new();
    private DataTable _detail4 = new();
    private string? _editingKey; // null이면 신규모드

    public TplMasterOneSheet()
    {
        InitializeComponent();

        Text = "__MENU_CAPTION__";

        Controls.Add(BuildScreenHeader());

        // grd2~grd5(하위 그리드, 전부 선택사항)는 편집 가능하고 각자 자기 저장프로시저로 저장된다
        // (TplMasterFormSubGrid와 같은 방식) - Role=Edit이면 그리드 자신의 EmbeddedNavigator에도
        // 추가/삭제 버튼이 뜬다(탭당 하나씩, 독립 동작).
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

        gvw4.Role = GridRoleWyn.Edit;
        gvw4.HighlightFocusedRow = true;
        gvw4.RowAdd += (s, e) => gvw4.AddNewRow();
        gvw4.RowDelete += (s, e) =>
        {
            try { if (gvw4.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };

        gvw5.Role = GridRoleWyn.Edit;
        gvw5.HighlightFocusedRow = true;
        gvw5.RowAdd += (s, e) => gvw5.AddNewRow();
        gvw5.RowDelete += (s, e) =>
        {
            try { if (gvw5.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };

        // panelWyn1의 공용 추가/삭제 버튼 - 현재 활성 탭의 그리드에 적용(각 그리드 자체
        // EmbeddedNavigator와 별개로, 탭을 안 넘나들어도 되는 지름길).
        btnAddRow1.Click += (s, e) => ActiveDetailView().AddNewRow();
        btnDeletRow1.Click += (s, e) =>
        {
            try { if (ActiveDetailView().GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };

        // 개발자용 마우스오버 툴팁(BindingField) - 실제 적용은 BaseForm.ApplyBindingFieldTooltips가
        // 공통으로 처리한다(Session.IsDeveloper일 때만). 그리드 컬럼은 FieldName이 이미 DB
        // 컬럼명이라 자동 적용되지만, panData 개별 컨트롤은 Tag에 미리 넣어둬야 잡힌다
        // (2026-09-12 - 손으로 만든 화면 여러 개에서 이게 빠져있던 걸 발견하고 템플릿에도 추가함).
        // @AI_BUILDER:BEGIN DETAIL_FORM_TAG
        //txtDetailSample1.Tag = new BindingFieldTag("sample1");
        // @AI_BUILDER:END DETAIL_FORM_TAG

        // 화면종료 시 저장 확인(BaseForm.ConfirmCloseAsync)이 이 추적에 기댄다 - grd2~grd5는
        // QueryCore/EnterNewMode에서 새 DataTable로 바뀔 때마다 그때그때 TrackDirty를 다시 걸어야 한다.
        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) => await QueryClick();
    }

    private GridViewWyn ActiveDetailView() => tabDetailGrids.SelectedTabPage switch
    {
        var t when ReferenceEquals(t, tabDetail2) => gvw3,
        var t when ReferenceEquals(t, tabDetail3) => gvw4,
        var t when ReferenceEquals(t, tabDetail4) => gvw5,
        _ => gvw2
    };

    /// <summary>grd1 선택 단계가 없으므로 조회 = panHeader 조건으로 __QUERY_PROC__를 한 번 부르고,
    /// 그 결과(레코드셋 여러 개 - 0번은 panData용, 1~4번은 grd2~grd5용, 전부 선택사항)를 그대로
    /// 화면에 채우는 것뿐이다. QueryClick(사용자가 직접 누른 조회)과 저장 직후 재조회 둘 다
    /// forceKey 하나로 구분한다 - 저장 직후엔 방금 만들었거나 수정한 문서의 키로 다시 조회해서
    /// 그 값 그대로 되돌려준다(사용자가 입력한 검색조건은 그대로 두고 키만 얹어 보낸다).</summary>
    public override async Task QueryClick() => await QueryCore(forceKey: null);

    private async Task QueryCore(string? forceKey)
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p___MASTER_KEY_COLUMN__"] = forceKey,
            // @AI_BUILDER:BEGIN QUERY_PARAMS
            ["p_sample1"] = txtSearchQ.Text,
            // @AI_BUILDER:END QUERY_PARAMS
        };
        var tables = await QueryMultiAsync("__QUERY_PROC__", p);
        var header = tables.Count > 0 ? tables[0] : new DataTable();
        _detail1 = tables.Count > 1 ? tables[1] : new DataTable();
        _detail2 = tables.Count > 2 ? tables[2] : new DataTable();
        _detail3 = tables.Count > 3 ? tables[3] : new DataTable();
        _detail4 = tables.Count > 4 ? tables[4] : new DataTable();

        if (header.Rows.Count > 0) await OnRowLoadedAsync(header.Rows[0]);
        else EnterNewMode();
    }

    private async Task OnRowLoadedAsync(DataRow row)
    {
        // 코드가 값을 채우는 것뿐인데 TrackDirty(panData)가 "사용자가 고쳤다"로 오인하지 않게 감싼다.
        SuppressDirtyTracking(() =>
        {
            _editingKey = row["__MASTER_KEY_COLUMN__"]?.ToString();
            // @AI_BUILDER:BEGIN DETAIL_FORM_ASSIGN
            //txtDetailSample1.Text = row["sample1"]?.ToString() ?? string.Empty;
            // @AI_BUILDER:END DETAIL_FORM_ASSIGN
        });

        await BindDetailGridsAsync();
    }

    private Task BindDetailGridsAsync()
    {
        TrackDirty(_detail1);
        TrackDirty(_detail2);
        TrackDirty(_detail3);
        TrackDirty(_detail4);
        grd2.DataSource = _detail1;
        grd3.DataSource = _detail2;
        grd4.DataSource = _detail3;
        grd5.DataSource = _detail4;
        return Task.CompletedTask;
    }

    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = null;
            // @AI_BUILDER:BEGIN DETAIL_FORM_CLEAR
            //txtDetailSample1.Text = string.Empty;
            // @AI_BUILDER:END DETAIL_FORM_CLEAR
            _detail1 = _detail1.Clone();
            _detail2 = _detail2.Clone();
            _detail3 = _detail3.Clone();
            _detail4 = _detail4.Clone();
            TrackDirty(_detail1);
            TrackDirty(_detail2);
            TrackDirty(_detail3);
            TrackDirty(_detail4);
            grd2.DataSource = _detail1;
            grd3.DataSource = _detail2;
            grd4.DataSource = _detail3;
            grd5.DataSource = _detail4;
        });
    }

    public override Task NewClick()
    {
        EnterNewMode();
        FocusFirstEntryField(panData); // 사업장 다음 첫 탭오더 컨트롤에 커서(표준)
        return Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        // 저장프로시저가 지정 안 된 조회전용 화면이면 여기서 막는다(TplSingleGrid.cs와 같은 가드) -
        // grd2~grd5 저장도 헤더가 돌려주는 키가 있어야 동작하므로, 헤더가 없으면 저장 전체를 여기서
        // 끊는다.
        if (string.IsNullOrEmpty("__SAVE_PROC__"))
        {
            AppMessageBox.Show("이 화면은 조회전용으로 생성됐습니다 - 저장프로시저가 지정되지 않았습니다.", "저장 불가");
            return;
        }

        // ---- 1) 헤더(panData -> __SAVE_PROC__) ----
        var headerParams = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            // @AI_BUILDER:BEGIN DETAIL_FORM_SAVE_PARAMS
            //["p_sample1"] = txtDetailSample1.Text,
            // @AI_BUILDER:END DETAIL_FORM_SAVE_PARAMS
        };

        var headerResult = await SaveAsync("__SAVE_PROC__", headerParams);
        if (headerResult == null || !headerResult.Success)
        {
            AppMessageBox.Show(headerResult?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        var headerKey = _editingKey ?? headerResult.GeneratedCode;

        // ---- 2~5) 명세(grd2~grd5 -> __SAVE_PROC_1~4__) - 전부 선택사항이다(Save Actions를 안
        // 채우면 빈 문자열로 치환됨) - 저장프로시저가 없으면 그 단계를 건너뛴다(TplMasterFormSubGrid의
        // grd3~grd5 폴백과 같은 원칙, guard 없이 SaveAsync("", ...)를 부르면 서버가 빈 프로시저
        // 이름을 거부하며 원인을 알기 어려운 오류가 난다). ----
        if (!string.IsNullOrEmpty("__SAVE_PROC_1__"))
        {
            var detail1Ok = await SaveDetailRowsAsync(gvw2, _detail1, "__SAVE_PROC_1__", headerKey, (row, version) => new Dictionary<string, string?>
            {
                // @AI_BUILDER:BEGIN DETAIL1_SAVE_PARAMS
                ["p_sample1"] = ProcData.Str(row, "sample1", version),
                // @AI_BUILDER:END DETAIL1_SAVE_PARAMS
            });
            if (!detail1Ok) return;
        }

        if (!string.IsNullOrEmpty("__SAVE_PROC_2__"))
        {
            var detail2Ok = await SaveDetailRowsAsync(gvw3, _detail2, "__SAVE_PROC_2__", headerKey, (row, version) => new Dictionary<string, string?>
            {
                // @AI_BUILDER:BEGIN DETAIL2_SAVE_PARAMS
                ["p_sample1"] = ProcData.Str(row, "sample1", version),
                // @AI_BUILDER:END DETAIL2_SAVE_PARAMS
            });
            if (!detail2Ok) return;
        }

        if (!string.IsNullOrEmpty("__SAVE_PROC_3__"))
        {
            var detail3Ok = await SaveDetailRowsAsync(gvw4, _detail3, "__SAVE_PROC_3__", headerKey, (row, version) => new Dictionary<string, string?>
            {
                // @AI_BUILDER:BEGIN DETAIL3_SAVE_PARAMS
                ["p_sample1"] = ProcData.Str(row, "sample1", version),
                // @AI_BUILDER:END DETAIL3_SAVE_PARAMS
            });
            if (!detail3Ok) return;
        }

        if (!string.IsNullOrEmpty("__SAVE_PROC_4__"))
        {
            var detail4Ok = await SaveDetailRowsAsync(gvw5, _detail4, "__SAVE_PROC_4__", headerKey, (row, version) => new Dictionary<string, string?>
            {
                // @AI_BUILDER:BEGIN DETAIL4_SAVE_PARAMS
                ["p_sample1"] = ProcData.Str(row, "sample1", version),
                // @AI_BUILDER:END DETAIL4_SAVE_PARAMS
            });
            if (!detail4Ok) return;
        }

        _editingKey ??= headerResult.GeneratedCode;
        Toast.Show("저장되었습니다.");
        await QueryCore(forceKey: _editingKey); // 방금 저장한 문서 유지 - QueryClick(사용자 조회)과 다른 경로
    }

    /// <summary>grd2~grd5 공통 저장 루프 - 변경된 행마다 N/U/D로 나눠 저장한다(SingleGrid.SaveClick과
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
                ["p___MASTER_KEY_COLUMN__"] = masterKey,
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
            ["p___MASTER_KEY_COLUMN__"] = _editingKey,
        };
        var result = await SaveAsync("__SAVE_PROC__", p);
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "삭제에 실패했습니다.", "삭제 실패");
            return;
        }

        _editingKey = null;
        Toast.Show("삭제되었습니다.");
        EnterNewMode();
    }
}
