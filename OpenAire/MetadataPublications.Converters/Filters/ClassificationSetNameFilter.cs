using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OpenScience.Data.Base.Models;
using OpenScience.Data.Classifications.Models;

namespace MetadataPublications.Converters.Filters
{
	public class ClassificationSetNameFilter : EntityFilter<Classification>
	{
		public ClassificationSetNameFilter(IEnumerable<string> classificationNames)
		{
			ClassificationNames = classificationNames ?? new List<string>();
		}

		public IEnumerable<string> ClassificationNames { get; set; }

		public override Expression<Func<Classification, bool>> GetPredicate() => classification => ClassificationNames.Contains(classification.Name);
	}
}
