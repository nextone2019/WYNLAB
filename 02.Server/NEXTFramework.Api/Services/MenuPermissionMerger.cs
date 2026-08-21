using NEXTFramework.Api.Repositories;
using NEXTFramework.Shared.Dtos;

namespace NEXTFramework.Api.Services;

/// <summary>
/// TSMMENUAUTH 원본 행들을 받아 메뉴별 최종 권한(MenuDto)으로 병합한다.
///
/// 규칙:
/// 1) 같은 메뉴에 대해 사용자가 속한 모든 그룹(GRP) 권한을 OR로 합산
///    (하나의 그룹에서라도 Y면 최종 Y)
/// 2) 그 메뉴에 개인(USER) 권한 행이 별도로 존재하면, 그룹 합산 결과를 개인 권한으로 완전히 덮어씀
///    (개인 예외 부여/회수 용도)
/// </summary>
public static class MenuPermissionMerger
{
    public static List<MenuDto> Merge(List<MenuRow> menus, List<MenuAuthRow> authRows)
    {
        var groupAuthByMenu = authRows
            .Where(a => a.AuthTargetType == "GRP")
            .GroupBy(a => a.MenuCd);

        var userAuthByMenu = authRows
            .Where(a => a.AuthTargetType == "USER")
            .ToDictionary(a => a.MenuCd);

        var mergedGroupAuth = groupAuthByMenu.ToDictionary(
            g => g.Key,
            g => new MenuAuthRow
            {
                MenuCd = g.Key,
                ViewYn = OrYn(g.Select(x => x.ViewYn)),
                InsertYn = OrYn(g.Select(x => x.InsertYn)),
                UpdateYn = OrYn(g.Select(x => x.UpdateYn)),
                DeleteYn = OrYn(g.Select(x => x.DeleteYn)),
                ExcelYn = OrYn(g.Select(x => x.ExcelYn)),
            });

        var result = new List<MenuDto>();

        foreach (var menu in menus)
        {
            // 개인 권한이 있으면 그것을 최우선 적용, 없으면 그룹 합산 결과, 둘 다 없으면 전부 비허용
            var finalAuth = userAuthByMenu.TryGetValue(menu.MenuCd, out var userAuth)
                ? userAuth
                : mergedGroupAuth.GetValueOrDefault(menu.MenuCd);

            result.Add(new MenuDto
            {
                MenuCd = menu.MenuCd,
                MenuNm = menu.MenuNm,
                UpperMenuCd = menu.UpperMenuCd,
                MenuLevel = menu.MenuLevel,
                MenuType = menu.MenuType,
                FormClassNm = menu.FormClassNm,
                IconNm = menu.IconNm,
                SortOrder = menu.SortOrder,
                ViewYn = finalAuth?.ViewYn == "Y",
                InsertYn = finalAuth?.InsertYn == "Y",
                UpdateYn = finalAuth?.UpdateYn == "Y",
                DeleteYn = finalAuth?.DeleteYn == "Y",
                ExcelYn = finalAuth?.ExcelYn == "Y",
            });
        }

        return result;
    }

    private static string OrYn(IEnumerable<string> values) =>
        values.Any(v => v == "Y") ? "Y" : "N";
}
