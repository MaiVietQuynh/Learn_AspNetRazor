using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EF_Identity.Models;
using Microsoft.AspNetCore.Authorization;

namespace EF_Identity.Pages_Blog
{
    public class EditModel : PageModel
    {
        private readonly EF_Identity.Models.MyBlogContext _context;
        private readonly IAuthorizationService _authorizationService;

        public EditModel(EF_Identity.Models.MyBlogContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public Article Article { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return Content("Khong thay bai viet");
            }

            Article = await _context.Articles.FirstOrDefaultAsync(m => m.Id == id);

            if (Article == null)
            {
                return Content("Khong thay bai viet");
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Article).State = EntityState.Modified;

            try
            {
                //Kiem tra quyen cap nhat
                var canUpdate= await _authorizationService.AuthorizeAsync(this.User,Article, "CanUpdateArticle");
                if (canUpdate.Succeeded)
                {
					await _context.SaveChangesAsync();
				}   
                else
                {
					return Content("Khong duoc quyen cap nhat");
				}
                   
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(Article.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}
