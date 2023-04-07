using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EF_Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EF_Identity.Areas.Admin.Pages.User
{
    public class AddRoleModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MyBlogContext _context;
        public AddRoleModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            MyBlogContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }
        public AppUser user { get; set; }
        [BindProperty]
        [DisplayName("Cac role gan cho User")]
        public string[] RoleNames { get; set; }
        public SelectList allRoles { get; set; }
        public List<IdentityRoleClaim<string>> claimsInRole { get; set; }
        public List<IdentityUserClaim<string>> claimsInUserClaim { get; set; }
        async Task GetClaims(string id)
        {
            var listRoles = from r in _context.Roles
                            join ur in _context.UserRoles on r.Id equals ur.RoleId
                            where ur.UserId == id
                            select r;
            var _claimsInRole = from c in _context.RoleClaims
                                join r in listRoles on c.RoleId equals r.Id
                                select c;
            claimsInRole = await _claimsInRole.ToListAsync();

            claimsInUserClaim = await (from c in _context.UserClaims
                                       where c.UserId == id
                                       select c).ToListAsync();
        }
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

            await GetClaims(id);

            RoleNames = (await _userManager.GetRolesAsync(user)).ToArray<string>();
            List<string> roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            allRoles = new SelectList(roleNames);

            await GetClaims(id);

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
            var OldRoleNames = (await _userManager.GetRolesAsync(user)).ToArray();
            var deleteRoles = OldRoleNames.Where(r => !RoleNames.Contains(r));
            var addRoles = RoleNames.Where(r => !OldRoleNames.Contains(r));
            var resultDelete = await _userManager.RemoveFromRolesAsync(user, deleteRoles);

            List<string> roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            allRoles = new SelectList(roleNames);

            if (!resultDelete.Succeeded)
            {
                resultDelete.Errors.ToList().ForEach(er =>
                {
                    ModelState.AddModelError(string.Empty, er.Description);
                });
                return Page();
            }
            var resultAdd = await _userManager.AddToRolesAsync(user, addRoles);
            if (!resultAdd.Succeeded)
            {
                resultAdd.Errors.ToList().ForEach(er =>
                {
                    ModelState.AddModelError(string.Empty, er.Description);
                });
                return Page();
            }
            StatusMessage = $"Ban vua cap nhat role cho User:{user.UserName}";

            return RedirectToPage("./Index");
        }
    }
}
