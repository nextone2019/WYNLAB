namespace WYNLAB.Base.Controls;

/// <summary>
/// 그리드가 조회전용인지 입력/수정 가능한지 구분한다. GridViewWyn/BandedGridViewWyn.Role이 이
/// 값에 따라 셀 편집 가능 여부(OptionsBehavior.Editable/AllowAddRows/AllowDeleteRows)와
/// EmbeddedNavigator의 추가/삭제/편집 버튼 노출을 한 번에 맞춘다(GridViewWynBehavior 참고).
///
/// 기본값은 Query다 - 새로 만드는 그리드가 아무 설정 없이도 "실수로 편집 가능한 조회그리드"가
/// 되지 않도록, 입력이 필요한 그리드만 명시적으로 Edit를 지정하게 한다.
/// </summary>
public enum GridRoleWyn
{
    /// <summary>조회전용 - 셀 편집 불가, 행 추가/삭제 불가. 네비게이터엔 이동 버튼(처음/이전/다음/끝)만 보인다.</summary>
    Query,

    /// <summary>입력/수정 가능 - 셀 편집 가능, 행 추가/삭제 가능. 네비게이터에 추가/삭제/편집 버튼도 보인다.</summary>
    Edit
}
