using Chat.Identity.Domain.Entities;

namespace Chat.Identity.Domain.Services;

public interface IAccessService
{
    Task AddPermissionToUserAsync(string userId, Permission permission);
    Task AddRoleToUserAsync(string userId, Role role);
    Task AddPermissionsToUserAsync(string userId, List<Permission> permissions);
    Task AddRolesToUserAsync(string userId, List<Role> roles);
    Task<List<Permission>> GetUserPermissionsAsync(string userId);
    Task<List<Role>> GetUserRolesAsync(string userId);
}
