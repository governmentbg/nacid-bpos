using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NacidRas.Ras.Nomenclatures
{
	[Route("api/Nomenclatures/CheckAcademicRankIndicator")]
	public class CheckAcademicRankIndicatorController
	{
		readonly RasDbContext context;

		public CheckAcademicRankIndicatorController(RasDbContext context)
		{
			this.context = context;
		}

		[AllowAnonymous]
		[Route("")]
		[HttpGet]
		public int CheckIndicators([FromQuery] int researchAreaId, [FromQuery] int academicRankId)
		{
			var result = this.context.AcademicRankIndicatorTotals
						.Where(t => t.ResearchAreaId == researchAreaId && t.AcademicRankTypeId == academicRankId && t.IsActive)
						.OrderBy(t => t.IndicatorGroup)
						.Select(s => s.TotalScore)
						.ToList();

			int totalResult = 0;

			foreach (var item in result)
			{
				totalResult += item;
			}

			return totalResult;
		}

	}
}
