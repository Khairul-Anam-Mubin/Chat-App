using Chat.Activity.Domain.Entities;
using Chat.Activity.Domain.Repositories;
using Chat.Domain.Shared.Events;
using Chat.Framework.EDD;
using Chat.Framework.Identity;
using Chat.Framework.MessageBrokers;

namespace Chat.Activity.Application.Consumers;

public class VerifiedUserAccountCreatedEventConsumer : AEventConsumer<VerifiedUserAccountCreatedEvent>
{
    private readonly IPresenceRepository _presenceRepository;

    public VerifiedUserAccountCreatedEventConsumer(IPresenceRepository presenceRepository, IScopeIdentity scopeIdentity)
        : base(scopeIdentity)
    {
        _presenceRepository = presenceRepository;
    }

    protected override async Task OnConsumeAsync(VerifiedUserAccountCreatedEvent @event, IMessageContext<VerifiedUserAccountCreatedEvent>? context = null)
    {
        var result = Presence.Create(@event.UserId);

        if (result is { IsSuccess: true, Value: not null })
        {
            await _presenceRepository.SaveAsync(result.Value);
        }
    }
}
