using AuthService.API.Data;
using AuthService.API.Models;
using AuthService.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly TokenService _tokenService;

		public AuthController(AppDbContext context, TokenService tokenService)
		{
			_context = context;
			_tokenService = tokenService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<User>>> GetUsers()
		{
			return await _context.Users.ToListAsync();
		}

		[HttpPost]
		public async Task<ActionResult<User>> CreateUser(User user)
		{
			_context.Users.Add(user);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
		}

		[HttpPost("login")]
		public IActionResult Login([FromBody] UserLogin login)
		{
			var user = _context.Users.SingleOrDefault(u => u.Username == login.Username);

			if (user != null && BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
			{
				var token = _tokenService.GenerateToken(user.Username);
				return Ok(new { token });
			}

			return Unauthorized();
		}
	}
}
