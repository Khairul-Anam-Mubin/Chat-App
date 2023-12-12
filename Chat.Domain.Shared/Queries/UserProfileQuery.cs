using Chat.Domain.Shared.Entities;
using Chat.Framework.CQRS;
using Chat.Framework.Pagination;

namespace Chat.Domain.Shared.Queries;

public class UserProfileQuery : APaginationQuery<UserProfile>, IQuery
{
    public List<string>? UserIds { get; set; }
    public List<string>? Emails { get; set; }
}