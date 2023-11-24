using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Notification.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Chat.Notification.Infrastructure.Services;

public class HubConnectionService : IHubConnectionService
{
    private readonly Dictionary<string, string> _connectionIdUserIdMapper;
    private readonly Dictionary<string, string> _userIdConnectionIdMapper;
    private readonly IRedisContext _redisContext;
    private readonly DatabaseInfo _databaseInfo;
    private readonly string _hubInstanceId;

    public HubConnectionService(IRedisContext redisContext, IConfiguration configuration)
    {
        _hubInstanceId = Guid.NewGuid().ToString();
        _redisContext = redisContext;
        _databaseInfo = configuration.GetSection("RedisConfig:DatabaseInfo").Get<DatabaseInfo>();
        _connectionIdUserIdMapper = new Dictionary<string, string>();
        _userIdConnectionIdMapper = new Dictionary<string, string>();
    }

    public async Task AddConnectionAsync(string connectionId, string userId)
    {
        if (_userIdConnectionIdMapper.TryGetValue(userId, out var prevConnectionId))
        {
            if (_connectionIdUserIdMapper.ContainsKey(prevConnectionId))
            {
                _connectionIdUserIdMapper.Remove(prevConnectionId);
            }
        }

        _userIdConnectionIdMapper[userId] = connectionId;
        _connectionIdUserIdMapper[connectionId] = userId;

        var channel = GetCurrentHubInstanceId();

        await _redisContext.SetDataAsync(_databaseInfo, GetPreparedKey(userId), channel);
    }

    public string GetConnectionId(string userId)
    {
        return _userIdConnectionIdMapper.TryGetValue(userId, out var value) ? value : string.Empty;
    }

    public string GetUserId(string connectionId)
    {
        return _connectionIdUserIdMapper.TryGetValue(connectionId, out var value) ? value : string.Empty;
    }

    public async Task RemoveConnectionAsync(string connectionId)
    {
        var userId = GetUserId(connectionId);

        if (!string.IsNullOrEmpty(userId))
        {
            _connectionIdUserIdMapper.Remove(userId);
            await _redisContext.DeleteDataAsync(_databaseInfo, GetPreparedKey(userId));
        }

        if (_connectionIdUserIdMapper.ContainsKey(connectionId))
        {
            _connectionIdUserIdMapper.Remove(connectionId);
        }
    }

    public bool IsUserConnectedWithCurrentHubInstance(string userId)
    {
        return _userIdConnectionIdMapper.ContainsKey(userId);
    }

    public async Task<bool> IsUserConnectedWithAnyHubInstanceAsync(string userId)
    {
        return await _redisContext.IsDataExistsAsync(_databaseInfo, GetPreparedKey(userId));
    }

    public string GetCurrentHubInstanceId()
    {
        return _hubInstanceId;
    }

    public async Task<string?> GetUserConnectedHubInstanceIdAsync(string userId)
    {
        if (IsUserConnectedWithCurrentHubInstance(userId))
        {
            return GetCurrentHubInstanceId();
        }

        return await _redisContext.GetDataAsync<string>(_databaseInfo, GetPreparedKey(userId));
    }

    private string GetPreparedKey(string key)
    {
        return $"HubData-{key}";
    }
}