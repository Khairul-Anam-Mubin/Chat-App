using Chat.Contact.Domain.Results;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Results;

namespace Chat.Contact.Domain.Entities;

public class ContactModel : IRepositoryItem
{
    public string Id { get; private set; }
    public string UserId { get; private set; }
    public string ContactUserId { get; private set; }
    public bool IsPending { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private ContactModel(string userId, string contactUserId)
    {
        Id = Guid.NewGuid().ToString();
        UserId = userId;
        ContactUserId = contactUserId;
        IsPending = true;
        CreatedAt = DateTime.UtcNow;
    }

    public static ContactModel Create(string userId, string contactUserId)
    {
        return new ContactModel(userId, contactUserId);
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