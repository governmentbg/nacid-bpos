using System;

namespace OpenScience.Data.Publications.Models
{
	public class PublicationOriginDescription : PublicationEntity
	{
		// the baseURL of the originating repository from which the metadata record was harvested.
		public string BaseUrl { get; set; }

		// the unique identifier of the item in the originating repository from which the metadata record was disseminated.
		public string Identifier { get; set; }

		// the datestamp of the metadata record disseminated by the originating repository.
		public string Datestamp { get; set; }

		// the XML namespace URI of the metadata format of the record harvested from the originating repository.
		public string MetadataNamespace { get; set; }

		// an optional originDescription block which was that obtained when the metadata record was harvested. A set of nested originDescription blocks will describe provenance over a sequence of
		public string OriginDescription { get; set; }

		// the responseDate of the OAI-PMH response that resulted in the record being harvested from the originating repository.
		public DateTime HarvestDate { get; set; }

		// a boolean value which must be true if the harvested record was altered before being disseminated again.
		public string Altered { get; set; }
	}
}
