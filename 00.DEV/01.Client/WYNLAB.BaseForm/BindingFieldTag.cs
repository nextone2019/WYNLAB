namespace WYNLAB.Base;

/// <summary>
/// 개발자용 마우스오버 툴팁("BindingField : xxx")이 읽어갈 컬럼명을 담는 전용 래퍼 -
/// Control.Tag에 그냥 문자열을 넣지 않는 이유는 BaseForm.ApplyBindingFieldTooltips 설명 참고
/// (frmShortcut.cs가 이미 Tag를 다른 용도로 쓰고 있어서 타입으로 구분해야 안전하다).
///
/// 사용법: 컨트롤 선언 시 한 줄만 추가하면 된다 - 예:
///   private readonly TextEditWyn txtDeptCd = new() { Tag = new BindingFieldTag("dept_cd") };
/// 그리드/트리 컬럼은 FieldName이 이미 실제 DB 컬럼명이라 이거 없이도 자동 적용된다.
/// </summary>
public sealed class BindingFieldTag
{
    public string Field { get; }

    public BindingFieldTag(string field) => Field = field;
}
