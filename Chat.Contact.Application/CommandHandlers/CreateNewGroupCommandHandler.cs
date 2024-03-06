using Chat.Contact.Application.Commands;
using Chat.Contact.Domain.Interfaces;
using Chat.Contact.Domain.Models;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.Results;

namespace Chat.Contact.Application.CommandHandlers;

public class CreateNewGroupCommandHandler : ICommandHandler<CreateNewGroupCommand>
{
    private readonly IGroupRepository _groupRepository;
    private readonly ICommandExecutor _commandExecutor;
    private readonly IScopeIdentity _scopeIdentity;

    public CreateNewGroupCommandHandler(IGroupRepository groupRepository, ICommandExecutor commandExecutor, IScopeIdentity scopeIdentity)
    {
        _scopeIdentity = scopeIdentity;
        _groupRepository = groupRepository;
        _commandExecutor = commandExecutor;
    }

    public async Task<IResult> HandleAsync(CreateNewGroupCommand request)
    {
        var groupName = request.GroupName;
        var userId = _scopeIdentity.GetUserId()!;

        var groupCreateResult = GroupModel.Create(groupName, userId);

        if (groupCreateResult.IsSuccess || groupCreateResult.Value is null)
        {
            return groupCreateResult;
        }

        var groupModel = groupCreateResult.Value;

        await _groupRepository.SaveAsync(groupModel);

        var addMemberToGroupCommand = new AddMemberToGroupCommand(groupModel.Id, userId, userId);

        await _commandExecutor.ExecuteAsync(addMemberToGroupCommand);

        var result = Result.Success("Group Created Successfully");
        
        result.SetData("GroupId", groupModel.Id);

        return result;
    }
}