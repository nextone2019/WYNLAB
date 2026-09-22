using System.ComponentModel;

namespace WYNLAB.Base.Controls;

/// <summary>
/// 원래는 DevExpress GroupControl 기반이었는데, 캡션이 테두리 위쪽 바 형태로 렌더링되어
/// "WinForm 고유 컨트롤 GroupBox처럼" 보이길 원하는 요청(2026-09-15, frmItem 그룹화 작업 중)으로
/// 순정 System.Windows.Forms.GroupBox 기반으로 바꿨다 - 캡션이 상단 테두리 선 안에 파고드는
/// 클래식 윈폼 모양이 이걸로 바로 나온다(DevExpress 스타일을 흉내 내는 대신 그냥 그 컨트롤을
/// 그대로 상속). 기본 폰트는 다른 컨트롤들과 통일되도록 AppFonts.Body(맑은 고딕 9pt)로 맞춘다 -
/// 화면마다 다시 지정할 필요 없이 이 컨트롤을 쓰기만 하면 된다.
/// </summary>
[ToolboxItem(true)]
public class GroupBoxWyn : System.Windows.Forms.GroupBox
{
    public GroupBoxWyn()
    {
        Font = WYNLAB.Base.AppFonts.Body;
    }
}
