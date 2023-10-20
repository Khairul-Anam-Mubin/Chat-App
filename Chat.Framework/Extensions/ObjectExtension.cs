using System.Text.Json;

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
            Console.WriteLine(ex.Message);
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
            Console.WriteLine(e);
        }

        try
        {
            return obj.ToString().Deserialize<T>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return default;
    }
}