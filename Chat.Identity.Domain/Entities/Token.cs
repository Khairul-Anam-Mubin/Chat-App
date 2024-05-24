using Peacious.Framework.ORM.Interfaces;
using Peacious.Framework.Results;

namespace Chat.Identity.Domain.Entities;

public class Token : IRepositoryItem
{
    public string Id { get; private set; }
    public string AccessToken { get; private set; }
    public string UserId { get; private set; }
    public string AppId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool Expired { get; private set; }

    private Token(string refreshToken, string accessToken, string userId, string appId)
    {
        Id = refreshToken;
        AccessToken = accessToken;
        UserId = userId;
        AppId = appId;
        CreatedAt = DateTime.UtcNow;
        Expired = false;
    }

    public static IResult<Token> Create(string refreshToken, string accessToken, string userId, string appId)
    {
        return Result.Success(new Token(refreshToken, accessToken, userId, appId));
    }

    public void MakeTokenExpired()
    {
        Expired = true;
    }

    public IResult IsTokenAllowedToRefresh(string accessToken, string appId)
    {
        if (AccessToken != accessToken)
        {
            return Result.Error("AccessToken Error");
        }

        if (AppId != appId)
        {
            return Result.Error("AppId Error");
        }

        return Result.Success();
    }

    public IResult CheckForValidRefreshAttempt()
    {
        if (Expired)
        {
            return Result.Error("Suspicious Token refresh attempt");
        }

        return Result.Success();
    }
}