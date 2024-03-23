using Chat.Identity.Domain.Entities;
using Chat.Identity.Domain.Repositories;

namespace Chat.Identity.Domain.Services;

public class AccessService : IAccessService
{
    private readonly IAccessRepository _accessRepository;
    
    public AccessService(IAccessRepository accessRepository)
    {
        _accessRepository = accessRepository;
    }
    
    public async Task AddPermissionsToUserAsync(string userId, List<Permission> permissions)
    {
        var permissionsAccess = new List<PermissionAccess>();
        permissions.ForEach(permission =>
        {
            permissionsAccess.Add
            (
                PermissionAccess.Create(permission.Id,userId, default, true)    
            );
        });

        await _accessRepository.SaveUserPermissionsAsync(permissionsAccess);
    }

    public async Task AddPermissionToUserAsync(string userId, Permission permission)
    {
        var permissionAccess = PermissionAccess.Create(permission.Id, userId, default, true);

        await _accessRepository.SaveUserPermissionsAsync(new List<PermissionAccess> { permissionAccess});
    }

    public async Task AddRolesToUserAsync(string userId, List<Role> roles)
    {
        var rolesAccess = new List<RoleAccess>();
        roles.ForEach(roleAccess =>
        {
            rolesAccess.Add
            (
                RoleAccess.Create(roleAccess.Id, userId, default, true)
            );
        });

        await _accessRepository.SaveUserRolesAsync(rolesAccess);
    }

    public async Task AddRoleToUserAsync(string userId, Role role)
    {
        var roleAccess = RoleAccess.Create(role.Id, userId, default, true);

        await _accessRepository.SaveUserRolesAsync(
            new List<RoleAccess> { roleAccess });
    }
}
