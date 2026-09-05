using WYNLAB.Api.Repositories.SM;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Services;

/// <summary>
/// TSMMENUAUTH 원본 행들을 받아 메뉴별 최종 권한(MenuDto)으로 병합한다.
///
/// 규칙(합산 방식 - 개인 권한은 그룹 권한을 대체하지 않고 항상 "추가"만 됨):
/// 같은 메뉴에 대해 사용자가 속한 모든 그룹(GRP) 권한 + 본인(USER) 개인 권한을
/// 전부 OR로 합산한다 (그중 하나라도 Y면 최종 Y). 그룹 소속을 늘리거나 개인 권한을
/// 추가로 부여하는 것은 항상 권한을 늘리는 방향으로만 동작하고, 개인 설정으로
/// 그룹 권한보다 더 적게 줄 수는 없다(그런 예외가 필요하면 별도 그룹으로 분리해서 운영).
/// </summary>
public static class MenuPermissionMerger
{
    public static List<MenuDto> Merge(List<MenuRow> menus, List<MenuAuthRow> authRows)
    {
        // GRP/USER 구분 없이 같은 메뉴에 걸린 모든 권한행을 그냥 다 같이 OR 합산한다.
        var authByMenu = authRows.ToLookup(a => a.MenuId);

        var result = new List<MenuDto>();

        foreach (var menu in menus)
        {
            var rows = authByMenu[menu.MenuId].ToList();

            result.Add(new MenuDto
            {
                MenuId = menu.MenuId,
                MenuNm = menu.MenuNm,
                UpperMenuId = menu.UpperMenuId,
                MenuLevel = menu.MenuLevel,
                MenuType = menu.MenuType,
                Module = menu.Module,
                ScreenClassNm = menu.ScreenClassNm,
                IconNm = menu.IconNm,
                SortOrder = menu.SortOrder,
                ViewYn = OrYn(rows.Select(r => r.ViewYn)),
                InsertYn = OrYn(rows.Select(r => r.InsertYn)),
                UpdateYn = OrYn(rows.Select(r => r.UpdateYn)),
                DeleteYn = OrYn(rows.Select(r => r.DeleteYn)),
                PrintYn = OrYn(rows.Select(r => r.PrintYn)),
                ExcelYn = OrYn(rows.Select(r => r.ExcelYn)),
                Auth = MergeAuth(rows),
            });
        }

        // 리프 메뉴(FORM)에 조회권한을 줬다고 해서 그 상위 그룹(GROUP)들까지 자동으로 조회
        // 가능해지는 게 아니다 - TSMMENUAUTH에 그룹 자체를 대상으로 한 행이 따로 없으면 그
        // 그룹은 ViewYn=false로 계산되고, 클라이언트(ShellForm)는 ViewYn=true인 메뉴만으로
        // 트리를 만들기 때문에 상위 폴더가 없어서 리프까지 같이 안 보이게 된다(실제로 겪음 -
        // frmUserAuth로 리프만 체크해도 상위그룹 자동체크가 없어서 관리자가 정상적으로 권한을
        // 줘도 그대로 재현됨). 여기서 한 번에 바로잡는다 - 조회 가능한 메뉴가 하나라도 있으면
        // 그 메뉴까지 가는 경로의 모든 상위 그룹도 조회 가능하게 강제로 켠다.
        // "이미 true면 그 위 조상도 이미 처리됐다"고 보고 중간에 멈추면 안 된다 - 그 부모 자신이
        // (이 리프와 무관하게) 자기 권한행만으로 이미 true였을 수도 있는데, 그 경우 그 부모의
        // 조상까지 처리됐다는 보장이 없다. 그래서 매번 뿌리까지 끝까지 올라간다(메뉴 트리가
        // 몇십 개 수준이라 비용은 무시할 만하다) - 이미 true인 조상을 다시 true로 덮어써도 결과는 같다.
        var byMenuId = result.ToDictionary(m => m.MenuId);
        foreach (var menu in result.Where(m => m.ViewYn).ToList())
        {
            var upperId = menu.UpperMenuId;
            while (upperId.HasValue && byMenuId.TryGetValue(upperId.Value, out var parent))
            {
                parent.ViewYn = true;
                upperId = parent.UpperMenuId;
            }
        }

        // 반대 방향 정리: GROUP 자신에게 직접 권한행이 있어서(위 로직과 무관하게 원래도 이미
        // ViewYn=true였던 경우) 하위에 조회 가능한 메뉴가 하나도 없는데도 트리에 빈 폴더로
        // 남는 경우가 있다(2026-09-02 발견 - 상위그룹 자동체크 로직을 넣고 나니 반대 케이스도
        // 눈에 띔). FORM 리프를 하나도 조회 못 하는 GROUP은 화면에 보여줘도 열어볼 게 없으므로
        // 강제로 ViewYn=false로 되돌린다. 리프의 ViewYn(위에서 이미 확정됨)만 보고 재귀적으로
        // 판단하므로, 어떤 GROUP을 여기서 꺼도 그 판단 근거가 된 리프의 상태나 다른 GROUP의
        // 판단에는 영향이 없다(자기 자신의 존재 여부만 결정됨).
        var childrenByParent = result.Where(m => m.UpperMenuId.HasValue)
            .ToLookup(m => m.UpperMenuId!.Value);
        var visibleLeafCache = new Dictionary<long, bool>();
        bool HasVisibleLeafDescendant(MenuDto group)
        {
            if (visibleLeafCache.TryGetValue(group.MenuId, out var cached)) return cached;
            visibleLeafCache[group.MenuId] = false; // 순환 참조 방지용 임시값
            var found = childrenByParent[group.MenuId].Any(child =>
                child.MenuType == "FORM" ? child.ViewYn : HasVisibleLeafDescendant(child));
            visibleLeafCache[group.MenuId] = found;
            return found;
        }
        foreach (var group in result.Where(m => m.MenuType == "GROUP" && m.ViewYn))
        {
            if (!HasVisibleLeafDescendant(group))
                group.ViewYn = false;
        }

        return result;
    }

    private static bool OrYn(IEnumerable<string> values) => values.Any(v => v == "Y");

    private static bool[] MergeAuth(List<MenuAuthRow> rows) => new[]
    {
        OrYn(rows.Select(r => r.Auth01)), OrYn(rows.Select(r => r.Auth02)), OrYn(rows.Select(r => r.Auth03)),
        OrYn(rows.Select(r => r.Auth04)), OrYn(rows.Select(r => r.Auth05)), OrYn(rows.Select(r => r.Auth06)),
        OrYn(rows.Select(r => r.Auth07)), OrYn(rows.Select(r => r.Auth08)), OrYn(rows.Select(r => r.Auth09)),
        OrYn(rows.Select(r => r.Auth10)),
    };
}
