using Chat.Api.ContactModule.Models;

namespace Chat.Api.ContactModule.Extensions;

public static class ContactExtension
{
    public static ContactDto ToContactDto(this Contact contact, string userId)
    {
        return new ContactDto
        {
            ContactId = contact.Id,
            ContactUserId = contact.UserId == userId? contact.ContactUserId : contact.UserId,
        };
    }
}