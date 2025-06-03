namespace AuthService.API.Services
{
	public class AuthUserService : IAuthService
	{
		private readonly IUserService _userService;

		public AuthUserService(IUserService userService)
		{
			_userService = userService;
		}

		public async Task<bool> ValidateCredentialsAsync(string username, string password)
		{
			return await _userService.ValidateCredentialsAsync(username, password);
		}

		public async Task<bool> RegisterUserAsync(string username, string email, string password)
		{
			return await _userService.RegisterUserAsync(username, email, password);
		}
	}
}