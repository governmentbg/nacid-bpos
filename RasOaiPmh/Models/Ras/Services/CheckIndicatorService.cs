using NacidRas.Ras.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NacidRas.Ras.Services
{
	public class CheckIndicatorRankService
	{

		private readonly RasDbContext context;

		public CheckIndicatorRankService(RasDbContext context)
		{
			this.context = context;
		}


		public bool CheckIndicators(List<AcademicRankIndicatorGroupName> indicators, int researchAreaId, int academicRankId)
		{
			bool isHabilitated = true;

			decimal sumIndicatorA = 0; // А
			decimal sumIndicatorB = 0; // Б
			decimal sumIndicatorC = 0; // В
			decimal sumIndicatorD = 0; // Г
			decimal sumIndicatorE = 0; // Д
			decimal sumIndicatorF = 0; // Е

			indicators.ForEach(item => {
				if (item.ScientificIndicatorType.IndicatorGroup.Equals("А"))
					sumIndicatorA += item.Score ?? 0;
				if (item.ScientificIndicatorType.IndicatorGroup.Equals("Б"))
					sumIndicatorB += item.Score ?? 0;
				if (item.ScientificIndicatorType.IndicatorGroup.Equals("В"))
					sumIndicatorC += item.Score ?? 0;
				if (item.ScientificIndicatorType.IndicatorGroup.Equals("Г"))
					sumIndicatorD += item.Score ?? 0;
				if (item.ScientificIndicatorType.IndicatorGroup.Equals("Д"))
					sumIndicatorE += item.Score ?? 0;
				if (item.ScientificIndicatorType.IndicatorGroup.Equals("Е"))
					sumIndicatorF += item.Score ?? 0;

			});

			var totalScores = this.context.AcademicRankIndicatorTotals
					.Where(t => t.ResearchAreaId == researchAreaId && t.AcademicRankTypeId == academicRankId && t.IsActive)
					.OrderBy(t => t.IndicatorGroup)
					.Select(s => new { s.TotalScore, s.IndicatorGroup })
					.ToList();

			totalScores.ForEach(element => {
				if (element.IndicatorGroup.Equals("А"))
				{
					if (element.TotalScore != 0 && sumIndicatorA < element.TotalScore)
					{
						isHabilitated = false;
					}
				}
				else if (element.IndicatorGroup.Equals("Б"))
				{
					if (element.TotalScore != 0 && sumIndicatorB < element.TotalScore)
					{
						isHabilitated = false;
					}
				}
				else if (element.IndicatorGroup.Equals("В"))
				{
					if (element.TotalScore != 0 && sumIndicatorC < element.TotalScore)
					{
						isHabilitated = false;
					}
				}
				else if (element.IndicatorGroup.Equals("Г"))
				{
					if (element.TotalScore != 0 && sumIndicatorD < element.TotalScore)
					{
						isHabilitated = false;
					}
				}
				else if (element.IndicatorGroup.Equals("Д"))
				{
					if (element.TotalScore != 0 && sumIndicatorE < element.TotalScore)
					{
						isHabilitated = false;
					}
				}
				else if (element.IndicatorGroup.Equals("Е"))
				{
					if (element.TotalScore != 0 && sumIndicatorF < element.TotalScore)
					{
						isHabilitated = false;
					}
				}
			});

			return isHabilitated;
		}

	}
}
