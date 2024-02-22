using System;
using System.Globalization;
using System.Linq;
using MetadataHarvesting.Core.Converters;
using MetadataHarvesting.Models;
using OpenScience.Data.Nomenclatures.Models;
using OpenScience.Data.Publications.Models;
using OpenScience.Services.Nomenclatures;

namespace MetadataPublications.Converters
{
	public abstract class BasePublicationConverter<TMetadata> : IPublicationConverter
		where TMetadata : RecordMetadata
	{
		private const string DefaultLanguageAlias = "bg";

		private readonly IMetadataEncoder metadataEncoder;
		protected readonly IAliasNomenclatureService aliasNomenclatureService;

		protected BasePublicationConverter(Func<string, IMetadataEncoder> metadataEncoderFactory, string prefix, IAliasNomenclatureService aliasNomenclatureService)
		{
			this.aliasNomenclatureService = aliasNomenclatureService;
			this.Prefix = prefix;
			this.metadataEncoder = metadataEncoderFactory.Invoke(Prefix);
		}

		public string Prefix { get; }

		public Publication ConvertFromMetadata(RecordMetadata recordMetadata)
		{
			if (recordMetadata is TMetadata metadata)
			{
				return Convert(metadata);
			}

			return null;
		}

		public RecordMetadata ConvertToMetadata(Publication publication)
		{
			var metadata = ConvertToRecordMetadata(publication);

			metadata.Content = metadataEncoder.Encode(metadata);

			return metadata;
		}

		protected abstract RecordMetadata ConvertToRecordMetadata(Publication publication);

		protected abstract Publication Convert(TMetadata metadata);

		protected virtual PublicationLanguage ConvertToPublicationLanguage(string language)
		{
			if (string.IsNullOrEmpty(language))
			{
				return null;
			}

			language = language.ToLower();

			var languageCode = CultureInfo.GetCultures(CultureTypes.NeutralCultures)
								   .FirstOrDefault(c =>
									   language.StartsWith(c.TwoLetterISOLanguageName) ||
									   language.StartsWith(c.ThreeLetterISOLanguageName))?
								   .TwoLetterISOLanguageName ?? DefaultLanguageAlias;

			return new PublicationLanguage {
				Language = aliasNomenclatureService.FindByAlias<Language>(languageCode)
			};
		}
	}
}
