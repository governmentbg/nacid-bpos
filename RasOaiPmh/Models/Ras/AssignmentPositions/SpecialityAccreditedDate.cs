using NacidRas.Infrastructure.Data;
using System;

namespace NacidRas.Ras.AssignmentPositions
{
	public class SpecialityAccreditedDate : EntityVersion
	{
		public int SpecialityId { get; set; }
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }
	}
}
