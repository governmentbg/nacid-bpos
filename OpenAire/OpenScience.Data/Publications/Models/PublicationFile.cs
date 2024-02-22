using FilesStorageNetCore.Models;
using OpenScience.Data.Base.Interfaces;

namespace OpenScience.Data.Publications.Models
{
	public class PublicationFile : AttachedFile, IEntity
	{
		public int Id { get; set; }

		public int PublicationId { get; set; }
		public Publication Publication { get; set; }
	}
}
