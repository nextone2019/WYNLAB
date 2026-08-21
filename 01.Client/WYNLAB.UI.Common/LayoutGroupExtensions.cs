using DevExpress.XtraLayout;
using System.Drawing;

namespace WYNLAB.UI.Common;

/// <summary>
/// LayoutControl의 그룹 상자(기본정보/연결정보 등)를 "테두리 박스" 대신 굵은 섹션 제목
/// 하나만 있는 평평한 스타일로 바꾸는 확장메서드. DevExpress 기본 그룹 테두리(입체감 있는
/// 상자+왼쪽 상단에 걸친 캡션)는 최신 UI 트렌드와 안 맞아 보여서, 이 앱 전체(사용자등록,
/// 사용자그룹등록, 메뉴관리 등 LayoutControl을 쓰는 모든 등록/수정 화면)에 통일해서 적용한다.
/// </summary>
public static class LayoutGroupExtensions
{
    public static LayoutControlGroup StyleAsSection(this LayoutControlGroup group)
    {
        group.GroupBordersVisible = false;
        group.AppearanceGroup.Font = AppFonts.BodyBold;
        group.AppearanceGroup.ForeColor = Color.FromArgb(70, 70, 70);
        group.AppearanceGroup.Options.UseFont = true;
        group.AppearanceGroup.Options.UseForeColor = true;
        return group;
    }

    /// <summary>
    /// 입력 컨트롤의 폭을 입력될 값의 길이에 맞는 고정폭으로 지정한다. 지정하지 않으면
    /// LayoutControl 기본 동작대로 그룹/화면 폭에 맞춰 늘어나 버려서, 코드나 콤보처럼 짧은
    /// 값까지 화면 끝까지 늘어져 밋밋하고 헐렁해 보인다. 창(MDI) 크기가 바뀌어도 이 폭은
    /// 고정으로 유지된다 - 등록/수정 폼에서 필드가 창 크기 따라 늘었다 줄었다 할 필요는 없다는
    /// 방침. 이 앱 전체(사용자등록, 메뉴관리 등) LayoutControl 화면에서 공통으로 사용한다.
    /// </summary>
    public static LayoutControlItem FixedControlWidth(this LayoutControlItem item, int width, int height = 22)
    {
        item.SizeConstraintsType = SizeConstraintsType.Custom;
        item.ControlMinSize = new Size(width, height);
        item.ControlMaxSize = new Size(width, height);
        return item;
    }
}
