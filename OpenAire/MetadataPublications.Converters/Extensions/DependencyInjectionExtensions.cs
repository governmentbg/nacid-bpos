using System;
using System.Linq;
using MetadataHarvesting.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MetadataPublications.Converters.Extensions
{
	public static class DependencyInjectionExtensions
	{
		public static IServiceCollection AddMetadataPublicationsConverters(this IServiceCollection services)
		{
			services
				.AddScoped<IPublicationConverter, DublinCorePublicationConverter>()
				.AddScoped<IPublicationConverter, OpenAirePublicationConverter>()
				.AddScoped<Func<string, IPublicationConverter>>(provider => metadataFormat =>
					provider.GetServices<IPublicationConverter>().Single(c => c.Prefix == metadataFormat));

			services
				.AddScoped<SetSpecClassificationService>()
				.AddTransient<SetSpecService>();

			return services;
		}
	}
}
