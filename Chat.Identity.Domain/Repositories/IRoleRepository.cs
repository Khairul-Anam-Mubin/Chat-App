using Chat.Framework.ORM.Interfaces;
using Chat.Identity.Domain.Entities;

namespace Chat.Identity.Domain.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetRoleByTitleAsync(string title);
    Task<List<Role>> GetRolesByTitlesAsync(List<string> titles);
}
