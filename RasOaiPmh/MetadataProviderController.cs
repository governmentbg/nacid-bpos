using System;
using Microsoft.AspNetCore.Mvc;
using NacidRas.Integrations.OaiPmhProvider.Models;
using NacidRas.Integrations.OaiPmhProvider.Providers;

namespace NacidRas.Integrations.OaiPmhProvider
{
	[ApiController]
	[Route("oai/request")]
	public class MetadataProviderController : ControllerBase
	{
		private readonly DataProvider dataProvider;

		public MetadataProviderController(DataProvider dataProvider)
		{
			this.dataProvider = dataProvider;
		}

		[HttpGet("")]
		[Produces("application/xml")]
		public ContentResult Get([FromQuery] ArgumentContainer arguments) => ExecuteRequest(arguments);

		[HttpPost("")]
		[Produces("application/xml")]
		[Consumes("application/x-www-form-urlencoded")]
		public ContentResult Post([FromForm] ArgumentContainer arguments) => ExecuteRequest(arguments);

		private ContentResult ExecuteRequest(ArgumentContainer arguments)
		{
			var date = DateTime.UtcNow;

			var document = dataProvider.ToString(date, arguments);

			return new ContentResult {
				Content = document,
				ContentType = "application/xml",
				StatusCode = 200
			};
		}
	}
}
