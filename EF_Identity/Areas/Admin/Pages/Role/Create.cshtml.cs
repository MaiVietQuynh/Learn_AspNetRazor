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
	public class CreateModel : RolePageModel
	{
		public CreateModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
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

		public void OnGet()
		{
		}
		public async Task<IActionResult> OnPostAsync()
		{
			if(!ModelState.IsValid)
			{
				return Page();
			}
			var newRole = new IdentityRole(Input.Name);
			var result = await _roleManager.CreateAsync(newRole);
			if(result.Succeeded)
			{
				StatusMessage = $"Ban vua tao Role moi: {Input.Name}";
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
