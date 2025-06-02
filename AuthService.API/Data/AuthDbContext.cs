using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuthService.API.Models;

namespace AuthService.API.Data
{
	public class AuthDbContext : IdentityDbContext<ApplicationUser>
	{
		public AuthDbContext(DbContextOptions<AuthDbContext> options)
			: base(options)
		{
		}
	}
}