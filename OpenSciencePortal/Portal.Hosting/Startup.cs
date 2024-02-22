using FilesStorageNetCore;
using FullTextSearch.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenScience.Services.Base;
using OpenScience.Services.Logs;
using OpenScience.Services.Publications;
using Portal.Hosting.Infrastructure.Extensions;
using Portal.Hosting.Infrastructure.Middlewares;

namespace Portal.Hosting
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
			})
			.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
			;

			builder.ConfigureJson();

			var configuration = services.ConfigureApplicationConfiguration(environment);

			services.ConfigureDbContext(
				configuration.GetSection("DbConfiguration:ConnectionString").Value,
				configuration.GetSection("DbConfiguration:LogConnectionString").Value
			);

			services.ConfigureFullTextSearch(
				ocrConfig => ocrConfig.WithOcrServiceUrl(configuration.GetSection("FullTextConfiguration:OcrServiceUrl").Value),
				elasticConfig => elasticConfig.WithUrl(configuration.GetSection("FullTextConfiguration:ElasticsearchServiceUrl").Value)
			);

			services
				.AddScoped(typeof(INomenclatureService<>), typeof(BaseNomenclatureService<>))
				.AddScoped<PublicationService>()
				.AddScoped<DbLoggerService>();

			//new FileStorageModule(services, c => configuration.GetSection("DbConfiguration:Descriptors").Bind(c));


			//if (environment.IsProduction())
			//{
			//	services.AddHttpsRedirection(options => {
			//		options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
			//		options.HttpsPort = 443;
			//	});
			//}
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			//else
			//{
			//	app.UseHsts();
			//}

			app
				.UseMiddleware<RedirectionMiddleware>()
				.UseMiddleware<ErrorLoggingMiddleware>()
				;

			//app.UseHttpsRedirection();
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

			app.UseMvc();
		}
	}
}
