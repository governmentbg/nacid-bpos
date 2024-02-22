using NacidRas.Infrastructure.Data;

namespace NacidRas.Ras.BaseNomenclatures
{
	public class District : Nomenclature
	{
		public string Code { get; set; }
		public string Code2 { get; set; }
		public string SecondLevelRegionCode { get; set; }
		public string MainSettlementCode { get; set; }
		public string Alias { get; set; }
		public string Description { get; set; }
		public string NameAlt { get; set; }
	}
}
