﻿using Chat.Contact.Domain.Interfaces;
using Chat.Contact.Domain.Models;
using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Contact.Infrastructure.Repositories;

public class GroupMemberRepository : RepositoryBase<GroupMemberModel>, IGroupMemberRepository
{
    public GroupMemberRepository(DatabaseInfo databaseInfo, IDbContextFactory dbContextFactory)
        : base(databaseInfo, dbContextFactory.GetDbContext(Context.Mongo)) {}

    public async Task<bool> IsUserAlreadyExistsInGroupAsync(string groupId, string userId)
    {
        var filterBuilder = new FilterBuilder<GroupMemberModel>();
        var groupIdFilter = filterBuilder.Eq(o => o.GroupId, groupId);
        var memberIdFilter = filterBuilder.Eq(o => o.MemberId, userId);
        var filter = filterBuilder.And(groupIdFilter, memberIdFilter);
        return await DbContext.CountAsync<GroupMemberModel>(DatabaseInfo, filter) > 0;
    }
}