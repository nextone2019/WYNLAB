using System.ComponentModel;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;

namespace WYNLAB.Base.Controls;

/// <summary>
/// PopupLookupEditWyn(패널용 독립 컨트롤)의 팝업(sysPopUpM.popup_key) 연결을 그리드 컬럼 편집기
/// (GridColumn.ColumnEdit)에서도 그대로 쓰기 위한 RepositoryItem 버전 - LookUpColumnEdit과 같은
/// 발상/등록 방식이다(그 클래스 설명 참고, 이 클래스는 컬럼당 하나씩 만든다는 점까지 동일).
/// AI Builder의 Control="POP"이 grd1/grd2/grd3 컬럼에서도 실제 팝업(...버튼)을 쓸 수 있게 하려고
/// 추가했다(2026-09-07, 처음엔 panData 상세폼만 지원하고 그리드는 미뤄뒀었음).
///
/// LookUpColumnEdit과 다른 점: LookUpEditWyn.LookupKey는 RepositoryItemLookUpEdit 자체가 가진
/// DataSource/Columns/ValueMember 같은 표준 프로퍼티를 그대로 채우는 것이라, 그리드가 셀 편집기를
/// 만들 때 Properties를 이 RepositoryItem 인스턴스로 그대로 공유해주는 것만으로 자동 연결된다.
/// 반면 PopupLookupEditWyn.LookupKey는 표준 RepositoryItemButtonEdit엔 없는 이 컨트롤만의 커스텀
/// 프로퍼티라, 셀 편집기(런타임에 새로 만들어지는 PopupLookupEditWyn 인스턴스)가 자동으로 이 값을
/// 물려받지 않는다 - 그래서 PopupLookupEditWyn.LookupKey 게터가 자기 필드가 비어있으면
/// (Properties as PopupLookupColumnEdit)?.LookupKey로 폴백하도록 따로 손봤다(PopupLookupEditWyn.cs
/// 참고) - 그리드 편집 중엔 실제로 Properties가 이 인스턴스를 가리키므로 그 경로로 값이 전달된다.
/// </summary>
[ToolboxItem(true)]
public class PopupLookupColumnEdit : RepositoryItemButtonEdit
{
    public const string CustomEditName = "PopupLookupColumnEdit";

    static PopupLookupColumnEdit()
    {
        // DevExpress 자체 "ButtonEdit" 등록 엔트리를 그대로 리플렉션해서 확인한 EditorType 이름 -
        // 설치된 DevExpress.XtraEditors.v21.2.dll을 직접 리플렉션해서 맞췄다(추측 금지 컨벤션,
        // LookUpColumnEdit 클래스 설명의 ControlNavigator 사례와 같은 이유). EditorType만
        // PopupLookupEditWyn으로 바꾸고 ViewInfo/Painter는 ButtonEdit 것을 그대로 재사용한다.
        EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(
            CustomEditName,
            typeof(PopupLookupEditWyn),
            typeof(PopupLookupColumnEdit),
            typeof(ButtonEditViewInfo),
            new ButtonEditPainter(),
            true));
    }

    public PopupLookupColumnEdit()
    {
        NullText = string.Empty;
    }

    /// <summary>ButtonEdit 계열 공통 버그(LookUpColumnEdit.EndInit 참고) - Designer의 BeginInit/
    /// EndInit 구간을 지나면서 코드로 등록하지 않은 버튼이 비워질 수 있어서, 여기서 다시 채워
    /// 넣는다. 화면마다 Designer.cs에 Buttons.AddRange를 직접 안 넣어도 항상 "..." 버튼이 보인다.</summary>
    public override void EndInit()
    {
        base.EndInit();
        if (Buttons.Count == 0)
        {
            Buttons.Add(new EditorButton(ButtonPredefines.Ellipsis, "...") { Width = 24 });
        }
    }

    public override string EditorTypeName => CustomEditName;

    /// <summary>어느 팝업을 열지(sysPopUpM.popup_key), 예: "P_MENU". PopupLookupEditWyn.LookupKey와
    /// 완전히 같은 방식 - 이 값만 지정하면 셀 편집 중 "..." 버튼으로 팝업이 뜬다.</summary>
    [Category("WYNLAB")]
    [Description("sysPopUpM에 등록해둔 팝업 이름(popup_key). 이 값만 지정하면 \"...\" 버튼으로 팝업이 뜹니다.")]
    [DefaultValue(null)]
    public string? LookupKey { get; set; }
}
