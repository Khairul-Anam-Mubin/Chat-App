using KCluster.Framework.MessageBrokers;
using KCluster.Framework.ServiceInstaller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Infrastructure.Shared.Configurations;

public class RabbitMqInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddRabbitMqMassTransit(configuration);
    }
}