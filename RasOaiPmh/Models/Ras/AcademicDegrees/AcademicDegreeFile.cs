using FilesStorageNetCore.Models;
using NacidRas.Infrastructure.Data;

namespace NacidRas.Ras
{
	public class AcademicDegreeFile : AttachedFile, IEntityVersion
	{
		// we need this because of migration
		public int Id { get; set; }
		public int AcademicDegreeId { get; set; }
		public string Description { get; set; }
		public int Version { get; set; }
	}
}
