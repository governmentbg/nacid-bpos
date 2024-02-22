using Microsoft.Extensions.DependencyInjection;
using OpenScience.Handle.Configuration;
using System;
using System.Net.Http;

namespace OpenScience.Handle.Extensions
{
	public static class HandleModuleConfigurationExtension
	{
		public static void ConfigureHandleModule(this IServiceCollection services, Action<HandleServerConfig> handleConfig)
		{
			services
				.Configure(handleConfig)
				.AddSingleton<IHandleServerAdapter, HandleServerAdapter>();
			
			services.AddHttpClient("UntrustedClient")
				.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler {
				ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
			});
		}
	}
}
