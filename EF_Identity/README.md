# Entity Framework
	
## Tool
	- Add-Migration name
	- Update-Database
	- Drop-Database
## Tao du lieu mau co san
	- Su dung thu vien Bogus(Nuget)
## Tao cac trang CRUD
	-  dotnet aspnet-codegenerator razorpage -m EF_Identity.Models.Article -dc EF_Identity.Models.MyBlogContext -outDir Pages/Blog -udl --referenceScriptLibraries
	- Tao cac noi dung tu nhan biet Html: @Html.Raw(Model.Article.Content);