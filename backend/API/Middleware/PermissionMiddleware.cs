using Application.Interfaces;
using System.Security.Claims;

namespace API.Middleware;

public class PermissionMiddleware
{
    private readonly RequestDelegate _next;

    public PermissionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        IPermissionService permissionService)
    {
        var endpoint = context.GetEndpoint();
        var attribute = endpoint?.Metadata.GetMetadata<HasPermissionAttribute>();

        // Endpoint ما يحتاجش permission
        if (attribute == null)
        {
            await _next(context);
            return;
        }

        // ❌ مش authenticated
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        // userId من JWT
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var userId = Guid.Parse(userIdClaim.Value);

        // ❌ ما عندوش permission
        var hasPermission = await permissionService
            .HasPermissionAsync(userId, attribute.Permission);

        if (!hasPermission)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        await _next(context);
    }
}