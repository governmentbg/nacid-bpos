using OpenScience.Common.Linq;
using OpenScience.Data.Base.Models;
using OpenScience.Data.Classifications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ServerApplication.ClassificationsModule.Dtos
{
	public class ClassificationSearchFilter : EntityFilter<Classification>
	{
		public int? ParentId { get; set; }

		public List<int> OrganizationIds { get; set; } = new List<int>();

		public override Expression<Func<Classification, bool>> GetPredicate()
		{
			var predicate = PredicateBuilder.True<Classification>();

			predicate = predicate.And(e => e.ParentId == ParentId);

			if(OrganizationIds.Any())
			{
				predicate = predicate.And(e => !e.OrganizationId.HasValue || OrganizationIds.Contains(e.OrganizationId.Value));
			}

			return predicate;
		}

		public ClassificationSearchFilter(int? parentId = null)
		{
			this.ParentId = parentId;
			this.Limit = int.MaxValue;
		}
	}
}
