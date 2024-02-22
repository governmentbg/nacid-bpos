using System.Threading.Tasks;
using OpenScience.Data.Base.Models;

namespace OpenScience.Services.Nomenclatures
{
	public interface IAliasNomenclatureService
	{
		Task<TAliasNomenclature> FindByAliasAsync<TAliasNomenclature>(string alias)
			where TAliasNomenclature : AliasNomenclature;

		TAliasNomenclature FindByAlias<TAliasNomenclature>(string alias)
			where TAliasNomenclature : AliasNomenclature;
	}
}
