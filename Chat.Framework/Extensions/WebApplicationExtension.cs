using Chat.Framework.Interfaces;
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
            initialService.InitializeAsync().Wait();
        }
        
        return app;
    }
}