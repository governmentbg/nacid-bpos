using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using MetadataHarvesting.Core.Data;
using MetadataHarvesting.Models;
using MetadataHarvesting.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace MetadataHarvesting.Core.Services
{
	public class HarvestingSourceService : IHarvestingSourceService
	{
		private readonly HarvestingDbContext context;

		public HarvestingSourceService(HarvestingDbContext context)
		{
			this.context = context;
		}

		public async Task<IEnumerable<HarvestingSource>> GetHarvestingSources()
		{
			return await context
				.HarvestingSources
				.AsNoTracking()
				.Where(s => s.IsActive)
				.ToListAsync();
		}

		public async Task UpdateHarvestingSource(int id, DateTime? lastHarvestDate, string lastHarvestResumptionToken = null)
		{
			var source = await GetById(id);

			context.HarvestingSources.Attach(source);

			source.LastHarvestResumptionToken = lastHarvestResumptionToken;
			source.LastHarvestDate = lastHarvestDate;

			context.Entry(source).State = EntityState.Modified;

			await context.SaveChangesAsync();
		}

		public async Task<HarvestingSource> GetById(int sourceId)
		{
			return await context
				.HarvestingSources
				.AsNoTracking()
				.SingleOrDefaultAsync(s => s.Id == sourceId && s.IsActive);
		}

		public async Task<IEnumerable<HarvestingSource>> GetHarvestingSourcesWithUnprocessedRecords()
		{
			return await context
				.HarvestingSources
				.AsNoTracking()
				.Where(s => s.HarvestedRecords.Any(r => r.Status == HarvestedRecordStatus.Harvested))
				.OrderBy(s => s.LastHarvestDate)
				.ToListAsync();
		}

		public async Task<IEnumerable<HarvestingSource>> CreateHarvestingSources(string url,
			int rootClassificationId,
			string metadataFormat,
			int? defaultIdentifierTypeId,
			int? defaultAccessRightsId,
			int? defaultLicenseConditionId,
			DateTime? defaultLicenseStartDate,
			int? defaultResourceTypeId,
			ICollection<string> sets)
		{
			if (!sets.Any())
			{
				sets.Add(null);
			}

			var sources = CreateSources(url, rootClassificationId, metadataFormat, defaultIdentifierTypeId,
				defaultAccessRightsId, defaultLicenseConditionId, defaultLicenseStartDate, defaultResourceTypeId, sets);

			await context.HarvestingSources.AddRangeAsync(sources);
			await context.SaveChangesAsync();

			return sources;
		}

		public async Task UpdateClassificationHarvestingSources(int rootClassificationId,
			string url,
			string metadataFormat,
			int? defaultIdentifierTypeId,
			int? defaultAccessRightsId,
			int? defaultLicenseConditionId,
			DateTime? defaultLicenseStartDate,
			int? defaultResourceTypeId,
			ICollection<string> sets)
		{
			var sources = await context.HarvestingSources
				.Where(s => s.RootClassificationId == rootClassificationId)
				.ToListAsync();

			if (string.IsNullOrEmpty(url))
			{
				DeactivateSources(sources);
			}
			else
			{
				var updateSourceAction = new Action<HarvestingSource>((harvestingSource) => {
					harvestingSource.Url = url;
					harvestingSource.MetadataFormat = metadataFormat;
					harvestingSource.DefaultResourceTypeId = defaultResourceTypeId;
					harvestingSource.DefaultAccessRightsId = defaultAccessRightsId;
					harvestingSource.DefaultLicenseConditionId = defaultLicenseConditionId;
					harvestingSource.DefaultIdentifierTypeId = defaultIdentifierTypeId;
					harvestingSource.DefaultLicenseStartDate = defaultLicenseStartDate;
				});

				if (sets.Any())
				{
					var setMap = sets.ToImmutableHashSet();

					var sourcesToRemove = sources.Where(s => !setMap.Contains(s.Set));
					DeactivateSources(sourcesToRemove);

					var newSets = sets.Where(s => sources.All(src => src.Set != s)).ToList();
					var sourcesToAdd = CreateSources(url, rootClassificationId, metadataFormat, defaultIdentifierTypeId,
						defaultAccessRightsId, defaultLicenseConditionId, defaultLicenseStartDate, defaultResourceTypeId,
						newSets);

					await context.AddRangeAsync(sourcesToAdd);

					var sourcesToUpdate = sources.Where(s => setMap.Contains(s.Set));
					foreach (var harvestingSource in sourcesToUpdate)
					{
						updateSourceAction.Invoke(harvestingSource);
					}
				}
				else
				{
					if (sources.Any(s => s.Set != null))
					{
						// Remove old
						DeactivateSources(sources);

						// Create one new with empty set
						var newSets = new List<string> { null };
						var newSources = CreateSources(url, rootClassificationId, metadataFormat, defaultIdentifierTypeId,
							defaultAccessRightsId, defaultLicenseConditionId, defaultLicenseStartDate,
							defaultResourceTypeId, newSets);

						await context.AddRangeAsync(newSources);
					}
					else
					{
						var harvestingSource = sources.Single();
						updateSourceAction.Invoke(harvestingSource);
					}
				}
			}

			context.UpdateRange(sources);

			await context.SaveChangesAsync();
		}

		private void DeactivateSources(IEnumerable<HarvestingSource> sources)
		{
			foreach (var harvestingSource in sources)
			{
				harvestingSource.IsActive = false;
			}
		}

		private IEnumerable<HarvestingSource> CreateSources(string url,
			int rootClassificationId,
			string metadataFormat,
			int? defaultIdentifierTypeId,
			int? defaultAccessRightsId,
			int? defaultLicenseConditionId,
			DateTime? defaultLicenseStartDate,
			int? defaultResourceTypeId,
			ICollection<string> sets)
		{
			var sources = sets
				.Select(set => new HarvestingSource {
					Url = url,
					RootClassificationId = rootClassificationId,
					MetadataFormat = metadataFormat,
					DefaultAccessRightsId = defaultAccessRightsId,
					DefaultIdentifierTypeId = defaultIdentifierTypeId,
					DefaultLicenseConditionId = defaultLicenseConditionId,
					DefaultLicenseStartDate = defaultLicenseStartDate,
					DefaultResourceTypeId = defaultResourceTypeId,
					Set = set
				})
				.ToList();

			return sources;
		}
	}
}
