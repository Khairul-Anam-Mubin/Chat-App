using Chat.Identity.Domain.Entities;
using Peacious.Framework.ORM.Interfaces;

namespace Chat.Identity.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<bool> IsUserExistAsync(string email);

    Task<bool> IsUserExistAsync(User userModel);

    Task<User?> GetUserByEmailAsync(string email);

    Task<List<User>> GetUsersByUserIdOrEmailAsync(string userId, string email);

    Task<List<User>> GetUsersByUserIdsAsync(List<string> userIds);

    Task<List<User>> GetUsersByEmailsAsync(List<string> emails);

    Task<bool> UpdateEmailVerificationStatus(string userId, bool isVerified);
}