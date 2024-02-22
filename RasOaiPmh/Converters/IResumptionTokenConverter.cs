using System.Xml.Linq;
using NacidRas.Integrations.OaiPmhProvider.Models;

namespace NacidRas.Integrations.OaiPmhProvider.Converters
{
    public interface IResumptionTokenConverter
    {
	    ResumptionToken Decode(string resumptionToken);

        string Encode(ResumptionToken resumptionToken);

        XElement ToXElement(ResumptionToken resumptionToken);
    }
}
