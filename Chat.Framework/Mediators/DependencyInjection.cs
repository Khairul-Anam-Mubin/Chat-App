using System.Reflection;
using Chat.Framework.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Chat.Framework.Mediators;

public static class DependencyInjection
{
    private static readonly List<Type> HandlerTypes = new() { typeof(IHandler<,>), typeof(IHandler<>) };

    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        return services.AddTransient<IMediator, Mediator>();
    }

    public static IServiceCollection AddHandler(this IServiceCollection services, Type handler, ServiceLifetime serviceLifeTime)
    {
        if(!handler.CanInstantiate())
        {
            throw new Exception($"{handler.Name} can't be instantiate");
        }

        var handlerInterfaces = handler.GetInterfaces().Where(x => 
            x.IsGenericType && HandlerTypes.Contains(x.GetGenericTypeDefinition())).ToList();

        if (handlerInterfaces.Any() == false) return services;

        foreach (var handlerInterface in handlerInterfaces)
        {
            services.TryAdd(new ServiceDescriptor(handlerInterface, handler, serviceLifeTime));
        }
        return services;
    }

    public static IServiceCollection AddHandler(this IServiceCollection services, Type handler)
    {
        return AddHandler(services, handler, ServiceLifetime.Transient);
    }

    public static IServiceCollection AddHandler<T>(this IServiceCollection services, ServiceLifetime serviceLifetime)
    {
        return AddHandler(services, typeof(T), serviceLifetime);
    }

    public static IServiceCollection AddHandler<T>(this IServiceCollection services)
    {
        return AddHandler(services, typeof(T));
    }

    public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly assembly, params Assembly[] assemblies)
    {
        var assemblyList = assemblies.ToList();
        
        assemblyList.Add(assembly);

        return AddHandlers(services, assemblyList);
    }

    public static IServiceCollection AddHandlers(this IServiceCollection services, List<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetExportedTypes())
            {
                if (!type.CanInstantiate()) continue;

                AddHandler(services, type);
            }
        }
        return services;
    }
}