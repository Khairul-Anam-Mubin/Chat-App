using Chat.Application.Commands;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Peacious.Framework.CQRS;
using Peacious.Framework.Identity;
using Peacious.Framework.Results;

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

    public Task<IResult> Handle(UpdateMessageStatusCommand request, CancellationToken cancellationToken)
    {
        return HandleAsync(request);
    }

    public async Task<IResult> HandleAsync(UpdateMessageStatusCommand command)
    {
        var userId = _scopeIdentity.GetUserId()!;

        var conversationId = 
            Conversation.GetConversationId(userId, command.OpenedChatUserId);

        var conversation = 
            await _conversationRepository.GetByIdAsync(conversationId);
        
        if (conversation is null)
        {
            return Error.NotFound<Conversation>();
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