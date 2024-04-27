using KCluster.Framework.DDD;
using KCluster.Framework.ORM.Interfaces;
using KCluster.Framework.Results;
using KCluster.Framework.Security;

namespace Chat.Domain.Entities;

public class Conversation : AggregateRoot, IRepositoryItem
{
    public string SenderId { get; private set; }
    public string ReceiverId { get; private set; }
    public string LatestMessageId { get; private set; } 
    public string Content { get; private set; }
    public DateTime SentAt { get; private set; }
    public string Status { get; private set; }
    public bool IsGroupConversation { get; private set; }
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

    private Conversation(string id, string senderId, string receiverId, string messageContent, DateTime sentAt, string status, bool isGroupMessage)
        : base(id)
    {
        SenderId = senderId;
        ReceiverId = receiverId;
        Content = messageContent;
        SentAt = sentAt;
        Status = status;
        IsGroupConversation = isGroupMessage;
        Occurrence = 0;
        LatestMessageId = string.Empty;
    }

    public static string GetConversationId(string senderId, string receiverId)
    {
        return CheckSumGenerator.GetCheckSum(senderId, receiverId);
    }

    public static Conversation Create(string id, string senderId, string receiverId, string messageContent, DateTime sentAt, string status, bool isGroupMessage)
    {
        return new Conversation(id, senderId, receiverId, messageContent, sentAt, status, isGroupMessage);
    }

    public static Conversation Create(string conversationId, string senderId, string receiverId, bool isGroupMessage)
    {
        return Create(conversationId, senderId, receiverId, string.Empty, default, string.Empty, isGroupMessage);
    }

    public bool SeenChat(string userId)
    {
        if (SenderId != userId)
        {
            Occurrence = 0;
            return true;
        }
        return false;
    }

    public IResult AddNewMessage(string senderId, string receiverId, string messageContent)
    {
        var messageCreateResult = 
            Message.Create(Id, senderId, receiverId, messageContent, IsGroupConversation);

        var message = messageCreateResult.Value;

        if (messageCreateResult.IsFailure || message is null)
        {
            return messageCreateResult;
        }

        if (senderId == SenderId)
        {
            Occurrence++;
        }
        else
        {
            Occurrence = 1;
        }

        LatestMessageId = message.Id;
        SenderId = message.SenderId;
        ReceiverId = message.ReceiverId;
        Content = message.Content;
        SentAt = message.SentAt;
        Status = message.Status;

        AddMessage(message);

        return Result.Success();
    }
}