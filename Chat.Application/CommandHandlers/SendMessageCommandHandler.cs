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
    private readonly IScopeIdentity _scopeIdentity;

    public SendMessageCommandHandler(
        IScopeIdentity scopeIdentity,
        IConversationRepository conversationRepository)
    {
        _scopeIdentity = scopeIdentity;
        _conversationRepository = conversationRepository;
    }

    public async Task<IResult> HandleAsync(SendMessageCommand command)
    {
        var senderId = _scopeIdentity.GetUserId()!;
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
            return Error.SaveProblem<Conversation>();
        }
        
        return Result.Success();
    }
}