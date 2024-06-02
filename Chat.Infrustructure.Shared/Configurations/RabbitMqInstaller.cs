using Peacious.Framework.MessageBrokers;
using Peacious.Framework.ServiceInstaller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Peacious.Framework.Extensions;
using Peacious.Framework;

namespace Chat.Infrastructure.Shared.Configurations;

public class RabbitMqInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        //services.AddRabbitMqMassTransit(
        //    configuration.TryGetConfig<MessageBrokerConfig>("MessageBrokerConfig"), 
        //    AssemblyCache.Instance.GetAddedAssemblies());
    }
}