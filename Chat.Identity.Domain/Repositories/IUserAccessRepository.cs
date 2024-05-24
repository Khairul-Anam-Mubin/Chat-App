using Chat.Identity.Domain.Entities;
using Peacious.Framework.ORM.Interfaces;

namespace Chat.Identity.Domain.Repositories;

public interface IUserAccessRepository : IRepository<UserAccess>
{
    Task<UserAccess?> GetUserAccessByUserIdAsync(string userId);
}
