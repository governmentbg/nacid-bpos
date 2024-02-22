using Microsoft.EntityFrameworkCore;
using NacidRas.GroupModifications.Models;
using NacidRas.Infrastructure.Data;
using NacidRas.Infrastructure.DomainValidation;
using NacidRas.Integrations.RndIntegration.Models;
using NacidRas.Integrations.RndIntegration.Publications;
using NacidRas.Ras.Indicators;
using NacidRas.RasRegister;
using NacidRas.RasRegister.Models;
using NacidRis.Infrastructure.DomainValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NacidRas.Ras.Services
{
	public class CompareAcademicRankIndicatorService
	{
		private readonly RasDbContext context;
		private readonly PublicationService publicationService;
		private readonly DomainValidator domainValidator;

		public CompareAcademicRankIndicatorService(RasDbContext context, PublicationService publicationService, DomainValidator domainValidator)
		{
			this.context = context;
			this.publicationService = publicationService;
			this.domainValidator = domainValidator;
		}

		public List<IndicatorRankDiffsDto> CompareIndicators(int commitId, int partId)
		{
			(AcademicRankPart comparisonPart, AcademicRankPart modifiedPart) = FindCompareParts(commitId, partId);

			return CompareRanks(comparisonPart.Entity.AcademicRankIndicatorGroupNames, modifiedPart.Entity.AcademicRankIndicatorGroupNames);
		}

		public object CheckCorrectComparison(int commitId, int partId)
		{
			(AcademicRankPart comparisonPart, AcademicRankPart modifiedPart) = FindCompareParts(commitId, partId);

			if (modifiedPart?.Entity?.AcademicRankTypeId != comparisonPart?.Entity?.AcademicRankTypeId
				|| modifiedPart?.Entity?.ResearchAreaId != comparisonPart?.Entity?.ResearchAreaId)
			{
				domainValidator.ThrowErrorMessage(SystemErrorCode.CompareAcademicRanks);
			}

			return new object();
		}


		// Search in history, if not found fallback to actual part
		private (AcademicRankPart comparisonPart, AcademicRankPart modifiedPart) FindCompareParts(int commitId, int partId)
		{
			AcademicRankPart modifiedPart = context
					.AcademicRankParts
					.IncludeIndicatorProperties()
					.Single(s => s.Id == partId);

			ModificationRequest modificationRequest = context
				.ModificationRequests
				.SingleOrDefault(e => modifiedPart.ModificationRequestId.HasValue && e.Id == modifiedPart.ModificationRequestId.Value);

			AcademicRankPart comparisonPart = null;
			GroupModification history = null;

			if(modificationRequest != null)
			{
				history = context
					.GroupModifications
					.Include(e => e.ModificationRequests)
						.ThenInclude(e => e.AcademicRankParts)
					.Where(e => e.ParentId == modificationRequest.GroupModificationId
						&& e.State == GroupModifications.Models.GroupModificationState.History
						&& e.ModificationRequests.Any(mr => mr.AcademicRankParts.Any(part => part.InitialPartId == modifiedPart.InitialPartId)))
					.OrderByDescending(e => e.Id)
					.FirstOrDefault();
			}

			if (history != null)
			{
				var historyPart = history
					.ModificationRequests
					.Single(e => e.AcademicRankParts.Any(part => part.InitialPartId == modifiedPart.InitialPartId))
					.AcademicRankParts
					.Single(e => e.InitialPartId == modifiedPart.InitialPartId);

				if (historyPart != null)
					comparisonPart = context.AcademicRankParts.IncludeIndicatorProperties().Single(e => e.Id == historyPart.Id);
			}

			if (comparisonPart == null)
				comparisonPart = context.AcademicRankParts
										.IncludeIndicatorProperties()
										.SingleOrDefault(s => s.CommitId == commitId && s.InitialPartId == modifiedPart.InitialPartId);

			return (comparisonPart, modifiedPart);
		}

		public CommonRankInformationDto GetRankInformation(int partId)
		{
			var entity = context.AcademicRankParts
										.Include(e => e.Entity)
											.ThenInclude(e => e.AcademicRankType)
										.Include(e => e.Entity)
											.ThenInclude(e => e.ResearchArea)
										.Include(e => e.Entity)
											.ThenInclude(e => e.Institution)
										.SingleOrDefault(res => res.Id == partId).Entity;
			return new CommonRankInformationDto {
				AcademicRankType = entity.AcademicRankType,
				Institution = entity.Institution,
				ResearchArea = entity.ResearchArea
			};
		}

		private List<IndicatorRankDiffsDto> CompareRanks(List<AcademicRankIndicatorGroupName> actualAcademicRankIndicators, List<AcademicRankIndicatorGroupName> modifiedAcademicRankIndicators)
		{
			List<IndicatorRankDiffsDto> indicatorDiffs = new List<IndicatorRankDiffsDto>();

			foreach (var modifiedIndicator in modifiedAcademicRankIndicators)
			{
				var currentActualRankIndicator = actualAcademicRankIndicators.Single(t => t.ScientificIndicatorTypeId == modifiedIndicator.ScientificIndicatorTypeId);

				indicatorDiffs.Add(CompareDifferentRanks(currentActualRankIndicator, modifiedIndicator));
			}

			return PopulateNavigationProperties(indicatorDiffs);
		}


		private List<Publication> ExtractInitialPartIds(List<IndicatorRankDiffsDto> indicatorDiffs)
		{

			List<int> initialPartIdItems = new List<int>();

			foreach (var item in indicatorDiffs)
			{
				foreach (var publicationForAdd in item.AcademicRankIndicatorPublicationForAdd)
				{
					if (publicationForAdd.PublicationInitialPartId.HasValue)
						initialPartIdItems.Add(publicationForAdd.PublicationInitialPartId.Value);
				}

				foreach (var publicationForUpdate in item.AcademicRankIndicatorPublicationForUpdate)
				{
					if (publicationForUpdate.Item1.PublicationInitialPartId.HasValue)
						initialPartIdItems.Add(publicationForUpdate.Item1.PublicationInitialPartId.Value);
					if (publicationForUpdate.Item2.PublicationInitialPartId.HasValue)
						initialPartIdItems.Add(publicationForUpdate.Item2.PublicationInitialPartId.Value);
				}

				foreach (var publicationForDelete in item.AcademicRankIndicatorPublicationForDelete)
				{
					if (publicationForDelete.PublicationInitialPartId.HasValue)
						initialPartIdItems.Add(publicationForDelete.PublicationInitialPartId.Value);
				}
			}

			string initialPartIdsStringItems = String.Join(", ", initialPartIdItems);

			var extractedPublications = publicationService.GetPublicationsByInitialPartId(initialPartIdsStringItems);

			return extractedPublications;
		}

		private List<IndicatorRankDiffsDto> PopulateNavigationProperties(List<IndicatorRankDiffsDto> indicatorDiffs)
		{
			var extractedPublications = ExtractInitialPartIds(indicatorDiffs);

			foreach (var item in indicatorDiffs)
			{

				foreach (var publicationForAdd in item.AcademicRankIndicatorPublicationForAdd)
				{
					if (publicationForAdd.PublicationInitialPartId.HasValue)
						publicationForAdd.PublicationInitialPart = extractedPublications.FirstOrDefault(s => s.PublicationInitialPartId == publicationForAdd.PublicationInitialPartId.Value);
				}

				foreach (var publicationForUpdate in item.AcademicRankIndicatorPublicationForUpdate)
				{
					if (publicationForUpdate.Item1.PublicationInitialPartId.HasValue)
						publicationForUpdate.Item1.PublicationInitialPart = extractedPublications.FirstOrDefault(s => s.PublicationInitialPartId == publicationForUpdate.Item1.PublicationInitialPartId.Value);
					if (publicationForUpdate.Item2.PublicationInitialPartId.HasValue)
						publicationForUpdate.Item2.PublicationInitialPart = extractedPublications.FirstOrDefault(s => s.PublicationInitialPartId == publicationForUpdate.Item2.PublicationInitialPartId.Value);
				}

				foreach (var publicationForDelete in item.AcademicRankIndicatorPublicationForDelete)
				{
					if (publicationForDelete.PublicationInitialPartId.HasValue)
						publicationForDelete.PublicationInitialPart = extractedPublications.FirstOrDefault(s => s.PublicationInitialPartId == publicationForDelete.PublicationInitialPartId.Value);
				}
			}

			return indicatorDiffs.OrderBy(t => t.ScientificIndicatorTypeId).ToList();
		}

		private IndicatorRankDiffsDto CompareDifferentRanks(AcademicRankIndicatorGroupName original, AcademicRankIndicatorGroupName modification)
		{
			IndicatorRankDiffsDto indicatorDiff = new IndicatorRankDiffsDto();

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
				var forAdd = modification.AcademicRankIndicatorPublications.Where(newItem =>
					!original.AcademicRankIndicatorPublications.Any(originalItem => newItem.PublicationInitialPartId == originalItem.PublicationInitialPartId))
				.ToList();

				var forDelete = original.AcademicRankIndicatorPublications.Where(originalItem =>
					!modification.AcademicRankIndicatorPublications.Any(newItem => newItem.PublicationInitialPartId == originalItem.PublicationInitialPartId))
					.ToList();

				indicatorDiff.AcademicRankIndicatorPublicationForAdd.AddRange(forAdd);

				foreach (var item in modification.AcademicRankIndicatorPublications)
				{
					if (!original.AcademicRankIndicatorPublications.Any(s => item.PublicationInitialPartId == s.PublicationInitialPartId
						 && item.Score == s.Score))
					{
						if (!indicatorDiff.AcademicRankIndicatorPublicationForAdd.Any(s => s.Id == item.Id))
							indicatorDiff.AcademicRankIndicatorPublicationForUpdate.Add(new Tuple<AcademicRankIndicatorPublication, AcademicRankIndicatorPublication>
								(item, original.AcademicRankIndicatorPublications.FirstOrDefault(s => s.PublicationInitialPartId == item.PublicationInitialPartId)));
					}
				}

				// item1 are modified, item2 are original
				foreach (var item in indicatorDiff.AcademicRankIndicatorPublicationForUpdate)
				{

					int counter = 1;

					var quotesForAddIds = item.Item1.AcademicRankIndicatorPublicationQuotes.Where(newItem =>
								!item.Item2.AcademicRankIndicatorPublicationQuotes.Any(originalItem => !String.IsNullOrWhiteSpace(newItem.Quote) && !String.IsNullOrWhiteSpace(originalItem.Quote) && newItem.Quote.Equals(originalItem.Quote)))
								.Select(s => s.Id)
								.ToList();

					var quotesForDeleteIds = item.Item2.AcademicRankIndicatorPublicationQuotes.Where(originalItem =>
								!item.Item1.AcademicRankIndicatorPublicationQuotes.Any(newItem => !String.IsNullOrWhiteSpace(newItem.Quote) && !String.IsNullOrWhiteSpace(originalItem.Quote) && newItem.Quote.Equals(originalItem.Quote)))
								.Select(s => s.Id)
								.ToList();


					var quotesForUpdateIds = item.Item1.AcademicRankIndicatorPublicationQuotes.Where(newItem =>
						item.Item2.AcademicRankIndicatorPublicationQuotes.Any(originalItem => !String.IsNullOrWhiteSpace(newItem.Quote) && !String.IsNullOrWhiteSpace(originalItem.Quote) && originalItem.Quote.Equals(newItem.Quote)
									&& originalItem.Score != newItem.Score))
									.Select(s => s.Id)
									.ToList();

					foreach (var quoteForAdd in item.Item1.AcademicRankIndicatorPublicationQuotes)
					{
						if (quotesForAddIds.Any(s => s == quoteForAdd.Id))
						{
							quoteForAdd.ModifiedStatus = 1; // new
							quoteForAdd.Order = counter;

							item.Item2.AcademicRankIndicatorPublicationQuotes.Add(new AcademicRankIndicatorPublicationQuote() {
								Order = counter,
								ModifiedStatus = 1
							});

							counter++;
						}
					}

					foreach (var quoteForUpdate in item.Item1.AcademicRankIndicatorPublicationQuotes)
					{
						if (quotesForUpdateIds.Any(s => s == quoteForUpdate.Id))
						{
							quoteForUpdate.ModifiedStatus = 2; // update
							quoteForUpdate.Order = counter;

							var originalItem = item.Item2.AcademicRankIndicatorPublicationQuotes
														 .FirstOrDefault(s => !String.IsNullOrWhiteSpace(s.Quote) && !String.IsNullOrWhiteSpace(quoteForUpdate.Quote) && s.Quote.Equals(quoteForUpdate.Quote));

							originalItem.Order = counter;
							originalItem.ModifiedStatus = 2;

							counter++;
						}
					}

					foreach (var quoteForDelete in item.Item2.AcademicRankIndicatorPublicationQuotes)
					{
						if (quotesForDeleteIds.Any(s => s == quoteForDelete.Id))
						{
							quoteForDelete.ModifiedStatus = 3; // delete
							quoteForDelete.Order = counter;

							item.Item1.AcademicRankIndicatorPublicationQuotes.Add(new AcademicRankIndicatorPublicationQuote() {
								Order = counter,
								ModifiedStatus = 3
							});
							counter++;
						}
					}

					foreach (var quoteForUpdateFromOriginal in item.Item2.AcademicRankIndicatorPublicationQuotes)
					{
						if (quoteForUpdateFromOriginal.ModifiedStatus == 0) // 0 is unchanged
						{
							quoteForUpdateFromOriginal.Order = counter;

							var modifiedItem = item.Item2.AcademicRankIndicatorPublicationQuotes.FirstOrDefault(s => s.Quote != null && s.Quote.Equals(quoteForUpdateFromOriginal.Quote)
							&& s.Score == quoteForUpdateFromOriginal.Score);
							modifiedItem.Order = counter;

							counter++;
						}
					}

					item.Item1.AcademicRankIndicatorPublicationQuotes = item.Item1.AcademicRankIndicatorPublicationQuotes
																				   .OrderBy(t => t.Order)
																				   .ToList();
					item.Item2.AcademicRankIndicatorPublicationQuotes = item.Item2.AcademicRankIndicatorPublicationQuotes
																				   .OrderBy(t => t.Order)
																				   .ToList();

				}

				indicatorDiff.AcademicRankIndicatorPublicationForDelete.AddRange(forDelete);

			}
			else if (original.ScientificIndicatorType.IndicatorType == IndicatorType.TextAndScore)
			{
				var forAdd = modification.AcademicRankTextAndScores.Where(newItem =>
							!original.AcademicRankTextAndScores.Any(s => !String.IsNullOrWhiteSpace(newItem.Text) && !String.IsNullOrWhiteSpace(s.Text) && newItem.Text.Equals(s.Text)))
							.ToList();

				var forDelete = original.AcademicRankTextAndScores.Where(originalItem =>
					!modification.AcademicRankTextAndScores.Any(newItem => !String.IsNullOrWhiteSpace(newItem.Text) && !String.IsNullOrWhiteSpace(originalItem.Text) && newItem.Text.Equals(originalItem.Text)))
					.ToList();

				indicatorDiff.AcademicRankTextAndScoresForAdd.AddRange(forAdd);

				foreach (var item in modification.AcademicRankTextAndScores)
				{
					if (original.AcademicRankTextAndScores.Any(s => !String.IsNullOrWhiteSpace(item.Text) && !String.IsNullOrWhiteSpace(s.Text) && s.Text.Equals(item.Text) && s.Score != item.Score))
					{
						var originalItem = original.AcademicRankTextAndScores.FirstOrDefault(t => !String.IsNullOrWhiteSpace(item.Text) && !String.IsNullOrWhiteSpace(t.Text) && t.Text.Equals(item.Text));

						indicatorDiff.AcademicRankTextAndScoresForUpdate.Add(new Tuple<AcademicRankTextAndScore, AcademicRankTextAndScore>
							(item, originalItem));

					}
				}

				indicatorDiff.AcademicRankTextAndScoresForDelete.AddRange(forDelete);
			}

			return indicatorDiff;
		}
	}

	public class CompareRankResultDto
	{
		public CommonRankInformationDto EntityInformation { get; set; }
		public List<IndicatorRankDiffsDto> Result { get; set; }
	}

	public class CommonRankInformationDto
	{
		public Institution Institution { get; set; }
		public ResearchArea ResearchArea { get; set; }
		public AcademicRankType AcademicRankType { get; set; }

	}

	public class IndicatorRankDiffsDto
	{
		public int ScientificIndicatorTypeId { get; set; }
		public string ScientificIndicatorTypeName { get; set; }
		public IndicatorType IndicatorType { get; set; }
		public decimal? OriginalScore { get; set; }
		public decimal? ModifiedScore { get; set; }
		public string OriginalTextProof { get; set; }
		public string ModifiedTextProof { get; set; }
		public List<Tuple<AcademicRankIndicatorPublication, AcademicRankIndicatorPublication>> AcademicRankIndicatorPublicationForUpdate { get; set; }
		public List<AcademicRankIndicatorPublication> AcademicRankIndicatorPublicationForAdd { get; set; }
		public List<AcademicRankIndicatorPublication> AcademicRankIndicatorPublicationForDelete { get; set; }
		public List<Tuple<AcademicRankTextAndScore, AcademicRankTextAndScore>> AcademicRankTextAndScoresForUpdate { get; set; }
		public List<AcademicRankTextAndScore> AcademicRankTextAndScoresForAdd { get; set; }
		public List<AcademicRankTextAndScore> AcademicRankTextAndScoresForDelete { get; set; }
		public IndicatorRankDiffsDto()
		{
			AcademicRankIndicatorPublicationForAdd = new List<AcademicRankIndicatorPublication>();
			AcademicRankIndicatorPublicationForUpdate = new List<Tuple<AcademicRankIndicatorPublication, AcademicRankIndicatorPublication>>();
			AcademicRankIndicatorPublicationForDelete = new List<AcademicRankIndicatorPublication>();
			AcademicRankTextAndScoresForAdd = new List<AcademicRankTextAndScore>();
			AcademicRankTextAndScoresForUpdate = new List<Tuple<AcademicRankTextAndScore, AcademicRankTextAndScore>>();
			AcademicRankTextAndScoresForDelete = new List<AcademicRankTextAndScore>();
		}
	}
}
