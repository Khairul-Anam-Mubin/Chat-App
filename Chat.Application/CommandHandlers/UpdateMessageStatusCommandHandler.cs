using Chat.Application.Commands;
using Chat.Domain.Repositories;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.Results;

namespace Chat.Application.CommandHandlers;

public class UpdateMessageStatusCommandHandler : ICommandHandler<UpdateMessageStatusCommand>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IScopeIdentity _scopeIdentity;

    public UpdateMessageStatusCommandHandler(IMessageRepository messageRepository, IConversationRepository conversationRepository, IScopeIdentity scopeIdentity)
    {
        _conversationRepository = conversationRepository;
        _messageRepository = messageRepository;
        _scopeIdentity = scopeIdentity;
    }

    public async Task<IResult> HandleAsync(UpdateMessageStatusCommand command)
    {
        var userId = _scopeIdentity.GetUserId()!;

        var conversation = 
            await _conversationRepository.GetConversationAsync(userId, command.OpenedChatUserId);
        
        if (conversation is null)
        {
            return Result.Error("Conversation not found");
        }

        if (conversation.SeenChat(userId))
        {
            await _conversationRepository.SaveAsync(conversation);
        }
            
        var messages = 
            await _messageRepository.GetSenderAndReceiverSpecificMessagesAsync(command.OpenedChatUserId, userId);
        
        foreach (var message in messages)
        {
            message.MessageSeen();
        }

        await _messageRepository.SaveAsync(messages);

        return Result.Success();
    }
}