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
	public class HarvestedRecordService : IHarvestedRecordService
	{
		private readonly HarvestingDbContext context;

		public HarvestedRecordService(HarvestingDbContext context)
		{
			this.context = context;
		}

		public async Task<IEnumerable<HarvestedRecord>> GetUnprocessedRecords(int harvestingSourceId, int limit)
		{
			return await context
				.HarvestedRecords
				.Where(r => r.HarvestingSourceId == harvestingSourceId && r.Status == HarvestedRecordStatus.Harvested)
				.OrderBy(r => r.HarvestDate)
				.Take(limit)
				.ToListAsync();
		}

		public async Task UpdateFailedRecords(IEnumerable<HarvestedRecord> records) =>
			await UpdateStatus(records, HarvestedRecordStatus.Failed);

		public async Task UpdateProcessedRecords(IEnumerable<HarvestedRecord> records) =>
			await UpdateStatus(records, HarvestedRecordStatus.Processed);

		public async Task SaveRecords(IEnumerable<Record> records, HarvestingSource harvestingSource, DateTime harvestDate)
		{
			var harvestedRecords = records
				.Select(r => new HarvestedRecord {
					Status = HarvestedRecordStatus.Harvested,
					Identifier = r.Header.Identifier,
					Content = r.RecordXml,
					HarvestingSourceId = harvestingSource.Id,
					HarvestDate = harvestDate,
					MetadataFormat = harvestingSource.MetadataFormat,
					SetSpecs = r.Header.SetSpecs
				})
				.ToList();

			await context.HarvestedRecords.AddRangeAsync(harvestedRecords);
			await context.SaveChangesAsync();
		}

		private async Task UpdateStatus(IEnumerable<HarvestedRecord> records, HarvestedRecordStatus status)
		{
			var ids = records.Select(r => r.Id).ToImmutableHashSet();

			var dbRecords = context
				.HarvestedRecords
				.Where(r => ids.Contains(r.Id));

			foreach (var harvestedRecord in dbRecords)
			{
				harvestedRecord.Status = status;
			}

			await context.SaveChangesAsync();
		}
	}
}
