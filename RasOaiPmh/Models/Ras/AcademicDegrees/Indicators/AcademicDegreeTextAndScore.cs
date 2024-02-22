using NacidRas.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NacidRas.Ras.Indicators
{
	public class AcademicDegreeTextAndScore : EntityVersion
	{
		public int AcademicDegreeIndicatorGroupNameId { get; set; }
		public string Text { get; set; }
		public decimal? Score { get; set; }
	}
}
