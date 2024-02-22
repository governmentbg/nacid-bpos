using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NacidRas.Infrastructure.Data;
using NacidRas.Ras.Enums;
using NacidRas.Ras.Files;
using NacidRas.Ras.Nomenclatures.Models;

namespace NacidRas.Ras
{
	// Научна степен
	public class AcademicDegree : EntityVersion
	{
		//public int PersonId { get; set; }
		public int? _ExternalBaseDegreeHonorisId { get; set; }
		public int? _ExternalBaseSirenaId { get; set; }
		public bool IsActive { get; set; }

		// определя от старите кои имат сигурен фк към degree_honoris и кои нямат
		// header-а на фронт-енда се оцветява ако няма
		public bool HasProperlyFK { get; set; } = true;
		// определя дали е от старите им true-да, false-не
		// не се показва на фронт-енда
		public bool Before2011 { get; set; } = false;
		public string ProtocolNumber { get; set; }
		public string AdditionalProtocolNumber { get; set; }
		public DateTime? ProtocolDate { get; set; }
		public bool HasIssuedCertificate { get; set; }
		public DateTime? CertificateDate { get; set; }
		public string CertificateNumber { get; set; }
		[CloneUpdate]
		public int? CertificateFileId { get; set; }
		public CertificateFile CertificateFile { get; set; }

		public List<AcademicDegreeFile> AcademicDegreeFiles { get; set; }

		public int? AcademicDegreeTypeId { get; set; }
		[Skip]
		public AcademicDegreeType AcademicDegreeType { get; set; }

		public int? ResearchAreaId { get; set; }
		[Skip]
		public ResearchArea ResearchArea { get; set; }
		// -- Номенклатура от старата база
		public int? SpecialityOldId { get; set; }
		[Skip]
		public SpecialityOld SpecialityOld { get; set; }

		public DateTime? DiplomaDate { get; set; }
		public string DiplomaNumber { get; set; }
		[CloneUpdate]
		public int? DiplomaFileId { get; set; }
		public DiplomaFile DiplomaFile { get; set; }

		// Indicators from template with score
		public List<AcademicDegreeIndicatorGroupName> AcademicDegreeIndicatorGroupNames { get; set; }
		public decimal? IndicatorsSum { get; set; }
		// Данни за защитилите в чужбина		
		public bool GraduatedAbroad { get; set; }

		// Допълнителни полета при избор на защитил в чужбина според тип организация
		public int? CountryId { get; set; }
		[Skip]
		public Country Country { get; set; }
		// Should become nomenclature
		public string ForeignInstitution { get; set; }
		public string ForeignInstitutionAlt { get; set; }
		// Град, в който е признат дисертационният труд 
		public string ForeignTown { get; set; }
		public string ForeignTownAlt { get; set; }
		// -- временно ще се използва за град в България, при тях не е номенклатура, но при нас има Settlement !
		public int? SettlementId { get; set; }
		[Skip]
		public Settlement Settlement { get; set; }

		// Научна организация, в което е признат дисертационният труд или е защитен:
		// Institution отговаря на nsb_org от старата база InstitutionType = ScientificOrganization
		// Българско висше училище, в което е признат дисертационният труд или е защитен:
		// Institution отговаря на nsb_vuz от старата база InstitutionType = University
		public int? InstitutionId { get; set; }
		[Skip]
		public Institution Institution { get; set; }
		public int? InstitutionByParentId { get; set; }
		[Skip]
		public Institution InstitutionByParent { get; set; }
		public InstitutionRegisterTypeEnum InstitutionRegisterType { get; set; } = InstitutionRegisterTypeEnum.Register;
		public string ScientificOrganizationOther { get; set; }
		public string UniversityPrimaryUnit { get; set; }

		// Допълнителни полета при избор на защитил в страна според тип организация
		//public string UniversityFaculty { get; set; }
		//public string UniversityPrimaryUnit { get; set; }
		//public string ScientificOrganizationFaculty { get; set; }
		//public string ScientificOrganizationPrimaryUnit { get; set; }

		// данни за удостоверението за признаване на защитения дисертационен труд;

		// Не са потвърдени още дали ще се слагат.
		// Форма на убочение: редовно, задочно, дистанционно, самостоятелна, 
		// дали е държавна поръчка

		//public int? DissertationId { get; set; }
		public Dissertation Dissertation { get; set; }

		public AcademicDegree()
		{
			AcademicDegreeIndicatorGroupNames = new List<AcademicDegreeIndicatorGroupName>();
			AcademicDegreeFiles = new List<AcademicDegreeFile>();
		}
	}

	public class AcademicDegreeConfiguration : IEntityTypeConfiguration<AcademicDegree>
	{
		public void Configure(EntityTypeBuilder<AcademicDegree> builder)
		{
			builder.HasOne(e => e.Dissertation)
				.WithOne()
				.HasForeignKey<Dissertation>();
		}
	}
}
