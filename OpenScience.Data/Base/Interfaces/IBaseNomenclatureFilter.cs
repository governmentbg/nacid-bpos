using OpenScience.Data.Base.Models;
using System;
using System.Linq.Expressions;

namespace OpenScience.Data.Base.Interfaces
{
	public interface IBaseNomenclatureFilter<T>
		where T: Nomenclature
	{
		int? Limit { get; set; }
		int? Offset { get; set; }

		string TextFilter { get; set; }

		Expression<Func<T, bool>> GetPredicate();
	}
}
