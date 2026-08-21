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
}
