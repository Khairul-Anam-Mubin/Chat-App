using Chat.Contact.Application.Commands;
using Chat.Contact.Domain.Interfaces;
using Chat.Contact.Domain.Models;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.Results;

namespace Chat.Contact.Application.CommandHandlers;

public class AddMemberToGroupCommandHandler : ICommandHandler<AddMemberToGroupCommand>
{
    private readonly IGroupMemberRepository _groupMemberRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IScopeIdentity _scopeIdentity;

    public AddMemberToGroupCommandHandler(
        IGroupMemberRepository groupMemberRepository, 
        IGroupRepository groupRepository,
        IScopeIdentity scopeIdentity)
    {
        _groupMemberRepository = groupMemberRepository;
        _groupRepository = groupRepository;
        _scopeIdentity = scopeIdentity;
    }

    public async Task<IResult> HandleAsync(AddMemberToGroupCommand request)
    {
        var groupId = request.GroupId;
        var memberId = request.MemberId;
        var addedBy = _scopeIdentity.GetUserId();

        var group = await _groupRepository.GetByIdAsync(groupId);

        if (group is null)
        {
            return Result.Error("Group not exists");
        }

        var result = group.AddNewMemberToGroup(
            addedBy, 
            memberId,
            await _groupMemberRepository.IsUserAlreadyExistsInGroupAsync(groupId, memberId));

        if (result.IsFailure)
        {
            return result;
        }

        foreach (var member in group.Members())
        {
            await _groupMemberRepository.SaveAsync(member);
        }

        return Result.Success("Member added to group successfully");
    }
}