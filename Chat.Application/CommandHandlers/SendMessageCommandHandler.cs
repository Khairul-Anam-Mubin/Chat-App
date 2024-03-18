using Chat.Application.Commands;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.Results;

namespace Chat.Application.CommandHandlers;

public class SendMessageCommandHandler : ICommandHandler<SendMessageCommand>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IScopeIdentity _scopIdentity;

    public SendMessageCommandHandler(
        IScopeIdentity scopeIdentity,
        IConversationRepository conversationRepository)
    {
        _scopIdentity = scopeIdentity;
        _conversationRepository = conversationRepository;
    }

    public async Task<IResult> HandleAsync(SendMessageCommand command)
    {
        var senderId = _scopIdentity.GetUserId()!;
        var receiverId = command.SendTo;
        var messageContent = command.MessageContent;
        var isGroupMessage = command.IsGroupMessage;

        var conversationId = Conversation.GetConversationId(senderId, receiverId);

        var conversation = 
            await _conversationRepository.GetByIdAsync(conversationId);

        conversation ??=
                Conversation.Create(
                    conversationId, 
                    senderId, 
                    receiverId, 
                    isGroupMessage);

        var messageAddResult = 
            conversation.AddNewMessage(senderId, receiverId, messageContent);

        if (messageAddResult.IsFailure)
        {
            return messageAddResult;
        }

        if (!await _conversationRepository.SaveAsync(conversation))
        {
            return Result.Error("Failed to persist the message");
        }
        
        return Result.Success();
    }
}