using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using OpenScience.Data;

namespace Portal.Hosting.Infrastructure.Extensions
{
	public static class ServicesDbContextConfigurationExtension
	{
		public static void ConfigureDbContext(this IServiceCollection services, string appConnectionString, string logConnectionString)
		{
			services
				.AddDbContext<AppDbContext>(options => options.UseNpgsql(appConnectionString)
					.EnableSensitiveDataLogging()
					.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning))
					.ConfigureWarnings(warnings => warnings.Throw(CoreEventId.IncludeIgnoredWarning))
				)
				.AddDbContext<AppLogContext>(options => options.UseNpgsql(logConnectionString));
		}
	}
}
