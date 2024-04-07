using Chat.Domain.Shared.Constants;
using Chat.Framework.ORM.Interfaces;
using Chat.Identity.Domain.Entities;
using Chat.Identity.Domain.Repositories;

namespace Chat.Identity.Infrastructure.Migrations;

public class PermissionMigrationJob : IMigrationJob
{
    private readonly IPermissionRepository _permissionRepository;

    public PermissionMigrationJob(IPermissionRepository permissionRepository) 
    {
        _permissionRepository = permissionRepository;
    }

    public async Task MigrateAsync()
    {
        var permissions = new List<Permission>
        {
            Permission.Create(Permissions.User),
            Permission.Create(Permissions.UserCreate),
            Permission.Create(Permissions.UserRead),
            Permission.Create(Permissions.UserUpdate),
            Permission.Create(Permissions.UserDelete),

            Permission.Create(Permissions.Contact),
            Permission.Create(Permissions.ContactCreate),
            Permission.Create(Permissions.ContactRead),
            Permission.Create(Permissions.ContactUpdate),
            Permission.Create(Permissions.ContactDelete),
        };

        await _permissionRepository.SaveAsync(permissions);
    }
}
