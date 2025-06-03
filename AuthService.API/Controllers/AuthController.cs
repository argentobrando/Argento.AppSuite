using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthService.API.Services;
using Microsoft.Extensions.Options;
using AuthService.API.Settings;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using AuthService.API.Models.DTO;

namespace AuthService.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		private readonly JwtSettings _jwtSettings;

		public AuthController(IAuthService authService, IOptions<JwtSettings> jwtSettings)
		{
			_authService = authService;
			_jwtSettings = jwtSettings.Value;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
		{
			bool valid = await _authService.ValidateCredentialsAsync(request.Username, request.Password);

			if (valid)
			{
				// Issue JWT
				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, request.Username),
					new Claim(ClaimTypes.Role, "User")
				};

				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
				var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

				var token = new JwtSecurityToken(
					issuer: _jwtSettings.Issuer,
					audience: _jwtSettings.Audience,
					claims: claims,
					expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
					signingCredentials: creds
				);

				var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

				return Ok(new { token = tokenString });
			}
			else
			{
				return Unauthorized();
			}
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
		{
			bool success = await _authService.RegisterUserAsync(request.Username, request.Email, request.Password);

			if (success)
			{
				return Ok();
			}
			else
			{
				return Conflict("User already exists.");
			}
		}

		[Authorize]
		[HttpGet("userinfo")]
		public async Task<ActionResult<UserInfoDto>> UserInfo([FromServices] IUserService userService)
		{
			var username = User.Identity?.Name;

			if (string.IsNullOrEmpty(username))
				return Unauthorized();

			var user = await userService.GetUserByUsernameAsync(username);

			if (user == null)
				return Unauthorized();

			return new UserInfoDto
			{
				Username = user.Username,
				Email = user.Email,
				Roles = User.Claims
					.Where(c => c.Type == ClaimTypes.Role)
					.Select(c => c.Value)
					.ToArray()
			};
		}
	}
}
