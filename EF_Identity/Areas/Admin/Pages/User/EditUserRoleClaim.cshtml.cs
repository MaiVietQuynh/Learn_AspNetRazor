using EF_Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;

namespace EF_Identity.Areas.Admin.Pages.User
{
    public class EditUserRoleClaimModel : PageModel
    {
        private readonly MyBlogContext _context;
        private readonly UserManager<AppUser> _userManager;
        public EditUserRoleClaimModel(MyBlogContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [TempData]
        public string StatusMessage { get; set; }
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
		public AppUser user { get; set; }
		public IdentityUserClaim<string> userclaim { get; set; }
		public NotFoundObjectResult OnGet() => NotFound("Khong duoc truy cap");
		public async Task<IActionResult> OnGetAddClaimAsync(string userid)
		{
			user = await _userManager.FindByIdAsync(userid);
			if(user == null)
			{
				return NotFound("Khong tim thay user");
			}
			return Page();
		}
		public async Task<IActionResult> OnPostAddClaimAsync(string userid)
		{
			user = await _userManager.FindByIdAsync(userid);
			if (user == null)
			{
				return NotFound("Khong tim thay user");
			}
			if (!ModelState.IsValid)
			{
				return Page();
			}
			var claim = _context.UserClaims.Where(c => c.UserId == userid);
			if (claim.Any(c => c.ClaimType == Input.ClaimType && c.ClaimValue == Input.ClaimValue))
			{
				ModelState.AddModelError(string.Empty, "Dac tinh nay da co");
				return Page();
			}
			await _userManager.AddClaimAsync(user, new Claim(Input.ClaimType, Input.ClaimValue));
			StatusMessage = "Da them dac tinh cho User";
			return RedirectToPage("./AddRole", new { id = userid });
		}
		
		public async Task<IActionResult> OnGetEditClaimAsync(int? claimid)
		{
			if(claimid == null) return NotFound("Khong tim thay user");
			userclaim = _context.UserClaims.Where(c => c.Id == claimid).FirstOrDefault();
			user = await _userManager.FindByIdAsync(userclaim.UserId);
			if (user == null)
			{
				return NotFound("Khong tim thay user");
			}
			Input = new InputModel()
			{
				ClaimType = userclaim.ClaimType,
				ClaimValue = userclaim.ClaimValue
			};
			return Page();
		}
		public async Task<IActionResult> OnPostEditClaimAsync(int? claimid)
		{
			if (claimid == null) return NotFound("Khong tim thay user");
			userclaim = _context.UserClaims.Where(c => c.Id == claimid).FirstOrDefault();
			user = await _userManager.FindByIdAsync(userclaim.UserId);
			if (user == null)
			{
				return NotFound("Khong tim thay user");
			}
			if (!ModelState.IsValid) return Page();
			if(_context.UserClaims.Any(c=>c.UserId==user.Id 
									&& c.ClaimType==Input.ClaimType 
									&& c.ClaimValue==Input.ClaimValue 
									&& c.Id != userclaim.Id))
			{
				ModelState.AddModelError(string.Empty, "Claim nay da co");
				return Page();
			}

			userclaim.ClaimType = Input.ClaimType;
			userclaim.ClaimValue = Input.ClaimValue;
			await _context.SaveChangesAsync();
			StatusMessage = "Ban vua cap nhat Claim";
			return RedirectToPage("./AddRole", new { id = user.Id });
		}
		public async Task<IActionResult> OnPostDeleteAsync(int? claimid)
		{
			if (claimid == null) return NotFound("Khong tim thay user");
			userclaim = _context.UserClaims.Where(c => c.Id == claimid).FirstOrDefault();
			user = await _userManager.FindByIdAsync(userclaim.UserId);
			if (user == null)
			{
				return NotFound("Khong tim thay user");
			}
			await _userManager.RemoveClaimAsync(user, new Claim(userclaim.ClaimType, userclaim.ClaimValue));
			StatusMessage = "Ban vua xoa Claim";
			
			return RedirectToPage("./AddRole", new { id = user.Id });
		}
	}
}
