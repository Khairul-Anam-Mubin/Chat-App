using Microsoft.AspNetCore.Authorization;

namespace Chat.Framework.Identity;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public PermissionRequirement(string permission) 
    {
        Permission = permission;
    }
}