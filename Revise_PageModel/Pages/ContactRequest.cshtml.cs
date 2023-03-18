using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Revise_PageModel.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Revise_PageModel.Pages
{
    public class ContactRequestModel : PageModel
    {
        [BindProperty]
        [DisplayName("Nhap ten cua ban: ")]
        [Range(10, 100, ErrorMessage = "Nhap sai")]
        public int UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

		private readonly ILogger<ContactRequestModel> _logger;
       
        public ContactRequestModel(ILogger<ContactRequestModel> logger)
        {
            _logger= logger;
            _logger.LogInformation("Init Contact..");
        }
        public void OnPost()
        {
            Console.WriteLine(this.UserId.ToString());
        }
    }
}
