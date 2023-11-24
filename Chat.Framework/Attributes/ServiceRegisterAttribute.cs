using Microsoft.Extensions.DependencyInjection;

namespace Chat.Framework.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ServiceRegisterAttribute : Attribute
{
    public Type? ServiceType { get; set; }
    public ServiceLifetime ServiceLifetime { get; set; }

    public ServiceRegisterAttribute(ServiceLifetime serviceLifetime)
    {
        ServiceLifetime = serviceLifetime;
    }

    public ServiceRegisterAttribute(Type serviceType, ServiceLifetime serviceLifeTime)
    {
        ServiceType = serviceType;
        ServiceLifetime = serviceLifeTime;
    }
}