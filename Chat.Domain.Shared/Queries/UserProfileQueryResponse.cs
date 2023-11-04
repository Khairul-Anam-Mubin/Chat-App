using Chat.Domain.Shared.Entities;
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