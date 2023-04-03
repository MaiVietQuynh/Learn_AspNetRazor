using EF_Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EF_Identity.Areas.Admin.Pages.User
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        public IndexModel(UserManager<AppUser> userManager) 
        {
            _userManager= userManager;
        }
		public const int ITEMS_PER_PAGE = 10;
		[BindProperty(SupportsGet = true, Name = "p")]
		public int currentPage { get; set; }
		public int countPages { get; set; }
		[TempData]
        public string StatusMessage { get; set; }
        public List<AppUser> users { get; set; }
        public int totalUsers { get; set; }

		public async Task OnGet()
        {
            //users = await _userManager.Users.OrderBy(u=>u.UserName).ToListAsync();
            var qr = _userManager.Users.OrderBy(u => u.UserName);
		    totalUsers = await qr.CountAsync();
			countPages = (int)Math.Ceiling((double)totalUsers / ITEMS_PER_PAGE);
			if (currentPage < 1)
				currentPage = 1;
			if (currentPage > countPages)
				currentPage = countPages;
			var qr1 = qr.Skip((currentPage - 1) * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE);
            users = await qr1.ToListAsync();

		}
        public void OnPost()
        {
            RedirectToPage();
        }
    }
}
