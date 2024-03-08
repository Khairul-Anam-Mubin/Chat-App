using Chat.Activity.Domain.Entities;
using Chat.Activity.Domain.Repositories;
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

    public async Task<bool> TrackLastSeenAsync(string userId, bool isActive)
    {
        var userIdFilter = new FilterBuilder<LastSeenModel>().Eq(o => o.UserId, userId);
        
        var update =
            new UpdateBuilder<LastSeenModel>()
            .Set(o => o.IsActive, isActive)
            .Set(o => o.LastSeenAt, DateTime.UtcNow)
            .Build();
        
        return await DbContext.UpdateOneAsync<LastSeenModel>(DatabaseInfo, userIdFilter, update);
    }
}