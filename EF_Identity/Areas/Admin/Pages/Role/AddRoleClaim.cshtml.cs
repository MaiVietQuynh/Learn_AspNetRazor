using EF_Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EF_Identity.Areas.Admin.Pages.Role
{
	public class AddRoleClaimModel : RolePageModel
	{
		public AddRoleClaimModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
		{
		}
		public class InputModel
		{
			[DisplayName("Ten cua Claim")]
			[Required(ErrorMessage = "Phai nhap {0}")]
			[StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phai dai tu {2} den {1} ky tu")]
			public string ClaimType { get; set; }
            [DisplayName("Gia tri cua Claim")]
            [Required(ErrorMessage = "Phai nhap {0}")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phai dai tu {2} den {1} ky tu")]
            public string ClaimValue { get; set; }

        }
		[BindProperty]
		public InputModel Input { get; set; }
		public IdentityRole role { get; set; }

		public async Task<IActionResult> OnGetAsync(string roleid)
		{
			role = await _roleManager.FindByIdAsync(roleid);
			if (role == null)
			{
				return NotFound("Khong tim thay Role");
			}
			return Page();

		}
		public async Task<IActionResult> OnPostAsync(string roleid)
		{
            role = await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("Khong tim thay Role");
            }
            if (!ModelState.IsValid)
			{
				return Page();
			}
			if((await _roleManager.GetClaimsAsync(role)).Any(c => c.Type == Input.ClaimType && c.Value == Input.ClaimValue))
			{
				ModelState.AddModelError(string.Empty, "Claim nay da co");
				return Page();
			}
			var newClaims = new Claim(Input.ClaimType,Input.ClaimValue);
			var result = await _roleManager.AddClaimAsync(role, newClaims);
			if(!result.Succeeded)
			{
				result.Errors.ToList().ForEach(error =>
				{
					ModelState.AddModelError(string.Empty, error.Description);
				});
                return Page();
            }
			StatusMessage = "Vua them Claim(dac tinh) moi";
			return RedirectToPage("./Edit", new {roleid=role.Id});
			
		}
	}
}
