using Chat.Framework;
using Chat.Framework.MessageBrokers;
using Chat.Framework.ServiceInstaller;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Infrastructure.Shared.Configurations;

public class RabbitMqInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration.GetSection("MessageBrokerConfig").Get<MessageBrokerConfig>());

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

        services.AddTransient<IEventBus, EventBus>();
        services.AddTransient<ICommandBus, CommandBus>();
        services.AddTransient<IMessageRequestClient, MessageRequestClient>();
    }
}