using OpenScience.Common.Linq;
using OpenScience.Data.Base.Interfaces;
using OpenScience.Data.Base.Models;
using System;
using System.Linq.Expressions;

namespace ServerApplication.NomenclaturesModule.Dtos
{
	public class BaseNomenclatureFilterDto<T> : IBaseNomenclatureFilter<T>
		where T: Nomenclature
	{
		public int? Limit { get; set; }
		public int? Offset { get; set; }

		public string TextFilter { get; set; }

		public virtual Expression<Func<T, bool>> GetPredicate()
		{
			var predicate = PredicateBuilder.True<T>();

			if(!string.IsNullOrWhiteSpace(TextFilter))
			{
				predicate = predicate.And(e => e.Name.Trim().ToLower().Contains(TextFilter.Trim().ToLower()));
			}

			return predicate;
		}
	}
}
