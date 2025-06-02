using Microsoft.EntityFrameworkCore;
using InventoryService.API.Models;

namespace InventoryService.API.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<Product> Products { get; set; }
	}
}
