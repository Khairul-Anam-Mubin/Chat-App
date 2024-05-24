using Peacious.Framework.Results;

namespace Chat.Contacts.Domain.Results;

public static class ContactResult
{
    public static IResult ContactAdded(this IResult result)
        => result.SetMessage("Contact Added Successfully");

    public static IResult ContactAccepted(this IResult result)
        => result.SetMessage("Contact Accepted.");

    public static IResult ContactRejected(this IResult result)
        => result.SetMessage("Contact Rejected.");

    public static IResult ContactUserIdEmpty(this IResult result)
        => result.SetMessage("Contact user id can not be empty.");

    public static IResult InvalidContactUser(this IResult result)
        => result.SetMessage("Only contact user can accept request.");

    public static IResult ContactNotFound(this IResult result)
        => result.SetMessage("Contact");

    public static IResult ContactSaveProblem(this IResult result)
        => result.SetMessage("Contact");

    public static IResult ContactDeleteProblem(this IResult result)
        => result.SetMessage("Contact delete problem.");

    public static IResult GetUserFailed(this IResult result)
        => result.SetMessage("Get user failed.");
}
