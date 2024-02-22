using OpenScience.Common.Linq;
using OpenScience.Data.Base.Models;
using OpenScience.Data.Institutions.Models;
using System;
using System.Linq.Expressions;

namespace ServerApplication.InstitutionsModule.Dtos
{
	public class InstitutionSearchFilter : EntityFilter<Institution>
	{
		public string Name { get; set; }

		public string RepositoryUrl { get; set; }

		public bool? SearchInForeignNameOnly { get; set; }

		public override Expression<Func<Institution, bool>> GetPredicate()
		{
			var predicate = PredicateBuilder.True<Institution>();

			if (!string.IsNullOrWhiteSpace(Name))
			{
				var trimmedName = this.Name.Trim().ToLower();
				if (SearchInForeignNameOnly.HasValue && SearchInForeignNameOnly.Value)
				{
					predicate = predicate.And(e => e.NameEn.Trim().ToLower().Contains(trimmedName));
				}
				else
				{
					predicate = predicate.And(e => e.Name.Trim().ToLower().Contains(trimmedName)
						|| e.NameEn.Trim().ToLower().Contains(trimmedName));
				}
			}

			if (!string.IsNullOrWhiteSpace(RepositoryUrl))
			{
				predicate = predicate.And(e => e.RepositoryUrl.Trim().ToLower().Contains(this.RepositoryUrl.Trim().ToLower()));
			}

			return predicate;
		}
	}
}
