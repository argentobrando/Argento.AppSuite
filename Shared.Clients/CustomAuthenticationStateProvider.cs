using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Shared.Clients
{
	public class CustomAuthenticationStateProvider : AuthenticationStateProvider
	{
		private string? _token;

		public void SetToken(string? token)
		{
			_token = token;
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}

		public override Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			ClaimsIdentity identity = new();

			if (!string.IsNullOrWhiteSpace(_token))
			{
				var handler = new JwtSecurityTokenHandler();
				var jwt = handler.ReadJwtToken(_token);

				var claims = jwt.Claims;
				identity = new ClaimsIdentity(claims, "jwt");
			}

			var user = new ClaimsPrincipal(identity);
			return Task.FromResult(new AuthenticationState(user));
		}

		public string? GetToken() => _token;
	}
}