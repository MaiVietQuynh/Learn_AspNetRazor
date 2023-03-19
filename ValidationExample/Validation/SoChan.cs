using System;
using System.ComponentModel.DataAnnotations;

namespace ValidationExample.Validation
{
	public class SoChan : ValidationAttribute
	{
		public SoChan() 
		{
			ErrorMessage = "{0} phai la so chan";
		}
		public override bool IsValid(object value)
		{
			if(value == null) { return false; }
			int i = Convert.ToInt32(value);
			return i % 2==0;
		}
	}
}
