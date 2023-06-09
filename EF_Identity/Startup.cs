﻿using EF_Identity.Models;
using EF_Identity.Security.Requirements;
using EF_Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EF_Identity
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddOptions();
			var mailsetting = Configuration.GetSection("MailSettings");
			services.Configure<MailSettings>(mailsetting);

			services.AddSingleton<IEmailSender,SendMailService>();

			services.AddDbContext<MyBlogContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("MyBlogContext"));
			});
			services.AddRazorPages();

			//Dang ky Identity
			services.AddIdentity<AppUser, IdentityRole>()
				.AddEntityFrameworkStores<MyBlogContext>()
				.AddDefaultTokenProviders();
			//services.AddDefaultIdentity<AppUser>()
			//    .AddEntityFrameworkStores<MyBlogContext>()
			//    .AddDefaultTokenProviders();
			// Truy cập IdentityOptions
			services.Configure<IdentityOptions>(options => {
                // Thiết lập về Password
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 3 lần thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;  // Email là duy nhất

                // Cấu hình đăng nhập.
                options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại

				options.SignIn.RequireConfirmedAccount= true;
            });

			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/login/";
				options.LoginPath = "/logout/";
				options.AccessDeniedPath = "/khongduoctruycap.html";
			});

			services.AddAuthentication()
				.AddGoogle(googleOption =>
				{
                    IConfigurationSection googleConfig = Configuration.GetSection("Authentication:Google");
                    googleOption.ClientId = googleConfig["ClientId"];
                    googleOption.ClientSecret = googleConfig["ClientSecret"];
                    googleOption.CallbackPath = "/dang-nhap-tu-google";
					//CallbackPath mac dinh : https://localhost:5001/signin-google
				})
				.AddFacebook(facebookOptions =>
				{
                    IConfigurationSection facebookConfig = Configuration.GetSection("Authentication:Facebook");
                    facebookOptions.AppId = facebookConfig["AppId"];
                    facebookOptions.AppSecret = facebookConfig["AppSecret"];
                    facebookOptions.CallbackPath = "/dang-nhap-tu-facebook";
					
				});
			services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();
			services.AddAuthorization(options =>
			{
				options.AddPolicy("AllowEditRole", policyBuilder =>
				{
					policyBuilder.RequireAuthenticatedUser();
					policyBuilder.RequireRole("Admin");
                    //policyBuilder.RequireRole("Editor");
                    //policyBuilder.RequireClaim("manager.role", "add", "update");
                    policyBuilder.RequireClaim("canedit", "User");
                });
				options.AddPolicy("InGenZ", policyBuilder =>
				{
					policyBuilder.Requirements.Add(new GenZRequirement());
				});
				options.AddPolicy("ShowAdminMenu", policyBuilder =>
				{
					policyBuilder.RequireRole("Admin");
				});
				options.AddPolicy("CanUpdateArticle", policyBuilder =>
				{
					policyBuilder.Requirements.Add(new ArticleUpdateRequirement());
				});
			});
			services.AddTransient<IAuthorizationHandler, AppAuthorizationhandler>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
			});
		}
	}
}
