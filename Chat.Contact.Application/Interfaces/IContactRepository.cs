namespace Chat.Contact.Application.Interfaces
{
    public interface IContactRepository
    {
        Task<bool> SaveContactAsync(Domain.Models.Contact contact);
        Task<List<Domain.Models.Contact>> GetUserContactsAsync(string userId);
        Task<List<Domain.Models.Contact>> GetContactRequestsAsync(string userId);
        Task<List<Domain.Models.Contact>> GetPendingContactsAsync(string userId);
        Task<Domain.Models.Contact?> GetContactByIdAsync(string contactId);
        Task<bool> DeleteContactById(string contactId);
    }
}