using Chat.Contact.Domain.Models;

namespace Chat.Contact.Application.Interfaces;

public interface IContactRepository
{
    Task<bool> SaveContactAsync(ContactModel contactModel);
    Task<List<ContactModel>> GetUserContactsAsync(string userId);
    Task<List<ContactModel>> GetContactRequestsAsync(string userId);
    Task<List<ContactModel>> GetPendingContactsAsync(string userId);
    Task<ContactModel?> GetContactByIdAsync(string contactId);
    Task<bool> DeleteContactById(string contactId);
}