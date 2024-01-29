using Chat.Contact.Application.Commands;
using Chat.Contact.Domain.Interfaces;
using Chat.Contact.Domain.Models;
using Chat.Framework.CQRS;
using Chat.Framework.Results;

namespace Chat.Contact.Application.CommandHandlers;

public class AddMemberToGroupCommandHandler : ICommandHandler<AddMemberToGroupCommand>
{
    private readonly IGroupMemberRepository _groupMemberRepository;
    private readonly IGroupRepository _groupRepository;

    public AddMemberToGroupCommandHandler(
        IGroupMemberRepository groupMemberRepository, 
        IGroupRepository groupRepository)
    {
        _groupMemberRepository = groupMemberRepository;
        _groupRepository = groupRepository;
    }

    public async Task<IResult> HandleAsync(AddMemberToGroupCommand request)
    {
        var groupId = request.GroupId;
        var memberId = request.MemberId;
        var addedBy = request.AddedBy;

        var groupModel = await _groupRepository.GetByIdAsync(groupId);

        if (groupModel is null)
        {
            return Result.Error("Group not exists");
        }

        if (groupModel.CreatedBy != addedBy)
        {
            return Result.Error("Can't add to group");
        }

        if (await _groupMemberRepository.IsUserAlreadyExistsInGroupAsync(groupId, memberId))
        {
            return Result.Error("User Already Exists in the group");
        }

        var groupMemberModel = new GroupMemberModel(groupId, memberId, addedBy);

        await _groupMemberRepository.SaveAsync(groupMemberModel);

        return Result.Success("Member added to group successfully");
    }
}