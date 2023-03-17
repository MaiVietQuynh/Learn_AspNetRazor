using Learn_AspNetRazor.Models;
using Learn_AspNetRazor.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Learn_AspNetRazor.Pages.Shared.Components.ProductBox
{
	//[ViewComponent]
	public class ProductBox : ViewComponent
	{
		private readonly ProductListService productService;
		public ProductBox(ProductListService _products)
		{
			productService = _products;
		}
		public IViewComponentResult Invoke(bool sapxeptang = true)
		{

			List<Product> _product = null;
			if(sapxeptang)
			{
				_product = productService.products.OrderBy(p=>p.Price).ToList();
			}
			else
			{
				_product = productService.products.OrderByDescending(p => p.Price).ToList();
			}
			return View<List<Product>>(_product); //Default.cshtml
		}
	}
}
