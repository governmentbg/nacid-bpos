using FilesStorageNetCore.Models;
using NacidRas.Infrastructure.Data;

namespace NacidRas.Ras.Files
{
	public class EmploymentContractFile : AttachedFile, IEntityVersion
	{
		public int Id { get; set; }
		public int Version { get; set; }
	}
}
