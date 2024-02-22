namespace OpenScience.Data.Base.Interfaces
{
	public interface IConcurrency
	{
		int Version { get; set; }

		void IncrementVersion();
	}
}
