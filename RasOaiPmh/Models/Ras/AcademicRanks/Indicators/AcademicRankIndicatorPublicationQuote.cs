using NacidRas.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NacidRas.Ras.Indicators
{
	public class AcademicRankIndicatorPublicationQuote : EntityVersion
	{
		public int AcademicRankIndicatorPublicationId { get; set; }
		public string Quote { get; set; }
		public decimal? Score { get; set; }

		[NotMapped]
		public int ModifiedStatus { get; set; }

		[NotMapped]
		public int Order { get; set; }
	}
}
