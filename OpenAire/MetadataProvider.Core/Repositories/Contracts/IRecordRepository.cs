using System.Threading.Tasks;
using MetadataHarvesting.Models;

namespace MetadataProvider.Core
{
	/// <summary>
	/// The interface representing the repository for records.
	/// </summary>
	public interface IRecordRepository
	{
		/// <summary>
		/// Gets the record corresponding to the provided identifier.
		/// </summary>
		/// <param name="identifier">The identifier to specify the record.</param>
		/// <param name="metadataPrefix">The prefix to specify the metadata format.</param>
		/// <returns>The record corresponding to the provided identifier and prefix.</returns>
		Task<Record> GetRecord(string identifier, string metadataPrefix);

		/// <summary>
		/// Gets an incomplete list of records corresponding to the provided arguments and resumption token.
		/// </summary>
		/// <param name="arguments">The OAI-PMH arguments.</param>
		/// <param name="resumptionToken">The optional token used for resumption.</param>
		/// <returns>An incomplete list of records corresponding to the provided arguments and resumption token.</returns>
		Task<ListContainer<Record>> GetRecords(ArgumentContainer arguments, ResumptionToken resumptionToken = null);

		/// <summary>
		/// Gets an incomplete list of identifiers corresponding to the provided arguments and resumption token.
		/// </summary>
		/// <param name="arguments">The OAI-PMH arguments.</param>
		/// <param name="resumptionToken">The optional token used for resumption.</param>
		/// <returns>An incomplete list of identifiers corresponding to the provided arguments and resumption token.</returns>
		Task<ListContainer<Record>> GetIdentifiers(ArgumentContainer arguments, ResumptionToken resumptionToken = null);
	}
}
