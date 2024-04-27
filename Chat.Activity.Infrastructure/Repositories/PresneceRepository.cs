using Chat.Activity.Domain.Entities;
using Chat.Activity.Domain.Repositories;
using KCluster.Framework.ORM;
using KCluster.Framework.ORM.Builders;
using KCluster.Framework.ORM.Enums;
using KCluster.Framework.ORM.Interfaces;

namespace Chat.Activity.Infrastructure.Repositories;

public class PresneceRepository : RepositoryBase<Presence>, IPresenceRepository
{
    public PresneceRepository(IDbContextFactory dbContextFactory, DatabaseInfo databaseInfo) 
        : base(databaseInfo, dbContextFactory.GetDbContext(Context.Mongo)) 
    {}

    public async Task<Presence?> GetPresenceByUserIdAsync(string userId)
    {
        var filterBuilder = new FilterBuilder<Presence>();
        var userIdFilter = filterBuilder.Eq(o => o.UserId, userId);
        
        return await DbContext.GetOneAsync<Presence>(DatabaseInfo, userIdFilter);
    }

    public async Task<List<Presence>> GetPresenceListByUserIdsAsync(List<string> userIds)
    {
        var filterBuilder = new FilterBuilder<Presence>();
        var userIdsFilter = filterBuilder.In(o => o.UserId, userIds);
        return await DbContext.GetManyAsync<Presence>(DatabaseInfo, userIdsFilter);
    }

    public async Task<bool> TrackPresenceAsync(string userId)
    {
        var userIdFilter = new FilterBuilder<Presence>().Eq(o => o.UserId, userId);
        
        var update =
            new UpdateBuilder<Presence>()
            .Set(o => o.LastSeenAt, DateTime.UtcNow)
            .Build();
        
        return await DbContext.UpdateOneAsync<Presence>(DatabaseInfo, userIdFilter, update);
    }
}