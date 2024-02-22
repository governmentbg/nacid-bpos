using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using NacidRas.Competitions.Models;
using NacidRas.Ems.Models;
using NacidRas.GroupModifications.Models;
using NacidRas.Infrastructure.Data;
using NacidRas.Infrastructure.Emails.Models;
using NacidRas.Ras;
using NacidRas.Ras.AdministrativePositions;
using NacidRas.Ras.AssignmentPositions;
using NacidRas.Ras.BaseNomenclatures;
using NacidRas.Ras.Files;
using NacidRas.Ras.Nomenclatures.Models;
using NacidRas.RasRegister;
using NacidRas.RasRegister.Models;
using NacidRas.Register;
using NacidRas.Users.Models;
using NacidRis.Portal.Application.Models;

namespace NacidRas
{
	public class RasDbContext : DbContext
	{
		public RasDbContext(DbContextOptions<RasDbContext> options)
			: base(options)
		{
		}

		public DbSet<District> Districts { get; set; }
		public DbSet<Municipality> Municipalities { get; set; }
		public DbSet<Settlement> Settlement { get; set; }

		public DbSet<Email> Emails { get; set; }
		public DbSet<EmailTemplate> EmailTemplates { get; set; }

		public DbSet<AcademicDegreeType> AcademicDegreeTypes { get; set; }
		public DbSet<AcademicRankType> AcademicRankTypes { get; set; }
		public DbSet<ResearchArea> ResearchAreas { get; set; }
		public DbSet<Language> Languages { get; set; }
		public DbSet<Country> Countries { get; set; }
		public DbSet<Institution> Institutions { get; set; }
		public DbSet<ScientificIndicatorType> ScientificIndicatorTypes { get; set; }
		public DbSet<CommitModificationReason> CommitModificationReasons { get; set; }

		public DbSet<GroupModification> GroupModifications { get; set; }
		public DbSet<GroupModificationNote> GroupModificationNotes { get; set; }
		public DbSet<ModificationRequest> ModificationRequests { get; set; }

		public DbSet<AcademicDegreeIndicatorGroupNameTemplate> AcademicDegreeIndicatorGroupNameTemplates { get; set; }
		public DbSet<AcademicRankIndicatorGroupNameTemplate> AcademicRankIndicatorGroupNameTemplates { get; set; }
		public DbSet<AcademicDegreeIndicatorGroupName> AcademicDegreeIndicatorGroupNames { get; set; }
		public DbSet<AcademicDegreeIndicatorTotal> AcademicDegreeIndicatorTotals { get; set; }
		public DbSet<AcademicRankIndicatorTotal> AcademicRankIndicatorTotals { get; set; }

		public DbSet<RasLot> RasLot { get; set; }
		public DbSet<RasLotUser> RasLotUser { get; set; }
		public DbSet<RasCommit> RasCommit { get; set; }
		public DbSet<PersonPart> PersonParts { get; set; }
		public DbSet<Person> Persons { get; set; }
		public DbSet<PersonIdn> PersonIdns { get; set; }
		public DbSet<AcademicDegreePart> AcademicDegreeParts { get; set; }
		public DbSet<AcademicDegree> AcademicDegrees { get; set; }
		public DbSet<Dissertation> Dissertations { get; set; }

		public DbSet<EmploymentContractFile> EmploymentContractFiles { get; set; }
		public DbSet<DiplomaFile> DiplomaFiles { get; set; }
		public DbSet<AcademicDegreeFile> AcademicDegreeFiles { get; set; }
		public DbSet<DissertationFile> DissertationFiles { get; set; }
		public DbSet<SummaryFile> SummaryFiles { get; set; }
		public DbSet<CertificateFile> CertificateFiles { get; set; }
		public DbSet<AcademicRankFile> AcademicRankFiles { get; set; }

		public DbSet<AcademicRankPart> AcademicRankParts { get; set; }
		public DbSet<AcademicRank> AcademicRanks { get; set; }

		public DbSet<AssignmentPositionPart> AssignmentPositionParts { get; set; }
		public DbSet<AssignmentPosition> AssignmentPositions { get; set; }
		public DbSet<AssignmentPositionSpeciality> AssignmentPositionSpecialities { get; set; }
		public DbSet<ContractType> ContractTypes { get; set; }
		public DbSet<Position> Positions { get; set; }
		public DbSet<Speciality> Specialities { get; set; }

		public DbSet<AdministrativePositionPart> AdministrativePositionParts { get; set; }
		public DbSet<AdministrativePosition> AdministrativePositions { get; set; }

		public DbSet<Competition> Competitions { get; set; }
		public DbSet<CompetitionAnnouncement> CompetitionAnnouncements { get; set; }

		public DbSet<ApplicationWithCode> ApplicationsWithCode { get; set; }
		public DbSet<ApplicationCaseLink> ApplicationCaseLinks { get; set; }

		public IDbContextTransaction BeginTransaction()
		{
			return Database.BeginTransaction();
		}

		public override int SaveChanges()
		{
			IncrementIConcurrencyEntitiesVersion();

			try
			{
				return base.SaveChanges();
			}
			catch (DbUpdateConcurrencyException e)
			{
				throw;
			}
		}

		public void SetValues(IEntityVersion original, IEntityVersion entity)
			//where T : IEntity, IVersion
		{
			entity.Id = original.Id;

			/* ей тука като копира файл обекта, той все още не е създаден, а SetValues
			* копира само прости типове, а файла е референтен и още не е добавен и няма ид
			* и се копира съответно нищо...
			 */
			//Entry(original).CurrentValues.SetValues(entity);


			Entry(original).OriginalValues["Version"] = entity.Version; 
		}

		//public void MarkUnchanged(object entity)
		//{
		//	foreach (var navigationProperty in Entry(entity).Metadata.GetNavigations())
		//	{
		//		var type = entity.GetType().GetProperty(navigationProperty.Name);
		//		var removeEntity = type.GetValue(entity, null);
		//		if (removeEntity != null)
		//		{
		//			if (removeEntity.GetType().GetInterface("IList", true) != null)
		//			{
		//				foreach (var item in (IEnumerable)removeEntity)
		//				{
		//					MarkUnchanged(item);
		//				}
		//			}
		//			else
		//			{
		//				Entry(removeEntity).State = EntityState.Unchanged;
		//				MarkUnchanged(removeEntity);
		//			}
		//		}
		//	}
		//}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			ApplyConfigurations(modelBuilder);
			DisableCascadeDelete(modelBuilder);
			ConfigurePgSqlNameMappings(modelBuilder);
			ConfigureOptimisticConcurrencyToken(modelBuilder);
		}

		protected void ApplyConfigurations(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new RasLotConfiguration());
			modelBuilder.ApplyConfiguration(new RasLotUserConfiguration());
			modelBuilder.ApplyConfiguration(new RasCommitConfiguration());

			modelBuilder.ApplyConfiguration(new PersonPartConfiguration());
			modelBuilder.ApplyConfiguration(new AcademicDegreePartConfiguration());
			modelBuilder.ApplyConfiguration(new AcademicDegreeConfiguration());
			modelBuilder.ApplyConfiguration(new AcademicRankPartConfiguration());
			modelBuilder.ApplyConfiguration(new AssignmentPositionPartConfiguration());
			modelBuilder.ApplyConfiguration(new AdministrativePositionPartConfiguration());

			modelBuilder.ApplyConfiguration(new GroupModificationConfiguration());
			modelBuilder.ApplyConfiguration(new ModificationRequestConfiguration());

			modelBuilder.ApplyConfiguration(new InstitutionConfiguration());
		}

		protected void DisableCascadeDelete(ModelBuilder modelBuilder)
		{
			modelBuilder.Model.GetEntityTypes()
				.SelectMany(t => t.GetForeignKeys())
				.Where(fk => !fk.IsOwnership
					&& fk.DeleteBehavior == DeleteBehavior.Cascade)
				.ToList()
				.ForEach(e => e.DeleteBehavior = DeleteBehavior.Restrict);
		}

		protected void ConfigurePgSqlNameMappings(ModelBuilder modelBuilder)
		{
			foreach (var entity in modelBuilder.Model.GetEntityTypes())
			{
				// Configure pgsql table names convention.
				entity.Relational().TableName = entity.ClrType.Name.ToLower();

				// Configure pgsql column names convention.
				foreach (var property in entity.GetProperties())
					property.Relational().ColumnName = property.Name.ToLower();
			}
		}

		protected void ConfigureOptimisticConcurrencyToken(ModelBuilder modelBuilder)
		{
			foreach (var entity in modelBuilder.Model.GetEntityTypes())
			{
				var isIVersion = typeof(IVersion).IsAssignableFrom(entity.ClrType);
				if (isIVersion)
					modelBuilder.Entity(entity.ClrType).Property("Version")
						.IsConcurrencyToken();
			}
		}

		public List<object> GetAllChanges() =>
			ChangeTracker
				.Entries()
				.Where(x => x.State == EntityState.Modified
					|| x.State == EntityState.Added
					|| x.State == EntityState.Deleted)
				.Select(x => new {
					Type = x.Entity.GetType(),
					x.State,
					Entry = x,
					ModifiedProperties = x.Properties.Where(t => t.IsModified)
				})
				.ToList<object>();

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.EnableSensitiveDataLogging(true);

			optionsBuilder
					// QueryClientEvaluation is threated as error.
					.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
		}

		private void IncrementIConcurrencyEntitiesVersion()
		{
			foreach (var dbEntityEntry in ChangeTracker.Entries()
				.Where(x => x.State == EntityState.Modified))
			{
				if (dbEntityEntry.Entity is IVersion entity)
				{
					entity.Version = entity.Version + 1;
				}
			}
		}
	}
}
