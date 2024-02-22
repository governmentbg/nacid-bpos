using System.Xml;
using MetadataHarvesting.Models.DublinCore;

namespace MetadataHarvesting.Core.Converters
{
	public interface IDublinCoreMetadataConverter
	{
		DublinCoreElement ReadElement(XmlReader reader);
	}
}
