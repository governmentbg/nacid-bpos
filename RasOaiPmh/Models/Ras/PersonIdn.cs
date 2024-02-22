using NacidRas.Infrastructure.Data;

namespace NacidRas.Ras
{
	public class PersonIdn : EntityVersion
	{
		public int PersonId { get; set; }
		public int InstitutionId { get; set; }
		[Skip]
		public Institution Institution { get; set; }
		public string IdnNumber { get; set; }
	}
}
