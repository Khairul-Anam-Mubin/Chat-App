using Chat.Framework.DDD;
using Chat.Framework.ORM.Interfaces;
using Chat.Framework.Results;
using Chat.Framework.Security;

namespace Chat.Domain.Entities;

public class Conversation : AggregateRoot, IRepositoryItem
{
    public string UserId { get; private set; }
    public string SendTo { get; private set; }
    public string LatestMessageId { get; private set; } 
    public string Content { get; private set; }
    public DateTime SentAt { get; private set; }
    public string Status { get; private set; }
    public bool IsGroupMessage { get; private set; }
    public int Occurrence { get; private set; }


    private List<Message> _messages = new();

    public List<Message> Messages
    {
        get
        {
            _messages ??= new();
            return _messages.ToList();
        }
    }

    private void AddMessage(Message message)
    {
        _messages ??= new();
        _messages.Add(message);
    }

    private Conversation(string id, string userId, string sendTo, string messageContent, DateTime sentAt, string status, bool isGroupMessage)
        : base(id)
    {
        UserId = userId;
        SendTo = sendTo;
        Content = messageContent;
        SentAt = sentAt;
        Status = status;
        IsGroupMessage = isGroupMessage;
        Occurrence = 0;
        LatestMessageId = string.Empty;
    }

    public static string GetConversationId(string userId, string sendTo)
    {
        return CheckSumGenerator.GetCheckSum(userId, sendTo);
    }

    public static Conversation Create(string id, string userId, string sendTo, string messageContent, DateTime sentAt, string status, bool isGroupMessage)
    {
        return new Conversation(id, userId, sendTo, messageContent, sentAt, status, isGroupMessage);
    }

    public static Conversation Create(string conversationId, string senderId, string receiverId, bool isGroupMessage)
    {
        return Create(conversationId, senderId, receiverId, string.Empty, default, string.Empty, isGroupMessage);
    }

    public bool SeenChat(string userId)
    {
        if (UserId != userId)
        {
            Occurrence = 0;
            return true;
        }
        return false;
    }

    public IResult AddNewMessage(string userId, string sendTo, string messageContent)
    {
        var messageCreateResult = 
            Message.Create(Id, userId, sendTo, messageContent, IsGroupMessage);

        var message = messageCreateResult.Value;

        if (messageCreateResult.IsFailure || message is null)
        {
            return messageCreateResult;
        }

        if (userId == UserId)
        {
            Occurrence++;
        }
        else
        {
            Occurrence = 1;
        }

        LatestMessageId = message.Id;
        UserId = message.UserId;
        SendTo = message.SendTo;
        Content = message.Content;
        SentAt = message.SentAt;
        Status = message.Status;

        AddMessage(message);

        return Result.Success();
    }
}