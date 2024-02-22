using OpenScience.Data.Base.Models;
using OpenScience.Data.Nomenclatures.Interfaces;

namespace OpenScience.Data.Nomenclatures.Models
{
	public class Language : AliasNomenclature, IForeignNameNomenclature
	{
		public string NameEn { get; set; }
	}
}
