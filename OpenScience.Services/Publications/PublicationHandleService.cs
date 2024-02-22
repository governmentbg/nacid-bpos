using OpenScience.Common.Constants;
using OpenScience.Data.Nomenclatures.Models;
using OpenScience.Data.Publications.Models;
using OpenScience.Handle;
using OpenScience.Services.Nomenclatures;
using System.Threading.Tasks;

namespace OpenScience.Services.Publications
{
	public class PublicationHandleService
	{
		private readonly IHandleServerAdapter handleService;
		private readonly IAliasNomenclatureService nomenclatureService;

		public PublicationHandleService(
			IHandleServerAdapter handleService,
			IAliasNomenclatureService nomenclatureService)
		{
			this.handleService = handleService;
			this.nomenclatureService = nomenclatureService;
		}

		public async Task<Publication> SetPublicationHandle(string publicationPreviewLinkTemplate, Publication publication)
		{
			var url = string.Format(publicationPreviewLinkTemplate, publication.Id);
			var response = await handleService.CreateUrlHandleAsync(url);

			if (!string.IsNullOrEmpty(response?.Handle))
			{
				publication.Identifier = response.Handle;

				publication.IdentifierTypeId = (await nomenclatureService.FindByAliasAsync<ResourceIdentifierType>(ResourceIdentitfierAliases.HANDLE)).Id;
			}

			return publication;
		}
	}
}
