using Chat.Contacts.Application.DTOs;
using Chat.Contacts.Application.Extensions;
using Chat.Contacts.Application.Queries;
using Chat.Contacts.Domain.Entities;
using Chat.Contacts.Domain.Repositories;
using Peacious.Framework.CQRS;
using Peacious.Framework.Identity;
using Peacious.Framework.Pagination;
using Peacious.Framework.Results;

namespace Chat.Contacts.Application.QueryHandlers;

public class ContactQueryHandler : IQueryHandler<ContactQuery, IPaginationResponse<ContactDto>>
{
    private readonly IContactRepository _contactRepository;
    private readonly IScopeIdentity _scopeIdentity;

    public ContactQueryHandler(IContactRepository contactRepository, IScopeIdentity scopeIdentity)
    {
        _contactRepository = contactRepository;
        _scopeIdentity = scopeIdentity;
    }

    public Task<IResult<IPaginationResponse<ContactDto>>> Handle(ContactQuery request, CancellationToken cancellationToken)
    {
        return HandleAsync(request);
    }

    public async Task<IResult<IPaginationResponse<ContactDto>>> HandleAsync(ContactQuery query)
    {
        var response = query.CreateResponse();

        var userId = _scopeIdentity.GetUserId()!;

        List<Contact> contacts;

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