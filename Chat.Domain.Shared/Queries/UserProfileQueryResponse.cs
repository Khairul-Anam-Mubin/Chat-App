using Chat.Domain.Shared.Models;

namespace Chat.Domain.Shared.Queries;

public class UserProfileQueryResponse
{
    public List<UserProfile> Profiles { get; set; }

    public UserProfileQueryResponse()
    {
        Profiles = new List<UserProfile>();
    }
}