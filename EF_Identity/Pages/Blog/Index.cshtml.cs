using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EF_Identity.Models;

namespace EF_Identity.Pages_Blog
{
    public class IndexModel : PageModel
    {

        private readonly EF_Identity.Models.MyBlogContext _context;

        public IndexModel(EF_Identity.Models.MyBlogContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get;set; }
        public const int ITEMS_PER_PAGE = 15;
        [BindProperty(SupportsGet =true,Name ="p")]
        public int currentPage { get; set; }
        public int countPages { get; set; }
        public async Task OnGetAsync(string SearchString)
        {
            int totalArticle = await _context.Articles.CountAsync();
            countPages = (int)Math.Ceiling((double)totalArticle / ITEMS_PER_PAGE);
            if (currentPage < 1)
                currentPage = 1;
            if(currentPage>countPages)
                currentPage= countPages;
            //Article = await _context.Articles.ToListAsync();
            var qr = (from p in _context.Articles
                     orderby p.Created descending
                     select p).Skip((currentPage-1)*ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE);
            if(!string.IsNullOrEmpty(SearchString))
            {
                Article = await qr.Where(p=>p.Title.Contains(SearchString)).ToListAsync();
            }
            else
            {
                Article = await qr.ToListAsync();
            }
            
        }
    }
}
