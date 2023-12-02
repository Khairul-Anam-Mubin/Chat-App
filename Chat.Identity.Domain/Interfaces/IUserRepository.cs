using Chat.Framework.Database.ORM.Interfaces;
using Chat.Identity.Domain.Models;

namespace Chat.Identity.Domain.Interfaces;

public interface IUserRepository : IRepository<UserModel>
{
    Task<bool> IsUserExistAsync(UserModel userModel);

    Task<UserModel?> GetUserByEmailAsync(string email);

    Task<List<UserModel>> GetUsersByUserIdOrEmailAsync(string userId, string email);

    Task<List<UserModel>> GetUsersByUserIdsAsync(List<string> userIds);

    Task<List<UserModel>> GetUsersByEmailsAsync(List<string> emails);
}