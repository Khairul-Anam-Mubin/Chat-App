using Chat.Contacts.Domain.Entities;
using Chat.Contacts.Domain.Repositories;
using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Contacts.Infrastructure.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly IDbContext _dbContext;
    private readonly DatabaseInfo _databaseInfo;

    public GroupRepository(IDbContextFactory dbContextFactory, DatabaseInfo databaseInfo)
    {
        _dbContext = dbContextFactory.GetDbContext(Context.Mongo);
        _databaseInfo = databaseInfo;
    }

    public async Task<List<Group>> GetGroupsByGroupIds(List<string> groupIds)
    {
        var filter = new FilterBuilder<Group>().In(o => o.Id, groupIds);
        return await _dbContext.GetManyAsync<Group>(_databaseInfo, filter);
    }

    public async Task<Group?> GetGroupByIdAsync(string groupId)
    {
        return await _dbContext.GetByIdAsync<Group>(_databaseInfo, groupId);
    }

    public async Task<bool> SaveAsync(Group group)
    {
        var result = await _dbContext.SaveAsync(_databaseInfo, group);

        result &= await SaveGroupMembersAsync(group.Members());

        return result;
    }

    public async Task<List<GroupMember>> GetAllGroupMembers(string groupId)
    {
        var filterBuilder = new FilterBuilder<GroupMember>();
        var groupIdFilter = filterBuilder.Eq(o => o.GroupId, groupId);
        return await _dbContext.GetManyAsync<GroupMember>(_databaseInfo, groupIdFilter);
    }

    public async Task<List<GroupMember>> GetUserGroupsAsync(string userId)
    {
        var filter = new FilterBuilder<GroupMember>().Eq(o => o.MemberId, userId);
        return await _dbContext.GetManyAsync<GroupMember>(_databaseInfo, filter);
    }

    public async Task<bool> IsUserAlreadyExistsInGroupAsync(string groupId, string userId)
    {
        var filterBuilder = new FilterBuilder<GroupMember>();
        var groupIdFilter = filterBuilder.Eq(o => o.GroupId, groupId);
        var memberIdFilter = filterBuilder.Eq(o => o.MemberId, userId);
        var filter = filterBuilder.And(groupIdFilter, memberIdFilter);
        return await _dbContext.CountAsync<GroupMember>(_databaseInfo, filter) > 0;
    }

    public async Task<bool> SaveGroupMembersAsync(List<GroupMember> groupMembers)
    {
        if (!groupMembers.Any())
        {
            return false;
        }
        return await _dbContext.SaveManyAsync(_databaseInfo, groupMembers);
    }
}