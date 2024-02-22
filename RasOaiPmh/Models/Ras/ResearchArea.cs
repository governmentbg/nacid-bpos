using NacidRas.Infrastructure.Data;

namespace NacidRas.Ras
{
	public class ResearchArea : Nomenclature
	{
		public string Code { get; set; }

		public string NameAlt { get; set; }

		public int? ParentId { get; set; }

		public int? _ExternalId { get; set; }
	}
}
