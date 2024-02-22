using System.Collections.Generic;
using System.Linq;
using System.Text;
using NacidRas.Integrations.OaiPmhProvider.Contracts;
using NacidRas.Integrations.OaiPmhProvider.Models;
using NacidRas.Ras;

namespace NacidRas.Integrations.OaiPmhProvider.Repositories
{
	public class ResearchAreaSetRepository : ISetRepository
	{
		private readonly RasDbContext context;

		public ResearchAreaSetRepository(RasDbContext context)
		{
			this.context = context;
		}

		public ListContainer<Set> GetSets(ArgumentContainer arguments, ResumptionToken resumptionToken = null)
		{
			var areas = context
				.ResearchAreas
				.Where(ra => ra.IsActive)
				.ToList();

			var areaLookup = areas
				.ToDictionary(ra => ra.Id, ra => ra);

			var sets = areas
				.Select(a => new Set { Name = a.Name, Spec = BuildSpec(areaLookup, a) })
				.ToList();

			return new ListContainer<Set> { Items = sets };
		}

		private string BuildSpec(IDictionary<int, ResearchArea> areaLookup, ResearchArea researchArea)
		{
			var builder = new StringBuilder(researchArea.Name);

			if (researchArea.ParentId.HasValue)
			{
				while (researchArea.ParentId.HasValue && areaLookup.ContainsKey(researchArea.ParentId.Value))
				{
					researchArea = areaLookup[researchArea.ParentId.Value];

					builder.Insert(0, $"{researchArea.Name}:");
				}
			}

			return builder.ToString();
		}
	}
}
