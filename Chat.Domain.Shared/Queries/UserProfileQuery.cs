using Chat.Domain.Shared.Entities;
using Chat.Framework.RequestResponse;

namespace Chat.Domain.Shared.Queries;

public class UserProfileQuery : APaginationQuery<UserProfile>
{
    public List<string>? UserIds { get; set; }
    public List<string>? Emails { get; set; }
}