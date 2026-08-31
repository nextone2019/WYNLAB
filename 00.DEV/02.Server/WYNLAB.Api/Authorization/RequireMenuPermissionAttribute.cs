using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WYNLAB.Api.Services;

namespace WYNLAB.Api.Authorization;

public enum MenuAction
{
    View,
    Insert,
    Update,
    Delete,
    Print,
    Excel
}

/// <summary>
/// 컨트롤러 액션 하나에 특정 메뉴(TSMMENU.MENU_CD)의 특정 권한(조회/등록/수정/삭제/엑셀)이
/// 있어야만 통과시킨다. [Authorize]는 "로그인했는가"만 확인하고 그 이상은 검증하지 않아서,
/// 로그인한 사용자라면 누구나 다른 사람 데이터를 등록/수정/삭제할 수 있었다 - 그 화면의
/// 등록/수정/삭제 버튼 활성화 여부(CanInsert 등)는 지금까지 클라이언트에서만 판단했기 때문.
/// 이 필터가 서버에서도 같은 권한(TSMMENUAUTH, MenuPermissionService가 병합)을 한 번 더
/// 확인해서, API를 직접 호출하는 우회 시도까지 막는다.
///
/// Action 필터([Authorize]가 처리하는 Authorization 필터보다 나중 단계)로 구현해서, 이 필터가
/// 실행되는 시점엔 JWT 인증이 이미 끝나 있고 HttpContext.User에 클레임이 채워져 있다고
/// 보장할 수 있다.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class RequireMenuPermissionAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _menuCd;
    private readonly MenuAction _action;

    public RequireMenuPermissionAttribute(string menuCd, MenuAction action)
    {
        _menuCd = menuCd;
        _action = action;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var permissionService = context.HttpContext.RequestServices.GetRequiredService<IMenuPermissionService>();
        var permission = await permissionService.GetEffectivePermissionAsync(userId, _menuCd);

        var allowed = permission != null && _action switch
        {
            MenuAction.View => permission.ViewYn,
            MenuAction.Insert => permission.InsertYn,
            MenuAction.Update => permission.UpdateYn,
            MenuAction.Delete => permission.DeleteYn,
            MenuAction.Print => permission.PrintYn,
            MenuAction.Excel => permission.ExcelYn,
            _ => false
        };

        if (!allowed)
        {
            context.Result = new ObjectResult(new { success = false, message = "이 작업에 대한 권한이 없습니다." })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return;
        }

        await next();
    }
}
