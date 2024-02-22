using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NacidRas.Infrastructure.Linq;
using NacidRas.Ras.AssignmentPositions.Enums;
using NacidRas.Ras.Nomenclatures.Enums;
using NacidRas.RasRegister;

namespace NacidRas.Ras.Dtos
{
	public class PersonFilterDto
	{
		private readonly List<int> habilitatedRankIds = new List<int> { 1, 2 };

		public int Offset { get; set; }
		public int Limit { get; set; }

		public string Name { get; set; }
		public bool? HasExactNameMatch { get; set; }
		public DateTime? BirthDateFrom { get; set; }
		public DateTime? BirthDateTo { get; set; }
		public string Uin { get; set; }
		public PersonType? Type { get; set; }
		public int? ResearchAreaId { get; set; }
		public int? AcademicDegreeTypeId { get; set; }
		public DateTime? DiplomaDateFrom { get; set; }
		public DateTime? DiplomaDateTo { get; set; }
		public string DiplomaNumber { get; set; }
		public int? InstitutionId { get; set; }
		public string DissertationTitleKeywords { get; set; }
		public string FirstLetter { get; set; }
		public bool? IsHabilitated { get; set; }
		public List<int?> AcademicRankTypeIds { get; set; }
		public bool IsCurrentAcademicRank { get; set; }
		public bool IsNotCurrentAcademicRank { get; set; }

		public DateTime? DismissDateFrom { get; set; }
		public DateTime? DismissDateTo { get; set; }

		public DateTime? ContractDateFrom { get; set; }
		public DateTime? ContractDateTo { get; set; }

		// Ras Official filter properties
		public int? RasOfficialPositionId { get; set; }
		public int? RasOfficialInstitutionId { get; set; }
		public int? RasOfficialFacultyId { get; set; }
		public int? RasOfficialDepartmentId { get; set; }
		public int? RasOfficialContractTypeId { get; set; }
		public AcademicStaffType? RasOfficialAcademicStaffType { get; set; }
		public PositionType? RasOfficialPositionType { get; set; }
		public DateTime? RasOfficialFromAppointmentActDate { get; set; }
		public DateTime? RasOfficialToAppointmentActDate { get; set; }

		// Use this to clear all ras official filters (if ras user does not have permission for ras official)
		public void ClearRasOfficialFilters()
		{
			RasOfficialPositionId = null;
			RasOfficialInstitutionId = null;
			RasOfficialFacultyId = null;
			RasOfficialDepartmentId = null;
			RasOfficialContractTypeId = null;
			RasOfficialAcademicStaffType = null;
			RasOfficialPositionType = null;
			RasOfficialFromAppointmentActDate = null;
			RasOfficialToAppointmentActDate = null;
		}

		public PersonFilterDto()
		{
			this.AcademicRankTypeIds = new List<int?>();
		}

		public Expression<Func<RasCommit, bool>> GetPredicate()
		{
			var predicate = PredicateBuilder.True<RasCommit>();

			if (!string.IsNullOrWhiteSpace(Name))
			{

				if (HasExactNameMatch.HasValue && HasExactNameMatch.Value)
				{
					ICollection<Expression<Func<RasCommit, string>>> expressions = new List<Expression<Func<RasCommit, string>>> {
						e => e.PersonPart.Entity.Name.ToLower()
					};

					predicate = predicate.AndOrStringContainsSeparate(expressions, Name);
				}
				else if (!HasExactNameMatch.HasValue || !HasExactNameMatch.Value)
				{
					var splitedNames = Name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
					foreach (var item in splitedNames)
						predicate = predicate.And(e => e.PersonPart.Entity.Name.ToLower().Contains(item.ToLower()));
				}

			}

			// TODO: add check for indicators by min score !
			if (IsHabilitated.HasValue && IsHabilitated.Value == true)
			{
				predicate = predicate
					.And(s => s.AcademicRankParts.Any(p => p.Entity.AcademicRankTypeId.HasValue && habilitatedRankIds.Contains(p.Entity.AcademicRankTypeId.Value) && p.Entity.IsHabilitated));
			}

			if (BirthDateFrom.HasValue)
			{
				predicate = predicate.And(s => s.PersonPart.Entity.BirthDate >= BirthDateFrom.Value);
			}
			if (BirthDateTo.HasValue)
			{
				predicate = predicate.And(s => s.PersonPart.Entity.BirthDate <= BirthDateTo.Value);
			}
			if (!string.IsNullOrWhiteSpace(Uin))
			{
				predicate = predicate.And(s => s.PersonPart.Entity.Uin.Contains(Uin)
					|| s.PersonPart.Entity.PersonIdns.Any(e => e.IdnNumber.Contains(Uin)));
			}
			if (Type.HasValue)
			{
				predicate = predicate.And(s => s.PersonPart.Entity.Type == Type);
			}

			if (ResearchAreaId.HasValue)
			{
				var inner = PredicateBuilder.False<RasCommit>();

				if (IsHabilitated.HasValue && IsHabilitated.Value)
				{
					inner = inner
					.Or(s => s.AcademicRankParts.Any(p => p.Entity.AcademicRankTypeId.HasValue && habilitatedRankIds.Contains(p.Entity.AcademicRankTypeId.Value) && p.Entity.IsHabilitated && (p.Entity.ResearchAreaId == ResearchAreaId || p.Entity.ResearchArea.ParentId == ResearchAreaId)));
				}
				else
				{
					inner = inner.Or(s => s.AcademicDegreeParts.Any(t => t.Entity.ResearchAreaId == ResearchAreaId || t.Entity.ResearchArea.ParentId == ResearchAreaId));
					inner = inner.Or(s => s.AcademicRankParts.Any(t => t.Entity.ResearchAreaId == ResearchAreaId || t.Entity.ResearchArea.ParentId == ResearchAreaId));
				}

				predicate = predicate.And(inner);
			}

			if (AcademicDegreeTypeId.HasValue)
			{
				predicate = predicate.And(s => s.AcademicDegreeParts.Any(t => t.Entity.AcademicDegreeTypeId == AcademicDegreeTypeId));
			}

			if (DiplomaDateFrom.HasValue)
			{
				predicate = predicate.And(s => s.AcademicDegreeParts.Any(t => t.Entity.DiplomaDate >= DiplomaDateFrom));
			}
			if (DiplomaDateTo.HasValue)
			{
				predicate = predicate.And(s => s.AcademicDegreeParts.Any(t => t.Entity.DiplomaDate <= DiplomaDateTo));
			}

			if (!string.IsNullOrWhiteSpace(DiplomaNumber))
			{
				predicate = predicate.And(s => s.AcademicDegreeParts.Any(t => t.Entity.DiplomaNumber.Contains(DiplomaNumber)));
			}

			if (!string.IsNullOrWhiteSpace(DissertationTitleKeywords))
			{
				var keywords = DissertationTitleKeywords.Split(' ', StringSplitOptions.RemoveEmptyEntries)
					.Select(k => k.Trim().ToLower())
					.ToList();

				foreach (var keyword in keywords)
				{
					predicate = predicate.And(s => s.AcademicDegreeParts.Any(t => t.Entity.Dissertation.Title.Trim().ToLower().Contains(keyword)));
				}
			}

			if (InstitutionId.HasValue)
			{
				if (IsHabilitated.HasValue && IsHabilitated.Value)
				{
					predicate = predicate.And(s => s.AcademicRankParts.Any(t => (t.Entity.InstitutionId == InstitutionId || t.Entity.InstitutionByParentId == InstitutionId) && t.Entity.IsHabilitated));
				}
				else
				{
					var inner = PredicateBuilder.False<RasCommit>();
					//inner = inner.Or(s => s.AcademicDegreeParts.Any(t => t.Entity.InstitutionId == InstitutionId || t.Entity.InstitutionByParentId == InstitutionId));

					if (IsCurrentAcademicRank && !IsNotCurrentAcademicRank)
					{
						inner = inner.Or(s => s.AcademicRankParts.Any(t =>
							(t.Entity.InstitutionId == InstitutionId ||
							 t.Entity.InstitutionByParentId == InstitutionId) && t.Entity.IsCurrent));
					}
					else if (IsNotCurrentAcademicRank && !IsCurrentAcademicRank)
					{
						inner = inner.Or(s => s.AcademicRankParts.Any(t =>
							(t.Entity.InstitutionId == InstitutionId ||
							 t.Entity.InstitutionByParentId == InstitutionId) && !t.Entity.IsCurrent));
					}
					else
					{
						inner = inner.Or(s => s.AcademicRankParts.Any(t =>
							t.Entity.InstitutionId == InstitutionId ||
							t.Entity.InstitutionByParentId == InstitutionId));
					}

					predicate = predicate.And(inner);
				}
			}

			if (AcademicRankTypeIds.Any())
			{
				predicate = predicate.And(s => s.AcademicRankParts.Any(t => AcademicRankTypeIds.Contains(t.Entity.AcademicRankTypeId)));
			}

			if (!string.IsNullOrEmpty(FirstLetter))
			{
				var firstLetter = FirstLetter[0];

				predicate = predicate.And(s => s.PersonPart.Entity.FirstName.IndexOf(firstLetter) == 0);
			}

			if (ContractDateFrom.HasValue)
			{
				predicate = predicate.And(s =>
				s.AcademicRankParts.Any(t => (!AcademicRankTypeIds.Any() || AcademicRankTypeIds.Contains(t.Entity.AcademicRankTypeId))
				&& t.Entity.DateOfContract >= ContractDateFrom));
			}

			if (ContractDateTo.HasValue)
			{
				predicate = predicate.And(s =>
				s.AcademicRankParts.Any(t => (!AcademicRankTypeIds.Any() || AcademicRankTypeIds.Contains(t.Entity.AcademicRankTypeId))
				&& t.Entity.DateOfContract <= ContractDateTo));
			}

			if (DismissDateFrom.HasValue)
			{
				predicate = predicate.And(s =>
				s.AcademicRankParts.Any(t => (!AcademicRankTypeIds.Any() || AcademicRankTypeIds.Contains(t.Entity.AcademicRankTypeId))
				&& t.Entity.DismissActDate >= DismissDateFrom));
			}

			if (DismissDateTo.HasValue)
			{
				predicate = predicate.And(s =>
				s.AcademicRankParts.Any(t => (!AcademicRankTypeIds.Any() || AcademicRankTypeIds.Contains(t.Entity.AcademicRankTypeId))
				&& t.Entity.DismissActDate <= DismissDateTo));
			}


			if (IsCurrentAcademicRank && IsNotCurrentAcademicRank)
			{
				var inner = PredicateBuilder.False<RasCommit>();

				inner = inner.Or(s => s.AcademicRankParts.Any(t => t.Entity.IsCurrent));
				inner = inner.Or(s => s.AcademicRankParts.Any(t => !t.Entity.IsCurrent));

				predicate = predicate.And(inner);
			}
			else if (IsCurrentAcademicRank)
			{
				predicate = predicate.And(s => s.AcademicRankParts.Any(t => (!AcademicRankTypeIds.Any() || AcademicRankTypeIds.Contains(t.Entity.AcademicRankTypeId)) &&
																t.Entity.IsCurrent));
			}
			else if (IsNotCurrentAcademicRank)
			{
				predicate = predicate.And(s => s.AcademicRankParts.Any(t => (!AcademicRankTypeIds.Any() || AcademicRankTypeIds.Contains(t.Entity.AcademicRankTypeId)) &&
																!t.Entity.IsCurrent));
			}

			// Ras Official filter
			if (RasOfficialPositionId.HasValue)
			{
				predicate = predicate.And(e => e.AssignmentPositionParts
						.Any(s => s.Entity.PositionId == RasOfficialPositionId)
					|| e.AdministrativePositionParts
						.Any(s => s.Entity.PositionId == RasOfficialPositionId));
			}

			if (RasOfficialInstitutionId.HasValue)
			{
				predicate = predicate.And(e => e.AssignmentPositionParts
						.Any(s => s.Entity.InstitutionId == RasOfficialInstitutionId)
					|| e.AdministrativePositionParts
						.Any(s => s.Entity.InstitutionId == RasOfficialInstitutionId));
			}

			if (RasOfficialFacultyId.HasValue)
			{
				predicate = predicate.And(e => e.AssignmentPositionParts
						.Any(s => s.Entity.InstitutionByParentId == RasOfficialFacultyId)
					|| e.AdministrativePositionParts
						.Any(s => s.Entity.InstitutionByParentId == RasOfficialFacultyId));
			}

			if (RasOfficialDepartmentId.HasValue)
			{
				predicate = predicate.And(e => e.AssignmentPositionParts
						.Any(s => s.Entity.DepartmentId == RasOfficialDepartmentId)
					|| e.AdministrativePositionParts
						.Any(s => s.Entity.DepartmentId == RasOfficialDepartmentId));
			}

			if (RasOfficialContractTypeId.HasValue)
			{
				predicate = predicate.And(e => e.AssignmentPositionParts
						.Any(s => s.Entity.ContractTypeId == RasOfficialContractTypeId)
					|| e.AdministrativePositionParts
						.Any(s => s.Entity.ContractTypeId == RasOfficialContractTypeId));
			}

			if (RasOfficialAcademicStaffType.HasValue)
			{
				predicate = predicate.And(e => e.AssignmentPositionParts
						.Any(s => s.Entity.AcademicStaffType == RasOfficialAcademicStaffType));
			}

			if (RasOfficialPositionType.HasValue)
			{
				predicate = predicate.And(e => e.AssignmentPositionParts
						.Any(s => s.Entity.Position.PositionType == RasOfficialPositionType)
					|| e.AdministrativePositionParts
						.Any(s => s.Entity.Position.PositionType == RasOfficialPositionType));
			}

			if (RasOfficialFromAppointmentActDate.HasValue && RasOfficialToAppointmentActDate.HasValue)
			{
				predicate = predicate.And(e => e.AssignmentPositionParts
						.Any(s => s.Entity.AppointmentActDate >= RasOfficialFromAppointmentActDate && s.Entity.AppointmentActDate <= RasOfficialToAppointmentActDate)
					|| e.AdministrativePositionParts
					.Any(s => s.Entity.AppointmentActDate >= RasOfficialFromAppointmentActDate && s.Entity.AppointmentActDate <= RasOfficialToAppointmentActDate));
			}
			else if (RasOfficialFromAppointmentActDate.HasValue)
			{
				predicate = predicate.And(e => e.AssignmentPositionParts
						.Any(s => s.Entity.AppointmentActDate >= RasOfficialFromAppointmentActDate)
					|| e.AdministrativePositionParts
					.Any(s => s.Entity.AppointmentActDate >= RasOfficialFromAppointmentActDate));
			}
			else if (RasOfficialToAppointmentActDate.HasValue)
			{
				predicate = predicate.And(e => e.AssignmentPositionParts
						.Any(s => s.Entity.AppointmentActDate <= RasOfficialToAppointmentActDate)
					|| e.AdministrativePositionParts
					.Any(s => s.Entity.AppointmentActDate <= RasOfficialToAppointmentActDate));
			}

			return predicate;
		}
	}
}
