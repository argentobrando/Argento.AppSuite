using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
#if WINDOWS || ANDROID || IOS
using Microsoft.Maui.Storage;
#endif

namespace Shared.Clients;

public class AuthServiceClient
{
	private readonly HttpClient _httpClient;
	private readonly AuthenticationStateProvider _authStateProvider;
	private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

	public AuthServiceClient(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
	{
		_httpClient = httpClient;
		_authStateProvider = authStateProvider;
	}

	public async Task<bool> IsAuthenticatedAsync()
	{
		var state = await _authStateProvider.GetAuthenticationStateAsync();
		return state.User.Identity?.IsAuthenticated ?? false;
	}

	public async Task<ClaimsPrincipal> GetUserAsync()
	{
		var state = await _authStateProvider.GetAuthenticationStateAsync();
		return state.User;
	}

	public async Task<bool> SignInAsync(string username, string password)
	{
		var response = await _httpClient.PostAsJsonAsync("/api/auth/login", new { username, password });

		if (!response.IsSuccessStatusCode)
			return false;

#if WINDOWS || ANDROID || IOS
        // MAUI Hybrid - store tokens manually
        var tokens = await response.Content.ReadFromJsonAsync<TokenResponse>(_jsonOptions);
        if (tokens != null)
        {
            await SecureStorage.SetAsync("access_token", tokens.AccessToken);
            await SecureStorage.SetAsync("refresh_token", tokens.RefreshToken);
            return true;
        }
        return false;
#else
		// Web - cookies are automatically managed by server
		return true;
#endif
	}

	public async Task SignOutAsync()
	{
#if WINDOWS || ANDROID || IOS
        await SecureStorage.Default.RemoveAsync("access_token");
        await SecureStorage.Default.RemoveAsync("refresh_token");
#endif
		await _httpClient.PostAsync("/api/auth/logout", null);
	}

	public async Task<string?> GetAccessTokenAsync()
	{
#if WINDOWS || ANDROID || IOS
        return await SecureStorage.GetAsync("access_token");
#else
		// Web - token is in cookie, no manual access
		return null;
#endif
	}

	public async Task<bool> RefreshTokenAsync()
	{
#if WINDOWS || ANDROID || IOS
        var refreshToken = await SecureStorage.GetAsync("refresh_token");
        if (string.IsNullOrEmpty(refreshToken))
            return false;

        var response = await _httpClient.PostAsJsonAsync("/api/auth/refresh", new { refreshToken });

        if (!response.IsSuccessStatusCode)
            return false;

        var tokens = await response.Content.ReadFromJsonAsync<TokenResponse>(_jsonOptions);
        if (tokens != null)
        {
            await SecureStorage.SetAsync("access_token", tokens.AccessToken);
            await SecureStorage.SetAsync("refresh_token", tokens.RefreshToken);
            return true;
        }
        return false;
#else
		// Web - refresh is handled via cookie by server
		return true;
#endif
	}

	private class TokenResponse
	{
		public string AccessToken { get; set; } = string.Empty;
		public string RefreshToken { get; set; } = string.Empty;
	}
}
