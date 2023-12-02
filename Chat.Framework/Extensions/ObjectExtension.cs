using System.Text.Json;
using StackExchange.Redis;

namespace Chat.Framework.Extensions;

public static class ObjectExtension
{
    public static string Serialize(this object? obj)
    {
        if (obj is null)
        {
            return string.Empty;
        }

        try
        {
            return JsonSerializer.Serialize(obj);
        }
        catch (Exception ex)
        {
            // ignored
        }

        return string.Empty;
    }

    public static T? SmartCast<T>(this object? obj)
    {
        if (obj is null)
        {
            return default;
        }

        try
        {
            return (T) obj;
        }
        catch (Exception e)
        {
            // ignored
        }

        if (obj is RedisValue ob)
        {
            try
            {
                return obj.ToString().Deserialize<T>();
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        try
        {
            return obj.Serialize().Deserialize<T>();
        }
        catch (Exception e)
        {
            // ignored
        }

        return default;
    }
}