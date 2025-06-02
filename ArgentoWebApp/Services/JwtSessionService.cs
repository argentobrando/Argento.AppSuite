namespace ArgentoWebApp.Services
{
	public class JwtSessionService
	{
		private string _jwtToken = "";

		public void SetJwtToken(string token)
		{
			_jwtToken = token;
		}

		public string GetJwtToken()
		{
			return _jwtToken;
		}

		public bool HasJwtToken => !string.IsNullOrEmpty(_jwtToken);

		public void ClearJwtToken()
		{
			_jwtToken = "";
		}
	}
}
