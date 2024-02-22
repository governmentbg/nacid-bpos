using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetadataHarvesting.Core.Services;
using MetadataHarvesting.Models;
using MetadataPublications.Converters.Filters;
using MetadataPublications.Converters.Models;
using OpenScience.Data.Classifications.Models;
using OpenScience.Services.Classifications;

namespace MetadataPublications.Converters
{
	public class SetSpecClassificationService
	{
		private readonly ClassificationService classificationService;
		private readonly SetSpecService setSpecService;

		public SetSpecClassificationService(ClassificationService classificationService, SetSpecService setSpecService)
		{
			this.classificationService = classificationService;
			this.setSpecService = setSpecService;
		}

		public async Task<IList<Classification>> GetClassificationsFromSetSpecs(IEnumerable<string> setSpecs, ClassificationHierarchyItem rootClassification)
		{
			var classifications = new List<Classification>();
			foreach (var setSpec in setSpecs)
			{
				var classification = await GetClassificationFromSetSpec(setSpec, rootClassification);
				classifications.Add(classification);
			}

			return classifications;
		}

		public async Task<Classification> GetClassificationFromSetSpec(string setSpec, ClassificationHierarchyItem root)
		{
			var sets = setSpecService.GetSets(setSpec);

			foreach (var setName in sets)
			{
				if (!root.Children.ContainsKey(setName))
				{
					var parentClassification = root.Classification;

					var classification = new Classification {
						Name = setName,
						SetName = setName,
						ParentId = parentClassification.Id,
						IsReadonly = parentClassification.IsReadonly,
						IsOpenAirePropagationEnabled = parentClassification.IsOpenAirePropagationEnabled,
						OrganizationId = parentClassification.OrganizationId,
						IsFromHarvesting = true
					};

					await classificationService.AddClassificationAsync(classification);

					var child = new ClassificationHierarchyItem(classification);

					root.Children.Add(setName, child);
				}

				root = root.Children[setName];
			}

			return root.Classification;
		}

		public async Task<ClassificationHierarchyItem> BuildClassificationHierarchy(int rootId)
		{
			var rootClassification = await classificationService.GetHierarchy(rootId);

			var root = new ClassificationHierarchyItem(rootClassification);

			return root;
		}

		public async Task<Classification> GetClassificationBySetSpec(string setSpec)
		{
			var sets = setSpecService.GetSets(setSpec);

			var classificationList = await classificationService.GetFilteredAsync(new ClassificationSetNameFilter(sets));

			var parentId = (int?)null;
			var classification = (Classification)null;

			foreach (var set in sets)
			{
				classification = classificationList.SingleOrDefault(c => c.Name == set && c.ParentId == parentId);

				if (classification == null)
				{
					break;
				}

				parentId = classification.Id;
			}

			return classification;
		}

		public async Task<IEnumerable<Set>> GetSets()
		{
			var classifications = await classificationService.GetFilteredAsync(new ClassificationOpenAirePropagationEnabledFilter());

			var sets = classifications
				.Select(c => new Set { Name = c.Name, Spec = BuildSetSpec(c) })
				.ToList();

			return sets;
		}

		public async Task<IDictionary<int, string>> CreateLookupForClassificationSetSpecs(IEnumerable<int> classificationIds)
		{
			var classificationIdMap = classificationIds.ToHashSet();

			var lookup = (await classificationService.GetAllAsync())
				.Where(c => classificationIdMap.Contains(c.Id))
				.ToDictionary(c => c.Id, BuildSetSpec);

			return lookup;
		}

		public string BuildSetSpec(Classification classification)
		{
			var builder = new StringBuilder(classification.Name);

			while (classification.Parent != null)
			{
				classification = classification.Parent;

				builder.Insert(0, $"{classification.Name}:");
			}

			return builder.ToString();
		}
	}
}
