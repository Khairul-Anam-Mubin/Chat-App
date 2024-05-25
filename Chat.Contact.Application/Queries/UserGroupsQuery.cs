using Chat.Contacts.Domain.Entities;
using Peacious.Framework.CQRS;

namespace Chat.Contacts.Application.Queries;

public class UserGroupsQuery : IQuery<List<Group>> {}
