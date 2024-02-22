using NacidRas.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NacidRas.Ras.Nomenclatures.Models
{
	public class AcademicRankIndicatorTotal : Nomenclature
	{
		//public int Id { get; set; }
		public int ResearchAreaId { get; set; }
		public int AcademicRankTypeId { get; set; }
		public int TotalScore { get; set; }
		public string IndicatorGroup { get; set; }
	}
}
