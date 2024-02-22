using System.Collections.Generic;
using MetadataHarvesting.Models;
using MetadataHarvesting.Models.OpenAire;
using OpenScience.Data.Publications.Models;

namespace MetadataPublications.Converters.Extensions
{
	public static class PublicationExtensions
	{
		public static ICollection<TPublicationEntity> SetViewOrder<TPublicationEntity>(this IList<TPublicationEntity> items)
			where TPublicationEntity : PublicationEntity
		{
			for (var i = 0; i < items.Count; i++)
			{
				items[i].ViewOrder = i + 1;
			}

			return items;
		}

		public static TBaseListElement SetViewOrder<TBaseListElement, TPublicationEntity>(this TBaseListElement baseListElement)
			where TPublicationEntity : PublicationEntity, IXmlSerializableMetadataElement
			where TBaseListElement : BaseListElement<TPublicationEntity>
		{
			baseListElement.Items.SetViewOrder();

			return baseListElement;
		}
	}
}
