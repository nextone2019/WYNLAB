using WYNLAB.Base.Controls;

namespace WYNLAB.Popup;

/// <summary>
/// WYNLAB.Controls가 노출한 팝업 훅(PopupLookupProvider.OpenPopup)에 실제 구현(popPopUp)을
/// 연결한다 - ControlDataSources.Initialize()와 원래 같은 자리에 있던 코드인데, popPopUp이
/// WYNLAB.Popup 프로젝트로 옮겨오면서(2026-09-12) 여기로 같이 옮겼다. BaseForm -> Popup ->
/// BaseForm으로 순환참조가 생기면 안 되므로(Popup이 BaseForm의 ApiClient/Session 등을 쓰려면
/// BaseForm -> Popup 참조가 있으면 안 됨), 이 등록만 따로 떼어 Popup 쪽에 둔다.
///
/// Program.cs의 Main()에서 ControlDataSources.Initialize() 바로 다음 줄에 이것도 호출해야 한다 -
/// 화면 모듈이 로드되어 PopupLookupEditWyn 컨트롤이 생성되기 전에 반드시 먼저 해둬야 한다.
/// </summary>
public static class PopupWiring
{
    public static void Initialize()
    {
        // PopupLookupEditWyn("..." 버튼)이 실제로 팝업을 여는 방법 - popPopUp이
        // sysPopUpM/sysPopUpD 정의를 읽어서 스스로 그린다(엔티티별 폼 클래스 없음).
        PopupLookupProvider.OpenPopup = popPopUp.ShowAsync;
    }
}
