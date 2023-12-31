using Chat.Activity.Domain.Interfaces.Repositories;
using Chat.Activity.Domain.Models;
using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Activity.Infrastructure.Repositories;

public class LastSeenRepository : RepositoryBase<LastSeenModel>, ILastSeenRepository
{
    public LastSeenRepository(IDbContextFactory dbContextFactory, DatabaseInfo databaseInfo) 
        : base(databaseInfo, dbContextFactory.GetDbContext(Context.Mongo)) 
    {}

    public async Task<LastSeenModel?> GetLastSeenModelByUserIdAsync(string userId)
    {
        var filterBuilder = new FilterBuilder<LastSeenModel>();
        var userIdFilter = filterBuilder.Eq(o => o.UserId, userId);
        
        return await DbContext.GetOneAsync<LastSeenModel>(DatabaseInfo, userIdFilter);
    }

    public async Task<List<LastSeenModel>> GetLastSeenModelsByUserIdsAsync(List<string> userIds)
    {
        var filterBuilder = new FilterBuilder<LastSeenModel>();
        var userIdsFilter = filterBuilder.In(o => o.UserId, userIds);
        return await DbContext.GetManyAsync<LastSeenModel>(DatabaseInfo, userIdsFilter);
    }
}