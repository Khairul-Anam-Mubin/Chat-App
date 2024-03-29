using Chat.Framework.ORM.Interfaces;
using Chat.Identity.Domain.Entities;

namespace Chat.Identity.Domain.Repositories;

public interface IPermissionRepository : IRepository<Permission>
{
    Task<List<Permission>> GetChildPermissionsAsync(string permissionId);
    Task<List<Permission>> GetFlatChildPermissionsAsync(string permissionId);
}
