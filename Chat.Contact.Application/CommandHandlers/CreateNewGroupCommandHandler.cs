using Chat.Contact.Application.Commands;
using Chat.Contact.Domain.Entities;
using Chat.Contact.Domain.Repositories;
using Chat.Contact.Domain.Results;
using Chat.Framework.CQRS;
using Chat.Framework.EDD;
using Chat.Framework.Identity;
using Chat.Framework.Results;

namespace Chat.Contact.Application.CommandHandlers;

public class CreateNewGroupCommandHandler : ICommandHandler<CreateNewGroupCommand>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IScopeIdentity _scopeIdentity;
    private readonly IEventService _eventService;

    public CreateNewGroupCommandHandler(
        IGroupRepository groupRepository, 
        IScopeIdentity scopeIdentity,
        IEventService eventService)
    {
        _scopeIdentity = scopeIdentity;
        _groupRepository = groupRepository;
        _eventService = eventService;
    }

    public async Task<IResult> HandleAsync(CreateNewGroupCommand request)
    {
        var groupName = request.GroupName;
        var userId = _scopeIdentity.GetUserId()!;

        var groupCreateResult = GroupModel.Create(groupName, userId);

        if (groupCreateResult.IsFailure || groupCreateResult.Value is null)
        {
            return groupCreateResult;
        }

        var group = groupCreateResult.Value;

        await _groupRepository.SaveAsync(group);

        await _eventService.PublishEventAsync(group.DomainEvents.FirstOrDefault()!);

        var result = Result.Success().GroupCreated();
        
        result.SetData("GroupId", group.Id);

        return result;
    }
}