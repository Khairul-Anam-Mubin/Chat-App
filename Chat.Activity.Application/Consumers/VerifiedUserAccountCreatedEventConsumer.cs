﻿using Chat.Activity.Domain.Interfaces.Repositories;
using Chat.Activity.Domain.Models;
using Chat.Domain.Shared.Events;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.MessageBrokers;

namespace Chat.Activity.Application.Consumers;

public class VerifiedUserAccountCreatedEventConsumer : AEventConsumer<VerifiedUserAccountCreatedEvent>
{
    private readonly ILastSeenRepository _lastSeenRepository;

    public VerifiedUserAccountCreatedEventConsumer(ILastSeenRepository lastSeenRepository, IScopeIdentity scopeIdentity)
        : base(scopeIdentity)
    {
        _lastSeenRepository = lastSeenRepository;
    }

    protected override async Task OnConsumeAsync(VerifiedUserAccountCreatedEvent @event, IMessageContext<VerifiedUserAccountCreatedEvent>? context = null)
    {
        var lastSeenModel = new LastSeenModel(@event.UserId);
        await _lastSeenRepository.SaveAsync(lastSeenModel);
    }
}
