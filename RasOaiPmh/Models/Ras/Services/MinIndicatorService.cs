using NacidRas.Ras.Nomenclatures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NacidRas.Ras.Services
{
	public class MinIndicatorService
	{

		private readonly RasDbContext context;

		public MinIndicatorService(RasDbContext context)
		{
			this.context = context;
		}


		public int? MinIndicatorScore(int researchAreaId, int academicId, string indicatorGroup, bool isAcademicRank)
		{
			if (isAcademicRank)
			{
				return this.context.Set<AcademicRankIndicatorTotal>().SingleOrDefault(t => t.IndicatorGroup == indicatorGroup
							&& t.ResearchAreaId == researchAreaId
							&& t.AcademicRankTypeId == academicId 
							&& t.IsActive)?.TotalScore;
			}else
			{
				return this.context.Set<AcademicDegreeIndicatorTotal>().SingleOrDefault(t => t.IndicatorGroup == indicatorGroup
							&& t.ResearchAreaId == researchAreaId
							&& t.AcademicDegreeTypeId == academicId 
							&& t.IsActive)?.TotalScore;
			}
		}
	}
}
