namespace WYNLAB.Api.Authorization;

/// <summary>MustChangePasswordFilter가 이 표시가 붙은 액션만 mustChangePwd=Y 토큰으로도
/// 통과시킨다 - AuthController.ChangePassword 하나에만 붙는다.</summary>
[AttributeUsage(AttributeTargets.Method)]
public class AllowWhilePasswordChangeRequiredAttribute : Attribute
{
}
