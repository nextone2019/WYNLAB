// AI Builder가 마스터-폼-탭그리드 템플릿을 복제해서 자동 생성 - 2026-09-08.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmItem : BaseForm
{
    private DataTable _list = new();
    private DataTable _detail1 = new();
    private DataTable _detail2 = new();
    private string? _editingKey; // null이면 신규모드
    private bool _loadingGrpChain; // LoadGrpChainAsync가 품목그룹 1~4단을 순서대로 채우는 동안 true

    public frmItem()
    {
        InitializeComponent();

        Text = "품목등록";

        Controls.Add(BuildScreenHeader());

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        // 다른 마스터 행을 고를 때 panData/grd2/grd3에 저장 안 된 변경이 있으면 먼저 확인한다
        // (BaseForm.ConfirmMasterRowSwitch 참고, 2026-09-06 - 모든 화면 공통 적용. 새 화면을 이
        // 템플릿에서 복제하면 기본으로 따라온다. 지우지 말 것). 이름 있는 메서드로 등록해야
        // QueryCore가 grd1을 다시 그리는 동안 잠깐 구독을 끊을 수 있다(Gvw1_FocusedRowObjectChanged
        // 설명 참고) - 람다로 등록하면 나중에 뗄 방법이 없다.
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        // grd2/grd3는 MasterFormSubGrid의 grd2(조회전용)와 달리 편집 가능하다 - 각자 자기
        // 저장프로시저(USP_BA_ITEM_S_1/)로 저장되기 때문. Role=Edit이면 그리드
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

        // 개발자용 마우스오버 툴팁(BindingField) - 실제 적용은 BaseForm.ApplyBindingFieldTooltips가
        // 공통으로 처리한다(Session.IsDeveloper일 때만). 그리드 컬럼은 FieldName이 이미 DB
        // 컬럼명이라 자동 적용되지만, panData 개별 컨트롤은 Tag에 미리 넣어둬야 잡힌다
        // (2026-09-12 감사 - 이 화면엔 원래 빠져있었음).
        cboDetailAccCd.Tag = new BindingFieldTag("acc_id");
        txtDetailItemId.Tag = new BindingFieldTag("item_id");
        txtDetailItemNo.Tag = new BindingFieldTag("item_no");
        txtDetailItemNm.Tag = new BindingFieldTag("item_nm");
        txtDetailItemSpec.Tag = new BindingFieldTag("item_spec");
        cboDetailUnitCd.Tag = new BindingFieldTag("unit_cd");
        cboDetailPoUnitCd.Tag = new BindingFieldTag("po_unit_cd");
        txtDetailWhId.Tag = new BindingFieldTag("wh_id");
        popDetailWhNm.Tag = new BindingFieldTag("wh_nm");
        txtDetailLocId.Tag = new BindingFieldTag("loc_id");
        popDetailLocNm.Tag = new BindingFieldTag("loc_nm");
        numDetailSafeQty.Tag = new BindingFieldTag("safe_qty");
        txtDetailDeptId.Tag = new BindingFieldTag("dept_id");
        popDetailDeptNm.Tag = new BindingFieldTag("dept_nm");
        txtDetailEmpId.Tag = new BindingFieldTag("emp_id");
        txtDetailEmpNo.Tag = new BindingFieldTag("emp_no");
        popDetailEmpNm.Tag = new BindingFieldTag("emp_nm");
        txtDetailCustId.Tag = new BindingFieldTag("cust_id");
        popDetailCustNm.Tag = new BindingFieldTag("cust_nm");
        cboDetailAssetType.Tag = new BindingFieldTag("asset_type");
        txtDetailOutType.Tag = new BindingFieldTag("out_type");
        chkDetailPoQcYn.Tag = new BindingFieldTag("po_qc_yn");
        chkDetailProdQcYn.Tag = new BindingFieldTag("prod_qc_yn");
        chkDetailLotYn.Tag = new BindingFieldTag("lot_yn");
        chkDetailStockYn.Tag = new BindingFieldTag("stock_yn");
        cboDetailStatCd.Tag = new BindingFieldTag("stat_cd");
        cboDetailGrp1Id.Tag = new BindingFieldTag("grp1_id");
        cboDetailGrp2Id.Tag = new BindingFieldTag("grp2_id");
        cboDetailGrp3Id.Tag = new BindingFieldTag("grp3_id");
        cboDetailGrp4Id.Tag = new BindingFieldTag("grp4_id");
        txtDetailRemark.Tag = new BindingFieldTag("remark");

        // 창고(wh_id/wh_nm)/위치(loc_id/loc_nm) 팝업 - PopupLookupEditWyn의 멀티필드 모드(②)를 쓴다.
        // 이 컨트롤 자신(popDetailWhNm)이 곧 "명칭" 표시값(MatchField="wh_nm")이고, 팝업에서 고른
        // 행의 wh_id 컬럼을 MapField로 옆의 읽기전용 txtDetailWhId에 채워 넣는다 - 이름을 직접
        // 타이핑하고 탭으로 빠져나가면(OnLeaveAsync) 정확히 하나만 일치할 때 조용히, 아니면 그
        // 값으로 미리 채운 팝업이 뜬다. loc도 동일.
        popDetailWhNm.MatchField = "wh_nm";
        popDetailWhNm.MapField("wh_id", txtDetailWhId);
        popDetailLocNm.MatchField = "loc_nm";
        popDetailLocNm.MapField("loc_id", txtDetailLocId);

        // 담당부서/담당자/구매처 팝업도 위와 같은 멀티필드 모드로 연결한다 - 이전에는 MapField
        // 연결이 아예 없어서(popDetailCustNm.LookupKey도 "P_EMP"로 잘못 지정돼 있었음, Designer.cs
        // 참고) 팝업으로 골라도 이름만 바뀌고 실제 저장에 쓰는 ID는 그대로 남는 문제가 있었다
        // (2026-09-15 신고). P_DEPT/P_EMP/P_CUST 팝업이 돌려주는 키 컬럼은 DEPT_ID/EMP_ID/CUST_ID
        // 대문자다(SSP_POP_DEPT_Q/SSP_POP_EMP_Q/SSP_POP_CUST_Q 정의 그대로, P_WH/P_LOC와 달리
        // 대소문자를 정확히 맞춰야 한다). 담당자는 emp_id(저장용, 화면엔 안 보임)와 emp_no(사원
        // 번호 표시용) 둘 다 같은 팝업 결과에서 채운다.
        popDetailDeptNm.MatchField = "dept_nm";
        popDetailDeptNm.MapField("DEPT_ID", txtDetailDeptId);
        popDetailEmpNm.MatchField = "emp_nm";
        popDetailEmpNm.MapField("EMP_ID", txtDetailEmpId);
        popDetailEmpNm.MapField("emp_no", txtDetailEmpNo);
        popDetailCustNm.MatchField = "cust_nm";
        popDetailCustNm.MapField("CUST_ID", txtDetailCustId);

        // 품목그룹 1~4단 연쇄 LookUp(L_ITEM_GRP, 2026-09-15) - 하나의 lookup_key를 grp_lvl 값만
        // 바꿔가며 4번 재사용한다(SSP_CBO_ITEM_GRP_Q가 grp_lvl+par_grp_id 둘 다 받음). 파라미터를
        // 전부 채운 "뒤에" LookupKey를 지정해야 한다 - LookupKey 지정이 곧바로 조회를 한 번
        // 쏘기 때문에, Designer.cs에는 LookupKey를 안 넣고 여기서 파라미터 다음에 마지막으로
        // 지정한다(안 그러면 파라미터가 덜 채워진 조회가 나중에 도착해 제대로 된 조회 결과를
        // 덮어쓸 수 있음). 최상위(1단)는 항상 par_grp_id='0'(TBAITEMGRP 최상위 그룹 관례) 고정.
        cboDetailGrp1Id.SetParam("p_grp_lvl", "1");
        cboDetailGrp1Id.SetParam("p_par_grp_id", "0");
        cboDetailGrp1Id.LookupKey = "L_ITEM_GRP";

        cboDetailGrp2Id.SetParam("p_grp_lvl", "2");
        cboDetailGrp2Id.SetParam("p_par_grp_id", "0");
        cboDetailGrp2Id.LookupKey = "L_ITEM_GRP";

        cboDetailGrp3Id.SetParam("p_grp_lvl", "3");
        cboDetailGrp3Id.SetParam("p_par_grp_id", "0");
        cboDetailGrp3Id.LookupKey = "L_ITEM_GRP";

        cboDetailGrp4Id.SetParam("p_grp_lvl", "4");
        cboDetailGrp4Id.SetParam("p_par_grp_id", "0");
        cboDetailGrp4Id.LookupKey = "L_ITEM_GRP";

        // 상위 그룹이 바뀌면 하위 그룹의 목록을 그 상위에 속한 것들로 다시 채우고, 기존에 골라둔
        // 하위 선택값은(더 이상 유효하지 않을 수 있으므로) 지운다 - 지우는 동작 자체가 그 하위의
        // EditValueChanged를 또 발생시켜서 3->4단까지 자동으로 연쇄된다. 단, 기존 레코드를 불러오는
        // 중(_loadingGrpChain=true, LoadGrpChainAsync)에는 값을 지우면 안 된다 - 그쪽이 순서대로
        // 직접 채우는 중이라 여기서 지워버리면 방금 채운 값이 날아간다.
        cboDetailGrp1Id.EditValueChanged += (s, e) => OnGrpParentChanged(cboDetailGrp2Id, cboDetailGrp1Id.EditValue?.ToString());
        cboDetailGrp2Id.EditValueChanged += (s, e) => OnGrpParentChanged(cboDetailGrp3Id, cboDetailGrp2Id.EditValue?.ToString());
        cboDetailGrp3Id.EditValueChanged += (s, e) => OnGrpParentChanged(cboDetailGrp4Id, cboDetailGrp3Id.EditValue?.ToString());


        // 화면종료 시 저장 확인(BaseForm.ConfirmCloseAsync)과 다른 마스터 행으로 옮길 때 확인
        // (ConfirmMasterRowSwitch) 둘 다 이 추적에 기댄다 - grd2/grd3(편집 가능한 하위 그리드)는
        // EnterNewMode/LoadDetailAsync에서 새 DataTable로 바뀔 때마다 그때그때 TrackDirty(_detail1)/
        // TrackDirty(_detail2)를 다시 걸어야 한다.
        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) => await QueryClick();
    }

    private GridViewWyn ActiveDetailView() => ReferenceEquals(tabDetailGrids.SelectedTabPage, tabDetail2) ? gvw3 : gvw2;

    /// <summary>사용자가 상위 그룹을 바꿀 때만 호출됨(생성자의 EditValueChanged 구독) - 기존
    /// 레코드를 불러오는 중(LoadGrpChainAsync)에는 이 클래스가 값을 직접 순서대로 채우므로
    /// 호출부에서 이 메서드를 부르지 않고 LoadGrpChainAsync가 SetParamAsync로 직접 처리한다.</summary>
    private void OnGrpParentChanged(LookUpEditWyn child, string? parGrpId)
    {
        child.SetParam("p_par_grp_id", string.IsNullOrEmpty(parGrpId) ? "0" : parGrpId);
        if (_loadingGrpChain) return;
        child.EditValue = null!; // 상위가 바뀌면 이전 하위 선택은 더 이상 유효하지 않을 수 있다
    }

    /// <summary>선택된 마스터 행의 품목그룹 1~4단을 순서대로(상위 -> 하위) 채운다 - 하위 단계의
    /// 목록은 바로 위 단계 값에 따라 달라지므로, 그 목록이 실제로 다시 채워질 때까지
    /// SetParamAsync로 기다린 다음에야 그 하위의 EditValue를 설정해야 표시텍스트가 정상적으로
    /// 붙는다(SetParam은 fire-and-forget이라 순서 보장이 안 됨). _loadingGrpChain으로 감싸서,
    /// 이 과정에서 발생하는 EditValueChanged(OnGrpParentChanged)가 방금 채운 값을 도로 지우지
    /// 않게 막는다.</summary>
    private async Task LoadGrpChainAsync(DataRow row)
    {
        _loadingGrpChain = true;
        try
        {
            var grp1 = row["grp1_id"]?.ToString();
            var grp2 = row["grp2_id"]?.ToString();
            var grp3 = row["grp3_id"]?.ToString();
            var grp4 = row["grp4_id"]?.ToString();

            cboDetailGrp1Id.EditValue = grp1 ?? string.Empty;
            await cboDetailGrp2Id.SetParamAsync("p_par_grp_id", string.IsNullOrEmpty(grp1) ? "0" : grp1);
            cboDetailGrp2Id.EditValue = grp2 ?? string.Empty;
            await cboDetailGrp3Id.SetParamAsync("p_par_grp_id", string.IsNullOrEmpty(grp2) ? "0" : grp2);
            cboDetailGrp3Id.EditValue = grp3 ?? string.Empty;
            await cboDetailGrp4Id.SetParamAsync("p_par_grp_id", string.IsNullOrEmpty(grp3) ? "0" : grp3);
            cboDetailGrp4Id.EditValue = grp4 ?? string.Empty;
        }
        finally
        {
            _loadingGrpChain = false;
        }
    }

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
            ["p_item_id"] = txtItemId.Text,
        };
        _list = await QueryAsync("USP_BA_ITEM_Q", p);

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
        // editingKey가 없으면(최초 조회) grd1 바인딩 직후 DevExpress가 스스로 0번 행에 포커스를
        // 준다 - 그 자동 포커스 행을 그대로 panData에 채운다(2026-09-08 실제 발견/수정 - AI
        // Builder 템플릿 자체의 누락이었고 frmAcc.cs가 이미 쓰던 패턴을 가져왔다).
        else if (gvw1.GetFocusedRow() is DataRowView focusedView) await OnMasterSelectedAsync(focusedView.Row);
        else EnterNewMode();
    }

    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e) =>
        ConfirmMasterRowSwitch(gvw1, e, row => _ = OnMasterSelectedAsync(row.Row));

    /// <summary>조회된 목록에서 키(item_id)로 행을 찾는다. 없으면 null.</summary>
    private DataRow? FindRow(string key) =>
        _list.Rows.Cast<DataRow>()
            .FirstOrDefault(r => string.Equals(Convert.ToString(r["item_id"]), key, StringComparison.OrdinalIgnoreCase));

    /// <summary>키로 grd1의 행 핸들을 찾는다. 없으면 null. 컬럼 셀 값으로 찾는 방식은 그 키
    /// 컬럼이 화면에 안 보이는 숨김 컬럼일 때 안 먹힌다(2026-09-11 실제 발견, frmEMP에서 재현) -
    /// FindRow로 찾은 DataRow의 DataTable상 인덱스를 GridView.GetRowHandle로 표시 행 핸들로
    /// 변환하면 키 컬럼이 보이든 안 보이든 항상 동작한다.</summary>
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
        // 품목그룹 1~4단(LoadGrpChainAsync)은 await가 섞여 있어서 동기 버전(SuppressDirtyTracking)
        // 으로는 이 블록이 끝난 뒤(_suppressDirtyTracking이 이미 꺼진 상태)의 await 이후 코드가
        // 보호를 못 받는다 - 비동기 버전을 쓴다(frmSiteConfig가 이미 쓰던 패턴).
        await SuppressDirtyTrackingAsync(async () =>
        {
            _editingKey = row["item_id"]?.ToString();
        cboDetailAccCd.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
        txtDetailItemId.Text = row["item_id"]?.ToString() ?? string.Empty;
        txtDetailItemNo.Text = row["item_no"]?.ToString() ?? string.Empty;
        txtDetailItemNm.Text = row["item_nm"]?.ToString() ?? string.Empty;
        txtDetailItemSpec.Text = row["item_spec"]?.ToString() ?? string.Empty;
        cboDetailUnitCd.EditValue = row["unit_cd"]?.ToString() ?? string.Empty;
        cboDetailPoUnitCd.EditValue = row["po_unit_cd"]?.ToString() ?? string.Empty;
        txtDetailWhId.Text = row["wh_id"]?.ToString() ?? string.Empty;
        popDetailWhNm.Text = row["wh_nm"]?.ToString() ?? string.Empty;
        txtDetailLocId.Text = row["loc_id"]?.ToString() ?? string.Empty;
        popDetailLocNm.Text = row["loc_nm"]?.ToString() ?? string.Empty;
        numDetailSafeQty.EditValue = decimal.TryParse(row["safe_qty"]?.ToString(), out var numSafeQty) ? numSafeQty : (decimal?)null;
        txtDetailDeptId.EditValue = decimal.TryParse(row["dept_id"]?.ToString(), out var numDeptId) ? numDeptId : (decimal?)null;
        popDetailDeptNm.Text = row["dept_nm"]?.ToString() ?? string.Empty;
        txtDetailEmpId.Text = row["emp_id"]?.ToString() ?? string.Empty;
        txtDetailEmpNo.Text = row["emp_no"]?.ToString() ?? string.Empty;
        popDetailEmpNm.Text = row["emp_nm"]?.ToString() ?? string.Empty;
        txtDetailCustId.Text = row["cust_id"]?.ToString() ?? string.Empty;
        popDetailCustNm.Text = row["cust_nm"]?.ToString() ?? string.Empty;
        cboDetailAssetType.EditValue = row["asset_type"]?.ToString() ?? string.Empty;
        txtDetailOutType.Text = row["out_type"]?.ToString() ?? string.Empty;
        chkDetailPoQcYn.Checked = row["po_qc_yn"]?.ToString() == "Y";
        chkDetailProdQcYn.Checked = row["prod_qc_yn"]?.ToString() == "Y";
        chkDetailLotYn.Checked = row["lot_yn"]?.ToString() == "Y";
        chkDetailStockYn.Checked = row["stock_yn"]?.ToString() == "Y";
        cboDetailStatCd.EditValue = row["stat_cd"]?.ToString() ?? string.Empty;
        txtDetailRemark.Text = row["remark"]?.ToString() ?? string.Empty;

        await LoadGrpChainAsync(row);
        });

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
        cboDetailAccCd.EditValue = Session.AccId?.ToString() ?? string.Empty;
        txtDetailItemId.Text = string.Empty;
        txtDetailItemNo.Text = string.Empty;
        txtDetailItemNm.Text = string.Empty;
        txtDetailItemSpec.Text = string.Empty;
        cboDetailUnitCd.EditValue = string.Empty;
        cboDetailPoUnitCd.EditValue = string.Empty;
        txtDetailWhId.Text = string.Empty;
        popDetailWhNm.Text = string.Empty;
        txtDetailLocId.Text = string.Empty;
        popDetailLocNm.Text = string.Empty;
        numDetailSafeQty.EditValue = null;
        txtDetailDeptId.EditValue = null;
        popDetailDeptNm.Text = string.Empty;
        txtDetailEmpId.Text = string.Empty;
        txtDetailEmpNo.Text = string.Empty;
        popDetailEmpNm.Text = string.Empty;
        txtDetailCustId.Text = string.Empty;
        popDetailCustNm.Text = string.Empty;
        cboDetailAssetType.EditValue = string.Empty;
        txtDetailOutType.Text = string.Empty;
        chkDetailPoQcYn.Checked = false;
        chkDetailProdQcYn.Checked = false;
        chkDetailLotYn.Checked = false;
        chkDetailStockYn.Checked = false;
        cboDetailStatCd.EditValue = "0"; // 신규입력 시 품목상태 기본값
        cboDetailGrp1Id.EditValue = null!;
        cboDetailGrp2Id.EditValue = null!;
        cboDetailGrp3Id.EditValue = null!;
        cboDetailGrp4Id.EditValue = null!;
        txtDetailRemark.Text = string.Empty;
            _detail1 = _detail1.Clone();
            _detail2 = _detail2.Clone();
            TrackDirty(_detail1);
            TrackDirty(_detail2);
            grd2.DataSource = _detail1;
            grd3.DataSource = _detail2;
        });

        txtDetailItemNo.Focus();
    }

    /// <summary>선택된 마스터 행의 하위 목록 2개(grd2/grd3)를 한 번의 호출로 같이 조회한다 -
    /// USP_BA_ITEM_Q_1가 work_type='Q'일 때 레코드셋을 2개(순서대로
    /// grd2용, grd3용) 반환하기 때문에 QueryMultiAsync를 쓴다(QueryAsync는 첫 레코드셋만 받음).</summary>
    private async Task LoadDetailAsync()
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_item_id"] = _editingKey,
        };
        var tables = await QueryMultiAsync("USP_BA_ITEM_Q_1", p);
        _detail1 = tables.Count > 0 ? tables[0] : new DataTable();
        _detail2 = tables.Count > 1 ? tables[1] : new DataTable();
        TrackDirty(_detail1);
        TrackDirty(_detail2);
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
        // 저장프로시저가 지정 안 된 조회전용 화면이면 여기서 막는다(TplSingleGrid.cs와 같은 가드) -
        // grd2/grd3 저장도 헤더가 돌려주는 키(headerKey)가 있어야 동작하므로, 헤더가 없으면
        // 저장 전체를 여기서 끊는다.
        if (string.IsNullOrEmpty("USP_BA_ITEM_S"))
        {
            AppMessageBox.Show("이 화면은 조회전용으로 생성됐습니다 - 저장프로시저가 지정되지 않았습니다.", "저장 불가");
            return;
        }

        // ---- 1) 헤더(panData -> USP_BA_ITEM_S) ----
        var headerParams = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_acc_id"] = cboDetailAccCd.EditValue?.ToString(),
            // item_id는 BIGINT(IDENTITY)인데 컨트롤이 TextEditWyn(TEXT)이라 신규 등록 시 빈
            // 문자열("")이 그대로 넘어갈 수 있다 - ""는 NULL과 달리 BIGINT 변환이 실패해서 그대로
            // SqlException으로 죽는다(ProcData.cs의 NUMBER null-safety 수정과 같은 이유). 빈 값이면
            // null로 보낸다.
            ["p_item_id"] = string.IsNullOrEmpty(txtDetailItemId.Text) ? null : txtDetailItemId.Text,
            ["p_item_no"] = txtDetailItemNo.Text,
            ["p_item_nm"] = txtDetailItemNm.Text,
            ["p_item_spec"] = txtDetailItemSpec.Text,
            ["p_unit_cd"] = cboDetailUnitCd.EditValue?.ToString() ?? string.Empty,
            ["p_po_unit_cd"] = cboDetailPoUnitCd.EditValue?.ToString() ?? string.Empty,
            // wh_id/loc_id도 item_id와 같은 이유(TEXT 컨트롤이 BIGINT 컬럼을 대표) - 빈 값이면
            // null. 팝업(popDetailWhNm/popDetailLocNm)이 선택 시 MapField로 이 값을 채워준다.
            ["p_wh_id"] = string.IsNullOrEmpty(txtDetailWhId.Text) ? null : txtDetailWhId.Text,
            ["p_loc_id"] = string.IsNullOrEmpty(txtDetailLocId.Text) ? null : txtDetailLocId.Text,
            ["p_safe_qty"] = numDetailSafeQty.EditValue?.ToString(),
            // dept_id도 item_id와 같은 이유(TEXT 컨트롤이 BIGINT 컬럼을 대표) - 빈 값이면 null.
            ["p_dept_id"] = string.IsNullOrEmpty(txtDetailDeptId.Text) ? null : txtDetailDeptId.Text,
            // emp_id/cust_id - 이전에는 이 두 파라미터를 전혀 안 보내고 있어서(USP_BA_ITEM_S는
            // 이미 받는데 화면이 안 보냈음, 2026-09-15 발견) 담당자/구매처를 뭘 고르든 저장이
            // 안 됐다. 팝업(popDetailEmpNm/popDetailCustNm)이 선택 시 MapField로 이 값을 채운다.
            ["p_emp_id"] = string.IsNullOrEmpty(txtDetailEmpId.Text) ? null : txtDetailEmpId.Text,
            ["p_cust_id"] = string.IsNullOrEmpty(txtDetailCustId.Text) ? null : txtDetailCustId.Text,
            ["p_asset_type"] = cboDetailAssetType.EditValue?.ToString() ?? string.Empty,
            ["p_out_type"] = txtDetailOutType.Text,
            ["p_po_qc_yn"] = chkDetailPoQcYn.Checked ? "Y" : "N",
            ["p_prod_qc_yn"] = chkDetailProdQcYn.Checked ? "Y" : "N",
            ["p_lot_yn"] = chkDetailLotYn.Checked ? "Y" : "N",
            ["p_stock_yn"] = chkDetailStockYn.Checked ? "Y" : "N",
            // prod_yn/po_yn/sale_yn/po_acnt_cd/sale_acnt_cd: TBAITEM에서 이 컬럼들이 전부
            // 삭제되어(2026-09-15, 사장님 확인) 화면 컨트롤도 없고 프로시저도 이 파라미터들을
            // 더 안 받는다 - USP_BA_ITEM_Q/S 쪽도 같은 마이그레이션에서 같이 정리함.
            ["p_stat_cd"] = cboDetailStatCd.EditValue?.ToString() ?? string.Empty,
            ["p_grp1_id"] = cboDetailGrp1Id.EditValue?.ToString(),
            ["p_grp2_id"] = cboDetailGrp2Id.EditValue?.ToString(),
            ["p_grp3_id"] = cboDetailGrp3Id.EditValue?.ToString(),
            ["p_grp4_id"] = cboDetailGrp4Id.EditValue?.ToString(),
            ["p_remark"] = txtDetailRemark.Text,
        };

        var headerResult = await SaveAsync("USP_BA_ITEM_S", headerParams);
        if (headerResult == null || !headerResult.Success)
        {
            AppMessageBox.Show(headerResult?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        var headerKey = _editingKey ?? headerResult.GeneratedCode;

        // ---- 2) 명세1(grd2 -> USP_BA_ITEM_S_1) - grd2도 grd3과 마찬가지로 선택사항이다(AI
        // Builder에서 Save Actions를 안 채우면 빈 문자열로 치환됨) - 저장프로시저가 없으면 이
        // 단계를 건너뛴다. grd2는 대개 저장까지 채우는 경우가 많아 guard가 없어도 되는 것처럼
        // 보였지만, grd3처럼 조회전용(Save Actions 미설정)으로만 쓰는 경우도 있어서 guard 없이
        // SaveAsync("", ...)를 그대로 호출하면 서버가 빈 프로시저 이름을 거부하며 원인을 알기
        // 어려운 오류가 난다(2026-09-07 실제 발견 - frmItem333 grd2에서 재현). MasterFormSubGrid의
        // panData 조회전용 폴백과 같은 원칙. ----
        if (!string.IsNullOrEmpty("USP_BA_ITEM_S_1"))
        {
            var detail1Ok = await SaveDetailRowsAsync(gvw2, _detail1, "USP_BA_ITEM_S_1", headerKey, (row, version) => new Dictionary<string, string?>
            {
            ["p_acc_id"] = ProcData.Str(row, "acc_id", version),
            ["p_item_id"] = ProcData.Str(row, "item_id", version),
            ["p_fr_unit_cd"] = ProcData.Str(row, "fr_unit_cd", version),
            ["p_fr_qty"] = ProcData.Str(row, "fr_qty", version),
            ["p_to_unit_cd"] = ProcData.Str(row, "to_unit_cd", version),
            ["p_to_qty"] = ProcData.Str(row, "to_qty", version),
            ["p_remark"] = ProcData.Str(row, "remark", version),
            });
            if (!detail1Ok) return;
        }

        // ---- 3) 명세2(grd3 -> ) - grd3은 선택사항이라(AI Builder에서 안 채우면
        // 빈 문자열로 치환됨) 저장프로시저가 없으면 이 단계를 건너뛴다(MasterFormSubGrid의 panData
        // 조회전용 폴백과 같은 원칙 - "지우지 말 것" 주석이 아니라 실제로 이 화면에 grd3이 없는
        // 정상적인 경우다). ----
        if (!string.IsNullOrEmpty(""))
        {
            var detail2Ok = await SaveDetailRowsAsync(gvw3, _detail2, "", headerKey, (row, version) => new Dictionary<string, string?>
            {
            });
            if (!detail2Ok) return;
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
                ["p_item_id"] = masterKey,
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
            ["p_item_id"] = _editingKey,
        };
        var result = await SaveAsync("USP_BA_ITEM_S", p);
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
