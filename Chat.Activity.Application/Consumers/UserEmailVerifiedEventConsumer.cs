using Chat.Activity.Domain.Entities;
using Chat.Activity.Domain.Repositories;
using Chat.Domain.Shared.Events;
using Peacious.Framework.EDD;
using Peacious.Framework.Identity;
using Peacious.Framework.MessageBrokers;

namespace Chat.Activity.Application.Consumers;

public class UserEmailVerifiedEventConsumer : AEventConsumer<UserEmailVerifiedEvent>
{
    private readonly IPresenceRepository _presenceRepository;

    public UserEmailVerifiedEventConsumer(IPresenceRepository presenceRepository, IScopeIdentity scopeIdentity)
        : base(scopeIdentity)
    {
        _presenceRepository = presenceRepository;
    }

    protected override async Task OnConsumeAsync(UserEmailVerifiedEvent @event, IMessageContext<UserEmailVerifiedEvent>? context = null)
    {
        var result = Presence.Create(@event.UserId);

        if (result is { IsSuccess: true, Value: not null })
        {
            await _presenceRepository.SaveAsync(result.Value);
        }
    }
}
