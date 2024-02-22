using Microsoft.EntityFrameworkCore;
using NacidRas.GroupModifications.Models;
using NacidRas.Infrastructure.DomainValidation;
using NacidRas.Integrations.RndIntegration.Models;
using NacidRas.Integrations.RndIntegration.Publications;
using NacidRas.Ras.Indicators;
using NacidRas.RasRegister;
using NacidRis.Infrastructure.DomainValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NacidRas.Ras.Services
{
	public class CompareAcademicDegreeIndicatorService
	{
		private readonly RasDbContext context;
		private readonly PublicationService publicationService;
		private readonly DomainValidator domainValidator;

		public CompareAcademicDegreeIndicatorService(RasDbContext context, PublicationService publicationService, DomainValidator domainValidator)
		{
			this.context = context;
			this.publicationService = publicationService;
			this.domainValidator = domainValidator;
		}

		public List<IndicatorDegreeDiffsDto> CompareIndicators(int commitId, int partId)
		{
			(AcademicDegreePart comparisonPart, AcademicDegreePart modifiedDegreePart) = FindCompareParts(commitId, partId);

			return CompareDegrees(comparisonPart.Entity.AcademicDegreeIndicatorGroupNames, modifiedDegreePart.Entity.AcademicDegreeIndicatorGroupNames);
		}

		public object CheckCorrectComparison(int commitId, int partId)
		{
			(AcademicDegreePart comparisonPart, AcademicDegreePart modifiedPart) = FindCompareParts(commitId, partId);

			if (modifiedPart?.Entity?.AcademicDegreeTypeId != comparisonPart?.Entity?.AcademicDegreeTypeId
				|| modifiedPart?.Entity?.ResearchAreaId != comparisonPart?.Entity?.ResearchAreaId)
			{
				domainValidator.ThrowErrorMessage(SystemErrorCode.CompareAcademicDegrees);
			}

			return new object();
		}


		// Search in history, if not found fallback to actual part
		private (AcademicDegreePart comparisonPart, AcademicDegreePart modifiedDegreePart) FindCompareParts(int commitId, int partId)
		{
			AcademicDegreePart modifiedPart = context
				.AcademicDegreeParts
				.IncludeIndicatorProperties()
				.Single(s => s.Id == partId);

			ModificationRequest modificationRequest = context
				.ModificationRequests
				.SingleOrDefault(e => modifiedPart.ModificationRequestId.HasValue && e.Id == modifiedPart.ModificationRequestId.Value);

			AcademicDegreePart comparisonPart = null;
			GroupModification history = null;

			if(modificationRequest != null)
			{
				history = context
					.GroupModifications
					.Include(e => e.ModificationRequests)
						.ThenInclude(e => e.AcademicDegreeParts)
					.Where(e => e.ParentId == modificationRequest.GroupModificationId
						&& e.State == GroupModificationState.History
						&& e.ModificationRequests.Any(mr => mr.AcademicDegreeParts.Any(part => part.InitialPartId == modifiedPart.InitialPartId)))
					.OrderByDescending(e => e.Id)
					.FirstOrDefault();
			}

			if (history != null)
			{
				var historyPart = history
					.ModificationRequests
					.Single(e => e.AcademicDegreeParts.Any(part => part.InitialPartId == modifiedPart.InitialPartId))
					.AcademicDegreeParts
					.Single(e => e.InitialPartId == modifiedPart.InitialPartId);

				if (historyPart != null)
					comparisonPart = context.AcademicDegreeParts.IncludeIndicatorProperties().Single(e => e.Id == historyPart.Id);
			}

			if (comparisonPart == null)
				comparisonPart = context.AcademicDegreeParts
										.IncludeIndicatorProperties()
										.SingleOrDefault(s => s.CommitId == commitId && s.InitialPartId == modifiedPart.InitialPartId);


			return (comparisonPart,modifiedPart);
		}

		public CommonDegreeInformationDto GetDegreeInformation(int partId)
		{
			var entity = context.AcademicDegreeParts
										.Include(e => e.Entity)
											.ThenInclude(e => e.AcademicDegreeType)
										.Include(e => e.Entity)
											.ThenInclude(e => e.ResearchArea)
										.Include(e => e.Entity)
											.ThenInclude(e => e.Institution)
										.SingleOrDefault(res => res.Id == partId).Entity;
			return new CommonDegreeInformationDto {
				AcademicDegreeType = entity.AcademicDegreeType,
				Institution = entity.Institution,
				ResearchArea = entity.ResearchArea
			};
		}

		private List<IndicatorDegreeDiffsDto> CompareDegrees(List<AcademicDegreeIndicatorGroupName> actualAcademicDegreeIndicators, List<AcademicDegreeIndicatorGroupName> modifiedAcademicDegreeIndicators)
		{
			List<IndicatorDegreeDiffsDto> indicatorDiffs = new List<IndicatorDegreeDiffsDto>();

			foreach (var modifiedIndicator in modifiedAcademicDegreeIndicators)
			{
				var currentActualDegreeIndicator = actualAcademicDegreeIndicators.Single(t => t.ScientificIndicatorTypeId == modifiedIndicator.ScientificIndicatorTypeId);

				indicatorDiffs.Add(CompareDifferentDegrees(currentActualDegreeIndicator, modifiedIndicator));
			}

			return PopulateNavigationProperties(indicatorDiffs);
		}


		private List<Publication> ExtractInitialPartIds(List<IndicatorDegreeDiffsDto> indicatorDiffs)
		{
			List<int> initialPartIdItems = new List<int>();

			foreach (var item in indicatorDiffs)
			{
				foreach (var publicationForAdd in item.AcademicDegreeIndicatorPublicationForAdd)
				{
					if (publicationForAdd.PublicationInitialPartId.HasValue)
						initialPartIdItems.Add(publicationForAdd.PublicationInitialPartId.Value);
				}

				foreach (var publicationForUpdate in item.AcademicDegreeIndicatorPublicationForUpdate)
				{
					if (publicationForUpdate.Item1.PublicationInitialPartId.HasValue)
						initialPartIdItems.Add(publicationForUpdate.Item1.PublicationInitialPartId.Value);
					if (publicationForUpdate.Item2.PublicationInitialPartId.HasValue)
						initialPartIdItems.Add(publicationForUpdate.Item2.PublicationInitialPartId.Value);
				}

				foreach (var publicationForDelete in item.AcademicDegreeIndicatorPublicationForDelete)
				{
					if (publicationForDelete.PublicationInitialPartId.HasValue)
						initialPartIdItems.Add(publicationForDelete.PublicationInitialPartId.Value);
				}
			}

			string initialPartIdsStringItems = String.Join(", ", initialPartIdItems);

			var extractedPublications = publicationService.GetPublicationsByInitialPartId(initialPartIdsStringItems);

			return extractedPublications;
		}

		private List<IndicatorDegreeDiffsDto> PopulateNavigationProperties(List<IndicatorDegreeDiffsDto> indicatorDiffs)
		{
			var extractedPublications = ExtractInitialPartIds(indicatorDiffs);

			foreach (var item in indicatorDiffs)
			{
				foreach (var publicationForAdd in item.AcademicDegreeIndicatorPublicationForAdd)
				{
					if (publicationForAdd.PublicationInitialPartId.HasValue)
						publicationForAdd.PublicationInitialPart = extractedPublications.FirstOrDefault(s => s.PublicationInitialPartId == publicationForAdd.PublicationInitialPartId.Value);
				}

				foreach (var publicationForUpdate in item.AcademicDegreeIndicatorPublicationForUpdate)
				{
					if (publicationForUpdate.Item1.PublicationInitialPartId.HasValue)
						publicationForUpdate.Item1.PublicationInitialPart = extractedPublications.FirstOrDefault(s => s.PublicationInitialPartId == publicationForUpdate.Item1.PublicationInitialPartId.Value);
					if (publicationForUpdate.Item2.PublicationInitialPartId.HasValue)
						publicationForUpdate.Item2.PublicationInitialPart = extractedPublications.FirstOrDefault(s => s.PublicationInitialPartId == publicationForUpdate.Item2.PublicationInitialPartId.Value);
				}

				foreach (var publicationForDelete in item.AcademicDegreeIndicatorPublicationForDelete)
				{
					if (publicationForDelete.PublicationInitialPartId.HasValue)
						publicationForDelete.PublicationInitialPart = extractedPublications.FirstOrDefault(s => s.PublicationInitialPartId == publicationForDelete.PublicationInitialPartId.Value);
				}
			}

			return indicatorDiffs.OrderBy(t => t.ScientificIndicatorTypeId).ToList();
		}

		private IndicatorDegreeDiffsDto CompareDifferentDegrees(AcademicDegreeIndicatorGroupName original, AcademicDegreeIndicatorGroupName modification)
		{
			IndicatorDegreeDiffsDto indicatorDiff = new IndicatorDegreeDiffsDto();

			indicatorDiff.ScientificIndicatorTypeName = original.ScientificIndicatorType.Name;
			indicatorDiff.IndicatorType = original.ScientificIndicatorType.IndicatorType;
			indicatorDiff.ScientificIndicatorTypeId = original.ScientificIndicatorTypeId;

			if (original.ScientificIndicatorType.IndicatorType == IndicatorType.Text)
			{
				if (!String.IsNullOrWhiteSpace(original.TextProof)
					&& !String.IsNullOrWhiteSpace(modification.TextProof)
					&& !original.TextProof.Equals(modification.TextProof))
				{
					indicatorDiff.ModifiedTextProof = modification.TextProof;
					indicatorDiff.OriginalTextProof = original.TextProof;
					indicatorDiff.ModifiedScore = modification.Score;
					indicatorDiff.OriginalScore = original.Score;
				}
				else if (original.Score != modification.Score)
				{
					indicatorDiff.ModifiedTextProof = modification.TextProof;
					indicatorDiff.OriginalTextProof = original.TextProof;
					indicatorDiff.ModifiedScore = modification.Score;
					indicatorDiff.OriginalScore = original.Score;
				}
			}
			else if (original.ScientificIndicatorType.IndicatorType == IndicatorType.Publication || original.ScientificIndicatorType.IndicatorType == IndicatorType.TextAndPublicationWithQuotes)
			{
				var forAdd = modification.AcademicDegreeIndicatorPublications.Where(newItem =>
					!original.AcademicDegreeIndicatorPublications.Any(originalItem => newItem.PublicationInitialPartId == originalItem.PublicationInitialPartId))
				.ToList();

				var forDelete = original.AcademicDegreeIndicatorPublications.Where(originalItem =>
					!modification.AcademicDegreeIndicatorPublications.Any(newItem => newItem.PublicationInitialPartId == originalItem.PublicationInitialPartId))
					.ToList();

				indicatorDiff.AcademicDegreeIndicatorPublicationForAdd.AddRange(forAdd);

				foreach (var item in modification.AcademicDegreeIndicatorPublications)
				{
					if (!original.AcademicDegreeIndicatorPublications.Any(s => item.PublicationInitialPartId == s.PublicationInitialPartId
						 && item.Score == s.Score))
					{
						if (!indicatorDiff.AcademicDegreeIndicatorPublicationForAdd.Any(s => s.Id == item.Id))
							indicatorDiff.AcademicDegreeIndicatorPublicationForUpdate.Add(new Tuple<AcademicDegreeIndicatorPublication, AcademicDegreeIndicatorPublication>
								(item, original.AcademicDegreeIndicatorPublications.FirstOrDefault(s => s.PublicationInitialPartId == item.PublicationInitialPartId)));
					}
				}

				// item1 are modified, item2 are original
				foreach (var item in indicatorDiff.AcademicDegreeIndicatorPublicationForUpdate)
				{

					int counter = 1;

					var quotesForAddIds = item.Item1.AcademicDegreeIndicatorPublicationQuotes.Where(newItem =>
								!item.Item2.AcademicDegreeIndicatorPublicationQuotes.Any(originalItem => !String.IsNullOrWhiteSpace(newItem.Quote) && !String.IsNullOrWhiteSpace(originalItem.Quote) && newItem.Quote.Equals(originalItem.Quote)))
								.Select(s => s.Id)
								.ToList();

					var quotesForDeleteIds = item.Item2.AcademicDegreeIndicatorPublicationQuotes.Where(originalItem =>
								!item.Item1.AcademicDegreeIndicatorPublicationQuotes.Any(newItem => !String.IsNullOrWhiteSpace(newItem.Quote) && !String.IsNullOrWhiteSpace(originalItem.Quote) && newItem.Quote.Equals(originalItem.Quote)))
								.Select(s => s.Id)
								.ToList();


					var quotesForUpdateIds = item.Item1.AcademicDegreeIndicatorPublicationQuotes.Where(newItem =>
						item.Item2.AcademicDegreeIndicatorPublicationQuotes.Any(originalItem => !String.IsNullOrWhiteSpace(newItem.Quote) && !String.IsNullOrWhiteSpace(originalItem.Quote) && originalItem.Quote.Equals(newItem.Quote)
									&& originalItem.Score != newItem.Score))
									.Select(s => s.Id)
									.ToList();

					foreach (var quoteForAdd in item.Item1.AcademicDegreeIndicatorPublicationQuotes)
					{
						if (quotesForAddIds.Any(s => s == quoteForAdd.Id))
						{
							quoteForAdd.ModifiedStatus = 1; // new
							quoteForAdd.Order = counter;

							item.Item2.AcademicDegreeIndicatorPublicationQuotes.Add(new AcademicDegreeIndicatorPublicationQuote() {
								Order = counter,
								ModifiedStatus = 1
							});

							counter++;
						}
					}

					foreach (var quoteForUpdate in item.Item1.AcademicDegreeIndicatorPublicationQuotes)
					{
						if (quotesForUpdateIds.Any(s => s == quoteForUpdate.Id))
						{
							quoteForUpdate.ModifiedStatus = 2; // update
							quoteForUpdate.Order = counter;

							var originalItem = item.Item2.AcademicDegreeIndicatorPublicationQuotes
														 .FirstOrDefault(s => !String.IsNullOrWhiteSpace(s.Quote) && !String.IsNullOrWhiteSpace(quoteForUpdate.Quote) && s.Quote.Equals(quoteForUpdate.Quote));

							originalItem.Order = counter;
							originalItem.ModifiedStatus = 2;

							counter++;
						}
					}

					foreach (var quoteForDelete in item.Item2.AcademicDegreeIndicatorPublicationQuotes)
					{
						if (quotesForDeleteIds.Any(s => s == quoteForDelete.Id))
						{
							quoteForDelete.ModifiedStatus = 3; // delete
							quoteForDelete.Order = counter;

							item.Item1.AcademicDegreeIndicatorPublicationQuotes.Add(new AcademicDegreeIndicatorPublicationQuote() {
								Order = counter,
								ModifiedStatus = 3
							});
							counter++;
						}
					}

					foreach (var quoteForUpdateFromOriginal in item.Item2.AcademicDegreeIndicatorPublicationQuotes)
					{
						if (quoteForUpdateFromOriginal.ModifiedStatus == 0) // 0 is unchanged
						{
							quoteForUpdateFromOriginal.Order = counter;

							var modifiedItem = item.Item2.AcademicDegreeIndicatorPublicationQuotes.FirstOrDefault(s => s.Quote != null && s.Quote.Equals(quoteForUpdateFromOriginal.Quote)
							&& s.Score == quoteForUpdateFromOriginal.Score);
							modifiedItem.Order = counter;

							counter++;
						}
					}

					item.Item1.AcademicDegreeIndicatorPublicationQuotes = item.Item1.AcademicDegreeIndicatorPublicationQuotes
																				   .OrderBy(t => t.Order)
																				   .ToList();
					item.Item2.AcademicDegreeIndicatorPublicationQuotes = item.Item2.AcademicDegreeIndicatorPublicationQuotes
																				   .OrderBy(t => t.Order)
																				   .ToList();

				}

				indicatorDiff.AcademicDegreeIndicatorPublicationForDelete.AddRange(forDelete);

			}
			else if (original.ScientificIndicatorType.IndicatorType == IndicatorType.TextAndScore)
			{
				var forAdd = modification.AcademicDegreeTextAndScores.Where(newItem =>
							!original.AcademicDegreeTextAndScores.Any(s => !String.IsNullOrWhiteSpace(newItem.Text) && !String.IsNullOrWhiteSpace(s.Text) && newItem.Text.Equals(s.Text)))
							.ToList();

				var forDelete = original.AcademicDegreeTextAndScores.Where(originalItem =>
					!modification.AcademicDegreeTextAndScores.Any(newItem => !String.IsNullOrWhiteSpace(newItem.Text) && !String.IsNullOrWhiteSpace(originalItem.Text) && newItem.Text.Equals(originalItem.Text)))
					.ToList();

				indicatorDiff.AcademicDegreeTextAndScoresForAdd.AddRange(forAdd);

				foreach (var item in modification.AcademicDegreeTextAndScores)
				{
					if (original.AcademicDegreeTextAndScores.Any(s => !String.IsNullOrWhiteSpace(item.Text) && !String.IsNullOrWhiteSpace(s.Text) && s.Text.Equals(item.Text) && s.Score != item.Score))
					{
						var originalItem = original.AcademicDegreeTextAndScores.FirstOrDefault(t => !String.IsNullOrWhiteSpace(item.Text) && !String.IsNullOrWhiteSpace(t.Text) && t.Text.Equals(item.Text));

						indicatorDiff.AcademicDegreeTextAndScoresForUpdate.Add(new Tuple<AcademicDegreeTextAndScore, AcademicDegreeTextAndScore>
							(item, originalItem));

					}
				}

				indicatorDiff.AcademicDegreeTextAndScoresForDelete.AddRange(forDelete);
			}

			return indicatorDiff;
		}
	}

	public class CompareDegreeResultDto
	{
		public CommonDegreeInformationDto EntityInformation { get; set; }
		public List<IndicatorDegreeDiffsDto> Result { get; set; }
	}

	public class CommonDegreeInformationDto
	{
		public Institution Institution { get; set; }
		public ResearchArea ResearchArea { get; set; }
		public AcademicDegreeType AcademicDegreeType { get; set; }

	}

	public class IndicatorDegreeDiffsDto
	{
		public int ScientificIndicatorTypeId { get; set; }
		public string ScientificIndicatorTypeName { get; set; }
		public IndicatorType IndicatorType { get; set; }
		public decimal? OriginalScore { get; set; }
		public decimal? ModifiedScore { get; set; }
		public string OriginalTextProof { get; set; }
		public string ModifiedTextProof { get; set; }
		public List<Tuple<AcademicDegreeIndicatorPublication, AcademicDegreeIndicatorPublication>> AcademicDegreeIndicatorPublicationForUpdate { get; set; }
		public List<AcademicDegreeIndicatorPublication> AcademicDegreeIndicatorPublicationForAdd { get; set; }
		public List<AcademicDegreeIndicatorPublication> AcademicDegreeIndicatorPublicationForDelete { get; set; }
		public List<Tuple<AcademicDegreeTextAndScore, AcademicDegreeTextAndScore>> AcademicDegreeTextAndScoresForUpdate { get; set; }
		public List<AcademicDegreeTextAndScore> AcademicDegreeTextAndScoresForAdd { get; set; }
		public List<AcademicDegreeTextAndScore> AcademicDegreeTextAndScoresForDelete { get; set; }
		public IndicatorDegreeDiffsDto()
		{
			AcademicDegreeIndicatorPublicationForAdd = new List<AcademicDegreeIndicatorPublication>();
			AcademicDegreeIndicatorPublicationForUpdate = new List<Tuple<AcademicDegreeIndicatorPublication, AcademicDegreeIndicatorPublication>>();
			AcademicDegreeIndicatorPublicationForDelete = new List<AcademicDegreeIndicatorPublication>();
			AcademicDegreeTextAndScoresForAdd = new List<AcademicDegreeTextAndScore>();
			AcademicDegreeTextAndScoresForUpdate = new List<Tuple<AcademicDegreeTextAndScore, AcademicDegreeTextAndScore>>();
			AcademicDegreeTextAndScoresForDelete = new List<AcademicDegreeTextAndScore>();
		}
	}
}
