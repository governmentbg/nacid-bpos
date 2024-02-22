using FilesStorageNetCore.Models;
using NacidRas.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NacidRas.Ras.Files
{
	public class AcademicRankFile : AttachedFile, IEntityVersion
	{
		public int Id { get; set; }
		public int AcademicRankId { get; set; }
		public string Description { get; set; }
		public int Version { get; set; }
	}
}
