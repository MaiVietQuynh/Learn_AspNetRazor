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
	public class EditRoleClaimModel : RolePageModel
	{
		public EditRoleClaimModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
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
		public IdentityRoleClaim<string> claim { get; set; }
		public async Task<IActionResult> OnGetAsync(int? claimid)
		{
			if (claimid == null)
			{
				return NotFound("Khong tim thay Role");
			}
			claim = _context.RoleClaims.Where(c=>c.Id == claimid).FirstOrDefault();
			if(claim == null)
			{
                return NotFound("Khong tim thay Role");
            }
			role = await _roleManager.FindByIdAsync(claim.RoleId);
            Input = new InputModel()
			{
				ClaimType = claim.ClaimType,
				ClaimValue = claim.ClaimValue
			};
			return Page();
		}
		public async Task<IActionResult> OnPostAsync(int? claimid)
		{
            if (claimid == null)
            {
                return NotFound("Khong tim thay Role");
            }
            claim = _context.RoleClaims.Where(c => c.Id == claimid).FirstOrDefault();
            if (claim == null)
            {
                return NotFound("Khong tim thay Role");
            }
            role = await _roleManager.FindByIdAsync(claim.RoleId);
            if (role == null)
            {
                return NotFound("Khong tim thay Role");
            }
            if (!ModelState.IsValid)
			{
				return Page();
			}

			if(_context.RoleClaims.Any(c => c.RoleId==role.Id && c.ClaimType == Input.ClaimType && c.ClaimValue == Input.ClaimValue && c.Id == claim.Id))
			{
				ModelState.AddModelError(string.Empty, "Claim nay da co");
				return Page();
			}
			claim.ClaimType= Input.ClaimType;
			claim.ClaimValue= Input.ClaimValue;
			await _context.SaveChangesAsync();
			StatusMessage = "Vua cap nhat Claim";
			return RedirectToPage("./Index", new {roleid=role.Id});
			
		}
        public async Task<IActionResult> OnPostDeleteAsync(int? claimid)
        {
            if (claimid == null)
            {
                return NotFound("Khong tim thay Role");
            }
            claim = _context.RoleClaims.Where(c => c.Id == claimid).FirstOrDefault();
            if (claim == null)
            {
                return NotFound("Khong tim thay Role");
            }
            role = await _roleManager.FindByIdAsync(claim.RoleId);
            if (role == null)
            {
                return NotFound("Khong tim thay Role");
            }
			await _roleManager.RemoveClaimAsync(role, new Claim(claim.ClaimType,claim.ClaimValue));
            StatusMessage = "Vua xoa Claim";
            return RedirectToPage("./Edit", new { roleid = role.Id });

        }
    }
}
