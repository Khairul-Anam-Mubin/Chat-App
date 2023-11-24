using Chat.Application;
using Chat.Domain.Interfaces;
using Chat.Framework.ServiceInstaller;
using Chat.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Infrastructure.Configurations;

public class ChatInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddChatApplication();
        services.AddSingleton<IChatRepository, ChatRepository>();
        services.AddSingleton<ILatestChatRepository, LatestChatRepository>();
    }
}