using AuthService.API.Models;

namespace AuthService.API.Data
{
	public class DbInitializer
	{
		public static void Initialize(AppDbContext context)
		{
			context.Database.EnsureCreated();

			if (context.Users.Any())
			{
				return; // Already seeded
			}

			var users = new User[]
			{
				new User
				{
					Username = "admin",
					PasswordHash = BCrypt.Net.BCrypt.HashPassword("password")
				}
			};

			context.Users.AddRange(users);
			context.SaveChanges();
		}
	}
}
