using Chat.Contact.Application.DTOs;
using Chat.Contact.Application.Extensions;
using Chat.Contact.Application.Queries;
using Chat.Contact.Domain.Interfaces;
using Chat.Contact.Domain.Models;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.Pagination;
using Chat.Framework.Results;

namespace Chat.Contact.Application.QueryHandlers;

public class ContactQueryHandler : IQueryHandler<ContactQuery, IPaginationResponse<ContactDto>>
{
    private readonly IContactRepository _contactRepository;
    private readonly IScopeIdentity _scopeIdentity;

    public ContactQueryHandler(IContactRepository contactRepository, IScopeIdentity scopeIdentity)
    {
        _contactRepository = contactRepository;
        _scopeIdentity = scopeIdentity;
    }

    public async Task<IResult<IPaginationResponse<ContactDto>>> HandleAsync(ContactQuery query)
    {
        var response = query.CreateResponse();

        var userId = _scopeIdentity.GetUserId()!;

        List<ContactModel> contacts;

        if (query.IsRequestContacts)
        {
            contacts = await _contactRepository.GetContactRequestsAsync(userId);
        }
        else if (query.IsPendingContacts)
        {
            contacts = await _contactRepository.GetPendingContactsAsync(userId);
        }
        else
        {
            contacts = await _contactRepository.GetUserContactsAsync(userId);
        }

        foreach (var contact in contacts)
        {
            response.AddItem(contact.ToContactDto(userId));
        }

        return Result.Success(response);
    }
}