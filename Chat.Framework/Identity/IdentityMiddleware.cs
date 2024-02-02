using Chat.Framework.Extensions;
using Microsoft.AspNetCore.Http;

namespace Chat.Framework.Identity;

public class IdentityMiddleware : IMiddleware
{
    private readonly IScopeIdentity _scopeIdentity;
    
    public IdentityMiddleware(IScopeIdentity scopeIdentity)
    {
        _scopeIdentity = scopeIdentity;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _scopeIdentity.SwitchIdentity(context.GetAccessToken());

        await next(context);
    }
}