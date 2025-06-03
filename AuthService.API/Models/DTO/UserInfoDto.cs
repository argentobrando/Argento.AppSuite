namespace AuthService.API.Models.DTO
{
	public class UserInfoDto
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public string[] Roles { get; set; } = Array.Empty<string>();
	}
}
