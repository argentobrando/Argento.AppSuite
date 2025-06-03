namespace AuthService.API.Services
{
	public interface IAuthService
	{
		Task<bool> ValidateCredentialsAsync(string username, string password);
		Task<bool> RegisterUserAsync(string username, string email, string password);
	}
}