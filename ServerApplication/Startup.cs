using FilesStorageNetCore;
using FullTextSearch.Extensions;
using MetadataHarvesting.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenScience.Common.DomainValidation.Extensions;
using OpenScience.Handle.Extensions;
using ServerApplication.Infrastructure.Auth;
using ServerApplication.Infrastructure.Extensions;
using ServerApplication.Infrastructure.Middlewares;
using ServerApplication.MetadataProviderModule;

namespace ServerApplication
{
	public class Startup
	{
		private readonly IHostingEnvironment environment;

		public Startup(IHostingEnvironment environment)
		{
			this.environment = environment;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			var builder = services.AddMvcCore(options => {
				options.OutputFormatters.Add(new HttpNoContentOutputFormatter());
				options.Filters.Add(new ProducesAttribute("application/json"));
				options.Filters.Add(new AuthorizeFilter("default"));
			})
			.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
			.AddXmlSerializerFormatters()
			.AddXmlDataContractSerializerFormatters();

			builder.ConfigureJson();

			var configuration = services.ConfigureApplicationConfiguration(environment);

			AuthConfiguration authConfig = configuration.GetSection("AuthConfiguration").Get<AuthConfiguration>();
			services.ConfigureJwtAuthService(authConfig.SecretKey, authConfig.Issuer, authConfig.Audience);

			services.ConfigureDbContext(
				configuration.GetSection("DbConfiguration:ConnectionString").Value,
				configuration.GetSection("DbConfiguration:LogConnectionString").Value
			);

			services
				.AddMetadataProvider(configuration)
				.AddMetadataHarvestingServices(configuration.GetSection("DbConfiguration:HarvestedRecordsDatabaseConnectionString").Value);

			services.ConfigureDI();

			services.ConfigureHandleModule(handleConfig => handleConfig.WithAddress(configuration.GetSection("HandleConfiguration:HttpServerAddress").Value)
					.WithPrefix(configuration.GetSection("HandleConfiguration:Prefix").Value)
					.WithCredentials(
						int.Parse(configuration.GetSection("HandleConfiguration:AdminHandleIndex").Value),
						configuration.GetSection("HandleConfiguration:AdminHandle").Value,
						configuration.GetSection("HandleConfiguration:AdminCertPath").Value,
						configuration.GetSection("HandleConfiguration:AdminCertPass").Value
					)
				);

			services.ConfigureDomainValidationModule();

			services.ConfigureFullTextSearch(
				ocrConfig => ocrConfig.WithOcrServiceUrl(configuration.GetSection("FullTextConfiguration:OcrServiceUrl").Value),
				elasticConfig => elasticConfig.WithUrl(configuration.GetSection("FullTextConfiguration:ElasticsearchServiceUrl").Value)
			);

			builder.ConfigureAuthorization();

			var fileStorageModule = new FileStorageModule(services, c => configuration.GetSection("DbConfiguration:Descriptors").Bind(c));

			if (environment.IsProduction())
			{
				services.AddHttpsRedirection(options => {
					options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
					options.HttpsPort = 443;
				});
			}
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment environment)
		{
			if (environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app
				.UseMiddleware<RedirectionMiddleware>()
				.UseMiddleware<ErrorLoggingMiddleware>()
				;

			app.UseHttpsRedirection();

			app.UseDefaultFiles();

			app.UseStaticFiles(new StaticFileOptions {
				OnPrepareResponse = context => {
					if (context.File.Name == "index.html")
					{
						context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
						context.Context.Response.Headers.Add("Expires", "-1");
					}
				}
			});

			app.UseAuthentication();

			app.UseMvc();
		}
	}
}
