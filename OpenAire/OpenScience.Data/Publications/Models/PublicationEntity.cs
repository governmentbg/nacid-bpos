using OpenScience.Data.Base.Models;

namespace OpenScience.Data.Publications.Models
{
	public abstract class PublicationEntity : Entity
	{
		public int PublicationId { get; set; }
		public Publication Publication { get; set; }

		public int? ViewOrder { get; set; }
	}
}
