using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NacidRas.Infrastructure.Data;
using NacidRas.Infrastructure.Linq;
using NacidRas.Ras.AssignmentPositions;
using NacidRas.Ras.Nomenclatures.Dtos;
using NacidRas.Ras.Nomenclatures.Models;
using NacidRas.Register;
using NacidRas.Users;
using System.Linq;
using NacidRas.Infrastructure.Permissions;
using NacidRas.Ras.Globals;
using System.Collections.Generic;
using NacidRas.Ras.Nomenclatures.Enums;

namespace NacidRas.Ras.Nomenclatures
{
	[Controller]
	[Route("api/Nomenclatures/Person/AcademicDegree")]
	public class AcademicDegreeController : BaseNomenclatureController<AcademicDegreeType, NomenclatureFilter>
	{
		public AcademicDegreeController(RasDbContext context)
			: base(context)
		{
		}

		public override SearchResultDto<AcademicDegreeType> GetAll([FromQuery]NomenclatureFilter filter)
		{
			if (filter == null)
			{
				filter = new NomenclatureFilter();
			}

			var predicate = PredicateBuilder.True<AcademicDegreeType>();
			if (!string.IsNullOrWhiteSpace(filter.TextFilter))
			{
				predicate = predicate.And(e => e.Name.Trim().ToLower().Contains(filter.TextFilter.Trim().ToLower()));
			}
			if (filter.IsActive.HasValue && filter.IsActive.Value)
			{
				predicate = predicate.And(e => e.IsActive);
			}

			var query = context.Set<AcademicDegreeType>()
				.AsNoTracking()
				.Where(predicate)
				.OrderBy(e => e.ViewOrder)
				.ThenBy(e => e.Name);

			var result = new SearchResultDto<AcademicDegreeType> {
				TotalCount = query.Count(),
				Result = query.Skip(filter.Offset).Take(filter.Limit).ToList()
			};

			return result;
		}

	}

	[Controller]
	[Route("api/Nomenclatures/Person/Speciality")]
	public class SpecialityController : BaseNomenclatureController<Speciality, SpecialityNomenclatureFilter>
	{
		public SpecialityController(RasDbContext context)
			: base(context)
		{
		}

		public override SearchResultDto<Speciality> GetAll([FromQuery]SpecialityNomenclatureFilter filter)
		{
			if (filter == null)
			{
				filter = new SpecialityNomenclatureFilter();
			}

			var predicate = PredicateBuilder.True<Speciality>();
			if (!string.IsNullOrWhiteSpace(filter.TextFilter))
			{
				predicate = predicate.And(e => e.Name.Trim().ToLower().Contains(filter.TextFilter.Trim().ToLower()));
			}
			if (filter.IsActive.HasValue && filter.IsActive.Value)
			{
				predicate = predicate.And(e => e.IsActive);
			}
			if (filter.InstitutionId.HasValue)
			{
				predicate = predicate.And(e => e.InstitutionId == filter.InstitutionId.Value);
			}
			if (!string.IsNullOrWhiteSpace(filter.PickedSpecialitiesIds))
			{
				var pickedSpecialitiesIds = filter.PickedSpecialitiesIds.Split(",").Select(int.Parse).ToList();
				predicate = predicate.And(e => !pickedSpecialitiesIds.Contains(e.Id));
			}

			predicate = predicate.And(e => e.IsAccredited);

			var query = context.Set<Speciality>()
				.AsNoTracking()
				.Include(e => e.EducationalForm)
				.Include(e => e.NationalStatisticalInstitute)
				.Include(e => e.NsiRegion)
				.Include(e => e.EducationalQualification)
				.Include(e => e.ResearchArea)
				.Where(predicate)
				.Where(s => s.IsActive)
				.OrderBy(e => e.ViewOrder)
				.ThenBy(e => e.Name);

			var result = new SearchResultDto<Speciality> {
				TotalCount = query.Count(),
				Result = query.Skip(filter.Offset).Take(filter.Limit).ToList()
			};

			return result;
		}
	}

	public class SpecialityNomenclatureFilter : NomenclatureFilter
	{
		public int? InstitutionId { get; set; } = null;
		public override bool? IsActive { get; set; } = true;
		public string PickedSpecialitiesIds { get; set; }
	}

	[Controller]
	[Route("api/Nomenclatures/Person/Position")]
	public class PositionController : BaseNomenclatureController<Position, PositionNomenclatureFilter>
	{
		public PositionController(RasDbContext context)
			: base(context)
		{
		}

		public override SearchResultDto<Position> GetAll([FromQuery]PositionNomenclatureFilter filter)
		{
			if (filter == null)
			{
				filter = new PositionNomenclatureFilter();
			}

			var predicate = PredicateBuilder.True<Position>();
			if (!string.IsNullOrWhiteSpace(filter.TextFilter))
			{
				predicate = predicate.And(e => e.Name.Trim().ToLower().Contains(filter.TextFilter.Trim().ToLower()));
			}
			if (filter.IsActive.HasValue && filter.IsActive.Value)
			{
				predicate = predicate.And(e => e.IsActive);
			}
			if (filter.PositionType.HasValue)
			{
				predicate = predicate.And(e => e.PositionType == filter.PositionType);
			}

			var query = context.Set<Position>()
				.AsNoTracking()
				.Where(predicate)
				.Where(s => s.IsActive)
				.OrderBy(e => e.ViewOrder)
				.ThenBy(e => e.Name);

			var result = new SearchResultDto<Position> {
				TotalCount = query.Count(),
				Result = query.Skip(filter.Offset).Take(filter.Limit).ToList()
			};

			return result;
		}
	}

	public class PositionNomenclatureFilter : NomenclatureFilter
	{
		public PositionType? PositionType { get; set; } = null;
	}

	[Controller]
	[Route("api/Nomenclatures/Person/ContractType")]
	public class ContractTypeController : BaseNomenclatureController<ContractType, NomenclatureFilter>
	{
		public ContractTypeController(RasDbContext context)
			: base(context)
		{
		}
	}

	[Controller]
	[Route("api/Nomenclatures/Person/SnsOld")]
	public class SnsOldController : BaseNomenclatureController<SnsOld, NomenclatureFilter>
	{
		public SnsOldController(RasDbContext context)
			: base(context)
		{
		}
	}

	[Controller]
	[Route("api/Nomenclatures/Person/DepositoryOld")]
	public class DepositoryOldController : BaseNomenclatureController<DepositoryOld, NomenclatureFilter>
	{
		public DepositoryOldController(RasDbContext context)
			: base(context)
		{
		}
	}

	[Controller]
	[Route("api/Nomenclatures/Person/AcademicRank")]
	public class AcademicRankController : BaseNomenclatureController<AcademicRankType, NomenclatureFilter>
	{
		public AcademicRankController(RasDbContext context)
			: base(context)
		{
		}

		public override SearchResultDto<AcademicRankType> GetAll([FromQuery]NomenclatureFilter filter)
		{
			if (filter == null)
			{
				filter = new NomenclatureFilter();
			}

			var predicate = PredicateBuilder.True<AcademicRankType>();
			if (!string.IsNullOrWhiteSpace(filter.TextFilter))
			{
				predicate = predicate.And(e => e.Name.Trim().ToLower().Contains(filter.TextFilter.Trim().ToLower()));
			}
			if (filter.IsActive.HasValue && filter.IsActive.Value)
			{
				predicate = predicate.And(e => e.IsActive);
			}

			var query = context.Set<AcademicRankType>()
				.AsNoTracking()
				.Where(predicate)
				.OrderBy(e => e.ViewOrder)
				.ThenBy(e => e.Name);

			var result = new SearchResultDto<AcademicRankType> {
				TotalCount = query.Count(),
				Result = query.Skip(filter.Offset).Take(filter.Limit).ToList()
			};

			return result;
		}

	}

	[Controller]
	[Route("api/Nomenclatures/Person/SpecialityOld")]
	public class SpecialityOldController : BaseNomenclatureController<SpecialityOld, NomenclatureFilter>
	{
		public SpecialityOldController(RasDbContext context)
			: base(context)
		{
		}
	}

	[Controller]
	[Route("api/Nomenclatures/Person/Institution")]
	public class InstitutionController : BaseNomenclatureController<Institution, InstitutionNomenclatureFilter>
	{
		private readonly UserContext userContext;
		private readonly AuthorizedUserTypeService authorizedUserTypeService;
		private readonly PermissionVerificator permissionVerificator;

		public InstitutionController(
			RasDbContext context,
			UserContext userContext,
			AuthorizedUserTypeService authorizedUserTypeService,
			PermissionVerificator permissionVerificator
			) : base(context)
		{
			this.userContext = userContext;
			this.authorizedUserTypeService = authorizedUserTypeService;
			this.permissionVerificator = permissionVerificator;
		}

		public override SearchResultDto<Institution> GetAll([FromQuery] InstitutionNomenclatureFilter filter)
		{
			if (filter == null)
				filter = new InstitutionNomenclatureFilter();


			filter.GetAllInstitutions = false;

			var predicate = PredicateBuilder.True<Institution>();

			predicate = predicate.And(e => e.IsOnlyRas.HasValue && e.IsOnlyRas.Value);

			if (userContext.IsPublicUser
				&& !filter.GetAllInstitutions
				&& authorizedUserTypeService.IsInstitutionUser())
			{
				//var institutionUsers = context.InstitutionUser
				//	.Include(e => e.Institution)
				//	.Where(e => e.UserId == userContext.UserId)
				//	.ToList();

				var institutionIds = permissionVerificator.GetPermittedInstitutionsWithParent();
				//var parentInstitutionIds = institutionUsers
				//	.Where(e => e.Institution.ParentId != null)
				//	.Select(e => e.Institution.ParentId.Value)
				//	.ToList();

				//var childInstitutionIds = context.Institutions
				//	.Where(e => institutionIds.Contains(e.ParentId.Value))
				//	.Select(e => e.Id)
				//	.ToList();

				//institutionIds.AddRange(parentInstitutionIds);
				//institutionIds.AddRange(childInstitutionIds);

				predicate = predicate.And(e => institutionIds.Contains(e.Id));
			}

			if (filter.InstitutionType.HasValue)
			{
				predicate = predicate.And(GlobalFunctions.GetInstitutionSubtypesByMainTypeExpr(filter.InstitutionType.Value));
			}

			if (!string.IsNullOrWhiteSpace(filter.TextFilter))
				predicate = predicate.And(e => e.Name.Trim().ToLower().Contains(filter.TextFilter.Trim().ToLower()));

			if (filter.IsActive.HasValue && filter.IsActive.Value)
				predicate = predicate.And(e => e.IsActive);

			if(!filter.GetAllInstitutionsUnauthorized) { 
				if (filter.ParentId.HasValue)
					predicate = predicate.And(e => e.ParentId == filter.ParentId.Value);
				else
					predicate = predicate.And(e => e.ParentId == null);
			}

			var query = context.Set<Institution>()
				.AsNoTracking()
				.Include(e => e.Settlement)
				.Where(predicate)
				.OrderBy(e => e.ViewOrder)
				.ThenBy(e => e.Name);

			var result = new SearchResultDto<Institution> {
				TotalCount = query.Count(),
				Result = query.Skip(filter.Offset).Take(filter.Limit).ToList()
			};

			return result;
		}
	}

	[Controller]
	[Route("api/Nomenclatures/InstitutionAuthorized")]
	public class InstitutionAuthorizedController : ControllerBase
	{
		private readonly RasDbContext context;

		public InstitutionAuthorizedController(
			RasDbContext context
			)
		{
			this.context = context;
		}

		public IActionResult GetAll([FromQuery] InstitutionNomenclatureFilter filter)
		{
			if (filter == null)
				filter = new InstitutionNomenclatureFilter();

			var predicate = PredicateBuilder.True<Institution>();

			predicate = predicate.And(e => e.IsOnlyRas.HasValue && e.IsOnlyRas.Value);

			if (!string.IsNullOrWhiteSpace(filter.TextFilter))
				predicate = predicate.And(e => e.Name.Trim().ToLower().Contains(filter.TextFilter.Trim().ToLower()));

			// Currently inactive should also be added to result.
			// In future institutions should be implemented as a Lot
			//predicate = predicate.And(e => e.IsActive);

			if (!filter.GetAllInstitutions)
			{
				if (filter.ParentId.HasValue)
					predicate = predicate.And(e => e.ParentId == filter.ParentId.Value);
				else
					predicate = predicate.And(e => e.ParentId == null);
			}

			var query = context.Set<Institution>()
				.AsNoTracking()
				.Where(predicate)
				.Select(e => new { e.Id, Name = e.Name + (e.ParentId.HasValue ? " (" + e.Parent.Name + ")" : "") })
				.OrderBy(e => e.Name);

			var result = new {
				TotalCount = query.Count(),
				Result = query.Skip(filter.Offset).Take(filter.Limit).ToList()
			};

			return Ok(result);
		}
	}

	public class InstitutionNomenclatureFilter : NomenclatureFilter
	{
		public InstitutionType? InstitutionType { get; set; } = null;
		public int? ParentId { get; set; } = null;
		public bool GetAllInstitutions { get; set; } = true;
		public override bool? IsActive { get; set; } = true;
		public bool GetAllInstitutionsUnauthorized { get; set; } = false;
	}

	[Controller]
	[Route("api/Nomenclatures/Person/ResearchArea")]
	public class ResearchAreaController : BaseNomenclatureController<ResearchArea, ResearchAreaNomenclatureFilter>
	{
		public ResearchAreaController(RasDbContext context)
			: base(context)
		{
		}

		public override SearchResultDto<ResearchArea> GetAll([FromQuery]ResearchAreaNomenclatureFilter filter)
		{
			if (filter == null)
			{
				filter = new ResearchAreaNomenclatureFilter();
			}

			var predicate = PredicateBuilder.True<ResearchArea>();
			if (!string.IsNullOrWhiteSpace(filter.TextFilter))
			{
				predicate = predicate.And(e => e.Name.Trim().ToLower().Contains(filter.TextFilter.Trim().ToLower())
				|| e.Code.Trim().Contains(filter.TextFilter.Trim()));
			}
			if (filter.IsActive.HasValue && filter.IsActive.Value)
			{
				predicate = predicate.And(e => e.IsActive);
			}

			if (filter.HasOnlyProffesionalDirection)
			{
				predicate = predicate.And(e => e.ParentId.HasValue);
			}

			var query = context.Set<ResearchArea>()
				.AsNoTracking()
				.Where(predicate)
				.Where(s => s.IsActive)
				.OrderBy(e => e.ViewOrder)
				.ThenBy(e => e.Name);

			var result = new SearchResultDto<ResearchArea> {
				TotalCount = query.Count(),
				Result = query.Skip(filter.Offset).Take(filter.Limit).ToList()
			};

			return result;
		}
	}

	[Controller]
	[Route("api/Nomenclatures/Language")]
	public class LanguageController : BaseNomenclatureController<Language, NomenclatureFilter>
	{
		public LanguageController(RasDbContext context)
			: base(context)
		{
		}
	}

	[Controller]
	[Route("api/Nomenclatures/Settlement")]
	public class SettlementController : BaseNomenclatureController<Settlement, NomenclatureFilter>
	{
		public SettlementController(RasDbContext context)
			: base(context)
		{
		}
	}

	[Controller]
	[Route("api/Nomenclatures/Country")]
	public class CountryController : BaseNomenclatureController<Country, NomenclatureFilter>
	{
		public CountryController(RasDbContext context)
			: base(context)
		{
		}
	}

	[Controller]
	[Route("api/Nomenclature/CommitModificationReason")]
	public class CommitModificationReasonController : BaseNomenclatureController<CommitModificationReason, NomenclatureFilter>
	{
		public CommitModificationReasonController(RasDbContext context)
			: base(context)
		{ }
	}
}
