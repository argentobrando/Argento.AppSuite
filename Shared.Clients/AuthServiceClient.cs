using Shared.Clients.Data.DTO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace Shared.Clients
{
	public class AuthServiceClient
	{
		private readonly HttpClient _http;

		public string JwtToken { get; set; }
		public AuthServiceClient(HttpClient http)
		{
			_http = http;
		}

		public async Task<bool> LoginAsync(LoginRequestDto request)
		{
			var response = await _http.PostAsJsonAsync("api/auth/login", request);

			if (response.IsSuccessStatusCode)
			{
				// Read JWT from response
				var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
				JwtToken = result?.Token;

				return true;
			}

			return false;
		}

		public async Task LogoutAsync()
		{
			// Optional: clear token
			JwtToken = null;
			await _http.PostAsync("api/auth/logout", null);
		}

		public async Task<UserInfoDto> GetUserInfoAsync()
		{
			if (!string.IsNullOrEmpty(JwtToken))
			{
				_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
			}

			return await _http.GetFromJsonAsync<UserInfoDto>("api/auth/userinfo");
		}

		public async Task<bool> RegisterAsync(RegisterRequestDto request)
		{
			var response = await _http.PostAsJsonAsync("api/auth/register", request);
			return response.IsSuccessStatusCode;
		}
	}

	public class LoginResponseDto
	{
		public string Token { get; set; }
	}
}
