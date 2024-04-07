using Chat.Domain.Shared.Constants;
using Chat.Framework.ORM.Interfaces;
using Chat.Identity.Domain.Entities;
using Chat.Identity.Domain.Repositories;

namespace Chat.Identity.Infrastructure.Migrations;

public class RoleMigrationJob : IMigrationJob
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;

    public RoleMigrationJob(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
    }

    public async Task MigrateAsync()
    {
        var roles = new List<Role>
        {
            Role.Create(Roles.Admin, string.Empty)
        };

        await AddDeveloperRolesAsync(roles);
        await AddVisitorRolesAsync(roles);

        await _roleRepository.SaveAsync(roles);
    }

    private async Task AddDeveloperRolesAsync(List<Role> roles)
    {
        var developerRole = Role.Create(Roles.Developer, string.Empty);

        var permissions = await _permissionRepository.GetManyAsync();

        developerRole.AddPermissions(permissions);

        roles.Add(developerRole);
    }

    private async Task AddVisitorRolesAsync(List<Role> roles)
    {
        var visitorRole = Role.Create(Roles.Visitor, string.Empty);

        // will remove later
        var permissions = new List<Permission>
        {
            Permission.Create(Permissions.UserCreate),
            Permission.Create(Permissions.UserRead),
            Permission.Create(Permissions.UserUpdate),
        };

        visitorRole.AddPermissions(permissions);

        roles.Add(visitorRole);

        await Task.CompletedTask;
    }
}
