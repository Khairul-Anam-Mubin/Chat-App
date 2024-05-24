using Peacious.Framework.DDD;

namespace Chat.Contacts.Domain.DomainEvents;

public class NewGroupCreatedDomainEvent : DomainEvent
{
    public string GroupName { get; set; }
    public string CreatedBy { get; set; }

    public NewGroupCreatedDomainEvent(string id, string groupName, string createdBy)
        : base(id)
    {
        GroupName = groupName;
        CreatedBy = createdBy;
    }
}
