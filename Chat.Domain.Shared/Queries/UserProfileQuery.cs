using Chat.Domain.Shared.Entities;
using Peacious.Framework.CQRS;
using Peacious.Framework.Pagination;

namespace Chat.Domain.Shared.Queries;

public class UserProfileQuery : APaginationQuery<UserProfile>, IQuery<UserProfileResponse>, IInternalMessage
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