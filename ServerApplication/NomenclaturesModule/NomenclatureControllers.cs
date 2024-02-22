using Microsoft.AspNetCore.Mvc;
using OpenScience.Data;
using OpenScience.Data.Nomenclatures.Models;
using OpenScience.Services.Base;
using ServerApplication.Infrastructure.Data;
using ServerApplication.NomenclaturesModule.Dtos;

namespace ServerApplication.NomenclaturesModule
{
	[ApiController]
	[Route("api/[controller]")]
	public class ResourceTypeController : BaseNomenclatureController<ResourceType, BaseForeignNameNomenclatureFilterDto<ResourceType>>
	{
		public ResourceTypeController(
			AppDbContext context,
			INomenclatureService<ResourceType> nomenclatureService
			) : base(context, nomenclatureService)
		{

		}
	}

	[Route("api/[controller]")]
	public class ResourceTypeGeneralController : BaseNomenclatureController<ResourceTypeGeneral, BaseForeignNameNomenclatureFilterDto<ResourceTypeGeneral>>
	{
		public ResourceTypeGeneralController(
			AppDbContext context,
			INomenclatureService<ResourceTypeGeneral> nomenclatureService
			) : base(context, nomenclatureService)
		{

		}
	}

	[ApiController]
	[Route("api/[controller]")]
	public class ResourceIdentifierTypeController : BaseNomenclatureController<ResourceIdentifierType, BaseNomenclatureFilterDto<ResourceIdentifierType>>
	{
		public ResourceIdentifierTypeController(
			AppDbContext context,
			INomenclatureService<ResourceIdentifierType> nomenclatureService
			) : base(context, nomenclatureService)
		{

		}
	}

	[ApiController]
	[Route("api/[controller]")]
	public class NameIdentifierSchemeController : BaseNomenclatureController<NameIdentifierScheme, BaseNomenclatureFilterDto<NameIdentifierScheme>>
	{
		public NameIdentifierSchemeController(
			AppDbContext context,
			INomenclatureService<NameIdentifierScheme> nomenclatureService
			) : base(context, nomenclatureService)
		{

		}
	}

	[ApiController]
	[Route("api/[controller]")]
	public class OrganizationalIdentifierSchemeController : BaseNomenclatureController<OrganizationalIdentifierScheme, BaseNomenclatureFilterDto<OrganizationalIdentifierScheme>>
	{
		public OrganizationalIdentifierSchemeController(
			AppDbContext context,
			INomenclatureService<OrganizationalIdentifierScheme> nomenclatureService
			) : base(context, nomenclatureService)
		{

		}
	}

	[ApiController]
	[Route("api/[controller]")]
	public class TitleTypeController : BaseNomenclatureController<TitleType, BaseForeignNameNomenclatureFilterDto<TitleType>>
	{
		public TitleTypeController(
			AppDbContext context,
			INomenclatureService<TitleType> nomenclatureService
			) : base(context, nomenclatureService)
		{

		}
	}

	[ApiController]
	[Route("api/[controller]")]
	public class LanguageController : BaseNomenclatureController<Language, BaseForeignNameNomenclatureFilterDto<Language>>
	{
		public LanguageController(
			AppDbContext context,
			INomenclatureService<Language> nomenclatureService
			) : base(context, nomenclatureService)
		{

		}
	}

	[ApiController]
	[Route("api/[controller]")]
	public class ContributorTypeController : BaseNomenclatureController<ContributorType, BaseForeignNameNomenclatureFilterDto<ContributorType>>
	{
		public ContributorTypeController(
			AppDbContext context,
			INomenclatureService<ContributorType> nomenclatureService
			) : base(context, nomenclatureService)
		{

		}
	}

	[ApiController]
	[Route("api/[controller]")]
	public class LicenseTypeController : BaseNomenclatureController<LicenseType, BaseNomenclatureFilterDto<LicenseType>>
	{
		public LicenseTypeController(
			AppDbContext context,
			INomenclatureService<LicenseType> nomenclatureService
			) : base(context, nomenclatureService)
		{

		}
	}

	[ApiController]
	[Route("api/[controller]")]
	public class AccessRightController : BaseNomenclatureController<AccessRight, BaseForeignNameNomenclatureFilterDto<AccessRight>>
	{
		public AccessRightController(
			AppDbContext context,
			INomenclatureService<AccessRight> nomenclatureService
			) : base(context, nomenclatureService)
		{

		}
	}

	[ApiController]
	[Route("api/[controller]")]
	public class RelationTypeController : BaseNomenclatureController<RelationType, BaseForeignNameNomenclatureFilterDto<RelationType>>
	{
		public RelationTypeController(
			AppDbContext context,
			INomenclatureService<RelationType> nomenclatureService
			) : base(context, nomenclatureService)
		{

		}
	}

	[ApiController]
	[Route("api/[controller]")]
	public class AudienceTypeController : BaseNomenclatureController<AudienceType, BaseForeignNameNomenclatureFilterDto<AudienceType>>
	{
		public AudienceTypeController(
			AppDbContext context,
			INomenclatureService<AudienceType> nomenclatureService
			) : base(context, nomenclatureService)
		{

		}
	}
}
