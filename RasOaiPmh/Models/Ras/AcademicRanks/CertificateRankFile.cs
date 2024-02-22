using FilesStorageNetCore.Models;
using NacidRas.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NacidRas.Ras.AcademicRanks
{
	public class CertificateRankFile : AttachedFile, IEntityVersion
	{
		public int Id { get; set; }
		public int Version { get; set; }
	}
}
