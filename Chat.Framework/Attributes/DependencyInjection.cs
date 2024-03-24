using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Chat.Framework.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Chat.Framework.Attributes;

public static class DependencyInjection
{
    public static IServiceCollection AddAttributeRegisteredServices(this IServiceCollection services, List<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var exportedTypes = assembly.GetExportedTypes();

            foreach (var type in exportedTypes)
            {
                if (!type.CanInstantiate()) continue;

                var serviceRegisterAttributes = type.GetCustomAttributes<ServiceRegisterAttribute>().ToList();

                if (serviceRegisterAttributes.Any() == false) continue;

                foreach (var serviceRegisterAttribute in serviceRegisterAttributes)
                {
                    if (serviceRegisterAttribute.ServiceType is null)
                    { 
                        if (string.IsNullOrEmpty(serviceRegisterAttribute.Key))
                        {
                            services.TryAdd(new ServiceDescriptor(type, type, serviceRegisterAttribute.ServiceLifetime));
                        }
                        else
                        {
                            services.TryAdd(new ServiceDescriptor(type, serviceRegisterAttribute.Key, type, serviceRegisterAttribute.ServiceLifetime));
                        }
                        
                        continue;
                    }

                    if (!type.IsAssignableTo(serviceRegisterAttribute.ServiceType))
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(serviceRegisterAttribute.Key))
                    {
                        services.TryAdd(new ServiceDescriptor(serviceRegisterAttribute.ServiceType, type,
                        serviceRegisterAttribute.ServiceLifetime));
                    }
                    else
                    {
                        services.TryAdd(new ServiceDescriptor(type, serviceRegisterAttribute.Key, type, serviceRegisterAttribute.ServiceLifetime));
                    }
                }
                
            }
        }
        return services;
    }

    public static IServiceCollection AddAttributeRegisteredServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        var assemblyList = assemblies.ToList();

        return AddAttributeRegisteredServices(services, assemblyList);
    }
}