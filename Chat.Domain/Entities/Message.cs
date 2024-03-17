using Chat.Domain.DomainEvents;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.DDD;
using Chat.Framework.Results;

namespace Chat.Domain.Entities;

public class Message : Entity, IRepositoryItem
{
    public string UserId { get; private set; }
    public string SendTo { get; private set; }
    public string Content { get; private set; }
    public DateTime SentAt { get; private set; }
    public string Status { get; private set; }
    public bool IsGroupMessage { get; private set; }

    private Message(string userId, string sendTo, string messageContent, bool isGroupMessage)
        : base(Guid.NewGuid().ToString())
    {
        UserId = userId;
        SendTo = sendTo;
        Content = messageContent;
        SentAt = DateTime.UtcNow;
        Status = "Sent";
        IsGroupMessage = isGroupMessage;
    }

    public static IResult<Message> Create(string userId, string sendTo, string messageContent, bool isGroupMessage)
    {
        var message = new Message(userId, sendTo, messageContent, isGroupMessage);

        message.RaiseDomainEvent(new MessageCreatedDomainEvent(message));

        return Result.Success(message);
    }

    public void MessageSeen()
    {
        Status = "Seen";
    }
}