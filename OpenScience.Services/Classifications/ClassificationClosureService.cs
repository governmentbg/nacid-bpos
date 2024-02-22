using System;
using Microsoft.EntityFrameworkCore;
using OpenScience.Data;
using OpenScience.Data.Classifications.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using OpenScience.Data.Base.Models;

namespace OpenScience.Services.Classifications
{
	public class ClassificationClosureService
	{
		private readonly AppDbContext context;

		public ClassificationClosureService(AppDbContext context)
		{
			this.context = context;
		}

		public async Task<IEnumerable<ClassificationClosure>> CreateClosuresAsync(Classification classification)
		{
			var closures = new List<ClassificationClosure>();
			if (classification.ParentId.HasValue)
			{
				// After add of a new item to the context, it is not included in the db set without calling SaveChanges (db set is a query to the database).
				// The Local collection represents a local view of all Added, Unchanged, and Modified entities in this set.
				// Previously created closures are not saved to the database, so a fetch from the Local collection is required
				// Call to context.ClassificationClosures.ToList adds items from db to the Local collection
				await context.ClassificationClosures
					.Where(e => e.ChildId == classification.ParentId)
					.ToListAsync();

				closures = context.ClassificationClosures.Local
					.Where(e => e.ChildId == classification.ParentId)
					.Select(e => new ClassificationClosure {
						ParentId = e.ParentId,
						Parent = e.Parent,
						Child = classification
					})
					.ToList();
			}

			closures.Add(new ClassificationClosure {
				Parent = classification,
				Child = classification
			});

			return closures;
		}

		public async Task<IEnumerable<TResult>> GetFilteredAsync<TResult>(Expression<Func<ClassificationClosure, bool>> filterExpression, Expression<Func<ClassificationClosure, TResult>> selectExpression)
		{
			return await context
				.ClassificationClosures
				.Where(filterExpression)
				.Select(selectExpression)
				.ToListAsync();
		}
	}
}
