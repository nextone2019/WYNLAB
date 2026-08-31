using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

/// <summary>
/// 품목현황 화면 - 조회 전용 목록 하나만 있다(등록/수정/삭제는 품목등록(frmItem)에서 한다).
/// USP_BA_ITEMLIST_Q 하나로 동작하는 범용 데이터 통로 화면(GENERIC_DATA_API.md) - 서버
/// Controller/Repository 없음, 메뉴등록(TSMMENU)의 PROC_PREFIX=USP_BA_ITEMLIST_ 만으로 동작.
/// </summary>
public partial class frmItemList : BaseForm
{
    public frmItemList()
    {
        InitializeComponent();

        Text = "품목현황";
        MenuCd = "BA_ITEMLIST";

        // 조회전용 - 편집/행추가삭제 전부 막는다(등록/수정은 frmItem에서).
        gvw1.Role = GridRoleWyn.Query;

        // 개발자용 마우스오버 툴팁(BindingField)이 읽어갈 정보 - 실제 적용은
        // BaseForm.ApplyBindingFieldTooltips가 공통으로 처리한다. 그리드 컬럼은 FieldName이
        // 이미 실제 DB 컬럼명이라 자동 적용되고, 검색창 하나만 여기서 심어둔다.
        txtSearchQ.Tag = new BindingFieldTag("item_cd / item_nm");
    }

    /// <summary>품목 목록 조회 - 검색어 하나로 품목코드/품목명 둘 다(LIKE) 찾는다.
    /// 상세 패널이 없는 화면이라 재조회 후 선택 복원 같은 처리가 필요 없다(frmAcc/frmEmp와
    /// 다른 점 - QueryClick이 preserveSelection 분기 없이 이거 하나뿐).</summary>
    public override async Task QueryClick()
    {
        var keyword = txtSearchQ.Text.Trim();

        var items = await QueryAsync("USP_BA_ITEMLIST_Q", new
        {
            p_work_type = "Q",
            p_item_cd = keyword,
            p_item_nm = keyword
        });

        grd1.DataSource = items;
    }
}
