using NacidRas.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NacidRas.Ras.Nomenclatures.Models
{
	public class AcademicRankIndicatorGroupNameTemplate : Entity
	{
		public int ResearchAreaId { get; set; }
		public int AcademicRankTypeId { get; set; }
		public int ScientificIndicatorTypeId { get; set; }
		[Skip]
		public ScientificIndicatorType ScientificIndicatorType { get; set; }
		[NotMapped]
		public int? MinScore { get; set; }
		public bool IsActive { get; set; }
	}
}
