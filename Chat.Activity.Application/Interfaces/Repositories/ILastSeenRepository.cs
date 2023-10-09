using Chat.Activity.Domain.Models;
using Chat.Framework.Database.Interfaces;

namespace Chat.Activity.Application.Interfaces.Repositories;

public interface ILastSeenRepository : IRepository<LastSeenModel>
{
    Task<LastSeenModel?> GetLastSeenModelByUserIdAsync(string userId);
    Task<List<LastSeenModel>> GetLastSeenModelsByUserIdsAsync(List<string> userIds);
}