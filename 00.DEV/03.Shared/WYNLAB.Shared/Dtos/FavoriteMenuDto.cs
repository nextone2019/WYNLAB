namespace WYNLAB.Shared.Dtos;

/// <summary>사이드바 "마이 메뉴" 즐겨찾기 1건(TSMUSERFAVORITEMENU) - 메뉴ID와 사용자가 지정한
/// 폴더 이름(비어있으면 마이 메뉴 최상위에 평평하게 표시). 순서는 응답 목록 자체의 순서
/// (서버 SORT_ORDER 기준)를 그대로 따른다 - 별도 필드로 안 내려준다.</summary>
public class FavoriteMenuDto
{
    public long MenuId { get; set; }
    public string? Folder { get; set; }
}
