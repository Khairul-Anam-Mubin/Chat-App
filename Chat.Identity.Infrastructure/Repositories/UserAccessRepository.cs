using Peacious.Framework.DDD;
using Peacious.Framework.EDD;
using Peacious.Framework.Extensions;
using Peacious.Framework.ORM;
using Peacious.Framework.ORM.Builders;
using Peacious.Framework.ORM.Enums;
using Peacious.Framework.ORM.Interfaces;
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
