using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NacidRas.Integrations.OaiPmhProvider.Contracts;
using NacidRas.Integrations.OaiPmhProvider.Converters;
using NacidRas.Integrations.OaiPmhProvider.Converters.DissertationToMetadataConverters;
using NacidRas.Integrations.OaiPmhProvider.Providers;
using NacidRas.Integrations.OaiPmhProvider.Repositories;

namespace NacidRas.Integrations.OaiPmhProvider
{
	public static class OaiPmhProviderDependencyInjectionConfiguration
	{
		public static void AddOaiPmhProvider(this IServiceCollection services, IConfiguration configuration)
		{
			services
				.AddSingleton<IDateConverter, DateConverter>()
				.AddSingleton<IResumptionTokenConverter, ResumptionTokenConverter>();

			services
				.AddSingleton<IMetadataEncoder, DublinCoreMetadataConverter>()
				.AddSingleton<IMetadataEncoder, OpenAireMetadataConverter>()
				.AddSingleton<Func<string, IMetadataEncoder>>(provider => prefix => provider.GetServices<IMetadataEncoder>().Single(p => p.Prefix == prefix));

			services
				.AddSingleton<IDissertationToMetadataConverter, DissertationToDublinCoreMetadataConverter>()
				.AddSingleton<IDissertationToMetadataConverter, DissertationToOpenAireMetadataConverter>()
				.AddSingleton<Func<string, IDissertationToMetadataConverter>>(provider => prefix => provider.GetServices<IDissertationToMetadataConverter>().Single(p => p.Prefix == prefix));

			services
				.AddScoped<DataProvider>()
				.AddScoped<IRecordRepository, DissertaionRecordRepository>()
				.AddScoped<IMetadataFormatRepository, MetadataFormatRepository>()
				.AddScoped<ISetRepository, ResearchAreaSetRepository>()
				.AddSingleton(OaiConfiguration.Instance);
		}
	}
}
