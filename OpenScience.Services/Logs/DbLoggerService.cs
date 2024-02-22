using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenScience.Common.DomainValidation.Enums;
using OpenScience.Common.DomainValidation.Models;
using OpenScience.Data;
using OpenScience.Data.Logs.Enums;
using OpenScience.Data.Logs.Models;
using OpenScience.Services.Logs.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenScience.Services.Logs
{
	public class DbLoggerService
	{
		private readonly AppLogContext logContext;

		public DbLoggerService(AppLogContext logContext)
		{
			this.logContext = logContext;
		}

		public void LogException(Exception exception, HttpRequest request = null)
		{
			var exceptionInfo = $"Type: {exception.GetType().FullName} \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}";

			Log(LogType.ExceptionLog, exceptionInfo, request);
		}

		public async Task WriteResponseAsync(RequestDelegate next, HttpContext context, Exception exception)
		{
			if (exception is DomainErrorException customException)
			{
				await StructureError(context, customException.ErrorMessages.ToList());
			}
			else if (exception is DbUpdateConcurrencyException)
			{
				var errorMessages = new List<DomainErrorMessage> {
					new DomainErrorMessage(SystemErrorCode.System_ConcurrencyException)
				};

				await StructureError(context, errorMessages);
			}
			else
			{
				await next(context);
			}
		}

		private void Log(LogType type, string message, HttpRequest request = null)
		{
			var log = new Log {
				Type = type,
				LogDate = DateTime.UtcNow,
				IP = request?.HttpContext.Connection.RemoteIpAddress.ToString(),
				Verb = request?.Method,
				Url = request?.GetDisplayUrl(),
				UserAgent = request?.Headers["User-Agent"].ToString(),
				UserId = request?.GetUserId(),
				Message = message
			};

			logContext.Logs.Add(log);
			logContext.SaveChanges();
		}

		private async Task StructureError(HttpContext context, List<DomainErrorMessage> errorMessages)
		{
			var responseMessage = new ResponseMessage {
				Messages = errorMessages,
				Status = "Error"
			};

			context.Response.StatusCode = 422;
			context.Response.ContentType = @"application/json";

			await context.Response.WriteAsync(JsonConvert.SerializeObject(responseMessage));
		}
	}
}
