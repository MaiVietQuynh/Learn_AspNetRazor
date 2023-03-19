using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;
using ValidationExample.Models;
using ValidationExample.Validation;
using ValidationExample.Binders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ValidationExample.Pages
{
    public class ContactModel : PageModel
    {
		//      //[BindProperty]
		//[Required(ErrorMessage ="Phai nhap {0}")]
		//[StringLength(20,MinimumLength =3,ErrorMessage ="{0} phai dai tu {2} toi {1} ky tu ")]
		//[Display(Name = "Ten khach hang")]
		//[ModelBinder(BinderType = typeof(UserNameBinding))]
		//public string CustomerName { get; set; }
		//[BindProperty]
		//[Display(Name = "Dia chi Email")]
		//[EmailAddress(ErrorMessage ="Dia chi Email khong phu hop")]
		//[Required(ErrorMessage ="Phai nhap {0}")]
		//public string Email { get; set; }

		//[BindProperty]
		//[DisplayName("Nam sinh")]
		//[Required(ErrorMessage = "Phai nhap {0}")]
		//[Range(1970,2000,ErrorMessage ="{0} sai, phia nhap trong khoang tu {1} den {2}")]
		//[SoChan]
		//public int? YearOfBirth { get; set; }
		private readonly IWebHostEnvironment _environment;
		public ContactModel(IWebHostEnvironment eviroment)
		{
			_environment= eviroment;
		}
		public string thongbao { get; set; }
		[BindProperty]
		public CustomerInfor customerInfor { get; set; }
		[BindProperty]
		[DataType(DataType.Upload)]
		//[Required(ErrorMessage = "Chon file upload")]
		[DisplayName("File Upload")]
		[CheckFileExtensions(Extensions ="jpg,png,gif")]
		public IFormFile FileUpload { get; set; }
		[DisplayName("Nhieu File Upload")]
		public IFormFile[] FileUploads { get; set; }

		public void OnGet()
		{

		}
		public void OnPost()
        {
			if(ModelState.IsValid)
			{
				thongbao = "Du lieu phu hop";

				if(FileUpload != null)
				{
					var filePath = Path.Combine(_environment.WebRootPath, "uploads",FileUpload.FileName);
					using FileStream fileStream = new FileStream(filePath, FileMode.Create);
					FileUpload.CopyTo(fileStream);
				}
				foreach(IFormFile f in FileUploads)				
				{
					var filePath = Path.Combine(_environment.WebRootPath, "uploads", f.FileName);
					using FileStream fileStream = new(filePath, FileMode.Create);
					f.CopyTo(fileStream);
				}
			}
			else
			{
				thongbao = "Du lieu khong phu hop";
			}
        }
    }
}
