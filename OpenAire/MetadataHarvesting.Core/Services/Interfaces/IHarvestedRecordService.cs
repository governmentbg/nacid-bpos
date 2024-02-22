using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MetadataHarvesting.Models;

namespace MetadataHarvesting.Core.Services
{
	public interface IHarvestedRecordService
	{
		Task<IEnumerable<HarvestedRecord>> GetUnprocessedRecords(int harvestingSourceId, int limit);

		Task UpdateProcessedRecords(IEnumerable<HarvestedRecord> records);

		Task UpdateFailedRecords(IEnumerable<HarvestedRecord> records);

		Task SaveRecords(IEnumerable<Record> records, HarvestingSource harvestingSource, DateTime harvestDate);
	}
}
