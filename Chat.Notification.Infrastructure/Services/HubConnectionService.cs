using System.Collections.Concurrent;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.ORM;
using Chat.Framework.Extensions;
using Chat.Notification.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Chat.Notification.Infrastructure.Services;

public class HubConnectionService : IHubConnectionService
{
    private readonly string _hubInstanceId;
    private readonly ConcurrentDictionary<string, string> _connectionIdUserIdMapper;
    private readonly ConcurrentDictionary<string, HashSet<string>> _userIdConnectionIdsMapper;

    private readonly IRedisContext _redisContext;
    private readonly DatabaseInfo _databaseInfo;

    public HubConnectionService(IRedisContext redisContext, IConfiguration configuration)
    {
        _hubInstanceId = Guid.NewGuid().ToString();
        _connectionIdUserIdMapper = new ConcurrentDictionary<string, string>();
        _userIdConnectionIdsMapper = new ConcurrentDictionary<string, HashSet<string>>();
        _redisContext = redisContext;
        _databaseInfo = configuration.TryGetConfig<DatabaseInfo>("RedisConfig");
    }

    public async Task AddConnectionToHubAsync(string connectionId, string userId)
    {
        if (_userIdConnectionIdsMapper.TryGetValue(userId, out var connectionIds))
        {
            connectionIds.Add(connectionId);
        }
        else
        {
            _userIdConnectionIdsMapper.TryAdd(userId, new HashSet<string>{connectionId});
        }

        _connectionIdUserIdMapper[connectionId] = userId;

        await _redisContext.GetDatabase(_databaseInfo).SetAddAsync(GetSetKey(userId), GetCurrentHubId());
    }

    public List<string> GetConnectionIds(string userId)
    {
        return _userIdConnectionIdsMapper.TryGetValue(userId, out var connectionIds) ? 
            connectionIds.ToList() : new List<string>();
    }

    public string GetUserId(string connectionId)
    {
        return _connectionIdUserIdMapper.TryGetValue(connectionId, out var value) ? 
            value : string.Empty;
    }

    public async Task RemoveConnectionFromHubAsync(string connectionId)
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

            if (connectionIds is not null && connectionIds.Count == 0)
            {
                await _redisContext.GetDatabase(_databaseInfo).SetRemoveAsync(GetSetKey(userId), GetCurrentHubId());
            }
        }

        if (_connectionIdUserIdMapper.ContainsKey(connectionId))
        {
            _connectionIdUserIdMapper.Remove(connectionId, out var value);
        }
    }

    public string GetCurrentHubId()
    {
        return _hubInstanceId;
    }

    public async Task<List<string>> GetUserConnectedHubIdsAsync(string userId)
    {
        var redisDb = _redisContext.GetDatabase(_databaseInfo);
        var hubIds = new List<string>();
        var redisHubIds = await redisDb.SetMembersAsync(GetSetKey(userId));
        foreach (var hubId in redisHubIds)
        {
            hubIds.Add(hubId.ToString());
        }
        return hubIds;
    }

    private string GetSetKey(string key)
    {
        return $"Set-{key}";
    }
}