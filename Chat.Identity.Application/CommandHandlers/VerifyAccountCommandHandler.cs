using Chat.Domain.Shared.Events;
using Chat.Framework.CQRS;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Results;
using Chat.Identity.Application.Commands;
using Chat.Identity.Domain.Repositories;

namespace Chat.Identity.Application.CommandHandlers;

public class VerifyAccountCommandHandler : ICommandHandler<VerifyAccountCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IEventBus _eventBus;
    private readonly IEventService _eventService;

    public VerifyAccountCommandHandler(IUserRepository userRepository, IEventBus eventBus, IEventService eventService)
    {
        _userRepository = userRepository;
        _eventBus = eventBus;
        _eventService = eventService;
    }

    public async Task<IResult> HandleAsync(VerifyAccountCommand request)
    {
        if (!await _userRepository.UpdateEmailVerificationStatus(request.UserId, true))
        {
            return Result.Error("Account verification failed");
        }
        
        var verifiedUserAccountCreatedEvent = 
            new VerifiedUserAccountCreatedEvent(request.UserId);
        
        await _eventService.PublishAsync(verifiedUserAccountCreatedEvent);
        
        return Result.Success("Account verified successfully.");
    }
}