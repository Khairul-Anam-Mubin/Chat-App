using KCluster.Framework.DDD;
using KCluster.Framework.EDD;
using KCluster.Framework.Extensions;
using KCluster.Framework.ORM;
using KCluster.Framework.ORM.Builders;
using KCluster.Framework.ORM.Enums;
using KCluster.Framework.ORM.Interfaces;
using Chat.Identity.Domain.Entities;
using Chat.Identity.Domain.Repositories;
using Microsoft.Extensions.Configuration;

namespace Chat.Identity.Infrastructure.Repositories;

public class UserAccessRepository : RepositoryBaseWrapper<UserAccess>, IUserAccessRepository
{
    public UserAccessRepository(IConfiguration configuration, IDbContextFactory dbContextFactory, IEventService eventService) 
        : base(configuration.TryGetConfig<DatabaseInfo>("DatabaseInfo"), dbContextFactory.GetDbContext(Context.Mongo), eventService)
    {}

    public async Task<UserAccess?> GetUserAccessByUserIdAsync(string userId)
    {
        var userIdFilter = new FilterBuilder<UserAccess>().Eq(user => user.UserId, userId);

        return await DbContext.GetOneAsync<UserAccess>(DatabaseInfo, userIdFilter);
    }
}
