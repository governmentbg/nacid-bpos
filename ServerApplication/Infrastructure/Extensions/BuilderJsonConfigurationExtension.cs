using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ServerApplication.Infrastructure.Extensions
{
	public static class BuilderJsonExtension
	{
		public static void ConfigureJson(this IMvcCoreBuilder builder)
		{
			builder.AddJsonOptions(options => {
				options.SerializerSettings.ReferenceLoopHandling = JsonSerializerSettings.ReferenceLoopHandling;
			});
			builder.AddJsonFormatters(options => {
				options = JsonSerializerSettings;
			});
			JsonConvert.DefaultSettings = () => JsonSerializerSettings;
		}

		private static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings() {
#if DEBUG
			Formatting = Formatting.Indented,
#else
                    Formatting = Formatting.None,
#endif
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
			NullValueHandling = NullValueHandling.Include,
			DefaultValueHandling = DefaultValueHandling.Include,
			DateFormatHandling = DateFormatHandling.IsoDateFormat,
			ContractResolver = new CamelCasePropertyNamesContractResolver()
		};
	}
}
