using OpenScience.Common.Linq;
using OpenScience.Data.Base.Models;
using OpenScience.Data.Nomenclatures.Interfaces;
using System;
using System.Linq.Expressions;

namespace ServerApplication.NomenclaturesModule.Dtos
{
	public class BaseForeignNameNomenclatureFilterDto<T> : BaseNomenclatureFilterDto<T>
			where T : Nomenclature, IForeignNameNomenclature
	{
		public bool? SearchInForeignNameOnly { get; set; }

		public override Expression<Func<T, bool>> GetPredicate()
		{
			var predicate = PredicateBuilder.True<T>();

			if (!string.IsNullOrWhiteSpace(TextFilter))
			{
				string trimmedFilter = TextFilter.Trim().ToLower();
				if (SearchInForeignNameOnly.HasValue && SearchInForeignNameOnly.Value)
				{
					predicate = predicate.And(e => e.NameEn.Trim().ToLower().Contains(trimmedFilter));
				}
				else
				{
					predicate = predicate.And(e => e.Name.Trim().ToLower().Contains(trimmedFilter)
					|| e.NameEn.Trim().ToLower().Contains(trimmedFilter));
				}
			}

			return predicate;
		}
	}
}
