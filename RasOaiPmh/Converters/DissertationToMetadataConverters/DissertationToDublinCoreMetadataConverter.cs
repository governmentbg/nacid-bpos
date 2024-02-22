using System;
using System.Globalization;
using System.Linq;
using NacidRas.Integrations.OaiPmhProvider.Models;
using NacidRas.Integrations.OaiPmhProvider.Models.DublinCore;
using NacidRas.Ras;
using NacidRas.RasRegister;

namespace NacidRas.Integrations.OaiPmhProvider.Converters
{
	public class DissertationToDublinCoreMetadataConverter : IDissertationToMetadataConverter
	{
		private readonly IMetadataEncoder metadataEncoder;

		public DissertationToDublinCoreMetadataConverter(Func<string, IMetadataEncoder> metadataEncoderFactory)
		{
			this.metadataEncoder = metadataEncoderFactory.Invoke(this.Prefix);
		}

		public string Prefix => DublinCoreMetadata.MetadataFormatPrefix;

		public RecordMetadata MapToRecordMetadata(AcademicDegreePart part, Person author)
		{
			var metadata = new DublinCoreMetadata();

			var dissertation = part.Entity.Dissertation;

			var language = CultureInfo.GetCultures(CultureTypes.AllCultures)
							   .FirstOrDefault(c => c.EnglishName == dissertation.Language?.NameAlt)?
							   .TwoLetterISOLanguageName ?? "bg";

			if (dissertation.Language != null)
			{
				metadata.Language.Add(language, "en");
			}

			if (!string.IsNullOrEmpty(dissertation.Title)) { metadata.Title.Add(dissertation.Title, language); }
			if (!string.IsNullOrEmpty(dissertation.TitleAlt)) { metadata.Title.Add(dissertation.TitleAlt, "en"); }

			if (author != null)
			{
				if (!string.IsNullOrEmpty(author.LastName) && !string.IsNullOrEmpty(author.FirstName))
				{
					var name = $"{author.LastName}, {author.FirstName}";
					metadata.Creator.Add(name, language);
				}

				if (!string.IsNullOrEmpty(author.FirstNameAlt) && !string.IsNullOrEmpty(author.LastNameAlt))
				{
					var nameAlt = $"{author.LastNameAlt}, {author.FirstNameAlt}";
					metadata.Creator.Add(nameAlt, "en");
				}
			}

			if (!string.IsNullOrEmpty(dissertation.HeadOfJury)) { metadata.Contributor.Add(dissertation.HeadOfJury, language); }
			if (!string.IsNullOrEmpty(dissertation.HeadOfJuryAlt)) { metadata.Contributor.Add(dissertation.HeadOfJuryAlt, "en"); }

			metadata.Date.Add(dissertation.DateOfAcquire ?? part.Entity.DiplomaDate);

			if (!string.IsNullOrEmpty(dissertation.Annotation)) { metadata.Description.Add(dissertation.Annotation, language); }
			if (!string.IsNullOrEmpty(dissertation.AnnotationAlt)) { metadata.Description.Add(dissertation.AnnotationAlt, "en"); }

			metadata.Format.Add("application/pdf", string.Empty);
			metadata.Identifier.Add(dissertation.Id.ToString(), string.Empty);

			metadata.Rights.Add("open access", string.Empty);
			metadata.Type.Add("Text", string.Empty);

			metadata.Content = metadataEncoder.Encode(metadata);

			return metadata;
		}
	}
}
