using FilesStorageNetCore.Models;
using NacidRas.Infrastructure.Data;

namespace NacidRas.Ras
{
	public class DiplomaFile : AttachedFile, IEntityVersion
	{
		public int Id { get; set; }
		public int Version { get; set; }
	}
}
