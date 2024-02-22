using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenScience.Common.Constants;
using OpenScience.Common.DomainValidation;
using OpenScience.Common.DomainValidation.Enums;
using OpenScience.Common.Services;
using OpenScience.Data;
using OpenScience.Data.Publications.Enums;
using OpenScience.Data.Publications.Models;
using OpenScience.Services.Publications;
using ServerApplication.Infrastructure.Auth;
using ServerApplication.Infrastructure.Configuration;
using ServerApplication.PublicationsModule.Dtos;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ServerApplication.PublicationsModule
{
	[ApiController]
	[Route("api/[controller]")]
	public class PublicationController : ControllerBase
	{
		private readonly AppDbContext context;
		private readonly UserContext userContext;
		private readonly PublicationService publicationService;
		private readonly PublicationHandleService publicationHandleService;
		private readonly PublicationIndexingService publicationIndexingService;
		private readonly PublicationPermissionVerificator publicationPermissionVerificator;
		private readonly DomainValidationService validator;
		private readonly RetryExecutionService retryExecutionService;

		private readonly HandleConfiguration handleConfiguration;
		private readonly FullTextConfiguration fullTextConfiguration;

		public PublicationController(
			AppDbContext context,
			UserContext userContext,
			PublicationService publicationService,
			PublicationHandleService publicationHandleService,
			PublicationIndexingService publicationIndexingService,
			PublicationPermissionVerificator publicationPermissionVerificator,
			DomainValidationService validator,
			RetryExecutionService retryExecutionService,
			IOptions<HandleConfiguration> handleConfigurationOptions,
			IOptions<FullTextConfiguration> fullTextConfigurationOptions
			)
		{
			this.context = context;
			this.userContext = userContext;
			this.publicationService = publicationService;
			this.publicationHandleService = publicationHandleService;
			this.publicationIndexingService = publicationIndexingService;
			this.publicationPermissionVerificator = publicationPermissionVerificator;
			this.validator = validator;
			this.retryExecutionService = retryExecutionService;
			this.handleConfiguration = handleConfigurationOptions.Value;
			this.fullTextConfiguration = fullTextConfigurationOptions.Value;
		}

		[HttpGet]
		public async Task<IEnumerable<PublicationSearchResultDto>> Search([FromQuery]PublicationSearchFilter filter)
		{
			if(userContext.RoleAlias == UserRoleAliases.SCIENTIST)
			{
				filter.CreatedByUserId = userContext.UserId;
			}
			else if(userContext.RoleAlias == UserRoleAliases.MODERATOR || userContext.RoleAlias == UserRoleAliases.ORGANIZATION_ADMINISTRATOR)
			{
				filter.FilterByInstitutions = true;
				filter.InstitutionIds = userContext.InstitutionIds;
			}
			
			return await publicationService.GetFilteredAsync(filter, PublicationSearchResultDto.SelectExpression, e => e.ModificationDate);
		}

		[HttpGet("{id:int}")]
		public async Task<Publication> GetPublication([FromRoute]int id)
		{
			Publication publication = await publicationService.GetByIdAsync(id);
			if(!publicationPermissionVerificator.CanPreviewPublication(userContext.UserId, userContext.RoleAlias, userContext.InstitutionIds, publication))
			{
				validator.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
			}

			return publication;
		}

		[HttpPost]
		public async Task<Publication> PostPublication([FromBody]Publication publication)
		{
			if(!publicationPermissionVerificator.CanCreatePublication(userContext.RoleAlias, userContext.InstitutionIds, publication))
			{
				validator.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
			}

			publication.CreatedByUserId = userContext.UserId;	
			using (var transaction = context.BeginTransaction())
			{
				publicationService.Add(publication);
				await context.SaveChangesAsync();

				if (publication.Status == PublicationStatus.Published)
				{
					if (!publicationPermissionVerificator.CanSubmitPublication(userContext.RoleAlias, userContext.InstitutionIds, publication))
					{
						validator.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
					}

					if (string.IsNullOrEmpty(publication.Identifier))
					{
						await retryExecutionService.ExecuteAsync<HttpRequestException>(
							handleConfiguration.MaxNumberAttempts,
							async _ => {
								await publicationHandleService.SetPublicationHandle(handleConfiguration.PreviewLinkTemplate, publication);
							}
						);
					}

					await publicationIndexingService.IndexPublication(publication, fullTextConfiguration.ElasticMaxNumberAttempts, fullTextConfiguration.FileFetchingUrlTemplate, fullTextConfiguration.OcrResponseUrlTemplate);
				}

				await context.SaveChangesAsync();
			}
			
			return publication;
		}

		[HttpPut("{id:int}")]
		public async Task<Publication> PutPublication([FromRoute]int id, [FromBody]Publication publication)
		{
			if (!publicationPermissionVerificator.CanEditPublication(userContext.UserId, userContext.RoleAlias, userContext.InstitutionIds, publication))
			{
				validator.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
			}

			if (publication.Status == PublicationStatus.Published)
			{
				if (!publicationPermissionVerificator.CanSubmitPublication(userContext.RoleAlias, userContext.InstitutionIds, publication))
				{
					validator.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
				}

				if (string.IsNullOrEmpty(publication.Identifier))
				{
					await retryExecutionService.ExecuteAsync<HttpRequestException>(
						handleConfiguration.MaxNumberAttempts,
						async _ => {
							await publicationHandleService.SetPublicationHandle(handleConfiguration.PreviewLinkTemplate, publication);
						}
					);
				}

				await publicationIndexingService.IndexPublication(publication, fullTextConfiguration.ElasticMaxNumberAttempts, fullTextConfiguration.FileFetchingUrlTemplate, fullTextConfiguration.OcrResponseUrlTemplate);
			}

			publicationService.Update(publication);
			await context.SaveChangesAsync();

			return publication;
		}

		[HttpPost("{id:int}/PendingApproval")]
		public async Task MarkPendingApprovalPublication([FromRoute]int id)
		{
			Publication publication = await publicationService.GetPlainByIdAsync(id);
			if (!publicationPermissionVerificator.CanEditPublication(userContext.UserId, userContext.RoleAlias, userContext.InstitutionIds, publication))
			{
				validator.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
			}

			if(publication.Status != PublicationStatus.Draft)
			{
				validator.ThrowErrorMessage(PublicationErrorCode.Publication_CannotMarkPendingApprovalNotDraft);
			}
			
			publication.Status = PublicationStatus.PendingApproval;
			publicationService.UpdatePlain(publication);
			await context.SaveChangesAsync();
		}

		[HttpPost("{id:int}/Publishing")]
		public async Task PublishPublication([FromRoute]int id)
		{
			Publication publication = await publicationService.GetByIdAsync(id);
			if (!publicationPermissionVerificator.CanSubmitPublication(userContext.RoleAlias, userContext.InstitutionIds, publication))
			{
				validator.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
			}

			publication.Status = PublicationStatus.Published;

			if (string.IsNullOrEmpty(publication.Identifier))
			{
				await retryExecutionService.ExecuteAsync<HttpRequestException>(
					handleConfiguration.MaxNumberAttempts,
					async _ => {
						await publicationHandleService.SetPublicationHandle(handleConfiguration.PreviewLinkTemplate, publication);
						publicationService.Update(publication);
						await context.SaveChangesAsync();
					}
				);
			}
			else
			{
				publicationService.Update(publication);
				await context.SaveChangesAsync();
			}

			await publicationIndexingService.IndexPublication(publication, fullTextConfiguration.ElasticMaxNumberAttempts, fullTextConfiguration.FileFetchingUrlTemplate, fullTextConfiguration.OcrResponseUrlTemplate);
		}

		[HttpPost("{id:int}/Denial")]
		public async Task DenyPublication([FromRoute]int id)
		{
			Publication publication = await publicationService.GetPlainByIdAsync(id);
			if (!publicationPermissionVerificator.CanSubmitPublication(userContext.RoleAlias, userContext.InstitutionIds, publication))
			{
				validator.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
			}

			publication.Status = PublicationStatus.NotApproved;
			publicationService.UpdatePlain(publication);
			await context.SaveChangesAsync();
		}
	}
}
