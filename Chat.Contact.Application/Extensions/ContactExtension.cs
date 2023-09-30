using Chat.Contact.Domain.Models;

namespace Chat.Contact.Application.Extensions;

public static class ContactExtension
{
    public static ContactDto ToContactDto(this Domain.Models.Contact contact, string userId)
    {
        return new ContactDto
        {
            ContactId = contact.Id,
            ContactUserId = contact.UserId == userId ? contact.ContactUserId : contact.UserId,
        };
    }
}