using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MetadataHarvesting.Models;

namespace MetadataHarvesting.Core.Services
{
	public interface IHarvestingSourceService
	{
		Task<IEnumerable<HarvestingSource>> GetHarvestingSources();

		Task UpdateHarvestingSource(int id, DateTime? lastHarvestDate = null, string lastHarvestResumptionToken = null);

		Task<HarvestingSource> GetById(int sourceId);

		Task<IEnumerable<HarvestingSource>> GetHarvestingSourcesWithUnprocessedRecords();

		Task<IEnumerable<HarvestingSource>> CreateHarvestingSources(string url, int rootClassificationId, string metadataFormat, int? defaultIdentifierTypeId, int? defaultAccessRightsId, int? defaultLicenseConditionId, DateTime? defaultLicenseStartDate, int? defaultResourceTypeId, ICollection<string> sets);

		Task UpdateClassificationHarvestingSources(int rootClassificationId, string url, string metadataFormat, int? defaultIdentifierTypeId, int? defaultAccessRightsId, int? defaultLicenseConditionId, DateTime? defaultLicenseStartDate, int? defaultResourceTypeId, ICollection<string> sets);
	}
}