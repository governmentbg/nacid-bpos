using NacidRas.Ems.Models;
using NacidRas.Users.Representative.Models;
using ServerApplication.Ems.Models;
using System.Collections.Generic;

namespace NacidRas.Ras
{
	public class Application
	{
		public Application()
		{
			this.AuthorizedRepresentatives = new List<AuthorizedRepresentative>();
			this.AttachedFiles = new List<DocFileDO>();
		}

		public Person Person { get; set; }
		public AcademicDegree AcademicDegree { get; set; }
		public Dissertation Dissertation { get; set; }

		public int? LotId { get; set; }

		public AcademicRank AcademicRank { get; set; }
		public List<DocFileDO> AttachedFiles { get; set; }

		public ICollection<AuthorizedRepresentative> AuthorizedRepresentatives { get; set; }
	}
}
