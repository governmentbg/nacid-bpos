using NacidRas.Infrastructure.Data;

namespace NacidRas.Ras.AssignmentPositions
{
	public class AssignmentPositionSpeciality : IEntityVersion
	{
		public int Id { get; set; }
		public int AssignmentPositionId { get; set; }
		public int SpecialityId { get; set; }
		[Skip]
		public Speciality Speciality { get; set; }
		public int Version { get; set; }
	}
}
