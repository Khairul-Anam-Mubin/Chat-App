using Chat.Contacts.Domain.Entities;
using Peacious.Framework.ORM.Interfaces;

namespace Chat.Contacts.Domain.Repositories;

public interface IContactRepository : IRepository<Contact>
{
    Task<List<Contact>> GetUserContactsAsync(string userId);

    Task<List<Contact>> GetContactRequestsAsync(string userId);

    Task<List<Contact>> GetPendingContactsAsync(string userId);
}