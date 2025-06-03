using AuthService.API.Models;

namespace AuthService.API.Services
{
	public interface IUserService
	{
		Task<User> GetUserByUsernameAsync(string username);
		Task<bool> ValidateCredentialsAsync(string username, string password);
		Task<bool> RegisterUserAsync(string username, string email, string password);
	}
}
