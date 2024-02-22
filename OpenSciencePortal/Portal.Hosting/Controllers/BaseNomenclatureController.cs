using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenScience.Common.Linq;
using OpenScience.Data;
using OpenScience.Data.Base.Interfaces;
using OpenScience.Data.Base.Models;
using OpenScience.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Portal.Hosting
{
	public class BaseNomenclatureFilterDto<T> : IBaseNomenclatureFilter<T>
		where T: Nomenclature
	{
		public string TextFilter { get; set; }
		public int? Limit { get; set; }
		public int? Offset { get; set; }

		public virtual Expression<Func<T, bool>> GetPredicate()
		{
			var predicate = PredicateBuilder.True<T>();

			if (!string.IsNullOrWhiteSpace(TextFilter))
			{
				predicate = predicate.And(e => e.Name.Trim().ToLower().Contains(TextFilter.Trim().ToLower()));
			}

			return predicate;
		}
	}

	public abstract class BaseNomenclatureController<T> : ControllerBase
		where T : Nomenclature
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
		public async Task<IEnumerable<T>> GetFiltered([FromQuery]BaseNomenclatureFilterDto<T> filter)
		{
			return await nomenclatureService
				.GetFilteredAsync(filter);
		}

		[HttpGet("{id:int}")]
		public async Task<T> GetById(int id)
		{
			var nomenclature = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
			return nomenclature;
		}
	}
}
