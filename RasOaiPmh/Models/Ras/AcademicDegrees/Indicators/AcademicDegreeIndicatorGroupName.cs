using NacidRas.Infrastructure.Data;
using NacidRas.Ras.Indicators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NacidRas.Ras
{
	public class AcademicDegreeIndicatorGroupName: EntityVersion
	{
		public int AcademicDegreeId { get; set; }
		public int ResearchAreaId { get; set; }
		public int AcademicDegreeTypeId { get; set; }
		public int ScientificIndicatorTypeId { get; set; }
		[Skip]
		public ScientificIndicatorType ScientificIndicatorType { get; set; }
		public ICollection<AcademicDegreeIndicatorPublication> AcademicDegreeIndicatorPublications { get; set; }
		public ICollection<AcademicDegreeTextAndScore> AcademicDegreeTextAndScores { get; set; }
		public bool IsExternalIndicator { get; set; } = false;
		public string ExternalIndicatorName { get; set; }
		public string TextProof { get; set; }
		public decimal? Score { get; set; }
		[NotMapped]
		public int? MinScore { get; set; }
		public AcademicDegreeIndicatorGroupName()
		{
			this.AcademicDegreeIndicatorPublications = new List<AcademicDegreeIndicatorPublication>();
			this.AcademicDegreeTextAndScores = new List<AcademicDegreeTextAndScore>();
		}
	}
}
