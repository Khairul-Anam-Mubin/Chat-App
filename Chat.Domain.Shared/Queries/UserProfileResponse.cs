using Chat.Domain.Shared.Entities;
using Peacious.Framework.Pagination;

namespace Chat.Domain.Shared.Queries;

public class UserProfileResponse : PaginationResponse<UserProfile>
{
    public List<UserProfile> Profiles { get; set; }

    public UserProfileResponse()
    {
        Profiles = new List<UserProfile>();
    }
}