using NacidRas.Infrastructure.Data;

namespace NacidRas.Ras.Nomenclatures.Models
{
	public class NationalStatisticalInstitute : Nomenclature
	{
		public string Code { get; set; }
		public string NameAlt { get; set; }
		public string OldCode { get; set; }
		public int? ParentId { get; set; }
		public int? RootId { get; set; }
		public int? Level { get; set; }
	}
}
