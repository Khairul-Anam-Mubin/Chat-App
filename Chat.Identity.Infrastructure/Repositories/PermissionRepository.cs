using Chat.Framework.DDD;
using Chat.Framework.EDD;
using Chat.Framework.Extensions;
using Chat.Framework.ORM;
using Chat.Framework.ORM.Enums;
using Chat.Framework.ORM.Interfaces;
using Chat.Identity.Domain.Entities;
using Chat.Identity.Domain.Repositories;
using Microsoft.Extensions.Configuration;

namespace Chat.Identity.Infrastructure.Repositories;

public class PermissionRepository : RepositoryBaseWrapper<Permission>, IPermissionRepository
{
    public PermissionRepository(IConfiguration configuration, IDbContextFactory dbContextFactory, IEventService eventService) 
        : base(configuration.TryGetConfig<DatabaseInfo>("DatabaseInfo"), dbContextFactory.GetDbContext(Context.Mongo), eventService) {}

    public async Task<List<Permission>> GetChildPermissionsAsync(string permissionId)
    {
        var permission = await GetByIdAsync(permissionId);

        if (permission is null) return new List<Permission>();

        return await GetManyByIdsAsync(permission.PermissionIds);
    }

    public async Task<List<Permission>> GetFlatChildPermissionsAsync(string permissionId)
    {
        var flatPermissions = new List<Permission>();
        
        var childPermissions = await GetChildPermissionsAsync(permissionId);

        while (childPermissions.Any())
        {
            flatPermissions.AddRange(childPermissions);

            var childPermissionIds = new List<string>();

            childPermissions.ForEach(childPermission => childPermissionIds.AddRange(childPermission.PermissionIds));

            childPermissions = await GetManyByIdsAsync(childPermissionIds);
        }

        return flatPermissions;
    }
}
