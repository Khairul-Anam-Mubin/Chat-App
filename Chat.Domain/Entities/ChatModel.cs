using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Results;

namespace Chat.Domain.Entities;

public class ChatModel : IEntity
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; private set; }
    public string SendTo { get; private set; }
    public string Message { get; private set; }
    public DateTime SentAt { get; private set; }
    public string Status { get; private set; }
    public bool IsGroupMessage { get; private set; }

    private ChatModel(string userId, string sendTo, string message, bool isGroupMessage)
    {
        Id = Guid.NewGuid().ToString();
        UserId = userId;
        SendTo = sendTo;
        Message = message;
        SentAt = DateTime.UtcNow;
        Status = "Sent";
        IsGroupMessage = isGroupMessage;
    }

    public static IResult<ChatModel> Create(string userId, string sendTo, string message, bool isGroupMessage)
    {
        return Result.Success(new ChatModel(userId, sendTo, message, isGroupMessage));
    }

    public void MessageSeen()
    {
        Status = "Seen";
    }
}