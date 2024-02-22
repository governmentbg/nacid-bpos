using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Portal.Hosting.Infrastructure.Extensions
{
	public static class ServicesApplicationConfigurationExtension
	{
		public static IConfiguration ConfigureApplicationConfiguration(this IServiceCollection services, IHostingEnvironment environment)
		{
			var configurationBuilder = new ConfigurationBuilder()
				.SetBasePath(environment.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

			IConfiguration configuration = configurationBuilder.Build();
			
			//services
			//	.Configure<HandleConfiguration>(config => configuration.GetSection("HandleConfiguration").Bind(config))
			//	.Configure<AuthConfiguration>(config => configuration.GetSection("AuthConfiguration").Bind(config))
			//	.Configure<EmailConfiguration>(config => configuration.GetSection("EmailConfiguration").Bind(config))
			//	.Configure<FullTextConfiguration>(config => configuration.GetSection("FullTextConfiguration").Bind(config))
			//;

			services.AddOptions();

			return configuration;
		}
	}
}
