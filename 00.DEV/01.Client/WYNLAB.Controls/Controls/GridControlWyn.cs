using System.ComponentModel;
using DevExpress.XtraGrid;

namespace WYNLAB.Base.Controls;

/// <summary>
/// GridControl 기반 - Toolbox 아이콘 구분 + RowNoView 편의 속성을 얹은 서브클래스.
///
/// GridViewWyn을 여기 생성자에서 그냥 new로 만들어 붙이면(예전 방식), VS 디자이너의 "정식
/// 컴포넌트 생성 경로"(IDesignerHost.CreateComponent)를 안 타서 DevExpress 그리드 컬럼 편집기가
/// 그 View를 제대로 된 컴포넌트로 못 알아보고, 디자이너에서 추가한 컬럼이 재빌드하면 사라지는
/// 문제가 있었다.
///
/// [Designer] 속성으로 전용 디자이너(WYNLAB.Controls.Design 프로젝트의 GridControlWynDesigner -
/// 도구상자에서 캔버스에 놓는 순간 자동으로 GridViewWyn을 만들어 붙여주는 시도)를 연결해봤는데,
/// 그렇게 하면 그리드 자체가 캔버스에 아예 안 붙고 트레이에만 아이콘으로 남는 문제가 생겼다 -
/// InitializeNewComponent 안의 로직을 거의 다 들어내도(예외를 전부 삼키게, 심지어 BeginInvoke로
/// 통째로 지연 실행해도) 증상이 똑같아서, [Designer] 커스텀 지정 자체가 원인으로 보여
/// 일단 되돌렸다(원인 규명은 VS 디자이너 프로세스에 직접 디버거를 붙여야 할 것 같음 - 보류).
///
/// 그래서 지금은 GridControlWyn을 캔버스에 놓으면 기본 GridView가 붙고, 스마트태그의
/// Run Designer -> Views 편집기에서 직접 "추가" -> GridViewWyn을 선택해서 붙여야 한다(정식
/// 컴포넌트 생성 경로를 타므로 이 방법 자체는 컬럼 편집이 정상 저장될 것으로 보이나, 아직 실제
/// 컬럼 추가 -> 재빌드 -> 재확인까지 실사용자 테스트로 검증되지는 않음).
/// </summary>
[ToolboxItem(true)]
public class GridControlWyn : GridControl
{
    public GridControlWyn()
    {
        UseEmbeddedNavigator = true;
    }

    /// <summary>MainView를 GridViewWyn으로 캐스팅해서 접근하는 편의 속성 - MainView가
    /// GridViewWyn이 아직 아니면(Views 편집기로 추가하기 전) null을 돌려준다.
    ///
    /// ShowRowNumbers/Role처럼 실제로 그리드가 어떻게 보이고 동작할지 결정하는 옵션은 전부
    /// GridView 소속이라(DevExpress 자체 설계가 그렇다 - OptionsBehavior 등) 여기 GridControlWyn엔
    /// 그 값들을 되비추는 편의 프로퍼티를 두지 않는다. 화면 코드는 grd1이 아니라 gvw1(View) 쪽
    /// 프로퍼티를 직접 쓴다 - 예: gvw1.Role = GridRoleWyn.Query.</summary>
    [Browsable(false)]
    public GridViewWyn? View => MainView as GridViewWyn;

    /// <summary>하단 레코드 탐색바(첫/이전/다음/마지막/추가/삭제 버튼) - 기본으로 켜둔다.
    /// 기반 클래스(GridControl)의 DefaultValue는 false인데 생성자에서 true로 켜므로,
    /// 여기서 다시 override + [DefaultValue(true)]로 맞춰주지 않으면 디자이너에서 일부러
    /// false로 꺼도 "이미 기본값과 같다"고 오인해 저장을 생략하고 재빌드 후 도로 켜진다
    /// (PanelWyn.BorderStyle에서 겪은 것과 같은 종류의 함정).</summary>
    [DefaultValue(true)]
    public override bool UseEmbeddedNavigator
    {
        get => base.UseEmbeddedNavigator;
        set => base.UseEmbeddedNavigator = value;
    }
}
