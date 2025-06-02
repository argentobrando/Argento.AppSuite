using InventoryService.API.Data;
using InventoryService.API.Models;

namespace InventoryService.API.Data
{
	public static class DbInitializer
	{
		public static void Initialize(AppDbContext context)
		{
			// Ensure DB is created + migrations applied
			context.Database.EnsureCreated();

			// Check if data already exists
			if (context.Products.Any())
			{
				return; // DB has been seeded
			}

			// Seed data
			var products = new Product[]
			{
				new Product { Name = "Widget A", Price = 9.99m, Stock = 100 },
				new Product { Name = "Widget B", Price = 19.99m, Stock = 50 },
				new Product { Name = "Widget C", Price = 29.99m, Stock = 25 }
			};

			context.Products.AddRange(products);
			context.SaveChanges();
		}
	}
}
