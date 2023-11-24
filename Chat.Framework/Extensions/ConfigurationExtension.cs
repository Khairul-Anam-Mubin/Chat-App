using Microsoft.Extensions.Configuration;

namespace Chat.Framework.Extensions;

public static class ConfigurationExtension
{
    public static T? GetConfig<T>(this IConfiguration configuration, string key)
    {
        var config = configuration[key];

        return string.IsNullOrEmpty(config) ? default : config.Deserialize<T>();
    }

    public static T? GetConfig<T>(this IConfiguration configuration)
    {
        var key = typeof(T).Name;
        return GetConfig<T>(configuration, key);
    }

    public static T TryGetConfig<T>(this IConfiguration configuration)
    {
        var key = typeof(T).Name;
        return GetConfig<T>(configuration, key) ?? throw new Exception("Config null");
    }
    public static T TryGetConfig<T>(this IConfiguration configuration, string key)
    {
        return GetConfig<T>(configuration, key) ?? throw new Exception("Config null");
    }
}