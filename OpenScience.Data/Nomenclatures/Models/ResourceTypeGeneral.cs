﻿using OpenScience.Data.Base.Models;
using OpenScience.Data.Nomenclatures.Interfaces;

namespace OpenScience.Data.Nomenclatures.Models
{
	public class ResourceTypeGeneral : AliasNomenclature, IForeignNameNomenclature
	{
		public string NameEn { get; set; }
	}
}
