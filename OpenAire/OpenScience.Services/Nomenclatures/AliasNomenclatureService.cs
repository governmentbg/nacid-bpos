using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenScience.Data;
using OpenScience.Data.Base.Models;

namespace OpenScience.Services.Nomenclatures
{
	public class AliasNomenclatureService : IAliasNomenclatureService
	{
		private readonly AppDbContext context;

		public AliasNomenclatureService(AppDbContext context)
		{
			this.context = context;
		}

		public async Task<TAliasNomenclature> FindByAliasAsync<TAliasNomenclature>(string alias)
			where TAliasNomenclature : AliasNomenclature
		{
			if (string.IsNullOrEmpty(alias))
			{
				return null;
			}

			return await context.Set<TAliasNomenclature>()
				.AsNoTracking()
				.SingleOrDefaultAsync(n => n.Alias == alias);
		}

		public TAliasNomenclature FindByAlias<TAliasNomenclature>(string alias)
			where TAliasNomenclature : AliasNomenclature
		{
			if (string.IsNullOrEmpty(alias))
			{
				return null;
			}

			return context.Set<TAliasNomenclature>()
				.AsNoTracking()
				.SingleOrDefault(n => n.Alias == alias);
		}
	}
}
