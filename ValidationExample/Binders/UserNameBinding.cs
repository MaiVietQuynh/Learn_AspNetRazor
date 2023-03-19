using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace ValidationExample.Binders
{
	/*
	 * Chuyen ten thanh In hoa
	 * Ten khong duoc chua xxx
	 * Cat khoang trang o dau va cuoi
	 */
	public class UserNameBinding : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if(bindingContext == null)
			{
				throw new ArgumentNullException(nameof(bindingContext));
			}
			string modelName = bindingContext.ModelName;
			//Doc gia tri gui den
			var valueProviderResult =bindingContext.ValueProvider.GetValue(modelName);
			if(valueProviderResult==ValueProviderResult.None)
			{
				return Task.CompletedTask;
			}
			string value = valueProviderResult.FirstValue;
			if(value == null)
			{
				return Task.CompletedTask;
			}
			//Binding
			string s = value.ToUpper();
			if(s.Contains("XXX"))
			{
				bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);
				bindingContext.ModelState.TryAddModelError(modelName, "Loi do chua xxx");
				return Task.CompletedTask;
			}
			s=s.Trim();
			bindingContext.ModelState.SetModelValue(modelName, s, s);
			bindingContext.Result = ModelBindingResult.Success(s);
			return Task.CompletedTask;
		}
	}
}
