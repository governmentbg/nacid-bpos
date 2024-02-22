using NacidRas.Infrastructure.Data;
using NacidRas.Ras.Nomenclatures.Models;
using System;

namespace NacidRas.Ras.AdministrativePositions
{
	public class AdministrativePosition : EntityVersion
	{
		public DateTime FromDate { get; set; }
		public DateTime? ToDate { get; set; }
		public int ContractNumber { get; set; }
		public int? InstitutionId { get; set; }
		[Skip]
		public Institution Institution { get; set; }
		public int? InstitutionByParentId { get; set; }
		[Skip]
		public Institution InstitutionByParent { get; set; }
		public int? DepartmentId { get; set; }
		[Skip]
		public Institution Department { get; set; }
		public int? PositionId { get; set; }
		[Skip]
		public Position Position { get; set; }
		public int? ContractTypeId { get; set; }
		[Skip]
		public ContractType ContractType { get; set; }
		public string AppointmentActNumber { get; set; }
		public DateTime? AppointmentActDate { get; set; }
		public string Note { get; set; }
	}
}
