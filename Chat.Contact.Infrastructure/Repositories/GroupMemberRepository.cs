using Chat.Contacts.Domain.Entities;
using Chat.Contacts.Domain.Repositories;
using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.DDD;
using Chat.Framework.EDD;

namespace Chat.Contacts.Infrastructure.Repositories;

public class GroupMemberRepository : RepositoryBaseWrapper<GroupMember>, IGroupMemberRepository
{
    public GroupMemberRepository(DatabaseInfo databaseInfo, IDbContext dbContext, IEventService eventService) 
        : base(databaseInfo, dbContext, eventService) {}

    public async Task<List<GroupMember>> GetAllGroupMembersAsync(string groupId)
    {
        var filterBuilder = new FilterBuilder<GroupMember>();
        
        var groupIdFilter = filterBuilder.Eq(o => o.GroupId, groupId);

        return await DbContext.GetManyAsync<GroupMember>(DatabaseInfo, groupIdFilter);
    }

    public async Task<List<GroupMember>> GetUserGroupsAsync(string userId)
    {
        var filter = new FilterBuilder<GroupMember>().Eq(o => o.MemberId, userId);

        return await DbContext.GetManyAsync<GroupMember>(DatabaseInfo, filter);
    }

    public async Task<bool> IsUserAlreadyExistInGroupAsync(string groupId, string userId)
    {
        var filterBuilder = new FilterBuilder<GroupMember>();
        var groupIdFilter = filterBuilder.Eq(o => o.GroupId, groupId);
        var memberIdFilter = filterBuilder.Eq(o => o.MemberId, userId);
        var filter = filterBuilder.And(groupIdFilter, memberIdFilter);

        return await DbContext.CountAsync<GroupMember>(DatabaseInfo, filter) > 0;
    }
}
