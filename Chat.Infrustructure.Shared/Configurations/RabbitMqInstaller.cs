using KCluster.Framework;
using KCluster.Framework.Extensions;
using KCluster.Framework.MessageBrokers;
using KCluster.Framework.ServiceInstaller;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Infrastructure.Shared.Configurations;

public class RabbitMqInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration.TryGetConfig<MessageBrokerConfig>());
        services.AddTransient<IEventBus, EventBus>();
        services.AddTransient<ICommandBus, CommandBus>();
        services.AddTransient<IMessageRequestClient, MessageRequestClient>();

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetDefaultEndpointNameFormatter();

            AssemblyCache.Instance.GetAddedAssemblies().ForEach(assembly =>
                busConfigurator.AddConsumers(assembly));

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                var messageBrokerConfig = context.GetRequiredService<MessageBrokerConfig>();

                configurator.Host(new Uri(messageBrokerConfig.Host), hostConfigurator =>
                {
                    hostConfigurator.Username(messageBrokerConfig.UserName);
                    hostConfigurator.Password(messageBrokerConfig.Password);
                });

                configurator.AutoDelete = true;

                configurator.ConfigureEndpoints(context);
            });
        });
    }
}