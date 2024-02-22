using System;
using System.Linq.Expressions;
using OpenScience.Data.Base.Models;
using OpenScience.Data.Classifications.Models;

namespace MetadataPublications.Converters.Filters
{
	public class ClassificationOpenAirePropagationEnabledFilter : EntityFilter<Classification>
	{
		public ClassificationOpenAirePropagationEnabledFilter()
		{
			Limit = int.MaxValue;
		}

		public override Expression<Func<Classification, bool>> GetPredicate() =>
			classification => classification.IsOpenAirePropagationEnabled;
	}
}
