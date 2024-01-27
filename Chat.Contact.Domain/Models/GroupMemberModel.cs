namespace Chat.Contact.Domain.Models;

public class GroupMemberModel
{
    public string Id { get; set; }
    public string GroupId { get; set; }
    public string MemberId { get; set; }
    public string AddedBy { get; set; }
    public DateTime JoinedAt { get; set; }

    public GroupMemberModel(string groupId, string memberId, string addedBy)
    {
        Id = Guid.NewGuid().ToString();
        GroupId = groupId;
        MemberId = memberId;
        AddedBy = addedBy;
        JoinedAt = DateTime.UtcNow;
    }
}
