using OpenScience.Data.Base.Interfaces;

namespace OpenScience.Data.Base.Models
{
	public class Entity : IEntity, IConcurrency
	{
		public int Id { get; set; }
		public int Version { get; set; } = 0;

		public void IncrementVersion()
		{
			this.Version++;
		}
	}
}
