using Chat.Contacts.Domain.Entities;
using Chat.Contacts.Domain.Repositories;
using Chat.Framework.DDD;
using Chat.Framework.EDD;
using Chat.Framework.ORM;
using Chat.Framework.ORM.Builders;
using Chat.Framework.ORM.Enums;
using Chat.Framework.ORM.Interfaces;

namespace Chat.Contacts.Infrastructure.Repositories;

public class GroupRepository : RepositoryBaseWrapper<Group>, IGroupRepository
{
    private readonly IGroupMemberRepository _groupMemberRepository;

    public GroupRepository(IDbContextFactory dbContextFactory, DatabaseInfo databaseInfo, IEventService eventService, IGroupMemberRepository groupMemberRepository)
        : base(databaseInfo, dbContextFactory.GetDbContext(Context.Mongo), eventService) 
    {
        _groupMemberRepository = groupMemberRepository;
    }

    public async Task<List<Group>> GetGroupsByGroupIds(List<string> groupIds)
    {
        var filter = new FilterBuilder<Group>().In(o => o.Id, groupIds);
        return await DbContext.GetManyAsync<Group>(DatabaseInfo, filter);
    }

    public async Task<Group?> GetGroupByIdAsync(string groupId)
    {
        return await DbContext.GetByIdAsync<Group>(DatabaseInfo, groupId);
    }

    public override async Task<bool> SaveAsync(Group group)
    {
        if (group.Members.Any())
        {
            await _groupMemberRepository.SaveAsync(group.Members);
        }

        return await base.SaveAsync(group);
    }

    public async Task<bool> IsUserAlreadyExistInGroupAsync(string groupId, string userId)
    {
        return await _groupMemberRepository.IsUserAlreadyExistInGroupAsync(groupId, userId);
    }
}