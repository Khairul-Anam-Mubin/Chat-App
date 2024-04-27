using Chat.Identity.Domain.Entities;
using KCluster.Framework.ORM.Interfaces;

namespace Chat.Identity.Domain.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetRoleByTitleAsync(string title);
    Task<List<Role>> GetRolesByTitlesAsync(List<string> titles);
}
