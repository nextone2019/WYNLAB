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
/// 컨트롤러 액션 하나에 특정 메뉴(TSMMENU)의 특정 권한(조회/등록/수정/삭제/엑셀)이
/// 있어야만 통과시킨다. [Authorize]는 "로그인했는가"만 확인하고 그 이상은 검증하지 않아서,
/// 로그인한 사용자라면 누구나 다른 사람 데이터를 등록/수정/삭제할 수 있었다 - 그 화면의
/// 등록/수정/삭제 버튼 활성화 여부(CanInsert 등)는 지금까지 클라이언트에서만 판단했기 때문.
/// 이 필터가 서버에서도 같은 권한(TSMMENUAUTH, MenuPermissionService가 병합)을 한 번 더
/// 확인해서, API를 직접 호출하는 우회 시도까지 막는다.
///
/// 어느 메뉴인지는 MENU_ID(정수)가 아니라 Module+ScreenClassNm(예: "SM"+"frmMenu") 두 문자열로
/// 지정한다 - MENU_ID는 DB마다(로컬/서버) IDENTITY로 따로 채번돼서 같은 값이 환경마다 다른
/// 메뉴를 가리킬 수 있는데, 이 어트리뷰트 값은 컴파일된 DLL 하나에 고정돼서 여러 환경에 그대로
/// 배포되므로 그 값이 절대 흔들리면 안 된다. Module/ScreenClassNm은 실제 화면 클래스 이름
/// 그대로라(개발자가 정하는 값, DB가 채번하는 값이 아님) 어느 환경이든 항상 동일하다.
///
/// Action 필터([Authorize]가 처리하는 Authorization 필터보다 나중 단계)로 구현해서, 이 필터가
/// 실행되는 시점엔 JWT 인증이 이미 끝나 있고 HttpContext.User에 클레임이 채워져 있다고
/// 보장할 수 있다.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class RequireMenuPermissionAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _module;
    private readonly string _screenClassNm;
    private readonly MenuAction _action;

    public RequireMenuPermissionAttribute(string module, string screenClassNm, MenuAction action)
    {
        _module = module;
        _screenClassNm = screenClassNm;
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
        var permission = await permissionService.GetEffectivePermissionByKeyAsync(userId, _module, _screenClassNm);

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
