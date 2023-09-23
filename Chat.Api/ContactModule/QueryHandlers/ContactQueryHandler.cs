using Chat.Api.ContactModule.Extensions;
using Chat.Api.ContactModule.Interfaces;
using Chat.Api.ContactModule.Models;
using Chat.Api.ContactModule.Queries;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;

namespace Chat.Api.ContactModule.QueryHandlers
{
    [ServiceRegister(typeof(IRequestHandler<ContactQuery, QueryResponse>), ServiceLifetime.Singleton)]
    public class ContactQueryHandler : AQueryHandler<ContactQuery>
    {
        private readonly IContactRepository _contactRepository;
        public ContactQueryHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        protected override async Task<QueryResponse> OnHandleAsync(ContactQuery query)
        {
            var response = query.CreateResponse();

            List<Contact> contacts;
            
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
}