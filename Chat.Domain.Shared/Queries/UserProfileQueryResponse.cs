using Chat.Domain.Shared.Models;
using Chat.Framework.CQRS;

namespace Chat.Domain.Shared.Queries;

public class UserProfileQueryResponse : QueryResponse
{
    public List<UserProfile> Profiles { get; set; }

    public UserProfileQueryResponse()
    {
        Profiles = new List<UserProfile>();
    }
}