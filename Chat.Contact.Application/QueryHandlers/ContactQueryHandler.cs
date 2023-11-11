using Chat.Contact.Application.Extensions;
using Chat.Contact.Domain.Interfaces;
using Chat.Contact.Domain.Models;
using Chat.Contact.Domain.Queries;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Contact.Application.QueryHandlers;

[ServiceRegister(typeof(IHandler<ContactQuery, IQueryResponse>), ServiceLifetime.Singleton)]
public class ContactQueryHandler : IHandler<ContactQuery, IQueryResponse>
{
    private readonly IContactRepository _contactRepository;

    public ContactQueryHandler(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<IQueryResponse> HandleAsync(ContactQuery query)
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