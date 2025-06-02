using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Shared.Models;

namespace Shared.Clients
{
	public class InventoryApiClient
	{
		private readonly HttpClient _httpClient;

		public InventoryApiClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<List<Product>> GetProductsAsync()
		{
			return await _httpClient.GetFromJsonAsync<List<Product>>("api/products")
				?? new List<Product>();
		}
	}
}