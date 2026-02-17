using Microsoft.AspNetCore.Http;
using WPM.Core.Interfaces;
using WPM.Core.Models;

namespace WPM.Infrastructure.Services;

public class SiteMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ISiteResolver resolver)
    {
        var host = context.Request.Host.Value;
        if (string.IsNullOrEmpty(host))
        {
            context.Response.StatusCode = 400;
            return;
        }

        var siteContext = await resolver.ResolveAsync(host, context.RequestAborted);
        if (siteContext is null)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync($"Site not found for host: {host}");
            return;
        }

        context.Items["SiteContext"] = siteContext;
        await next(context);
    }
}

public static class SiteMiddlewareExtensions
{
    public static SiteContext GetSiteContext(this HttpContext context) =>
        context.Items["SiteContext"] as SiteContext
        ?? throw new InvalidOperationException("SiteContext not available. Is SiteMiddleware registered?");
}
