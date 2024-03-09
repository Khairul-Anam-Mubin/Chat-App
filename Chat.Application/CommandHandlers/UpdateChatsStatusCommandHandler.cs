using Chat.Application.Commands;
using Chat.Domain.Repositories;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.Results;

namespace Chat.Application.CommandHandlers;

public class UpdateChatsStatusCommandHandler : ICommandHandler<UpdateChatsStatusCommand>
{
    private readonly ILatestChatRepository _latestChatRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IScopeIdentity _scopeIdentity;

    public UpdateChatsStatusCommandHandler(IChatRepository chatRepository, ILatestChatRepository latestChatRepository, IScopeIdentity scopeIdentity)
    {
        _latestChatRepository = latestChatRepository;
        _chatRepository = chatRepository;
        _scopeIdentity = scopeIdentity;
    }

    public async Task<IResult> HandleAsync(UpdateChatsStatusCommand command)
    {
        var userId = _scopeIdentity.GetUserId()!;

        var latestChatModel = 
            await _latestChatRepository.GetLatestChatAsync(userId, command.OpenedChatUserId);
        
        if (latestChatModel is null)
        {
            return Result.Error("LatestChatModel not found");
        }

        if (latestChatModel.UserId != userId)
        {
            latestChatModel.Occurrence = 0;
            await _latestChatRepository.SaveAsync(latestChatModel);
        }
            
        var chatModels = 
            await _chatRepository.GetSenderAndReceiverSpecificChatModelsAsync(command.OpenedChatUserId, userId);
        
        foreach (var chatModel in chatModels)
        {
            chatModel.MessageSeen();
        }

        await _chatRepository.SaveAsync(chatModels);

        return Result.Success();
    }
}