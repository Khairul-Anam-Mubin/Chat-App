using Chat.Domain.DomainEvents;
using Peacious.Framework.DDD;
using Peacious.Framework.ORM.Interfaces;
using Peacious.Framework.Results;

namespace Chat.Domain.Entities;

public class Message : Entity, IRepositoryItem
{
    public string ConversationId { get; private set; }
    public string SenderId { get; private set; }
    public string ReceiverId { get; private set; }
    public string Content { get; private set; }
    public DateTime SentAt { get; private set; }
    public string Status { get; private set; }
    public bool IsGroupMessage { get; private set; }

    private Message(string conversationId, string senderId, string receiverId, string messageContent, bool isGroupMessage)
        : base(Guid.NewGuid().ToString())
    {
        SenderId = senderId;
        ReceiverId = receiverId;
        Content = messageContent;
        SentAt = DateTime.UtcNow;
        Status = MessageStatus.Sent;
        IsGroupMessage = isGroupMessage;
        ConversationId = conversationId;
    }

    public static IResult<Message> Create(string conversationId, string senderId, string receiverId, string messageContent, bool isGroupMessage)
    {
        var message = new Message(conversationId, senderId, receiverId, messageContent, isGroupMessage);

        message.RaiseDomainEvent(new MessageCreatedDomainEvent(message.Id));

        return Result.Success(message);
    }

    public void MessageSeen()
    {
        Status = MessageStatus.Seen;
    }
}

public class MessageStatus
{
    public const string Seen = "Seen";
    public const string Sent = "Sent";
    public const string Delivered = "Delivered";
}