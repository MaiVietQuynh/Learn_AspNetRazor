using EF_Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EF_Identity.Areas.Admin.Pages.Role
{
	[Authorize(Policy ="AllowEditRole")]
	public class EditModel : RolePageModel
	{
		public EditModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
		{
		}
		public class InputModel
		{
			[DisplayName("Ten cua Role")]
			[Required(ErrorMessage = "Phai nhap {0}")]
			[StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phai dai tu {2} den {1} ky tu")]
			public string Name { get; set; }

		}
		[BindProperty]
		public InputModel Input { get; set; }
		public IdentityRole role { get; set; }
		public List<IdentityRoleClaim<string>> Claims { get; set; }

		public async Task<IActionResult> OnGetAsync(string roleid)
		{
			if(roleid == null)
				return NotFound("Khong tim thay role");
			role = await _roleManager.FindByIdAsync(roleid);
			if(role!=null)
			{
				Input = new InputModel
				{
					Name = role.Name
				};
				Claims = _context.RoleClaims.Where(rc => rc.RoleId == role.Id).ToList();
				return Page();
			}
			return NotFound("Khong tim thay Role");
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
			Claims = _context.RoleClaims.Where(rc => rc.RoleId == role.Id).ToList();
			if (!ModelState.IsValid)
			{
				return Page();
			}
			role.Name = Input.Name;
			var result = await _roleManager.UpdateAsync(role);
			if(result.Succeeded)
			{
				StatusMessage = $"Ban vua doi ten Role : {Input.Name}";
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
