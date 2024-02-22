using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenScience.Common.Constants;
using OpenScience.Common.DomainValidation;
using OpenScience.Common.DomainValidation.Enums;
using OpenScience.Data;
using OpenScience.Data.Institutions.Models;
using OpenScience.Services.Classifications.Dtos;
using OpenScience.Services.Institutions;
using ServerApplication.Infrastructure.Auth;
using ServerApplication.Infrastructure.Data;
using ServerApplication.InstitutionsModule.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApplication.InstitutionsModule
{
	[ApiController]
	[Route("api/[controller]")]
	public class InstitutionController : BaseEntityController<Institution, InstitutionSearchFilter>
	{
		private readonly UserContext userContext;
		private readonly DomainValidationService validator;
		private readonly InstitutionService institutionService;

		public InstitutionController(
			AppDbContext context,
			InstitutionService entityService,
			UserContext userContext,
			DomainValidationService validator
			)
			: base(context, entityService)
		{
			this.userContext = userContext;
			this.validator = validator;
			this.institutionService = entityService;
		}

		public override async Task<IEnumerable<Institution>> GetFiltered([FromQuery] InstitutionSearchFilter filter)
		{
			var predicate = filter.GetPredicate();

			IQueryable<Institution> query = institutionService.PrepareFilterQuery()
				.Where(predicate);
			if (filter.SearchInForeignNameOnly.HasValue && filter.SearchInForeignNameOnly.Value)
			{
				query = query.OrderBy(e => e.NameEn)
					.ThenBy(e => e.Id);
			}
			else
			{
				query = query.OrderBy(e => e.Name)
					.ThenBy(e => e.Id);
			}

			return await query
				.Skip(filter.Offset)
				.Take(filter.Limit)
				.ToListAsync();
		}

		[HttpGet("Select")]
		public async Task<IEnumerable<Institution>> GetSelect([FromQuery]InstitutionSelectFilterDto filter)
		{
			if (filter == null)
			{
				filter = new InstitutionSelectFilterDto();
			}
			filter.InstitutionIds = userContext.InstitutionIds;

			IQueryable<Institution> query = context.Institutions
				.AsNoTracking()
				.Where(filter.GetPredicate());

			if(filter.SearchInForeignNameOnly.HasValue && filter.SearchInForeignNameOnly.Value)
			{
				query = query.OrderBy(e => e.NameEn)
					.ThenBy(e => e.Id);
			}
			else
			{
				query = query.OrderBy(e => e.Name)
					.ThenBy(e => e.Id);
			}

			if (filter.Offset.HasValue && filter.Limit.HasValue)
			{
				query = query.Skip(filter.Offset.Value)
					.Take(filter.Limit.Value);
			}

			var result = await query.ToListAsync();

			return result;
		}

		[HttpGet("Classifications")]
		public async Task<IEnumerable<FlatClassificationHierarchyItemDto>> GetHierarchy([FromQuery]int institutionId, [FromQuery]int? publicationId)
		{
			if(userContext.RoleAlias != UserRoleAliases.ADMINISTRATOR
				&& !userContext.InstitutionIds.Contains(institutionId))
			{
				validator.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
			}

			return await institutionService.GetInstitutionClassifications(institutionId);
		}

		[HttpPut("{id:int}/Deactivation")]
		public async Task Deactivate([FromRoute]int id)
		{
			if(userContext.RoleAlias != UserRoleAliases.ADMINISTRATOR)
			{
				validator.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
			}

			Institution institution = await entityService.SingleAsync(e => e.Id == id);
			if(!institution.IsActive)
			{
				validator.ThrowErrorMessage(InsitutionErrorCode.Insitution_CannotDeactivateNotActive);
			}

			institution.Deactivate();
			entityService.Update(institution);
			await context.SaveChangesAsync();
		}
	}
}
