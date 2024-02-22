using NacidRas.Infrastructure.Data;
using NacidRas.Integrations.RndIntegration.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NacidRas.Ras.Indicators
{
	public class AcademicDegreeIndicatorPublication : EntityVersion
	{
		public int AcademicDegreeIndicatorGroupNameId { get; set; }
		public int? PublicationInitialPartId { get; set; }
		[NotMapped]
		[EmptyReference]
		public Publication PublicationInitialPart { get; set; }
		public ICollection<AcademicDegreeIndicatorPublicationQuote> AcademicDegreeIndicatorPublicationQuotes { get; set; }
		public string TextProof { get; set; }
		public decimal? Score { get; set; }

		public AcademicDegreeIndicatorPublication()
		{
			this.AcademicDegreeIndicatorPublicationQuotes = new List<AcademicDegreeIndicatorPublicationQuote>();
		}
	}
}
