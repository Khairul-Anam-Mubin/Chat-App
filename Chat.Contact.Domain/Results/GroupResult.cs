using Peacious.Framework.Results;

namespace Chat.Contacts.Domain.Results;

public static class GroupResult
{
    public static IResult MemberAlreadyExist(this IResult result)
        => result.SetMessage($"Member already exist in the group.");

    public static IResult GroupAddMemberPermissionProblem(this IResult result)
        => result.SetMessage($"Does not have permission to add member in the group.");

    public static IResult GroupNotFound(this IResult result)
        => result.SetMessage("Group not found.");

    public static IResult MemberAdded(this IResult result)
        => result.SetMessage("Member added to the group successfully.");

    public static IResult GroupCreated(this IResult result)
        => result.SetMessage("Group created successfully.");
}
