using NacidRas.Infrastructure.Data;

namespace NacidRas.Ras.Nomenclatures.Models
{
	// Storage of old diplomas.
	// Used in legacy records. Could be removed in future.
	public class DepositoryOld : Nomenclature
	{
		public string NameAlt { get; set; }
	}
}
