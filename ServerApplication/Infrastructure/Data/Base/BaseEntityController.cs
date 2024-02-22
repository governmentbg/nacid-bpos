using Microsoft.AspNetCore.Mvc;
using OpenScience.Data;
using OpenScience.Data.Base.Models;
using OpenScience.Services.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerApplication.Infrastructure.Data
{
	public abstract class BaseEntityController<T, TFilter> : ControllerBase
		where T : Entity
		where TFilter : EntityFilter<T>
	{
		protected readonly AppDbContext context;
		protected readonly IEntityService<T> entityService;

		public BaseEntityController(
			AppDbContext context,
			IEntityService<T> entityService)
		{
			this.context = context;
			this.entityService = entityService;
		}

		[HttpGet]
		public virtual async Task<IEnumerable<T>> GetFiltered([FromQuery]TFilter filter)
		{
			return await entityService.GetFilteredAsync(filter);
		}

		[HttpGet("{id:int}")]
		public async Task<T> GetById([FromRoute]int id)
		{
			return await entityService.GetByIdAsync(id);
		}

		[HttpPost]
		public async Task<T> Post(T entity)
		{
			entityService.Add(entity);
			await context.SaveChangesAsync();

			return entity;
		}

		[HttpPut("{id:int}")]
		public async Task<T> Put(T entity)
		{
			entityService.Update(entity);
			await context.SaveChangesAsync();

			return entity;
		}
	}
}
