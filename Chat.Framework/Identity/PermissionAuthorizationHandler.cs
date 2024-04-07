using Microsoft.AspNetCore.Authorization;

namespace Chat.Framework.Identity
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            PermissionRequirement requirement)
        {
            if (context.User?.Claims is null)
            {
                return;
            }
            
            var permissions = context.User.Claims
                .Where(x => x.Type == "permissions")
                .Select(x => x.Value)
                .ToHashSet();

            if (permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }
            
            await Task.CompletedTask;
        }
    }
}
