using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NacidRas.Ras.Nomenclatures
{
	[Route("api/Nomenclatures/CheckAcademicDegreeIndicator")]
	public class CheckAcademicDegreeIndicatorController
	{
		readonly RasDbContext context;

		public CheckAcademicDegreeIndicatorController(RasDbContext context)
		{
			this.context = context;
		}

		[AllowAnonymous]
		[Route("")]
		[HttpGet]
		public int CheckIndicators([FromQuery] int researchAreaId, [FromQuery] int academicDegreeId)
		{

			if (academicDegreeId != 1 && academicDegreeId != 2)
			{
				if (academicDegreeId == 7)
				{
					academicDegreeId = 1;
				}
				else
				{
					academicDegreeId = 2;
				}
			}

			var result = this.context.AcademicDegreeIndicatorTotals
						.Where(t => t.ResearchAreaId == researchAreaId && t.AcademicDegreeTypeId == academicDegreeId && t.IsActive)
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
