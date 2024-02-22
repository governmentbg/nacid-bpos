using OpenScience.Common.Linq;
using OpenScience.Data.Base.Models;
using OpenScience.Data.Publications.Enums;
using OpenScience.Data.Publications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ServerApplication.PublicationsModule.Dtos
{
	public class PublicationSearchFilter : EntityFilter<Publication>
	{
		public string Title { get; set; }

		public string CreatorFirstName { get; set; }
		public string CreatorLastName { get; set; }

		public PublicationStatus? Status { get; set; }

		public int? CreatedByUserId { get; set; }

		public int? ModeratorInstitutionId { get; set; }

		public bool FilterByInstitutions { get; set; }
		public List<int> InstitutionIds { get; set; } = new List<int>();

		public override Expression<Func<Publication, bool>> GetPredicate()
		{
			var predicate = PredicateBuilder.True<Publication>();

			if(CreatedByUserId.HasValue)
			{
				predicate = predicate.And(e => e.CreatedByUserId == CreatedByUserId.Value);
			}

			if(this.FilterByInstitutions)
			{
				predicate = predicate.And(e => InstitutionIds.Contains(e.ModeratorInstitutionId.Value));
			}

			if(!string.IsNullOrWhiteSpace(Title))
			{
				predicate = predicate.And(e => e.Titles.Any(t => t.Value.Trim().ToLower().Contains(Title.Trim().ToLower())));
			}

			if (!string.IsNullOrWhiteSpace(CreatorFirstName))
			{
				predicate = predicate.And(e => e.Creators.Any(c => c.FirstName.Trim().ToLower().Contains(CreatorFirstName.Trim().ToLower())));
			}

			if (!string.IsNullOrWhiteSpace(CreatorLastName))
			{
				predicate = predicate.And(e => e.Creators.Any(c => c.LastName.Trim().ToLower().Contains(CreatorLastName.Trim().ToLower())));
			}

			if (Status.HasValue)
			{
				predicate = predicate.And(e => e.Status == Status.Value);
			}

			if(ModeratorInstitutionId.HasValue)
			{
				predicate = predicate.And(e => e.ModeratorInstitutionId == ModeratorInstitutionId);
			}

			return predicate;
		}
	}
}
