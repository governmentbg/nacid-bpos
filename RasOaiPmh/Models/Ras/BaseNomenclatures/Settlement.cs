using NacidRas.Infrastructure.Data;

namespace NacidRas.Ras.Nomenclatures.Models
{
	public class Settlement : Nomenclature
	{
		public int MunicipalityId { get; set; }
		public int DistrictId { get; set; }
		public string MunicipalityCode { get; set; }
		public string DistrictCode { get; set; }
		public string MunicipalityCode2 { get; set; }
		public string DistrictCode2 { get; set; }
		public string Code { get; set; }
		public string TypeName { get; set; }
		public string SettlementName { get; set; }
		public string SettlementNameAlt { get; set; }
		public string TypeCode { get; set; }
		public string MayoraltyCode { get; set; }
		public string Category { get; set; }
		public string Altitude { get; set; }
		public string Alias { get; set; }
		public string Description { get; set; }
		public bool IsDistrict { get; set; }
	}
}
