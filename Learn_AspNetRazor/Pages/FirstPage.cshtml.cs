using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Learn_AspNetRazor.Pages
{
    public class FirstPageModel : PageModel
    {
        public string title { get; set; } = "Trang razor cua MVQ";
		public void OnGet()
		{
			Console.WriteLine("Truy van GET");
			ViewData["myData"] = "razorpage demo 2023";
		}
		//url: ...?handler=Xyz
		public void OnGetXyz()
		{
			Console.WriteLine("Truy van GET");
			ViewData["myData"] = "razorpage demo 2023 XYZ";
		}
	}
}
