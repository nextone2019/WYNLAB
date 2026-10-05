// AI Builder가 트리마스터-서브그리드 템플릿을 복제해서 자동 생성 - 2026-09-11.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
using System.Data;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmItemGrp : BaseForm
{
    private DataTable _list = new();
    private DataTable _detail1 = new();
    private DataTable _detail2 = new();
    private DataTable _detail3 = new();
    private DataTable _detail4 = new();
    private string? _editingKey; // null이면 신규모드
    private bool _syncingLevel; // 그룹 레벨 값을 코드가 채우는 중 - 레벨 변경 처리(상위그룹 비우기)를 건너뛴다

    public frmItemGrp()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준(2026-10-03): 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "품목그룹등록";


        // tree1은 자기참조 계층 데이터를 그린다 - KeyFieldName은 grd1의 grp_id과
        // 같은 역할(행 식별), ParentFieldName은 이 템플릿에만 있는 값으로 "이 행의 상위 행"을
        // 가리키는 컬럼이다(예: 부서 테이블의 UPPER_DEPT_ID). 둘 다 QuerySources에서 고른 grd1
        // 컬럼 목록에 실제로 있는 이름이어야 한다.
        tree1.KeyFieldName = "grp_id";
        tree1.ParentFieldName = "par_grp_id";
        // 다른 마스터 노드를 고를 때 panData/grd2/grd3에 저장 안 된 변경이 있으면 먼저 확인한다
        // (BaseForm.ConfirmMasterRowSwitch의 TreeList 오버로드 - GridView 버전과 완전히 같은 원리,
        // 2026-09-08. 새 화면을 이 템플릿에서 복제하면 기본으로 따라온다. 지우지 말 것). 이름 있는
        // 메서드로 등록해야 QueryCore가 tree1을 다시 그리는 동안 잠깐 구독을 끊을 수 있다(바로 아래
        // Tree1_FocusedNodeChanged 설명 참고) - 람다로 등록하면 나중에 뗄 방법이 없다.
        tree1.FocusedNodeChanged += Tree1_FocusedNodeChanged;

        // grd2/grd3는 MasterFormSubGrid와 마찬가지로 편집 가능하다 - 각자 자기 저장프로시저
        // (/)로 저장되기 때문. Role=Edit이면 그리드 자신의
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
        // (2026-09-12 감사 - 이 화면엔 원래 빠져있었음).
        cboDetailAccId.Tag = new BindingFieldTag("acc_id");
        txtDetailGrpId.Tag = new BindingFieldTag("grp_id");
        txtDetailGrpNm.Tag = new BindingFieldTag("grp_nm");
        cboDetailGrpLvl.Tag = new BindingFieldTag("grp_lvl");
        cboDetailParGrpId.Tag = new BindingFieldTag("par_grp_id");
        memDetailRemark.Tag = new BindingFieldTag("remark");

        // 화면종료 시 저장 확인(BaseForm.ConfirmCloseAsync)과 다른 마스터 노드로 옮길 때 확인
        // (ConfirmMasterRowSwitch) 둘 다 이 추적에 기댄다 - grd2/grd3/grd4/grd5(편집 가능한 하위
        // 그리드)는 EnterNewMode/LoadDetailAsync에서 새 DataTable로 바뀔 때마다 그때그때
        // TrackDirty(_detail1..4)를 다시 걸어야 한다.
        // 상위그룹 콤보 - 선택한 그룹 레벨의 바로 위 레벨 그룹만 보인다(L_ITEM_GRP, 파라미터를 채운 "뒤에" LookupKey 지정 - frmItem 품목그룹 콤보와 같은 이유).
        // 그룹 레벨을 고르면 그 레벨-1로 목록을 바꾸고 기존 선택은 비운다. 1레벨은 상위그룹이 없으므로 콤보를 막는다(서버도 같은 규칙, 293).
        cboDetailParGrpId.SetParam("p_grp_lvl", "1");
        cboDetailParGrpId.SetParam("p_par_grp_id", string.Empty);
        cboDetailParGrpId.LookupKey = "L_ITEM_GRP";
        cboDetailGrpLvl.EditValueChanged += (s, e) => { if (!_syncingLevel) ApplyGrpLevel(clearParent: true); };

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
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "Q",
            //["p_grp_id"] = txtGrpId.Text,
            ["p_grp_nm"] = txtGrpNm_Q.Text,
        };
        _list = await QueryAsync("USP_BA_ITEMGRP_Q", p);

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

    /// <summary>조회된 목록에서 키(grp_id)로 행을 찾는다. 없으면 null.</summary>
    private DataRow? FindRow(string key) =>
        _list.Rows.Cast<DataRow>()
            .FirstOrDefault(r => string.Equals(Convert.ToString(r["grp_id"]), key, StringComparison.OrdinalIgnoreCase));

    /// <summary>키로 tree1의 노드를 찾는다(계층 전체를 재귀 탐색) - GridView 버전의 FindRowHandle과
    /// 같은 역할. FindNodeByKeyID 대신 직접 재귀 탐색하는 이유: 그 메서드가 키 값을 어떤 타입으로
    /// 비교하는지(예: 문자열 "123"과 실제 컬럼의 bigint 123을 같다고 볼지) 확인된 바가 없어서, 이미
    /// 검증된 방식(FindRow와 동일하게 Convert.ToString 비교)만 쓴다.</summary>
    private static TreeListNode? FindTreeNode(TreeListNodes nodes, string key)
    {
        foreach (TreeListNode node in nodes)
        {
            if (string.Equals(Convert.ToString(node.GetValue("grp_id")), key, StringComparison.OrdinalIgnoreCase))
                return node;

            var found = FindTreeNode(node.Nodes, key);
            if (found != null) return found;
        }
        return null;
    }

    /// <summary>그룹 레벨에 맞춰 상위그룹 콤보를 맞춘다 - 2~4레벨은 (레벨-1) 그룹 목록 + 필수, 1레벨/미선택은 비활성.</summary>
    private void ApplyGrpLevel(bool clearParent)
    {
        var lvl = int.TryParse(cboDetailGrpLvl.EditValue?.ToString(), out var n) ? n : 0;
        var needParent = lvl >= 2;
        cboDetailParGrpId.Enabled = needParent;
        cboDetailParGrpId.Required = needParent;
        if (clearParent) cboDetailParGrpId.EditValue = null!;
        if (needParent) cboDetailParGrpId.SetParam("p_grp_lvl", (lvl - 1).ToString());
    }

    /// <summary>선택한 그룹의 상위그룹 값을 콤보에 채운다 - 목록(레벨-1)이 실제로 다시 채워진 뒤에 값을 넣어야 표시 텍스트가 붙는다. 상위 없음(0)은 빈 값.</summary>
    private async Task LoadParentAsync(DataRow row)
    {
        var lvl = int.TryParse(row["grp_lvl"]?.ToString(), out var n) ? n : 0;
        cboDetailParGrpId.Enabled = lvl >= 2;
        cboDetailParGrpId.Required = lvl >= 2;
        if (lvl >= 2) await cboDetailParGrpId.SetParamAsync("p_grp_lvl", (lvl - 1).ToString());

        var par = row["par_grp_id"]?.ToString();
        SuppressDirtyTracking(() => cboDetailParGrpId.EditValue = string.IsNullOrEmpty(par) || par == "0" ? null! : par);
    }

    private async Task OnMasterSelectedAsync(DataRow row)
    {
        _syncingLevel = true;
        try
        {
        // 코드가 값을 채우는 것뿐인데 TrackDirty(panData)가 "사용자가 고쳤다"로 오인하지 않게 감싼다.
        SuppressDirtyTracking(() =>
        {
            _editingKey = row["grp_id"]?.ToString();
        cboDetailAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
        txtDetailGrpId.Text = row["grp_id"]?.ToString() ?? string.Empty;
        txtDetailGrpNm.Text = row["grp_nm"]?.ToString() ?? string.Empty;
        cboDetailGrpLvl.EditValue = row["grp_lvl"]?.ToString() ?? string.Empty;
        memDetailRemark.Text = row["remark"]?.ToString() ?? string.Empty;
        });

        await LoadParentAsync(row);
        }
        finally { _syncingLevel = false; }

        await LoadDetailAsync();
    }

    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = null;
        // 신규입력 시 사업장을 매번 고르게 하지 않고 로그인 세션의 사업장을 기본값으로 채운다
        // (2026-09-11 - 모든 입력화면 공통 표준, frmEmp에서 먼저 적용됐던 것과 동일) - 필요하면
        // 사용자가 직접 다른 사업장으로 바꿀 수 있다.
        cboDetailAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        txtDetailGrpId.Text = string.Empty;
        txtDetailGrpNm.Text = string.Empty;
        _syncingLevel = true;
        try { cboDetailGrpLvl.EditValue = string.Empty; }
        finally { _syncingLevel = false; }
        ApplyGrpLevel(clearParent: true); // 레벨 미선택 - 상위그룹 콤보는 비활성
        memDetailRemark.Text = string.Empty;
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
    /// 조회한다 - 가 work_type=''일 때 레코드셋을
    /// 순서대로(grd2용, grd3용, grd4용, grd5용) 반환하기 때문에 QueryMultiAsync를 쓴다(QueryAsync는
    /// 첫 레코드셋만 받음). grd3/grd4/grd5는 전부 선택사항이라 프로시저가 그만큼 레코드셋을 안
    /// 돌려줘도(Count가 모자라도) 빈 DataTable로 채워질 뿐 에러가 안 난다.
    ///
    /// grd2까지 포함해 하위그리드가 아예 없는 화면(순수 마스터 목록만 필요한 경우)도 생성될 수
    /// 있다(2026-09-11 요청 - "바인딩 정보가 없어도 그냥 생성되어야 해") - 그러면
    ///  자체가 빈 문자열로 치환되므로, 빈 프로시저명으로 서버를 부르는 대신
    /// 여기서 막고 하위그리드 전부를 빈 상태로 둔다(SaveClick의 USP_BA_ITEMGRP_S 빈 문자열 체크와
    /// 같은 가드 패턴).</summary>
    private async Task LoadDetailAsync()
    {
        // 하위 그리드는 grd2 하나만 쓴다 - 선택한 그룹에 속한 품목 리스트(조회 전용, USP_BA_ITEMGRP_Q Q1). 그룹을 고를 때마다 다시 조회한다.
        if (_editingKey == null)
        {
            _detail1 = new DataTable();
            grd2.DataSource = null;
            sectionHeaderWyn2.Text = "소속 품목 LIST";
            return;
        }

        var key = _editingKey;
        var table = await QueryAsync("USP_BA_ITEMGRP_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q1",
            ["p_acc_id"] = cboDetailAccId.EditValue?.ToString(),
            ["p_grp_id"] = key,
        });
        if (key != _editingKey) return; // 조회하는 사이 다른 그룹으로 옮겼으면 이 결과는 버린다

        _detail1 = table;
        grd2.DataSource = _detail1;
        sectionHeaderWyn2.Text = $"소속 품목 LIST ({_detail1.Rows.Count}건)";
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        // 저장프로시저가 지정 안 된 조회전용 화면이면 여기서 막는다(TplSingleGrid.cs와 같은 가드) -
        // grd2/grd3 저장도 헤더가 돌려주는 키(headerKey)가 있어야 동작하므로, 헤더가 없으면
        // 저장 전체를 여기서 끊는다.
        if (string.IsNullOrEmpty("USP_BA_ITEMGRP_S"))
        {
            AppMessageBox.Show("이 화면은 조회전용으로 생성됐습니다 - 저장프로시저가 지정되지 않았습니다.", "저장 불가");
            return;
        }

        // ---- 1) 헤더(panData -> USP_BA_ITEMGRP_S) ----
        var headerParams = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_acc_id"] = cboDetailAccId.EditValue?.ToString() ?? string.Empty,
            ["p_grp_id"] = txtDetailGrpId.Text,
            ["p_grp_nm"] = txtDetailGrpNm.Text,
            ["p_grp_lvl"] = cboDetailGrpLvl.EditValue?.ToString() ?? string.Empty,
            ["p_par_grp_id"] = cboDetailParGrpId.EditValue?.ToString() ?? string.Empty,
            ["p_remark"] = memDetailRemark.Text,
        };

        var headerResult = await SaveAsync("USP_BA_ITEMGRP_S", headerParams);
        if (headerResult == null || !headerResult.Success)
        {
            AppMessageBox.Show(headerResult?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        var headerKey = _editingKey ?? headerResult.GeneratedCode;

        // ---- 2) 명세1(grd2 -> ) - grd3과 마찬가지로 선택사항이다(TplMasterFormSubGrid
        // 와 같은 원칙 - 저장프로시저가 없으면 이 단계를 건너뛴다). ----
        if (!string.IsNullOrEmpty(""))
        {
            var detail1Ok = await SaveDetailRowsAsync(gvw2, _detail1, "", headerKey, (row, version) => new Dictionary<string, string?>
            {
            });
            if (!detail1Ok) return;
        }

        // ---- 3) 명세2(grd3 -> ) - 선택사항. ----
        if (!string.IsNullOrEmpty(""))
        {
            var detail2Ok = await SaveDetailRowsAsync(gvw3, _detail2, "", headerKey, (row, version) => new Dictionary<string, string?>
            {
            });
            if (!detail2Ok) return;
        }

        // ---- 4) 명세3(grd4 -> ) - 선택사항. ----
        if (!string.IsNullOrEmpty(""))
        {
            var detail3Ok = await SaveDetailRowsAsync(gvw4, _detail3, "", headerKey, (row, version) => new Dictionary<string, string?>
            {
            });
            if (!detail3Ok) return;
        }

        // ---- 5) 명세4(grd5 -> ) - 선택사항. ----
        if (!string.IsNullOrEmpty(""))
        {
            var detail4Ok = await SaveDetailRowsAsync(gvw5, _detail4, "", headerKey, (row, version) => new Dictionary<string, string?>
            {
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
                ["p_grp_id"] = masterKey,
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
            ["p_grp_id"] = _editingKey,
        };
        var result = await SaveAsync("USP_BA_ITEMGRP_S", p);
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
