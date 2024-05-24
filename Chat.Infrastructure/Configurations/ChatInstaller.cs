using Chat.Application;
using Chat.Domain.Repositories;
using Peacious.Framework.ORM.Interfaces;
using Peacious.Framework.ServiceInstaller;
using Chat.Infrastructure.Migrations;
using Chat.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Infrastructure.Configurations;

public class ChatInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddChatApplication();
        
        services.AddTransient<IIndexCreator, MessageIndexCreator>();
        services.AddTransient<IIndexCreator, ConversationIndexCreator>();
        
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IConversationRepository, ConversationRepository>();
    }
}