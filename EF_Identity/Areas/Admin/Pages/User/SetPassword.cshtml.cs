using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EF_Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EF_Identity.Areas.Admin.Pages.User
{
    public class SetPasswordModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public SetPasswordModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage ="Phai nhap{0}")]
            [StringLength(100, ErrorMessage = "{0} phai dai tu {2} den {1} ky tu.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mat khau moi")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Xac nhan mat khau")]
            [Compare("NewPassword", ErrorMessage = "Lap lai mat khau khong chinh xac.")]
            public string ConfirmPassword { get; set; }
            
        }
        public AppUser user { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return NotFound($"Khong co User");
            }
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Khong thay User co Id={id}");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
			if (string.IsNullOrEmpty(id))
			{
				return NotFound($"Khong co User");
			}
			user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound($"Khong thay User co Id={id}");
			}
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _userManager.RemovePasswordAsync(user);
            var addPasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            
            StatusMessage = $"Ban vua cap nhat mat khau cho User:{user.UserName}";

            return RedirectToPage("./Index");
        }
    }
}
