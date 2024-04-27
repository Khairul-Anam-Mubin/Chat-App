using System.Collections.Concurrent;
using KCluster.Framework.Cache.DistributedCache;
using KCluster.Framework.Extensions;
using KCluster.Framework.ORM;
using Chat.Notification.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Chat.Notification.Infrastructure.Services;

public class HubConnectionService : IHubConnectionService
{
    private readonly string _hubInstanceId;
    private readonly ConcurrentDictionary<string, string> _connectionIdUserIdMapper;
    private readonly ConcurrentDictionary<string, HashSet<string>> _userIdConnectionIdsMapper;

    private readonly IDistributedCache _distributedCache;
    private readonly DatabaseInfo _databaseInfo;

    public HubConnectionService(IDistributedCache distributedCache, IConfiguration configuration)
    {
        _hubInstanceId = Guid.NewGuid().ToString();
        _connectionIdUserIdMapper = new ConcurrentDictionary<string, string>();
        _userIdConnectionIdsMapper = new ConcurrentDictionary<string, HashSet<string>>();
        _distributedCache = distributedCache;
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

        var hubIds = await _distributedCache.GetByKeyAsync<List<string>>(GetSetKey(userId));

        if (hubIds is null)
        {
            hubIds = new List<string>();
        }

        hubIds.Add(GetCurrentHubId());

        await _distributedCache.SetByKeyAsync(GetSetKey(userId), hubIds);
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

                var hubIds = await _distributedCache.GetByKeyAsync<List<string>>(GetSetKey(userId));

                if (hubIds is null)
                {
                    hubIds = new List<string>();
                }

                hubIds.Remove(GetCurrentHubId());

                await _distributedCache.SetByKeyAsync(GetSetKey(userId), hubIds);
     
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
        var hubIds = await _distributedCache.GetByKeyAsync<List<string>>(GetSetKey(userId));

        if (hubIds is null)
        {
            hubIds = new List<string>();
        }

        return hubIds;
    }

    private string GetSetKey(string key)
    {
        return $"Set-{key}";
    }
}