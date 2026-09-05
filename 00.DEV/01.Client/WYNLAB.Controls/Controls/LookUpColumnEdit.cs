using System.ComponentModel;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;

namespace WYNLAB.Base.Controls;

/// <summary>
/// LookUpEditWyn(패널용 독립 컨트롤)의 LookupKey 한 줄짜리 편의를 그리드 컬럼 편집기
/// (GridColumn.ColumnEdit)에서도 그대로 쓰기 위한 RepositoryItem 버전. 지금까지는 코드로
/// LookUpEditWyn.CreateGridRepositoryItemAsync(key)를 호출해 컬럼마다 화면 생성자에 비동기
/// 셋업 메서드를 만들어야 했는데(frmItem.SetupUnitToLookupAsync 참고), 이 클래스는 Designer의
/// 그리드 "Edit Repository..." 대화상자에서 바로 추가해서 속성창에 LookupKey만 채우면 끝나게
/// 한다(2026-09-04, 사장님 지시 - "속성에서 추가해서 사용할 수 있도록 wyn 붙여서 만든 것처럼").
///
/// LookUpEditWyn과 마찬가지로 "살아있는" 바인딩이다 - LookupKey를 바꾸면(Designer 속성창에서든
/// 코드에서든) 그때마다 다시 조회한다. CreateGridRepositoryItemAsync는 반대로 호출 시점 스냅샷
/// 한 번뿐이라 LookUp 정의가 바뀌어도 화면 재시작 전엔 안 바뀐다 - 이 차이가 필요 없다면(대부분의
/// 코드성 목록은 자주 안 바뀜) 여전히 CreateGridRepositoryItemAsync를 써도 된다.
///
/// DevExpress에 커스텀 RepositoryItem을 등록하는 표준 방법(정적 생성자에서
/// EditorRegistrationInfo.Default.Editors.Add) - 실제 시그니처는 설치된 DevExpress.XtraEditors.
/// v21.2.dll을 리플렉션으로 직접 확인해서 맞췄다(추측 금지 컨벤션, GridViewWynBehavior의
/// ControlNavigator 사례와 같은 이유) - LookUpEdit 자신의 등록 엔트리를 그대로 읽어서 Painter/
/// ViewInfo 타입을 그대로 재사용했다.
///
/// 클래스 이름은 원래 RepositoryItemLookUpEditWyn이었는데, Designer의 ColumnEdit 드롭다운에
/// 새 빌드 반영 후에도 안 뜨는지 확인하는 과정에서 사장님 지시로 LookUpColumnEdit으로 변경
/// (2026-09-04) - 등록 이름(EditorTypeName)도 같이 바꿨으니 이전 이름으로 저장된 Designer 코드가
/// 있다면 다시 저장해야 한다(현재는 아직 실제로 쓰인 화면이 없어서 해당 없음).
/// </summary>
public class LookUpColumnEdit : RepositoryItemLookUpEdit
{
    public const string CustomEditName = "LookUpColumnEdit";

    static LookUpColumnEdit()
    {
        EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(
            CustomEditName,
            typeof(LookUpEditWyn),
            typeof(LookUpColumnEdit),
            typeof(LookUpEditViewInfo),
            new LookUpEditPainter(),
            true));
    }

    public LookUpColumnEdit()
    {
        NullText = string.Empty;
    }

    /// <summary>LookUpEdit 계열은 생성자에서 기본 콤보 버튼(드롭다운 삼각형)이 Buttons에 있지만,
    /// Designer의 BeginInit/EndInit 구간을 지나면서(코드에 Buttons.AddRange를 안 넣어주면) 비워져서
    /// 실제로는 삼각형이 안 보인다 - LookUpEditWyn.OnHandleCreated와 같은 버그(2026-09-04, 사용자
    /// 등록화면의 grd1 "사용" 컬럼에서 실제로 겪음). LookUpEditWyn은 Control이라 OnHandleCreated에서
    /// 고치지만, RepositoryItem은 그게 없어서 대신 EndInit()에서 고친다 - 이렇게 하면 화면마다
    /// Designer.cs에 Buttons.AddRange를 직접 안 넣어도(까먹어도) 항상 삼각형이 보인다.</summary>
    public override void EndInit()
    {
        base.EndInit();
        if (Buttons.Count == 0)
        {
            Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        }
    }

    public override string EditorTypeName => CustomEditName;

    private string? _lookupKey;

    /// <summary>sysLookupM에 등록해둔 LookUp 이름. LookUpEditWyn.LookupKey와 완전히 같은 방식 -
    /// 이 값만 지정하면(Designer 속성창 또는 코드) 조회가 실행되고 코드/명 2열 팝업이 채워진다.</summary>
    [Category("WYNLAB")]
    [Description("sysLookupM에 등록해둔 LookUp 이름. 이 값만 지정하면 코드/명 목록이 자동으로 채워집니다.")]
    [DefaultValue(null)]
    public string? LookupKey
    {
        get => _lookupKey;
        set { _lookupKey = value; _ = LoadFromLookupKeyAsync(); }
    }

    private async Task LoadFromLookupKeyAsync()
    {
        var lookupKey = _lookupKey;
        if (string.IsNullOrEmpty(lookupKey) || ComboLookupProvider.Fetch == null) return;

        try
        {
            var result = await ComboLookupProvider.Fetch(lookupKey!, new Dictionary<string, string?>());
            if (lookupKey != _lookupKey) return; // 응답 오는 사이 LookupKey가 또 바뀌었으면 버림

            var items = result.Items;
            items.Insert(0, new CodeLookupItem()); // 값 지우기(빈 값으로 되돌리기) 가능하게 - LookUpEditWyn과 같은 이유

            Columns.Clear();
            var multiColumn = ComboLookupColumnBuilder.Build(items, result.Columns);
            if (multiColumn != null)
            {
                foreach (var col in multiColumn.Value.Columns) Columns.Add(col);
                ValueMember = nameof(CodeLookupItem.Value);
                DisplayMember = nameof(CodeLookupItem.Display);
                DataSource = multiColumn.Value.Table;
            }
            else
            {
                Columns.Add(new LookUpColumnInfo(nameof(CodeLookupItem.Value), "코드", 80));
                Columns.Add(new LookUpColumnInfo(nameof(CodeLookupItem.Display), "명칭"));
                ValueMember = nameof(CodeLookupItem.Value);
                DisplayMember = nameof(CodeLookupItem.Display);
                DataSource = items;
            }
            PopupWidth = 260;
            AutoSearchColumnIndex = 1;
            ShowFooter = false;
        }
        catch
        {
            // 목록 하나 못 불러온다고 화면 전체가 죽으면 안 됨 - LookUpEditWyn과 같은 이유.
        }
    }
}
