using Microsoft.EntityFrameworkCore;
using OpenScience.Common.Linq;
using OpenScience.Data;
using OpenScience.Data.Base.Models;
using OpenScience.Data.Classifications.Models;
using OpenScience.Data.Institutions.Models;
using OpenScience.Services.Base;
using OpenScience.Services.Classifications;
using OpenScience.Services.Classifications.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenScience.Services.Institutions
{
	public class InstitutionService : BaseEntityService<Institution>
	{
		private readonly ClassificationClosureService classificationClosureService;
		private readonly ClassificationService classificationService;

		public InstitutionService(
			AppDbContext context,
			ClassificationClosureService classificationClosureService,
			ClassificationService classificationService
			)
			: base(context)
		{
			this.classificationClosureService = classificationClosureService;
			this.classificationService = classificationService;
		}

		public async Task<IEnumerable<FlatClassificationHierarchyItemDto>> GetInstitutionClassifications(int institutionId)
		{
			Institution institution = await SingleAsync(e => e.Id == institutionId);

			var predicate = PredicateBuilder.True<ClassificationClosure>();
			predicate = predicate.And(e => e.Parent.OrganizationId == institutionId);
			if(institution.AreCommonClassificationsVisible)
			{
				predicate = predicate.Or(e => e.Parent.OrganizationId == null);
			}

			var classifications = await classificationClosureService.GetFilteredAsync(
				predicate,
				e => e.Child
			);

			return classificationService.BuildFlatHierarchy(1, classifications.Where(e => e.ParentId == null), 0);
		}
	}
}
