// @AI_BUILDER:BEGIN FILE_HEADER
// AI Builder 트리마스터-서브그리드 템플릿 원본 - 이 파일 자체는 실행되지 않는다(어떤 메뉴에도
// 등록돼 있지 않음). VS에서 열어 로직 골격을 확인/조정하는 용도. TplMasterFormSubGrid와 완전히
// 같은 구조(tree1/panData+하위그리드 탭 1~4개, grd3/grd4/grd5는 선택사항)인데 grd1(평범한 목록)
// 대신 tree1(TreeListWyn, 자기참조 계층 데이터)이 마스터다(2026-09-08 - TplMasterFormSubGrid를
// 복사해서 만듦, 부서/메뉴처럼 상위-하위 구조를 가진 목록이 마스터인 화면용).
// @AI_BUILDER:END FILE_HEADER
using System.Data;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.TEMPLATE;

public partial class TplTreeMasterSubGrid : BaseForm
{
    private DataTable _list = new();
    private DataTable _detail1 = new();
    private DataTable _detail2 = new();
    private DataTable _detail3 = new();
    private DataTable _detail4 = new();
    private string? _editingKey; // null이면 신규모드

    public TplTreeMasterSubGrid()
    {
        InitializeComponent();

        Text = "__MENU_CAPTION__";

        Controls.Add(BuildScreenHeader());

        // tree1은 자기참조 계층 데이터를 그린다 - KeyFieldName은 grd1의 __MASTER_KEY_COLUMN__과
        // 같은 역할(행 식별), ParentFieldName은 이 템플릿에만 있는 값으로 "이 행의 상위 행"을
        // 가리키는 컬럼이다(예: 부서 테이블의 UPPER_DEPT_ID). 둘 다 QuerySources에서 고른 grd1
        // 컬럼 목록에 실제로 있는 이름이어야 한다.
        tree1.KeyFieldName = "__MASTER_KEY_COLUMN__";
        tree1.ParentFieldName = "__MASTER_PARENT_COLUMN__";
        // 다른 마스터 노드를 고를 때 panData/grd2/grd3에 저장 안 된 변경이 있으면 먼저 확인한다
        // (BaseForm.ConfirmMasterRowSwitch의 TreeList 오버로드 - GridView 버전과 완전히 같은 원리,
        // 2026-09-08. 새 화면을 이 템플릿에서 복제하면 기본으로 따라온다. 지우지 말 것). 이름 있는
        // 메서드로 등록해야 QueryCore가 tree1을 다시 그리는 동안 잠깐 구독을 끊을 수 있다(바로 아래
        // Tree1_FocusedNodeChanged 설명 참고) - 람다로 등록하면 나중에 뗄 방법이 없다.
        tree1.FocusedNodeChanged += Tree1_FocusedNodeChanged;

        // grd2/grd3는 MasterFormSubGrid와 마찬가지로 편집 가능하다 - 각자 자기 저장프로시저
        // (__SAVE_PROC_1__/__SAVE_PROC_2__)로 저장되기 때문. Role=Edit이면 그리드 자신의
        // EmbeddedNavigator에도 추가/삭제 버튼이 뜬다(탭당 하나씩, 독립 동작).
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
        // 공통으로 처리한다(Session.IsDeveloper일 때만). 그리드/트리 컬럼은 FieldName이 이미 DB
        // 컬럼명이라 자동 적용되지만, panData 개별 컨트롤은 Tag에 미리 넣어둬야 잡힌다
        // (2026-09-12 - 손으로 만든 화면 여러 개에서 이게 빠져있던 걸 발견하고 템플릿에도 추가함).
        // @AI_BUILDER:BEGIN DETAIL_FORM_TAG
        //txtDetailSample1.Tag = new BindingFieldTag("sample1");
        // @AI_BUILDER:END DETAIL_FORM_TAG

        // 화면종료 시 저장 확인(BaseForm.ConfirmCloseAsync)과 다른 마스터 노드로 옮길 때 확인
        // (ConfirmMasterRowSwitch) 둘 다 이 추적에 기댄다 - grd2/grd3/grd4/grd5(편집 가능한 하위
        // 그리드)는 EnterNewMode/LoadDetailAsync에서 새 DataTable로 바뀔 때마다 그때그때
        // TrackDirty(_detail1..4)를 다시 걸어야 한다.
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

    /// <summary>사용자가 직접 누른 조회(preserveSelection: false)는 새 검색이므로 첫 노드부터,
    /// 저장/삭제 뒤의 내부 재조회(preserveSelection: true)는 방금 편집하던 노드에 포커스를 되돌려
    /// panData/grd2/grd3까지 그 값 그대로 다시 채운다(TplMasterFormSubGrid.QueryCore와 같은 원칙).
    ///
    /// GridView와 달리 TreeList는 DataSource를 바꿔도 스스로 첫 노드에 포커스를 주지 않는다(직접
    /// 확인 필요 없이 안전하게 가정할 수 없어서 명시적으로 처리) - editingKey가 없을 때도
    /// tree1.Nodes[0]로 명시적으로 포커스를 줘야 panData가 첫 조회에서부터 채워진다(그리드
    /// 템플릿들이 겪었던 "최초 조회 시 panData 안 채워짐" 버그와 같은 함정을 여기서는 애초에
    /// 피한다).</summary>
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

        // 구독을 잠깐 끊는 이유는 GridView 버전(TplMasterFormSubGrid.QueryCore 주석 참고)과 동일 -
        // SuppressMasterRowSwitchConfirm은 "확인창"만 막지, 안에서 tree1.FocusedNode를 바꾸면
        // FocusedNodeChanged 자체는 그대로 발생해서 중복 호출이 겹칠 수 있다.
        tree1.FocusedNodeChanged -= Tree1_FocusedNodeChanged;
        SuppressMasterRowSwitchConfirm(tree1, () =>
        {
            tree1.DataSource = _list;
            tree1.ExpandAll();
            if (editingKey != null)
            {
                var node = FindTreeNode(tree1.Nodes, editingKey);
                if (node != null) tree1.FocusedNode = node;
            }
            else if (tree1.Nodes.Count > 0)
            {
                tree1.FocusedNode = tree1.Nodes[0];
            }
        });
        tree1.FocusedNodeChanged += Tree1_FocusedNodeChanged;

        var row = editingKey == null ? null : FindRow(editingKey);
        if (row != null) await OnMasterSelectedAsync(row);
        else if (tree1.FocusedNode != null && tree1.GetDataRecordByNode(tree1.FocusedNode) is DataRowView focusedView) await OnMasterSelectedAsync(focusedView.Row);
        else EnterNewMode();
    }

    private void Tree1_FocusedNodeChanged(object? sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e) =>
        ConfirmMasterRowSwitch(tree1, e, row => _ = OnMasterSelectedAsync(row.Row));

    /// <summary>조회된 목록에서 키(__MASTER_KEY_COLUMN__)로 행을 찾는다. 없으면 null.</summary>
    private DataRow? FindRow(string key) =>
        _list.Rows.Cast<DataRow>()
            .FirstOrDefault(r => string.Equals(Convert.ToString(r["__MASTER_KEY_COLUMN__"]), key, StringComparison.OrdinalIgnoreCase));

    /// <summary>키로 tree1의 노드를 찾는다(계층 전체를 재귀 탐색) - GridView 버전의 FindRowHandle과
    /// 같은 역할. FindNodeByKeyID 대신 직접 재귀 탐색하는 이유: 그 메서드가 키 값을 어떤 타입으로
    /// 비교하는지(예: 문자열 "123"과 실제 컬럼의 bigint 123을 같다고 볼지) 확인된 바가 없어서, 이미
    /// 검증된 방식(FindRow와 동일하게 Convert.ToString 비교)만 쓴다.</summary>
    private static TreeListNode? FindTreeNode(TreeListNodes nodes, string key)
    {
        foreach (TreeListNode node in nodes)
        {
            if (string.Equals(Convert.ToString(node.GetValue("__MASTER_KEY_COLUMN__")), key, StringComparison.OrdinalIgnoreCase))
                return node;

            var found = FindTreeNode(node.Nodes, key);
            if (found != null) return found;
        }
        return null;
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

    /// <summary>선택된 마스터 노드의 하위 목록 최대 4개(grd2/grd3/grd4/grd5)를 한 번의 호출로 같이
    /// 조회한다 - __DETAIL_QUERY_PROC__가 work_type='__DETAIL_WORK_TYPE__'일 때 레코드셋을
    /// 순서대로(grd2용, grd3용, grd4용, grd5용) 반환하기 때문에 QueryMultiAsync를 쓴다(QueryAsync는
    /// 첫 레코드셋만 받음). grd3/grd4/grd5는 전부 선택사항이라 프로시저가 그만큼 레코드셋을 안
    /// 돌려줘도(Count가 모자라도) 빈 DataTable로 채워질 뿐 에러가 안 난다.
    ///
    /// grd2까지 포함해 하위그리드가 아예 없는 화면(순수 마스터 목록만 필요한 경우)도 생성될 수
    /// 있다(2026-09-11 요청 - "바인딩 정보가 없어도 그냥 생성되어야 해") - 그러면
    /// __DETAIL_QUERY_PROC__ 자체가 빈 문자열로 치환되므로, 빈 프로시저명으로 서버를 부르는 대신
    /// 여기서 막고 하위그리드 전부를 빈 상태로 둔다(SaveClick의 __SAVE_PROC__ 빈 문자열 체크와
    /// 같은 가드 패턴).</summary>
    private async Task LoadDetailAsync()
    {
        if (string.IsNullOrEmpty("__DETAIL_QUERY_PROC__"))
        {
            grd2.DataSource = null;
            grd3.DataSource = null;
            grd4.DataSource = null;
            grd5.DataSource = null;
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

        // ---- 2) 명세1(grd2 -> __SAVE_PROC_1__) - grd3과 마찬가지로 선택사항이다(TplMasterFormSubGrid
        // 와 같은 원칙 - 저장프로시저가 없으면 이 단계를 건너뛴다). ----
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

        // ---- 3) 명세2(grd3 -> __SAVE_PROC_2__) - 선택사항. ----
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

        // ---- 4) 명세3(grd4 -> __SAVE_PROC_3__) - 선택사항. ----
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
        await QueryCore(preserveSelection: true); // 방금 저장한 노드 유지 - QueryClick(사용자 조회)과 다른 경로
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
