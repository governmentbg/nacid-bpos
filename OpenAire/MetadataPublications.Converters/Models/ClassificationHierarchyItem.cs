using System.Collections.Generic;
using System.Linq;
using OpenScience.Data.Classifications.Models;

namespace MetadataPublications.Converters.Models
{
	public class ClassificationHierarchyItem
	{
		public ClassificationHierarchyItem(Classification classification)
		{
			Classification = classification;
			Children = classification.Children.ToDictionary(c => c.SetName, c => new ClassificationHierarchyItem(c));
		}

		public Classification Classification { get; set; }

		public IDictionary<string, ClassificationHierarchyItem> Children { get; }
	}
}
