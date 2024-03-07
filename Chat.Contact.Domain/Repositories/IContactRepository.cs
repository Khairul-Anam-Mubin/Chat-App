using Chat.Contact.Domain.Models;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Contact.Domain.Repositories;

public interface IContactRepository : IRepository<ContactModel>
{
    Task<List<ContactModel>> GetUserContactsAsync(string userId);

    Task<List<ContactModel>> GetContactRequestsAsync(string userId);

    Task<List<ContactModel>> GetPendingContactsAsync(string userId);
}