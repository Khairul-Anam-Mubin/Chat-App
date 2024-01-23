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

        _connectionIdUserIdMapper[connectionId] = userId;

        await _redisContext.GetDatabase(_databaseInfo).SetAddAsync(GetSetKey(userId), GetCurrentHubId());
    }

    public List<string> GetConnectionIds(string userId)
    {
        return _userIdConnectionIdsMapper.TryGetValue(userId, out var connectionIds) ? 
            connectionIds.ToList() : 
            new List<string>();
    }

    public string GetUserId(string connectionId)
    {
        return _connectionIdUserIdMapper.TryGetValue(connectionId, out var value) ? 
            value : 
            string.Empty;
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

            if (connectionIds is not null && connectionIds.Count == 0)
            {
                await _redisContext.GetDatabase(_databaseInfo).SetRemoveAsync(GetSetKey(userId), GetCurrentHubId());
            }
        }

        if (_connectionIdUserIdMapper.ContainsKey(connectionId))
        {
            _connectionIdUserIdMapper.Remove(connectionId);
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

    private string GetPreparedKey(string key)
    {
        return $"HubSetData-{key}";
    }

    private string GetSetKey(string key)
    {
        return $"Set-{key}";
    }
}