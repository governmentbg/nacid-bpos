using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using NacidRas.Ras.Services;

namespace NacidRas.Ras
{

	[Route("api/Nomenclatures/AcademicDegreeIndicatorGroupNameTemplate")]
	public class AcademicDegreeIndicatorGroupNameTemplateController
	{
		private readonly RasDbContext context;
		private readonly MinIndicatorService minIndicatorService;

		public AcademicDegreeIndicatorGroupNameTemplateController(RasDbContext context, MinIndicatorService minIndicatorService)
		{
			this.context = context;
			this.minIndicatorService = minIndicatorService;
		}

		[AllowAnonymous]
		[Route("")]
		public List<AcademicDegreeIndicatorGroupNameTemplate> Get([FromQuery] int researchAreaId, [FromQuery] int academicDegreeId)
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

			var result = this.context.AcademicDegreeIndicatorGroupNameTemplates
				.Include(t => t.ScientificIndicatorType)
				.Where(t => t.ResearchAreaId == researchAreaId && t.AcademicDegreeTypeId == academicDegreeId && t.IsActive)
				.Select(t => new AcademicDegreeIndicatorGroupNameTemplate {
					AcademicDegreeTypeId = t.AcademicDegreeTypeId,
					ResearchAreaId = t.ResearchAreaId,
					ScientificIndicatorType = t.ScientificIndicatorType,
					ScientificIndicatorTypeId = t.ScientificIndicatorTypeId
				})
				.ToList();

			string currentGroup = null;
			foreach (var item in result)
			{
				if (item.ScientificIndicatorType.IndicatorGroup != currentGroup)
				{
					item.MinScore = this.minIndicatorService.MinIndicatorScore(item.ResearchAreaId, item.AcademicDegreeTypeId, item.ScientificIndicatorType.IndicatorGroup, false);
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
