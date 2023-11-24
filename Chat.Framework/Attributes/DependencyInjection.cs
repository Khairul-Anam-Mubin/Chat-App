using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
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
                if (!type.IsClass || type.IsAbstract || type.IsInterface) continue;

                var serviceRegisterAttributes = type.GetCustomAttributes<ServiceRegisterAttribute>().ToList();

                if (serviceRegisterAttributes.Any() == false) continue;

                foreach (var serviceRegisterAttribute in serviceRegisterAttributes)
                {
                    if (serviceRegisterAttribute.ServiceType == null)
                    {
                        services.TryAdd(new ServiceDescriptor(type, type, serviceRegisterAttribute.ServiceLifetime));
                        continue;
                    }

                    if (!type.IsAssignableTo(serviceRegisterAttribute.ServiceType))
                    {
                        continue;
                    }

                    services.TryAdd(new ServiceDescriptor(serviceRegisterAttribute.ServiceType, type,
                        serviceRegisterAttribute.ServiceLifetime));
                }
                
            }
        }
        return services;
    }

    public static IServiceCollection AddAttributeRegisteredServices(this IServiceCollection services, Assembly assembly, params Assembly[] assemblies)
    {
        var assemblyList = assemblies.ToList();
        assemblyList.Add(assembly);

        return AddAttributeRegisteredServices(services, assemblyList);
    }
}