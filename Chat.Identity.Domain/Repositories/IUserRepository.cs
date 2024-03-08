using Chat.Framework.Database.ORM.Interfaces;
using Chat.Identity.Domain.Entities;

namespace Chat.Identity.Domain.Repositories;

public interface IUserRepository : IRepository<UserModel>
{
    Task<bool> IsUserExistAsync(string email);

    Task<bool> IsUserExistAsync(UserModel userModel);

    Task<UserModel?> GetUserByEmailAsync(string email);

    Task<List<UserModel>> GetUsersByUserIdOrEmailAsync(string userId, string email);

    Task<List<UserModel>> GetUsersByUserIdsAsync(List<string> userIds);

    Task<List<UserModel>> GetUsersByEmailsAsync(List<string> emails);

    Task<bool> UpdateEmailVerificationStatus(string userId, bool isVerified);
}