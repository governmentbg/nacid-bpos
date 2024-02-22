using OpenScience.Data;
using OpenScience.Data.Classifications.Models;
using OpenScience.Services.Base;
using OpenScience.Services.Classifications.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace OpenScience.Services.Classifications
{
	public class ClassificationService : BaseEntityService<Classification>
	{
		private readonly ClassificationClosureService classificationClosureService;

		public ClassificationService(
			AppDbContext context,
			ClassificationClosureService classificationClosureService
			)
			: base(context)
		{
			this.classificationClosureService = classificationClosureService;
		}

		public async Task AddClassificationAsync(Classification classification)
		{
			this.Add(classification);

			var closures = await classificationClosureService.CreateClosuresAsync(classification);
			context.ClassificationClosures.AddRange(closures);
		}

		public async Task<Classification> GetHierarchy(int rootId)
		{
			var classifications = await classificationClosureService.GetFilteredAsync(
				c => c.ParentId == rootId,
				c => c.Child);

			var classification = classifications.SingleOrDefault(c => c.Id == rootId);

			return classification;
		}

		public override async Task<Classification> GetByIdAsync(int id)
		{
			return await context.Classifications
				.AsNoTracking()
				.Include(e => e.Organization)
				.Include(e => e.Parent)
					.ThenInclude(e => e.Organization)
				.Include(e => e.DefaultResourceType)
				.Include(e => e.DefaultIdentifierType)
				.Include(e => e.DefaultAccessRight)
				.Include(e => e.DefaultLicenseCondition)
				.SingleOrDefaultAsync(e => e.Id == id);
		}

		public IEnumerable<FlatClassificationHierarchyItemDto> BuildFlatHierarchy(decimal parentViewOrder, IEnumerable<Classification> classifications, int level)
		{
			var result = new List<FlatClassificationHierarchyItemDto>();

			var orderedClassifications = classifications.OrderBy(e => e.OrganizationId)
				.ThenBy(e => e.Id);

			int count = 1;
			foreach (var item in orderedClassifications)
			{
				var element = new FlatClassificationHierarchyItemDto {
					Id = item.Id,
					ParentId = item.ParentId,
					ViewOrder = level == 0 ? count : parentViewOrder + (decimal)(Math.Pow(10, (-2) * level)) * count,
					Name = item.Name,
					Level = level,
					HasChildren = item.Children != null && item.Children.Any()
				};
				result.Add(element);

				result.AddRange(BuildFlatHierarchy(element.ViewOrder, item.Children.OrderBy(e => e.Id), element.Level + 1));
				count++;
			}

			return result.OrderBy(e => e.ViewOrder);
		}

		public async Task<bool> IsEditableClassification(int id)
		{
			Classification classification = await context.Classifications
				.AsNoTracking()
				.Include(e => e.Publications)
				.Include(e => e.Children)
				.SingleOrDefaultAsync(e => e.Id == id);
			if (classification == null || classification.Publications.Any() || classification.Children.Any())
			{
				return false;
			}

			return true;
		}

		public async Task DeleteAsync(int id)
		{
			List<ClassificationClosure> closures = await context.ClassificationClosures
				.Where(e => e.ParentId == id || e.ChildId == id)
				.ToListAsync();
			context.ClassificationClosures.RemoveRange(closures);

			Classification classification = await context.Classifications
				.SingleAsync(e => e.Id == id);
			context.Classifications.Remove(classification);
		}

		public override IQueryable<Classification> PrepareFilterQuery()
		{
			return base.PrepareFilterQuery()
				.Include(c => c.Children)
				.ThenInclude(c => c.Children)
				.Include(c => c.Parent);
		}
	}
}
