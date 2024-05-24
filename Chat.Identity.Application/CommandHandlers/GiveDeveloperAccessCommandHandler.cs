using Chat.Domain.Shared.Constants;
using Chat.Identity.Application.Commands;
using Chat.Identity.Domain.Entities;
using Chat.Identity.Domain.Repositories;
using Peacious.Framework.CQRS;
using Peacious.Framework.Results;

namespace Chat.Identity.Application.CommandHandlers;

public class GiveDeveloperAccessCommandHandler : ICommandHandler<GiveDeveloperAccessCommand>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUserAccessRepository _userAccessRepository;

    public GiveDeveloperAccessCommandHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository, IUserAccessRepository userAccessRepository)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _userAccessRepository = userAccessRepository;
    }

    public async Task<IResult> HandleAsync(GiveDeveloperAccessCommand command)
    {
        var userId = command.UserId;

        var userAccess = UserAccess.Create(userId);

        var developerRole = await _roleRepository.GetRoleByTitleAsync(Roles.Developer);

        if (developerRole is null)
        {
            return Result.Error("Developer role not found");
        }

        userAccess.AddRole(developerRole);

        var permissions = 
            await _permissionRepository.GetManyByIdsAsync(developerRole.PermissionIds);

        permissions.ForEach(permission => userAccess.AddPermission(permission));

        await _userAccessRepository.SaveAsync(userAccess);

        return Result.Success();
    }
}
