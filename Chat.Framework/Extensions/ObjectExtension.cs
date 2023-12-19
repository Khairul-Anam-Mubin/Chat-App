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

    public static List<T> SmartCastToList<T>(this object? obj)
    {
        if (obj is IEnumerable<T> objects)
        {
            return objects.ToList();
        }
        return new List<T>();
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

    public static Dictionary<string, object?> ToDictionary(this object? obj)
    {
        var propertyValueDictionary = new Dictionary<string, object?>();

        if (obj is null)
        {
            return propertyValueDictionary;
        }

        foreach (var prop in obj.GetType().GetProperties())
        {
            propertyValueDictionary.TryAdd(prop.Name, prop.GetValue(obj));
        }

        return propertyValueDictionary;
    }
}