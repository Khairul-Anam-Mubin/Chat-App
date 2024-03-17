using Chat.Framework.Database.ORM.Interfaces;
using Chat.Identity.Domain.Entities;

namespace Chat.Identity.Domain.Repositories;

public interface ITokenRepository : IRepository<Token>
{
    Task<bool> RevokeAllTokenByAppIdAsync(string appId);

    Task<bool> RevokeAllTokensByUserIdAsync(string userId);
}