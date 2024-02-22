using System.Xml.Linq;
using MetadataHarvesting.Models;

namespace MetadataHarvesting.Core.Converters
{
    public interface IResumptionTokenConverter
    {
	    ResumptionToken Decode(string resumptionToken);

        string Encode(ResumptionToken resumptionToken);

        XElement ToXElement(ResumptionToken resumptionToken);
    }
}
