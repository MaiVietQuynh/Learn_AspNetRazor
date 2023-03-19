using Revise_PageModel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Revise_PageModel.Services
{
	public class ProductService
	{
		public ProductService() 
		{
			LoadProduct();
		}
		private List<Product> products= new List<Product>();
		public Product Find(int id)
		{
			var qr = from p in products
					 where p.Id == id
					 select p;
			return qr.FirstOrDefault();
		}
		public List<Product> AllProducts() => products;
		public void LoadProduct()
		{
			products.Clear();
			products.Add(new Product()
			{
				Id = 1,
				Name = "Iphone",
				Description = "Dien thoai cua Apple",
				Price = 2000
			}) ;
			products.Add(new Product()
			{
				Id = 2,
				Name = "Samsung",
				Description = "Dien thoai cua Samsung",
				Price = 1000
			});
			products.Add(new Product()
			{
				Id = 3,
				Name = "Huawei",
				Description = "Dien thoai cua Huawei",
				Price = 3000
			});
		}

	}
}
