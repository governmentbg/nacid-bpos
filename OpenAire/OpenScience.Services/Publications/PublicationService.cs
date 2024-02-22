using Microsoft.EntityFrameworkCore;
using OpenScience.Data;
using OpenScience.Data.Publications.Models;
using OpenScience.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using OpenScience.Data.Base.Interfaces;
using OpenScience.Data.Base.Models;

namespace OpenScience.Services.Publications
{
	public class PublicationService : BaseEntityService<Publication>
	{
		public PublicationService(AppDbContext context) : base(context)
		{
		}

		public override async Task<Publication> GetByIdAsync(int id)
		{
			var item = await PrepareFilterQuery()
				.SingleOrDefaultAsync(e => e.Id == id);

			return item;
		}

		public async Task<Publication> GetPlainByIdAsync(int id)
		{
			return await context.Publications
				.AsNoTracking()
				.SingleOrDefaultAsync(e => e.Id == id);
		}

		public override Publication Update(Publication entity)
		{
			var oldClassifications = context.Set<PublicationClassification>()
				.AsNoTracking()
				.Where(i => i.PublicationId == entity.Id)
				.ToList();

			var itemsToAdd = entity.Classifications.Where(entityClassification =>
				oldClassifications.All(oldClassification =>
					oldClassification.ClassificationId != entityClassification.ClassificationId));
			foreach (var item in itemsToAdd)
			{
				context.Entry(item).State = EntityState.Added;
			}

			var itemsToRemove = oldClassifications.Where(oldClassification =>
				entity.Classifications.All(entityClassification =>
					oldClassification.ClassificationId != entityClassification.ClassificationId));
			foreach (var item in itemsToRemove)
			{
				context.Entry(item).State = EntityState.Deleted;
			}

			UpdatePublicationCollection(entity.Id, entity.Titles, (oldTitle, entityTitle) => (
				oldTitle.Value == entityTitle.Value && oldTitle.LanguageId == entityTitle.Language?.Id && oldTitle.TypeId == entityTitle.Type?.Id));

			UpdatePublicationCollection(entity.Id, entity.Languages, (oldLanguage, entityLanguage) => oldLanguage.LanguageId == entityLanguage.Language?.Id);

			UpdatePublicationCollection(entity.Id, entity.Descriptions, (oldDescription, entityDescription) => (oldDescription.Value == entityDescription.Value && oldDescription.LanguageId == entityDescription.Language?.Id));

			UpdatePublicationCollection(entity.Id, entity.Subjects, (oldSubject, entitySubject) => (
				  oldSubject.Value == entitySubject.Value && oldSubject.Language == entitySubject.Language));

			UpdatePublicationCollection(entity.Id, entity.Publishers, (oldPublisher, entityPublisher) => (
				  oldPublisher.Name == entityPublisher.Name));

			UpdatePublicationCollection(entity.Id, entity.Sources, (oldSource, entitySource) => (
				  oldSource.Value == entitySource.Value));

			UpdatePublicationCollection(entity.Id, entity.Formats, (oldFormat, entityFormat) => (
				oldFormat.Value == entityFormat.Value));

			UpdatePublicationCollection(entity.Id, entity.Sizes, (oldSize, entitySize) => (
				oldSize.Value == entitySize.Value));

			UpdatePublicationCollection(entity.Id, entity.Audiences, (oldAudience, entityAudience) => (
				oldAudience.TypeId == entityAudience.Type?.Id));

			UpdatePublicationCollection(entity.Id, entity.Coverages, (oldCoverage, entityCoverage) => (
				oldCoverage.Value == entityCoverage.Value));

			UpdatePublicationCollection(entity.Id, entity.AlternateIdentifiers, (oldAlternateIdentifier, entityAlternateIdentifier) => (
				oldAlternateIdentifier.Value == entityAlternateIdentifier.Value && oldAlternateIdentifier.TypeId == entityAlternateIdentifier.Type?.Id));

			UpdatePublicationCollection(entity.Id, entity.RelatedIdentifiers, (oldRelatedIdentifier, entityRelatedIdentifier) => (
				  oldRelatedIdentifier.Value == entityRelatedIdentifier.Value
				  && oldRelatedIdentifier.RelatedMetadataScheme == entityRelatedIdentifier.RelatedMetadataScheme
				  && oldRelatedIdentifier.RelationTypeId == entityRelatedIdentifier.RelationType?.Id
				  && oldRelatedIdentifier.ResourceTypeGeneralId == entityRelatedIdentifier.ResourceTypeGeneral?.Id
				  && oldRelatedIdentifier.TypeId == entityRelatedIdentifier.Type?.Id
				  && oldRelatedIdentifier.SchemeType == entityRelatedIdentifier.SchemeType
				  && oldRelatedIdentifier.SchemeURI == entityRelatedIdentifier.SchemeURI));

			UpdatePublicationCollection(entity.Id, entity.FundingReferences, (oldFundingReference, entityFundingReference) => (
				  oldFundingReference.Name == entityFundingReference.Name
				  && oldFundingReference.AwardNumber == entityFundingReference.AwardNumber
				  && oldFundingReference.AwardTitle == entityFundingReference.AwardTitle
				  && oldFundingReference.AwardURI == entityFundingReference.AwardURI
				  && oldFundingReference.SchemeId == entityFundingReference.Scheme?.Id));

			UpdatePublicationCollection(entity.Id, entity.Creators, (oldCreator, entityCreator) =>
				  oldCreator.DisplayName == entityCreator.DisplayName &&
				  oldCreator.Language == entityCreator.Language &&
				  oldCreator.NameType == entityCreator.NameType,
				(creator, oldCreator) => {
					UpdateEntityCollection(creator.Affiliations, oldCreator.Affiliations, (entityAf, oldAf) => entityAf.Id == oldAf.Id || entityAf.InstitutionName == oldAf.InstitutionName);
					UpdateEntityCollection(creator.Identifiers, oldCreator.Identifiers, (entityIdentifier, oldIdentifier) => entityIdentifier.Id == oldIdentifier.Id || entityIdentifier.Value == oldIdentifier.Value);
				},
				pc => pc.Identifiers,
				pc => pc.Affiliations);

			UpdatePublicationCollection(entity.Id, entity.Contributors, (oldContributor, entityContributor) => (
				 oldContributor.FirstName == entityContributor.FirstName &&
				 oldContributor.LastName == entityContributor.LastName &&
				 oldContributor.TypeId == entityContributor.Type?.Id &&
				 oldContributor.NameType == entityContributor.NameType &&
				 oldContributor.InstitutionAffiliationName == entityContributor.InstitutionAffiliationName),
				(contributor, oldContributor) => {
					UpdateEntityCollection(contributor.Identifiers, oldContributor.Identifiers, (entityIdentifier, oldIdentifier) => entityIdentifier.Id == oldIdentifier.Id || entityIdentifier.Value == oldIdentifier.Value);
				},
				pc => pc.Identifiers);

			var oldFiles = context.Set<PublicationFile>().AsNoTracking()
				.Where(i => i.PublicationId == entity.Id)
				.ToList();

			UpdateEntityCollection(entity.Files, oldFiles, (oldFile, entityFile) => (oldFile.Hash == entityFile.Hash));

			UpdatePublicationCollection(entity.Id, entity.FileLocations,
				(oldFileLocation, entityFileLocation) =>
					oldFileLocation.AccessRightsUri == entityFileLocation.AccessRightsUri &&
					oldFileLocation.FileUrl == entityFileLocation.FileUrl &&
					oldFileLocation.MimeType == entityFileLocation.MimeType &&
					oldFileLocation.ObjectType == entityFileLocation.ObjectType);

			entity.ModificationDate = DateTime.UtcNow;
			context.Entry(entity).State = EntityState.Modified;

			return entity;
		}

		public Publication UpdatePlain(Publication publication)
		{
			publication.ModificationDate = DateTime.UtcNow;

			return base.Update(publication);
		}

		public override IQueryable<Publication> PrepareFilterQuery()
		{
			return base.PrepareFilterQuery()
				.Include(p => p.Classifications)
					.ThenInclude(p => p.Classification)
				.Include(p => p.ResourceType)
				.Include(p => p.Titles)
					.ThenInclude(t => t.Type)
				.Include(p => p.Titles)
					.ThenInclude(t => t.Language)
				.Include(p => p.Creators)
					.ThenInclude(c => c.Identifiers)
						.ThenInclude(i => i.Scheme)
				.Include(p => p.Creators)
					.ThenInclude(c => c.Affiliations)
				.Include(p => p.Contributors)
					.ThenInclude(c => c.Type)
				.Include(p => p.Contributors)
					.ThenInclude(c => c.Identifiers)
						.ThenInclude(i => i.Scheme)
				.Include(p => p.Contributors)
					.ThenInclude(c => c.Identifiers)
						.ThenInclude(i => i.OrganizationalScheme)
				.Include(p => p.Descriptions)
					.ThenInclude(d => d.Language)
				.Include(p => p.Languages)
					.ThenInclude(l => l.Language)
				.Include(p => p.IdentifierType)
				.Include(p => p.Subjects)
					.ThenInclude(s => s.Language)
				.Include(p => p.FundingReferences)
					.ThenInclude(f => f.Scheme)
				.Include(p => p.RelatedIdentifiers)
					.ThenInclude(ri => ri.Type)
				.Include(p => p.RelatedIdentifiers)
					.ThenInclude(ri => ri.RelationType)
				.Include(p => p.RelatedIdentifiers)
					.ThenInclude(ri => ri.ResourceTypeGeneral)
				.Include(p => p.AlternateIdentifiers)
					.ThenInclude(ai => ai.Type)
				.Include(p => p.Sources)
				.Include(p => p.Publishers)
				.Include(p => p.Coverages)
				.Include(p => p.Audiences)
					.ThenInclude(a => a.Type)
				.Include(p => p.AccessRight)
				.Include(p => p.LicenseType)
				.Include(p => p.Files)
				.Include(p => p.Formats)
				.Include(p => p.OriginDescription)
				.Include(p => p.ModeratorInstitution);
		}

		private void UpdatePublicationCollection<TPublicationEntity>(int publicationId,
			ICollection<TPublicationEntity> entityItems,
			Func<TPublicationEntity, TPublicationEntity, bool> equalityComparisonFunc,
			Action<TPublicationEntity, TPublicationEntity> updateEntityFunc = null,
			params Expression<Func<TPublicationEntity, object>>[] includes)
				where TPublicationEntity : PublicationEntity
		{
			var oldItems = includes.Aggregate(context.Set<TPublicationEntity>().AsNoTracking(),
					(current, include) => current.Include(include))
				.Where(i => i.PublicationId == publicationId)
				.ToList();

			UpdateEntityCollection(entityItems, oldItems, equalityComparisonFunc, updateEntityFunc);
		}

		private void UpdateEntityCollection<TEntity>(ICollection<TEntity> entityItems,
			ICollection<TEntity> oldItems,
			Func<TEntity, TEntity, bool> equalityComparisonFunc,
			Action<TEntity, TEntity> updateEntityFunc = null)
			where TEntity : class, IEntity
		{
			var itemsToAdd = entityItems.Where(entityItem =>
				oldItems.All(oldItem => oldItem.Id != entityItem.Id || !equalityComparisonFunc.Invoke(oldItem, entityItem))).ToList();
			foreach (var item in itemsToAdd)
			{
				context.Entry(item).State = EntityState.Added;
			}

			var itemsToRemove = oldItems.Where(oldItem =>
				entityItems.All(entityItem => oldItem.Id != entityItem.Id || !equalityComparisonFunc.Invoke(oldItem, entityItem))).ToList();
			foreach (var item in itemsToRemove)
			{
				context.Entry(item).State = EntityState.Deleted;
			}

			var itemsToUpdate = entityItems.Except(itemsToAdd);
			foreach (var item in itemsToUpdate)
			{
				context.Entry(item).State = EntityState.Modified;

				if (updateEntityFunc != null)
				{
					var oldItem = oldItems.Single(oi => oi.Id == item.Id || equalityComparisonFunc.Invoke(oi, item));
					updateEntityFunc.Invoke(item, oldItem);
				}
			}
		}
	}
}
