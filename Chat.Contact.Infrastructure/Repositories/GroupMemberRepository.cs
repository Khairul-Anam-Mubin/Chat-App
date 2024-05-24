using Chat.Contacts.Domain.Entities;
using Chat.Contacts.Domain.Repositories;
using Peacious.Framework.DDD;
using Peacious.Framework.EDD;
using Peacious.Framework.ORM;
using Peacious.Framework.ORM.Builders;
using Peacious.Framework.ORM.Enums;
using Peacious.Framework.ORM.Interfaces;

namespace Chat.Contacts.Infrastructure.Repositories;

public class GroupMemberRepository : RepositoryBaseWrapper<GroupMember>, IGroupMemberRepository
{
    public GroupMemberRepository(DatabaseInfo databaseInfo, IDbContextFactory dbContextFactory, IEventService eventService) 
        : base(databaseInfo, dbContextFactory.GetDbContext(Context.Mongo), eventService) {}

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
