using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Chat.Framework.Extensions;

namespace Chat.Framework.ServiceInstaller;

public static class DependencyInjection
{
    public static IServiceCollection InstallServices(this IServiceCollection services, 
        IConfiguration configuration, Assembly assembly,
        params Assembly[] assemblies)
    {
        var assemblyList = assemblies.ToList();
        assemblyList.Add(assembly);

        return InstallServices(services, configuration, assemblyList);
    }

    public static IServiceCollection InstallServices(this IServiceCollection services, 
        IConfiguration configuration, List<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetExportedTypes())
            {
                if (type.CanInstantiate() == false || type.IsAssignableTo(typeof(IServiceInstaller)) == false)
                {
                    continue;
                }
                
                var installer = Activator.CreateInstance(type).SmartCast<IServiceInstaller>();
                
                installer?.Install(services, configuration);
            }
        }

        return services;
    }
}