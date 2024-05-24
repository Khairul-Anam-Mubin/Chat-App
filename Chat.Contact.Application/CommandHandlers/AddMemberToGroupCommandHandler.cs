using Chat.Contacts.Application.Commands;
using Chat.Contacts.Domain.Repositories;
using Chat.Contacts.Domain.Results;
using Peacious.Framework.CQRS;
using Peacious.Framework.Identity;
using Peacious.Framework.Results;

namespace Chat.Contacts.Application.CommandHandlers;

public class AddMemberToGroupCommandHandler : ICommandHandler<AddMemberToGroupCommand>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IScopeIdentity _scopeIdentity;

    public AddMemberToGroupCommandHandler(
        IGroupRepository groupRepository,
        IScopeIdentity scopeIdentity)
    {
        _groupRepository = groupRepository;
        _scopeIdentity = scopeIdentity;
    }

    public async Task<IResult> HandleAsync(AddMemberToGroupCommand request)
    {
        var groupId = request.GroupId;
        var memberId = request.MemberId;
        var addedBy = _scopeIdentity.GetUserId();

        var group = await _groupRepository.GetGroupByIdAsync(groupId);

        if (group is null)
        {
            return Result.Error().GroupNotFound();
        }

        var result = group.AddNewMemberToGroup(
            addedBy,
            memberId,
            await _groupRepository.IsUserAlreadyExistInGroupAsync(groupId, memberId));

        if (result.IsFailure)
        {
            return result;
        }

        await _groupRepository.SaveAsync(group);

        return Result.Success().MemberAdded();
    }
}