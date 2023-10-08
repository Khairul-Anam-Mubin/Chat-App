using Chat.Application.Interfaces;
using Chat.Application.Shared.Providers;
using Chat.Framework.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Persistence.Repositories;

[ServiceRegister(typeof(IHubConnectionService), ServiceLifetime.Singleton)]
public class HubConnectionService : IHubConnectionService
{
    private readonly Dictionary<string, string> _connectionIdUserIdMapper;
    private readonly Dictionary<string, string> _userIdConnectionIdMapper;

    public HubConnectionService()
    {
        _connectionIdUserIdMapper = new Dictionary<string, string>();
        _userIdConnectionIdMapper = new Dictionary<string, string>();
    }

    public void AddConnection(string connectionId, string accessToken)
    {
        var userProfile = IdentityProvider.GetUserProfile(accessToken);
        var userId = userProfile.Id;
        if (_userIdConnectionIdMapper.TryGetValue(userId, out var prevConnectionId))
        {
            if (_connectionIdUserIdMapper.ContainsKey(prevConnectionId))
            {
                _connectionIdUserIdMapper.Remove(prevConnectionId);
            }
        }
        _userIdConnectionIdMapper[userId] = connectionId;
        _connectionIdUserIdMapper[connectionId] = userId;
    }

    public string GetConnectionId(string userId)
    {
        return _userIdConnectionIdMapper.TryGetValue(userId, out var value) ? value : string.Empty;
    }

    public string GetUserId(string connectionId)
    {
        return _connectionIdUserIdMapper.TryGetValue(connectionId, out var value) ? value : string.Empty;
    }

    public void RemoveConnection(string connectionId)
    {
        var userId = GetUserId(connectionId);
        if (!string.IsNullOrEmpty(userId))
        {
            _connectionIdUserIdMapper.Remove(userId);
        }
        if (_connectionIdUserIdMapper.ContainsKey(connectionId))
        {
            _connectionIdUserIdMapper.Remove(connectionId);
        }
    }

    public bool IsUserConnectedWithHub(string userId)
    {
        return _userIdConnectionIdMapper.ContainsKey(userId);
    }
}