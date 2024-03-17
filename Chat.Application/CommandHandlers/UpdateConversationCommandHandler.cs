using Chat.Application.Commands;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Framework.CQRS;
using Chat.Framework.Results;

namespace Chat.Application.CommandHandlers;

public class UpdateConversationCommandHandler : ICommandHandler<UpdateConversationCommand>
{
    private readonly IConversationRepository _conversationRepository;
        
    public UpdateConversationCommandHandler(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    public async Task<IResult> HandleAsync(UpdateConversationCommand command)
    {
        var conversation = 
            await _conversationRepository.GetConversationAsync(command.Conversation!.UserId, command.Conversation.SendTo);
        
        if (conversation is null)
        {
            conversation = 
                Conversation.Create(
                    command.Conversation.Id, 
                    command.Conversation.UserId, 
                    command.Conversation.SendTo, 
                    command.Conversation.Content, 
                    command.Conversation.SentAt, 
                    command.Conversation.Status, 
                    command.Conversation.IsGroupMessage);
        }

        conversation.Update(
            command.Conversation.UserId,
            command.Conversation.SendTo,
            command.Conversation.Content,
            command.Conversation.Status,
            command.Conversation.SentAt);

        await _conversationRepository.SaveAsync(conversation);

        return Result.Success();
    }
}