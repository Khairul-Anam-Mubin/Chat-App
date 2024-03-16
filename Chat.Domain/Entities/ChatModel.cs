using Chat.Domain.DomainEvents;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.DDD;
using Chat.Framework.Results;

namespace Chat.Domain.Entities;

public class ChatModel : AEntity, IRepositoryItem
{
    public string UserId { get; private set; }
    public string SendTo { get; private set; }
    public string Message { get; private set; }
    public DateTime SentAt { get; private set; }
    public string Status { get; private set; }
    public bool IsGroupMessage { get; private set; }

    private ChatModel(string userId, string sendTo, string message, bool isGroupMessage)
        : base(Guid.NewGuid().ToString())
    {
        UserId = userId;
        SendTo = sendTo;
        Message = message;
        SentAt = DateTime.UtcNow;
        Status = "Sent";
        IsGroupMessage = isGroupMessage;
    }

    public static IResult<ChatModel> Create(string userId, string sendTo, string message, bool isGroupMessage)
    {
        var chat = new ChatModel(userId, sendTo, message, isGroupMessage);

        chat.RaiseDomainEvent(new ChatCreatedDomainEvent(chat));

        return Result.Success(chat);
    }

    public void MessageSeen()
    {
        Status = "Seen";
    }
}