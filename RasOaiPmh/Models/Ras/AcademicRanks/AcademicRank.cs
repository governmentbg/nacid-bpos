using NacidRas.Infrastructure.Data;
using NacidRas.Ras.AcademicRanks;
using NacidRas.Ras.Enums;
using NacidRas.Ras.Files;
using NacidRas.Ras.Indicators;
using NacidRas.Ras.Nomenclatures.Models;
using System;
using System.Collections.Generic;

namespace NacidRas.Ras
{
	// Академична длъжност
	public class AcademicRank : EntityVersion
	{
		// Академични длъжности от 1962 г. до 27.03.2011 г. (научни звания)
		//public bool IsVak { get; set; }

		// за звание
		//Свидетелство No/дата: 23161 / 01.08.2005, утвърдено с Протокол No/дата: 8 / 09.05.2005

		// за длъжност Заповед No/дата: 649 / 09.07.2012

		public int? _ExternalBaseDegreeHonorisId { get; set; }
		public int? _ExternalBaseSirenaId { get; set; }
		public bool IsActive { get; set; }

		// определя от старите кои имат сигурен фк към degree_honoris и кои нямат
		// има и идентификатор на фронт-енда
		public bool HasProperlyFK { get; set; } = true;
		// определя дали е от старите им true-да, false-не
		public bool Before2011 { get; set; } = false;
		public string ProtocolNumber { get; set; }
		public string AdditionalProtocolNumber { get; set; }
		public DateTime? ProtocolDate { get; set; }

		// Мигрирано поле от старата база
		public string Title { get; set; }
		// Научна организация, в което е признат дисертационният труд или е защитен:
		// Institution отговаря на nsb_org от старата база
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

		public InstitutionRegisterTypeEnum InstitutionRegisterType{ get; set; } = InstitutionRegisterTypeEnum.Register;
		public string UniversityPrimaryUnit { get; set; }
		public int? AcademicRankTypeId { get; set; }
		[Skip]
		public AcademicRankType AcademicRankType { get; set; }
		// -- В старата база длъжностите преди години са се казвали звания, флаг за фронт-енда
		public bool IsHonorableTitle { get; set; }
		public int? ResearchAreaId { get; set; }
		[Skip]
		public ResearchArea ResearchArea { get; set; }
		// -- Номенклатура от старата база
		public int? SpecialityOldId { get; set; }
		[Skip]
		public SpecialityOld SpecialityOld { get; set; }
		//public List<ScientificIndicator> Indicators { get; set; }
		public string NumberOfContract { get; set; }
		public DateTime? DateOfContract { get; set; }
		public DateTime? StartingFrom { get; set; }
		public string DismissActNumber { get; set; }
		public DateTime? DismissActDate { get; set; }
		public DateTime? StartingTo { get; set; }
		public string DismissMotive { get; set; }

		// Председател на Жури
		public string HeadOfJury { get; set; }
		// -- Председател на Жури на английски от старата база
		public string HeadOfJuryAlt { get; set; }
		// Членове на Жури
		public string Jury { get; set; }
		// Членове на Жури на английски от старата база
		public string JuryAlt { get; set; }

		// Призната без конкурс
		//public bool Something { get; set; }

		public bool IsCurrent { get; set; } = false;

		// Копие от акт за назначаване
		//public int RecruitmentActFileId { get; set; }

		public List<AcademicRankIndicatorGroupName> AcademicRankIndicatorGroupNames { get; set; }
		public decimal? IndicatorsSum { get; set; }
		public bool IsHabilitated { get; set; } = false;
		public List<AcademicRankFile> AcademicRankFiles { get; set; }

		[CloneUpdate]
		public int? EmploymentContractFileId { get; set; }
		public EmploymentContractFile EmploymentContractFile { get; set; }

		public bool HasIssuedCertificate { get; set; }
		public DateTime? CertificateDate { get; set; }
		public string CertificateNumber { get; set; }
		[CloneUpdate]
		public int? CertificateRankFileId { get; set; }
		public CertificateRankFile CertificateRankFile { get; set; }

		public AcademicRank()
		{
			AcademicRankIndicatorGroupNames = new List<AcademicRankIndicatorGroupName>();
			AcademicRankFiles = new List<AcademicRankFile>();
		}
	}
}
