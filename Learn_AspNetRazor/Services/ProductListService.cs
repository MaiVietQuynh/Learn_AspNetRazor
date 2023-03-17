using Learn_AspNetRazor.Models;
using System.Collections.Generic;

namespace Learn_AspNetRazor.Services
{
	public class ProductListService
	{
		public List<Product> products { get; set; } = new List<Product>()
		{
			new Product() { Name = "San pham 1 ", Description = "Mo ta san pham 1", Price = 1000 },
			new Product() { Name = "San pham 2 ", Description = "Mo ta san pham 2", Price = 2000 },
			new Product() { Name = "San pham 3 ", Description = "Mo ta san pham 3", Price = 1500 },
			new Product() { Name = "San pham 4 ", Description = "Mo ta san pham 4", Price = 600 }

		};
	}
}
