using System.Threading.Tasks;
using MetadataHarvesting.Models;

namespace MetadataHarvesting.Core.Providers
{
	public interface IRepositoryMetadataHarvester
	{
		string ResponseDate { get; }

		Task<Identify> Identify();

		Task<ListMetadataFormats> ListMetadataFormats(string sidentifier = null);

		Task<ListContainer<Identifiers>> ListIdenifiers(string prefix = null, string set = null, string from = null,
			string until = null, ResumptionToken resumptionToken = null);

		Task<Record> GetRecord(string identifier = null, string prefix = null);

		Task<ListContainer<Set>> ListSets(ResumptionToken resumptionToken);

		Task<ListContainer<Record>> ListRecords(string prefix = null, string set = null, string from = null,
			string until = null, ResumptionToken resumptionToken = null);
	}
}
