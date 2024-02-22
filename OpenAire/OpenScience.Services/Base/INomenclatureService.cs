using OpenScience.Data.Base.Interfaces;
using OpenScience.Data.Base.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenScience.Services.Base
{
	public interface INomenclatureService<T>
			where T : Nomenclature
	{
		Task<IEnumerable<T>> GetFilteredAsync(IBaseNomenclatureFilter<T> filter);

		T Add(T entity);

		T Update(T entity);

		Task DeleteAsync(int id);
	}
}
