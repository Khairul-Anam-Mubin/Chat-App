using Microsoft.AspNetCore.Authorization;

namespace Chat.Framework.Identity;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission) 
        : base(policy : permission) {}
}
