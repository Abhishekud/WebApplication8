using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication8.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        private readonly PermissionsService _permissionsService;

        public AccountController(IConfiguration configuration, UserService userService, PermissionsService permissionsService)
        {
            _configuration = configuration;
            _userService = userService;
            _permissionsService = permissionsService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            // Validate the user credentials (you should replace this with your own logic)
            if (IsValidUserCredentials(request.Username, request.Password, out string userId))
            {
                await _userService.OnUserLoginAsync(userId);
                var token = GenerateJwtToken(userId);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            var userId = User.FindFirst("userId")?.Value;
            if (userId != null)
            {
                _permissionsService.ClearPermissions(userId);
                return Ok();
            }
            return BadRequest();
        }

        private bool IsValidUserCredentials(string username, string password, out string userId)
        {
            // Dummy validation logic (replace with actual user validation)
            if (username == "testuser" && password == "password")
            {
                userId = "12345"; // This should be the actual user ID from your data source
                return true;
            }

            userId = null;
            return false;
        }

        private string GenerateJwtToken(string userId)
        {
            var secretKey = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim("userId", userId)
            };

            var token = new JwtSecurityToken(issuer,
              audience,
              claims,
              expires: DateTime.Now.AddHours(10),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
