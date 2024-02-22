using NacidRas.Infrastructure.Data;
using NacidRas.Ras.AssignmentPositions.Enums;
using NacidRas.Ras.Nomenclatures.Models;
using System;
using System.Collections.Generic;

namespace NacidRas.Ras.AssignmentPositions
{
	public class AssignmentPosition : EntityVersion
	{
		public int Year { get; set; }
		public Semester Semester { get; set; }
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
		public int? AnnualRate { get; set; }
		public int? AssignedHours { get; set; }
		public int? ContractTypeId { get; set; }
		[Skip]
		public ContractType ContractType { get; set; }
		public AcademicStaffType? AcademicStaffType { get; set; }
		public string AppointmentActNumber { get; set; }
		public DateTime? AppointmentActDate { get; set; }
		public string Note { get; set; }
		// We need this for the migration from ACAD_Data DB
		public double? _AcadPersonId { get; set; }

		public List<AssignmentPositionSpeciality> AssignmentPositionSpecialities { get; set; }

		public AssignmentPosition()
		{
			AssignmentPositionSpecialities = new List<AssignmentPositionSpeciality>();
		}
	}
}
