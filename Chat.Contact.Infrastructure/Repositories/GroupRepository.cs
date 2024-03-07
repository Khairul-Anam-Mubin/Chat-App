using Chat.Contact.Domain.Models;
using Chat.Contact.Domain.Repositories;
using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Contact.Infrastructure.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly IDbContext _dbContext;
    private readonly DatabaseInfo _databaseInfo;

    public GroupRepository(IDbContextFactory dbContextFactory, DatabaseInfo databaseInfo)
    {
        _dbContext = dbContextFactory.GetDbContext(Context.Mongo);
        _databaseInfo = databaseInfo;
    }

    public async Task<List<GroupModel>> GetGroupsByGroupIds(List<string> groupIds)
    {
        var filter = new FilterBuilder<GroupModel>().In(o => o.Id, groupIds);
        return await _dbContext.GetManyAsync<GroupModel>(_databaseInfo, filter);
    }

    public async Task<GroupModel?> GetGroupByIdAsync(string groupId)
    {
        return await _dbContext.GetByIdAsync<GroupModel>(_databaseInfo, groupId);
    }

    public async Task<bool> SaveAsync(GroupModel group)
    {
        var result = await _dbContext.SaveAsync(_databaseInfo, group);

        result &= await SaveGroupMembersAsync(group.Members());

        return result;
    }

    public async Task<List<GroupMemberModel>> GetAllGroupMembers(string groupId)
    {
        var filterBuilder = new FilterBuilder<GroupMemberModel>();
        var groupIdFilter = filterBuilder.Eq(o => o.GroupId, groupId);
        return await _dbContext.GetManyAsync<GroupMemberModel>(_databaseInfo, groupIdFilter);
    }

    public async Task<List<GroupMemberModel>> GetUserGroupsAsync(string userId)
    {
        var filter = new FilterBuilder<GroupMemberModel>().Eq(o => o.MemberId, userId);
        return await _dbContext.GetManyAsync<GroupMemberModel>(_databaseInfo, filter);
    }

    public async Task<bool> IsUserAlreadyExistsInGroupAsync(string groupId, string userId)
    {
        var filterBuilder = new FilterBuilder<GroupMemberModel>();
        var groupIdFilter = filterBuilder.Eq(o => o.GroupId, groupId);
        var memberIdFilter = filterBuilder.Eq(o => o.MemberId, userId);
        var filter = filterBuilder.And(groupIdFilter, memberIdFilter);
        return await _dbContext.CountAsync<GroupMemberModel>(_databaseInfo, filter) > 0;
    }

    public async Task<bool> SaveGroupMembersAsync(List<GroupMemberModel> groupMembers)
    {
        if (!groupMembers.Any())
        {
            return false;
        }
        return await _dbContext.SaveManyAsync(_databaseInfo, groupMembers);
    }
}