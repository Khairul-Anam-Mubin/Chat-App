using Chat.Activity.Domain.Interfaces.Repositories;
using Chat.Activity.Domain.Models;
using Chat.Domain.Shared.Events;
using Chat.Framework.MessageBrokers;

namespace Chat.Activity.Application.Consumers;

public class VerifiedUserAccountCreatedEventConsumer : AMessageConsumer<VerifiedUserAccountCreatedEvent>
{
    private readonly ILastSeenRepository _lastSeenRepository;

    public VerifiedUserAccountCreatedEventConsumer(ILastSeenRepository lastSeenRepository)
    {
        _lastSeenRepository = lastSeenRepository;
    }

    public override async Task Consume(IMessageContext<VerifiedUserAccountCreatedEvent> context)
    {
        var lastSeenModel = new LastSeenModel(context.Message.UserId);
        await _lastSeenRepository.SaveAsync(lastSeenModel);
    }
}
