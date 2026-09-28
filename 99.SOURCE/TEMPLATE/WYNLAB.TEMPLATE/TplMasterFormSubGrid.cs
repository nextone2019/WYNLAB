// @AI_BUILDER:BEGIN FILE_HEADER
// AI Builder 마스터-폼-서브그리드 템플릿 원본 - 이 파일 자체는 실행되지 않는다(어떤 메뉴에도
// 등록돼 있지 않음). VS에서 열어 로직 골격을 확인/조정하는 용도. 하위그리드(grd2/grd3/grd4/grd5)는
// 탭으로 묶여 있고 grd3/grd4/grd5는 전부 선택사항이다 - 필요한 개수만큼만 남기고 나머지는 생성 후
// 그 탭/저장액션만 지우면 된다(2026-09-08 - grd4/grd5 두 개를 추가해서 최대 4개까지 늘렸다. 예전엔
// grd2 하나만 조회전용으로 만드는 별도 템플릿이 있었는데 이 템플릿이 그 쓰임새를 포함해서 통합).
// @AI_BUILDER:END FILE_HEADER
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.TEMPLATE;

public partial class TplMasterFormSubGrid : BaseForm
{
    private DataTable _list = new();
    private DataTable _detail1 = new();
    private DataTable _detail2 = new();
    private DataTable _detail3 = new();
    private DataTable _detail4 = new();
    private string? _editingKey; // null이면 신규모드

    public TplMasterFormSubGrid()
    {
        InitializeComponent();

        Text = "__MENU_CAPTION__";

        Controls.Add(BuildScreenHeader());

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        // 다른 마스터 행을 고를 때 panData/grd2/grd3/grd4/grd5에 저장 안 된 변경이 있으면 먼저 확인한다
        // (BaseForm.ConfirmMasterRowSwitch 참고, 2026-09-06 - 모든 화면 공통 적용. 새 화면을 이
        // 템플릿에서 복제하면 기본으로 따라온다. 지우지 말 것). 이름 있는 메서드로 등록해야
        // QueryCore가 grd1을 다시 그리는 동안 잠깐 구독을 끊을 수 있다(바로 아래 Gvw1_
        // FocusedRowObjectChanged 설명 참고) - 람다로 등록하면 나중에 뗄 방법이 없다.
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        // grd2/grd3는 MasterFormSubGrid의 grd2(조회전용)와 달리 편집 가능하다 - 각자 자기
        // 저장프로시저(__SAVE_PROC_1__/__SAVE_PROC_2__)로 저장되기 때문. Role=Edit이면 그리드
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
        btnAddRow2.Click += (s, e) => ActiveDetailView().AddNewRow();
        btnDeletRow2.Click += (s, e) =>
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

        // 화면종료 시 저장 확인(BaseForm.ConfirmCloseAsync)과 다른 마스터 행으로 옮길 때 확인
        // (ConfirmMasterRowSwitch) 둘 다 이 추적에 기댄다 - grd2/grd3(편집 가능한 하위 그리드)는
        // EnterNewMode/LoadDetailAsync에서 새 DataTable로 바뀔 때마다 그때그때 TrackDirty(_detail1)/
        // TrackDirty(_detail2)를 다시 걸어야 한다.
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

    public override async Task QueryClick() => await QueryCore(preserveSelection: false);

    /// <summary>사용자가 직접 누른 조회(preserveSelection: false)는 새 검색이므로 0번 행부터,
    /// 저장/삭제 뒤의 내부 재조회(preserveSelection: true)는 방금 편집하던 행에 포커스를 되돌려
    /// panData/grd2/grd3까지 그 값 그대로 다시 채운다 - 안 그러면 저장 직후 조회했을 때 panData가
    /// 안 채워진 것처럼 보인다(2026-09-06 요청, frmCust/frmMinorCode/frmAcc 등과 같은 패턴 - 새
    /// 화면을 이 템플릿에서 복제하면 기본으로 따라온다. 지우지 말 것).</summary>
    private async Task QueryCore(bool preserveSelection)
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            // @AI_BUILDER:BEGIN QUERY_PARAMS
            ["p_sample1"] = txtSearchQ.Text,
            // @AI_BUILDER:END QUERY_PARAMS
        };
        _list = await QueryAsync("__QUERY_PROC__", p);

        var editingKey = preserveSelection ? _editingKey : null;

        // 저장 직후(SaveClick -> QueryCore)엔 아직 IsDirty가 true로 남아있을 수 있다(DataTable.
        // AcceptChanges()는 RowChanged를 안 냄) - 이 재바인딩이 그 상태에서 자동으로 0번 행에
        // 포커스를 주면 ConfirmMasterRowSwitch가 "변경사항이 있다"고 또 확인창을 띄우는 오작동이
        // 생긴다. 아래에서 직접 EnterNewMode/OnMasterSelectedAsync를 호출해 최종 상태를 맞추므로
        // 이 재바인딩 구간만 조용히 지나가면 된다.
        //
        // 구독을 잠깐 끊는 이유(2026-09-08 실제 발견): SuppressMasterRowSwitchConfirm은 "확인창"만
        // 막지, 안에서 gvw1.FocusedRowHandle을 바꾸면 FocusedRowObjectChanged 자체는 그대로
        // 발생한다 - 그 이벤트가 다시 OnMasterSelectedAsync를 fire-and-forget으로 부르고, 바로
        // 아래에서 같은 행에 대해 또 한 번(이번엔 await로) OnMasterSelectedAsync를 부른다. 저장
        // 직후처럼 그 두 호출이 겹치면(둘 다 grd2/grd3용 LoadDetailAsync까지 비동기로 이어짐)
        // 나중에 끝나는 쪽이 먼저 끝난 쪽의 TrackDirty 구독/그리드 바인딩을 덮어써서, panData를
        // 닫아도 될 상태로 다시 정리했다고 여겼던 IsDirty가 조용히 다시 true가 되는 경우가
        // 있었다(저장 버튼을 누르고 바로 탭을 닫아도 "변경 내역이 존재합니다" 확인창이 뜸). 아래
        // 명시적 호출 하나만으로 충분하므로, 재바인딩 구간에서는 이벤트를 끊어 중복 호출 자체를
        // 없앤다(frmAcc.cs가 이미 쓰던 패턴).
        gvw1.FocusedRowObjectChanged -= Gvw1_FocusedRowObjectChanged;
        SuppressMasterRowSwitchConfirm(gvw1, () =>
        {
            grd1.DataSource = _list;
            if (editingKey != null)
            {
                var handle = FindRowHandle(editingKey);
                if (handle != null) gvw1.FocusedRowHandle = handle.Value;
            }
        });
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        var row = editingKey == null ? null : FindRow(editingKey);
        if (row != null) await OnMasterSelectedAsync(row);
        // editingKey가 없으면(최초 조회, preserveSelection=false) grd1을 바인딩한 직후 DevExpress가
        // 스스로 0번 행에 포커스를 준다 - 그 자동 포커스 행을 그대로 panData에 채운다. 여기서
        // EnterNewMode()로 바로 넘어가면(예전 버전의 실제 버그) 목록엔 데이터가 있는데 처음 눌러보기
        // 전까지 panData는 계속 비어있는 것처럼 보인다(2026-09-08 실제 발견 - frmItem 최초 조회 시
        // panData 안 채워짐). frmAcc.cs가 이미 쓰던 패턴 그대로 가져온다.
        else if (gvw1.GetFocusedRow() is DataRowView focusedView) await OnMasterSelectedAsync(focusedView.Row);
        else EnterNewMode();
    }

    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e) =>
        ConfirmMasterRowSwitch(gvw1, e, row => _ = OnMasterSelectedAsync(row.Row));

    /// <summary>조회된 목록에서 키(__MASTER_KEY_COLUMN__)로 행을 찾는다. 없으면 null.</summary>
    private DataRow? FindRow(string key) =>
        _list.Rows.Cast<DataRow>()
            .FirstOrDefault(r => string.Equals(Convert.ToString(r["__MASTER_KEY_COLUMN__"]), key, StringComparison.OrdinalIgnoreCase));

    /// <summary>키(__MASTER_KEY_COLUMN__)로 grd1의 행 핸들을 찾는다. 없으면 null. 컬럼 셀 값으로
    /// 찾는 방식(gvw1.Columns[fieldName] + GetRowCellValue)은 그 키 컬럼이 화면에 안 보이는 숨김
    /// 컬럼(Visible=false)일 때 안 먹힌다(2026-09-11 실제 발견 - 저장 후 재조회하면 선택된 행이
    /// 안 돌아오고 항상 0번 행으로 가던 버그, frmEMP의 emp_id처럼 PK 컬럼을 그리드에 안 보이게
    /// 둔 화면에서 재현됨). FindRow로 찾은 DataRow의 DataTable상 인덱스를 GridView.GetRowHandle로
    /// 표시 행 핸들로 변환하면 키 컬럼이 보이든 안 보이든 항상 동작한다 - 앞으로 이 템플릿에서
    /// 복제되는 모든 화면에 자동으로 적용된다.</summary>
    private int? FindRowHandle(string key)
    {
        var row = FindRow(key);
        if (row == null) return null;

        var rowIndex = _list.Rows.IndexOf(row);
        var handle = gvw1.GetRowHandle(rowIndex);
        return handle >= 0 ? handle : null;
    }

    private async Task OnMasterSelectedAsync(DataRow row)
    {
        // 코드가 값을 채우는 것뿐인데 TrackDirty(panData)가 "사용자가 고쳤다"로 오인하지 않게 감싼다.
        SuppressDirtyTracking(() =>
        {
            _editingKey = row["__MASTER_KEY_COLUMN__"]?.ToString();
            // @AI_BUILDER:BEGIN DETAIL_FORM_ASSIGN
            //txtDetailSample1.Text = row["sample1"]?.ToString() ?? string.Empty;
            // @AI_BUILDER:END DETAIL_FORM_ASSIGN
        });

        await LoadDetailAsync();
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

    /// <summary>선택된 마스터 행의 하위 목록 최대 4개(grd2/grd3/grd4/grd5)를 한 번의 호출로 같이
    /// 조회한다 - __DETAIL_QUERY_PROC__가 work_type='__DETAIL_WORK_TYPE__'일 때 레코드셋을
    /// 순서대로(grd2용, grd3용, grd4용, grd5용) 반환하기 때문에 QueryMultiAsync를 쓴다(QueryAsync는
    /// 첫 레코드셋만 받음). grd2/grd3/grd4/grd5는 전부 선택사항이다(2026-09-09 - grd1+panData만
    /// 있는 화면도 이 템플릿으로 만들 수 있게 grd2까지 선택사항으로 풀었다) - 프로시저가 그만큼
    /// 레코드셋을 안 돌려줘도(Count가 모자라도) 빈 DataTable로 채워질 뿐 에러가 안 난다. 하위
    /// 그리드가 하나도 없으면(__DETAIL_QUERY_PROC__ 자체가 빈 문자열) 호출 자체를 건너뛴다 -
    /// 빈 프로시저 이름으로 QueryMultiAsync를 부르면 서버가 이를 거부하며 원인을 알기 어려운
    /// 오류가 난다(TplMasterFormSubGrid.SaveClick의 __SAVE_PROC_1~4__ 빈 문자열 가드와 같은 이유).</summary>
    private async Task LoadDetailAsync()
    {
        if (string.IsNullOrEmpty("__DETAIL_QUERY_PROC__"))
        {
            _detail1 = new DataTable();
            _detail2 = new DataTable();
            _detail3 = new DataTable();
            _detail4 = new DataTable();
            TrackDirty(_detail1);
            TrackDirty(_detail2);
            TrackDirty(_detail3);
            TrackDirty(_detail4);
            grd2.DataSource = _detail1;
            grd3.DataSource = _detail2;
            grd4.DataSource = _detail3;
            grd5.DataSource = _detail4;
            return;
        }

        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "__DETAIL_WORK_TYPE__",
            ["p___DETAIL_KEY_PARAM__"] = _editingKey,
        };
        var tables = await QueryMultiAsync("__DETAIL_QUERY_PROC__", p);
        _detail1 = tables.Count > 0 ? tables[0] : new DataTable();
        _detail2 = tables.Count > 1 ? tables[1] : new DataTable();
        _detail3 = tables.Count > 2 ? tables[2] : new DataTable();
        _detail4 = tables.Count > 3 ? tables[3] : new DataTable();
        TrackDirty(_detail1);
        TrackDirty(_detail2);
        TrackDirty(_detail3);
        TrackDirty(_detail4);
        grd2.DataSource = _detail1;
        grd3.DataSource = _detail2;
        grd4.DataSource = _detail3;
        grd5.DataSource = _detail4;
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
        // grd2/grd3 저장도 헤더가 돌려주는 키(headerKey)가 있어야 동작하므로, 헤더가 없으면
        // 저장 전체를 여기서 끊는다.
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

        // ---- 2) 명세1(grd2 -> __SAVE_PROC_1__) - grd2도 grd3과 마찬가지로 선택사항이다(AI
        // Builder에서 Save Actions를 안 채우면 빈 문자열로 치환됨) - 저장프로시저가 없으면 이
        // 단계를 건너뛴다. grd2는 대개 저장까지 채우는 경우가 많아 guard가 없어도 되는 것처럼
        // 보였지만, grd3처럼 조회전용(Save Actions 미설정)으로만 쓰는 경우도 있어서 guard 없이
        // SaveAsync("", ...)를 그대로 호출하면 서버가 빈 프로시저 이름을 거부하며 원인을 알기
        // 어려운 오류가 난다(2026-09-07 실제 발견 - frmItem333 grd2에서 재현). MasterFormSubGrid의
        // panData 조회전용 폴백과 같은 원칙. ----
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

        // ---- 3) 명세2(grd3 -> __SAVE_PROC_2__) - grd3은 선택사항이라(AI Builder에서 안 채우면
        // 빈 문자열로 치환됨) 저장프로시저가 없으면 이 단계를 건너뛴다(MasterFormSubGrid의 panData
        // 조회전용 폴백과 같은 원칙 - "지우지 말 것" 주석이 아니라 실제로 이 화면에 grd3이 없는
        // 정상적인 경우다). ----
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

        // ---- 4) 명세3(grd4 -> __SAVE_PROC_3__) - grd3/grd4/grd5와 같은 원칙으로 선택사항이다. ----
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

        // ---- 5) 명세4(grd5 -> __SAVE_PROC_4__) - 선택사항. ----
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
        await QueryCore(preserveSelection: true); // 방금 저장한 행 유지 - QueryClick(사용자 조회)과 다른 경로
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
        await QueryClick();
    }
}
