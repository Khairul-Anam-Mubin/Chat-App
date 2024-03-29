using Chat.Identity.Domain.Entities;
using Chat.Identity.Domain.Repositories;

namespace Chat.Identity.Domain.Services;

public class AccessService : IAccessService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUserAccessRepository _userAccessRepository;

    public AccessService(IRoleRepository roleRepository, IPermissionRepository permissionRepository, IUserAccessRepository userAccessRepository)
    {
        _permissionRepository = permissionRepository;
        _roleRepository = roleRepository;
        _userAccessRepository = userAccessRepository;
    }

    public async Task<List<Role>> GetUserRolesAsync(string userId)
    {
        var userAccess = await _userAccessRepository.GetUserAccessByUserIdAsync(userId);

        if (userAccess is null) return new List<Role>();

        return await _roleRepository.GetManyByIds(userAccess.RoleIds);
    }

    public async Task<List<string>> GetUserPermissionIdsAsync(string userId)
    {
        var userAccess = await _userAccessRepository.GetUserAccessByUserIdAsync(userId);

        if (userAccess is null) return new List<string>();

        var roles = await _roleRepository.GetManyByIds(userAccess.RoleIds);

        var rolePermissionIds = new List<string>();

        roles.ForEach(role => rolePermissionIds.AddRange(role.PermissionIds));

        var distinctPermissionIds = new HashSet<string>();

        rolePermissionIds.ForEach(id => distinctPermissionIds.Add(id));

        userAccess.PermissionIds.ForEach(id => distinctPermissionIds.Add(id));

        return distinctPermissionIds.ToList();
    }

    public async Task<List<string>> GetUserFlatPermissionsAsync(string userId)
    {
        var userPermissionIds = await GetUserPermissionIdsAsync(userId);

        var permissions = await _permissionRepository.GetManyByIds(userPermissionIds);

        var flatPermissions = new List<Permission>();

        foreach (var permission in permissions)
        {
            flatPermissions.AddRange(await _permissionRepository.GetFlatChildPermissionsAsync(permission.Id));
        }

        flatPermissions.AddRange(permissions);

        var distinctPermissions = new HashSet<string>();

        flatPermissions.ForEach(permission => distinctPermissions.Add(permission.Title));

        return distinctPermissions.ToList();
    }
}
