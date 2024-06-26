using Peacious.Framework.Extensions;
using Peacious.Framework.ORM;
using Peacious.Framework.ORM.Builders;
using Peacious.Framework.ORM.Enums;
using Peacious.Framework.ORM.Interfaces;
using Chat.Identity.Domain.Entities;
using Chat.Identity.Domain.Repositories;
using Microsoft.Extensions.Configuration;

namespace Chat.Identity.Infrastructure.Repositories;

public class TokenRepository : RepositoryBase<Token>, ITokenRepository
{
    public TokenRepository(IDbContextFactory dbContextFactory, IConfiguration configuration)
        : base(configuration.TryGetConfig<DatabaseInfo>("DatabaseInfo"), dbContextFactory.GetDbContext(Context.Mongo))
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