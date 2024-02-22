using System;
using System.Linq.Expressions;

namespace OpenScience.Data.Base.Models
{
	public abstract class EntityFilter<T>
			where T : Entity
	{
		public int Limit { get; set; } = 10;
		public int Offset { get; set; } = 0;

		public abstract Expression<Func<T, bool>> GetPredicate();
	}
}
