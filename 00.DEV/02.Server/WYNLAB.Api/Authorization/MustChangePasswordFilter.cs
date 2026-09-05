using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WYNLAB.Api.Authorization;

/// <summary>
/// 모든 컨트롤러 액션에 전역으로 걸린다(Program.cs의 AddControllers 옵션 참고). 로그인 응답의
/// RequirePasswordChange=true는 클라이언트(LoginForm)가 강제 변경 다이얼로그를 먼저 띄우라는
/// 뜻인데, 그건 어디까지나 클라이언트 쪽 약속이라 API를 직접 두드리면 건너뛸 수 있다 - 그
/// 우회를 서버에서도 막는다. mustChangePwd=Y 토큰으로는 [AllowWhilePasswordChangeRequired]가
/// 붙은 액션(비밀번호 변경 자체) 말고는 전부 403.
///
/// [AllowAnonymous] 액션(로그인, 비밀번호 찾기)은 애초에 이 토큰을 아직 안 가진 상태라
/// User.FindFirstValue가 null을 돌려주므로 자연히 통과한다(막을 필요도 없음).
/// </summary>
public class MustChangePasswordFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var exempt = context.ActionDescriptor.EndpointMetadata
            .OfType<AllowWhilePasswordChangeRequiredAttribute>().Any();

        if (!exempt)
        {
            var mustChangePwd = context.HttpContext.User.FindFirstValue("mustChangePwd");
            if (mustChangePwd == "Y")
            {
                context.Result = new ObjectResult(new { success = false, message = "비밀번호를 먼저 변경해야 합니다." })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
                return;
            }
        }

        await next();
    }
}
