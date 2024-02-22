using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OpenScience.Common.Linq;
using OpenScience.Data.Base.Models;
using OpenScience.Data.Users.Models;

namespace ServerApplication.UsersModule.Dtos
{
	public class UserSearchFilterDto : EntityFilter<User>
	{
		public string Username { get; set; }
		public string Name { get; set; }

		public int? RoleId { get; set; }

		public List<int> InstitutionIds { get; set; }

		public override Expression<Func<User, bool>> GetPredicate()
		{
			var predicate = PredicateBuilder.True<User>();
			if (!string.IsNullOrWhiteSpace(Username))
			{
				predicate = predicate.And(e => e.Username.Trim().ToLower().Contains(Username.Trim().ToLower()));
			}

			if (!string.IsNullOrWhiteSpace(Name))
			{
				predicate = predicate.And(e => e.Fullname.Trim().ToLower().Contains(Name.Trim().ToLower()));
			}

			if(RoleId.HasValue)
			{
				predicate = predicate.And(e => e.RoleId == RoleId.Value);
			}

			if(InstitutionIds != null && InstitutionIds.Any())
			{
				predicate = predicate.And(e => e.Institutions.Any(i => InstitutionIds.Contains(i.InstitutionId)));
			}

			return predicate;
		}
	}
}
