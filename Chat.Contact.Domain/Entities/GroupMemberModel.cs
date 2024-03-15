using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Results;

namespace Chat.Contact.Domain.Entities;

public class GroupMemberModel : IRepositoryItem
{
    public string Id { get; private set; }
    public string GroupId { get; private set; }
    public string MemberId { get; private set; }
    public string AddedBy { get; private set; }
    public DateTime JoinedAt { get; private set; }

    private GroupMemberModel(string groupId, string memberId, string addedBy)
    {
        Id = Guid.NewGuid().ToString();
        GroupId = groupId;
        MemberId = memberId;
        AddedBy = addedBy;
        JoinedAt = DateTime.UtcNow;
    }

    public static IResult<GroupMemberModel> Create(string groupId, string memberId, string addedBy)
    {
        return Result.Success(new GroupMemberModel(groupId, memberId, addedBy));
    }
}
