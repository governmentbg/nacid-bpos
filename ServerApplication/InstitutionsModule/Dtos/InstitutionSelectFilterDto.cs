using OpenScience.Common.Linq;
using OpenScience.Data.Institutions.Models;
using ServerApplication.Base.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ServerApplication.InstitutionsModule.Dtos
{
	public class InstitutionSelectFilterDto : BaseEntitySelectFilterDto<Institution>
	{
		public List<int> InstitutionIds { get; set; } = new List<int>();

		public bool? SearchInForeignNameOnly { get; set; }

		public override Expression<Func<Institution, bool>> GetPredicate()
		{
			var predicate = PredicateBuilder.True<Institution>()
				.And(e => e.IsActive);

			if(!string.IsNullOrWhiteSpace(TextFilter))
			{
				string trimmedFilterText = TextFilter.Trim().ToLower();
				if (SearchInForeignNameOnly.HasValue && SearchInForeignNameOnly.Value)
				{
					predicate = predicate.And(e => e.NameEn.Trim().ToLower().Contains(trimmedFilterText));
				}
				else
				{
					predicate = predicate.And(e => e.Name.Trim().ToLower().Contains(trimmedFilterText)
				   || e.NameEn.Trim().ToLower().Contains(trimmedFilterText));
				}
			}

			if(InstitutionIds != null && InstitutionIds.Any())
			{
				predicate = predicate.And(e => InstitutionIds.Contains(e.Id));
			}

			return predicate;
		}
	}
}
