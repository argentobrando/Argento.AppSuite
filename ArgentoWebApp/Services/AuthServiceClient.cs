using ArgentoWebApp.Models.DTO;

namespace ArgentoWebApp.Services
{
	public class AuthServiceClient
	{
		private readonly HttpClient _httpClient;
		private readonly JwtSessionService _jwtSession;

		public AuthServiceClient(HttpClient httpClient, JwtSessionService jwtSession)
		{
			_httpClient = httpClient;
			_jwtSession = jwtSession;
		}

		public async Task<bool> LoginAsync(string email, string password)
		{
			var loginDto = new LoginDto { Email = email, Password = password };
			var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(loginDto), System.Text.Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync("auth/login", content);

			if (!response.IsSuccessStatusCode) return false;

			var json = await response.Content.ReadAsStringAsync();
			var authResponse = System.Text.Json.JsonSerializer.Deserialize<AuthResponseDto>(json, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			_jwtSession.SetJwtToken(authResponse?.Token ?? "");
			return true;
		}

		public void AddJwtToClient()
		{
			if (_jwtSession.HasJwtToken)
			{
				_httpClient.DefaultRequestHeaders.Authorization =
					new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _jwtSession.GetJwtToken());
			}
		}
	}
}