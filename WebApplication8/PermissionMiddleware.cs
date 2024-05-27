namespace WebApplication8
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly PermissionsService _permissionsService;

        public PermissionMiddleware(RequestDelegate next, PermissionsService permissionsService)
        {
            _next = next;
            _permissionsService = permissionsService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path;

            // Skip token validation for the login endpoint
            if (path.StartsWithSegments("/account/login")
                || path.StartsWithSegments("/account/logout"))
            {
                await _next(context);
                return;
            }
            if (context.User.Identity.IsAuthenticated)
            {
                var userId = context.User.FindFirst("userId")?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    var permissions = await _permissionsService.GetUserPermissionsAsync(userId);
                    if (permissions.Any())
                    {
                        var requiredPermission = context.GetEndpoint()?.Metadata.GetMetadata<RequiredPermissionAttribute>()?.Permission;
                        if (requiredPermission != null && permissions.Contains(requiredPermission, StringComparer.OrdinalIgnoreCase))
                        {
                            await _next(context);
                            return;
                        }
                    }
                }
            }

            context.Response.StatusCode = StatusCodes.Status403Forbidden;
        }
    }
}
