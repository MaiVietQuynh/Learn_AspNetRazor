using EF_Identity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EF_Identity.Pages
{
	public class IndexModel : PageModel
	{
		
		private readonly ILogger<IndexModel> _logger;
		private readonly MyBlogContext _MyBlogContext;
		public IndexModel(ILogger<IndexModel> logger, MyBlogContext myBlogContext)
		{
			_MyBlogContext= myBlogContext;
			_logger = logger;
		}

		public void OnGet()
		{
			
			var posts = (from p in _MyBlogContext.Articles 
						orderby p.Created descending
						select p).ToList();
			ViewData["posts"] = posts;
		}
	}
}
