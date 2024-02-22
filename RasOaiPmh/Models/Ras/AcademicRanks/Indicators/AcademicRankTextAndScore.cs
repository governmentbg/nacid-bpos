using NacidRas.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NacidRas.Ras.Indicators
{
	public class AcademicRankTextAndScore : EntityVersion
	{
		public int AcademicRankIndicatorGroupNameId { get; set; }
		public string Text { get; set; }
		public decimal? Score { get; set; }
	}
}
