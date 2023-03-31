using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
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
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            // Lay duoc provider tu Login submit
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
			// Lay doi tuong kieu AuthenticationProperties(chua ten dich vu, client id, client secret,....)
			var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
			//Su dung thu vien tuong ung voi provider de ket noi voi dich vu ngoai, tra ve ChallengeResult,
            //noi dung nay duoc render tren trinh duyet va pop up len 1 cua so de nguoi dung ket noi den dich vu ngoai va cho phep ung dung truy cap den thong tin cua ho,
            //khi nguoi dung dong y truy cap thi dich vu ngoai se gui ma token den ung dung cua chung ta thong qua dia chi callBack, tren dia chi nay thu vien tuong ung voi provider se lay duoc ma token va tu dong truy cap thong tin ket noi toi tai khoan user,
            //khi ket noi va lay duoc thong tin user thi se tu dong chuyen huong ve trang ma ta thiet lap o redirectUrl, thong tin lay duoc luu vvao section cua ung dung, luc nay o cac trang truy cap khac co the doc lai duoc thong tin nay
			return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
           
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Loi tu dich vu ngoai: {remoteError}";
                return RedirectToPage("./Login", new {ReturnUrl = returnUrl });
            }
            // info chua thong tin nguoi dung tu dich vu ngoai
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Khong lay duoc thong tin tu dich vu ngoai.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor : true);
            if (result.Succeeded)
            {
                // Dang nhap thanh cong khi ung dung da co mot account(duoc lien ket voi dich vu ngoai la LoginProvider(vd google))
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // Co tai khoan nhung chua lien ket voi dich vu ngoai Google -> Lien ket dich vu ngoai
                // Chua co tai khoan -> Tao tai khoan, lien ket dich vu ngoai, dang nhap
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Loi lay thong tin tu dich vu ngoai.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {

                var registeredUser = await _userManager.FindByEmailAsync(Input.Email);
                //var info = await _signInManager.GetExternalLoginInfoAsync();
                string externalEmail = null;
                AppUser externalEmailUser = null;
                //Kiem tra info co cung cap dia chi Email khong
                // Tim trong Principal xem co cac claim co claim type bang email khong
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    externalEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
                }
                if (externalEmail != null)
                {
                    externalEmailUser = await _userManager.FindByEmailAsync(externalEmail);
                }
                if ((registeredUser != null) && (externalEmailUser != null))
                {
                    // Input.Email==externalEmail
                    if (registeredUser.Id == externalEmailUser.Id)
                    {
                        // Lien ket tai khoan -> dang nhap
                        var resultLink = await _userManager.AddLoginAsync(registeredUser, info);
                        if (resultLink.Succeeded)
                        {
                            await _signInManager.SignInAsync(registeredUser, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }
                    else
                    {
                        // Input.Email!=externalEmail
                        /*
                         * info -> user1(email1@gmail.com)
                         *      -> user2(email2@gmail.com)
                         */
                        ModelState.AddModelError(string.Empty, "Khong lien ket duoc tai khoan, hay su dung email khac");
                        return Page();
                    }

                }
                if(externalEmailUser !=null && registeredUser==null)
                {
                    ModelState.AddModelError(string.Empty, "Khong ho tro tao tai khoan co email khac voi email tao tu dich vu ngoai");
                    return Page();
                }
                if(externalEmailUser==null && externalEmail==Input.Email)
                {
                    // chua co account
                    var newUser = new AppUser
                    {
                        Email = externalEmail,
                        UserName = externalEmail,
                    };
                    var resultNewUser = await _userManager.CreateAsync(newUser);
                    if(resultNewUser.Succeeded)
                    {
                        await _userManager.AddLoginAsync(newUser, info);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                        await _userManager.ConfirmEmailAsync(newUser, code);
                        await _signInManager.SignInAsync(newUser,isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Khong tao duoc tai khoan moi");
                        return Page();
                    }
                }

                var user = new AppUser { UserName = Input.Email, Email = Input.Email };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);

                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
        }
    }
}
