using Chat.Framework.ORM.Interfaces;
using Chat.Identity.Domain.Entities;

namespace Chat.Identity.Domain.Repositories;

public interface IUserAccessRepository : IRepository<UserAccess>
{
    Task<UserAccess?> GetUserAccessByUserIdAsync(string userId);
}
