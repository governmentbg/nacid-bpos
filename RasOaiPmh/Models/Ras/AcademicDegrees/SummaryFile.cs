using NacidRas.Infrastructure.Data;
using FilesStorageNetCore.Models;

namespace NacidRas.Ras
{
	public class SummaryFile : AttachedFile, IEntityVersion
	{
		public int Id { get; set; }
		public int Version { get; set; }
	}
}
