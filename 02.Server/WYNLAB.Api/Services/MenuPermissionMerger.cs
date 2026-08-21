using WYNLAB.Api.Repositories;
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
        var authByMenu = authRows.ToLookup(a => a.MenuCd);

        var result = new List<MenuDto>();

        foreach (var menu in menus)
        {
            var rows = authByMenu[menu.MenuCd].ToList();

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
                ViewYn = OrYn(rows.Select(r => r.ViewYn)),
                InsertYn = OrYn(rows.Select(r => r.InsertYn)),
                UpdateYn = OrYn(rows.Select(r => r.UpdateYn)),
                DeleteYn = OrYn(rows.Select(r => r.DeleteYn)),
                ExcelYn = OrYn(rows.Select(r => r.ExcelYn)),
            });
        }

        return result;
    }

    private static bool OrYn(IEnumerable<string> values) => values.Any(v => v == "Y");
}
