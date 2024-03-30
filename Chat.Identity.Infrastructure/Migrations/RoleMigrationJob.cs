using Chat.Framework.ORM.Interfaces;
using Chat.Identity.Domain.Constants;
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
            Role.Create(Roles.Visitor, string.Empty),
            Role.Create(Roles.Admin, string.Empty)
        };

        await AddDeveloperRolesAsync(roles);

        await _roleRepository.SaveAsync(roles);
    }

    private async Task AddDeveloperRolesAsync(List<Role> roles)
    {
        var developerRole = Role.Create(Roles.Developer, string.Empty);

        var permissions = await _permissionRepository.GetManyAsync();

        developerRole.AddPermissions(permissions);

        roles.Add(developerRole);
    }
}
