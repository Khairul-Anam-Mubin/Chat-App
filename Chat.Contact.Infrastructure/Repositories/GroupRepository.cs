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

    public async Task<List<GroupModel>> GetGroupsByGroupIds(List<string> groupIds)
    {
        var filter = new FilterBuilder<GroupModel>().In(o => o.Id, groupIds);
        return await DbContext.GetManyAsync<GroupModel>(DatabaseInfo, filter);
    }
}