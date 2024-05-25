using Chat.Contacts.Application.Commands;
using Chat.Contacts.Domain.Entities;
using Chat.Contacts.Domain.Repositories;
using Chat.Contacts.Domain.Results;
using Peacious.Framework.CQRS;
using Peacious.Framework.Identity;
using Peacious.Framework.Results;

namespace Chat.Contacts.Application.CommandHandlers;

public class CreateNewGroupCommandHandler : ICommandHandler<CreateNewGroupCommand>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IScopeIdentity _scopeIdentity;

    public CreateNewGroupCommandHandler(
        IGroupRepository groupRepository,
        IScopeIdentity scopeIdentity)
    {
        _scopeIdentity = scopeIdentity;
        _groupRepository = groupRepository;
    }

    public Task<IResult> Handle(CreateNewGroupCommand request, CancellationToken cancellationToken)
    {
        return HandleAsync(request);
    }

    public async Task<IResult> HandleAsync(CreateNewGroupCommand request)
    {
        var groupName = request.GroupName;
        var userId = _scopeIdentity.GetUserId()!;

        var groupCreateResult = Group.Create(groupName, userId);

        var group = groupCreateResult.Value;

        if (groupCreateResult.IsFailure || group is null)
        {
            return groupCreateResult;
        }

        await _groupRepository.SaveAsync(group);

        var result = Result.Success().GroupCreated();

        result.SetData("GroupId", group.Id);

        return result;
    }
}