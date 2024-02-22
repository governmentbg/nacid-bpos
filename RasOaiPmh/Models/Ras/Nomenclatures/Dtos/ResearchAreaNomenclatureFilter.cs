using NacidRas.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NacidRas.Ras.Nomenclatures.Dtos
{
	public class ResearchAreaNomenclatureFilter : NomenclatureFilter
	{
		public bool HasOnlyProffesionalDirection { get; set; } = false;
	}
}
