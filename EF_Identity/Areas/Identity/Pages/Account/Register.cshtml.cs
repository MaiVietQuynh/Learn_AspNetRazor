using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using EF_Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace EF_Identity.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage ="Phai nhap {0}")]
            [EmailAddress(ErrorMessage ="Sai dinh dang Email")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0}phai dai tu {2} den {1} ky tu.", MinimumLength = 2)]
            [DataType(DataType.Password)]
            [Display(Name = "Mat Khau")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Nhap lai Mat Khau")]
            [Compare("Password", ErrorMessage = "Mat khau khong trung nhau")]
            public string ConfirmPassword { get; set; }
			[DataType(DataType.Text)]
			[Display(Name = "Ten tai khoan")]
			[Required(ErrorMessage = "Phai nhap {0}")]
			[StringLength(100, ErrorMessage = "{0}phai dai tu {2} den {1} ky tu.", MinimumLength = 3)]
			public string UserName { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            foreach(var provider in ExternalLogins)
            {
                _logger.LogInformation(provider.Name);
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = Input.UserName, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Da tao thanh cong User moi.");
                    //Phat sinh ra duong link de gui den nguoi dung bam vao xac nhan

                    //Phat sinh token de xac nhan email, khi mo email bam vao link thi se gui token den ung dung va ung dung biet de xac nhan
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //Duoc Endcode de dinh kem tren Url
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //Phat sinh Url de goi trang ConfirmEmail
                    //https://localhost:5001/confirm-email?userId=dffds&code=xyz&returnUrl=returnUrl
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);
                    //Gui email den User
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"ban da dang ky tai khoan tren RazorWeb, hay <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>bam vao day de kich hoat tai khoan</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        //isPersistent: thiet lap cookie de nho tai khoan neu bang true
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
