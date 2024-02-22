using Microsoft.AspNetCore.Http;
using System.Linq;

namespace OpenScience.Services.Logs.Extensions
{
	public static class HttpRequestLogExtension
	{
		public static int? GetUserId(this HttpRequest request)
		{
			int? userId = null;

			try
			{
				if (request != null)
				{
					var user = request.HttpContext.User;

					if (user != null)
					{
						var claim = user.Claims.SingleOrDefault(c => c.Type.Equals("jti"));

						if (claim != null && int.TryParse(claim.Value, out int uId))
						{
							userId = uId;
						}
					}
				}
			}
			catch { }

			return userId;
		}
	}
}
