using Microsoft.AspNetCore.Mvc;
using OpenScience.Data;
using OpenScience.Data.Nomenclatures.Models;
using OpenScience.Services.Base;

namespace Portal.Hosting
{
	[ApiController]
	[Route("api/[controller]")]
	public class ResourceTypeController : BaseNomenclatureController<ResourceType>
	{
		public ResourceTypeController(
			AppDbContext context,
			INomenclatureService<ResourceType> nomenclatureService
			) : base(context, nomenclatureService)
		{

		}
	}

	[ApiController]
	[Route("api/[controller]")]
	public class LanguageController : BaseNomenclatureController<Language>
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
	public class AccessRightController : BaseNomenclatureController<AccessRight>
	{
		public AccessRightController(
			AppDbContext context,
			INomenclatureService<AccessRight> nomenclatureService
			) : base(context, nomenclatureService)
		{

		}
	}
}
