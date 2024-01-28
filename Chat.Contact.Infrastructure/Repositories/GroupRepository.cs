using Chat.Contact.Domain.Interfaces;
using Chat.Contact.Domain.Models;
using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Contact.Infrastructure.Repositories;

public class GroupRepository : RepositoryBase<GroupModel>, IGroupRepository
{
    public GroupRepository(IDbContextFactory dbContextFactory, DatabaseInfo databaseInfo) 
        : base(databaseInfo, dbContextFactory.GetDbContext(Context.Mongo)) {}

    public async Task<bool> IsGroupCreatorAsync(string groupId, string userId)
    {
        var filterBuilder = new FilterBuilder<GroupModel>();

        var groupIdFilter = filterBuilder.Eq(o => o.Id, groupId);
        var createdByFilter = filterBuilder.Eq(o => o.CreatedBy, userId);
        
        var filter = filterBuilder.And(groupIdFilter, createdByFilter);
        
        return await DbContext.CountAsync<GroupModel>(DatabaseInfo, filter) > 0;
    }
}