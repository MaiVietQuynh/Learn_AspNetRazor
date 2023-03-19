using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ValidationExample.Binders;
using ValidationExample.Validation;

namespace ValidationExample.Models
{
	public class CustomerInfor
	{

		//[BindProperty]
		[Required(ErrorMessage = "Phai nhap {0}")]
		[StringLength(20, MinimumLength = 3, ErrorMessage = "{0} phai dai tu {2} toi {1} ky tu ")]
		[Display(Name = "Ten khach hang")]
		[ModelBinder(BinderType = typeof(UserNameBinding))]
		public string CustomerName { get; set; }
		[BindProperty]
		[Display(Name = "Dia chi Email")]
		[EmailAddress(ErrorMessage = "Dia chi Email khong phu hop")]
		[Required(ErrorMessage = "Phai nhap {0}")]
		public string Email { get; set; }

		[BindProperty]
		[DisplayName("Nam sinh")]
		[Required(ErrorMessage = "Phai nhap {0}")]
		[Range(1970, 2000, ErrorMessage = "{0} sai, phia nhap trong khoang tu {1} den {2}")]
		[SoChan]
		public int? YearOfBirth { get; set; }

		public string thongbao { get; set; }
	}
}
