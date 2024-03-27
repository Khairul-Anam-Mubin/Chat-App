using Chat.Contacts.Domain.Results;
using Chat.Framework.DDD;
using Chat.Framework.ORM.Interfaces;
using Chat.Framework.Results;

namespace Chat.Contacts.Domain.Entities;

public class Contact : Entity, IRepositoryItem
{
    public string UserId { get; private set; }
    public string ContactUserId { get; private set; }
    public bool IsPending { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Contact(string userId, string contactUserId)
        : base(Guid.NewGuid().ToString())
    {
        UserId = userId;
        ContactUserId = contactUserId;
        IsPending = true;
        CreatedAt = DateTime.UtcNow;
    }

    public static Contact Create(string userId, string contactUserId)
    {
        return new Contact(userId, contactUserId);
    }

    public IResult AcceptRequest(string? contactUserId)
    {
        if (string.IsNullOrEmpty(contactUserId))
        {
            return Result.Error().ContactUserIdEmpty();
        }

        if (contactUserId != ContactUserId)
        {
            return Result.Error().InvalidContactUser();
        }

        IsPending = false;

        return Result.Success().ContactAccepted();
    }

    public IResult RejectRequest(string? contactUserId)
    {
        if (string.IsNullOrEmpty(contactUserId))
        {
            return Result.Error().ContactUserIdEmpty();
        }

        if (contactUserId != ContactUserId)
        {
            return Result.Error().InvalidContactUser();
        }

        return Result.Success().ContactRejected();
    }
}