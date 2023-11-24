namespace Chat.Framework.Extensions;

public static class TypeExtension
{
    public static bool CanInstantiate(this Type type)
    {
        return !(
            type.IsAbstract ||
            type.IsInterface ||
            type.IsClass == false);
    }
}