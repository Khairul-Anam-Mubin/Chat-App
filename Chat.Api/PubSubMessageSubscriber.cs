using Chat.Domain.Commands;
using Chat.Framework.Attributes;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Extensions;
using Chat.Framework.Interfaces;
using Chat.Framework.Proxy;

namespace Chat.Api;

[ServiceRegister(typeof(IInitialService), ServiceLifetime.Singleton)]
public sealed class PubSubMessageSubscriber : IInitialService
{
    private readonly IRedisContext _redisContext;
    private readonly IConfiguration _configuration;
    private readonly ICommandQueryProxy _commandQueryProxy;

    public PubSubMessageSubscriber(
        IRedisContext redisContext, 
        IConfiguration configuration, 
        ICommandQueryProxy commandQueryProxy)
    {
        _redisContext = redisContext;
        _configuration = configuration;
        _commandQueryProxy = commandQueryProxy;
    }

    public async Task InitializeAsync()
    {
        var subscriber = _redisContext.GetSubscriber(
            _configuration.GetSection("RedisConfig:DatabaseInfo").Get<DatabaseInfo>());
            
        var channel = _configuration.GetSection("RedisConfig:GlobalChannel").Get<string>();
            
        await subscriber.SubscribeAsync(channel, (redisChannel, message) =>
        {
            Console.WriteLine($"Message received form channel : {redisChannel}\n");
            var pubSubMessage = message.SmartCast<PubSubMessage>();

            if (pubSubMessage == null) return ;

            Console.WriteLine($"PubSubMessage.Id : {pubSubMessage?.Id}, PubSubMessageType: {pubSubMessage?.MessageType.ToString()} , message : {message}\n");
            
            if (pubSubMessage?.MessageType == MessageType.UserMessage)
            {
                var sendMessageToClientCommand = new SendMessageToClientCommand
                {
                    MessageId = pubSubMessage.Id
                };
                _commandQueryProxy.GetCommandResponseAsync(sendMessageToClientCommand).Wait();
            }

        });
    }
}