using Chat.FileStore.Application;
using Chat.FileStore.Domain.Repositories;
using Chat.FileStore.Infrastructure.Repositories;
using Peacious.Framework.ServiceInstaller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.FileStore.Infrastructure.Configurations;

public class FileStoreInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddFileStoreApplication();
        services.AddScoped<IFileDirectoryRepository, FileDirectoryRepository>();
    }
}