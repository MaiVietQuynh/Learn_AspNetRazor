using EF_Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EF_Identity.Security.Requirements
{
	public class AppAuthorizationhandler : IAuthorizationHandler
	{
		private readonly ILogger _logger;
		private readonly UserManager<AppUser> _userManager;
		public AppAuthorizationhandler(ILogger<AppAuthorizationhandler> logger, UserManager<AppUser> userManager)
		{
			_logger= logger;
			_userManager= userManager;
		}
		public Task HandleAsync(AuthorizationHandlerContext context)
		{
			var requirements = context.PendingRequirements.ToList();
			_logger.LogInformation("context resource ~" + context.Resource?.GetType().Name);
			foreach(var requirement in requirements )
			{
				if(requirement is GenZRequirement)
				{
					if (IsGenZ(context.User, (GenZRequirement)requirement))
						context.Succeed(requirement);
					//Code xu ly kiem tra User dam bao Requirement/GenZRequirement
				}
				if(requirement is ArticleUpdateRequirement)
				{
					if (CanUpdateRequirement(context.User, context.Resource, (ArticleUpdateRequirement)requirement))
						context.Succeed(requirement);
				}
			}
			return Task.CompletedTask;
		}

		private bool CanUpdateRequirement(ClaimsPrincipal user, object resource, ArticleUpdateRequirement requirement)
		{
			if (user.IsInRole("Admin"))
			{
				_logger.LogInformation("Admin cap nhat...");
				return true;
			}
			var article = resource as Article;
			var dateCreate = article.Created;
			var dateCanUpdate = new DateTime(requirement.Year,requirement.Month,requirement.Day);
			if(dateCreate < dateCanUpdate)
			{
				_logger.LogInformation("Qua ngay cap nhat");
				return false;
			}
			return true;
		}

		private bool IsGenZ(ClaimsPrincipal user, GenZRequirement requirement)
		{
			var appUserTask = _userManager.GetUserAsync(user);
			Task.WaitAll(appUserTask);
			var appUser = appUserTask.Result;
			if (appUser.Birthdate == null)
			{
				_logger.LogInformation($"{appUser.UserName} khong co ngay sinh, khong thoa man GenZRequirement");
				return false;
			}
			int year = appUser.Birthdate.Value.Year;
			bool success = (year >= requirement.FromYear && year <= requirement.ToYear);
			if(success)
			{
				_logger.LogInformation($"{appUser.UserName} thoa man GenZRequirement");
			}
			else
			{
				_logger.LogInformation($"{appUser.UserName} khong thoa man GenZRequirement");
			}
			return success;
		
		}
	}
}
