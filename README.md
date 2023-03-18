# Learn_AspNetRazor
## @page "url"
## Rewrite URL
      - Chuyen het url thanh chu thuong:
        + services.Configure<RouteOptions>(routeoptions =>
          {
                routeoptions.LowercaseUrls= true;
          });
## Tag Helper -> html
    - @addTagHelper
    - Nap va huy Tag Helper:
        + Nap: @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
        + Huy: @removeTagHelper Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper, Microsoft.AspNetCore.Mvc.TagHelpers
               Them dau ! truoc ten the
    - Xay dung TagHelper rieng:
        + Xay dung class ke thua tu tagHelper
        + Nap chong phuong thuc Process
## ViewData["myData"]
## Handler: 
    - Onget(), OnPost
## PageModel: 
     - Chia ra thanh file .cs .cshtml
     - .cs: ke thua tu PageModel
## ViewData["myData"]
## handler: 
    - Onget(), OnPost
## PageModel: 
 - Chia ra thanh file .cs .cshtml
 - .cs: ke thua tu PageModel
>>>>>>> 42c36bbdb74cc6bac24532481ba22f89ea05c947
## Layout:
    - Co dau _ de endpointrouting khong dung file do lam diem endpoint
    - Bo cuc chung cua trang(header, footer, menu,...)
    - /Pages/_MyLayout.cshtml 
    - /Pages/Shared/_MyLayout.cshtml 
    - /Views/Shared/_MyLayout.cshtml 
## section: 
    - @RenderSection
## Partial View: 
    - La file .cshtml khong co @page
    - Chia nho page thanh cac file nho
    - Su dung lai cac thanh phan(tranh trung lap code
    - /Pages/_ProductItem.cshtml 
    - /Pages/Shared/_ProductItem.cshtml 
    - /Views/Shared/_ProductItem.cshtml 
        + <partial name="_ProductItem" />
        + @await Html.PartialAsync("_ProductItem") 
        + @{
            await Html.RenderPartialAsync("_ProductItem") 
          }
## Component:
    - Component ~ Partial View, cos the dung Di de inject cac dich vu vao component ~ Mini razor page
    - (1):
        + [ViewComponent] or khai bao ten lop co hau to Component(ProductBoxViewComponent) or ke thua class ViewComponent(recomment)
        +(string or IViewComponentResult) Invoke or InvokeAsync
        +public IViewComponentResult Invoke(bool sapxeptang = true)     @await Component.InvokeAsync("ProductBox", false)
    -(2): DI in ViewComponent:
        +	private readonly ProductListService productService;
		    public ProductBox(ProductListService _products)
		    {
			    productService = _products;
		    }
    -(3): Chuyen huong trang voi ViewComponent
        + MesseagePage.cs
        + Index.cshtml.cs
