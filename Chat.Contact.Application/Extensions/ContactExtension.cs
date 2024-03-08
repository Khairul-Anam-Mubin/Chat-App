using Chat.Contact.Application.DTOs;
using Chat.Contact.Domain.Entities;

namespace Chat.Contact.Application.Extensions;

public static class ContactExtension
{
    public static ContactDto ToContactDto(this ContactModel contactModel, string userId)
    {
        return new ContactDto
        {
            ContactId = contactModel.Id,
            ContactUserId = contactModel.UserId == userId ? contactModel.ContactUserId : contactModel.UserId,
        };
    }
}