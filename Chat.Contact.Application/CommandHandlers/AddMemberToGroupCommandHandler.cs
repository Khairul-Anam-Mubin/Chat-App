using Chat.Contact.Application.Commands;
using Chat.Contact.Domain.Repositories;
using Chat.Contact.Domain.Results;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.Results;

namespace Chat.Contact.Application.CommandHandlers;

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
            await _groupRepository.IsUserAlreadyExistsInGroupAsync(groupId, memberId));

        if (result.IsFailure)
        {
            return result;
        }

        await _groupRepository.SaveGroupMembersAsync(group.Members());

        return Result.Success().MemberAdded();
    }
}