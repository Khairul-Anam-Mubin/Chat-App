using Chat.Identity.Domain.Entities;

namespace Chat.Identity.Domain.Services;

public interface IAccessService
{
    Task<List<string>> GetUserFlatPermissionsAsync(string userId);
    Task<List<string>> GetUserPermissionIdsAsync(string userId);
    Task<List<Role>> GetUserRolesAsync(string userId);
}