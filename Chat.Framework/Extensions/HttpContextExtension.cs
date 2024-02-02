using Microsoft.AspNetCore.Http;

namespace Chat.Framework.Extensions;

public static class HttpContextExtension
{
    public static string? GetAccessToken(this HttpContext httpContext)
    {
        var headerToken = httpContext?.Request?.Headers.Authorization;
        var queryToken = httpContext?.Request?.Query["access_token"];
        
        if (!string.IsNullOrEmpty(headerToken))
        {
            return headerToken;
        }
        
        return queryToken;
    }
}