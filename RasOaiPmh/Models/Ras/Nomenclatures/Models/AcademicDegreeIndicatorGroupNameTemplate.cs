using NacidRas.Infrastructure.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace NacidRas.Ras
{
	// Nomenclature template 
	public class AcademicDegreeIndicatorGroupNameTemplate : Entity
	{
		public int ResearchAreaId { get; set; }
		public int AcademicDegreeTypeId { get; set; }
		public int ScientificIndicatorTypeId { get; set; }
		[Skip]
		public ScientificIndicatorType ScientificIndicatorType { get; set; }
		[NotMapped]
		public int? MinScore { get; set; }
		public bool IsActive { get; set; }
	}
}
