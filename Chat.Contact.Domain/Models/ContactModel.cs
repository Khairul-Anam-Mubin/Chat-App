using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Results;

namespace Chat.Contact.Domain.Models;

public class ContactModel : IEntity
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string ContactUserId { get; set; }
    public bool IsPending { get; set; }
    public DateTime CreatedAt { get; set; }

    private ContactModel(string userId, string contactUserId)
    {
        Id = Guid.NewGuid().ToString();
        UserId = userId;
        ContactUserId = contactUserId;
        IsPending = true;
        CreatedAt = DateTime.UtcNow;
    }

    public IResult AcceptRequest(string? contactUserId)
    {
        if (string.IsNullOrEmpty(contactUserId))
        {
            return Result.Error("Contact User Id can't be empty");
        }

        if (contactUserId != ContactUserId)
        {
            return Result.Error("Only contact user can accept request.");
        }
        
        IsPending = false;
        
        return Result.Success();
    }
    
    public static ContactModel Create(string userId, string contactUserId)
    {
        return new ContactModel(userId, contactUserId);
    }
}