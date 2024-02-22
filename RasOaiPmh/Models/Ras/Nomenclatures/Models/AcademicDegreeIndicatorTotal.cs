using NacidRas.Infrastructure.Data;

namespace NacidRas.Ras
{
	public class AcademicDegreeIndicatorTotal : Nomenclature
	{
		//public int Id { get; set; }
		public int ResearchAreaId { get; set; }
		public int AcademicDegreeTypeId { get; set; }
		public int TotalScore { get; set; }
		public string IndicatorGroup { get; set; }
	}
}
