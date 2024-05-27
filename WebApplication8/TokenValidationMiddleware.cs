using System.IdentityModel.Tokens.Jwt;

namespace WebApplication8
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token))
                {
                    try
                    {
                        var jwtToken = handler.ReadJwtToken(token);
                        var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "userId");
                        if (userIdClaim != null)
                        {
                            context.Items["UserId"] = userIdClaim.Value;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception
                        Console.WriteLine($"Error reading JWT token: {ex.Message}");
                    }
                }
            }

            await _next(context);
        }
    }
}
