using Microsoft.Extensions.DependencyInjection;

namespace Chat.Framework.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ServiceRegisterAttribute : Attribute
{
    public string? Key { get; private set; }
    public Type? ServiceType { get; private set; }
    public ServiceLifetime ServiceLifetime { get; private set; }

    public ServiceRegisterAttribute(ServiceLifetime serviceLifetime)
    {
        ServiceLifetime = serviceLifetime;
    }

    public ServiceRegisterAttribute(Type serviceType, ServiceLifetime serviceLifeTime)
    {
        ServiceType = serviceType;
        ServiceLifetime = serviceLifeTime;
    }

    public ServiceRegisterAttribute(string key, ServiceLifetime serviceLifeTime)
    {
        Key = key;
        ServiceLifetime = serviceLifeTime;
    }

    public ServiceRegisterAttribute(string key, Type serviceType, ServiceLifetime serviceLifeTime)
    {
        Key = key;
        ServiceType = serviceType;
        ServiceLifetime = serviceLifeTime;
    }
}