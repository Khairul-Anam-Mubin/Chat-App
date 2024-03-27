using Chat.Framework.Interfaces;
using Chat.Framework.ORM.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Framework.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication DoCreateIndexes(this WebApplication application, bool doCreateIndexes = false)
    {
        if (doCreateIndexes == false)
        {
            doCreateIndexes = application.Configuration.GetConfig<bool>("EnableIndexCreation");
        }

        if (doCreateIndexes)
        {
            var indexCreators = application.Services.GetServices<IIndexCreator>().ToList();

            Console.WriteLine($"Index Creation Started. IndexCreator Found : {indexCreators.Count}");

            indexCreators.ForEach(indexCreator =>
            {
                indexCreator.CreateIndexes();
            });
        }
        return application;
    }

    public static WebApplication DoMigration(this WebApplication application, bool doMigration = false)
    {
        if (doMigration == false)
        {
            doMigration = application.Configuration.GetConfig<bool>("EnableMigration");
        }

        if (doMigration)
        {
            var enabledMigrationJobs = application.Configuration.GetConfig<List<string>>("MigrationJobs");

            if (enabledMigrationJobs is null)
            {
                return application;
            }

            var migrationJobs = application.Services.GetServices<IMigrationJob>()
                .Where(job => enabledMigrationJobs.Contains(job.GetType().Name))
                .ToList();
            
            migrationJobs.ForEach(job =>
            {
                job.MigrateAsync();
            });
        }

        return application;
    }

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
}