using Chat.Application.Extensions;
using Chat.Application.Interfaces;
using Chat.Domain.Commands;
using Chat.Domain.Events;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Chat.Framework.Proxy;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<SendMessageCommand, CommandResponse>), ServiceLifetime.Singleton)]
public class SendMessageCommandHandler : ACommandHandler<SendMessageCommand>
{
    private readonly IChatRepository _chatRepository;
    private readonly ICommandQueryProxy _commandQueryProxy;
    private readonly IHubConnectionService _hubConnectionService;

    public SendMessageCommandHandler(
        IChatRepository chatRepository,
        ICommandQueryProxy commandQueryProxy, 
        IHubConnectionService hubConnectionService)
    {
        _chatRepository = chatRepository;
        _commandQueryProxy = commandQueryProxy;
        _hubConnectionService = hubConnectionService;
    }

    protected override async Task<CommandResponse> OnHandleAsync(SendMessageCommand command)
    { 
        var chatModel = command.ChatModel;

        if (chatModel is null)
        {
            throw new Exception("ChatModel not set");
        }

        chatModel.Id = Guid.NewGuid().ToString();
        chatModel.SentAt = DateTime.UtcNow;
        chatModel.Status = "Sent";

        if (!await _chatRepository.SaveAsync(chatModel))
        {
            throw new Exception("Chat model save error");
        }

        if (_hubConnectionService.IsUserConnectedWithHub(chatModel.SendTo))
        {
            var sendMessageToClientCommand = new SendMessageToClientCommand
            {
                ChatModel = chatModel,
                FireAndForget = true
            };

            await _commandQueryProxy.GetCommandResponseAsync(sendMessageToClientCommand);
        }
        else
        {
            var publishMessageToConnectedServerEvent = new PublishMessageToConnectedServerEvent
            {
                UserId = chatModel.UserId,
                SendTo = chatModel.SendTo,
                MessageId = chatModel.Id,
                ChatModel = chatModel
            };

            await _commandQueryProxy.SendAsync(publishMessageToConnectedServerEvent);
        }

        var latestChatModel = chatModel.ToLatestChatModel();

        var updateLatestChatCommand = new UpdateLatestChatCommand
        {
            LatestChatModel = latestChatModel,
            FireAndForget = true
        };

        await _commandQueryProxy.GetCommandResponseAsync(updateLatestChatCommand);
        
        var response = command.CreateResponse();
        
        response.SetData("Message", chatModel.ToChatDto());

        return response;
    }
}