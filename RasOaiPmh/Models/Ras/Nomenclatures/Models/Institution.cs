using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NacidRas.Infrastructure.Data;
using NacidRas.Ras.BaseNomenclatures;
using NacidRas.Ras.Nomenclatures.Models;

namespace NacidRas.Ras
{
	public class Institution : Nomenclature
	{
        public int LotNumber { get; set; }
        public int State { get; set; }
        public string InstitutionTypeName { get; set; }
        public int? _ExternalId { get; set; }
		public int? ParentId { get; set; }
		public Institution Parent { get; set; }
		public string NameAlt { get; set; }
		public int? InitialId { get; set; }
		public Institution Initial { get; set; }
		public InstitutionType? InstitutionType { get; set; }
		public bool? IsOnlyRas { get; set; }
		public int? SettlementId { get; set; }
		[Skip]
		public Settlement Settlement { get; set; }
		public int? DistrictId { get; set; }
		[Skip]
		public District District { get; set; }
		public int? MunicipalityId { get; set; }
		[Skip]
		public Municipality Municipality { get; set; }
		public int? _ACADUniId { get; set; }
		public int? _ACADFacultyId { get; set; }
	}

	public class InstitutionConfiguration : IEntityTypeConfiguration<Institution>
	{
		public void Configure(EntityTypeBuilder<Institution> builder)
		{
			builder.HasOne(t => t.Parent)
					.WithMany()
					.HasForeignKey(t => t.ParentId);

			builder.HasOne(t => t.Initial)
					.WithMany()
					.HasForeignKey(t => t.InitialId);
		}
	}

	public enum InstitutionType
	{
		Institution = 1,
		Center = 4,
		University = 7,
		Faculty = 8,
		Agency = 11,
		AccreditedHospital = 14,
		BotanicalGarden = 15,
		Station = 22,
		ScientificOrganization = 25
	}
}
