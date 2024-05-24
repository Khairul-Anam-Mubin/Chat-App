using Chat.Identity.Domain.Entities;
using Peacious.Framework.ORM.Interfaces;

namespace Chat.Identity.Domain.Repositories;

public interface IPermissionRepository : IRepository<Permission>
{
    Task<List<Permission>> GetChildPermissionsAsync(string permissionId);
    Task<List<Permission>> GetFlatChildPermissionsAsync(string permissionId);
}
