using OpenScience.Common.Linq;
using OpenScience.Data.Base.Models;
using System;
using System.Linq.Expressions;

namespace ServerApplication.Base.Dtos
{
	public class BaseEntitySelectFilterDto<T>
			where T : Entity
	{
		public int? Limit { get; set; }
		public int? Offset { get; set; }

		public string TextFilter { get; set; }

		public virtual Expression<Func<T, bool>> GetPredicate()
		{
			var predicate = PredicateBuilder.True<T>();

			return predicate;
		}
	}
}
