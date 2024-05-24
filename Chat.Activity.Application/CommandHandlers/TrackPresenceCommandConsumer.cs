using Chat.Activity.Domain.Repositories;
using Chat.Activity.Domain.Results;
using Chat.Domain.Shared.Commands;
using Peacious.Framework.CQRS;
using Peacious.Framework.Identity;
using Peacious.Framework.MessageBrokers;
using Peacious.Framework.Results;

namespace Chat.Activity.Application.CommandHandlers;

public class TrackPresenceCommandConsumer : ACommandConsumer<TrackPresenceCommand>
{
    private readonly IPresenceRepository _presenceRepository;

    public TrackPresenceCommandConsumer(IPresenceRepository presenceRepository, IScopeIdentity scopeIdentity) 
        : base(scopeIdentity)
    {
        _presenceRepository = presenceRepository;
    }

    protected override async Task<IResult> OnConsumeAsync(TrackPresenceCommand command, IMessageContext<TrackPresenceCommand>? context = null)
    {
        var userId = ScopeIdentity.GetUserId()!;

        if (!await _presenceRepository.TrackPresenceAsync(userId))
        {
            return Result.Error().TrackPresenceFailed();
        }
        
        return Result.Success();
    }
}