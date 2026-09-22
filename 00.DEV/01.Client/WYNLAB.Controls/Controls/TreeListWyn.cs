using System.ComponentModel;
using System.Linq;
using DevExpress.XtraTreeList;

namespace WYNLAB.Base.Controls;

/// <summary>
/// 메뉴관리(frmMenu.menuTree)/권한부여(frmUserManage.authTree 등) 화면이 각자 손으로 반복해서
/// 맞추던 트리 공통 스타일(모듈/그룹/leaf 행 배색, 줄 높이, 라인 옵션)을 뽑아낸 TreeList -
/// TabControlWyn을 ShellForm의 탭 그리기 로직에서 뽑아낸 것과 같은 방식이다.
///
/// 컬럼 구성(메뉴명 하나만 쓸지, 권한부여처럼 체크박스 컬럼 5개를 더 붙일지)은 화면마다 다르므로
/// 여기서 미리 정하지 않는다 - GridControlWyn이 컬럼을 미리 정하지 않는 것과 같은 이유. 이 컨트롤은
/// "모듈/그룹/leaf 행을 배경색으로 구분한다"는 공통 규칙만 담당한다.
///
/// 모듈(최상위)/그룹(중간 폴더)/leaf 구분은 데이터에 "MenuType" 필드가 있고 그 값이 "GROUP"인지,
/// "MenuLevel"이 1(최상위)인지로 판단한다(TSMMENU.MENU_TYPE/MENU_LEVEL 규약과 동일) - 메뉴/권한
/// 트리는 전부 이 값을 이미 갖고 있어서 별도 매핑 없이 그대로 맞는다.
///
/// [MenuType이 없는 트리(예: 부서 계층)] "MenuType" 컬럼 자체가 없는 데이터에 바인딩하면
/// (2026-08-28, frmDept 부서 트리에서 처음 겪음) 배색 구분 기준이 없으므로 이 로직 자체를
/// 건너뛰고 DevExpress 기본 모양 그대로 둔다.
///
/// [아이콘은 여기서 다루지 않는다] 처음엔 이 컨트롤에 레벨별/타입별 아이콘 속성을 공용으로
/// 추가했었는데(2026-09-16), 화면마다 아이콘을 고르는 기준(레벨/특정 컬럼값/기타 조건)이 다
/// 달라서 공용 컨트롤에 미리 박아두면 오히려 안 쓰는 속성만 늘어났다 - 삭제했다. 아이콘이
/// 필요한 화면은 그 화면 코드에서 직접 ImageList(또는 SvgImageCollection)를 만들고
/// menuTree.NodeCellStyle(또는 menuTree.SelectImageList + e.Node.ImageIndex)로 원하는 조건에
/// 맞춰 붙이면 된다 - TreeListWyn은 그 이벤트를 이미 하나 쓰고 있지만 이벤트는 여러 핸들러를
/// 동시에 가질 수 있으므로(멀티캐스트), 화면 쪽에서 하나 더 구독해서 ImageIndex만 자기 조건으로
/// 덮어써도 안전하다.
/// </summary>
[ToolboxItem(true)]
public class TreeListWyn : TreeList
{
    public TreeListWyn()
    {
        // 그리드처럼 줄 사이에 선이 보였으면 좋겠다는 요청(2026-09-14) - 이전엔 선을 다 꺼서
        // 깔끔했지만, 행 경계가 안 보여서 오히려 구분이 안 된다는 피드백이었다.
        OptionsView.ShowIndicator = false;
        OptionsView.ShowHorzLines = true;
        OptionsView.ShowVertLines = true;
        RowHeight = 26; // 기본 행높이는 다소 빡빡해 보여서 살짝 여유를 줌(frmMenu.menuTree와 동일)
        Appearance.Row.Font = AppFonts.Body;

        NodeCellStyle += TreeListWyn_NodeCellStyle;
    }

    /// <summary>보이는 컬럼이 하나뿐이면 헤더 행 자체가 필요 없다(2026-09-14 - "회사명/부서명처럼
    /// 이름 하나만 있는 트리는 참고 이미지(BARO_CRM류)처럼 헤더 없이 이름만 깔끔하게 보이게
    /// 해달라"는 요청). 권한부여(frmUserAuth 등)처럼 체크박스 컬럼을 여러 개 나란히 쓰는 화면은
    /// 그 컬럼들을 구분할 라벨이 필요하므로 그대로 헤더가 보인다 - 컬럼이 2개 이상이면 이 로직이
    /// 아무것도 하지 않는다. 화면이 컬럼을 다 만든 뒤(InitializeComponent 끝)에야 최종 개수를
    /// 알 수 있어서 핸들 생성 시점에 판단한다.</summary>
    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        if (Columns.Count(c => c.Visible) <= 1)
            OptionsView.ShowColumns = false;
    }

    /// <summary>모듈(최상위 그룹, MENU_LEVEL=1)/그룹(그 아래 중간 폴더)/leaf(실제 화면) 3단계를
    /// 배경/글자색으로 구분한다. 색은 UiTheme(appsettings.json Theme 섹션)에서 가져와서, 트리를
    /// 쓰는 화면이 늘어나도 전부 같은 톤을 재사용하게 한다(frmMenu.BuildLeftPanel의 원본 로직).
    /// "MenuType" 컬럼이 없는 데이터(메뉴/권한 트리가 아닌 일반 계층 데이터, 예: 부서 트리)면
    /// 아무것도 안 하고 DevExpress 기본 모양 그대로 둔다.</summary>
    private void TreeListWyn_NodeCellStyle(object? sender, DevExpress.XtraTreeList.GetCustomNodeCellStyleEventArgs e)
    {
        if (Columns["MenuType"] != null)
        {
            var menuType = e.Node.GetValue("MenuType") as string;
            var isGroupType = menuType == "GROUP";
            var level = e.Node.GetValue("MenuLevel") as int?;
            var isModule = isGroupType && (level ?? 0) <= 1; // 최상위(모듈) - SM/BA/MA 같은 업무영역 자체

            e.Appearance.BackColor = isModule ? UiTheme.TreeModuleBackColor
                : isGroupType ? UiTheme.TreeGroupBackColor
                : UiTheme.TreeLeafBackColor;
            e.Appearance.ForeColor = isGroupType ? UiTheme.TreeGroupForeColor : UiTheme.TreeLeafForeColor;
            e.Appearance.Font = isGroupType ? AppFonts.BodyBold : AppFonts.Body;
            e.Appearance.Options.UseBackColor = true;
            e.Appearance.Options.UseForeColor = true;
            e.Appearance.Options.UseFont = true;
        }

        // 사용중지(USE_YN='N']) 행은 배색과 별개로 글자색을 흐리게 표시한다 - 메뉴관리(frmMenu)의
        // "삭제"는 실제로는 소프트삭제(USE_YN='N')라 목록엔 계속 남는데(재활성화 가능하도록
        // 일부러 안 지움), 화면상 아무 표시가 없어 "삭제가 안 됐다"고 오해하기 쉬웠다(2026-09-16
        // 실제 지적 - 좌측 실제 메뉴에선 사라졌는데 관리화면 트리엔 그대로 남아있어 보임). UseYn
        // 필드가 없는 트리(부서 계층 등)에는 GetValue가 null을 돌려줘 아무 영향 없다 - 그래서
        // MenuType 유무와 무관하게 항상 검사한다.
        if (e.Node.GetValue("UseYn") is bool useYn && !useYn)
        {
            e.Appearance.ForeColor = System.Drawing.Color.FromArgb(180, 180, 180);
            e.Appearance.Options.UseForeColor = true;
        }
    }
}
