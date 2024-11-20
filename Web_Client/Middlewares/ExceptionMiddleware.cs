using Microsoft.AspNetCore.Authentication;
using NuGet.Protocol;
using CRM_APIRequests.Exceptions;

namespace Web_Client.Middlewares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionMiddleware(RequestDelegate next) => _next = next;

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (SkillProfiUnauthorizedException)
			{

				await context.SignOutAsync();
				await context.ChallengeAsync(new AuthenticationProperties
				{
					RedirectUri = context.Request.Path,
				});
			}
		}
	}
}
