using Peacious.Framework.Results;

namespace Chat.Activity.Domain.Results;

public static class PresenceResult
{
    public static IResult<T> IdNotSet<T>(this IResult<T> result)
        => (IResult<T>)result.SetMessage("Id not set.");
    
    public static IResult TrackPresenceFailed(this IResult result)
        => result.SetMessage("Track presence failed.");
}
