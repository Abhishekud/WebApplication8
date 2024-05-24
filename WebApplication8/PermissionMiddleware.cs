using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly PermissionsService _permissionsService;
        private readonly UserService _userService;

        public PermissionMiddleware(RequestDelegate next, IMemoryCache cache, PermissionsService permissionsService, UserService userService)
        {
            _next = next;
            _cache = cache;
            _permissionsService = permissionsService;
            _userService = userService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Items["UserId"] is string userId)
            {
                try
                {
                    _userService.OnUserLogin(userId);
                    var permissions = _permissionsService.GetUserPermissions(userId);
                    if (permissions != null)
                    {
                        var requiredPermission = context.Request.Headers["Required-Permission"].FirstOrDefault();
                        if (requiredPermission != null && HasPermission(permissions, requiredPermission))
                        {
                            await _next(context);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error processing permissions for user {userId}: {ex.Message}");
                }
            }

            context.Response.StatusCode = StatusCodes.Status403Forbidden;
        }

        private bool HasPermission(Permissions permissions, string requiredPermission)
        {
            if (Enum.TryParse(requiredPermission, out Permissions requiredEnumPermission))
            {
                return (permissions & requiredEnumPermission) == requiredEnumPermission;
            }
            else
            {
                // Log invalid permission string
                Console.WriteLine($"Invalid required permission: {requiredPermission}");
                return false;
            }
        }
    }
}
