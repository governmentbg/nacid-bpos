using MetadataHarvesting.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenScience.Common.Constants;
using OpenScience.Common.DomainValidation;
using OpenScience.Common.DomainValidation.Enums;
using OpenScience.Data;
using OpenScience.Data.Classifications.Models;
using OpenScience.Services.Classifications;
using ServerApplication.ClassificationsModule.Dtos;
using ServerApplication.Infrastructure.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerApplication.ClassificationsModule
{
	[Authorize(Policy = "RequireAdministratorRole")]
	[ApiController]
	[Route("api/[controller]")]
	public class ClassificationController : ControllerBase
	{
		private readonly AppDbContext context;
		private readonly UserContext userContext;
		private readonly ClassificationService classificationService;
		private readonly DomainValidationService validator;
		private readonly IHarvestingSourceService harvestingSourceService;

		public ClassificationController(
			AppDbContext context,
			UserContext userContext,
			ClassificationService classificationService,
			DomainValidationService validator,
			IHarvestingSourceService harvestingSourceService
			)
		{
			this.context = context;
			this.userContext = userContext;
			this.classificationService = classificationService;
			this.validator = validator;
			this.harvestingSourceService = harvestingSourceService;
		}

		[HttpGet("Roots")]
		public async Task<IEnumerable<ClassificationDto>> GetRootsClassifications()
		{
			var filter = new ClassificationSearchFilter();
			if (userContext.RoleAlias != UserRoleAliases.ADMINISTRATOR)
			{
				filter.OrganizationIds = userContext.InstitutionIds;
			}

			return await classificationService.GetFilteredAsync(filter, ClassificationDto.SelectExpression);
		}

		[HttpGet("Roots/{id:int}")]
		public async Task<IEnumerable<ClassificationDto>> GetChildHierarchy([FromRoute]int id)
		{
			var filter = new ClassificationSearchFilter(id);
			if (userContext.RoleAlias != UserRoleAliases.ADMINISTRATOR)
			{
				filter.OrganizationIds = userContext.InstitutionIds;
			}

			return await classificationService.GetFilteredAsync(filter, ClassificationDto.SelectExpression);
		}

		[HttpGet("{id:int}")]
		public async Task<Classification> GetClassification([FromRoute]int id)
		{
			return await classificationService.GetByIdAsync(id);
		}

		[HttpPost("")]
		public async Task<ClassificationDto> AddClassification([FromBody]Classification classification)
		{
			await classificationService.AddClassificationAsync(classification);
			await context.SaveChangesAsync();

			if (classification.IsReadonly && !string.IsNullOrEmpty(classification.HarvestUrl))
			{
				await harvestingSourceService.CreateHarvestingSources(classification.HarvestUrl,
					classification.Id,
					classification.MetadataFormat,
					classification.DefaultIdentifierTypeId,
					classification.DefaultAccessRightId,
					classification.DefaultLicenseConditionId,
					classification.DefaultLicenseStartDate,
					classification.DefaultResourceTypeId,
					classification.Sets);
			}

			return await classificationService.SingleAsync(e => e.Id == classification.Id, ClassificationDto.SelectExpression);
		}

		[HttpPut("{id:int}")]
		public async Task<ClassificationDto> PutClassification([FromRoute]int id, [FromBody]Classification classification)
		{
			bool isEditableClassification = await classificationService.IsEditableClassification(id);
			if (!isEditableClassification)
			{
				validator.ThrowErrorMessage(ClassificationErrorCode.Classification_CannotEditClassification);
			}

			classificationService.Update(classification);
			await context.SaveChangesAsync();

			await harvestingSourceService.UpdateClassificationHarvestingSources(classification.Id,
				classification.HarvestUrl,
				classification.MetadataFormat,
				classification.DefaultIdentifierTypeId,
				classification.DefaultAccessRightId,
				classification.DefaultLicenseConditionId,
				classification.DefaultLicenseStartDate,
				classification.DefaultResourceTypeId,
				classification.Sets);

			return await classificationService.SingleAsync(e => e.Id == id, ClassificationDto.SelectExpression);
		}

		[HttpDelete("{id:int}")]
		public async Task DeleteClassification([FromRoute]int id)
		{
			bool isEditableClassification = await classificationService.IsEditableClassification(id);
			if (!isEditableClassification)
			{
				validator.ThrowErrorMessage(ClassificationErrorCode.Classification_CannotDeleteClassification);
			}

			await classificationService.DeleteAsync(id);
			await context.SaveChangesAsync();
		}
	}
}
