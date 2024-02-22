using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using NacidRas.Ras.Nomenclatures.Models;
using System;
using NacidRas.Ras.Services;

namespace NacidRas.Ras
{

	[Route("api/Nomenclatures/AcademicRankIndicatorGroupNameTemplate")]
	public class AcademicRankIndicatorGroupNameTemplateController
	{
		private readonly RasDbContext context;
		private readonly MinIndicatorService minIndicatorService;

		public AcademicRankIndicatorGroupNameTemplateController(RasDbContext context, MinIndicatorService minIndicatorService)
		{
			this.context = context;
			this.minIndicatorService = minIndicatorService;
		}

		[AllowAnonymous]
		[Route("")]
		public List<AcademicRankIndicatorGroupNameTemplate> Get([FromQuery] int researchAreaId, [FromQuery] int academicRankId)
		{
			var result = this.context.AcademicRankIndicatorGroupNameTemplates
				.Include(t => t.ScientificIndicatorType)
				.Where(t => t.ResearchAreaId == researchAreaId && t.AcademicRankTypeId == academicRankId && t.IsActive)
				.Select(t => new AcademicRankIndicatorGroupNameTemplate {
					AcademicRankTypeId = t.AcademicRankTypeId,
					ResearchAreaId = t.ResearchAreaId,
					ScientificIndicatorType = t.ScientificIndicatorType,
					ScientificIndicatorTypeId = t.ScientificIndicatorTypeId

				})
				.OrderBy(t => t.ScientificIndicatorType.ViewOrder)
				.ToList();

			string currentGroup = null;
			foreach (var item in result)
			{
				if(item.ScientificIndicatorType.IndicatorGroup != currentGroup)
				{
					item.MinScore = this.minIndicatorService.MinIndicatorScore(item.ResearchAreaId, item.AcademicRankTypeId, item.ScientificIndicatorType.IndicatorGroup, true);
				}
				currentGroup = item.ScientificIndicatorType.IndicatorGroup;
			}

			return result;
		}

		[AllowAnonymous]
		[Route("GetExternal")]
		public ScientificIndicatorType GetExternal()
		{
			var externalIndicator = this.context.ScientificIndicatorTypes.Single(t => String.IsNullOrEmpty(t.IndicatorGroup));
			return externalIndicator;
		}
	}
}
