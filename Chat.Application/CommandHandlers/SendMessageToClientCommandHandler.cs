using Chat.Application.Interfaces;
using Chat.Domain.Commands;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Chat.Framework.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<SendMessageToClientCommand, Response>), ServiceLifetime.Singleton)]
public class SendMessageToClientCommandHandler : IRequestHandler<SendMessageToClientCommand, Response>
{
    private readonly IChatHubService _chatHubService;
    private readonly IChatRepository _chatRepository;

    public SendMessageToClientCommandHandler(
        IChatHubService chatHubService,
        IChatRepository chatRepository)
    {
        _chatHubService = chatHubService;
        _chatRepository = chatRepository;
    }

    public async Task<Response> HandleAsync(SendMessageToClientCommand command)
    {
        var chatModel = command.ChatModel ?? await _chatRepository.GetByIdAsync(command.MessageId);
        
        if (chatModel == null)
        {
            throw new Exception("ChatModel not found");
        }

        await _chatHubService.SendAsync(chatModel.SendTo, chatModel);

        return (Response)command.CreateResponse();
    }
}