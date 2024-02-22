using NacidRas.Infrastructure.Data;
using NacidRas.Ras.Nomenclatures.Enums;

namespace NacidRas.Ras.Nomenclatures.Models
{
	public class Position : Nomenclature
	{
		public string Code { get; set; }
		public PositionType? PositionType { get; set; }
	}
}
