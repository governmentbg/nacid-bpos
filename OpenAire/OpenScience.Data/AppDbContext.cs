using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OpenScience.Data.Base.Interfaces;
using OpenScience.Data.Classifications.Models;
using OpenScience.Data.Emails.Models;
using OpenScience.Data.Institutions.Models;
using OpenScience.Data.Nomenclatures.Models;
using OpenScience.Data.Publications.Models;
using OpenScience.Data.Users.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace OpenScience.Data
{
	public class AppDbContext : DbContext
	{

		#region Classifications

		public DbSet<Classification> Classifications { get; set; }
		public DbSet<ClassificationClosure> ClassificationClosures { get; set; }

		#endregion

		#region Insititutions

		public DbSet<Institution> Institutions { get; set; }

		#endregion

		#region Publications

		public DbSet<Publication> Publications { get; set; }
		public DbSet<PublicationClassification> PublicationClassifications { get; set; }
		public DbSet<PublicationTitle> PublicationTitles { get; set; }
		public DbSet<PublicationCreator> PublicationCreators { get; set; }
		public DbSet<PublicationCreatorIdentifier> PublicationCreatorIdentifiers { get; set; }
		public DbSet<PublicationContributor> PublicationContributors { get; set; }
		public DbSet<PublicationFundingReference> PublicationFundingReferences { get; set; }
		public DbSet<PublicationLanguage> PublicationLanguages { get; set; }
		public DbSet<PublicationPublisher> PublicationPublishers { get; set; }
		public DbSet<PublicationDescription> PublicationDescriptions { get; set; }
		public DbSet<PublicationFormat> PublicationFormats { get; set; }
		public DbSet<PublicationSubject> PublicationSubjects { get; set; }
		public DbSet<PublicationSize> PublicationSizes { get; set; }
		public DbSet<PublicationDate> PublicationDates { get; set; }
		public DbSet<PublicationCoverage> PublicationCoverages { get; set; }
		public DbSet<PublicationOriginDescription> PublicationOriginDescriptions { get; set; }
		public DbSet<PublicationFile> PublicationFiles { get; set; }
		public DbSet<PublicationFileLocation> PublicationFileLocations { get; set; }

		#endregion

		#region Users

		public DbSet<Role> Roles { get; set; }
		public DbSet<Permission> Permissions { get; set; }
		public DbSet<RolePermission> RolePermissions { get; set; }

		public DbSet<User> Users { get; set; }
		public DbSet<UserClassification> UserClassifications { get; set; }
		public DbSet<UserRole> UserRoles { get; set; }



		#endregion

		#region Nomenclatures

		public DbSet<ResourceType> ResourceTypes { get; set; }
		public DbSet<ResourceTypeGeneral> GeneralResourceTypes { get; set; }
		public DbSet<TitleType> TitleTypes { get; set; }
		public DbSet<Language> Languages { get; set; }
		public DbSet<ContributorType> ContributorTypes { get; set; }
		public DbSet<LicenseType> LicenseTypes { get; set; }
		public DbSet<AccessRight> AccessRights { get; set; }
		public DbSet<NameIdentifierScheme> NameIdentifierSchemes { get; set; }
		public DbSet<OrganizationalIdentifierScheme> OrganizationalIdentifierSchemes { get; set; }
		public DbSet<EmailType> EmailTypes { get; set; }

		#endregion

		#region Emails
		public DbSet<Email> Emails { get; set; }
		public DbSet<PasswordToken> PasswordTokens { get; set; }
		#endregion

		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{ }

		public IDbContextTransaction BeginTransaction()
		{
			return Database.BeginTransaction();
		}

		public override int SaveChanges()
		{
			IncrementIConcurrencyEntitiesVersion();

			var entities = ChangeTracker.Entries()
				.Where(e => e.State == EntityState.Modified || e.State == EntityState.Added)
				.Select(e => e.Entity)
				.ToList();

			var validationResults = new List<ValidationResult>();
			foreach (var entity in entities)
			{
				if (!Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults))
				{
					throw new ValidationException();
				}
			}

			return base.SaveChanges();
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			IncrementIConcurrencyEntitiesVersion();

			return await base.SaveChangesAsync();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			foreach (var entity in modelBuilder.Model.GetEntityTypes())
			{
				// Configure name mappings
				entity.Relational().TableName = entity.ClrType.Name.ToLower();

				// Configure optimistic concurrency token
				if (typeof(IConcurrency).IsAssignableFrom(entity.ClrType))
				{
					modelBuilder.Entity(entity.ClrType)
						.Property(nameof(IConcurrency.Version))
						.IsConcurrencyToken();
				}

				if (typeof(IEntity).IsAssignableFrom(entity.ClrType))
				{
					modelBuilder.Entity(entity.ClrType)
						.HasKey(nameof(IEntity.Id));
				}

				// Configure column names
				foreach (var property in entity.GetProperties())
				{
					property.Relational().ColumnName = property.Name.ToLower();
				}

				// Disable cascade delete
				modelBuilder.Model.GetEntityTypes()
					.SelectMany(t => t.GetForeignKeys())
					.Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
					.ToList()
					.ForEach(e => e.DeleteBehavior = DeleteBehavior.Restrict);

				// Configure all EntityTypeConfiguration
				var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
					.Where(t => t.GetInterfaces().Any(gi => gi.IsGenericType && gi.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
					.ToList();
				foreach (var type in typesToRegister)
				{
					dynamic configurationInstance = Activator.CreateInstance(type);
					modelBuilder.ApplyConfiguration(configurationInstance);
				}
			}
		}

		private void IncrementIConcurrencyEntitiesVersion()
		{
			foreach (var dbEntityEntry in ChangeTracker.Entries()
				.Where(x => x.State == EntityState.Modified))
			{
				var entity = dbEntityEntry.Entity as IConcurrency;
				if (entity != null)
				{
					entity.IncrementVersion();
				}
			}
		}
	}
}
