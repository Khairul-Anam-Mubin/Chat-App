using Chat.Identity.Domain.Entities;
using KCluster.Framework.ORM.Interfaces;

namespace Chat.Identity.Domain.Repositories;

public interface ITokenRepository : IRepository<Token>
{
    Task<bool> RevokeAllTokenByAppIdAsync(string appId);

    Task<bool> RevokeAllTokensByUserIdAsync(string userId);
}