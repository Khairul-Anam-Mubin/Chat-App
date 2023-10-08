using Chat.Activity.Domain.Models;

namespace Chat.Activity.Application.Interfaces.Repositories;

public interface ILastSeenRepository
{
    Task<bool> SaveLastSeenModelAsync(LastSeenModel lastSeenModel);
    Task<LastSeenModel?> GetLastSeenModelByUserIdAsync(string userId);
    Task<List<LastSeenModel>> GetLastSeenModelsByUserIdsAsync(List<string> userIds);
}