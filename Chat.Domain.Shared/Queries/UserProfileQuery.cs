using Chat.Domain.Shared.Entities;
using Chat.Framework.CQRS;
using Chat.Framework.Pagination;

namespace Chat.Domain.Shared.Queries;

public class UserProfileQuery : APaginationQuery<UserProfile>, IQuery, IInternalMessage
{
    public List<string> UserIds { get; set; }
    public List<string> Emails { get; set; }
    public string? Token { get; set; }

    public UserProfileQuery()
    {
        UserIds = new List<string>();
        Emails = new List<string>();
    }
}