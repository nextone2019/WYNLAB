namespace WYNLAB.Shared.Dtos;

/// <summary>개인별 그리드 레이아웃(컬럼 순서/숨김/폭) 1건 - TSMUSERGRIDLAYOUT.GRID_KEY/LAYOUT_XML.
/// 전용 컨트롤러(api/grid-layout)를 통해서만 오간다 - 범용 데이터 통로(api/data/*)는 화면마다
/// 등록된 PROC_PREFIX로만 호출을 제한하는데, 이 기능은 특정 화면 소유 데이터가 아니라 로그인한
/// 사용자면 어느 화면에서든 써야 해서 그 제약과 안 맞는다.</summary>
public class GridLayoutItemDto
{
    public string GridKey { get; set; } = string.Empty;
    public string LayoutXml { get; set; } = string.Empty;
}

/// <summary>그리드 레이아웃 저장 요청.</summary>
public class SaveGridLayoutRequest
{
    public long MenuId { get; set; }
    public string GridKey { get; set; } = string.Empty;
    public string LayoutXml { get; set; } = string.Empty;
}
