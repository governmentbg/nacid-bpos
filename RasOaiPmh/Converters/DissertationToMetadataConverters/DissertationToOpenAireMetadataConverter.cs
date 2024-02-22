using System;
using System.Globalization;
using System.Linq;
using NacidRas.Integrations.OaiPmhProvider.Models;
using NacidRas.Integrations.OaiPmhProvider.Models.DublinCore;
using NacidRas.Integrations.OaiPmhProvider.Models.Metadata.OpenAire;
using NacidRas.Integrations.OaiPmhProvider.Models.OpenAire;
using NacidRas.Ras;
using NacidRas.RasRegister;

namespace NacidRas.Integrations.OaiPmhProvider.Converters.DissertationToMetadataConverters
{
	public class DissertationToOpenAireMetadataConverter : IDissertationToMetadataConverter
	{
		private readonly IMetadataEncoder metadataEncoder;
		private readonly string fileStorageUrlTemplate;

		public DissertationToOpenAireMetadataConverter(Func<string, IMetadataEncoder> metadataEncoderFactory, OaiConfiguration configuration)
		{
			this.metadataEncoder = metadataEncoderFactory.Invoke(this.Prefix);
			this.fileStorageUrlTemplate = configuration.FileStorageUrlTemplate;
		}

		public string Prefix => OpenAireMetadata.MetadataFormatPrefix;

		public RecordMetadata MapToRecordMetadata(AcademicDegreePart part, Person author)
		{
			var dissertation = part.Entity.Dissertation;

			var language = CultureInfo.GetCultures(CultureTypes.AllCultures)
							   .FirstOrDefault(c => c.EnglishName == dissertation.Language?.NameAlt)?
							   .TwoLetterISOLanguageName ?? "bg";

			var metadata = new OpenAireMetadata {
				ResourceType = new OpenAireResourceType {
					ResourceType = ResourceType.DoctoralThesis,
					ResourceTypeGeneral = ResourceTypeGeneral.Literature
				},
				AccessRigths = new OpenAireRights { AccessRightsType = AccessRightsType.MetadataOnlyAccess },
				PublicationDate = new OpenAireDatesDate { DateType = DateType.Issued, Value = dissertation.DateOfAcquire?.ToString("yyyy-MM-dd") },
			};

			if (!string.IsNullOrEmpty(dissertation.Title) || !string.IsNullOrEmpty(dissertation.TitleAlt))
			{
				var titles = new OpenAireTitles();
				if (!string.IsNullOrEmpty(dissertation.Title))
				{
					titles.Items.Add(new OpenAireTitlesTitle { Language = language, Value = dissertation.Title });
				}

				if (!string.IsNullOrEmpty(dissertation.TitleAlt))
				{
					titles.Items.Add(new OpenAireTitlesTitle { Language = "en", Value = dissertation.TitleAlt });
				}

				metadata.Titles = titles;
			}

			if (author != null)
			{
				var creators = new OpenAireCreators();
				var creator = new OpenAireCreatorsCreator {
					FamilyName = author.LastName,
					GivenName = author.FirstName,
					CreatorName = new OpenAireCreatorsCreatorCreatorName {
						NameType = NameType.Personal,
						Value = $"{author.LastName}, { author.FirstName}"
					}
				};

				var institution = part.Entity.Institution?.Name ?? part.Entity.ForeignInstitution;
				if (!string.IsNullOrEmpty(institution))
				{
					creator.Affiliations.Add(institution);
				}

				creators.Items.Add(creator);

				metadata.Creators = creators;
			}

			if (!string.IsNullOrEmpty(dissertation.HeadOfJury))
			{
				var contributors = new OpenAireContributors();
				contributors.Items.Add(new OpenAireContributorsContributor { ContributorName = new OpenAireContributorsContributorContributorName { NameType = NameType.Personal, Value = dissertation.HeadOfJury }, ContributorType = ContributorType.Supervisor });

				metadata.Contributors = contributors;
			}

			if (!string.IsNullOrEmpty(dissertation.Annotation))
			{
				metadata.Descriptions.Add(dissertation.Annotation, language);
			}

			if (!string.IsNullOrEmpty(dissertation.AnnotationAlt))
			{
				metadata.Descriptions.Add(dissertation.AnnotationAlt, "en");
			}

			metadata.Languages.Add(language, "en");
			metadata.Formats.Add("application/pdf");
			metadata.Publishers.Add("РАС");

			if (dissertation.DissertationFile != null)
			{
				var fileUrl = string.Format(fileStorageUrlTemplate, dissertation.DissertationFile.Key, dissertation.DissertationFile.DbId);
				metadata.FileLocations.Add(new OpenAireFile { AccessRightsType = AccessRightsType.OpenAccess, MimeType = "application/pdf", ObjectType = ObjectType.Fulltext, Value = fileUrl });
			}

			metadata.Content = metadataEncoder.Encode(metadata);

			return metadata;
		}
	}
}
