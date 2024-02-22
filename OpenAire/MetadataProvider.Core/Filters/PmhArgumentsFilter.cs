using System;
using System.Linq;
using System.Linq.Expressions;
using OpenScience.Common.Linq;
using OpenScience.Data.Base.Models;
using OpenScience.Data.Publications.Enums;
using OpenScience.Data.Publications.Models;

namespace MetadataProvider.Core.Filters
{
	public class PmhArgumentsFilter : EntityFilter<Publication>
	{
		public DateTime? From { get; set; }
		public DateTime? Until { get; set; }
		public int? ClassificationId { get; set; }

		public override Expression<Func<Publication, bool>> GetPredicate()
		{
			var predicate = PredicateBuilder.True<Publication>();

			predicate = predicate
				.And(p => p.Status == PublicationStatus.Published)
				.And(p => p.Classifications.Any(c => c.Classification.IsOpenAirePropagationEnabled));

			if (From.HasValue)
			{
				predicate = predicate
					.And(p => p.ModificationDate >= From.Value);
			}

			if (Until.HasValue)
			{
				predicate = predicate
					.And(p => p.ModificationDate <= Until.Value);
			}

			if (ClassificationId.HasValue)
			{
				predicate = predicate
					.And(p => p.Classifications.Any(c => c.ClassificationId == ClassificationId.Value));
			}

			return predicate;
		}
	}
}
