using OpenScience.Data.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OpenScience.Services.Base
{
	public interface IEntityService<T>
			where T : Entity
	{
		Task<IEnumerable<T>> GetAllAsync();

		Task<int> GetFilteredCountAsync(EntityFilter<T> filter);

		Task<IEnumerable<T>> GetFilteredAsync(EntityFilter<T> filter);

		Task<IEnumerable<TDto>> GetFilteredAsync<TDto>(EntityFilter<T> filter, Expression<Func<T, TDto>> select);

		Task<T> SingleAsync(Expression<Func<T, bool>> criteria);

		Task<T> GetByIdAsync(int id);

		T Add(T entity);

		T Update(T entity);
	}
}
