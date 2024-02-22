using MetadataHarvesting.Core.Extensions;
using MetadataProvider.Core;
using MetadataProvider.Core.Repositories;
using MetadataPublications.Converters.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServerApplication.MetadataProviderModule
{
	public static class MetadataProviderDependencyInjectionConfiguration
	{
		public static IServiceCollection AddMetadataProvider(this IServiceCollection services, IConfiguration configuration)
		{
			services
				.AddOaiPmhConfiguration(configuration)
				.AddScoped<MetadataProvider.Core.MetadataProvider>()
				.AddScoped<IMetadataFormatRepository, MetadataFormatRepository>()
				.AddScoped<ISetRepository, ClassificationSetRepository>()
				.AddScoped<IRecordRepository, PublicationRecordRepository>()
				.AddPmhConverters()
				.AddMetadataPublicationsConverters()
				.AddMetadataEncoders()
				.AddRepositoryMetadataHarvester()
				.AddMetadataParsers();

			services
				.Configure<MetadataHarvestingConfiguration>(
					config => configuration.GetSection(typeof(MetadataHarvestingConfiguration).Name).Bind(config));

			return services;
		}
	}
}
