using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;

namespace WYNLAB.Base.Controls;

/// <summary>
/// DateEditWyn(패널용 독립 컨트롤)의 SmartDateParser 규칙(2자리/4자리/8자리 자동인식)을 그리드
/// 컬럼 편집기(GridColumn.ColumnEdit)에서도 그대로 쓰기 위한 RepositoryItem 버전 - LookUpColumnEdit과
/// 완전히 같은 목적/패턴이다(2026-09-09 요청 - "그리드에 사용하는 날짜 컨트롤도 만들어서 동일한
/// 기능으로 적용"). Designer의 그리드 "Edit Repository..." 대화상자에서 바로 추가해서 쓰면 된다 -
/// 별도 속성 설정 없이도 파싱 규칙 + 캘린더 버튼이 항상 붙어 있다.
///
/// DevExpress에 커스텀 RepositoryItem을 등록하는 표준 방법(LookUpColumnEdit과 같은 컨벤션,
/// 추측 금지) - EditorClassInfo 생성자 파라미터(name, editorType, repositoryType, viewInfoType,
/// painter, designTimeVisible)와 "DateEdit" 기본 등록 항목의 ViewInfoType/Painter는 설치된
/// DevExpress.XtraEditors.v21.2.dll을 리플렉션으로 직접 확인해서 맞췄다(DateEditViewInfo /
/// ButtonEditPainter).
/// </summary>
public class DateColumnEdit : RepositoryItemDateEdit
{
    public const string CustomEditName = "DateColumnEdit";

    static DateColumnEdit()
    {
        EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(
            CustomEditName,
            typeof(DateEditWyn),
            typeof(DateColumnEdit),
            typeof(DateEditViewInfo),
            new ButtonEditPainter(),
            true));
    }

    public DateColumnEdit()
    {
        // DateEditWyn 생성자와 같은 이유(그 클래스 설명 참고) - 대화형 마스크를 꺼서 순수 텍스트
        // 입력으로 바꿔야 SmartDateParser가 온전한 문자열을 받는다.
        Mask.MaskType = MaskType.None;
        ParseEditValue += DateColumnEdit_ParseEditValue;
        CustomDisplayText += DateColumnEdit_CustomDisplayText;
    }

    public override string EditorTypeName => CustomEditName;

    /// <summary>LookUpColumnEdit.EndInit과 같은 이유 - DateEdit 계열은 생성자에서 기본 캘린더
    /// 버튼이 Buttons에 있지만, Designer의 BeginInit/EndInit 구간을 지나면서 비워져서 실제로는
    /// 안 보인다(DateEditWyn.OnHandleCreated와 같은 버그, RepositoryItem은 OnHandleCreated가
    /// 없어서 대신 EndInit()에서 고친다). 버튼 종류는 DateEditWyn과 같은 이유로 Combo여야 한다
    /// (Glyph는 이미지 없이는 안 그려짐 - 리플렉션으로 확인한 DevExpress 기본값도 Combo).</summary>
    public override void EndInit()
    {
        base.EndInit();
        if (Buttons.Count == 0)
        {
            Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        }
    }

    private static void DateColumnEdit_ParseEditValue(object? sender, DevExpress.XtraEditors.Controls.ConvertEditValueEventArgs e)
        => SmartDateParser.Handle(e);

    /// <summary>그리드 셀의 실제 데이터가 DateTime이 아니라 "yyyyMMdd" 원시 문자열(TBAEMP.ent_date
    /// 등 VARCHAR(8) 감사컬럼 관례, DateEditWyn과 같은 이유)이라 ParseEditValue(편집 중에만 동작)
    /// 만으로는 편집 안 하고 있을 때(그리드 보기 모드)의 표시가 그대로 "20260911"처럼 날짜
    /// 형식이 아니게 보인다(2026-09-11 실제 발견 - "grd1의 날짜 컬럼들이 날짜 형식으로 보이지
    /// 않아"). CustomDisplayText는 편집 중이 아닐 때도(그리드 셀 렌더링 시) 불리므로 여기서
    /// SmartDateParser로 원시 값을 해석해서 화면표시용 문자열로 바꿔준다.</summary>
    private static void DateColumnEdit_CustomDisplayText(object? sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
    {
        if (SmartDateParser.TryParse(e.Value?.ToString(), out var date))
            e.DisplayText = date.ToString("yyyy-MM-dd");
    }
}
