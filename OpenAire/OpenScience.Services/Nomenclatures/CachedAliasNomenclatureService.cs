using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using OpenScience.Data.Base.Models;

namespace OpenScience.Services.Nomenclatures
{
	public class CachedAliasNomenclatureService : IAliasNomenclatureService
	{
		private readonly IAliasNomenclatureService nomenclatureService;
		private readonly IMemoryCache cache;

		public CachedAliasNomenclatureService(IAliasNomenclatureService nomenclatureService, IMemoryCache cache)
		{
			this.nomenclatureService = nomenclatureService;
			this.cache = cache;
		}

		public async Task<TAliasNomenclature> FindByAliasAsync<TAliasNomenclature>(string alias)
			where TAliasNomenclature : AliasNomenclature
		{
			if (string.IsNullOrEmpty(alias))
			{
				return null;
			}

			var key = GetCacheKey<TAliasNomenclature>(alias);

			var nomenclature = cache.Get<TAliasNomenclature>(key);

			if (nomenclature == null)
			{
				nomenclature = await nomenclatureService.FindByAliasAsync<TAliasNomenclature>(alias);

				if (nomenclature != null)
				{
					cache.Set(key, nomenclature);
				}
			}

			return nomenclature;
		}

		public TAliasNomenclature FindByAlias<TAliasNomenclature>(string alias)
			where TAliasNomenclature : AliasNomenclature
		{
			if (string.IsNullOrEmpty(alias))
			{
				return null;
			}

			var key = GetCacheKey<TAliasNomenclature>(alias);

			var nomenclature = cache.Get<TAliasNomenclature>(key);

			if (nomenclature == null)
			{
				nomenclature = nomenclatureService.FindByAlias<TAliasNomenclature>(alias);

				if (nomenclature != null)
				{
					cache.Set(key, nomenclature);
				}
			}

			return nomenclature;
		}

		private string GetCacheKey<TAliasNomenclature>(string alias)
			where TAliasNomenclature : AliasNomenclature
			=> $"{typeof(TAliasNomenclature).Name}|{alias}";
	}
}
