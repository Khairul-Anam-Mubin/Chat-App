﻿using Chat.Identity.Domain.Entities;

namespace Chat.Identity.Domain.Repositories;

public interface IAccessRepository
{
    Task SaveUserRolesAsync(List<RoleAccess> roles);
    Task SaveUserPermissionsAsync(List<PermissionAccess> permissions);
    Task<List<RoleAccess>> GetUserRolesAsync(string userId);
    Task<List<PermissionAccess>> GetUserPermissionsAsync(string userId);
    Task<List<Permission>> GetPermissionsAsync(List<string> permissionIds);
    Task<List<Role>> GetRolesAsync(List<string> roleIds);
}
