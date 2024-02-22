using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetadataHarvesting.Core.Exceptions;
using MetadataHarvesting.Core.Providers;
using MetadataHarvesting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenScience.Common.Services;
using ServerApplication.MetadataProviderModule.Dtos;

namespace ServerApplication.MetadataProviderModule
{
	[ApiController]
	[Route("api/MetadataHarvesting")]
	public class MetadataHarvestingController
	{
		private readonly Func<string, IRepositoryMetadataHarvester> harvesterFactory;
		private readonly RetryExecutionService retryExecutionService;
		private readonly MetadataHarvestingConfiguration configuration;

		public MetadataHarvestingController(
			Func<string, IRepositoryMetadataHarvester> harvesterFactory,
			RetryExecutionService retryExecutionService,
			IOptions<MetadataHarvestingConfiguration> options)
		{
			this.harvesterFactory = harvesterFactory;
			this.retryExecutionService = retryExecutionService;
			this.configuration = options.Value;
		}

		[HttpGet("ListMetadataFormats")]
		public async Task<IEnumerable<MetadataFormatDto>> ListMetadataFormats([FromQuery]string url, [FromQuery] string textFilter)
		{
			var harvester = harvesterFactory.Invoke(url);

			var metadataFormats = (await harvester.ListMetadataFormats()).MetadataFormats;

			if (!string.IsNullOrEmpty(textFilter))
			{
				metadataFormats = metadataFormats
					.Where(f => f.Prefix.Trim().ToLower().Contains(textFilter.Trim().ToLower()))
					.ToList();
			}

			var result = metadataFormats
				.Select(f => new MetadataFormatDto { Name = f.Prefix })
				.ToList();

			return result;
		}

		[HttpGet("ListSets")]
		public async Task<IEnumerable<Set>> ListSets([FromQuery]string url)
		{
			var harvester = harvesterFactory.Invoke(url);

			var result = await harvester.ListSets(null);

			return result.Items;
		}

		[HttpGet("ValidateRepositoryUrl")]
		public async Task<bool> ValidateRepositoryUrl([FromQuery]string url)
		{
			var harvester = harvesterFactory.Invoke(url);

			var (success, _, __) = await retryExecutionService.ExecuteAndCaptureAsync<RepositoryConnectionFailureException, Identify>(configuration.RepositoryValidationRetryAttempts, async _ => await harvester.Identify());

			return success;
		}
	}
}
