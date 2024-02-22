using Microsoft.Extensions.DependencyInjection;

namespace OpenScience.Common.DomainValidation.Extensions
{
	public static class DomainValidationServiceConfigurationExtension
	{
		public static void ConfigureDomainValidationModule(this IServiceCollection services)
		{
			services.AddScoped<DomainValidationService>();
		}
	}
}
