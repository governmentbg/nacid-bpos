using Microsoft.AspNetCore.Http;
using OpenScience.Services.Logs;
using System;
using System.Threading.Tasks;

namespace Portal.Hosting.Infrastructure.Middlewares
{
	public class ErrorLoggingMiddleware
	{
		readonly RequestDelegate next;

		public ErrorLoggingMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task Invoke(HttpContext context, DbLoggerService logger)
		{
			try
			{
				await next(context);
			}
			catch (Exception exception)
			{
				while (exception.InnerException != null)
				{
					exception = exception.InnerException;
				}

				logger.LogException(exception, context.Request);
				await logger.WriteResponseAsync(next, context, exception);
			}
		}
	}
}
