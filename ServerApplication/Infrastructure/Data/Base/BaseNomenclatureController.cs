using Microsoft.AspNetCore.Mvc;
using OpenScience.Data;
using OpenScience.Data.Base.Models;
using OpenScience.Services.Base;
using ServerApplication.NomenclaturesModule.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerApplication.Infrastructure.Data
{
	public abstract class BaseNomenclatureController<T, TFilter> : ControllerBase
		where T : Nomenclature
		where TFilter: BaseNomenclatureFilterDto<T>
	{
		protected readonly AppDbContext context;
		protected readonly INomenclatureService<T> nomenclatureService;

		public BaseNomenclatureController(
			AppDbContext context,
			INomenclatureService<T> nomenclatureService
			)
		{
			this.context = context;
			this.nomenclatureService = nomenclatureService; 
		}

		[HttpGet("")]
		public async Task<IEnumerable<T>> GetFiltered([FromQuery]TFilter filter)
		{
			return await nomenclatureService.GetFilteredAsync(filter);
		}

		[HttpPost("")]
		public async Task<T> Add([FromBody]T entity)
		{
			nomenclatureService.Add(entity);
			await context.SaveChangesAsync();

			return entity;
		}

		[HttpPut("{id:int}")]
		public async Task<T> Update([FromRoute]int id, [FromBody]T entity)
		{
			nomenclatureService.Update(entity);
			await context.SaveChangesAsync();

			return entity;
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete([FromRoute]int id)
		{
			await nomenclatureService.DeleteAsync(id);
			await context.SaveChangesAsync();

			return Ok();
		}
	}
}
