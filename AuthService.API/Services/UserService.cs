using AuthService.API.Data;
using AuthService.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.API.Services
{
	public class UserService : IUserService
	{
		private readonly AuthDbContext _db;

		public UserService(AuthDbContext db)
		{
			_db = db;
		}

		public async Task<User> GetUserByUsernameAsync(string username)
		{
			return await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
		}

		public async Task<bool> ValidateCredentialsAsync(string username, string password)
		{
			var user = await GetUserByUsernameAsync(username);
			if (user == null)
				return false;

			var hash = HashPassword(password);
			return user.PasswordHash == hash;
		}

		public async Task<bool> RegisterUserAsync(string username, string email, string password)
		{
			if (await GetUserByUsernameAsync(username) != null)
				return false;

			var user = new User
			{
				Username = username,
				Email = email,
				PasswordHash = HashPassword(password)
			};

			_db.Users.Add(user);
			await _db.SaveChangesAsync();
			return true;
		}

		private string HashPassword(string password)
		{
			using var sha256 = SHA256.Create();
			var bytes = Encoding.UTF8.GetBytes(password);
			var hashBytes = sha256.ComputeHash(bytes);
			return Convert.ToBase64String(hashBytes);
		}
	}
}