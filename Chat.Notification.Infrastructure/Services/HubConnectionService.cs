using System.Collections.Concurrent;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.ORM;
using Chat.Framework.Extensions;
using Chat.Notification.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Chat.Notification.Infrastructure.Services;

public class HubConnectionService : IHubConnectionService
{
    private readonly Dictionary<string, string> _connectionIdUserIdMapper;
    private readonly Dictionary<string, string> _userIdConnectionIdMapper;
    private readonly ConcurrentDictionary<string, HashSet<string>> _userIdConnectionIdsMapper;
    private readonly IRedisContext _redisContext;
    private readonly DatabaseInfo _databaseInfo;
    private readonly string _hubInstanceId;

    public HubConnectionService(IRedisContext redisContext, IConfiguration configuration)
    {
        _hubInstanceId = Guid.NewGuid().ToString();
        _redisContext = redisContext;
        _databaseInfo = configuration.GetConfig<DatabaseInfo>("RedisConfig")!;
        _connectionIdUserIdMapper = new Dictionary<string, string>();
        _userIdConnectionIdMapper = new Dictionary<string, string>();
        _userIdConnectionIdsMapper = new ConcurrentDictionary<string, HashSet<string>>();
    }

    public async Task AddConnectionAsync(string connectionId, string userId)
    {
        if (_userIdConnectionIdsMapper.TryGetValue(userId, out var connectionIds))
        {
            connectionIds.Add(connectionId);
        }
        else
        {
            _userIdConnectionIdsMapper.TryAdd(userId, new HashSet<string>{connectionId});
        }

        if (_userIdConnectionIdMapper.TryGetValue(userId, out var prevConnectionId))
        {
            if (_connectionIdUserIdMapper.ContainsKey(prevConnectionId))
            {
                _connectionIdUserIdMapper.Remove(prevConnectionId);
            }
        }

        _userIdConnectionIdMapper[userId] = connectionId;
        _connectionIdUserIdMapper[connectionId] = userId;

        var channel = GetCurrentHubId();

        await _redisContext.SetDataAsync(_databaseInfo, GetPreparedKey(userId), channel);
    }

    public string GetConnectionId(string userId)
    {
        return _userIdConnectionIdMapper.TryGetValue(userId, out var value) ? value : string.Empty;
    }

    public List<string> GetConnectionIds(string userId)
    {
        if (_userIdConnectionIdsMapper.TryGetValue(userId, out var connectionIds))
        {
            return connectionIds.ToList();
        }

        return new List<string>();
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
            if (_userIdConnectionIdsMapper.TryGetValue(userId, out var connectionIds))
            {
                if (connectionIds.Contains(connectionId))
                {
                    connectionIds.Remove(connectionId);
                }
            }
        }
        
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

    public string GetCurrentHubId()
    {
        return _hubInstanceId;
    }

    public async Task<string?> GetUserConnectedHubInstanceIdAsync(string userId)
    {
        if (IsUserConnectedWithCurrentHubInstance(userId))
        {
            return GetCurrentHubId();
        }

        return await _redisContext.GetDataAsync<string>(_databaseInfo, GetPreparedKey(userId));
    }

    public async Task<List<string>> GetUserConnectedHubIdsAsync(string userId)
    {
        await Task.CompletedTask;
        return new List<string>();
    }

    private string GetPreparedKey(string key)
    {
        return $"HubData-{key}";
    }
}