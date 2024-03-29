using Chat.Framework.ORM;
using Chat.Framework.ORM.Builders;
using Chat.Framework.ORM.Enums;
using Chat.Framework.ORM.Interfaces;
using Chat.Identity.Domain.Entities;
using Chat.Identity.Domain.Repositories;

namespace Chat.Identity.Infrastructure.Repositories;

public class TokenRepository : RepositoryBase<Token>, ITokenRepository
{
    public TokenRepository(IDbContextFactory dbContextFactory, DatabaseInfo databaseInfo)
        : base(databaseInfo, dbContextFactory.GetDbContext(Context.Mongo))
    { }

    public async Task<bool> RevokeAllTokenByAppIdAsync(string appId)
    {
        var filterBuilder = new FilterBuilder<Token>();
        var filter = filterBuilder.Eq(o => o.AppId, appId);
        return await DbContext.DeleteManyAsync<Token>(DatabaseInfo, filter);
    }

    public async Task<bool> RevokeAllTokensByUserIdAsync(string userId)
    {
        var filterBuilder = new FilterBuilder<Token>();
        var filter = filterBuilder.Eq(o => o.UserId, userId);
        return await DbContext.DeleteManyAsync<Token>(DatabaseInfo, filter);
    }
}