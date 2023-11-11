using Chat.Contact.Application.DTOs;
using Chat.Contact.Application.Extensions;
using Chat.Contact.Application.Queries;
using Chat.Contact.Domain.Interfaces;
using Chat.Contact.Domain.Models;
using Chat.Framework.Attributes;
using Chat.Framework.Mediators;
using Chat.Framework.RequestResponse;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Contact.Application.QueryHandlers;

[ServiceRegister(typeof(IHandler<ContactQuery, IPaginationResponse<ContactDto>>), ServiceLifetime.Singleton)]
public class ContactQueryHandler : IHandler<ContactQuery, IPaginationResponse<ContactDto>>
{
    private readonly IContactRepository _contactRepository;

    public ContactQueryHandler(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<IPaginationResponse<ContactDto>> HandleAsync(ContactQuery query)
    {
        var response = query.CreateResponse();

        List<ContactModel> contacts;

        if (query.IsRequestContacts)
        {
            contacts = await _contactRepository.GetContactRequestsAsync(query.UserId);
        }
        else if (query.IsPendingContacts)
        {
            contacts = await _contactRepository.GetPendingContactsAsync(query.UserId);
        }
        else
        {
            contacts = await _contactRepository.GetUserContactsAsync(query.UserId);
        }

        foreach (var contact in contacts)
        {
            response.AddItem(contact.ToContactDto(query.UserId));
        }

        return response;
    }
}