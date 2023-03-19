using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Revise_PageModel.Models;
using Revise_PageModel.Services;
using System;
using System.Linq;

namespace Revise_PageModel.Pages
{
    public class ProductPageModel : PageModel
    {
        private readonly ILogger<ProductPageModel> _logger;
        public readonly ProductService productService;
        public ProductPageModel(ILogger<ProductPageModel> logger, ProductService _productService)
        {
            _logger= logger;
            productService= _productService;
        }
        public Product product { get; set; }
        public void OnGet(int? id)
        {
            if (id != null)
            {
               
				ViewData["title"] = $"San pham co ID = {id}";
                product = productService.Find((int)id);
			}
            else
            {
				ViewData["title"] = "danh sach sp";
			}
            
        }
        public IActionResult OnGetLastProduct()
        {
            ViewData["title"] = "San pham cuoi";
			product = productService.AllProducts().LastOrDefault();
            if(product != null)
            {
                return Page();
            }
            else
            {
                return this.Content("Khong thay san pham");
            }
		}
        public IActionResult OnGetRemoveAll()
        {
            productService.AllProducts().Clear();
            return RedirectToPage("ProductPage");
        }
		public IActionResult OnGetLoad()
		{
			productService.LoadProduct();
			return RedirectToPage("ProductPage");
		}
		public IActionResult OnPostDelete(int? id)
		{
            if(id != null)
            {
                product = productService.Find(id.Value);
                if(product != null)
                {
                    productService.AllProducts().Remove(product);
                }
            }
			
			return RedirectToPage("ProductPage", new  { id = string.Empty});
		}

	}
}
