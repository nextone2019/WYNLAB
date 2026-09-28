using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.Popup;

/// <summary>
/// 전용 팝업 검색패널의 기준 샘플(2026-09-25) - 팝업관리 화면 "검색패널"에 WYNLAB.Popup.pnlItemSearch를
/// 넣으면 P_ITEM_PO 팝업이 자동 생성 검색창 대신 이 패널을 쓴다. 규칙은 하나뿐: 조회조건 컨트롤의
/// Name을 프로시저 파라미터명(SSP_POP_ITEM_PO_Q의 @p_keyword, @p_asset_type, @p_grp1_id~4)과 똑같이 짓는다 -
/// 그 이름으로 값을 읽어 프로시저에 넘기므로 여기엔 연결 코드가 없다(품목그룹 룩업 설정만 아래에 있다).
/// 배치는 TableLayoutPanel(라벨 열 자동 폭 + 입력창 열 고정 폭) - 글꼴/배율이 바뀌어도 라벨과 입력창이 겹치지 않는다.
/// 조건을 더하려면 디자이너에서 표에 행/열을 추가하고 컨트롤 Name만 파라미터명으로 짓는다.
/// </summary>
public partial class pnlItemSearch : PopupSearchPanelBase
{
    public pnlItemSearch()
    {
        InitializeComponent();

        // 품목그룹 1~4단 L_ITEM_GRP(frmItem과 같은 방식 - 그쪽 주석 참고): 파라미터를 채운 "뒤에" LookupKey를 지정한다.
        // 상위 그룹을 고르면 하위 목록을 그 상위에 속한 것으로 다시 채우고 하위 선택값은 지운다(3->4단까지 연쇄).
        LookUpEditWyn[] grp = { p_grp1_id, p_grp2_id, p_grp3_id, p_grp4_id };
        for (var i = 0; i < grp.Length; i++)
        {
            grp[i].SetParam("p_grp_lvl", (i + 1).ToString());
            grp[i].SetParam("p_par_grp_id", "0");
            grp[i].LookupKey = "L_ITEM_GRP";
        }

        for (var i = 0; i < grp.Length - 1; i++)
        {
            var parent = grp[i];
            var child = grp[i + 1];
            parent.EditValueChanged += (s, e) =>
            {
                var parentId = parent.EditValue?.ToString();
                child.SetParam("p_par_grp_id", string.IsNullOrEmpty(parentId) ? "0" : parentId);
                child.EditValue = null;
            };
        }
    }
}
