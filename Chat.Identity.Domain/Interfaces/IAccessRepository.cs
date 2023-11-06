using Chat.Framework.Database.Interfaces;
using Chat.Identity.Domain.Models;

namespace Chat.Identity.Domain.Interfaces;

public interface IAccessRepository : IRepository<AccessModel>
{
    Task<bool> DeleteAllTokenByAppId(string appId);

    Task<bool> DeleteAllTokensByUserId(string userId);
}