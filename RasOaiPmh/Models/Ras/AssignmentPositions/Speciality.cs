using NacidRas.Infrastructure.Data;
using NacidRas.Ras.AssignmentPositions.Enums;
using NacidRas.Ras.Nomenclatures.Models;
using System;
using System.Collections.Generic;

namespace NacidRas.Ras.AssignmentPositions
{
	public class Speciality : Nomenclature
	{
		public int InstitutionId { get; set; }
		[Skip]
		public Institution Institution { get; set; }
		public int? NsiRegionId { get; set; }
		[Skip]
		public NationalStatisticalInstitute NsiRegion { get; set; }
		public int? NationalStatisticalInstituteId { get; set; }
		[Skip]
		public NationalStatisticalInstitute NationalStatisticalInstitute { get; set; }
		public int? ResearchAreaId { get; set; }
		[Skip]
		public ResearchArea ResearchArea { get; set; }
		public int? EducationalQualificationId { get; set; }
		[Skip]
		public EducationalQualification EducationalQualification { get; set; }
		public int? EducationalFormId { get; set; }
		[Skip]
		public EducationalForm EducationalForm { get; set; }
		public decimal? Duration { get; set; }
		public bool IsAccredited { get; set; }
		public bool IsRegulated { get; set; }
		public bool IsForCadets { get; set; }
		public DateTime? CreateDate { get; set; }
		public long? _ACADSpecialityId { get; set; }

		public ICollection<SpecialityAccreditedDate> SpecialityAccreditedDates { get; set; }

		public Speciality()
		{
			SpecialityAccreditedDates = new List<SpecialityAccreditedDate>();
		}
	}
}
