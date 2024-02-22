using System.Linq;
using System.Threading.Tasks;
using MetadataHarvesting.Models;
using MetadataPublications.Converters;

namespace MetadataProvider.Core.Repositories
{
	public class ClassificationSetRepository : ISetRepository
	{
		private readonly SetSpecClassificationService setSpecClassificationService;

		public ClassificationSetRepository(SetSpecClassificationService setSpecClassificationService)
		{
			this.setSpecClassificationService = setSpecClassificationService;
		}

		public async Task<ListContainer<Set>> GetSetsAsync(ArgumentContainer arguments, ResumptionToken resumptionToken = null)
		{
			var sets = await setSpecClassificationService.GetSets();

			return new ListContainer<Set> { Items = sets.ToList() };
		}
	}
}
