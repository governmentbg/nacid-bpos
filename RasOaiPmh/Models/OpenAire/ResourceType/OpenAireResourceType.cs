using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireResourceType : IXmlSerializableMetadataElement
	{
		private readonly IDictionary<ResourceType, string> resourceTypeUris = new Dictionary<ResourceType, string> {
{ ResourceType.Annotation,"http://purl.org/coar/resource_type/c_1162"},
{ ResourceType.JournalArticle,"http://purl.org/coar/resource_type/c_6501"},
{ ResourceType.LetterToTheEditor,"http://purl.org/coar/resource_type/c_545b"},
{ ResourceType.Editorial,"http://purl.org/coar/resource_type/c_b239"},
{ ResourceType.ResearchArticle,"http://purl.org/coar/resource_type/c_2df8fbb1"},
{ ResourceType.ReviewArticle,"http://purl.org/coar/resource_type/c_dcae04bc"},
{ ResourceType.DataPaper,"http://purl.org/coar/resource_type/c_beb9"},
{ ResourceType.ContributionToJournal,"http://purl.org/coar/resource_type/c_3e5a"},
{ ResourceType.BookReview,"http://purl.org/coar/resource_type/c_ba08"},
{ ResourceType.BookPart,"http://purl.org/coar/resource_type/c_3248"},
{ ResourceType.Book,"http://purl.org/coar/resource_type/c_2f33"},
{ ResourceType.Bibliography,"http://purl.org/coar/resource_type/c_86bc"},
{ ResourceType.Preprint,"http://purl.org/coar/resource_type/c_816b"},
{ ResourceType.WorkingPaper,"http://purl.org/coar/resource_type/c_8042"},
{ ResourceType.TechnicalDocumentation,"http://purl.org/coar/resource_type/c_71bd"},
{ ResourceType.TechnicalReport,"http://purl.org/coar/resource_type/c_18gh"},
{ ResourceType.ResearchReport,"http://purl.org/coar/resource_type/c_18ws"},
{ ResourceType.ReportToFundingAgency,"http://purl.org/coar/resource_type/c_18hj"},
{ ResourceType.ProjectDeliverable,"http://purl.org/coar/resource_type/c_18op"},
{ ResourceType.PolicyReport,"http://purl.org/coar/resource_type/c_186u"},
{ ResourceType.OtherTypeOfReport,"http://purl.org/coar/resource_type/c_18wq"},
{ ResourceType.Memorandum,"http://purl.org/coar/resource_type/c_18wz"},
{ ResourceType.InternalReport,"http://purl.org/coar/resource_type/c_18ww"},
{ ResourceType.Review,"http://purl.org/coar/resource_type/c_efa0"},
{ ResourceType.ResearchProposal,"http://purl.org/coar/resource_type/c_baaf"},
{ ResourceType.ReportPart,"http://purl.org/coar/resource_type/c_ba1f"},
{ ResourceType.Report,"http://purl.org/coar/resource_type/c_93fc"},
{ ResourceType.Patent,"http://purl.org/coar/resource_type/c_15cd"},
{ ResourceType.ConferencePosterNotInProceedings,"http://purl.org/coar/resource_type/c_18co"},
{ ResourceType.ConferencePaperNotInProceedings,"http://purl.org/coar/resource_type/c_18cp"},
{ ResourceType.ConferencePoster,"http://purl.org/coar/resource_type/c_6670"},
{ ResourceType.ConferencePaper,"http://purl.org/coar/resource_type/c_5794"},
{ ResourceType.ConferenceObject,"http://purl.org/coar/resource_type/c_c94f"},
{ ResourceType.ConferenceProceedings,"http://purl.org/coar/resource_type/c_f744"},
{ ResourceType.BachelorThesis,"http://purl.org/coar/resource_type/c_7a1f"},
{ ResourceType.MasterThesis,"http://purl.org/coar/resource_type/c_bdcc"},
{ ResourceType.DoctoralThesis,"http://purl.org/coar/resource_type/c_db06"},
{ ResourceType.Thesis,"http://purl.org/coar/resource_type/c_46ec"},
{ ResourceType.Letter,"http://purl.org/coar/resource_type/c_0857"},
{ ResourceType.Lecture,"http://purl.org/coar/resource_type/c_8544"},
{ ResourceType.Text,"http://purl.org/coar/resource_type/c_18cf"},
{ ResourceType.MusicalNotation,"http://purl.org/coar/resource_type/c_18cw"},
{ ResourceType.MusicalComposition,"http://purl.org/coar/resource_type/c_18cd"},
{ ResourceType.Sound,"http://purl.org/coar/resource_type/c_18cc"},
{ ResourceType.Video,"http://purl.org/coar/resource_type/c_12ce"},
{ ResourceType.MovingImage,"http://purl.org/coar/resource_type/c_8a7e"},
{ ResourceType.StillImage,"http://purl.org/coar/resource_type/c_ecc8"},
{ ResourceType.Image,"http://purl.org/coar/resource_type/c_c513"},
{ ResourceType.Map,"http://purl.org/coar/resource_type/c_12cd"},
{ ResourceType.CartographicMaterial,"http://purl.org/coar/resource_type/c_12cc"},
{ ResourceType.Software,"http://purl.org/coar/resource_type/c_5ce6"},
{ ResourceType.Dataset,"http://purl.org/coar/resource_type/c_ddb1"},
{ ResourceType.InteractiveResource,"http://purl.org/coar/resource_type/c_e9a0"},
{ ResourceType.Website,"http://purl.org/coar/resource_type/c_7ad9"},
{ ResourceType.Workflow,"http://purl.org/coar/resource_type/c_393c"},
{ ResourceType.Other,"http://purl.org/coar/resource_type/c_1843"},
	};

		public ResourceTypeGeneral ResourceTypeGeneral { get; set; }
		public ResourceType ResourceType { get; set; }
		public string Uri => resourceTypeUris[ResourceType];
		public string Value => (new string(ResourceType.ToString().SelectMany((c, i) => i > 0 && char.IsUpper(c) ? new[] { ' ', c } : new[] { c }).ToArray())).ToLower();

		public XElement Serialize()
		{
			return new XElement(OaiNamespaces.OpenAireNamespace + "resourceType",
			  new XAttribute("resourceTypeGeneral", ResourceTypeGeneral.ToString().ToLower()),
			  new XAttribute("uri", Uri),
			  Value
			  );
		}
	}
}
