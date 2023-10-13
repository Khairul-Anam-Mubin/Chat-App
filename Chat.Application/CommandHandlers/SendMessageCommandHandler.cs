using Chat.Application.Extensions;
using Chat.Application.Interfaces;
using Chat.Domain.Commands;
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
    private readonly IChatHubService _chatHubService;
    private readonly ICommandQueryProxy _commandQueryProxy;

    public SendMessageCommandHandler(IChatRepository chatRepository, IChatHubService chatHubService, ICommandQueryProxy commandQueryProxy)
    {
        _chatHubService = chatHubService;
        _chatRepository = chatRepository;
        _commandQueryProxy = commandQueryProxy;
    }

    protected override async Task<CommandResponse> OnHandleAsync(SendMessageCommand command)
    {
        var response = command.CreateResponse();

        if (command.ChatModel == null)
        {
            throw new Exception("ChatModel not set");
        }

        command.ChatModel.Id = Guid.NewGuid().ToString();
        command.ChatModel.SentAt = DateTime.UtcNow;
        command.ChatModel.Status = "Sent";

        if (!await _chatRepository.SaveAsync(command.ChatModel))
        {
            throw new Exception("Chat model save error");
        }
            
        await _chatHubService.SendAsync(command.ChatModel.SendTo, command.ChatModel);

        var latestChatModel = command.ChatModel.ToLatestChatModel();

        var updateLatestChatCommand = new UpdateLatestChatCommand
        {
            LatestChatModel = latestChatModel,
            FireAndForget = true
        };
        await _commandQueryProxy.GetCommandResponseAsync(updateLatestChatCommand);

        response.SetData("Message", command.ChatModel.ToChatDto());

        return response;
    }
}