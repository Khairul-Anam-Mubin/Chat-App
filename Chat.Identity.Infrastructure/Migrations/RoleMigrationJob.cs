using Chat.Framework.ORM.Interfaces;
using Chat.Identity.Domain.Constants;
using Chat.Identity.Domain.Entities;
using Chat.Identity.Domain.Repositories;

namespace Chat.Identity.Infrastructure.Migrations;

public class RoleMigrationJob : IMigrationJob
{
    private readonly IRoleRepository _roleRepository;

    public RoleMigrationJob(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task MigrateAsync()
    {
        var roles = new List<Role>
        {
            Role.Create(Roles.Visitor, string.Empty),
            Role.Create(Roles.Admin, string.Empty)
        };

        await _roleRepository.SaveAsync(roles);
    }
}
