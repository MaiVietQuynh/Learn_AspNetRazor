using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_Identity.Models
{
	public class Article
	{
		[Key]
		public int Id { get; set; }
		[StringLength(255,MinimumLength =5,ErrorMessage ="{0} phai dai tu {2} toi {1} ky tu")]
		[Required(ErrorMessage ="{0} phai nhap")]
		[Column(TypeName ="nvarchar")]
		[DisplayName("Tieu de")]
		public string Title { get; set; }
		[DataType(DataType.Date)]
		[Required(ErrorMessage = "{0} phai nhap")]
		[DisplayName("Ngay tao")]
		public DateTime Created { get; set; }
		[Column(TypeName ="ntext")]
		[DisplayName("Noi dung")]
		public string Content { get; set; }
	}
}
