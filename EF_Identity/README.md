﻿# Entity Framework
	
## Tool
	- Add-Migration name
	- Update-Database
	- Drop-Database
## Tao du lieu mau co san
	- Su dung thu vien Bogus(Nuget)
## Tao cac trang CRUD
	-  dotnet aspnet-codegenerator razorpage -m EF_Identity.Models.Article -dc EF_Identity.Models.MyBlogContext -outDir Pages/Blog -udl --referenceScriptLibraries
	- Tao cac noi dung tu nhan biet Html: @Html.Raw(Model.Article.Content);

# Identity

## Cac package
	- dotnet add package System.Data.SqlClient
	- dotnet add package Microsoft.EntityFrameworkCore
	- dotnet add package Microsoft.EntityFrameworkCore.SqlServer
	- dotnet add package Microsoft.EntityFrameworkCore.Design
	- dotnet add package Microsoft.Extensions.DependencyInjection
	- dotnet add package Microsoft.Extensions.Logging.Console

	- dotnet add package Microsoft.AspNetCore.Identity
	- dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
	- dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
	- dotnet add package Microsoft.AspNetCore.Identity.UI
	- dotnet add package Microsoft.AspNetCore.Authentication
	- dotnet add package Microsoft.AspNetCore.Http.Abstractions
	- dotnet add package Microsoft.AspNetCore.Authentication.Cookies
	- dotnet add package Microsoft.AspNetCore.Authentication.Facebook
	- dotnet add package Microsoft.AspNetCore.Authentication.Google
	- dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
	- dotnet add package Microsoft.AspNetCore.Authentication.MicrosoftAccount
	- dotnet add package Microsoft.AspNetCore.Authentication.oAuth
	- dotnet add package Microsoft.AspNetCore.Authentication.OpenIDConnect
	- dotnet add package Microsoft.AspNetCore.Authentication.Twitter
## Identity

### Tong quan
	- Authentication: Xac dinh danh tinh -> Login,Logout,...
	- Authorization: Xac thuc quyen truy cap
	- Quan ly User: Sign Up, Role, User,...
	- Dang ky Identity:
		+ services.AddIdentity<AppUser,IdentityRole>()
					.AddEntityFrameworkStores<MyBlogContext>()
					.AddDefaultTokenProviders();
	- Bo tien to AspNet dau ten bang:
		+ 	foreach(var entityType in modelBuilder.Model.GetEntityTypes())
			{
				var tableName = entityType.GetTableName();
				if(tableName.StartsWith("AspNet"))
				{
					entityType.SetTableName(tableName.Substring(6));
				}
			}
	- Truy cập IdentityOptions
		+ services.Configure<IdentityOptions> (options => {
				// Thiết lập về Password
				options.Password.RequireDigit = false; // Không bắt phải có số
				options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
				options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
				options.Password.RequireUppercase = false; // Không bắt buộc chữ in
				options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
				options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

				// Cấu hình Lockout - khóa user
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Khóa 5 phút
				options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
				options.Lockout.AllowedForNewUsers = true;

				// Cấu hình về User.
				options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
					"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
				options.User.RequireUniqueEmail = true;  // Email là duy nhất

				// Cấu hình đăng nhập.
				options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
				options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại

		 });
	- Trang mac dinh cua Identity:
		+ Identity/Account/Login
		+ Identity/Account/Register
	- Inject cac dich vu cua Identity vao View:
			+ @using Microsoft.AspNetCore.Identity;
			+ @using EF_Identity.Models;
			+ @inject SignInManager<AppUser> SignInManager
			+ @inject UserManager<AppUser> UserManager
	- Trong Controller, PageModel, View,.. co san property la User(co kieu ClaimsPrincipal) chua thong tin cua User
	- Thuoc tinh User nay duoc thiet lap trong moi truy van do 2 middleware la Authorization, Authentication;

### Register
	- Muon truy cap phai dang nhap
		+ [Authorize]
		+ 	services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/login/";
				options.LoginPath = "/logout/";
				options.AccessDeniedPath = "/khongduoctruycap.html";
			});
		+ Xem Register.cshtml.cs

### Login Logout Reset Passwork Lock Account
	- Co 2 phuong thuc dang nhap:
		+ _signInManager.SignInAsync
		+ _signInManager.PasswordSignInAsync
	- Dang nhap sai qua nhieu bi khoa:
		+ _signInManager.PasswordSignInAsync: lockoutOnFailure: true


### Google Facebook Login
	- Đăng ký dịch vụ xác thực từ Google - OAuth của Google: https://console.developers.google.com/
	- Cau hinh appsettings.json:
		+   "Authentication": {
				"Google": {
				  "ClientId": "891070304210-vtnv61d8v1ot977bk441gf0ivvfc68c6.apps.googleusercontent.com",
				  "ClientSecret": "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"
				}
		}
	- Dang ky vao startup:
		+ services.AddAuthentication()
				.AddGoogle(option =>
				{
					var googleConfig = Configuration.GetSection("Authentication:Google");
					option.ClientId = googleConfig["ClientId"];
					option.ClientSecret = googleConfig["ClientSecret"];
					option.CallbackPath= "/dang-nhap-tu-google";
					//CallbackPath mac dinh : https://localhost:5001/signin-google
				});
	- Class Register.cshtml.cs:
		+ De lay cac provider(la cac dich vu ngoai), ta goi: _signInManager.GetExternalAuthenticationSchemesAsync() ->  Tra ve tap hop AuthenticationScheme(Chua thong tin ve provider)
	- Xem Class Login->ExternalLogin

### Role

#### Role-based authorization
	- Role-based authorization
		+ Role(Vai tro): Admin, Editor,....
	- Tuy bien thong bao loi Identity
		+ Trong RoleManager co inject IdentityErrorDescriber, implement nó để tùy biến
		+ Dang ky AppIdentityErrorDescriber vao Starup
#### Police-based authorization
	- Dang ky trong Starup
#### Claims-based authorization
	- IdentityRoleClaim<string> claim1 -> DbContext
	- IdentityUserClaim<string> claim2 -> DbContext
	- Claim claim3 -> Dich vu Identity
#### IAuthorizationService
	- Tao Requirements
		+ Tao 1 class ke thua IAuthorizationRequirement
	- Khi he thong kiem tra User theo Requirement thi phai tim tren he thong 1 dich vu nao do chuyen xu ly nhung doi tuong Requirement do
	- Tao dich vu xu ly Requirements
		+  IAuthorization handler -> Tao 1 class implement tu IAuthorizationHandler
		+ Dang ky dich vu vao he thong
		+ Thong tin chuyen den AuthorizationHandler duoc chua trong context
		+ context.PendingRequirements: Cac requirements chua xu ly
		+ context.Resource: La mot doi tuong bat ky nao do, trong cach xac thuc su dung Authorize thi Resource la HttpContextDefault