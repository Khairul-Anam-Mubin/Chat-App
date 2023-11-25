using Chat.Framework.Interfaces;
using Chat.Framework.ServiceInstaller;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Framework.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication StartInitialServices(this WebApplication app)
    {
        var initialServices = app.Services.GetServices<IInitialService>().ToList();

        Console.WriteLine($"Start Running Initial Services. Initial Services found: {initialServices.Count}\n");

        foreach (var initialService in initialServices)
        {
            try
            {
                initialService.InitializeAsync().Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        return app;
    }

    public static WebApplicationBuilder AddGlobalConfig(this WebApplicationBuilder builder, string globalConfigPath = "")
    {
        var configuration = builder.Configuration;
        
        var configPath = configuration["GlobalConfigPath"];

        if (!string.IsNullOrEmpty(globalConfigPath))
        {
            configPath = globalConfigPath;
        }

        if (string.IsNullOrEmpty(configPath))
        {
            throw new Exception("Config Path Not Found");
        }

        var configText = File.ReadAllText(configPath);

        if (string.IsNullOrEmpty(configText))
        {
            throw new Exception("File is empty");
        }

        var configDictionary = configText.Deserialize<Dictionary<string, object>>();

        if (configDictionary == null)
        {
            throw new Exception("ConfigDictionary is null");
        }

        GlobalConfig.Instance.AddConfigFromDictionary(configDictionary);

        return builder;
    }

    public static WebApplicationBuilder AddAllAssemblies(this WebApplicationBuilder builder, string assemblyPrefix = "")
    {
        if (string.IsNullOrEmpty(assemblyPrefix))
        {
            assemblyPrefix = builder.Configuration.TryGetConfig<string>("AssemblyPrefix");
        }
        
        AssemblyCache.Instance.AddAllAssemblies(assemblyPrefix);
        
        return builder;
    }

    public static WebApplicationBuilder InstallServices(this WebApplicationBuilder builder)
    {
        builder.Services.InstallServices(builder.Configuration, AssemblyCache.Instance.GetAddedAssemblies());
        return builder;
    }
}