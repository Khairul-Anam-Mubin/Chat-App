using Chat.Domain.Shared.Constants;
using Chat.Identity.Application.Commands;
using Chat.Identity.Domain.Entities;
using Chat.Identity.Domain.Repositories;
using Peacious.Framework.CQRS;
using Peacious.Framework.Results;

namespace Chat.Identity.Application.CommandHandlers;

public class GiveVisitorAccessCommandHandler : ICommandHandler<GiveVisitorAccessCommand>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUserAccessRepository _userAccessRepository;

    public GiveVisitorAccessCommandHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository, IUserAccessRepository userAccessRepository)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _userAccessRepository = userAccessRepository;
    }

    public async Task<IResult> HandleAsync(GiveVisitorAccessCommand command)
    {
        var userId = command.UserId;

        var userAccess = UserAccess.Create(userId);

        var visitorRole = await _roleRepository.GetRoleByTitleAsync(Roles.Visitor);

        if (visitorRole is null)
        {
            return Result.Error("Visitor role not found");
        }

        userAccess.AddRole(visitorRole);

        var permissions =
            await _permissionRepository.GetManyByIdsAsync(visitorRole.PermissionIds);

        permissions.ForEach(permission => userAccess.AddPermission(permission));

        await _userAccessRepository.SaveAsync(userAccess);

        return Result.Success();
    }
}
