using Microsoft.EntityFrameworkCore;
using OpenScience.Data;
using OpenScience.Data.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OpenScience.Services.Base
{
	public class BaseEntityService<T> : IEntityService<T>
			where T : Entity
	{
		protected readonly AppDbContext context;
		protected readonly DbSet<T> dbSet;

		public BaseEntityService(AppDbContext context)
		{
			this.context = context;
			this.dbSet = context.Set<T>();
		}

		public virtual async Task<IEnumerable<T>> GetAllAsync()
		{
			var items = await PrepareFilterQuery()
				.OrderBy(e => e.Id)
				.ToListAsync();

			return items;
		}

		public async Task<int> GetFilteredCountAsync(EntityFilter<T> filter)
		{
			var predicate = filter.GetPredicate();

			var count = await dbSet
				.Where(predicate)
				.CountAsync();

			return count;
		}

		public virtual async Task<IEnumerable<T>> GetFilteredAsync(EntityFilter<T> filter)
		{
			var predicate = filter.GetPredicate();

			var items = await PrepareFilterQuery()
				.Where(predicate)
				.OrderBy(e => e.Id)
				.Skip(filter.Offset)
				.Take(filter.Limit)
				.ToListAsync();

			return items;
		}

		public virtual async Task<IEnumerable<TDto>> GetFilteredAsync<TDto>(EntityFilter<T> filter, Expression<Func<T, TDto>> select)
		{
			var predicate = filter.GetPredicate();

			var items = await dbSet.AsNoTracking()
				.Where(predicate)
				.OrderBy(e => e.Id)
				.Skip(filter.Offset)
				.Take(filter.Limit)
				.Select(select)
				.ToListAsync();

			return items;
		}

		public virtual async Task<IEnumerable<TDto>> GetFilteredAsync<TDto>(EntityFilter<T> filter, Expression<Func<T, TDto>> select, Expression<Func<T, object>> orderByDesc)
		{
			var predicate = filter.GetPredicate();

			var items = await dbSet.AsNoTracking()
				.Where(predicate)
				.OrderByDescending(orderByDesc)
					.ThenByDescending(e => e.Id)
				.Skip(filter.Offset)
				.Take(filter.Limit)
				.Select(select)
				.ToListAsync();

			return items;
		}

		public virtual async Task<TDto> SingleAsync<TDto>(Expression<Func<T, bool>> filter, Expression<Func<T, TDto>> select)
		{
			var item = await dbSet.AsNoTracking()
				.OrderBy(e => e.Id)
				.Where(filter)
				.Select(select)
				.SingleAsync();

			return item;
		}

		public virtual async Task<TDto> SingleOrDefaultAsync<TDto>(Expression<Func<T, bool>> filter, Expression<Func<T, TDto>> select)
		{
			var item = await dbSet.AsNoTracking()
				.OrderBy(e => e.Id)
				.Where(filter)
				.Select(select)
				.SingleOrDefaultAsync();

			return item;
		}

		public async Task<T> SingleAsync(Expression<Func<T, bool>> criteria)
		{
			return await PrepareFilterQuery()
				.SingleOrDefaultAsync(criteria);
		}

		public virtual IQueryable<T> PrepareFilterQuery()
		{
			return dbSet.AsNoTracking();
		}

		public virtual async Task<T> GetByIdAsync(int id)
		{
			var item = await dbSet.AsNoTracking()
				.SingleOrDefaultAsync(e => e.Id == id);

			return item;
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
	}
}
