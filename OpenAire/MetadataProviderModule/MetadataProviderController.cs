using System;
using System.Threading.Tasks;
using MetadataHarvesting.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServerApplication.MetadataProviderModule
{
	[ApiController]
	[AllowAnonymous]
	[Route("oai/request")]
	public class MetadataProviderController : ControllerBase
	{
		private readonly MetadataProvider.Core.MetadataProvider dataProvider;

		public MetadataProviderController(MetadataProvider.Core.MetadataProvider dataProvider)
		{
			this.dataProvider = dataProvider;
		}

		[HttpGet("")]
		[Produces("application/xml")]
		public async Task<ContentResult> Get([FromQuery] ArgumentContainer arguments) => await ExecuteRequest(arguments);

		[HttpPost("")]
		[Produces("application/xml")]
		[Consumes("application/x-www-form-urlencoded")]
		public async Task<ContentResult> Post([FromForm] ArgumentContainer arguments) => await ExecuteRequest(arguments);

		private async Task<ContentResult> ExecuteRequest(ArgumentContainer arguments)
		{
			var date = DateTime.UtcNow;

			var document = await dataProvider.ToString(date, arguments);

			return new ContentResult {
				Content = document,
				ContentType = "application/xml",
				StatusCode = 200
			};
		}
	}
}
