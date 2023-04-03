using EF_Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EF_Identity.Areas.Admin.Pages.Role
{
	public class DeleteModel : RolePageModel
	{
		public DeleteModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
		{
		}
		public IdentityRole role { get; set; }

		public async Task<IActionResult> OnGetAsync(string roleid)
		{
			if(roleid == null)
				return NotFound("Khong tim thay role");
			role = await _roleManager.FindByIdAsync(roleid);
			if(role==null)
			{
                return NotFound("Khong tim thay Role");
            }
			return Page();
			
		}
		public async Task<IActionResult> OnPostAsync(string roleid)
		{
            if (roleid == null)
            {
                return NotFound("Khong tim thay role");
            }
            role = await _roleManager.FindByIdAsync(roleid);
			if(role==null)
			{
                return NotFound("Khong tim thay role");
            }
		
			var result = await _roleManager.DeleteAsync(role);
			if(result.Succeeded)
			{
				StatusMessage = $"Ban vua xoa Role : {role.Name}";
				return RedirectToPage("./Index");
			}
			else
			{
				result.Errors.ToList().ForEach(error =>
				{
					ModelState.AddModelError(string.Empty, error.Description);
				});
			}
			return Page();
		}
	}
}
