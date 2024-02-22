using Microsoft.EntityFrameworkCore;
using OpenScience.Data;
using OpenScience.Data.Base.Interfaces;
using OpenScience.Data.Base.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenScience.Services.Base
{
	public class BaseNomenclatureService<T> : INomenclatureService<T>
			where T : Nomenclature
	{
		protected readonly AppDbContext context;
		protected readonly DbSet<T> dbSet;

		public BaseNomenclatureService(AppDbContext context)
		{
			this.context = context;
			this.dbSet = context.Set<T>();
		}

		public virtual async Task<IEnumerable<T>> GetFilteredAsync(IBaseNomenclatureFilter<T> filter)
		{
			IQueryable<T> query = dbSet.AsNoTracking();
			if (filter != null)
			{
				query = query.Where(filter.GetPredicate());
			}

			query = query
				.OrderBy(e => e.ViewOrder)
					.ThenBy(e => e.Id);
			if(filter.Offset.HasValue && filter.Limit.HasValue)
			{
				query = query
					.Skip(filter.Offset.Value)
					.Take(filter.Limit.Value);
			}
			
			return await query.ToListAsync();
		}

		public virtual T Add(T entity)
		{
			dbSet.Attach(entity);
			context.Entry(entity).State = EntityState.Added;

			return entity;
		}

		public virtual T Update(T entity)
		{
			dbSet.Attach(entity);
			context.Entry(entity).State = EntityState.Modified;

			return entity;
		}

		public virtual async Task DeleteAsync(int id)
		{
			T removeEntity = await dbSet.SingleOrDefaultAsync(e => e.Id == id);
			if (removeEntity == null)
			{
				return;
			}

			dbSet.Remove(removeEntity);
		}
	}
}
