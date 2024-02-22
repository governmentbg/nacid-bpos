using NacidRas.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NacidRas.Ras.Indicators
{
	public class AcademicRankIndicatorGroupName: EntityVersion
	{
		public int AcademicRankId { get; set; }
		public int ResearchAreaId { get; set; }
		public int AcademicRankTypeId { get; set; }
		public int ScientificIndicatorTypeId { get; set; }
		[Skip]
		public ScientificIndicatorType ScientificIndicatorType { get; set; }
		public ICollection<AcademicRankIndicatorPublication> AcademicRankIndicatorPublications { get; set; }
		public ICollection<AcademicRankTextAndScore> AcademicRankTextAndScores { get; set; }
		public bool IsExternalIndicator { get; set; } = false;
		public string ExternalIndicatorName { get; set; }
		public string TextProof { get; set; }
		public decimal? Score { get; set; }
		[NotMapped]
		public int? MinScore { get; set; }

		public AcademicRankIndicatorGroupName()
		{
			this.AcademicRankIndicatorPublications = new List<AcademicRankIndicatorPublication>();
			this.AcademicRankTextAndScores = new List<AcademicRankTextAndScore>();
		}
	}
}
