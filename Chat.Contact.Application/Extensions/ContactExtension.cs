using Chat.Contacts.Application.DTOs;
using Chat.Contacts.Domain.Entities;

namespace Chat.Contacts.Application.Extensions;

public static class ContactExtension
{
    public static ContactDto ToContactDto(this Contact contact, string userId)
    {
        return new ContactDto
        {
            ContactId = contact.Id,
            ContactUserId = contact.UserId == userId ? contact.ContactUserId : contact.UserId,
        };
    }
}