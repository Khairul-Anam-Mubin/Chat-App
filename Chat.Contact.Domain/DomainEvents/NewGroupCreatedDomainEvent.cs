using Chat.Framework.DDD;

namespace Chat.Contact.Domain.DomainEvents;

public class NewGroupCreatedDomainEvent : IDomainEvent
{
    public string Id { get; set; }
    public string GroupName { get; set; }
    public string CreatedBy { get; set; }

    public NewGroupCreatedDomainEvent(string id, string groupName, string createdBy)
    {
        Id = id;
        GroupName = groupName;
        CreatedBy = createdBy;
    }
}
