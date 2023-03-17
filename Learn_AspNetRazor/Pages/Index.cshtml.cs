using Learn_AspNetRazor.Pages.Shared.Components.MessagePage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Learn_AspNetRazor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public IActionResult OnPost()
        {
            var username = this.Request.Form["username"];
            var messeage = new MessagePage.Message();
            messeage.title = "Noi dung Thong bao";
            messeage.htmlcontent = $"Cam on {username} da gui thong bao";
            messeage.secondwait = 4;
            messeage.urlredirect = Url.Page("Privacy");
            return ViewComponent("MessagePage",messeage);
        }
        //public IActionResult OnGet()
        //{
        //    // return Partial("_Message");
        //    return ViewComponent("ProductBox");
        //}
    }
}
